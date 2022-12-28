using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class ConsignmentBookingDetailView : System.Web.UI.Page
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
            clvar.ToDate = DateTime.Parse(Request.QueryString["dateTo"]);

            if (Request.QueryString["XCode"].ToUpper() != "ALL")
            {
                if (Request.QueryString["mode"].ToUpper() == "CONSIGNERACCOUNTNO")
                {
                    clvar.CheckCondition = "A.RIDERCODE, A." + Request.QueryString["sort"];
                    clvar.AccountNo = "and c.consignerAccountNo = '" + Request.QueryString["XCode"] + "'";
                }
                else if (Request.QueryString["mode"].ToUpper() == "RIDERCODE")
                {
                    clvar.CheckCondition = "A.RIDERCODE, A." + Request.QueryString["sort"];
                    clvar.AccountNo = "and c.RIDERCODE = '" + Request.QueryString["XCode"] + "'";
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (Request.QueryString["mode"].ToUpper() == "CONSIGNERACCOUNTNO")
                {
                    clvar.CheckCondition = "A.RIDERCODE, A." + Request.QueryString["sort"];
                    clvar.AccountNo = "";
                }
                else if (Request.QueryString["mode"].ToUpper() == "RIDERCODE")
                {
                    clvar.CheckCondition = "A.RiderCode, A." + Request.QueryString["sort"];
                    clvar.AccountNo = "";
                }
                else
                {
                    return;
                }
            }


            //clvar.AccountNo = Request.QueryString["XCode"];


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
                    if (Request.QueryString["mode"].ToUpper() == "CONSIGNERACCOUNTNO")
                    {
                        clvar.CheckCondition = "'" + Request.QueryString["mode"] + "', '" + Request.QueryString["XCode"] + "', '', '" + clvar.FromDate.ToString("yyyy-MM-dd") + "', '" + clvar.ToDate.ToString("yyyy-MM-dd") + "','" + Request.QueryString["sort"] + "', '" + Session["U_ID"].ToString() + "'";
                        clvar.Consigner = dt_.Rows[0]["Consigner"].ToString();
                    }
                    else if (Request.QueryString["mode"].ToUpper() == "RIDERCODE")
                    {
                        clvar.CheckCondition = "'" + Request.QueryString["mode"] + "', '', '" + Request.QueryString["XCode"] + "', '" + clvar.FromDate.ToString("yyyy-MM-dd") + "', '" + clvar.ToDate.ToString("yyyy-MM-dd") + "','" + Request.QueryString["sort"] + "', '" + Session["U_ID"].ToString() + "'";
                        clvar.Consigner = dt_.Rows[0]["RiderName"].ToString();
                    }
                }
                else
                {
                    if (Request.QueryString["mode"].ToUpper() == "CONSIGNERACCOUNTNO")
                    {
                        clvar.CheckCondition = "'" + Request.QueryString["mode"] + "', '" + Request.QueryString["XCode"] + "', '', '" + clvar.FromDate.ToString("yyyy-MM-dd") + "', '" + clvar.ToDate.ToString("yyyy-MM-dd") + "','" + Request.QueryString["sort"] + "', '" + Session["U_ID"].ToString() + "'";
                        clvar.Consigner = "ALL";
                    }
                    else if (Request.QueryString["mode"].ToUpper() == "RIDERCODE")
                    {
                        clvar.CheckCondition = "'" + Request.QueryString["mode"] + "', '', '" + Request.QueryString["XCode"] + "', '" + clvar.FromDate.ToString("yyyy-MM-dd") + "', '" + clvar.ToDate.ToString("yyyy-MM-dd") + "','" + Request.QueryString["sort"] + "', '" + Session["U_ID"].ToString() + "'";
                        clvar.Consigner = "ALL";
                    }
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (i % 35 == 0 & i != 0)
                    //if (rowcount >= 35)
                    if (rowcnt >= 35)
                    {
                        if (dt.Rows[i]["RiderCode"].ToString() != dt.Rows[i - 1]["RiderCode"].ToString())
                        {
                            clvar.Consigner = dt.Rows[i]["RiderName"].ToString();
                            clvar.riderCode = dt.Rows[i]["RiderCode"].ToString();
                        }
                        flag = true;
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

                            if (rowcnt + 6 >= 35)
                            {
                                flag = true;
                                rowcnt = 6;
                            }
                            else
                            {
                                flag = false;
                                rowcnt = rowcnt + 6;
                            }
                            html.Append("</table>");
                            html.Append(HeaderTable(""));
                            html.Append(HeaderRow());

                        }
                    }
                    html.Append(DataRow(dt.Rows[i]));
                    rowcnt = rowcnt + 1;
                }
                html.Append("");
                html.Append("</table>");



                html.Append(SummaryRows(dt_));
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
            string sqlString = "<table style=\"width: 100%; font-size: small; font-family:Calibri;" + style + "\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 35%; text-align: left;\">\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center;\">\n" +
            "                    M&P Express & Logistics\n" +
            "                </td>\n" +
            "                <td style=\"width: 35%; text-align: right;\">\n" +

            "                </td>\n" +
            "            </tr>\n";
            if (Request.QueryString["mode"].ToUpper() == "CONSIGNERACCOUNTNO")
            {
                sqlString += "            <tr>\n" +
                            "                <td style=\"width: 35%; text-align: left;\">\n" +

                            "                </td>\n" +
                            "                <td style=\"width: 30%; text-align: center;\">\n" +
                            "                <b>" + BranchName + "</b>\n" +
                            "                </td>\n" +
                            "                <td style=\"width: 35%; text-align: right;\">\n" +

                            "                </td>\n" +
                            "            </tr>  \n" +
                            "            <tr>\n" +
                            "                <td style=\"width: 35%; text-align: left;\">\n" +
                            //"                <b>Account #:</b>" + Request.QueryString["XCode"] + "" +
                            "                </td>\n" +
                            "                <td style=\"width: 30%; text-align: center;\">\n" +
                            "                <b>Consignment Booking Detail Report</b>\n" +
                            "                </td>\n" +
                            "                <td style=\"width: 35%; text-align: right;\">\n" +
                            "                    <b>Print Date</b>:" + DateTime.Now.ToString() + "\n" +
                            "                </td>\n" +
                            "            </tr>  \n" +
                            "            <tr>\n" +
                            "                <td style=\"width: 35%; text-align: left;\">\n" +
                            "                <b>Account #:</b>" + clvar.riderCode + "" +
                            "                </td>\n" +
                            "                <td style=\"width: 30%; text-align: center;\">\n" +
                            "                <b>" + HttpContext.Current.Session["U_NAME"].ToString() + "</b>\n" +
                            "                </td>\n" +
                            "                <td style=\"width: 35%; text-align: right;\">\n" +
                            //"                    <b>Print Date</b>:" + DateTime.Now.ToString() + "\n" +
                            "                </td>\n" +
                            "            </tr>  \n" +

                            "            <tr>\n" +
                            "                <td style=\"width: 35%; text-align: left;\">\n" +
                            "                <b>DSSPNumber: </b>" + clvar.Consigner + "" +
                            "                </td>\n" +
                            "                <td style=\"width: 30%; text-align: center;\">\n" +
                            "                <b>" + headmode + "</b>\n" +
                            "                </td>\n" +
                            "                <td style=\"width: 35%; text-align: right;\">\n" +
                            "                    \n" +
                            "                </td>\n" +
                            "            </tr>" +
                            "";
            }
            else if (Request.QueryString["mode"].ToUpper() == "RIDERCODE")
            {
                sqlString += "            <tr>\n" +
                            "                <td style=\"width: 35%; text-align: left;\">\n" +

                            "                </td>\n" +
                            "                <td style=\"width: 30%; text-align: center;\">\n" +
                            "                <b>" + BranchName + "</b>\n" +
                            "                </td>\n" +
                            "                <td style=\"width: 35%; text-align: right;\">\n" +

                            "                </td>\n" +
                            "            </tr>  \n" +
                             "            <tr>\n" +
                             "                <td style=\"width: 35%; text-align: left;\">\n" +
                             //"                <b>Rider Code #:</b>" + Request.QueryString["XCode"] + "" +
                             "                </td>\n" +
                             "                <td style=\"width: 30%; text-align: center;\">\n" +
                             "                <b>Consignment Booking Detail Report</b>\n" +
                             "                </td>\n" +
                             "                <td style=\"width: 35%; text-align: right;\">\n" +
                             "                    <b>Print Date</b>:" + DateTime.Now.ToString() + "\n" +
                             "                </td>\n" +
                             "            </tr> \n" +
                             "            <tr>\n" +
                             "                <td style=\"width: 35%; text-align: left;\">\n" +
                             "                <b>Rider Code #:</b>" + clvar.riderCode + "" +
                             "                </td>\n" +
                             "                <td style=\"width: 30%; text-align: center;\">\n" +
                             "                <b>" + Session["U_NAME"].ToString() + "</b>\n" +
                             "                </td>\n" +
                             "                <td style=\"width: 35%; text-align: right;\">\n" +
                             "                    \n" +
                             "                </td>\n" +
                             "            </tr>" +
                             "            <tr>\n" +
                             "                <td style=\"width: 35%; text-align: left;\">\n" +
                             "                <b>RiderName: </b>" + clvar.Consigner + "" +
                             "                </td>\n" +
                             "                <td style=\"width: 30%; text-align: center;\">\n" +
                             "                <b>" + headmode + "</b>\n" +
                             "                </td>\n" +
                             "                <td style=\"width: 35%; text-align: right;\">\n" +
                             "                    \n" +
                             "                </td>\n" +
                             "            </tr>" +
                             "";
            }

            sqlString += " </table>";
            return sqlString;
        }
        protected string HeaderRow()
        {

            string sqlString = "<table style=\"width: 100%; font-size: xx-small; font-family: Calibri;\">\n" +
            "            <tr>\n" +
            "                <td>\n" +
            "                    <b>Sno</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>CN Number</b>\n" +
            "                </td>\n";

            //sqlString += "                <td>\n" +
            //            "                    <b>RiderCode</b>\n" +
            //            "                </td>\n";

            sqlString += "                <td>\n" +
                         "                    <b>AccountNo</b>\n" +
                         "                </td>\n";

            sqlString += "                <td>\n" +
            "                    <b>Name</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>Weight</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>Pieces</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>DSSP No</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>Service Type</b>\n" +
            "                </td>\n";


            sqlString +=
            "                <td>\n" +
            "                    <b>CN Type</b>\n" +
            "                </td>\n" +
            "                <td style=\"text-align:right;\">\n" +
            "                    <b>TAMT + GST + CAMT + PAMT</b>\n" +
            "                </td>\n" +
            "                <td style=\"text-align:center;\">\n" +
            "                    <b>Approved</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>Booking Date</b>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }
        protected string DataRow(DataRow dr)
        {

            string sqlString = "            <tr>\n" +
                "                <td>\n" +
            "                   " + dr["SNO"].ToString() + "\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                   " + dr["consignmentNumber"].ToString() + "\n" +
            "                </td>\n";

            //sqlString += "                <td>\n" +
            //            "                    " + dr["riderCode"].ToString() + "\n" +
            //            "                </td>\n";

            sqlString += "                <td>\n" +
                         "                    " + dr["consignerAccountNo"].ToString() + "\n" +
                         "                </td>\n";

            sqlString +=
            "                <td>\n" +
            "                   " + dr["consignee"].ToString() + "\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                   " + dr["weight"].ToString() + "\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                   " + dr["Destination"].ToString() + "\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                   " + dr["pieces"].ToString() + "\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                   " + dr["Consigner"].ToString() + "\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                   " + dr["serviceTypeName"].ToString() + "\n" +
            "                </td>\n";


            double tempTotalAmount = 0;
            double tempTotalGst = 0;
            double tempTotalChargedAmount = 0;
            double tempTotalPAMT = 0;

            double.TryParse(dr["totalAmount"].ToString(), out tempTotalAmount);
            double.TryParse(dr["gst"].ToString(), out tempTotalGst);
            double.TryParse(dr["chargedAmount"].ToString(), out tempTotalChargedAmount);
            double.TryParse(dr["PAMT"].ToString(), out tempTotalPAMT);

            sqlString += "                <td>\n" +
            "                   " + dr["ConsignmentType"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"text-align:right;\">\n" +
            "                   " + String.Format("{0:N2}", tempTotalAmount) + " + " + String.Format("{0:N2}", tempTotalGst) + " + " + String.Format("{0:N2}", tempTotalChargedAmount) + " + " + String.Format("{0:N2}", tempTotalPAMT) + "\n" +
            "                </td>\n" +
            "                <td style=\"text-align:center;\">\n" +
            "                   " + dr["Approved"].ToString() + "\n" +
            "                </td>\n" +
            //"                <td>\n" +
            //"                   " + dr["invoiceNumber"].ToString() + "\n" +
            //"                </td>\n" +
            //"                <td>\n" +
            //"                   " + dr["CreatedBy"].ToString() + "\n" +
            //"                </td>\n" +
            "                <td>\n" +
            "                   " + dr["BookingDate"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        protected string SummaryRows(DataTable dt)
        {
            DataView view = new DataView(dt);
            DataTable distinctProducts = view.ToTable(true, "ServiceTypeName");
            string sqlString = "";
            if (distinctProducts.Rows.Count + (dt.Rows.Count % 30) + 5 > 35)
            {
                flag = true;
                sqlString += HeaderTable("Shipment Summaries");
            }
            else
            {
                flag = false;
                sqlString += HeaderTable("Shipment Summaries");
            }
            sqlString += "<table style=\"Width:100%; font-family:Calibri; \">\n" +
            "        <tr>\n" +
            "            <td style=\"Width:25%\">\n" +
            "                <b>Product</b>\n" +
            "            </td>\n" +
            "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
            "                <b>Total Shipment</b>\n" +
            "            </td>\n" +
            "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
            "                <b>Total Amt (Amt + Tax)</b>\n" +
            "            </td>\n" +
            "            <td style=\"Width:15 %; text-align:right; padding-right:5px;\">\n" +
            "                <b>Modifier Amt (Amt + Tax)</b>\n" +
            "            </td>\n" +
            "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
            "                <b>Charged Amt</b>\n" +
            "            </td>\n" +
            "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
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
                                "            <td style=\"Width:25%\">\n" +
                                "                " + row[0].ToString() + "\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:N0}", tempdt.Rows.Count) + "\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:N2}", (totalAmount + totalTax)) + "\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:N2}", (totalPAMT)) + "\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:N2}", totalChargedAmount) + "\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                " + String.Format("{0:N2}", ((totalAmount + totalTax) - totalChargedAmount)) + "\n" +
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
                                "            <td style=\"Width:35%\">\n" +
                                "                <b>Grand Total</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:N0}", dt.Rows.Count) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:20%; text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:N2}", (grandTotalAmount + grandTotalTax)) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:20%; text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:N2}", (grandTotalPAMT)) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:N2}", grandTotalChargedAmount) + "</b>\n" +
                                "            </td>\n" +
                                "            <td style=\"Width:15%; text-align:right; padding-right:5px;\">\n" +
                                "                <b>" + String.Format("{0:N2}", ((grandTotalAmount + grandTotalTax) - grandTotalChargedAmount)) + "</b>\n" +
                                "            </td>\n" +
                                "        </tr></table>";
            return sqlString;

        }


        public DataTable GetConsignmentBookingDetailReport(Cl_Variables clvar)
        {

            string sqlString = "SELECT ROW_NUMBER()OVER(order by A." + clvar.CheckCondition + ") SNO , A.*,\n" +
            "       CASE\n" +
            "         WHEN i.IsInvoiceCanceled = '1' THEN\n" +
            "          ''\n" +
            "         ELSE\n" +
            "          i.invoiceNumber\n" +
            "       END invoiceNumber_\n" +
            "  FROM (SELECT c.consignmentNumber,\n" +
            "               c.creditClientId,\n" +
            "               b.name Origin,\n" +
            "               c.serviceTypeName,\n" +
            "               c.consigner,\n" +
            "               cc.name consignee,\n" +
            "               b2.sname Destination,\n" +
            "               round(c.weight,2) weight,\n" +
            "               c.riderCode,\n" +
            "               ec.name OriginExpressCenter,\n" +
            "               ct.name ConsignmentType,\n" +
            "               c.chargedAmount,\n" +
            "               CASE WHEN CAST(c.isApproved AS VARCHAR) = '1' then 'YES' ELSE 'NO' END Approved,\n" +
            "               c.consignerAccountNo accountNo,\n" +
            "               cm.priceModifierId,\n" +
            "               p.name priceModifierName,\n" +
            "               cm.calculatedValue,\n" +
            "               cm.calculationBase,\n" +
            "               cm.isTaxable,\n" +
            "               cm.SortOrder,\n" +
            "               p.description,\n" +
            "               ec2.name DestinationExpressCenter,\n" +
            "               c.accountReceivingDate,\n" +
            "               cast(c.bookingDate as date) bookingDate,\n" +
            "               c.COD,\n" +
            "               ic.invoiceNumber,\n" +
            "               z.name CreatedBy, c.CreatedOn, c.pieces\n" +
            "\n" +
            "          FROM consignment c\n" +
            "          LEFT OUTER JOIN InvoiceConsignment ic\n" +
            "            ON c.consignmentNumber = ic.consignmentNumber\n" +
            "          LEFT OUTER JOIN ConsignmentModifier cm\n" +
            "            ON c.consignmentNumber = cm.consignmentNumber\n" +
            "          LEFT OUTER JOIN PriceModifiers p\n" +
            "            ON cm.priceModifierId = p.id\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "         inner join Branches b\n" +
            "            on b.branchCode = c.orgin\n" +
            "         inner join Branches b2\n" +
            "            on b2.branchCode = c.destination\n" +
            "         inner join ExpressCenters ec\n" +
            "            on ec.expressCenterCode = c.originExpressCenter\n" +
            "         inner join ExpressCenters ec2\n" +
            "            on ec2.expressCenterCode = c.destinationExpressCenterCode\n" +
            "         inner join ConsignmentType ct\n" +
            "            on CAST(c.consignmentTypeId as varchar) = CAST(ct.id as varchar)\n" +
            "          left outer join ZNI_USER1 z\n" +
            "            on CAST(z.U_ID as varchar) = c.createdBy\n" +
            "         WHERE CONVERT(date, c.bookingDate, 105) >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND CONVERT(date, c.bookingDate, 105) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "           and c.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' " + clvar.AccountNo + "\n" +
            "\n" +
            "        ) A\n" +
            "  LEFT OUTER JOIN Invoice i\n" +
            "    ON A.invoiceNumber = i.invoiceNumber\n" +
            " ORDER BY A." + clvar.CheckCondition + "";



            sqlString = "SELECT ROW_NUMBER()OVER(order by " + clvar.CheckCondition + ") SNO , A.*\n" +
            "  FROM (SELECT c.consignmentNumber,\n" +
            "               c.creditClientId,\n" +
            "               b.name Origin,\n" +
            "               c.serviceTypeName,\n" +
            "               c.consigner,\n" +
            "               cc.name consignee,\n" +
            "               b2.sname Destination,\n" +
            "               c.weight,\n" +
            "              CAST(c.riderCode as int) riderCode,\n" +
            "               ec.name OriginExpressCenter,\n" +
            "               ct.name ConsignmentType,\n" +
            "               c.chargedAmount,\n" +
            "               CASE WHEN CAST(c.isApproved AS VARCHAR) = '1' then 'YES' ELSE 'NO' END Approved,\n" +
            "               c.consignerAccountNo,\n" +

            "               ec2.name DestinationExpressCenter,\n" +
            "               c.accountReceivingDate,\n" +
            "               Convert(varchar, c.bookingDate, 106) bookingdate,\n" +
            "               c.COD,\n" +
           "               z.name CreatedBy, c.CreatedOn, r.firstName + ' ' + r.lastName RiderName, c.totalAmount, c.gst, c.pieces\n" +
            "\n" +
            "          FROM consignment c\n" +

            "         INNER JOIN CreditClients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "         inner join Branches b\n" +
            "            on b.branchCode = c.orgin\n" +
            "         inner join Branches b2\n" +
            "            on b2.branchCode = c.destination\n" +
            "         inner join ExpressCenters ec\n" +
            "            on ec.expressCenterCode = c.originExpressCenter\n" +
            "         inner join ExpressCenters ec2\n" +
            "            on ec2.expressCenterCode = c.destinationExpressCenterCode\n" +
            "         inner join ConsignmentType ct\n" +
            "            on CAST(c.consignmentTypeId as varchar) = CAST(ct.id as varchar)\n" +
            "          left outer join ZNI_USER1 z\n" +
            "            on CAST(z.U_ID as varchar) = c.createdBy\n" +
            "          left outer join Riders r\n" +
            "          on r.riderCode = c.riderCode\n" +
            "          and r.branchId = c.branchCode \n" +
            "         WHERE CONVERT(date, c.accountReceivingDate, 105) >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND CONVERT(date, c.accountReceivingDate, 105) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "           and c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' " + clvar.AccountNo + " and r.status = '1'\n" +
            "\n" +
            "\n" +
            "        ) A\n" +
            " ORDER BY " + clvar.CheckCondition + "";




            sqlString = "\n" +
            "SELECT ROW_NUMBER() OVER(ORDER BY " + clvar.CheckCondition + ") SNO, A.*\n" +
            "  FROM (SELECT c.consignmentNumber,\n" +
            "               c.creditClientId,\n" +
            "               b.name Origin,\n" +
            "               c.serviceTypeName,\n" +
            "               DD.DSSPNumber consigner,\n" +
          //  "               DD.DSSPNumber, \n" +
            "               cc.name consignee,\n" +
            "               b2.sname Destination,\n" +
            "               c.weight,\n" +
            "               CAST(c.riderCode AS INT) riderCode,\n" +
            "               ec.name OriginExpressCenter,\n" +
            "               ct.name ConsignmentType,\n" +
            "               c.chargedAmount,\n" +
            "               CASE\n" +
            "                 WHEN CAST(c.isApproved AS VARCHAR) = '1' THEN\n" +
            "                  'YES'\n" +
            "                 ELSE\n" +
            "                  'NO'\n" +
            "               END Approved,\n" +
            "               c.consignerAccountNo,\n" +
            "               --ec2.name DestinationExpressCenter,\n" +
            "               c.accountReceivingDate,\n" +
            "               CONVERT(VARCHAR, c.bookingDate, 106) bookingdate,\n" +
            "               c.COD,\n" +
            "               z.name CreatedBy,\n" +
            "               c.CreatedOn,\n" +
            "               r.firstName + ' ' + r.lastName RiderName,\n" +
            "               c.totalAmount,\n" +
            "               (SUM(cm.calculatedValue) + SUM(cm.calculatedGST)) PAMT,\n" +
            "               c.gst,\n" +
            "               c.pieces\n" +
            "          FROM consignment c\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "         INNER JOIN Branches b\n" +
            "            ON b.branchCode = c.orgin\n" +
            "         INNER JOIN Branches b2\n" +
            "            ON b2.branchCode = c.destination\n" +
            "         INNER JOIN ExpressCenters ec\n" +
            "            ON ec.expressCenterCode = c.originExpressCenter\n" +
            //"         INNER JOIN ExpressCenters ec2\n" +
            //"            ON ec2.expressCenterCode = c.destinationExpressCenterCode\n" +
            "         INNER JOIN ConsignmentType ct\n" +
            "            ON CAST(c.consignmentTypeId AS VARCHAR) = CAST(ct.id AS VARCHAR)\n" +
            "          LEFT OUTER JOIN ZNI_USER1 z\n" +
            "            ON CAST(z.U_ID AS VARCHAR) = c.createdBy\n" +
            "          LEFT OUTER JOIN Riders r\n" +
            "            ON r.riderCode = c.riderCode\n" +
            "           AND r.branchId = c.branchCode\n" +
            "          LEFT OUTER JOIN ConsignmentModifier cm\n" +
            "            ON cm.consignmentNumber = c.consignmentNumber\n" +
            "          LEFT JOIN MNP_Detail_Retail_DSSP DD ON C.consignmentNumber = DD.ConsignmentNumber \n" +
            "         WHERE CONVERT(date, c.accountReceivingDate, 105) >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + "'\n" +
            "           AND CONVERT(date, c.accountReceivingDate, 105) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "           and c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' " + clvar.AccountNo + " and r.status = '1'\n" +
            "         GROUP BY c.consignmentNumber,\n" +
            "                  c.creditClientId,\n" +
            "                  b.name,\n" +
            "                  c.serviceTypeName,\n" +
            "                  c.consigner,\n" +
            "                  cc.name,\n" +
            "                  b2.sname,\n" +
            "                  c.weight,\n" +
            "                  c.riderCode,\n" +
            "                  ec.name,\n" +
            "                  ct.name,\n" +
            "                  c.chargedAmount,\n" +
            "                  c.isApproved,\n" +
            "                  c.consignerAccountNo,\n" +
            "                  --ec2.name,\n" +
            "                  c.accountReceivingDate,\n" +
            "                  c.bookingDate,\n" +
            "                  c.COD,\n" +
            "                  z.name,\n" +
            "                  c.CreatedOn,\n" +
            "                  r.firstName,\n" +
            "                  r.lastName,\n" +
            "                  c.totalAmount,\n" +
            "                  DD.DSSPNumber, \n" +
            "                  c.gst,\n" +
            "                  c.pieces) A\n" +
            " ORDER BY  " + clvar.CheckCondition + "";




            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.SelectCommand.CommandTimeout = 300000;

                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
    }
}