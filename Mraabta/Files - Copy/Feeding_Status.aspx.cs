using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class Feeding_Status : System.Web.UI.Page
    {
        #region Variable


        Variable clvar = new Variable();

        string StartDate = string.Empty;
        string BranchName = string.Empty;
        string Month = string.Empty;
        string Year = string.Empty;

        decimal total_retail_sale = 0M;
        decimal total_cash_recovery = 0M;
        decimal total_cod = 0M;
        decimal total_misc_receipt = 0M;
        decimal total_cod_deposite = 0M;
        decimal total_noncod_deposite = 0M;
        decimal total_difference = 0M;

        string TopHeader1 = "RECEIPTS";
        string TopHeader2 = "DEPOSITS";
        string TopHeader3 = "RECEIPTS - DEPOSITS";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StartDate = Request.QueryString["date"];
                BranchName = Request.QueryString["branchname"];

                hf_Date.Value = StartDate;
                hf_Branch_Name.Value = BranchName;

                DateTime Month_Year = DateTime.Parse(hf_Date.Value.ToString());

                Month = Month_Year.ToString("MM");
                Year = Month_Year.ToString("yyyy");

                hf_Month.Value = Month;
                hf_Year.Value = Year;

                Title = "Feeding Status (" + BranchName + " - " + Month_Year.ToString("MMMM yyyy") + ")";

                GenerateReport();
            }

        }

        public void GenerateReport()
        {
            DateTime Month_Year = DateTime.Parse(hf_Date.Value.ToString());

            DataTable dt = Get_Feeding_Status(clvar).Tables[0];
            if (dt.Rows.Count > 0)
            {
                btn_csv.Visible = true;
                btn_Excel.Visible = true;
                lbl_report_name.Text = "Feeding Status Report (" + BranchName + " - " + Month_Year.ToString("MMMM yyyy") + ")";
                lbl_total_record.Text = "Total Records: " + dt.Rows.Count.ToString();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string RowData = dt.Rows[i][0].ToString();      //  GET DATE COLUMN DATA
                    DateTime DateRowData = DateTime.ParseExact(RowData, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    dt.Rows[i][0] = DateRowData.ToString("dd MMM yyyy");
                    dt.AcceptChanges();
                }

                GridViewFeeding_Status.DataSource = dt;
                GridViewFeeding_Status.DataBind();

            }
            else
            {
                btn_csv.Visible = false;
                btn_Excel.Visible = false;
                lbl_report_name.Text = "";
                lbl_total_record.Text = "";
                lbl_Message.Text = "There is No data to show!";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
            }


        }

        public DataSet Get_Feeding_Status(Variable clvar)
        {
            //  Get Branch ID Related to Branch Name First Then Run Feeding Status Query
            string Branch_ID = Get_Branch_ID(clvar).Tables[0].Rows[0][0].ToString();

            DataSet ds = new DataSet();
            string sqlString = "";

            try
            {
                #region SELECT QUERY OF SQL 24-09-2019


                string sqlString___24092019 = "SELECT \n"
               + "---- Feeding Status (PC Closing Balance Print Detail) \n"
               + "-- " + Session["U_NAME"].ToString() + " \n"
               + "       Z.DATE AS 'DATE', \n"
               + "--       CAST(VARCHAR(11), Z.DATE, 106) AS 'DATE', \n"
               + "       ISNULL(SUM(Z.RETAIL_SALE), 0)         RETAIL_SALE, \n"
               + "       ISNULL(SUM(Z.CASH_RECOVERY), 0)       CASH_RECOVERY, \n"
               + "       ISNULL(SUM(z.COD), 0)                 COD, \n"
               + "       ISNULL(SUM(z.MISC_RECEIPT), 0)        MISC_RECEIPT, \n"
               + "       ISNULL(SUM(Z.COD_DEPOSITE), 0)        COD_DEPOSITE, \n"
               + "       ISNULL(SUM(Z.DEPOSITE_NONCOD), 0)     DEPOSITE_NONCOD, \n"
               + "       ( \n"
               + "           ( \n"
               + "               ISNULL(SUM(Z.RETAIL_SALE), 0) + \n"
               + "               ISNULL(SUM(Z.CASH_RECOVERY), 0) + \n"
               + "               ISNULL(SUM(z.COD), 0) + \n"
               + "               ISNULL(SUM(z.MISC_RECEIPT), 0) \n"
               + "           ) \n"
               + "           -( \n"
               + "               ISNULL(SUM(Z.COD_DEPOSITE), 0) + \n"
               + "               ISNULL(SUM(Z.DEPOSITE_NONCOD), 0) \n"
               + "           ) \n"
               + "       ) AS 'DIFFERENCE' \n"
               + "FROM   ( \n"
               + "           SELECT X.DATE, \n"
               + "                  ISNULL(SUM(RETAIL_SALE), 0) RETAIL_SALE, \n"
               + "                  ISNULL(SUM(CASH_RECOVERY), 0) CASH_RECOVERY, \n"
               + "                  ISNULL(SUM(COD), 0)     COD, \n"
               + "                  ISNULL(SUM(MISC_RECEIPT), 0) MISC_RECEIPT, \n"
               + "                  0                       COD_DEPOSITE, \n"
               + "                  0                       DEPOSITE_NONCOD \n"
               + "           FROM   ( \n"
               + "                      SELECT CONVERT(VARCHAR, A.DATE, 103) DATE, \n"
               + "                             CASE  \n"
               + "                                  WHEN A.NAME = 'EXP CENTER SALE' THEN SUM(DEBIT) \n"
               + "                             END     RETAIL_SALE, \n"
               + "                             CASE WHEN A.NAME = 'PAYMENT' THEN SUM(DEBIT) END CASH_RECOVERY, \n"
               + "                             CASE  \n"
               + "                                  WHEN A.NAME = 'COD PAYMENT' THEN SUM(DEBIT) \n"
               + "                             END     COD, \n"
               + "                             CASE  \n"
               + "                                  WHEN A.NAME NOT IN ('COD PAYMENT', 'EXP CENTER SALE','PAYMENT') THEN  \n"
               + "                                       SUM(DEBIT) \n"
               + "                             END     MISC_RECEIPT, \n"
               + "                             0       COD_DEPOSITE, \n"
               + "                             0       DEPOSITE_NONCOD \n"
               + "                      FROM   ( \n"
               + "                                 SELECT '1' RANK, \n"
               + "                                        SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0))  \n"
               + "                                        DEBIT, \n"
               + "                                        0 CREDIT, \n"
               + "                                        CASE  \n"
               + "                                             WHEN PV.REFNO IS NULL \n"
               + "                                 OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')'  \n"
               + "                                    + ' REF NO# ' + PV.REFNO END CASHTYPE, \n"
               + "                                 PT.NAME, \n"
               + "                                 VOUCHERDATE DATE, \n"
               + "                                 B.[NAME] BRANCH, \n"
               + "                                 '' COMPANY \n"
               + "                                 FROM PAYMENTVOUCHERS PV \n"
               + "                                 LEFT OUTER JOIN PAYMENTTYPES PT \n"
               + "                                 ON PT.ID = PV.PAYMENTTYPEID \n"
               + "                                 LEFT OUTER JOIN ( \n"
               + "                                     SELECT MP.VOUCHERID, \n"
               + "                                            SUM(MP.AMOUNT) PRODUCTAMOUNT \n"
               + "                                     FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN  \n"
               + "                                            MP \n"
               + "                                     WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD') \n"
               + "                                     GROUP BY \n"
               + "                                            MP.VOUCHERID \n"
               + "                                 ) MM \n"
               + "                                 ON MM.VOUCHERID = PV.ID \n"
               + "                                 INNER JOIN BRANCHES B \n"
               + "                                 ON B.BRANCHCODE = PV.BRANCHCODE \n"
               + "                                 WHERE PV.BRANCHCODE = '" + Branch_ID + "' \n"
               //+ "                                 AND PV.VOUCHERDATE = '" + hf_Date.Value.ToString() + "' \n"
               + "                                 AND MONTH(PV.VOUCHERDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                                 AND YEAR(PV.VOUCHERDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                                 AND PV.CASHPAYMENTSOURCE IS NOT NULL \n"
               + "                                 AND (PV.PAYMENTSOURCEID IS NULL OR PV.PAYMENTSOURCEID = '1') \n"
               + "                                 AND PV.CASHPAYMENTSOURCE != '4' \n"
               + "                                     GROUP BY \n"
               + "                                     VOUCHERDATE, \n"
               + "                                 PV.REFNO, \n"
               + "                                 PT.NAME, \n"
               + "                                 B.[NAME] \n"
               + "                                  \n"
               + "                                  \n"
               + "                                 UNION ALL \n"
               + "                                  \n"
               + "                                  \n"
               + "                                 SELECT '1' RANK, \n"
               + "                                        SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0))  \n"
               + "                                        DEBIT, \n"
               + "                                        0 CREDIT, \n"
               + "                                        CASE  \n"
               + "                                             WHEN PV.REFNO IS NULL \n"
               + "                                 OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')'  \n"
               + "                                    + ' REF NO# ' + PV.REFNO END CASHTYPE, \n"
               + "                                 PT.NAME, \n"
               + "                                 VOUCHERDATE DATE, \n"
               + "                                 B.[NAME] BRANCH, \n"
               + "                                 '' COMPANY \n"
               + "                                 FROM PAYMENTVOUCHERS PV \n"
               + "                                 LEFT OUTER JOIN ( \n"
               + "                                     SELECT MP.VOUCHERID, \n"
               + "                                            SUM(MP.AMOUNT) PRODUCTAMOUNT \n"
               + "                                     FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN  \n"
               + "                                            MP \n"
               + "                                     WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD') \n"
               + "                                     GROUP BY \n"
               + "                                            MP.VOUCHERID \n"
               + "                                 ) MM \n"
               + "                                 ON MM.VOUCHERID = PV.ID \n"
               + "                                 LEFT OUTER JOIN PAYMENTTYPES PT \n"
               + "                                 ON PT.ID = PV.PAYMENTTYPEID \n"
               + "                                 INNER JOIN BRANCHES B \n"
               + "                                 ON B.BRANCHCODE = PV.BRANCHCODE \n"
               + "                                 WHERE PV.BRANCHCODE = '" + Branch_ID + "' \n"
               //+ "                                 AND PV.VOUCHERDATE = '" + hf_Date.Value.ToString() + "' \n"
               + "                                 AND MONTH(PV.VOUCHERDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                                 AND YEAR(PV.VOUCHERDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                                 AND PV.CREDITCLIENTID IS NOT NULL \n"
               + "                                 AND PV.PAYMENTSOURCEID = '1' \n"
               + "                                     GROUP BY \n"
               + "                                     VOUCHERDATE, \n"
               + "                                 PV.REFNO, \n"
               + "                                 PT.NAME, \n"
               + "                                 B.[NAME] \n"
               + "                                 UNION ALL \n"
               + "                                  \n"
               + "                                  \n"
               + "                                 SELECT '1' RANK, \n"
               + "                                        SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0))  \n"
               + "                                        DEBIT, \n"
               + "                                        0 CREDIT, \n"
               + "                                        CASE  \n"
               + "                                             WHEN PV.REFNO IS NULL \n"
               + "                                 OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')'  \n"
               + "                                    + ' REF NO# ' + PV.REFNO END CASHTYPE, \n"
               + "                                 PT.NAME, \n"
               + "                                 VOUCHERDATE DATE, \n"
               + "                                 B.[NAME] BRANCH, \n"
               + "                                 '' COMPANY \n"
               + "                                 FROM PAYMENTVOUCHERS PV \n"
               + "                                 LEFT OUTER JOIN ( \n"
               + "                                     SELECT MP.VOUCHERID, \n"
               + "                                            SUM(MP.AMOUNT) PRODUCTAMOUNT \n"
               + "                                     FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN  \n"
               + "                                            MP \n"
               + "                                     WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD') \n"
               + "                                     GROUP BY \n"
               + "                                            MP.VOUCHERID \n"
               + "                                 ) MM \n"
               + "                                 ON MM.VOUCHERID = PV.ID \n"
               + "                                 LEFT OUTER JOIN PAYMENTTYPES PT \n"
               + "                                 ON PT.ID = PV.PAYMENTTYPEID \n"
               + "                                 INNER JOIN BRANCHES B \n"
               + "                                 ON B.BRANCHCODE = PV.BRANCHCODE \n"
               + "                                 WHERE PV.BRANCHCODE = '" + Branch_ID + "' \n"
               //+ "                                 AND PV.VOUCHERDATE = '" + hf_Date.Value.ToString() + "' \n"
               + "                                 AND MONTH(PV.VOUCHERDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                                 AND YEAR(PV.VOUCHERDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                                 AND PV.CREDITCLIENTID IS NULL \n"
               + "                                 AND PV.CLIENTGROUPID IS NOT NULL \n"
               + "                                 AND PV.PAYMENTSOURCEID = '1' \n"
               + "                                     GROUP BY \n"
               + "                                     VOUCHERDATE, \n"
               + "                                 PV.REFNO, \n"
               + "                                 PT.NAME, \n"
               + "                                 B.[NAME] \n"
               + "                             )       A \n"
               + "                      GROUP BY \n"
               + "                             CONVERT(VARCHAR, A.DATE, 103), \n"
               + "                             A.NAME \n"
               + "                  )                       X \n"
               + "           GROUP BY \n"
               + "                  X.DATE, \n"
               + "                  x.COD_DEPOSITE, \n"
               + "                  x.DEPOSITE_NONCOD \n"
               + "            \n"
               + "           UNION ALL \n"
               + "            \n"
               + "           SELECT X.RRDATE  AS DATE, \n"
               + "                  0                RETAIL_SALE, \n"
               + "                  0                CASH_RECOVERY, \n"
               + "                  0                COD, \n"
               + "                  0                MISC_RECEIPT, \n"
               + "                  ISNULL(SUM(COD_DEPOSITE), 0) COD_DEPOSITE, \n"
               + "                  ISNULL(SUM(DEPOSITE_NONCOD), 0) DEPOSITE_NONCOD \n"
               + "           FROM   ( \n"
               + "                      SELECT CONVERT(VARCHAR, B.RRDATE, 103) RRDATE, \n"
               + "                             0       RETAIL_SALE, \n"
               + "                             0       CASH_RECOVERY, \n"
               + "                             0       COD, \n"
               + "                             0       MISC_RECEIPT, \n"
               + "                             CASE  \n"
               + "                                  WHEN B.NATUREDEPOSIT = '01' THEN SUM(AMOUNT) \n"
               + "                             END     COD_DEPOSITE, \n"
               + "                             CASE  \n"
               + "                                  WHEN B.NATUREDEPOSIT = '02' THEN SUM(AMOUNT) \n"
               + "                             END     DEPOSITE_NONCOD \n"
               + "                      FROM   DEPOSIT_TO_BANK B \n"
               //+ "                      WHERE  CAST(B.CHEQUEDATE AS DATE) = '" + hf_Date.Value.ToString() + "' \n"
               + "                      WHERE  MONTH(B.RRDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                      AND YEAR(B.RRDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                      and b.branchcode = '" + Branch_ID + "' \n"
               + "                      GROUP BY \n"
               + "                             B.RRDATE, \n"
               + "                             B.NATUREDEPOSIT \n"
               + "                  )                X \n"
               + "           GROUP BY \n"
               + "                  X.RRDATE, \n"
               + "                  X.CASH_RECOVERY, \n"
               + "                  X.COD, \n"
               + "                  X.MISC_RECEIPT \n"
               + "       )                                     Z \n"
               + "GROUP BY \n"
               + "       z.DATE \n"
               + "ORDER BY \n"
               + "       1 \n";


                #endregion

                #region SELECT QUERY OF SQL


                sqlString = "SELECT \n"
               + "---- Feeding Status (PC Closing Balance Print Detail) \n"
               + "-- " + Session["U_NAME"].ToString() + " \n"
               + "       Z.DATE AS 'DATE', \n"
               + "--       CAST(VARCHAR(11), Z.DATE, 106) AS 'DATE', \n"
               + "       ISNULL(SUM(Z.RETAIL_SALE), 0)         RETAIL_SALE, \n"
               + "       ISNULL(SUM(Z.CASH_RECOVERY), 0)       CASH_RECOVERY, \n"
               + "       ISNULL(SUM(z.COD), 0)                 COD, \n"
               + "       ISNULL(SUM(z.MISC_RECEIPT), 0)        MISC_RECEIPT, \n"
               + "       ISNULL(SUM(Z.COD_DEPOSITE), 0)        COD_DEPOSITE, \n"
               + "       ISNULL(SUM(Z.DEPOSITE_NONCOD), 0)     DEPOSITE_NONCOD, \n"
               + "       ( \n"
               + "           ( \n"
               + "               ISNULL(SUM(Z.RETAIL_SALE), 0) + \n"
               + "               ISNULL(SUM(Z.CASH_RECOVERY), 0) + \n"
               + "               ISNULL(SUM(z.COD), 0) + \n"
               + "               ISNULL(SUM(z.MISC_RECEIPT), 0) \n"
               + "           ) \n"
               + "           -( \n"
               + "               ISNULL(SUM(Z.COD_DEPOSITE), 0) + \n"
               + "               ISNULL(SUM(Z.DEPOSITE_NONCOD), 0) \n"
               + "           ) \n"
               + "       ) AS 'DIFFERENCE' \n"
               + "FROM   ( \n"
               + "           SELECT X.DATE, \n"
               + "                  ISNULL(SUM(RETAIL_SALE), 0) RETAIL_SALE, \n"
               + "                  ISNULL(SUM(CASH_RECOVERY), 0) CASH_RECOVERY, \n"
               + "                  ISNULL(SUM(COD), 0)     COD, \n"
               + "                  ISNULL(SUM(MISC_RECEIPT), 0) MISC_RECEIPT, \n"
               + "                  0                       COD_DEPOSITE, \n"
               + "                  0                       DEPOSITE_NONCOD \n"
               + "           FROM   ( \n"
               + "                      SELECT distinct CONVERT(VARCHAR, A.DATE, 103) DATE,A.NAME, \n"
               + "                             CASE  \n"
               + "                                  WHEN A.NAME = 'EXP CENTER SALE' THEN DEBIT \n"
               + "                             END     RETAIL_SALE, \n"
               + "                             CASE WHEN A.NAME = 'PAYMENT' THEN DEBIT END CASH_RECOVERY, \n"
               + "                             CASE  \n"
               + "                                  WHEN A.NAME = 'COD PAYMENT' THEN DEBIT \n"
               + "                             END     COD, \n"
               + "                             CASE  \n"
               + "                                 WHEN A.NAME = 'License Fee' THEN sum(DEBIT) \n"
               + "                                  WHEN A.NAME NOT IN ('COD PAYMENT', 'EXP CENTER SALE','PAYMENT') THEN  DEBIT \n"
               + "                             END     MISC_RECEIPT, \n"
               + "                             0       COD_DEPOSITE, \n"
               + "                             0       DEPOSITE_NONCOD, CASHTYPE \n"
               + "                      FROM   ( \n"
               + "                                 SELECT '1' RANK, \n"
               + "                                        SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0))  \n"
               + "                                        DEBIT, \n"
               + "                                        0 CREDIT, \n"
               + "                                        CASE  \n"
               + "                                             WHEN PV.REFNO IS NULL \n"
               + "                                 OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')'  \n"
               + "                                    + ' REF NO# ' + PV.REFNO END CASHTYPE, \n"
               + "                                 PT.NAME, \n"
               + "                                 VOUCHERDATE DATE, \n"
               + "                                 B.[NAME] BRANCH, \n"
               + "                                 '' COMPANY \n"
               + "                                 FROM PAYMENTVOUCHERS PV \n"
               + "                                 LEFT OUTER JOIN PAYMENTTYPES PT \n"
               + "                                 ON PT.ID = PV.PAYMENTTYPEID \n"
               + "                                 LEFT OUTER JOIN ( \n"
               + "                                     SELECT MP.VOUCHERID, \n"
               + "                                            SUM(MP.AMOUNT) PRODUCTAMOUNT \n"
               + "                                     FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN  \n"
               + "                                            MP \n"
               + "                                     WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD') \n"
               + "                                     GROUP BY \n"
               + "                                            MP.VOUCHERID \n"
               + "                                 ) MM \n"
               + "                                 ON MM.VOUCHERID = PV.ID \n"
               + "                                 INNER JOIN BRANCHES B \n"
               + "                                 ON B.BRANCHCODE = PV.BRANCHCODE \n"
               + "                                 WHERE PV.BRANCHCODE = '" + Branch_ID + "' \n"
               //+ "                                 AND PV.VOUCHERDATE = '" + hf_Date.Value.ToString() + "' \n"
               + "                                 AND MONTH(PV.VOUCHERDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                                 AND YEAR(PV.VOUCHERDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                                 AND PV.CASHPAYMENTSOURCE IS NOT NULL \n"
               + "                                 AND (PV.PAYMENTSOURCEID IS NULL OR PV.PAYMENTSOURCEID = '1') \n"
               + "                                 AND PV.CASHPAYMENTSOURCE != '4' \n"
               + "                                     GROUP BY \n"
               + "                                     VOUCHERDATE, \n"
               + "                                 PV.REFNO, \n"
               + "                                 PT.NAME, \n"
               + "                                 B.[NAME] \n"
               + "                                  \n"
               + "                                  \n"
               + "                                 UNION ALL \n"
               + "                                  \n"
               + "                                  \n"
               + "                                 SELECT '1' RANK, \n"
               + "                                        SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0))  \n"
               + "                                        DEBIT, \n"
               + "                                        0 CREDIT, \n"
               + "                                        CASE  \n"
               + "                                             WHEN PV.REFNO IS NULL \n"
               + "                                 OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')'  \n"
               + "                                    + ' REF NO# ' + PV.REFNO END CASHTYPE, \n"
               + "                                 PT.NAME, \n"
               + "                                 VOUCHERDATE DATE, \n"
               + "                                 B.[NAME] BRANCH, \n"
               + "                                 '' COMPANY \n"
               + "                                 FROM PAYMENTVOUCHERS PV \n"
               + "                                 LEFT OUTER JOIN ( \n"
               + "                                     SELECT MP.VOUCHERID, \n"
               + "                                            SUM(MP.AMOUNT) PRODUCTAMOUNT \n"
               + "                                     FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN  \n"
               + "                                            MP \n"
               + "                                     WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD') \n"
               + "                                     GROUP BY \n"
               + "                                            MP.VOUCHERID \n"
               + "                                 ) MM \n"
               + "                                 ON MM.VOUCHERID = PV.ID \n"
               + "                                 LEFT OUTER JOIN PAYMENTTYPES PT \n"
               + "                                 ON PT.ID = PV.PAYMENTTYPEID \n"
               + "                                 INNER JOIN BRANCHES B \n"
               + "                                 ON B.BRANCHCODE = PV.BRANCHCODE \n"
               + "                                 WHERE PV.BRANCHCODE = '" + Branch_ID + "' \n"
               //+ "                                 AND PV.VOUCHERDATE = '" + hf_Date.Value.ToString() + "' \n"
               + "                                 AND MONTH(PV.VOUCHERDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                                 AND YEAR(PV.VOUCHERDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                                 AND PV.CREDITCLIENTID IS NOT NULL \n"
               //+ "                                 AND PV.PAYMENTSOURCEID = '1' \n"
               + "                                 AND (PV.PAYMENTSOURCEID IS NULL OR PV.PAYMENTSOURCEID = '1') \n"
               + "                                     GROUP BY \n"
               + "                                     VOUCHERDATE, \n"
               + "                                 PV.REFNO, \n"
               + "                                 PT.NAME, \n"
               + "                                 B.[NAME] \n"
               + "                                 UNION ALL \n"
               + "                                  \n"
               + "                                  \n"
               + "                                 SELECT '1' RANK, \n"
               + "                                        SUM(PV.AMOUNT) - SUM(ISNULL(MM.PRODUCTAMOUNT, 0))  \n"
               + "                                        DEBIT, \n"
               + "                                        0 CREDIT, \n"
               + "                                        CASE  \n"
               + "                                             WHEN PV.REFNO IS NULL \n"
               + "                                 OR PV.REFNO = '' THEN 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')' ELSE 'CASH IN HAND (' + ISNULL(PT.NAME, '')  \n"
               + "                                    + ')'  \n"
               + "                                    + ' REF NO# ' + PV.REFNO END CASHTYPE, \n"
               + "                                 PT.NAME, \n"
               + "                                 VOUCHERDATE DATE, \n"
               + "                                 B.[NAME] BRANCH, \n"
               + "                                 '' COMPANY \n"
               + "                                 FROM PAYMENTVOUCHERS PV \n"
               + "                                 LEFT OUTER JOIN ( \n"
               + "                                     SELECT MP.VOUCHERID, \n"
               + "                                            SUM(MP.AMOUNT) PRODUCTAMOUNT \n"
               + "                                     FROM   MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN  \n"
               + "                                            MP \n"
               + "                                     WHERE  MP.PRODUCT IN ('JAZZCASH', 'JAZZ CARD') \n"
               + "                                     GROUP BY \n"
               + "                                            MP.VOUCHERID \n"
               + "                                 ) MM \n"
               + "                                 ON MM.VOUCHERID = PV.ID \n"
               + "                                 LEFT OUTER JOIN PAYMENTTYPES PT \n"
               + "                                 ON PT.ID = PV.PAYMENTTYPEID \n"
               + "                                 INNER JOIN BRANCHES B \n"
               + "                                 ON B.BRANCHCODE = PV.BRANCHCODE \n"
               + "                                 WHERE PV.BRANCHCODE = '" + Branch_ID + "' \n"
               //+ "                                 AND PV.VOUCHERDATE = '" + hf_Date.Value.ToString() + "' \n"
               + "                                 AND MONTH(PV.VOUCHERDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                                 AND YEAR(PV.VOUCHERDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                                 AND PV.CREDITCLIENTID IS NULL \n"
               + "                                 AND PV.CLIENTGROUPID IS NOT NULL \n"
               + "                                 AND PV.PAYMENTSOURCEID = '1' \n"
               + "                                     GROUP BY \n"
               + "                                     VOUCHERDATE, \n"
               + "                                 PV.REFNO, \n"
               + "                                 PT.NAME, \n"
               + "                                 B.[NAME] \n"
               + "                             )       A \n"
               + "                      GROUP BY \n"
               + "                             CONVERT(VARCHAR, A.DATE, 103), \n"
               + "                             A.NAME,DEBIT, CASHTYPE \n"
               //   + "                             CASE  \n"
               //   + "                                  WHEN A.NAME = 'EXP CENTER SALE' THEN DEBIT \n"
               //   + "                             END, \n"
               //   + "                             CASE WHEN A.NAME = 'PAYMENT' THEN DEBIT END, \n"
               //   + "                             CASE  \n"
               //   + "                                  WHEN A.NAME = 'COD PAYMENT' THEN DEBIT \n"
               //   + "                             END, \n"
               //   + "                             CASE  \n"
               //   + "                                  WHEN A.NAME NOT IN ('COD PAYMENT', 'EXP CENTER SALE','PAYMENT') THEN DEBIT \n"
               ////   + "                                 WHEN A.NAME = ('License Fee') THEN sum(DEBIT) \n"
               //   + "                                        \n"
               //   + "                             END      \n"
               + "                  )                       X \n"
               + "           GROUP BY \n"
               + "                  X.DATE, \n"
               + "                  x.COD_DEPOSITE, \n"
               + "                  x.DEPOSITE_NONCOD \n"
               + "            \n"
               + "           UNION ALL \n"
               + "            \n"
               + "           SELECT X.RRDATE  AS DATE, \n"
               + "                  0                RETAIL_SALE, \n"
               + "                  0                CASH_RECOVERY, \n"
               + "                  0                COD, \n"
               + "                  0                MISC_RECEIPT, \n"
               + "                  ISNULL(SUM(COD_DEPOSITE), 0) COD_DEPOSITE, \n"
               + "                  ISNULL(SUM(DEPOSITE_NONCOD), 0) DEPOSITE_NONCOD \n"
               + "           FROM   ( \n"
               + "                      SELECT CONVERT(VARCHAR, B.RRDATE, 103) RRDATE, \n"
               + "                             0       RETAIL_SALE, \n"
               + "                             0       CASH_RECOVERY, \n"
               + "                             0       COD, \n"
               + "                             0       MISC_RECEIPT, \n"
               + "                             CASE  \n"
               + "                                  WHEN B.NATUREDEPOSIT = '01' THEN SUM(AMOUNT) \n"
               + "                             END     COD_DEPOSITE, \n"
               + "                             CASE  \n"
               + "                                  WHEN B.NATUREDEPOSIT = '02' THEN SUM(AMOUNT) \n"
               + "                             END     DEPOSITE_NONCOD \n"
               + "                      FROM   DEPOSIT_TO_BANK B \n"
               //+ "                      WHERE  CAST(B.CHEQUEDATE AS DATE) = '" + hf_Date.Value.ToString() + "' \n"
               + "                      WHERE  MONTH(B.RRDATE) = '" + hf_Month.Value.ToString() + "' \n"
               + "                      AND YEAR(B.RRDATE) = '" + hf_Year.Value.ToString() + "' \n"
               + "                      and b.branchcode = '" + Branch_ID + "' \n"
               + "                      GROUP BY \n"
               + "                             B.RRDATE, \n"
               + "                             B.NATUREDEPOSIT \n"
               + "                  )                X \n"
               + "           GROUP BY \n"
               + "                  X.RRDATE, \n"
               + "                  X.CASH_RECOVERY, \n"
               + "                  X.COD, \n"
               + "                  X.MISC_RECEIPT \n"
               + "       )                                     Z \n"
               + "GROUP BY \n"
               + "       z.DATE \n"
               + "ORDER BY \n"
               + "       1 \n";


                #endregion

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 6000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);

                orcl.Close();

            }
            catch (Exception Err)
            {
                lbl_Message.Text = Err.Message;
                lbl_Message.ForeColor = System.Drawing.Color.Red;
            }
            finally
            { }
            return ds;
        }

        public DataSet Get_Branch_ID(Variable clvar)
        {
            DataSet ds = new DataSet();
            string sqlString = "";

            try
            {
                #region SELECT QUERY OF SQL


                sqlString = "SELECT branchcode, name from branches where name = '" + hf_Branch_Name.Value.ToString() + "' \n and status = '1'";


                #endregion

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 6000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);

                orcl.Close();

            }
            catch (Exception Err)
            {
                lbl_Message.Text = Err.Message;
                lbl_Message.ForeColor = System.Drawing.Color.Red;
            }
            finally
            { }
            return ds;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void GridViewFeeding_Status_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label RETAIL_SALE = (Label)e.Row.FindControl("RETAIL_SALE");
                Label CASH_RECOVERY = (Label)e.Row.FindControl("CASH_RECOVERY");
                Label COD = (Label)e.Row.FindControl("COD");
                Label MISC_RECEIPT = (Label)e.Row.FindControl("MISC_RECEIPT");
                Label COD_DEPOSITE = (Label)e.Row.FindControl("COD_DEPOSITE");
                Label DEPOSITE_NONCOD = (Label)e.Row.FindControl("DEPOSITE_NONCOD");
                Label DIFFERENCE = (Label)e.Row.FindControl("DIFFERENCE");

                #region RETAIL_SALE

                if (RETAIL_SALE != null)
                {
                    decimal RETAIL_SALE_ = Decimal.Parse(RETAIL_SALE.Text);
                    total_retail_sale += RETAIL_SALE_;
                }

                #endregion

                #region CASH_RECOVERY

                if (CASH_RECOVERY != null)
                {
                    decimal CASH_RECOVERY_ = Decimal.Parse(CASH_RECOVERY.Text);
                    total_cash_recovery += CASH_RECOVERY_;
                }

                #endregion

                #region COD

                if (COD != null)
                {
                    decimal COD_ = Decimal.Parse(COD.Text);
                    total_cod += COD_;
                }

                #endregion

                #region MISC_RECEIPT

                if (MISC_RECEIPT != null)
                {
                    decimal MISC_RECEIPT_ = Decimal.Parse(MISC_RECEIPT.Text);
                    total_misc_receipt += MISC_RECEIPT_;
                }

                #endregion

                #region COD_DEPOSITE

                if (COD_DEPOSITE != null)
                {
                    decimal COD_DEPOSITE_ = Decimal.Parse(COD_DEPOSITE.Text);
                    total_cod_deposite += COD_DEPOSITE_;
                }

                #endregion

                #region DEPOSITE_NONCOD

                if (DEPOSITE_NONCOD != null)
                {
                    decimal DEPOSITE_NONCOD_ = Decimal.Parse(DEPOSITE_NONCOD.Text);
                    total_noncod_deposite += DEPOSITE_NONCOD_;
                }

                #endregion

                #region DEPOSITE_NONCOD

                if (DIFFERENCE != null)
                {
                    decimal DIFFERENCE_ = Decimal.Parse(DIFFERENCE.Text);
                    total_difference += DIFFERENCE_;
                }

                #endregion

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label TOTAL_RETAIL_SALE = (Label)e.Row.FindControl("TOTAL_RETAIL_SALE");
                Label TOTAL_CASH_RECOVERY = (Label)e.Row.FindControl("TOTAL_CASH_RECOVERY");
                Label TOTAL_COD = (Label)e.Row.FindControl("TOTAL_COD");
                Label TOTAL_MISC_RECEIPT = (Label)e.Row.FindControl("TOTAL_MISC_RECEIPT");
                Label TOTAL_COD_DEPOSITE = (Label)e.Row.FindControl("TOTAL_COD_DEPOSITE");
                Label TOTAL_DEPOSITE_NONCOD = (Label)e.Row.FindControl("TOTAL_DEPOSITE_NONCOD");
                Label TOTAL_DIFFERENCE = (Label)e.Row.FindControl("TOTAL_DIFFERENCE");

                #region TOTAL_RETAIL_SALE

                if (TOTAL_RETAIL_SALE != null)
                {
                    TOTAL_RETAIL_SALE.Text = total_retail_sale.ToString("N0");

                }

                #endregion

                #region TOTAL_CASH_RECOVERY

                if (TOTAL_CASH_RECOVERY != null)
                {
                    TOTAL_CASH_RECOVERY.Text = total_cash_recovery.ToString("N0");

                }

                #endregion

                #region TOTAL_COD

                if (TOTAL_COD != null)
                {
                    TOTAL_COD.Text = total_cod.ToString("N0");

                }

                #endregion

                #region TOTAL_MISC_RECEIPT

                if (TOTAL_MISC_RECEIPT != null)
                {
                    TOTAL_MISC_RECEIPT.Text = total_misc_receipt.ToString("N0");

                }

                #endregion

                #region TOTAL_COD_DEPOSITE

                if (TOTAL_COD_DEPOSITE != null)
                {
                    TOTAL_COD_DEPOSITE.Text = total_cod_deposite.ToString("N0");

                }

                #endregion

                #region TOTAL_DEPOSITE_NONCOD

                if (TOTAL_DEPOSITE_NONCOD != null)
                {
                    TOTAL_DEPOSITE_NONCOD.Text = total_noncod_deposite.ToString("N0");

                }

                #endregion

                #region TOTAL_DIFFERENCE

                if (TOTAL_DIFFERENCE != null)
                {
                    TOTAL_DIFFERENCE.Text = total_difference.ToString("N0");

                }

                #endregion

            }
        }

        protected void GridViewFeeding_Status_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewFeeding_Status.PageIndex = e.NewPageIndex;
            DataTable dt = Get_Feeding_Status(clvar).Tables[0];
            GridViewFeeding_Status.DataSource = dt;
            GridViewFeeding_Status.DataBind();
        }

        protected void GridViewFeeding_Status_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = TopHeader1;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.LightBlue;
                HeaderCell.Font.Size = FontUnit.Parse("12px");
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.VerticalAlign = VerticalAlign.Middle;
                HeaderCell.ColumnSpan = 4;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = TopHeader2;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.LightBlue;
                HeaderCell.Font.Size = FontUnit.Parse("12px");
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.VerticalAlign = VerticalAlign.Middle;
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = TopHeader3;
                HeaderCell.Font.Bold = true;
                HeaderCell.BackColor = System.Drawing.Color.LightBlue;
                HeaderCell.Font.Size = FontUnit.Parse("12px");
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.VerticalAlign = VerticalAlign.Middle;
                HeaderCell.ColumnSpan = 1;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderGrid.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }

        private decimal GetTotalRowSum(DataTable datatable, string ColumnName)
        {
            decimal Total = 0M;

            if (datatable.Rows.Count < 0)
            {
                return Total;
            }
            else
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    if (!datatable.Rows[i][ColumnName].Equals(null))
                    {
                        Total += decimal.Parse(datatable.Rows[i][ColumnName].ToString());
                    }

                }
                return Total;
            }
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            StartDate = Request.QueryString["date"];
            BranchName = Request.QueryString["branchname"];

            hf_Date.Value = StartDate;
            hf_Branch_Name.Value = BranchName;

            DateTime Month_Year = DateTime.Parse(hf_Date.Value.ToString());

            Month = Month_Year.ToString("MM");
            Year = Month_Year.ToString("yyyy");

            hf_Month.Value = Month;
            hf_Year.Value = Year;

            if (GridViewFeeding_Status.Rows.Count.Equals(0) || GridViewFeeding_Status.Rows.Count > 0)
            {
                DataTable dt = new DataTable();

                if (GridViewFeeding_Status.AllowPaging)
                {
                    GridViewFeeding_Status.AllowPaging = false;
                    dt = Get_Feeding_Status(clvar).Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string RowData = dt.Rows[i][0].ToString();      //  GET DATE COLUMN DATA
                        DateTime DateRowData = DateTime.Parse(RowData);

                        dt.Rows[i][0] = DateRowData.ToString("dd MMM yyyy");
                        dt.AcceptChanges();
                    }

                    GridViewFeeding_Status.DataSource = dt;
                    GridViewFeeding_Status.DataBind();

                }

                #region Using String Builder

                Response.Clear();

                Response.Buffer = true;

                Response.ContentType = "text/csv";

                Response.AddHeader("content-disposition",

                "attachment;filename=Report Feeding Status (" + BranchName + " - " + Month_Year.ToString("MMMM yyyy") + ").csv");

                Response.Charset = "";

                StringBuilder sb = new StringBuilder();

                int Column = 0;


                #region Useful & Working But Takes Less Time

                sb.Append(",," + TopHeader1 + ",,,," + TopHeader2 + ",," + TopHeader3);

                sb.Append("\r\n");

                Column = GridViewFeeding_Status.Columns.Count - 1;

                for (int k = 0; k < GridViewFeeding_Status.Columns.Count; k++)
                {

                    string ColumnName = HttpContext.Current.Server.HtmlDecode(GridViewFeeding_Status.Columns[k].HeaderText.ToString());

                    if (k.Equals(Column))
                    {
                        sb.Append('"' + ColumnName + '"');
                    }

                    else if (!k.Equals(Column))
                    {
                        sb.Append('"' + ColumnName + '"' + ',');
                    }

                }

                //append new line

                sb.Append("\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if ((GridViewFeeding_Status.Rows[i].FindControl("lblRowNumber") as Label) != null)
                    {
                        sb.Append('"' + (GridViewFeeding_Status.Rows[i].FindControl("lblRowNumber") as Label).Text + '"' + ',');
                    }

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {

                        string RowData = HttpContext.Current.Server.HtmlDecode(dt.Rows[i].ItemArray[k].ToString());

                        #region When Row Data is NULL OR Empty

                        if (RowData.Equals(string.Empty))
                        {

                            if (k.Equals(0))    //  DATE
                            {
                                sb.Append("=" + '"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(1))   //  RETAIL SALE
                            {
                                sb.Append('"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(2))   //  CASH RECOVERY
                            {
                                sb.Append('"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(3))   //  COD
                            {
                                sb.Append('"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(4))   //  MISC RECEIPT
                            {
                                sb.Append('"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(5))   //  COD DEPOSITE
                            {
                                sb.Append('"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(6))   //  NONCOD DEPOSITE
                            {
                                sb.Append('"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(7))   //  DIFFERENCE
                            {
                                sb.Append('"' + RowData + '"');
                            }
                        }

                        #endregion

                        #region When Row Data is NOT NULL OR Empty

                        else if (!RowData.Equals(string.Empty))
                        {

                            if (k.Equals(0))    //  DATE
                            {
                                sb.Append("=" + '"' + RowData + '"' + ',');
                            }
                            else if (k.Equals(1))   //  RETAIL SALE
                            {
                                sb.Append('"' + int.Parse(RowData).ToString("N0") + '"' + ',');
                            }
                            else if (k.Equals(2))   //  CASH RECOVERY
                            {
                                sb.Append('"' + int.Parse(RowData).ToString("N0") + '"' + ',');
                            }
                            else if (k.Equals(3))   //  COD
                            {
                                sb.Append('"' + int.Parse(RowData).ToString("N0") + '"' + ',');
                            }
                            else if (k.Equals(4))   //  MISC RECEIPT
                            {
                                sb.Append('"' + int.Parse(RowData).ToString("N0") + '"' + ',');
                            }
                            else if (k.Equals(5))   //  COD DEPOSITE
                            {
                                sb.Append('"' + int.Parse(RowData).ToString("N0") + '"' + ',');
                            }
                            else if (k.Equals(6))   //  NONCOD DEPOSITE
                            {
                                sb.Append('"' + int.Parse(RowData).ToString("N0") + '"' + ',');
                            }
                            else if (k.Equals(7))   //  DIFFERENCE
                            {
                                sb.Append('"' + int.Parse(RowData).ToString("N0") + '"');
                            }
                        }

                        #endregion

                    }

                    //append new line

                    sb.Append("\r\n");

                }

                #region When User Wants Result in HTML


                object RETAIL_SALE = dt.Compute("SUM(RETAIL_SALE)", string.Empty);
                object CASH_RECOVERY = dt.Compute("SUM(CASH_RECOVERY)", string.Empty);
                object COD = dt.Compute("SUM(COD)", string.Empty);
                object MISC_RECEIPT = dt.Compute("SUM(MISC_RECEIPT)", string.Empty);
                object COD_DEPOSITE = dt.Compute("SUM(COD_DEPOSITE)", string.Empty);
                object DEPOSITE_NONCOD = dt.Compute("SUM(DEPOSITE_NONCOD)", string.Empty);
                object DIFFERENCE = dt.Compute("SUM(DIFFERENCE)", string.Empty);

                string Retail_Sale = int.Parse(RETAIL_SALE.ToString()).ToString("N0");
                string Cash_Recovery = int.Parse(CASH_RECOVERY.ToString()).ToString("N0");
                string Cod = int.Parse(COD.ToString()).ToString("N0");
                string Misc_Receipt = int.Parse(MISC_RECEIPT.ToString()).ToString("N0");
                string Cod_Deposite = int.Parse(COD_DEPOSITE.ToString()).ToString("N0");
                string NonCod_Deposite = int.Parse(DEPOSITE_NONCOD.ToString()).ToString("N0");
                string Difference = int.Parse(DIFFERENCE.ToString()).ToString("N0");

                sb.Append("=" + '"' + '"' + ',' + "=" + '"' + '"' + ',' + '"' + Retail_Sale.ToString() + '"' + ',' + '"' + Cash_Recovery.ToString() + '"' + ',' + '"' + Cod.ToString() + '"' + ',' + '"' + Misc_Receipt.ToString() + '"' + ',' + '"' + Cod_Deposite.ToString() + '"' + ',' + '"' + NonCod_Deposite.ToString() + '"' + ',' + '"' + Difference.ToString() + '"');


                #endregion


                #endregion


                Response.Output.Write(sb.ToString());

                Response.Flush();

                Response.End();

                GridViewFeeding_Status.AllowPaging = true;


                #endregion

            }

        }

        protected void btn_Excel_Click(object sender, EventArgs e)
        {
            StartDate = Request.QueryString["date"];
            BranchName = Request.QueryString["branchname"];

            hf_Date.Value = StartDate;
            hf_Branch_Name.Value = BranchName;

            DateTime Month_Year = DateTime.Parse(hf_Date.Value.ToString());

            Month = Month_Year.ToString("MM");
            Year = Month_Year.ToString("yyyy");

            hf_Month.Value = Month;
            hf_Year.Value = Year;

            if (GridViewFeeding_Status.Rows.Count == 0 || GridViewFeeding_Status.Rows.Count > 0)
            {

                if (GridViewFeeding_Status.AllowPaging)
                {
                    GridViewFeeding_Status.AllowPaging = false;
                }
                DataTable dt = Get_Feeding_Status(clvar).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string RowData = dt.Rows[i][0].ToString();      //  GET DATE COLUMN DATA
                    DateTime DateRowData = DateTime.Parse(RowData);

                    dt.Rows[i][0] = DateRowData.ToString("dd MMM yyyy");
                    dt.AcceptChanges();
                }

                GridViewFeeding_Status.DataSource = dt;
                GridViewFeeding_Status.DataBind();

            }


            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition", "attachment;filename=Report Feeding Status (" + BranchName + " - " + Month_Year.ToString("MMMM yyyy") + ").xls");

            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                foreach (TableCell cell in GridViewFeeding_Status.HeaderRow.Cells)
                {
                    //cell.Attributes.Add("style", "text-align:center; vertical-align:middle;");
                    cell.CssClass = "textmode";
                }

                foreach (GridViewRow row in GridViewFeeding_Status.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        //cell.Attributes.Add("style", "text-align:center; vertical-align:middle;");
                        cell.CssClass = "textmode";
                    }
                }

                foreach (TableCell cell in GridViewFeeding_Status.FooterRow.Cells)
                {
                    //cell.Attributes.Add("style", "text-align:center; vertical-align:middle;");
                    cell.Font.Bold = true;
                    cell.CssClass = "textmode";
                }

                //To Export all pages

                GridViewFeeding_Status.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                GridViewFeeding_Status.Visible = false;
                Response.Flush();
                Response.End();
                return;
            }
        }
    }
}