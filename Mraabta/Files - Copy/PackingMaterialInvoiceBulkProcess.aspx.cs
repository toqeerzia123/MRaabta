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
    public partial class PackingMaterialInvoiceBulkProcess : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString();
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
                string strWhere = "";
                if (ddlStatus.SelectedValue != "-1" && ddlStatus.SelectedValue != "")
                {
                    strWhere = strWhere + " and AppStatus='" + ddlStatus.SelectedItem.Text + "' ";


                }
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(@"select sum((R.Rate*IssuedQuantity)+R.GST*(R.Rate*IssuedQuantity)) as TotalAmount, req.PackingRequestID,c.name,
                                                            c.accountNo,l.locationName LocationID,RequestDate,ApprovedDate,IssuedDate,AppStatus, L.locationID
                                                            from PR_PackingRequestDetail as prd
                                                            inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                                            inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                                            inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                                            inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                                                            inner join CreditClients as C on Req.CustomerID = C.id									 
                                                            left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                                                            where RequestStatus in (3,4) " + strWhere + @" GROUP BY
                                                            req.PackingRequestID,c.name,
                                                            c.accountNo,l.locationName,RequestDate,ApprovedDate,IssuedDate,AppStatus, L.locationID
                                                            order by 1 desc"))

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
                                foreach (GridViewRow row in grPML.Rows)
                                {
                                    CheckBox chk_chk = (CheckBox)row.Cells[0].FindControl("chk_chk");

                                    if (ddlStatus.SelectedValue == "4")
                                    {
                                        chk_chk.Visible = false;
                                        //LinkButton lnkbutton = (LinkButton)row.Cells[10].FindControl("linkRequesID1");
                                        //lnkbutton.Visible = true;
                                    }
                                    else
                                    {
                                        ddlStatus.Visible = true;
                                        //LinkButton lnkbutton = (LinkButton)row.Cells[10].FindControl("linkRequesID1");
                                        //lnkbutton.Visible = false;
                                    }
                                }
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void lb_selectAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grPML.Rows)
            {
                //insert into Consignment

                CheckBox chk = (CheckBox)row.Cells[0].FindControl("chk_chk");
                chk.Checked = true;
            }
        }
        protected void lb_clearAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grPML.Rows)
            {
                //insert into Consignment

                CheckBox chk = (CheckBox)row.Cells[0].FindControl("chk_chk");
                chk.Checked = false;
            }
        }
        protected void linkRequesID_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            LinkButton lblID = (LinkButton)clickedRow.FindControl("linkRequesID");
            Label lblAccountNO = (Label)clickedRow.FindControl("lblAccountNO");

            Response.Redirect("PackingMaterialInvoice.aspx?ID=" + lblID.Text + "&LID=" + lblAccountNO.Text + "&Mode=V");

        }
        protected void btnRunScheduler_Click(object sender, EventArgs e)
        {
            string RequestID = "";
            if (grPML.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select atleast One Item')", true);
                return;
            }
            foreach (GridViewRow row in grPML.Rows)
            {
                // TextBox lblConsignmentNumber = (TextBox)row.Cells[0].FindControl("ConsignmentNumber");
                CheckBox chk_chk = (CheckBox)row.Cells[0].FindControl("chk_chk");
                {
                    if (chk_chk.Checked == true)
                    {
                        LinkButton lnkbutton = (LinkButton)row.Cells[0].FindControl("linkRequesID");
                        RequestID = lnkbutton.Text;
                    }
                }


                // RequestID = RequestID.Replace("''", "','");
                //if (RequestID == "")
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select atleast One Item')", true);
                //    return;
                //}

                long lrt = 0;
                string InvoiceNo = "", clientID = "";

                try
                {
                    if (RequestID != "")
                    {
                        /*
                        string com = @"UPDATE PR_PackingRequest SET InvoiceBy = @InvoiceBy, InvoiceDate = getdate(), 
                               AppStatus = @AppStatus, RequestStatus = @RequestStatus 
                               WHERE PackingRequestID in ( " + RequestID + ") ";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(com))
                            {
                                cmd.Parameters.AddWithValue("@InvoiceBy", Convert.ToInt32(Session["u_ID"].ToString()));
                                cmd.Parameters.AddWithValue("@AppStatus", "Invoice Saved");
                                cmd.Parameters.AddWithValue("@RequestStatus", 4);
                                cmd.Connection = con;
                                con.Open();
                                lrt = cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        */
                        // if (lrt > 0)
                        // {
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            #region MyRegion
                            //                  using (SqlCommand cmd = new SqlCommand(@" 
                            //                       update  PR_PackingRequestDetail set rate =p.Rate ,GST=p.GST, TotalPrice =p.TotalPrice
                            //                       ,GSTAmount=p.GSTAmount,TotalAmount=p.TotalAmount
                            //                       from  PR_PackingRequestDetail K
                            //                       inner join (
                            //                      select prd.PackingRequestID,prd.ItemID, 
                            //                      PackingRequestDetailID,R.Rate,R.GST,R.Rate*IssuedQuantity as TotalPrice
                            //	,R.GST*(R.Rate*IssuedQuantity) as GSTAmount
                            //	,(R.Rate*IssuedQuantity)+(R.GST*(R.Rate*IssuedQuantity)) as TotalAmount

                            //from PR_PackingRequestDetail as prd
                            //	inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                            //                      inner join PR_packing_material as pm on prd.ItemID = pm.ID
                            //                          inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                            //                          inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                            //                          inner join CreditClients as C on Req.CustomerID = C.id									 
                            //                          left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                            //                          where prd.PackingRequestID  in  ( " + RequestID + @") 
                            //                          ) P  on k.PackingRequestID=p.PackingRequestID and k.PackingRequestDetailID=p.PackingRequestDetailID
                            //                      "))

                            #endregion

                            using (SqlCommand cmd = new SqlCommand("select MAX(invoiceNumber) invoiceNumber from invoice where createdon = CAST(GETDATE() AS DATE) and invoiceNumber like 'PM%'"))
                            {
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.Connection = con;
                                    sda.SelectCommand = cmd;
                                    using (DataTable dt1 = new DataTable())
                                    {
                                        sda.Fill(dt1);

                                        if (dt1.Rows[0]["invoiceNumber"].ToString() == "NULL" || dt1.Rows[0]["invoiceNumber"].ToString() == "0")
                                        {
                                            InvoiceNo = "PM" + DateTime.Now.ToString("yy") + "" + DateTime.Now.ToString("MM") + "01";
                                        }
                                        else
                                        {
                                            string sentence = dt1.Rows[0]["invoiceNumber"].ToString();
                                            string[] words = sentence.Split('M');
                                            double inv = double.Parse(words[1]);

                                            InvoiceNo = "PM" + (inv + 1).ToString();
                                        }
                                    }
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Invoice (invoiceNumber, companyId, clientID, chkIsAuto, startDate, endDate, invoiceDate, createdBy, createdOn, 
                            modifiedBy, modifiedOn, totalAmount, overdueDate, deliveryStatus, BillNo, DiscountOnDomestic, DiscountOnDocument, DiscountOnSample, MonthlyFixCharges, 
                            IsInvoiceCanceled, PrintFlag, discount_slab, discount, discount_gst, TotalWeight) VALUES (
                            '" + InvoiceNo + @"','1','" + clientID + @"','0',cast(DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0) AS date) , cast(DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE()), -1) AS date),
                            getdate(), '" + Session["u_ID"].ToString() + @"' ,cast(getdate() as date),null,null,'" + row.Cells[5].Text + @"', getdate(),'PENDING',0,0,0,0,0,0,1,0,0,0,0
                            )"))
                            {
                                cmd.Connection = con;
                                con.Open();
                                long lrt1 = cmd.ExecuteNonQuery();
                                con.Close();
                            }

                            string com = @"UPDATE PR_PackingRequest SET InvoiceBy = @InvoiceBy, InvoiceDate = getdate(), 
                               AppStatus = @AppStatus, RequestStatus = @RequestStatus, InvoiceNumber = @InvoiceNumber 
                               WHERE PackingRequestID in ( " + RequestID + ") ";

                            using (SqlCommand cmd = new SqlCommand(com))
                            {
                                cmd.Parameters.AddWithValue("@InvoiceBy", Convert.ToInt32(Session["u_ID"].ToString()));
                                cmd.Parameters.AddWithValue("@AppStatus", "Invoice Saved");
                                cmd.Parameters.AddWithValue("@RequestStatus", 4);
                                cmd.Parameters.AddWithValue("@InvoiceNumber", InvoiceNo);
                                cmd.Connection = con;
                                con.Open();
                                lrt = cmd.ExecuteNonQuery();
                                con.Close();
                            }

                            if(lrt > 0)
                            {
                                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                                string ecnryptedConsignment = RequestID;
                                string script = String.Format(script_, "PackingMaterialInvoice.aspx?id=" + ecnryptedConsignment+ "&Mode=V", "_blank", "");
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                            }


                        }
                        BindGrid();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Invoice has been Generated')", true);
                        //}
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        protected void btnPostInvoice_Click(object sender, EventArgs e)
        {
            string RequestID = "";
            if (grPML.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select atleast One Item')", true);
                return;
            }
            foreach (GridViewRow row in grPML.Rows)
            {

                // TextBox lblConsignmentNumber = (TextBox)row.Cells[0].FindControl("ConsignmentNumber");
                CheckBox chk_chk = (CheckBox)row.Cells[0].FindControl("chk_chk");
                {
                    if (chk_chk.Checked == true)
                    {
                        RequestID += "'" + row.Cells[1].Text + "'";
                    }
                }
            }
            RequestID = RequestID.Replace("''", "','");
            if (RequestID == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select atleast One Item')", true);
                return;
            }
            long lrt = 0;

            try
            {
                string com = @"insert into PM_Invoice (totalAmount )
			                    select sum((R.Rate*IssuedQuantity)+(R.GST*(R.Rate*IssuedQuantity))) as TotalAmount
					            from PR_PackingRequestDetail as prd
                                inner join  PR_PackingRequest Req on prd.PackingRequestID=Req.PackingRequestID
                                inner join PR_packing_material as pm on prd.ItemID = pm.ID
                                inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                inner join PR_PackingMaterialRate as R on R.MaterialID = pm.ID and R.SizeID=pms.ID	
                                inner join CreditClients as C on Req.CustomerID = C.id									 
                                left join COD_CustomerLocations as L on Req.LocationID = L.locationID and C.id=L.CreditClientID									
                                where  prd.PackingRequestID  in (" + RequestID + @") 
                                group by 
                                    prd.PackingRequestID,Req.AppStatus,Req.CustomerID,Req.Address,Req.LocationID,Req.RequestLabel	
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
                    BindGrid();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Invoice has been Posted')", true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        
    }
}