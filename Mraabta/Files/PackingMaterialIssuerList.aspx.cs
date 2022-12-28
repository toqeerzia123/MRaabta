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
    public partial class PackingMaterialIssuerList : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustomerList();
                BindGrid();
            }
        }
        private void BindGrid()
        {
            string customer="", Req_date="", App_date="", Status="";

            if (ddcustomer.SelectedValue != "0")
            {
                customer = "AND CC.ACCOUNTNO = '" + ddcustomer.SelectedValue + "'";
            }

            if (req_date.Text != "")
            {
                Req_date = "AND cast(RequestDate as date) = '" + req_date.Text + "'";
            }

            if (app_date.Text != "")
            {
                App_date = "AND cast(ApprovedDate as date) = '" + app_date.Text + "'";
            }

            if (ddlStatus.SelectedValue != "")
            {
                Status = "AND AppStatus = '" + ddlStatus.SelectedValue + "'";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT distinct p.PackingRequestID,prd.ConsignmentNo, \n" +
                                                           "cc.name, \n" +
                                                           "cc.accountNo, \n" +
                                                           "l.locationName LocationID, \n" +
                                                           "RequestDate, \n" +
                                                           "ApprovedDate, \n" +
                                                           "IssuedDate, \n" +
                                                           "AppStatus \n" +
                                                           "FROM PR_PackingRequest p \n" +
                                                           "INNER JOIN PR_PackingRequestDetail prd ON p.PackingRequestID = prd.PackingRequestID \n" +
                                                           "INNER JOIN CreditClients cc ON p.CustomerID = cc.id \n" +
                                                           "INNER JOIN COD_CustomerLocations l ON p.LocationID = l.locationID \n" +
                                                           "WHERE p.ApprovedBy IS NOT NULL \n" +
                                                           customer + " \n" +
                                                           Req_date + " \n" +
                                                           App_date + " \n" +
                                                           Status + " \n" +
                                                           "ORDER BY \n" +
                                                           "1 DESC"))
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
            Label lblIssueDate = (Label)clickedRow.FindControl("lblIssueDate");

            if (lblIssueDate.Text == "")
            {
                Response.Redirect("PackingMaterialIssuer.aspx?ID=" + lblID.Text + "&Mode=E");
            }
            else
            {

            }
        }
        protected void linkRequesID_HyderLinkClick(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            LinkButton lblID = (LinkButton)clickedRow.FindControl("linkRequesID1");

            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string ecnryptedConsignment = lblID.Text;
            string script = String.Format(script_, "Material_AddressLabel.aspx?id=" + ecnryptedConsignment, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

        }
        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        private void CustomerList()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string com1 = @"SELECT  
                                UPPER(CC.NAME) +' ('+Z.NAME+'-'+B.NAME+'-'+CC.ACCOUNTNO +')' AS NAME, CC.ACCOUNTNO
                                FROM CREDITCLIENTS CC 
                                INNER JOIN COD_CUSTOMERLOCATIONS AS CCL ON CCL.CREDITCLIENTID = CC.ID
                                INNER JOIN BRANCHES AS B ON B.BRANCHCODE = CC.BRANCHCODE
                                INNER JOIN ZONES AS Z ON Z.ZONECODE = CC.ZONECODE
                                WHERE CC.ISCOD = '1' AND CC.CODTYPE !='3' AND CC.ISACTIVE = '1'
                                ORDER BY 1 ";

                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                ddcustomer.DataSource = dt1;
                ddcustomer.DataBind();
                ddcustomer.DataTextField = "NAME";
                ddcustomer.DataValueField = "ACCOUNTNO";
                ddcustomer.DataBind();
                ListItem item1 = new ListItem("Select Account No", "0");
                ddcustomer.Items.Insert(0, item1);
                dt1.Dispose();
                dt1.Clear();
            }
            catch (Exception)
            {

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}