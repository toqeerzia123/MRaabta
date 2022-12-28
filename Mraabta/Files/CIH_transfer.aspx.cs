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
    public partial class CIH_transfer : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function bfunc = new bayer_Function();
        CommonFunction cfunc = new CommonFunction();
        clsVariables clsvar = new clsVariables();
        Cl_Variables cl_var = new Cl_Variables();
        double remaining_val = 0;
        double remaining_pettyCash = 0;
        double diff = 0;
        double petty_amount = 0;
        int count = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetYear();
                //Get_Month();
                dd_month.SelectedValue = DateTime.Now.ToString("MM");
                Get_Zones();
                Get_from_Companies();
                Get_to_Companies();

            }
        }
        public void GetYear()
        {

            string sqlString = "SELECT pmp.[YEAR], pm.mon MONTH, pm.ldesc MonthName FROM PC_MONTH_PERIOD pmp\n" +
            "INNER JOIN pc_month pm\n" +
            "ON pm.mon = pmp.[MONTH]\n" +
            "WHERE pmp.CLOSED_ON = (SELECT MAX(closed_ON) FROM PC_MONTH_PERIOD pmp2)\n" +
            "AND pmp.[status] = '2'";

            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dd_year.Items.Clear();
                    dd_year.DataSource = dt;
                    dd_year.DataTextField = "YEAR";
                    dd_year.DataValueField = "YEAR";
                    dd_year.DataBind();

                    dd_month.DataSource = dt;
                    dd_month.DataTextField = "MonthName";
                    dd_month.DataValueField = "Month";
                    dd_month.DataBind();
                }
            }
            catch (Exception ex)
            { }

        }

        #region dropdown calling


        public void Get_from_Companies()
        {
            DataSet ds = new DataSet();
            ds = ds_companies();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_from_company.DataTextField = "sdesc_of";
                dd_from_company.DataValueField = "code_of";
                dd_from_company.DataSource = ds.Tables[0].DefaultView;
                dd_from_company.DataBind();
            }
        }
        public void Get_to_Companies()
        {
            DataSet ds = new DataSet();
            ds = ds_companies();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_to_company.DataTextField = "sdesc_of";
                dd_to_company.DataValueField = "code_of";
                dd_to_company.DataSource = ds.Tables[0].DefaultView;
                dd_to_company.DataBind();
            }
        }
        public DataSet ds_companies()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string query = "select * from COMPANY_OF order by code_of";
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
        public DataSet GetTempData()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string sqlString = "select D.ID, CONVERT(varchar, d.DATE, 106) date ,\n" +
                "d.AMOUNT,\n" +
                "CONVERT(varchar, d.Created_ON, 106) Created_ON , pm.[desc] cash_type,d.chque_no\n" +
                " from PC_temp_cih_detail d\n" +
                " inner join PC_cash_mode pm\n" +
                " on pm.ID=d.cash_type\n" +
                "where\n";
                //"d.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "' and head_id='" + int.Parse(txt_ID.Text) + "' order by D.ID";

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
        public DataSet UserPrevEntry()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string sqlString = "select * from pc_temp_CIH_head \n" +
                "where\n" +
                "CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'  ";
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
        public DataTable CIH_remainingCash()
        {
            DataTable dt = new DataTable();
            try
            {
                string sqlString = "select 1 sno, c.*\n" +
                "  from CIH_remainings c\n" +
                 " where c.branch = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                "   and c.company = '" + dd_from_company.SelectedValue.ToString() + "'\n" +
                "   and c.month = '" + dd_month.SelectedValue.ToString() + "'\n" +
                "   and c.year = '" + dd_year.SelectedItem.ToString() + "'" +
                " union\n" +
                " select 2 sno, c.*\n" +
                "  from CIH_remainings c\n" +
                " where c.branch = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                "   and c.company = '" + dd_to_company.SelectedValue.ToString() + "'\n" +
                "   and c.month = '" + dd_month.SelectedValue.ToString() + "'\n" +
                "   and c.year = '" + dd_year.SelectedItem.ToString() + "'";


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

        public int update_CIH(double amount, string company)
        {
            int stat = 0;
            try
            {
                string query_head = " UPDATE CIH_remainings\n" +
                " SET VALUE='" + amount + "' ,CREATED_ON = GETDATE()  \n" +
                " where branch= '" + dd_branch.SelectedValue.ToString() + "'  and \n" +
                " year= '" + dd_year.SelectedItem.ToString() + "' and \n" +
                " COMPANY= '" + company + "' and \n" +
                " month='" + dd_month.SelectedValue.ToString() + "' \n" +
                " \n" +
                " ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query_head, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                hf_status.Value = "1";
                stat = 1;
                lbl_error.Text = "";
            }
            catch (Exception Err)
            {
                hf_status.Value = "";
                lbl_error.Text = "The entry is not saved due to an error!!";
                stat = 0;
            }
            finally
            { }
            return stat;
        }
        public int Insertion(double amount)
        {
            int stat = 0;
            try
            {

                string sqlString = "insert into cih_transfer\n" +
                "  (year,\n" +
                "   month,\n" +
                "   branch,\n" +
                "   from_comp,\n" +
                "   to_comp,\n" +
                "   t_date,\n" +
                "   amount,\n" +
                "   remarks,\n" +
                "   created_by,\n" +
                "   created_on)\n" +
                "values\n" +
                "  (  " +
                " '" + dd_year.SelectedItem.ToString() + "', " +
                " '" + dd_month.SelectedValue.ToString() + "', " +
                " '" + dd_branch.SelectedValue.ToString() + "', " +
                " '" + dd_from_company.SelectedValue.ToString() + "', " +
                " '" + dd_to_company.SelectedValue.ToString() + "', " +
                " '" + DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd") + "', " +
                " '" + double.Parse(txt_amount.Text) + "', " +
                " '" + txt_remarks.Text.Replace("'", "") + "', " +
                " '" + HttpContext.Current.Session["U_ID"].ToString() + "', " +
                " GETDATE() " +
                " )";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                lbl_error.Text = "";
                stat = 1;

            }
            catch (Exception Err)
            {
                lbl_error.Text = "The entry is not saved due to an error!!";
                stat = 0;
            }
            finally
            {

            }
            return stat;
        }
        public int insert_to_company_CIH_remainings(double value, double pcash)
        {
            int stat = 0;
            try
            {
                string sqlString = "insert into CIH_remainings\n" +
                "  (ID,year,\n" +
                "   month,\n" +
                "   company,\n" +
                "   branch,\n" +
                "   OPENING_VALUE,\n" +
                "   VALUE,\n" +
                "   opening_pcash,\n" +
                "   petty_cash, " +
                "   created_by, " +
                "   created_on " +
                " )\n" +
                "values\n" +
                "  ('1',\n" +
                 " '" + dd_year.SelectedItem.ToString() + "', " +
                " '" + dd_month.SelectedValue.ToString() + "', " +
                " '" + dd_to_company.SelectedValue.ToString() + "', " +
                " '" + dd_branch.SelectedValue.ToString() + "', " +
                "  '" + value + "',\n" +
                "  '" + value + "',\n" +
                "  '" + pcash + "',\n" +
                "  '" + pcash + "',\n" +
                " '" + HttpContext.Current.Session["U_ID"].ToString() + "', " +
                " GETDATE() " +
                "  )";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                lbl_error.Text = "";
                stat = 1;

            }
            catch (Exception Err)
            {
                lbl_error.Text = "The entry is not saved due to an error!!";
                stat = 0;
            }
            finally
            { }

            return stat;
        }

        #endregion

        #region ZONE AND BRANCHES
        public void Get_Zones()
        {

            DataSet ds = new DataSet();
            ds = ds_zones();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_zone.DataTextField = "name";
                dd_zone.DataValueField = "code";
                dd_zone.DataSource = ds.Tables[0].DefaultView;
                dd_zone.DataBind();
            }
            Get_Branches();
        }
        public DataSet ds_zones()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string zone = "";
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["ZONECODE"].ToString()))
                {
                    zone = "'" + HttpContext.Current.Session["ZONECODE"].ToString().Replace(",", "','") + "'";
                }
                string sqlString = "SELECT Z.zoneCode CODE,Z.name FROM ZONES Z\n" +
                "WHERE Z.zoneCode in  (" + zone + ")";
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
        protected void dd_zone_Changed(object sender, EventArgs e)
        {
            Get_Branches();

        }
        public DataSet Get_Branches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                string extraCondition = "";
                if (HttpContext.Current.Session["BranchCode"].ToString().ToUpper() == "ALL")
                {
                    extraCondition = "";
                }
                else
                {
                    extraCondition = "AND BranchCode in (" + HttpContext.Current.Session["BranchCode"].ToString() + ")";
                }
                string sql = "SELECT NAME, \n"
               + "       branchCode \n"
               + "FROM   Branches \n"
               + "WHERE  STATUS = '1' \n"
               + "       AND zonecode = '" + dd_zone.SelectedValue.ToString() + "'  " + extraCondition + "\n"
               + "                             UNION ALL \n"
               + "SELECT NAME, \n"
               + "       branchCode \n"
               + "FROM   Branches \n"
               + "WHERE  branchCode = '331' \n"
               + "       AND zonecode = '" + dd_zone.SelectedValue.ToString() + "' \n"
               + "ORDER BY \n"
               + "       NAME";

                query = "select NAME, branchCode from Branches where status = '1' \n" +
                                " AND zonecode='" + dd_zone.SelectedValue.ToString() + "'  ORDER BY NAME";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
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


        public void Get_Branches()
        {
            DataSet ds = new DataSet();
            Variable var = new Variable();
            ds = Get_Branches(var);
            dd_branch.Items.Clear();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_branch.DataTextField = "Name";
                dd_branch.DataValueField = "branchCode";
                dd_branch.DataSource = ds.Tables[0].DefaultView;
                dd_branch.DataBind();

            }
        }


        #endregion

        #region month
        public void Get_Month()
        {
            DataSet ds = new DataSet();
            ds = Months();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_month.DataTextField = "ldesc";
                dd_month.DataValueField = "mon";
                dd_month.DataSource = ds.Tables[0].DefaultView;
                dd_month.DataBind();
                calendar();
            }
        }
        public DataSet Months()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string sqlString = "select distinct m.mon,m.sdesc,m.ldesc from pc_month m\n" +
                "inner join PC_MONTH_PERIOD p\n" +
                "on m.mon=p.[month]\n" +
                "and p.[year]='" + dd_year.SelectedItem.ToString() + "'\n" +
                "and p.status='2'\n" +
                "and p.month not in (select a.[month] from  PC_MONTH_PERIOD a\n" +
                "where a.[year]='" + dd_year.SelectedItem.ToString() + "'\n" +
                "and a.status='1'\n" +
                ")";


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


        protected void btn_add_Click(object sender, EventArgs e)
        {


            //:::::::::::Amount Checking::::::::::::::::://
            double remain_amount = 0;
            DataTable dt = new DataTable();
            dt = CIH_remainingCash();
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["remaining_cash"].ToString()))
                {
                    remaining_val = double.Parse(dt.Rows[0]["remaining_cash"].ToString());

                }
                else
                {
                    remaining_val = 0;
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["petty_cash"].ToString()))
                {
                    remaining_pettyCash = double.Parse(dt.Rows[0]["petty_cash"].ToString());

                }
                else
                {
                    remaining_pettyCash = 0;

                }
            }
            hf_remaining_pettyCash.Value = remaining_pettyCash.ToString();
            hf_remaining_val.Value = remaining_val.ToString();

            diff = remaining_val - double.Parse(txt_amount.Text);
            petty_amount = double.Parse(txt_amount.Text) + remaining_pettyCash;

            hf_diff.Value = diff.ToString();
            hf_petty_amount.Value = petty_amount.ToString();

            if (remaining_val <= double.Parse(txt_amount.Text))
            {
                lbl_error.Text = "The amount you entered exceeds the available Cash In Hand!!";
                return;
            }

        }

        protected void dd_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            calendar();
        }
        protected void dd_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            calendar();
        }

        public DataSet Dates()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string table = "";

                table = "PC_date_PERIOD";

                string sqlString = "SELECT MAX(P.CLOSING_DATE) clsoing_dates FROM " + table + " P\n" +
                "WHERE P.MONTH='" + dd_month.SelectedValue.ToString() + "'\n" +
                "AND P.YEAR='" + dd_year.SelectedItem.ToString() + "'";

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
        public void calendar()
        {
            string d = "01/" + dd_month.SelectedValue.ToString() + "/" + dd_year.SelectedItem.ToString() + "";
            DateTime date_ = DateTime.Parse(d);
            DateTime date = DateTime.Now;

            #region Closing Date
            string end_date = "";
            DataSet ds_closing_Date = Dates();
            if (ds_closing_Date.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds_closing_Date.Tables[0].Rows[0][0].ToString()))
                {
                    end_date = DateTime.Parse(ds_closing_Date.Tables[0].Rows[0][0].ToString()).ToString();
                }
            }
            #endregion

            if (date.ToString("MM/yyyy") == date_.ToString("MM/yyyy"))
            {

                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                if (!string.IsNullOrEmpty(end_date))
                {
                    CalendarExtender1.StartDate = DateTime.Parse(end_date.ToString()).AddDays(1);
                    CalendarExtender1.SelectedDate = DateTime.Parse(end_date.ToString()).Date.AddDays(1);
                }
                else
                {
                    CalendarExtender1.StartDate = DateTime.Parse(firstDayOfMonth.ToString());
                    CalendarExtender1.SelectedDate = DateTime.Parse(firstDayOfMonth.ToString()).Date;
                }
                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender1.SelectedDate = DateTime.Now.Date;
            }
            else
            {

                //last date of month
                DateTime firstOfNextMonth = new DateTime(date_.Year, date_.Month, 1).AddMonths(1);
                DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);


                var firstDayOfMonth = new DateTime(date_.Year, date_.Month, 1);
                //var lastDayOfMonth = ;
                if (!string.IsNullOrEmpty(end_date))
                {
                    CalendarExtender1.StartDate = DateTime.Parse(end_date.ToString()).AddDays(1);
                    CalendarExtender1.SelectedDate = DateTime.Parse(end_date.ToString()).Date.AddDays(1);
                }
                else
                {
                    CalendarExtender1.StartDate = DateTime.Parse(firstDayOfMonth.ToString());
                    CalendarExtender1.SelectedDate = DateTime.Parse(firstDayOfMonth.ToString()).Date;
                }
                CalendarExtender1.EndDate = lastOfThisMonth;


            }
        }



        protected void btn_submit_click(object sender, EventArgs e)
        {
            lbl_message.Text = "";
            lbl_error.Text = "";

            //From Company & To Company cannot be same
            if (dd_from_company.SelectedValue.ToString() != dd_to_company.SelectedValue.ToString())
            {
                //:::::::::::Amount Checking::::::::::::::::://
                double remain_amount = 0;
                DataTable dt = new DataTable();
                dt = CIH_remainingCash();
                if (dt.Rows.Count > 0)
                {
                    //Checking For Entries Of Source Company
                    DataRow[] dr_from = dt.Select("sno=1");
                    if (dr_from.Count() > 0)
                    {
                        double from_value = 0;
                        double from_value_update = 0;

                        double to_value = 0;
                        double to_value_update = 0;
                        double p_cash = 0;

                        //check remaining CIH from Source Company
                        from_value = double.Parse(dr_from[0]["value"].ToString());
                        if (double.Parse(txt_amount.Text) <= from_value)
                        {
                            //Update remaining value of source company for later update in the table
                            from_value_update = from_value - double.Parse(txt_amount.Text);

                            //Checking If To Company Entry is Already Present In CIH remaining table
                            DataRow[] dr_to = dt.Select("sno=2");
                            if (dr_to.Count() > 0)
                            {
                                to_value = double.Parse(dr_to[0]["value"].ToString());
                                to_value_update = to_value + double.Parse(txt_amount.Text);

                                //Updating Entries In The Data Base
                                int i1 = Insertion(to_value_update);
                                if (i1 == 1)
                                {
                                    int i2 = update_CIH(to_value_update, dd_to_company.SelectedValue.ToString());
                                    if (i2 == 1)
                                    {
                                        int i3 = update_CIH(from_value_update, dd_from_company.SelectedValue.ToString());
                                        if (i3 == 1)
                                        {
                                            lbl_message.Text = "CIH Transferred Successfully!!";
                                        }
                                    }
                                    lbl_message.Text = "CIH Transferred Successfully!!";
                                    lbl_error.Text = UpdateCIH_remaining();
                                    lbl_error.Text += UpdatePC_remaining();
                                }

                            }
                            else
                            {
                                to_value_update = double.Parse(txt_amount.Text);
                                int i1 = Insertion(to_value_update);
                                if (i1 == 1)
                                {
                                    int i2 = insert_to_company_CIH_remainings(to_value_update, p_cash);
                                    if (i2 == 1)
                                    {
                                        int i3 = update_CIH(from_value_update, dd_from_company.SelectedValue.ToString());
                                        if (i3 == 1)
                                        {
                                            lbl_message.Text = "CIH Transferred Successfully!!";
                                        }
                                    }
                                    lbl_message.Text = "CIH Transferred Successfully!!";
                                }
                                lbl_error.Text = UpdateCIH_remaining();
                                lbl_error.Text += UpdatePC_remaining();
                            }
                        }
                        else
                        {
                            lbl_error.Text = "There is no amount found in Company: " + dd_from_company.SelectedItem.ToString() + "";
                            return;
                        }

                    }
                    else
                    {
                        lbl_error.Text = "There is no amount found in Company: " + dd_from_company.SelectedItem.ToString() + "";
                        return;
                    }

                }
                else
                {
                    lbl_error.Text = "There is no amount found in Company: " + dd_from_company.SelectedItem.ToString() + "";
                    return;
                }
                lbl_error.Text = "";
            }
            else
            {
                lbl_error.Text = "From Company & To Company Cannot Be Same!!";
                return;
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
                DateTime first_day = new DateTime(int.Parse(dd_year.SelectedItem.ToString()), int.Parse(dd_month.SelectedValue.ToString()), 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("UpdatePettyCashBalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter SParam = new SqlParameter("Result", SqlDbType.VarChar);
                SParam.Direction = ParameterDirection.Output;
                SParam.Size = 500;
                sqlcmd.Parameters.AddWithValue("@BranchCode", dd_branch.SelectedValue.ToString());
                sqlcmd.Parameters.AddWithValue("@StartDate", first_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@EndDate", last_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@Month", dd_month.SelectedValue.ToString());
                sqlcmd.Parameters.AddWithValue("@Year", dd_year.SelectedItem.ToString());
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
                DateTime first_day = new DateTime(int.Parse(dd_year.SelectedItem.ToString()), int.Parse(dd_month.SelectedValue.ToString()), 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("UpdateCashInHandBalance", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter SParam = new SqlParameter("Result", SqlDbType.VarChar);
                SParam.Direction = ParameterDirection.Output;
                SParam.Size = 500;
                sqlcmd.Parameters.AddWithValue("@BranchCode", dd_branch.SelectedValue.ToString());
                sqlcmd.Parameters.AddWithValue("@StartDate", first_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@EndDate", last_day.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@Month", dd_month.SelectedValue.ToString());
                //  sqlcmd.Parameters.AddWithValue("@Year", dd_year.SelectedItem.ToString());
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