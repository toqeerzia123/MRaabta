using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Drawing;
using System.Web.UI.WebControls.WebParts;
using ListItem = System.Web.UI.WebControls.ListItem;
using System.Web.Services;
using System.Text.RegularExpressions;
using Ionic.Zip;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class RetailCODPayment : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        Cl_Variables clvar_ = new Cl_Variables();
        bayer_Function b_fun = new bayer_Function();
        MISReport mis = new MISReport();

        DataTable dt_ = new DataTable();

        public string CustomerFileName;
        string PaymentId_;
        int count = 0;
        Boolean flag = false;
        int page = 1;
        double CODTotalAmount = 0, DeliveredAmount, DeliveredwithRRAmount, DeliveredwithoutRRAmount;
        int Delivered, DeliveredCN, DeliveredCN2;
        string date, query, paidsts, reportid;

        protected void Page_Load(object sender, EventArgs e)
        {

            error_msg.Text = "";
            Errorid.Text = "";
            if (!IsPostBack)
            {
                Get_Zone();

                //UpdatePanel mainPanel = Page.Master.FindControl("mainPanel") as UpdatePanel;
                //UpdatePanelControlTrigger trigger = new PostBackTrigger();
                //trigger.ControlID = btn_print.UniqueID;
                //mainPanel.Triggers.Add(trigger);

                //trigger = new PostBackTrigger();
                //trigger.ControlID = Button4.UniqueID;
                //mainPanel.Triggers.Add(trigger);
                string script = String.Format("javascript:return DeleteGridView('{0}');", GridView2.ClientID);
                //txtPercentage.Attributes.Add("onkeydown", script);



            }

        }
        public void Get_CreditClient(Variable clvar)
        {
            DataSet ds_zone = Get_CreditClientAccounts(clvar);

            if (ds_zone.Tables[0].Rows.Count != 0)
            {
                dd_customer.DataTextField = "name";
                dd_customer.DataValueField = "accountno";
                dd_customer.DataSource = ds_zone.Tables[0].DefaultView;
                dd_customer.DataBind();
            }
        }
        public DataSet Get_CODPaymentReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {


                #region MyRegion
                //string sqlString = "select ROW_NUMBER()OVER(Order by b.CONSIGNMENTNUMBER) SR, b.CONSIGNMENTNUMBER, b.BOOKINGDATE, b.RRSTATUS, b.PaymentVoucherID, b.CODAMOUNT, b.ORGZONE, b.ORGZONECODE, b.accountNo, b.CUSTOMERNAME, b.DELIVEREDCN, b.PaidStatus  from (  \n" +
                //       "----- COD PAYMENT REPORTS\n" +
                //       " --" + HttpContext.Current.Session["U_NAME"].ToString() + "\n" +
                //       "      SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //       "       CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE ,\n" +
                //       "       l.AttributeValue RRSTATUS, \n" +
                //       "       isnull(PV.ReceiptNo,'-') PaymentVoucherID,\n" +
                //       "       CD.CODAMOUNT CODAMOUNT,\n" +
                //       "       Z.NAME ORGZONE,\n" +
                //       "       Z.ZONECODE ORGZONECODE,\n" +
                //       "       CC.ACCOUNTNO,\n" +
                //       "       CC.NAME CUSTOMERNAME,\n" +
                //       "       COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN, \n" +
                //       "       case when c.isPayable = '1' then 'Paid' else 'UnPaid' end PaidStatus, \n" +
                //       "       i.invoiceNumber \n" +
                //       "  FROM CONSIGNMENT C\n" +
                //       " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                //       "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "  LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //       "    ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "   AND  R.createdOn =\n" +
                //       "   (select MAX(createdOn)\n" +
                //       "   from RunsheetConsignment rc1\n" +
                //       "   where rc1.consignmentNumber = R.consignmentNumber) \n" +
                //       "  LEFT JOIN PaymentVouchers pv \n " +
                //       "    ON pv.ConsignmentNo = c.consignmentNumber \n" +
                //       " INNER JOIN CREDITCLIENTS CC\n" +
                //       "    ON CC.ID = C.CREDITCLIENTID\n" +
                //       " INNER JOIN BRANCHES B2\n" +
                //       "    ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z\n" +
                //       "    ON Z.ZONECODE = B2.ZONECODE\n" +
                //       " INNER JOIN BRANCHES B3\n" +
                //       "    ON C.DESTINATION = B3.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z2\n" +
                //       "    ON B3.ZONECODE = Z2.ZONECODE\n" +
                //    //" LEFT join InvoiceConsignment ic \n" +
                //    //" on c.consignmentNumber = ic.consignmentNumber\n" +
                //    //" left OUTER join Invoice i\n" +
                //    //" on ic.invoiceNumber = i.invoiceNumber\n" +
                //       "LEFT OUTER JOIN rvdbo.Lookup L \n" +
                //        " ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //       "  WHERE C.COD = '1'\n" +
                //       "  AND (C.status <> '9' or C.status is null)\n" +
                //       "    AND isnull(i.IsInvoiceCanceled,0) != '1' \n " +
                //       "   AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //     clvar.StartDate + "\n" +
                //       "GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //        "         CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //        "         l.AttributeValue, \n" +
                //        "         CD.CODAMOUNT,\n" +
                //        "         Z.NAME,\n" +
                //        "         Z.ZONECODE,\n" +
                //        "         CC.ACCOUNTNO,\n" +
                //        "         CC.NAME, PV.ReceiptNo, i.invoiceNumber, c.isPayable \n" +
                //        "UNION ALL \n" +
                //        "      SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //       "       CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE ,\n" +
                //       "       l.AttributeValue RRSTATUS, \n" +
                //       "       isnull(PV.ReceiptNo,'-') PaymentVoucherID,\n" +
                //       "       CD.CODAMOUNT CODAMOUNT,\n" +
                //       "       Z.NAME ORGZONE,\n" +
                //       "       Z.ZONECODE ORGZONECODE,\n" +
                //       "       CC.ACCOUNTNO,\n" +
                //       "       CC.NAME CUSTOMERNAME,\n" +
                //       "       COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN, \n" +
                //       "       case when c.isPayable = '1' then 'Paid' else 'UnPaid' end PaidStatus, \n" +
                //       "       i.invoiceNumber \n" +
                //       "  FROM CONSIGNMENT C\n" +
                //       " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                //       "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "  LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //       "    ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //       "   AND  R.createdOn =\n" +
                //       "   (select MAX(createdOn)\n" +
                //       "   from RunsheetConsignment rc1\n" +
                //       "   where rc1.consignmentNumber = R.consignmentNumber) \n" +
                //       "  LEFT JOIN PaymentVouchers pv \n " +
                //       "    ON pv.ConsignmentNo = c.consignmentNumber \n" +
                //       " INNER JOIN CREDITCLIENTS CC\n" +
                //       "    ON CC.ID = C.CREDITCLIENTID\n" +
                //       " INNER JOIN BRANCHES B2\n" +
                //       "    ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z\n" +
                //       "    ON Z.ZONECODE = B2.ZONECODE\n" +
                //       " INNER JOIN BRANCHES B3\n" +
                //       "    ON C.DESTINATION = B3.BRANCHCODE\n" +
                //       " INNER JOIN ZONES Z2\n" +
                //       "    ON B3.ZONECODE = Z2.ZONECODE\n" +
                //    //" LEFT join InvoiceConsignment ic \n" +
                //    //" on c.consignmentNumber = ic.consignmentNumber\n" +
                //    //" left OUTER join Invoice i\n" +
                //    //" on ic.invoiceNumber = i.invoiceNumber\n" +
                //       "LEFT OUTER JOIN rvdbo.Lookup L \n" +
                //        " ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //       "  WHERE C.COD = '1'\n" +
                //       "  AND (C.status <> '9' or C.status is null)\n" +
                //       "    AND isnull(i.IsInvoiceCanceled,0) != '1' \n " +
                //       "   AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //     clvar.StartDate + "\n" +
                //       "GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //        "         CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //        "         l.AttributeValue, \n" +
                //        "         CD.CODAMOUNT,\n" +
                //        "         Z.NAME,\n" +
                //        "         Z.ZONECODE,\n" +
                //        "         CC.ACCOUNTNO,\n" +
                //        "         CC.NAME, PV.ReceiptNo, i.invoiceNumber, c.isPayable  \n" +
                //        ") b \n" +
                //        "   where " + clvar.Seal + " \n" +
                //        "ORDER BY 1"; 

                //  string sqlString = "select ROW_NUMBER() OVER(Order by b.CONSIGNMENTNUMBER) SR,\n" +
                //"       b.CONSIGNMENTNUMBER,\n" +
                //"       b.BOOKINGDATE,\n" +
                //"       b.RRSTATUS,\n" +
                //"       b.PaymentVoucherID,\n" +
                //"       b.CODAMOUNT,\n" +
                //"       b.ORGZONE,\n" +
                //"       b.ORGZONECODE,\n" +
                //"       b.accountNo,\n" +
                //"       b.CUSTOMERNAME,\n" +
                //"       b.DELIVEREDCN,\n" +
                //"       b.PaidStatus, b.ChequeNo\n" +
                //"  from (\n" +
                //"        ----- COD PAYMENT REPORTS\n" +
                //"        --khi.acc@ocs.com.pk\n" +
                //"        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //"                CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                //"                l.AttributeValue RRSTATUS,\n" +
                //"                isnull(PV.ReceiptNo, '-') PaymentVoucherID,\n" +
                //"                CD.CODAMOUNT CODAMOUNT,\n" +
                //"                Z.NAME ORGZONE,\n" +
                //"                Z.ZONECODE ORGZONECODE,\n" +
                //"                CC.ACCOUNTNO,\n" +
                //"                CC.NAME CUSTOMERNAME,\n" +
                //"                COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                //"                case\n" +
                //"                  when c.isPayable = '1' then\n" +
                //"                   'Paid'\n" +
                //"                  else\n" +
                //"                   'UnPaid'\n" +
                //"                end PaidStatus, C.transactionNumber ChequeNo\n" +
                //"          FROM CONSIGNMENT C\n" +
                //"         INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
                //"            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //"            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"           AND R.createdOn =\n" +
                //"               (select MAX(createdOn)\n" +
                //"                  from RunsheetConsignment rc1\n" +
                //"                 where rc1.consignmentNumber = R.consignmentNumber)\n" +
                //"          LEFT JOIN PaymentVouchers pv\n" +
                //"            ON pv.ConsignmentNo = c.consignmentNumber\n" +
                //"         INNER JOIN CREDITCLIENTS CC\n" +
                //"            ON CC.ID = C.CREDITCLIENTID\n" +
                //"         INNER JOIN BRANCHES B2\n" +
                //"            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z\n" +
                //"            ON Z.ZONECODE = B2.ZONECODE\n" +
                //"         INNER JOIN BRANCHES B3\n" +
                //"            ON C.DESTINATION = B3.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z2\n" +
                //"            ON B3.ZONECODE = Z2.ZONECODE\n" +
                //"          LEFT OUTER JOIN rvdbo.Lookup L\n" +
                //"            ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //"         WHERE C.COD = '1'\n" +
                //"           AND (C.status <> '9' or C.status is null)\n" +
                //"   AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //clvar.StartDate + "\n" +
                //"         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //"                   CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //"                   l.AttributeValue,\n" +
                //"                   CD.CODAMOUNT,\n" +
                //"                   Z.NAME,\n" +
                //"                   Z.ZONECODE,\n" +
                //"                   CC.ACCOUNTNO,\n" +
                //"                   CC.NAME,\n" +
                //"                   PV.ReceiptNo,\n" +
                //"                   c.isPayable, C.transactionNumber\n" +
                //"        UNION ALL\n" +
                //"        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                //"               CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                //"               l.AttributeValue RRSTATUS,\n" +
                //"               isnull(PV.ReceiptNo, '-') PaymentVoucherID,\n" +
                //"               CD.CODAMOUNT CODAMOUNT,\n" +
                //"               Z.NAME ORGZONE,\n" +
                //"               Z.ZONECODE ORGZONECODE,\n" +
                //"               CC.ACCOUNTNO,\n" +
                //"               CC.NAME CUSTOMERNAME,\n" +
                //"               COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                //"               case\n" +
                //"                 when c.isPayable = '1' then\n" +
                //"                  'Paid'\n" +
                //"                 else\n" +
                //"                  'UnPaid'\n" +
                //"               end PaidStatus, C.transactionNumber ChequeNo\n" +
                //"          FROM CONSIGNMENT C\n" +
                //"         INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
                //"            ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"          LEFT JOIN RUNSHEETCONSIGNMENT R\n" +
                //"            ON R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n" +
                //"           AND R.createdOn =\n" +
                //"               (select MAX(createdOn)\n" +
                //"                  from RunsheetConsignment rc1\n" +
                //"                 where rc1.consignmentNumber = R.consignmentNumber)\n" +
                //"          LEFT JOIN PaymentVouchers pv\n" +
                //"            ON pv.ConsignmentNo = c.consignmentNumber\n" +
                //"         INNER JOIN CREDITCLIENTS CC\n" +
                //"            ON CC.ID = C.CREDITCLIENTID\n" +
                //"         INNER JOIN BRANCHES B2\n" +
                //"            ON CC.BRANCHCODE = B2.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z\n" +
                //"            ON Z.ZONECODE = B2.ZONECODE\n" +
                //"         INNER JOIN BRANCHES B3\n" +
                //"            ON C.DESTINATION = B3.BRANCHCODE\n" +
                //"         INNER JOIN ZONES Z2\n" +
                //"            ON B3.ZONECODE = Z2.ZONECODE\n" +
                //"          LEFT OUTER JOIN rvdbo.Lookup L\n" +
                //"            ON CAST(R.Reason AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                //"         WHERE C.COD = '1'\n" +
                //"           AND (C.status <> '9' or C.status is null)\n" +
                //"           AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
                //clvar.StartDate + "\n" +
                //"         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                //"                  CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                //"                  l.AttributeValue,\n" +
                //"                  CD.CODAMOUNT,\n" +
                //"                  Z.NAME,\n" +
                //"                  Z.ZONECODE,\n" +
                //"                  CC.ACCOUNTNO,\n" +
                //"                  CC.NAME,\n" +
                //"                  PV.ReceiptNo,\n" +
                //"                  c.isPayable, C.transactionNumber) b\n" +
                //" where " + clvar.Seal + " \n" +
                //" ORDER BY 1";
                #endregion





                string sqlString = "SELECT ROW_NUMBER() OVER(ORDER BY B.CONSIGNMENTNUMBER) SR,\n" +
                "       B.CONSIGNMENTNUMBER,\n" +
                "       B.BOOKINGDATE,\n" +
                "       B.RRSTATUS,\n" +
                "       B.PAYMENTVOUCHERID, B.VoucherID, CAST(B.VOUCHERDATE as VARCHAR) VOUCHERDATE,B.COLLECTIONBR,\n" +
                "       B.CODAMOUNT,\n" +
                "       B.AvailableAmount,\n" +
                "       B.ORGZONE,\n" +
                "       B.ORGZONECODE,\n" +
                "       B.ACCOUNTNO,\n" +
                "       B.CUSTOMERNAME,\n" +
                "       B.DELIVEREDCN,\n" +
                "       B.PAIDSTATUS,\n" +
                "       B.CHEQUENO, B.STATUS DeliveryStatus\n" +
                "  FROM (\n" +
                "        ----- COD PAYMENT REPORTS\n" +
                "        --KHI.ACC@OCS.COM.PK\n" +
                "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                "                CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                "                L.ATTRIBUTEVALUE RRSTATUS,\n" +
                "                ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, pv.id VoucherID, PV.VOUCHERDATE,B4.SNAME COLLECTIONBR,\n" +
                "                CD.CODAMOUNT CODAMOUNT,\n" +
                "                (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
                "                Z.NAME ORGZONE,\n" +
                "                Z.ZONECODE ORGZONECODE,\n" +
                "                CC.ACCOUNTNO,\n" +
                "                CC.NAME CUSTOMERNAME,\n" +
                "                COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                "                CASE\n" +
                "                  WHEN C.ISPAYABLE = '1' THEN\n" +
                "                   'PAID'\n" +
                "                  ELSE\n" +
                "                   'UNPAID'\n" +
                "                END PAIDSTATUS,\n" +
                "                C.TRANSACTIONNUMBER CHEQUENO, R.STATUS\n" +
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
                "		     ON B4.branchCode = PV.BranchCode" +
                "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
                "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                "         WHERE C.COD = '1' and c.isapproved = '1'\n" +
                "           --AND C.transactionNumber IS NULL\n" +
                "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
                "           AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
               clvar.StartDate + "\n" +
                "\n" +
                "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                "                   CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                "                   L.ATTRIBUTEVALUE,\n" +
                "                   CD.CODAMOUNT, \n" +
                "                   Z.NAME,\n" +
                "                   Z.ZONECODE,\n" +
                "                   CC.ACCOUNTNO,\n" +
                "                   CC.NAME,\n" +
                "                   PV.RECEIPTNO, pv.id,\n" +
                "                   C.ISPAYABLE,\n" +
                "                   C.TRANSACTIONNUMBER,\n" +
                "                   PV.VOUCHERDATE,B4.SNAME, PV.Amount, PV.Amountused, R.STATUS\n" +
                "        UNION ALL\n" +
                "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                "               CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                "               L.ATTRIBUTEVALUE RRSTATUS,\n" +
                "               ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, pv.ID VoucherID, PV.VOUCHERDATE,B4.SNAME COLLECTIONBR,\n" +
                "               CD.CODAMOUNT CODAMOUNT,\n" +
                "               (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
                "               Z.NAME ORGZONE,\n" +
                "               Z.ZONECODE ORGZONECODE,\n" +
                "               CC.ACCOUNTNO,\n" +
                "               CC.NAME CUSTOMERNAME,\n" +
                "               COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                "               CASE\n" +
                "                 WHEN C.ISPAYABLE = '1' THEN\n" +
                "                  'PAID'\n" +
                "                 ELSE\n" +
                "                  'UNPAID'\n" +
                "               END PAIDSTATUS,\n" +
                "               C.TRANSACTIONNUMBER CHEQUENO, R.STATUS\n" +
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
                "		     ON B4.branchCode = PV.BranchCode" +
                "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
                "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                "         WHERE C.COD = '1' and c.isapproved = '1'\n" +
                "           --AND C.transactionNumber IS NULL\n" +
                "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
                "           AND CC.ACCOUNTNO = '" + clvar.ACNumber + "'\n" +
               clvar.StartDate + "\n" +
                "\n" +
                "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                "                  CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                "                  L.ATTRIBUTEVALUE,\n" +
                "                  CD.CODAMOUNT,\n" +
                "                  Z.NAME,\n" +
                "                  Z.ZONECODE,\n" +
                "                  CC.ACCOUNTNO,\n" +
                "                  CC.NAME,\n" +
                "                  PV.RECEIPTNO, pv.id,\n" +
                "                  C.ISPAYABLE,\n" +
                "                  C.TRANSACTIONNUMBER,\n" +
                "                  PV.VOUCHERDATE,B4.SNAME, PV.amount, PV.AmountUsed, R.STATUS) B\n" +
                " where " + clvar.Seal + " \n" +
                " ORDER BY 1";



                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 3000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        #region PDF WALA KAAM
        //public void ExporttopdfSummary(string origin, string date, string account, string customer, double sbqty, double sbamt, double sdqty, double sdamt, double drrqty, double drramt, double dwrrqty, double dwrramt, double totalwrr)
        //{
        //    Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
        //    iTextSharp.text.Font NormalFont = FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL); //FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        //    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //    {
        //        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
        //        Phrase phrase = null;
        //        PdfPCell cell = null;
        //        PdfPTable table_up = null;
        //        PdfPTable table = null;
        //        PdfPTable table_down = null;
        //        PdfPTable table_pageNum = null;
        //        //iTextSharp.text.BaseColor color = null;

        //        document.Open();

        //        table_up = new PdfPTable(2);
        //        table_up.TotalWidth = 550f;
        //        table_up.LockedWidth = true;
        //        //table.SetWidths(new float[] { 0.7f, 0.7f, 0.7f, 0.7f });             

        //        cell = PhraseCell(new Phrase("Customer's Payment Summary", FontFactory.GetFont("Courier New", 16, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 2;
        //        cell.PaddingBottom = 20f;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Origin:            " + origin, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Date:            " + date, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Customer:        " + customer, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorBottom = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Account #:     " + account, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        //cell.BorderColorBottom = BaseColor.BLACK;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_up.AddCell(cell);


        //        //Center table

        //        int c = 1;

        //        if (type.SelectedValue == "html")
        //        {
        //            clvar = new Variable();
        //            string[] split_number = dd_customer.SelectedValue.Split('_');
        //            clvar.ACNumber = split_number[0];
        //            if (paidstatus.Items.FindByValue("PAID").Selected == true)
        //            {
        //                #region Marking Paid
        //                cnNumber.Style.Add("display", "none");
        //                ChequeCriteria.Style.Add("display", "none");
        //                ChequeNo.Style.Add("display", "none");
        //                paidsts = "b.PaidStatus = 'UNPAID'";

        //                if (split_number[1] == "OLD")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (split_number[1] == "NEW")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }
        //                #endregion
        //            }
        //            else if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //            {
        //                #region Marking Unpaid and Editing
        //                cnNumber.Style.Add("display", "block");
        //                ChequeCriteria.Style.Add("display", "none");
        //                ChequeNo.Style.Add("display", "block");
        //                if (txt_cnNumber.Text.Trim() == "")
        //                {
        //                    Alert("Enter Consignment Number", "Red");
        //                    return;
        //                }

        //                paidsts = "b.PaidStatus = 'PAID'";

        //                if (split_number[1] == "OLD")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (split_number[1] == "NEW")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }
        //                paidsts += " AND b.ConsignmentNumber = '" + txt_cnNumber.Text + "'";
        //                #endregion
        //            }
        //            else if (paidstatus.Items.FindByValue("PRINT").Selected == true)
        //            {
        //                #region Printing
        //                paidsts = "b.PaidStatus = 'PAID'";
        //                cnNumber.Style.Add("display", "none");
        //                ChequeCriteria.Style.Add("display", "block");

        //                if (split_number[1] == "OLD")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (split_number[1] == "NEW")
        //                {
        //                    query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                            "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //                }

        //                if (rbtn_ChequeCriteria.SelectedValue == "0")
        //                {
        //                    ChequeNo.Style.Add("display", "none");
        //                    paidsts += " and ( b.ChequeNo is NULL OR b.ChequeNo = '')";
        //                }
        //                else
        //                {
        //                    ChequeNo.Style.Add("display", "block");
        //                    if (txt_chequeNo.Text.Trim() == "")
        //                    {
        //                        Alert("Enter Cheque Number", "Red");
        //                        return;
        //                    }
        //                    paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
        //                }
        //                #endregion
        //            }


        //            //if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //            //{
        //            //    paidsts = "b.PaidStatus = 'UNPAID'";
        //            //}



        //            //clvar.StartDate = date;
        //            clvar.Status = query;
        //            clvar.Seal = paidsts;

        //            if (clvar.StartDate != null && clvar.Status != "" || 1==1)
        //            {
        //                DataSet header = Get_CODPaymentReport(clvar);//------------------------------------
        //                header.Tables[0].Columns.Add("Sr.", typeof(int));
        //                GridView1.DataSource = header;
        //                GridView1.DataBind();
        //                int colcount = GridView1.Rows[0].Cells.Count;


        //                table = new PdfPTable(colcount);
        //                table.TotalWidth = 550f;

        //                table.SetWidths(new float[] { 35f, 75f, 70f, 80f, 70f, 60f, 60f, 80f });
        //                table.LockedWidth = true;

        //                for (int i = 0; i < colcount; i++)
        //                {
        //                    cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[i].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                    cell.Colspan = 1;
        //                    //cell.BorderColor = BaseColor.BLACK;
        //                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                    table.AddCell(cell);
        //                }
        //                int sr = 1;
        //                int z = 1;
        //                foreach (GridViewRow row in GridView1.Rows)
        //                {
        //                    cell = PhraseCell(new Phrase(sr.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                    cell.Colspan = 1;
        //                    //cell.BorderColor = BaseColor.BLACK;

        //                    table.AddCell(cell);
        //                    for (int i = 0; i < colcount; i++)
        //                    {
        //                        if (i == 0)
        //                        {

        //                        }
        //                        else
        //                        {
        //                            if (row.Cells[i].Text == "&nbsp;" || row.Cells[i].Text == null || row.Cells[i].Text == "" || String.IsNullOrWhiteSpace(row.Cells[i].Text))
        //                            {
        //                                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                            }
        //                            else
        //                            {
        //                                cell = PhraseCell(new Phrase(row.Cells[i].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);

        //                            }

        //                            cell.Colspan = 1;
        //                            //cell.BorderColor = BaseColor.BLACK;

        //                            table.AddCell(cell);
        //                        }
        //                    }
        //                    z++;
        //                    sr++;
        //                    if (c == 1 && z == 73)
        //                    {
        //                        for (int a = 1; a < 17; a++)//z is row number
        //                        {
        //                            string pageNum;
        //                            if (a == 16) //a cell spaces that are left then pagenum will print curently 8cells to give one line gap its 16
        //                            {
        //                                pageNum = "Page Num  " + c.ToString();
        //                            }
        //                            else
        //                            {
        //                                pageNum = "";
        //                            }
        //                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_RIGHT);
        //                            cell.Colspan = 1;
        //                            cell.Padding = 5f;
        //                            table.AddCell(cell);
        //                        }
        //                        //string pageNum;
        //                        //for (int a = 0; a < 5; a++)
        //                        //{
        //                        //    if (a == 4)
        //                        //    {
        //                        //        pageNum = "Page Num  " + c.ToString();
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        pageNum = "";
        //                        //    }
        //                        //    cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_RIGHT);
        //                        //    cell.Colspan = 1;
        //                        //    table.AddCell(cell);
        //                        //}

        //                        for (int j = 0; j < colcount; j++)
        //                        {
        //                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                            cell.Colspan = 1;
        //                            //cell.BorderColor = BaseColor.BLACK;
        //                            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(cell);

        //                        }
        //                        z = 1;

        //                        c++;
        //                    }

        //                    else if (c > 1 && z == 80)
        //                    {
        //                        string pageNum;

        //                        for (int a = 1; a < 17; a++)//z is row number
        //                        {

        //                            if (a == 16)//a cell spaces that are left then pagenum will print
        //                            {
        //                                pageNum = "Page Num  " + c.ToString();
        //                            }
        //                            else
        //                            {
        //                                pageNum = "";
        //                            }
        //                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_RIGHT);
        //                            cell.Colspan = 1;
        //                            cell.Padding = 5f;
        //                            table.AddCell(cell);
        //                        }

        //                        for (int j = 0; j < colcount; j++)
        //                        {
        //                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //                            cell.Colspan = 1;
        //                            //cell.BorderColor = BaseColor.BLACK;
        //                            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //                            table.AddCell(cell);


        //                        }
        //                        //cell = PhraseCell(c, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_RIGHT);
        //                        // cell.Colspan = 1;
        //                        //cell.BorderColor = BaseColor.BLACK;

        //                        //table.AddCell(cell);

        //                        z = 1;
        //                        c++;
        //                    }
        //                }
        //            }
        //        }

        //        //Footer Table

        //        table_down = new PdfPTable(7);
        //        table_down.TotalWidth = 550f;
        //        table_down.LockedWidth = true;
        //        table_down.SpacingBefore = 30f;
        //        table_down.SetWidths(new float[] { 35f, 10f, 20f, 10f, 5f, 30f, 5f });


        //        cell = PhraseCell(new Phrase("Summary:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Qty", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Amount", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("✔", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("____________________", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorTop = BaseColor.BLACK;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Shipments Booked", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(sbqty.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(sbamt.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Head Of Operations", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);



        //        cell = PhraseCell(new Phrase("Shipments Delivered", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(Delivered.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredAmount.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("COD Buisness", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        //cell.Colspan = 1;
        //        //cell.Padding = 5f;
        //        //cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        //table_down.AddCell(cell);
        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);



        //        /////////////////////



        //        cell = PhraseCell(new Phrase("Dilevered with RR", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase(DeliveredCN.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredwithRRAmount.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 7;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        //cell.Colspan = 1;
        //        //cell.Padding = 5f;
        //        //cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        //table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Delivered without RR", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredCN2.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(DeliveredwithoutRRAmount.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("[ ]", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("____________________", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //        //cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);


        //        cell = PhraseCell(new Phrase("Total Payable(with & without RR)", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase(totalwrr.ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        cell.UseVariableBorders = true;
        //       // cell.BorderColorBottom = BaseColor.BLACK;
        //        //cell.BorderColorLeft = BaseColor.BLACK;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("Director 3PL & Special Projects", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_CENTER);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //        table_down.AddCell(cell);

        //        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL)), PdfPCell.ALIGN_LEFT);
        //        cell.Colspan = 1;
        //        cell.Padding = 5f;
        //       // cell.BorderColorBottom = BaseColor.BLACK;
        //        cell.UseVariableBorders = true;
        //       // cell.BorderColorRight = BaseColor.BLACK;
        //        table_down.AddCell(cell);


        //        table_pageNum = new PdfPTable(2);
        //        table_pageNum.TotalWidth = 550f;
        //        table_pageNum.LockedWidth = true;
        //        table_pageNum.SpacingBefore = 30f;
        //        table_pageNum.SetWidths(new float[] { 35f, 40f });


        //        for (int a = 1; a < 5; a++)
        //        {
        //            string pageNum;
        //            if (a == 4)
        //            {
        //                pageNum = "Page Num  " + c.ToString();
        //            }
        //            else
        //            {
        //                pageNum = "";
        //            }
        //            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD)), PdfPCell.ALIGN_RIGHT);
        //            cell.Colspan = 1;
        //            cell.Padding = 5f;
        //            table_pageNum.AddCell(cell);
        //        }




        //        document.Add(table_up);
        //        document.Add(table);
        //        document.Add(table_down);
        //        document.Add(table_pageNum);


        //        document.Close();
        //        byte[] bytes = memoryStream.ToArray();
        //        memoryStream.Close();
        //        Response.Clear();
        //        Response.ContentType = "application/pdf";
        //        string fileName = "ExportToPdf_" + DateTime.Now.ToShortDateString();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
        //        Response.ContentType = "application/pdf";

        //        Response.Buffer = true;
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.BinaryWrite(bytes);
        //        Response.End();
        //        Response.Close();




        //        ////using (MemoryStream ms = new MemoryStream())
        //        ////using (Document document = new Document(PageSize.A4, 25, 25, 30, 30))
        //        //using (PdfWriter writer = PdfWriter.GetInstance(document, ms))
        //        //{
        //        //    document.Open();
        //        //    document.Add(new Paragraph("Hello World"));
        //        //    document.Close();
        //        //    writer.Close();
        //        //    ms.Close();
        //        //    Response.ContentType = "pdf/application";
        //        //    Response.AddHeader("content-disposition", "attachment;filename=First_PDF_document.pdf");
        //        //    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        //        //}
        //    }
        //} 


        //private static PdfPCell PhraseCell(Phrase phrase, int align)
        //{
        //    PdfPCell cell = new PdfPCell(phrase);
        //    //cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
        //    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
        //    cell.HorizontalAlignment = align;
        //    cell.PaddingBottom = 2f;
        //    cell.PaddingTop = 0f;
        //    return cell;
        //}

        //protected void ExportToPDF(object sender, ImageClickEventArgs e)
        //{
        //    if (type.SelectedValue == "html")
        //    {
        //        clvar = new Variable();
        //        string[] split_number = dd_customer.SelectedValue.Split('_');
        //        clvar.ACNumber = split_number[0];
        //        if (paidstatus.Items.FindByValue("PAID").Selected == true)
        //        {
        //            #region Marking Paid
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "none");
        //            paidsts = "b.PaidStatus = 'UNPAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        {
        //            #region Marking Unpaid and Editing
        //            cnNumber.Style.Add("display", "block");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "block");
        //            if (txt_cnNumber.Text.Trim() == "")
        //            {
        //                Alert("Enter Consignment Number", "Red");
        //                return;
        //            }

        //            paidsts = "b.PaidStatus = 'PAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            paidsts += " AND b.ConsignmentNumber = '" + txt_cnNumber.Text + "'";
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("PRINT").Selected == true)
        //        {
        //            #region Printing
        //            paidsts = "b.PaidStatus = 'PAID'";
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "block");

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (rbtn_ChequeCriteria.SelectedValue == "0")
        //            {
        //                ChequeNo.Style.Add("display", "none");
        //                paidsts += " and ( b.ChequeNo is NULL OR b.ChequeNo = '')";
        //            }
        //            else
        //            {
        //                ChequeNo.Style.Add("display", "block");
        //                if (txt_chequeNo.Text.Trim() == "")
        //                {
        //                    Alert("Enter Cheque Number", "Red");
        //                    return;
        //                }
        //                paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
        //            }
        //            #endregion
        //        }


        //        //if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        //{
        //        //    paidsts = "b.PaidStatus = 'UNPAID'";
        //        //}



        //        clvar.StartDate = date;
        //        clvar.Status = query;
        //        clvar.Seal = paidsts;

        //        if (clvar.StartDate != null && clvar.Status != "" || 1 == 1)
        //        {
        //            DataSet header = Get_CODPaymentReport(clvar);

        //            if (header.Tables[0].Rows.Count != 0)
        //            {
        //                error_msg.Text = "";
        //                #region MyRegion


        //                Literal lt_Main = new Literal();
        //                lt_Main.Text += "</b";
        //                lt_Main.Text += "</tr></table>";
        //                lt_Main.Text += "<table class='header'>";
        //                lt_Main.Text += "<tr>"; //phela table ha jisma se variabble uthane ha or grid ke sab se uper aien
        //                lt_Main.Text += "<td width=\"14%\"><b> Orign:</b></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ORGZONE"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Date</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["BOOKING DATE"].ToString() + "</td>";

        //                //Store your database DateTime value into a variable
        //                //   DateTime dt = DateTime.ParseExact(dd_start_date.Text, "yyyyMMdd", null);
        //                //   DateTime txtMyDate = DateTime.Parse(dd_start_date.Text);


        //                //   DateTime dateValue = DateTime.ParseExact(dd_start_date.Text, "M/d/yyyy", CultureInfo.InvariantCulture);


        //                //    lt_Main.Text += "<td width=\"36%\">" + txtMyDate + " TO " + dd_end_date.Text + "</td>";
        //                lt_Main.Text += "</tr>";

        //                lt_Main.Text += "<tr>";

        //                lt_Main.Text += "<td width=\"14%\"><b> Customer:</b></td>";//phela table ha jisma se variabble uthane ha or grid ke sab se uper aien total 4 varaible hain
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Account</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ACCOUNTNO"].ToString() + "</td>";
        //                lt_Main.Text += "</tr>";


        //                myHolder1.Dispose();
        //                myHolder1.Controls.Clear();
        //                myHolder1.Controls.Add(lt_Main);
        //                lt_Main.Text += "</table>";


        //                Literal lt_Main1 = new Literal();
        //                lt_Main1.Text += "<table class='detail' cellspacing='0'>";
        //                lt_Main1.Text += "<tr>";
        //                lt_Main1.Text += "<th width=\"3%\"><b> S.NO</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b> CONSIGNMENT NUMBER</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>BOOKING DATE</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>STATUS</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>RR NUMBER</b></font></th>";
        //                lt_Main1.Text += "<th width=\"8%\"><b>COD AMOUNT</b></font></th>";
        //                lt_Main1.Text += "</tr>";

        //                for (int j = 0; j < header.Tables[0].Rows.Count; j++)
        //                {
        //                    lt_Main1.Text += "<tr>";
        //                    lt_Main1.Text += "<td>" + (j + 1).ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["CONSIGNMENT NUMBER"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["BOOKING DATE"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["RR STATUS"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["Payment Voucher ID"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + string.Format("{0:N0}", header.Tables[0].Rows[j]["COD AMOUNT"].ToString()) + "</td>";

        //                    CODTotalAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                    //total payable ammount
        //                    int i = 1;
        //                    int x = 1;
        //                    int a = 1;
        //                    //  if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "DELIVERED")
        //                    if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "D-DELIVERED")
        //                    {
        //                        Delivered += i++;
        //                        DeliveredAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                        //shipment diliver

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "1")
        //                        {
        //                            DeliveredCN += x++;
        //                            DeliveredwithRRAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                        }

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "0")
        //                        {
        //                            DeliveredCN2 += x++;
        //                            DeliveredwithoutRRAmount += double.Parse(header.Tables[0].Rows[j]["COD AMOUNT"].ToString());
        //                        }
        //                    }

        //                    lt_Main1.Text += "</tr>";
        //                }

        //                myHolder2.Dispose();
        //                myHolder2.Controls.Clear();
        //                myHolder2.Controls.Add(lt_Main1);
        //                lt_Main1.Text += "</table>";



        //                Literal lt_Main2 = new Literal();
        //                lt_Main2.Text += "<table class='summary'>";
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td width=\"20%\"><b>SUMMARY</b></td>";
        //                lt_Main2.Text += "<td width=\"5%\"><b>QTY</b></td>";
        //                lt_Main2.Text += "<td width=\"8%\"><b>AMOUNT</b></font></td>";
        //                lt_Main2.Text += "</tr>";
        //                //y
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Booked </td>";
        //                lt_Main2.Text += "<td>" + header.Tables[0].Rows.Count + "</td>"; //shipment booked
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", CODTotalAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Delivered </td>";
        //                lt_Main2.Text += "<td>" + Delivered + "</td>";
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered with RR </td>";
        //                lt_Main2.Text += "<td>" + DeliveredCN + "</td>"; //diliver rr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered without RR </td>"; //
        //                lt_Main2.Text += "<td>" + DeliveredCN2 + "</td>"; //diliver withOutrr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithoutRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td><b>Total Payable (with & without RR) </b></td>";
        //                lt_Main2.Text += "<td></td>";
        //                lt_Main2.Text += "<td><b>" + string.Format("{0:N0}", DeliveredAmount) + "</b></td>";
        //                lt_Main2.Text += "</tr>";

        //                myHolder3.Dispose();
        //                myHolder3.Controls.Clear();
        //                myHolder3.Controls.Add(lt_Main2);
        //                lt_Main2.Text += "</table>";




        //                // ye last ma 2 lines chaye hain          
        //                Literal lt_Main3 = new Literal();
        //                lt_Main3.Text += "<table class='note'>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"20%\">*Approval of Director 3PL will be required for payment of those shipments whose RR not available.</td>";
        //                lt_Main3.Text += "</tr>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"5%\">**No service charges while payment will be deducted, rather monthly invoice to Customer will be submitted for payment as per routine business.</td>";
        //                lt_Main3.Text += "</tr>";

        //                myHolder4.Dispose();
        //                myHolder4.Controls.Clear();
        //                myHolder4.Controls.Add(lt_Main3);
        //                lt_Main3.Text += "</table>";
        //                //ye necha signature wala part
        //                Literal lt_Main4 = new Literal();
        //                lt_Main4.Text += "<table class='signature'>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"20%\" style='position: relative; border-top: 3px solid rgb(0, 0, 0);'><b>Head of Operations</b></td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"5%\">COD Business</td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr><td style='height: 42px;'></td></tr>";
        //                lt_Main4.Text += "<tr><td style='border-top: 3px solid rgb(0, 0, 0);'><b>Director 3PL & Special Projects</b></td></tr>";

        //                myHolder5.Dispose();
        //                myHolder5.Controls.Clear();
        //                myHolder5.Controls.Add(lt_Main4);
        //                lt_Main4.Text += "</table>";



        //                //?????????????????? kia hua bhai is buttonh pe aie or bechma ye table aie phir ek phela uper phir table phir nechay wala
        //                ExporttopdfSummary(header.Tables[0].Rows[0]["ORGZONE"].ToString(), header.Tables[0].Rows[0]["BOOKING DATE"].ToString(), header.Tables[0].Rows[0]["ACCOUNTNO"].ToString(), header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString(), header.Tables[0].Rows.Count, CODTotalAmount, Delivered, DeliveredAmount, DeliveredCN, DeliveredwithRRAmount, DeliveredCN2, DeliveredwithoutRRAmount, DeliveredAmount);

        //                #endregion
        //            }
        //            else
        //            {
        //                error_msg.Text = "No Record Found...";
        //            }
        //        }
        //        else
        //        {
        //            error_msg.Text = "Select Filter";
        //        }



        //    }







        //}
        //public void ExportToPDF()
        //{
        //    if (type.SelectedValue == "html")
        //    {
        //        clvar = new Variable();
        //        string[] split_number = dd_customer.SelectedValue.Split('_');
        //        clvar.ACNumber = split_number[0];
        //        if (paidstatus.Items.FindByValue("PAID").Selected == true)
        //        {
        //            #region Marking Paid
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "none");
        //            paidsts = "b.PaidStatus = 'UNPAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        {
        //            #region Marking Unpaid and Editing
        //            cnNumber.Style.Add("display", "block");
        //            ChequeCriteria.Style.Add("display", "none");
        //            ChequeNo.Style.Add("display", "block");
        //            if (txt_cnNumber.Text.Trim() == "")
        //            {
        //                Alert("Enter Consignment Number", "Red");
        //                return;
        //            }

        //            paidsts = "b.PaidStatus = 'PAID'";

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }
        //            paidsts += " AND b.ConsignmentNumber = '" + txt_cnNumber.Text + "'";
        //            #endregion
        //        }
        //        else if (paidstatus.Items.FindByValue("PRINT").Selected == true)
        //        {
        //            #region Printing
        //            paidsts = "b.PaidStatus = 'PAID'";
        //            cnNumber.Style.Add("display", "none");
        //            ChequeCriteria.Style.Add("display", "block");

        //            if (split_number[1] == "OLD")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (split_number[1] == "NEW")
        //            {
        //                query = " INNER JOIN CODCONSIGNMENTDETAIL_NEW CD\n" +
        //                        "    ON CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER\n";
        //            }

        //            if (rbtn_ChequeCriteria.SelectedValue == "0")
        //            {
        //                ChequeNo.Style.Add("display", "none");
        //                paidsts += " and ( b.ChequeNo is NULL OR b.ChequeNo = '')";
        //            }
        //            else
        //            {
        //                ChequeNo.Style.Add("display", "block");
        //                if (txt_chequeNo.Text.Trim() == "")
        //                {
        //                    Alert("Enter Cheque Number", "Red");
        //                    return;
        //                }
        //                paidsts += " and b.ChequeNo = '" + txt_chequeNo.Text + "'";
        //            }
        //            #endregion
        //        }


        //        //if (paidstatus.Items.FindByValue("UNPAID").Selected == true)
        //        //{
        //        //    paidsts = "b.PaidStatus = 'UNPAID'";
        //        //}



        //        clvar.StartDate = date;
        //        clvar.Status = query;
        //        clvar.Seal = paidsts;

        //        if (clvar.StartDate != null && clvar.Status != "" || 1 == 1)
        //        {
        //            DataSet header = Get_CODPaymentReport(clvar);

        //            if (header.Tables[0].Rows.Count != 0)
        //            {
        //                error_msg.Text = "";
        //                #region MyRegion


        //                Literal lt_Main = new Literal();
        //                lt_Main.Text += "</b";
        //                lt_Main.Text += "</tr></table>";
        //                lt_Main.Text += "<table class='header'>";
        //                lt_Main.Text += "<tr>"; //phela table ha jisma se variabble uthane ha or grid ke sab se uper aien
        //                lt_Main.Text += "<td width=\"14%\"><b> Orign:</b></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ORGZONE"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Date</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["BOOKINGDATE"].ToString() + "</td>";

        //                //Store your database DateTime value into a variable
        //                //   DateTime dt = DateTime.ParseExact(dd_start_date.Text, "yyyyMMdd", null);
        //                //   DateTime txtMyDate = DateTime.Parse(dd_start_date.Text);


        //                //   DateTime dateValue = DateTime.ParseExact(dd_start_date.Text, "M/d/yyyy", CultureInfo.InvariantCulture);


        //                //    lt_Main.Text += "<td width=\"36%\">" + txtMyDate + " TO " + dd_end_date.Text + "</td>";
        //                lt_Main.Text += "</tr>";

        //                lt_Main.Text += "<tr>";

        //                lt_Main.Text += "<td width=\"14%\"><b> Customer:</b></td>";//phela table ha jisma se variabble uthane ha or grid ke sab se uper aien total 4 varaible hain
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString() + "</td>";
        //                lt_Main.Text += "<td width=\"14%\"><b>Account</b></font></td>";
        //                lt_Main.Text += "<td width=\"36%\">" + header.Tables[0].Rows[0]["ACCOUNTNO"].ToString() + "</td>";
        //                lt_Main.Text += "</tr>";


        //                myHolder1.Dispose();
        //                myHolder1.Controls.Clear();
        //                myHolder1.Controls.Add(lt_Main);
        //                lt_Main.Text += "</table>";


        //                Literal lt_Main1 = new Literal();
        //                lt_Main1.Text += "<table class='detail' cellspacing='0'>";
        //                lt_Main1.Text += "<tr>";
        //                lt_Main1.Text += "<th width=\"3%\"><b> S.NO</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b> CONSIGNMENT NUMBER</b></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>BOOKING DATE</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>STATUS</b></font></th>";
        //                lt_Main1.Text += "<th width=\"14%\"><b>RR NUMBER</b></font></th>";
        //                lt_Main1.Text += "<th width=\"8%\"><b>COD AMOUNT</b></font></th>";
        //                lt_Main1.Text += "</tr>";

        //                for (int j = 0; j < header.Tables[0].Rows.Count; j++)
        //                {
        //                    lt_Main1.Text += "<tr>";
        //                    lt_Main1.Text += "<td>" + (j + 1).ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["CONSIGNMENTNUMBER"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["BOOKINGDATE"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["RRSTATUS"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + header.Tables[0].Rows[j]["PaymentVoucherID"].ToString() + "</td>";
        //                    lt_Main1.Text += "<td>" + string.Format("{0:N0}", header.Tables[0].Rows[j]["CODAMOUNT"].ToString()) + "</td>";

        //                    CODTotalAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                    //total payable ammount
        //                    int i = 1;
        //                    int x = 1;
        //                    int a = 1;
        //                    //  if (header.Tables[0].Rows[j]["RR STATUS"].ToString() == "DELIVERED")
        //                    if (header.Tables[0].Rows[j]["RRSTATUS"].ToString() == "D-DELIVERED")
        //                    {
        //                        Delivered += i++;
        //                        DeliveredAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                        //shipment diliver

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "1")
        //                        {
        //                            DeliveredCN += x++;
        //                            DeliveredwithRRAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                        }

        //                        if (header.Tables[0].Rows[j]["DeliveredCN"].ToString() == "0")
        //                        {
        //                            DeliveredCN2 += x++;
        //                            DeliveredwithoutRRAmount += double.Parse(header.Tables[0].Rows[j]["CODAMOUNT"].ToString());
        //                        }
        //                    }

        //                    lt_Main1.Text += "</tr>";
        //                }

        //                myHolder2.Dispose();
        //                myHolder2.Controls.Clear();
        //                myHolder2.Controls.Add(lt_Main1);
        //                lt_Main1.Text += "</table>";



        //                Literal lt_Main2 = new Literal();
        //                lt_Main2.Text += "<table class='summary'>";
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td width=\"20%\"><b>SUMMARY</b></td>";
        //                lt_Main2.Text += "<td width=\"5%\"><b>QTY</b></td>";
        //                lt_Main2.Text += "<td width=\"8%\"><b>AMOUNT</b></font></td>";
        //                lt_Main2.Text += "</tr>";
        //                //y
        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Booked </td>";
        //                lt_Main2.Text += "<td>" + header.Tables[0].Rows.Count + "</td>"; //shipment booked
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", CODTotalAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td>Shipment Delivered </td>";
        //                lt_Main2.Text += "<td>" + Delivered + "</td>";
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered with RR </td>";
        //                lt_Main2.Text += "<td>" + DeliveredCN + "</td>"; //diliver rr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td style='padding: 0px 0px 0px 25px;'>Delivered without RR </td>"; //
        //                lt_Main2.Text += "<td>" + DeliveredCN2 + "</td>"; //diliver withOutrr
        //                lt_Main2.Text += "<td>" + string.Format("{0:N0}", DeliveredwithoutRRAmount) + "</td>";
        //                lt_Main2.Text += "</tr>";

        //                lt_Main2.Text += "<tr>";
        //                lt_Main2.Text += "<td><b>Total Payable (with & without RR) </b></td>";
        //                lt_Main2.Text += "<td></td>";
        //                lt_Main2.Text += "<td><b>" + string.Format("{0:N0}", DeliveredAmount) + "</b></td>";
        //                lt_Main2.Text += "</tr>";

        //                myHolder3.Dispose();
        //                myHolder3.Controls.Clear();
        //                myHolder3.Controls.Add(lt_Main2);
        //                lt_Main2.Text += "</table>";




        //                // ye last ma 2 lines chaye hain          
        //                Literal lt_Main3 = new Literal();
        //                lt_Main3.Text += "<table class='note'>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"20%\">*Approval of Director 3PL will be required for payment of those shipments whose RR not available.</td>";
        //                lt_Main3.Text += "</tr>";
        //                lt_Main3.Text += "<tr>";
        //                lt_Main3.Text += "<td width=\"5%\">**No service charges while payment will be deducted, rather monthly invoice to Customer will be submitted for payment as per routine business.</td>";
        //                lt_Main3.Text += "</tr>";

        //                myHolder4.Dispose();
        //                myHolder4.Controls.Clear();
        //                myHolder4.Controls.Add(lt_Main3);
        //                lt_Main3.Text += "</table>";
        //                //ye necha signature wala part
        //                Literal lt_Main4 = new Literal();
        //                lt_Main4.Text += "<table class='signature'>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"20%\" style='position: relative; border-top: 3px solid rgb(0, 0, 0);'><b>Head of Operations</b></td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr>";
        //                lt_Main4.Text += "<td width=\"5%\">COD Business</td>";
        //                lt_Main4.Text += "</tr>";
        //                lt_Main4.Text += "<tr><td style='height: 42px;'></td></tr>";
        //                lt_Main4.Text += "<tr><td style='border-top: 3px solid rgb(0, 0, 0);'><b>Director 3PL & Special Projects</b></td></tr>";

        //                myHolder5.Dispose();
        //                myHolder5.Controls.Clear();
        //                myHolder5.Controls.Add(lt_Main4);
        //                lt_Main4.Text += "</table>";



        //                //?????????????????? kia hua bhai is buttonh pe aie or bechma ye table aie phir ek phela uper phir table phir nechay wala
        //                ExporttopdfSummary(header.Tables[0].Rows[0]["ORGZONE"].ToString(), header.Tables[0].Rows[0]["BOOKINGDATE"].ToString(), header.Tables[0].Rows[0]["ACCOUNTNO"].ToString(), header.Tables[0].Rows[0]["CUSTOMERNAME"].ToString(), header.Tables[0].Rows.Count, CODTotalAmount, Delivered, DeliveredAmount, DeliveredCN, DeliveredwithRRAmount, DeliveredCN2, DeliveredwithoutRRAmount, DeliveredAmount);

        //                #endregion
        //            }
        //            else
        //            {
        //                GridView2.DataSource = null;
        //                GridView2.DataBind();
        //                error_msg.Text = "No Record Found...";
        //            }
        //        }
        //        else
        //        {
        //            error_msg.Text = "Select Filter";
        //        }



        //    }
        //}
        #endregion
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        /*
         protected void ExportToPDF(object sender, EventArgs e)
         {
            Response.Clear();
            Response.Buffer = true;        
            Response.AddHeader("content-disposition", "attachment;filename=CreditConsignmentSummary.pdf");
            Response.Charset = "";
            Response.ContentType = "application/pdf";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

              //  GridView.AllowPaging = false;
               // Result();
                Table_1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        */
        protected void paidstatus_chk_CheckedChanged(object sender, EventArgs e)
        {

        }
        public DataSet Get_CreditClientAccounts(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                #region sqlString___11-04-2020
                string sqlString = "\n" +
                "SELECT B.ACCOUNTNO, B.NAME\n" +
                "  FROM (SELECT CC.ACCOUNTNO + '_NEW' ACCOUNTNO,\n" +
                "               CC.NAME + ' ( ' + CC.ACCOUNTNO + ' )' NAME\n" +
                "          FROM CODUsers CC\n" +
                "         WHERE CC.STATUS = '1'\n" +
                "\n" +
                "        UNION ALL\n" +
                "\n" +
                "        SELECT CC.ACCOUNTNO + '_OLD' ACCOUNTNO,\n" +
                "               CC.NAME + ' ( ' + CC.ACCOUNTNO + ' )' NAME\n" +
                "          FROM CREDITCLIENTS CC\n" +
                "         WHERE CC.ISCOD = '1'\n" +
                "           AND CC.ACCOUNTNO NOT IN (SELECT CU.ACCOUNTNO FROM CODUsers CU WHERE CU.status = '1')) B\n" +
                " GROUP BY B.ACCOUNTNO, B.NAME\n" +
                " ORDER BY 2";

                #endregion

                string sql = "SELECT CC.ACCOUNTNO ACCOUNTNO, \n"
                           + "       CC.NAME + ' ( ' + CC.ACCOUNTNO + ' )' NAME \n"
                           + "FROM   CREDITCLIENTS CC \n"
                           + "INNER JOIN MNP_Retail_COD_Customers rc ON cc.accountNo = rc.AccountNumber \n"
                           + "WHERE  CC.ISCOD = '1' AND isnull(rc.isApproved,'0') = '1' \n" + clvar.Check_Condition1;

                if (clvar._Zone != "" && clvar._Zone != null)
                {
                    sql += "AND cc.zoneCode = '" + clvar._Zone + "' \n";
                }

                if (clvar.TownCode != "" && clvar.TownCode != null)
                {
                    sql += "AND cc.branchCode = '" + clvar.TownCode + "' \n";
                }

                sql += "Order by cc.name \n";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = e.Row.FindControl("chk_paid") as CheckBox;
                if (e.Row.Cells[4].Text.ToString() == "-" || e.Row.Cells[6].Text.ToUpper() == "PAID")
                {

                    //chk.Enabled = false;
                    if (e.Row.Cells[6].Text.ToUpper() == "PAID")
                    {
                        chk.Checked = true;
                    }
                }
                if ((paidstatus.SelectedValue == "PAID" && e.Row.Cells[4].Text.ToString() == "-"))
                {
                    chk.Enabled = false;
                    if (paidstatus.SelectedValue == "PRINT" && e.Row.Cells[4].Text.ToString() == "-")
                    {
                        chk.Checked = false;
                    }
                }

            }
        }



        protected void btn_reset_Click(object sender, EventArgs e)
        {

        }
        //public static void Alert(string message, string color)
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        //    Errorid.Text = message;
        //    Errorid.ForeColor = System.Drawing.Color.FromName(color);
        //}

        public string EditChequeNumber(string CN)
        {
            string error = "";
            string archiveString = "insert into Consignment_Archive select * from consignment c where c.consignmentnumber in (" + CN + ")";
            string updateString = "update consignment set " + clvar_.CheckCondition + " where consignmentNumber in (" + CN + ")";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            try
            {


                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = archiveString;

                int cnt = sqlcmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    trans.Rollback();
                    error = "CONSIGNMENTS COULD NOT BE UPDATED";
                    sqlcon.Close();
                    return error;
                }
                cnt = 0;
                sqlcmd.CommandText = updateString;
                cnt = sqlcmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    trans.Rollback();
                    error = "CONSIGNMENTS COULD NOT BE UPDATED";
                    sqlcon.Close();
                    return error;
                }

                trans.Commit();
                error = "OK";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                sqlcon.Close();
                error = ex.Message;
            }
            return error;
        }
        public string MarkUnPaid(Cl_Variables clvar)
        {
            string error = "";
            string archiveCommand = "insert into Consignment_Archive select * from consignment where consignmentNumber = '" + clvar.consignmentNo + "'";
            string updateCommand = "Update Consignment " + clvar.CheckCondition + ", Modifiedon = GETDATE(), ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "' where consignmentNumber = '" + clvar.consignmentNo + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            try
            {
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = archiveCommand;
                int count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    error = "Could Not Update";
                    sqlcon.Close();
                    return error;
                }
                sqlcmd.CommandText = updateCommand;
                count = 0;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    error = "Could Not Update";
                    sqlcon.Close();
                    return error;
                }
                trans.Commit();
                error = "OK";
                sqlcon.Close();
            }
            catch (Exception ex)
            { trans.Rollback(); sqlcon.Close(); error = ex.Message; }
            return error;
        }
        public string Print()
        {
            return "";
        }


        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }

        private static DataSet getData(string invoiceNumbers, string Check_Condition1)
        {
            Variable clvar = new Variable();

            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT ROW_NUMBER() OVER(ORDER BY b.consignmentNumber) Sr, \n"
                        + "       b.consignmentNumber, \n"
                        + "       b.orderRefNo, \n"
                        + "       CONVERT(VARCHAR(11), b.bookingDate, 106) bookingDate, \n"
                        + "       CASE WHEN p.VoucherDate is null then '' else convert(varchar(11),p.VoucherDate,106) end RRDATE, \n"
                        + "       b.consignee, \n"
                        + "       b.origin, \n"
                        + "       b.destination, \n"
                        + "       b.[weight], \n"
                        + "       b.CNStatus, \n"
                        + "       SUM(b.CODAMOUNT)           CODAMOUNT, \n"
                        + "       SUM(b.ShippingCharges)     ShippingCharges  \n"
                        //+ "       b.transactionNumber        transactionNumber \n"
                        + "FROM   ( \n"
                        + "           SELECT c.consignmentNumber, \n"
                        + "                  cd.orderRefNo, \n"
                        + "                  c.bookingDate, \n"
                        + "                  c.consignee, \n"
                        + "                  b.sname              origin, \n"
                        + "                  b2.sname             destination, \n"
                        + "                  c.[weight], \n"
                        + "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT, \n"
                        + "                  --cd.codAmount, \n"
                        + "                  pv.Amount            CODAMOUNT, \n"
                        + "                  l.AttributeValue     CNStatus, \n"
                        + "                  0                    ShippingCharges,\n"
                        + "                  transactionNumber \n"
                        + "           FROM   Consignment c \n"
                        + "                  INNER JOIN Branches b \n"
                        + "                       ON  b.branchCode = c.orgin \n"
                        + "                  INNER JOIN Branches b2 \n"
                        + "                       ON  b2.branchCode = c.destination \n"
                        + "                  LEFT OUTER JOIN CODConsignmentDetail_New cd \n"
                        + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
                        + "                  INNER JOIN PaymentVouchers pv \n"
                        + "                       ON  pv.ConsignmentNo = c.consignmentNumber \n"
                        + "                  LEFT OUTER JOIN RunsheetConsignment rc \n"
                        + "                       ON  rc.consignmentNumber = c.consignmentNumber \n"
                        + "                       AND rc.createdOn = ( \n"
                        + "                               SELECT MAX(RC2.createdOn) statusdate \n"
                        + "                               FROM   RunsheetConsignment RC2 \n"
                        + "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber \n"
                        + "                           ) \n"
                        + "                  LEFT OUTER JOIN rvdbo.Lookup l \n"
                        + "                       ON  l.AttributeGroup = 'POD_STATUS' \n"
                        + "                       AND CAST(l.Id AS VARCHAR) = rc.[Status] \n"
                        + "           WHERE pv.id in (" + Check_Condition1 + ") --c.transactionNumber = '999999' \n"
                        + "                                                                   --and c.consignerAccountNo = '4G7' \n"
                        + "           GROUP BY \n"
                        + "                  c.consignmentNumber, \n"
                        + "                  cd.orderRefNo, \n"
                        + "                  c.bookingDate, \n"
                        + "                  c.consignee, \n"
                        + "                  b.sname, \n"
                        + "                  b2.sname, \n"
                        + "                  c.[weight], \n"
                        + "                  pv.Amount, \n"
                        + "                  l.AttributeValue, \n"
                        + "                  transactionNumber  \n"
                        + "            \n"
                        + "            \n"
                        + "           UNION ALL \n"
                        + "           SELECT c.consignmentNumber, \n"
                        + "                  cd.orderRefNo, \n"
                        + "                  c.bookingDate, \n"
                        + "                  c.consignee, \n"
                        + "                  b.sname              origin, \n"
                        + "                  b2.sname             destination, \n"
                        + "                  c.[weight], \n"
                        + "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT, \n"
                        + "                  --cd.codAmount, \n"
                        + "                  0                    CODAMOUNT, \n"
                        + "                  l.AttributeValue     CNStatus, \n"
                        + "                  c.totalAmount        ShippingCharges,\n"
                        + "                  transactionNumber \n"
                        + "           FROM   Consignment c \n"
                        + "                  INNER JOIN Branches b \n"
                        + "                       ON  b.branchCode = c.orgin \n"
                        + "                  INNER JOIN Branches b2 \n"
                        + "                       ON  b2.branchCode = c.destination \n"
                        + "                  LEFT OUTER JOIN CODConsignmentDetail_New cd \n"
                        + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
                        + "                  INNER JOIN InvoiceConsignment ic \n"
                        + "                       ON  ic.consignmentNumber = c.consignmentNumber \n"
                        + "                  LEFT OUTER JOIN RunsheetConsignment rc \n"
                        + "                       ON  rc.consignmentNumber = c.consignmentNumber \n"
                        + "                       AND rc.createdOn = ( \n"
                        + "                               SELECT MAX(RC2.createdOn) statusdate \n"
                        + "                               FROM   RunsheetConsignment RC2 \n"
                        + "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber \n"
                        + "                           ) \n"
                        + "                  LEFT OUTER JOIN rvdbo.Lookup l \n"
                        + "                       ON  l.AttributeGroup = 'POD_STATUS' \n"
                        + "                       AND CAST(l.Id AS VARCHAR) = rc.[Status] \n"
                        + "           WHERE  ic.invoiceNumber IN (" + invoiceNumbers + ") \n"
                        + "           GROUP BY \n"
                        + "                  c.consignmentNumber, \n"
                        + "                  cd.orderRefNo, \n"
                        + "                  c.bookingDate, \n"
                        + "                  c.consignee, \n"
                        + "                  b.sname, \n"
                        + "                  b2.sname, \n"
                        + "                  c.[weight], \n"
                        + "                  l.AttributeValue, \n"
                        + "                  c.totalAmount, \n"
                        + "                  transactionNumber  \n"
                        + "       )                          b left outer join paymentvouchers p on p.consignmentNo = b.consignmentNumber \n"
                        + "GROUP BY \n"
                        + "       b.consignmentNumber, \n"
                        + "       b.orderRefNo, \n"
                        + "       CONVERT(VARCHAR(11), b.bookingDate, 106), \n"
                        + "       b.consignee, \n"
                        + "       b.origin, \n"
                        + "       b.destination, \n"
                        + "       b.CNStatus, \n"
                        + "       b.[weight], \n"
                        + "       b.transactionNumber, p.VoucherDate";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }



        // ======================= New Code =======================

        public class codPayments
        {
            public string PaymentId { get; set; }
            public string Account { get; set; }
            public string PDFURL { get; set; }
            public string Check_Condition1 { get; set; }


        }

        public class Payments
        {
            public string CustomerName { get; set; }
            public string AccountNo { get; set; }
            public double Oustanding { get; set; }
            public double CODPayable { get; set; }
            public double NetPayable { get; set; }
        }

        [WebMethod]
        public static List<Payments> GetPaymentData(string zone, string customer, string payable_amount, string codtype)
        {
            List<Payments> listPayments = new List<Payments>();
            Cl_Variables clvar_ = new Cl_Variables();
            string condtion = "", CODType = "";

            if (customer != "0")
            {
                clvar_.AccountNo = customer;
            }

            if (zone != "")
            {
                clvar_.Zone = "AND z.zoneCode = '" + zone + "'";
            }

            if (payable_amount != "")
            {
                condtion = "HAVING (SUM(round(x.CODPayable,0)) - SUM(round(x.Oustanding,0))) >= " + payable_amount + " \n";
            }

            //if (codtype == "CorporateCOD")
            //{
            //    CODType = "";
            //}

            if (codtype == "RetailCOD")
            {
                CODType = "AND cc.CODType = '3' \n";
            }

            DataTable dt = GetInvoicesForVoucherNonCentralized(clvar_, condtion, CODType);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    int index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        Payments employee = new Payments();

                        index = i;
                        employee.CustomerName = dt.Rows[index]["CustomerName"].ToString();
                        employee.AccountNo = dt.Rows[index]["AccountNo"].ToString();
                        employee.Oustanding = double.Parse(dt.Rows[index]["Oustanding"].ToString());
                        employee.CODPayable = double.Parse(dt.Rows[index]["CODPayable"].ToString());
                        employee.NetPayable = double.Parse(dt.Rows[index]["NetPayable"].ToString());
                        i++;

                        listPayments.Add(employee);
                    }
                }
            }
            else
            {

            }

            return listPayments;
        }

        [WebMethod]
        public static codPayments SaveCustomerPaymentData(string zone, string customer, string paidstatus)
        {
            codPayments Response = new codPayments();

            Cl_Variables clvar_ = new Cl_Variables();
            Variable clvar = new Variable();

            //string CustomersAccount = Regex.Replace(customer, ",+", ",").Trim(',');
            //string[] arrCount = CustomersAccount.Split(',');

            #region ProcessOfPayment

            if (paidstatus == "PAID")
            {
                #region Marking Paid

                string paidstatusQuery = "AND b.PaidStatus = 'UNPAID' AND b.CHEQUENO IS NULL";

                string accountNo = customer;

                if (accountNo == "")
                {
                    return Response;
                }

                DataSet ds = Get_CODPaymentReport_New(accountNo, paidstatusQuery, clvar);

                DataTable ConsignmentNumbers = new DataTable();
                ConsignmentNumbers.Columns.Add("ConsignmentNumber", typeof(string));
                bool doSave = false;

                string CN = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[]{
                        new DataColumn("RR"),
                        new DataColumn("Invoice"),
                        new DataColumn("Amount", typeof(double)),
                        new DataColumn("Finished")
                        });

                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        doSave = true;
                        CN += "'" + ds.Tables[0].Rows[j]["CONSIGNMENTNUMBER"].ToString() + "'";
                        ConsignmentNumbers.Rows.Add(ds.Tables[0].Rows[j]["CONSIGNMENTNUMBER"].ToString());

                        DataRow dr = dt.NewRow();
                        dr["RR"] = ds.Tables[0].Rows[j]["VoucherID"].ToString();
                        dr["Amount"] = ds.Tables[0].Rows[j]["AvailableAmount"].ToString();
                        dr["Finished"] = "0";
                        dt.Rows.Add(dr);

                        clvar.Check_Condition1 += "'" + ds.Tables[0].Rows[j]["VoucherID"].ToString() + "'";
                    }

                    CN = CN.Replace("''", "','");

                    string Check_Condition1 = clvar.Check_Condition1.Replace("''", "','");

                    if (doSave)
                    {
                        #region AutoRedeem Calculations

                        DataTable finalDt = new DataTable();
                        finalDt.Columns.AddRange(new DataColumn[] {
                            new DataColumn("VoucherID", typeof(Int64)),
                            new DataColumn("Invoice", typeof(string)),
                            new DataColumn("Amount", typeof(float))
                        });

                        DataTable creditClient = Get_CreditClientID(accountNo, clvar);
                        string CreditClientID = creditClient.Rows[0][0].ToString();

                        DataTable invoiceDetails = GetInvoicesData(CreditClientID, clvar);

                        int k = 0;
                        for (int a = 0; a < invoiceDetails.Rows.Count; a++)
                        {
                            string invoiceNumber = invoiceDetails.Rows[a]["invoiceNumber"].ToString();
                            string invoiceAmount = invoiceDetails.Rows[a]["Oustanding"].ToString();

                            float remainingIAmount = 0;
                            float remainingRRAmount = 0;

                            float.TryParse(invoiceAmount, out remainingIAmount);

                            for (int j = k; j < dt.Rows.Count; j++)
                            {
                                Int64 RR = Int64.Parse(dt.Rows[j][0].ToString());
                                string RRa = dt.Rows[j][2].ToString();


                                float.TryParse(RRa, out remainingRRAmount);
                                if (remainingIAmount == 0)
                                {
                                    break;
                                }
                                if (remainingRRAmount == 0)
                                {
                                    continue;
                                }
                                if (remainingRRAmount <= remainingIAmount)
                                {
                                    DataRow dr = finalDt.NewRow();
                                    dr[0] = RR;
                                    dr[1] = invoiceNumber;
                                    dr[2] = remainingRRAmount;
                                    finalDt.Rows.Add(dr);
                                    remainingIAmount = remainingIAmount - remainingRRAmount;
                                    dt.Rows[j][3] = "1";
                                    dt.Rows[j][2] = "0";
                                    remainingRRAmount = 0;
                                    k = k + 1;
                                }
                                else if (remainingRRAmount > remainingIAmount && remainingIAmount > 0)
                                {
                                    DataRow dr = finalDt.NewRow();
                                    dr[0] = RR;
                                    dr[1] = invoiceNumber;
                                    dr[2] = remainingIAmount;
                                    finalDt.Rows.Add(dr);
                                    dt.Rows[j][3] = "0";
                                    dt.Rows[j][2] = remainingRRAmount - remainingIAmount;
                                    remainingRRAmount = remainingRRAmount - remainingIAmount;
                                    //i--;
                                    //j--;
                                    remainingIAmount = 0;
                                }
                            }
                        }
                        #endregion

                        string[] error_ = new String[4];

                        error_ = MarkPaid(CN, finalDt, ConsignmentNumbers, clvar);

                        if (error_[0] == "OK")
                        {
                            string paymentId = error_[1];

                            ExporttopdfSummary(Check_Condition1, paymentId, zone);

                            //  string Folder = HttpContext.Current.Server.MapPath("~/CODPayment/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + zone + "/");

                            Response.PaymentId = error_[1];
                            Response.Account = customer;
                            Response.Check_Condition1 = Check_Condition1;
                            // Response.PDFURL = Folder;

                            return Response;
                        }
                        else
                        {
                            // Alert(error, "Red");
                            // return;
                        }

                    }
                }
                else
                {
                    Response.PaymentId = "Payment Can't Created";
                    Response.Account = customer;
                    Response.Check_Condition1 = "00000";
                    // Response.PDFURL = Folder;

                    return Response;
                }
                //}
                #endregion
            }
            #endregion

            return Response;
        }
        public static DataSet Get_CODPaymentReport_New(string accountNo, string paidstatus, Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sqlString = "SELECT ROW_NUMBER() OVER(ORDER BY B.CONSIGNMENTNUMBER) SR,\n" +
                "       B.CONSIGNMENTNUMBER,\n" +
                "       B.BOOKINGDATE,\n" +
                "       B.RRSTATUS,\n" +
                "       B.PAYMENTVOUCHERID, B.VoucherID, CAST(B.VOUCHERDATE as VARCHAR) VOUCHERDATE,B.COLLECTIONBR,\n" +
                "       B.CODAMOUNT,\n" +
                "       B.AvailableAmount,\n" +
                "       B.ORGZONE,\n" +
                "       B.ORGZONECODE,\n" +
                "       B.ACCOUNTNO,\n" +
                "       B.CUSTOMERNAME,\n" +
                "       B.DELIVEREDCN,\n" +
                "       B.PAIDSTATUS,\n" +
                "       B.CHEQUENO, B.STATUS DeliveryStatus\n" +
                "  FROM (\n" +
                "        ----- COD PAYMENT GRID\n" +
                "       -- " + HttpContext.Current.Session["U_NAME"].ToString() + " \n" +
                "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                "                CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                "                L.ATTRIBUTEVALUE RRSTATUS,\n" +
                "                ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, pv.id VoucherID, PV.VOUCHERDATE,B4.SNAME COLLECTIONBR,\n" +
                "                CD.CODAMOUNT CODAMOUNT,\n" +
                "                (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
                "                Z.NAME ORGZONE,\n" +
                "                Z.ZONECODE ORGZONECODE,\n" +
                "                CC.ACCOUNTNO,\n" +
                "                CC.NAME CUSTOMERNAME,\n" +
                "                COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                "                CASE\n" +
                "                  WHEN C.ISPAYABLE = '1' THEN\n" +
                "                   'PAID'\n" +
                "                  ELSE\n" +
                "                   'UNPAID'\n" +
                "                END PAIDSTATUS,\n" +
                "                C.TRANSACTIONNUMBER CHEQUENO, R.STATUS\n" +
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
                "		     ON B4.branchCode = PV.BranchCode" +
                "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
                "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                "         WHERE C.COD = '1' and c.isapproved = '1'\n" +
                "           --AND C.transactionNumber IS NULL\n" +
                "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
                "           AND CC.ACCOUNTNO = '" + accountNo + "'\n" +
                "\n" +
                "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                "                   CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                "                   L.ATTRIBUTEVALUE,\n" +
                "                   CD.CODAMOUNT, \n" +
                "                   Z.NAME,\n" +
                "                   Z.ZONECODE,\n" +
                "                   CC.ACCOUNTNO,\n" +
                "                   CC.NAME,\n" +
                "                   PV.RECEIPTNO, pv.id,\n" +
                "                   C.ISPAYABLE,\n" +
                "                   C.TRANSACTIONNUMBER,\n" +
                "                   PV.VOUCHERDATE,B4.SNAME, PV.Amount, PV.Amountused, R.STATUS\n" +
                "        UNION ALL\n" +
                "        SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER,\n" +
                "               CONVERT(VARCHAR(11), C.BOOKINGDATE, 106) BOOKINGDATE,\n" +
                "               L.ATTRIBUTEVALUE RRSTATUS,\n" +
                "               ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, pv.ID VoucherID, PV.VOUCHERDATE,B4.SNAME COLLECTIONBR,\n" +
                "               CD.CODAMOUNT CODAMOUNT,\n" +
                "               (PV.Amount - PV.AmountUsed) AvailableAmount,\n" +
                "               Z.NAME ORGZONE,\n" +
                "               Z.ZONECODE ORGZONECODE,\n" +
                "               CC.ACCOUNTNO,\n" +
                "               CC.NAME CUSTOMERNAME,\n" +
                "               COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN,\n" +
                "               CASE\n" +
                "                 WHEN C.ISPAYABLE = '1' THEN\n" +
                "                  'PAID'\n" +
                "                 ELSE\n" +
                "                  'UNPAID'\n" +
                "               END PAIDSTATUS,\n" +
                "               C.TRANSACTIONNUMBER CHEQUENO, R.STATUS\n" +
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
                "		     ON B4.branchCode = PV.BranchCode" +
                "          LEFT OUTER JOIN RVDBO.LOOKUP L\n" +
                "            ON CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20))\n" +
                "         WHERE C.COD = '1' and c.isapproved = '1'\n" +
                "           --AND C.transactionNumber IS NULL\n" +
                "           AND (C.STATUS <> '9' OR C.STATUS IS NULL)\n" +
                "           AND CC.ACCOUNTNO = '" + accountNo + "'\n" +
                "\n" +
                "         GROUP BY C.CONSIGNMENTNUMBER,\n" +
                "                  CONVERT(VARCHAR(11), C.BOOKINGDATE, 106),\n" +
                "                  L.ATTRIBUTEVALUE,\n" +
                "                  CD.CODAMOUNT,\n" +
                "                  Z.NAME,\n" +
                "                  Z.ZONECODE,\n" +
                "                  CC.ACCOUNTNO,\n" +
                "                  CC.NAME,\n" +
                "                  PV.RECEIPTNO, pv.id,\n" +
                "                  C.ISPAYABLE,\n" +
                "                  C.TRANSACTIONNUMBER,\n" +
                "                  PV.VOUCHERDATE,B4.SNAME, PV.amount, PV.AmountUsed, R.STATUS) B\n" +
                " where RRSTATUS = 'D-DELIVERED' " + paidstatus + " \n" +
                " ORDER BY 1";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 3000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }
        public static DataTable GetInvoicesForVoucherNonCentralized(Cl_Variables clvar, string condtion, string CODType)
        {
            #region sqlString__11-04-2020
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
            "           WHERE  ISNULL(pv.PaymentSourceId, 1) in ('1','8')\n" +
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
            "WHERE  b.clientId = '" + clvar.CreditClientID + "' and cc.sectorid != '0'\n" +
            "           and  i.IsInvoiceCanceled = '0' and i.startDate >= '2017-06-29' \n" +
           "GROUP BY\n" +
            "       b.invoiceNumber,\n" +
            "       b.clientId,\n" +
            "       b2.name ,\n" +
            "       datename(MM,i.startDate)+'-'+datename(YY,i.startDate) ,\n" +
            "       c.companyName\n" +
            "HAVING SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) >= 1";

            #endregion

            string sql = "SELECT \n"
               + "       ----- COD PAYMENT GET DATA \n"
               + "       -- " + HttpContext.Current.Session["U_NAME"].ToString() + " \n"
               + "       x.CUSTOMERNAME, \n"
               + "       x.ACCOUNTNO, \n"
               + "       SUM(round(x.Oustanding,0))     Oustanding, \n"
               + "       SUM(round(x.CODPayable,0))     CODPayable, \n"
               + "       SUM(round(x.CODPayable,0)) - SUM(round(x.Oustanding,0)) NETPAYABLE, \n"
               + "       NULL                  PAYMENTID, \n"
               + "       NULL                  DOWNLOAD \n"
               + "FROM   ( \n"
               + "           SELECT NULL invoiceNumber,CUSTOMERNAME, \n"
               + "                  ACCOUNTNO, \n"
               + "                  SUM(CODAMOUNT)     CODPayable, \n"
               + "                  0                  Total_Amount, \n"
               + "                  0                  Oustanding \n"
               + "           FROM   ( \n"
               + "                      SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER, \n"
               + "                             CONVERT(VARCHAR(11), C.BOOKINGDATE, 106)  \n"
               + "                             BOOKINGDATE, \n"
               + "                             L.ATTRIBUTEVALUE RRSTATUS, \n"
               + "                             ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, \n"
               + "                             pv.id          VoucherID, \n"
               + "                             PV.VOUCHERDATE, \n"
               + "                             B4.SNAME       COLLECTIONBR, \n"
               + "                             CD.CODAMOUNT CODAMOUNT, \n"
               + "                             (PV.Amount - PV.AmountUsed) AvailableAmount, \n"
               + "                             Z.NAME         ORGZONE, \n"
               + "                             Z.ZONECODE     ORGZONECODE, \n"
               + "                             CC.ACCOUNTNO, \n"
               + "                             CC.NAME        CUSTOMERNAME, \n"
               + "                             COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN, \n"
               + "                             CASE  \n"
               + "                                  WHEN C.ISPAYABLE = '1' THEN 'PAID' \n"
               + "                                  ELSE 'UNPAID' \n"
               + "                             END            PAIDSTATUS, \n"
               + "                             C.TRANSACTIONNUMBER CHEQUENO, \n"
               + "                             R.STATUS \n"
               + "                      FROM   CONSIGNMENT C \n"
               + "                             INNER JOIN CODCONSIGNMENTDETAIL CD \n"
               + "                                  ON  CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER \n"
               + "                             INNER JOIN RUNSHEETCONSIGNMENT R \n"
               + "                                  ON  R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER \n"
               + "                                  AND R.CREATEDON = ( \n"
               + "                                          SELECT MAX(CREATEDON) \n"
               + "                                          FROM   RUNSHEETCONSIGNMENT RC1 \n"
               + "                                          WHERE  RC1.CONSIGNMENTNUMBER = R.CONSIGNMENTNUMBER \n"
               + "                                      ) \n"
               + "                             LEFT JOIN PAYMENTVOUCHERS PV \n"
               + "                                  ON  PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER \n"
               + "                                  AND PV.CREDITCLIENTID = C.CREDITCLIENTID \n"
               + "                             INNER JOIN CREDITCLIENTS CC \n"
               + "                                  ON  CC.ID = C.CREDITCLIENTID \n"
               + "                             INNER JOIN MNP_Retail_COD_Customers rc ON cc.accountNo = rc.AccountNumber \n"
            + "                             INNER JOIN BRANCHES B2 \n"
               + "                                  ON  CC.BRANCHCODE = B2.BRANCHCODE \n"
               + "                             INNER JOIN ZONES Z \n"
               + "                                  ON  Z.ZONECODE = B2.ZONECODE \n"
               + "                             INNER JOIN BRANCHES B3 \n"
               + "                                  ON  C.DESTINATION = B3.BRANCHCODE \n"
               + "                             INNER JOIN ZONES Z2 \n"
               + "                                  ON  B3.ZONECODE = Z2.ZONECODE \n"
               + "                             INNER JOIN Branches B4 \n"
               + "                                  ON  B4.branchCode = PV.BranchCode \n"
               + "                             LEFT OUTER JOIN RVDBO.LOOKUP L \n"
               + "                                  ON  CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20)) \n"
               + "                      WHERE  C.COD = '1' \n"
               + "                             AND ISNULL(C.isapproved, '0') = '1' \n"
               + "                             AND ISNULL(C.STATUS, '0') != '9' \n"
               + "                             AND r.[Status] = '55' \n"
               + "                             AND isnull(rc.isApproved,'0') = '1' \n"
               + "                             AND cc.CODType != '2' \n" + // EXCLUDE 2 PAY CUSTOMERS
                clvar.Zone + " \n" +
                CODType + "\n"
            + "                             AND ISNULL(cc.isActive, '0') = '1' \n";
            if (clvar.AccountNo != "")
            {
                sql += "                   AND CC.ACCOUNTNO = '" + clvar.AccountNo + "' \n";
            }
            //   + "                             AND z.zoneCode = '3' -- customer zone \n"
            sql += "                      GROUP BY \n"
            + "                             C.CONSIGNMENTNUMBER, \n"
            + "                             CONVERT(VARCHAR(11), C.BOOKINGDATE, 106), \n"
            + "                             L.ATTRIBUTEVALUE, \n"
            + "                             CD.CODAMOUNT, \n"
            + "                             Z.NAME, \n"
            + "                             Z.ZONECODE, \n"
            + "                             CC.ACCOUNTNO, \n"
            + "                             CC.NAME, \n"
            + "                             PV.RECEIPTNO, \n"
            + "                             pv.id, \n"
            + "                             C.ISPAYABLE, \n"
            + "                             C.TRANSACTIONNUMBER, \n"
            + "                             PV.VOUCHERDATE, \n"
            + "                             B4.SNAME, \n"
            + "                             PV.Amount, \n"
            + "                             PV.Amountused, \n"
            + "                             R.STATUS \n"
            + "                       \n"
            + "                      UNION ALL \n"
            + "                       \n"
            + "                      SELECT C.CONSIGNMENTNUMBER CONSIGNMENTNUMBER, \n"
            + "                             CONVERT(VARCHAR(11), C.BOOKINGDATE, 106)  \n"
            + "                             BOOKINGDATE, \n"
            + "                             L.ATTRIBUTEVALUE RRSTATUS, \n"
            + "                             ISNULL(PV.RECEIPTNO, '-') PAYMENTVOUCHERID, \n"
            + "                             pv.ID          VoucherID, \n"
            + "                             PV.VOUCHERDATE, \n"
            + "                             B4.SNAME       COLLECTIONBR, \n"
            + "                             CD.CODAMOUNT CODAMOUNT, \n"
            + "                             (PV.Amount - PV.AmountUsed) AvailableAmount, \n"
            + "                             Z.NAME         ORGZONE, \n"
            + "                             Z.ZONECODE     ORGZONECODE, \n"
            + "                             CC.ACCOUNTNO, \n"
            + "                             CC.NAME        CUSTOMERNAME, \n"
            + "                             COUNT(DISTINCT PV.CONSIGNMENTNO) DELIVEREDCN, \n"
            + "                             CASE  \n"
            + "                                  WHEN C.ISPAYABLE = '1' THEN 'PAID' \n"
            + "                                  ELSE 'UNPAID' \n"
            + "                             END            PAIDSTATUS, \n"
            + "                             C.TRANSACTIONNUMBER CHEQUENO, \n"
            + "                             R.STATUS \n"
            + "                      FROM   CONSIGNMENT C \n"
            + "                             INNER JOIN CODCONSIGNMENTDETAIL_NEW CD \n"
            + "                                  ON  CD.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER \n"
            + "                             INNER JOIN RUNSHEETCONSIGNMENT R \n"
            + "                                  ON  R.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER \n"
            + "                                  AND R.CREATEDON = ( \n"
            + "                                          SELECT MAX(CREATEDON) \n"
            + "                                          FROM   RUNSHEETCONSIGNMENT RC1 \n"
            + "                                          WHERE  RC1.CONSIGNMENTNUMBER = R.CONSIGNMENTNUMBER \n"
            + "                                      ) \n"
            + "                             LEFT JOIN PAYMENTVOUCHERS PV \n"
            + "                                  ON  PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER \n"
            + "                                  AND PV.CREDITCLIENTID = C.CREDITCLIENTID \n"
            + "                             INNER JOIN CREDITCLIENTS CC \n"
            + "                                  ON  CC.ID = C.CREDITCLIENTID \n"
            + "                             INNER JOIN MNP_Retail_COD_Customers rc ON cc.accountNo = rc.AccountNumber \n"
            + "                             INNER JOIN BRANCHES B2 \n"
            + "                                  ON  CC.BRANCHCODE = B2.BRANCHCODE \n"
            + "                             INNER JOIN ZONES Z \n"
            + "                                  ON  Z.ZONECODE = B2.ZONECODE \n"
            + "                             INNER JOIN BRANCHES B3 \n"
            + "                                  ON  C.DESTINATION = B3.BRANCHCODE \n"
            + "                             INNER JOIN ZONES Z2 \n"
            + "                                  ON  B3.ZONECODE = Z2.ZONECODE \n"
            + "                             INNER JOIN Branches B4 \n"
            + "                                  ON  B4.branchCode = PV.BranchCode \n"
            + "                             LEFT OUTER JOIN RVDBO.LOOKUP L \n"
            + "                                  ON  CAST(R.REASON AS VARCHAR(20)) = CAST(L.ID AS VARCHAR(20)) \n"
            + "                      WHERE  C.COD = '1' \n"
            + "                             AND ISNULL(C.isapproved, '0') = '1' \n"
            + "                             AND ISNULL(C.STATUS, '0') != '9' \n"
            + "                             AND r.[Status] = '55' \n"
               + "                             AND isnull(rc.isApproved,'0') = '1' \n"
            + "                             AND cc.CODType != '2' \n" + // EXCLUDE 2 PAY CUSTOMERS
                clvar.Zone + " \n" +
                CODType + "\n"
            + "                             AND ISNULL(cc.isActive, '0') = '1' \n";
            if (clvar.AccountNo != "")
            {
                sql += "                   AND CC.ACCOUNTNO = '" + clvar.AccountNo + "' \n";
            }
            sql += "                      GROUP BY \n"
               + "                             C.CONSIGNMENTNUMBER, \n"
               + "                             CONVERT(VARCHAR(11), C.BOOKINGDATE, 106), \n"
               + "                             L.ATTRIBUTEVALUE, \n"
               + "                             CD.CODAMOUNT, \n"
               + "                             Z.NAME, \n"
               + "                             Z.ZONECODE, \n"
               + "                             CC.ACCOUNTNO, \n"
               + "                             CC.NAME, \n"
               + "                             PV.RECEIPTNO, \n"
               + "                             pv.id, \n"
               + "                             C.ISPAYABLE, \n"
               + "                             C.TRANSACTIONNUMBER, \n"
               + "                             PV.VOUCHERDATE, \n"
               + "                             B4.SNAME, \n"
               + "                             PV.amount, \n"
               + "                             PV.AmountUsed, \n"
               + "                             R.STATUS \n"
               + "                  )                  B \n"
               + "           WHERE  b.PaidStatus = 'UNPAID' \n"
               + "                  AND b.CHEQUENO IS NULL \n"
               + "           GROUP BY \n"
               + "                  CUSTOMERNAME, \n"
               + "                  ACCOUNTNO \n"
               + "            \n"
               + "           UNION ALL \n"
               + "            \n"
               + "           --------- Total Invoice Amount --------- \n"
               + "            \n"
               + "            \n"
               + "           SELECT /* b.invoiceNumber, \n"
               + "                  b.clientId, \n"
               + "                  b2.name Branch, \n"
               + "                  datename(MM,i.startDate)+'-'+datename(YY,i.startDate) Month, \n"
               + "                  c.companyName,*/ \n"
               + "                  b.invoiceNumber,CC.NAME                   CUSTOMERNAME, \n"
               + "                  CC.ACCOUNTNO, \n"
               + "                  0                         CODPayable, \n"
               + "                  SUM(b.Invoice_Amount)     Total_Amount, \n"
               + "                  SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) \n"
               + "                  Oustanding \n"
               + "           FROM   ( \n"
               + "                      SELECT i.invoiceNumber, \n"
               + "                             i.clientId, \n"
               + "                             SUM(i.totalAmount) Invoice_Amount, \n"
               + "                             0           RECOVERY, \n"
               + "                             0           Adjustment \n"
               + "                      FROM   Invoice  AS i \n"
               + "                      WHERE  i.IsInvoiceCanceled = '0' \n"
               + "                      GROUP BY \n"
               + "                             i.invoiceNumber, \n"
               + "                             i.clientId \n"
               + "                       \n"
               + "                      UNION \n"
               + "                       \n"
               + "                      SELECT ir.InvoiceNo, \n"
               + "                             i.clientId, \n"
               + "                             0     Invoice_Amount, \n"
               + "                             SUM(ir.Amount) RECOVERY, \n"
               + "                             0     Adjustment \n"
               + "                      FROM   InvoiceRedeem AS ir \n"
               + "                             INNER JOIN Invoice AS i \n"
               + "                                  ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "                             INNER JOIN PaymentVouchers AS pv \n"
               + "                                  ON  pv.Id = ir.PaymentVoucherId \n"
               + "                      WHERE  ISNULL(pv.PaymentSourceId, 1) IN ('1', '8') \n"
               + "                             AND i.IsInvoiceCanceled = '0' \n"
               + "                      GROUP BY \n"
               + "                             ir.InvoiceNo, \n"
               + "                             i.clientId \n"
               + "                       \n"
               + "                      UNION \n"
               + "                       \n"
               + "                      SELECT ir.InvoiceNo, \n"
               + "                             i.clientId, \n"
               + "                             0     Invoice_Amount, \n"
               + "                             SUM(ir.Amount) RECOVERY, \n"
               + "                             0     Adjustment \n"
               + "                      FROM   InvoiceRedeem AS ir \n"
               + "                             INNER JOIN Invoice AS i \n"
               + "                                  ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "                             INNER JOIN PaymentVouchers AS pv \n"
               + "                                  ON  pv.Id = ir.PaymentVoucherId \n"
               + "                             INNER JOIN ChequeStatus AS cs \n"
               + "                                  ON  cs.PaymentVoucherId = pv.Id \n"
               + "                      WHERE  pv.PaymentSourceId IN ('2', '3', '4') \n"
               + "                             AND i.IsInvoiceCanceled = '0' \n"
               + "                             AND cs.IsCurrentState = '1' \n"
               + "                             AND cs.ChequeStateId IN ('1', '2') \n"
               + "                      GROUP BY \n"
               + "                             ir.InvoiceNo, \n"
               + "                             i.clientId \n"
               + "                       \n"
               + "                       \n"
               + "                      UNION \n"
               + "                       \n"
               + "                      SELECT gv.InvoiceNo, \n"
               + "                             gv.CreditClientId, \n"
               + "                             0     Invoice_Amount, \n"
               + "                             0     RECOVERY, \n"
               + "                             SUM(gv.Amount) Adjustment \n"
               + "                      FROM   GeneralVoucher AS gv \n"
               + "                      GROUP BY \n"
               + "                             gv.InvoiceNo, \n"
               + "                             gv.CreditClientId \n"
               + "                  ) b \n"
               + "                  INNER JOIN Invoice     AS i \n"
               + "                       ON  i.invoiceNumber = b.invoiceNumber \n"
               + "                  INNER JOIN CreditClients AS cc \n"
               + "                       ON  cc.id = b.clientId \n"
               + "                  INNER JOIN MNP_Retail_COD_Customers rc ON cc.accountNo = rc.AccountNumber \n"
               + "                  INNER JOIN Branches    AS b2 \n"
               + "                       ON  b2.branchCode = cc.branchCode \n"
               + "                  INNER JOIN Zones z \n"
               + "                       ON  B2.zoneCode = Z.zoneCode \n"
               + "                           --INNER JOIN Company     AS c \n"
               + "                           --     ON  c.Id = i.companyId \n"
               + "           WHERE  --b.clientId = '435298'  \n"
               + "                  cc.sectorid != '0' \n"
               + "                  AND i.IsInvoiceCanceled = '0' \n"
               + "                  AND i.startDate >= '2017-06-29' \n" +
                clvar.Zone + "\n" +
                CODType + "\n"
               + "                  AND CC.IsCOD = '1' \n"
               + "                  AND isnull(rc.isApproved,'0') = '1' \n"
               + "                  AND cc.CODType != '2' \n"  // EXCLUDE 2 PAY CUSTOMERS
               + "                  AND ISNULL(cc.isActive, '0') = '1' \n";
            if (clvar.AccountNo != "")
            {
                sql += "        AND CC.ACCOUNTNO = '" + clvar.AccountNo + "' \n";
            }
            sql += "           GROUP BY \n"
           + "                  CC.NAME, \n"
           + "                  CC.ACCOUNTNO, b.invoiceNumber \n"
           + "           HAVING SUM(b.Invoice_Amount) -(SUM(b.Recovery) + SUM(b.Adjustment)) >= 1 \n"
           + "       ) x \n"
           + "GROUP BY \n"
           + "       x.CUSTOMERNAME, \n"
           + "       x.ACCOUNTNO \n"
           + condtion + " \n"
           + "ORDER BY 5 DESC";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 3000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public static DataTable GetInvoicesData(string CreditClientID, Variable clvar)
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
            " WHERE b.clientId = '" + CreditClientID + "'\n" +
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
            "           WHERE  ISNULL(pv.PaymentSourceId, 1) in ('1','8')\n" +
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
            "WHERE  b.clientId = '" + CreditClientID + "' and cc.sectorid != '0'\n" +
            "           and  i.IsInvoiceCanceled = '0' and i.startDate >= '2017-06-29' \n" +
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
        public static DataTable Get_CreditClientID(string accountNo, Variable clvar)
        {
            string query = "SELECT * FROM CreditClients cc WHERE cc.accountNo = '" + accountNo + "' AND cc.isActive = '1'";

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
        public static string[] MarkPaid(string CN, DataTable dt, DataTable cns, Variable clvar)
        {
            string error = "", PaymentId_ = "";

            string[] error_ = new String[2];

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "MnP_CODPayment_Paid";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@CNNumbers", cns);
                cmd.Parameters.AddWithValue("@tbl_redeem", dt);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                //if (txt_chequeNo.Text.Trim() != "")
                //{
                //    cmd.Parameters.AddWithValue("@ChequeNumber", txt_chequeNo.Text);
                //}
                cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@ResultStatus"].Value.ToString() == "0")
                {
                    error = cmd.Parameters["@result"].Value.ToString();
                }
                else
                {
                    PaymentId_ = cmd.Parameters["@result"].Value.ToString();
                    //   error = "OK";

                    error_[0] = "OK";
                    error_[1] = PaymentId_;

                    //txt_chequeNo.Text = cmd.Parameters["@result"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                //error_ = ex.Message; 
            }
            finally { con.Close(); }
            return error_;
        }
        public void Get_Zone()
        {
            DataSet ds_zone = Get_AllZones(clvar);

            dd_branch.Items.Clear();

            if (ds_zone.Tables[0].Rows.Count != 0)
            {
                dd_zone.DataTextField = "Name";
                dd_zone.DataValueField = "zoneCode";
                dd_zone.DataSource = ds_zone.Tables[0].DefaultView;
                dd_zone.DataBind();
            }
        }
        public DataSet Get_AllZones(Variable clvar)
        {
            DataSet ds = new DataSet();
            string query;
            try
            {
                query = "SELECT z.zoneCode, z.name FROM Zones z WHERE z.Region IS NOT NULL ORDER BY z.name ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }
        protected void branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codTypeValue = codtype.SelectedValue;

            if (codTypeValue == "RetailCOD")
            {
                clvar.Check_Condition1 = "AND CC.CODTYPE = '3' \n";
            }

            if (codTypeValue == "CorporateCOD")
            {
                clvar.Check_Condition1 = "AND CC.CODTYPE != '3' \n";
            }

            String ZoneID = "";
            for (int i = 0; i < dd_zone.Items.Count; i++)
            {
                if (dd_zone.Items[i].Selected)
                {
                    ZoneID += dd_zone.Items[i].Value + ',';
                }
            }
            ZoneID = ZoneID.Remove(ZoneID.Length - 1);
            ZoneID.ToString();

            clvar._Zone = ZoneID;

            DataSet ds_zone = Get_CreditClientAccounts(clvar);
            dd_customer.Items.Clear();
            if (ds_zone.Tables[0].Rows.Count != 0)
            {
                dd_customer.DataTextField = "name";
                dd_customer.DataValueField = "accountno";
                dd_customer.DataSource = ds_zone.Tables[0].DefaultView;
                dd_customer.DataBind();
                dd_customer.Items.Insert(0, new ListItem("Select Customer Name", "0"));
            }
        }
        protected void branch_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (branch_chk.Checked)
            {
                dd_branch.Visible = false;
            }
            else
            {
                dd_branch.Visible = true;
            }
        }
        protected void codtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            branch_SelectedIndexChanged(sender, e);
        }
        public DataSet Get_ZonebyBranches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string query;
            try
            {
                query = "select branchCode, name from Branches where status = '1' AND zoneCode IN (" + clvar._Zone + ") order by name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }
        public static void ExporttopdfSummary(string Check_Condition1, string paymentId, string zone)
        {
            string invNum = "";
            string Invoice_numbers = "";

            DataTable dt_upper = getUpperData(Check_Condition1);
            DataTable RTS = GetRTSData(Check_Condition1);

            if (dt_upper.Rows.Count == 0)
            {
                return;
            }

            for (int inv = 0; inv < dt_upper.Rows.Count; inv++)
            {
                if (dt_upper.Rows[inv]["invoicenumber"].ToString() != "")
                {
                    invNum += "'" + dt_upper.Rows[inv]["invoicenumber"].ToString() + "'";

                    if (inv + 1 < dt_upper.Rows.Count)
                    {
                        Invoice_numbers += dt_upper.Rows[inv]["invoicenumber"].ToString() + ",";
                    }

                    else
                    {
                        Invoice_numbers += dt_upper.Rows[inv]["invoicenumber"].ToString();
                    }
                }
            }
            invNum = invNum.Replace("''", "','");
            if (invNum.Trim() == "")
            {
                invNum = "''";
            }
            Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            iTextSharp.text.Font NormalFont = FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table_up = null;
                PdfPTable table = null;
                PdfPTable table_Center = null;
                PdfPTable table_pageNum = null;
                PdfPTable table_header = null;
                iTextSharp.text.BaseColor color = null;

                document.Open();

                table_header = new PdfPTable(3);
                table_header.TotalWidth = 550f;
                table_header.LockedWidth = true;
                table_header.SetWidths(new float[] { 50f, 60f, 370f });

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images") + "\\new_logo.png");
                jpg.ScaleAbsolute(60f, 30f);
                PdfPCell imageCell = new PdfPCell(jpg);
                imageCell.Colspan = 1;
                table_header.AddCell(imageCell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 18, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.PaddingBottom = 0f;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("M&P Payment Detail ", FontFactory.GetFont("Courier New", 18, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.PaddingBottom = 0f;
                table_header.AddCell(cell);


                table_up = new PdfPTable(4);
                table_up.TotalWidth = 550f;
                table_up.LockedWidth = true;
                table_up.SetWidths(new float[] { 20f, 40f, 20f, 40f });
                //UPPER TABLE

                cell = PhraseCell(new Phrase("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 4;
                cell.Padding = 10f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase("Account Number:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("Payment ID :", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                // cell = PhraseCell(new Phrase(txt_chequeNo.Text, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("Customer Name:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("Print Date/Time:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("City:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);
                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.Padding = 5f;
                //cell.UseVariableBorders = true;
                //table_up.AddCell(cell);


                cell = PhraseCell(new Phrase("Beneficiary Name:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase(dt_upper.Rows[0]["BeneficiaryName"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("Payment Period:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                DateTime dt_min = DateTime.Parse(dt_upper.Rows[0]["MinPaymentPeriod"].ToString());
                DateTime dt_max = DateTime.Parse(dt_upper.Rows[0]["MaxPaymentPeriod"].ToString());


                cell = PhraseCell(new Phrase(dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);



                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 2;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);


                cell = PhraseCell(new Phrase("Invoice Numbers:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                cell = PhraseCell(new Phrase(Invoice_numbers, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 3;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);



                cell = PhraseCell(new Phrase("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 4;
                cell.Padding = 10f;
                cell.UseVariableBorders = true;
                table_up.AddCell(cell);

                //cell = PhraseCell(new Phrase("Invoice Number:     " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 2;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.Padding = 5f;
                //cell.UseVariableBorders = true;
                //table_up.AddCell(cell);

                //cell = PhraseCell(new Phrase("Reporting Date:        " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 2;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.Padding = 5f;
                //cell.UseVariableBorders = true;
                //table_up.AddCell(cell);



                //Center table
                DataTable dt_right = CenterTableRight_Data(invNum, Check_Condition1);
                if (dt_right.Rows.Count == 0)
                {
                    return;
                }

                DataTable dt_left = CenterTableLeft_Data(invNum, Check_Condition1);
                if (dt_right.Rows.Count == 0)
                {
                    return;
                }


                table_Center = new PdfPTable(4);
                table_Center.TotalWidth = 550f;
                table_Center.LockedWidth = true;

                cell = PhraseCell(new Phrase("INVOICE SUMMARY ", FontFactory.GetFont("Courier New", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 10f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("PAYMENT SUMMARY", FontFactory.GetFont("Courier New", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 10f;
                cell.PaddingTop = 10f;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Shipping Charges:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double shipmentCharge = 0;

                double.TryParse(dt_right.Rows[0]["shipmentCharge"].ToString(), out shipmentCharge);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", shipmentCharge), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);


                cell = PhraseCell(new Phrase("Total COD Amount:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double TotalCODAmount = 0;
                double.TryParse(dt_left.Rows[0]["TotalCODAmount"].ToString(), out TotalCODAmount);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", TotalCODAmount), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Fuel Surcharge:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double FUEL_CHARGE = 0;
                double.TryParse(dt_right.Rows[0]["FUEL_CHARGE"].ToString(), out FUEL_CHARGE);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", FUEL_CHARGE), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Total Invoice Amount:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double invoiceAmount = 0;
                double.TryParse(dt_left.Rows[0]["invoiceAmount"].ToString(), out invoiceAmount);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", invoiceAmount), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("GST:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double GST = 0;
                double.TryParse(dt_right.Rows[0]["GST"].ToString(), out GST);


                cell = PhraseCell(new Phrase(string.Format("{0:N0}", GST), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Net Payable:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double NetPayable = 0;
                double.TryParse(dt_left.Rows[0]["NetPayable"].ToString(), out NetPayable);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", NetPayable), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Extra Charge:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double EXTRACHARGES = 0;
                double.TryParse(dt_right.Rows[0]["EXTRACHARGES"].ToString(), out EXTRACHARGES);

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", EXTRACHARGES), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorBottom = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);


                cell = PhraseCell(new Phrase("Returned to Shipper", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase(String.Format("{0:N2}", RTS.Rows[0][0]), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("Total Invoice Amount:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                double SUM_AMOUNT = shipmentCharge + FUEL_CHARGE + GST + EXTRACHARGES;

                cell = PhraseCell(new Phrase(string.Format("{0:N0}", SUM_AMOUNT), FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorBottom = BaseColor.BLACK;

                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);


                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.Padding = 5f;
                cell.UseVariableBorders = true;
                table_Center.AddCell(cell);

                table_Center.SpacingAfter = 1000f;
                document.Add(table_header);
                document.Add(table_up);
                document.Add(table_Center);

                //Grid
                int c = 1;

                DataSet header = getData(invNum, Check_Condition1);//------------------------------------

                //  header.Tables[0].Columns.Add("Sr.", typeof(int));
                double footerCodAmount = 0;
                double footerShippingCharges = 0;

                foreach (DataRow dr in header.Tables[0].Rows)
                {
                    double temp = 0;
                    double.TryParse(dr["CODAmount"].ToString(), out temp);

                    footerCodAmount += temp;

                    temp = 0;
                    double.TryParse(dr["ShippingCharges"].ToString(), out temp);

                    footerShippingCharges += temp;

                }

                //header.Tables[0].Rows.Add("0", "", "", "", "", String.Format("{0:N2}", footerCodAmount), "", "", "", "", String.Format("{0:N2}", footerShippingCharges), "", "");
                GridView GridView1 = new GridView();

                GridView1.DataSource = header;
                GridView1.DataBind();
                int colcount = GridView1.Rows[0].Cells.Count;


                table = new PdfPTable(colcount);
                table.TotalWidth = 550f;

                // table.SetWidths(new float[] { 60, 25f, 57f, 45f, 45f, 50f, 24f, 24, 30f, 65f, 50f, 40f, 25f });
                table.SetWidths(new float[] { 20f, 60, 35f, 45f, 35f, 45f, 30f, 35f, 32f, 50f, 45f, 50f });
                table.LockedWidth = true;

                for (int i = 0; i < colcount; i++)
                {
                    if (i == 0)
                    {
                        cell = PhraseCell(new Phrase("Account Number: " + dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);


                        cell = PhraseCell(new Phrase("Customer Name: " + dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 5;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);


                        cell = PhraseCell(new Phrase("City: " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        cell = PhraseCell(new Phrase("Payment Period: " + dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 3;
                        cell.Padding = 5f;
                        cell.BorderColor = BaseColor.BLACK;
                        table.AddCell(cell);



                    }

                    cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[i].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 1;
                    cell.Padding = 4f;
                    cell.BorderColor = BaseColor.BLACK;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }
                int sr = 1;
                int z = 1;

                int totalPages = 0;

                totalPages = (GridView1.Rows.Count % 45 > 0) ? (GridView1.Rows.Count / 45) + 1 : (GridView1.Rows.Count / 45) + 0;

                foreach (GridViewRow row in GridView1.Rows)
                {
                    cell = PhraseCell(new Phrase(sr.ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.BorderColor = BaseColor.BLACK;
                    cell.Padding = 5f;
                    //cell.Width = 10f;

                    table.AddCell(cell);
                    for (int i = 0; i < colcount; i++)
                    {
                        if (i == 0)
                        {

                        }
                        else
                        {
                            if (row.Cells[i].Text == "&nbsp;" || row.Cells[i].Text == null || row.Cells[i].Text == "" || String.IsNullOrWhiteSpace(row.Cells[i].Text))
                            {
                                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                            }
                            else
                            {
                                if (i == 2)
                                {
                                    string refNo = (row.Cells[i].Text.Length >= 15) ? row.Cells[i].Text.Substring(0, 15) : row.Cells[i].Text.ToString();
                                    cell = PhraseCell(new Phrase(refNo, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                }

                                else if (i == 5)
                                {
                                    string name = row.Cells[i].Text;
                                    if (name.Length >= 10)
                                    {
                                        cell = PhraseCell(new Phrase(name.Substring(0, 10), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                    }
                                    else
                                    {
                                        cell = PhraseCell(new Phrase(row.Cells[i].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                    }
                                }

                                else if (i == 10)
                                {
                                    double ammount = 0;
                                    double.TryParse(row.Cells[i].Text, out ammount);

                                    cell = PhraseCell(new Phrase(string.Format("{0:N2}", ammount), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                                }

                                else if (i == 11)
                                {
                                    double ammount = 0;
                                    double.TryParse(row.Cells[i].Text, out ammount);

                                    cell = PhraseCell(new Phrase(string.Format("{0:N2}", ammount), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                                }

                                else
                                {
                                    cell = PhraseCell(new Phrase(row.Cells[i].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                                }


                            }

                            cell.Colspan = 1;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.Padding = 5f;
                            if (i == 2)
                            {
                                cell.PaddingLeft = 2f;
                                cell.PaddingRight = 2f;
                            }
                            else if (i == 3 || i == 4)
                            {
                                cell.PaddingLeft = 2f;
                                cell.PaddingRight = 2f;
                            }
                            else
                            {
                                cell.PaddingLeft = 5f;
                                cell.PaddingRight = 5f;
                            }

                            //cell.Width = 10f;

                            table.AddCell(cell);
                        }
                    }
                    z++;
                    sr++;
                    if (c == 1 && z == 46)
                    {
                        for (int a = 1; a < 25; a++)//z is row number
                        {
                            string pageNum;
                            if (a == 24) //a cell spaces that are left then pagenum will print curently 8cells to give one line gap its 16
                            {
                                pageNum = "Page No  " + c.ToString() + " of " + totalPages.ToString();
                            }
                            else
                            {
                                pageNum = "";
                            }
                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                            cell.Colspan = 3;
                            cell.Padding = 5f;
                            table.AddCell(cell);

                        }

                        table.SpacingAfter = 1000f;
                        document.Add(table);
                        table = new PdfPTable(colcount);
                        table.TotalWidth = 550f;

                        //table.SetWidths(new float[] {60, 25f, 57f, 40f, 40f, 50f, 24f, 24, 30f, 65f, 50f, 40f });
                        table.SetWidths(new float[] { 20f, 60, 35f, 45f, 35f, 45f, 30f, 35f, 32f, 50f, 45f, 50f });
                        table.LockedWidth = true;
                        //string pageNum;
                        //for (int a = 0; a < 5; a++)
                        //{
                        //    if (a == 4)
                        //    {
                        //        pageNum = "Page Num  " + c.ToString();
                        //    }
                        //    else
                        //    {
                        //        pageNum = "";
                        //    }
                        //    cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        //    cell.Colspan = 1;
                        //    table.AddCell(cell);
                        //}

                        for (int j = 0; j < colcount; j++)
                        {

                            if (j == 0)
                            {
                                cell = PhraseCell(new Phrase("Account Number: " + dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("Customer Name: " + dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 5;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("City: " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = PhraseCell(new Phrase("Payment Period: " + dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 3;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);



                            }

                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                            cell.Colspan = 1;
                            cell.Padding = 4f;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);

                        }
                        z = 1;

                        c++;
                    }

                    else if (c > 1 && z == 46)
                    {
                        string pageNum;

                        for (int a = 1; a < 23; a++)//z is row number
                        {

                            if (a == 6)//a cell spaces that are left then pagenum will print
                            {
                                pageNum = "Page No  " + c.ToString() + " of " + totalPages.ToString();
                            }
                            else
                            {
                                pageNum = "";
                            }
                            cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                            cell.Colspan = 2;
                            cell.Padding = 5f;
                            table.AddCell(cell);

                        }
                        table.SpacingAfter = 1000f;
                        document.Add(table);
                        table = new PdfPTable(colcount);
                        table.TotalWidth = 550f;

                        table.SetWidths(new float[] { 25f, 60, 57f, 40f, 40f, 50f, 24f, 24, 30f, 65f, 50f, 40f });
                        table.LockedWidth = true;
                        for (int j = 0; j < colcount; j++)
                        {
                            if (j == 0)
                            {
                                cell = PhraseCell(new Phrase("Account Number: " + dt_upper.Rows[0]["accountNo"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("Customer Name: " + dt_upper.Rows[0]["clientname"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 5;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);


                                cell = PhraseCell(new Phrase("City: " + dt_upper.Rows[0]["city"].ToString(), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 2;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);

                                cell = PhraseCell(new Phrase("Payment Period: " + dt_min.ToString("dd/MM/yyyy") + " To " + dt_max.ToString("dd/MM/yyyy"), FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                                cell.Colspan = 3;
                                cell.Padding = 5f;
                                cell.BorderColor = BaseColor.BLACK;
                                table.AddCell(cell);



                            }
                            cell = PhraseCell(new Phrase(GridView1.HeaderRow.Cells[j].Text, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                            cell.Colspan = 1;
                            cell.Padding = 4f;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);


                        }
                        //cell = PhraseCell(c, FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        // cell.Colspan = 1;
                        //cell.BorderColor = BaseColor.BLACK;

                        //table.AddCell(cell);

                        z = 1;
                        c++;
                    }
                }

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase("TOTALS:", FontFactory.GetFont("Courier New", 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 9;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(String.Format("{0:N2}", footerCodAmount), FontFactory.GetFont("Courier New", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(String.Format("{0:N2}", footerShippingCharges), FontFactory.GetFont("Courier New", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 5f;
                cell.BorderColor = BaseColor.BLACK;
                table.AddCell(cell);



                table_pageNum = new PdfPTable(2);
                table_pageNum.TotalWidth = 550f;
                table_pageNum.LockedWidth = true;
                table_pageNum.SpacingBefore = 30f;
                table_pageNum.SetWidths(new float[] { 35f, 40f });


                for (int a = 1; a < 5; a++)
                {
                    string pageNum;
                    if (a == 4)
                    {
                        pageNum = "Page No  " + c.ToString() + " of " + totalPages.ToString();
                    }
                    else
                    {
                        pageNum = "";
                    }
                    cell = PhraseCell(new Phrase(pageNum, FontFactory.GetFont("Courier New", 6, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                    cell.Colspan = 1;
                    cell.Padding = 5f;
                    table_pageNum.AddCell(cell);
                }

                document.Add(table);

                document.Add(table_pageNum);

                document.Close();

                /*
                //Create document  
                //Document doc = new Document();
                ////Create PDF Table  
                //PdfPTable tableLayout = new PdfPTable(4);
                //Create a PDF file in specific path  
                PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("Sample-PDF-File.pdf"), FileMode.Create));

                //Open the PDF document  
                //doc.Open();
                ////Add Content to PDF  
                //doc.Add(table(tableLayout));
                //// Closing the document  
                //doc.Close();

                //Open the PDF document  
                document.Open();
                document.Add(table);

                document.Add(table_pageNum);

                document.Close();
                 */

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                string Folder = HttpContext.Current.Server.MapPath("~/CODPayment/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + zone + "/");
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }

                string fileName = dt_upper.Rows[0]["clientname"].ToString() + "_" + paymentId;
                System.IO.File.WriteAllBytes(Folder + fileName + ".pdf", bytes);

                HttpContext.Current.Session["FolderName"] = Folder;
                HttpContext.Current.Session["FolderZoneCode"] = zone;

            }
        }
        private static DataTable getUpperData(string Check_Condition1)
        {
            Variable clvar = new Variable();
            DataTable ds = new DataTable();
            try
            {
                string sql = "SELECT DISTINCT cc.accountNo, \n"
               + "       cc.name clientname, \n"
               + "       b.name                  City, \n"
               + "       ir.InvoiceNo Invoicenumber, \n"
               + "       MIN(pv.VoucherDate)     MinPaymentPeriod, \n"
               + "       MAX(pv.VoucherDate)     MaxPaymentPeriod, cc.BeneficiaryName \n"
               + "FROM   PaymentVouchers pv \n"
               + "       LEFT OUTER JOIN InvoiceRedeem ir \n"
               + "            ON  ir.PaymentVoucherId = pv.Id \n"
               + "       LEFT OUTER JOIN Invoice i \n"
               + "            ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  cc.id = pv.creditclientId \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.branchCode = cc.branchCode \n"
               + "WHERE  pv.id in (" + Check_Condition1 + ") \n"
               + "GROUP BY \n"
               + "       cc.accountNo, \n"
               + "       cc.name, \n"
               + "       b.name, cc.BeneficiaryName,\n"
               + "       ir.InvoiceNo \n"
               + "";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
        private static DataTable GetRTSData(string Check_Condition1)
        {
            Variable clvar = new Variable();

            DataTable dt = new DataTable();
            try
            {

                string sqlString = "SELECT ROW_NUMBER() OVER(order by b.consignmentNumber) Sr, b.consignmentNumber,\n" +
                "       b.orderRefNo,\n" +
                "       convert(varchar(11),b.bookingDate,106) bookingDate,\n" +
                "       b.consignee,\n" +
                "       b.origin,\n" +
                "       b.[weight],\n" +
                "       b.destination,\n" +
                "       SUM(b.CODAMOUNT)           CODAMOUNT,\n" +
                "       b.CNStatus,\n" +
                "       b.transactionNumber        transactionNumber,\n" +
                "       SUM(b.ShippingCharges)     ShippingCharges\n" +
                "FROM   (\n" +
                "           SELECT c.consignmentNumber,\n" +
                "                  cd.orderRefNo,\n" +
                "                  c.bookingDate,\n" +
                "                  c.consignee,\n" +
                "                  b.sname                  origin,\n" +
                "                  b2.sname                  destination,\n" +
                "                  c.[weight],\n" +
                "                  --SUM(cd.codAmount) / COUNT(cd.consignmentNumber) CODAMOUNT,\n" +
                "                  --cd.codAmount,\n" +
                "                  pv.Amount                CODAMOUNT,\n" +
                "                  l.AttributeValue         CNStatus,\n" +
                "                  ic.consignmentAmount     ShippingCharges,\n" +
                "                  transactionNumber\n" +
                "           FROM   Consignment c\n" +
                "                  INNER JOIN Branches b\n" +
                "                       ON  b.branchCode = c.orgin\n" +
                "                  INNER JOIN Branches b2\n" +
                "                       ON  b2.branchCode = c.destination\n" +
                "                  INNER JOIN CODConsignmentDetail_New cd\n" +
                "                       ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "                  INNER JOIN PaymentVouchers pv\n" +
                "                       ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "                  LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "                       ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "                  LEFT OUTER JOIN Invoice i\n" +
                "                       ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "                       AND i.IsInvoiceCanceled = '0'\n" +
                "                  LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "                       ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "                       AND rc.createdOn = (\n" +
                "                               SELECT MAX(RC2.createdOn) statusdate\n" +
                "                               FROM   RunsheetConsignment RC2\n" +
                "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                           )\n" +
                "                  LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "                       ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "                       AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "   WHERE pv.id in (" + Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
                "      --and c.consignerAccountNo = '4G7'\n" +
                "           GROUP BY\n" +
                "                  c.consignmentNumber,\n" +
                "                  cd.orderRefNo,\n" +
                "                  c.bookingDate,\n" +
                "                  c.consignee,\n" +
                "                  b.sname,\n" +
                "                  b2.sname,\n" +
                "                  c.[weight],\n" +
                "                  pv.Amount,\n" +
                "                  l.AttributeValue,\n" +
                "                  ic.consignmentAmount,\n" +
                "                  transactionNumber\n" +
                "       )                          b\n" +
                "GROUP BY\n" +
                "       b.consignmentNumber,\n" +
                "       b.orderRefNo,\n" +
                "       convert(varchar(11),b.bookingDate,106),\n" +
                "       b.consignee,\n" +
                "       b.origin,\n" +
                "       b.destination,\n" +
                "       b.CNStatus,\n" +
                "       b.[weight],\n" +
                "       b.transactionNumber";

                string sql = "SELECT ISNULL(SUM(ISNULL(cdn.codAmount, 0)),0) RTSAmount FROM CODConsignmentDetail_New cdn \n"
               + "       INNER JOIN PaymentVouchers pv \n"
               + "       ON pv.ConsignmentNo = cdn.consignmentNumber \n"
               + "       INNER JOIN RunsheetConsignment rc \n"
               + "       ON rc.consignmentNumber = cdn.consignmentNumber \n"
               + "       AND rc.createdOn = ( \n"
               + "                               SELECT MAX(RC2.createdOn) statusdate \n"
               + "                               FROM   RunsheetConsignment RC2 \n"
               + "                               WHERE  RC2.consignmentNumber = rc.consignmentNumber \n"
               + "       ) \n"
               + "       WHERE rc.Reason = '59' \n"
               + "       AND pv.id in (" + Check_Condition1 + ")";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return dt;
        }
        private static DataTable CenterTableRight_Data(string invoiceNumbers, string Check_Condition1)
        {
            Variable clvar = new Variable();

            DataTable ds = new DataTable();

            try
            {

                if (invoiceNumbers == "")
                {
                    invoiceNumbers = "''";
                }
                string sqlString = "\n" +
                "SELECT SUM(shipmentCharge) shipmentCharge,\n" +
                "SUM(FUELCHARGE) FUEL_CHARGE,\n" +
                "SUM(EXTRACHARGES) EXTRACHARGES,\n" +
                "SUM(EXTRACHARGESGST) + SUM(FUELCHARGEGST) GST\n" +
                "\n" +
                "FROM\n" +
                "(\n" +
                "SELECT sum(c.totalAmount) shipmentCharge,\n" +
                "\n" +
                "(SELECT ISNULL(SUM(IPM.CALCULATEDAMOUNT), 0)\n" +
                "   FROM INVOICEPRICEMODIFIERASSOCIATION IPM\n" +
                "  WHERE IPM.INVOICENO = ic.INVOICENUMBER\n" +
                "  AND IPM.MODIFIERTYPE = '1'\n" +
                ") FUELCHARGE,\n" +
                "\n" +
                "(SELECT ISNULL(SUM(IPM.CALCULATEDGST), 0)\n" +
                "   FROM INVOICEPRICEMODIFIERASSOCIATION IPM\n" +
                "  WHERE IPM.INVOICENO = ic.INVOICENUMBER\n" +
                "  AND IPM.MODIFIERTYPE = '1'\n" +
                ") FUELCHARGEGST,\n" +
                "\n" +
                "(SELECT ISNULL(SUM(CM.CALCULATEDVALUE), 0)\n" +
                "   FROM CONSIGNMENTMODIFIER AS CM\n" +
                "  WHERE CM.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER) EXTRACHARGES,\n" +
                "(SELECT ISNULL(SUM(CM.CALCULATEDGST), 0)\n" +
                "   FROM CONSIGNMENTMODIFIER AS CM\n" +
                "  WHERE CM.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER) EXTRACHARGESGST\n" +
                "\n" +
                "\n" +
                "FROM   Consignment c\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  b.branchCode = c.orgin\n" +
                "       INNER JOIN Branches b2\n" +
                "            ON  b2.branchCode = c.destination\n" +
                "       INNER JOIN CODConsignmentDetail_New cd\n" +
                "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "       INNER JOIN CreditClients cc\n" +
                "            ON  c.creditClientId = cc.id\n" +
                "       INNER JOIN Branches b3\n" +
                "            ON  cc.branchCode = b3.branchCode\n" +
                "       INNER JOIN PaymentVouchers pv\n" +
                "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN Invoice i\n" +
                "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "            AND i.IsInvoiceCanceled = '0'\n" +
                "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "            AND rc.createdOn = (\n" +
                "                    SELECT MAX(RC2.createdOn) statusdate\n" +
                "                    FROM   RunsheetConsignment RC2\n" +
                "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                )\n" +
                "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "WHERE  pv.id in (" + Check_Condition1 + ") --c.transactionNumber = '00000162'\n" +
                "       --AND c.consignerAccountNo = '4a260'\n" +
                "GROUP BY\n" +
                "ic.INVOICENUMBER, C.CONSIGNMENTNUMBER\n" +
                ") b";

                string sql = "SELECT SUM(c.totalAmount) shipmentCharge, \n"
               + "       SUM(ISNULL(ipma.calculatedAmount, 0)) FUEL_CHARGE, \n"
               + "       SUM(ISNULL(ipma.calculatedGST, 0)) EXTRACHARGES, \n"
               + "       SUM(ISNULL(ipma.calculatedAmount, 0)) + SUM(ISNULL(ipma.calculatedGST, 0))  \n"
               + "       GST \n"
               + "FROM   PaymentVouchers pv \n"
               + "       left outer JOIN InvoiceRedeem ir \n"
               + "            ON  ir.PaymentVoucherId = pv.Id \n"
               + "       INNER JOIN invoice i \n"
               + "            ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "            AND i.IsInvoiceCanceled = '0' \n"
               + "       INNER JOIN InvoiceConsignment ic \n"
               + "            ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "       INNER JOIN Consignment c \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       LEFT OUTER JOIN InvoicePriceModifierAssociation ipma \n"
               + "            ON  ipma.InvoiceNo = i.invoiceNumber \n"
               + "            AND ipma.modifierType = '1'\n"
               + "Where pv.id in (" + Check_Condition1 + ")";



                sql = "SELECT ISNULL(SUM(A.ShippingCharges), 0)     shipmentCharge, \n"
               + "       ISNULL(SUM(A.GST), 0)                 GST, \n"
               + "       ISNULL(SUM(A.FuelSurcharge), 0)       FUEL_CHARGE, \n"
               + "       ISNULL(SUM(A.ExtraCharges), 0)        EXTRACHARGES \n"
               + "FROM   ( \n"
               + "           SELECT ROUND(SUM(ISNULL(c.totalAmount, 0)), 2) ShippingCharges, \n"
               + "                  ROUND(SUM(ISNULL(c.gst, 0)), 2) GST, \n"
               + "                  0     FuelSurcharge, \n"
               + "                  0     ExtraCharges \n"
               + "           FROM   Invoice i \n"
               + "                  INNER JOIN InvoiceConsignment ic \n"
               + "                       ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "                  INNER JOIN Consignment c \n"
               + "                       ON  c.consignmentNumber = ic.consignmentNumber \n"
               //+ "                  LEFT OUTER JOIN InvoicePriceModifierAssociation ip \n"
               //+ "                       ON  ip.InvoiceNo = i.invoiceNumber \n"
               //+ "                       AND ip.modifierType = '1' \n"
               //+ "                       AND ip.calculatedAmount > 0 \n"
               + "           WHERE  i.invoiceNumber in (" + invoiceNumbers + ") \n"
               + "            \n"
               + "           UNION  \n"
               + "            \n"
               + "           SELECT 0     ShippingCharges, \n"
               + "                  ROUND(SUM(ISNULL(ip.calculatedGST, 0)), 2) GST, \n"
               + "                  ROUND(SUM(ISNULL(ip.calculatedAmount, 0)), 2) FuelSurcharge, \n"
               + "                  0     ExtraCharges \n"
               + "           FROM   InvoicePriceModifierAssociation ip \n"
               + "           WHERE  ip.InvoiceNo in (" + invoiceNumbers + ") \n"
               //+ "                  AND ip.modifierType = '1' \n"
               + "            \n"
               + "           UNION \n"
               + "            \n"
               + "           SELECT 0     ShippingCharges, \n"
               + "                  ROUND(SUM(ISNULL(cm.calculatedGST, 0)), 2) GST, \n"
               + "                  0     FuelSurcharge, \n"
               + "                  ROUND(SUM(ISNULL(cm.calculatedValue, 0)), 2) ExtraCharges \n"
               + "           FROM   ConsignmentModifier cm \n"
               + "                  INNER JOIN InvoiceConsignment ic \n"
               + "                       ON  ic.consignmentNumber = cm.consignmentNumber \n"
               + "           WHERE  ic.invoiceNumber in (" + invoiceNumbers + ") \n"
               + "       )                          A \n"
               + "";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
        private static DataTable CenterTableLeft_Data(string invoiceNumbers, string Check_Condition1)
        {
            Variable clvar = new Variable();

            DataTable ds = new DataTable();

            try
            {

                if (invoiceNumbers == "")
                {
                    invoiceNumbers = "''";
                }

                string sqlString = "\n" +
                "SELECT SUM(cd.codAmount) TotalCODAmount, SUM(ic.consignmentAmount) invoiceAmount,\n" +
                "SUM(cd.codAmount) - SUM(ic.consignmentAmount) NetPayable\n" +
                "\n" +
                "FROM   Consignment c\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  b.branchCode = c.orgin\n" +
                "       INNER JOIN Branches b2\n" +
                "            ON  b2.branchCode = c.destination\n" +
                "       INNER JOIN CODConsignmentDetail_New cd\n" +
                "            ON  cd.consignmentNumber = c.consignmentNumber\n" +
                "       INNER JOIN CreditClients cc\n" +
                "            ON  c.creditClientId = cc.id\n" +
                "       INNER JOIN Branches b3\n" +
                "            ON  cc.branchCode = b3.branchCode\n" +
                "       INNER JOIN PaymentVouchers pv\n" +
                "            ON  pv.ConsignmentNo = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN InvoiceConsignment ic\n" +
                "            ON  ic.consignmentNumber = c.consignmentNumber\n" +
                "       LEFT OUTER JOIN Invoice i\n" +
                "            ON  i.invoiceNumber = ic.invoiceNumber\n" +
                "            AND i.IsInvoiceCanceled = '0'\n" +
                "       LEFT OUTER JOIN RunsheetConsignment rc\n" +
                "            ON  rc.consignmentNumber = c.consignmentNumber\n" +
                "            AND rc.createdOn = (\n" +
                "                    SELECT MAX(RC2.createdOn) statusdate\n" +
                "                    FROM   RunsheetConsignment RC2\n" +
                "                    WHERE  RC2.consignmentNumber = rc.consignmentNumber\n" +
                "                )\n" +
                "       LEFT OUTER JOIN rvdbo.Lookup l\n" +
                "            ON  l.AttributeGroup = 'POD_STATUS'\n" +
                "            AND CAST(l.Id AS VARCHAR) = rc.[Status]\n" +
                "\n" +
                "\n" +
                "\n" +
                "WHERE  pv.id in (" + Check_Condition1 + ") --c.transactionNumber = '999999'\n" +
                "       --AND c.consignerAccountNo = '4G7'";

                string sql = "SELECT SUM(pv.amount)         TotalCODAmount, \n"
               + "       SUM(i.totalAmount)     invoiceAmount, \n"
               + "       (SUM(pv.Amount) - SUM(i.totalAmount)) NetPayable \n"
               + "FROM   PaymentVouchers pv \n"
               + "       left outer JOIN InvoiceRedeem ir \n"
               + "            ON  ir.PaymentVoucherId = pv.Id \n"
               + "       left outer JOIN Invoice i \n"
               + "            ON  i.invoiceNumber = ir.InvoiceNo \n"
               + "WHERE  pv.id in (" + Check_Condition1 + ")";


                sql = "SELECT SUM(A.CODAmount)         TotalCODAmount, \n"
              + "       SUM(A.InvoiceAmount)     InvoiceAmount, \n"
              + "       (SUM(A.CODAmount) - SUM(A.InvoiceAmount)) NETPayable \n"
              + "FROM   ( \n"
              + "           SELECT SUM(pv.Amount)      CODAmount, \n"
              + "                  0                   InvoiceAmount \n"
              + "           FROM   PaymentVouchers     pv \n"
              + "           WHERE  pv.Id IN (" + Check_Condition1 + ") \n"
              + "            \n"
              + "           UNION \n"
              + "            \n"
              + "           SELECT 0                      CODAmount, \n"
              + "                  SUM(i.totalAmount)     InvoiceAmount \n"
              + "           FROM   Invoice                i \n"
              + "           WHERE  i.invoiceNumber IN (" + invoiceNumbers + ") \n"
              + "       )                        A";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandTimeout = 30000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
        protected void DownloadFiles(object sender, EventArgs e)
        {
            if (Session["FolderName"] != null)
            {
                Response.Redirect("~/CODPaymentsZip/download");
            }

        }
    }
}