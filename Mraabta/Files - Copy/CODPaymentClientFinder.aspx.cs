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
    public partial class CODPaymentClientFinder : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetZones(clvar);
            }
            //DataTable dt = GetClientsReport(clvar);
        }

        public void GetZones(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sqlString = "SELECT distinct z.zoneCode, z.name\n" +
            "  FROM Branches b\n" +
            " INNER JOIN Zones z\n" +
            "    ON z.zoneCode = b.zoneCode\n" +
            " WHERE z.[status] = '1'\n" +
            "   AND b.[status] = '1'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_Zone.DataSource = dt;
                    dd_Zone.DataTextField = "NAME";
                    dd_Zone.DataValueField = "Zonecode";
                    dd_Zone.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        public void GetBranches(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sqlString = "SELECT b.branchCode, b.sname + '-' + b.name BranchName\n" +
            "  FROM Branches b\n" +
            " INNER JOIN Zones z\n" +
            "    ON z.zoneCode = b.zoneCode\n" +
            " WHERE z.[status] = '1'\n" +
            "   AND b.[status] = '1'\n" +
            "   AND b.zoneCode = '" + clvar.Zone + "'";

            dd_branch.Items.Clear();
            dd_branch.Items.Add(new ListItem { Text = "Select Branch", Value = "0" });
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_branch.DataSource = dt;
                    dd_branch.DataTextField = "BranchNAME";
                    dd_branch.DataValueField = "BranchCode";
                    dd_branch.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        public DataTable GetClientsReport(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();


            string sqlString =
            "SELECT b.ORGZONE ZoneName,\n" +
            "       B.BranchName BranchName,\n" +
            "       B.CreditClientID,\n" +
            "       B.ACCOUNTNO AccNo,\n" +
            "       B.CUSTOMERNAME AccName,\n" +
            "       B.BenBank,\n" +
            "       B.BenBankCode,\n" +
            "       B.BenName,\n" +
            "       B.BenAccNo,\n" +
            "       COUNT(B.CONSIGNMENTNUMBER) CnCount,\n" +
            "       COUNT(B.VoucherID) RRCount,\n" +
            "       d.InvCount InvCount,\n" +
            "       SUM(B.CODAMOUNT) CODAmt,\n" +
            "       SUM(B.RRAmount) RRAmt,\n" +
            "       SUM(B.AvailableAmount) AvailableAMT,\n" +
            "       d.TotalInvoiceAmount InvoiceAMT,\n" +
            "       d.TotalOutStanding OutstandingAMT,\n" +
            "       (SUM(B.AvailableAmount) - ISNULL(d.TotalOutStanding,0)) NetPayable\n" +
            "  FROM (\n" +
            "        ----- COD PAYMENT REPORTS\n" +
            "        --KHI.ACC@OCS.COM.PK\n" +
            "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
            "                CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
            "                L.ATTRIBUTEVALUE RRSTATUS,\n" +
            "                ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID,\n" +
            "                pv.id VoucherID,\n" +
            "                PV.VOUCHERDATE,\n" +
            "                B4.SNAME COLLECTIONBR,\n" +
            "                CD.CODAMOUNT CODAMOUNT,\n" +
            "                pv.Amount RRAmount,\n" +
            "                (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
            "                Z.NAME ORGZONE,\n" +
            "                Z.ZONECODE ORGZONECODE,\n" +
            "                B2.sname BranchName,\n" +
            "                CC.ACCOUNTNO,\n" +
            "                CC.NAME CUSTOMERNAME,\n" +
            "                COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
            "                C.TRANSACTIONNUMBER CHEQUENO,\n" +
            "                R.STATUS,\n" +
            "                b5.Name BenBank,\n" +
            "                b5.SBPCode BenBankCode,\n" +
            "                cc.BeneficiaryName BenName,\n" +
            "                cc.BeneficiaryBankAccNo BenAccNo,\n" +
            "                cc.id CreditClientID\n" +
            "          FROM CONSIGNMENT C\n" +
            "         INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
            "            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
            "          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
            "            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
            "           AND R.CREATEDON =\n" +
            "               (SELECT MAX(CREATEDON)\n" +
            "                  FROM RUNSHEETCONSIGNMENT RC1\n" +
            "                 WHERE RC1.CONSIGNMENTNUMBER = R.CONSIGNMENTNUMBER)\n" +
            "          LEFT JOIN PAYMENTVOUCHERS PV\n" +
            "            ON PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER\n" +
            "           AND PV.CREDITCLIENTID = C.CREDITCLIENTID\n" +
            "         INNER JOIN CREDITCLIENTS CC\n" +
            "            ON CC.ID = C.CREDITCLIENTID\n" +
            "         INNER JOIN BRANCHES B2\n" +
            "            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
            "         INNER JOIN ZONES Z\n" +
            "            ON Z.ZONECODE = B2.ZONECODE\n" +
            "         INNER JOIN BRANCHES B3\n" +
            "            ON C.DESTINATION = B3.BRANCHCODE\n" +
            "         INNER JOIN ZONES Z2\n" +
            "            ON B3.ZONECODE = Z2.ZONECODE\n" +
            "         INNER JOIN Branches B4\n" +
            "            ON B4.branchCode = PV.BranchCode\n" +
            "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
            "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
            "          LEFT OUTER JOIN Banks b5\n" +
            "            ON b5.id = cc.BeneficiaryBankCode\n" +
            "           AND b5.isMNPBank = '0'\n" +
            "         WHERE C.COD = '1'\n" +
            "           AND c.isapproved = '1'\n" +
            "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
            "              --AND CC.ACCOUNTNO = '9M37'\n" +
            "           AND c.ispayable = '0'\n" +
            "           AND R.[Status] = '55'\n" +
            "           " + clvar.CheckCondition + "\n" +
            "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
            "                   CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
            "                   L.ATTRIBUTEVALUE,\n" +
            "                   CD.CODAMOUNT,\n" +
            "                   pv.Amount,\n" +
            "                   Z.NAME,\n" +
            "                   Z.ZONECODE,\n" +
            "                   b2.sname,\n" +
            "                   CC.ACCOUNTNO,\n" +
            "                   CC.NAME,\n" +
            "                   PV.RECEIPTNO,\n" +
            "                   pv.id,\n" +
            "                   C.ISPAYABLE,\n" +
            "                   C.TRANSACTIONNUMBER,\n" +
            "                   PV.VOUCHERDATE,\n" +
            "                   B4.SNAME,\n" +
            "                   PV.Amount,\n" +
            "                   PV.Amountused,\n" +
            "                   R.STATUS,\n" +
            "                   b5.Name,\n" +
            "                   b5.SBPCode,\n" +
            "                   cc.BeneficiaryName,\n" +
            "                   cc.BeneficiaryBankAccNo,\n" +
            "                   cc.id\n" +
            "        UNION ALL\n" +
            "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
            "                CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
            "                L.ATTRIBUTEVALUE RRSTATUS,\n" +
            "                ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID,\n" +
            "                pv.ID VoucherID,\n" +
            "                PV.VOUCHERDATE,\n" +
            "                B4.SNAME COLLECTIONBR,\n" +
            "                CD.CODAMOUNT CODAMOUNT,\n" +
            "                pv.Amount RRAmount,\n" +
            "                (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
            "                Z.NAME ORGZONE,\n" +
            "                Z.ZONECODE ORGZONECODE,\n" +
            "                b2.sname BranchName,\n" +
            "                CC.ACCOUNTNO,\n" +
            "                CC.NAME CUSTOMERNAME,\n" +
            "                COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
            "                C.TRANSACTIONNUMBER CHEQUENO,\n" +
            "                R.STATUS,\n" +
            "                b5.Name BenBank,\n" +
            "                b5.SBPCode BenBankCode,\n" +
            "                cc.BeneficiaryName BenName,\n" +
            "                cc.BeneficiaryBankAccNo BenAccNo,\n" +
            "                cc.id CreditClientID\n" +
            "          FROM CONSIGNMENT C\n" +
            "         INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
            "            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
            "          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
            "            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
            "           AND R.CREATEDON =\n" +
            "               (SELECT MAX(CREATEDON)\n" +
            "                  FROM RUNSHEETCONSIGNMENT RC1\n" +
            "                 WHERE RC1.CONSIGNMENTNUMBER = R.CONSIGNMENTNUMBER)\n" +
            "          LEFT JOIN PAYMENTVOUCHERS PV\n" +
            "            ON PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER\n" +
            "           AND PV.CREDITCLIENTID = C.CREDITCLIENTID\n" +
            "         INNER JOIN CREDITCLIENTS CC\n" +
            "            ON CC.ID = C.CREDITCLIENTID\n" +
            "         INNER JOIN BRANCHES B2\n" +
            "            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
            "         INNER JOIN ZONES Z\n" +
            "            ON Z.ZONECODE = B2.ZONECODE\n" +
            "         INNER JOIN BRANCHES B3\n" +
            "            ON C.DESTINATION = B3.BRANCHCODE\n" +
            "         INNER JOIN ZONES Z2\n" +
            "            ON B3.ZONECODE = Z2.ZONECODE\n" +
            "         INNER JOIN Branches B4\n" +
            "            ON B4.branchCode = PV.BranchCode\n" +
            "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
            "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
            "          LEFT OUTER JOIN Banks b5\n" +
            "            ON b5.id = cc.BeneficiaryBankCode\n" +
            "           AND b5.isMNPBank = '0'\n" +
            "         WHERE C.COD = '1'\n" +
            "           AND c.isapproved = '1'\n" +
            "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
            "              --AND CC.ACCOUNTNO = '9M37'\n" +
            "           AND c.ispayable = '0'\n" +
            "           AND R.[Status] = '55'\n" +
            "           " + clvar.CheckCondition + "\n" +
            "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
            "                   CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
            "                   L.ATTRIBUTEVALUE,\n" +
            "                   CD.CODAMOUNT,\n" +
            "                   pv.Amount,\n" +
            "                   Z.NAME,\n" +
            "                   Z.ZONECODE,\n" +
            "                   b2.sname,\n" +
            "                   CC.ACCOUNTNO,\n" +
            "                   CC.NAME,\n" +
            "                   PV.RECEIPTNO,\n" +
            "                   pv.id,\n" +
            "                   C.ISPAYABLE,\n" +
            "                   C.TRANSACTIONNUMBER,\n" +
            "                   PV.VOUCHERDATE,\n" +
            "                   B4.SNAME,\n" +
            "                   PV.amount,\n" +
            "                   PV.AmountUsed,\n" +
            "                   R.STATUS,\n" +
            "                   b5.Name,\n" +
            "                   b5.SBPCode,\n" +
            "                   cc.BeneficiaryName,\n" +
            "                   cc.BeneficiaryBankAccNo,\n" +
            "                   cc.id) B\n" +
            "  LEFT OUTER JOIN (SELECT inv.clientId,\n" +
            "                          inv.ZoneName,\n" +
            "                          inv.Branch BranchName,\n" +
            "                          COUNT(inv.invoiceNumber) InvCount,\n" +
            "                          SUM(inv.Total_Amount) TotalInvoiceAmount,\n" +
            "                          SUM(inv.Oustanding) TotalOutStanding\n" +
            "                     FROM (SELECT b.invoiceNumber,\n" +
            "                                  b.clientId,\n" +
            "                                  z.name ZoneName,\n" +
            "                                  b2.name Branch,\n" +
            "                                  DATENAME(MM, i.startDate) + '-' +\n" +
            "                                  DATENAME(YY, i.startDate) MONTH,\n" +
            "                                  c.companyName,\n" +
            "                                  SUM(b.Invoice_Amount) Total_Amount,\n" +
            "                                  SUM(b.Invoice_Amount) -\n" +
            "                                  (SUM(b.Recovery) + SUM(b.Adjustment)) Oustanding\n" +
            "                             FROM (SELECT i.invoiceNumber,\n" +
            "                                          i.clientId,\n" +
            "                                          SUM(i.totalAmount) Invoice_Amount,\n" +
            "                                          0 RECOVERY,\n" +
            "                                          0 Adjustment\n" +
            "                                     FROM Invoice AS i\n" +
            "                                    WHERE i.IsInvoiceCanceled = '0'\n" +
            "                                    GROUP BY i.invoiceNumber, i.clientId\n" +
            "\n" +
            "                                   UNION\n" +
            "\n" +
            "                                   SELECT ir.InvoiceNo,\n" +
            "                                          i.clientId,\n" +
            "                                          0 Invoice_Amount,\n" +
            "                                          SUM(ir.Amount) RECOVERY,\n" +
            "                                          0 Adjustment\n" +
            "                                     FROM InvoiceRedeem AS ir\n" +
            "                                    INNER JOIN Invoice AS i\n" +
            "                                       ON i.invoiceNumber = ir.InvoiceNo\n" +
            "                                    INNER JOIN PaymentVouchers AS pv\n" +
            "                                       ON pv.Id = ir.PaymentVoucherId\n" +
            "                                    WHERE ISNULL(pv.PaymentSourceId, 1) = '1'\n" +
            "                                      AND i.IsInvoiceCanceled = '0'\n" +
            "                                    GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "                                   UNION\n" +
            "\n" +
            "                                   SELECT ir.InvoiceNo,\n" +
            "                                          i.clientId,\n" +
            "                                          0 Invoice_Amount,\n" +
            "                                          SUM(ir.Amount) RECOVERY,\n" +
            "                                          0 Adjustment\n" +
            "                                     FROM InvoiceRedeem AS ir\n" +
            "                                    INNER JOIN Invoice AS i\n" +
            "                                       ON i.invoiceNumber = ir.InvoiceNo\n" +
            "                                    INNER JOIN PaymentVouchers AS pv\n" +
            "                                       ON pv.Id = ir.PaymentVoucherId\n" +
            "                                    INNER JOIN ChequeStatus AS cs\n" +
            "                                       ON cs.PaymentVoucherId = pv.Id\n" +
            "                                    WHERE pv.PaymentSourceId IN\n" +
            "                                          ('2', '3', '4')\n" +
            "                                      AND i.IsInvoiceCanceled = '0'\n" +
            "                                      AND cs.IsCurrentState = '1'\n" +
            "                                      AND cs.ChequeStateId IN ('1', '2')\n" +
            "                                    GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "                                   UNION\n" +
            "\n" +
            "                                   SELECT gv.InvoiceNo,\n" +
            "                                          gv.CreditClientId,\n" +
            "                                          0 Invoice_Amount,\n" +
            "                                          0 RECOVERY,\n" +
            "                                          SUM(gv.Amount) Adjustment\n" +
            "                                     FROM GeneralVoucher AS gv\n" +
            "                                    GROUP BY gv.InvoiceNo, gv.CreditClientId) b\n" +
            "                            INNER JOIN Invoice AS i\n" +
            "                               ON i.invoiceNumber = b.invoiceNumber\n" +
            "                            INNER JOIN CreditClients AS cc\n" +
            "                               ON cc.id = b.clientId\n" +
            "                            INNER JOIN Branches AS b2\n" +
            "                               ON b2.branchCode = cc.branchCode\n" +
            "                            INNER JOIN Zones z\n" +
            "                               ON z.zoneCode = cc.zoneCode\n" +
            "                            INNER JOIN Company AS c\n" +
            "                               ON c.Id = i.companyId\n" +
            "                            WHERE cc.sectorid != '0'\n" +
            "                              AND i.IsInvoiceCanceled = '0'\n" +
            "                              AND i.startDate >= '2017-06-29'\n" +
            "           " + clvar.CheckCondition + "\n" +
            "                            GROUP BY z.name,\n" +
            "                                     b.invoiceNumber,\n" +
            "                                     b.clientId,\n" +
            "                                     b2.name,\n" +
            "                                     DATENAME(MM, i.startDate) + '-' +\n" +
            "                                     DATENAME(YY, i.startDate),\n" +
            "                                     c.companyName\n" +
            "                           HAVING SUM(b.Invoice_Amount) - (SUM(b.Recovery) + SUM(b.Adjustment)) > = 1) inv\n" +
            "                    GROUP BY inv.clientId, inv.ZoneName, inv.Branch) D\n" +
            "    ON b.CreditClientID = D.clientId\n" +
            " GROUP BY B.ORGZONE,\n" +
            "          b.BranchName,\n" +
            "          D.InvCount,\n" +
            "          d.TotalInvoiceAmount,\n" +
            "          d.TotalOutStanding,\n" +
            "          B.CreditClientID,\n" +
            "          B.ACCOUNTNO,\n" +
            "          B.CUSTOMERNAME,\n" +
            "          B.BenBank,\n" +
            "          B.BenBankCode,\n" +
            "          B.BenName,\n" +
            "          B.BenAccNo";




            sqlString =
            "SELECT z.name ZoneName,\n" +
            "       b.name BranchName,\n" +
            "       cmb.creditClientId,\n" +
            "       cmb.accountNo AccNo,\n" +
            "       cmb.name AccName,\n" +
            "       cmb.BeneficiaryName BenName,\n" +
            "       cmb.BeneficiaryBankAccNo BenAccNo,\n" +
            "       cmb.BenefeciaryBankName BenBank,\n" +
            "       cmb.beneficiaryBankCode BenBankCode,\n" +
            "       SUM(cmb.CNCount) CnCount,\n" +
            "       SUM(cmb.DeliveredCount) DeliveredCount,\n" +
            "       SUM(cmb.RRCount) RRCount,\n" +
            "       SUM(cmb.INVCount) InvCount,\n" +
            "       SUM(cmb.RRAmount) RRAmt,\n" +
            "       SUM(cmb.AvailableAmount) AvailableAmt,\n" +
            "       SUM(cmb.CODAmount) CODAmt,\n" +
            "       SUM(cmb.INVAmount) InvoiceAmt,\n" +
            "       SUM(cmb.OutStandingAmount) OutstandingAMT,\n" +
            "       (SUM(cmb.AvailableAmount) - SUM(cmb.OutStandingAmount)) NetPayable\n" +
            "  FROM (SELECT cc.zoneCode,\n" +
            "               cc.branchCode,\n" +
            "               c.creditClientId,\n" +
            "               cc.accountNo,\n" +
            "               cc.name,\n" +
            "               cc.BeneficiaryName,\n" +
            "               cc.BeneficiaryBankAccNo,\n" +
            "               b.Name BenefeciaryBankName,\n" +
            "               b.SBPCode beneficiaryBankCode,\n" +
            "               COUNT(c.consignmentNumber) CNCount,\n" +
            "               COUNT(rc3.consignmentNumber) DeliveredCount,\n" +
            "               COUNT(pv.ConsignmentNo) RRCount,\n" +
            "               0 INVCount,\n" +
            "               SUM(pv.Amount) RRAmount,\n" +
            "               SUM(ISNULL(pv2.Amount, 0) - ISNULL(pv2.AmountUsed, 0)) AvailableAmount,\n" +
            "               SUM(cdn.codAmount) CODAmount,\n" +
            "               0 INVAmount,\n" +
            "               0 OutStandingAmount\n" +
            "          FROM Consignment c\n" +
            "         INNER JOIN CODConsignmentDetail_New cdn\n" +
            "            ON cdn.consignmentNumber = c.consignmentNumber\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON cc.id = c.creditClientId\n" +
            "          LEFT OUTER JOIN RunsheetConsignment rc\n" +
            "            ON rc.consignmentNumber = c.consignmentNumber\n" +
            "           AND rc.createdOn =\n" +
            "               (SELECT MAX(RC2.createdOn)\n" +
            "                  FROM RunsheetConsignment RC2\n" +
            "                 WHERE RC2.consignmentNumber = c.consignmentNumber)\n" +
            "          LEFT OUTER JOIN RunsheetConsignment rc3\n" +
            "            ON rc3.consignmentNumber = c.consignmentNumber\n" +
            "           AND rc3.createdOn =\n" +
            "               (SELECT MAX(RC4.createdOn)\n" +
            "                  FROM RunsheetConsignment RC4\n" +
            "                 WHERE RC4.consignmentNumber = c.consignmentNumber)\n" +
            "           AND rc3.Status = '55'\n" +
            "          LEFT OUTER JOIN PaymentVouchers pv\n" +
            "            ON pv.ConsignmentNo = c.consignmentNumber\n" +
            "          LEFT OUTER JOIN PaymentVouchers pv2\n" +
            "            ON pv2.ConsignmentNo = c.consignmentNumber\n" +
            "           AND pv2.ConsignmentNo = rc3.consignmentNumber\n" +
            "          LEFT OUTER JOIN Banks b\n" +
            "            ON b.Id = cc.BeneficiaryBankCode\n" +
            "           AND b.isMNPBank = '0'\n" +
            "         WHERE C.COD = '1'\n" +
            "           AND cc.CODType != '2'\n" +
            "           AND cc.IsCOD = '1'\n" +
            "           AND c.isapproved = '1'\n" +
            "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
            "           AND c.ispayable = '0'\n" +
            "           " + clvar.CheckCondition + "\n" +
            "         GROUP BY cc.zoneCode,\n" +
            "                  cc.branchCode,\n" +
            "                  c.creditClientId,\n" +
            "                  cc.name,\n" +
            "                  cc.accountNo,\n" +
            "                  cc.name,\n" +
            "                  cc.BeneficiaryName,\n" +
            "                  cc.BeneficiaryBankAccNo,\n" +
            "                  b.Name,\n" +
            "                  b.SBPCode\n" +
            "\n" +
            "        UNION ALL\n" +
            "\n" +
            "        SELECT INV.zoneCode,\n" +
            "               INV.branchCode,\n" +
            "               INV.creditClientId,\n" +
            "               INV.accountNo,\n" +
            "               INV.name,\n" +
            "               INV.BeneficiaryName,\n" +
            "               INV.BeneficiaryBankAccNo,\n" +
            "               INV.BenefeciaryBankName,\n" +
            "               INV.beneficiaryBankCode,\n" +
            "               0 CNCount,\n" +
            "               0 DeliveredCount,\n" +
            "               0 RRcount,\n" +
            "               COUNT(INV.invoiceNumber) INVCount,\n" +
            "               0 RRAmount,\n" +
            "               0 AvailableAmount,\n" +
            "               0 CODAmount,\n" +
            "               SUM(ISNULL(INV.Total_Amount, 0)) INVAmount,\n" +
            "               SUM(ISNULL(INV.Oustanding, 0)) OutstandingAmount\n" +
            "          FROM (SELECT cc.zoneCode,\n" +
            "                       cc.branchCode,\n" +
            "                       cc.id creditClientId,\n" +
            "                       cc.accountNo,\n" +
            "                       cc.name,\n" +
            "                       cc.BeneficiaryName,\n" +
            "                       cc.BeneficiaryBankAccNo,\n" +
            "                       b.invoiceNumber,\n" +
            "                       b3.Name BenefeciaryBankName,\n" +
            "                       b3.SBPCode beneficiaryBankCode,\n" +
            "                       SUM(b.Invoice_Amount) Total_Amount,\n" +
            "                       SUM(b.Invoice_Amount) -\n" +
            "                       (SUM(b.Recovery) + SUM(b.Adjustment)) Oustanding\n" +
            "                  FROM (SELECT i.invoiceNumber,\n" +
            "                               i.clientId,\n" +
            "                               SUM(i.totalAmount) Invoice_Amount,\n" +
            "                               0 RECOVERY,\n" +
            "                               0 Adjustment\n" +
            "                          FROM Invoice AS i\n" +
            "                         WHERE i.IsInvoiceCanceled = '0'\n" +
            "                         GROUP BY i.invoiceNumber, i.clientId\n" +
            "\n" +
            "                        UNION\n" +
            "\n" +
            "                        SELECT ir.InvoiceNo,\n" +
            "                               i.clientId,\n" +
            "                               0 Invoice_Amount,\n" +
            "                               SUM(ir.Amount) RECOVERY,\n" +
            "                               0 Adjustment\n" +
            "                          FROM InvoiceRedeem AS ir\n" +
            "                         INNER JOIN Invoice AS i\n" +
            "                            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "                         INNER JOIN PaymentVouchers AS pv\n" +
            "                            ON pv.Id = ir.PaymentVoucherId\n" +
            "                         WHERE ISNULL(pv.PaymentSourceId, 1) IN ('1','8')\n" +
            "                           AND i.IsInvoiceCanceled = '0'\n" +
            "                         GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "                        UNION\n" +
            "\n" +
            "                        SELECT ir.InvoiceNo,\n" +
            "                               i.clientId,\n" +
            "                               0 Invoice_Amount,\n" +
            "                               SUM(ir.Amount) RECOVERY,\n" +
            "                               0 Adjustment\n" +
            "                          FROM InvoiceRedeem AS ir\n" +
            "                         INNER JOIN Invoice AS i\n" +
            "                            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "                         INNER JOIN PaymentVouchers AS pv\n" +
            "                            ON pv.Id = ir.PaymentVoucherId\n" +
            "                         INNER JOIN ChequeStatus AS cs\n" +
            "                            ON cs.PaymentVoucherId = pv.Id\n" +
            "                         WHERE pv.PaymentSourceId IN ('2', '3', '4')\n" +
            "                           AND i.IsInvoiceCanceled = '0'\n" +
            "                           AND cs.IsCurrentState = '1'\n" +
            "                           AND cs.ChequeStateId IN ('1', '2')\n" +
            "                         GROUP BY ir.InvoiceNo, i.clientId\n" +
            "\n" +
            "                        UNION\n" +
            "\n" +
            "                        SELECT gv.InvoiceNo,\n" +
            "                               gv.CreditClientId,\n" +
            "                               0 Invoice_Amount,\n" +
            "                               0 RECOVERY,\n" +
            "                               SUM(gv.Amount) Adjustment\n" +
            "                          FROM GeneralVoucher AS gv\n" +
            "                         GROUP BY gv.InvoiceNo, gv.CreditClientId) b\n" +
            "                 INNER JOIN Invoice AS i\n" +
            "                    ON i.invoiceNumber = b.invoiceNumber\n" +
            "                 INNER JOIN CreditClients AS cc\n" +
            "                    ON cc.id = b.clientId\n" +
            "                 INNER JOIN Branches AS b2\n" +
            "                    ON b2.branchCode = cc.branchCode\n" +
            "                  LEFT OUTER JOIN Banks b3\n" +
            "                    ON b3.Id = cc.BeneficiaryBankCode\n" +
            "                   AND b3.isMNPBank = '0'\n" +
            "                 WHERE cc.IsCOD = '1'\n" +
            "                   AND cc.CODType != '2'\n" +
            "                   AND cc.sectorid != '0'\n" +
            "                   AND i.IsInvoiceCanceled = '0'\n" +
            "                   AND i.startDate >= '2017-06-29'\n" +
            "           " + clvar.CheckCondition + "\n" +

            "                 GROUP BY b.invoiceNumber,\n" +
            "                          cc.zoneCode,\n" +
            "                          cc.branchCode,\n" +
            "                          cc.id,\n" +
            "                          cc.accountNo,\n" +
            "                          cc.name,\n" +
            "                          cc.BeneficiaryName,\n" +
            "                          cc.BeneficiaryBankAccNo,\n" +
            "                          b.invoiceNumber,\n" +
            "                          b3.Name,\n" +
            "                          b3.SBPCode\n" +
            "                HAVING SUM(b.Invoice_Amount) - (SUM(b.Recovery) + SUM(b.Adjustment)) > = 1) INV\n" +
            "         GROUP BY INV.zoneCode,\n" +
            "                  INV.branchCode,\n" +
            "                  INV.creditClientId,\n" +
            "                  INV.accountNo,\n" +
            "                  INV.name,\n" +
            "                  INV.BeneficiaryName,\n" +
            "                  INV.BeneficiaryBankAccNo,\n" +
            "                  --COUNT(INV.invoiceNumber) INVCount,\n" +
            "                  INV.BenefeciaryBankName,\n" +
            "                  INV.beneficiaryBankCode) CMB\n" +
            " INNER JOIN Zones z\n" +
            "    ON z.zoneCode = cmb.zoneCode\n" +
            " INNER JOIN Branches b\n" +
            "    ON b.branchCode = cmb.branchCode\n" +
            " GROUP BY z.name,\n" +
            "          b.name,\n" +
            "          cmb.creditClientId,\n" +
            "          cmb.accountNo,\n" +
            "          cmb.name,\n" +
            "          cmb.BeneficiaryName,\n" +
            "          cmb.BeneficiaryBankAccNo,\n" +
            "          cmb.BenefeciaryBankName,\n" +
            "          cmb.beneficiaryBankCode\n" +
            " ORDER BY cmb.accountNo";





            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.SelectCommand.CommandTimeout = 3000;
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return dt;
        }
        protected void dd_Zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            clvar.Zone = dd_Zone.SelectedValue;
            if (dd_Zone.SelectedValue == "0")
            {
                AlertMessage("Select Zone", "Red");
                return;
            }
            else
            {
                GetBranches(clvar);
            }
        }
        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            #region Validations
            if (dd_Zone.SelectedValue == "0" && chk_zone.Checked == false)
            {
                AlertMessage("Select Zone", "Red");
                return;
            }
            else if (chk_zone.Checked)
            {
                clvar.CheckCondition += "";
            }
            else if (dd_Zone.SelectedValue != "0" && chk_zone.Checked == false)
            {
                clvar.CheckCondition += " AND cc.ZoneCode = '" + dd_Zone.SelectedValue + "'\n";
                if (dd_branch.SelectedValue == "0" && chk_branch.Checked == false)
                {
                    AlertMessage("Select Branch", "Red");
                    return;
                }
                else if (chk_branch.Checked)
                {
                    clvar.CheckCondition += "";
                }
                else if (dd_branch.SelectedValue != "0" && chk_branch.Checked == false)
                {
                    clvar.CheckCondition += " AND cc.BranchCode = '" + dd_branch.SelectedValue + "'\n";
                }
            }

            if (txt_accountNumber.Text.Trim() != "")
            {
                clvar.CheckCondition += " AND CC.AccountNo = '" + txt_accountNumber.Text.Trim() + "'";
            }


            #endregion

            DataTable dt = GetClientsReport(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_clientDetails.DataSource = dt;
                    gv_clientDetails.DataBind();
                }
                else
                {
                    gv_clientDetails.DataSource = null;
                    gv_clientDetails.DataBind();
                    AlertMessage("No Data Found", "Red");
                    return;
                }
            }
        }
        protected void gv_clientDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double netPayable = 0;
                double.TryParse(e.Row.Cells[17].Text.ToString(), out netPayable);

                if (netPayable <= 0)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#FFC1CB");
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                }
            }
        }
    }
}