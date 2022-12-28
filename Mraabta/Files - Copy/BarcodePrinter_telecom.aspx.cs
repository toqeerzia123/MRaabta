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
    public partial class BarcodePrinter_telecom : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        bool last = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            PrintBarCodes();

            //NewPrinting();
            last = true;
            //NewPrinting();

        }

        public string NewPrinting(DataRow dr, int i)
        {
            string pageBreak = "";
            if (last)
            {
                pageBreak = "";
            }
            else
            {
                //pageBreak = "page-break-after: always;";
            }


            string sqlString = "<div style=\"width: 350px; height: 185px; border: 2px solid black; border-radius: 45px; padding-left: 35px; padding-right:10px; margin-top:2px;" + pageBreak + "\">\n" +
            "            <div style=\"float: left; transform: rotate(90deg); transform-origin: left top 0; padding-left: 20px; padding-right: 20px; height:0px; font-size:x-small;\">\n" +
            "                <label><strong><span>Booked on " + dr["BookingDate"].ToString() + "</span></strong></label>\n" +
            "            </div>\n" +
            "            <table style=\"padding-left: 15px; height:185px; border-collapse:collapse; width:350px;\">\n" +
            "                <tr>\n" +
            "                    <td style=\"font-size: Small; font-family: Calibri; text-align:center; font-weight:bold;\">\n" +
            "                        <label>" + dr["ORIGIN"].ToString() + "</label>\n" +
            //"                        <br />\n" +
            "                        <hr style=\"border: 2px Solid Black;\"/>\n" +
            "                    </td>\n" +
            "                    <td>\n" +
            "                        <div style=\"width: 100%; font-family:IDAutomationHC39M; text-align:center; font-size:small;\">\n" +
            "                           *" + dr["ConsignmentNumber"].ToString() + "*" +
            "                        </div>\n" +
            "                    </td>\n" +
            "                    <td style=\"font-size: Small; font-family: Calibri; text-align:center; font-weight:bold;\">\n" +
            "                        <label>" + dr["Weight"].ToString() + "</label>\n" +
            //"                        <br />\n" +
            "                        <hr  style=\"border: 2px Solid Black;\"/>\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "                <tr>\n" +
            "                    <td style=\"font-size: Small; font-family: Calibri; text-align:center; font-weight:bold;\">\n" +
            "                        <label>" + dr["DestName"].ToString() + "</label>\n" +
            "\n" +
            "                    </td>\n" +
            "                    <td style=\"font-size: Small; font-family: Calibri; text-align: center;\">\n" +
            "                        <div style=\"text-align: center; font-size:x-large; font-weight: Bold;\">" + i + " of " + dr["sealCount"].ToString() + " Bag(s)</div>\n" +
            "                        <div style=\"text-align: center; font-size:Medium;\">TI: " + dr["CouponNumber"].ToString() + "</div>\n" +
            "                    </td>\n" +
            "                    <td style=\"font-size: Small; font-family: Calibri; text-align:center; font-weight:bold;\">\n" +
            "                        <label>KGs</label>\n" +
            "\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "                <tr>\n" +
            "                    <td></td>\n" +
            "\n" +
            "                    <td style=\"font-size: Small; font-family: Calibri; text-align: center;\">\n" +
            //"                        <br />\n" +
            "                        Consignee: " + dr["Consignee"].ToString() + "\n" +
            "                    </td>\n" +
            "\n" +
            "                    <td></td>\n" +
            "                </tr>\n" +
            "                <tr>\n" +
            "                    <td></td>\n" +
            "                    <td style=\"font-size: small; font-family: Calibri; text-align: center;\">\n" +
            //"                        <br />\n" +
            "                        Muller & Phipps\n" +
            "                    </td>\n" +
            "\n" +
            "                    <td></td>\n" +
            "                </tr>\n" +
            "                <tr>\n" +
            "                    <td colspan=\"3\" rowspan=\"1\" style=\"font-size: Small; font-family: Calibri; text-align: center; border-bottom: 1px Solid Black;\">\n" +

            "                        " + dr["Address"].ToString() + "\n" +
            //"                        <hr />\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "                  <tr>\n" +
            "                    <td colspan=\"3\" style=\"font-size: Small; font-family: Calibri; text-align: center;\">\n" +
            "\n" +
            "                        Shipper:\n" +
            "                         " + dr["SHIPPER"].ToString() + "\n" +
            "                    </td>\n" +
            "                </tr>\n" +
            "            </table>\n" +
            "        </div>";


            return sqlString;
        }
        public void PrintBarCodes()
        {
            List<string[]> GP = Session["GPList"] as List<string[]>;
            //GP = new List<string>();
            //GP.Add("0124013156");
            //GP.Add("102100000007");
            //GP.Add("102100000008");
            if (GP != null)
            {
                if (GP.Count > 0)
                {
                    DataTable Cns = GetConsignmentNumbers(GP, clvar);
                    Cns.Columns.Add(new DataColumn("SealCount"));
                    foreach (string[] str in GP)
                    {
                        DataRow dr = Cns.Select("ConsignmentNumber = '" + str[0] + "'").FirstOrDefault();
                        if (dr != null)
                        {
                            dr["SealCount"] = str[1].ToString();
                        }
                    }
                    //foreach (DataRow dr in Cns.Rows)
                    //{

                    //if (dr["transactionNumber"].ToString().Length > 0 && dr["transactionNumber"].ToString().Split('-').Length == 2)
                    //{
                    //    string firstSeal = dr["transactionNumber"].ToString().Split('-')[0];
                    //    string lastSeal = dr["transactionNumber"].ToString().Split('-')[1];

                    //    int firstSealLength = firstSeal.Length;
                    //    int lastSealLength = lastSeal.Length;

                    //    string tempLastSeal = firstSeal.Substring(0, firstSealLength - lastSealLength);

                    //    float sealCount = 0;
                    //    while (true)
                    //    {
                    //        if (float.Parse(firstSeal) > float.Parse(tempLastSeal + lastSeal))
                    //        {
                    //            tempLastSeal = (float.Parse(tempLastSeal) + 1).ToString();
                    //        }
                    //        else
                    //        {
                    //            break;
                    //        }
                    //    }

                    //    sealCount = float.Parse(tempLastSeal + lastSeal) + 1 - float.Parse(firstSeal);
                    //    dr["SealCount"] = sealCount.ToString();
                    //}
                    //}
                    StringBuilder html = new StringBuilder();
                    for (int j = 0; j < Cns.Rows.Count; j++)
                    {
                        int pieces = 0;
                        DataRow dr = Cns.Rows[j];
                        int.TryParse(dr["sealcount"].ToString(), out pieces);
                        for (int i = 1; i <= pieces; i++)
                        {
                            html.Append(NewPrinting(dr, i));
                        }
                    }

                    ph1.Controls.Add(new Literal { Text = html.ToString() });
                    HttpContext.Current.Response.Write("<script>window.print();</script>");
                }


            }
        }

        public DataTable GetConsignmentNumbers(List<string[]> GP, Cl_Variables clvar)
        {
            DataTable dt = new DataTable();
            string docNos = "";
            foreach (string[] str in GP)
            {
                docNos += "'" + str[0] + "'";
            }
            docNos = docNos.Replace("''", "','");
            //string sqlString = "SELECT *\n" +
            //"  FROM GATEPASS_CONSIGNMENT GC\n" +
            //" WHERE GC.COMPANY = '" + HttpContext.Current.Session["Company"].ToString() + "'\n" +
            //"   AND GC.DISTRIBUTOR = '" + HttpContext.Current.Session["DISTRIBUTOR"].ToString() + "'\n" +
            //"   AND GC.DOCUMENT = 'GP'\n" +
            //"   AND GC.SUB_DOCUMENT = '01'\n" +
            //"   AND GC.DOC_NO IN (" + docNos + ")";


            string sqlString = "SELECT * FROM CONSIGNMENT c where c.consignmentnumber in (" + docNos + ")";

            sqlString = "selecT c.consignmentNumber,\n" +
           "       c.consigner         Shipper,\n" +
           "       c.consignee,\n" +
           "       bookingDate,\n" +
           "       c.address,\n" +
           "       pieces,\n" +
           "       weight,\n" +
           "       orgin,\n" +
           "       destination,\n" +
           "       org.sname           Origin,\n" +
           "       dest.sname          DestName, c.CouponNumber, c.transactionNumber\n" +
           "  from consignment c\n" +
           " inner join branches org\n" +
           "    on org.branchCode = c.orgin\n" +
           " inner join Branches dest\n" +
           "    on dest.branchCode = c.destination\n" +
           " where c.consignmentNumber in (" + docNos + ")";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter oda = new SqlDataAdapter(sqlString, con);
                oda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return dt;
        }
    }
}