using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class PettyCash_ApprovalReport : System.Web.UI.Page
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


                }
                else
                {
                    lbl_error.Text = "No Data Found!!";
                    GridView.DataSource = null;
                    GridView.DataBind();
                }
            }
            catch
            {
                lbl_error.Text = "The report cannot be generated due to error!!";
                GridView.DataSource = null;
                GridView.DataBind();
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
            dd_ec.Items.Clear();
            ds = cfunc.ExpressCenterOrigin(cl_var);
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



            string sqlString = "select concat(h.ID, '-', d.ID) as voucher_id,\n" +
            "       h.ID head_id,\n" +
            "       d.ID,\n" +
            "       CONVERT(varchar, d.[DATE], 103) date,\n" +
            "       s.sub_desc,\n" +
            "       m.[description] expense,\n" +
            "       d.AMOUNT,\n" +
            "       b.name branch,\n" +
            "       CASE h.express_center\n" +
            "         WHEN '0' THEN\n" +
            "          'Branch'\n" +
            "         ELSE\n" +
            "          e.name\n" +
            "       END ec,\n" +
            "       CONVERT(varchar, h.Created_ON, 103) created,d.status status_code,\n" +
            " case d.[status] when '1' then 'Approved' when '2' then 'Rejected' else 'Unapproved' end status,d.narrate,d.status ,\n" +
            " U.[Name] CREATED_BY,\n" +
            " CONVERT(varchar, d.Created_ON, 103) created_on,\n" +
            " U1.[Name] approved_BY,\n" +
            " CONVERT(varchar, d.approved_ON, 103) approved_on\n" +
            "   from pc_head h\n" +
            " inner join PC_DETAIL d\n" +
            "    on h.ID = d.HEAD_ID and h.CREATED_BY=d.CREATED_BY\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = h.BRANCH\n" +
            "  left outer join ExpressCenters e\n" +
            "    on e.expressCenterCode = h.express_center\n" +
            " inner join pc_mainHead m\n" +
            "    on m.code = d.EXPENSE\n" +
            " inner join pc_subHead s\n" +
            "    on m.code = s.headcode\n" +
            "   and d.[desc] = s.subcode\n" +
            "   INNER JOIN ZNI_USER1 U\n" +
            "   ON U.U_ID=D.CREATED_BY\n" +
            "   LEFT OUTER JOIN ZNI_USER1 U1\n" +
            "   ON U1.U_ID=d.APPROVED_BY\n" +
            "\n" +
            " where \n" +
            " d.[DATE] BETWEEN   CONVERT(DATETIME, '" + txt_frmdate.Text + "', 103) AND CONVERT(DATETIME, '" + txt_todate.Text + "', 103) ";

            sqlString +=
            " \n" +
            " " + branch + " " + ec + " " + status + "   order by h.ID desc, h.Created_ON";


            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();
            SqlCommand orcd = new SqlCommand(sqlString, orcl);
            orcd.CommandType = CommandType.Text;
            SqlDataAdapter oda = new SqlDataAdapter(orcd);
            oda.Fill(Ds_1);
            orcl.Close();

            return Ds_1;
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            //if (GridView.Columns.Count == 0)
            //{
            //    btn_add_Click(sender, e);
            //}
            //Session["grid"] = GridView;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "", "window.open('pettycash_ApprovePrint.aspx?from=" + txt_frmdate.Text + "&to=" + txt_todate.Text + "&status=" + dd_status.SelectedValue.ToString() + "&branch=" + dd_branch.SelectedValue.ToString() + "&ec=" + dd_ec.SelectedValue.ToString() + "');", true);


        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
            return;
        }
    }
}