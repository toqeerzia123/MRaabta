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
    public partial class CustomerBranchMapping : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString_"].ToString(); // connection string
        //string constr = ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString(); // connection string
        public string getMSG(string msg)
        {
            string javas = "<script type=\"text/javascript\"> swal(' " + msg + "') </script>";
            return javas;
        }                                                                                      //  SqlConnection con = new SqlConnection(constr);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["PR_MSG"] != null)
                {
                    string msg = Session["PR_MSG"].ToString();
                    Session["PR_MSG"] = null;

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", getMSG(msg));
                }
                //txtStartDate.Text = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                //txtEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                loadCombo();
                //BindGridShowLog();
                BindGrid();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            long lrt = 0;
            try
            {
                if (hdnCustomerId.Value == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Customer')", true);

                    return;
                }

                if (ddlLocationID.SelectedValue == "-1" && ddlLocationID.SelectedValue == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Location')", true);

                    return;
                }
                //if (ddlLocationID.SelectedValue == "-1")
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Location')", true);

                //    return;
                //}
                if (ddlRouteCode.SelectedValue == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please select Route Code')", true);

                    return;
                }


                SqlConnection con3 = new SqlConnection(constr);
                con3.Open();
                string com3 = @"SELECT * from  PR_ClientRoute  where  rec_status=1 " +
                    "  and CustomerID=" + Convert.ToInt32(hdnCustomerId.Value) + "   and LocationID=" + Convert.ToInt32(ddlLocationID.SelectedValue) + "  and BranchCode=" + Convert.ToInt32(ddlBranchCode.SelectedValue);
                SqlDataAdapter adpt3 = new SqlDataAdapter(com3, con3);
                DataTable dt3 = new DataTable();
                adpt3.Fill(dt3);
                con3.Close();
                if (dt3.Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Already Mapped')", true);

                    return;
                }

                using (SqlConnection con = new SqlConnection(constr))
                {

                    if (hdnCustomerId.Value == "-1")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Please Select Location')", true);
                        return;

                    }
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO PR_ClientRoute (LocationID, CustomerID,BranchCode,RouteCode,rec_status,WeightType)
                        VALUES (@LocationID, @CustomerID,@BranchCode,@RouteCode,1,@WeightType)"))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(hdnCustomerId.Value));
                        cmd.Parameters.AddWithValue("@LocationID", Convert.ToInt32(ddlLocationID.SelectedValue));
                        cmd.Parameters.AddWithValue("@BranchCode", Convert.ToInt32(ddlBranchCode.SelectedValue));
                        cmd.Parameters.AddWithValue("@RouteCode", ddlRouteCode.SelectedValue);
                        cmd.Parameters.AddWithValue("@WeightType", ddlIsHeavy.SelectedValue);


                        cmd.Connection = con;
                        con.Open();
                        lrt = cmd.ExecuteNonQuery();
                        con.Close();
                        txtSearch.Text = "";
                        hdnCustomerId.Value = "";
                        loadCombo();
                        if (lrt > 0)
                        {
                            Session["PR_MSG"] = "Branch Map Sucessfully";

                            Response.Redirect("CustomerBranchMapping.aspx", true);

                            //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Branch Map Sucessfully')", true);
                        }
                        else
                        {
                            Session["PR_MSG"] = "Branch cannot be map";
                            Response.Redirect("CustomerBranchMapping.aspx", true);
                            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Branch cannot be map')", true);

                        }
                    }


                }
                BindGrid();
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
                string com = "select BranchCode as ID,Name as Name from Branches where BranchCode='" + Session["BranchCode"].ToString() + "'";
                SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                con.Close();
                ddlBranchCode.DataSource = dt;
                ddlBranchCode.DataTextField = "Name";
                ddlBranchCode.DataValueField = "ID";
                ddlBranchCode.DataBind();

                con.Open();
                string com5 = $@"SELECT ID,Name FROM PR_WeightType pwt ";
                SqlDataAdapter adpt5 = new SqlDataAdapter(com5, con);
                DataTable dt5 = new DataTable();
                adpt5.Fill(dt5);
                con.Close();
                ddlIsHeavy.DataSource = dt5;
                ddlIsHeavy.DataTextField = "Name";
                ddlIsHeavy.DataValueField = "ID";
                ddlIsHeavy.DataBind();
                dt5.Dispose();
                dt5.Clear();

                con.Open();
                string com4 = @"select distinct r.routeCode AS ID,r.routeCode+'-'+name  as routeCode  from Routes r inner join Riders rr on rr.routeCode = r.routeCode 
                where r.status=1 and rr.status=1 and r.bid='" + Session["BranchCode"].ToString() + "' AND rr.branchId='" + Session["BranchCode"].ToString() + "'";
                if (ddlIsHeavy.SelectedValue.ToString() == "2")
                {
                    com4 += " and userTypeId in ('90') order by r.RouteCode";
                }
                if (ddlIsHeavy.SelectedValue.ToString() == "1")
                {
                    com4 += " and userTypeId in ('72','217') order by r.RouteCode";
                }
                SqlDataAdapter adpt4 = new SqlDataAdapter(com4, con);
                DataTable dt4 = new DataTable();
                adpt4.Fill(dt4);
                con.Close();
                ddlRouteCode.DataSource = dt4;
                ddlRouteCode.DataTextField = "routeCode";
                ddlRouteCode.DataValueField = "ID";
                ddlRouteCode.DataBind();
                ListItem item4 = new ListItem("Select Route Code", "-1");
                ddlRouteCode.Items.Insert(0, item4);
                dt4.Dispose();
                dt4.Clear();


            }
            catch (Exception)
            {

            }
        }
        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        //protected void grdPR_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName == "CmdCancel")
        //        {
        //            /*your code to download here */
        //            int PRid = Convert.ToInt32(e.CommandArgument.ToString());
        //            using (SqlConnection con = new SqlConnection(constr))
        //            {
        //                using (SqlCommand cmd = new SqlCommand("update PR_PickupRequest Set Status=@Status where PickupRequestID=@PickupRequestID"))
        //                {
        //                    cmd.Parameters.AddWithValue("@Status", 0);
        //                    cmd.Parameters.AddWithValue("@PickupRequestID", PRid);
        //                    cmd.Connection = con;
        //                    con.Open();
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }

        //                SqlCommand cmd1 = new SqlCommand("INSERT INTO PR_PickupRequestLog (PickupRequestID,CustomerID,PickupDate,Slot,Qauntity,Weight,UserID,UserType,Source,Status,RouteCode,RiderCode,ReasonCode,ActionUserID,Timestamp,VehicleRequired,LocationID) " +
        //                " select PickupRequestID,CustomerID,PickupDate,Slot,Qauntity,Weight,UserID,UserType,Source," + 0 + ",RouteCode,RiderCode," + 2 + ",UserID,getdate(),VehicleRequired,LocationID from PR_PickupRequestLog where PickupRequestID=" + PRid);
        //                cmd1.Connection = con;
        //                con.Open();
        //                cmd1.ExecuteNonQuery();
        //                con.Close();

        //            }
        //        }
        //        BindGrid();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        //private void BindGrid()
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SELECT PR.PickupRequestID ,PR.CustomerID,C.CustomerName, convert(varchar(10),PR.PickupDate,101) as PickupDate,CONVERT(VARCHAR(10), PR.Slot, 0) as Slot,PR.Qauntity,PR.Weight,PR.UserID,PR.UserType,PR.Source,PR.Status,PR.RouteCode,Ro.RouteName,PR.RiderCode,R.RiderName " +
        //                " from PR_PickupRequest PR  inner join Rider R on PR.RiderCode=R.RiderCode  inner join Customer C on PR.CustomerID=C.CustomerID  inner join [Route] Ro on PR.RouteCode=Ro.Route Where PR.Status=1"))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    cmd.Connection = con;
        //                    sda.SelectCommand = cmd;
        //                    using (DataTable dt = new DataTable())
        //                    {
        //                        sda.Fill(dt);
        //                        grdPR.DataSource = dt;
        //                        grdPR.DataBind();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        public void BindGrid()
        {
            string strwhere = "";
            try
            {
                if (hdnCustomerId.Value != "")
                {
                    strwhere = strwhere + " and PR.CustomerID= '" + hdnCustomerId.Value + "'";
                }

                if (ddlLocationID.SelectedValue != "-1" && ddlLocationID.SelectedValue != "")
                {
                    strwhere = strwhere + " and PR.LocationID= '" + ddlLocationID.SelectedValue + "'";
                }
                //if (ddlLocationID.SelectedValue != "")
                //{
                //    strwhere = strwhere + " and PR.LocationID= '" + ddlLocationID.SelectedValue + "'";
                //}

                if (ddlRouteCode.SelectedValue != "-1")
                {
                    strwhere = strwhere + " and PR.RouteCode= '" + ddlRouteCode.SelectedValue + "'";
                }


                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("select PR.ClientRouteID,PR.LocationID,L.locationName as Location,CASE PR.WeightType when 1 then 'Light' when 2 then 'Heavy' end as ShipmentType,PR.CustomerID,C.Name + ' (' + c.accountNo +')' as Customer, " +
                   " PR.BranchCode,B.Name as Branch, PR.RouteCode,Ro.routeCode+'-'+Ro.Name as Route from PR_ClientRoute PR " +
               " left join COD_CustomerLocations L on PR.LocationID=L.LocationID and l.brancahCode=" + ddlBranchCode.SelectedValue +
                " inner join CreditClients C on PR.CustomerID=C.ID and c.branchCode=" + ddlBranchCode.SelectedValue +
                " inner join Branches B on PR.BranchCode=B.BranchCode  " +
                " inner join Routes Ro on PR.RouteCode=Ro.RouteCode  and ro.status = 1 and ro.BID=" + ddlBranchCode.SelectedValue +
                             @"where PR.Rec_Status=1 and b.branchCode= " + ddlBranchCode.SelectedValue + strwhere, con);
                //inner join Riders R on PR.RiderCode=R.RiderCode //PR.RiderCode,R.FirstName as Rider,

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {

            }

        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long lrt = 0;
            try
            {
                int ClientRouteID = Convert.ToInt16(GridView1.DataKeys[e.RowIndex].Values["ClientRouteID"].ToString());
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("  update  PR_ClientRoute set  Rec_Status=2  where ClientRouteID = @id", con);
                cmd.Parameters.AddWithValue("@id", ClientRouteID);
                con.Open();
                lrt = cmd.ExecuteNonQuery();
                con.Close();
                BindGrid();
                if (lrt > 0)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Record has been deleted')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Record cannot be deleted')", true);

                }

            }
            catch (Exception ex)
            {

            }

        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            long lrt = 0;
            try
            {
                int ClientRouteID = Convert.ToInt16(GridView1.DataKeys[e.RowIndex].Values["ClientRouteID"].ToString());

                DropDownList grdLocationID = GridView1.Rows[e.RowIndex].FindControl("ddlLocationID") as DropDownList;
                DropDownList grdCustomerID = GridView1.Rows[e.RowIndex].FindControl("ddlCustomerID") as DropDownList;
                DropDownList grdBranchCode = GridView1.Rows[e.RowIndex].FindControl("ddlBranchCode") as DropDownList;
                DropDownList grdShipmentType = GridView1.Rows[e.RowIndex].FindControl("ddlIsHeavy") as DropDownList;

                DropDownList grdRouteCode = GridView1.Rows[e.RowIndex].FindControl("ddlRouteCode") as DropDownList;

                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("update PR_ClientRoute set LocationID=@LocationID, CustomerID=@CustomerID,BranchCode=@BranchCode,RouteCode=@RouteCode, WeightType=@WeightType where ClientRouteID =@ClientRouteID", con);
                cmd.Parameters.AddWithValue("@LocationID", grdLocationID.SelectedValue);
                cmd.Parameters.AddWithValue("@CustomerID", grdCustomerID.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchCode", grdBranchCode.SelectedValue);
                cmd.Parameters.AddWithValue("@RouteCode", grdRouteCode.SelectedValue);
                cmd.Parameters.AddWithValue("@WeightType", grdShipmentType.SelectedValue);
                cmd.Parameters.AddWithValue("@ClientRouteID", ClientRouteID);
                con.Open();
                lrt = cmd.ExecuteNonQuery();
                con.Close();
                GridView1.EditIndex = -1;
                BindGrid();
                if (lrt > 0)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Branch Map Sucessfully')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "swal('Branch cannot be map')", true);

                }

            }
            catch (Exception ex)
            {

            }

        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                    {
                        DropDownList ddListBranch = (DropDownList)e.Row.FindControl("ddlBranchCode");
                        DataTable dt = getDataTable("BranchCode", "Name", "Branches", " where BranchCode='" + Session["BranchCode"].ToString() + "'");
                        ddListBranch.DataSource = dt;
                        ddListBranch.DataTextField = "Name";
                        ddListBranch.DataValueField = "ID";
                        ddListBranch.DataBind();
                        DataRowView dr2 = e.Row.DataItem as DataRowView;
                        ddListBranch.SelectedValue = dr2["BranchCode"].ToString();
                        ddListBranch.Enabled = false;

                        DropDownList ddListCustomer = (DropDownList)e.Row.FindControl("ddlCustomerID");
                        dt = getDataTable("ID", "Name", "CreditClients", " where BranchCode='" + Session["BranchCode"].ToString() + "'");
                        ddListCustomer.DataSource = dt;
                        ddListCustomer.DataTextField = "Name";
                        ddListCustomer.DataValueField = "ID";
                        ddListCustomer.DataBind();
                        DataRowView dr1 = e.Row.DataItem as DataRowView;
                        ddListCustomer.SelectedValue = dr1["CustomerID"].ToString();
                        ddListCustomer.Enabled = false;

                        DropDownList ddListLocation = (DropDownList)e.Row.FindControl("ddlLocationID");
                        dt = getDataTable("locationID", "locationName", "COD_CustomerLocations", " Where status=1 and  brancahCode=" + Session["BranchCode"].ToString() + "  and CreditClientID='" + ddListCustomer.SelectedValue + "'");
                        ddListLocation.DataSource = dt;
                        ddListLocation.DataTextField = "Name";
                        ddListLocation.DataValueField = "ID";
                        ddListLocation.DataBind();
                        dt.Dispose();
                        dt.Clear();
                        DataRowView dr = e.Row.DataItem as DataRowView;
                        ddListLocation.SelectedValue = dr["LocationID"].ToString();
                        ddListLocation.Enabled = false;



                        ////  dt = getDataTable("RouteCode", "Name", "Routes", "   where BID=" + Session["BranchCode"].ToString());
                        //  string com4 = @"select routeCode AS ID,routeCode+'-'+name  as routeCode from Routes where status=1 and bid=" + Session["BranchCode"].ToString() + " order by RouteCode";
                        //  SqlDataAdapter adpt4 = new SqlDataAdapter(com4, con);
                        //  DataTable dt4 = new DataTable();
                        //  adpt4.Fill(dt4);
                        //  con.Close();

                        //  ddListRoute.DataSource = dt;
                        //  ddListRoute.DataTextField = "routeCode";
                        //  ddListRoute.DataValueField = "ID";
                        //  ddListRoute.DataBind();
                        //  DataRowView dr4 = e.Row.DataItem as DataRowView;
                        //  ddListRoute.SelectedValue = dr4["RouteCode"].ToString();

                        SqlConnection con = new SqlConnection(constr);

                        DropDownList ddlListShipmentType = (DropDownList)e.Row.FindControl("ddlIsHeavy");
                        con.Open();
                        string com5 = $@"SELECT ID,Name FROM PR_WeightType pwt ";
                        SqlDataAdapter adpt2 = new SqlDataAdapter(com5, con);
                        DataTable dtnew1 = new DataTable();
                        adpt2.Fill(dtnew1);
                        con.Close();
                        ddlListShipmentType.DataSource = dtnew1;
                        ddlListShipmentType.DataTextField = "Name";
                        ddlListShipmentType.DataValueField = "ID";
                        ddlListShipmentType.DataBind();
                        dtnew1.Dispose();
                        dtnew1.Clear();
                        DataRowView dr8 = e.Row.DataItem as DataRowView;
                        if (dr8["ShipmentType"].ToString() != "" && dr8["ShipmentType"].ToString() != null && dr8["ShipmentType"].ToString() != "-1")
                        {
                            if (dr8["ShipmentType"].ToString()=="Heavy")
                            {
                                ddlListShipmentType.SelectedValue = "2";
                            }
                            if (dr8["ShipmentType"].ToString() == "Light")
                            {
                                ddlListShipmentType.SelectedValue = "1";
                            }

                        }

                        DropDownList ddListRoute = (DropDownList)e.Row.FindControl("ddlRouteCode");

                        con.Open();
                        string com1 = $@"select distinct r.routeCode AS ID,r.routeCode+'-'+name  as routeCode  from Routes r inner join Riders rr on rr.routeCode = r.routeCode 
                        where r.status=1 AND rr.[status]=1 and r.bid='" + Session["BranchCode"].ToString() + "' AND rr.branchId='" + Session["BranchCode"].ToString()+"'";
                        if (ddlListShipmentType.SelectedValue.ToString() == "2")
                        {
                            com1 += " and userTypeId in ('90') order by r.RouteCode";
                        }
                        if (ddlListShipmentType.SelectedValue.ToString() == "1")
                        {
                            com1 += " and userTypeId in ('72','217') order by r.RouteCode";
                        }
                        SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
                        DataTable dtnew = new DataTable();
                        adpt1.Fill(dtnew);
                        con.Close();
                        ddListRoute.DataSource = dtnew;
                        ddListRoute.DataTextField = "routeCode";
                        ddListRoute.DataValueField = "ID";
                        ddListRoute.DataBind();


                        dtnew.Dispose();
                        dtnew.Clear();
                        DataRowView dr7 = e.Row.DataItem as DataRowView;
                        if (dr7["RouteCode"].ToString() != "")
                        {
                            ListItem item4 = new ListItem(dr7["Route"].ToString(), dr7["RouteCode"].ToString());
                            ddListRoute.Items.Insert(0, item4);
                            //ddlRouteCode.Text = dr7["RouteCode"].ToString();
                            //ddlRouteCode.SelectedValue = dr7["RouteCode"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public DataTable getDataTable(string ID, string Name, string tbName, string strWhere)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select " + ID + " as ID, " + Name + " as Name from " + tbName + strWhere + " ", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        protected void hdnCustomerId_ValueChanged(object sender, EventArgs e)
        {
            // Load    ()
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            string com1 = "select LocationID as ID, locationName as Name from COD_CustomerLocations where status=1 and  brancahCode=" + Session["BranchCode"].ToString() + " and CreditClientID=" + hdnCustomerId.Value + " order by LocationID asc ";
            SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
            DataTable dt1 = new DataTable();
            adpt1.Fill(dt1);
            con.Close();
            ddlLocationID.DataSource = dt1;

            ddlLocationID.DataTextField = "Name";
            ddlLocationID.DataValueField = "ID";
            ddlLocationID.DataBind();

            ListItem item4 = new ListItem("Select Location", "-1");
            ddlLocationID.Items.Insert(0, item4);
            dt1.Dispose();
            dt1.Clear();

            txtSearch.Text = hfCp.Value;
        }

        protected void ddlCustomerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlMake = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlMake.NamingContainer;
            if (row != null)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    //DropDownList ddlModel = (DropDownList)row.FindControl("ddlProductModel");
                    //ddlModel.DataSource = GetProductModelByMake(Convert.ToInt32(ddlMake.SelectedValue));
                    //ddlModel.DataValueField = "ProductModelID";
                    //ddlModel.DataTextField = "Model";
                    //ddlModel.DataBind();

                    DropDownList ddListLocation = (DropDownList)row.FindControl("ddlLocationID");
                    DataTable dt = getDataTable("locationID", "locationName", "COD_CustomerLocations", " Where brancahCode=" + Session["BranchCode"].ToString() + " and status=1 and CreditClientID='" + ddlMake.SelectedValue + "'  order by LocationID asc; ");
                    ddListLocation.DataSource = dt;
                    ddListLocation.DataTextField = "Name";
                    ddListLocation.DataValueField = "ID";
                    ddListLocation.DataBind();
                    dt.Dispose();
                    dt.Clear();

                }
            }
        }
        protected void ddlShipmentTypeDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();

                string com4 = @"select distinct r.routeCode AS ID,r.routeCode+'-'+name  as routeCode  from Routes r inner join Riders rr on rr.routeCode = r.routeCode 
                where r.status=1 AND rr.[status]=1 and rr.branchid= '" + Session["BranchCode"].ToString() + "' and r.bid='" + Session["BranchCode"].ToString() + "'";
                if (ddlIsHeavy.SelectedValue.ToString() == "2")
                {
                    com4 += " and userTypeId in ('90') order by r.RouteCode";
                }
                if (ddlIsHeavy.SelectedValue.ToString() == "1")
                {
                    com4 += " and userTypeId in ('72','217') order by r.RouteCode";
                }
                SqlDataAdapter adpt4 = new SqlDataAdapter(com4, con);
                DataTable dt4 = new DataTable();
                adpt4.Fill(dt4);
                con.Close();
                ddlRouteCode.DataSource = dt4;
                ddlRouteCode.DataTextField = "routeCode";
                ddlRouteCode.DataValueField = "ID";
                ddlRouteCode.DataBind();
                ListItem item4 = new ListItem("Select Route Code", "-1");
                ddlRouteCode.Items.Insert(0, item4);
                dt4.Dispose();
                dt4.Clear();

            }
            catch (Exception)
            {

            }
        }
        protected void ddlShipmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlMake = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlMake.NamingContainer;
            if (row != null)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddListRoute = (DropDownList)row.FindControl("ddlRouteCode");
                    DropDownList ddListType = (DropDownList)row.FindControl("ddlIsHeavy");
                    try
                    {
                        SqlConnection con = new SqlConnection(constr);
                        con.Open();

                        string com4 = @"select distinct r.routeCode AS ID,r.routeCode+'-'+name  as routeCode  from Routes r inner join Riders rr on rr.routeCode = r.routeCode 
                where r.status=1 AND rr.[status]=1 and rr.branchid= '" + Session["BranchCode"].ToString() + "' and r.bid='" + Session["BranchCode"].ToString() + "'";
                        if (ddListType.SelectedValue.ToString() == "2")
                        {
                            com4 += " and userTypeId in ('90') order by r.RouteCode";
                        }
                        if (ddListType.SelectedValue.ToString() == "1")
                        {
                            com4 += " and userTypeId in ('72','217') order by r.RouteCode";
                        }
                        SqlDataAdapter adpt4 = new SqlDataAdapter(com4, con);
                        DataTable dt4 = new DataTable();
                        adpt4.Fill(dt4);
                        con.Close();
                        ddListRoute.DataSource = dt4;
                        ddListRoute.DataTextField = "routeCode";
                        ddListRoute.DataValueField = "ID";
                        ddListRoute.DataBind();
                        ListItem item4 = new ListItem("Select Route Code", "-1");
                        ddlRouteCode.Items.Insert(0, item4);
                        dt4.Dispose();
                        dt4.Clear();
                        //DataRowView dr7 = row.Cells("ddlRouteCode") as DataRowView;
                        //if (dr7["RouteCode"].ToString() != "")
                        //{
                        //    ListItem item4 = new ListItem(dr7["Route"].ToString(), dr7["RouteCode"].ToString());
                        //    ddListRoute.Items.Insert(0, item4);
                        //    //ddlRouteCode.Text = dr7["RouteCode"].ToString();
                        //    //ddlRouteCode.SelectedValue = dr7["RouteCode"].ToString();
                        //}

                    }
                    catch (Exception)
                    {

                    }

                }
            }
        }

        //protected void txtAccountNumber_TextChanged(object sender, EventArgs e)
        //{
        //    SqlConnection con = new SqlConnection(constr);
        //    con.Open();
        //    string com1 = "select * from CreditClients WHERE accountNo != '' and accountNo='" + txtAccountNumber.Text + "'";
        //    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
        //    DataTable dt1 = new DataTable();
        //    adpt1.Fill(dt1);
        //    con.Close();
        //    if (dt1.Rows.Count > 0)
        //    {
        //        ///txtAccountName.Text = dt1.Rows[0]["name"].ToString();
        //    }
        //}

        //protected void ddlBranchCode_TextChanged(object sender, EventArgs e)
        //{
        //    SqlConnection con = new SqlConnection(constr);
        //    con.Open();
        //    string com1 = "select * from CreditClients WHERE accountNo != '' and accountNo='" + txtAccountNumber.Text + "'";
        //    SqlDataAdapter adpt1 = new SqlDataAdapter(com1, con);
        //    DataTable dt1 = new DataTable();
        //    adpt1.Fill(dt1);
        //    con.Close();
        //    if (dt1.Rows.Count > 0)
        //    {
        //        ddlCustomerID.SelectedValue = dt1.[];
        //    }
        //}
    }
}