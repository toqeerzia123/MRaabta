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
    public partial class Search_Unloading : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        LoadingPrintReport b_fun = new LoadingPrintReport();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void submit_Click(object sender, EventArgs e)
        {
            if (txt_date.Text != "")
            {
                clvar._StartDate = txt_date.Text;
                DataSet ds = Get_Search_UnLoading(clvar);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    GridView.DataSource = ds.Tables[0].DefaultView;
                    GridView.DataBind();
                }
                else
                {
                    GridView.DataSource = null;
                    GridView.DataBind();
                }
            }
        }


        public DataSet Get_Search_UnLoading(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {

                //string sqlString = "select u.ID , u.RefLoadingID,b1.name origin,b2.name destination,CONVERT(VARCHAR(10), u.Createdon, 105) Date\n" +
                //"from\n" +
                //" mnp_unloading u,\n" +
                //" Branches b1,\n" +
                //" Branches b2\n" +
                //" where\n" +
                //" u.origin=b1.branchCode\n" +
                //" and u.destination=b2.branchCode\n" +
                //" and CONVERT(VARCHAR(10), u.Createdon, 105)='" + clvar._StartDate + "'";

                string sqlString = @"select u.ID, RefLoadingID = isnull(STUFF((
                                    SELECT ',' + cast(md.loadingID as nvarchar)
                                    FROM MnP_UnloadingRef md
                                    WHERE u.id = md.unloadingID
                                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''),u.RefLoadingID) , 
                                    b1.name origin, b2.name destination, CONVERT(VARCHAR(10), u.Createdon, 105) Date
                                from
                                mnp_unloading u,
                                Branches b1,
                                Branches b2
                                where
                                u.origin = b1.branchCode
                                and u.destination = b2.branchCode and CONVERT(VARCHAR(10), u.Createdon, 105)='" + clvar._StartDate + "'";


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