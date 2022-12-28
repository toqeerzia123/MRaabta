using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;

namespace MRaabta.Files
{
    public partial class ReceiptVoucher : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        Cl_Receipts rec = new Cl_Receipts();
        LoadingPrintReport enc_ = new LoadingPrintReport();

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_cnNo.Text = "";
            txt_cnNo.Enabled = false;
            rbtn_cod.Enabled = false;
            if (dd_paySource.SelectedValue != "1" && dd_paySource.SelectedValue != "0" && dd_paySource.SelectedValue != "6" && dd_paySource.SelectedValue != "7")
            {
                dd_banks.Enabled = true;
                txt_chequeNo.Enabled = true;
                picker_chequeDate.Enabled = true;

                if (dd_paySource.SelectedValue == "2")
                {
                    dd_depositSlipBank.Enabled = true;
                    txt_dslipNo.Enabled = true;
                }
                else
                {
                    dd_depositSlipBank.Enabled = false;
                    txt_dslipNo.Enabled = false;
                }
            }
            else
            {
                dd_banks.Enabled = false;
                txt_chequeNo.Enabled = false;
                picker_chequeDate.Enabled = false;
                dd_depositSlipBank.Enabled = false;
                txt_dslipNo.Enabled = false;
            }

            if (dd_type.SelectedValue == "4")
            {
                dd_EC.Enabled = true;
            }
            else
            {
                if (rbtn_paymentMode.SelectedValue == "1")
                {
                    dd_EC.Enabled = true;
                }
                else
                {
                    dd_EC.Enabled = false;
                }

            }
            if (dd_type.SelectedValue == "1")
            {
                cpr.Style.Add("display", "block");
                stw.Style.Add("display", "none");
            }
            else if (dd_type.SelectedValue == "10")
            {
                cpr.Style.Add("display", "none");
                stw.Style.Add("display", "block");
            }
            else
            {
                cpr.Style.Add("display", "none");
                stw.Style.Add("display", "none");
            }

            Errorid.Text = "";
            if (dd_paySource.SelectedValue == "2")
            {

            }
            try
            {
                string temp = HttpContext.Current.Session["BranchCode"].ToString();
            }
            catch (Exception ex)
            {

                Errorid.Text = "Session Expired. Please Re-Login";
                Errorid.ForeColor = Color.Red;
                return;
            }


            if (rbtn_paymentMode.SelectedValue == "1")
            {
                rbnt_byEC_CheckedChanged(sender, e);

            }




