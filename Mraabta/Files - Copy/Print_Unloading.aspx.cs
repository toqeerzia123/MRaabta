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
    public partial class Print_Unloading : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        int page = 1;
        int totalPages = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            PrintLoad();
        }
        protected void PrintLoad()
        {
            try
            {
                string loadnumber = Request.QueryString["Xcode"].ToString();

                DataTable dt = GetDePrintHeader(loadnumber);
                DataTable dt_detail = GetDePrintDetails(loadnumber);

                StringBuilder html = new StringBuilder();
                totalPages = (dt_detail.Rows.Count / 25) + ((dt_detail.Rows.Count % 25 != 0) ? 1 : 0);
                html.Append(HeaderTable(dt));

                for (int i = 0; i < dt_detail.Rows.Count; i++)
                {
                    if (i % 25 == 0 && i != 0)
                    {
                        page++;
                        html.Append("</table>");
                        html.Append("<table style=\"font-family: Calibri;\"><tr><td style='text-align: left;'><br/><b>Document Created By : " + HttpContext.Current.Session["U_NAME"].ToString() + "</b></td></tr>");
                        html.Append("<tr><td style='text-align: left;'><br/><b>Signature: __________________________</b></td></tr></table>");
                        html.Append(HeaderTable(dt));
                        html.Append(DataRow(dt_detail.Rows[i]));

                    }
                    else
                    {
                        html.Append(DataRow(dt_detail.Rows[i]));
                    }

                }
                html.Append("</table>");
                html.Append("<table style=\"font-family: Calibri;\"><tr><td style='text-align: left;'><br/><b>Document Created By : " + HttpContext.Current.Session["U_NAME"].ToString() + "</b></td></tr>");
                html.Append("<tr><td style='text-align: left;'><br/><b>Signature: __________________________</b></td></tr></table>");


                ph1.Controls.Add(new Literal { Text = html.ToString() });

            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }

        }

        protected DataTable GetDePrintHeader(string loadnumer)
        {



            string sqlString = "select l.id LoadingID,\n" +
            "       ul.ID UnloadingID,\n" +
            "       lu.AttributeDesc TransportType,\n" +
            "       CONVERT(varchar,l.date, 106) date,\n" +
            "       Case\n" +
            "         when l.vehicleID != '103' then\n" +
            "          v.MakeModel + ' (' + v.Description + ')'\n" +
            "         else\n" +
            "          '(Rented)' + l.VehicleRegNo\n" +
            "       end VehicleName,\n" +
            "       l.courierName,\n" +
            "       b1.name OrgName,\n" +
            "       b2.name DestName,\n" +
            "       l.description,\n" +
            "       l.flightNo,\n" +
            "       l.sealno,\n" +
            "       l.departureflightdate,\n" +
            "       (select COUNT(lb.BagNumber)\n" +
            "          from mnp_UnloadingBag lb\n" +
            "         where lb.unloadingID = '" + loadnumer + "') TotalBagCount,\n" +
            "       (select COUNT(lc.ConsignmentNumber)\n" +
            "          from mnp_UnloadingConsignment lc\n" +
            "         where lc.UnLoadingID = '" + loadnumer + "') TotalCNCount\n" +
            "  from mnp_Loading l\n" +
            "  left outer join rvdbo.Lookup lu\n" +
            "    on lu.id = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.Vehicle v\n" +
            "    on v.VehicleCode = l.vehicleId\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = l.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = l.destination\n" +
            " inner join mnp_unloading ul\n" +
            "    on ul.RefLoadingID = l.id\n" +
            "\n" +
            " where ul.ID = '" + loadnumer + "'";





            sqlString = "select LoadingID = isnull(STUFF((SELECT ',' + cast(md.loadingID as nvarchar) FROM MnP_UnloadingRef md WHERE l.id = md.unloadingID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''),l.RefLoadingID) , \n" +
            "       l.ID UnloadingID,\n" +
            "       lu.AttributeDesc TransportType,\n" +
            "       CONVERT(varchar, ul.date, 106) date,\n" +
            "       Case\n" +
            "         when l.vehicleID != '103' then\n" +
            "          v.MakeModel + ' (' + v.Description + ')'\n" +
            "         else\n" +
            "          '(Rented)' + l.VehicleRegNo\n" +
            "       end VehicleName,\n" +
            "       l.courierName,\n" +
            "       b1.name OrgName,\n" +
            "       b2.name DestName,\n" +
            "       l.description,\n" +
            "       l.flightNo,\n" +
            "       l.sealno,\n" +
            "       l.departureflightdate,\n" +
            "       (select COUNT(lb.BagNumber)\n" +
            "          from mnp_UnloadingBag lb\n" +
            "         where lb.unloadingID = '" + loadnumer + "') TotalBagCount,\n" +
            "       (select COUNT(lc.ConsignmentNumber)\n" +
            "          from mnp_UnloadingConsignment lc\n" +
            "         where lc.UnLoadingID = '" + loadnumer + "') TotalCNCount\n" +
            "  from mnp_unLoading l\n" +
            "  left outer join rvdbo.Lookup lu\n" +
            "    on lu.id = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.Vehicle v\n" +
            "    on v.VehicleCode = l.vehicleId\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = l.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = l.destination\n" +
            "  left outer join mnp_loading ul\n" +
            "    on ul.id = l.RefLoadingID\n" +
            "\n" +
            " where l.ID = '" + loadnumer + "'";


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return dt;
        }
        protected DataTable GetDePrintDetails(string loadnumer)
        {




            string sqlString = "select dest.SNAME Destination,\n" +
              "       org.SNAME Origin,\n" +
              "       lb.BagNumber,\n" +
              "       '' OutPieceNumber,\n" +
              "       lu.AttributeDesc STATUS,\n" +
              "       lb.bagWeight Weight,\n" +
              "       '' Pieces,\n" +
              "       lb.bagRemarks Remarks\n" +
              "  from mnp_unloading l\n" +
              " inner join mnp_UnloadingBag lb\n" +
              "    on lb.UnloadingID = l.ID\n" +
              " inner join branches dest\n" +
              "    on dest.branchCode = lb.BagDestination\n" +
              //"  inner join Bag b\n" +
              //"    on lb.BagNumber = b.bagNumber\n" +
              "  inner join branches org\n" +
              "    on lb.bagorigin = org.branchCode\n" +
              " inner join rvdbo.Lookup lu\n" +
              "    on lu.Id = lb.unloadingStateID\n" +
              " where l.ID = '" + loadnumer + "'\n" +
              "\n" +
              "union all\n" +
              "\n" +
              "select dest.SNAME Destination,\n" +
              "       org.SNAME Origin,\n" +
              "       '' BagNumber,\n" +
              "       lb.consignmentNumber OutPieceNumber,\n" +
              "       lu.AttributeDesc STATUS,\n" +
              "       lb.cnweight Weight,\n" +
              "       lb.cnPieces Pieces,\n" +
              "       lb.cnRemarks Remarks\n" +
              "  from mnp_unloading l\n" +
              " inner join mnp_UnloadingConsignment lb\n" +
              "    on lb.UnloadingID = l.ID\n" +
              " inner join branches dest\n" +
              "    on dest.branchCode = lb.CNDestination\n" +
              " --inner join Consignment b\n" +
              //"\n" +
              " --on lb.ConsignmentNumber = b.consignmentNumber\n" +
              //"\n" +
              "  inner join branches org\n" +
              "    on lb.cnorigin = org.branchCode\n" +
              " inner join rvdbo.Lookup lu\n" +
              "    on lu.Id = lb.unloadingStateID\n" +
              " where l.ID = '" + loadnumer + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlString, con);
                cmd.CommandTimeout = 3000;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return dt;
        }
        //HttpContext.Current.Session["U_NAME"].ToString()
        //DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") + 
        protected string HeaderTable(DataTable dt)
        {
            string pageBreak = "";
            if (page > 1)
            {
                pageBreak = "page-break-before: always;";
            }


            string sqlString = "<table style=\"width: 100%; border: 2px Solid Black; border-bottom: 1px Solid Black; " + pageBreak + "\n" +
            "            border-collapse: collapse; font-family: Calibri; font-size: small;\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 100%; font-size: x-large; font-variant: small-caps; text-align: center;\n" +
            "                    border-bottom: 2px Solid Black\" colspan=\"8\">\n" +
            "                    <b>Un-Loading Summary</b>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 15%; text-align: left; border: 1px Solid Black; border-bottom: 2px Solid Black\" colspan=\"2\">\n" +
            "                    <b>Un-Loading VID</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: left; border: 1px Solid Black; border-bottom: 2px Solid Black\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["UnloadingID"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: left; border: 1px Solid Black; border-bottom: 2px Solid Black\" colspan=\"2\">\n" +
            "                    <b>Vehicle</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: left; border: 1px Solid Black; border-bottom: 2px Solid Black\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["VehicleName"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 15%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Loading VID</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["LoadingID"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Transportation Type</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["TransportType"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +
                "            <tr>\n" +
            "                <td style=\"width: 15%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Vehicle Seal Number</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["sealno"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Courier Name</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["courierName"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +
                   "            <tr>\n" +
            "                <td style=\"width: 15%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Flight Number</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["flightNo"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Print Date & Time</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + DateTime.Now.ToString("dd/MM/yyy hh:mm tt") + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +
                      "            <tr>\n" +
            "                <td style=\"width: 15%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["OrgName"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["DestName"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +

                           "            <tr>\n" +
            "                <td  style=\"width: 15%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>User</b>\n" +
            "                </td>\n" +
            "                <td   style=\"width: 30%; text-align: left; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + HttpContext.Current.Session["U_NAME"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +



            "            <tr>\n" +
            "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Date</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Sub Total Bag(Count)</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 27%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Sub Total Outpiece (Count)</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\" colspan=\"2\">\n" +
            "                    <b>Page :" + page + "/" + totalPages + "</b>\n" +
            "                </td>\n" +
            "            </tr>" +

             "            <tr>\n" +
             "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["date"].ToString() + "\n" +
            "                </td>\n" +
              "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["TotalBagCount"].ToString() + "\n" +
            "                </td>\n" +
              "                <td style=\"width: 27%; text-align: center; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    " + dt.Rows[0]["TotalCNCount"].ToString() + "\n" +
            "                </td>\n" +
              "                <td style=\"width: 28%; text-align: center; border: 1px Solid Black;\" colspan=\"2\">\n" +
            "                    \n" +
            "                </td>\n" +
            "            </tr>" +
              "            <tr>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>ORG</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>DEST</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Bag#</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Outpiece#</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>STATUS</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 7%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Weight</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 7%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Pieces</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 21%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Remarks</b>\n" +
            "                </td>\n" +
            "            </tr>";

            return sqlString;
        }

        protected string DataRow(DataRow dr)
        {

            string sqlString = "<tr>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Origin"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Destination"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
             "                    " + dr["BagNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["OutPieceNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["STATUS"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 7%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Weight"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 7%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Pieces"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 21%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Remarks"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        protected void Alert(string MEssage)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + MEssage + "')", true);
            //ErrorID.Text = MEssage;
        }
    }
}