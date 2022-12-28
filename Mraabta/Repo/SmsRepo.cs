using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Dapper;

namespace MRaabta.Repo
{
    public class SmsRepo
    {
        SqlConnection con;
        HttpClient client;
        public SmsRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
        public async Task<List<dynamic>> GetBulkSMS()
        {
            try
            {
                var query = $@"select ConsignmentNumber as CN, MessageID as Id, replace(trim(TRANSLATE(isnull(recepient,0),'- +E.','     ')), ' ', '') as PhoneNo, MessageContent as Msg from MnP_SmsStatus
                                where CreatedOn >= '{DateTime.Now.AddMinutes(-15).ToString("yyyy-MM-dd HH:mm:ss")}' and len(replace(trim(TRANSLATE(isnull(recepient,0),'- +E.','     ')), ' ', '')) between 10 and 13 and [STATUS] = 0;";
                await con.OpenAsync();
                var rs = await con.QueryAsync(query, commandTimeout: int.MaxValue);
                Close();
                return rs.ToList();
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public async Task<(string exeption, XDocument doc)> SendSms(string phoneno, string msg)
        {
            try
            {
                //var response = await client.GetAsync($"https://pullsms.its.com.pk/api/?username=OCS-82660&password=Pakistan321&receiver={phoneno}&msgdata={HttpUtility.UrlEncode(msg)}");
                var response = await client.GetAsync($"https://gateway.its.com.pk/api?action=sendmessage&username=MP_Express&password=mPty5a!701&recipient={phoneno}&originator=82660&messagedata={HttpUtility.UrlEncode(msg)}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return (null, XDocument.Parse(await response.Content.ReadAsStringAsync()));
                }
                else
                {
                    return (null, null);
                }
            }
            catch (SqlException ex)
            {
                return (ex.Message, null);
            }
            catch (Exception ex)
            {
                return (ex.Message, null);
            }
        }
        public async Task<bool> Log(string query, string path)
        {
            path = path.Replace("Quartz", "SMSLogQueries");
            try
            {
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue);
                Close();
                return rs > 0;
            }
            catch (SqlException ex)
            {
                Close();
                File.AppendAllText(path, $"Exeption Occred {ex.Message}\n" + query);
                return false;
            }
            catch (Exception ex)
            {
                Close();
                File.AppendAllText(path, $"Exeption Occred {ex.Message}\n" + query);
                return false;
            }
        }
        public void DisposeClient()
        {
            client.Dispose();
        }
    }
}