using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;


namespace MRaabta.Files
{
    public partial class ChequeStatus : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        bayer_Function b_fun = new bayer_Function();
        Cl_Receipts rec = new Cl_Receipts();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetBanks();
                GetChequeStates();
            }
        }

        protected void GetBanks()
        {
            DataTable dt = GetBanks_();

            if (dt != null)
            {
                DataTable clientBank = dt.Select("isMnpBank = 'FALSE'").CopyToDataTable();
                dd_clientBank.DataSource = clientBank;
                dd_clientBank.DataTextField = "Name";
                dd_clientBank.DataValueField = "ID";
                dd_clientBank.DataBind();

                DataTable depositBank = dt.Select("isMnpBank = 'TRUE'").CopyToDataTable();
                dd_depositBank.DataSource = depositBank;
                dd_depositBank.DataTextField = "Name";
                dd_depositBank.DataValueField = "ID";
                dd_depositBank.DataBind();
            }
        }

        public DataTable GetBanks_()
        {

            string query = "select * from Banks order by Name";

            DataTable dt = new DataTable();
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

        protected void GetChequeStates()
        {
            DataTable dt = rec.GetChequeStates();
            if (dt != null)
            {
                ViewState["states"] = dt;
            }
        }

        protected void btn_GetCheques_Click(object sender, EventArgs e)
        {
            if (txt_month.Text.Trim() == "")
            {
                Errorid.Text = "Select Month";
                return;

            }
            if (dd_clientBank.SelectedValue == "0")
            {
                Errorid.Text = "Select Client Bank";
                return;
            }

            string month = "";
            string year = "";

            if (dd_depositBank.SelectedValue == "0")
            {
                clvar.CheckCondition = "";
            }
            else
            {
                clvar.CheckCondition = dd_depositBank.SelectedValue;
            }

            clvar.Bank = dd_clientBank.SelectedValue;

            DateTime date = DateTime.Parse(txt_month.Text);

            month = date.Month.ToString();
            year = date.Year.ToString();

            DataTable dt = rec.GetCheques(clvar, month, year);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_cheques_.DataSource = null;
                    gv_cheques_.DataBind();

                    gv_cheques_.DataSource = dt.DefaultView;
                    gv_cheques_.DataBind();


                    foreach (GridViewRow row in gv_cheques_.Rows)
                    {
                        RadioButtonList status = (RadioButtonList)row.FindControl("rbtn_gChequeStatus");

                        DataTable statuses = ViewState["states"] as DataTable;
                        status.DataSource = statuses;
                        status.DataTextField = "Name";
                        status.DataValueField = "ID";
                        status.DataBind();
                        status.RepeatColumns = statuses.Rows.Count;
                        status.SelectedValue = ((HiddenField)row.FindControl("hd_id")).Value;
                    }
                }
                else
                {
                    Errorid.Text = "No Cheques Found for this Month";
                    //  gv_cheques.DataSource = null;
                    // gv_cheques.DataBind();
                }
            }
            else
            {
                Errorid.Text = "No Cheques Found for this Month";
                //   gv_cheques.DataSource = null;
                //   gv_cheques.DataBind();
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("updateID");
            dt.Columns.Add("status");
            foreach (GridViewRow row in gv_cheques_.Rows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = (row.FindControl("hd_updateID") as HiddenField).Value;
                dr[1] = (row.FindControl("rbtn_gChequeStatus") as RadioButtonList).SelectedValue;

                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }

            rec.UpdateChequeStatus(clvar, dt);

            btn_GetCheques_Click(sender, e);
        }
    }
}