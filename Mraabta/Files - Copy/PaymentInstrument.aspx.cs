using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Configuration;

namespace MRaabta.Files
{
    public partial class PaymentInstrument : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime test = DateTime.Parse("03-11-2017");
            Errorid.Text = "";
            if (chk_Allzone.Checked)
            {
                dd_zone.Enabled = false;
                dd_branch.Enabled = false;
                chk_Allbranch.Enabled = false;
                chk_Allbranch.Checked = true;
                //dd_zone.Attributes.Add("disabled", "true");
                //dd_branch.Attributes.Add("disabled", "true");
                //chk_Allbranch.Attributes.Add("disabled", "true");
                //chk_Allbranch.Attributes.Add("checked", "true");
                ////dd_zone.disabled = true;
                ////dd_branch.disabled = true

            }
            else
            {
                dd_zone.Enabled = true;
                chk_Allbranch.Enabled = true;
                //dd_zone.Attributes.Remove("disabled");
                //dd_zone.Attributes.Add("disabled", "false");

                //chk_Allbranch.Attributes.Add("disabled", "false");
                if (chk_Allbranch.Checked)
                {
                    dd_branch.Enabled = false;
                    //dd_branch.Attributes.Add("disabled", "true");
                }
                else
                {
                    dd_branch.Enabled = true;
                    //dd_branch.Attributes.Add("disabled", "false");
                }
            }
            //if (HttpContext.Current.Session["U_ID"] == null)
            //{

            //    Response.Redirect("~/login");
            //}

            //if (!IsPostBack)
            //{
            //    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //    //  txt_fromdate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            //    //   txt_todate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            if (!IsPostBack)
            {
                GET_Zone();
            }

