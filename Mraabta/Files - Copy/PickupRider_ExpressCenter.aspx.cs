using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Text;

namespace MRaabta.Files
{
    public partial class PickupRider_ExpressCenter : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        String end_date_tostring = "";
        String from_date_tostring = "";
        StringBuilder sb;
        string branch, zone, rider;
        String SessionBranch = "";
        String SessionZone = "", SessionExpressC, U_ID = "";
        DataTable ds_riders = new DataTable();
        DataTable ds_Express = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SessionBranch = Session["BRANCHCODE"].ToString();
                SessionZone = Session["ZONECODE"].ToString();
                SessionExpressC = Session["ExpressCenter"].ToString();
                U_ID = Session["U_ID"].ToString();
                if (!IsPostBack)
                {
                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                HyperLink lblTicketBookingEC = (HyperLink)e.Row.FindControl("lblTicketBookingEC");
                if (lblTicketBookingEC != null)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    String ticketNumber = drv["ticketNumber"].ToString();
                    if (lblTicketBookingEC != null)
                    {
                        lblTicketBookingEC.NavigateUrl = "DiscountBookingEC.aspx?TicketNumber=" + ticketNumber;
                    }
                }
            }
            catch (Exception er)
            {

            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                //if (dd_start_date.Text.Length <= 0 || dd_end_date.Text.Length <= 0)
                //{
                //    error_msg.Text = "Please select start and end date!";
                //    gg_CustomerLedger_Month.DataSource = null;
                //    gg_CustomerLedger_Month.DataBind();

                //}

                //else
                {
                    //else
                    {

                        //if (dd_zone.SelectedValue != "")
                        {
                            //if (type.SelectedValue == "html")
                            {
                                //clvar = new Variable();

                                //String ZoneID = "", ZoneName = "";


                                //for (int i = 0; i < dd_zone.Items.Count; i++)
                                //{
                                //    if (dd_zone.Items[i].Selected)
                                //    {
                                //        ZoneName += dd_zone.Items[i].Text + ",";
                                //        ZoneID += dd_zone.Items[i].Value + ",";
                                //    }
                                //}
                                //if (ZoneName != "")
                                //{
                                //    ZoneName = ZoneName.Remove(ZoneName.Length - 1);
                                //    ZoneName.ToString();

                                //    ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                                //    ZoneID.ToString();
                                //}
                                //if (dd_zone.SelectedValue != "")
                                //{
                                //    zone = ZoneID.ToString();
                                //}

                                ////if (branch_chk.Checked)
                                ////{
                                ////    branch = "";
                                ////    zone = ZoneID.ToString();
                                ////}
                                ////else
                                //{
                                //    String BranchId = "", BranchName = "";

                                //    for (int i = 0; i < dd_branch.Items.Count; i++)
                                //    {
                                //        if (dd_branch.Items[i].Selected)
                                //        {
                                //            BranchName += "'" + dd_branch.Items[i].Text + "',";
                                //            BranchId += "'" + dd_branch.Items[i].Value + "',";
                                //        }
                                //    }

                                //    if (BranchId != "")
                                //    {
                                //        BranchId = BranchId.Remove(BranchId.Length - 1);
                                //        BranchId.ToString();
                                //    }

                                //    if (dd_branch.SelectedValue != "")
                                //    {
                                //        branch = BranchId.ToString();
                                //        zone = "";
                                //    }

                                //    else
                                //    {
                                //        branch = "";
                                //        zone = ZoneID.ToString();
                                //    }
                                //}

                                clvar._TownCode = SessionBranch;
                                clvar._Zone = SessionZone;
                                clvar.RiderCode = rider;


                                DataTable Year_check = GetPickupData(clvar);

                                gg_CustomerLedger_Month.Visible = true;
                                lbl_total_record.Text = "";

                                if (Year_check.Rows.Count != 0)
                                {

                                    //lbl_msg.Text = "<b>First Time Out</b>";
                                    error_msg.Text = "";
                                    lbl_total_record.Text = "Total Record: " + Year_check.Rows.Count.ToString();

                                    gg_CustomerLedger_Month.DataSource = Year_check.DefaultView;
                                    gg_CustomerLedger_Month.DataBind();

                                }
                                else
                                {
                                    gg_CustomerLedger_Month.Visible = false;
                                    lbl_total_record.Visible = false;
                                    error_msg.Text = "No Record Found...";
                                }
                            }

                        }

                        //if (type.SelectedValue == "CSV")
                        //{
                        //    String ZoneID = "", ZoneName = "";

                        //    for (int i = 0; i < dd_zone.Items.Count; i++)
                        //    {
                        //        if (dd_zone.Items[i].Selected)
                        //        {
                        //            ZoneName += dd_zone.Items[i].Text + ",";
                        //            ZoneID += dd_zone.Items[i].Value + ",";
                        //        }
                        //    }
                        //    if (ZoneName != "")
                        //    {
                        //        ZoneName = ZoneName.Remove(ZoneName.Length - 1);
                        //        ZoneName.ToString();

                        //        ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                        //        ZoneID.ToString();
                        //    }
                        //    String BranchId = "", BranchName = "";
                        //    if (branch_chk.Checked)
                        //    {
                        //        branch = "";
                        //        zone = ZoneID.ToString();
                        //    }
                        //    else
                        //    {

                        //        for (int i = 0; i < dd_branch.Items.Count; i++)
                        //        {
                        //            if (dd_branch.Items[i].Selected)
                        //            {
                        //                BranchName += dd_branch.Items[i].Text + ",";
                        //                BranchId += dd_branch.Items[i].Value + ",";
                        //            }
                        //        }

                        //        if (BranchId != "")
                        //        {
                        //            BranchId = BranchId.Remove(BranchId.Length - 1);
                        //            BranchId.ToString();
                        //        }
                        //        if (dd_branch.SelectedValue != "")
                        //        {
                        //            zone = "";
                        //            branch = BranchId.ToString();
                        //        }
                        //    }

                        //    clvar._TownCode = branch;
                        //    clvar._Zone = zone;
                        //    ExportToCSVOriginal(sender, e);
                        //}
                    }
                }
            }
            catch (Exception err)
            {
                gg_CustomerLedger_Month.Visible = false;
                lbl_total_record.Visible = false;
                error_msg.Text = "No Record Found...";
            }
        }

        public DataTable GetPickupData(Variable clvar)
        {

            DataTable ds = new DataTable();
            try
            {
                String queryCondition = ""; String ReportType_ = "";
                string expresscentrCondition = "";

                queryCondition = "  mpb.orgin in (" + clvar._TownCode + ") and mpb.expressCenterCode = '" + SessionExpressC + "'";

                expresscentrCondition = "   ec.bid IN (" + clvar._TownCode + ")";
                //if (rb_Booked.Checked)
                //{
                //    ReportType_ = " AND isnull(mpb.isBooked,'0')='1' ";
                //}
                //else if (rb_NoResponse.Checked)
                //{
                //    ReportType_ = " AND isnull(mpb.isBooked,'0')='0' AND mpb.remarks IS NOT null ";
                //}
                //else if (rb_Pending.Checked)
                {
                    ReportType_ = " AND isnull(mpb.isBooked,'0')='0'  ";
                }
                string sql = "SELECT mpb.ticketNumber, \n"
               + "       mpb.consigner, \n"
               + "       mpb.consignee, \n"
               + "       mpb.consigneraddress, \n"
               + "       mpb.consignerCellNo, \n"
               + "       mpb.pieces,mpb.expressCenterCode,mpb.reason, \n"
               + "       mpb.weight,mpb.RiderCode,ec.name+'-'+ec.expressCenterCode expressCenter,Convert(varchar,mpb.pickupScheduled,5) pickupScheduled, \n"
               + "       case when mpb.scheduledService=0 THEN 'Pickup' WHEN  mpb.scheduledService=1 THEN 'Drop' when mpb.scheduledService=2 THEN 'Inquiry' end scheduledService  "
               + "FROM   MNP_PreBookingConsignment mpb \n"
               + "       INNER JOIN branches b1 \n"
               + "            ON  b1.branchCode = mpb.orgin \n"
               + "       INNER JOIN branches b2 \n"
               + "            ON  b2.branchCode = mpb.destination"
               + "       left JOIN ExpressCenters ec "
               + "             ON ec.expressCenterCode = mpb.expressCenterCode "
               + " where   "
                 + queryCondition + ReportType_ + "  order by mpb.createdOn desc ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);

                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();

            }
            catch (Exception Err)
            {
                //throw Err;
                return null;
            }
            finally
            { }
            return ds;
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gg_CustomerLedger_Month.PageIndex = e.NewPageIndex;
            Btn_Search_Click(sender, e);

        }
    }
}