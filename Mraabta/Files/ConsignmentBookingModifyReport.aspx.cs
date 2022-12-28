using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class ConsignmentBookingModifyReport : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["U_ID"].ToString().Trim() != "1786")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/Files/BTSDashoard.aspx';", true);
            //    return;
            //}
            if (!IsPostBack)
            {
                txt_dateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //txt_dateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        protected void btn_generateReport_Click(object sender, EventArgs e)
        {


            clvar.BookingDate = txt_dateFrom.Text;
            clvar.FromDate = DateTime.Parse(txt_dateFrom.Text);
            clvar.ToDate = DateTime.Parse(txt_dateFrom.Text);
            clvar.CheckCondition = "BookingDate";
            //if (chk_all.Checked)
            //{
            //    clvar.AccountNo = "ALL";
            //}
            //else
            //{
            //    clvar.AccountNo = txt_accountNo.Text;
            //}
            if (rbAll.Checked) clvar.AccountNo = "ALL";
            else if (rbCash.Checked) clvar.AccountNo = "Cash";
            else if (rbCredit.Checked) clvar.AccountNo = "Credit";

            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script;
            if (rbDP.Checked)
            {
                script = String.Format(script_, "ConsignmentBookingModificationView.aspx?Xcode=" + clvar.AccountNo + "&dateFrom=" + txt_dateFrom.Text + "&tt=dp", "_blank", "");
            }
            else
                script = String.Format(script_, "ConsignmentBookingModificationView.aspx?Xcode=" + clvar.AccountNo + "&dateFrom=" + txt_dateFrom.Text + "&tt=acc", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
    }
}