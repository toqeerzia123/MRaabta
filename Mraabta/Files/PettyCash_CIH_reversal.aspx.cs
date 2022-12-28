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
    public partial class PettyCash_CIH_reversal : System.Web.UI.Page
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
                //Get_Month();
                GetYear();
                dd_month.SelectedValue = DateTime.Now.ToString("MM");
                //Get_Branches();
                Get_Zones();
                Get_Cash_Transfer_Mode();
                Get_Companies();
                //DateTime date = DateTime.Now;
                //var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                //var lastDayOfMonth = firstDayOfMonth;
                //CalendarExtender1.StartDate = DateTime.Parse(firstDayOfMonth.ToString());
                //CalendarExtender1.EndDate = DateTime.Now;
                //CalendarExtender1.SelectedDate = DateTime.Now.Date;
                //Checking entry
                //check();
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
                    CalendarExtender1.StartDate = DateTime.Parse(dt.Rows[0]["YEAR"].ToString() + "-" + dt.Rows[0]["Month"].ToString() + "-01");
                    CalendarExtender1.EndDate = DateTime.Parse(dt.Rows[0]["YEAR"].ToString() + "-" + dt.Rows[0]["Month"].ToString() + "-01").AddMonths(1).AddDays(-1);
                }
            }
            catch (Exception ex)
            { }

        }
        #region dropdown calling
        public void check()
        {
            //Checking whether the user already has temp data previously not submitted
            DataSet ds = new DataSet();
            ds = UserPrevEntry();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txt_ID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                dd_branch.SelectedValue = ds.Tables[0].Rows[0]["BRANCH"].ToString();
                dd_year.SelectedValue = ds.Tables[0].Rows[0]["year"].ToString();
                dd_month.SelectedValue = int.Parse(ds.Tables[0].Rows[0]["month"].ToString()).ToString("D2");
                dd_company.SelectedValue = ds.Tables[0].Rows[0]["company"].ToString();

                dd_zone.SelectedValue = ds.Tables[0].Rows[0]["ZONECODE"].ToString();
                //  txt_checque_no.Text = ds.Tables[0].Rows[0]["cheque_no"].ToString();
                calendar();


                dd_branch.Enabled = false;
                dd_zone.Enabled = false;
                dd_year.Enabled = false;
                dd_month.Enabled = false;
                dd_company.Enabled = false;

                btn_submit.Visible = true;
                btn_clear.Visible = true;
                hf_status.Value = "1";

                DataSet ds_detail = new DataSet();
                ds_detail = GetTempData();

                // Declare an object variable.
                object sumObject;
                sumObject = ds_detail.Tables[0].Compute("Sum(Amount)", "");
                petty_amount = double.Parse(sumObject.ToString());
                hf_petty_amount.Value = petty_amount.ToString();
                hf_amount.Value = sumObject.ToString();
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

                diff = remaining_val - double.Parse(petty_amount.ToString());
                petty_amount = petty_amount + remaining_pettyCash;

                hf_diff.Value = diff.ToString();
                hf_petty_amount.Value = petty_amount.ToString();

                ViewState["dt"] = ds_detail.Tables[0];
                GV.DataSource = ds_detail.Tables[0];
                GV.DataBind();


                tb_1.Visible = false;

            }
            else
            {
                ////Get ID for the new entry
                //double id = Get_maxhead();
                //if (!string.IsNullOrEmpty(id.ToString()))
                //{
                //    txt_ID.Text = id.ToString();
                //}
            }
        }
        //public void Get_Branches()
        //{
        //    DataSet ds = new DataSet();
        //    ds = bfunc.Get_Branches(clvar);
        //    if (ds.Tables[0].Rows.Count != 0)
        //    {
        //        dd_branch.DataTextField = "Name";
        //        dd_branch.DataValueField = "branchCode";
        //        dd_branch.DataSource = ds.Tables[0].DefaultView;
        //        dd_branch.DataBind();
        //    }
        //}
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
                "where\n" +
                     "d.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "' and head_id='" + double.Parse(txt_ID.Text) + "' order by D.ID";

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

                string sqlString = "select c.[VALUE] remaining_cash,petty_cash from CIH_remainings c\n" +
                "where\n" +
                "c.month='" + dd_month.SelectedValue.ToString() + "'\n" +
                "and c.year='" + dd_year.SelectedItem.ToString() + "'\n" +
                 "and c.company='" + dd_company.SelectedValue.ToString() + "'\n" +
                "and c.branch='" + dd_branch.SelectedValue.ToString() + "'";

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

        public void Insert_headEntry()
        {

            try
            {

                string sqlString1 = "UPDATE systemcodes\n" +
              "SET    codeValue      = codeValue + 1,\n" +
              "       [year]         = YEAR\n" +
              "       OUTPUT DELETED.year + DELETED.codeValue\n" +
              "WHERE  codeType       = 'PETTY_CIH'\n" +
              "       AND [year]     = YEAR(GETDATE())";

                //double count_ = Get_maxhead();


                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();

                SqlCommand orcd = new SqlCommand(sqlString1, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandText = sqlString1;
                Int64 headID = 0;
                object obj = orcd.ExecuteScalar();
                Int64.TryParse(obj.ToString(), out headID);

                if (headID <= 0)
                {
                    orcl.Close();
                    return;
                }
                txt_ID.Text = headID.ToString();

                string query_head = "INSERT INTO PC_temp_CIH_head(ID,COMPANY,BRANCH,YEAR,MONTH,CREATED_BY,Created_ON,zonecode)\n" +
                 "VALUES(\n" +
                 "'" + double.Parse(txt_ID.Text) + "' ,\n" +
                 " '" + dd_company.SelectedValue.ToString() + "' ,\n" +
                 " '" + dd_branch.SelectedValue.ToString() + "' ,\n" +
                 " '" + dd_year.SelectedItem.ToString() + "' ,\n" +
                 " '" + dd_month.SelectedValue.ToString() + "' ,\n" +
                 " '" + HttpContext.Current.Session["U_ID"].ToString() + "' ,\n" +
                 " GETDATE(),'" + dd_zone.SelectedValue.ToString() + "'\n" +
                 " )";

                orcd = new SqlCommand(query_head, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                hf_status.Value = "1";
                hf_headID.Value = txt_ID.Text;
                lbl_error.Text = "";
            }
            catch (Exception Err)
            {
                hf_status.Value = "";
                lbl_error.Text = "The entry is not saved due to an error!!";
                return;
            }
            finally
            { }



        }
        public void Insert_DetailEntry(int amount)
        {
            try
            {
                //diff = remaining_val - double.Parse(amount.ToString());
                //petty_amount = amount + remaining_pettyCash;

                if (dd_cash_mode.SelectedValue.ToString() != "2")
                {
                    txt_checque_no.Text = "";

                }

                double count_ = Get_maxdet(double.Parse(txt_ID.Text));
                string sqlString = "INSERT INTO pc_temp_cih_detail(ID,HEAD_ID,DATE,AMOUNT,amt_rmain,CREATED_BY,Created_ON,cash_type,chque_no,status)\n" +
                "VALUES\n" +
                "('" + count_ + "','" + double.Parse(txt_ID.Text) + "',\n" +
                " '" + DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd") + "', \n" +
                " '" + amount + "','" + amount + "',\n" +
                " '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                " GETDATE(),'" + dd_cash_mode.SelectedValue.ToString() + "',\n" +
                "'" + txt_checque_no.Text + "','0' )";




                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();




                orcl.Close();

                if (hf_subheadID.Value == "")
                {
                    hf_subheadID.Value = "1";
                }
                else
                {
                    int val = int.Parse(hf_subheadID.Value.ToString()) + 1;//for N


                }
                lbl_error.Text = "";

            }
            catch (Exception Err)
            {
                lbl_error.Text = "The entry is not saved due to an error!!";
                return;
            }
            finally
            { }
        }
        public int Insert_Final_headEntry()
        {
            int stat = 0;
            try
            {
                string query_head = "INSERT INTO PC_cih_HEAD \n" +
                "select * from PC_temp_cih_HEAD where id='" + double.Parse(txt_ID.Text) + "' and created_by='" + HttpContext.Current.Session["U_ID"].ToString() + "'\n";
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query_head, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
                lbl_error.Text = "";
                lbl_err2.Text = "";
                stat = 1;
            }
            catch (Exception Err)
            {
                lbl_err2.Text = "The entry is not saved due to an error!!";
                stat = 0;
            }
            finally
            {

            }
            return stat;
        }
        public int Insert_Final_DetailEntry()
        {
            int stat = 0;
            try
            {
                string query_head = "INSERT INTO PC_cih_detail \n" +
                "select * from PC_temp_cih_detail where head_id='" + double.Parse(txt_ID.Text) + "'\n";

                string sqlString1 = "";
                if (dd_cash_mode.SelectedValue.ToString() == "1")
                {
                    //sqlString1 = "update CIH_remainings set petty_cash='" + double.Parse(hf_petty_amount.Value.ToString()) + "', value='" + double.Parse(hf_diff.Value.ToString()) + "'\n" +
                    //" where\n" +
                    //" [month]='" + dd_month.SelectedValue.ToString() + "' and\n" +
                    //" [year]='" + dd_year.SelectedItem.ToString() + "' and\n" +
                    //" [COMPANY]='" + dd_company.SelectedValue.ToString() + "' and\n" +
                    //" [branch]='" + dd_branch.SelectedValue.ToString() + "'";
                }
                else
                {
                    //sqlString1 = "update CIH_remainings set petty_cash=petty_cash + '" + double.Parse(hf_amount.Value.ToString()) + "'\n" +
                    //" where\n" +
                    //" [month]='" + dd_month.SelectedValue.ToString() + "' and\n" +
                    //" [year]='" + dd_year.SelectedItem.ToString() + "' and\n" +
                    //" [COMPANY]='" + dd_company.SelectedValue.ToString() + "' and\n" +
                    //" [branch]='" + dd_branch.SelectedValue.ToString() + "'";
                }
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query_head, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();

                //update query
                //SqlCommand orcd1 = new SqlCommand(sqlString1, orcl);
                //orcd1.CommandType = CommandType.Text;
                //SqlDataAdapter oda1 = new SqlDataAdapter(orcd1);
                //orcd1.ExecuteNonQuery();


                orcl.Close();
                lbl_error.Text = "";
                lbl_err2.Text = "";
                stat = 1;
            }
            catch (Exception Err)
            {
                lbl_err2.Text = "The entry is not saved due to an error!!";
                stat = 0;
            }
            finally
            {

            }
            return stat;
        }

        public int Delete_tempHead()
        {
            int i = 0; ;
            try
            {
                string sqlString = "delete from PC_temp_cih_head where id='" + double.Parse(txt_ID.Text) + "'\n";

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
        public int Delete_tempDetail()
        {
            int i = 0; ;
            try
            {
                string sqlString = "delete from PC_temp_cih_detail where head_id='" + double.Parse(txt_ID.Text) + "'\n";

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
        public int Delete_DetailEntry(int id)
        {
            int i = 0; ;
            try
            {
                string sqlString = "delete from PC_temp_CIH_DETAIL where ID='" + id + "' and head_id='" + double.Parse(txt_ID.Text) + "'\n";

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

        public double Get_maxhead()
        {
            double maxid = 0;
            DataSet ds = new DataSet();
            try
            {

                string sqlString = "UPDATE systemcodes\n" +
                "SET    codeValue      = codeValue + 1,\n" +
                "       [year]         = YEAR\n" +
                "       OUTPUT DELETED.year + DELETED.codeValue\n" +
                "WHERE  codeType       = 'PETTY_CIH'\n" +
                "       AND [year]     = YEAR(GETDATE())";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcl.Close();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0].Clone();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            dt.Rows.Add(dr.ItemArray);
                            dt.AcceptChanges();
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        maxid = 20182000001;
                    }
                    else if (dt.Rows.Count > 0)
                    {
                        maxid = double.Parse(dt.Rows[0][0].ToString());
                    }
                    //else if (dt.Rows.Count == 2)
                    //{
                    //    if (int.Parse(dt.Rows[0][0].ToString()) > int.Parse(dt.Rows[1][0].ToString()))
                    //    {
                    //        maxid = int.Parse(dt.Rows[0][0].ToString());
                    //    }
                    //    else
                    //    {
                    //        maxid = int.Parse(dt.Rows[1][0].ToString());
                    //    }
                    //}

                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }
            return maxid;
        }
        public int Get_maxdet(double head_id)
        {
            int maxid = 0;
            DataSet ds = new DataSet();
            try
            {
                string max_id = " select max(id)+1 id from PC_temp_cih_DETAIL where HEAD_ID='" + head_id + "'";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(max_id, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                    maxid = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                else
                    maxid = 1;
            }
            catch (Exception Err)
            {
            }
            finally
            { }
            return maxid;
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
            #region date check
            try
            {
                DateTime.Parse(txt_date.Text);
            }
            catch
            {
                lbl_error.Text = "Date is Invalid!!";
                return;
            }
            txt_date.Text = DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd");
            if (DateTime.Parse(DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd")) >= CalendarExtender1.StartDate && DateTime.Parse(DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd")) <= CalendarExtender1.EndDate)
            {
            }
            else
            {
                lbl_error.Text = "Date is not Under specified range!!";
                return;
            }
            #endregion

            //:::::::::::Amount Checking::::::::::::::::://
            double remain_amount = 0;
            hf_amount.Value = txt_amount.Text;

            double cash_check = Get_Val();
            if (cash_check < double.Parse(txt_amount.Text))
            {
                lbl_error.Text = "The amount exceeds the reversable amount!!";
                return;
            }

            DataTable dt = new DataTable();

            btn_submit.Visible = true;
            btn_clear.Visible = true;
            lbl_error.Text = "";
            int amount = int.Parse(txt_amount.Text) * -1;
            if (hf_status.Value.ToString() == "")
            {
                //---------------------------Head Entry Insertion (One Time Entry)-------------------------------- //
                Insert_headEntry();
            }
            //---------------------------Detail Entry Insertion (One Time Entry)----------------------------------//
            Insert_DetailEntry(amount);
            //--------------------------Gridview Binding---------------------------------------------------------//
            DataSet ds = new DataSet();
            ds = GetTempData();
            ViewState["dt"] = ds.Tables[0];


            GV.DataSource = ds.Tables[0];
            GV.DataBind();

            tb_1.Visible = false;

            dd_branch.Enabled = false;
            dd_year.Enabled = false;
            dd_month.Enabled = false;
            dd_company.Enabled = false;

        }
        protected void btn_submit_Click(object sender, EventArgs e)
        {
            //Insert PC Head Entry
            int head_stat = Insert_Final_headEntry();
            if (head_stat == 1)
            {
                int detail_stat = Insert_Final_DetailEntry();
                if (detail_stat == 1)
                {
                    lbl_err2.Text = UpdateCIH_remaining() + "<\b>";
                    lbl_err2.Text += UpdatePC_remaining();
                    Delete_tempHead();
                    Delete_tempDetail();

                    Response.Redirect("pc_confirmation.aspx?id=" + double.Parse(txt_ID.Text) + "&type=CIH");
                }
                else
                {
                    lbl_err2.Text = "The entry is not saved due to error!!";
                    return;
                }

            }
            else
            {
                lbl_err2.Text = "No Entry To Save!! Please Add!!";
                return;
            }

        }
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                Delete_tempHead();
                Delete_tempDetail();
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                confirmValue = "";
                return;
            }
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            GridViewRow row = (GridViewRow)GV.Rows[e.RowIndex];
            string id = row.Cells[1].Text;
            int i = Delete_DetailEntry(int.Parse(id));

            DataSet ds = new DataSet();
            ds = GetTempData();
            ViewState["dt"] = ds.Tables[0];
            GV.DataSource = ds.Tables[0];
            GV.DataBind();

        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ID = e.Row.Cells[1].Text;
                foreach (Button button in e.Row.Cells[0].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete the entry with ID:  " + ID + "?')){ return false; };";
                    }
                }
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


        public double Get_Val()
        {
            double val = 0;
            DataSet ds = new DataSet();
            try
            {
                DateTime first_day = new DateTime(int.Parse(dd_year.SelectedItem.ToString()), int.Parse(dd_month.SelectedValue.ToString()), 1);
                DateTime last_day = first_day.AddMonths(1).AddSeconds(-1);


                string sqlString = "/************************************************************\n" +
                " * Code formatted by SoftTree SQL Assistant © v6.3.153\n" +
                " * Time: 6/28/2018 3:47:43 PM\n" +
                " ************************************************************/\n" +
                "\n" +
                "\n" +
                "SELECT cr.branch,\n" +
                "       cr.company ,\n" +
                "       cr.[month],\n" +
                "       cr.[year],\n" +
                "       ISNULL(SUM(BB.dnote), 0)     DNOTE,\n" +
                "       ISNULL(SUM(BB.cnote), 0)     CNOTE,\n" +
                "       cr.opening_pcash,\n" +
                "       ISNULL((ISNULL(SUM(bb.dnote),0) + ISNULL(cr.opening_pcash, 0) - ISNULL(SUM(bb.cnote),0)), 0)\n" +
                "       ClosingPcash,\n" +
                "       cr.petty_cash,\n" +
                "       (\n" +
                "           ISNULL((ISNULL(SUM(bb.dnote),0) + ISNULL(cr.opening_pcash, 0) - ISNULL(SUM(bb.cnote),0)), 0) - cr.petty_cash\n" +
                "       )                            DIFF\n" +
                "FROM   CIH_remainings cr\n" +
                "       LEFT OUTER JOIN (\n" +
                "                SELECT a.rank,\n" +
                "       a.COMPANY,\n" +
                "       a.YEAR,\n" +
                "       a.MONTH,\n" +
                "       a.Date,\n" +
                "       a.Debit      dnote,\n" +
                "       a.Credit     cnote,\n" +
                "       b.branchCode       branch,\n" +
                "       CASE e.name\n" +
                "            WHEN '0' THEN 'ALL'\n" +
                "            ELSE ISNULL(e.name, 'ALL')\n" +
                "       END          ec,\n" +
                "       a.expense,\n" +
                "       a.[description],\n" +
                "       a.narrate,\n" +
                "       a.ID,\n" +
                "       a.comp1\n" +
                "FROM   (\n" +
                "           SELECT '1' RANK,\n" +
                "                  pch.COMPANY,\n" +
                "                  pch.BRANCH,\n" +
                "                  pch.[YEAR],\n" +
                "                  pch.[MONTH],\n" +
                "                  CONVERT(VARCHAR, pcd.DATE, 103) \"Date\",\n" +
                "                  pch.express_center,\n" +
                "                  0            AS Credit,\n" +
                "                  pcd.AMOUNT   AS Debit,\n" +
                "                  'Petty Cash' expense,\n" +
                "                  'AMOUNT TRANSFERRED FROM CIH TO PETTY CASH' + '(' + pmode.[desc]\n" +
                "                  + ')' DESCRIPTION,\n" +
                "                  '' narrate,\n" +
                "                  pch.ID,\n" +
                "                  cm.code_OF     comp1\n" +
                "           FROM   PC_CIH_head  AS pch\n" +
                "                  INNER JOIN PC_CIH_detail AS pcd\n" +
                "                       ON  pcd.head_id = pch.ID\n" +
                "                  LEFT OUTER JOIN PC_cash_mode pmode\n" +
                "                       ON  pmode.ID = pcd.cash_type\n" +
                "                  INNER JOIN COMPANY_OF cm\n" +
                "                       ON  cm.code_OF = pch.COMPANY\n" +
                "                       AND pch.COMPANY = '" + dd_company.SelectedValue.ToString() + "'\n" +
                "           WHERE  pcd.[DATE] BETWEEN '" + first_day.ToString("yyyy-MM-dd") + "' --CONVERT(DATETIME, '01/07/2018', 103) AND\n" +
                "                  AND '" + last_day.ToString("yyyy-MM-dd") + "' --CONVERT(DATETIME, '31/07/2018', 103)\n" +
                "                 AND pch.BRANCH = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                "           UNION ALL\n" +
                "\n" +
                "           SELECT '2' RANK,\n" +
                "                  pch.COMPANY,\n" +
                "                  pch.BRANCH,\n" +
                "                  pch.[YEAR],\n" +
                "                  pch.[MONTH],\n" +
                "                  CONVERT(VARCHAR, pcd.DATE, 103) \"Date\",\n" +
                "                  pch.express_center,\n" +
                "                  pcd.AMOUNT            AS Credit,\n" +
                "                  0                     AS Debit,\n" +
                "                  m.description            expense,\n" +
                "                  s.sub_desc               DESCRIPTION,\n" +
                "                  pcd.NARRATE,\n" +
                "                  pch.ID,\n" +
                "                  cm.code_OF              comp1\n" +
                "           FROM   PC_head               AS pch\n" +
                "                  INNER JOIN PC_detail  AS pcd\n" +
                "                       ON  pcd.head_id = pch.ID --and pch.CREATED_BY=pcd.CREATED_BY\n" +
                "                  LEFT OUTER JOIN pc_mainHead m\n" +
                "                       ON  pcd.expense = m.code\n" +
                "                  LEFT OUTER JOIN pc_subHead s\n" +
                "                       ON  pcd.[desc] = s.subcode\n" +
                "                       AND pcd.EXPENSE = s.headcode\n" +
                "                  INNER JOIN COMPANY_OF cm\n" +
                "                       ON  cm.code_OF = pch.COMPANY\n" +
                "                       AND pch.COMPANY = '1'\n" +
                "           WHERE  pcd.[DATE] BETWEEN '" + first_day.ToString("yyyy-MM-dd") + "' --CONVERT(DATETIME, '01/07/2018', 103) AND\n" +
                "                  AND '" + last_day.ToString("yyyy-MM-dd") + "' --CONVERT(DATETIME, '31/07/2018', 103)\n" +
                "\n" +
                "                  AND pch.BRANCH = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                "       ) a\n" +
                "                       INNER JOIN Branches b\n" +
                "                            ON  a.BRANCH = b.branchCode\n" +
                "                       LEFT OUTER JOIN ExpressCenters e\n" +
                "                            ON  a.express_center = e.expressCenterCode\n" +
                "            ) BB\n" +
                "            ON  cr.company = bb.company\n" +
                "            AND cr.branch = bb.branch\n" +
                "               WHERE  cr.company = '" + dd_company.SelectedValue.ToString() + "'\n" +
                "and cr.[year] = '" + dd_year.SelectedValue.ToString() + "'\n" +
                "and cr.[month] = '" + dd_month.SelectedValue.ToString() + "'\n" +
                "AND cr.branch = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                "GROUP BY\n" +
                "       cr.company,\n" +
                "       cr.branch,\n" +
                "       cr.opening_pcash,\n" +
                "       cr.petty_cash, cr.[month], cr.[year]\n" +
                "ORDER BY\n" +
                "       cr.branch,\n" +
                "       2";



                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    val = double.Parse(ds.Tables[0].Rows[0]["ClosingPcash"].ToString());
                }

            }
            catch (Exception Err)
            {
            }
            finally
            { }
            return val;
        }
    }
}