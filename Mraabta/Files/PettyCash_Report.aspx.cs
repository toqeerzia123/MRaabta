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
    public partial class PettyCash_Report : System.Web.UI.Page
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
                Get_Zones();
                //Get_Branches();
                Get_Companies();
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
                    GridView.DataSource = ds.Tables[0];
                    GridView.DataBind();
                    btn_view_print_all.Visible = true;
                }
                else
                {
                    lbl_error.Text = "No Data Found!!";
                    GridView.DataSource = null;
                    GridView.DataBind();
                    btn_view_print_all.Visible = false;
                }
            }
            catch
            {
                lbl_error.Text = "The report cannot be generated due to error!!";
                GridView.DataSource = null;
                GridView.DataBind();
                btn_view_print_all.Visible = false;
                return;
            }
            finally
            {
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
            Get_ExpressCenter();
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
                dd_ec.Items.Insert(0, new ListItem("Select Express Center", ""));

            }
            else
            {
                dd_ec.Items.Insert(0, new ListItem("Branch", "0"));
            }
        }
        protected void dd_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_ExpressCenter();
        }
        public DataSet Mainhead()
        {
            DataSet Ds_1 = new DataSet();

            string branch = "";
            string ec = "";

            if (dd_branch.SelectedValue.ToString() != "")
            {
                branch = " and h.BRANCH='" + dd_branch.SelectedValue.ToString() + "' ";
            }
            if (dd_ec.SelectedValue.ToString() != "")
            {
                ec = " and h.express_center='" + dd_ec.SelectedValue.ToString() + "' ";
            }
            string status = "";
            if (dd_status.SelectedValue.ToString() != "")
            {
                status = " and d.status='" + dd_status.SelectedValue.ToString() + "' ";
            }

            string sqlString = "select h.ID,\n" +
            "       b.name branch,\n" +
            "      CASE h.express_center  WHEN '0' THEN 'Branch' ELSE e.name END ec,\n" +
            "       CONVERT(varchar, h.Created_ON, 106) created_on,h.company\n" +
            "  from pc_head h   inner join PC_DETAIL d on d.HEAD_ID=h.ID\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = h.BRANCH\n" +
            " left outer join ExpressCenters e\n" +
            "    on e.expressCenterCode = h.express_center\n" +
           " where \n" +
               " d.[DATE] BETWEEN CONVERT(DATETIME, '" + txt_frmdate.Text + "', 103) AND CONVERT(DATETIME, '" + txt_todate.Text + "', 103) ";
            sqlString +=
            " \n" +
            " " + branch + " " + ec + " " + status + "  ";
            sqlString +=
            "  and h.company='" + dd_company.SelectedValue.ToString() + "'\n" +
            "  order by h.ID desc, h.Created_ON";

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
            if (e.CommandName == "view")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GridView.Rows[rowIndex];

                //Fetch value of ID.
                string id = row.Cells[1].Text;

                //Redirect to the print view page
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('PettyCash_htmlprintall.aspx?ID=" + id + "&Mode=EDIT&company=" + dd_company.SelectedValue.ToString() + "');", true);
            }
        }
        protected void btn_view_print_all_Click(object sender, EventArgs e)
        {
            string id = "";
            foreach (GridViewRow gr in GridView.Rows)
            {
                CheckBox check = (CheckBox)gr.FindControl("cb_check");
                HiddenField Hd_Sno = (HiddenField)gr.FindControl("Hd_ID");

                if (check.Checked == true)
                {
                    id += Hd_Sno.Value.ToString() + ",";
                }
            }
            if (!string.IsNullOrEmpty(id))
            {
                id = id.TrimEnd(',');

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "PettyCash_htmlprintall.aspx?ID=" + id + "&company=" + dd_company.SelectedValue.ToString() + "", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

            }


            //string year = "";
            //string month = "";
            //string branch = "";
            //string ec = "";
            //year = dd_year.SelectedItem.ToString();
            //if (dd_month.SelectedValue.ToString() != "")
            //{
            //    month = dd_month.SelectedValue.ToString();
            //}
            //branch = dd_branch.SelectedValue.ToString();
            //if (dd_ec.SelectedValue.ToString() != "")
            //{
            //    ec = dd_ec.SelectedValue.ToString();
            //}
            //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            //string script = String.Format(script_, "PettyCash_print_all.aspx?year=" + year + "&month=" + month + "&branch=" + branch + "&ec=" + ec, "_blank", "");
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
    }
}