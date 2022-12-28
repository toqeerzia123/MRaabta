using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class PC_ClosingBalances_QA_Print : System.Web.UI.Page
    {
        //HiddenField hf_from = new HiddenField();
        //HiddenField hf_company = new HiddenField();
        //HiddenField hf_branch = new HiddenField();
        Cl_Variables clvar = new Cl_Variables();

        string ZoneID = "", BranchID = "";
        string ZoneID_ = "", BranchID_ = "";

        string Date = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!(HttpContext.Current.Session["U_ID"].ToString() == "3148" || HttpContext.Current.Session["U_ID"].ToString() == "3177"))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You are not authorized to view this page.'); window.location='" + Request.ApplicationPath + "/Files/BTSDashoard.aspx';", true);
            //    return;
            //}

            if (!IsPostBack)
            {

                Date = Decrypt_QueryString(Request.QueryString["fromdate"]);
                Generate_Data();


                //ZoneID += "'" + HttpContext.Current.Session["ZONECODE"].ToString().Items[i].Value + "',";


            }
        }

        private void Generate_Data()
        {
            if (HttpContext.Current.Session["ZONECODE"].ToString() != "ALL")
            {
                string zone = HttpContext.Current.Session["ZONECODE"].ToString().Replace(",", "','");
                ZoneID = "AND z.zoneCode IN ('" + zone + "')";
                ZoneID_ = "where z.zoneCode IN ('" + zone + "')";
            }
            else
            {
                ZoneID = "";
            }

            if (HttpContext.Current.Session["BRANCHCODE"].ToString() != "ALL")
            {
                string branch = HttpContext.Current.Session["BRANCHCODE"].ToString().Replace(",", "','");
                BranchID = "AND b.branchCode in ('" + branch + "')";
            }
            else
            {
                BranchID = "";
            }

            //var dt = DateTime.ParseExact(hf_from.Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //hf_from.Value = dt.ToString("yyyy-MM-dd");

            Get_RemoveTempTable(clvar);
            hf_from.Value = Date;
            DataTable branchDt = GetAllBranches();
            DataTable cihDt = GetCIHOpeningBalance(hf_from.Value);
            DataTable pcDt = GetPCOpeningBalances(hf_from.Value);
            DataTable initialDt = new DataTable();
            initialDt.Columns.AddRange(new DataColumn[] {
            new DataColumn("SNO"),
            new DataColumn("Zone"),
            new DataColumn("Branch"),
            new DataColumn("Company"),
            new DataColumn("BranchName"),
            new DataColumn("CompanyName"),
            new DataColumn("CIHOpeningBalance"),
            new DataColumn("PCOpeningBalance")

        });

            if (branchDt != null)
            {
                if (branchDt.Rows.Count > 0)
                {
                    for (int i = 0; i < branchDt.Rows.Count; i++)
                    {
                        DataRow dr = branchDt.Rows[i];
                        DataRow initialRow = initialDt.NewRow();
                        initialRow["Sno"] = (i + 1).ToString();
                        initialRow["Branch"] = dr["Branch"].ToString();
                        initialRow["Zone"] = dr["Zone"].ToString();
                        initialRow["Company"] = dr["Company"].ToString();
                        initialRow["BranchName"] = dr["BranchName"].ToString();
                        initialRow["CompanyName"] = dr["CompanyName"].ToString();

                        initialDt.Rows.Add(initialRow);
                    }
                }
            }

            if (cihDt != null)
            {
                if (cihDt.Rows.Count > 0)
                {
                    for (int i = 0; i < cihDt.Rows.Count; i++)
                    {
                        DataRow cihRow = cihDt.Rows[i];
                        DataRow initialRow = initialDt.Select("Branch = '" + cihRow["Branch"].ToString() + "' AND Company = '" + cihRow["Company"].ToString() + "'").First();

                        initialRow["CIHOpeningBalance"] = cihRow["ClosingBalance"].ToString();
                    }
                }
            }

            if (pcDt != null)
            {
                if (pcDt.Rows.Count > 0)
                {
                    for (int i = 0; i < pcDt.Rows.Count; i++)
                    {
                        DataRow pcRow = pcDt.Rows[i];
                        DataRow initialRow = initialDt.Select("Branch = '" + pcRow["Branch"].ToString() + "' AND Company = '" + pcRow["Company"].ToString() + "'").First();

                        initialRow["PCOpeningBalance"] = pcRow["ClosingPCash"].ToString();
                    }
                }
            }
            DataTable CIH = GetCashInHandLedgerForDay(hf_from.Value);

            DataTable PC = GetPettyCashLedgerForDay(hf_from.Value);


            DataTable finalDt = new DataTable();
            finalDt.Columns.AddRange(new DataColumn[]{
            new DataColumn("SNO"),
            new DataColumn("Zone"),
            new DataColumn("BranchName"),
            new DataColumn("C1CashInHand"),
            new DataColumn("C1PettyCash"),
            new DataColumn("C2CashInHand"),
            new DataColumn("C2PettyCash")
        });

            DataView branchView = new DataView(initialDt);
            branchView.Sort = "Zone, branchName";
            DataTable distinctBranches = branchView.ToTable(true, "Zone", "BranchName", "Branch");
            int sno = 0;

            foreach (DataRow brRow in distinctBranches.Rows)
            {
                sno++;
                DataRow[] iRow = initialDt.Select("Branch = '" + brRow["Branch"].ToString() + "'");
                DataRow[] pcRow = PC.Select("Branch = '" + brRow["Branch"].ToString() + "'");
                DataRow[] cihRow = CIH.Select("Branch = '" + brRow["Branch"].ToString() + "'");

                DataRow finalRow = finalDt.NewRow();
                finalRow["Sno"] = sno.ToString();
                finalRow["BranchName"] = brRow["BranchName"].ToString();
                finalRow["Zone"] = brRow["Zone"].ToString();
                double C1pcOpening = 0;
                double C1pcCredit = 0;
                double C1pcDebit = 0;
                double C1pcClosing = 0;

                double C1cihOpening = 0;
                double C1cihCredit = 0;
                double C1cihDebit = 0;
                double C1cihClosing = 0;

                double C2pcOpening = 0;
                double C2pcCredit = 0;
                double C2pcDebit = 0;
                double C2pcClosing = 0;

                double C2cihOpening = 0;
                double C2cihCredit = 0;
                double C2cihDebit = 0;
                double C2cihClosing = 0;


                for (int i = 0; i < iRow.Count(); i++)
                {
                    if (iRow[i]["COMPANY"].ToString() == "1")
                    {
                        // C1pcOpening = double.Parse(iRow[i]["PCOpeningBalance"].ToString());

                        if (iRow[i]["PCOpeningBalance"].ToString() != null && iRow[i]["PCOpeningBalance"].ToString() != "")
                        {
                            C1pcOpening = double.Parse(iRow[i]["PCOpeningBalance"].ToString());
                        }
                        else
                        {
                            C1pcOpening = 0;
                        }


                        if (iRow[i]["CIHOpeningBalance"].ToString() != null && iRow[i]["CIHOpeningBalance"].ToString() != "")
                        {
                            C1cihOpening = double.Parse(iRow[i]["CIHOpeningBalance"].ToString());
                        }
                        else
                        {
                            C1cihOpening = 0;
                        }
                    }
                    else if (iRow[i]["COMPANY"].ToString() == "2")
                    {
                        // C2pcOpening = double.Parse(iRow[i]["PCOpeningBalance"].ToString());

                        if (iRow[i]["PCOpeningBalance"].ToString() != null && iRow[i]["PCOpeningBalance"].ToString() != "")
                        {
                            C2pcOpening = double.Parse(iRow[i]["PCOpeningBalance"].ToString());
                        }
                        else
                        {
                            C2pcOpening = 0;
                        }

                        if (iRow[i]["CIHOpeningBalance"].ToString() != null && iRow[i]["CIHOpeningBalance"].ToString() != "")
                        {
                            C2cihOpening = double.Parse(iRow[i]["CIHOpeningBalance"].ToString());
                        }
                        else
                        {
                            C2cihOpening = 0;
                        }
                    }
                }

                for (int i = 0; i < pcRow.Count(); i++)
                {
                    if (pcRow[i]["Company"].ToString() == "1")
                    {
                        C1pcDebit = double.Parse(pcRow[i]["DEBIT"].ToString());
                        C1pcCredit = double.Parse(pcRow[i]["CREDIT"].ToString());
                    }
                    else if (pcRow[i]["Company"].ToString() == "2")
                    {
                        C2pcDebit = double.Parse(pcRow[i]["DEBIT"].ToString());
                        C2pcCredit = double.Parse(pcRow[i]["CREDIT"].ToString());
                    }
                }

                for (int i = 0; i < cihRow.Count(); i++)
                {
                    if (cihRow[i]["Company"].ToString() == "1")
                    {
                        C1cihDebit = double.Parse(cihRow[i]["DEBIT"].ToString());
                        C1cihCredit = double.Parse(cihRow[i]["CREDIT"].ToString());
                    }
                    else if (cihRow[i]["Company"].ToString() == "2")
                    {
                        C2cihDebit = double.Parse(cihRow[i]["DEBIT"].ToString());
                        C2cihCredit = double.Parse(cihRow[i]["CREDIT"].ToString());
                    }
                }

                C1cihClosing = C1cihDebit - C1cihCredit + C1cihOpening;
                C2cihClosing = C2cihDebit - C2cihCredit + C2cihOpening;

                C1pcClosing = C1pcDebit - C1pcCredit + C1pcOpening;
                C2pcClosing = C2pcDebit - C2pcCredit + C2pcOpening;


                finalRow["C1CashInHand"] = string.Format("{0:N2}", C1cihClosing);
                finalRow["C1PettyCash"] = string.Format("{0:N2}", C1pcClosing);
                finalRow["C2CashInHand"] = string.Format("{0:N2}", C2cihClosing);
                finalRow["C2PettyCash"] = string.Format("{0:N2}", C2pcClosing);
                finalDt.Rows.Add(finalRow);
                finalDt.AcceptChanges();
            }

            if (finalDt.Rows.Count > 0)
            {
                DataView finalView = new DataView(finalDt);
                finalView.Sort = "zone, branchName";

                rp_data.DataSource = finalView.ToTable();
                rp_data.DataBind();
            }
        }

        public static string Decrypt_QueryString(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        public DataTable GetCIHOpeningBalance(string requiredDate)
        {
            string month = "";
            string year = "";
            string startDate = "";

            month = requiredDate.Split('-')[1];
            month = int.Parse(month).ToString();

            year = requiredDate.Split('-')[0];
            year = int.Parse(year).ToString();



            startDate = $@"{year}/{month}/1";
            string endDate = DateTime.Parse(requiredDate).AddDays(-1).ToString("yyyy-MM-dd");
            //startDate = DateTime.Parse("01" + "/" + DateTime.Parse(endDate).Month.ToString() + "/" + DateTime.Parse(endDate).Year.ToString()).ToString("yyyy-MM-dd");

            #region MyRegion
            string sqlString___ = "SELECT \n" +
            "----- PC -- CLOSING BALANCES REPORT (Opening Balance) \n" +
            " --" + HttpContext.Current.Session["U_NAME"].ToString() + "\n" +
            "       cr.branch,\n" +
            "       b.name             BranchName,\n" +
            "       cr.company,\n" +
            "       c.companyName,\n" +
            "       cr.[year],\n" +
            "       cr.[month],\n" +
            "       SUM(BB.dnote)      DNOTE,\n" +
            "       SUM(BB.cnote)      CNOTE,\n" +
            "       cr.OPENING_VALUE,\n" +
            "       ISNULL(\n" +
            "           ROUND((SUM(bb.dnote) + cr.OPENING_VALUE - SUM(bb.cnote)), 0),\n" +
            "           0\n" +
            "       )                  ClosingBalance,\n" +
            "       cr.[VALUE],\n" +
            "       ISNULL(\n" +
            "           ROUND((SUM(bb.dnote) + cr.OPENING_VALUE - SUM(bb.cnote)), 0),\n" +
            "           0\n" +
            "       ) - cr.[VALUE]     DIFF\n" +
            "INTO CLOSING_BALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " \n" +
            "FROM   CIH_remainings cr\n" +
            "       LEFT OUTER JOIN (\n" +
            "                SELECT distinct a.rank,\n" +
            "                       CONVERT(VARCHAR, a.date, 103) date,\n" +
            "                       a.cashtype,\n" +
            "                       a.debit      dnote,\n" +
            "                       a.credit     cnote,\n" +
            "                       a.branch,\n" +
            "                       company\n" +
            "                FROM   (\n" +
            "                           SELECT '1' RANK,\n" +
            "                                  --SUM(Amount) debit,\n" +
            "                                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0))\n" +
            "                                  debit,\n" +
            "                                  0 credit,\n" +
            "                                  'Cash In Hand (' + ISNULL(pt.name, ',') + ')'\n" +
            "                                  cashtype,\n" +
            "                                  VoucherDate date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  '1' company\n" +
            "                           FROM   PaymentVouchers pv\n" +
            "                                  LEFT OUTER JOIN (\n" +
            "                                           SELECT mp.voucherID,\n" +
            "                                                  SUM(mp.amount) productAmount\n" +
            "                                           FROM\n" +
            "                                                  MnP_PaymentVouchersProductBreakDown\n" +
            "                                                  mp\n" +
            "                                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
            "                                           GROUP BY\n" +
            "                                                  mp.voucherID\n" +
            "                                       ) mm\n" +
            "                                       ON  mm.voucherID = pv.Id\n" +
            "                                  LEFT OUTER JOIN PaymentTypes pt\n" +
            "                                       ON  pt.Id = pv.PaymentTypeId\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = pv.BranchCode\n" +
            "                           WHERE  1 = 1 --AND pv.BranchCode = @BranchCode\n" +
            "                                  AND pv.VoucherDate BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND pv.CashPaymentSource IS NOT NULL\n" +
            "                                  AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1')\n" +
            "                                  AND pv.CashPaymentSource != '4'\n" +
            "                           GROUP BY\n" +
            "                                  VoucherDate,\n" +
            "                                  pt.name,\n" +
            "                                  b.branchCode\n" +
            "\n" +
            "\n" +
            "                           UNION ALL\n" +
            "\n" +
            "\n" +
            "                           SELECT '1' RANK,\n" +
            "                                  --SUM(Amount) debit,\n" +
            "                                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0))\n" +
            "                                  debit,\n" +
            "                                  0 credit,\n" +
            "                                  'Cash In Hand (' + ISNULL(pt.name, ',') + ')'\n" +
            "                                  cashtype,\n" +
            "                                  VoucherDate date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  '1' company\n" +
            "                           FROM   PaymentVouchers pv\n" +
            "                                  LEFT OUTER JOIN (\n" +
            "                                           SELECT mp.voucherID,\n" +
            "                                                  SUM(mp.amount) productAmount\n" +
            "                                           FROM\n" +
            "                                                  MnP_PaymentVouchersProductBreakDown\n" +
            "                                                  mp\n" +
            "                                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
            "                                           GROUP BY\n" +
            "                                                  mp.voucherID\n" +
            "                                       ) mm\n" +
            "                                       ON  mm.voucherID = pv.Id\n" +
            "                                  LEFT OUTER JOIN PaymentTypes pt\n" +
            "                                       ON  pt.Id = pv.PaymentTypeId\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = pv.BranchCode\n" +
            "                           WHERE  1 = 1 --AND pv.BranchCode = @BranchCode\n" +
            "                                  AND pv.VoucherDate BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND pv.CreditClientId IS NOT NULL\n" +
            "                                  AND pv.paymentsourceid = '1'\n" +
            "                           GROUP BY\n" +
            "                                  VoucherDate,\n" +
            "                                  pt.name,\n" +
            "                                  b.branchCode\n" +
            "                           UNION ALL\n" +
            "                           SELECT '1' RANK,\n" +
            "                                  --SUM(Amount) debit,\n" +
            "                                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0))\n" +
            "                                  debit,\n" +
            "                                  0 credit,\n" +
            "                                  'Cash In Hand (' + ISNULL(pt.name, ',') + ')'\n" +
            "                                  cashtype,\n" +
            "                                  VoucherDate date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  '1' company\n" +
            "                           FROM   PaymentVouchers pv\n" +
            "                                  LEFT OUTER JOIN (\n" +
            "                                           SELECT mp.voucherID,\n" +
            "                                                  SUM(mp.amount) productAmount\n" +
            "                                           FROM\n" +
            "                                                  MnP_PaymentVouchersProductBreakDown\n" +
            "                                                  mp\n" +
            "                                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
            "                                           GROUP BY\n" +
            "                                                  mp.voucherID\n" +
            "                                       ) mm\n" +
            "                                       ON  mm.voucherID = pv.Id\n" +
            "                                  LEFT OUTER JOIN PaymentTypes pt\n" +
            "                                       ON  pt.Id = pv.PaymentTypeId\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = pv.BranchCode\n" +
            "                           WHERE  1 = 1 --AND pv.BranchCode = @BranchCode\n" +
            "                                  AND pv.VoucherDate BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND pv.ClientGroupId IS NOT NULL\n" +
            "                                      AND pv.CreditClientId IS NULL\n" +
            "                                  AND pv.paymentsourceid = '1'\n" +
            "                           GROUP BY\n" +
            "                                  VoucherDate,\n" +
            "                                  pt.name,\n" +
            "                                  b.branchCode\n" +
            "                           UNION ALL\n" +
            "\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(pv.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Deposit To Bank' cashtype,\n" +
            "                                  pv.ChequeDate date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  pv.company company\n" +
            "                           FROM   deposit_to_bank pv\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = pv.BranchCode\n" +
            "                           WHERE  1 = 1 --AND pv.BranchCode = @BranchCode\n" +
            "                                  AND pv.company = '1'\n" +
            "                                  AND pv.ChequeDate BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                           GROUP BY\n" +
            "                                  ChequeDate,\n" +
            "                                  b.branchCode,\n" +
            "                                  pv.Id,\n" +
            "                                  pv.company\n" +
            "\n" +
            "\n" +
            "                           UNION ALL\n" +
            "\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(pv.Amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(pv.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Transferred To Petty Cash (From : ' + pc.[desc]\n" +
            "                                  + ')'\n" +
            "                                  cashtype,\n" +
            "                                  pv.[date] date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm.code_OF\n" +
            "                           FROM   PC_CIH_detail pv\n" +
            "                                  INNER JOIN PC_CIH_head ph\n" +
            "                                       ON  ph.ID = pv.head_id\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ph.BRANCH\n" +
            "                                  INNER JOIN PC_cash_mode pc\n" +
            "                                       ON  pc.ID = pv.cash_type\n" +
            "                                       AND pv.cash_type = '1'\n" +
            "                                       AND pc.ID = '1'\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ph.COMPANY\n" +
            "                                       AND ph.COMPANY = '1'\n" +
            "                           WHERE  1 = 1 --AND ph.BRANCH = @BranchCode\n" +
            "                                  AND pv.[date] BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                           GROUP BY\n" +
            "                                  [date],\n" +
            "                                  b.branchCode,\n" +
            "                                  pc.[desc],\n" +
            "                                  cm.code_OF\n" +
            "                           UNION ALL\n" +
            "                           --From Company\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(ch.amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.Amount) < 0 THEN (SUM(ch.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(ch.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Transfered To  ' + CAST(cm1.code_of AS VARCHAR)\n" +
            "                                  cashtype,\n" +
            "                                  ch.t_date date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm.code_of company\n" +
            "                           FROM   cih_transfer ch\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ch.BRANCH\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ch.from_comp\n" +
            "                                  INNER JOIN COMPANY_OF cm1\n" +
            "                                       ON  cm1.code_OF = ch.to_comp\n" +
            "                           WHERE  1 = 1 --AND ch.BRANCH = @BranchCode\n" +
            "                                  AND ch.t_date BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND ch.from_comp = '1'\n" +
            "                           GROUP BY\n" +
            "                                  ch.t_date,\n" +
            "                                  b.branchCode,\n" +
            "                                  cm.code_OF,\n" +
            "                                  cm1.code_OF\n" +
            "\n" +
            "                           UNION ALL\n" +
            "                           --To Company\n" +
            "                           SELECT '1' RANK,\n" +
            "                                  --SUM(ch.amount) debit,\n" +
            "                                  --0 credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(ch.amount)\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
            "                                  END     credit,\n" +
            "                                  'Received From  ' + CAST(cm.code_of AS VARCHAR)\n" +
            "                                  cashtype,\n" +
            "                                  ch.t_date date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm1.code_OF company\n" +
            "                           FROM   cih_transfer ch\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ch.BRANCH\n" +
            "                                  LEFT OUTER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ch.from_comp\n" +
            "                                  INNER JOIN COMPANY_OF cm1\n" +
            "                                       ON  cm1.code_OF = ch.to_comp\n" +
            "                           WHERE  1 = 1 --AND ch.BRANCH = @BranchCode\n" +
            "                                  AND ch.t_date BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND ch.to_comp = '1'\n" +
            "                           GROUP BY\n" +
            "                                  ch.t_date,\n" +
            "                                  b.branchCode,\n" +
            "                                  cm.code_OF,\n" +
            "                                  cm1.code_OF\n" +
            "                       )            a\n" +
            "            ) BB\n" +
            "            ON  cr.company = bb.company\n" +
            "            AND cr.branch = bb.branch\n" +
            "       INNER JOIN Branches b\n" +
            "            ON  b.branchCode = cr.branch\n" +
            "       INNER JOIN Zones z ON z.zoneCode = b.zoneCode \n" +
            "       INNER JOIN Company c\n" +
            "            ON  c.Id = cr.company\n" +
            "WHERE  cr.company = '1'\n" +
            "       AND cr.[month] = @Month\n" +
            "           --AND cr.branch = @BranchCode\n" +
            ZoneID + "\n" +
            BranchID + "\n" +
            "GROUP BY\n" +
            "       cr.company,\n" +
            "       b.name,\n" +
            "       c.companyName,\n" +
            "       cr.branch,\n" +
            "       cr.OPENING_VALUE,\n" +
            "       cr.[VALUE],\n" +
            "       cr.[year],\n" +
            "       cr.[month]\n" +
            "       --Company 1 Ends here\n" +

            //"       UNION ALL\n" +
            "\n" +
            "       --Company 2 Starts Here\n" +
            "SELECT cr.branch,\n" +
            "       b.name                       BranchName,\n" +
            "       cr.company,\n" +
            "       c.companyName,\n" +
            "       cr.[year],\n" +
            "       cr.[month],\n" +
            "       ISNULL(SUM(BB.dnote), 0)     Dnote,\n" +
            "       ISNULL(SUM(BB.cnote), 0)     CNOTE,\n" +
            "       cr.OPENING_VALUE,\n" +
            "       ISNULL(\n" +
            "           ROUND(\n" +
            "               ISNULL(SUM(BB.dnote), 0) - ISNULL(SUM(BB.cnote), 0) + cr.OPENING_VALUE,\n" +
            "               0\n" +
            "           ),\n" +
            "           0\n" +
            "       )                            closingBalance,\n" +
            "       cr.[VALUE],\n" +
            "       ISNULL(\n" +
            "           ROUND(\n" +
            "               ISNULL(SUM(BB.dnote), 0) - ISNULL(SUM(BB.cnote), 0) + cr.OPENING_VALUE,\n" +
            "               0\n" +
            "           ),\n" +
            "           0\n" +
            "       ) - cr.[VALUE]               DIFF\n" +
            "INTO CLOSING_BALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " \n" +
            "FROM   CIH_remainings cr\n" +
            "       LEFT OUTER JOIN (\n" +
            "                SELECT a.rank,\n" +
            "                       CONVERT(VARCHAR, a.date, 103) date,\n" +
            "                       a.cashtype,\n" +
            "                       a.debit      dnote,\n" +
            "                       a.credit     cnote,\n" +
            "                       a.branch,\n" +
            "                       company\n" +
            "                FROM   (\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(pv.Amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(pv.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Deposit To Bank' cashtype,\n" +
            "                                  pv.ChequeDate date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  '2' company\n" +
            "                           FROM   deposit_to_bank pv\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = pv.BranchCode\n" +
            "                           WHERE  1 = 1 --AND pv.BranchCode = @BranchCode\n" +
            "                                  AND pv.company = '2'\n" +
            "                                  AND pv.ChequeDate BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                           GROUP BY\n" +
            "                                  ChequeDate,\n" +
            "                                  b.branchCode,\n" +
            "                                  pv.Id\n" +
            "\n" +
            "                           UNION ALL\n" +
            "\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(pv.Amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(pv.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Transferred To Petty Cash (From : ' + pc.[desc]\n" +
            "                                  + ')'\n" +
            "                                  cashtype,\n" +
            "                                  pv.[date] date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm.code_of company\n" +
            "                           FROM   PC_CIH_detail pv\n" +
            "                                  INNER JOIN PC_CIH_head ph\n" +
            "                                       ON  ph.ID = pv.head_id\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ph.BRANCH\n" +
            "                                  INNER JOIN PC_cash_mode pc\n" +
            "                                       ON  pc.ID = pv.cash_type\n" +
            "                                       AND pv.cash_type = '1'\n" +
            "                                       AND pc.ID = '1'\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ph.COMPANY\n" +
            "                                       AND ph.COMPANY = '2'\n" +
            "                           WHERE  1 = 1 --AND ph.BRANCH = @BranchCode\n" +
            "                                  AND pv.[date]BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                           GROUP BY\n" +
            "                                  [date],\n" +
            "                                  b.branchCode,\n" +
            "                                  pc.[desc],\n" +
            "                                  cm.code_of\n" +
            "                           UNION ALL\n" +
            "                           --From Company\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(ch.amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.Amount) < 0 THEN (SUM(ch.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(ch.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Transfered To  ' + CAST(cm1.code_of AS VARCHAR)\n" +
            "                                  cashtype,\n" +
            "                                  ch.t_date date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm.code_OF company\n" +
            "                           FROM   cih_transfer ch\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ch.BRANCH\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ch.from_comp\n" +
            "                                  INNER JOIN COMPANY_OF cm1\n" +
            "                                       ON  cm1.code_OF = ch.to_comp\n" +
            "                           WHERE  1 = 1 --AND ch.BRANCH = @BranchCode\n" +
            "                                  AND ch.t_date BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND ch.from_comp = '2'\n" +
            "                           GROUP BY\n" +
            "                                  ch.t_date,\n" +
            "                                  b.branchCode,\n" +
            "                                  cm1.code_OF,\n" +
            "                                  cm.code_of\n" +
            "\n" +
            "                           UNION ALL\n" +
            "                           --To Company\n" +
            "                           SELECT '1' RANK,\n" +
            "                                  --SUM(ch.amount) debit,\n" +
            "                                  --0 credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(ch.amount)\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
            "                                  END     credit,\n" +
            "                                  'Received From  ' + CAST(cm.codE_of AS VARCHAR)\n" +
            "                                  cashtype,\n" +
            "                                  ch.t_date date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm1.code_of company\n" +
            "                           FROM   cih_transfer ch\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ch.BRANCH\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ch.from_comp\n" +
            "                                  INNER JOIN COMPANY_OF cm1\n" +
            "                                       ON  cm1.code_OF = ch.to_comp\n" +
            "                           WHERE  1 = 1 --AND ch.BRANCH = @BranchCode\n" +
            "                                  AND ch.t_date BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND ch.to_comp = '2'\n" +
            "                           GROUP BY\n" +
            "                                  ch.t_date,\n" +
            "                                  b.branchCode,\n" +
            "                                  cm.code_of,\n" +
            "                                  cm1.code_OF\n" +
            "                       )            a\n" +
            "            ) BB\n" +
            "            ON  cr.company = bb.company\n" +
            "            AND cr.branch = bb.branch\n" +
            "       INNER JOIN Branches b\n" +
            "            ON  b.branchCode = cr.branch\n" +
            "       INNER JOIN Zones z ON z.zoneCode = b.zoneCode \n" +
            "       INNER JOIN Company c\n" +
            "            ON  c.Id = cr.company\n" +
            "WHERE  cr.company = '2'\n" +
            "       AND cr.[month] = @Month\n" +
            ZoneID + "\n" +
            BranchID + "\n" +
            "           --AND cr.branch = @BranchCode\n" +
            "GROUP BY\n" +
            "       cr.company,\n" +
            "       cr.branch,\n" +
            "       cr.OPENING_VALUE,\n" +
            "       cr.[VALUE],\n" +
            "       cr.[year],\n" +
            "       cr.[month],\n" +
            "       b.name,\n" +
            "       c.companyName\n" +
            "\n \n" +
            "SELECT * FROM CLOSING_BALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " c where c.[year] = @Year \n" +
            "UNION ALL \n" +
            "SELECT * FROM CLOSING_BALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " c where c.[year] = @Year\n" +
            "ORDER BY\n" +
            "       CAST(branch AS INT),\n" +
            "       2 \n\n" +
            "DROP TABLE CLOSING_BALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " \n" +
            "DROP TABLE CLOSING_BALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " \n";



            #endregion

            string sqlString = "SELECT  \n"
               + "----- PC -- CLOSING BALANCES REPORT (Opening Balance) \n"
               + " --" + HttpContext.Current.Session["U_NAME"].ToString() + "\n"
               + "       cr.branch, \n"
               + "       b.name             BranchName, \n"
               + "       cr.company, \n"
               + "       c.companyName, \n"
               + "       cr.[year], \n"
               + "       cr.[month], \n"
               + "       SUM(BB.dnote)      DNOTE, \n"
               + "       SUM(BB.cnote)      CNOTE, \n"
               + "       cr.OPENING_VALUE, \n"
               + "       ISNULL( \n"
               + "           ROUND((SUM(bb.dnote) + cr.OPENING_VALUE - SUM(bb.cnote)), 0), \n"
               + "           0 \n"
               + "       )                  ClosingBalance, \n"
               + "       cr.[VALUE], \n"
               + "       ISNULL( \n"
               + "           ROUND((SUM(bb.dnote) + cr.OPENING_VALUE - SUM(bb.cnote)), 0), \n"
               + "           0 \n"
               + "       ) - cr.[VALUE]     DIFF \n"
               + "INTO CLOSING_BALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " \n"
               + "FROM   CIH_remainings cr \n"
               + "       LEFT OUTER JOIN ( \n"
               + "       	 \n"
               + "       	----------------------------- \n"
               + "     \n"
               + "SELECT DISTINCT a.rank, \n"
               + "       CONVERT(VARCHAR, a.date, 103)     date, \n"
               + "       a.cashtype, \n"
               + "       a.debit                           dnote, \n"
               + "       a.credit                          cnote, \n"
               + "       a.branch, \n"
               + "       company \n"
               + "FROM   ( \n"
               + "           SELECT '1' RANK, \n"
               + "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit, \n"
               + "                  0               credit, \n"
               + "                   CASE WHEN PV.RefNo IS NULL OR PV.RefNo = '' THEN 'Cash In Hand (' + ISNULL(pt.name, '') + ')' ELSE 'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo END cashtype,  \n"
               + "                  VoucherDate     date, \n"
               + "                  b.branchCode        branch, \n"
               + "                  '1' company \n"
               + "           FROM   PaymentVouchers pv \n"
               + "                  LEFT OUTER JOIN PaymentTypes pt \n"
               + "                       ON  pt.Id = pv.PaymentTypeId \n"
               + "                  LEFT OUTER JOIN ( \n"
               + "                           SELECT mp.voucherID, \n"
               + "                                  SUM(mp.amount) productAmount \n"
               + "                           FROM   MnP_PaymentVouchersProductBreakDown mp \n"
               + "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card') \n"
               + "                           GROUP BY \n"
               + "                                  mp.voucherID \n"
               + "                       ) mm \n"
               + "                       ON  mm.voucherID = pv.Id \n"
               + "                  INNER JOIN Branches b \n"
               + "            ON  b.branchCode = pv.BranchCode \n"
               + " WHERE 1 = 1 -- pv.BranchCode = @BranchCode \n"
               + "   and pv.VoucherDate between @StartDate AND @EndDate \n"
               + "       AND pv.CashPaymentSource IS NOT NULL \n"
               + "                  AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1') \n"
               + "                  AND pv.CashPaymentSource != '4' \n"
               + "           GROUP BY \n"
               + "                  VoucherDate, PV.RefNo, \n"
               + "                  pt.name, \n"
               + "                  b.branchCode \n"
               + " \n"
               + " \n"
               + "           UNION ALL \n"
               + " \n"
               + " \n"
               + "           SELECT '1' RANK, \n"
               + "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit, \n"
               + "                  0               credit, \n"
               + "                   CASE WHEN PV.RefNo IS NULL OR PV.RefNo = '' THEN 'Cash In Hand (' + ISNULL(pt.name, '') + ')' ELSE 'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo END cashtype,  \n"
               + "                  VoucherDate     date, \n"
               + "                  b.branchCode       branch, \n"
               + "                  '1' company \n"
               + "           FROM   PaymentVouchers pv \n"
               + "                  LEFT OUTER JOIN ( \n"
               + "                           SELECT mp.voucherID, \n"
               + "                                  SUM(mp.amount) productAmount \n"
               + "                           FROM   MnP_PaymentVouchersProductBreakDown mp \n"
               + "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card') \n"
               + "                           GROUP BY \n"
               + "                                  mp.voucherID \n"
               + "                       ) mm \n"
               + "                       ON  mm.voucherID = pv.Id \n"
               + "                  LEFT OUTER JOIN PaymentTypes pt \n"
               + "                       ON  pt.Id = pv.PaymentTypeId \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = pv.BranchCode \n"
               + " WHERE 1 = 1 --pv.BranchCode = @BranchCode \n"
               + "   and pv.VoucherDate between @StartDate AND @EndDate \n"
               + "                  AND pv.CreditClientId IS NOT NULL \n"
               + "                 AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1')  \n"
               + "           GROUP BY \n"
               + "                  VoucherDate, PV.RefNo, \n"
               + "                  pt.name, \n"
               + "                  b.branchCode \n"
               + "           UNION ALL \n"
               + " \n"
               + " \n"
               + "           SELECT '1' RANK, \n"
               + "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit, \n"
               + "                  0               credit, \n"
               + "                   CASE WHEN PV.RefNo IS NULL OR PV.RefNo = '' THEN 'Cash In Hand (' + ISNULL(pt.name, '') + ')'  ELSE 'Cash In Hand (' + ISNULL(pt.name, '') + ')' + ' Ref No# '+PV.RefNo END cashtype,  \n"
               + "                  VoucherDate     date, \n"
               + "                  b.branchCode        branch, \n"
               + "                  '1' company \n"
               + "           FROM   PaymentVouchers pv \n"
               + "                  LEFT OUTER JOIN ( \n"
               + "                           SELECT mp.voucherID, \n"
               + "                                  SUM(mp.amount) productAmount \n"
               + "                           FROM   MnP_PaymentVouchersProductBreakDown mp \n"
               + "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card') \n"
               + "                           GROUP BY \n"
               + "                                  mp.voucherID \n"
               + "                       ) mm \n"
               + "                       ON  mm.voucherID = pv.Id \n"
               + "                  LEFT OUTER JOIN PaymentTypes pt \n"
               + "                       ON  pt.Id = pv.PaymentTypeId \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = pv.BranchCode \n"
               + " WHERE 1 = 1 --pv.BranchCode = @BranchCode \n"
               + "   and pv.VoucherDate between @StartDate AND @EndDate \n"
               + "                  AND pv.CreditClientId IS NULL \n"
               + "                  AND pv.ClientGroupId IS NOT NULL \n"
               + "                 AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1')  \n"
               + "           GROUP BY \n"
               + "                  VoucherDate, PV.RefNo,  \n"
               + "                  pt.name, \n"
               + "                  b.branchCode \n"
               + "           UNION ALL \n"
               + " \n"
               + "           SELECT '2' RANK, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1) \n"
               + "                       ELSE 0 \n"
               + "                  END               debit, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(pv.Amount) < 0 THEN 0 \n"
               + "                       ELSE SUM(pv.Amount) \n"
               + "                  END               credit, \n"
               + "                  CASE WHEN (SUM(pv.Amount) < 0 OR pv.ldesc is not null)  THEN pv.LDESC ELSE  \n"
               + "                  'Deposit To Bank: ' + isnull(bo.bank_name,'') + ' (Dslip No: ' + isnull(pv.DepositSlipNo,'')  \n"
               + "                  + ')' END cashtype,  \n"
               + "                  pv.ChequeDate     date, \n"
               + "                  b.branchCode        branch, \n"
               + "                  pv.company company \n"
               + "           FROM   deposit_to_bank pv \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = pv.BranchCode \n"
               + "                  LEFT OUTER JOIN banks_of bo \n"
               + "                       ON  bo.bank_code = pv.DepositSlipBankID \n"
               + " WHERE 1 = 1 --pv.BranchCode = @BranchCode \n"
               + "   and pv.ChequeDate between @StartDate AND @EndDate \n"
               + "           GROUP BY \n"
               + "                  ChequeDate, \n"
               + "                  b.branchCode, \n"
               + "                  pv.Id, \n"
               + "                  bo.bank_name,pv.company, \n"
               + "                  pv.DepositSlipNo, pv.LDESC \n"
               + " \n"
               + " \n"
               + "           UNION ALL \n"
               + " \n"
               + "           SELECT '2' RANK, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(pv.AMOUNT) < 0 THEN (SUM(pv.AMOUNT) * -1) \n"
               + "                       ELSE 0 \n"
               + "                  END                       debit, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(pv.Amount) < 0 THEN 0 \n"
               + "                       ELSE SUM(pv.AMOUNT) \n"
               + "                  END                       credit, \n"
               + "                  'Transferred To Petty Cash (From : ' + pc.[desc] + ')' \n"
               + "                  cashtype, \n"
               + "                  pv.[date]                 date, \n"
               + "                  b.branchCode                 branch, \n"
               + "                  cm.code_OF               company \n"
               + "           FROM   PC_CIH_detail pv \n"
               + "                  INNER JOIN PC_CIH_head ph \n"
               + "                       ON  ph.ID = pv.head_id \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = ph.BRANCH \n"
               + "                  INNER JOIN PC_cash_mode pc \n"
               + "                       ON  pc.ID = pv.cash_type \n"
               + "                       AND pv.cash_type = '1' \n"
               + "                       AND pc.ID = '1' \n"
               + "                  INNER JOIN COMPANY_OF cm \n"
               + "                       ON  cm.code_OF = ph.COMPANY \n"
               + "   and ph.COMPANY='1' \n"
               + " WHERE 1 = 1 --ph.BRANCH = @BranchCode \n"
               + "   and pv.[date] between @StartDate AND @EndDate \n"
               + "           GROUP BY \n"
               + "                  [date], \n"
               + "                  b.branchCode, \n"
               + "                  pc.[desc], \n"
               + "                  cm.code_OF \n\n"
               + "           UNION ALL \n"
               + "           --From Company \n"
               + "           SELECT '2' RANK, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(ch.AMOUNT) < 0 THEN (SUM(ch.AMOUNT) * -1) \n"
               + "                       ELSE 0 \n"
               + "                  END              debit, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(ch.Amount) < 0 THEN 0 \n"
               + "                       ELSE SUM(ch.AMOUNT) \n"
               + "                  END              credit, \n"
               + "                  'Transfered To  ' + isnull(cm1.sdesc_OF,'') + ' Remarks: '+ch.remarks cashtype, \n"
               + "                  ch.t_date        date, \n"
               + "                  b.branchCode        branch, \n"
               + "                  cm1.sdesc_OF     company \n"
               + "           FROM   cih_transfer ch \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = ch.BRANCH \n"
               + "                  INNER JOIN COMPANY_OF cm \n"
               + "                       ON  cm.code_OF = ch.from_comp \n"
               + "                  INNER JOIN COMPANY_OF cm1 \n"
               + "                       ON  cm1.code_OF = ch.to_comp \n"
               + " WHERE 1 = 1 --ch.BRANCH = @BranchCode \n"
               + "   and ch.t_date between @StartDate AND @EndDate \n"
               + "  and ch.from_comp='1' \n"
               + "           GROUP BY \n"
               + "                  ch.t_date, \n"
               + "                  b.branchCode, \n"
               + "                  cm1.sdesc_OF,ch.remarks  \n"
               + " \n"
               + "           UNION ALL \n"
               + "           --To Company \n"
               + "           SELECT '1' RANK, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(ch.amount) < 0 THEN 0 \n"
               + "                       ELSE SUM(ch.amount) \n"
               + "                  END             debit, \n"
               + "                  CASE \n"
               + "                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1) \n"
               + "                  END             credit, \n"
               + "                  'Received From  ' + isnull(cm.sdesc_OF,'') + ' Remarks: '+ch.remarks cashtype, \n"
               + "                  ch.t_date       date, \n"
               + "                  b.branchCode        branch, \n"
               + "                  cm.sdesc_OF     company \n"
               + "           FROM   cih_transfer ch \n"
               + "                  INNER JOIN Branches b \n"
               + "                       ON  b.branchCode = ch.BRANCH \n"
               + "                   left outer  JOIN COMPANY_OF cm \n"
               + "                       ON  cm.code_OF = ch.from_comp \n"
               + "                  INNER JOIN COMPANY_OF cm1 \n"
               + "                       ON  cm1.code_OF = ch.to_comp \n"
               + " WHERE 1 = 1 --ch.BRANCH = @BranchCode \n"
               + "   and ch.t_date between @StartDate AND @EndDate \n"
               + "  and ch.to_comp='1' \n"
               + "           GROUP BY \n"
               + "                  ch.t_date, \n"
               + "                  b.branchCode, \n"
               + "                  cm.sdesc_OF, ch.remarks  \n"
               + "       )                                 a \n"
               + " \n"
               + "       	----------------------------- \n"
               + "       	 \n"
               + "       	 \n"
               + "       	) BB \n"
               + "            ON  cr.company = bb.company \n"
               + "            AND cr.branch = bb.branch \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.branchCode = cr.branch \n"
               + "       INNER JOIN Zones z ON z.zoneCode = b.zoneCode  \n"
               + "       INNER JOIN Company c \n"
               + "            ON  c.Id = cr.company \n"
               + "WHERE  cr.company = '1' \n"
               + "       AND cr.[month] = @Month \n"
               + "           --AND cr.branch = @BranchCode \n"
               + ZoneID + "\n"
               + BranchID + "\n"
               + " \n"
               + "GROUP BY \n"
               + "       cr.company, \n"
               + "       b.name, \n"
               + "       c.companyName, \n"
               + "       cr.branch, \n"
               + "       cr.OPENING_VALUE, \n"
               + "       cr.[VALUE], \n"
               + "       cr.[year], \n"
               + "       cr.[month] \n"
               + "       --Company 1 Ends here \n" +
            //"       UNION ALL\n" +
            "\n" +
            "       --Company 2 Starts Here\n" +
            "SELECT cr.branch,\n" +
            "       b.name                       BranchName,\n" +
            "       cr.company,\n" +
            "       c.companyName,\n" +
            "       cr.[year],\n" +
            "       cr.[month],\n" +
            "       ISNULL(SUM(BB.dnote), 0)     Dnote,\n" +
            "       ISNULL(SUM(BB.cnote), 0)     CNOTE,\n" +
            "       cr.OPENING_VALUE,\n" +
            "       ISNULL(\n" +
            "           ROUND(\n" +
            "               ISNULL(SUM(BB.dnote), 0) - ISNULL(SUM(BB.cnote), 0) + cr.OPENING_VALUE,\n" +
            "               0\n" +
            "           ),\n" +
            "           0\n" +
            "       )                            closingBalance,\n" +
            "       cr.[VALUE],\n" +
            "       ISNULL(\n" +
            "           ROUND(\n" +
            "               ISNULL(SUM(BB.dnote), 0) - ISNULL(SUM(BB.cnote), 0) + cr.OPENING_VALUE,\n" +
            "               0\n" +
            "           ),\n" +
            "           0\n" +
            "       ) - cr.[VALUE]               DIFF\n" +
            "INTO CLOSING_BALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " \n" +
            "FROM   CIH_remainings cr\n" +
            "       LEFT OUTER JOIN (\n" +
            "                SELECT a.rank,\n" +
            "                       CONVERT(VARCHAR, a.date, 103) date,\n" +
            "                       a.cashtype,\n" +
            "                       a.debit      dnote,\n" +
            "                       a.credit     cnote,\n" +
            "                       a.branch,\n" +
            "                       company\n" +
            "                FROM   (\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(pv.Amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(pv.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Deposit To Bank' cashtype,\n" +
            "                                  pv.ChequeDate date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  '2' company\n" +
            "                           FROM   deposit_to_bank pv\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = pv.BranchCode\n" +
            "                           WHERE  1 = 1 --AND pv.BranchCode = @BranchCode\n" +
            "                                  AND pv.company = '2'\n" +
            "                                  AND pv.ChequeDate BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                           GROUP BY\n" +
            "                                  ChequeDate,\n" +
            "                                  b.branchCode,\n" +
            "                                  pv.Id\n" +
            "\n" +
            "                           UNION ALL\n" +
            "\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(pv.Amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(pv.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Transferred To Petty Cash (From : ' + pc.[desc]\n" +
            "                                  + ')'\n" +
            "                                  cashtype,\n" +
            "                                  pv.[date] date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm.code_of company\n" +
            "                           FROM   PC_CIH_detail pv\n" +
            "                                  INNER JOIN PC_CIH_head ph\n" +
            "                                       ON  ph.ID = pv.head_id\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ph.BRANCH\n" +
            "                                  INNER JOIN PC_cash_mode pc\n" +
            "                                       ON  pc.ID = pv.cash_type\n" +
            "                                       AND pv.cash_type = '1'\n" +
            "                                       AND pc.ID = '1'\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ph.COMPANY\n" +
            "                                       AND ph.COMPANY = '2'\n" +
            "                           WHERE  1 = 1 --AND ph.BRANCH = @BranchCode\n" +
            "                                  AND pv.[date]BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                           GROUP BY\n" +
            "                                  [date],\n" +
            "                                  b.branchCode,\n" +
            "                                  pc.[desc],\n" +
            "                                  cm.code_of\n" +
            "                           UNION ALL\n" +
            "                           --From Company\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  --0 debit,\n" +
            "                                  --SUM(ch.amount) credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.Amount) < 0 THEN (SUM(ch.Amount) * -1)\n" +
            "                                       ELSE 0\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(ch.Amount)\n" +
            "                                  END     credit,\n" +
            "                                  'Transfered To  ' + CAST(cm1.code_of AS VARCHAR)\n" +
            "                                  cashtype,\n" +
            "                                  ch.t_date date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm.code_OF company\n" +
            "                           FROM   cih_transfer ch\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ch.BRANCH\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ch.from_comp\n" +
            "                                  INNER JOIN COMPANY_OF cm1\n" +
            "                                       ON  cm1.code_OF = ch.to_comp\n" +
            "                           WHERE  1 = 1 --AND ch.BRANCH = @BranchCode\n" +
            "                                  AND ch.t_date BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND ch.from_comp = '2'\n" +
            "                           GROUP BY\n" +
            "                                  ch.t_date,\n" +
            "                                  b.branchCode,\n" +
            "                                  cm1.code_OF,\n" +
            "                                  cm.code_of\n" +
            "\n" +
            "                           UNION ALL\n" +
            "                           --To Company\n" +
            "                           SELECT '1' RANK,\n" +
            "                                  --SUM(ch.amount) debit,\n" +
            "                                  --0 credit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
            "                                       ELSE SUM(ch.amount)\n" +
            "                                  END     debit,\n" +
            "                                  CASE\n" +
            "                                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
            "                                  END     credit,\n" +
            "                                  'Received From  ' + CAST(cm.codE_of AS VARCHAR)\n" +
            "                                  cashtype,\n" +
            "                                  ch.t_date date,\n" +
            "                                  b.branchCode branch,\n" +
            "                                  cm1.code_of company\n" +
            "                           FROM   cih_transfer ch\n" +
            "                                  INNER JOIN Branches b\n" +
            "                                       ON  b.branchCode = ch.BRANCH\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = ch.from_comp\n" +
            "                                  INNER JOIN COMPANY_OF cm1\n" +
            "                                       ON  cm1.code_OF = ch.to_comp\n" +
            "                           WHERE  1 = 1 --AND ch.BRANCH = @BranchCode\n" +
            "                                  AND ch.t_date BETWEEN @StartDate AND\n" +
            "                                      @EndDate\n" +
            "                                  AND ch.to_comp = '2'\n" +
            "                           GROUP BY\n" +
            "                                  ch.t_date,\n" +
            "                                  b.branchCode,\n" +
            "                                  cm.code_of,\n" +
            "                                  cm1.code_OF\n" +
            "                       )            a\n" +
            "            ) BB\n" +
            "            ON  cr.company = bb.company\n" +
            "            AND cr.branch = bb.branch\n" +
            "       INNER JOIN Branches b\n" +
            "            ON  b.branchCode = cr.branch\n" +
            "       INNER JOIN Zones z ON z.zoneCode = b.zoneCode \n" +
            "       INNER JOIN Company c\n" +
            "            ON  c.Id = cr.company\n" +
            "WHERE  cr.company = '2'\n" +
            "       AND cr.[month] = @Month\n" +
            ZoneID + "\n" +
            BranchID + "\n" +
            "           --AND cr.branch = @BranchCode\n" +
            "GROUP BY\n" +
            "       cr.company,\n" +
            "       cr.branch,\n" +
            "       cr.OPENING_VALUE,\n" +
            "       cr.[VALUE],\n" +
            "       cr.[year],\n" +
            "       cr.[month],\n" +
            "       b.name,\n" +
            "       c.companyName\n" +
            "\n \n" +
            "SELECT * FROM CLOSING_BALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " c where c.[year] = @Year \n" +
            "UNION ALL \n" +
            "SELECT * FROM CLOSING_BALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " c where c.[year] = @Year\n" +
            "ORDER BY\n" +
            "       CAST(branch AS INT),\n" +
            "       2 \n\n" +
            "DROP TABLE CLOSING_BALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " \n" +
            "DROP TABLE CLOSING_BALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " \n";





            if (DateTime.Parse(endDate).Month != DateTime.Parse(endDate).AddDays(1).Month)
            {
                sqlString = "select cr.Branch, cr.Company, cr.Opening_Value ClosingBalance from cih_remainings cr where cr.month = @Month and cr.year = @Year and @StartDate = @StartDate and @EndDate = @EndDate";
            }
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3000;
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.CommandText = sqlString;

                SqlDataAdapter oda = new SqlDataAdapter();
                oda.SelectCommand = cmd;
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }

            return dt;
        }

        public DataTable GetPCOpeningBalances(string requiredDate)
        {

            string month = "";
            string year = "";
            string startDate = "";

            month = requiredDate.Split('-')[1];
            month = int.Parse(month).ToString();

            year = requiredDate.Split('-')[0];
            year = int.Parse(year).ToString();

            startDate = $@"{year}/{month}/1";
            string endDate = DateTime.Parse(requiredDate).AddDays(-1).ToString("yyyy-MM-dd");
            //startDate = DateTime.Parse("01" + "/" + requiredDate.Split('-')[1] + "/" + requiredDate.Split('-')[0]).ToString("yyyy-MM-dd");
            //string endDate = DateTime.Parse(requiredDate.Split('-')[2] + "/" + requiredDate.Split('-')[1] + "/" + requiredDate.Split('-')[0]).AddDays(-1).ToString("yyyy-MM-dd");



            string sqlString = "/************************************************************\n" +
            " * Code formatted by SoftTree SQL Assistant © v6.3.153\n" +
            " * Time: 12/19/2018 4:10:40 PM\n" +
            " ************************************************************/\n" +
            "SELECT cr.branch,\n" +
            "       cr.company,\n" +
            "       cr.[month],\n" +
            "       cr.[year],\n" +
            "       ISNULL(SUM(BB.dnote), 0)     DNOTE,\n" +
            "       ISNULL(SUM(BB.cnote), 0)     CNOTE,\n" +
            "       cr.opening_pcash,\n" +
            "       ISNULL(\n" +
            "           (\n" +
            "               ISNULL(SUM(bb.dnote), 0) + ISNULL(cr.opening_pcash, 0) - ISNULL(SUM(bb.cnote), 0)\n" +
            "           ),\n" +
            "           0\n" +
            "       )\n" +
            "       ClosingPcash,\n" +
            "       cr.petty_cash,\n" +
            "       (\n" +
            "           ISNULL(\n" +
            "               (\n" +
            "                   ISNULL(SUM(bb.dnote), 0) + ISNULL(cr.opening_pcash, 0) -\n" +
            "                   ISNULL(SUM(bb.cnote), 0)\n" +
            "               ),\n" +
            "               0\n" +
            "           ) - cr.petty_cash\n" +
            "       )                            DIFF\n" +
            "FROM   CIH_remainings cr\n" +
            "       LEFT OUTER JOIN (\n" +
            "                SELECT a.rank,\n" +
            "                       a.COMPANY,\n" +
            "                       a.YEAR,\n" +
            "                       a.MONTH,\n" +
            "                       a.Date,\n" +
            "                       a.Debit          dnote,\n" +
            "                       a.Credit         cnote,\n" +
            "                       b.branchCode     branch,\n" +
            "                       CASE e.name\n" +
            "                            WHEN '0' THEN 'ALL'\n" +
            "                            ELSE ISNULL(e.name, 'ALL')\n" +
            "                       END              ec,\n" +
            "                       a.expense,\n" +
            "                       a.[description],\n" +
            "                       a.narrate,\n" +
            "                       a.ID,\n" +
            "                       a.comp1\n" +
            "                FROM   (\n" +
            "                           SELECT '1' RANK,\n" +
            "                                  pch.COMPANY,\n" +
            "                                  pch.BRANCH,\n" +
            "                                  pch.[YEAR],\n" +
            "                                  pch.[MONTH],\n" +
            "                                  CONVERT(VARCHAR, pcd.DATE, 103) \"Date\",\n" +
            "                                  pch.express_center,\n" +
            "                                  0 AS Credit,\n" +
            "                                  pcd.AMOUNT AS Debit,\n" +
            "                                  'Petty Cash' expense,\n" +
            "                                  'AMOUNT TRANSFERRED FROM CIH TO PETTY CASH' +\n" +
            "                                  '(' + pmode.[desc]\n" +
            "                                  + ')' DESCRIPTION,\n" +
            "                                  '' narrate,\n" +
            "                                  pch.ID,\n" +
            "                                  cm.code_OF comp1\n" +
            "                           FROM   PC_CIH_head AS pch\n" +
            "                                  INNER JOIN PC_CIH_detail AS pcd\n" +
            "                                       ON  pcd.head_id = pch.ID\n" +
            "                                  LEFT OUTER JOIN PC_cash_mode pmode\n" +
            "                                       ON  pmode.ID = pcd.cash_type\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = pch.COMPANY\n" +
            "                                           --AND pch.COMPANY = '1'\n" +
            "                           WHERE  pcd.[DATE] BETWEEN @StartDate --CONVERT(DATETIME, '01/07/2018', 103) AND\n" +
            "                                  AND @EndDate --CONVERT(DATETIME, '31/07/2018', 103)\n" +
            "                                               --AND pch.BRANCH = '4'\n" +
            "                           UNION ALL\n" +
            "\n" +
            "                           SELECT '2' RANK,\n" +
            "                                  pch.COMPANY,\n" +
            "                                  pch.BRANCH,\n" +
            "                                  pch.[YEAR],\n" +
            "                                  pch.[MONTH],\n" +
            "                                  CONVERT(VARCHAR, pcd.DATE, 103) \"Date\",\n" +
            "                                  pch.express_center,\n" +
            "                                  pcd.AMOUNT AS Credit,\n" +
            "                                  0 AS Debit,\n" +
            "                                  m.description expense,\n" +
            "                                  s.sub_desc DESCRIPTION,\n" +
            "                                  pcd.NARRATE,\n" +
            "                                  pch.ID,\n" +
            "                                  cm.code_OF comp1\n" +
            "                           FROM   PC_head AS pch\n" +
            "                                  INNER JOIN PC_detail AS pcd\n" +
            "                                       ON  pcd.head_id = pch.ID --and pch.CREATED_BY=pcd.CREATED_BY\n" +
            "\n" +
            "                                  LEFT OUTER JOIN pc_mainHead m\n" +
            "                                       ON  pcd.expense = m.code\n" +
            "                                  LEFT OUTER JOIN pc_subHead s\n" +
            "                                       ON  pcd.[desc] = s.subcode\n" +
            "                                       AND pcd.EXPENSE = s.headcode\n" +
            "                                  INNER JOIN COMPANY_OF cm\n" +
            "                                       ON  cm.code_OF = pch.COMPANY\n" +
            "                                           --AND pch.COMPANY = '1'\n" +
            "                           WHERE  pcd.[DATE] BETWEEN @StartDate --CONVERT(DATETIME, '01/07/2018', 103) AND\n" +
            "                                  AND @EndDate --CONVERT(DATETIME, '31/07/2018', 103)\n" +
            "                                               --AND pch.BRANCH = '4'\n" +
            "                       ) a\n" +
            "                       INNER JOIN Branches b\n" +
            "                            ON  a.BRANCH = b.branchCode\n" +
            "                       LEFT OUTER JOIN ExpressCenters e\n" +
            "                            ON  a.express_center = e.expressCenterCode\n" +
            "            ) BB\n" +
            "            ON  cr.company = bb.company\n" +
            "            AND cr.branch = bb.branch\n" +
            "            left JOIN Branches b ON  bb.BRANCH = b.branchCode \n" +
            "            left JOIN Zones z  ON z.zoneCode = b.zoneCode \n" +
            "                --WHERE  --cr.company = '1'\n" +
            "WHERE  cr.[month] = @month\n" +
            "       AND cr.[year] = @Year\n" +
            "           --AND cr.branch = '4'\n" +
            ZoneID + "\n" +
            BranchID + "\n" +
            "GROUP BY\n" +
            "       cr.company,\n" +
            "       cr.branch,\n" +
            "       cr.opening_pcash,\n" +
            "       cr.petty_cash,\n" +
            "       cr.[month],\n" +
            "       cr.[year]\n" +
            "ORDER BY\n" +
            "       cr.branch,\n" +
            "       2\n" +
            "\n" +
            "";
            if (DateTime.Parse(endDate).Month != DateTime.Parse(endDate).AddDays(1).Month)
            {
                sqlString = "select cr.Branch, cr.Company, cr.Opening_PCash ClosingPCash from cih_remainings cr where cr.month = @Month and cr.year = @Year and @StartDate = @StartDate and @EndDate = @EndDate";
            }

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {


                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataAdapter oda = new SqlDataAdapter();
                oda.SelectCommand = cmd;
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }

            return dt;
        }

        public DataTable GetAllBranches()
        {

            string sqlString = "SELECT DISTINCT CR.BRANCH,\n" +
            "         B.NAME BRANCHNAME,\n" +
            "         CR.COMPANY,\n" +
            "         C.COMPANYNAME,\n" +
            "         Z.\n" +
            "  FROM   CIH_REMAININGS CR\n" +
            "         INNER JOIN BRANCHES B\n" +
            "              ON  B.BRANCHCODE = CR.BRANCH\n" +
            "         INNER JOIN ZONES z\n" +
            "              ON Z.ZoneCode = B.ZoneCode\n" +
            "\n" +
            "         INNER JOIN COMPANY C\n" +
            "              ON  C.ID = CR.COMPANY\n" +
            "  ORDER BY\n" +
            "         B.NAME,\n" +
            "         CR.COMPANY";



            sqlString = "SELECT DISTINCT CR.BRANCH,\n" +
            "                B.NAME        BRANCHNAME,\n" +
            "                CR.COMPANY,\n" +
            "                C.COMPANYNAME,\n" +
            "                Z.NAME        ZONE\n" +
            "  FROM CIH_REMAININGS CR\n" +
            " INNER JOIN BRANCHES B\n" +
            "    ON B.BRANCHCODE = CR.BRANCH\n" +
            " INNER JOIN ZONES Z\n" +
            "    ON Z.ZONECODE = B.ZONECODE\n" +
            " INNER JOIN COMPANY C\n" +
            "    ON C.ID = CR.COMPANY\n" +
            ZoneID_ + "\n" +
            BranchID + "\n" +
            " ORDER BY B.NAME, CR.COMPANY";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {


                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                SqlDataAdapter oda = new SqlDataAdapter();
                oda.SelectCommand = cmd;
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }

            return dt;
        }

        public DataTable GetCashInHandLedgerForDay(string date)
        {
            //  string Date = "";

            string sqlString = "DECLARE @StartDate VARCHAR(10) \n"
               + "	DECLARE @EndDate VARCHAR(10) \n"
               + "	DECLARE @Month INT \n"
               + "	DECLARE @Year INT \n"
               + "	 \n"
               + "	SET @StartDate = CAST('" + date + "' AS DATE) \n"
               + "	SET @EndDate = CAST('" + date + "' AS DATE) \n"
               + "	 \n"
               + "	SET @Month = MONTH(@StartDate) \n"
               + "	SET @Year = YEAR(@StartDate)	 \n"
               + "\n\n"
               + "SELECT distinct a.rank, \n"
               + "	       --CONVERT(VARCHAR, a.date, 103) date, \n"
               + "	       FORMAT(a.date, 'MM/dd/yyyy')     date, \n"
               + "	       a.cashtype, \n"
               + "	       a.debit                          dnote, \n"
               + "	       a.credit                         cnote, \n"
               + "	       a.branch, \n"
               + "	       company \n"
               + "	       INTO CLOSINGBALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " \n"
               + "	FROM   ( \n"
               + "	           SELECT distinct '1' RANK, \n"
               + "	                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0))  \n"
               + "	                  debit, \n"
               + "	                  0                credit, \n"
               + "	                  'Cash In Hand (' + ISNULL(pt.name, '')  \n"
               + "	                  + \n"
               + "	                  ')'  \n"
               + "	                  cashtype, \n"
               + "	                  VoucherDate      date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  '1' company \n"
               + "	           FROM   PaymentVouchers pv \n"
               + "	                  LEFT OUTER JOIN ( \n"
               + "	                           SELECT mp.voucherID, \n"
               + "	                                  SUM(mp.amount)  \n"
               + "	                                  productAmount \n"
               + "	                           FROM   MnP_PaymentVouchersProductBreakDown  \n"
               + "	                                  mp \n"
               + "	                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card') \n"
               + "	                           GROUP BY \n"
               + "	                                  mp.voucherID \n"
               + "	                       ) mm \n"
               + "	                       ON  mm.voucherID = pv.Id \n"
               + "	                  LEFT OUTER JOIN PaymentTypes pt \n"
               + "	                       ON  pt.Id = pv.PaymentTypeId \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = pv.BranchCode \n"
               + "	           WHERE  1 = 1 --AND pv.BranchCode = '4' \n"
               + "	                  AND pv.VoucherDate BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	                  AND pv.CashPaymentSource IS NOT  \n"
               + "	                      NULL \n"
               + "	                  AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1') \n"
               + "	                  AND pv.CashPaymentSource != '4' \n"
               + "	           GROUP BY \n"
               + "	                  VoucherDate, \n"
               + "	                  pt.name, \n"
               + "	                  b.branchCode \n"
               + "	            \n"
               + "	            \n"
               + "	           UNION ALL \n"
               + "	            \n"
               + "	            \n"
               + "	           SELECT '1' RANK, \n"
               + "	                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0))  \n"
               + "	                  debit, \n"
               + "	                  0                credit, \n"
               + "	                  'Cash In Hand (' + ISNULL(pt.name, '')  \n"
               + "	                  + \n"
               + "	                  ')'  \n"
               + "	                  cashtype, \n"
               + "	                  VoucherDate      date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  '1' company \n"
               + "	           FROM   PaymentVouchers pv \n"
               + "	                  LEFT OUTER JOIN ( \n"
               + "	                           SELECT mp.voucherID, \n"
               + "	                                  SUM(mp.amount)  \n"
               + "	                                  productAmount \n"
               + "	                           FROM   MnP_PaymentVouchersProductBreakDown  \n"
               + "	                                  mp \n"
               + "	                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card') \n"
               + "	                           GROUP BY \n"
               + "	                                  mp.voucherID \n"
               + "	                       ) mm \n"
               + "	                       ON  mm.voucherID = pv.Id \n"
               + "	                  LEFT OUTER JOIN PaymentTypes pt \n"
               + "	                       ON  pt.Id = pv.PaymentTypeId \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = pv.BranchCode \n"
               + "	           WHERE  1 = 1 --AND pv.BranchCode = '4' \n"
               + "	                  AND pv.VoucherDate BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	                  AND pv.CreditClientId IS NOT  \n"
               + "	                      NULL \n"
               + "	                  AND pv.paymentsourceid = '1' \n"
               + "	           GROUP BY \n"
               + "	                  VoucherDate, \n"
               + "	                  pt.name, \n"
               + "	                  b.branchCode \n"
               + "	           UNION ALL \n"
               + "	           SELECT '1' RANK, \n"
               + "	                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0))  \n"
               + "	                  debit, \n"
               + "	                  0                credit, \n"
               + "	                  'Cash In Hand (' + ISNULL(pt.name, '')  \n"
               + "	                  + \n"
               + "	                  ')'  \n"
               + "	                  cashtype, \n"
               + "	                  VoucherDate      date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  '1' company \n"
               + "	           FROM   PaymentVouchers pv \n"
               + "	                  LEFT OUTER JOIN ( \n"
               + "	                           SELECT mp.voucherID, \n"
               + "	                                  SUM(mp.amount)  \n"
               + "	                                  productAmount \n"
               + "	                           FROM   MnP_PaymentVouchersProductBreakDown  \n"
               + "	                                  mp \n"
               + "	                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card') \n"
               + "	                           GROUP BY \n"
               + "	                                  mp.voucherID \n"
               + "	                       ) mm \n"
               + "	                       ON  mm.voucherID = pv.Id \n"
               + "	                  LEFT OUTER JOIN PaymentTypes pt \n"
               + "	                       ON  pt.Id = pv.PaymentTypeId \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = pv.BranchCode \n"
               + "	           WHERE  1 = 1 --AND pv.BranchCode = '4' \n"
               + "	                  AND pv.VoucherDate BETWEEN @StartDate AND @EndDate \n"
               //+ "	                  AND pv.ClientGroupId IS NOT NULL \n"
               //+ "	                      --AND pv.CreditClientId IS NULL \n"
               + "                  AND pv.CreditClientId IS NULL \n"
               + "                  AND pv.ClientGroupId IS NOT NULL \n"
               + "	                  AND pv.paymentsourceid = '1' \n"
               + "	           GROUP BY \n"
               + "	                  VoucherDate, \n"
               + "	                  pt.name, \n"
               + "	                  b.branchCode \n"
               + "	           UNION ALL \n"
               + "	            \n"
               + "	           SELECT '2' RANK, \n"
               + "	                  --0 debit, \n"
               + "	                  --SUM(pv.Amount) credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1) \n"
               + "	                       ELSE 0 \n"
               + "	                  END               debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(pv.Amount) \n"
               + "	                  END               credit, \n"
               + "	                  'Deposit To Bank ' + ISNULL(bo.bank_name, 'NA')  \n"
               + "	                  + ' Deposit Slip No: ' + ISNULL(pv.DepositSlipNo, 'NA')  \n"
               + "	                  cashtype, \n"
               + "	                  pv.ChequeDate     date, \n"
               + "	                  b.branchCode      branch, \n"
               + "	                  pv.company        company \n"
               + "	           FROM   deposit_to_bank pv \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = pv.BranchCode \n"
               + "	                  LEFT OUTER JOIN banks_of bo \n"
               + "	                       ON  bo.bank_code = pv.DepositSlipBankID \n"
               + "	           WHERE  1 = 1 -- AND pv.BranchCode = '4' \n"
               + "	                  AND pv.company = '1' \n"
               + "	                  AND pv.ChequeDate BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	           GROUP BY \n"
               + "	                  ChequeDate, \n"
               + "	                  b.branchCode, \n"
               + "	                  pv.Id, \n"
               + "	                  pv.company, \n"
               + "	                  bo.bank_name, \n"
               + "	                  pv.DepositSlipNo \n"
               + "	            \n"
               + "	            \n"
               + "	           UNION ALL \n"
               + "	            \n"
               + "	           SELECT '2' RANK, \n"
               + "	                  --0 debit, \n"
               + "	                  --SUM(pv.Amount) credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1) \n"
               + "	                       ELSE 0 \n"
               + "	                  END              debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(pv.Amount) \n"
               + "	                  END              credit, \n"
               + "	                  'Transferred To Petty Cash (From : '  \n"
               + "	                  + pc.[desc]  \n"
               + "	                  + ')'  \n"
               + "	                  cashtype, \n"
               + "	                  pv.[date]        date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  cm.code_OF \n"
               + "	           FROM   PC_CIH_detail pv \n"
               + "	                  INNER JOIN PC_CIH_head ph \n"
               + "	                       ON  ph.ID = pv.head_id \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = ph.BRANCH \n"
               + "	                  INNER JOIN PC_cash_mode pc \n"
               + "	                       ON  pc.ID = pv.cash_type \n"
               + "	                       AND pv.cash_type = '1' \n"
               + "	                       AND pc.ID = '1' \n"
               + "	                  INNER JOIN COMPANY_OF cm \n"
               + "	                       ON  cm.code_OF = ph.COMPANY \n"
               + "	                       AND ph.COMPANY = '1' \n"
               + "	           WHERE  1 = 1 -- AND ph.BRANCH = '4' \n"
               + "	                  AND pv.[date] BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	           GROUP BY \n"
               + "	                  [date], \n"
               + "	                  b.branchCode, \n"
               + "	                  pc.[desc], \n"
               + "	                  cm.code_OF \n"
               + "	           UNION ALL  \n"
               + "	           --From Company \n"
               + "	           SELECT '2' RANK, \n"
               + "	                  --0 debit, \n"
               + "	                  --SUM(ch.amount) credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.Amount) < 0 THEN (SUM(ch.Amount) * -1) \n"
               + "	                       ELSE 0 \n"
               + "	                  END              debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.Amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(ch.Amount) \n"
               + "	                  END              credit, \n"
               + "	                  'Transfered To  ' + CAST(cm1.code_of AS VARCHAR)  \n"
               + "	                  cashtype, \n"
               + "	                  ch.t_date        date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  cm.code_of       company \n"
               + "	           FROM   cih_transfer ch \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = ch.BRANCH \n"
               + "	                  INNER JOIN COMPANY_OF cm \n"
               + "	                       ON  cm.code_OF = ch.from_comp \n"
               + "	                  INNER JOIN COMPANY_OF cm1 \n"
               + "	                       ON  cm1.code_OF = ch.to_comp \n"
               + "	           WHERE  1 = 1 -- AND ch.BRANCH = '4' \n"
               + "	                  AND ch.t_date BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	                  AND ch.from_comp = '1' \n"
               + "	           GROUP BY \n"
               + "	                  ch.t_date, \n"
               + "	                  b.branchCode, \n"
               + "	                  cm.code_OF, \n"
               + "	                  cm1.code_OF \n"
               + "	            \n"
               + "	           UNION ALL \n"
               + "	           --To Company \n"
               + "	           SELECT '1' RANK, \n"
               + "	                  --SUM(ch.amount) debit, \n"
               + "	                  --0 credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(ch.amount) \n"
               + "	                  END              debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1) \n"
               + "	                  END              credit, \n"
               + "	                  'Received From  ' + CAST(cm.code_of AS VARCHAR)  \n"
               + "	                  cashtype, \n"
               + "	                  ch.t_date        date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  cm1.code_OF      company \n"
               + "	           FROM   cih_transfer ch \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = ch.BRANCH \n"
               + "	                  INNER JOIN COMPANY_OF cm \n"
               + "	                       ON  cm.code_OF = ch.from_comp \n"
               + "	                  INNER JOIN COMPANY_OF cm1 \n"
               + "	                       ON  cm1.code_OF = ch.to_comp \n"
               + "	           WHERE  1 = 1 -- AND ch.BRANCH = '4' \n"
               + "	                  AND ch.t_date BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	                  AND ch.to_comp = '1' \n"
               + "	           GROUP BY \n"
               + "	                  ch.t_date, \n"
               + "	                  b.branchCode, \n"
               + "	                  cm.code_OF, \n"
               + "	                  cm1.code_OF \n"
               + "	       )                                a \n"
               + "	 \n"
               + "	 \n"
               + "	---- TABLE 1 END \n"
               + "	 \n"
               + "	---- TABLE 2 START \n"
               + "	 \n"
               + "	 \n"
               + "	 \n"
               + "	SELECT distinct a.rank, \n"
               + "	       --CONVERT(VARCHAR, a.date, 103) date, \n"
               + "	       FORMAT(a.date, 'MM/dd/yyyy')     date, \n"
               + "	       a.cashtype, \n"
               + "	       a.debit                          dnote, \n"
               + "	       a.credit                         cnote, \n"
               + "	       a.branch, \n"
               + "	       company \n"
               + "	       INTO CLOSINGBALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " \n"
               + "	FROM   ( \n"
               + "	           SELECT '2' RANK, \n"
               + "	                  --0 debit, \n"
               + "	                  --SUM(pv.Amount) credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1) \n"
               + "	                       ELSE 0 \n"
               + "	                  END               debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(pv.Amount) \n"
               + "	                  END               credit, \n"
               + "	                  'Deposit To Bank ' + ISNULL(bo.bank_name, 'NA')  \n"
               + "	                  + ' Deposit Slip No: ' + ISNULL(pv.DepositSlipNo, 'NA')  \n"
               + "	                  cashtype, \n"
               + "	                  pv.ChequeDate     date, \n"
               + "	                  b.branchCode      branch, \n"
               + "	                  '2' company \n"
               + "	           FROM   deposit_to_bank pv \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = pv.BranchCode \n"
               + "	                  LEFT OUTER JOIN banks_of bo \n"
               + "	                       ON  bo.bank_code = pv.DepositSlipBankID \n"
               + "	           WHERE  1 = 1 --AND  pv.BranchCode = 4 \n"
               + "	                  AND pv.company = '2' \n"
               + "	                  AND pv.ChequeDate BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	           GROUP BY \n"
               + "	                  ChequeDate, \n"
               + "	                  b.branchCode, \n"
               + "	                  pv.Id, \n"
               + "	                  bo.bank_name, \n"
               + "	                  pv.DepositSlipNo \n"
               + "	            \n"
               + "	           UNION ALL \n"
               + "	            \n"
               + "	           SELECT '2' RANK, \n"
               + "	                  --0 debit, \n"
               + "	                  --SUM(pv.Amount) credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1) \n"
               + "	                       ELSE 0 \n"
               + "	                  END              debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(pv.Amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(pv.Amount) \n"
               + "	                  END              credit, \n"
               + "	                  'Transferred To Petty Cash (From : '  \n"
               + "	                  + pc.[desc]  \n"
               + "	                  + ')'  \n"
               + "	                  cashtype, \n"
               + "	                  pv.[date]        date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  cm.code_of       company \n"
               + "	           FROM   PC_CIH_detail pv \n"
               + "	                  INNER JOIN PC_CIH_head ph \n"
               + "	                       ON  ph.ID = pv.head_id \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = ph.BRANCH \n"
               + "	                  INNER JOIN PC_cash_mode pc \n"
               + "	                       ON  pc.ID = pv.cash_type \n"
               + "	                       AND pv.cash_type = '1' \n"
               + "	                       AND pc.ID = '1' \n"
               + "	                  INNER JOIN COMPANY_OF cm \n"
               + "	                       ON  cm.code_OF = ph.COMPANY \n"
               + "	                       AND ph.COMPANY = '2' \n"
               + "	           WHERE  1 = 1 --AND ph.BRANCH = 4 \n"
               + "	                  AND pv.[date]BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	           GROUP BY \n"
               + "	                  [date], \n"
               + "	                  b.branchCode, \n"
               + "	                  pc.[desc], \n"
               + "	                  cm.code_of \n"
               + "	           UNION ALL \n"
               + "	           --From Company \n"
               + "	           SELECT '2' RANK, \n"
               + "	                  --0 debit, \n"
               + "	                  --SUM(ch.amount) credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.Amount) < 0 THEN (SUM(ch.Amount) * -1) \n"
               + "	                       ELSE 0 \n"
               + "	                  END              debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.Amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(ch.Amount) \n"
               + "	                  END              credit, \n"
               + "	                  'Transfered To  ' + CAST(cm1.code_of AS VARCHAR)  \n"
               + "	                  cashtype, \n"
               + "	                  ch.t_date        date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  cm.code_OF       company \n"
               + "	           FROM   cih_transfer ch \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = ch.BRANCH \n"
               + "	                  INNER JOIN COMPANY_OF cm \n"
               + "	                       ON  cm.code_OF = ch.from_comp \n"
               + "	                  INNER JOIN COMPANY_OF cm1 \n"
               + "	                       ON  cm1.code_OF = ch.to_comp \n"
               + "	           WHERE  1 = 1 --AND ch.BRANCH = 4 \n"
               + "	                  AND ch.t_date BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	                  AND ch.from_comp = '2' \n"
               + "	           GROUP BY \n"
               + "	                  ch.t_date, \n"
               + "	                  b.branchCode, \n"
               + "	                  cm1.code_OF, \n"
               + "	                  cm.code_of \n"
               + "	            \n"
               + "	           UNION ALL \n"
               + "	           --To Company \n"
               + "	           SELECT '1' RANK, \n"
               + "	                  --SUM(ch.amount) debit, \n"
               + "	                  --0 credit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.amount) < 0 THEN 0 \n"
               + "	                       ELSE SUM(ch.amount) \n"
               + "	                  END              debit, \n"
               + "	                  CASE  \n"
               + "	                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1) \n"
               + "	                  END              credit, \n"
               + "	                  'Received From  ' + CAST(cm.codE_of AS VARCHAR)  \n"
               + "	                  cashtype, \n"
               + "	                  ch.t_date        date, \n"
               + "	                  b.branchCode     branch, \n"
               + "	                  cm1.code_of      company \n"
               + "	           FROM   cih_transfer ch \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = ch.BRANCH \n"
               + "	                  INNER JOIN COMPANY_OF cm \n"
               + "	                       ON  cm.code_OF = ch.from_comp \n"
               + "	                  INNER JOIN COMPANY_OF cm1 \n"
               + "	                       ON  cm1.code_OF = ch.to_comp \n"
               + "	           WHERE  1 = 1 --AND ch.BRANCH = 4 \n"
               + "	                  AND ch.t_date BETWEEN @StartDate  \n"
               + "	                      AND  \n"
               + "	                      @EndDate \n"
               + "	                  AND ch.to_comp = '2' \n"
               + "	           GROUP BY \n"
               + "	                  ch.t_date, \n"
               + "	                  b.branchCode, \n"
               + "	                  cm.code_of, \n"
               + "	                  cm1.code_OF \n"
               + "	       )                                a \n"
               + "	 \n"
               + "	 \n"
               + "	---- TABLE 2 END	 \n"
               + "	 \n"
               + "	 \n"
               + "	SELECT A.branch, \n"
               + "	       A.BranchName, \n"
               + "	       A.company, \n"
               + "	       SUM(ISNULL(A.Debit, 0))      DEBIT, \n"
               + "	       SUM(ISNULL(A.Credit, 0))     CREDIT \n"
               + "	FROM   ( \n"
               + "	           SELECT cr.branch, \n"
               + "	                  b.name       BranchName, \n"
               + "	                  cr.company, \n"
               + "	                  --cr.[year], \n"
               + "	                  --cr.[month], \n"
               + "	                  bb.date, \n"
               + "	                  REPLACE( \n"
               + "	                      REPLACE(REPLACE(bb.cashtype, CHAR(9), ' '), CHAR(10), ' '), \n"
               + "	                      CHAR(13), \n"
               + "	                      ' ' \n"
               + "	                  )            DESCRIPTION, \n"
               + "	                  BB.dnote     Debit, \n"
               + "	                  BB.cnote     Credit, \n"
               + "	                  cr.OPENING_VALUE [Opening Balance], \n"
               + "	                  ISNULL( \n"
               + "	                      SUM(bb.dnote) OVER(PARTITION BY cr.branch, cr.company, cr.[year], cr.MONTH), \n"
               + "	                      0 \n"
               + "	                  ) - \n"
               + "	                  ISNULL( \n"
               + "	                      SUM(bb.cnote) OVER(PARTITION BY cr.branch, cr.company, cr.[year], cr.MONTH), \n"
               + "	                      0 \n"
               + "	                  ) + cr.OPENING_VALUE [Closing Balance] \n"
               + "	           FROM   CIH_remainings cr \n"
               + "	                  LEFT OUTER JOIN CLOSINGBALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " BB \n"
               + "	                       ON  cr.company = bb.company \n"
               + "	                       AND cr.branch = bb.branch \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = cr.branch \n"
               + "	                  INNER JOIN Zones z  \n"
               + "							ON z.zoneCode = b.zoneCode \n"
               + "	           WHERE  cr.company = '1' \n"
               + "	                  AND cr.[month] = CAST(@Month AS VARCHAR) \n"
               + "	                  AND cr.[year] = CAST(@Year AS VARCHAR) \n"
               + ZoneID + "\n"
               + BranchID + "\n"
               + "	           --GROUP BY \n"
               + "	           --       cr.company, \n"
               + "	           --       cr.branch, \n"
               + "	           --       cr.OPENING_VALUE, \n"
               + "	           --       cr.[VALUE], \n"
               + "	           --       cr.[year], \n"
               + "	           --       cr.[month] \n"
               + "	            \n"
               + "	            \n"
               + "	            \n"
               + "	           --       --Company 1 Ends here \n"
               + "	           UNION ALL \n"
               + "	            \n"
               + "	           --Company 2 Starts Here \n"
               + "	           SELECT cr.branch, \n"
               + "	                  b.name          BranchName, \n"
               + "	                  cr.company, \n"
               + "	                  --cr.[year], \n"
               + "	                  --cr.[month], \n"
               + "	                  bb.date, \n"
               + "	                  bb.cashtype     DESCRIPTION, \n"
               + "	                  ISNULL(BB.dnote, 0) Debit, \n"
               + "	                  ISNULL(BB.cnote, 0) Credit, \n"
               + "	                  cr.OPENING_vALUE [Opening Balance], \n"
               + "	                  cr.OPENING_VALUE + \n"
               + "	                  ISNULL( \n"
               + "	                      SUM(bb.dnote) OVER(PARTITION BY cr.branch, cr.company, cr.[year], cr.MONTH), \n"
               + "	                      0 \n"
               + "	                  ) - \n"
               + "	                  ISNULL( \n"
               + "	                      SUM(bb.cnote) OVER(PARTITION BY cr.branch, cr.company, cr.[year], cr.MONTH), \n"
               + "	                      0 \n"
               + "	                  ) [Closing Balance] \n"
               + "	           FROM   CIH_remainings cr \n"
               + "	                  LEFT OUTER JOIN CLOSINGBALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " BB \n"
               + "	                       ON  cr.company = bb.company \n"
               + "	                       AND cr.branch = bb.branch \n"
               + "	                  INNER JOIN Branches b \n"
               + "	                       ON  b.branchCode = cr.branch \n"
               + "	                  INNER JOIN Zones z  \n"
               + "							ON z.zoneCode = b.zoneCode \n"
               + "	           WHERE  cr.company = '2' \n"
               + "	                  AND cr.[month] = CAST(@Month AS VARCHAR) \n"
               + "	                  AND cr.[year] = CAST(@Year AS VARCHAR) \n"
               + ZoneID + "\n"
               + BranchID + "\n"
               + "	                      --GROUP BY \n"
               + "	                      --       cr.company, \n"
               + "	                      --       cr.branch, \n"
               + "	                      --       cr.OPENING_VALUE, \n"
               + "	                      --       cr.[VALUE], \n"
               + "	                      --       cr.[year], \n"
               + "	                      --       cr.[month] \n"
               + "	       )                            A \n"
               + "	GROUP BY \n"
               + "	       A.branch, \n"
               + "	       A.BranchName, \n"
               + "	       A.company \n"
               + "	ORDER BY \n"
               + "	       CAST(A.branch AS INT), \n"
               + "	       A.company \n"
               + "	 \n"
               + "	 \n"
               + "	DROP TABLE CLOSINGBALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " \n"
               + "	DROP TABLE CLOSINGBALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " \n";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3000;
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.CommandText = sqlString;

                SqlDataAdapter oda = new SqlDataAdapter();
                oda.SelectCommand = cmd;
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }

            return dt;

            /*
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandTimeout = 3000;
                cmd.CommandText = "GET_CASH_IN_HAND_LEDGER_FOR_DAY_QA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@UserId", HttpContext.Current.Session["U_ID"].ToString());

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;

                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
             */
        }

        public DataTable GetPettyCashLedgerForDay(string date)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "GET_PETTY_CASH_LEDGER_FOR_DAY";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", date);

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;


                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Get_RemoveTempTable(Cl_Variables clvar)
        {
            string sqlString = "DROP TABLE CLOSING_BALANCES_1_" + HttpContext.Current.Session["U_ID"].ToString() + " \n" +
            "DROP TABLE CLOSING_BALANCES_2_" + HttpContext.Current.Session["U_ID"].ToString() + " \n";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3000;
                cmd.CommandText = sqlString;

                SqlDataAdapter oda = new SqlDataAdapter();
                oda.SelectCommand = cmd;
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }

            return dt;
        }

        protected void rp_data_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                Label company1 = e.Item.FindControl("lbl_company1") as Label;
                Label company2 = e.Item.FindControl("lbl_company2") as Label;

                company1.Text = "M&P Express Logistics (Private) Limited";
                company2.Text = "M&P Logistics (Private) Limited";
            }

            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HyperLink branchname = e.Item.FindControl("hl_BranchName") as HyperLink;

                if (branchname.Text.ToString().Equals("HEAD OFFICE")
                    || branchname.Text.ToString().Equals("GAWADAR MAIN OFFICE"))
                {
                }
                else
                {
                    branchname.NavigateUrl = "Feeding_Status.aspx?date=" + hf_from.Value.ToString() + "&branchname=" + branchname.Text.ToString() + "";
                }
            }
        }
    }
}