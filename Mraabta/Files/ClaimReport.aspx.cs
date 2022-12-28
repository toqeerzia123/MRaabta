using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Configuration;

namespace MRaabta.Files
{
    public partial class ClaimReport : System.Web.UI.Page
    {
        string startDate_ = ""; string endDate_ = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public DataTable getNewRequestByDate(string Fromdate, string Todate, string condition)
        {
            DataTable ds = new DataTable();

            string sql = " \n"
               + "SELECT cnr.nr_ID, \n"
               + "       cnr.nr_CreatedDate, \n"
               + "       cd.dept_name, \n"
               + "       b1.name                origin, \n"
               + "       b2.name                destination, \n"
               + "       cnr.shipper, \n"
               + "       cnr.[weight], \n"
               + "       cnr.pieces, \n"
               + "       cnr.PaymentMode, \n"
               + "       crn.rn_name            RequestNature, \n"
               + "       cu.[User_Name]         CreatedBy, \n"
               + "       cnr.ConsignmentNum, \n"
               + "       cnr.nr_account accountNo, \n"
               + "       cit.it_name            Inquirer, \n"
               + "       b2.name                destination, \n"
               + "       cnr.nr_sourceMedia     Media, \n"
               + "       ccs.ClaimStatus, \n"
               + "       cz.name                AllocationZone, \n"
               + "       cda.DefaulterName      DefaulterZone, \n"
               + "       cnr.nr_modifiedDate, \n"
               + "       cnr.ShipmentContents, \n"
               + "       ccc.CommodityName      Commodity, \n"
               + "       cct.ClaimTypeName      ClaimType, \n"
               + "       cnr.ClaimAsked, \n"
               + "       cnr.NegotiatedAmount, \n"
               + "       cda.DefaulterName      DefaulterName, \n"
               + "       ccp.ProductName        ProductName, \n"
               + "       c.bookingDate , \n"
               + "       ccd.dept_name as ClaimDept, \n"
               + "       cre.region, \n"
               + "       ccr.requirements,\n"
               + "       cnr.ClaimCasePendingCode,  \n"
               + " (select top 1 i.invoiceNumber from Invoice i inner \n"
               + "          join InvoiceConsignment ic on i.invoiceNumber = ic.invoiceNumber \n"
               + "          where i.IsInvoiceCanceled = 0 \n"
               + "          and ic.consignmentNumber = cnr.ConsignmentNum) as invoiceNumber \n"
               + "  FROM   CSD_NewRequest_history cnr \n"
               + "       INNER JOIN CSD_Users cu \n"
               + "            ON  cnr.nr_createdBy = cu.[User_ID] \n"
               + "       INNER JOIN CSD_InquirerType cit \n"
               + "            ON  cit.it_ID = cnr.InquirerType \n"
               + "       INNER JOIN Branches b1 \n"
               + "            ON  b1.branchCode = cnr.nr_origin \n"
               + "       INNER JOIN Branches b2 \n"
               + "            ON  b2.branchCode = cnr.nr_destination \n"
               + "       INNER JOIN CSD_RequestNature crn \n"
               + "            ON  crn.rn_ID = cnr.rn_ID \n"
               + "       INNER JOIN CSD_RequestType crt \n"
               + "            ON  crt.rt_ID = cnr.RequestType \n"
               + "       INNER JOIN CSD_RequestActions cra \n"
               + "            ON  cra.statusID = cnr.nr_status \n"
               + "       INNER JOIN CSD_department cd \n"
               + "            ON  cd.dept_ID = cnr.nr_dept \n"
               + "       LEFT JOIN CSD_Users cu1 \n"
               + "            ON  cnr.nr_AssignedTo = cu1.[User_ID] \n"
               + "       LEFT JOIN CSD_DefaulterArea cda \n"
               + "            ON  cda.DefaulterID = cnr.nr_DefaulterArea \n"
               + "       LEFT JOIN CSD_ClaimStatus ccs \n"
               + "            ON  cnr.ClaimStatusID = ccs.ClaimStatusID \n"
               + "       LEFT JOIN CSD_ClaimDepartment ccd \n"
               + "            ON cnr.ClaimDeptID = ccd.claim_dept_ID \n"
               + "       LEFT JOIN CSD_ClaimRegion cre \n"
               + "            On cnr.ClaimRegionID = cre.claim_region_ID \n"
               + "       LEFT JOIN CSD_ClaimRequirements ccr \n"
               + "             On cnr.ClaimRequirementID = ccr.claim_requirement_ID \n"
               + "       INNER JOIN CSD_Zones cz \n"
               + "            ON  CONVERT(VARCHAR(10), cnr.nr_allocationZone) = cz.zoneCode \n"
               + "       LEFT JOIN CSD_ClaimCommodity ccc \n"
               + "            ON  cnr.CommodityID = ccc.CommodityID \n"
               + "       LEFT JOIN CSD_ClaimType cct \n"
               + "            ON  cnr.ClaimTypeID = cct.ClaimTypeID \n"
               + "       INNER JOIN Consignment c \n"
               + "            ON  c.consignmentNumber = cnr.ConsignmentNum \n"
               + "       LEFT JOIN CSD_ClaimProducts ccp \n"
               + "            ON  cnr.productID = ccp.ProductID \n"
               + "WHERE  CONVERT(date, cnr.nr_CreatedDate) BETWEEN '" + Fromdate + "' AND '" + Todate + "' \n"
               + "       AND cnr.rn_ID = '3' \n"
               + "       AND ISNULL(cnr.nr_modifiedDate, '') = ( \n"
               + "               SELECT ISNULL(MAX(nr_modifiedDate), '') \n"
               + "               FROM   CSD_NewRequest_history cnr2 \n"
               + "               WHERE  cnr2.nr_id = cnr.nr_id \n"
               + "           ) \n"
               + "ORDER BY \n"
               + "       cnr.nr_ID           ASC, \n"
               + "       cra.ra_Name            DESC";

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                SqlCommand orcd = new SqlCommand(sql, con);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }


