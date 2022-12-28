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
    public partial class CardConsignment_Datashuffle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            string error = "";
            Cl_Variables clvar = new Cl_Variables();
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("Mergeconsignment", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandTimeout = 300;
                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Data has been Shuffled')", true);

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

        }
    }
}