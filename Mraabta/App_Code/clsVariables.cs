using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Globalization;

namespace MRaabta.App_Code
{
    public class clsVariables
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public static int LastNewOrders { get; set; }
        public static int LastdeliveredOrders { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Unique { get; set; }

        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format(" {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format(" {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format(" {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format(" {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format(" {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format(" {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }
        public static string GetRandomString(int length)
        {
            var randomString = "";
            var r = new Random();

            for (int i = 0; i < length; i++)
            {
                var charcode = (char)r.Next(48, 98);

                randomString += charcode.ToString();

            }

            return randomString;

        }
        public static void SendMail(string toAddress, string subject, string body)
        {

            const string SmtpServer = "smtp.live.com";
            const int SmtpPort = 587;
            const string FromAddress = "tungaresturant@outlook.com";
            const string Password = "ABC123456";

            var client = new SmtpClient(SmtpServer, SmtpPort)
            {
                Credentials = new NetworkCredential(FromAddress, Password),
                EnableSsl = true
            };

            client.Send(FromAddress, toAddress, subject, body);

        }

        public string LoginCheck()
        {
            if (HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserName"].ToString() != "")
            {
                UserName = HttpContext.Current.Session["UserName"].ToString();
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/login");
            }

            return UserName;
        }

        public string GetDate(string Date)
        {
            String MyString = Date; // get value from text field
            DateTime MyDateTime = new DateTime();
            MyDateTime = DateTime.ParseExact(MyString, "dd/MM/yyyy", null); //16-07-2016
            String MyString_new = MyDateTime.ToString("dd/MM/yyyy");

            return MyString_new;// returns 09/25/2011
        }
        public DataTable DataFill(string sqlString)
        {
            string query = sqlString;

            using (SqlConnection Xcon = new SqlConnection(ConnectionString))
            {

                Xcon.Open();
                SqlDataAdapter adp = new SqlDataAdapter(query, Xcon);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Xcon.Close();
                adp.Dispose();
                return dt;
            }


        }

        public string InsertRecord(string sqlString)
        {
            string msg;
            try
            {
                using (SqlConnection Xcon = new SqlConnection(ConnectionString))
                {
                    Xcon.Open();
                    SqlCommand CMD = new SqlCommand(sqlString, Xcon);
                    CMD.ExecuteNonQuery();
                    CMD.Dispose();
                    Xcon.Close();
                    msg = "Added Succesfully ";

                }
            }
            catch (Exception ex)
            {

                msg = "Added Unsuccesfully. The following error occured: " + ex.Message; ;
            }
            return msg;

        }

    }
}