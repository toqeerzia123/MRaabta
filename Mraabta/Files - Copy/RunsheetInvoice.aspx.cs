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
    public partial class RunsheetInvoice : System.Web.UI.Page
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

                Boolean flag = false;
                html.Append(HeaderTable(clvar));

                string sqlString = "";
                html.Append(TableStart());
                int count_ = dt.Rows.Count - 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 20 == 0 && i > 0)
                    {
                        html.Append("</table>");
                        html.Append(HeaderTable(clvar));
                        html.Append(TableStart());
                    }
                    if (i + 1 == dt.Rows.Count && (i + 1) % 2 != 0)
                    {

                        sqlString = "<tr style=\"height: 93px;\">\n" +
                    "                <td style=\"width: 2%; vertical-align: top; position: relative; left: 4%;\">\n" +
                    "                    " + (i + 1).ToString() +
                    "                </td>\n" +

                   "                <td style=\"width: 18%; vertical-align: top; position: relative; left: 5%;\">\n" +
                    "                    " + dt.Rows[i]["ConsignmentNumber"].ToString() + "<br>";
                        if (dt.Rows[i]["COD"].ToString() == "1")
                        {
                            sqlString += "COD Rs. " + dt.Rows[i]["CODAMOUNT"].ToString() + "<br>" +
                                "                    " + dt.Rows[i]["consignee"].ToString() + "<br><span style=\"width: 18%; position: relative; left: 0%;top: 17px; \">" +
                        "                    " + dt.Rows[i]["orign"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 20%; top: 17px;\">" +
                        "                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 35%;top: 17px; \">" +
                        "                    " + dt.Rows[i]["weight"].ToString() + "</span>" +
                        "                </td>\n";
                        }
                        else
                        {
                            sqlString += "" +
                        "                    " + dt.Rows[i]["consignee"].ToString() + "<br><span style=\"width: 18%; position: relative; left: 0%;top: 25px; \">" +
                        "                    " + dt.Rows[i]["orign"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 20%; top: 25px;\">" +
                        "                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 35%;top: 25px; \">" +
                        "                    " + dt.Rows[i]["weight"].ToString() + "</span> </td>";

                        }
                        sqlString += "                <td style=\"width: 2%; vertical-align: top;\">\n" +
                        "                    \n" +
                        "                </td>\n" +

                        "                <td style=\"width: 2%; vertical-align: top; text-align: center; text-align:Left;\">\n";
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
                        html.Append(sqlString);

                    }
                    else
                    {
                        sqlString = "<tr style=\"height: 93px;\">\n" +

                    "                <td style=\"width: 2%; vertical-align: top; position: relative; left: 4%;\">\n" +
                    "                    " + (i + 1).ToString() +
                    "                </td>\n" +

                    "                <td style=\"width: 18%; vertical-align: top; position: relative; left: 5%;\">\n" +
                    "                    " + dt.Rows[i]["ConsignmentNumber"].ToString() + "<br>";
                        if (dt.Rows[i]["COD"].ToString() == "1")
                        {
                            sqlString += "COD Rs. " + dt.Rows[i]["CODAMOUNT"].ToString() + "<br>" +
                                "                    " + dt.Rows[i]["consignee"].ToString() + "<br><span style=\"width: 18%; position: relative; left: 0%;top: 17px; \">" +
                        "                    " + dt.Rows[i]["orign"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 20%; top: 17px;\">" +
                        "                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 35%;top: 17px; \">" +
                        "                    " + dt.Rows[i]["weight"].ToString() + "</span>" +
                        "                </td>\n";
                        }
                        else
                        {
                            sqlString += "" +
                        "                    " + dt.Rows[i]["consignee"].ToString() + "<br><span style=\"width: 18%; position: relative; left: 0%;top: 25px; \">" +
                        "                    " + dt.Rows[i]["orign"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 20%; top: 25px;\">" +
                        "                    " + dt.Rows[i]["pieces"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 35%;top: 25px; \">" +
                        "                    " + dt.Rows[i]["weight"].ToString() + "</span> </td>";

                        }
                        sqlString += "                <td style=\"width: 2%; vertical-align: top;\">\n" +

                        "                </td>\n" +
                        "                <td style=\"width: 13%; vertical-align: top; text-align:Left;\">\n";
                        if (dt.Rows[i]["RiderName"].ToString().Trim() != "")
                        {
                            sqlString += "Given To Rider:</br>";
                        }
                        sqlString += "" +
                        dt.Rows[i]["RiderName"].ToString() +
                        "                </td>\n" +

                        "                <td style=\"width: 2%; vertical-align: top; text-align: center;\">\n" +
                        "                    " + (i + 2).ToString() +
                        "                </td>\n";
                        if (dt.Rows[i + 1]["COD"].ToString() == "1")
                        {
                            sqlString += "                <td style=\"width: 18%; vertical-align: top; position: relative; left: 1%;\">\n" +
                        "                    " + dt.Rows[i + 1]["ConsignmentNumber"].ToString() + "<br>" +
                        "               COD Rs. " + dt.Rows[i + 1]["CODAMOUNT"].ToString() + "<br>" +
                        "                    " + dt.Rows[i + 1]["consignee"].ToString() + "<br><span style=\"width: 18%; position: relative; left: 0%; top: 17px;\">" +
                        "                    " + dt.Rows[i + 1]["orign"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 20%; top: 17px;\">" +
                        "                    " + dt.Rows[i + 1]["pieces"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 35%;top: 17px; \">" +
                        "                    " + dt.Rows[i + 1]["weight"].ToString() + "</span>" +

                        "                </td>\n";
                        }
                        else
                        {
                            sqlString += "                <td style=\"width: 18%; vertical-align: top; position: relative; left: 1%;\">\n" +
                        "                    " + dt.Rows[i + 1]["ConsignmentNumber"].ToString() + "<br>" +
                        "                    " + dt.Rows[i + 1]["consignee"].ToString() + "<br><span style=\"width: 18%; position: relative; left: 0%; top: 25px;\">" +
                        "                    " + dt.Rows[i + 1]["orign"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 20%; top: 25px;\">" +
                        "                    " + dt.Rows[i + 1]["pieces"].ToString() + "</span><span style=\"width: 18%; position: relative; left: 35%;top: 25px; \">" +
                        "                    " + dt.Rows[i + 1]["weight"].ToString() + "</span>" +

                        "                </td>\n";
                        }
                        sqlString += "                <td style=\"width: 2%; vertical-align: top;\">\n" +

                        "                </td>\n" +
                        "                <td style=\"width: 13%; vertical-align: top; text-align:Left;\">\n";
                        if (dt.Rows[i + 1]["RiderName"].ToString().Trim() != "")
                        {
                            sqlString += "Given To Rider:</br>";
                        }
                        sqlString += "" +
                        dt.Rows[i + 1]["RiderName"].ToString() +
                        "                </td>\n";

                        sqlString += "            </tr>";

                        html.Append(sqlString);
                        i++;
                    }
                }

                ph1.Controls.Add(new Literal { Text = html.ToString() });

                HttpContext.Current.Response.Write("<script>window.print();</script>");
            }
            else
            {
                //  lbl_error.Text = "No Data Found";
            }
        }

        public string HeaderTable(Variable clvar)
        {
            string sqlString =
            "            <table style=\"width: 100%; font-family: Calibri; font-size: large; vertical-align: top; height: 70px; \">\n" +
            "                <tr>\n" +
            "                    <td style=\"width: 20%\">\n" +
            //    "                        <img src=\"../images/OCS_Transparent.png\" height=\"60px\" alt=\"logo\" width=\"157px\" />\n" +
            "                    </td>\n" +
            "                    <td style=\"width:60%; text-align:center;\">\n" +
            "                        <h2>\n" +
            "                            </h2> \n" +
            //   "                    </td><td style=\"width:20%; vertical-align:top; text-align:right\">" + DateTime.Now.ToString() + "</td>\n" +
            "                </tr>\n" +
            "            </table>\n";

            sqlString += "<table style=\"width: 100%; font-family: Calibri; font-size: 13px;\">\n" +
           "            <tr>\n" +

           "                <td style=\"text-align: left; width: 23%; position: absolute; top: 66px; left: 20%;\">\n" +
           "                    " + clvar.StartDate +
           "                </td>\n" +

           "                <td style=\"text-align: left; width: 23%; position: relative; left: 19%; top:15px;\">\n" +
           "                    " + clvar.RunsheetNumber +
           "                </td>\n" +
           "                <td style=\"text-align: left; width: 23%; position: relative; top: -12px; left: 22%;\">\n" +
           "                    " + clvar.BranchManager +
           "                </td>\n" +

           "                <td style=\"text-align: left; width: 23%; left: 23%; position: relative; top: -20px;\">\n" +
           "                    " + clvar.Route +
           "                </td>\n" +
           "                <td style=\"text-align: left; width: 23%; position: relative; left: 23%;top: -13px;\">\n" +
           "                    " + clvar.Code +
           "                </td>\n" +

           "                <td style=\"text-align: left; position: relative; width: 23%; left: -21%; top: 15px;\">\n" +
           "                    " + clvar.CourierName +
           "                </td>\n" +
           "                <td style=\"text-align: left; width: 23%; position: relative; left: -7%; top: 15px;\">\n" +
           "                    " + clvar.RiderCode +
           "                </td>\n" +

           "            </tr>\n" +

           "        </table>";

            return sqlString;
        }

        public string TableStart()
        {

            string sqlString = "<table style=\"width: 100%; font-family: Calibri; font-weight: normal; font-size: small; border-collapse: collapse; position: relative; top: 75px;\n" +
            "            border-collapse: collapse;\">\n" +
            "            <tr>\n" +
            "                <td\n" +
            "                    style=\"width: 1%; \n" +
            "                    \">\n" +
            "                    \n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; \n" +
            "                    \">\n" +
            "                    \n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; \n" +
            "                    \">\n" +
            "                    \n" +
            "                </td>\n" +
            "                <td style=\"width: 1%; \n" +
            "                    \">\n" +
            "                    \n" +
            "                </td>\n" +
            "                <td style=\"width: 22%; \n" +
            "                    \">\n" +
            "                    \n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; \n" +
            "                    \">\n" +
            "                    \n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        public string DataRow(DataRow dr, Boolean flag)
        {
            string sqlString = "";
            if (flag)
            {
                sqlString = "<tr>\n" +
            "                <td style=\" width: 18%\">\n" +
            "                    " + dr["ConsignmentNumber"].ToString() + "\n" +
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

            sqlString = "selecT temp.consignmentNumber,\n" +
            "       temp.pieces,\n" +
            "       temp.weight,\n" +
            "       temp.orign,\n" +
            "       temp.consignee,\n" +
            "       temp.cod,\n" +
            "       temp.SortOrder,\n" +
            "       SUM(temp.codAmount) CODAMOUNT, temp.Receiver_CNIC, temp.RiderName\n" +
            "  from (select c.consignmentNumber,\n" +
            "               c.pieces,\n" +
            "               c.weight,\n" +
            "               b.sname orign,\n" +
            "               c.consignee,\n" +
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
            "        select c.consignmentNumber,\n" +
            "               c.pieces,\n" +
            "               c.weight,\n" +
            "               b.sname orign,\n" +
            "               c.consignee,\n" +
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
            "          temp.consignee,\n" +
            "          temp.cod,\n" +
            "          temp.SortOrder, temp.Receiver_CNIC,\n" +
            "          temp.RiderName\n" +
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
            "       CONVERT(VARCHAR(10), R.RUNSHEETDATE, 105) RUNSHEETDATE\n" +
            "\n" +
            "  FROM RUNSHEET R\n" +
            "\n" +
            " INNER JOIN RIDERRUNSHEET RR\n" +
            "    ON R.RUNSHEETNUMBER = RR.RUNSHEETNUMBER\n" +
            "   AND R.CREATEDBY = RR.CREATEDBY\n" +
            "\n" +
            " INNER JOIN RIDERS RS\n" +
            "    ON RS.BRANCHID = R.BRANCHCODE\n" +
            "   AND RS.ROUTECODE = R.ROUTECODE\n" +
            "   AND RS.RIDERCODE = RR.RIDERCODE\n" +
            "  -- AND RS.EXPRESSCENTERID = RR.EXPIDTEMP\n" +
            "\n" +
            " INNER JOIN BRANCHES B\n" +
            "    ON B.BRANCHCODE = R.BRANCHCODE\n" +
            "  LEFT OUTER JOIN ROUTES RC\n" +
            "    ON RC.CITYID = B.CITYID\n" +
            "   AND RC.ROUTECODE = R.ROUTECODE\n" +
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
    }
}