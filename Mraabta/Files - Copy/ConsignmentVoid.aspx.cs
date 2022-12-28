using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class ConsignmentVoid : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        CommonFunction func = new CommonFunction();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetRemarks();
                if (Request.QueryString.Keys.Count != 0)
                {

                    txt_consignmentNumber.Text = Request.QueryString["XCode"];
                    txt_consignmentNumber.Enabled = false;
                    btn_print.Visible = true;
                    ConsignmentDetails();

                }
            }
        }

        protected void GetRemarks()
        {
            DataTable dt = new DataTable();
            dt = func.GetRemarks();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_remarks.DataSource = dt;
                    dd_remarks.DataTextField = "SDESC";
                    dd_remarks.DataValueField = "RemarksID";
                    dd_remarks.DataBind();

                    //dd_remarks.Items.Add(new ListItem("Others", "OTH"));
                }
            }
        }
        protected void ConsignmentDetails()
        {
            DataTable dt = new DataTable();
            clvar.consignmentNo = txt_consignmentNumber.Text;
            dt = con.GetConsignmentDetail(clvar);
            if (dt.Rows.Count > 0)
            {
                txt_consignmentNumber.Text = dt.Rows[0]["ConNo"].ToString();
                lbl_accountNo.Text = dt.Rows[0]["AccountNo"].ToString();
                lbl_bookingDate.Text = dt.Rows[0]["BookingDate"].ToString();
                lbl_city.Text = dt.Rows[0]["City"].ToString();
                lbl_destination.Text = dt.Rows[0]["Branch"].ToString();
                lbl_discount.Text = dt.Rows[0]["Discount"].ToString();
                lbl_gstCharges.Text = dt.Rows[0]["gst"].ToString();
                lbl_insurance.Text = "00000";
                lbl_origin.Text = dt.Rows[0]["ORigin"].ToString();
                lbl_originExpressCenter.Text = dt.Rows[0]["OriginExpressCenter"].ToString();
                lbl_packageContents.Text = dt.Rows[0]["PackageContents"].ToString();
                lbl_pieces.Text = dt.Rows[0]["Pieces"].ToString();
                lbl_riderCode.Text = dt.Rows[0]["RiderCode"].ToString();
                lbl_serviceType.Text = dt.Rows[0]["ServiceTypeName"].ToString();
                lbl_totalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
                lbl_totalCharges.Text = dt.Rows[0]["ChargedAmount"].ToString();
                lbl_weight.Text = dt.Rows[0]["Weight"].ToString();
                lbl_consignee.Text = dt.Rows[0]["Consignee"].ToString();
                lbl_consigneeAddress.Text = dt.Rows[0]["Address"].ToString();
                lbl_consigneeCell.Text = dt.Rows[0]["ConsigneeCell"].ToString();
                lbl_consigneeCNIC.Text = dt.Rows[0]["ConsigneeCNIC"].ToString();
                lbl_consigner.Text = dt.Rows[0]["Consigner"].ToString();
                lbl_consignerAddress.Text = dt.Rows[0]["ShipperAddress"].ToString();
                lbl_consignerCell.Text = dt.Rows[0]["ConsignerCell"].ToString();
                lbl_consignerCNIC.Text = dt.Rows[0]["ConsignerCNIC"].ToString();
                lbl_consignmentType.Text = dt.Rows[0]["ConType"].ToString();
                if (dt.Rows[0]["CODTYPE"].ToString() != "0")
                {
                    cb_COD.Checked = true;
                }
                else
                {
                    cb_COD.Checked = false;
                }
            }

            DataTable cod = new DataTable();
            cod = con.GetCodDetailByConsignmentNumber(clvar);
            if (cod.Rows.Count > 0)
            {
                lbl_orderRef.Text = cod.Rows[0]["orderrefNo"].ToString();
                lbl_productType.Text = cod.Rows[0]["PRODUCTTYPEID"].ToString();
                if (cod.Rows[0]["ChargeCodAmount"].ToString() != "0")
                {
                    Cb_CODAmount.Checked = true;
                }
                else
                {
                    Cb_CODAmount.Checked = false;
                }

                lbl_descriptionCOD.Text = cod.Rows[0]["ProductDescription"].ToString();
                lbl_CODAmount.Text = cod.Rows[0]["CODAMount"].ToString();
            }

            DataTable priceModifier = con.GetConsignmentModifier(clvar);
            if (priceModifier.Rows.Count > 0)
            {
                RadGrid1.DataSource = priceModifier;
                RadGrid1.DataBind();
            }


        }

        protected void txt_consignmentNumber_TextChanged(object sender, EventArgs e)
        {
            ConsignmentDetails();
        }
        protected void btn_PrintConsignment_Click(object sender, EventArgs e)
        {

            clvar.VoidConsignment = true;
            clvar.consignmentNo = txt_consignmentNumber.Text;
            clvar.Branch = con.GetBranchCodeForEC().Rows[0][0].ToString();
            clvar.Remarks = txt_Remarks.Text;

            if (dd_remarks.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Remarks')", true);
                return;
            }

            if (dd_remarks.SelectedItem.Text.ToUpper() == "OTHERS" && txt_Remarks.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter Remarks')", true);
                return;
            }

            clvar.Company = dd_remarks.SelectedValue.Split('-')[0];
            clvar.RemarksID = dd_remarks.SelectedValue.Split('-')[1];



            if (txt_Remarks.Text.Trim(' ') == "" || txt_Remarks.Text.Trim(' ') == null)
            {

            }
            string error = con.VoidConsignment(clvar);
            if (error != "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + error + "')", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Operation Completed\\n Consignment Number: " + clvar.consignmentNo + " is void')", true);
                return;
            }


        }
        protected void dd_remarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_remarks.SelectedItem.Text.ToUpper() == "OTHERS")
            {
                txt_Remarks.Text = "";
                txt_Remarks.Visible = true;
            }
            else
            {
                txt_Remarks.Text = "";
                txt_Remarks.Visible = false;
            }
        }
    }
}