            if (!IsPostBack)
            {
                ExpressCenters();
                PaymentVoucherTypes();
                PaymentSources();
                Banks();
                Groups();
                Products();
                //picker_voucherDate.SelectedDate = DateTime.Now;// DateTime.Parse(Session["WorkingDate"].ToString());
                picker_voucherDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        protected void ExpressCenters()
        {
            clvar.origin = HttpContext.Current.Session["BranchCode"].ToString();
            DataTable dt = ExpressCenterOrigin(clvar).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_EC.DataSource = dt;
                    dd_EC.DataTextField = "ECname";
                    dd_EC.DataValueField = "expressCenterCode";
                    dd_EC.DataBind();
                }
            }
        }

        protected void PaymentVoucherTypes()
        {
            string mode = rbtn_paymentMode.SelectedValue;

            DataTable dt = GetPaymentVoucherTypes(mode);
            dd_type.Items.Clear();
            dd_type.Items.Add(new ListItem { Text = "Select Type", Value = "0" });
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_type.DataSource = dt;
                    dd_type.DataTextField = "NAME";
                    dd_type.DataValueField = "ID";
                    dd_type.DataBind();
                }
            }
        }

        protected void PaymentSources()
        {
            string PaymentType = dd_type.SelectedValue;
            DataTable dt = new DataTable();
            if (rbtn_paymentMode.SelectedValue == "2" || dd_type.SelectedValue == "11" || dd_type.SelectedValue == "15")
            {
                dt = GetPaymentSources(PaymentType);

                dd_paySource.Enabled = true;
            }
            else
            {
                dt = GetPaymentSources(PaymentType);

                dd_paySource.Enabled = true;

            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_paySource.DataSource = dt;
                    dd_paySource.DataTextField = "NAME";
                    dd_paySource.DataValueField = "ID";
                    dd_paySource.DataBind();
                }
            }
        }

        public DataTable GetPaymentSources(string PaymentType)
        {
            string query = "";
            if (rbtn_paymentMode.SelectedValue == "2")
            {
                query = "select * from PaymentSource ps WHERE ps.Id IN (SELECT t.value FROM StringToRows((SELECT pt.paymentSource FROM PaymentTypes pt WHERE pt.Id = '" + PaymentType + "'), ',') t) and status='1' ";

            }
            else
            {
                query = "select * from PaymentSource ps WHERE ps.Id IN (SELECT t.value FROM StringToRows((SELECT pt.paymentSource FROM PaymentTypes pt WHERE pt.Id = '" + PaymentType + "'), ',') t) and status='1' ";
            }

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable GetPaymentVoucherTypes(string mode)
        {
            string query = "select * from PaymentTypes where id != '5' and status = '" + mode + "' order by sortOrder";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        protected void Banks()
        {
            DataTable dt = rec.GetBanks();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable nonMnpBanks = dt.Select("isMNPBank = FALSE").CopyToDataTable();
                    dd_banks.DataSource = nonMnpBanks;
                    dd_banks.DataTextField = "NAME";
                    dd_banks.DataValueField = "ID";
                    dd_banks.DataBind();
                    DataTable mnpBanks = dt.Select("isMNPBank = TRUE").CopyToDataTable();
                    dd_depositSlipBank.DataSource = mnpBanks;
                    dd_depositSlipBank.DataTextField = "NAME";
                    dd_depositSlipBank.DataValueField = "ID";
                    dd_depositSlipBank.DataBind();
                }
            }
        }

        protected void Groups()
        {
            DataTable dt = GetClientGroups();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_group.DataSource = dt;
                    dd_group.DataTextField = "NAME";
                    dd_group.DataValueField = "ID";
                    dd_group.DataBind();
                }
            }
        }
        public DataTable GetClientGroups()
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = "select * from ClientGroups cg where status = 1 and cg.collectionCenter='" + HttpContext.Current.Session["BranchCode"].ToString() + "'  order by name";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        protected void Products()
        {
            DataTable dt = GetProducts();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_productWiseAmount.DataSource = dt;
                    gv_productWiseAmount.DataBind();
                }
            }
        }

        protected void rbnt_byEC_CheckedChanged(object sender, EventArgs e)
        {

            //RadioButton rbtn = (RadioButton)sender;
            if (rbnt_byExpressCenter.Checked)
            {
                //dd_rider.Enabled = false;
                txt_rider.Enabled = false;
                txt_cnNo.Enabled = false;
                txt_saleAmt.Enabled = false;
                if (dd_type.SelectedValue == "15" || dd_type.SelectedValue == "11")
                {
                    dd_paySource.Enabled = true;
                }
                else
                {
                    //dd_paySource.Enabled = false;
                }

                txt_clientAcNo.Enabled = false;
                txt_clientName.Enabled = false;
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                txt_chequeNo.Enabled = false;
                chk_centralized.Enabled = false;
                dd_group.Enabled = false;
                txt_clientGroup.Enabled = false;
                txt_cnNo.Text = "";
                txt_clientAcNo.Text = "";
                txt_saleAmt.Text = "";
                txt_clientName.Text = "";
            }
            else if (rbtn_byRider.Checked)
            {
                txt_cnNo.Enabled = false;
                //dd_rider.Enabled = true;
                txt_rider.Enabled = true;
                txt_saleAmt.Enabled = false;

                if (dd_type.SelectedValue == "15" || dd_type.SelectedValue == "11")
                {
                    dd_paySource.Enabled = true;
                }
                else
                {
                }
                txt_clientAcNo.Enabled = false;
                txt_clientName.Enabled = false;
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                txt_chequeNo.Enabled = false;
                chk_centralized.Enabled = false;
                dd_group.Enabled = false;
                txt_clientGroup.Enabled = false;
                txt_cnNo.Text = "";
                txt_clientAcNo.Text = "";
                txt_saleAmt.Text = "";
                txt_clientName.Text = "";
            }
            else
            {
                txt_cnNo.Enabled = false;
                //dd_rider.Enabled = true;
                txt_rider.Enabled = true;
                txt_saleAmt.Enabled = false;
                dd_paySource.Enabled = false;
                txt_clientAcNo.Enabled = false;
                txt_clientName.Enabled = false;
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                txt_chequeNo.Enabled = false;
                chk_centralized.Enabled = false;
                dd_group.Enabled = false;
                txt_clientGroup.Enabled = false;
            }
        }
        protected void rbtn_formMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_reset_Click(sender, e);

            if (rbtn_formMode.SelectedValue == "2")
            {
                rbtn_paymentMode.Enabled = false;
                panel1.Enabled = false;
                txt_voucherNo.Enabled = true;
                rbtn_paymentMode.ClearSelection();
                dd_EC.ClearSelection();
                dd_group.ClearSelection();
                dd_paySource.ClearSelection();
                dd_type.ClearSelection();
                rbtn_byRider.Checked = false;
                rbtn_cod.Checked = false;
                rbnt_byExpressCenter.Checked = false;
                dd_banks.ClearSelection();
                picker_voucherDate.Enabled = false;
                bt_ReportView.Enabled = true;
            }
            else
            {
                txt_voucherNo.Text = "";
                rbtn_paymentMode.Enabled = true;
                panel1.Enabled = true;
                txt_voucherNo.Enabled = false;
                rbtn_paymentMode.SelectedValue = "1";
                rbtn_paymentMode_SelectedIndexChanged(sender, e);
                rbnt_byExpressCenter.Checked = true;
                picker_voucherDate.Enabled = true;
                bt_ReportView.Enabled = false;

            }
        }

        protected void rbtn_paymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaymentVoucherTypes();
            if (rbtn_paymentMode.SelectedValue == "1")
            {
                dd_EC.Enabled = true;
                rbnt_byEC_CheckedChanged(sender, e);
                rbtn_byRider.Enabled = true;
                rbnt_byExpressCenter.Enabled = true;
                rbtn_cod.Enabled = false;
                txt_receiptNo.Enabled = false;
                txt_receiptNo.Text = "";
                PaymentSources();
            }
            else
            {
                txt_receiptNo.Enabled = true;
                txt_receiptNo.Text = "";
                dd_EC.Enabled = false;
                txt_saleAmt.Enabled = false;
                txt_clientName.Enabled = false;
                txt_chequeNo.Enabled = false;
                dd_group.Enabled = false;
                txt_clientGroup.Enabled = false;
                txt_cnNo.Enabled = false;
                rbnt_byExpressCenter.Enabled = false;
                rbtn_byRider.Enabled = false;
                rbtn_cod.Enabled = false;
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                rbtn_byRider.Enabled = false;
                rbnt_byExpressCenter.Enabled = false;
                rbtn_cod.Enabled = false;
                dd_paySource.Enabled = true;
                chk_centralized.Enabled = true;
                txt_clientAcNo.Enabled = true;
                PaymentSources();

            }
        }
        protected void dd_paySource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_paySource.SelectedItem.Text.ToUpper() == "CHEQUE")
            {
                txt_dslipNo.Enabled = true;
                dd_depositSlipBank.Enabled = true;
            }
            else
            {
                txt_dslipNo.Enabled = false;
                dd_depositSlipBank.Enabled = false;
            }
            if (dd_paySource.SelectedItem.Text.ToUpper() != "CASH")
            {
                dd_banks.Enabled = true;
                picker_chequeDate.Enabled = true;
                txt_chequeNo.Enabled = true;
            }
            else
            {
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                txt_chequeNo.Enabled = false;
            }
        }
        protected void dd_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            dd_paySource.Items.Clear();
            dd_paySource.Items.Add(new ListItem { Text = "Select Payment Source", Value = "0" });
            PaymentSources();
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (dd_paySource.SelectedValue.ToString() == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Payment Source')", true);
                    return;
                }


                loader.Style.Add("display", "none");

                btnDiv.Style.Add("display", "block");
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                DataTable dates = MinimumDate(clvar);
                if (dates != null)
                {
                    if (dates.Rows[0][0].ToString().Trim() != "")
                    {
                        //DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
                        DateTime minAllowedDate = ((DateTime)dates.Rows[0]["DateAllowed"]).AddDays(1);
                        DateTime maxAllowedDate = DateTime.Now;
                        if (DateTime.Parse(picker_voucherDate.Text) < minAllowedDate || DateTime.Parse(picker_voucherDate.Text) > maxAllowedDate)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot Create Vocher')", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End Has never been performed for your Branch. Contact Head Office Accounts Department.')", true);

                        return;
                    }
                }
                else
                {

                }

                if (picker_voucherDate.Text.ToString() == "")
                {
                    Errorid.Text = "Please Provide Missing Fields";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide Missing Fields')", true);
                    return;
                }

                if (dd_type.SelectedValue == "0")
                {
                    Errorid.Text = "Select Type";
                    Errorid.ForeColor = Color.Red;

                    return;
                }

                if (dd_type.SelectedValue == "1" && txt_cprNo.Text.Trim() == "")
                {
                    Errorid.Text = "Enter CPR Number";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter CPR Number')", true);
                    return;
                }
                else if (dd_type.SelectedValue == "1" && txt_cprNo.Text.Trim() != "")
                {
                    if (!IsValidCPR(txt_cprNo.Text.Trim()))
                    {
                        Errorid.Text = "CPR Number already Exists";
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CPR Number already Exists')", true);
                        return;
                    }
                }
                else if (dd_type.SelectedValue == "10" && txt_stwNo.Text.Trim() == "")
                {
                    Errorid.Text = "Enter STW Number";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Enter STW Number')", true);
                    return;
                }



                float totalVoucherAmount = 0f;
                float.TryParse(txt_amount.Text, out totalVoucherAmount);
                float productAmounts = 0f;

                DataTable productBreakDown = new DataTable();
                productBreakDown.Columns.AddRange(new DataColumn[]
                    {
                   new DataColumn("Product", typeof(string)),
                   new DataColumn("Amount", typeof(float))

                    });
                foreach (GridViewRow row in gv_productWiseAmount.Rows)
                {
                    DataRow dr = productBreakDown.NewRow();
                    if ((row.FindControl("txt_amount") as TextBox).Text.Trim() != "")
                    {
                        dr["Product"] = (row.FindControl("hd_product") as HiddenField).Value;
                        dr["Amount"] = float.Parse((row.FindControl("txt_amount") as TextBox).Text);
                        productAmounts += float.Parse((row.FindControl("txt_amount") as TextBox).Text);
                        productBreakDown.Rows.Add(dr);
                    }
                }

                if (productAmounts != totalVoucherAmount /*&& dd_type.SelectedValue == "4"*/)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vocuher Amount and Product Amounts do not Match. Please Recalculate.')", true);
                    Errorid.Text = "Voucher Amount and Product Amounts do not Match. Please Recalculate.";
                    return;
                }

                if (rbtn_paymentMode.SelectedValue == "2")
                {
                    #region MyRegion
                    #endregion

                    #region Credit Ka Scene
                    if (dd_paySource.SelectedValue == "2" && (dd_depositSlipBank.SelectedValue == "0" || txt_dslipNo.Text.Trim() == ""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Deposit Slip Details')", true);
                        Errorid.Text = "Provide Deposit Slip Details.";
                        return;
                    }
                    //  DataTable dt = rec.ReceiptNo();

                    clvar.ReceiptNo = txt_receiptNo.Text;//dt.Rows[0]["codeValue"].ToString();
                    clvar.RefNo = txt_refNo.Text;
                    clvar.VoucherDate = DateTime.Parse(picker_voucherDate.Text).ToString("yyyy-MM-dd");
                    clvar.PaymentSource = dd_paySource.SelectedValue;
                    if (dd_type.SelectedValue == "4")
                    {
                        if (dd_EC.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Express Center')", true);
                            Errorid.Text = "Select Express Center.";
                            return;
                        }
                        clvar.expresscenter = dd_EC.SelectedValue;
                    }
                    if (chk_centralized.Checked)
                    {
                        clvar.ClientGroupID = dd_group.SelectedValue;
                        clvar.IsCentralized = true;
                    }
                    else
                    {
                        clvar.CreditClientID = creditClientID.Value;
                        clvar.IsCentralized = false;
                    }
                    clvar.PaymentSource = dd_paySource.SelectedValue;



                    if (clvar.PaymentSource != "1")
                    {
                        if (picker_chequeDate.Text.ToString() == "")
                        {
                            Errorid.Text = "Please Provide Cheque Date";
                            Errorid.ForeColor = System.Drawing.Color.Red;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide Cheque Date')", true);
                            return;
                        }
                        clvar.Bank = dd_banks.SelectedValue;
                        clvar.ChequeDate = DateTime.Parse(picker_chequeDate.Text).ToString("yyyy-MM-dd");
                        clvar.ChequeNo = txt_chequeNo.Text;
                    }

                    clvar.IsByCreditClientID = true;

                    clvar.amount = double.Parse(txt_amount.Text);
                    clvar.PaymentType = dd_type.SelectedValue;

                    if (clvar.PaymentType == "1" && (txt_cprNo.Text.Trim() == "0" || txt_cprNo.Text.Trim() == ""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid CPR Number')", true);
                        return;
                    }

                    string error = GeneratePaymentVoucher(clvar, productBreakDown);

                    if (error.Contains("ERROR"))
                    {

                        Errorid.Text = "Could not Create Voucher. " + error + "";
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Create Voucher.\\n" + error + "')", true);
                        return;


                    }
                    else
                    {
                        if (error == "0")
                        {
                            Errorid.Text = "Could not Create Voucher. Receipt Already Exists";
                            Errorid.ForeColor = System.Drawing.Color.Red;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Create Voucher.\\nReceipt Already Exists')", true);
                            return;
                        }
                        Errorid.Text = "Voucher Saved. Voucher No: " + error + "";
                        Errorid.ForeColor = System.Drawing.Color.Green;


                        // ==================== Recovery SMS Start ====================

                        if (rbtn_paymentMode.SelectedValue.ToString() == "2")
                        {
                            if (dd_paySource.SelectedValue.ToString() == "1")
                            {
                                string AccNo = "", ClientId = "";

                                if (txt_clientAcNo.Text != "")
                                {
                                    AccNo = txt_clientAcNo.Text;
                                    ClientId = "AND CC.ID = '" + creditClientID.Value + "'";
                                }
                                if (dd_group.SelectedValue != "")
                                {
                                    ClientId = "AND CC.id = '" + creditClientID.Value + "'";
                                }

                                //Post_BrandedSMS(picker_voucherDate.Text, txt_amount.Text, ClientId,AccNo, error);
                            }
                        }

                        // ==================== Recovery SMS End ====================


                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher Saved.\\nVoucher No: " + error + "')", true);
                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                        string script = String.Format(script_, "Receipt_Print.aspx?ID=" + error + "&RCode=", "_blank", "");
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                        ResetAll();

                    }
                    #endregion
                }
                else
                {
                    #region MyRegion
                    //ADDED ON 06/08/2016 08:56 AM BY MUHAMMAD RABI////////////////////////////////

                    if (rbtn_cod.Checked)
                    {

                        if (dd_EC.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SELECT EXPRESS CENTER')", true);
                            Errorid.Text = "SELECT EXPRESS CENTER";
                            Errorid.ForeColor = Color.Red;
                            return;
                        }

                        if (txt_rider.Text.Trim() == ""/*dd_rider.SelectedValue == "0"*/)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ENTER RIDER')", true);
                            Errorid.Text = "SELECT RIDER";
                            Errorid.ForeColor = Color.Red;
                            return;
                        }

                        if (txt_cnNo.Text.Trim() == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PROVIDE CN NUMBER')", true);
                            Errorid.Text = "PROVIDE CN NUMBER";
                            Errorid.ForeColor = Color.Red;
                            return;
                        }
                    }
                    else if (rbtn_byRider.Checked)
                    {
                        if (dd_EC.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SELECT EXPRESS CENTER')", true);
                            Errorid.Text = "SELECT EXPRESS CENTER";
                            Errorid.ForeColor = Color.Red;
                            return;
                        }

                        if (txt_rider.Text.Trim() == "" /*dd_rider.SelectedValue == "0"*/)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ENTER RIDER CODE')", true);
                            Errorid.Text = "SELECT RIDER";
                            Errorid.ForeColor = Color.Red;
                            return;
                        }
                    }
                    else if (rbnt_byExpressCenter.Checked)
                    {
                        if (dd_EC.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SELECT EXPRESS CENTER')", true);
                            Errorid.Text = "SELECT EXPRESS CENTER";
                            Errorid.ForeColor = Color.Red;
                            return;
                        }
                    }
                    //ADDED ON 06/08/2016 08:56 AM BY MUHAMMAD RABI//////////////////////////////// 
                    #endregion
                    #region CASH ka Scene
                    #region CASH K SCENE KI VALIDATIONS
                    if (rbnt_byExpressCenter.Checked && dd_EC.SelectedValue == "0")
                    {
                        Errorid.Text = "Select Express Center";
                        return;
                    }
                    else if (rbtn_byRider.Checked && txt_rider.Text.Trim() == ""/*dd_rider.SelectedValue == "0"*/)
                    {
                        Errorid.Text = "Select Rider";
                        return;
                    }
                    else if (rbtn_cod.Checked && txt_cnNo.Text.Trim() == "")
                    {
                        if (/*dd_rider.SelectedValue == "0"*/txt_rider.Text.Trim() == "")
                        {
                            Errorid.Text = "Select Rider";
                            return;
                        }
                        Errorid.Text = "Enter CN Number";
                        return;
                    }

                    if (txt_amount.Text.Trim() == "")
                    {
                        Errorid.Text = "Enter Amount";
                        return;
                    }
                    #endregion


                    clvar.expresscenter = dd_EC.SelectedValue;

                    clvar.CheckCondition = "1";
                    if (rbtn_byRider.Checked)
                    {
                        clvar.riderCode = txt_rider.Text.Trim();// dd_rider.SelectedValue;
                        clvar.CheckCondition = "2";
                    }

                    if (rbtn_cod.Checked)
                    {
                        clvar.riderCode = txt_rider.Text.Trim();// dd_rider.SelectedValue;
                        clvar.consignmentNo = txt_cnNo.Text;
                        clvar.CreditClientID = creditClientID.Value;
                        if (txt_clientAcNo.Text == "0")
                        {
                            clvar.IsByCreditClientID = false;
                        }
                        else
                        {
                            clvar.IsByCreditClientID = true;
                        }
                        clvar.CheckCondition = "3";
                    }

                    //                DataTable dt = rec.ReceiptNo();

                    //clvar.ReceiptNo = dt.Rows[0]["codeValue"].ToString();
                    clvar.RefNo = txt_refNo.Text;
                    clvar.PaymentType = dd_type.SelectedValue;
                    clvar.VoucherDate = DateTime.Parse(picker_voucherDate.Text).ToString("yyyy-MM-dd");
                    clvar.amount = double.Parse(txt_amount.Text);

                    if ((dd_type.SelectedValue == "15" || dd_type.SelectedValue == "11") && dd_paySource.SelectedValue == "0")
                    {
                        AlertMessage("Select Payment Source", "Red");
                        dd_paySource.Enabled = true;
                        return;
                    }
                    else if ((dd_type.SelectedValue == "15" || dd_type.SelectedValue == "11") && dd_paySource.SelectedValue != "1")
                    {
                        if (dd_banks.SelectedValue == "0")
                        {
                            AlertMessage("Select Bank", "Red");

                            return;
                        }

                        if (picker_chequeDate.Text.Trim() == "")
                        {
                            AlertMessage("Select Bank", "Red");

                            return;
                        }
                        if (txt_chequeNo.Text.Trim() == "")
                        {
                            AlertMessage("Select Bank", "Red");

                            return;
                        }
                        if (dd_paySource.SelectedValue == "2")
                        {
                            if (dd_depositSlipBank.SelectedValue == "0")
                            {
                                AlertMessage("Select Deposit Slip Bank", "Red");
                                return;
                            }
                            if (txt_dslipNo.Text.Trim() == "")
                            {
                                AlertMessage("Enter Deposit Slip Number", "Red");
                                return;
                            }
                        }
                    }
                    string error = GenerateCashPaymentVoucher(clvar, productBreakDown);

                    if (error.Contains("ERROR"))
                    {

                        Errorid.Text = "Could not Create Voucher. " + error + "";
                        Errorid.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Create Voucher.\\n" + error + "')", true);
                        return;


                    }
                    else
                    {
                        if (error == "0")
                        {
                            Errorid.Text = "Could not Create Voucher. Receipt Already Exists";
                            Errorid.ForeColor = System.Drawing.Color.Red;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Create Voucher.\\nReceipt Already Exists')", true);
                            return;
                        }
                        if (error == "2")
                        {
                            Errorid.Text = "Could not Create Voucher. CN Does not Exist";
                            Errorid.ForeColor = System.Drawing.Color.Red;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Create Voucher.\\nCN Does not Exist')", true);
                            return;
                        }
                        Errorid.Text = "Voucher Saved. Voucher No: " + error + "";
                        Errorid.ForeColor = System.Drawing.Color.Green;

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher Saved.\\nVoucher No: " + error + "')", true);
                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                        string script = String.Format(script_, "Receipt_Print.aspx?ID=" + error + "&RCode=", "_blank", "");
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                        ResetAll();

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {

                Errorid.Text = ex.Message;
                Errorid.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void ResetAll()
        {
            txt_amount.Text = "";
            txt_chequeNo.Text = "";
            txt_clientAcNo.Text = "";
            txt_clientName.Text = "";
            txt_cnNo.Text = "";
            txt_receiptNo.Text = "";
            txt_refNo.Text = "";
            txt_saleAmt.Text = "";
            txt_voucherNo.Text = "";
            //picker_chequeDate.Clear();
            picker_voucherDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dd_banks.ClearSelection();
            dd_EC.ClearSelection();
            dd_group.ClearSelection();
            dd_paySource.ClearSelection();
            //dd_rider.ClearSelection();
            txt_rider.Text = "";
            dd_type.ClearSelection();
            Products();
        }
        protected void txt_clientAcNo_TextChanged(object sender, EventArgs e)
        {
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            clvar.AccountNo = txt_clientAcNo.Text;
            DataTable dt = GetAccountDetailByAccountNumber(clvar);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["centralizedClient"].ToString() == "1" || dt.Rows[0]["centralizedClient"].ToString().ToUpper() == "TRUE")
                {
                    AlertMessage("This is a Centralized/Nationwide account.", "Red");

                    creditClientID.Value = "";
                    txt_clientName.Text = "";
                    txt_clientAcNo.Text = "";

                    chk_centralized.Checked = true;
                    dd_group.Enabled = true;
                    txt_clientGroup.Enabled = true;
                    txt_clientAcNo.Enabled = false;

                    if (dd_group.Items.FindByValue(dt.Rows[0]["clientGrpId"].ToString()) != null)
                    {
                        dd_group.SelectedValue = dt.Rows[0]["clientGrpId"].ToString();
                    }
                    return;
                }
                creditClientID.Value = dt.Rows[0]["ID"].ToString();
                txt_clientName.Text = dt.Rows[0]["Name"].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account No')", true);
                Errorid.Text = "Invalid Account No";
                Errorid.ForeColor = System.Drawing.Color.Red;
                txt_clientAcNo.Text = "";
                return;
            }
        }
        public DataTable GetAccountDetailByAccountNumber(Cl_Variables clvar)
        {

            string query = "SELECT  z.name ZoneName, c.* FROM CREDITCLIENTS c inner join Zones z on z.zoneCode = c.zoneCode where c.ACCOUNTNO = '" + clvar.AccountNo + "' and c.branchcode = '" + clvar.Branch + "' --and c.isactive = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        protected void chk_centralized_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_centralized.Checked == true)
            {
                txt_clientAcNo.Enabled = false;
                dd_group.Enabled = true;
                txt_clientGroup.Enabled = true;
            }
            else
            {
                txt_clientAcNo.Enabled = true;
                dd_group.Enabled = false;
                txt_clientGroup.Enabled = false;
            }
        }
        protected void dd_EC_SelectedIndexChanged(object sender, EventArgs e)
        {
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            clvar.expresscenter = dd_EC.SelectedValue;
            //dd_rider.Items.Clear();
            //dd_rider.Items.Add(new ListItem("Select Rider", "0"));
            //DataTable dt = GetRidersByExpressCenter(clvar);

            //if (dt.Rows.Count > 0)
            //{

            //    dd_rider.DataSource = dt;
            //    dd_rider.DataTextField = "NAME";
            //    dd_rider.DataValueField = "RiderCode";
            //    dd_rider.DataBind();
            //}
            //else
            //{
            //    Errorid.Text = "No Riders Found";
            //    Errorid.ForeColor = System.Drawing.Color.Red;
            //}
        }
        protected void txt_voucherNo_TextChanged(object sender, EventArgs e)
        {
            clvar.VoucherNo = txt_voucherNo.Text;
            DataSet ds = enc_.Get_ReceiptReport(clvar);

            if (ds.Tables[0].Rows.Count != 0)
            {
                txt_receiptNo.Text = ds.Tables[0].Rows[0]["ReceiptNo"].ToString();
                picker_voucherDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["VoucherDate"].ToString()).ToString("yyyy-MM-dd");
                txt_clientAcNo.Text = ds.Tables[0].Rows[0]["Accountno"].ToString();
                txt_clientName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                if (ds.Tables[0].Rows[0]["PaymentMode"].ToString() == "Cash")
                {
                    rbtn_paymentMode.SelectedValue = "1";
                }
                else
                {
                    rbtn_paymentMode.SelectedValue = "2";
                }
                dd_paySource.SelectedItem.Text = ds.Tables[0].Rows[0]["PaymentSource"].ToString();
                dd_type.SelectedItem.Text = ds.Tables[0].Rows[0]["PaymentType"].ToString();

                txt_cnNo.Text = ds.Tables[0].Rows[0]["ConsignmentNo"].ToString();
                txt_chequeNo.Text = ds.Tables[0].Rows[0]["ChequeNo"].ToString();
                if (ds.Tables[0].Rows[0]["ChequeDate"].ToString() != "")
                {
                    picker_chequeDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["ChequeDate"].ToString()).ToString("yyyy-MM-dd");// pv.ChequeDate
                }
                if (ds.Tables[0].Rows[0]["ExpressCenterCode"].ToString().Length.ToString() != "0")
                {
                    dd_EC.SelectedValue = ds.Tables[0].Rows[0]["ExpressCenterCode"].ToString();
                }
                dd_EC_SelectedIndexChanged(sender, e);
                if (ds.Tables[0].Rows[0]["RiderCode"].ToString().Length.ToString() != "0")
                {
                    txt_rider.Text = ds.Tables[0].Rows[0]["RiderCode"].ToString();// dd_rider.SelectedValue = ds.Tables[0].Rows[0]["RiderCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BankId"].ToString().Length.ToString() != "0")
                {
                    dd_banks.SelectedValue = ds.Tables[0].Rows[0]["BankId"].ToString();
                }


                txt_amount.Text = string.Format("{0:N0}", Double.Parse(ds.Tables[0].Rows[0]["Amount"].ToString()));


                DataTable dt_ = GetProductBreakDown(clvar.VoucherNo);
                if (dt_.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gv_productWiseAmount.Rows)
                    {
                        string product = row.Cells[0].Text;
                        TextBox txt = row.FindControl("txt_amount") as TextBox;
                        DataRow dr = dt_.Select("product = '" + product + "'").FirstOrDefault();
                        if (dr != null)
                        {
                            txt.Text = dr["Amount"].ToString();
                        }

                    }
                }

            }
        }
        protected void bt_ReportView_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "Receipt_Print.aspx?ID=" + txt_voucherNo.Text + "&RCode=", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

        }
        //protected void txt_cnNo_TextChanged(object sender, EventArgs e)
        //{
        //    clvar.consignmentNo = txt_cnNo.Text;
        //    DataTable dt = GetConsignmentDetail(clvar);
        //    if (dt != null)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0]["PRESENT"].ToString().Trim(' ') != "")
        //            {
        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher No.# " + dt.Rows[0]["VoucherID"].ToString() + " Already Created for CN Number " + clvar.consignmentNo + "')", true);
        //                txt_cnNo.Text = "";
        //                txt_cnNo.Focus();
        //                return;
        //            }
        //            txt_clientName.Text = dt.Rows[0]["name"].ToString();
        //            txt_clientAcNo.Text = dt.Rows[0]["consignerAccountNo"].ToString();
        //            creditClientID.Value = dt.Rows[0]["creditClientID"].ToString();
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
        //            txt_cnNo.Text = "";
        //            txt_cnNo.Focus();
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
        //        txt_cnNo.Text = "";
        //        txt_cnNo.Focus();
        //        return;
        //    }
        //}
        protected void txt_cnNo_TextChanged(object sender, EventArgs e)
        {
            clvar.consignmentNo = txt_cnNo.Text;
            DataTable dt = GetConsignmentDetail(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["PRESENT"].ToString().Trim(' ') != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher No.# " + dt.Rows[0]["VoucherID"].ToString() + " Already Created for CN Number " + clvar.consignmentNo + "')", true);
                        txt_cnNo.Text = "";
                        txt_cnNo.Focus();
                        txt_clientName.Text = "";
                        txt_clientAcNo.Text = "";
                        creditClientID.Value = "";
                        txt_amount.Text = "";
                        txt_saleAmt.Text = "";
                        return;
                    }
                    txt_clientName.Text = dt.Rows[0]["name"].ToString();
                    txt_clientAcNo.Text = dt.Rows[0]["consignerAccountNo"].ToString();
                    creditClientID.Value = dt.Rows[0]["creditClientID"].ToString();
                    //txt_amount.Text = dt.Rows[0]["TotalAmount"].ToString();
                    txt_saleAmt.Text = dt.Rows[0]["CODAmount"].ToString();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
                    txt_clientName.Text = "";
                    txt_clientAcNo.Text = "";
                    creditClientID.Value = "";
                    txt_amount.Text = "";
                    txt_cnNo.Text = "";
                    txt_cnNo.Focus();
                    txt_saleAmt.Text = "";
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Not Found')", true);
                txt_clientName.Text = "";
                txt_clientAcNo.Text = "";
                creditClientID.Value = "";
                txt_amount.Text = "";
                txt_cnNo.Text = "";
                txt_saleAmt.Text = "";
                txt_cnNo.Focus();
                return;
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "Receipt_Print.aspx?ID=" + txt_voucherNo.Text + "&RCode=", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }


        public DataTable GetConsignmentDetail(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sqlString = "select c.consignmentNumber, cc.accountNo, c.consignerAccountNo, cc.name, c.creditClientId, cc.id\n" +
            "  from consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on cc.accountNo = c.consignerAccountNo\n" +
            "   and cc.branchCode = c.branchCode\n" +
            " where \n" +
            "   c.consignmentNumber = '" + clvar.consignmentNo + "'";


            sqlString = "select c.consignmentNumber,\n" +
            "       cc.accountNo,\n" +
            "       c.consignerAccountNo,\n" +
            "       cc.name,\n" +
            "       c.creditClientId,\n" +
            "       cc.id,\n" +
            "       p.ConsignmentNo Present, p.id VoucherID, c.totalAmount\n" +
            "  from consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on cc.id = c.creditclientID\n" +
            "    --on cc.accountNo = c.consignerAccountNo\n" +
            "   --and cc.branchCode = c.branchCode\n" +
            "  left outer join PaymentVouchers p\n" +
            "    on p.ConsignmentNo = c.consignmentNumber\n" +
            "   and p.CreditClientId = c.creditClientId\n" +
            " where c.consignmentNumber = '" + clvar.consignmentNo + "'";


            sqlString = "\n" +
           "SELECT c.consignmentNumber,\n" +
           "       cc.accountNo,\n" +
           "       c.consignerAccountNo,\n" +
           "       cc.name,\n" +
           "       c.creditClientId,\n" +
           "       cc.id,\n" +
           "       p.ConsignmentNo Present,\n" +
           "       p.id VoucherID,\n" +
           "       c.totalAmount,\n" +
           "       CASE\n" +
           "         WHEN COUNT(c2.Accountno) > 0 THEN\n" +
           "          (SELECT SUM(distinct cdn.codAmount)\n" +
           "             FROM CODConsignmentDetail_New cdn\n" +
           "            WHERE cdn.consignmentNumber = c.consignmentNumber)\n" +
           "         ELSE\n" +
           "          (SELECT SUM(cd.codAmount)\n" +
           "             FROM CODConsignmentDetail cd\n" +
           "            WHERE cd.consignmentNumber = c.consignmentNumber)\n" +
           "       END CODAMOUNT\n" +
           "  FROM consignment c\n" +
           " INNER JOIN CreditClients cc\n" +
           "    ON cc.id = c.creditclientID\n" +
           "--on cc.accountNo = c.consignerAccountNo\n" +
           "--and cc.branchCode = c.branchCode\n" +
           "\n" +
           "  LEFT OUTER JOIN PaymentVouchers p\n" +
           "    ON p.ConsignmentNo = c.consignmentNumber\n" +
           "   AND p.CreditClientId = c.creditClientId\n" +
           "  LEFT OUTER JOIN CODUsers c2\n" +
           "    ON c2.CreditClientID = c.creditClientId\n" +
           "   AND c2.Accountno = c.consignerAccountNo\n" +
           "   AND c2.IsCOD = '1'\n" +
           " WHERE c.consignmentnumber = '" + clvar.consignmentNo + "'\n" +
           " GROUP BY c.consignmentNumber,\n" +
           "          cc.accountNo,\n" +
           "          c.consignerAccountNo,\n" +
           "          cc.name,\n" +
           "          c.creditClientId,\n" +
           "          cc.id,\n" +
           "          p.ConsignmentNo,\n" +
           "          p.id,\n" +
           "          c.totalAmount";

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

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '" + clvar.Branch + "' and doc_type = 'R'";
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

        public DataTable GetRidersByExpressCenter(Cl_Variables clvar)
        {
            string sqlString = "select  r.riderCode + ' - ' + r.firstName+' '+r.lastName NAME, r.riderCode from Riders r where r.expressCenterId = '" + clvar.expresscenter + "' and r.branchId = '" + clvar.Branch + "' and r.status = '1'  order by 1";
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




        public DataSet ExpressCenterOrigin(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {

                string query = "SELECT ec.ExpressCenterCode + ' - ' + ec.name ECName, ec.* FROM ExpressCenters ec where bid='" + clvar.origin + "' and status='1' order by NAME";


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

        public DataTable GetProducts()
        {
            string query = " select distinct Products\n" +
                           "   from ServiceTypes_New st \n" +
                           "  where (st.Products not like 'AVIA%')\n" +
                           "    and st.status = '1'";


            query = "SELECT DISTINCT Products,\n" +
            "                CASE\n" +
            "                  WHEN st.companyId = '1' THEN\n" +
            "                   'MPEX ' + st.Products\n" +
            "                  WHEN st.companyId = '2' THEN\n" +
            "                   'OCS ' + st.Products\n" +
            "                END ProductDisplay\n" +
            "  FROM ServiceTypes_New st\n" +
            " WHERE (st.Products NOT LIKE 'AVIA%')\n" +
            "   AND st.status = '1'\n" +
            "   AND st.companyId IN (1, 2)";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }



        public string GeneratePaymentVoucher(Cl_Variables clvar, DataTable dt)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            string id = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("MnP_CreatePaymentVoucher_new", con);
                cmd.CommandType = CommandType.StoredProcedure;

                IDbDataParameter ID = cmd.CreateParameter();
                ID.ParameterName = "@ID";
                ID.Direction = System.Data.ParameterDirection.Output;
                ID.DbType = System.Data.DbType.String;
                ID.Size = 50;
                cmd.Parameters.Add(ID);

                cmd.Parameters.AddWithValue("@Chk_Centralized", clvar.IsCentralized);
                cmd.Parameters.AddWithValue("@Chk_PaySource", clvar.PaymentSource);
                cmd.Parameters.AddWithValue("@ReceiptNo", clvar.ReceiptNo);
                cmd.Parameters.AddWithValue("@RefNo", clvar.RefNo);
                cmd.Parameters.AddWithValue("@VoucherDate", clvar.VoucherDate);
                cmd.Parameters.AddWithValue("@isCentralized", clvar.IsCentralized);
                cmd.Parameters.AddWithValue("@ClientGroupID", clvar.ClientGroupID);
                cmd.Parameters.AddWithValue("@CreditClientID", clvar.CreditClientID);
                cmd.Parameters.AddWithValue("@isByCreditClient", clvar.IsByCreditClientID);
                cmd.Parameters.AddWithValue("@PaymentSourceID", clvar.PaymentSource);
                cmd.Parameters.AddWithValue("@ChequeNo", clvar.ChequeNo);
                cmd.Parameters.AddWithValue("@ChequeDate", clvar.ChequeDate);
                cmd.Parameters.AddWithValue("@Amount", float.Parse(clvar.amount.ToString()));
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@PaymentTypeID", clvar.PaymentType);
                cmd.Parameters.AddWithValue("@CashPaymentSource", "4");
                cmd.Parameters.AddWithValue("@BankID", clvar.Bank);
                cmd.Parameters.AddWithValue("@AmountUsed", 0);
                cmd.Parameters.AddWithValue("@breakDown", dt);
                if (clvar.PaymentSource == "2")
                {
                    cmd.Parameters.AddWithValue("@DslipNo", txt_dslipNo.Text);
                    cmd.Parameters.AddWithValue("@DslipBank", dd_depositSlipBank.SelectedValue);
                }
                if (dd_type.SelectedValue == "4")
                {

                    cmd.Parameters.AddWithValue("@ECCode", clvar.expresscenter);
                }
                if (clvar.PaymentType == "1")
                {
                    cmd.Parameters.AddWithValue("@CPRNumber", txt_cprNo.Text);
                }
                else if (clvar.PaymentType == "10")
                {
                    cmd.Parameters.AddWithValue("@STWNumber", txt_stwNo.Text);
                }
                //cmd.Parameters.AddWithValue("@breakDown", dt);

                cmd.ExecuteNonQuery();
                id = ID.Value.ToString();
            }
            catch (Exception ec)
            {
                con.Close();
                return "ERROR:" + ec.Message;
            }
            finally { con.Close(); }
            return id;
        }
        public string GenerateCashPaymentVoucher(Cl_Variables clvar, DataTable dt)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            string id = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("MnP_CreateCashPaymentVoucher_new", con);
                cmd.CommandType = CommandType.StoredProcedure;

                IDbDataParameter ID = cmd.CreateParameter();
                ID.ParameterName = "@ID";
                ID.Direction = System.Data.ParameterDirection.Output;
                ID.DbType = System.Data.DbType.String;
                ID.Size = 50;
                cmd.Parameters.Add(ID);

                if (clvar.CheckCondition == "1")
                {
                    cmd.Parameters.AddWithValue("@RiderCode", DBNull.Value);
                    cmd.Parameters.AddWithValue("@creditClientID", DBNull.Value);

                    cmd.Parameters.AddWithValue("@ConsignmentNumber", DBNull.Value);
                }
                else if (clvar.CheckCondition == "2")
                {
                    cmd.Parameters.AddWithValue("@RiderCode", clvar.riderCode);
                    cmd.Parameters.AddWithValue("@creditClientID", DBNull.Value);

                    cmd.Parameters.AddWithValue("@ConsignmentNumber", DBNull.Value);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@RiderCode", clvar.riderCode);
                    cmd.Parameters.AddWithValue("@creditClientID", clvar.CreditClientID);

                    cmd.Parameters.AddWithValue("@ConsignmentNumber", clvar.consignmentNo);
                }
                if ((dd_type.SelectedValue == "15" || dd_type.SelectedValue == "11") && dd_paySource.SelectedItem.Text.ToUpper() != "CASH")
                {
                    cmd.Parameters.AddWithValue("@PaymentSourceID", dd_paySource.SelectedValue);
                    cmd.Parameters.AddWithValue("@BankID", dd_banks.SelectedValue);
                    cmd.Parameters.AddWithValue("@ChequeDate", picker_chequeDate.Text);
                    cmd.Parameters.AddWithValue("@ChequeNumber", txt_chequeNo.Text);
                }
                else if (dd_paySource.SelectedValue == "1" || dd_paySource.SelectedValue == "6" || dd_paySource.SelectedValue == "7")
                {
                    cmd.Parameters.AddWithValue("@PaymentSourceID", dd_paySource.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PaymentSourceID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@BankID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ChequeDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ChequeNumber", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@ReceiptNo", clvar.ReceiptNo);
                cmd.Parameters.AddWithValue("@RefNo", clvar.RefNo);
                cmd.Parameters.AddWithValue("@VoucherDate", clvar.VoucherDate);
                cmd.Parameters.AddWithValue("@Amount", float.Parse(clvar.amount.ToString()));
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@PaymentTypeID", clvar.PaymentType);
                cmd.Parameters.AddWithValue("@CashPaymentSource", clvar.CheckCondition);

                cmd.Parameters.AddWithValue("@ExpressCenterCode", clvar.expresscenter);
                cmd.Parameters.AddWithValue("@breakDown", dt);
                if (clvar.PaymentType == "1")
                {
                    cmd.Parameters.AddWithValue("@CPRNumber", txt_cprNo.Text);
                }
                else if (clvar.PaymentType == "10")
                {
                    cmd.Parameters.AddWithValue("@STWNumber", txt_stwNo.Text);
                }

                cmd.ExecuteNonQuery();
                id = ID.Value.ToString();
            }
            catch (Exception ec)
            {
                con.Close();
                return "ERROR:" + ec.Message;
            }
            finally { con.Close(); }
            return id;
        }

        public DataTable GetProductBreakDown(string voucherid)
        {
            string query = "select * from MnP_PaymentVouchersProductBreakDown where voucherid = '" + voucherid + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception)
            {


            }
            finally { con.Close(); }
            return dt;
        }

        public bool IsValidCPR(string cprNumber)
        {

            string query = "SELECT * FROM PaymentVouchers pv WHERE pv.CPRNumber = 'IT" + cprNumber + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }



            return false;
        }
        protected void txt_rider_TextChanged(object sender, EventArgs e)
        {
            if (txt_rider.Text.Trim() == "")
            {
                AlertMessage("Enter Rider Code", "Red");
                return;
            }

            string riderCode = txt_rider.Text.Trim();
            DataTable Riders = ValidateRider(riderCode);

            if (Riders != null)
            {
                if (Riders.Rows.Count > 0)
                {

                }
                else
                {
                    AlertMessage("Invalid Rider Code", "Red");
                    txt_rider.Text = "";
                    return;
                }
            }
            else
            {
                AlertMessage("Invalid Rider Code", "Red");
                txt_rider.Text = "";
                return;
            }

        }

        public DataTable ValidateRider(string riderCode)
        {
            DataTable dt = new DataTable();
            string sqlString = "SelecT * from riders where riderCode = '" + riderCode + "' and BranchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and status = '1'";

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
        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
        }

        public void ValidateClientGroup(object sender, EventArgs e)
        {
            string grpID = txt_clientGroup.Text;
            dd_group.ClearSelection();

            bool grpFound = false;
            foreach (ListItem item in dd_group.Items)
            {
                if (item.Value == grpID)
                {
                    item.Selected = true;
                    grpFound = true;
                    break;
                }
            }

            if (!grpFound)
            {
                txt_clientGroup.Text = "";
                AlertMessage("Invalid Group ID", "Red");
                return;
            }
        }

        protected void dd_paySource_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (dd_type.SelectedValue == "1")
            {
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                txt_chequeNo.Enabled = false;
            }
            else if (dd_type.SelectedValue == "10")
            {
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                txt_chequeNo.Enabled = false;
            }
            else if (dd_paySource.SelectedValue == "1" || dd_paySource.SelectedValue == "6" || dd_paySource.SelectedValue == "7")
            {
                dd_banks.Enabled = false;
                picker_chequeDate.Enabled = false;
                txt_chequeNo.Enabled = false;
            }
            else
            {
                dd_banks.Enabled = true;
                picker_chequeDate.Enabled = true;
                txt_chequeNo.Enabled = true;
            }

        }

        public void Post_BrandedSMS(string date, string amount, string ClientId,string AccNo, string pv)
        {
            try
            {
                string dateformat = DateTime.Parse(date).ToString("dddd, dd MMMM yyyy");

                string smsContent = "Dear Customer, Payment of Amount PKR " + amount + " has been received against account number " + AccNo + " on " + dateformat + " should you have any query, please contact your local Recovery representative or call us 021-38653210 ";

                string query2 = @"insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber,smsformtype)
                                  SELECT DISTINCT
                                  'PV-" + pv + @"', cc.phoneNo, '" + smsContent + @"', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + @"', 'N/A','3'
                                  FROM CreditClients cc 
                                  WHERE ISNULL(cc.isActive,'0') = '1' and cc.iscod = 0
                                  " + ClientId + @"
                                  AND cc.branchCode = '" + HttpContext.Current.Session["BRANCHCODE"].ToString() + @"'";

                int count = 0;
                string error = "";
                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                try
                {
                    sqlcon.Open();
                    SqlCommand sqlcmd = new SqlCommand(query2, sqlcon);
                    sqlcmd.CommandType = CommandType.Text;

                    count = sqlcmd.ExecuteNonQuery();
                    sqlcon.Close();
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

            }
            catch (Exception Err)
            {
                //listBox1.Items.Add("Failed " + Err + "Mobile Number " + clvar.MobileNumber + " and Response " + resp);
            }
        }
    }
}