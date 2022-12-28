using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using Telerik.Web.UI;

namespace MRaabta.Files
{
    public partial class ExpressionConsignmentApproval : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Variable clvar1 = new Variable();
        Consignemnts con = new Consignemnts();
        CommonFunction CF = new CommonFunction();
        ExpressionConsignment EC = new ExpressionConsignment();

        protected void Page_Load(object sender, EventArgs e)
        {
            Errorid.Text = "";
            if (!IsPostBack)
            {
                btn_save.Visible = false;
                btn_update.Visible = false;
                btn_unapprove1.Visible = false;


                gv_items.DataSource = null;
                gv_items.DataBind();
                //BindCities();
                Get_Destination();
                Get_Items();

                DataTable dt_items = new DataTable();
                //   dt_items.Columns.Add("ID", typeof(string));
                dt_items.Columns.Add("id", typeof(Int64));
                dt_items.Columns.Add("consignementNo", typeof(string));
                dt_items.Columns.Add("itemId", typeof(Int64));
                dt_items.Columns.Add("itemQty", typeof(float));
                dt_items.Columns.Add("amount", typeof(float));
                dt_items.Columns.Add("message", typeof(string));
                dt_items.Columns.Add("status", typeof(string));
                dt_items.Columns.Add("createdOn", typeof(DateTime));
                dt_items.Columns.Add("createdBy", typeof(string));
                dt_items.Columns.Add("modifiedOn", typeof(DateTime));
                dt_items.Columns.Add("modifiedBy", typeof(string));
                dt_items.Columns.Add("gst", typeof(float));
                dt_items.Columns.Add("serviceCharges", typeof(float));

                ViewState["PM"] = dt_items;

                dt_DeliveryDatetime.SelectedDate = DateTime.Now.Date;
                //dt_DeliveryDatetime.MinDate = DateTime.Now.Date;
            }
        }

        public void Get_Destination()
        {
            DataTable ds = Cities_(); //CF.Branch();
            if (ds != null)
            {
                //  cb_Destination
                //  this.cb_Destination.Items.Add(new DropDownListItem("Select Destination", "0"));
                this.cb_Destination.DataTextField = "SNAME";
                this.cb_Destination.DataValueField = "BranchCode";
                this.cb_Destination.DataSource = ds.DefaultView;
                this.cb_Destination.DataBind();
                ViewState["Cities"] = ds;
            }
        }

        public void Get_Items()
        {
            DataSet ds = CF.ExpressionItems();
            if (ds.Tables.Count != 0)
            {
                //  cb_Destination
                //this.dd_item.Items.Add(new DropDownListItem("Select Item", "0"));
                dd_item.DataSource = ds.Tables[0];
                dd_item.DataTextField = "NAME";
                dd_item.DataValueField = "CODE";
                dd_item.DataBind();

                ViewState["Expressions"] = ds.Tables[0];
            }
        }


        //protected void BindCities()
        //{
        //    string query = "SELECT * FROM CITIES WHERE isActive = '1'";
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        //    try
        //    {
        //        con.Open();
        //        SqlDataAdapter sda = new SqlDataAdapter(query, con);
        //        sda.Fill(dt);


        //    }
        //    catch (Exception ex)
        //    { }
        //    finally { con.Close(); }
        //    if (dt == null)
        //    {
        //        return;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        //dd_destination.DataSource = dt;
        //        //dd_destination.DataTextField = "CITYNAME";
        //        //dd_destination.DataValueField = "ID";
        //        //dd_destination.DataBind();
        //    }
        //}
        //protected void BindItems()
        //{
        //    string query = "select CODE, code + ' -- ' + name as NAME FROM ExpressionProduct where status = '1'";
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        //    try
        //    {
        //        con.Open();
        //        SqlDataAdapter sda = new SqlDataAdapter(query, con);
        //        sda.Fill(dt);

        //        ViewState["items"] = dt;
        //    }
        //    catch (Exception ex)
        //    { }
        //    finally { con.Close(); }
        //    if (dt == null)
        //    {
        //        return;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        dd_item.DataSource = dt;
        //        dd_item.DataTextField = "NAME";
        //        dd_item.DataValueField = "CODE";
        //        dd_item.DataBind();
        //    }
        //}

        protected void btn_add_Click(object sender, EventArgs e)
        {

            //Branch Information
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }

            DataTable dt_item = (DataTable)ViewState["PM"];

            string Itemid = dd_item.SelectedValue;
            DataRow[] dr_ = dt_item.Select("itemid ='" + HiddenField2.Value + "'");
            if (dr_.Length == 0)
            {
                DataRow dr = dt_item.NewRow();
                dr[0] = Int64.Parse("0");
                dr[1] = txt_ConNo.Text;
                dr[2] = Int64.Parse(HiddenField2.Value);
                dr[3] = float.Parse(txt_qty.Text);
                dr[4] = (float.Parse(txt_amount.Text) - 100 - float.Parse(gst)) * int.Parse(txt_qty.Text);
                dr[5] = DBNull.Value;
                dr[6] = "2";
                dr[7] = DateTime.Now.ToString();
                dr[8] = DBNull.Value;
                dr[9] = DateTime.Now.ToString();
                dr[10] = DBNull.Value;
                dr[11] = float.Parse(gst);// gst;
                dr[12] = float.Parse("100");// "100";

                dt_item.Rows.Add(dr);
                dt_item.AcceptChanges();
                ViewState["PM"] = dt_item;
                LoadGrid();
            }

            double Modfier = 0;
            double TotalGstAmount = 0;

            double TotalAmount = 0;
            double TotalServiceCharges = 0;




            foreach (RepeaterItem rp in gv_items.Items)
            {
                if (rp.Visible == true)
                {
                    double Amount = 0;

                    double ServiceCharges = 0;
                    double GstAmount = 0;

                    Label tx = ((Label)rp.FindControl("lbl_Amount"));
                    Modfier = double.Parse(tx.Text);

                    ServiceCharges = 100;
                    GstAmount = (ServiceCharges / 100) * double.Parse(gst);
                    Amount = (Modfier) + ServiceCharges + GstAmount;

                    TotalServiceCharges += ServiceCharges;
                    TotalGstAmount += GstAmount;
                    TotalAmount += Amount;
                }


            }

            txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
            txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
            txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
            txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);

        }

        protected void txt_ConNo_TextChanged(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;

            DataSet ds = GetConsignmentForApproval(clvar);
            DataTable dt = ConsignmentExpressionDetail(clvar);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables[0].Rows[0]["isApproved"].ToString().ToUpper() == "TRUE" || ds.Tables[0].Rows[0]["isApproved"].ToString() == "1")
                {
                    txt_approveStatus.Text = "APPROVED";
                }
                else
                {
                    txt_approveStatus.Text = "UNAPPROVED";
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["invoiceNumber_"].ToString() != "")
                    {
                        txt_invoiceNumber.Text = dr["invoiceNumber_"].ToString();
                        break;
                    }
                    else
                    {
                        txt_invoiceNumber.Text = "";
                    }
                }
                if (ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString() == null || ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString().Trim() == "")
                {
                    dt_DeliveryDatetime.SelectedDate = DateTime.Now;
                }
                else
                {
                    dt_DeliveryDatetime.SelectedDate = DateTime.Parse(ds.Tables[0].Rows[0]["ExpressionDeliveryDateTime"].ToString());
                }

                txt_ConNo.Text = ds.Tables[0].Rows[0]["consignmentNumber"].ToString();
                txt_AccNo.Text = ds.Tables[0].Rows[0]["consignerAccountNo"].ToString();

                if (ds.Tables[0].Rows[0]["customerType"].ToString() == "1")
                {
                    rbtn_customerType.SelectedValue = "1";
                }

                if (ds.Tables[0].Rows[0]["customerType"].ToString() == "2")
                {
                    rbtn_customerType.SelectedValue = "2";
                }

                if (ds.Tables[0].Rows[0]["destinationExpressCenterCode"].ToString() != "")
                {
                    cb_Destination.SelectedValue = ds.Tables[0].Rows[0]["Destination"].ToString();// ds.Tables[0].Rows[0]["destinationExpressCenterCode"].ToString();
                } //    txt_consignmentRefNo.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_RiderCode.Text = ds.Tables[0].Rows[0]["riderCode"].ToString();
                //  hd_OriginExpressCenter = ds.Tables[0].Rows[0][""].ToString();
                txt_pieces.Text = ds.Tables[0].Rows[0]["pieces"].ToString();
                txt_otherCharges.Text = ds.Tables[0].Rows[0]["otherCharges"].ToString();
                txt_packageContents.Text = ds.Tables[0].Rows[0]["PakageContents"].ToString();

                txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["consignerCellNo"].ToString();
                Txt_ConsignerCNIC.Text = ds.Tables[0].Rows[0]["consignerCNICNo"].ToString();
                txt_Consigner.Text = ds.Tables[0].Rows[0]["consigner"].ToString();
                txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["shipperAddress"].ToString();

                txt_Consignee.Text = ds.Tables[0].Rows[0]["consignee"].ToString();
                txt_Address.Text = ds.Tables[0].Rows[0]["address"].ToString();
                // dd_consigneeArea.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_ConsigneeCellno.Text = ds.Tables[0].Rows[0]["consigneePhoneNo"].ToString();

                txt_chargedRate.Text = ds.Tables[0].Rows[0]["chargedAmount"].ToString();
                //   txt_chargesWalaOtherCharges.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_gst.Text = ds.Tables[0].Rows[0]["gst"].ToString();
                txt_totalAmount.Text = ds.Tables[0].Rows[0]["totalAmount"].ToString();
                txt_message.Text = ds.Tables[0].Rows[0]["expressionMessage"].ToString();
                txt_consignmentRefNo.Text = ds.Tables[0].Rows[0]["ExpressionConsignmentRefNumber"].ToString();
                booking_date.Text = DateTime.Parse(ds.Tables[0].Rows[0]["bookingDate"].ToString()).ToString("yyyy-MM-dd");
                DateTime tempdate = DateTime.Today;
                DateTime.TryParse(ds.Tables[0].Rows[0]["AccountReceivingDate"].ToString(), out tempdate);
                txt_reportingDate.Text = tempdate.ToString("yyyy-MM-dd");

                //booking_date.Enabled = false;

                //txt_message.Text = ds.Tables[0].Rows[0]["expressionDeliveryDateTime"].ToString();
                //dt_DeliveryDatetime

                //for (int i = 0; i <= dt.Rows.Count; i++)
                //{
                //    gv_items.DataSource = dt.DefaultView;
                //    gv_items.DataBind();
                //}

                DataTable dt_item = (DataTable)ViewState["PM"];
                dt_item.Clear();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt_item.NewRow();

                    dr[0] = dt.Rows[i]["id"].ToString();
                    dr[1] = dt.Rows[i]["consignementno"].ToString();
                    dr[2] = dt.Rows[i]["itemid"].ToString();
                    dr[3] = float.Parse(dt.Rows[i]["itemqty"].ToString());
                    dr[4] = float.Parse(dt.Rows[i]["amount"].ToString());
                    dr[5] = dt.Rows[i]["message"].ToString();
                    dr[6] = dt.Rows[i]["status"].ToString();
                    dr[7] = dt.Rows[i]["createdon"].ToString();
                    dr[8] = dt.Rows[i]["createdby"].ToString();
                    // dr[9] = dt.Rows[i]["modifiedon"].ToString();
                    if (dt.Rows[i]["modifiedon"].ToString() != "")
                    {
                        dr[9] = dt.Rows[i]["modifiedon"].ToString();
                    }
                    else
                    {
                        dr[9] = DBNull.Value;
                    }
                    dr[10] = dt.Rows[i]["modifiedby"].ToString();
                    dr[11] = float.Parse(dt.Rows[i]["gst"].ToString());// gst;
                    dr[12] = float.Parse("100");// "100";


                    dt_item.Rows.Add(dr);
                    dt.AcceptChanges();
                    ViewState["PM"] = dt_item;

                }

                gv_items.DataSource = dt.DefaultView;
                gv_items.DataBind();

                btn_save.Visible = false;
                btn_update.Visible = true;
                btn_unapprove1.Visible = true;

            }
            else
            {
                btn_save.Visible = true;
                btn_update.Visible = false;
                btn_unapprove1.Visible = false;

                txt_invoiceNumber.Text = "";
                txt_approveStatus.Text = "";
                //    txt_ConNo.Text = "";
                txt_AccNo.Text = "";

                rbtn_customerType.SelectedValue = "";

                cb_Destination.SelectedValue = "0";
                //    txt_consignmentRefNo.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_RiderCode.Text = "";
                //  hd_OriginExpressCenter = ds.Tables[0].Rows[0][""].ToString();
                txt_pieces.Text = "";
                txt_otherCharges.Text = "";
                txt_packageContents.Text = "";

                txt_ConsignerCellNo.Text = "";
                Txt_ConsignerCNIC.Text = "";
                txt_Consigner.Text = "";
                txt_ShipperAddress.Text = "";

                txt_Consignee.Text = "";
                txt_Address.Text = "";
                // dd_consigneeArea.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_ConsigneeCellno.Text = "";

                txt_chargedRate.Text = "";
                //   txt_chargesWalaOtherCharges.Text = ds.Tables[0].Rows[0][""].ToString();
                txt_gst.Text = "";
                txt_totalAmount.Text = "";
                txt_message.Text = "";
                booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txt_reportingDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //ViewState["PM"] = null;
                gv_items.DataSource = null;
                gv_items.DataBind();



                txt_invoiceNumber.Text = "";
                txt_approveStatus.Text = "";
            }
            txt_AccNo.Focus();
        }

        /*
        protected void RadGrid1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable dt = EC.ConsignmentExpressionDetail(clvar);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                (e.Item.FindControl("lbl_qty") as Literal).Text = dt.Rows[i]["itemQty"].ToString();
            }
        }
        */
        protected void txt_AccNo_TextChanged(object sender, EventArgs e)
        {
            if (txt_AccNo.Text != "")
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                clvar.AccountNo = txt_AccNo.Text;
                DataSet ds = CustomerInformation(clvar);
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        //txt_Consigner.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                        //txt_ConsignerCellNo.Text = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                        //txt_ShipperAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                        hd_CreditClientID.Value = ds.Tables[0].Rows[0]["id"].ToString();

                        if (ds.Tables[0].Rows[0]["accountNo"].ToString() == "0")
                        {
                            this.rbtn_customerType.SelectedValue = "1";
                        }
                        else
                        {
                            this.rbtn_customerType.SelectedValue = "2";
                        }
                        cb_Destination.Focus();
                    }
                    else
                    {
                        txt_AccNo.Text = "";
                        return;

                    }
                }
                else
                {
                    txt_AccNo.Text = "";
                    return;

                }
            }
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

        protected void cb_Destination_ItemSelected(object sender, EventArgs e)
        {

        }

        protected void txt_RiderCode_TextChanged(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();// cb_Origin.SelectedValue;

            DataSet Rider = con.RiderInformation(clvar);
            if (Rider.Tables[0].Rows.Count != 0)
            {
                hd_OriginExpressCenter.Value = Rider.Tables[0].Rows[0]["expresscenterid"].ToString();
                booking_date.Focus();
            }
            else
            {
                txt_RiderCode.Text = "";
                txt_RiderCode.Focus();
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!IsNumeric(txt_ConNo.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                Errorid.Text = "Only Numerics allowed in Consignment Number";
                return;
            }
            if (txt_ConNo.Text.Length > 15 || txt_ConNo.Text.Length < 11)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number must be between 11 and 15 digits.')", true);
                Errorid.Text = "Consignment Number must be between 11 and 15 digits.";
                return;
            }
            if (txt_reportingDate.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Account Receving Date')", true);
                return;
            }

            if (txt_approveStatus.Text.ToUpper() == "APPROVED")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Already Approved')", true);
                Errorid.Text = "Consignment Already Approved";
                return;
            }
            if (txt_invoiceNumber.Text.Trim() != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Invoiced. Cannot Update')", true);
                Errorid.Text = "Consignment Invoiced. Cannot Update";
                return;
            }
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (DateTime.Parse(booking_date.Text) < minAllowedDate || DateTime.Parse(booking_date.Text) > maxAllowedDate || DateTime.Parse(txt_reportingDate.Text) < minAllowedDate || DateTime.Parse(txt_reportingDate.Text) > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot Approve CN')", true);
                return;
            }
            if (DateTime.Parse(booking_date.Text) > DateTime.Now || DateTime.Parse(txt_reportingDate.Text) > DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date')", true);
                booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                return;
            }

            if (DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString()) < minAllowedDate || DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString()) < DateTime.Parse(booking_date.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Delivery Date')", true);
                dt_DeliveryDatetime.SelectedDate = DateTime.Now;
                return;
            }

            if (cb_Destination.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
                return;
            }
            DataTable cities = ViewState["Cities"] as DataTable;
            txt_AccNo_TextChanged(this, e);
            if (txt_AccNo.Text == "")
            {
                return;
            }
            Errorid.Text = "";
            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;
            clvar.AccountNo = txt_AccNo.Text;
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.Destination = cb_Destination.SelectedValue;// cities.Select("expressCenterCode = '" + cb_Destination.SelectedValue + "'")[0]["bid"].ToString();//cb_Destination.SelectedValue;// hd_Destination.Value;
            clvar.ServiceTypeName = "Expressions";//cb_ServiceType.SelectedValue;
            clvar.Weight = 0;//float.Parse(txt_Weight.Text);
            clvar.Unit = 1;// cb_Unit.SelectedValue;
                           //clvar.pieces = 1;// txt_Piecies.Text;
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();

            //Consignee and Consigner Information
            clvar.Consignee = txt_Consignee.Text;
            clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
            clvar.ConsigneeCNIC = "";// txt_ConsigneeCNIC.Text;
            clvar.ConsigneeAddress = txt_Address.Text;
            clvar.Consigner = txt_Consigner.Text;
            clvar.ConsignerCell = txt_ConsignerCellNo.Text;
            clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
            clvar.ConsignerAddress = txt_ShipperAddress.Text;
            clvar.consignerAccountNo = txt_AccNo.Text;
            clvar.expressionconsignmentRefNumber = txt_consignmentRefNo.ToString();

            // clvar.Bookingdate = DateTime.Parse(booking_date.Text.ToString());  //DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());
            clvar.BookingDate = booking_date.Text.ToString();


            //  clvar.Bookingdate = DateTime.Parse(booking_date.DbSelectedDate.ToString());  //DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();// cb_Origin.SelectedValue;
            clvar.Insurance = "";//txt_insurance.Text;
            clvar.Othercharges = 0;// txt_Othercharges.Text;
            clvar.Day = "";// rb_1.SelectedValue;
            clvar.expresscenter = hd_OriginExpressCenter.Value;
            clvar.destinationExpressCenterCode = cities.Select("BranchCode = '" + cb_Destination.SelectedValue + "'")[0]["ECCode"].ToString();//cb_Destination.SelectedValue;
            clvar.CustomerClientID = hd_CreditClientID.Value;
            clvar.Con_Type = 10;

            clvar.expressionDeliveryDateTime = DateTime.Parse(dt_DeliveryDatetime.DbSelectedDate.ToString());
            clvar.expressionGreetingCard = chk_greetingCard.Checked;
            clvar.expressionMessage = txt_message.Text;
            clvar.expressionconsignmentRefNumber = txt_consignmentRefNo.Text;
            //clvar.routeCode = dd_consigneeArea.Text;

            //Branch Information
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            //Calculating Modifier

            double Modfier = 0;
            double TotalGstAmount = 0;

            double TotalAmount = 0;
            double TotalServiceCharges = 0;
            //Calculating InsuranceAmount
            double insper = 0;

            clvar.pieces = 0;
            foreach (RepeaterItem rp in gv_items.Items)
            {
                if (rp.Visible == true)
                {
                    double Amount = 0;

                    double ServiceCharges = 0;
                    double GstAmount = 0;

                    Label tx = ((Label)rp.FindControl("lbl_Amount"));
                    Modfier = double.Parse(tx.Text);

                    ServiceCharges = 100;
                    GstAmount = (ServiceCharges / 100) * double.Parse(gst);
                    Amount = (Modfier) + ServiceCharges + GstAmount;

                    TotalServiceCharges += ServiceCharges;
                    TotalGstAmount += GstAmount;
                    TotalAmount += Amount;
                }


            }

            txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
            txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
            txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
            txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);

            clvar.TotalAmount = float.Parse(txt_totalAmount.Text) - float.Parse(txt_gst.Text); ;// TotalAmount;
            clvar.gst = float.Parse(txt_gst.Text);
            clvar.ChargeAmount = 0;
            clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);
            DataTable dt = (DataTable)ViewState["PM"];
            if (dt.Rows.Count != 0)
            {
                clvar.isPM = true;
                clvar.expresssion = dt;
            }
            else
            {
                clvar.isPM = false;
                clvar.expresssion = null;
            }

            int tempPieces = 0;
            int.TryParse(dt.Compute("SUM(itemQty)", "").ToString(), out tempPieces);
            clvar.pieces = tempPieces;
            clvar.destinationCountryCode = "PAK";

            //con.Add_Consignment(clvar);
            clvar.cnScreenId = "11";
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.createdBy = HttpContext.Current.Session["U_ID"].ToString();
            if (dt.Rows.Count > 0)
            {

            }
            //clvar.pieces = int.Parse(dt.Compute("SUM(itemQty)", "").ToString());
            clvar.PakageContents = txt_packageContents.Text;
            string error = Add_Consignment(clvar);
            if (error != "")
            {
                Errorid.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment.')", true);

                return;
            }

            string error1 = Update_ExpressionConsignmentApproval2(clvar);

            error = "";
            error = con.WriteToDatabase_(clvar);
            if (error != "")
            {
                error = con.DeleteConsignment(clvar);
                if (error != "")
                {

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support. Error: " + error + "')", true);
                    return;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. Error = '" + error + "'.')", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Saved Successfully')", true);
            txt_ConNo_TextChanged(this, e);

        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            if (!IsNumeric(txt_ConNo.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                Errorid.Text = "Only Numerics allowed in Consignment Number";
                return;
            }
            if (txt_ConNo.Text.Length > 15 || txt_ConNo.Text.Length < 11)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number must be between 11 and 15 digits.')", true);
                Errorid.Text = "Consignment Number must be between 11 and 15 digits.";
                return;
            }
            if (txt_reportingDate.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Account Receving Date')", true);
                return;
            }

            if (txt_approveStatus.Text.ToUpper() == "APPROVED")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Already Approved. Cannot Update')", true);
                Errorid.Text = "Consignment Already Approved";
                return;
            }
            if (txt_invoiceNumber.Text.Trim() != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Invoiced. Cannot Update')", true);
                Errorid.Text = "Consignment Invoiced. Cannot Update";
                return;
            }

            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (DateTime.Parse(booking_date.Text) < minAllowedDate || DateTime.Parse(booking_date.Text) > maxAllowedDate || DateTime.Parse(txt_reportingDate.Text) < minAllowedDate || DateTime.Parse(txt_reportingDate.Text) > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot Approve CN')", true);
                return;
            }
            if (cb_Destination.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
                return;
            }
            txt_RiderCode_TextChanged(this, e);
            if (txt_RiderCode.Text.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Rider Code')", true);
                Errorid.Text = "Invalid Rider Code";
                return;
            }
            DataTable cities = ViewState["Cities"] as DataTable;
            if (DateTime.Parse(booking_date.Text) > DateTime.Now || DateTime.Parse(txt_reportingDate.Text) > DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date')", true);
                booking_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                return;
            }
            if (DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString()) < minAllowedDate || DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString()) < DateTime.Parse(booking_date.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Delivery Date')", true);
                dt_DeliveryDatetime.SelectedDate = DateTime.Now;
                return;
            }

            if (cb_Destination.SelectedItem == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
                return;
            }
            txt_AccNo_TextChanged(this, e);
            if (txt_AccNo.Text == "")
            {
                return;
            }
            Errorid.Text = "";
            clvar = new Cl_Variables();
            clvar.consignmentNo = txt_ConNo.Text;
            clvar.AccountNo = txt_AccNo.Text;
            clvar.RiderCode = txt_RiderCode.Text;
            clvar.Destination = cb_Destination.SelectedValue;// cities.Select("expressCenterCode = '" + cb_Destination.SelectedValue + "'")[0]["bid"].ToString();// cb_Destination.SelectedValue;// hd_Destination.Value;
            clvar.ServiceTypeName = "Expressions";//cb_ServiceType.SelectedValue;
            clvar.Weight = 0;//float.Parse(txt_Weight.Text);
            clvar.Unit = 1;// cb_Unit.SelectedValue;
                           //   clvar.pieces = 1;// txt_Piecies.Text;
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();

            //  string qty = txt_qty.Text;




            //for (int i = 0; i <= txt_qty.Text.Count;   )

            //Consignee and Consigner Information
            clvar.Consignee = txt_Consignee.Text;
            clvar.ConsigneeCell = txt_ConsigneeCellno.Text;
            clvar.ConsigneeCNIC = "";// txt_ConsigneeCNIC.Text;
            clvar.ConsigneeAddress = txt_Address.Text;
            clvar.Consigner = txt_Consigner.Text;
            clvar.ConsignerCell = txt_ConsignerCellNo.Text;
            clvar.ConsignerCNIC = Txt_ConsignerCNIC.Text;
            clvar.ConsignerAddress = txt_ShipperAddress.Text;
            clvar.consignerAccountNo = txt_AccNo.Text;

            //    clvar.Bookingdate = DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString());

            clvar.BookingDate = booking_date.Text.ToString();


            // clvar.origin = "4";// cb_Origin.SelectedValue;
            clvar.origin = Session["BranchCode"].ToString();
            clvar.Insurance = "";//txt_insurance.Text;
            clvar.Othercharges = 0;// txt_Othercharges.Text;
            clvar.Day = "";// rb_1.SelectedValue;
            clvar.expresscenter = hd_OriginExpressCenter.Value;
            clvar.destinationExpressCenterCode = cities.Select("BranchCode = '" + cb_Destination.SelectedValue + "'")[0]["ECCode"].ToString();// cb_Destination.SelectedValue;
            clvar.CustomerClientID = hd_CreditClientID.Value;
            clvar.Con_Type = 10;

            clvar.expressionDeliveryDateTime = DateTime.Parse(dt_DeliveryDatetime.DbSelectedDate.ToString());
            clvar.expressionGreetingCard = chk_greetingCard.Checked;
            clvar.expressionMessage = txt_message.Text;
            clvar.expressionconsignmentRefNumber = txt_consignmentRefNo.Text;
            //clvar.routeCode = dd_consigneeArea.Text;

            //Branch Information
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            //Calculating Modifier
            double Modfier = 0;
            double TotalGstAmount = 0;

            double TotalAmount = 0;
            double TotalServiceCharges = 0;


            foreach (RepeaterItem rp in gv_items.Items)
            {
                if (rp.Visible == true)
                {
                    double Amount = 0;

                    double ServiceCharges = 0;
                    double GstAmount = 0;

                    Label tx = ((Label)rp.FindControl("lbl_Amount"));
                    Modfier = double.Parse(tx.Text);

                    ServiceCharges = 100;
                    GstAmount = (ServiceCharges / 100) * double.Parse(gst);
                    Amount = (Modfier) + ServiceCharges + GstAmount;

                    TotalServiceCharges += ServiceCharges;
                    TotalGstAmount += GstAmount;
                    TotalAmount += Amount;
                }


            }
            txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
            txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
            txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
            txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);
            //Calculating InsuranceAmount
            double insper = 0;
            clvar.TotalAmount = float.Parse(txt_totalAmount.Text) - float.Parse(txt_gst.Text); ;// TotalAmount;
            clvar.gst = float.Parse(txt_gst.Text);
            clvar.ChargeAmount = 0;
            DataTable dt = (DataTable)ViewState["PM"];
            if (dt.Rows.Count != 0)
            {
                /*
                for (int i = 0; i <= dt.Rows[0]["itemqty"].ToString().Count(); i++)
                {
                    clvar.pieces += Int32.Parse(dt.Rows[i]["itemqty"].ToString());
                }
                    */
                clvar.isPM = true;
                clvar.expresssion = dt;
            }
            else
            {

                clvar.isPM = false;
                clvar.expresssion = null;
            }
            int tempPieces = 0;
            int.TryParse(dt.Compute("SUM(itemQty)", "").ToString(), out tempPieces);
            clvar.pieces = tempPieces;

            clvar.destinationCountryCode = "PAK";
            clvar.Customertype = int.Parse(rbtn_customerType.SelectedValue);

            //con.Add_Consignment(clvar);
            clvar.CustomerClientID = hd_CreditClientID.Value;



            string error = Update_ExpressionConsignmentApproval(clvar);
            if (error != "")
            {
                Errorid.Text = error;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment.')", true);

                return;
            }
            error = "";
            error = con.WriteToDatabase_(clvar);
            if (error != "")
            {
                //error = con.DeleteConsignment(clvar);
                if (error != "")
                {

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not Save. Please Contact IT Support. Error: " + error + "')", true);
                    return;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Consignment. Error = '" + error + "'.')", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Update Successfully')", true);
            txt_ConNo_TextChanged(this, e);
        }

        protected void dd_item_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Expressions"];
            if (dd_item.SelectedValue != "0")
            {
                DataRow[] Dr = dt.Select("code='" + dd_item.SelectedValue + "'");
                if (Dr.Length != 0)
                {
                    txt_qty.Text = "1";
                    txt_amount.Text = Dr[0]["rate"].ToString();
                    HiddenField2.Value = Dr[0]["id"].ToString();
                }
            }
        }

        public void LoadGrid()
        {
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
            DataSet BranchGSTInformation = con.BranchGSTInformation(clvar);
            string gst = "";
            if (BranchGSTInformation.Tables[0].Rows.Count != 0)
            {
                gst = BranchGSTInformation.Tables[0].Rows[0]["gst"].ToString();
            }
            double Modfier = 0;
            double TotalGstAmount = 0;

            double TotalAmount = 0;
            double TotalServiceCharges = 0;
            DataTable dt = (DataTable)ViewState["PM"];
            if (dt.Rows.Count > 0)
            {
                gv_items.DataSource = dt.DefaultView;
                gv_items.DataBind();


                foreach (RepeaterItem rp in gv_items.Items)
                {
                    if (rp.Visible == true)
                    {
                        double Amount = 0;

                        double ServiceCharges = 0;
                        double GstAmount = 0;

                        Label tx = ((Label)rp.FindControl("lbl_Amount"));
                        Modfier = double.Parse(tx.Text);

                        ServiceCharges = 100;
                        GstAmount = (ServiceCharges / 100) * double.Parse(gst);
                        Amount = (Modfier) + ServiceCharges + GstAmount;

                        TotalServiceCharges += ServiceCharges;
                        TotalGstAmount += GstAmount;
                        TotalAmount += Amount;
                    }


                }

                txt_chargedRate.Text = string.Format("{0:N2}", TotalAmount - TotalGstAmount - TotalServiceCharges);
                txt_chargesWalaOtherCharges.Text = string.Format("{0:N2}", TotalServiceCharges);
                txt_gst.Text = string.Format("{0:N2}", TotalGstAmount);
                txt_totalAmount.Text = string.Format("{0:N2}", TotalAmount);
            }
            else
            {
                gv_items.DataSource = null;
                gv_items.DataBind();
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable Expressions = (DataTable)ViewState["Expressions"];
                string Itemid = (e.Item.FindControl("Hd_ID") as HiddenField).Value;
                HiddenField hd_status = (e.Item.FindControl("hd_status") as HiddenField);
                DataRow[] dr = Expressions.Select("id ='" + Itemid + "'");
                if (dr.Length != 0)
                {
                    (e.Item.FindControl("lbl_ItemDescription") as Label).Text = dr[0]["name"].ToString();
                }
                if (hd_status.Value == "0")
                {
                    e.Item.Visible = false;
                }
            }
            else
            {
                /*
                clvar.consignmentNo = txt_ConNo.Text;
                DataTable dt = EC.ConsignmentExpressionDetail(clvar);

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    (e.Item.FindControl("lbl_qty") as Label).Text = dt.Rows[i]["itemQty"].ToString();

                    DataRow dr = dt.NewRow();
                    dr[0] = Int64.Parse("0");
                    dr[1] = txt_ConNo.Text;
                    dr[2] = Int64.Parse(HiddenField2.Value);
                    dr[3] = float.Parse(txt_qty.Text);
                    dr[4] = float.Parse(txt_amount.Text) - 100 - float.Parse(dt.Rows[i]["gst"].ToString());
                    dr[5] = DBNull.Value;
                    dr[6] = DBNull.Value;
                    dr[7] = DateTime.Now.ToString();
                    dr[8] = DBNull.Value;
                    dr[9] = DateTime.Now.ToString();
                    dr[10] = DBNull.Value;
                    dr[11] = float.Parse(dt.Rows[i]["gst"].ToString());// gst;
                    dr[12] = float.Parse("100");// "100";

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    ViewState["PM"] = dt;


                }
                 */
            }
        }

        protected void RadGrid1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string Itemid = (e.Item.FindControl("Hd_ID") as HiddenField).Value;

                DataTable dt = (DataTable)ViewState["PM"];

                DataRow dr = dt.Select("itemid ='" + Itemid + "' and status <> '0'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    if (dr["status"].ToString() == "1")
                    {
                        dr["Status"] = "0";
                    }
                    else
                    {
                        dr.Delete();//changes the Product_name
                    }

                }
                dt.AcceptChanges();
                ViewState["PM"] = dt;

                LoadGrid();
            }
        }
        protected void dt_DeliveryDatetime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            //DateTime dt = dt_DeliveryDatetime.SelectedDate.Value;
            //if (DateTime.Parse(dt_DeliveryDatetime.SelectedDate.ToString()) > DateTime.Now)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Delivery Date')", true);
            //    dt_DeliveryDatetime.SelectedDate = DateTime.Now;
            //    return;
            //}
        }
        protected void dd_item_TemplateNeeded_2(object sender, RadComboBoxItemEventArgs e)
        {

        }

        protected void dd_item_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Expressions"];
            if (dd_item.SelectedValue != "0")
            {
                DataRow[] Dr = dt.Select("code='" + dd_item.SelectedValue + "'");
                if (Dr.Length != 0)
                {
                    txt_qty.Text = "1";
                    txt_amount.Text = Dr[0]["rate"].ToString();
                    HiddenField2.Value = Dr[0]["id"].ToString();
                }
            }
        }

        public string Update_ExpressionConsignmentApproval(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;

            try
            {
                string sql = "update Consignment set \n"
                               + "		    [consigner] = '" + obj.Consigner + "' , \n"
                               + "		    [consignee]  = '" + obj.Consignee + "' , \n"
                               + "		    [couponNumber]  = '" + obj.CouponNo + "' , \n"
                               + "		    [customerType]  = '" + obj.Customertype + "' , \n"
                               + "		    [orgin]  = '" + obj.origin + "' , \n"
                               + "		    [destination]  = '" + obj.Destination + "' , \n"
                               + "		    [pieces]  = '" + obj.pieces + "' , \n"
                               + "		    [serviceTypeName]  = '" + obj.ServiceTypeName + "' , \n"
                               //  + "		    [creditClientId] = '" + obj.CustomerClientID + "' , \n"
                               //  + "		    [weight] = '" + obj.Weight + "' , \n"
                               //  + "		    [weightUnit] = '" + obj.Unit + "' , \n"
                               //   + "		    [discount] = '" + obj.Discount + "' , \n"
                               + "		    [cod] = '0' , \n"
                               + "		    [address] = '" + obj.ConsigneeAddress + "' , \n"
                               + "		    [modifiedBy]  = '" + obj.createdBy + "' , \n"
                               + "          [modifiedOn]  = GETDATE() ,\n"
                               + "		    [status]  = '" + obj.status + "' , \n"
                               + "		    [totalAmount] = '" + obj.TotalAmount + "' , \n"
                               + "		    [zoneCode] = '" + obj.Zone + "' , \n"
                               + "		    [branchCode]  = '" + obj.origin + "' , \n"
                               + "		    [expressCenterCode] = '" + obj.expresscenter + "' , \n"
                               //  + "		    [syncStatus] = '"++"' , \n"
                               + "		    [consignmentTypeId] = '" + obj.Con_Type + "' , \n"
                               + "		    [isCreatedFromMMU] = '" + obj.isCreatedFromMMUs + "' , \n"
                               + "		    [deliveryType] = '" + obj.deliveryType + "' , \n"
                               + "		    [remarks] = '" + obj.Remarks + "' , \n"
                               + "		    [shipperAddress] = '" + obj.ConsignerAddress + "' , \n"
                               + "		    [riderCode] = '" + obj.RiderCode + "' , \n"
                               + "		    [gst] = '" + obj.gst + "' , \n"
                               + "		    [width] = '" + obj.width + "' , \n"
                               + "		    [breadth] = '" + obj.breadth + "' , \n"
                               + "		    [height] = '" + obj.height + "' , \n"
                               + "		    [PakageContents] = '" + obj.PakageContents + "' , \n"
                               + "		    [expressionDeliveryDateTime] = '" + obj.expressionDeliveryDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [expressionGreetingCard] = '" + obj.expressionGreetingCard + "' , \n"
                               + "		    [expressionMessage] = '" + obj.expressionMessage + "' , \n"
                               + "		    [consigneePhoneNo] = '" + obj.ConsigneeCell + "' , \n"
                               + "		    [expressionconsignmentRefNumber] = '" + obj.expressionconsignmentRefNumber + "' , \n"
                               + "		    [otherCharges] = '" + obj.Othercharges + "' , \n"
                               + "		    [routeCode] = '" + obj.routeCode + "' , \n"
                               + "		    [docPouchNo] = '" + obj.docPouchNo + "' , \n"
                               + "		    [consignerPhoneNo] = '" + obj.ConsignerPhone + "' , \n"
                               + "		    [consignerCellNo] = '" + obj.ConsignerCell + "' , \n"
                               + "		    [consignerCNICNo] = '" + obj.ConsignerCNIC + "' , \n"
                               + "		    [consignerAccountNo] = '" + obj.consignerAccountNo + "' , \n"
                               + "		    [CreditClientID] = '" + obj.CustomerClientID + "' , \n"
                               + "		    [consignerEmail] = '" + obj.consignerEmail + "' , \n"
                               + "		    [docIsHomeDelivery] = '" + obj.docIsHomeDelivery + "' , \n"
                               + "		    [cutOffTime] = '" + obj.cutOffTime.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [destinationCountryCode] = '" + obj.destinationCountryCode + "' , \n"
                               + "		    [decalaredValue] = '" + obj.Declaredvalue + "' , \n"
                               + "		    [insuarancePercentage] = '" + obj.insuarancePercentage + "' , \n"
                               + "		    [consignmentScreen] = '11' , \n"
                               + "		    [isInsured] = '" + obj.isInsured + "' , \n"
                               + "		    [isReturned] = '" + obj.isReturned + "' , \n"
                               + "		    [consigneeCNICNo] = '" + obj.ConsigneeCNIC + "' , \n"
                               + "		    [cutOffTimeShift] = '" + obj.cutOffTimeShift.ToString("yyyy-MM-dd hh:mm:ss") + "' , \n"
                               + "		    [bookingDate] = '" + obj.BookingDate + "' , \n"
                               + "		    [cnClientType] = '" + obj.cnClientType + "' , \n"
                               + "		    [destinationExpressCenterCode] = '" + obj.destinationExpressCenterCode + "' , \n"
                               + "		    [originExpressCenter] = '" + obj.expresscenter + "' , \n"
                               + "		    [receivedFromRider] = '" + obj.receivedFromRider + "' , \n"
                               + "		    [chargedAmount] = '" + obj.ChargeAmount + "' , \n"
                               + "		    [isApproved] = '1' , \n"
                               + "		    [isPriceComputed] = '0' , \n"
                               + "		    [accountReceivingDate] = '" + DateTime.Parse(txt_reportingDate.Text).ToString("yyyy-MM-dd") + "' \n"
                               + "          Where CONSIGNMENTNUMBER = '" + obj.consignmentNo + "' ";


                SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
                SqlTransaction trans;
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                try
                {
                    sqlcmd.CommandText = sql;
                    sqlcmd.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    sqlcon.Close();
                }
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
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public string Add_Consignment(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon2());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
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
                //   sqlcmd.Parameters.AddWithValue("@cod", obj.StateID);
                sqlcmd.Parameters.AddWithValue("@address", obj.ConsigneeAddress);
                sqlcmd.Parameters.AddWithValue("@createdBy", obj.createdBy);
                sqlcmd.Parameters.AddWithValue("@status", obj.status);
                sqlcmd.Parameters.AddWithValue("@totalAmount", obj.TotalAmount);
                sqlcmd.Parameters.AddWithValue("@zoneCode", obj.Zone);
                sqlcmd.Parameters.AddWithValue("@branchCode", obj.origin);
                sqlcmd.Parameters.AddWithValue("@expressCenterCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@isCreatedFromMMU", "0");
                sqlcmd.Parameters.AddWithValue("@deliveryType", obj.deliveryType);
                sqlcmd.Parameters.AddWithValue("@remarks", obj.Remarks);
                sqlcmd.Parameters.AddWithValue("@shipperAddress", obj.ConsignerAddress);
                sqlcmd.Parameters.AddWithValue("@riderCode", obj.RiderCode);
                sqlcmd.Parameters.AddWithValue("@gst", obj.gst);
                sqlcmd.Parameters.AddWithValue("@width", obj.width);
                sqlcmd.Parameters.AddWithValue("@breadth", obj.breadth);
                sqlcmd.Parameters.AddWithValue("@height", obj.height);
                sqlcmd.Parameters.AddWithValue("@PakageContents", obj.PakageContents);
                sqlcmd.Parameters.AddWithValue("@expressionDeliveryDateTime", obj.expressionDeliveryDateTime);
                sqlcmd.Parameters.AddWithValue("@expressionGreetingCard", "0");
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
                sqlcmd.Parameters.AddWithValue("@docIsHomeDelivery", "0");
                sqlcmd.Parameters.AddWithValue("@cutOffTime", obj.cutOffTime);
                sqlcmd.Parameters.AddWithValue("@destinationCountryCode", obj.destinationCountryCode);
                sqlcmd.Parameters.AddWithValue("@decalaredValue", obj.Declaredvalue);
                sqlcmd.Parameters.AddWithValue("@insuarancePercentage", obj.insuarancePercentage);
                sqlcmd.Parameters.AddWithValue("@consignmentScreen", obj.cnScreenId);
                sqlcmd.Parameters.AddWithValue("@isInsured", "0");
                sqlcmd.Parameters.AddWithValue("@isReturned", "0");
                sqlcmd.Parameters.AddWithValue("@consigneeCNICNo", obj.ConsigneeCNIC);
                sqlcmd.Parameters.AddWithValue("@cutOffTimeShift", obj.cutOffTimeShift);
                sqlcmd.Parameters.AddWithValue("@bookingDate", obj.BookingDate);
                sqlcmd.Parameters.AddWithValue("@cnClientType", obj.cnClientType);
                sqlcmd.Parameters.AddWithValue("@destinationExpressCenterCode", obj.destinationExpressCenterCode);
                sqlcmd.Parameters.AddWithValue("@dayType", obj.dayType);
                sqlcmd.Parameters.AddWithValue("@originExpressCenter", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@receivedFromRider", "0");
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
                //    sqlcmd.Parameters.AddWithValue("@chargeCODAmount", obj.chargeCODAmount);
                sqlcmd.Parameters.AddWithValue("@codAmount", obj.codAmount);
                //sqlcmd.Parameters.AddWithValue("@isPM", obj.isPM);
                //sqlcmd.Parameters.AddWithValue("@LstModifiersCN", obj.LstModifiersCNt);
                sqlcmd.Parameters.AddWithValue("@serviceTypeId", obj.serviceTypeId);
                sqlcmd.Parameters.AddWithValue("@originId", obj.originId);
                sqlcmd.Parameters.AddWithValue("@destId", obj.destId);
                sqlcmd.Parameters.AddWithValue("@cnTypeId", obj.cnTypeId);
                sqlcmd.Parameters.AddWithValue("@cnOperationalType", obj.cnOperationalType);
                sqlcmd.Parameters.AddWithValue("@cnScreenId", "11");
                sqlcmd.Parameters.AddWithValue("@cnStatus", obj.cnStatus);
                sqlcmd.Parameters.AddWithValue("@flagSendConsigneeMsg", obj.flagSendConsigneeMsg);
                sqlcmd.Parameters.AddWithValue("@isExpUser", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@isInt", obj.isExpUser);
                //sqlcmd.Parameters.AddWithValue("@ispriceComputed", "0");

                //   SqlParameter P_XCode = new SqlParameter("@XCode", SqlDbType.VarChar, 256);
                //    P_XCode.Direction = ParameterDirection.Output;

                //  sqlcmd.Parameters.Add(P_XCode);
                // sqlcmd.Parameters.Add(SParam);
                sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
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

        public string Update_ExpressionConsignmentApproval2(Cl_Variables obj)
        {
            clvar.Error = "";
            try
            {
                string sql = "update Consignment set \n"
                               + "		    [isApproved] = '1' , isPriceComputed = '0', \n"
                               + "		    [accountReceivingDate] = '" + DateTime.Parse(txt_reportingDate.Text).ToString("yyyy-MM-dd") + "' \n"
                               + "          Where CONSIGNMENTNUMBER = '" + obj.consignmentNo + "' ";


                SqlConnection sqlcon = new SqlConnection(clvar.Strcon2());
                SqlTransaction trans;
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                try
                {
                    sqlcmd.CommandText = sql;
                    sqlcmd.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    sqlcon.Close();
                }
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

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'A'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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



        public DataSet GetConsignmentForApproval(Cl_Variables clvar)
        {

            #region MyRegion
            //string sqlString = "select c.bookingDate,\n" +
            //"       c.consignmentNumber,\n" +
            //"       c.customerType,\n" +
            //"       c.creditClientId,\n" +
            //"       c.orgin,\n" +
            //"       c.serviceTypeName,\n" +
            //"       c.consigner,\n" +
            //"       c.consignee,\n" +
            //"       c.destination,\n" +
            //"       c.weight,\n" +
            //"       c.riderCode,\n" +
            //"       c.originExpressCenter,\n" +
            //"       c.consignmentTypeId,\n" +
            //"       c.chargedAmount,\n" +
            //"       c.isApproved,\n" +
            //"       ic.invoiceNumber,\n" +
            //"       i.startDate ReportingDate,\n" +
            //"       i.deliveryStatus,\n" +
            //"       cm.priceModifierId,\n" +
            //"       p.name,\n" +
            //"       cm.calculatedValue,\n" +
            //"       cm.calculationBase,\n" +
            //"       cm.isTaxable,\n" +
            //"       cm.SortOrder,\n" +
            //"       p.description, c.destinationExpressCenterCode, c.couponNumber\n" +
            //"\n" +
            //"  from consignment c\n" +
            //" inner join InvoiceConsignment ic\n" +
            //"    on c.consignmentNumber = ic.consignmentNumber\n" +
            //" inner join Invoice i\n" +
            //"    on i.invoiceNumber = ic.invoiceNumber\n" +
            //"  left outer join ConsignmentModifier cm\n" +
            //"    on c.consignmentNumber = cm.consignmentNumber\n" +
            //"  left outer join PriceModifiers p\n" +
            //"    on cm.priceModifierId = p.id\n" +
            //" where c.orgin = '4'\n" +
            //"   and c.consignmentTypeId <> '10'\n" +
            //"   and cm.priceModifierId is not null\n" +
            //"   and IsNull( i.IsInvoiceCanceled , 0 ) ='0'\n" +
            //"   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            //" order by consignmentNumber, SortOrder";

            //// sqlString = "select c.bookingDate,\n" +
            ////"       c.consignmentNumber,\n" +
            ////"       c.customerType,\n" +
            ////"       c.creditClientId,\n" +
            ////"       c.orgin,\n" +
            ////"       c.serviceTypeName,\n" +
            ////"       c.consigner,\n" +
            ////"       c.consignee,\n" +
            ////"       c.destination,\n" +
            ////"       c.weight,\n" +
            ////"       c.riderCode,\n" +
            ////"       c.originExpressCenter,\n" +
            ////"       c.consignmentTypeId,\n" +
            ////"       c.chargedAmount,\n" +
            ////"       CAST(c.isApproved as varchar) isApproved,\n" +
            ////"       c.consignerAccountNo accountNo,\n" +
            ////"       ic.invoiceNumber,\n" +
            ////"       i.startDate ReportingDate,\n" +
            ////"       i.deliveryStatus,\n" +
            ////"       cm.priceModifierId,\n" +
            ////"       p.name priceModifierName,\n" +
            ////"       cm.calculatedValue,\n" +
            ////"       cm.calculationBase,\n" +
            ////"       cm.isTaxable,\n" +
            ////"       cm.SortOrder,\n" +
            ////"       p.description, c.destinationExpressCenterCode, c.accountReceivingDate, c.bookingDate, c.COD\n" +
            ////"\n" +
            ////"  from consignment c\n" +
            ////" left outer join InvoiceConsignment ic\n" +
            ////"    on c.consignmentNumber = ic.consignmentNumber\n" +
            ////" left outer join Invoice i\n" +
            ////"    on i.invoiceNumber = ic.invoiceNumber\n" +
            ////"  left outer join ConsignmentModifier cm\n" +
            ////"    on c.consignmentNumber = cm.consignmentNumber\n" +
            ////"  left outer join PriceModifiers p\n" +
            ////"    on cm.priceModifierId = p.id\n" +
            ////" inner join CreditClients cc\n" +
            ////"    on c.creditClientId = cc.id\n" +
            ////" where /*c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            ////"   and c.consignmentTypeId <> '10'\n" +
            ////"   and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' and ISNULL(i.IsInvoiceCanceled,0) = '0' \n" +
            ////" order by consignmentNumber, SortOrder"; 
            #endregion

            string sql = "SELECT A.*, i.deliveryStatus, CASE  \n"
               + "WHEN i.IsInvoiceCanceled = '1' THEN ''\n"
               + " ELSE i.invoiceNumber      \n"
               + "END invoiceNumber_ FROM ( \n"
               + "SELECT --c.bookingDate, \n"
               + "       c.consignmentNumber, \n"
               + "       c.customerType, \n"
               + "       c.creditClientId, \n"
               + "       c.orgin, \n"
               + "       c.serviceTypeName, \n"
               + "       c.consigner, \n"
               + "       c.consignee, \n"
               + "       c.destination, \n"
               + "       c.weight, \n"
               + "       c.riderCode, \n"
               + "       c.originExpressCenter, \n"
               + "       c.consignmentTypeId, \n"
               + "       c.chargedAmount, \n"
               + "       CAST(c.isApproved AS VARCHAR)     isApproved, \n"
               + "       c.consignerAccountNo              , \n"
               + "       --ic.invoiceNumber, \n"
               + "   --    i.startDate                       ReportingDate, \n"
               + "   --    i.deliveryStatus, \n"
               + "       cm.priceModifierId, \n"
               + "       p.name                            priceModifierName, \n"
               + "       cm.calculatedValue, \n"
               + "       cm.modifiedCalculationValue modifiedCalculatedValue,\n"
               + "       cm.calculationBase, \n"
               + "       cm.isTaxable, \n"
               + "       cm.SortOrder, \n"
               + "       p.description, \n"
               + "       c.destinationExpressCenterCode, \n"
               + "       c.accountReceivingDate, \n"
               + "       c.bookingDate, \n"
               + "       c.COD, c.gst, c.totalAmount, \n"
               + "       ic.invoiceNumber, c.consignerCellNo, c.ConsigneePhoneNo, c.address, c.pieces, c.couponNumber, c.ExpressionDeliveryDateTime, c.otherCharges\n"
               + "    --   i.IsInvoiceCanceled \n"
               + "FROM   consignment c \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       --LEFT OUTER JOIN Invoice i \n"
               + "       --     ON  i.invoiceNumber = ic.invoiceNumber \n"
               + "       LEFT OUTER JOIN ConsignmentModifier cm \n"
               + "            ON  c.consignmentNumber = cm.consignmentNumber \n"
               + "       LEFT OUTER JOIN PriceModifiers p \n"
               + "            ON  cm.priceModifierId = p.id \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  c.creditClientId = cc.id \n"
               + "WHERE  /*c.orgin = '4' \n"
               + "       and c.consignmentTypeId <> '10' \n"
               + "       and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' \n"
               + "  --     AND ISNULL(i.IsInvoiceCanceled, 0) = '0' \n"
               + ") A \n"
               + "LEFT OUTER JOIN  \n"
               + "Invoice i  \n"
               + "ON A.invoiceNumber = i.invoiceNumber \n"
               + "  ORDER BY i.createdOn desc";




            sql = "SELECT A.*,\n" +
           "       i.deliveryStatus,\n" +
           "       CASE\n" +
           "         WHEN i.IsInvoiceCanceled = '1' THEN\n" +
           "          ''\n" +
           "         ELSE\n" +
           "          i.invoiceNumber\n" +
           "       END invoiceNumber_\n" +
           "  FROM (SELECT c.*, ic.invoiceNumber\n" +
           "          FROM consignment c\n" +
           "          LEFT OUTER JOIN InvoiceConsignment ic\n" +
           "            ON c.consignmentNumber = ic.consignmentNumber\n" +
           "         WHERE c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "'\n" +
           "        ) A\n" +
           "  LEFT OUTER JOIN Invoice i\n" +
           "    ON A.invoiceNumber = i.invoiceNumber\n" +
           " ORDER BY i.createdOn desc";




            DataSet dt = new DataSet();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataSet Consignment(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM Consignment c WHERE c.consignmentNumber = '" + clvar.consignmentNo + "'";

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
        public DataTable ConsignmentExpressionDetail(Cl_Variables clvar)
        {
            string query = "select \n" +
                                        "ced.id, ced.consignementNo, ced.itemId, ced.itemQty, ced.amount, ced.gst, ced.serviceCharges, \n" +
                                        "ep.code + ' -- ' +ep.name itemcode, ced.message, CASE WHEN (ced.status is null or ced.status = '') then '1' else ced.status end status, ced.createdon, ced.createdby, ced.modifiedon, ced.modifiedby \n" +
                                        "from ConsignmentExpressionDetail ced , ExpressionProduct ep  \n" +
                                        "where  \n" +
                                        "ced.itemId = ep.id \n" +
                                        "and ced.consignementNo = '" + clvar.consignmentNo + "'";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;

        }

        protected void btn_unapprove_Click(object sender, EventArgs e)
        {
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dates = MinimumDate(clvar);
            DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            txt_ConNo_TextChanged(this, e);
            if (DateTime.Parse(booking_date.Text) < minAllowedDate || DateTime.Parse(booking_date.Text) > maxAllowedDate || DateTime.Parse(txt_reportingDate.Text) < minAllowedDate || DateTime.Parse(txt_reportingDate.Text) > maxAllowedDate)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot Approve CN')", true);
                return;
            }

            if (txt_invoiceNumber.Text.Trim() != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Consignment Cannot Be Unapproved')", true);
                return;
            }
            clvar.consignmentNo = txt_ConNo.Text.Trim();
            string error = UnapproveConsignment(clvar);
            if (error != "OK")
            {
                Errorid.Text = "Consignment Could not be unapproved";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Could not be unapproved')", true);
                return;
            }
            else
            {
                Errorid.Text = "Consignment unapproved";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment unapproved')", true);
                txt_ConNo_TextChanged(this, e);
                //txt_consignmentNo_TextChanged(this, e);
                return;
            }
        }


        public string UnapproveConsignment(Cl_Variables clvar)
        {
            string query = "update consignment Set isApproved = '0' where consignmentnumber = '" + clvar.consignmentNo + "'";
            string query2 = "insert into MNP_ConsignmentUnapproval (ConsignmentNumber, USERID, TransactionTime, STATUS) VALUES (\n" +
                "'" + clvar.consignmentNo + "', '" + HttpContext.Current.Session["U_ID"].ToString() + "',GETDATE(), '0')";
            string query1 = "insert into Consignment_Archive select * from consignment c where c.consignmentNumber = '" + clvar.consignmentNo + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(query1, con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(query2, con);
                cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                //Consignemnts con = new Consignemnts();
                con.Close();
                //InsertErrorLog(clvar.consignmentNo, "", "", "", "", "", "UNAPPROVE CONSIGNMENT", ex.Message);
                return ex.Message;
            }
            finally { con.Close(); }

            return "OK";
        }


        public bool IsNumeric(string text)
        {
            char[] arr = text.ToCharArray();
            foreach (char ch in arr)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }

            return true;
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
    }
}