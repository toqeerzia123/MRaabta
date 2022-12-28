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
    public partial class PettyCash_D2BEdit : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function bfunc = new bayer_Function();
        CommonFunction cfunc = new CommonFunction();
        clsVariables clsvar = new clsVariables();
        Cl_Variables cl_var = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get_Branches();
                Get_Zones();
                //if (Request.QueryString["MODE"] != null)
                //{
                //    if (Request.QueryString["MODE"].ToString() == "EDIT")
                //    {
                //        btn_add_Click(sender, e);
                //    }
                //}
                //else if (Session["HO_CIH"] != null)
                //{
                //    if (Session["parameters_list"] != null)
                //    {
                //        #region parameter maintaining list
                //        List<string> Parameter = (List<string>)Session["parameters_list"];
                //        txt_frmdate.Text = Parameter[0].ToString();
                //        txt_todate.Text = Parameter[1].ToString();
                //        dd_branch.Items.FindByValue(Parameter[2].ToString()).Selected = true;


                //        btn_add_Click(sender, e);
                //        Session["parameters_list"] = null;
                //        Session["BRANCH_CIH"] = null;
                //        #endregion
                //    }
                //}

                calendar();
            }
        }
        protected void btn_add_Click(object sender, EventArgs e)
        {
            lbl_error.Text = "";
            DataSet ds = new DataSet();
            try
            {
                ds = Mainhead();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["dt"] = ds.Tables[0];
                    GridView.DataSource = ds.Tables[0];
                    GridView.DataBind();
                    //fillDD();
                    //btn_view_print_all.Visible = true;


                }
                else
                {
                    lbl_error.Text = "No Data Found!!";
                    GridView.DataSource = null;
                    GridView.DataBind();
                    //  btn_view_print_all.Visible = false;

                }
            }
            catch
            {
                lbl_error.Text = "The report cannot be generated due to error!!";
                GridView.DataSource = null;
                GridView.DataBind();

                //    btn_view_print_all.Visible = false;
                return;
            }
            finally
            {
            }
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

        public DataSet Mainhead()
        {
            string id = "";
            DataSet Ds_1 = new DataSet();

            string branch = "";
            string ec = "";
            //if (Request.QueryString["MODE"] != null)
            //{
            //    if (Request.QueryString["MODE"].ToString() == "EDIT")
            //    {
            //        id = "and d.head_id='" + Request.QueryString["head_ID"].ToString() + "'  ";
            //    }
            //}
            //else
            //{
            //    if (dd_branch.SelectedValue.ToString() != "")
            //    {
            //        branch = " and h.BRANCH='" + dd_branch.SelectedValue.ToString() + "' ";
            //    }
            //}


            string sqlString = "SELECT d.Id\n" +
            "      ,d.ChequeNo\n" +
            "      ,CONVERT(varchar, d.ChequeDate, 103) cheque_date\n" +
            "      ,d.Amount\n" +
            "      ,d.remaining_amount\n" +
            "      ,d.BranchCode\n" +
            "      ,CONVERT(varchar, d.CreatedOn, 103) created_on\n" +
            "      ,d.CreatedBy\n" +
            "      ,d.ReceiptNo\n" +
            "      ,d.DepositSlipNo\n" +
            "      ,d.DepositSlipBankID\n" +
            "      ,d.company\n" +
            "      ,d.zonecode\n" +
            "    ,c.sdesc_OF company_name\n" +
            "    ,b.[name] branch_name\n" +
            "    ,z.Name cuser_name\n" +
            "    ,ba.bank_name \n" +
            "  FROM Deposit_to_bank d\n" +
            "  inner join Branches b\n" +
            "  on b.branchCode=d.BranchCode\n" +
            "  inner join COMPANY_OF c\n" +
            "  on c.code_OF=d.company\n" +
            "  inner join ZNI_USER1 z\n" +
            "  on z.U_ID=d.CreatedBy\n" +
            "  left outer join banks_of ba\n" +
            "  on ba.bank_code=d.DepositSlipBankID\n" +
            "  where d.BranchCode='" + dd_branch.SelectedValue.ToString() + "'\n";
            if (txt_frmdate.Text != "")
            {
                sqlString += "  and  d.ChequeDate BETWEEN CONVERT(DATETIME, '" + txt_frmdate.Text + "', 103) AND CONVERT(DATETIME, '" + txt_todate.Text + "', 103)";
            }
            sqlString += " order by d.CreatedOn desc,d.Id desc ";

            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();
            SqlCommand orcd = new SqlCommand(sqlString, orcl);
            orcd.CommandType = CommandType.Text;
            SqlDataAdapter oda = new SqlDataAdapter(orcd);
            oda.Fill(Ds_1);
            orcl.Close();

            return Ds_1;
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "redirect")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                GridViewRow row = (GridViewRow)GridView.Rows[RowIndex];


                //#region parameter maintaining list
                //List<string> Parameter = new List<string>();
                //Parameter.Add(txt_frmdate.Text);
                //Parameter.Add(txt_todate.Text);
                //Parameter.Add(dd_branch.SelectedValue.ToString());
                //Parameter.Add("");
                //Parameter.Add("BRANCH");
                //Session["parameters_list"] = Parameter;
                //Session["HO_CIH"] = "1";
                //#endregion


                HiddenField hf_headID = (HiddenField)row.FindControl("hf_head_id");

                Response.Redirect("CIH2BankVoucherEdit.aspx?id=" + hf_headID.Value + "&stat=");
                //Redirect to the edit page
                //  ScriptManager.RegisterStartupScript(Page, typeof(Page),"", "window.open('PettyCash_voucherEdit.aspx?head_id=" + hf_headID.Value + "&id=" + hf_detailID.Value + "');", true);

            }
        }


        public DataSet Dates()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string table = "";

                table = "PC_date_PERIOD";

                string sqlString = "SELECT MAX(YEAR),MAX(MONTH) FROM PC_MONTH_PERIOD AS pmp WHERE PMP.status_desc='OPEN'\n";
                //"WHERE P.MONTH='" + dd_month.SelectedValue.ToString() + "'\n" +
                //"AND P.YEAR='" + dd_year.SelectedItem.ToString() + "'";

                sqlString = "SELECT pmp.[YEAR], pm.mon MONTH\n" +
                "  FROM PC_MONTH_PERIOD pmp\n" +
                " INNER JOIN pc_month pm\n" +
                "    ON pm.mon = pmp.[MONTH]\n" +
                " WHERE pmp.CLOSED_ON = (SELECT MAX(closed_ON) FROM PC_MONTH_PERIOD pmp2)\n" +
                "   AND pmp.[status] = '2'";


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

            #region Closing Date
            DateTime? st_date = null;
            DataSet ds_statring_Date = Dates();
            if (ds_statring_Date.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds_statring_Date.Tables[0].Rows[0][0].ToString()))
                {
                    //string start_date = "01/" + ds_statring_Date.Tables[0].Rows[0][1].ToString() + "/" + ds_statring_Date.Tables[0].Rows[0][0].ToString();
                    string start_date = ds_statring_Date.Tables[0].Rows[0][0].ToString() + "/" + ds_statring_Date.Tables[0].Rows[0][1].ToString() + "/" + "01";
                    st_date = DateTime.Parse(start_date);
                }
            }
            #endregion

            if (st_date.HasValue)
            {
                CalendarExtender1.StartDate = st_date.Value;
                CalendarExtender1.SelectedDate = st_date.Value;

                CalendarExtender2.StartDate = st_date.Value;
                CalendarExtender2.SelectedDate = st_date.Value;
            }

            CalendarExtender1.EndDate = DateTime.Now;

            CalendarExtender2.EndDate = DateTime.Now;
            CalendarExtender2.SelectedDate = DateTime.Now.Date;

        }

    }
}