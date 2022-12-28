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
    public partial class PettyCash_ReceiptEdit : System.Web.UI.Page
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
                if (Request.QueryString["MODE"] != null)
                {
                    if (Request.QueryString["MODE"].ToString() == "EDIT")
                    {
                        // calendar();
                        btn_add_Click(sender, e);
                    }
                }
                else if (Session["HO_CIH"] != null)
                {
                    if (Session["parameters_list"] != null)
                    {
                        #region parameter maintaining list
                        List<string> Parameter = (List<string>)Session["parameters_list"];
                        txt_frmdate.Text = Parameter[0].ToString();
                        txt_todate.Text = Parameter[1].ToString();
                        dd_zone.Items.FindByValue(Parameter[5].ToString()).Selected = true;
                        dd_zone_Changed(sender, e);
                        dd_branch.Items.FindByValue(Parameter[2].ToString()).Selected = true;

                        btn_add_Click(sender, e);
                        Session["parameters_list"] = null;
                        Session["BRANCH_CIH"] = null;
                        #endregion
                    }
                }

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
            if (Session["parameters_list"] != null)
            {

            }
            else
            {
                Get_Branches();
            }
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
            if (Request.QueryString["MODE"] != null)
            {
                if (Request.QueryString["MODE"].ToString() == "EDIT")
                {
                    id = "and d.head_id='" + Request.QueryString["head_ID"].ToString() + "'  ";
                }
            }
            else
            {
                if (dd_branch.SelectedValue.ToString() != "")
                {
                    branch = " and h.BRANCH='" + dd_branch.SelectedValue.ToString() + "' ";
                }
            }


            string sqlString = "select h.ID,\n" +
            "       b.name branch,\n" +
            "\tCONVERT(varchar, d.[date], 103) date,\n" +
            "\td.amount amount,\n" +
            "\td.chque_no,\n" +
            "\tcm.[desc] cash_type,\n" +
            "       CONVERT(varchar, h.Created_ON, 103) created_on\n" +
            "  from PC_CIH_head h\n" +
            " inner join PC_CIH_detail d\n" +
            "    on d.HEAD_ID = h.ID\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = h.BRANCH\n" +
            "  left outer join ExpressCenters e\n" +
            "    on e.expressCenterCode = h.express_center\n" +
            "\tinner join PC_cash_mode cm\n" +
            "\ton cm.ID=d.cash_type\n" +
            " INNER JOIN PC_MONTH_PERIOD PMP\n" +
            " ON PMP.[YEAR]=H.[YEAR]\n" +
            " AND PMP.[MONTH]=H.[MONTH]\n" +
            " AND PMP.status_desc='OPEN'\n" +
            " where\n" +
            " d.[status]='0' " + id + " \n";
            if (txt_frmdate.Text != "")
            {
                sqlString += " and  d.[DATE] BETWEEN CONVERT(DATETIME, '" + txt_frmdate.Text + "', 103) AND CONVERT(DATETIME, '" + txt_todate.Text + "', 103) ";
            }
            sqlString +=
            " \n" +
            " " + branch + "  ";
            sqlString +=
            //"   and h.CREATED_BY = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
            "   and h.MONTH=(SELECT MAX(MONTH) FROM PC_MONTH_PERIOD WHERE YEAR = (SELECT MAX(YEAR) FROM   PC_MONTH_PERIOD)  )  and h.year=(SELECT MAX(year) FROM PC_MONTH_PERIOD ) order by h.ID desc, h.Created_ON";


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


                #region parameter maintaining list
                List<string> Parameter = new List<string>();
                Parameter.Add(txt_frmdate.Text);
                Parameter.Add(txt_todate.Text);
                Parameter.Add(dd_branch.SelectedValue.ToString());
                Parameter.Add("");
                Parameter.Add("BRANCH");
                Parameter.Add(dd_zone.SelectedValue.ToString());
                Session["parameters_list"] = Parameter;
                Session["HO_CIH"] = "1";
                #endregion


                HiddenField hf_headID = (HiddenField)row.FindControl("hf_head_id");

                Response.Redirect("PettyCashReceipt_voucherEdit.aspx?id=" + hf_headID.Value + "&stat=BRANCH_CIH");
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
            string st_date = "";
            DataSet ds_statring_Date = Dates();
            if (ds_statring_Date.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds_statring_Date.Tables[0].Rows[0][0].ToString()))
                {
                    string start_date = "01/" + ds_statring_Date.Tables[0].Rows[0][1].ToString() + "/" + ds_statring_Date.Tables[0].Rows[0][0].ToString();
                    st_date = DateTime.Parse(start_date).ToString();
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(st_date))
            {
                CalendarExtender1.StartDate = DateTime.Parse(st_date.ToString());
                CalendarExtender1.SelectedDate = DateTime.Parse(st_date.ToString());

                CalendarExtender2.StartDate = DateTime.Parse(st_date.ToString());
                CalendarExtender2.SelectedDate = DateTime.Parse(st_date.ToString());
            }

            CalendarExtender1.EndDate = DateTime.Now;

            CalendarExtender2.EndDate = DateTime.Now;
            CalendarExtender2.SelectedDate = DateTime.Now.Date;

        }

    }
}