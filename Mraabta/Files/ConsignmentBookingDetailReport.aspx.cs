using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class ConsignmentBookingDetailReport : System.Web.UI.Page
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
                txt_dateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        protected void btn_generateReport_Click(object sender, EventArgs e)
        {


            clvar.BookingDate = txt_dateFrom.Text;
            clvar.FromDate = DateTime.Parse(txt_dateFrom.Text);
            clvar.ToDate = DateTime.Parse(txt_dateTo.Text);
            clvar.CheckCondition = dd_sort.SelectedValue;
            if (chk_all.Checked)
            {
                clvar.AccountNo = "ALL";
            }
            else
            {
                clvar.AccountNo = txt_accountNo.Text;
            }

            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "ConsignmentBookingDetailView.aspx?Xcode=" + clvar.AccountNo + "&dateFrom=" + txt_dateFrom.Text + "&dateTo=" + txt_dateTo.Text + "&sort=" + clvar.CheckCondition + "&mode=" + dd_criteria.SelectedValue, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
    }
}