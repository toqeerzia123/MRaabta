using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using MRaabta.App_Code;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace MRaabta.Files
{
    public partial class FollowUpPickup : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_dateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txt_dateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //txt_dateFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //txt_dateTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //string datefrom = txt_dateFrom.Text;
                //string dateto = txt_dateTo.Text;
            }
        }
        protected void btn_generate_Click(object sender, EventArgs e)
        {
            Call();
        }

        protected void Call()
        {
            string datefrom1 = txt_dateFrom.Text;
            string dateto1 = txt_dateTo.Text;
            string option = "";

            if (rb_report.SelectedValue == "0")
            {
                if (txt_input.Text.Trim() == "")
                {
                    option = "";
                }
                else
                {
                    option = " and pd.accountno = '" + txt_input.Text + "'";
                }
            }

            if (rb_report.SelectedValue == "1")
            {
                if (txt_input.Text.Trim() == "")
                {
                    option = "";
                }
                else
                {
                    option = " and pd.riderCode = '" + txt_input.Text + "'";
                }
            }

            DataSet ds = new DataSet();
            ds = Get_cod_summary(datefrom1, dateto1, option);


            if (ds.Tables[0].Rows.Count > 0)
            {
                rp_Pickups.DataSource = ds;
                rp_Pickups.DataBind();
                Errorid.Text = "";
            }
            else
            {
                rp_Pickups.DataSource = null;
                rp_Pickups.DataBind();
                Errorid.Text = "No Pickups found!!";
                //Response.Write("<script>alert('No Consignments found!!')</script>");
                return;
            }
        }
        public DataSet Get_cod_summary(string datefrom, string dateto, string option)
        {
            DataSet ds = new DataSet();
            try
            {
                string sqlString = "select pd.riderCode,\n" +
                "       (r.firstName + ' ' + r.lastName) RiderName,\n" +
                "       b.name Origin,\n" +
                "       pd.riderPhone,\n" +
                "       pd.weight,\n" +
                "       pd.pieces,\n" +
                "       pd.pickup_Date,\n" +
                "       pd.pickup_status,\n" +
                "       ps.StatusDescription,\n" +
                "       pd.createdOn, pd.id, pd.remarks, pd.alternate_Address, pd.createdOn\n" +
                "  from Pickup_Details pd,\n" +
                "       CreditClients  cc,\n" +
                "       Riders         r,\n" +
                "       Branches       b,\n" +
                "       Pickup_Status  ps\n" +
                " where pd.AccountNo = cc.accountNo\n" +
                "   and pd.origin = cc.branchCode\n" +
                "   and pd.riderCode = r.riderCode\n" +
                "   and pd.origin = r.branchId\n" +
                "   and pd.origin = b.branchCode\n" +
                "   and ISNULL(pd.pickup_status,1) = ps.StatusCode\n" +
                "  " + option + "\n" +
                 " and  cast(pd.pickup_date as DATE) >= '" + datefrom + "' and cast(pd.pickup_date as DATE) <='" + dateto + "'\n" +
                " order by pd.pickup_Date";


                sqlString = "select pd.riderCode,\n" +
               "       (r.firstName + ' ' + r.lastName) RiderName,\n" +
               "       b.name Origin,\n" +
               "       pd.riderPhone,\n" +
               "       pd.weight,\n" +
               "       pd.pieces,\n" +
               "       pd.pickup_Date,\n" +
               "       pd.pickup_status,\n" +
               "       ps.StatusDescription,\n" +
               "       pd.createdOn,\n" +
               "       pd.id,\n" +
               "       pd.remarks,\n" +
               "       pd.alternate_Address,\n" +
               "       pd.createdOn,\n" +
               "       cc.accountNo,\n" +
               "       cc.name\n" +
               "  from Pickup_Details pd,\n" +
               "       CreditClients  cc,\n" +
               "       Riders         r,\n" +
               "       Branches       b,\n" +
               "       Pickup_Status  ps\n" +
               " where pd.AccountNo = cc.accountNo\n" +
               "   and pd.origin = cc.branchCode\n" +
               "   and pd.riderCode = r.riderCode\n" +
               "   and pd.origin = r.branchId\n" +
               "   and pd.origin = b.branchCode\n" +
               "   and ISNULL(pd.pickup_status, 1) = ps.StatusCode\n" +
               "  " + option + "\n" +
               "   and cc.branchCode = pd.origin\n" +
               "   and pd.origin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
               " and  cast(pd.pickup_date as DATE) >= '" + datefrom + "' and cast(pd.pickup_date as DATE) <='" + dateto + "'\n" +
               " order by pd.pickup_Date";

                sqlString = "SELECT pd.riderCode,\n" +
                "       (r.firstName + ' ' + r.lastName) RiderName,\n" +
                "       b.name Origin,\n" +
                "       pd.riderPhone,\n" +
                "       pd.weight,\n" +
                "       pd.pieces,\n" +
                "       pd.pickup_Date,\n" +
                "       pd.pickup_status,\n" +
                "       ps.StatusDescription,\n" +
                "       pd.createdOn,\n" +
                "       pd.id,\n" +
                "       pd.remarks,\n" +
                "       pd.alternate_Address,\n" +
                "       pd.createdOn,\n" +
                "       cc.accountNo,\n" +
                "       cc.name\n" +
                "  FROM Pickup_Details pd\n" +
                " INNER JOIN CreditClients cc\n" +
                "    ON pd.AccountNo = cc.accountNo\n" +
                "   AND pd.origin = cc.branchCode\n" +
                " INNER JOIN Riders r\n" +
                "    ON pd.riderCode = r.riderCode\n" +
                "   AND pd.origin = r.branchId\n" +
                " INNER JOIN Branches b\n" +
                "    ON pd.origin = b.branchCode\n" +
                " INNER JOIN Pickup_Status ps\n" +
                "    ON ISNULL(pd.pickup_status, 1) = ps.StatusCode\n" +
                " WHERE pd.origin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
                "   AND cast(pd.pickup_date as DATE) >= '" + datefrom + "'\n" +
                "   AND cast(pd.pickup_date as DATE) <= '" + dateto + "'\n" +
                "  " + option + "\n" +
                " ORDER BY  StatusDescription  desc";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }




        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void rp_Pickups_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trID");
                    DataRowView drv = e.Item.DataItem as DataRowView;

                    RepeaterItem item = e.Item;
                    Label lbldate = item.FindControl("lblStatusDescription") as Label;
                    if (lbldate.Text == "Done")
                    {
                        tr.Attributes.Add("style", "background-color:greenyellow; ");
                    }
                    else if (lbldate.Text == "In Process")
                    {
                        tr.Attributes.Add("style", "background-color:yellow; ");
                    }

                }
            }
            catch (Exception er)
            {

            }
        }
    }
}