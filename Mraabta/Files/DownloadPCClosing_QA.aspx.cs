using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace MRaabta.Files
{
    public partial class DownloadPCClosing_QA : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            int year = 0;
            int month = 0;
            string dataType = Request.QueryString["Type"];
            year = int.Parse(Request.QueryString["year"]);
            month = int.Parse(Request.QueryString["month"]);

            DataTable dt = new DataTable();

            string fileName = "";
            string monthName = new DateTime(year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
            if (dataType == "1")
            {
                dt = GetPettyCashData(month, year);
                fileName = "PettyCash For  " + monthName + " " + year.ToString();
            }
            else if (dataType == "2")
            {
                dt = GetCashInHandData(month, year);
                fileName = "Cash in Hand For  " + monthName + " " + year.ToString();
            }


            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {


                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    using (StringWriter sw = new StringWriter())
                    {
                        HtmlTextWriter hw = new HtmlTextWriter(sw);

                        gv_pc.AllowPaging = false;
                        gv_pc.DataSource = dt;
                        gv_pc.DataBind();
                        gv_pc.RenderControl(hw);

                        //style to format numbers to string
                        //   string style = @"<style> .textmode { } </style>";
                        string style = @"<style> td { mso-number-format:\@;} </style>";
                        Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                        Response.Close();
                    }



                }
                else
                {
                    AlertMessage("Caution", "No Data Found", "Red");
                }
            }
            else
            {
                AlertMessage("Caution", "No Data Found", "Red");
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "CloseThisWindow()", true);
            //DataSet ds = GetData(clvar, year, month);

            //if (ds != null)
            //{
            //    if (ds.Tables.Count != 2)
            //    {
            //        AlertMessage("Error", "No Data Found", "Red");
            //        return;
            //    }
            //    else
            //    {
            //        gv_cih.DataSource = ds.Tables["CIH"];
            //        gv_cih.DataBind();

            //        gv_pc.DataSource = ds.Tables["PC"];
            //        gv_pc.DataBind();
            //    }
            //}
            //else
            //{
            //    AlertMessage("Error", "No Data Found", "Red");
            //    return;
            //}
        }

        public DataSet GetData(Cl_Variables clvar, int year, int month)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GET_PETTY_CASH_LEDGER_FOR_MONTH";
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;

                sda.Fill(ds, "PC");

                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GET_CASH_IN_HAND_LEDGER_FOR_MONTH";
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;

                sda.Fill(ds, "CIH");
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }

        public DataTable GetPettyCashData(int month, int year)
        {
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //  cmd.CommandText = "GET_PETTY_CASH_LEDGER_FOR_MONTH";
                cmd.CommandText = "GET_PETTY_CASH_LEDGER_FOR_MONTH_QA";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);

            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        public DataTable GetCashInHandData(int month, int year)
        {
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "GET_CASH_IN_HAND_LEDGER_FOR_MONTH_QA";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@UserId", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["U_NAME"].ToString());
                cmd.CommandTimeout = 300000;
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);

            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        public void AlertMessage(string messageType, string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), messageType, "alert('" + message + "')", true);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }
}