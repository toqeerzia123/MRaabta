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
    public partial class GeneratePickupRequestList : System.Web.UI.Page
    {
      //  string constr = ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString(); // connection string
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString(); // connection string
        string Product;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PR_MSG"] != null)
            {
                string msg = Session["PR_MSG"].ToString();
                Session["PR_MSG"] = null;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", "<script type=\"text/javascript\"> swal(' " + msg + "') </script>");
            }
            if (!IsPostBack)
            {
                //txtStartDate.Text = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                //  txtEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                //loadCombo();

                DateTime dt_frommin = new DateTime(2000, 12, 1);

                DateTime dt_frommax = new DateTime(2050, 12, 1);
                txtPickupDate.Attributes["min"] = dt_frommin.ToString("yyyy-MM-dd");
                txtPickupDate.Attributes["max"] = dt_frommax.ToString("yyyy-MM-dd");
                txtPickupDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                BindGrid();
                ddlSubStatus.Visible = false;
                loadCombo();
                dataload();
                //BindGridShowLog();
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "if (typeof DisableDropDowns == 'function') {  DisableDropDowns();}", true);
        }
        protected void linkRequesID_Click(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            string strurl = "GeneratePickupRequest.aspx?ID=" + Id + "&Mode=E";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OpenWindow", "window.open('" + strurl + "','_blank'); DisableDropDowns();", true);
            //   Response.Redirect("GeneratePickupRequest.aspx?ID=" + Id + "&Mode=E");

            //    GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //   LinkButton lblID = (LinkButton)clickedRow.FindControl("linkRequesID");

            //   Response.Redirect("Scheduler.aspx?ID=" + lblID.Text + "&Mode=E");

        }
        private void dataload()
        {
            string strWhere = "";
            if (txtPickupDate.Text != "")
            {
                strWhere = strWhere + " and PickupDate='" + txtPickupDate.Text + "'";
            }

            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string com1 = "SELECT  COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'Aviation Sale' AND  ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1) " + strWhere + "   ";
            SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
            DataTable dt1 = new DataTable();
            adpt1.Fill(dt1);
            con.Close();
            Linkbtn_Aviation.Text = dt1.Rows[0]["Unassigned"].ToString();


            con.Open();
            string com2 = "SELECT    COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'Domestic'  AND  ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1)  " + strWhere + "   ";
            SqlDataAdapter adpt2 = new SqlDataAdapter(com2, con);
            DataTable dt2 = new DataTable();
            adpt2.Fill(dt2);
            con.Close();
            Linkbtn_Domestic.Text = dt2.Rows[0]["Unassigned"].ToString();


            con.Open();
            string com3 = "SELECT    COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'Cash on Delivery'  AND  ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1)  " + strWhere + "   ";
            SqlDataAdapter adpt3 = new SqlDataAdapter(com3, con);
            DataTable dt3 = new DataTable();
            adpt3.Fill(dt3);
            con.Close();
            Linkbtn_COD.Text = dt3.Rows[0]["Unassigned"].ToString();


            con.Open();
            string com4 = "SELECT    COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'Import'  AND  ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1)  " + strWhere + "   ";
            SqlDataAdapter adpt4 = new SqlDataAdapter(com4, con);
            DataTable dt4 = new DataTable();
            adpt4.Fill(dt4);
            con.Close();
            Linkbtn_Import.Text = dt4.Rows[0]["Unassigned"].ToString();

            con.Open();
            string com5 = "SELECT    COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'International'  AND  ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1)  " + strWhere + "   ";
            SqlDataAdapter adpt5 = new SqlDataAdapter(com5, con);
            DataTable dt5 = new DataTable();
            adpt5.Fill(dt5);
            con.Close();
            Linkbtn_International.Text = dt5.Rows[0]["Unassigned"].ToString();

            con.Open();
            string com6 = "SELECT    COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'Jazz Card' AND   ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1)  " + strWhere + "   ";
            SqlDataAdapter adpt6 = new SqlDataAdapter(com6, con);
            DataTable dt6 = new DataTable();
            adpt6.Fill(dt6);
            con.Close();
            Linkbtn_Jazz_Card.Text = dt6.Rows[0]["Unassigned"].ToString();

            con.Open();
            string com7 = "SELECT    COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'JAzzcash'  AND  ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1)  " + strWhere + "   ";
            SqlDataAdapter adpt7 = new SqlDataAdapter(com7, con);
            DataTable dt7 = new DataTable();
            adpt7.Fill(dt7);
            con.Close();
            Linkbtn_Jazz_Cash.Text = dt7.Rows[0]["Unassigned"].ToString();

            con.Open();
            string com8 = "SELECT    COUNT(*) AS 'Unassigned' FROM PR_PickupRequest WHERE Product = 'Road n Rail'  AND  ( isnull(RouteCode,'')='' or RouteCode='-1' or  STATUS=1) " + strWhere + "   ";
            SqlDataAdapter adpt8 = new SqlDataAdapter(com8, con);
            DataTable dt8 = new DataTable();
            adpt8.Fill(dt8);
            con.Close();
            Linkbtn_Road_Rail.Text = dt8.Rows[0]["Unassigned"].ToString();

        }
        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPR.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void EditRouteCode(object sender, EventArgs e)
        {
            DropDownList ddlRouteCode = sender as DropDownList;
            string id = ddlRouteCode.ID;
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + ddlRouteCode.SelectedItem.Value + "');", true);
            GridViewRow row = (GridViewRow)ddlRouteCode.NamingContainer;
            DropDownList ddlRiderCode = (DropDownList)row.FindControl("ddlRiderCode");
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string com1 = " select distinct riderCode as ID, cast(riderCode as varchar(100) )+' - '+cast(firstName as varchar(100) )  as riderCode  from Riders where userTypeId in ('217','72','90') and status=1 and  riderCode not  in ('', '-','0') AND routeCode='" + ddlRouteCode.SelectedValue + "' and branchId = '" + Session["BranchCode"].ToString() + "'";//"select R.RiderCode AS ID, R.FirstName as Name from Riders R  where R.RiderCode<>'' and R.Ridercode<>'-' and R.Status=1 ";// and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)";
            SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
            DataTable dt = new DataTable();
            adpt1 = new SqlDataAdapter(com1, con);
            adpt1.Fill(dt);
            con.Close();
            ddlRiderCode.DataSource = dt;
            ddlRiderCode.DataTextField = "riderCode";
            ddlRiderCode.DataValueField = "ID";
            ddlRiderCode.DataBind();
            if (dt.Rows.Count > 0)
            {
                ddlRiderCode.SelectedValue = dt.Rows[0]["ID"].ToString();
            }
            ListItem item3 = new ListItem("Select Rider", "-1");
            ddlRiderCode.Items.Insert(0, item3);
            // ddlRiderCode.SelectedValue=
            dt.Dispose();
            dt.Clear();
        }
        protected void grdPR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
             
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString());

                if (e.CommandName == "CmdUpdate")
                {
                    long lrtLog = 0;
                    string PRID = (e.CommandArgument.ToString());
                    long RequestID = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    lrtLog = CreateLog(PRID); lrtLog = CreateLog(PRID);
                    DropDownList ddlRouteCode = (DropDownList)grdPR.Rows[row.RowIndex].FindControl("ddlRouteCode");
                    DropDownList ddlRiderCode = (DropDownList)grdPR.Rows[row.RowIndex].FindControl("ddlRiderCode");
                    TextBox txtRemarks = (TextBox)grdPR.Rows[row.RowIndex].FindControl("txtRemarks");
                    DropDownList ddlStatus = (DropDownList)grdPR.Rows[row.RowIndex].FindControl("ddlStatus");
                    DropDownList ddlSubStatus = (DropDownList)grdPR.Rows[row.RowIndex].FindControl("ddlSubStatus");
                    if (ddlStatus.Text == "1" && ddlRiderCode.Text != "" && ddlRouteCode.Text != "")
                    {
                        ddlSubStatus.Text = ddlSubStatus.SelectedValue;
                        ddlStatus.Text = "2";
                    }

                    string qry = @"update PR_PickupRequest set RouteCode = @RouteCode, Ridercode = @Ridercode, Status = @Status, 
                                   SubStatus= @SubStatus, AdditionalRemarks = @AdditionalRemarks where PickupRequestID = @PickupRequestID";
                    SqlCommand cmd = new SqlCommand(qry, con);
                    cmd.Parameters.AddWithValue("@RouteCode", ddlRouteCode.Text);
                    cmd.Parameters.AddWithValue("@Ridercode", ddlRiderCode.Text);
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.Text);
                    cmd.Parameters.AddWithValue("@SubStatus", ddlSubStatus.Text);
                    cmd.Parameters.AddWithValue("@AdditionalRemarks", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@PickupRequestID", PRID);
                    con.Open();
                    //cmd.ExecuteNonQuery();
                    
                    long lrt = 0;
                    lrt = cmd.ExecuteNonQuery();
                    if (lrt > 0)
                    {
                        SendSMS(RequestID);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal('Item Updated');", true);
                        dataload();
                        BindGrid();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal('Item Can not Update')", true);
                    }
                    con.Close();
                }

                // BindGrid();
            }
            catch (Exception ex)
            {

            }
        }

        protected void changesubstatus_onindexchanged(object sender, EventArgs e)
        {
            DropDownList ddlMake = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlMake.NamingContainer;
            if (row != null)
            {
                
                    DropDownList ddListStatus = (DropDownList)row.FindControl("ddlStatus");
                    DropDownList ddLSubStatus = (DropDownList)row.FindControl("ddlSubStatus");
                    try
                    {
                        SqlConnection con = new SqlConnection(constr);
                        con.Open();

                        string com4 = $@"select status, Description from Pr_PickupRequestSubStatus where parentstatus = '{ddListStatus.SelectedValue}' ";
                        
                        SqlDataAdapter adpt4 = new SqlDataAdapter(com4, con);
                        DataTable dt4 = new DataTable();
                        adpt4.Fill(dt4);
                        con.Close();
                        ddLSubStatus.DataSource = dt4;
                        ddLSubStatus.DataTextField = "Description";
                        ddLSubStatus.DataValueField = "Status";
                        ddLSubStatus.DataBind();
                        ListItem item4 = new ListItem("Select SubStatus", "-1");
                        ddLSubStatus.Items.Insert(0, item4);
                        dt4.Dispose();
                        dt4.Clear();
                        //ddlSubStatus.Text= dr4["SubStatus"].ToString();
                        //ddlSubStatus.SelectedValue = dr4["SubStatus"].ToString();

                    }
                    catch (Exception)
                    {
                    }
            }
        }
        protected void grdPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                    SqlConnection con = new SqlConnection(constr);

                    DropDownList ddlRouteCode = (DropDownList)e.Row.FindControl("ddlRouteCode");
                    Label lblWeightType = (Label)e.Row.FindControl("lblWeightTypeID");
                    //Label lblRidCode = (Label)e.Row.FindControl("lblRiderCode");
                    con.Open();

                    string com1 = @"select distinct r.routeCode AS ID,r.routeCode+'-'+r.name  as routeCode from Routes r inner join Riders rr on rr.RouteCode= r.routecode and rr.status=1 where r.status=1 and rr.status=1 and rr.branchid=" + Session["BranchCode"].ToString() + " and r.bid=" + Session["BranchCode"].ToString() + "";
                    if (lblWeightType.Text.ToString() == "1")
                    {
                        com1 += " and rr.userTypeId in ('72','217') ";
                    }
                    if (lblWeightType.Text.ToString() == "2")
                    {
                        com1 += " and rr.userTypeId in ('90') ";
                    }
                    com1 += " order by r.RouteCode";
                    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                    DataTable dt = new DataTable();
                    adpt1 = new SqlDataAdapter(com1, con);
                    adpt1.Fill(dt);
                    con.Close();
                    ddlRouteCode.DataSource = dt;
                    ddlRouteCode.DataTextField = "routeCode";
                    ddlRouteCode.DataValueField = "ID";
                    ddlRouteCode.DataBind();
                    ListItem item4 = new ListItem("Select Route Code", "-1");
                    ddlRouteCode.Items.Insert(0, item4);
                    dt.Dispose();
                    dt.Clear();
                    DataRowView dr1 = e.Row.DataItem as DataRowView;
                    if (dr1["routeCode"].ToString() != "")
                    {
                        //ddlRouteCode.Text = dr1["routeCode"].ToString();
                        ddlRouteCode.SelectedValue = dr1["routeCode"].ToString();
                    }

                    DropDownList ddlRiderCode = (DropDownList)e.Row.FindControl("ddlRiderCode");
                    con.Open();
                    com1 = "  select distinct riderCode as ID, cast(riderCode as varchar(100) )+' - '+cast(firstName as varchar(100) )  as riderCode  from Riders where userTypeId in ('217','72','90') and status=1 and  riderCode not  in ('', '-','0') AND routeCode='" + ddlRouteCode.SelectedValue + "' and branchId = '" + Session["BranchCode"].ToString() + "'";//"select R.RiderCode AS ID, R.FirstName as Name from Riders R  where R.RiderCode<>'' and R.Ridercode<>'-' and R.Status=1 ";// and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)";
                    adpt1 = new SqlDataAdapter(com1, con);
                    adpt1.Fill(dt);
                    con.Close();
                    ddlRiderCode.DataSource = dt;
                    ddlRiderCode.DataTextField = "riderCode";
                    ddlRiderCode.DataValueField = "ID";
                    ddlRiderCode.DataBind();
                    ListItem item3 = new ListItem("Select Rider", "-1");
                    ddlRiderCode.Items.Insert(0, item3);
                    dt.Dispose();
                    dt.Clear();
                    DataRowView dr3 = e.Row.DataItem as DataRowView;
                    if (dr3["riderCode"].ToString() != "")
                    {
                        //ListItem item31 = new ListItem("Select Rider", "-1");
                        //  ddlRiderCode.Items.Insert(0, item31);
                        // ddlRiderCode.Text = dr3["riderCode"].ToString();
                        ddlRiderCode.SelectedValue = dr3["riderCode"].ToString();
                    }

                    //else if (dr1["routeCode"].ToString() != "-1")
                    //{
                    //    ddlRouteCode.Text = dr1["routeCode"].ToString();
                    //    ddlRouteCode.SelectedValue = dr1["routeCode"].ToString();
                    //}


                    //if (dr1["routeCode"].ToString() == "" || dr1["routeCode"].ToString() == "-1")
                    //{
                    //    ddlRouteCode.Enabled = true;
                    //}
                    //else
                    //{
                    //    ddlRouteCode.Enabled = false;
                    //}
                    //if (dr3["riderCode"].ToString() == "" || dr3["riderCode"].ToString() == "-1")
                    //{
                    //    ddlRiderCode.Enabled = true;
                    //}
                    //else
                    //{
                    //    ddlRiderCode.Enabled = false;
                    //}


                    // ddlRouteCode.SelectedValue = dr1["routeCode"].ToString();

                    DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                    //Label lblRidCode = (Label)e.Row.FindControl("lblRiderCode");
                    con.Open();
                    com1 = "select status,Description from Pr_PickupRequestStatus";//"select R.RiderCode AS ID, R.FirstName as Name from Riders R  where R.RiderCode<>'' and R.Ridercode<>'-' and R.Status=1 ";// and A.AttendanceStatusID=1 and A.AttendanceDate=cast (getdate() as date)";
                    adpt1 = new SqlDataAdapter(com1, con);
                    adpt1.Fill(dt);
                    con.Close();
                    ddlStatus.DataSource = dt;
                    ddlStatus.DataTextField = "Description";
                    ddlStatus.DataValueField = "status";
                    ddlStatus.DataBind();
                    dt.Dispose();
                    dt.Clear();
                    DataRowView dr2 = e.Row.DataItem as DataRowView;
                    ddlStatus.SelectedValue = dr2["status"].ToString();

                    DropDownList ddlSubStatus = (DropDownList)e.Row.FindControl("ddlSubStatus");
                    com1 = $@"select status, Description from Pr_PickupRequestSubStatus where parentstatus= '{ddlStatus.SelectedValue}' ";
                    adpt1 = new SqlDataAdapter(com1, con);
                    adpt1.Fill(dt);
                    con.Close();
                    ddlSubStatus.DataSource = dt;
                    ddlSubStatus.DataTextField = "Description";
                    ddlSubStatus.DataValueField = "status";
                    ddlSubStatus.DataBind();
                    dt.Dispose();
                    dt.Clear();
                    DataRowView dr4 = e.Row.DataItem as DataRowView;
                    ListItem item31 = new ListItem("Select SubStatus", "-1");
                    ddlSubStatus.Items.Insert(0, item31);
                    ddlSubStatus.Text = dr4["SubStatus"].ToString();
                    ddlSubStatus.SelectedValue = dr4["SubStatus"].ToString();

                    TextBox lblremarks = (TextBox)e.Row.FindControl("txtRemarks");
                    Button updatebutton = (Button)e.Row.FindControl("btnUpdate");

                    if (ddlStatus.SelectedValue == "5" || ddlStatus.SelectedValue == "6")
                    {
                        ddlStatus.Enabled = false;
                        ddlSubStatus.Enabled = false;
                        ddlRiderCode.Enabled = false;
                        ddlRouteCode.Enabled = false;
                        lblremarks.Enabled = false;
                        updatebutton.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void BindGrid()
        {
            try
            {
                string strWhere = "";
                //if (txtconsignmentNumber.Text != "")
                //{
                //    strWhere = strWhere + " and consignmentNumber='" + txtconsignmentNumber.Text + "'";
                //}
                if (txtPickupDate.Text != "")
                {
                    strWhere = strWhere + " and PickupDate='" + txtPickupDate.Text + "'";
                }
                if (txtAccountNO.Text != "")
                {
                    strWhere = strWhere + " and AccountNumber='" + txtAccountNO.Text + "'";
                }
                if (Product != null)
                {
                    ddlProduct.SelectedValue = Product;
                    //  txtPickupDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    strWhere = strWhere + " and Product='" + Product + "' ";
                }
                if (txtRouteCode.Text != "")
                {
                    strWhere = strWhere + " and RouteCode='" + txtRouteCode.Text + "'";
                }
                if (txtPickupID.Text != "")
                {
                    strWhere = strWhere + " and PickupRequestID='" + txtPickupID.Text + "'";
                }
                if (ddlStatus.SelectedValue != "-1" && ddlStatus.SelectedValue != "")
                {
                    if (ddlStatus.SelectedValue == "-2")
                    {
                        strWhere = strWhere + " and (isnull(RouteCode, '') = '' or RouteCode='-1' or status=1) ";

                    }
                    else
                    {
                        strWhere = strWhere + " and Status=" + ddlStatus.SelectedValue;
                    }
                }

                if (ddlSubStatus.SelectedValue != "-1" && ddlSubStatus.SelectedValue != "")
                {

                    strWhere = strWhere + " and SubStatus=" + ddlSubStatus.SelectedValue;
                }

                if (ddlProduct.SelectedValue != "")
                {

                    strWhere = strWhere + " and Product='" + ddlProduct.SelectedValue + "'  ";
                }

                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("select *, (House + ', Building- ' + Building + ', Street- '+ Street+ ', Sector- ' + Sector + ', Area- '+ Area+ ', Plot No- ' + plotno + ', Floor- ' + floorNo ) AS Address from PR_PickupRequest where PickupDate is not null " + strWhere + " order by 1 desc"))
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
            }
            catch (Exception)
            {
                //throw;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            loadCombo();
            dataload();
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }

        public void checkStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedItem.Text == "Not Picked")
            {
                ddlSubStatus.Visible = true;
                loadSubStatusCombo(sender, e);
            }
            else
            {
                ddlSubStatus.Visible = false;
            }
        }

        private void loadSubStatusCombo(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string com1 = @"select status, Description from Pr_PickupRequestSubStatus where parentstatus= '" + ddlStatus.SelectedItem.Value + "'";
                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                ddlSubStatus.DataSource = dt1;
                ddlSubStatus.DataBind();
                ddlSubStatus.DataTextField = "Description";
                ddlSubStatus.DataValueField = "status";
                ddlSubStatus.DataBind();
                dt1.Dispose();
                dt1.Clear();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
            }
            catch (Exception ex)
            {
            }
        }

        private void loadCombo()
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string com1 = @"select -1 status,'All' as Description 
                                union all 
                               select -2 status,'Unassigned' as Description 
                                union all
                                select status, Description from Pr_PickupRequestStatus";
                SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                DataTable dt1 = new DataTable();
                adpt1.Fill(dt1);
                con.Close();
                ddlStatus.DataSource = dt1;
                ddlStatus.DataBind();
                ddlStatus.DataTextField = "Description";
                ddlStatus.DataValueField = "status";
                ddlStatus.DataBind();
                dt1.Dispose();
                dt1.Clear();

                con.Open();
                string com2 = @"select '' id,'All' as Products union all 
                    select distinct Products as ID , Products as Name from ServiceTypes_New where Products  in (
'Cash on Delivery',
'Domestic',
'International',
'Road n Rail'
)
union all
select  'Cash on Delivery' as ID , 'Cash on Delivery' as Name";
                SqlDataAdapter adpt2 = new SqlDataAdapter(com2, con);
                DataTable dt2 = new DataTable();
                adpt2.Fill(dt2);
                con.Close();
                ddlProduct.DataSource = dt2;
                ddlProduct.DataBind();
                ddlProduct.DataTextField = "Products";
                ddlProduct.DataValueField = "ID";
                ddlProduct.DataBind();
                dt2.Dispose();
                dt2.Clear();

            }
            catch (Exception)
            {

            }
        }

        protected long CreateLog(string PRID)
        {


            long lrt = 0;


            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString());
            con.Open();
            try
            {


                string qry = @"insert into PR_PickupRequest_Log ( 
	[PickupRequestID],
	[PickupDate],
	[Weight],
	[UserID],
	[Status],
    [SubStatus],
	[RouteCode],
	[PickupType],
	[AccountNumber],
	[Location],
	[MiddleName],
	[LastName],
	[ContactNo],
	[WhatsappNo],
	[EmailID],
	[House],
	[FloorNo],
	[Building],
	[PlotNo],
	[Street],
	[Sector],
	[Area],
	[PostalCode],
	[City],
	[Shipments],
	[WeightTypeID],
	[Product],
	[Service],
	[NeedPackingMaterialID],
	[Contents],
	[LoadersRequired],
	[HandlingToolID],
	[AdditionalRemarks],
	[Scheduled],
	[Timeslot],
	[ScheduleType],
	[RouteDescription],
	[AutoAssign],
	[AccountName],
	[FName],
	[RiderCode])


