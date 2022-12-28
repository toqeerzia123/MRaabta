using System;
using System.Configuration;
using System.Data.SqlClient;
using MRaabta.App_Code;
using MRaabta.Repo;

namespace MRaabta.Files
{
    public partial class PickupDashboard : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        BTS_DB pd;
        SqlConnection orcl;
        public PickupDashboard()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            pd = new BTS_DB();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }


    }
}