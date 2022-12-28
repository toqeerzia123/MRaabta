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
    public partial class InvoiceCancellation_Bulk : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        cl_InvoiceCancelation cli = new cl_InvoiceCancelation();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Years();
                GetZones();
                GetBranches();
                Get_InvoieCancelationReasons();
            }
        }
        protected void Years()
        {
            DataTable dt = GetYears();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_year.DataSource = dt;
                    dd_year.DataTextField = "YEAR";
                    dd_year.DataValueField = "YEAR";
                    dd_year.DataBind();
                    dd_year.Items.Add(new ListItem { Text = "2017", Value = "2017" });
                }
            }

        }
        protected void GetZones()
        {
            DataTable dt = new DataTable();
            dt = func.GetZonesForDomesticTariff();
            DataView dv = dt.AsDataView();
            dv.Sort = "Name";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_zone.DataSource = dv;
                    dd_zone.DataTextField = "Name";
                    dd_zone.DataValueField = "ZoneCode";
                    dd_zone.DataBind();
                }
            }
        }
        protected void GetBranches()
        {
            dd_branch.Items.Clear();
            clvar.Zone = dd_zone.SelectedValue;
            DataTable dt = GetBranchesByZoneCode(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_branch.Items.Add(new ListItem { Text = "SELECT BRANCH", Value = "0" });
                    dd_branch.DataSource = dt;
                    dd_branch.DataTextField = "Name";
                    dd_branch.DataValueField = "BranchCode";
                    dd_branch.DataBind();

                }
                else
                {
                    dd_branch.Items.Add(new ListItem { Text = "NO BRANCHES AVAILABLE", Value = "0" });
                }
            }
        }

        public void Get_InvoieCancelationReasons()
        {
            DataTable dt = cli.InvoiceCancelation_Reasons(clvar);
            if (dt.Rows.Count != 0)
            {
                ViewState["CancelReasons"] = dt;
            }

        }


        protected void txt_accountNo_TextChanged(object sender, EventArgs e)
        {
            if (txt_accountNo.Text.Trim() != "")
            {
                clvar.Branch = dd_branch.SelectedValue;
                clvar.AccountNo = txt_accountNo.Text;
                DataTable dt = func.GetAccountDetailByAccountNumber(clvar);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        hd_creditClientID.Value = dt.Rows[0]["id"].ToString();
                        lbl_accountName.Text = dt.Rows[0]["Name"].ToString();
                        btn_GetInvoices_Click(this, e);
                    }
                    else
                    {
                        AlertMessage("Invalid Account Number");
                        txt_accountNo.Text = "";
                        lbl_accountName.Text = "";
                        hd_creditClientID.Value = "";
                        gv_invoices.DataSource = null;
                        gv_invoices.DataBind();
                    }
                }
                else
                {
                    AlertMessage("Invalid Account Number");
                    txt_accountNo.Text = "";
                    lbl_accountName.Text = "";
                    hd_creditClientID.Value = "";
                    gv_invoices.DataSource = null;
                    gv_invoices.DataBind();
                }
            }
        }
        protected void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }

        protected void btn_GetInvoices_Click(object sender, EventArgs e)
        {
            string month = dd_month.SelectedValue;
            string year = dd_year.SelectedValue;
            clvar.CreditClientID = hd_creditClientID.Value;
            if (dd_year.SelectedValue == "0")
            {
                AlertMessage("Select Year");
            }
            if (dd_zone.SelectedValue == "0")
            {
                clvar.CheckCondition = "";
            }
            else
            {
                if (dd_branch.SelectedValue == "0")
                {
                    clvar.CheckCondition = " and cc.zoneCode = '" + dd_zone.SelectedValue + "'\n";
                }
                else
                {
                    clvar.CheckCondition = " and cc.zoneCode = '" + dd_zone.SelectedValue + "'\n" +
                                           " and cc.BranchCode = '" + dd_branch.SelectedValue + "'\n";
                }
            }
            if (txt_accountNo.Text.Trim() != "")
            {
                clvar.CheckCondition += " and cc.id = '" + hd_creditClientID.Value + "'";
            }
            DataTable dt = GetInvoicesByAccountNumber(clvar, year, month);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv_invoices.DataSource = dt;
                    gv_invoices.DataBind();
                    DataTable cancelReasons = ViewState["CancelReasons"] as DataTable;
                    foreach (GridViewRow row in gv_invoices.Rows)
                    {
                        DropDownList dd_gReason = row.FindControl("dd_gReason") as DropDownList;
                        dd_gReason.DataSource = cancelReasons;
                        dd_gReason.DataTextField = "Reason";
                        dd_gReason.DataValueField = "id";
                        dd_gReason.DataBind();
                    }
                }
                else
                {
                    AlertMessage("No Invoices Found");
                    gv_invoices.DataSource = null;
                    gv_invoices.DataBind();
                }
            }
            else
            {
                AlertMessage("No Invoices Found");
                gv_invoices.DataSource = null;
                gv_invoices.DataBind();
            }
        }
        protected void dd_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetBranches();
        }
        protected void dd_branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_accountNo.Text.Trim() != "")
            {
                txt_accountNo_TextChanged(sender, e);
            }
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            if (txt_accountNo.Text.Trim() == "")
            {

            }




            if (gv_invoices.Rows.Count > 0)
            {
                List<List<string>> invoices = new List<List<string>>();
                foreach (GridViewRow row in gv_invoices.Rows)
                {
                    CheckBox chk = row.FindControl("chk_cancel") as CheckBox;
                    DropDownList reason = row.FindControl("dd_gReason") as DropDownList;
                    if (chk.Checked)
                    {
                        if (reason.SelectedValue == "0")
                        {
                            row.BackColor = System.Drawing.Color.FromName("#FFCCD7"); ;
                            AlertMessage("Select Reason");
                            return;
                        }
                        else
                        {
                            row.BackColor = System.Drawing.Color.FromName("#FFFFFF");
                        }

                        string bilal = row.Cells[3].Text;

                        DateTime GVDate = Convert.ToDateTime(bilal);
                        DateTime formattedDate = DateTime.Parse(GVDate.ToString("yyyy-MM-dd"));

                        DataTable dates = MinimumDate(clvar);
                        DateTime minAllowedDate = DateTime.Parse(dates.Rows[0]["DateAllowed"].ToString()).AddDays(1);
                        DateTime maxAllowedDate = DateTime.Now;
                        if (formattedDate < minAllowedDate || formattedDate > maxAllowedDate)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End has already been performed for selected Dates. Cannot Cancel Invoice')", true);
                            return;
                        }

                        List<string> temp = new List<string>();
                        temp.Add(row.Cells[1].Text);
                        temp.Add(reason.SelectedValue);
                        invoices.Add(temp);
                    }

                }
                if (invoices.Count > 0)
                {
                    string error = BulkCancelInvoices(invoices);
                    if (error == "OK")
                    {
                        AlertMessage("Selected invoices Canceled Successfully");
                        btn_GetInvoices_Click(sender, e);
                    }
                    else
                    {
                        AlertMessage(error);
                    }
                }

            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
        protected void ResetAll()
        {
            txt_accountNo.Text = "";
            dd_zone.ClearSelection();
            GetBranches();
            lbl_accountName.Text = "";

            gv_invoices.DataSource = null;
            gv_invoices.DataBind();
        }

        public string BulkCancelInvoices(List<List<string>> invoices)
        {
            string error = "";
            string invoiceNumbers = "";
            foreach (List<string> str in invoices)
            {
                invoiceNumbers += "'" + str[0] + "'";
            }
            invoiceNumbers = invoiceNumbers.Replace("''", "','");


            string updateConsignments = "update consignment\n" +
            "   set isinvoiced = '0'\n" +
            " where consignmentNumber in\n" +
            "       (select ic.consignmentNumber\n" +
            "          from InvoiceConsignment ic\n" +
            "         where ic.invoiceNumber in (" + invoiceNumbers + "))";

            string updateInvoices = "UPDATE Invoice set isInvoiceCanceled = '1' where invoiceNumber in (" + invoiceNumbers + ")";

            string insertInvoiceCancelation = "INSERT INTO MNP_InvoiceCancelation \n"
                   + " ( \n"
                   + " 	-- id -- this column value is auto-generated \n"
                   + " 	InvocieNo, \n"
                   + " 	Cancelation_Reason, \n"
                   + " 	Created_date, \n"
                   + " 	Created_by \n"
                   + " ) \n";
            for (int i = 0; i < invoices.Count - 1; i++)
            {
                insertInvoiceCancelation += " SELECT '" + invoices[i][0] + "', '" + invoices[i][1] + "', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
                    " UNION ALL \n";
            }
            insertInvoiceCancelation += " SELECT '" + invoices[invoices.Count - 1][0] + "', '" + invoices[invoices.Count - 1][1] + "', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            con.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = con;
            trans = con.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = updateInvoices;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = updateConsignments;
                sqlcmd.ExecuteNonQuery();

                sqlcmd.CommandText = insertInvoiceCancelation;
                sqlcmd.ExecuteNonQuery();

                trans.Commit();
                error = "OK";
            }
            catch (Exception ex)
            {
                trans.Rollback();
                error = ex.Message;
            }
            finally { con.Close(); }

            return error;
        }

        protected void gv_invoices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string redeemNumber = (e.Row.FindControl("hd_redeemNumber") as HiddenField).Value;
                if (redeemNumber != "")
                {
                    (e.Row.FindControl("chk_cancel") as CheckBox).Enabled = false;
                    e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    e.Row.ToolTip = "Redeemed Invoice";
                }
            }
        }


        public DataTable GetYears()
        {
            string query = "select distinct YEAR from Calendar";
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
        public DataTable GetBranchesByZoneCode(Cl_Variables clvar)
        {
            string query = "select * from branches where zonecode = '" + clvar.Zone + "' order by name";
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
        public DataTable GetInvoicesByAccountNumber(Cl_Variables clvar, string year, string month)
        {

            string sqlString = "selecT i.companyId,cc.branchCode, cc.accountNo, cc.name, i.startDate, i.endDate, i.invoiceDate,isnull(cpa.calculationBase,0) calculationBase, isnull(cpa.modifiedCalculationValue,0) modifiedCalculationValue, isnull(cc.DIscountOnDomestic,0) DIscountOnDomestic,isnull(cc.discountOnSample,0) discountOnSample, isnull(cc.discountOnDocument,0) discountOnDocument  \n"
                + "  from Invoice i \n"
                + " inner join CreditClients cc \n"
                + "    on cc.id = i.clientId \n"
                + " Left OUTER JOIN ClientPriceModifierAssociation Cpa \n"
                + " ON cpa.creditClientId = cc.id"
                + " where i.invoiceNumber = '" + clvar.RefNumber + "'";

            sqlString = "select distinct i.invoiceNumber,\n" +
           "       i.companyId,\n" +
           "       CAST (i.startDate as Varchar) startDate,\n" +
           "       CAST (i.endDate as Varchar) endDate,\n" +
           "       CAST (i.invoiceDate as Varchar) invoiceDate,\n" +
           "       i.totalAmount, cc.accountNo, cc.name accountName, ir.invoiceNo irnumber, b.name BranchName\n" +
           "  from Invoice i\n" +
           " inner join CreditCLients cc\n" +
           "    on cc.id = i.clientId \n" +
           "  left outer join InvoiceRedeem ir\n" +
           "    on ir.invoiceNo = i.invoiceNumber\n" +
           "  left outer join branches b\n" +
           "    on b.branchCode = cc.branchCode\n" +
           " where YEAR(i.invoiceDate) = '" + year + "' and month(i.invoiceDate) = '" + month + "'\n" +
           "   and i.isinvoiceCanceled = '0' and i.invoiceNumber not in (select invocieNo from mnp_invoiceCancelation)\n" +
           "   " + clvar.CheckCondition + "\n" +

           " order by invoiceNumber";

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

        protected DataTable MinimumDate(Cl_Variables clvar)
        {
            string sqlString = "SELECT CASE\n" +
            "         WHEN ZU.PROFILE IN ('2', '5', '9', '12', '38', '113','126') AND\n" +
            "              MAX(A.OPSDATEALLOWED) > MAX(A.ACCDATEALLOWED) THEN\n" +
            "          MAX(A.OPSDATEALLOWED)\n" +
            "         WHEN ZU.PROFILE IN ('2', '5', '9', '12', '38', '113','126') AND\n" +
            "              MAX(A.OPSDATEALLOWED) <= MAX(A.ACCDATEALLOWED) THEN\n" +
            "          MAX(A.ACCDATEALLOWED)\n" +
            "         WHEN ZU.PROFILE IN\n" +
            "              ('6', '16', '33', '37', '39', '44', '53', '52', '108','126') THEN\n" +
            "          MAX(A.ACCDATEALLOWED)\n" +
            "       END DateAllowed\n" +
            "  FROM (SELECT MAX(D.DATETIME) ACCDATEALLOWED, 0 OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '4' \n" + //'" + clvar.Branch + "'\n" +
            "           AND D.DOC_TYPE = 'A'\n" +
            "        UNION ALL\n" +
            "        SELECT 0 ACCDATEALLOWED, MAX(D.DATETIME) OPSDATEALLOWED\n" +
            "          FROM MNP_ACCOUNT_DAYEND D\n" +
            "         WHERE D.BRANCH = '4' \n" + //'" + clvar.Branch + "'\n" +
            "           AND D.DOC_TYPE = 'O') A\n" +
            " INNER JOIN ZNI_USER1 ZU\n" +
            "    ON ZU.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n" +
            " GROUP BY ZU.PROFILE";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
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