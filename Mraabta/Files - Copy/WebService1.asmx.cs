using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Services;

namespace MRaabta.Files
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCustomers(string prefix)
        {
            List<string> customers = new List<string>();
            using (
                SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                // string constr = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString(); // connection string

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select cast(name as varchar(100) ) +'  ('+cast(accountNo as varchar(100) )+')' as ContactName, id as CustomerId from CreditClients where BranchCode='" + HttpContext.Current.Session["BranchCode"].ToString() + "' and " +
                   "name like  @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using ( SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(string.Format("{0}-{1}", sdr["ContactName"], sdr["CustomerId"]));
                        }
                    }
                    conn.Close();
                }
                return customers.ToArray();
            }
        }
    }
}
