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
    public partial class PettyCash_voucherEdit : System.Web.UI.Page
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

                if (Request.QueryString["head_id"] != null)
                {

                    hf_headID.Value = Request.QueryString["head_id"].ToString();
                    hf_ID.Value = Request.QueryString["id"].ToString();
                    //dd_month.SelectedValue = DateTime.Now.ToString("MM");

                    Get_MainHead();
                    Get_Companies();
                    Get_Division();
                    Get_proj();
                    Get_Product();

                    Get_subdept();



                    dataDisplay();
                    txt_date.Enabled = false;
                    dd_company.Enabled = false;
                    if (Request.QueryString["stat"].ToString() == "HO")//HEAD OFFICE LEVEL
                    {
                        hf_user.Value = Request.QueryString["stat"].ToString();
                        btn_add.Text = "Approve";
                    }
                    else if (Request.QueryString["stat"].ToString() == "BRANCH")//BRANCH LEVEL
                    {
                        hf_user.Value = Request.QueryString["stat"].ToString();
                        btn_add.Text = "Update";
                    }

                }
            }

        }

        public void dataDisplay()
        {
            DataSet ds = GetData();
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region OF fields assignment
                string div = ds.Tables[0].Rows[0]["division"].ToString();
                string subprod = ds.Tables[0].Rows[0]["sub_prod"].ToString();
                string subdept = ds.Tables[0].Rows[0]["sub_dept"].ToString();
                string proj = ds.Tables[0].Rows[0]["project"].ToString();

                dd_company.Items.FindByValue(ds.Tables[0].Rows[0]["company"].ToString()).Selected = true;
                Get_Subprod();
                if (!string.IsNullOrEmpty(div))
                {
                    dd_division.Items.FindByValue(div).Selected = true;
                }
                if (!string.IsNullOrEmpty(proj))
                {
                    dd_proj.Items.FindByValue(proj).Selected = true;
                }
                if (!string.IsNullOrEmpty(subdept))
                {
                    dd_subdept.Items.FindByValue(subdept).Selected = true;
                }
                if (!string.IsNullOrEmpty(subprod))
                {
                    dd_sub_prod.Items.FindByValue(subprod).Selected = true;
                }
                #endregion//OF Fields region Ends here

                hf_bcode.Value = ds.Tables[0].Rows[0]["bcode"].ToString();
                dd_ec.Text = ds.Tables[0].Rows[0]["ec"].ToString();
                dd_year.Text = ds.Tables[0].Rows[0]["year"].ToString();
                dd_branch.Text = ds.Tables[0].Rows[0]["branch"].ToString();
                dd_month.Items.FindByValue(ds.Tables[0].Rows[0]["month"].ToString()).Selected = true;

                hf_old_amount.Value = ds.Tables[0].Rows[0]["amount"].ToString();

                //dd_status.Items.FindByValue(ds.Tables[0].Rows[0]["status_code"].ToString()).Selected = true;
                dd_eh.Items.FindByValue(ds.Tables[0].Rows[0]["hoa"].ToString()).Selected = true;
                dd_eh.Enabled = false;
                Get_subHead();
                dd_subhead.Items.FindByValue(ds.Tables[0].Rows[0]["ddesc"].ToString()).Selected = true;
                dd_subhead.Enabled = false;
                txt_amount.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                txt_narrate.Text = ds.Tables[0].Rows[0]["narrate"].ToString();
                txt_ID.Text = ds.Tables[0].Rows[0]["voucher_id"].ToString();
                txt_date.Text = ds.Tables[0].Rows[0]["date"].ToString();

                lbl_stat.Text = "Current Staus: " + ds.Tables[0].Rows[0]["status"].ToString();
            }
        }

        public void month_cih_check()
        {

            string sqlString = "select * from CIH_remainings c\n" +
            "where c.month='" + dd_month.SelectedValue.ToString() + "'\n" +
            "and c.branch='" + hf_bcode.Value.ToString() + "' and year='" + dd_year.Text + "' and c.company='" + dd_company.SelectedValue.ToString() + "'";
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
                if (Ds_1.Rows.Count > 0)
                {

                    if (!string.IsNullOrEmpty(Ds_1.Rows[0]["petty_cash"].ToString()))
                    {
                        double p_cash = double.Parse(Ds_1.Rows[0]["petty_cash"].ToString());
                        hf_pamount.Value = p_cash.ToString();
                    }
                    else
                    {
                        hf_pamount.Value = "0";
                    }
                }

            }
            catch (Exception Err)
            { }
            finally
            { }

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

        #region dropdown calling
        public void Get_MainHead()
        {
            DataSet ds = new DataSet();
            ds = Mainhead();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_eh.DataTextField = "description";
                dd_eh.DataValueField = "code";
                dd_eh.DataSource = ds.Tables[0].DefaultView;
                dd_eh.DataBind();
            }
        }
        public void Get_subHead()
        {
            DataSet ds = new DataSet();
            ds = subhead();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_subhead.DataTextField = "sub_desc";
                dd_subhead.DataValueField = "subcode";
                dd_subhead.DataSource = ds.Tables[0].DefaultView;
                dd_subhead.DataBind();
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
        public void Get_Division()
        {
            DataSet ds = new DataSet();
            ds = ds_Division();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_division.DataTextField = "sdesc_of";
                dd_division.DataValueField = "code_of";
                dd_division.DataSource = ds.Tables[0].DefaultView;
                dd_division.DataBind();
                dd_division.Items.Insert(0, new ListItem("Select Division", "0"));
            }
        }
        public DataSet ds_Division()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from DIVISION_OF order by sdesc_of";
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
        public void Get_Product()
        {
            DataSet ds = new DataSet();
            ds = ds_product();
            if (ds.Tables[0].Rows.Count != 0)
            {
                //dd_prod.DataTextField = "sdesc_of";
                //dd_prod.DataValueField = "prd_of";
                //dd_prod.DataSource = ds.Tables[0].DefaultView;
                //dd_prod.DataBind();
                //dd_prod.Items.Insert(0, new ListItem("Select Product", "0"));
            }
        }
        public DataSet ds_product()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from products_of order by sdesc_of";
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
        public void Get_Subprod()
        {
            DataSet ds = new DataSet();
            ds = ds_Subprod();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_sub_prod.DataTextField = "sdesc_of";
                dd_sub_prod.DataValueField = "prd_of";
                dd_sub_prod.DataSource = ds.Tables[0].DefaultView;
                dd_sub_prod.DataBind();
                dd_sub_prod.Items.Insert(0, new ListItem("Select Sub Product", "0"));
            }
        }
        public DataSet ds_Subprod()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from SUB_PRODUCTS_OF  where " +
                       "  company='" + dd_company.SelectedValue.ToString() + "'  order by sdesc_of";
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
        public void Get_Dept()
        {
            DataSet ds = new DataSet();
            ds = ds_dept();
            if (ds.Tables[0].Rows.Count != 0)
            {
                //dd_dept.DataTextField = "sdesc_of";
                //dd_dept.DataValueField = "code";
                //dd_dept.DataSource = ds.Tables[0].DefaultView;
                //dd_dept.DataBind();
                //dd_dept.Items.Insert(0, new ListItem("Select Department", "0"));
            }
        }
        public DataSet ds_dept()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from DEPARTMENT_OF order by sdesc_of";
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
        public void Get_subdept()
        {
            DataSet ds = new DataSet();
            ds = ds_subdept();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_subdept.DataTextField = "sdesc_of";
                dd_subdept.DataValueField = "sub_code";
                dd_subdept.DataSource = ds.Tables[0].DefaultView;
                dd_subdept.DataBind();
                dd_subdept.Items.Insert(0, new ListItem("Select Sub Department", "0"));
            }
        }
        public DataSet ds_subdept()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from SUB_DEPARTMENT_OF  " +
                    "  order by sdesc_of ";

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

        public void Get_proj()
        {
            DataSet ds = new DataSet();
            ds = ds_proj();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_proj.DataTextField = "sdesc_of";
                dd_proj.DataValueField = "code";
                dd_proj.DataSource = ds.Tables[0].DefaultView;
                dd_proj.DataBind();
                // dd_subdept.Items.Insert(0, new ListItem("Select ", "0"));
            }
        }
        public DataSet ds_proj()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from PROJECTS_OF order by sdesc_of";
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

        protected void dd_company_SelectedIndexChanged(object sender, EventArgs e)
        {

            Get_Subprod();
        }
        #endregion

        #region selected index code
        protected void dd_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void dd_eh_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_subHead();
        }
        #endregion

        #region queries execution code
        public DataSet Mainhead()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from pc_mainhead order by description";


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
        public DataSet subhead()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string query = "select * from pc_subhead where headcode='" + dd_eh.SelectedValue.ToString() + "' order by sub_desc";
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
        public DataSet GetData()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string sqlString = "select h.company,h.year,h.month,concat(h.ID, '-', d.ID) as voucher_id,\n" +
           "       h.ID head_id,\n" +
           "       d.ID,\n" +
           "       CONVERT(varchar, d.[DATE], 103) date,\n" +
           "       s.sub_desc,\n" +
           "       m.[description] expense,d.expense hoa,\n" +
           "       d.AMOUNT,\n" +
           "       b.name branch,\n" +
           "       CASE h.express_center\n" +
           "         WHEN '0' THEN\n" +
           "          'Branch'\n" +
           "         ELSE\n" +
           "          e.name\n" +
           "       END ec,\n" +
           "       CONVERT(varchar, h.Created_ON, 106) created_on,d.status status_code, \n" +
           " case d.[status] when '1' then 'Approved' when '2' then 'Rejected' else 'Unapproved' end status,d.narrate,d.status,h.branch bcode " +
           "  ,d.[desc] ddesc,d.division,d.project,d.sub_dept,d.sub_prod from pc_head h\n" +
           " inner join PC_DETAIL d\n" +
           "    on h.ID = d.HEAD_ID\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = h.BRANCH\n" +
           "  left outer join ExpressCenters e\n" +
           "    on e.expressCenterCode = h.express_center\n" +
           " inner join pc_mainHead m\n" +
           "    on m.code = d.EXPENSE\n" +
           " inner join pc_subHead s\n" +
           "    on m.code = s.headcode\n" +
           "   and d.[desc] = s.subcode\n" +
           " where \n" +
                "  d.head_id='" + hf_headID.Value.ToString() + "' and d.id='" + hf_ID.Value.ToString() + "' ";
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
        protected void btn_add_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            int new_amount = int.Parse(txt_amount.Text);
            int sum_amount = 0;

            month_cih_check();
            double pamount = double.Parse(hf_pamount.Value.ToString()) + double.Parse(hf_old_amount.Value.ToString());
            if (pamount <= double.Parse(txt_amount.Text))
            {
                lbl_error.Text = "Your Amount Exceeds the Petty Amount!!";
                return;
            }
            else
            {
                int j = UpdateData();
                if (j == 1)
                {
                    hf_old_amount.Value = txt_amount.Text;
                    double new_petty_amount = pamount - double.Parse(txt_amount.Text);
                    lbl_error.Text = UpdatePC_remaining();
                    //update_cih_remaining(new_petty_amount);
                    month_cih_check();
                }
            }
        }
        #endregion
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
                sqlcmd.Parameters.AddWithValue("@BranchCode", hf_bcode.Value.ToString());
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
        public int update_cih_remaining(double new_petty_amount)
        {
            int i = 0; ;
            try
            {
                string sqlString = " update CIH_remainings set petty_cash=" + new_petty_amount + " " +
                    " where branch='" + hf_bcode.Value.ToString() + "' and month='" + dd_month.SelectedValue.ToString() + "' " +
                    " and year='" + dd_year.Text + "' and company='" + dd_company.SelectedValue.ToString() + "' \n";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                lbl_error.Text = "";
                i = 1;
            }
            catch (Exception Err)
            {
                i = 0;

            }
            finally
            { }
            return i;
        }
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
                #region OF Fields
                string div = "";
                string subprod = "";
                string subdept = "";
                string proj = "";

                if (dd_division.SelectedValue.ToString() != "0")
                {
                    div = dd_division.SelectedValue.ToString();
                }
                if (dd_proj.SelectedValue.ToString() != "0")
                {
                    proj = dd_proj.SelectedValue.ToString();
                }
                if (dd_sub_prod.SelectedValue.ToString() != "0")
                {
                    subprod = dd_sub_prod.SelectedValue.ToString();
                }
                if (dd_subdept.SelectedValue.ToString() != "0")
                {
                    subdept = dd_subdept.SelectedValue.ToString();
                }
                #endregion
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

                string query_head = "update PC_detail set  " + by_user + "  " + on_date + " " +
                    " amount='" + txt_amount.Text + "' " +
                    " " + status + ",narrate='" + txt_narrate.Text + "', " +
                    " division='" + div + "', " +
                    " project='" + proj + "', " +
                    " sub_dept='" + subdept + "', " +
                    " sub_prod='" + subprod + "' " +
                    " where head_id='" + hf_headID.Value.ToString() + "' " +
                    " and id='" + hf_ID.Value.ToString() + "' \n";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query_head, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                lbl_error.Text = "";
                Label1.Text = "Data Updated!!";
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
            if (hf_user.Value == "HO")
            {
                Response.Redirect("PettyCash_approval.aspx");
            }
            else if (hf_user.Value == "BRANCH")
            {
                Response.Redirect("PettyCash_branchedit.aspx");
            }
        }
    }
}