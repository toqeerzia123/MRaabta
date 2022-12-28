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
    public partial class ReceiptVoucherCODBulk : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        Cl_Receipts rec = new Cl_Receipts();
        LoadingPrintReport enc_ = new LoadingPrintReport();
        DataTable dt_paymentSource;

        decimal totalAmount = 0M;
        double Total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_date.Text == "")
            {
                AlertMessage("Enter Date", "Red");
                return;
            }

            string UserId = Session["U_ID"].ToString();
            DataTable dtUserId = GetReportAccess(UserId);

            if (dtUserId.Rows.Count > 0)
            {

            }
            else
            {
                Response.Redirect("~/login");
            }

            DataTable dtDate = MinimumDate();

            DateTime SelectedDate = DateTime.Parse(txt_date.Text);

            DateTime minAllowedDate = DateTime.Parse(dtDate.Rows[0]["DateAllowed"].ToString()).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;
            if (SelectedDate < minAllowedDate)
            {
                AlertMessage("Month End has already been performed for selected Date", "Red");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Month End has already been performed for selected Date.')", true);
                return;
            }
            if (SelectedDate > maxAllowedDate)
            {
                AlertMessage("Invalid Date", "Red");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Date')", true);
                return;
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
                    //else if (codType == "2" && (tempVoucherAmount <= 0 || tempVoucherAmount < tempCodAmount))
                    //{
                    //    row.BackColor = System.Drawing.Color.LightPink;
                    //}
                    //else
                    //{
                    //    row.BackColor = System.Drawing.Color.White;
                    //}
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

            clvar.RunsheetNumber = txt_runsheetNumber.Text;
            clvar.SealNumber = txt_runsheetNumber.Text + "-1";
            clvar.PaymentType = dd_paymentSource.SelectedValue;
            clvar.VoucherDate = DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd");
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            clvar.createdBy = HttpContext.Current.Session["U_ID"].ToString();

            long countamount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                countamount += Convert.ToInt64(dt.Rows[i]["CODAmount"].ToString());
            }
            if(ECAmount.Text != "0" && ECAmount.Text.Length > 0)
            {
                long difference = Convert.ToInt64(gv_summary.Rows[0].Cells[3].Text.Replace(",", "").ToString());
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
                        CashAmount.Text = (countamount - Convert.ToInt64(ECAmount.Text)).ToString();
                    }
                    gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", countamount);
                    CreateCODCashSummary();
                }
            }
            string error = CreateCODBulkVouchers(clvar, dt);

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
                clvar.RunsheetNumber = txt_runsheetNumber.Text;
                Session["VariableObj"] = clvar;
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                string script = String.Format(script_, "ReceiptPrint_Bulk.aspx", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                ResetAll();
                return;
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
                    chk.Checked = true;
                    Total += double.Parse(txt_amount.Text);
                }
                else
                {
                    chk.Checked = false;
                }
            }
            long difference = Convert.ToInt64(gv_summary.Rows[0].Cells[3].Text.Replace(",", "").ToString());
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
        }

        protected void changeamount(object sender, EventArgs e)
        {
            double Total = 0;long checkedcount = 0;
            for (int i = 0; i < gv_consignments.Rows.Count; i++)
            {
                TextBox txt_amount = (TextBox)gv_consignments.Rows[i].FindControl("txt_amount");
                txt_amount.Text = gv_consignments.Rows[i].Cells[9].Text;
                gv_consignments.Rows[i].Cells[10].Enabled = false;

                CheckBox chk = gv_consignments.Rows[i].FindControl("chk_check") as CheckBox;

                if (chk.Checked)
                {
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
            long difference = Convert.ToInt64(gv_summary.Rows[0].Cells[3].Text.Replace(",", "").ToString());
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


            //if (Convert.ToInt64(Total) > Convert.ToInt64(ECAmount.Text.ToString()))
            //{
            //    CashAmount.Text = (Convert.ToInt64(Total) - Convert.ToInt64(ECAmount.Text.ToString())).ToString();
            //}
            //else
            //{
            //    CashAmount.Text = "0";
            //}

            gv_consignments.FooterRow.Cells[10].Text = string.Format("{0:N0}", Total);

        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            clvar.RunsheetNumber = txt_runsheetNumber.Text;
            clvar.RiderCode = txt_riderCode.Text;
            DataTable ds = GetRiderCashSummary(clvar);
            ECAmount.Text = "";
            if (ds.Rows.Count > 0)
            {
                gv_summary.DataSource = ds;
                gv_summary.DataBind();
                gv_summary.Visible = true;
                gv_summary.Enabled = true;
            }
            DataTable dt = GetConsignmentsFromRunsheet(clvar);
            dt_paymentSource = getPaymentSource();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("RRNumber <> '' and DepositSlipNo <> '" + txt_runsheetNumber.Text.Trim() + "'");
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
                                chk.Checked = true;

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
                    dr = dt.Select("DepositSlipNo = '" + txt_runsheetNumber.Text + "'");
                    if (dr.Count() > 0)
                    {
                        //gv_consignments.Enabled = false;
                        //btn_save.Enabled = false;
                        //AlertMessage("RR Already Generated for This Runsheet. New RR not Allowed on this Runsheet","Red");
                        //return;
                    }
                    else
                    {
                        gv_consignments.Enabled = true;

                    }
                    txt_runsheetNumber.Enabled = false;
                }
                else
                {
                    gv_consignments.DataSource = null;
                    gv_consignments.DataBind();
                    txt_runsheetNumber.Enabled = true;
                    AlertMessage("No Records Found", "Red");
                    return;
                }
            }
            else
            {
                gv_consignments.DataSource = null;
                gv_consignments.DataBind();
                txt_runsheetNumber.Enabled = true;
                AlertMessage("Some Error Occured. Please Contact IT Support.", "Red");
                return;
            }
        }

        private DataTable getPaymentSource()
        {
            string sql = "SELECT * \n"
                 + "FROM   PaymentSource ps \n"
                 + "WHERE  id IN (1, 6, 8)";
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
            if (txt_runsheetNumber.Text.Trim() == "")
            {
                AlertMessage("Enter Runsheet Number", "Red");
                return;
            }
            clvar.RunsheetNumber = txt_runsheetNumber.Text;
            Session["VariableObj"] = clvar;
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
            string script = String.Format(script_, "ReceiptPrint_Bulk.aspx", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
        public DataTable GetConsignmentsFromRunsheet(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT c.consignmentNumber      CNNo, \n"
               + "       c.consignerAccountNo     AccountNumber, \n"
               + "   CASE \n"
               + "        WHEN LEN(cc.name) > 21 THEN SUBSTRING(cc.name, 1, 21)\n"
               + "        ELSE cc.name\n"
               + "       END                      AccName,\n"
               + "       CEILING(SUM(ISNULL(cdn.codAmount, '0')) / COUNT(ISNULL(cdn.consignmentNumber, '0'))) CodAmount, \n"
               + "       pv.Id                    VoucherID, \n"
               + "       pv.ReceiptNo             RRNumber, \n"
               + "       CAST(FORMAT(pv.VoucherDate, 'dd-MM-yyyy') AS VARCHAR) RRDate, \n"
               + "       FORMAT(pv.CreatedOn, 'dd-MM-yyyy hh:mm tt') RREntry, \n"
               + "       pv.Amount                RRAmount, \n"
               + "       pv.RefNo                 RefNumber, \n"
               + "       CASE  \n"
               + "            WHEN pv.Id IS NOT NULL THEN 'Y' \n"
               + "            ELSE 'N' \n"
               + "       END                      VoucherExists, \n"
               + "       rc.runsheetNumber        RunsheetNumber, \n"
               + "       l.AttributeValue         RunsheetReason, \n"
               + "       r2.riderCode             RiderCode, \n"
               + "       r2.expressCenterId       ExpressCenterCode, \n"
               + "       cc.id                    CreditClientID,\n"
               + "       dbo.SplitString(pv.DepositSlipNo, '-', '1') DepositSlipNo, cc.codType, rc.sortOrder,pv.PaymentSourceId,pv.remarks\n"
               + "FROM   Runsheet r \n"
               + "       INNER JOIN RunsheetConsignment rc \n"
               + "            ON  rc.runsheetNumber = r.runsheetNumber \n"
               //  + "            AND rc.routeCode = r.routeCode \n"
               + "            AND rc.branchCode = r.branchCode \n"
               + "       INNER JOIN RiderRunsheet rr\n"
               + "            ON rr.runsheetNumber = rc.runsheetNumber\n"
               + "       INNER JOIN Riders r2 \n"
               + "            ON r2.branchId = r.branchCode \n"
               //+ "           AND r2.routeCode = r.routeCode \n"
               + "           and r2.riderCode = rr.riderCode\n"
               + "           --AND r2.status = '1' \n"
               + "       INNER JOIN Consignment c \n"
               + "            ON  c.consignmentNumber = rc.consignmentNumber \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  cc.id = c.creditClientId \n"
               + "       LEFT OUTER JOIN CODConsignmentDetail_New cdn \n"
               + "            ON  cdn.consignmentNumber = rc.consignmentNumber \n"
               + "       LEFT OUTER JOIN PaymentVouchers pv \n"
               + "            ON  pv.ConsignmentNo = c.consignmentNumber \n"
               + "       LEFT OUTER JOIN rvdbo.Lookup l \n"
               + "            ON  l.AttributeGroup = 'POD_REASON' \n"
               + "            AND l.Id = rc.Reason \n"
               + "      WHERE rc.runsheetNumber = '" + clvar.RunsheetNumber + "' \n"
               + "            AND r.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n"
               + "       --AND r2.riderCode = '650' \n"
               + "       AND c.cod = '1' \n"
               + "GROUP BY \n"
               + "       c.consignmentNumber, \n"
               + "       c.consignerAccountNo, \n"
               + "       cc.name, \n"
               + "       pv.Id, \n"
               + "       pv.VoucherDate, \n"
               + "       rc.runsheetNumber, \n"
               + "       r2.riderCode, \n"
               + "       r2.expressCenterId, \n"
               + "       cc.id, \n"
               + "       pv.ReceiptNo, \n"
               + "       pv.CreatedOn, \n"
               + "       pv.Amount, \n"
               + "       l.AttributeValue, \n"
               + "       pv.RefNo,pv.DepositSlipNo, cc.codType, rc.sortOrder,PaymentSourceId,pv.remarks order by rc.sortOrder desc \n"
               + "";
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

            string sql = $@" select isnull(RiderName, r.ridercode) as RiderName, isnull(Amount,0) Amount, isnull(SubmitWithCashier,0)SubmitWithCashier, isnull(Difference,0)Difference
                            FROM Runsheet r 
                            left join (SELECT tc.RiderCode, CONCAT( tc.RiderName,' (', tc.RiderCode,' )') AS RiderName,
                            (select FORMAT(sum(CollectedAmount),'N0') from tbl_ridercashpayment where cast(RiderCode as varchar) =(select r.ridercode FROM Runsheet r WHERE r.runsheetNumber='{clvar.RunsheetNumber}'))  AS Amount,
                            (select isnull(FORMAT(sum(Amount),'N0'),0) from tbl_RiderCashSummary where cast(RiderCode as varchar) =(select r.ridercode FROM Runsheet r WHERE r.runsheetNumber='{clvar.RunsheetNumber}')) as SubmitWithCashier,
                            isnull(FORMAT((select sum(CollectedAmount) from tbl_ridercashpayment where cast(RiderCode as varchar) =(select r.ridercode FROM Runsheet r WHERE r.runsheetNumber='{clvar.RunsheetNumber}')) - 
                            (select isnull(sum(Amount),0) from tbl_RiderCashSummary where cast(RiderCode as varchar)  =(select r.ridercode  FROM Runsheet r WHERE r.runsheetNumber='{clvar.RunsheetNumber}')),'N0'),0) as Difference
                            FROM tbl_ridercashpayment tc WHERE cast(tc.RiderCode as varchar) =(select r.ridercode  FROM Runsheet r WHERE r.runsheetNumber='{clvar.RunsheetNumber}')
                             GROUP BY tc.RiderCode, CONCAT( tc.RiderName,' (', tc.RiderCode,' )')) as xb on xb.RiderCode = r.ridercode
                            WHERE r.runsheetNumber='{clvar.RunsheetNumber}'";



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
                cmd.CommandText = "MnP_CreateBulkCODPaymentVouchers_new";
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
                                    ('{clvar.RunsheetNumber}',(SELECT ridercode FROM Runsheet r WHERE r.runsheetNumber='{clvar.RunsheetNumber}'),{Convert.ToInt64(ECAmount.Text)}, '{HttpContext.Current.Session["U_ID"].ToString()}')";
                cmd.CommandText = cmd.CommandText.Trim(',');

                //cmd.CommandText = "INSERT INTO tbl_RiderCashSummary(Runsheet,CN,RiderCode, Amount,CreatedBy) VALUES";

                //for(int i = 0; i < dt.Rows.Count; i++)
                //{
                //    cmd.CommandText += $@"('{dt.Rows[i]["RunsheetNumber"]}','{dt.Rows[i]["ConsignmentNumber"]}','{dt.Rows[i]["RiderCode"]}',{dt.Rows[i]["CODAmount"]}, '{HttpContext.Current.Session["U_ID"].ToString()}'),";
                //}
                //cmd.CommandText = cmd.CommandText.Trim(',');
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                error = cmd.Parameters["@ErrorMessage"].SqlValue.ToString();
            }
            catch (Exception ex)
            { error = ex.Message; }
            finally { con.Close(); }

            return error;
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

        protected DataTable MinimumDate()
        {
            string sql = "SELECT MAX(isnull(d.DateTime, Getdate()))        DateAllowed \n"
               + "FROM   Mnp_Account_DayEnd     d \n"
               + "WHERE  d.Doc_Type = 'R' \n"
               + "       AND d.Branch = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
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
        public void ResetAll()
        {
            txt_runsheetNumber.Text = "";
            txt_riderCode.Text = "";
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
            gv_summary.DataSource = null;
            gv_summary.DataBind();
            ECAmount.Text = "";
            CashAmount.Text = "";

        }

        protected DataTable GetReportAccess(string UserId)
        {
            string sql = "SELECT zu.U_NAME,zu.[Profile] FROM ZNI_USER1 zu \n"
               + "INNER JOIN Profile_Detail pd ON zu.[Profile] = pd.Profile_Id \n"
               + "WHERE pd.ChildMenu_Id = '201' \n"
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

        protected void btn_changeRunsheer_Click(object sender, ImageClickEventArgs e)
        {
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
            txt_runsheetNumber.Enabled = true;
            txt_runsheetNumber.Text = "";
            if (!txt_runsheetNumber.Enabled)
            {

                AlertMessage("Enter New Runsheet Number", "Red");
            }
            else
            {
                AlertMessage("Enter Runsheet Number", "Red");
            }

            return;

        }
    }
}