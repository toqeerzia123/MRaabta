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
    public partial class Scheduler : System.Web.UI.Page
    {
        string Mode = "A";
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString(); // connection string
        public string getMSG(string msg)
        {
            string javas = "<script type=\"text/javascript\"> swal(' " + msg + "') </script>";
            return javas;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["PR_MSG"] != null)
            {
                string msg = Session["PR_MSG"].ToString();
                Session["PR_MSG"] = null;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", getMSG(msg));
            }

            if (Request.QueryString["Mode"] != null)
            {
                Mode = Request.QueryString["Mode"].ToString();
            }


            if (!IsPostBack)
            {
                Pickup_Type_SelectedIndexChanged(null, null);
                rdschedulerPickup_SelectedIndexChanged(null, null);
                loadCombo();
                txtPickupDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
                if (Mode == "E")
                {
                    ddlLocation1.Visible = false;
                    txtlocation.Visible = true;

                    ddlRouteCode.Visible = false;
                    txtRoutCode.Visible = true;
                    btnSubmit.Visible = false;
                    btnReset.Visible = false;
                    rdPickupType.Enabled = false;
                    rdschedulerPickup.Enabled = false;
                    ddlServiceID.Enabled = false;
                    ddlProductID.Enabled = false;
                    txtAccountNumber.ReadOnly = true;
                    ddlRouteCode.Enabled = false;
                    String SID = "0";
                    if (Request.QueryString["ID"] != null)
                    {
                        SID = Request.QueryString["ID"].ToString();
                    }


                    LoadDataView(SID);
                    //  loadComboofserviceType();

                    Pickup_Type_SelectedIndexChanged(null, null);
                    //rdScheduled.Enabled = false;
                    chkWeekDay.Enabled = false;
                    chkMonthDat.Enabled = false;
                    ddlScheduleType.Enabled = false;


                }






                if (rdPickupType.SelectedValue == "Cash")
                {
                    ddlAutoAssign.Enabled = false;
                    ddlAutoAssign.SelectedValue = "No";
                    rdScheduled.SelectedValue = "No";
                    //   rdScheduled.Enabled = false;
                    ddl_Schedule.Visible = false;
                    Label_Schedule.Visible = false;
                    rWeek.Visible = false;
                    rMonth.Visible = false;


                }

            }
        }

        protected void LoadDataView(String SchedulerID)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string strquery = @"select SchedulerID, pickupdate,weight,userid,pr.status,routecode,pickuptype,pr.CUSTOMERID,accountnumber,
