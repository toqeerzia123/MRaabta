using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class Receipt_Print : System.Web.UI.Page
    {

        Cl_Variables clvar = new Cl_Variables();
        CommonFunction fun = new CommonFunction();
        cl_Encryption enc = new cl_Encryption();
        LoadingPrintReport enc_ = new LoadingPrintReport();


        int count = 0;
        Boolean flag = false;
        int page = 1;

        string createdBy = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
            {
                //  PrintBag();
                lbl_Dd.Text = DateTime.Now.Date.ToString();
                Print_Receipt();
            }

        }

        public void Print_Receipt()
        {
            clvar = new Cl_Variables();
            clvar.VoucherNo = Request.QueryString["ID"];
            DataSet ds = Get_ReceiptReport(clvar);

            if (ds.Tables[0].Rows.Count != 0)
            {
                lbl_Voucher_1.Text = ds.Tables[0].Rows[0]["id"].ToString();
                lbl_Voucher2.Text = ds.Tables[0].Rows[0]["id"].ToString();
                lbl_Branch.Text = ds.Tables[0].Rows[0]["BranchName"].ToString();
                lbl_Zone.Text = ds.Tables[0].Rows[0]["ZoneName"].ToString();
                lbl_ReceiptNo.Text = ds.Tables[0].Rows[0]["sbr"].ToString() + "-" + ds.Tables[0].Rows[0]["ReceiptNo"].ToString();
                lbl_ReceiptDate.Text = ds.Tables[0].Rows[0]["VoucherDate"].ToString().Substring(0, 10);
                lbl_Date.Text = ds.Tables[0].Rows[0]["VoucherDate"].ToString().Substring(0, 10);
                lbl_AccountNo.Text = ds.Tables[0].Rows[0]["Accountno"].ToString();
                lbl_AcountNo.Text = ds.Tables[0].Rows[0]["Accountno"].ToString();
                lbl_CustomeName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                lbl_CustomerType.Text = ds.Tables[0].Rows[0]["PaymentMode"].ToString();
                lbl_PaymentSource.Text = ds.Tables[0].Rows[0]["PaymentSource"].ToString();
                lbl_PaymentType.Text = ds.Tables[0].Rows[0]["PaymentType"].ToString();

                lbl_ConsignmentNo.Text = ds.Tables[0].Rows[0]["ConsignmentNo"].ToString();
                lbl_RiderName.Text = ds.Tables[0].Rows[0]["RiderName"].ToString();
                lbl_ExpressCenter.Text = ds.Tables[0].Rows[0]["ExpressCenter"].ToString();
                lbl_Amount.Text = string.Format("{0:N0}", Double.Parse(ds.Tables[0].Rows[0]["Amount"].ToString()));
                lbl_Balance.Text = string.Format("{0:N0}", Double.Parse(ds.Tables[0].Rows[0]["Balance"].ToString()));
                lbl_dslipNo.Text = ds.Tables[0].Rows[0]["DepositSlipNo"].ToString();
                lbl_dslipBank.Text = ds.Tables[0].Rows[0]["DepositSlipBank"].ToString();
                lbl_cprNo.Text = ds.Tables[0].Rows[0]["CPRNumber"].ToString();
                lbl_stwNo.Text = ds.Tables[0].Rows[0]["STWNumber"].ToString();
                GetProductWiseBreakDown(clvar.VoucherNo);

            }
        }

        protected void GetProductWiseBreakDown(string recID)
        {
            string query = "select * from MnP_PaymentVouchersProductBreakDown p where p.voucherID = '" + recID + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    gv_productWiseAmount.DataSource = dt;
                    gv_productWiseAmount.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
            finally { con.Close(); }
        }
        public DataSet Get_ReceiptReport(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT pv.*,  \n"
               + "       CASE   \n"
               + "            WHEN pv.IsByCreditClient = '1' THEN 'Credit'  \n"
               + "            ELSE 'Cash'  \n"
               + "       END                       PaymentMode,  \n"
               + "         \n"
               + "       pt.Name                   PaymentType,  \n"
               + "       (Amount - AmountUsed)     Balance,  \n"
               + "         \n"
               + "       (  \n"
               + "           SELECT TOP(1) r.firstName + ' ' + r.lastName  \n"
               + "           FROM   Riders r  \n"
               + "           WHERE  r.riderCode = pv.RiderCode  AND r.branchId = pv.BranchCode   \n"
               + "       )                         RiderName,  \n"
               + "       (  \n"
               + "           SELECT r.name  \n"
               + "           FROM   ExpressCenters r  \n"
               + "           WHERE  r.expressCenterCode = pv.ExpressCenterCode  \n"
               + "       )                         ExpressCenter,  \n"
               + "       (  \n"
               + "           SELECT r.accountNo  \n"
               + "           FROM   CreditClients r  \n"
               + "           WHERE   r.id = pv.CreditClientId  \n"
               + "       )                         accountNo,  \n"
               + "       (  \n"
               + "           SELECT r.name  \n"
               + "           FROM   CreditClients r  \n"
               + "           WHERE   r.id = pv.CreditClientId  \n"
               + "       )                         name,  \n"
               + "        (  \n"
               + "           SELECT r.name  \n"
               + "           FROM   PaymentSource r  \n"
               + "           WHERE  pv.PaymentSourceId = r.Id  \n"
               + "       )                         PaymentSource, \n"
               + "       b.name BranchName, b.sname sbr,  \n"
               + "       z.name ZoneName,pv.ExpressCenterCode,pv.RiderCode,pv.ChequeNo,pv.BankId    \n"
               + "FROM   PaymentVouchers           pv,  \n"
               + "         \n"
               + "       PaymentTypes              pt,  \n"
               + "        \n"
               + "       Branches b,  \n"
               + "       Zones z  \n"
               + "WHERE   \n"
               + "       pv.PaymentTypeId = pt.Id  \n"
               + "       AND b.branchCode = pv.BranchCode  \n"
               + "       AND b.zoneCode = pv.ZoneCode  \n"
               + "       AND z.zoneCode = pv.ZoneCode \n"
                + "       AND pv.Id = '" + clvar.VoucherNo + "'";


                string sqlString = "SELECT pv.*,\n" +
                "       CASE\n" +
                "         WHEN pv.IsByCreditClient = '1' THEN\n" +
                "          'Credit'\n" +
                "         ELSE\n" +
                "          'Cash'\n" +
                "       END PaymentMode,\n" +
                "\n" +
                "       pt.Name PaymentType,\n" +
                "       (Amount - AmountUsed) Balance,\n" +
                "\n" +
                "       (SELECT TOP(1) r.firstName + ' ' + r.lastName\n" +
                "          FROM Riders r\n" +
                "         WHERE r.riderCode = pv.RiderCode\n" +
                "           AND r.branchId = pv.BranchCode) RiderName,\n" +
                "       (SELECT r.name\n" +
                "          FROM ExpressCenters r\n" +
                "         WHERE r.expressCenterCode = pv.ExpressCenterCode) ExpressCenter,\n" +
                "       (SELECT r.accountNo\n" +
                "          FROM CreditClients r\n" +
                "         WHERE r.id = pv.CreditClientId) accountNo,\n" +
                "       (SELECT r.name FROM CreditClients r WHERE r.id = pv.CreditClientId) name,\n" +
                "       (SELECT r.name FROM PaymentSource r WHERE pv.PaymentSourceId = r.Id) PaymentSource,\n" +
                "       b.name BranchName,\n" +
                "       b.sname sbr,\n" +
                "       z.name ZoneName,\n" +
                "       pv.ExpressCenterCode,\n" +
                "       pv.RiderCode,\n" +
                "       pv.ChequeNo,\n" +
                "       pv.BankId,\n" +
                "       pv.depositSlipNo,\n" +
                "       (SELECT name from Banks where Id = pv.depositSlipBankID) DepositSlipBank,\n" +
                "       pv.STWNumber,\n" +
                "       pv.CPRNumber\n" +
                "  FROM PaymentVouchers pv, PaymentTypes pt, Branches b, Zones z\n" +
                " WHERE pv.PaymentTypeId = pt.Id\n" +
                "   AND b.branchCode = pv.BranchCode\n" +
                "   AND b.zoneCode = pv.ZoneCode\n" +
                "   AND z.zoneCode = pv.ZoneCode\n" +
                "\n" +
                "   AND pv.Id = '" + clvar.VoucherNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
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
    }
}