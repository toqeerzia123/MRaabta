using MRaabta.App_Code;
using System;
using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace MRaabta.Files
{
    public partial class BTSDashoard : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        BTS_DB pd;
        SqlConnection orcl;
        public BTSDashoard()
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