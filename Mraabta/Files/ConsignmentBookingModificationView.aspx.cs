using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class ConsignmentBookingModificationView : System.Web.UI.Page
    {
        bool flag = false;
        Cl_Variables clvar = new Cl_Variables();
        double rowcnt = 0;
        CommonFunction func = new CommonFunction();
        string BranchName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            BranchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();
            if (!IsPostBack)
            {

                GenerateReport();
            }
        }

        protected void GenerateReport()
        {

            clvar.FromDate = DateTime.Parse(Request.QueryString["dateFrom"]);
            //clvar.ToDate = DateTime.Parse(Request.QueryString["dateTo"]);

            if (Request.QueryString["XCode"].ToUpper() != "ALL")
            {
                clvar.CheckCondition = "A.RIDERCODE, A.BookingDate";
                if (Request.QueryString["XCode"].ToUpper() == "CASH")
                    clvar.AccountNo = "and c.consignerAccountNo = '0'";
                else if (Request.QueryString["XCode"].ToUpper() == "CREDIT")
                    clvar.AccountNo = "and c.consignerAccountNo <> '0'";

                else
                {
                    return;
                }
            }
            else
            {
                clvar.CheckCondition = "A.RIDERCODE, A.BookingDate";
                clvar.AccountNo = "";
            }

            DataTable dt_ = GetConsignmentBookingDetailReport(clvar);

            StringBuilder html = new StringBuilder();
            if (dt_ == null)
            {
                ph1.Controls.Add(new Literal { Text = "No Records Found!" });
            }
            if (dt_.Rows.Count > 0)
            {

                if (Request.QueryString["Xcode"].ToUpper() != "ALL")
                {
                    clvar.CheckCondition = "'" + Request.QueryString["XCode"] + "', '', '" + clvar.FromDate.ToString("yyyy-MM-dd") + "', '" + Session["U_ID"].ToString() + "'";
                    clvar.Consigner = dt_.Rows[0]["Consigner"].ToString();

                }
                else
                {
                    clvar.CheckCondition = "'" + Request.QueryString["XCode"] + "', '', '" + clvar.FromDate.ToString("yyyy-MM-dd") + "', '" + Session["U_ID"].ToString() + "'";
                    clvar.Consigner = "ALL";
                }



                int rowcount = 0;
                //int rowcnt = 0;
                DataView view = new DataView(dt_);
                view.Sort = "riderCode";
                DataTable distinctValues = view.ToTable(true, "RiderCode");
                bool flag1 = false;




                DataTable dt = dt_;//.Select("riderCode = '" + dr["riderCode"].ToString() + "'").CopyToDataTable();
                clvar.Consigner = dt.Rows[0]["RiderName"].ToString();
                clvar.riderCode = dt.Rows[0]["RiderCode"].ToString();
                html.Append(HeaderTable(""));
                html.Append(HeaderRow());
                rowcnt = rowcnt + 6;
                flag = false;
                string ridcode = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ridcode = dt.Rows[i]["RiderCode"].ToString();
                    if (i > 0)
                        flag = true;
                    //if (i % 35 == 0 & i != 0)
                    //if (rowcount >= 35)
                    if (rowcnt >= 60)
                    {
                        if (dt.Rows[i]["RiderCode"].ToString() != dt.Rows[i - 1]["RiderCode"].ToString())
                        {
                            clvar.Consigner = dt.Rows[i]["RiderName"].ToString();
                            clvar.riderCode = dt.Rows[i]["RiderCode"].ToString();
                        }
                        //flag = true;
                        html.Append("</table>");
                        html.Append(HeaderTable(""));
                        html.Append(HeaderRow());
                        rowcnt = 6;
                    }
                    else if (i > 0)
                    {
                        if (dt.Rows[i]["RiderCode"].ToString() != dt.Rows[i - 1]["RiderCode"].ToString())
                        {
                            clvar.Consigner = dt.Rows[i]["RiderName"].ToString();
                            clvar.riderCode = dt.Rows[i]["RiderCode"].ToString();
                            //if (rowcnt + 6 >= 50)
                            //{
                            //    //flag = true;
                            //    rowcnt = 6;
                            //}
                            //else
                            //{
                            //    //flag = false;
                            //    rowcnt = rowcnt + 6;
                            //}
                            ridcode = dt.Rows[i - 1]["RiderCode"].ToString();
                            html.Append("</table>");
                            html.Append(SummaryRows(dt_, ridcode));
                            html.Append("</table>");
                            html.Append(HeaderTable(""));
                            html.Append(HeaderRow());
                            rowcnt = 6;
                        }
                    }
                    html.Append(DataRow(dt.Rows[i]));
                    rowcnt = rowcnt + 1;
                }
                html.Append("");
                html.Append("</table>");
                html.Append(SummaryRows(dt_, ridcode));
                html.Append("</table>");
                ph1.Controls.Add(new Literal { Text = html.ToString() });


                func.InsertConsignmentBookingDetailReportLog(clvar);
            }
            else
            {
                ph1.Controls.Add(new Literal { Text = "No Records Found!" });
            }

        }

        protected string HeaderTable(string headmode)
        {
            string style = "";
            if (flag)
            {
                style = "page-break-before:always;";
            }
            string sqlString = "<table style=\"width:8in; font-size: small; font-family:Calibri;" + style + "\">\n" +
                                "   <tr>\n" +
                                "       <td style=\"width: 25%; text-align: left;\"><b>Edit Date</b>:" + Request.QueryString["dateFrom"] + "</td>\n" +
                                "       <td style=\"width: 40%; text-align: center; font-size:10;\"><font size='+1'>M&P Express Logistics (Pvt) Limited</font></td>\n" +
                                "       <td style=\"width: 35%; text-align: right;\"><b>Print By</b>:" + Session["U_NAME"].ToString() + "<br/><b>Print Date</b>:" + DateTime.Now.ToString() + "</td>\n" +
                                "   </tr>\n";
            //if (Request.QueryString["mode"].ToUpper() == "CONSIGNERACCOUNTNO")
            //{
            //    sqlString += "            <tr>\n" +
            //                    "                <td style=\"width: 35%; text-align: left;\"><b>Consigner: </b>" + clvar.riderCode + " - " + clvar.Consigner + "</td>\n" +
            //                    "                <td style=\"width: 30%; text-align: center;\"><b>Consignment Booking Detail Report</b></td>\n" +
            //                    "                <td style=\"width: 35%; text-align: right;\">" + BranchName + "</td>\n" +
            //                    "            </tr>  \n";
            //}
            //else if (Request.QueryString["mode"].ToUpper() == "RIDERCODE")
            {
                int a = clvar.Consigner.Length;
                if (a > 14)
                    a = 14;
                sqlString += "<tr>\n" +
                             "  <td style=\"width: 25%; text-align: left;\"><b>Rider: </b>" + clvar.riderCode + " - " + clvar.Consigner.Substring(0, a - 1) + "</td>\n" +
                             "  <td style=\"width: 40%; text-align: center;\"><b>Consignment Edit Detail Report";
                if (Request.QueryString["tt"] == "dp")
                    sqlString += " (DP Editing) ";
                else if (Request.QueryString["tt"] == "dp")
                    sqlString += " (Accounts Editing) ";
                sqlString += "</b></td>\n" +
                             "  <td style=\"width: 35%; text-align: right;\"><b>Branch: </b>" + BranchName + "</td>\n" +
                             "</tr>  \n";
            }
            sqlString += "<tr><td><b>" + headmode + "</b></td></tr>";
            sqlString += "</table><br/>";
            return sqlString;
        }
        protected string HeaderRow()
        {

            //string sqlString = "<table style=\"width: 100%; font-size: small; font-family: Calibri;\">\n" +
            //                    "   <tr>\n" +
            //                    "       <td style=\"width: 2%;\">\n<b>Sno</b>\n</td>\n" +
            //                    "       <td style=\"width: 7%;\">\n<b>CN Number</b>\n</td>\n" +
            //                    "       <td style=\"width: 5%;\"><b>AccountNo</b>\n</td>\n" +
            //                    "       <td style=\"width: 20%;\"><b>Name</b>\n</td>\n" +
            //                    "       <td style=\"width: 3%;\">\n<b>Weight</b>\n</td>\n" +
            //                    "       <td style=\"width: 5%;\"><b>Destination</b>\n</td>\n" +
            //                    "       <td style=\"width: 3%;\"><b>Pieces</b>\n</td>\n" +
            //                    "       <td style=\"width: 6%;\"><b>Service Type</b>\n</td>\n" +
            //                    "       <td style=\"width: 5%;\"><b>CN Type</b>\n</td>\n" +
            //                    "       <td style=\"width: 4%;text-align:right;\">\n<b>TAMT</b>\n</td>\n" +
            //                    "       <td style=\"width: 4%;text-align:right;\">\n<b>GST</b>\n</td>\n" +
            //                    "       <td style=\"width: 4%;text-align:right;\">\n<b>CAMT</b>\n</td>\n" +
            //                    "       <td style=\"width: 4%;text-align:right;\">\n<b>PAMT</b>\n</td>\n";
            //if (Request.QueryString["tt"] == "dp")
            //    sqlString += "       <td style=\"width: 5%;\"><b>DP User</b>\n</td>\n" +
            //                    "       <td style=\"width: 5%;\">\n<b>DP Date</b>\n</td>\n";
            //else if (Request.QueryString["tt"] == "acc")
            //    sqlString += "       <td style=\"width: 5%;\"><b>Acc By</b>\n</td>\n" +
            //                   "       <td style=\"width: 5%;\">\n<b>Acc Date</b>\n</td>\n";
            //sqlString +="       <td style=\"width: 5%;text-align:center;\">\n<b>Approved</b>\n</td>\n" +
            //                    "   </tr>";

            string sqlString = "<table style=\"width:8in; font-size: x-small; font-family: Calibri;\">\n" +
                                "   <tr>\n" +
                                //"       <td>\n<b>Sno</b>\n</td>\n" +
                                "       <td>\n<b>CN Number</b>\n</td>\n" +
                                "       <td><b>Acc</b>\n</td>\n" +
                                "       <td><b>Name</b>\n</td>\n" +
                                "       <td>\n<b>Wgt</b>\n</td>\n" +
                                "       <td><b>Dest</b>\n</td>\n" +
                                "       <td><b>Pcs</b>\n</td>\n" +
                                "       <td><b>Ser Type</b>\n</td>\n" +
                                "       <td><b>CN Type</b>\n</td>\n" +
                                "       <td style=text-align:right;\">\n<b>TAMT</b>\n</td>\n" +
                                "       <td style=text-align:right;\">\n<b>CAMT</b>\n</td>\n" +
                                "       <td style=text-align:right;\">\n<b>PAMT</b>\n</td>\n" +
                                "       <td style=text-align:center;\">\n<b>Book Date</b>\n</td>\n";
            if (Request.QueryString["tt"] == "dp")
                sqlString += "       <td style=\"text-align:center;\"><b>DP User</b>\n</td>\n" +
                                "       <td style=\"text-align:center;\">\n<b>DP Time</b>\n</td>\n";
            else if (Request.QueryString["tt"] == "acc")
                sqlString += "       <td style=\"text-align:center;\"><b>Acc By</b>\n</td>\n" +
                               "       <td style=\"text-align:center;\">\n<b>Acc.Time</b>\n</td>\n";
            sqlString += "       <td style=text-align:center;\">\n<b>App</b>\n</td>\n" +
                                "   </tr>";

            return sqlString;
        }
        protected string DataRow(DataRow dr)
        {
            double tempTotalAmount = 0;
            double tempTotalGst = 0;
            double tempTotalChargedAmount = 0;
            double tempTotalPAMT = 0;

            double.TryParse(dr["totalAmount"].ToString(), out tempTotalAmount);
            double.TryParse(dr["gst"].ToString(), out tempTotalGst);
            double.TryParse(dr["chargedAmount"].ToString(), out tempTotalChargedAmount);
            double.TryParse(dr["PAMT"].ToString(), out tempTotalPAMT);

            string sqlString = "<tr>\n" +
                                //"   <td style=\"width:2%;\">" + dr["SNO"].ToString() + "</td>\n" +
                                "   <td style=\"width:7%;\">" + dr["consignmentNumber"].ToString() + "</td>\n" +
                                "   <td style=\"width:6%;\">" + dr["consignerAccountNo"].ToString() + "</td>\n" +
                                "   <td style=\"width:20%;\">" + dr["consignee"].ToString() + "</td>\n" +
                                "   <td style=\"width:3%;\">" + dr["weight"].ToString() + "</td>\n" +
                                "   <td style=\"width:3%;\">" + dr["Destination"].ToString() + "</td>\n" +
                                "   <td style=\"width:2%;\">" + dr["pieces"].ToString() + "</td>\n" +
                                "   <td style=\"width:9%;\">" + dr["serviceTypeName"].ToString() + "</td>\n" +
                                "   <td style=\"width:10%;\">" + dr["ConsignmentType"].ToString() + "</td>\n" +
                                "   <td style=\"text-align:right;width:3%;\">" + String.Format("{0:0,0}", tempTotalAmount + tempTotalGst) + "</td>\n" +
                                //"   <td style=\"text-align:right;\">" + String.Format("{0:N2}", tempTotalGst) + "</td>\n" +
                                "   <td style=\"text-align:right;width:3%;\">" + String.Format("{0:0,0}", tempTotalChargedAmount) + "</td>\n" +
                                "   <td style=\"text-align:right;width:3%;\">" + String.Format("{0:0,0}", tempTotalPAMT) + "</td>\n" +
                                "   <td style=\"text-align:center;width:10%;\">" + dr["bookingdate"].ToString() + "</td>\n";
            if (Request.QueryString["tt"] == "dp")
            {

                //   sqlString += "   <td></td><td></td>\n";

                sqlString += "   <td style=\"text-align:center;width:14%;\">" + dr["DPUser"].ToString() + "</td>\n" +
                                    "   <td style=\"text-align:center;width:4%;\">" + dr["DPChangeAt"].ToString() + "</td>\n";

            }
            else if (Request.QueryString["tt"] == "acc")
            {
                sqlString += "   <td style=\"text-align:center;width:14%;\">" + dr["AccUser"].ToString() + "</td>\n" +
                "   <td style=\"text-align:center;width:4%;\">" + dr["AccChangeAt"].ToString() + "</td>\n";
            }
            sqlString += "   <td style=\"text-align:center;width:3%;\">" + dr["Approved"].ToString() + "</td><td></td>\n" +
                            //"   <td>" + dr["BookingDate"].ToString() + "</td>\n" +
                            "</tr>";
            return sqlString;
        }

        protected string SummaryRows(DataTable dt, string rider)
        {
            DataRow[] dr = dt.Select("RiderCode = '" + rider + "'");
            DataTable dtSumm = dr.CopyToDataTable();
            dt = dtSumm;
            DataView view = new DataView(dt);
            DataTable distinctProducts = view.ToTable(true, "ServiceTypeName");
            string sqlString = "";
            //if (distinctProducts.Rows.Count + (dt.Rows.Count % 30) + 5 > 60)
            //{
            //    flag = true;
            //    sqlString += HeaderTable("Shipment Summaries");
            //}
            //else
            //{
            //    flag = false;
            //    sqlString += HeaderTable("Shipment Summaries");
            //}
            sqlString += "<table style=\"Width:8in; font-family:Calibri; font-size:10px; border-top: 1px dashed black \">\n" +
            "        <tr>\n" +
            "            <td style=\"width: 40%;\">\n" +
            "                <b>Product</b>\n" +
            "            </td>\n" +
            "            <td style=\"text-align:right; padding-right:5px;\">\n" +
            "                <b>Total Shipment</b>\n" +
            "            </td>\n" +
            "            <td style=\"text-align:right; padding-right:5px;\">\n" +
            "                <b>Total Amt (Amt + Tax)</b>\n" +
            "            </td>\n" +
            "            <td style=\"text-align:right; padding-right:5px;\">\n" +
            "                <b>Modifier Amt (Amt + Tax)</b>\n" +
            "            </td>\n" +
            "            <td style=\"text-align:right; padding-right:5px;\">\n" +
            "                <b>Charged Amt</b>\n" +
            "            </td>\n" +
            "            <td style=\"text-align:right; padding-right:5px;\">\n" +
            "                <b>Difference</b>\n" +
            "            </td>\n" +
            "        </tr>";
            //DataView view = new DataView(dt);
            //DataTable distinctProducts = view.ToTable(true, "ServiceTypeName");

            foreach (DataRow row in distinctProducts.Rows)
            {
                DataRow[] tempRow = dt.Select("serviceTypeName = '" + row[0].ToString() + "'", "");
                if (tempRow.Count() > 0)
                {
                    DataTable tempdt = tempRow.CopyToDataTable();
                    double totalAmount = 0;
                    double totalTax = 0;
                    double totalChargedAmount = 0;
                    double totalPAMT = 0;

                    object obj = tempdt.Compute("SUM(totalAmount)", "");
                    double.TryParse(obj.ToString(), out totalAmount);

                    obj = tempdt.Compute("SUM(gst)", "");
                    double.TryParse(obj.ToString(), out totalTax);

                    obj = tempdt.Compute("SUM(ChargedAmount)", "");
                    double.TryParse(obj.ToString(), out totalChargedAmount);

                    obj = tempdt.Compute("SUM(PAMT)", "");
                    double.TryParse(obj.ToString(), out totalPAMT);

                    sqlString += "        <tr>\n" +
                                "            <td>\n" +
                                "                " + row[0].ToString() + "\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:N0}", tempdt.Rows.Count) + "\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:0,0}", (totalAmount + totalTax)) + "\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:0,0}", (totalPAMT)) + "\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:0,0}", totalChargedAmount) + "\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:0,0}", ((totalAmount + totalTax) - totalChargedAmount)) + "\n" +
                                "            </td>\n" +
                                "        </tr>";
                }
            }

            double grandTotalAmount = 0;
            double grandTotalTax = 0;
            double grandTotalChargedAmount = 0;
            double grandTotalPAMT = 0;

            object gobj = dt.Compute("SUM(totalAmount)", "");
            double.TryParse(gobj.ToString(), out grandTotalAmount);

            gobj = dt.Compute("SUM(gst)", "");
            double.TryParse(gobj.ToString(), out grandTotalTax);

            gobj = dt.Compute("SUM(ChargedAmount)", "");
            double.TryParse(gobj.ToString(), out grandTotalChargedAmount);

            gobj = dt.Compute("SUM(PAMT)", "");
            double.TryParse(gobj.ToString(), out grandTotalPAMT);


            sqlString += "        <tr>\n" +
                                "            <td >\n" +
                                "                <b>Grand Total</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:N0}", dt.Rows.Count) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:0,0}", (grandTotalAmount + grandTotalTax)) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:0,0}", (grandTotalPAMT)) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:0,0}", grandTotalChargedAmount) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:0,0}", ((grandTotalAmount + grandTotalTax) - grandTotalChargedAmount)) + "</b>\n" +
                                "            </td>\n" +
                                "        </tr>";
            //"        </tr></table>";
            return sqlString;

        }


        public DataTable GetConsignmentBookingDetailReport(Cl_Variables clvar)
        {

            string sqlString =//" + clvar.CheckCondition + "
                @"SELECT ROW_NUMBER() OVER(ORDER BY consignmentNumber ) SNO, A.* 
                FROM 
                (SELECT c.consignmentNumber, c.creditClientId, b.name Origin, left(c.serviceTypeName,12) as serviceTypeName, c.consigner, left(cc.name,20) consignee, b2.sname Destination," +
            "               c.weight, CAST(c.riderCode AS INT) riderCode, ec.name OriginExpressCenter, left(ct.name,10) as ConsignmentType, round(c.chargedAmount,0) as chargedAmount, " +
            "               CASE WHEN CAST(c.isApproved AS VARCHAR) = '1' THEN 'YES' ELSE 'NO' END Approved," +
            "               c.consignerAccountNo, c.accountReceivingDate, CONVERT(VARCHAR, c.bookingDate, 106) bookingdate, c.COD, z.name CreatedBy, " +
            "               c.CreatedOn, r.firstName + ' ' + r.lastName RiderName, round(c.totalAmount,0) as totalAmount,\n" +
            "               round((SUM(cm.calculatedValue) + SUM(cm.calculatedGST)),0) PAMT, round(c.gst,0) as gst, c.pieces, isnull((select left(name,16) from ZNI_USER1 where u_id = c.DPUser),'-') as DPUser, left(CONVERT(time, c.DPChangeAt),8) as DPChangeAt, isnull((select left(name,16) from ZNI_USER1 where u_id = c.ACCUser),'-') as ACCUser, left(CONVERT(time, c.ACCChangeAt),8) as ACCChangeAt\n" +
            "          FROM consignment c\n" +
            "          INNER JOIN CreditClients cc ON c.creditClientId = cc.id\n" +
            "          INNER JOIN Branches b ON b.branchCode = c.orgin\n" +
            "          INNER JOIN Branches b2 ON b2.branchCode = c.destination\n" +
            "          INNER JOIN ExpressCenters ec ON ec.expressCenterCode = c.originExpressCenter\n" +
            "          INNER JOIN ConsignmentType ct ON CAST(c.consignmentTypeId AS VARCHAR) = CAST(ct.id AS VARCHAR)\n" +
            "          LEFT OUTER JOIN ZNI_USER1 z ON CAST(z.U_ID AS VARCHAR) = c.createdBy\n" +
            "          LEFT JOIN ZNI_USER1 zdp ON CAST(zdp.U_ID AS VARCHAR) = c.DPUser\n" +
            "          LEFT JOIN ZNI_USER1 zacc ON CAST(zacc.U_ID AS VARCHAR) = c.ACCUser\n" +
            "          LEFT OUTER JOIN Riders r ON r.riderCode = c.riderCode AND r.branchId = c.branchCode\n" +
            "          LEFT OUTER JOIN ConsignmentModifier cm ON cm.consignmentNumber = c.consignmentNumber\n" +
            "          WHERE CONVERT(date, c.createdon, 105) >= '2019-08-01'\n";
            //"          AND CONVERT(date, c.accountReceivingDate, 105) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            if (Request.QueryString["tt"] == "dp")
                sqlString += " and c.DPChangeAt >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + " 09:00:00.000' and c.DPChangeAt < '" + clvar.FromDate.AddDays(1).ToString("yyyy-MM-dd") + " 09:00:00.000'";
            //sqlString += " and CONVERT(date, c.dpchangeat, 105) = '" + clvar.FromDate.ToString("yyyy-MM-dd") + "'\n";
            else
                sqlString += " and CONVERT(date, c.accchangeat, 105) = '" + clvar.FromDate.ToString("yyyy-MM-dd") + "'\n";

            sqlString += "          and c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' " + clvar.AccountNo + " and r.status = '1'\n" +
            "          GROUP BY c.consignmentNumber, c.creditClientId, b.name, c.serviceTypeName, c.consigner, cc.name,\n" +
            "               b2.sname, c.weight, c.riderCode, ec.name, ct.name, c.chargedAmount, c.isApproved, c.consignerAccountNo,\n" +
            "               c.accountReceivingDate, c.bookingDate, c.COD, z.name, c.CreatedOn, r.firstName, r.lastName, c.totalAmount, c.gst, c.pieces, c.DPUser, c.DPChangeAt, c.ACCUser, c.ACCChangeAt ) A\n" +
            "          ORDER BY RiderCode, consignmentNumber ";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.SelectCommand.CommandTimeout = 300000;

                sda.Fill(dt);
            }
            catch (Exception ex) { }
            finally { con.Close(); }
            return dt;
        }
    }
}