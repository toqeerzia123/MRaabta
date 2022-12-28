using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Text;

namespace MRaabta.Files
{
    public partial class Arrival_Print_Speedy : System.Web.UI.Page
    {
        cl_Encryption enc = new cl_Encryption();
        Variable clvar = new Variable();
        CommonFunction fun = new CommonFunction();
        int count = 0;
        int page = 0;
        string totalWeight = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
            {
                PrintArrival();
            }
        }

        public void PrintArrival()
        {
            StringBuilder html = new StringBuilder();
            clvar.ArrivalID = Int64.Parse((Request.QueryString["XCode"].ToString()));
            DataTable head = GetArrivalPrintHead(clvar);
            clvar.RiderCode = head.Rows[0]["RiderCode"].ToString();
            clvar.RiderName = head.Rows[0]["RiderName"].ToString();
            count = int.Parse(head.Rows[0]["ConsignmentCount"].ToString());
            totalWeight = head.Rows[0]["Weight"].ToString();
            DataTable dt = GetArrivalPrintDetail(clvar);
            Boolean flag = false;
            page = 1;
            clvar.EndDate = head.Rows[0]["Createdon"].ToString();
            html.Append(HeaderTable(clvar));

            string sqlString = "";
            html.Append(TableStart());
            int count_ = dt.Rows.Count - 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % 80 == 0 && i > 0)
                {
                    page++;
                    html.Append("</table>");
                    html.Append(HeaderTable(clvar));
                    html.Append(TableStart());
                }
                if (i + 1 == dt.Rows.Count && (i + 1) % 2 != 0)
                {

                    sqlString = "<tr>\n" +
                "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
                "                    " + dt.Rows[i]["ConsignmentNumber"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 16%\">\n" +
                "                    " + dt.Rows[i]["ServiceTypeName"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i]["Weight"].ToString() + "\n" +
                "                </td>\n";
                    if (Request.QueryString["pg"] != null)
                        if (Request.QueryString["pg"].ToString() == "qaszdfg")
                            sqlString += " <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i]["BookingWeight"].ToString() + "\n" +
                "                </td>\n";

                    sqlString += "<td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i]["Pieces"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
                "                    \n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 16%\">\n" +
                "                    \n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    \n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    \n" +
                "                </td>\n" +
                "            </tr>";
                    html.Append(sqlString);

                }
                else
                {
                    sqlString = "<tr>\n" +
                "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
                "                    " + dt.Rows[i]["ConsignmentNumber"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 16%\">\n" +
                "                    " + dt.Rows[i]["ServiceTypeName"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i]["Weight"].ToString() + "\n" +
                "                </td>\n";
                    if (Request.QueryString["pg"] != null)
                        if (Request.QueryString["pg"].ToString() == "qaszdfg")
                            sqlString += " <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i]["BookingWeight"].ToString() + "\n" +
                "                </td>\n";

                    sqlString += "<td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i]["Pieces"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
                "                    " + dt.Rows[i + 1]["ConsignmentNumber"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 16%\">\n" +
                "                    " + dt.Rows[i + 1]["ServiceTypeName"].ToString() + "\n" +
                "                </td>\n" +
                "                <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i + 1]["Weight"].ToString() + "\n" +
                "                </td>\n";
                    if (Request.QueryString["pg"] != null)
                        if (Request.QueryString["pg"].ToString() == "qaszdfg")
                            sqlString += " <td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i]["BookingWeight"].ToString() + "\n" +
                "                </td>\n";

                    sqlString += "<td style=\"border: 1px solid #000000; width: 8%\">\n" +
                "                    " + dt.Rows[i + 1]["Pieces"].ToString() + "\n" +
                "                </td>\n" +
                "            </tr>";
                    html.Append(sqlString);
                    i++;
                }



            }

            ph1.Controls.Add(new Literal { Text = html.ToString() });

            HttpContext.Current.Response.Write("<script>window.print();</script>");
        }

        public string HeaderTable(Variable clvar)
        {
            string pageBreak = "";
            if (page > 1)
            {
                pageBreak = "page-break-before: always;";
            }
            string sqlString =
            "            <table style=\"width: 100%; text-align: center; font-family: Calibri; font-size: small; " + pageBreak + "\">\n" +
            "                <tr>\n" +
            "                    <td style=\"width: 20%\">\n" +
            "                        <img src=\"../images/OCS_Transparent.png\" height=\"60px\" alt=\"logo\" width=\"157px\" />\n" +
            "                    </td>\n" +
            "                    <td style=\"width:60%; text-align:center;\">\n" +
            "                        <h2>\n" +
            "                            CONSIGNMENT RECEIVE REPORT</h2> \n" +
            "                    </td><td style=\"width:20%; vertical-align:top; text-align:right\">" + DateTime.Now.ToString();
            if (Request.QueryString["pg"] != null)
                if (Request.QueryString["pg"].ToString() == "qaszdfg")
                    sqlString += " <br/>Auto Weight";
            sqlString += "</td>\n" +
            "                </tr>\n" +
            "            </table>\n";

            sqlString += "<table style=\"width: 100%; font-family: Calibri; font-size: large;\">\n" +
           "            <tr>\n" +
           "                <td style=\"width: 30%; text-align: Left;\">\n" +
           "                    <b>Arrival ID:</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 20%; text-align: left;\">\n" +
           clvar.ArrivalID.ToString() +
           "                </td>\n" +
           "                <td style=\"width: 20%; text-align: Left;\">\n" +
           "                    <b>Arrival Date</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 30%; text-align: left;\">\n" +
           clvar.EndDate +
           "                </td>\n" +
           "            </tr>\n" +
           "            <tr>\n" +
           "                <td style=\"width: 30%; text-align: Left;\">\n" +
           "                    <b>Rider Code:</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 20%; text-align: left;\">\n" +
           clvar.RiderCode +
           "                </td>\n" +
           "                <td style=\"width: 20%; text-align: Left;\">\n" +
           "                    <b>Rider Name:</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 30%; text-align: left;\">\n" +
           clvar.RiderName +
           "                </td>\n" +
           "            </tr>\n" +
           "            <tr>\n" +
           "                <td style=\"width: 30%; text-align: Left;\">\n" +
           "                    <b>Consignment Count:</b>\n" +
           "                </td>\n" +
           "                <td style=\"width: 20%; text-align: left;\">\n" +
           count +
           "                </td>\n" +
           "                <td style=\"width: 20%; text-align: Left;\">\n" +
           "                    <b>Total Weight</b>" +
           "                </td>\n" +
           "                <td style=\"width: 30%; text-align: left;\">\n" +
           totalWeight +
           "                </td>\n" +
           "            </tr>\n" +
           "        </table>";

            return sqlString;
        }

        public string TableStart()
        {

            string sqlString = "<table style=\"border: 3px solid #000000; width: 100%; font-family: Calibri; font-size: small;\n" +
            "            border-collapse: collapse;\">\n" +
            "            <tr>\n" +
            "                <td\n" +
            "                    style=\"width: 18%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Consignment #.</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 16%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Service Type</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Weight</b>\n" +
            "                </td>\n";
            if (Request.QueryString["pg"] != null)
                if (Request.QueryString["pg"].ToString() == "qaszdfg")
                    sqlString += "                <td style=\"width: 8%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Booking Weight</b>\n" +
            "                </td>\n";

            sqlString += "   <td style=\"width: 8%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Pieces</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 18%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Consignment #.</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 16%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Service Type</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 8%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Weight</b>\n" +
            "                </td>\n";
            if (Request.QueryString["pg"] != null)
                if (Request.QueryString["pg"].ToString() == "qaszdfg")
                    sqlString += "                <td style=\"width: 8%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Booking Weight</b>\n" +
            "                </td>\n";

            sqlString += "<td style=\"width: 8%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\n" +
            "                    border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Pieces</b>\n" +
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
            "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
            "                    " + dr["ConsignmentNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "            </tr>";
            }
            else
            {
                sqlString = "<tr>\n" +
            "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
            "                    " + dr[""] + "\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 18%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 26%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "                <td style=\"border: 1px solid #000000; width: 6%\">\n" +
            "                    0000\n" +
            "                </td>\n" +
            "            </tr>";
            }


            return sqlString;

        }



        public DataTable GetArrivalPrintHead(Variable clvar)
        {

            string sqlString = "select a.RiderCode,\n" +
            "       a.OriginExpressCenterCode, a.weight,\n" +
            "       r.firstName + ' ' + r.lastName RiderName,\n" +
            "       COUNT(ad.consignmentNumber) ConsignmentCount, FORMAT(a.createdon, 'yyyy-MM-dd hh:mm tt') createdon\n" +
            "  from ArrivalScan a\n" +
            "\n" +
            " inner join ArrivalScan_Detail ad\n" +
            "    on ad.ArrivalID = a.Id\n" +
            " left outer join Riders r\n" +
            "    on a.OriginExpressCenterCode = r.expressCenterId\n" +
            "   and a.RiderCode = r.riderCode\n" +
            " where a.Id = '" + clvar.ArrivalID + "'\n" +
            "\n" +
            " group by a.RiderCode, a.OriginExpressCenterCode, r.firstName, r.lastName, a.weight,a.createdon ";
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
        public DataTable GetArrivalPrintDetail(Variable clvar)
        {

            string sqlString = "select ad.consignmentNumber, ad.serviceType serviceTypeName, ad.cnweight weight, ad.cnpieces Pieces, (select bookingweight from Consignment c where c.consignmentNumber = ad.consignmentNumber) as BookingWeight\n" +
            "  from ArrivalScan a\n" +
            " inner join ArrivalScan_Detail ad\n" +
            "    on a.Id = ad.ArrivalID\n" +
            " where ad.ArrivalID = '" + clvar.ArrivalID + "' order by 1";
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