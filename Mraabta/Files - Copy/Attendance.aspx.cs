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
    public partial class Attendance : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString(); // connection string
      //  string constr = ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString(); // connection string

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //txtStartDate.Text = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                txtDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                loadCombo();
                BindGrid("");
            }
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("INSERT INTO PR_Attendance (AttendanceDate, AttendanceStatusID,RiderCode,AlternateRiderCode,rec_status) VALUES (@AttendanceDate, @AttendanceStatusID,@RiderCode,@AlternateRiderCode,@rec_status)"))
        //            {
        //                cmd.Parameters.AddWithValue("@AttendanceDate", Convert.ToDateTime(txtDate.Text));
        //                cmd.Parameters.AddWithValue("@AttendanceStatusID", Convert.ToInt32(ddlAttendanceStatusID.SelectedValue));
        //                cmd.Parameters.AddWithValue("@RiderCode", ddlRiderCode.SelectedValue);
        //                cmd.Parameters.AddWithValue("@AlternateRiderCode", ddlAlternativeRiderCode.SelectedValue);
        //                cmd.Parameters.AddWithValue("@rec_status", 1);
        //                cmd.Connection = con;
        //                con.Open();
        //                cmd.ExecuteNonQuery();
        //                con.Close();
        //            }
        //        }
        //        BindGrid("");
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        //protected void ddlRiderCode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable dt2 = new DataTable();
        //        dt2 = getDataTableddlRider("1");
        //        ddlAlternativeRiderCode.DataSource = dt2;
        //        ddlAlternativeRiderCode.DataBind();
        //        ddlAlternativeRiderCode.DataTextField = "Name";
        //        ddlAlternativeRiderCode.DataValueField = "ID";
        //        ddlAlternativeRiderCode.DataBind();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        private void loadCombo()
        {
            try
            {

                SqlConnection con = new SqlConnection(constr);

                DataTable dt = new DataTable();
                //dt = getDataTableddlRider("");

                //ddlRiderCode.DataSource = dt;
                //ddlRiderCode.DataBind();
                //ddlRiderCode.DataTextField = "Name";
                //ddlRiderCode.DataValueField = "ID";
                //ddlRiderCode.DataBind();

                //con.Open();
                //string com1 = "select AttendanceStatusID as ID, AttendanceStatus as Name from PR_AttendanceStatus";
                //SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                //DataTable dt1 = new DataTable();
                //adpt1.Fill(dt1);
                //con.Close();
                //ddlAttendanceStatusID.DataSource = dt1;
                //ddlAttendanceStatusID.DataBind();
                //ddlAttendanceStatusID.DataTextField = "Name";
                //ddlAttendanceStatusID.DataValueField = "ID";
                //ddlAttendanceStatusID.DataBind();


              



            }
            catch (Exception)
            {

            }
        }
        public DataTable getDataTable(string ID, string Name, string tbName)
        {
            //select A.RiderCode AS ID, R.FirstName as Name from PR_Attendance A  inner join  Riders R on R.RiderCode=A.RiderCode where A.RiderCode<>'' and A.Ridercode<>'-' and R.Status=1 and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select " + ID + " as ID, " + Name + " as Name from " + tbName + " ", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public DataTable getDataTableddlRider(String RiderType)
        {
            string strwhere = "";
            //if (RiderType != "")
            //{
            //    strwhere = " and RiderCode not in ('" + ddlRiderCode.SelectedValue + "')";


            //}

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(@"select distinct RiderCode AS ID, 
FirstName as Name from Riders where userTypeId in ('217','72','90') and RiderCode<>'' and Ridercode<>'-' and Status=1 " + strwhere + "  order by 2", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable getchkAttendance(string RiderCode)
        {

            SqlConnection con = new SqlConnection(constr);//PR_Attendance (AttendanceDate, AttendanceStatusID,RiderCode
            SqlCommand cmd = new SqlCommand("select * from PR_Attendance where rec_status=1 and AttendanceDate=cast(getdate() as date) and branchId=" + Session["BranchCode"].ToString() + " and   RiderCode='" + RiderCode + "'", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public DataTable getDataTableRider(string CurRider)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(@"select * from 
(select distinct A.RiderCode AS ID, R.riderCode+'-'+ isnull(R.firstName,'')+' '+isnull(R.MiddleName,'')+' '+isnull(R.lastName,'') as Name from PR_Attendance A  

inner join  Riders R on R.RiderCode=A.RiderCode where R.userTypeId in ('217','72','90') and A.RiderCode<>''  
and A.Ridercode<>'-' and R.Status=1 and A.AttendanceStatusID=1 and a.rec_status=1 
and A.AttendanceDate=cast (getdate() as date)
and r.branchId = '" + Session["BranchCode"].ToString() + "'  and a.branchId = '" + Session["BranchCode"].ToString() + "'  and A.RiderCode<>'" + CurRider + @"'
       )  M  
 order by   M.Name asc ", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public string getSelectedAlternateRider(string CurRider)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand($@"SELECT pa.AlternateRiderCode as ID FROM PR_Attendance pa
 WHERE pa.RiderCode='{CurRider}' AND pa.AttendanceDate=CAST(GETDATE() AS date) ", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            string result = dt.Rows[0][0].ToString();
            return result;
        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPR.PageIndex = e.NewPageIndex;
            BindGrid("");
        }
        protected void grdPR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CmdCancel")
                {
                    /*your code to download here */
                    int PRid = Convert.ToInt32(e.CommandArgument.ToString());
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("update PR_Attendance Set rec_status=@rec_status where AttendanceID=@AttendanceID"))
                        {
                            cmd.Parameters.AddWithValue("@rec_status", 0);
                            cmd.Parameters.AddWithValue("@AttendanceID", PRid);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        //SqlCommand cmd1 = new SqlCommand("INSERT INTO PR_PickupRequestLog (PickupRequestID,CustomerID,PickupDate,Slot,Qauntity,Weight,UserID,UserType,Source,Status,RouteCode,RiderCode,ReasonCode,ActionUserID,Timestamp,VehicleRequired,LocationID) " +
                        //" select PickupRequestID,CustomerID,PickupDate,Slot,Qauntity,Weight,UserID,UserType,Source," + 0 + ",RouteCode,RiderCode," + 2 + ",UserID,getdate(),VehicleRequired,LocationID from PR_PickupRequestLog where PickupRequestID=" + PRid);
                        //cmd1.Connection = con;
                        //con.Open();
                        //cmd1.ExecuteNonQuery();
                        //con.Close();

                    }
                }
                if (e.CommandName == "Edit")
                {

                }
                BindGrid("");
            }
            catch (Exception)
            {

            }
        }
        private void BindGrid(string strW)
        {
            //try
            //{

            string strwhere = strW;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select distinct A.*,isnull(R.firstName,'')+isnull(R.MiddleName,'') +isnull(R.lastName,'') as Rider,R.routeCode, AR.riderCode+'-'+ isnull(AR.firstName,'')+' '+isnull(AR.MiddleName,'')+' '+isnull(AR.lastName,'') as AlternateRider,AT.AttendanceStatus " +
                    " from PR_Attendance A inner join Riders R on A.RiderCode=R.riderCode inner join PR_AttendanceStatus AT on A.AttendanceStatusID=AT.AttendanceStatusID " +
                    @" left join Riders AR on A.AlternateRiderCode=AR.riderCode and AR.riderCode<>'' 
                   and R.userTypeId in ('217','72','90') and AR.userTypeId in ('217','72','90') and r.status=1 AND AR.[status]=1  and AR.branchId=" + Session["BranchCode"].ToString() + "" +
                   "INNER JOIN routes rr ON rr.routeCode=r.routeCode where a.rec_status=1 and rr.status=1 and rr.BID =" + Session["BranchCode"].ToString() + " and r.branchId =" + Session["BranchCode"].ToString() + " and A.branchId =" + Session["BranchCode"].ToString() + "  and  a.AttendanceDate = cast(getdate()as dATE)  " +  strW + "order by 1 desc")) 
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            grdPR.DataSource = dt;
                            grdPR.DataBind();
                        }
                    }
                }
            }
            //}
            //catch (Exception)
            //{
            //}
        }
       
        //protected void ddlAttendanceStatusID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        try
        //        {
        //            DataTable dt2 = new DataTable();
        //            dt2 = getDataTableddlRider("1");
        //            ddlAlternativeRiderCode.DataSource = dt2;
        //            ddlAlternativeRiderCode.DataBind();
        //            ddlAlternativeRiderCode.DataTextField = "Name";
        //            ddlAlternativeRiderCode.DataValueField = "ID";
        //            ddlAlternativeRiderCode.DataBind();
        //        }
        //        catch (Exception)
        //        {

        //        }

        //        if (ddlAttendanceStatusID.SelectedItem.Text == "Absent")
        //        {

        //            lblARider.Visible = true;
        //            ddlARider.Visible = true;
        //        }
        //        else
        //        {
        //            lblARider.Visible = false;
        //            ddlARider.Visible = false;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        protected void btnGetAllAttendance_Click(object sender, EventArgs e)
        {
            try
            {


                SqlConnection con3 = new SqlConnection(constr);
                con3.Open();
                string com3 = "SELECT TOP 1 AttendanceID  FROM  PR_Attendance WHERE AttendanceDate = cast(getdate() as date) and branchId = '" + Session["BranchCode"].ToString() + "' AND rec_status=1";
                SqlDataAdapter adpt3 = new SqlDataAdapter(com3, con3);
                DataTable dt3 = new DataTable();
                adpt3.Fill(dt3);
                con3.Close();
                if (dt3.Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Already attendance marked')", true);
                   // Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", GetMsg("Attendance already run today"));


                    return;
                }
   
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO PR_Attendance (RiderCode,AttendanceDate, AttendanceStatusID,BranchId,rec_status) 
                                
                                   
  select distinct r.RiderCode ,@AttendanceDate as AttendanceDate,@AttendanceStatusID as AttendanceStatusID,@BranchId as BranchId,1 as rec_status
  from Riders r
  inner join routes rr on rr.routeCode = r.routeCode AND rr.[status]=1 where r.userTypeId in ('217','72','90') and r.status=1 AND  r.branchId = @BranchId and rr.BID = @BranchId "))
                            {
                                cmd.Parameters.AddWithValue("@AttendanceDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@AttendanceStatusID", 1);
                              
                                cmd.Parameters.AddWithValue("@BranchId", Session["BranchCode"].ToString());

                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    

                

                BindGrid("");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('All Riders attendance has been marked')", true);
               // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('All Riders attendance has been marked')", true);
            }
            catch (Exception ex)
            {

            }
        }

        protected void grdPR_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdPR.EditIndex = e.NewEditIndex;
            BindGrid("");
        }
        protected void grdPR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt16(grdPR.DataKeys[e.RowIndex].Values["AttendanceID"].ToString());
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("update PR_Attendance set rec_status=2 where AttendanceID =@AttendanceID", con);
            cmd.Parameters.AddWithValue("@AttendanceID", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            BindGrid("");

        }
        protected void grdPR_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdPR.EditIndex = -1;
            BindGrid("");
        }
        protected void grdPR_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string alternaterider = "";
                int gAttendanceID = Convert.ToInt32(grdPR.DataKeys[e.RowIndex].Values["AttendanceID"].ToString());
                //TextBox txtname = grdPR.Rows[e.RowIndex].FindControl("TextBox2") as TextBox;
                //TextBox txtemail = grdPR.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
                DropDownList grdAttendanceStatus = grdPR.Rows[e.RowIndex].FindControl("ddlgAttendanceStatus") as DropDownList;
                if (grdAttendanceStatus.SelectedValue == "1")
                {
                    alternaterider = "";
                }
                else
                {
                    DropDownList grdAlternateRider = grdPR.Rows[e.RowIndex].FindControl("ddlgAlternateRider") as DropDownList;
                    alternaterider = grdAlternateRider.SelectedValue;
                }
                //DropDownList grdBranchCode = grdPR.Rows[e.RowIndex].FindControl("ddlBranchCode") as DropDownList;
                //DropDownList grdRiderCode = grdPR.Rows[e.RowIndex].FindControl("ddlRiderCode") as DropDownList;
                //DropDownList grdRouteCode = grdPR.Rows[e.RowIndex].FindControl("ddlRouteCode") as DropDownList;

                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("update PR_Attendance set AttendanceStatusID=@AttendanceStatusID, AlternateRiderCode=@AlternateRiderCode where AttendanceID =@AttendanceID", con);
                cmd.Parameters.AddWithValue("@AttendanceStatusID", grdAttendanceStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@AlternateRiderCode", alternaterider);
                //cmd.Parameters.AddWithValue("@BranchCode", grdBranchCode.SelectedValue);
                //cmd.Parameters.AddWithValue("@RiderCode", grdRiderCode.SelectedValue);
                //cmd.Parameters.AddWithValue("@RouteCode", grdRouteCode.SelectedValue);
                cmd.Parameters.AddWithValue("@AttendanceID", gAttendanceID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                grdPR.EditIndex = -1;
                BindGrid("");
               // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('All Riders attendance has been marked')", true);
              //  Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", " < script type =\"text/javascript\"> swal('Attendance ID = '" + gAttendanceID + " is updated') </script>");

                 ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Attendance ID = '"+ gAttendanceID + " is updated)", true);
            }
            catch (Exception ex)
            {


            }



        }
        protected void grdPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.RowState == DataControlRowState.Edit) //  
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                //if (e.Row.RowType == DataControlRowType.DataRow && grdPR.EditIndex == e.Row.RowIndex)
                {

                    DropDownList ddListAttendanceStatus = (DropDownList)e.Row.FindControl("ddlgAttendanceStatus");
                    DataTable dt = getDataTable("AttendanceStatusID", "AttendanceStatus", "PR_AttendanceStatus");
                    ddListAttendanceStatus.DataSource = dt;
                    ddListAttendanceStatus.DataTextField = "Name";
                    ddListAttendanceStatus.DataValueField = "ID";
                    ddListAttendanceStatus.DataBind();
                    dt.Dispose();
                    dt.Clear();
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    ddListAttendanceStatus.SelectedValue = dr["AttendanceStatusID"].ToString();


                    DropDownList ddListAlternateRider = (DropDownList)e.Row.FindControl("ddlgAlternateRider");
                    Label lblRidCode = (Label)e.Row.FindControl("lblERiderCode");
                    //    SqlConnection con = new SqlConnection(constr);
                    //    con.Open();
                    ////string com1 = "select A.RiderCode AS ID, R.FirstName as Name from PR_Attendance A  inner join  Riders R on R.RiderCode=A.RiderCode where A.RiderCode<>'' and A.Ridercode<>'-' and R.Status=1 and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)";
                    //string com1 = "select R.RiderCode AS ID, R.FirstName as Name from Riders R  where R.RiderCode<>'' and R.Ridercode<>'-' and R.Status=1 ";// and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)";
                    //    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                    //    adpt1.Fill(dt);
                    //    con.Close();
                    dt = getDataTableRider(lblRidCode.Text);
                    ddListAlternateRider.DataSource = dt;
                    ddListAlternateRider.DataTextField = "Name";
                    ddListAlternateRider.DataValueField = "ID";
                    ddListAlternateRider.DataBind();
                    string result = getSelectedAlternateRider(lblRidCode.Text);
                    ddListAlternateRider.SelectedValue = result; //dt.ToString();
                    if (ddListAttendanceStatus.SelectedValue == "1"){
                        ddListAlternateRider.Visible = false;
                    }
                }
            }
            //}
            //catch (Exception)
            //{

            //}

        }

        protected void ddlgAttendanceStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlActive = (DropDownList)sender;

                // get reference to the row
                GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);

                //Get the reference of this ddlActive
                DropDownList grdAttendanceStatus = (DropDownList)gvr.FindControl("ddlgAttendanceStatus");
                DropDownList grdAlternateRider = (DropDownList)gvr.FindControl("ddlgAlternateRider");
                if (grdAttendanceStatus.SelectedValue == "2")
                {
                    grdAlternateRider.Visible = true;
                    DataRowView dr = (DataRowView)sender;
                    DataRowView dr1 = (DataRowView)sender;
                    grdAlternateRider.SelectedValue = dr1["RiderCode"].ToString();
                }
                else
                    grdAlternateRider.Visible = false;

            }
            catch (Exception)
            {

            }
        }


