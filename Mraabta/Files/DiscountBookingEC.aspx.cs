using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Collections;
using Telerik.Web.UI;
using System.Text.RegularExpressions;

namespace MRaabta.Files
{
    public partial class DiscountBookingEC : System.Web.UI.Page
    {
        CommonFunction CF = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();

        String ServiceType = "";
        string CODType = "";
        DataTable dt_discount;
        string limitover = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- Remove 13-04-2020 but when Retail Project Live below line uncomment
            if (Session["BookingUserStatus"].ToString() == "1")
            {
                txt_RiderCode.Text = Session["BookingStaff"].ToString();
                txt_AccNo.Text = "0";
                txt_AccNo.Enabled = false;
            }

            if (!IsPostBack)
            {
                if (Session["BRANCHCODE"] != null)
                {
                    if (Session["BRANCHCODE"].ToString() != "ALL")
                    {
                        cb_Origin.SelectedValue = Session["BRANCHCODE"].ToString();
                        clvar.origin = Session["BRANCHCODE"].ToString();
                    }
                    if (Session["ZONECODE"].ToString() != "ALL")
                    {
                        clvar.Zone = Session["ZONECODE"].ToString();
                    }

                    Get_Branches();
                    Get_Discount();
                    Get_origin();
                    Get_Service_Type();
                    Get_ConType();
                    Get_PriceModifiers();
                    Get_Cities();
                    Get_OriginExpressCenter();
                    Get_Paymentsource();
                    DataTable dtc = new DataTable();
                    dtc.Columns.AddRange(new DataColumn[15]
                {
                new DataColumn("PriceModifierID", typeof(int)),
                new DataColumn("ConsignmentNumber", typeof(Int64)),
                new DataColumn("CreatedBy",typeof(string)),
                new DataColumn("CreatedOn", typeof(DateTime)),
                new DataColumn("ModifiedBy", typeof(string)),
                new DataColumn("ModifiedOn", typeof(DateTime)),
                new DataColumn("ModifiedCalculationValue", typeof(float)),
                new DataColumn("CalculatedValue", typeof(float)),
                new DataColumn("CalculatedGST", typeof(float)),
                new DataColumn("CalculationBase", typeof(int)),
                new DataColumn("isTaxable", typeof(bool)),
                new DataColumn("SortOrder", typeof(int)),
                new DataColumn("isGST", typeof(string)),
                new DataColumn("AlternateValue", typeof(string)),
                new DataColumn("Pieces", typeof(string))

                });
                    dtc.AcceptChanges();
                    ViewState["PM"] = dtc;
                    dt_Picker.Text = DateTime.Now.ToShortDateString();
                    dt_Picker.Enabled = false;

                    //dt_Picker.MinDate = DateTime.Now.Date;
                    //dt_Picker.MaxDate = DateTime.Now.Date;

                    //txt_insurance.Text = "2.50";
                    //GetSequence(clvar);
                    //GetCodSequence("");
                    clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();

                    // --- Remove 13-04-2020 but when Retail Project Live below line uncomment
                    // txt_RiderCode.Text = HttpContext.Current.Session["BookingStaff"].ToString();


                    //CustomerInformation(clvar);

                    lbl_ScreenName.Text = "Discount Booking";
                    // Get Ticket Number From CS Team
                    if (Request.QueryString["TicketNumber"] != null)
                    {
                        string TicketNo = Request.QueryString["TicketNumber"].ToString();
                        lbl_ScreenName.Text = "HPS Booking";
                        Get_TicketData(TicketNo);
                    }
                }
            }
        }
        public void Get_Cities()
        {
            //DataTable dt = CF.Cities();

            //dd_city.DataSource = dt;
            //dd_city.DataTextField = "ServiceName";
            //dd_city.DataValueField = "ServiceID";
            //dd_city.DataBind();


            DataTable dt = Cities_();
            this.dd_city.DataTextField = "sname";
            this.dd_city.DataValueField = "branchCode";
            this.dd_city.DataSource = dt;//ds.Tables[0].DefaultView;
            this.dd_city.DataBind();
            ViewState["cities"] = dt;
        }
        public void Get_Branches()
        {
            clvar.CityCode = dd_city.SelectedValue;
            DataSet ds = CF.ExpressCenterLocal(clvar);
            ViewState["city_dest"] = ds;
        }
        public void Get_Discount()
        {
            DataTable dt = Discount();
            this.dd_discount.DataTextField = "SDESC";
            this.dd_discount.DataValueField = "DiscountID";
            this.dd_discount.DataSource = dt;//ds.Tables[0].DefaultView;
            this.dd_discount.DataBind();
        }
        public void Get_Destinations()
        {
            clvar.expresscenter = dd_city.SelectedValue;
            DataSet ds = (DataSet)ViewState["city_dest"];

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    string expresscenter = "";
                    DataRow[] dr = ds.Tables[0].Select("ServiceID='" + dd_city.SelectedValue + "'");
                    if (dr.Length != 0)
                    {
                        DataTable dt = dr.CopyToDataTable();
                        expresscenter = dr[0]["expresscentercode"].ToString();

                        ViewState["Ex_Info"] = dt;// ds.Tables[0];
                        DataTable ds_ = (DataTable)ViewState["Ex_Info"];
                        if (ds_.Rows.Count != 0)
                        {
                            DataRow[] dr_ = ds_.Select("expressCenterCode='" + expresscenter + "'");

                            if (dr.Length != 0)
                            {
                                hd_Destination.Value = dr[0]["bid"].ToString();
                                hd_DestinationZone.Value = dr[0]["ZoneCode"].ToString();
                                hd_Destination_Ec.Value = dr[0]["expressCenterCode"].ToString();
                            }
                        }
                        else
                        {
                            //cb_Destination.SelectedValue = "0";
                        }

                    }
                    else
                    {
                        //this.cb_Destination.Items.Clear();
                        //this.cb_Destination.Text = "";

                    }
                }
            }
        }
        public void Get_origin()
        {
            DataSet ds = CF.Branch();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                this.cb_Origin.DataTextField = "BranchName";
                this.cb_Origin.DataValueField = "branchCode";
                this.cb_Origin.DataSource = ds.Tables[0].DefaultView;
                this.cb_Origin.DataBind();
            }
        }
        public void Get_Service_Type()
        {
            DataSet ds = CF.ServiceTypeName();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //       this.cb_ServiceType.Items.Add(new RadComboBoxItem("Select Service Type Name", "0"));
                this.cb_ServiceType.DataTextField = "ServiceTypeName";
                this.cb_ServiceType.DataValueField = "ServiceTypeName";
                this.cb_ServiceType.DataSource = ds.Tables[0].DefaultView;
                this.cb_ServiceType.DataBind();

                cb_ServiceType.SelectedValue = "overnight";
            }
        }
        public void Get_ConType()
        {
            DataSet ds = ConsignmentType();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //   this.cb_ConType.Items.Add(new ListItem("Select Consignment Type", "0"));
                this.cb_ConType.DataTextField = "ConsignmentType";
                this.cb_ConType.DataValueField = "id";
                this.cb_ConType.DataSource = ds.Tables[0].DefaultView;
                this.cb_ConType.DataBind();
                cb_ConType.SelectedValue = "12";
            }
        }
        public void Get_PriceModifiers()
        {
            DataSet ds = PriceModifiers();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                this.cb_PriceModifier.Items.Add(new ListItem { Text = "Select Price Modifier", Value = "0" });
                this.cb_PriceModifier.DataTextField = "PriceModifier";
                this.cb_PriceModifier.DataValueField = "id";
                this.cb_PriceModifier.DataSource = ds.Tables[0].DefaultView;
                this.cb_PriceModifier.DataBind();

                ViewState["PM_"] = ds.Tables[0];
            }
        }
        public void Get_OriginExpressCenter()
        {

            DataSet ds = CF.ExpressCenterOrigin(clvar);
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //this.cb_Destination.Items.Add(new DropDownListItem("Select Destination", "0"));
                this.cb_originExpresscenter.Items.Add(new ListItem { Text = "Select Origin EC", Value = "0" });
                this.cb_originExpresscenter.AppendDataBoundItems = true;
                //this.cb_originExpresscenter.Items.Clear();
                this.cb_originExpresscenter.DataTextField = "Name";
                this.cb_originExpresscenter.DataValueField = "expresscentercode";
                this.cb_originExpresscenter.DataSource = ds.Tables[0].DefaultView;
                this.cb_originExpresscenter.DataBind();
                this.cb_originExpresscenter.Enabled = false;
                cb_originExpresscenter.SelectedValue = HttpContext.Current.Session["ExpressCenter"].ToString();
                ViewState["Ex_origin"] = ds.Tables[0];
                // hd_oecCatid.Value = ds.Tables[0].Rows[0]["CategoryID"].toString(); 
            }
        }
        protected void RadGrid1_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }
        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
        protected void RadGrid1_ItemInserted(object sender, Telerik.Web.UI.GridInsertedEventArgs e)
        {

        }
        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
        }
        protected void cb_Account_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cb_Account.Checked == true)
            {
                txt_AccNo.Enabled = false;
            }
            else
            {
                txt_AccNo.Enabled = true;
            }
        }
        protected void txt_AccNo_Fixed_TextChanged(object sender, EventArgs e)
        {
            if (txt_AccNo.Text == "280605")
            {
                dd_city.SelectedValue = "31";
                dd_city.Enabled = false;

                txt_Type.Text = "DHAQ-";
                txt_Type.MaxLength = 11;
                txt_Package_Handcarry.Text = "DHA Quetta Form";
                txt_Package_Handcarry.Enabled = false;

                txt_remarks.Text = "Deposit Slip No: ";
                txt_Consignee.Text = "DHA QUETTA";
                txt_Consignee.Enabled = false;
                txt_ConsigneeCNIC.Enabled = false;
                txt_ConsigneeCellno.Enabled = false;
                txt_Address.Enabled = false;
                txt_RiderCode.Focus();
            }
            else
            {
                txt_Type.Text = "";
                dd_city.SelectedValue = "0";
                dd_city.Enabled = true;
                txt_Type.MaxLength = 500;
                txt_remarks.Text = "";
                txt_Consignee.Text = "";
                txt_Consignee.Enabled = true;
                txt_ConsigneeCNIC.Enabled = true;
                txt_ConsigneeCellno.Enabled = true;
                txt_Address.Enabled = true;
                txt_RiderCode.Focus();
                txt_Package_Handcarry.Text = "";
                txt_Package_Handcarry.Enabled = true;

            }
        }
        protected void cb_Rider_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cb_Rider.Checked == true)
            {
                txt_RiderCode.Enabled = false;
            }
            else
            {
                txt_RiderCode.Enabled = true;
            }
        }
        protected void txt_ConNo_TextChanged(object sender, EventArgs e)
        {


            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('1')", true);
            //return;
            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;

            DataSet ds = con.Consignment(clvar);
            DataTable dt = ds.Tables[0];
            if (txt_ConNo.Text.Length >= 12)
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                clvar.consignmentNo = txt_ConNo.Text.Trim();
                DataTable zoneSequence = ViewState["ZoneSequence"] as DataTable;
                if (zoneSequence != null)
                {
                    if (zoneSequence.Rows.Count > 0)
                    {
                        bool valid = false;
                        foreach (DataRow dr in zoneSequence.Rows)
                        {
                            Int64 start = Int64.Parse(dr["SequenceStart"].ToString());
                            Int64 end = Int64.Parse(dr["SequenceEnd"].ToString());
                            Int64 cn = Int64.Parse(txt_ConNo.Text.Trim());
                            if (cn >= start && cn <= end)
                            {
                                valid = true;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not Allowed in this Zone.')", true);
                            lbl_Error.Text = "This CN Number is Not valid.";
                            return;
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                        lbl_Error.Text = "This CN Number is Not valid.";
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                    lbl_Error.Text = "This CN Number is Not valid.";
                    return;
                }
                //if (SequenceCheck(clvar, "").Rows.Count == 0)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                //    lbl_Error.Text = "This CN Number is Not valid.";
                //    btn_reset_Click(sender, e);
                //    txt_ConNo.Focus();
                //    return;
                //}
                if (ds.Tables[0].Rows.Count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Number/Duplicate Consignment No.')", true);
                    txt_ConNo.Text = "";
                    txt_ConNo.Focus();
                    // btn_SaveConsignment.Enabled = false;
                    txt_ConNo.Text = dt.Rows[0]["ConsignmentNumber"].ToString();
                    txt_Consignee.Text = dt.Rows[0]["consignee"].ToString();
                    txt_CodAmount.Text = "0";
                    txt_AccNo.Text = dt.Rows[0]["ConsignerAccountNo"].ToString();
                    txt_Address.Text = dt.Rows[0]["Address"].ToString();
                    txt_ShipperAddress.Text = dt.Rows[0]["ShipperAddress"].ToString();
                    txt_Consignee.Text = dt.Rows[0]["Consignee"].ToString();
                    txt_ConsigneeCellno.Text = dt.Rows[0]["consigneePhoneNo"].ToString();
                    txt_ConsigneeCNIC.Text = dt.Rows[0]["ConsigneeCNICNO"].ToString();
                    txt_Consigner.Text = dt.Rows[0]["Consigner"].ToString();
                    txt_ConsignerCellNo.Text = dt.Rows[0]["ConsignerCellNo"].ToString();
                    Txt_ConsignerCNIC.Text = dt.Rows[0]["ConsignerCnicNo"].ToString();
                    //txt_DeclaredValue.Text = dt.Rows[0]["DecalaredValue"].ToString();
                    txt_Description.Text = "";
                    txt_Description2.Text = "";
                    //txt_insurance.Text = dt.Rows[0]["insuarancePercentage"].ToString();
                    txt_Piecies.Text = dt.Rows[0]["Pieces"].ToString();
                    txt_RiderCode.Text = dt.Rows[0]["RiderCode"].ToString();
                    txt_TotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
                    txt_Type.Text = "";
                    txt_Weight.Text = dt.Rows[0]["Weight"].ToString();
                    txt_Package_Handcarry.Text = dt.Rows[0]["PAKAGECONTENTS"].ToString();

                    cb_ServiceType.SelectedValue = dt.Rows[0]["serviceTypeName"].ToString();
                    dd_city.SelectedValue = dt.Rows[0]["destination"].ToString();
                }
                else
                {
                    //   ResetAll_();
                    txt_AccNo.Focus();
                    lbl_Error.Text = "";
                    DataTable CODsequece = ViewState["CODSequence"] as DataTable;



                    if (CODsequece != null)
                    {
                        if (CODsequece.Rows.Count > 0)
                        {
                            bool valid = false;
                            foreach (DataRow dr in CODsequece.Rows)
                            {
                                Int64 start = Int64.Parse(dr["SequenceStart"].ToString());
                                Int64 end = Int64.Parse(dr["SequenceEnd"].ToString());
                                Int64 cn = Int64.Parse(txt_ConNo.Text.Trim());
                                if (cn >= start && cn <= end)
                                {
                                    valid = true;
                                    break;
                                }
                            }
                            if (valid)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN is Reserved for COD.')", true);
                                lbl_Error.Text = "This CN is Reserved for COD.";
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                            lbl_Error.Text = "This CN Number is Not valid.";
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                        lbl_Error.Text = "This CN Number is Not valid.";
                        return;
                    }
                    //if (CF.CheckForCodSequence(txt_ConNo.Text).Rows.Count > 0)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is reserved for COD Client.')", true);
                    //    lbl_Error.Text = "This CN Number is reserved for COD Client.";
                    //    btn_reset_Click(sender, e);
                    //    return;
                    //}


                }
            }
            else
            {
                txt_ConNo.Text = "";
                txt_ConNo.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment No cannot be less than 12 Digits')", true);
                txt_ConNo.Focus();

                return;

            }

        }
        protected void txt_AccNo_TextChanged(object sender, EventArgs e)
        {
            if (txt_AccNo.Text != "")
            {
                if (txt_AccNo.Text != "")
                {
                    clvar.Branch = cb_Origin.SelectedValue;
                    clvar.AccountNo = txt_AccNo.Text;
                    DataSet ds = con.CustomerInformation(clvar);
                    if (ds.Tables.Count != 0)
                    {
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            txt_Consigner.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                            txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                            txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                            hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();

                            if (ds.Tables[0].Rows[0]["accountNo"].ToString() == "0")
                            {
                                this.Rb_CustomerType.Items[1].Selected = true;
                            }
                            else
                            {
                                this.Rb_CustomerType.Items[0].Selected = true;
                            }

                            if (ds.Tables[0].Rows[0]["IsCOD"].ToString() == "True")
                            {
                                cb_COD.Checked = true;
                                Cb_CODAmount.Checked = true;
                                txt_OrderRefNo.Enabled = true;
                                dd_ProductType.Enabled = true;
                                txt_Description.Enabled = true;
                                txt_CodAmount.Enabled = true;

                                //Now Populating Product Type of COD
                                clvar.CustomerClientID = ds.Tables[0].Rows[0]["id"].ToString();
                                ViewState["CODType"] = ds.Tables[0].Rows[0]["CODType"].ToString();
                                DataSet ds_Product = con.ProductTypeInfo(clvar);
                                if (ds_Product.Tables[0].Rows.Count != 0)
                                {
                                    dd_ProductType.Items.Add(new ListItem { Text = "Select Product", Value = "0" });
                                    dd_ProductType.DataTextField = "ProductTypeName";
                                    dd_ProductType.DataValueField = "Producttypecode";
                                    dd_ProductType.DataSource = ds_Product.Tables[0].DefaultView;
                                    dd_ProductType.DataBind();
                                }
                                else
                                {
                                    dd_ProductType.Items.Clear();
                                }
                            }
                            else
                            {
                                cb_COD.Checked = false;
                                Cb_CODAmount.Checked = false;
                                txt_OrderRefNo.Enabled = false;
                                dd_ProductType.Enabled = false;
                                txt_Description.Enabled = false;
                                txt_CodAmount.Enabled = false;
                            }
                        }



                        lbl_Error.Text = "";
                        txt_RiderCode.Focus();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                        txt_AccNo.Text = "";
                        txt_AccNo.Focus();
                    }
                }
            }

            if (txt_AccNo.Text.Trim(' ') == "0")
            {
                txt_Consigner.Text = "";//ds.Tables[0].Rows[0]["Name"].ToString();
                txt_ConsignerCellNo.Text = "";// ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                txt_ShipperAddress.Text = ""; //ds.Tables[0].Rows[0]["address"].ToString();
            }


        }
        //protected void cb_ConType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cb_ConType.SelectedValue != "0")
        //    {
        //        if (cb_ConType.SelectedValue == "12")
        //        {
        //            lbl_Type.Text = "Coupon No";

        //            //dd_city.Focus();
        //        }
        //        if (cb_ConType.SelectedValue == "13")
        //        {
        //            lbl_Type.Text = "Flyer";
        //            //dd_city.Focus();
        //        }
        //        if (cb_ConType.SelectedValue == "14")
        //        {
        //            lbl_Type.Text = "Ref. No";
        //            //dd_city.Focus();
        //        }
        //    }
        //    //dd_city.Focus();
        //    lbl_Error.Text = "";
        //}
        protected void cb_Insurance_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Insurance.Checked)
                lbl_InsuranceMsg.Text = "Insurance Offered but Refused";
            //if (cb_Insurance.Checked == true)
            //{
            //    txt_DeclaredValue.Enabled = true;
            //    txt_DeclaredValue.Focus();
            //}
            //else
            //{
            //    txt_DeclaredValue.Text = "0";
            //    txt_DeclaredValue.Enabled = false;
            //    txt_Package_Handcarry.Focus();
            //}
        }
        protected void cb_PriceModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_PriceModifier.SelectedValue != "0")
            {
                DataTable dt_1 = (DataTable)ViewState["TariffInfo"];
                double DV = 0;
                DataRow[] dr2 = null;

                txt_pricequantity.Text = "1";

                DataTable dt_ = (DataTable)ViewState["PM_"];

                txt_Description2.Text = cb_PriceModifier.SelectedItem.Text;

                if (cb_PriceModifier.SelectedValue.ToString() == "98" || cb_PriceModifier.SelectedValue.ToString() == "100")
                {
                    div_InsuranceMsg.Visible = false;
                    div_InsuranceMsg.Style.Add("display", "none");
                }
                else
                {
                    div_InsuranceMsg.Visible = true;
                    div_InsuranceMsg.Style.Add("display", "block");
                }


                DataRow[] dr = dt_.Select(" id ='" + cb_PriceModifier.SelectedValue + "'");

                //txt_pricequantity

                if (cb_PriceModifier.SelectedValue == "111" || cb_PriceModifier.SelectedValue == "112" || cb_PriceModifier.SelectedValue == "113" ||
                    cb_PriceModifier.SelectedValue == "114" || cb_PriceModifier.SelectedValue == "115" || cb_PriceModifier.SelectedValue == "116" ||
                    cb_PriceModifier.SelectedValue == "117")
                {
                    txt_pricequantity.Text = "1";
                    txt_pricequantity.Enabled = true;
                }

                if (txt_Description2.Text != "Extra Charges")
                {
                    if (dr.Length != 0)
                    {
                        Double ach = Double.Parse(dr[0][2].ToString());
                        txt_Value.Text = string.Format("{0:N2}", ach); //string.Format("{0:N0}", (txt_TotalCharges.Value / 100) * double.Parse(string.Format("{0:N0}", ach)));

                        //txt_pricequantity.Text = "0";
                        //txt_pricequantity.Enabled = false;

                        if (dr[0][4].ToString() == "2")
                        {
                            rb_Calculation.SelectedValue = "2";
                            txt_insuranceDeclaredValue.Enabled = false;

                        }
                        else if (dr[0][4].ToString() == "1")
                        {
                            rb_Calculation.SelectedValue = "1";
                            txt_insuranceDeclaredValue.Enabled = false;
                        }
                        else if (dr[0][4].ToString() == "3")
                        {
                            rb_Calculation.SelectedValue = "3";
                            txt_insuranceDeclaredValue.Enabled = true;
                        }
                    }
                }
                else
                {
                    txt_pricequantity.Text = "0";
                    txt_pricequantity.Enabled = false;

                    txt_Value.Text = string.Format("{0:N2}", dr[0][2].ToString());
                    rb_Calculation.SelectedValue = "1";
                }
            }
            else
            {
                cb_PriceModifier.SelectedValue = "0";
            }
            lbl_Error.Text = "";
        }
        protected void rd_AddPriceModifier_Click(object sender, EventArgs e)
        {

            Int64 tempDeclaredValue = 0;
            Int64.TryParse(txt_insuranceDeclaredValue.Text.Trim(), out tempDeclaredValue);
            if (rb_Calculation.SelectedValue == "3" && (txt_insuranceDeclaredValue.Text.Trim() == "" || txt_insuranceDeclaredValue.Text.Trim() == "0" || tempDeclaredValue <= 0))
            {
                Alert("Please Provide Proper Declared Value.");
                return;
            }
            else if (rb_Calculation.SelectedValue == "3")
            {
                //if (tempDeclaredValue < 2000)
                //{
                //    Alert("Declared Value must be greater than 2,000");
                //    txt_insuranceDeclaredValue.Text = "";
                //    return;
                //}
                // // Added following line Date: 21-09-2019 by Bilal
                if (tempDeclaredValue < 500)
                {
                    Alert("Declared Value must be greater than 500");
                    txt_insuranceDeclaredValue.Text = "";
                    return;
                }
                // Added following line Date: 26-07-2019 by Bilal
                else if (tempDeclaredValue > 300000)
                {
                    Alert("Declared Value must not exceed 300,000");
                    txt_insuranceDeclaredValue.Text = "";
                    return;
                }
                //else if (tempDeclaredValue > 200000)
                //{
                //    Alert("Declared Value must not exceed 200,000");
                //    txt_insuranceDeclaredValue.Text = "";
                //    return;
                //}
            }

            DataTable dt = (DataTable)ViewState["PM"];
            string PM = cb_PriceModifier.SelectedValue;
            DataRow[] dr = dt.Select(" priceModifierId ='" + cb_PriceModifier.SelectedItem.Value + "'");
            clvar.origin = cb_Origin.SelectedValue;
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";

            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            if (dr.Length == 0)
            {
                if (txt_Description2.Text != "Extra Charges")
                {

                    DataTable dt_ = (DataTable)ViewState["PM_"];
                    //   DataRow   dr = dt.NewRow()
                    dr = dt_.Select(" id ='" + cb_PriceModifier.SelectedValue + "'");

                    DataRow dr_ = dt.NewRow();

                    dr_["pricemodifierid"] = int.Parse(cb_PriceModifier.SelectedItem.Value);
                    dr_["ConsignmentNumber"] = Int64.Parse(txt_ConNo.Text);
                    dr_["CreatedBy"] = "TEST";
                    dr_["CreatedOn"] = DateTime.Now;
                    dr_["ModifiedBy"] = "";
                    dr_["ModifiedOn"] = DBNull.Value;

                    if (int.Parse(cb_PriceModifier.SelectedItem.Value) == 111 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 112 ||
                    int.Parse(cb_PriceModifier.SelectedItem.Value) == 113 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 114 ||
                    int.Parse(cb_PriceModifier.SelectedItem.Value) == 115 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 116 ||
                    int.Parse(cb_PriceModifier.SelectedItem.Value) == 117)
                    {
                        dr_["CalculatedValue"] = float.Parse(txt_pricequantity.Text) * float.Parse(txt_Value.Text);
                    }
                    //else if (int.Parse(cb_PriceModifier.SelectedItem.Value) == 98 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 100)
                    //{
                    //    dr_["CalculatedValue"] = (float.Parse(txt_insuranceDeclaredValue.Text) * float.Parse(txt_Value.Text))/100;
                    //}
                    else
                    {
                        dr_["CalculatedValue"] = float.Parse(dr[0]["calculationValue"].ToString());// *(float.Parse(txt_TotalCharges.Text) / 100); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());
                    }

                    dr_["ModifiedCalculationValue"] = float.Parse(txt_Value.Text);//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                    dr_["CalculatedGst"] = 0;// ((float.Parse(dr[0]["calculationValue"].ToString()) * (float.Parse(txt_TotalCharges.Text) / 100)) / 100) * float.Parse(gst);
                    dr_["CalculationBase"] = dr[0]["calculationbase"].ToString();//int.Parse("1");
                    dr_["isTaxable"] = 1;
                    dr_["SortOrder"] = dt.Rows.Count + 1;
                    dr_["isGST"] = dr[0]["isGST"].ToString();
                    dr_["Pieces"] = float.Parse(txt_pricequantity.Text);

                    float declaredValue = 0f;
                    float.TryParse(txt_insuranceDeclaredValue.Text, out declaredValue);

                    dr_["AlternateValue"] = Math.Round(declaredValue, 2).ToString();
                    dt.Rows.Add(dr_);
                    dt.AcceptChanges();

                    ViewState["PM"] = dt;
                }
                else
                {
                    DataTable dt_ = (DataTable)ViewState["PM_"];
                    //   DataRow   dr = dt.NewRow()
                    dr = dt_.Select(" id ='" + cb_PriceModifier.SelectedValue + "'");

                    DataRow dr_ = dt.NewRow();

                    dr_["pricemodifierid"] = int.Parse(cb_PriceModifier.SelectedItem.Value);
                    dr_["ConsignmentNumber"] = Int64.Parse(txt_ConNo.Text);
                    dr_["CreatedBy"] = "TEST";
                    dr_["CreatedOn"] = DateTime.Now;
                    dr_["ModifiedBy"] = "";
                    dr_["ModifiedOn"] = DBNull.Value;
                    // dr_["CalculatedValue"] = float.Parse(dr[0]["calculationValue"].ToString());// +(float.Parse(txt_TotalCharges.Text)); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());

                    if (int.Parse(cb_PriceModifier.SelectedItem.Value) == 111 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 112 ||
                    int.Parse(cb_PriceModifier.SelectedItem.Value) == 113 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 114 ||
                    int.Parse(cb_PriceModifier.SelectedItem.Value) == 115 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 116 ||
                    int.Parse(cb_PriceModifier.SelectedItem.Value) == 117)
                    {
                        dr_["CalculatedValue"] = int.Parse(txt_pricequantity.Text) * int.Parse(txt_Value.Text);
                    }
                    //else if (int.Parse(cb_PriceModifier.SelectedItem.Value) == 98 || int.Parse(cb_PriceModifier.SelectedItem.Value) == 100)
                    //{
                    //    dr_["CalculatedValue"] = (float.Parse(txt_insuranceDeclaredValue.Text) * float.Parse(txt_Value.Text)) / 100;
                    //}
                    else
                    {
                        dr_["CalculatedValue"] = float.Parse(dr[0]["calculationValue"].ToString());// *(float.Parse(txt_TotalCharges.Text) / 100); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());
                    }

                    dr_["ModifiedCalculationValue"] = float.Parse(txt_Value.Text);//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                    dr_["CalculatedGst"] = 0;// ((float.Parse(dr[0]["calculationValue"].ToString()) + (float.Parse(txt_TotalCharges.Text) / 100)) / 100) * float.Parse(gst);
                    dr_["CalculationBase"] = dr[0]["calculationbase"].ToString();//int.Parse("1");
                    dr_["isTaxable"] = 1;
                    dr_["SortOrder"] = dt.Rows.Count + 1;
                    dr_["isGST"] = dr[0]["isGST"].ToString();
                    dr_["Pieces"] = float.Parse(txt_pricequantity.Text);

                    float declaredValue = 0f;
                    float.TryParse(txt_insuranceDeclaredValue.Text, out declaredValue);

                    dr_["AlternateValue"] = Math.Round(declaredValue, 2).ToString();
                    dt.Rows.Add(dr_);
                    dt.AcceptChanges();

                    ViewState["PM"] = dt;

                }
                LoadGrid();
                Total();

                cb_PriceModifier.ClearSelection();
                rb_Calculation.ClearSelection();
                txt_insuranceDeclaredValue.Text = "";
                txt_insuranceDeclaredValue.Enabled = false;
                txt_Value.Text = "";
            }

        }
        public void LoadGrid()
        {
            DataTable dt = (DataTable)ViewState["PM"];

            if (dt.Rows.Count > 0)
            {
                RadGrid1.DataSource = dt.DefaultView;
                RadGrid1.DataBind();

                txt_pricequantity.Text = "0";
                txt_pricequantity.Enabled = false;

                if (dt.Select("CalculationBase = '3'").Count() > 0)
                {
                    cb_Insurance.Checked = true;
                    cb_Insurance.Enabled = false;
                }
                else
                {
                    cb_Insurance.Checked = false;
                    cb_Insurance.Enabled = true;
                }

            }
            else
            {
                RadGrid1.DataSource = null;
                RadGrid1.DataBind();
                cb_Insurance.Checked = false;
                cb_Insurance.Enabled = true;
            }
            Total();
        }
        protected void cb_ServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_ServiceType.SelectedValue != "0")
            {
                // ServiceType_2();
                // Total();
            }
        }
        protected void RadGrid1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                string ID = ((HiddenField)e.Item.FindControl("Hd_ID")).Value;
                string Text = ((TextBox)e.Item.FindControl("txt_Value")).Text;
                string PricemodifierPieces = ((TextBox)e.Item.FindControl("txt_PricemodifierPieces")).Text;
                string ModifierValue = ((TextBox)e.Item.FindControl("txt_CalPriceModifierValue")).Text;



                clvar.origin = cb_Origin.SelectedValue;
                DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
                string gst = "";
                if (BranchGSTInformation.Tables[0].Rows.Count != 0)
                {
                    gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
                }
                DataTable dt = (DataTable)ViewState["PM"];
                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Select("priceModifierId='" + ID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                    double d = double.Parse(dr["CalculatedValue"].ToString());
                    if (dr != null)
                    {
                        if (txt_Value.Text == "")
                        {
                            txt_Value.Text = Text;
                        }
                        if (txt_TotalCharges.Text == "")
                        {
                            txt_TotalCharges.Text = "0";
                        }

                        if (ID == "111" || ID == "112" || ID == "113" || ID == "114" || ID == "115" || ID == "116" || ID == "117")
                        {
                            dr["CalculatedValue"] = float.Parse(PricemodifierPieces) * float.Parse(ModifierValue);
                        }
                        else
                        {
                            dr["CalculatedValue"] = float.Parse(Text) * ((float.Parse(txt_TotalCharges.Text) - d) / 100); //float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["ActualValue"].ToString());
                            dr["ModifiedCalculationValue"] = float.Parse(txt_Value.Text);//float.Parse(pm.Select("id = '" + dr["id"].ToString() + "'")[0]["CalculationValue"].ToString());
                            dr["CalculatedGst"] = ((float.Parse(Text)) / 100) * float.Parse(gst);
                        }
                    }
                    dt.AcceptChanges();
                    ViewState["PM"] = dt;
                }
                LoadGrid();
                Total();

            }
            if (e.CommandName == "Delete")
            {
                string ID = ((HiddenField)e.Item.FindControl("Hd_ID")).Value;

                DataTable dt = (DataTable)ViewState["PM"];

                DataRow dr = dt.Select("priceModifierId='" + ID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    dr.Delete(); //changes the Product_name
                }
                dt.AcceptChanges();
                ViewState["PM"] = dt;

                LoadGrid();
                Total();

            }
        }
        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        protected void txt_RiderCode_TextChanged(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.origin = cb_Origin.SelectedValue;
            if (txt_RiderCode.Text == txt_Weight.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Rider Code Cannot be Equal to Weight.')", true);
                return;
            }
            DataSet Rider = con.RiderInformation(clvar);
            if (Rider.Tables[0].Rows.Count != 0)
            {
                // hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                this.cb_originExpresscenter.SelectedValue = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                cb_ConType.Focus();
            }
            else
            {
                txt_RiderCode.Text = "";
                txt_RiderCode.Focus();
            }
        }
        protected void cb_Destination_ItemSelected(object sender, EventArgs e)
        {
            //if (cb_Destination.SelectedValue != "0")
            //{
            //    DataTable ds = (DataTable)ViewState["Ex_Info"];
            //    if (ds.Rows.Count != 0)
            //    {
            //        DataRow[] dr = ds.Select("expressCenterCode='" + cb_Destination.SelectedValue + "'");

            //        if (dr.Length != 0)
            //        {
            //            hd_Destination.Value = dr[0]["bid"].ToString();
            //            hd_DestinationZone.Value = dr[0]["ZoneCode"].ToString();


            //        }
            //    }
            //    else
            //    {
            //        cb_Destination.SelectedValue = "0";
            //        //       ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Service Type Again')", true);

            //    }
            //}
            //txt_Weight.Focus();
        }
        protected void RadGrid1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable dt = (DataTable)ViewState["PM_"];

                string PM = (e.Item.FindControl("Hd_ID") as HiddenField).Value;// cb_PriceModifier.SelectedValue;
                TextBox txtValue = e.Item.FindControl("txt_Value") as TextBox;
                TextBox Pieces = e.Item.FindControl("txt_PricemodifierPieces") as TextBox;
                TextBox CalPriceModifierValue = e.Item.FindControl("txt_CalPriceModifierValue") as TextBox;



                DataRow[] dr = dt.Select(" id ='" + PM + "'");
                if (dr.Length != 0)
                {
                    (e.Item.FindControl("lbl_PriceModifier") as Label).Text = dr[0]["PriceModifier"].ToString();
                    (e.Item.FindControl("lbl_Description") as Label).Text = dr[0]["description"].ToString();
                }
                if (txtValue.Text.Trim() == "" || txtValue.Text.Trim() == "0")
                {
                    txtValue.Enabled = true;
                }
                else
                {
                    txtValue.Enabled = false;
                    CalPriceModifierValue.Enabled = false;

                }

                if (PM == "111" || PM == "112" || PM == "113" || PM == "114" || PM == "115" || PM == "116" || PM == "117")
                {
                    Pieces.Enabled = false;
                    CalPriceModifierValue.Enabled = false;
                }
                else
                {
                    Pieces.Enabled = false;
                    CalPriceModifierValue.Enabled = false;
                }
            }
        }
        protected void dd_cities_selectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_city.SelectedValue != "0")
            {
                //Get_Destinations(); // Get_Branches();
            }
            //dd_city.Focus();
            //cb_Destination.Focus();
            txt_Weight.Focus();
        }
        protected void txt_Weight_TextChanged(object sender, EventArgs e)
        {
            //  ServiceType_2();
            //  Total();
            // cb_ServiceType.Focus();
            if (txt_Weight.Text == txt_RiderCode.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight Cannot be Equal to Rider Code.')", true);
                txt_Weight.Text = "";
                txt_Weight.Focus();
                return;
            }
            else
            {
                cb_ServiceType.Focus();
            }
            //clvar.RiderCode = txt_Weight.Text;
            //clvar.origin = cb_Origin.SelectedValue;

            //DataSet Rider = con.RiderInformation(clvar);
            //if (Rider.Tables[0].Rows.Count != 0)
            //{
            //    // hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
            //    txt_Weight.Text = "";
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight Cannot be Equal to Rider Code.')", true);
            //    return;
            //}
            //else
            //{

            //}

        }
        protected void txt_Consignee_TextChanged(object sender, EventArgs e)
        {
            // txt_ConsigneeCNIC.Focus();// txt_ConsigneeCellno.Focus();
        }
        protected void txt_ConsigneeCellno_TextChanged1(object sender, EventArgs e)
        {
            //txt_ConsigneeCNIC.Focus();
            if (txt_AccNo.Text.Trim(' ') == "" && txt_AccNo.Text.Trim(' ') != "0")
            {
                return;
            }
            if (txt_ConsigneeCellno.Text != "" && txt_ConsigneeCellno.Text != "0")
            {
                clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
                DataTable dt = con.GetConsigneeByCellNo(clvar);


                if (dt.Rows.Count > 0)
                {
                    txt_Consignee.Text = dt.Rows[0]["ClientName"].ToString();
                    txt_Address.Text = dt.Rows[0]["ClientAddress"].ToString();
                    txt_ConsigneeCNIC.Text = dt.Rows[0]["ClientCNIC"].ToString();
                    txt_Consignee.Focus();

                }
                else
                {
                    txt_Consignee.Text = ""; //dt.Rows[0]["ClientName"].ToString();
                    txt_Address.Text = ""; //dt.Rows[0]["ClientAddress"].ToString();
                    txt_ConsigneeCNIC.Text = ""; //dt.Rows[0]["ClientCNIC"].ToString();
                    txt_Consignee.Focus();
                }
            }
            else
            {
                txt_Consignee.Focus();
            }
        }
        protected void txt_ConsigneeCNIC_TextChanged(object sender, EventArgs e)
        {
            //  txt_ConsignerCellNo.Focus();
        }
        protected void txt_Consigner_TextChanged(object sender, EventArgs e)
        {
            // Txt_ConsignerCNIC.Focus();// txt_ConsignerCellNo.Focus();
        }
        protected void txt_ConsignerCellNo_TextChanged(object sender, EventArgs e)
        {
            if (txt_AccNo.Text.Trim(' ') == "0")
            {
                if (txt_ConsignerCellNo.Text != "" && txt_ConsignerCellNo.Text != "0")
                {
                    clvar.ConsignerCell = txt_ConsignerCellNo.Text;
                    DataTable dt = con.GetConsingerByCellNo(clvar);

                    if (dt.Rows.Count > 0)
                    {
                        txt_ShipperAddress.Text = dt.Rows[0]["ConsignerAddress"].ToString();
                        txt_Consigner.Text = dt.Rows[0]["ConsignerName"].ToString();
                        Txt_ConsignerCNIC.Text = dt.Rows[0]["ConsignerCNIC"].ToString();
                        //hd_CreditClientID.Value = dt.Rows[0]["ClientID"].ToString();
                        //  txt_Consigner.Focus();
                    }
                    else
                    {
                        txt_ShipperAddress.Text = "";
                        txt_Consigner.Text = ""; //dt.Rows[0]["ConsignerName"].ToString();
                        Txt_ConsignerCNIC.Text = ""; //dt.Rows[0]["ConsignerCNIC"].ToString();
                        txt_Consigner.Focus();
                    }
                }
                else
                {
                    txt_Consigner.Focus();
                }
            }
        }
        protected void Txt_ConsignerCNIC_TextChanged(object sender, EventArgs e)
        {
            // txt_Type.Focus();
        }
        protected void txt_Type_TextChanged(object sender, EventArgs e)
        {
            //cb_Insurance.Focus();
        }
        protected void txt_Package_Handcarry_TextChanged(object sender, EventArgs e)
        {
            // txt_Address.Focus();
        }
        protected void txt_Address_TextChanged(object sender, EventArgs e)
        {
            //  txt_ShipperAddress.Focus();
            //  Total();
        }
        protected void ResetAll()
        {
            txt_remarks.Text = "";
            txt_w.Text = txt_l.Text = txt_h.Text = "1";

            txt_insuranceDeclaredValue.Text = "";
            cb_PriceModifier.ClearSelection();
            RadGrid1.DataSource = null;
            RadGrid1.DataBind();
            rb_Calculation.ClearSelection();
            DataTable dt = ViewState["PM"] as DataTable;
            dt.Clear();

            if (!(cb_Account.Checked))
            {
                txt_AccNo.Text = "";
            }
            if (!(cb_Rider.Checked))
            {
                txt_RiderCode.Text = "";
            }
            // txt_AccNo.Text = "";
            txt_Address.Text = "";
            txt_CodAmount.Text = "";
            txt_ConNo.Text = "";
            txt_Consignee.Text = "";
            if (Session["BRANCHCODE"] != null)
            {
                cb_Origin.SelectedValue = Session["BRANCHCODE"].ToString();
            }
            txt_ConsigneeCellno.Text = "";
            txt_ConsigneeCNIC.Text = "";
            txt_Consigner.Text = "";
            txt_ConsignerCellNo.Text = "";
            Txt_ConsignerCNIC.Text = "";
            //txt_DeclaredValue.Text = "";
            txt_Description.Text = "";
            txt_Description2.Text = "";
            txt_Discount.Text = "";
            //txt_insurance.Text = "2.5";
            txt_LblFirstCon.Text = "";
            txt_LblLastConsignment.Text = "";
            txt_OrderRefNo.Text = "";
            txt_Othercharges.Text = "";
            txt_Package_Handcarry.Text = "";
            txt_Piecies.Text = "1";
            // txt_RiderCode.Text = "";
            txt_ShipperAddress.Text = "";
            txt_Type.Text = "";
            txt_Value.Text = "";
            txt_Weight.Text = "0.5";
            dd_city.SelectedValue = "0";
            dd_ProductType.SelectedValue = "0";
            cb_Account.Checked = false;
            cb_Rider.Checked = false;
            cb_ConType.SelectedValue = "12";
            //dd_city.Text = "";
            dd_city.SelectedValue = "0";
            //cb_Destination.Text = "";
            //cb_Destination.SelectedValue = "0";
            cb_ServiceType.SelectedValue = "overnight";
            txt_TotalAmount.Text = "";
            txt_Othercharges.Text = "";
            txt_TotalCharges.Text = "";
            cb_Insurance.Checked = false;
            cb_Insurance.Enabled = true;
        }
        protected void ResetAll_()
        {
            txt_AccNo.Text = "";
            txt_Address.Text = "";
            txt_CodAmount.Text = "";
            //      txt_ConNo.Text = "";
            txt_Consignee.Text = "";
            txt_ConsigneeCellno.Text = "";
            txt_ConsigneeCNIC.Text = "";
            txt_Consigner.Text = "";
            txt_ConsignerCellNo.Text = "";
            Txt_ConsignerCNIC.Text = "";
            //txt_DeclaredValue.Text = "";
            txt_Description.Text = "";
            txt_Description2.Text = "";
            txt_Discount.Text = "";
            //txt_insurance.Text = "2.5";
            txt_LblFirstCon.Text = "";
            txt_LblLastConsignment.Text = "";
            txt_OrderRefNo.Text = "";
            txt_Othercharges.Text = "";
            txt_Package_Handcarry.Text = "";
            txt_Piecies.Text = "1";
            txt_Preprinted.Text = "";
            txt_RiderCode.Text = "";
            txt_ShipperAddress.Text = "";
            txt_SmsConsignment.Text = "";
            txt_Type.Text = "";
            txt_Value.Text = "";
            txt_Weight.Text = "0.5";
            dd_city.SelectedValue = "0";
            dd_ProductType.SelectedValue = "0";
            cb_Account.Checked = false;
            cb_Rider.Checked = false;
            cb_ConType.SelectedValue = "12";
            //cb_Destination.SelectedValue = "0";
            cb_ServiceType.SelectedValue = "overnight";
            txt_TotalAmount.Text = "";
            txt_Othercharges.Text = "";
            cb_Insurance.Checked = false;
        }
        protected void ResetAll_2()
        {
            txt_TotalCharges.Text = "";

            if (cb_Account.Checked == true)
            {
            }
            else
            {
                txt_AccNo.Text = "";
            }

            txt_Address.Text = "";
            txt_CodAmount.Text = "";
            txt_Consignee.Text = "";
            txt_ConsigneeCellno.Text = "";
            txt_ConsigneeCNIC.Text = "";
            txt_Consigner.Text = "";
            txt_ConsignerCellNo.Text = "";
            Txt_ConsignerCNIC.Text = "";
            //txt_DeclaredValue.Text = "";
            txt_Description.Text = "";
            txt_Description2.Text = "";
            txt_Discount.Text = "";
            //txt_insurance.Text = "2.5";
            txt_OrderRefNo.Text = "";
            txt_Othercharges.Text = "";
            txt_Package_Handcarry.Text = "";
            txt_Piecies.Text = "1";

            if (cb_Rider.Checked == true)
            {
            }
            else
            {
                txt_RiderCode.Text = "";
            }

            txt_ShipperAddress.Text = "";
            txt_Type.Text = "";
            txt_Value.Text = "";
            txt_Weight.Text = "0.5";
            dd_city.SelectedValue = "0";
            dd_city.Text = "";
            //cb_Destination.Text = "";
            //cb_Destination.SelectedValue = "0";
            dd_ProductType.SelectedValue = "0";
            cb_Origin.SelectedValue = Session["BRANCHCODE"].ToString();
            cb_Account.Checked = false;
            cb_Rider.Checked = false;
            cb_ConType.SelectedValue = "12";
            //        cb_Destination.SelectedValue = "0";
            cb_ServiceType.SelectedValue = "overnight";
            txt_TotalAmount.Text = "";
            txt_Othercharges.Text = "";
            cb_Insurance.Checked = false;
            cb_Insurance.Enabled = true;
            DataTable dt = ViewState["PM"] as DataTable;
            dd_PaymentMode.SelectedValue = "1";
            txt_TransactionID.Text = "0";
            dt.Clear();
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            if (cb_RangeSelection.Checked == true)
            {
                string consignment = txt_ConNo.Text;
                ArrayList Arlist = (ArrayList)ViewState["Series_SeriesConsign"];
                if (Arlist.Count > 0)
                {
                    for (int i = 0; i < Arlist.Count; i++)
                    {
                        if (consignment == Arlist[i].ToString())
                        {
                            Arlist.RemoveAt(i);
                            txt_ConNo.Text = Arlist[0].ToString();
                            ResetAll_2();
                        }
                    }
                    ViewState["Series_SeriesConsign"] = Arlist;
                }
                else
                {
                    Response.Redirect("Consignment.aspx");
                }
            }
            else
            {
                ResetAll();

            }
            // ResetAll();
        }
        protected void cb_ServiceType_PreRender(object sender, EventArgs e)
        {

        }
        public void Total()
        {
            //double DV = 0;

            //clvar.origin = cb_Origin.SelectedValue;
            //DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            //string gst = "";
            //if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            //{
            //    gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            //}

            //double Modfier = 0;
            //DataTable dt = (DataTable)ViewState["PM"];
            //foreach (DataRow rp in dt.Rows)
            //{
            //    //  TextBox tx = ((TextBox)rp.FindControl("txt_Value"));
            //    Modfier += double.Parse(rp["CalculatedValue"].ToString());
            //}

            //if (cb_ServiceType.SelectedValue != "Road n Rail")
            //{
            //    DataTable dr = ViewState["AdditionalFac"] as DataTable;
            //    DataTable dt_1 = (DataTable)ViewState["TariffInfo"];
            //    double maxWeight = 0;
            //    DataRow[] dr_ = dt_1.Select("ServiceID='" + cb_ServiceType.SelectedValue + "'");

            //    if (dt_1 != null)
            //    {
            //        maxWeight = double.Parse(dr_.CopyToDataTable().Compute("MAX(TOWEIGHT)", "").ToString());
            //    }

            //    double actualWeight = double.Parse(txt_Weight.Text);
            //    if (double.Parse(txt_Weight.Text) <= maxWeight)
            //    {
            //        DataRow[] dr2 = null;
            //        if (dt_1 != null)
            //        {
            //            if (dt_1.Rows.Count != 0)
            //            {
            //                dr2 = dt_1.Select("ToWeight >= '" + double.Parse(txt_Weight.Text) + "' AND FROMWEIGHT < '" + double.Parse(txt_Weight.Text) + "' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
            //                if (dr2.Length != 0)
            //                {
            //                    DV = double.Parse(dr2[0]["Price"].ToString());
            //                }
            //            }
            //            else
            //            {
            //                //error no tarrif found
            //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Avaiable.')", true);
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            //error no tarrif found
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Avaiable.')", true);
            //            return;
            //        }
            //    }

            //    else if (actualWeight > maxWeight)
            //    {
            //        DataRow[] dr2 = null;
            //        if (dt_1 != null)
            //        {
            //            if (dt_1.Rows.Count != 0)
            //            {
            //                dr2 = dt_1.Select("ToWeight_='" + maxWeight.ToString() + "' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
            //                if (dr2.Length != 0)
            //                {
            //                    DV = double.Parse(dr2[0]["Price"].ToString());
            //                }
            //            }
            //            else
            //            {
            //                //error no tarrif available
            //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Avaiable.')", true);
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            //error no tarrif found
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tariff Not Avaiable.')", true);
            //            return;
            //        }
            //        if (dr != null)
            //        {
            //            if (dr.Rows.Count == 0)
            //            {
            //                //error no additional factor available
            //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Additional Factor.')", true);
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            //error no additional factor available
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Additional Factor.')", true);
            //            return;
            //        }
            //        double remainingWeight = actualWeight - maxWeight;
            //        double weightFactor = double.Parse(dr.Rows[0]["additionalFactor"].ToString());
            //        double price = double.Parse(dr.Rows[0]["Price"].ToString());
            //        double value = Math.Ceiling(remainingWeight / weightFactor) * price;
            //        DV += value;

            //    }
            //}
            //else
            //{
            //    DataTable dt_1 = (DataTable)ViewState["RNRTarif"];
            //    DataRow[] dr2 = null;
            //    if (dt_1 != null)
            //    {
            //        if (dt_1.Rows.Count != 0)
            //        {
            //            DataRow[] dr_originec = ((DataTable)ViewState["Ex_origin"]).Select("expresscentercode ='" + cb_originExpresscenter.SelectedValue + "'");
            //            DataRow[] dr_Destec = ((DataTable)ViewState["Ex_Info"]).Select("expresscentercode ='" + this.cb_Destination.SelectedValue + "'");

            //            dr2 = dt_1.Select("FromCatID='" + dr_originec[0]["CategoryID"].ToString() + "' and ToCatID='" + dr_Destec[0]["CategoryID"].ToString() + "'");

            //            if (dr2.Length > 0)
            //            {
            //                Double w = float.Parse(txt_Weight.Text);
            //                if (w < 5)
            //                {
            //                    DV = 5 * float.Parse(dr2[0]["value"].ToString());
            //                }
            //                else
            //                {
            //                    DV = Math.Ceiling(float.Parse(txt_Weight.Text)) * float.Parse(dr2[0]["value"].ToString());
            //                }
            //            }
            //        }
            //    }
            //}

            ////Calculating InsuranceAmount
            //double insper = 0;
            //if (txt_DeclaredValue.Text != "0" && txt_DeclaredValue.Text != "")
            //{
            //    insper = (double.Parse(txt_DeclaredValue.Text) / 100) * double.Parse(this.txt_insurance.Text);
            //}

            //// Calculating Net Total
            //if (hd_priceModifier.Value == "")
            //{
            //    hd_priceModifier.Value = "0";
            //}
            //double TotalAmount = (DV + insper);

            //// Calculating GST
            //double gst_ = ((TotalAmount) / 100) * double.Parse(gst);

            //clvar.TotalAmount = TotalAmount;
            //clvar.gst = gst_;
            //clvar.ChargeAmount = 0;

            //if (TotalAmount != 0)
            //{
            //    txt_TotalAmount.Value = TotalAmount + gst_ + Modfier;
            //    txt_Othercharges.Value = gst_;
            //    txt_TotalCharges.Value = TotalAmount + Modfier;
            //}
            //else
            //{
            //    txt_TotalAmount.Value = 0;
            //    txt_Othercharges.Value = 0;
            //    txt_TotalCharges.Value = 0;
            //}
        }
        protected void txt_Piecies_TextChanged(object sender, EventArgs e)
        {
            //Total();

            this.txt_ConsigneeCellno.Focus();
        }
        protected void txt_ConNo_TextChanged1(object sender, EventArgs e)
        {

        }
        protected void txt_DeclaredValue_TextChanged(object sender, EventArgs e)
        {
            // ServiceType_2();
            //  Total();
            //  txt_Package_Handcarry.Focus();
        }
        protected void cb_Destination_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cb_Destination.SelectedValue != "0")
            //{
            //    DataTable ds = (DataTable)ViewState["Ex_Info"];
            //    if (ds.Rows.Count != 0)
            //    {
            //        DataRow[] dr = ds.Select("expressCenterCode='" + cb_Destination.SelectedValue + "'");

            //        if (dr.Length != 0)
            //        {
            //            hd_Destination.Value = dr[0]["bid"].ToString();
            //            hd_DestinationZone.Value = dr[0]["ZoneCode"].ToString();


            //        }
            //    }
            //    else
            //    {
            //        cb_Destination.SelectedValue = "0";
            //        //       ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Service Type Again')", true);

            //    }
            //}

            //// ServiceType_2();

            ////  txt_Weight.Text = "";
            //txt_Weight.Focus();
        }
        protected void cb_ServiceType_SelectedIndexChanged1(object sender, DropDownListEventArgs e)
        {
            //txt_ConsigneeCellno.Focus();
        }
        public void ServiceType_()
        {
            clvar = new Cl_Variables();


            if (cb_ServiceType.SelectedValue != "Road n Rail")
            {
                DataRow[] dr_3 = ((DataSet)ViewState["ClientTarifInformationLocal"]).Tables[0].Select("ServiceID='" + cb_ServiceType.SelectedValue + "'");

                DataSet ds = (DataSet)ViewState["ClientTarifInformation_"];//con.ClientTarifInformation(clvar);
                if (dr_3.Length != 0)
                {
                    try
                    {
                        ViewState["TariffInfo_"] = ds.Tables[0];
                        ViewState["TariffInfo"] = ViewState["TariffInfo_"];

                        DataTable dt_ = (DataTable)ViewState["ClientTarifAddtionalFactor"];//CF.ClientTarifAddtionalFactor(clvar);

                        if (cb_Origin.SelectedValue == hd_Destination.Value)
                        {
                            ViewState["TariffInfo"] = null;
                            ViewState["TariffInfo"] = ((DataSet)ViewState["ClientTarifInformationLocal"]).Tables[0];
                            DataRow[] dr = dt_.Select("BranchCode ='" + hd_Destination.Value + "' and ToZoneCode='LOCAL' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
                            if (dr.Length > 0)
                            {
                                ViewState["AdditionalFac"] = dr.CopyToDataTable();
                            }
                            else
                            {
                                ViewState["AdditionalFac"] = null;
                            }
                        }
                        else if (hd_DestinationZone.Value == clvar.Zone)
                        {
                            DataRow[] dr = dt_.Select("FromZoneCode ='" + hd_DestinationZone.Value + "' and ToZoneCode='SAME' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
                            if (dr.Length > 0)
                            {
                                ViewState["AdditionalFac"] = dr.CopyToDataTable();
                            }
                            else
                            {
                                ViewState["AdditionalFac"] = null;
                            }
                        }
                        else
                        {
                            DataRow[] dr = dt_.Select("ToZoneCode='DIFF' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
                            if (dr.Length > 0)
                            {
                                ViewState["AdditionalFac"] = dr.CopyToDataTable();
                            }
                            else
                            {
                                ViewState["AdditionalFac"] = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Available')", true);
                        //return;
                    }
                    this.txt_ConsigneeCellno.Focus();
                }
                else
                {
                    clvar.AccountNo = "0"; //txt_AccNo.Text;

                    ds = (DataSet)ViewState["ClientTarifInformation_1"];//con.ClientTarifInformation(clvar);

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        clvar.AccountNo = hd_CreditClientID.Value;

                        ViewState["TariffInfo_"] = ds.Tables[0];
                        ViewState["TariffInfo"] = ViewState["TariffInfo_"];

                        DataTable dt_ = (DataTable)ViewState["ClientTarifAddtionalFactor_1"];//CF.ClientTarifAddtionalFactor(clvar);
                        try
                        {
                            if (cb_Origin.SelectedValue == hd_Destination.Value)
                            {
                                ViewState["TariffInfo"] = null;
                                clvar.AccountNo = "0";
                                ViewState["TariffInfo"] = ds.Tables[0];//((DataSet)ViewState["ClientTarifInformationLocal_1"]).Tables[0];

                                DataRow[] dr = dt_.Select("BranchCode ='" + hd_Destination.Value + "' and ToZoneCode='LOCAL' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
                                if (dr.Length > 0)
                                {
                                    ViewState["AdditionalFac"] = dr.CopyToDataTable();
                                }
                                else
                                {
                                    ViewState["AdditionalFac"] = null;
                                }
                            }
                            else if (hd_DestinationZone.Value == clvar.Zone)
                            {
                                DataRow[] dr = dt_.Select("FromZoneCode ='" + hd_DestinationZone.Value + "' and ToZoneCode='SAME' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
                                if (dr.Length > 0)
                                {
                                    ViewState["AdditionalFac"] = dr.CopyToDataTable();
                                }
                                else
                                {
                                    ViewState["AdditionalFac"] = null;
                                }
                            }
                            else
                            {
                                DataRow[] dr = dt_.Select("ToZoneCode='DIFF' and ServiceID='" + cb_ServiceType.SelectedValue + "'");
                                if (dr.Length > 0)
                                {
                                    ViewState["AdditionalFac"] = dr.CopyToDataTable();
                                }
                                else
                                {
                                    ViewState["AdditionalFac"] = null;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Available')", true);
                            //return;
                        }
                        this.txt_ConsigneeCellno.Focus();
                    }
                    else
                    {

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Service Type Again and Tarif Doesnot exist')", true);
                        cb_ServiceType.SelectedValue = "0";
                        cb_ServiceType.Focus();

                    }
                }
            }
            else
            {
                clvar.CustomerClientID = hd_CreditClientID.Value;
                clvar.Destination = hd_Destination.Value; //dr[0]["bid"].ToString();

                if (CF.RNRTarrif(clvar).Rows.Count != 0)
                {
                    ViewState["RNRTarif"] = CF.RNRTarrif(clvar);

                    this.txt_ConsigneeCellno.Focus();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Service Type Again and Tarif Doesnot exist')", true);
                    cb_ServiceType.SelectedValue = "0";
                    cb_ServiceType.Focus();
                }
            }

        }
        public void ServiceType_2()
        {
            clvar = new Cl_Variables();
            clvar.Zone = Session["ZONECODE"].ToString();
            clvar.origin = cb_Origin.SelectedValue;
            clvar.Destination = hd_Destination_Ec.Value;
            clvar.AccountNo = hd_CreditClientID.Value; //txt_AccNo.Text;
            clvar.ServiceType = cb_ServiceType.SelectedValue;
            if (cb_ServiceType.SelectedValue != "Road n Rail")
            {
                DataSet ds = con.ClientTarifInformation(clvar);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    try
                    {
                        ViewState["TariffInfo_"] = ds.Tables[0];
                        ViewState["TariffInfo"] = ViewState["TariffInfo_"];

                        DataTable dt_ = CF.ClientTarifAddtionalFactor(clvar);

                        if (cb_Origin.SelectedValue == hd_Destination.Value)
                        {
                            ViewState["TariffInfo"] = null;
                            ViewState["TariffInfo"] = con.ClientTarifInformationLocal(clvar).Tables[0];
                            DataRow[] dr = dt_.Select("BranchCode ='" + hd_Destination.Value + "' and ToZoneCode='LOCAL'");
                            if (dr.Length > 0)
                            {
                                ViewState["AdditionalFac"] = dr.CopyToDataTable();
                            }
                            else
                            {
                                ViewState["AdditionalFac"] = null;
                            }
                        }
                        else if (hd_DestinationZone.Value == clvar.Zone)
                        {
                            DataRow[] dr = dt_.Select("FromZoneCode ='" + hd_DestinationZone.Value + "' and ToZoneCode='SAME'");
                            if (dr.Length > 0)
                            {
                                ViewState["AdditionalFac"] = dr.CopyToDataTable();
                            }
                            else
                            {
                                ViewState["AdditionalFac"] = null;
                            }
                        }
                        else
                        {
                            DataRow[] dr = dt_.Select("ToZoneCode='DIFF'");
                            if (dr.Length > 0)
                            {
                                ViewState["AdditionalFac"] = dr.CopyToDataTable();
                            }
                            else
                            {
                                ViewState["AdditionalFac"] = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Available')", true);
                        //return;
                    }
                    txt_Consignee.Focus();
                }
                else
                {
                    clvar.AccountNo = "0"; //txt_AccNo.Text;

                    ds = con.ClientTarifInformation(clvar);

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        clvar.AccountNo = "0";
                        ViewState["TariffInfo"] = ds.Tables[0];
                        DataTable dt_ = CF.ClientTarifAddtionalFactor(clvar);
                        try
                        {

                            if (cb_Origin.SelectedValue == hd_Destination.Value)
                            {
                                ViewState["TariffInfo"] = null;
                                clvar.AccountNo = "0";
                                ViewState["TariffInfo"] = con.ClientTarifInformationLocal(clvar).Tables[0];

                                DataRow[] dr = dt_.Select("BranchCode ='" + hd_Destination.Value + "' and ToZoneCode='LOCAL'");
                                if (dr.Length > 0)
                                {
                                    ViewState["AdditionalFac"] = dr.CopyToDataTable();
                                }
                                else
                                {
                                    ViewState["AdditionalFac"] = null;
                                }
                            }
                            else if (hd_DestinationZone.Value == clvar.Zone)
                            {
                                DataRow[] dr = dt_.Select("FromZoneCode ='" + hd_DestinationZone.Value + "' and ToZoneCode='SAME'");
                                if (dr.Length > 0)
                                {
                                    ViewState["AdditionalFac"] = dr.CopyToDataTable();
                                }
                                else
                                {
                                    ViewState["AdditionalFac"] = null;
                                }
                            }
                            else
                            {
                                DataRow[] dr = dt_.Select("ToZoneCode='DIFF'");
                                if (dr.Length > 0)
                                {
                                    ViewState["AdditionalFac"] = dr.CopyToDataTable();
                                }
                                else
                                {
                                    ViewState["AdditionalFac"] = null;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Additional Factor Not Available')", true);
                            //return;
                        }
                        //    txt_Consignee.Focus();
                    }
                    else
                    {

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tarif Doesnot exist')", true);
                        cb_ServiceType.SelectedValue = "0";
                        cb_ServiceType.Focus();

                    }
                }
            }
            else
            {
                clvar.CustomerClientID = hd_CreditClientID.Value;
                clvar.Destination = hd_Destination.Value; //dr[0]["bid"].ToString();

                if (CF.RNRTarrif(clvar).Rows.Count != 0)
                {
                    ViewState["RNRTarif"] = CF.RNRTarrif(clvar);


                    txt_Consignee.Focus();

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Service Type Again and Tarif Doesnot exist')", true);
                    cb_ServiceType.SelectedValue = "0";
                    cb_ServiceType.Focus();
                }
            }

            btn_SaveConsignment1.Enabled = true;
        }
        protected void cb_ServiceType_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btn_PrintConsignment_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Files/invoicepdf.aspx?id" + txt_ConNo.Text);
        }
        protected void Rb_CustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void Post_BrandedSMS(string mobile, string resp, string Consignee, string destination)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                //   WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.

                // Create POST data and convert it to a byte array.
                if (txt_ConNo.Text != string.Empty)
                {
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, A shipment CN " + resp + " is booked successfully. You can visit www.mulphilog.com or call us on 111-202-202 to track delivery status. Thank you";
                    newResp = "Thank you for choosing M&P Courier. Your shipment with CN #: " + resp + " is booked successfully. Track it at www.mulphilog.com or call 021 111 202 202.";
                    //newResp = newResp.Replace("&", "%26");
                    // newResp = newResp.Replace("#", "%23");
                    string resp_ = "Dear Valued Customer, We have received your shipment under CN:" + resp + "for " + Consignee + " - " + destination + " Amount :" + string.Format("{0:N0}", Double.Parse(txt_TotalAmount.Text)) + ". Please visit www.mulphilog.com or call us on 021-111-202-202 to track delivery status. Thank you";


                    string smsContent = newResp;//"Dear Customer, M&P Courier visited your address to deliver " + hd_consigner.Value + " shipment under CN #" + row.Cells[1].Text + " but it could not deliver. Pls contact M&P Customer Services 111-202-202";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber,smsformtype) \n" +
                             "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A','3')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                        sqlcmd.CommandType = CommandType.Text;

                        count = sqlcmd.ExecuteNonQuery();
                        //IsUnique = Int32.Parse(SParam.Value.ToString());
                        // obj.XCode = obj.consignmentNo;
                        sqlcon.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                }
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }
        public void Post_BrandedSMS_(string mobile, string resp, string Consignee, string destination)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                //   WebRequest request = WebRequest.Create("https://services.converget.com/MTEntryDyn/?");
                // Set the Method property of the request to POST.

                // Create POST data and convert it to a byte array.
                if (txt_ConNo.Text != string.Empty)
                {
                    //  DataTable dt = clvar.GetMaxOrderNo();
                    string newResp = "Dear Customer, Your shipment CN " + resp + " is received on " + DateTime.Now.Date.ToString("yyyy-MM-dd") + ". Thank You. For further details, contact us on 021-111-202-202";
                    newResp = "Dear Customer, M&P Courier just received a shipment with CN #: " + resp + " for delivery to you. Track it at www.mulphilog.com or call 021 111 202 202. ";
                    // newResp = newResp.Replace("&", "%26");
                    // newResp = newResp.Replace("#", "%23");
                    string resp_ = "Dear Customer, A shipment has been booked under CN:" + resp + "for " + Consignee + " - " + destination + ".You can visit www.mulphilog.com or call us on 021-111 202 202 to track delivery status. Thank you";

                    string smsContent = newResp;//"Dear Customer, M&P Courier visited your address to deliver " + hd_consigner.Value + " shipment under CN #" + row.Cells[1].Text + " but it could not deliver. Pls contact M&P Customer Services 111-202-202";
                    string query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber) \n" +
                             "values ('" + resp + "', '" + mobile + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', 'N/A')";

                    int count = 0;
                    string error = "";
                    SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                        sqlcmd.CommandType = CommandType.Text;

                        count = sqlcmd.ExecuteNonQuery();
                        //IsUnique = Int32.Parse(SParam.Value.ToString());
                        // obj.XCode = obj.consignmentNo;
                        sqlcon.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }


                }
            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }

        }
        protected void cb_RangeSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_RangeSelection.Checked == true)
            {
                this.txt_ConNo.Text = "";
                this.txt_ConNo.Enabled = false;

                txt_LblFirstCon.Enabled = true;
                txt_LblFirstCon.Text = "0";
                txt_LblLastConsignment.Enabled = true;
                txt_LblLastConsignment.Text = "0";
                this.rb_CustomerEntry.Enabled = true;

            }
            else
            {
                this.txt_ConNo.Text = "";
                this.txt_ConNo.Enabled = true;

                txt_LblFirstCon.Enabled = false;
                txt_LblFirstCon.Text = "0";
                txt_LblLastConsignment.Enabled = false;
                txt_LblLastConsignment.Text = "0";

                this.rb_CustomerEntry.Enabled = false;
            }
        }
        ArrayList SeriesConsignment = new ArrayList();
        protected void rb_CustomerEntry_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (rb_CustomerEntry.SelectedValue == "2")
            {
                string Difference = (Int64.Parse(txt_LblLastConsignment.Text.ToString()) - Int64.Parse(txt_LblFirstCon.Text.ToString())).ToString();

                Int64 First = Int64.Parse(txt_LblFirstCon.Text.ToString());
                for (int i = 0; i <= int.Parse(Difference); i++)
                {
                    SeriesConsignment.Add(First.ToString());
                    First = First + 1;
                }

                ViewState["Series_SeriesConsign"] = SeriesConsignment;
                txt_ConNo.Text = txt_LblFirstCon.Text.ToString();
            }
        }
        protected void cb_ServiceType_SelectedIndexChanged2(object sender, EventArgs e)
        {

        }
        protected void txt_ShipperAddress_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btn_SaveConsignment1_Click(object sender, EventArgs e)
        {
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
            clvar.CheckCondition = " AND mzc.Product = (select products from serviceTypes_new where serviceTypeName = '" + cb_ServiceType.SelectedValue + "')";
            clvar.consignmentNo = txt_ConNo.Text.Trim();

            DataTable dt_discount_ = ViewState["Discount"] as DataTable;
            if (ViewState["Discount"] != null)
            {
                if (dt_discount_.Rows[0]["IsSpecial"].ToString() == "1")
                {
                    if (txt_ConsignerCellNo.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Discount which you has selected please Enter Consigner CellNo')", true);
                        return;
                    }
                    if (txt_ConsignerCellNo.Text.Trim() != dt_discount_.Rows[0]["SpecialId"].ToString())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consigner CellNo for selected Discount')", true);
                        return;
                    }
                    if (float.Parse(dt_discount_.Rows[0]["MinWeight"].ToString()) > float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be below then " + dt_discount_.Rows[0]["MinWeight"].ToString() + "')", true);
                        return;
                    }

                    if (float.Parse(dt_discount_.Rows[0]["MaxWeight"].ToString()) < float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be more then " + dt_discount_.Rows[0]["MaxWeight"].ToString() + "')", true);
                        return;
                    }

                    if (dt_discount_.Rows[0]["ServiceType"].ToString().ToUpper() != cb_ServiceType.SelectedValue.ToUpper())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This discount is avaliable for " + dt_discount_.Rows[0]["ServiceType"].ToString() + " Service')", true);
                        return;
                    }
                }
                else
                {
                    if (float.Parse(dt_discount_.Rows[0]["MinWeight"].ToString()) > float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be below then " + dt_discount_.Rows[0]["MinWeight"].ToString() + "')", true);
                        return;
                    }

                    if (float.Parse(dt_discount_.Rows[0]["MaxWeight"].ToString()) < float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be more then " + dt_discount_.Rows[0]["MaxWeight"].ToString() + "')", true);
                        return;
                    }

                    if (dt_discount_.Rows[0]["ServiceType"].ToString().ToUpper() != cb_ServiceType.SelectedValue.ToUpper())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This discount is avaliable for " + dt_discount_.Rows[0]["ServiceType"].ToString() + " Service')", true);
                        return;
                    }
                }
            }

            Tuple<bool, string> CNCheck = ConsignmentCheck();
            if (CNCheck.Item1 == false)
            {
                Alert(CNCheck.Item2.ToString());
                btn_SaveConsignment0.Enabled = false;
                return;
            }

            Tuple<bool, string> AccNoCheck = AccountNumberCheck();
            if (AccNoCheck.Item1 == false)
            {
                Alert(AccNoCheck.Item2.ToString());
                btn_SaveConsignment0.Enabled = false;
                return;
            }

            Tuple<bool, string> RiderCheck = RiderCodeCheck();
            if (RiderCheck.Item1 == false)
            {
                Alert(RiderCheck.Item2.ToString());
                btn_SaveConsignment0.Enabled = false;
                return;
            }

            Tuple<bool, string> EcCheck = GetECBySeq(txt_ConNo.Text.Trim(), txt_AccNo.Text.Trim(), txt_RiderCode.Text.Trim());
            if (EcCheck.Item1 == false)
            {
                Alert(EcCheck.Item2.ToString());
                btn_SaveConsignment0.Enabled = false;
                return;
            }

            lbl_Error.Text = "";
            try
            {
                DataTable cities = ViewState["cities"] as DataTable;
                clvar = new Cl_Variables();
                clvar.consignmentNo = txt_ConNo.Text;
                clvar.AccountNo = txt_AccNo.Text;
                clvar.RiderCode = txt_RiderCode.Text;
                clvar.Destination = dd_city.SelectedValue;// hd_Destination.Value;
                clvar.ServiceTypeName = cb_ServiceType.SelectedValue;
                clvar.ServiceType = cb_ServiceType.SelectedValue;
                if (txt_Weight.Text.Trim() == "0" || txt_Weight.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be zero or Empty')", true);
                    return;
                }
                clvar.Weight = float.Parse(txt_Weight.Text);
                clvar.Unit = 1;// cb_Unit.SelectedValue;
                if (txt_Piecies.Text.Trim() == "0" || txt_Piecies.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pieces cannot be zero or Empty')", true);
                    return;
                }
                clvar.pieces = int.Parse(txt_Piecies.Text);// txt_Piecies.Text;
                clvar.Zone = Session["ZONECODE"].ToString();
                clvar.PakageContents = txt_Package_Handcarry.Text;

                //Consignee and Consigner Information
                clvar.Consignee = txt_Consignee.Text.ToUpper();
                clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
                clvar.ConsigneeCNIC = txt_ConsigneeCNIC.Text;
                clvar.ConsigneeAddress = txt_Address.Text.ToUpper();
                clvar.Consigner = txt_Consigner.Text.ToUpper();
                clvar.ConsignerCell = txt_ConsignerCellNo.Text;
                clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
                clvar.ConsignerAddress = txt_ShipperAddress.Text.ToUpper();
                clvar.consignerAccountNo = txt_AccNo.Text;
                try
                {
                    clvar.Bookingdate = DateTime.Now; // DateTime.Parse(Session["WorkingDate"].ToString());//DateTime.Parse(dt_Picker.SelectedDate.ToString());
                }
                catch (Exception ez)
                {
                    Response.Redirect("~/login");
                }

                clvar.origin = cb_Origin.SelectedValue;
                //clvar.Insurance = txt_insurance.Text;
                clvar.Othercharges = 0;// txt_Othercharges.Text;
                clvar.Day = rb_1.SelectedValue;
                clvar.expresscenter = Session["ExpressCenter"].ToString();//cb_originExpresscenter.SelectedValue;//hd_OriginExpressCenter.Value;
                clvar.destinationExpressCenterCode = cities.Select("BranchCode = '" + dd_city.SelectedValue + "'")[0]["ECCODE"].ToString();// hd_Destination_Ec.Value;// cb_Destination.SelectedValue;
                clvar.CustomerClientID = hd_CreditClientID.Value;

                if (cb_ConType.SelectedValue == "12")
                {
                    clvar.Con_Type = int.Parse(cb_ConType.SelectedValue);
                    clvar.CouponNo = txt_Type.Text;
                }
                else if (cb_ConType.SelectedValue == "13")
                {
                    clvar.Con_Type = int.Parse(cb_ConType.SelectedValue);
                    clvar.CouponNo = txt_Type.Text;
                }
                else if (cb_ConType.SelectedValue == "14")
                {
                    clvar.Con_Type = int.Parse(cb_ConType.SelectedValue);
                    clvar.CouponNo = txt_Type.Text;
                }
                //Branch Information
                DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
                string gst = "";
                if (BranchGSTInformation.Tables[0].Rows.Count != 0)
                {
                    gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
                }
                //Calculating Modifier
                double Modfier = 0;

                foreach (RepeaterItem rp in RadGrid1.Items)
                {
                    Label PriceModifierName = ((Label)rp.FindControl("lbl_PriceModifier"));
                    TextBox tx = ((TextBox)rp.FindControl("txt_Value"));
                    Modfier += double.Parse(tx.Text);
                }

                //Now Getting Tarrif
                double DV = 0;

                if (cb_Insurance.Checked == true)
                {
                    clvar.isInsured = true;
                }
                double insper = 0;
                //if (txt_DeclaredValue.Text != "" && txt_DeclaredValue.Text == "0")
                //{
                //    clvar.Declaredvalue = float.Parse(txt_DeclaredValue.Text);
                //    insper = (double.Parse(txt_DeclaredValue.Text) / 100) * double.Parse(this.txt_insurance.Text);
                //}
                // Calculating Net Total
                if (hd_priceModifier.Value == "")
                {
                    hd_priceModifier.Value = "0";
                }
                double TotalAmount = (DV + 0);

                // Calculating GST
                double gst_ = ((TotalAmount) / 100) * double.Parse(gst);


                clvar.TotalAmount = TotalAmount;
                clvar.gst = gst_;
                clvar.ChargeAmount = 0;
                DataTable dt = (DataTable)ViewState["PM"];
                if (dt.Rows.Count != 0)
                {
                    clvar.isPM = true;
                    clvar.LstModifiersCNt = dt;
                }
                else
                {
                    clvar.isPM = false;
                    clvar.LstModifiersCNt = null;
                }
                clvar.destinationCountryCode = "PAK";

                //COD
                if (cb_COD.Checked == true)
                {
                    clvar.isCod = cb_COD.Checked;
                    if (ViewState["CODType"].ToString() == "1")
                    {
                        clvar.codAmount = float.Parse(txt_CodAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_CodAmount.Text);
                    }
                    else if (ViewState["CODType"].ToString() == "2")
                    {
                        clvar.codAmount = float.Parse(txt_TotalAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_TotalAmount.Text);
                    }
                    else if (ViewState["CODType"].ToString() == "3")
                    {
                        clvar.codAmount = float.Parse(txt_TotalAmount.Text) + float.Parse(txt_CodAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_TotalAmount.Text) + float.Parse(txt_CodAmount.Text);
                    }

                    clvar.chargeCODAmount = Cb_CODAmount.Checked;
                    clvar.productDescription = txt_Description.Text;
                    if (dd_ProductType.Items.Count > 0)
                    {
                        clvar.productTypeId = int.Parse(dd_ProductType.SelectedValue);
                    }
                    else
                    {
                        clvar.productTypeId = 0;
                    }
                    clvar.orderRefNo = txt_OrderRefNo.Text;
                }

                if (Rb_CustomerType.SelectedValue.ToUpper() == "CREDIT")
                {
                    clvar.Customertype = 2;
                }
                else
                {
                    clvar.Customertype = 1;
                }
                if (Session["U_ID"] != null)
                {
                    clvar.createdBy = Session["U_ID"].ToString();
                }

                clvar.breadth = double.Parse(txt_l.Text);
                clvar.width = double.Parse(txt_w.Text);
                clvar.height = double.Parse(txt_h.Text);

                discountId_Fixed_TextChanged(sender, e);

                if (limitover == "limitover")
                {
                    return;
                }

                string Error = con.Add_Consignment_Validation(clvar);
                DataTable dt_ = new DataTable();

                if (cb_ServiceType.SelectedValue != "Road n Rail" && cb_ServiceType.SelectedValue != "Express Cargo")
                {
                    dt_ = con.Add_OcsValidation(clvar);

                    txt_TotalAmount.Text = Math.Round((double.Parse(dt_.Rows[0]["Amount"].ToString()) + double.Parse(dt_.Rows[0]["gst"].ToString())), 2).ToString();
                    //clvar.gst = gst_;
                    txt_Othercharges.Text = Math.Round((double.Parse(dt_.Rows[0]["gst"].ToString())), 2).ToString();
                    txt_TotalCharges.Text = Math.Round((double.Parse(dt_.Rows[0]["amount"].ToString())), 2).ToString();
                    //  clvar.ChargeAmount = 0;
                    hd_amount.Value = txt_TotalCharges.Text;
                    hd_gst.Value = txt_Othercharges.Text;


                    DataTable modifier = ViewState["PM_"] as DataTable;

                    double totalAmount_ = double.Parse(dt_.Rows[0]["Amount"].ToString());
                    double totalGst = double.Parse(dt_.Rows[0]["gst"].ToString());
                    double totalPriceModifierCharges = 0;
                    double totalPriceModifierCharges_ = 0;

                    foreach (RepeaterItem item in RadGrid1.Items)
                    {
                        string pID = (item.FindControl("Hd_ID") as HiddenField).Value;
                        string pBase = (item.FindControl("hd_base") as HiddenField).Value;
                        string calculationValue = (item.FindControl("txt_Value") as TextBox).Text;
                        string isGST = (item.FindControl("hd_gstExempt") as HiddenField).Value;

                        if (pBase == "1")
                        {
                            //totalAmount_ += double.Parse(calculationValue);
                            // totalPriceModifierCharges += double.Parse(calculationValue);
                            if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                            {
                                totalGst += (double.Parse(gst) / 100) * double.Parse(calculationValue);
                            }
                            else
                            {
                                //double a = double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                                //totalGst += a * (double.Parse(gst) / 100);

                                totalPriceModifierCharges += double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                                double a = double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                                totalGst += a * (double.Parse(gst) / 100);
                            }

                            //totalAmount_ += double.Parse(calculationValue);
                        }
                        else if (pBase == "2")
                        {
                            double tempTotal = totalAmount_;
                            double calculatedValue = totalAmount_ * (double.Parse(calculationValue) / 100);
                            totalPriceModifierCharges += totalAmount_ * (double.Parse(calculationValue) / 100);
                            if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                            {
                                totalGst += (double.Parse(gst) / 100) * calculatedValue;
                            }
                            //totalAmount_ += calculatedValue;
                        }
                        else if (pBase == "3")
                        {
                            /*
                            double tempTotal = totalAmount_;
                            double declaredValue = 0;
                            double insurancePercentage = 0;

                            double.TryParse((item.FindControl("lbl_gDeclaredValue") as Label).Text, out declaredValue);
                            double.TryParse((item.FindControl("txt_Value") as TextBox).Text, out insurancePercentage);

                            totalPriceModifierCharges += declaredValue * (insurancePercentage / 100);
                            double calculatedValue = declaredValue + (insurancePercentage / 100);
                            if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                            {
                                totalGst += (double.Parse(gst) / 100) * totalPriceModifierCharges;
                            }
                            */


                            // Bilal Modify on 15-08-2020
                            double tempTotal = totalAmount_;
                            double declaredValue = 0;
                            double insurancePercentage = 0;

                            double.TryParse((item.FindControl("lbl_gDeclaredValue") as Label).Text, out declaredValue);
                            double.TryParse((item.FindControl("txt_Value") as TextBox).Text, out insurancePercentage);

                            totalPriceModifierCharges_ = declaredValue * (insurancePercentage / 100);
                            double calculatedValue = declaredValue + (insurancePercentage / 100);
                            if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                            {
                                totalGst += (double.Parse(gst) / 100) * totalPriceModifierCharges_;
                            }
                        }

                    }
                    //txt_TotalCharges.Text = String.Format("{0:N2}", totalAmount_ + totalPriceModifierCharges);
                    //txt_Othercharges.Text = String.Format("{0:N2}", totalGst);
                    //txt_TotalAmount.Text = String.Format("{0:N2}", totalAmount_ + totalPriceModifierCharges + totalGst);

                    if (cb_ServiceType.SelectedValue == "Mango Fiesta 5 Kg")
                    {
                        txt_TotalCharges.Text = "1499.00";
                        txt_Othercharges.Text = "0.00";
                        txt_TotalAmount.Text = "1499.00";
                    }
                    else if (cb_ServiceType.SelectedValue == "Mango Fiesta 7 Kg")
                    {
                        txt_TotalCharges.Text = "1999.00";
                        txt_Othercharges.Text = "0.00";
                        txt_TotalAmount.Text = "1999.00";
                    }
                    else if (cb_ServiceType.SelectedValue == "Mango Fiesta 10 Kg")
                    {
                        txt_TotalCharges.Text = "2699.00";
                        txt_Othercharges.Text = "0.00";
                        txt_TotalAmount.Text = "2699.00";
                    }
                    else
                    {
                        // Bilal Modify on 15-08-2020
                        txt_TotalCharges.Text = String.Format("{0:N2}", totalAmount_ + totalPriceModifierCharges + totalPriceModifierCharges_);
                        txt_Othercharges.Text = String.Format("{0:N2}", totalGst);
                        txt_TotalAmount.Text = String.Format("{0:N2}", totalAmount_ + totalPriceModifierCharges + totalPriceModifierCharges_ + totalGst);
                    }

                    hd_TotalAmount.Value = txt_TotalCharges.Text;
                    hd_gst.Value = txt_Othercharges.Text;
                    hd_chargeamount.Value = txt_TotalAmount.Text;
                }
                else
                {
                    dt_ = con.Add_RNRValidation(clvar);

                    txt_TotalAmount.Text = Math.Round((double.Parse(dt_.Rows[0]["totalamount"].ToString()) + double.Parse(dt_.Rows[0]["gst"].ToString())), 2).ToString();
                    //clvar.gst = gst_;
                    txt_Othercharges.Text = Math.Round((double.Parse(dt_.Rows[0]["gst"].ToString())), 2).ToString();
                    txt_TotalCharges.Text = Math.Round((double.Parse(dt_.Rows[0]["totalamount"].ToString())), 2).ToString();

                    //hd_amount.Value = txt_TotalCharges.Text;
                    //hd_gst.Value = txt_Othercharges.Text;

                    //hd_chargeamount.Value = txt_TotalAmount.Text;

                    hd_amount.Value = txt_TotalCharges.Text;
                    hd_gst.Value = txt_Othercharges.Text;

                    hd_TotalAmount.Value = txt_TotalCharges.Text;
                    hd_gst.Value = txt_Othercharges.Text;
                    hd_chargeamount.Value = txt_TotalAmount.Text;

                    DataTable modifier = ViewState["PM_"] as DataTable;

                    double totalAmount_ = double.Parse(dt_.Rows[0]["totalamount"].ToString());
                    double totalGst = double.Parse(dt_.Rows[0]["gst"].ToString());
                    double totalPriceModifierCharges = 0;
                    double totalPriceModifierCharges_ = 0;

                    foreach (RepeaterItem item in RadGrid1.Items)
                    {
                        string pID = (item.FindControl("Hd_ID") as HiddenField).Value;
                        string pBase = (item.FindControl("hd_base") as HiddenField).Value;
                        string calculationValue = (item.FindControl("txt_Value") as TextBox).Text;
                        string isGST = (item.FindControl("hd_gstExempt") as HiddenField).Value;

                        if (pBase == "1")
                        {
                            if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                            {
                                totalGst += (double.Parse(gst) / 100) * double.Parse(calculationValue);
                            }
                            else
                            {
                                totalPriceModifierCharges += double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                                double a = double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                                totalGst += a * (double.Parse(gst) / 100);
                            }
                        }
                        else if (pBase == "2")
                        {
                            double tempTotal = totalAmount_;
                            double calculatedValue = totalAmount_ * (double.Parse(calculationValue) / 100);
                            totalPriceModifierCharges += totalAmount_ * (double.Parse(calculationValue) / 100);
                            if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                            {
                                totalGst += (double.Parse(gst) / 100) * calculatedValue;
                            }
                        }
                        else if (pBase == "3")
                        {
                            // Bilal Modify on 15-08-2020
                            double tempTotal = totalAmount_;
                            double declaredValue = 0;
                            double insurancePercentage = 0;

                            double.TryParse((item.FindControl("lbl_gDeclaredValue") as Label).Text, out declaredValue);
                            double.TryParse((item.FindControl("txt_Value") as TextBox).Text, out insurancePercentage);

                            totalPriceModifierCharges_ = declaredValue * (insurancePercentage / 100);
                            double calculatedValue = declaredValue + (insurancePercentage / 100);
                            if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                            {
                                totalGst += (double.Parse(gst) / 100) * totalPriceModifierCharges_;
                            }
                        }
                    }

                    // Bilal Modify on 15-08-2020
                    txt_TotalCharges.Text = String.Format("{0:N2}", totalAmount_ + totalPriceModifierCharges + totalPriceModifierCharges_);
                    txt_Othercharges.Text = String.Format("{0:N2}", totalGst);
                    txt_TotalAmount.Text = String.Format("{0:N2}", totalAmount_ + totalPriceModifierCharges + totalPriceModifierCharges_ + totalGst);

                    hd_TotalAmount.Value = txt_TotalCharges.Text;
                    hd_gst.Value = txt_Othercharges.Text;
                    hd_chargeamount.Value = txt_TotalAmount.Text;
                }

                // FOR DHA QUTTA FORM
                if (txt_AccNo.Text == "280605")
                {
                    txt_TotalCharges.Text = "0";
                    txt_Othercharges.Text = "0";
                    txt_TotalAmount.Text = "0";
                }


                //Confirmation Message  
                btn_SaveConsignment0.Enabled = true;

                return;

            }
            catch (Exception Err)
            {
                lbl_Error.Text = Err.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Save Consignment. Error: " + Err.Message + "')", true);
            }
            finally
            {

            }


        }
        protected void btn_SaveConsignment_Click(object sender, EventArgs e)
        {
            #region Validations

            discountId_Fixed_TextChanged(sender, e);

            if (limitover == "limitover")
            {
                return;
            }

            if (txt_AccNo.Text.Trim() == "")
            {
                Alert("Enter Account Number");
                return;
            }
            if (txt_ConNo.Text.Trim() == "")
            {
                Alert("Enter Consignment Number");
                return;
            }
            if (txt_RiderCode.Text.Trim() == "")
            {
                Alert("Enter Rider Code");
                return;
            }

            if (txt_Weight.Text.Trim() == "")
            {
                Alert("Enter Weight");
                return;
            }
            if (txt_Piecies.Text.Trim() == "" || txt_Piecies.Text.Trim('0') == "")
            {
                Alert("Pieces cannot be empty or 0");
                return;
            }

            float tempAweight = 0f;
            float tempWeight = 0f;
            float.TryParse(txt_aWeight.Text, out tempAweight);
            float.TryParse(txt_Weight.Text, out tempWeight);

            if (tempWeight < 0.1f)
            {
                Alert("Invalid Weight");
                return;
            }
            #endregion

            DataTable dt_discount_ = ViewState["Discount"] as DataTable;
            if (ViewState["Discount"] != null)
            {
                if (dt_discount_.Rows[0]["IsSpecial"].ToString() == "1")
                {
                    if (txt_ConsignerCellNo.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Discount which you has selected please Enter Consigner CellNo')", true);
                        return;
                    }
                    if (txt_ConsignerCellNo.Text.Trim() != dt_discount_.Rows[0]["SpecialId"].ToString())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consigner CellNo for selected Discount')", true);
                        return;
                    }
                    if (float.Parse(dt_discount_.Rows[0]["MinWeight"].ToString()) > float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be below then " + dt_discount_.Rows[0]["MinWeight"].ToString() + "')", true);
                        return;
                    }

                    if (float.Parse(dt_discount_.Rows[0]["MaxWeight"].ToString()) < float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be more then " + dt_discount_.Rows[0]["MaxWeight"].ToString() + "')", true);
                        return;
                    }

                    if (dt_discount_.Rows[0]["ServiceType"].ToString().ToUpper() != cb_ServiceType.SelectedValue.ToUpper())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This discount is avaliable for " + dt_discount_.Rows[0]["ServiceType"].ToString() + " Service')", true);
                        return;
                    }
                }
                else
                {
                    if (float.Parse(dt_discount_.Rows[0]["MinWeight"].ToString()) > float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be below then " + dt_discount_.Rows[0]["MinWeight"].ToString() + "')", true);
                        return;
                    }

                    if (float.Parse(dt_discount_.Rows[0]["MaxWeight"].ToString()) < float.Parse(txt_Weight.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Weight cannot be more then " + dt_discount_.Rows[0]["MaxWeight"].ToString() + "')", true);
                        return;
                    }

                    if (dt_discount_.Rows[0]["ServiceType"].ToString().ToUpper() != cb_ServiceType.SelectedValue.ToUpper())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This discount is avaliable for " + dt_discount_.Rows[0]["ServiceType"].ToString() + " Service')", true);
                        return;
                    }
                }
            }

            lbl_Error.Text = "";
            try
            {
                if (dd_PaymentMode.SelectedValue.ToString() == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Payment Mode')", true);
                    return;
                }

                clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();

                clvar.consignmentNo = txt_ConNo.Text.Trim();
                //DataTable seq = SequenceCheck(clvar, " AND mzc.Product = (select products from serviceTypes_new where serviceTypeName = '" + cb_ServiceType.SelectedValue + "')");
                DataTable seq = SequenceCheck(clvar, " AND P.masterproduct = (select products from serviceTypes_new where serviceTypeName = '" + cb_ServiceType.SelectedValue + "')");
                if (seq.Rows.Count <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number cannot be used for selected Service')", true);
                    return;
                }

                DataTable cities = ViewState["cities"] as DataTable;
                clvar = new Cl_Variables();
                clvar.consignmentNo = txt_ConNo.Text;
                clvar.AccountNo = txt_AccNo.Text;
                clvar.RiderCode = txt_RiderCode.Text;
                clvar.Destination = dd_city.SelectedValue;//hd_Destination.Value;
                clvar.ServiceTypeName = cb_ServiceType.SelectedValue;
                clvar.ServiceType = cb_ServiceType.SelectedValue;
                clvar.Weight = (float)Math.Round(float.Parse(txt_Weight.Text), 1);
                clvar.Unit = 1;// cb_Unit.SelectedValue;
                clvar.pieces = int.Parse(txt_Piecies.Text);// txt_Piecies.Text;
                clvar.Zone = Session["ZONECODE"].ToString();
                clvar.PakageContents = txt_Package_Handcarry.Text;

                //Consignee and Consigner Information
                clvar.Consignee = txt_Consignee.Text.ToUpper();
                clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
                clvar.ConsigneeCNIC = txt_ConsigneeCNIC.Text;
                clvar.ConsigneeAddress = txt_Address.Text.ToUpper();
                clvar.Consigner = txt_Consigner.Text.ToUpper();
                clvar.ConsignerCell = txt_ConsignerCellNo.Text;
                clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
                clvar.ConsignerAddress = txt_ShipperAddress.Text.ToUpper();
                clvar.consignerAccountNo = txt_AccNo.Text;

                string qry1 = "insert into customer_directory (name, contact, address, nic, addedon, isactive) \n" +
                             "values ('" + txt_Consignee.Text.ToUpper() + "', '" + txt_ConsigneeCellno.Text + "', '" + txt_Address.Text.ToUpper() + "', '" + txt_ConsigneeCNIC.Text + "', GETDATE(), '1')";

                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                try
                {
                    sqlcon.Open();
                    SqlCommand sqlcmd = new SqlCommand(qry1, sqlcon);
                    sqlcmd.CommandType = CommandType.Text;

                    sqlcmd.ExecuteNonQuery();
                    sqlcon.Close();
                }
                catch (Exception ex)
                {

                }

                string qry2 = "insert into customer_directory (name, contact, address, nic, addedon, isactive) \n" +
                             "values ('" + txt_Consigner.Text.ToUpper() + "', '" + txt_ConsignerCellNo.Text + "', '" + txt_ShipperAddress.Text.ToUpper() + "', '" + Txt_ConsignerCNIC.Text + "', GETDATE(), '1')";
                try
                {
                    sqlcon.Open();
                    SqlCommand sqlcmd = new SqlCommand(qry2, sqlcon);
                    sqlcmd.CommandType = CommandType.Text;

                    sqlcmd.ExecuteNonQuery();
                    sqlcon.Close();
                }
                catch (Exception ex)
                {

                }

                try
                {
                    clvar.Bookingdate = DateTime.Now;// DateTime.Parse(DateTime.Now());//DateTime.Parse(dt_Picker.SelectedDate.ToString());
                }
                catch (Exception ez)
                {
                    Response.Redirect("~/login");
                }

                clvar.origin = cb_Origin.SelectedValue;
                //clvar.Insurance = txt_insurance.Text;
                clvar.Othercharges = 0;// txt_Othercharges.Text;
                clvar.Day = rb_1.SelectedValue;
                clvar.OriginExpressCenterCode = cb_originExpresscenter.SelectedValue;
                clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString(); //cb_originExpresscenter.SelectedValue;//hd_OriginExpressCenter.Value;
                clvar.destinationExpressCenterCode = cities.Select("BranchCode = '" + dd_city.SelectedValue + "'")[0]["ECCode"].ToString();// hd_Destination_Ec.Value;// cb_Destination.SelectedValue;
                clvar.CustomerClientID = hd_CreditClientID.Value;

                if (cb_ConType.SelectedValue == "12")
                {
                    clvar.Con_Type = int.Parse(cb_ConType.SelectedValue);
                    clvar.CouponNo = txt_Type.Text;
                }
                else if (cb_ConType.SelectedValue == "13")
                {
                    clvar.Con_Type = int.Parse(cb_ConType.SelectedValue);
                    clvar.CouponNo = txt_Type.Text;
                }
                else if (cb_ConType.SelectedValue == "14")
                {
                    clvar.Con_Type = int.Parse(cb_ConType.SelectedValue);
                    clvar.CouponNo = txt_Type.Text;
                }
                //Branch Information
                DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
                string gst = "";
                if (BranchGSTInformation.Tables[0].Rows.Count != 0)
                {
                    gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
                }
                //Calculating Modifier
                double Modfier = 0;

                foreach (RepeaterItem rp in RadGrid1.Items)
                {
                    TextBox tx = ((TextBox)rp.FindControl("txt_Value"));
                    Modfier += double.Parse(tx.Text);
                }

                //Now Getting Tarrif
                double DV = 0;

                //Calculating InsuranceAmount
                if (cb_Insurance.Checked == true)
                {
                    clvar.isInsured = true;
                }
                double insper = 0;

                // Calculating Net Total
                if (hd_priceModifier.Value == "")
                {
                    hd_priceModifier.Value = "0";
                }
                double TotalAmount = double.Parse(txt_TotalCharges.Text);// (DV + 0);

                // Calculating GST
                double gst_ = double.Parse(txt_Othercharges.Text);

                if (TotalAmount == 0)
                {
                    if (txt_AccNo.Text != "280605")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. Please Contact IT Support')", true);
                        return;
                    }
                }
                clvar.TotalAmount = TotalAmount;
                clvar.gst = gst_;
                clvar.ChargeAmount = 0;
                DataTable dt = (DataTable)ViewState["PM"];
                if (dt.Rows.Count != 0)
                {
                    clvar.isPM = true;
                    clvar.LstModifiersCNt = dt;
                }
                else
                {
                    clvar.isPM = false;
                    clvar.LstModifiersCNt = null;
                }
                clvar.destinationCountryCode = "PAK";

                //COD
                if (cb_COD.Checked == true)
                {
                    clvar.isCod = cb_COD.Checked;

                    if (ViewState["CODType"].ToString() == "1")
                    {
                        if (txt_CodAmount.Text == "0" || txt_CodAmount.Text == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Amount')", true);
                            return;
                        }
                        clvar.codAmount = float.Parse(txt_CodAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_CodAmount.Text);
                    }
                    else if (ViewState["CODType"].ToString() == "2")
                    {
                        if (txt_TotalAmount.Text == "0" || txt_TotalAmount.Text == null)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Amount')", true);
                            return;
                        }
                        clvar.codAmount = float.Parse(txt_TotalAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_TotalAmount.Text);
                    }
                    else if (ViewState["CODType"].ToString() == "3")
                    {
                        if ((txt_CodAmount.Text == "0" || txt_CodAmount.Text == null) && (txt_TotalAmount.Text == "0" || txt_TotalAmount.Text == null))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Amount')", true);
                            return;
                        }
                        clvar.codAmount = float.Parse(txt_TotalAmount.Text) + float.Parse(txt_CodAmount.Text);
                        clvar.calculatedCodAmount = float.Parse(txt_TotalAmount.Text) + float.Parse(txt_CodAmount.Text);
                    }

                    clvar.chargeCODAmount = Cb_CODAmount.Checked;
                    clvar.productDescription = txt_Description.Text;
                    if (dd_ProductType.Items.Count > 0)
                    {
                        clvar.productTypeId = int.Parse(dd_ProductType.SelectedValue);
                    }
                    else
                    {
                        clvar.productTypeId = 0;
                    }
                    clvar.orderRefNo = txt_OrderRefNo.Text;
                }


                //Confirmation Message

                //   String Message = "Are you sure you want to save the consignment.\n Gst : " + String.Format("{0:N0}", gst) + ".\n Total Amount : " + String.Format("{0:N0}", TotalAmount) +".";

                //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "confirm('" + Message + "');", true);
                //   return;
                //if (Rb_CustomerType.SelectedItem.Text.ToUpper() == "CREDIT")
                if (txt_AccNo.Text != "0")
                {
                    clvar.Customertype = 2;
                }
                else
                {
                    clvar.Customertype = 1;
                }
                if (Session["U_ID"] != null)
                {
                    clvar.createdBy = Session["U_ID"].ToString();
                }
                double tempDimesion = 0;
                double.TryParse(txt_l.Text, out tempDimesion);
                clvar.breadth = tempDimesion;

                tempDimesion = 0;
                double.TryParse(txt_w.Text, out tempDimesion);
                clvar.width = tempDimesion;

                tempDimesion = 0;
                double.TryParse(txt_h.Text, out tempDimesion);
                clvar.height = tempDimesion;

                float tempDimesionf = 0;
                float.TryParse(txt_aWeight.Text, out tempDimesionf);
                float denseWeight = tempDimesionf;

                clvar.PaymentMode = dd_PaymentMode.SelectedValue;
                clvar.PaymentTransactionID = txt_TransactionID.Text;
                clvar.NoteType = dd_discount.SelectedValue.ToString();

                if (cb_ServiceType.SelectedValue == "Mango Fiesta 5 Kg")
                {
                    clvar.TotalAmount = 1499.00;
                    clvar.gst = 0;
                    clvar.ChargeAmount = 1499.00;
                }
                else if (cb_ServiceType.SelectedValue == "Mango Fiesta 7 Kg")
                {
                    clvar.TotalAmount = 1999.00;
                    clvar.gst = 0;
                    clvar.ChargeAmount = 1999.00;
                }
                else if (cb_ServiceType.SelectedValue == "Mango Fiesta 10 Kg")
                {
                    clvar.TotalAmount = 2699.00;
                    clvar.gst = 0;
                    clvar.ChargeAmount = 2699.00;
                }
                else
                {
                    clvar.TotalAmount = double.Parse(hd_amount.Value);
                    clvar.gst = double.Parse(hd_gst.Value);
                    clvar.ChargeAmount = 0;
                }

                //string getAutoDiscountNumber;

                if (txt_discountId.Text != "")
                {
                    clvar.NoteNumber = txt_discountId.Text.Trim();
                }
                else
                //if (dd_discount.SelectedValue.ToString() != "0")
                {
                    DataTable dt_dn = Get_DiscountNumber();
                    if (dt_dn.Rows.Count > 0)
                    {
                        clvar.NoteNumber = dt_dn.Rows[0]["DiscountNumber"].ToString();

                        //string bilal = dd_discount.SelectedValue.ToString();
                        //return;
                    }
                }

                if (Request.QueryString["TicketNumber"] != null)
                {
                    clvar.CouponNo = Request.QueryString["TicketNumber"].ToString();
                }

                string error;
                if (dd_discount.SelectedValue == "0")
                {
                    clvar.ChargeAmount = double.Parse(hd_chargeamount.Value);
                    clvar.TotalAmount = double.Parse(hd_TotalAmount.Value);
                    clvar.gst = double.Parse(hd_gst.Value);

                    // Noraml Booking without Discount
                    error = Add_Consignment(clvar, denseWeight, txt_remarks.Text);
                }
                else
                {
                    clvar.ChargeAmount = double.Parse(hd_chargeamount.Value);
                    clvar.TotalAmount = double.Parse(hd_TotalAmount.Value);
                    clvar.gst = double.Parse(hd_gst.Value);

                    // Discount Booking
                    error = Add_Consignment_Discount(clvar, denseWeight, txt_remarks.Text);
                    if (error == "")
                    {
                        if (txt_discountId.Text == "")
                        {
                            Add_EC_IncrementNumber(clvar);
                            lbl_Error.Text = "Your Discount Id: " + clvar.NoteNumber;
                        }
                    }
                }

                //string error = Add_Consignment(clvar, denseWeight, txt_remarks.Text);
                if (error != "")
                {
                    lbl_Error.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Save Consignment.')", true);
                    return;
                }
                DataTable dtc = (DataTable)ViewState["PM"];
                error = "";
                dtc.Clear();
                double totalAmount_ = clvar.TotalAmount;
                double totalGst = clvar.gst;
                foreach (RepeaterItem item in RadGrid1.Items)
                {
                    DataRow dtcRow = dtc.NewRow();
                    string pID = (item.FindControl("Hd_ID") as HiddenField).Value;
                    string pBase = (item.FindControl("hd_base") as HiddenField).Value;
                    string calculationValue;

                    calculationValue = (item.FindControl("txt_Value") as TextBox).Text;
                    string isGST = (item.FindControl("hd_gstExempt") as HiddenField).Value;
                    double calculatedValue = 0;
                    double calculatedGST = 0;
                    if (pBase == "1")
                    {
                        //totalAmount_ += double.Parse(calculationValue);
                        // calculatedValue = double.Parse(calculationValue);
                        if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                        {
                            //totalGst += (double.Parse(gst) / 100) * double.Parse(calculationValue);
                            calculatedGST = (double.Parse(gst) / 100) * calculatedValue;
                        }
                        else
                        {
                            // double a = double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                            // calculatedGST = a * (double.Parse(gst) / 100);


                            calculatedValue += double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                            double a = double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                            calculatedGST += a * (double.Parse(gst) / 100);
                        }
                    }
                    else if (pBase == "2")
                    {
                        double tempTotal = totalAmount_;
                        calculatedValue = totalAmount_ * (double.Parse(calculationValue) / 100);
                        if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                        {
                            totalGst += (double.Parse(gst) / 100) * calculatedValue;
                            calculatedGST = (double.Parse(gst) / 100) * calculatedValue;
                        }
                        //totalAmount_ += calculatedValue;
                    }
                    if (pBase == "3")
                    {
                        double tempTotal = totalAmount_;
                        double declaredValue = 0;
                        double insurancePercentage = 0;

                        double.TryParse((item.FindControl("lbl_gDeclaredValue") as Label).Text, out declaredValue);
                        double.TryParse((item.FindControl("txt_Value") as TextBox).Text, out insurancePercentage);

                        //totalPriceModifierCharges += declaredValue * (insurancePercentage / 100);
                        calculatedValue = declaredValue * (insurancePercentage / 100);
                        if (isGST.ToString() == "1" || isGST.ToString().ToUpper() == "TRUE")
                        {
                            calculatedGST = (double.Parse(gst) / 100) * calculatedValue;
                        }
                        else
                        {
                            double a = double.Parse(calculationValue) / ((double.Parse(gst) + 100) / 100);
                            totalGst += a * (double.Parse(gst) / 100);
                        }
                    }

                    dtcRow["PriceModifierID"] = pID.ToString();
                    dtcRow["consignmentNumber"] = txt_ConNo.Text;
                    dtcRow["CreatedBy"] = HttpContext.Current.Session["U_ID"].ToString();
                    dtcRow["CreatedON"] = DateTime.Now.ToString();

                    if (pID == "111" || pID == "112" || pID == "113" || pID == "114" || pID == "115" || pID == "116" || pID == "117")
                    {
                        dtcRow["ModifiedCalculationValue"] = (item.FindControl("txt_CalPriceModifierValue") as TextBox).Text;
                    }
                    else
                    {
                        dtcRow["ModifiedCalculationValue"] = calculationValue;
                    }

                    //dtcRow["ModifiedCalculationValue"] = calculationValue;
                    dtcRow["CalculatedValue"] = calculatedValue;
                    dtcRow["CalculatedGST"] = calculatedGST;
                    dtcRow["CalculationBase"] = pBase;
                    dtcRow["IsTaxable"] = isGST;
                    dtcRow["SortOrder"] = item.ItemIndex;
                    dtcRow["AlternateValue"] = (item.FindControl("lbl_gDeclaredValue") as Label).Text;
                    dtcRow["Pieces"] = (item.FindControl("txt_PricemodifierPieces") as TextBox).Text;
                    dtc.Rows.Add(dtcRow);
                }

                error = Add_ConsignmentModifier(dtc, clvar);

                if (error == "")
                {
                    if (dd_discount.SelectedValue == "0")
                    {
                        clvar.ChargeAmount = double.Parse(hd_chargeamount.Value);
                        clvar.TotalAmount = double.Parse(hd_TotalAmount.Value);
                        clvar.gst = double.Parse(hd_gst.Value);

                        string u_qty = @"UPDATE CONSIGNMENT 
                                     SET totalAmount = '" + clvar.TotalAmount + @"', 
                                     gst = '" + clvar.gst + @"', 
                                     chargedAmount = '" + clvar.ChargeAmount + @"', 
                                     isPriceComputed ='1' 
                                     WHERE CONSIGNMENTNUMBER = '" + txt_ConNo.Text + @"'";
                        try
                        {
                            sqlcon.Open();
                            SqlCommand sqlcmd = new SqlCommand(u_qty, sqlcon);
                            sqlcmd.CommandType = CommandType.Text;

                            sqlcmd.ExecuteNonQuery();
                            sqlcon.Close();
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    Update_CSTableAgainstCN(clvar);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Has Been Saved.')", true);

                    // IF CN WAS NOT DISCOUNT
                    if (dd_discount.SelectedValue == "0")
                    {
                        Post_BrandedSMS(txt_ConsignerCellNo.Text, txt_ConNo.Text, txt_Consigner.Text.ToUpper(), dd_city.SelectedItem.Text);

                        if (txt_SmsConsignment.Checked == true)
                        {
                            Post_BrandedSMS_(txt_ConsigneeCellno.Text, txt_ConNo.Text, txt_Consignee.Text.ToUpper(), dd_city.Text);
                        }

                        #region FOR PRINT CN

                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                        string ecnryptedConsignment = EnryptString(txt_ConNo.Text);
                        string script = String.Format(script_, "RetailBookingReceipt.aspx?id=" + ecnryptedConsignment, "_blank", "");

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                        #endregion
                    }

                    btn_reset_Click(sender, e);

                    if (Session["BookingUserStatus"].ToString() == "1")
                    {
                        txt_RiderCode.Text = Session["BookingStaff"].ToString();
                        txt_AccNo.Text = "0";
                        txt_AccNo.Enabled = false;
                    }

                    discountId_Fixed_TextChanged(sender, e);

                    #region OLD_CODE_16 May 2021

                    /*
                    if (dd_discount.SelectedValue == "0")
                    {
                        if (dd_discount.SelectedValue == "0")
                        {
                            clvar.ChargeAmount = double.Parse(hd_chargeamount.Value);

                            string u_qty = "UPDATE CONSIGNMENT SET chargedAmount = '" + clvar.ChargeAmount + "' WHERE CONSIGNMENTNUMBER = '" + txt_ConNo.Text + "'";
                            try
                            {
                                sqlcon.Open();
                                SqlCommand sqlcmd = new SqlCommand(u_qty, sqlcon);
                                sqlcmd.CommandType = CommandType.Text;

                                sqlcmd.ExecuteNonQuery();
                                sqlcon.Close();
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        Post_BrandedSMS(txt_ConsignerCellNo.Text, txt_ConNo.Text, txt_Consigner.Text.ToUpper(), dd_city.SelectedItem.Text);
                        if (txt_SmsConsignment.Checked == true)
                        {
                            Post_BrandedSMS_(txt_ConsigneeCellno.Text, txt_ConNo.Text, txt_Consignee.Text.ToUpper(), dd_city.Text);
                        }

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Has Been Saved.')", true);
                        //   return;

                        Update_CSTableAgainstCN(clvar);

                        #region FOR PRINT CN

                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                        string ecnryptedConsignment = EnryptString(txt_ConNo.Text);
                        string script = String.Format(script_, "RetailBookingReceipt.aspx?id=" + ecnryptedConsignment, "_blank", "");

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                        #endregion
                        btn_reset_Click(sender, e);
                    }
                    else
                    {
                        Update_CSTableAgainstCN(clvar);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Has Been Saved.')", true);

                        btn_reset_Click(sender, e);

                        if (Session["BookingUserStatus"].ToString() == "1")
                        {
                            txt_RiderCode.Text = Session["BookingStaff"].ToString();
                            txt_AccNo.Text = "0";
                            txt_AccNo.Enabled = false;
                        }

                        discountId_Fixed_TextChanged(sender, e);
                    }
                    */

                    #endregion
                }
                else
                {
                    lbl_Error.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support. Error: " + error + "')", true);
                    return;
                }

                if (clvar.Error != "")
                {
                    lbl_Error.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Some Error Occured')", true);
                    return;
                }
                else
                {
                    // Response.Redirect("SearchConsignment.aspx?Xcode=" + clvar.consignmentNo + "");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved')", true);
                }



                return;

            }
            catch (Exception Err)
            {
                lbl_Error.Text = Err.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Save Consignment. Error: " + Err.Message + "')", true);
            }
            finally
            {

            }
        }
        private string IsMobileNumberValid(string mobileNumber)
        {
            string _mobileNumber = "";
            // remove all non-numeric characters
            _mobileNumber = CleanNumber(mobileNumber);

            // trim any leading zeros
            _mobileNumber = _mobileNumber.TrimStart(new char[] { '0' });

            // check for this in case they've entered 44 (0)xxxxxxxxx or similar
            if (_mobileNumber.StartsWith("920"))
            {
                _mobileNumber = _mobileNumber.Remove(2, 1);
            }

            // add country code if they haven't entered it
            if (!_mobileNumber.StartsWith("92"))
            {
                _mobileNumber = "92" + _mobileNumber;
            }

            // check if it's the right length
            if (_mobileNumber.Length != 12)
            {
                return "0";
            }

            return _mobileNumber;
        }
        private string CleanNumber(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }
        protected void cb_ConType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_ConType.SelectedValue != "0")
            {
                if (cb_ConType.SelectedValue == "12")
                {
                    lbl_Type.Text = "Coupon No";

                    //dd_city.Focus();
                }
                if (cb_ConType.SelectedValue == "13")
                {
                    lbl_Type.Text = "Flyer";
                    //dd_city.Focus();
                }
                if (cb_ConType.SelectedValue == "14")
                {
                    lbl_Type.Text = "Ref. No";
                    //dd_city.Focus();
                }
            }
            //dd_city.Focus();
            lbl_Error.Text = "";

        }
        protected void cb_ServiceType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            /*
            if (cb_ServiceType.SelectedValue != "0")
            {
                ViewState["ServiceType"] = cb_ServiceType.SelectedValue;
                if (ViewState["ServiceType"] != null)
                {
                    ViewState["ServiceType"] = cb_ServiceType.SelectedValue;
                }
            }
            */

            txt_aWeight.Enabled = true;
            txt_l.Enabled = true;
            txt_w.Enabled = true;
            txt_h.Enabled = true;

            if (cb_ServiceType.SelectedValue == "Mango Fiesta 5 Kg")
            {
                txt_Weight.Text = "5";
                txt_aWeight.Text = "5";
                txt_vWeight.Text = "5";
                txt_l.Text = "0";
                txt_w.Text = "0";
                txt_h.Text = "0";
                txt_Weight.Enabled = false;
                txt_aWeight.Enabled = false;
                txt_vWeight.Enabled = false;
                txt_l.Enabled = false;
                txt_w.Enabled = false;
                txt_h.Enabled = false;
            }

            if (cb_ServiceType.SelectedValue == "Mango Fiesta 7 Kg")
            {
                txt_Weight.Text = "7";
                txt_aWeight.Text = "7";
                txt_vWeight.Text = "7";
                txt_l.Text = "0";
                txt_w.Text = "0";
                txt_h.Text = "0";
                txt_Weight.Enabled = false;
                txt_aWeight.Enabled = false;
                txt_vWeight.Enabled = false;
                txt_l.Enabled = false;
                txt_w.Enabled = false;
                txt_h.Enabled = false;
            }

            if (cb_ServiceType.SelectedValue == "Mango Fiesta 10 Kg")
            {
                txt_Weight.Text = "10";
                txt_aWeight.Text = "10";
                txt_vWeight.Text = "10";
                txt_l.Text = "0";
                txt_w.Text = "0";
                txt_h.Text = "0";
                txt_Weight.Enabled = false;
                txt_aWeight.Enabled = false;
                txt_vWeight.Enabled = false;
                txt_l.Enabled = false;
                txt_w.Enabled = false;
                txt_h.Enabled = false;
            }
        }
        public DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name SNAME, ec.ExpressCenterCode ECCode\n" +
            "  from branches b\n" +
            "  left outer join ExpressCenters ec\n" +
            "    on ec.bid = b.branchCode\n" +
            "   and ec.Main_EC = '1'\n" +
            " where b.status = '1'\n" +
            " order by 2";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable Discount()
        {

            string sqlString = "SELECT distinct d.* \n" +
                               "FROM  MnP_MasterDiscount d \n" +
                               clvar.pruchaseorder + " \n" +
                               "WHERE d.ExpressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "' \n " +
                               "AND d.BranchCode = '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "' \n " +
                               "AND d.[STATUS] = '1' \n" +
                               "AND ISNULL(d.BOOKINGTYPE,'0') = '0' \n" +
                               "AND ISNULL(d.is_Approved,'0') = '1' \n" +
                               "AND cast(GETDATE() as date) BETWEEN cast(d.ValidFrom as date) AND cast(d.ValidTo AS date) \n" +
                               clvar.InvoiceNo;

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable Get_DiscountNumber()
        {

            string sqlString = "SELECT ExpressCenter,Zone,Branch,DiscountNumber+1 DiscountNumber FROM MNP_Retail_EC_DiscountNumber   \n" +
                                "where \n" +
                                "Branch = '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "' \n" +
                                "and ExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "' \n";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public string Add_Consignment(Cl_Variables obj, float denseWeight, string remarks)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp2", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);

                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "0");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.OriginExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@accountReceivingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@denseWeight", denseWeight);
                sqlcmd.Parameters.AddWithValue("@isApproved", "0");
                sqlcmd.Parameters.AddWithValue("@ispriceComputed", "1");
                sqlcmd.Parameters.AddWithValue("@InsertType", 2);
                sqlcmd.Parameters.AddWithValue("@PaymentMode", obj.PaymentMode);
                sqlcmd.Parameters.AddWithValue("@PaymentTransactionID", obj.PaymentTransactionID);
                
                //sqlcmd.Parameters.AddWithValue("@PaymentTransactionID", obj.Bank);

                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }
        public string Add_Consignment_Discount(Cl_Variables obj, float denseWeight, string remarks)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_DiscountWise", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@consigner", obj.Consigner);
                sqlcmd.Parameters.AddWithValue("@consignee", obj.Consignee);
                sqlcmd.Parameters.AddWithValue("@couponNumber", obj.CouponNo);
                sqlcmd.Parameters.AddWithValue("@customerType", obj.Customertype);
                sqlcmd.Parameters.AddWithValue("@orgin", obj.origin);
                sqlcmd.Parameters.AddWithValue("@destination", obj.Destination);
                sqlcmd.Parameters.AddWithValue("@pieces", obj.pieces);
                sqlcmd.Parameters.AddWithValue("@serviceTypeName", obj.ServiceTypeName);
                sqlcmd.Parameters.AddWithValue("@creditClientId", obj.CustomerClientID);
                sqlcmd.Parameters.AddWithValue("@consignmentTypeId", obj.Con_Type);
                sqlcmd.Parameters.AddWithValue("@weight", obj.Weight);
                sqlcmd.Parameters.AddWithValue("@weightUnit", obj.Unit);
                sqlcmd.Parameters.AddWithValue("@discount", obj.Discount);
                sqlcmd.Parameters.AddWithValue("@cod", obj.isCod);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                //sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@status", "1");
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", obj.isCreatedFromMMUs);
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", obj.expressionGreetingCard);
                sqlcmd.Parameters.AddWithValue("@expressionMessage", obj.expressionMessage);
                sqlcmd.Parameters.AddWithValue("@consigneePhoneNo", obj.ConsigneeCell);
                sqlcmd.Parameters.AddWithValue("@expressionconsignmentRefNumber", obj.expressionconsignmentRefNumber);
                sqlcmd.Parameters.AddWithValue("@otherCharges", obj.Othercharges);
                sqlcmd.Parameters.AddWithValue("@routeCode", obj.routeCode);
                sqlcmd.Parameters.AddWithValue("@docPouchNo", obj.docPouchNo);
                sqlcmd.Parameters.AddWithValue("@consignerPhoneNo", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@consignerCellNo", obj.ConsignerCell);
                sqlcmd.Parameters.AddWithValue("@consignerCNICNo", obj.ConsignerCNIC);
                sqlcmd.Parameters.AddWithValue("@consignerAccountNo", obj.consignerAccountNo);
                sqlcmd.Parameters.AddWithValue("@consignerEmail", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", obj.docIsHomeDelivery);
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", "0");
                sqlcmd.Parameters.AddWithValue("@isInsured", obj.isInsured);
                sqlcmd.Parameters.AddWithValue("@isReturned", obj.isReturned);
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.OriginExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", obj.receivedFromRider);
                sqlcmd.Parameters.AddWithValue("@chargedAmount", obj.ChargeAmount);
                sqlcmd.Parameters.AddWithValue("@cnState", obj.cnState);
                sqlcmd.Parameters.AddWithValue("@cardRefNo", obj.cardRefNo);
                sqlcmd.Parameters.AddWithValue("@manifestNo", obj.manifestNo);
                sqlcmd.Parameters.AddWithValue("@manifestId", obj.manifestId);
                sqlcmd.Parameters.AddWithValue("@originBrName", obj.originBrName);
                sqlcmd.Parameters.AddWithValue("@orderRefNo", obj.orderRefNo);
                sqlcmd.Parameters.AddWithValue("@customerName", obj.customerName);
                sqlcmd.Parameters.AddWithValue("@productTypeId", obj.productTypeId);
                sqlcmd.Parameters.AddWithValue("@productDescription", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@accountReceivingDate", obj.Bookingdate);
                sqlcmd.Parameters.AddWithValue("@denseWeight", denseWeight);
                sqlcmd.Parameters.AddWithValue("@isApproved", "0");
                sqlcmd.Parameters.AddWithValue("@ispriceComputed", "1");
                sqlcmd.Parameters.AddWithValue("@InsertType", 2);
                sqlcmd.Parameters.AddWithValue("@PaymentMode", obj.PaymentMode);
                sqlcmd.Parameters.AddWithValue("@DiscountID", obj.NoteNumber);
                sqlcmd.Parameters.AddWithValue("@PaymentTransactionID", obj.PaymentTransactionID);
                sqlcmd.Parameters.AddWithValue("@DiscountSelectionID", obj.NoteType);
                sqlcmd.Parameters.AddWithValue("@locationID", "");

                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }
        public string Add_Consignment_Discount_Tariff(Cl_Variables obj, DataTable dt_tblDiscount)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_UpdateDiscountConsignment_Tariff", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@consignmentNumber", obj.consignmentNo);
                sqlcmd.Parameters.AddWithValue("@tblDiscount", dt_tblDiscount);
                //sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                //sqlcmd.Parameters.AddWithValue("@gst", gst_);
                //sqlcmd.Parameters.AddWithValue("@chargedAmount", chargeAmount_);
                //sqlcmd.Parameters.AddWithValue("@DiscountAmount", discountAmount_);
                //sqlcmd.Parameters.AddWithValue("@DiscountGST", discountGst_);

                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar.Error;
        }
        public void Add_EC_IncrementNumber(Cl_Variables clvar)
        {
            string qry1 = "UPDATE MNP_Retail_EC_DiscountNumber SET \n" +
                          "DiscountNumber = '" + clvar.NoteNumber + "', \n" +
                          "ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                          "ModifiedOn = GETDATE() \n" +
                          "WHERE ExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "' \n" +
                          "AND Branch = '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "' ";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(qry1, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable SequenceCheck(Cl_Variables clvar, string specialCondition)
        {
            //string sql = "SELECT * \n"
            //    + "FROM mnp_riderCnSequence mzc \n"
            //    + "WHERE mzc.ZoneCode='" + clvar.Zone + "' AND \n"
            //    + "       '" + clvar.consignmentNo + "' BETWEEN mzc.SequenceStart AND mzc.SequenceEnd " + specialCondition + "";

            string sql = @"SELECT *
                            FROM MnP_CNIssue_Stock s
                            INNER JOIN MnP_CNIssue_Product p ON s.ProductID = p.ID
                            WHERE s.ZoneCode = '" + clvar.Zone + @"'
                            AND s.ECRiderCode = '" + Session["ExpressCenter"].ToString() + @"'
                            AND p.TypeId = '2'
                            AND s.StatusID ='1'
                            AND '" + clvar.consignmentNo + @"' BETWEEN s.BarcodeFrm AND s.BarcodeTo
                          " + specialCondition;

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 120;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetSequence(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string query = "select * from mnp_riderCnSequence where ZoneCode = '" + HttpContext.Current.Session["ZoneCode"].ToString() + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                ViewState["ZoneSequence"] = dt;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }


            return dt;
        }
        public DataTable GetCodSequence(string cn)
        {
            DataTable dt = new DataTable();
            string query = "select * from CODUserCNSequence";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                //ViewState["CODSequence"] = dt;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public Tuple<bool, string> ConsignmentCheck()
        {
            Tuple<bool, string> errorTuple;

            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;

            DataSet ds = con.Consignment(clvar);
            DataTable dt = ds.Tables[0];
            if (txt_ConNo.Text.Length >= 12)
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                clvar.consignmentNo = txt_ConNo.Text.Trim();
                DataTable zoneSequence = ViewState["ZoneSequence"] as DataTable;

                clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
                clvar.CheckCondition = " AND mzc.Product = (select products from serviceTypes_new where serviceTypeName = '" + cb_ServiceType.SelectedValue + "')";
                clvar.consignmentNo = txt_ConNo.Text.Trim();

                //#region Checking For CN Number availability without Product
                //if (zoneSequence != null)
                //{
                //    if (zoneSequence.Rows.Count > 0)
                //    {
                //        bool valid = false;
                //        foreach (DataRow dr in zoneSequence.Rows)
                //        {
                //            Int64 start = Int64.Parse(dr["SequenceStart"].ToString());
                //            Int64 end = Int64.Parse(dr["SequenceEnd"].ToString());
                //            Int64 cn = Int64.Parse(txt_ConNo.Text.Trim());
                //            if (cn >= start && cn <= end)
                //            {
                //                valid = true;
                //                break;
                //            }
                //        }
                //        if (!valid)
                //        {
                //            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not Allowed in this Zone.')", true);
                //            //lbl_Error.Text = "This CN Number is Not valid.";
                //            errorTuple = new Tuple<bool, string>(false, "CN Number Not Allowed in this Zone");
                //            return errorTuple;
                //        }

                //    }
                //    else
                //    {
                //        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                //        //lbl_Error.Text = "This CN Number is Not valid.";
                //        errorTuple = new Tuple<bool, string>(false, "CN Number Not Allowed in this Zone");
                //        return errorTuple;
                //        //return;
                //    }
                //}
                //else
                //{
                //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                //    //lbl_Error.Text = "This CN Number is Not valid.";
                //    errorTuple = new Tuple<bool, string>(false, "This CN Number is Not valid. Contact IT Support");
                //    return errorTuple;
                //    //return;
                //}
                //#endregion
                #region Checking For CN Number availability with Product

                // Backup Date 13-Nov 2020
                //DataTable seq = SequenceCheck(clvar, " AND mzc.Product = (select products from serviceTypes_new where serviceTypeName = '" + cb_ServiceType.SelectedValue + "')");
                DataTable seq = SequenceCheck(clvar, " AND p.masterproduct = (select products from serviceTypes_new where serviceTypeName = '" + cb_ServiceType.SelectedValue + "')");

                if (seq.Rows.Count <= 0)
                {
                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number cannot be used for selected Service')", true);
                    errorTuple = new Tuple<bool, string>(false, "Invalid Consignment Number");
                    return errorTuple;
                }
                #endregion




                if (ds.Tables[0].Rows.Count != 0)
                {

                    errorTuple = new Tuple<bool, string>(false, "Invalid Consignment Number/Duplicate Consignment No.");
                    txt_ConNo.Text = "";
                    return errorTuple;
                }
                else
                {

                    lbl_Error.Text = "";
                    DataTable CODsequece = GetCodSequence(clvar.consignmentNo);//ViewState["CODSequence"] as DataTable;
                    #region Checking for CN Number in COD USER CN Sequence
                    if (CODsequece != null)
                    {
                        if (CODsequece.Rows.Count > 0)
                        {
                            bool valid = false;
                            foreach (DataRow dr in CODsequece.Rows)
                            {
                                Int64 start = Int64.Parse(dr["SequenceStart"].ToString());
                                Int64 end = Int64.Parse(dr["SequenceEnd"].ToString());
                                Int64 cn = Int64.Parse(txt_ConNo.Text.Trim());
                                if (cn >= start && cn <= end)
                                {
                                    valid = true;
                                    break;
                                }
                            }
                            if (valid)
                            {
                                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN is Reserved for COD.')", true);
                                //lbl_Error.Text = "This CN is Reserved for COD.";
                                errorTuple = new Tuple<bool, string>(false, "This CN is Reserved for COD.");
                                return errorTuple;
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                            //lbl_Error.Text = "This CN Number is Not valid.";
                            errorTuple = new Tuple<bool, string>(false, "This CN Number is Not valid.");
                            return errorTuple;
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
                        //lbl_Error.Text = "This CN Number is Not valid.";
                        errorTuple = new Tuple<bool, string>(false, "This CN Number is Not valid.");
                        return errorTuple;
                    }
                    #endregion
                }
            }
            else
            {
                txt_ConNo.Text = "";
                txt_ConNo.Focus();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment No cannot be less than 12 Digits')", true);
                //txt_ConNo.Focus();

                errorTuple = new Tuple<bool, string>(false, "Consignment No cannot be less than 12 Digits");
                return errorTuple;

            }


            errorTuple = new Tuple<bool, string>(true, "");
            return errorTuple;
        }
        public Tuple<bool, string> AccountNumberCheck()
        {
            Tuple<bool, string> errorTuple;

            if (txt_AccNo.Text != "")
            {
                if (txt_AccNo.Text != "")
                {
                    clvar.Branch = cb_Origin.SelectedValue;
                    clvar.AccountNo = txt_AccNo.Text;
                    DataSet ds = con.CustomerInformation(clvar);

                    if (ds.Tables.Count != 0)
                    {
                        DataRow[] dr = ds.Tables[0].Select("AccountNo = '" + clvar.AccountNo + "'");
                        if (dr.Count() == 0)
                        {
                            errorTuple = new Tuple<bool, string>(false, "Invalid Account Number");
                            return errorTuple;
                        }

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            //txt_Consigner.Text = dr[0]["Name"].ToString();
                            //txt_ConsignerCellNo.Text = dr[0]["PhoneNo"].ToString();
                            //txt_ShipperAddress.Text = dr[0]["address"].ToString();
                            hd_CreditClientID.Value = dr[0]["id"].ToString();

                            if (dr[0]["accountNo"].ToString() == "0")
                            {
                                this.Rb_CustomerType.Items[1].Selected = true;
                            }
                            else
                            {
                                this.Rb_CustomerType.Items[0].Selected = true;
                            }

                            if (dr[0]["IsCOD"].ToString() == "True")
                            {
                                cb_COD.Checked = true;
                                Cb_CODAmount.Checked = true;
                                txt_OrderRefNo.Enabled = true;
                                dd_ProductType.Enabled = true;
                                txt_Description.Enabled = true;
                                txt_CodAmount.Enabled = true;

                                //Now Populating Product Type of COD
                                clvar.CustomerClientID = dr[0]["id"].ToString();
                                ViewState["CODType"] = dr[0]["CODType"].ToString();
                                DataSet ds_Product = con.ProductTypeInfo(clvar);
                                if (ds_Product.Tables[0].Rows.Count != 0)
                                {
                                    dd_ProductType.Items.Add(new ListItem { Text = "Select Product", Value = "0" });
                                    dd_ProductType.DataTextField = "ProductTypeName";
                                    dd_ProductType.DataValueField = "Producttypecode";
                                    dd_ProductType.DataSource = ds_Product.Tables[0].DefaultView;
                                    dd_ProductType.DataBind();
                                }
                                else
                                {
                                    dd_ProductType.Items.Clear();
                                }
                            }
                            else
                            {
                                cb_COD.Checked = false;
                                Cb_CODAmount.Checked = false;
                                txt_OrderRefNo.Enabled = false;
                                dd_ProductType.Enabled = false;
                                txt_Description.Enabled = false;
                                txt_CodAmount.Enabled = false;
                            }
                        }
                        lbl_Error.Text = "";
                        txt_RiderCode.Focus();
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account Number')", true);
                        txt_AccNo.Text = "";
                        txt_AccNo.Focus();
                        errorTuple = new Tuple<bool, string>(false, "Invalid Account Number");
                        return errorTuple;

                    }
                }
            }
            else
            {

            }
            if (txt_AccNo.Text.Trim(' ') == "0")
            {
                //txt_Consigner.Text = "";//ds.Tables[0].Rows[0]["Name"].ToString();
                //txt_ConsignerCellNo.Text = "";// ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                //txt_ShipperAddress.Text = ""; //ds.Tables[0].Rows[0]["address"].ToString();
            }



            errorTuple = new Tuple<bool, string>(true, "");
            return errorTuple;
        }
        public Tuple<bool, string> RiderCodeCheck()
        {
            Tuple<bool, string> errorTuple;

            clvar = new Cl_Variables();
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.origin = cb_Origin.SelectedValue;
            if (txt_RiderCode.Text == txt_Weight.Text)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Rider Code Cannot be Equal to Weight.')", true);
                errorTuple = new Tuple<bool, string>(false, "Rider Code Cannot be Equal to Weight.");
                return errorTuple;
            }
            DataSet Rider = con.RiderInformation(clvar);
            if (Rider.Tables[0].Rows.Count != 0)
            {
                // hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                //this.cb_originExpresscenter.SelectedValue = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                cb_ConType.Focus();
            }
            else
            {
                txt_RiderCode.Text = "";
                txt_RiderCode.Focus();

                errorTuple = new Tuple<bool, string>(false, "Invalid Rider Code.");
                return errorTuple;
            }



            errorTuple = new Tuple<bool, string>(true, "");
            return errorTuple;
        }
        public Tuple<bool, string> GetECBySeq(string consignmentNumber, string accountNumber, string riderCode)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");
            if (accountNumber.Trim() == "0")
            {
                DataTable seqTbl = GetECBySequence(consignmentNumber);
                if (seqTbl != null)
                {
                    if (seqTbl.Rows.Count > 0)
                    {
                        if (seqTbl.Rows.Count > 1)
                        {
                            resp = new Tuple<bool, string>(false, "Consignment Sequence Issued to Multiple Express Centers. Contact your Zonal Accountant.");
                            //cb_originExpresscenter
                            cb_originExpresscenter.ClearSelection();
                            return resp;
                        }
                        ListItem item = cb_originExpresscenter.Items.FindByValue(seqTbl.Rows[0]["ExpressCenter"].ToString());
                        if (item != null)
                        {
                            cb_originExpresscenter.SelectedValue = seqTbl.Rows[0]["ExpressCenter"].ToString();
                            resp = new Tuple<bool, string>(true, seqTbl.Rows[0]["ExpressCEnter"].ToString());
                            return resp;
                        }
                        else
                        {
                            if (seqTbl.Rows[0]["ExpressCenter"].ToString().Trim() == "")
                            {
                                resp = new Tuple<bool, string>(false, "No Express Center Defined for this sequence.");
                                cb_originExpresscenter.ClearSelection();
                                return resp;
                            }
                            cb_originExpresscenter.Items.Add(new ListItem { Value = seqTbl.Rows[0]["ExpressCenter"].ToString(), Text = seqTbl.Rows[0]["ECName"].ToString() });
                            cb_originExpresscenter.SelectedValue = seqTbl.Rows[0]["ExpressCEnter"].ToString();
                            resp = new Tuple<bool, string>(true, seqTbl.Rows[0]["ExpressCEnter"].ToString());
                            return resp;
                        }

                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "Express Center not found for Entered consignment.");
                        cb_originExpresscenter.ClearSelection();
                        return resp;
                    }
                }
                else
                {
                    resp = new Tuple<bool, string>(false, "Express Center not found for Entered consignment.");
                    cb_originExpresscenter.ClearSelection();
                    return resp;
                }
            }
            else
            {
                DataTable riderTbl = GetECByRider(riderCode);
                if (riderTbl != null)
                {
                    if (riderTbl.Rows.Count > 0)
                    {

                        if (riderTbl.Rows[0]["ExpressCenterID"].ToString().Trim() == "")
                        {
                            resp = new Tuple<bool, string>(false, "No Express Center Found for Entered Rider.");
                            cb_originExpresscenter.ClearSelection();
                            return resp;
                        }
                        else
                        {
                            ListItem item = cb_originExpresscenter.Items.FindByValue(riderTbl.Rows[0]["ExpressCenterID"].ToString());
                            if (item != null)
                            {
                                cb_originExpresscenter.SelectedValue = riderTbl.Rows[0]["ExpressCenterID"].ToString();
                            }
                            else
                            {
                                cb_originExpresscenter.Items.Add(new ListItem { Text = riderTbl.Rows[0]["ECNAME"].ToString(), Value = riderTbl.Rows[0]["ExpressCenterID"].ToString() });
                                cb_originExpresscenter.SelectedValue = riderTbl.Rows[0]["ExpressCenterID"].ToString();
                            }
                            resp = new Tuple<bool, string>(true, riderTbl.Rows[0]["ExpressCenterID"].ToString());
                            return resp;
                        }
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "No Express Center Found for Entered Rider.");
                        cb_originExpresscenter.ClearSelection();
                        return resp;
                    }
                }
                else
                {
                    resp = new Tuple<bool, string>(false, "No Express Center Found for Entered Rider.");
                    cb_originExpresscenter.ClearSelection();
                    return resp;
                }
            }
            return resp;
        }
        public DataTable GetECBySequence(string consignmentNumber)
        {

            string sqlString = "SELECT *\n" +
            "  FROM Mnp_RiderCNSequence mrc\n" +
            " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd\n" +
            "   AND mrc.Branch = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

            sqlString = "SELECT mrc.*, ec.name ECName\n" +
            "  FROM Mnp_RiderCNSequence mrc\n" +
            " LEFT OUTER JOIN ExpressCenters ec\n" +
            "    ON ec.expressCenterCode = mrc.ExpressCenter\n" +
            "   AND ec.bid = mrc.Branch\n" +
            " WHERE '" + consignmentNumber + "' BETWEEN mrc.SequenceStart AND mrc.SequenceEnd";


            sqlString = @"SELECT s.ECRiderCode ExpressCenter, ec.name ECName,*
                FROM   MnP_CNIssue_Stock s
                INNER JOIN MnP_CNIssue_Product p ON s.ProductID = p.ID
                LEFT OUTER JOIN ExpressCenters ec ON ec.expressCenterCode = s.ECRiderCode
                WHERE  s.ZoneCode = '" + HttpContext.Current.Session["ZoneCode"].ToString() + @"'
                AND s.ECRiderCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + @"'
                AND p.TypeId = '2'
                AND s.StatusID ='1'
                AND '" + consignmentNumber + @"' BETWEEN s.BarcodeFrm AND s.BarcodeTo                
                AND p.masterproduct = (select products from serviceTypes_new where serviceTypeName = '" + cb_ServiceType.SelectedValue + @"')";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }
        public DataTable GetECByRider(string riderCode)
        {
            clvar.riderCode = riderCode;
            clvar.RiderCode = riderCode;
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
            DataSet Rider = RiderInformation(clvar);
            if (Rider == null)
            {
                return new DataTable();
            }
            else if (Rider.Tables[0] == null)
            {
                return new DataTable();
            }
            else
            {
                return Rider.Tables[0];
            }


        }
        public void Alert(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            lbl_Error.Text = message;

        }
        public DataSet CustomerInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode='" + clvar.Branch + "'";

                string sqlString = "SELECT cc.*,\n" +
                "       case\n" +
                "         when cu.accountNO is not null then\n" +
                "          '1'\n" +
                "         else\n" +
                "          '0'\n" +
                "       end isAPIClient\n" +
                "  FROM CreditClients cc\n" +
                "  left outer join CODUSERS cu\n" +
                "    on cu.accountno = cc.accountno\n" +
                "   and cu.creditCLientID = cc.id\n" +
                "\n" +
                " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
                "   and cc.branchCode = '" + clvar.Branch + "'\n" +
                "   and cc.isActive = '1'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                //ViewState["AccountsInfo"] = Ds_1;
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        public DataSet PriceModifiers()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT id, \n"
               + "       pm.name PriceModifier, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], calculationbase, pm.isGST \n"
               + "FROM   PriceModifiers pm \n"
               + "WHERE  pm.[status] = '1' \n"
               + "AND pm.chkBillingModifier ='0' AND isnull(pm.Import,'0') = '0' \n"
               + "GROUP BY \n"
               + "       id, \n"
               + "       pm.name, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], pm.calculationbase, pm.isGST \n"
               + "ORDER BY pm.name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        public DataSet PaymentSource()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "Select id,Name from Paymentsource where booking ='1' and status ='1'";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        public void Get_Paymentsource()
        {
            DataSet ds = PaymentSource();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination

                this.dd_PaymentMode.Items.Add(new ListItem { Text = "Select Payment Mode", Value = "0" });
                this.dd_PaymentMode.DataTextField = "Name";
                this.dd_PaymentMode.DataValueField = "id";
                this.dd_PaymentMode.DataSource = ds.Tables[0].DefaultView;
                this.dd_PaymentMode.DataBind();

                this.dd_PaymentMode.SelectedValue = "0";

            }

        }
        public DataSet RiderInformation(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                //string query = "SELECT * FROM Riders r left outer join expressCenters ec on ec.expressCenterCode = r.ExpressCenterId and ec.bid = r.branchid WHERE r.branchId ='" + clvar.origin + "' and ridercode='" + clvar.RiderCode + "' and r.status = '1'";
                string query = "SELECT ec.name ECNAME, r.* FROM Riders r left outer join expressCenters ec on ec.expressCenterCode = r.ExpressCenterId and ec.bid = r.branchid WHERE r.branchId ='" + clvar.origin + "' and ridercode='" + clvar.RiderCode + "' and r.status = '1'";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        public DataTable Get_DiscountCNs()
        {
            string sqlString = @"SELECT c.consignmentNumber, c.totalAmount, c.chargedAmount, c.gst, c.DiscountID, c.DiscountApplied, c.DiscountGST, 
                               c.consigner, c.shipperAddress, c.consignerCellNo, c.consignerCNICNo, SUM(ISNULL(cm.modifiedCalculationValue,'0')) ModifierValue,
                               isnull(cm.calculatedValue,0) calculatedValue, isnull(cm.calculatedGST,0) calculatedGST
                               FROM 
                               Consignment c
                               LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber = c.consignmentNumber
                               WHERE  
                               c.DiscountID = '" + txt_discountId.Text + @"' 
                               AND C.STATUS !='9'
                               AND c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + @"'  
                               AND c.orgin = '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + @"' 
                               AND CAST(c.bookingdate AS date) = cast(GETDATE() AS date) "
                              + clvar.VoucherNo +
                              @"GROUP BY c.consignmentNumber, c.totalAmount, c.chargedAmount, c.gst, c.DiscountID, c.DiscountApplied, c.DiscountGST, 
                              c.consigner, c.shipperAddress, c.consignerCellNo, c.consignerCNICNo, cm.calculatedValue, cm.calculatedGST
                              Order by c.consignmentNumber DESC";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public string Add_ConsignmentModifier(DataTable dt, Cl_Variables clvar)
        {
            clvar.Error = "";
            if (dt.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(clvar.Strcon()))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        try
                        {
                            //Set the database table name
                            sqlBulkCopy.DestinationTableName = "dbo.ConsignmentModifier";
                            DataTable dt_ = dt.Clone();
                            dt_ = dt.Copy();
                            dt_.Columns.Remove("isGST");
                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                            //sqlBulkCopy.ColumnMappings.Add("PriceModifierID", "");
                            //sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                            //sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt_);
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            clvar.Error = ex.Message;
                        }
                    }
                }
            }
            return clvar.Error;
        }
        protected void txt_ConsigneeCellno_TextChanged(object sender, EventArgs e)
        {
            if (txt_ConsigneeCellno.Text.Length > 8)
            {
                DataSet Ds_1 = new DataSet();
                try
                {
                    string query = "SELECT top(1) * FROM customer_directory where contact = '" + txt_ConsigneeCellno.Text.ToString() + "' and isactive ='1' order by addedOn desc";
                    SqlConnection orcl = new SqlConnection(clvar.Strcon());
                    orcl.Open();
                    SqlCommand orcd = new SqlCommand(query, orcl);
                    orcd.CommandType = CommandType.Text;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    oda.Fill(Ds_1);
                    orcl.Close();
                    if (Ds_1.Tables[0].Rows.Count > 0)
                    {
                        txt_Consignee.Text = Ds_1.Tables[0].Rows[0]["name"].ToString();
                        txt_Address.Text = Ds_1.Tables[0].Rows[0]["address"].ToString();
                        txt_ConsigneeCNIC.Text = Ds_1.Tables[0].Rows[0]["nic"].ToString();
                    }
                }
                catch (Exception Err)
                { }
            }
        }
        protected void txt_ConsignerCellNo_TextChanged1(object sender, EventArgs e)
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string query = "SELECT top(1) * FROM customer_directory where contact = '" + txt_ConsignerCellNo.Text.ToString() + "' and isactive ='1' order by addedOn desc";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
                if (Ds_1.Tables[0].Rows.Count > 0)
                {
                    txt_Consigner.Text = Ds_1.Tables[0].Rows[0]["name"].ToString();
                    txt_ShipperAddress.Text = Ds_1.Tables[0].Rows[0]["address"].ToString();
                    Txt_ConsignerCNIC.Text = Ds_1.Tables[0].Rows[0]["nic"].ToString();
                }
            }
            catch (Exception Err)
            { }
        }
        protected void PricemodifierPieces_TextChanged(object sender, EventArgs e)
        {
            //txt_CalPriceModifierValue.Text = txt_pricemodifier_pieces.Text * txt_Value;
            //double cal = double.Parse(((TextBox)RadGrid1.Items[0].FindControl("txt_CalPriceModifierValue")).Text);
            //double Pieces = double.Parse(((TextBox)RadGrid1.Items[0].FindControl("txt_PricemodifierPieces")).Text);
            //double ModifierValue = double.Parse(((TextBox)RadGrid1.Items[0].FindControl("txt_Value")).Text);

            //cal = Pieces * ModifierValue;

            //(this.RadGrid1.Controls[this.RadGrid1.Items.Count + 1].FindControl("txt_CalPriceModifierValue") as Label).Text = cal.ToString();

            foreach (RepeaterItem item in RadGrid1.Items)
            {
                TextBox cal = (TextBox)item.FindControl("txt_CalPriceModifierValue");
                TextBox Pieces = (TextBox)item.FindControl("txt_PricemodifierPieces");
                TextBox ModifierValue = (TextBox)item.FindControl("txt_Value");

                // for (int i = 0; i <= item.ItemIndex; i++)
                // {
                if (Pieces.Text != "0" && Pieces.Text != "")
                {
                    int Pieces_ = int.Parse(((TextBox)RadGrid1.Items[item.ItemIndex].FindControl("txt_PricemodifierPieces")).Text);
                    int Value_ = int.Parse(((TextBox)RadGrid1.Items[item.ItemIndex].FindControl("txt_Value")).Text);
                    int Cal_ = int.Parse(((TextBox)RadGrid1.Items[item.ItemIndex].FindControl("txt_CalPriceModifierValue")).Text);

                    // cal = double.Parse(Pieces.ToString());// *ModifierValue;

                    //string a = double.Parse(Pieces.Text).ToString("F0");
                    //string a = double.Parse(Pieces.Text).ToString("F0");

                    //double.Parse(cal.Text).ToString("F0") = double.Parse(Pieces.Text).ToString("F0") * double.Parse(ModifierValue.Text).ToString("F0");

                    // int total = int.Parse(Pieces.Text) * int.Parse(ModifierValue.Text);
                    int total = int.Parse(Pieces_.ToString()) * int.Parse(Value_.ToString());

                    //Cal_.Text = total;
                    //Cal_ = total;

                    //  cal = Convert.ToInt32(total);

                    int x = Convert.ToInt32(total);
                    cal.Text = x.ToString();

                }
                //}
            }
        }
        protected void discount_SelectedIndexChanged(object sender, EventArgs e)
        {
            String DiscountID = "";
            for (int i = 0; i < dd_discount.Items.Count; i++)
            {
                if (dd_discount.Items[i].Selected)
                {
                    DiscountID += dd_discount.Items[i].Value + ',';
                }
            }
            DiscountID = DiscountID.Remove(DiscountID.Length - 1);

            clvar.InvoiceNo = "AND DiscountID = '" + DiscountID.ToString() + "'";

            dt_discount = Discount();
            if (dt_discount.Rows.Count > 0)
            {
                //lbl_discount.Text = "<B><U>Description:</u></b><BR><b>Min CN</b>: " + dt_discount.Rows[0]["MinCNCount"].ToString() +
                //                    "&nbsp;&nbsp;&nbsp; <b>Max CN</b>: " + dt_discount.Rows[0]["MaxCNCount"].ToString() + "<br>" +
                //                    "<b>Min Weight</b>: " + dt_discount.Rows[0]["MinWeight"].ToString() +
                //                    "&nbsp;&nbsp;&nbsp; <b>Max Weight</b>: " + dt_discount.Rows[0]["MaxWeight"].ToString() + "<br>" +
                //                    "<b>Service</b>: " + dt_discount.Rows[0]["ServiceType"].ToString() + "<br>" +
                //                    "<b>Discount Amount</b>: " + dt_discount.Rows[0]["DiscountValue"].ToString();

                lbl_discount.Text = "<B><U>Description:</u></b><BR>" +
                                    "<b>Min CN</b>: " + dt_discount.Rows[0]["MinCNCount"].ToString() + "<br>" +
                                    "<b>Max CN</b>: " + dt_discount.Rows[0]["MaxCNCount"].ToString() + "<br>" +
                                    "<b>Min Weight</b>: " + dt_discount.Rows[0]["MinWeight"].ToString() + "<br>" +
                                    "<b>Max Weight</b>: " + dt_discount.Rows[0]["MaxWeight"].ToString() + "<br>" +
                                    "<b>Service</b>: " + dt_discount.Rows[0]["ServiceType"].ToString() + "<br>";

                if (dt_discount.Rows[0]["DiscountValueType"].ToString() == "1")// FOR DISCOUNT PERCENTAGE
                {
                    lbl_discount.Text += "<b>Discount Percentage</b>: " + dt_discount.Rows[0]["DiscountValue"].ToString();
                }
                if (dt_discount.Rows[0]["DiscountValueType"].ToString() == "2") // FOR DISCOUNT AMOUNT
                {
                    lbl_discount.Text += "<b>Discount Amount</b>: PK RS " + dt_discount.Rows[0]["DiscountValue"].ToString();
                }

                ViewState["Discount"] = dt_discount;

                txt_discountId.Text = "";
            }
            else
            {
                ViewState["Discount"] = null;
                lbl_discount.Text = "";
                txt_discountId.Text = "";
            }

        }
        protected void discountId_Fixed_TextChanged(object sender, EventArgs e)
        {
            if (dd_discount.SelectedValue != "0" || txt_discountId.Text != "")
            {
                //clvar.pruchaseorder = "INNER JOIN MNP_DiscountConsignment dc ON dc.DiscountID = d.DiscountID \n" +
                //                      "INNER JOIN Consignment c ON c.consignmentNumber = dc.ConsignmentNumber \n";

                if (txt_discountId.Text.Trim() != "")
                {
                    clvar.InvoiceNo = "AND c.DiscountID = '" + txt_discountId.Text.Trim() + "'";
                    clvar.pruchaseorder = "INNER JOIN MNP_DiscountConsignment dc ON dc.DiscountID = d.DiscountID \n" +
                                          "INNER JOIN Consignment c ON c.consignmentNumber = dc.ConsignmentNumber \n";
                }
                else
                {
                    clvar.InvoiceNo = "AND d.DiscountID = '" + dd_discount.SelectedValue + "'";
                }

                dt_discount = Discount();
                if (dt_discount.Rows.Count > 0)
                {
                    lbl_discount.Text = "<B><U>Description:</u></b><BR>" +
                                        "<b>Min CN</b>: " + dt_discount.Rows[0]["MinCNCount"].ToString() + "<br>" +
                                        "<b>Max CN</b>: " + dt_discount.Rows[0]["MaxCNCount"].ToString() + "<br>" +
                                        "<b>Min Weight</b>: " + dt_discount.Rows[0]["MinWeight"].ToString() + "<br>" +
                                        "<b>Max Weight</b>: " + dt_discount.Rows[0]["MaxWeight"].ToString() + "<br>" +
                                        "<b>Service</b>: " + dt_discount.Rows[0]["ServiceType"].ToString() + "<br>";

                    if (dt_discount.Rows[0]["DiscountValueType"].ToString() == "1")// FOR DISCOUNT PERCENTAGE
                    {
                        lbl_discount.Text += "<b>Discount Percentage</b>: " + dt_discount.Rows[0]["DiscountValue"].ToString();
                    }
                    if (dt_discount.Rows[0]["DiscountValueType"].ToString() == "2") // FOR DISCOUNT AMOUNT
                    {
                        lbl_discount.Text += "<b>Discount Amount</b>: PK RS " + dt_discount.Rows[0]["DiscountValue"].ToString();
                    }
                    dd_discount.SelectedValue = dt_discount.Rows[0]["DiscountId"].ToString();
                    dd_discount.Enabled = false;
                    btn_discount_save.Visible = false;

                    ViewState["Discount"] = dt_discount;

                    if (txt_discountId.Text != "")
                    {
                        div_discountcn.Attributes.Add("style", "display:block;height: 155px; overflow-y: hidden;");

                        clvar.VoucherNo = "AND DiscountApplied IS null \n" +
                                                "and DiscountGST IS NULL \n" +
                                                "AND ISNULL(STATUS,'0') = '1' \n" +
                                                "AND ISNULL(isApproved,'0') = '0' \n" +
                                                "AND ISNULL(isPriceComputed,'0') = '1' \n";

                        DataTable discn_dt = Get_DiscountCNs();

                        discountedCN.Text = "";

                        if (discn_dt.Rows.Count == int.Parse(dt_discount.Rows[0]["MaxCNCount"].ToString()))
                        {
                            if (discn_dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < discn_dt.Rows.Count; i++)
                                {
                                    discountedCN.Text += "(" + (i + 1) + ") " + discn_dt.Rows[i]["consignmentNumber"].ToString() + "<br>";
                                    btn_discount_save.Visible = true;
                                }

                                limitover = "limitover";

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Slab exceeding the max slab limit')", true);
                                return;
                            }
                        }

                        if (discn_dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < discn_dt.Rows.Count; i++)
                            {
                                discountedCN.Text += "(" + (i + 1) + ") " + discn_dt.Rows[i]["consignmentNumber"].ToString() + "<br>";

                                txt_ConsignerCellNo.Text = discn_dt.Rows[i]["consignerCellNo"].ToString();
                                txt_Consigner.Text = discn_dt.Rows[i]["consigner"].ToString();
                                Txt_ConsignerCNIC.Text = discn_dt.Rows[i]["consignerCNICNo"].ToString();
                                txt_ShipperAddress.Text = discn_dt.Rows[i]["shipperAddress"].ToString();
                            }

                            if (dt_discount.Rows[0]["IsSpecial"].ToString() == "1")
                            {
                                if (discn_dt.Rows.Count >= int.Parse(dt_discount.Rows[0]["MinCNCount"].ToString()) && discn_dt.Rows.Count <= int.Parse(dt_discount.Rows[0]["MaxCNCount"].ToString()))
                                {
                                    btn_discount_save.Visible = true;
                                }
                            }
                            else
                            {
                                if (discn_dt.Rows.Count >= int.Parse(dt_discount.Rows[0]["MinCNCount"].ToString()) && discn_dt.Rows.Count <= int.Parse(dt_discount.Rows[0]["MaxCNCount"].ToString()))
                                {
                                    btn_discount_save.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            ViewState["Discount"] = null;
                            lbl_discount.Text = "";
                            dd_discount.Enabled = true;
                            dd_discount.SelectedValue = "0";

                            txt_discountId.Text = "";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Discount Id already Used.')", true);
                            return;
                        }
                    }
                }
                else
                {
                    ViewState["Discount"] = null;
                    lbl_discount.Text = "";
                    dd_discount.Enabled = true;
                    dd_discount.SelectedValue = "0";
                    div_discountcn.Attributes.Add("style", "display:none;");
                }
            }
        }
        protected void btn_Discount_Save_Click(object sender, EventArgs e)
        {
            clvar.pruchaseorder = "INNER JOIN MNP_DiscountConsignment dc ON dc.DiscountID = d.DiscountID \n" +
                                  "INNER JOIN Consignment c ON c.consignmentNumber = dc.ConsignmentNumber \n";

            clvar.InvoiceNo = "AND c.DiscountID = '" + txt_discountId.Text.Trim() + "'";

            float discountAmount_ = 0;
            float discountGst_ = 0;
            float chargeAmount_ = 0;
            float gst_ = 0;
            float ModifierValue = 0;
            float ModifierGst = 0;

            DataTable dt = Discount();

            DataTable dt_tblDiscount = new DataTable();
            dt_tblDiscount.Columns.AddRange(new DataColumn[]
                {
                new DataColumn("consignmentNumber"),
                new DataColumn("chargeAmount"),
                new DataColumn("totalAmount"),
                new DataColumn("gst"),
                new DataColumn("DiscountAmount"),
                new DataColumn("DiscountGST")
                });

            dt_tblDiscount.AcceptChanges();
            ViewState["dt_tblDiscount"] = dt_tblDiscount;
            String consignments = "";

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["DiscountValueType"].ToString() == "1")
                {
                    // FOR PERCENTAGE FLAG

                    DataTable dt_cn = Get_DiscountCNs();

                    if (dt_cn.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_cn.Rows.Count; i++)
                        {
                            consignments += "" + dt_cn.Rows[i]["consignmentNumber"].ToString() + ",";

                            clvar.consignmentNo = dt_cn.Rows[i]["consignmentNumber"].ToString();
                            clvar.ChargeAmount = float.Parse(dt_cn.Rows[i]["chargedAmount"].ToString());
                            clvar.gst = float.Parse(dt_cn.Rows[i]["gst"].ToString());
                            clvar.TotalAmount = float.Parse(dt_cn.Rows[i]["totalAmount"].ToString());
                            //ModifierValue = float.Parse(dt_cn.Rows[i]["ModifierValue"].ToString());
                            ModifierValue = float.Parse(dt_cn.Rows[i]["calculatedValue"].ToString());
                            ModifierGst = float.Parse(dt_cn.Rows[i]["calculatedGST"].ToString());


                            float TotalAmount = float.Parse(clvar.TotalAmount.ToString()) - ModifierValue;
                            float TotalGST = float.Parse(clvar.gst.ToString()) - ModifierGst;

                            discountAmount_ = 100 - float.Parse(dt.Rows[0]["DiscountValue"].ToString());
                            discountGst_ = float.Parse(dt.Rows[0]["DiscountValue"].ToString()) * (TotalGST / 100);
                            gst_ = TotalGST - discountGst_;

                            chargeAmount_ = (TotalAmount * (discountAmount_ / 100)) + gst_ + ModifierValue + ModifierGst;

                            float FinalTotalAmount = (TotalAmount * (discountAmount_ / 100)) + ModifierValue;
                            float FinalGST = gst_ + ModifierGst;

                            /*
                            
                            // ==================
                            // OLD DISCOUNT CODE
                            // ==================
                            //discountAmount_ = 100 - float.Parse(dt.Rows[0]["DiscountValue"].ToString());
                            //discountGst_ = float.Parse(dt.Rows[0]["DiscountValue"].ToString()) * (float.Parse(dt_cn.Rows[i]["gst"].ToString()) / 100);
                            //gst_ = float.Parse(dt_cn.Rows[i]["gst"].ToString()) - discountGst_;

                            //FinalChargeAmount = (clvar.TotalAmount - ModifierValue) * (discountAmount_ / 100) + gst_;
                            //chargeAmount_ = float.Parse(FinalChargeAmount.ToString());

                            //chargeAmount_ = (float.Parse(dt_cn.Rows[i]["totalAmount"].ToString()) * (discountAmount_ / 100)) + gst_;
                            //chargeAmount_ = chargeAmount_ + float.Parse(dt_cn.Rows[i]["ModifierValue"].ToString());

                            //chargeAmount_ = (float.Parse(dt_cn.Rows[i]["totalAmount"].ToString()) * (discountAmount_ / 100));

                            */

                            DataRow dr = dt_tblDiscount.NewRow();

                            dr["consignmentNumber"] = clvar.consignmentNo;
                            dr["chargeAmount"] = chargeAmount_;
                            dr["totalAmount"] = FinalTotalAmount;
                            dr["gst"] = FinalGST;

                            //dr["totalAmount"] = clvar.TotalAmount;
                            //dr["gst"] = clvar.gst;
                            dr["DiscountAmount"] = float.Parse(dt.Rows[0]["DiscountValue"].ToString());
                            dr["DiscountGST"] = discountGst_;

                            dt_tblDiscount.Rows.Add(dr);


                            /*
                            DataRow dr = dt_tblDiscount.NewRow();

                            dr["consignmentNumber"] = clvar.consignmentNo;
                            dr["chargeAmount"] = chargeAmount_;
                            dr["totalAmount"] = clvar.TotalAmount;
                            dr["gst"] = gst_;
                            dr["DiscountAmount"] = discountAmount_;
                            dr["DiscountGST"] = discountGst_;

                            dt_tblDiscount.Rows.Add(dr);
                             */
                        }
                    }
                }
                else
                {
                    // FOR AMOUNT FLAG
                    DataTable dt_cn = Get_DiscountCNs();

                    if (dt_cn.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_cn.Rows.Count; i++)
                        {
                            consignments += "" + dt_cn.Rows[i]["consignmentNumber"].ToString() + ",";
                            clvar.consignmentNo = dt_cn.Rows[i]["consignmentNumber"].ToString();
                            clvar.ChargeAmount = float.Parse(dt_cn.Rows[i]["chargedAmount"].ToString());
                            clvar.gst = float.Parse(dt_cn.Rows[i]["gst"].ToString());
                            clvar.TotalAmount = float.Parse(dt_cn.Rows[i]["totalAmount"].ToString());

                            discountAmount_ = float.Parse(dt.Rows[0]["DiscountValue"].ToString());
                            //discountGst_ = float.Parse(dt_cn.Rows[i]["gst"].ToString()) - ((float.Parse(dt_cn.Rows[i]["totalAmount"].ToString()) - float.Parse(dt.Rows[0]["DiscountValue"].ToString())) * (float.Parse(dt_cn.Rows[i]["gst"].ToString()) / 100));
                            discountGst_ = float.Parse(dt.Rows[0]["DiscountValue"].ToString()) * (float.Parse(dt_cn.Rows[i]["gst"].ToString()) / 100);
                            //clvar.gst = float.Parse(dt_cn.Rows[i]["gst"].ToString()) - (float.Parse(dt.Rows[0]["DiscountValue"].ToString()) * (float.Parse(dt_cn.Rows[i]["gst"].ToString()) / 100));

                            gst_ = float.Parse(dt_cn.Rows[i]["gst"].ToString()) - discountGst_;
                            //chargeAmount_ = float.Parse(dt_cn.Rows[i]["chargedAmount"].ToString()) - discountAmount_;
                            chargeAmount_ = (float.Parse(dt_cn.Rows[i]["totalAmount"].ToString()) - discountAmount_) + gst_;

                            DataRow dr = dt_tblDiscount.NewRow();

                            dr["consignmentNumber"] = clvar.consignmentNo;
                            dr["chargeAmount"] = chargeAmount_;
                            dr["totalAmount"] = clvar.TotalAmount;
                            //dr["gst"] = gst_;
                            dr["gst"] = clvar.gst;
                            dr["DiscountAmount"] = discountAmount_;
                            dr["DiscountGST"] = discountGst_;

                            dt_tblDiscount.Rows.Add(dr);
                        }
                    }
                }
                if (consignments.Length > 0)
                {
                    consignments = consignments.Remove(consignments.Length - 1, 1);
                }
                string error;
                //error = Add_Consignment_Discount_Tariff(clvar, chargeAmount_, gst_, discountAmount_, discountGst_);
                error = Add_Consignment_Discount_Tariff(clvar, dt_tblDiscount);

                if (error != "")
                {
                    lbl_Error.Text = error;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Save Consignment.')", true);
                    return;
                }
                else
                {
                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                    string ecnryptedConsignment = EnryptString(consignments);
                    string script = String.Format(script_, "RetailBookingReceipt.aspx?id=" + ecnryptedConsignment, "_blank", "");

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Discount Applied.')", true);

                    btn_reset_Click(sender, e);
                }

            }
        }
        public DataSet Get_PreBookingData(string TicketNo)
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string query = "SELECT * FROM MNP_PreBookingConsignment WHERE ticketNumber = '" + TicketNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        public DataSet Get_PreBookingPriceModifierData(string TicketNo)
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string query = "SELECT * FROM PreBookingConsignmentModifier WHERE consignmentNumber = '" + TicketNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        public DataSet Update_CSTableAgainstCN(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string query = "UPDATE MNP_PreBookingConsignment SET \n" +
                               "isBooked = '1', consignmentNumber ='" + clvar.consignmentNo + "', callStatus = '4' \n" +
                               "WHERE ticketNumber = '" + clvar.CouponNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
        protected void Get_TicketData(string TicketNo)
        {
            DataSet ds = Get_PreBookingData(TicketNo);

            if (ds.Tables.Count > 0)
            {
                dd_city.SelectedValue = ds.Tables[0].Rows[0]["destination"].ToString();
                txt_aWeight.Text = ds.Tables[0].Rows[0]["weight"].ToString();
                txt_Weight.Text = ds.Tables[0].Rows[0]["weight"].ToString();
                cb_ServiceType.SelectedValue = ds.Tables[0].Rows[0]["servicetypename"].ToString();
                txt_Piecies.Text = ds.Tables[0].Rows[0]["pieces"].ToString();
                txt_ConsigneeCellno.Text = ds.Tables[0].Rows[0]["consigneeCellNo"].ToString();
                txt_Consignee.Text = ds.Tables[0].Rows[0]["consignee"].ToString();
                txt_ConsigneeCNIC.Text = ds.Tables[0].Rows[0]["consigneeCNICNo"].ToString();
                txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["consignerCellNo"].ToString();
                txt_Consigner.Text = ds.Tables[0].Rows[0]["consigner"].ToString();
                Txt_ConsignerCNIC.Text = ds.Tables[0].Rows[0]["consignerCNICNo"].ToString();
                txt_Package_Handcarry.Text = ds.Tables[0].Rows[0]["pakageContents"].ToString();
                txt_Address.Text = ds.Tables[0].Rows[0]["consigneeAddress"].ToString();
                txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["consigneraddress"].ToString();
                txt_remarks.Text = ds.Tables[0].Rows[0]["specialInstructions"].ToString();
                txt_l.Text = "0";
                txt_w.Text = "0";
                txt_h.Text = "0";


                if (ds.Tables[0].Rows[0]["isInsured"].ToString() == "0")
                {
                    cb_Insurance.Checked = false;
                    lbl_InsuranceMsg.Style.Add("display", "block");
                }
                else
                {
                    cb_Insurance.Checked = true;
                    lbl_InsuranceMsg.Style.Add("display", "none");
                }
            }
        }
        public DataSet ConsignmentType()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = " SELECT ct.id, ct.name ConsignmentType FROM ConsignmentType ct  \n"
                + "       WHERE ct.[status]='1' AND ct.id='12' \n"
                + "       GROUP BY ct.name,ct.id";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }
    }
}