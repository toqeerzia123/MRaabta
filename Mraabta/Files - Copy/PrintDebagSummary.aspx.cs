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
    public partial class PrintDebagSummary : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        int page = 1;
        int totalPages = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //ph1.Controls.Add(new Literal { Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") });
            PrintDemanifest();
        }

        protected void PrintDemanifest()
        {
            try
            {
                string date = Request.QueryString["Date"].ToString();

                DataTable dt = GetDemanifestPrint(date);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder html = new StringBuilder();
                        totalPages = (dt.Rows.Count / 35) + ((dt.Rows.Count % 35 != 0) ? 1 : 0);
                        html.Append(HeaderTable(dt));

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i % 35 == 0 && i != 0)
                            {
                                page++;
                                html.Append("</table></div>");
                                html.Append(HeaderTable(dt));
                            }

                            html.Append(DataRow(dt.Rows[i]));
                        }
                        ph1.Controls.Add(new Literal { Text = html.ToString() });
                    }
                    else
                    {
                        Alert("No Records Found");
                    }
                }
                else
                {
                    Alert("No Records Found");
                }






            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }

        }



        protected DataTable GetDemanifestPrint(string date)
        {/*
        string sqlString = "selecT m.origin,\n" +
        "       b.name OriginName,\n" +
        "       m.destination,\n" +
        "       b1.name DestName,\n" +
        "       m.manifestNumber,\n" +
        "       CAST(CAST(m.createdon as date) as varchar) Mdate,\n" +
        "       COUNT(cm.consignmentNumber) CNCOUNT\n" +
        "  from Mnp_Manifest m\n" +
        " inner join Mnp_ConsignmentManifest cm\n" +
        "    on m.manifestNumber = cm.manifestNumber\n" +
        " inner join branches b\n" +
        "    on b.branchCode = m.origin\n" +
        " inner join branches b1\n" +
        "    on b1.branchCode = m.destination\n" +
        " where CAST(m.createdOn as date) = '" + date + "'\n" +
        "   and m.destination = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
        "   and m.isDemanifested = '1'\n" +
        "\n" +
        " group by m.origin,\n" +
        "          b.name,\n" +
        "          m.destination,\n" +
        "          b1.name,\n" +
        "          m.manifestNumber,\n" +
        "          CAST(m.createdon as date)\n" +
        " order by m.manifestNumber";
        */

            string sqlString = "select d.id,\n" +
            "       d.BagNumber,\n" +
            "       b.name BagOrigin,\n" +
            "       b2.name BagDestination,\n" +
            "       COUNT(dm.manifestNumber) TotalManifestCount, d.Date dateCreated\n" +
            "  from MnP_Debag d\n" +
            " left outer join MnP_DebagManifest dm\n" +
            "    on dm.DebagID = d.id\n" +
            " inner join branches b\n" +
            "    on b.branchCode = d.origin\n" +
            " inner join branches b2\n" +
            "    on b2.branchCode = d.destination\n" +
            " where CAST(d.CreatedON as date)  = '" + date + "' and d.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            " group by d.id, d.BagNumber,\n" +
            "    b.name ,\n" +
            "    b2.name ,d.Date";


            sqlString = "SELECT A.id,\n" +
            "       A.BagNumber,\n" +
            "       A.BagOrigin,\n" +
            "       A.BagDestination,\n" +
            "       A.dateCreated,\n" +
            "       SUM(A.TotalManifestCount) TotalManifestCount,\n" +
            "       SUM(A.TotalOutpieceCount) TotalOutpieceCount\n" +
            "  FROM (\n" +
            "\n" +
            "        select d.id,\n" +
            "                d.BagNumber,\n" +
            "                b.name BagOrigin,\n" +
            "                b2.name BagDestination,\n" +
            "                COUNT(dm.manifestNumber) TotalManifestCount,\n" +
            "                '' TotalOutpieceCount,\n" +
            "                d.Date dateCreated\n" +
            "          from MnP_Debag d\n" +
            "          left outer join MnP_DebagManifest dm\n" +
            "            on dm.DebagID = d.id\n" +
            "         inner join branches b\n" +
            "            on b.branchCode = d.origin\n" +
            "         inner join branches b2\n" +
            "            on b2.branchCode = d.destination\n" +
            "         where CAST(d.CreatedON as date) = '" + date + "' and d.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "         group by d.id, d.BagNumber, b.name, b2.name, d.Date\n" +
            "\n" +
            "        UNION\n" +
            "        select d.id,\n" +
            "               d.BagNumber,\n" +
            "               b.name BagOrigin,\n" +
            "               b2.name BagDestination,\n" +
            "               '' TotalManifestCount,\n" +
            "               COUNT(dm.outpieceNumber) TotalOutpieceCount,\n" +
            "               d.Date dateCreated\n" +
            "          from MnP_Debag d\n" +
            "          left outer join MnP_DebagOutPieces dm\n" +
            "            on dm.DebagID = d.id\n" +
            "         inner join branches b\n" +
            "            on b.branchCode = d.origin\n" +
            "         inner join branches b2\n" +
            "            on b2.branchCode = d.destination\n" +
            "         where CAST(d.CreatedON as date) = '" + date + "' and d.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "         group by d.id, d.BagNumber, b.name, b2.name, d.Date) A\n" +
            "\n" +
            " group by A.id, A.BagNumber, A.BagOrigin, A.BagDestination, A.dateCreated";


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
                pageBreak = "style:'page-break-before: always;'";
            }

            string sqlString = "<div " + pageBreak + "><table style=\"width: 100%; border: 2px Solid Black; border-bottom: 1px Solid Black;  margin-top:10px;\n" +
            "            border-collapse: collapse; font-family: Calibri; font-size: medium;\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 100%; font-size: x-large; font-variant: small-caps; text-align: center;\n" +
            "                    border-bottom: 2px Solid Black\" colspan=\"5\">\n" +
            "                    <b>De-Bagging Summary</b>\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    <b>User</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    " + HttpContext.Current.Session["U_NAME"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    <b>Print Date Time</b>\n" +
            "                </td>\n" +
            "                <td colspan='2' style=\"width: 25%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Date</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Sub Total De-Bagging (Count)</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Sub Total Manifest (Count)</b>\n" +
            "                </td>\n" +
            "                <td colspan='2' style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    Page\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + dt.Rows[0]["dateCreated"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + dt.Rows.Count.ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + dt.Compute("SUM(TotalManifestCount)", "").ToString() + "\n" +
            "                </td>\n" +
            "                <td colspan='2' style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + page + "/" + totalPages + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Bag #</b>\n" +
            "                </td>\n" +
            "                <td colspan='2' style=\"width: 25%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Total Manifest Count</b>\n" +
            "                </td>\n" +
            "            </tr>\n";



            return sqlString;
        }
        protected string DataRow(DataRow dr)
        {

            string sqlString = "<tr>\n" +
            "                <td style=\"width: 20%; text-align: left; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["BagOrigin"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["BagDestination"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: left; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <a href=\"PrintDebugDetails.aspx?id=" + dr["id"].ToString() + "\" target=\"_blank\">" + dr["BagNumber"].ToString() + "</a>\n" +
            "                </td>\n" +
            "                <td style=\"width: 12.5%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["TotalManifestCount"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 12.5%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["TotalOutpieceCount"].ToString() + "\n" +
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