using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRaabta.App_Code;
using System.Data;

namespace MRaabta.Files
{
    public partial class SearchRunSheet : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();

        string zone, sts, chksts, id, ConsignmentTypeName, Expresscentercode, CNcount;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt_head = new DataTable();
            dt_head.Columns.Add("runsheetNumber", typeof(string));
            dt_head.Columns.Add("runsheetDate", typeof(string));
            dt_head.Columns.Add("routeCode", typeof(string));
            dt_head.Columns.Add("CNcount", typeof(string));
            dt_head.Columns.Add("createdby", typeof(string));

            ViewState["dthead"] = dt_head;
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            Errorid.Text = "0";
            if (txt_route.Text != "" && dd_start_date.Text != "")
            {
                clvar = new Variable();

                clvar._StartDate = dd_start_date.Text;
                clvar.Route = txt_route.Text;
                Errorid.Text = "1";
                DataSet ds = b_fun.Get_SearchRunSheet(clvar);
                Errorid.Text = "2";
                string runsheetNumber, runsheetDate, routeCode, createdby;

                if (ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt_head = ViewState["dthead"] as DataTable;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        runsheetNumber = ds.Tables[0].Rows[i]["runsheetNumber"].ToString();
                        runsheetDate = ds.Tables[0].Rows[i]["runsheetDate"].ToString();
                        routeCode = ds.Tables[0].Rows[i]["routeCode"].ToString();
                        CNcount = ds.Tables[0].Rows[i]["CNcount"].ToString();
                        createdby = ds.Tables[0].Rows[i]["createdby"].ToString();



                        dt_head.Rows.Add(runsheetNumber, runsheetDate, routeCode, CNcount, createdby);
                    }

                    dt_head.AcceptChanges();
                    ViewState["dthead"] = dt_head;
                    GridView.DataSource = dt_head;
                    GridView.DataBind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                        "alert('Please Fill Full Form');", true);
                txt_route.Text = "";
                dd_start_date.Text = "";
            }
        }
    }
}