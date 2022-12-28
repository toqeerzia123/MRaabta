using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class Get_DayCloseReport : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (txt_riderCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Rider Code')", true);
                return;
            }
            bool resp = ValidateRiderCode();

            if (!resp && txt_riderCode.Text.Trim() != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                return;
            }
            else
            {
                string rider = "";
                if (txt_riderCode.Text.Trim() != "")
                {
                    rider = "&RiderCode=" + txt_riderCode.Text.Trim();
                }
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                string script = String.Format(script_, "DayCloseReport.aspx?date=" + txt_date.Text.Trim() + rider, "_blank", "");

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
            }
        }

        protected bool ValidateRiderCode()
        {
            bool resp = false;
            string query = "select * from riders where status = '1' and ridercode = '" + txt_riderCode.Text.Trim() + "' and branchId = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    resp = true;
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return resp;
        }
    }
}