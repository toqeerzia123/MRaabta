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
    public partial class PrintDemanifestDetails : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        int page = 1;
        int totalPages = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            PrintDemanifest_Details();
        }
        protected void PrintDemanifest_Details()
        {
            try
            {
                string ManifestNumber = Request.QueryString["Xcode"].ToString();

                DataTable dt = GetDeManifestPrintDetails(ManifestNumber);

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

        protected DataTable GetDeManifestPrintDetails(string manifestNumber)
        {

            string sqlString = "select c.consignmentNumber CNNUmber,\n" +
            "       CAST(CAST(c.bookingDate as date) as varchar) bookingDate,\n" +
            "       c.orgin,\n" +
            "       b.name originName,\n" +
            "       b1.name DestName\n" +
            "  from Mnp_ConsignmentManifest cm\n" +
            " inner join Mnp_Manifest m\n" +
            "    on m.manifestNumber = cm.manifestNumber\n" +
            " inner join((select c.*\n" +
            "               from Consignment c\n" +
            "              inner join consignment_ops co\n" +
            "                 on c.consignmentNumber = co.consignmentNumber\n" +
            "             union all\n" +
            "             select co.*\n" +
            "               from Consignment co\n" +
            "              where co.consignmentNumber not in\n" +
            "                    (select c.consignmentNumber from Consignment_ops c)\n" +
            "             union all\n" +
            "             select *, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL \n" +
            "               from Consignment_ops coo\n" +
            "              where coo.consignmentNumber not in\n" +
            "                    (select consignmentNumber from consignment))) c\n" +
            "    on cm.consignmentNumber = c.consignmentNumber\n" +
            " inner join branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " inner join branches b1\n" +
            "    on b1.branchCode = c.destination\n" +
            " where cm.manifestNumber = '" + manifestNumber + "'\n" +
            "   and m.destination = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
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

            string sqlString = "<table style=\"width: 100%; border: 2px Solid Black; border-bottom: 1px Solid Black;\n" +
            "            border-collapse: collapse; font-family: Calibri; font-size: medium;\">\n" +
            "            <tr>\n" +
            "                <td style=\"width: 100%; font-size: x-large; font-variant: small-caps; text-align: center;\n" +
            "                    border-bottom: 2px Solid Black\" colspan=\"4\">\n" +
            "                    <b>Demanifest CN Details</b>\n" +
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
            "                    <b>Manifest</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + Request.QueryString["Xcode"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    <b>Page #</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black;\">\n" +
            "                    " + page + "/" + totalPages + "\n" +
            "                </td>\n" +
            "            </tr>\n" +
            "\n" +
            "            <tr>\n" +
            "                <td style=\"width: 20%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>CN #</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Date</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Origin</b>\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-top: 2px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    <b>Destination</b>\n" +
            "                </td>\n" +
            "            </tr>";

            return sqlString;
        }

        protected string DataRow(DataRow dr)
        {

            string sqlString = "<tr>\n" +
            "                <td style=\"width: 20%; text-align: left; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["CnNumber"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 30%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["BookingDate"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: left; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["OriginName"].ToString() + "\n" +
            "                </td>\n" +
            "                <td style=\"width: 25%; text-align: center; border: 1px Solid Black; border-top: 1px Solid Black;\n" +
            "                    border-bottom: 2px Solid Black;\">\n" +
            "                    " + dr["DestName"].ToString() + "\n" +
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