using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class CIH_HBL_GLprint : System.Web.UI.Page
    {
        double op_balance = 0;
        Variable clvar = new Variable();
        bayer_Function bfunc = new bayer_Function();
        CommonFunction cfunc = new CommonFunction();
        clsVariables clsvar = new clsVariables();
        Cl_Variables cl_var = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["from"] != null)
                {
                    hf_branch.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["branch"]));
                    hf_from.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["from"]));
                    hf_to.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["to"]));
                    hf_company.Value = Decrypt(HttpUtility.UrlDecode(Request.QueryString["company"]));

                    lbl_from.Text = hf_from.Value.ToString();
                    lbl_to.Text = hf_to.Value.ToString();
                    if (hf_ec.Value.ToString() == "")
                    { }
                }
                OpeningBalance();
                Data();
            }
        }
        public void Data()
        {
            DataSet ds = new DataSet();
            ds = GetData();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr_comp = ds.Tables[0].Select("company <> ''");
                if (dr_comp.Count() > 0)
                {
                    lbl_company.Text = dr_comp[0]["company"].ToString();
                }


                lbl_branch.Text = Decrypt(HttpUtility.UrlDecode(Request.QueryString["bname"]));
                lbl_currentdate.Text = DateTime.Now.ToString();

                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                //debit credit balance
                dt.Columns.Add("balance", typeof(double));

                double debit = 0;
                double credit = 0;
                double balance = 0;
                double credit_total = 0;
                double debit_total = 0;

                if (op_balance != 0)
                {
                    debit = op_balance;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["dnote"].ToString()))
                    {
                        debit += double.Parse(dr["dnote"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dr["cnote"].ToString()))
                    {
                        credit += double.Parse(dr["cnote"].ToString());
                    }
                    if (credit == 0)
                    {
                        credit = 0;
                    }
                    balance = debit - credit;
                    dr["balance"] = double.Parse(balance.ToString());

                    lbl_closing_balnc.Text = double.Parse(dr["balance"].ToString()).ToString("N0");
                }
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["cnote"].ToString() != "")
                    {
                        credit_total += double.Parse(dr["cnote"].ToString());
                        dr["cnote"] = dr["cnote"].ToString();
                    }
                    if (dr["dnote"].ToString() != "")
                    {
                        debit_total += double.Parse(dr["dnote"].ToString());
                        dr["dnote"] = dr["dnote"].ToString();

                    }
                }
                lbl_user.Text = HttpContext.Current.Session["U_NAME"].ToString();

                DataRow total_row = dt.NewRow();
                total_row["dnote"] = debit_total;
                total_row["cnote"] = credit_total;
                dt.Rows.Add(total_row);
                dt.AcceptChanges();

                GridView.DataSource = dt;
                GridView.DataBind();
                if (hf_ec.Value.ToString() != "")
                {
                    lbl_closing_balnc.Text = "";
                    opening.InnerText = "";
                    closing.InnerText = "";
                    lbl_opening_balnc.Text = "";
                }
                GridView.Rows[GridView.Rows.Count - 1].Cells[0].Text = "Total";
                GridView.Rows[GridView.Rows.Count - 1].Cells[1].Visible = false;
                GridView.Rows[GridView.Rows.Count - 1].Cells[2].Visible = false;
                GridView.Rows[GridView.Rows.Count - 1].Cells[0].ColumnSpan = 3;
                GridView.Rows[GridView.Rows.Count - 1].Font.Bold = true;

            }
        }
        public void OpeningBalance()
        {
            DataSet ds = new DataSet();
            ds = GetOpeningBalance();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                {
                    lbl_opening_balnc.Text = double.Parse(ds.Tables[0].Rows[0][0].ToString()).ToString("N0");
                    op_balance = double.Parse(ds.Tables[0].Rows[0][0].ToString());
                }
                else
                {
                    lbl_opening_balnc.Text = "0";
                }

            }
        }
        public DataSet GetData()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                string test = DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                string branch = "";
                string ec = "";
                string sqlString = "";
                string sqlString_Updated = "";
                if (hf_company.Value == "1")
                {
                    sqlString =
                        "/************************************************************			\n "
+ " * Code formatted by SoftTree SQL Assistant © v11.0.35			\n "
+ " * '" + DateTime.Now.ToString() + "'			\n "
+ " ************************************************************/			\n "
+ " -- CASH IN HAND COMPANY 1			\n "
+ "			\n "
+ "SELECT DISTINCT a.rank			\n "
+ "      ,CONVERT(VARCHAR ,a.date ,103)     DATE			\n "
+ "      ,a.cashtype			\n "
+ "      ,a.debit                           dnote			\n "
+ "      ,a.credit                          cnote			\n "
+ "      ,a.branch			\n "
+ "      ,a.DepositSlipNo			\n "
+ "      ,company			\n "
+ "FROM   (			\n "
+ "           SELECT '1'                       RANK			\n "
+ "                 ,SUM(pv.Amount)- SUM(ISNULL(mm.productAmount ,0)) debit			\n "
+ "                 ,0                         credit			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN PV.RefNo IS     NULL			\n "
+ "           OR PV.RefNo='' THEN 'Cash In Hand ('+ISNULL(pt.name ,'')+')' ELSE 'Cash In Hand ('+ISNULL(pt.name ,'')+')'+			\n "
+ "              ' Ref No# '+PV.RefNo END cashtype			\n "
+ "          ,VoucherDate DATE			\n "
+ "          ,b.[name] branch			\n "
+ "          ,'' DepositSlipNo			\n "
+ "          ,'' company			\n "
+ "           FROM PaymentVouchers pv			\n "
+ "           LEFT OUTER JOIN PaymentTypes pt			\n "
+ "           ON pt.Id=pv.PaymentTypeId			\n "
+ "           LEFT OUTER JOIN (			\n "
+ "               SELECT mp.voucherID			\n "
+ "                     ,SUM(mp.amount) productAmount			\n "
+ "               FROM   MnP_PaymentVouchersProductBreakDown mp			\n "
+ "               WHERE  mp.Product IN ('JAzzcash' ,'Jazz Card')			\n "
+ "               GROUP BY			\n "
+ "                      mp.voucherID			\n "
+ "           ) mm			\n "
+ "           ON mm.voucherID=pv.Id			\n "
+ "           INNER JOIN Branches b			\n "
+ "           ON b.branchCode=pv.BranchCode			\n "
+ "           WHERE pv.BranchCode IN (" + hf_branch.Value.ToString() + ")			\n "
+ "           AND pv.VoucherDate BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'\n "
+ "           AND pv.CashPaymentSource IS NOT NULL			\n "
+ "           AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId='1')			\n "
+ "           AND pv.CashPaymentSource!='4'			\n "
+ "               GROUP BY			\n "
+ "               VoucherDate			\n "
+ "          ,PV.RefNo			\n "
+ "          ,pt.name			\n "
+ "          ,b.[name]			\n "
+ "           UNION ALL			\n "
+ "           SELECT '1'                       RANK			\n "
+ "                 ,SUM(pv.Amount)- SUM(ISNULL(mm.productAmount ,0)) debit			\n "
+ "                 ,0                         credit			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN PV.RefNo IS     NULL			\n "
+ "           OR PV.RefNo='' THEN 'Cash In Hand ('+ISNULL(pt.name ,'')+')' ELSE 'Cash In Hand ('+ISNULL(pt.name ,'')+')'+			\n "
+ "              ' Ref No# '+PV.RefNo END cashtype			\n "
+ "          ,VoucherDate DATE			\n "
+ "          ,b.[name] branch			\n "
+ "          ,'' DepositSlipNo			\n "
+ "          ,'' company			\n "
+ "           FROM PaymentVouchers pv			\n "
+ "           LEFT OUTER JOIN (			\n "
+ "               SELECT mp.voucherID			\n "
+ "                     ,SUM(mp.amount) productAmount			\n "
+ "               FROM   MnP_PaymentVouchersProductBreakDown mp			\n "
+ "               WHERE  mp.Product IN ('JAzzcash' ,'Jazz Card')			\n "
+ "               GROUP BY			\n "
+ "                      mp.voucherID			\n "
+ "           ) mm			\n "
+ "           ON mm.voucherID=pv.Id			\n "
+ "           LEFT OUTER JOIN PaymentTypes pt			\n "
+ "           ON pt.Id=pv.PaymentTypeId			\n "
+ "           INNER JOIN Branches b			\n "
+ "           ON b.branchCode=pv.BranchCode			\n "
+ "           WHERE pv.BranchCode IN (" + hf_branch.Value.ToString() + ")			\n "
+ "           AND pv.VoucherDate BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'	\n "
+ "           AND pv.CreditClientId IS NOT NULL			\n "
+ "           AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId='1') 			\n "
+ "               GROUP BY			\n "
+ "               VoucherDate			\n "
+ "          ,PV.RefNo			\n "
+ "          ,pt.name			\n "
+ "          ,b.[name]			\n "
+ "           UNION ALL			\n "
+ "           SELECT '1'                       RANK			\n "
+ "                 ,SUM(pv.Amount)- SUM(ISNULL(mm.productAmount ,0)) debit			\n "
+ "                 ,0                         credit			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN PV.RefNo IS     NULL			\n "
+ "           OR PV.RefNo='' THEN 'Cash In Hand ('+ISNULL(pt.name ,'')+')' ELSE 'Cash In Hand ('+ISNULL(pt.name ,'')+')'+			\n "
+ "              ' Ref No# '+PV.RefNo END cashtype			\n "
+ "          ,VoucherDate DATE			\n "
+ "          ,b.[name] branch			\n "
+ "          ,'' DepositSlipNo			\n "
+ "          ,'' company			\n "
+ "           FROM PaymentVouchers pv			\n "
+ "           LEFT OUTER JOIN (			\n "
+ "               SELECT mp.voucherID			\n "
+ "                     ,SUM(mp.amount) productAmount			\n "
+ "               FROM   MnP_PaymentVouchersProductBreakDown mp			\n "
+ "               WHERE  mp.Product IN ('JAzzcash' ,'Jazz Card')			\n "
+ "               GROUP BY			\n "
+ "                      mp.voucherID			\n "
+ "           ) mm			\n "
+ "           ON mm.voucherID=pv.Id			\n "
+ "           LEFT OUTER JOIN PaymentTypes pt			\n "
+ "           ON pt.Id=pv.PaymentTypeId			\n "
+ "           INNER JOIN Branches b			\n "
+ "           ON b.branchCode=pv.BranchCode			\n "
+ "           WHERE pv.BranchCode IN (" + hf_branch.Value.ToString() + ")			\n "
+ "           AND pv.VoucherDate BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'	\n "
+ "           AND pv.CreditClientId IS NULL			\n "
+ "           AND pv.ClientGroupId IS NOT NULL			\n "
+ "           AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId='1') 			\n "
+ "               GROUP BY			\n "
+ "               VoucherDate			\n "
+ "          ,PV.RefNo			\n "
+ "          ,pt.name			\n "
+ "          ,b.[name]			\n "
+ "           UNION ALL			\n "
+ "           SELECT '1'                     AS RANK			\n "
+ "                 ,SUM(k.CollectedAmount)  AS Debit			\n "
+ "                 ,0                       AS credit			\n "
+ "                 ,'Cash in Hand (HBL Konnect)' AS cashtype			\n "
+ "                 ,CAST(k.CreatedOn AS DATE) AS DATE			\n "
+ "                 ,b.name                  AS branch			\n "
+ "                 ,''                      AS DepositeSlipNo			\n "
+ "                 ,''                      AS Company			\n "
+ "           FROM   tbl_RiderHBLKonnectPayment k			\n "
+ "                  INNER JOIN riders r			\n "
+ "                       ON  r.riderCode = k.RiderCode			\n "
+ "                  INNER JOIN Branches b			\n "
+ "                       ON  b.branchCode = r.branchId			\n "
+ "                           AND b.status = 1			\n "
+ "           WHERE  r.branchId IN (" + hf_branch.Value.ToString() + ")			\n "
+ "                  AND CAST(k.CreatedOn AS DATE) BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'	\n "
+ "           GROUP BY			\n "
+ "                  k.CollectedAmount			\n "
+ "                 ,k.CreatedOn			\n "
+ "                 ,b.name			\n "
+ "           UNION ALL			\n "
+ "           -------------------------------------------------------------------------- MSA Start			\n "
+ "           --          SELECT '2' RANK,			\n "
+ "           --                 CASE			\n "
+ "           --                      WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)			\n "
+ "           --                      ELSE 0			\n "
+ "           --                 END               debit,			\n "
+ "           --                 CASE			\n "
+ "           --                      WHEN SUM(pv.Amount) < 0 THEN 0			\n "
+ "           --                      ELSE SUM(pv.Amount)			\n "
+ "           --                 END               credit,			\n "
+ "           --                 CASE WHEN (SUM(pv.Amount) < 0 OR pv.ldesc is not null)  THEN pv.LDESC ELSE			\n "
+ "           --                 'Deposit To Bank: ' + isnull(bo.bank_name,'')			\n "
+ "           --                  END cashtype,			\n "
+ "           --                 pv.ChequeDate     date,			\n "
+ "           --                 b.[name]          branch, isnull( pv.DepositSlipNo,'')   DepositSlipNo,			\n "
+ "           --                 '' company			\n "
+ "           --          FROM   deposit_to_bank pv			\n "
+ "           --                 INNER JOIN Branches b			\n "
+ "           --                      ON  b.branchCode = pv.BranchCode			\n "
+ "           --                 LEFT OUTER JOIN banks_of bo			\n "
+ "           --                      ON  bo.bank_code = pv.DepositSlipBankID			\n "
+ "           --WHERE pv.BranchCode  IN (" + hf_branch.Value.ToString() + ")		\n "
+ "           --and pv.company='1'			\n "
+ "           --  and pv.ChequeDate between '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'\n "
+ "           --          GROUP BY			\n "
+ "           --                 ChequeDate,			\n "
+ "           --                 b.[name],			\n "
+ "           --                 pv.Id,			\n "
+ "           --                 bo.bank_name,			\n "
+ "           --                 pv.DepositSlipNo, pv.LDESC			\n "
+ "           			\n "
+ "           --UNION ALL			\n "
+ "           SELECT '2'                AS RANK			\n "
+ "                 ,0                  AS Debit			\n "
+ "                 ,k.CollectedAmount  AS credit			\n "
+ "                 ,CONCAT('HBL Konnect - ' ,k.RiderCode) AS cashtype			\n "
+ "                 ,CAST(k.CreatedOn AS DATE) AS DATE			\n "
+ "                 ,b.name             AS branch			\n "
+ "                 ,k.TransactionID    AS DepositeSlipNo			\n "
+ "                 ,''                 AS Company			\n "
+ "           FROM   tbl_RiderHBLKonnectPayment k			\n "
+ "                  INNER JOIN riders r			\n "
+ "                       ON  r.riderCode = k.RiderCode			\n "
+ "                  INNER JOIN Branches b			\n "
+ "                       ON  b.branchCode = r.branchId			\n "
+ "                           AND b.status = 1			\n "
+ "           WHERE  r.branchId IN (" + hf_branch.Value.ToString() + ")		\n "
+ "                  AND CAST(k.CreatedOn AS DATE) BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'	\n "
+ "           			\n "
+ "           UNION ALL			\n "
+ "           -------------------------------------------------------------------------- MSA End			\n "
+ "           SELECT '2'                       RANK			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN SUM(pv.AMOUNT)<0 THEN (SUM(pv.AMOUNT)*-1)			\n "
+ "                       ELSE 0			\n "
+ "                  END                       debit			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN SUM(pv.Amount)<0 THEN 0			\n "
+ "                       ELSE SUM(pv.AMOUNT)			\n "
+ "                  END                       credit			\n "
+ "                 ,'Transferred To Petty Cash (From : '+pc.[desc]+')'			\n "
+ "                  cashtype			\n "
+ "                 ,pv.[date]                 DATE			\n "
+ "                 ,b.[name]                  branch			\n "
+ "                 ,''                        DepositSlipNo			\n "
+ "                 ,cm.sdesc_OF               company			\n "
+ "           FROM   PC_CIH_detail pv			\n "
+ "                  INNER JOIN PC_CIH_head ph			\n "
+ "                       ON  ph.ID = pv.head_id			\n "
+ "                  INNER JOIN Branches b			\n "
+ "                       ON  b.branchCode = ph.BRANCH			\n "
+ "                  INNER JOIN PC_cash_mode pc			\n "
+ "                       ON  pc.ID = pv.cash_type			\n "
+ "                           AND pv.cash_type = '1'			\n "
+ "                           AND pc.ID = '1'			\n "
+ "                  INNER JOIN COMPANY_OF cm			\n "
+ "                       ON  cm.code_OF = ph.COMPANY			\n "
+ "                           AND ph.COMPANY = '1'			\n "
+ "           WHERE  ph.BRANCH  IN (" + hf_branch.Value.ToString() + ")	\n "
+ "                  AND pv.[date] BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'	\n "
+ "           GROUP BY			\n "
+ "                  [date]			\n "
+ "                 ,b.[name]			\n "
+ "                 ,pc.[desc]			\n "
+ "                 ,cm.sdesc_OF			\n "
+ "           UNION ALL			\n "
+ "           --From Company			\n "
+ "           SELECT '2'              RANK			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN SUM(ch.AMOUNT)<0 THEN (SUM(ch.AMOUNT)*-1)			\n "
+ "                       ELSE 0			\n "
+ "                  END              debit			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN SUM(ch.Amount)<0 THEN 0			\n "
+ "                       ELSE SUM(ch.AMOUNT)			\n "
+ "                  END              credit			\n "
+ "                 ,'Transfered To  '+ISNULL(cm1.sdesc_OF ,'')+' Remarks: '+ch.remarks cashtype			\n "
+ "                 ,ch.t_date        DATE			\n "
+ "                 ,b.[name]         branch			\n "
+ "                 ,''               DepositSlipNo			\n "
+ "                 ,cm1.sdesc_OF     company			\n "
+ "           FROM   cih_transfer ch			\n "
+ "                  INNER JOIN Branches b			\n "
+ "                       ON  b.branchCode = ch.BRANCH			\n "
+ "                  INNER JOIN COMPANY_OF cm			\n "
+ "                       ON  cm.code_OF = ch.from_comp			\n "
+ "                  INNER JOIN COMPANY_OF cm1			\n "
+ "                       ON  cm1.code_OF = ch.to_comp			\n "
+ "           WHERE  ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")	\n "
+ "                  AND ch.t_date BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'			\n "
+ "                  AND ch.from_comp = '1'			\n "
+ "           GROUP BY			\n "
+ "                  ch.t_date			\n "
+ "                 ,b.[name]			\n "
+ "                 ,cm1.sdesc_OF			\n "
+ "                 ,ch.remarks 			\n "
+ "           			\n "
+ "           UNION ALL			\n "
+ "           --To Company			\n "
+ "           SELECT '1'             RANK			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN SUM(ch.amount)<0 THEN 0			\n "
+ "                       ELSE SUM(ch.amount)			\n "
+ "                  END             debit			\n "
+ "                 ,CASE 			\n "
+ "                       WHEN SUM(ch.amount)<0 THEN (SUM(ch.amount)*-1)			\n "
+ "                  END             credit			\n "
+ "                 ,'Received From  '+ISNULL(cm.sdesc_OF ,'')+' Remarks: '+ch.remarks cashtype			\n "
+ "                 ,ch.t_date       DATE			\n "
+ "                 ,b.[name]        branch			\n "
+ "                 ,''              DepositSlipNo			\n "
+ "                 ,cm.sdesc_OF     company			\n "
+ "           FROM   cih_transfer ch			\n "
+ "                  INNER JOIN Branches b			\n "
+ "                       ON  b.branchCode = ch.BRANCH			\n "
+ "                  LEFT OUTER  JOIN COMPANY_OF cm			\n "
+ "                       ON  cm.code_OF = ch.from_comp			\n "
+ " INNER JOIN COMPANY_OF cm1	\n "
+ "                       ON  cm1.code_OF = ch.to_comp	\n "
+ "           WHERE  ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")	\n "
+ "                  AND ch.t_date BETWEEN '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'	\n "
+ "                  AND ch.to_comp = '1'	\n "
+ "           GROUP BY	\n "
+ " ch.t_date	\n "
+ " ,b.[name] \n "
+ " ,cm.sdesc_OF	\n "
+ " ,ch.remarks	\n "
+ ") a	\n "
+ "ORDER BY	\n "
+ "       2	\n "
+ "      ,a.branch	\n "
+ "      ,1	";
                }
                else
                {
                    sqlString = "/************************************************************\n" +
                    " * Code formatted by SoftTree SQL Assistant © v6.3.153\n" +
                    " * Time: 7/24/2018 12:34:05 PM\n" +
                    " * CASH IN HAND COMPANY 2\n" +
                    " ************************************************************/\n" +
                    "\n" +
                    "SELECT a.rank,\n" +
                    "       CONVERT(VARCHAR, a.date, 103)     date,\n" +
                    "       a.cashtype,\n" +
                    "       a.debit                           dnote,\n" +
                    "       a.credit                          cnote,\n" +
                    "       a.branch,a.DepositSlipNo,\n" +
                    "       company\n" +
                    "FROM   (\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END               debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(pv.Amount)\n" +
                    "                  END               credit,\n" +
                    "                  'Deposit To Bank: ' + isnull(bo.bank_name,'')  \n" +
                    "                  cashtype,\n" +
                    "                  pv.ChequeDate     date,\n" +
                    "                  b.[name]          branch,  isnull( pv.DepositSlipNo,'')   DepositSlipNo,\n" +
                    "                  '' company\n" +
                    "           FROM   deposit_to_bank pv\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = pv.BranchCode\n" +
                    "                  INNER JOIN banks_of bo\n" +
                    "                       ON  bo.bank_code = pv.DepositSlipBankID\n" +
                    " WHERE pv.BranchCode  IN (" + hf_branch.Value.ToString() + ")\n" +
                    " and pv.company='" + hf_company.Value.ToString() + "'\n" +
                    " and pv.ChequeDate between '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'\n" +
                    "           GROUP BY\n" +
                    "                  ChequeDate,\n" +
                    "                  b.[name],\n" +
                    "                  pv.Id,\n" +
                    "                  bo.bank_name,\n" +
                    "                  pv.DepositSlipNo\n" +
                    "\n" +
                    "           UNION ALL\n" +
                    "\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.AMOUNT) < 0 THEN (SUM(pv.AMOUNT) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END                      debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(pv.AMOUNT)\n" +
                    "                  END                      credit,\n" +
                    "                  'Transferred To Petty Cash (From : ' + pc.[desc] + ')'\n" +
                    "                  cashtype,\n" +
                    "                  pv.[date]                date,\n" +
                    "                  b.[name]                 branch, ''  DepositSlipNo,\n" +
                    "                  cm.sdesc_OF              company\n" +
                    "           FROM   PC_CIH_detail pv\n" +
                    "                  INNER JOIN PC_CIH_head ph\n" +
                    "                       ON  ph.ID = pv.head_id\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ph.BRANCH\n" +
                    "                  INNER JOIN PC_cash_mode pc\n" +
                    "                       ON  pc.ID = pv.cash_type\n" +
                    "                       AND pv.cash_type = '1'\n" +
                    "                       AND pc.ID = '1'\n" +
                    "                  INNER JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ph.COMPANY\n" +
                    "           and ph.COMPANY = '" + hf_company.Value.ToString() + "'\n" +
                    "         WHERE ph.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                    "           and pv.[date]between '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' " +

                    "           GROUP BY\n" +
                    "                  [date],\n" +
                    "                  b.[name],\n" +
                    "                  pc.[desc],\n" +
                    "                  cm.sdesc_OF\n" +
                    "           UNION ALL\n" +
                    "           --From Company\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.AMOUNT) < 0 THEN (SUM(ch.AMOUNT) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END              debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(ch.AMOUNT)\n" +
                    "                  END              credit,\n" +
                    "                  'Transfered To  ' + isnull(cm1.sdesc_OF,'') + ' Remarks: '+ch.remarks cashtype,\n" +
                    "                  ch.t_date        date,\n" +
                    "                  b.[name]         branch,\n" +
                    "                  cm1.sdesc_OF     company\n" +
                    "           FROM   cih_transfer ch\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ch.BRANCH\n" +
                    "                  INNER JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ch.from_comp\n" +
                    "                  INNER JOIN COMPANY_OF cm1\n" +
                    "                       ON  cm1.code_OF = ch.to_comp\n" +
                    "         WHERE ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                    "           and ch.t_date between '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'\n" +
                    "           and ch.from_comp = '" + hf_company.Value.ToString() + "'\n" +
                    "           GROUP BY\n" +
                    "                  ch.t_date,\n" +
                    "                  b.[name],\n" +
                    "                  cm1.sdesc_OF,ch.remarks \n" +
                    "\n" +
                    "           UNION ALL\n" +
                    "           --To Company\n" +
                    "           SELECT '1' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(ch.amount)\n" +
                    "                  END             debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END             credit,\n" +
                    "                  'Received From  ' + isnull(cm.sdesc_OF,'') + ' Remarks: '+ch.remarks  cashtype,\n" +
                    "                  ch.t_date       date,\n" +
                    "                  b.[name]        branch, ''  DepositSlipNo,\n" +
                    "                  cm.sdesc_OF     company\n" +
                    "           FROM   cih_transfer ch\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ch.BRANCH\n" +
                    "                   left outer  JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ch.from_comp\n" +
                    "                  INNER JOIN COMPANY_OF cm1\n" +
                    "                       ON  cm1.code_OF = ch.to_comp\n" +
                    "         WHERE ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                  "           and ch.t_date between '" + DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' and '" + DateTime.ParseExact(hf_to.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'\n" +
                  "           and ch.to_comp = '" + hf_company.Value.ToString() + "'\n" +
                    "           GROUP BY\n" +
                    "                  ch.t_date,\n" +
                    "                  b.[name],\n" +
                    "                  cm.sdesc_OF,ch.remarks  \n" +
                    "       )                                 a\n" +
                    "ORDER BY\n" +
                    "       2,a.branch, 1";

                }
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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

        public DataSet GetOpeningBalance()
        {
            DataSet Ds_1 = new DataSet();
            try
            {
                var dt = DateTime.ParseExact(hf_from.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime startOfMonth = new DateTime(dt.Year, dt.Month, 1);   //new DateTime(year, month, 1);
                int flag_sdate = 0;
                if (dt == startOfMonth)
                {
                    flag_sdate = 1;
                }

                //DateTime endOfLastMonth = new DateTime(dt.AddMonths(-1).Year, dt.AddMonths(-1).Month, DateTime.DaysInMonth(dt.AddMonths(-1).Year, dt.AddMonths(-1).Month)); //new DateTime(year, month, DateTime.DaysInMonth(year, month));

                var lastmonth = dt.AddMonths(-1);
                var endOfLastMonth = new DateTime(lastmonth.Year, lastmonth.Month, DateTime.DaysInMonth(lastmonth.Year, lastmonth.Month));
                string sqlString = "";
                if (hf_company.Value.ToString() == "1")
                {
                    sqlString = "select sum(b.amount) + SUM(b.dnote) - SUM(b.cnote) amount\n" +
                    "  from (SELECT 0 dnote, 0 cnote, SUM(OPENING_VALUE) amount\n" +
                    "          FROM CIH_remainings pv\n" +
                    "         WHERE pv.company = '" + hf_company.Value.ToString() + "'\n" +
                    "           and pv.branch  IN (" + hf_branch.Value.ToString() + ")\n" +
                    "           and pv.month = '" + dt.Month + "'\n" +
                    "           and pv.year = '" + dt.Year + "'\n" +
                    "\n" +
                    "\n";
                    if (flag_sdate != 1)
                    {
                        sqlString +=
                    "        union (\n" +
                    "SELECT a.debit                           dnote,\n" +
                    "       a.credit                          cnote,\n" +
                    "       0 amount\n" +
                    "\t   FROM   (\n" +
                    "           SELECT '1' RANK,\n" +
                    "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit,\n" +
                    "                  0               credit,\n" +
                    "                  'Cash In Hand (' + ISNULL(pt.name, '') + ')' cashtype,\n" +
                    "                  VoucherDate     date,\n" +
                    "                  b.[name]        branch,\n" +
                    "                  '' company\n" +
                    "           FROM   PaymentVouchers pv\n" +
                    "                  LEFT OUTER JOIN PaymentTypes pt\n" +
                    "                       ON  pt.Id = pv.PaymentTypeId\n" +
                    "                  LEFT OUTER JOIN (\n" +
                    "                           SELECT mp.voucherID,\n" +
                    "                                  SUM(mp.amount) productAmount\n" +
                    "                           FROM   MnP_PaymentVouchersProductBreakDown mp\n" +
                    "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
                    "                           GROUP BY\n" +
                    "                                  mp.voucherID\n" +
                    "                       ) mm\n" +
                    "                       ON  mm.voucherID = pv.Id\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = pv.BranchCode\n" +
                    "           WHERE  pv.BranchCode IN (" + hf_branch.Value.ToString() + ")\n" +
                    "                  AND pv.VoucherDate BETWEEN CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                       " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102)  \n" +
                    "                  AND pv.CashPaymentSource IS NOT NULL\n" +
                    "                  AND (pv.PaymentSourceId IS NULL OR pv.PaymentSourceId = '1')\n" +
                    "                  AND pv.CashPaymentSource != '4'\n" +
                    "           GROUP BY\n" +
                    "                  VoucherDate,\n" +
                    "                  pt.name,\n" +
                    "                  b.[name]\n" +
                    "\n" +
                    "\n" +
                    "           UNION ALL\n" +
                    "\n" +
                    "\n" +
                    "           SELECT '1' RANK,\n" +
                    "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit,\n" +
                    "                  0               credit,\n" +
                    "                  'Cash In Hand (' + ISNULL(pt.name, '') + ')' cashtype,\n" +
                    "                  VoucherDate     date,\n" +
                    "                  b.[name]        branch,\n" +
                    "                  '' company\n" +
                    "           FROM   PaymentVouchers pv\n" +
                    "                  LEFT OUTER JOIN (\n" +
                    "                           SELECT mp.voucherID,\n" +
                    "                                  SUM(mp.amount) productAmount\n" +
                    "                           FROM   MnP_PaymentVouchersProductBreakDown mp\n" +
                    "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
                    "                           GROUP BY\n" +
                    "                                  mp.voucherID\n" +
                    "                       ) mm\n" +
                    "                       ON  mm.voucherID = pv.Id\n" +
                    "                  LEFT OUTER JOIN PaymentTypes pt\n" +
                    "                       ON  pt.Id = pv.PaymentTypeId\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = pv.BranchCode\n" +
                    "           WHERE  pv.BranchCode IN (" + hf_branch.Value.ToString() + ")\n" +
                    "                  AND pv.VoucherDate BETWEEN CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                       " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102)  \n" +
                    "                  AND pv.CreditClientId IS NOT NULL\n" +
                    "                  AND pv.paymentsourceid = '1'\n" +
                    "           GROUP BY\n" +
                    "                  VoucherDate,\n" +
                    "                  pt.name,\n" +
                    "                  b.[name]\n" +
                    "           UNION ALL\n" +
                    "\n" +
                    "\n" +
                    "           SELECT '1' RANK,\n" +
                    "                  SUM(pv.Amount) - SUM(ISNULL(mm.productAmount, 0)) debit,\n" +
                    "                  0               credit,\n" +
                    "                  'Cash In Hand (' + ISNULL(pt.name, '') + ')' cashtype,\n" +
                    "                  VoucherDate     date,\n" +
                    "                  b.[name]        branch,\n" +
                    "                  '' company\n" +
                    "           FROM   PaymentVouchers pv\n" +
                    "                  LEFT OUTER JOIN (\n" +
                    "                           SELECT mp.voucherID,\n" +
                    "                                  SUM(mp.amount) productAmount\n" +
                    "                           FROM   MnP_PaymentVouchersProductBreakDown mp\n" +
                    "                           WHERE  mp.Product IN ('JAzzcash', 'Jazz Card')\n" +
                    "                           GROUP BY\n" +
                    "                                  mp.voucherID\n" +
                    "                       ) mm\n" +
                    "                       ON  mm.voucherID = pv.Id\n" +
                    "                  LEFT OUTER JOIN PaymentTypes pt\n" +
                    "                       ON  pt.Id = pv.PaymentTypeId\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = pv.BranchCode\n" +
                    "           WHERE  pv.BranchCode IN (" + hf_branch.Value.ToString() + ")\n" +
                    "                  AND pv.VoucherDate BETWEEN CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                       " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102)  \n" +
                    "                  AND pv.CreditClientId IS NULL\n" +
                    "                  AND pv.ClientGroupId IS NOT NULL\n" +
                    "                  AND pv.paymentsourceid = '1'\n" +
                    "           GROUP BY\n" +
                    "                  VoucherDate,\n" +
                    "                  pt.name,\n" +
                    "                  b.[name]\n" +
                    "           UNION ALL\n" +
                    "\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END               debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(pv.Amount)\n" +
                    "                  END               credit,\n" +
                    "                  'Deposit To Bank: ' + isnull(bo.bank_name,'') \n" +
                    "                   cashtype,\n" +
                    "                  pv.ChequeDate     date,\n" +
                    "                  b.[name]        branch,\n" +
                    "                  '' company\n" +
                    "           FROM   deposit_to_bank pv\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = pv.BranchCode\n" +
                    "                  LEFT OUTER JOIN banks_of bo\n" +
                    "                       ON  bo.bank_code = pv.DepositSlipBankID\n" +
                    "           WHERE  pv.BranchCode  IN (" + hf_branch.Value.ToString() + ")\n" +
                    "                  AND pv.company = '" + hf_company.Value.ToString() + "'\n" +
                    "                  AND pv.ChequeDate BETWEEN CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                       " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102)  \n" +
                    "           GROUP BY\n" +
                    "                  ChequeDate,\n" +
                    "                  b.[name],\n" +
                    "                  pv.Id,\n" +
                    "                  bo.bank_name \n" +
                    "                  \n" +
                    "\n" +
                    "\n" +
                    "           UNION ALL\n" +
                    "\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.AMOUNT) < 0 THEN (SUM(pv.AMOUNT) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END                       debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(pv.AMOUNT)\n" +
                    "                  END                       credit,\n" +
                    "                  'Transferred To Petty Cash (From : ' + pc.[desc] + ')'\n" +
                    "                  cashtype,\n" +
                    "                  pv.[date]                 date,\n" +
                    "                  b.[name]                  branch,\n" +
                    "                  cm.sdesc_OF               company\n" +
                    "           FROM   PC_CIH_detail pv\n" +
                    "                  INNER JOIN PC_CIH_head ph\n" +
                    "                       ON  ph.ID = pv.head_id\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ph.BRANCH\n" +
                    "                  INNER JOIN PC_cash_mode pc\n" +
                    "                       ON  pc.ID = pv.cash_type\n" +
                    "                       AND pv.cash_type = '1'\n" +
                    "                       AND pc.ID = '1'\n" +
                    "                  INNER JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ph.COMPANY\n" +
                    "                       AND ph.COMPANY = '1'\n" +
                    "           WHERE  ph.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                    "                  AND pv.[date] BETWEEN   CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                       " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102)  \n" +
                    "           GROUP BY\n" +
                    "                  [date],\n" +
                    "                  b.[name],\n" +
                    "                  pc.[desc],\n" +
                    "                  cm.sdesc_OF\n" +
                    "           UNION ALL\n" +
                    "           --From Company\n" +
                    "           SELECT '2' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.AMOUNT) < 0 THEN (SUM(ch.AMOUNT) * -1)\n" +
                    "                       ELSE 0\n" +
                    "                  END              debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(ch.AMOUNT)\n" +
                    "                  END              credit,\n" +
                    "                  'Transfered To  ' + cm1.sdesc_OF cashtype,\n" +
                    "                  ch.t_date        date,\n" +
                    "                  b.[name]         branch,\n" +
                    "                  cm1.sdesc_OF     company\n" +
                    "           FROM   cih_transfer ch\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ch.BRANCH\n" +
                    "                  INNER JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ch.from_comp\n" +
                    "                  INNER JOIN COMPANY_OF cm1\n" +
                    "                       ON  cm1.code_OF = ch.to_comp\n" +
                    "           WHERE  ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                    "                  AND ch.t_date BETWEEN CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                       " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102)  \n" +
                    "                  AND ch.from_comp = '" + hf_company.Value.ToString() + "'\n" +
                    "           GROUP BY\n" +
                    "                  ch.t_date,\n" +
                    "                  b.[name],\n" +
                    "                  cm1.sdesc_OF\n" +
                    "\n" +
                    "           UNION ALL\n" +
                    "           --To Company\n" +
                    "           SELECT '1' RANK,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
                    "                       ELSE SUM(ch.amount)\n" +
                    "                  END             debit,\n" +
                    "                  CASE\n" +
                    "                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
                    "                  END             credit,\n" +
                    "                  'Received From  ' + cm.sdesc_OF cashtype,\n" +
                    "                  ch.t_date       date,\n" +
                    "                  b.[name]        branch,\n" +
                    "                  cm.sdesc_OF     company\n" +
                    "           FROM   cih_transfer ch\n" +
                    "                  INNER JOIN Branches b\n" +
                    "                       ON  b.branchCode = ch.BRANCH\n" +
                    "                   left outer  JOIN COMPANY_OF cm\n" +
                    "                       ON  cm.code_OF = ch.from_comp\n" +
                    "                  INNER JOIN COMPANY_OF cm1\n" +
                    "                       ON  cm1.code_OF = ch.to_comp\n" +
                    "           WHERE  ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                    "                  AND ch.t_date BETWEEN CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                       " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102)  \n" +
                    "                  AND ch.to_comp = '" + hf_company.Value.ToString() + "'\n" +
                    "           GROUP BY\n" +
                    "                  ch.t_date,\n" +
                    "                  b.[name],\n" +
                    "                  cm.sdesc_OF\n" +
                    "       )                                 a )  ";
                    }
                    sqlString +=
                    ") b";


                }
                else
                {

                    sqlString = "select sum(b.amount) + SUM(b.dnote) - SUM(b.cnote) amount\n" +
                      "  from (SELECT 0 dnote, 0 cnote, SUM(OPENING_VALUE) amount\n" +
                      "          FROM CIH_remainings pv\n" +
                      "         WHERE pv.company = '" + hf_company.Value.ToString() + "'\n" +
                      "           and pv.branch  IN (" + hf_branch.Value.ToString() + ")\n" +
                      "           and pv.month = '" + dt.ToString("MM").TrimStart('0') + "'\n" +
                      "           and pv.year = '" + dt.ToString("yyyy").TrimStart('0') + "'\n" +
                      "\n";
                    if (flag_sdate != 1)
                    {
                        sqlString += "        union (SELECT sum(a.debit) dnote, sum(a.credit) cnote, 0\n" +
                          "                FROM ( " +
                          "\n" +

                          "	SELECT '1' rank,\n" +
                          "                  CASE\n" +
                          "                       WHEN SUM(Amount) < 0 THEN (SUM(Amount) * -1)\n" +
                          "                       ELSE 0\n" +
                          "                  END               debit,\n" +
                          "                  CASE\n" +
                          "                       WHEN SUM(Amount) < 0 THEN 0\n" +
                          "                       ELSE SUM(Amount)\n" +
                          "                  END               credit,\n" +
                          "           'Cash In Hand' cashtype\n" +
                          "      FROM cih_transfer pv\n" +
                          "     WHERE pv.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                          "   and pv.to_comp='" + hf_company.Value.ToString() + "'\n" +
                          "       and pv.t_date between '" + startOfMonth.ToString("yyyy-MM-dd") + "' " +
                          " and  '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' \n" +
                          "		 union all\n" +

                          "                      SELECT '2' rank,\n" +
                          "                  CASE\n" +
                          "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
                          "                       ELSE 0\n" +
                          "                  END               debit,\n" +
                          "                  CASE\n" +
                          "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                          "                       ELSE SUM(pv.Amount)\n" +
                          "                  END               credit,\n" +
                          "                             'Deposit To Bank' cashtype\n" +
                          "                        FROM deposit_to_bank pv\n" +
                          "                       inner join Branches b\n" +
                          "                          on b.branchCode = pv.BranchCode\n" +
                          "                       WHERE pv.BranchCode  IN (" + hf_branch.Value.ToString() + ")\n" +
                          "                       and pv.COMPANY = '" + hf_company.Value.ToString() + "'  and pv.ChequeDate between  '" + startOfMonth.ToString("yyyy-MM-dd") + "' " +
                           " and  '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' \n" +
                          "                      union all\n" +
                          "\n" +
                          "                      SELECT '2' rank,\n" +
                          "                  CASE\n" +
                          "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
                          "                       ELSE 0\n" +
                          "                  END               debit,\n" +
                          "                  CASE\n" +
                          "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                          "                       ELSE SUM(pv.Amount)\n" +
                          "                  END               credit,\n" +
                          "                             'Transferred To Petty Cash' cashtype\n" +
                          "                        FROM PC_CIH_detail pv\n" +
                          "                       inner join PC_CIH_head ph\n" +
                          "                          on ph.ID = pv.head_id\n" +
                          "                       inner join Branches b\n" +
                          "                          on b.branchCode = ph.BRANCH\n" +
                          "                       inner join PC_cash_mode pc\n" +
                          "                          on pc.ID = pv.cash_type\n" +
                          "                         and pv.cash_type = '1'\n" +
                          "                         and pc.ID = '1'\n" +
                          "                       inner join COMPANY_OF cm\n" +
                          "                          on cm.code_OF = ph.COMPANY\n" +
                          "                         and ph.COMPANY = '" + hf_company.Value.ToString() + "'\n" +
                          "                       WHERE ph.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                          "                         and pv.[date]\n" +
                          "                        between  CONVERT(DATE,'" + startOfMonth.ToString("yyyy-MM-dd") + "', 102)  " +
                          " and CONVERT(DATE, '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "', 102) " +
                          "    ) a) ";
                    }
                    sqlString += " ) b";



                    sqlString = "/************************************************************\n" +
                   " * Code formatted by SoftTree SQL Assistant © v6.3.153\n" +
                   " * Time: 7/24/2018 12:34:05 PM\n" +
                   " * CASH IN HAND COMPANY 2\n" +
                   " ************************************************************/\n" +
                   " select sum(b.amount) + SUM(b.dnote) - SUM(b.cnote) amount\n" +
                   "  from (SELECT 0 dnote, 0 cnote, SUM(OPENING_VALUE) amount\n" +
                   "          FROM CIH_remainings pv\n" +
                    "         WHERE pv.company = '" + hf_company.Value.ToString() + "'\n" +
                     "           and pv.branch  IN (" + hf_branch.Value.ToString() + ")\n" +
                     "           and pv.month = '" + dt.ToString("MM").TrimStart('0') + "'\n" +
                     "           and pv.year = '" + dt.ToString("yyyy").TrimStart('0') + "'\n" +

                     "\n";
                    if (flag_sdate != 1)
                    {
                        sqlString +=
                   "        union (\n" +
                   "SELECT a.debit                           dnote,\n" +
                   "       a.credit                          cnote,\n" +
                   "       0 amount\n" +
                   "FROM   (\n" +
                   "           SELECT '2' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.Amount) < 0 THEN (SUM(pv.Amount) * -1)\n" +
                   "                       ELSE 0\n" +
                   "                  END               debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(pv.Amount)\n" +
                   "                  END               credit,\n" +
                   "                  'Deposit To Bank: ' + isnull(bo.bank_name,'') \n" +
                   "                  cashtype,\n" +
                   "                  pv.ChequeDate     date,\n" +
                   "                  b.[name]          branch,\n" +
                   "                  '' company\n" +
                   "           FROM   deposit_to_bank pv\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = pv.BranchCode\n" +
                   "                  INNER JOIN banks_of bo\n" +
                   "                       ON  bo.bank_code = pv.DepositSlipBankID\n" +
                   "           WHERE  pv.BranchCode  IN (" + hf_branch.Value.ToString() + ")\n" +
                   "                  AND pv.company = '" + hf_company.Value.ToString() + "'\n" +
                   "                  AND pv.ChequeDate between '" + startOfMonth.ToString("yyyy-MM-dd") + "' " +
                         " and  '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' \n" +
                   "           GROUP BY\n" +
                   "                  ChequeDate,\n" +
                   "                  b.[name],\n" +
                   "                  pv.Id,\n" +
                   "                  bo.bank_name,\n" +
                   "                \n" +
                   "\n" +
                   "           UNION ALL\n" +
                   "\n" +
                   "           SELECT '2' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.AMOUNT) < 0 THEN (SUM(pv.AMOUNT) * -1)\n" +
                   "                       ELSE 0\n" +
                   "                  END                      debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(pv.Amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(pv.AMOUNT)\n" +
                   "                  END                      credit,\n" +
                   "                  'Transferred To Petty Cash (From : ' + pc.[desc] + ')'\n" +
                   "                  cashtype,\n" +
                   "                  pv.[date]                date,\n" +
                   "                  b.[name]                 branch,\n" +
                   "                  cm.sdesc_OF              company\n" +
                   "           FROM   PC_CIH_detail pv\n" +
                   "                  INNER JOIN PC_CIH_head ph\n" +
                   "                       ON  ph.ID = pv.head_id\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = ph.BRANCH\n" +
                   "                  INNER JOIN PC_cash_mode pc\n" +
                   "                       ON  pc.ID = pv.cash_type\n" +
                   "                       AND pv.cash_type = '1'\n" +
                   "                       AND pc.ID = '1'\n" +
                   "                  INNER JOIN COMPANY_OF cm\n" +
                   "                       ON  cm.code_OF = ph.COMPANY\n" +
                   "                       AND ph.COMPANY = '" + hf_company.Value.ToString() + "'\n" +
                   "           WHERE  ph.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                   "                  AND pv.[date] between '" + startOfMonth.ToString("yyyy-MM-dd") + "' " +
                         " and  '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' \n" +
                   "           GROUP BY\n" +
                   "                  [date],\n" +
                   "                  b.[name],\n" +
                   "                  pc.[desc],\n" +
                   "                  cm.sdesc_OF\n" +
                   "           UNION ALL\n" +
                   "           --From Company\n" +
                   "           SELECT '2' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.AMOUNT) < 0 THEN (SUM(ch.AMOUNT) * -1)\n" +
                   "                       ELSE 0\n" +
                   "                  END              debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.Amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(ch.AMOUNT)\n" +
                   "                  END              credit,\n" +
                   "                  'Transfered To  ' + cm1.sdesc_OF cashtype,\n" +
                   "                  ch.t_date        date,\n" +
                   "                  b.[name]         branch,\n" +
                   "                  cm1.sdesc_OF     company\n" +
                   "           FROM   cih_transfer ch\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = ch.BRANCH\n" +
                   "                  INNER JOIN COMPANY_OF cm\n" +
                   "                       ON  cm.code_OF = ch.from_comp\n" +
                   "                  INNER JOIN COMPANY_OF cm1\n" +
                   "                       ON  cm1.code_OF = ch.to_comp\n" +
                   "           WHERE  ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                   "                  AND ch.t_date between '" + startOfMonth.ToString("yyyy-MM-dd") + "' " +
                         " and  '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' \n" +
                   "                  AND ch.from_comp = '" + hf_company.Value.ToString() + "'\n" +
                   "           GROUP BY\n" +
                   "                  ch.t_date,\n" +
                   "                  b.[name],\n" +
                   "                  cm1.sdesc_OF\n" +
                   "\n" +
                   "           UNION ALL\n" +
                   "           --To Company\n" +
                   "           SELECT '1' RANK,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.amount) < 0 THEN 0\n" +
                   "                       ELSE SUM(ch.amount)\n" +
                   "                  END             debit,\n" +
                   "                  CASE\n" +
                   "                       WHEN SUM(ch.amount) < 0 THEN (SUM(ch.amount) * -1)\n" +
                   "                       ELSE 0\n" +
                   "                  END             credit,\n" +
                   "                  'Received From  ' + cm.sdesc_OF cashtype,\n" +
                   "                  ch.t_date       date,\n" +
                   "                  b.[name]        branch,\n" +
                   "                  cm.sdesc_OF     company\n" +
                   "           FROM   cih_transfer ch\n" +
                   "                  INNER JOIN Branches b\n" +
                   "                       ON  b.branchCode = ch.BRANCH\n" +
                   "                   left outer  JOIN COMPANY_OF cm\n" +
                   "                       ON  cm.code_OF = ch.from_comp\n" +
                   "                  INNER JOIN COMPANY_OF cm1\n" +
                   "                       ON  cm1.code_OF = ch.to_comp\n" +
                   "           WHERE  ch.BRANCH  IN (" + hf_branch.Value.ToString() + ")\n" +
                   "                  AND ch.t_date between '" + startOfMonth.ToString("yyyy-MM-dd") + "' " +
                         " and  '" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' \n" +
                   "                  AND ch.to_comp = '" + hf_company.Value.ToString() + "'\n" +
                   "           GROUP BY\n" +
                   "                  ch.t_date,\n" +
                   "                  b.[name],\n" +
                   "                  cm.sdesc_OF\n" +
                           "    ) a) ";
                    }
                    sqlString += " ) b";
                }

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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

        #region converting amount to words
        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX  
                bool isDone = false;//test if already translated  
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))  
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric  
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping  
                    String place = "";//digit grouping name:hundres,thousand,etc...  
                    switch (numDigits)
                    {
                        case 1://ones' range  

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range  
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range  
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range  
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range  
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range  
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...  
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)  
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros  
                        //if (beginsZero) word = " and " + word.Trim();  
                    }
                    //ignore digit grouping names  
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents  
                        endStr = "Paisa " + endStr;//Cents  
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        #endregion
        #region Encryption & decryption
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }
}