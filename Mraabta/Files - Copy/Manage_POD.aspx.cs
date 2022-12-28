using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net.Mail;
using MRaabta.App_Code;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class Manage_POD : System.Web.UI.Page
    {
        Consignemnts con = new Consignemnts();
        Cl_Variables clvar = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        UpdateCustomer uc = new UpdateCustomer();
        bool flag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Errorid.Text = "";
            err_msg.Text = "";
            if (!IsPostBack)
            {
                GetZones();
                dd_zone.SelectedValue = HttpContext.Current.Session["ZoneCode"].ToString();
                GetBranches();
                dd_branch.SelectedValue = HttpContext.Current.Session["BranchCode"].ToString();
                if (HttpContext.Current.Session["Profile"].ToString() == "78")
                {
                    tr_Branch.Style.Add("display", "Block");
                }
                else
                {
                    tr_Branch.Style.Add("display", "None");
                }
                PODStatus();
                PODReasons();
                Relations();
                //Get_MasterVehicle();
                GetVehicleType();


                //txt_runsheetNumber.Text = "201810960711";
                //txt_riderCode.Text = "1930";
                //btn_search_Click(this, e);
            }

        }
        public void GetZones()
        {
            DataTable dt = new DataTable();

            string sql = "SELECT DISTINCT z.zoneCode, \n"
               + "       z.name \n"
               + "FROM   Zones z \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.zoneCode = z.zoneCode \n"
               + "            AND b.[status] = '1' \n"
               + "WHERE  z.[status] = '1'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

                dd_zone.DataSource = dt;
                dd_zone.DataTextField = "name";
                dd_zone.DataValueField = "zoneCode";
                dd_zone.DataBind();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        public void GetBranches()
        {
            DataTable dt = new DataTable();
            string query = "select Sname + ' - ' + name BranchName, b.branchCode from Branches b where b.status = '1' and b.zoneCode = '" + dd_zone.SelectedValue + "'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

                dd_branch.DataSource = dt;
                dd_branch.DataTextField = "BranchName";
                dd_branch.DataValueField = "branchCode";
                dd_branch.DataBind();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }

        protected void Relations()
        {
            DataTable dt = func.GetRelations();
            ViewState["Relations"] = dt;
        }
        protected void PODStatus()
        {
            DataTable dt = func.GetPODStatus();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["PODStatus"] = dt;
                }
            }
        }

        protected void PODReasons()
        {
            DataTable dt = func.GetPODReasons();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["PODReasons"] = dt;
                }
            }
        }

        protected void GetVehicleType()
        {
            string query = "select * from Vehicle_Type where status = '1'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_vehicleType.DataSource = dt;
                    dd_vehicleType.DataTextField = "TypeDesc";
                    dd_vehicleType.DataValueField = "Typeid";
                    dd_vehicleType.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }

        protected void gv_consignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable reasons = ViewState["PODReasons"] as DataTable;
                DataTable statuses = ViewState["PODStatus"] as DataTable;
                DataTable consignments = ViewState["dt"] as DataTable;
                DataTable relations = ViewState["Relations"] as DataTable;
                DataTable dt_CN_Disabled = ViewState["dt_CN_Disabled"] as DataTable;
                DataRow[] dr = consignments.Select("consignmentNumber = '" + e.Row.Cells[1].Text + "'");
                DataRow[] dr_CN_Disabled = dt_CN_Disabled.Select("consignmentNumber = '" + e.Row.Cells[1].Text + "'");
                DropDownList dd_gStatus = e.Row.FindControl("dd_gStatus") as DropDownList;
                DropDownList dd_gReasons = e.Row.FindControl("dd_gReason") as DropDownList;
                //dd_gReasons.Attributes.Add("onblur", "ChangeFocus()");
                HiddenField hd_status = e.Row.FindControl("hd_status") as HiddenField;
                DropDownList dd_relation = e.Row.FindControl("dd_relation") as DropDownList;
                HiddenField hdRelation = e.Row.FindControl("hd_relation") as HiddenField;
                HiddenField hdWrongCN = e.Row.FindControl("hd_wrong") as HiddenField;
                TextBox receivedBy = e.Row.FindControl("txt_gReceivedBy") as TextBox;
                HiddenField hd_origin = (HiddenField)e.Row.FindControl("hd_origin");
                HiddenField hd_rtnBranch = (HiddenField)e.Row.FindControl("hd_rtnBranch");
                HiddenField hd_RTO = (HiddenField)e.Row.FindControl("hd_RTO");
                HiddenField hd_reason = (HiddenField)e.Row.FindControl("hd_reason");
                HiddenField hd_destination = (HiddenField)e.Row.FindControl("hd_destination");
                HiddenField hd_isPayable = (HiddenField)e.Row.FindControl("hd_isPayable");
                HiddenField hd_cod = (HiddenField)e.Row.FindControl("hd_cod");

                string profileCode = Session["PROFILE"].ToString();

                dd_relation.DataSource = relations;
                dd_relation.DataTextField = "NAME";
                dd_relation.DataValueField = "ID";
                dd_relation.DataBind();
                if (hdRelation.Value.Trim() != "")
                {
                    dd_relation.SelectedValue = hdRelation.Value;
                }


                dd_gStatus.DataSource = statuses;
                dd_gStatus.DataTextField = "AttributeValue";
                dd_gStatus.DataValueField = "ID";
                dd_gStatus.DataBind();
                if (hd_status.Value.ToUpper() == "UNDELIVERED")
                {
                    dd_gStatus.SelectedValue = "56";
                }
                else if (hd_status.Value != "0" && hd_status.Value != string.Empty)
                {
                    dd_gStatus.SelectedValue = hd_status.Value;
                }

                dd_gReasons.DataSource = reasons;
                dd_gReasons.DataTextField = "AttributeValue";
                dd_gReasons.DataValueField = "ID";
                dd_gReasons.DataBind();



                if (dr.Length > 0)
                {
                    foreach (ListItem item in dd_gReasons.Items)
                    {
                        if (item.Value == dr[0]["ReasonID"].ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }



                    #region Commented on instructions of Mr.Nasir Hussain

                    //if (hd_rtnBranch.Value == hd_destination.Value || hd_rtnBranch.Value == dd_branch.SelectedValue /*HttpContext.Current.Session["BranchCode"].ToString()*/)
                    if (hd_rtnBranch.Value == dd_branch.SelectedValue /*HttpContext.Current.Session["BranchCode"].ToString()*/)
                    {
                        ListItem RTO = dd_gReasons.Items.FindByValue("58");
                        if (RTO != null)
                        {
                            dd_gReasons.Items.Remove(RTO);
                        }
                    }
                    if (!(e.Row.Cells[1].Text.ToString().StartsWith("5") && e.Row.Cells[1].Text.ToString().Length == 15))
                    {
                        if (hd_origin.Value != dd_branch.SelectedValue /* HttpContext.Current.Session["BranchCode"].ToString()*/ /*hd_destination.Value*/)
                        {
                            ListItem RTS = dd_gReasons.Items.FindByValue("59");
                            if (RTS != null)
                            {
                                dd_gReasons.Items.Remove(RTS);
                            }
                        }
                    }
                    //if (e.Row.Cells[1].Text.ToString().StartsWith("5") && e.Row.Cells[1].Text.ToString().Length == 15)
                    //{
                    //    if ((!Convert.ToBoolean(dt.Rows[0]["GotNCI"])) && (!Convert.ToBoolean(dt.Rows[0]["AtOrgn"])) && (!Convert.ToBoolean(dt.Rows[0]["isRTS"])))
                    //    {
                    //        ListItem RO = dd_gReasons.Items.FindByValue("58");
                    //        if (RO != null)
                    //        {
                    //            dd_gReasons.Items.Remove(RO);
                    //        }
                    //    }
                    //    if ((!Convert.ToBoolean(dt.Rows[0]["GotNCI"])) && (!Convert.ToBoolean(dt.Rows[0]["isRTO"])) && (!Convert.ToBoolean(dt.Rows[0]["isRTS"])))
                    //    { 
                    //        ListItem RTS = dd_gReasons.Items.FindByValue("59");
                    //        if (RTS != null)
                    //        {
                    //            dd_gReasons.Items.Remove(RTS);
                    //        }
                    //    }
                    //    if (hd_rtnBranch.Value != dd_branch.SelectedValue /* HttpContext.Current.Session["BranchCode"].ToString()*/ /*hd_destination.Value*/)
                    //    {
                    //        ListItem RTS = dd_gReasons.Items.FindByValue("59");
                    //        if (RTS != null)
                    //        {
                    //            dd_gReasons.Items.Remove(RTS);
                    //        }
                    //    }
                    //}
                    if (hd_reason.Value == "59")
                    {
                        //dd_gReasons.SelectedValue = "59";     
                        if (profileCode != "78")
                        {
                            e.Row.Enabled = false;
                        }
                        else
                        {
                            e.Row.Enabled = true;
                        }
                    }
                    else if ((hd_RTO.Value.ToString().ToUpper() == "TRUE" || hd_RTO.Value.ToString() == "1") /*&& hd_origin.Value == HttpContext.Current.Session["BranchCode"].ToString()*/)
                    {
                        dd_gReasons.Items.Clear();
                        dd_gReasons.Items.Add(new ListItem { Text = "Select Reason", Value = "0" });
                        if (hd_rtnBranch.Value == HttpContext.Current.Session["BranchCode"].ToString())
                        {
                            DataRow[] afterRTOReasons = reasons.Select("ID in ('59','64', '" + hd_reason.Value + "')");
                            dd_gReasons.DataSource = afterRTOReasons.CopyToDataTable();
                            dd_gReasons.DataTextField = "AttributeValue";
                            dd_gReasons.DataValueField = "ID";
                            dd_gReasons.DataBind();
                            dd_gReasons.SelectedValue = hd_reason.Value;
                        }
                        else
                        {
                            DataRow[] afterRTOReasons = reasons.Select("ID in ('64', '" + hd_reason.Value + "')");
                            dd_gReasons.DataSource = afterRTOReasons.CopyToDataTable();
                            dd_gReasons.DataTextField = "AttributeValue";
                            dd_gReasons.DataValueField = "ID";
                            dd_gReasons.DataBind();
                            dd_gReasons.SelectedValue = hd_reason.Value;
                        }

                    }
                    //else
                    //{
                    //    if (hd_origin.Value != HttpContext.Current.Session["BranchCode"].ToString())
                    //    {
                    //        //dd_gReasons.Items.Remove(dd_gReasons.Items.FindByValue("59"));
                    //    }
                    //}

                    if (profileCode != "78")
                    {
                        if (dd_gReasons.SelectedValue != "223")
                        {
                            ListItem item = dd_gReasons.Items.FindByValue("223");
                            if (item != null)
                            {
                                dd_gReasons.Items.Remove(item);
                            }
                        }
                    }
                    #endregion

                    #region Query to check NCI for RO or RTS
                    string sql = @"select consignmentnumber, (select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + dr[0].ItemArray[2].ToString() + @"') bypass
                        , sum(AtOrgn) as AtOrgn, sum(AtDest) as AtDest, sum(GotNCI) as GotNCI, sum(isRTO) as isRTO, sum(isRTS) as isRTS from (
                                        select '" + dr[0].ItemArray[2].ToString() + @"' consignmentnumber, 0 as AtOrgn, 0 as AtDest, 0 as GotNCI, 0 as isRTO, 0 as isRTS 
	                                    union
	                                    select rc.consignmentnumber, case when Reason not in ('70') and c.orgin = rc.branchcode then 1 else 0 end as AtOrgn, 
		                                    case when Reason in ('70') and c.orgin = rc.branchcode then 0 else 1 end AtDest, 0 as GotNCI, 
                                            case when Reason in ('58') then 1 else 0 end as isRTO, case when Reason in ('59') or c.orgin = c.destination then 1 else 0 end as isRTS 
                                            from RunsheetConsignment rc inner join Consignment c on c.consignmentNumber = rc.consignmentNumber 
		                                    where rc.consignmentNumber = '" + dr[0].ItemArray[2].ToString() + @"' and rc.createdOn = 
                                            (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + dr[0].ItemArray[2].ToString() + @"' ) 
	                                    union
	                                    select consignmentnumber, 0 as AtOrgn, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as GotNCI, 0 as isRTO, 0 as isRTS 
                                        from MNP_NCI_Request where calltrack = 2 and consignmentnumber ='" + dr[0].ItemArray[2].ToString() + @"' group by consignmentnumber
                                    ) xb group by consignmentnumber ";

                    DataTable dt = new DataTable();

                    try
                    {
                        SqlConnection orcl = new SqlConnection(clvar.Strcon());
                        orcl.Open();
                        SqlCommand orcd = new SqlCommand(sql, orcl);
                        orcd.CommandType = CommandType.Text;
                        orcd.CommandTimeout = 300;
                        SqlDataAdapter oda = new SqlDataAdapter(orcd);
                        oda.Fill(dt);
                        orcl.Close();
                    }
                    catch (Exception)
                    { }
                    if (dt.Rows.Count > 0)
                        if (e.Row.Cells[1].Text.ToString().StartsWith("5") && e.Row.Cells[1].Text.ToString().Length == 15)
                        {
                            if ((!Convert.ToBoolean(dt.Rows[0]["GotNCI"])) && (!Convert.ToBoolean(dt.Rows[0]["AtOrgn"])) && (!Convert.ToBoolean(dt.Rows[0]["isRTS"])) && dt.Rows[0]["bypass"].ToString() == "0")
                            {
                                ListItem RO = dd_gReasons.Items.FindByValue("58");
                                if (RO != null)
                                {
                                    dd_gReasons.Items.Remove(RO);
                                }
                            }
                            if ((!Convert.ToBoolean(dt.Rows[0]["GotNCI"])) && (!Convert.ToBoolean(dt.Rows[0]["isRTO"])) && (!Convert.ToBoolean(dt.Rows[0]["isRTS"])) && dt.Rows[0]["bypass"].ToString() == "0")
                            {
                                ListItem RTS = dd_gReasons.Items.FindByValue("59");
                                if (RTS != null)
                                {
                                    dd_gReasons.Items.Remove(RTS);
                                }
                            }
                            if (hd_rtnBranch.Value != dd_branch.SelectedValue /* HttpContext.Current.Session["BranchCode"].ToString()*/ /*hd_destination.Value*/)
                            {
                                ListItem RTS = dd_gReasons.Items.FindByValue("59");
                                if (RTS != null)
                                {
                                    dd_gReasons.Items.Remove(RTS);
                                }
                            }
                        }
                    #endregion

                    //Binding DateTime
                    if (dr[0]["deliveryDateTime"].ToString() != "")
                    {
                        if (dr[0]["deliveryDateTime"].ToString().Contains("01/01/1900"))
                        {

                        }
                        else
                        {
                            ((Telerik.Web.UI.RadDatePicker)e.Row.FindControl("txt_gDelvDate")).SelectedDate = DateTime.Parse(DateTime.Parse(dr[0]["deliveryDateTime"].ToString()).ToString("yyyy-MM-dd"));
                            ((Telerik.Web.UI.RadDatePicker)e.Row.FindControl("txt_gDelvDate")).Visible = true;

                            ((TextBox)e.Row.FindControl("txt_gTime")).Text = DateTime.Parse(dr[0]["deliveryDateTime"].ToString()).ToString("hh:m");
                            ((Label)e.Row.FindControl("lbl_time")).Text = DateTime.Parse(dr[0]["deliveryDateTime"].ToString()).ToString("hh:m");
                        }

                    }
                    else
                    {
                        ((Telerik.Web.UI.RadDatePicker)e.Row.FindControl("txt_gDelvDate")).Visible = true;
                        //((Label)e.Row.FindControl("Lbl_1")).Visible = false;
                        ((Telerik.Web.UI.RadDatePicker)e.Row.FindControl("txt_gDelvDate")).SelectedDate = txt_date.SelectedDate;
                    }
                    if (dr_CN_Disabled.Count() > 0)
                    {
                        e.Row.Enabled = false;

                    }

                    else if ((dd_gStatus.SelectedItem.Text == "DELIVERED" || dd_gReasons.SelectedValue == "204" || dd_gReasons.SelectedValue == "223") && (profileCode != "78" || ((hd_isPayable.Value.ToUpper() == "TRUE" || hd_isPayable.Value.ToUpper() == "1") && hd_cod.Value == "1")))
                    //else if ((dd_gStatus.SelectedItem.Text == "DELIVERED" || dd_gReasons.SelectedValue != "0") && (profileCode != "78" || ((hd_isPayable.Value.ToUpper() == "TRUE" || hd_isPayable.Value.ToUpper() == "1") && hd_cod.Value == "1")))
                    {
                        e.Row.Enabled = false;
                    }

                    if (hdWrongCN.Value == "1")
                    {

                        e.Row.Enabled = false;
                        e.Row.BackColor = System.Drawing.Color.FromArgb(255, 56, 56);
                        receivedBy.Text = "WRONG CN";
                    }
                }

            }
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_date.Clear();
            txt_podDate.Text = "";
            txt_riderCode.Text = "";
            txt_riderName.Text = "";
            txt_route.Text = "";
            txt_runsheetNumber.Text = "";
            txt_vehicleNumber.Text = "";
            txt_runsheetNumber.Enabled = true;
            txt_riderCode.Enabled = true;
            // dd_vehicle.ClearSelection();
            dd_vehicleType.ClearSelection();
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            string alert = "";
            string vehicleID = "";// txt_vehicleNumber.Text;

            vehicleID = txt_vehicleNumber.Text;

            string vehicleType = dd_vehicleType.SelectedValue;
            Int64 meterStart = 0;
            Int64 meterEnd = 0;

            Int64.TryParse(txt_meterStart.Text, out meterStart);
            Int64.TryParse(txt_meterEnd.Text, out meterEnd);

            DataTable temp = ViewState["dt"] as DataTable;
            DataRow[] dr = temp.Select("ReasonID is not null");

            clvar.RunsheetNumber = txt_runsheetNumber.Text;
            clvar.routeCode = hd_routeCode.Value;
            foreach (GridViewRow row in gv_consignments.Rows)
            {
               
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                DropDownList status = (DropDownList)row.FindControl("dd_gStatus");
                TextBox receivedBy = (TextBox)row.FindControl("txt_gReceivedBy");
                TextBox time = (TextBox)row.FindControl("txt_gTime");
                DropDownList reason = (DropDownList)row.FindControl("dd_gReason");
                TextBox comments = (TextBox)row.FindControl("txt_gComments");
                DropDownList dd_relation = row.FindControl("dd_relation") as DropDownList;
                TextBox givenToRider = (row.FindControl("txt_gRiderCode") as TextBox);

                DataTable dt = ViewState["PODReasons"] as DataTable;
                if (reason.SelectedValue == "0" && row.Enabled == true)
                {

                }
                else if (row.Enabled == true)
                {
                    status.SelectedValue = dt.Select("id = '" + reason.SelectedValue + "'")[0]["ID1"].ToString();
                }

                clvar.ClvarListStr.Add(row.Cells[1].Text);
                if ((status.SelectedItem.Text.ToUpper() == "DELIVERED") && (receivedBy.Text.Trim() == "" || time.Text.Trim() == "" || picker.SelectedDate.Value.ToString().Trim() == ""))
                {
                    row.Cells[5].Focus();
                    alert = "Received By and Time Cannot be Empty";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "HideLoader_('" + alert + "');", true);
                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aletMessage", "alert('Received By, Time and Delivery Date Cannot be Empty')", true);
                    err_msg.Text = "Received By and Time Cannot be Empty";
                    return;
                }
            }
            clvar.RunsheetNumber = txt_runsheetNumber.Text.ToString();

            string error = ""; //con.UpdatePOD_(gv_consignments, clvar);

            if (HttpContext.Current.Session["Profile"].ToString() == "78")
            {
                clvar.Branch = dd_branch.SelectedValue;
                if (dd_branch.SelectedValue == "0")
                {
                    //Alert("Select Branch", "Red");
                    alert = "Select Branch";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "HideLoader_('" + alert + "');", true);
                    return;
                }
            }
            else
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            }

            error = UpdatePOD_Consignment(gv_consignments, clvar, vehicleID, vehicleType, meterStart, meterEnd);

            if (error == "OK")
            {
                StringBuilder html = new StringBuilder();
                html.Append("<b>Dear Operations Manager,</b>");
                html.Append("<br />");
                html.Append("<br />");

                html.Append("Please note your today’s pending shipment’s details as on dated " + DateTime.Now.ToString("dd/MM/yyyy") + ".");
                html.Append("<br />");
                html.Append("<br />");

                html.Append("You need to immediately look into this/these discrepancies and fix them followed by system update.<br />Since this is a system generated email, please do not reply on this, rather contact your respective Regional OR Head Office Management.");
                html.Append("<br />");
                html.Append("<br />");

                html.Append("<table style=\"width:100%; border-collapse:collapse; font-size:x-small; border:1px Solid Black; font-family: Calibri; \">");
                html.Append("<tr>");
                html.Append("<td style=\"border: 1px Solid Black; width:3%;\">");
                html.Append("<b>Sr.</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:10%;\">");
                html.Append("<b>Report Date Time</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:10%;\">");
                html.Append("<b>CN Number</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:6%;\">");
                html.Append("<b>Booking Date</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:3%;\">");
                html.Append("<b>ORG</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:3%;\">");
                html.Append("<b>DSTN</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:3%;\">");
                html.Append("<b>Pieces</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:3%;\">");
                html.Append("<b>Weight</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:15%;\">");
                html.Append("<b>Consignee</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:23%;\">");
                html.Append("<b>Address</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:5%;\">");
                html.Append("<b>COD Amount</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:10%;\">");
                html.Append("<b>Reason</b>");
                html.Append("</td>");
                html.Append("<td style=\"border: 1px Solid Black; width:6%;\">");
                html.Append("<b>Remarks</b>");
                html.Append("</td>");
                html.Append("</tr>");
                bool sendEmail = false;
                int emailCNcount = 1;
                string cnForEmail = string.Empty;
                foreach (GridViewRow row in gv_consignments.Rows)
                {
                    /////////cod controls start
                    string ConsignmentNo = row.Cells[1].Text;
                    DropDownList reason = (DropDownList)row.FindControl("dd_gReason");
                    string[][] Response= CheckCODControls(ConsignmentNo, reason.SelectedValue);

                    if (Response[0][0]=="false")
                    {
                        alert = Response[0][1];
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "HideLoader_('" + alert + "');", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "aletMessage", "alert('Could Not Save POD.Error:" + error + "')", true);

                        err_msg.Text = Response[0][1];
                        return;
                    }
                    /////////cod controls end

                    Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                    DropDownList status = (DropDownList)row.FindControl("dd_gStatus");
                    TextBox receivedBy = (TextBox)row.FindControl("txt_gReceivedBy");
                    TextBox time = (TextBox)row.FindControl("txt_gTime");
                    
                    TextBox comments = (TextBox)row.FindControl("txt_gComments");
                    HiddenField cod = (HiddenField)row.FindControl("hd_cod");

                    if (row.Enabled == true)
                    {
                        if (status.SelectedValue == "55")
                        {
                            string cn = row.Cells[1].Text;
                            clvar.consignmentNo = cn;

                            DataSet orderRefNo = Get_CustomerRefFromConsignment(clvar);

                            if (orderRefNo.Tables[0].Rows.Count > 0)
                            {
                                string custRefNo = orderRefNo.Tables[0].Rows[0][0].ToString();
                                string accountno = orderRefNo.Tables[0].Rows[0][1].ToString();
                                List<string> responseList = uc.Update_OrderStatus(accountno, custRefNo, "55");

                                DataSet GetItemsToMarkStatus = Get_ItemsToUpdate(custRefNo, accountno);

                                foreach (DataRow rowX in GetItemsToMarkStatus.Tables[0].Rows)
                                {
                                    List<string> responseListItem = uc.Update_ItemStatus(accountno, custRefNo, "55", rowX["itemcode"].ToString());
                                }
                            }

                        }
                    }
                    if (reason.SelectedValue == "64")
                    {
                         ConsignmentNo = row.Cells[1].Text;
                        clvar.consignmentNo = ConsignmentNo;
                        DataSet get_consignment = con.Consignment(clvar);
                        if (get_consignment.Tables[0].Rows.Count != 0)
                        {

                            if ((DateTime.Parse(txt_date.SelectedDate.Value.ToShortDateString()) - DateTime.Today).TotalDays <= 2)
                            {
                                if (get_consignment.Tables[0].Rows[0]["consigneePhoneNo"].ToString() != "")
                                {
                                    con.SavePODSmsEntry(ConsignmentNo, get_consignment.Tables[0].Rows[0]["consigneePhoneNo"].ToString(), "F");
                                }
                            }
                        }
                    }
                    if (reason.SelectedValue == "123")
                    {
                         ConsignmentNo = row.Cells[1].Text;
                        string ReceivedBy = receivedBy.Text;

                        clvar.consignmentNo = ConsignmentNo;
                        DataSet get_consignment = con.Consignment(clvar);
                        if (get_consignment.Tables[0].Rows.Count != 0)
                        {
                            if ((DateTime.Parse(txt_date.SelectedDate.Value.ToShortDateString()) - DateTime.Today).TotalDays <= 2)
                            {
                                if (get_consignment.Tables[0].Rows[0]["consignerPhoneNo"].ToString() != "")
                                {
                                    con.SavePODSmsEntry(ConsignmentNo, get_consignment.Tables[0].Rows[0]["consignerPhoneNo"].ToString(), "D");
                                }
                                if (get_consignment.Tables[0].Rows[0]["consignerCellNo"].ToString() != "")
                                {
                                    con.SavePODSmsEntry(ConsignmentNo, get_consignment.Tables[0].Rows[0]["consignerCellNo"].ToString(), "D");
                                }

                                if (get_consignment.Tables[0].Rows[0]["ConsigneePhoneNo"].ToString() != "")
                                {

                                }
                            }
                        }
                    }

                    if (reason.SelectedValue == "59")
                    {
                         ConsignmentNo = row.Cells[1].Text;
                        string ReceivedBy = receivedBy.Text;

                        clvar.consignmentNo = ConsignmentNo;
                        DataSet get_consignment = con.Consignment(clvar);
                        if (get_consignment.Tables[0].Rows.Count != 0)
                        {
                            if ((DateTime.Parse(txt_date.SelectedDate.Value.ToShortDateString()) - DateTime.Today).TotalDays <= 2 && (cod.Value.ToString().ToUpper() == "TRUE" || cod.Value.ToString().ToUpper() == "1"))
                            {
                                string resp = "Dear Customer, your shipment CN " + clvar.consignmentNo + " has been returned received by " + ReceivedBy + " on " + picker.SelectedDate.Value.ToString("dd/MM/yyyy") + ". Thank You. For further details, contact us at 111-202-202";
                            }
                        }
                    }

                    if (reason.SelectedValue == "207")
                    {
                        sendEmail = true;
                         ConsignmentNo = row.Cells[1].Text;
                        cnForEmail += "'" + ConsignmentNo + "'";
                    }
                }

                err_msg.Text = "POD Saved";
                flag = true;

                if (sendEmail)
                {
                    cnForEmail = cnForEmail.Replace("''", "','");
                    clvar.CheckCondition = cnForEmail;
                    DataTable cnDetails = GetCNDetailsForEmail(clvar);
                    if (cnDetails != null)
                    {
                        if (cnDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dr_ in cnDetails.Rows)
                            {
                                string ConsignmentNo = dr_["ConsignmentNumber"].ToString();
                                html.Append("<tr>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["SR"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(DateTime.Parse(dr_["ReportTime"].ToString()).ToString("dd/MM/yyyy HH:mm tt"));
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(ConsignmentNo.ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(DateTime.Parse(dr_["BookingDate"].ToString()).ToShortDateString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["ORG"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["DSTN"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["PIECES"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["WEIGHT"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["Consignee"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["Address"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"text-align:right; border: 1px Solid Black;\">");
                                html.Append(dr_["CODAMOUNT"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");
                                html.Append(dr_["Reason"].ToString());
                                html.Append("</td>");
                                html.Append("<td style=\"border: 1px Solid Black;\">");

                                html.Append("</td>");
                                html.Append("</tr>");
                            }
                            html.Append("</table>");

                            send_Email("", html.ToString(), "Pending COD Shipments " + DateTime.Now.ToShortDateString());
                        }
                    }
                }


                ScriptManager.RegisterStartupScript(this, this.GetType(), "aletMessage", "alert('POD Saved')", true);

                btn_search_Click(this, e);
                return;
            }
            else
            {
                alert = "Could Not Save POD.Error:" + error + "";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "HideLoader_('" + alert + "');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "aletMessage", "alert('Could Not Save POD.Error:" + error + "')", true);

                err_msg.Text = "Could Not Save POD.Error:" + error + "";
                //btn_reset_Click(sender, e);
                return;
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (txt_runsheetNumber.Text.Trim() == "")
            {
                Errorid.Text = "Enter Runsheet Number";
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (txt_riderCode.Text.Trim() == "")
            {
                Errorid.Text = "Enter Rider Code";
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
            clvar.RunSheetNumber = txt_runsheetNumber.Text;
            clvar.riderCode = txt_riderCode.Text;
            if (HttpContext.Current.Session["Profile"].ToString() == "78")
            {
                clvar.Branch = dd_branch.SelectedValue;
                if (dd_branch.SelectedValue == "0")
                {
                    Alert("Select Branch", "Red");
                    return;
                }
            }
            else
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            }
            DataTable detail = GetRunsheetDetail(clvar);
            string RS_DATE = "";
            if (detail != null)
            {
                if (detail.Rows.Count > 0)
                {
                    clvar.routeCode = detail.Rows[0]["RouteCode"].ToString();
                    hd_routeCode.Value = detail.Rows[0]["RouteCode"].ToString();
                    RS_DATE = detail.Rows[0]["createdOn"].ToString();

                }
                else
                {
                    txt_runsheetNumber.Enabled = true;
                    txt_riderCode.Enabled = true;
                    gv_consignments.DataSource = null;
                    gv_consignments.DataBind();
                    Errorid.Text = "Runsheet Not Found";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            else
            {

                gv_consignments.DataSource = null;
                gv_consignments.DataBind();
                Errorid.Text = "Runsheet Not Found";
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
            DataTable dt = GetConsignmentsForRunsheet(clvar);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    txt_runsheetNumber.Enabled = false;
                    txt_riderCode.Enabled = false;
                    txt_date.SelectedDate = DateTime.Parse(detail.Rows[0]["runsheetDate"].ToString());
                    txt_riderCode.Text = detail.Rows[0]["Ridercode"].ToString();
                    txt_riderName.Text = detail.Rows[0]["RiderName"].ToString();
                    txt_route.Text = detail.Rows[0]["Name"].ToString();

                    DataTable dt_CN_Disabled = getCN_Disabled(txt_runsheetNumber.Text, DateTime.Parse(RS_DATE).ToString("yyyy-MM-dd hh:mm:ss.fff"));

                    string vehicle = detail.Rows[0]["VehicleNumber"].ToString();
                    string vehicleType = detail.Rows[0]["VehicleType"].ToString();
                    if (vehicle != "")
                    {
                        ListItem lt_ = dd_vehicleType.Items.FindByValue(vehicleType);
                        txt_vehicleNumber.Text = detail.Rows[0]["VehicleNumber"].ToString();


                        if (lt_ != null)
                        {
                            dd_vehicleType.SelectedValue = lt_.Value;
                        }
                        else
                        {
                            dd_vehicleType.SelectedValue = "0";
                        }

                    }

                    txt_meterStart.Text = detail.Rows[0]["MeterStart"].ToString();
                    txt_meterEnd.Text = detail.Rows[0]["MeterEnd"].ToString();
                    gv_consignments.DataSource = dt;
                    ViewState["dt"] = dt;
                    ViewState["dt_CN_Disabled"] = dt_CN_Disabled;
                    DataRow[] dr = dt.Select("ReasonID is not null");

                    gv_consignments.DataBind();
                    lb_applyAll.Visible = true;
                }
                else
                {
                    txt_runsheetNumber.Enabled = true;
                    txt_riderCode.Enabled = true;
                    gv_consignments.DataSource = null;
                    gv_consignments.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Runsheet Not Found')", true);
                    Errorid.Text = "Runsheet Not Found";
                    Errorid.ForeColor = System.Drawing.Color.Red;
                    return;

                }
            }
            else
            {
                txt_runsheetNumber.Enabled = true;
                txt_riderCode.Enabled = true;
                gv_consignments.DataSource = null;
                gv_consignments.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Runsheet Not Found')", true);
                Errorid.Text = "Runsheet Not Found";
                Errorid.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        private DataTable getCN_Disabled(string runsheetNumber, string RS_DATE)
        {
            string sql = "SELECT * \n"
               + "FROM   ( \n"
               + "           SELECT a.consignmentNumber, \n"
               + "                  CASE WHEN a.RS_DATE > ( \n"
               + "                                SELECT rd.createdOn \n"
               + "                                FROM   runsheet rd \n"
               + "                                WHERE  rd.runsheetNumber = '" + runsheetNumber + "' \n"
               + "                            ) THEN '1' \n"
               + "                       ELSE '0' END  \n"
               + "                  RS_DATE_flag \n"
               + "           FROM   ( \n"
               + "                      SELECT r.runsheetNumber, \n"
               + "                             r.consignmentNumber, \n"
               + "                             r2.createdOn RS_DATE \n"
               + "                      FROM   RunsheetConsignment r \n"
               + "                             INNER JOIN Runsheet r2 \n"
               + "                                  ON  r.runsheetNumber = r2.runsheetNumber -- and r.createdOn = r2.createdOn \n"
               + "                      WHERE  r.consignmentNumber IN (SELECT rc.consignmentNumber \n"
               + "                                                     FROM   RunsheetConsignment  \n"
               + "                                                            rc \n"
               + "                                                            INNER JOIN Runsheet  \n"
               + "                                                                 rs \n"
               + "                                                                 ON  rc.runsheetNumber =  \n"
               + "                                                                     rs.runsheetNumber \n"
               + "                                                     WHERE  rc.runsheetNumber =  \n"
               + "                                                            '" + runsheetNumber + "') \n"
               + "                             AND r2.runsheetNumber <> '" + runsheetNumber + "' \n"
               + "                             AND r2.createdOn > ( \n"
               + "                                     SELECT rd.createdOn \n"
               + "                                     FROM   runsheet rd \n"
               + "                                     WHERE  rd.runsheetNumber = '" + runsheetNumber + "' \n"
               + "                                 ) \n"
               + "                  ) A \n"
               + "       ) B \n"
               + "GROUP BY \n"
               + "       b.consignmentNumber, \n"
               + "       b.RS_DATE_flag \n"
               + "HAVING b.RS_DATE_flag = '1' \n"
               + "ORDER BY \n"
               + "       1, \n"
               + "       2";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        public void Post_BrandedSMS_(string mobile, string resp, string Consignee, string consignmentNumber, string DeliveryDate)
        {

        }

        protected void lb_applyAll_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < gv_consignments.Rows.Count; i++)
            {
                if (gv_consignments.Rows[i].Enabled == false)
                {
                    continue;
                }
                (gv_consignments.Rows[i].FindControl("txt_gTime") as TextBox).Text = (gv_consignments.Rows[0].FindControl("txt_gTime") as TextBox).Text;
                (gv_consignments.Rows[i].FindControl("txt_gReceivedBy") as TextBox).Text = (gv_consignments.Rows[0].FindControl("txt_gReceivedBy") as TextBox).Text;
                (gv_consignments.Rows[i].FindControl("dd_gStatus") as DropDownList).SelectedValue = (gv_consignments.Rows[0].FindControl("dd_gStatus") as DropDownList).SelectedValue;
                (gv_consignments.Rows[i].FindControl("txt_gDelvDate") as Telerik.Web.UI.RadDatePicker).SelectedDate = (gv_consignments.Rows[0].FindControl("txt_gDelvDate") as Telerik.Web.UI.RadDatePicker).SelectedDate;
                (gv_consignments.Rows[i].FindControl("dd_gReason") as DropDownList).SelectedValue = (gv_consignments.Rows[0].FindControl("dd_gReason") as DropDownList).SelectedValue;
                (gv_consignments.Rows[i].FindControl("txt_gComments") as TextBox).Text = (gv_consignments.Rows[0].FindControl("txt_gComments") as TextBox).Text;
                (gv_consignments.Rows[i].FindControl("lbl_flg") as TextBox).Text = "1";
            }
        }

        public DataSet Get_CustomerRefFromConsignment(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {

                string sqlString = "select cd.orderRefNo, c.consignerAccountNo\n" +
                "  from CODConsignmentDetail_New cd, Consignment c\n" +
                " where cd.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
                "   and cd.consignmentNumber = c.consignmentNumber";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Get_ItemsToUpdate(string orderno, string accountNo)
        {

            DataSet Ds_1 = new DataSet();

            string sqlString_ = "select cd.itemcode, cd.itemdescription,cd.status, cs.Description\n" +
            " from CODConsignmentDetail_new cd, Consignment c, CODStatus cs\n" +
            " where c.consignmentNumber = cd.consignmentNumber\n" +
            " and c.consignerAccountNo = '" + accountNo + "'\n" +
            " and cd.Status = cs.status\n" +
            " and cd.orderRefNo = '" + orderno + "'";

            DataSet ds = new DataSet();

            try
            {


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString_, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();

            }

            catch (Exception Err)
            { }
            finally
            { }
            return Ds_1;
        }

        public DataTable GetRunsheetDetail(Cl_Variables clvar)
        {


            string sqlString = "select r.routeCode,\n" +
                "       rt.name,\n" +
                "       rr.riderCode,\n" +
                "       rrr.firstName + ' ' + rrr.lastName RiderName,\n" +
                "       r.runsheetDate, r.VehicleNumber, r.VehicleType, r.MeterStart, r.MeterEnd,r.createdOn \n" +
                "  from Runsheet r\n" +
                " inner join RiderRunsheet rr\n" +
                "    on rr.runsheetNumber = r.runsheetNumber\n" +
                " inner join Riders rrr\n" +
                "    on rrr.riderCode = rr.riderCode\n" +
                "   and r.branchCode = rrr.branchId\n" +
                " inner join Routes rt\n" +
                "    on rt.routeCode = r.routeCode\n" +
                //"   and rrr.routeCode = rt.routeCode\n" +
                "   and rt.bid = r.branchCode \n" +
                " inner join Branches b\n" +
                "    on b.branchCode = r.branchCode\n" +
                 " where r.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
               "   and r.branchCode = '" + clvar.Branch + "'\n" +
               "   and rrr.riderCode = '" + clvar.riderCode + "'";


            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public DataTable GetConsignmentsForRunsheet(Cl_Variables clvar)
        {

            // string sqlString = "select rc.SortOrder,\n" +
            // "       rc.runsheetNumber,\n" +
            // "       c.consignmentNumber,\n" +
            // "       c.consignee,\n" +
            // "       c.orgin,\n" +
            // "       b.name ONAME,\n" +
            // "       c.pieces,\n" +
            // "       rc.time,\n" +
            // "       rc.receivedBy,\n" +
            // "       rc.Status,\n" +
            // "       rc.deliveryDate,\n" +
            // "       rc.Reason,\n" +
            // "       rc.Comments, rc.GiventoRider\n" +
            // "  from Runsheet r\n" +
            // " inner join RunsheetConsignment rc\n" +
            // "    on r.runsheetNumber = rc.runsheetNumber\n" +
            // " inner join Consignment c\n" +
            // "    on c.consignmentNumber = rc.consignmentNumber\n" +
            // " inner join Branches b\n" +
            // "    on b.branchCode = c.orgin\n" +
            // " where rc.runsheetNumber = '" + clvar.RunSheetNumber + "'\n" +
            // " order by SortOrder";

            // sqlString = "\tselect rc.SortOrder,\n" +
            //"\t\t   rc.RunsheetId,\n" +
            //"\t\t   c.consignmentNumber,\n" +
            //"\t\t   c.consignee,\n" +
            //"\t\t   c.orgin,\n" +
            //"\t\t   b.name ONAME,\n" +
            //"\t\t   c.pieces,\n" +
            //"\t\t   rc.DeliveryDateTime ,\n" +
            //"\t\t   rc.receivedBy,\n" +
            //"\t\t   rc.StatusId,\n" +
            //"\t\t   rc.ReasonId,\n" +
            //"\t\t   rc.Comments\n" +
            //"\t from Runsheet r\n" +
            //"\tinner join rvdbo.RunsheetConsignment  rc\n" +
            //"\ton r.runsheetNumber = CAST(rc.RunsheetId as varchar)\n" +
            //"\tinner join Consignment c\n" +
            //"\ton c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
            //"\tinner join Branches b\n" +
            //"\ton b.branchCode = c.orgin\n" +

            //"where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "' and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            //"order by SortOrder DESC";


            // sqlString = "select rc.SortOrder,\n" +
            //"       rc.RunsheetId,\n" +
            //"       c.consignmentNumber,\n" +
            //"       c.consignee,\n" +
            //"       c.orgin,\n" +
            //"       b.name ONAME,\n" +
            //"       c.pieces,\n" +
            //"       rc.DeliveryDateTime,\n" +
            //"       rc.receivedBy,\n" +
            //"       rc.StatusId,\n" +
            //"       rc.ReasonId,\n" +
            //"       rc.Comments, rc.Receiver_CNIC, rc.Relation\n" +
            //"  from Runsheet r\n" +
            //"INNER JOIN rvdbo.RunsheetConsignment rc       \n" +
            //"     ON  r.runsheetNumber = CAST(rc.RunsheetId AS VARCHAR)    \n" +
            //"     AND r.branchCode = rc.BranchCode   \n" +
            //"INNER JOIN RunsheetConsignment RC2           \n" +
            //"     ON  R.runsheetNumber = RC2.runsheetNumber AND R.createdBy = RC2.createdBy      \n" +
            //"     AND rc.RunsheetId = RC2.runsheetNumber       \n" +
            //"     AND rc.ConsignmentId = RC2.consignmentNumber \n" +
            //"     AND rc.BranchCode = RC2.branchcode           \n" +
            //"INNER JOIN Consignment c                        \n" +
            //"    on c.consignmentNumber = CAST(rc.ConsignmentId as varchar)\n" +
            //" inner join Branches b\n" +
            //"    on b.branchCode = c.orgin\n" +
            //" where cast(rc.RunsheetId as varchar) = '" + clvar.RunSheetNumber + "'\n" +
            //"   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            //"   and r.routeCode = '" + clvar.routeCode + "'\n" +
            //" group by rc.SortOrder,\n" +
            //"       rc.RunsheetId,\n" +
            //"       c.consignmentNumber,\n" +
            //"       c.consignee,\n" +
            //"       c.orgin,\n" +
            //"       b.name ,\n" +
            //"       c.pieces,\n" +
            //"       rc.DeliveryDateTime,\n" +
            //"       rc.receivedBy,\n" +
            //"       rc.StatusId,\n" +
            //"       rc.ReasonId,\n" +
            //"       rc.Comments,rc.Receiver_CNIC, rc.Relation\n" +
            //" order by SortOrder ";


            // sqlString = "select ROW_NUMBER() OVER(order by rc2.SortOrder desc) SortOrder,\n" +
            //"       rc2.runsheetNumber  RunsheetId,\n" +
            //"       c.consignmentNumber,\n" +
            //"       c.consignee,\n" +
            //"       c.orgin,\n" +
            //"       b.name              ONAME,\n" +
            //"       c.pieces,\n" +
            //"       cast(rc2.time  as time)  DeliveryDateTime,\n" +
            //"       RC2.receivedBy,\n" +
            //"       RC2.Status          StatusId,\n" +
            //"       rc2.Reason          ReasonId,\n" +
            //"       rc2.Comments,\n" +
            //"       rc2.Receiver_CNIC,\n" +
            //"       rc2.Relation, rc2.ModifiedOn, rc2.routeCode\n" +
            //"  from Runsheet r\n" +
            //" INNER JOIN RunsheetConsignment RC2\n" +
            //"    ON R.runsheetNumber = RC2.runsheetNumber\n" +
            //"   and r.branchCode = RC2.branchcode\n" +
            //"   and r.routeCode = RC2.RouteCode\n" +
            //"   AND R.createdBy = RC2.createdBy\n" +
            //"\n" +
            //" INNER JOIN Consignment c\n" +
            //"    on c.consignmentNumber = CAST(rc2.consignmentNumber as varchar)\n" +
            //" inner join Branches b\n" +
            //"    on b.branchCode = c.orgin\n" +
            //" where cast(RC2.runsheetNumber as varchar) = '" + clvar.RunSheetNumber + "'\n" +
            //"   and r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            //"   and r.routeCode = '" + clvar.routeCode + "'\n" +
            //" group by rc2.SortOrder,\n" +
            //"          rc2.runsheetNumber,\n" +
            //"          c.consignmentNumber,\n" +
            //"          c.consignee,\n" +
            //"          c.orgin,\n" +
            //"          b.name,\n" +
            //"          c.pieces,\n" +
            //"          rc2.time,\n" +
            //"          RC2.receivedBy,\n" +
            //"          RC2.Status,\n" +
            //"          rc2.Reason,\n" +
            //"          rc2.Comments,\n" +
            //"          rc2.Receiver_CNIC,\n" +
            //"          rc2.Relation, rc2.ModifiedOn, rc2.routeCode\n" +
            //" order by SortOrder ";


            // sqlString = "select ROW_NUMBER() OVER(order by rc2.SortOrder desc) SortOrder,\n" +
            // "       rc2.runsheetNumber RunsheetId,\n" +
            // "       c.consignmentNumber,\n" +
            // "       c.consignee,\n" +
            // "       c.orgin, c.destination,\n" +
            // "       b.name ONAME,\n" +
            // "       c.pieces,\n" +
            // "       cast(rc2.time as time) DeliveryDateTime,\n" +
            // "       RC2.receivedBy,\n" +
            // "       RC2.Status StatusId,\n" +
            // "       rc2.Reason ReasonId,\n" +
            // "       rc2.Comments,\n" +
            // "       rc2.Receiver_CNIC,\n" +
            // "       rc2.Relation,\n" +
            // "       rc2.ModifiedOn,\n" +
            // "       rc2.routeCode,\n" +
            // "       RC2.GivenToRider,\n" +
            // "       ri.riderCode + '-' + ri.firstName + ' ' + ri.lastName GivenToRiderName, c.cod, case when mp.Prefix is null and isnull(c.isapproved,0) = '0' then '1' else '0' end WRONGCN\n" +
            // "  from Runsheet r\n" +
            // " INNER JOIN RunsheetConsignment RC2\n" +
            // "    ON R.runsheetNumber = RC2.runsheetNumber\n" +
            // "   and r.branchCode = RC2.branchcode\n" +
            // "   and r.routeCode = RC2.RouteCode\n" +
            // "   AND R.createdBy = RC2.createdBy\n" +
            // "LEFT OUTER JOIN (SELECT DISTINCT mpcl.prefix, mpcl.length FROM MnP_ConsignmentLengths mpcl) mp\n" +
            // "            ON  LEN(RC2.consignmentNumber) = mp.Length\n" +
            // "            AND mp.Prefix = SUBSTRING(RC2.consignmentNumber, 0, LEN(mp.Prefix) + 1)" +
            // "\n" +
            // " INNER JOIN Consignment c\n" +
            // "    on c.consignmentNumber = CAST(rc2.consignmentNumber as varchar)\n" +
            // " inner join Branches b\n" +
            // "    on b.branchCode = c.orgin\n" +
            // "  left outer join Riders ri\n" +
            // "    on ri.riderCode = RC2.GivenToRider\n" +
            // "      --and ri.routeCode = RC2.RouteCode\n" +
            // "   and ri.branchId = RC2.branchcode\n" +
            // " where cast(RC2.runsheetNumber as varchar) = '" + clvar.RunSheetNumber + "'\n" +
            //"   and r.branchCode = '" + clvar.Branch + "'\n" +
            //"   and r.routeCode = '" + clvar.routeCode + "'\n" +
            // " group by rc2.SortOrder,\n" +
            // "          rc2.runsheetNumber,\n" +
            // "          c.consignmentNumber,\n" +
            // "          c.consignee,\n" +
            // "          c.orgin,c.destination,\n" +
            // "          b.name,\n" +
            // "          c.pieces,\n" +
            // "          rc2.time,\n" +
            // "          RC2.receivedBy,\n" +
            // "          RC2.Status,\n" +
            // "          rc2.Reason,\n" +
            // "          rc2.Comments,\n" +
            // "          rc2.Receiver_CNIC,\n" +
            // "          rc2.Relation,\n" +
            // "          rc2.ModifiedOn,\n" +
            // "          rc2.routeCode,\n" +
            // "          RC2.GivenToRider,\n" +
            // "          ri.riderCode + '-' + ri.firstName + ' ' + ri.lastName, c.cod, mp.prefix, c.isapproved\n" +
            // " order by SortOrder";

            //string sql = " \n"
            //   + "SELECT ROW_NUMBER() OVER(ORDER BY RC2.SortOrder DESC) SortOrder, \n"
            //   + "       RC2.runsheetNumber         RunsheetId, \n"
            //   + "       c.consignmentNumber, \n"
            //   + "       c.consignee, \n"
            //   + "       c.orgin, \n"
            //   + "       c.destination, \n"
            //   + "       b.name                     ONAME, \n"
            //   + "       c.pieces, \n"
            //   + "       CAST(RC2.time AS TIME)     DeliveryDateTime, \n"
            //   + "       RC2.receivedBy, \n"
            //   + "       RC2.Status                 StatusId, \n"
            //   + "       ISNULL(RC2.Reason, 0)                 ReasonId, \n"
            //   + "       RC2.Comments, \n"
            //   + "       RC2.Receiver_CNIC, \n"
            //   + "       RC2.Relation, \n"
            //   + "       RC2.ModifiedOn, \n"
            //   + "       RC2.routeCode, \n"
            //   + "       RC2.GivenToRider, \n"
            //   + "       ri.riderCode + '-' + ri.firstName + ' ' + ri.lastName GivenToRiderName, \n"
            //   + "       c.cod, \n"
            //   + "       CASE  \n"
            //   + "            WHEN mp.Prefix IS NULL AND ISNULL(c.isapproved, 0) = '0' THEN '1' \n"
            //   + "            ELSE '0' \n"
            //   + "       END                        WRONGCN, \n"
            //   + "       ISNULL(mco.RTO, 0) RTO, '0' Flg,\n"
            //   + "       c.consigneePhoneNo,consignerCellNo, c.consigner, c.ispayable  \n"
            //   + "FROM   Runsheet r \n"
            //   + "       INNER JOIN RunsheetConsignment RC2 \n"
            //   + "            ON  R.runsheetNumber = RC2.runsheetNumber \n"
            //   + "            AND r.branchCode = RC2.branchcode \n"
            //   + "            AND r.routeCode = RC2.RouteCode \n"
            //   + "            --AND R.createdBy = RC2.createdBy \n"
            //   + "       LEFT OUTER JOIN ( \n"
            //   + "                SELECT DISTINCT mpcl.prefix, \n"
            //   + "                       mpcl.length \n"
            //   + "                FROM   MnP_ConsignmentLengths mpcl  WHERE mpcl.[STATUS] = '1'\n"
            //   + "            ) mp \n"
            //   + "            ON  LEN(RC2.consignmentNumber) = mp.Length \n"
            //   + "            AND mp.Prefix = SUBSTRING(RC2.consignmentNumber, 0, LEN(mp.Prefix) + 1) \n"
            //   + "       INNER JOIN Consignment c \n"
            //   + "            ON  c.consignmentNumber = CAST(RC2.consignmentNumber AS VARCHAR) \n"
            //   + "       INNER JOIN Branches b \n"
            //   + "            ON  b.branchCode = c.orgin \n"
            //   + "       LEFT OUTER JOIN Riders ri \n"
            //   + "            ON  ri.riderCode = RC2.GivenToRider \n"
            //   + "                --and ri.routeCode = RC2.RouteCode \n"
            //   + "            AND ri.branchId = RC2.branchcode \n"
            //   + "       LEFT OUTER JOIN Mnp_ConsignmentOperations mco \n"
            //   + "            ON  mco.ConsignmentId = c.consignmentNumber \n"
            //   + "WHERE  CAST(RC2.runsheetNumber AS VARCHAR) = '" + clvar.RunSheetNumber + "' \n"
            //   + "       AND r.branchCode = '" + clvar.Branch + "' \n"
            //   + "       AND r.routeCode = '" + clvar.routeCode + "' \n"
            //   + "GROUP BY \n"
            //   + "       RC2.SortOrder, \n"
            //   + "       RC2.runsheetNumber, \n"
            //   + "       c.consignmentNumber, \n"
            //   + "       c.consignee, \n"
            //   + "       c.orgin, \n"
            //   + "       c.destination, \n"
            //   + "       b.name, \n"
            //   + "       c.pieces, \n"
            //   + "       RC2.time, \n"
            //   + "       RC2.receivedBy, \n"
            //   + "       RC2.Status, \n"
            //   + "       RC2.Reason, \n"
            //   + "       RC2.Comments, \n"
            //   + "       RC2.Receiver_CNIC, \n"
            //   + "       RC2.Relation, \n"
            //   + "       RC2.ModifiedOn, \n"
            //   + "       RC2.routeCode, \n"
            //   + "       RC2.GivenToRider,c.consignerCellNo, \n"
            //   + "       ri.riderCode + '-' + ri.firstName + ' ' + ri.lastName, \n"
            //   + "       c.cod, \n"
            //   + "       mp.prefix, \n"
            //   + "       c.isapproved, \n"
            //   + "       mco.RTO,c.consigneePhoneNo, c.consigner, c.ispayable  \n"
            //   + "ORDER BY \n"
            //   + "       SortOrder";
            string sql = @"SELECT ROW_NUMBER() OVER(ORDER BY RC2.SortOrder DESC) SortOrder,  RC2.runsheetNumber RunsheetId,  c.consignmentNumber,  c.consignee, c.orgin
                        , (select case when rtntype = 1 then rtnbranch else c.orgin end from CreditClients where id = c.creditClientId) as rtnBranch
                        , c.destination, 
                        b.name ONAME, c.pieces, CAST(RC2.time AS TIME) DeliveryDateTime, RC2.receivedBy, RC2.Status StatusId, ISNULL(RC2.Reason, 0) ReasonId, RC2.Comments, 
                        RC2.Receiver_CNIC, RC2.Relation, RC2.ModifiedOn, RC2.routeCode, RC2.GivenToRider, ri.riderCode + '-' + ri.firstName + ' ' + ri.lastName GivenToRiderName, c.cod, 
                        CASE WHEN mp.Prefix IS NULL AND ISNULL(c.isapproved, 0) = '0' THEN '1' ELSE '0' END WRONGCN, ISNULL(mco.RTO, 0) RTO, '0' Flg,
                        c.consigneePhoneNo,consignerCellNo, c.consigner, c.ispayable

                        FROM Runsheet r 
                        INNER JOIN RunsheetConsignment RC2 ON R.runsheetNumber = RC2.runsheetNumber AND r.branchCode = RC2.branchcode AND r.routeCode = RC2.RouteCode 
                        LEFT OUTER JOIN ( SELECT DISTINCT mpcl.prefix, mpcl.length FROM MnP_ConsignmentLengths mpcl WHERE mpcl.[STATUS] = '1') mp 
	                        ON LEN(RC2.consignmentNumber) = mp.Length AND mp.Prefix = SUBSTRING(RC2.consignmentNumber, 0, LEN(mp.Prefix) + 1) 
                        INNER JOIN Consignment c ON c.consignmentNumber = CAST(RC2.consignmentNumber AS VARCHAR) 
                        INNER JOIN Branches b ON b.branchCode = c.orgin 
                        LEFT OUTER JOIN Riders ri ON ri.riderCode = RC2.GivenToRider AND ri.branchId = RC2.branchcode 
                        LEFT OUTER JOIN Mnp_ConsignmentOperations mco ON mco.ConsignmentId = c.consignmentNumber 
                        WHERE CAST(RC2.runsheetNumber AS VARCHAR) = '" + clvar.RunSheetNumber + "' AND r.branchCode = '" + clvar.Branch + "' AND r.routeCode = '" + clvar.routeCode + @"'
                        GROUP BY RC2.SortOrder, RC2.runsheetNumber, c.creditClientId,c.consignmentNumber, c.consignee, c.orgin, c.destination, b.name, c.pieces, RC2.time, RC2.receivedBy, RC2.Status, 
                        RC2.Reason, RC2.Comments, RC2.Receiver_CNIC, RC2.Relation, RC2.ModifiedOn, RC2.routeCode, RC2.GivenToRider,c.consignerCellNo, 
                        ri.riderCode + '-' + ri.firstName + ' ' + ri.lastName, c.cod, mp.prefix, c.isapproved, mco.RTO,c.consigneePhoneNo, c.consigner, c.ispayable
                        ORDER BY SortOrder";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        public string UpdatePOD_Consignment(System.Web.UI.WebControls.GridView gv, Cl_Variables clvar, string vehicleID, string vehicleType, Int64 meterStart, Int64 meterEnd)
        {
            CommonFunction func = new CommonFunction();
            //clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();
            string trackQuery = "";
            string Delete_trackQuery = "";
            DataTable consignmentOps = GetConsignmentOps(gv, clvar);
            DataTable consignmentOps_ = new DataTable();
            consignmentOps_.Columns.Add("consignmentID");
            consignmentOps_.Columns.Add("Operation");
            consignmentOps_.AcceptChanges();
            ArrayList Consignment_ = new ArrayList();
            string Consignment = "";
            List<string> cnNumbers = new List<string>();

            Consignment_.Add("UPDATE Runsheet set VehicleNumber = '" + vehicleID + "', VehicleType = '" + vehicleType + "', MeterStart = '" + meterStart.ToString() + "', MeterEnd = '" + meterEnd.ToString() + "', modifiedBy = '" + Session["U_ID"].ToString() + "', modifiedOn = Getdate() where runsheetNumber = '" + clvar.RunsheetNumber + "' and branchcode = '" + clvar.Branch + "'");
            foreach (System.Web.UI.WebControls.GridViewRow row in gv.Rows)
            {
                Telerik.Web.UI.RadDatePicker picker = (Telerik.Web.UI.RadDatePicker)row.FindControl("txt_gDelvDate");
                System.Web.UI.WebControls.DropDownList status = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gStatus");
                System.Web.UI.WebControls.TextBox receivedBy = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gReceivedBy");
                System.Web.UI.WebControls.TextBox time = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gTime");
                System.Web.UI.WebControls.DropDownList reason = (System.Web.UI.WebControls.DropDownList)row.FindControl("dd_gReason");
                System.Web.UI.WebControls.TextBox comments = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_gComments");
                System.Web.UI.WebControls.TextBox cnic = (System.Web.UI.WebControls.TextBox)row.FindControl("txt_receiverCNIC");
                System.Web.UI.WebControls.DropDownList dd_relation = row.FindControl("dd_relation") as DropDownList;
                System.Web.UI.WebControls.TextBox givenToRider = (row.FindControl("txt_gRiderCode") as TextBox);
                System.Web.UI.WebControls.TextBox hd_flag = (row.FindControl("lbl_flg") as TextBox);
                System.Web.UI.WebControls.HiddenField cod = (HiddenField)row.FindControl("hd_cod");
                System.Web.UI.WebControls.HiddenField hd_consigner = (HiddenField)row.FindControl("hd_consigner");
                System.Web.UI.WebControls.HiddenField hd_consigneePhoneNo = (HiddenField)row.FindControl("hd_consigneeCell");
                System.Web.UI.WebControls.HiddenField hd_consignerCellNo = (HiddenField)row.FindControl("hd_consignerCellNo");
                //System.Web.UI.WebControls.Label Lable = (System.Web.UI.WebControls.Label)row.FindControl("Lbl_1");
                bool IsReturned = false;
                bool RTO = false;
                bool IsDelivered = false;

                if (consignmentOps.Select("ConsignmentID = '" + row.Cells[1].Text + "'").Count() == 0)
                {
                    DataRow dr = consignmentOps_.NewRow();
                    dr["consignmentID"] = row.Cells[1].Text;
                    dr["Operation"] = "insert";
                    consignmentOps_.Rows.Add(dr);
                    consignmentOps_.AcceptChanges();
                }
                else
                {
                    DataRow dr = consignmentOps_.NewRow();
                    dr["consignmentID"] = row.Cells[1].Text;
                    dr["Operation"] = "update";
                    consignmentOps_.Rows.Add(dr);
                    consignmentOps_.AcceptChanges();

                    DataRow[] dr_ops = consignmentOps.Select("consignmentID = '" + row.Cells[1].Text + "'");
                    if (dr_ops[0]["IsReturned"].ToString() == "1")
                    {
                        IsReturned = true;
                    }
                    else if (dr_ops[0]["RTO"].ToString().ToUpper() == "TRUE")
                    {
                        RTO = true;
                    }
                    else if (dr_ops[0]["IsDelivered"].ToString().ToUpper() == "TRUE")
                    {
                        IsDelivered = true;
                    }
                }

                Consignment = "'" + row.Cells[1].Text + "'";
                string IsCNModified = "0";
                try
                {
                    IsCNModified = IsConsignmentModified(row.Cells[1].Text.ToString(), clvar._RunsheetNumber).Rows[0][0].ToString();
                }
                catch (Exception ex)
                {
                }
                

                if (row.Enabled == true)
                {
                    if (reason.SelectedValue != "0")// && hd_flag.Text == "1")
                    {
                        cnNumbers.Add(row.Cells[1].Text);
                        #region Delivered Marking


                        if (status.SelectedValue == "55")
                        {
                            string query = "Update runsheetConsignment Set time = '" + txt_date.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' ,\n" +
                                            "                               receivedBy = '" + receivedBy.Text + "' ,\n" +
                                            "                               Comments = '" + comments.Text + "' ,\n" +
                                            "                               Status = '" + status.SelectedValue + "' ,\n" +
                                            "                               Reason = '" + reason.SelectedValue + "' , \n" +
                                            "                               DeliveryDate = '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + "',\n" +
                                            "                               ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', " +
                                            "                               ModifiedOn = GETDATE(), " +
                                            "                               receiver_CNIC = '" + cnic.Text + "', " +
                                            "                               relation = '" + dd_relation.SelectedValue + "'" +
                                            " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + clvar.Branch + "'" +
                                            " and (time <> '" + txt_date.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000'" +
                                            " or receivedBy <> '" + receivedBy.Text + "'" +
                                            " or Comments <> '" + comments.Text + "'" +
                                            " or Status <> '" + status.SelectedValue + "'" +
                                            " or Reason <> '" + reason.SelectedValue + "'" +
                                            " or receiver_CNIC <> '" + cnic.Text + "'" +
                                            " or relation <> '" + dd_relation.SelectedValue + "')";
                            string query1 = "Update rvdbo.runsheetConsignment Set DeliveryDateTime = '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' ,\n" +
                                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                            "                          Comments = '" + comments.Text + "' ,\n" +
                                            "                          StatusID = '" + status.SelectedValue + "' ,\n" +
                                            "                          ReasonID = '" + reason.SelectedValue + "' \n" +
                                            " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + clvar.Branch + "'";
                            
                            Delete_trackQuery = "DELETE FROM ConsignmentsTrackingHistory WHERE runsheetNumber = '" + clvar.RunsheetNumber + "' AND consignmentNumber = '" + row.Cells[1].Text + "' AND stateID = '10'";
                            trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, RunsheetNumber, riderName, TransactionTime, Reason, StatusTime)\n" +
                                            " VALUES (\n" +
                                            "'" + row.Cells[1].Text + "', '10', '" + branchName + "', '" + clvar.RunsheetNumber + "', '" + clvar.RiderCode + "', (select modifiedOn from RunsheetConsignment where consignmentNumber =  '" + row.Cells[1].Text + "' and runsheetNumber = '" + clvar.RunsheetNumber + "'), '" + status.SelectedItem.Text + "','" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000'\n" +
                                            ")";



                            Consignment_.Add(query);
                            //if (IsCNModified == "1")
                            {
                                Consignment_.Add(Delete_trackQuery);
                                Consignment_.Add(trackQuery);
                            }

                            string query2 = "";
                            if (consignmentOps_.Select("consignmentID = '" + row.Cells[1].Text + "'")[0]["Operation"].ToString() == "insert")
                            {
                                query2 = "insert into mnp_consignmentOperations (\n" +
                                    "consignmentID,\n" +
                                    " operationalType,\n" +
                                    " OriginBranchID,\n" +
                                    " DestBranchID,\n" +
                                    " ConsignmentTypeID,\n" +
                                    " isReturned,\n" +
                                    " CnStatus,\n" +
                                    "isMisrouted,\n" +
                                    "weight,\n" +
                                    "ScreenID,\n" +
                                    "ServiceTypeid,\n" +
                                    "CreatedOn,\n" +
                                    "NoOfPieces,\n" +
                                    "isRunsheetAllowed,\n" +
                                    "isDelivered\n" +
                                    ")\n" +
                                    "VALUES (\n" +
                                    "'" + row.Cells[1].Text.Trim() + "',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +//isreturned
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0.5',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "GETDATE(),\n" +
                                    "'1',\n" +
                                    "'0',\n" +
                                    "'1')";
                            }
                            else
                            {
                                // if (!IsReturned && !IsDelivered)
                                //{
                                query2 = "UPDATE mnp_consignmentOperations set isReturned = '0', isDelivered = '1', isRunsheetAllowed = '0', RTO = '0' where consignmentid = '" + row.Cells[1].Text + "'";

                                //  }
                            }
                            Consignment_.Add(query2);



                            string smsContent = "M&P Courier has delivered your shipment with CN #: " + row.Cells[1].Text + " on " + picker.SelectedDate.Value.ToString("dd-MM-yyyy") + ". Thank you for choosing M&P Express Logistics.";

                            string smsCommand = "INSERT INTO MnP_SmsStatus\n" +
                            "  (\n" +
                            "   -- MessageID -- this column value is auto-generated\n" +
                            "   ConsignmentNumber,\n" +
                            "   Recepient,\n" +
                            "   MessageContent,\n" +
                            "   STATUS,\n" +
                            "   CreatedOn,\n" +
                            "   CreatedBy,\n" +
                            "   ModifiedOn,\n" +
                            "   ModifiedBy,\n" +
                            "   RunsheetNumber,\n" +
                            "   ErrorCode,\n" +
                            "   Error)\n";

                            smsCommand += "SELECT A.* \n" +
                            "FROM   (\n" +
                            "           SELECT '" + row.Cells[1].Text + "' CN,\n" +
                            "                  '" + hd_consigneePhoneNo.Value + "' CONSIGNEEPHONE,\n" +
                            "                  '" + smsContent + "'     SMSCONTENT,\n" +
                            "                  '0' STATUS,\n" +
                            "                  GETDATE()       CREATEDON,\n" +
                            "                  '" + HttpContext.Current.Session["U_ID"].ToString() + "' CREATEDBY,\n" +
                            "                  NULL            MODIFIEDON,\n" +
                            "                  NULL            MODIFIEDBY,\n" +
                            "                  '" + clvar._RunsheetNumber + "' RUNSHEETNUMBER,\n" +
                            "                  NULL            ERRORCODE,\n" +
                            "                  NULL            ERROR\n" +
                            "       ) A\n" +
                            "       LEFT OUTER JOIN MNP_SMSSTATUS SMS\n" +
                            "            ON  SMS.CONSIGNMENTNUMBER = A.CN\n" +
                            "            AND SMS.RECEPIENT = A.CONSIGNEEPHONE\n" +
                            "            AND SMS.MESSAGECONTENT = A.SMSCONTENT\n" +
                            "            AND SMS.RUNSHEETNUMBER = A.RUNSHEETNUMBER\n" +
                            "WHERE  SMS.MESSAGEID IS NULL";

                            Consignment_.Add(smsCommand);


                            //string smsContent_consigner = "M&P Courier has delivered your shipment with CN #: " + row.Cells[1].Text + " on " + picker.SelectedDate.Value.ToString("dd-MM-yyyy") + ". Thank you for choosing M&P Express Logistics.";

                            string smsContent_consigner = "Dear Customer, CN# " + row.Cells[1].Text + " has been delivered on " + time.Text + ", " + picker.SelectedDate.Value.ToString("dd-MM-yyyy") + ". For further details visit mulphilog.com/track-shipment/ or call us at 111-202-202.";

                            string smsCommand_consigner = "INSERT INTO MnP_SmsStatus\n" +
                            "  (\n" +
                            "   -- MessageID -- this column value is auto-generated\n" +
                            "   ConsignmentNumber,\n" +
                            "   Recepient,\n" +
                            "   MessageContent,\n" +
                            "   STATUS,\n" +
                            "   CreatedOn,\n" +
                            "   CreatedBy,\n" +
                            "   ModifiedOn,\n" +
                            "   ModifiedBy,\n" +
                            "   RunsheetNumber,\n" +
                            "   ErrorCode,\n" +
                            "   Error)\n";

                            smsCommand_consigner += "SELECT A.* \n" +
                            "FROM   (\n" +
                            "           SELECT '" + row.Cells[1].Text + "' CN,\n" +
                            "                  '" + hd_consignerCellNo.Value + "' consignerCellNo,\n" +
                            "                  '" + smsContent_consigner + "'     SMSCONTENT,\n" +
                            "                  '0' STATUS,\n" +
                            "                  GETDATE()       CREATEDON,\n" +
                            "                  '" + HttpContext.Current.Session["U_ID"].ToString() + "' CREATEDBY,\n" +
                            "                  NULL            MODIFIEDON,\n" +
                            "                  NULL            MODIFIEDBY,\n" +
                            "                  '" + clvar._RunsheetNumber + "' RUNSHEETNUMBER,\n" +
                            "                  NULL            ERRORCODE,\n" +
                            "                  NULL            ERROR\n" +
                            "       ) A\n" +
                            "       LEFT OUTER JOIN MNP_SMSSTATUS SMS\n" +
                            "            ON  SMS.CONSIGNMENTNUMBER = A.CN\n" +
                            "            AND SMS.RECEPIENT = A.consignerCellNo\n" +
                            "            AND SMS.MESSAGECONTENT = A.SMSCONTENT\n" +
                            "            AND SMS.RUNSHEETNUMBER = A.RUNSHEETNUMBER\n" +
                            "       inner join consignment c on A.CN = c.consignmentnumber \n" +
                            "       and c.consignerAccountNo = '0' \n" +
                            "WHERE  SMS.MESSAGEID IS NULL";

                            Consignment_.Add(smsCommand_consigner);

                        }
                        #endregion

                        #region Other Marking
                        else
                        {

                            string givenToClause = "";

                            string query = "Update runsheetConsignment Set";
                            if (time.Text.Trim() != "__:__")
                            {
                                query += " time = '" + txt_date.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' ,\n";
                            }
                            query += "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                   "                            Comments = '" + comments.Text + "' ,\n" +
                                   "                              Status = '" + status.SelectedValue + "', \n" +
                                   "                              Reason = '" + reason.SelectedValue + "',\n" +
                                   "                          ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', ModifiedOn = GETDATE(), receiver_CNIC = '" + cnic.Text + "', Relation = '" + dd_relation.SelectedValue + "'," +
                                   "                        DeliveryDate = '" + picker.SelectedDate.Value.ToString("yyyy-MM-dd") + "' \n" +
                                   " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + clvar.Branch + "'" +
                                            " and (";
                            if (time.Text.Trim() != "__:__")
                                query += " time <> '" + txt_date.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000' or ";
                            query += " receivedBy <> '" + receivedBy.Text + "'" +
                                            " or Comments <> '" + comments.Text + "'" +
                                            " or Status <> '" + status.SelectedValue + "'" +
                                            " or isnull(Reason, '') <> '" + reason.SelectedValue + "'"+
                                            " or receiver_CNIC <> '" + cnic.Text + "'"+
                                            " or Relation <> '" + dd_relation.SelectedValue + "')";
                            string query1 = "Update rvdbo.runsheetConsignment Set --DeliveryDateTime = " + time.Text + "' ,\n" +
                                            "                        receivedBy = '" + receivedBy.Text + "' ,\n" +
                                            "                          Comments = '" + comments.Text + "' ,\n" +
                                            "                          StatusID = '" + status.SelectedValue + "' ,\n" +
                                            "                          ReasonID = '" + reason.SelectedValue + "'  \n" +
                                            //    "                      DeliveryDate =  " + picker.Text + " \n" +
                                            " where ConsignmentId = " + Consignment + " and RunsheetID = '" + clvar._RunsheetNumber + "' and BranchCode = '" + clvar.Branch + "'";
                            trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, RunsheetNumber, riderName, TransactionTime, Reason, StatusTime)\n" +
                                            " VALUES (\n" +
                                            "'" + row.Cells[1].Text + "', '10', '" + branchName + "', '" + clvar.RunsheetNumber + "', '" + clvar.RiderCode + "', (select modifiedOn from RunsheetConsignment where consignmentNumber =  '" + row.Cells[1].Text + "' and runsheetNumber = '" + clvar.RunsheetNumber + "'), '" + status.SelectedItem.Text + "', (select modifiedOn from RunsheetConsignment where consignmentNumber =  '" + row.Cells[1].Text + "' and runsheetNumber = '" + clvar.RunsheetNumber + "')\n" +
                                            ")";

                            Delete_trackQuery = "DELETE FROM ConsignmentsTrackingHistory WHERE runsheetNumber = '" + clvar.RunsheetNumber + "' AND consignmentNumber = '" + row.Cells[1].Text + "' AND stateID = '10'";
                            Consignment_.Add(query);
                            //if (IsCNModified == "1")
                            {
                                Consignment_.Add(Delete_trackQuery);
                                Consignment_.Add(trackQuery);
                            }


                            string query2 = "";
                            if (consignmentOps_.Select("consignmentID = '" + row.Cells[1].Text + "'")[0]["Operation"].ToString() == "insert")
                            {
                                if (status.SelectedItem.Text.ToUpper() == "RETURNED")
                                {
                                    query2 = "insert into mnp_consignmentOperations (\n" +
                                    "consignmentID,\n" +
                                    " operationalType,\n" +
                                    " OriginBranchID,\n" +
                                    " DestBranchID,\n" +
                                    " ConsignmentTypeID,\n" +
                                    " isReturned,\n" +
                                    " CnStatus,\n" +
                                    "isMisrouted,\n" +
                                    "weight,\n" +
                                    "ScreenID,\n" +
                                    "ServiceTypeid,\n" +
                                    "CreatedOn,\n" +
                                    "NoOfPieces,\n" +
                                    "isRunsheetAllowed,\n" +
                                    "isDelivered\n" +
                                    ")\n" +
                                    "VALUES (\n" +
                                    "'" + row.Cells[1].Text.Trim() + "',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'1',\n" +//isreturned
                                    "'0',\n" +
                                    "'0',\n" +
                                    "'0.5',\n" +
                                    "'0',\n" +
                                    "'0',\n" +
                                    "GETDATE(),\n" +
                                    "'1',\n" +
                                    "'0',\n" + //isRunsheetAllowed
                                    "'0')"; //isDelivered
                                }
                                else if (reason.SelectedValue == "58")
                                {
                                    query2 = "insert into mnp_consignmentOperations (\n" +
                                   "consignmentID,\n" +
                                   " operationalType,\n" +
                                   " OriginBranchID,\n" +
                                   " DestBranchID,\n" +
                                   " ConsignmentTypeID,\n" +
                                   " isReturned,\n" +
                                   " CnStatus,\n" +
                                   "isMisrouted,\n" +
                                   "weight,\n" +
                                   "ScreenID,\n" +
                                   "ServiceTypeid,\n" +
                                   "CreatedOn,\n" +
                                   "NoOfPieces,\n" +
                                   "isRunsheetAllowed,\n" +
                                   "isDelivered, RTO\n" +
                                   ")\n" +
                                   "VALUES (\n" +
                                   "'" + row.Cells[1].Text + "',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +//isreturned
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0.5',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "GETDATE(),\n" +
                                   "'1',\n" +
                                   "'1',\n" + //isRunsheetAllowed
                                   "'0', '1')"; //isDelivered
                                }
                                else
                                {
                                    query2 = "insert into mnp_consignmentOperations (\n" +
                                   "consignmentID,\n" +
                                   " operationalType,\n" +
                                   " OriginBranchID,\n" +
                                   " DestBranchID,\n" +
                                   " ConsignmentTypeID,\n" +
                                   " isReturned,\n" +
                                   " CnStatus,\n" +
                                   "isMisrouted,\n" +
                                   "weight,\n" +
                                   "ScreenID,\n" +
                                   "ServiceTypeid,\n" +
                                   "CreatedOn,\n" +
                                   "NoOfPieces,\n" +
                                   "isRunsheetAllowed,\n" +
                                   "isDelivered, RTO\n" +
                                   ")\n" +
                                   "VALUES (\n" +
                                   "'" + row.Cells[1].Text + "',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0',\n" +//isreturned
                                   "'0',\n" +
                                   "'0',\n" +
                                   "'0.5',\n" +
                                   "'0',\n" +
                                   "'0',\n" +
                                   "GETDATE(),\n" +
                                   "'1',\n" +
                                   "'1',\n" + //isRunsheetAllowed
                                   "'0', '0')"; //isDelivered
                                }
                            }
                            else
                            {
                                if (status.SelectedItem.Text.ToUpper() == "RETURNED")//&& !IsDelivered && !IsReturned)
                                {
                                    query2 = "UPDATE mnp_consignmentOperations set isReturned = '1', isDelivered = '0', isRunsheetAllowed = '0' where consignmentid = '" + row.Cells[1].Text + "'";

                                }
                                else if (reason.SelectedValue == "58")// && !IsDelivered && !IsReturned)
                                {
                                    query2 = "UPDATE mnp_consignmentOperations set isReturned = '0', isDelivered = '0', isRunsheetAllowed = '1', RTO = '1' where consignmentid = '" + row.Cells[1].Text + "'";
                                }
                                else //if (!RTO && !IsDelivered && !IsReturned)
                                {
                                    query2 = "UPDATE mnp_consignmentOperations set isReturned = '0', isDelivered = '0', isRunsheetAllowed = '1', RTO = '0' where consignmentid = '" + row.Cells[1].Text + "'";
                                }

                            }
                            Consignment_.Add(query2);

                            if ((cod.Value == "1" || cod.Value.ToUpper() == "TRUE") && (hd_flag.Text == "1" || hd_flag.Text.ToUpper() == "TRUE"))
                            {
                                string smsContent = "Dear Customer, M&P Courier visited your address to deliver " + hd_consigner.Value + " shipment under CN #" + row.Cells[1].Text + " but it could not deliver. Pls contact M&P Customer Services 111-202-202";
                                query2 = "insert into mnp_smsStatus (ConsignmentNumber, Recepient, MessageContent, Status, Createdon, CreatedBy, RunsheetNumber) \n" +
                                         "values ('" + row.Cells[1].Text + "', '" + hd_consigneePhoneNo.Value + "', '" + smsContent + "', '0', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', '" + txt_runsheetNumber.Text + "')";
                                //Consignment_.Add(query2);
                            }
                            //Consignment_.Add(query1);

                        }
                        #endregion
                    }
                }
                else
                {
                    ///////////////////////////
                    if (receivedBy.Text == "WRONG CN")
                    {
                        string query = "update runsheetconsignment set receivedby = case when (receivedby = '' OR receivedBy is null) then 'WRONG CN' else receivedby end, comments = 'WRONG CN' \n" +
                        " where ConsignmentNumber = " + Consignment + " and runsheetNumber = '" + clvar._RunsheetNumber + "' and branchcode = '" + clvar.Branch + "'" +
                                            " and (time <> '" + txt_date.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + time.Text + ":00.000'" +
                                            " or receivedBy <> '" + receivedBy.Text + "'" +
                                            " or Comments <> '" + comments.Text + "')";
                        Consignment_.Add(query);
                    }

                }
            }

            string auditQuery = "INSERT INTO RUNSHEETCONSIGNMENT_AUDIT SELECT * FROM RUNSHEETCONSIGNMENT WHERE RUNSHEETNUMBER = '" + clvar.RunsheetNumber + "' and BranchCode = '" + clvar.Branch + "' and consignmentNumber in (";
            string cns = "";
            foreach (string str in cnNumbers)
            {
                cns += "'" + str + "'";
            }

            cns = cns.Replace("''", "','");

            auditQuery += cns + ")";
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;

            sqlcon.Open();
            SqlCommand dbCommand = new SqlCommand("", sqlcon);

            var sqlStatementArray = dbCommand.CommandText.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            trans = sqlcon.BeginTransaction();

            try
            {
                dbCommand.Transaction = trans;
                if (cnNumbers.Count > 0)
                {
                    dbCommand.CommandText = auditQuery;
                    dbCommand.ExecuteNonQuery();
                }
                foreach (string sqlStatement in Consignment_)
                {
                    dbCommand.CommandText = sqlStatement;
                    dbCommand.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception Ex)
            {
                trans.Rollback();

                return Ex.Message;
            }

            sqlcon.Close();

            return "OK";
        }
        public DataTable GetConsignmentOps(GridView gv, Cl_Variables clvar)
        {
            string numbers = "";
            foreach (GridViewRow row in gv.Rows)
            {
                numbers += "'" + row.Cells[1].Text.Trim() + "'";
            }
            numbers = numbers.Replace("''", "','");

            string query = "select * from mnp_consignmentOperations c where c.consignmentId in (" + numbers + ")";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
        }

        public DataTable IsConsignmentModified(string ConsignmentNumber, string RunsheetNo)
        {
            string numbers = "";

            string query = "select case when DATEDIFF(MINUTE, modifiedOn, getdate())< 5 then 1 else 0 end as a from RunsheetConsignment where consignmentNumber = '"+ConsignmentNumber+"' and runsheetNumber = '"+ RunsheetNo + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            finally { con.Close(); }
            return dt;
        }

        private string IsMobileNumberValid(string mobileNumber)
        {
            string _mobileNumber = "";
            // remove all non-numeric characters
            _mobileNumber = CleanNumber(mobileNumber);

            // trim any leading zeros
            _mobileNumber = _mobileNumber.TrimStart(new char[] { '0' });

            // check for this in case they've entered 44 (0)xxxxxxxxx or similar
            if (_mobileNumber.StartsWith("920"))
            {
                _mobileNumber = _mobileNumber.Remove(2, 1);
            }

            // add country code if they haven't entered it
            if (!_mobileNumber.StartsWith("92"))
            {
                _mobileNumber = "92" + _mobileNumber;
            }

            // check if it's the right length
            if (_mobileNumber.Length != 12)
            {
                return "0";
            }

            return _mobileNumber;
        }

        private string CleanNumber(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }

        public void Alert(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
        }

        private void send_Email(string email, string finalResult, string subject)
        {
            string newjobemail = finalResult.ToString();

            MailMessage message = new MailMessage();


            message.From = new MailAddress("System.cod@mulphilog.com");
            message.To.Add(new MailAddress("nasir.hussain@mulphico.pk"));
            message.To.Add(new MailAddress("shaheer.sohail@mulphilog.com"));

            message.Subject = subject;
            message.IsBodyHtml = true;

            message.Body = finalResult;


            SmtpClient mail = new SmtpClient();
            mail.Port = int.Parse("25");
            mail.Host = "192.168.200.20 ";
            mail.Credentials = new System.Net.NetworkCredential("System.cod@mulphilog.com", "system1223+");
            try
            {
                //mail.Send(message);
            }
            catch (Exception Err)
            {

            }

        }

        public DataTable GetCNDetailsForEmail(Cl_Variables clvar)
        {

            string sql = "SELECT ROW_NUMBER() OVER(ORDER BY c.consignmentNumber) Sr,  \n"
               + "                  GETDATE()     ReportTime,  \n"
               + "                  c.consignmentNumber,  \n"
               + "                  c.bookingDate,  \n"
               + "                  b.sname       ORG,  \n"
               + "                  b2.sname      DSTN,  \n"
               + "                  c.pieces,  \n"
               + "                  c.[weight],  \n"
               + "                  c.consignee,  \n"
               + "                  c.[address],  \n"
               + "                  (SUM(cdn.codAmount) / COUNT(cdn.consignmentNumber)) CODAmount,  \n"
               + "                  l.AttributeValue     Reason,  \n"
               + "                  '' SystemRemarks  \n"
               + "           FROM   Consignment c  \n"
               + "                  INNER JOIN Branches b  \n"
               + "                       ON  b.branchCode = c.orgin  \n"
               + "                  INNER JOIN Branches b2  \n"
               + "                       ON  b2.branchCode = c.destination  \n"
               + "                  INNER JOIN CODConsignmentDetail_New cdn  \n"
               + "                       ON  cdn.consignmentNumber = c.consignmentNumber  \n"
               + "                  INNER JOIN RunsheetConsignment rc  \n"
               + "                       ON  rc.consignmentNumber = c.consignmentNumber  \n"
               + "                  LEFT OUTER JOIN rvdbo.Lookup l \n"
               + "                  ON l.Id = rc.Reason \n"
               + "WHERE  c.consignmentNumber IN (" + clvar.CheckCondition + ") \n"
               + "       AND rc.runsheetNumber = '" + clvar.RunsheetNumber + "' \n"
               + "       AND rc.RouteCode = '" + clvar.routeCode + "' \n"
               + "           GROUP BY  \n"
               + "                  c.consignmentNumber,  \n"
               + "                  c.bookingDate,  \n"
               + "                  b.sname,  \n"
               + "                  b2.sname,  \n"
               + "                  c.pieces,  \n"
               + "                  c.[weight],  \n"
               + "                  c.consignee,  \n"
               + "                  c.[address],  \n"
               + "                  l.AttributeValue";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());

            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable GetEmailAddresses()
        {
            string query = "SELECT * from mnp_podEmailSetup where zonecode = '" + HttpContext.Current.Session["zoneCode"].ToString() + "'";
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        protected void dd_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetBranches();
        }


        private static string[][] CheckCODControls(string cn,string SelectedValue)
        {
            List<string[]> resp = new List<string[]>();
            string[] Response = { "", "" };

            string ConsignmentNo = cn;
            string Reason = "";
            string status = "true";

            if (cn.StartsWith("5"))
            {
                if (cn.Length==15)
                {
                    DataTable RTSProcessDT = new DataTable();
                    DataTable BookingDT = CheckConsignmentBooking(ConsignmentNo);
                    DataTable FirstProcessDT = CheckFirstProcessOrigin(ConsignmentNo);
                    if (SelectedValue == "58" || SelectedValue == "59")
                    {
                        RTSProcessDT = CheckRTSProcessOrigin(ConsignmentNo);
                    }

                    if (BookingDT.Rows.Count > 0)
                    {
                        //if (BookingDT.Rows[0]["bypass"].ToString() == "0")
                        {
                            if (FirstProcessDT.Rows.Count > 0 || BookingDT.Rows[0]["isApproved"].ToString() == "True")
                            {
                                if (BookingDT.Rows[0]["status"].ToString() == "9")
                                {
                                    status = "false";
                                    Reason = "Alert: Consignment is Void perform Arrival";
                                }
                                else
                                {
                                    if (RTSProcessDT.Rows.Count > 0)
                                    {
                                        status = "true";
                                        Reason = "Success";
                                    }
                                    else
                                    {
                                        if (SelectedValue == "58" || SelectedValue == "59")
                                        {
                                            status = "false";
                                            Reason = "Alert: Shipper advice is necessary, CN: " + ConsignmentNo;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                status = "false";
                                Reason = "Alert: Not Approved from orign, CN: " + ConsignmentNo;
                            }
                        }
                        Response[0] = status;
                        Response[1] = Reason;
                        resp.Add(Response);
                    }
                    else
                    {

                        Response[0] = "false";
                        Response[1] = "Alert: no booking found for this COD, CN: " + ConsignmentNo;
                    }
                }
                else
                {
                    Response[0] = "false";
                    Response[1] = "Alert: COD CN must have length of 15, CN: " + ConsignmentNo;
                    resp.Add(Response);
                }
            }
            Response[0] = status;
            Response[1] = Reason;
            resp.Add(Response);
            return resp.ToArray();
        }
        
        private static DataTable CheckConsignmentBooking(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = " SELECT * FROM Consignment c WHERE c.consignmentNumber = '" + Consignment + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        private static DataTable CheckFirstProcessOrigin(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = " select * from consignment where consignmentNumber = '" + Consignment + "' and isApproved = 1 ";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        private static DataTable CheckRTSProcessOrigin(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = " select * from MNP_NCI_Request where CallTrack = 2 and ConsignmentNumber = '"+ Consignment + "' ";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
    }
}