using MRaabta.Repo;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.App_Start
{
    public class SMSJob : IJob
    {
        SmsRepo repo;
        public SMSJob()
        {
            repo = new SmsRepo();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string path = dataMap.GetString("path");
            try
            {
                File.AppendAllText(path, "SMS Job Started at " + DateTime.Now.ToString() + "\n");
                var rs = await repo.GetBulkSMS();
                StringBuilder sb = new StringBuilder();
                foreach (var item in rs)
                {
                    string phoneNo = item.PhoneNo.ToString();
                    string msg = item.Msg.ToString();
                    var apiresponse = await repo.SendSms(phoneNo, msg);
                    if (apiresponse.exeption == null)
                    {
                        var xml = apiresponse.doc;
                        if (xml != null)
                        {
                            var data = xml.Root.Element("data");
                            var acceptreport = data?.Element("acceptreport");
                            var messageid = acceptreport?.Element("messageid")?.Value ?? "0";
                            var errorno = acceptreport?.Element("statuscode")?.Value ?? data?.Element("errorcode")?.Value;
                            var statusmessage = acceptreport?.Element("statusmessage")?.Value ?? data?.Element("errormessage")?.Value;

                            sb.AppendLine($@"update MnP_SmsStatus set STATUS = '1', ModifiedOn = getdate(), ModifiedBy = 'Schedular', responseId = {(messageid)}, ErrorCode = {errorno}, Error = '{statusmessage}' where MessageID = {item.Id};
                                        insert into SmsLogs (CN,PhoneNo,Msg,MsgId,StatusCode,Status,CreatedBy)
                                        values('{item.CN ?? ""}','{item.PhoneNo}','{item.Msg}',{messageid},{errorno},'{statusmessage}',0);");

                        }
                        else
                        {
                            sb.AppendLine($@"update MnP_SmsStatus set STATUS = '1', ModifiedOn = getdate(), ModifiedBy = 'Schedular', responseId = 0, ErrorCode = 0, Error = 'Response is null' where MessageID = {item.Id};
                                        insert into SmsLogs (CN,PhoneNo,Msg,MsgId,StatusCode,Status,CreatedBy)
                                        values('{item.CN ?? ""}','{item.PhoneNo}','{item.Msg}', 0, 0,'Response is null',0);");
                        }
                    }
                    else
                    {
                        sb.AppendLine($@"update MnP_SmsStatus set STATUS = '1', ModifiedOn = getdate(), ModifiedBy = 'Schedular', responseId = 0, ErrorCode = 0, Error = '{apiresponse.exeption}' where MessageID = {item.Id};
                                        insert into SmsLogs (CN,PhoneNo,Msg,MsgId,StatusCode,Status,CreatedBy)
                                        values('{item.CN ?? ""}','{item.PhoneNo}','{item.Msg}', 0, 0,'{apiresponse.exeption}',0);");
                    }
                }

                repo.DisposeClient();
                await repo.Log(sb.ToString(), path);
            }
            catch (Exception ex)
            {
                repo.Close();
                File.AppendAllText(path, "Exception occured at " + DateTime.Now.ToString() + " " + ex.Message + "\n");
            }
        }
    }
}