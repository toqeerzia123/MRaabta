using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Text;


namespace MRaabta.Files
{
    public partial class RecoveryReport : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();

        String zone = "", U_ID = "";
        String branch = "", BookingCode_ = "", DSSPId, CreationDatee;


        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                String DSSPNumber = "";
                if (Request.QueryString["DSSPNumber"] != null)
                {
                    DSSPNumber = Request.QueryString["DSSPNumber"].ToString();
                    if (DSSPNumber != "")
                    {
                        DSSPId = DSSPNumber;
                        GetData(DSSPNumber);
                    }
                }

                U_ID = Session["U_ID"].ToString();
                zone = Session["ZONECODE"].ToString();
                branch = Session["BRANCHCODE"].ToString();
                if (!Page.IsPostBack)
                {
                    //CreationDate.Text = System.DateTime.Now.ToString("dd-MM-yyyy");
                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }

        private void GetData(string DSSPIdTxtbox)
        {
            bool status = LoadFirstTable(DSSPIdTxtbox);
            if (!status) { return; }
            LoadSecondTable(DSSPIdTxtbox);
            LoadLastConsignmentTable(DSSPIdTxtbox);

        }

        public void CheckIfPendingExists(string dsspId, string creationdate)
        {
            DataTable Ds_1 = new DataTable();
            string dssPID = "";
            try
            {
                string sql = "SELECT DSSPNumber FROM MNP_Master_Retail_DSSP mmrd WHERE isnull(mmrd.CollectAmount,'0')='0' \n"
                   + "AND mmrd.BookingCode  in( \n"
                   + "	select mm.BookingCode   from MNP_Master_Retail_DSSP mm WHERE mm.DSSPNumber='" + DSSPId + "' --AND CAST (mmrd.CreatedOn AS date)=CAST('" + CreationDatee + "' AS date) \n"
                   + ")";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                using (SqlDataReader rdr = orcd.ExecuteReader())
                {
                    int rowcount = 0;
                    while (rdr.Read())
                    {
                        rowcount++;
                        dssPID += rdr.GetInt64(0).ToString() + ","; //The 0 stands for "the 0'th column", so the first column of the result.
                                                                    // Do somthing with this rows string, for example to put them in to a list
                        if (rowcount % 6 == 0)
                        {
                            dssPID += "<br />";
                        }
                    }
                    if (dssPID.Length > 0)
                    {
                        dssPID = dssPID.Remove(dssPID.Length - 1, 1);

                    }
                }
            }
            catch (Exception Err)
            { }
            finally
            { }
        }
        protected bool LoadFirstTable(string dsspNum)
        {
            bool status = true;
            DataTable Ds_1 = new DataTable();
            try
            {
                string sql_old = "SELECT k.zone,k.branch,k.expressCenter,k.BookingCode, \n"
                        + "format(Count(k.ConsignmentNumber), N'N0') [totalCN],format(SUM(k.ChargedAmount), N'N0') totalCnAmount,format(SUM(k.AmountCollect), N'N0') totalAmountCollect	  \n"
                        + " FROM  ( \n"
                        + "SELECT  \n"
                        + "z.name zone,b.name branch,ec.name expressCenter,rd.BookingCode,rd.ConsignmentNumber, \n"
                        + "rd.ChargedAmount,rd.AmountCollect \n"
                        + "FROM MNP_Retail_DSSP rd \n"
                        + "INNER JOIN zones z ON z.zoneCode=rd.ZoneCode \n"
                        + "INNER JOIN Branches b ON b.branchCode=rd.BranchCode \n"
                        + "INNER JOIN ExpressCenters ec ON ec.expressCenterCode=rd.OriginExpressCenter \n"
                        + "WHERE DSSPNumber ='" + dsspNum + "' AND cast(rd.CreatedOn AS date)=CAST('" + CreationDatee + "' AS date) \n"
                        + " ) k  \n"
                        + "GROUP BY k.zone,k.branch,k.expressCenter,k.BookingCode";

                string sql = "SELECT DSSPNumber, \n"
                       + "       z.name                        zone, \n"
                       + "       b.name                        branch, \n"
                       + "       ec.name                       expressCenter, \n"
                       + "       BookingCode, \n"
                       + "       CNCount totalCN ,        \n"
                       + "       TotalAmount totalCnAmount, \n"
                       + "       ISNULL(CollectAmount, 0)     totalAmountCollect, \n"
                       + "       BookingShift, \n"
                       + "       ISNULL(VehicleNo, '')         VehicleNo, \n"
                       + "       ISNULL(Remarks, '')           Remarks, \n"
                       + "       mmrd.CreatedBy, \n"
                       + "       CONVERT(varchar,mmrd.CreatedOn,111) CreatedOn \n"
                       + "FROM   MNP_Master_Retail_DSSP mmrd \n"
                       + "       INNER JOIN Zones z \n"
                       + "            ON  z.ZoneCode = mmrd.ZoneCode \n"
                       + "       INNER JOIN Branches b \n"
                       + "            ON  b.BranchCode = mmrd.BranchCode \n"
                       + "       INNER JOIN ExpressCenters ec \n"
                       + "            ON  ec.ExpressCenterCode = mmrd.ExpressCenterCode \n"
                       + "WHERE  mmrd.DSSPNumber ='" + dsspNum + "' -- AND cast(mmrd.CreatedOn AS date)=CAST('" + CreationDatee + "' AS date) ";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();

                DataSet Ds_aa = new DataSet();
                string sql_amount = @"SELECT        
                       d.products + ' (' + PaymentMode + ')' 
                       product,
                  SUM(d.[GrossAmount])     chargedAmountSum,PaymentMode 
           FROM    ( 
                      SELECT c.consignmentNumber, 
                             c.serviceTypeName, 
                             c.weight, 
                             c.pieces, 
                             c.chargedAmount, 
           
                              CASE  
           					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.totalAmount + c.gst 
           					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) = 0 THEN c.chargedAmount 
           					WHEN ISNULL(c.DiscountID,'0') = '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  
           						 c.totalAmount + c.gst + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)  
           					WHEN ISNULL(c.DiscountID,'0') != '0' and sum(ISNULL(cm.calculatedValue, '0')) != 0 THEN  
           						c.chargedAmount + SUM(cm.calculatedValue) + SUM(cm.calculatedGST) 
                             END GrossAmount,  
                             
                              stn.products, 
                             CASE  
                                         WHEN C.PaymentMode = '1' THEN 'CASH' 
                                         WHEN C.PaymentMode = '6' THEN 'QR CODE' 
                                         WHEN C.PaymentMode = '7' THEN 'CREDIT CARD' 
                                     END PaymentMode 
                      FROM   Consignment c 
							  INNER JOIN ServiceTypes_New stn 
                                  ON  stn.serviceTypeName = c.serviceTypeName 
                             LEFT JOIN ConsignmentModifier cm 
                                  ON  cm.consignmentNumber = c.consignmentNumber 
                      WHERE  
                      C.consignmentNumber IN (SELECT M.ConsignmentNumber FROM MNP_Detail_Retail_DSSP M WHERE M.DSSPNumber = '" + dsspNum + @"')

                      GROUP BY 
                             c.consignmentNumber, 
                             c.serviceTypeName, 
                             c.weight,c.DiscountID, 
                             c.pieces, 
                             c.chargedAmount, 
                             c.totalAmount, 
                             c.gst, 
                             stn.products,C.PaymentMode 
                  )                        d 
           GROUP BY 
                  d.products, PaymentMode ";
                SqlConnection orcl_a = new SqlConnection(clvar.Strcon());
                orcl_a.Open();
                SqlCommand orcd_a = new SqlCommand(sql_amount, orcl_a);
                orcd_a.CommandType = CommandType.Text;
                SqlDataAdapter oda_a = new SqlDataAdapter(orcd_a);
                oda_a.Fill(Ds_aa);
                orcl_a.Close();
                int totalAmount_aaTable = 0;
                double cash = 0;
                int cashRow = 0;
                if (Ds_aa.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < Ds_aa.Tables[0].Rows.Count; j++)
                    {
                        DataRow Currentdr = Ds_aa.Tables[0].Rows[j];

                        if (Currentdr["PaymentMode"].ToString() == "CASH")
                        {
                            cash += float.Parse(Currentdr["chargedAmountSum"].ToString());
                            cashRow = j;
                        }
                    }
                    totalAmount_aaTable = (int)Math.Round(Convert.ToDecimal(Ds_aa.Tables[0].Rows[cashRow]["chargedAmountSum"].ToString()));
                }

                if (Ds_1.Rows.Count > 0)
                {
                    //statusMsg2.InnerText = "";
                    ServiceTable.Style.Add("display", "block");
                    zoneLbl.InnerText = Ds_1.Rows[0]["zone"].ToString().ToLower();
                    branchLbl.InnerText = Ds_1.Rows[0]["branch"].ToString().ToLower();
                    ECLbl.InnerText = Ds_1.Rows[0]["expressCenter"].ToString().ToLower();
                    bookingLbl.InnerText = Ds_1.Rows[0]["BookingCode"].ToString().ToLower();
                    DSSPLbl.InnerText = DSSPId;
                    totalCNLbl.InnerText = Ds_1.Rows[0]["totalCN"].ToString();
                    //totalCNAmountLbl.InnerText = Ds_1.Rows[0]["totalCnAmount"].ToString();
                    //    SaveSheet2.Visible = true;
                    //  AllRemarks.Text = Ds_1.Rows[0]["Remarks"].ToString();


                    //int totalAmountCN = (int)Math.Round(Convert.ToDecimal(Ds_1.Rows[0]["totalCnAmount"].ToString()));
                    int totalAmountCN = totalAmount_aaTable;
                    TotalAmountCollectLbl.InnerText = totalAmountCN.ToString() + " Rs";

                    int totalAmountToCollect = (int)Math.Round(Convert.ToDecimal(Ds_1.Rows[0]["totalAmountCollect"].ToString()));
                    //totalAmountCollected.Text = Ds_1.Rows[0]["totalAmountCollect"].ToString();

                    ViewState["OldtotalAmountCollect"] = Convert.ToDecimal(Ds_1.Rows[0]["totalAmountCollect"].ToString());
                    ViewState["TotalCNAmount"] = totalAmountCN;
                    ViewState["DSSPId"] = DSSPId.ToString();
                    ViewState["CreationDatee"] = Ds_1.Rows[0]["CreatedOn"].ToString();

                }
                else
                {
                    ServiceTable.Style.Add("display", "none");

                    ServiceTable.Style.Add("display", "none");
                    //statusMsg2.InnerText = "No Data found";
                    //statusMsg2.Style.Add("font", "bold");

                    zoneLbl.InnerText = "";
                    branchLbl.InnerText = "";
                    ECLbl.InnerText = "";
                    bookingLbl.InnerText = "";
                    DSSPLbl.InnerText = "";
                    totalCNLbl.InnerText = "";
                    //totalCNAmountLbl.InnerText = "";
                    TotalAmountCollectLbl.InnerText = "";
                    status = false;
                }
            }
            catch (Exception Err)
            {
                status = false;
            }
            finally
            { }
            return status;
        }
        protected void LoadSecondTable(string dsspNum)
        {
            DataTable Ds_1 = new DataTable();
            try
            {
                string sql_old = "SELECT m.ServiceType,format(count(m.ConsignmentNumber), N'N0') totalCN,format(sum(m.AmountCollect), N'N0') totalAmount  \n"
               + "FROM MNP_Retail_DSSP m \n"
               + "WHERE m.DSSPNumber ='" + DSSPId + "' AND cast(m.CreatedOn AS date)=CAST('" + CreationDatee + "' AS date)  \n"
               + "GROUP BY m.ServiceType \n"
               + "";

                string sql_29_06_2020 = "SELECT mdrd.ServiceType,count(mdrd.ConsignmentNumber) totalCN,ROUND(sum(mdrd.ChargedAmount),0) totalAmount \n"
               + "  FROM MNP_Detail_Retail_DSSP mdrd \n"
               + "WHERE mdrd.dsspNumber='" + DSSPId + "' --AND cast(mdrd.CreatedOn AS date)=CAST('" + CreationDatee + "' AS date)\n"
               + "GROUP BY mdrd.ServiceType";

                string sql_new = @"SELECT COUNT(h.consignmentNumber)     totalCN,
                       h.ServiceType                  ServiceType,
                       SUM(h.AmountCollect)           totalAmount
                FROM   (
                           SELECT c.consignmentNumber,
                                  c.serviceTypeName     ServiceType,
                                  CEILING(
                                      (
                                          SELECT CASE 
                                                      WHEN ISNULL(c.DiscountID, '0') = '0'
                                          AND SUM(ISNULL(cm.calculatedValue, '0')) 
                                              = 0 THEN c.totalAmount + c.gst
                                              WHEN ISNULL(c.DiscountID, '0') != '0'
                                          AND SUM(ISNULL(cm.calculatedValue, '0')) 
                                              = 0 THEN c.chargedAmount
                                              WHEN ISNULL(c.DiscountID, '0') = '0'
                                          AND SUM(ISNULL(cm.calculatedValue, '0')) 
                                              != 0 THEN c.totalAmount + c.gst + SUM(cm.calculatedValue) 
                                              +
                                              SUM(cm.calculatedGST)
                                              WHEN ISNULL(c.DiscountID, '0') != '0'
                                          AND SUM(ISNULL(cm.calculatedValue, '0')) 
                                              != 0 THEN c.chargedAmount + SUM(cm.calculatedValue) 
                                              + SUM(cm.calculatedGST)
                                              END AmountCollect
                                      )
                                  )                     AmountCollect
                           FROM   Consignment c
                                  INNER JOIN ServiceTypes_New stn
                                       ON  stn.serviceTypeName = c.serviceTypeName
                                  LEFT JOIN ConsignmentModifier cm
                                       ON  cm.consignmentNumber = c.consignmentNumber
                           WHERE  C.consignmentNumber IN (SELECT M.ConsignmentNumber
                                                          FROM   MNP_Detail_Retail_DSSP M
                                                          WHERE  M.DSSPNumber = '" + DSSPId + @"')
                           GROUP BY
                                  c.consignmentNumber,
                                  c.serviceTypeName,
                                  c.DiscountID,
                                  c.BookingDate,
                                  c.chargedAmount,
                                  c.totalAmount,
                                  c.gst
                       )                              h
                GROUP BY
                       h.ServiceType";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql_new, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
                if (Ds_1.Rows.Count > 0)
                {

                    CreateServiceSummary(Ds_1);
                }
            }
            catch (Exception Err)
            { }
            finally
            { }
        }
        private void CreateServiceSummary(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;

            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn columnSingle = dt.Columns[i];
                    sb.Append("<div style='text-align:center;font-size:12px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</div>");
                }
            }
            if (rowMax == 1)
            {
                secondHeader1.Visible = false;
                secondHeader2.Visible = false;
                secondHeader3.Visible = false;
            }
            else
            {
                secondHeader1.Visible = true;
                secondHeader2.Visible = true;
                secondHeader3.Visible = true;
            }

            ServiceSummaryLiteral12.Text = sb.ToString();
        }
        protected void LoadLastConsignmentTable(string dsspNum)
        {
            DataTable Ds_1 = new DataTable();
            try
            {

                string sql_old = "SELECT m.ConsignmentNumber,convert(varchar,m.BookingDate,103) BookingDate,m.ServiceType,m.Destination, \n"
                      + "	0 DiscountCN,format(m.AmountCollect, N'N0') AmountCollect,format(m.FranchiseComission, N'N0') FranchiseComission,m.Collected_Amount,Remarks \n"
                      + "  FROM MNP_Retail_DSSP m \n"
                      + "WHERE m.DSSPNumber ='" + DSSPId + "' AND cast(m.CreatedOn AS date)=CAST('" + CreationDatee + "' AS date) ";

                string sql = "SELECT mdrd.ConsignmentNumber,convert(varchar, mdrd.BookingDate,103) BookingDate, mdrd.ServiceType, mdrd.Destination,       0 DiscountCN," +
                        "            FORMAT(mdrd.ChargedAmount, N'N0') AmountCollect, mdrd.FranchiseComission " +
                        "  FROM MNP_Detail_Retail_DSSP mdrd " +
                        "  WHERE mdrd.DSSPNumber = '" + DSSPId + "'-- AND cast(mdrd.CreatedOn AS date)=CAST('" + CreationDatee + "' AS date)  ";

                string sql_new = @"SELECT c.consignmentNumber,
               CONVERT(VARCHAR, c.BookingDate, 103) BookingDate,
               c.serviceTypeName     ServiceType,
               bd.sname              destination,
               CASE 
                    WHEN dm.DiscountValueType = 1 THEN (c.totalAmount + c.gst) - c.chargedAmount
                    ELSE ISNULL(dm.DiscountValue, 0)
               END                AS [DiscountCN],
               CEILING(
                   (
                       SELECT CASE 
                                   WHEN ISNULL(c.DiscountID, '0') = '0'
                       AND SUM(ISNULL(cm.calculatedValue, '0')) 
                           = 0 THEN c.totalAmount + c.gst
                           WHEN ISNULL(c.DiscountID, '0') != '0'
                       AND SUM(ISNULL(cm.calculatedValue, '0')) 
                           = 0 THEN c.chargedAmount
                           WHEN ISNULL(c.DiscountID, '0') = '0'
                       AND SUM(ISNULL(cm.calculatedValue, '0')) 
                           != 0 THEN c.totalAmount + c.gst + SUM(cm.calculatedValue) + 
                           SUM(cm.calculatedGST)
                           WHEN ISNULL(c.DiscountID, '0') != '0'
                       AND SUM(ISNULL(cm.calculatedValue, '0')) 
                           != 0 THEN c.chargedAmount + SUM(cm.calculatedValue) + SUM(cm.calculatedGST)
                           END AmountCollect
                   )
               )                     AmountCollect,
               0 [FranchiseComission]
        FROM   Consignment c
               INNER JOIN ServiceTypes_New stn
                    ON  stn.serviceTypeName = c.serviceTypeName
               INNER JOIN Branches bd
                    ON  bd.branchCode = c.destination
               LEFT JOIN ConsignmentModifier cm
                    ON  cm.consignmentNumber = c.consignmentNumber
               LEFT JOIN MNP_DiscountConsignment dc
                    ON  c.consignmentNumber = dc.ConsignmentNumber
               LEFT JOIN MnP_MasterDiscount dm
                    ON  dc.DiscountID = dm.DiscountID
        WHERE  C.consignmentNumber IN (SELECT M.ConsignmentNumber
                                       FROM   MNP_Detail_Retail_DSSP M
                                       WHERE  M.DSSPNumber = '" + DSSPId + @"')
        GROUP BY
               c.consignmentNumber,
               c.serviceTypeName,
               c.DiscountID,
               c.BookingDate,
               c.chargedAmount,
               c.totalAmount,
               c.gst,
               bd.sname,
               dm.DiscountValueType,
               dm.DiscountValue
               ";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql_new, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
                if (Ds_1.Rows.Count > 0)
                {
                    ConsignmentTable.DataSource = Ds_1;
                    ConsignmentTable.DataBind();
                }
            }
            catch (Exception Err)
            {
            }
            finally
            { }
        }
        protected void grvMergeHeader_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Consignment Wise Detail";
                HeaderCell.ColumnSpan = 10;
                HeaderCell.BackColor = System.Drawing.Color.Gray;
                HeaderCell.ForeColor = System.Drawing.Color.Black;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.Font.Size = 12;
                HeaderCell.Font.Bold = true;
                HeaderCell.BorderStyle = BorderStyle.Solid;
                HeaderCell.BorderColor = System.Drawing.Color.Black;
                HeaderCell.BorderWidth = 1;
                HeaderGridRow.Cells.Add(HeaderCell);

                ConsignmentTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }

        //protected void btn_Save_Data(object sender, EventArgs e)
        //{
        //    bool status = true;
        //    String CreationDate = "";
        //    String DSSPId = "";
        //    //string cntot = ViewState["TotalCNAmount"].ToString();
        //    decimal CNTotalFromDB = Convert.ToDecimal(ViewState["TotalCNAmount"].ToString());
        //    Decimal amount = Convert.ToDecimal(totalAmountCollected.Text);
        //    TextBox txt = totalAmountCollected;
        //    String Remarks = AllRemarks.Text;
        //    Decimal oldCollectedAmount = Convert.ToDecimal(ViewState["OldtotalAmountCollect"].ToString());

        //    if (amount < oldCollectedAmount)
        //    {
        //        status = false;

        //        //statusMsg2.InnerText = "Invalid CN amount";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount!. Not saved')", true);
        //        return;
        //    }
        //    if (amount < 0 || amount > CNTotalFromDB)
        //    {
        //        status = false;

        //        //statusMsg2.InnerText = "Invalid CN amount";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount!. Not saved')", true);
        //        return;
        //    }
        //    else
        //    {
        //        if (CNTotalFromDB == amount)
        //        {
        //            AllRemarks.Enabled = false;
        //            totalAmountCollected.Enabled = false;
        //        }
        //        DSSPId = ViewState["DSSPId"].ToString();
        //        CreationDate = ViewState["CreationDatee"].ToString();
        //        SendData(DSSPId, CreationDate, amount, Remarks, CNTotalFromDB);
        //    }

        //}



        //protected void btn_Save_Data(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Boolean status = true;
        //        DataTable dt_new = new DataTable();

        //        // add the columns to the datatable            
        //        if (ConsignmentTable.HeaderRow != null)
        //        {

        //            for (int i = 0; i < ConsignmentTable.HeaderRow.Cells.Count; i++)
        //            {
        //                String header = ConsignmentTable.HeaderRow.Cells[i].Text;
        //                header = header.Replace(' ', '_');
        //                dt_new.Columns.Add(header);
        //            }
        //        }

        //        //  add each of the data rows to the table
        //        foreach (GridViewRow row in ConsignmentTable.Rows)
        //        {
        //            DataRow dr;
        //            dr = dt_new.NewRow();

        //            TextBox amountCollected = (TextBox)row.Cells[6].FindControl("AmountCollectGrid");
        //            TextBox remarks = (TextBox)row.Cells[8].FindControl("RemarksCN");


        //            for (int i = 0; i < row.Cells.Count; i++)
        //            {
        //                if (i == 7)
        //                {

        //                    if (amountCollected.Text.ToString() == "")
        //                    {
        //                        dr[i] = 0;
        //                        row.Cells[7].Style.Add("color", "red");
        //                        row.Cells[7].Style.Add("background-color", "red");
        //                        row.Cells[7].Style.Add("borderColor", "red");
        //                        status = false;
        //                    }
        //                    else
        //                    {
        //                        decimal amount = Convert.ToDecimal(amountCollected.Text);
        //                        dr[i] = amount;
        //                        String amountToCollectString = row.Cells[6].Text.Replace(",", "");
        //                        decimal cnAmount = Convert.ToDecimal(amountToCollectString);
        //                        if (amount > cnAmount)
        //                        {
        //                            status = false;
        //                            row.Cells[7].Style.Add("borderColor", "red");
        //                            row.Cells[7].Style.Add("color", "red");
        //                            row.Cells[7].Style.Add("background-color", "red");
        //                        }
        //                        else if (amount == cnAmount)
        //                        {
        //                            amountCollected.Enabled = false;
        //                            remarks.Enabled = false;

        //                            row.Cells[7].Style.Add("color", "black");
        //                            row.Cells[7].Style.Add("background-color", "white");
        //                            row.Cells[7].Style.Add("borderColor", "black");
        //                        }
        //                        else
        //                        {
        //                            row.Cells[7].Style.Add("color", "black");
        //                            row.Cells[7].Style.Add("borderColor", "black");
        //                            row.Cells[7].Style.Add("background-color", "white");
        //                        }
        //                    }
        //                }
        //                else if (i == 9)
        //                {
        //                    dr[i] = remarks.Text.ToString();
        //                }
        //                else
        //                {
        //                    dr[i] = row.Cells[i].Text.Replace(" ", "");
        //                }
        //            }
        //            dt_new.Rows.Add(dr);
        //        }

        //        if (!status)
        //        {
        //            statusMsg.InnerText = "Invalid CN amount";
        //            //statusMsg2.InnerText = "Invalid CN amount";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount, record could not be saved!')", true);

        //            return;
        //        }
        //        dt_new.Columns.RemoveAt(0);
        //        SendData(dt_new);
        //    }
        //    catch (Exception er)
        //    {
        //        statusMsg.InnerText = "Invalid CN amount";
        //        //statusMsg2.InnerText = "Invalid CN amount";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount, record could not be saved!')", true);

        //    }
        //}


        //private void SendData(String DSSP, String Creation, decimal CNAmount, String remarks, decimal TotalCnAmountDB)
        //{

        //    //add created by columns and createdOn columns,branch,zone,ec,DSSPNo

        //    using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)))
        //    {
        //        conn.Open();
        //        SqlCommand command = conn.CreateCommand();
        //        SqlTransaction transaction;

        //        transaction = conn.BeginTransaction("Insert RR");
        //        command.Connection = conn;
        //        command.Transaction = transaction;
        //        try
        //        {
        //            command.CommandText = "UPDATE MNP_Master_Retail_DSSP SET Remarks =@Remarks ,CollectAmount =@CollectedAm,ModifyOn = GETDATE(),ModifyBy=@ModifiedBy WHERE DSSPNumber=@DSSPNum   AND CAST(CreatedOn AS date)=CAST(@CreationDatee AS date)";
        //            command.CommandType = CommandType.Text;
        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@DSSPNum", DSSP);
        //            command.Parameters.AddWithValue("@CreationDatee", Creation);
        //            command.Parameters.AddWithValue("@Remarks", remarks);
        //            command.Parameters.AddWithValue("@CollectedAm", CNAmount);
        //            command.Parameters.AddWithValue("@ModifiedBy", U_ID);
        //            command.ExecuteNonQuery();
        //            transaction.Commit();

        //            //statusMsg2.InnerText = "Successfully saved";
        //            ViewState["OldtotalAmountCollect"] = CNAmount;
        //            pendingAmountLbl.InnerText = (TotalCnAmountDB - CNAmount).ToString() + " Rs";

        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successfully saved!')", true);

        //            return;
        //        }

        //        catch (Exception ex)
        //        {
        //            try
        //            {
        //                transaction.Rollback();
        //                //statusMsg2.InnerText = "Invalid CN amount";
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount!. Not saved')", true);

        //                return;
        //            }
        //            catch (Exception ex2)
        //            {
        //                transaction.Rollback();
        //                //statusMsg2.InnerText = "Invalid CN amount";
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid CN amount!. Not saved')", true);

        //                return;
        //            }
        //        }

        //        finally
        //        {
        //            conn.Close();

        //        }
        //    }
        //}
        //protected void totalAmountCollected_TextChanged(object sender, EventArgs e)
        //{
        //    decimal CNTotalFromDB = Convert.ToDecimal(ViewState["TotalCNAmount"]);
        //    Decimal TotalAmount = Convert.ToDecimal(totalAmountCollected.Text);
        //    TextBox txt = totalAmountCollected;

        //    decimal amount = Convert.ToDecimal(TotalAmount);

        //    if (amount < 0 || amount > CNTotalFromDB)
        //    {

        //        txt.ForeColor = System.Drawing.Color.Red;
        //        txt.BorderColor = System.Drawing.Color.Red;
        //    }
        //    else
        //    {
        //        txt.ForeColor = System.Drawing.Color.Black;
        //        txt.BorderColor = System.Drawing.Color.Black;
        //    }
        //    //AllRemarks.Focus();

        //}
    }
}