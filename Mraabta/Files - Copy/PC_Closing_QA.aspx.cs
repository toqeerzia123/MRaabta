using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class PC_Closing_QA : System.Web.UI.Page
    {
        public Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["U_ID"].ToString() != "3148")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/Files/BTSDashoard.aspx';", true);
            //    return;
            //}
            if (!IsPostBack)
            {
                GetYear();
            }
        }

        public void GetYear()
        {
            string sqlString = "SELECT DISTINCT cr.[year] FROM CIH_remainings cr";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

                dd_year.DataSource = dt;
                dd_year.DataTextField = "YEAR";
                dd_year.DataValueField = "YEAR";
                dd_year.DataBind();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            int year = 0;
            int month = 0;

            if (dd_year.SelectedValue == "0")
            {
                AlertMessage("Error", "Select Year", "Red");
                return;
            }
            if (dd_month.SelectedValue == "0")
            {
                AlertMessage("Error", "Select Month", "Red");
                return;
            }

            year = int.Parse(dd_year.SelectedValue);
            month = int.Parse(dd_month.SelectedValue);

            if (dd_dataType.SelectedValue == "0")
            {
                AlertMessage("Error", "Select DataType", "Red");
                return;
            }
        }

        public void AlertMessage(string messageType, string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), messageType, "alert('" + message + "')", true);
        }
    }
}