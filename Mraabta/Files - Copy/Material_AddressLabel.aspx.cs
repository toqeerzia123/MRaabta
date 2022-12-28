using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace MRaabta.Files
{
    public partial class Material_AddressLabel : System.Web.UI.Page
    {
        string HTMLPrintType = string.Empty;
        string RequestNumber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HTMLPrintType = "1";
                RequestNumber = Request.QueryString["id"].ToString();
                if (RequestNumber != "")
                {
                    GenerateReport();
                }
            }
        }
        public void GenerateReport()
        {
            //if (Session["conssss"] == null)
            //{
            //    string consignment = Decrypt_QueryString(Request.QueryString["con"]);
            //    //string consignment = Request.QueryString["con"];
            //}
            DataSet ds1 = Get_PreBookingsToProcess();
            if (HTMLPrintType == "0")
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        //printAddressLabel(ds1.Tables[0].Rows[i]["Consignment"].ToString(), ds1.Tables[0].Rows[i]["shipper"].ToString(), ds1.Tables[0].Rows[i]["service"].ToString(), ds1.Tables[0].Rows[i]["remarks"].ToString(), ds1.Tables[0].Rows[i]["pieces"].ToString(), ds1.Tables[0].Rows[i]["origin"].ToString(), ds1.Tables[0].Rows[i]["destination"].ToString(), ds1.Tables[0].Rows[i]["consignee"].ToString(), ds1.Tables[0].Rows[i]["codamount"].ToString(), ds1.Tables[0].Rows[i]["weight"].ToString(), ds1.Tables[0].Rows[i]["productdetail"].ToString(), ds1.Tables[0].Rows[i]["CustomerRef"].ToString(), ds1.Tables[0].Rows[i]["Consigneeaddress"].ToString(), ds1.Tables[0].Rows[i]["ConsigneePhoneNo"].ToString(), ds1.Tables[0].Rows[i]["Bookingdate"].ToString(), ds1.Tables[0].Rows[i]["locationName"].ToString(), ds1.Tables[0].Rows[i]["locationAddress"].ToString());
                        printAddressLabel(ds1.Tables[0].Rows[i]["ConsignmentNo"].ToString(), ds1.Tables[0].Rows[i]["SHIPPER_ADDRESS"].ToString(), ds1.Tables[0].Rows[i]["SERVICE"].ToString(), ds1.Tables[0].Rows[i]["Remarks"].ToString(), ds1.Tables[0].Rows[i]["Pieces"].ToString(), ds1.Tables[0].Rows[i]["ORIGIN"].ToString(), ds1.Tables[0].Rows[i]["DST"].ToString(), ds1.Tables[0].Rows[i]["weight"].ToString(), ds1.Tables[0].Rows[i]["ConsigneeADDRESS"].ToString(), ds1.Tables[0].Rows[i]["phoneNo"].ToString(), ds1.Tables[0].Rows[i]["IssuedDate"].ToString());
                        if (i == (ds1.Tables[0].Rows.Count - 1))
                        {

                        }
                        else
                        {
                            //add page break here
                            //lt_main.Text += "<p style='page-break-after:always;'></p>";
                            Response.Write("<p style='page-break-after:always;'></p>");
                        }
                    }
                }

            }
            //Single Copy
            else if (HTMLPrintType == "1")
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    int Index = 0;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        Index += 1;
                        //printAddressLabel(ds1.Tables[0].Rows[i]["Consignment"].ToString(), ds1.Tables[0].Rows[i]["shipper"].ToString(), ds1.Tables[0].Rows[i]["service"].ToString(), ds1.Tables[0].Rows[i]["remarks"].ToString(), ds1.Tables[0].Rows[i]["pieces"].ToString(), ds1.Tables[0].Rows[i]["origin"].ToString(), ds1.Tables[0].Rows[i]["destination"].ToString(), ds1.Tables[0].Rows[i]["consignee"].ToString(), ds1.Tables[0].Rows[i]["codamount"].ToString(), ds1.Tables[0].Rows[i]["weight"].ToString(), ds1.Tables[0].Rows[i]["productdetail"].ToString(), ds1.Tables[0].Rows[i]["CustomerRef"].ToString(), ds1.Tables[0].Rows[i]["Consigneeaddress"].ToString(), ds1.Tables[0].Rows[i]["ConsigneePhoneNo"].ToString(), ds1.Tables[0].Rows[i]["Bookingdate"].ToString(), ds1.Tables[0].Rows[i]["locationName"].ToString(), ds1.Tables[0].Rows[i]["locationAddress"].ToString());
                        printAddressLabel(ds1.Tables[0].Rows[i]["ConsignmentNo"].ToString(), ds1.Tables[0].Rows[i]["SHIPPER_ADDRESS"].ToString(), ds1.Tables[0].Rows[i]["SERVICE"].ToString(), ds1.Tables[0].Rows[i]["Remarks"].ToString(), ds1.Tables[0].Rows[i]["Pieces"].ToString(), ds1.Tables[0].Rows[i]["ORIGIN"].ToString(), ds1.Tables[0].Rows[i]["DST"].ToString(), ds1.Tables[0].Rows[i]["weight"].ToString(), ds1.Tables[0].Rows[i]["ConsigneeADDRESS"].ToString(), ds1.Tables[0].Rows[i]["phoneNo"].ToString(), ds1.Tables[0].Rows[i]["IssuedDate"].ToString());
                        if (i == (ds1.Tables[0].Rows.Count - 1))
                        {

                        }
                        if (Index % 3 == 0)     //  Check to Add Page Break After Every 3 Consecutive Records
                        {
                            //add page break here
                            //lt_main.Text += "<p style='page-break-after:always;'></p>";
                            Response.Write("<p style='page-break-after:always;'></p>");
                        }
                    }
                }
            }
            else if (HTMLPrintType == "2")
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        //printAddressLabel(ds1.Tables[0].Rows[i]["Consignment"].ToString(), ds1.Tables[0].Rows[i]["shipper"].ToString(), ds1.Tables[0].Rows[i]["service"].ToString(), ds1.Tables[0].Rows[i]["remarks"].ToString(), ds1.Tables[0].Rows[i]["pieces"].ToString(), ds1.Tables[0].Rows[i]["origin"].ToString(), ds1.Tables[0].Rows[i]["destination"].ToString(), ds1.Tables[0].Rows[i]["consignee"].ToString(), ds1.Tables[0].Rows[i]["codamount"].ToString(), ds1.Tables[0].Rows[i]["weight"].ToString(), ds1.Tables[0].Rows[i]["productdetail"].ToString(), ds1.Tables[0].Rows[i]["CustomerRef"].ToString(), ds1.Tables[0].Rows[i]["Consigneeaddress"].ToString(), ds1.Tables[0].Rows[i]["ConsigneePhoneNo"].ToString(), ds1.Tables[0].Rows[i]["Bookingdate"].ToString(), ds1.Tables[0].Rows[i]["locationName"].ToString(), ds1.Tables[0].Rows[i]["locationAddress"].ToString());
                        printAddressLabel(ds1.Tables[0].Rows[i]["ConsignmentNo"].ToString(), ds1.Tables[0].Rows[i]["SHIPPER_ADDRESS"].ToString(), ds1.Tables[0].Rows[i]["SERVICE"].ToString(), ds1.Tables[0].Rows[i]["Remarks"].ToString(), ds1.Tables[0].Rows[i]["Pieces"].ToString(), ds1.Tables[0].Rows[i]["ORIGIN"].ToString(), ds1.Tables[0].Rows[i]["DST"].ToString(), ds1.Tables[0].Rows[i]["weight"].ToString(), ds1.Tables[0].Rows[i]["ConsigneeADDRESS"].ToString(), ds1.Tables[0].Rows[i]["phoneNo"].ToString(), ds1.Tables[0].Rows[i]["IssuedDate"].ToString());
                        if (i == (ds1.Tables[0].Rows.Count - 1))
                        {

                        }
                        else
                        {
                            //add page break here
                            //lt_main.Text += "<p style='page-break-after:always;'></p>";
                            Response.Write("<p style='page-break-after:always;'></p>");
                        }
                    }
                }

            }
        }
        public static string Decrypt_QueryString_OLD(string str)
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
        private string Decrypt_QueryString(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public DataSet Get_PreBookingsToProcess()
        {
            string sortColumn = "consignmentNumber";
            string sortDirection = "ASC";
            if (Request.QueryString.AllKeys.Contains<string>("S") && Request.QueryString.AllKeys.Contains<string>("D"))
            {
                sortColumn = Request.QueryString["S"];
                sortDirection = Request.QueryString["D"];
            }
            string orderBy = "";

            if (sortColumn != "" && sortDirection != "")
            {
                orderBy = "ORDER BY CAST(" + sortColumn + " as varchar) " + sortDirection;
                if (sortColumn.ToUpper() == "CONSIGNMENTNUMBER")
                {
                    orderBy = "ORDER BY c.ConsignmentNumber " + sortDirection;
                }
                if (sortColumn.ToUpper() == "BOOKINGDATE")
                {
                    orderBy = "ORDER BY FORMAT(" + "c." + sortColumn + ", 'yyyy-MM-dd') " + sortDirection;
                }


            }
            string multiple_consignment = "";
            string consignment = "";

            string sqlString = @"SELECT prd.ConsignmentNo,
	                        c.IssuedDate, b.name ORIGIN, b2.name DST, PRD.CNWeight WEIGHT, PRD.Pieces, L.locationAddress ConsigneeADDRESS, 
	                        NULL SHIPPER_ADDRESS, PRD.Remarks, CC.phoneNo, 'MATERIAL' SERVICE
                        FROM
                            PR_PackingRequest c
                            INNER JOIN PR_PackingRequestDetail prd ON c.PackingRequestID = prd.PackingRequestID
                            INNER JOIN COD_CustomerLocations L ON C.CustomerID = L.CreditClientID AND C.LocationID = L.locationID
                            INNER JOIN Branches b ON b.branchcode = c.branchcode
	                        INNER JOIN Branches b2 ON b2.branchCode = L.brancahCode
                            INNER JOIN CreditClients cc ON cc.id = L.CreditClientID
                        WHERE
                            prd.ConsignmentNo = '" + RequestNumber + @"'";

            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
        public DataSet Get_PreBookingsDetail()
        {
            //string consignment = Request.QueryString["con"];
            string consignment = Decrypt_QueryString(Request.QueryString["con"]);

            string sqlString = "select * from CODConsignmentDetail_new c where c.consignmentNumber = '" + consignment + "'";

            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
        public void printAddressLabel(string consignment, string Shipper, string Service, string Remarks, string pieces, string Origin, string destination, string Weight, string ConsigneeAddress, string consigneePhone, string bookingTime)
        //public void printAddressLabel(string consignment, string Shipper, string Service, string Remarks, string pieces, string Origin, string destination, string Consignee, string codamount, string Weight, string productDetail, string orderRef, string ConsigneeAddress, string consigneePhone, string bookingTime, string locationName, string locationAddress)
        {


            string sqlString = "<p style='font-family:IDAutomationHC39M'>";

            if (HTMLPrintType == "0")
            {
                sqlString = "<table border='1' cellpadding='0' cellspacing='0' width='100%' style='border: 1px solid black; height:228px'>\n" +
           "           <tr>\n" +
           "               <td rowspan='3' style='width: 150px;'>\n" +
           "                   <img src='assets/images/mnpLogo.png' alt='logo' width='130' height='50' />\n" +
           "               </td>\n" +
           "               <td rowspan='3' style='text-align: center; width: 280px;'>\n" +
           "                   <p style='font-family:IDAutomationHC39M'>*" + consignment + "*</p>\n" +
           "                   <h5>\n" +
           "                       Consignee Copy\n" +
           "                   </h5>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                   <b>Booking Date</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                  <p>" + bookingTime + "</p>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                   <b>Print Date & Time</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
          "                    <p>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td>\n" +
           "                   <b>Service</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                    <p>" + Service + "</p>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                   <b>Location Name</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
           //"                    <p>" + locationName + "</p>\n" +
           "                    <p></p>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td>\n" +
           "                   <b>Origin</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                    <p>" + Origin + "</p>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                   <b>Destination</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                    <p>" + destination + "</p>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td>\n" +
           "                   <b>Pieces</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                    <p>" + pieces + "</p>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                   <b>Weight</b>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "                    <p>" + Weight + "</p>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td style='border-bottom: 2px solid black;'>\n" +
           "                   <b>Shipper</b>\n" +
           "               </td>\n" +
           "               <td colspan='2' style='border-bottom: 1px;'>\n" +
           "                    <p>" + Shipper + "</p>\n" +
           "               </td>\n" +
           "               <td style='border-bottom: 2px solid black;'>\n" +
           "                   <b>Consignee</b>\n" +
           "               </td>\n" +
           "               <td colspan='2' style='border-bottom: 0px;'>\n" +
           // "                    <p>" + Consignee + "</p>\n" +
           "                    <p> </p>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td colspan='3' style='border-top: 0px;'>\n" +
           // "                    <p>" + locationAddress.Substring(0, Math.Min(120, locationAddress.Length)) + "</p>\n" +
           "                    <p> </p>\n" +
           "               </td>\n" +
           "               <td colspan='3' style='border-top: 0px;height:65px;'>\n" +
           "                    <p>" + ConsigneeAddress.Substring(0, Math.Min(120, ConsigneeAddress.Length)) + "</p>\n" +
           "               </td>\n" +
           "           </tr>\n" +

           "           <tr>\n" +
           "               <td>\n" +
           "                   <b>Product Detail</b>\n" +
           "               </td>\n" +
           "               <td colspan='5'>\n" +
           //"                    <p>" + productDetail + "</p>\n" +
           "                    <p></p>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td>\n" +
           "                   <b>Remarks</b>\n" +
           "               </td>\n" +
           "               <td colspan='5'>\n" +
           "                    <b>" + Remarks + "</b>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td>\n" +
           "                   <b>Customer Ref. #</b>\n" +
           "               </td>\n" +
           "               <td colspan='4'>\n" +
           // "                   <p>" + orderRef + "</p>\n" +
           "                   <p> </p>\n" +
           "               </td>\n" +
           "               <td>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "           <tr>\n" +
           "               <td colspan='6' style='text-align: center'>\n" +
           "                   <b>Please don't accept if shipment is not intact. Before paying the COD, shipment can't be opened. M&P is not responsible for Product, in case of any issue please contact shop/shipper.</b>\n" +
           "               </td>\n" +
           "           </tr>\n" +
           "       </table>\n" +
           "       <div style='width: 90%; padding-left: 70px;'>\n" +
           "           <hr />\n" +
           "       </div>\n";

            }
            else if (HTMLPrintType == "1")
            {
                sqlString = "<table border='1' cellpadding='0' cellspacing='0' width='100%' style='border: 1px solid black; height:228px'>\n" +
     "           <tr>\n" +
     "               <td rowspan='3' style='width: 150px;'>\n" +
     "                   <img src='../images/mnpLogo.png' alt='logo' width='130' height='50' />\n" +
     "               </td>\n" +
     "               <td rowspan='3' style='text-align: center; width: 280px;'>\n" +
     "                   <p style='font-family:IDAutomationHC39M'>*" + consignment + "*</p>\n" +
     "                   <h5>\n" +
     "                       Consignee Copy\n" +
     "                   </h5>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Booking Date</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                  <p>" + bookingTime + "</p>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Print Date & Time</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
    "                    <p>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td>\n" +
     "                   <b>Service</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                    <p>" + Service + "</p>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Location Name</b>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px'>\n" +
     "                    <p> </p>\n" +
     //"                    <p>" + locationName + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td>\n" +
     "                   <b>Origin</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                    <p>" + Origin + "</p>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Destination</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                    <p>" + destination + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td colspan='2'>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Pieces</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                    <p>" + pieces + "</p>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Weight</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                    <p>" + Weight + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td style='border-bottom: 2px solid black;'>\n" +
     "                   <b>Shipper</b>\n" +
     "               </td>\n" +
     "               <td colspan='2' style='border-bottom: 1px;'>\n" +
     "                    <p>" + Shipper + "</p>\n" +
     "               </td>\n" +
     "               <td style='border-bottom: 2px solid black;'>\n" +
     "                   <b>Consignee</b>\n" +
     "               </td>\n" +
     "               <td colspan='2' style='border-bottom: 0px;'>\n" +
     //"                    <p>" + Consignee + "</p>\n" +
     "                    <p> </p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td colspan='3' style='border-top: 0px;'>\n" +
     //"                    <p>" + locationAddress.Substring(0, Math.Min(120, locationAddress.Length)) + "</p>\n" +
     "                    <p> </p>\n" +
     "               </td>\n" +
     "               <td colspan='3' style='border-top: 0px;height:65px;'>\n" +
     "                    <p>" + ConsigneeAddress.Substring(0, Math.Min(120, ConsigneeAddress.Length)) + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +

     "           <tr>\n" +
     "               <td>\n" +
     "                   <b>Product Detail</b>\n" +
     "               </td>\n" +
     "               <td colspan='2'>\n" +
     // "                    <p>" + productDetail + "</p>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Consignee Contact</b>\n" +
     "               </td>\n" +
     "               <td colspan='2'>\n" +
      "                    <p>" + consigneePhone + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td>\n" +
     "                   <b>Remarks</b>\n" +
     "               </td>\n" +
     "               <td colspan='2'>\n" +
     "                    <b>" + Remarks + "</b>\n" +
     "               </td>\n" +
     "               <td>\n" +
     "                   <b>Customer Ref. #</b>\n" +
     "               </td>\n" +
     "               <td colspan='2'>\n" +
     // "                   <p>" + orderRef + "</p>\n" +
     "                   <p> </p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td colspan='6' style='text-align: center'>\n" +
     "                   <b>Please don't accept if shipment is not intact.</b>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "       </table>\n";
            }
            else if (HTMLPrintType == "2")
            {
                sqlString = "<table border='1' cellpadding='0' cellspacing='0' style='border: 1px solid black; width:500px;'>\n" +
     "           <tr>\n" +
     "             <td colspan='6'> <table border='0' cellpadding='0' cellspacing='0' width='100%'>\n" +
     "               <td  rowspan='2' style='padding:3px;'>\n" +
     "                   <img src='assets/images/mnpLogoS.png' alt='logo'/>\n" +
     "               </td>\n" +
     "               <td colspan='3' rowspan='2' style='text-align:center; width: 450px; height: 60px; padding-left:3px;border: 1px solid black;'>\n" +
     "                   <p style='font-family:IDAutomationHC39M;font-size:14px'>*" + consignment + "*</p>\n" +
     "               </td>\n" +
     "               <td colspan='2' style='text-align:center;width:180px;font-size:14px'>\n" +
     "                  <b>Customer Ref. No</b>\n" +
     "                </td>\n" +
    "           </tr>\n" +
     "           <tr>\n" +
     "                <td colspan='2' style='text-align:center;font-size:18px'>\n" +
     //"                  <p>" + orderRef + "</p>\n" +
     "                  <p> </p>\n" +
     "                </td>\n" +
     "                  </table>" +
     "                </td>\n" +
    "           </tr>\n" +
     "           <tr>\n" +
     "               <td style='font-size:12px;width:70px;padding-left:3px'>\n" +
     "                   <b>Booking Date</b>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;width:80px;padding-left:3px;'>\n" +
     "                  <p>" + bookingTime + "</p>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;width:70px;padding-left:3px;'>\n" +
     "                   <b>Print Date & Time</b>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;width:60px;padding-left:3px;'>\n" +
    "                    <p>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;padding-left:3px;'>\n" +
     "                   <b>Service</b>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;padding-left:3px;'>\n" +
     "                    <p>" + Service + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td style='font-size:12px;padding-left:3px;'>\n" +
     "                   <b>Origin</b>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;padding-left:3px;'>\n" +
     "                    <p>" + Origin + "</p>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;padding-left:3px;'>\n" +
     "                   <b>Location Name</b>\n" +
     "               </td>\n" +
     "               <td colspan='3' style='font-size:12px;padding-left:3px;'>\n" +
     //  "                    <p>" + locationName + "</p>\n" +
     "                    <p> </p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td style='font-size:12px;padding-left:3px;'>\n" +
     "                   <b>Insurance Value</b>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;padding-left:3px;text-align:center;'>\n" +
     "                    <p>0</p>\n" +
     "               </td>\n" +
     "               <td style='font-size:12px;padding-left:3px;'>\n" +
     "                   <b>Destination</b>\n" +
     "               </td>\n" +
     "               <td colspan='3' style='font-size:12px;padding-left:3px;'>\n" +
     "                    <p>" + destination + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td style='border-bottom: 2px solid black;font-size:13px;width:50px;'>\n" +
     "                   <b>Shipper</b>\n" +
     "               </td>\n" +
     "               <td colspan='2' style='border-bottom: 1px;font-size:14px;'>\n" +
     "                    <p>" + Shipper + "</p>\n" +
     "               </td>\n" +
     "               <td style='border-bottom: 2px solid black;font-size:13px;'>\n" +
     "                   <b>Consignee</b>\n" +
     "               </td>\n" +
     "               <td colspan='2' style='border-bottom: 0px;font-size:14px;'>\n" +
     //  "                    <p>" + Consignee + " (0" + consigneePhone + ")</p>\n" +
     "                    <p> </p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td colspan='3' style='border-top: 0px;height:65px;font-size:14px;'>\n" +
     // "                    <p>" + locationAddress.Substring(0, Math.Min(120, locationAddress.Length)) + "</p>\n" +
     "                    <p> </p>\n" +
     "               </td>\n" +
     "               <td colspan='3' style='border-top: 0px;height:65px;font-size:14px;'>\n" +
     "                    <p>" + ConsigneeAddress.Substring(0, Math.Min(120, ConsigneeAddress.Length)) + "</p>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "           <tr>\n" +
     "               <td colspan='6' style='border: 1px black; padding-left: 0px;'>\n" +
     "                   <table border='0' cellpadding='0' cellspacing='0' width='100%'>\n" +
     "                       <tr>\n" +
     "                           <td style='border: 1px solid black;font-size:14px;'>\n" +
     "                               <b>Pieces</b>\n" +
     "                           </td>\n" +
     "                           <td style='border: 1px solid black; text-align: right;font-size:14px;'>\n" +
     "                                <p>" + pieces + "</p>\n" +
     "                           </td>\n" +
     "                           <td style='border: 1px solid black;font-size:14px;'>\n" +
     "                               <b>Weight</b>\n" +
     "                           </td>\n" +
     "                           <td style='border: 1px solid black; text-align: right;font-size:14px;'>\n" +
     "                               <p>" + Weight + "</p>\n" +
     "                           </td>\n" +
     "                          <td style='border: 1px solid black; text-align: center;width:100px;font-size:14px;'>\n" +
     "                              <b>COD AMOUNT</b>\n" +
     "                          </td>\n" +
     "                          <td style='border: 1px solid black; text-align: left; width:80px;font-size:16px;'>\n" +
     //"                              <B>" + string.Format("{0:C}", Convert.ToInt32(codamount)).ToString().Replace('$', ' ') + "/-</B>\n" +
     "                              <B> </B>\n" +
     "                          </td>\n" +
     "                           <td style='border: 1px solid black; text-align: left;font-size:14px;'>\n" +
     "                               <b>Fragile</b>\n" +
     "                           </td>\n" +
     "                          <td style='border: 1px solid black; text-align: left;font-size:14px;padding-left:3px;padding-right:5px;'>\n" +
     "                              <p>Yes</p>\n" +
     "                          </td>\n" +
     "                       </tr>\n" +
     "                   </table>\n" +
     "                   <table border='0' cellpadding='0' cellspacing='0' width='100%'>\n" +
     "                       <tr>\n" +
     "                           <td style='border: 1px solid black; width:124px;text-align: left;font-size:13px;'>\n" +
     "                               <b>Product Detail</b>\n" +
     "                           </td>\n" +
     "                           <td style='border: 1px solid black;font-size:13px;'>\n" +
     // "                    <B>" + productDetail.Substring(0, Math.Min(80, productDetail.Length)) + "</b>\n" +
     "                    <B> </b>\n" +
     "                           </td>\n" +
     "                       <tr>\n" +
     "                       </tr>\n" +
     "                           <td style='border: 1px solid black;font-size:13px;'>\n" +
     "                               <b>Remarks</b>\n" +
     "                           </td>\n" +
     "                           <td style='border: 1px solid black;font-size:13px;'>\n" +
     "                    <b>" + Remarks.Substring(0, Math.Min(40, Remarks.Length)) + "</b>\n" +
     "                       </tr>\n" +
     "                   </table>\n" +
     "               </td>\n" +
     "           <tr>\n" +
     "               <td colspan='6' style='text-align: left;font-size:10.3px;'>\n" +
     "                   <b>Please don't accept if shipment is not intact. Before paying the COD, shipment can't be opened. M&P is not responsible for Product, in case of any issue please contact shop/shipper.</b>\n" +
     "               </td>\n" +
     "           </tr>\n" +
     "       </table>\n";

            }
            //lt_main.Text += sqlString;
            Response.Write(sqlString);

        }
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document
            doc.Open();
            htmlWorker.StartDocument();

            // 5: parse the html into the document
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }
        public void DownloadPDF()
        {
            string HTMLContent = "Hello <b>World</b>";

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GetPDF(HTMLContent));
            Response.End();
        }
    }
}