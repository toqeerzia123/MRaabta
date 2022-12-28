using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta
{
    public partial class ECSelection : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();

        protected void Page_Load(object sender, EventArgs e)
        {
            Database_check();
            if (!IsPostBack)
            {
                GetExperssCenters();
                //   div_1.Style.Add("display", "");
                //urlCheck();
            }
        }

        public void GetExperssCenters()
        {
            string sql = "SELECT ec.name            ECNAME, \n"
                + "       ec.expressCenterCode \n"
                + "FROM   ExpressCenters     ec \n"
                + "WHERE  ec.[status] = '1' \n"
                + "       AND ec.bid = '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "'";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

                dd_ec.DataSource = dt;
                dd_ec.DataTextField = "ECNAME";
                dd_ec.DataValueField = "expressCenterCode";
                dd_ec.DataBind();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

        }

        public void urlCheck()
        {
            if (Request.QueryString["U_ID"] != null)
            {
                Session["Mac"] = Request.QueryString["U_ID"].ToString();
                Response.Redirect("login?ST=OK");
            }
            else if (Request.QueryString["ST"] != null)
            {
                if (Request.QueryString["ST"].ToString() == "OK" && Session["Mac"] != null)
                {
                    //div_1.Style.Add("display", "");
                    //div2.Style.Add("display", "none");
                }
                else
                {
                    //div_1.Style.Add("display", "none");

                    string Login_text = "";
                    Login_text += " <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #FFFFFF'>";
                    Login_text += "<tr>";
                    Login_text += "<td colpsan='2' align='center'>&nbsp;</td></tr>";
                    Login_text += "<td colpsan='2' align='center' style='top:auto'><img src='images/Alert.png' alt='ZNI' style='height: 20px' /> &nbsp; You are not an Authorized User</td></tr>";
                    Login_text += "<td colpsan='2' align='center'><a href='Default.aspx'>Login</a></td></tr>";
                    Login_text += "<td colpsan='2' align='center'>&nbsp;</td></tr>";
                    Login_text += "</table>";

                    //div2.Style.Add("display", "");
                    //div2.InnerHtml = Login_text;

                }
            }
            else
            {
                //div_1.Style.Add("display", "none");

                string Login_text = "";
                Login_text += " <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #FFFFFF'>";
                Login_text += "<tr>";
                Login_text += "<td colpsan='2' align='center'>&nbsp;</td></tr>";
                Login_text += "<td colpsan='2' align='center' style='top:auto'><img src='images/Alert.png' alt='ZNI' style='height: 20px' /> &nbsp; You are not an Authorized User</td></tr>";
                Login_text += "<td colpsan='2' align='center'><a href='Default.aspx'>Login</a></td></tr>";
                Login_text += "<td colpsan='2' align='center'>&nbsp;</td></tr>";
                Login_text += "</table>";

                //div2.Style.Add("display", "");
                //div2.InnerHtml = Login_text;

            }
        }

        protected void LoginButton_onClick(object sender, EventArgs e)
        {
            if (dd_ec.SelectedValue == "0")
            {
                AlertMessage("Select Express Center");
                return;
            }
            Session["ExpressCenter"] = dd_ec.SelectedValue;
            Session["EXPRESSCENTERNAME"] = dd_ec.SelectedItem.Text;

            //Session["User_Info"] = userName;

            b_fun.Insert_UserTrackLog(clvar);

            if (Session["bts_user"].ToString() == "0")
            {
                Response.Redirect("Files/Dashboard.aspx");
            }
            if (Session["bts_user"].ToString() == "1")
            {
                DateTime ExpireDate = DateTime.Parse(Session["INACTIVE_DATE"].ToString());
                DateTime CurrentDate = DateTime.Now;
                TimeSpan difference = ExpireDate.Date - CurrentDate.Date;
                int days = (int)difference.TotalDays;

                if (days <= 0)
                {
                    Response.Redirect("Files/PasswordChange.aspx");
                }
                else
                {
                    // Response.Redirect("Files/Consignment.aspx");
                    Response.Redirect("Files/BTSDashoard.aspx");
                }
            }
            if (Session["bts_user"].ToString() == "2")
            {
                Response.Redirect("Files/BTSDashoard.aspx");
            }
        }


        public void Database_check()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            try
            {
                con.Open();
                //  SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                //  sda.Fill(dt);

            }
            catch (Exception ex)
            {
                Response.Redirect("~/Login");
            }
            finally { con.Close(); }
        }



        public void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
    }
}