accountname,location,FName,middlename,lastname,contactno,whatsappno,emailid,house,
floorno,building,plotno,street,sector,area,postalcode,city,shipments,weighttypeid,
product,service,needpackingmaterialid,contents,loadersrequired,handlingtoolid,additionalremarks,
scheduled,timeslot,scheduletype,routedescription,autoassign,pr.branchCode as Branch,pr.RiderCode,RiderName,locationName ,routeCodeName from PR_Scheduler_PickupRequest pr
Inner Join (select * from CreditClients where branchCode=@branchCode ) as cc on  pr.CUSTOMERID =cc.id
left join (select riderCode,firstName as RiderName from  Riders  where userTypeId in ('217', '72', '90') ) r on pr.riderCode=r.riderCode
left join (select locationName,locationID from COD_CustomerLocations where status=1 and  brancahCode =@branchCode) L 
on l.locationID=Location
left join (select routeCode AS RouteCodeID,routeCode+'-'+name  as routeCodeName from Routes where status=1 and bid= @branchCode) RC 
on pr.RouteCode= rc.RouteCodeID
where SchedulerID=@SchedulerID and rec_status=1 and pr.branchCode=@branchCode";
                    using (SqlCommand cmd = new SqlCommand(strquery))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@SchedulerID", SchedulerID.Trim());
                            cmd.Parameters.AddWithValue("@branchCode", Session["BranchCode"].ToString());
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {




                                    txtAccountNumber.Text = dt.Rows[0]["AccountNumber"].ToString();
                                    txtAccountName.Text = dt.Rows[0]["AccountName"].ToString();
                                    // ddlLocation1.SelectedValue = dt.Rows[0]["Location"].ToString();
                                    //  ddlLocation1.Text = dt.Rows[0]["asdas"].ToString();
                                    txtlocation.Text = dt.Rows[0]["locationName"].ToString();

                                    ddlLocation1.Items.Insert(0, new ListItem(dt.Rows[0]["locationName"].ToString(), dt.Rows[0]["Location"].ToString()));
                                    CustomerID.Value = dt.Rows[0]["CUSTOMERID"].ToString();
                                    txtPickupDate.Text = Convert.ToDateTime(dt.Rows[0]["PickupDate"]).ToString("yyyy-MM-dd");
                                    txtWeight.Text = dt.Rows[0]["weight"].ToString();
                                    txtRoutCode.Text = dt.Rows[0]["routeCodeName"].ToString();
                                    ddlRouteCode.SelectedValue = dt.Rows[0]["RouteCode"].ToString();

                                    //   ddlRouteCode.Items.Insert(0, new ListItem(dt.Rows[0]["routeCodeName"].ToString(), dt.Rows[0]["RouteCode"].ToString()));
                                    rdPickupType.Text = dt.Rows[0]["PickupType"].ToString();

                                    txtFirstName.Text = dt.Rows[0]["FName"].ToString();
                                    txtMiddleName.Text = dt.Rows[0]["MiddleName"].ToString();
                                    txtLastName.Text = dt.Rows[0]["LastName"].ToString();
                                    txtContactNo.Text = dt.Rows[0]["ContactNo"].ToString();


                                    txtWhatsappNo.Text = dt.Rows[0]["WhatsappNo"].ToString();
                                    txtEmailID.Text = dt.Rows[0]["EmailID"].ToString();
                                    txtHouse.Text = dt.Rows[0]["House"].ToString();
                                    txtFloorNo.Text = dt.Rows[0]["FloorNo"].ToString();

                                    txtBuilding.Text = dt.Rows[0]["Building"].ToString();

                                    txtPlotNo.Text = dt.Rows[0]["PlotNo"].ToString();

                                    txtStreet.Text = dt.Rows[0]["Street"].ToString();
                                    txtSector.Text = dt.Rows[0]["Sector"].ToString();
                                    txtArea.Text = dt.Rows[0]["Area"].ToString();
                                    txtPostalCode.Text = dt.Rows[0]["PostalCode"].ToString();
                                    txtShipments.Text = dt.Rows[0]["Shipments"].ToString();


                                    ddlWeightTypeID.SelectedValue = dt.Rows[0]["WeightTypeID"].ToString();
                                    ddlProductID.SelectedValue = dt.Rows[0]["Product"].ToString();
                                    ddlServiceID.Items.Insert(0, new ListItem(dt.Rows[0]["Service"].ToString(), dt.Rows[0]["Service"].ToString()));
                                    ddlNeedPackingMaterialID.SelectedValue = dt.Rows[0]["NeedPackingMaterialID"].ToString();
                                    txtContents.Text = dt.Rows[0]["Contents"].ToString();

                                    ddlLoaders.SelectedValue = dt.Rows[0]["LoadersRequired"].ToString();
                                    ddlHandlingToolID.SelectedValue = dt.Rows[0]["HandlingToolID"].ToString();
                                    txtAdditionalRemarks.Text = dt.Rows[0]["AdditionalRemarks"].ToString();

                                    rdScheduled.SelectedValue = dt.Rows[0]["Scheduled"].ToString();
                                    ddlTimeslot.SelectedValue = dt.Rows[0]["Timeslot"].ToString();
                                    txtridercode.Text = dt.Rows[0]["RiderCode"].ToString();
                                    txtRiderName.Text = dt.Rows[0]["RiderName"].ToString();
                                    txtRouteDescription.Text = dt.Rows[0]["RouteDescription"].ToString();

                                    ddlAutoAssign.SelectedValue = dt.Rows[0]["AutoAssign"].ToString();



                                    ddlScheduleType.SelectedValue = dt.Rows[0]["ScheduleType"].ToString();




                                    if (ddlScheduleType.SelectedValue == "2")
                                    {
                                        Scheduled_Type_SelectedIndexChanged(null, null);
                                        DataTable dt1 = getshedule(SchedulerID);
                                        for (int i = 0; i < dt1.Rows.Count; i++)
                                        {
                                            chkWeekDay.Items.FindByValue(dt1.Rows[i]["Value"].ToString()).Selected = true;
                                        }

                                    }
                                    if (ddlScheduleType.SelectedValue == "3")
                                    {
                                        Scheduled_Type_SelectedIndexChanged(null, null);
                                        DataTable dt1 = getshedule(SchedulerID);
                                        for (int i = 0; i < dt1.Rows.Count; i++)
                                        {
                                            chkMonthDat.Items.FindByValue(dt1.Rows[i]["Value"].ToString()).Selected = true;
                                        }
                                    }


                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected DataTable getshedule(String SID)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString();
            DataTable dt2 = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string strquery = @"SELECT  [WeekMonthID],[SchedulerID]  ,[SchduleType]  ,[Value] ,[rec_status] FROM [PR_WeekMonth]  
where [rec_status]=1 and [SchedulerID] in (SELECT [SchedulerID] FROM PR_Scheduler_PickupRequest where SchedulerID =@SchedulerID)";
                using (SqlCommand cmd = new SqlCommand(strquery))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@SchedulerID", SID.Trim());
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dt2 = dt;
                        }

                    }
                }
            }
            return dt2;
        }
        private void loadCombo()
        {
            try
            {

                SqlConnection con = new SqlConnection(constr);

                con.Open();
                string comb = "select BranchCode as ID,Name as Name from Branches where branchCode =" + Session["BranchCode"];
                SqlDataAdapter adptb = new SqlDataAdapter(comb, con);
                DataTable dtb = new DataTable();
                adptb.Fill(dtb);
                con.Close();
                ddlBranchCode.DataSource = dtb;
                ddlBranchCode.DataBind();
                ddlBranchCode.DataTextField = "Name";
                ddlBranchCode.DataValueField = "ID";
                ddlBranchCode.DataBind();


                con.Open();
                string com1 = "select * from PR_packing_material where rec_Status=1 order by id";
                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                ddlNeedPackingMaterialID.DataSource = dt1;
                //ddlNeedPackingMaterialID.DataBind();
                ddlNeedPackingMaterialID.DataTextField = "Name";
                ddlNeedPackingMaterialID.DataValueField = "ID";
                ddlNeedPackingMaterialID.DataBind();
                ListItem item11 = new ListItem("Not Required", "-2");

                ddlNeedPackingMaterialID.Items.Insert(0, item11);
                ddlNeedPackingMaterialID.SelectedIndex = 0;
                dt1.Dispose();
                dt1.Clear();

                con.Open();
                string com2 = "select * from PR_handling_tool";
                SqlDataAdapter adpt2 = new SqlDataAdapter(com2, con);
                DataTable dt2 = new DataTable();
                adpt2.Fill(dt2);
                con.Close();
                ddlHandlingToolID.DataSource = dt2;
                ddlHandlingToolID.DataBind();
                ddlHandlingToolID.DataTextField = "Name";
                ddlHandlingToolID.DataValueField = "ID";
                ddlHandlingToolID.DataBind();
                ddlHandlingToolID.Items.Insert(0, item11);
                ddlHandlingToolID.SelectedIndex = 0;
                dt2.Dispose();
                dt2.Clear();


                con.Open();
                string com3 = "select * from PR_WeightType";
                SqlDataAdapter adpt3 = new SqlDataAdapter(com3, con);
                DataTable dt3 = new DataTable();
                adpt3.Fill(dt3);
                con.Close();
                ddlWeightTypeID.DataSource = dt3;
                ddlWeightTypeID.DataBind();
                ddlWeightTypeID.DataTextField = "Name";
                ddlWeightTypeID.DataValueField = "ID";
                ddlWeightTypeID.DataBind();
                ddlWeightTypeID.Items.Insert(0, item11);
                ddlWeightTypeID.SelectedIndex = 0;
                dt3.Dispose();
                dt3.Clear();

                con.Open();
                string com4 = "select 0 ID,'Schedule Type' as Name union all select ID, Name from PR_ScheduleType";
                SqlDataAdapter adpt4 = new SqlDataAdapter(com4, con);
                DataTable dt4 = new DataTable();
                adpt4.Fill(dt4);
                con.Close();
                ddlScheduleType.DataSource = dt4;
                ddlScheduleType.DataBind();
                ddlScheduleType.DataTextField = "Name";
                ddlScheduleType.DataValueField = "ID";
                ddlScheduleType.DataBind();
                dt4.Dispose();
                dt4.Clear();

                con.Open();
                string com5 = "select * from PR_Slot";
                SqlDataAdapter adpt5 = new SqlDataAdapter(com5, con);
                DataTable dt5 = new DataTable();
                adpt5.Fill(dt5);
                con.Close();
                ddlTimeslot.DataSource = dt5;
                ddlTimeslot.DataBind();
                ddlTimeslot.DataTextField = "Slot";
                ddlTimeslot.DataValueField = "Slot";
                ddlTimeslot.DataBind();
                ListItem item3 = new ListItem("Select Slot", "0");
                ddlTimeslot.Items.Insert(0, item3);
                dt5.Dispose();
                dt5.Clear();

                con.Open();
                string com6 = @" select r.routeCode AS ID,r.routeCode+'-'+name  as routeCode  from Routes r inner join Riders rr on rr.routeCode = r.routeCode and r.RiderCode = rr.riderCode where r.status=1 and userTypeId in ('72','90','217') and bid= '" + Session["BranchCode"].ToString() + "' order by routeCode";
                SqlDataAdapter adpt6 = new SqlDataAdapter(com6, con);
                DataTable dt6 = new DataTable();
                adpt6.Fill(dt6);
                con.Close();
                ddlRouteCode.DataSource = dt6;
                ddlRouteCode.DataBind();
                ddlRouteCode.DataTextField = "routeCode";
                ddlRouteCode.DataValueField = "ID";
                ddlRouteCode.DataBind();
                ListItem itemr = new ListItem("Select Route Code", "-1");

                ddlRouteCode.Items.Insert(0, itemr);
                dt6.Dispose();
                dt6.Clear();



                con.Open();
                string com8 = @"select distinct Products as ID , Products as Name from ServiceTypes_New where Products  in (
'Cash on Delivery',
'Domestic',
'International',
'Road n Rail'
)
union all
select  'Cash on Delivery' as ID , 'Cash on Delivery' as Name";
                SqlDataAdapter adpt8 = new SqlDataAdapter(com8, con);
                DataTable dt8 = new DataTable();
                adpt8.Fill(dt8);
                con.Close();
                ddlProductID.DataSource = dt8;
                ddlProductID.DataBind();
                ddlProductID.DataTextField = "Name";
                ddlProductID.DataValueField = "ID";
                ddlProductID.DataBind();
                ListItem item5 = new ListItem("Select Product", "-1");
                ddlProductID.Items.Insert(0, item5);
                dt8.Dispose();
                dt8.Clear();

                loadComboofserviceType();

            }
            catch (Exception ex)
            {

            }
        }

        private void loadComboofserviceType()
        {
            try
            {

                SqlConnection con = new SqlConnection(constr);


                con.Open();
                string com9 = @"  select   ID ,  Name ,Products from (
 select serviceTypeName as ID , serviceTypeName as Name ,Products From ServiceTypes_New where status = '1' and [name] in  (
'overnight',
'Same Day',
'Second Day',
'Express Cargo') OR[name] in  (select[name] From ServiceTypes_New
where status = '1' and Products = 'international')

union all
select 'overnight' as ID  ,'overnight' as Name,'Cash on Delivery' as Products
union all
select 'Second Day' as ID  ,'Second Day' as Name,'Cash on Delivery' as Products
) M where Products = '" + ddlProductID.SelectedValue + "'";
                SqlDataAdapter adpt9 = new SqlDataAdapter(com9, con);
                DataTable dt9 = new DataTable();
                adpt9.Fill(dt9);
                con.Close();
                ddlServiceID.DataSource = dt9;
                ddlServiceID.DataBind();
                ddlServiceID.DataTextField = "Name";
                ddlServiceID.DataValueField = "ID";
                ddlServiceID.DataBind();
                ListItem item6 = new ListItem("Select Service", "-1");
                ddlServiceID.Items.Insert(0, item6);
                dt9.Dispose();
                dt9.Clear();

            }
            catch (Exception)
            {

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                rdPickupType.Text = "Credit";
                rdPickupType.SelectedValue = "Credit";
                //ddlAutoAssign.SelectedValue = "Yes";
                //string script = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });";
                //ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);
                //System.Threading.Thread.Sleep(5000);
                if (rdScheduled.SelectedValue == "Yes" && ddlScheduleType.SelectedValue == "0")
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Schedule Type')", true);
                    return;

                }
                if (ddlTimeslot.SelectedValue == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Time Slot')", true);
                    return;
                }
                if (ddlAutoAssign.SelectedValue == "Yes" && ddlRouteCode.SelectedValue == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Route code')", true);
                    return;
                }
                if (rdScheduled.SelectedValue == "Yes")
                {
                    bool chkweek = false;
                    if (ddlScheduleType.SelectedValue == "2")
                    {
                        for (int i = 0; i < chkWeekDay.Items.Count; i++)
                        {
                            if (chkWeekDay.Items[i].Selected)
                            {
                                chkweek = true;
                            }

                        }
                        if (!chkweek)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please Select Week Day')", true);
                            return;
                        }
                    }

                    bool chkmonthday = false;
                    if (ddlScheduleType.SelectedValue == "3")
                    {
                        for (int i = 0; i < chkMonthDat.Items.Count; i++)
                        {
                            if (chkMonthDat.Items[i].Selected)
                            {
                                chkmonthday = true;
                            }

                        }
                        if (!chkmonthday)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please Select Month Day')", true);
                            return;
                        }
                    }



                }

                string strRnd = getRandomNum();
                if (rdschedulerPickup.SelectedValue == "Yes" && rdPickupType.SelectedValue == "Credit")
                {

                    if (rdScheduled.SelectedValue == "No")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please Select Scheduler')", true);
                        return;
                    }
                    else
                    {
                        RequestAAD("");
                    }

                    //   ResetValues();
                    Session["PR_MSG"] = "Only Scheduler Request Generated";

                    Response.Redirect("Scheduler.aspx", true);
                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Only Scheduler Request Generated')", true);
                    return;
                }
                if (rdPickupType.SelectedValue == "Credit" && txtAccountNumber.Text != "" && rdschedulerPickup.SelectedValue == "No")
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(@"insert into pr_pickuprequest (pickupdate,weight,userid,status,routecode,pickuptype,CUSTOMERID,accountnumber,accountname,location,FName,middlename,lastname,contactno,whatsappno,emailid,house,floorno,building,plotno,street,sector,area,postalcode,city,shipments,weighttypeid,product,service,needpackingmaterialid,contents,loadersrequired,handlingtoolid,additionalremarks,scheduled,timeslot,scheduletype,routedescription,autoassign,Branch,RiderCode) 
                                                                   values (@pickupdate,@weight,@userid,@status,@routecode,@PickupType,@CUSTOMERID,@accountnumber,@accountname,@location,@FName,@middlename,@lastname,@contactno,@whatsappno,@emailid,@house,@floorno,@building,@plotno,@street,@sector,@area,@postalcode,@city,@shipments,@weighttypeid,@product,@service,@needpackingmaterialid,@contents,@loadersrequired,@handlingtoolid,@additionalremarks,@Scheduled,@timeslot,@scheduletype,@routedescription,@autoassign,@Branch,@RiderCode)

                                                                 SELECT SCOPE_IDENTITY()"))
                        {
                            if (rdPickupType.SelectedValue == "Credit")
                            {
                                cmd.Parameters.AddWithValue("@AccountNumber", txtAccountNumber.Text);
                                cmd.Parameters.AddWithValue("AccountName", txtAccountName.Text);
                                cmd.Parameters.AddWithValue("@Location", ddlLocation1.SelectedValue);
                                cmd.Parameters.AddWithValue("@CUSTOMERID", CustomerID.Value);
                                cmd.Parameters.AddWithValue("@PickupDate", Convert.ToDateTime(txtPickupDate.Text).Date);
                                cmd.Parameters.AddWithValue("@Weight", Convert.ToDecimal(txtWeight.Text));
                                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["u_ID"].ToString()));
                                cmd.Parameters.AddWithValue("@Status", 1);
                                if (ddlRouteCode.SelectedValue == "-1")
                                {
                                    cmd.Parameters.AddWithValue("@RouteCode", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@RouteCode", ddlRouteCode.SelectedValue);

                                }

                                cmd.Parameters.AddWithValue("@PickupType", rdPickupType.SelectedValue);
                                cmd.Parameters.AddWithValue("@FName", txtFirstName.Text);
                                cmd.Parameters.AddWithValue("@MiddleName", txtMiddleName.Text);
                                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                                cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);
                                cmd.Parameters.AddWithValue("@WhatsappNo", txtWhatsappNo.Text);
                                cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
                                cmd.Parameters.AddWithValue("@House", txtHouse.Text);
                                cmd.Parameters.AddWithValue("@FloorNo", txtFloorNo.Text);
                                cmd.Parameters.AddWithValue("@Building", txtBuilding.Text);
                                cmd.Parameters.AddWithValue("@PlotNo", txtPlotNo.Text);
                                cmd.Parameters.AddWithValue("@Street", txtStreet.Text);
                                cmd.Parameters.AddWithValue("@Sector", txtSector.Text);
                                cmd.Parameters.AddWithValue("@Area", txtArea.Text);
                                cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                                cmd.Parameters.AddWithValue("@City", Convert.ToInt32(1));
                                cmd.Parameters.AddWithValue("@Shipments", Convert.ToInt32(txtShipments.Text));
                                cmd.Parameters.AddWithValue("@WeightTypeID", ddlWeightTypeID.SelectedValue);
                                cmd.Parameters.AddWithValue("@Product", ddlProductID.SelectedValue);
                                cmd.Parameters.AddWithValue("@Service", ddlServiceID.SelectedValue);
                                cmd.Parameters.AddWithValue("@NeedPackingMaterialID", ddlNeedPackingMaterialID.SelectedValue);
                                cmd.Parameters.AddWithValue("@Contents", txtContents.Text);
                                cmd.Parameters.AddWithValue("@LoadersRequired", ddlLoaders.SelectedValue);
                                cmd.Parameters.AddWithValue("@HandlingToolID", ddlHandlingToolID.SelectedValue);
                                cmd.Parameters.AddWithValue("@AdditionalRemarks", txtAdditionalRemarks.Text);
                                cmd.Parameters.AddWithValue("@Scheduled", rdScheduled.SelectedValue);
                                cmd.Parameters.AddWithValue("@Timeslot", ddlTimeslot.SelectedItem.Text);
                                cmd.Parameters.AddWithValue("@Branch", Session["BranchCode"].ToString());
                                cmd.Parameters.AddWithValue("@RiderCode", txtridercode.Text);
                                cmd.Parameters.AddWithValue("@RouteDescription", txtRouteDescription.Text);
                                cmd.Parameters.AddWithValue("@AutoAssign", ddlAutoAssign.SelectedValue);

                                if (rdScheduled.SelectedValue == "No")
                                {
                                    cmd.Parameters.AddWithValue("@ScheduleType", 0);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ScheduleType", ddlScheduleType.SelectedValue);


                                }
                                long PICKUPID = 0;
                                cmd.Connection = con;
                                con.Open();
                                PICKUPID = Convert.ToInt32(cmd.ExecuteScalar());
                                con.Close();

                                if (PICKUPID > 0)
                                {
                                    if (rdScheduled.SelectedValue == "No")
                                    {

                                    }
                                    else
                                    {

                                        RequestAAD(PICKUPID.ToString());

                                    }
                                }
                            }
                        }
                        UpdateRiderAndRoute();
                        if (ddlAutoAssign.SelectedValue == "Yes")
                        {
                            Requestassgin();
                        }
                        Session["PR_MSG"] = "Pickup Request Generated";

                        Response.Redirect("Scheduler.aspx", true);
                        //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Pickup Request Generated')", true);
                    }

                }
                if (rdPickupType.SelectedValue == "Cash")
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("insert into pr_pickuprequest (pickupdate,weight,userid,status,routecode,pickuptype,accountnumber,accountname,location,FName,middlename,lastname,contactno,whatsappno,emailid,house,floorno,building,plotno,street,sector,area,postalcode,city,shipments,weighttypeid,product,service,needpackingmaterialid,contents,loadersrequired,handlingtoolid,additionalremarks,scheduled,timeslot,scheduletype,routedescription,autoassign) " +
                                                                  " values (@pickupdate,@weight,@userid,@status,@routecode,@pickuptype,@accountnumber,@accountname,@location,@FName,@middlename,@lastname,@contactno,@whatsappno,@emailid,@house,@floorno,@building,@plotno,@street,@sector,@area,@postalcode,@city,@shipments,@weighttypeid,@product,@service,@needpackingmaterialid,@contents,@loadersrequired,@handlingtoolid,@additionalremarks,@Scheduled,@timeslot,@scheduletype,@routedescription,@autoassign)"))
                        {
                            cmd.Parameters.AddWithValue("@AccountNumber", 0);
                            cmd.Parameters.AddWithValue("AccountName", "");
                            cmd.Parameters.AddWithValue("@Location", "");
                            cmd.Parameters.AddWithValue("@PickupDate", Convert.ToDateTime(txtPickupDate.Text).Date);
                            cmd.Parameters.AddWithValue("@Weight", Convert.ToDecimal(txtWeight.Text));
                            cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["u_ID"].ToString()));// Session["u_ID"].ToString());
                            cmd.Parameters.AddWithValue("@Status", 1);
                            if (ddlRouteCode.SelectedValue == "-1")
                            {
                                cmd.Parameters.AddWithValue("@RouteCode", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@RouteCode", ddlRouteCode.SelectedValue);

                            }

                            cmd.Parameters.AddWithValue("@PickupType", rdPickupType.Text);
                            cmd.Parameters.AddWithValue("@FName", txtFirstName.Text);
                            cmd.Parameters.AddWithValue("@MiddleName", txtMiddleName.Text);
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);
                            cmd.Parameters.AddWithValue("@WhatsappNo", txtWhatsappNo.Text);
                            cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
                            cmd.Parameters.AddWithValue("@House", txtHouse.Text);
                            cmd.Parameters.AddWithValue("@FloorNo", txtFloorNo.Text);
                            cmd.Parameters.AddWithValue("@Building", txtBuilding.Text);
                            cmd.Parameters.AddWithValue("@PlotNo", txtPlotNo.Text);
                            cmd.Parameters.AddWithValue("@Street", txtStreet.Text);
                            cmd.Parameters.AddWithValue("@Sector", txtSector.Text);
                            cmd.Parameters.AddWithValue("@Area", txtArea.Text);
                            cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                            cmd.Parameters.AddWithValue("@City", Convert.ToInt32(1));
                            cmd.Parameters.AddWithValue("@Shipments", Convert.ToInt32(txtShipments.Text));
                            cmd.Parameters.AddWithValue("@WeightTypeID", ddlWeightTypeID.SelectedValue);
                            cmd.Parameters.AddWithValue("@Product", ddlProductID.SelectedValue);
                            cmd.Parameters.AddWithValue("@Service", ddlServiceID.SelectedValue);
                            cmd.Parameters.AddWithValue("@NeedPackingMaterialID", ddlNeedPackingMaterialID.SelectedValue);
                            cmd.Parameters.AddWithValue("@Contents", txtContents.Text);
                            cmd.Parameters.AddWithValue("@LoadersRequired", ddlLoaders.SelectedValue);
                            cmd.Parameters.AddWithValue("@HandlingToolID", ddlHandlingToolID.SelectedValue);
                            cmd.Parameters.AddWithValue("@AdditionalRemarks", txtAdditionalRemarks.Text);
                            cmd.Parameters.AddWithValue("@Scheduled", "No");
                            cmd.Parameters.AddWithValue("@ScheduleType", "0");
                            cmd.Parameters.AddWithValue("@AutoAssign", "No");
                            cmd.Parameters.AddWithValue("@Timeslot", ddlTimeslot.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@RouteDescription", txtRouteDescription.Text);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    //    ResetValues();
                    Session["PR_MSG"] = "Pickup Request Generated";

                    Response.Redirect("Scheduler.aspx", true);
                    //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Pickup Request Generated')", true);
                }



            }
            catch (Exception ex)
            {

            }
        }

        void RequestAAD(String PickupRequestID)
        {

            string routecode = "NULL";
            int lrt = 0;
            string strRnd = getRandomNum();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd1 = new SqlCommand(@" select RouteCode from [PR_ClientRoute] where LocationID = " + ddlLocation1.SelectedValue + " and [WeightType] = " + ddlWeightTypeID.SelectedValue + " and [BranchCode] = " + Session["BranchCode"] + " and [CustomerID] = " + CustomerID.Value))
                {
                    cmd1.Connection = con;
                    con.Open();
                    routecode = Convert.ToString(cmd1.ExecuteScalar());
                    con.Close();
                }
                //if (routecode == null || routecode == "")
                //{
                //    using (SqlCommand cmd2 = new SqlCommand(@"INSERT INTO [PR_ClientRoute] RouteCode from [PR_ClientRoute] where LocationID = " + ddlLocation1.SelectedValue + " and [WeightType] = " + ddlWeightTypeID.SelectedValue + " and [BranchCode] = " + Session["BranchCode"] + " and [CustomerID] = " + CustomerID.Value))
                //    {
                //        cmd2.Connection = con;
                //        con.Open();
                //        routecode = Convert.ToString(cmd2.ExecuteScalar());
                //        con.Close();
                //    }
                //}
                using (SqlCommand cmd = new SqlCommand("insert into PR_Scheduler_PickupRequest (PickupRequestID,PickupType,Slot,Timeslot,PickupDate,BranchCode," +
                    "Product,Service,SchedulerTypeID,ScheduleType,weight,userid,status,routecode,location,LocationID,FName,middlename,lastname," +
                    "contactno,whatsappno,emailid,house,floorno,building,plotno,street,sector,area,postalcode,city,shipments,weighttypeid,needpackingmaterialid," +
                      "AccountNumber,AccountName,CustomerID," +
                    "contents,loadersrequired,handlingtoolid,additionalremarks,rec_rnd_key) " +
                                                          " values (@PickupRequestID,@Pickuptype,@Slot,@Timeslot,@PickupDate,@BranchCode,@Product," +
                                                          "@Service,@SchedulerTypeID,@ScheduleType,@weight,@userid,@status,@routecode,@location,@LocationID,@FName," +
                                                          "@middlename,@lastname,@contactno,@whatsappno,@emailid,@house,@floorno,@building," +
                                                          "@plotno,@street,@sector,@area,@postalcode,@city,@shipments,@weighttypeid,@needpackingmaterialid," +
                                                           "@AccountNumber,@AccountName,@CustomerID," +
                                                          "@contents,@loadersrequired,@handlingtoolid,@additionalremarks,@rec_rnd_key)" +
                                                          "SELECT SCOPE_IDENTITY()"))

                {

                    if (PickupRequestID == "")
                    {
                        cmd.Parameters.AddWithValue("@PickupRequestID", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PickupRequestID", PickupRequestID);
                    }

                    cmd.Parameters.AddWithValue("@BranchCode", Session["BranchCode"].ToString());
                    if (rdPickupType.SelectedValue == "Credit")
                    {
                        cmd.Parameters.AddWithValue("@location", ddlLocation1.SelectedValue);
                        cmd.Parameters.AddWithValue("@LocationID", ddlLocation1.SelectedValue);
                        cmd.Parameters.AddWithValue("@AccountNumber", txtAccountNumber.Text);
                        cmd.Parameters.AddWithValue("@AccountName", txtAccountName.Text);

                        cmd.Parameters.AddWithValue("@CustomerID", CustomerID.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@location", DBNull.Value);
                        cmd.Parameters.AddWithValue("@LocationID", DBNull.Value);

                        cmd.Parameters.AddWithValue("@AccountNumber", DBNull.Value);
                        cmd.Parameters.AddWithValue("@AccountName", DBNull.Value);

                        cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    }


                    // cmd.Parameters.AddWithValue("@location", Convert.ToInt32(ddlLocationID.SelectedItem.Text.ToString()));
                    cmd.Parameters.AddWithValue("@PickupDate", Convert.ToDateTime(txtPickupDate.Text).Date);
                    cmd.Parameters.AddWithValue("@Pickuptype", rdPickupType.Text);
                    cmd.Parameters.AddWithValue("@Slot", ddlTimeslot.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Timeslot", ddlTimeslot.SelectedItem.Text);
                    //cmd.Parameters.AddWithValue("@VehicleRequired", Convert.ToInt32(ddlVehicleReq.SelectedValue));
                    cmd.Parameters.AddWithValue("@Product", ddlProductID.SelectedValue);
                    cmd.Parameters.AddWithValue("@Service", ddlServiceID.SelectedValue);
                    cmd.Parameters.AddWithValue("@SchedulerTypeID", Convert.ToInt32(ddlScheduleType.SelectedValue));
                    cmd.Parameters.AddWithValue("@ScheduleType", Convert.ToInt32(ddlScheduleType.SelectedValue));
                    cmd.Parameters.AddWithValue("@Weight", Convert.ToDecimal(txtWeight.Text));
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["u_ID"].ToString())); ;
                    cmd.Parameters.AddWithValue("@Status", 1);
                    if (ddlRouteCode.SelectedValue == "-1")
                    {
                        cmd.Parameters.AddWithValue("@RouteCode", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@RouteCode", routecode);

                    }

                    cmd.Parameters.AddWithValue("@FName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@MiddleName", txtMiddleName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);
                    cmd.Parameters.AddWithValue("@WhatsappNo", txtWhatsappNo.Text);
                    cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
                    cmd.Parameters.AddWithValue("@House", txtHouse.Text);
                    cmd.Parameters.AddWithValue("@FloorNo", txtFloorNo.Text);
                    cmd.Parameters.AddWithValue("@Building", txtBuilding.Text);
                    cmd.Parameters.AddWithValue("@PlotNo", txtPlotNo.Text);
                    cmd.Parameters.AddWithValue("@Street", txtStreet.Text);
                    cmd.Parameters.AddWithValue("@Sector", txtSector.Text);
                    cmd.Parameters.AddWithValue("@Area", txtArea.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                    cmd.Parameters.AddWithValue("@City", Convert.ToInt32(1));
                    cmd.Parameters.AddWithValue("@Shipments", Convert.ToInt32(txtShipments.Text));
                    cmd.Parameters.AddWithValue("@WeightTypeID", ddlWeightTypeID.SelectedValue);
                    //cmd.Parameters.AddWithValue("@Product", ddlProductID.SelectedValue);
                    //cmd.Parameters.AddWithValue("@Service", ddlServiceID.SelectedValue);
                    cmd.Parameters.AddWithValue("@NeedPackingMaterialID", ddlNeedPackingMaterialID.SelectedValue);
                    cmd.Parameters.AddWithValue("@Contents", txtContents.Text);
                    cmd.Parameters.AddWithValue("@LoadersRequired", ddlLoaders.SelectedValue);
                    cmd.Parameters.AddWithValue("@HandlingToolID", ddlHandlingToolID.SelectedValue);
                    cmd.Parameters.AddWithValue("@AdditionalRemarks", txtAdditionalRemarks.Text);
                    cmd.Parameters.AddWithValue("@rec_rnd_key", strRnd);
                    cmd.Connection = con;
                    con.Open();
                    lrt = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }

                string SchduleType = "P";


                if (ddlScheduleType.SelectedValue == "2")
                {
                    SchduleType = "W";
                    for (int i = 0; i < chkWeekDay.Items.Count; i++)
                    {
                        if (chkWeekDay.Items[i].Selected)
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO PR_WeekMonth (SchedulerID,Value,SchduleType,rec_status) Values" +
                                  " (" + lrt + ",'" + chkWeekDay.Items[i].Text + "','" + SchduleType + "',1 )");


                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

                if (ddlScheduleType.SelectedValue == "3")
                {
                    SchduleType = "M";
                    for (int i = 0; i < chkMonthDat.Items.Count; i++)
                    {
                        if (chkMonthDat.Items[i].Selected)
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO PR_WeekMonth (SchedulerID,Value,SchduleType,rec_status) Values" +
                                  " (" + lrt + ",'" + chkMonthDat.Items[i].Text + "','" + SchduleType + "',1 )");


                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }


            // ResetValues();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Scheduler Request Generated')", true);

        }
        void UpdateRiderAndRoute()
        {
            int lrt = 0;
            string strRnd = getRandomNum();
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand(@"

select RouteCode from [PR_ClientRoute] where LocationID=" + ddlLocation1.SelectedValue + " and [WeightType]= " + ddlWeightTypeID.SelectedValue + " and   [BranchCode]=" + Session["BranchCode"] + " and [CustomerID]=" + CustomerID.Value + @"

update 
insert into PR_ClientRoute ([LocationID],[CustomerID] ,[BranchCode]  ,[RouteCode] ,[Rec_Status],[WeighType])

Values (@LocationID,@CustomerID,@BranchCode,@RouteCode,1,@WeightType)"

))
                {

                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID.Value);
                    cmd.Parameters.AddWithValue("@BranchCode", Session["BranchCode"].ToString());
                    cmd.Parameters.AddWithValue("@LocationID", ddlLocation1.SelectedValue);
                    cmd.Parameters.AddWithValue("@WeightType", ddlWeightTypeID.SelectedValue);
                    if (ddlRouteCode.SelectedValue == "-1")
                    {
                        cmd.Parameters.AddWithValue("@RouteCode", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@RouteCode", ddlRouteCode.SelectedValue);

                    }




                    cmd.Connection = con;
                    con.Open();
                    lrt = cmd.ExecuteNonQuery();
                    con.Close();
                }


            }




        }

        void Requestassgin()
        {
            int lrt = 0;
            string strRnd = getRandomNum();
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand(@"

delete from   [PR_ClientRoute] where LocationID=" + ddlLocation1.SelectedValue + " and [WeightType]= " + ddlWeightTypeID.SelectedValue + " and   [BranchCode]=" + Session["BranchCode"] + " and [CustomerID]=" + CustomerID.Value + @"

insert into PR_ClientRoute ([LocationID],[CustomerID] ,[BranchCode]  ,[RouteCode] ,[Rec_Status],[WeighType])

Values (@LocationID,@CustomerID,@BranchCode,@RouteCode,1,@WeightType)"

))
                {

                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID.Value);
                    cmd.Parameters.AddWithValue("@BranchCode", Session["BranchCode"].ToString());
                    cmd.Parameters.AddWithValue("@LocationID", ddlLocation1.SelectedValue);
                    cmd.Parameters.AddWithValue("@WeightType", ddlWeightTypeID.SelectedValue);
                    if (ddlRouteCode.SelectedValue == "-1")
                    {
                        cmd.Parameters.AddWithValue("@RouteCode", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@RouteCode", ddlRouteCode.SelectedValue);

                    }




                    cmd.Connection = con;
                    con.Open();
                    lrt = cmd.ExecuteNonQuery();
                    con.Close();
                }


            }




        }
        protected void Pickup_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdPickupType.SelectedValue == "Credit")
            {
                ddlAutoAssign.Enabled = true;
                ddlAutoAssign.SelectedValue = "No";
                rdScheduled.SelectedValue = "Yes";
                // rdScheduled.Enabled = true;
                tdCredit.Visible = true;
                labelCredit.Visible = true;
                //tdCredit1.Visible = true;
                rdschedulerPickup.SelectedValue = "Yes";
                msg.Enabled = false;
                msg.Text = "";
                // ddlRouteCode.Items.Clear();

            }
            else
            {
                ddlAutoAssign.SelectedValue = "No";
                ddlAutoAssign.Enabled = false;
                tdCredit.Visible = false;
                labelCredit.Visible = false;
                rdScheduled.SelectedValue = "Yes";
                // rdScheduled.Enabled = false;
                ddl_Schedule.Visible = false;
                Label_Schedule.Visible = false;
                rWeek.Visible = false;
                rMonth.Visible = false;
                ddlScheduleType.SelectedValue = "0";
                rdschedulerPickup.SelectedValue = "Yes";
                msg.Enabled = false;
                msg.Text = "";
                chkWeekDay.ClearSelection();
                chkMonthDat.ClearSelection();

            }
        }
        protected void Add_Scheduled_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdScheduled.SelectedValue == "No")
            {
                ddl_Schedule.Visible = false;
                Label_Schedule.Visible = false;
                rWeek.Visible = false;
                rMonth.Visible = false;
                ddlScheduleType.SelectedValue = "0";
                chkWeekDay.ClearSelection();
                chkMonthDat.ClearSelection();


            }
            else
            {
                ddl_Schedule.Visible = true;
                Label_Schedule.Visible = true;

            }
        }
        void ResetValues()
        {
            try
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtMiddleName.Text = "";
                txtPickupDate.Text = "";
                txtWhatsappNo.Text = "";
                txtWeight.Text = "";
                txtShipments.Text = "";
                txtAdditionalRemarks.Text = "";
                txtContents.Text = "";
                txtAccountNumber.Text = "";
                txtAccountName.Text = "";
                ddlLocation1.Items.Clear();
                txtContactNo.Text = "";
                txtEmailID.Text = "";
                txtHouse.Text = "";
                txtFloorNo.Text = "";
                txtBuilding.Text = "";
                txtPlotNo.Text = "";
                txtStreet.Text = "";
                txtSector.Text = "";
                txtArea.Text = "";
                txtContents.Text = "";
                CustomerID.Value = "";
                // ddlLoaders.Items.Clear() ;
                txtAdditionalRemarks.Text = "";
                txtRouteDescription.Text = "";
                //ddlCityID.SelectedValue = "-1";
                //ddlCityID.Text = "Select City";

                //ddlProductID.SelectedValue = "-1";
                //ddlProductID.Text = "Select Product";

                //ddlServiceID.SelectedValue = "-1";
                //ddlServiceID.Text = "Select Service";



                //ddlTimeslot.SelectedValue = "0";
                //ddlTimeslot.Text = "Select Slot";

                //ddlWeightTypeID.SelectedValue = "-1";
                //ddlWeightTypeID.Text = "Select Weight Type";


                //ddlHandlingToolID.SelectedValue = "-1";
                //ddlHandlingToolID.Text = "Select handling tools";

                //ddlNeedPackingMaterialID.SelectedValue = "-1";
                //ddlNeedPackingMaterialID.Text = "Select Packing Material";

                //ddlRouteCode.SelectedValue = "";
                //ddlRouteCode.Text = "Select Route Code";
                loadCombo();







            }
            catch (Exception)
            {

            }
        }
        protected void Scheduled_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            try

            {

                if (ddlScheduleType.SelectedValue == "1")
                {
                    rWeek.Visible = false;
                    rMonth.Visible = false;
                    chkWeekDay.ClearSelection();
                    chkMonthDat.ClearSelection();
                }
                else if (ddlScheduleType.SelectedValue == "2")
                {
                    rWeek.Visible = true;
                    rMonth.Visible = false;
                    chkWeekDay.ClearSelection();
                    chkMonthDat.ClearSelection();
                }
                else if (ddlScheduleType.SelectedValue == "3")
                {
                    rWeek.Visible = false;
                    rMonth.Visible = true;
                    chkWeekDay.ClearSelection();
                    chkMonthDat.ClearSelection();
                }
                else
                {
                    rWeek.Visible = false;
                    rMonth.Visible = false;
                    chkWeekDay.ClearSelection();
                    chkMonthDat.ClearSelection();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void laodLocationonly()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                //string com_loc = "select locationName from COD_CustomerLocations where brancahCode = @branchcode and accountNo = '@accountcode'";
                string com_loc = "select locationName,locationID from COD_CustomerLocations where status=1 and  brancahCode = " + Session["BranchCode"] + " and accountNo = '" + txtAccountNumber.Text + "'";
                SqlDataAdapter adpt_loc = new SqlDataAdapter(com_loc, con);
                DataTable dt_loc = new DataTable();
                adpt_loc.Fill(dt_loc);
                //  cmd1.ExecuteNonQuery();
                con.Close();
                if (dt_loc.Rows.Count > 0)
                {
                    ddlLocation1.DataSource = dt_loc;
                    ddlLocation1.DataBind();
                    ddlLocation1.DataTextField = "locationName";
                    ddlLocation1.DataValueField = "locationID";
                    ddlLocation1.DataBind();
                    dt_loc.Dispose();
                    dt_loc.Clear();




                }
            }
            catch (Exception ex)
            {

            }

        }
        protected void txtAccountNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string com1 = "select * from CreditClients WHERE status = 1 and accountNo != '' and accountNo=@accountNo and branchCode=@branchCode";
                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                adpt1.SelectCommand.Parameters.AddWithValue("@accountNo", txtAccountNumber.Text.Trim());
                adpt1.SelectCommand.Parameters.AddWithValue("@branchCode", Session["BranchCode"].ToString());
                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                //select locationName from COD_CustomerLocations where brancahCode = 4 and accountNo = '4T67'  ddlLocation1
                if (dt1.Rows.Count > 0)
                {

                    txtAccountName.Text = dt1.Rows[0]["name"].ToString();
                    txtFirstName.Text = dt1.Rows[0]["contactPerson"].ToString().Replace(" ", "");
                    txtContactNo.Text = dt1.Rows[0]["phoneNo"].ToString().Replace("-", ""); ;
                    txtEmailID.Text = dt1.Rows[0]["email"].ToString();
                    txtHouse.Text = dt1.Rows[0]["address"].ToString();
                    CustomerID.Value = dt1.Rows[0]["id"].ToString();
                    dt1.Dispose();
                    dt1.Clear();




                    con.Open();
                    //string com_loc = "select locationName from COD_CustomerLocations where brancahCode = @branchcode and accountNo = '@accountcode'";
                    string com_loc = "select locationName,locationID from COD_CustomerLocations where status=1 and  brancahCode = " + Session["BranchCode"] + " and accountNo = '" + txtAccountNumber.Text + "'";
                    SqlDataAdapter adpt_loc = new SqlDataAdapter(com_loc, con);
                    DataTable dt_loc = new DataTable();
                    adpt_loc.Fill(dt_loc);
                    //  cmd1.ExecuteNonQuery();
                    con.Close();
                    if (dt_loc.Rows.Count > 0)
                    {
                        ddlLocation1.DataSource = dt_loc;
                        ddlLocation1.DataBind();
                        ddlLocation1.DataTextField = "locationName";
                        ddlLocation1.DataValueField = "locationID";
                        ddlLocation1.DataBind();
                        dt_loc.Dispose();
                        dt_loc.Clear();
                        LoadRouteCodeAssgin();
                        //LoadRouteAddress();



                    }

                    else
                    {
                        ddlLocation1.Items.Clear();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('This Account No Location not available')", true);
                    }

                    //con.Open();

                    //string com_assgin = "SELECT [ClientRouteID] FROM PR_ClientRoute  where LocationID=" + ddlLocation1.SelectedValue + " and   [BranchCode]=" + Session["BranchCode"] + " and [CustomerID]=" + CustomerID.Value + "";
                    //SqlDataAdapter adpt_assgin = new SqlDataAdapter(com_assgin, con);
                    //DataTable dt_assgin = new DataTable();
                    //adpt_loc.Fill(dt_assgin);

                    //con.Close();
                    //if (dt_assgin.Rows.Count > 0)
                    //{




                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please  map route this Customer ')", true);
                    //}





                }
                else
                {
                    // ResetValues();
                    Session["PR_MSG"] = "Account does not Exist"; ;

                    Response.Redirect("Scheduler.aspx", true);
                    //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Account does not Exist')", true);

                }
            }
            catch (Exception ex)
            {

            }

        }
        protected void ddlRouteCode_SelectedIndexChanged(object sender, EventArgs e)

        {
            LoadRouteAddress();
            LoadRider();
        }
        protected void ddlProductID_SelectedIndexChanged(object sender, EventArgs e)

        {
            loadComboofserviceType();


        }

        void LoadRider()
        {
            try
            {
                txtridercode.Text = "";
                txtRiderName.Text = "";

                if (ddlRouteCode.SelectedValue == "-1")
                {
                    txtRouteDescription.Text = null;
                }
                else
                {
                    SqlConnection con = new SqlConnection(constr);
                    con.Open();
                    string com1 = "select     firstName as RiderName, RiderCode from  Riders where userTypeId in ('72','90','217') and riderCode in (   [dbo].[GeRiderCode]('" + ddlRouteCode.SelectedValue + "',GetDate()," + Session["BranchCode"] + " ) ) ";
                    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                    DataTable dt1 = new DataTable();
                    adpt1.Fill(dt1);
                    con.Close();
                    if (dt1.Rows.Count > 0)
                    {
                        txtRiderName.Text = dt1.Rows[0]["RiderName"].ToString();
                        txtridercode.Text = dt1.Rows[0]["ridercode"].ToString();
                    }
                    else
                    {
                        txtRiderName.Text = "";
                        txtridercode.Text = "";
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        void LoadRouteAddress()
        {
            try
            {
                if (ddlRouteCode.SelectedValue == "-1")
                {
                    txtRouteDescription.Text = null;
                }
                else
                {
                    SqlConnection con = new SqlConnection(constr);
                    con.Open();
                    string com1 = "select description as address from Routes WHERE routeCode='" + ddlRouteCode.SelectedValue + "'";
                    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                    DataTable dt1 = new DataTable();
                    adpt1.Fill(dt1);
                    con.Close();
                    if (dt1.Rows.Count > 0)
                    {
                        txtRouteDescription.Text = dt1.Rows[0]["address"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        public static string getRandomNum()
        {
            string strObj = DateTime.Now.Ticks.ToString() + String.Format("{0:X2}", (new Random()).Next(0x100));
            strObj = strObj.Substring(0, 20);
            return strObj;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetValues();
            Response.Redirect("Scheduler.aspx", true);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            ResetValues();
            Response.Redirect("SchedulerList.aspx", true);
        }
        protected void ddlLocation1_TextChanged(object sender, EventArgs e)
        {
            LoadRouteCodeAssgin();
            //     LoadRouteAddress();
        }

        void LoadRouteCodeAssgin()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                string com6 = @" select description as address,routeCode from Routes WHERE routeCode in ( SELECT [routecode] FROM PR_ClientRoute 
        where LocationID=" + ddlLocation1.SelectedValue + " and   [BranchCode]=" + Session["BranchCode"] + " and [CustomerID]=" + CustomerID.Value + " )  ";

                con.Open();

                SqlDataAdapter adpt6 = new SqlDataAdapter(com6, con);
                DataTable dt6 = new DataTable();
                adpt6.Fill(dt6);
                con.Close();
                if (dt6.Rows.Count > 0)
                {
                    txtRouteDescription.Text = dt6.Rows[0]["address"].ToString();
                    ddlRouteCode.SelectedValue = dt6.Rows[0]["routecode"].ToString();
                    //  ListItem item = new ListItem(dt6.Rows[0]["routecode"].ToString(), dt6.Rows[0]["routecode"].ToString());

                    //    ddlRouteCode.Items.Insert(0, item);

                }
                else
                {
                    ListItem item = new ListItem("Select Route Code", "-1");

                    ddlRouteCode.Items.Insert(0, item);

                    //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('This  Route code not available')", true);

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void rdschedulerPickup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdschedulerPickup.SelectedValue == "Yes")
            {
                msg.Enabled = true;
                msg.Text = "Only Scheduler Pickup Generated";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Only Scheduler Pickup Generated')", true);
                return;
            }
            else
            {
                msg.Enabled = false;
                msg.Text = "";
            }


        }


    }
}