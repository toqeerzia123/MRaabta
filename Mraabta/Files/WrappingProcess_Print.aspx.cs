using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;

namespace MRaabta.Files
{
    public partial class WrappingProcess_Print : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();

        string Consignment = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                GenerateReport();
            }

        }

        public void GenerateReport()
        {

            string consignment = Decrypt_QueryString(Request.QueryString["ConsignmentNo"]);
            string consignee = Decrypt_QueryString(Request.QueryString["Consignee"]);
            string consigner = Decrypt_QueryString(Request.QueryString["Consigner"]);
            string consigneeaddress = Decrypt_QueryString(Request.QueryString["ConsignerAddress"]);
            string consigneraddress = Decrypt_QueryString(Request.QueryString["ConsigneeAddress"]);
            string bookingdate = Decrypt_QueryString(Request.QueryString["BookingDate"]);
            string originname = Decrypt_QueryString(Request.QueryString["Origin"]);
            string destinationname = Decrypt_QueryString(Request.QueryString["Destination"]);
            string pieces = Decrypt_QueryString(Request.QueryString["Pieces"]);
            string weight = Decrypt_QueryString(Request.QueryString["Weight"]);
            string servicetype = Decrypt_QueryString(Request.QueryString["ServiceType"]);
            string packagecontent = Decrypt_QueryString(Request.QueryString["PackageContent"]);
            //string wrappingcharges = Decrypt_QueryString(Request.QueryString["WrappingCharges"]);
            string comment = Decrypt_QueryString(Request.QueryString["Comment"]);
            string consigneemobile = Decrypt_QueryString(Request.QueryString["ConsigneeMobile"]);
            string consigneePhoneNo = Decrypt_QueryString(Request.QueryString["ConsigneePhoneNo"]);

            printAddressLabel(consignment, consigner, servicetype, pieces, originname, destinationname, consignee, weight, packagecontent, comment, consigneeaddress, consigneraddress, consigneemobile, consigneePhoneNo, bookingdate);

            #region Previous Logic


            /*
            DataTable dt = Get_WrappingProcess(consignment);

            if (dt.Rows.Count < 0)
            {
                string consignee = Decrypt_QueryString(Request.QueryString["Consignee"]);
                string consigner = Decrypt_QueryString(Request.QueryString["Consigner"]);
                string consigneeaddress = Decrypt_QueryString(Request.QueryString["ConsigneeAddress"]);
                string consigneraddress = Decrypt_QueryString(Request.QueryString["ConsignerAddress"]);
                string bookingdate = Decrypt_QueryString(Request.QueryString["BookingDate"]);
                string originname = Decrypt_QueryString(Request.QueryString["Origin"]);
                string destinationname = Decrypt_QueryString(Request.QueryString["Destination"]);
                string pieces = Decrypt_QueryString(Request.QueryString["Pieces"]);
                string weight = Decrypt_QueryString(Request.QueryString["Weight"]);
                string servicetype = Decrypt_QueryString(Request.QueryString["ServiceType"]);
                string packagecontent = Decrypt_QueryString(Request.QueryString["PackageContent"]);
                //string wrappingcharges = Decrypt_QueryString(Request.QueryString["WrappingCharges"]);
                string comment = Decrypt_QueryString(Request.QueryString["Comment"]);
                string consigneemobile = Decrypt_QueryString(Request.QueryString["ConsigneeMobile"]);
                string consigneePhoneNo = Decrypt_QueryString(Request.QueryString["ConsigneePhoneNo"]);

                printAddressLabel(consignment, consigner, servicetype, pieces, originname, destinationname, consignee, weight, packagecontent, comment, consigneeaddress, consigneraddress, consigneemobile, consigneePhoneNo, bookingdate);
                //return;
            }

            else
            {
                printAddressLabel(dt.Rows[0]["ConsignmentNumber"].ToString(), dt.Rows[0]["Consigner"].ToString(), dt.Rows[0]["ServiceTypeName"].ToString(), dt.Rows[0]["Pieces"].ToString(), dt.Rows[0]["Origin"].ToString(), dt.Rows[0]["Destination"].ToString(), dt.Rows[0]["Consignee"].ToString(), dt.Rows[0]["Weight"].ToString(), dt.Rows[0]["PackageContent"].ToString(), dt.Rows[0]["Comment"].ToString(), dt.Rows[0]["ConsigneeAddress"].ToString(), dt.Rows[0]["ConsignerAddress"].ToString(), dt.Rows[0]["ConsigneeMobile"].ToString(), dt.Rows[0]["ConsigneePhoneNo"].ToString(), dt.Rows[0]["BookingDate"].ToString());
            }
            */


            #endregion

        }

        public DataTable Get_WrappingProcess(string CN)
        {

            string ConsignmentNo = CN;
            DataTable dt = new DataTable();

            if (ConsignmentNo == "")
            {
                return dt;
            }

            string query = string.Empty;
            if (ConsignmentNo != "")
            {

                query = "SELECT wp.[ConsignmentNumber] AS 'ConsignmentNumber', \n"
                   + "       wp.[Consigner] AS 'Consigner', \n"
                   + "       wp.[Consignee] AS 'Consignee', \n"
                   + "       wp.[consigneePhoneNo] AS 'ConsigneePhoneNo', \n"
                   + "       wp.[consigneeMobileNo] AS 'ConsigneeMobile', \n"
                   + "       wp.[consigneeMobileNo] AS 'ConsigneeMobile', \n"
                   + "       wp.[consignee_address]           AS 'ConsigneeAddress', \n"
                   + "       wp.[shipperAddress]      AS 'ConsignerAddress', \n"
                   + "       b1.name AS 'Origin', \n"
                   + "       b2.name AS 'Destination', \n"
                   + "       wp.[Pieces] AS 'Pieces', \n"
                   + "       UPPER(wp.[ServiceTypeName]) AS 'ServiceTypeName', \n"
                   + "       wp.[Weight] AS 'Weight', \n"
                   + "       wp.[PackageContent2] AS 'PackageContent', \n"
                   + "       wp.[ConsignerAccountNo] AS 'AccountNo', \n"
                   + "       CONVERT(VARCHAR(10), wp.BookingDate, 103) AS 'BookingDate', \n"
                   + "       wp.[WrappingCharges] AS 'WrappingCharges', \n"
                   + "       wp.[usercomment]      AS 'Comment' \n"
                   + "FROM   [APL_BTS].[dbo].[MNP_Wrapping_Process] wp \n"
                   + "INNER JOIN Branches b1 \n"
                   + "ON b1.branchcode = wp.Orgin \n"
                   + "INNER JOIN Branches b2 \n"
                   + "ON b2.branchcode = wp.Destination \n"
                   + "WHERE \n"
                   + "wp.ConsignmentNumber = '" + ConsignmentNo + "'";
                //+ "AND wp.Orgin = '" + Session["BRANCHCODE"].ToString() + "'";
            }
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public static string Decrypt_QueryString(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        public void printAddressLabel(string consignment, string Shipper, string Service, string pieces, string Origin, string destination, string Consignee, string Weight, string productDetail, string Comment, string ConsigneeAddress, string ShipperAddress, string consigneeMobile, string consigneePhone, string bookingTime)
        {

            string sqlString_ = "<p style=\"font-family:IDAutomationHC39M\">";

            string Font_Size = "14";
            string Font_Size_Pieces = "18";
            string Address_Font_Size = "30";
            string Other_Font_Size = "20";

            #region For Label Print

            string BookingDate = bookingTime;
            BookingDate = BookingDate.Replace("-", "/");
            DateTime Temp_BookingDate = DateTime.Parse(BookingDate);
            BookingDate = Temp_BookingDate.ToString("dd/MM/yyyy");
            string sqlString = string.Empty;

            //228   OLD
            //455   NEW

            string Temp = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border: 1px solid black; height:450px\">\n" +
            "           <tr>\n" +
            "               <td rowspan=\"5\" style=\"width: 150px;\">\n" +
            "                   <img src=\"../images/mnpLogo.png\" alt=\"logo\" width=\"150\" height=\"80\" />\n" +
            "               </td>\n" +
            "               <td rowspan=\"5\" style=\"text-align: center; width: 240px;\">\n" +
            "                   <center>\n" +
            "                   <p style=\"font-family:IDAutomationHC39M; font-size:" + Font_Size + "px;\">*" + consignment + "*</p>\n" +
            "                   </center>\n" +
            "               </td>\n" +
            "               <td style=\"font-size:" + Font_Size + "px; width: 100px;\">\n" +
            "                   <b>Print Date & Time</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"font-size:" + Font_Size + "px\">\n" +
            "                    <p>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"width: 100px; font-size:" + Font_Size + "px;\">\n" +
            "                   <b>Booking Date</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"font-size:" + Font_Size + "px;\">\n" +
            "                  <p>" + BookingDate + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"width: 100px; font-size:" + Font_Size + "px\">\n" +
            "                   <b>Service</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"font-size:" + Font_Size + "px;\">\n" +
            "                    <p>" + Service + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"width: 100px; font-size:" + Font_Size + "px;\">\n" +
            "                   <b>Origin</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"font-size:" + Font_Size + "px;\">\n" +
            "                    <p>" + Origin + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"width: 100px; font-size:" + Font_Size + "px;\">\n" +
            "                   <b>Destination</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"font-size:" + Font_Size + "px;\">\n" +
            "                    <p>" + destination + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"border-bottom: 2px solid black; font-size:" + Font_Size + "px; width: 100px;\">\n" +
            "                   <b>From (Shipper)</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"1\" style=\"border: 2px black solid; font-size:" + Font_Size + "px;\">\n" +
            "                    <p>" + Shipper + "</p>\n" +
            "               </td>\n" +
            "               <td style=\"border-bottom: 2px solid black; font-size:" + Font_Size + "px; width: 100px;\">\n" +
            "                   <b>To (Consignee)</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"border: 2px black solid; font-size:" + Font_Size + "px;\">\n" +
            "                    <p>" + Consignee + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"4\" style=\"border-top: 0px;height:75px; font-size:14px; padding: 10px;\">\n" +
            "                    <p style=\"font-size:" + Other_Font_Size + "px; vertical-align:top; float:left; padding: 10px;\">To (Consignee),</p>\n" +
            "                    <p style=\"font-size:" + Address_Font_Size + "px; vertical-align:top; padding: 10px;\"><br><b>" + ShipperAddress.Substring(0, Math.Min(120, ShipperAddress.Length)) + "</b></p>\n" +
            "                    <p style=\"font-size:" + Other_Font_Size + "px; vertical-align:bottom; float:right; padding: 10px;\"><br><br><b>(" + destination + ")</b></p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"4\" style=\"border-top: 0px;height:75px; font-size:14px; padding: 10px;\">\n" +
            "                    <p style=\"font-size:" + Other_Font_Size + "px; vertical-align:top; float:left; padding: 10px;\">From (Shipper),</p>\n" +
            "                    <p style=\"font-size:" + Address_Font_Size + "px; vertical-align:top; padding: 10px;\"><br><b>" + ConsigneeAddress.Substring(0, Math.Min(120, ConsigneeAddress.Length)) + "</b></p>\n" +
            "                    <p style=\"font-size:" + Other_Font_Size + "px; vertical-align:bottom; float:right; padding: 10px;\"><br><br><b>(" + Origin + ")</b></p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"6\" style=\"border: 1px black; padding-left: 0px;\">\n" +
            "                   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" height=\"100%\">\n" +
            "                       <tr>\n" +
            "                           <td style=\"width: 96px; border: 1px solid black; font-size:" + Font_Size + "px;\">\n" +
            "                               <b>Pieces</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"width: 120px; border: 1px solid black; text-align: center; font-weight:bold; font-size:" + Font_Size_Pieces + "px;\">\n" +
            "                                <p>" + pieces + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"width: 100px; border: 1px solid black;font-size:" + Font_Size_Pieces + "px;\">\n" +
            "                               <b>Weight</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"width: 60px; border: 1px solid black; text-align: center; font-weight:bold; font-size:" + Font_Size + "px;\">\n" +
            "                               <p>" + Weight + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"width: 96px; border: 1px solid black; font-size:" + Font_Size + "px;\">\n" +
            "                               <b style='font-size:14px'>Consignee Mobile</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"width: 96px; border: 1px solid black; font-size:" + Font_Size + "px;\">\n" +
            "                                <p>" + consigneeMobile + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"width: 96px; border: 1px solid black; font-size:" + Font_Size + "px;\">\n" +
            "                               <b style='font-size:14px'>Consignee Phone</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"width: 96px; border: 1px solid black; font-size:" + Font_Size + "px;\">\n" +
            "                                <p>" + consigneePhone + "</p>\n" +
            "                           </td>\n" +
            "                       </tr>\n" +
            "                   </table>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"height:70px; font-size:" + Font_Size + "px;\">\n" +
            "                   <b>Package Content</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"7\" style=\"height:30px; font-size:" + Font_Size + "px; padding: 10px\">\n" +
            "                    <p>" + productDetail.Substring(0, Math.Min(120, productDetail.Length)) + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"height:70px; font-size:" + Font_Size + "px;\">\n" +
            "                   <b>Comment</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"7\" style=\"height:30px; font-size:" + Font_Size + "px; padding: 10px\">\n" +
            "                    <p>" + Comment.Substring(0, Math.Min(120, Comment.Length)) + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"6\" style=\"text-align: center\">\n" +
            "                   <b>WRAPPING LABEL</b>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "       </table>\n";


            sqlString += Temp;


            #endregion

            //lt_main.Text += sqlString;
            Response.Write(sqlString);

        }

    }
}