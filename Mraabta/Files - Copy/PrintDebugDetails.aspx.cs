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
    public partial class PrintDebugDetails : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        int page = 1;
        int totalPages = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            PrintDemanifestDetails();
        }
        protected void PrintDemanifestDetails()
        {
            try
            {
                string BagID = Request.QueryString["id"].ToString();

                DataTable dt = GetDeManifestPrintDetails(BagID);

                StringBuilder html = new StringBuilder();
                totalPages = (dt.Rows.Count / 35) + ((dt.Rows.Count % 35 != 0) ? 1 : 0);
                html.Append(HeaderTable(dt));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 35 == 0 && i != 0)
                    {
                        page++;
                        html.Append("</table>");
                        html.Append(HeaderTable(dt));
                    }

                    html.Append(DataRow(dt.Rows[i]));
                }



                ph1.Controls.Add(new Literal { Text = html.ToString() });

            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }

        }

        protected DataTable GetDeManifestPrintDetails(string BagID)
        {



            string sqlString = "select dm.manifestNumber,dm.bagNumber bagNumber,\n" +
            "       convert (VARCHAR(10),m.date,102) Date,\n" +
            "       org.name          ManifestOrigin,\n" +
            "       dest.name         ManifestDest\n" +
            "  from MnP_DebagManifest dm\n" +
            " inner join Mnp_Manifest m\n" +
            "    on m.manifestNumber = dm.manifestNumber\n" +
            " inner join branches org\n" +
            "    on m.origin = org.branchCode\n" +
            " inner join Branches dest\n" +
            "    on m.destination = dest.branchCode\n" +
            " where dm.DebagID = '" + BagID + "'";



            sqlString = "select dm.bagNumber bagNumber,\n" +
            "       dm.manifestNumber,\n" +
            "       '' OutpieceNumber,\n" +
            "       convert(VARCHAR(10), m.date, 102) Date,\n" +
            "       org.sname ManifestOrigin,\n" +
            "       dest.sname ManifestDest\n" +
            "  from MnP_DebagManifest dm\n" +
            " inner join Mnp_Manifest m\n" +
            "    on m.manifestNumber = dm.manifestNumber\n" +
            " inner join branches org\n" +
            "    on m.origin = org.branchCode\n" +
            " inner join Branches dest\n" +
            "    on m.destination = dest.branchCode\n" +
            " where dm.DebagID = '" + BagID + "'\n" +
            " UNION ALL\n" +
            "select dm.bagNumber bagNumber,\n" +
            "       '' manifestNumber,\n" +
            "        OutpieceNumber,\n" +
            "       convert(VARCHAR(10), m.createdOn, 102) Date,\n" +
            "       org.sname ManifestOrigin,\n" +
            "       dest.sname ManifestDest\n" +
            "  from MnP_DebagOutPieces dm\n" +
            " inner join consignment m\n" +
            "    on m.consignmentNumber = dm.outpieceNumber\n" +
            " inner join branches org\n" +
            "    on m.orgin = org.branchCode\n" +
            " inner join Branches dest\n" +
            "    on m.destination = dest.branchCode\n" +
            " where dm.DebagID = '" + BagID + "'";


            sqlString = "select dm.bagNumber bagNumber,\n" +
            "       dm.manifestNumber,\n" +
            "       '' OutpieceNumber,\n" +
            "       convert(VARCHAR(10), m.date, 102) Date,\n" +
            "       org.sname ManifestOrigin,\n" +
            "       dest.sname ManifestDest,\n" +
            "       '' Weight,\n" +
            "       '' Pieces,\n" +
            "       dm.reason Remarks\n" +
            "  from MnP_DebagManifest dm\n" +
            " inner join Mnp_Manifest m\n" +
            "    on m.manifestNumber = dm.manifestNumber\n" +
            " inner join branches org\n" +
            "    on m.origin = org.branchCode\n" +
            " inner join Branches dest\n" +
            "    on m.destination = dest.branchCode\n" +
            " where dm.DebagID = '" + BagID + "'\n" +
            "UNION ALL\n" +
            "select dm.bagNumber bagNumber,\n" +
            "       '' manifestNumber,\n" +
            "       OutpieceNumber,\n" +
            "       convert(VARCHAR(10), m.createdOn, 102) Date,\n" +
            "       org.sname ManifestOrigin,\n" +
            "       dest.sname ManifestDest,\n" +
            "       CAST(dm.cnWeight as Varchar) weight,\n" +
            "       CAST(dm.cnPieces as varchar) pieces,\n" +
            "       dm.reason Remarks\n" +
            "  from MnP_DebagOutPieces dm\n" +
            " inner join consignment m\n" +
            "    on m.consignmentNumber = dm.outpieceNumber\n" +
            " inner join branches org\n" +
            "    on m.orgin = org.branchCode\n" +
            " inner join Branches dest\n" +
            "    on m.destination = dest.branchCode\n" +
            " where dm.DebagID = '" + BagID + "'";


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
            "                    <b>De-Bagging Detail</b>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 12%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    <b>User</b>\n" +
            "                </td>\n" +
            "                <td colspan=\"2\" style=\"width: 25%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    " + HttpContext.Current.Session["U_NAME"].ToString() + "\n" +
            "                </td>\n" +
            "                <td colspan=\"2\" style=\"width: 20%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    <b>Print Date Time</b>\n" +
            "                </td>\n" +
            "                <td colspan=\"3\" style=\"width: 43%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 12%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Bag #</b>\n" +
            "                </td>\n" +
            "                <td colspan=\"2\" style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + dt.Rows[0]["bagNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td colspan=\"2\" style=\"width: 20%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Page #</b>\n" +
            "                </td>\n" +
            "                <td colspan=\"3\" style=\"width: 43%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + page + "/" + totalPages + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +
            "            <tr>\n" +
            "                <td style=\"width: 12%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Manifest #</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Outpiece #</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Date</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
             "                <td style=\"width: 10%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Weight</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Pieces</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Remarks</b>\n" +
            "                </td>\n" +
            "            </tr>";

            return sqlString;
        }

        protected string DataRow(DataRow dr)
        {

            string sqlString = "<tr>\n" +
            "                <td style=\"width: 12%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["manifestNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["OutPieceNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Date"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["ManifestOrigin"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 10%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["ManifestDest"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Weight"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 7.5%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["Pieces"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 28%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
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