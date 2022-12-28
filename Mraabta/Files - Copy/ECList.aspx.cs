using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class ECList : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "SELECT ROW_NUMBER() OVER(ORDER BY ec.expressCEnterCode) \" \", ec.expressCenterCode ExpressCenterCode, ec.name ExpressCenterName, ec.address Address \n"
               + "  FROM ExpressCenters ec WHERE ec.bid = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' AND ec.[status] = '1' \n"
               + "ORDER BY ec.expressCenterCode";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);
                gv_ECList.DataSource = dt;
                gv_ECList.DataBind();
            }
            catch (Exception ex)
            { }
        }
    }
}