SELECT 
	[PickupRequestID],
	[PickupDate],
	[Weight],
	[UserID],
	[Status],
    [SubStatus],
	[RouteCode],
	[PickupType],
	[AccountNumber],
	[Location],
	[MiddleName],
	[LastName],
	[ContactNo],
	[WhatsappNo],
	[EmailID],
	[House],
	[FloorNo],
	[Building],
	[PlotNo],
	[Street],
	[Sector],
	[Area],
	[PostalCode],
	[City],
	[Shipments],
	[WeightTypeID],
	[Product],
	[Service],
	[NeedPackingMaterialID],
	[Contents],
	[LoadersRequired],
	[HandlingToolID],
	[AdditionalRemarks],
	[Scheduled],
	[Timeslot],
	[ScheduleType],
	[RouteDescription],
	[AutoAssign],
	[AccountName],
	[FName],
	[RiderCode]
  FROM PR_PickupRequest 
                    where PickupRequestID = @PickupRequestID";

                SqlCommand cmd = new SqlCommand(qry, con);
                //cmd.Parameters.AddWithValue("@LogAddBy", Session["u_ID"].ToString());

                cmd.Parameters.AddWithValue("@PickupRequestID", PRID);
                lrt = cmd.ExecuteNonQuery();

                //  gv_clientList.EditIndex = -1;
                //BindGrid();
            }
            catch (Exception ex)
            {


            }
            finally
            {
                con.Close();
            }
            return lrt;
        }

        protected void Linkbtn_Aviation_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";


            Product = "Aviation Sale";
            BindGrid();
        }

        protected void Linkbtn_CashOnDelivery_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";

            Product = "Cash on Delivery";
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }

        protected void Linkbtn_Import_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";

            Product = "Import";
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }

        protected void Linkbtn_International_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";

            Product = "International";
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }

        protected void Linkbtn_Jazz_Card_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";

            Product = "Jazz Card";
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }

        protected void Linkbtn_Jazz_Cash_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";

            Product = "JAzzcash";
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }

        protected void Linkbtn_Road_Rail_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";

            Product = "Road n Rail";
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }

        protected void Linkbtn_Domestic_Click(object sender, EventArgs e)
        {
            ddlStatus.SelectedValue = "-2";

            Product = "Domestic";
            BindGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", " DisableDropDowns();", true);
        }
        protected void btnRunScheduler_Click(object sender, EventArgs e)
        {

            SqlConnection con3 = new SqlConnection(constr);
            con3.Open();
            string qry = "select count(*) from PR_Attendance where AttendanceDate = cast(getdate() as date)";
            SqlDataAdapter adptq = new SqlDataAdapter(qry, con3);
            DataTable dtq = new DataTable();
            adptq.Fill(dtq);
            con3.Close();

            if (dtq.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtq.Rows[0][0]) > 0)
                {
                    con3.Open();
                    string com1 = "select id from PR_SchedulerStatus where SchedulerDate = cast(getdate() as date) and rec_status = 1";
                    SqlDataAdapter adpt3 = new SqlDataAdapter(com1, con3);
                    DataTable dt3 = new DataTable();
                    adpt3.Fill(dt3);
                    con3.Close();
                    if (dt3.Rows.Count > 0)
                    {
                        Session["PR_MSG"] = "Scheduler has already run today";
                        Response.Redirect("GeneratePickupRequestList.aspx", true);
                       // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Scheduler already run today')", true);
                       // return;
                    }

                    long lrt = 0;

                    try
                    {




                        string com = @"INSERT INTO PR_PickupRequest
	( PickupDate,Weight,UserID,Status, SubStatus,RouteCode,PickupType,AccountNumber,Location,MiddleName,LastName,ContactNo,WhatsappNo,EmailID
,House,FloorNo,Building,PlotNo,Street,Sector,Area,PostalCode,City,Shipments,WeightTypeID,Product,Service,NeedPackingMaterialID,Contents,LoadersRequired
,HandlingToolID,AdditionalRemarks,Scheduled,Timeslot,ScheduleType,RouteDescription,AutoAssign,AccountName,FName,rec_time_stamp,rec_modified_on
,rec_status,CustomerID,Slot,Qauntity,UserType,Source,VehicleRequired,ProductID,ServiceID,branch,RiderCode)
	

select distinct
 p.PickupDate, p.Weight, p.UserID, p.Status, p.SubStatus, p.RouteCode, p.PickupType, p.AccountNumber, p.Location, p.MiddleName, p.LastName,
 p.ContactNo, p.WhatsappNo, p.EmailID
, p.House, p.FloorNo, p.Building, p.PlotNo, p.Street, p.Sector, p.Area, p.PostalCode, p.City, p.Shipments, p.WeightTypeID,
p.Product, p.Service, p.NeedPackingMaterialID, p.Contents, p.LoadersRequired
, p.HandlingToolID, p.AdditionalRemarks, p.Scheduled, p.Timeslot, p.ScheduleType, p.RouteDescription, p.AutoAssign, p.AccountName, 
p.FName, p.rec_time_stamp, p.rec_modified_on
, p.rec_status, p.CustomerID, p.Slot, p.Qauntity, p.UserType, p.Source, p.VehicleRequired, p.ProductID, p.ServiceID, p.branchcode as branch
, case when A.AttendanceStatusID=2 then A.AlternateRiderCode else A.RiderCode end as RiderCode from (
                    select S.* from PR_Scheduler_PickupRequest S where SchedulerTypeID=1  and S.rec_Status=1 
                    union all
                     select S.* from PR_Scheduler_PickupRequest S inner join PR_WeekMonth W on S.SchedulerID = W.SchedulerID 
					 where S.SchedulerTypeID = 2  and  w.SchduleType='W'
                     and W.Value = DATENAME(dw, GETDATE())  and W.rec_Status=1  and S.rec_Status=1
                     union all 
                     select S.* from PR_Scheduler_PickupRequest S inner join PR_WeekMonth M on S.SchedulerID = M.SchedulerID 
                     where S.SchedulerTypeID = 3 and  M.SchduleType='M'  and M.Value = DATEPart(Day, GETDATE())
					  and M.rec_Status=1 and S.rec_Status=1
                     )p
                     left join Riders R on p.RouteCode=R.routeCode 
                     left join PR_Attendance A on R.riderCode = A.RiderCode
                      where 1=1 and R.userTypeId in ('217','72','90') and R.routeCode<>'' and  p.RouteCode<>''";
                        using (SqlConnection con = new SqlConnection(constr))
                        {

                            using (SqlCommand cmd = new SqlCommand(com))
                            {
                                cmd.Connection = con;
                                con.Open();
                                lrt = cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        if (lrt > 0)
                        {
                            using (SqlConnection con = new SqlConnection(constr))
                            {

                                using (SqlCommand cmd = new SqlCommand(@"  INSERT INTO PR_SchedulerStatus (SchedulerDate,rec_status) VALUES
  (	GETDATE(),1)"))
                                {




                                    cmd.Connection = con;
                                    con.Open();
                                    long lrt1 = cmd.ExecuteNonQuery();
                                    con.Close();
                                }



                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal('Pickup Request Generated');", true);
                            Response.Redirect("GeneratePickupRequestList.aspx", true);

                        }


                    }
                    catch (Exception ex)
                    {
                        con3.Close();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal('"+ex+"');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal('Do Attendance First');", true);
              

                }
            }


        }

        protected void SendSMS(long PickupID)
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                string com2 = @"SELECT ppr.PickupRequestID, cast(ppr.PickupDate AS date) AS PickupDate, ppr.ContactNo, r.name + ' ('+r.routeCode+')' AS Route, r2.firstName + ' '+ r2.lastName+' ('+ r2.riderCode+')' AS Rider, ppr.status,r2.phoneNo,
ppr.Product,case when ppr.AccountNumber='0' THEN 'Cash' ELSE ppr.AccountNumber END AS Account, ppr.FName AS Customer,
('House # '+House + ', Building- ' + Building + ', Street- '+ Street+ ', Sector- ' + Sector + ', Area- '+ Area+ ', Plot No- ' + plotno + ', Floor- ' + floorNo ) AS Address
  FROM PR_PickupRequest ppr
INNER JOIN routes r ON r.routeCode = ppr.RouteCode
INNER JOIN Riders r2 ON r2.riderCode = ppr.RiderCode
WHERE PPR.PickupRequestID =" + PickupID;
                SqlDataAdapter adpt2 = new SqlDataAdapter(com2, con);
                DataTable dt2 = new DataTable();
                adpt2.Fill(dt2);
                con.Close();
                if (dt2 != null)
                {

                    if (dt2.Rows[0][5].ToString() == "2")
                    {
                        string Message = @"Pickup request with the ID# " + dt2.Rows[0][0].ToString() + ",Product: " + dt2.Rows[0][7].ToString() + ", Account#: " + dt2.Rows[0][8].ToString() + ", Customer: " + dt2.Rows[0][9].ToString() + ", Address: " + dt2.Rows[0][10].ToString() +
                                        ", is assigned to you for the Pickup date: " + dt2.Rows[0][1].ToString();

                        SqlCommand cmd = new SqlCommand(@"INSERT INTO MnP_SmsStatus
                                    ( ConsignmentNumber, Recepient,MessageContent, [STATUS], CreatedOn, CreatedBy )
                                    VALUES(NULL,'" + dt2.Rows[0][6].ToString() + "','" + Message + "', 0, GETDATE()," + HttpContext.Current.Session["U_ID"] + " )");


                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

    }
}