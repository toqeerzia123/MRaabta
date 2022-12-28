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
    public partial class PrintDemanifest : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        int page = 1;
        int totalPages = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //ph1.Controls.Add(new Literal { Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") });
            Print_Demanifest();
        }

        protected void Print_Demanifest()
        {
            try
            {
                string date = Request.QueryString["Date"].ToString();

                DataTable dt = GetDemanifestPrint(date);

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
            catch (Exception ex)
            {
                Alert(ex.Message);
            }

        }



        protected DataTable GetDemanifestPrint(string date)
        {
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
            "                    border-bottom: 2px Solid Black\" colspan=\"4\">\n" +
            "                    <b>Demanifest Detail</b>\n" +
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
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-bottom: 2px Solid Black\">\n" +
            "                    " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Date</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Demanifest Count</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>CN Count</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    Page\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + dt.Rows[0]["Mdate"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + dt.Rows.Count.ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + dt.Compute("SUM(CNCOUNT)", "").ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
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
            "                    <b>Manifest Number</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Consignment Count</b>\n" +
            "                </td>\n" +
            "            </tr>\n";



            return sqlString;
        }
        protected string DataRow(DataRow dr)
        {

            string sqlString = "<tr>\n" +
            "                <td style=\"width: 20%; text-align: left; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["OriginName"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["DestName"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: left; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["ManifestNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <a href=\"PrintDemanifestDetails.aspx?Xcode=" + dr["ManifestNumber"].ToString() + "\" target=\"_blank\">" + dr["CNCOUNT"].ToString() + "</a>\n" +
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