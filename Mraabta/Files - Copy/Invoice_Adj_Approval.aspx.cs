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
    public partial class Invoice_Adj_Approval : System.Web.UI.Page
    {
        Cl_Invocie cl_inv = new Cl_Invocie();
        Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                company();
            }
        }

        public void company()
        {
            DataTable ds = cl_inv.Company(clvar);
            if (ds.Rows.Count > 0)
            {
                dd_company.DataTextField = "CompanyName";
                dd_company.DataValueField = "id";
                dd_company.DataSource = ds.DefaultView;
                dd_company.DataBind();
            }
        }

        protected void btn_getConsignment_Click(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            clvar.Company = dd_company.SelectedValue;
            clvar.FromDate = pickerStart.SelectedDate.Value;
            clvar.ToDate = pickerEndDate.SelectedDate.Value;

            DataTable dt_inv = InvoiceAddApproval(clvar);
            ViewState["INv_Det"] = dt_inv;
            if (dt_inv.Rows.Count != 0)
            {
                gv_cns.DataSource = dt_inv.DefaultView;
                gv_cns.DataBind();
            }
        }

        public DataTable InvoiceAddApproval(Cl_Variables clvar)
        {
            string sql = " \n"
               + "SELECT in1.invoiceNumber, \n"
               + "       in1.companyId, \n"
               + "       ( \n"
               + "           SELECT companyName \n"
               + "           FROM   company \n"
               + "           WHERE  id = in1.companyId \n"
               + "       )             companyName, \n"
               + "       ( \n"
               + "           SELECT cc.name \n"
               + "           FROM   CreditClients cc \n"
               + "           WHERE  cc.accountNo = icn.Account_No \n"
               + "       )             ClientName, \n"
               + "       icn.Account_No AccountNo, \n"
               + "       z.name        Zonename, \n"
               + "       b.name        OriginName, \n"
               + "       in1.TotalAmount, \n"
               + "       in1.Gst, \n"
               + "       zu.U_NAME     CreatedBy, \n"
               + "       in1.Note_type, \n"
               + "       in1.Approved, \n"
               + "       in1.Note_Number, \n"
               + "       IN1.Discount, \n"
               + "       in1.Fuel, \n"
               + "       in1.Fuelgst \n"
               + "FROM   Invoice_Note in1 \n"
               + "       INNER JOIN InvoiceConsignment_Note icn \n"
               + "            ON  icn.Note_number = in1.Note_Number \n"
               + "                --INNER JOIN CreditClients cc \n"
               + "                --     ON  in1.clientId = cc.id \n"
               + "                --     AND icn.Account_No = cc.accountNo \n"
               + "                 \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.branchCode = icn.Origin \n"
               + "       INNER JOIN ZNI_USER1 zu \n"
               + "            ON  icn.createdBy = zu.U_ID \n"
               + "       INNER JOIN Zones z \n"
               + "            ON  b.zoneCode = z.zoneCode \n"
               + "WHERE  CAST(icn.createdOn AS date) >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' \n"
               + "       AND CAST(icn.createdOn AS date) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "' \n"
               + "       and in1.companyId ='" + clvar.Company + "'\n"
               + "      AND in1.Approved = '0' \n"
               + "GROUP BY \n"
               + "       in1.invoiceNumber, \n"
               + "       in1.companyId, \n"
               + "       icn.Account_No, \n"
               + "       z.name, \n"
               + "       b.name, \n"
               + "       in1.TotalAmount, \n"
               + "       in1.Gst, \n"
               + "       zu.U_NAME, \n"
               + "       in1.Note_type, \n"
               + "       in1.Approved, \n"
               + "       in1.Note_Number, \n"
               + "       IN1.Discount, \n"
               + "       in1.Fuel, \n"
               + "       in1.Fuelgst \n"
               + "       ";

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
            return dt;

        }

        public void Gen()
        {
            DataTable dt = (DataTable)ViewState["INv_Det"];
            if (dt.Rows.Count.ToString() != "0")
            {
                gv_cns.DataSource = dt.DefaultView;
                gv_cns.DataBind();
            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {

        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            clvar = new Cl_Variables();
            Cl_Invocie cl_1 = new Cl_Invocie();

            clvar.Company = dd_company.SelectedValue;
            clvar.FromDate = pickerStart.SelectedDate.Value;
            clvar.ToDate = pickerEndDate.SelectedDate.Value;

            foreach (GridViewRow gr in gv_cns.Rows)
            {
                if (((CheckBox)gr.FindControl("cb_Approve")).Checked == true)
                {
                    if ((gr.FindControl("hd_Note_type") as HiddenField).Value == "C")
                    {
                        string Note_Number = (gr.FindControl("hd_Note_Number") as HiddenField).Value;
                        string Invoice = (gr.FindControl("hd_invoiceNumber") as HiddenField).Value;
                        string Note_Type = (gr.FindControl("hd_Note_type") as HiddenField).Value;

                        //Updating Invoice Note

                        clvar = new Cl_Variables();
                        clvar.NoteNumber = Note_Number;
                        clvar.NoteType = Note_Type;
                        clvar.InvoiceNo = Invoice;

                        // adding in General Voucher

                        string flag = cl_inv.CreditNOteInsert_GV(clvar);
                        if (flag == "0")
                        {
                            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                            DataTable dt_1 = (DataTable)ViewState["INv_Det"];
                            string script = String.Format(script_, "creditnote_report.aspx?Xcode=" + Note_Number, "_blank", "");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                            script = String.Format(script_, "CN_Detail_Adjustment.aspx?Xcode=" + Note_Number, "_blank", "");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                            string consignmnet = Note_Number;//(row.FindControl("hd_Consignment") as HiddenField).Value;
                            DataRow[] dr = dt_1.Select("Note_Number=" + consignmnet);
                            if (dr.Length != 0)
                            {
                                dt_1.Rows.Remove(dr[0]);
                                dt_1.AcceptChanges();
                            }
                            ViewState["INv_Det"] = dt_1;

                            Gen();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Note has been saved ')", true);

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Note cannot be saved ')", true);

                        }

                    }
                    else
                    {
                        string Note_Number = (gr.FindControl("hd_Note_Number") as HiddenField).Value;
                        string Invoice = (gr.FindControl("hd_invoiceNumber") as HiddenField).Value;
                        string Note_Type = (gr.FindControl("hd_Note_type") as HiddenField).Value;

                        clvar = new Cl_Variables();
                        clvar.NoteNumber = Note_Number;
                        clvar.NoteType = Note_Type;
                        clvar.InvoiceNo = Invoice;

                        // Now Adding in Invoice

                        string flag = cl_inv.DebitNoteInsert_Invoice(clvar);
                        if (flag == "0")
                        {

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Note has been saved ')", true);

                            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                            DataTable dt_1 = (DataTable)ViewState["INv_Det"];
                            string script = String.Format(script_, "Debitnote_report.aspx?Xcode=" + Note_Number, "_blank", "");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                            script = String.Format(script_, "CN_Detail_Adjustment.aspx?Xcode=" + Note_Number, "_blank", "");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                            string consignmnet = Note_Number;//(row.FindControl("hd_Consignment") as HiddenField).Value;
                            DataRow[] dr = dt_1.Select("Note_Number=" + consignmnet);
                            if (dr.Length != 0)
                            {
                                dt_1.Rows.Remove(dr[0]);
                                dt_1.AcceptChanges();
                            }
                            ViewState["INv_Det"] = dt_1;


                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Note cannot be saved ')", true);
                        }

                    }
                }
            }



        }

        protected void gv_cns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Status = (e.Row.FindControl("hd_approve") as HiddenField).Value;
                if (Status == "False")
                {
                    (e.Row.FindControl("cb_Approve") as CheckBox).Enabled = true;
                }
                else
                {
                    (e.Row.FindControl("cb_Approve") as CheckBox).Enabled = false;
                    (e.Row.FindControl("cb_Approve") as CheckBox).Checked = true;
                }

                string Note_Number = (e.Row.FindControl("hd_Note_Number") as HiddenField).Value;
                string Note_Type = (e.Row.FindControl("hd_Note_type") as HiddenField).Value;
                if (Note_Type == "C")
                {
                    (e.Row.FindControl("lbl_link") as HyperLink).NavigateUrl = "creditnote_report.aspx?Xcode=" + Note_Number;
                    (e.Row.FindControl("lbl_link") as HyperLink).Target = "blank";
                }
                else
                {
                    (e.Row.FindControl("lbl_link") as HyperLink).NavigateUrl = "Debitnote_report.aspx?Xcode=" + Note_Number;
                    (e.Row.FindControl("lbl_link") as HyperLink).Target = "blank";
                }
            }
        }
    }
}