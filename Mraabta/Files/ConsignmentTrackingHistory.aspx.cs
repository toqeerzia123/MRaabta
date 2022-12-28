using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace MRaabta.Files
{
    public partial class ConsignmentTrackingHistory : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();

        string branch, zone, payment, year, month;
        string reportid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TrackingHistory_Detail.Visible = false;
            }
        }

        protected void Get_ReportVersion()
        {
            clvar = new Variable();

            lbl_report_version.Text = "";

            reportid = "17";

            clvar._reportid = reportid;

            DataSet ds = b_fun.Get_Report_VersionByReportId(clvar);
            if (ds.Tables[0].Rows.Count != 0)
            {
                lbl_report_version.Text = "Version: " + ds.Tables[0].Rows[0]["version"].ToString();
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gg_CustomerLedger_Month.PageIndex = e.NewPageIndex;
            Btn_Search_Click(sender, e);
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (type.SelectedValue == "html")
            {
                error_msg.Text = "";
                clvar = new Variable();
                TrackingHistory_Detail.Visible = false;

                clvar._CNNumber = cnnumber.Text.Trim();
                DataSet Year_check = b_fun.Get_ConsignmentTrackingHistory(clvar);
                DataSet ds_detail = b_fun.Get_ConsignmentTrackingHistory_Detail(clvar);

                gg_CustomerLedger_Month.Visible = true;

                if (Year_check.Tables[0].Rows.Count != 0)
                {
                    reportid = "17";
                    gg_CustomerLedger_Month.DataSource = Year_check.Tables[0].DefaultView;
                    gg_CustomerLedger_Month.DataBind();

                    Get_ReportVersion();

                    // Insert Report Track Log
                    clvar._ReportId = reportid;
                    b_fun.Insert_ReportTrackLog(clvar);
                }
                else
                {
                    error_msg.Text = "No Record Found...";
                    gg_CustomerLedger_Month.Visible = false;
                }

                if (ds_detail.Tables[0].Rows.Count != 0)
                {
                    TrackingHistory_Detail.Visible = true;

                    lbl_con_num.Text = ds_detail.Tables[0].Rows[0]["consignmentNumber"].ToString();
                    bdate.Text = ds_detail.Tables[0].Rows[0]["bookingDate"].ToString();
                    lbl_orign.Text = ds_detail.Tables[0].Rows[0]["orign"].ToString();
                    lbl_weight.Text = ds_detail.Tables[0].Rows[0]["weight"].ToString();
                    lbl_delivery_time.Text = ds_detail.Tables[0].Rows[0]["DeliveryTime"].ToString();
                    lbl_delivery_rider.Text = ds_detail.Tables[0].Rows[0]["delievryRider"].ToString();
                    lbl_serive_type.Text = ds_detail.Tables[0].Rows[0]["serviceTypeName"].ToString();
                    lbl_destination.Text = ds_detail.Tables[0].Rows[0]["Destination"].ToString();
                    lbl_shipper.Text = ds_detail.Tables[0].Rows[0]["consigner"].ToString();
                    lbl_consignee.Text = ds_detail.Tables[0].Rows[0]["consignee"].ToString();
                    lbl_received.Text = ds_detail.Tables[0].Rows[0]["ReceivedBy"].ToString();
                    lbl_comment.Text = "";
                    lbl_status.Text = ds_detail.Tables[0].Rows[0]["CurrentStatus"].ToString();
                    lbl_account.Text = ds_detail.Tables[0].Rows[0]["AccoutNo"].ToString();
                }
            }

            if (type.SelectedValue == "excel")
            {
                clvar._CNNumber = cnnumber.Text.Trim();
                ExportToExcel(sender, e);
            }

            if (type.SelectedValue == "pdf")
            {
                clvar._CNNumber = cnnumber.Text.Trim();
                ExportToPDF(sender, e);
            }
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ConsignmentTrackingHistory.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                clvar._CNNumber = cnnumber.Text.Trim();
                DataSet Year_check = b_fun.Get_ConsignmentTrackingHistory(clvar);

                gg_CustomerLedger_Month.AllowPaging = false;

                if (Year_check.Tables[0].Rows.Count != 0)
                {
                    gg_CustomerLedger_Month.DataSource = Year_check.Tables[0].DefaultView;
                    gg_CustomerLedger_Month.DataBind();
                }

                foreach (TableCell cell in gg_CustomerLedger_Month.HeaderRow.Cells)
                {
                    cell.BackColor = gg_CustomerLedger_Month.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gg_CustomerLedger_Month.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gg_CustomerLedger_Month.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gg_CustomerLedger_Month.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gg_CustomerLedger_Month.RenderControl(hw);

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

        }

        private void ExportToPDF(object sender, EventArgs e)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=CustomerLedger.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            clvar._CNNumber = cnnumber.Text.Trim();
            DataSet Year_check = b_fun.Get_ConsignmentTrackingHistory(clvar);

            gg_CustomerLedger_Month.AllowPaging = false;

            if (Year_check.Tables[0].Rows.Count != 0)
            {
                gg_CustomerLedger_Month.DataSource = Year_check.Tables[0].DefaultView;
                gg_CustomerLedger_Month.DataBind();
            }

            gg_CustomerLedger_Month.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A3, 5f, 5f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
    }
}