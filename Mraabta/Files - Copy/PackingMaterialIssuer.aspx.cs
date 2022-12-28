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
    public partial class PackingMaterialIssuer : System.Web.UI.Page
    {
        //string constr = ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString();
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
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
                    SqlConnection con = new SqlConnection(constr);
                    con.Open();
                    string com1 = @"SELECT CC.CONTACTPERSON, L.LOCATIONADDRESS ADDRESS, L.LOCATIONNAME, PR.PACKINGREQUESTID,PR.ISSUEDDATE,
                                    ISNULL(R.RATE,'0') RATE, PRC.COST AS COMPANYCOST
                                    FROM PR_PACKINGREQUEST AS PR
                                    INNER JOIN CREDITCLIENTS AS CC ON PR.CUSTOMERID = CC.ID
                                    INNER JOIN COD_CUSTOMERLOCATIONS AS L ON PR.LOCATIONID = L.LOCATIONID AND L.CREDITCLIENTID = CC.ID
                                    INNER JOIN PR_PACKINGREQUESTDETAIL AS PRD ON PRD.PACKINGREQUESTID = PR.PACKINGREQUESTID
	                                LEFT JOIN PR_PACKINGMATERIALRATE R ON L.CREDITCLIENTID = R.CLIENTID
                                    INNER JOIN PR_PACKING_MATERIAL AS PM ON PRD.ITEMID = PM.ID
                                    INNER JOIN PR_PACKINGMATERIALSIZE AS PMS ON PRD.ITEMSIZE = PMS.ID
	                                INNER JOIN PR_COMPANYCOST PRC ON PRC.SIZEID=PMS.ID AND PRC.MATERIALID=PM.ID
                                    WHERE PR.PACKINGREQUESTID=" + PID;

                    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                    DataTable dt2 = new DataTable();
                    adpt1.Fill(dt2);
                    con.Close();
                    if (dt2.Rows.Count > 0)
                    {
                        if (dt2.Rows[0]["IssuedDate"].ToString() == "")
                        {
                            txtCustomer.Text = dt2.Rows[0]["contactPerson"].ToString();
                            txtAddress.Text = dt2.Rows[0]["address"].ToString();
                            ddlLocation.Text = dt2.Rows[0]["locationName"].ToString();
                            lblRate.Text = dt2.Rows[0]["RATE"].ToString();
                            lblCompanyRate.Text = dt2.Rows[0]["COMPANYCOST"].ToString();
                        }
                        else
                        {
                            Response.Redirect("PackingMaterialIssuerList.aspx");
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(@"
                                        SELECT 
	                                        ROW_NUMBER() OVER(ORDER BY PackingRequestDetailID) AS SNO, pm.Name, pms.Size, RequestQuantity, ApprovedQuantity,
	                                        PackingRequestDetailID, ISNULL(r.Rate,'0') Rate, PRC.COST AS CompanyCost
                                        FROM 
	                                        PR_PackingRequestDetail as prd
	                                        INNER JOIN PR_PackingRequest AS ppr ON prd.PackingRequestID = ppr.PackingRequestID
	                                        inner join PR_packing_material as pm on prd.ItemID = pm.ID
	                                        inner join PR_PackingMaterialSize as pms on prd.ItemSize = pms.ID
                                            INNER JOIN PR_COMPANYCOST PRC ON PRC.SIZEID=PMS.ID AND PRC.MATERIALID=PM.ID
	                                        left JOIN PR_PackingMaterialRate R ON ppr.CustomerID = R.ClientId  AND r.MaterialID=pm.ID AND pms.ID=r.SizeID
                                        WHERE 
	                                        prd.PackingRequestID =" + PID))
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
                            con.Close();
                        }
                    }
                }
                else
                {
                    Response.Redirect("PackingMaterialIssuerList.aspx");
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

                foreach (GridViewRow drReview in grdPR.Rows)
                {
                    TextBox ISSQuantity = (TextBox)drReview.FindControl("txtIssuedQuantity");
                    Label ApprovedQuantity = (Label)drReview.FindControl("lblApprovedQuantity");
                    string CN = txt_cn.Text;
                    string Weight = txt_weight.Text;
                    string Pieces = txt_pieces.Text;
                    string Remark = txt_remarks.Text;

                    if (ISSQuantity.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Quantity')", true);
                        return;
                    }

                    if (CN == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Consignment Number')", true);
                        return;
                    }

                    if (Weight == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Weight')", true);
                        return;
                    }

                    if (Pieces == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Pieces')", true);
                        return;
                    }

                    if (double.Parse(ApprovedQuantity.Text) >= double.Parse(ISSQuantity.Text))
                    {

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Issued Quantity cant be greater then Approved Quantity')", true);
                        return;
                    }

                    PRupdate();

                    Label DetailID = (Label)drReview.FindControl("lblDetailID");
                    SqlCommand cmd2 = new SqlCommand("UPDATE PR_PackingRequestDetail SET " +
                                                     "IssuedQuantity = @IssuedQuantity, " +
                                                     "ConsignmentNo = @ConsignmentNo,  " +
                                                     "CNWeight = @CNWeight, " +
                                                     "Pieces = @Pieces, " +
                                                     "Remarks = @Remarks " +
                                                     "WHERE PackingRequestDetailID =" + DetailID.Text);

                    cmd2.Parameters.AddWithValue("@IssuedQuantity", Convert.ToInt32(ISSQuantity.Text));
                    cmd2.Parameters.AddWithValue("@ConsignmentNo", CN);
                    cmd2.Parameters.AddWithValue("@CNWeight", Weight);
                    cmd2.Parameters.AddWithValue("@Pieces", Pieces);
                    cmd2.Parameters.AddWithValue("@Remarks", Remark);  
                    cmd2.Connection = con;
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Issued Packing Request ID: " + txtRequestNumber.Text + "')", true);

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                string ecnryptedConsignment = txtRequestNumber.Text;
                string script = String.Format(script_, "Material_AddressLabel.aspx?id=" + ecnryptedConsignment, "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        void PRupdate()
        {
            SqlConnection con = new SqlConnection(constr);
            string query = "UPDATE PR_PackingRequest SET IssuedBy = @IssuedBy, IssuedDate = getdate(), AppStatus = @AppStatus, RequestStatus = @RequestStatus WHERE PackingRequestID =" + txtRequestNumber.Text;
            SqlCommand cmd = new SqlCommand(query);

            cmd.Parameters.AddWithValue("@IssuedBy", Convert.ToInt32(Session["u_ID"].ToString()));
            cmd.Parameters.AddWithValue("@AppStatus", "Pending Invoice");
            cmd.Parameters.AddWithValue("@RequestStatus", 3);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}