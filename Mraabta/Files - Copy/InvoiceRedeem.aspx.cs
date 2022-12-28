using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class InvoiceRedeem : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Cl_Receipts rec = new Cl_Receipts();
        protected void Page_Load(object sender, EventArgs e)
        {
            //btn_save.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btn_save, null) + ";");
            Errorid.Text = "";
            if (!IsPostBack)
            {


            }
        }
        protected void btn_add_Click(object sender, EventArgs e)
        {

        }
        protected void txt_payVno_TextChanged(object sender, EventArgs e)
        {
            clvar.VoucherNo = txt_payVno.Text;
            DataTable dt = rec.GetReceiptVoucherHeader(clvar);

            if (dt.Rows.Count > 0)
            {
                txt_vAmt.Text = dt.Rows[0]["Amount"].ToString();
                txt_balAmt.Text = dt.Rows[0]["BalAmount"].ToString();
                double balAmount = 0;
                double.TryParse(txt_balAmt.Text, out balAmount);

                txt_chequeNo.Text = dt.Rows[0]["ChequeNo"].ToString();
                if (dt.Rows[0]["ClientName"].ToString().Trim() == "")
                {
                    txt_clientAccNo.Text = "";
                    txt_clientName.Text = "";
                    clvar.ClientGroupID = dt.Rows[0]["ClientGroupID"].ToString();
                    if (balAmount == 0 || balAmount < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher Already Redeemed')", true);
                        Errorid.Text = "Voucher Already Redeemed";
                        Errorid.ForeColor = Color.Red;
                        return;
                    }
                    DataTable dt_ = GetInvoicesForVoucherCentralized(clvar);
                    if (dt_.Rows.Count > 0)
                    {
                        dt_.Columns.Add("RedeemAmt", typeof(double));
                        dt_.AcceptChanges();
                        gv_invoices.DataSource = dt_;
                        gv_invoices.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invoices Not Found')", true);
                        Errorid.Text = "Invoices Not Found";
                        Errorid.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    txt_clientName.Text = dt.Rows[0]["ClientName"].ToString();
                    txt_clientAccNo.Text = dt.Rows[0]["AccountNo"].ToString();


                    clvar.CreditClientID = dt.Rows[0]["CreditClientID"].ToString();
                    if (balAmount == 0 || balAmount < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher Already Redeemed')", true);
                        Errorid.Text = "Voucher Already Redeemed";
                        Errorid.ForeColor = Color.Red;
                        return;
                    }
                    DataTable dt_ = GetInvoicesForVoucherNonCentralized(clvar);

                    if (dt_.Rows.Count > 0)
                    {
                        dt_.Columns.Add("RedeemAmt", typeof(double));
                        dt_.AcceptChanges();
                        gv_invoices.DataSource = dt_;
                        gv_invoices.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invoices Not Found')", true);
                        Errorid.Text = "Invoices Not Found";
                        Errorid.ForeColor = Color.Red;
                        return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Voucher Does not Exist')", true);
                Errorid.Text = "Voucher Does not Exist";
                Errorid.ForeColor = Color.Red;
            }

        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void ResetAll()
        {

            txt_balAmt.Text = "";
            txt_chequeNo.Text = "";
            txt_clientAccNo.Text = "";
            txt_clientName.Text = "";
            txt_invoiceNo.Text = "";
            txt_payVno.Text = "";
            txt_redeemAmt.Text = "";
            txt_vAmt.Text = "";
            gv_invoices.DataSource = null;
            gv_invoices.DataBind();



        }
        protected void btn_save_Click(object sender, EventArgs e)
        {


            clvar.VoucherNo = txt_payVno.Text;
            clvar.ChequeNo = txt_chequeNo.Text;
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(Int64));
            dt.Columns.Add("PaymentVoucherId", typeof(Int64));
            dt.Columns.Add("InvoiceNo", typeof(string));
            dt.Columns.Add("Amount", typeof(float));
            dt.Columns.Add("CreatedBy", typeof(string));
            dt.Columns.Add("CreatedOn", typeof(DateTime));
            dt.AcceptChanges();
            foreach (GridViewRow row in gv_invoices.Rows)
            {
                TextBox txt_gRedeemAmt = row.FindControl("txt_gRedeemAmt") as TextBox;
                if (txt_gRedeemAmt.Text.Trim() != "" && txt_gRedeemAmt.Text.Trim() != "0")
                {
                    DataRow dr = dt.NewRow();
                    dr["PaymentVoucherId"] = Int64.Parse(clvar.VoucherNo);
                    dr["invoiceNo"] = (row.FindControl("lbl_gInvoiceNo") as Label).Text;
                    dr["Amount"] = float.Parse((row.FindControl("txt_gRedeemAmt") as TextBox).Text);
                    dr["createdBy"] = HttpContext.Current.Session["U_ID"].ToString();
                    dr["CreatedOn"] = DateTime.Now;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            string error = rec.InsertInvoiceRedeem(dt, clvar);
            if (error == "OK")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Redemption Complete')", true);
                Errorid.Text = "Redemption Complete";
                Errorid.ForeColor = Color.Green;
                ResetAll();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Redemption Could not be Completed')", true);
                Errorid.Text = "Redemption Could not be Completed";
                Errorid.ForeColor = Color.Red;
            }
        }

        public DataTable GetInvoicesForVoucherNonCentralized(Cl_Variables clvar)
        {
            #region sqlString___
            string sqlString___ = "SELECT b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       SUM(b.Invoice_Amount) Invoice_Amount,\n" +
            "       SUM(b.Recovery) RECOVERY,\n" +
            "       SUM(b.Adjustment) Adjustment,\n" +
            "       SUM(b.Invoice_Amount) - (SUM(b.Recovery) + SUM(b.Adjustment)) Oustanding\n" +
            "  FROM (SELECT i.invoiceNumber,\n" +
            "               i.clientId,\n" +
            "               SUM(i.totalAmount) Invoice_Amount,\n" +
            "               0 RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM Invoice AS i\n" +
            "           Where  i.IsInvoiceCanceled = '0'\n" +
            "         GROUP BY i.invoiceNumber, i.clientId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT ir.InvoiceNo,\n" +
            "               i.clientId,\n" +
            "               0 Invoice_Amount,\n" +
            "               SUM(ir.Amount) RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM InvoiceRedeem AS ir\n" +
            "         INNER JOIN Invoice AS i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "         INNER JOIN PaymentVouchers AS pv\n" +
            "            ON pv.Id = ir.PaymentVoucherId\n" +
            "         WHERE pv.PaymentSourceId = '1'\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "         GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT ir.InvoiceNo,\n" +
            "               i.clientId,\n" +
            "               0 Invoice_Amount,\n" +
            "               SUM(ir.Amount) RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM InvoiceRedeem AS ir\n" +
            "         INNER JOIN Invoice AS i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "         INNER JOIN PaymentVouchers AS pv\n" +
            "            ON pv.Id = ir.PaymentVoucherId\n" +
            "         INNER JOIN ChequeStatus AS cs\n" +
            "            ON cs.PaymentVoucherId = pv.Id\n" +
            "         WHERE pv.PaymentSourceId = '2'\n" +
            "           AND cs.IsCurrentState = '1'\n" +
            "           AND cs.ChequeStateId IN ('1', '2')\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "         GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT gv.InvoiceNo,\n" +
            "               gv.CreditClientId,\n" +
            "               0 Invoice_Amount,\n" +
            "               0 RECOVERY,\n" +
            "               SUM(gv.Amount) Adjustment\n" +
            "          FROM GeneralVoucher AS gv\n" +
            "         GROUP BY gv.InvoiceNo, gv.CreditClientId) b\n" +
            " WHERE b.clientId = '" + clvar.CreditClientID + "'\n" +
            " GROUP BY b.invoiceNumber, b.clientId\n" +
            "HAVING SUM(b.Invoice_Amount) - (SUM(b.Recovery) + SUM(b.Adjustment)) > 0";


            #endregion

            #region sqlString___27022019

            string sqlString___27022019 = "SELECT b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b2.name Branch,\n" +
            "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) Month,\n" +
            "       c.companyName,\n" +
            "       SUM(b.Invoice_Amount)     Total_Amount,\n" +
            "--       SUM(b.[Recovery])         RECOVERY,\n" +
            "--       SUM(b.Adjustment)         Adjustment,\n" +
            "--case when SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) > 1 then SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) else 0 end oustanding\n" +
            "       SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment))Oustanding\n" +
            "FROM   (\n" +
            "           SELECT i.invoiceNumber,\n" +
            "                  i.clientId,\n" +
            "                  SUM(i.totalAmount)     Invoice_Amount,\n" +
            "                  0                      RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   Invoice             AS i\n" +
            "           Where  i.IsInvoiceCanceled = '0' \n" +
            "           GROUP BY\n" +
            "                  i.invoiceNumber,\n" +
            "                  i.clientId\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT ir.InvoiceNo,\n" +
            "                  i.clientId,\n" +
            "                  0                      Invoice_Amount,\n" +
            "                  SUM(ir.Amount)         RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   InvoiceRedeem       AS ir\n" +
            "                  INNER JOIN Invoice  AS i\n" +
            "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
            "                  INNER JOIN PaymentVouchers AS pv\n" +
            "                       ON  pv.Id = ir.PaymentVoucherId\n" +
            "           WHERE  pv.PaymentSourceId = '1'\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "           GROUP BY\n" +
            "                  ir.InvoiceNo,\n" +
            "                  i.clientId\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT ir.InvoiceNo,\n" +
            "                  i.clientId,\n" +
            "                  0                      Invoice_Amount,\n" +
            "                  SUM(ir.Amount)         RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   InvoiceRedeem       AS ir\n" +
            "                  INNER JOIN Invoice  AS i\n" +
            "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
            "                  INNER JOIN PaymentVouchers AS pv\n" +
            "                       ON  pv.Id = ir.PaymentVoucherId\n" +
            "                  INNER JOIN ChequeStatus AS cs\n" +
            "                       ON  cs.PaymentVoucherId = pv.Id\n" +
            "           WHERE  pv.PaymentSourceId in ('2', '3', '4')\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "                  AND cs.IsCurrentState = '1'\n" +
            "                  AND cs.ChequeStateId IN ('1', '2')\n" +
            "           GROUP BY\n" +
            "                  ir.InvoiceNo,\n" +
            "                  i.clientId\n" +
            "\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT gv.InvoiceNo,\n" +
            "                  gv.CreditClientId,\n" +
            "                  0                  Invoice_Amount,\n" +
            "                  0                  RECOVERY,\n" +
            "                  SUM(gv.Amount)     Adjustment\n" +
            "           FROM   GeneralVoucher  AS gv\n" +
            "           GROUP BY\n" +
            "                  gv.InvoiceNo,\n" +
            "                  gv.CreditClientId\n" +
            "       )                         b\n" +
            "INNER JOIN Invoice AS i ON i.invoiceNumber=b.invoiceNumber\n" +
            "INNER JOIN CreditClients AS cc ON cc.id=b.clientId\n" +
            "INNER JOIN Branches AS b2 ON b2.branchCode = cc.branchCode\n" +
            "INNER JOIN Company AS c ON c.Id=i.companyId\n" +
            "\n" +
            "WHERE  b.clientId = '" + clvar.CreditClientID + "'\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
           "GROUP BY\n" +
            "       b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b2.name ,\n" +
            "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) ,\n" +
            "       c.companyName\n" +
            "HAVING SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) >= 1";

            #endregion

            string sqlString = "SELECT b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b2.name Branch,\n" +
            "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) Month,\n" +
            "       c.companyName,\n" +
            "       SUM(b.Invoice_Amount)     Total_Amount,\n" +
            "--       SUM(b.[Recovery])         RECOVERY,\n" +
            "--       SUM(b.Adjustment)         Adjustment,\n" +
            "--case when SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) > 1 then SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) else 0 end oustanding\n" +
            "       SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment))Oustanding\n" +
            "FROM   (\n" +
            "           SELECT i.invoiceNumber,\n" +
            "                  i.clientId,\n" +
            "                  SUM(i.totalAmount)     Invoice_Amount,\n" +
            "                  0                      RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   Invoice             AS i\n" +
            "           Where  i.IsInvoiceCanceled = '0' \n" +
            "           GROUP BY\n" +
            "                  i.invoiceNumber,\n" +
            "                  i.clientId\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT ir.InvoiceNo,\n" +
            "                  i.clientId,\n" +
            "                  0                      Invoice_Amount,\n" +
            "                  SUM(ir.Amount)         RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   InvoiceRedeem       AS ir\n" +
            "                  INNER JOIN Invoice  AS i\n" +
            "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
            "                  INNER JOIN PaymentVouchers AS pv\n" +
            "                       ON  pv.Id = ir.PaymentVoucherId\n" +
            "           WHERE  pv.PaymentSourceId IN ('1','5') \n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "           GROUP BY\n" +
            "                  ir.InvoiceNo,\n" +
            "                  i.clientId\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT ir.InvoiceNo,\n" +
            "                  i.clientId,\n" +
            "                  0                      Invoice_Amount,\n" +
            "                  SUM(ir.Amount)         RECOVERY,\n" +
            "                  0                      Adjustment\n" +
            "           FROM   InvoiceRedeem       AS ir\n" +
            "                  INNER JOIN Invoice  AS i\n" +
            "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
            "                  INNER JOIN PaymentVouchers AS pv\n" +
            "                       ON  pv.Id = ir.PaymentVoucherId\n" +
            "                  INNER JOIN ChequeStatus AS cs\n" +
            "                       ON  cs.PaymentVoucherId = pv.Id\n" +
            "           WHERE  pv.PaymentSourceId in ('2', '3', '4')\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
            "                  AND cs.IsCurrentState = '1'\n" +
            "                  AND cs.ChequeStateId IN ('1', '2')\n" +
            "           GROUP BY\n" +
            "                  ir.InvoiceNo,\n" +
            "                  i.clientId\n" +
            "\n" +
            "\n" +
            "           UNION\n" +
            "\n" +
            "           SELECT gv.InvoiceNo,\n" +
            "                  gv.CreditClientId,\n" +
            "                  0                  Invoice_Amount,\n" +
            "                  0                  RECOVERY,\n" +
            "                  SUM(gv.Amount)     Adjustment\n" +
            "           FROM   GeneralVoucher  AS gv\n" +
            "           GROUP BY\n" +
            "                  gv.InvoiceNo,\n" +
            "                  gv.CreditClientId\n" +
            "       )                         b\n" +
            "INNER JOIN Invoice AS i ON i.invoiceNumber=b.invoiceNumber\n" +
            "INNER JOIN CreditClients AS cc ON cc.id=b.clientId\n" +
            "INNER JOIN Branches AS b2 ON b2.branchCode = cc.branchCode\n" +
            "INNER JOIN Company AS c ON c.Id=i.companyId\n" +
            "\n" +
            "WHERE  b.clientId = '" + clvar.CreditClientID + "'\n" +
            "           and  i.IsInvoiceCanceled = '0' \n" +
           "GROUP BY\n" +
            "       b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b2.name ,\n" +
            "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) ,\n" +
            "       c.companyName\n" +
            "HAVING SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) >= 1";

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

        public DataTable GetInvoicesForVoucherCentralized(Cl_Variables clvar)
        {
            #region sqlString___
            string sqlString___ = "SELECT b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b.GroupID,\n" +
            "       SUM(b.Invoice_Amount) Invoice_Amount,\n" +
            "       SUM(b. [ Recovery ]) RECOVERY,\n" +
            "       SUM(b.Adjustment) Adjustment,\n" +
            "       SUM(b.Invoice_Amount) - (SUM(b. [ Recovery ]) + SUM(b.Adjustment)) Oustanding\n" +
            "  FROM (SELECT i.invoiceNumber,\n" +
            "               i.clientId,\n" +
            "               cc.clientGrpId GroupID,\n" +
            "               SUM(i.totalAmount) Invoice_Amount,\n" +
            "               0 RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM Invoice AS i\n" +
            "         INNER JOIN CreditClients AS cc\n" +
            "            ON cc.id = i.clientId\n" +
            "         GROUP BY i.invoiceNumber, i.clientId, clientGrpId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT ir.InvoiceNo,\n" +
            "               i.clientId,\n" +
            "               cc.clientGrpId GroupID,\n" +
            "               0 Invoice_Amount,\n" +
            "               SUM(ir.Amount) RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM InvoiceRedeem AS ir\n" +
            "         INNER JOIN Invoice AS i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "         INNER JOIN PaymentVouchers AS pv\n" +
            "            ON pv.Id = ir.PaymentVoucherId\n" +
            "         INNER JOIN CreditClients AS cc\n" +
            "            ON cc.id = i.clientId\n" +
            "         WHERE pv.PaymentSourceId = '1'\n" +
            "         GROUP BY ir.InvoiceNo, i.clientId, cc.clientGrpId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT ir.InvoiceNo,\n" +
            "               i.clientId,\n" +
            "               cc.clientGrpId GroupID,\n" +
            "               0 Invoice_Amount,\n" +
            "               SUM(ir.Amount) RECOVERY,\n" +
            "               0 Adjustment\n" +
            "          FROM InvoiceRedeem AS ir\n" +
            "         INNER JOIN Invoice AS i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "         INNER JOIN PaymentVouchers AS pv\n" +
            "            ON pv.Id = ir.PaymentVoucherId\n" +
            "         INNER JOIN ChequeStatus AS cs\n" +
            "            ON cs.PaymentVoucherId = pv.Id\n" +
            "         INNER JOIN CreditClients AS cc\n" +
            "            ON cc.id = i.clientId\n" +
            "         WHERE pv.PaymentSourceId in ('2', '3', '4')\n" +
            "           AND cs.IsCurrentState = '1'\n" +
            "           AND cs.ChequeStateId IN ('1', '2')\n" +
            "         GROUP BY ir.InvoiceNo, i.clientId, cc.clientGrpId\n" +
            "\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT gv.InvoiceNo,\n" +
            "               gv.CreditClientId,\n" +
            "               cc.clientGrpId GroupID,\n" +
            "               0 Invoice_Amount,\n" +
            "               0 RECOVERY,\n" +
            "               SUM(gv.Amount) Adjustment\n" +
            "          FROM GeneralVoucher AS gv\n" +
            "         INNER JOIN CreditClients AS cc\n" +
            "            ON cc.id = gv.CreditClientId\n" +
            "         GROUP BY gv.InvoiceNo, gv.CreditClientId, cc.clientGrpId) b\n" +
            " WHERE b.GroupID = '" + clvar.ClientGroupID + "'\n" +
            " GROUP BY b.invoiceNumber, b.clientId, b.GroupID\n" +
            "HAVING SUM(b.Invoice_Amount) - (SUM(b. [ Recovery ]) + SUM(b.Adjustment)) > 0";


            #endregion

            #region sqlString___27022019

            string sqlString___27022019 = "SELECT b.invoiceNumber,\n" +
           "       b.clientId,\n" +
           "       b.GroupID,\n" +
           "       b2.name Branch,\n" +
           "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) Month,\n" +
           "       c.companyName,\n" +
           "       SUM(b.Invoice_Amount)     Total_Amount,\n" +
           "--       SUM(b.Recovery)         RECOVERY,\n" +
           "--       SUM(b.Adjustment)         Adjustment,\n" +
           "--case when SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) > 1 then SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) else 0 end oustanding\n" +
           "       SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment))Oustanding\n" +
           "FROM   (\n" +
           "           SELECT i.invoiceNumber,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId         GroupID,\n" +
           "                  SUM(i.totalAmount)     Invoice_Amount,\n" +
           "                  0                      RECOVERY,\n" +
           "                  0                      Adjustment\n" +
           "           FROM   Invoice             AS i\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = i.clientId\n" +
           "           Where  i.IsInvoiceCanceled = '0'\n" +
           "           GROUP BY\n" +
           "                  i.invoiceNumber,\n" +
           "                  i.clientId,\n" +
           "                  clientGrpId\n" +
           "\n" +
           "           UNION\n" +
           "\n" +
           "           SELECT ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId         GroupID,\n" +
           "                  0                      Invoice_Amount,\n" +
           "                  SUM(ir.Amount)         RECOVERY,\n" +
           "                  0                      Adjustment\n" +
           "           FROM   InvoiceRedeem       AS ir\n" +
           "                  INNER JOIN Invoice  AS i\n" +
           "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
           "                  INNER JOIN PaymentVouchers AS pv\n" +
           "                       ON  pv.Id = ir.PaymentVoucherId\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = i.clientId\n" +
           "           WHERE  pv.PaymentSourceId = '1'\n" +
           "                  and i.IsInvoiceCanceled = '0'\n" +
           "           GROUP BY\n" +
           "                  ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId\n" +
           "\n" +
           "           UNION\n" +
           "\n" +
           "           SELECT ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId         GroupID,\n" +
           "                  0                      Invoice_Amount,\n" +
           "                  SUM(ir.Amount)         RECOVERY,\n" +
           "                  0                      Adjustment\n" +
           "           FROM   InvoiceRedeem       AS ir\n" +
           "                  INNER JOIN Invoice  AS i\n" +
           "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
           "                  INNER JOIN PaymentVouchers AS pv\n" +
           "                       ON  pv.Id = ir.PaymentVoucherId\n" +
           "                  INNER JOIN ChequeStatus AS cs\n" +
           "                       ON  cs.PaymentVoucherId = pv.Id\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = i.clientId\n" +
           "           WHERE  pv.PaymentSourceId = '2'\n" +
           "                  AND cs.IsCurrentState = '1'\n" +
           "                  and i.IsInvoiceCanceled = '0'\n" +
           "                  AND cs.ChequeStateId IN ('1', '2')\n" +
           "           GROUP BY\n" +
           "                  ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId\n" +
           "\n" +
           "\n" +
           "           UNION\n" +
           "\n" +
           "           SELECT gv.InvoiceNo,\n" +
           "                  gv.CreditClientId,\n" +
           "                  cc.clientGrpId     GroupID,\n" +
           "                  0                  Invoice_Amount,\n" +
           "                  0                  RECOVERY,\n" +
           "                  SUM(gv.Amount)     Adjustment\n" +
           "           FROM   GeneralVoucher  AS gv\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = gv.CreditClientId\n" +
           "           GROUP BY\n" +
           "                  gv.InvoiceNo,\n" +
           "                  gv.CreditClientId,\n" +
           "                  cc.clientGrpId\n" +
           "       )                         b\n" +
           "INNER JOIN Invoice AS i ON i.invoiceNumber=b.invoiceNumber\n" +
           "INNER JOIN CreditClients AS cc ON cc.id=b.clientId\n" +
           "INNER JOIN Branches AS b2 ON b2.branchCode = cc.branchCode\n" +
           "INNER JOIN Company AS c ON c.Id=i.companyId\n" +
           "\n" +
           "WHERE  b.GroupID = '" + clvar.ClientGroupID + "'\n" +
           " and i.IsInvoiceCanceled = '0'\n" +
           "GROUP BY\n" +
           "       b.invoiceNumber,\n" +
           "       b.clientId,\n" +
           "       b.GroupID,\n" +
           "       b2.name ,\n" +
           "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) ,\n" +
           "       c.companyName\n" +
           "\n" +
           "HAVING SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) >= 1\n" +
           "";

            #endregion


            string sqlString = "SELECT b.invoiceNumber,\n" +
           "       b.clientId,\n" +
           "       b.GroupID,\n" +
           "       b2.name Branch,\n" +
           "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) Month,\n" +
           "       c.companyName,\n" +
           "       SUM(b.Invoice_Amount)     Total_Amount,\n" +
           "--       SUM(b.Recovery)         RECOVERY,\n" +
           "--       SUM(b.Adjustment)         Adjustment,\n" +
           "--case when SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) > 1 then SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) else 0 end oustanding\n" +
           "       SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment))Oustanding\n" +
           "FROM   (\n" +
           "           SELECT i.invoiceNumber,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId         GroupID,\n" +
           "                  SUM(i.totalAmount)     Invoice_Amount,\n" +
           "                  0                      RECOVERY,\n" +
           "                  0                      Adjustment\n" +
           "           FROM   Invoice             AS i\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = i.clientId\n" +
           "           Where  i.IsInvoiceCanceled = '0'\n" +
           "           GROUP BY\n" +
           "                  i.invoiceNumber,\n" +
           "                  i.clientId,\n" +
           "                  clientGrpId\n" +
           "\n" +
           "           UNION\n" +
           "\n" +
           "           SELECT ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId         GroupID,\n" +
           "                  0                      Invoice_Amount,\n" +
           "                  SUM(ir.Amount)         RECOVERY,\n" +
           "                  0                      Adjustment\n" +
           "           FROM   InvoiceRedeem       AS ir\n" +
           "                  INNER JOIN Invoice  AS i\n" +
           "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
           "                  INNER JOIN PaymentVouchers AS pv\n" +
           "                       ON  pv.Id = ir.PaymentVoucherId\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = i.clientId\n" +
           "           WHERE  pv.PaymentSourceId in ('1','5') \n" +
           "                  and i.IsInvoiceCanceled = '0'\n" +
           "           GROUP BY\n" +
           "                  ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId\n" +
           "\n" +
           "           UNION\n" +
           "\n" +
           "           SELECT ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId         GroupID,\n" +
           "                  0                      Invoice_Amount,\n" +
           "                  SUM(ir.Amount)         RECOVERY,\n" +
           "                  0                      Adjustment\n" +
           "           FROM   InvoiceRedeem       AS ir\n" +
           "                  INNER JOIN Invoice  AS i\n" +
           "                       ON  i.invoiceNumber = ir.InvoiceNo\n" +
           "                  INNER JOIN PaymentVouchers AS pv\n" +
           "                       ON  pv.Id = ir.PaymentVoucherId\n" +
           "                  INNER JOIN ChequeStatus AS cs\n" +
           "                       ON  cs.PaymentVoucherId = pv.Id\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = i.clientId\n" +
           "           WHERE  pv.PaymentSourceId in ('2','3','4')\n" +
           "                  AND cs.IsCurrentState = '1'\n" +
           "                  and i.IsInvoiceCanceled = '0'\n" +
           "                  AND cs.ChequeStateId IN ('1', '2')\n" +
           "           GROUP BY\n" +
           "                  ir.InvoiceNo,\n" +
           "                  i.clientId,\n" +
           "                  cc.clientGrpId\n" +
           "\n" +
           "\n" +
           "           UNION\n" +
           "\n" +
           "           SELECT gv.InvoiceNo,\n" +
           "                  gv.CreditClientId,\n" +
           "                  cc.clientGrpId     GroupID,\n" +
           "                  0                  Invoice_Amount,\n" +
           "                  0                  RECOVERY,\n" +
           "                  SUM(gv.Amount)     Adjustment\n" +
           "           FROM   GeneralVoucher  AS gv\n" +
           "                  INNER JOIN CreditClients AS cc\n" +
           "                       ON  cc.id = gv.CreditClientId\n" +
           "           GROUP BY\n" +
           "                  gv.InvoiceNo,\n" +
           "                  gv.CreditClientId,\n" +
           "                  cc.clientGrpId\n" +
           "       )                         b\n" +
           "INNER JOIN Invoice AS i ON i.invoiceNumber=b.invoiceNumber\n" +
           "INNER JOIN CreditClients AS cc ON cc.id=b.clientId\n" +
           "INNER JOIN Branches AS b2 ON b2.branchCode = cc.branchCode\n" +
           "INNER JOIN Company AS c ON c.Id=i.companyId\n" +
           "\n" +
           "WHERE  b.GroupID = '" + clvar.ClientGroupID + "'\n" +
           " and i.IsInvoiceCanceled = '0'\n" +
           "GROUP BY\n" +
           "       b.invoiceNumber,\n" +
           "       b.clientId,\n" +
           "       b.GroupID,\n" +
           "       b2.name ,\n" +
           "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) ,\n" +
           "       c.companyName\n" +
           "\n" +
           "HAVING SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) >= 1\n" +
           "";

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