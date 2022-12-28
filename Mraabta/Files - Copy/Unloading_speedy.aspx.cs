using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using Dapper;

namespace MRaabta.Files
{
    public partial class Unloading_speedy : System.Web.UI.Page
    {

        static Cl_Variables clvar = new Cl_Variables();
        Variable clvar_ = new Variable();
        LoadingPrintReport_NEW con = new LoadingPrintReport_NEW();
        CommonFunction func = new CommonFunction();
        LoadingPrintReport cl_lp = new LoadingPrintReport();
        bayer_Function b_fun = new bayer_Function();

        protected void Page_Load(object sender, EventArgs e)
        {
            string monthName = "January";
            int month = DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
            if (Vehicle.Checked)
            {
                vehiclediv1.Visible = true;
                vehiclediv2.Visible = true;

                regdiv1.Visible = false;
                regdiv2.Visible = false;
            }

            if (Rented.Checked)
            {
                vehiclediv1.Visible = false;
                vehiclediv2.Visible = false;

                regdiv1.Visible = true;
                regdiv2.Visible = true;
            }

            if (tab_1.Checked)
            {
                tab_1_Div.Style.Add("display", "block");// = true;
                tab_2_Div.Style.Add("display", "none");// = false;
            }

            if (tab_2.Checked)
            {
                tab_2_Div.Style.Add("display", "block");// = true;
                tab_1_Div.Style.Add("display", "none");// = false;
            }
            if (!IsPostBack)
            {
                // tbl_bagnew.Visible = false;
                // tbl_cns.Visible = false;

                dd_destination.Enabled = false;
                dd_orign.Enabled = false;
                dd_destination.Enabled = false;
                dd_route.Enabled = false;
                dd_transporttype.Enabled = false;
                dd_vehicle.Enabled = false;
                //txt_vid.Enabled = false;
                Get_Destination();
                flight.Visible = false;
                Get_Orign();
                Get_Destination();
                Get_MasterRoute();
                Get_MasterTransportType();
                Get_MasterVehicle();


                txt_date.Text = Session["WorkingDate"].ToString();
                GetRoutes();
                GetStatuses();



                DataTable bags = new DataTable();
                bags.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("BagNo"),
                    new DataColumn("Description"),
                    new DataColumn("TotalWeight"),
                    new DataColumn("originId"),
                    new DataColumn("destid"),
                    new DataColumn("DestBranchId"),
                    new DataColumn("originBranchID"),
                    new DataColumn("Status"),
                    new DataColumn("SealNo"),
                    new DataColumn("BagStatus") ,
                    new DataColumn("Remarks")

                });
                bags.AcceptChanges();
                ViewState["temp"] = null;



                DataTable Cns = new DataTable();
                Cns.Columns.AddRange(new DataColumn[]
                {

                    new DataColumn("cnno"),
                    new DataColumn("Description"),
                    new DataColumn("weight"),
                    new DataColumn("pieces"),
                    new DataColumn("originId"),
                    new DataColumn("destid"),
                    new DataColumn("DestBranchId"),
                    new DataColumn("originBranchID"),
                    new DataColumn("cnStatus"),
                    new DataColumn("Remarks")
            });
                Cns.AcceptChanges();


                ViewState["bags"] = bags;
                ViewState["cns"] = Cns;

                // Consignment
                GetCNLengths();


                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        public void GetCNLengths()
        {
            string query = "SELECT * FROM MNP_ConsignmentLengths where status = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                ViewState["cnLengths"] = dt;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            cnControls.DataSource = dt;
            cnControls.DataBind();
        }
        public void Get_Orign()
        {

            DataTable ds_orign = Cities_();

            if (ds_orign.Rows.Count != 0)
            {
                dd_orign.DataTextField = "BranchName";
                dd_orign.DataValueField = "branchCode";
                dd_orign.DataSource = ds_orign.DefaultView;
                dd_orign.DataBind();

                dd_orign.SelectedValue = HttpContext.Current.Session["BranchCode"].ToString();
            }
        }



        public void Get_Destination()
        {
            //DataSet ds_destination =  b_fun.Get_MasterDestination(clvar);
            DataTable ds_destination = Cities_();// b_fun.Get_MasterDestination(clvar);
            if (ds_destination.Rows.Count != 0)
            {
                dd_destination.DataTextField = "BranchName";
                dd_destination.DataValueField = "branchCode";
                dd_destination.DataSource = ds_destination.DefaultView;
                dd_destination.DataBind();
                ViewState["destinations"] = ds_destination;

            }
        }

        public void Get_MasterRoute()
        {
            DataSet ds_route = Get_MasterRoute(clvar_);
            dd_route.Items.Clear();
            if (ds_route.Tables[0].Rows.Count != 0)
            {
                dd_route.DataTextField = "Name";
                dd_route.DataValueField = "MovementRouteId";
                dd_route.DataSource = ds_route.Tables[0].DefaultView;
                dd_route.DataBind();
            }
            dd_route.Items.Insert(0, new ListItem("Select Route ", ""));
        }

        protected void Route_SelectedIndexChanged(object sender, EventArgs e)
        {
            clvar_._Route = dd_route.SelectedValue;

            DataSet ds_touchpoint = b_fun.Get_RouteByRouteId(clvar_);

            //if (ds_touchpoint.Tables[0].Rows.Count != 0)
            //{
            //    dd_touchpoint.DataTextField = "name";
            //    dd_touchpoint.DataValueField = "MovementRouteId";
            //    dd_touchpoint.DataSource = ds_touchpoint.Tables[0].DefaultView;
            //    dd_touchpoint.DataBind();
            //}


            DataSet ds_destination = Get_MasterDestinationbyRouteId(clvar_);

            if (ds_destination.Tables[0].Rows.Count != 0)
            {
                //dd_destination.DataTextField = "BranchName";
                //dd_destination.DataValueField = "branchCode";
                //dd_destination.DataSource = ds_destination.Tables[0].DefaultView;
                //dd_destination.DataBind();

                dd_orign.SelectedValue = ds_destination.Tables[0].Rows[0]["ORiginCode"].ToString();
            }
        }

        public void Get_MasterTransportType()
        {
            DataSet ds_transporttype = cl_lp.Get_MasterTransportType(clvar_);

            if (ds_transporttype.Tables[0].Rows.Count != 0)
            {
                dd_transporttype.DataTextField = "AttributeDesc";
                dd_transporttype.DataValueField = "id";
                dd_transporttype.DataSource = ds_transporttype.Tables[0].DefaultView;
                dd_transporttype.DataBind();
            }
            dd_transporttype.Items.Insert(0, new ListItem("Select Transport Type ", ""));
        }

        public void Get_MasterVehicle()
        {
            DataSet ds_vehicle = b_fun.Get_MasterVehicle(clvar_);

            if (ds_vehicle.Tables[0].Rows.Count != 0)
            {
                dd_vehicle.DataTextField = "MakeModel";
                dd_vehicle.DataValueField = "VehicleCode";
                dd_vehicle.DataSource = ds_vehicle.Tables[0].DefaultView;
                dd_vehicle.DataBind();
            }
            dd_vehicle.Items.Insert(0, new ListItem("Select Vehicle ", ""));
        }

        protected void dd_transporttype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (dd_transporttype.SelectedValue == "197")
            {
                flight.Visible = true;
            }
            else
            {
                flight.Visible = false;
            }

        }



