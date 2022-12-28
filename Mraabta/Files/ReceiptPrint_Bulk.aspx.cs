using iTextSharp.text;
using iTextSharp.text.pdf;
using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace MRaabta.Files
{
    public partial class ReceiptPrint_Bulk : System.Web.UI.Page
    {
        double totalCODAmount = 0;
        double totalRRAmount = 0;
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            clvar = Session["VariableObj"] as Cl_Variables;
            //clvar.RunsheetNumber = "201710000128";
            DataTable header = GetRunsheetHeaderDetails(clvar);

            DataTable dt = GetBulkCODRRByRunsheetNumber(clvar);
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
                    PrintBulkCODRR(header, dt);
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
        public DataTable GetRunsheetHeaderDetails(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT r.runsheetNumber, \n"
               + "CAST(FORMAT(r.runsheetDate, 'dd-MMMM yyyy') AS VARCHAR) RunsheetDate,\n"
               + "	       rr.riderCode               DepositorID, \n"
               + "	       r2.firstName + ' ' + r2.lastName DepositorName, \n"
               + "	       b.sname + '-' + b.name     Location, \n"
               + "	       z.name                     Zone, \n"
               + "	       r2.HRS_CODE EMPLOYEEID, \n"
               + "	       zu.U_Name                    EmployeeName \n"
               + "	FROM   Runsheet r \n"
               + "	       INNER JOIN RiderRunsheet rr \n"
               + "	            ON  rr.runsheetNumber = r.runsheetNumber \n"
               + "	       INNER JOIN Riders r2 \n"
               + "	            ON  r2.riderCode = rr.riderCode \n"
               + "	            AND r2.branchId = r.branchCode \n"
               + "	       INNER JOIN Branches b \n"
               + "	            ON  b.branchCode = r.branchCode \n"
               + "	       INNER JOIN Zones z \n"
               + "	            ON  z.zoneCode = b.zoneCode \n"
               + "	       INNER JOIN ZNI_USER1 zu \n"
               + "	            ON  zu.U_ID = '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
               + "	WHERE  r.runsheetNumber = '" + clvar.RunsheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";

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
        public DataTable GetBulkCODRRByRunsheetNumber(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sql = "SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.splitString(pv.depositSlipNo, '-', '2'),0) DESC ) Sr,\n"
                + "       c.consignmentNumber      CNNo, \n"
                + "       c.consignerAccountNo     AccountNumber, \n"
                + "       CASE \n"
                + "            WHEN LEN(cc.name) > 21 THEN SUBSTRING(cc.name, 1, 21)\n"
                + "            ELSE cc.name\n"
                + "       END                      AccName,\n"
                + "       SUM(cdn.codAmount) / COUNT(cdn.consignmentNumber) CodAmount, \n"
                + "       pv.Id                    VoucherID, \n"
                + "       pv.ReceiptNo             RRNumber, \n"
                + "       CAST(FORMAT(pv.VoucherDate, 'dd-MM-yyyy') AS VARCHAR) RRDate, \n"
                + "       FORMAT(pv.CreatedOn, 'dd-MM-yyyy hh:mm:ss tt') RREntry, \n"
                + "       pv.Amount                RRAmount, \n"
                + "       pv.RefNo                 RefNumber, \n"
                + "       CASE  \n"
                + "            WHEN pv.Id IS NOT NULL THEN 'Y' \n"
                + "            ELSE 'N' \n"
                + "       END                      VoucherExists, \n"
                + "       rc.runsheetNumber        RunsheetNumber, \n"
                + "       dbo.splitstring(l.AttributeValue,'-',1)         RunsheetReason, \n"
                + "       r2.riderCode             RiderCode, \n"
                + "       r2.expressCenterId       ExpressCenterCode, \n"
                + "       cc.id                    CreditClientID, ISNULL(dbo.splitString(pv.depositSlipNo, '-', '2'),0) Attempt, case when pv.id is null then 'R' else 'E' end RRstatus, rc.sortOrder \n"
                + "FROM   Runsheet r \n"
                + "       INNER JOIN RunsheetConsignment rc \n"
                + "            ON  rc.runsheetNumber = r.runsheetNumber \n"
                //+ "            AND rc.routeCode = r.routeCode \n"
                + "           -- AND rc.createdBy = r.createdBy \n"
                + "            AND rc.branchCode = r.branchCode \n"
                + "       INNER JOIN RiderRunsheet rr\n"
                + "            ON rr.runsheetNumber = rc.runsheetNumber\n"
                + "           --AND rr.createdBy = rc.createdBy\n"
                + "       INNER JOIN Riders r2 \n"
                + "            ON r2.branchId = r.branchCode \n"
                //+ "           AND r2.routeCode = r.routeCode \n"
                + "           and r2.riderCode = rr.riderCode\n"
                + "           --AND r2.status = '1' \n"
                + "       INNER JOIN Consignment c \n"
                + "            ON  c.consignmentNumber = rc.consignmentNumber \n"
                + "       LEFT OUTER JOIN CODConsignmentDetail_New cdn \n"
                + "            ON  cdn.consignmentNumber = rc.consignmentNumber \n"
                + "       INNER JOIN CreditClients cc \n"
                + "            ON  cc.id = c.creditClientId \n"
                + "       INNER JOIN PaymentVouchers pv \n"
                + "            ON  pv.ConsignmentNo = c.consignmentNumber \n"
                + "            AND dbo.SplitString(pv.depositSlipNo, '-', '1') = rc.runsheetNumber\n"
                + "       LEFT OUTER JOIN rvdbo.Lookup l \n"
                + "            ON  l.AttributeGroup = 'POD_REASON' \n"
                + "            AND l.Id = rc.Reason \n"
                + "WHERE  rc.runsheetNumber = '" + clvar.RunsheetNumber + "' AND rc.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n"
                + "       --AND r2.riderCode = '650' \n"
                + "       AND c.cod = '1' \n"
                + "GROUP BY \n"
                + "       c.consignmentNumber, \n"
                + "       c.consignerAccountNo, \n"
                + "       cc.name, \n"
                + "       pv.Id, \n"
                + "       pv.VoucherDate, \n"
                + "       rc.runsheetNumber, \n"
                + "       r2.riderCode, \n"
                + "       r2.expressCenterId, \n"
                + "       cc.id, \n"
                + "       pv.ReceiptNo, \n"
                + "       pv.CreatedOn, \n"
                + "       pv.Amount, \n"
                + "       l.AttributeValue, \n"
                + "       pv.RefNo, pv.DepositSlipNo, rc.sortOrder\n"
                + "";
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
        public DataTable GetRemainingConsignments(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string sqlString =
                                "SELECT c.consignmentNumber CNNo,\n" +
                                "       c.consignerAccountNo AccountNumber,\n" +
                                "       CASE\n" +
                                "         WHEN LEN(cc.name) > 21 THEN\n" +
                                "          SUBSTRING(cc.name, 1, 21)\n" +
                                "         ELSE\n" +
                                "          cc.name\n" +
                                "       END AccName,\n" +
                                "       SUM(ISNULL(cdn.codAmount, '0')) /\n" +
                                "       COUNT(ISNULL(cdn.consignmentNumber, '0')) CodAmount,\n" +
                                "       pv.Id VoucherID,\n" +
                                "       pv.ReceiptNo RRNumber,\n" +
                                "       CAST(FORMAT(pv.VoucherDate, 'dd-MM-yyyy') AS VARCHAR) RRDate,\n" +
                                "       FORMAT(pv.CreatedOn, 'dd-MM-yyyy hh:mm tt') RREntry,\n" +
                                "       pv.Amount RRAmount,\n" +
                                "       pv.RefNo RefNumber,\n" +
                                "       CASE\n" +
                                "         WHEN pv.Id IS NOT NULL THEN\n" +
                                "          'Y'\n" +
                                "         ELSE\n" +
                                "          'N'\n" +
                                "       END VoucherExists,\n" +
                                "       rc.runsheetNumber RunsheetNumber,\n" +
                                "       l.AttributeValue RunsheetReason,\n" +
                                "       r2.riderCode RiderCode,\n" +
                                "       r2.expressCenterId ExpressCenterCode,\n" +
                                "       cc.id CreditClientID,\n" +
                                "       dbo.SplitString(pv.DepositSlipNo, '-', '1') DepositSlipNo\n" +
                                "  FROM Runsheet r\n" +
                                " INNER JOIN RunsheetConsignment rc\n" +
                                "    ON rc.runsheetNumber = r.runsheetNumber\n" +
                                "   AND rc.routeCode = r.routeCode\n" +
                                "   AND rc.createdBy = r.createdBy\n" +
                                "   AND rc.branchCode = r.branchCode\n" +
                                " INNER JOIN RiderRunsheet rr\n" +
                                "    ON rr.runsheetNumber = rc.runsheetNumber\n" +
                                "   AND rr.createdBy = rc.createdBy\n" +
                                " INNER JOIN Riders r2\n" +
                                "    ON r2.branchId = r.branchCode\n" +
                                "   AND r2.routeCode = r.routeCode\n" +
                                "   AND r2.riderCode = rr.riderCode\n" +
                                "   --AND r2.status = '1'\n" +
                                " INNER JOIN Consignment c\n" +
                                "    ON c.consignmentNumber = rc.consignmentNumber\n" +
                                " INNER JOIN CreditClients cc\n" +
                                "    ON cc.id = c.creditClientId\n" +
                                "  LEFT OUTER JOIN CODConsignmentDetail_New cdn\n" +
                                "    ON cdn.consignmentNumber = rc.consignmentNumber\n" +
                                "  LEFT OUTER JOIN PaymentVouchers pv\n" +
                                "    ON pv.ConsignmentNo = c.consignmentNumber\n" +
                                "  LEFT OUTER JOIN rvdbo.Lookup l\n" +
                                "    ON l.AttributeGroup = 'POD_REASON'\n" +
                                "   AND l.Id = rc.Reason\n" +
                                " WHERE rc.runsheetNumber = '" + clvar.RunsheetNumber + "' and rc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
                                "      --AND r2.riderCode = '650'\n" +
                                "   AND c.cod = '1'\n" +
                                "   AND rc.runsheetNumber = dbo.splitstring(pv.DepositSlipNo, '-', 1)\n" +
                                "   AND pv.Id IS NULL\n" +
                                " GROUP BY c.consignmentNumber,\n" +
                                "          c.consignerAccountNo,\n" +
                                "          cc.name,\n" +
                                "          pv.Id,\n" +
                                "          pv.VoucherDate,\n" +
                                "          rc.runsheetNumber,\n" +
                                "          r2.riderCode,\n" +
                                "          r2.expressCenterId,\n" +
                                "          cc.id,\n" +
                                "          pv.ReceiptNo,\n" +
                                "          pv.CreatedOn,\n" +
                                "          pv.Amount,\n" +
                                "          l.AttributeValue,\n" +
                                "          pv.RefNo,\n" +
                                "          pv.DepositSlipNo";




            sqlString = "SELECT c.consignmentNumber CNNo,\n" +
            "       c.consignerAccountNo AccountNumber,\n" +
            "       CASE\n" +
            "         WHEN LEN(cc.name) > 21 THEN\n" +
            "          SUBSTRING(cc.name, 1, 21)\n" +
            "         ELSE\n" +
            "          cc.name\n" +
            "       END AccName,\n" +
            "       SUM(ISNULL(cdn.codAmount, '0')) /\n" +
            "       COUNT(ISNULL(cdn.consignmentNumber, '0')) CodAmount,\n" +
            "       pv.Id VoucherID,\n" +
            "       pv.ReceiptNo RRNumber,\n" +
            "       CAST(FORMAT(pv.VoucherDate, 'dd-MM-yyyy') AS VARCHAR) RRDate,\n" +
            "       FORMAT(pv.CreatedOn, 'dd-MM-yyyy hh:mm tt') RREntry,\n" +
            "       pv.Amount RRAmount,\n" +
            "       pv.RefNo RefNumber,\n" +
            "       CASE\n" +
            "         WHEN pv.Id IS NOT NULL THEN\n" +
            "          'Y'\n" +
            "         ELSE\n" +
            "          'N'\n" +
            "       END VoucherExists,\n" +
            "       rc.runsheetNumber RunsheetNumber,\n" +
            "       l.AttributeValue RunsheetReason,\n" +
            "       r2.riderCode RiderCode,\n" +
            "       r2.expressCenterId ExpressCenterCode,\n" +
            "       cc.id CreditClientID,\n" +
            "       dbo.SplitString(pv.DepositSlipNo, '-', '1') DepositSlipNo\n" +
            "  FROM Runsheet r\n" +
            " INNER JOIN RunsheetConsignment rc\n" +
            "    ON rc.runsheetNumber = r.runsheetNumber\n" +
            "   AND rc.routeCode = r.routeCode\n" +
            "   AND rc.createdBy = r.createdBy\n" +
            "   AND rc.branchCode = r.branchCode\n" +
            " INNER JOIN RiderRunsheet rr\n" +
            "    ON rr.runsheetNumber = rc.runsheetNumber\n" +
            "   AND rr.createdBy = rc.createdBy\n" +
            " INNER JOIN Riders r2\n" +
            "    ON r2.branchId = r.branchCode\n" +
            "   AND r2.routeCode = r.routeCode\n" +
            "   AND r2.riderCode = rr.riderCode\n" +
            "   --AND r2.status = '1'\n" +
            " INNER JOIN Consignment c\n" +
            "    ON c.consignmentNumber = rc.consignmentNumber\n" +
            " INNER JOIN CreditClients cc\n" +
            "    ON cc.id = c.creditClientId\n" +
            "  LEFT OUTER JOIN CODConsignmentDetail_New cdn\n" +
            "    ON cdn.consignmentNumber = rc.consignmentNumber\n" +
            "  LEFT OUTER JOIN PaymentVouchers pv\n" +
            "    ON pv.ConsignmentNo = c.consignmentNumber\n" +
            "  LEFT OUTER JOIN rvdbo.Lookup l\n" +
            "    ON l.AttributeGroup = 'POD_REASON'\n" +
            "   AND l.Id = rc.Reason\n" +
            " WHERE rc.runsheetNumber = '" + clvar.RunsheetNumber + "' and rc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "      --AND r2.riderCode = '650'\n" +
            "   AND c.cod = '1'\n" +
            "      --AND rc.runsheetNumber = dbo.splitstring(pv.DepositSlipNo, '-', 1)\n" +
            "   AND pv.Id IS NULL\n" +
            " GROUP BY c.consignmentNumber,\n" +
            "          c.consignerAccountNo,\n" +
            "          cc.name,\n" +
            "          pv.Id,\n" +
            "          pv.VoucherDate,\n" +
            "          rc.runsheetNumber,\n" +
            "          r2.riderCode,\n" +
            "          r2.expressCenterId,\n" +
            "          cc.id,\n" +
            "          pv.ReceiptNo,\n" +
            "          pv.CreatedOn,\n" +
            "          pv.Amount,\n" +
            "          l.AttributeValue,\n" +
            "          pv.RefNo,\n" +
            "          pv.DepositSlipNo";

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



        public void PrintBulkCODRR(DataTable header, DataTable dt)
        {
            int totalConsignmentCount = 0;
            string runsheetNumber = "";
            int runsheetAttempt = 0;
            object obj = dt.Compute("MAX(ATTEMPT)", "");

            int.TryParse(obj.ToString(), out runsheetAttempt);


            DataSet ds = new DataSet();
            DataTable tempdt = dt.Clone();
            tempdt.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                {
                    if (dt.Rows[i]["ATTEMPT"].ToString() != dt.Rows[i - 1]["ATTEMPT"].ToString())
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

                cell = PhraseCell(new Phrase("Runsheet #", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingLeft = 20f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(header.Rows[0]["RunsheetNumber"].ToString() + '-' + runsheetAttempt.ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase("Runsheet Date", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);

                cell = PhraseCell(new Phrase(header.Rows[0]["RunsheetDate"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
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


                cell = PhraseCell(new Phrase("Depositer ID", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingLeft = 20f;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);
                cell = PhraseCell(new Phrase(header.Rows[0]["DepositorID"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                table_header.AddCell(cell);
                cell = PhraseCell(new Phrase("Depositer Name", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                table_header.AddCell(cell);
                cell = PhraseCell(new Phrase(header.Rows[0]["DepositorName"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);


                cell = PhraseCell(new Phrase("Employee ID", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.PaddingLeft = 20f;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);
                cell = PhraseCell(new Phrase(header.Rows[0]["EMPLOYEEID"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);
                cell = PhraseCell(new Phrase("User Name", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 3f;
                cell.PaddingBottom = 5f;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_header.AddCell(cell);
                cell = PhraseCell(new Phrase(header.Rows[0]["EmployeeName"].ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
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

                PdfPTable table_detail = new PdfPTable(11);
                table_detail.TotalWidth = 550f;
                table_detail.SetWidths(new float[11] { 15f, 37f, 70f, 26f, 98f, 25f, 45f, 88f, 46f, 50f, 50f });
                table_detail.LockedWidth = true;

                #region Heading Row
                cell = PhraseCell(new Phrase("  ", FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 11;
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

                cell = PhraseCell(new Phrase("Consignment No.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("ACC#", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Customer Name", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("RSN.", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
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

                cell = PhraseCell(new Phrase("RR Date", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("Ref#", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("COD Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("RR Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
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
                    double attemptedCODAmount = 0;
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

                        cell = PhraseCell(new Phrase(dr["VoucherID"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["CNNo"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["AccountNumber"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["AccName"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["RunsheetReason"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["RRNumber"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(dr["RRDATE"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);
                        string refNumber = "";
                        if (dr["RefNumber"].ToString().Length > 10)
                        {
                            refNumber = dr["RefNumber"].ToString().Substring(0, 10);
                        }
                        else
                        {
                            refNumber = dr["RefNumber"].ToString();
                        }
                        cell = PhraseCell(new Phrase(refNumber, FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        double tempCodAmount = 0;

                        double.TryParse(dr["CODAMOUNT"].ToString(), out tempCodAmount);
                        totalCODAmount += tempCodAmount;
                        attemptedCODAmount += tempCodAmount;
                        cell = PhraseCell(new Phrase(String.Format("{0:N2}", tempCodAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        double tempRRAmount = 0;
                        double.TryParse(dr["RRAmount"].ToString(), out tempRRAmount);
                        totalRRAmount += tempRRAmount;
                        attemptedRRRAmount += tempRRAmount;
                        cell = PhraseCell(new Phrase(String.Format("{0:N2}", tempRRAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);
                    }

                    if (j == 0)
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
                        cell = PhraseCell(new Phrase("CURRENT ATTEMP", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 4;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase("Attempted On", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(ds.Tables[j].Rows[0]["RREntry"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase("Payable", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(String.Format("{0:N2}", attemptedCODAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(String.Format("{0:N2}", attemptedRRRAmount), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);
                    }
                    else
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
                        int attempt = 0;
                        int.TryParse(ds.Tables[j].Rows[0]["Attempt"].ToString(), out attempt);
                        if (attempt == 0)
                        {
                            attempt = 1;
                        }
                        cell = PhraseCell(new Phrase("ATTEMPT: " + attempt.ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                        cell.Colspan = 4;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase("Attempted On", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 2;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(ds.Tables[j].Rows[0]["RREntry"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(String.Format("{0:N2}", attemptedCODAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);

                        cell = PhraseCell(new Phrase(String.Format("{0:N2}", attemptedRRRAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        cell.BorderColorBottom = BaseColor.BLACK;

                        cell.UseVariableBorders = true;
                        table_detail.AddCell(cell);
                    }

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
                #endregion

                #region Footer Row
                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;

                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase(totalConsignmentCount.ToString(), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);

                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);


                cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);


                cell = PhraseCell(new Phrase(String.Format("{0:N2}", totalCODAmount), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);



                cell = PhraseCell(new Phrase(String.Format("{0:N2}", totalRRAmount), FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                cell.Colspan = 1;
                cell.Padding = 2f;
                cell.BorderColorTop = BaseColor.BLACK;
                cell.BorderColorLeft = BaseColor.BLACK;
                cell.BorderColorRight = BaseColor.BLACK;
                cell.BorderColorBottom = BaseColor.BLACK;
                cell.UseVariableBorders = true;
                table_detail.AddCell(cell);
                document.Add(table_detail);
                #endregion

                #region Remaining HeaderRow


                DataTable remainingDt = GetRemainingConsignments(clvar);
                if (remainingDt.Rows.Count > 0)
                {
                    PdfPTable remainingCNTable = new PdfPTable(6);
                    remainingCNTable.TotalWidth = 550f;
                    remainingCNTable.SetWidths(new float[6] { 40f, 100f, 60f, 180f, 60f, 110f });
                    remainingCNTable.LockedWidth = true;

                    cell = PhraseCell(new Phrase("     ", FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 6;
                    cell.Padding = 2f;
                    //cell.BorderColorTop = BaseColor.BLACK;
                    //cell.BorderColorLeft = BaseColor.BLACK;
                    //cell.BorderColorRight = BaseColor.BLACK;
                    //cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("Remaining Consignments.", FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 6;
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

                    cell = PhraseCell(new Phrase("Consignment Number", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("Account #", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("Account Name", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("COD Amount", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("Delivery Status", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);
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

                        cell = PhraseCell(new Phrase(remainingDt.Rows[i]["CNNo"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        cell = PhraseCell(new Phrase(remainingDt.Rows[i]["AccountNumber"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        cell = PhraseCell(new Phrase(remainingDt.Rows[i]["AccName"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        double remainingCodAmount = 0;
                        double.TryParse(remainingDt.Rows[i]["CodAmount"].ToString(), out remainingCodAmount);
                        totalRemainingCODAmount += remainingCodAmount;
                        cell = PhraseCell(new Phrase(String.Format("{0:N2}", remainingCodAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);

                        cell = PhraseCell(new Phrase(remainingDt.Rows[i]["RunsheetReason"].ToString(), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                        cell.Colspan = 1;
                        cell.Padding = 2f;
                        //cell.BorderColorTop = BaseColor.BLACK;
                        cell.BorderColorLeft = BaseColor.BLACK;
                        cell.BorderColorRight = BaseColor.BLACK;
                        //cell.BorderColorBottom = BaseColor.BLACK;
                        cell.UseVariableBorders = true;
                        remainingCNTable.AddCell(cell);
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

                    cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase(String.Format("{0:N2}", totalRemainingCODAmount), FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_RIGHT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);

                    cell = PhraseCell(new Phrase("", FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)), PdfPCell.ALIGN_LEFT);
                    cell.Colspan = 1;
                    cell.Padding = 2f;
                    cell.BorderColorTop = BaseColor.BLACK;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderColorBottom = BaseColor.BLACK;
                    cell.UseVariableBorders = true;
                    remainingCNTable.AddCell(cell);
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
                string fileName = "" + clvar.RunsheetNumber + "_" + DateTime.Now.ToShortDateString();
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