using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MRaabta.App_Code;
using System.Data.SqlClient;


namespace MRaabta.Files
{
    public partial class ReceiptVoucherCODBulk_Rider_New : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        Cl_Receipts rec = new Cl_Receipts();
        LoadingPrintReport enc_ = new LoadingPrintReport();
        DataTable dt_paymentSource;

        decimal totalAmount = 0M;
        double Total = 0;

        string RSNo = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                SetInitialRow();
            }

            string UserId = Session["U_ID"].ToString();
            DataTable dtUserId = GetReportAccess(UserId);

            if (dtUserId.Rows.Count == 0)
            {
                Response.Redirect("~/login");
            }
            Errorid.Text = "";
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            gv_runsheet.DataSource = null;
            gv_runsheet.DataBind();
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
            txt_riderCode.Enabled = true;
            txt_riderCode.Text = "";
            gv_summary.DataSource = null;
            gv_summary.DataBind();
            ECAmount.Text = "";
            CashAmount.Text = "";
            if (!txt_riderCode.Enabled)
            {

                AlertMessage("Enter New Rider Code", "Red");
            }
            else
            {
                AlertMessage("Enter Rider Code", "Red");
            }

            return;

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            decimal totalhblamount = 0;
            decimal hbloutstanding = 0;
            string UserId = Session["U_ID"].ToString();
            DataTable dtUserId = GetReportAccess(UserId);

            if (dtUserId.Rows.Count > 0)
            {

            }
            else
            {
                Response.Redirect("~/login");
            }

            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("RunsheetNumber", typeof(string)),
        new DataColumn("RiderCode", typeof(string)),
        new DataColumn("ExpressCenterCode", typeof(string)),
        new DataColumn("ConsignmentNumber", typeof(string)),
        new DataColumn("CODAmount", typeof(float)),
        new DataColumn("CreditClientID", typeof(Int64)),
        new DataColumn("RefNumber", typeof(string)),
        new DataColumn("PaymentSource", typeof(string)),
           new DataColumn("Remarks", typeof(string))
        });
            bool invalidAmounts = false;
            bool voucherAmountGreaterThanCODAmount = false;
            foreach (GridViewRow row in gv_consignments.Rows)
            {
                CheckBox chk = row.FindControl("chk_check") as CheckBox;
                if (chk.Checked)
                {
                    string runsheetNumber = (row.FindControl("hd_runsheetNumber") as HiddenField).Value;
                    string riderCode = (row.FindControl("hd_riderCode") as HiddenField).Value;
                    string expressCenterCode = (row.FindControl("hd_expressCenterCode") as HiddenField).Value;
                    string consignmentNumber = row.Cells[1].Text;
                    string codAmount = row.Cells[9].Text;
                    string voucherAmount = (row.FindControl("txt_amount") as TextBox).Text;
                    string creditClientID = (row.FindControl("hd_creditClientID") as HiddenField).Value;
                    string refNumber = (row.FindControl("txt_refNumber") as TextBox).Text;
                    string codType = (row.FindControl("hd_CODType") as HiddenField).Value;

                    float tempCodAmount = 0f;
                    float tempVoucherAmount = 0f;

                    float.TryParse(codAmount, out tempCodAmount);
                    float.TryParse(voucherAmount, out tempVoucherAmount);


                    if (tempVoucherAmount <= 0)
                    {
                        row.BackColor = System.Drawing.Color.LightPink;
                        invalidAmounts = true;
                    }
                    if (Math.Floor(tempCodAmount) != Math.Floor(tempVoucherAmount) && codType != "2")
                    {
                        row.BackColor = System.Drawing.Color.FromName("#fc4444");
                        voucherAmountGreaterThanCODAmount = true;
                    }
                    else
                    {
                        row.BackColor = System.Drawing.Color.White;
                    }

                    DataRow dr = dt.NewRow();

                    dr["RunsheetNumber"] = runsheetNumber;
                    dr["RiderCode"] = riderCode;
                    dr["ExpressCenterCode"] = expressCenterCode;
                    dr["ConsignmentNumber"] = consignmentNumber;
                    dr["CODAmount"] = tempVoucherAmount;
                    dr["CreditClientID"] = Int64.Parse(creditClientID);
                    clvar.RunsheetNumber = runsheetNumber;
                    clvar.RunSheetNumber = runsheetNumber;

                    if (Session["U_NAME"].ToString().ToUpper() == "affaq.qamar@mulphilog.com".ToUpper())
                    {
                        DropDownList ddl_adjustment = (row.FindControl("ddl_adjustment") as DropDownList);
                        DropDownList ddl_PaymentSource = (row.FindControl("ddl_PaymentSource") as DropDownList);
                        string txt_Remarks = (row.FindControl("txt_Remarks") as TextBox).Text;
                        if (ddl_PaymentSource.SelectedValue == "1" || ddl_PaymentSource.SelectedValue == "6")
                        {
                            dr["RefNumber"] = refNumber;
                        }
                        else
                        {
                            dr["RefNumber"] = ddl_adjustment.SelectedItem.ToString();
                        }
                        dr["PaymentSource"] = ddl_PaymentSource.SelectedValue.ToString();
                        dr["Remarks"] = txt_Remarks;
                    }
                    else
                    {
                        dr["RefNumber"] = refNumber;
                        dr["PaymentSource"] = '1';
                        dr["Remarks"] = "";
                    }
                    dt.Rows.Add(dr);

                    dt.AcceptChanges();
                }
            }
            if (invalidAmounts)
            {
                AlertMessage("Voucher Amount Cannot be less than or equal to 0", "Red");
                return;
            }
            if (voucherAmountGreaterThanCODAmount)
            {
                AlertMessage("Voucher Amount must equal COD Amount", "Red");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                AlertMessage("Select Consignments to Create Vouchers for.", "Red");
                return;
            }

            //   clvar.RunsheetNumber = txt_runsheetNumber.Text;
            clvar.RiderCode = txt_riderCode.Text;
            //  clvar.SealNumber = txt_runsheetNumber.Text + "-1";
            clvar.PaymentType = "5";
            clvar.VoucherDate = DateTime.Now.ToString("yyyy-MM-dd");
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            clvar.createdBy = HttpContext.Current.Session["U_ID"].ToString();

            string list = "";
            foreach (ListItem item in ddl_PaymentSource.Items)
            {
                if (item.Selected)
                    list += item.Value + ",";
            }

            long countamount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                countamount += Convert.ToInt64(dt.Rows[i]["CODAmount"].ToString());
            }
            if (ECAmount.Text != "0" && ECAmount.Text.Length > 0)
            {
                long difference = Convert.ToInt64(gv_summary.Rows[0].Cells[5].Text.Replace(",", "").ToString());
                if (difference > 1 && ECAmount.Text == "")
                {
                    CashAmount.Text = Total.ToString();
                    ECAmount.Text = "0";
                    AlertMessage("Please Enter EC Amount", "Red");
                    gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", countamount);
                    return;
                }
                else
                    if (Convert.ToInt64(ECAmount.Text) > countamount || Convert.ToInt64(ECAmount.Text) > difference)
                {
                    CashAmount.Text = "0";
                    AlertMessage("Amount is greater than the total amount against runsheet", "Red");
                    gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", countamount);
                    return;
                }
                else
                {
                    if (Convert.ToInt64((ECAmount.Text).ToString()) > countamount)
                    {
                        CashAmount.Text = "0";
                    }
                    else if (Convert.ToInt64((ECAmount.Text).ToString()) <= countamount)
                    {
                        if (!(list.Contains("11")))
                        {
                            CashAmount.Text = (countamount - Convert.ToInt64(ECAmount.Text)).ToString();
                        }
                        else
                        {
                            if (gridview_tab4.Rows.Count > 0)
                            {
                                if (((TextBox)gridview_tab4.Rows[0].Cells[2].FindControl("HBLKonnect")).Text.Replace(" ", "").Replace(",", "") == "")
                                {
                                    AlertMessage("Enter Amount for HBL-Konnect Cash", "Red");
                                }
                                else if (((TextBox)gridview_tab4.Rows[0].Cells[1].FindControl("HBLKonnect")).Text.Replace(" ", "").Replace(",", "") == "")
                                {
                                    AlertMessage("Enter Transaction ID for HBL-Konnect Cash", "Red");
                                }
                                else
                                {
                                    double hblamount = 0;
                                    hblamount = Convert.ToInt64(ECAmount.Text);
                                    for (int i = 0; i < gridview_tab4.Rows.Count; i++)
                                    {
                                        if (((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text.Replace(" ", "").Replace(",", "") != ""
                                            && ((TextBox)gridview_tab4.Rows[i].Cells[1].FindControl("transHBL")).Text.Replace(" ", "").Replace(",", "") != "")
                                        {
                                            hblamount += Convert.ToDouble(((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text);
                                        }
                                    }
                                    double amountval = countamount - hblamount;
                                    if (amountval >= 0)
                                    {
                                        CashAmount.Text = amountval.ToString();
                                    }
                                    else
                                    {
                                        CashAmount.Text = "0";
                                    }

                                }
                            }
                        }
                    }
                    gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", countamount);

                    CreateCODCashSummary();
                }
            }
            string error = "";
            if (list.Contains("11"))
            {
                (error, totalhblamount, hbloutstanding) = InsertHBLKonnectRecord(countamount);
            }
            if (!(error == "") && !(error == "Null"))
            {
                AlertMessage(error, "Red");
                return;
            }
            clvar.TotalAmount = Convert.ToDouble(totalhblamount);
            clvar.amount = Convert.ToDouble(hbloutstanding);

            float RemCashAmt = (Convert.ToInt32(CashAmount.Text) + Convert.ToInt32(ECAmount.Text));
            float RemHBLAmt = Convert.ToInt32(totalhblamount - hbloutstanding);

            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("RunsheetNumber", typeof(string)),
        new DataColumn("RiderCode", typeof(string)),
        new DataColumn("ExpressCenterCode", typeof(string)),
        new DataColumn("ConsignmentNumber", typeof(string)),
        new DataColumn("CODAmount", typeof(float)),
        new DataColumn("CreditClientID", typeof(Int64)),
        new DataColumn("RefNumber", typeof(string)),
        new DataColumn("PaymentSource", typeof(string)),
           new DataColumn("Remarks", typeof(string))
        });

            foreach (GridViewRow row in gv_consignments.Rows)
            {
                CheckBox chk = row.FindControl("chk_check") as CheckBox;
                if (chk.Checked)
                {
                    float RemVoucherAmt = 0f;

                    string runsheetNumber = (row.FindControl("hd_runsheetNumber") as HiddenField).Value;
                    string riderCode = (row.FindControl("hd_riderCode") as HiddenField).Value;
                    string expressCenterCode = (row.FindControl("hd_expressCenterCode") as HiddenField).Value;
                    string consignmentNumber = row.Cells[1].Text;
                    string codAmount = row.Cells[9].Text;
                    string voucherAmount = (row.FindControl("txt_amount") as TextBox).Text;
                    string creditClientID = (row.FindControl("hd_creditClientID") as HiddenField).Value;
                    string refNumber = (row.FindControl("txt_refNumber") as TextBox).Text;
                    string codType = (row.FindControl("hd_CODType") as HiddenField).Value;

                    float tempCodAmount = 0f;
                    float tempVoucherAmount = 0f;

                    float.TryParse(codAmount, out tempCodAmount);
                    float.TryParse(voucherAmount, out tempVoucherAmount);

                    RemVoucherAmt = tempVoucherAmount;

                    if (tempVoucherAmount <= 0)
                    {
                        row.BackColor = System.Drawing.Color.LightPink;
                        invalidAmounts = true;
                    }
                    if (Math.Floor(tempCodAmount) != Math.Floor(tempVoucherAmount) && codType != "2")
                    {
                        row.BackColor = System.Drawing.Color.FromName("#fc4444");
                        voucherAmountGreaterThanCODAmount = true;
                    }
                    else
                    {
                        row.BackColor = System.Drawing.Color.White;
                    }

                    clvar.RunsheetNumber = runsheetNumber;
                    clvar.RunSheetNumber = runsheetNumber;

                    if (gridview_tab4.Rows.Count > 0 && (totalhblamount - hbloutstanding) > 0)
                    {
                        if (RemCashAmt >= RemVoucherAmt && RemVoucherAmt > 0)
                        {
                            DataRow dr = dt.NewRow();

                            dr["RunsheetNumber"] = runsheetNumber;
                            dr["RiderCode"] = riderCode;
                            dr["ExpressCenterCode"] = expressCenterCode;
                            dr["ConsignmentNumber"] = consignmentNumber;
                            dr["CreditClientID"] = Int64.Parse(creditClientID);
                            dr["CODAmount"] = tempVoucherAmount;
                            dr["PaymentSource"] = '1';

                            RemVoucherAmt = 0;
                            RemCashAmt = RemCashAmt - tempVoucherAmount;

                            if (Session["U_NAME"].ToString().ToUpper() == "affaq.qamar@mulphilog.com".ToUpper())
                            {
                                DropDownList ddl_adjustment = (row.FindControl("ddl_adjustment") as DropDownList);
                                DropDownList ddl_PaymentSource = (row.FindControl("ddl_PaymentSource") as DropDownList);
                                string txt_Remarks = (row.FindControl("txt_Remarks") as TextBox).Text;
                                if (ddl_PaymentSource.SelectedValue == "1" || ddl_PaymentSource.SelectedValue == "6")
                                {
                                    dr["RefNumber"] = refNumber;
                                }
                                else
                                {
                                    dr["RefNumber"] = ddl_adjustment.SelectedItem.ToString();
                                }
                                dr["PaymentSource"] = ddl_PaymentSource.SelectedValue.ToString();
                                dr["Remarks"] = txt_Remarks;
                            }
                            else
                            {
                                dr["RefNumber"] = refNumber;
                                dr["Remarks"] = "";
                            }

                            dt.Rows.Add(dr);
                            dt.AcceptChanges();

                        }
                        else
                        {
                            if (RemHBLAmt > 0 && RemVoucherAmt > 0)
                            {
                                if (RemHBLAmt >= RemVoucherAmt)
                                {
                                    DataRow dr = dt.NewRow();

                                    dr["RunsheetNumber"] = runsheetNumber;
                                    dr["RiderCode"] = riderCode;
                                    dr["ExpressCenterCode"] = expressCenterCode;
                                    dr["ConsignmentNumber"] = consignmentNumber;
                                    dr["CreditClientID"] = Int64.Parse(creditClientID);
                                    dr["CODAmount"] = tempVoucherAmount;
                                    dr["PaymentSource"] = "11";

                                    RemVoucherAmt = 0;
                                    RemHBLAmt = RemHBLAmt - tempVoucherAmount;


                                    if (Session["U_NAME"].ToString().ToUpper() == "affaq.qamar@mulphilog.com".ToUpper())
                                    {
                                        DropDownList ddl_adjustment = (row.FindControl("ddl_adjustment") as DropDownList);
                                        DropDownList ddl_PaymentSource = (row.FindControl("ddl_PaymentSource") as DropDownList);
                                        string txt_Remarks = (row.FindControl("txt_Remarks") as TextBox).Text;
                                        if (ddl_PaymentSource.SelectedValue == "1" || ddl_PaymentSource.SelectedValue == "6")
                                        {
                                            dr["RefNumber"] = refNumber;
                                        }
                                        else
                                        {
                                            dr["RefNumber"] = ddl_adjustment.SelectedItem.ToString();
                                        }
                                        dr["PaymentSource"] = ddl_PaymentSource.SelectedValue.ToString();
                                        dr["Remarks"] = txt_Remarks;
                                    }
                                    else
                                    {
                                        dr["RefNumber"] = refNumber;
                                        dr["Remarks"] = "";
                                    }

                                    dt.Rows.Add(dr);
                                    dt.AcceptChanges();
                                }
                                else
                                {
                                    DataRow dr = dt.NewRow();

                                    dr["RunsheetNumber"] = runsheetNumber;
                                    dr["RiderCode"] = riderCode;
                                    dr["ExpressCenterCode"] = expressCenterCode;
                                    dr["ConsignmentNumber"] = consignmentNumber;
                                    dr["CreditClientID"] = Int64.Parse(creditClientID);
                                    dr["PaymentSource"] = "11";
                                    dr["CODAmount"] = RemHBLAmt;
                                    RemVoucherAmt = RemVoucherAmt - RemHBLAmt;
                                    RemHBLAmt = 0;

                                    if (Session["U_NAME"].ToString().ToUpper() == "affaq.qamar@mulphilog.com".ToUpper())
                                    {
                                        DropDownList ddl_adjustment = (row.FindControl("ddl_adjustment") as DropDownList);
                                        DropDownList ddl_PaymentSource = (row.FindControl("ddl_PaymentSource") as DropDownList);
                                        string txt_Remarks = (row.FindControl("txt_Remarks") as TextBox).Text;
                                        if (ddl_PaymentSource.SelectedValue == "1" || ddl_PaymentSource.SelectedValue == "6")
                                        {
                                            dr["RefNumber"] = refNumber;
                                        }
                                        else
                                        {
                                            dr["RefNumber"] = ddl_adjustment.SelectedItem.ToString();
                                        }
                                        dr["PaymentSource"] = ddl_PaymentSource.SelectedValue.ToString();
                                        dr["Remarks"] = txt_Remarks;
                                    }
                                    else
                                    {
                                        dr["RefNumber"] = refNumber;
                                        dr["Remarks"] = "";
                                    }

                                    dt.Rows.Add(dr);
                                    dt.AcceptChanges();
                                }
                            }
                            if (RemCashAmt > 0 && RemVoucherAmt > 0)
                            {
                                DataRow dr = dt.NewRow();

                                dr["RunsheetNumber"] = runsheetNumber;
                                dr["RiderCode"] = riderCode;
                                dr["ExpressCenterCode"] = expressCenterCode;
                                dr["ConsignmentNumber"] = consignmentNumber;
                                dr["CreditClientID"] = Int64.Parse(creditClientID);
                                dr["PaymentSource"] = '1';
                                dr["CODAmount"] = RemCashAmt;
                                RemVoucherAmt = RemVoucherAmt - RemCashAmt;
                                RemCashAmt = 0;

                                if (Session["U_NAME"].ToString().ToUpper() == "affaq.qamar@mulphilog.com".ToUpper())
                                {
                                    DropDownList ddl_adjustment = (row.FindControl("ddl_adjustment") as DropDownList);
                                    DropDownList ddl_PaymentSource = (row.FindControl("ddl_PaymentSource") as DropDownList);
                                    string txt_Remarks = (row.FindControl("txt_Remarks") as TextBox).Text;
                                    if (ddl_PaymentSource.SelectedValue == "1" || ddl_PaymentSource.SelectedValue == "6")
                                    {
                                        dr["RefNumber"] = refNumber;
                                    }
                                    else
                                    {
                                        dr["RefNumber"] = ddl_adjustment.SelectedItem.ToString();
                                    }
                                    dr["PaymentSource"] = ddl_PaymentSource.SelectedValue.ToString();
                                    dr["Remarks"] = txt_Remarks;
                                }
                                else
                                {
                                    dr["RefNumber"] = refNumber;
                                    dr["Remarks"] = "";
                                }

                                dt.Rows.Add(dr);
                                dt.AcceptChanges();
                            }

                        }
                    }
                    else if (RemVoucherAmt > 0)
                    {
                        DataRow dr = dt.NewRow();

                        dr["RunsheetNumber"] = runsheetNumber;
                        dr["RiderCode"] = riderCode;
                        dr["ExpressCenterCode"] = expressCenterCode;
                        dr["ConsignmentNumber"] = consignmentNumber;
                        dr["CreditClientID"] = Int64.Parse(creditClientID);
                        dr["CODAmount"] = tempVoucherAmount;
                        dr["PaymentSource"] = '1';

                        RemVoucherAmt = 0;

                        if (Session["U_NAME"].ToString().ToUpper() == "affaq.qamar@mulphilog.com".ToUpper())
                        {
                            DropDownList ddl_adjustment = (row.FindControl("ddl_adjustment") as DropDownList);
                            DropDownList ddl_PaymentSource = (row.FindControl("ddl_PaymentSource") as DropDownList);
                            string txt_Remarks = (row.FindControl("txt_Remarks") as TextBox).Text;
                            if (ddl_PaymentSource.SelectedValue == "1" || ddl_PaymentSource.SelectedValue == "6")
                            {
                                dr["RefNumber"] = refNumber;
                            }
                            else
                            {
                                dr["RefNumber"] = ddl_adjustment.SelectedItem.ToString();
                            }
                            dr["PaymentSource"] = ddl_PaymentSource.SelectedValue.ToString();
                            dr["Remarks"] = txt_Remarks;
                        }
                        else
                        {
                            dr["RefNumber"] = refNumber;
                            dr["Remarks"] = "";
                        }

                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                }
            }


            error = CreateCODBulkVouchers(clvar, dt);

            if (error == "OK")
            {
                DataTable ds = GetRiderCashSummary(clvar);
                if (ds.Rows.Count > 0)
                {
                    gv_summary.DataSource = ds;
                    gv_summary.DataBind();
                    gv_summary.Visible = true;
                    gv_summary.Enabled = true;
                }
                AlertMessage("Vouchers generated for Selected Consignments", "Green");
                // clvar.RunsheetNumber = txt_runsheetNumber.Text;
                Session["VariableObj"] = clvar;
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                string script = String.Format(script_, "ReceiptPrint_Bulk_New.aspx", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                ResetAll();
                Page_Load(sender, e);
                //  return;
            }
            else
            {
                AlertMessage(error, "Red");
                return;
            }
        }
        protected void CheckAll(object sender, EventArgs e)
        {
            double Total = 0;
            CheckBox chkAll = gv_consignments.HeaderRow.Cells[0].FindControl("chk_checkAll") as CheckBox;

            for (int i = 0; i < gv_consignments.Rows.Count; i++)
            {
                TextBox txt_amount = (TextBox)gv_consignments.Rows[i].FindControl("txt_amount");
                txt_amount.Text = gv_consignments.Rows[i].Cells[9].Text;
                gv_consignments.Rows[i].Cells[10].Enabled = false;

                CheckBox chk = gv_consignments.Rows[i].FindControl("chk_check") as CheckBox;
                if (chkAll.Checked)
                {
                    if (double.Parse(txt_amount.Text) != 0)
                    {
                        chk.Checked = true;
                        chk.Enabled = false;
                        Total += double.Parse(txt_amount.Text);
                    }
                    else
                    {
                        chk.Checked = false;
                        chk.Enabled = true;
                    }
                }
                if (gv_consignments.Rows[i].Cells[4].Text == "D-DELIVERED" || gv_consignments.Rows[i].Cells[5].Text != "&nbsp;" || txt_amount.Text == "0")
                {
                    chk.Checked = false;
                    Total -= double.Parse(txt_amount.Text);
                }
                if (gv_consignments.Rows[i].Cells[4].Text == "D-DELIVERED" && gv_consignments.Rows[i].Cells[5].Text == "&nbsp;" && txt_amount.Text != "0")
                {
                    chk.Checked = true;
                    Total += double.Parse(txt_amount.Text);
                }
                if (!chkAll.Checked)
                {
                    Total += double.Parse(txt_amount.Text);

                }
                if (!chkAll.Checked && gv_consignments.Rows[i].Cells[4].Text != "D-DELIVERED")
                {
                    chk.Checked = false;
                    Total -= double.Parse(txt_amount.Text);
                }
            }
            long difference = Convert.ToInt64(gv_summary.Rows[0].Cells[5].Text.Replace(",", "").ToString());
            if (difference > 1 && ECAmount.Text == "")
            {
                CashAmount.Text = Total.ToString();
                ECAmount.Text = "0";
                AlertMessage("Please Enter EC Amount", "Red");
            }
            else
               if (Convert.ToInt64(ECAmount.Text) > Total || Convert.ToInt64(ECAmount.Text) > difference)
            {
                CashAmount.Text = "0";
                AlertMessage("Amount is greater than the total amount against runsheet", "Red");
            }
            else
            {
                if (Convert.ToInt64((ECAmount.Text).ToString()) > Total)
                {
                    CashAmount.Text = "0";
                }
                else if (Convert.ToInt64((ECAmount.Text).ToString()) <= Total)
                {
                    CashAmount.Text = (Total - Convert.ToInt64(ECAmount.Text)).ToString();
                }
            }
            gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", Total);
            int cncount = 0; decimal codamount = 0;
            for (int i = 0; i < gv_runsheet.Rows.Count; i++)
            {

                cncount += Convert.ToInt32(((Label)gv_runsheet.Rows[i].Cells[2].FindControl("CNCount")).Text.Replace(",", "").ToString());
                codamount += Convert.ToDecimal(((Label)gv_runsheet.Rows[i].Cells[3].FindControl("CODAmount")).Text.Replace(",", "").ToString());

            }
            gv_runsheet.FooterRow.Cells[2].Text = string.Format("{0:N0}", cncount);
            gv_runsheet.FooterRow.Cells[3].Text = string.Format("{0:N0}", codamount);
        }
        protected void PaymentSource_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (ListItem item in ddl_PaymentSource.Items)
            {
                if (item.Selected && item.Value == "11")
                {
                    gridview_tab4.Enabled = true;
                    btn_addkonn.Enabled = true;
                }
                else
                {
                    gridview_tab4.Enabled = false;
                    btn_addkonn.Enabled = false;
                }
            }

            if (ddl_PaymentSource.SelectedValue == "")
            {
                AlertMessage("Atleast one payment source must be selected", "red");
                ddl_PaymentSource.SelectedValue = "1";
                gridview_tab4.Enabled = false;
                btn_addkonn.Enabled = false;
            }
            changeamount(sender, e);

        }
        protected void changeamount(object sender, EventArgs e)
        {
            string list = "";
            foreach (ListItem item in ddl_PaymentSource.Items)
            {
                if (item.Selected)
                    list += item.Value + ",";
            }
            if (list.Contains("11"))
            {
                gridview_tab4.Enabled = true;
                btn_addkonn.Enabled = true;
            }

            double Total = 0; long checkedcount = 0;
            for (int i = 0; i < gv_consignments.Rows.Count; i++)
            {
                gv_consignments.Rows[i].Cells[10].Enabled = false;

                CheckBox chk = gv_consignments.Rows[i].FindControl("chk_check") as CheckBox;

                if (chk.Checked)
                {
                    TextBox txt_amount = (TextBox)gv_consignments.Rows[i].FindControl("txt_amount");
                    txt_amount.Text = gv_consignments.Rows[i].Cells[9].Text;
                    checkedcount = checkedcount + 1;
                    Total += double.Parse(txt_amount.Text);
                }
            }
            CheckBox chkAll = gv_consignments.HeaderRow.Cells[0].FindControl("chk_checkAll") as CheckBox;
            if (gv_consignments.Rows.Count == checkedcount)
            {
                chkAll.Checked = true;
            }
            else
            {
                chkAll.Checked = false;
            }
            long difference = Convert.ToInt64(gv_summary.Rows[0].Cells[5].Text.Replace(",", "").ToString());
            if (ECAmount.Text == "")
            {
                ECAmount.Text = "0";
            }
            if (difference > 1 && ECAmount.Text == "")
            {
                CashAmount.Text = Total.ToString();
                ECAmount.Text = "0";
                AlertMessage("Please Enter EC Amount", "Red");
            }
            else if (Convert.ToInt64(ECAmount.Text) > Total || Convert.ToInt64(ECAmount.Text) > difference)
            {
                CashAmount.Text = "0";
                AlertMessage("Amount is greater than the total amount against runsheet", "Red");
            }
            else
            {
                if (Convert.ToInt64((ECAmount.Text).ToString()) > Total)
                {
                    CashAmount.Text = "0";
                }
                else if (Convert.ToInt64((ECAmount.Text).ToString()) <= Total)
                {
                    if (!(list.Contains("11")))
                    {
                        CashAmount.Text = (Total - Convert.ToInt64(ECAmount.Text)).ToString();
                    }
                    else
                    {
                        if (gridview_tab4 != null && gridview_tab4.Rows.Count > 0)
                        {
                            if (((TextBox)gridview_tab4.Rows[0].Cells[2].FindControl("HBLKonnect")).Text.Replace(" ", "").Replace(",", "") == "")
                            {
                                AlertMessage("Enter Amount for HBL-Konnect Cash", "Red");
                            }
                            else if (((TextBox)gridview_tab4.Rows[0].Cells[1].FindControl("HBLKonnect")).Text.Replace(" ", "").Replace(",", "") == "")
                            {
                                AlertMessage("Enter Transaction ID for HBL-Konnect Cash", "Red");
                            }
                            else
                            {
                                double hblamount = 0;
                                hblamount = Convert.ToInt64(ECAmount.Text);
                                for (int i = 0; i < gridview_tab4.Rows.Count; i++)
                                {
                                    if (((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text.Replace(" ", "").Replace(",", "") != "")
                                    {
                                        hblamount += Convert.ToDouble(((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text);
                                    }
                                }
                                double amountval = Total - hblamount;
                                if (amountval >= 0)
                                {
                                    CashAmount.Text = amountval.ToString();
                                }
                                else
                                {
                                    CashAmount.Text = "0";
                                }

                            }
                        }
                        else
                        {
                            if (Total - Convert.ToInt64(ECAmount.Text) >= 0)
                            {
                                CashAmount.Text = (Total - Convert.ToInt64(ECAmount.Text)).ToString();
                            }
                            else
                            {
                                CashAmount.Text = "0";
                            }
                        }
                    }
                }
            }


            //if (Convert.ToInt64(Total) > Convert.ToInt64(ECAmount.Text.ToString()))
            //{
            //    CashAmount.Text = (Convert.ToInt64(Total) - Convert.ToInt64(ECAmount.Text.ToString())).ToString();
            //}
            //else
            //{
            //    CashAmount.Text = "0";
            //}

            gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", Total);
            int cncount = 0; decimal codamount = 0;
            for (int i = 0; i < gv_runsheet.Rows.Count; i++)
            {

                cncount += Convert.ToInt32(((Label)gv_runsheet.Rows[i].Cells[2].FindControl("CNCount")).Text.Replace(",", "").ToString());
                codamount += Convert.ToDecimal(((Label)gv_runsheet.Rows[i].Cells[3].FindControl("CODAmount")).Text.Replace(",", "").ToString());

            }
            gv_runsheet.FooterRow.Cells[2].Text = string.Format("{0:N0}", cncount);
            gv_runsheet.FooterRow.Cells[3].Text = string.Format("{0:N0}", codamount);

        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            clvar.RiderCode = txt_riderCode.Text;
            dt_paymentSource = getPaymentSource();
            if (dt_paymentSource.Rows.Count > 0)
            {
                ddl_PaymentSource.DataSource = dt_paymentSource;
                ddl_PaymentSource.DataTextField = "Name";
                ddl_PaymentSource.DataValueField = "id";
                ddl_PaymentSource.DataBind();
                ddl_PaymentSource.SelectedValue = "1";
            }
            DataTable ds = GetRiderCashSummary(clvar);
            ECAmount.Text = "";
            if (ds.Rows.Count > 0)
            {
                gv_summary.DataSource = ds;
                gv_summary.DataBind();
                gv_summary.Visible = true;
                gv_summary.Enabled = true;
            }

            DataTable todayrunsheet = GetRunsheetFromRider(clvar);
            if (todayrunsheet.Rows.Count > 0)
            {
                gv_runsheet.DataSource = todayrunsheet;
                gv_runsheet.DataBind();
                gv_runsheet.FooterRow.Cells[2].Text = string.Format("{0:N0}", todayrunsheet.Compute("SUM(CNCount)", ""));
                gv_runsheet.FooterRow.Cells[3].Text = string.Format("{0:N0}", todayrunsheet.Compute("SUM(CODAmount)", ""));
                gv_runsheet.ShowFooter = true;
                txt_riderCode.Enabled = false;
                fld_amounts.Visible = true;
            }
            else
            {
                gv_summary.DataSource = null;
                gv_summary.DataBind();
                gv_summary.Visible = false;
                gv_summary.Enabled = true;
                fld_amounts.Visible = false;
                AlertMessage("No runsheets found", "Red");
            }
            return;
        }
        protected void btn_RunsheetSelect(object sender, EventArgs e)
        {
            //int countchk = 0;
            clvar.RunsheetNumber = "";
            CheckBox checkBox = (sender as CheckBox).FindControl("chk_Run") as CheckBox;

            CheckBox chkrun = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)chkrun.NamingContainer;
            int id = gvr.RowIndex;
            //string id = gv_runsheet.Rows[gvr.RowIndex].Value.ToString();
            //string cellvalue = GridView1.Rows[gvr.RowIndex].Cells[1].Text;
            //int id = int.Parse(checkBox.ID);

            //CheckBox chkrunall = (CheckBox)gv_runsheet.HeaderRow.Cells[0].FindControl("chk_AllRun");
            int cncount = 0;
            decimal codamount = 0;
            for (int i = 0; i < gv_runsheet.Rows.Count; i++)
            {
                if (id != i)
                {
                    CheckBox chk = (CheckBox)gv_runsheet.Rows[i].Cells[0].FindControl("chk_Run");
                    chk.Checked = false;
                }
                else
                {
                    clvar.RunsheetNumber += "'" + gv_runsheet.Rows[i].Cells[1].Text + "',";
                }
                cncount += Convert.ToInt32(((Label)gv_runsheet.Rows[i].Cells[2].FindControl("CNCount")).Text.Replace(",", "").ToString());
                codamount += Convert.ToDecimal(((Label)gv_runsheet.Rows[i].Cells[3].FindControl("CODAmount")).Text.Replace(",", "").ToString());

            }
            gv_runsheet.FooterRow.Cells[2].Text = string.Format("{0:N0}", cncount);
            gv_runsheet.FooterRow.Cells[3].Text = string.Format("{0:N0}", codamount);

            //for (int i = 0; i < gv_runsheet.Rows.Count; i++)
            //{
            //    CheckBox chk = (CheckBox)gv_runsheet.Rows[i].Cells[0].FindControl("chk_Run");
            //    if (chk.Checked)
            //    {
            //        countchk++;
            //        clvar.RunsheetNumber += "'" + gv_runsheet.Rows[i].Cells[1].Text + "',";
            //    }
            //}
            //if (gv_runsheet.Rows.Count == countchk)
            //{
            //    chkrunall.Checked = true;
            //}
            //else
            //{
            //    chkrunall.Checked = false;

            //}
            clvar.RunsheetNumber = clvar.RunsheetNumber.Trim(',');
            RSNo = clvar.RunsheetNumber;
            DataTable dt = GetConsignmentsFromRunsheet(clvar);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("RRNumber <> '' and DepositSlipNo <> RunsheetNumber");
                    foreach (DataRow dr_ in dr)
                    {
                        dt.Rows.Remove(dr_);
                    }
                    gv_consignments.DataSource = dt;
                    gv_consignments.DataBind();
                    if (Session["U_NAME"].ToString().ToUpper() != "affaq.qamar@mulphilog.com".ToUpper())
                    {
                        gv_consignments.Columns[11].Visible = false;
                        gv_consignments.Columns[13].Visible = false;
                    }

                    foreach (GridViewRow row in gv_consignments.Rows)
                    {
                        if (dt.Rows[row.RowIndex][12].ToString() == "D-DELIVERED")
                        {
                            gv_consignments.Rows[row.RowIndex].Cells[1].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[1].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[2].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[2].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[3].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[3].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[4].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[4].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[5].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[5].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[6].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[6].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[7].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[7].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[8].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[8].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[9].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[9].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[10].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[10].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[11].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[11].ForeColor = System.Drawing.Color.White;

                            gv_consignments.Rows[row.RowIndex].Cells[12].BackColor = System.Drawing.Color.Green;
                            gv_consignments.Rows[row.RowIndex].Cells[12].ForeColor = System.Drawing.Color.White;


                            if (dt.Rows[row.RowIndex][10].ToString() == "N")
                            {
                                TextBox txt_amount = (TextBox)gv_consignments.Rows[row.RowIndex].FindControl("txt_amount");
                                txt_amount.Text = gv_consignments.Rows[row.RowIndex].Cells[9].Text;
                                gv_consignments.Rows[row.RowIndex].Cells[10].Enabled = false;

                                CheckBox chk = row.FindControl("chk_check") as CheckBox;
                                if (double.Parse(txt_amount.Text) != 0)
                                {
                                    chk.Checked = true;
                                }

                                Total += double.Parse(txt_amount.Text);

                                gv_consignments.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                                gv_consignments.FooterRow.Cells[10].Font.Bold = true;
                                gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", Total);

                            }
                            ECAmount.Text = "0";
                            CashAmount.Text = Total.ToString();
                        }
                    }
                    dr = null;
                    dr = dt.Select("DepositSlipNo = RunsheetNumber");
                    if (dr.Count() > 0)
                    {
                    }
                    else
                    {
                        gv_consignments.Enabled = true;

                    }
                    txt_riderCode.Enabled = false;
                }
                else
                {
                    gv_consignments.DataSource = null;
                    gv_consignments.DataBind();
                    txt_riderCode.Enabled = false;
                    AlertMessage("No Records Found", "Red");
                    return;
                }
            }
            else
            {
                gv_consignments.DataSource = null;
                gv_consignments.DataBind();
                //    txt_runsheetNumber.Enabled = true;
                txt_riderCode.Enabled = false;
                AlertMessage("Some Error Occured. Please Contact IT Support.", "Red");
                return;
            }
        }
        //protected void btn_RunsheetSelectAll(object sender, EventArgs e)
        //{
        //    clvar.RunsheetNumber = "";
        //    CheckBox chkall = gv_runsheet.HeaderRow.Cells[0].FindControl("chk_AllRun") as CheckBox;
        //    for (int i = 0; i < gv_runsheet.Rows.Count; i++)
        //    {
        //        CheckBox chkrun = (CheckBox)gv_runsheet.Rows[i].Cells[0].FindControl("chk_Run");
        //        if (chkall.Checked)
        //        {
        //            clvar.RunsheetNumber += "'" + gv_runsheet.Rows[i].Cells[1].Text + "',";
        //            chkrun.Checked = true;
        //        }
        //        else
        //        {
        //            chkrun.Checked = false;
        //        }
        //    }



        //    clvar.RunsheetNumber = clvar.RunsheetNumber.Trim(',');

        //    DataTable dt = GetConsignmentsFromRunsheet(clvar);

        //    if (dt != null)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            DataRow[] dr = dt.Select("RRNumber <> '' and DepositSlipNo <> RunsheetNumber");
        //            foreach (DataRow dr_ in dr)
        //            {
        //                dt.Rows.Remove(dr_);
        //            }
        //            gv_consignments.DataSource = dt;
        //            gv_consignments.DataBind();
        //            if (Session["U_NAME"].ToString().ToUpper() != "affaq.qamar@mulphilog.com".ToUpper())
        //            {
        //                gv_consignments.Columns[11].Visible = false;
        //                gv_consignments.Columns[13].Visible = false;
        //            }

        //            foreach (GridViewRow row in gv_consignments.Rows)
        //            {
        //                if (dt.Rows[row.RowIndex][12].ToString() == "D-DELIVERED")
        //                {
        //                    gv_consignments.Rows[row.RowIndex].Cells[1].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[1].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[2].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[2].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[3].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[3].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[4].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[4].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[5].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[5].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[6].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[6].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[7].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[7].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[8].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[8].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[9].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[9].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[10].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[10].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[11].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[11].ForeColor = System.Drawing.Color.White;

        //                    gv_consignments.Rows[row.RowIndex].Cells[12].BackColor = System.Drawing.Color.Green;
        //                    gv_consignments.Rows[row.RowIndex].Cells[12].ForeColor = System.Drawing.Color.White;


        //                    if (dt.Rows[row.RowIndex][10].ToString() == "N")
        //                    {
        //                        TextBox txt_amount = (TextBox)gv_consignments.Rows[row.RowIndex].FindControl("txt_amount");
        //                        txt_amount.Text = gv_consignments.Rows[row.RowIndex].Cells[9].Text;
        //                        gv_consignments.Rows[row.RowIndex].Cells[10].Enabled = false;

        //                        CheckBox chk = row.FindControl("chk_check") as CheckBox;
        //                        chk.Checked = true;

        //                        Total += double.Parse(txt_amount.Text);

        //                        gv_consignments.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Center;
        //                        gv_consignments.FooterRow.Cells[10].Font.Bold = true;
        //                        gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", Total);

        //                    }
        //                    ECAmount.Text = "0";
        //                    CashAmount.Text = Total.ToString();
        //                }
        //            }
        //            dr = null;
        //            dr = dt.Select("DepositSlipNo = RunsheetNumber");
        //            if (dr.Count() > 0)
        //            {
        //            }
        //            else
        //            {
        //                gv_consignments.Enabled = true;

        //            }
        //            txt_riderCode.Enabled = false;
        //        }
        //        else
        //        {
        //            gv_consignments.DataSource = null;
        //            gv_consignments.DataBind();
        //            txt_riderCode.Enabled = false;
        //            AlertMessage("No Records Found", "Red");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        gv_consignments.DataSource = null;
        //        gv_consignments.DataBind();
        //        //    txt_runsheetNumber.Enabled = true;
        //        txt_riderCode.Enabled = false;
        //        AlertMessage("Some Error Occured. Please Contact IT Support.", "Red");
        //        return;
        //    }
        //}
        private DataTable getPaymentSource()
        {
            string sql = "SELECT id, Name FROM PaymentSource ps WHERE  id IN (1, 11)";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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
        protected void btn_print_Click(object sender, EventArgs e)
        {
            if (txt_riderCode.Text.Trim() == "")
            {
                AlertMessage("Enter Rider Code", "Red");
                return;
            }
            //    clvar.RunsheetNumber = RSNo;
            Session["VariableObj"] = clvar;
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
            string script = String.Format(script_, "ReceiptPrint_Bulk_New.aspx", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
        public DataTable GetRunsheetFromRider(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sql = $@"SELECT rc.runsheetNumber RunsheetNumber, count(rc.consignmentNumber) CNCOUNT, SUM(ISNULL(cdn.codAmount,0)) CODAmount
                      FROM   Runsheet r
                      INNER JOIN RunsheetConsignment rc
                           ON  rc.runsheetNumber = r.runsheetNumber
                           AND rc.branchCode = r.branchCode
                      INNER JOIN Consignment c 
                           ON  c.consignmentNumber = rc.consignmentNumber 
                      INNER JOIN RiderRunsheet rr
                           ON rr.runsheetNumber = rc.runsheetNumber
                      INNER JOIN Riders r2
                           ON r2.branchId = r.branchCode
                          and r2.riderCode = rr.riderCode
                        INNER JOIN CreditClients cc 
                           ON  cc.id = c.creditClientId AND cc.IsCOD=1
                        inner JOIN CODConsignmentDetail_New cdn 
                           ON  cdn.consignmentNumber = rc.consignmentNumber 
                      LEFT OUTER JOIN PaymentVouchers pv 
                           ON  pv.ConsignmentNo = rc.consignmentNumber 
                           LEFT OUTER JOIN rvdbo.Lookup l 
                           ON  l.AttributeGroup = 'POD_REASON' 
                           AND l.Id = rc.Reason 
						WHERE r.ridercode='{clvar.RiderCode}' 
                           AND r.BranchCode = '{HttpContext.Current.Session["BranchCode"].ToString()}' AND rc.Reason ='123' AND cdn.codAmount-ISNULL(pv.Amount,0)>0  
                    --       AND cast(r.createdOn AS date)>= CAST((GETDATE()-1) AS date)
                        GROUP BY rc.runsheetNumber";

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
        public DataTable GetConsignmentsFromRunsheet(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sql = $@"SELECT c.consignmentNumber      CNNo, 
                      c.consignerAccountNo     AccountNumber, 
                  CASE 
                       WHEN LEN(cc.name) > 21 THEN SUBSTRING(cc.name, 1, 21)
                       ELSE cc.name
                      END                      AccName,
                      CEILING(SUM(ISNULL(cdn.codAmount, '0')) / COUNT(ISNULL(cdn.consignmentNumber, '0'))) CodAmount, 
                      pv.Id                    VoucherID, 
                      pv.ReceiptNo             RRNumber, 
                      CAST(FORMAT(pv.VoucherDate, 'dd-MM-yyyy') AS VARCHAR) RRDate, 
                      FORMAT(pv.CreatedOn, 'dd-MM-yyyy hh:mm tt') RREntry, 
                      pv.Amount                RRAmount, 
                      pv.RefNo                 RefNumber, 
                      CASE  
                           WHEN pv.Id IS NOT NULL THEN 'Y' 
                           ELSE 'N' 
                      END                      VoucherExists, 
                      rc.runsheetNumber        RunsheetNumber, 
                      l.AttributeValue         RunsheetReason, 
                      r2.riderCode             RiderCode, 
                      r2.expressCenterId       ExpressCenterCode, 
                      cc.id                    CreditClientID,
                      dbo.SplitString(pv.DepositSlipNo, '-', '1') DepositSlipNo, cc.codType, rc.sortOrder,pv.PaymentSourceId,pv.remarks
               FROM   Runsheet r 
                      INNER JOIN RunsheetConsignment rc 
                           ON  rc.runsheetNumber = r.runsheetNumber                
                           AND rc.branchCode = r.branchCode 
                      INNER JOIN RiderRunsheet rr
                           ON rr.runsheetNumber = rc.runsheetNumber
                      INNER JOIN Riders r2 
                           ON r2.branchId = r.branchCode               
                          and r2.riderCode = rr.riderCode                       
                      INNER JOIN Consignment c 
                           ON  c.consignmentNumber = rc.consignmentNumber 
                      INNER JOIN CreditClients cc 
                           ON  cc.id = c.creditClientId 
                      LEFT OUTER JOIN CODConsignmentDetail_New cdn 
                           ON  cdn.consignmentNumber = rc.consignmentNumber 
                      LEFT OUTER JOIN PaymentVouchers pv 
                           ON  pv.ConsignmentNo = c.consignmentNumber 
                      LEFT OUTER JOIN rvdbo.Lookup l 
                           ON  l.AttributeGroup = 'POD_REASON' 
                           AND l.Id = rc.Reason 
                     WHERE rc.runsheetNumber in ({clvar.RunsheetNumber})
                           AND r.BranchCode = '{HttpContext.Current.Session["BranchCode"].ToString()}'
                      AND c.cod = '1' 
               GROUP BY 
                      c.consignmentNumber, 
                      c.consignerAccountNo, 
                      cc.name, 
                      pv.Id, 
                      pv.VoucherDate, 
                      rc.runsheetNumber, 
                      r2.riderCode, 
                      r2.expressCenterId, 
                      cc.id, 
                      pv.ReceiptNo, 
                      pv.CreatedOn, 
                      pv.Amount, 
                      l.AttributeValue, 
                      pv.RefNo,pv.DepositSlipNo, cc.codType, rc.sortOrder,PaymentSourceId,pv.remarks order by rc.sortOrder desc ";


            sql = $@"select x.CNNo,x.AccountNumber,x.AccName,x.CodAmount,x.VoucherID,x.RRNumber,x.RRDate,x.RREntry,x.RRAmount,x.RefNumber,
case when x.RRNumber IS NOT NULL THEN 'Y' ELSE 'N'   END VoucherExists,
x.RunsheetNumber,x.RunsheetReason,x.RiderCode,x.ExpressCenterCode,x.CreditClientID,x.DepositSlipNo,x.CODType,x.SortOrder,x.PaymentSourceId,x.SourceWithAmount, x.remarks
from
(SELECT distinct c.consignmentNumber      CNNo, 
                      c.consignerAccountNo     AccountNumber, 
                  CASE 
                       WHEN LEN(cc.name) > 21 THEN SUBSTRING(cc.name, 1, 21)
                       ELSE cc.name
                      END                      AccName,
                      CEILING(SUM(ISNULL(cdn.codAmount, '0')) / COUNT(ISNULL(cdn.consignmentNumber, '0'))) CodAmount, 
STUFF((SELECT distinct ', ' +cast(p2.id as varchar) 
         from PaymentVouchers p2  where p2.ConsignmentNo=c.consignmentNumber FOR XML PATH(''))  ,1,2,'') VoucherID, 
					  STUFF((SELECT distinct +', ' +cast(p2.ReceiptNo as varchar) 
         from PaymentVouchers p2  where p2.ConsignmentNo=c.consignmentNumber FOR XML PATH(''))  ,1,2,'')             RRNumber, 
                   (SELECT top 1 p2.VoucherDate
         from PaymentVouchers p2  where p2.ConsignmentNo=c.consignmentNumber ) RRDate, 
                    (SELECT top 1 p2.CreatedOn  
         from PaymentVouchers p2  where p2.ConsignmentNo=c.consignmentNumber )  RREntry, 
                    (SELECT SUM(p2.Amount) from PaymentVouchers p2  where p2.ConsignmentNo=c.consignmentNumber )   RRAmount, 
                      STUFF((SELECT distinct ', ' +cast(p2.refNo  as varchar)
         from PaymentVouchers p2  where p2.ConsignmentNo=c.consignmentNumber FOR XML PATH(''))  ,1,2,'')                 RefNumber, 
                --      CASE      WHEN pv.Id IS NOT NULL THEN 'Y'       ELSE 'N'   END          
					  'N' VoucherExists, 
                      rc.runsheetNumber        RunsheetNumber, 
                      l.AttributeValue         RunsheetReason, 
                      r2.riderCode             RiderCode, 
                      r2.expressCenterId       ExpressCenterCode, 
                      cc.id                    CreditClientID,
                    STUFF((SELECT distinct ', ' +cast(dbo.SplitString(p2.DepositSlipNo, '-', '1') as varchar) 
         from PaymentVouchers p2  where p2.consignmentno=c.consignmentNumber FOR XML PATH(''))  ,1,2,'')  DepositSlipNo, cc.codType, rc.sortOrder,
					 STUFF((SELECT distinct ', ' +cast(p2.PaymentSourceId  as varchar) 
         from PaymentVouchers p2  where p2.consignmentno=c.consignmentNumber FOR XML PATH(''))  ,1,2,'') PaymentSourceId,
  STUFF((SELECT distinct ', ' +cast(ps2.Name  as varchar) + ' ('+cast(p2.Amount as varchar)+')'
         from PaymentVouchers p2 inner join PaymentSource ps2 on ps2.id=p2.PaymentSourceId  where p2.consignmentno=c.consignmentNumber FOR XML PATH(''))  ,1,2,'') SourceWithAmount,
STUFF((SELECT distinct ', ' +cast(p2.remarks  as varchar)
         from PaymentVouchers p2  where p2.consignmentno=c.consignmentNumber FOR XML PATH(''))  ,1,2,'') Remarks
               FROM   Runsheet r 
                      INNER JOIN RunsheetConsignment rc 
                           ON  rc.runsheetNumber = r.runsheetNumber                
                           AND rc.branchCode = r.branchCode 
                      INNER JOIN RiderRunsheet rr
                           ON rr.runsheetNumber = rc.runsheetNumber
                      INNER JOIN Riders r2 
                           ON r2.branchId = r.branchCode               
                          and r2.riderCode = rr.riderCode                       
                      INNER JOIN Consignment c 
                           ON  c.consignmentNumber = rc.consignmentNumber 
                      INNER JOIN CreditClients cc 
                           ON  cc.id = c.creditClientId 
                      LEFT OUTER JOIN CODConsignmentDetail_New cdn 
                           ON  cdn.consignmentNumber = rc.consignmentNumber 
                      LEFT OUTER JOIN rvdbo.Lookup l 
                           ON  l.AttributeGroup = 'POD_REASON' 
                           AND l.Id = rc.Reason 
                     WHERE rc.runsheetNumber in ({clvar.RunsheetNumber})
                           AND r.BranchCode = '{HttpContext.Current.Session["BranchCode"].ToString()}'
                      AND c.cod = '1' 
					
               GROUP BY 
                     c.consignmentNumber, 
                      c.consignerAccountNo, 
                      cc.name,  
                      rc.runsheetNumber, 
                      r2.riderCode, 
                      r2.expressCenterId, 
                      cc.id, 
                      l.AttributeValue, cc.codType, rc.sortOrder
					  )x order by x.sortOrder desc 
";

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
        public DataTable GetRiderCashSummary(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sql = $@"SELECT TOP 1 isnull(RiderName, r.ridercode) as RiderName, isnull(Amount,0) Amount, isnull(SubmitWithCashier,0)SubmitWithCashier,
isnull(SubmitWithHBLKonnect,0) SubmitWithHBLKonnect, isnull(Difference,0) Difference, isnull(HBLKonnExtraPaid,0) HBLKonnExtraPaid
,(select top 1 b.name from riders rr inner join branches b on b.branchCode=rr.branchId and rr.ridercode='{clvar.RiderCode}') Location
FROM Runsheet r 
left join (SELECT tc.RiderCode, CONCAT( tc.RiderName,' (', tc.RiderCode,' )') AS RiderName,
(select FORMAT(sum(CollectedAmount),'N0') from tbl_ridercashpayment where cast(RiderCode as varchar) ='{clvar.RiderCode}')  AS Amount,
(select isnull(FORMAT(sum(Amount),'N0'),0) from tbl_RiderCashSummary where cast(RiderCode as varchar) ='{clvar.RiderCode}') as SubmitWithCashier,
(select isnull(FORMAT(sum(CollectedAmount),'N0'),0) from tbl_RiderHBLKonnectPayment where cast(RiderCode as varchar) ='{clvar.RiderCode}') as SubmitWithHBLKonnect,
isnull(FORMAT((select sum(CollectedAmount) from tbl_ridercashpayment where cast(RiderCode as varchar) ='{clvar.RiderCode}') - 
(select isnull(sum(Amount),0) from tbl_RiderCashSummary where cast(RiderCode as varchar)  ='{clvar.RiderCode}') -
isnull((select sum(isnull(CollectedAmount,0)-isnull(ExtraAmount,0)) from tbl_RiderHBLKonnectPayment where cast(RiderCode as varchar) ='{clvar.RiderCode}'),0),'N0'),0) as Difference,
isnull(FORMAT((select sum(ExtraAmount) from tbl_RiderHBLKonnectPayment where cast(RiderCode as varchar) ='{clvar.RiderCode}'),'N0'),0) as HBLKonnExtraPaid 
FROM tbl_ridercashpayment tc WHERE cast(tc.RiderCode as varchar) ='{clvar.RiderCode}'
GROUP BY tc.RiderCode, CONCAT( tc.RiderName,' (', tc.RiderCode,' )')) as xb on cast(xb.RiderCode AS VARCHAR)= cast(r.ridercode AS VARCHAR)
WHERE r.ridercode='{clvar.RiderCode}'";

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
        public string CreateCODBulkVouchers(Cl_Variables clvar, DataTable dt)
        {
            string error = "";

            SqlConnection con = new SqlConnection(clvar.Strcon());

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 300000;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_CreateBulkCODPaymentVouchers_2022";
                cmd.Parameters.AddWithValue("@Runsheet", clvar.RunsheetNumber);
                cmd.Parameters.AddWithValue("@Runsheetwithdash", clvar.SealNumber);
                cmd.Parameters.AddWithValue("@VoucherDate", clvar.VoucherDate);
                cmd.Parameters.AddWithValue("@ZoneCode", clvar.Zone);
                cmd.Parameters.AddWithValue("@BranchCode", clvar.Branch);
                cmd.Parameters.AddWithValue("@CreatedBy", clvar.createdBy);
                cmd.Parameters.AddWithValue("@PaymentTypeID", clvar.PaymentType);
                cmd.Parameters.AddWithValue("@Consignments", dt);
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                error = cmd.Parameters["@ErrorMessage"].SqlValue.ToString();
            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }

            return error;
        }
        public string CreateCODCashSummary()
        {
            string error = "";

            SqlConnection con = new SqlConnection(clvar.Strcon());

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 300000;
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = $@"INSERT INTO tbl_RiderCashSummary(Runsheet,RiderCode, Amount,CreatedBy) VALUES
                                    ('{clvar.RunsheetNumber}','{clvar.RiderCode}',{Convert.ToInt64(ECAmount.Text)}, '{HttpContext.Current.Session["U_ID"].ToString()}')";
                cmd.CommandText = cmd.CommandText.Trim(',');
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                error = cmd.Parameters["@ErrorMessage"].SqlValue.ToString();
            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }

            return error;
        }
        protected void gridview_tab4_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());

                DataTable dt = new DataTable();
                if (gridview_tab4.HeaderRow != null)
                {
                    dt.Columns.Add("Column1");
                    dt.Columns.Add("Column2");
                    dt.Columns.Add("Column3");
                }
                DataRow dr;
                foreach (GridViewRow row in gridview_tab4.Rows)
                {
                    dr = dt.NewRow();

                    dr[0] = ((TextBox)row.Cells[1].FindControl("transHBL")).Text.Replace(" ", "");
                    dr[1] = ((TextBox)row.Cells[2].FindControl("HBLKonnect")).Text.Replace(" ", "");
                    dr[2] = ((LinkButton)row.Cells[3].FindControl("del")).Text.Replace(" ", "");

                    dt.Rows.Add(dr);
                }

                dr = dt.Rows[id];
                dt.Rows.Remove(dr);
                dt.AcceptChanges();
                ViewState["CurrentTable"] = dt;

                gridview_tab4.DataSource = dt;
                gridview_tab4.DataBind();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ((TextBox)gridview_tab4.Rows[i].Cells[1].FindControl("transHBL")).Text = dt.Rows[i][0].ToString();
                    ((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text = dt.Rows[i][1].ToString();
                    ((LinkButton)gridview_tab4.Rows[i].Cells[3].FindControl("del")).Text = dt.Rows[i][2].ToString();
                }
            }
            changeamount(sender, e);
        }
        public (string, decimal, decimal) InsertHBLKonnectRecord(decimal CNsTotalAmount)
        {
            string error = "";
            decimal hbltotalamount = 0; decimal hbloutstanding = 0;
            SqlConnection con = new SqlConnection(clvar.Strcon());
            int checktransIDcount = 0;
            string duptransid = "";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 300000;
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                for (int i = 0; i < gridview_tab4.Rows.Count; i++)
                {
                    duptransid = ((TextBox)gridview_tab4.Rows[i].Cells[1].FindControl("transHBL")).Text;
                    cmd.CommandText = $@"SELECT COUNT(*) AS Count FROM tbl_RiderHBLKonnectPayment rhp WHERE rhp.TransactionID='{((TextBox)gridview_tab4.Rows[i].Cells[1].FindControl("transHBL")).Text}' ";
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            checktransIDcount += reader.GetInt32(0);
                        }
                    reader.Close();
                }


                if (checktransIDcount == 0)
                {
                    for (int i = 0; i < gridview_tab4.Rows.Count; i++)
                    {
                        if (((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text.Replace(" ", "").Replace(",", "") != ""
                                                && ((TextBox)gridview_tab4.Rows[i].Cells[1].FindControl("transHBL")).Text.Replace(" ", "").Replace(",", "") != "")
                        {
                            hbltotalamount += Convert.ToDecimal(((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text);

                            cmd.CommandText = $@"insert into tbl_RiderHBLKonnectPayment(RiderCode, RunsheetNumber, TransactionID, CollectedAmount,CreatedBy,CreatedOn) Values
('{clvar.RiderCode}', '{clvar.RunsheetNumber}','{((TextBox)gridview_tab4.Rows[i].Cells[1].FindControl("transHBL")).Text}', {Convert.ToDecimal(((TextBox)gridview_tab4.Rows[i].Cells[2].FindControl("HBLKonnect")).Text)},'{HttpContext.Current.Session["U_ID"].ToString()}',getdate());";
                            cmd.CommandText = cmd.CommandText.Trim(',');

                            cmd.ExecuteNonQuery();
                            error = cmd.Parameters["@ErrorMessage"].SqlValue.ToString();
                        }
                    }
                    if (gridview_tab4.Rows.Count > 0 && error == "" && hbltotalamount > CNsTotalAmount - (Convert.ToDecimal(ECAmount.Text) + Convert.ToDecimal(CashAmount.Text)))
                    {
                        hbloutstanding = CNsTotalAmount - (hbltotalamount + Convert.ToDecimal(ECAmount.Text) + Convert.ToDecimal(CashAmount.Text));

                        cmd.CommandText = $@"insert into tbl_RiderHBLKonnectPayment(RiderCode, RunsheetNumber, TransactionID, ExtraAmount,CreatedBy,CreatedOn) Values
                        ('{clvar.RiderCode}', '{clvar.RunsheetNumber}','',{hbloutstanding} ,'{HttpContext.Current.Session["U_ID"].ToString()}',getdate());";
                        cmd.CommandText = cmd.CommandText.Trim(',');

                        cmd.ExecuteNonQuery();
                        error = cmd.Parameters["@ErrorMessage"].SqlValue.ToString();
                    }
                }
                else
                {
                    error = $@"Duplicate Transaction ID ({duptransid}) found ";
                }
            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }

            return (error, hbltotalamount, hbloutstanding);
        }
        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
            return;
        }
        protected void gv_consignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string VoucherExists = (e.Row.FindControl("hd_voucherExists") as HiddenField).Value;
                string CNStatus = e.Row.Cells[4].Text;
                if (VoucherExists.ToUpper() == "TRUE" || VoucherExists == "1" || VoucherExists.ToUpper() == "Y")
                {
                    e.Row.Enabled = false;
                    CheckBox chk = e.Row.FindControl("chk_check") as CheckBox;
                    chk.Enabled = false;
                    chk.Checked = false;

                    TextBox TextBox1 = (TextBox)e.Row.FindControl("txt_amount");
                    decimal RRAmount = Decimal.Parse(TextBox1.Text);
                    totalAmount += RRAmount;
                }
                if (CNStatus == "D-DELIVERED")
                {
                    e.Row.Enabled = false;
                    CheckBox chk = e.Row.FindControl("chk_check") as CheckBox;
                    chk.Enabled = false;
                    chk.Checked = false;
                }

                var PaymentSourceId = (HiddenField)e.Row.FindControl("hd_PaymentSourceId");
                var ddl_PaymentSource = (DropDownList)e.Row.FindControl("ddl_PaymentSource");
                ddl_PaymentSource.DataSource = dt_paymentSource;
                ddl_PaymentSource.DataTextField = "Name";
                ddl_PaymentSource.DataValueField = "id";
                ddl_PaymentSource.DataBind();
                if (PaymentSourceId.Value != "")
                {
                    ddl_PaymentSource.SelectedValue = PaymentSourceId.Value;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                HyperLink footerTotal = (HyperLink)e.Row.FindControl("footerTotal");


                footerTotal.Text = totalAmount.ToString("N0");
            }
        }
        protected void add_hblkonn(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)gridview_tab4.Rows[rowIndex].Cells[1].FindControl("transHBL");
                        TextBox box2 = (TextBox)gridview_tab4.Rows[rowIndex].Cells[2].FindControl("HBLKonnect");
                        LinkButton box3 = (LinkButton)gridview_tab4.Rows[rowIndex].Cells[0].FindControl("del");
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    gridview_tab4.DataSource = dtCurrentTable;
                    gridview_tab4.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks  
            SetPreviousData(sender, e);
        }
        private void SetPreviousData(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)gridview_tab4.Rows[rowIndex].Cells[1].FindControl("transHBL");
                        TextBox box2 = (TextBox)gridview_tab4.Rows[rowIndex].Cells[2].FindControl("HBLKonnect");
                        LinkButton box3 = (LinkButton)gridview_tab4.Rows[rowIndex].Cells[3].FindControl("del");
                        box1.Text = dt.Rows[i]["Column1"].ToString();
                        box2.Text = dt.Rows[i]["Column2"].ToString();
                        box3.Text = dt.Rows[i]["Column3"].ToString() == "" ? "Delete" : dt.Rows[i]["Column3"].ToString();
                        rowIndex++;
                    }
                }
                else
                {
                    SetInitialRow();
                    changeamount(sender, e);
                }
            }
        }
        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            dr = dt.NewRow();
            dr["Column1"] = string.Empty;
            dr["Column2"] = string.Empty;
            dr["Column3"] = string.Empty;
            dt.Rows.Add(dr);
            //Store the DataTable in ViewState  
            ViewState["CurrentTable"] = dt;
            gridview_tab4.DataSource = dt;
            gridview_tab4.DataBind();
        }
        public void ResetAll()
        {
            fld_amounts.Visible = false;
            gridview_tab4.DataSource = null;
            gridview_tab4.DataBind();
            gridview_tab4.Enabled = false;
            SetInitialRow();
            //gridview_tab4.Visible = false;
            ddl_PaymentSource.ClearSelection();
            ddl_PaymentSource.SelectedValue = "1";
            txt_riderCode.Text = "";
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
            gv_runsheet.DataSource = null;
            gv_runsheet.DataBind();
            gv_summary.DataSource = null;
            gv_summary.DataBind();
            ECAmount.Text = "";
            CashAmount.Text = "";
            txt_riderCode.Enabled = true;
        }
        protected DataTable GetReportAccess(string UserId)
        {
            string sql = "SELECT zu.U_NAME,zu.[Profile] FROM ZNI_USER1 zu \n"
               + "INNER JOIN Profile_Detail pd ON zu.[Profile] = pd.Profile_Id \n"
               //+ "WHERE pd.ChildMenu_Id = '944' \n"
               + "WHERE pd.ChildMenu_Id = '851' \n"
               + "AND zu.[STATUS] = '1'  \n"
               + "and zu.U_ID = '" + UserId + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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
        protected void btn_changeRider_Click(object sender, ImageClickEventArgs e)
        {
            gv_runsheet.DataSource = null;
            gv_runsheet.DataBind();
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
            txt_riderCode.Enabled = true;
            txt_riderCode.Text = "";
            gv_summary.DataSource = null;
            gv_summary.DataBind();
            ECAmount.Text = "";
            CashAmount.Text = "";
            if (!txt_riderCode.Enabled)
            {

                AlertMessage("Enter New Rider Code", "Red");
            }
            else
            {
                AlertMessage("Enter Rider Code", "Red");
            }

            return;

        }
    }
}