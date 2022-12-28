using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class SearchBags : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();

        string bagnumber, totalweight, orign, destination, sealno, createon, manifestcount, cncount;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt_head = new DataTable();
            dt_head.Columns.Add("bagnumber", typeof(string));
            dt_head.Columns.Add("totalweight", typeof(string));
            dt_head.Columns.Add("orign", typeof(string));
            dt_head.Columns.Add("destination", typeof(string));
            dt_head.Columns.Add("sealno", typeof(string));
            dt_head.Columns.Add("createon", typeof(string));
            //  dt_head.Columns.Add("manifestcount", typeof(string));
            //  dt_head.Columns.Add("cncount", typeof(string));
            ViewState["dthead"] = dt_head;
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {

            clvar = new Variable();
            clvar._StartDate = dd_start_date.Text;
            if (dd_start_date.Text.Trim() != "")
            {
                clvar.BagNumber = " AND cast(b.createdOn as date) = '" + clvar._StartDate + "'";
            }

            if (ddl_critera.SelectedValue == "1")
            {
                if (txt_bag.Text.Trim() != "")
                {
                    clvar.BagNumber += " and b.bagNumber = '" + txt_bag.Text + "' ";
                }

            }
            else
            {
                if (txt_bag.Text.Trim() != "")
                {
                    clvar.BagNumber += " and b.sealNo = '" + txt_bag.Text + "' ";
                }

            }


            DataSet ds = Get_SearchBags(clvar);

            if (ds.Tables[0].Rows.Count != 0)
            {
                DataTable dt_head = ViewState["dthead"] as DataTable;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    bagnumber = ds.Tables[0].Rows[i]["bagNumber"].ToString();
                    totalweight = ds.Tables[0].Rows[i]["totalWeight"].ToString() + " Kg";
                    orign = ds.Tables[0].Rows[i]["orign"].ToString();
                    destination = ds.Tables[0].Rows[i]["destination"].ToString();
                    sealno = ds.Tables[0].Rows[i]["sealNo"].ToString();
                    createon = ds.Tables[0].Rows[i]["createdOn"].ToString();
                    //  manifestcount   = ds.Tables[0].Rows[i]["manifestCount"].ToString();
                    //  cncount         = ds.Tables[0].Rows[i]["CNCount"].ToString();

                    dt_head.Rows.Add(bagnumber, totalweight, orign, destination, sealno, createon);//, manifestcount, cncount);
                }

                dt_head.AcceptChanges();
                ViewState["dthead"] = dt_head;
                GridView.DataSource = dt_head;
                GridView.DataBind();

                txt_bag.Text = "";
                dd_start_date.Text = "";

                ddl_critera.SelectedValue = "Select";
            }
            else
            {
                GridView.DataSource = null;
                GridView.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Bags Found')", true);
                txt_bag.Text = "";
                dd_start_date.Text = "";
            }

        }
        public DataSet Get_SearchBags(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = " select b.bagNumber, CONVERT(NVARCHAR, b.createdon, 105) createdon, b.totalWeight, b.sealNo, \n" +
                             " b1.name orign, b2.name destination --, COUNT(bm.manifestNumber) manifestCount, COUNT(bo.outpieceNumber) CNCount\n" +
                             " from Bag b\n" +
                             //" inner join BagManifest bm\n" +
                             //" on bm.bagNumber = b.bagNumber\n" +
                             // " inner join BagOutpieceAssociation bo\n" +
                             // " on bo.bagNumber = b.bagNumber\n" +
                             " inner join Branches b1\n" +
                             " on b1.branchCode = b.origin\n" +
                             " inner join Branches b2\n" +
                             " on b2.branchCode = b.destination\n" +
                             " where 1 = 1 \n" +
                             "  \n" +
                             " " + clvar.BagNumber + " and b.origin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
                             " group by\n" +
                             " b.bagNumber, b.createdOn, b.totalWeight, b.sealNo, b.origin, b.destination, b1.name, b2.name";

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
    }
}