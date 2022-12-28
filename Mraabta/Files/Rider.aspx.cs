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
    public partial class Rider : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString(); // connection string

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //txtStartDate.Text = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                // txtDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                //loadCombo();
                BindGrid("");
                //lblARider.Visible = false;
                //ddlARider.Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            long lrt = 0;

            DataTable dt1= getduplicate(txtRiderCode.Text.Trim());
            if (dt1.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Rider already  Exist')", true);
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Riders (RiderCode, FirstName,LastName,CNIC,Email,Address,Phoneno,BranchID) 
                                VALUES (@RiderCode, @FirstName,@LastName,@CNIC,@Email,@Address,@Phoneno,@BranchID )"))
                    {
                        cmd.Parameters.AddWithValue("@RiderCode", txtRiderCode.Text);
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@CNIC", txtCNIC.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@Phoneno", txtPhoneNo.Text);
                        cmd.Parameters.AddWithValue("@BranchID", Session["BranchCode"].ToString());
                        cmd.Connection = con;
                        con.Open();
                        lrt= cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    
                }
                BindGrid("");
                if (lrt > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Submit Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Can not Submit')", true);

                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable getduplicate(string ridercode )
        {
            string strquery = "";

            strquery= @"select distinct RiderCode AS ID from Riders where  
               branchid=" + Session["BranchCode"].ToString() + " and ridercode='" + ridercode + "' ";
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(strquery, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
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
        public DataTable getDataTableddlRider()
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select distinct RiderCode AS ID, FirstName as Name from Riders where branchid=" + Session["BranchCode"].ToString() + " and  RiderCode<>'' and Ridercode<>'-' and Status=1 order by 2", con);
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
            SqlCommand cmd = new SqlCommand("select distinct A.RiderCode AS ID, R.FirstName as Name from PR_Attendance A  inner join  Riders R on R.RiderCode=A.RiderCode where A.RiderCode<>'' and A.Ridercode<>'-' and R.branchid=" + Session["BranchCode"].ToString() + " and A.branchid=" + Session["BranchCode"].ToString() + " and R.Status=1 and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date) and A.RiderCode<>'" + CurRider + "'", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
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
                using (SqlCommand cmd = new SqlCommand(" Select * from Riders where Status=1 and branchid=" + Session["BranchCode"].ToString() + " " + strW))
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

        protected void grdPR_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdPR.EditIndex = e.NewEditIndex;
            BindGrid("");
        }
        protected void grdPR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = grdPR.DataKeys[e.RowIndex].Values["RiderCode"].ToString();
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("update Riders set Status=0 where RiderCode =@RiderCode and branchid=" + Session["BranchCode"].ToString() + "", con);
            cmd.Parameters.AddWithValue("@RiderCode", id);
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

                string gRiderCode = grdPR.DataKeys[e.RowIndex].Values["RiderCode"].ToString();
                //TextBox txtname = grdPR.Rows[e.RowIndex].FindControl("TextBox2") as TextBox;
                //TextBox txtemail = grdPR.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
                TextBox grdFirstName = grdPR.Rows[e.RowIndex].FindControl("txtEFirstName") as TextBox;
                TextBox grdLastName = grdPR.Rows[e.RowIndex].FindControl("txtELastName") as TextBox;
                TextBox grdCNIC = grdPR.Rows[e.RowIndex].FindControl("txtECNIC") as TextBox;
                TextBox grdEmail = grdPR.Rows[e.RowIndex].FindControl("txtEEmail") as TextBox;
                TextBox grdAddress = grdPR.Rows[e.RowIndex].FindControl("txtEAddress") as TextBox;
                TextBox grdPhoneNo = grdPR.Rows[e.RowIndex].FindControl("txtEPhoneNo") as TextBox;

                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("update Riders set FirstName=@FirstName, LastName=@LastName,CNIC=@CNIC,Email=@Email,Address=@Address,PhoneNo=@PhoneNo where RiderCode =@RiderCode", con);
                cmd.Parameters.AddWithValue("@FirstName", grdFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", grdLastName.Text);
                cmd.Parameters.AddWithValue("@CNIC", grdCNIC.Text);
                cmd.Parameters.AddWithValue("@Email", grdEmail.Text);
                cmd.Parameters.AddWithValue("@Address", grdAddress.Text);
                cmd.Parameters.AddWithValue("@PhoneNo", grdPhoneNo.Text);
                cmd.Parameters.AddWithValue("@RiderCode", gRiderCode);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                grdPR.EditIndex = -1;
                BindGrid("");
            }
            catch (Exception)
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

                    //DropDownList ddListAttendanceStatus = (DropDownList)e.Row.FindControl("ddlgAttendanceStatus");
                    //DataTable dt = getDataTable("AttendanceStatusID", "AttendanceStatus", "PR_AttendanceStatus");
                    //ddListAttendanceStatus.DataSource = dt;
                    //ddListAttendanceStatus.DataTextField = "Name";
                    //ddListAttendanceStatus.DataValueField = "ID";
                    //ddListAttendanceStatus.DataBind();
                    //dt.Dispose();
                    //dt.Clear();
                    //DataRowView dr = e.Row.DataItem as DataRowView;
                    //ddListAttendanceStatus.SelectedValue = dr["AttendanceStatusID"].ToString();


                    //DropDownList ddListAlternateRider = (DropDownList)e.Row.FindControl("ddlgAlternateRider");
                    //Label lblRidCode = (Label)e.Row.FindControl("lblERiderCode");
                    //    SqlConnection con = new SqlConnection(constr);
                    //    con.Open();
                    ////string com1 = "select A.RiderCode AS ID, R.FirstName as Name from PR_Attendance A  inner join  Riders R on R.RiderCode=A.RiderCode where A.RiderCode<>'' and A.Ridercode<>'-' and R.Status=1 and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)";
                    //string com1 = "select R.RiderCode AS ID, R.FirstName as Name from Riders R  where R.RiderCode<>'' and R.Ridercode<>'-' and R.Status=1 ";// and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)";
                    //    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                    //    adpt1.Fill(dt);
                    //    con.Close();
                    //dt = getDataTableRider(lblRidCode.Text);
                    //ddListAlternateRider.DataSource = dt;
                    //ddListAlternateRider.DataTextField = "Name";
                    //ddListAlternateRider.DataValueField = "ID";
                    //ddListAlternateRider.DataBind();
                    //DataRowView dr1 = e.Row.DataItem as DataRowView;
                    //ddListAlternateRider.SelectedValue = dr1["RiderCode"].ToString();



                }
            }
            //}
            //catch (Exception)
            //{

            //}

        }

        //protected void ddlgAttendanceStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DropDownList ddlActive = (DropDownList)sender;

        //        // get reference to the row
        //        GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);

        //        //Get the reference of this ddlActive
        //        DropDownList grdAttendanceStatus = (DropDownList)gvr.FindControl("ddlgAttendanceStatus");
        //        DropDownList grdAlternateRider = (DropDownList)gvr.FindControl("ddlgAlternateRider");
        //        if (grdAttendanceStatus.SelectedValue == "2")
        //        {
        //            grdAlternateRider.Visible = true;
        //        }
        //        else
        //            grdAlternateRider.Visible = false;

        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //protected void btnRunScheduler_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        SqlConnection con1 = new SqlConnection(constr);

        //        con1.Open();
        //        string com = "select * from PR_Attendance where  rec_status=1 and Ridercode<>'' and AttendanceDate=cast (getdate() as date) ";//AttendanceStatusID=1 and

        //        SqlDataAdapter adpt1 = new SqlDataAdapter(com, con1);
        //        DataTable dt1 = new DataTable();
        //        adpt1.Fill(dt1);
        //        con1.Close();

        //        for (int j = 0; j < dt1.Rows.Count; j++)
        //        {
        //            //SqlConnection con1 = new SqlConnection(constr);
        //            con1.Open();
        //            string strwhere = " and RiderCode='" + dt1.Rows[j]["Ridercode"].ToString() + "'";
        //            com = "select * from (select S.* from PR_Scheduler S where SchedulerTypeID=1 and  PickupDate=cast (getdate() as date) union all " +
        //           " select S.*from PR_Scheduler S inner join PR_WeekDay W on S.SchedulerID = W.SchedulerID where S.SchedulerTypeID = 2 and S.PickupDate = cast(getdate() as date)  and W.WeekDay = DATENAME(dw, GETDATE()) " +
        //           " union all select S.*from PR_Scheduler S inner join PR_MonthDay M on S.SchedulerID = M.SchedulerID where S.SchedulerTypeID = 3 and S.PickupDate = cast(getdate() as date)  and M.MonthDay = DATEPart(Day, GETDATE()))p where 1=1 " + strwhere;

        //            SqlDataAdapter adpt = new SqlDataAdapter(com, con1);
        //            DataTable dt = new DataTable();
        //            adpt.Fill(dt);
        //            con1.Close();


        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                using (SqlConnection con = new SqlConnection(constr))
        //                {
        //                    using (SqlCommand cmd = new SqlCommand("INSERT INTO PR_PickupRequest (CustomerID, PickupDate,Slot,Qauntity,Weight,UserID,UserType,Source,Status,RouteCode,RiderCode,VehicleRequired,ProductID,ServiceID) " +
        //                        " VALUES (@CustomerID, @PickupDate,@Slot,@Qauntity,@Weight,@UserID,@UserType,@Source,@Status,@RouteCode,@RiderCode,@VehicleRequired,@ProductID,@ServiceID)"))
        //                    {
        //                        cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dt.Rows[i]["CustomerID"]));
        //                        cmd.Parameters.AddWithValue("@PickupDate", Convert.ToDateTime(dt.Rows[i]["PickupDate"]).Date);
        //                        cmd.Parameters.AddWithValue("@Slot", dt.Rows[i]["Slot"]);
        //                        cmd.Parameters.AddWithValue("@Qauntity", 0);
        //                        cmd.Parameters.AddWithValue("@Weight", 0);
        //                        cmd.Parameters.AddWithValue("@UserID", 1);
        //                        cmd.Parameters.AddWithValue("@UserType", 1);
        //                        cmd.Parameters.AddWithValue("@Source", 1);
        //                        cmd.Parameters.AddWithValue("@Status", 1);
        //                        cmd.Parameters.AddWithValue("@RouteCode", dt.Rows[i]["RouteCode"]);
        //                        if (dt1.Rows[j]["AlternateRidercode"].ToString() != "" && dt1.Rows[j]["AttendanceStatusID"].ToString() != "2")
        //                            cmd.Parameters.AddWithValue("@RiderCode", dt.Rows[i]["AlternateRidercode"]);
        //                        else
        //                            cmd.Parameters.AddWithValue("@RiderCode", dt.Rows[i]["RiderCode"]);
        //                        cmd.Parameters.AddWithValue("@VehicleRequired", Convert.ToInt32(dt.Rows[i]["VehicleRequired"]));
        //                        cmd.Parameters.AddWithValue("@ProductID", dt.Rows[i]["ProductID"]);
        //                        cmd.Parameters.AddWithValue("@ServiceID", dt.Rows[i]["ServiceID"]);
        //                        cmd.Connection = con;
        //                        con.Open();
        //                        cmd.ExecuteNonQuery();
        //                        con.Close();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string strWhere = "";

            if (txtSearchRider.Text != "")
            {
                strWhere = " and FirstName like '%" + txtSearchRider.Text + "%'";
            }
            BindGrid(strWhere);
        }
    }
}