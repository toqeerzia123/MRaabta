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
    public partial class DayCloseReport : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        //cl_DayClose dayclose = new cl_DayClose();

        string bdate;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                GenerateReport();
            }
        }

        protected void GenerateReport()
        {
            /*
            //Session["ExpressCenter"] = "0111";
            if (Request.QueryString.Keys.Count != 0)
            {
                clvar.Day = DateTime.Parse(Request.QueryString["Date"].ToString()).ToString("yyyy-MM-dd");
            }
            else
            {
                clvar.Day = Session["WorkingDate"].ToString();
                clvar.Day = "2016-02-23";
                //clvar.Day = DateTime.Now.ToString("yyyy-MM-dd");
            }
             */

            clvar.BookingDate = // Request.QueryString["date"].ToString(); //Session["WorkingDate"].ToString();

            //DataSet ds = Get_DayCloseDate(clvar);

            clvar.Day = Request.QueryString["date"].ToString();//DateTime.Parse(ds.Tables[0].Rows[0]["date"].ToString()).ToString("yyyy-MM-dd");
            if (Request.QueryString.Count > 1)
            {
                clvar.riderCode = "   AND c.riderCode = '" + Request.QueryString["RiderCode"] + "'\n";
            }

            DataTable dt = GetDayCloseReport(clvar);
            if (dt.Rows.Count > 0)
            {

                bdate = dt.Rows[0]["bookingdate"].ToString();

                double totalAmount = 0;
                double Tax = 0;
                double weight = 0;
                DataTable dt_ = new DataTable();
                DataTable dt__ = new DataTable();
                DataTable dt___ = new DataTable();
                try
                {
                    dt_ = dt.Select("ConType= 'DOMESTIC'", "").CopyToDataTable();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    dt__ = dt.Select("ConType= 'LOCAL'").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                try
                {
                    dt___ = dt.Select("ConType = 'VOID'").CopyToDataTable();
                }
                catch (Exception ex)
                { }

                string serviceType = dt.Rows[0]["ServiceTypeName"].ToString();
                int rowCount = 1;
                StringBuilder html = new StringBuilder();

                if (dt_.Rows.Count > 0)
                {
                    html.Append(HeaderTable(false, true, serviceType, "<b>Domestic Consignments</b>"));
                    for (int i = 0; i < dt_.Rows.Count; i++)
                    {
                        #region Page Break And Service Type
                        if (rowCount == 25)
                        {
                            if (dt_.Rows[i]["ServiceTypeName"].ToString() == serviceType)
                            {
                                rowCount++;
                                html.Append("</table>");
                                html.Append(HeaderTable(true, false, "", "<b>Domestic Consignments</b>"));
                            }
                            else
                            {
                                rowCount = rowCount + 2;
                                serviceType = dt_.Rows[i]["ServiceTypeName"].ToString();
                                html.Append("</table>");
                                html.Append(HeaderTable(true, true, serviceType, "<b>Domestic Consignments</b>"));
                            }
                            rowCount = 0;
                        }
                        if (dt_.Rows[i]["ServiceTypeName"].ToString() != serviceType)
                        {
                            rowCount = rowCount + 2;
                            serviceType = dt_.Rows[i]["ServiceTypeName"].ToString();
                            html.Append(HeaderRow(true, serviceType));
                        }
                        #endregion

                        #region Data Row
                        html.Append("<tr>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt_.Rows[i]["CNNO"].ToString() + "</td>");
                        if (dt_.Rows[i]["Consigner"].ToString().Length > 24)
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt_.Rows[i]["Consigner"].ToString().Substring(0, 24) + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt_.Rows[i]["Consigner"].ToString() + "</td>");
                        }
                        if (dt_.Rows[i]["Consignee"].ToString().Length > 24)
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt_.Rows[i]["Consignee"].ToString().Substring(0, 24) + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt_.Rows[i]["Consignee"].ToString() + "</td>");
                        }

                        html.Append("<td style=\"text-align:center !important;\">" + dt_.Rows[i]["Origin"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt_.Rows[i]["Destination"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt_.Rows[i]["AccNo"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt_.Rows[i]["Weight"].ToString() + "</td>");
                        if (dt_.Rows[i]["customerType"].ToString() == "1")
                        {
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N2}", dt_.Rows[i]["Tax"]).ToString() + "</td>");

                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N2}", dt_.Rows[i]["TotalAmount"]).ToString() + "</td>");
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N0}", Math.Round(double.Parse(dt_.Rows[i]["Tax"].ToString()) + double.Parse(dt_.Rows[i]["TotalAmount"].ToString()))).ToString() + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:right !important;\">0</td>");

                            html.Append("<td style=\"text-align:right !important;\">0</td>");
                            html.Append("<td style=\"text-align:right !important;\">0</td>");

                        }
                        html.Append("</tr>");
                        rowCount++;
                        #endregion
                    }
                    if (rowCount + 6 > 25)
                    {
                        html.Append("</table>");
                        html.Append(HeaderTable(true, false, "", "<b>Domestic Consignments</b>"));
                        //html.Append("</table>");
                    }

                    html.Append("<tr><td colspan='10' style=\"text-align:center !important; font-size:small !important;\">");
                    html.Append("<h3><u>DOMESTIC CONSIGNMENTS SUMMARY</u></h3>");
                    html.Append("</td></tr>");
                    html.Append("<tr>");
                    // double.TryParse(dt_.Compute("NVL(SUM(TOTALAMOUNT),0)", "customerType = '1'").ToString(), out totalAmount);
                    double.TryParse(dt_.Compute("SUM(Weight)", "").ToString(), out weight);
                    double.TryParse(dt_.Compute("SUM(TAX)", "customerType = '1'").ToString(), out Tax);
                    double.TryParse(dt_.Compute("SUM(TotalAmount)", "CustomerType = '1'").ToString(), out totalAmount);
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total no. of Consignments: </b>" + dt_.Rows.Count + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Consignment Price + Modifier: </b>" + String.Format("{0:N2}", totalAmount) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", weight) + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Tax: </b>" + String.Format("{0:N2}", Tax) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Cash Consignments :</b>" + dt_.Select("CustomerType = '1'").Count() + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Price + Modifier + Tax:</b>" + String.Format("{0:N2}", totalAmount + Tax) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Cash Consignments :</b>" + dt_.Select("CustomerType = '2'").Count() + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"></td>");
                    html.Append("</tr>");
                    rowCount = 1;
                    html.Append("</table>");
                    totalAmount = 0;
                    weight = 0;
                    Tax = 0;
                    //html.Append("<div style=\"width:100% !important;  font-family:Calibri !important; page-break-before:always !important; text-align:center !important;\"><h2>LOCAL CONSIGNMENTS</b></div>");



                }

                if (dt__.Rows.Count > 0)
                {

                    serviceType = dt__.Rows[0]["ServiceTypeName"].ToString();
                    html.Append(HeaderTable(false, true, serviceType, "<b>Local Consignments</b>"));
                    for (int i = 0; i < dt__.Rows.Count; i++)
                    {
                        #region Page Break And Service Type
                        if (rowCount == 25)
                        {
                            if (dt__.Rows[i]["ServiceTypeName"].ToString() == serviceType)
                            {
                                rowCount++;
                                html.Append("</table>");
                                html.Append(HeaderTable(true, false, "", "<b>Local Consignments</b>"));
                            }
                            else
                            {
                                rowCount = rowCount + 2;
                                serviceType = dt__.Rows[i]["ServiceTypeName"].ToString();
                                html.Append("</table>");
                                html.Append(HeaderTable(true, true, serviceType, "<b>Local Consignments</b>"));
                            }
                            rowCount = 0;
                        }
                        if (dt__.Rows[i]["ServiceTypeName"].ToString() != serviceType)
                        {
                            rowCount = rowCount + 2;
                            serviceType = dt__.Rows[i]["ServiceTypeName"].ToString();
                            html.Append(HeaderRow(true, serviceType));
                        }
                        #endregion

                        #region Data Row
                        html.Append("<tr>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt__.Rows[i]["CNNO"].ToString() + "</td>");
                        if (dt__.Rows[i]["Consigner"].ToString().Length > 24)
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt__.Rows[i]["Consigner"].ToString().Substring(0, 24) + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt__.Rows[i]["Consigner"].ToString() + "</td>");
                        }
                        if (dt__.Rows[i]["Consignee"].ToString().Length > 24)
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt__.Rows[i]["Consignee"].ToString().Substring(0, 24) + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt__.Rows[i]["Consignee"].ToString() + "</td>");
                        }
                        html.Append("<td style=\"text-align:center !important;\">" + dt__.Rows[i]["Origin"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt__.Rows[i]["Destination"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt__.Rows[i]["AccNo"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt__.Rows[i]["Weight"].ToString() + "</td>");
                        if (dt__.Rows[i]["CustomerType"].ToString() == "1")
                        {
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N2}", dt__.Rows[i]["Tax"]).ToString() + "</td>");
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N2}", dt__.Rows[i]["TotalAmount"]).ToString() + "</td>");
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N0}", Math.Round(double.Parse(dt__.Rows[i]["Tax"].ToString()) + double.Parse(dt__.Rows[i]["TotalAmount"].ToString()))).ToString() + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:right !important;\">0</td>");
                            html.Append("<td style=\"text-align:right !important;\">0</td>");
                            html.Append("<td style=\"text-align:right !important;\">0</td>");

                        }
                        html.Append("</tr>");
                        rowCount++;
                        #endregion
                    }


                    if (rowCount + 6 > 25)
                    {
                        html.Append("</table>");
                        html.Append(HeaderTable(true, false, "", "<b>Local Consignments</b>"));
                        //html.Append("</table>");

                    }
                    // double.TryParse(dt__.Compute("NVL(SUM(TOTALAMOUNT),0)", "customerType = '1'").ToString(), out totalAmount);
                    double.TryParse(dt__.Compute("SUM(Weight)", "").ToString(), out weight);
                    double.TryParse(dt__.Compute("SUM(TAX)", "customerType = '1'").ToString(), out Tax);

                    html.Append("<tr><td colspan='10' style=\"text-align:center !important; font-size:small !important;\">");
                    html.Append("<h3><u>LOCAL CONSIGNMENTS SUMMARY</u></h3>");
                    html.Append("</td></tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total no. of Consignments: </b>" + dt__.Rows.Count + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Consignment Price + Modifier: </b>" + String.Format("{0:N2}", totalAmount) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", weight) + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Tax: </b>" + String.Format("{0:N2}", Tax) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Cash Consignments :</b>" + dt__.Select("CustomerType = '1'").Count() + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Price + Modifier + Tax:</b>" + String.Format("{0:N2}", totalAmount + Tax) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Credit Consignments :</b>" + dt__.Select("CustomerType = '2'").Count() + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"></td>");
                    html.Append("</tr>");
                    totalAmount = 0;
                    weight = 0;
                    Tax = 0;
                    rowCount = 1;
                }


                if (dt___.Rows.Count > 0)
                {
                    html.Append(HeaderTable(false, false, serviceType, "<b>Void Consignments</b>"));
                    for (int i = 0; i < dt___.Rows.Count; i++)
                    {
                        #region Page Break And Service Type
                        if (rowCount == 25)
                        {
                            if (dt___.Rows[i]["ServiceTypeName"].ToString() == serviceType)
                            {
                                rowCount++;
                                html.Append("</table>");
                                html.Append(HeaderTable(true, false, "", "<b>Local Consignments</b>"));
                            }
                            else
                            {
                                rowCount = rowCount + 2;
                                serviceType = dt___.Rows[i]["ServiceTypeName"].ToString();
                                html.Append("</table>");
                                html.Append(HeaderTable(true, true, serviceType, "<b>Local Consignments</b>"));
                            }
                            rowCount = 0;
                        }
                        if (dt___.Rows[i]["ServiceTypeName"].ToString() != serviceType)
                        {
                            rowCount = rowCount + 2;
                            serviceType = dt___.Rows[i]["ServiceTypeName"].ToString();
                            html.Append(HeaderRow(true, serviceType));
                        }
                        #endregion

                        #region Data Row
                        html.Append("<tr>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt___.Rows[i]["CNNO"].ToString() + "</td>");
                        if (dt___.Rows[i]["Consigner"].ToString().Length > 24)
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt___.Rows[i]["Consigner"].ToString().Substring(0, 24) + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt___.Rows[i]["Consigner"].ToString() + "</td>");
                        }
                        if (dt___.Rows[i]["Consignee"].ToString().Length > 24)
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt___.Rows[i]["Consignee"].ToString().Substring(0, 24) + "</td>");
                        }
                        else
                        {
                            html.Append("<td style=\"text-align:left !important;\">" + dt___.Rows[i]["Consignee"].ToString() + "</td>");
                        }
                        html.Append("<td style=\"text-align:center !important;\">" + dt___.Rows[i]["Origin"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt___.Rows[i]["Destination"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt___.Rows[i]["AccNo"].ToString() + "</td>");
                        html.Append("<td style=\"text-align:center !important;\">" + dt___.Rows[i]["Weight"].ToString() + "</td>");
                        if (dt___.Rows[i]["customerType"].ToString() == "1")
                        {
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N2}", dt___.Rows[i]["Tax"]).ToString() + "</td>");
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N2}", dt___.Rows[i]["TotalAmount"]).ToString() + "</td>");
                            html.Append("<td style=\"text-align:right !important;\">" + String.Format("{0:N0}", Math.Round(double.Parse(dt___.Rows[i]["Tax"].ToString()) + double.Parse(dt___.Rows[i]["TotalAmount"].ToString()))).ToString() + "</td>");

                        }
                        else
                        {
                            html.Append("<td style=\"text-align:right !important;\">0</td>");
                            html.Append("<td style=\"text-align:right !important;\">0</td>");
                            html.Append("<td style=\"text-align:right !important;\">0</td>");

                        }
                        html.Append("</tr>");
                        rowCount++;
                        #endregion
                    }
                    if (rowCount + 6 > 25)
                    {
                        html.Append("</table>");
                        html.Append(HeaderTable(true, false, "", ""));
                        //html.Append("</table>");
                        rowCount = 1;
                    }
                    html.Append("<tr><td colspan='10' style=\"text-align:center !important; font-size:small !important;\">");
                    html.Append("<h3><u>VOID CONSIGNMENTS SUMMARY</u></h3>");
                    html.Append("</td></tr>");
                    html.Append("<tr>");
                    // double.TryParse(dt___.Compute("NVL(SUM(TOTALAMOUNT),0)", "customerType = '1'").ToString(), out totalAmount);
                    double.TryParse(dt___.Compute("SUM(Weight)", "").ToString(), out weight);
                    double.TryParse(dt___.Compute("SUM(TAX)", "customerType = '1'").ToString(), out Tax);
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total no. of Consignments: </b>" + dt___.Rows.Count + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Consignment Price  + Modifier: </b>" + String.Format("{0:N2}", totalAmount) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", weight) + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Tax: </b>" + String.Format("{0:N2}", Tax) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Cash Consignments: </b>" + dt___.Select("customerType = '1'").Count() + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Price + Modifier + Tax:</b>" + String.Format("{0:N2}", totalAmount + Tax) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Credit Consignments: </b>" + dt___.Select("customerType = '2'").Count() + "</td>");
                    html.Append("<td colspan='5' style=\"text-align:right !important;\"></td>");
                    html.Append("</tr>");
                    totalAmount = 0;
                    weight = 0;
                    Tax = 0;
                }
                if (true)
                {
                    html.Append("</table>");
                    html.Append(HeaderTable());
                    //html.Append("</table>");
                    rowCount = 1;
                }
                int TotalNoOfCNs = 0;
                double totalCNPrice = 0;
                double totalWeight = 0;
                double totalTax = 0;
                double totalPricePlusTax = 0;


                TotalNoOfCNs = dt.Rows.Count;

                object obj = dt.Compute("SUM(TOTALAMOUNT)", "");
                double.TryParse(obj.ToString(), out totalCNPrice);

                obj = dt.Compute("SUM(Weight)", "");
                double.TryParse(obj.ToString(), out totalWeight);

                obj = dt.Compute("SUM(TAX)", "");
                double.TryParse(obj.ToString(), out totalTax);

                totalPricePlusTax = totalCNPrice + totalTax;
                html.Append("<tr><td colspan='10' style=\"text-align:center !important; font-size:small !important;\">");
                html.Append("<h2><u>ALL CONSIGNMENTS SUMMARY</u></h2>");
                html.Append("</td></tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total no. of Consignments: </b>" + dt.Rows.Count + "</td>");
                html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Consignment Price + Modifier: </b>" + String.Format("{0:N2}", totalCNPrice) + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", totalWeight) + "</td>");
                html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Tax: </b>" + String.Format("{0:N2}", totalTax) + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Cash Consignments: </b>" + dt.Select("customerType = '1'").Count() + "</td>");
                html.Append("<td colspan='5' style=\"text-align:right !important;\"><b>Total Price + Modifier + Tax:</b>" + String.Format("{0:N2}", totalPricePlusTax) + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;\"><b>Credit Consignments: </b>" + dt.Select("customerType = '2'").Count() + "</td>");
                html.Append("<td colspan='5' style=\"text-align:right !important;\"></td>");
                html.Append("</tr>");
                html.Append("</table>");
                placeholder1.Controls.Add(new Literal { Text = html.ToString() });

            }
            else
            {
                placeholder1.Controls.Add(new Literal { Text = "No Data Available" });
            }

        }

        protected string HeaderTable(Boolean pBreak, Boolean serviceType, string st, string type)
        {
            string pageBreak = "";
            if (pBreak)
            {
                pageBreak = "page-break-before:always !important;";
            }
            else
            {
                pageBreak = "";
            }
            string header = "";
            clvar.expresscenter = Session["ExpressCenter"].ToString();
            header = "<table width=\"100%\" style=\"font-family:Calibri !important; " + pageBreak + "\">\n" +
            "        <tr>\n" +
            "            <td style=\"width: 25% !important;\">\n" +
            "                <b>Booking Date: </b>" + DateTime.Parse(bdate.ToString()).ToString("dd-MM-yyyy") + "\n" +
            "            </td>\n" +
            "            <td style=\"width: 50% !important; text-align: center !important;\">\n" +
            "                <b>DAILY CONSIGNMENTS REPORT</b><br />\n" +
            "                <b>" + con.GetExpressCenterName(clvar).Rows[0][0].ToString() + "</b>\n" +
            "            </td>\n" +
            "            <td style=\"width: 25% !important; text-align: right !important;\">\n" +
            "                <b>Created On: </b>" + DateTime.Now.ToString() + "\n" +
            "            </td>\n" +
            "        </tr>\n" +
            "        <tr>\n" +
            "            <td style=\"width: 25% !important\">\n" +
            "            </td>\n" +
            "            <td style=\"width: 50% !important; text-align: center !important;\">\n" +
            "                " + type + "\n" +
            "            </td>\n" +
            "            <td style=\"width: 25% !important; text-align: right !important;\">\n" +
            "            </td>\n" +
            "        </tr>\n";

            header += "    </table>";
            header += HeaderRow(serviceType, st);
            return header;
        }
        protected string HeaderTable()
        {
            string pageBreak = "";

            string header = "";
            clvar.expresscenter = Session["ExpressCenter"].ToString();
            header = "<table width=\"100%\" style=\"font-family:Calibri !important; page-break-before:always !important;\">\n" +
            "        <tr>\n" +
            "            <td style=\"width: 25% !important;\">\n" +
            "                <b>Booking Date: </b>" + DateTime.Parse(bdate.ToString()).ToString("dd-MM-yyyy") + "\n" +
            "            </td>\n" +
            "            <td style=\"width: 50% !important; text-align: center !important;\">\n" +
            "                <b>DAILY CONSIGNMENTS REPORT</b><br />\n" +
            "                <b>" + con.GetExpressCenterName(clvar).Rows[0][0].ToString() + "</b>\n" +
            "            </td>\n" +
            "            <td style=\"width: 25% !important; text-align: right !important;\">\n" +
            "                <b>Created On: </b>" + DateTime.Now.ToString() + "\n" +
            "            </td>\n" +
            "        </tr>\n" +
            "        <tr>\n" +
            "            <td style=\"width: 25% !important\">\n" +
            "            </td>\n" +
            "            <td style=\"width: 50% !important; text-align: center !important;\">\n" +
            "                \n" +
            "            </td>\n" +
            "            <td style=\"width: 25% !important; text-align: right !important;\">\n" +
            "            </td>\n" +
            "        </tr>\n";

            header += "    </table>";
            header += "<table width=\"100%\" style=\"font-family:Calibri !important; font-size:smaller !important;\">";

            return header;
        }
        protected string ServiceType(string st)
        {
            string serviceType = "";
            serviceType += "<tr><td colspan='10'><b>Service Type:</b>" + st + "</td></tr>";
            return serviceType;
        }
        protected string HeaderRow(Boolean serviceType, string st)
        {
            string headerRow = "<table width=\"100%\" style=\"font-family:Calibri !important; font-size:smaller !important;\">";

            if (serviceType)
            {
                headerRow += ServiceType(st);
            }
            headerRow += "<tr>";

            headerRow += "<td style=\"width:10% !important;text-align:center !important;\">";
            headerRow += "<b>CN No.</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:20% !important;text-align:left !important;\">";
            headerRow += "<b>Consigner</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:20% !important;text-align:left !important;\">";
            headerRow += "<b>Consignee</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:8% !important;text-align:center !important;\">";
            headerRow += "<b>Origin</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:8% !important;text-align:center !important;\">";
            headerRow += "<b>Dest</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:6.8% !important;text-align:center !important;\">";
            headerRow += "<b>Acc No.</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:6.8% !important;text-align:center !important;\">";
            headerRow += "<b>Weight</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:6.8% !important;text-align:right !important;\">";
            headerRow += "<b>Tax</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:6.8% !important;text-align:right !important;\">";
            headerRow += "<b>Price + Modifier</b>";
            headerRow += "</td>";

            headerRow += "<td style=\"width:6.8% !important;text-align:right !important;\">";
            headerRow += "<b>Total</b>";
            headerRow += "</td>";

            headerRow += "</tr>";
            return headerRow;
        }


        public DataTable GetDayCloseReport(Cl_Variables clvar)
        {
            string sqlString = "select c.consignmentNumber [CN No], convert(varchar(11), c.bookingDate, 106) bookingDate,\n" +
           "\t   convert(varchar(11), c.createdOn, 106) createdOn, \n" +
            "\t   c.consigner Consigner,\n" +
            "\t   c.consignee,\n" +
            "\t   c.serviceTypeName,\n" +
            "\t   b.name Origin,\n" +
            "\t   c.destination Destination,\n" +
            "\t   c.consignerAccountNo AccNo,\n" +
            "\t   c.weight,\n" +
            "\t   c.gst TAX,\n" +
            "\t   c.chargedAmount,\n" +
            "     case\n" +
            "\t\t\twhen c.isInsured = '1'\n" +
            "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
            "\t\t\telse c.totalAmount\n" +
            "\t\tend TotalAmount," +
            "\t   case\n" +
            "\t\t\twhen c.orgin = c.destination\n" +
            "\t\t\tthen 'Local'\n" +
            "\t\t\telse 'Domestic'\n" +
            "\t   end ConType\n" +
            "\n" +
            "  FROM Consignment c\n" +
            "  inner join Branches b\n" +
            "  on b.branchCode = c.orgin\n" +
           " where CONVERT(date, c.createdOn, 105) = '" + clvar.Day + "'\n" +
           "   and c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
            " order by contype, serviceTypeName";



            sqlString = "select c.consignmentNumber [CN No], c.customerType,convert(varchar(11), c.bookingDate, 106) bookingDate,\n" +
           "\t   convert(varchar(11), c.createdOn, 106) createdOn, \n" +
           "\t   c.consigner Consigner,\n" +
           "\t   c.consignee,\n" +
           "\t   c.serviceTypeName,\n" +
           "\t   b.name Origin,\n" +
           "\t   (               \n" +
           "\t    SELECT b2.name    \n" +
           "\t    FROM   Branches b2    \n" +
           "\t   WHERE  b2.branchCode = c.destination    \n" +
           "\t )   DestinationBranch,   \n" +
           "\t  (        \n" +
           "\t      SELECT ec.name   \n" +
           "\t      FROM   ExpressCenters ec   \n" +
           "\t     WHERE  ec.expressCenterCode = c.destinationExpressCenterCode    \n" +
           "\t  )    Destination,     \n" +
           "\t   c.consignerAccountNo AccNo,\n" +
           "\t   c.weight,\n" +
           "\t   c.gst TAX,\n" +
           "\t   c.chargedAmount,\n" +
           "     case\n" +
           "\t\t\twhen c.isInsured = '1'\n" +
           "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
           "\t\t\telse c.totalAmount\n" +
           "\t\tend TotalAmount," +
           "\t   case when (select COUNT(vc.ConsignmentNo) from MNP_VOID_Consignment vc where vc.ConsignmentNo = c.consignmentNumber) > 0 then 'VOID'\n" +
           "\t\t\twhen c.orgin = c.destination\n" +
           "\t\t\tthen 'Local'\n" +
           "\t\t\telse 'Domestic'\n" +
           "\t   end ConType\n" +
           "\n" +
           "  FROM Consignment c\n" +
           "  inner join Branches b\n" +
           "  on b.branchCode = c.orgin\n" +
           " left outer join InternationalConsignmentDetail cd on cd.consignmentNo = c.consignmentNumber \n " +
           " where CONVERT(date, c.createdOn, 105) = '" + clvar.Day + "'\n" +
           "   and c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
           " order by contype, serviceTypeName";

            sqlString = "select c.consignmentNumber CNNo,\n" +
           "       c.customerType,\n" +
           "       convert(varchar(11), c.bookingDate, 106) bookingDate,\n" +
           "       convert(varchar(11), c.createdOn, 106) createdOn,\n" +
           "       c.consigner Consigner,\n" +
           "       c.consignee,\n" +
           "       UPPER(c.serviceTypeName) serviceTypeName,\n" +
           "       b.name Origin,\n" +
           "       (SELECT b2.name FROM Branches b2 WHERE b2.branchCode = c.destination) DestinationBranch,\n" +
           "       (SELECT ec.name\n" +
           "          FROM ExpressCenters ec\n" +
           "         WHERE ec.expressCenterCode = c.destinationExpressCenterCode) Destination,\n" +
           "       c.consignerAccountNo AccNo,\n" +
           "       c.weight,\n" +
           "       c.gst TAX,\n" +
           "       c.chargedAmount,\n" +
           "       case\n" +
           "         when c.isInsured = '1' then\n" +
           "          (c.decalaredValue * (c.insuarancePercentage / 100)) +\n" +
           "          c.totalAmount\n" +
           "         else\n" +
           "          c.totalAmount\n" +
           "       end TotalAmount,\n" +
           "       case\n" +
           "         when (select COUNT(vc.ConsignmentNo)\n" +
           "                 from MNP_VOID_Consignment vc\n" +
           "                where vc.ConsignmentNo = c.consignmentNumber) > 0 then\n" +
           "          'VOID'\n" +
           "         when c.orgin = c.destination then\n" +
           "          'Local'\n" +
           "         else\n" +
           "          'Domestic'\n" +
           "       end ConType\n" +
           "\n" +
           "  FROM Consignment c\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = c.orgin\n" +
           "  left outer join InternationalConsignmentDetail cd\n" +
           "    on cd.consignmentNo = c.consignmentNumber\n" +
           " inner join ZNI_USER1 z\n" +
           "    on CAST(z.U_ID as varchar) = c.createdBy\n" +
           "   --and z.Designation = 'ECI'\n" +
           " where CONVERT(date, c.bookingDate, 105) = '" + clvar.Day + "'\n" +
           "   and c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
           "   and c.riderCode = '" + clvar.riderCode + "'\n" +
           " order by contype, serviceTypeName";


            sqlString =
            "SELECT c.consignmentNumber CNNo,\n" +
            "       c.customerType,\n" +
            "       CONVERT(VARCHAR(11), c.bookingDate, 106) bookingDate,\n" +
            "       CONVERT(VARCHAR(11), c.createdOn, 106) createdOn,\n" +
            "       c.consigner Consigner,\n" +
            "       c.consignee,\n" +
            "       UPPER(c.serviceTypeName) serviceTypeName,\n" +
            "       b.name Origin,\n" +
            "       (SELECT b2.name FROM Branches b2 WHERE b2.branchCode = c.destination) DestinationBranch,\n" +
            "       (SELECT ec.name\n" +
            "          FROM ExpressCenters ec\n" +
            "         WHERE ec.expressCenterCode = c.destinationExpressCenterCode) Destination,\n" +
            "       c.consignerAccountNo AccNo,\n" +
            "       c.weight,\n" +
            "       SUM(ISNULL(cm.calculatedGST, 0)) + c.gst TAX,\n" +
            "       c.chargedAmount,\n" +
            "       SUM(ISNULL(cm.calculatedValue, 0)) + c.totalAmount TotalAmount,\n" +
            "       --case\n" +
            "       --  when c.isInsured = '1' then\n" +
            "       --   (c.decalaredValue * (c.insuarancePercentage / 100)) +\n" +
            "       --   c.totalAmount\n" +
            "       --  else\n" +
            "       --   c.totalAmount\n" +
            "       --end TotalAmount,\n" +
            "       CASE\n" +
            "         WHEN (SELECT COUNT(vc.ConsignmentNo)\n" +
            "                 FROM MNP_VOID_Consignment vc\n" +
            "                WHERE vc.ConsignmentNo = c.consignmentNumber) > 0 THEN\n" +
            "          'VOID'\n" +
            "         WHEN c.orgin = c.destination THEN\n" +
            "          'Local'\n" +
            "         ELSE\n" +
            "          'Domestic'\n" +
            "       END ConType\n" +
            "  FROM Consignment c\n" +
            " INNER JOIN Branches b\n" +
            "    ON b.branchCode = c.orgin\n" +
            "  LEFT OUTER JOIN InternationalConsignmentDetail cd\n" +
            "    ON cd.consignmentNo = c.consignmentNumber\n" +
            " INNER JOIN ZNI_USER1 z\n" +
            "    ON CAST(z.U_ID AS VARCHAR) = c.createdBy\n" +
            "--and z.Designation = 'ECI'\n" +
            "\n" +
            "  LEFT OUTER JOIN ConsignmentModifier cm\n" +
            "    ON cm.consignmentNumber = c.consignmentNumber\n" +
            " WHERE CAST(c.bookingDate AS DATE) = '" + clvar.Day + "'\n" +
            "   AND c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
            clvar.riderCode +
            " GROUP BY c.consignmentNumber,\n" +
            "          c.customerType,\n" +
            "          c.bookingDate,\n" +
            "          c.createdOn,\n" +
            "          c.consigner,\n" +
            "          c.consignee,\n" +
            "          c.serviceTypeName,\n" +
            "          b.name,\n" +
            "          c.destination,\n" +
            "          c.destinationExpressCenterCode,\n" +
            "          c.consignerAccountNo,\n" +
            "          c.[weight],\n" +
            "          c.gst,\n" +
            "          c.chargedAmount,\n" +
            "          c.totalAmount,\n" +
            "          c.orgin\n" +
            " ORDER BY contype, serviceTypeName";



            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 300;
                orcd.CommandType = CommandType.Text;
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