        protected void btn_save_Click(object sender, EventArgs e)
        {

            if (startDate.Text == "" || endDate.Text == "")
            {
                if (startDate.Text == "")
                {
                    Errorid.Text = "Please provide start date! ";
                    return;
                }
                if (endDate.Text == "")
                {
                    Errorid.Text = "Please provide end date! ";
                    return;
                }
            }
            startDate_ = DateTime.Parse(startDate.Text).ToString("yyyy-MM-dd");
            endDate_ = DateTime.Parse(endDate.Text).ToString("yyyy-MM-dd");

            //string Profile_ID = Session["Profile_ID"].ToString();
           // string Fromdate = Request.Form[hfName.UniqueID];
           // string Todate = Request.Form[hfName1.UniqueID];
            string condition = "";
            //if (Profile_ID == "1" || Profile_ID == "2")
            //{
            //    condition = "AND cnr.nr_AssignedTo = '" + Session["User_ID"].ToString() + "'";
            //}
            //else if (Profile_ID == "3")
            //{
            //    condition = "AND cu1.Agent_ID in ( select Agent4_ID from CSD_LevelAgent_4 where Agent3_ID = '" + Session["Agent_ID"].ToString() + "' and Designation = 'Backline')";
            //}
            //else if (Profile_ID == "4")
            //{
            //    condition = "AND cu1.Agent_ID in ( select Agent4_ID from CSD_LevelAgent_4 where Agent2_ID = '" + Session["Agent_ID"].ToString() + "' and Designation = 'Backline')";
            //}

            if (startDate_ != "" && endDate_ != "")
            {
                
                DataTable dt_resocrds = getNewRequestByDate(startDate_, endDate_, condition);

                if (dt_resocrds.Rows.Count > 0)
                {
                    GV_Histroy.DataSource = dt_resocrds;
                    GV_Histroy.DataBind();
                    Gv_CSV.DataSource = dt_resocrds.DefaultView;
                    Gv_CSV.DataBind();
                    //td_excell.Visible = true;
                    //td_csv.Visible = true;
                }
                else
                {
                    GV_Histroy.DataSource = null;
                    GV_Histroy.DataBind();
                    Gv_CSV.DataSource = null;
                    Gv_CSV.DataBind();
                    //td_excell.Visible = false;
                    //td_csv.Visible = false;
                    Response.Write("<script>alert('No records found.');</script>");
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "onSearchClick()", true);
            }
            else
            {
                Response.Write("<script>alert('Kindly enter dates.');</script>");
                GV_Histroy.DataSource = null;
                GV_Histroy.DataBind();
                //td_excell.Visible = false;
                //td_csv.Visible = true;
            }
        }

        protected void GV_Histroy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Histroy.PageIndex = e.NewPageIndex;
            btn_save_Click(sender, e);
        }

        protected void GV_Histroy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}