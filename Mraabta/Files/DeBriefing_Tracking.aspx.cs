using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class DeBriefing_Tracking : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();

        string branch, zone, payment, year, month;
        string reportid, cnNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //  lbl_live_msg.Text = "DeBriefing Tracking Report<br>Report On: Live Server<br>";
                cnNumber = Request.QueryString["cn"];
                clvar.CNNumber = cnNumber;
                Result();
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
            GV.PageIndex = e.NewPageIndex;
            Result();
        }

        protected void Result()
        {
            error_msg.Text = "";
            clvar = new Variable();

            #region Debriefing
            if (Request.QueryString["type"] == "debriefing")
            {
                GV.DataSource = null;
                GV.DataBind();

                clvar._CNNumber = Request.QueryString["cn"];
                DataSet ds = getDeBriefing(clvar);

                GV.Visible = true;

                if (ds.Tables[0].Rows.Count != 0)
                {
                    lbl_live_msg.Text = "DeBriefing Tracking Report<br>Report On: Live Server<br>";
                    reportid = "17";
                    btn_excel.Visible = true;
                    GV.DataSource = ds.Tables[0].DefaultView;
                    GV.DataBind();
                }
                else
                {
                    error_msg.Text = "No Record Found...";
                    GV.Visible = false;
                }
            }
            #endregion

            #region Dimension

            if (Request.QueryString["type"] == "dimension")
            {
                clvar._CNNumber = Request.QueryString["cn"];
                DataSet ds = getDimension_Detail(clvar);

                GV.Visible = false;

                if (ds.Tables[0].Rows.Count != 0)
                {
                    GV.Visible = true;
                    lbl_live_msg.Text = "Dimension Tracking Report<br>Report On: Live Server<br>";

                    reportid = "17";
                    btn_excel.Visible = true;
                    GV.DataSource = ds.Tables[0].DefaultView;
                    GV.DataBind();

                    double colTotal1 = 0, colTotal2 = 0, colTotal3 = 0, colTotal4 = 0, colTotal5 = 0, colTotal6 = 0;

                    DataColumn col1 = ds.Tables[0].Columns["Width"];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        colTotal1 += double.Parse(row[col1].ToString());
                    }

                    DataColumn col2 = ds.Tables[0].Columns["Breadth"];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        colTotal2 += double.Parse(row[col2].ToString());
                    }

                    DataColumn col3 = ds.Tables[0].Columns["Height"];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        colTotal3 += double.Parse(row[col3].ToString());
                    }

                    DataColumn col4 = ds.Tables[0].Columns["Dense Weight"];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        colTotal4 += double.Parse(row[col4].ToString());
                    }

                    DataColumn col5 = ds.Tables[0].Columns["volume Weight"];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        colTotal5 += double.Parse(row[col5].ToString());
                    }

                    DataColumn col6 = ds.Tables[0].Columns["Pieces"];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        colTotal6 += double.Parse(row[col6].ToString());
                    }

                    GV.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    GV.FooterRow.Cells[1].Font.Bold = true;
                    GV.FooterRow.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                    GV.FooterRow.Cells[1].Text = string.Format("{0:N0}", colTotal1);

                    GV.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    GV.FooterRow.Cells[2].Font.Bold = true;
                    GV.FooterRow.Cells[2].BackColor = System.Drawing.Color.LightBlue;
                    GV.FooterRow.Cells[2].Text = string.Format("{0:N0}", colTotal2);

                    GV.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    GV.FooterRow.Cells[3].Font.Bold = true;
                    GV.FooterRow.Cells[3].BackColor = System.Drawing.Color.LightBlue;
                    GV.FooterRow.Cells[3].Text = string.Format("{0:N0}", colTotal3);

                    GV.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    GV.FooterRow.Cells[4].Font.Bold = true;
                    GV.FooterRow.Cells[4].BackColor = System.Drawing.Color.LightBlue;
                    GV.FooterRow.Cells[4].Text = string.Format("{0:N0}", colTotal4);

                    GV.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    GV.FooterRow.Cells[5].Font.Bold = true;
                    GV.FooterRow.Cells[5].BackColor = System.Drawing.Color.LightBlue;
                    GV.FooterRow.Cells[5].Text = string.Format("{0:N2}", colTotal5);

                    GV.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    GV.FooterRow.Cells[6].Font.Bold = true;
                    GV.FooterRow.Cells[6].BackColor = System.Drawing.Color.LightBlue;
                    GV.FooterRow.Cells[6].Text = string.Format("{0:N0}", colTotal6);

                }
                else
                {
                    error_msg.Text = "No Record Found...";
                    GV.Visible = false;
                }
            }

            #endregion


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

                GV.RenderControl(hw);

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

        public DataSet getDeBriefing(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT db.shipperName,db.shipperAddress,db.shipperContact,db.consigneeName, \n"
               + "  db.consigneeAddress,db.consigneeContact,db.runsheetNumber,db.comments,db.createdOn,z.name zoneCode,b.name branchCode,zu.U_NAME \n"
               + "    FROM Debriefing_Consignment db \n"
               + "  INNER JOIN Zones z ON z.zoneCode = db.zonecode \n"
               + "   INNER JOIN branches b ON b.branchCode = db.branchCode \n"
               + "   INNER JOIN ZNI_USER1 zu ON db.createdBy = zu.U_ID \n"
               + "  WHERE db.consignmentNumber = '" + clvar._CNNumber + "' ORDER BY db.createdOn desc";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
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

        public DataSet getDimension_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT  \n"
               + "c.Width,c.Breadth,c.Height,c.DenseWeight 'Dense Weight',\n"
               + "(c.Width*c.Breadth*c.Height)/5000 'volume Weight', \n"
               + "c.Pieces,zu.U_NAME 'Created By', c.CreatedOn 'Created On' \n"
               + "FROM Consignment_Dimensions c  \n"
               + "INNER JOIN ZNI_USER1 zu ON c.CreatedBy = zu.U_ID \n"
               + "WHERE c.ConsignmentNumber = '" + clvar._CNNumber + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
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