//        protected void btnRunScheduler_Click(object sender, EventArgs e)
//        {

//            SqlConnection con3 = new SqlConnection(constr);
//            con3.Open();
//            string com1 = "select id from PR_SchedulerStatus where SchedulerDate = cast(getdate() as date) and rec_status = 1";
//            SqlDataAdapter adpt3 = new SqlDataAdapter(com1, con3);
//            DataTable dt3 = new DataTable();
//            adpt3.Fill(dt3);
//            con3.Close();
//            if (dt3.Rows.Count > 0)
//            {
//                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Scheduler already run today')", true);
//                return;
//            }

//            long lrt = 0;

//            try { 




//                string com = @"INSERT INTO PR_PickupRequest
//	( PickupDate,Weight,UserID,Status,RouteCode,PickupType,AccountNumber,Location,MiddleName,LastName,ContactNo,WhatsappNo,EmailID
//,House,FloorNo,Building,PlotNo,Street,Sector,Area,PostalCode,City,Shipments,WeightTypeID,Product,Service,NeedPackingMaterialID,Contents,LoadersRequired
//,HandlingToolID,AdditionalRemarks,Scheduled,Timeslot,ScheduleType,RouteDescription,AutoAssign,AccountName,FName,rec_time_stamp,rec_modified_on
//,rec_status,CustomerID,Slot,Qauntity,UserType,Source,VehicleRequired,ProductID,ServiceID,branch,RiderCode)
	

