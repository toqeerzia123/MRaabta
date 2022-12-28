﻿using MRaabta.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class Manifest_Print : System.Web.UI.Page
    {
        int count = 0;
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        cl_Encryption enc = new cl_Encryption();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
            {

                PrintReport();
            }
        }

        protected void PrintReport()
        {
            //    clvar.manifestNo = enc.Decrypt(Request.QueryString["XCode"].ToString());
            clvar.manifestNo = Request.QueryString["XCode"].ToString();
            DataTable dt = new DataTable();
            dt = GetConsignmentDetailByManifestNumber(clvar);
            DataTable header = con.GetManifestHeader(clvar);
            clvar.originBrName = header.Rows[0]["Origin"].ToString();
            clvar.Destination = header.Rows[0]["Destination"].ToString();
            clvar.deliveryDate = DateTime.Parse(header.Rows[0]["Date"].ToString());
            clvar.BranchName = header.Rows[0]["Branch"].ToString();
            int rowCount = 0;
            StringBuilder html = new StringBuilder();
            html.Append(HeaderTable(clvar));
            html.Append(TableStart());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (rowCount == 20)
                {
                    rowCount = 0;
                    html.Append("</table>");
                    html.Append(HeaderTable(clvar));
                    html.Append(TableStart());
                }

                html.Append(DataRow(dt.Rows[i]));
                rowCount++;

            }

            html.Append("</table>");
            html.Append(FooterRow());
            ph1.Controls.Add(new Literal { Text = html.ToString() });
            HttpContext.Current.Response.Write("<script>window.print();</script>");


        }

        protected string HeaderTable(Cl_Variables clvar)
        {

            string sqlString = "<div style=\"width:700px;\"><table width=\"100%\" class=\"none\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%\">\n" +
            "                    <img src=\"../images/OCS_Transparent.png\" height=\"60px\" alt=\"logo\" width=\"157px\" />\n" +
            "                </td>\n" +
            "                <td style=\"width: 80%\">\n" +
            "                    <table width=\"100%\" style=\"font-family:Calibri; font-size:small;\">\n" +
            "                        <tr>\n" +
            "                            <td colspan=\"6\" style=\"text-align: center;\">\n" +
            "                                <h2>\n" +
            "                                    MANIFEST REPORT</h2>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"text-align: right; width: 30px;\">\n" +
            "                                <b>BRACNH</b>\n" +
            "                            </td>\n" +
            "                            <td style=\"border-bottom: 3px Solid Black;\">\n" +
            clvar.BranchName +
            "                            </td>\n" +
            "                            <td style=\"text-align: right; width: 30px;\">\n" +
            "                                <b>DATE</b>\n" +
            "                            </td>\n" +
            "                            <td style=\"border-bottom: 3px Solid Black;\">\n" +
            clvar.deliveryDate.ToShortDateString() +
            "                            </td>\n" +
            "                            <td style=\"text-align: right; width: 30px;\">\n" +
            "                                <b>NO</b>\n" +
            "                            </td>\n" +
            "                            <td style=\"border-bottom: 3px Solid Black;\">\n" +
            clvar.manifestNo +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"text-align: right; width: 30px;\">\n" +
            "                                <b>FROM</b>\n" +
            "                            </td>\n" +
            "                            <td style=\"border-bottom: 3px Solid Black;\">\n" +
            clvar.originBrName +
            "                            </td>\n" +
            "                            <td style=\"text-align: right; width: 30px;\">\n" +
            "                                <b>VIA</b>\n" +
            "                            </td>\n" +
            "                            <td style=\"border-bottom: 3px Solid Black;\">\n" +
            "                            </td>\n" +
            "                            <td style=\"text-align: right; width: 30px;\">\n" +
            "                                <b>TO</b>\n" +
            "                            </td>\n" +
            "                            <td style=\"border-bottom: 3px Solid Black;\">\n" +
            clvar.Destination +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                    </table>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "        </table></div>";


            return sqlString;

        }

        protected string TableStart()
        {

            string sqlString = "<table class=\"table\" style=\"font-family: Calibri; font-size: small; border: medium solid rgb(0, 0, 0);\n" +
            "            margin: 0px; padding: 0px; width: 700px; position: relative; top: 10px;\">\n" +
            "            <tr>\n" +
            "                <td style=\"width:5%\">\n" +
            "                    <b>SN</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>SHIPPER</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>CONSIGNMENT NO.</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>CONSIGNEE</b>\n" +
            "                </td>\n" +
            "                <td>\n" +
            "                    <b>ACTUAL WEIGHT</b>\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;
        }

        protected string DataRow(DataRow dr)
        {
            count++;
            string sqlString = "<tr>\n" +
            "                <td style=\"width: 10%;\">\n" +
            "                    " + count + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 27.5%;\">\n" +
            "                    " + dr["Consigner"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%;\">\n" +
            "                    " + dr["ConsignmentNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 27.5%;\">\n" +
            "                    " + dr["Consignee"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 15%;\">\n" +
            "                    " + dr["Weight"].ToString() + " kg\n" +
            "                </td>\n" +
            "            </tr>";
            return sqlString;

        }

        protected string FooterRow()
        {
            string sqlString = "<br /><br /><table width='700px' style=\"font-family:Calibri; font-size:small\">\n" +
                "<tr>\n" +
                "<td style=\"width:10%; text-align:right;\"></td>\n" +
                "<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\">Received From</td>\n" +
                "<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                "<td style=\"width:10%; text-align:right;\"></td>\n" +
                "<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\">Received By</td>\n" +
                "<td style=\"width:17.5%; text-align:left; border-bottom: 1px Solid Black;\"></td>\n" +
                "<td style=\"width:10%; text-align:right;\"></td>\n" +
                "</tr></table>";


            return sqlString;
        }

        public DataTable GetConsignmentDetailByManifestNumber(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber, c.consigner, c.consignee, c.weight,\n" +
            "       c.orgin,\n" +
            "       b.name              OriginName,\n" +
            "       c.destination,\n" +
            "       b2.name             DestinationName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces, 'NO' ISMODIFIED,\n" +
            "       (Select DATE from MNP_Manifest where manifestNumber = '" + clvar.manifestNo + "') ManifestDate" +
            "  from  consignment c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on c.destination = b2.branchCode\n" +
            " where c.consignmentNumber in ( SELECT cm.ConsignmentNumber from MNP_ConsignmentManifest cm where cm.manifestNumber = '" + clvar.manifestNo + "'  )";

            sqlString = "SELECT cm.consignmentNumber, \n"
               + "        c.consigner, \n"
               + "        c.consignee, \n"
               + "        c.orgin, \n"
               + "        b.sname + '-' + b.name       OriginName, \n"
               + "        c.destination, \n"
               + "        b2.sname + '-' + b2.name     DestinationName, \n"
               + "        c.consignmentTypeId, \n"
               + "        c.serviceTypeName, \n"
               + "        cm.[Weight], \n"
               + "        cm.Pieces, \n"
               + "        'NO' ISMODIFIED, \n"
               + "        ( \n"
               + "            SELECT Date \n"
               + "            FROM   Mnp_Manifest mm \n"
               + "            WHERE  mm.manifestNumber = '" + clvar.manifestNo + "' \n"
               + "        )                            ManifestDate \n"
               + " FROM   Mnp_ConsignmentManifest cm \n"
               + "        INNER JOIN Consignment c \n"
               + "             ON  c.consignmentNumber = cm.consignmentNumber \n"
               + "        INNER JOIN Branches b \n"
               + "             ON  b.branchCode = c.orgin \n"
               + "        INNER JOIN Branches b2 \n"
               + "             ON  b2.branchCode = c.destination \n"
               + " WHERE  cm.manifestNumber = '" + clvar.manifestNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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