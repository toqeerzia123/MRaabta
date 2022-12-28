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
    public partial class Manage_Invoices : System.Web.UI.Page
    {

        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {

            Errorid.Text = "";
            if (!IsPostBack)
            {
                GetCompanies();
                pickerEndDate.SelectedDate = pickerInvoiceDate.SelectedDate = pickerStart.SelectedDate = DateTime.Today.Date;//DateTime.Parse(Session["WorkingDate"].ToString());
            }
        }
        public void GetCompanies()
        {
            DataTable dt = func.GetCompanies();
            dd_company.DataSource = dt;
            dd_company.DataTextField = "CompanyName";
            dd_company.DataValueField = "ID";
            dd_company.DataBind();
        }

        protected void btn_getConsignment_Click(object sender, EventArgs e)
        {
            clvar.Company = dd_company.SelectedValue;
            clvar.FromDate = pickerStart.SelectedDate.Value;
            clvar.ToDate = pickerEndDate.SelectedDate.Value;
            clvar.CustomerClientID = hd_customerClientID.Value;
            txt_clientAccNo_TextChanged(sender, e);
            clvar.AccountNo = txt_clientAccNo.Text;
            DataTable dt = GetConsignmentsForInvoice(clvar);
            DataTable dt_ = GetConsignmentsForInvoice_count(clvar);
            lbl_Count1.Text = dt_.Rows[0][0].ToString();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    try
                    {
                        gv_cns.DataSource = dt.Select("ispriceComputed = 'True'").CopyToDataTable();
                        gv_cns.DataBind();
                    }
                    catch (Exception ex)
                    { }
                    try
                    {
                        gv_notComputedCNS.DataSource = dt.Select("ispriceComputed = 'False'").CopyToDataTable();
                        gv_notComputedCNS.DataBind();
                    }
                    catch (Exception ex)
                    { }

                    if (gv_notComputedCNS.Rows.Count > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Invoice. Uncomputed CN(s) Exist')", true);
                        return;
                    }

                }
                else
                {
                    gv_cns.DataSource = null;
                    gv_cns.DataBind();
                    gv_notComputedCNS.DataSource = null;
                    gv_notComputedCNS.DataBind();
                }
            }

        }
        protected void txt_clientAccNo_TextChanged(object sender, EventArgs e)
        {
            gv_cns.DataSource = null;
            gv_cns.DataBind();
            gv_notComputedCNS.DataSource = null;
            gv_notComputedCNS.DataBind();

            clvar.AccountNo = txt_clientAccNo.Text;
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();

            DataTable dt = func.GetAccountDetailByAccountNumber(clvar);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    hd_customerClientID.Value = dt.Rows[0]["ID"].ToString();
                    txt_clientName.Text = dt.Rows[0]["NAme"].ToString();

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account No')", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Account No')", true);
                return;
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void ResetAll()
        {
            txt_clientAccNo.Text = "";
            txt_clientName.Text = "";
            txt_invoiceNo.Text = "";
            if (rbtn_formMode.SelectedValue == "1")
            {
                txt_invoiceNo.Enabled = true;
                txt_clientAccNo.Enabled = false;
                txt_clientName.Enabled = false;
                dd_company.Enabled = false;
                pickerStart.Enabled = false;
                pickerEndDate.Enabled = false;
                pickerInvoiceDate.Enabled = false;
                btn_print.Visible = true;
                btn_save.Visible = false;
            }
            else
            {
                txt_invoiceNo.Enabled = false;
                txt_clientAccNo.Enabled = true;
                txt_clientName.Enabled = true;
                dd_company.Enabled = true;
                pickerStart.Enabled = true;
                pickerEndDate.Enabled = true;
                pickerInvoiceDate.Enabled = true;
                btn_print.Visible = false;
                btn_save.Visible = true;
            }
            gv_cns.DataSource = null;
            gv_cns.DataBind();
            gv_notComputedCNS.DataSource = null;
            gv_notComputedCNS.DataBind();
            pickerEndDate.SelectedDate = DateTime.Parse(DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("dd/MM/yyyy"));
            pickerInvoiceDate.SelectedDate = DateTime.Parse(DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("dd/MM/yyyy"));
            pickerStart.SelectedDate = DateTime.Parse(DateTime.Parse(HttpContext.Current.Session["WorkingDate"].ToString()).ToString("dd/MM/yyyy"));
            dd_company.ClearSelection();

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (gv_notComputedCNS.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Invoice. Uncomputed CN(s) Exist')", true);
                return;
            }

            DataTable dt = Get_InvoiceEndCheck(HttpContext.Current.Session["BranchCode"].ToString(), HttpContext.Current.Session["ZONECODE"].ToString(), pickerStart.SelectedDate.Value.ToString("yyyy-MM-dd"), pickerEndDate.SelectedDate.Value.ToString("yyyy-MM-dd"));

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Month Already Closed')", true);
                //    err_msg.Text = "";
                return;

            }

            if (gv_cns.Rows.Count > 0)
            {
                txt_clientAccNo_TextChanged(this, e);
                clvar.Company = dd_company.SelectedValue;
                clvar.CreditClientID = hd_customerClientID.Value;
                clvar.FromDate = DateTime.Parse(pickerStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                clvar.ToDate = DateTime.Parse(pickerEndDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                clvar.LoadingDate = pickerInvoiceDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                string error = GenerateManualInvoice(clvar);
                double number = 0;
                double.TryParse(error, out number);
                if (number == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Create invoice.\\nError: " + error + "')", true);
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    Errorid.Text = error;
                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invoice Generated: Invoice Number " + number + "')", true);
                    btn_reset_Click(this, e);
                    //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    //string script = String.Format(script_, "InvoiceSummary.aspx?XCode=" + error, "_blank", "");
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                }
            }
        }

        public DataTable Get_InvoiceEndCheck(string branch, string Zone, string fromdate, string todate)
        {
            string sql = "SELECT * FROM Mnp_Account_DayEnd made WHERE made.Doc_Type='I' AND cast(made.[DateTime] as date) BETWEEN '" + fromdate + "' AND '" + todate + "' and zone='" + Zone + "' and branch='" + branch + "'";
            Cl_Variables clvar = new Cl_Variables();
            DataSet dt = new DataSet();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.TableMappings.Add("Table", "tblInvoices");
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt.Tables[0];
        }


        public string GenerateManualInvoice(Cl_Variables clvar)
        {
            string invoiceNumber = "";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("spGenerateAutoInvoices_v2", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandTimeout = 2000;

                sqlcmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                sqlcmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZONECODE"].ToString());
                sqlcmd.Parameters.AddWithValue("@DateFrom", clvar.FromDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@DateTo", clvar.ToDate.ToString("yyyy-MM-dd"));
                sqlcmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["U_ID"].ToString());
                sqlcmd.Parameters.AddWithValue("@ClientId", clvar.CreditClientID);
                sqlcmd.Parameters.AddWithValue("@CompanyId", clvar.Company);
                sqlcmd.Parameters.AddWithValue("@Autocheck", "0");


                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                sqlcon.Close();

                //sqlcmd.ExecuteNonQuery();
                //sqlcon.Close();

                //string id = Convert.ToString(parm.Value);
                if (ds.Tables[3].Rows.Count != 0)
                {
                    invoiceNumber = ds.Tables[3].Rows[0][0].ToString(); ;

                }
            }
            catch (Exception ex)
            { return ex.Message; }
            finally { sqlcon.Close(); }

            return invoiceNumber;
        }



        protected void rbtn_formMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void rbtn_formMode_SelectedIndexChanged1(object sender, EventArgs e)
        {
            btn_reset_Click(sender, e);
        }
        protected void btn_print_Click(object sender, EventArgs e)
        {
            string invoiceNo = txt_invoiceNo.Text;
            if (invoiceNo == "")
            {
                return;
            }
            CreateInvoiceSummaryReport(invoiceNo, this);

            //string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            //string script = String.Format(script_, "InvoiceSummary.aspx?XCode=" + invoiceNo, "_blank", "");
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
        protected void txt_invoiceNo_TextChanged(object sender, EventArgs e)
        {
            string invoiceNo = txt_invoiceNo.Text;
            clvar.RefNumber = invoiceNo;
            DataTable dt = func.GetConsignmentsInInvoice(clvar);
            DataTable header = func.GetInvoiceHeader(clvar);

            if (header.Rows.Count > 0)
            {
                txt_clientAccNo.Text = header.Rows[0]["AccountNo"].ToString();
                txt_clientName.Text = header.Rows[0]["name"].ToString();
                dd_company.SelectedValue = header.Rows[0]["CompanyID"].ToString();
                pickerEndDate.SelectedDate = DateTime.Parse(header.Rows[0]["EndDate"].ToString());
                pickerStart.SelectedDate = DateTime.Parse(header.Rows[0]["startDate"].ToString());
                pickerInvoiceDate.SelectedDate = DateTime.Parse(header.Rows[0]["invoiceDate"].ToString());
            }
            if (dt.Rows.Count > 0)
            {
                gv_cns.DataSource = dt;
                gv_cns.DataBind();
            }
            else
            {
                gv_cns.DataSource = null;
                gv_cns.DataBind();
                Errorid.Text = "No Record Found";
                Errorid.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void CreateInvoiceSummaryReport(string invoiceNo, object sender)
        {
            //invoiceNo = invoiceNo;

        }
        protected void lk_summary_Click(object sender, EventArgs e)
        {
            //objRpt.FileName = Server.MapPath("OCSInvoiceSummaryReportNF.rpt");




        }
        protected void lk_detail_Click(object sender, EventArgs e)
        {


        }

        public DataTable GetConsignmentsForInvoice(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sqlString = "select substring(CAST(CONVERT(date, c.bookingdate, 105) as varchar),0,10) BookingDate,\n" +
            "\t   c.consignmentNumber,\n" +
            "\t   c.pieces,\n" +
            "\t   c.serviceTypeName,\n" +
            "\t   c.destination dCode,\n" +
            "\t   b.name Destination,\n" +
            "\t   c.weight,\n" +
            "\t   c.totalAmount, c.isPriceComputed,\n" +
            "\t   case when ic.consignmentNumber is null\n" +
            "\t\t\tthen\n" +
            "\t\t\t\t'0'\n" +
            "\t\t\telse '1'\n" +
            "\t\t\tend isInvoiced\n" +
            "\n" +
            "\t   from Consignment c\n" +
            "\n" +
            "\t   inner join Branches b\n" +
            "\t   on c.destination = b.branchCode\n" +
            "\n" +
            "\t   inner join ServiceTypes st\n" +
            "\t   on st.serviceTypeName = c.serviceTypeName\n" +
            "\n" +
            "\t   left outer join ConsignmentModifier cm\n" +
            "\t   on cm.consignmentNumber = c.consignmentNumber\n" +
            "\n" +
            "     left outer join InvoiceConsignment ic\n" +
            "     on cast(ic.consignmentAmount as varchar) = c.consignmentNumber\n" +
            "\n" +
            "     where c.creditClientId = '" + clvar.CustomerClientID + "'\n" +
            "     and st.companyId = '" + clvar.Company + "'\n" +
            "     and c.bookingDate between '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "     order by 2";



            sqlString = "select cc.id,\n" +
            "       z.name Zone,\n" +
            "       bb.name Branch,\n" +
            "       cc.accountNo,\n" +
            "       cc.name 'Client Name',\n" +
            "       cast(c.bookingDate AS DATE) bookingDate,\n" +
            "       c.consignmentNumber ConsignmentNumber,\n" +
            "       c.pieces pieces,\n" +
            "       case\n" +
            "         when c.consignmentTypeId = '13' then\n" +
            "          'Hand Carry'\n" +
            "         else\n" +
            "          c.serviceTypeName\n" +
            "       end as ServiceTypeName,\n" +
            "       b.name Destination,\n" +
            "       c.weight Weight,\n" +
            "       c.totalAmount totalAmount,\n" +
            "       case\n" +
            "         When c.serviceTypeName in ('Road n Rail',\n" +
            "                                    'Flyer',\n" +
            "                                    'NTS',\n" +
            "                                    'HEC',\n" +
            "                                    'Bank to Bank',\n" +
            "                                    'Bulk Shipment',\n" +
            "                                    'overnight',\n" +
            "                                    'Return Service',\n" +
            "                                    'Same Day',\n" +
            "                                    'Second Day',\n" +
            "                                    'Smart Box',\n" +
            "                                    'Sunday & Holiday',\n" +
            "                                    'Smart Box',\n" +
            "                                    'Hand Carry',\n" +
            "                                    'Smart Cargo',\n" +
            "                                    'MB10',\n" +
            "                                    'MB2',\n" +
            "                                    'MB20',\n" +
            "                                    'MB30',\n" +
            "                                    'MB5',\n" +
            "                                    'Aviation Sale') then\n" +
            "          'Domestic'\n" +
            "         When c.serviceTypeName in ('Expressions',\n" +
            "                                    'International Expressions',\n" +
            "                                    'Mango',\n" +
            "                                    'Mango Petty') then\n" +
            "          'Expression'\n" +
            "         When c.serviceTypeName in ('International 5 Percent Discount Tariff Non Doc',\n" +
            "                                    'International 10 Percent Discount Tariff Non Doc',\n" +
            "                                    'International 15 Percent Discount tariff Non Doc',\n" +
            "                                    'International 20 Percent Discount tariff Non Doc',\n" +
            "                                    'International 25 Percent Discount Tariff Non Doc',\n" +
            "                                    'International Cargo',\n" +
            "                                    'International_Box',\n" +
            "                                    'International_Non-Doc',\n" +
            "                                    'International_Non-Doc_Special_Hub_2014',\n" +
            "                                    'Logex') then\n" +
            "          'SAMPLE'\n" +
            "         When c.serviceTypeName in\n" +
            "              ('International Discount Tariff 5 Percent',\n" +
            "               'International 10 Percent Discount tariff',\n" +
            "               'International 15 percent Discount tariff',\n" +
            "               'International 20 Percent Discount tariff',\n" +
            "               'International 25 Percent Discount tariff',\n" +
            "               'International Special Rates from KHI',\n" +
            "               'International Special Rates from Up Country',\n" +
            "               'International Student Package Tariff',\n" +
            "               'International_Doc',\n" +
            "               'International_Doc_Special_Hub') then\n" +
            "          'DOCUMENT'\n" +
            "       END as Product,\n" +
            "       c.isPriceComputed\n" +
            "  from Consignment c\n" +
            " inner join CreditClients cc\n" +
            "    on c.creditClientId = cc.id\n" +
            " inner join Branches bb\n" +
            "    on cc.branchCode = bb.branchCode\n" +
            " INNER JOIN Branches AS b\n" +
            "    ON b.branchCode = c.destination\n" +
            " inner join Zones z\n" +
            "    on bb.zoneCode = z.zoneCode\n" +
            " inner join ServiceTypes st\n" +
            "    on c.serviceTypeName = st.serviceTypeName\n" +
            " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
            "   AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "   and st.companyId = '" + clvar.Company + "'";

            sqlString = "select * from (\n" +
           "select ic.consignmentNumber iConsignmentNumber, cc.id,\n" +
           "       z.name Zone,\n" +
           "       bb.name Branch,\n" +
           "       cc.accountNo,\n" +
           "       cc.name 'Client Name',\n" +
           "       cast(c.bookingDate AS DATE) bookingDate,\n" +
           "       c.consignmentNumber ConsignmentNumber,\n" +
           "       c.pieces pieces,\n" +
           "       case\n" +
           "         when c.consignmentTypeId = '13' then\n" +
           "          'Hand Carry'\n" +
           "         else\n" +
           "          c.serviceTypeName\n" +
           "       end as ServiceTypeName,\n" +
           "       b.name Destination,\n" +
           "       c.weight Weight,\n" +
           "       c.totalAmount totalAmount,\n" +
           "       case\n" +
           "         When c.serviceTypeName in ('Road n Rail',\n" +
           "                                    'Flyer',\n" +
           "                                    'NTS',\n" +
           "                                    'HEC',\n" +
           "                                    'Bank to Bank',\n" +
           "                                    'Bulk Shipment',\n" +
           "                                    'overnight',\n" +
           "                                    'Return Service',\n" +
           "                                    'Same Day',\n" +
           "                                    'Second Day',\n" +
           "                                    'Smart Box',\n" +
           "                                    'Sunday & Holiday',\n" +
           "                                    'Smart Box',\n" +
           "                                    'Hand Carry',\n" +
           "                                    'Smart Cargo',\n" +
           "                                    'MB10',\n" +
           "                                    'MB2',\n" +
           "                                    'MB20',\n" +
           "                                    'MB30',\n" +
           "                                    'MB5',\n" +
           "                                    'Aviation Sale') then\n" +
           "          'Domestic'\n" +
           "         When c.serviceTypeName in ('Expressions',\n" +
           "                                    'International Expressions',\n" +
           "                                    'Mango',\n" +
           "                                    'Mango Petty') then\n" +
           "          'Expression'\n" +
           "         When c.serviceTypeName in ('International 5 Percent Discount Tariff Non Doc',\n" +
           "                                    'International 10 Percent Discount Tariff Non Doc',\n" +
           "                                    'International 15 Percent Discount tariff Non Doc',\n" +
           "                                    'International 20 Percent Discount tariff Non Doc',\n" +
           "                                    'International 25 Percent Discount Tariff Non Doc',\n" +
           "                                    'International Cargo',\n" +
           "                                    'International_Box',\n" +
           "                                    'International_Non-Doc',\n" +
           "                                    'International_Non-Doc_Special_Hub_2014',\n" +
           "                                    'Logex') then\n" +
           "          'SAMPLE'\n" +
           "         When c.serviceTypeName in\n" +
           "              ('International Discount Tariff 5 Percent',\n" +
           "               'International 10 Percent Discount tariff',\n" +
           "               'International 15 percent Discount tariff',\n" +
           "               'International 20 Percent Discount tariff',\n" +
           "               'International 25 Percent Discount tariff',\n" +
           "               'International Special Rates from KHI',\n" +
           "               'International Special Rates from Up Country',\n" +
           "               'International Student Package Tariff',\n" +
           "               'International_Doc',\n" +
           "               'International_Doc_Special_Hub') then\n" +
           "          'DOCUMENT'\n" +
           "       END as Product,\n" +
           "       c.isPriceComputed\n" +
           "  from Consignment c\n" +
           " inner join CreditClients cc\n" +
           "    on c.creditClientId = cc.id\n" +
           " inner join Branches bb\n" +
           "    on cc.branchCode = bb.branchCode\n" +
           " INNER JOIN Branches AS b\n" +
           "    ON b.branchCode = c.destination\n" +
           " inner join Zones z\n" +
           "    on bb.zoneCode = z.zoneCode\n" +
           " inner join ServiceTypes st\n" +
           "    on c.serviceTypeName = st.serviceTypeName\n" +
           " left outer join InvoiceConsignment ic\n" +
           " on ic.consignmentNumber = c.consignmentNumber\n" +
           " where cc.accountNo = '" + clvar.AccountNo + "'\n" +
           "   AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
           "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
           "   and st.companyId = '" + clvar.Company + "'\n" +
           "   ) temp where temp.iConsignmentNumber is null";


            string sql = " \n"
               + "SELECT top(100) * \n"
               + "FROM   ( \n"
               + "          SELECT   -- ic.consignmentNumber     iConsignmentNumber, \n"
               + "           cc.id, \n"
               + "           z.name Zone, \n"
               + "           bb.name Branch, \n"
               + "           cc.accountNo, \n"
               + "           cc.name 'Client Name', \n"
               + "           CAST(c.bookingDate AS DATE) bookingDate, \n"
               + "           c.consignmentNumber ConsignmentNumber, \n"
               + "           c.pieces pieces, \n"
               + "           CASE  \n"
               + "                WHEN c.consignmentTypeId = '13' THEN 'Hand Carry' \n"
               + "                ELSE c.serviceTypeName \n"
               + "           END AS ServiceTypeName, \n"
               + "           b.name Destination, \n"
               + "           c.weight WEIGHT, \n"
               + "           c.totalAmount totalAmount, \n"
               + "           CASE  \n"
               + "                WHEN c.serviceTypeName IN ('Road n Rail', 'Flyer', 'NTS', 'HEC',  \n"
               + "                                          'Bank to Bank', 'Bulk Shipment',  \n"
               + "                                          'overnight', 'Return Service',  \n"
               + "                                          'Same Day', 'Second Day', 'Smart Box',  \n"
               + "                                          'Sunday & Holiday', 'Smart Box',  \n"
               + "                                          'Hand Carry', 'Smart Cargo', 'MB10',  \n"
               + "                                          'MB2', 'MB20', 'MB30', 'MB5',  \n"
               + "                                          'Aviation Sale') THEN 'Domestic' \n"
               + "                WHEN c.serviceTypeName IN ('Expressions',  \n"
               + "                                          'International Expressions', 'Mango',  \n"
               + "                                          'Mango Petty') THEN 'Expression' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International 5 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 10 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 15 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 20 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 25 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International Cargo',  \n"
               + "                                          'International_Box',  \n"
               + "                                          'International_Non-Doc',  \n"
               + "                                          'International_Non-Doc_Special_Hub_2014',  \n"
               + "                                          'Logex') THEN 'SAMPLE' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International Discount Tariff 5 Percent',  \n"
               + "                                          'International 10 Percent Discount tariff',  \n"
               + "                                          'International 15 percent Discount tariff',  \n"
               + "                                          'International 20 Percent Discount tariff',  \n"
               + "                                          'International 25 Percent Discount tariff',  \n"
               + "                                          'International Special Rates from KHI',  \n"
               + "                                          'International Special Rates from Up Country',  \n"
               + "                                          'International Student Package Tariff',  \n"
               + "                                          'International_Doc',  \n"
               + "                                          'International_Doc_Special_Hub') THEN  \n"
               + "                     'DOCUMENT' \n"
               + "           END AS Product, \n"
               + "           c.isPriceComputed \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN CreditClients cc \n"
               + "           ON c.creditClientId = cc.id \n"
               + "           INNER JOIN Branches bb \n"
               + "           ON cc.branchCode = bb.branchCode \n"
               + "           INNER JOIN Branches AS b \n"
               + "           ON b.branchCode = c.destination \n"
               + "           INNER JOIN Zones z \n"
               + "           ON bb.zoneCode = z.zoneCode \n"
               + "           INNER JOIN ServiceTypes st \n"
               + "           ON c.serviceTypeName = st.serviceTypeName \n"
               + "            \n"
               + "           WHERE cc.accountNo = '" + clvar.AccountNo + "' \n"
               + "           AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n"
               + "           AND c.status != '9' and c.isapproved = '1' and isinvoiced ='0' \n"
               + "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n"
               + "   and st.companyId = '" + clvar.Company + "'\n"
               + "       ) temp \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  ic.consignmentNumber = temp.consignmentNumber \n"
               + "       LEFT OUTER JOIN Invoice i \n"
               + "            ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "            AND isnull(i.IsInvoiceCanceled,'0') = '0'";


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
            return dt;
        }

        public DataTable GetConsignmentsForInvoice_count(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sql = " \n"
               + "SELECT count(*) count \n"
               + "FROM   ( \n"
               + "          SELECT   -- ic.consignmentNumber     iConsignmentNumber, \n"
               + "           cc.id, \n"
               + "           z.name Zone, \n"
               + "           bb.name Branch, \n"
               + "           cc.accountNo, \n"
               + "           cc.name 'Client Name', \n"
               + "           CAST(c.bookingDate AS DATE) bookingDate, \n"
               + "           c.consignmentNumber ConsignmentNumber, \n"
               + "           c.pieces pieces, \n"
               + "           CASE  \n"
               + "                WHEN c.consignmentTypeId = '13' THEN 'Hand Carry' \n"
               + "                ELSE c.serviceTypeName \n"
               + "           END AS ServiceTypeName, \n"
               + "           b.name Destination, \n"
               + "           c.weight WEIGHT, \n"
               + "           c.totalAmount totalAmount, \n"
               + "           CASE  \n"
               + "                WHEN c.serviceTypeName IN ('Road n Rail', 'Flyer', 'NTS', 'HEC',  \n"
               + "                                          'Bank to Bank', 'Bulk Shipment',  \n"
               + "                                          'overnight', 'Return Service',  \n"
               + "                                          'Same Day', 'Second Day', 'Smart Box',  \n"
               + "                                          'Sunday & Holiday', 'Smart Box',  \n"
               + "                                          'Hand Carry', 'Smart Cargo', 'MB10',  \n"
               + "                                          'MB2', 'MB20', 'MB30', 'MB5',  \n"
               + "                                          'Aviation Sale') THEN 'Domestic' \n"
               + "                WHEN c.serviceTypeName IN ('Expressions',  \n"
               + "                                          'International Expressions', 'Mango',  \n"
               + "                                          'Mango Petty') THEN 'Expression' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International 5 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 10 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International 15 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 20 Percent Discount tariff Non Doc',  \n"
               + "                                          'International 25 Percent Discount Tariff Non Doc',  \n"
               + "                                          'International Cargo',  \n"
               + "                                          'International_Box',  \n"
               + "                                          'International_Non-Doc',  \n"
               + "                                          'International_Non-Doc_Special_Hub_2014',  \n"
               + "                                          'Logex') THEN 'SAMPLE' \n"
               + "                WHEN c.serviceTypeName IN ( \n"
               + "                                          'International Discount Tariff 5 Percent',  \n"
               + "                                          'International 10 Percent Discount tariff',  \n"
               + "                                          'International 15 percent Discount tariff',  \n"
               + "                                          'International 20 Percent Discount tariff',  \n"
               + "                                          'International 25 Percent Discount tariff',  \n"
               + "                                          'International Special Rates from KHI',  \n"
               + "                                          'International Special Rates from Up Country',  \n"
               + "                                          'International Student Package Tariff',  \n"
               + "                                          'International_Doc',  \n"
               + "                                          'International_Doc_Special_Hub') THEN  \n"
               + "                     'DOCUMENT' \n"
               + "           END AS Product, \n"
               + "           c.isPriceComputed \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN CreditClients cc \n"
               + "           ON c.creditClientId = cc.id \n"
               + "           INNER JOIN Branches bb \n"
               + "           ON cc.branchCode = bb.branchCode \n"
               + "           INNER JOIN Branches AS b \n"
               + "           ON b.branchCode = c.destination \n"
               + "           INNER JOIN Zones z \n"
               + "           ON bb.zoneCode = z.zoneCode \n"
               + "           INNER JOIN ServiceTypes st \n"
               + "           ON c.serviceTypeName = st.serviceTypeName \n"
               + "            \n"
               + "           WHERE cc.accountNo = '" + clvar.AccountNo + "' \n"
               + "           AND cc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n"
               + "           AND c.status != '9' and c.isapproved = '1' and isPriceComputed ='1' and isinvoiced ='0' \n"
               + "   and cast(c.bookingDate AS DATE) BETWEEN '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n"
               + "   and st.companyId = '" + clvar.Company + "'\n"
               + "       ) temp \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  ic.consignmentNumber = temp.consignmentNumber \n"
               + "       LEFT OUTER JOIN Invoice i \n"
               + "            ON  ic.invoiceNumber = i.invoiceNumber \n"
               + "            AND isnull(i.IsInvoiceCanceled,'0') = '0'";


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
            return dt;
        }

    }
}