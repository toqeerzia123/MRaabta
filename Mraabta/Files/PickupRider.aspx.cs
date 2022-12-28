using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using MRaabta.Models;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MRaabta.Files
{
    public partial class PickupRider : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        String end_date_tostring = "";
        String from_date_tostring = "";
        StringBuilder sb;
        string branch, zone, rider;
        String SessionBranch = "";
        String SessionZone = "", U_ID = "";
        DataTable ds_riders = new DataTable();
        DataTable ds_Express = new DataTable();
        DataTable ds_call = new DataTable();
        private JavaScriptSerializer _Serializer = new JavaScriptSerializer();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SessionBranch = Session["BRANCHCODE"].ToString();
                SessionZone = Session["ZONECODE"].ToString();
                U_ID = Session["U_ID"].ToString();
                if (!IsPostBack)
                {
                    Get_Zone();
                    getbranches();


                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }
        public void getbranches()
        {
            String ZoneID = "";
            for (int i = 0; i < dd_zone.Items.Count; i++)
            {

                ZoneID += dd_zone.Items[i].Value + ',';

            }
            ZoneID = ZoneID.Remove(ZoneID.Length - 1);
            ZoneID.ToString();

            clvar._Zone = ZoneID;


            DataSet ds_branch = b_fun.Get_ZonebyBranches2(clvar);

            dd_branch.Items.Clear();
            if (ds_branch.Tables.Count != 0)
            {
                if (ds_branch.Tables[0].Rows.Count != 0)
                {
                    //  dd_branch.Items.Add(new ListItem("Select Branch Name", "0"));
                    dd_branch.DataTextField = "name";
                    dd_branch.DataValueField = "branchCode";
                    dd_branch.DataSource = ds_branch.Tables[0].DefaultView;
                    dd_branch.DataBind();
                }

            }
        }
        protected void branch_SelectedIndexChanged(object sender, EventArgs e)
        {
            String ZoneID = "";
            for (int i = 0; i < dd_zone.Items.Count; i++)
            {
                if (dd_zone.Items[i].Selected)
                {
                    ZoneID += dd_zone.Items[i].Value + ',';
                }
            }
            ZoneID = ZoneID.Remove(ZoneID.Length - 1);
            ZoneID.ToString();

            clvar._Zone = ZoneID;


            DataSet ds_branch = b_fun.Get_ZonebyBranches2(clvar);

            dd_branch.Items.Clear();
            if (ds_branch.Tables.Count != 0)
            {
                if (ds_branch.Tables[0].Rows.Count != 0)
                {
                    //  dd_branch.Items.Add(new ListItem("Select Branch Name", "0"));
                    dd_branch.DataTextField = "name";
                    dd_branch.DataValueField = "branchCode";
                    dd_branch.DataSource = ds_branch.Tables[0].DefaultView;
                    dd_branch.DataBind();
                }
            }
        }

        public void Get_Zone()
        {
            try
            {
                DataSet ds_zone = b_fun.Get_AllZones1(clvar);

                if (ds_zone.Tables[0].Rows.Count != 0)
                {
                    dd_zone.DataTextField = "name";
                    dd_zone.DataValueField = "zoneCode";
                    dd_zone.DataSource = ds_zone.Tables[0].DefaultView;
                    dd_zone.DataBind();
                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }
        //protected void Get_ReportVersion()
        //{
        //    clvar = new Variable();

        //    lbl_report_version.Text = "";

        //    clvar._reportid = reportid;

        //    DataSet ds = b_fun.Get_Report_VersionByReportId(clvar);
        //    if (ds.Tables[0].Rows.Count != 0)
        //    {
        //        lbl_report_version.Text = "Version: " + ds.Tables[0].Rows[0]["version"].ToString();
        //    }
        //}


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (rb_Pending.Checked)
                    {
                        gg_CustomerLedger_Month.Columns[8].Visible = false;
                    }
                    if (rb_NoResponse.Checked)
                    {
                        gg_CustomerLedger_Month.Columns[8].Visible = true;
                    }
                    if (rb_refuse.Checked)
                    {
                        gg_CustomerLedger_Month.Columns[8].Visible = true;
                    }

                    if (rb_NoResponse.Checked || rb_Pending.Checked || rb_refuse.Checked)
                    {
                        HyperLink lblshipment = (HyperLink)e.Row.FindControl("lblshipment");
                        DataRowView drv = (DataRowView)e.Row.DataItem;
                        String ticketNumber = drv["ticketNumber"].ToString();
                        String riderCode_ = drv["riderCode"].ToString();
                        String expressCenterCode_ = drv["expressCenterCode"].ToString();
                        String callStatusCode_ = drv["callStatusCode"].ToString();

                        if (lblshipment != null)
                        {
                            lblshipment.NavigateUrl = "PickupRider_Details.aspx?TicketNumber=" + ticketNumber;
                        }
                        var ddl_exp = (DropDownList)e.Row.FindControl("ddl_expressCenter");
                        ddl_exp.DataSource = ds_Express;
                        ddl_exp.DataTextField = "NAME";
                        ddl_exp.DataValueField = "code";
                        if (expressCenterCode_ == "" || expressCenterCode_ == "0")
                        {
                        }
                        else
                        {
                            ddl_exp.SelectedValue = expressCenterCode_;
                        }
                        ddl_exp.DataBind();
                        ddl_exp.Items.Insert(0, new ListItem("Select Express Center", "0"));

                        ///adding rider dropdown
                        var ddl = (DropDownList)e.Row.FindControl("ddl_rider");
                        ddl.DataSource = ds_riders;
                        ddl.DataTextField = "NAME";
                        ddl.DataValueField = "riderCode";
                        if (riderCode_ != "")
                        {
                            ddl.SelectedValue = riderCode_;
                        }
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("Select Rider", "0"));

                        ///adding call status dropdown
                        var ddl_call = (DropDownList)e.Row.FindControl("ddl_callStatus");
                        ddl_call.DataSource = ds_call;
                        ddl_call.DataTextField = "NAME";
                        ddl_call.DataValueField = "Id";
                        if (callStatusCode_ != "")
                        {
                            ddl_call.SelectedValue = callStatusCode_;
                        }
                        ddl_call.DataBind();
                        ddl_call.Items.Insert(0, new ListItem("Select Call Status", "0"));

                    }
                    else
                    {
                        HyperLink lblshipment = (HyperLink)e.Row.FindControl("lblshipment");
                        lblshipment.ForeColor = System.Drawing.Color.Black;
                        lblshipment.Text = "";
                        var ddl = (DropDownList)e.Row.FindControl("ddl_rider");
                        ddl.Width = 50;

                        var ddl_exp = (DropDownList)e.Row.FindControl("ddl_expressCenter");
                        ddl_exp.Width = 50;
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
                if (dd_start_date.Text.Length <= 0 || dd_end_date.Text.Length <= 0)
                {
                    error_msg.Text = "Please select start and end date!";
                    gg_CustomerLedger_Month.DataSource = null;
                    gg_CustomerLedger_Month.DataBind();
                }
                else
                {
                    DateTime from = Convert.ToDateTime(dd_start_date.Text);
                    from_date_tostring = from.ToString("yyyy-MM-dd");
                    DateTime end = Convert.ToDateTime(dd_end_date.Text);
                    end_date_tostring = end.ToString("yyyy-MM-dd");
                    int Diff = (end.Date - from.Date).Days;
                    if (from.Date > end.Date)
                    {
                        error_msg.Text = "Please enter correct dates!";
                        gg_CustomerLedger_Month.DataSource = null;
                        gg_CustomerLedger_Month.DataBind();
                    }
                    else if (Diff > 61)
                    {
                        error_msg.Text = "Please enter dates within 61 days!";
                        gg_CustomerLedger_Month.DataSource = null;
                        gg_CustomerLedger_Month.DataBind();
                    }
                    else
                    {

                        if (dd_zone.SelectedValue != "")
                        {
                            //if (type.SelectedValue == "html")
                            {
                                clvar = new Variable();

                                String ZoneID = "", ZoneName = "";


                                for (int i = 0; i < dd_zone.Items.Count; i++)
                                {
                                    if (dd_zone.Items[i].Selected)
                                    {
                                        ZoneName += dd_zone.Items[i].Text + ",";
                                        ZoneID += dd_zone.Items[i].Value + ",";
                                    }
                                }
                                if (ZoneName != "")
                                {
                                    ZoneName = ZoneName.Remove(ZoneName.Length - 1);
                                    ZoneName.ToString();
                                    ZoneID = ZoneID.Remove(ZoneID.Length - 1);
                                    ZoneID.ToString();
                                }
                                if (dd_zone.SelectedValue != "")
                                {
                                    zone = ZoneID.ToString();
                                }

                                //if (branch_chk.Checked)
                                //{
                                //    branch = "";
                                //    zone = ZoneID.ToString();
                                //}
                                //else
                                {
                                    String BranchId = "", BranchName = "";

                                    for (int i = 0; i < dd_branch.Items.Count; i++)
                                    {
                                        if (dd_branch.Items[i].Selected)
                                        {
                                            BranchName += "'" + dd_branch.Items[i].Text + "',";
                                            BranchId += "'" + dd_branch.Items[i].Value + "',";
                                        }
                                    }

                                    if (BranchId != "")
                                    {
                                        BranchId = BranchId.Remove(BranchId.Length - 1);
                                        BranchId.ToString();
                                    }

                                    if (dd_branch.SelectedValue != "")
                                    {
                                        branch = BranchId.ToString();
                                        zone = "";
                                    }

                                    else
                                    {
                                        branch = "";
                                        zone = ZoneID.ToString();
                                    }
                                }

                                clvar._TownCode = branch;
                                clvar._Zone = zone;
                                clvar.RiderCode = rider;
                                DataTable Year_check;
                                if (rb_Pending.Checked || rb_NoResponse.Checked || rb_refuse.Checked)
                                {
                                    Year_check = GetPickupData(clvar);
                                    gg_CustomerLedger_Month.Visible = true;
                                    GridviewBooked.Visible = false;
                                    lbl_total_record.Text = "";
                                    if (rb_NoResponse.Checked)
                                    {
                                        gg_CustomerLedger_Month.Columns[8].Visible = true;
                                    }
                                    if (Year_check.Rows.Count != 0)
                                    {
                                        error_msg.Text = "";
                                        lbl_total_record.Text = "Total Record: " + Year_check.Rows.Count.ToString();
                                        gg_CustomerLedger_Month.DataSource = Year_check.DefaultView;
                                        gg_CustomerLedger_Month.DataBind();
                                    }
                                    else
                                    {
                                        GridviewBooked.Visible = false;
                                        gg_CustomerLedger_Month.Visible = false;
                                        lbl_total_record.Visible = false;
                                        error_msg.Text = "No Record Found...";
                                    }
                                }
                                else
                                {
                                    Year_check = GetPickupData(clvar);
                                    GridviewBooked.Visible = true;
                                    gg_CustomerLedger_Month.Visible = false;

                                    lbl_total_record.Text = "";
                                    if (Year_check.Rows.Count != 0)
                                    {
                                        error_msg.Text = "";
                                        lbl_total_record.Text = "Total Record: " + Year_check.Rows.Count.ToString();
                                        GridviewBooked.DataSource = Year_check.DefaultView;
                                        GridviewBooked.DataBind();
                                    }
                                    else
                                    {
                                        GridviewBooked.Visible = false;
                                        gg_CustomerLedger_Month.Visible = false;
                                        lbl_total_record.Visible = false;
                                        error_msg.Text = "No Record Found...";
                                    }
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
                if (clvar._Zone == "")
                {
                    queryCondition = " and mpb.orgin in (" + clvar._TownCode + ") ";
                    expresscentrCondition = "   ec.bid IN (" + clvar._TownCode + ") ";
                }
                else
                {
                    queryCondition = " ";
                }
                if (rb_Booked.Checked)
                {
                    ReportType_ = " AND isnull(mpb.isBooked,'0')='1' ";
                }
                else if (rb_NoResponse.Checked)
                {
                    ReportType_ = " AND isnull(mpb.isBooked,'0')='0' AND mpb.remarks IS NOT null ";
                }
                else if (rb_Pending.Checked)
                {
                    ReportType_ = " AND isnull(mpb.isBooked,'0')='0'  AND mpb.remarks IS null and isnull(mpb.callstatus,'0')!='5'";
                }
                else if (rb_refuse.Checked)
                {
                    ReportType_ = " AND isnull(mpb.isBooked,'0')='0'  AND mpb.remarks IS null and isnull(mpb.callstatus,'0')='5'";
                }
                string sql = "SELECT mpb.ticketNumber, \n"
               + "       mpb.consigner, \n"
               + "       mpb.consignee, \n"
               + "       mpb.consigneraddress,r.firstName+' '+r.lastName+'-'+r.riderCode ridername,  \n"
               + "       mpb.consignerCellNo,CONVERT(varchar(15), mpb.pickuptime, 100) as pickuptime, pbr.name reason, \n"
               + "       mpb.pieces,mpb.expressCenterCode,pcs.NAME callStatus, pcs.id callStatusCode, \n"
               + "       mpb.weight,mpb.RiderCode,ec.name+'-'+ec.expressCenterCode expressCenter,Convert(varchar,mpb.pickupScheduled,5) pickupScheduled, \n"
               + "       case when mpb.scheduledService=0 THEN 'Pickup' WHEN  mpb.scheduledService=1 THEN 'Drop' when mpb.scheduledService=2 THEN 'Inquiry' end scheduledService  "
               + "FROM   MNP_PreBookingConsignment mpb \n"
               + "       INNER JOIN branches b1 \n"
               + "            ON  b1.branchCode = mpb.orgin \n"
               + "       INNER JOIN branches b2 \n"
               + "            ON  b2.branchCode = mpb.destination"
               + "       left JOIN ExpressCenters ec "
               + "             ON ec.expressCenterCode = mpb.expressCenterCode "
               + "       LEFT JOIN riders r "
               + "             ON r.riderCode=mpb.RiderCode "
               + "       LEFT JOIN MNP_PreBookingReason pbr "
               + "             ON pbr.id = mpb.Reason "
               + "        LEFT JOIN MNP_PreBookingCallStatus pcs "
               + "            ON pcs.id = mpb.callStatus   "
               + " where  cast(mpb.createdOn as date)>=Cast('" + from_date_tostring + "' as date) and cast(mpb.createdOn as date)<=Cast('" + end_date_tostring + "' as date) "
                 + queryCondition + ReportType_ + "  order by mpb.createdOn desc ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);

                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();


                string sql_2 = "";
                orcl.Open();
                //if (SessionZone.ToLower() == "all")
                //{
                //    sql_2 = " SELECT firstName+' '+lastName+'-'+riderCode NAME,riderCode \n"
                //   + "  from Riders WHERE [status]=1 order by firstName,lastName";
                //}
                //else if (SessionBranch.ToLower() == "all")
                //{
                //    sql_2 = " SELECT firstName+' '+lastName+'-'+riderCode NAME,riderCode \n"
                //  + "  from Riders WHERE Zoneid in(" + SessionZone + ") AND [status]=1 order by firstName,lastName";
                //}
                //else
                //{
                //    sql_2 = " SELECT firstName+' '+lastName+'-'+riderCode NAME,riderCode \n"
                //   + "  from Riders WHERE branchId in (" + SessionBranch + ") AND [status]=1 order by firstName,lastName";
                //}

                sql_2 = " SELECT firstName+' '+lastName+'-'+riderCode NAME,riderCode \n"
        + "  from Riders WHERE branchId in (" + clvar._TownCode + ") AND [status]=1 order by firstName,lastName";

                SqlCommand cmd = new SqlCommand(sql_2, orcl);
                SqlDataAdapter oda_rider = new SqlDataAdapter(cmd);
                oda_rider.Fill(ds_riders);
                orcl.Close();


                string sql_Exp = "";
                orcl.Open();
                //if (SessionZone.ToLower() == "all")
                //{

                //     sql_Exp = "SELECT ec.name+'-'+ec.expressCenterCode name,ec.expressCenterCode code \n"
                //       + "  FROM ExpressCenters ec WHERE  ec.[status]=1 \n"
                //       + "ORDER BY ec.name \n";
                //}
                //else if (SessionBranch.ToLower() == "all")
                //{
                //    sql_Exp= " SELECT ec.name + '-' + ec.expressCenterCode NAME, \n"
                //       + "       ec.expressCenterCode     code \n"
                //       + "FROM   ExpressCenters           ec \n"
                //       + "INNER JOIN Branches b on b.branchCode=ec.bid \n"
                //       + "WHERE  b.zoneCode IN (" + SessionZone + ") \n"
                //       + "       AND ec.[status] = 1 \n"
                //       + "ORDER BY \n"
                //       + "       ec.name";
                //}
                //else
                //{
                //    sql_Exp = "SELECT ec.name + '-' + ec.expressCenterCode NAME, \n"
                //       + "       ec.expressCenterCode     code \n"
                //       + "FROM   ExpressCenters           ec \n"
                //       + "WHERE  ec.bid IN (" + SessionBranch + ") \n"
                //       + "       AND ec.[status] = 1 \n"
                //       + "ORDER BY \n"
                //       + "       ec.name";
                //}

                sql_Exp = "SELECT ec.name + '-' + ec.expressCenterCode NAME, \n"
                    + "       ec.expressCenterCode     code \n"
                    + "FROM   ExpressCenters           ec \n"
                    + "WHERE ec.bid IN (" + clvar._TownCode + ")  \n"
                    + "       AND ec.[status] = 1 \n"
                    + "ORDER BY \n"
                    + "       ec.name";
                SqlCommand cmd_exp = new SqlCommand(sql_Exp, orcl);
                SqlDataAdapter oda_exp = new SqlDataAdapter(cmd_exp);
                oda_exp.Fill(ds_Express);
                orcl.Close();

                String sql_Call = " select * from MNP_PreBookingCallStatus WHERE [STATUS]=1";

                SqlCommand cmd_call = new SqlCommand(sql_Call, orcl);
                SqlDataAdapter oda_call = new SqlDataAdapter(cmd_call);
                oda_call.Fill(ds_call);
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
        protected void ExportToCSVOriginal(object sender, EventArgs e)
        {
            DataTable Year_check = GetPickupData(clvar);

            if (Year_check != null)
            {
                if (Year_check.Rows.Count != 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=RetailDiscountReport" + from_date_tostring + "_to_" + end_date_tostring + ".csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    sb = new StringBuilder();
                    for (int k = 0; k < Year_check.Columns.Count; k++)
                    {
                        //add separator
                        sb.Append(Year_check.Columns[k].ColumnName.ToString() + ',');
                    }
                    //append new line
                    sb.Append("\r\n");
                    for (int i = 0; i < Year_check.Rows.Count; i++)
                    {
                        for (int k = 0; k < Year_check.Columns.Count; k++)
                        {
                            if (Year_check.Rows[i][k].ToString() == "" || Year_check.Rows[i][k].ToString() == "&nbsp;" || Year_check.Rows[i][k].ToString() == null)
                            {
                                if (Year_check.Rows[i][0].ToString() == "")
                                {
                                    string data = null;
                                    data = Year_check.Rows[i + 1][0].ToString();
                                    sb.Append(data.ToString() + ',');
                                }
                                else
                                {
                                    sb.Append("" + ',');
                                }
                            }
                            else
                            {

                                string data = null;
                                data = Year_check.Rows[i][k].ToString().Trim();
                                data = Regex.Replace(data, @"&#39;", @"'").ToString();
                                data = String.Format("\"{0}\"", data);
                                sb.Append(data.ToString() + ',');
                            }
                        }
                        //append new line
                        sb.Append("\r\n");
                    }
                    Response.Output.Write(sb.ToString());
                    Response.Flush();


                    Response.SuppressContent = true;

                    // Directs the thread to finish, bypassing additional processing
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    gg_CustomerLedger_Month.Visible = false;
                    error_msg.Text = "No Record Found...";
                    lbl_total_record.Text = "";
                }
            }
        }

        //protected void branch_chk_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (branch_chk.Checked)
        //    {
        //        dd_branch.Visible = false;
        //    }
        //    else
        //    {
        //        dd_branch.Visible = true;
        //    }
        //}

        protected void ddl_rider_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            Label ddlAddLabTestShortName = (Label)row.FindControl("lbl_Id");
            String TicketNumber = ddlAddLabTestShortName.Text.ToString();
            String riderSelected = ddlLabTest.SelectedValue.ToString();
            if (riderSelected != "0")
            {
                String st = "update MNP_PreBookingConsignment set ridercode='" + riderSelected + "',modifiedOn=getdate(),modifiedby=" + U_ID + " where ticketNumber='" + TicketNumber + "'";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                try
                {
                    orcl.Open();
                    SqlCommand sqlcom = new SqlCommand(st, orcl);
                    sqlcom.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Rider updated');", true);

                }
                catch (SqlException ex)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Error updating rider')</script>");
                }
                finally
                {
                    orcl.Close();
                }
            }

        }

        protected void ddl_expressCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            Label ddlAddLabTestShortName = (Label)row.FindControl("lbl_Id_ticket");
            String TicketNumber = ddlAddLabTestShortName.Text.ToString();
            String ECSelected = ddlLabTest.SelectedValue.ToString();
            if (ECSelected != "0")
            {
                String st = "update MNP_PreBookingConsignment set expresscentercode='" + ECSelected + "',modifiedOn=getdate(),modifiedby=" + U_ID + " where ticketNumber='" + TicketNumber + "'";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                try
                {
                    orcl.Open();
                    SqlCommand sqlcom = new SqlCommand(st, orcl);
                    sqlcom.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Express Center updated');", true);

                }
                catch (SqlException ex)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Error updating Express Center')</script>");
                }
                finally
                {
                    orcl.Close();
                }
            }
        }


        protected void ddl_callStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlLabTest.NamingContainer;
            Label ddlAddLabTestShortName = (Label)row.FindControl("lbl_Id_CallStatus");
            String TicketNumber = ddlAddLabTestShortName.Text.ToString();
            String ECSelected = ddlLabTest.SelectedValue.ToString();
            if (ECSelected != "0")
            {
                String st = "update MNP_PreBookingConsignment set callStatus='" + ECSelected + "',modifiedOn=getdate(),modifiedby=" + Session["U_ID"].ToString() + " where ticketNumber='" + TicketNumber + "'";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                try
                {
                    orcl.Open();
                    SqlCommand sqlcom = new SqlCommand(st, orcl);
                    sqlcom.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('call status updated');", true);

                }
                catch (SqlException ex)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Error updating call status')</script>");
                }
                finally
                {
                    orcl.Close();
                }

                if (ECSelected != "4")
                {
                    Label lblRider = (Label)row.FindControl("Lbl_riders");
                    String riderSelected = lblRider.Text.ToString();
 
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        try
                        {
                            HttpClient client = new HttpClient();

                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));

                            HttpResponseMessage response = client.PostAsJsonAsync("http://20.46.47.21/CustomerAPI/api/Users/SendMessageAndroid", new PickupRiderNotification { U_ID = Session["U_ID"].ToString(), TicketNumber = TicketNumber, CallStatus = ECSelected }).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                var rs = response.Content.ReadAsAsync<Response_PickupRiderNotification>().Result;
                            }
                        }
                        catch (Exception er)
                        {  }
                    }).Start();
                }
            }
        }
    }
}