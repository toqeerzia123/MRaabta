using MRaabta.App_Code;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI;

namespace MRaabta.Files
{
    public partial class ReceiptVoucherDSSP : System.Web.UI.Page
    {
        double totalCODAmount = 0;
        double totalRRAmount = 0;
        string DSSPNumber = "";
        string BookingCode = "";
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {

            DSSPNumber = Request.QueryString["DSSP"];
            BookingCode = Request.QueryString["BookingCode"];
            //clvar.RunsheetNumber = "201710000128";
            DataTable header = GetRunsheetHeaderDetails();
            DataTable ExpressCenterUserInfo = GetExpressCenterInfo();
            DataTable dt = GetBulkCODRRByRunsheetNumber();
            if (dt != null && header != null)
            {
                if (dt.Rows.Count > 0 && header.Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    //if (dt.Rows.Count > 5)
                    //{
                    //    //DataTable dt_ = dt.Clone();
                    //    //for (int i = 0; i < dt.Rows.Count; i++)
                    //    //{
                    //    //    if (i > 0 && i % 5 == 0)
                    //    //    {
                    //    //        ds.Tables.Add(dt_);
                    //    //        dt_.Clear();
                    //    //    }
                    //    //    dt_.Rows.Add(dt.Rows[i].ItemArray);
                    //    //}
                    //    //if (dt_.Rows.Count > 0)
                    //    //{
                    //    //    ds.Tables.Add(dt_);
                    //    //}


                    //}
                    //else
                    //{
                    PrintBulkCODRR(header, dt, ExpressCenterUserInfo);
                    //}

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('No Records Found.'); window.close();", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('No Records found.'); window.close();", true);
                return;
            }
            //PrintBulkCODRR();
        }
        public DataTable GetRunsheetHeaderDetails()
        {
            DataTable dt = new DataTable();
            //string sql = "SELECT r.runsheetNumber, \n"
            //   + "CAST(FORMAT(r.runsheetDate, 'dd-MMMM yyyy') AS VARCHAR) RunsheetDate,\n"
            //   + "	       rr.riderCode               DepositorID, \n"
            //   + "	       r2.firstName + ' ' + r2.lastName DepositorName, \n"
            //   + "	       b.sname + '-' + b.name     Location, \n"
            //   + "	       z.name                     Zone, \n"
            //   + "	       r2.HRS_CODE EMPLOYEEID, \n"
            //   + "	       zu.U_Name                    EmployeeName \n"
            //   + "	FROM   Runsheet r \n"
            //   + "	       INNER JOIN RiderRunsheet rr \n"
            //   + "	            ON  rr.runsheetNumber = r.runsheetNumber \n"
            //   + "	       INNER JOIN Riders r2 \n"
            //   + "	            ON  r2.riderCode = rr.riderCode \n"
            //   + "	            AND r2.branchId = r.branchCode \n"
            //   + "	       INNER JOIN Branches b \n"
            //   + "	            ON  b.branchCode = r.branchCode \n"
            //   + "	       INNER JOIN Zones z \n"
            //   + "	            ON  z.zoneCode = b.zoneCode \n"
            //   + "	       INNER JOIN ZNI_USER1 zu \n"
            //   + "	            ON  zu.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
            //   + "	WHERE  r.runsheetNumber = '" + clvar.RunsheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";


            string sql = @"SELECT 
                                   DISTINCT
                                   D.DSSPNUMBER, 
                                   CAST(FORMAT( D.CREATEDON, 'dd-MMMM yyyy') AS VARCHAR) DSSPDATE, 
                                   Z.NAME ZONE, B.SNAME +'-'+B.NAME LOCATION, ZU.BOOKINGSTAFF BOOKINGCODE, R.HRS_CODE,
                                   CAST(FORMAT(GETDATE(), 'dd-MMMM yyyy') AS VARCHAR) PRINT_DATE,
                                   CAST(FORMAT(GETDATE(), 'hh:mm tt') AS VARCHAR) PRINT_DATE,'T_T' DepositorName
                            FROM 
                                   MNP_MASTER_RETAIL_DSSP D 
                                   INNER JOIN MNP_RETAIL_STAFF ZU ON D.CREATEDBY = ZU.USERID
                                   INNER JOIN BRANCHES B ON ZU.BRANCH = B.BRANCHCODE
                                   INNER JOIN ZONES Z ON ZU.ZONE = Z.ZONECODE
                                   INNER JOIN RIDERS R ON ZU.BOOKINGSTAFF = R.RIDERCODE
                            WHERE D.DSSPNUMBER = '" + DSSPNumber + "' ";



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

        public DataTable GetExpressCenterInfo()
        {
            DataTable dt = new DataTable();

            string sql = @"SELECT mprs.EmployeeUsername,ec.expressCenterCode,ec.name
                          FROM MnP_Retail_Staff mprs
                        INNER JOIN ExpressCenters ec ON ec.expressCenterCode=mprs.ExpressCenterCode
                        WHERE mprs.BookingStaff='" + BookingCode + "'";

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
        public DataTable GetBulkCODRRByRunsheetNumber()
        {
            DataTable dt = new DataTable();

            string sql = @"
                        -- GRID DETAIL
                            SELECT  ROW_NUMBER() OVER(
                                   ORDER BY ISNULL(dbo.splitString(PV.ID, '-', '2'), 0) DESC
                               )       Sr,
                               D.DSSPNUMBER,
                               PV.ID            VOUCHER,
                               PS.NAME          PAYMENT_SOURCE,
                               PB.PRODUCT,
                               PV.RECEIPTNO     RR_NUMBER,
                               CAST(FORMAT(PV.VOUCHERDATE, 'dd-MMMM yyyy') AS VARCHAR) RR_DATE,
                               PB.AMOUNT
                        FROM   MNP_MASTER_RETAIL_DSSP D
                               INNER JOIN PAYMENTVOUCHERS PV
                                    ON  CAST(D.DSSPNUMBER AS VARCHAR) = PV.REFNO
                               INNER JOIN MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN PB
                                    ON  PB.VOUCHERID = PV.ID
                               INNER JOIN PAYMENTSOURCE PS
                                    ON  PV.PAYMENTSOURCEID = PS.ID
                        WHERE  D.DSSPNUMBER = '" + DSSPNumber + @"'
                        ORDER BY
                               PS.NAME,
                               PB.PRODUCT    ASC ";

            sql = @" 
                    -- GRID DETAIL  
                                SELECT  ROW_NUMBER() OVER(
                                                       ORDER BY ISNULL(dbo.splitString(PV.ID, '-', '2'), 0) DESC
                                                   )       Sr,
                                   D.DSSPNUMBER, PV.ID VOUCHER, PS.NAME PAYMENT_SOURCE, 
                                   PB.PRODUCT,
                                   PV.RECEIPTNO RR_NUMBER, CAST(FORMAT(PV.VOUCHERDATE, 'dd-MMMM yyyy') AS VARCHAR) RR_DATE,
                                   PB.AMOUNT
                            FROM 
                                   MNP_MASTER_RETAIL_DSSP D
                                   INNER JOIN PAYMENTVOUCHERS PV ON CAST(D.DSSPNUMBER AS VARCHAR) = PV.REFNO     
                                   INNER JOIN MNP_PAYMENTVOUCHERSPRODUCTBREAKDOWN PB ON PB.VOUCHERID = PV.ID
                                   INNER JOIN PAYMENTSOURCE PS ON PV.PAYMENTSOURCEID = PS.ID
                            WHERE D.DSSPNUMBER = '" + DSSPNumber + @"'   
                            GROUP BY
                                         D.DSSPNUMBER, PV.ID, PS.NAME, 
                                         PB.PRODUCT,
                                         PV.RECEIPTNO, CAST(FORMAT(PV.VOUCHERDATE, 'dd-MMMM yyyy') AS VARCHAR),
                                         PB.AMOUNT
                            HAVING PB.AMOUNT > 0 
                            ORDER BY PS.NAME, PB.PRODUCT ASC ";

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
        public DataTable GetRemainingConsignments()
        {
            DataTable dt = new DataTable();

            string sqlString = @"SELECT D.DSSPNumber,convert(varchar,d.CreatedOn,106) CreatedDate,
                               ROUND(ISNULL(TotalAmount, '0'), 0) Total_Amount,
                               ROUND(ISNULL(D.CollectAmount, '0'), 0) Collect_Amount
                        FROM   MNP_Master_Retail_DSSP D
                        WHERE ROUND(D.TotalAmount, 0) > ROUND(ISNULL(D.CollectAmount, '0'), 0)
                               AND D.BookingCode = '" + BookingCode + @"'
                                AND CAST(d.CreatedOn AS DATE)>='2020-08-06'
                        ORDER BY
                               1 ASC";
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

        public void PrintBulkCODRR(DataTable header, DataTable dt, DataTable ExperCenterInfo)
        {
            int totalConsignmentCount = 0;
            string runsheetNumber = "";
            int runsheetAttempt = 0;
            //  object obj = dt.Compute("MAX(ATTEMPT)", "");

            // int.TryParse(obj.ToString(), out runsheetAttempt);


            DataSet ds = new DataSet();
            DataTable tempdt = dt.Clone();
            tempdt.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                {
                    // if (dt.Rows[i]["ATTEMPT"].ToString() != dt.Rows[i - 1]["ATTEMPT"].ToString())
                    {
                        ds.Tables.Add(tempdt);

                        tempdt = new DataTable();
                        tempdt = dt.Clone();
                        tempdt.Clear();
                    }
                }

                tempdt.Rows.Add(dt.Rows[i].ItemArray);
            }
            ds.Tables.Add(tempdt);

            Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            iTextSharp.text.Font NormalFont = FontFactory.GetFont("Calibri", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;

                PdfPTable table_header = null;
                iTextSharp.text.BaseColor color = null;
                document.Open();

                #region First Header Containing Image and Heading of the Document
                table_header = new PdfPTable(3);
                table_header.TotalWidth = 550f;
                table_header.LockedWidth = true;
                table_header.SetWidths(new float[] { 50f, 60f, 370f });

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images") + "\\new_logo.png");
                jpg.ScaleAbsolute(60f, 30f);
                PdfPCell imageCell = new PdfPCell(jpg);
                imageCell.Colspan = 1;
                table_header.AddCell(imageCell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 18, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.PaddingBottom = 0f;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("Receipt Voucher ", FontFactory.GetFont("Calibri", 18, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;

                table_header.AddCell(cell);

                document.Add(table_header);

                #endregion

                #region Header Containing the RR Header Details
                table_header = new PdfPTable(4);
                table_header.TotalWidth = 550f;
                table_header.LockedWidth = true;
                table_header.SetWidths(new float[4] { 85f, 190f, 85f, 190f });
                table_header.PaddingTop = 100f;

                cell = PhraseCell(new Phrase("  ", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 4;
                cell.Padding = 10;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("DSSP #", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingLeft = 20f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase(header.Rows[0]["DSSPNUMBER"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("DSSP Date", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase(header.Rows[0]["DSSPDATE"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase("Location", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingLeft = 20f;
                //cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase(header.Rows[0]["Location"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                //cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase("Print Date", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                //cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(DateTime.Now.ToString("dd-MMMM yyyy"), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                //cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("Zone", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingLeft = 20f;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(header.Rows[0]["Zone"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("Print Time", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(DateTime.Now.ToString("hh:mm tt"), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase("Booking Code", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingLeft = 20f;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(header.Rows[0]["BOOKINGCODE"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("Cashier Id", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(Session["U_NAME"].ToString().ToLower(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase("Staff Id", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.PaddingLeft = 20f;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(ExperCenterInfo.Rows[0]["EmployeeUsername"].ToString().ToLower(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("EC Code", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingLeft = 20f;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(ExperCenterInfo.Rows[0]["expressCenterCode"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.UseVariableBorders = true;
                cell.BorderColorBottom = BaseColor.BLACK;

                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("Express Center", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(ExperCenterInfo.Rows[0]["name"].ToString().ToUpper(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                document.Add(table_header);
                #endregion

                #region Detail Table

                PdfPTable table_detail = new PdfPTable(8);
                table_detail.TotalWidth = 550f;
                table_detail.SetWidths(new float[8] { 17f, 41f, 55f, 40f, 55f, 55f, 45f, 55f });
                table_detail.LockedWidth = true;

                #region Heading Row
                cell = PhraseCell(new Phrase("  ", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 8;
                cell.Padding = 10;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Sr.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Voucher#", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("DSSP No.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Payment Source", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Product", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("RSN.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("RR Date", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);


                cell = PhraseCell(new Phrase("RR Number", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);


                //cell = PhraseCell(new Phrase("Ref#", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("COD Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);
                #endregion

                #region DataRows
                DataRow dr = null;
                for (int j = 0; j < ds.Tables.Count; j++)
                {
                    dt.Clear();
                    dt = new DataTable();
                    dt = ds.Tables[j].Copy();
                    // double attemptedCODAmount = 0;
                    double attemptedRRRAmount = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        totalConsignmentCount++;
                        dr = dt.Rows[i];
                        cell = PhraseCell(new Phrase(dr["SR"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["Voucher"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["DSSPNUMBER"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["PAYMENT_SOURCE"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["PRODUCT"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        //cell = PhraseCell(new Phrase(dr["RunsheetReason"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        //cell.Colspan = 1;
                        //cell.Padding = 2f;
                        ////cell.BorderColorTop = BaseColor.BLACK;
                        //cell.BorderColorLeft = BaseColor.BLACK;
                        //cell.BorderColorRight = BaseColor.BLACK;
                        ////cell.BorderColorBottom = BaseColor.BLACK;
                        //cell.UseVariableBorders = true;
                        //table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["RR_DATE"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["RR_Number"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);



                        //string refNumber = "";
                        //if (dr["RefNumber"].ToString().Length > 10)
                        //{
                        //    refNumber = dr["RefNumber"].ToString().Substring(0, 10);
                        //}
                        //else
                        //{
                        //    refNumber = dr["RefNumber"].ToString();
                        //}
                        //cell = PhraseCell(new Phrase(refNumber, FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        //cell.Colspan = 1;
                        //cell.Padding = 2f;
                        ////cell.BorderColorTop = BaseColor.BLACK;
                        //cell.BorderColorLeft = BaseColor.BLACK;
                        //cell.BorderColorRight = BaseColor.BLACK;
                        ////cell.BorderColorBottom = BaseColor.BLACK;
                        //cell.UseVariableBorders = true;
                        //table_detail.AddCell(cell);

                        //double tempCodAmount = 0;

                        //double.TryParse(dr["CODAMOUNT"].ToString(), out tempCodAmount);
                        //totalCODAmount += tempCodAmount;
                        //attemptedCODAmount += tempCodAmount;
                        //cell = PhraseCell(new Phrase(String.Format("{0:N2}", tempCodAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        //cell.Colspan = 1;
                        //cell.Padding = 2f;
                        ////cell.BorderColorTop = BaseColor.BLACK;
                        //cell.BorderColorLeft = BaseColor.BLACK;
                        //cell.BorderColorRight = BaseColor.BLACK;
                        ////cell.BorderColorBottom = BaseColor.BLACK;
                        //cell.UseVariableBorders = true;
                        //table_detail.AddCell(cell);

                        double tempRRAmount = 0;
                        double.TryParse(dr["Amount"].ToString(), out tempRRAmount);
                        totalRRAmount += tempRRAmount;
                        attemptedRRRAmount += tempRRAmount;
                        cell = PhraseCell(new Phrase(String.Format("{0:N0}", tempRRAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);
                    }

                    if (j == ds.Tables.Count - 1)
                    {
                        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);
                        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 2;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;


                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 3;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase("Received Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        //cell = PhraseCell(new Phrase(String.Format("{0:N2}", attemptedCODAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        //cell.Colspan = 1;
                        //cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        //cell.BorderColorLeft = BaseColor.BLACK;
                        //cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;

                        //cell.UseVariableBorders = true;
                        //table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(String.Format("{0:N0}", totalRRAmount), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);
                    }
                    //else
                    //{
                    //    cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    //    cell.Colspan = 1;
                    //    cell.Padding = 2f;
                    //    cell.BorderColorTop = BaseColor.BLACK;
                    //    cell.BorderColorLeft = BaseColor.BLACK;
                    //    cell.BorderColorRight = BaseColor.BLACK;
                    //    cell.BorderColorBottom = BaseColor.BLACK;

                    //    cell.UseVariableBorders = true;
                    //    table_detail.AddCell(cell);
                    //    int attempt = 0;
                    //    //int.TryParse(ds.Tables[j].Rows[0]["Attempt"].ToString(), out attempt);
                    //    if (attempt == 0)
                    //    {
                    //        attempt = 1;
                    //    }
                    //    cell = PhraseCell(new Phrase("ATTEMPT: " + attempt.ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    //    cell.Colspan = 4;
                    //    cell.Padding = 2f;
                    //    cell.BorderColorTop = BaseColor.BLACK;
                    //    cell.BorderColorLeft = BaseColor.BLACK;
                    //    cell.BorderColorRight = BaseColor.BLACK;
                    //    cell.BorderColorBottom = BaseColor.BLACK;

                    //    cell.UseVariableBorders = true;
                    //    table_detail.AddCell(cell);

                    //    cell = PhraseCell(new Phrase("Attempted On", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    //    cell.Colspan = 2;
                    //    cell.Padding = 2f;
                    //    cell.BorderColorTop = BaseColor.BLACK;
                    //    cell.BorderColorLeft = BaseColor.BLACK;
                    //    cell.BorderColorRight = BaseColor.BLACK;
                    //    cell.BorderColorBottom = BaseColor.BLACK;

                    //    cell.UseVariableBorders = true;
                    //    table_detail.AddCell(cell);

                    //    cell = PhraseCell(new Phrase("---", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    //    cell.Colspan = 1;
                    //    cell.Padding = 2f;
                    //    cell.BorderColorTop = BaseColor.BLACK;
                    //    cell.BorderColorLeft = BaseColor.BLACK;
                    //    cell.BorderColorRight = BaseColor.BLACK;
                    //    cell.BorderColorBottom = BaseColor.BLACK;

                    //    cell.UseVariableBorders = true;
                    //    table_detail.AddCell(cell);

                    //    cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    //    cell.Colspan = 1;
                    //    cell.Padding = 2f;
                    //    cell.BorderColorTop = BaseColor.BLACK;
                    //    cell.BorderColorLeft = BaseColor.BLACK;
                    //    cell.BorderColorRight = BaseColor.BLACK;
                    //    cell.BorderColorBottom = BaseColor.BLACK;

                    //    cell.UseVariableBorders = true;
                    //    table_detail.AddCell(cell);

                    //    //cell = PhraseCell(new Phrase(String.Format("{0:N2}", attemptedCODAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                    //    //cell.Colspan = 1;
                    //    //cell.Padding = 2f;
                    //    //cell.BorderColorTop = BaseColor.BLACK;
                    //    //cell.BorderColorLeft = BaseColor.BLACK;
                    //    //cell.BorderColorRight = BaseColor.BLACK;
                    //    //cell.BorderColorBottom = BaseColor.BLACK;

                    //    //cell.UseVariableBorders = true;
                    //    //table_detail.AddCell(cell);

                    //    cell = PhraseCell(new Phrase(String.Format("{0:N2}", attemptedRRRAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                    //    cell.Colspan = 1;
                    //    cell.Padding = 2f;
                    //    cell.BorderColorTop = BaseColor.BLACK;
                    //    cell.BorderColorLeft = BaseColor.BLACK;
                    //    cell.BorderColorRight = BaseColor.BLACK;
                    //    cell.BorderColorBottom = BaseColor.BLACK;

                    //    cell.UseVariableBorders = true;
                    //    table_detail.AddCell(cell);
                    //}

                }

                #region MyRegion
                //dr = dt.Rows[dt.Rows.Count - 1];
                //cell = PhraseCell(new Phrase(dr["SR"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(dr["VoucherID"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(dr["CNNo"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(dr["AccountNumber"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(dr["AccName"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(dr["RunsheetReason"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(dr["RRNumber"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(dr["RRDATE"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);
                //cell = PhraseCell(new Phrase(dr["RefNumber"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell); 


                //double tempCodAmount_ = 0;
                //double.TryParse(dr["CODAMOUNT"].ToString(), out tempCodAmount_);
                //totalCODAmount += tempCodAmount_;
                //cell = PhraseCell(new Phrase(String.Format("{0:N2}", tempCodAmount_), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //double tempRRAmount_ = 0;
                //double.TryParse(dr["RRAmount"].ToString(), out tempRRAmount_);
                //totalRRAmount += tempRRAmount_;
                //cell = PhraseCell(new Phrase(String.Format("{0:N2}", tempRRAmount_), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);
                #endregion


                #region Footer Row
                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;

                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase(totalConsignmentCount.ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);


                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);


                //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);



                //cell = PhraseCell(new Phrase(String.Format("{0:N0}", totalRRAmount), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                //cell.Colspan = 1;
                //cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                //cell.UseVariableBorders = true;
                //table_detail.AddCell(cell);

                // #region Remaining HeaderRow
                cell = PhraseCell(new Phrase("  ", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 8;
                cell.Padding = 2;
                table_detail.AddCell(cell);


                document.Add(table_detail);
                #endregion



                DataTable remainingDt = GetRemainingConsignments();
                if (remainingDt.Rows.Count > 0)
                {
                    PdfPTable remainingCNTable = new PdfPTable(5);
                    remainingCNTable.TotalWidth = 550f;
                    remainingCNTable.SetWidths(new float[5] { 17f,85f, 85f, 95f, 95f });
                    remainingCNTable.LockedWidth = true;


                    cell = PhraseCell(new Phrase("Remaining DSSP.", FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 5;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("SNo.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("DSSP Number", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("Date", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("Total Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("Collected Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    //cell = PhraseCell(new Phrase("COD Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    //cell.Colspan = 1;
                    //cell.Padding = 2f;
                    //cell.BorderColorTop = BaseColor.BLACK;
                    //cell.BorderColorLeft = BaseColor.BLACK;
                    //cell.BorderColorRight = BaseColor.BLACK;
                    //cell.BorderColorBottom = BaseColor.BLACK;
                    //cell.UseVariableBorders = true;
                    //remainingCNTable.AddCell(cell);

                    //cell = PhraseCell(new Phrase("Delivery Status", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    //cell.Colspan = 1;
                    //cell.Padding = 2f;
                    //cell.BorderColorTop = BaseColor.BLACK;
                    //cell.BorderColorLeft = BaseColor.BLACK;
                    //cell.BorderColorRight = BaseColor.BLACK;
                    //cell.BorderColorBottom = BaseColor.BLACK;
                    //cell.UseVariableBorders = true;
                    //remainingCNTable.AddCell(cell);

                    double totalRemainingCODAmount = 0;
                    for (int i = 0; i < remainingDt.Rows.Count; i++)
                    {
                        cell = PhraseCell(new Phrase((i + 1).ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        cell = PhraseCell(new Phrase(remainingDt.Rows[i]["DSSPNumber"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        cell = PhraseCell(new Phrase(remainingDt.Rows[i]["CreatedDate"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        cell = PhraseCell(new Phrase(Double.Parse(remainingDt.Rows[i]["Total_Amount"].ToString()).ToString("N0"), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        cell = PhraseCell(new Phrase(Double.Parse(remainingDt.Rows[i]["Collect_Amount"].ToString()).ToString("N0"), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        //double remainingCodAmount = 0;
                        //double.TryParse(remainingDt.Rows[i]["CodAmount"].ToString(), out remainingCodAmount);
                        //totalRemainingCODAmount += remainingCodAmount;
                        //cell = PhraseCell(new Phrase(String.Format("{0:N2}", remainingCodAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        //cell.Colspan = 1;
                        //cell.Padding = 2f;
                        ////cell.BorderColorTop = BaseColor.BLACK;
                        //cell.BorderColorLeft = BaseColor.BLACK;
                        //cell.BorderColorRight = BaseColor.BLACK;
                        ////cell.BorderColorBottom = BaseColor.BLACK;
                        //cell.UseVariableBorders = true;
                        //remainingCNTable.AddCell(cell);

                        //cell = PhraseCell(new Phrase(remainingDt.Rows[i]["RunsheetReason"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        //cell.Colspan = 1;
                        //cell.Padding = 2f;
                        ////cell.BorderColorTop = BaseColor.BLACK;
                        //cell.BorderColorLeft = BaseColor.BLACK;
                        //cell.BorderColorRight = BaseColor.BLACK;
                        ////cell.BorderColorBottom = BaseColor.BLACK;
                        //cell.UseVariableBorders = true;
                        //remainingCNTable.AddCell(cell);
                    }



                    cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase(remainingDt.Rows.Count.ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase(Double.Parse(remainingDt.Compute("sum(Total_Amount)", string.Empty).ToString()).ToString("N0"), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase(Double.Parse(remainingDt.Compute("sum(Collect_Amount)", string.Empty).ToString()).ToString("N0"), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    //cell = PhraseCell(new Phrase(String.Format("{0:N2}", totalRemainingCODAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                    //cell.Colspan = 1;
                    //cell.Padding = 2f;
                    //cell.BorderColorTop = BaseColor.BLACK;
                    //cell.BorderColorLeft = BaseColor.BLACK;
                    //cell.BorderColorRight = BaseColor.BLACK;
                    //cell.BorderColorBottom = BaseColor.BLACK;
                    //cell.UseVariableBorders = true;
                    //remainingCNTable.AddCell(cell);

                    //cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    //cell.Colspan = 1;
                    //cell.Padding = 2f;
                    //cell.BorderColorTop = BaseColor.BLACK;
                    //cell.BorderColorLeft = BaseColor.BLACK;
                    //cell.BorderColorRight = BaseColor.BLACK;
                    //cell.BorderColorBottom = BaseColor.BLACK;
                    //cell.UseVariableBorders = true;
                    //remainingCNTable.AddCell(cell);


                    document.Add(remainingCNTable);
                }



                #endregion
                table_detail = new PdfPTable(2);
                table_detail.TotalWidth = 550f;
                table_detail.SetWidths(new float[2] { 275f, 275f });
                table_detail.LockedWidth = true;

                cell = PhraseCell(new Phrase("This is a system generated report and does not require any signature.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                //cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Printed on " + DateTime.Now.ToString("dd-MMM-yy HH:mm") + " hrs.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                ////cell.BorderColorTop = BaseColor.BLACK;
                //cell.BorderColorLeft = BaseColor.BLACK;
                //cell.BorderColorRight = BaseColor.BLACK;
                //cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                document.Add(table_detail);
                #endregion


                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                string fileName = "ReceiptVoucher_" + DSSPNumber + "_" + DateTime.Now.ToShortDateString();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
            }
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }
    }
}