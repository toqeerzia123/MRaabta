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
    public partial class SearchArrivalScanNew : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();

        string zone, sts, chksts, id, ConsignmentTypeName, Expresscentercode;
        string branchcode, expresscenter, ridercode, weight, createon, Count;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt_head = new DataTable();
            dt_head.Columns.Add("id", typeof(string));
            dt_head.Columns.Add("branchcode", typeof(string));
            dt_head.Columns.Add("expresscenter", typeof(string));
            dt_head.Columns.Add("ridercode", typeof(string));
            dt_head.Columns.Add("weight", typeof(string));
            dt_head.Columns.Add("createon", typeof(string));
            dt_head.Columns.Add("Count", typeof(string));
            ViewState["dthead"] = dt_head;
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            if (txt_ridercode.Text != "" && dd_start_date.Text != "")
            {
                clvar = new Variable();

                clvar.RiderCode = txt_ridercode.Text;
                clvar._StartDate = dd_start_date.Text;

                DataSet ds = Get_SearchArrivalScan(clvar);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt_head = ViewState["dthead"] as DataTable;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        id = ds.Tables[0].Rows[i]["id"].ToString();
                        branchcode = ds.Tables[0].Rows[i]["branchcode"].ToString();
                        expresscenter = ds.Tables[0].Rows[i]["originexpresscentercode"].ToString();
                        ridercode = ds.Tables[0].Rows[i]["ridercode"].ToString();
                        weight = ds.Tables[0].Rows[i]["weight"].ToString();
                        createon = ds.Tables[0].Rows[i]["createdon"].ToString();
                        Count = ds.Tables[0].Rows[i]["Count"].ToString();

                        dt_head.Rows.Add(id, branchcode, expresscenter, ridercode, weight, createon, Count);
                    }

                    dt_head.AcceptChanges();
                    ViewState["dthead"] = dt_head;
                    GridView.DataSource = dt_head;
                    GridView.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                    "alert('No Data Found...');", true);
                    txt_ridercode.Text = "";
                    dd_start_date.Text = "";

                    GridView.DataSource = null;
                    GridView.DataBind();
                }
            }
        }


        public DataSet Get_SearchArrivalScan(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql_old = " select CONVERT(NVARCHAR, a.createdon, 105) createdon, a.* \n" +
                             " from arrivalscan a where a.ridercode = '" + clvar.RiderCode + "' AND a.createdon = '" + clvar._StartDate + "' \n" +
                             " order by Id desc ";

                string sql = " select \n" +
                             " a.Id, b.name +' ('+b.branchcode+')' branchcode, e.name+' ('+e.expressCenterCode+')' originexpresscentercode, a.RiderCode, a.Weight, CONVERT(NVARCHAR, a.createdon, 105) createdon, COUNT(ad.consignmentNumber) Count\n" +
                             " from arrivalscan a  \n" +
                             " inner join ArrivalScan_Detail ad\n" +
                             " on ad.ArrivalID = a.Id \n" +
                             " inner join Branches b\n" +
                             " on b.branchCode = a.BranchCode \n" +
                             " inner join ExpressCenters e\n" +
                             " on e.expressCenterCode = a.OriginExpressCenterCode\n" +
                             " where a.ridercode = '" + clvar.RiderCode + "' AND CAST(a.CreatedOn AS date) = '" + clvar._StartDate + "' \n" +
                             " group by \n" +
                             " a.Id, b.name +' ('+b.branchcode+')', e.name+' ('+e.expressCenterCode+')', a.RiderCode, a.Weight, a.CreatedOn\n" +
                             " order by Id desc ";


                string sqlString = "select a.Id,\n" +
                "       b.name + ' (' + b.branchcode + ')' branchcode,\n" +
                "       e.name + ' (' + e.expressCenterCode + ')' originexpresscentercode,\n" +
                "       a.RiderCode,\n" +
                "       a.Weight,\n" +
                "       CONVERT(NVARCHAR, a.createdon, 105) createdon,\n" +
                "       COUNT(ad.consignmentNumber) Count\n" +
                "  from arrivalscan a\n" +
                " inner join ArrivalScan_Detail ad\n" +
                "    on ad.ArrivalID = a.Id\n" +
                " inner join Branches b\n" +
                "    on b.branchCode = a.BranchCode\n" +
                " LEFT OUTER join ExpressCenters e\n" +
                "    on e.expressCenterCode = a.OriginExpressCenterCode\n" +
                " where a.ridercode = '" + clvar.RiderCode + "'\n" +
                "   and a.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
                "   AND CAST(a.CreatedOn AS date) = '" + clvar._StartDate + "'\n" +
                " group by a.Id,\n" +
                "          b.name + ' (' + b.branchcode + ')',\n" +
                "          e.name + ' (' + e.expressCenterCode + ')',\n" +
                "          a.RiderCode,\n" +
                "          a.Weight,\n" +
                "          a.CreatedOn\n" +
                " order by Id desc";


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
    }
}