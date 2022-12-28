using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MRaabta.Files
{
    public partial class LoadingPrintNew : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        // Consignemnts con = new Consignemnts();
        LoadingPrintReport_NEW con = new LoadingPrintReport_NEW();
        int totalBags = 0;
        int totalOutPieces = 0;
        int totalPieces = 0;
        double totalWeight = 0;
        Boolean flag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //ph1.Controls.Add(new Literal { Text = HeaderTable() });
            PrintLoad();
        }

        protected void PrintLoad()
        {
            string userName = Session["U_NAME"].ToString();

            clvar.LoadingID = Request.QueryString["XCode"].ToString();
            DataTable header = GetLoadingHeader(clvar);
            DataTable dt = GetLoadingDetail(clvar);
            DataTable dtCon = GetLoadingConsignments(clvar);

            if (dt != null)
            {
                //totalShipmentCount = dt.Rows.Count;
                totalBags = dt.Rows.Count;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //object obj = dt.Compute("SUM(totalWeight)", "");
                        double tempWeight = 0;
                        double.TryParse(row["totalWeight"].ToString(), out tempWeight);
                        totalWeight += tempWeight;
                    }


                }
            }
            if (dtCon != null)
            {
                totalOutPieces = dtCon.Rows.Count;
                //totalShipmentCount += dtCon.Rows.Count;
                object obj = dtCon.Compute("SUM(Pieces)", "");
                int.TryParse(obj.ToString(), out totalPieces);
                totalPieces = totalPieces + totalBags;
                if (dtCon.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCon.Rows)
                    {
                        //object obj = dt.Compute("SUM(totalWeight)", "");
                        double tempWeight = 0;
                        double.TryParse(row["weight"].ToString(), out tempWeight);
                        totalWeight += tempWeight;
                    }


                }
            }
            //totalShipmentCount = dt.Rows.Count + dtCon.Rows.Count;

            if (header.Rows.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                int rowCount = 0;
                html.Append(HeaderTable(header));
                html.Append(HeaderRow());
                flag = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 35 == 0 && i > 0)
                    {
                        rowCount = 3;
                        html.Append("</table>");
                        html.Append("<table style=\"font-family:Calibri;\">");
                        html.Append("<tr><td>Document Created By:  </td><td><b>" + userName + "</b></td></tr>");
                        html.Append("<tr><td></br>Signature:</td><td style=\"width:100px;\">_____________________</td></tr>");
                        html.Append("</table>");
                        html.Append(HeaderTable(header));
                        html.Append(HeaderRow());
                    }

                    html.Append(DataRow(dt.Rows[i]));
                    rowCount++;
                }

                if (dt.Rows.Count > 0)
                {
                    double totalWeight = 0;
                    try
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            double tempweight = 0;
                            double.TryParse(dr["totalWeight"].ToString(), out tempweight);
                            totalWeight += tempweight;
                        }
                        string sqlString = "<tr>\n" +
                            "                <td style=\"border-width: 1px; border-color: #000000; width: 4%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid; text-align:Center;\">\n" +
                                            "                    <b></b>\n" +
                                            "                </td>\n" +
                                            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid; text-align:Center;\">\n" +
                                            "                    <b>Total</b>\n" +
                                            "                </td>\n" +
                                            "                <td style=\"border-width: 1px; border-color: #000000; width: 10%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid; text-align:center;\">\n" +
                                            "                    <b>" + totalWeight.ToString() + "</b>\n" +
                                            "                </td>\n" +
                                            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
                                            "                    \n" +
                                            "                </td>\n" +
                                            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
                                            "                    \n" +
                                            "                </td>\n" +
                                            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
                                            "                    \n" +
                                            "                </td>\n" +
                                            "                <td style=\"border-width: 1px; border-color: #000000; width: 26%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
                                            "                    \n" +
                                            "                </td>\n" +
                                                 "                <td style=\"border-width: 1px; border-color: #000000; width: 26%; border-left-style: none;\n" +
                                            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
                                            "                    \n" +
                                            "                </td>\n" +
                                            "            </tr>";
                        html.Append(sqlString);
                        rowCount++;
                    }
                    catch (Exception)
                    {


                    }




                }

                html.Append("</table><br />");

                if (dtCon.Rows.Count > 0)
                {
                    if (rowCount > 30)
                    {

                        html.Append("<table style=\"font-family:Calibri;\">");
                        html.Append("<tr><td>Document Created By:  </td><td><b>" + userName + "</b></td></tr>");
                        html.Append("<tr><td></br>Signature:</td><td style=\"width:100px;\">_____________________</td></tr>");
                        html.Append("</table>");
                        html.Append(HeaderTable(header));
                        rowCount = 2;
                    }
                    html.Append(OutPieceHeadingRow());

                    for (int i = 0; i < dtCon.Rows.Count; i++)
                    {
                        if (rowCount >= 28)
                        {
                            html.Append("</table>");
                            html.Append("<table style=\"font-family:Calibri;\">");
                            html.Append("<tr><td>Document Created By:  </td><td><b>" + userName + "</b></td></tr>");
                            html.Append("<tr><td></br>Signature:</td><td style=\"width:100px;\">_____________________</td></tr>");
                            html.Append("</table>");
                            html.Append(HeaderTable(header));
                            html.Append(OutPieceHeadingRow());
                            rowCount = 3;
                        }

                        html.Append(OutPieceDataRow(dtCon.Rows[i]));
                        //double rcnt = 19.0;
                        int lcnt = dtCon.Rows[i]["Consigner"].ToString().Length;
                        //int tnt = lnt / rcnt;
                        rowCount = rowCount + (lcnt / 19) + ((lcnt % 19 > 0 || lcnt == 0) ? 1 : 0);
                        //rowCount++;
                    }

                    if (dtCon.Rows.Count > 0)
                    {

                        double totalWeight = 0;
                        double totalPieces = 0;
                        try
                        {
                            foreach (DataRow dr in dtCon.Rows)
                            {
                                double tempweight = 0;
                                double tempPieces = 0;
                                double.TryParse(dr["weight"].ToString(), out tempweight);
                                double.TryParse(dr["pieces"].ToString(), out tempPieces);
                                totalWeight += tempweight;
                                totalPieces += tempPieces;
                            }
                            string sqlString = "<tr>\n" +
                                               "                <td style=\"width: 4% !important; text-align: center; font-size: small; border-left: 3px Solid Black;\n" +
                                               "                    border-right: 1px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                    <b></b>\n" +
                                               "                </td>\n" +
                                               "                <td style=\"width: 15% !important; text-align: center; font-size: small; border-left: 1px Solid Black;\n" +
                                               "                    border-right: 1px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                    <b>TOTAL</b>\n" +
                                               "                </td>\n" +
                                               "                <td style=\"width: 8% !important; text-align: center; font-size: small; border: 1px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                    <b>" + totalPieces.ToString() + "</b>\n" +
                                               "                </td>\n" +
                                               "                <td style=\"width: 8% !important; font-size: small; border: 1px Solid Black; text-align: center; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                   <b>" + totalWeight.ToString() + "</b> \n" +
                                               "                </td>\n" +
                                               //"                <td style=\"width: 20% !important; font-size: small; border: 1px Solid Black;\">\n" +
                                               //"                    \n" +
                                               //"                </td>\n" +
                                               "                <td style=\"width: 15% !important; font-size: small; border: 1px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                    \n" +
                                               "                </td>\n" +
                                               "                <td style=\"width: 15% !important; text-align: center; font-size: small; border-left: 1px Solid Black;\n" +
                                               "                    border-right: 1px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                   \n" +
                                               "                </td>\n" +
                                               "                <td style=\"width: 19% !important; text-align: center; font-size: small; border-left: 1px Solid Black;\n" +
                                               "                    border-right: 1px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                   \n" +
                                               "                </td>\n" +
                                               "                <td style=\"width: 20% !important; text-align: center; font-size: small; border-left: 1px Solid Black;\n" +
                                               "                    border-right: 3px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                   \n" +
                                               "                </td>\n" +
                                               "                <td style=\"width: 20% !important; text-align: center; font-size: small; border-left: 1px Solid Black;\n" +
                                               "                    border-right: 3px Solid Black; border-top: 2px Solid Black; border-bottom: 3px Solid Black;\">\n" +
                                               "                   \n" +
                                               "                </td>\n" +
                                               "            </tr>";
                            html.Append(sqlString);

                        }
                        catch (Exception)
                        {


                        }
                    }
                    html.Append("</table>");
                    html.Append("<table style=\"font-family:Calibri;\">");
                    html.Append("<tr><td>Document Created By:  </td><td><b>" + userName + "</b></td></tr>");
                    html.Append("<tr><td></br>Signature:</td><td style=\"width:100px;\">_____________________</td></tr>");
                    html.Append("</table>");
                }
                ph1.Controls.Add(new Literal { Text = html.ToString() });

                //System.IO.File.Delete(Server.MapPath("~") + "\\docs\\" + clvar.LoadingID + "___.png");
            }
            else
            {
                lbl_error.Text = "No Record Found...";
            }

        }

        protected string HeaderTable(DataTable dt)
        {
            RadBarcode barcode = new RadBarcode();
            barcode.Text = clvar.LoadingID;
            barcode.Type = BarcodeType.Code128B;
            //barcode.Width = 200;
            //barcode.Height = 40;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(barcode.GetImage());

            Bitmap image2 = new Bitmap(image);

            image2.Save(Server.MapPath("~") + "\\docs\\" + clvar.LoadingID + "___.png", ImageFormat.Png);
            string pageBreak = "";
            if (flag)
            {
                pageBreak = "page-break-before:always;";
            }

            string sqlString = "<table style=\"width: 100%; text-align: center; font-family: Calibri; " + pageBreak + " font-size: smaller;\">\n" +
            "           <tr>\n" +
            "               <td style=\"width: 20%\">\n" +
            "                   <img src=\"../images/OCS_Transparent.png\" height=\"60px\" alt=\"logo\" width=\"157px\" />\n" +
            "               </td>\n" +
            "               <td style=\"width: 60%; text-align: center;\">\n" +
            "                   <h3>\n" +
            "                       LOADING DETAIL</h3>\n" +
            "                   <img src=\"../docs/" + clvar.LoadingID + "___.png\" height=\"40px\" alt=\"logo\" width=\"157px\" />\n" +
            "                   <br />\n" +
            "" + clvar.LoadingID + "\n" +
            "               </td>\n" +
            "               <td style=\"width: 20%; vertical-align: top; text-align: right\">\n" +
            "                <b>Print Date:</b>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") +
            "               </td>\n" +
            "           </tr>\n" +
            "       </table> ";

            sqlString += "<table style=\"width: 100%; font-family: Calibri; font-size: small; border-bottom: 2px Solid Black; Border-top: 2px Solid Black;\">\n" +
           "            <tr>\n" +
           "                <td style=\"width: 17%; text-align: left;\">\n" +
           "                    <b>Transportation Type</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 22%; text-align: left;\">\n" +
           dt.Rows[0]["TransportType"].ToString() +
           "                </td>\n" +
           "                <td style=\"width: 9%; text-align: left;\">\n" +
           "                    <b>Date</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 19%; text-align: left;\">\n" +
           dt.Rows[0]["Date"].ToString().Substring(0, 10) +
           "                </td>\n" +
           "                <td style=\"width: 14%; text-align: left;\">  <b>At Airport</b\n" +
           "                </td>\n" +
           "                <td style=\"width: 19%; text-align: left;\">\n" +
           dt.Rows[0]["AtAirport"].ToString() +
           "                </td>\n" +
           "            </tr>\n" +
           "            <tr>\n" +
           "                <td style=\"width: 17%; text-align: left;\">\n" +
           "                    <b>Vehicle</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 22%; text-align: left;\">\n" +
           dt.Rows[0]["VehicleName"].ToString() +
           "                </td>\n" +
           "                <td style=\"width: 9%; text-align: left;\">\n" +
           "                    <b>Vehicle Type</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 19%; text-align: left;\">\n" +
           dt.Rows[0]["VehicleType"].ToString() +//dt.Rows[0]["Date"].ToString().Substring(0, 10) + vehicle Type ayegi yaha
            "                </td>\n" +
            "                <td style=\"width: 14%; text-align: left;\">\n" +
            "                    <b>Vehicle Seal Number</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 19%; text-align: left;\">\n" +
            dt.Rows[0]["sealno"].ToString() +    //dt.Rows[0]["VehicleName"].ToString() +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 17%; text-align: left;\">\n" +
            "                    <b>Courier Name</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; text-align: left;\">\n" +
            dt.Rows[0]["courierName"].ToString() +
            "                </td>\n" +
            "                <td style=\"width: 9%; text-align: left;\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 19%; text-align: left;\">\n" +
            dt.Rows[0]["OrgName"].ToString() +
            "                </td>\n" +
            "                <td style=\"width: 14%; text-align: left;\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 19%; text-align: left;\">\n" +
            dt.Rows[0]["DestName"].ToString() +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 17%; text-align: left;\">\n" +
            "                    <b>Description</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; text-align: left;\" >\n" +
            dt.Rows[0]["description"].ToString() +
            "                </td>\n" +
            "                <td style=\"width: 18%; text-align: left;\">\n" +
            "                    <b>Departure Flight Date</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; text-align: left;\" >\n" +
            dt.Rows[0]["departureflightdate"].ToString() +
            "                </td>\n" +
            "                <td style=\"width: 17%; text-align: left;\">\n" +
            "                    <b>Flight No.</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; text-align: left;\" >\n" +
            dt.Rows[0]["flightNo"].ToString() +
            "                </td>\n" +
            "            </tr>\n" +

            "            <tr>\n" +
            "                <td style=\"width: 17%; text-align: left;\">\n" +
            "<b>Total Weight</b>" +
            "                </td>\n" +
            "                <td style=\"width: 22%; text-align: left; float:left;\">\n" +
            "" + String.Format("{0:N2}", totalWeight) + "" +
            "                </td>\n" +
            "                <td  style=\"width: 18%; text-align: left;\"> <b>Route Name </b>\n </td>\n" +
            "                <td  style=\"width: 22%; text-align: left;\"> " + dt.Rows[0]["RouteName"].ToString() + "</td>" +
            "            </tr>\n" +
            "            <tr>" +
            "                <td style=\"width: 17%; text-align: left;\">\n" +
            "                    <b>Total Bags</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; text-align: left; float:left;\">\n" +
            totalBags.ToString() +
            "                </td>\n" +
            "                <td  style=\"width: 18%; text-align: left;\"><b>Total OutPieces</b></td>\n" +
            "                <td  style=\"width: 22%; text-align: left;\">" + totalOutPieces.ToString() + "</td>" +
            "                <td style=\"width: 17%; text-align: left;\">\n" +
            "                    <b>Total Pieces</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; text-align: left;\" >\n" +
            totalPieces.ToString() +
            "                </td>\n" +
            "</tr>" +

            "        </table><br />";

            return sqlString;
        }

        protected string HeaderRow()
        {

            string sqlString = "<table style=\"border-color: #000000; border-style: Solid; border-width: 3px; width: 100%;\n" +
            "            font-family: Calibri; border-collapse: collapse; font-size: smaller;\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 100%; text-align: center; font-size: medium; border-bottom: 2px Solid Black;\"\n" +
            "                    colspan=\"8\">\n" +
            "                    <b>Bags Detail</b>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 4%; border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>Sr#</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>Bag Number</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>Bag Weight</b>\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-right-style: solid; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
             "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-right-style: solid; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>Seal Number</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 26%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>Remarks</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 26%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                    <b>User name</b>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        protected string DataRow(DataRow dr)
        {

            string sqlString = "<tr>\n" +
                "                <td style=\"border-width: 1px; border-color: #000000; width: 4%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
            "                    " + dr["SRNO"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
            "                    " + dr["BagNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 10%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid; text-align:center;\">\n" +
            "                    " + dr["TotalWeight"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
            "                    " + dr["OrgName"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
            "                    " + dr["DestName"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 15%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
            "                    " + dr["SealNo"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 26%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
            "                    " + dr["Remarks"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border-width: 1px; border-color: #000000; width: 26%; border-left-style: none;\n" +
            "                    border-bottom-style: solid; border-right-style: solid;\">\n" +
            "                    " + dr["u_name"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        protected string OutPieceHeadingRow()
        {

            string sqlString = "<table style=\"width: 100%; font-family: Calibri; border-collapse:collapse; border-style: Solid Black; border-width: 3px; font-size: smaller;\">\n" +
            "            <tr>\n" +
            "                <td colspan=\"9\" style=\"width: 100%; text-align: center; font-size: medium; border: 3px Solid Black; border-bottom: 2px Solid Black;\">\n" +
            "                   <b> Consignments Detail</b>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 4% !important; text-align: center; font-size:9pt; border-bottom: 2px Solid Black; border-right:1px Solid Black !important; border-left:3px Solid Black !important;\">\n" +
            "                    <b>Sr#</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 15% !important; text-align: center; font-size:9pt; border-bottom: 2px Solid Black; border-right:1px Solid Black !important;\">\n" +
            "                    <b>CN Number</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8% !important; text-align: center; font-size:9pt; border-bottom: 2px Solid Black; border-right:1px Solid Black !important;\">\n" +
            "                    <b>Pieces</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8% !important; text-align: center; font-size:9pt; border-bottom: 2px Solid Black; border-right:1px Solid Black !important;\">\n" +
            "                    <b>Weight</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 15% !important; text-align: center; font-size:9pt; border-bottom: 2px Solid Black; border-right:1px Solid Black !important;\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
            //"                <td style=\"width: 20% !important; font-size:small; border: 2px Solid Black;\">\n" +
            //"                    <b>Consigner</b>\n" +
            //"                </td>\n" +
            //"                <td style=\"width: 20% !important; font-size:small; border: 2px Solid Black;\">\n" +
            //"                    <b>Consignee</b>\n" +
            //"                </td>\n" +
            "                <td style=\"width: 15% !important; font-size:9pt; border-bottom: 2px Solid Black; border-right:1px Solid Black !important;\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 15% !important; font-size:9pt; border-bottom: 2px Solid Black; border-right:1px Solid Black !important;\">\n" +
            "                    <b>Consigner</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 20% !important; font-size:9pt; border-bottom: 2px Solid Black; border-right:3px Solid Black !important;\">\n" +
            "                    <b>Remarks</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 20% !important; font-size:9pt; border-bottom: 2px Solid Black; border-right:3px Solid Black !important;\">\n" +
            "                    <b>User name</b>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        protected string OutPieceDataRow(DataRow dr)
        {

            string sqlString = "<tr>\n" +
            "                <td style=\"width: 4% !important; text-align: center; font-size: small; border-left: 3px Solid Black;\n" +
            "                    border-right: 1px Solid Black; border-top: 1px Solid Black; border-bottom: 1px Solid Black;\">\n" +
            "                    " + dr["SRNO"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15% !important; text-align: center; font-size: small; border-left: 1px Solid Black;\n" +
            "                    border-right: 1px Solid Black; border-top: 1px Solid Black; border-bottom: 1px Solid Black;\">\n" +
            "                    " + dr["consignmentNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 8% !important; text-align: center; font-size: small; border: 1px Solid Black;\">\n" +
            "                    " + dr["Pieces"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 8% !important; text-align: center; font-size: small; border-left: 1px Solid Black;\n" +
            "                    border-right: 1px Solid Black; border-top: 1px Solid Black; border-bottom: 1px Solid Black;\">\n" +
            "                    " + dr["Weight"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15% !important; text-align: center; font-size: small; border: 1px Solid Black;\">\n" +
            "                    " + dr["Origin"].ToString() + "\n" +
            "                </td>\n" +
            //"                <td style=\"width: 20% !important; font-size: x-small; border: 1px Solid Black;\">\n" +
            //"                    " + dr["Consigner"].ToString() + "\n" +
            //"                </td>\n" +
            //"                <td style=\"width: 20% !important; font-size: small; border: 1px Solid Black;\">\n" +
            //"                    " + dr["Consignee"].ToString() + "\n" +
            //"                </td>\n" +
            "                <td style=\"width: 15% !important; font-size: small; border: 1px Solid Black;\">\n" +
            "                    " + dr["Destination"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15% !important; font-size: small; border: 1px Solid Black;\">\n" +
            "                    " + dr["consigner"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 20% !important; text-align: left; font-size: small; border-left: 1px Solid Black;\n" +
            "                    border-right: 3px Solid Black; border-top: 1px Solid Black; border-bottom: 1px Solid Black;\">\n" +
            "                    " + dr["Remarks"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 20% !important; text-align: left; font-size: small; border-left: 1px Solid Black;\n" +
            "                    border-right: 3px Solid Black; border-top: 1px Solid Black; border-bottom: 1px Solid Black;\">\n" +
            "                    " + dr["u_name"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>";

            return sqlString;
        }

        public DataTable GetLoadingHeader(Cl_Variables clvar)
        {
            string sqlString = "select lu.AttributeDesc TransportType,\n" +
            "       l.date,\n" +
            "       Case when l.vehicleID != '103' then v.MakeModel + ' (' + v.Description + ')' else '(Rented)' + l.VehicleRegNo end VehicleName,\n" +
            "       l.courierName,\n" +
            "       b1.name OrgName,\n" +
            "       b2.name DestName,\n" +
            "       l.description,\n" +
            "       l.flightNo,\n" +
            "       l.sealno,\n" +
            "       l.departureflightdate \n" +
            "  from mnp_Loading l\n" +
            " left outer join rvdbo.Lookup lu\n" +
            "    on lu.id = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " left outer join rvdbo.Vehicle v\n" +
            "    on v.VehicleCode = l.vehicleId\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = l.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = l.destination\n" +
            " where l.id = '" + clvar.LoadingID + "'";

            string sql = "select lu.AttributeDesc TransportType, \n"
               + "       l.date, \n"
               + "       Case when l.vehicleID != '103' then v.MakeModel + ' (' + v.Description + ')' else '(Rented)' + l.VehicleRegNo end VehicleName, \n"
               + "       l.courierName, \n"
               + "       b1.name OrgName, \n"
               + "       b2.name DestName, \n"
               + "       l.description, \n"
               + "       l.flightNo, \n"
               + "       l.sealno, \n"
               + "       l.departureflightdate, \n"
               + "       vt.TypeDesc VehicleType,  Case when isairport ='1' then 'Yes' else 'No' end AtAirport,mm.Name RouteName    \n"
               + "  from mnp_Loading l \n"
               + "  inner join rvdbo.Movementroute mm \n"
               + "  on l.routeid = mm.movementrouteid \n"
         + " left outer join rvdbo.Lookup lu \n"
               + "    on lu.id = l.transportationType \n"
               + "   and lu.AttributeGroup = 'TRANSPORT_TYPE' \n"
               + " LEFT OUTER JOIN Vehicle_Type vt \n"
               + " ON vt.TypeID = l.VehicleType \n"
               + " LEFT OUTER JOIN rvdbo.Vehicle v \n"
               + "    on v.VehicleCode = l.vehicleId \n"
               + " inner join Branches b1 \n"
               + "    on b1.branchCode = l.origin \n"
               + " inner join Branches b2 \n"
               + "    on b2.branchCode = l.destination \n"
               + " where l.id = '" + clvar.LoadingID + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetLoadingDetail(Cl_Variables clvar)
        {
            string sqlString = "select ROW_NUMBER() over( order by lb.bagNumber) srNo,   lb.loadingId,\n" +
            "\t   lb.bagNumber, lb.BagWeight totalWeight,\n" +
            "\t   b1.name OrgName,\n" +
            "\t   b2.name DestName,\n" +
            "\t   lb.bagseal sealNo, lb.remarks, u_name\n" +
            "  from mnp_LoadingBag lb\n" +
            "  inner join zni_user1 zu on zu.u_id = lb.createdby \n" +
            //" inner join Bag b\n" +
            //"\ton b.bagNumber = lb.bagNumber\n" +
            " left outer join Branches b1\n" +
            "\ton b1.branchCode = lb.bagorigin\n" +
            " left outer join Branches b2\n" +
            "\ton b2.branchCode = lb.bagdestination\n" +
            " where lb.loadingId = '" + clvar.LoadingID + "'";
            if (Session["Destination"] != null)
            {
                sqlString = sqlString + " and lb.bagdestination='" + Session["Destination"].ToString() + "'";
            }
            sqlString = sqlString + " order by lb.bagNumber";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public DataTable GetLoadingConsignments(Cl_Variables clvar)
        {
            string sqlString = "select ROW_NUMBER() over( order by l.consignmentNumber) srNo,  l.consignmentNumber,\n" +
            "       l.cnpieces pieces,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       b.name Destination, b1.Name ORIGIN,\n" +
            "       l.cnweight weight, l.remarks, c.consigner\n" +
            "  from mnp_LoadingConsignment l\n" +
            "  inner join zni_user1 zu on zu.u_id = lb.createdby \n" +
            "inner join Consignment c\n" +
            "    on c.consignmentNumber = l.consignmentNumber\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = l.cndestination\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = c.orgin\n" +
            " where l.loadingId = '" + clvar.LoadingID + "'   order by l.consignmentNumber";

            sqlString = "SELECT ROW_NUMBER() OVER(ORDER BY l.consignmentNumber) srNo,\n" +
            "       l.consignmentNumber,\n" +
            "       l.cnpieces pieces,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       b.name Destination,\n" +
            "       b1.Name ORIGIN,\n" +
            "       l.cnweight WEIGHT,\n" +
            "       l.remarks,\n" +
            "       c.consigner, u_name\n" +
            "  FROM mnp_LoadingConsignment l\n" +
             "  inner join zni_user1 zu on zu.u_id = l.createdby \n" +
            " INNER JOIN MnP_Loading mpl\n" +
            "    ON mpl.id = l.loadingId\n" +
            "  LEFT OUTER JOIN Consignment c\n" +
            "    ON c.consignmentNumber = l.consignmentNumber\n" +
            " INNER JOIN Branches b\n" +
            "    ON b.branchCode = l.cndestination\n" +
            " INNER JOIN Branches b1\n" +
            "    ON b1.branchCode = CASE\n" +
            "         WHEN c.consignmentNumber IS NULL THEN\n" +
            "          mpl.origin\n" +
            "         ELSE\n" +
            "          c.orgin\n" +
            "       END\n" +
            " WHERE l.loadingId = '" + clvar.LoadingID + "'\n";
            if (Session["Destination"] != null)
            {
                sqlString = sqlString + " and lb.cndestination ='" + Session["Destination"].ToString() + "'";
            }
            sqlString = sqlString + " ORDER BY l.consignmentNumber";


            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

    }
}