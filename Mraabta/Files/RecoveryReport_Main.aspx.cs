using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Configuration;
using System.Web;
using System.Web.Services;
using System.Threading;

namespace MRaabta.Files
{
    public partial class RecoveryReport_Main : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        DataTable Ds_1 = new DataTable();
        string DateTimeStarting = "2022-09-01";
        string ExpressCenterCode = "", U_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                U_ID = Session["U_ID"].ToString();
            }
            catch (Exception er)
            {
                Response.Redirect("~/Login");
            }
        }
        protected void btn_Get_Data_Click(object sender, EventArgs e)
        {
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            try
            {
                if (ExpressCentertxt.Text != "" /*&& CreationDate.Text != ""*/)
                {
                    ExpressCenterCode = ExpressCentertxt.Text.ToString();

                    string query = "SELECT h.DSSPNumber,CONVERT(VARCHAR(11), CreatedOn, 106) CreatedOn,h.branch,h.zone,h.expressCenter,h.ExpressCenterCode,h.BookingCode,h.CNCount," +
                        "ROUND(h.TotalAmount, 0) + ISNULL(ROUND(h.TotalRiderPayment, 0), 0) TotalAmount, \n" +
                    "ROUND(h.TotalAmount, 0) ECAmount,ISNULL(ROUND(h.TotalRiderPayment, 0), 0) RiderAmount,ROUND(h.CollectAmount, 0) CollectAmount, \n" +
                    "(ROUND(h.TotalAmount, 0) + ISNULL(ROUND(h.TotalRiderPayment, 0), 0)) - ROUND(h.CollectAmount, 0) outstanding, \n" +
                    "ISNULL(ROUND(h.CASH_Domestic, 0), 0) CASH_Domestic, \n" +
                    "ISNULL(ROUND(h.CASH_International, 0), 0) CASH_International, \n" +
"ISNULL(ROUND(h.CASH_RoadRail, 0), 0) CASH_RoadRail, \n" +

"ISNULL(ROUND(h.CREDIT_CARD_Domestic, 0), 0) CREDIT_CARD_Domestic, \n" +

"ISNULL(ROUND(h.CREDIT_CARD_International, 0), 0) CREDIT_CARD_International, \n" +

"ISNULL(ROUND(h.CREDIT_CARD_RoadRail, 0), 0) CREDIT_CARD_RoadRail, \n" +

"ISNULL(ROUND(h.QR_Domestic, 0), 0) QR_Domestic, \n" +

"ISNULL(ROUND(h.QR_International, 0), 0) QR_International, \n" +

"ISNULL(ROUND(h.QR_RoadRail, 0), 0) QR_RoadRail, \n" +

"ISNULL(ROUND(h.RiderPaymentLeft, 0), 0) RiderPayment  \n" +

"FROM( \n" +

"select DsspNumber, CreatedOn, branch, zone, expressCenter, ExpressCenterCode, BookingCode, CNCount, CollectAmount, TotalRiderPayment, RiderPaymentLeft, \n" +

"sum(TotalAmount) as TotalAmount, sum(CASH_Domestic) as CASH_Domestic, sum(CASH_International) as CASH_International, sum(CASH_RoadRail) as CASH_RoadRail, sum(CREDIT_CARD_Domestic) as CREDIT_CARD_Domestic, \n" +

"sum(CREDIT_CARD_International) as CREDIT_CARD_International, sum(CREDIT_CARD_RoadRail) as CREDIT_CARD_RoadRail, sum(QR_Domestic) as QR_Domestic, sum(QR_International) as QR_International, sum(QR_RoadRail) as QR_RoadRail \n" +

"from ( \n" +

"select DsspNumber, CreatedOn, branch, zone, expressCenter, ExpressCenterCode, BookingCode, CNCount, CollectAmount, TotalRiderPayment, RiderPaymentLeft, sum(Amount) as TotalAmount, \n" +

"case when PaymentMode = '1' and Products = 'Domestic' then sum(Amount) else 0 end as CASH_Domestic, \n" +

"case when PaymentMode = '1' and Products = 'International' then sum(Amount) else 0 end as CASH_International, \n" +

"case when PaymentMode = '1' and Products = 'Road n Rail' then sum(Amount) else 0 end as CASH_RoadRail, \n" +

"case when PaymentMode = '7' and Products = 'Domestic' then sum(Amount) else 0 end as CREDIT_CARD_Domestic, \n" +

"case when PaymentMode = '7' and Products = 'International' then sum(Amount) else 0 end as CREDIT_CARD_International, \n" +

"case when PaymentMode = '7' and Products = 'Road n Rail' then sum(Amount) else 0 end as CREDIT_CARD_RoadRail, \n" +

"case when PaymentMode = '6' and Products = 'Domestic' then sum(Amount) else 0 end as QR_Domestic, \n" +

"case when PaymentMode = '6' and Products = 'International' then sum(Amount) else 0 end as QR_International, \n" +

"case when PaymentMode = '6' and Products = 'Road n Rail' then sum(Amount) else 0 end as QR_RoadRail \n" +

"from( \n" +

"SELECT mmrd.DsspNumber, mmrd.CreatedOn, b.name branch, z.name zone, ec.name expressCenter, mmrd.ExpressCenterCode, mmrd.BookingCode, mmrd.CNCount, \n" +

"ISNULL(mmrd.CollectAmount, 0) CollectAmount, xb.Products, xb.PaymentMode, xb.Amount, (SELECT SUM(ISNULL(ROUND(r.[GrossAmount], 0), 0)) RiderCash FROM( \n" +

"select dd.ChargedAmount GrossAmount from MNP_Master_Retail_DSSP md inner join MNP_Detail_Retail_DSSP dd on dd.DSSPNumber = md.DSSPNumber where md.DSSPNumber = mmrd.DsspNumber and md.ExpressCenterCode = '" + ExpressCenterCode + @"' " +

" \n AND md.CreatedOn >= '" + DateTimeStarting + @"' and dd.RiderCode is not null) r) TotalRiderPayment, " +

"\n (SELECT SUM(ISNULL(ROUND(cod.[GrossAmount], 0), 0)) RiderCash FROM( \n" +

"select cd.Amount GrossAmount from CollectionCODRider cd where cd.DSSPCode = mmrd.DsspNumber and cd.EcCode = '" + ExpressCenterCode + @"' AND cd.CreatedOn >= '" + DateTimeStarting + @"' and cd.RiderCode is not null ) cod) RiderPaymentLeft " +

" \n FROM MNP_Master_Retail_DSSP mmrd \n" +

"INNER JOIN Branches b ON b.branchCode = mmrd.BranchCode \n" +

"INNER JOIN Zones z ON z.zoneCode = b.zoneCode \n" +

"INNER JOIN ExpressCenters ec ON ec.expressCenterCode = mmrd.ExpressCenterCode \n" +

"inner join \n" +

"(SELECT DSSPNumber, d.Products, PaymentMode, SUM(ISNULL(ROUND(d.[GrossAmount], 0), 0)) Amount \n" +

"FROM(SELECT M.DSSPNumber, c.consignmentNumber, c.chargedAmount, \n" +

"CASE WHEN ISNULL(c.DiscountID, '0') ='0' AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 and (m.createdon < '" + DateTimeStarting + @"' or stn.products <> 'International') THEN c.totalAmount + c.gst " +

"WHEN ISNULL(c.DiscountID, '0') != '0' AND SUM(ISNULL(cm.calculatedValue, '0')) = 0 and (m.createdon < '" + DateTimeStarting + @"' or stn.products <> 'International') THEN c.chargedAmount  " +

"  WHEN ISNULL(c.DiscountID, '0') = '0' AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN c.totalAmount + c.gst " +

"  WHEN ISNULL(c.DiscountID, '0') != '0' AND SUM(ISNULL(cm.calculatedValue, '0')) != 0 THEN c.chargedAmount END GrossAmount, " +

" stn.products,C.PaymentMode--CASE WHEN C.PaymentMode = '1' THEN 'CASH' WHEN C.PaymentMode = '6' THEN 'QR CODE' WHEN C.PaymentMode = '7' THEN 'CREDIT CARD' END PaymentMode \n" +

"FROM Consignment c \n" +

"INNER JOIN ServiceTypes_New stn ON stn.serviceTypeName = c.serviceTypeName \n" +

"inner join(select distinct M.DSSPNumber, D.ConsignmentNumber,m.createdon from MNP_Master_Retail_DSSP M inner join MNP_Detail_Retail_DSSP D On D.DSSPNumber = M.DSSPNumber where ExpressCenterCode = '" + ExpressCenterCode + @"' and M.CreatedOn >= '" + DateTimeStarting + @"') M on C.consignmentNumber = M.ConsignmentNumber " +

 " \n LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber = c.consignmentNumber \n" +

" \n WHERE c.originExpressCenter = '" + ExpressCenterCode + @"' AND  c.status != 9 and c.PaymentMode not in (12,6) " +

"\n GROUP BY M.DSSPNumber, m.createdon, c.consignmentNumber,c.DiscountID,c.chargedAmount,c.totalAmount,c.gst,stn.products,C.PaymentMode ) d \n" +

 "    GROUP BY DSSPNumber, d.PaymentMode, d.Products ) xb on xb.DSSPNumber = mmrd.DsspNumber \n" +

"WHERE mmrd.BookingCode IN(SELECT mm.BookingCode FROM MNP_Master_Retail_DSSP mm WHERE mm.ExpressCenterCode = '" + ExpressCenterCode + @"') AND ec.expressCenterCode = '" + ExpressCenterCode + @"') as xxb " +

" \n group by DsspNumber, CreatedOn, branch, zone, expressCenter, ExpressCenterCode, BookingCode, CNCount, CollectAmount, TotalRiderPayment, RiderPaymentLeft, Products, PaymentMode \n" +

") xc \n" +

"group by DsspNumber, CreatedOn, branch, zone, expressCenter, ExpressCenterCode, BookingCode, CNCount, CollectAmount, TotalRiderPayment, RiderPaymentLeft " +

")    h \n" +

"   WHERE ISNULL(CAST(ROUND(h.CollectAmount, 0) AS FLOAT), 0) < CAST(ROUND(h.TotalAmount, 0) + ISNULL(ROUND(h.RiderPaymentLeft, 0), 0) AS FLOAT) \n" +

 "   AND(ROUND(h.TotalAmount, 0) + ISNULL(ROUND(h.RiderPaymentLeft, 0), 0) - ROUND(h.CollectAmount, 0)) > 1 ORDER BY h.CreatedOn ";
                    orcl.Open();
                    SqlCommand orcd = new SqlCommand(query, orcl);
                    orcd.CommandType = CommandType.Text;
                    orcd.CommandTimeout = 3000;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    oda.Fill(Ds_1);
                    if (Ds_1.Rows.Count > 0)
                    {
                        tblheader.Visible = true;
                        lbl_zone.Text = Ds_1.Rows[0]["Zone"].ToString();
                        lbl_branch.Text = Ds_1.Rows[0]["branch"].ToString();
                        lbl_ecname.Text = Ds_1.Rows[0]["expressCenter"].ToString();
                        lbl_eccode.Text = Ds_1.Rows[0]["ExpressCenterCode"].ToString();
                        hd_lbleccode.Text = Ds_1.Rows[0]["ExpressCenterCode"].ToString();
                        Session["UpdatedFetchedData"] = Ds_1;

                        ReportMainGrid.DataSource = Ds_1;
                        ReportMainGrid.DataBind();
                    }
                }
            }
            catch (Exception er)
            {
                statusMsg.InnerText = er.Message.ToString();
            }
            finally
            {
                orcl.Close();
            }
        }
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
            ((LinkButton)sender).Visible = false;
            ((LinkButton)sender).Enabled = false;
            try
            {
                //   int rowIndex = Convert.ToInt32(e.CommandArgument);
                int rowIndex = row.RowIndex + 1;

                //string DSSPNumber = string.IsNullOrEmpty((row.FindControl("lblDssp") as Label).Text) ? "0" : (row.FindControl("lblDssp") as Label).Text;
                //string DSSPDate = string.IsNullOrEmpty((row.FindControl("lblDsspDate") as Label).Text) ? "0" : (row.FindControl("lblDsspDate") as Label).Text;
                //string BookingCode = string.IsNullOrEmpty((row.FindControl("lblBookingCode") as Label).Text) ? "0" : (row.FindControl("lblBookingCode") as Label).Text;

                string DSSPNumber = ReportMainGrid.Rows[rowIndex].Cells[2].Text;
                string BookingCode = ReportMainGrid.Rows[rowIndex].Cells[1].Text;
                string DSSPDate = ReportMainGrid.Rows[rowIndex].Cells[3].Text;

                Double TotalAmount = Double.Parse(string.IsNullOrEmpty(ReportMainGrid.Rows[rowIndex].Cells[5].Text) ? "0" : ReportMainGrid.Rows[rowIndex].Cells[5].Text);
                Double Outstanding = Double.Parse(string.IsNullOrEmpty(ReportMainGrid.Rows[rowIndex].Cells[8].Text) ? "0" : ReportMainGrid.Rows[rowIndex].Cells[8].Text);
                Double RiderCashTotalAmount = Double.Parse(string.IsNullOrEmpty(ReportMainGrid.Rows[rowIndex].Cells[6].Text) ? "0" : ReportMainGrid.Rows[rowIndex].Cells[6].Text);


                Double CashCollected = 0;
                Double CashDomesticCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("cashdomestic_txt") as TextBox).Text) ? "0" : (row.FindControl("cashdomestic_txt") as TextBox).Text);
                Double CashIntCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("cashInternational_txt") as TextBox).Text) ? "0" : (row.FindControl("cashInternational_txt") as TextBox).Text);
                Double CashRnRCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("cashRoadRail_txt") as TextBox).Text) ? "0" : (row.FindControl("cashRoadRail_txt") as TextBox).Text);
                CashCollected = CashDomesticCollected + CashIntCollected + CashRnRCollected;


                Double CreditCardCollected = 0;
                Double CreditCardDomesticCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("ccdomestic_txt") as TextBox).Text) ? "0" : (row.FindControl("ccdomestic_txt") as TextBox).Text);
                Double CreditCardIntCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("ccinternational_txt") as TextBox).Text) ? "0" : (row.FindControl("ccinternational_txt") as TextBox).Text);
                Double CreditCardRnRCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("ccrnr_txt") as TextBox).Text) ? "0" : (row.FindControl("ccrnr_txt") as TextBox).Text);
                CreditCardCollected = CreditCardDomesticCollected + CreditCardIntCollected + CreditCardRnRCollected;

                Double QRCollected = 0;
                Double QRDomesticCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("qrdomestic_txt") as TextBox).Text) ? "0" : (row.FindControl("qrdomestic_txt") as TextBox).Text);
                Double QRIntCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("qrInt_txt") as TextBox).Text) ? "0" : (row.FindControl("qrInt_txt") as TextBox).Text);
                Double QRRnRCollected = double.Parse(string.IsNullOrEmpty((row.FindControl("qrrnr_txt") as TextBox).Text) ? "0" : (row.FindControl("qrrnr_txt") as TextBox).Text);
                QRCollected = QRDomesticCollected + QRIntCollected + QRRnRCollected;

                Double RiderCashEntered = double.Parse(string.IsNullOrEmpty((row.FindControl("riderpayment_txt") as TextBox).Text) ? "0" : (row.FindControl("riderpayment_txt") as TextBox).Text);
                Double RiderCashRecieved = double.Parse(string.IsNullOrEmpty((row.FindControl("riderpayment_txt_hidden") as TextBox).Text) ? "0" : (row.FindControl("riderpayment_txt_hidden") as TextBox).Text);

                Double TotalCollected = CashCollected + CreditCardCollected + QRCollected + RiderCashEntered;
                string lastCollected = (row.FindControl("lblCollectedAmount") as Label).Text;

                if (TotalCollected <= 0)
                {
                    statusMsg.InnerText = "Amount cannot be less than zero: DSSP# " + DSSPNumber;
                    row.BackColor = System.Drawing.Color.Red;
                    row.ForeColor = System.Drawing.Color.White;

                    ReportMainGrid.DataSource = (DataTable)Session["UpdatedFetchedData"];
                    ReportMainGrid.DataBind();
                }
                else if (TotalCollected > Outstanding)
                {
                    statusMsg.InnerText = "Collected amount greater than dssp amount: DSSP# " + DSSPNumber;
                    row.BackColor = System.Drawing.Color.Red;
                    row.ForeColor = System.Drawing.Color.White;

                    ReportMainGrid.DataSource = (DataTable)Session["UpdatedFetchedData"];
                    ReportMainGrid.DataBind();

                }
                else if (RiderCashTotalAmount < (RiderCashRecieved + RiderCashEntered))
                {
                    statusMsg.InnerText = "Rider Collected Amount is greater than Total Rider Payment";
                    row.BackColor = System.Drawing.Color.Red;
                    row.ForeColor = System.Drawing.Color.White;
                    ReportMainGrid.DataSource = (DataTable)Session["UpdatedFetchedData"];
                    ReportMainGrid.DataBind();
                }
                else if (TotalAmount < Outstanding)
                {
                    statusMsg.InnerText = "Amount greater than dssp amount: DSSP# " + DSSPNumber;
                    row.BackColor = System.Drawing.Color.Red;
                    row.ForeColor = System.Drawing.Color.White;
                    ReportMainGrid.DataSource = (DataTable)Session["UpdatedFetchedData"];
                    ReportMainGrid.DataBind();

                }
                else
                {
                    DataTable BreakdownDt = new DataTable();
                    BreakdownDt.Columns.Add("Product", typeof(string));
                    BreakdownDt.Columns.Add("amount", typeof(float));

                    using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)))
                    {
                        string VoucherId = "";
                        string receiptNo = "";
                        conn.Open();
                        SqlCommand command = conn.CreateCommand();
                        SqlTransaction transaction;

                        transaction = conn.BeginTransaction("Insert RR");
                        command.Connection = conn;
                        command.Transaction = transaction;
                        try
                        {
                            //command.CommandText = " SELECT * FROM CreditClients cc WHERE cc.isActive='1' AND cc.accountNo='0' AND cc.expressCenterCode=@ecCodeCredit ";
                            //command.CommandType = CommandType.Text;
                            //command.Parameters.Clear();
                            //command.Parameters.AddWithValue("@ecCodeCredit", hd_lbleccode.Text);
                            //var dt = new DataTable();
                            //var oda = new SqlDataAdapter(command);
                            //oda.Fill(dt);


                            //command.CommandText = @"   SELECT z.zoneCode,b.branchCode,ec.expressCenterCode
                            //                        FROM ExpressCenters ec
                            //                        --INNER JOIN Branches b ON b.branchCode = ec.bid
                            //                        --INNER JOIN Zones z ON z.zoneCode = b.zoneCode
                            //                        WHERE ec.expressCenterCode = @ecCodeBranch";


                            command.CommandText = @"SELECT cc.zoneCode, cc.branchCode, ec.expressCenterCode, cc.accountNo, cc.id
                                                    FROM ExpressCenters ec 
                                                    INNER JOIN CreditClients cc ON ec.bid = cc.branchCode
                                                    WHERE ec.expressCenterCode = @ecCodeBranch
                                                    AND ec.[status] = '1'
                                                    AND cc.isActive = '1'
                                                    AND cc.accountNo = '0'";

                            command.CommandType = CommandType.Text;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@ecCodeBranch", hd_lbleccode.Text);
                            var dtBranch = new DataTable();
                            var odaBranch = new SqlDataAdapter(command);
                            odaBranch.Fill(dtBranch);


                            command.CommandText = " UPDATE MNP_Master_Retail_DSSP SET CollectAmount =" + (TotalAmount - Outstanding + TotalCollected) + ",ModifyOn = GETDATE(),ModifyBy=" + Session["U_ID"].ToString() + " WHERE DSSPNumber=" + DSSPNumber;
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            //command.Parameters.Clear();
                            //command.Parameters.AddWithValue("@DSSPNum", DSSPNumber);
                            //command.Parameters.AddWithValue("@CollectedAm", (TotalAmount - Outstanding + TotalCollected));
                            //command.Parameters.AddWithValue("@ModifiedBy", Session["U_ID"].ToString());

                            int PaymentSourceId = 0;
                            int loopTime = 0;

                            if (RiderCashTotalAmount <= (RiderCashEntered + RiderCashRecieved))
                            {
                                InsertRiderCashDetail(DSSPNumber, BookingCode, DSSPDate, lbl_eccode.Text, Session["branchCode"].ToString(), RiderCashEntered.ToString(), U_ID, conn, transaction);
                            }

                            while (loopTime < 3)
                            {
                                //////////
                                ///
                                BreakdownDt.Clear();

                                if (CashCollected > 0)
                                {
                                    BreakdownDt.Clear();
                                    if (CashDomesticCollected > 0)
                                    {
                                        DataRow BreakdownDtRowDomestic = BreakdownDt.NewRow();
                                        BreakdownDtRowDomestic["Product"] = "Domestic";
                                        BreakdownDtRowDomestic["amount"] = CashDomesticCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowDomestic);
                                    }

                                    if (CashIntCollected > 0)
                                    {
                                        DataRow BreakdownDtRowInt = BreakdownDt.NewRow();
                                        BreakdownDtRowInt["Product"] = "International";
                                        BreakdownDtRowInt["amount"] = CashIntCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowInt);
                                    }

                                    if (CashRnRCollected > 0)
                                    {
                                        DataRow BreakdownDtRowRnR = BreakdownDt.NewRow();
                                        BreakdownDtRowRnR["Product"] = "Road n Rail";
                                        BreakdownDtRowRnR["amount"] = CashRnRCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowRnR);
                                    }
                                    CashCollected = 0;
                                    PaymentSourceId = 1;
                                }
                                else if (CreditCardCollected > 0)
                                {
                                    BreakdownDt.Clear();

                                    if (CreditCardDomesticCollected > 0)
                                    {
                                        DataRow BreakdownDtRowDomestic = BreakdownDt.NewRow();
                                        BreakdownDtRowDomestic["Product"] = "Domestic";
                                        BreakdownDtRowDomestic["amount"] = CreditCardDomesticCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowDomestic);
                                    }
                                    if (CreditCardIntCollected > 0)
                                    {
                                        DataRow BreakdownDtRowInt = BreakdownDt.NewRow();
                                        BreakdownDtRowInt["Product"] = "International";
                                        BreakdownDtRowInt["amount"] = CreditCardIntCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowInt);
                                    }
                                    if (CreditCardRnRCollected > 0)
                                    {
                                        DataRow BreakdownDtRowRnR = BreakdownDt.NewRow();
                                        BreakdownDtRowRnR["Product"] = "Road n Rail";
                                        BreakdownDtRowRnR["amount"] = CreditCardRnRCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowRnR);
                                    }

                                    CreditCardCollected = 0;
                                    PaymentSourceId = 7;
                                }
                                else if (QRCollected > 0)
                                {
                                    BreakdownDt.Clear();
                                    if (QRDomesticCollected > 0)
                                    {
                                        DataRow BreakdownDtRowDomestic = BreakdownDt.NewRow();
                                        BreakdownDtRowDomestic["Product"] = "Domestic";
                                        BreakdownDtRowDomestic["amount"] = QRDomesticCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowDomestic);
                                    }
                                    if (QRIntCollected > 0)
                                    {

                                        DataRow BreakdownDtRowInt = BreakdownDt.NewRow();
                                        BreakdownDtRowInt["Product"] = "International";
                                        BreakdownDtRowInt["amount"] = QRIntCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowInt);
                                    }
                                    if (QRRnRCollected > 0)
                                    {

                                        DataRow BreakdownDtRowRnR = BreakdownDt.NewRow();
                                        BreakdownDtRowRnR["Product"] = "Road n Rail";
                                        BreakdownDtRowRnR["amount"] = QRRnRCollected;
                                        BreakdownDt.Rows.Add(BreakdownDtRowRnR);
                                    }
                                    loopTime = 2;
                                    QRCollected = 0;
                                    PaymentSourceId = 6;
                                }


                                if (BreakdownDt.Rows.Count > 0)
                                {
                                    command.CommandText = @"   UPDATE SystemCodes
		                            SET    codeValue = codevalue + 1,
		                                   @ReceiptNooo = codevalue
		                            WHERE  codeType = 'RECIEPT_VOUCHER'
		                                   AND [year] = YEAR(GETDATE())  ";
                                    command.CommandType = CommandType.Text;
                                    command.Parameters.Clear();
                                    IDbDataParameter Receipt = command.CreateParameter();
                                    Receipt.ParameterName = "@ReceiptNooo";
                                    Receipt.Direction = System.Data.ParameterDirection.Output;
                                    Receipt.DbType = System.Data.DbType.String;
                                    Receipt.Size = 50;
                                    command.Parameters.Add(Receipt);
                                    command.CommandTimeout = int.MaxValue;
                                    command.ExecuteNonQuery();
                                    receiptNo = Receipt.Value.ToString();


                                    // command.CommandText = "MnP_CreatePaymentVoucher_new"; MnP_CreateCashPaymentVoucher_new
                                    command.CommandText = "MnP_CreateCashPaymentVoucher_new";
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.Clear();

                                    IDbDataParameter ID = command.CreateParameter();
                                    ID.ParameterName = "@ID";
                                    ID.Direction = System.Data.ParameterDirection.Output;
                                    ID.DbType = System.Data.DbType.String;
                                    ID.Size = 50;
                                    command.Parameters.Add(ID);
                                    command.Parameters.AddWithValue("@ConsignmentNumber", DBNull.Value);
                                    //command.Parameters.AddWithValue("@Chk_Centralized", 0);
                                    // command.Parameters.AddWithValue("@Chk_PaySource", PaymentSourceId);
                                    command.Parameters.AddWithValue("@ReceiptNo", receiptNo);
                                    command.Parameters.AddWithValue("@RefNo", DSSPNumber);
                                    command.Parameters.AddWithValue("@VoucherDate", DateTime.Now);
                                    // command.Parameters.AddWithValue("@isCentralized", DBNull.Value);
                                    // command.Parameters.AddWithValue("@ClientGroupID", DBNull.Value);
                                    command.Parameters.AddWithValue("@CreditClientID", dtBranch.Rows[0]["id"].ToString());
                                    // command.Parameters.AddWithValue("@isByCreditClient", 0);
                                    command.Parameters.AddWithValue("@PaymentSourceID", PaymentSourceId); //cash  
                                                                                                          //command.Parameters.AddWithValue("@ChequeNo", DBNull.Value);
                                                                                                          //command.Parameters.AddWithValue("@ChequeDate", DBNull.Value);
                                    command.Parameters.AddWithValue("@Amount", BreakdownDt.Compute("Sum(amount)", string.Empty));
                                    command.Parameters.AddWithValue("@ZoneCode", dtBranch.Rows[0]["zoneCode"].ToString());
                                    command.Parameters.AddWithValue("@BranchCode", dtBranch.Rows[0]["branchCode"].ToString());
                                    command.Parameters.AddWithValue("@CreatedBy", Session["U_ID"].ToString());
                                    command.Parameters.AddWithValue("@PaymentTypeID", 4);
                                    command.Parameters.AddWithValue("@CashPaymentSource", "2");
                                    //command.Parameters.AddWithValue("@BankID", DBNull.Value);
                                    command.Parameters.AddWithValue("@RiderCode", DBNull.Value);
                                    //command.Parameters.AddWithValue("@AmountUsed", 0);
                                    command.Parameters.AddWithValue("@breakDown", BreakdownDt);
                                    command.Parameters.AddWithValue("@ExpressCenterCode", hd_lbleccode.Text);
                                    command.ExecuteNonQuery();
                                    VoucherId = ID.Value.ToString();
                                }
                                loopTime++;
                                ///////////
                                ///
                            }
                            transaction.Commit();

                            string url = "ReceiptVoucherDSSP.aspx?DSSP=" + DSSPNumber + "&BookingCode=" + BookingCode;

                            try
                            {
                                if (BreakdownDt.Rows.Count > 0)
                                {

                                    HttpContext.Current.Response.Redirect("ReceiptVoucherDSSP.aspx?DSSP=" + DSSPNumber + "&BookingCode=" + BookingCode);
                                }
                                statusMsg.InnerText = "Successfully updated DSSP# " + DSSPNumber;
                            }
                            catch (ThreadAbortException er)
                            {

                            }
                            //ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('" + url + "','Graph','height=400,width=500');", true);
                        }

                        catch (Exception ex)
                        {
                            statusMsg.Focus();
                            try
                            {
                                transaction.Rollback();
                                statusMsg.InnerText = "Error saving amount!";
                                //statusMsg2.InnerText = "Invalid CN amount";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount!. Not saved')", true);

                                statusMsg.Focus();
                                row.BackColor = System.Drawing.Color.Red;
                                row.ForeColor = System.Drawing.Color.White;
                                return;
                            }
                            catch (Exception ex2)
                            {
                                //statusMsg.InnerText = "Invalid amount!";
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount!. Not saved')", true);

                                //statusMsg.Focus();
                                //row.BackColor = System.Drawing.Color.Red;
                                //row.ForeColor = System.Drawing.Color.White;
                                //return;
                            }
                        }
                        finally
                        {
                            conn.Close();
                            //btn_Get_Data_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception er)
            {
                statusMsg.InnerText = "Invalid Amount";
                row.BackColor = System.Drawing.Color.Red;
                row.ForeColor = System.Drawing.Color.White;
            }
        }
        protected void ReportMainGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ReportMainGrid.PageIndex = e.NewPageIndex;
            btn_Get_Data_Click(sender, e);

        }
        protected void ReportMainGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double TotalAmount = Double.Parse(e.Row.Cells[5].Text);
                double Outstanding = Double.Parse(e.Row.Cells[8].Text);
                if (TotalAmount != Outstanding)
                {
                    TextBox t_cashDom = (TextBox)e.Row.FindControl("cashdomestic_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("cashInternational_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("cashRoadRail_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("ccdomestic_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("ccinternational_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("ccrnr_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("qrdomestic_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("qrInt_txt");
                    t_cashDom.Text = "0";

                    t_cashDom = (TextBox)e.Row.FindControl("qrrnr_txt");
                    t_cashDom.Text = "0";
                }
            }
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 9;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.CssClass = "green";
            cell.Text = "CASH";

            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = "CREDIT CARD";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 3;
            cell.Text = "QR CODE";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "RIDER CASH";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "";
            row.Controls.Add(cell);


            //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
            ReportMainGrid.HeaderRow.Parent.Controls.AddAt(0, row);
        }
        protected void InsertRiderCashDetail(string DSSPNumber, string BookingCode, string DSSPCreatedDate, string EcCode, string BranchId, string Amount, string CreatedBy, SqlConnection conn, SqlTransaction trans)
        {
            try
            {
                DataTable dtDSSP = new DataTable();
                string GetDsspDetail = @"select * from MNP_Detail_Retail_DSSP where riderCode is not null and  DSSPNumber='" + DSSPNumber + "'";
                SqlCommand orcd = new SqlCommand(GetDsspDetail, conn, trans);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 3000;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dtDSSP);

                if (dtDSSP.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (var item in dtDSSP.DefaultView)
                    {
                        var Rider = dtDSSP.Rows[i]["RiderCode"].ToString();

                        orcd.CommandText = @"Insert into CollectionCODRider" +
                            " (DSSPCode, BookingCode, RiderCode, DSSPCreatedDate, EcCode, ZoneId, BranchId, Amount, [CreatedBy], [CreatedOn]) values " +
                            " ('" + DSSPNumber + "','" + BookingCode + "','" + Rider + "','" + DSSPCreatedDate + "','" + EcCode + "','','" + BranchId + "','" + dtDSSP.Rows[i]["ChargedAmount"].ToString() + "','" + CreatedBy + "',getdate() )";
                        orcd.CommandType = CommandType.Text;
                        orcd.Transaction = trans;
                        orcd.ExecuteNonQuery();
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                conn.Close();
                throw ex;
            }
        }
    }
}