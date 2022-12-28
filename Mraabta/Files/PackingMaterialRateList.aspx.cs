using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class PackingMaterialRateList : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindGrid();

            }
        }
        private void BindGrid()
        {
            try
            {

                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("select PackingRequestID,CreditClients.name,CreditClients.accountNo,LocationID,RequestDate,ApprovedDate,IssuedDate,AppStatus from PR_PackingRequest INNER JOIN CreditClients on PR_PackingRequest.CustomerID = CreditClients.id where RequestStatus in (3,4) order by 1 desc"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt1 = new DataTable())
                            {
                                sda.Fill(dt1);
                                grPML.DataSource = dt1;
                                grPML.DataBind();
                            }
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void grPML_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grPML.PageIndex = e.NewPageIndex;
            BindGrid();
        }



        protected void linkRequesID_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            LinkButton lblID = (LinkButton)clickedRow.FindControl("linkRequesID");

            Response.Redirect("PackingMaterialRate.aspx?ID=" + lblID.Text + "&Mode=E");

        }
    }
}