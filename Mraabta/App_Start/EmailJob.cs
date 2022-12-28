using MRaabta.Repo;
using MRaabta.Util;
using OfficeOpenXml;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRaabta.App_Start
{
    public class EmailJob : IJob
    {
        EmailRepo repo;
        public EmailJob()
        {
            repo = new EmailRepo();
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string path = dataMap.GetString("path");
            try
            {

                File.AppendAllText(path, "Email Job Started at " + DateTime.Now.ToString() + "\n");

                await repo.OpenAsync();

                var rs = await repo.GetData();

                foreach (var item in rs)
                {
                    string to = item.To;
                    string cc = item.CC;
                    string subject = item.Subject;
                    string body = item.Body;

                    List<string> tos = to.Split(',').ToList();
                    List<string> ccs = cc?.Split(',')?.ToList();

                    MemoryStream ms = new MemoryStream();
                    
                    if (item.IsTable)
                    {
                        DataTable dt = repo.GetQueryData(item.Query);

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage pck = new ExcelPackage())
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Data");
                            ws.Cells["A1"].LoadFromDataTable(dt, true);
                            pck.SaveAs(ms);
                            ms.Position = 0;
                        }
                    }

                    Email.SendEmail(tos, ccs, subject, body,ms);
                    ms.Dispose();
                    await repo.UpdateEmail(item.Id);
                }

                repo.Close();
            }
            catch (Exception ex)
            {
                repo.Close();
                File.AppendAllText(path, "Exception occured at " + DateTime.Now.ToString() + " " + ex.Message + "\n");
            }
        }
    }
}