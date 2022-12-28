using MRaabta.App_Code;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class CODPickupReport : System.Web.UI.Page
    { 
        static SqlConnection conStatic = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["U_NAME"].ToString();
                Session["U_ID"].ToString();
                Session["ZONECODE"].ToString();
                Session["BRANCHCODE"].ToString();
                Session["PROFILE"].ToString();
            }
            catch (Exception er)
            {
                Response.Redirect("~/Login");
            }

            try
            {
                if (Request.QueryString["xy"] != null)
                {
                    string url = Request.QueryString["xy"].ToString();
                    string deccryptedString = "";

                    url = url.Replace(" ", "+");
                    byte[] key = { };// Key
                    byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
                    byte[] inputByteArray = new byte[url.Length];

                    key = Encoding.UTF8.GetBytes("mnp@123+");
                    DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(url);
                    MemoryStream Objmst = new MemoryStream();
                    CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                    Objcs.Write(inputByteArray, 0, inputByteArray.Length);
                    Objcs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;

                    deccryptedString = encoding.GetString(Objmst.ToArray());

                    string[] arr = deccryptedString.Split('&');
                    CODPickupSearch dataa = new CODPickupSearch();
                    dataa.startDate = arr[0].Split('=').Last();
                    dataa.endDate = arr[1].Split('=').Last();
                    dataa.Zone = arr[2].Split('=').Last();
                    dataa.Account = arr[3].Split('=').Last();
                    dataa.Status = arr[4].Split('=').Last();

                    ExportToExcel(dataa);
                }
            }
            catch (Exception er)
            {
                //Response.Redirect("");
            }
        }

        protected void ExportToExcel(CODPickupSearch fields)
        {
            try
            {
                DataTable Year_check  =GetMainData(fields.startDate, fields.endDate, fields.Zone, fields.Account, fields.Account);

                if (Year_check.Rows.Count > 0)
                {
                    Response.Clear();

                    Response.Buffer = true;
                    Response.AddHeader("content-disposition",
                     "attachment;filename=CODPickupReport.csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";

                    string type = Request.QueryString["type"];
                     

                    StringBuilder sb = new StringBuilder();
                    for (int k = 0; k < Year_check .Columns.Count; k++)
                    {
                        //add separator
                        sb.Append(Year_check .Columns[k].ColumnName.ToString() + ',');
                    }
                    //append new line
                    sb.Append("\r\n");
                    for (int i = 0; i < Year_check .Rows.Count; i++)
                    {
                        for (int k = 0; k < Year_check .Columns.Count; k++)
                        {
                            if (Year_check .Rows[i][k].ToString() == "" || Year_check .Rows[i][k].ToString() == "&nbsp;" || Year_check.Rows[i][k].ToString() == null)
                            {
                                if (Year_check .Rows[i][0].ToString() == "")
                                {
                                    string data = null;

                                    data = Year_check .Rows[i + 1][0].ToString();
                                    sb.Append(data + ',');
                                }
                                else
                                {
                                    sb.Append("" + ',');
                                }
                            }
                            else
                            {

                                string data = null;
                                data = Year_check .Rows[i][k].ToString().Trim();
                                data = Regex.Replace(data, @"&#39;", @"'").ToString();
                                data = String.Format("\"{0}\"", data);

                                sb.Append(data + ',');
                            }
                        }
                        //append new line
                        sb.Append("\r\n");
                    }
                     Response.Output.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception er)
            {
            }
            finally
            {
            }
        }

        [WebMethod]
        public static string Search_CSV_Click(string DateStart, string DateEnd, string Zone, string Account, string Status, string OutputType)
        {
            try
            {
                string zoneQuery = "";
                string buuildValues = "DateStart=" + DateStart + "&DateEnd=" + DateEnd + "&zone=" + Zone + "&Account=" + Account + "&Status=" + Status;
                byte[] key = { }; //Encryption Key
                byte[] IV = { 30, 20, 30, 40, 50, 60, 70, 80 };
                byte[] inputByteArray;
                key = Encoding.UTF8.GetBytes("mnp@123+");
                DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
                inputByteArray = Encoding.UTF8.GetBytes(buuildValues);
                MemoryStream Objmst = new MemoryStream();
                CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                Objcs.Write(inputByteArray, 0, inputByteArray.Length);
                Objcs.FlushFinalBlock();
                string encryp = Convert.ToBase64String(Objmst.ToArray());//encrypted string
                string redirectUrl = "COD_PickupReport.aspx?xy=" + encryp;
                return redirectUrl;
            }
            catch (Exception er)
            {
                return "COD_PickupReport.aspx";
            }
        }

        [WebMethod]
        public static CODPickupReportModel Search_Click(string DateStart, string DateEnd, string Zone, string Account, string Status, string OutputType)
        {
            CODPickupReportModel resp = new CODPickupReportModel();
            DataTable ds = new DataTable();
            try
            {
                DateTime dtStart = DateTime.Parse(DateStart);
                DateTime dtEnd = DateTime.Parse(DateEnd);
                 

                List<CODPickupResponse> list = new List<CODPickupResponse>();
                ds  = GetMainData(dtStart.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"), Zone, Account, Status);
                int serialNo = 0;
                for (int j=0;j<ds.Rows.Count;j++)
                {
                    serialNo++;
                    list.Add(new CODPickupResponse
                    {
                        serial = serialNo.ToString(),
                        consignerAccountNo = ds.Rows[j]["consignerAccountNo"].ToString(),
                        consigner = ds.Rows[j]["consigner"].ToString(),
                        Zone = ds.Rows[j]["Zone"].ToString(),
                        branch = ds.Rows[j]["branch"].ToString(),
                        LocationName = ds.Rows[j]["LocationName"].ToString(),
                        CreatedDate = ds.Rows[j]["CreatedDate"].ToString(),
                        BookedCount = ds.Rows[j]["BookedCount"].ToString(),
                        LoadsheetCount= ds.Rows[j]["LoadsheetCount"].ToString(),
                        ArrivalCount = ds.Rows[j]["ArrivalCount"].ToString(),
                        ManifestCount= ds.Rows[j]["ManifestCount"].ToString(),
                        BaggingCount= ds.Rows[j]["BaggingCount"].ToString(),
                        LoadingCount = ds.Rows[j]["LoadingCount"].ToString(),
                        STATUS = ds.Rows[j]["STATUS"].ToString(),
                    });
                }

                if (OutputType.ToUpper() == "HTML")
                {
                    resp.status = true;
                    resp.Message = "Success";
                    resp.CODTable = list;
                }
            }
            catch (Exception er)
            {
                resp.Message = er.Message.ToString();
                resp.status = false;

            }
            return resp;
        }

        public static DataTable GetMainData(string dtStart, string dtEnd, string ZoneID, string Accounts, string statusSelected)
        {
            DataTable dt = new DataTable();
            try
            {
                string queryCondition1 = "";
                string queryCondition2 = "";
                string queryCondition3 = "";
                if (ZoneID == "" || ZoneID.ToUpper() == "ALL")
                { }
                else {  
                    queryCondition1 = " AND z.zoneCode IN (" + ZoneID + ") ";
                }
                if (Accounts == "" || Accounts.ToUpper() == "ALL")
                {}
                else { 
                    queryCondition2 = " AND c.consignerAccountNo IN(" + Accounts + ") ";
                }

                if (statusSelected == "1")
                {
                    queryCondition3 = "  AND c.isapproved <> '1' -- 'Booked' ";
                }

                if (statusSelected == "2")
                {
                    queryCondition3 = "  AND c.isapproved = '1' -- 'Picked' ";
                }
                if (statusSelected == "4")
                {
                    queryCondition3 = " AND rc.time IS NOT NULL -- Delivered ";
                }

                string sql = @"  
                SELECT TOP(20) 
                       CAST(isnull(c.consignerAccountNo,'') AS VARCHAR) consignerAccountNo,
                       CAST(isnull(c.consigner,'') AS VARCHAR) consigner,
                       CAST(isnull(z.name,'') AS VARCHAR)  AS [Zone],
                       CAST(isnull(b.name,'') AS VARCHAR)  AS branch,
                       CAST(bookingDate AS VARCHAR) bookingDate,
                       CAST(isnull(cd.orderRefNo,'') AS VARCHAR) orderRefNo,
                       CAST(isnull(c.consignmentNumber,'') AS VARCHAR) consignmentNumber,
                       CAST(isnull(consignee,'') AS VARCHAR) consignee,
                       CAST(isnull(consigneePhoneNo,'') AS VARCHAR) consigneePhoneNo,
                       CAST(isnull(c.ADDRESS,'') AS VARCHAR)   ADDRESS,
                       CAST(isnull(c.weight,'') AS VARCHAR)  WEIGHT,
                       CAST(isnull(c.pieces,'') AS VARCHAR)  pieces,
                        '' LocationName,'' CreatedDate,'' BookedCount,'' LoadsheetCount,''ArrivalCount,
                        '' ManifestCount,'' BaggingCount,'' BaggingCount,'' LoadingCount,
                       CASE 
                            WHEN rc.time IS NOT NULL THEN 'Delivered'
                            WHEN c.isApproved = 1 AND c.status <> '9' THEN 'Picked'
                            WHEN c.status = '9' THEN 'Void'
                            ELSE 'Booked'
                       END                                AS STATUS,
                       CAST(c.accountReceivingDate AS VARCHAR) Arrivaldt,
                       CAST(rc.time AS VARCHAR)           AS deliverydate
                FROM   Consignment C
                       INNER JOIN CODConsignmentDetail_New cd
                            ON  cd.consignmentNumber = c.consignmentNumber
                       LEFT JOIN RunsheetConsignment rc
                            ON  rc.consignmentNumber = c.consignmentNumber
                            AND rc.status = '55'
                            AND rc.Reason = '123'
                       LEFT JOIN BRANCHES B
                            ON  B.BRANCHCODE = C.BRANCHCODE
                            AND b.status = 1
                       LEFT JOIN Zones z
                            ON  z.zoneCode = b.zoneCode
               WHERE  C.serviceTypeName = 'Reversal COD' AND  CAST(c.createdOn AS DAte) BETWEEN CAST('" + dtStart + @"' AS date) AND CAST('" + dtEnd + @"' AS date)
                      "+ queryCondition2 + @"
                       "+queryCondition1+ @"                        
                       "+queryCondition3 ;

                //AND c.status = '9'-- Void

                conStatic.Open();
                SqlCommand orcd = new SqlCommand(sql, conStatic);
                orcd.CommandType = CommandType.Text;      
                 orcd.CommandTimeout = 3000;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                conStatic.Close();
            }
            catch (Exception er)
            {

            }
            finally
            {
                conStatic.Close();
            }
            return dt;
        }

        
        [WebMethod]
        public static List<SearchModel> GetAccounts(string Zone)
        {
            List<SearchModel> model = new List<SearchModel>();
            conStatic.Open();
            try
            {
                string sql = "";
                if (Zone.ToUpper() == "ALL")
                {
                    sql = @"   SELECT DISTINCT cc.accountNo,cc.accountNo+'-'+cc.name name   FROM CreditClients cc where cc.STATUS=1 AND cc.CODType=1  ";
                }
                else
                {
                    sql = @"   SELECT DISTINCT cc.accountNo,cc.accountNo+'-'+cc.name name   FROM CreditClients cc where cc.STATUS=1 AND cc.CODType=1   ";
                }
                SqlCommand command = new SqlCommand(sql, conStatic);
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        model.Add(new SearchModel { Value = rdr.GetString(0), Text = rdr.GetString(1) });
                    }
                }
            }
            catch (Exception er)
            {

            }
            finally
            {
                conStatic.Close();
            }
            return model;
        }

        [WebMethod]
        public static List<SearchModel> GetZones()
        {
            List<SearchModel> model = new List<SearchModel>();
            conStatic.Open();
            try
            {
                string sql = @" SELECT ZoneCode,name FROM Zones WHERE STATUS=1 AND Region IS NOT null";

                SqlCommand command = new SqlCommand(sql, conStatic);

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        model.Add(new SearchModel { Value = rdr.GetString(0), Text = rdr.GetString(1) });
                    }
                }
            }
            catch (Exception er)
            {

            }
            finally
            {
                conStatic.Close();
            }
            return model;
        }

         

    }
}