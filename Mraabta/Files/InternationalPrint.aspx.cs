using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class InternationalPrint : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
            {
                string consignmentnumber = Request.QueryString["XCode"];
                DataSet dt = GetConsignment(consignmentnumber);
                Print(dt, "COPY CONSIGNEE", true);
                //ph1.Controls.Add(new Literal { Text = "<hr />" });
                Print(dt, "COPY DISPATCH & ACCOUNTING", true);

                Print(dt, "COPY SHIPPER'S COPY", true);
                //ph1.Controls.Add(new Literal { Text = "<hr />" });
                Print(dt, "COPY ORIGINAL STATION", true);

                Print(dt, "COPY CUSTOMS", true);
            }
        }

        public DataSet GetConsignment(string consignmentNumber)
        {



            DataSet dt = new DataSet();


            string sqlString = "SELECT C.ConsignmentNumber, C.CONSIGNERACCOUNTNO,\n" +
            "       C.CONSIGNER,\n" +
            "       C.CONSIGNEE,\n" +
            "       B.SNAME ORIGIN,\n" +
            "       B1.SNAME DESTINATION,\n" +
            "       C.ADDRESS CONSIGNEEADDRESS,\n" +
            "       C.SHIPPERADDRESS,\n" +
            "       C.CONSIGNEEPHONENO,\n" +
            "       C.CONSIGNERCELLNO,\n" +
            "       C.PAKAGECONTENTS,\n" +
            "       C.WEIGHT,\n" +
            "       MONTH(C.BOOKINGDATE) MONTH,\n" +
            "       DAY(C.BOOKINGDATE) DAY,\n" +
            "       YEAR(C.BOOKINGDATE) YEAR,\n" +
            "       DATEPART(HOUR, C.CREATEDON) HOUR,\n" +
            "       DATEPART(MINUTE, C.CREATEDON) MINUTE,\n" +
            "       CONVERT(VARCHAR(5), C.CREATEDON, 108) + ' ' +\n" +
            "       RIGHT(CONVERT(VARCHAR(30), C.CREATEDON, 9), 2) TIME,\n" +
            "       C.BOOKINGDATE,\n" +
            "       R.FIRSTNAME + ' ' + R.LASTNAME RIDERNAME,\n" +
            "       C.PIECES,\n" +
            "       C.WEIGHT,\n" +
            "       CASE\n" +
            "         WHEN C.ISINSURED = '1' THEN\n" +
            "          'YES'\n" +
            "         ELSE\n" +
            "          'NO'\n" +
            "       END INSURANCE,c.decalaredValue\n" +
            "\n" +
            "  FROM CONSIGNMENT C\n" +
            " INNER JOIN BRANCHES B\n" +
            "    ON B.BRANCHCODE = C.ORGIN\n" +
            " INNER JOIN BRANCHES B1\n" +
            "    ON B1.BRANCHCODE = C.DESTINATION\n" +
            " INNER JOIN RIDERS R\n" +
            "    ON R.RIDERCODE = C.RIDERCODE\n" +
            "   AND R.BRANCHID = C.ORGIN\n" +

            " WHERE C.CONSIGNMENTNUMBER = '" + consignmentNumber + "'";



            sqlString = "SELECT C.CONSIGNMENTNUMBER,\n" +
            "       C.CONSIGNERACCOUNTNO,\n" +
            "       C.CONSIGNER,\n" +
            "       C.CONSIGNEE,\n" +
            "       B.SNAME ORIGIN,\n" +
            "       B1.SNAME DESTINATION,\n" +
            "       C.ADDRESS CONSIGNEEADDRESS,\n" +
            "       C.SHIPPERADDRESS,\n" +
            "       C.CONSIGNEEPHONENO,\n" +
            "       C.CONSIGNERCELLNO,\n" +
            "       C.PAKAGECONTENTS,\n" +
            "       C.WEIGHT,\n" +
            "       BG.GST BRANCHGST, C.GST,\n" +
            "       MONTH(C.BOOKINGDATE) MONTH,\n" +
            "       DAY(C.BOOKINGDATE) DAY,\n" +
            "       YEAR(C.BOOKINGDATE) YEAR,\n" +
            "       DATEPART(HOUR, C.CREATEDON) HOUR,\n" +
            "       DATEPART(MINUTE, C.CREATEDON) MINUTE,\n" +
            "       CONVERT(VARCHAR(5), C.CREATEDON, 108) + ' ' +\n" +
            "       RIGHT(CONVERT(VARCHAR(30), C.CREATEDON, 9), 2) TIME,\n" +
            "       C.BOOKINGDATE,\n" +
            "       R.FIRSTNAME + ' ' + R.LASTNAME RIDERNAME,\n" +
            "       C.PIECES,\n" +
            "       C.WEIGHT,\n" +
            "\n" +
            "       CASE\n" +
            "         WHEN C.ISINSURED = '1' THEN\n" +
            "          'YES'\n" +
            "         ELSE\n" +
            "          'NO'\n" +
            "       END INSURANCE,\n" +
            "       C.DECALAREDVALUE, C.chargedAmount, C.totalAmount, C.createdOn, C.DestinationCountryCode, C.serviceTypeName\n" +
            "\n" +
            "  FROM CONSIGNMENT C\n" +
            " INNER JOIN BRANCHES B\n" +
            "    ON B.BRANCHCODE = C.ORGIN\n" +
            " INNER JOIN BRANCHES B1\n" +
            "    ON B1.BRANCHCODE = C.DESTINATION\n" +
            " INNER JOIN RIDERS R\n" +
            "    ON R.RIDERCODE = C.RIDERCODE\n" +
            "   AND R.BRANCHID = C.ORGIN\n" +
            " INNER JOIN BRANCHGST BG\n" +
            "    ON BG.BRANCHCODE = C.BRANCHCODE\n" +
            "\n" +
            " WHERE C.CONSIGNMENTNUMBER = '" + consignmentNumber + "'\n" +
            "   AND BG.STATUS = '1'\n" +
            "   AND C.BOOKINGDATE BETWEEN BG.EFFECTIVEFROM AND BG.EFFECTIVETO\n" +
            "   AND BG.COMPANYID = '1'";

            string query = "selecT * from ConsignmentModifier cm where cm.consignmentNumber = '" + consignmentNumber + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt, "Consignment");
                sda = new SqlDataAdapter(query, con);
                sda.Fill(dt, "Modifiers");

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return dt;
        }

        public string Print(DataSet ds, string copy, bool pBreak)
        {
            double totalCharges = 0;
            DataTable dt = ds.Tables["Consignment"];
            DataTable dt_ = ds.Tables["Modifiers"];
            string time = "";
            if (dt.Rows[0]["Time"].ToString() != "")
            {
                time = DateTime.Parse(dt.Rows[0]["CreatedOn"].ToString()).ToString("hh:mm tt").Trim('A', 'M', 'P');
            }
            double gst = 0;
            object gst_ = dt_.Compute("SUM(CalculatedGST)", "");
            double tempgst = 0;
            double.TryParse(gst_.ToString(), out tempgst);

            double modifierGST = 0;
            double totalAmount = 0;
            //double.TryParse(dt.Rows[0]["totalAmount"].ToString(), out totalAmount);
            double.TryParse(dt.Rows[0]["gst"].ToString(), out gst);
            double.TryParse(dt.Rows[0]["totalAmount"].ToString(), out totalAmount);
            string pageBreak = "";
            if (pBreak)
            {
                pageBreak = "page-break-after: always";
            }
            gst = tempgst + gst;
            totalCharges = totalAmount + gst;
            string sqlString = "<div style=\"height: 520px; " + pageBreak + "\"><table style=\"width: 650px; \" cellpadding=\"0\" cellspacing=\"0\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 635px;\">\n" +
            "                    <table style=\"width: 600px; border-collapse: collapse; font-size: x-small; font-family: Arial;\"\n" +
            "                        cellpadding=\"0px\">\n" +
            "                        <tr>\n" +
            "                            <td style=\"width: 317px;\">\n" +
            "                                <table style=\"width: 317px; border-collapse: collapse; font-size: x-small; font-family: Arial;\">\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"width: 130px; height: 50px; border-top: 2px Solid Black; border-left: 2px Solid Black;\n" +
            "                                            border-bottom: 1px Solid Black; border-right: 1px Solid Black; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                                padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                SHIPPER'S ACCOUNT\n" +
            "                                            </div>\n" +
            "                                           " + dt.Rows[0]["consignerAccountNo"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 58px; height: 50px; border-top: 2px Solid Black; border-left: 1px Solid Black;\n" +
            "                                            border-bottom: 1px Solid Black; border-right: 2px Solid Black; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                                padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                ORIGIN\n" +
            "                                            </div>\n" +
            "                                               " + dt.Rows[0]["ORIGIN"].ToString() + " \n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 129px; border-bottom: 2px Solid Black;\">\n" +
            "                                            <img src=\"../images/OCS_Transparent.png\" height=\"50px\" />\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"width: 130px; height: 50px; border-top: 1px Solid Black; border-left: 2px Solid Black;\n" +
            "                                            border-bottom: 1px Solid Black; border-right: 1px Solid Black; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                                padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                SECTION CODE\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 187px; height: 50px; border-right: 2px Solid Black; border-bottom: 1px Solid Black;\"\n" +
            "                                            colspan=\"2\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                                padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                SERVICE (PLEASE CHECK)\n" +
            "                                            </div>\n" +
            "                                            <table style=\"width: 100%; height: 35px; border-collapse: collapse;\">\n" +
            "                                                <tr>\n" +
            "                                                    <td style=\"border-right: 1px Solid Black; width: 100%; text-align: center;\" colspan='4'>\n" +
            "                                                           " + ds.Tables[0].Rows[0]["ServiceTypeName"].ToString() + "" +
            //"                                                        DOC\n" +
            //"                                                    </td>\n" +
            //"                                                    <td style=\"border-right: 1px Solid Black; width: 25%\">\n" +
            //"                                                    </td>\n" +
            //"                                                    <td style=\"border-right: 1px Solid Black; width: 25%\">\n" +
            //"                                                    </td>\n" +
            //"                                                    <td style=\"width: 25%; text-align: center;\">\n" +
            //"                                                        SPS\n" +
            "                                                    </td>\n" +
            "                                                </tr>\n" +
            "                                            </table>\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td colspan=\"3\" style=\"width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;\n" +
            "                                            height: 110px; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                FROM (SHIPPER)\n" +
            "                                            </div>\n" +
            //"                                                " + dt.Rows[0]["Consigner"].ToString() + " </ br> \n"+
            "                                                " + dt.Rows[0]["ShipperAddress"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"width: 130px; height: 34px; border-bottom: 1px Solid Black; border-left: 2px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 187px; height: 34px; border: 1px Solid Black; vertical-align: top;\"\n" +
            "                                            colspan=\"2\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: top;\">\n" +
            "                                                PHONE\n" +
            "                                            </div>\n" +
            "                                                " + dt.Rows[0]["ConsignerCellNo"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td colspan=\"3\" style=\"width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;\n" +
            "                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                SHIPPER'S NAME/DEPT. (SIGNATURE)\n" +
            "                                            </div>\n" +
            "                                                " + dt.Rows[0]["Consigner"].ToString() + "  \n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td colspan=\"3\" style=\"width: 100%; border-left: 2px Solid Black; border-right: 1px Solid Black;\n" +
            "                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                SHIPPER'S REFERENCE\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                </table>\n" +
            "                            </td>\n" +
            "                            <td style=\"width: 300px;\">\n" +
            "                                <table style=\"width: 301px; border-collapse: collapse; font-size: x-small; font-family: Arial;\">\n" +
            "                                    <tr>\n" +
            "                                        <td colspan=\"3\" style=\"width: 100%; height: 105px; border-bottom: 2px Solid Black; text-align: center; font-size: large; font-family: IDAutomationHC39M;\">\n" +
            "                                           *" + dt.Rows[0]["ConsignmentNumber"].ToString() + "*\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td colspan=\"3\" style=\"width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;\n" +
            "                                            height: 110px; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                TO (CONSIGNEE)\n" +
            "                                            </div>\n" +
            "                                                " + dt.Rows[0]["ConsigneeAddress"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"width: 130px; height: 34px; border-bottom: 1px Solid Black; border-left: 1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 187px; height: 34px; border: 1px Solid Black; vertical-align: top;\"\n" +
            "                                            colspan=\"2\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: top;\">\n" +
            "                                                PHONE\n" +
            "                                            </div>\n" +
            "                                                " + dt.Rows[0]["consigneePhoneNo"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td colspan=\"3\" style=\"width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;\n" +
            "                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                ATTN\n" +
            "                                            </div>\n" +
            "                                                " + dt.Rows[0]["Consignee"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td colspan=\"3\" style=\"width: 100%; border-left: 1px Solid Black; border-right: 1px Solid Black;\n" +
            "                                            border-bottom: 1px Solid Black; height: 34px; vertical-align: top;\">\n" +
            "                                            <div style=\"width: 100%; height: 14px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                                OTHER: (SPECIAL)\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                </table>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"width: 633px; height: 73px; vertical-align: top; border-bottom: 1px Solid Black;\"\n" +
            "                                colspan=\"2\">\n" +
            "                                <div style=\"border-left: 2px Solid Black; width: 616px; height: 73px; border-right: 1px Solid Black;\">\n" +
            "                                    <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                        padding-left: 2px; vertical-align: middle; text-align: center;\">\n" +
            "                                        DESCRIPTION OF CONTENTS\n" +
            "                                    </div>\n" +
            "                                    <div style=\"width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                        text-align: left; font-size: x-small;\">\n" +
            "                                        Please attach a commercial invoice of SPS\n" +
            "                                    </div>\n" +
            "                                                " + dt.Rows[0]["PakageContents"].ToString() + "\n" +
            "                                </div>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"width: 317px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;\n" +
            "                                vertical-align: top;\">\n" +
            "                                <table style=\"width: 317px; height: 50px; border-collapse: collapse; border-left: 2px Solid Black;\"\n" +
            "                                    cellpadding=\"0px;\">\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\">\n" +
            "                                            <div style=\"width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                DECLARED VALUE FOR CUSTOMS\n" +
            "                                            </div>\n" +
            //"                                                " + dt.Rows[0]["decalaredValue"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\">\n" +
            "                                            <div style=\"width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                INSURANCE\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 47px; font-size: small;\">\n" +
            "                                                " + dt.Rows[0]["decalaredValue"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 34px; font-size: small;\">\n" +
            "                                            " + dt.Rows[0]["Insurance"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                </table>\n" +
            "                            </td>\n" +
            "                            <td style=\"width: 300px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;\n" +
            "                                vertical-align: top;\">\n" +
            "                                <table style=\"width: 300px; height: 50px; border-collapse: collapse;\" cellpadding=\"0px;\">\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\">\n" +
            "                                            <div style=\"width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: left; font-size: x-small;\">\n" +
            "                                                DIMENSIONS (CM)\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\">\n" +
            "                                            <div style=\"width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                CHARGEABLE WEIGHT\n" +
            "                                            </div>\n" +
            //"                                               " + dt.Rows[0]["Weight"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 34px;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 34px; font-size: small;\">\n" +
            "                                               " + dt.Rows[0]["Weight"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                </table>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"width: 317px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;\n" +
            "                                vertical-align: top;\">\n" +
            "                                <table style=\"width: 317px; height: 50px; border-collapse: collapse; border-left: 2px Solid Black;\"\n" +
            "                                    cellpadding=\"0px;\">\n" +
            "                                    <tr style=\"height: 15px\">\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\" colspan=\"3\">\n" +
            "                                            <div style=\"width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                DATE\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px; border-right: 1px Solid Black;\"\n" +
            "                                            colspan=\"4\">\n" +
            "                                            <div style=\"width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                TIME\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\" colspan=\"3\">\n" +
            "                                            <div style=\"width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                PICKED-UP FOR\n" +
            "                                                <img src=\"../images/OCS_Transparent.png\" height=\"15px\" />\n" +
            "                                                BY\n" +
            "                                            </div>\n" +

            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr style=\"height: 15px\">\n" +
            "                                        <td style=\"text-align: right; vertical-align: middle; height: 15px;\">\n" +
            "                                           <span style=\"text-align: center ! important; float: left; width: 21px ! important;\">" + dt.Rows[0]["MONTH"].ToString() + "</span><span>/</span>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: right; vertical-align: middle; height: 15px;\">\n" +
            "                                            <span style=\"text-align: center ! important; float: left; width: 21px ! important;\">" + dt.Rows[0]["DAY"].ToString() + "</span><span>/</span>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                            " + dt.Rows[0]["YEAR"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                        </td>\n";
            if (dt.Rows[0]["Time"].ToString().Contains("AM"))
            {
                sqlString += "                           <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
                    "                                            " + time + "\n" +
                    "                                        </td>\n";
            }
            else
            {
                sqlString += "                           <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
                    "                                            \n" +
                    "                                        </td>\n";
            }
            sqlString +=
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;\">\n" +
            "                                            AM\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\" colspan=\"3\" rowspan='2'>\n" +
            "                                           " + dt.Rows[0]["RiderName"].ToString() + "    \n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr style=\"height: 15px\">\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            M\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: Center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            D\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            Y\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                        </td>\n";
            if (dt.Rows[0]["Time"].ToString().Contains("PM"))
            {
                sqlString += "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            " + time + "\n" +
            "                                        </td>\n";
            }
            else
            {
                sqlString += "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            \n" +
            "                                        </td>\n";
            }
            sqlString +=

            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;\">\n" +
            "                                            PM\n" +
            "                                        </td>\n" +
            //"                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\" colspan=\"3\">\n" +
            //"                                        </td>\n" +
            "                                    </tr>\n" +
            "                                </table>\n" +
            "                            </td>\n" +
            "                            <td style=\"width: 300px; height: 50px; border-bottom: 1px Solid Black; border-right: 1px Solid Black;\n" +
            "                                vertical-align: top;\">\n" +
            "                                <table style=\"width: 300px; height: 50px; border-collapse: collapse;\" cellpadding=\"0px;\">\n" +
            "                                    <tr style=\"height: 15px\">\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\" colspan=\"3\">\n" +
            "                                            <div style=\"width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                DATE\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px; border-right: 1px Solid Black;\"\n" +
            "                                            colspan=\"4\">\n" +
            "                                            <div style=\"width: 74px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                TIME\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\" colspan=\"3\">\n" +
            "                                            <div style=\"width: 148px; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\n" +
            "                                                text-align: Center; font-size: x-small;\">\n" +
            "                                                CONSIGNEE'S SIGNATURE\n" +
            "                                            </div>\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr style=\"height: 15px\">\n" +
            "                                        <td style=\"text-align: right; vertical-align: middle; height: 15px;\">\n" +
            "                                           <span style=\"text-align: center ! important; float: left; width: 21px ! important;\">" + dt.Rows[0]["MONTH"].ToString() + "</span><span>/</span>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: right; vertical-align: middle; height: 15px;\">\n" +
            "                                           <span style=\"text-align: center ! important; float: left; width: 21px ! important;\">" + dt.Rows[0]["DAY"].ToString() + "</span><span>/</span>\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                           " + dt.Rows[0]["YEAR"].ToString() + "\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                        </td>\n";
            if (dt.Rows[0]["Time"].ToString().Contains("AM"))
            {
                sqlString += "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                           " + time + "\n" +
            "                                        </td>\n";
            }
            else
            {
                sqlString += "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                           \n" +
            "                                        </td>\n";
            }
            sqlString +=

            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;\">\n" +
            "                                            AM\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\" colspan=\"3\">\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                    <tr style=\"height: 15px\">\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            M\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: Center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            D\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            Y\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                        </td>\n";
            if (dt.Rows[0]["Time"].ToString().Contains("PM"))
            {
                sqlString += "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            " + time + "\n" +
            "                                        </td>\n";
            }
            else
            {
                sqlString += "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                            \n" +
            "                                        </td>\n";
            }
            sqlString +=

            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; width: 24px;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: middle; height: 15px; border-right: 1px Solid Black;\">\n" +
            "                                            PM\n" +
            "                                        </td>\n" +
            "                                        <td style=\"text-align: center; vertical-align: top; height: 15px;\" colspan=\"3\">\n" +
            "                                        </td>\n" +
            "                                    </tr>\n" +
            "                                </table>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                    </table>\n" +
            "                </td>\n" +
            "                <td style=\"width: 85px; vertical-align: top;\">\n" +
            "                    <table cellpadding=\"0\" style=\"border-collapse: collapse; font-size: x-small; font-family: Arial;\">\n" +
            "                        <tr>\n" +
            "                            <td style=\"height: 53px; width: 85px; border-top: 2px Solid Black; border-left: 2px Solid Black;\n" +
            "                                border-right: 2px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;\">\n" +
            "                                <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                    padding-left: 2px; vertical-align: middle;\">\n" +
            "                                    DESTINATION\n" +
            "                                </div>\n" +
            "                                <div style='Height: 28px; text-align:center; position: relative; top:10px; font-size: small; font-weight:bold;'>" +
            "                                   " + dt.Rows[0]["DESTINATIONCOUNTRYCODE"].ToString() + "</div>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"height: 50px; width: 85px; border-top: 2px Solid Black; border-left: 2px Solid Black;\n" +
            "                                border-right: 2px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;\">\n" +
            "                                <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                    padding-left: 2px; vertical-align: middle;\">\n" +
            "                                    PIECES\n" +
            "                                </div>\n" +
            "                                <div style='Height: 28px; text-align:center; position: relative; top:10px; font-size: small; font-weight:bold;'>" +
            "                                    " + dt.Rows[0]["Pieces"].ToString() + "</div>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;\n" +
            "                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;\">\n" +
            "                                <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                    padding-left: 2px; vertical-align: middle;\">\n" +
            "                                    WEIGHT\n" +
            "                                </div>\n" +
            "                                <div style=\"width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                    (1) ADX/APX\n" +
            "                                </div>\n" +
            "                                <div style=\"width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle; text-align:center; font-size:small; font-weight: bold;\">\n" +
            //"                                    " + String.Format("{0:N2}", totalAmount) + "\n" +
            "                                      " + dt.Rows[0]["Weight"].ToString() + "\n" +
            "                                </div>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;\n" +
            "                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;\">\n" +
            "                                <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                    padding-left: 2px; vertical-align: middle; font-size: xx-small;\">\n" +
            "                                    WEIGHT CHARGE\n" +
            "                                </div>\n" +
            "                                <div style=\"width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle;\">\n" +
            "                                    (2) HDLG\n" +
            "                                </div>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"height: 182.5px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;\n" +
            "                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;\">\n" +
            "                                <div style=\"width: 100%; height: 18px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                    padding-left: 2px; vertical-align: middle;\">\n" +
            "                                    SURCHARGE\n" +
            "                                </div>\n" +
            "                                <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 84px; border-collapse: collapse;\">\n";
            sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black; text-align:center; font-size: small; font-weight: bold;\">\n" +
            "                                           " + String.Format("{0:N2}", totalAmount) + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            //"                                    <tr style=\"height: 23.5px;\">\n" +
            //"                                        <td style=\"width: 20px; border-right: 1px Solid Black;\">\n" +
            //"                                        </td>\n" +
            //"                                        <td style=\"width: 60px;\">\n" +

            //"                                        </td>\n" +
            //"                                    </tr>\n";

            sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black; font-weight:bold; text-align:center;\">(3)\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black; font-weight:bold; text-align:center;\">" + dt.Rows[0]["BranchGST"].ToString() + "% GST\n" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black; text-align:center; font-size: small; font-weight: bold;\">\n" +
            "                                           " + Math.Round(gst, 2) + "\n" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black; font-weight:bold; text-align:center;\">(4)\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black; font-weight:bold; text-align:center;\">OTHERS\n" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            if (dt_.Rows.Count > 0)
            {
                double val = 0;
                double.TryParse(dt_.Rows[0]["CalculatedValue"].ToString(), out val);
                totalCharges = val + totalCharges;
                sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black; text-align:center; font-size: small; font-weight: bold;\">\n" +
            "                                           " + Math.Round(val, 2).ToString() + "" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            }
            else
            {

                sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            }

            if (dt_.Rows.Count > 1)
            {
                double val = 0;
                double.TryParse(dt_.Rows[1]["CalculatedValue"].ToString(), out val);
                totalCharges = totalCharges + val;
                sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black; text-align:center; font-size: small; font-weight: bold;\">\n" +
            "                                           " + Math.Round(val, 2).ToString() + "" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            }
            else
            {
                sqlString +=
            "                                    <tr style=\"height: 23.5px;\">\n" +
            "                                        <td style=\"width: 20px; border-right: 1px Solid Black; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                        <td style=\"width: 60px; border-bottom:1px Solid Black;\">\n" +
            "                                        </td>\n" +
            "                                    </tr>\n";
            }

            sqlString +=
            "                                </table>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"height: 50px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;\n" +
            "                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;\">\n" +
            "                                <div style=\"width: 100%; height: 15px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                    padding-left: 2px; vertical-align: middle; font-size: 8px;\">\n" +
            "                                    <b>COLLECT CHARGE</b>\n" +
            "                                </div>\n" +
            "                                <div style=\"width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle; text-align:center; font-size: small; font-weight: bold;\">\n" +
            "                                   " + dt.Rows[0]["ChargedAmount"].ToString() + "\n" +
            "                                </div>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                        <tr>\n" +
            "                            <td style=\"height: 75px; width: 85px; border-top: 2px Solid Black; border-right: 2px Solid Black;\n" +
            "                                border-left: 1px Solid Black; border-bottom: 1px Solid Black; vertical-align: top;\">\n" +
            "                                <div style=\"width: 100%; height: 30px; border-bottom: 1px Solid Black; margin-left: -1px;\n" +
            "                                    padding-left: 2px; vertical-align: middle;\">\n" +
            "                                    TOTAL CHARGE\n" +
            "\n" +
            "                                    (1+2+3+4)\n" +
            "                                </div>\n" +
            "                                <div style=\"width: 100%; height: 15px; margin-left: -1px; padding-left: 2px; vertical-align: middle; text-align:center; font-size: small; font-weight: bold;\">\n" +
            "                                   " + String.Format("{0:N2}", totalCharges) + "\n" +
            "                                </div>\n" +
            "                            </td>\n" +
            "                        </tr>\n" +
            "                    </table>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "        </table>\n" +
            "<div style=\"position: relative; font-family: Arial; width: 150px; text-align: center; font-size:small;\"><b>NON NEGOTIABLE</b></div>\n" +
            "<div style=\"position: relative; font-family: Arial; width: 250px; text-align: right; margin-left: 450px; margin-top: -16px; font-size:small;\"><b>" + copy + "</b></div>" +
            "</div>";

            ph1.Controls.Add(new Literal { Text = sqlString });
            return sqlString;
        }

    }
}