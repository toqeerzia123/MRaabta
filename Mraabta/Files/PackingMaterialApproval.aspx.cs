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
    public partial class PackingMaterialApproval : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string Mode = "A";
        int PID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PID = Convert.ToInt32(Request.QueryString["ID"]);
                if (Request.QueryString["Mode"] != null)
                {
                    Mode = Request.QueryString["Mode"].ToString();
                }

                if (Mode == "E")
                {
                    txtRequestNumber.Text = PID.ToString();
                    SqlConnection con = new SqlConnection(constr);
                    con.Open();
                    string com1 = @"select cc.contactPerson,l.locationAddress address, l.locationName, pr.PackingRequestID from PR_PackingRequest as pr
                                    inner join CreditClients as cc on pr.CustomerID = cc.id
                                    inner join COD_CustomerLocations as l on pr.LocationID = l.locationID AND l.CreditClientID = cc.id
                                    where pr.PackingRequestID=" + PID;
                    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                    DataTable dt2 = new DataTable();
                    adpt1.Fill(dt2);
                    con.Close();
                    //select locationName from COD_CustomerLocations where brancahCode = 4 and accountNo = '4T67'  ddlLocation1
                    if (dt2.Rows.Count > 0)
                    {
                        txtCustomer.Text = dt2.Rows[0]["contactPerson"].ToString();
                        txtAddress.Text = dt2.Rows[0]["address"].ToString();
                        ddlLocation.Text = dt2.Rows[0]["locationName"].ToString();
                    }


                    //using (SqlConnection con = new SqlConnection(constr)) 
                    //{
                    using (SqlCommand cmd = new SqlCommand(@"SELECT ROW_NUMBER() OVER (ORDER BY PACKINGREQUESTDETAILID) AS SNO, 
                                                            PM.NAME,PMS.SIZE,REQUESTQUANTITY, 
                                                            PACKINGREQUESTDETAILID, R.RATE, R.GST, PRC.COST AS COMPANYCOST
                                                            FROM PR_PACKINGREQUESTDETAIL AS PRD
	                                                        INNER JOIN PR_PACKINGREQUEST AS PR ON PRD.PACKINGREQUESTID = PR.PACKINGREQUESTID
                                                            INNER JOIN PR_PACKING_MATERIAL AS PM ON PRD.ITEMID = PM.ID
                                                            INNER JOIN PR_PACKINGMATERIALSIZE AS PMS ON PRD.ITEMSIZE = PMS.ID
                                                            LEFT JOIN PR_PACKINGMATERIALRATE R ON PR.CUSTOMERID = R.CLIENTID
                                                            INNER JOIN PR_COMPANYCOST PRC ON PRC.SIZEID=PMS.ID AND PRC.MATERIALID=PM.ID
                                                            WHERE PRD.PACKINGREQUESTID = " + PID))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt1 = new DataTable())
                            {
                                sda.Fill(dt1);
                                grdPR.DataSource = dt1;
                                grdPR.DataBind();
                            }
                        }
                    }
                    //}
                }
                //loadCombo();
                BindGrid();
            }

        }
        private void loadCombo()
        {
            try
            {

                //SqlConnection con = new SqlConnection(constr);
                //DataTable dt1 = new DataTable();
                //con.Open();
                //string com1 = "select * from PR_packing_material";
                //SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                //
                //adpt1.Fill(dt1);
                //con.Close();
                //ddlNeedPackingMaterialID.DataSource = dt1;
                //ddlNeedPackingMaterialID.DataBind();
                //ddlNeedPackingMaterialID.DataTextField = "Name";
                //ddlNeedPackingMaterialID.DataValueField = "Name";
                //ddlNeedPackingMaterialID.DataBind();
                //dt1.Dispose();
                //dt1.Clear();


                //con.Open();
                //string com2 = "select PackingRequestID from PR_PackingRequest";
                //SqlDataAdapter adpt2 = new SqlDataAdapter(com2, con);
                //DataTable dt2 = new DataTable();
                //adpt2.Fill(dt2);
                //con.Close();
                //ddlRequestNo.DataSource = dt2;
                //ddlRequestNo.DataBind();
                //ddlRequestNo.DataTextField = "PackingRequestID";
                //ddlRequestNo.DataValueField = "PackingRequestID";
                //ddlRequestNo.DataBind();
                //dt2.Dispose();
                //dt2.Clear();




            }
            catch (Exception)
            {

            }
        }
        private void BindGrid()
        {
            //try

            //{


            //    using (SqlConnection con = new SqlConnection(constr))
            //    {
            //        using (SqlCommand cmd = new SqlCommand("select ItemID,RequestQuantity from PR_PackingRequestDetail where PackingRequestID =" + ddlRequestNo.SelectedValue + " "))
            //        {
            //            using (SqlDataAdapter sda = new SqlDataAdapter())
            //            {
            //                cmd.Connection = con;
            //                sda.SelectCommand = cmd;
            //                using (DataTable dt = new DataTable())
            //                {
            //                    sda.Fill(dt);
            //                    grdPR.DataSource = dt;
            //                    grdPR.DataBind();
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    //throw;
            //}
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow drReview in grdPR.Rows)
                {
                    Label DetailID = (Label)drReview.FindControl("lblRequestedQuantity");
                    TextBox AppQuantity = (TextBox)drReview.FindControl("txtApprovedQuantity");

                    if(double.Parse(DetailID.Text) >= double.Parse(AppQuantity.Text))
                    {

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Approved Quantity cant be greater then Requested Quantity')", true);
                        return;
                    }
                }

                SqlConnection con = new SqlConnection(constr);
                string query = "UPDATE PR_PackingRequest SET ApprovedBy = @ApprovedBy, ApprovedDate = getdate(), AppStatus = @AppStatus,RequestStatus =@RequestStatus WHERE PackingRequestID =" + txtRequestNumber.Text;
                SqlCommand cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@ApprovedBy", Convert.ToInt32(Session["u_ID"].ToString()));
                cmd.Parameters.AddWithValue("@AppStatus", "Waiting For Issued");
                cmd.Parameters.AddWithValue("@RequestStatus", 2);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();

                foreach (GridViewRow drReview in grdPR.Rows)
                {
                    TextBox AppQuantity = (TextBox)drReview.FindControl("txtApprovedQuantity");
                    Label DetailID = (Label)drReview.FindControl("lblDetailID");

                    SqlCommand cmd2 = new SqlCommand("UPDATE PR_PackingRequestDetail SET ApprovedQuantity = @ApprovedQuantity WHERE PackingRequestDetailID =" + DetailID.Text);
                    cmd2.Parameters.AddWithValue("@ApprovedQuantity", Convert.ToInt32(AppQuantity.Text));

                    cmd2.Connection = con;
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Approved Packing Request ID: " + txtRequestNumber.Text + "')", true);
                Response.Redirect("PackingMaterialApprovalList.aspx");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected void grdPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grdPR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void ddlRequestNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try

            //{


            //    using (SqlConnection con = new SqlConnection(constr))
            //    {
            //        using (SqlCommand cmd = new SqlCommand("select ItemID,RequestQuantity from PR_PackingRequestDetail where PackingRequestID =" + ddlRequestNo.SelectedValue + " "))
            //        {
            //            using (SqlDataAdapter sda = new SqlDataAdapter())
            //            {
            //                cmd.Connection = con;
            //                sda.SelectCommand = cmd;
            //                using (DataTable dt = new DataTable())
            //                {
            //                    sda.Fill(dt);
            //                    grdPR.DataSource = dt;
            //                    grdPR.DataBind();
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    //throw;
            //}
        }
    }
}