//select distinct
// p.PickupDate, p.Weight, p.UserID, p.Status, p.RouteCode, p.PickupType, p.AccountNumber, p.Location, p.MiddleName, p.LastName,
// p.ContactNo, p.WhatsappNo, p.EmailID
//, p.House, p.FloorNo, p.Building, p.PlotNo, p.Street, p.Sector, p.Area, p.PostalCode, p.City, p.Shipments, p.WeightTypeID,
//p.Product, p.Service, p.NeedPackingMaterialID, p.Contents, p.LoadersRequired
//, p.HandlingToolID, p.AdditionalRemarks, p.Scheduled, p.Timeslot, p.ScheduleType, p.RouteDescription, p.AutoAssign, p.AccountName, 
//p.FName, p.rec_time_stamp, p.rec_modified_on
//, p.rec_status, p.CustomerID, p.Slot, p.Qauntity, p.UserType, p.Source, p.VehicleRequired, p.ProductID, p.ServiceID, p.branchcode as branch
//, case when A.AttendanceStatusID=2 then A.AlternateRiderCode else A.RiderCode end as RiderCode from (
//                    select S.* from PR_Scheduler_PickupRequest S where SchedulerTypeID=1  and S.rec_Status=1 
//                    union all
//                     select S.* from PR_Scheduler_PickupRequest S inner join PR_WeekMonth W on S.SchedulerID = W.SchedulerID 
//					 where S.SchedulerTypeID = 2  and  w.SchduleType='W'
//                     and W.Value = DATENAME(dw, GETDATE())  and W.rec_Status=1  and S.rec_Status=1
//                     union all 
//                     select S.* from PR_Scheduler_PickupRequest S inner join PR_WeekMonth M on S.SchedulerID = M.SchedulerID 
//                     where S.SchedulerTypeID = 3 and  M.SchduleType='M'  and M.Value = DATEPart(Day, GETDATE())
//					  and M.rec_Status=1 and S.rec_Status=1
//                     )p
//                     left join Riders R on p.RouteCode=R.routeCode 
//                     left join PR_Attendance A on R.riderCode = A.RiderCode
//                      where 1=1 and R.routeCode<>'' and  p.RouteCode<>''";
//                using (SqlConnection con = new SqlConnection(constr))
//                {

