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
    public partial class ReceiptVoucherAdj : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        CommonFunction func = new CommonFunction();
        Cl_Receipts rec = new Cl_Receipts();

        protected void Page_Load(object sender, EventArgs e)
        {
            Errorid.Text = "";
            if (!IsPostBack)
            {
                Products();
            }
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
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_adjAmount.Text = "";
            txt_bank.Text = "";
            txt_chequeDate.Text = "";
            txt_chequeNo.Text = "";
            txt_clientName.Text = "";
            txt_expressCenter.Text = "";
            txt_paymentMode.Text = "";
            txt_paySource.Text = "";
            txt_rider.Text = "";
            txt_usedAmount.Text = "";
            txt_VoucherAmount.Text = "";
            txt_voucherNo.Text = "";
            Products();
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (hd_paid.Value == "1")
            {
                AlertMessage("This Voucher has already been paid.", "Red");
                return;
            }
            if (hd_closed.Value == "1")
            {
                AlertMessage("This Voucher is Closed", "Red");
                return;
            }
            if (txt_adjAmount.Text == txt_VoucherAmount.Text)
            {
                //Errorid.Text = "Cannot Adjust. Amounts are Same";
                AlertMessage("Cannot Adjust. Amounts are Same", "Red");
                return;
            }
            if (hd_paymentType.Value == "5")
            {
                if (txt_VoucherAmount.Text == "0")
                {
                    AlertMessage("Cannot Adjust. COD Amount Already 0", "Red");
                    return;
                }
            }

            clvar.TotalAmount = double.Parse(txt_VoucherAmount.Text);
            clvar.VoucherNo = txt_voucherNo.Text;
            float adjAmount = 0;
            float.TryParse(txt_adjAmount.Text, out adjAmount);
            float vAmount = 0;
            float.TryParse(txt_VoucherAmount.Text, out vAmount);
            float uAmount = 0;
            float.TryParse(txt_usedAmount.Text, out uAmount);
            if (adjAmount + vAmount < uAmount)
            {
                Errorid.Text = "Invalid Amount";
                return;
            }

            if (hd_paymentType.Value == "5")
            {
                if (adjAmount + vAmount != 0)
                {
                    AlertMessage("Adjustment Not allowed. COD Payment can only be adjusted to 0 Amount.", "Red");
                    return;
                }
            }


            clvar.CheckCondition = txt_adjAmount.Text;
            clvar.PaymentType = hd_paymentType.Value;
            string error = InsertReceiptAdjustment(clvar);
            if (error == "Success")
            {
                AlertMessage("Voucher Adjusted.", "Green");
            }
            else
            {
                AlertMessage(error, "Red");
                return;
            }
            string voucherNo = txt_voucherNo.Text;
            btn_reset_Click(sender, e);
            txt_voucherNo.Text = voucherNo;
            txt_voucherNo_TextChanged(this, e);

        }
        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);

        }
        protected void rbtn_formMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtn_formMode.SelectedValue == "1")
            {
                btn_save.Enabled = true;
                txt_adjAmount.Enabled = true;
            }
            else
            {
                btn_save.Enabled = false;
                txt_adjAmount.Enabled = false;
            }
        }
        protected void txt_voucherNo_TextChanged(object sender, EventArgs e)
        {
            clvar.VoucherNo = txt_voucherNo.Text;
            if (rbtn_formMode.SelectedValue == "1")
            {
                clvar.VoucherNo = txt_voucherNo.Text;
                DataSet ds = GetReceiptVoucherHeader(clvar);
                DataTable dt = ds.Tables["VoucherDetails"];
                DataTable breakDown = ds.Tables["ProductBreakDown"];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {

                        txt_bank.Text = dt.Rows[0]["BankName"].ToString();
                        txt_chequeDate.Text = dt.Rows[0]["ChequeDate"].ToString();
                        txt_chequeNo.Text = dt.Rows[0]["ChequeNo"].ToString();
                        txt_clientName.Text = dt.Rows[0]["client_NAME"].ToString();
                        txt_expressCenter.Text = dt.Rows[0]["NAME"].ToString();
                        txt_paymentMode.Text = dt.Rows[0]["paymentMode"].ToString();
                        txt_paySource.Text = dt.Rows[0]["PaySource"].ToString();
                        txt_rider.Text = dt.Rows[0]["RiderName"].ToString();
                        txt_usedAmount.Text = dt.Rows[0]["AmountUsed"].ToString();
                        txt_VoucherAmount.Text = dt.Rows[0]["Amount"].ToString();
                        hd_paymentType.Value = dt.Rows[0]["PaymentTypeID"].ToString();
                        hd_paysource.Value = dt.Rows[0]["PaymentSourceId"].ToString();
                        hd_VoucherDate.Value = dt.Rows[0]["VoucherDate"].ToString();
                        if (breakDown.Rows.Count > 0 && hd_paymentType.Value == "4")
                        {
                            foreach (GridViewRow row in gv_productWiseAmount.Rows)
                            {
                                string product = row.Cells[0].Text;
                                TextBox txt = row.FindControl("txt_amount") as TextBox;
                                DataRow dr = breakDown.Select("product = '" + product + "'").FirstOrDefault();
                                if (dr != null)
                                {
                                    txt.Text = dr["Amount"].ToString();
                                }
                            }
                        }

                        if (dt.Rows[0]["CLOSED"].ToString() == "YES")
                        {
                            hd_closed.Value = "1";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Voucher has been Closed.')", true);
                            Errorid.Text = "This Voucher has been Closed.";
                            btn_save.Enabled = false;
                            Errorid.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            hd_closed.Value = "0";
                        }
                        if (dt.Rows[0]["PAID"].ToString() == "YES")
                        {
                            hd_paid.Value = "1";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This Voucher has been Paid.')", true);
                            Errorid.Text = "This Voucher has been Paid.";
                            btn_save.Enabled = false;
                            Errorid.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            hd_paid.Value = "0";
                        }
                        btn_save.Enabled = true;

                    }
                }
                else
                {
                    Errorid.Text = "Voucher Not Found";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher Not Found')", true);
                    return;
                }
            }
            else
            {
                clvar.VoucherNo = txt_voucherNo.Text;
                DataTable dt = GetReceiptVoucherHeader(clvar).Tables[0];

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        txt_bank.Text = dt.Rows[0]["BankName"].ToString();
                        txt_chequeDate.Text = dt.Rows[0]["ChequeDate"].ToString();
                        txt_chequeNo.Text = dt.Rows[0]["ChequeNo"].ToString();
                        txt_clientName.Text = dt.Rows[0]["client_NAME"].ToString();
                        txt_expressCenter.Text = dt.Rows[0]["NAME"].ToString();
                        txt_paymentMode.Text = dt.Rows[0]["paymentMode"].ToString();
                        txt_paySource.Text = dt.Rows[0]["PaySource"].ToString();
                        txt_rider.Text = dt.Rows[0]["RiderName"].ToString();
                        txt_usedAmount.Text = dt.Rows[0]["AmountUsed"].ToString();
                        txt_VoucherAmount.Text = dt.Rows[0]["Amount"].ToString();
                        hd_paymentType.Value = dt.Rows[0]["PaymentTypeID"].ToString();
                        hd_paysource.Value = dt.Rows[0]["PaymentSourceId"].ToString();
                        hd_VoucherDate.Value = dt.Rows[0]["VoucherDate"].ToString();
                    }
                }
                else
                {
                    Errorid.Text = "Voucher Not Found";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher Not Found')", true);
                    return;
                }
            }
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
        public string InsertReceiptAdjustment(Cl_Variables clvar)
        {
            string query = "Update paymentVouchers set amount = amount + " + clvar.CheckCondition + " where id = '" + clvar.VoucherNo + "' and amount + " + clvar.CheckCondition + " >= AmountUsed";
            string query_ = "insert into PaymentVoucherAdj (PaymentVoucherID, OldAmount, NewAmount, Createdon, CreatedBy) \n" +
                           "VALUES ( \n" +
                           " '" + clvar.VoucherNo + "', '" + clvar.TotalAmount + "', '" + (clvar.TotalAmount + double.Parse(clvar.CheckCondition)).ToString() + "',\n" +
                           " GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                           ")";

            DateTime voucherDate = DateTime.Parse(hd_VoucherDate.Value);
            string CIH_Remaining = "update cih_remainings set value = value + " + clvar.CheckCondition + " where company = '1' and branch = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and year = YEAR('" + voucherDate.ToString("yyyy-MM-dd") + "') and month = MONTH('" + voucherDate.ToString("yyyy-MM-dd") + "')";

            string breakDownCommand = "";
            string breakDownDeleteCommand = "";
            if (clvar.PaymentType == "4")
            {
                breakDownDeleteCommand = "delete from MnP_PaymentVouchersProductBreakDown where voucherID = '" + clvar.VoucherNo + "'";
                breakDownCommand = "insert into MnP_PaymentVouchersProductBreakDown (voucherID, amount, Product) VALUES \n";
                double totalAmount = 0;

                string temp = "";
                foreach (GridViewRow row in gv_productWiseAmount.Rows)
                {
                    string product = "";
                    double amount = 0;

                    product = (row.FindControl("hd_product") as HiddenField).Value;
                    double.TryParse((row.FindControl("txt_amount") as TextBox).Text, out amount);

                    if (amount >= 0 && (row.FindControl("txt_amount") as TextBox).Text.Trim() != "")
                    {
                        totalAmount = totalAmount + amount;
                        temp += "('" + clvar.VoucherNo + "', '" + amount + "', '" + product + "'),";
                    }
                }
                temp = temp.TrimEnd(',');
                if (clvar.TotalAmount + double.Parse(clvar.CheckCondition) != totalAmount)
                {
                    return "Error: Product Breakdown and amounts Do not Match";
                }
                breakDownCommand = breakDownCommand + temp;
            }
            string CODpayment = "";
            double balAmount = clvar.TotalAmount + (double.Parse(txt_adjAmount.Text));
            if (clvar.PaymentType == "5" && balAmount != 0)
            {
                AlertMessage("Adjustment Not allowed. COD Payment can only be adjusted to 0 Amount.", "Red");
                return "Adjustment Not allowed. COD Payment can only be adjusted to 0 Amount.";
            }
            if (clvar.PaymentType == "5" && balAmount <= 0)
            {
                CODpayment = "Update paymentVouchers set ConsignmentNo ='', DepositSlipNo = ''  where id = '" + clvar.VoucherNo + "'";

            }

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;

            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {


                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = query_;
                sqlcmd.ExecuteNonQuery();
                if (clvar.PaymentType == "4")
                {
                    sqlcmd.CommandText = breakDownDeleteCommand;
                    sqlcmd.ExecuteNonQuery();

                    sqlcmd.CommandText = breakDownCommand;
                    sqlcmd.ExecuteNonQuery();
                }
                if (clvar.PaymentType == "5" && balAmount <= 0)
                {
                    sqlcmd.CommandText = CODpayment;
                    sqlcmd.ExecuteNonQuery();
                }

                if (hd_paysource.Value.ToUpper() == "1" || hd_paysource.Value.ToUpper() == "TRUE")
                {
                    sqlcmd.CommandText = CIH_Remaining;
                    sqlcmd.ExecuteNonQuery();
                }



                trans.Commit();
                //trans.Rollback();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                sqlcon.Close();

                return "Error:" + ex.Message;

            }
            finally { sqlcon.Close(); }
            return "Success";

            //sqlcmd.CommandTimeout = 300;

        }

        public DataSet GetReceiptVoucherHeader(Cl_Variables clvar)
        {
            string query = "select p.Id,\n" +
            "       case\n" +
            "         when p.CashPaymentSource is null then\n" +
            "          'CREDIT'\n" +
            "         else\n" +
            "          'CASH'\n" +
            "       End PaymentMode,\n" +
            "       p.ExpressCenterCode,\n" +
            "       ec.name,\n" +
            "       p.RiderCode,\n" +
            "       r.firstName + ' ' + r.lastName RiderName,\n" +
            "       p.PaymentSourceId,\n" +
            "       Case\n" +
            "         When p.ChequeNo is null then\n" +
            "          'CASH'\n" +
            "         else\n" +
            "          ps.Name\n" +
            "       End PaySource,\n" +
            "       Case\n" +
            "         When p.IsCentralized = '1' then\n" +
            "          cg.name\n" +
            "         else\n" +
            "          cc.name\n" +
            "       End Client_Name,\n" +
            "       p.BankId,\n" +
            "       b.Name BankName,\n" +
            "       p.ChequeNo,\n" +
            "       p.ChequeDate,\n" +
            "       p.Amount,\n" +
            "       p.AmountUsed, PaymentTypeId\n" +
            "  from PaymentVouchers p\n" +
            "  left outer join PaymentSource ps\n" +
            "    on ps.Id = p.PaymentSourceId\n" +
            "  left outer join CreditClients cc\n" +
            "    on p.CreditClientId = cc.id\n" +
            "  left outer join ClientGroups cg\n" +
            "    on p.ClientGroupId = cg.id\n" +
            "  left outer join Banks b\n" +
            "    on p.BankId = b.Id\n" +
            "  left outer join ExpressCenters ec\n" +
            "    on p.ExpressCenterCode = ec.expressCenterCode\n" +
            "  left outer join Riders r\n" +
            "    on r.riderCode = p.RiderCode\n" +
            "   and p.BranchCode = r.branchId\n" +
            "   and p.ExpressCenterCode = r.expressCenterId\n" +
            " where p.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and p.Id = '" + clvar.VoucherNo + "'";


            string sqlString = "SELECT p.Id,\n" +
            "       CASE\n" +
            "            WHEN p.CashPaymentSource IS NULL THEN 'CREDIT'\n" +
            "            ELSE 'CASH'\n" +
            "       END                                PaymentMode,\n" +
            "       p.ExpressCenterCode,\n" +
            "       ec.name,\n" +
            "       p.RiderCode,\n" +
            "       r.firstName + ' ' + r.lastName     RiderName,\n" +
            "       p.PaymentSourceId,\n" +
            "       CASE\n" +
            "            WHEN p.ChequeNo IS NULL THEN 'CASH'\n" +
            "            ELSE ps.Name\n" +
            "       END                                PaySource,\n" +
            "       CASE\n" +
            "            WHEN p.IsCentralized = '1' THEN cg.name\n" +
            "            ELSE cc.name\n" +
            "       END                                Client_Name,\n" +
            "       p.BankId,\n" +
            "       b.Name                             BankName,\n" +
            "       p.ChequeNo,\n" +
            "       p.ChequeDate,\n" +
            "       p.Amount,\n" +
            "       p.AmountUsed,\n" +
            "       CASE\n" +
            "            WHEN p.VoucherDate >= ma.[DateTime] THEN 'NO'\n" +
            "            ELSE 'YES'\n" +
            "       END                                Closed,\n" +
            "       CASE\n" +
            "            WHEN ISNULL(c.ispayable, 0) = '1' THEN 'YES'\n" +
            "            ELSE 'NO'\n" +
            "       END                                PAID, p.paymentTypeID, p.voucherDate\n" +
            "FROM   PaymentVouchers p\n" +
            "       LEFT OUTER JOIN Consignment c\n" +
            "            ON  c.consignmentNumber = p.ConsignmentNo\n" +
            "       LEFT OUTER JOIN PaymentSource ps\n" +
            "            ON  ps.Id = p.PaymentSourceId\n" +
            "       LEFT OUTER JOIN CreditClients cc\n" +
            "            ON  p.CreditClientId = cc.id\n" +
            "       LEFT OUTER JOIN ClientGroups cg\n" +
            "            ON  p.ClientGroupId = cg.id\n" +
            "       LEFT OUTER JOIN Banks b\n" +
            "            ON  p.BankId = b.Id\n" +
            "       LEFT OUTER JOIN ExpressCenters ec\n" +
            "            ON  p.ExpressCenterCode = ec.expressCenterCode\n" +
            "       LEFT OUTER JOIN Riders r\n" +
            "            ON  r.riderCode = p.RiderCode\n" +
            "            AND p.BranchCode = r.branchId\n" +
            "            AND p.ExpressCenterCode = r.expressCenterId\n" +
            "       LEFT OUTER JOIN Mnp_Account_DayEnd ma\n" +
            "            ON  ma.Branch = p.BranchCode\n" +
            "            AND ma.Createdon = (\n" +
            "                    SELECT MAX(mad.Createdon)\n" +
            "                    FROM   Mnp_Account_DayEnd mad\n" +
            "                    WHERE  mad.Branch = p.BranchCode\n" +
            "                           AND mad.Doc_Type = 'R'\n" +
            "                )\n" +
            " where p.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and p.Id = '" + clvar.VoucherNo + "'\n" +
            "";
            string sql = "SELECT mp.voucherID, \n"
               + "         CASE  \n"
               + "              WHEN stn.companyId = '1' THEN 'MPEX ' + stn.Products \n"
               + "              WHEN stn.companyId = '2' THEN 'OCS ' + stn.Products \n"
               + "         END Product, \n"
               + "         mp.amount \n"
               + "  FROM   MnP_PaymentVouchersProductBreakDown mp \n"
               + "         INNER JOIN ServiceTypes_New stn \n"
               + "              ON  stn.Products = mp.Product \n"
               + "  WHERE  mp.voucherID = '" + clvar.VoucherNo + "'";
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(ds, "VoucherDetails");

                sda = new SqlDataAdapter(sql, con);
                sda.Fill(ds, "ProductBreakDown");

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }
    }
}