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
    public partial class PickupRider_Details : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        string ticketNumber_ = "";
        String U_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                U_ID = Session["U_ID"].ToString();
                if (!IsPostBack)
                {
                    GetResult();
                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }

        private void GetResult()
        {
            ticketNumber_ = Request.QueryString["TicketNumber"];

            updateReasonDDL();
            DataTable ds = GetPickupDetails(ticketNumber_);

            DataTable ds_modifier = GetPriceModifierDetails(ticketNumber_);

            if (ds_modifier.Rows.Count != 0)
            {
                String literalText = "";
                for (int j = 0; j < ds_modifier.Rows.Count; j++)
                {
                    literalText += @"  <tr>
                                <td style='border-top: 1px solid black;'>
                                    <h3>Price Modifier</h3>
                                </td>
                            </tr>
                                <tr>
                                <td><b>Modifier Name</b></td>
                                <td>
                                    <asp:Label ID='ModifierName' runat='server'>" + ds_modifier.Rows[j]["name"].ToString() + @"</asp:Label>
                            </td>
                                <td><b>per piece amount</b></td>
                                <td>
                                    <asp:Label ID='perPieceValue' runat='server'>" + ds_modifier.Rows[j]["modifiedcalculationvalue"].ToString() + @"</asp:Label>
                                </td>
                                <td><b>Pieces</b></td>
                                <td>
                                    <asp:Label ID='PiecesModifier' runat='server'>" + ds_modifier.Rows[j]["pieces"].ToString() + @"</asp:Label>
                                </td>
                                <td><b>Total Amount</b></td>
                                <td>
                                    <asp:Label ID='TotalamountModifier' runat='server'>" + ds_modifier.Rows[j]["calculatedvalue"].ToString() + @"</asp:Label>
                                </td>
                               
                            </tr>";
                }
                literalModifier.Text = literalText;
            }
            else
            {
                String literalText = "";


                literalModifier.Text = literalText;
            }

            if (ds.Rows.Count != 0)
            {
                Origin.Text = ds.Rows[0]["orginName"].ToString();
                Destination.Text = ds.Rows[0]["destinationName"].ToString();
                lbl_ticketNumber.Text = ds.Rows[0]["ticketNumber"].ToString();
                consignee.Text = ds.Rows[0]["consignee"].ToString();
                consigneeAddress.Text = ds.Rows[0]["consigneeAddress"].ToString();
                ConsigneePhone.Text = ds.Rows[0]["ConsigneeCellNo"].ToString();
                Consignor.Text = ds.Rows[0]["Consigner"].ToString();
                consignorAddress.Text = ds.Rows[0]["consignerAddress"].ToString();
                consignorPhone.Text = ds.Rows[0]["consignerCellNo"].ToString();
                Service.Text = ds.Rows[0]["serviceTypeName"].ToString();
                Pieces.Text = ds.Rows[0]["Pieces"].ToString();
                Weight.Text = ds.Rows[0]["Weight"].ToString();
                total_Amount.Text = ds.Rows[0]["totalAmount"].ToString();
                PackageContent.Text = ds.Rows[0]["pakageContents"].ToString();
                specialInstructions.Text = ds.Rows[0]["pakageContents"].ToString();
                lblRider.Text = ds.Rows[0]["rider"].ToString();
                rem_txtbox.Text = ds.Rows[0]["Remarks"].ToString();
                lblgst.Text = ds.Rows[0]["gst"].ToString();
                lblGross.Text = ds.Rows[0]["chargedAmount"].ToString();
                if (ds.Rows[0]["chargedAmount"].ToString() != "")
                {
                    ReasonList.SelectedValue = ds.Rows[0]["reason"].ToString();
                }
            }
            else
            {
                error_msg.Text = "No Record Found...";
            }
        }

        private void updateReasonDDL()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "select * from MNP_PreBookingReason WHERE STATUS=1";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);

                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
                ReasonList.DataSource = dt;
                ReasonList.DataTextField = "NAME";
                ReasonList.DataValueField = "id";
                ReasonList.DataBind();
                ReasonList.Items.Insert(0, new ListItem("Select Reason", "0"));

            }
            catch (Exception er)
            {
            }
        }

        private DataTable GetPriceModifierDetails(string ticketNumber_)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM PreBookingConsignmentModifier pbc \n"
               + "INNER JOIN PriceModifiers pm ON pm.id=pbc.pricemodifierId \n"
               + "WHERE consignmentNumber='" + ticketNumber_ + "'  ORDER BY pbc.SortOrder";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);

                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception er)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Error finding details')</script>");
            }
            return dt;
        }

        private DataTable GetPickupDetails(string ticketNumber_)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT mpb.*, \n"
               + "       b1.name     orginName, \n"
               + "       b2.name     destinationName,r.firstName+' '+r.lastName+'-'+r.riderCode rider\n"
               + "FROM   MNP_PreBookingConsignment mpb \n"
               + "       INNER JOIN branches b1 \n"
               + "            ON  b1.branchCode = mpb.orgin \n"
               + "       INNER JOIN branches b2 \n"
               + "            ON  b2.branchCode = mpb.destination "
               + "       left JOIN riders r "
               + "          ON r.riderCode = mpb.RiderCode"
               + " where  mpb.ticketnumber='" + ticketNumber_ + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);

                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception er)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Error finding details')</script>");
            }
            return dt;
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            String remarks = rem_txtbox.Text;
            String selectedReason = ReasonList.SelectedValue.ToString();
            if (selectedReason != "0")
            {
                String st = "update MNP_PreBookingConsignment set Remarks='" + remarks + "',reason='" + selectedReason + "',modifiedOn=getdate(),modifiedby=" + U_ID + " where ticketNumber='" + lbl_ticketNumber.Text.ToString() + "'";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                try
                {
                    orcl.Open();
                    SqlCommand sqlcom = new SqlCommand(st, orcl);
                    sqlcom.ExecuteNonQuery();
                    //ClientScript.RegisterStartupScript(Page.GetType(), "validationn", "<script language='javascript'>alert('Remarks Updated')</script>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Remarks updated');", true);
                }
                catch (SqlException ex)
                {
                    //ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Error updating rider')</script>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Error updating remarks');", true);
                }
                finally
                {
                    orcl.Close();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select remarks ');", true);
            }
        }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            rem_txtbox.Text = "";
        }
    }
}