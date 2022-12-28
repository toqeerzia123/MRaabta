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
    public partial class PackingMaterialRate : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString();
        string Mode = "A";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int PID = Convert.ToInt32(Request.QueryString["ID"]);
                if (Request.QueryString["Mode"] != null)
                {
                    Mode = Request.QueryString["Mode"].ToString();
                }

                if (Mode == "E")
                {
                    txtRequestNumber.Text = PID.ToString();
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(@" select ROW_NUMBER() OVER(ORDER BY prd.PackingRequestDetailID) AS SNO,prd.PackingRequestID,prd.ItemID, pm.Name, pms.Size, RequestQuantity, ApprovedQuantity,IssuedQuantity,
                                        PackingRequestDetailID,R.Rate,R.GST,R.Rate*IssuedQuantity as TotalPrice
										,R.GST*(R.Rate*IssuedQuantity) as GSTAmount
										,(R.Rate*IssuedQuantity)+(R.GST*(R.Rate*IssuedQuantity)) as TotalAmount
									,Req.CustomerID,Req.Address,Req.LocationID,Req.RequestLabel	,C.name as Cusname,L.Locationname,Req.InvoiceDate
									from PR_PackingRequestDetail as prd
										inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                        inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                         inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                         inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                                         inner join CreditClients as C on Req.CustomerID = C.id									 
                                         left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                                         where prd.PackingRequestID =" + PID))
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
                                    if (dt1.Rows.Count>0)
                                    {
                                       // txtRequestNumber.Text = dt1.Rows[0]["RequestLabel"].ToString();
                                        txtCustomer.Text = dt1.Rows[0]["Cusname"].ToString();
                                        ddlLocation.Text = dt1.Rows[0]["Locationname"].ToString();
                                        txtAddress.Text = dt1.Rows[0]["Address"].ToString();
                                        if (dt1.Rows[0]["InvoiceDate"].ToString() == "")
                                        {
                                            btnPost.Visible = false;
                                            btnInvoice.Visible = false;
                                        }
                                        else
                                        {
                                            btnPost.Visible = true;
                                            btnInvoice.Visible = true;

                                        }
                                    }
                                    decimal total = dt1.AsEnumerable().Sum(row => row.Field<decimal>("TotalAmount"));
                                    grdPR.FooterRow.Cells[10].Text = "Total";
                                    grdPR.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                                    grdPR.FooterRow.Cells[11].Text = total.ToString("N2");
                                    grdPR.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;

                                }
                            }
                        }

                    }

                }
                else
                {
                    Response.Redirect("PackingMaterialRateList.aspx");
                }
            }

        }

        protected void grdPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdPR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void IssueSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                string query = "UPDATE PR_PackingRequest SET InvoiceBy = @InvoiceBy, InvoiceDate = getdate(), AppStatus = @AppStatus, RequestStatus = @RequestStatus WHERE PackingRequestID =" + txtRequestNumber.Text;
                SqlCommand cmd = new SqlCommand(query);

                cmd.Parameters.AddWithValue("@InvoiceBy", Convert.ToInt32(Session["u_ID"].ToString()));
                cmd.Parameters.AddWithValue("@AppStatus", "Invoice Saved");
                cmd.Parameters.AddWithValue("@RequestStatus", 4);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();

                foreach (GridViewRow drReview in grdPR.Rows)
                {
                    //TextBox ISSQuantity = (TextBox)drReview.FindControl("txtIssuedQuantity");
                    Label DetailID = (Label)drReview.FindControl("lblDetailID");
                    Label gRate = (Label)drReview.FindControl("lblRate");
                    Label gGST = (Label)drReview.FindControl("lblGST");
                    Label gTotalPrice = (Label)drReview.FindControl("lblTotalPrice");
                    Label gGSTAmount = (Label)drReview.FindControl("lblGSTAmount");
                    Label gTotalAmount = (Label)drReview.FindControl("lblTotalAmount");
                    SqlCommand cmd2 = new SqlCommand("UPDATE PR_PackingRequestDetail SET Rate = @Rate,GST = @GST,TotalPrice = @TotalPrice,GSTAmount = @GSTAmount,TotalAmount = @TotalAmount WHERE PackingRequestDetailID =" + DetailID.Text);
                    cmd2.Parameters.AddWithValue("@Rate", Convert.ToDecimal(gRate.Text));
                    cmd2.Parameters.AddWithValue("@GST", Convert.ToDecimal(gGST.Text));
                    cmd2.Parameters.AddWithValue("@TotalPrice", Convert.ToDecimal(gTotalPrice.Text));
                    cmd2.Parameters.AddWithValue("@GSTAmount", Convert.ToDecimal(gGSTAmount.Text));
                    cmd2.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(gTotalAmount.Text));

                    cmd2.Connection = con;
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Invoice saved')", true);
                Response.Redirect("PackingMaterialRateList.aspx");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected void btnPost_Click(object sender, EventArgs e)
        {

            string RequestID = txtRequestNumber.Text;
            long lrt = 0;

            try
            {




                string com = @"



insert into PM_Invoice (totalAmount )
			select-- ROW_NUMBER() OVER(ORDER BY prd.PackingRequestID) AS SNO
--Req.AppStatus,
--prd.PackingRequestID,sum(R.Rate*IssuedQuantity) as TotalPrice
									--	,sum(R.GST*(R.Rate*IssuedQuantity)) as GSTAmount
										sum((R.Rate*IssuedQuantity)+(R.GST*(R.Rate*IssuedQuantity))) as TotalAmount
								--	,Req.CustomerID,Req.Address,Req.LocationID,Req.RequestLabel	,C.name as Cusname,L.Locationname,Req.InvoiceDate
									from PR_PackingRequestDetail as prd
										inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                        inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                         inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                         inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                                         inner join CreditClients as C on Req.CustomerID = C.id									 
                                         left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                                         where   
                   prd.PackingRequestID  in (" + RequestID + @") 

										 group by prd.PackingRequestID,Req.AppStatus,Req.CustomerID,Req.Address,Req.LocationID,Req.RequestLabel	
										 ,L.Locationname,Req.InvoiceDate,C.name


UPDATE PR_PackingRequest SET AppStatus = 'Posted',RequestStatus='5' WHERE PackingRequestID in ( " + RequestID + @") 

";
                using (SqlConnection con = new SqlConnection(constr))
                {

                    using (SqlCommand cmd = new SqlCommand(com))
                    {




                        cmd.Connection = con;
                        con.Open();
                        lrt = cmd.ExecuteNonQuery();
                        con.Close();
                    }



                }
                if (lrt > 0)
                {
                   


                    
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Invoice has been Posted')", true);


                }


            }
            catch (Exception ex)
            {

            }
        }
        protected void btnPost1_Click(object sender, EventArgs e)
        {
            PostInvoce(txtRequestNumber.Text);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Invoice has been Post')", true);
            btnPost.Visible = false;
            IssueSubmit.Visible = false;

        }
        public string PostInvoce(string RequsetNumber)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "PM_Bulk_InvoicePost";
        

                cmd1.Parameters.AddWithValue("@RequsetNumber", RequsetNumber);
              
               
           
                cmd1.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
             
                cmd1.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd1.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd1.ExecuteNonQuery();
                string retunvalue = cmd1.Parameters["@result"].Value.ToString();
                //string error = cmd1.Parameters["@result"].ToString();

                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return ex.Message;
            }


            return "OK";
        }
        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            Response.Redirect("PackingMaterialInvoice.aspx?ID=" + txtRequestNumber.Text + "&Mode=V");
        }
    }
}