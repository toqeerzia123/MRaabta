using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using MRaabta.App_Code;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class DSSP : System.Web.UI.Page
    {
        String branchCode, ZoneCode, ECCode, ExpressCName, UserName, U_ID, BookingCode_ = "", shift_ = "";
        Cl_Variables clvar = new Cl_Variables();
        static Cl_Variables clvarStatic = new Cl_Variables();
        String startDate = "2020-06-08";
        String VoidStartDate = "2020-12-01";
        String endDate = "getdate()";
        string previousPageUrl = "";
        string previousPageName = "";
        string oldDSSP = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            SysDate.Text = DateTime.Now.ToString();
            try
            {
                U_ID = Session["U_ID"].ToString();
                branchCode = Session["BRANCHCODE"].ToString();
                ZoneCode = Session["ZONECODE"].ToString();
                ECCode = Session["ExpressCenter"].ToString();
                ExpressCName = Session["EXPRESSCENTERNAME"].ToString();
                UserName = Session["NAME"].ToString();
                BookingCode_ = Session["BookingStaff"].ToString();
                shift_ = Session["retail_shift"].ToString();
                if (!IsPostBack)
                {
                    if (Request.QueryString["DSSPNo"] != null)
                    {
                        printBtn.Visible = true;
                        oldDSSP = Request.QueryString["DSSPNo"].ToString();
                        getZoneBranchDSSP_OLD(oldDSSP);
                        FillDataDSSPPage_OLD(oldDSSP);
                    }
                    else
                    {
                        printBtn.Visible = false;
                        getZoneBranch(ZoneCode, branchCode);
                        ExpressCenterCode.Text = ECCode.ToLower();
                        ECName.Text = ExpressCName.ToLower();
                        StaffName.Text = UserName.ToLower();
                        bookingCode.Text = BookingCode_.ToLower();
                        Shift.Text = shift_.ToUpper();
                        FillDSSPPage();
                    }
                }

            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }

        }
        public void getZoneBranchDSSP_OLD(String oldDSSP)
        {
            String zoneName = "", branchName = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());


            string sql = "SELECT mmrd.ZoneCode,z.name zoneName,mmrd.BranchCode,b.name branchName,mmrd.ExpressCenterCode,ec.name expressCenterName, \n"
               + "      mmrd.BookingCode,cnCount,mmrd.TotalAmount,isnull(mmrd.BookingShift,'') BookingShift,isnull(mmrd.VehicleNo,'') VehicleNo,mmrd.CreatedOn, zu.U_NAME CreatedBy  \n"
               + " From MNP_Master_Retail_DSSP mmrd \n"
               + " INNER JOIN Zones z ON z.ZoneCode = mmrd.ZoneCode \n"
               + " INNER JOIN Branches b ON b.BranchCode = mmrd.BranchCode \n"
               + " INNER JOIN ExpressCenters ec ON ec.expressCenterCode=mmrd.ExpressCenterCode \n"
               + " INNER JOIN ZNI_USER1 zu ON zu.U_ID=mmrd.CreatedBy \n"
               + " WHERE mmrd.DSSPNumber=" + oldDSSP + "";
            using (var cmd2 = new SqlCommand(sql, con))
            {
                con.Open();
                SqlDataReader rdr = cmd2.ExecuteReader();
                if (rdr.Read())
                {
                    Zone.Text = rdr.GetString(1).ToLower();
                    Branch.Text = rdr.GetString(3).ToLower();
                    ExpressCenterCode.Text = rdr.GetString(4).ToLower();
                    ECName.Text = rdr.GetString(5).ToLower();
                    bookingCode.Text = rdr.GetString(6).ToLower();
                    Shift.Text = rdr.GetString(9);
                    VehicleNo.Text = rdr.GetString(10);
                    PrintDate.Text = rdr.GetDateTime(11).ToString();
                    StaffName.Text = rdr.GetString(12).ToLower();
                    SysDate.Text = DateTime.Now.ToString();
                }
                rdr.Close();

            }
            con.Close();
        }
        public void FillDataDSSPPage_OLD(string oldDSSP)
        {
            try
            {
                generate.Enabled = false;
                AutoDSSPNO.Text = oldDSSP + " Duplicate";
                ConsignmentWiseTable_OLD(oldDSSP);
                VoidTable_OLD(oldDSSP);
                CashSalesSummmaryProductWiseTable_OLD(oldDSSP);
                ServiceWiseSummaryTable_OLD(oldDSSP);
                SummaryofInternationSalesTable_OLD(oldDSSP);
                ShipmentandAmountSummaryTable_OLD(oldDSSP);
            }
            catch (Exception er)
            {
                AutoDSSPNO.Text = "Error getting data";
            }
        }
        public void CashSalesSummmaryProductWiseTable_OLD(string oldDSSP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql__old = "SELECT x.Products, \n"
               + "       COUNT(DISTINCT x.shipmentQuantity) shipmentQuantity, \n"
               + "       SUM(CAST(x.weightSum AS DECIMAL(18, 2))) weightSum, \n"
               + "       SUM(CAST(x.piecesSum AS INT))     piecesSum, \n"
               + "       Round(SUM(x.chargedAmountSum),1)           chargedAmountSum \n"
               + "FROM   ( \n"
               + "           SELECT DISTINCT stn.Products, \n"
               + "                  mdrd.consignmentNumber     shipmentQuantity, \n"
               + "                  [weight]                   weightSum, \n"
               + "                  pieces                     piecesSum, \n"
               + "                  mdrd.chargedAmount         chargedAmountSum \n"
               + "           FROM   MNP_Detail_Retail_DSSP mdrd \n"
               + "                  INNER JOIN ServiceTypes_New stn \n"
               + "                       ON  stn.serviceTypeName = mdrd.ServiceType \n"
               // + "           WHERE  stn.Products != 'International' \n"
               + "                  AND mdrd.DSSPNumber = '" + oldDSSP + "' AND mdrd.CNStatus=1 \n"
               + "       )                                 x \n"
               + "GROUP BY \n"
               + "       x.Products";


                string sql______old = @"SELECT x.Products, 
                       COUNT(DISTINCT x.shipmentQuantity) shipmentQuantity, 
                       SUM(CAST(x.weightSum AS DECIMAL(18, 2))) weightSum, 
                       SUM(CAST(x.piecesSum AS INT))     piecesSum, 
                       Round(SUM(x.chargedAmountSum),1)           chargedAmountSum 
                FROM   ( 
                           SELECT DISTINCT stn.Products, 
                                  mdrd.consignmentNumber     shipmentQuantity, 
                                  [weight]                   weightSum, 
                                  pieces                     piecesSum, 
                                  mdrd.chargedAmount         chargedAmountSum 
                           FROM   Consignment  mdrd 
                                  INNER JOIN ServiceTypes_New stn 
                                       ON  stn.serviceTypeName = mdrd.serviceTypeName 
                           WHERE  --stn.Products != 'International' and
                                  mdrd.consignmentNumber IN (SELECT M.ConsignmentNumber FROM MNP_Detail_Retail_DSSP M WHERE M.DSSPNumber = '" + oldDSSP + @"')
                       )                                 x 
                GROUP BY 
                       x.Products";



                string sql = @" SELECT x.Products, 
    COUNT(DISTINCT x.shipmentQuantity) shipmentQuantity, 
    SUM(CAST(x.weightSum AS DECIMAL(18, 2))) weightSum, 
    SUM(CAST(x.piecesSum AS INT))     piecesSum, 
    SUM(Round(x.chargedAmountSum,0)) chargedAmountSum
FROM(
        SELECT DISTINCT stn.Products,
                C.consignmentNumber     shipmentQuantity,
                [weight]                   weightSum,
                C.pieces                     piecesSum,
/*
                CASE
                                 WHEN ISNULL(c.DiscountID, '0') = '0'
                     AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst
                         WHEN ISNULL(c.DiscountID, '0') != '0'
                     AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount
                         WHEN ISNULL(c.DiscountID, '0') = '0'
                     AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN
                         c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)
                         WHEN ISNULL(c.DiscountID, '0') != '0'
                     AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN
                         c.chargedAmount + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)
                         END chargedAmountSum
*/
                c.chargedAmount chargedAmountSum
        FROM   Consignment  C
                INNER JOIN ServiceTypes_New stn ON  stn.serviceTypeName = C.serviceTypeName
                LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber = C.consignmentNumber
        WHERE
                C.consignmentNumber IN(SELECT M.ConsignmentNumber FROM MNP_Detail_Retail_DSSP M WHERE M.DSSPNumber = '" + oldDSSP + @"'  AND M.CNStatus=1)
        GROUP BY

            stn.Products,
            C.consignmentNumber,

            [weight],
C.pieces, c.chargedAmount, c.totalAmount, c.gst, DiscountID
    )                                 x
GROUP BY
    x.Products";

                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;
                    object TotalWeight = 0;
                    object TotalPcs = 0;
                    object TotalAmount = 0;

                    TotalQuantity = dt.Compute("Sum(shipmentQuantity)", string.Empty);
                    TotalWeight = dt.Compute("Sum(weightSum)", string.Empty);
                    TotalPcs = dt.Compute("Sum(piecesSum)", string.Empty);
                    TotalAmount = dt.Compute("Sum(chargedAmountSum)", string.Empty);
                    dt.Rows.Add("Total", TotalQuantity, TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmount)));

                    CreateCashSalesProductView(dt);
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        public void ServiceWiseSummaryTable_OLD(string oldDSSP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {

                string sql___ = "SELECT mdrd.ServiceType serviceTypeName,COUNT(mdrd.ServiceType)  quantity \n"
               + "       FROM MNP_Detail_Retail_DSSP mdrd \n"
               + "       INNER JOIN ServiceTypes_New stn  \n"
               + "                  ON  stn.serviceTypeName = mdrd.ServiceType \n"
               + "       WHERE mdrd.DSSPNumber=" + oldDSSP + " \n"
               + "       AND stn.Products!='International' \n"
               + "       GROUP BY mdrd.ServiceType";

                string sql = "SELECT serviceTypeName, count(quantity) quantity FROM ( \n"
               + "SELECT mdrd.ServiceType serviceTypeName,COUNT( distinct mdrd.ServiceType)  quantity , mdrd.ConsignmentNumber \n"
               + "               FROM MNP_Detail_Retail_DSSP mdrd  \n"
               + "               INNER JOIN ServiceTypes_New stn   \n"
               + "                          ON  stn.serviceTypeName = mdrd.ServiceType  \n"
               + "               WHERE mdrd.DSSPNumber=" + oldDSSP + "  AND mdrd.CNStatus=1\n"
               // + "               AND stn.Products!='International'  \n"
               + "               GROUP BY mdrd.ServiceType, mdrd.ConsignmentNumber \n"
               + ") x \n"
               + "GROUP BY \n"
               + "serviceTypeName";

                using (var cmd2 = new SqlCommand(sql, con))
                {

                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;

                    TotalQuantity = dt.Compute("Sum(quantity)", string.Empty);
                    dt.Rows.Add("Total Shipments", TotalQuantity);

                    CreateServiceWiseSummaryView(dt);
                }

            }

            catch (Exception ex)
            {
                con.Close();
            }
        }
        public void SummaryofInternationSalesTable_OLD(string oldDSSP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = "SELECT mdrd.ServiceType,COUNT(mdrd.ServiceType) Quantity,SUM(round(mdrd.ChargedAmount,0)) Amount \n"
               + "FROM   MNP_Detail_Retail_DSSP mdrd \n"
               + "       INNER JOIN ServiceTypes_New stn \n"
               + "		ON  stn.serviceTypeName = mdrd.ServiceType  \n"
               + "WHERE mdrd.DSSPNumber=" + oldDSSP + " \n"
               + "	  AND stn.Products = 'International'  AND mdrd.CNStatus=1 \n"
               + "GROUP BY mdrd.ServiceType \n"
               + "                       ";
                using (var cmd2 = new SqlCommand(sql, con))
                {

                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;
                    object Amount = 0;

                    TotalQuantity = dt.Compute("Sum(Quantity)", string.Empty);
                    Amount = dt.Compute("Sum(Amount)", string.Empty);

                    dt.Rows.Add("Total Intl", TotalQuantity, Math.Round(Convert.ToDecimal(Amount)));
                    SummaryofInternationSalesTableView(dt);
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        public void ShipmentandAmountSummaryTable_OLD(string oldDSSP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql__ = "SELECT products, \n"
               + "       ROUND(SUM(x.ChargedAmount), 2)     chargedAmountSum \n"
               + "FROM   ( \n"
               + "           SELECT DISTINCT stn.products, \n"
               + "                  mdrd.ConsignmentNumber, \n"
               + "                  mdrd.ChargedAmount \n"
               + "           FROM   MNP_Detail_Retail_DSSP mdrd \n"
               + "                  INNER JOIN ServiceTypes_New stn \n"
               + "                       ON  stn.serviceTypeName = mdrd.ServiceType \n"
               + "           WHERE  mdrd.DSSPNumber = " + oldDSSP + " AND mdrd.CNStatus=1 \n"
               + "       )                                  x \n"
               + "GROUP BY \n"
               + "       x.Products";


                string sql = @"SELECT CASE WHEN PaymentMode = 'CASH' THEN d.products + ' (TOTAL)'        ELSE 
                                d.products + ' (' + PaymentMode + ')' 
                                END product,
                                SUM(ROUND(d.[GrossAmount],0)) chargedAmountSum,
                                PaymentMode, Id
           FROM   ( 
                      SELECT c.consignmentNumber, 
                             c.serviceTypeName, 
                             c.weight, 
                             c.pieces, 
                             c.chargedAmount, c.chargedAmount GrossAmount,
           /*
                              CASE  
           					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst 
           					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount 
           					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  
           						 c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  
           					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  
           						c.chargedAmount + SUM(cm.calculatedValue) + SUM(cm.calculatedGST) 
                             END GrossAmount,  
                             */

                              stn.products, ps.name PaymentMode, ps.Id
                            /* CASE  
                                         WHEN C.PaymentMode = '1' THEN 'CASH' 
                                         WHEN C.PaymentMode = '6' THEN 'QR CODE' 
                                         WHEN C.PaymentMode = '7' THEN 'CREDIT CARD' 
                                     END PaymentMode */
                      FROM   Consignment c 
							 INNER JOIN ServiceTypes_New stn ON  stn.serviceTypeName = c.serviceTypeName 
                             LEFT JOIN ConsignmentModifier cm  ON  cm.consignmentNumber = c.consignmentNumber 
                             LEFT JOIN PaymentSource AS ps ON c.PaymentMode = ps.Id
                      WHERE  
                      C.consignmentNumber IN (SELECT M.ConsignmentNumber FROM MNP_Detail_Retail_DSSP M WHERE M.DSSPNumber = '" + oldDSSP + @"' AND M.CNStatus=1)

                      GROUP BY 
                             c.consignmentNumber, 
                             c.serviceTypeName, 
                             c.weight,c.DiscountID, 
                             c.pieces, 
                             c.chargedAmount, 
                             c.totalAmount, 
                             c.gst, 
                             stn.products,C.PaymentMode ,ps.name, ps.Id
                  )                        d 
           GROUP BY 
                  d.products, PaymentMode, Id";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;
                    double QRCode = 0;
                    double cash = 0;
                    double cashTotal = 0;
                    double creditCard = 0;
                    int cashRow = 0;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow Currentdr = dt.Rows[j];
                        if (Currentdr["PaymentMode"].ToString() != "Cash")
                        {
                            QRCode = float.Parse(Currentdr["chargedAmountSum"].ToString());
                        }
                        if (Currentdr["PaymentMode"].ToString() == "Cash")
                        {
                            cash = float.Parse(Currentdr["chargedAmountSum"].ToString());
                            cashTotal += float.Parse(Currentdr["chargedAmountSum"].ToString());
                            cashRow = j;
                        }
                        if (Currentdr["PaymentMode"].ToString() != "Cash")
                        {
                            creditCard = float.Parse(Currentdr["chargedAmountSum"].ToString());
                        }
                    }

                    TotalQuantity = cashTotal;
                    dt.Rows.Add("Total Amount (Less Credit card and QR code amount)", Math.Round(Convert.ToDecimal(TotalQuantity)));
                    ShipmentAndAmountSummaryView(dt);
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        public void getZoneBranch(String ZoneCode, String branchCode)
        {
            String zoneName = "", branchName = "";
            SqlConnection con = new SqlConnection(clvar.Strcon());

            String sql = " SELECT z.name zone,b.name branch FROM Branches b " +
                " INNER JOIN Zones z ON z.zoneCode = b.zoneCode  " +
                " WHERE b.branchCode =" + branchCode;
            using (var cmd2 = new SqlCommand(sql, con))
            {
                con.Open();
                SqlDataReader rdr = cmd2.ExecuteReader();
                if (rdr.Read())
                {
                    zoneName = rdr.GetString(0);
                    branchName = rdr.GetString(1);
                }
                rdr.Close();

            }
            con.Close();
            Zone.Text = zoneName.ToLower();
            Branch.Text = branchName.ToLower();
        }
        protected void FillDSSPPage()
        {
            try
            {

                ShipmentandAmountSummaryTable();
                TableWithSumAndCount_QA model = ConsignmentWiseTable();
                if (model.dt.Rows.Count > 0)
                {
                    generate.Enabled = true;
                    CashSalesSummmaryProductWiseTable();
                    ServiceWiseSummaryTable();
                    SummaryofInternationSalesTable();
                    ShipmentandAmountSummaryTable();
                }
                VoidConsignmentTable();
            }
            catch (Exception er)
            {
                AutoDSSPNO.Text = "Error getting data";
            }
        }
        private void ShipmentandAmountSummaryTable()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = "SELECT -------DSSP shipment and amount summary \n"
               + "        \n"
               + " CASE \n"
               + " WHEN PaymentMode = 'CASH' AND ID = '999' THEN d.products\n"
               + " WHEN PaymentMode = 'CASH' THEN d.products + ' (TOTAL)'\n"
               + " ELSE d.products + ' (' + PaymentMode + ')'\n"
               + " END product,   \n"
               + " ISNULL(SUM(ROUND(d.[GrossAmount],0)),0) chargedAmountSum,PaymentMode, Id \n"
               + "FROM   ( \n"
               + "           SELECT c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"

                //+ "                   ROUND(   CASE  \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
                //+ "						 c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
                //+ "						c.chargedAmount --+ SUM(cm.calculatedValue) + SUM(cm.calculatedGST) \n"
                //+ "                  END,0) GrossAmount,  \n"
                + "                  c.chargedAmount GrossAmount,  \n"
                + "                  stn.products, \n"
               //+ "                  CASE  \n"
               //+ "                              WHEN C.PaymentMode = '1' THEN 'CASH' \n"
               //+ "                              WHEN C.PaymentMode = '6' THEN 'QR CODE' \n"
               //+ "                              WHEN C.PaymentMode = '7' THEN 'CREDIT CARD' \n"
               //+ "                          END PaymentMode \n"
               + "                  ps.Name PaymentMode, ps.Id \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN ServiceTypes_New stn \n"
               + "                       ON  stn.serviceTypeName = c.serviceTypeName \n"
               + "                  LEFT JOIN ConsignmentModifier cm \n"
               + "                       ON  cm.consignmentNumber = c.consignmentNumber \n"
               + "                  LEFT JOIN PaymentSource AS ps ON c.PaymentMode = ps.Id \n"
               + "           WHERE  c.orgin = '" + branchCode + "' \n"
               + "                  AND c.originExpressCenter ='" + ECCode + "' \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'               \n"
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date) \n"
               + "                  AND ISNULL(c.status, 0) != '9' \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2' \n"
               + "                  AND ISNULL(c.isApproved, 0) = '0' \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1' \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0' \n"
               + "                  AND c.consignerAccountNo = '0' \n"
               + "           GROUP BY \n"
               + "                  c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight,c.DiscountID, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"
               + "                  c.totalAmount, \n"
               + "                  c.gst, \n"
               + "                  stn.products,C.PaymentMode, ps.Name, ps.Id  \n"
               + "           UNION ALL \n\n"/*
               + "           -- DISCOUNTED CN BUT APPLY DISCOUNT BUTTON \n"
               + "           SELECT c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"

               //+ "                   ROUND(   CASE  \n"
               //+ "					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst \n"
               //+ "					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount \n"
               //+ "					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "						 c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
               //+ "					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "						c.chargedAmount --+ SUM(cm.calculatedValue) + SUM(cm.calculatedGST) \n"
               //+ "                  END,0) GrossAmount,  \n"
                + "                  c.chargedAmount GrossAmount,  \n"
                + "                  stn.products, \n"
               //+ "                  CASE  \n"
               //+ "                              WHEN C.PaymentMode = '1' THEN 'CASH' \n"
               //+ "                              WHEN C.PaymentMode = '6' THEN 'QR CODE' \n"
               //+ "                              WHEN C.PaymentMode = '7' THEN 'CREDIT CARD' \n"
               //+ "                          END PaymentMode \n"
               + "                  ps.Name PaymentMode, ps.Id \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN ServiceTypes_New stn \n"
               + "                       ON  stn.serviceTypeName = c.serviceTypeName \n"
               + "                  LEFT JOIN ConsignmentModifier cm \n"
               + "                       ON  cm.consignmentNumber = c.consignmentNumber \n"
               + "                  LEFT JOIN PaymentSource AS ps ON c.PaymentMode = ps.Id \n"
               + "           WHERE  c.orgin = '" + branchCode + "' \n"
               + "                  AND c.originExpressCenter ='" + ECCode + "' \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'               \n"
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date) \n"
               + "                  AND ISNULL(c.status, 0) = '9' \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2' \n"
               + "                  AND ISNULL(c.isApproved, 0) = '0' \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '0' \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0' \n"
               + "                  AND c.consignerAccountNo = '0' \n"
               + "           GROUP BY \n"
               + "                  c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight,c.DiscountID, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"
               + "                  c.totalAmount, \n"
               + "                  c.gst, \n"
               + "                  stn.products,C.PaymentMode, ps.Name, ps.Id  \n"
               + "          UNION ALL \n\n"*/
               + "           SELECT c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"
                //+ "                   ROUND(   CASE  \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
                //+ "						 c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
                //+ "					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
                //+ "						c.chargedAmount --+ SUM(cm.calculatedValue) + SUM(cm.calculatedGST) \n"
                //+ "                  END,0) GrossAmount,  \n"
                + "                  c.chargedAmount GrossAmount,  \n"
                + "                  stn.products, \n"
               //+ "                  CASE  \n"
               //+ "                              WHEN C.PaymentMode = '1' THEN 'CASH' \n"
               //+ "                              WHEN C.PaymentMode = '6' THEN 'QR CODE' \n"
               //+ "                              WHEN C.PaymentMode = '7' THEN 'CREDIT CARD' \n"
               //+ "                          END PaymentMode \n"
               + "                  ps.Name PaymentMode, ps.Id \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN ServiceTypes_New stn \n"
               + "                       ON  stn.serviceTypeName = c.serviceTypeName \n"
               + "                  LEFT JOIN ConsignmentModifier cm \n"
               + "                       ON  cm.consignmentNumber = c.consignmentNumber \n"
               + "                  LEFT JOIN PaymentSource AS ps ON c.PaymentMode = ps.Id \n"
               + "           WHERE  c.orgin = '" + branchCode + "' \n"
               + "                  AND c.originExpressCenter ='" + ECCode + "' \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'               \n"
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date) \n"
               + "                  AND ISNULL(c.status, 0) != '9' \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2' \n"
               + "                  AND ISNULL(c.isApproved, 0) = '0' \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1' \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0' \n"
               + "                  AND ISNULL(c.cod, 0) = '1' \n"
               + "                  AND c.consignerAccountNo != '0' \n"
               + "           GROUP BY \n"
               + "                  c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight,c.DiscountID, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"
               + "                  c.totalAmount, \n"
               + "                  c.gst, \n"
               + "                  stn.products,C.PaymentMode, ps.Name, ps.Id  \n"
               + "          UNION   \n"
               + "          --FOR RIDER SUBMIT AMOUNT IN EXPRESS CENTER \n"
               + "          SELECT NULL consignmentNumber, \n"
               + "          NULL serviceTypeName,\n"
               + "          NULL weight, \n"
               + "          NULL pieces,\n"
               + "          rc.CollectedAmount chargedAmount,\n"
               + "          rc.CollectedAmount GrossAmount,  \n"
               + "          RC.RiderName products,\n"
               + "          'Cash' PaymentMode, '999' Id\n"
               + "          FROM tbl_RiderCashPayment rc\n"
               + "          INNER JOIN ExpressCenters AS ec ON rc.EcCode = EC.expressCenterCode\n"
               + "          WHERE ec.expressCenterCode = '" + ECCode + "' \n"
               + "          AND rc.CreatedBy = '" + Session["U_ID"].ToString() + "' AND rc.DSSP = '0' \n"
               + "          AND CAST(rc.CreatedOn AS date) = CAST(getdate() AS date)\n"
               + "          GROUP BY\n"
               + "          rc.CollectedAmount , RC.RiderName,RC.RiderCode \n"
               + "       )                        d \n"
               + "GROUP BY \n"
               + "       d.products, PaymentMode, Id \n";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    //   cmd2.CommandTimeout = 600;
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;
                    double QRCode = 0;
                    double cash = 0;
                    double cashTotal = 0;
                    double creditCard = 0;
                    int cashRow = 0;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow Currentdr = dt.Rows[j];
                        if (Currentdr["PaymentMode"].ToString() != "Cash")
                        {
                            QRCode = float.Parse(Currentdr["chargedAmountSum"].ToString());
                        }
                        if (Currentdr["PaymentMode"].ToString() == "Cash")
                        {
                            cash = float.Parse(Currentdr["chargedAmountSum"].ToString());
                            cashTotal += float.Parse(Currentdr["chargedAmountSum"].ToString());
                            cashRow = j;
                        }
                        if (Currentdr["PaymentMode"].ToString() != "Cash")
                        {
                            creditCard = float.Parse(Currentdr["chargedAmountSum"].ToString());
                        }
                    }

                    TotalQuantity = cashTotal;
                    dt.Rows.Add("Total Amount (Less Credit card and QR code amount)", Math.Round(Convert.ToDecimal(TotalQuantity)));
                    ShipmentAndAmountSummaryView(dt);
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        private void SummaryofInternationSalesTable()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = "------DSSP summary of international fedex  \n"
                    + "   ---- " + Session["U_NAME"] + " \n"
                    + " SELECT d.serviceTypeName, \n"
                    + "       COUNT(d.serviceTypeName)     Quantity, \n"
                    + "       SUM(d.chargedAmount)         Amount \n"
                    + "FROM   ( \n"
                    + "           SELECT c.pieces, \n"
                    + "                  c.serviceTypeName, \n"
                    + "                  stn.Products, \n"
                    + "                  c.chargedAmount \n"
                    + "           FROM   Consignment c \n"
                    + "                  INNER JOIN ServiceTypes_New stn \n"
                    + "                       ON  stn.serviceTypeName = c.serviceTypeName \n"
                    + "           WHERE  c.orgin = '" + branchCode + "' \n"
                    + "                  AND c.originExpressCenter = '" + ECCode + "' \n"
                    + "                  AND c.riderCode='" + BookingCode_ + "'                                                                  "
                    + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
                    + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date) \n"
                    + "                  AND ISNULL(c.status, 0) !='9'  \n"
                    + "                AND ISNULL(c.InsertType, 0) = '2' "
                    + "                  AND ISNULL(c.isApproved, 0) = '0' \n"
                    + "                  AND ISNULL(c.isPriceComputed, 0) = '1' \n"
                    + "                  AND ISNULL(c.misRouted, 0) = '0' \n"
                    + "                  AND c.consignerAccountNo = '0' \n"
                    + "                  AND stn.Products = 'International' \n"
                    + "       )                            d \n"
                    + "GROUP BY \n"
                    + "       d.serviceTypeName";


                using (var cmd2 = new SqlCommand(sql, con))
                {

                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;
                    object Amount = 0;

                    TotalQuantity = dt.Compute("Sum(Quantity)", string.Empty);
                    Amount = dt.Compute("Sum(Amount)", string.Empty);

                    dt.Rows.Add("Total Intl", TotalQuantity, Amount);
                    SummaryofInternationSalesTableView(dt);
                }



            }

            catch (Exception ex)
            {
                con.Close();
            }
        }
        private void ServiceWiseSummaryTable()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {

                string sql = " ----DSSP Service wise summary  \n"
                + "   ---- " + Session["U_NAME"] + " \n"
               + "  SELECT d.serviceTypeName, \n"
               + "       COUNT(d.serviceTypeName)     quantity \n"
               + "FROM   ( \n"
               + "           SELECT c.pieces, \n"
               + "                  c.createdOn, \n"
               + "                  CASE  \n"
               + "                       WHEN c.[status] = 9 THEN 'VOID' \n"
               + "                       ELSE c.serviceTypeName \n"
               + "                  END AS serviceTypeName \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN ServiceTypes_New stn  ON  stn.serviceTypeName = c.serviceTypeName \n"
               + "           WHERE  c.orgin = '" + branchCode + "' \n"
               + "                  AND c.originExpressCenter = '" + ECCode + "' \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'              \n   "
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date) \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2'   \n"
               + "                  AND ISNULL(c.status, 0) !='9'  \n"
               + "                  AND ISNULL(c.isApproved, 0) = '0' \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1' \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0' \n"
               + "                  AND c.consignerAccountNo = '0' \n"
               + "           UNION ALL \n"/*
               + "           --DISCOUNTED CN BUT NOT APPLY DISCOUNT BUTTON \n"
               + "           SELECT c.pieces, \n"
               + "                  c.createdOn, \n"
               + "                  c.serviceTypeName \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN ServiceTypes_New stn  ON  stn.serviceTypeName = c.serviceTypeName \n"
               + "           WHERE  c.orgin = '" + branchCode + "' \n"
               + "                  AND c.originExpressCenter = '" + ECCode + "' \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'              \n   "
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date) \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2'   \n"
               + "                  AND ISNULL(c.status, 0) ='9'  \n"
               + "                  AND ISNULL(c.isApproved, 0) = '0' \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '0' \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0' \n"
               + "                  AND c.consignerAccountNo = '0' \n"
               + "           UNION ALL \n\n"*/
               + "           SELECT c.pieces, \n"
               + "                  c.createdOn, \n"
               + "                  CASE  \n"
               + "                       WHEN c.[status] = 9 THEN 'VOID' \n"
               + "                       ELSE c.serviceTypeName \n"
               + "                  END AS serviceTypeName \n"
               + "           FROM   Consignment c \n"
               + "                  INNER JOIN ServiceTypes_New stn  ON  stn.serviceTypeName = c.serviceTypeName \n"
               + "           WHERE  c.orgin = '" + branchCode + "' \n"
               + "                  AND c.originExpressCenter = '" + ECCode + "' \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'              \n   "
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date) \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2'   \n"
               + "                  AND ISNULL(c.status, 0) !='9'  \n"
               + "                  AND ISNULL(c.isApproved, 0) = '0' \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1' \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0' \n"
               + "                  AND ISNULL(c.cod, 0) = '1' \n"
               + "                  AND c.consignerAccountNo != '0' \n"
               + "       )                            d \n"
               + "GROUP BY \n"
               + "       d.serviceTypeName ";




                using (var cmd2 = new SqlCommand(sql, con))
                {

                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;

                    TotalQuantity = dt.Compute("Sum(quantity)", string.Empty);
                    dt.Rows.Add("Total Shipments", TotalQuantity);

                    CreateServiceWiseSummaryView(dt);
                }


            }

            catch (Exception ex)
            {
                con.Close();
            }
        }
        private void ConsignmentWiseTable_OLD(string oldDSSP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                #region old_query_22062020
                // string sql = "  SELECT mdrd.ConsignmentNumber consignmentNumber, \n"
                //+ "       convert(varchar,mdrd.BookingDate, 103) bookingdateVarchar, \n"
                //+ "       mdrd.ServiceType ServiceType , \n"
                //+ "       mdrd.Destination destination, \n"
                //+ "       cast(mdrd.[Weight] AS float) [Weight], \n"
                //+ "       Cast(mdrd.Pieces AS INT) pieces, \n"
                //+ "       Round(CAST(mdrd.GrossAmount AS FLOAT),2) GrossAmount, \n"
                //+ "       Round(CAST(mdrd.ChargedAmount AS FLOAT),2) chargedAmount, \n"
                //+ "       Round(CAST(mdrd.ShipmentDiscount AS FLOAT),2) ShipmentDiscount, \n"
                //+ "       pm.name SuppService, \n"
                //+ "       Round(CAST(mdrd.SupplementaryCharges AS FLOAT),2) SuppCharges, \n"
                //+ "       Round(CAST(mdrd.FranchiseComission AS FLOAT),2) FranchiseComission, \n"
                //+ "       ps.name   [paymentMethod],  \n"
                //+ "       Round(CAST(mdrd.CalculatedIncentive AS FLOAT),2) CalculatedIncentive, \n"
                //+ "       Round(CAST(mdrd.ChargedAmount AS FLOAT),2) [AmountCollect], \n"
                //+ "       mdrd.GST, \n"
                //+ "       mdrd.BookingDate, mdrd.SupplementaryId  SuppserviceID  \n"
                //+ " FROM   MNP_Detail_Retail_DSSP mdrd \n"
                //+ " left JOIN PriceModifiers pm ON pm.id=mdrd.SupplementaryId  \n"
                //+ "      inner join Consignment c on c.ConsignmentNumber=mdrd.ConsignmentNumber \n "
                //+ "       INNER JOIN Paymentsource ps ON ps.Id=c.PaymentMode \n "
                //+ "                   AND ps.booking = '1'  \n"
                //+ "                          AND ps.STATUS = '1'   \n"
                //+ "  WHERE  DSSPNumber = " + oldDSSP + " \n"
                //+ "  order by mdrd.ConsignmentNumber \n";
                #endregion

                string sql = @"SELECT --old dssp main table  
                       d.consignmentNumber,   
                       d.bookingdateVarchar,   
                       d.ServiceType,   
                       d.destination,   
                       Cast(d.weight as decimal(18,2)) as weight,   
                       d.pieces,   
                       Round(cast(d.GrossAmount as decimal(18,0)),0) as GrossAmount ,    
                       Round(d.chargedAmount,0) chargedAmount,   
                        Round((d.shipmentDiscount),2) ShipmentDiscount,         
                       d.SuppService,   
                       isnull(Round((d.SuppCharges+d.SuppGst),2),0) SuppCharges,   
                       d.FranchiseComission,   
                       d.paymentMethod ,       d.CalculatedIncentive,   
                       Round(((d.chargedAmount - d.FranchiseComission)+isnull(d.SuppCharges,0)+isnull(d.SuppGst,0)),0) [AmountCollect],    
                       d.GST,   
                       d.bookingDate ,  d.SuppserviceID   ,d.CNStatus,d.PaymentMode
                 FROM   (   
                           SELECT distinct c.consignmentNumber,   
                                  CONVERT(VARCHAR, c.bookingDate, 103) bookingdateVarchar,   
                                  c.serviceTypeName ServiceType,   
                                  b.sname     destination,   
                                  c.weight,   
                                  c.pieces,   
                                  c.totalAmount [GrossAmount],   
                                 -- CASE WHEN isnull(c.DiscountID,'0') = '0' THEN c.totalamount + c.gst 
                                 -- ELSE c.chargedAmount END  chargedAmount,   
                                  c.chargedAmount,
                                 CASE    
						                WHEN dm.DiscountValueType = 1 THEN dm.DiscountValue     
					                ELSE  
						                dm.DiscountValue 
					                END  AS [ShipmentDiscount], 
                   
                                  pm.name [SuppService],  ps.name ,  
                                  cm.calculatedValue [SuppCharges], cm.calculatedGST [SuppGst],   
                                  0 [FranchiseComission],   
                                  ps.name   [paymentMethod],   
                                  0 [CalculatedIncentive],   
                                  c.gst [GST],   
                                  c.bookingDate ,  pm.id SuppserviceID, isnull(c.DiscountID,'0') DiscountID     ,mdrd.CNStatus,mdrd.PaymentMode
                           FROM   MNP_Detail_Retail_DSSP mdrd
                                  INNER JOIN Consignment c
                                       ON  c.ConsignmentNumber = mdrd.ConsignmentNumber
                                  INNER JOIN Paymentsource ps
                                       ON  ps.Id = c.PaymentMode
                                       AND ps.booking = '1'
                                       AND ps.STATUS = '1'
                                  INNER JOIN Branches b
                                       ON  b.branchCode = c.destination
                                  LEFT JOIN ConsignmentModifier cm
                                       ON  cm.consignmentNumber = c.consignmentNumber
                                  LEFT JOIN MNP_DiscountConsignment dc
                                       ON  c.consignmentNumber = dc.ConsignmentNumber
                                  LEFT JOIN PriceModifiers pm
                                       ON  pm.id = cm.priceModifierId
                                  LEFT JOIN MnP_MasterDiscount dm
                                       ON  dc.DiscountID = dm.DiscountID
                           WHERE  DSSPNumber = " + oldDSSP + @"  AND mdrd.CNStatus=1
                       )                                    d
                ORDER BY
                       d.ConsignmentNumber ";

                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    float suppCharges = 0;
                    float chargedAmount = 0;
                    float FranchiseComission = 0;
                    for (int j = 0; j < dt.Rows.Count - 1; j++)
                    {
                        DataRow Currentdr = dt.Rows[j];
                        DataRow dr = dt.Rows[j + 1];
                        if (Currentdr["consignmentNumber"].ToString() == dr["consignmentNumber"].ToString())
                        {
                            String CurrentdrRowService = Currentdr["SuppService"].ToString();
                            String drService = dr["SuppService"].ToString();

                            float CurrentdrRowServicePrice = float.Parse(Currentdr["SuppCharges"].ToString());
                            float drServicePrice = float.Parse(dr["SuppCharges"].ToString());

                            Currentdr["SuppService"] = CurrentdrRowService + ", " + drService;
                            Currentdr["SuppCharges"] = (CurrentdrRowServicePrice + drServicePrice);

                            suppCharges = CurrentdrRowServicePrice + drServicePrice;
                            dr.Delete();
                            dt.AcceptChanges();
                            --j;
                        }
                        else
                        {
                            suppCharges = float.Parse(Currentdr["SuppCharges"].ToString());
                        }
                        chargedAmount = float.Parse(Currentdr["chargedAmount"].ToString());
                        FranchiseComission = float.Parse(Currentdr["FranchiseComission"].ToString());
                        //suppGST = float.Parse(Currentdr["SuppCharges"].ToString());
                        Currentdr["AmountCollect"] = chargedAmount - FranchiseComission;
                        //Currentdr["AmountCollect"] = (chargedAmount + suppCharges) - FranchiseComission;
                    }

                    dt.AcceptChanges();

                    object TotalWeight = 0;
                    object TotalPcs = 0;
                    object TotalAmountGross = 0;
                    object TotalAmountCharge = 0;
                    object TotalFranchiseComission = 0;
                    object TotalAmountCollect = 0;
                    object TotalSupplementaryCharges = 0;
                    object TotalCalculatedIncentive = 0;
                    object TotalShipmentDiscount = 0;

                    TotalWeight = dt.Compute("Sum(weight)", string.Empty);
                    TotalPcs = dt.Compute("Sum(pieces)", string.Empty);
                    TotalAmountGross = dt.Compute("Sum(GrossAmount)", string.Empty);
                    TotalAmountCharge = dt.Compute("Sum(chargedAmount)", string.Empty);
                    TotalFranchiseComission = dt.Compute("Sum(FranchiseComission)", string.Empty);
                    TotalAmountCollect = dt.Compute("Sum(AmountCollect)", string.Empty);
                    TotalSupplementaryCharges = dt.Compute("Sum(SuppCharges)", string.Empty);
                    TotalShipmentDiscount = dt.Compute("Sum(ShipmentDiscount)", string.Empty);
                    if (TotalShipmentDiscount == DBNull.Value)
                    {
                        if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    else
                    {
                        if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    TotalCalculatedIncentive = dt.Compute("Sum(CalculatedIncentive)", string.Empty);
                    CreateTableView(dt);

                }
            }
            catch (Exception er)
            {

            }
        }
        private TableWithSumAndCount_QA ConsignmentWiseTable()
        {
            TableWithSumAndCount_QA response = new TableWithSumAndCount_QA();

            DataTable dt = new DataTable();
            DataTable dt_to_generate = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = "SELECT --NORMAL DSSP \n"
               + "       d.consignmentNumber,   \n"
               + "       d.bookingdateVarchar,   \n"
               + "       d.ServiceType,   \n"
               + "       d.destination,   \n"
               + "       Cast(d.weight as decimal(18,2)) as weight,   \n"
               + "       d.pieces,   \n"
               + "       Round(cast(d.GrossAmount as decimal(18,2)),0) as GrossAmount ,    \n"
               + "       Round(d.chargedAmount,0) chargedAmount,   \n"
               //+ "       Round((ISNULL(d.shipmentDiscount,'0')),0) ShipmentDiscount,         \n"
               + "       d.shipmentDiscount ShipmentDiscount,         \n"
               + "       d.SuppService,   \n"
               + "       isnull(Round((d.SuppCharges+d.SuppGst),2),0) SuppCharges,   \n"
               + "       d.FranchiseComission,   \n"
               + "       d.paymentMethod ,       d.CalculatedIncentive,   \n"
               // + "       Round(((d.chargedAmount - d.FranchiseComission)+isnull(d.SuppCharges,0)+isnull(d.SuppGst,0)),0) [AmountCollect],    \n"
               + "       Round(((d.chargedAmount - d.FranchiseComission)),0) [AmountCollect],    \n"
               + "       d.GST,   \n"
               + "       d.bookingDate ,  d.SuppserviceID,'1' CNStatus,d.PaymentMode, DiscountGST   \n"
               + " FROM   (   \n"
               + "           SELECT c.consignmentNumber,   \n"
               + "                  CONVERT(VARCHAR, c.bookingDate, 103) bookingdateVarchar,   \n"
               + "                  c.serviceTypeName ServiceType,   \n"
               + "                  b.sname     destination,   \n"
               + "                  c.weight,   \n"
               + "                  c.pieces,   \n"
               + "                  c.totalAmount [GrossAmount],   \n"
               //+ "                  CASE WHEN isnull(c.DiscountID,'0') = '0' THEN c.totalamount + c.gst \n"
               //+ "                  ELSE c.chargedAmount END  chargedAmount,   \n"
               + " c.chargedAmount, \n"
               + "                  --CASE    \n"
               + "                  --     WHEN dm.DiscountValueType = 1 THEN c.chargedAmount * (dm.DiscountValue / 100)+c.gst   \n"
               + "                  --     ELSE c.chargedAmount - (dm.DiscountValue / 100)+c.gst   \n"
               + "                  --END      AS [ShipmentDiscount],  \n"
               + "                  -- \n"
               + "                  CASE    \n"
               // + "						WHEN dm.DiscountValueType = 1 THEN (c.totalAmount + c.gst) - c.chargedAmount     \n"
               + "						WHEN dm.DiscountValueType = 1 THEN dm.DiscountValue    \n"
               + "					ELSE  \n"
               + "						dm.DiscountValue \n"
               + "					END \n"
               + "					AS [ShipmentDiscount], \n"
               + "                   \n"
               + "                  pm.name [SuppService],  ps.name ,  \n"
               + "                  cm.calculatedValue [SuppCharges], cm.calculatedGST [SuppGst],   \n"
               + "                  0 [FranchiseComission],   \n"
               + "                  ps.name   [paymentMethod],   \n"
               + "                  0 [CalculatedIncentive],   \n"
               + "                  c.gst [GST],   \n"
               + "                  c.bookingDate ,  pm.id SuppserviceID, isnull(c.DiscountID,'0') DiscountID ,c.PaymentMode, c.DiscountGST     \n"
               + "           FROM   Consignment c   \n"
               + "                  INNER JOIN Branches b ON  b.branchCode = c.destination   \n"
               + "                  INNER JOIN Paymentsource ps ON ps.Id=c.PaymentMode AND ps.booking = '1' AND ps.STATUS = '1'    \n"
               + "                  LEFT JOIN ConsignmentModifier cm ON  cm.consignmentNumber = c.consignmentNumber   \n"
               + "                  LEFT JOIN MNP_DiscountConsignment dc ON  c.consignmentNumber = dc.ConsignmentNumber   \n"
               + "                  LEFT JOIN PriceModifiers pm ON pm.id  =cm.priceModifierId    \n"
               + "                  LEFT JOIN MnP_MasterDiscount dm ON  dc.DiscountID = dm.DiscountID   \n"
               + "           WHERE     \n"
               + "                  c.orgin = '" + branchCode + "'  \n"
               + "                  AND c.originExpressCenter =  '" + ECCode + "'  \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'       "
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date)  \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date)  \n"
               + "                  AND ISNULL(c.status, 0) !='9'   \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2'    \n"
               + "                  AND ISNULL(c.isApproved, 0) ='0'   \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1'   \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0'   \n"
               + "                  AND c.consignerAccountNo = '0'  \n"
               + "                  \n"
               + " UNION ALL \n"
               + "           SELECT c.consignmentNumber,   \n"
               + "                  CONVERT(VARCHAR, c.bookingDate, 103) bookingdateVarchar,   \n"
               + "                  c.serviceTypeName ServiceType,   \n"
               + "                  b.sname     destination,   \n"
               + "                  c.weight,   \n"
               + "                  c.pieces,   \n"
               + "                  c.totalAmount [GrossAmount],   \n"
               //+ "                  CASE WHEN isnull(c.DiscountID,'0') = '0' THEN c.totalamount + c.gst \n"
               //+ "                  ELSE c.chargedAmount END  chargedAmount,   \n"
               + " c.chargedAmount, \n"

               + "                  CASE    \n"
               // + "						WHEN dm.DiscountValueType = 1 THEN (c.totalAmount + c.gst) - c.chargedAmount     \n"
               + "						WHEN dm.DiscountValueType = 1 THEN dm.DiscountValue    \n"
               + "					ELSE  \n"
               + "						dm.DiscountValue \n"
               + "					END \n"
               + "					AS [ShipmentDiscount], \n"
               + "                   \n"
               + "                  pm.name [SuppService],  ps.name ,  \n"
               + "                  cm.calculatedValue [SuppCharges], cm.calculatedGST [SuppGst],   \n"
               + "                  0 [FranchiseComission],   \n"
               + "                  ps.name   [paymentMethod],   \n"
               + "                  0 [CalculatedIncentive],   \n"
               + "                  c.gst [GST],   \n"
               + "                  c.bookingDate ,  pm.id SuppserviceID, isnull(c.DiscountID,'0') DiscountID ,c.PaymentMode, c.DiscountGST     \n"
               + "           FROM   Consignment c   \n"
               + "                  INNER JOIN Branches b   \n"
               + "                       ON  b.branchCode = c.destination   \n"
               + "                   INNER JOIN Paymentsource ps ON ps.Id=c.PaymentMode  \n"
               + "                           AND ps.booking = '1'   \n"
               + "                          AND ps.STATUS = '1'    \n"
               + "                  LEFT JOIN ConsignmentModifier cm   \n"
               + "                       ON  cm.consignmentNumber = c.consignmentNumber   \n"
               + "                  LEFT JOIN MNP_DiscountConsignment dc   \n"
               + "                       ON  c.consignmentNumber = dc.ConsignmentNumber   \n"
               + "                  LEFT JOIN PriceModifiers pm   \n"
               + "                                         ON pm.id  =cm.priceModifierId    \n"
               + "                  LEFT JOIN MnP_MasterDiscount dm   \n"
               + "                       ON  dc.DiscountID = dm.DiscountID   \n"
               + "           WHERE     \n"
               + "                  c.orgin = '" + branchCode + "'  \n"
               + "                 AND c.originExpressCenter =  '" + ECCode + "'  \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'       "
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date)  \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date)  \n"
               + "                  AND ISNULL(c.status, 0) !='9'   \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2'    \n"
               + "                  AND ISNULL(c.isApproved, 0) ='0'   \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1'   \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0'   \n"
               + "                  AND ISNULL(c.cod, 0) = '1'   \n"
               + "                  AND c.consignerAccountNo != '0'  \n"
               + "          UNION   \n"
               + "          SELECT 'COD PAYMENT ('+CAST(rcp.RiderCode AS VARCHAR) consignmentNumber, \n"
               + "          null bookingdateVarchar,   \n"
               + "          NULL ServiceType,\n"
               + "          null     destination,   \n"
               + "          NULL weight,\n"
               + "          NULL pieces,   \n"
               + "          null[GrossAmount],   \n"
               + "          rcp.CollectedAmount chargedAmount,\n"
               + "          null[ShipmentDiscount], \n"
               + "          null[SuppService],  NULL name,\n"
               + "          NULL[SuppCharges], NULL[SuppGst],\n"
               + "          0[FranchiseComission],   \n"
               + "          NULL[paymentMethod],   \n"
               + "          0[CalculatedIncentive],   \n"
               + "          NULL[GST],   \n"
               + "          CreatedOn bookingDate, NULL SuppserviceID, null DiscountID,'1' PaymentMode, NULL DiscountGST \n"
               + "          FROM tbl_RiderCashPayment AS rcp WHERE CAST(CreatedOn AS date) = CAST(GETDATE() AS date) \n"
               + "          AND CreatedBy = '" + U_ID + "'  \n"
               + "          AND rcp.EcCode = '" + ECCode + "' AND rcp.DSSP = '0' \n"
               + "      ) d   \n"
               + "ORDER BY   \n"
               + "       d.consignmentNumber   \n";

                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                dt_to_generate = dt.Copy();
                if (dt.Rows.Count > 0)
                {
                    float suppCharges = 0;
                    float chargedAmount = 0;
                    float FranchiseComission = 0;
                    for (int j = 0; j < dt.Rows.Count - 1; j++)
                    {
                        DataRow Currentdr = dt.Rows[j];
                        DataRow dr = dt.Rows[j + 1];
                        if (Currentdr["consignmentNumber"].ToString() == dr["consignmentNumber"].ToString())
                        {
                            String CurrentdrRowService = Currentdr["SuppService"].ToString();
                            String drService = dr["SuppService"].ToString();

                            float CurrentdrRowServicePrice = float.Parse(Currentdr["SuppCharges"].ToString());
                            float drServicePrice = float.Parse(dr["SuppCharges"].ToString());

                            Currentdr["SuppService"] = CurrentdrRowService + ", " + drService;
                            Currentdr["SuppCharges"] = (CurrentdrRowServicePrice + drServicePrice);

                            suppCharges = CurrentdrRowServicePrice + drServicePrice;
                            dr.Delete();
                            dt.AcceptChanges();
                            --j;
                        }
                        else
                        {
                            suppCharges = float.Parse(Currentdr["SuppCharges"].ToString());
                        }
                        chargedAmount = float.Parse(Currentdr["chargedAmount"].ToString());
                        FranchiseComission = float.Parse(Currentdr["FranchiseComission"].ToString());
                        Currentdr["AmountCollect"] = chargedAmount - FranchiseComission;
                        //Currentdr["AmountCollect"] =  (chargedAmount + suppCharges) - FranchiseComission ;
                    }

                    DataRow Currentdr_ = dt.Rows[dt.Rows.Count - 1];
                    suppCharges = float.Parse(Currentdr_["SuppCharges"].ToString());

                    chargedAmount = float.Parse(Currentdr_["chargedAmount"].ToString());
                    FranchiseComission = float.Parse(Currentdr_["FranchiseComission"].ToString());
                    //suppGST = float.Parse(Currentdr["SuppCharges"].ToString());
                    //Currentdr_["AmountCollect"] = Math.Round((chargedAmount + suppCharges) - FranchiseComission);

                    // OLD CODE LINE 16 AUG 2021
                    //Currentdr_["AmountCollect"] = ((chargedAmount + suppCharges) - FranchiseComission);
                    Currentdr_["AmountCollect"] = (chargedAmount - FranchiseComission);

                    for (int j = 0; j < dt.Rows.Count - 1; j++)
                    {
                        dt.Rows[dt.Rows.Count - 1]["AmountCollect"] = Math.Round(double.Parse(dt.Rows[dt.Rows.Count - 1]["AmountCollect"].ToString()));
                    }
                    dt.AcceptChanges();
                    response.CNCount = dt.Rows.Count;

                    object TotalWeight = 0;
                    object TotalPcs = 0;
                    object TotalAmountGross = 0;
                    object TotalAmountCharge = 0;
                    object TotalFranchiseComission = 0;
                    object TotalAmountCollect = 0;
                    object TotalSupplementaryCharges = 0;
                    object TotalCalculatedIncentive = 0;
                    object TotalShipmentDiscount = 0;

                    
                    TotalWeight = dt.Compute("Sum(weight)", string.Empty);
                    TotalPcs = dt.Compute("Sum(pieces)", string.Empty);
                    TotalAmountGross = dt.Compute("Sum(GrossAmount)", string.Empty);
                    TotalAmountCharge = dt.Compute("Sum(chargedAmount)", string.Empty);
                    TotalFranchiseComission = dt.Compute("Sum(FranchiseComission)", string.Empty);
                    TotalAmountCollect = dt.Compute("Sum(AmountCollect)", string.Empty);
                    TotalSupplementaryCharges = dt.Compute("Sum(SuppCharges)", string.Empty);
                    TotalShipmentDiscount = dt.Compute("Sum(ShipmentDiscount)", string.Empty);
                    if (TotalShipmentDiscount == DBNull.Value)
                    {
                        
                        if (TotalPcs == DBNull.Value)
                        {
                            dt.Rows.Add(0, 0, 0, Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, 0, 0, 0, 0, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                      //  if else()
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    else
                    {
                        if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    TotalCalculatedIncentive = dt.Compute("Sum(CalculatedIncentive)", string.Empty);
                    CreateTableView(dt);
                    response.dt = dt_to_generate;
                    return response;
                }
                else
                {
                    response.dt = dt_to_generate;
                    return response;
                }
            }

            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
        private void CashSalesSummmaryProductWiseTable()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {

                string sql = "SELECT \n"
               + "       --- DSSP Cash sales summary product wise  \n"
               + "       ---- " + Session["U_NAME"] + "  \n"
               + "       d.products, \n"
               + "       COUNT(d.consignmentNumber)     shipmentQuantity, \n"
               + "       CAST(SUM(d.[weight]) AS DECIMAL(18, 2)) weightSum, \n"
               + "       SUM(d.pieces)                  piecesSum, \n"
               + "       SUM(ROUND(d.GrossAmount,0))             chargedAmountSum \n"
               + "FROM   ( \n"
               + "           SELECT c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"
               + "                  c.chargedAmount GrossAmount, \n"
               //+ "                ROUND( CASE  \n"
               //+ "                       WHEN ISNULL(c.DiscountID, '0') = '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') != '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') = '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "               c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') != '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "               c.chargedAmount --+ SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
               //+ "               END,0) GrossAmount, \n"
               + "           stn.products, \n"
               + "           c.createdOn \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN ServiceTypes_New stn \n"
               + "           ON stn.serviceTypeName = c.serviceTypeName \n"
               + "           LEFT JOIN ConsignmentModifier cm  \n"
               + "           ON cm.consignmentNumber = c.consignmentNumber  \n"
               + "           WHERE c.orgin = '" + branchCode + "' \n"
               + "           AND c.originExpressCenter = '" + ECCode + "' \n"
               + "           AND c.riderCode='" + BookingCode_ + "' "
               + "           AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "           AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date)   \n"
               + "           AND ISNULL(c.status, 0) != '9' \n"
               + "           AND ISNULL(c.isApproved, 0) = '0' \n"
               + "           AND ISNULL(c.isPriceComputed, 0) = '1' \n"
               + "           AND ISNULL(c.misRouted, 0) = '0' \n"
               + "           AND c.consignerAccountNo = '0' \n"
               + "           AND ISNULL(c.InsertType, 0) = '2' \n"
               + "           GROUP BY \n"
               + "           c.consignmentNumber, \n"
               + "           c.serviceTypeName, \n"
               + "           c.weight, \n"
               + "           c.pieces, \n"
               + "           c.chargedAmount, \n"
               + "           stn.products, \n"
               + "           c.createdOn, \n"
               + "           c.DiscountID, \n"
               + "           c.chargedAmount, \n"
               + "           c.totalAmount + c.gst \n\n"

               + "           UNION ALL \n\n"/*

               + "           --DISCOUNTED CN BUT NOT APPLY DISCOUNT BUTTON \n"
               + "           SELECT c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"
               + "                  c.chargedAmount GrossAmount, \n"
               //+ "                ROUND( CASE  \n"
               //+ "                       WHEN ISNULL(c.DiscountID, '0') = '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') != '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') = '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "               c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') != '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "               c.chargedAmount --+ SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
               //+ "               END,0) GrossAmount, \n"
               + "           stn.products, \n"
               + "           c.createdOn \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN ServiceTypes_New stn \n"
               + "           ON stn.serviceTypeName = c.serviceTypeName \n"
               + "           LEFT JOIN ConsignmentModifier cm  \n"
               + "           ON cm.consignmentNumber = c.consignmentNumber  \n"
               + "           WHERE c.orgin = '" + branchCode + "' \n"
               + "           AND c.originExpressCenter = '" + ECCode + "' \n"
               + "           AND c.riderCode='" + BookingCode_ + "' "
               + "           AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "           AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date)   \n"
               + "           AND ISNULL(c.status, 0) = '9' \n"
               + "           AND ISNULL(c.isApproved, 0) = '0' \n"
               + "           AND ISNULL(c.isPriceComputed, 0) = '0' \n"
               + "           AND ISNULL(c.misRouted, 0) = '0' \n"
               + "           AND c.consignerAccountNo = '0' \n"
               + "           AND ISNULL(c.InsertType, 0) = '2' \n"
               + "           GROUP BY \n"
               + "           c.consignmentNumber, \n"
               + "           c.serviceTypeName, \n"
               + "           c.weight, \n"
               + "           c.pieces, \n"
               + "           c.chargedAmount, \n"
               + "           stn.products, \n"
               + "           c.createdOn, \n"
               + "           c.DiscountID, \n"
               + "           c.chargedAmount, \n"
               + "           c.totalAmount + c.gst \n\n"
               + "           UNION ALL \n\n"*/
               + "           SELECT c.consignmentNumber, \n"
               + "                  c.serviceTypeName, \n"
               + "                  c.weight, \n"
               + "                  c.pieces, \n"
               + "                  c.chargedAmount, \n"
               + "                  c.chargedAmount GrossAmount, \n"
               //+ "                ROUND(   CASE  \n"
               //+ "                       WHEN ISNULL(c.DiscountID, '0') = '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') != '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') = '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "               c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
               //+ "               WHEN ISNULL(c.DiscountID, '0') != '0' \n"
               //+ "           AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN  \n"
               //+ "               c.chargedAmount --+ SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  \n"
               //+ "               END,0) GrossAmount, \n"
               + "           stn.products, \n"
               + "           c.createdOn \n"
               + "           FROM Consignment c \n"
               + "           INNER JOIN ServiceTypes_New stn \n"
               + "           ON stn.serviceTypeName = c.serviceTypeName \n"
               + "           LEFT JOIN ConsignmentModifier cm  \n"
               + "           ON cm.consignmentNumber = c.consignmentNumber  \n"
               + "           WHERE c.orgin = '" + branchCode + "' \n"
               + "           AND c.originExpressCenter = '" + ECCode + "' \n"
               + "           AND c.riderCode='" + BookingCode_ + "' "
               + "           AND CAST(c.bookingDate AS date) >= CAST('" + startDate + "' AS date) \n"
               + "           AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date)   \n"
               + "           AND ISNULL(c.status, 0) != '9' \n"
               + "           AND ISNULL(c.isApproved, 0) = '0' \n"
               + "           AND ISNULL(c.isPriceComputed, 0) = '1' \n"
               + "           AND ISNULL(c.misRouted, 0) = '0' \n"
               + "           AND ISNULL(c.cod, 0) = '1' \n"
               + "           AND c.consignerAccountNo != '0' \n"
               + "           AND ISNULL(c.InsertType, 0) = '2' \n"
               + "           GROUP BY \n"
               + "           c.consignmentNumber, \n"
               + "           c.serviceTypeName, \n"
               + "           c.weight, \n"
               + "           c.pieces, \n"
               + "           c.chargedAmount, \n"
               + "           stn.products, \n"
               + "           c.createdOn, \n"
               + "           c.DiscountID, \n"
               + "           c.chargedAmount, \n"
               + "           c.totalAmount + c.gst \n"
               + "       ) d \n"
               + "GROUP BY \n"
               + "       d.products ";



                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    object TotalQuantity = 0;
                    object TotalWeight = 0;
                    object TotalPcs = 0;
                    object TotalAmount = 0;

                    TotalQuantity = dt.Compute("Sum(shipmentQuantity)", string.Empty);
                    TotalWeight = dt.Compute("Sum(weightSum)", string.Empty);
                    TotalPcs = dt.Compute("Sum(piecesSum)", string.Empty);
                    TotalAmount = dt.Compute("Sum(chargedAmountSum)", string.Empty);
                    dt.Rows.Add("Total", TotalQuantity, TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmount)));

                    CreateCashSalesProductView(dt);
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        private void ShipmentAndAmountSummaryView(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            dt.Columns.Remove("PaymentMode");
            dt.Columns.Remove("Id");

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;
                sb.Append("<tr>");
                if (j == rowMax - 1)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px;background:lightblue;'><b>" + rowSingle[columnSingle.ColumnName].ToString() + "</b></td>");
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                    }
                }
                sb.Append("</tr>");
            }
            ShipmentAmountSummaryLiteral.Text = sb.ToString();
        }
        private void SummaryofInternationSalesTableView(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;
                sb.Append("<tr>");
                if (j == rowMax - 1)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'><b>" + rowSingle[columnSingle.ColumnName].ToString() + "</b></td>");
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                    }
                }
                sb.Append("</tr>");
            }
            summaryOfInternationSalesLiteral.Text = sb.ToString();
        }
        private void CreateServiceWiseSummaryView(DataTable dt)
        {

            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;
                sb.Append("<tr>");
                if (j == rowMax - 1)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'><b>" + rowSingle[columnSingle.ColumnName].ToString() + "</b></td>");
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                    }
                }
                sb.Append("</tr>");
            }
            ServiceWiseSummaryLiteral.Text = sb.ToString();
        }
        private void CreateCashSalesProductView(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;
                sb.Append("<tr>");
                if (j == rowMax - 1)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'><b>" + rowSingle[columnSingle.ColumnName].ToString() + "</b></td>");
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                    }
                }
                sb.Append("</tr>");
            }


            CashSalesSummaryProductLiteral.Text = sb.ToString();
        }
        private void CreateTableView(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                sb.Append("<tr >");

                DataRow rowSingle = dt.Rows[j];
                if (j == rowMax - 1)
                {
                    sb.Append("<td colspan='2' style='text-align:center;font-size:10px;background-color:lightblue;height:25px;'></td>");
                    sb.Append("<td style='text-align:center;font-size:10px;background-color:lightblue;height:25px;' colspan='4'><b>GrandTotal</b></td>");
                    for (int i = 0; i < dt.Columns.Count - 10; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px;background-color:lightblue;height:25px;'><b>" + rowSingle[columnSingle.ColumnName].ToString() + "</b></td>");
                    }
                }
                else
                {
                    serial++;
                    sb.Append("<tr>");

                    if ((rowSingle["ShipmentDiscount"].ToString() != "" && rowSingle["DiscountGST"].ToString() != "") || Request.QueryString["DSSPNo"] != null)
                    //if (rowSingle["ShipmentDiscount"].ToString() != "0" || Request.QueryString["DSSPNo"] != null)
                    {
                        sb.Append("<td style='text-align:center;font-size:10px' class='bilal'></td>");
                    }
                    else if (rowSingle["ServiceType"].ToString() == "" )
                    {
                        sb.Append("<td style='text-align:center;font-size:10px' class='bilal'></td>");
                    }
                    else
                    {
                        sb.Append("<td style='text-align:center;font-size:10px'><input type='checkbox'  onclick='ChangeRow(this)'  class='CNCheckbox' value=" + rowSingle["ConsignmentNumber"].ToString() + "></input></td>");
                    }
                    sb.Append("<td style='text-align:center;font-size:10px'>" + serial + "</td>");
                    for (int i = 0; i < dt.Columns.Count - 6; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                    }
                }
                sb.Append("</tr>");
            }
            ConsignmentWiseLiteral.Text = sb.ToString();
        }

        protected void Generate_Btn(object sender, EventArgs e)
        {
            try
            {
                String time = "";
                SysDate.Text = DateTime.Now.ToString();
                time = DateTime.Now.ToString();

                TableWithSumAndCount_QA model = ConsignmentWiseTable();
                TableWithSumAndCount_QA VoidTable = VoidConsignmentTable();

                model.TotalAmount = model.dt.Compute("Sum(AmountCollect)", "PaymentMode = 1").ToString();

                model.dt.Merge(VoidTable.dt);
                model.CNCount += VoidTable.CNCount;

                Int64 MaxDSSPNumber = 0;
                if (model.dt != null)
                {
                    if (model.dt.Rows.Count > 0)
                    {
                        SqlConnection con = new SqlConnection(clvar.Strcon());

                        String sql = " SELECT isNull(max(r.DSSPNumber),0) DSSPNumber FROM MNP_Master_Retail_DSSP r";
                        using (var cmd2 = new SqlCommand(sql, con))
                        {
                            con.Open();
                            SqlDataReader rdr = cmd2.ExecuteReader();
                            if (rdr.Read())
                            {
                                MaxDSSPNumber = rdr.GetInt64(0);
                            }
                            rdr.Close();
                        }
                        con.Close();
                        String stat = "";
                        string newCode = "";

                        if (MaxDSSPNumber > 0)
                        {
                            MaxDSSPNumber++;
                            newCode = MaxDSSPNumber.ToString();
                            newCode = newCode.Remove(0, 4);
                        }
                        else
                        {
                            MaxDSSPNumber++;
                            newCode = MaxDSSPNumber.ToString();
                        }

                        String YearStartingCode = DateTime.Now.Year.ToString();
                        string maxxCode = YearStartingCode + newCode.ToString().PadLeft(8, '0');
                        AutoDSSPNO.Text = maxxCode;

                        AutoDSSPNO.ForeColor = System.Drawing.Color.Black;
                        PrintDate.Text = DateTime.Now.ToString();

                        //  return;
                        stat = AddRecord(model.dt, maxxCode, model.CNCount, model.TotalAmount, VoidTable.dt);
                        if (stat == "Success")
                        {
                            VoidBtn.Visible = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "name", "printDiv('bodyToPrint');", true);
                        }
                        else
                        {
                            PrintDate.Text = "";
                            AutoDSSPNO.Text = stat;
                            AutoDSSPNO.ForeColor = System.Drawing.Color.Red;
                            AutoDSSPNO.CssClass = "large";
                            AutoDSSPNO.Font.Bold = true;
                        }
                    }
                    return;
                }
                else
                {
                    CashSalesSummaryProductLiteral.Text = "";
                    ConsignmentWiseLiteral.Text = "";
                    ServiceWiseSummaryLiteral.Text = "";
                    ShipmentAmountSummaryLiteral.Text = "";
                    summaryOfInternationSalesLiteral.Text = "";

                    PrintDate.Text = "";
                    AutoDSSPNO.Text = "No DSSP no. generated";
                    AutoDSSPNO.ForeColor = System.Drawing.Color.Red;
                    AutoDSSPNO.Font.Bold = true;
                }
            }
            catch (Exception er)
            {

            }
        }
        protected String AddRecord(DataTable dt, String DSSPNumber, int cnCount, String totalAmount, DataTable voidConsignmentsTable)
        {
            BookingCode_ = Session["BookingStaff"].ToString();
            branchCode = Session["BRANCHCODE"].ToString();
            ZoneCode = Session["ZONECODE"].ToString();
            ECCode = Session["ExpressCenter"].ToString();

            //return "";

            //removing extra column in datatable
            dt.Columns.Remove("bookingdateVarchar");
            dt.Columns.Remove("paymentMethod");
            string VoidTableconsignments = "";

            Int64 DSSPbigInt = Convert.ToInt64(DSSPNumber);
            using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString)))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                transaction = conn.BeginTransaction("Insert retail DSSP");
                command.Connection = conn;
                command.Transaction = transaction;
                try
                {
                    String consignments = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        consignments = consignments + "'" + dt.Rows[i]["consignmentNumber"] + "',";
                    }
                    consignments = consignments.Remove(consignments.Length - 1, 1);



                    command.CommandText = "UPDATE Consignment SET misRouted = 1,accountReceivingDate=GETDATE(),isApproved=1 WHERE consignmentNumber IN(" + consignments + ")";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Clear();
                    int rowsEffected = command.ExecuteNonQuery();

                    command.CommandText = "UPDATE tbl_RiderCashPayment SET DSSP = '" + DSSPNumber + "' WHERE EcCode = '" + Session["ExpressCenter"].ToString() + "' AND DSSP = '0' AND CreatedBy = '" + Session["U_ID"].ToString() + "' AND CAST(CreatedOn AS DATE) > DATEADD(D,-1,GETDATE())";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Clear();
                    int rowsEffected_ = command.ExecuteNonQuery();

                    if (voidConsignmentsTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < voidConsignmentsTable.Rows.Count; i++)
                        {
                            VoidTableconsignments = VoidTableconsignments + "'" + voidConsignmentsTable.Rows[i]["consignmentNumber"] + "',";
                        }
                        VoidTableconsignments = VoidTableconsignments.Remove(VoidTableconsignments.Length - 1, 1);
                        command.CommandText = "UPDATE Consignment SET misRouted = 1,accountReceivingDate=GETDATE(),isApproved=1 WHERE consignmentNumber IN (" + VoidTableconsignments + ")";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }


                    // return "";
                    //VoidTableconsignments = VoidTableconsignments.Remove(VoidTableconsignments.Length - 1, 1);
                    //command.CommandText = "UPDATE Consignment SET misRouted = 1,accountReceivingDate=GETDATE(),isApproved=1 WHERE consignmentNumber IN (" + VoidTableconsignments + ")";
                    //command.CommandType = CommandType.Text;
                    //command.ExecuteNonQuery();


                    string sqlMasterTable = "  INSERT INTO MNP_Master_Retail_DSSP \n"
                       + "  (  DSSPNumber,  	ZoneCode,  	BranchCode,  	ExpressCenterCode, \n"
                       + "  	BookingCode,  	CNCount,  	TotalAmount,  	 \n"
                       + "  	BookingShift,  	CreatedBy, CreatedOn ) \n"
                       + "  VALUES \n"
                       + "  (  	@DSSPNumberM,  	@zoneM, \n"
                       + "  	@BranchM,  	@ExpressM, \n"
                       + "  	@BookingM,  	@CNCountM, \n"
                       + "  	@TotalAmountM,  	@ShiftM, \n"
                       + "  	@CreateByM,  	GETDATE()  )";
                    command.CommandText = sqlMasterTable;
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@DSSPNumberM", DSSPNumber);
                    command.Parameters.AddWithValue("@zoneM", ZoneCode);
                    command.Parameters.AddWithValue("@BranchM", branchCode);
                    command.Parameters.AddWithValue("@ExpressM", ECCode);
                    command.Parameters.AddWithValue("@BookingM", BookingCode_);
                    command.Parameters.AddWithValue("@CNCountM", cnCount);
                    command.Parameters.AddWithValue("@TotalAmountM", totalAmount.ToString());
                    command.Parameters.AddWithValue("@ShiftM", shift_);
                    command.Parameters.AddWithValue("@CreateByM", U_ID);
                    int rowsMaster = command.ExecuteNonQuery();

                    //add created by columns and createdOn columns,branch,zone,ec,DSSPNo
                    dt.Columns.Add("DSSPNumber", typeof(string));
                    dt.Columns.Add("ZoneCode", typeof(string));
                    dt.Columns.Add("BranchCode", typeof(string));
                    dt.Columns.Add("OriginExpressCenter", typeof(string));
                    dt.Columns.Add("BookingCode", typeof(string));
                    dt.Columns.Add("CreatedBy", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        //need to set value to NewColumn column
                        row["DSSPNumber"] = DSSPNumber;   // or set it to some other value
                        row["ZoneCode"] = ZoneCode;   // or set it to some other value
                        row["BranchCode"] = branchCode;   // or set it to some other value
                        row["OriginExpressCenter"] = ECCode;   // or set it to some other value
                        row["BookingCode"] = BookingCode_;   // or set it to some other value
                        row["CreatedBy"] = U_ID;   // or set it to some other value
                    }

                    //DataTable t;
                    dt.Columns.Remove("DiscountGST");


                    command.CommandText = "MNP_Retail_DSSP_Insert";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@tbl", dt);
                    command.Parameters.Add("@returnMsg", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    String message = command.Parameters["@returnMsg"].SqlValue.ToString().ToUpper();

                    transaction.Commit();
                    if (message != "1")
                    {
                        // status = "Error Inserting discount record!: " + message;
                        throw new Exception();
                    }
                    else
                    {
                        //status = "Group Id: " + ParentGroupId + " .Discount Record Successfully Saved, pending activation";
                        conn.Close();
                    }
                    return "Success";
                }

                catch (Exception ex)
                {
                    AutoDSSPNO.Text = "No DSSP no. generated";
                    AutoDSSPNO.ForeColor = System.Drawing.Color.Red;
                    AutoDSSPNO.Font.Bold = true;
                    try
                    {
                        transaction.Rollback();
                        return "Error DSSP inserting record";
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.;
                        return "Error inserting DSSP record, rollback transaction failed";

                    }
                }
            }
        }
        protected void Print_Btn(object sender, EventArgs e)
        {
            //AutoDSSPNO.Text = "No DSSP no. generated";
            //AutoDSSPNO.ForeColor = System.Drawing.Color.Red;
            //AutoDSSPNO.Font.Bold = true;
            SysDate.Text = DateTime.Now.ToString();
            PrintDate.Text = DateTime.Now.ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "name", "printDiv('bodyToPrint');", true);
        }
        protected void Back_btn(object sender, EventArgs e)
        {
            Response.Redirect("~/Files/BTSDashoard.aspx");

        }
        private TableWithSumAndCount_QA VoidConsignmentTable()
        {
            TableWithSumAndCount_QA response = new TableWithSumAndCount_QA();

            DataTable dt = new DataTable();
            DataTable dt_to_generate = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = "SELECT --VOID DSSP  \n"
               + "       d.consignmentNumber,   \n"
               + "       d.bookingdateVarchar,   \n"
               + "       d.ServiceType,   \n"
               + "       d.destination,   \n"
               + "       Cast(d.weight as decimal(18,2)) as weight,   \n"
               + "       d.pieces,   \n"
               + "       Round(cast(d.GrossAmount as decimal(18,2)),2) as GrossAmount ,    \n"
               + "       Round(d.chargedAmount,2) chargedAmount,   \n"
               + "        Round((d.shipmentDiscount),2) ShipmentDiscount,         \n"
               + "       d.SuppService,   \n"
               + "       isnull(Round((d.SuppCharges+d.SuppGst),2),0) SuppCharges,   \n"
               + "       d.FranchiseComission,   \n"
               + "       d.paymentMethod ,       d.CalculatedIncentive,   \n"
               + "       Round(((d.chargedAmount - d.FranchiseComission)+isnull(d.SuppCharges,0)+isnull(d.SuppGst,0)),2) [AmountCollect],    \n"
               + "       d.GST,   \n"
               + "       d.bookingDate ,  d.SuppserviceID,'0' CNStatus,d.PaymentMode, DiscountGST  \n"
               + " FROM   (   \n"
               + "           SELECT c.consignmentNumber,   \n"
               + "                  CONVERT(VARCHAR, c.bookingDate, 103) bookingdateVarchar,   \n"
               + "                  c.serviceTypeName ServiceType,   \n"
               + "                  b.sname     destination,   \n"
               + "                  c.weight,   \n"
               + "                  c.pieces,   \n"
               + "                  c.totalAmount [GrossAmount],   \n"
               + "                  CASE WHEN isnull(c.DiscountID,'0') = '0' THEN c.totalamount + c.gst \n"
               + "                  ELSE c.chargedAmount END  chargedAmount,   \n"
               //+ "                  CASE    \n"
               //+ "						WHEN dm.DiscountValueType = 1 THEN (c.totalAmount + c.gst) - c.chargedAmount     \n"
               //+ "					ELSE  \n"
               //+ "						dm.DiscountValue \n"
               //+ "					END \n"
               //+ "					AS [ShipmentDiscount], \n"
               + "                  CASE    \n"
               + "						WHEN dm.DiscountValueType = 1 THEN dm.DiscountValue  \n"
               + "					ELSE  \n"
               + "						dm.DiscountValue \n"
               + "					END \n"
               + "					AS [ShipmentDiscount], \n"
               + "                   \n"
               + "                  pm.name [SuppService],  ps.name ,  \n"
               + "                  cm.calculatedValue [SuppCharges], cm.calculatedGST [SuppGst],   \n"
               + "                  0 [FranchiseComission],   \n"
               + "                  ps.name   [paymentMethod],   \n"
               + "                  0 [CalculatedIncentive],   \n"
               + "                  c.gst [GST],   \n"
               + "                  c.bookingDate ,  pm.id SuppserviceID, isnull(c.DiscountID,'0') DiscountID ,c.PaymentMode,c.DiscountGST \n"
               + "           FROM   Consignment c   \n"
               + "                  INNER JOIN Branches b   \n"
               + "                       ON  b.branchCode = c.destination   \n"
               + "                   INNER JOIN Paymentsource ps ON ps.Id=c.PaymentMode  \n"
               + "                           AND ps.booking = '1'   \n"
               + "                          AND ps.STATUS = '1'    \n"
               + "                  LEFT JOIN ConsignmentModifier cm   \n"
               + "                       ON  cm.consignmentNumber = c.consignmentNumber   \n"
               + "                  LEFT JOIN MNP_DiscountConsignment dc   \n"
               + "                       ON  c.consignmentNumber = dc.ConsignmentNumber   \n"
               + "                  LEFT JOIN PriceModifiers pm   \n"
               + "                                         ON pm.id  =cm.priceModifierId    \n"
               + "                  LEFT JOIN MnP_MasterDiscount dm   \n"
               + "                       ON  dc.DiscountID = dm.DiscountID   \n"
               + "           WHERE     \n"
               + "                  c.orgin = '" + branchCode + "'  \n"
               + "                 AND c.originExpressCenter =  '" + ECCode + "'  \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'       "
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + VoidStartDate + "' AS date)  \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date)  \n"
               + "                  AND ISNULL(c.status, 0) ='9'   \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2'    \n"
               + "                  AND ISNULL(c.isApproved, 0) ='0'   \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1'   \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0'   \n"
               + "                  AND c.consignerAccountNo = '0'  \n"
               + "                  \n"
               + "      UNION ALL \n"
               + "           SELECT c.consignmentNumber,   \n"
               + "                  CONVERT(VARCHAR, c.bookingDate, 103) bookingdateVarchar,   \n"
               + "                  c.serviceTypeName ServiceType,   \n"
               + "                  b.sname     destination,   \n"
               + "                  c.weight,   \n"
               + "                  c.pieces,   \n"
               + "                  c.totalAmount [GrossAmount],   \n"
               + "                  CASE WHEN isnull(c.DiscountID,'0') = '0' THEN c.totalamount + c.gst \n"
               + "                  ELSE c.chargedAmount END  chargedAmount,   \n"
               //+ "                  CASE    \n"
               //+ "						WHEN dm.DiscountValueType = 1 THEN (c.totalAmount + c.gst) - c.chargedAmount     \n"
               //+ "					ELSE  \n"
               //+ "						dm.DiscountValue \n"
               //+ "					END \n"
               //+ "					AS [ShipmentDiscount], \n"
               + "                  CASE    \n"
               + "						WHEN dm.DiscountValueType = 1 THEN dm.DiscountValue  \n"
               + "					ELSE  \n"
               + "						dm.DiscountValue \n"
               + "					END \n"
               + "					AS [ShipmentDiscount], \n"
               + "                   \n"
               + "                  pm.name [SuppService],  ps.name ,  \n"
               + "                  cm.calculatedValue [SuppCharges], cm.calculatedGST [SuppGst],   \n"
               + "                  0 [FranchiseComission],   \n"
               + "                  ps.name   [paymentMethod],   \n"
               + "                  0 [CalculatedIncentive],   \n"
               + "                  c.gst [GST],   \n"
               + "                  c.bookingDate ,  pm.id SuppserviceID, isnull(c.DiscountID,'0') DiscountID ,c.PaymentMode, c.DiscountGST \n"
               + "           FROM   Consignment c   \n"
               + "                  INNER JOIN Branches b   \n"
               + "                       ON  b.branchCode = c.destination   \n"
               + "                   INNER JOIN Paymentsource ps ON ps.Id=c.PaymentMode  \n"
               + "                           AND ps.booking = '1'   \n"
               + "                          AND ps.STATUS = '1'    \n"
               + "                  LEFT JOIN ConsignmentModifier cm   \n"
               + "                       ON  cm.consignmentNumber = c.consignmentNumber   \n"
               + "                  LEFT JOIN MNP_DiscountConsignment dc   \n"
               + "                       ON  c.consignmentNumber = dc.ConsignmentNumber   \n"
               + "                  LEFT JOIN PriceModifiers pm   \n"
               + "                                         ON pm.id  =cm.priceModifierId    \n"
               + "                  LEFT JOIN MnP_MasterDiscount dm   \n"
               + "                       ON  dc.DiscountID = dm.DiscountID   \n"
               + "           WHERE     \n"
               + "                  c.orgin = '" + branchCode + "'  \n"
               + "                 AND c.originExpressCenter =  '" + ECCode + "'  \n"
               + "                  AND c.riderCode='" + BookingCode_ + "'       "
               + "                  AND CAST(c.bookingDate AS date) >= CAST('" + VoidStartDate + "' AS date)  \n"
               + "                  AND CAST(c.bookingDate AS date) <= CAST(" + endDate + " AS date)  \n"
               + "                  AND ISNULL(c.status, 0) ='9'   \n"
               + "                  AND ISNULL(c.InsertType, 0) = '2'    \n"
               + "                  AND ISNULL(c.isApproved, 0) ='0'   \n"
               + "                  AND ISNULL(c.isPriceComputed, 0) = '1'   \n"
               + "                  AND ISNULL(c.misRouted, 0) = '0'   \n"
               + "                  AND ISNULL(c.cod, 0) = '1'   \n"
               + "                  AND c.consignerAccountNo != '0'  \n"
               + "                  \n"
               + "       ) d   \n"
               + "ORDER BY   \n"
               + "       d.consignmentNumber   \n";

                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                dt_to_generate = dt.Copy();
                if (dt.Rows.Count > 0)
                {
                    float suppCharges = 0;
                    float chargedAmount = 0;
                    float FranchiseComission = 0;
                    for (int j = 0; j < dt.Rows.Count - 1; j++)
                    {
                        DataRow Currentdr = dt.Rows[j];
                        DataRow dr = dt.Rows[j + 1];
                        if (Currentdr["consignmentNumber"].ToString() == dr["consignmentNumber"].ToString())
                        {
                            String CurrentdrRowService = Currentdr["SuppService"].ToString();
                            String drService = dr["SuppService"].ToString();

                            float CurrentdrRowServicePrice = float.Parse(Currentdr["SuppCharges"].ToString());
                            float drServicePrice = float.Parse(dr["SuppCharges"].ToString());

                            Currentdr["SuppService"] = CurrentdrRowService + ", " + drService;
                            Currentdr["SuppCharges"] = (CurrentdrRowServicePrice + drServicePrice);

                            suppCharges = CurrentdrRowServicePrice + drServicePrice;
                            dr.Delete();
                            dt.AcceptChanges();
                            --j;
                        }
                        else
                        {
                            suppCharges = float.Parse(Currentdr["SuppCharges"].ToString());
                        }
                        chargedAmount = float.Parse(Currentdr["chargedAmount"].ToString());
                        FranchiseComission = float.Parse(Currentdr["FranchiseComission"].ToString());
                        //suppGST = float.Parse(Currentdr["SuppCharges"].ToString());
                        Currentdr["AmountCollect"] = Math.Round((chargedAmount + suppCharges) - FranchiseComission);
                    }

                    DataRow Currentdr_ = dt.Rows[dt.Rows.Count - 1];
                    suppCharges = float.Parse(Currentdr_["SuppCharges"].ToString());

                    chargedAmount = float.Parse(Currentdr_["chargedAmount"].ToString());
                    FranchiseComission = float.Parse(Currentdr_["FranchiseComission"].ToString());
                    //suppGST = float.Parse(Currentdr["SuppCharges"].ToString());

                    // OLD CODE 16 AUG 2021
                    //Currentdr_["AmountCollect"] = Math.Round((chargedAmount + suppCharges) - FranchiseComission);
                    Currentdr_["AmountCollect"] = Math.Round(chargedAmount - FranchiseComission);



                    dt.AcceptChanges();

                    response.CNCount = dt.Rows.Count;

                    object TotalWeight = 0;
                    object TotalPcs = 0;
                    object TotalAmountGross = 0;
                    object TotalAmountCharge = 0;
                    object TotalFranchiseComission = 0;
                    object TotalAmountCollect = 0;
                    object TotalSupplementaryCharges = 0;
                    object TotalCalculatedIncentive = 0;
                    object TotalShipmentDiscount = 0;

                    TotalWeight = dt.Compute("Sum(weight)", string.Empty);
                    TotalPcs = dt.Compute("Sum(pieces)", string.Empty);
                    TotalAmountGross = dt.Compute("Sum(GrossAmount)", string.Empty);
                    TotalAmountCharge = dt.Compute("Sum(chargedAmount)", string.Empty);
                    TotalFranchiseComission = dt.Compute("Sum(FranchiseComission)", string.Empty);
                    TotalAmountCollect = dt.Compute("Sum(AmountCollect)", string.Empty);
                    TotalSupplementaryCharges = dt.Compute("Sum(SuppCharges)", string.Empty);
                    TotalShipmentDiscount = dt.Compute("Sum(ShipmentDiscount)", string.Empty);
                    if (TotalShipmentDiscount == DBNull.Value)
                    {
                        if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    else
                    {
                        if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    CreateVoidCNTableView(dt);

                    response.dt = dt_to_generate;
                    response.TotalAmount = TotalAmountCollect.ToString();
                }
                else
                {
                    response.dt = dt_to_generate;
                    response.CNCount = dt.Rows.Count;
                    response.TotalAmount = "0";
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }
            return response;
        }

        private TableWithSumAndCount_QA CODRiderAmount()
        {
            TableWithSumAndCount_QA response = new TableWithSumAndCount_QA();

            DataTable dt = new DataTable();
            DataTable dt_to_generate = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                string sql = @"SELECT * FROM tbl_RiderCashPayment AS rcp WHERE CAST(CreatedOn AS date) = CAST(GETDATE() AS date) 
                                AND CreatedBy='" + U_ID + @"' AND rcp.EcCode = '" + ECCode + @"'
                                ORDER BY rcp.Id DESC";

                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                con.Close();
                dt_to_generate = dt.Copy();
                if (dt.Rows.Count > 0)
                {
                    // dt.AcceptChanges();

                    //  response.CNCount = dt.Rows.Count;

                    object CollectedAmount = 0;
                    CollectedAmount = dt.Compute("Sum(CollectedAmount)", string.Empty);

                    response.TotalAmount = CollectedAmount.ToString();
                }
                else
                {
                    response.dt = dt_to_generate;
                    response.CNCount = dt.Rows.Count;
                    response.TotalAmount = "0";
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }
            return response;
        }

        private void VoidTable_OLD(string oldDSSP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {

                string sql = @"SELECT --old dssp VOID table  
                       d.consignmentNumber,   
                       d.bookingdateVarchar,   
                       d.ServiceType,   
                       d.destination,   
                       Cast(d.weight as decimal(18,2)) as weight,   
                       d.pieces,   
                       Round(cast(d.GrossAmount as decimal(18,0)),0) as GrossAmount ,    
                       Round(d.chargedAmount,0) chargedAmount,   
                        Round((d.shipmentDiscount),2) ShipmentDiscount,         
                       d.SuppService,   
                       isnull(Round((d.SuppCharges+d.SuppGst),2),0) SuppCharges,   
                       d.FranchiseComission,   
                       d.paymentMethod ,       d.CalculatedIncentive,   
                       Round(((d.chargedAmount - d.FranchiseComission)+isnull(d.SuppCharges,0)+isnull(d.SuppGst,0)),0) [AmountCollect],    
                       d.GST,   
                       d.bookingDate ,  d.SuppserviceID   ,d.CNStatus,d.PaymentMode
                 FROM   (   
                           SELECT distinct c.consignmentNumber,   
                                  CONVERT(VARCHAR, c.bookingDate, 103) bookingdateVarchar,   
                                  c.serviceTypeName ServiceType,   
                                  b.sname     destination,   
                                  c.weight,   
                                  c.pieces,   
                                  c.totalAmount [GrossAmount],   
                                  CASE WHEN isnull(c.DiscountID,'0') = '0' THEN c.totalamount + c.gst 
                                  ELSE c.chargedAmount END  chargedAmount,   
                                  CASE    
						                WHEN dm.DiscountValueType = 1 THEN (c.totalAmount + c.gst) - c.chargedAmount     
					                ELSE  
						                dm.DiscountValue 
					                END 
					                AS [ShipmentDiscount], 
                   
                                  pm.name [SuppService],  ps.name ,  
                                  cm.calculatedValue [SuppCharges], cm.calculatedGST [SuppGst],   
                                  0 [FranchiseComission],   
                                  ps.name   [paymentMethod],   
                                  0 [CalculatedIncentive],   
                                  c.gst [GST],   
                                  c.bookingDate ,  pm.id SuppserviceID, isnull(c.DiscountID,'0') DiscountID     ,mdrd.CNStatus,mdrd.PaymentMode
                           FROM   MNP_Detail_Retail_DSSP mdrd
                                  INNER JOIN Consignment c
                                       ON  c.ConsignmentNumber = mdrd.ConsignmentNumber
                                  INNER JOIN Paymentsource ps
                                       ON  ps.Id = c.PaymentMode
                                       AND ps.booking = '1'
                                       AND ps.STATUS = '1'
                                  INNER JOIN Branches b
                                       ON  b.branchCode = c.destination
                                  LEFT JOIN ConsignmentModifier cm
                                       ON  cm.consignmentNumber = c.consignmentNumber
                                  LEFT JOIN MNP_DiscountConsignment dc
                                       ON  c.consignmentNumber = dc.ConsignmentNumber
                                  LEFT JOIN PriceModifiers pm
                                       ON  pm.id = cm.priceModifierId
                                  LEFT JOIN MnP_MasterDiscount dm
                                       ON  dc.DiscountID = dm.DiscountID
                           WHERE  DSSPNumber = " + oldDSSP + @"  AND mdrd.CNStatus=0
                       )                                    d
                ORDER BY
                       d.ConsignmentNumber ";

                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    float suppCharges = 0;
                    float chargedAmount = 0;
                    float FranchiseComission = 0;
                    for (int j = 0; j < dt.Rows.Count - 1; j++)
                    {
                        DataRow Currentdr = dt.Rows[j];
                        DataRow dr = dt.Rows[j + 1];
                        if (Currentdr["consignmentNumber"].ToString() == dr["consignmentNumber"].ToString())
                        {
                            String CurrentdrRowService = Currentdr["SuppService"].ToString();
                            String drService = dr["SuppService"].ToString();

                            float CurrentdrRowServicePrice = float.Parse(Currentdr["SuppCharges"].ToString());
                            float drServicePrice = float.Parse(dr["SuppCharges"].ToString());

                            Currentdr["SuppService"] = CurrentdrRowService + ", " + drService;
                            Currentdr["SuppCharges"] = (CurrentdrRowServicePrice + drServicePrice);

                            suppCharges = CurrentdrRowServicePrice + drServicePrice;
                            dr.Delete();
                            dt.AcceptChanges();
                            --j;
                        }
                        else
                        {
                            suppCharges = float.Parse(Currentdr["SuppCharges"].ToString());
                        }
                        chargedAmount = float.Parse(Currentdr["chargedAmount"].ToString());
                        FranchiseComission = float.Parse(Currentdr["FranchiseComission"].ToString());
                        //suppGST = float.Parse(Currentdr["SuppCharges"].ToString());
                        Currentdr["AmountCollect"] = (chargedAmount + suppCharges) - FranchiseComission;
                    }

                    dt.AcceptChanges();

                    object TotalWeight = 0;
                    object TotalPcs = 0;
                    object TotalAmountGross = 0;
                    object TotalAmountCharge = 0;
                    object TotalFranchiseComission = 0;
                    object TotalAmountCollect = 0;
                    object TotalSupplementaryCharges = 0;
                    object TotalCalculatedIncentive = 0;
                    object TotalShipmentDiscount = 0;

                    TotalWeight = dt.Compute("Sum(weight)", string.Empty);
                    TotalPcs = dt.Compute("Sum(pieces)", string.Empty);
                    TotalAmountGross = dt.Compute("Sum(GrossAmount)", string.Empty);
                    TotalAmountCharge = dt.Compute("Sum(chargedAmount)", string.Empty);
                    TotalFranchiseComission = dt.Compute("Sum(FranchiseComission)", string.Empty);
                    TotalAmountCollect = dt.Compute("Sum(AmountCollect)", string.Empty);
                    TotalSupplementaryCharges = dt.Compute("Sum(SuppCharges)", string.Empty);
                    TotalShipmentDiscount = dt.Compute("Sum(ShipmentDiscount)", string.Empty);
                    if (TotalShipmentDiscount == DBNull.Value)
                    {
                        if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), 0, 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    else
                    {
                        if (TotalSupplementaryCharges == DBNull.Value)
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, 0, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                        else
                        {
                            dt.Rows.Add(TotalWeight, TotalPcs, Math.Round(Convert.ToDecimal(TotalAmountGross), 0), Math.Round(Convert.ToDecimal(TotalAmountCharge)), Math.Round(Convert.ToDecimal(TotalShipmentDiscount)), 0, TotalSupplementaryCharges, TotalFranchiseComission, 0, TotalCalculatedIncentive, Math.Round(Convert.ToDecimal(TotalAmountCollect)));
                        }
                    }
                    TotalCalculatedIncentive = dt.Compute("Sum(CalculatedIncentive)", string.Empty);
                    CreateVoidCNTableView(dt);

                }
            }
            catch (Exception er)
            {

            }
        }

        private void CreateVoidCNTableView(DataTable dt)
        {

            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                sb.Append("<tr >");

                DataRow rowSingle = dt.Rows[j];
                if (j == rowMax - 1)
                {
                    sb.Append("<td colspan='2' style='text-align:center;font-size:10px;background-color:lightblue;height:25px;'></td>");
                    sb.Append("<td style='text-align:center;font-size:10px;background-color:lightblue;height:25px;' colspan='4'><b>GrandTotal</b></td>");
                    for (int i = 0; i < dt.Columns.Count - 10; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px;background-color:lightblue;height:25px;'><b>" + rowSingle[columnSingle.ColumnName].ToString() + "</b></td>");
                    }
                }
                else
                {
                    serial++;
                    sb.Append("<tr>");
                    sb.Append("<td style='text-align:center;font-size:10px' colspan='2'>" + serial + "</td>");
                    for (int i = 0; i < dt.Columns.Count - 6; i++)
                    {
                        DataColumn columnSingle = dt.Columns[i];
                        sb.Append("<td style='text-align:center;font-size:10px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                    }
                }
                sb.Append("</tr>");
            }
            VoidTableLiteral.Text = sb.ToString();
        }

        [WebMethod]
        public static string[][] VoidCNs(string[] Consignments)
        {
            SqlConnection con2 = new SqlConnection(clvarStatic.Strcon());
            List<string[]> resp = new List<string[]>();
            string[] Response = { "", "" };
            try
            {
                if (Consignments.Count() == 0)
                {
                    Response[0] = "False";
                    Response[1] = "No consignments found to void";
                    resp.Add(Response);
                    return resp.ToArray();
                }

                StringBuilder sb = new StringBuilder();
                foreach (var item in Consignments)
                {
                    sb.Append($"'{item}',");
                }
                var resultCN = sb.ToString().TrimEnd(',');
                SqlCommand UpdateCommand = new SqlCommand($" Update Consignment set status=9, isPriceComputed = '1' where ConsignmentNumber in ({resultCN})", con2);
                con2.Open();
                UpdateCommand.ExecuteNonQuery();

                Response[0] = "True";
                Response[1] = "Successfully made " + Consignments.Count() + " consignment(s) void";
            }
            catch (Exception er)
            {
                string msg = "Error occured: " + er.Message.ToString();
                msg = msg.Replace("\n", String.Empty);
                msg = msg.Replace("\r", String.Empty);
                Response[0] = "False";
                Response[1] = msg;
            }
            finally
            {
                con2.Dispose();
                con2.Close();
            }
            resp.Add(Response);
            return resp.ToArray();

        }

        public class TableWithSumAndCount_QA
        {
            public DataTable dt { get; set; }
            public int CNCount { get; set; }
            public String TotalAmount1 { get; set; }
            public String TotalAmount { get; set; }
            public String CollectAmount { get; set; }

        }
    }

}