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
    public partial class SearchClientGroups : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["PROFILE"].ToString() != "16")
            {
                rbtn_mode.Enabled = false;
            }
            if (!IsPostBack)
            {

                GetBranches();
            }
        }

        public void GetBranches()
        {
            string sql = "SELECT b.branchCode, sname + ' - ' + b.name BName \n"
               + "          FROM   Branches                          b \n"
               + "          where b.[status] ='1'  \n"
               + "          GROUP BY \n"
               + "                 b.branchCode, \n"
               + "                 b.name,sname order by b.name ASC";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);
                dd_grpBranch.DataSource = dt;
                dd_grpBranch.DataTextField = "Bname";
                dd_grpBranch.DataValueField = "BranchCode";
                dd_grpBranch.DataBind();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            clvar.customerName = txt_grpName.Text;
            clvar.Branch = dd_grpBranch.SelectedValue;
            txt_grpID.Text = "";

            DataTable dt = GetData(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_existingGroups.DataSource = dt;
                    gv_existingGroups.DataBind();
                }
                else
                {
                    gv_existingGroups.DataSource = null;
                    gv_existingGroups.DataBind();
                }
            }
            else
            {
                gv_existingGroups.DataSource = null;
                gv_existingGroups.DataBind();
            }
        }
        protected DataTable GetData(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sql = "select cg.*, b.name BranchName, CASE WHEN ISNULL(cg.status , 0) = '1' then 'ACTIVE' else 'INACTIVE' END GRPSTATUS \n" +
                         "  from clientGroups cg \n" +
                         " inner join branches b \n" +
                         "    on b.branchCode = cg.collectionCenter \n" +
                         " where /*cg.collectionCenter = '" + clvar.Branch + "' and */cg.name like '%" + clvar.customerName + "%'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        protected void btn_Create_Click(object sender, EventArgs e)
        {
            clvar.customerName = txt_grpName.Text;
            clvar.Branch = dd_grpBranch.SelectedValue;

            Tuple<bool, string> resp = CreateNewGroup(clvar);

            if (resp.Item1)
            {
                txt_grpID.Text = resp.Item2.ToString();
                rbtn_mode.SelectedValue = "0";
                Alert("Group Created", "Green");
                btn_search_Click(this, e);
            }
            else
            {
                Alert(resp.Item2.ToString(), "Red");
            }
        }

        protected void Alert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
        protected Tuple<bool, string> CreateNewGroup(Cl_Variables clvar)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");
            string id = "";
            string sql = "insert into ClientGroups (Name, status, CreatedBy, CreatedON, CollectionCenter) OUTPUT INSERTED.ID VALUES('" + clvar.customerName + "', '1', '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), '" + clvar.Branch + "')";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                Object obj = cmd.ExecuteScalar();
                id = obj.ToString();
                resp = new Tuple<bool, string>(true, id);
            }
            catch (Exception ex)
            { resp = new Tuple<bool, string>(false, ex.Message); }
            return resp;
        }
    }
}