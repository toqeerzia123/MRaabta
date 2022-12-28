using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class DayEnd : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {
            Errorid.Text = "";
            if (!IsPostBack)
            {
                txt_date.Enabled = false;
                txt_date.Text = Session["WorkingDate"].ToString();
                //GenerateReport();

            }
        }

        protected void GenerateReport()
        {
            try
            {
                clvar.Day = Session["WorkingDate"].ToString();
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login");
            }
            //Session["ExpressCenter"] = "0111";
            //clvar.Day = "2016-02-23"; // DateTime.Parse(Session["WorkingDate"].ToString()).ToString("yyyy-MM-dd");
            DataTable dt = con.GetDayCloseReport(clvar);
            ViewState["dt"] = dt;
            if (dt.Rows.Count > 0)
            {
                DataTable dt_ = dt.Clone();
                DataTable dt__ = dt.Clone();
                DataTable dt___ = dt.Clone();
                try
                {
                    dt_ = dt.Select("ConType= 'DOMESTIC' AND customerType = '1'", "").CopyToDataTable();
                }
                catch (Exception ex)
                { }

                try
                {
                    dt__ = dt.Select("ConType= 'LOCAL' AND customerType = '1'").CopyToDataTable();
                }
                catch (Exception exz)
                { }
                try
                {
                    dt___ = dt.Select("ConType= 'VOID' AND customerType = '1'", "").CopyToDataTable();
                }
                catch (Exception ex)
                { }

                string serviceType = dt.Rows[0]["ServiceTypeName"].ToString();
                int rowCount = 1;
                StringBuilder html = new StringBuilder();
                if (dt_ != null)
                {
                    if (dt_.Rows.Count > 0)
                    {
                        //html.Append(HeaderTable(false, true, serviceType, "<b>Domestic Consignments</b>"));
                        for (int i = 0; i < dt_.Rows.Count; i++)
                        {
                            #region Page Break And Service Type
                            //if (rowCount == 25)
                            //{
                            //    if (dt_.Rows[i]["ServiceTypeName"].ToString() == serviceType)
                            //    {
                            //        rowCount++;
                            //        html.Append("</table>");
                            //        html.Append(HeaderTable(true, false, "", "<b>Domestic Consignments</b>"));
                            //    }
                            //    else
                            //    {
                            //        rowCount = rowCount + 2;
                            //        serviceType = dt_.Rows[i]["ServiceTypeName"].ToString();
                            //        html.Append("</table>");
                            //        html.Append(HeaderTable(true, true, serviceType, "<b>Domestic Consignments</b>"));
                            //    }
                            //    rowCount = 0;
                            //}
                            //if (dt_.Rows[i]["ServiceTypeName"].ToString() != serviceType)
                            //{
                            //    rowCount = rowCount + 2;
                            //    serviceType = dt_.Rows[i]["ServiceTypeName"].ToString();
                            //    html.Append(HeaderRow(true, serviceType));
                            //}
                            #endregion

                            #region Data Row
                            //html.Append("<tr>");
                            //html.Append("<td ; style=\"text-align:center !important;float: left;width:10%\">" + dt_.Rows[i]["CN NO"].ToString() + "</td>");
                            //if (dt_.Rows[i]["Consigner"].ToString().Length > 24)
                            //{
                            //    html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt_.Rows[i]["Consigner"].ToString().Substring(0, 24) + "</td>");
                            //}
                            //else
                            //{
                            //    html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt_.Rows[i]["Consigner"].ToString() + "</td>");
                            //}
                            //if (dt_.Rows[i]["Consignee"].ToString().Length > 24)
                            //{
                            //    html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt_.Rows[i]["Consignee"].ToString().Substring(0, 24) + "</td>");
                            //}
                            //else
                            //{
                            //    html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt_.Rows[i]["Consignee"].ToString() + "</td>");
                            //}

                            //html.Append("<td  style=\"text-align:left !important;float: left;width:8%\">" + dt_.Rows[i]["Origin"].ToString() + "</td>");
                            //html.Append("<td  style=\"text-align:left !important;float: left;width:8%\">" + dt_.Rows[i]["DestinationBranch"].ToString() + "</td>");
                            //html.Append("<td  style=\"text-align:left !important;float: left;width:8%\">" + dt_.Rows[i]["DestinationExpressCenter"].ToString() + "</td>");
                            //html.Append("<td  style=\"text-align:left !important;float: left;width:6.4%\">" + dt_.Rows[i]["AccNo"].ToString() + "</td>");
                            //html.Append("<td  style=\"text-align:left !important;float: left;width:6.4%\">" + String.Format("{0:N2}", dt_.Rows[i]["Weight"]).ToString() + "</td>");
                            //html.Append("<td  style=\"text-align:right !important;float: left;width:6.4%\">" + String.Format("{0:N2}", dt_.Rows[i]["Tax"]).ToString() + "</td>");
                            //html.Append("<td  style=\"text-align:right !important;float: left;width:6.4%\">" + String.Format("{0:N2}", dt_.Rows[i]["TotalAmount"]).ToString() + "</td>");
                            //html.Append("<td  style=\"text-align:right !important;float: left;width:6.4%\">" + String.Format("{0:N0}", Math.Round(double.Parse(dt_.Rows[i]["Tax"].ToString()) + double.Parse(dt_.Rows[i]["TotalAmount"].ToString()))).ToString() + "</td>");
                            //html.Append("</tr>");
                            //rowCount++;
                            #endregion
                        }
                        //if (rowCount + 6 > 25)
                        //{
                        //    html.Append("</table>");
                        //    html.Append(HeaderTable(true, false, "", "<b>Domestic Consignments</b>"));
                        //    //html.Append("</table>");
                        //}
                        html.Append("<table width=\"100%\">");
                        html.Append("<tr><td colspan='11' style=\"text-align:center !important; font-size:small !important;float: left; width:100%\">");
                        html.Append("<h3><u>DOMESTIC CONSIGNMENTS SUMMARY</u></h3>");
                        html.Append("</td></tr>");
                        html.Append("<tr>");
                        html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total no. of Consignments: </b>" + dt_.Rows.Count + "</td>");
                        html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Consignment Price: </b>" + String.Format("{0:N2}", double.Parse(dt_.Compute("SUM(TOTALAMOUNT)", "").ToString())) + "</td>");
                        html.Append("</tr>");
                        html.Append("<tr>");
                        html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", double.Parse(dt_.Compute("SUM(Weight)", "").ToString())) + "</td>");
                        html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Tax: </b>" + String.Format("{0:N2}", double.Parse(dt_.Compute("SUM(TAX)", "").ToString())) + "</td>");
                        html.Append("</tr>");
                        html.Append("<tr>");
                        html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Cash Consignments:</b>" + dt.Select("ConType= 'DOMESTIC' AND customerType = '1'").Count() + "</td>");
                        html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Price + Tax:</b>" + String.Format("{0:N2}", (double.Parse(dt_.Compute("SUM(TOTALAMOUNT)", "").ToString())) + double.Parse(dt_.Compute("SUM(TAX)", "").ToString())) + "</td>");
                        html.Append("</tr>");
                        html.Append("<tr>");
                        html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Credit Consignments:</b>" + dt.Select("ConType= 'DOMESTIC' AND customerType = '2'").Count() + "</td>");
                        html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"></td>");
                        html.Append("</tr>");

                        rowCount = 1;
                        html.Append("</table>");

                    }
                }
                if (dt__.Rows.Count > 0)
                {
                    //html.Append("<div style=\"width:100% !important;  font-family:Calibri !important; page-break-before:always !important; text-align:center !important;\"><h2>LOCAL CONSIGNMENTS</b></div>");
                    serviceType = dt__.Rows[0]["ServiceTypeName"].ToString();
                    #region MyRegion
                    //html.Append(HeaderTable(false, true, serviceType, "<b>Local Consignments</b>"));
                    //for (int i = 0; i < dt__.Rows.Count; i++)
                    //{
                    //    #region Page Break And Service Type
                    //    if (rowCount == 25)
                    //    {
                    //        if (dt__.Rows[i]["ServiceTypeName"].ToString() == serviceType)
                    //        {
                    //            rowCount++;
                    //            html.Append("</table>");
                    //            html.Append(HeaderTable(true, false, "", "<b>Local Consignments</b>"));
                    //        }
                    //        else
                    //        {
                    //            rowCount = rowCount + 2;
                    //            serviceType = dt__.Rows[i]["ServiceTypeName"].ToString();
                    //            html.Append("</table>");
                    //            html.Append(HeaderTable(true, true, serviceType, "<b>Local Consignments</b>"));
                    //        }
                    //        rowCount = 0;
                    //    }
                    //    if (dt__.Rows[i]["ServiceTypeName"].ToString() != serviceType)
                    //    {
                    //        rowCount = rowCount + 2;
                    //        serviceType = dt__.Rows[i]["ServiceTypeName"].ToString();
                    //        html.Append(HeaderRow(true, serviceType));
                    //    }
                    //    #endregion

                    //    #region Data Row
                    //    html.Append("<tr>");
                    //    html.Append("<td style=\"text-align:center !important;float: left;width:10%\">" + dt__.Rows[i]["CN NO"].ToString() + "</td>");
                    //    if (dt__.Rows[i]["Consigner"].ToString().Length > 24)
                    //    {
                    //        html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt__.Rows[i]["Consigner"].ToString().Substring(0, 24) + "</td>");
                    //    }
                    //    else
                    //    {
                    //        html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt__.Rows[i]["Consigner"].ToString() + "</td>");
                    //    }
                    //    if (dt__.Rows[i]["Consignee"].ToString().Length > 24)
                    //    {
                    //        html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt__.Rows[i]["Consignee"].ToString().Substring(0, 24) + "</td>");
                    //    }
                    //    else
                    //    {
                    //        html.Append("<td ; style=\"text-align:left !important;float: left;width:16%\">" + dt__.Rows[i]["Consignee"].ToString() + "</td>");
                    //    }
                    //    html.Append("<td  style=\"text-align:center !important;float: left;width:8%\">" + dt__.Rows[i]["Origin"].ToString() + "</td>");
                    //    html.Append("<td  style=\"text-align:center !important;float: left;width:8%\">" + dt_.Rows[i]["DestinationBranch"].ToString() + "</td>");
                    //    html.Append("<td  style=\"text-align:center !important;float: left;width:8%\">" + dt_.Rows[i]["DestinationExpressCenter"].ToString() + "</td>");
                    //    html.Append("<td  style=\"text-align:center !important;float: left;width:6.4%\">" + dt__.Rows[i]["AccNo"].ToString() + "</td>");
                    //    html.Append("<td  style=\"text-align:center !important;float: left;width:6.4%\">" + String.Format("{0:N2}", dt__.Rows[i]["Weight"]).ToString() + "</td>");
                    //    html.Append("<td  style=\"text-align:right !important;float: left;width:6.4%\">" + String.Format("{0:N2}", dt__.Rows[i]["Tax"]).ToString() + "</td>");
                    //    html.Append("<td  style=\"text-align:right !important;float: left;width:6.4%\">" + String.Format("{0:N2}", dt__.Rows[i]["TotalAmount"]).ToString() + "</td>");
                    //    html.Append("<td  style=\"text-align:right !important;float: left;width:6.4%\">" + String.Format("{0:N0}", Math.Round(double.Parse(dt__.Rows[i]["Tax"].ToString()) + double.Parse(dt__.Rows[i]["TotalAmount"].ToString()))).ToString() + "</td>");
                    //    html.Append("</tr>");
                    //    rowCount++;
                    //    #endregion
                    //}
                    //if (rowCount + 6 > 25)
                    //{
                    //    html.Append("</table>");
                    //    html.Append(HeaderTable(true, false, "", "<b>Local Consignments</b>"));
                    //    //html.Append("</table>");
                    //} 
                    #endregion
                    html.Append("<table width=\"100%\">");
                    html.Append("<tr><td colspan='11' style=\"text-align:center !important; font-size:small !important;float: left; width:100%\">");
                    html.Append("<h3><u>LOCAL CONSIGNMENTS SUMMARY</u></h3>");
                    html.Append("</td></tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total no. of Consignments: </b>" + dt__.Rows.Count + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Consignment Price: </b>" + String.Format("{0:N2}", double.Parse(dt__.Compute("SUM(TOTALAMOUNT)", "").ToString())) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", double.Parse(dt__.Compute("SUM(Weight)", "").ToString())) + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Tax: </b>" + String.Format("{0:N2}", double.Parse(dt__.Compute("SUM(TAX)", "").ToString())) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Cash Consignments</b>: " + dt.Select("ConType= 'LOCAL' AND customerType = '1'").Count() + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Price + Tax:</b>" + String.Format("{0:N2}", (double.Parse(dt__.Compute("SUM(TOTALAMOUNT)", "").ToString())) + double.Parse(dt__.Compute("SUM(TAX)", "").ToString())) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Credit Consignments</b>: " + dt.Select("ConType= 'LOCAL' AND customerType = '2'").Count() + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"></td>");
                    html.Append("</tr>");
                    html.Append("</table>");
                }

                if (dt___.Rows.Count > 0)
                {
                    html.Append("<table width=\"100%\">");
                    html.Append("<tr><td colspan='11' style=\"text-align:center !important; font-size:small !important;float: left; width:100%\">");
                    html.Append("<h3><u>VOID CONSIGNMENTS SUMMARY</u></h3>");
                    html.Append("</td></tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total no. of Consignments: </b>" + dt___.Rows.Count + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Consignment Price: </b>" + String.Format("{0:N2}", double.Parse(dt___.Compute("SUM(TOTALAMOUNT)", "").ToString())) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", double.Parse(dt___.Compute("SUM(Weight)", "").ToString())) + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Tax: </b>" + String.Format("{0:N2}", double.Parse(dt___.Compute("SUM(TAX)", "").ToString())) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Cash Consignments</b>:" + dt.Select("ConType= 'VOID' AND customerType = '1'").Count() + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Price + Tax:</b>" + String.Format("{0:N2}", (double.Parse(dt___.Compute("SUM(TOTALAMOUNT)", "").ToString())) + double.Parse(dt___.Compute("SUM(TAX)", "").ToString())) + "</td>");
                    html.Append("</tr>");
                    html.Append("<tr>");
                    html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Credit Consignments</b>:" + dt.Select("ConType= 'VOID' AND customerType = '2'").Count() + "</td>");
                    html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"></td>");
                    html.Append("</tr>");
                    rowCount = 1;
                    html.Append("</table>");
                }

                rowCount = 1;

                //if (rowCount + 6 > 25)
                //{
                //    html.Append("</table>");
                //    html.Append(HeaderTable(true, false, "", ""));
                //    //html.Append("</table>");
                //}
                html.Append("<table width=\"100%\">");
                html.Append("<tr><td colspan='11' style=\"text-align:center !important; font-size:small !important;float: left; width:100%\">");
                html.Append("<h3><u>ALL CONSIGNMENTS SUMMARY</u></h3>");
                html.Append("</td></tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total no. of Consignments: </b>" + dt.Rows.Count + "</td>");
                html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Consignment Price: </b>" + String.Format("{0:N2}", double.Parse(dt.Compute("SUM(TOTALAMOUNT)", " Contype <> 'VOID' AND CustomerType = '1'").ToString())) + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Weight of Consignments: </b>" + String.Format("{0:N2}", double.Parse(dt.Compute("SUM(Weight)", "Contype <> 'VOID' AND CustomerType = '1'").ToString())) + "</td>");
                html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Tax: </b>" + String.Format("{0:N2}", double.Parse(dt.Compute("SUM(TAX)", "Contype <> 'VOID' AND CustomerType = '1'").ToString())) + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Cash Consignments</b>:" + dt.Select("customerType = '1'").Count() + "</td>");
                html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"><b>Total Price + Tax:</b>" + String.Format("{0:N2}", (double.Parse(dt.Compute("SUM(TOTALAMOUNT)", "customerType = '1'").ToString())) + double.Parse(dt.Compute("SUM(TAX)", "customerType = '1'").ToString())) + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='5' style=\"text-align:left !important;float: left; width:48%\"><b>Total Credit Consignments</b>:" + dt.Select("customerType = '2'").Count() + "</td>");
                html.Append("<td colspan='6' style=\"text-align:right !important;float: left; width:50%\"></td>");
                html.Append("</tr>");
                html.Append("</table>");
                ph1.Controls.Add(new Literal { Text = html.ToString() });

            }
            else
            {
                ph1.Controls.Add(new Literal { Text = "No Data Available" });
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
            "            <td style=\"width: 25% !important;float: left;\">\n" +
            "            </td>\n" +
            "            <td style=\"width: 50% !important; text-align: center !important;float: left;\">\n" +
            "                <b>DAILY CONSIGNMENTS REPORT</b><br />\n" +
            "                <b>" + con.GetExpressCenterName(clvar).Rows[0][0].ToString() + "</b>\n" +
            "            </td>\n" +
            "            <td style=\"width: 24% !important; text-align: right !important;float: left;\">\n" +
            "                <b>Created On: </b>" + DateTime.Now.ToString() + "\n" +
            "            </td>\n" +
            "        </tr>\n" +
            "        <tr>\n" +
            "            <td style=\"width: 25% !important;float: left;\">\n" +
            "            </td>\n" +
            "            <td style=\"width: 50% !important; text-align: center !important;float: left;\">\n" +
            "                " + type + "\n" +
            "            </td>\n" +
            "            <td style=\"width: 24% !important; text-align: right !important;float: left;\">\n" +
            "            </td>\n" +
            "        </tr>\n";

            header += "    </table>";
            header += HeaderRow(serviceType, st);
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
            string headerRow = "<table width=\"100%\" style=\"font-family:Calibri !important; font-size:smaller !important;float: left;\">";

            //if (serviceType)
            //{
            //    headerRow += ServiceType(st);
            //}
            //headerRow += "<tr>";

            //headerRow += "<td style=\"width:12% !important;text-align:center !important;float: left;\">";
            //headerRow += "<b>CN No.</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:16% !important;text-align:left !important;float: left;\">";
            //headerRow += "<b>Consigner</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:16% !important;text-align:left !important;float: left;\">";
            //headerRow += "<b>Consignee</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:8% !important;text-align:left !important;float: left;\">";
            //headerRow += "<b>Origin B</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:8% !important;text-align:left !important;float: left;\">";
            //headerRow += "<b>Dest B</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:8% !important;text-align:left !important;float: left;\">";
            //headerRow += "<b>Dest Exp Center</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:6.4% !important;text-align:left !important;float: left;\">";
            //headerRow += "<b>Acc No.</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:6.4% !important;text-align:left !important;float: left;\">";
            //headerRow += "<b>Weight</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:6.4% !important;text-align:right !important;float: left;\">";
            //headerRow += "<b>Tax</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:6.4% !important;text-align:right !important;float: left;\">";
            //headerRow += "<b>Price</b>";
            //headerRow += "</td>";

            //headerRow += "<td style=\"width:6.4% !important;text-align:right !important;float: left;\">";
            //headerRow += "<b>Total</b>";
            //headerRow += "</td>";

            //headerRow += "</tr>";
            return headerRow;
        }
        protected void btn_dayEnd_Click(object sender, EventArgs e)
        {
            div2.Style.Add("display", "none");
            DataTable dt = ViewState["dt"] as DataTable;

            DataTable newday = con.GetNewDayForDayEnd();
            //if (DateTime.Parse(newday.Rows[0][])
            //{

            //}
            //Session["WorkingDate"] = "2016-05-30";
            //int count = con.InsertDayEnd(clvar, dt);
            DateTime newWorkingDate = DateTime.Parse(newday.Rows[0]["WorkingDate"].ToString());
            if (DateTime.Compare(DateTime.Parse(Session["WorkingDate"].ToString()), newWorkingDate) > 0)
            {
                Errorid.Text = "Cannot Perform Day End. Day End Already Performed";
                return;
            }
            clvar.Day = Session["WorkingDate"].ToString();


            divDialogue.Style.Add("display", "block !important");
            lbl_error.Text = "Are You sure You want to Perform Day End for '" + HttpContext.Current.Session["WorkingDate"].ToString() + "'";
            return;

            //int updateCount = 

        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
        protected void btn_okDialogue_Click(object sender, EventArgs e)
        {
            int count = 0;
            count = con.GetCNCountForDayEnd();
            if (count == 0)
            {
                div1.Style.Add("display", "block !important");
                divDialogue.Style.Add("display", "none");
                lbl_error2.Text = "Total Consignments Are 0 (ZERO). Are You sure You still want to Perform Day End";
                return;
            }

            string altuFaltu = "";
            DataTable dt = con.CheckWorkingDate();

            DateTime date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            if (dt != null)
            {
                if (dt.Rows[0][0].ToString() == null || dt.Rows[0][0].ToString() == "")
                {

                }
                else
                {
                    DateTime maxWorkingDate = DateTime.Parse(Session["WorkingDate"].ToString());
                    altuFaltu = maxWorkingDate.ToString();
                    if (DateTime.Compare(maxWorkingDate, date) > 0)
                    {
                        Errorid.Text = "Cannot Perform Day End For Later Dates";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Perform Day End For Later Dates')", true);
                        return;
                    }
                    else if (DateTime.Compare(maxWorkingDate, date) == 0)
                    {
                        //Errorid.Text = "Day End Already Performed";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End Already Performed')", true);
                        //return;
                    }
                    else
                    {
                        //Errorid.Text = "Cannot Perform Day End For Later Dates";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Perform Day End For Later Dates')", true);
                        //return;
                    }
                }

            }

            string error = con.PerformDayEnd().ToString();



            if (error != "OK")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not perform Day End. Contact IT Support')", true);
                Errorid.Text = error;
                return;
            }
            else
            {

                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"")";
                string script = String.Format(script_, "DayCloseReport.aspx?Date=" + altuFaltu + "&m=1", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);

                //string script_1 = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //string script1 = String.Format(script_1, "~/login", "_blank", "");
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script1, true);
                ////System.Text.StringBuilder sb = new System.Text.StringBuilder();
                ////sb.Append("<script language='javascript'> ");
                //////sb.Append("window.close();");
                ////sb.Append("window.open('DayCloseReport.aspx?Date=" + clvar.Day + "', '_blank',");
                //////sb.Append("'top=0, left=0, width='+screen.availwidth+', height='+screen.availheight+', menubar=no,toolbar=no,status,resizable=no,addressbar=no');<");
                ////sb.Append("'top=0, left=0,scrollbars=yes,menubar=no,toolbar=no,status=no,resizable=no,addressbar=no');");
                ////sb.Append("</script>");

                ////Type t = this.GetType();

                ////if (!ClientScript.IsClientScriptBlockRegistered(t, "PopupScript"))
                ////    ClientScript.RegisterClientScriptBlock(t, "PopupScript", sb.ToString());

                //Session.Clear();
                //Response.Redirect("~/login");
            }
        }
        protected void btn_cancelDialogue_Click(object sender, EventArgs e)
        {
            divDialogue.Style.Add("display", "none");
            Response.Redirect("DayEnd.aspx");
            lbl_error.Text = "";
        }
        protected void btn_ok2Dialogue_Click(object sender, EventArgs e)
        {
            DataTable dt = con.CheckWorkingDate();
            string altuFaltu = "";
            DateTime date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            if (dt != null)
            {
                if (dt.Rows[0][0].ToString() == null || dt.Rows[0][0].ToString() == "")
                {

                }
                else
                {
                    DateTime maxWorkingDate = DateTime.Parse(Session["WorkingDate"].ToString());
                    altuFaltu = maxWorkingDate.ToString();
                    if (DateTime.Compare(maxWorkingDate, date) > 0)
                    {
                        Errorid.Text = "Cannot Perform Day End For Later Dates";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Perform Day End For Later Dates')", true);
                        return;
                    }
                    else if (DateTime.Compare(maxWorkingDate, date) == 0)
                    {
                        //Errorid.Text = "Day End Already Performed";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Day End Already Performed')", true);
                        //return;
                    }
                    else
                    {
                        //Errorid.Text = "Cannot Perform Day End For Later Dates";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Perform Day End For Later Dates')", true);
                        //return;
                    }
                }

            }
            string error = con.PerformDayEnd().ToString();
            if (error != "OK")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not perform Day End. Contact IT Support')", true);
                Errorid.Text = error;
                return;
            }
            else
            {
                string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                string script = String.Format(script_, "DayCloseReport.aspx?Date=" + altuFaltu + "&m=1", "_blank", "");
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.Append("<script language='javascript'> ");
                //sb.Append("window.close();");
                //sb.Append("window.open('DayCloseReport.aspx?Date=" + clvar.Day + "&m=1', '_blank',");
                ////sb.Append("'top=0, left=0, width='+screen.availwidth+', height='+screen.availheight+', menubar=no,toolbar=no,status,resizable=no,addressbar=no');<");
                //sb.Append("'top=0, left=0,scrollbars=yes,menubar=no,toolbar=no,status=no,resizable=no,addressbar=no');<");
                //sb.Append("/script>");

                //Type t = this.GetType();

                //if (!ClientScript.IsClientScriptBlockRegistered(t, "PopupScript"))
                //    ClientScript.RegisterClientScriptBlock(t, "PopupScript", sb.ToString());


                //Session.Clear();
                //Response.Redirect("~/login");
            }
        }
        protected void btn_cancel2Dialogue_Click(object sender, EventArgs e)
        {
            Response.Redirect("DayEnd.aspx");
            div1.Style.Add("display", "none");
        }
    }
}