//                    using (SqlCommand cmd = new SqlCommand(com))
//                    {




//                        cmd.Connection = con;
//                        con.Open();
//                        lrt = cmd.ExecuteNonQuery();
//                        con.Close();
//                    }



//                }
//                if (lrt > 0)
//                {
//                    using (SqlConnection con = new SqlConnection(constr))
//                    {

//                        using (SqlCommand cmd = new SqlCommand(@"  INSERT INTO PR_SchedulerStatus (SchedulerDate,rec_status) VALUES
//  (	GETDATE(),1)"))
//                        {




//                            cmd.Connection = con;
//                            con.Open();
//                           long lrt1 = cmd.ExecuteNonQuery();
//                            con.Close();
//                        }



//                    }
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Pickup Request Generated')", true);


//                }


//            }
//            catch (Exception ex)
//            {

//            }
//}
        public void BulkInsertToDataBase(DataTable dt)
        {
            //Cl_Variables clvar = new Cl_Variables();
            DataTable dtProductSold = dt;// (DataTable)ViewState["ProductsSold"];

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Bulk_PR_PickupRequest"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@tblScheduler", dt);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string strWhere = "";

            if (txtSearchRider.Text != "")
            {
                strWhere = " and  A.RiderCode  like '%" + txtSearchRider.Text.Trim() + "%'";
            }
            BindGrid(strWhere);
            if (grdPR.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('No data Found')", true);
                return;
            }
      

        }
        public string GetMsg(string msg)
    {
        string javas = "<script type=\"text/javascript\"> swal(' " + msg + "') </script>";
        return javas;
    }
    }

}