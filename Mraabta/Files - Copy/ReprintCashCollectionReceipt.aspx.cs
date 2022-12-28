using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class ReprintCashCollectionReceipt : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        string branchCode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                branchCode = Session["BRANCHCODE"].ToString();
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "";
                sql = @" SELECT pv.id VoucherNo, PV.refno DSSP,ec.name EC,b.name Branch,pv.Amount,mmrd.BookingCode FROM 
                                PaymentVouchers pv
                                INNER JOIN ExpressCenters ec ON ec.expressCenterCode=pv.ExpressCenterCode
                                INNER JOIN Branches b ON b.branchCode=pv.BranchCode
                                INNER JOIN MNP_Master_Retail_DSSP mmrd ON mmrd.DSSPNumber=pv.RefNo ";
                if (DateRadio.Checked)
                {
                    if (CreationDate.Text == "")
                    {
                        statuslbl.Text = "Please provide voucher date.";
                        return;
                    }

                    sql += " WHERE   pv.Amount>0 AND CAST(pv.VoucherDate AS date)='" + CreationDate.Text + "'   ";
                }
                else if (NumberRadio.Checked)
                {
                    if (RRNumberTxt.Text == "")
                    {
                        statuslbl.Text = "Please provide voucher number.";
                        return;
                    }
                    sql += "  WHERE  pv.Amount>0  AND  id=" + RRNumberTxt.Text;
                }
                else if (CashierIdRadio.Checked)
                {
                    if (cashierIdTxt.Text == "")
                    {
                        statuslbl.Text = "Please provide Cashier Id.";
                        return;
                    }
                    sql += "    INNER JOIN ZNI_USER1 zu ON   pv.CreatedBy=zu.U_ID  WHERE CAST(pv.CreatedOn AS date)>='2020-09-01' AND zu.U_NAME = '" + cashierIdTxt.Text + "' ";

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
                DateDiv.Visible = false;
                NumberDiv.Visible = true;
                CashierDiv.Visible = false;

            }
            else if (DateRadio.Checked)
            {
                NumberDiv.Visible = false;
                DateDiv.Visible = true;
                CashierDiv.Visible = false;

            }
            else
            {
                NumberDiv.Visible = false;
                DateDiv.Visible = false;
                CashierDiv.Visible = true;
            }
        }
    }
}