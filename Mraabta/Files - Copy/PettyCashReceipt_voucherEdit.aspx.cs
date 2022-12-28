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
    public partial class PettyCashReceipt_voucherEdit : System.Web.UI.Page
    {
        //Static Variable
        static string prevPage = string.Empty;

        Variable clvar = new Variable();
        bayer_Function bfunc = new bayer_Function();
        CommonFunction cfunc = new CommonFunction();
        clsVariables clsvar = new clsVariables();
        Cl_Variables cl_var = new Cl_Variables();
        int count = 1;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //   prevPage = Request.UrlReferrer.ToString();

                if (Request.QueryString["id"] != null)
                {

                    hf_headID.Value = Request.QueryString["id"].ToString();
                    //   hf_ID.Value = Request.QueryString["id"].ToString();
                    //  dd_month.SelectedValue = DateTime.Now.ToString("MM");
                    Get_Companies();
                    Get_Cash_Transfer_Mode();

                    dataDisplay();

                    hf_old_amount.Value = txt_amount.Text;

                    txt_date.Enabled = false;
                    if (Request.QueryString["stat"].ToString() == "HO_CIH")//HEAD OFFICE LEVEL
                    {
                        hf_user.Value = Request.QueryString["stat"].ToString();
                        btn_add.Text = "Approve";
                    }
                    else if (Request.QueryString["stat"].ToString() == "BRANCH_CIH")//BRANCH LEVEL
                    {
                        hf_user.Value = Request.QueryString["stat"].ToString();
                        btn_add.Text = "Update";
                    }

                }
            }

        }
        public void Get_Companies()
        {
            DataSet ds = new DataSet();
            ds = ds_companies();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_company.DataTextField = "sdesc_of";
                dd_company.DataValueField = "code_of";
                dd_company.DataSource = ds.Tables[0].DefaultView;
                dd_company.DataBind();
            }
        }
        public DataSet ds_companies()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from COMPANY_OF order by sdesc_of";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        public void dataDisplay()
        {
            DataSet ds = GetData();
            if (ds.Tables[0].Rows.Count > 0)
            {
                hf_branch.Value = ds.Tables[0].Rows[0]["bcode"].ToString();
                dd_year.Text = ds.Tables[0].Rows[0]["year"].ToString();
                dd_branch.Text = ds.Tables[0].Rows[0]["branch"].ToString();
                dd_month.Items.FindByValue(ds.Tables[0].Rows[0]["month"].ToString()).Selected = true;
                dd_company.Items.FindByValue(ds.Tables[0].Rows[0]["company"].ToString()).Selected = true;
                hf_old_amount.Value = ds.Tables[0].Rows[0]["amount"].ToString();

                //dd_status.Items.FindByValue(ds.Tables[0].Rows[0]["status_code"].ToString()).Selected = true;
                dd_cash_mode.Items.FindByValue(ds.Tables[0].Rows[0]["cash_type"].ToString()).Selected = true;
                txt_amount.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                txt_ID.Text = ds.Tables[0].Rows[0]["id"].ToString();
                txt_date.Text = ds.Tables[0].Rows[0]["date"].ToString();
                if (dd_cash_mode.SelectedValue.ToString() == "2")
                {
                    txt_checque_no.Visible = true;
                }
                else
                {
                    txt_checque_no.Visible = false;
                }
                txt_checque_no.Text = ds.Tables[0].Rows[0]["checque_no"].ToString();
                lbl_stat.Text = "Current Staus: " + ds.Tables[0].Rows[0]["status"].ToString();
            }
        }
        public DataSet Get_CashMode(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                query = "select * from  PC_cash_mode order by id\n";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {
            }
            finally
            { }
            return ds;
        }
        public void Get_Cash_Transfer_Mode()
        {
            DataSet ds = new DataSet();
            ds = Get_CashMode(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_cash_mode.DataTextField = "DESC";
                dd_cash_mode.DataValueField = "ID";
                dd_cash_mode.DataSource = ds.Tables[0].DefaultView;
                dd_cash_mode.DataBind();
            }
        }
        public DataTable data_generate(string sqlString)
        {
            DataTable Ds_1 = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        protected void dd_cash_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_cash_mode.SelectedValue.ToString() == "2")
            {
                lbl1.Visible = true;
                txt_checque_no.Visible = true;
            }
            else
            {
                lbl1.Visible = false;
                txt_checque_no.Visible = false;
            }
        }



        #region
        public DataSet GetData()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string sqlString = "select h.company,h.year,h.month,concat(h.ID, '-', d.ID) as voucher_id,\n" +
                "       h.ID head_id,\n" +
                "       d.ID,\n" +
                "       CONVERT(varchar, d.[DATE], 103) date,\n" +
                "     d.chque_no checque_no,d.cash_type,\n" +
                "       d.AMOUNT,\n" +
                "       b.name branch,h.branch bcode,\n" +
                "       CONVERT(varchar, h.Created_ON, 106) created_on,d.status status_code,\n" +
                " case d.[status] when '1' then 'Approved' when '2' then 'Rejected' else 'Unapproved' end status,d.status   from PC_CIH_head h\n" +
                " inner join PC_CIH_detail d\n" +
                "    on h.ID = d.HEAD_ID\n" +
                " inner join Branches b\n" +
                "    on b.branchCode = h.BRANCH\n" +
                " where\n" +
                   "  d.head_id='" + hf_headID.Value.ToString() + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        #endregion

        #region Button Click
        public DataTable CIH_remainingCash()
        {
            DataTable dt = new DataTable();
            try
            {

                string sqlString = "select c.[VALUE] remaining_cash,petty_cash from CIH_remainings c\n" +
                "where\n" +
                "c.month='" + dd_month.SelectedValue.ToString() + "'\n" +
                "and c.year='" + dd_year.Text + "'\n" +
                "and c.branch='" + hf_branch.Value.ToString() + "' and c.company='" + dd_company.SelectedValue.ToString() + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return dt;
        }
        protected void btn_add_Click(object sender, EventArgs e)
        {
            //:::::::::::Amount Checking::::::::::::::::://
            double remain_amount = 0;
            double remaining_val = 0;
            double remaining_pettyCash = 0;
            double diff = 0;
            double petty_amount = 0;
            lbl_error.Text = "";
            DataTable dt = new DataTable();
            dt = CIH_remainingCash();
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["remaining_cash"].ToString()))
                {
                    remaining_val = double.Parse(dt.Rows[0]["remaining_cash"].ToString()) + double.Parse(hf_old_amount.Value.ToString());

                }
                else
                {
                    remaining_val = 0;
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["petty_cash"].ToString()))
                {
                    remaining_pettyCash = double.Parse(dt.Rows[0]["petty_cash"].ToString()) - double.Parse(hf_old_amount.Value.ToString());

                }
                else
                {
                    remaining_pettyCash = 0;

                }
            }

            diff = remaining_val - double.Parse(txt_amount.Text);
            petty_amount = double.Parse(txt_amount.Text) + remaining_pettyCash;


            if (remaining_val <= double.Parse(txt_amount.Text))
            {
                lbl_error.Text = "The amount you entered exceeds the available Cash In Hand!!";
                return;
            }

            else
            {
                int j = UpdateData();
                if (j == 1)
                {
                    Label1.Text = "Voucher Updated!!";
                    //  lbl_error.Text = "";
                    //  string sqlString1 = "";
                    //  if (dd_cash_mode.SelectedValue.ToString() == "2")
                    //  {
                    //       sqlString1 = "update CIH_remainings set petty_cash='" + petty_amount + "', value='" + diff + "'\n" +
                    // " where\n" +
                    // " [month]='" + dd_month.SelectedValue.ToString() + "' and\n" +
                    // " [year]='" + dd_year.Text + "' and\n" +
                    // " [branch]='" + hf_branch.Value.ToString() + "' and company='" + dd_company.SelectedValue.ToString() + "'";
                    //  }
                    //  else
                    //  {
                    //       sqlString1 = "update CIH_remainings set petty_cash='" + petty_amount + "'\n" +
                    //" where\n" +
                    //" [month]='" + dd_month.SelectedValue.ToString() + "' and\n" +
                    //" [year]='" + dd_year.Text + "' and\n" +
                    //" [branch]='" + hf_branch.Value.ToString() + "' and company='" + dd_company.SelectedValue.ToString() + "'";
                    //  }

                    //  //update query
                    //  SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                    //  orcl.Open();
                    //  SqlCommand orcd1 = new SqlCommand(sqlString1, orcl);
                    //  orcd1.CommandType = CommandType.Text;
                    //  SqlDataAdapter oda1 = new SqlDataAdapter(orcd1);
                    //  orcd1.ExecuteNonQuery();
                    //  orcl.Close();

                }
                else
                {
                    Label1.Text = "";
                    lbl_error.Text = "The Voucher Is not Updated Due to Error!!";
                }
                lbl_error.Text = UpdateCIH_remaining() + "<\b>";
                lbl_error.Text += UpdatePC_remaining();
            }
        }
        #endregion

        public void calendar()
        {
            //string d = "01/" + dd_month.SelectedValue.ToString() + "/" + dd_year.SelectedItem.ToString() + "";
            //DateTime date_ = DateTime.Parse(d);
            //DateTime date = DateTime.Now;
            //if (date.ToString("MM/yyyy") == date_.ToString("MM/yyyy"))
            //{

            //    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            //    CalendarExtender1.StartDate = DateTime.Parse(firstDayOfMonth.ToString());
            //    CalendarExtender1.EndDate = DateTime.Now;
            //    CalendarExtender1.SelectedDate = DateTime.Now.Date;
            //}
            //else
            //{
            //    //last date of month
            //    DateTime firstOfNextMonth = new DateTime(date_.Year, date_.Month, 1).AddMonths(1);
            //    DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);


            //    var firstDayOfMonth = new DateTime(date_.Year, date_.Month, 1);
            //    //var lastDayOfMonth = ;
            //    CalendarExtender1.StartDate = DateTime.Parse(firstDayOfMonth.ToString());
            //    CalendarExtender1.EndDate = lastOfThisMonth;
            //    CalendarExtender1.SelectedDate = DateTime.Parse(firstDayOfMonth.ToString()).Date;

            //}
        }
        public int UpdateData()
        {
            try
            {
                string status = "";
                string by_user = "";
                string on_date = "";



                if (hf_user.Value == "HO")
                {
                    status = ", status = 1";
                    by_user = " APPROVED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "',";
                    on_date = " APPROVED_ON =GETDATE(),";
                }
                else if (hf_user.Value == "BRANCH")
                {
                    by_user = " CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "',";
                    on_date = " CREATED_ON =GETDATE(),";
                }
                string checque = "";
                if (dd_cash_mode.SelectedValue.ToString() == "2")
                {
                    checque = ",chque_no='" + txt_checque_no.Text + "'";
                }
                string query_head = "update PC_cih_detail set  " + by_user + "  " + on_date + " " +
                    " amount='" + txt_amount.Text + "',cash_type='" + dd_cash_mode.SelectedValue.ToString() + "' " +
                    " " + status + "" + checque + " where head_id='" + hf_headID.Value.ToString() + "' " +
                    "  \n";
                //, DATE='" + DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd") + "'
                //  " expense='" + dd_eh.SelectedValue.ToString() + "',[desc]='" + dd_subhead.SelectedValue.ToString() + "' " +
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query_head, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                lbl_error.Text = "";
                // Label1.Text = "Data Updated!!";
                dataDisplay();
                return 1;
            }
            catch (Exception Err)
            {
                lbl_error.Text = "The entry is not saved due to an error!!";
                return 0;
            }
            finally
            { }
        }
        protected void btn_back_Click(object sender, EventArgs e)
        {
            if (hf_user.Value == "HO_CIH")
            {
                Response.Redirect("PettyCash_ReceiptApproval.aspx");
            }
            else if (hf_user.Value == "BRANCH_CIH")
            {
                Response.Redirect("PettyCash_ReceiptEdit.aspx");
            }
        }

        #region update cih remainings
        public string UpdatePC_remaining()
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
            string result = "";
            DataSet ds = new DataSet();
            try
            {
                DateTime first_day = new DateTime(int.Parse(dd_year.Text), int.Parse(dd_month.SelectedValue.ToString()), 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("UpdatePettyCashBalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter SParam = new SqlParameter("Result", SqlDbType.VarChar);
                SParam.Direction = ParameterDirection.Output;
                SParam.Size = 500;
                sqlcmd.Parameters.AddWithValue("@BranchCode", hf_branch.Value.ToString());
                sqlcmd.Parameters.AddWithValue("@StartDate", first_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@EndDate", last_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@Month", dd_month.SelectedValue.ToString());
                sqlcmd.Parameters.AddWithValue("@Year", dd_year.Text);
                sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                result = SParam.Value.ToString();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                result = "Error in CIH Update!!";
            }
            finally
            { }
            return result;
        }
        public string UpdateCIH_remaining()
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
            string result = "";
            DataSet ds = new DataSet();
            try
            {
                DateTime first_day = new DateTime(int.Parse(dd_year.Text), int.Parse(dd_month.SelectedValue.ToString()), 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("UpdateCashInHandBalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter SParam = new SqlParameter("Result", SqlDbType.VarChar);
                SParam.Direction = ParameterDirection.Output;
                SParam.Size = 500;
                sqlcmd.Parameters.AddWithValue("@BranchCode", hf_branch.Value.ToString());
                sqlcmd.Parameters.AddWithValue("@StartDate", first_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@EndDate", last_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@Month", dd_month.SelectedValue.ToString());
                //   sqlcmd.Parameters.AddWithValue("@Year", dd_year.Text);
                sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                result = SParam.Value.ToString();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                result = "Error in CIH Update!!";
            }
            finally
            { }
            return result;
        }
        #endregion
    }
}