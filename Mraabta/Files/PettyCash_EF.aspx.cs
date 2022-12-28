using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Globalization;

namespace MRaabta.Files
{
    public partial class PettyCash_EF : System.Web.UI.Page
    {
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

                //Get_Month();
                GetYear();
                //dd_month.SelectedValue = DateTime.Now.ToString("MM");
                //Get_Branches();
                Get_Zones();
                Get_MainHead();
                Get_Companies();
                Get_Division();
                Get_proj();
                Get_Product();
                Get_Subprod();
                Get_subdept();
                month_cih_check();
                //Checking entry
                check();
                // txt_date_TextChanged(sender, e);

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
                    calendar();
                }
            }
            catch (Exception ex)
            { }

        }
        public void month_cih_check()
        {

            string sqlString = "select * from CIH_remainings c\n" +
            "where c.month='" + dd_month.SelectedValue.ToString() + "'\n" +
            "and c.branch='" + dd_branch.SelectedValue.ToString() + "' and year='" + dd_year.SelectedItem.ToString() + "' and c.company='" + dd_company.SelectedValue.ToString() + "'";
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
                    if (!string.IsNullOrEmpty(Ds_1.Rows[0]["value"].ToString()))
                    {
                        double val = double.Parse(Ds_1.Rows[0]["value"].ToString());
                        hf_value.Value = val.ToString();
                    }
                    else
                    {
                        hf_value.Value = "0";
                    }
                    if (!string.IsNullOrEmpty(Ds_1.Rows[0]["petty_cash"].ToString()))
                    {
                        double p_cash = double.Parse(Ds_1.Rows[0]["petty_cash"].ToString());
                        hf_pamount.Value = p_cash.ToString();
                        if (p_cash <= 0)
                        {
                            hf_pamount.Value = "0";
                            lbl_cih.Text = "There is no Petty Cash Amount For the Month " + dd_month.SelectedItem.ToString() + " ";
                            return;
                        }
                        else
                        {

                            lbl_cih.Text = "The Petty Cash Amount For the Month " + dd_month.SelectedItem.ToString() + " is : " + p_cash.ToString("N0") + " ";
                        }
                    }
                    else
                    {
                        hf_pamount.Value = "0";
                        lbl_cih.Text = "There is no Petty Cash Amount For the Month " + dd_month.SelectedItem.ToString() + " ";
                        return;
                    }
                }
                else
                {
                    hf_pamount.Value = "0";
                    lbl_cih.Text = "There is no Petty Cash Amount For the Month " + dd_month.SelectedItem.ToString() + " ";
                    return;
                }
            }

            catch (Exception Err)
            { }
            finally
            { }

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
            Get_ExpressCenter();
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
            dd_branch_SelectedIndexChanged(sender, e);
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
                dd_ec.SelectedValue = ds.Tables[0].Rows[0]["express_center"].ToString();
                dd_zone.SelectedValue = ds.Tables[0].Rows[0]["ZONECODE"].ToString();

                calendar();
                dd_zone.Enabled = false;
                dd_branch.Enabled = false;
                dd_ec.Enabled = false;
                dd_year.Enabled = false;
                dd_month.Enabled = false;
                dd_company.Enabled = false;

                btn_submit.Visible = true;
                btn_clear.Visible = true;
                hf_status.Value = "1";

                DataSet ds_detail = new DataSet();
                ds_detail = GetTempData();

                if (ds_detail.Tables[0].Rows.Count > 0)
                {
                    object sumObject;
                    sumObject = ds_detail.Tables[0].Compute("Sum(Amount)", "");
                    string amount_total = sumObject.ToString();

                    #region month cih checking
                    month_cih_check();
                    double p_amount = int.Parse(hf_pamount.Value.ToString()) - int.Parse(sumObject.ToString());
                    hf_pamount.Value = p_amount.ToString();
                    lbl_cih.Text = "The Remaining Petty Cash Amount For the Month " + dd_month.SelectedItem.ToString() + " : is " + p_amount.ToString("N0") + " ";
                    #endregion


                    DataRow gt_row = ds_detail.Tables[0].NewRow();
                    gt_row["AMount"] = double.Parse(amount_total).ToString();
                    // gt_row[0] = "Total";
                    ds_detail.Tables[0].Rows.Add(gt_row);
                    // tbl_form_type.Visible = false;
                    ViewState["dt"] = ds_detail.Tables[0];
                    GridView.DataSource = ds_detail.Tables[0];
                    GridView.DataBind();
                    btn_add.Visible = true;
                    dd_eh.Items.FindByText(ds_detail.Tables[0].Rows[0]["expense"].ToString()).Selected = true;
                    Get_subHead();
                    dd_eh.Enabled = false;
                    Button lb = (Button)GridView.Rows[GridView.Rows.Count - 1].Cells[0].Controls[0];
                    lb.Visible = false;
                }
                else
                {
                }
            }
            else
            {
                //Get ID for the new entry
                //double id = Get_maxhead();
                //if (!string.IsNullOrEmpty(id.ToString()))
                //{
                //    txt_ID.Text = id.ToString();
                //}
            }
        }
        public void Get_ExpressCenter()
        {
            DataSet ds = new DataSet();
            cl_var.origin = dd_branch.SelectedValue.ToString();
            ds = cfunc.ExpressCenterOrigin(cl_var);
            dd_ec.Items.Clear();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_ec.DataTextField = "Name";
                dd_ec.DataValueField = "expressCenterCode";
                dd_ec.DataSource = ds.Tables[0].DefaultView;
                dd_ec.DataBind();
                dd_ec.Items.Insert(0, new ListItem("Branch", "0"));
            }
            else
            {
                dd_ec.Items.Insert(0, new ListItem("Branch", "0"));
            }
        }
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
                dd_eh.Items.Insert(0, new ListItem("Select Expense", "0"));
            }
        }
        public void Get_subHead()
        {
            DataSet ds = new DataSet();
            ds = subhead();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_sh.DataTextField = "sub_desc";
                dd_sh.DataValueField = "subcode";
                dd_sh.DataSource = ds.Tables[0].DefaultView;
                dd_sh.DataBind();


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

                string query = "select * from PROJECTS_OF order by code ASC";
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
        protected void dd_prod_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_Subprod();
        }
        protected void dd_dept_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region selected index code
        protected void dd_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_ExpressCenter();
            month_cih_check();
        }
        protected void dd_eh_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_add.Visible = true;
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
        public DataSet GetTempData()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string sqlString = "select D.ID, CONVERT(varchar, d.DATE, 103) date ,\n" +
                "m.description expense,\n" +
                "s.sub_desc descr,\n" +
                "d.NARRATE,\n" +
                "d.AMOUNT,\n" +
                "CONVERT(varchar, d.Created_ON, 103) Created_ON\n" +
                " from PC_temp_DETAIL d\n" +
                "inner join pc_mainHead m\n" +
                "on m.code=d.EXPENSE\n" +
                //"and d.HEAD_ID='"++"'\n" +
                "inner join pc_subHead s\n" +
                "on s.subcode=d.[DESC] and s.headcode=d.EXPENSE\n" +
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
                string sqlString = "select * from pc_temp_head \n" +
                "where\n" +
                "CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'";
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
        public DataSet available_CIH(string date)
        {
            DataSet Ds_1 = new DataSet();
            try
            {//comment
                string sqlString = "select SUM(m.cih) cih,SUM(m.expense),SUM(m.cih)-SUM(m.expense) diff from (\n" +
    "select isnull(sum(cd.amt_rmain),0) cih,0 expense\n" +
    "from\n" +
                " PC_CIH_detail cd inner join PC_CIH_head h on h.ID=cd.head_id\n" +
                "where cd.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "'\n" +
                "  and h.YEAR='" + dd_year.SelectedItem.ToString() + "' and   CONVERT(varchar, cd.date, 103) <='" + DateTime.Parse(date).ToString("dd/MM/yyyy") + "'  AND H.express_center='" + dd_ec.SelectedValue.ToString() + "' union all\n" +
                "select 0 cih,isnull(sum(cd.AMOUNT),0) expense from\n" +
                "PC_temp_detail cd inner join PC_TEMP_head h on h.ID=cd.head_id\n" +
                "where cd.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "'\n" +
                "  and h.YEAR='" + dd_year.SelectedItem.ToString() + "' and   CONVERT(varchar, cd.date, 103) <='" + DateTime.Parse(date).ToString("dd/MM/yyyy") + "'  AND H.express_center='" + dd_ec.SelectedValue.ToString() + "' union all\n" +
                "select 0 cih,isnull(sum(cd.AMOUNT),0) expense from\n" +
                "PC_detail cd inner join PC_head h on h.ID=cd.head_id\n" +
                "where cd.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "' AND H.express_center='" + dd_ec.SelectedValue.ToString() + "'  and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "' and   CONVERT(varchar, cd.date, 103) <='" + DateTime.Parse(date).ToString("dd/MM/yyyy") + "'  and h.YEAR='" + dd_year.SelectedItem.ToString() + "' ) m";

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
        public DataSet available_pettyAmount()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string sqlString = "select sum(d.AMOUNT) amount from PC_DETAIL d inner join pc_head h\n" +
                "on h.ID=d.HEAD_ID\n" +
                "and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "'\n" +
                "and h.YEAR='" + dd_year.SelectedItem.ToString() + "'\n" +
                "and h.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'";

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
        public DataSet month_cih()
        {
            DataSet Ds_1 = new DataSet();
            try
            {

                string sqlString = "select d.ID,d.head_id,d.CREATED_BY,d.Created_ON,d.[date],d.amt_rmain from PC_CIH_detail d\n" +
                "inner join PC_CIH_head h\n" +
                "on h.ID=d.head_id\n" +
                "and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "'\n" +
                "and h.YEAR='" + dd_year.SelectedItem.ToString() + "'\n" +
                "and h.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'";

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
        public DataSet available_CIH()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string sqlString = "select SUM(m.cih) cih,SUM(m.expense),SUM(m.cih)-SUM(m.expense) diff from (\n" +
    "select isnull(sum(cd.amt_rmain),0) cih,0 expense\n" +
    "from\n" +
    " PC_CIH_detail cd inner join PC_CIH_head h on h.ID=cd.head_id\n" +
    "where cd.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "'\n" +
    "  and h.YEAR='" + dd_year.SelectedItem.ToString() + "' and   CONVERT(varchar, cd.date, 103) <='" + DateTime.Parse(txt_date.Text).ToString("dd/MM/yyyy") + "'   union all\n" +
    //"select 0 cih,isnull(sum(cd.AMOUNT),0) expense from\n" +
    //"PC_temp_detail cd inner join PC_TEMP_head h on h.ID=cd.head_id\n" +
    //"where cd.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "'\n" +
    //"  and h.YEAR='" + dd_year.SelectedItem.ToString() + "' and   CONVERT(varchar, cd.date, 103) <='" + DateTime.Parse(txt_date.Text).ToString("dd/MM/yyyy") + "'   union all\n" +
    "select 0 cih,isnull(sum(cd.AMOUNT),0) expense from\n" +
    "PC_detail cd inner join PC_head h on h.ID=cd.head_id\n" +
    "where cd.CREATED_BY='" + HttpContext.Current.Session["U_ID"].ToString() + "'  and h.MONTH='" + int.Parse(dd_month.SelectedValue) + "' and   CONVERT(varchar, cd.date, 103) <='" + DateTime.Parse(txt_date.Text).ToString("dd/MM/yyyy") + "'  and h.YEAR='" + dd_year.SelectedItem.ToString() + "' ) m";

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

        public void Insert_headEntry()
        {
            try
            {

                string sqlString1 = "UPDATE systemcodes\n" +
              "SET    codeValue      = codeValue + 1,\n" +
              "       [year]         = YEAR\n" +
              "       OUTPUT DELETED.year + DELETED.codeValue\n" +
              "WHERE  codeType       = 'PETTY_EXPENSE'\n" +
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

                string query_head = "INSERT INTO PC_temp_HEAD(ID,COMPANY,BRANCH,EXPRESS_CENTER,YEAR,MONTH,CREATED_BY" +
              " ,Created_ON,status,zonecode)\n" +
              "VALUES(\n" +
              " '" + Int64.Parse(txt_ID.Text) + "' ,\n" +
              " '" + dd_company.SelectedValue.ToString() + "' ,\n" +
              " '" + dd_branch.SelectedValue.ToString() + "' ,\n" +
              " '" + dd_ec.SelectedValue.ToString() + "' ,\n" +
              " '" + dd_year.SelectedItem.ToString() + "' ,\n" +
              " '" + dd_month.SelectedValue.ToString() + "' ,\n" +
              " '" + HttpContext.Current.Session["U_ID"].ToString() + "' ,\n" +
              " GETDATE(),'0','" + dd_zone.SelectedValue.ToString() + "'\n" +
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
        public void Insert_DetailEntry()
        {
            try
            {
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

                int count_ = Get_maxdet(Int64.Parse(txt_ID.Text));
                string sqlString = "INSERT INTO PC_temp_DETAIL(ID,HEAD_ID,DATE,EXPENSE,[DESC],AMOUNT,NARRATE,CREATED_BY,Created_ON,status" +
                ",division,project,sub_prod,sub_dept)\n" +
                "VALUES\n" +
                    "('" + count_ + "','" + Int64.Parse(txt_ID.Text) + "',\n" +
                    " '" + txt_date.Text + "', \n" +
                    " '" + dd_eh.SelectedValue.ToString() + "',\n" +
                    " '" + dd_sh.SelectedValue.ToString() + "',\n" +
                    " '" + txt_amount.Text + "',\n" +
                    " '" + txt_narrate.Text.Replace("'", " ") + "',\n" +
                " '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                " GETDATE(),0,\n" +
                " '" + div + "', " +
                " '" + proj + "', " +
                " '" + subprod + "', " +
                " '" + subdept + "' " +
                " )";

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
                string query_head = "INSERT INTO PC_HEAD \n" +
                "select * from PC_temp_HEAD where id='" + Int64.Parse(txt_ID.Text) + "' and created_by='" + HttpContext.Current.Session["U_ID"].ToString() + "'\n";
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




                string query_head = "INSERT INTO PC_detail \n" +
                "select * from PC_temp_detail where head_id='" + Int64.Parse(txt_ID.Text) + "'\n";
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

        public int Delete_tempHead()
        {
            int i = 0; ;
            try
            {
                string sqlString = "delete from PC_temp_head where id='" + Int64.Parse(txt_ID.Text) + "'\n";

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
        public int update_cih_remaining()
        {
            int i = 0; ;
            try
            {
                //string sqlString = " update CIH_remainings set petty_cash=" + double.Parse(hf_pamount.Value.ToString()) + " " +
                //    " where branch='" + dd_branch.SelectedValue.ToString() + "' and month='" + dd_month.SelectedValue.ToString() + "' " +
                //    " and year='" + dd_year.SelectedItem.ToString() + "' and company='" + dd_company.SelectedValue.ToString() + "' \n";

                //SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                //orcl.Open();
                //SqlCommand orcd = new SqlCommand(sqlString, orcl);
                //orcd.CommandType = CommandType.Text;
                //SqlDataAdapter oda = new SqlDataAdapter(orcd);
                //orcd.ExecuteNonQuery();
                //orcl.Close();

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
                string sqlString = "delete from PC_temp_detail where head_id='" + Int64.Parse(txt_ID.Text) + "'\n";

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
        public int Delete_cih_Detail(string id)
        {
            int i = 0; ;
            try
            {
                string sqlString = "delete from PC_cih_detail where head_id in (" + id + ")\n";

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
                string cond = "";
                if (id != 0)
                {
                    cond = ID = " id='" + id + "' and ";
                }
                string sqlString = "delete from PC_temp_DETAIL where " + cond + " head_id='" + Int64.Parse(txt_ID.Text) + "'\n";

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
        public DataTable gettingAmountOf_voucher(int id, string sqlString)
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
                "WHERE  codeType       = 'PETTY_EXPENSE'\n" +
                "       AND [year]     = YEAR(GETDATE())";


                //string sqlString = " select max(ID) ID from  ( select max(h.ID) + 1 ID  from PC_HEAD h\n" +
                //"union all\n" +
                //"select max(h.ID) + 1 ID  from PC_temp_HEAD h ) a";

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
                        maxid = 2018000001;
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
        public int Get_maxdet(Int64 head_id)
        {
            int maxid = 0;
            DataSet ds = new DataSet();
            try
            {
                string max_id = " select max(id)+1 id from PC_temp_DETAIL where HEAD_ID='" + head_id + "'";
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

        #region Button Click
        protected void btn_add_Click(object sender, EventArgs e)
        {//comment

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
            //if (DateTime.Parse(DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd")) >= CalendarExtender1.StartDate && DateTime.Parse(DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd")) <= CalendarExtender1.EndDate)
            //{
            //}
            //else
            //{
            //    lbl_error.Text = "Date is not Under specified range!!";
            //    return;
            //}
            #endregion
            if (txt_amount.Text == "")
            {
                lbl_error.Text = "Write Amount!!";
                return;
            }
            month_cih_check();
            lbl_err2.Text = "";
            lbl_error.Text = "";
            //  lbl_cih.Text = "";
            if (dd_eh.SelectedValue.ToString() != "")
            {
                //int amount = int.Parse(txt_amount.Text);
                lbl_err2.Text = "";
                lbl_error.Text = "";
                lbl_cih.Text = "";
                int j = 0;
                int sum_amount = 0;
                //CIH Checking

                lbl_err2.Text = "";
                lbl_error.Text = "";
                //    lbl_cih.Text = "";

                //----------------CHECKING CASH IN HAND---------------------------//
                if (int.Parse(hf_pamount.Value.ToString()) >= int.Parse(txt_amount.Text))
                {
                    if (hf_status.Value.ToString() == "")
                    {
                        //---------------------------Head Entry Insertion (One Time Entry)-------------------------------- //
                        Insert_headEntry();
                    }
                    //---------------------------Detail Entry Insertion (One Time Entry)----------------------------------//

                    Insert_DetailEntry();
                    btn_submit.Visible = true;
                    btn_clear.Visible = true;
                    lbl_error.Text = "";

                    //--------------------------Gridview Binding---------------------------------------------------------//
                    DataSet ds = new DataSet();
                    ds = GetTempData();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        object sumObject;

                        sumObject = ds.Tables[0].Compute("Sum(Amount)", "");
                        string amount_total = sumObject.ToString();

                        #region month cih checking
                        month_cih_check();
                        double p_amount = int.Parse(hf_pamount.Value.ToString()) - int.Parse(sumObject.ToString());
                        hf_pamount.Value = p_amount.ToString();
                        lbl_cih.Text = "The Remaining Petty Cash Amount For the Month " + dd_month.SelectedItem.ToString() + " : is " + p_amount.ToString("N0") + " ";
                        #endregion

                        DataRow gt_row = ds.Tables[0].NewRow();
                        gt_row["AMount"] = double.Parse(amount_total).ToString();
                        ds.Tables[0].Rows.Add(gt_row);
                        ViewState["dt"] = ds.Tables[0];
                        GridView.DataSource = ds.Tables[0];
                        GridView.DataBind();
                        Button lb = (Button)GridView.Rows[GridView.Rows.Count - 1].Cells[0].Controls[0];
                        lb.Visible = false;
                        dd_branch.Enabled = false;
                        dd_ec.Enabled = false;
                        dd_year.Enabled = false;
                        dd_month.Enabled = false;
                        dd_company.Enabled = false;
                        dd_eh.Enabled = false;
                        txt_date.Enabled = false;

                    }
                    else
                    {
                        GridView.DataSource = null;
                        GridView.DataBind();
                    }
                }
                else
                {
                    lbl_error.Text = "The expense amount exceeds the petty cash amount!!";
                    return;
                }
            }

            else
            {
                GridView.DataSource = null;
                GridView.DataBind();
                lbl_error.Text = "Select Form Type!!";
                return;

            }


        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            GridViewRow row = (GridViewRow)GridView.Rows[e.RowIndex];
            string id = row.Cells[1].Text;
            string date = row.Cells[2].Text;

            int i = Delete_DetailEntry(int.Parse(id));

            #region readjusting the Detail IDs in case of deletion it had got disturbed
            string sqlString = "select * from PC_TEMP_DETAIL where head_id='" + Int64.Parse(txt_ID.Text) + "'";
            DataTable dt = data_generate(sqlString);
            if (dt.Rows.Count > 0)
            {
                int val = 1;
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    if (int.Parse(dt.Rows[a]["ID"].ToString()) != val)
                    {
                        dt.Rows[a]["ID"] = val;
                    }
                    val++;
                }
            }

            //delete previous entry
            int stat1 = Delete_DetailEntry(0);
            if (stat1 == 1)
            {
                //Bulk Insertion Operation
                // connect to SQL
                using (SqlConnection connection =
                        new SqlConnection(clvar.Strcon()))
                {
                    // make sure to enable triggers
                    // more on triggers in next post
                    SqlBulkCopy bulkCopy =
                        new SqlBulkCopy
                        (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    // set the destination table name
                    bulkCopy.DestinationTableName = "PC_temp_detail";
                    connection.Open();

                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dt);
                    connection.Close();
                }
            }

            #endregion

            DataSet ds = new DataSet();
            ds = GetTempData();
            ViewState["dt"] = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                object sumObject;
                sumObject = ds.Tables[0].Compute("Sum(Amount)", "");
                string amount_total = sumObject.ToString();
                DataRow gt_row = ds.Tables[0].NewRow();
                gt_row["AMount"] = double.Parse(amount_total).ToString();
                // gt_row[0] = "Total";
                ds.Tables[0].Rows.Add(gt_row);
                tbl_form_type.Visible = false;
                ViewState["dt"] = ds.Tables[0];
                GridView.DataSource = ds.Tables[0];
                GridView.DataBind();
            }
            else
            {
                Delete_tempHead();
                Response.Redirect("pettyCash_EF.aspx");
            }

            Button lb = (Button)GridView.Rows[GridView.Rows.Count - 1].Cells[0].Controls[0];
            lb.Visible = false;

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
        #endregion

        public int cih_checking(string date, string amount1)
        {
            //comment
            int amount = int.Parse(amount1);
            DataSet ds_cih = new DataSet();
            ds_cih = available_CIH(date);
            if (ds_cih.Tables[0].Rows.Count > 0)
            {
                if (int.Parse(ds_cih.Tables[0].Rows[0]["cih"].ToString()) == 0)
                {
                    lbl_cih.Text = "There is no Cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString();
                    return 0;
                }
                else
                {//comment
                    int diff = int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) - amount;
                    if (int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) == 0)
                    {
                        lbl_cih.Text = "The Cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString() +
                            " is : " +
                            int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()).ToString("N0") + ".You cannot add more expenses";
                        return 0;
                    }//comment
                    else if (int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) < 0 || diff < 0)
                    {
                        lbl_cih.Text = "The amount exceeds the remaining cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString() +
                                               " is : " +
                                               int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()).ToString("N0");
                        return 0;
                    }
                    else if (int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) > 0)
                    {

                        lbl_cih.Text = "The remaining cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString() +
                           " is : " +
                           diff.ToString("N0");
                        return 1;

                    }


                }

            }
            return 0;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                //Insert PC Head Entry
                int head_stat = Insert_Final_headEntry();
                // int head_stat = 1;
                if (head_stat == 1)
                {
                    int detail_stat = Insert_Final_DetailEntry();
                    //int detail_stat = 1;
                    if (detail_stat == 1)
                    {
                        Delete_tempHead();
                        Delete_tempDetail();
                        // update_cih_remaining();
                        lbl_error.Text = UpdatePC_remaining();

                        hf_status.Value = "";
                        hf_headID.Value = "";

                        string sqlString = "SELECT pch.COMPANY,\n" +
                        "       pch.BRANCH,\n" +
                        "       pch.[YEAR],\n" +
                        "       pch.[MONTH],\n" +
                        "       CONVERT(VARCHAR, pcd.DATE, 103) \"Date\",\n" +
                        "       pch.express_center,\n" +
                        "       pcd.AMOUNT amount,\n" +
                        "       m.description expense,\n" +
                        "       s.sub_desc description,\n" +
                        "       pcd.NARRATE,\n" +
                        "       pch.ID,\n" +
                        "       cm.sdesc_OF comp1,\n" +
                        "       pch.CREATED_BY,\n" +
                        "       pch.Created_ON\n" +
                        "  FROM PC_head AS pch\n" +
                        " INNER JOIN PC_detail AS pcd\n" +
                        "    ON pcd.head_id = pch.ID\n" +
                        "  left outer join pc_mainHead m\n" +
                        "    on pcd.expense = m.code\n" +
                        "  left outer join pc_subHead s\n" +
                        "    on pcd.[desc] = s.subcode\n" +
                        "   and pcd.EXPENSE = s.headcode\n" +
                        " inner join COMPANY_OF cm\n" +
                        "    on cm.code_OF = pch.COMPANY\n" +
                        "   and pch.COMPANY = '" + dd_company.SelectedValue.ToString() + "'\n" +
                        " where cast(pch.Created_ON as date) = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'\n" +
                        "   and pch.BRANCH = '" + dd_branch.SelectedValue.ToString() + "'\n" +
                        "   and pch.CREATED_BY = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                        " order by pch.ID desc, pcd.ID";
                        DataTable dt = data_generate(sqlString);
                        if (dt.Rows.Count > 0)
                        {
                            object sumObject;

                            sumObject = dt.Compute("Sum(Amount)", "");
                            string amount_total = sumObject.ToString();

                            DataRow gt_row = dt.NewRow();
                            gt_row["AMount"] = double.Parse(amount_total).ToString();
                            dt.Rows.Add(gt_row);

                            dd_eh.Enabled = true;
                            txt_date.Enabled = true;
                            gridview2.DataSource = dt;
                            gridview2.DataBind();
                            month_cih_check();
                            GridView.DataSource = null;
                            GridView.DataBind();
                            btn_add.Visible = true;
                            txt_amount.Text = "";
                            txt_narrate.Text = "";
                        }
                        else
                        {
                            lbl_error.Text = "No Data Found!!";
                            return;
                        }
                        //Response.Redirect("pc_confirmation.aspx?id=" + Int64.Parse(txt_ID.Text) + "&type=EF");
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
            catch (Exception err)
            {
            }
            finally
            {
            }


        }
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                Delete_tempHead();
                Delete_tempDetail();
                Response.Redirect(Request.RawUrl);
            }
            else
            {
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
            month_cih_check();
            //txt_date_TextChanged(sender, e);
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
            string d = "01/" + dd_month.SelectedValue.ToString().PadLeft(2, '0') + "/" + dd_year.SelectedItem.ToString() + "";
            DateTime date_ = DateTime.ParseExact(d, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var lastDayOfMonth = DateTime.DaysInMonth(date_.Year, date_.Month);
            string ld = "" + lastDayOfMonth.ToString() + "/" + dd_month.SelectedValue.ToString().PadLeft(2, '0') + "/" + dd_year.SelectedItem.ToString() + "";
            DateTime ldate_ = DateTime.ParseExact(ld, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var firstDayOfMonth = new DateTime(date_.Year, date_.Month, 1);
            CalendarExtender1.StartDate = DateTime.Parse(firstDayOfMonth.ToString());
            CalendarExtender1.EndDate = ldate_;
        }

        public int cih_checking(int amount_sum)
        {

            int amount = amount_sum;
            DataSet ds_cih = new DataSet();
            ds_cih = available_CIH();
            if (ds_cih.Tables[0].Rows.Count > 0)
            {
                if (int.Parse(ds_cih.Tables[0].Rows[0]["cih"].ToString()) == 0)
                {
                    lbl_cih.Text = "There is no Cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString();
                    return 0;
                }
                else
                {
                    int diff = int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) - amount;
                    if (int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) == 0)
                    {
                        lbl_cih.Text = "The Cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString() +
                            " is : " +
                            int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()).ToString("N0") + ".You cannot add more expenses";
                        return 0;
                    }
                    else if (int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) < 0 || diff < 0)
                    {
                        lbl_cih.Text = "The amount exceeds the remaining cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString() +
                                               " is : " +
                                               int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()).ToString("N0");
                        return 0;
                    }
                    else if (int.Parse(ds_cih.Tables[0].Rows[0]["diff"].ToString()) > 0)
                    {

                        lbl_cih.Text = "The remaining cash in hand amount for the Selected month i.e. " + dd_month.SelectedItem.ToString() + "," + dd_year.SelectedItem.ToString() +
                           " is : " +
                           diff.ToString("N0");
                        return 1;

                    }


                }

            }
            return 0;
        }
        protected void txt_date_TextChanged(object sender, EventArgs e)
        {

            //if (txt_date.Text != "")
            //{
            //    DataSet ds_available_amt = available_CIH();
            //    if (ds_available_amt.Tables[0].Rows.Count > 0)
            //    {
            //        if (int.Parse(ds_available_amt.Tables[0].Rows[0]["cih"].ToString()) == 0)
            //        {
            //            lbl_cih.Text = "There is no Petty Cash Amount for the Selected Date";
            //            btn_add.Visible = false;
            //        }
            //        else
            //        {
            //            hf_available_amt.Value = ds_available_amt.Tables[0].Rows[0]["cih"].ToString();
            //            if (dd_ec.SelectedValue.ToString() != "0")
            //            {
            //                btn_add.Visible = true;
            //            }

            //            lbl_cih.Text = "The Petty Cash Amount for the Selected date is: " + int.Parse(ds_available_amt.Tables[0].Rows[0]["cih"].ToString()).ToString("N0");
            //        }

            //    }

            //}

        }
    }
}