            //}
        }

        protected void chk_Allbranch_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Allbranch.Checked)
            {
                ViewState["ddl_branch"] = dd_branch.SelectedValue;
                dd_branch.Enabled = false;
                dd_zone.Enabled = false;
            }
            else
            {
                dd_branch.Enabled = true;
                dd_zone.Enabled = true;
            }

        }

        protected void chk_Allzone_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Allzone.Checked)
            {
                ViewState["ddl_zone"] = dd_zone.SelectedValue;
                dd_zone.Enabled = false;
            }
            else
            {
                dd_zone.Enabled = true;
            }
        }


        public void GET_Zone()
        {
            //    dd_zone.Items.Insert(0, new ListItem("SELECT ZONE", "0"));
            DataSet ds = Get_zone();
            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_zone.DataTextField = "zonename";
                dd_zone.DataValueField = "zoneCode";
                dd_zone.DataSource = ds.Tables[0].DefaultView;
                dd_zone.DataBind();
            }

        }

        protected void dd_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clearing other items
            dd_branch.Items.Clear();
            dd_branch.Items.Insert(0, new ListItem("Select Branch", "0"));

            if (dd_zone.SelectedValue.ToString() != "0")
            {
                //SELECTING Branche

                DataSet ds = GET_BRANCHES(dd_zone.SelectedValue);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dd_branch.Items.Clear();
                    dd_branch.DataTextField = "branches";
                    dd_branch.DataValueField = "branchcode";
                    dd_branch.DataSource = ds.Tables[0].DefaultView;
                    dd_branch.DataBind();
                    dd_branch.Items.Insert(0, new ListItem("Select Branch", "0"));
                }
            }


        }


        public DataSet Get_zone()
        {
            string query = "";


            query = "SELECT DISTINCT z.zoneCode,\n" +
            "                  z.name as zonename\n" +
            "           FROM   Zones z\n" +
            "                  INNER JOIN Branches b\n" +
            "                       ON  b.zoneCode = z.zoneCode\n" +
            "                       AND b.[status] = '1'\n" +
            "           WHERE  z.[status] = '1'";

            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }

        public DataSet GET_BRANCHES(string zone)
        {
            string query = "";



            query = "select sname + ' - ' + name as branches,\n" +
            "branchcode\n" +
            "from branches\n" +
            "where zonecode = '" + zone + "'\n" +
            "and status = '1'";

            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }


        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (chk_Allzone.Checked)
            {
                clvar.CheckCondition = "";
            }
            else
            {
                if (dd_zone.SelectedValue == "0")
                {
                    AlertMessage("Select Zone", "Red");
                    return;
                }
                else
                {
                    clvar.CheckCondition += "AND c.zoneCode = '" + dd_zone.SelectedValue + "'\n";
                    if (!chk_Allbranch.Checked)
                    {
                        if (dd_branch.SelectedValue == "0")
                        {
                            AlertMessage("Select Branch", "Red");
                            return;
                        }
                        else
                        {
                            clvar.CheckCondition += "AND c.BranchCode = '" + dd_branch.SelectedValue + "'\n";
                        }
                    }

                }
            }

            if (txt_accountNo.Text.Trim() != "")
            {
                clvar.CheckCondition += "AND c.consignerAccountNo = '" + txt_accountNo.Text.Trim().ToUpper() + "'\n";
            }

            if (txt_fromdate.Text.Trim() != "")
            {
                clvar.FromDate = DateTime.Parse(txt_fromdate.Text);
            }
            else
            {
                AlertMessage("Enter From Date", "Red");
                return;
            }
            if (txt_todate.Text.Trim() != "")
            {
                clvar.ToDate = DateTime.Parse(txt_todate.Text);
            }
            else
            {
                AlertMessage("Enter To Date", "Red");
                return;
            }

            DataTable dt = GetData(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_payments.DataSource = dt;
                    gv_payments.DataBind();
                }
                else
                {
                    gv_payments.DataSource = null;
                    gv_payments.DataBind();
                }
            }
            else
            {
                gv_payments.DataSource = null;
                gv_payments.DataBind();
            }
        }

        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
            Errorid.Text = message;
            return;
        }

        public DataTable GetData(Cl_Variables clvar)
        {

            string sqlString = "SELECT z.name ZoneName,\n" +
            "       A.zoneCode,\n" +
            "       b.sname + ' - ' + b.name BranchName,\n" +
            "       A.branchCode,\n" +
            "       A.accountNo,\n" +
            "       A.creditclientID,\n" +
            "       A.AccName,\n" +
            "       A.PaymentID,\n" +
            "       A.PaidOn,\n" +
            "       SUM(A.RRAmount) RRAmount,\n" +
            "       SUM(A.ShippingCharges) InvoiceAmount,\n" +
            "       SUM(A.RRAmount) - SUM(A.ShippingCharges) NetPayable,\n" +
            "       A.InstrumentMode,\n" +
            "       A.InstrumentNumber\n" +
            "  FROM (SELECT cc.zoneCode,\n" +
            "               cc.branchCode,\n" +
            "               cc.accountNo,\n" +
            "               cc.id creditclientID,\n" +
            "               cc.name AccName,\n" +
            "               c.transactionNumber PaymentID,\n" +
            "               FORMAT(c.paidon, 'yyyy-MM-dd') PaidOn,\n" +
            "               SUM(pv.Amount) RRAmount,\n" +
            "               '' InvoiceNumber,\n" +
            "               0 ShippingCharges,\n" +
            "               c.InstrumentMode,\n" +
            "               c.InstrumentNumber\n" +
            "          FROM Consignment c\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON cc.id = c.creditClientId\n" +
            "           AND cc.accountNo = c.consignerAccountNo\n" +
            "         INNER JOIN PaymentVouchers pv\n" +
            "            ON pv.ConsignmentNo = c.consignmentNumber\n" +
            "         WHERE 1 = 1\n" +
            "           " + clvar.CheckCondition + "" +
            "           AND CAST(c.paidon AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "         GROUP BY cc.zoneCode,\n" +
            "                  cc.branchCode,\n" +
            "                  cc.accountNo,\n" +
            "                  cc.id,\n" +
            "                  cc.name,\n" +
            "                  c.transactionNumber,\n" +
            "                  c.paidon,\n" +
            "                  c.InstrumentMode,\n" +
            "                  c.InstrumentNumber\n" +
            "\n" +
            "        UNION ALL\n" +
            "        SELECT cc.zoneCode,\n" +
            "               cc.branchCode,\n" +
            "               cc.accountNo,\n" +
            "               cc.id creditclientID,\n" +
            "               cc.name AccName,\n" +
            "               c.transactionNumber PaymentID,\n" +
            "               FORMAT(c.paidon, 'yyyy-MM-dd') PaidOn,\n" +
            "               0 RRAmount,\n" +
            "               i.invoiceNumber,\n" +
            "               SUM(i.totalAmount) / COUNT(i.invoiceNumber) ShippingCharges,\n" +
            "               c.InstrumentMode,\n" +
            "               c.InstrumentNumber\n" +
            "          FROM Consignment c\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON cc.id = c.creditClientId\n" +
            "           AND cc.accountNo = c.consignerAccountNo\n" +
            "         INNER JOIN PaymentVouchers pv\n" +
            "            ON pv.ConsignmentNo = c.consignmentNumber\n" +
            "         INNER JOIN InvoiceRedeem ir\n" +
            "            ON ir.PaymentVoucherId = pv.Id\n" +
            "         INNER JOIN Invoice i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "         WHERE 1 = 1\n" +
            "           " + clvar.CheckCondition + "" +
            "           AND CAST(c.paidon AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "         GROUP BY cc.zoneCode,\n" +
            "                  cc.branchCode,\n" +
            "                  cc.accountNo,\n" +
            "                  cc.id,\n" +
            "                  cc.name,\n" +
            "                  c.transactionNumber,\n" +
            "                  c.paidon,\n" +
            "                  i.invoiceNumber,\n" +
            "                  c.InstrumentMode,\n" +
            "                  c.InstrumentNumber) A\n" +
            " INNER JOIN Zones z\n" +
            "    ON z.zoneCode = A.zoneCode\n" +
            "   AND z.[status] = '1'\n" +
            " INNER JOIN Branches b\n" +
            "    ON b.branchCode = a.branchCode\n" +
            "   AND b.[status] = '1'\n" +
            " GROUP BY z.name,\n" +
            "          b.name,\n" +
            "          b.sname,\n" +
            "          A.zoneCode,\n" +
            "          A.branchCode,\n" +
            "          A.accountNo,\n" +
            "          A.creditclientID,\n" +
            "          A.AccName,\n" +
            "          A.PaymentID,\n" +
            "          A.PaidOn,\n" +
            "          A.InstrumentMode,\n" +
            "          A.InstrumentNumber order by PaymentID";



            sqlString = "SELECT z.name ZoneName,\n" +
            "       A.zoneCode,\n" +
            "       b.sname + ' - ' + b.name BranchName,\n" +
            "       A.branchCode,\n" +
            "       A.accountNo,\n" +
            "       A.creditclientID,\n" +
            "       A.AccName,\n" +
            "       A.PaymentID,\n" +
            "       A.PaidOn,\n" +
            "       SUM(A.RRAmount) RRAmount,\n" +
            "       SUM(A.ShippingCharges) InvoiceAmount,\n" +
            "       SUM(A.RRAmount) - SUM(A.ShippingCharges) NetPayable,\n" +
            "       A.InstrumentMode,\n" +
            "       A.InstrumentNumber,\n" +
            "       A.BenName,\n" +
            "       A.BenAccNo,\n" +
            "       A.BenBankCode,\n" +
            "       A.BenBankName\n" +
            "  FROM (SELECT cc.zoneCode,\n" +
            "               cc.branchCode,\n" +
            "               cc.accountNo,\n" +
            "               cc.id creditclientID,\n" +
            "               cc.name AccName,\n" +
            "               c.transactionNumber PaymentID,\n" +
            "               FORMAT(c.paidon, 'yyyy-MM-dd') PaidOn,\n" +
            "               SUM(pv.Amount) RRAmount,\n" +
            "               '' InvoiceNumber,\n" +
            "               0 ShippingCharges,\n" +
            "               c.InstrumentMode,\n" +
            "               c.InstrumentNumber,\n" +
            "               cc.BeneficiaryName BenName,\n" +
            "               cc.BeneficiaryBankAccNo BenAccNo,\n" +
            "               b.SBPCode BenBankCode,\n" +
            "               b.Name BenBankName\n" +
            "          FROM Consignment c\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON cc.id = c.creditClientId\n" +
            "           AND cc.accountNo = c.consignerAccountNo\n" +
            "         INNER JOIN PaymentVouchers pv\n" +
            "            ON pv.ConsignmentNo = c.consignmentNumber\n" +
            "          LEFT OUTER JOIN Banks b\n" +
            "            ON b.Id = cc.BeneficiaryBankCode\n" +
            "           AND b.isMNPBank = '0'\n" +
            "         WHERE 1 = 1\n" +
            "           " + clvar.CheckCondition + "" +
            "           AND CAST(c.paidon AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "         GROUP BY cc.zoneCode,\n" +
            "                  cc.branchCode,\n" +
            "                  cc.accountNo,\n" +
            "                  cc.id,\n" +
            "                  cc.name,\n" +
            "                  c.transactionNumber,\n" +
            "                  c.paidon,\n" +
            "                  c.InstrumentMode,\n" +
            "                  c.InstrumentNumber,\n" +
            "                  cc.BeneficiaryName,\n" +
            "                  cc.BeneficiaryBankAccNo,\n" +
            "                  b.SBPCode,\n" +
            "                  b.Name\n" +
            "\n" +
            "        UNION ALL\n" +
            "        SELECT cc.zoneCode,\n" +
            "               cc.branchCode,\n" +
            "               cc.accountNo,\n" +
            "               cc.id creditclientID,\n" +
            "               cc.name AccName,\n" +
            "               c.transactionNumber PaymentID,\n" +
            "               FORMAT(c.paidon, 'yyyy-MM-dd') PaidOn,\n" +
            "               0 RRAmount,\n" +
            "               i.invoiceNumber,\n" +
            "               SUM(i.totalAmount) / COUNT(i.invoiceNumber) ShippingCharges,\n" +
            "               c.InstrumentMode,\n" +
            "               c.InstrumentNumber,\n" +
            "               cc.BeneficiaryName BenName,\n" +
            "               cc.BeneficiaryBankAccNo BenAccNo,\n" +
            "               b.SBPCode BenBankCode,\n" +
            "               b.Name BenBankName\n" +
            "          FROM Consignment c\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON cc.id = c.creditClientId\n" +
            "           AND cc.accountNo = c.consignerAccountNo\n" +
            "         INNER JOIN PaymentVouchers pv\n" +
            "            ON pv.ConsignmentNo = c.consignmentNumber\n" +
            "         INNER JOIN InvoiceRedeem ir\n" +
            "            ON ir.PaymentVoucherId = pv.Id\n" +
            "         INNER JOIN Invoice i\n" +
            "            ON i.invoiceNumber = ir.InvoiceNo\n" +
            "          LEFT OUTER JOIN Banks b\n" +
            "            ON b.Id = cc.BeneficiaryBankCode\n" +
            "           AND b.isMNPBank = '0'\n" +
            "         WHERE 1 = 1\n" +
            "           " + clvar.CheckCondition + "" +
            "           AND CAST(c.paidon AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "         GROUP BY cc.zoneCode,\n" +
            "                  cc.branchCode,\n" +
            "                  cc.accountNo,\n" +
            "                  cc.id,\n" +
            "                  cc.name,\n" +
            "                  c.transactionNumber,\n" +
            "                  c.paidon,\n" +
            "                  i.invoiceNumber,\n" +
            "                  c.InstrumentMode,\n" +
            "                  c.InstrumentNumber,\n" +
            "                  cc.BeneficiaryName,\n" +
            "                  cc.BeneficiaryBankAccNo,\n" +
            "                  b.SBPCode,\n" +
            "                  b.Name) A\n" +
            " INNER JOIN Zones z\n" +
            "    ON z.zoneCode = A.zoneCode\n" +
            "   AND z.status = '1'\n" +
            " INNER JOIN Branches b\n" +
            "    ON b.branchCode = a.branchCode\n" +
            "   AND b.status = '1'\n" +
            " GROUP BY z.name,\n" +
            "          b.name,\n" +
            "          b.sname,\n" +
            "          A.zoneCode,\n" +
            "          A.branchCode,\n" +
            "          A.accountNo,\n" +
            "          A.creditclientID,\n" +
            "          A.AccName,\n" +
            "          A.PaymentID,\n" +
            "          A.PaidOn,\n" +
            "          A.InstrumentMode,\n" +
            "          A.InstrumentNumber,\n" +
            "          A.BenName,\n" +
            "          BenAccNo,\n" +
            "          BenBankCode,\n" +
            "          BenBankName\n" +
            " ORDER BY PaymentID";


            sqlString = "SELECT Z.NAME ZONENAME,\n" +
            "       A.ZONECODE,\n" +
            "       B.SNAME + ' - ' + B.NAME BRANCHNAME,\n" +
            "       A.BRANCHCODE,\n" +
            "       A.ACCOUNTNO,\n" +
            "       A.CREDITCLIENTID,\n" +
            "       A.ACCNAME,\n" +
            "       A.PAYMENTID,\n" +
            "       A.PAIDON,\n" +
            "       SUM(A.RRAMOUNT) RRAMOUNT,\n" +
            "       SUM(A.SHIPPINGCHARGES) INVOICEAMOUNT,\n" +
            "       SUM(A.RRAMOUNT) - SUM(A.SHIPPINGCHARGES) NETPAYABLE,\n" +
            "       A.INSTRUMENTMODE,\n" +
            "       A.INSTRUMENTNUMBER,\n" +
            "       A.BENNAME,\n" +
            "       A.BENACCNO,\n" +
            "       A.BENBANKCODE,\n" +
            "       A.BENBANKNAME\n" +
            "  FROM (SELECT CC.ZONECODE,\n" +
            "               CC.BRANCHCODE,\n" +
            "               CC.ACCOUNTNO,\n" +
            "               CC.ID CREDITCLIENTID,\n" +
            "               CC.NAME ACCNAME,\n" +
            "               C.TRANSACTIONNUMBER PAYMENTID,\n" +
            "               FORMAT(C.PAIDON, 'yyyy-MM-dd') PAIDON,\n" +
            "               SUM(PV.AMOUNT) RRAMOUNT,\n" +
            "               '' INVOICENUMBER,\n" +
            "               0 SHIPPINGCHARGES,\n" +
            "               C.INSTRUMENTMODE,\n" +
            "               C.INSTRUMENTNUMBER,\n" +
            "               CC.BENEFICIARYNAME BENNAME,\n" +
            "               CC.BENEFICIARYBANKACCNO BENACCNO,\n" +
            "               B.SBPCODE BENBANKCODE,\n" +
            "               B.NAME BENBANKNAME\n" +
            "          FROM CONSIGNMENT C\n" +
            "         INNER JOIN CREDITCLIENTS CC\n" +
            "            ON CC.ID = C.CREDITCLIENTID\n" +
            "           AND CC.ACCOUNTNO = C.CONSIGNERACCOUNTNO\n" +
            "         INNER JOIN PAYMENTVOUCHERS PV\n" +
            "            ON PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER\n" +
            "          LEFT OUTER JOIN BANKS B\n" +
            "            ON B.ID = CC.BENEFICIARYBANKCODE\n" +
            "           AND B.ISMNPBANK = '0'\n" +
            "         WHERE 1 = 1\n" +
            "           " + clvar.CheckCondition + "" +
            "           AND CAST(c.paidon AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "         GROUP BY CC.ZONECODE,\n" +
            "                  CC.BRANCHCODE,\n" +
            "                  CC.ACCOUNTNO,\n" +
            "                  CC.ID,\n" +
            "                  CC.NAME,\n" +
            "                  C.TRANSACTIONNUMBER,\n" +
            "                  C.PAIDON,\n" +
            "                  C.INSTRUMENTMODE,\n" +
            "                  C.INSTRUMENTNUMBER,\n" +
            "                  CC.BENEFICIARYNAME,\n" +
            "                  CC.BENEFICIARYBANKACCNO,\n" +
            "                  B.SBPCODE,\n" +
            "                  B.NAME\n" +
            "\n" +
            "        UNION ALL\n" +
            "        SELECT CC.ZONECODE,\n" +
            "               CC.BRANCHCODE,\n" +
            "               CC.ACCOUNTNO,\n" +
            "               CC.ID CREDITCLIENTID,\n" +
            "               CC.NAME ACCNAME,\n" +
            "               C.TRANSACTIONNUMBER PAYMENTID,\n" +
            "               FORMAT(C.PAIDON, 'yyyy-MM-dd') PAIDON,\n" +
            "               0 RRAMOUNT,\n" +
            "               I.INVOICENUMBER,\n" +
            "               (SUM(I.TOTALAMOUNT) / COUNT(I.INVOICENUMBER)) -\n" +
            "               (SUM(ISNULL(GV.AMOUNT, 0)) / COUNT(ISNULL(GV.ID, 0))) SHIPPINGCHARGES,\n" +
            "               C.INSTRUMENTMODE,\n" +
            "               C.INSTRUMENTNUMBER,\n" +
            "               CC.BENEFICIARYNAME BENNAME,\n" +
            "               CC.BENEFICIARYBANKACCNO BENACCNO,\n" +
            "               B.SBPCODE BENBANKCODE,\n" +
            "               B.NAME BENBANKNAME\n" +
            "          FROM CONSIGNMENT C\n" +
            "         INNER JOIN CREDITCLIENTS CC\n" +
            "            ON CC.ID = C.CREDITCLIENTID\n" +
            "           AND CC.ACCOUNTNO = C.CONSIGNERACCOUNTNO\n" +
            "         INNER JOIN PAYMENTVOUCHERS PV\n" +
            "            ON PV.CONSIGNMENTNO = C.CONSIGNMENTNUMBER\n" +
            "         INNER JOIN INVOICEREDEEM IR\n" +
            "            ON IR.PAYMENTVOUCHERID = PV.ID\n" +
            "         INNER JOIN INVOICE I\n" +
            "            ON I.INVOICENUMBER = IR.INVOICENO\n" +
            "          LEFT OUTER JOIN BANKS B\n" +
            "            ON B.ID = CC.BENEFICIARYBANKCODE\n" +
            "           AND B.ISMNPBANK = '0'\n" +
            "          LEFT OUTER JOIN GENERALVOUCHER GV\n" +
            "            ON GV.CREDITCLIENTID = CC.ID\n" +
            "           AND GV.INVOICENO = I.INVOICENUMBER\n" +
            "         WHERE 1 = 1\n" +
            "           " + clvar.CheckCondition + "" +
            "           AND CAST(c.paidon AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "         GROUP BY CC.ZONECODE,\n" +
            "                  CC.BRANCHCODE,\n" +
            "                  CC.ACCOUNTNO,\n" +
            "                  CC.ID,\n" +
            "                  CC.NAME,\n" +
            "                  C.TRANSACTIONNUMBER,\n" +
            "                  C.PAIDON,\n" +
            "                  I.INVOICENUMBER,\n" +
            "                  C.INSTRUMENTMODE,\n" +
            "                  C.INSTRUMENTNUMBER,\n" +
            "                  CC.BENEFICIARYNAME,\n" +
            "                  CC.BENEFICIARYBANKACCNO,\n" +
            "                  B.SBPCODE,\n" +
            "                  B.NAME) A\n" +
            " INNER JOIN ZONES Z\n" +
            "    ON Z.ZONECODE = A.ZONECODE\n" +
            "   AND Z.STATUS = '1'\n" +
            " INNER JOIN BRANCHES B\n" +
            "    ON B.BRANCHCODE = A.BRANCHCODE\n" +
            "   AND B.STATUS = '1'\n" +
            " GROUP BY Z.NAME,\n" +
            "          B.NAME,\n" +
            "          B.SNAME,\n" +
            "          A.ZONECODE,\n" +
            "          A.BRANCHCODE,\n" +
            "          A.ACCOUNTNO,\n" +
            "          A.CREDITCLIENTID,\n" +
            "          A.ACCNAME,\n" +
            "          A.PAYMENTID,\n" +
            "          A.PAIDON,\n" +
            "          A.INSTRUMENTMODE,\n" +
            "          A.INSTRUMENTNUMBER,\n" +
            "          A.BENNAME,\n" +
            "          BENACCNO,\n" +
            "          BENBANKCODE,\n" +
            "          BENBANKNAME\n" +
            " ORDER BY PAYMENTID";



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
        protected void gv_payments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hd_instrumentMode = e.Row.FindControl("hd_instrumentMode") as HiddenField;
                RadioButtonList rbtn_modeList = e.Row.FindControl("rbtn_gMode") as RadioButtonList;
                if (hd_instrumentMode.Value == "1")
                {
                    rbtn_modeList.SelectedValue = "1";
                }
                else if (hd_instrumentMode.Value == "2")
                {
                    rbtn_modeList.SelectedValue = "2";
                }
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            dd_branch.ClearSelection();
            chk_Allbranch.Checked = false;
            chk_Allzone.Checked = false;
            txt_accountNo.Text = "";
            txt_fromdate.Text = "";
            txt_todate.Text = "";

            gv_payments.DataSource = null;
            gv_payments.DataBind();
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("PaymentID", typeof(string)),
        new DataColumn("InstrumentMode", typeof(int)),
        new DataColumn("InstrumentNumber", typeof(string)),
        new DataColumn("AccountNo", typeof(string))
        });
            bool modeNotSelected = false;
            bool emptyInstrumentNumber = false;
            foreach (GridViewRow row in gv_payments.Rows)
            {
                bool isChecked = (row.FindControl("chk_chk") as CheckBox).Checked;
                string paymentID = row.Cells[9].Text;
                string AccountNo = row.Cells[3].Text;
                string instrumentNumber = (row.FindControl("txt_gInstrumentNumber") as TextBox).Text;
                string mode = (row.FindControl("rbtn_gMode") as RadioButtonList).SelectedValue;
                int instrumentMode = 0;
                int.TryParse(mode, out instrumentMode);

                if (isChecked)
                {
                    if (mode == "")
                    {
                        modeNotSelected = true;
                        row.Cells[14].BackColor = System.Drawing.Color.LightPink;
                    }
                    else
                    {
                        row.Cells[14].BackColor = System.Drawing.Color.White;
                    }
                    if (instrumentNumber.Trim() == "")
                    {
                        emptyInstrumentNumber = true;
                        row.Cells[15].BackColor = System.Drawing.Color.LightPink;
                    }
                    else
                    {
                        row.Cells[15].BackColor = System.Drawing.Color.White;
                    }
                    dt.Rows.Add(paymentID, instrumentMode, instrumentNumber, AccountNo);
                }

            }
            if (modeNotSelected)
            {
                AlertMessage("Select Instrument Mode of the highlighted Payments.", "Red");
                return;
            }
            if (emptyInstrumentNumber)
            {
                AlertMessage("Enter Instrument Number of the highlighted Payments.", "Red");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                Tuple<int, string> resp = UpdateInstrumentNumbers(clvar, dt);
                if (resp.Item1 == 1)
                {
                    AlertMessage("Instrument Numbers Updated", "Green");
                    btn_search_Click(this, e);
                    return;
                }
                else if (resp.Item1 == 0)
                {
                    AlertMessage(resp.Item2, "Red");
                    return;
                }
            }
            else
            {
                AlertMessage("Select Payments to Save Data", "Red");
            }
        }
        protected Tuple<int, string> UpdateInstrumentNumbers(Cl_Variables clvar, DataTable dt)
        {
            Tuple<int, string> resp = new Tuple<int, string>(0, "");
            SqlConnection con = new SqlConnection(clvar.Strcon());

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_UpdateInstrumentNumbers";
                cmd.Parameters.AddWithValue("@RDetails", dt);
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                object obj = cmd.Parameters["@ResultStatus"].SqlValue.ToString();
                int temp = 0;
                int.TryParse(obj.ToString(), out temp);

                resp = new Tuple<int, string>(temp, cmd.Parameters["@result"].SqlValue.ToString());

            }
            catch (Exception ex)
            { resp = new Tuple<int, string>(0, ex.Message); }
            finally { con.Close(); }
            return resp;

            // return "";
        }
    }
}