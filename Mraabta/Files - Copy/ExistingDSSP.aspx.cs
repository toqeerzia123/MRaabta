using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MRaabta.Files
{
    public partial class ExistingDSSP : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        String branchCode, ZoneCode, ECCode, ExpressCName, UserName, U_ID, BookingCode_ = "", shift = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            BookingCode_ = Session["BookingStaff"].ToString();
            branchCode = Session["BRANCHCODE"].ToString();
            ZoneCode = Session["ZONECODE"].ToString();
            ECCode = Session["ExpressCenter"].ToString();
            ExpressCName = Session["EXPRESSCENTERNAME"].ToString();
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "";
                string zoneCondition = " AND mmrd.ZoneCode in (" + ZoneCode + ") ";
                string branchCondition = " AND mmrd.BranchCode in (" + branchCode + ") ";
                string ecCondition = "  AND ec.expressCenterCode='" + ECCode + "' ";
                string bookingCodeCondition = "AND mmrd.BookingCode='" + BookingCode_ + "' ";
                if (ECCode == "ALL" || ECCode == "")
                {
                    ecCondition = "";
                }
                if (branchCode == "ALL")
                {
                    branchCondition = "";
                }
                if (ZoneCode == "ALL")
                {
                    zoneCondition = "";
                }

                if (DateRadio.Checked)
                {
                    if (CreationDate.Text == "")
                    {
                        statuslbl.Text = "Please provide date.";
                        return;
                    }
                    DateTime dtime = DateTime.Parse(CreationDate.Text);
                    String formattedDate = dtime.ToString("yyyy-MM-dd");

                    sql = "SELECT mmrd.DSSPNumber, \n"
                      + "	   z.name zone, \n"
                      + "       b.name branch, \n"
                      + "       ec.name expressCenter, \n"
                      + "       mmrd.BookingCode, \n"
                      + "       mmrd.CNCount, \n"
                      + "       Round(mmrd.TotalAmount,0) TotalAmount, \n"
                      + "       Round(mmrd.CollectAmount,0) CollectAmount, \n"
                      + "       mmrd.BookingShift, \n"
                      + "       Convert(varchar,mmrd.CreatedOn,103) CreatedOn \n"
                      + "FROM   MNP_Master_Retail_DSSP mmrd \n"
                      + "INNER JOIN Branches b ON b.branchCode=mmrd.BranchCode \n"
                      + "INNER JOIN Zones z ON z.zoneCode=mmrd.ZoneCode \n"
                      + "INNER JOIN ExpressCenters ec ON ec.ExpressCenterCode = mmrd.ExpressCenterCode \n"
                      + "WHERE  CAST(mmrd.CreatedOn AS date) = CAST('" + formattedDate + "' AS date) \n"
                      + zoneCondition + branchCondition + ecCondition + bookingCodeCondition
                      + "ORDER BY ec.createdOn";
                }
                else if (NumberRadio.Checked)
                {
                    if (DSSPIdTxtbox.Text == "")
                    {
                        statuslbl.Text = "Please provide DSSPNumber.";
                        return;
                    }
                    sql = "SELECT mmrd.DSSPNumber, \n"
                       + "	   z.name zone, \n"
                       + "       b.name branch, \n"
                       + "       ec.name expressCenter, \n"
                       + "       mmrd.BookingCode, \n"
                       + "       mmrd.CNCount, \n"
                      + "       Round(mmrd.TotalAmount,0) TotalAmount, \n"
                      + "       Round(mmrd.CollectAmount,0) CollectAmount, \n"
                       + "       mmrd.BookingShift, \n"
                       + "       Convert(varchar,mmrd.CreatedOn,103) CreatedOn \n"
                       + "FROM   MNP_Master_Retail_DSSP mmrd \n"
                       + "INNER JOIN Branches b ON b.branchCode=mmrd.BranchCode \n"
                       + "INNER JOIN Zones z ON z.zoneCode=mmrd.ZoneCode \n"
                       + "INNER JOIN ExpressCenters ec ON ec.ExpressCenterCode = mmrd.ExpressCenterCode \n"
                       + "WHERE  mmrd.DSSPNumber = '" + DSSPIdTxtbox.Text + "' \n"
                       + zoneCondition + branchCondition + ecCondition + bookingCodeCondition;
                }
                else
                {
                    statuslbl.Text = "Please select search option";
                    return;
                }

                DataTable dt = new DataTable();
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter dr;
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                ConsignmentTable.DataSource = dt;
                ConsignmentTable.DataBind();
            }
            catch (Exception er)
            {
            }
            finally
            {
                con.Close();
            }
        }

        protected void searchBy_CheckedChanged(object sender, EventArgs e)
        {
            if (NumberRadio.Checked)
            {
                datediv.Visible = false;
                NumberDiv.Visible = true;

            }
            else
            {
                NumberDiv.Visible = false;
                datediv.Visible = true;

            }
        }
    }
}