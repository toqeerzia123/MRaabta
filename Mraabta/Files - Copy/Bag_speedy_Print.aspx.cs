using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class Bag_speedy_Print : System.Web.UI.Page
    {

        Cl_Variables clvar = new Cl_Variables();
        CommonFunction fun = new CommonFunction();
        cl_Encryption enc = new cl_Encryption();
        int count = 0;
        Boolean flag = false;
        int page = 1;

        string createdBy = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
            {
                PrintBag();
            }

        }

        protected void PrintBag()
        {
            clvar.BagNumber = Request.QueryString["XCode"].ToString();


            DataTable header = fun.GetBagPrintReportHeader(clvar);
            DataTable dt = GetBagPrintReportDetail(clvar);
            DataTable outPieces = GetBagOutPieces(clvar);

            StringBuilder html = new StringBuilder();
            //html.Append(HeaderTable(clvar));


            if (header.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                createdBy = "";// header.Rows[0]["CreatedBy"].ToString();
                clvar.BranchName = header.Rows[0]["Branch"].ToString();
                clvar.originBrName = header.Rows[0]["Origin"].ToString();
                clvar.Destination = header.Rows[0]["Destination"].ToString();
                clvar.deliveryDate = DateTime.Parse(header.Rows[0]["Date"].ToString());
                clvar.BagNumber = header.Rows[0]["BagNumber"].ToString();
                clvar.SealNumber = header.Rows[0]["SEALNO"].ToString();
                clvar.Weight = float.Parse(header.Rows[0]["totalWeight"].ToString());

                html.Append(HeaderTable(clvar));
                html.Append(TableStart());

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 30 == 0 && i > 0)
                    {
                        count++;
                        html.Append("</table>");
                        html.Append(FooterRow());
                        html.Append(HeaderTable(clvar));
                        html.Append(TableStart());
                    }
                    if (((i + 1) % 30 == 0 && i > 0) || i == dt.Rows.Count - 1)
                    {
                        flag = true;
                    }
                    html.Append(DataRow(dt.Rows[i]));
                }
                html.Append("</table>");
                html.Append("<br />");
                int rowCount = dt.Rows.Count % 30;
                flag = false;

                if (outPieces.Rows.Count > 0)
                {
                    if (rowCount + 5 > 30 || rowCount == 0)
                    {

                        rowCount = 0;
                        //html.Append("</table>");
                        html.Append(HeaderTable(clvar));
                        html.Append(HeaderRowForOutPiece());
                    }
                    else
                    {
                        html.Append(HeaderRowForOutPiece());
                    }
                    for (int i = 0; i < outPieces.Rows.Count; i++)
                    {
                        flag = false;
                        if (rowCount == 30)
                        {
                            rowCount = 0;
                            html.Append("</table>");
                            html.Append(HeaderTable(clvar));
                            html.Append(HeaderRowForOutPiece());
                        }
                        if (rowCount == 29 || i == outPieces.Rows.Count - 1)
                        {
                            flag = true;
                        }

                        html.Append(DataRowOutPiece(outPieces.Rows[i]));
                        rowCount++;
                    }
                    html.Append("</table>");
                }

                html.Append(FooterRow());
                ph1.Controls.Add(new Literal { Text = html.ToString() });
                HttpContext.Current.Response.Write("<script>window.print();</script>");
            }
            else if (outPieces.Rows.Count > 0)
            {
                createdBy = header.Rows[0]["CreatedBy"].ToString();
                clvar.BranchName = header.Rows[0]["Branch"].ToString();
                clvar.originBrName = header.Rows[0]["Origin"].ToString();
                clvar.Destination = header.Rows[0]["Destination"].ToString();
                clvar.deliveryDate = DateTime.Parse(header.Rows[0]["Date"].ToString());
                clvar.BagNumber = header.Rows[0]["BagNumber"].ToString();
                clvar.SealNumber = header.Rows[0]["SEALNO"].ToString();
                clvar.Weight = float.Parse(header.Rows[0]["totalWeight"].ToString());
                count = 1;
                int rowCount = 0;
                html.Append(HeaderTable(clvar));
                html.Append(HeaderRowForOutPiece());

                for (int i = 0; i < outPieces.Rows.Count; i++)
                {
                    flag = false;
                    if (rowCount == 30)
                    {
                        count++;
                        rowCount = 0;
                        html.Append("</table>");
                        html.Append(HeaderTable(clvar));
                        html.Append(HeaderRowForOutPiece());
                    }
                    if (rowCount == 29 || i == outPieces.Rows.Count - 1)
                    {
                        flag = true;
                    }

                    html.Append(DataRowOutPiece(outPieces.Rows[i]));
                    rowCount++;
                }
                html.Append("</table>");
                html.Append(FooterRow());
                ph1.Controls.Add(new Literal { Text = html.ToString() });
                HttpContext.Current.Response.Write("<script>window.print();</script>");
            }
            else
            {
                ph1.Controls.Add(new Literal { Text = "No Record Found" });
            }
        }




        protected string HeaderTable(Cl_Variables clvar)
        {
            string pageBreak = "";
            if (count > 1) pageBreak = " page-break-before:always;";
            string sqlString = "\n" +
            "            <table style=\"width: 100%; text-align: center; font-family: Calibri; font-size: small; " + pageBreak + "\">\n" +
            "                <tr>\n" +
            "                    <td style=\"width: 20%\">\n" +
            "                        <img src=\"../images/OCS_Transparent.png\" height=\"60px\" alt=\"logo\" width=\"157px\" />\n" +
            "                    </td>\n" +
            "                    <td style=\"width:60%; text-align:center;\">\n" +
            "                        <h2>\n" +
            "                            BAGGING REPORT</h2> \n" +
            "                    </td><td style=\"width:20%; vertical-align:top; text-align:right\">" + DateTime.Now.ToString() + "</td>\n" +
            "                </tr>\n" +
            "            </table>\n" +
            "            <table style=\"border: thin solid #003366; width: 100%; font-family: Calibri; font-size: small;\n" +
            "                border-collapse: collapse;\">\n" +
            "                <tr>\n" +
            "                    <td colspan=\"5\" style=\"text-align: center; font-size: medium; border: 2px Solid Black;\">\n" +
            "                        <b>BAG INFO</b>\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "                <tr>\n" +
            "                    <td style=\"border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\n" +
            "                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid;\">\n" +
            "                        <b>No:</b> " + clvar.BagNumber + "\n" +
            "                    </td>\n" +
            "                    <td style=\"border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\n" +
            "                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;\">\n" +
            "                        <b>Origin:</b> " + clvar.originBrName + "\n" +
            "                    </td>\n" +
            "                    <td style=\"border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\n" +
            "                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;\">\n" +
            "                        <b>Destination:</b> " + clvar.Destination + "\n" +
            "                    </td>\n" +
            "                    <td style=\"border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\n" +
            "                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;\">\n" +
            "                        <b>Weight:</b> " + clvar.Weight.ToString() + " Kg \n" +
            "                    </td>\n" +
            "                    <td style=\"border-right-style: solid; border-right-width: 2px; border-right-color: #000000;\n" +
            "                        border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;\">\n" +
            "                        <b>Date:</b> " + clvar.deliveryDate.ToString("dd/MM/yyyy") + "\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "                <tr>\n" +
            "                    <td style=\"border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\n" +
            "                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-left-width: 2px; border-left-style: Solid;\">\n" +
            "                        <b>Branch:</b> " + clvar.BranchName + "\n" +
            "                    </td>\n" +
            "                    <td colspan=\"4\" style=\"border-right-style: solid; border-right-width: 2px; border-right-color: #000000;\n" +
            "                        border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000;\">\n" +
            "                        <b>Seal No:</b> " + clvar.SealNumber + "\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "            </table>\n" +
            "         <br />";
            return sqlString;

        }

        protected string TableStart()
        {
            string sqlString = "<table style=\"border: thin solid #000000; width: 100%; font-family: Calibri; font-size: small; border-collapse: collapse;\">\n" +
            "            <tr>\n" +
            "                <td colspan=\"6\"\n" +
            "                    style=\"text-align: center; border-style: solid; border-width: 2px; border-color: #000000; font-size:Medium;\">\n" +
            "                    <b>Manifest Detail</b>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 15%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-right-style: solid; border-right-width: 1px; border-right-color: #000000; border-left-style: solid; border-left-width: 2px; border-left-color: #000000;\">\n" +
            "                    <b>Manifest #</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Service Type</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
           "                <td style=\"width: 5%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-right-style: solid; border-right-width: 1px; border-right-color: #000000;\">\n" +
            "                    <b>Pieces</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #000000; border-right-style: solid; border-right-width: 2px; border-right-color: #000000;\">\n" +
            "                    <b>Remarks</b>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        protected string DataRow(DataRow dr)
        {
            count++;
            string size = "";
            if (flag) size = " border-bottom-width:2px;";
            string sqlString = "<tr>\n" +
            "                <td style=\"width: 15%; border-style: solid; border-width: 1px; border-color: #000000; border-left-width:2px;" + size + "\">\n" +
            "                    " + dr["ManifestNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; border-style: solid; border-width: 1px; border-color: #000000;" + size + "\">\n" +
            "                    " + dr["ManifestType"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; border-style: solid; border-width: 1px; border-color: #000000;" + size + "\">\n" +
            "                    " + dr["Origin"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 20%; border-style: solid; border-width: 1px; border-color: #000000;" + size + "\">\n" +
            "                    " + dr["Destination"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 5%; border-style: solid; border-width: 1px; border-color: #000000;" + size + "\">\n" +
            "                    " + dr["pieces"].ToString() + "\n" +
            "                </td>\n" +
           "                <td style=\"width: 20%; border-style: solid; border-width: 1px; border-color: #000000; border-right-width:2px;" + size + "\">\n" +
            "                    " + dr["remarks"].ToString() + "\n" +
            "                </td>\n" +
            "            </tr>";
            flag = false;
            return sqlString;

        }

        protected string FooterRow()
        {
            string sqlString = "<br /><br /><br /><table width='700px' style=\"font-family:Calibri; font-size:small\">\n" +
                "<tr>\n" +
                "<td style=\"width:25%; text-align:right;\"></td>\n" +
                //"<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                //"<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                //"<td style=\"width:10%; text-align:right;\"></td>\n" +
                "<td style=\"width:15.5%; text-align:left;\">Verified By</td>\n" +
                "<td style=\"width:25%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                "<td style=\"width:5%; text-align:right;\"></td>\n" +
                "<td style=\"width:25%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                "<td style=\"width:5%; text-align:right;\"></td>\n" +
                "</tr>" +
                "<tr>\n" +
                "<td style=\"width:25%; text-align:right;\"></td>\n" +
                //"<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                //"<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                //"<td style=\"width:10%; text-align:right;\"></td>\n" +
                "<td style=\"width:15.5%; text-align:left;\"></td>\n" +
                "<td style=\"width:25%; text-align:left;\">Name</td>\n" +
                "<td style=\"width:5%; text-align:right;\"></td>\n" +
                "<td style=\"width:25%; text-align:left;\">Signature</td>\n" +
                "<td style=\"width:5%; text-align:right;\"></td>\n" +
                "</tr>" +
                 "<tr>\n" +
                "<td style=\"width:25%; text-align:right;\"></td>\n" +
                //"<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                //"<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                //"<td style=\"width:10%; text-align:right;\"></td>\n" +
                "<td style=\"width:15.5%; text-align:left;\">Created By</td>\n" +
                "<td style=\"width:25%; text-align:left; border-bottom: 1px Solid Black;\">" + createdBy + "</td>\n" +
                "<td style=\"width:5%; text-align:right;\"></td>\n" +
                "<td style=\"width:25%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                "<td style=\"width:5%; text-align:right;\"></td>\n" +
                "</tr>" +
                "</table>";


            return sqlString;
        }

        protected string HeaderRowForOutPiece()
        {

            string sqlString = "<table class=\"outpiece\">\n" +
                "<tr><th colspan='6' style=\"width: 20%; border-left: 2px Solid Black; border-right: 2px Solid Black;text-align:center; font-size: medium !important;\">OUTPIECES</th></tr>\n" +
            "                <tr>\n" +
            "                    <th style=\"width: 15%; border-left: 2px Solid Black;\">\n" +
            "                        <b>CN Number</b>\n" +
            "                    </th>\n" +
            "                    <th style=\"width: 5%; border-left: 1px Solid Black;\">\n" +
            "                        <b>Pieces</b>\n" +
            "                    </th>\n" +
            "                    <th style=\"width: 30%; border-left: 1px Solid Black;\">\n" +
            "                        <b>Consigner</b>\n" +
            "                    </th>\n" +
            "                    <th style=\"width: 30%; border-left: 1px Solid Black;\">\n" +
            "                        <b>Consignee</b>\n" +
            "                    </th>\n" +
            "                    <th style=\"width: 5%; border-left: 1px Solid Black; border-right: 1px Solid Black;\">\n" +
            "                        <b>Weight</b>\n" +
            "                    </th>\n" +
            "                    <th style=\"width: 15%; border-left: 1px Solid Black; border-right: 2px Solid Black;\">\n" +
            "                        <b>Remarks</b>\n" +
            "                    </th>\n" +
            "                </tr>";


            return sqlString;
        }

        protected string DataRowOutPiece(DataRow dr)
        {
            string border = "";
            if (flag)
            {
                border = "border-bottom: 2px Solid Black;";
            }
            else
            {
                border = "";
            }
            string sqlString = "<tr>\n" +
            "                    <td style=\"width: 15%; border-left: 2px Solid Black; " + border + "\">\n" +
            "                        " + dr["consignmentNumber"].ToString() + "\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 5%; border-left: 1px Solid Black; " + border + "\">\n" +
            "                        " + dr["pieces"].ToString() + "\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; border-left: 1px Solid Black; " + border + "\">\n" +
            "                        " + dr["consigner"].ToString() + "\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 30%; border-left: 1px Solid Black; " + border + "\">\n" +
            "                        " + dr["consignee"].ToString() + "\n" +
            "                    </td>\n" +
             "                    <td style=\"width: 5%; border-left: 1px Solid Black; " + border + "\">\n" +
            "                        " + dr["weight"].ToString() + "\n" +
            "                    </td>\n" +
            "                    <td style=\"width: 15%; border-left: 1px Solid Black; border-right: 2px Solid Black; " + border + "\">\n" +
            "                        " + dr["Remarks"].ToString() + "\n" +
            "                    </td>\n" +
            "                </tr>";
            return sqlString;
        }




        public DataTable GetBagPrintReportDetail(Cl_Variables clvar)
        {

            string sqlString = "select b.manifestNumber,\n" +
            "       m.manifestType,\n" +
            "       b1.name          Origin,\n" +
            "       b2.name          Destination, b.pieces, b.remarks\n" +
            "\n" +
            "  from BagManifest b\n" +
            " inner join mnp_Manifest m\n" +
            "    on b.manifestNumber = m.manifestNumber\n" +
            "\n" +
            " inner join Branches b1\n" +
            "    on m.origin = b1.branchCode\n" +
            "\n" +
            " inner join Branches b2\n" +
            "    on m.destination = b2.branchCode\n" +
            "\n" +
            " where b.bagNumber = '" + clvar.BagNumber + "'";
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

        public DataTable GetBagOutPieces(Cl_Variables clvar)
        {

            string sqlString = "select outpieceNumber consignmentNumber, bp.pieces, '' consigner,  '' consignee, bp.weight, bp.remarks\n" +
            "  from BagOutpieceAssociation bp\n" +
            " where bp.bagNumber = '" + clvar.BagNumber + "'";
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