        protected void GetRoutes()
        {
            //DataTable dt = con.GetRoutesByDestination();
            //if (dt != null)
            //{
            //    if (dt.Rows.Count > 0)
            //    {
            //        dd_route.DataSource = dt;
            //        dd_route.DataTextField = "Name";
            //        dd_route.DataValueField = "MovementRouteCode";
            //        dd_route.DataBind();
            //    }
            //}
        }
        protected void GetStatuses()
        {
            DataTable dt = func.GetReceivingStatus();
            ViewState["ReceivingStatuses"] = dt;
        }
        protected void btn_add_Click(object sender, EventArgs e)
        {

        }
        protected void chk_received_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;
            if ((gr.FindControl("chk_received") as CheckBox).Checked)
            {
                //gr.Cells[2].Text = "Received";

                (gr.FindControl("lbl_gDescription") as Label).Text = "Received";
            }
            else
            {
                gr.Cells[2].Text = "";
            }

        }
        protected void txt_loadingID_TextChanged(object sender, EventArgs e)
        {
            if (txt_vid.Text.Trim(' ') != "")
            {
                bool unloaded = false;
                clvar.LoadingID = txt_vid.Text;



                DataTable detail = GetLoading(clvar);

                DataTable dt = new DataTable();//con.GetLoadingBags(clvar);

                DataTable dtC = new DataTable();// con.GetConsignmentForUnload(clvar);

                if (detail != null)
                {
                    hdValue.Value = detail.Rows.Count.ToString();

                    if (detail.Rows.Count > 0)
                    {
                        dd_route.Enabled = false;
                        dd_orign.Enabled = false;
                        dd_destination.Enabled = false;
                        dd_transporttype.Enabled = false;
                        dd_vehicle.Enabled = false;
                        Rented.Enabled = false;
                        Vehicle.Enabled = false;

                        if (detail.Rows.Count != 0)
                        {
                            if (detail.Rows[0]["isunloaded"].ToString() == "1")
                            {
                                unloaded = true;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already Unloaded. Cannot Unload Again.')", true);
                                btn_save.Enabled = false;
                                txt_vid.Enabled = true;
                                hd_unloaded.Value = "1";
                                dt = GetLoadingBags(clvar);
                                ViewState["bags"] = dt;
                                dtC = GetConsignmentForUnload(clvar);
                                ViewState["cns"] = dtC;

                            }
                            else
                            {
                                txt_vid.Enabled = false;
                                dd_route.SelectedValue = detail.Rows[0]["RouteID"].ToString();
                                Route_SelectedIndexChanged(this, e);
                                dd_destination.SelectedValue = detail.Rows[0]["Destination"].ToString();
                                ListItem li = dd_orign.Items.FindByValue(detail.Rows[0]["origin"].ToString());
                                if (li != null)
                                {
                                    dd_orign.SelectedValue = detail.Rows[0]["Origin"].ToString();
                                }
                                else
                                {
                                    dd_orign.Items.Add(new ListItem { Text = detail.Rows[0]["OriginBranch"].ToString(), Value = detail.Rows[0]["Origin"].ToString() });
                                }

                                txt_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
                                txt_description.Text = detail.Rows[0]["LoadDescription"].ToString();
                                dd_transporttype.SelectedValue = "27";
                                // dd_transporttype.SelectedValue = detail.Rows[0]["TransportationType"].ToString();
                                dd_vehicle.SelectedValue = detail.Rows[0]["VehicleID"].ToString();
                                if (dd_vehicle.SelectedValue == "103")
                                {
                                    txt_reg.Text = detail.Rows[0]["VehicleRegNo"].ToString();
                                    Rented.Checked = true;
                                    vehiclediv1.Visible = false;
                                    vehiclediv2.Visible = false;
                                    regdiv1.Visible = true;
                                    regdiv2.Visible = true;
                                }
                                else
                                {
                                    Vehicle.Checked = true;
                                    vehiclediv1.Visible = true;
                                    vehiclediv2.Visible = true;
                                    regdiv1.Visible = false;
                                    regdiv2.Visible = false;
                                }

                                txt_couriername.Text = detail.Rows[0]["CourierName"].ToString();
                                txt_seal.Text = detail.Rows[0]["SealNo"].ToString();
                                hd_unloaded.Value = "0";
                                btn_save.Enabled = true;
                                dt = GetLoadingBags(clvar);
                                ViewState["bags"] = dt;
                                dtC = GetConsignmentForUnload(clvar);
                                ViewState["cns"] = dtC;
                            }
                        }

                        if (dt != null || dtC != null)
                        {
                            string result = "";
                            string result_ = "";
                            bool flag = false;
                            if (dt.Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                ds.Tables.Add(dt);
                                result = DataSetToJSON__(ds);
                                flag = true;
                            }
                            if (dtC.Rows.Count > 0)
                            {
                                DataSet ds_ = new DataSet();
                                ds_.Tables.Add(dtC);
                                result_ = DataSetToJSON__(ds_);
                                flag = true;
                            }
                            if (flag)
                            {
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "response", "LoadTable('" + result + "','" + result_ + "','" + unloaded + "');", true);
                            }
                        }
                    }
                    else
                    {
                        //gv_bagnew.DataSource = null;
                        //gv_bagnew.DataBind();
                        dd_route.Enabled = true;
                        //dd_orign.Enabled = true;
                        //dd_destination.Enabled = true;
                        dd_transporttype.Enabled = true;
                        dd_vehicle.Enabled = true;
                        Rented.Enabled = true;
                        Vehicle.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Loading Found!.')", true);
                    }

                }
                else
                {
                    dd_route.Enabled = false;
                    //dd_orign.Enabled = false;
                    //dd_destination.Enabled = false;
                    dd_transporttype.Enabled = false;
                    dd_vehicle.Enabled = false;
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly Insert Proper Loading ID')", true);
            }
        }

        private DataTable GetUnloadingLoading(Cl_Variables clvar)
        {
            throw new NotImplementedException();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string loadingID_Changed(string VID)
        {
            string result = null;

            clvar.LoadingID = VID;
            DataTable detail = GetLoading__(clvar);
            DataSet ds = new DataSet();
            ds.Tables.Add(detail);
            result = DataSetToJSON(ds);
            return result;
        }


        public static string DataSetToJSON(DataSet ds)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DataTable dt in ds.Tables)
            {
                object[] arr = new object[dt.Rows.Count + 1];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    arr[i] = dt.Rows[i].ItemArray;
                }

                dict.Add(dt.TableName, arr);
            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(dict);


        }
        public string DataSetToJSON__(DataSet ds)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DataTable dt in ds.Tables)
            {
                object[] arr = new object[dt.Rows.Count + 1];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    arr[i] = dt.Rows[i].ItemArray;
                }

                dict.Add(dt.TableName, arr);
            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(dict);


        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SaveChangedValues_saveButton(string unloadingID, BagModel[] Bags, ConsignmentModel[] Consignments)
        {
            ReturnChanged resp = new ReturnChanged();
            resp.Bags = Bags;
            resp.Consignments = Consignments;
            string bagCommand = "";
            string cnCommand = "";
            List<string> queries = new List<string>();
            if (Bags.Count() > 0)
            {

                foreach (BagModel Bag in Bags)
                {
                    if (Bag.chk_received.ToUpper() == "FALSE" && Bag.hd_status == "6")
                    {
                        bagCommand = "insert into mnp_unloadingBag (unloadingID, bagNumber, BagDestination, CreatedBy, Createdon, UnloadingStateID, BagWeight, BagRemarks, BagOrigin, BagSeal ) \n" +
                            " Values ('" + unloadingID + "', '" + Bag.BagNo + "','" + Bag.Destination + "',\n" +
                            "'" + HttpContext.Current.Session["U_ID"].ToString() + "',GETDATE(), '" + Bag.hd_status + "','" + Bag.Weight + "', '" + Bag.Remarks + "',\n" +
                            "'" + Bag.Origin + "', '" + Bag.SealNo + "')";
                    }
                    else
                    {
                        bagCommand = "Update mnp_unloadingBag set bagWeight = '" + Bag.Weight + "', bagRemarks = '" + Bag.Remarks + "', BagSeal = '" + Bag.SealNo + "', ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', modifiedOn = GETDATE() where BagNumber = '" + Bag.BagNo + "' and unloadingID = '" + unloadingID + "'";
                    }

                    queries.Add(bagCommand);
                }
            }

            if (Consignments.Count() > 0)
            {
                foreach (ConsignmentModel Consignment in Consignments)
                {
                    if (Consignment.chk_received.ToUpper() == "FALSE" && Consignment.hd_descStatus == "6")
                    {
                        if (Consignment.hd_cnStatus.ToUpper() == "EXISTS")
                        {
                            cnCommand = "INSERT INTO MnP_UnLoadingConsignment (UnLoadingID, ConsignmentNumber, CNDestination, CreatedBy, Createdon, UnloadingStateID, cnWeight, cnPieces, cnRemarks, cnOrigin)\n" +
                                      "Values(\n" +
                                      "'" + unloadingID + "',\n" +
                                      "'" + Consignment.ConNumber + "',\n" +
                                      "'" + Consignment.hd_destination + "',\n" +
                                      "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                      "GETDATE(),\n" +
                                      "'" + Consignment.hd_descStatus + "',\n" +
                                      "'" + Consignment.Weight + "',\n" +
                                      "'" + Consignment.Pieces + "',\n" +
                                      "'" + Consignment.Remarks + "', '" + Consignment.hd_origin + "'\n" +
                                      " )";
                        }
                        else if (Consignment.hd_cnStatus.ToUpper() == "INSERT")
                        {
                            cnCommand = "INSERT INTO MnP_unLoadingConsignment (UnLoadingID, ConsignmentNumber, CNDestination, CreatedBy, Createdon, UnloadingStateID, cnWeight, cnPieces, cnRemarks, cnOrigin)\n" +
                                      "Values(\n" +
                                      "'" + unloadingID + "',\n" +
                                      "'" + Consignment.ConNumber + "',\n" +
                                      "'" + Consignment.hd_destination + "',\n" +
                                      "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                      "GETDATE(),\n" +
                                      "'" + Consignment.hd_descStatus + "',\n" +
                                      "'" + Consignment.Weight + "',\n" +
                                      "'" + Consignment.Pieces + "',\n" +
                                      "'" + Consignment.Remarks + "', '" + Consignment.hd_origin + "'\n" +
                                      ")";
                        }
                    }
                    else
                    {
                        cnCommand = "Update mnp_unloadingConsignment set cnPieces = '" + Consignment.Pieces + "', cnWeight = '" + Consignment.Weight + "', cnRemarks = '" + Consignment.Remarks + "', cnOrigin = '" + Consignment.hd_origin + "' where unloadingId = '" + unloadingID + "' and consignmentNumber = '" + Consignment.ConNumber + "'";
                    }
                    queries.Add(cnCommand);
                }


            }

            if (queries.Count == 0)
            {
                return "Unload Successful.;" + unloadingID.ToString() + "";
                //return resp;
            }
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            con.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = con;
            trans = con.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                resp.Status = "OK";
                foreach (string command in queries)
                {
                    sqlcmd.CommandText = command;
                    int obj = sqlcmd.ExecuteNonQuery();
                    if (obj <= 0)
                    {
                        trans.Rollback();
                        resp.Status = "";
                        break;
                    }
                }
                if (resp.Status == "OK")
                {
                    trans.Commit();
                }

            }
            catch (Exception ex)
            {
                trans.Rollback();
                //resp.Status = ex.Message;
            }
            finally { con.Close(); }
            Int64 unloadingID_ = 0;
            Int64.TryParse(unloadingID, out unloadingID_);
            return "Unload Successful.;" + unloadingID_.ToString() + "";
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();


        }
        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void btn_cancelDialogue_Click(object sender, EventArgs e)
        {
            divDialogue.Style.Add("display", "none");
        }



        protected void ResetAll()
        {

            //txt_carrier.Text = "";
            txt_couriername.Text = "";

            txt_date.Text = "";
            txt_description.Text = "";
            //txt_destination.Text = "";
            dd_destination.ClearSelection();


        }





        protected void btn_Search_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "search_unloading.aspx", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
        protected void gv_bagnew_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "&nbsp;")
                {
                    (e.Row.FindControl("txt_gSealNo") as TextBox).Enabled = true;
                }
                else
                {
                    (e.Row.FindControl("txt_gSealNo") as TextBox).Enabled = false;
                }
            }
        }


        #region MyRegion
        public DataTable GetLoading(Cl_Variables clvar)
        {

            string sqlString = "select l.id,\n" +
            "       l.transportationType,\n" +
            "       l.VehicleId,\n" +
            "       b.name OriginBranch,\n" +
            "       b2.name DestBranchID, l.origin, l.destination,\n" +
            "       l.date,\n" +
            "       l.Description LoadDescription,\n" +
            "       v.description + ' (' + v.name + ')' VehicleDescription,\n" +
            "       lu.AttributeDesc TransportType,\n" +
            "       l.CourierName,\n" +
            "       r.name RouteName, Case when u.refLoadingID is null then '0' else '1' end isUnloaded, l.routeid, l.vehicleRegNo, l.sealNo\n" +
            "  from MnP_Loading l\n" +
            " inner join Branches b\n" +
            "    on l.origin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on l.destination = b2.branchCode\n" +
            " inner join Vehicle v\n" +
            "    on v.Id = l.VehicleId\n" +
            "  left outer join rvdbo.Lookup lu\n" +
            "    on lu.Id = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.MovementRoute r\n" +
            "    on r.MovementRouteId = l.routeId\n" +
            "  left outer join MnP_Unloading u \n" +
            "    on u.refLoadingID = CAST(l.id as varchar)" +
            " where l.id = '" + clvar.LoadingID + "'";


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

        public static DataTable GetLoading__(Cl_Variables clvar)
        {

            string sqlString = "select l.LoadingId,\n" +
            "       l.TransportTypeId,\n" +
            "       l.VehicleId,\n" +
            "       b.name            OriginBranch,\n" +
            "       b2.name           DestBranchID,\n" +
            "       l.LoadingDate,\n" +
            "       l.Description LoadDescription,\n" +
            "       v.description + ' (' + v.name + ')' VehicleDescription,\n" +
            "       lu.AttributeDesc TransportType , l.CourierName, r.name RouteName\n" +
            "  from rvdbo.Loading l\n" +
            " inner join Branches b\n" +
            "    on l.OriginBranchId = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on l.DestBranchId = b2.branchCode\n" +
            " inner join Vehicle v\n" +
            "    on v.Id = l.VehicleId\n" +
            " inner join rvdbo.Lookup lu\n" +
            "    on lu.Id = l.TransportTypeId\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.MovementRoute r \n" +
            "    on r.MovementRouteId = l.MovementRouteId\n" +
            " where l.LoadingId = '" + clvar.LoadingID + "' and l.DestBranchId = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";


            sqlString = "select l.id,\n" +
            "       l.transportationType,\n" +
            "       l.VehicleId,\n" +
            "       b.name OriginBranch,\n" +
            "       b2.name DestBranchID, l.origin, l.destination,\n" +
            "       l.date,\n" +
            "       l.Description LoadDescription,\n" +
            "       v.description + ' (' + v.name + ')' VehicleDescription,\n" +
            "       lu.AttributeDesc TransportType,\n" +
            "       l.CourierName,\n" +
            "       r.name RouteName, Case when u.refLoadingID is null then '0' else '1' end isUnloaded, l.routeid, l.vehicleRegNo, l.sealNo\n" +
            "  from MnP_Loading l\n" +
            " inner join Branches b\n" +
            "    on l.origin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on l.destination = b2.branchCode\n" +
            " inner join Vehicle v\n" +
            "    on v.Id = l.VehicleId\n" +
            "  left outer join rvdbo.Lookup lu\n" +
            "    on lu.Id = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.MovementRoute r\n" +
            "    on r.MovementRouteId = l.routeId\n" +
            "  left outer join MnP_Unloading u \n" +
            "    on u.refLoadingID = CAST(l.id as varchar)" +
            " where l.id = '" + clvar.LoadingID + "'";


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

        public static DataTable GetLoading_(Cl_Variables clvar)
        {

            string sqlString = "select l.LoadingId,\n" +
            "       l.TransportTypeId,\n" +
            "       l.VehicleId,\n" +
            "       b.name            OriginBranch,\n" +
            "       b2.name           DestBranchID,\n" +
            "       l.LoadingDate,\n" +
            "       l.Description LoadDescription,\n" +
            "       v.description + ' (' + v.name + ')' VehicleDescription,\n" +
            "       lu.AttributeDesc TransportType , l.CourierName, r.name RouteName\n" +
            "  from rvdbo.Loading l\n" +
            " inner join Branches b\n" +
            "    on l.OriginBranchId = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on l.DestBranchId = b2.branchCode\n" +
            " inner join Vehicle v\n" +
            "    on v.Id = l.VehicleId\n" +
            " inner join rvdbo.Lookup lu\n" +
            "    on lu.Id = l.TransportTypeId\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.MovementRoute r \n" +
            "    on r.MovementRouteId = l.MovementRouteId\n" +
            " where l.LoadingId = '" + clvar.LoadingID + "' and l.DestBranchId = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";


            sqlString = "select l.id,\n" +
            "       l.transportationType,\n" +
            "       l.VehicleId,\n" +
            "       b.name OriginBranch,\n" +
            "       b2.name DestBranchID, l.origin, l.destination,\n" +
            "       l.date,\n" +
            "       l.Description LoadDescription,\n" +
            "       v.description + ' (' + v.name + ')' VehicleDescription,\n" +
            "       lu.AttributeDesc TransportType,\n" +
            "       l.CourierName,\n" +
            "       r.name RouteName, Case when u.refLoadingID is null then '0' else '1' end isUnloaded, l.routeid, l.vehicleRegNo, l.sealNo\n" +
            "  from MnP_Loading l\n" +
            " inner join Branches b\n" +
            "    on l.origin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on l.destination = b2.branchCode\n" +
            " inner join Vehicle v\n" +
            "    on v.Id = l.VehicleId\n" +
            "  left outer join rvdbo.Lookup lu\n" +
            "    on lu.Id = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.MovementRoute r\n" +
            "    on r.MovementRouteId = l.routeId\n" +
            "  left outer join MnP_Unloading u \n" +
            "    on u.refLoadingID = CAST(l.id as varchar)" +
            " where l.id = '" + clvar.LoadingID + "'";


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
        public DataTable GetLoadingBags(Cl_Variables clvar)
        {

            string sqlString = "\n" +
            "SELECT dbo.TRIM(LB.BAGNUMBER) BAGNO,\n" +
            "       '' DESCRIPTION,\n" +
            "       CASE WHEN (LB.BAGWEIGHT IS NULL OR lb.BagWeight = '') THEN b.totalWeight ELSE lb.BagWeight end  TOTALWEIGHT,\n" +
            "       B1.SNAME ORIGINBRANCHID,\n" +
            "       LB.BAGORIGIN ORIGINID,\n" +
            "       LB.BAGDESTINATION DESTID,\n" +
            "       B2.SNAME DESTBRANCHID,\n" +
            "       CASE\n" +
            "         WHEN ISNULL(B.STATUS, 0) = 0 THEN\n" +
            "          'Bagged'\n" +
            "         WHEN B.STATUS = 1 THEN\n" +
            "          'Unbagged'\n" +
            "       END STATUS,\n" +
            "       CASE WHEN (LB.BAGSEAL IS NULL OR lb.BagSeal = '') THEN b.sealNo ELSE lb.BagSeal END SEALNO,\n" +
            "       LB.REMARKS REMARKS,\n" +
            "       'EXISTS' BAGSTATUS,\n" +
            "       LB.ULOADINGSTATEID DESCSTATUS\n" +
            "  FROM MNP_LOADING L\n" +
            " INNER JOIN MNP_LOADINGBAG LB\n" +
            "    ON LB.LOADINGID = L.ID\n" +
            "  LEFT OUTER JOIN BAG B\n" +
            "    ON B.BAGNUMBER = LB.BAGNUMBER\n" +
            " INNER JOIN BRANCHES B1\n" +
            "    ON B1.BRANCHCODE = LB.BAGORIGIN\n" +
            " INNER JOIN BRANCHES B2\n" +
            "    ON B2.BRANCHCODE = LB.BAGDESTINATION\n" +
            "  LEFT OUTER JOIN MNP_UNLOADING UL\n" +
            "    ON UL.REFLOADINGID = CAST(L.ID AS NVARCHAR)\n" +
            " WHERE L.ID = '" + clvar.LoadingID + "'";

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
        public static DataTable GetBagByBagNumber(string BagNumber)
        {

            string sqlString = "select b.bagNumber bagNo,\n" +
            "       b1.name Origin,\n" +
            "       b2.name Destination,\n" +
            "       b.origin originID,\n" +
            "       b.destination DestID,\n" +
            "       case\n" +
            "         when ISNULL(b.status, 0) = '0' then\n" +
            "          'Unbagged'\n" +
            "         else\n" +
            "          'Bagged'\n" +
            "       end status,\n" +
            "       b.sealNo, b.totalWeight\n" +
            "  from Bag b\n" +
            " inner join Branches b1\n" +
            "    on b.origin = b1.branchCode\n" +
            " inner join Branches b2\n" +
            "    on b.destination = b2.branchCode\n" +
            " where b.bagNumber = '" + BagNumber + "'";



            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            finally { con.Close(); }

            return dt;
        }
        public DataTable GetConsignmentForUnload(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber cnNo,\n" +
            "               '' Description,\n" +
            "               c.weight,\n" +
            "               b1.name OriginBranchId, c.orgin OriginID, c.Destination DestID,\n" +
            "               b2.name DestBranchId,\n" +
            "               '' Reason, 'EXISTS' CNSTATUS\n" +
            "          from mnp_Loading l\n" +
            "         inner join MnP_LoadingConsignment lc\n" +
            "            on lc.LoadingId = l.id\n" +
            "         inner join consignment c\n" +
            "            on c.consignmentNumber = lc.consignmentNumber\n" +
            "         inner join Branches b1\n" +
            "            on b1.branchCode = c.orgin\n" +
            "         inner join Branches b2\n" +
            "            on b2.branchCode = c.destination\n" +
            "         where l.id = '" + clvar.LoadingID + "'";


            sqlString = "select lc.consignmentNumber cnNo,\n" +
           "       '' Description,\n" +
           "       lc.CNWeight weight,\n" +
           "       lc.cnPieces pieces,\n" +
           "       b1.name OriginBranchId,\n" +
           "       l.origin OriginID,\n" +
           "       lc.cnDestination DestID,\n" +
           "       b2.name DestBranchId,\n" +
           "       lc.Remarks Remarks,\n" +
           "       'EXISTS' CNSTATUS\n" +
           "  from mnp_Loading l\n" +
           " inner join MnP_LoadingConsignment lc\n" +
           "    on lc.LoadingId = l.id\n" +
           //" inner join consignment c\n" +
           //"    on c.consignmentNumber = lc.consignmentNumber\n" +
           " inner join Branches b1\n" +
           "    on b1.branchCode = l.origin\n" +
           " inner join Branches b2\n" +
           "    on b2.branchCode = lc.cndestination\n" +
           "  left outer join mnp_unloading ul\n" +
           "    on ul.RefLoadingID = CAST(l.id as nvarchar)\n" +
           //"  left outer join mnp_UnloadingConsignment ulc\n" +
           //"    on ulc.UnLoadingID = ul.ID \n" +
           " where l.id = '" + clvar.LoadingID + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
                DataTable dt_ = new DataTable();
                string consignmentNumbers = "";

                foreach (DataRow dr in dt.Rows)
                {
                    consignmentNumbers += "'" + dr["CNNO"].ToString() + "'";
                }
                consignmentNumbers = consignmentNumbers.Replace("''", "','");

                string query = "selecT c.consignmentNumber, c.orgin, b.name BranchName from consignment c inner join branches b on b.branchCode = c.orgin where c.consignmentNumber in (" + consignmentNumbers + ")";
                sda = new SqlDataAdapter(query, con);
                sda.Fill(dt_);

                foreach (DataRow dr in dt_.Rows)
                {
                    DataRow dr_ = dt.Select("CNNO = '" + dr["consignmentNumber"].ToString() + "'").FirstOrDefault();
                    dr_["OriginBranchId"] = dr["BranchName"].ToString();
                    dr_["OriginID"] = dr["orgin"].ToString();
                }


            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        public DataTable GetExcessConsignmentForUnload(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string query = "select c.consignmentNumber conNo, '' Description, c.orgin OriginID, c.Destination DestID, b1.name Origin, b2.name Destination, c.weight \n" +
                            " from Consignment c\n" +
                            "left outer join Branches b1\n" +
                            "   on b1.branchCode = c.orgin\n" +
                            "left outer join Branches b2\n" +
                            "   on b2.branchCode = c.destination\n" +
                            "where c.consignmentNumber = '" + clvar.consignmentNo + "'";
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

        public static DataTable GetExcessConsignmentForUnload_(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string query = "select c.consignmentNumber conNo, '' Description, c.orgin OriginID, c.Destination DestID, b1.name Origin, b2.name Destination, c.weight \n" +
                            " from Consignment c\n" +
                            "left outer join Branches b1\n" +
                            "   on b1.branchCode = c.orgin\n" +
                            "left outer join Branches b2\n" +
                            "   on b2.branchCode = c.destination\n" +
                            "where c.consignmentNumber = '" + clvar.consignmentNo + "'";
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

        public DataTable GetUnloadedBagsByRefNumber(Cl_Variables clvar)
        {
            string sqlString = "\n" +
             "select b.bagNumber bagNo,\n" +
             "       '' Description,\n" +
             "       b.TotalWeight,\n" +
             "       b1.name OriginBranchId, b.origin OriginID, b.Destination DestID,\n" +
             "       b2.name DestBranchId,\n" +
             "       case\n" +
             "         when isnull(b.status, 0) = 0 then\n" +
             "          'Bagged'\n" +
             "         when b.status = 1 then\n" +
             "          'Unbagged'\n" +
             "       end Status,\n" +
             "       b.SealNo,\n" +
             "       '' Reason, 'EXISTS' BagStatus, lb.unloadingStateID descStatus\n" +
             "  from mnp_unLoading l\n" +
             " inner join MnP_unLoadingBag lb\n" +
             "    on lb.unLoadingId = l.id\n" +
             " inner join Bag b\n" +
             "    on b.bagNumber = lb.bagNumber\n" +
             " inner join Branches b1\n" +
             "    on b1.branchCode = b.origin\n" +
             " inner join Branches b2\n" +
             "    on b2.branchCode = b.destination\n" +
             " where l.refLoadingid = '" + clvar.LoadingID + "'";


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
        public DataTable GetUnloadedCNsByRefNumber(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber cnNo,\n" +
            "       loo.AttributeDesc Description,\n" +
            "       c.weight,\n" +
            "       b1.name OriginBranchId,\n" +
            "       c.orgin OriginID,\n" +
            "       c.Destination DestID,\n" +
            "       b2.name DestBranchId,\n" +
            "       '' Reason,\n" +
            "       'EXISTS' CNSTATUS,\n" +
            "       lc.UnloadingStateID\n" +
            "  from mnp_unLoading l\n" +
            " inner join MnP_unLoadingConsignment lc\n" +
            "    on lc.unLoadingId = l.id\n" +
            " inner join consignment c\n" +
            "    on c.consignmentNumber = lc.consignmentNumber\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = c.orgin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = c.destination\n" +
            "  left outer join rvdbo.Lookup loo\n" +
            "    on loo.Id = lc.UnloadingStateID\n" +
            " where l.RefLoadingID = '" + clvar.LoadingID + "'\n" +
            "   and loo.AttributeGroup = 'RECEIVING_STATUS'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }
        #endregion
        protected void btn_Loading_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "MasterLoading_Unloading.aspx", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }


        public DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name BranchName, b.branchCode\n" +
            "  from branches b\n" +
            " where b.status = '1'\n" +
            " order by 2";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            ViewState["origins"] = dt;

            return dt;
        }
        public static DataTable Cities___()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name BranchName, b.branchCode\n" +
            "  from branches b\n" +
            " where b.status = '1'\n" +
            " order by 2";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }



            return dt;
        }

        public class Branches
        {
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
        }
        [WebMethod]
        public static Branches[] GetBranchesForDropDown()
        {

            List<Branches> branches = new List<Branches>();


            DataTable dt = Cities___();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Branches branch = new Branches();
                        branch.BranchCode = dr["BranchCode"].ToString();
                        branch.BranchName = dr["BranchName"].ToString();
                        branches.Add(branch);
                    }
                }
            }





            return branches.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Branches[] Cities()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name BranchName, b.branchCode\n" +
            "  from branches b\n" +
            " where b.status = '1'\n" +
            " order by 2";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            List<Branches> branches = new List<Branches>();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Branches branch = new Branches();
                        branch.BranchCode = dr["BranchCode"].ToString();
                        branch.BranchName = dr["BranchName"].ToString();
                        branches.Add(branch);
                    }
                }
            }

            return branches.ToArray();
        }

        public DataSet Get_MasterRoute(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select * from rvdbo.MovementRoute MR where \n" +
                             "/*MR.OriginBranchId = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
                             "and*/ MR.ParentMovementRouteId is null and MR.IsActive = '1' order by Name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public DataSet Get_MasterOrign(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT b.branchCode, \n"
                               + "       b.name     BranchName \n"
                               + "FROM   Branches                          b \n"
                               + "where b.status ='1' \n"
                               + " --and b.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n"
                               + "GROUP BY \n"
                               + "       b.branchCode, \n"
                               + "       b.name \n"
                               + "ORDER BY BranchName ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }


        public DataSet Get_MasterDestinationbyRouteId(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT b.branchCode, \n"
                               + "       b.name     BranchName \n"
                               + "FROM   Branches                          b \n"
                               + "where b.[status] ='1' \n"
                               + "and b.branchCode = (select MR.DestBranchId from rvdbo.MovementRoute MR where MR.MovementRouteId = '" + clvar._Route + "')  \n"
                               + "GROUP BY \n"
                               + "       b.branchCode, \n"
                               + "       b.name \n"
                               + "ORDER BY BranchName ASC";


                string sqlString = "SELECT MAX(A.branchCode) branchCode,\n" +
                "       MAX(A.BranchName) BranchName,\n" +
                "       MAX(A.OriginCode) OriginCode,\n" +
                "       MAX(A.OriginName) OriginName\n" +
                "  from (SELECT b.branchCode, b.name BranchName, '' OriginCode, '' OriginName\n" +
                "          FROM Branches b\n" +
                "         where b.status = '1'\n" +
                "           and b.branchCode =\n" +
                "               (select MR.DestBranchId BranchCode\n" +
                "                  from rvdbo.MovementRoute MR\n" +
                "                 where MR.MovementRouteId = '" + clvar._Route + "')\n" +
                "         GROUP BY b.branchCode, b.name\n" +
                "        UNION\n" +
                "\n" +
                "        SELECT '' branchCode,\n" +
                "               '' BranchName,\n" +
                "               b.branchCode OriginCode,\n" +
                "               b.name OriginName\n" +
                "          FROM Branches b\n" +
                "         where b.status = '1'\n" +
                "           and b.branchCode =\n" +
                "               (select MR.OriginBranchId BranchCode\n" +
                "                  from rvdbo.MovementRoute MR\n" +
                "                 where MR.MovementRouteId = '" + clvar._Route + "')\n" +
                "         GROUP BY b.branchCode, b.name) A";


                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public bool IsNumeric(string text)
        {
            char[] arr = text.ToCharArray();
            foreach (char ch in arr)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }

            return true;
        }

        public class DetailsClass //Class for binding data
        {
            public string BagNo { get; set; }
            public string Description { get; set; }
            public string TotalWeight { get; set; }
            public string originId { get; set; }
            public string destid { get; set; }
            public string DestBranchId { get; set; }
            public string originBranchID { get; set; }
            public string status { get; set; }
            public string sealNo { get; set; }
            public string BagStatus { get; set; }
            public string Remarks { get; set; }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetBagNo(string bagNo, string origin, string originName, string destination, string destinationName)
        {
            string data = "";
            List<string> Detail = new List<string>();
            DataTable dt = GetBagByBagNumber(bagNo);
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add(bagNo, originName, destinationName, origin, destination, "", "", "1");
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            string result = DataSetToJSON(ds);


            return result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCnNo(string consignmentno, string origin, string originName, string destination, string destinationName)
        {
            string data = "";
            List<string> Detail = new List<string>();
            clvar.consignmentNo = consignmentno;
            DataTable dt = GetExcessConsignmentForUnload_(clvar);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(consignmentno, "", origin, destination, originName, destinationName, "0.5");
            }
            string result = DataSetToJSON(ds);


            return result;
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Get_LoadingID(string loadingID)
        {
            Cl_Variables clvar_w = new Cl_Variables();
            string data = "";
            List<string> Detail = new List<string>();


            if (loadingID.Trim(' ') != "")
            {
                clvar_w.LoadingID = loadingID;


                DataTable detail = GetLoading_(clvar_w);


                //if (detail != null)
                //{

                //    if (detail.Rows.Count > 0)
                //    {
                //        dd_route.Enabled = false;
                //        dd_orign.Enabled = false;
                //        dd_destination.Enabled = false;
                //        dd_transporttype.Enabled = false;
                //        dd_vehicle.Enabled = false;
                //        Rented.Enabled = false;
                //        Vehicle.Enabled = false;

                //        dd_route.SelectedValue = detail.Rows[0]["RouteID"].ToString();
                //        Route_SelectedIndexChanged(this, e);
                //        dd_destination.SelectedValue = detail.Rows[0]["Destination"].ToString();
                //        ListItem li = dd_orign.Items.FindByValue(detail.Rows[0]["origin"].ToString());
                //        if (li != null)
                //        {
                //            dd_orign.SelectedValue = detail.Rows[0]["Origin"].ToString();
                //        }
                //        else
                //        {
                //            dd_orign.Items.Add(new ListItem { Text = detail.Rows[0]["OriginBranch"].ToString(), Value = detail.Rows[0]["Origin"].ToString() });
                //        }

                //        txt_date.Text = detail.Rows[0]["date"].ToString();
                //        txt_description.Text = detail.Rows[0]["LoadDescription"].ToString();
                //        dd_transporttype.SelectedValue = detail.Rows[0]["TransportationType"].ToString();
                //        dd_vehicle.SelectedValue = detail.Rows[0]["VehicleID"].ToString();
                //        if (dd_vehicle.SelectedValue == "103")
                //        {
                //            txt_reg.Text = detail.Rows[0]["VehicleRegNo"].ToString();
                //            Rented.Checked = true;
                //            vehiclediv1.Visible = false;
                //            vehiclediv2.Visible = false;
                //            regdiv1.Visible = true;
                //            regdiv2.Visible = true;
                //        }
                //        else
                //        {
                //            Vehicle.Checked = true;
                //            vehiclediv1.Visible = true;
                //            vehiclediv2.Visible = true;
                //            regdiv1.Visible = false;
                //            regdiv2.Visible = false;
                //        }

                //        txt_couriername.Text = detail.Rows[0]["CourierName"].ToString();
                //        txt_seal.Text = detail.Rows[0]["SealNo"].ToString();
                //        //dd_route.SelectedValue = detail.Rows[0]["RouteName"].ToString();
                //        //hd_destination.Value = detail.Rows[0]["Destination"].ToString();
                //        //hd_origin.Value = detail.Rows[0]["origin"].ToString();

                //        //gv_bagnew.DataSource = dt;
                //        //gv_bagnew.DataBind();

                //        DataTable dt = new DataTable();//con.GetLoadingBags(clvar);

                //        DataTable dtC = new DataTable();// con.GetConsignmentForUnload(clvar);

                //        if (detail.Rows.Count != 0)
                //        {
                //            if (detail.Rows[0]["isunloaded"].ToString() == "1")
                //            {
                //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already Unloaded. Cannot Unload Again.')", true);
                //                btn_save.Enabled = false;

                //                dt = GetLoadingBags(clvar);
                //                ViewState["bags"] = dt;
                //                dtC = GetConsignmentForUnload(clvar);
                //                ViewState["cns"] = dtC;

                //            }
                //            else
                //            {
                //                btn_save.Enabled = true;
                //                dt = GetLoadingBags(clvar);
                //                ViewState["bags"] = dt;
                //                dtC = GetConsignmentForUnload(clvar);
                //                ViewState["cns"] = dtC;
                //            }
                //        }



                //        if (dtC != null)
                //        {
                //            if (dtC.Rows.Count > 0)
                //            {
                //                gv_cns.DataSource = dtC;
                //                gv_cns.DataBind();
                //                foreach (GridViewRow row_ in gv_cns.Rows)
                //                {

                //                    DataTable origins = ViewState["origins"] as DataTable;
                //                    DropDownList ddGorigin = row_.FindControl("dd_gOrigin") as DropDownList;
                //                    ddGorigin.DataSource = origins;
                //                    ddGorigin.DataTextField = "BranchName";
                //                    ddGorigin.DataValueField = "BranchCode";
                //                    ddGorigin.DataBind();
                //                    ddGorigin.SelectedValue = (row_.FindControl("hd_origin") as HiddenField).Value;
                //                }
                //            }
                //            else
                //            {
                //                gv_cns.DataSource = null;
                //                gv_cns.DataBind();
                //            }
                //        }

                //        if (dt != null)
                //        {
                //            if (dt.Rows.Count > 0)
                //            {
                //                gv_bagnew.DataSource = dt;
                //                gv_bagnew.DataBind();
                //            }
                //            else
                //            {
                //                gv_bagnew.DataSource = null;
                //                gv_bagnew.DataBind();
                //            }
                //        }
                //    }
                //    else
                //    {
                //        //gv_bagnew.DataSource = null;
                //        //gv_bagnew.DataBind();
                //        dd_route.Enabled = true;
                //        //dd_orign.Enabled = true;
                //        //dd_destination.Enabled = true;
                //        dd_transporttype.Enabled = true;
                //        dd_vehicle.Enabled = true;
                //        Rented.Enabled = true;
                //        Vehicle.Enabled = true;
                //    }

                //}
                //else
                //{
                //    dd_route.Enabled = false;
                //    //dd_orign.Enabled = false;
                //    //dd_destination.Enabled = false;
                //    dd_transporttype.Enabled = false;
                //    dd_vehicle.Enabled = false;
                //}
                return data;
            }
            return data;
        }

        public class inputsValuesModel
        {
            public string txt_vid { get; set; }
            public string dd_route { get; set; }
            public string txt_date { get; set; }
            public string dd_transporttype { get; set; }
            public string dd_orign { get; set; }
            public string Vehicle { get; set; }
            public string Rented { get; set; }
            public string dd_destination { get; set; }
            public string dd_vehicle { get; set; }
            public string txt_description { get; set; }
            public string txt_couriername { get; set; }
            public string txt_seal { get; set; }
            public string txt_totalLoadWeight { get; set; }
            public string txt_reg { get; set; }
            public string txt_flight { get; set; }
            public string dept_date { get; set; }
            public string hd_master { get; set; }
        }
        public class BagModel
        {
            public string chk_received { get; set; }
            public string BagNo { get; set; }
            public string Description { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string Status { get; set; }
            public string SealNo { get; set; }
            public string Weight { get; set; }
            public string Remarks { get; set; }
            public string hd_status { get; set; }
            public string hd_origin { get; set; }
            public string hd_descStatus { get; set; }
            public string hd_destination { get; set; }
            public string bagStatus { get; set; }


        }

        public class ConsignmentModel
        {
            public string chk_received { get; set; }
            public string hd_descStatus { get; set; }
            public string hd_origin { get; set; }
            public string hd_destination { get; set; }
            public string hd_cnStatus { get; set; }
            public string ConNumber { get; set; }
            public string Description { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string Weight { get; set; }
            public string Pieces { get; set; }
            public string Remarks { get; set; }


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string save_BAG(inputsValuesModel inputsValues, BagModel Bag)
        {
            string r = "";
            Variable clvar_ = new Variable();
            string txt_vid = inputsValues.txt_vid;
            string dd_route = inputsValues.dd_route;
            string txt_date = inputsValues.txt_date;
            string dd_transporttype = inputsValues.dd_transporttype;
            string dd_orign = inputsValues.dd_orign;
            string Vehicle = inputsValues.Vehicle;
            string Rented = inputsValues.Rented;
            string dd_destination = inputsValues.dd_destination;
            string dd_vehicle = inputsValues.dd_vehicle;
            string txt_description = inputsValues.txt_description;
            string txt_couriername = inputsValues.txt_couriername;
            string txt_seal = inputsValues.txt_seal;
            string txt_totalLoadWeight = inputsValues.txt_totalLoadWeight;
            string txt_reg = inputsValues.txt_reg;
            string txt_flight = inputsValues.txt_flight;
            string dept_date = inputsValues.dept_date;
            string hd_master = inputsValues.hd_master;


            if (Rented.ToUpper() == "TRUE")
            {
                clvar_._VehicleId = "103";
                clvar_.VehicleNo = txt_reg;

            }

            if (Vehicle.ToUpper() == "TRUE")
            {
                clvar_._VehicleId = dd_vehicle;
            }
            if (clvar_.VehicleId.ToString().Trim(' ') == "")
            {
                return "Vehicle";
            }
            if (dd_transporttype == "")
            {
                return "Transport";
            }
            if (dd_transporttype == "197")
            {
                clvar_.FlightNo = txt_flight;
                // clvar.FlightDepartureDate = dept_date.Text;
                clvar_.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + dept_date;
                if (dept_date.Trim(' ') == "__:__")
                {
                    return "Flight";
                }

                // "2013-09-16 12:22:38.833";
            }

            if (dd_transporttype != "197")
            {
                clvar_.FlightDepartureDate = "NULL";
            }

            clvar_._Route = dd_route;
            //clvar_._TouchPoint = dd_touchpoint.SelectedValue;
            clvar_._TransportType = dd_transporttype;
            clvar_._Vehicle = dd_vehicle;
            clvar_._Orign = dd_orign;
            //clvar_._StartDate = dd_start_date.Text;
            clvar_._Destination = dd_destination;
            clvar_._RegNo = txt_reg;
            clvar_._CourierName = txt_couriername;
            clvar_._Description = txt_description;
            clvar_.Seal = txt_seal;





            List<string> normal = new List<string>();
            List<string> shortReceived = new List<string>();
            List<string> ExcessReceived = new List<string>();

            if (Bag.chk_received.ToUpper() == "TRUE")
            {
                if (Bag.Description.Trim().ToUpper() == "RECEIVED")
                {
                    normal.Add(Bag.BagNo);
                }
                else if (Bag.Description.Trim() == "Excess Received")
                {
                    normal.Add(Bag.BagNo);
                }

            }



            clvar.LoadingID = txt_vid;
            clvar.NormalBags = normal;
            clvar.ShortReceivedBags = shortReceived;
            clvar.ExcessBags = ExcessReceived;
            clvar.originId = dd_orign;
            clvar.destId = dd_destination;
            try
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            }
            catch (Exception ex)
            {
                // Response.Redirect("~/login");
            }


            List<string> error = PerformUnloading_BAG(clvar, clvar_, Bag, txt_seal, hd_master);
            if (error[0].ToUpper() == "OK")
            {
                return "Unload Successful.;" + error[1] + "";
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unload UnSuccessful. " + error[1] + "')", true);
                return "Unload UnSuccessful.;" + error[1] + "";
            }
            //}


            return r;
        }


        public static List<string> PerformUnloading_BAG(Cl_Variables clvar, Variable clvar_, BagModel bags, string sealNum, string hd_master)
        {
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataSet ds = func.Branch(clvar);
            string CurrentLocation = HttpContext.Current.Session["LocationName"].ToString();
            string query = "";
            string error = "";
            List<string> error_ = new List<string>();
            string headerQuery = "";
            string bagNumbers = "";
            Int64 unloadingID = 0;
            List<string> cnNumber = new List<string>();
            int count = 0;
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            List<string> queries = new List<string>();
            try
            {
                if (hd_master == "0")
                {
                    //query = "UPDATE MnP_Loading set UnloadedOn = GETDATE(), UnloadedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', UnloadedAt = '" + HttpContext.Current.Session["BranchCode"].ToString() + "', isUnloaded = '1' where ID = '" + clvar.LoadingID + "'";
                    headerQuery = "Insert into MnP_Unloading(RefLoadingID, origin, Destination, BranchCode, CreatedOn, CreatedBy,VehicleID,SealNo, routeID, TransportationType, CourierName, VehicleRegNo,LocationID, \n";
                    if (clvar_._FlightDepartureDate == "NULL")
                    {
                        headerQuery += " Description, FLightNo ) output inserted.ID";
                    }
                    else
                    {
                        headerQuery += "DepartureFlightDate, Description, FLightNo ) output inserted.ID \n";
                    }

                    headerQuery += " Values ('" + clvar.LoadingID + "', '" + clvar.originId + "', '" + clvar.destId + "', '" + HttpContext.Current.Session["BranchCode"].ToString() + "', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "','" + clvar_._VehicleId + "','" + sealNum + "', '" + clvar_._Route + "', '" + clvar_._TransportType + "', '" + clvar_._CourierName + "', '" + clvar_.VehicleNo + "','" + HttpContext.Current.Session["LocationID"].ToString() + "', \n";
                    if (clvar_._FlightDepartureDate == "NULL")
                    {
                        headerQuery += " '" + clvar_._Description + "', '" + clvar_._FlightNo + "')";
                    }
                    else
                    {
                        headerQuery += "'" + clvar_._FlightDepartureDate + "', '" + clvar_._Description + "', '" + clvar_._FlightNo + "')";
                    }

                    //queries.Add(query);
                    sqlcmd.CommandText = headerQuery;

                    object obj = sqlcmd.ExecuteScalar();
                    Int64.TryParse(obj.ToString(), out unloadingID);
                    if (unloadingID == 0)
                    {
                        trans.Rollback();
                        error_.Add("Could Not Save Unload");
                        return error_;
                    }
                }
                else
                {
                    Int64.TryParse(hd_master, out unloadingID);
                }
                string descStatus = bags.hd_descStatus.ToUpper();
                //if (bagStatus.Value.ToUpper() == "INSERT")
                //{
                query = "insert into mnp_unloadingBag (unloadingID, bagNumber, BagDestination, CreatedBy, Createdon, UnloadingStateID, BagWeight, BagRemarks, BagOrigin, BagSeal ) \n" +
                        " SELECT * FROM (select '" + unloadingID + "' unloadingID, '" + bags.BagNo + "' bagNo,'" + bags.hd_destination + "' destination,\n" +
                        "'" + HttpContext.Current.Session["U_ID"].ToString() + "' createdBy ,GETDATE() createdon, '" + bags.hd_status + "' unloadingStateID,'" + bags.Weight + "' bagweight, '" + bags.Remarks + "' bagRemarks,\n" +
                        "'" + bags.hd_origin + "' bagOrigin, '" + sealNum + "' bagSeal ) A where A.bagNo not in (select lb.bagNumber from mnp_UnloadingBag lb where lb.unloadingID = '" + unloadingID + "')";
                queries.Add(query);
                if (bags.hd_status == "5" || bags.hd_status == "7")
                {
                    bagNumbers += "'" + bags.BagNo + "'";
                }

                if (bagNumbers.Trim(' ') != "")
                {
                    bagNumbers = bagNumbers.Replace("''", "','");
                }

                //if (hd_master == "0")
                //{
                string sqlString = "insert into ConsignmentsTrackingHistory\n" +
                "  (consignmentNumber,\n" +
                "   stateID,\n" +
                "   currentLocation,\n" +
                "   transactionTime,\n" +
                "   unloadingNumber, bagNumber,loadingNumber)\n" +
                "  SELECT temp.* FROM (select a.CNNUMBER,\n" +
                "         '5' stateID,\n" +
                "         '" + HttpContext.Current.Session["LocationName"].ToString() + "' CurrentLocation,\n" +
                "         GETDATE() TransactionTime,\n" +
                "         '" + unloadingID + "' UnloadingNumber, a.BagNumber, '" + clvar.LoadingID + "' loadingNumber\n" +
                "    from (select ba.bagNumber, '' ManifestNo, ba.outpieceNumber CNNUMBER\n" +
                "            from BagOutpieceAssociation ba\n" +
                "           where ba.bagNumber in (" + bagNumbers + ")\n" +
                "          union\n" +
                "          select bm.bagNumber,\n" +
                "                 bm.manifestNumber    ManifestNo,\n" +
                "                 cm.consignmentNumber CNNUMBER\n" +
                "            from BagManifest bm\n" +
                "           inner join Mnp_ConsignmentManifest cm\n" +
                "              on bm.manifestNumber = cm.manifestNumber\n" +
                "           where bm.bagNumber in (" + bagNumbers + ")) a\n" +
                "   inner join Branches b\n" +
                "      on b.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "') temp LEFT OUTER JOIN ConsignmentsTrackingHistory cth ON cth.consignmentNumber = temp.CNNUMBER AND cth.UnloadingNumber = '" + unloadingID + "' AND cth.bagNumber IN (" + bagNumbers + ") WHERE cth.id IS NULL\n" +
                "   order by 1, 2, 3";

                if (bagNumbers != "")
                {
                    queries.Add(sqlString);
                }
                //}
                foreach (string query_ in queries)
                {
                    sqlcmd.CommandText = query_;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();
                error_.Add("OK");
                error_.Add(unloadingID.ToString());
            }
            catch (Exception ex)
            {
                trans.Rollback();
                error_.Add("NOT OK");
                error_.Add(ex.Message);
                error = ex.Message;
            }
            return error_;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string save_CN(inputsValuesModel inputsValues, ConsignmentModel CN)
        {
            string r = "";
            Variable clvar_ = new Variable();
            string txt_vid = inputsValues.txt_vid;
            string dd_route = inputsValues.dd_route;
            string txt_date = inputsValues.txt_date;
            string dd_transporttype = inputsValues.dd_transporttype;
            string dd_orign = inputsValues.dd_orign;
            string Vehicle = inputsValues.Vehicle;
            string Rented = inputsValues.Rented;
            string dd_destination = inputsValues.dd_destination;
            string dd_vehicle = inputsValues.dd_vehicle;
            string txt_description = inputsValues.txt_description;
            string txt_couriername = inputsValues.txt_couriername;
            string txt_seal = inputsValues.txt_seal;
            string txt_totalLoadWeight = inputsValues.txt_totalLoadWeight;
            string txt_reg = inputsValues.txt_reg;
            string txt_flight = inputsValues.txt_flight;
            string dept_date = inputsValues.dept_date;
            string hd_master = inputsValues.hd_master;


            if (Rented.ToUpper() == "TRUE")
            {
                clvar_._VehicleId = "103";
                clvar_.VehicleNo = txt_reg;

            }

            if (Vehicle.ToUpper() == "TRUE")
            {
                clvar_._VehicleId = dd_vehicle;
            }
            if (clvar_.VehicleId.ToString().Trim(' ') == "")
            {
                return "Vehicle";
            }
            if (dd_transporttype == "")
            {
                return "Transport";
            }
            if (dd_transporttype == "197")
            {
                clvar_.FlightNo = txt_flight;
                // clvar.FlightDepartureDate = dept_date.Text;
                clvar_.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + dept_date;
                if (dept_date.Trim(' ') == "__:__")
                {
                    return "Flight";
                }

                // "2013-09-16 12:22:38.833";
            }

            if (dd_transporttype != "197")
            {
                clvar_.FlightDepartureDate = "NULL";
            }

            clvar_._Route = dd_route;
            //clvar_._TouchPoint = dd_touchpoint.SelectedValue;
            clvar_._TransportType = dd_transporttype;
            clvar_._Vehicle = dd_vehicle;
            clvar_._Orign = dd_orign;
            //clvar_._StartDate = dd_start_date.Text;
            clvar_._Destination = dd_destination;
            clvar_._RegNo = txt_reg;
            clvar_._CourierName = txt_couriername;
            clvar_._Description = txt_description;
            clvar_.Seal = txt_seal;





            List<string> normal = new List<string>();
            List<string> shortReceived = new List<string>();
            List<string> ExcessReceived = new List<string>();





            clvar.LoadingID = txt_vid;
            clvar.NormalBags = normal;
            clvar.ShortReceivedBags = shortReceived;
            clvar.ExcessBags = ExcessReceived;
            clvar.originId = dd_orign;
            clvar.destId = dd_destination;
            try
            {
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            }
            catch (Exception ex)
            {
                // Response.Redirect("~/login");
            }


            List<string> error = PerformUnloading_CN(clvar, clvar_, CN, txt_seal, hd_master);
            if (error[0].ToUpper() == "OK")
            {
                return "Unload Successful.;" + error[1] + "";
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Unload UnSuccessful. " + error[1] + "')", true);
                return "Unload UnSuccessful.;" + error[1] + "";
            }
            //}


            return r;
        }


        public static List<string> PerformUnloading_CN(Cl_Variables clvar, Variable clvar_, ConsignmentModel CN, string sealNum, string hd_master)
        {
            CommonFunction func = new CommonFunction();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            DataSet ds = func.Branch(clvar);
            string CurrentLocation = ds.Tables[0].Rows[0]["BranchName"].ToString();
            string query = "";
            string error = "";
            List<string> error_ = new List<string>();
            string headerQuery = "";
            string bagNumbers = "";
            Int64 unloadingID = 0;
            List<string> cnNumber = new List<string>();
            int count = 0;
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            List<string> queries = new List<string>();
            try
            {
                if (hd_master == "0")
                {
                    //query = "UPDATE MnP_Loading set UnloadedOn = GETDATE(), UnloadedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', UnloadedAt = '" + HttpContext.Current.Session["BranchCode"].ToString() + "', isUnloaded = '1' where ID = '" + clvar.LoadingID + "'";
                    headerQuery = "Insert into MnP_Unloading(RefLoadingID, origin, Destination, BranchCode, CreatedOn, CreatedBy,VehicleID,SealNo, routeID, TransportationType, CourierName, VehicleRegNo,LocationiD, \n";
                    if (clvar_._FlightDepartureDate == "NULL")
                    {
                        headerQuery += " Description, FLightNo ) output inserted.ID";
                    }
                    else
                    {
                        headerQuery += "DepartureFlightDate, Description, FLightNo ) output inserted.ID \n";
                    }

                    headerQuery += " Values ('" + clvar.LoadingID + "', '" + clvar.originId + "', '" + clvar.destId + "', '" + HttpContext.Current.Session["BranchCode"].ToString() + "', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "','" + clvar_._VehicleId + "','" + sealNum + "', '" + clvar_._Route + "', '" + clvar_._TransportType + "', '" + clvar_._CourierName + "', '" + clvar_.VehicleNo + "','" + HttpContext.Current.Session["LocationID"].ToString() + "', \n";
                    if (clvar_._FlightDepartureDate == "NULL")
                    {
                        headerQuery += " '" + clvar_._Description + "', '" + clvar_._FlightNo + "')";
                    }
                    else
                    {
                        headerQuery += "'" + clvar_._FlightDepartureDate + "', '" + clvar_._Description + "', '" + clvar_._FlightNo + "')";
                    }

                    //queries.Add(query);
                    sqlcmd.CommandText = headerQuery;

                    object obj = sqlcmd.ExecuteScalar();
                    Int64.TryParse(obj.ToString(), out unloadingID);
                    if (unloadingID == 0)
                    {
                        trans.Rollback();
                        error_.Add("Could Not Save Unload");
                        return error_;
                    }
                }
                else
                {
                    Int64.TryParse(hd_master, out unloadingID);
                }

                string hd_descStatus = CN.hd_descStatus;
                string chk = CN.chk_received.ToUpper();
                string cnStatus = CN.hd_cnStatus;
                //  string desc = arr[1].ToUpper();
                string hd_destination = CN.hd_destination;
                string txt_gCnWeight = CN.Weight;
                string txt_gCnPieces = CN.Pieces;
                if (txt_gCnPieces == "")
                {
                    txt_gCnPieces = "1";
                }
                string txt_gCnRemarks = CN.Remarks;
                string dd_gOrigin = CN.Origin;
                clvar.consignmentNo = CN.ConNumber;
                if (cnStatus.ToUpper() == "EXISTS")
                {
                    query = "INSERT INTO MnP_UnLoadingConsignment (UnLoadingID, ConsignmentNumber, CNDestination, CreatedBy, Createdon, UnloadingStateID, cnWeight, cnPieces, cnRemarks, cnOrigin)\n" +
                           "SELECT * FROM (\n" +
                           "SELECT '" + unloadingID + "' unloadingID,\n" +
                           "'" + clvar.consignmentNo + "' CNNO,\n" +
                           "'" + hd_destination + "' DEstination,\n" +
                           "'" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy,\n" +
                           "GETDATE() Createdon, \n" +
                           "'" + hd_descStatus + "' unloadingStateID,\n" +
                           "'" + txt_gCnWeight + "' cnWeight,\n" +
                           "'" + txt_gCnPieces + "' cnPieces,\n" +
                           "'" + txt_gCnRemarks + "' cnRemarks, '" + CN.hd_origin + "' cnOrigin\n" +
                           " ) A where A.CNNO not in (selecT lc.consignmentNumber from mnp_unloadingConsignment lc where lc.unloadingID = '" + unloadingID + "')";
                    queries.Add(query);

                    if (hd_descStatus == "5" || hd_descStatus == "7")
                    {
                        query = "insert into ConsignmentsTrackingHistory (ConsignmentNumber, UnloadingNumber, StateId, CurrentLocation, StatusTime, TransactionTime,loadingNumber)\n" +
                                "SELECT A.cnno, A.UnloadingNumber, A.stateID, b.name, A.StatusTime, A.transactionTime,A.loadingNumber FROM (select '" + clvar.consignmentNo + "' CNNO, '" + unloadingID + "' UnloadingNumber, '5' StateID, GETDATE() STATUSTIME, GETDATE() TRANSACTIONTIME,'" + clvar.LoadingID + "' loadingNumber) A \n" +
                                " INNER JOIN Branches b ON b.branchCode = '" + HttpContext.Current.Session["branchCode"].ToString() + "' left outer join ConsignmentsTrackingHistory cth on A.CNNO = cth.consignmentNumber and A.unloadingNumber = cth.unloadingNumber and cth.stateID = '5' where cth.stateid is null";
                        queries.Add(query);
                    }
                }
                else if (cnStatus.ToUpper() == "NEW CN")
                {
                    query = "insert into Consignment \n" +
                           "  (consignmentNumber, serviceTypeName, riderCode, consignmentTypeId, weight, orgin, destination, createdon, createdby, customerType, pieces, \n" +
                           "   creditClientId, weightUnit, discount, cod, totalAmount, zoneCode, branchCode, gst, consignerAccountNo, bookingDate, isApproved, \n" +
                           "   deliveryStatus, dayType, isPriceComputed, isNormalTariffApplied, receivedFromRider)\n" +
                           "values\n" +
                           "  ( \n" +
                           "   '" + clvar.consignmentNo + "',\n" +
                           "   'overnight',\n" +
                           "   '',\n" +
                           "   '12',\n" +
                           "   '" + txt_gCnWeight + "',\n" +
                           "   '" + dd_gOrigin + "',\n" +
                           "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                           "   GETDATE(),\n" +
                           "   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                           "   '1',\n" +
                           "   '" + txt_gCnPieces + "',\n" +
                           "   '330140',\n" +
                           "   '1',\n" +
                           "   '0',\n" +
                           "   '0',\n" +
                           "   '0',\n" +
                           "   '" + HttpContext.Current.Session["zonecode"].ToString() + "',\n" +
                           "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                           "   '0',\n" +
                           "   '4D1',\n" +
                           "   GETDATE(),\n" +
                           "   '0',\n" +
                           "   '4',\n" +
                           "   '0',\n" +
                           "   '0',\n" +
                           "   '0',\n" +
                           "   '1'\n" +
                           " ) ";
                    queries.Add(query);

                    query = "INSERT INTO MnP_unLoadingConsignment (UnLoadingID, ConsignmentNumber, CNDestination, CreatedBy, Createdon, UnloadingStateID, cnWeight, cnPieces, cnRemarks, cnOrigin)\n" +
                            "SELECT * FROM (\n" +
                           "SELECT '" + unloadingID + "' unloadingID,\n" +
                           "'" + clvar.consignmentNo + "' CNNO,\n" +
                           "'" + hd_destination + "' DEstination,\n" +
                           "'" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy,\n" +
                           "GETDATE() Createdon, \n" +
                           "'" + hd_descStatus + "' unloadingStateID,\n" +
                           "'" + txt_gCnWeight + "' cnWeight,\n" +
                           "'" + txt_gCnPieces + "' cnPieces,\n" +
                           "'" + txt_gCnRemarks + "' cnRemarks, '" + CN.hd_origin + "' cnOrigin\n" +
                           " ) A where A.CNNO not in (selecT lc.consignmentNumber from mnp_unloadingConsignment lc where lc.unloadingID = '" + unloadingID + "')";
                    queries.Add(query);
                    if (hd_descStatus == "5" || hd_descStatus == "7")
                    {
                        query = "insert into ConsignmentsTrackingHistory (ConsignmentNumber, UnloadingNumber, StateId, CurrentLocation, StatusTime, TransactionTime,loadingNumber)\n" +
                                "SELECT A.cnno, A.UnloadingNumber, A.stateID, b.name, A.StatusTime, A.transactionTime FROM (select '" + clvar.consignmentNo + "' CNNO, '" + unloadingID + "' UnloadingNumber, '5' StateID, GETDATE() STATUSTIME, GETDATE() TRANSACTIONTIME,'" + clvar.LoadingID + "' loadingNumber) A \n" +
                                " INNER JOIN Branches b ON b.branchCode = '" + HttpContext.Current.Session["branchCode"].ToString() + "' left outer join ConsignmentsTrackingHistory cth on A.CNNO = cth.consignmentNumber and A.unloadingNumber = cth.unloadingNumber and cth.stateID = '5' where cth.stateid is null";
                        queries.Add(query);
                    }
                }
                else if (cnStatus.ToUpper() == "INSERT")
                {
                    query = "INSERT INTO MnP_unLoadingConsignment (UnLoadingID, ConsignmentNumber, CNDestination, CreatedBy, Createdon, UnloadingStateID, cnWeight, cnPieces, cnRemarks, cnOrigin)\n" +
                            "SELECT * FROM (\n" +
                           "SELECT '" + unloadingID + "' unloadingID,\n" +
                           "'" + clvar.consignmentNo + "' CNNO,\n" +
                           "'" + hd_destination + "' DEstination,\n" +
                           "'" + HttpContext.Current.Session["U_ID"].ToString() + "' CreatedBy,\n" +
                           "GETDATE() Createdon, \n" +
                           "'" + hd_descStatus + "' unloadingStateID,\n" +
                           "'" + txt_gCnWeight + "' cnWeight,\n" +
                           "'" + txt_gCnPieces + "' cnPieces,\n" +
                           "'" + txt_gCnRemarks + "' cnRemarks, '" + CN.hd_origin + "' cnOrigin\n" +
                           " ) A where A.CNNO not in (selecT lc.consignmentNumber from mnp_unloadingConsignment lc where lc.unloadingID = '" + unloadingID + "')";
                    queries.Add(query);
                    if (hd_descStatus == "5" || hd_descStatus == "7")
                    {
                        query = "insert into ConsignmentsTrackingHistory (ConsignmentNumber, UnloadingNumber, StateId, CurrentLocation, StatusTime, TransactionTime,loadingNumber)\n" +
                                "SELECT A.cnno, A.UnloadingNumber, A.stateID, '" + HttpContext.Current.Session["LocationName"].ToString() + "', A.StatusTime, A.transactionTime,A.loadingNumber FROM (select '" + clvar.consignmentNo + "' CNNO, '" + unloadingID + "' UnloadingNumber, '5' StateID, GETDATE() STATUSTIME, GETDATE() TRANSACTIONTIME,'" + clvar.LoadingID + "' loadingNumber) A \n" +
                                " INNER JOIN Branches b ON b.branchCode = '" + HttpContext.Current.Session["branchCode"].ToString() + "' left outer join ConsignmentsTrackingHistory cth on A.CNNO = cth.consignmentNumber and A.unloadingNumber = cth.unloadingNumber and cth.stateID = '5' where cth.stateid is null";

                        queries.Add(query);
                    }
                }
                //else
                //{
                //    query = "UPDATE mnp_loadingConsignment Set unloadingStateID = '" + (cn.FindControl("hd_descStatus") as HiddenField).Value + "' where loadingID = '" + clvar.LoadingID + "' and consignmentNumber = '" + cn.Cells[1].Text + "'";
                //    queries.Add(query);
                //}

                if (hd_descStatus == "5" || hd_descStatus == "7")
                {
                    cnNumber.Add(clvar.consignmentNo);
                }




                foreach (string query_ in queries)
                {
                    sqlcmd.CommandText = query_;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();
                error_.Add("OK");
                error_.Add(unloadingID.ToString());
            }
            catch (Exception ex)
            {
                trans.Rollback();
                error_.Add("NOT OK");
                error_.Add(ex.Message);
                error = ex.Message;
            }
            return error_;
        }


        [WebMethod]
        public static string RemoveBag(string bagNumber, string loadingID)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            con.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = con;
            trans = con.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            string resp = "";
            string command = "delete from mnp_unloadingBag where unloadingID = (select id from mnp_Unloading l where l.refLoadingID = '" + loadingID + "') and bagNumber = '" + bagNumber + "'";
            string trackingCommand = "delete from ConsignmentstrackingHistory where unloadingNumber = (select id from mnp_Unloading l where l.refLoadingID = '" + loadingID + "') and bagNumber = '" + bagNumber + "' and stateID = '5'";

            try
            {

                sqlcmd.CommandText = command;
                int i = sqlcmd.ExecuteNonQuery();
                if (i <= 0)
                {
                    trans.Rollback();
                    con.Close();
                    return "Could not Remove Bag";
                }
                else
                {
                    sqlcmd.CommandText = trackingCommand;
                    sqlcmd.ExecuteNonQuery();
                    trans.Commit();
                    con.Close();
                    return "OK";
                }
            }
            catch (Exception ex)
            {

                trans.Rollback();
                con.Close();
                return ex.Message;
            }
            finally { con.Close(); }

        }

        [WebMethod]
        public static string RemoveCN(string CnNumber, string loadingID)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            con.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = con;
            trans = con.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            string resp = "";
            string command = "delete from mnp_unloadingConsignment where unloadingID = (select id from mnp_Unloading l where l.refLoadingID = '" + loadingID + "') and ConsignmentNumber = '" + CnNumber + "'";
            string trackingCommand = "delete from ConsignmentstrackingHistory where consignmentNumber = '" + CnNumber + "' and unloadingNumber = (select id from mnp_Unloading l where l.refLoadingID = '" + loadingID + "') and stateID = '5'";
            try
            {
                sqlcmd.CommandText = command;
                int i = sqlcmd.ExecuteNonQuery();
                if (i <= 0)
                {
                    trans.Rollback();
                    con.Close();
                    return "Could not Remove CN";
                }
                else
                {
                    sqlcmd.CommandText = trackingCommand;
                    sqlcmd.ExecuteNonQuery();
                    trans.Commit();
                    con.Close();
                    return "OK";
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                con.Close();
                return ex.Message;
            }
            finally { con.Close(); }

        }



        public class ReturnChanged
        {
            public string Status { get; set; }
            public BagModel[] Bags { get; set; }
            public ConsignmentModel[] Consignments { get; set; }
        }
        [WebMethod]
        public static ReturnChanged SaveChangedValues(string unloadingID, BagModel[] Bags, ConsignmentModel[] Consignments)
        {
            ReturnChanged resp = new ReturnChanged();
            resp.Bags = Bags;
            resp.Consignments = Consignments;
            string bagCommand = "";
            string cnCommand = "";
            List<string> queries = new List<string>();
            if (Bags.Count() > 0)
            {
                foreach (BagModel Bag in Bags)
                {
                    bagCommand = "Update mnp_unloadingBag set bagWeight = '" + Bag.Weight + "', bagRemarks = '" + Bag.Remarks + "', BagSeal = '" + Bag.SealNo + "', ModifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', modifiedOn = GETDATE() where BagNumber = '" + Bag.BagNo + "' and unloadingID = '" + unloadingID + "'";
                    queries.Add(bagCommand);
                }
            }

            if (Consignments.Count() > 0)
            {
                foreach (ConsignmentModel Consignment in Consignments)
                {
                    cnCommand = "Update mnp_unloadingConsignment set cnPieces = '" + Consignment.Pieces + "', cnWeight = '" + Consignment.Weight + "', cnRemarks = '" + Consignment.Remarks + "', cnOrigin = '" + Consignment.hd_origin + "' where unloadingId = '" + unloadingID + "' and consignmentNumber = '" + Consignment.ConNumber + "'";
                    queries.Add(cnCommand);
                }
            }

            if (queries.Count == 0)
            {
                resp.Status = "0";
                return resp;
            }
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            con.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = con;
            trans = con.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                resp.Status = "OK";
                foreach (string command in queries)
                {
                    sqlcmd.CommandText = command;
                    int obj = sqlcmd.ExecuteNonQuery();
                    if (obj <= 0)
                    {
                        trans.Rollback();
                        resp.Status = "";
                        break;
                    }
                }
                if (resp.Status == "OK")
                {
                    trans.Commit();
                }

            }
            catch (Exception ex)
            {
                trans.Rollback();
                resp.Status = ex.Message;
            }
            finally { con.Close(); }

            return resp;
        }


        [WebMethod]
        public static string[][] CheckControls(string cn)
        {
            List<string[]> resp = new List<string[]>();
            string[] Response = { "", "" };

            string ConsignmentNo = cn;
            string Reason = "";
            #region Primary Check By Fahad 12-oct-2020
            var rs = PrimaryCheck(cn);
            if (!string.IsNullOrEmpty(rs))
            {
                Response[0] = "false";
                Response[1] = rs;
                resp.Add(Response);
                return resp.ToArray();
            }
            #endregion

            #region RTS/DLV Check by Talha 23-oct-2020
            var rss = alreadyRTS_DLV(cn);
            if (!string.IsNullOrEmpty(rss))
            {
                Response[0] = "false";
                Response[1] = rss;
                resp.Add(Response);
                return resp.ToArray();
            }
            #endregion

            if (cn.StartsWith("5") && cn.Length == 15)
            {
                DataTable BookingDT = CheckConsignmentBooking(ConsignmentNo);
                DataTable FirstProcessDT = CheckFirstProcessOrigin(ConsignmentNo);
                string status = "true";
                if (BookingDT.Rows.Count > 0)
                {
                    if (BookingDT.Rows[0]["bypass"].ToString() == "0")
                    {
                        //if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                        if (BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                        {
                            status = "false";
                            Reason = "Alert: Once reached destination can only move with Return NCI";
                        }
                        else if (FirstProcessDT.Rows.Count > 0 || BookingDT.Rows[0]["isApproved"].ToString() == "True")
                        {
                            if (BookingDT.Rows[0]["status"].ToString() == "9")
                            {
                                status = "false";
                                Reason = "Alert: Consignment is Void perform Arrival";
                            }
                            else
                            {
                                status = "true";
                                Reason = "";
                            }
                        }
                        else
                        {
                            status = "false";
                            Reason = "Alert: First Process Must be at orign";
                        }
                    }
                    Response[0] = status;
                    Response[1] = Reason;
                    resp.Add(Response);
                }
                else
                {
                    Response[0] = "false";
                    Response[1] = "Alert: no booking found for this COD CN";
                    resp.Add(Response);
                }
            }
            else
            {
                Response[0] = "true";
                Response[1] = "";
                resp.Add(Response);
            }
            return resp.ToArray();
        }

        private static DataTable CheckConsignmentBooking(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = @"SELECT *,(select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + Consignment + @"') bypass FROM Consignment c 
            inner join (select consignmentnumber, sum(AtDest) as AtDest, sum(allowRTN) as allowRTN from (
            select '" + Consignment + @"' consignmentnumber, 0 AtDest, 0 allowRTN union
            select consignmentnumber, case when Reason in ('70', '58') then 0 else 1 end AtDest, 0 as allowRTN from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' 
            and createdOn = (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' ) union
            select consignmentnumber, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as allowRTN 
            from MNP_NCI_Request where calltrack = 2 and consignmentnumber ='" + Consignment + @"' group by consignmentnumber) xb
            group by consignmentnumber) xxb on xxb.consignmentNumber = c.consignmentNumber
            WHERE c.consignmentNumber = '" + Consignment + "'";
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
            string query = " select * from consignment where consignmentNumber = '" + Consignment + "' and orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' ";
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

        public static string PrimaryCheck(string cn)
        {
            Cl_Variables clvar = new Cl_Variables();
            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                try
                {
                    var query = $"select 'Consignment already exist in archive database' AS Msg from primaryconsignments where isManual = 1 AND consignmentnumber = '{cn}'";
                    con.Open();
                    var rs = con.QueryFirstOrDefault<string>(query);
                    con.Close();
                    return rs;
                }
                catch (SqlException ex)
                {
                    con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
            }
        }

        public static string alreadyRTS_DLV(string cn)
        {
            Cl_Variables clvar = new Cl_Variables();
            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                try
                {
                    var query = $"select top(1) 'Consignment already Marked Delivered or Returned' AS Msg from Mnp_ConsignmentOperations where ConsignmentId = '{cn}' and (IsDelivered = 1 or IsReturned = 1) ";
                    con.Open();
                    var rs = con.QueryFirstOrDefault<string>(query);
                    con.Close();
                    return rs;
                }
                catch (SqlException ex)
                {
                    con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
            }
        }
    }
}