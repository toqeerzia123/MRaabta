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
    public partial class SchedulerList : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString();
        
        protected void Page_Load(object sender, EventArgs e)
        {
           if( Request["MSG"]!=null)
            { 
            string msg = Request["MSG"];
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('"+ msg + "')", true);
            }
            if (!IsPostBack)
            {
                //DateTime dt_frommin = new DateTime(2000, 12, 1);

                //DateTime dt_frommax = new DateTime(2050, 12, 1);
                //txtPickupDate.Attributes["min"] = dt_frommin.ToString("yyyy-MM-dd");
                //txtPickupDate.Attributes["max"] = dt_frommax.ToString("yyyy-MM-dd");
                //txtPickupDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");

                BindGrid();

            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try

            {
                int id = Convert.ToInt16(grPML.DataKeys[e.RowIndex].Values["SchedulerID"].ToString());
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("update PR_Scheduler_PickupRequest set rec_status=2 where SchedulerID =@SchedulerID", con);
            cmd.Parameters.AddWithValue("@SchedulerID", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindGrid();
            }
            catch (Exception ex)
            {

            }

        }
        private void BindGrid()
        {
            try

            {
                string strWhere = "";
                //if (txtAccountNO.Text != "")
                //{
                //    strWhere = strWhere + " and accountNo='" + txtAccountNO.Text + "'";
                //}
                //if (txtPackingRequestID.Text != "")
                //{
                //    strWhere = strWhere + " and PackingRequestID='" + txtPackingRequestID.Text + "'";
                //}
                //if (txtRequesDate.Text != "")
                //{
                //    strWhere = strWhere + " and cast(RequestDate as date) ='" + txtRequesDate.Text + "'";
                //}

                //if (ddlStatus.SelectedValue != "-1" && ddlStatus.SelectedValue != "")
                //{
                //    strWhere = strWhere + " and RequestStatus=" + ddlStatus.SelectedValue;
                //}
                if (txtAccountNo.Text != "")
                {
                    strWhere = strWhere + " and c.accountno ='" + txtAccountNo.Text + "'";
                }
                using (SqlConnection con = new SqlConnection(constr))
                {
                   // using (SqlCommand cmd = new SqlCommand("select st.Name as ScheduleType, l.locationName as CustomerLocation,c.name as CustomerName,b.name as branchName,s.PickupDate,s.ProductID,s.Qauntity,s.Slot,s.RiderCode,s.RouteCode,s.SchedulerTypeID,s.SchedulerID, s.ServiceID, s.Status, s.VehicleRequired, s.CustomerID,c.accountNo, c.contactPerson, c.address, c.email  from PR_Scheduler as s INNER JOIN CreditClients as c on s.CustomerID = c.id INNER JOIN COD_CustomerLocations as L on s.LocationID = l.locationID Inner Join Branches as b on s.BranchCode = b.branchCode Inner Join PR_ScheduleType as st on st.ID = s.SchedulerTypeID order by  s.SchedulerID desc")) //where RequestStatus = 1
                    using (SqlCommand cmd = new SqlCommand(@"select   s.SchedulerID ,st.Name as ScheduleType, l.locationName as CustomerLocation,c.name as CustomerName,b.name as branchName,
cast(s.PickupDate AS date) AS PickupDate,s.Product as ProductID ,s.Qauntity,s.Slot,s.RiderCode,s.RouteCode,s.SchedulerTypeID
,s.SchedulerID, s.Service as ServiceID, s.Status, s.VehicleRequired, s.CustomerID,c.accountNo, 
c.contactPerson, c.address, c.email  from PR_Scheduler_PickupRequest as s 
INNER JOIN CreditClients as c on s.CustomerID = c.id
inner JOIN COD_CustomerLocations as L on s.LocationID = l.locationID 
inner  Join Branches as b on s.BranchCode = b.branchCode 
left Join PR_ScheduleType as st on st.ID = s.SchedulerTypeID WHERE Rec_status=1 " + strWhere + " and  s.BranchCode='" + Session["BranchCode"].ToString() + @"'  
order by  s.SchedulerID desc")) //where RequestStatus = 1
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
              
            }
        }
        protected void grPML_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grPML.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void linkRequesID_Click(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32( (sender as LinkButton).CommandArgument);
            Response.Redirect("Scheduler.aspx?ID=" + Id + "&Mode=E");

        //    GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
         //   LinkButton lblID = (LinkButton)clickedRow.FindControl("linkRequesID");

         //   Response.Redirect("Scheduler.aspx?ID=" + lblID.Text + "&Mode=E");

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            Response.Redirect("Scheduler.aspx", true);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}