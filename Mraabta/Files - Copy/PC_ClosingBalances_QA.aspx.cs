using System;
using System.Web.UI;
using MRaabta.App_Code;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MRaabta.Files
{
    public partial class PC_ClosingBalances_QA : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        string FromDate = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!(HttpContext.Current.Session["U_ID"].ToString() == "3148" || HttpContext.Current.Session["U_ID"].ToString() == "3177"))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/Files/BTSDashoard.aspx';", true);
            //    return;
            //}

            if (!IsPostBack)
            {
                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");




                //ZoneID += "'" + HttpContext.Current.Session["ZONECODE"].ToString().Items[i].Value + "',";


            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            FromDate = txt_date.Text;
            btn_HTML_Click(sender, e);
        }

        protected void btn_HTML_Click(object sender, EventArgs e)
        {
            string Query_FromDate = Encrypt_QueryString(FromDate.ToString());

            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "OpenWindow", "window.open('PC_ClosingBalances_QA_Print.aspx?fromdate=" + Query_FromDate + "','Petty Cash Closing Balances QA Print','menubar=1,scrollbars=yes,resizable=1,width=900,height=600');", true);
            return;
        }

        public static string Encrypt_QueryString(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}