using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;

namespace MRaabta.Files
{
    public partial class CIH2BankVoucher : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        Cl_Receipts rec = new Cl_Receipts();
        LoadingPrintReport enc_ = new LoadingPrintReport();
        bayer_Function bfunc = new bayer_Function();
        Variable var = new Variable();

        DataTable dt = new DataTable();
        string sqlString;

        #region values initialization
        double remaining_val = 0;
        double remaining_pettyCash = 0;
        double diff = 0;
        double petty_amount = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string temp = HttpContext.Current.Session["BranchCode"].ToString();
            }
            catch (Exception ex)
            {

                Errorid.Text = "Session Expired. Please Re-Login";
                Errorid.ForeColor = Color.Red;
                return;
            }


            if (!IsPostBack)
            {
                Errorid.Text = "";
                Banks();
                //Get_Branches();
                Get_Zones();
                Get_Companies();
                //Get_Month();
                GetYear();

                ViewState["temp"] = null;





            }
        }

        protected void naturedeposit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Label1.Text = "You Selected: " + DropDownList1.SelectedItem.Text;

            if (dd_nature.SelectedValue == "01")
            {
                txt_dslipNo.Text = "";
                lbl_totalamount.Text = "";
                txt_rrdate.Text = "";
                txt_amount.Text = "";
            }

            if (dd_nature.SelectedValue == "02")
            {
                txt_dslipNo.Text = "";
                lbl_totalamount.Text = "";
                txt_rrdate.Text = "";
                txt_amount.Text = "";
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

                string sqlString = "SELECT Z.zoneCode CODE,Z.name FROM ZONES Z WHERE Z.zoneCode in  (" + zone + ") ORDER BY Z.NAME ASC";

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

            txt_rrdate.Text = "";
            lbl_totalamount.Text = "";
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
                    calendar();
                }
            }
            catch (Exception ex)
            { }

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
        public void calendar__()
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
                CalendarExtender1.EndDate = DateTime.Now.Date;
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

        public void calendar()
        {
            //string d = "01/" + dd_month.SelectedValue.ToString() + "/" + dd_year.SelectedItem.ToString() + "";    //comment by fahad 24 sept 20
            string d = $"{dd_year.SelectedItem.ToString()}/{dd_month.SelectedValue.ToString()}/01";
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
                CalendarExtender1.EndDate = DateTime.Now.Date;
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

            if (txt_rrdate.Text != "" && txt_rrdate.Text != null)
            {
                txt_chequeDate.Text = (DateTime.Parse(txt_rrdate.Text)).ToString("yyyy-MM-dd");
                CalendarExtender1.SelectedDate = DateTime.Parse(txt_rrdate.Text);
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


        public DataTable GetBanks()
        {
            // string query = "select * from Banks_of";

            string query = "select * from Banks_of " + clvar.Company;

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
        protected void Banks()
        {
            if (Session["U_ID"].ToString() == "5163" || Session["U_ID"].ToString() == "5963")
            {
                clvar.Company = " where status in ('0','1') \n";
            }
            else
            {
                clvar.Company = " where status = '1' \n";
            }

            DataTable dt = GetBanks();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    // DataTable mnpBanks = dt;//.Select("isMNPBank = TRUE").CopyToDataTable();
                    dd_depositSlipBank.DataSource = dt;
                    dd_depositSlipBank.DataTextField = "bank_name";
                    dd_depositSlipBank.DataValueField = "bank_code";
                    dd_depositSlipBank.DataBind();
                }
            }
        }

        public float Get_cih()
        {
            float amount = 0;
            DataSet ds = new DataSet();
            try
            {
                float remain_amount = 0;
                string remaininig_date = "";
                DataSet ds_remain = Get_Remaings();
                amount = float.Parse(ds_remain.Tables[0].Rows[0]["remaining_amount"].ToString());

                if (ds_remain.Tables[0].Rows.Count > 0)
                {
                    remaininig_date = DateTime.Parse(ds_remain.Tables[0].Rows[0]["chequedate"].ToString()).ToString("yyyy-MM-dd");
                }

                string sqlString = "SELECT isnull(SUM(pv.Amount),0) TotalAmount,'0' ChequeDate\n" +
                "FROM   CIH_balance pv\n" +
                "WHERE  pv.BranchCode = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                " and pv.voucherdate between '" + remaininig_date + "' AND '" + txt_chequeDate.Text + "'  ";

                //Query To Be Run On First Time
                //string sqlString = "SELECT SUM(pv.Amount) TotalAmount, pv.BranchCode\n" +
                //"FROM   PaymentVouchers pv\n" +
                //"WHERE  pv.BranchCode = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                //"       AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1')\n" +
                //"          AND pv.CashPaymentSource != '4' AND CAST(pv.VoucherDate AS DATE) <= '2018-03-31'\n" +
                //"GROUP BY pv.BranchCode";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TotalAmount"].ToString()))
                        amount = float.Parse(ds.Tables[0].Rows[0]["TotalAmount"].ToString()) + amount;
                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }
            return amount;
        }
        public int Insert_Entry()
        {

            try
            {
                dt = (DataTable)ViewState["temp"];
                #region sqlString___15052019
                /*
                string sqlString___15052019 = "insert into Deposit_to_bank(ChequeDate,Amount,branchcode,createdon,\n" +
                "createdby,depositslipno,depositslipbankid,company,zonecode,year,month)\n" +
                "values\n" +
                "(\n" +
                " '" + txt_chequeDate.Text + "', " +
                " '" + float.Parse(txt_amount.Text) + "', " +
                " '" + dd_branch.SelectedValue.ToString() + "', " +
                " GETDATE(), " +
                " '" + HttpContext.Current.Session["U_ID"].ToString() + "', " +
                " '" + txt_dslipNo.Text + "', " +
                " '" + dd_depositSlipBank.SelectedValue.ToString() + "','" + dd_company.SelectedValue.ToString() + "','" + dd_zone.SelectedValue.ToString() + "', " +
                "'" + dd_year.SelectedItem.ToString() + "','" + dd_month.SelectedValue.ToString() + "' " +
                ")";

                int mont = DateTime.Parse(txt_chequeDate.Text).Month;
                int year = DateTime.Parse(txt_chequeDate.Text).Year;

                string sqlString111 = "update CIH_remainings set  value='" + double.Parse(hf_diff.Value.ToString()) + "'\n" +
                " where\n" +
                " [month]='" + dd_month.SelectedValue.ToString() + "' and\n" +
                " [year]='" + dd_year.SelectedItem.ToString() + "' and\n" +
                " [COMPANY]='" + dd_company.SelectedValue.ToString() + "' and\n" +
                " [branch]='" + dd_branch.SelectedValue.ToString() + "'";
                */

                #endregion



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sqlString = "insert into Deposit_to_bank(ChequeDate,Amount,branchcode,createdon,\n" +
                    "createdby,depositslipno,depositslipbankid,company,zonecode,year,month,NatureDeposit,RRDATE)\n" +
                    "values\n" +
                    "(\n" +
                    //  " '" + txt_chequeDate.Text + "', " +
                    // " '" + float.Parse(txt_amount.Text) + "', " +
                    " '" + dt.Rows[i][2].ToString() + "', " +
                    " '" + dt.Rows[i][1].ToString() + "', " +
                    " '" + dd_branch.SelectedValue.ToString() + "', " +
                    " GETDATE(), " +
                    " '" + HttpContext.Current.Session["U_ID"].ToString() + "', " +
                    // " '" + txt_dslipNo.Text + "', " +
                    " '" + dt.Rows[i][0].ToString() + "', " +
                    " '" + dt.Rows[i][3].ToString() + "', " +
                    //" '" + dd_depositSlipBank.SelectedValue.ToString() + "'," +
                    "'" + dd_company.SelectedValue.ToString() + "','" + dd_zone.SelectedValue.ToString() + "', " +
                    "'" + dd_year.SelectedItem.ToString() + "','" + dd_month.SelectedValue.ToString() + "', " +
                    "'" + dd_nature.SelectedValue.ToString() + "', " +
                    "'" + txt_rrdate.Text + "' " +
                    ")";



                    int mont = DateTime.Parse(txt_chequeDate.Text).Month;
                    int year = DateTime.Parse(txt_chequeDate.Text).Year;

                    string sqlString1 = "update CIH_remainings set  value='" + double.Parse(hf_diff.Value.ToString()) + "'\n" +
                    " where\n" +
                    " [month]='" + dd_month.SelectedValue.ToString() + "' and\n" +
                    " [year]='" + dd_year.SelectedItem.ToString() + "' and\n" +
                    " [COMPANY]='" + dd_company.SelectedValue.ToString() + "' and\n" +
                    " [branch]='" + dd_branch.SelectedValue.ToString() + "'";


                    SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                    orcl.Open();
                    SqlCommand orcd = new SqlCommand(sqlString, orcl);
                    orcd.CommandType = CommandType.Text;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    orcd.ExecuteNonQuery();

                    //update query
                    //SqlCommand orcd1 = new SqlCommand(sqlString1, orcl);
                    //orcd1.CommandType = CommandType.Text;
                    //SqlDataAdapter oda1 = new SqlDataAdapter(orcd1);
                    //orcd1.ExecuteNonQuery();

                    orcl.Close();
                    Errorid.Text = "";
                }

                return 1;
                // }
            }
            catch (Exception Err)
            {
                Errorid.Text = "The entry is not saved due to an error!!";
                return 0;
            }
            finally
            {
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {

            #region date check
            try
            {
                DateTime.Parse(txt_chequeDate.Text);
            }
            catch
            {
                lbl_err2.Text = "Date is Invalid!!";
                return;
            }
            /*
            txt_chequeDate.Text = DateTime.Parse(txt_chequeDate.Text).ToString("yyyy-MM-dd");
            if (DateTime.Parse(DateTime.Parse(txt_chequeDate.Text).ToString("yyyy-MM-dd")) >= CalendarExtender1.StartDate && DateTime.Parse(DateTime.Parse(txt_chequeDate.Text).ToString("yyyy-MM-dd")) <= CalendarExtender1.EndDate)
            {
            }
            else
            {
                lbl_err2.Text = "Date is not Under specified range!!";
                return;
            }
             */
            #endregion

            DateTime rrdate = DateTime.Parse(txt_rrdate.Text);
            DateTime chequedate = DateTime.Parse(txt_chequeDate.Text);

            if (rrdate > chequedate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('RR Date is Less Than Deposit Date')", true);
                return;
            }

            double grandTotal = double.Parse(lbl_totalamount.Text.Replace(",", "")) - double.Parse(lbl_grandamount.Text);

            if (grandTotal != 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('RR Total Amount and DSlip Total Amount not Same')", true);
                return;
            }


            if (dd_depositSlipBank.SelectedValue.ToString() == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Bank')", true);
                return;
            }

            clvar.Branch = dd_branch.SelectedValue.ToString();
            DataTable dates = MinimumDate(clvar);
            if (dates != null)
            {
                if (dates.Rows[0][0].ToString().Trim() != "")
                {
                    DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
                    DateTime maxAllowedDate = DateTime.Now;
                    if (DateTime.Parse(txt_chequeDate.Text) < minAllowedDate || DateTime.Parse(txt_chequeDate.Text) > maxAllowedDate)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Deposit Dates.')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End Has never been performed for your Branch. Contact Head Office Accounts Department.')", true);

                    return;
                }
            }
            else
            {

            }


            btn_save.Visible = false;

            lbl_message.Text = "";
            lbl_err2.Text = "";
            int chk = check_remainings();
            if (chk == 1)
            {
                try
                {
                    int i = Insert_Entry();
                    if (i == 1)
                    {
                        Errorid.Text += UpdateCIH_remaining();
                        //      Errorid.Text += UpdatePC_remaining();
                        lbl_message.Text = "The Transaction Is Saved Successfully";

                        ViewState["temp"] = null;
                        GridView.DataSource = null;
                        GridView.DataBind();
                        txt_dslipNo.Text = "";
                        txt_amount.Text = "";
                        lbl_count.Text = "";
                        txt_rrdate.Text = "";
                        lbl_totalamount.Text = "";
                        lbl_grandamount.Text = "";
                        divtotal.Visible = false;
                        txt_rrdate.Enabled = true;

                        btn_save.Visible = true;
                    }
                    else
                    {
                        if (Errorid.Text == "")
                        {
                            Errorid.Text = "The Entry Is Not Saved Due To Error!!";
                        }
                        Errorid.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {

                    Errorid.Text = "The Entry Is Not Saved Due To Error!!";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                }
            }

        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void ResetAll()
        {
            txt_amount.Text = "";
            txt_dslipNo.Text = "";
            txt_rrdate.Text = "";
            txt_chequeDate.Text = "";
            lbl_totalamount.Text = "";



        }

        protected void txt_cnNo_TextChanged(object sender, EventArgs e)
        {
            //   clvar.consignmentNo = txt_cnNo.Text;
            DataTable dt = GetConsignmentDetail(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["PRESENT"].ToString().Trim(' ') != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher No.# " + dt.Rows[0]["VoucherID"].ToString() + " Already Created for CN Number " + clvar.consignmentNo + "')", true);

                        txt_amount.Text = "";
                        return;
                    }

                    //txt_amount.Text = dt.Rows[0]["TotalAmount"].ToString();

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
                    txt_amount.Text = "";
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
                txt_amount.Text = "";
                return;
            }
        }
        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
        }

        public DataSet Get_Remaings()
        {
            float amount = 0;
            DataSet ds = new DataSet();
            try
            {


                string sqlString = " select pd.remaining_amount,MAX(pd.ChequeDate) chequedate from deposit_to_bank pd\n" +
                "where  pd.BranchCode = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                " and  pd.ChequeDate <= '" + txt_chequeDate.Text + "'   \n" +
                "  group by pd.remaining_amount";



                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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
        public DataTable CIH_remainingCash()
        {
            DataTable dt = new DataTable();
            try
            {
                int mont = DateTime.Parse(txt_chequeDate.Text).Month;
                int year = DateTime.Parse(txt_chequeDate.Text).Year;
                string sqlString__ = "select c.[VALUE] remaining_cash,petty_cash from CIH_remainings c\n" +
                "where\n" +
                "c.month='" + mont + "'\n" +
                "and c.year='" + year + "'\n" +
                "and c.company='" + dd_company.SelectedValue.ToString() + "'\n" +
                "and c.branch='" + dd_branch.SelectedValue.ToString() + "'";



                string sqlString = @"DECLARE @YEAR VARCHAR(4), @MONTH VARCHAR(2), @BRANCH VARCHAR(5), @COMPANY VARCHAR(5)
                SET @YEAR = '" + year + @"'
                SET @MONTH ='"+ mont + @"'
                SET @BRANCH ='"+ dd_branch.SelectedValue.ToString() + @"'
                SET @COMPANY = '" + dd_branch.SelectedValue.ToString() + @"'


                SELECT X.[YEAR],X.[MONTH], 
                X.Region, 
                X.zone, 
                X.BRANCH, 
                (SUM(OPENING_VALUE) +SUM(DNOTE))- SUM(CNOTE) remaining_cash,
                0 petty_cash
                FROM (
                SELECT DISTINCT A.RANK,
		                CONVERT(VARCHAR, A.DATE, 103)     DATE,
		                A.CASHTYPE,
		                A.DEBIT                           DNOTE,
		                A.CREDIT                          CNOTE,
		                A.Region,
		                A.zone,
		                A.BRANCH,
		                COMPANY, 0 OPENING_VALUE, A.[YEAR], A.[MONTH]
                FROM   (
			                SELECT '1' RANK,
					                SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0)) DEBIT,
					                0               CREDIT,
					                CASE WHEN PV.REFNO IS NULL OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '') + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '') + ')' + ' REF NO# '+PV.REFNO END CASHTYPE, 
					                VOUCHERDATE     DATE,
					                z.Region,
					                z.name zone,
					                B.[NAME]        BRANCH,
					                '' COMPANY,
					                YEAR(pv.VoucherDate) YEAR,
					                MONTH(pv.VoucherDate) MONTH
			                FROM   PAYMENTVOUCHERS PV
					                LEFT OUTER JOIN PAYMENTTYPES PT ON  PT.ID = PV.PAYMENTTYPEID
					                LEFT OUTER JOIN (
							                SELECT MP.VOUCHERID,
									                SUM(MP.AMOUNT) PRODUCTAMOUNT
							                FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN MP
							                WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD')
							                GROUP BY
									                MP.VOUCHERID
						                ) MM
						                ON  MM.VOUCHERID = PV.ID
					                INNER JOIN BRANCHES B ON  B.BRANCHCODE = PV.BRANCHCODE
					                INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
	                WHERE YEAR(PV.VOUCHERDATE) = @YEAR
	                AND MONTH(PV.VOUCHERDATE) = @MONTH
	                AND PV.BRANCHCODE = @BRANCH
		                AND PV.CASHPAYMENTSOURCE IS NOT NULL
					                AND (PV.PAYMENTSOURCEID IS NULL OR PV.PAYMENTSOURCEID = '1')
					                AND PV.CASHPAYMENTSOURCE != '4'
			                GROUP BY
					                VOUCHERDATE, PV.REFNO,
					                PT.NAME,
					                B.[NAME],    
					                z.Region,
					                z.name,              
					                YEAR(pv.VoucherDate),
					                MONTH(pv.VoucherDate)


			                UNION ALL


			                SELECT '1' RANK,
					                SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0)) DEBIT,
					                0               CREDIT,
					                CASE WHEN PV.REFNO IS NULL OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '') + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '') + ')' + ' REF NO# '+PV.REFNO END CASHTYPE, 
					                VOUCHERDATE     DATE,
					                z.Region,
					                z.name zone,
					                B.[NAME]        BRANCH,
					                '' COMPANY,
					                YEAR(pv.VoucherDate) YEAR,
					                MONTH(pv.VoucherDate) MONTH
			                FROM   PAYMENTVOUCHERS PV
					                LEFT OUTER JOIN (
							                SELECT MP.VOUCHERID,
									                SUM(MP.AMOUNT) PRODUCTAMOUNT
							                FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN MP
							                WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD')
							                GROUP BY
									                MP.VOUCHERID
						                ) MM
						                ON  MM.VOUCHERID = PV.ID
					                LEFT OUTER JOIN PAYMENTTYPES PT ON  PT.ID = PV.PAYMENTTYPEID
					                INNER JOIN BRANCHES B ON  B.BRANCHCODE = PV.BRANCHCODE					  
					                INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
			                WHERE YEAR(PV.VOUCHERDATE) = @YEAR
				                AND MONTH(PV.VOUCHERDATE) = @MONTH
				                AND PV.BRANCHCODE = @BRANCH
				                AND PV.CREDITCLIENTID IS NOT NULL
				                AND (PV.PAYMENTSOURCEID IS NULL OR PV.PAYMENTSOURCEID = '1') 
			                GROUP BY
					                VOUCHERDATE, PV.REFNO,
					                PT.NAME,
					                B.[NAME],
					                z.Region,
					                z.name,
					                YEAR(pv.VoucherDate),
					                MONTH(pv.VoucherDate)
                   
			                UNION ALL


			                SELECT '1' RANK,
					                SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0)) DEBIT,
					                0               CREDIT,
					                CASE WHEN PV.REFNO IS NULL OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '') + ')'  ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '') + ')' + ' REF NO# '+PV.REFNO END CASHTYPE, 
					                VOUCHERDATE     DATE,
					                z.Region,
					                z.name zone,
					                B.[NAME]        BRANCH,
					                '' COMPANY,
					                YEAR(pv.VoucherDate) YEAR,
					                MONTH(pv.VoucherDate) MONTH
			                FROM   PAYMENTVOUCHERS PV
					                LEFT OUTER JOIN (
							                SELECT MP.VOUCHERID,
									                SUM(MP.AMOUNT) PRODUCTAMOUNT
							                FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN MP
							                WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD')
							                GROUP BY
									                MP.VOUCHERID
						                ) MM
						                ON  MM.VOUCHERID = PV.ID
					                LEFT OUTER JOIN PAYMENTTYPES PT ON  PT.ID = PV.PAYMENTTYPEID
					                INNER JOIN BRANCHES B ON B.BRANCHCODE = PV.BRANCHCODE					  
					                INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
			                WHERE YEAR(PV.VOUCHERDATE) = @YEAR
				                AND MONTH(PV.VOUCHERDATE) = @MONTH
				                AND PV.BRANCHCODE = @BRANCH
				                AND PV.CREDITCLIENTID IS NULL
				                AND PV.CLIENTGROUPID IS NOT NULL
				                AND (PV.PAYMENTSOURCEID IS NULL OR PV.PAYMENTSOURCEID = '1') 
			                GROUP BY
					                VOUCHERDATE, PV.REFNO, 
					                PT.NAME,
					                B.[NAME],        
					                z.Region,
					                z.name,          
					                YEAR(pv.VoucherDate),
					                MONTH(pv.VoucherDate) 
                  
                  
			                UNION ALL

			                SELECT '2' RANK,
					                CASE
						                WHEN SUM(PV.AMOUNT) < 0 THEN (SUM(PV.AMOUNT) * -1)
						                ELSE 0
					                END               DEBIT,
					                CASE
						                WHEN SUM(PV.AMOUNT) < 0 THEN 0
						                ELSE SUM(PV.AMOUNT)
					                END               CREDIT,
					                CASE WHEN (SUM(PV.AMOUNT) < 0 OR PV.LDESC IS NOT NULL)  THEN PV.LDESC ELSE 
					                'DEPOSIT TO BANK: ' + ISNULL(BO.BANK_NAME,'') + ' (DSLIP NO: ' + ISNULL(PV.DEPOSITSLIPNO,'') 
					                + ')' END CASHTYPE, 
					                PV.CHEQUEDATE     DATE,
					                z.Region,
					                z.name zone,
					                B.[NAME]          BRANCH,
					                '' COMPANY,                  
					                YEAR(pv.CHEQUEDATE) YEAR,
					                MONTH(pv.CHEQUEDATE) MONTH
			                FROM   DEPOSIT_TO_BANK PV
					                INNER JOIN BRANCHES B ON B.BRANCHCODE = PV.BRANCHCODE					  
					                INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
					                LEFT OUTER JOIN BANKS_OF BO ON  BO.BANK_CODE = PV.DEPOSITSLIPBANKID
			                WHERE PV.COMPANY= @COMPANY
				                AND YEAR(PV.CHEQUEDATE) = @YEAR
				                AND MONTH(PV.CHEQUEDATE) = @MONTH
				                AND PV.BRANCHCODE = @BRANCH
			                GROUP BY
					                CHEQUEDATE,
					                B.[NAME],
					                PV.ID,
					                BO.BANK_NAME,
					                z.Region,
					                z.name,
					                PV.DEPOSITSLIPNO, PV.LDESC, 
					                YEAR(pv.CHEQUEDATE),
					                MONTH(pv.CHEQUEDATE)


			                UNION ALL

			                SELECT '2' RANK,
					                CASE
						                WHEN SUM(PV.AMOUNT) < 0 THEN (SUM(PV.AMOUNT) * -1)
						                ELSE 0
					                END                       DEBIT,
					                CASE
						                WHEN SUM(PV.AMOUNT) < 0 THEN 0
						                ELSE SUM(PV.AMOUNT)
					                END                       CREDIT,
					                'TRANSFERRED TO PETTY CASH (FROM : ' + PC.[DESC] + ')'
					                CASHTYPE,
					                PV.[DATE]                 DATE,
					                z.Region,
					                z.name zone,
					                B.[NAME]                  BRANCH,
					                CM.SDESC_OF               COMPANY,
					                YEAR(pv.date) YEAR,
					                MONTH(pv.date) MONTH
			                FROM   PC_CIH_DETAIL PV
					                INNER JOIN PC_CIH_HEAD PH ON  PH.ID = PV.HEAD_ID
					                INNER JOIN BRANCHES B ON  B.BRANCHCODE = PH.BRANCH					  
					                INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
					                INNER JOIN PC_CASH_MODE PC ON  PC.ID = PV.CASH_TYPE
						                AND PV.CASH_TYPE = '1'
						                AND PC.ID = '1'
					                INNER JOIN COMPANY_OF CM ON  CM.CODE_OF = PH.COMPANY
					                AND PH.COMPANY= @COMPANY
			                WHERE YEAR(PV.[DATE]) = @YEAR
				                AND MONTH(PV.[DATE]) = @MONTH
				                AND PH.BRANCH = @BRANCH
			                GROUP BY
					                [DATE],
					                B.[NAME],
					                PC.[DESC],
					                CM.SDESC_OF,
					                z.Region,
					                z.name,
					                YEAR(pv.date),
					                MONTH(pv.date)
                  
			                UNION ALL
           
			                --FROM COMPANY
			                SELECT '2' RANK,
					                CASE
						                WHEN SUM(CH.AMOUNT) < 0 THEN (SUM(CH.AMOUNT) * -1)
						                ELSE 0
					                END              DEBIT,
					                CASE
						                WHEN SUM(CH.AMOUNT) < 0 THEN 0
						                ELSE SUM(CH.AMOUNT)
					                END              CREDIT,
					                'TRANSFERED TO  ' + ISNULL(CM1.SDESC_OF,'') + ' REMARKS: '+CH.REMARKS CASHTYPE,
					                CH.T_DATE        DATE,
					                z.Region,
					                z.name zone,
					                B.[NAME]         BRANCH,
					                CM1.SDESC_OF     COMPANY,
					                YEAR(CH.T_DATE) YEAR,
					                MONTH(CH.T_DATE) MONTH
			                FROM   CIH_TRANSFER CH
					                INNER JOIN BRANCHES B ON  B.BRANCHCODE = CH.BRANCH					  
					                INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
					                INNER JOIN COMPANY_OF CM ON  CM.CODE_OF = CH.FROM_COMP
					                INNER JOIN COMPANY_OF CM1 ON  CM1.CODE_OF = CH.TO_COMP
			                WHERE YEAR(CH.T_DATE) = @YEAR
				                AND MONTH(CH.T_DATE) = @MONTH
				                AND CH.BRANCH = @BRANCH
				                AND CH.FROM_COMP= @COMPANY
			                GROUP BY
					                CH.T_DATE,
					                B.[NAME],
					                CM1.SDESC_OF,CH.REMARKS ,
					                YEAR(CH.T_DATE),
					                z.Region,
					                z.name,
					                MONTH(CH.T_DATE)

			                UNION ALL
           
			                --TO COMPANY
			                SELECT '1' RANK,
					                CASE
						                WHEN SUM(CH.AMOUNT) < 0 THEN 0
						                ELSE SUM(CH.AMOUNT)
					                END             DEBIT,
					                CASE
						                WHEN SUM(CH.AMOUNT) < 0 THEN (SUM(CH.AMOUNT) * -1)
					                END             CREDIT,
					                'RECEIVED FROM  ' + ISNULL(CM.SDESC_OF,'') + ' REMARKS: '+CH.REMARKS CASHTYPE,
					                CH.T_DATE       DATE,
					                z.Region,
					                z.name zone,
					                B.[NAME]        BRANCH,
					                CM.SDESC_OF     COMPANY,
					                YEAR(CH.T_DATE) YEAR,
					                MONTH(CH.T_DATE) MONTH
			                FROM   CIH_TRANSFER CH
					                INNER JOIN BRANCHES B ON  B.BRANCHCODE = CH.BRANCH					  
					                INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
					                LEFT OUTER  JOIN COMPANY_OF CM ON  CM.CODE_OF = CH.FROM_COMP
					                INNER JOIN COMPANY_OF CM1 ON  CM1.CODE_OF = CH.TO_COMP
			                WHERE YEAR(CH.T_DATE) = @YEAR
				                AND MONTH(CH.T_DATE) = @MONTH
				                AND CH.BRANCH = @BRANCH
				                AND CH.TO_COMP= @COMPANY
			                GROUP BY
					                CH.T_DATE,
					                B.[NAME],
					                CM.SDESC_OF, CH.REMARKS,
					                z.Region,
					                z.name,
					                YEAR(CH.T_DATE),
					                MONTH(CH.T_DATE)
		                ) A    


                        UNION ALL
                        (
		                  SELECT 
		                        NULL RANK,
		                        NULL     DATE,
		                        NULL CASHTYPE,
		                        NULL                         DNOTE,
		                        NULL                         CNOTE,
		                        z.Region,
		                        z.name zone,
		                        B.NAME BRANCH,
		                        CR.COMPANY,
		                        CR.OPENING_VALUE, cr.[year], CR.[month]
	                        FROM CIH_REMAININGS AS CR 
	                        INNER JOIN BRANCHES AS B ON CR.BRANCH = B.BRANCHCODE
	                        INNER JOIN Zones AS z ON b.zoneCode = z.zoneCode
	                        WHERE 
		                        CR.[YEAR] = @YEAR 
		                        AND CR.[MONTH] = @MONTH
		                        AND CR.BRANCH IN (@BRANCH)  
		                        AND CR.COMPANY = '1'    
		                        )
                ) X     
                GROUP BY
                X.[YEAR],X.[MONTH],X.BRANCH, X.Region, X.zone  ";

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
        public int check_remainings()
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

            // diff = remaining_val - double.Parse(txt_amount.Text);
            // petty_amount = double.Parse(txt_amount.Text) + remaining_pettyCash;

            diff = remaining_val - double.Parse(lbl_grandamount.Text);
            petty_amount = double.Parse(lbl_grandamount.Text) + remaining_pettyCash;

            // lbl_grandamount.Text


            hf_diff.Value = diff.ToString();
            hf_petty_amount.Value = petty_amount.ToString();

            //if (remaining_val <= double.Parse(txt_amount.Text)) // Changed on 02-04-2019
            //if (remaining_val < double.Parse(txt_amount.Text))
            if (remaining_val < double.Parse(lbl_grandamount.Text))
            {
                lbl_err2.Text = "The amount you entered exceeds the available Cash In Hand!!";
                return 0;
            }
            return 1;
        }

        protected void RRDate_TextChanged(object sender, EventArgs e)
        {
            clvar.remarks = txt_rrdate.Text;

            DataSet ds = Get_Sum_of_RR_Data();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["dnote"].ToString() != "")
                {
                    double a = double.Parse(ds.Tables[0].Rows[0]["dnote"].ToString());
                    lbl_totalamount.Text = string.Format("{0:N0}", a);// double.Parse(ds.Tables[0].Rows[0]["dnote"].ToString()).ToString("{0:N0}");
                                                                      //CalendarExtender1.SelectedDate = DateTime.Parse(txt_rrdate.Text);
                }
                else
                {
                    lbl_totalamount.Text = "";
                }
            }
            else
            {

            }


        }

        protected void chequeDate_TextChanged(object sender, EventArgs e)
        {
            DateTime rrdate = DateTime.Parse(txt_rrdate.Text);
            DateTime chequedate = DateTime.Parse(txt_chequeDate.Text);

            if (rrdate > chequedate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('RR Date is Less Than Deposit Date')", true);
                return;
            }

            clvar.Branch = dd_branch.SelectedValue.ToString();

            DataTable dates = MinimumDate(clvar);
            if (dates != null)
            {
                if (dates.Rows[0][0].ToString().Trim() != "")
                {
                    DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
                    DateTime maxAllowedDate = DateTime.Now;
                    if (DateTime.Parse(txt_chequeDate.Text) < minAllowedDate || DateTime.Parse(txt_chequeDate.Text) > maxAllowedDate)
                    {
                        txt_chequeDate.Text = "";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Deposit Dates.')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End Has never been performed for your Branch. Contact Head Office Accounts Department.')", true);

                    return;
                }
            }
            else
            {

            }


        }

        public DataTable GetConsignmentDetail(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sqlString = "select c.consignmentNumber, cc.accountNo, c.consignerAccountNo, cc.name, c.creditClientId, cc.id\n" +
            "  from consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on cc.accountNo = c.consignerAccountNo\n" +
            "   and cc.branchCode = c.branchCode\n" +
            " where \n" +
            "   c.consignmentNumber = '" + clvar.consignmentNo + "'";


            sqlString = "select c.consignmentNumber,\n" +
            "       cc.accountNo,\n" +
            "       c.consignerAccountNo,\n" +
            "       cc.name,\n" +
            "       c.creditClientId,\n" +
            "       cc.id,\n" +
            "       p.ConsignmentNo Present, p.id VoucherID, c.totalAmount\n" +
            "  from consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on cc.id = c.creditclientID\n" +
            "    --on cc.accountNo = c.consignerAccountNo\n" +
            "   --and cc.branchCode = c.branchCode\n" +
            "  left outer join PaymentVouchers p\n" +
            "    on p.ConsignmentNo = c.consignmentNumber\n" +
            "   and p.CreditClientId = c.creditClientId\n" +
            " where c.consignmentNumber = '" + clvar.consignmentNo + "'";


            sqlString = "\n" +
           "SELECT c.consignmentNumber,\n" +
           "       cc.accountNo,\n" +
           "       c.consignerAccountNo,\n" +
           "       cc.name,\n" +
           "       c.creditClientId,\n" +
           "       cc.id,\n" +
           "       p.ConsignmentNo Present,\n" +
           "       p.id VoucherID,\n" +
           "       c.totalAmount,\n" +
           "       CASE\n" +
           "         WHEN COUNT(c2.Accountno) > 0 THEN\n" +
           "          (SELECT SUM(distinct cdn.codAmount)\n" +
           "             FROM CODConsignmentDetail_New cdn\n" +
           "            WHERE cdn.consignmentNumber = c.consignmentNumber)\n" +
           "         ELSE\n" +
           "          (SELECT SUM(cd.codAmount)\n" +
           "             FROM CODConsignmentDetail cd\n" +
           "            WHERE cd.consignmentNumber = c.consignmentNumber)\n" +
           "       END CODAMOUNT\n" +
           "  FROM consignment c\n" +
           " INNER JOIN CreditClients cc\n" +
           "    ON cc.id = c.creditclientID\n" +
           "--on cc.accountNo = c.consignerAccountNo\n" +
           "--and cc.branchCode = c.branchCode\n" +
           "\n" +
           "  LEFT OUTER JOIN PaymentVouchers p\n" +
           "    ON p.ConsignmentNo = c.consignmentNumber\n" +
           "   AND p.CreditClientId = c.creditClientId\n" +
           "  LEFT OUTER JOIN CODUsers c2\n" +
           "    ON c2.CreditClientID = c.creditClientId\n" +
           "   AND c2.Accountno = c.consignerAccountNo\n" +
           "   AND c2.IsCOD = '1'\n" +
           " WHERE c.consignmentnumber = '" + clvar.consignmentNo + "'\n" +
           " GROUP BY c.consignmentNumber,\n" +
           "          cc.accountNo,\n" +
           "          c.consignerAccountNo,\n" +
           "          cc.name,\n" +
           "          c.creditClientId,\n" +
           "          cc.id,\n" +
           "          p.ConsignmentNo,\n" +
           "          p.id,\n" +
           "          c.totalAmount";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return dt;
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
                sqlcmd.CommandTimeout = 3000;
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


        protected void AddDSlip_TextChanged(object sender, EventArgs e)
        {
            btn_reset.Visible = false;
            DateTime rrdate = DateTime.Parse(txt_rrdate.Text);
            DateTime chequedate = DateTime.Parse(txt_chequeDate.Text);


            if (rrdate > chequedate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('RR Date is Less Than  ')", true);
                return;
            }

            if (dd_depositSlipBank.SelectedValue.ToString() == "0")
            {
                txt_dslipNo.Text = "";
                txt_amount.Text = "";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select DSlip Bank')", true);
                return;
            }

            if (txt_dslipNo.Text == "")
            {
                txt_amount.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Dslip No')", true);
                return;
            }



            DataTable dt = new DataTable();
            if (ViewState["temp"] != null)
            {
                dt = (DataTable)ViewState["temp"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["DSlipNo"] = txt_dslipNo.Text;
                    dr["DSlipAmount"] = txt_amount.Text;
                    dr["DepositDate"] = txt_chequeDate.Text;
                    dr["Bank"] = dd_depositSlipBank.SelectedValue;
                    dr["BankName"] = dd_depositSlipBank.SelectedItem;
                    dt.Rows.Add(dr);
                    ViewState["Row"] = dt;
                    lbl_count.Text = "Count: " + dt.Rows.Count.ToString();
                    divtotal.Visible = true;
                    // Declare an object variable.
                    object sumTotal;
                    sumTotal = dt.Compute("Sum(DSlipAmount)", "");
                    lbl_grandamount.Text = sumTotal.ToString();
                    //                lbl_diffamount.Text = decimal.Parse(lbl_totalamount.Text) - sumTotal.ToString();

                    double total_rr = double.Parse(lbl_totalamount.Text);
                    double sumtotal_rr = double.Parse(sumTotal.ToString());

                    lbl_diffamount.Text = "<br>Difference Amount: " + (total_rr - sumtotal_rr).ToString();

                    GridView.DataSource = ViewState["temp"];
                    GridView.DataBind();
                    txt_dslipNo.Text = "";
                    txt_amount.Text = "";
                    txt_rrdate.Enabled = false;
                    Cache["date"] = txt_chequeDate.Text; ;
                    txt_dslipNo.Focus();
                }
            }
            else
            {
                dt.Columns.Add(new DataColumn("DSlipNo", typeof(string)));
                dt.Columns.Add(new DataColumn("DSlipAmount", typeof(double)));
                dt.Columns.Add(new DataColumn("DepositDate", typeof(string)));
                dt.Columns.Add(new DataColumn("Bank", typeof(string)));
                dt.Columns.Add(new DataColumn("BankName", typeof(string)));

                DataRow dr1 = dt.NewRow();
                // dr1 = dt.NewRow();
                dr1["DSlipNo"] = txt_dslipNo.Text;
                dr1["DSlipAmount"] = txt_amount.Text;
                dr1["DepositDate"] = txt_chequeDate.Text;
                dr1["Bank"] = dd_depositSlipBank.SelectedValue;
                dr1["BankName"] = dd_depositSlipBank.SelectedItem;
                dt.Rows.Add(dr1);
                ViewState["temp"] = dt;
                divtotal.Visible = true;
                lbl_count.Text = "Count: " + dt.Rows.Count.ToString();
                object sumTotal;
                sumTotal = dt.Compute("Sum(DSlipAmount)", "");
                lbl_grandamount.Text = sumTotal.ToString();
                double total_rr = double.Parse(lbl_totalamount.Text);
                double sumtotal_rr = double.Parse(sumTotal.ToString());
                lbl_diffamount.Text = "<br>Difference Amount: " + (total_rr - sumtotal_rr).ToString();
                GridView.DataSource = ViewState["temp"];
                GridView.DataBind();
                txt_dslipNo.Text = "";
                txt_amount.Text = "";
                txt_rrdate.Enabled = false;
                txt_dslipNo.Focus();
            }

            CalendarExtender1.SelectedDate = DateTime.Parse(txt_chequeDate.Text);
        }

        public DataSet Get_Sum_of_RR_Data()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                //string branch = "";
                //string ec = "";
                string sqlString = "";
                if (dd_company.SelectedValue == "1")
                {


                    sqlString = "SELECT \n" +
                   //"       a.rank,\n" +
                   //"       CONVERT(VARCHAR, a.date, 103)     date,\n" +
                   //"       a.cashtype,\n" +
                   "       sum(isnull(x.dnote,'0'))                           dnote\n" +
                   //"       a.credit                          cnote,\n" +
                   //"       a.branch,\n" +
                   //"       company\n" +
                   "FROM   (\n" +
                   "SELECT DISTINCT a.rank,\n" +
                   "       CONVERT(VARCHAR, a.date, 103)     date,\n" +
                   "       a.cashtype,\n" +
                   "       a.debit                           dnote,\n" +
                   "       a.credit                          cnote,\n" +
                   "       a.branch,\n" +
                   "       company\n" +
                   "FROM   (\n" +
                   "           SELECT '1' RANK,\n" +
                   "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit,\n" +
                   "                  0               credit,\n" +
                   //"                  'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo cashtype,\n" +
                   "                   CASE WHEN PV.RefNo IS NULL OR PV.RefNo = '' THEN 'Cash In Hand (' + ISNULL(pt.name, '') + ')' ELSE 'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo END cashtype, \n" +
                   "                  VoucherDate     date,\n" +
                   "                  b.[name]        branch,\n" +
                   "                  '' company\n" +
                   "           FROM   PaymentVouchers pv\n" +
                   "                  LEFT OUTER JOIN PaymentTypes pt\n" +
                   "                       ON  pt.Id = pv.PaymentTypeId\n" +
                   "                  LEFT OUTER JOIN (\n" +
                   "                           SELECT mp.voucherID,\n" +
                   "                                  SUM(mp.amount) productAmount\n" +
                   "                           FROM   MnP_PaymentVouchersProductBreakDown mp\n" +
                   "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
                   "                           GROUP BY\n" +
                   "                                  mp.voucherID\n" +
                   "                       ) mm\n" +
                   "                       ON  mm.voucherID = pv.Id\n" +
                   "                  INNER JOIN Branches b\n" +
                   "            ON  b.branchCode = pv.BranchCode\n" +
                   " WHERE pv.BranchCode  = '" + dd_branch.SelectedValue + "' \n" +
                   "   and pv.VoucherDate  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                   "       AND pv.CashPaymentSource IS NOT NULL\n" +
                   "                  AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1')\n" +
                   "                  AND pv.CashPaymentSource != '4'\n" +
                   "           GROUP BY\n" +
                   "                  VoucherDate, PV.RefNo,\n" +
                   "                  pt.name,\n" +
                   "                  b.[name]\n" +
                   "\n" +
                   "\n" +
                   "           UNION ALL\n" +
                   "\n" +
                   "\n" +
                   "           SELECT '1' RANK,\n" +
                   "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit,\n" +
                   "                  0               credit,\n" +
                 //  "                  'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo cashtype,\n" +
                 "                   CASE WHEN PV.RefNo IS NULL OR PV.RefNo = '' THEN 'Cash In Hand (' + ISNULL(pt.name, '') + ')' ELSE 'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo END cashtype, \n" +
                   "                  VoucherDate     date,\n" +
                   "                  b.[name]        branch,\n" +
                   "                  '' company\n" +
                   "           FROM   PaymentVouchers pv\n" +
                   "                  LEFT OUTER JOIN (\n" +
                   "                           SELECT mp.voucherID,\n" +
                   "                                  SUM(mp.amount) productAmount\n" +
                   "                           FROM   MnP_PaymentVouchersProductBreakDown mp\n" +
                   "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
                   "                           GROUP BY\n" +
                   "                                  mp.voucherID\n" +
                   "                       ) mm\n" +
                   "                       ON  mm.voucherID = pv.Id\n" +
                   "                  LEFT OUTER JOIN PaymentTypes pt\n" +
                   "                       ON  pt.Id = pv.PaymentTypeId\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = pv.BranchCode\n" +
                   "WHERE pv.BranchCode  = '" + dd_branch.SelectedValue + "' \n" +
                   "   and pv.VoucherDate  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                   "                  AND pv.CreditClientId IS NOT NULL\n" +
                   // "                  AND pv.paymentsourceid = '1'\n" +
                   "                 AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1') \n" +
                   "           GROUP BY\n" +
                   "                  VoucherDate, PV.RefNo,\n" +
                   "                  pt.name,\n" +
                   "                  b.[name]\n" +
                   "           UNION ALL\n" +
                   "\n" +
                   "\n" +
                   "           SELECT '1' RANK,\n" +
                   "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit,\n" +
                   "                  0               credit,\n" +
                   //"                  'Cash In Hand (' + ISNULL(pt.name, '') + ')'  + ' Ref No# '+PV.RefNo cashtype,\n" +
                   "                   CASE WHEN PV.RefNo IS NULL OR PV.RefNo = '' THEN 'Cash In Hand (' + ISNULL(pt.name, '') + ')'  ELSE 'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo END cashtype, \n" +
                   "                  VoucherDate     date,\n" +
                   "                  b.[name]        branch,\n" +
                   "                  '' company\n" +
                   "           FROM   PaymentVouchers pv\n" +
                   "                  LEFT OUTER JOIN (\n" +
                   "                           SELECT mp.voucherID,\n" +
                   "                                  SUM(mp.amount) productAmount\n" +
                   "                           FROM   MnP_PaymentVouchersProductBreakDown mp\n" +
                   "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
                   "                           GROUP BY\n" +
                   "                                  mp.voucherID\n" +
                   "                       ) mm\n" +
                   "                       ON  mm.voucherID = pv.Id\n" +
                   "                  LEFT OUTER JOIN PaymentTypes pt\n" +
                   "                       ON  pt.Id = pv.PaymentTypeId\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = pv.BranchCode\n" +
                   " WHERE pv.BranchCode  = '" + dd_branch.SelectedValue + "' \n" +
                   "   and pv.VoucherDate  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +

                   "                  AND pv.CreditClientId IS NULL\n" +
                   "                  AND pv.ClientGroupId IS NOT NULL\n" +
                   "                  AND pv.paymentsourceid = '1'\n" +
                   "                 AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1') \n" +
                   "           GROUP BY\n" +
                   "                  VoucherDate, PV.RefNo, \n" +
                   "                  pt.name,\n" +
                   "                  b.[name]\n" +
                   "           UNION ALL\n" +
                   "\n" +
                   "           SELECT '2' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
                   "                       ELSE 0\n" +
                   "                  END               debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(pv.Amount)\n" +
                   "                  END               credit,\n" +
                   //"                  'Deposit To Bank: ' + isnull(bo.bank_name,'') + ' (Dslip No: ' + isnull(pv.DepositSlipNo,'')\n" +
                   //"                  + ')' cashtype,\n" +
                   "                  CASE WHEN (SUM(pv.Amount) < 0 OR pv.ldesc is not null)  THEN pv.LDESC ELSE \n" +
                   "                  'Deposit To Bank: ' + isnull(bo.bank_name,'') + ' (Dslip No: ' + isnull(pv.DepositSlipNo,'') \n" +
                   "                  + ')' END cashtype, \n" +
                   "                  pv.ChequeDate     date,\n" +
                   "                  b.[name]          branch,\n" +
                   "                  '' company\n" +
                   "           FROM   deposit_to_bank pv\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = pv.BranchCode\n" +
                   "                  LEFT OUTER JOIN banks_of bo\n" +
                   "                       ON  bo.bank_code = pv.DepositSlipBankID\n" +
                   " WHERE pv.BranchCode   = '" + dd_branch.SelectedValue + "' \n" +
                   " and pv.company='" + dd_company.SelectedValue + "'\n" +
                   "   and pv.ChequeDate  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +

                   "           GROUP BY\n" +
                   "                  ChequeDate,\n" +
                   "                  b.[name],\n" +
                   "                  pv.Id,\n" +
                   "                  bo.bank_name,\n" +
                   "                  pv.DepositSlipNo, pv.LDESC\n" +
                   "\n" +
                   "\n" +
                   "           UNION ALL\n" +
                   "\n" +
                   "           SELECT '2' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.AMOUNT) < 0 THEN (SUM(pv.AMOUNT) * -1)\n" +
                   "                       ELSE 0\n" +
                   "                  END                       debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(pv.AMOUNT)\n" +
                   "                  END                       credit,\n" +
                   "                  'Transferred To Petty Cash (From : ' + pc.[desc] + ')'\n" +
                   "                  cashtype,\n" +
                   "                  pv.[date]                 date,\n" +
                   "                  b.[name]                  branch,\n" +
                   "                  cm.sdesc_OF               company\n" +
                   "           FROM   PC_CIH_detail pv\n" +
                   "                  INNER JOIN PC_CIH_head ph\n" +
                   "                       ON  ph.ID = pv.head_id\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = ph.BRANCH\n" +
                   "                  INNER JOIN PC_cash_mode pc\n" +
                   "                       ON  pc.ID = pv.cash_type\n" +
                   "                       AND pv.cash_type = '1'\n" +
                   "                       AND pc.ID = '1'\n" +
                   "                  INNER JOIN COMPANY_OF cm\n" +
                   "                       ON  cm.code_OF = ph.COMPANY\n" +
                   "   and ph.COMPANY='" + dd_company.SelectedValue + "'\n" +
                   " WHERE ph.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                   "  and pv.[date]  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                   "           GROUP BY\n" +
                   "                  [date],\n" +
                   "                  b.[name],\n" +
                   "                  pc.[desc],\n" +
                   "                  cm.sdesc_OF\n" +
                   "           UNION ALL\n" +
                   "           --From Company\n" +
                   "           SELECT '2' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.AMOUNT) < 0 THEN (SUM(ch.AMOUNT) * -1)\n" +
                   "                       ELSE 0\n" +
                   "                  END              debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(ch.AMOUNT)\n" +
                   "                  END              credit,\n" +
                   "                  'Transfered To  ' + isnull(cm1.sdesc_OF,'') + ' Remarks: '+ch.remarks cashtype,\n" +
                   "                  ch.t_date        date,\n" +
                   "                  b.[name]         branch,\n" +
                   "                  cm1.sdesc_OF     company\n" +
                   "           FROM   cih_transfer ch\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = ch.BRANCH\n" +
                   "                  INNER JOIN COMPANY_OF cm\n" +
                   "                       ON  cm.code_OF = ch.from_comp\n" +
                   "                  INNER JOIN COMPANY_OF cm1\n" +
                   "                       ON  cm1.code_OF = ch.to_comp\n" +
                   " WHERE ch.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                  "  and ch.t_date  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                  "  and ch.from_comp ='" + dd_company.SelectedValue + "'\n" +
                   "           GROUP BY\n" +
                   "                  ch.t_date,\n" +
                   "                  b.[name],\n" +
                   "                  cm1.sdesc_OF,ch.remarks \n" +
                   "\n" +
                   "           UNION ALL\n" +
                   "           --To Company\n" +
                   "           SELECT '1' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(ch.amount)\n" +
                   "                  END             debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
                   "                  END             credit,\n" +
                   "                  'Received From  ' + isnull(cm.sdesc_OF,'') + ' Remarks: '+ch.remarks cashtype,\n" +
                   "                  ch.t_date       date,\n" +
                   "                  b.[name]        branch,\n" +
                   "                  cm.sdesc_OF     company\n" +
                   "           FROM   cih_transfer ch\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = ch.BRANCH\n" +
                   "                   left outer  JOIN COMPANY_OF cm\n" +
                   "                       ON  cm.code_OF = ch.from_comp\n" +
                   "                  INNER JOIN COMPANY_OF cm1\n" +
                   "                       ON  cm1.code_OF = ch.to_comp\n" +
                   " WHERE ch.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                  "  and ch.t_date  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                  "  and ch.to_comp ='" + dd_company.SelectedValue + "'\n" +
                   "           GROUP BY\n" +
                   "                  ch.t_date,\n" +
                   "                  b.[name],\n" +
                   "                  cm.sdesc_OF, ch.remarks \n" +
                   "       )                                 a\n";
                    //"ORDER BY\n" +
                    //"       2,\n" +
                    //"       1";


                }
                else
                {
                    sqlString = "--For Companies other than 1\n" +
                  "SELECT \n" +
                  //"       a.rank,\n" +
                  //"       CONVERT(varchar, a.date, 103) date,\n" +
                  //"       a.cashtype,\n" +
                  "       sum(isnull(a.debit,'0')) dnote,\n" +
                  //"       a.credit cnote,\n" +
                  //"       a.branch,\n" +
                  //"       company\n" +
                  "  FROM (\n" +
                  "SELECT '2' rank,0 debit,\n" +
                  "  SUM(pv.Amount) credit,\n" +
                  "       'Deposit To Bank: ' + isnull(bo.bank_name,'')  + ' (Dslip No: ' + isnull(pv.DepositSlipNo,'') + ')' cashtype,\n" +
                  "       pv.ChequeDate date,\n" +
                  "     b.[name] branch,\n" +
                  "\t '' company\n" +
                  "  FROM deposit_to_bank pv\n" +
                  "  inner join Branches b\n" +
                  "  on b.branchCode=pv.BranchCode\n" +
                  " inner join banks_of bo \n" +
                  " on bo.bank_code=pv.DepositSlipBankID \n" +
                  " WHERE pv.BranchCode   = '" + dd_branch.SelectedValue + "' \n" +
                  " and pv.company='" + dd_company.SelectedValue + "'\n" +
                  " and pv.ChequeDate  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                  " GROUP BY ChequeDate, b.[name],pv.Id,bo.bank_name,pv.DepositSlipNo\n" +
                  "\n" +
                  "        union all\n" +
                  "\n" +
                  "        SELECT '2' rank,\n" +
                  "               0 debit,\n" +
                  "               SUM(pv.Amount) credit,\n" +
                  "               'Transferred To Petty Cash (From : ' + pc.[desc]+ ')' cashtype,\n" +
                  "               pv.[date]date,\n" +
                  "               b.[name]branch,\n" +
                  "               cm.sdesc_OF company\n" +
                  "          FROM PC_CIH_detail pv\n" +
                  "         inner join PC_CIH_head ph\n" +
                  "            on ph.ID = pv.head_id\n" +
                  "         inner join Branches b\n" +
                  "            on b.branchCode = ph.BRANCH\n" +
                  "         inner join PC_cash_mode pc\n" +
                  "            on pc.ID = pv.cash_type\n" +
                  "           and pv.cash_type = '1'\n" +
                  "           and pc.ID = '1'\n" +
                  "         inner join COMPANY_OF cm\n" +
                  "            on cm.code_OF = ph.COMPANY\n" +
                  "           and ph.COMPANY = ='" + dd_company.SelectedValue + "'\n" +
                  "         WHERE ph.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                  "           and pv.[date] = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' " +
                  "         GROUP BY[date], b.[name], pc.[desc], cm.sdesc_OF\n" +
                  "        union ALL\n" +
                  "        --From Company\n" +
                  "        SELECT '2' rank,\n" +
                  "               0 debit,\n" +
                  "               SUM(ch.amount) credit,\n" +
                  "               'Transfered To  ' + isnull(cm1.sdesc_OF,'') + ' Remarks: '+ch.remarks cashtype,\n" +
                  "               ch.t_date date,\n" +
                  "               b.[name]branch,\n" +
                  "               cm1.sdesc_OF company\n" +
                  "          FROM cih_transfer ch\n" +
                  "         inner join Branches b\n" +
                  "            on b.branchCode = ch.BRANCH\n" +
                  "         inner join COMPANY_OF cm\n" +
                  "            on cm.code_OF = ch.from_comp\n" +
                  "         inner join COMPANY_OF cm1\n" +
                  "            on cm1.code_OF = ch.to_comp\n" +
                  "         WHERE ch.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                  "           and ch.t_date  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                  "           and ch.from_comp = '" + dd_company.SelectedValue + "'\n" +
                  "         GROUP BY ch.t_date, b.[name], cm1.sdesc_OF,ch.remarks \n" +
                  "\n" +
                  "        union all\n" +
                  "        --To Company\n" +
                  "        SELECT '1' rank,\n" +
                  "               SUM(ch.amount) debit,\n" +
                  "               0 credit,\n" +
                  "               'Received From  ' + isnull(cm.sdesc_OF,'') + ' Remarks: '+ch.remarks  cashtype,\n" +
                  "               ch.t_date date,\n" +
                  "               b.[name]branch,\n" +
                  "               cm.sdesc_OF company\n" +
                  "          FROM cih_transfer ch\n" +
                  "         inner join Branches b\n" +
                  "            on b.branchCode = ch.BRANCH\n" +
                  "          left outer  join COMPANY_OF cm\n" +
                  "            on cm.code_OF = ch.from_comp\n" +
                  "         inner join COMPANY_OF cm1\n" +
                  "            on cm1.code_OF = ch.to_comp\n" +
                  "         WHERE ch.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                  "           and ch.t_date  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                  "           and ch.to_comp = ='" + dd_company.SelectedValue + "'\n" +
                  "         GROUP BY ch.t_date, b.[name], cm.sdesc_OF, ch.remarks ) a\n";
                    // " ORDER BY 2, 1";


                    sqlString = "/************************************************************\n" +
                    " * Code formatted by SoftTree SQL Assistant © v6.3.153\n" +
                    " * Time: 7/24/2018 12:34:05 PM\n" +
                    " * CASH IN HAND COMPANY 2\n" +
                    " ************************************************************/\n" +
                    "\n" +
                    "SELECT x.rank,\n" +
                    //  "       CONVERT(VARCHAR, a.date, 103)     date,\n" +
                    //  "       a.cashtype,\n" +
                    "       sum(isnull(x.dnote,'0'))                           dnote\n" +
                    // "       a.credit                          cnote,\n" +
                    // "       a.branch,\n" +
                    // "       company\n" +
                    "FROM   (\n" +
                    "SELECT DISTINCT a.rank,\n" +
                   "       CONVERT(VARCHAR, a.date, 103)     date,\n" +
                   "       a.cashtype,\n" +
                   "       a.debit                           dnote,\n" +
                   "       a.credit                          cnote,\n" +
                   "       a.branch,\n" +
                   "       company\n" +
                   "FROM   (\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END               debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(pv.Amount)\n" +
                    "                  END               credit,\n" +
                    "                  'Deposit To Bank: ' + isnull(bo.bank_name,'') + ' (Dslip No: ' + isnull(pv.DepositSlipNo,'')\n" +
                    "                  + ')' cashtype,\n" +
                    "                  pv.ChequeDate     date,\n" +
                    "                  b.[name]          branch,\n" +
                    "                  '' company\n" +
                    "           FROM   deposit_to_bank pv\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = pv.BranchCode\n" +
                    "                  INNER JOIN banks_of bo\n" +
                    "                       ON  bo.bank_code = pv.DepositSlipBankID\n" +
                    " WHERE pv.BranchCode   = '" + dd_branch.SelectedValue + "' \n" +
                    " and pv.company=='" + dd_company.SelectedValue + "'\n" +
                    " and pv.ChequeDate  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                    "           GROUP BY\n" +
                    "                  ChequeDate,\n" +
                    "                  b.[name],\n" +
                    "                  pv.Id,\n" +
                    "                  bo.bank_name,\n" +
                    "                  pv.DepositSlipNo\n" +
                    "\n" +
                    "           UNION ALL\n" +
                    "\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.AMOUNT) < 0 THEN (SUM(pv.AMOUNT) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END                      debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(pv.AMOUNT)\n" +
                    "                  END                      credit,\n" +
                    "                  'Transferred To Petty Cash (From : ' + pc.[desc] + ')'\n" +
                    "                  cashtype,\n" +
                    "                  pv.[date]                date,\n" +
                    "                  b.[name]                 branch,\n" +
                    "                  cm.sdesc_OF              company\n" +
                    "           FROM   PC_CIH_detail pv\n" +
                    "                  INNER JOIN PC_CIH_head ph\n" +
                    "                       ON  ph.ID = pv.head_id\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ph.BRANCH\n" +
                    "                  INNER JOIN PC_cash_mode pc\n" +
                    "                       ON  pc.ID = pv.cash_type\n" +
                    "                       AND pv.cash_type = '1'\n" +
                    "                       AND pc.ID = '1'\n" +
                    "                  INNER JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ph.COMPANY\n" +
                    "           and ph.COMPANY = ='" + dd_company.SelectedValue + "'\n" +
                    "         WHERE ph.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                    "           and pv.[date] = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' " +

                    "           GROUP BY\n" +
                    "                  [date],\n" +
                    "                  b.[name],\n" +
                    "                  pc.[desc],\n" +
                    "                  cm.sdesc_OF\n" +
                    "           UNION ALL\n" +
                    "           --From Company\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.AMOUNT) < 0 THEN (SUM(ch.AMOUNT) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END              debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(ch.AMOUNT)\n" +
                    "                  END              credit,\n" +
                    "                  'Transfered To  ' + isnull(cm1.sdesc_OF,'') + ' Remarks: '+ch.remarks cashtype,\n" +
                    "                  ch.t_date        date,\n" +
                    "                  b.[name]         branch,\n" +
                    "                  cm1.sdesc_OF     company\n" +
                    "           FROM   cih_transfer ch\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ch.BRANCH\n" +
                    "                  INNER JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ch.from_comp\n" +
                    "                  INNER JOIN COMPANY_OF cm1\n" +
                    "                       ON  cm1.code_OF = ch.to_comp\n" +
                    "         WHERE ch.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                    "           and ch.t_date  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                    "           and ch.from_comp = '" + dd_company.SelectedValue + "'\n" +
                    "           GROUP BY\n" +
                    "                  ch.t_date,\n" +
                    "                  b.[name],\n" +
                    "                  cm1.sdesc_OF,ch.remarks \n" +
                    "\n" +
                    "           UNION ALL\n" +
                    "           --To Company\n" +
                    "           SELECT '1' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(ch.amount)\n" +
                    "                  END             debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END             credit,\n" +
                    "                  'Received From  ' + isnull(cm.sdesc_OF,'') + ' Remarks: '+ch.remarks  cashtype,\n" +
                    "                  ch.t_date       date,\n" +
                    "                  b.[name]        branch,\n" +
                    "                  cm.sdesc_OF     company\n" +
                    "           FROM   cih_transfer ch\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ch.BRANCH\n" +
                    "                   left outer  JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ch.from_comp\n" +
                    "                  INNER JOIN COMPANY_OF cm1\n" +
                    "                       ON  cm1.code_OF = ch.to_comp\n" +
                    "         WHERE ch.BRANCH   = '" + dd_branch.SelectedValue + "' \n" +
                  "           and ch.t_date  = '" + DateTime.Parse(txt_rrdate.Text.ToString()).ToString("yyyy-MM-dd") + "' \n" +
                  "           and ch.to_comp = '" + dd_company.SelectedValue + "'\n" +
                    "           GROUP BY\n" +
                    "                  ch.t_date,\n" +
                    "                  b.[name],\n" +
                    "                  cm.sdesc_OF,ch.remarks  \n" +
                    "       )                                 a\n";
                    //"ORDER BY\n" +
                    //"       2,\n" +
                    //"       1";

                }

                if (dd_nature.SelectedValue == "01")
                {
                    sqlString +=
                    "   WHERE a.cashtype like '%COD Payment%' \n ";
                }

                if (dd_nature.SelectedValue == "02")
                {
                    sqlString +=
                    "   WHERE a.cashtype NOT like '%COD Payment%' \n ";
                }


                sqlString += ") x ORDER BY 1 asc\n";


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

        protected void gv_consignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {
                string con = e.CommandArgument.ToString();
                DataTable dt = ViewState["temp"] as DataTable;
                DataRow dr = dt.Select("DSlipNo = '" + con + "'")[0];

                dt.Rows.Remove(dr);

                dt.AcceptChanges();
                ViewState["temp"] = dt;
                lbl_count.Text = "Count: " + dt.Rows.Count.ToString();

                object sumTotal;
                sumTotal = dt.Compute("Sum(DSlipAmount)", "");
                lbl_grandamount.Text = sumTotal.ToString();

                double total_rr = double.Parse(lbl_totalamount.Text);
                double sumtotal_rr = double.Parse(sumTotal.ToString());

                lbl_diffamount.Text = "<br>Difference Amount: " + (total_rr - sumtotal_rr).ToString();

                GridView.DataSource = dt;
                GridView.DataBind();
            }
        }

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'R'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return dt;
        }

    }
}