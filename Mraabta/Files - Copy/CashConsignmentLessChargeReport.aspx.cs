using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace MRaabta.Files
{
    public partial class CashConsignmentLessChargeReport : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();

        string date = "";
        string branch = "";
        string zone = "";
        string reportid;
        string start_date;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_Zone();
                Get_Years();
            }
        }

        public void Get_Zone()
        {
            DataSet ds_zone = b_fun.Get_AllZones1(clvar);

            dd_branch.Items.Clear();

            if (ds_zone.Tables[0].Rows.Count != 0)
            {
                dd_zone.DataTextField = "Name";
                dd_zone.DataValueField = "zoneCode";
                dd_zone.DataSource = ds_zone.Tables[0].DefaultView;
                dd_zone.DataBind();
            }
            //     dd_zone.Items.Insert(0, new ListItem("Select Zone", ""));
        }

        public void Get_Years()
        {
            var currentYear = DateTime.Today.Year;
            for (int i = 1; i >= 0; i--)
            {
                dd_year.Items.Add((currentYear - i).ToString());
            }
        }

        protected void branch_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            // clvar._Zone = dd_zone.SelectedValue;

            DataSet ds_branch = b_fun.Get_ZonebyBranches2(clvar);

            dd_branch.Items.Clear();
            if (ds_branch.Tables.Count != 0)
            {
                if (ds_branch.Tables[0].Rows.Count != 0)
                {
                    //  dd_branch.Items.Add(new ListItem("Select Branch Name", "0"));
                    dd_branch.DataTextField = "name";
                    dd_branch.DataValueField = "branchCode";
                    dd_branch.DataSource = ds_branch.Tables[0].DefaultView;
                    dd_branch.DataBind();
                }
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView.PageIndex = e.NewPageIndex;
            Btn_Search_Click(sender, e);
        }

        protected void Get_ReportVersion()
        {
            clvar = new Variable();

            lbl_report_version.Text = "";

            reportid = "9";

            clvar._reportid = reportid;

            DataSet ds = b_fun.Get_Report_VersionByReportId(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                lbl_report_version.Text = "Version: " + ds.Tables[0].Rows[0]["version"].ToString();
            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (type.SelectedValue == "html")
            {
                if (dd_year.Text != "")
                {
                    start_date = " AND year(c.accountReceivingDate) = '" + dd_year.Text + "' ";
                }

                if (dd_Month.Text != "")
                {
                    start_date = " AND month(c.accountReceivingDate) = '" + dd_Month.Text + "' ";
                }

                String BranchID = "", BranchName = "";
                if (branch_chk.Checked)
                {
                    branch = "";
                }
                else
                {

                    for (int i = 0; i < dd_branch.Items.Count; i++)
                    {
                        if (dd_branch.Items[i].Selected)
                        {
                            BranchName += dd_branch.Items[i].Text + ",";
                            BranchID += dd_branch.Items[i].Value + ",";
                        }
                    }
                    if (BranchName != "")
                    {
                        BranchName = BranchName.Remove(BranchName.Length - 1);
                        BranchName.ToString();

                        BranchID = BranchID.Remove(BranchID.Length - 1);
                        BranchID.ToString();
                    }

                    if (dd_branch.SelectedValue != "")
                    {
                        branch = " AND c.orgin IN (" + BranchID.ToString() + ") ";
                    }
                }

                String ZoneID = "", ZoneName = "";
                if (zone_chk.Checked)
                {
                    zone = "";
                }
                else
                {
                    for (int i = 0; i < dd_zone.Items.Count; i++)
                    {
                        if (dd_zone.Items[i].Selected)
                        {
                            ZoneName += dd_zone.Items[i].Text + ",";
                            ZoneID += dd_zone.Items[i].Value + ",";
                        }
                    }

                    if (ZoneName != "")
                    {
                        ZoneName = ZoneName.Remove(ZoneName.Length - 1);
                        ZoneName.ToString();

                        ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                        ZoneID.ToString();
                    }

                    if (dd_zone.SelectedValue != "")
                    {
                        zone = " AND z.zonecode IN (" + ZoneID.ToString() + ") ";
                    }
                }

                clvar._Year = start_date;
                clvar._TownCode = branch;
                clvar._Zone = zone;

                DataSet ds = b_fun.Get_CashConsignmentLessChargeReport(clvar);

                GridView.Visible = true;
                error_msg.Text = "";

                if (ds.Tables[0].Rows.Count != 0)
                {
                    reportid = "9";
                    lbl_report_name.Text = "Cash Consignment Less Charge Report";
                    lbl_total_record.Text = "Total Record: " + ds.Tables[0].Rows.Count.ToString();
                    GridView.DataSource = ds.Tables[0].DefaultView;
                    GridView.DataBind();

                    /*
                    double Total = 0;

                    DataColumn col = ds.Tables[0].Columns["TAMOUNT"];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Total += Double.Parse(row[col].ToString());
                    }

                    GridView.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                    GridView.FooterRow.Cells[8].Font.Bold = true;
                  //  GridView.FooterRow.Cells[7].Text = Total.ToString();
                    GridView.FooterRow.Cells[8].Text = String.Format("{0:N2}", Total);
                    */
                    // Insert Report Track Log
                    clvar._ReportId = reportid;
                    b_fun.Insert_ReportTrackLog(clvar);

                    Get_ReportVersion();
                }
                else
                {
                    error_msg.Text = "No Record Found...";
                }
            }

            if (type.SelectedValue == "excel")
            {
                if (dd_year.Text != "")
                {
                    start_date = " AND year(c.accountReceivingDate) = '" + dd_year.Text + "' ";
                }

                if (dd_Month.Text != "")
                {
                    start_date = " AND month(c.accountReceivingDate) = '" + dd_Month.Text + "' ";
                }

                String BranchID = "", BranchName = "";
                if (branch_chk.Checked)
                {
                    branch = "";
                }
                else
                {

                    for (int i = 0; i < dd_branch.Items.Count; i++)
                    {
                        if (dd_branch.Items[i].Selected)
                        {
                            BranchName += dd_branch.Items[i].Text + ",";
                            BranchID += dd_branch.Items[i].Value + ",";
                        }
                    }
                    if (BranchName != "")
                    {
                        BranchName = BranchName.Remove(BranchName.Length - 1);
                        BranchName.ToString();

                        BranchID = BranchID.Remove(BranchID.Length - 1);
                        BranchID.ToString();
                    }

                    if (dd_branch.SelectedValue != "")
                    {
                        branch = " AND c.orgin IN (" + BranchID.ToString() + ") ";
                    }
                }

                String ZoneID = "", ZoneName = "";
                if (zone_chk.Checked)
                {
                    zone = "";
                }
                else
                {
                    for (int i = 0; i < dd_zone.Items.Count; i++)
                    {
                        if (dd_zone.Items[i].Selected)
                        {
                            ZoneName += dd_zone.Items[i].Text + ",";
                            ZoneID += dd_zone.Items[i].Value + ",";
                        }
                    }

                    if (ZoneName != "")
                    {
                        ZoneName = ZoneName.Remove(ZoneName.Length - 1);
                        ZoneName.ToString();

                        ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                        ZoneID.ToString();
                    }

                    if (dd_zone.SelectedValue != "")
                    {
                        zone = " AND z.zonecode IN (" + ZoneID.ToString() + ") ";
                    }
                }

                clvar._Year = start_date;
                clvar._TownCode = branch;
                clvar._Zone = zone;

                ExportToExcel(sender, e);
            }

            if (type.SelectedValue == "pdf")
            {
                if (dd_year.Text != "")
                {
                    start_date = " AND year(c.accountReceivingDate) = '" + dd_year.Text + "' ";
                }

                if (dd_Month.Text != "")
                {
                    start_date = " AND month(c.accountReceivingDate) = '" + dd_Month.Text + "' ";
                }

                String BranchID = "", BranchName = "";
                if (branch_chk.Checked)
                {
                    branch = "";
                }
                else
                {

                    for (int i = 0; i < dd_branch.Items.Count; i++)
                    {
                        if (dd_branch.Items[i].Selected)
                        {
                            BranchName += dd_branch.Items[i].Text + ",";
                            BranchID += dd_branch.Items[i].Value + ",";
                        }
                    }
                    if (BranchName != "")
                    {
                        BranchName = BranchName.Remove(BranchName.Length - 1);
                        BranchName.ToString();

                        BranchID = BranchID.Remove(BranchID.Length - 1);
                        BranchID.ToString();
                    }

                    if (dd_branch.SelectedValue != "")
                    {
                        branch = " AND c.orgin IN (" + BranchID.ToString() + ") ";
                    }
                }

                String ZoneID = "", ZoneName = "";
                if (zone_chk.Checked)
                {
                    zone = "";
                }
                else
                {
                    for (int i = 0; i < dd_zone.Items.Count; i++)
                    {
                        if (dd_zone.Items[i].Selected)
                        {
                            ZoneName += dd_zone.Items[i].Text + ",";
                            ZoneID += dd_zone.Items[i].Value + ",";
                        }
                    }

                    if (ZoneName != "")
                    {
                        ZoneName = ZoneName.Remove(ZoneName.Length - 1);
                        ZoneName.ToString();

                        ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                        ZoneID.ToString();
                    }

                    if (dd_zone.SelectedValue != "")
                    {
                        zone = " AND z.zonecode IN (" + ZoneID.ToString() + ") ";
                    }
                }
                clvar._Year = start_date;
                clvar._TownCode = branch;
                clvar._Zone = zone;

                ExportToPDF(sender, e);
            }
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CashConsignmentLessChargeReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages 
                clvar._Year = start_date;
                clvar._TownCode = branch;
                clvar._Zone = zone;

                DataSet Year_check = b_fun.Get_CashConsignmentLessChargeReport(clvar);

                if (Year_check.Tables[0].Rows.Count != 0)
                {
                    excelgg.DataSource = Year_check.Tables[0].DefaultView;
                    excelgg.DataBind();
                    /*
                    double Total = 0.00;

                    DataColumn col = Year_check.Tables[0].Columns["TAMOUNT"];
                    foreach (DataRow row in Year_check.Tables[0].Rows)
                    {
                        Total += Double.Parse(row[col].ToString());
                    }

                    excelgg.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                    excelgg.FooterRow.Cells[8].Font.Bold = true;
                    excelgg.FooterRow.Cells[8].Text = Total.ToString();
                     * */
                }

                foreach (TableCell cell in excelgg.HeaderRow.Cells)
                {
                    cell.BackColor = excelgg.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in excelgg.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = excelgg.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = excelgg.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                excelgg.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private void ExportToPDF(object sender, EventArgs e)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    GridView.AllowPaging = false;

                    clvar._Year = start_date;
                    clvar._TownCode = branch;
                    clvar._Zone = zone;

                    DataSet Year_check = b_fun.Get_CashConsignmentLessChargeReport(clvar);

                    if (Year_check.Tables[0].Rows.Count != 0)
                    {
                        GridView.DataSource = Year_check.Tables[0].DefaultView;
                        GridView.DataBind();
                        /*
                        double Total = 0.00;

                        DataColumn col = Year_check.Tables[0].Columns["TAMOUNT"];
                        foreach (DataRow row in Year_check.Tables[0].Rows)
                        {
                            Total += Double.Parse(row[col].ToString());
                        }

                        GridView.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                        GridView.FooterRow.Cells[8].Font.Bold = true;
                        GridView.FooterRow.Cells[8].Text = String.Format("{0:0,0.0}", Total.ToString());
                         * */
                    }

                    GridView.RenderControl(hw);
                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A1, 5f, 5f, 5f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                    BaseFont bfChronicleFont = BaseFont.CreateFont("D:\\Asp-Projects\\ZNi-Bayer-New\\font\\ChronicleDisp-Black.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font ChronicleFont = new Font(bfChronicleFont, 16f);
                    BaseFont bfBrandonFont = BaseFont.CreateFont("D:\\Asp-Projects\\ZNi-Bayer-New\\font\\ChronicleDisp-Black.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font BrandonFont = new Font(bfBrandonFont, 14f, Font.NORMAL);

                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=CashConsignmentLessChargeReport.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }
        protected void zone_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (zone_chk.Checked)
            {
                dd_zone.Visible = false;
                DataSet ds_branch = b_fun.Get_MasterAllBranches(clvar);
                if (ds_branch.Tables.Count != 0)
                {
                    if (ds_branch.Tables[0].Rows.Count != 0)
                    {
                        dd_branch.DataTextField = "branchname";
                        dd_branch.DataValueField = "branchCode";
                        dd_branch.DataSource = ds_branch.Tables[0].DefaultView;
                        dd_branch.DataBind();
                    }
                }
            }
            else
            {

                dd_zone.Visible = true;
                branch_chk.Checked = false;
                dd_branch.Items.Clear();
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
    }
}