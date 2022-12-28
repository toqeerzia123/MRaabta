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
    public partial class PackingMaterialRequestList : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public string getMSG(string msg)
        {
            string javas = "<script type=\"text/javascript\"> swal(' " + msg + "') </script>";
            return javas;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["MSG"] != null)
            {
                string msg = Request["MSG"];
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", getMSG(msg));
            }
            if (!IsPostBack)
            {
                DateTime dt_from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtRequesDate.Attributes["min"] = Convert.ToDateTime(dt_from.AddMonths(-1)).ToString("yyyy-MM-dd");
                txtRequesDate.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
                txtRequesDate.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");
                BindGrid();

            }

        }
        private void loadCombo()
        {
            try
            {


                //SqlConnection con = new SqlConnection(constr);
                //con.Open();
                //string com1 = "select ID, Name from PR_packing_material where id > 1";
                //SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                //DataTable dt1 = new DataTable();
                //adpt1.Fill(dt1);
                //con.Close();
                //ddlNeedPackingMaterialID.DataSource = dt1;
                ////ddlNeedPackingMaterialID.DataBind();
                //ddlNeedPackingMaterialID.DataTextField = "Name";
                //ddlNeedPackingMaterialID.DataValueField = "ID";
                //ddlNeedPackingMaterialID.DataBind();
                //ListItem item1 = new ListItem("Select Packing Material", "-1");
                //ddlNeedPackingMaterialID.Items.Insert(0, item1);
                //dt1.Dispose();
                //dt1.Clear();


            }
            catch (Exception)
            {

            }
        }
        private void BindGrid()
        {
            try

            {
                string strWhere = "";
                if(txtAccountNO.Text != "")
                {
                    strWhere = strWhere + " and accountNo=@accountNo";
            
                }
                if (txtPackingRequestID.Text != "")
                {
                    strWhere = strWhere + " and PackingRequestID=@PackingRequestID";
                  
                }
                if (txtRequesDate.Text != "")
                {
                    strWhere = strWhere + " and cast(RequestDate as date) =@RequestDate";
                   
                }

                if ( ddlStatus.SelectedValue != "")
                {
                    strWhere = strWhere + " and RequestStatus=@RequestStatus";
                  
                }

                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(@"select PackingRequestID,CreditClients.name,CreditClients.accountNo,lo.locationName,pmr.locationID ,
                        RequestDate,AppStatus from PR_PackingRequest as pmr INNER JOIN CreditClients on pmr.CustomerID = CreditClients.id
                         INNER JOIN COD_CustomerLocations as Lo on pmr.LocationID = lo.locationID 
                     where  pmr.branchCode= @branchCode and pmr.rec_Status=1 
                         " + strWhere + " order by 1 desc")) //where RequestStatus = 1
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {

                            cmd.Parameters.AddWithValue("@branchCode", Session["BranchCode"].ToString());
                            if (txtAccountNO.Text != "")
                            {

                                cmd.Parameters.AddWithValue("@accountNo", txtAccountNO.Text);
                            }
                            if (txtPackingRequestID.Text != "")
                            {

                                cmd.Parameters.AddWithValue("@PackingRequestID", txtPackingRequestID.Text);
                            }
                            if (txtRequesDate.Text != "")
                            {

                                cmd.Parameters.AddWithValue("@RequestDate", txtRequesDate.Text);
                            }

                            if ( ddlStatus.SelectedValue != "")
                            {

                                cmd.Parameters.AddWithValue("@RequestStatus", ddlStatus.SelectedValue);
                            }


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
              
            }
        }

        protected void grPML_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grPML_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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

            Response.Redirect("PackingMaterialRequest.aspx?ID="+ lblID.Text+"&Mode=E" );
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("PackingMaterialRequest.aspx", true);
        }
    }
}