using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class RunsheetInvoice_Plain : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get_RunSheet();
                PrintArrival();
            }
        }

        public void PrintArrival()
        {
            StringBuilder html = new StringBuilder();
            // clvar.Depot = Request.QueryString["createdby"].ToString();

            if (Request.QueryString["RCode"].ToString() == "")
            {
                clvar.Route = Request.QueryString["RCode"].ToString();
                //clvar.Depot = Request.QueryString["createdby"].ToString();
            }
            else
            {

                clvar.Route = Request.QueryString["RCode"].ToString();
                //clvar.Depot = Request.QueryString["createdby"].ToString();
            }


            clvar.ArrivalID = Int64.Parse((Request.QueryString["XCode"].ToString()));

            DataTable head = Get_RunSheetInvoice_header(clvar);
            //clvar.Route = head.Rows[0]["routecode"].ToString();
            DataTable dt = Get_RunSheetInvoice(clvar);

            if (head.Rows.Count > 0)
            {
                clvar.RunsheetNumber = head.Rows[0]["runsheetNumber"].ToString();
                clvar.Route = head.Rows[0]["route"].ToString();
                clvar.Code = head.Rows[0]["routeCode"].ToString();
                clvar.BranchManager = head.Rows[0]["sname"].ToString(); //--
                clvar.CourierName = head.Rows[0]["courier"].ToString();
                clvar.RiderCode = head.Rows[0]["riderCode"].ToString(); // --
                clvar.StartDate = head.Rows[0]["runsheetDate"].ToString();  //---

                clvar.CityCode = head.Rows[0]["Master_Route_Name"].ToString();  //---
                clvar.FlightNo = head.Rows[0]["Master_Route_Code"].ToString();  //---

                // NEW COLUMNS
                clvar._check_condition1 = head.Rows[0]["MeterStart"].ToString();  //---
                clvar._check_condtion2 = head.Rows[0]["MeterEnd"].ToString();  //---
                clvar._check_condition3 = head.Rows[0]["VEHICLENUMBER"].ToString();  //---
                clvar._check_condition4 = head.Rows[0]["VEHICLETYPE"].ToString();  //---

                Boolean flag = false;
                html.Append(HeaderTable(clvar, false));

                string sqlString = "";
                html.Append(TableStart());
                int count_ = dt.Rows.Count - 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 20 == 0 && i > 0)
                    {
                        html.Append(FooterTable());
                        html.Append("</table>");
                        html.Append(HeaderTable(clvar, true));
                        html.Append(TableStart());
                    }
                    if (i + 1 == dt.Rows.Count && (i + 1) % 2 != 0)
                    {

                        sqlString = "<tr style=\"height: 107px;border: 1px Solid Black;\">\n" +
                    "                <td style=\"width: 2%; border-right: 1px solid #000; vertical-align: middle; text-align:center; border: 1px Solid Black;border-right:1px solid black; \">\n" +
                    //"                    S" + (i + 1).ToString() +
                    "                    S" + (i + 1).ToString() + "<br>" + dt.Rows[i]["orign"].ToString() + "<br>" + dt.Rows[i]["destination"].ToString() + "<br>" + dt.Rows[i]["pieces"].ToString() + "<br>" + dt.Rows[i]["weight"].ToString() +
                    "                </td>\n" +

                   "                <td style=\"width: 20%; border-right: 1px solid #000; vertical-align: top;\">\n" +
                    "                    " + dt.Rows[i]["ConsignmentNumber"].ToString() + " #:" + dt.Rows[i]["consigneePhoneNo"].ToString() + "<br>";
                        if (dt.Rows[i]["COD"].ToString() == "1")
                        {
                            sqlString += "COD Rs. " + dt.Rows[i]["CODAMOUNT"].ToString() + "<br>";
                            if (dt.Rows[i]["Consignee"].ToString().Length > 24)
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString().Substring(0, 24) + "..." + "";
                            }
                            else
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString() + "";
                            }

                            sqlString += //"<span style=\"   display: block; \">Consignee#:  " + dt.Rows[i]["consigneePhoneNo"].ToString() + "</span>" +
                                "<span style=\"   display: block; \">Ad:  " + dt.Rows[i]["address"].ToString() + "</span>" +
                        //        "<span style=\" style=\"left: 0%; border-right: 1px solid Black; position: relative; float: left; text-align: center; width: 33%; top: 4px; border-top: 1px solid Black;  \">" +
                        //"                    " + dt.Rows[i]["orign"].ToString() + "</span><span style=\"position: relative; border-right: 1px solid Black; float: left; text-align: center; width: 32%; top: 4px; border-top: 1px solid Black;\">" +
                        //"                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"position: relative; float: left; text-align: center; top: 4px; border-top: 1px solid Black; width: 33%;\">" +
                        //"                    " + dt.Rows[i]["weight"].ToString() + 
                        "</span><span style=\"position: relative; left: 0%; border-top: 1px solid Black; float: left; width: 100%; top: 4px;\">CNIC# " +
                        "                    " + dt.Rows[i]["Receiver_CNIC"].ToString() + "</span>" +
                        "                </td>\n";
                        }
                        else
                        {
                            sqlString += "";
                            if (dt.Rows[i]["Consignee"].ToString().Length > 24)
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString().Substring(0, 24) + "..." + "";
                            }
                            else
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString() + "";
                            }
                            sqlString += //"<span style=\"display: block; \">Consignee#:  " + dt.Rows[i]["consigneePhoneNo"].ToString() + "</span>" +
                                "<span style=\"   display: block; \">Ad:  " + dt.Rows[i]["address"].ToString() + "</span>" +
                           //"<span style=\"left: 0%; border-right: 1px solid Black; position: relative; float: left; text-align: center; width: 33%; top: 4px; border-top: 1px solid Black;\">" +
                           //"                    " + dt.Rows[i]["orign"].ToString() +" - "+ dt.Rows[i]["destination"].ToString() + "</span><span style=\"position: relative; border-right: 1px solid Black; float: left; text-align: center; width: 32%; top: 4px; border-top: 1px solid Black;\">" +
                           //"                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"position: relative; float: left; text-align: center; top: 4px; border-top: 1px solid Black; width: 33%;\">" +
                           //"                    " + dt.Rows[i]["weight"].ToString() + 
                           "</span><span style=\"position: relative; left: 0%; float: left; width: 100%; \">CNIC# " +
                           "                    " + dt.Rows[i]["Receiver_CNIC"].ToString() + "</span> </td>";

                        }
                        sqlString += "                <td style=\"width: 13%; border-right: 1px solid #000; vertical-align: top;\">" + dt.Rows[i]["remarks"].ToString() + "\n" +
                        "                    \n" +
                        "                </td>\n" +

                        "                <td style=\"width: 13%; border-right: 1px solid #000; vertical-align: top;text-align: left;\">\n";
                        if (dt.Rows[i]["RiderName"].ToString().Trim() != "")
                        {
                            sqlString += "Given To Rider:</br>";
                        }
                        sqlString += "" +
                        dt.Rows[i]["RiderName"].ToString() +
                        "                </td>\n" +

                        //"                <td style=\"width: 18%; vertical-align: top; position: relative; left: 1%;\">\n" +
                        //"                    \n" +
                        //"                </td>\n" +

                        "            </tr>";

                        //"<tr>" +
                        //"<td colspan='2'>S1: Serial Number   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>KHI: Orign   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>KHI: Destination   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>1: Pieces   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>0.5: Wieght   </td>" +
                        //"</tr>";
                        html.Append(sqlString);

                    }
                    else
                    {
                        sqlString = "<tr style=\"height: 85px;border: 1px Solid Black;\">\n" +

                    //    "                <td class='bnm' style=\"width: 2%; vertical-align: top; position: relative; left: 2%;border: 1px Solid Black;border-right:1px solid black; \">\n" +
                    "                <td style=\"width: 2%; border-right: 1px solid #000; vertical-align: middle; text-align:center; border: 1px Solid Black;border-right:1px solid black; \">\n" +
                    "                    S" + (i + 1).ToString() + "<br>" + dt.Rows[i]["orign"].ToString() + "<br>" + dt.Rows[i]["destination"].ToString() + "<br>" + dt.Rows[i]["pieces"].ToString() + "<br>" + dt.Rows[i]["weight"].ToString() +
                    "                </td>\n" +

                    "                <td style=\"width: 20%; border-right: 1px solid #000; vertical-align: top; \">\n" +
                    "                    " + dt.Rows[i]["ConsignmentNumber"].ToString() + " #:" + dt.Rows[i]["consigneePhoneNo"].ToString() + "<br>";
                        if (dt.Rows[i]["COD"].ToString() == "1")
                        {
                            sqlString += "COD Rs. " + dt.Rows[i]["CODAMOUNT"].ToString() + "<br>";
                            if (dt.Rows[i]["Consignee"].ToString().Length > 24)
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString().Substring(0, 24) + "..." + "";
                            }
                            else
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString() + "";
                            }

                            sqlString += //"<span style=\"display: block; \">Consignee#:  " + dt.Rows[i]["consigneePhoneNo"].ToString() + "</span>" +
                                "<span style=\"   display: block; \">Ad:  " + dt.Rows[i]["address"].ToString() + "</span>" +
                        //        "<span style=\"left: 0%; border-right: 1px solid Black; position: relative; float: left; text-align: center; width: 33%; top: 4px; border-top: 1px solid Black; \">" +
                        //"                    " + dt.Rows[i]["orign"].ToString() +" - "+ dt.Rows[i]["destination"].ToString() + "</span><span style=\"position: relative; border-right: 1px solid Black; float: left; text-align: center; width: 32%; top: 4px; border-top: 1px solid Black;\">" +
                        //"                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"position: relative; float: left; text-align: center; top: 4px; border-top: 1px solid Black; width: 33%;\">" +
                        //"                    " + dt.Rows[i]["weight"].ToString() + "</span>"+
                        "<span style=\"position: relative; left: 0%; float: left; width: 100%; \">CNIC# " +
                        "                    " + dt.Rows[i]["Receiver_CNIC"].ToString() + "</span>" +
                        "                </td>\n";
                        }
                        else
                        {
                            sqlString += "";
                            if (dt.Rows[i]["Consignee"].ToString().Length > 24)
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString().Substring(0, 24) + "..." + "";
                            }
                            else
                            {
                                sqlString += "                    " + dt.Rows[i]["consignee"].ToString() + "";
                            }
                            sqlString += //"<span style=\"   display: block; \">Consignee#:  " + dt.Rows[i]["consigneePhoneNo"].ToString() + "</span>" +
                                "<span style=\"   display: block; \">Ad:  " + dt.Rows[i]["address"].ToString() + "</span>" +
                        //        "<span style=\"left: 0%; border-right: 1px solid Black; position: relative; float: left; text-align: center; width: 33%; top: 4px; border-top: 1px solid Black;\">" +
                        //"                    " + dt.Rows[i]["orign"].ToString() +" - "+ dt.Rows[i]["destination"].ToString()+ "</span><span style=\"position: relative; border-right: 1px solid Black; float: left; text-align: center; width: 32%; top: 4px; border-top: 1px solid Black;\">" +
                        //"                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"position: relative; float: left; text-align: center; top: 4px; border-top: 1px solid Black; width: 33%;\">" +
                        //"                    " + dt.Rows[i]["weight"].ToString() + "</span>"+
                        "<span style=\"position: relative; left: 0%; float: left; width: 100%; \">CNIC# " +
                        "                    " + dt.Rows[i]["Receiver_CNIC"].ToString() + "</span> </td>";

                        }
                        sqlString += "                <td style=\"width: 13%; border-right: 1px solid #000; vertical-align: top;\">" + dt.Rows[i]["remarks"].ToString() + "\n" +

                        "                </td>\n" +
                        "                <td style=\"width: 13%; border-right: 1px solid #000; vertical-align: top;text-align: left;\">\n";
                        if (dt.Rows[i]["RiderName"].ToString().Trim() != "")
                        {
                            sqlString += "Given To Rider:</br>";
                        }
                        sqlString += "" +
                        dt.Rows[i]["RiderName"].ToString() +
                        "                </td>\n" +


                        "                <td style=\"width: 2%; border-right: 1px solid #000; vertical-align: middle; text-align:center; border-right:1px solid black; \">\n" +
                        // "                    " + (i + 2).ToString() +
                        "                    S" + (i + 2).ToString() + "<br>" + dt.Rows[i + 1]["orign"].ToString() + "<br>" + dt.Rows[i + 1]["destination"].ToString() + "<br>" + dt.Rows[i + 1]["pieces"].ToString() + "<br>" + dt.Rows[i + 1]["weight"].ToString() +

                        "                </td>\n";
                        if (dt.Rows[i + 1]["COD"].ToString() == "1")
                        {
                            sqlString += "                <td style=\"width: 20%; vertical-align: top; position: relative; border-right:1px solid black;/*left: 1%;*/\">\n" +
                                         "                    " + dt.Rows[i + 1]["ConsignmentNumber"].ToString() + " #:" + dt.Rows[i + 1]["consigneePhoneNo"].ToString() + "<br>" +
                                         "               COD Rs. " + dt.Rows[i + 1]["CODAMOUNT"].ToString() + "<br>";
                            if (dt.Rows[i + 1]["Consignee"].ToString().Length > 24)
                            {
                                sqlString += dt.Rows[i + 1]["Consignee"].ToString().Substring(0, 24) + "...";
                            }
                            else
                            {
                                sqlString += dt.Rows[i + 1]["Consignee"].ToString();
                            }
                            sqlString += //"<span style=\"display: block; \">Consignee#:  " + dt.Rows[i + 1]["consigneePhoneNo"].ToString() + "</span>" +
                                "<span style=\"   display: block; \">Ad:  " + dt.Rows[i + 1]["address"].ToString() + "</span>" +
                            //"<span style=\"left: 0%; border-right: 1px solid Black; position: relative; float: left; text-align: center; width: 33%; top: 4px; border-top: 1px solid Black;\">" +
                            //"                    " + dt.Rows[i + 1]["orign"].ToString() + " - " + dt.Rows[i]["destination"].ToString() + "</span><span style=\"position: relative; border-right: 1px solid Black; float: left; text-align: center; width: 32%; top: 4px; border-top: 1px solid Black;\">" +
                            //"                    " + dt.Rows[i + 1]["pieces"].ToString() + "</span><span style=\"position: relative; float: left; text-align: center; top: 4px; border-top: 1px solid Black; width: 33%; \">" +
                            //"                    " + dt.Rows[i + 1]["weight"].ToString() + "</span>"+
                            "<span style=\"position: relative; left: 0%; float: left; width: 100%; \">CNIC# " +
                            "                    " + dt.Rows[i]["Receiver_CNIC"].ToString() + "</span>" +

                            "                </td>\n";
                        }
                        else
                        {
                            sqlString += "                <td style=\"width: 20%; border-right: 1px solid #000; vertical-align: top; position: relative; left: 0%;\">\n" +
                        "                    " + dt.Rows[i + 1]["ConsignmentNumber"].ToString() + " #:" + dt.Rows[i + 1]["consigneePhoneNo"].ToString() + "<br>";
                            if (dt.Rows[i + 1]["Consignee"].ToString().Length > 24)
                            {
                                sqlString += dt.Rows[i + 1]["Consignee"].ToString().Substring(0, 24) + "...";
                            }
                            else
                            {
                                sqlString += dt.Rows[i + 1]["Consignee"].ToString();
                            }
                            sqlString += "" +
                        //"                    " + "<br> Consignee#:  " +
                        //"                    " + dt.Rows[i + 1]["consigneePhoneNo"].ToString() + 
                        " <br>Ad:" +
                        "                    " + dt.Rows[i + 1]["address"].ToString() + " <br>" +
                        //"<span style=\"left: 0%; border-right: 1px solid Black; position: relative; float: left; text-align: center; width: 33%; top: 4px; border-top: 1px solid Black;\">" +
                        //"                    " + dt.Rows[i + 1]["orign"].ToString() + " - " + dt.Rows[i]["destination"].ToString() + "</span><span style=\"position: relative; border-right: 1px solid Black; float: left; text-align: center; width: 32%; top: 4px; border-top: 1px solid Black;\">" +
                        //"                    " + dt.Rows[i + 1]["pieces"].ToString() + "</span><span style=\"position: relative; float: left; text-align: center; top: 4px; border-top: 1px solid Black; width: 33%; \">" +
                        //"                    " + dt.Rows[i + 1]["weight"].ToString() + "</span>"+
                        "<span style=\"position: relative; left: 0%; float: left; width: 100%; \">CNIC# " +
                        "                    " + dt.Rows[i + 1]["Receiver_CNIC"].ToString() + "</span>" +

                        "                </td>\n";
                        }
                        sqlString += "                <td style=\"width: 13%; border-right: 1px solid #000; vertical-align: top;\">" + dt.Rows[i + 1]["remarks"].ToString() + "\n" +

                        "                </td>\n" +
                        "                <td style=\"width: 13%; border-right: 1px solid #000;  vertical-align: top;text-align: left;\">\n";
                        if (dt.Rows[i + 1]["RiderName"].ToString().Trim() != "")
                        {
                            sqlString += "Given To Rider:</br>";
                        }
                        sqlString += "" +
                        dt.Rows[i + 1]["RiderName"].ToString() +
                        "                </td>\n";


                        sqlString += "            </tr>";
                        //    "<tr>" +
                        //"<td colspan='2'>S1: Serial Number   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>KHI: Orign   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>KHI: Destination   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>1: Pieces   </td>" +
                        //"</tr>" +
                        //"<tr>" +
                        //"<td colspan='2'>0.5: Wieght   </td>" +
                        //"</tr>";

                        html.Append(sqlString);
                        i++;
                    }
                }

                html.Append(FooterTable());
                ph1.Controls.Add(new Literal { Text = html.ToString() });

                //            HttpContext.Current.Response.Write("<script>window.print();</script>");
            }
            else
            {
                //  lbl_error.Text = "No Data Found";
            }
        }

        public string HeaderTable(Variable clvar, bool breakPage)
        {
            DataTable RunType = Get_RunSheetType(clvar);

            string pageBreak = "";
            if (breakPage)
            {
                pageBreak = "page-break-before:always;";
            }
        http://localhost:57507/MISOCS/images/mnpLogo.png
            string sqlString = "<div style='width: 96%; " + pageBreak + "'> <div style='float: left; padding: 0px 0px 0px 29px;'><img src=\"../images/mnpLogo.png\"  width=\"80\" height=\"38\"> </div>" +
            " <div class='barcode' style='float: left; padding: 0px 0px 0px 29px;'><p style='font-family:IDAutomationHC39M;float: left;padding: 0px 0px 0px 0px;margin: 0 0 0 0'>*" + clvar.RunsheetNumber + "*</p></div><div style='float: left;width: 50%;text-align: center; padding: 12px 0px 0px;'><b align='center' style='text-align:center;'>DELIVERY & POD SHEET</b></div> " +
            " <div  style='float: right;text-align: right; padding: 12px 0px 0px;'>" + RunType.Rows[0]["code"].ToString() + "</div></div> <br>" +

            "       <table style=\"width: 96%; border: 1px Solid Black; \n" +
            "            border-collapse: collapse; font-family: Calibri; font-size: 12px; left: 2%;position: relative;\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black; border-bottom: 1px Solid Black\">\n" +
            "                    DELIVERY DATE\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black; border-bottom: 1px Solid Black border-right:0;\">\n" +
            "                    " + clvar.StartDate + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 6%; text-align: left; border: 1px Solid Black; border-bottom: 1px Solid Black;border-left:0;\">\n" +
            "                    BRANCH\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black; border-bottom: 1px Solid Black\">\n" +
            "                    " + clvar.BranchManager + "\n" +
            "                </td>\n" +
            //"                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    ROUTE NAME\n" +
            //"                </td>\n" +
            //"                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    " + clvar.Route + "\n" +
            //"                </td>\n" +
            //"                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    ROUTE CODE\n" +
            //"                </td>\n" +
            //"                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    " + clvar.Code + "\n" +
            //"                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    COURIER OFFICER\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar.CourierName + "\n" +
            "                </td>\n" +
               "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    COURIER CODE\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar.RiderCode + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +
                "            <tr>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    SHEET NUMBER\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar.RunsheetNumber + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    R/M ROUTE CODE\n" +
            "                </td>\n" +
            "                <td colspan='6' style=\"width: 15%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar.Route + " -- " + clvar.Code + " | " + clvar.CityCode + " -- " + clvar.FlightNo + "\n" +
            "                </td>\n" +



            //"                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    COURIER OFFICER\n" +
            //"                </td>\n" +
            //"                <td style=\"width: 15%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    " + clvar.CourierName + "\n" +
            //"                </td>\n" +
            //   "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    COURIER CODE\n" +
            //"                </td>\n" +
            //"                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            //"                    " + clvar.RiderCode + "\n" +
            //"                </td>\n" +
            "            </tr> \n" +
            "            <tr>\n" +
            "                </td>\n" +
               "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    METER START\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar._check_condition1 + "\n" +
            "                </td>\n" +
               "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    METER END\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar._check_condtion2 + "\n" +
            "                </td>\n" +
               "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    VEHICLE NUMBER\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar._check_condition3 + "\n" +
            "                </td>\n" +
               "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    VEHICLE TYPE\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: left; border: 1px Solid Black;\">\n" +
            "                    " + clvar._check_condition4 + "\n" +
            "            </tr> \n" +
            "</table>\n";

            return sqlString;
        }

        public string TableStart()
        {

            string sqlString = "<table style=\"width: 100%; border: 1px Solid Black; \n" +
            "            border-collapse: collapse; font-family: Calibri; font-size: 12px; width: 96%; position: relative; left: 2%;\">\n" +
            "            <tr>\n" +
            //"                <td\n" +
            //"                    style=\"width: 1%; \n" +
            //"                    \">\n" +
            //"                    \n" +
            //"                </td>\n" +
            //"                <td style=\"width: 22%; \n" +
            //"                    \">\n" +
            //"                    \n" +
            //"                </td>\n" +
            //"                <td style=\"width: 10%; \n" +
            //"                    \">\n" +
            //"                    \n" +
            //"                </td>\n" +
            //"                <td style=\"width: 1%; \n" +
            //"                    \">\n" +
            //"                    \n" +
            //"                </td>\n" +
            //"                <td style=\"width: 22%; \n" +
            //"                    \">\n" +
            //"                    \n" +
            //"                </td>\n" +
            //"                <td style=\"width: 10%; \n" +
            //"                    \">\n" +
            //"                    \n" +
            //"                </td>\n" +
            "            </tr>" +
                        "<tr>" +
                        "<td colspan='8' style='text-align:center;text-transform: full-width; font-size: 11px;font-weight:bold;'>S1: Serial Number | KHI: Orign | KHI: Destination | 1: Pieces | 0.5: Wieght</td>" +
                        "</tr>";
            //"<tr>" +
            //"<td colspan='2'>   </td>" +
            //"</tr>" +
            //"<tr>" +
            //"<td colspan='2'>   </td>" +
            //"</tr>" +
            //"<tr>" +
            //"<td colspan='2'>   </td>" +
            //"</tr>" +
            //"<tr>" +
            //"<td colspan='2'>   </td>" +
            //"</tr>";
            return sqlString;
        }

        public string DataRow(DataRow dr, Boolean flag)
        {
            string sqlString = "";
            if (flag)
            {

                sqlString =
                    "<tr>\n" +
            "                <td style=\" width: 18%\">\n" +
            "                    " + dr["ConsignmentNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td  style=\" width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 18%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "            </tr>";
            }
            else
            {
                sqlString = "<tr>\n" +
            "                <td style=\" width: 18%\">\n" +
            "                    " + dr[""] + "\n" +
            "                </td>\n" +
            "                <td style=\" width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 18%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\" width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "            </tr>";
            }


            return sqlString;

        }

        public string FooterTable()
        {
            string sqlString =
                 "      </table><br><br> <table style=\"width: 96%;margin-top:30px; \">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 27%; text-align: center; border-top: 1px Solid Black; float: left;position: relative;left: 25px;margin-right: 40px; font-weight:bold \">\n" +
            "                    Courier/SDO Signature\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: center; border-top: 1px Solid Black; float: left;position: relative;left: 25px;margin-right: 40px;font-weight:bold; \">\n" +
            "                    OPS Incharge/Supervisor\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: center; border-top: 1px Solid Black; float: left;position: relative;left: 25px;margin-right: 40px;font-weight:bold; \">\n" +
            "                    Security Incharge/Supervisor\n" +
            "                </td>\n" +
            "           </tr>\n" +
             "</table>\n";

            return sqlString;
        }

        public DataTable Get_RunSheetInvoice(Variable clvar)
        {
            /*
            string sqlString = "select\n" +
            "r.runsheetNumber, r.runsheetDate, r.routeCode, r.route, c.consignmentNumber, c.consignee, c.pieces, c.weight,\n" +
            "rc.deliveryDate, b.sname, b1.sname orign, rs.firstName + ' '+rs.lastName courier, rr.riderCode\n" +
            "from\n" +
            "Runsheet r , RunsheetConsignment rc, RiderRunsheet rr, Riders rs, Branches b, Branches b1, Consignment c\n" +
            "where\n" +
            "r.runsheetNumber = rc.runsheetNumber\n" +
            "and r.branchCode = b.branchCode\n" +
            "and rc.consignmentNumber = c.consignmentNumber\n" +
            "and rc.branchcode = r.branchCode\n" +
            "and c.orgin = b1.branchCode\n" +
            "and rr.runsheetNumber = r.runsheetNumber\n" +
            "and rr.riderCode = rs.riderCode\n" +
            "and rr.expIdTemp = rs.expressCenterId\n" +
            "and r.runsheetNumber = '" + clvar.ArrivalID + "' \n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
            "and rs.branchid = '" + HttpContext.Current.Session["branchcode"].ToString() + "'\n" +
            "--and rr.riderCode = (select riderCode from Riders r1 where r1.routeCode = r.routeCode and r1.riderCode = rs.riderCode)\n" +
            "and r.routeCode = '" + clvar.Route + "'\n" +
            "order by SortOrder asc";
            */

            string sqlString = "select c.consignmentNumber, c.pieces, c.weight, b.sname orign, c.consignee \n" +
            "from RunsheetConsignment rc, Consignment c, Branches b \n" +
            "where \n" +
            "rc.consignmentNumber = c.consignmentNumber\n" +
            "and rc.branchCode = b.branchCode\n" +
            "and rc.runsheetNumber = '" + clvar.ArrivalID + "' \n" +
            "and rc.createdBy='" + clvar.Depot + "'\n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
            "order by rc.SortOrder asc ";

            sqlString = "select c.consignmentNumber, c.pieces, c.weight, b.sname orign, c.consignee\n" +
           "  from Runsheet R\n" +
           "\n" +
           " INNER JOIN RunsheetConsignment rc\n" +
           "    ON R.runsheetNumber = rc.runsheetNumber\n" +
           "   AND R.branchCode = rc.branchcode\n" +
           "   AND R.createdBy = rc.createdBy\n" +
           "   AND R.routeCode = rc.RouteCode\n" +
           "\n" +
           " INNER JOIN Consignment c\n" +
           "    ON C.consignmentNumber = rc.consignmentNumber\n" +
           "\n" +
           " INNER JOIN Branches B\n" +
           "    ON B.branchCode = C.orgin\n" +
           "\n" +
           " where rc.runsheetNumber = '" + clvar.ArrivalID + "'\n" +
           "   AND R.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "'\n" +
           "   AND R.routeCode = '" + clvar.Route + "'\n" +
           " order by rc.SortOrder desc";

            sqlString = "selecT temp.consignmentNumber,temp.remarks,temp.consigneePhoneNo,\n" +
            "       temp.pieces,\n" +
            "       round(temp.weight,2) weight,\n" +
            "       temp.orign,\n" +
            "       temp.destination, \n" +
            "       temp.consignee,\n" +
            "       CAST((lower(temp.[address])) AS NCHAR(80)) address, \n" +
            "       temp.cod,\n" +
            "       temp.SortOrder,\n" +
            "       SUM(temp.codAmount) CODAMOUNT, temp.Receiver_CNIC, temp.RiderName\n" +
            "  from (select c.consignmentNumber,c.remarks,c.consigneePhoneNo,\n" +
            "               c.pieces,\n" +
            "               c.weight,\n" +
            "               b.sname orign,\n" +
            "               b2.sname destination, \n" +
            "               c.consignee,\n" +
            "               c.[address], \n" +
            "               c.cod,\n" +
            "               rc.SortOrder,\n" +
            "               cd.codAmount, rc.Receiver_CNIC, \n" +
            "               ri.riderCode + ' ' + ri.firstName + ' ' + ri.lastName RiderName \n" +
            "          from Runsheet R\n" +
            "\n" +
            "         INNER JOIN RunsheetConsignment rc\n" +
            "            ON R.runsheetNumber = rc.runsheetNumber\n" +
            "           AND R.branchCode = rc.branchcode\n" +
            "           --AND R.createdBy = rc.createdBy\n" +
            "           AND R.routeCode = rc.RouteCode\n" +
            "\n" +
            "         INNER JOIN Consignment c\n" +
            "            ON C.consignmentNumber = rc.consignmentNumber\n" +
            "\n" +
            "         INNER JOIN Branches B\n" +
            "            ON B.branchCode = C.orgin\n" +
            "         INNER JOIN Branches b2 ON c.destination = b2.branchCode \n" +
            "          left outer join CODConsignmentDetail cd\n" +
            "            on cd.consignmentNumber = c.consignmentNumber\n" +
            "          left outer join Riders ri\n" +
            "            on rc.branchcode = ri.branchId\n" +
            "           and rc.GivenToRider = ri.riderCode\n" +
            "         where rc.runsheetNumber = '" + clvar.ArrivalID + "'\n" +
            "           AND R.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "'\n" +
            "           AND R.routeCode = '" + clvar.Route + "'\n" +
            "\n" +
            "        union\n" +
            "        select c.consignmentNumber,c.remarks,c.consigneePhoneNo,\n" +
            "               c.pieces,\n" +
            "               c.weight,\n" +
            "               b.sname orign,\n" +
            "               b2.sname destination, \n" +
            "               c.consignee,\n" +
            "               c.[address], \n" +
            "               c.cod,\n" +
            "               rc.SortOrder,\n" +
            "               cd.codAmount, rc.Receiver_CNIC, \n" +
            "               ri.riderCode + ' ' + ri.firstName + ' ' + ri.lastName RiderName\n" +
            "          from Runsheet R\n" +
            "\n" +
            "         INNER JOIN RunsheetConsignment rc\n" +
            "            ON R.runsheetNumber = rc.runsheetNumber\n" +
            "           AND R.branchCode = rc.branchcode\n" +
            "           --AND R.createdBy = rc.createdBy\n" +
            // "           AND R.routeCode = rc.RouteCode\n" +
            "\n" +
            "         INNER JOIN Consignment c\n" +
            "            ON C.consignmentNumber = rc.consignmentNumber\n" +
            "\n" +
            "         INNER JOIN Branches B\n" +
            "            ON B.branchCode = C.orgin\n" +
            "         INNER JOIN Branches b2 ON c.destination = b2.branchCode \n" +
            "          left outer join CODConsignmentDetail_New cd\n" +
            "            on cd.consignmentNumber = c.consignmentNumber\n" +
            "          left outer join Riders ri\n" +
            "            on rc.branchcode = ri.branchId\n" +
            "           and rc.GivenToRider = ri.riderCode\n" +
            "         where rc.runsheetNumber = '" + clvar.ArrivalID + "'\n" +
            "           AND R.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "'\n" +
            "           AND R.routeCode = '" + clvar.Route + "'\n" +
            "\n" +
            "        ) temp\n" +
            " group by temp.consignmentNumber,\n" +
            "          temp.pieces,\n" +
            "          temp.weight,\n" +
            "          temp.orign,\n" +
            "          temp.destination, \n" +
            "          temp.consignee,\n" +
            "          temp.[address], \n" +
            "          temp.cod,\n" +
            "          temp.SortOrder, temp.Receiver_CNIC,\n" +
            "          temp.RiderName,temp.remarks, temp.consigneePhoneNo\n" +
            "\n" +
            " order by temp.SortOrder desc";






            DataTable dt = new DataTable();
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

        public DataTable Get_RunSheetInvoice_header(Variable clvar)
        {
            string sqlString = "select r.runsheetNumber, r.route, r.routeCode, b.sname, rs.firstName + ' '+rs.lastName courier, rr.riderCode, convert(varchar(10),r.runsheetDate,105) runsheetDate \n" +
            "from RunsheetConsignment rc,Runsheet r,\n" +
            "(select r.*,e.bid from RiderRunsheet r,ExpressCenters e\n" +
            "where r.expIdTemp=e.expressCenterCode)rr\n" +
            ",Riders rs\n" +
            ", Branches b, Consignment c\n" +
            "where\n" +
            "rc.runsheetNumber=r.runsheetNumber\n" +
            "and rc.branchcode=r.branchCode\n" +
            "and rs.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "and rc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "and rr.runsheetNumber=rc.runsheetNumber\n" +
            "and rr.bid=rc.branchcode\n" +
            "and rr.riderCode=rs.riderCode\n" +
            "and rr.expIdTemp=rs.expressCenterId\n" +
            "and rc.runsheetNumber= '" + clvar.ArrivalID + "' \n" +
            "and r.branchCode = b.branchCode\n" +
            "and rc.consignmentNumber = c.consignmentNumber\n" +
            //  "and c.orgin = b1.branchCode\n" +
            "and r.branchCode = b.branchCode \n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n";


            sqlString = "select r.runsheetNumber,\n" +
           "       r.route,\n" +
           "       r.routeCode,\n" +
           "       b.sname,\n" +
           "       rs.firstName + ' ' + rs.lastName courier,\n" +
           "       rr.riderCode,\n" +
           "       convert(varchar(10), r.runsheetDate, 105) runsheetDate\n" +
           "\n" +
           "  FROM RUNSHEET R\n" +
           "\n" +
           " INNER JOIN RiderRunsheet RR\n" +
           "    ON R.runsheetNumber = RR.runsheetNumber\n" +
           "   AND R.createdBy = RR.createdBy\n" +
           "\n" +
           " INNER JOIN Riders RS\n" +
           "    ON RS.branchId = R.branchCode\n" +
           "   AND RS.routeCode = R.routeCode\n" +
           "   AND RS.riderCode = RR.riderCode\n" +
           "   AND RS.expressCenterId = RR.expIdTemp\n" +
           "\n" +
           " INNER JOIN Branches B\n" +
           "    ON B.branchCode = R.branchCode\n" +
           "\n" +
           " WHERE R.runsheetNumber = '" + clvar.ArrivalID + "'\n" +
           "   AND R.routeCode = '" + clvar.Route + "'\n" +
           "   AND R.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "'";


            sqlString = "SELECT R.RUNSHEETNUMBER,\n" +
            "       RC.NAME ROUTE,\n" +
            "       R.ROUTECODE,\n" +
            "       B.SNAME,\n" +
            "       RS.FIRSTNAME + ' ' + RS.LASTNAME COURIER,\n" +
            "       RR.RIDERCODE,\n" +
            "       CONVERT(VARCHAR(10), R.RUNSHEETDATE, 105) RUNSHEETDATE,\n" +
            "       r.MeterStart, r.MeterEnd, r.VEHICLENUMBER, vt.TypeDesc VEHICLETYPE, RP.Master_Route_Name, RP.Master_Route_Code, \n" +
            "       RC.NAME +' -- '+R.ROUTECODE + ' | ' +RP.Master_Route_Name+' -- '+RP.Master_Route_Code ROUTE_MASTER \n" +
            "\n" +
            "  FROM RUNSHEET R\n" +
            "\n" +
            " INNER JOIN RIDERRUNSHEET RR\n" +
            "    ON R.RUNSHEETNUMBER = RR.RUNSHEETNUMBER\n" +
            "   AND R.CREATEDBY = RR.CREATEDBY\n" +
            "\n" +
            " INNER JOIN RIDERS RS\n" +
            "    ON RS.BRANCHID = R.BRANCHCODE\n" +
            //  "   AND RS.ROUTECODE = R.ROUTECODE\n" +
            "   AND RS.RIDERCODE = RR.RIDERCODE\n" +
            " --  AND RS.EXPRESSCENTERID = RR.EXPIDTEMP\n" +

            // "LEFT JOIN Route_Profile_Master RP ON RS.Master_routes = RP.Master_Route_Code AND RP.[Status] = '1' \n" +
            "\n" +
            " INNER JOIN BRANCHES B\n" +
            "    ON B.BRANCHCODE = R.BRANCHCODE\n" +

            "  LEFT OUTER JOIN ROUTES RC\n" +
            // "    ON RC.CITYID = B.CITYID\n" +
            "   ON rc.BID = r.branchCode \n" +
            "   AND RC.ROUTECODE = R.ROUTECODE\n" +

            "   LEFT JOIN Route_Profile_Master RP \n" +
            "   ON rp.Master_Route_Code = rc.RouteTerritory\n" +
            "   AND rp.BranchCode = rc.BID \n" +

            "  LEFT JOIN Vehicle_Type vt ON r.VEHICLETYPE = vt.TypeID \n" +
            " WHERE R.RUNSHEETNUMBER = '" + clvar.ArrivalID + "'\n" +
           "    AND R.ROUTECODE = '" + clvar.Route + "'\n" +
           "    AND R.BRANCHCODE = '" + HttpContext.Current.Session["branchcode"].ToString() + "'";

            DataTable dt = new DataTable();

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

        public DataTable Get_RunSheetType(Variable clvar)
        {
            string sqlString = "SELECT l.code FROM Runsheet r \n" +
                               "INNER JOIN dbo.Lookup l ON r.runsheetType = l.Id \n" +
                               "WHERE r.runsheetNumber = '" + clvar.ArrivalID + "'\n";

            DataTable dt = new DataTable();

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
    }
}