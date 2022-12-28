using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class CTS_ReceiptPrint : System.Web.UI.Page
    {

        String branchCode, ZoneCode, ECCode, ExpressCName, UserName, U_ID, BookingCode_ = "", shift_ = "";
        Cl_Variables clvar = new Cl_Variables();
        String startDate = "2020-06-08";
        String endDate = "getdate()";
        string previousPageUrl = "";
        string previousPageName = "";
        string oldDSSP = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                U_ID = Session["U_ID"].ToString();
                branchCode = Session["BRANCHCODE"].ToString();
                ZoneCode = Session["ZONECODE"].ToString();
                ECCode = Session["ExpressCenter"].ToString();
                string consignmentNumber = Request.QueryString["id"].ToString();
                if (!string.IsNullOrEmpty(consignmentNumber))
                {
                   // consignmentNumber = Encryption(consignmentNumber);
                    consignmentNumber = Decryption(consignmentNumber);
                    PrintReceipt(consignmentNumber);

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide consignment Number')", true);
                }
            }
            catch (Exception er)
            {
                //Response.Redirect("~/Login");
            }
        }

        public static string Encryption(string clearText)
        {
            string EncryptionKey = "i0ks0r";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decryption(string cipherText)
        {
            string EncryptionKey = "i0ks0r";
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

        public string NewPrinting(DataTable CNs)
        {
            string pageBreak = "";

            pageBreak = "page-break-after: always;";
            string consignment= CNs.Rows[0]["ConsignmentNumber"].ToString();
            string ConsignerName = CNs.Rows[0]["consigner"].ToString();
            string ConsignerCNIC = CNs.Rows[0]["consignerCNICNo"].ToString();
            string ConsignerPhone = CNs.Rows[0]["consignerCNICNo"].ToString();
            string logoMnP = "../images/MP-Logo_new.png";
            string imgMnPPrint = "../images/mnp_design_print.jpg";
            string imgCTS = "../images/CTS_Copy_Print.jpg";
            string ImgApplicant = "../images/applicantCopy_logo.jpg";


            string printBody = @"<!DOCTYPE html>

<html xmlns='http://www.w3.org/1999/xhtml'>
<head runat='server'>
    <title></title>
    <meta content='text/html;charset=utf-8' http-equiv='Content-Type'>
<meta content='utf-8' http-equiv='encoding'>
</head>
<body>

<style>
.left-side{
    float:left;
    padding-left:10px;
}
.right-side{
    float:right;
    padding-right:10px;
}
.row{
    height:40px;
    max-width:980px;
    padding-top:5px;
padding-bottom:20px;
}
.DataTable td{
    border: 1px solid black;
}
</style>
<div>

    <div class='First'>
        <div class='row'  >
            <div class='Column left-side'>
                <img src='" + logoMnP + @"' alt='M&P' style='' height='42px'/>
      
            <h3>" + consignment + @"</h3>
            </div>
            <div class='Column right-side'  > 

    <p style='size: 16px;font-size: 20px;font-weight: bolder;'>Candidates Testing Services Pakistan</p>
 <p style='font-size: 17px;margin-top: -20px !important;margin-left:180px'>Payment Receipt</p>
      <img  style='font-size: 20px;margin-top: -15px !important;margin-left:165px' src='" + imgMnPPrint + @"' alt='M&P Copy' height='30px'>
   
            </div>
        </div>
       <div style='margin-top: 20px;'>
            <table class='DataTable' cellpadding=0 cellspacing=0 width=100% >
                <tr>
                    <td >Test Processing Fees including all Govt tax Rs. 100/-<br>
                        <b>Total Amount Rs.100/-</b>
                    </td>
                </tr>
                <tr>
                    <td>Amount in words: <b>Rupees One Hunderd Only/-</b>
                    </td>
                </tr>
            </table>
            
            <table class='DataTable' style='margin-top: 10px;' width=100%  cellpadding=0 cellspacing=0 > 
               <tr>
                    <td width='30%'>Applicant's Name: </td>
                    <td width='50%' style='padding-left: 10px;'>" + ConsignerName + @"</td>
                    <td width='30%'></td>
                </tr>
                <tr>
                    <td width='30%'>CNIC / B-Form No:</td>      
                    <td width='50%' style='padding-left: 10px;'>" + ConsignerCNIC + @"</td>
                    <td width='30%'></td>
                </tr>
                <tr >
                    <td width='30%'>Project ID:</td>
                    <td width='50%' style='padding-left: 10px;'></td>
                    <td width='30%' style='padding-left: 10px;'>PP-202</td>
                </tr>
            </table>
        </div>
    </div>
    <hr style='border: 1px dashed black;margin-top:22px;margin-bottom:22px'>";

            printBody += @"
<div class='Second'>
        <div class='row'  >
            <div class='Column left-side'>
                <img src='" + logoMnP + @"' alt='M&P' style='' height='42px'/>
      
            <h3>" + consignment + @"</h3>
            </div>
            <div class='Column right-side'  > 

    <p style='size: 16px;font-size: 20px;font-weight: bolder;'>Candidates Testing Services Pakistan</p>
 <p style='font-size: 17px;margin-top: -20px !important;margin-left:180px'>Payment Receipt</p>
      <img  style='font-size: 20px;margin-top: -15px !important;margin-left:165px' src='" + imgCTS + @"' alt='CTS Copy' height='30px'>
   
            </div>
        </div>
        <div >
            <table class='DataTable' cellpadding=0 cellspacing=0 width=100%  style='margin-top: 10px;'>
                <tr>
                    <td >Test Processing Fees including all Govt tax Rs. 100/-<br>
                        <b>Total Amount Rs.100/-</b>
                    </td>
                </tr>
                <tr>
                    <td>Amount in words: <b>Rupees One Hunderd Only/-</b>
                    </td>
                </tr>
            </table>
            
            <table class='DataTable' style='margin-top: 10px;' width=100%   cellpadding=0 cellspacing=0> 
              <tr>
                    <td width='30%'>Applicant's Name: </td>
                    <td width='50%' style='padding-left: 10px;'>" + ConsignerName + @"</td>
                    <td width='30%'></td>
                </tr>
                <tr>
                    <td width='30%'>CNIC / B-Form No:</td>      
                    <td width='50%' style='padding-left: 10px;'>" + ConsignerCNIC + @"</td>
                    <td width='30%'></td>
                </tr>
                <tr >
                    <td width='30%'>Project ID:</td>
                    <td width='50%' style='padding-left: 10px;'></td>
                    <td width='30%' style='padding-left: 10px;'>PP-202</td>
                </tr>
            </table>
        </div>
    </div>
   <hr style='border: 1px dashed black;margin-top:22px;margin-bottom:22px'>";

            printBody += @"
<div class='Third'>
        <div class='row'  >

           <div class='Column left-side'>
                <img src='" + logoMnP + @"' alt='M&P' style='' height='42px'/>
      
            <h3>" + consignment + @"</h3>
            </div>
            <div class='Column right-side'  > 

    <p style='size: 16px;font-size: 20px;font-weight: bolder;'>Candidates Testing Services Pakistan</p>
 <p style='font-size: 17px;margin-top: -20px !important;margin-left:180px'>Payment Receipt</p>
      <img  style='font-size: 20px;margin-top: -15px !important;margin-left:165px' src='" + ImgApplicant + @"' alt='Applicant Copy' height='30px'>
   
            </div>
        </div>
        <div >
            <table class='DataTable' cellpadding=0 cellspacing=0 width=100% style='margin-top: 10px;'>
                <tr>
                    <td >Test Processing Fees including all Govt tax Rs. 100/-<br>
                        <b>Total Amount Rs.100/-</b>
                    </td>
                </tr>
                <tr>
                    <td>Amount in words: <b>Rupees One Hunderd Only/-</b>
                    </td>
                </tr>
            </table>
            
            <table class='DataTable' style='margin-top: 10px;' width=100%  cellpadding=0 cellspacing=0 > 
               <tr>
                    <td width='30%'>Applicant's Name: </td>
                    <td width='50%' style='padding-left: 10px;'>" + ConsignerName + @"</td>
                    <td width='30%'></td>
                </tr>
                <tr>
                    <td width='30%'>CNIC / B-Form No:</td>      
                    <td width='50%' style='padding-left: 10px;'>" + ConsignerCNIC + @"</td>
                    <td width='30%'></td>
                </tr>
                <tr >
                    <td width='30%'>Project ID:</td>
                    <td width='50%' style='padding-left: 10px;'></td>
                    <td width='30%' style='padding-left: 10px;'>PP-202</td>
                </tr>
            </table>
        </div>
    </div>
      
</div>
</body>";


            return printBody;
        }
        public void PrintReceipt(string CN)
        {
            DataTable Cns = GetConsignmentData(CN);
            StringBuilder html = new StringBuilder();

            html.Append(NewPrinting(Cns));

            ph1.Controls.Add(new Literal { Text = html.ToString() });
            HttpContext.Current.Response.Write("<script>window.print();</script>");
        }

        public DataTable GetConsignmentData(string CN)
        {
            DataTable dt = new DataTable();

            string sqlString = " Select * from Consignment cn where cn.ConsignmentNumber=@CN and isnull(docPouchNo,'0') = '1'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter oda = new SqlDataAdapter(sqlString, con);
                oda.SelectCommand.Parameters.AddWithValue("@CN", CN);
                oda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return dt;
        }

    }
}