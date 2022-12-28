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
    public partial class InvoiceRedeemAdj : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void rbtn_formMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void txt_voucherNo_TextChanged(object sender, EventArgs e)
        {
            if (rbtn_formMode.SelectedValue == "1")
            {
                #region Search By InvoiceNumber
                clvar.CheckCondition = " AND ir.InvoiceNo = '" + txt_voucherNo.Text + "'";
                clvar.InvoiceNo = txt_voucherNo.Text.Trim();
                DataTable dt = GetRedeemedInvoices(clvar);
                DataTable dtHead = GetInvoiceHeader(clvar);
                if (dtHead.Rows.Count > 0)
                {
                    txt_invoiceDate.Text = dtHead.Rows[0]["invoiceDate"].ToString();
                    txt_accountNo.Text = dtHead.Rows[0]["AccountNo"].ToString();
                    txt_clientName.Text = dtHead.Rows[0]["name"].ToString();
                }
                else
                {
                    AlertMessage("Invalid Invoice", Color.Red);
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    gv_redemptions.DataSource = dt;
                    gv_redemptions.DataBind();
                }
                else
                {
                    gv_redemptions.DataSource = null;
                    gv_redemptions.DataBind();
                    AlertMessage("No Redemptions Found", Color.Red);
                }
                #endregion
            }
            else
            {
                #region Search By Voucher Number

                #endregion
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_voucherNo.Text = "";
            txt_accountNo.Text = "";
            txt_clientName.Text = "";
            txt_invoiceDate.Text = "";
            gv_redemptions.DataSource = null;
            gv_redemptions.DataBind();
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("InvoiceRedeemID");
            dt.Columns.Add("invoiceNo");
            dt.Columns.Add("VoucherNo");
            dt.Columns.Add("RedeemAmount");
            dt.Columns.Add("RedeemAmountNew");
            dt.Columns.Add("voucherUsedAmount");
            dt.Columns.Add("voucherUsedAmountNew");
            dt.Columns.Add("adjAmount");

            dt.AcceptChanges();
            string invoiceNo = txt_voucherNo.Text;
            foreach (GridViewRow row in gv_redemptions.Rows)
            {
                float redeemAmount = float.Parse(row.Cells[4].Text);
                float voucherUsedAmount = float.Parse(row.Cells[5].Text);
                TextBox txt_adjAmount = row.FindControl("txt_adjAmount") as TextBox;
                HiddenField hd_irID = row.FindControl("hd_irID") as HiddenField;
                float adjAmount = 0f;
                if (txt_adjAmount.Text.Trim() != "")
                {
                    adjAmount = float.Parse(txt_adjAmount.Text);
                    if (adjAmount > redeemAmount || adjAmount > voucherUsedAmount)
                    {
                        AlertMessage("Adj Amount Cannot be Greater than Redempted Amount or Voucher Used Amount", Color.Red);
                        row.BackColor = Color.FromName("#f7a0a0");
                    }
                    else
                    {
                        row.BackColor = Color.White;
                        DataRow dr = dt.NewRow();
                        dr["InvoiceRedeemID"] = hd_irID.Value;
                        dr["invoiceNo"] = row.Cells[0].Text;
                        dr["VoucherNo"] = row.Cells[1].Text;
                        dr["RedeemAmount"] = row.Cells[4].Text;
                        dr["RedeemAmountNew"] = Math.Round(redeemAmount - adjAmount, 0).ToString();
                        dr["VoucherUsedAmount"] = row.Cells[6].Text;
                        dr["VoucherUsedAmountNEW"] = Math.Round(voucherUsedAmount - adjAmount, 0).ToString();
                        dr["adjAmount"] = adjAmount.ToString();
                        dt.Rows.Add(dr);
                    }
                }
            }


            if (dt.Rows.Count > 0)
            {
                string errorMessage = AdjustInvoiceRedeem(dt);
                if (errorMessage == "OK")
                {
                    AlertMessage("Adjustment Complete", Color.Green);
                    btn_reset_Click(this, e);
                }
                else
                {
                    AlertMessage("Adjustment Could Not Be Completed", Color.Red);
                }
            }
        }


        public DataTable GetRedeemedInvoices(Cl_Variables clvar)
        {

            string sqlString = "select ir.InvoiceNo,\n" +
            "       pv.Id         VoucherID,\n" +
            "       pv.ChequeNo,\n" +
            "       ir.Amount     RedemptedAmount,\n" +
            "       i.totalAmount InvoiceAmount,\n" +
            "       pv.Amount     VoucherAmount,\n" +
            "       pv.AmountUsed VoucherUsedAmount,\n" +
            "       ''            AdjAmount\n" +
            "  from InvoiceRedeem ir\n" +
            " inner join Invoice i\n" +
            "    on i.invoiceNumber = ir.InvoiceNo\n" +
            " inner join PaymentVouchers pv\n" +
            "    on pv.Id = ir.PaymentVoucherId\n" +
            "  left outer join ChequeStatus cs\n" +
            "    on cs.PaymentVoucherId = pv.Id\n" +
            " where cs.ChequeStateId IN ('1', '2') \n" +
            "   " + clvar.CheckCondition + "";

            sqlString = "select ir.id InvoiceRedeemID, ir.InvoiceNo,\n" +
           "       pv.Id         VoucherID,\n" +
           "       pv.ChequeNo,\n" +
           "       ir.Amount     RedemptedAmount,\n" +
           "       i.totalAmount InvoiceAmount,\n" +
           "       pv.Amount     VoucherAmount,\n" +
           "       pv.AmountUsed VoucherUsedAmount,\n" +
           "       ''            AdjAmount\n" +
           "  from InvoiceRedeem ir\n" +
           " inner join Invoice i\n" +
           "    on i.invoiceNumber = ir.InvoiceNo\n" +
           " inner join PaymentVouchers pv\n" +
           "    on pv.Id = ir.PaymentVoucherId\n" +
           "  left outer join ChequeStatus cs\n" +
           "    on cs.PaymentVoucherId = pv.Id\n" +
           " where cs.ChequeStateId IN ('1', '2')\n" +
           "   and pv.PaymentSourceId in ('2', '3', '4')\n" +
           "   and ir.Amount > 0 \n" +
           "  " + clvar.CheckCondition + "\n" +
           "union\n" +
           "select ir.id InvoiceRedeemID, ir.InvoiceNo,\n" +
           "       pv.Id         VoucherID,\n" +
           "       pv.ChequeNo,\n" +
           "       ir.Amount     RedemptedAmount,\n" +
           "       i.totalAmount InvoiceAmount,\n" +
           "       pv.Amount     VoucherAmount,\n" +
           "       pv.AmountUsed VoucherUsedAmount,\n" +
           "       ''            AdjAmount\n" +
           "  from InvoiceRedeem ir\n" +
           " inner join Invoice i\n" +
           "    on i.invoiceNumber = ir.InvoiceNo\n" +
           " inner join PaymentVouchers pv\n" +
           "    on pv.Id = ir.PaymentVoucherId\n" +
           "\n" +
           " where pv.PaymentSourceId = '1'\n" +
           "   and ir.Amount > 0 \n" +
           "   " + clvar.CheckCondition + "";

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
        public DataTable GetInvoiceHeader(Cl_Variables clvar)
        {
            string query = "";
            DataTable dt = new DataTable();


            string sqlString = "select i.*, cc.accountNo, cc.name\n" +
            "  from invoice i\n" +
            " inner join creditClients cc\n" +
            "    on i.clientID = cc.id\n" +
            " where i.invoiceNumber = '" + clvar.InvoiceNo + "' and cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
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

        public void AlertMessage(string message, System.Drawing.Color color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = color;
        }

        public string AdjustInvoiceRedeem(DataTable dt)
        {
            string errorMessage = "";
            List<string> queries = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                string query = "";
                query += "insert into invoiceRedeemAdj(invoiceRedeemID, OLDAmount, NewAmount, Createdon, CreatedBy)\n" +
                         " VALUES(\n" +
                         "'" + row["InvoiceRedeemID"].ToString() + "',\n" +
                         "'" + row["RedeemAmount"].ToString() + "',\n" +
                         "'" + row["RedeemAmountNew"].ToString() + "',\n" +
                         "GETDATE(),\n" +
                         "'" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                         " )";
                queries.Add(query);

                query = "update invoiceRedeem set amount = '" + row["RedeemAmountNew"].ToString() + "' where id = '" + row["InvoiceRedeemID"].ToString() + "'";
                queries.Add(query);

                query = "update paymentVouchers set AmountUsed = (AmountUsed - " + row["adjAmount"].ToString() + ")  where id = '" + row["VoucherNo"].ToString() + "' and (AmountUsed - " + row["adjAmount"].ToString() + ") >= 0 ";
                queries.Add(query);

                query = "SELECT * FROM PAYMENTVOUCHERS WHERE ID = '" + row["VoucherNo"].ToString() + "'";
                //queries.Add(query);
            }

            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlConnection con_ = new SqlConnection(clvar.Strcon());

            SqlTransaction trans;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            trans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            try
            {
                //con.Open();
                int count = 0;
                for (int i = 0; i < queries.Count; i++)
                {


                    cmd.CommandText = queries[i];
                    int inCount = cmd.ExecuteNonQuery();
                    if (inCount == 0)
                    {
                        trans.Rollback();
                        con.Close();
                        errorMessage = "Cannot Adjust.";
                        return errorMessage;

                    }
                    count++;
                }

                trans.Commit();
                errorMessage = "OK";
            }
            catch (Exception ex)
            {
                CommonFunction func = new CommonFunction();
                func.InsertErrorLog("", "", "", "", "", "", "Invoice Redeem Adjustment", ex.Message.Replace("'", ""));
                trans.Rollback();
                errorMessage = ex.Message;
            }
            finally { con.Close(); }
            return errorMessage;
        }
    }
}