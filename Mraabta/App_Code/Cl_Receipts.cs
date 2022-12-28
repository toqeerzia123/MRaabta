using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Cl_Receipts
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Cl_Receipts
    {
        Cl_Variables clvar = new Cl_Variables();
        public Cl_Receipts()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataTable GetPaymentVoucherTypes()
        {
            string query = "select * from PaymentTypes";
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
        public DataTable GetPaymentSources()
        {
            string query = "select * from PaymentSource";
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
        public DataTable GetBanks()
        {

            string query = "select * from Banks";

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
        public DataTable GetClientGroups()
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = "select * from ClientGroups cg where cg.collectionCenter='" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
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
        public DataTable GetRidersByExpressCenter(Cl_Variables clvar)
        {
            string sqlString = "select  r.firstName+' '+r.lastName NAME, r.riderCode from Riders r where r.expressCenterId = '" + clvar.expresscenter + "' and r.branchId = '" + clvar.Branch + "' and r.status = '1'  order by 1";
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
        public string GeneratePaymentVoucher(Cl_Variables clvar)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            string id = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("MnP_CreatePaymentVoucher", con);
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
        public string GenerateCashPaymentVoucher(Cl_Variables clvar)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            string id = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("MnP_CreateCashPaymentVoucher", con);
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

        public DataTable GetReceiptVoucherHeader(Cl_Variables clvar)
        {

            string sqlString = "select cc.name ClientName, cc.accountNo, cg.name ClientGrpName, p.*, (p.amount - p.amountused) BalAmount \n" +
            "  from PaymentVouchers p\n" +
            "  left outer join CreditClients cc\n" +
            "    on cc.id = p.CreditClientId\n" +
            "  left outer join ClientGroups cg\n" +
            "    on cg.id = p.ClientGroupId\n" +
            " where p.Id = '" + clvar.VoucherNo + "' and p.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

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
            string sqlString = "SELECT b.invoiceNumber,\n" +
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
            "         WHERE pv.PaymentSourceId = '2'\n" +
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


            sqlString = "SELECT b.invoiceNumber,\n" +
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
        public DataTable GetInvoicesForVoucherNonCentralized(Cl_Variables clvar)
        {

            string sqlString = "SELECT b.invoiceNumber,\n" +
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



            sqlString = "SELECT b.invoiceNumber,\n" +
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
            "           WHERE  pv.PaymentSourceId = '2'\n" +
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

        public string InsertInvoiceRedeem(DataTable dt, Cl_Variables clvar)
        {
            if (dt.Rows.Count > 0)
            {

                using (SqlConnection con = new SqlConnection(clvar.Strcon()))
                {
                    con.Open();
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans))
                        {
                            try
                            {

                                string totalAmount = dt.Compute("SUM(Amount)", "").ToString();

                                //Set the database table name
                                sqlBulkCopy.DestinationTableName = "dbo.invoiceRedeem";

                                //[OPTIONAL]: Map the DataTable columns with that of the database table
                                //sqlBulkCopy.ColumnMappings.Add("PriceModifierID", "");
                                //sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                                SqlCommand cmd = new SqlCommand("UPDATE PaymentVouchers Set AmountUsed = (AmountUsed + " + totalAmount + ") where id = '" + clvar.VoucherNo + "'", con);
                                //sqlBulkCopy.ColumnMappings.Add("Country", "Country");

                                //con.Open();

                                sqlBulkCopy.WriteToServer(dt);
                                cmd.Transaction = trans;
                                cmd.ExecuteNonQuery();
                                trans.Commit();


                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();

                                clvar.Error = ex.Message;
                                return ex.Message;
                            }
                        }
                    }
                }
            }

            return "OK";
        }


        public DataTable GetChequeStates()
        {
            string sqlString = "selecT * from ChequeState";
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

        public DataTable GetCheques(Cl_Variables clvar, string month, string year)
        {

            string sqlString = "selecT cs.Id updateID, p.ChequeNo, p.Amount, b.Name DepositBank, cs.ChequeStateId id\n" +
            "  from PaymentVouchers p\n" +
            " inner join ChequeStatus cs\n" +
            "    on p.Id = cs.PaymentVoucherId\n" +
            "  left outer join Banks b\n" +
            "    on b.Id = cs.DepositBankId\n" +
            " inner join ChequeState css\n" +
            "    on css.Id = cs.ChequeStateId\n" +
            " where MONTH(p.VoucherDate) = '" + month + "'\n" +
            "   and YEAR(p.VoucherDate) = '" + year + "'\n" +
            "   and p.bankID = '" + clvar.Bank + "' and p.BranchCode = '" + HttpContext.Current.Session["BranchCode"] + "'\n";
            if (clvar.CheckCondition != "")
            {
                sqlString = "   and cs.DepositBankId = '" + clvar.CheckCondition + "'";
            }
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

        public DataTable ReceiptNo()
        {
            string sql = "SELECT *, (codeValue+1) No\n"
               + "			                FROM   SystemCodes \n"
               + "			                WHERE  codeType = 'RECIEPT_VOUCHER' \n"
               + "			                       \n"
               + "			                       AND [year] = CONVERT(VARCHAR, YEAR(GETDATE()))";
            DataTable dt = new DataTable();
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

            string sqlString = "UPDATE SystemCodes            \n" +
                            "SET    codeValue     = (codeValue+1)\n" +
                            "WHERE  codeType      = 'RECIEPT_VOUCHER'  \n" +
                            "       AND [year] = CONVERT(VARCHAR, YEAR(GETDATE()))";
            con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlString, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return dt;
        }




        public void UpdateChequeStatus(Cl_Variables clvar, DataTable dt)
        {
            string query = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    query = "UPDATE ChequeStatus set chequeStateID = '" + dr[1].ToString() + "' where id = '" + dr[0].ToString() + "'";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }

            con.Close();



        }

        public string InsertReceiptAdjustment(Cl_Variables clvar)
        {
            string query = "Update paymentVouchers set amount = amount + " + clvar.CheckCondition + " where id = '" + clvar.VoucherNo + "'";
            string query_ = "insert into PaymentVoucherAdj (PaymentVoucherID, OldAmount, NewAmount, Createdon, CreatedBy) \n" +
                           "VALUES ( \n" +
                           " '" + clvar.VoucherNo + "', '" + clvar.TotalAmount + "', '" + (clvar.TotalAmount + double.Parse(clvar.CheckCondition)).ToString() + "',\n" +
                           " GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                           ")";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;

                trans = sqlcon.BeginTransaction();
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;

                sqlcmd.CommandText = query;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = query_;
                sqlcmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                sqlcon.Close();
                return "Error:" + ex.Message;

            }
            finally { sqlcon.Close(); }
            return "Success";

            //sqlcmd.CommandTimeout = 300;

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