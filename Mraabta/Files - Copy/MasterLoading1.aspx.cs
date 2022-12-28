using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class MasterLoading1 : System.Web.UI.Page
    {
        private static Variable clvar = new Variable();
        public static bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();
        Cl_Variables cvar = new Cl_Variables();

        LoadingPrintReport cl_lp = new LoadingPrintReport();

        public static List<string> transactionQueires = new List<string>();

        string origin, destination, OrignName, DestinationName, BagNumber, BagId, BagDate, BagWeight, BagSeal, ConsignmentNo, serviceTypeName, consignmentTypeId, weight, pieces, CNtype;
        string CDestinationName, CDestinationId;

        public class MasterModel
        {
            public string Mode { get; set; }
            public string Date { get; set; }
            public string Route { get; set; }
            public string TouchPoint { get; set; }
            public string TransportType { get; set; }
            public string LoadingID { get; set; }
            public string VehicleMode { get; set; }
            public string Destination { get; set; }
            public string VehicleNo { get; set; }
            public string RegNo { get; set; }
            public string FlightNo { get; set; }
            public string FlightDeparture { get; set; }
            public string VehicleType { get; set; }
            public string Description { get; set; }
            public string CourierName { get; set; }
            public string SealNo { get; set; }
            public string TotalWeight { get; set; }
            public string lbl_loadingID { get; set; }
            public string hd_IDChk { get; set; }
            public string hd_U_ID { get; set; }
            public string hd_zoneCode { get; set; }
            public string hd_branchCode { get; set; }
            public string hd_expressCenterCode { get; set; }
            public string hd_LocationName { get; set; }
            public string hd_LocationID { get; set; }

        }

        public class BagModel
        {
            public string BagNo { get; set; }
            public string Weight { get; set; }
            public string Destination { get; set; }
            public string Origin { get; set; }
            public string SealNo { get; set; }
            public string Remarks { get; set; }
            public string SortOrder { get; set; }
            public string OriginName { get; set; }
        }

        public class ConsignmentModel
        {
            public string ConsignmentNumber { get; set; }
            public string ServiceType { get; set; }
            public string ConsignmentType { get; set; }
            public string Destination { get; set; }
            public string Weight { get; set; }
            public string Pieces { get; set; }
            public string Remarks { get; set; }
            public string SortOrder { get; set; }
        }

        public class BagModel_linked
        {
            public string BagNo { get; set; }
            public string Weight { get; set; }
            public string Destination { get; set; }
            public string Origin { get; set; }
            public string SealNo { get; set; }
            public string Remarks { get; set; }
            public string SortOrder { get; set; }
            public string OriginName { get; set; }
        }

        public class ConsignmentModel_linked
        {
            public string ConsignmentNumber { get; set; }
            public string ServiceType { get; set; }
            public string ConsignmentType { get; set; }
            public string Destination { get; set; }
            public string Weight { get; set; }
            public string Pieces { get; set; }
            public string Remarks { get; set; }
            public string SortOrder { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //AltuFaltu();
            error_msg.Text = "";

            if (!IsPostBack)
            {
                if (Session["U_ID"] != null)
                {
                    hd_U_ID.Value = Session["U_ID"].ToString();
                    hd_zoneCode.Value = Session["ZONECODE"].ToString();
                    hd_branchCode.Value = Session["BRANCHCODE"].ToString();
                    hd_expressCenterCode.Value = Session["ExpressCenter"].ToString();
                    hd_LocationName.Value = Session["LocationName"].ToString();
                    hd_LocationID.Value = Session["LocationID"].ToString();

                }
                else
                {
                    Response.Redirect("~/login");
                }


                dd_start_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dd_start_date.Enabled = false;
                txt_vid.Enabled = false;
                Get_Destination();
                Get_Orign();
                Get_MasterRoute();
                Get_MasterTransportType();
                Get_MasterVehicle();

                DataTable dt_head = new DataTable();
                dt_head.Columns.Add("bagNumber", typeof(string));
                dt_head.Columns.Add("weight", typeof(string));
                dt_head.Columns.Add("origin", typeof(string));
                dt_head.Columns.Add("destination", typeof(string));
                dt_head.Columns.Add("OrignName", typeof(string));
                dt_head.Columns.Add("DestinationName", typeof(string));
                dt_head.Columns.Add("sealno", typeof(string));
                dt_head.Columns.Add("Remarks", typeof(string));
                dt_head.Columns.Add("isNew", typeof(string));
                ViewState["handleBag"] = dt_head;


                DataTable dt_head2 = new DataTable();
                dt_head2.Columns.Add("consignmentno", typeof(string));
                dt_head2.Columns.Add("serviceTypeName", typeof(string));
                dt_head2.Columns.Add("consignmentTypeId", typeof(string));
                dt_head2.Columns.Add("weight", typeof(string));
                dt_head2.Columns.Add("DestinationName", typeof(string));
                dt_head2.Columns.Add("DestinationId", typeof(string));
                dt_head2.Columns.Add("pieces", typeof(string));
                dt_head2.Columns.Add("CNTYPE", typeof(string));
                dt_head2.Columns.Add("Remarks", typeof(string));
                dt_head2.Columns.Add("isNew", typeof(string));
                ViewState["handleConsignment"] = dt_head2;
                GetCNLengths();
                GetVehicleType();
                Session["Loading_check"] = "1";


                DateTime dateNow = DateTime.Now;
                //int newID = dateNow.Year + dateNow.Month + dateNow.Day + dateNow.Hour + dateNow.Minute + dateNow.Second + dateNow.Millisecond;
                //txt_vid.Text = DateTime.Now.Year.ToString().Substring(2, 2) + HttpContext.Current.Session["BranchCode"].ToString() + HttpContext.Current.Session["U_ID"].ToString() + newID;

                string LoadingIDLogic = DateTime.Now.Year.ToString().Substring(2, 2) + dateNow.Month.ToString("d2") + dateNow.Day.ToString("d2") + dateNow.Hour.ToString("D2") + dateNow.Minute.ToString("D2") + dateNow.Second.ToString("D2") + dateNow.Millisecond.ToString("D3");
                txt_vid.Text = LoadingIDLogic;

                if (rbtn_mode.SelectedValue.ToUpper() == "NEW")
                {
                    //hd_loadingID.Value = txt_vid.Text; 
                    hd_loadingID.Text = txt_vid.Text;
                    hd_IDChk.Value = "0";
                }
                else
                {
                    hd_loadingID.Text = "";
                    hd_IDChk.Value = "1";
                }

            }
        }

        private bool chkLoadingIDDuplication__OLD(string LoadingIDLogic)
        {
            bool chk = true;
            string query = "SELECT * FROM MnP_Loading mpl WHERE mpl.id ='" + LoadingIDLogic + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    chk = false;
                }
                else
                {
                    chk = true;
                }

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return chk;
        }

        private static bool chkLoadingIDDuplication(string LoadingIDLogic)
        {
            bool chk = true;
            string query = "SELECT * FROM MnP_Loading mpl WHERE mpl.id ='" + LoadingIDLogic + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    chk = false;
                }
                else
                {
                    chk = true;
                }

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return chk;
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
            DataSet ds_orign = b_fun.Get_MasterOrign(clvar);

            if (ds_orign.Tables[0].Rows.Count != 0)
            {
                dd_orign.DataTextField = "BranchName";
                dd_orign.DataValueField = "branchCode";
                dd_orign.DataSource = ds_orign.Tables[0].DefaultView;
                dd_orign.DataBind();
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

        public void AltuFaltu()
        {
            DataTable tempdt = ViewState["handleConsignment"] as DataTable;
            bool invalidOutPieceWeight = false;
            //foreach (GridViewRow row in GridView2.Rows)
            //{
            //    string cn = row.Cells[1].Text;
            //    TextBox weight = (row.FindControl("txt_gWeight") as TextBox);
            //    float fWeight = 0;
            //    float.TryParse(weight.Text, out fWeight);
            //    if (fWeight <= 0)
            //    {
            //        invalidOutPieceWeight = true;
            //        weight.BackColor = System.Drawing.Color.LightPink;
            //    }
            //    else
            //    {
            //        weight.BackColor = System.Drawing.Color.FromName("#919B9C");
            //    }
            //    DataRow dr = tempdt.Select("consignmentno = '" + cn + "'")[0];
            //    dr["pieces"] = (row.FindControl("txt_gPieces") as TextBox).Text;
            //    dr["weight"] = (row.FindControl("txt_gWeight") as TextBox).Text;
            //    dr["destinationID"] = (row.FindControl("dd_outpieceGDestination") as DropDownList).SelectedValue;
            //    dr["remarks"] = (row.FindControl("txt_gOutPieceRemarks") as TextBox).Text;
            //}

            if (invalidOutPieceWeight)
            {
                AlertMessage("Invalid Weight in highlighted Consignments.", "Red");
            }
            ViewState["handleConsignment"] = tempdt;



            DataTable tempBags = ViewState["handleBag"] as DataTable;
            bool invalidBagWeight = false;
            //foreach (GridViewRow row in gv_bags.Rows)
            //{
            //    string bag = row.Cells[1].Text;
            //    TextBox weight = (row.FindControl("txt_gWeight") as TextBox);
            //    float fWeight = 0;
            //    float.TryParse(weight.Text, out fWeight);
            //    if (fWeight <= 0)
            //    {
            //        invalidBagWeight = true;
            //        weight.BackColor = System.Drawing.Color.LightPink;
            //    }
            //    else
            //    {
            //        weight.BackColor = System.Drawing.Color.FromName("#919B9C");
            //    }
            //    DataRow[] dr = tempBags.Select("bagNumber = '" + bag + "'");
            //    if (dr.Count() > 0)
            //    {
            //        dr[0]["weight"] = (row.FindControl("txt_gWeight") as TextBox).Text;
            //        dr[0]["destination"] = (row.FindControl("dd_bagGDestination") as DropDownList).SelectedValue;
            //        dr[0]["Origin"] = (row.FindControl("dd_bagGOrigin") as DropDownList).SelectedValue;
            //        dr[0]["sealno"] = (row.FindControl("txt_gSeal") as TextBox).Text;
            //        dr[0]["Remarks"] = (row.FindControl("txt_gBagRemarks") as TextBox).Text;
            //    }
            //}

            if (invalidBagWeight)
            {
                AlertMessage("Invalid Weight in highlighted Bags.", "Red");
            }
        }

        public void Get_Destination()
        {
            //DataSet ds_destination =  b_fun.Get_MasterDestination(clvar);
            DataTable ds_destination = Cities_();// b_fun.Get_MasterDestination(clvar);
            if (ds_destination.Rows.Count != 0)
            {
                //dd_destination.DataTextField = "BranchName";
                //dd_destination.DataValueField = "branchCode";
                //dd_destination.DataSource = ds_destination.Tables[0].DefaultView;
                //dd_destination.DataBind();
                ViewState["destinations"] = ds_destination;

            }
        }

        public void Get_MasterRoute()
        {
            DataSet ds_route = b_fun.Get_MasterRoute(clvar);
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

        public void Get_MasterTransportType()
        {
            DataSet ds_transporttype = cl_lp.Get_MasterTransportType(clvar);

            if (ds_transporttype.Tables[0].Rows.Count != 0)
            {
                dd_transporttype.DataTextField = "AttributeDesc";
                dd_transporttype.DataValueField = "id";
                dd_transporttype.DataSource = ds_transporttype.Tables[0].DefaultView;
                dd_transporttype.DataBind();

                dd_transporttype.SelectedValue = "27";
            }
            dd_transporttype.Items.Insert(0, new ListItem("Select Transport Type ", ""));
        }

        public void Get_MasterVehicle()
        {
            DataSet ds_vehicle = Get_MasterVehicle(clvar);

            if (ds_vehicle.Tables[0].Rows.Count != 0)
            {
                dd_vehicle.DataTextField = "MakeModel";
                dd_vehicle.DataValueField = "VehicleCode";
                dd_vehicle.DataSource = ds_vehicle.Tables[0].DefaultView;
                dd_vehicle.DataBind();
            }
            dd_vehicle.Items.Insert(0, new ListItem("Select Vehicle ", ""));
        }

        protected void ResetAll()
        {
            txt_bagno.Text = "";
            txt_consignmentno.Text = "";
            txt_couriername.Text = "";
            txt_description.Text = "";
            txt_flight.Text = "";
            txt_reg.Text = "";
            txt_seal.Text = "";
            txt_totalLoadWeight.Text = "";
            txt_vid.Text = "";
            dd_destination.ClearSelection();
            dd_orign.ClearSelection();
            dd_route.ClearSelection();
            dd_start_date.Text = DateTime.Now.ToString("yyyy-MM-dd"); ;
            dd_touchpoint.ClearSelection();
            dd_transporttype.ClearSelection();
            dd_vehicle.ClearSelection();
            //gv_bags.DataSource = null;
            //GridView2.DataSource = null;
            ////gv_bags.DataBind();
            //GridView2.DataBind();

            Get_Destination();
            //flight.Visible = false;
            Get_Orign();
            //    Get_Destination();
            Get_MasterRoute();
            Get_MasterTransportType();
            Get_MasterVehicle();

            DataTable dt_head = new DataTable();
            //dt_head.Columns.Add("BagId", typeof(string));
            dt_head.Columns.Add("bagNumber", typeof(string));
            dt_head.Columns.Add("weight", typeof(string));
            dt_head.Columns.Add("origin", typeof(string));
            dt_head.Columns.Add("destination", typeof(string));
            dt_head.Columns.Add("OrignName", typeof(string));
            dt_head.Columns.Add("DestinationName", typeof(string));
            dt_head.Columns.Add("sealno", typeof(string));
            dt_head.Columns.Add("Remarks", typeof(string));
            dt_head.Columns.Add("isNew", typeof(string));
            ViewState["handleBag"] = dt_head;


            DataTable dt_head2 = new DataTable();
            dt_head2.Columns.Add("consignmentno", typeof(string));
            dt_head2.Columns.Add("serviceTypeName", typeof(string));
            dt_head2.Columns.Add("consignmentTypeId", typeof(string));
            dt_head2.Columns.Add("weight", typeof(string));
            dt_head2.Columns.Add("DestinationName", typeof(string));
            dt_head2.Columns.Add("DestinationId", typeof(string));
            dt_head2.Columns.Add("pieces", typeof(string));
            dt_head2.Columns.Add("CNTYPE", typeof(string));
            dt_head2.Columns.Add("Remarks", typeof(string));
            dt_head2.Columns.Add("isNew", typeof(string));
            ViewState["handleConsignment"] = dt_head2;
        }


        protected void dd_transporttype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (dd_transporttype.SelectedValue == "197")
            {
                //flight.Visible = true;
            }
            else
            {
                //flight.Visible = false;
            }

        }


        public DataSet Get_MasterVehicle(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select v.*, isnull(v.vehicleType, 0) VehicleType_ from rvdbo.Vehicle v where v.IsActive = '1' order by 1";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dd_vehicle.Items.Clear();
                    dd_vehicle.Items.Add(new ListItem { Text = "Select Vehicle", Value = "0" });
                    dd_vehicle.DataSource = ds.Tables[0];
                    dd_vehicle.DataTextField = "MakeModel";
                    dd_vehicle.DataValueField = "VehicleCode";
                    dd_vehicle.DataBind();

                    vehicleTypes.DataSource = ds.Tables[0];
                    vehicleTypes.DataBind();
                }
                else
                {
                    dd_vehicle.Items.Clear();
                    dd_vehicle.Items.Add(new ListItem { Text = "Select Vehicle", Value = "0" });
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Vehicle Available')", true);
                    error_msg.Text = "No Vehicle Avaiable";

                }
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            error_msg.Text = message;
            error_msg.ForeColor = System.Drawing.Color.FromName(color);

        }

        public class Branches
        {

            public string BranchCode { get; set; }
            public string BranchName { get; set; }
        }
        public class DropDownClass
        {
            public string id { get; set; }
            public string Value { get; set; }
            public string Text { get; set; }
        }

        [WebMethod]
        public static Branches[] GetBranchesForDropDown()
        {

            List<Branches> branches = new List<Branches>();


            DataTable dt = Cities_();

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
        public static DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name BRANCHNAME, b.branchCode\n" +
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

        [WebMethod]
        public static DropDownClass[] RouteChange(string route)
        {
            List<DropDownClass> dds = new List<DropDownClass>();

            clvar._Route = route;

            DataSet ds_touchpoint = b_fun.Get_RouteByRouteId(clvar);
            if (ds_touchpoint.Tables[0] != null)
            {
                if (ds_touchpoint.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_touchpoint.Tables[0].Rows)
                    {
                        DropDownClass dd = new DropDownClass();
                        dd.id = "1";
                        dd.Text = dr["Name"].ToString();
                        dd.Value = dr["MovementRouteId"].ToString();
                        dds.Add(dd);
                    }
                }
                else
                {
                    DropDownClass dd = new DropDownClass();
                    dd.id = "1";
                    dd.Value = "-1";
                    dd.Text = "Touch Point Not Found";
                    dds.Add(dd);
                }
            }
            else
            {
                DropDownClass dd = new DropDownClass();
                dd.id = "1";
                dd.Value = "-1";
                dd.Text = "Touch Point Not Found";
                dds.Add(dd);
            }



            DataSet ds_destination = b_fun.Get_MasterDestinationbyRouteId(clvar);

            if (ds_destination.Tables[0] != null)
            {
                if (ds_destination.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_destination.Tables[0].Rows)
                    {
                        DropDownClass dd = new DropDownClass();
                        dd.id = "2";
                        dd.Text = dr["BranchName"].ToString();
                        dd.Value = dr["branchCode"].ToString();
                        dds.Add(dd);
                    }
                }
                else
                {
                    DropDownClass dd = new DropDownClass();
                    dd.id = "2";
                    dd.Value = "-1";
                    dd.Text = "Destination Not Found";
                    dds.Add(dd);
                }
            }
            else
            {
                DropDownClass dd = new DropDownClass();
                dd.id = "2";
                dd.Value = "-1";
                dd.Text = "Destination Not Found";
                dds.Add(dd);
            }


            return dds.ToArray();
        }

        [WebMethod]
        public static DropDownClass[] TransportTypeChange(string transportType)
        {
            List<DropDownClass> dds = new List<DropDownClass>();


            return dds.ToArray();
        }

        [WebMethod]
        public static string[] InsertLoading(MasterModel Master, BagModel[] Bags, ConsignmentModel[] Consignments)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertLoading", "0", "InsertLoading Method Initialized", "InsertLoading"));
            string[] resp_ = { "0", "" };
            string RandomNumber = HttpContext.Current.Session["U_ID"].ToString() + DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace("/", "");

            DataTable tblBags = new DataTable();
            tblBags.Columns.AddRange(new DataColumn[]{
            new DataColumn("loadingId", typeof(Int64)) ,
            new DataColumn("bagNumber", typeof(string)) ,
            new DataColumn("BagDestination", typeof(string)) ,
            new DataColumn("UloadingStateID", typeof(string)) ,
            new DataColumn("Remarks", typeof(string)) ,
            new DataColumn("BagWeight", typeof(string)) ,
            new DataColumn("BagOrigin", typeof(string)) ,
            new DataColumn("BagSeal", typeof(string)),
            new DataColumn("UniqueNumber", typeof(string)),

            new DataColumn("SortOrder", typeof(int))
        });

            DataTable tblCn = new DataTable();
            tblCn.Columns.AddRange(new DataColumn[] {
            new DataColumn("loadingId", typeof(Int64)) ,
            new DataColumn("consignmentNumber", typeof(string)) ,
            new DataColumn("CNDestination", typeof(string)) ,
            new DataColumn("UnloadingStateID", typeof(string)) ,
            new DataColumn("Remarks", typeof(string)) ,
            new DataColumn("cnPieces", typeof(int)) ,
            new DataColumn("CNWeight", typeof(string)) ,
            new DataColumn("ServiceType", typeof(string)) ,
            new DataColumn("ConsignmentType", typeof(int)) ,
            new DataColumn("UniqueNumber", typeof(string)),

            new DataColumn("SortOrder", typeof(int))

        });
            int sortOrder = 1;
            foreach (BagModel bag in Bags)
            {
                DataRow dr = tblBags.NewRow();

                dr["LoadingID"] = Master.lbl_loadingID;
                dr["bagNumber"] = bag.BagNo.Trim();
                dr["BagDestination"] = bag.Destination;
                dr["UloadingStateID"] = "";
                dr["Remarks"] = bag.Remarks;
                dr["BagWeight"] = bag.Weight;
                dr["BagOrigin"] = HttpContext.Current.Session["BranchCode"].ToString();
                dr["BagSeal"] = bag.SealNo;
                dr["UniqueNumber"] = RandomNumber;

                dr["SortOrder"] = sortOrder;
                sortOrder++;
                tblBags.Rows.Add(dr);
            }
            sortOrder = 1;

            foreach (ConsignmentModel cn in Consignments)
            {
                DataRow dr = tblCn.NewRow();
                dr["loadingId"] = Master.lbl_loadingID;
                dr["consignmentNumber"] = cn.ConsignmentNumber.Trim();
                dr["CNDestination"] = cn.Destination;
                dr["UnloadingStateID"] = "";
                dr["Remarks"] = cn.Remarks;
                int cnPieces = 0;
                int.TryParse(cn.Pieces, out cnPieces);
                cn.Pieces = cnPieces.ToString();
                dr["cnPieces"] = int.Parse(cn.Pieces.ToString());
                dr["CNWeight"] = cn.Weight;
                dr["ServiceType"] = cn.ServiceType;
                dr["ConsignmentType"] = 12;
                dr["UniqueNumber"] = RandomNumber;

                dr["SortOrder"] = sortOrder;
                sortOrder++;
                tblCn.Rows.Add(dr);
            }


            #region Variables
            if (Master.VehicleMode.ToUpper() == "R")//Rented.Checked)
            {
                clvar._VehicleId = "103";
                clvar.VehicleNo = Master.RegNo;

            }

            if (Master.VehicleMode.ToUpper() == "V")// Vehicle.Checked)
            {
                clvar._VehicleId = Master.VehicleNo;// dd_vehicle.SelectedValue;
            }

            if (Master.TransportType == "197")//dd_transporttype.SelectedValue == "197")
            {
                clvar.FlightNo = Master.FlightNo;// txt_flight.Text;
                                                 // clvar.FlightDepartureDate = dept_date.Text;
                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
                //if (dept_date.Text.Trim(' ') == "__:__")
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Flight Departure time')", true);
                //    error_msg.Text = "Provide Flight Departure time";
                //    return;
                //}

                // "2013-09-16 12:22:38.833";
            }

            if (Master.TransportType != "197")//dd_transporttype.SelectedValue != "197")
            {
                clvar.FlightDepartureDate = "NULL";
            }

            clvar._Route = Master.Route;// dd_route.SelectedValue;
            clvar._TouchPoint = Master.TouchPoint;// dd_touchpoint.SelectedValue;
            clvar._TransportType = Master.TransportType;// dd_transporttype.SelectedValue;
            clvar._Vehicle = Master.VehicleNo;// dd_vehicle.SelectedValue;
            clvar._Orign = HttpContext.Current.Session["BranchCode"].ToString();// dd_orign.SelectedValue;
            clvar._StartDate = Master.Date;// dd_start_date.Text;
            clvar._Destination = Master.Destination;// dd_destination.SelectedValue;
            clvar._RegNo = Master.RegNo;// txt_reg.Text;
            clvar._CourierName = Master.CourierName;// txt_couriername.Text;
            clvar._Description = Master.Description;// txt_description.Text;
            clvar.Seal = Master.SealNo;// txt_seal.Text;
            clvar._StateId = "4"; // LOADED STATE ID
            clvar.Type = Master.VehicleType;// dd_vehicleType.SelectedValue;
            clvar.LoadingId = Master.lbl_loadingID;
            clvar.Remarks = RandomNumber;
            clvar.Weight = Master.TotalWeight;
            string mode = Master.VehicleMode;

            #endregion

            //if (Master.LoadingID != Master.lbl_loadingID)
            //{
            //    resp_[0] = "0";
            //    resp_[1] = "Merging Error: On Update/Save MNP Bag LoadingID is Different " + clvar.LoadingId + "!=" + Master.lbl_loadingID + ". Kindly Refresh Page and then use the same loading ID again through UPDATE tab or contact IT for assistance. ThankYou.";
            //    return resp_;
            //}


            if (Master.Mode.ToUpper() == "NEW")
            {
                if (Master.lbl_loadingID == "")
                {

                }
                else
                {
                    Tuple<bool, string> resp = UpdateLoading(Master, tblBags, tblCn, mode, RandomNumber, "InsertLoading_linked");
                    if (resp.Item1)
                    {
                        resp_[0] = "1";
                        resp_[1] = resp.Item2.ToString();
                        Insert_ConsignmentTrackingHistoryFromLoading_consignment_(Master, Consignments);
                        //Insert_ConsignmentTrackingHistoryFromLoading_consignment(Master, Consignments);
                        Insert_ConsignmentTrackingHistoryFromLoading_bag_(Master, Bags);
                        //Insert_ConsignmentTrackingHistoryFromLoading_bag(Master, Bags);
                    }
                    else
                    {
                        resp_[0] = "0";
                        resp_[1] = resp.Item2.ToString();
                    }
                }

            }
            else if (Master.Mode.ToUpper() == "UPDATE")
            {
                Tuple<bool, string> resp = UpdateLoading(Master, tblBags, tblCn, mode, RandomNumber, "InsertLoading_linked");
                if (resp.Item1)
                {
                    resp_[0] = "1";
                    resp_[1] = resp.Item2.ToString();
                    Insert_ConsignmentTrackingHistoryFromLoading_consignment_(Master, Consignments);
                    //Insert_ConsignmentTrackingHistoryFromLoading_consignment(Master, Consignments);
                    Insert_ConsignmentTrackingHistoryFromLoading_bag_(Master, Bags);


                }
                else
                {
                    resp_[0] = "0";
                    resp_[1] = resp.Item2.ToString();
                }
            }

            Transactionol(transactionQueires);
            transactionQueires.Clear();
            return resp_;

        }

        [WebMethod]
        public static bool GetData(string id, string route)
        {
            return CheckLocation(id, route);
        }

        public static bool CheckLocation(string BranchID, string route)
        {
            DataSet ds = new DataSet();

            string sql = "select * from RunnerRoutes_Child where MovementRouteCode = '" + route + "' and Child_branchCode = '" + BranchID + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    ds = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { con.Close(); }
        }
        private static void Insert_ConsignmentTrackingHistoryFromLoading_bag_(MasterModel Master, BagModel[] Bags)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_bag_", "0", "Insert_ConsignmentTrackingHistoryFromLoading_bag_ Method Initialized", "InsertLoading_linked"));
            foreach (BagModel cn1 in Bags)
            {
                string sql = "/************************************************************ \n"
               + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
               + " * Time: 11/13/2018 4:28:12 PM \n"
               + " ************************************************************/ \n"
               + " \n"
               + "MERGE ConsignmentsTrackingHistory AS TARGET  \n"
               + "                               USING ( \n"
               + "                                         SELECT a.CNNUMBER consignmentNumber, \n"
               + "                                                '4' stateID, \n"
               + "         '" + Master.hd_LocationName + "' CurrentLocation,\n"
               + "                                                GETDATE() TransactionTime, \n"
               + "                                                '" + Master.lbl_loadingID + "' loadingNumber, a.bagNumber \n"
               + "                                         FROM   ( \n"
               + "                                                    SELECT ba.bagNumber, \n"
               + "                                                           '' ManifestNo, \n"
               + "                                                           ba.outpieceNumber  \n"
               + "                                                           CNNUMBER \n"
               + "                                                    FROM    \n"
               + "                                                           BagOutpieceAssociation  \n"
               + "                                                           ba \n"
               + "                                                    WHERE  ba.bagNumber IN ('" + cn1.BagNo.Trim() + "')\n"
               + "                                                    UNION \n"
               + "                                                    SELECT bm.bagNumber, \n"
               + "                                                           bm.manifestNumber  \n"
               + "                                                           ManifestNo, \n"
               + "                                                           cm.consignmentNumber  \n"
               + "                                                           CNNUMBER \n"
               + "                                                    FROM   BagManifest bm \n"
               + "                                                           INNER JOIN  \n"
               + "                                                                Mnp_ConsignmentManifest  \n"
               + "                                                                cm \n"
               + "                                                                ON  bm.manifestNumber =  \n"
               + "                                                                    cm.manifestNumber \n"
               + "                                                    WHERE  bm.bagNumber IN ('" + cn1.BagNo.Trim() + "') \n"
               + "                                                ) a \n"
               + "                                                INNER JOIN Branches b \n"
               + "                                                     ON  b.branchCode = '" + Master.hd_branchCode + "' \n"
               + "                                     ) AS SOURCE  \n"
               + "                                ON ( \n"
               + "                                       TARGET.consignmentNumber = SOURCE.consignmentNumber \n"
               + "                                       AND source.loadingNumber = TARGET.loadingnumber \n"
               + "                                   ) \n"
               + "                                WHEN NOT MATCHED BY TARGET THEN   \n"
               + "INSERT  \n"
               + "  ( \n"
               + "    loadingNumber, \n"
               + "    CurrentLocation, \n"
               + "    consignmentNumber,bagNumber ,\n"
               + "    stateID, \n"
               + "    transactionTime \n"
               + "  ) \n"
               + "VALUES \n"
               + "  ( \n"
               + "    source.loadingNumber, \n"
               + "    '" + Master.hd_LocationName + "', \n"
               + "    SOURCE.consignmentNumber,SOURCE.bagNumber,\n"
               + "    '4', \n"
               + "    GETDATE() \n"
               + "  ); \n"
               + "";



                SqlConnection con = new SqlConnection(clvar.Strcon());
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_bag_", "0", "Error:" + ex.Message.Trim('\''), "InsertLoading_linked"));
                    CommonFunction ComFunction = new CommonFunction();
                    ComFunction.InsertErrorLog("", "", "", "", clvar._LoadingId.ToString(), "", "LOADING", ex.Message);
                }
                con.Close();

                break;
            }
        }

        private static void Insert_ConsignmentTrackingHistoryFromLoading_consignment_(MasterModel Master, ConsignmentModel[] Consignments)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_consignment_", "0", "Insert_ConsignmentTrackingHistoryFromLoading_consignment_ Method Initialized", "InsertLoading_linked"));
            foreach (ConsignmentModel cn1 in Consignments)
            {

                //string sqlString = "insert into ConsignmentsTrackingHistory\n" +
                // "  (loadingNumber, CurrentLocation, consignmentNumber, stateID, transactionTime)\n" +
                // "  select '" + Master.LoadingID + "', '" + HttpContext.Current.Session["LocationName"].ToString() + "', lc.consignmentNumber CN, '4', GETDATE()\n" +
                // "    from MnP_LoadingConsignment lc\n" +
                // "   where lc.loadingId = '" + Master.LoadingID + "'\n" +
                // "    and lc.consignmentNumber ='" + cn1.ConsignmentNumber + "'\n";

                string sqlString = "MERGE ConsignmentsTrackingHistory AS TARGET \n" +
                                    "USING (  select loadingId,consignmentNumber " +
                                    "                 from MnP_LoadingConsignment lc \n" +
                                    "                where lc.loadingId = '" + Master.lbl_loadingID + "'\n" +
                                    "                 and lc.consignmentNumber ='" + cn1.ConsignmentNumber.Trim() + "'\n" +
                                    " ) AS SOURCE \n" +
                                    " ON (TARGET.consignmentNumber = SOURCE.consignmentNumber and source.loadingID = Target.loadingnumber)\n" +
                                    " WHEN NOT MATCHED BY TARGET THEN  \n" +
                                    " INSERT(loadingNumber, CurrentLocation, consignmentNumber, stateID, transactionTime)\n" +
                                    " values (source.loadingID, '" + Master.hd_LocationName + "', SOURCE.consignmentNumber , '4', GETDATE() );";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sqlString, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_consignment_", "0", "Error:" + ex.Message.Trim('\''), "InsertLoading_linked"));
                    CommonFunction ComFunction = new CommonFunction();
                    ComFunction.InsertErrorLog("", "", "", "", clvar._LoadingId.ToString(), "", "LOADING", ex.Message);
                }
                con.Close();
            }

        }


        public static Tuple<bool, string> InsertLoadingData(Variable clvar, DataTable Bags, DataTable Cns)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            SqlTransaction trans_;
            trans_ = con.BeginTransaction();
            cmd.Transaction = trans_;
            try
            {



                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_GenerateLoading_New";
                cmd.Parameters.AddWithValue("@tblCN", Cns);
                cmd.Parameters.AddWithValue("@tblBag", Bags);
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@ExpressCenterCode", HttpContext.Current.Session["ExpressCenter"].ToString());
                cmd.Parameters.AddWithValue("@transportationType", clvar.TransportType);
                cmd.Parameters.AddWithValue("@routeId", clvar.Route);
                cmd.Parameters.AddWithValue("@VehicleType", clvar.Type);
                cmd.Parameters.AddWithValue("@vehicleId", clvar.VehicleId);
                cmd.Parameters.AddWithValue("@description", clvar.Description);
                cmd.Parameters.AddWithValue("@courierName", clvar.CourierName);
                cmd.Parameters.AddWithValue("@origin", clvar.Orign);
                cmd.Parameters.AddWithValue("@destination", clvar.Destination);
                HttpContext.Current.Session["Dest_branch"] = clvar.Destination;
                cmd.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@sealNo", clvar.Seal);
                cmd.Parameters.AddWithValue("@UniqueNumber", clvar.Remarks);

                if (clvar.TransportType == "197")
                {
                    cmd.Parameters.AddWithValue("@FlightNo", clvar.FlightNo);
                    cmd.Parameters.AddWithValue("@DepartureFlightDate", clvar.FlightDepartureDate);
                }

                cmd.Parameters.AddWithValue("@VehicleRegNo", clvar.RegNo);

                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result2", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@LoadingID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                Int64 loadingID = 0;
                if (cmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                {
                    object obj = cmd.Parameters["@LoadingID"].SqlValue;
                    Int64.TryParse(obj.ToString(), out loadingID);
                    resp = new Tuple<bool, string>(true, loadingID.ToString());
                }
                else
                {
                    resp = new Tuple<bool, string>(false, cmd.Parameters["@result"].SqlValue.ToString().ToUpper());
                }
                trans_.Commit();

            }
            catch (Exception ex)
            {
                trans_.Rollback();
                resp = new Tuple<bool, string>(false, ex.Message);

            }
            finally { con.Close(); }
            return resp;

        }

        public static Tuple<bool, string> UpdateLoading(MasterModel Master, DataTable Bags, DataTable Cns, string mode, string RandomNumber, string CN_Bag)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading", "0", "UpdateLoading Method Initialized " + mode + "", CN_Bag));

            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            bool updateFlag = false;
            bool delFlag = false;
            bool insertFlag = false;

            string FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;

            if (HttpContext.Current.Session["Loading_check"] == "2")
            {
                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                try
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading", "0", "Update MnP_Loading Method Initialized if Loading_check == 2" + mode + "", CN_Bag));

                    if (FlightDepartureDate == "NULL")
                    {
                        FlightDepartureDate = DateTime.Now.ToString();
                    }


                    //Int64 temp = 0;
                    //Int64.TryParse(clvar.LoadingId, out temp);
                    string updateQuery = "UPDATE MnP_Loading  \n" +
                      "Set [description] = '" + Master.Description + "', \n" +
                      "courierName = '" + Master.CourierName + "',  \n" +
                      "sealNo = '" + Master.SealNo + "',  \n" +
                      "modifiedBy = '" + Master.hd_U_ID + "',    \n" +
                      "modifiedOn = GETDATE(),     \n" +
                      "LocationID = '" + Master.hd_LocationID + "',  \n" +
                      "TotalWeight ='" + Master.TotalWeight + "'   \n" +
                      "WHERE  id = '" + Master.lbl_loadingID + "'";

                    updateFlag = true;

                    SqlTransaction trans;
                    sqlcon.Open();
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcon;
                    trans = sqlcon.BeginTransaction();
                    sqlcmd.Transaction = trans;
                    try
                    {
                        if (updateFlag)
                        {
                            sqlcmd.CommandType = CommandType.Text;
                            sqlcmd.CommandText = updateQuery;
                            sqlcmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex_)
                    {
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading", "0", "Error: Update MnP_Loading Method " + ex_.Message.ToString().Trim('\'') + "", CN_Bag));
                        trans.Rollback();
                    }

                    if (updateFlag)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcon;
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "MnP_EditLoading_Merge1_NEW";
                        sqlcmd.Parameters.AddWithValue("@loadingID", Master.lbl_loadingID);
                        sqlcmd.Parameters.AddWithValue("@tblBag", Bags);
                        sqlcmd.Parameters.AddWithValue("@origin", Master.hd_branchCode);
                        sqlcmd.Parameters.AddWithValue("@createdBy", Master.hd_U_ID);
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", RandomNumber);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
                    }
                    Int64 loadingID1 = 0;

                    if (sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                    {
                        resp = new Tuple<bool, string>(true, clvar.LoadingId);
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper());
                    }

                    if (updateFlag)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcon;
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "MnP_EditLoading_Merge2_NEW";
                        sqlcmd.Parameters.Clear();
                        sqlcmd.Parameters.AddWithValue("@loadingID", Master.lbl_loadingID);
                        sqlcmd.Parameters.AddWithValue("@tblCN", Cns);
                        sqlcmd.Parameters.AddWithValue("@origin", Master.hd_branchCode);
                        sqlcmd.Parameters.AddWithValue("@createdBy", Master.hd_U_ID);
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", RandomNumber);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
                        Int64 loadingID = 0;
                        if (sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                        {
                            resp = new Tuple<bool, string>(true, Master.lbl_loadingID);
                        }
                        else
                        {
                            resp = new Tuple<bool, string>(false, Master.lbl_loadingID);
                        }
                    }

                    sqlcon.Close();
                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading", "0", "Error: Update MnP_Loading Method Initialized if Loading_check == 2" + mode + " " + ex.Message.ToString().Trim('\'') + "", CN_Bag));
                    resp = new Tuple<bool, string>(false, ex.Message);
                }
                finally { sqlcon.Close(); }
                return resp;
            }
            else
            {
                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                try
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading", "0", "Update MnP_Loading Method Initialized else" + mode + "", CN_Bag));

                    string Vehicleclause = "";
                    if (FlightDepartureDate == "NULL")
                    {
                        FlightDepartureDate = DateTime.Now.ToString();
                    }

                    if (mode.ToUpper() == "R")
                    {
                        Vehicleclause = " vehicleRegno ='" + Master.RegNo + "',vehicleid ='103',";
                    }
                    else
                    {
                        Vehicleclause = " vehicleid ='" + Master.VehicleNo + "',vehicleRegno ='',";
                    }

                    //Int64 temp = 0;
                    //Int64.TryParse(clvar.LoadingId, out temp);
                    string updateQuery = "UPDATE MnP_Loading  \n" +
                      "Set  \n" +
                      "[description] = '" + Master.Description + "', \n" +
                      Vehicleclause +
                      "modifiedBy = '" + Master.hd_U_ID + "',    \n" +
                      "modifiedOn = GETDATE(),     \n" +
                      "courierName = '" + Master.CourierName + "',  \n" +
                      "sealNo = '" + Master.SealNo + "',  \n" +
                      "TotalWeight ='" + Master.TotalWeight + "'   \n" +
                      "WHERE  id = '" + Master.lbl_loadingID + "'";


                    updateFlag = true;

                    SqlTransaction trans;
                    sqlcon.Open();
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcon;
                    trans = sqlcon.BeginTransaction();
                    sqlcmd.Transaction = trans;
                    try
                    {
                        if (updateFlag)
                        {
                            sqlcmd.CommandType = CommandType.Text;
                            sqlcmd.CommandText = updateQuery;
                            sqlcmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex_)
                    {
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading", "0", "Error: Update MnP_Loading Method else" + ex_.Message.ToString().Trim('\'') + "", CN_Bag));
                        trans.Rollback();
                    }

                    if (updateFlag)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcon;
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "MnP_EditLoading_Merge1_NEW";
                        sqlcmd.Parameters.AddWithValue("@loadingID", Master.lbl_loadingID);
                        sqlcmd.Parameters.AddWithValue("@tblBag", Bags);
                        sqlcmd.Parameters.AddWithValue("@origin", Master.hd_branchCode);// dd_orign.SelectedValue;
                        sqlcmd.Parameters.AddWithValue("@createdBy", Master.hd_U_ID);
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", RandomNumber);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
                    }
                    Int64 loadingID1 = 0;

                    if (sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                    {
                        //object obj = sqlcmd.Parameters["@LoadingID"].SqlValue;
                        //Int64.TryParse(obj.ToString(), out loadingID1);
                        resp = new Tuple<bool, string>(true, clvar.LoadingId);
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper());
                    }

                    if (updateFlag)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcon;
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "MnP_EditLoading_Merge2_NEW";
                        sqlcmd.Parameters.Clear();
                        sqlcmd.Parameters.AddWithValue("@loadingID", Master.lbl_loadingID);
                        sqlcmd.Parameters.AddWithValue("@tblCN", Cns);
                        sqlcmd.Parameters.AddWithValue("@origin", Master.hd_branchCode);// dd_orign.SelectedValue;
                        sqlcmd.Parameters.AddWithValue("@createdBy", Master.hd_U_ID);
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", RandomNumber);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
                        Int64 loadingID = 0;
                        if (sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                        {
                            resp = new Tuple<bool, string>(true, Master.lbl_loadingID);
                        }
                        else
                        {
                            resp = new Tuple<bool, string>(false, Master.lbl_loadingID);
                        }
                    }

                    sqlcon.Close();


                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading", "0", "Error: Update MnP_Loading Method else" + ex.Message.ToString().Trim('\'') + "", CN_Bag));
                    resp = new Tuple<bool, string>(false, ex.Message);
                }
                finally { sqlcon.Close(); }
                return resp;

            }

        }


        public class ReturnEditClass
        {
            public string status { get; set; }
            public string reason { get; set; }
            public MasterModel Master { get; set; }
            public BagModel[] Bags { get; set; }
            public ConsignmentModel[] Consignments { get; set; }
            public DropDownClass[] TouchPoint { get; set; }
            public DropDownClass[] Destination { get; set; }
        }
        public class ReturnBagClass
        {
            public string Status { get; set; }
            public string Cause { get; set; }
            public BagModel Bag { get; set; }

        }
        [WebMethod]
        public static ReturnEditClass GetLoadingData(string vid)
        {
            ReturnEditClass resp = new ReturnEditClass();

            DataSet ds = GetLoadingDetails(vid);

            if (ds != null)
            {
                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        resp.status = "1";

                        DataRow dr = ds.Tables[0].Rows[0];
                        MasterModel mm = new MasterModel();
                        mm.LoadingID = dr["id"].ToString();
                        mm.Date = dr["Date"].ToString();
                        mm.Description = dr["Description"].ToString();
                        mm.Destination = dr["Destination"].ToString();
                        mm.TransportType = dr["TransportationType"].ToString();
                        mm.VehicleNo = dr["VehicleID"].ToString();
                        mm.CourierName = dr["CourierName"].ToString();
                        mm.Route = dr["Routeid"].ToString();
                        mm.RegNo = dr["VehicleRegNo"].ToString();
                        mm.SealNo = dr["SealNo"].ToString();
                        mm.FlightDeparture = dr["DepartureFlightDate"].ToString();
                        mm.VehicleType = dr["VehicleType"].ToString();
                        mm.FlightNo = dr["FlightNo"].ToString();
                        resp.Master = mm;

                        DropDownClass dd = new DropDownClass();
                        List<DropDownClass> dds = new List<DropDownClass>();
                        dd.Value = dr["TouchPointCode"].ToString();
                        dd.Text = dr["TouchPoint"].ToString();
                        dds.Add(dd);
                        resp.TouchPoint = dds.ToArray();

                        dds = new List<DropDownClass>();
                        dd = new DropDownClass();
                        dd.Value = dr["Destination"].ToString();
                        dd.Text = dr["DestinationName"].ToString();
                        dds.Add(dd);
                        resp.Destination = dds.ToArray();

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            List<BagModel> Bags = new List<BagModel>();
                            List<ConsignmentModel> Consignments = new List<ConsignmentModel>();
                            foreach (DataRow row in ds.Tables[1].Rows)
                            {
                                if (row["Type"].ToString() == "1")
                                {
                                    BagModel bag = new BagModel();
                                    bag.BagNo = row["BagNumber"].ToString();
                                    bag.Destination = row["BagDestination"].ToString();
                                    bag.OriginName = row["BagOrigin"].ToString();
                                    bag.Remarks = row["BagRemarks"].ToString();
                                    bag.SealNo = row["BagSeal"].ToString();
                                    bag.Weight = row["BagWeight"].ToString();

                                    Bags.Add(bag);
                                }
                                else if (row["Type"].ToString() == "2")
                                {
                                    ConsignmentModel consignment = new ConsignmentModel();
                                    consignment.ConsignmentNumber = row["ConsignmentNumber"].ToString();
                                    consignment.Destination = row["CNDestination"].ToString();
                                    consignment.ConsignmentType = row["ConsignmentTYpe"].ToString();
                                    consignment.ServiceType = row["ServiceType"].ToString();
                                    consignment.Pieces = row["CNPieces"].ToString();
                                    consignment.Weight = row["CNWeight"].ToString();
                                    consignment.Remarks = row["CnRemarks"].ToString();

                                    Consignments.Add(consignment);
                                }
                            }

                            resp.Consignments = Consignments.ToArray();
                            resp.Bags = Bags.ToArray();
                        }

                    }
                    else
                    {
                        resp.status = "0";
                        resp.reason = "Loading Not Found";
                        return resp;
                    }
                }
                else
                {
                    resp.status = "0";
                    resp.reason = "Loading Not Found";
                    return resp;
                }
            }
            else
            {
                resp.status = "0";
                resp.reason = "Loading Not Found";
                return resp;
            }


            return resp;
        }
        public static DataSet GetLoadingDetails(string LoadingID)
        {
            DataSet ds = new DataSet();

            string sqlStringHeader = "SELECT mpl.id,\n" +
            "       FORMAT(mpl.date, 'yyyy-MM-dd') Date,\n" +
            "       mpl.description,\n" +
            "       mpl.transportationType,\n" +
            "       mpl.vehicleId,\n" +
            "       mpl.courierName,\n" +
            "       mpl.origin,\n" +
            "       mpl.destination,\n" +
            "       mpl.expressCenterCode,\n" +
            "       mpl.branchCode,\n" +
            "       mpl.zoneCode,\n" +
            "       mpl.createdBy,\n" +
            "       mpl.createdOn,\n" +
            "       mpl.modifiedBy,\n" +
            "       mpl.modifiedOn,\n" +
            "       mpl.routeId,\n" +
            "       mpl.IsMaster,\n" +
            "       mpl.ParentLoadingId,\n" +
            "       mpl.VehicleRegNo,\n" +
            "       mpl.sealNo,\n" +
            "       mpl.FlightNo,\n" +
            "       mpl.DepartureFlightDate,\n" +
            "       mpl.CNWeight,\n" +
            "       mpl.VehicleType, mr.MovementRouteId TouchPointCode, mr.Name TouchPoint,b.name destinationName\n" +
            "  FROM MnP_Loading mpl\n" +
            "       INNER JOIN Branches b\n" +
            "            ON  b.branchCode = mpl.destination\n" +
            "       INNER JOIN rvdbo.MovementRoute mr\n" +
            "            ON mr.IsActive = '1'\n" +
            "           AND mr.MovementRouteId = mpl.routeId\n" +
            "            OR mr.ParentMovementRouteId = mpl.routeId\n" +
            "  WHERE mpl.id = '" + LoadingID + "' and mpl.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and mpl.IsAirport ='0'";

            string sqlStringDetail =
            "SELECT *\n" +
            "  FROM (SELECT '1' TYPE,\n" +
            "               mlb.bagNumber,\n" +
            "               mlb.BagDestination,\n" +
            "               mlb.Remarks BagRemarks,\n" +
            "               mlb.BagWeight,\n" +
            "               b.name BagOrigin,\n" +
            "               mlb.BagSeal,\n" +
            "               '' consignmentNumber,\n" +
            "               '' CNDestination,\n" +
            "               '' CnRemarks,\n" +
            "               '' cnPieces,\n" +
            "               '' CNWeight,\n" +
            "               '' ServiceType,\n" +
            "               '' ConsignmentType,\n" +
            "               mlb.sortOrder\n" +
            "          FROM MnP_LoadingBag mlb\n" +
            "         INNER JOIN Branches b\n" +
            "            ON b.branchCode = mlb.BagOrigin\n" +
            "         WHERE mlb.loadingId = '" + LoadingID + "'\n" +
            "        UNION ALL\n" +
            "\n" +
            "        SELECT '2' TYPE,\n" +
            "               '' bagNumber,\n" +
            "               '' BagDestination,\n" +
            "               '' BagRemarks,\n" +
            "               '' BagWeight,\n" +
            "               '' BagOrigin,\n" +
            "               '' BagSeal,\n" +
            "               mlc.consignmentNumber,\n" +
            "               mlc.CNDestination,\n" +
            "               mlc.Remarks CnRemarks,\n" +
            "               mlc.cnPieces,\n" +
            "               mlc.CNWeight,\n" +
            "               mlc.ServiceType,\n" +
            "               mlc.ConsignmentType,\n" +
            "               mlc.SortOrder\n" +
            "          FROM MnP_LoadingConsignment mlc\n" +
            "         WHERE mlc.loadingId = '" + LoadingID + "') A\n" +
            "\n" +
            " ORDER BY A.TYPE, sortOrder desc";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlStringHeader, con);
                sda.Fill(ds, "Header");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sda = new SqlDataAdapter(sqlStringDetail, con);
                    sda.Fill(ds, "Details");
                }
                else
                {
                    ds = null;
                    return ds;
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }

            return ds;
        }


        [WebMethod]
        public static ReturnBagClass InsertBag(MasterModel Master, BagModel Bag, BagModel_linked[] Bag_linked, ConsignmentModel_linked[] Consignment_linked)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBag", "0", "InsertBag Method Initialized", Bag.BagNo));

            ReturnBagClass resp_ = new ReturnBagClass();
            resp_.Bag = Bag;

            Tuple<bool, string> resp = InsertBagInDB(Master, Bag);
            string[] result = { "", "" };
            if (resp.Item1)
            {
                resp_.Status = "1";
                resp_.Cause = resp.Item2;
                string[] resp_Linked = InsertLoading_Linked(Master, Bag_linked, Consignment_linked, Bag.BagNo);
                if (resp_Linked[0] == "1")
                {
                    resp_.Status = "1";
                    resp_.Cause = resp_Linked[1];
                }
                else
                {
                    resp_.Status = "2";
                    resp_.Cause = resp_Linked[1];
                }

            }
            else
            {
                if (resp.Item2 == "ID0")
                {
                    resp_.Status = "ID0";
                    resp_.Cause = resp.Item2;
                }
                else
                {
                    resp_.Status = "0";
                    resp_.Cause = resp.Item2;
                }

            }
            Transactionol(transactionQueires);
            transactionQueires.Clear();
            return resp_;
        }

        public static Tuple<bool, string> InsertBagInDB(MasterModel Master, BagModel Bag)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB", "0", "InsertBagInDB Method Initialized", Bag.BagNo));
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");
            #region Variables
            if (Master.VehicleMode.ToUpper() == "R")
            {
                clvar._VehicleId = "103";
                clvar.VehicleNo = Master.RegNo;

            }

            if (Master.VehicleMode.ToUpper() == "V")
            {
                clvar._VehicleId = Master.VehicleNo;
            }

            if (Master.TransportType == "197")
            {
                clvar.FlightNo = Master.FlightNo;
                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
            }

            if (Master.TransportType != "197")
            {
                clvar.FlightDepartureDate = "NULL";
            }

            clvar._Route = Master.Route;
            clvar._TouchPoint = Master.TouchPoint;
            clvar._TransportType = Master.TransportType;
            clvar._Vehicle = Master.VehicleNo;
            clvar._Orign = HttpContext.Current.Session["BranchCode"].ToString();
            clvar._StartDate = Master.Date;
            clvar._Destination = Master.Destination;
            clvar._RegNo = Master.RegNo;
            clvar._CourierName = Master.CourierName;
            clvar._Description = Master.Description;
            clvar.Seal = Master.SealNo;
            clvar._StateId = "4";
            clvar.Type = Master.VehicleType;
            //clvar.LoadingId = Master.LoadingID;
            clvar.LoadingId = Master.lbl_loadingID;
            #endregion

            DataTable bagDetails = GetBagDetails(Bag.BagNo);
            if (bagDetails != null)
            {
                if (bagDetails.Rows.Count > 0)
                {
                    Bag.Destination = bagDetails.Rows[0]["Destination"].ToString();
                    Bag.Origin = bagDetails.Rows[0]["Origin"].ToString();
                    Bag.SealNo = bagDetails.Rows[0]["SealNo"].ToString();
                    Bag.OriginName = bagDetails.Rows[0]["OriginName"].ToString();
                    Bag.Weight = bagDetails.Rows[0]["totalWeight"].ToString();
                }
            }


            SqlCommand headerCommand = new SqlCommand();
            SqlCommand bagCommand = new SqlCommand();
            SqlConnection con = new SqlConnection(clvar.Strcon());

            con.Open();

            string headInsert = "";
            string bagInsert = "";

            //bool headInserted = false;
            try
            {
                if (Master.hd_IDChk.ToString() == "0")
                {
                    bool chk = chkLoadingIDDuplication(clvar.LoadingId);
                    if (chk)
                    {
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB", "0", "INSERT INTO MnP_Loading Initialized", Bag.BagNo));

                        headInsert = "INSERT INTO MnP_Loading\n" +
                    "  (id,\n" +
                    "   date,\n" +
                    "   description,\n" +
                    "   transportationType,\n" +
                    "   vehicleId,\n" +
                    "   courierName,\n" +
                    "   origin,\n" +
                    "   destination,\n" +
                    "   expressCenterCode,\n" +
                    "   branchCode,\n" +
                    "   zoneCode,\n" +
                    "   createdBy,\n" +
                    "   createdOn,\n" +
                    "   routeId,\n" +
                    "   VehicleRegNo,\n" +
                    "   sealNo,\n" +
                    "   FlightNo,\n" +
                    "   DepartureFlightDate,\n" +
                    "   VehicleType,\n" +
                    "   IsMaster) output inserted.id\n" +
                    "VALUES\n" +
                    "  (@id,\n" +
                    "   GETDATE(),\n" +
                    "   @description,\n" +
                    "   @transportationType,\n" +
                    "   @vehicleId,\n" +
                    "   @courierName,\n" +
                    "   @origin,\n" +
                    "   @destination,\n" +
                    "   @expressCenterCode,\n" +
                    "   @branchCode,\n" +
                    "   @zoneCode,\n" +
                    "   @createdBy,\n" +
                    "   GETDATE(),\n" +
                    "   @routeId,\n" +
                    "   @VehicleRegNo,\n" +
                    "   @sealNo,\n" +
                    "   @flightNo,\n" +
                    "   @DepartureFlightDate,\n" +
                    "   @VehicleType,\n" +
                    "   '1')";

                        //if (Master.LoadingID != Master.lbl_loadingID)
                        //{
                        //    resp = new Tuple<bool, string>(false, "Merging Error: On Insertion MNP Loading, Loading ID is Different " + clvar.LoadingId + "!=" + Master.lbl_loadingID);
                        //    return resp;
                        //}
                        string loadingNumber = Master.lbl_loadingID;
                        headerCommand.CommandText = headInsert;
                        //headerCommand.Parameters.AddWithValue("@id", loadingNumber); Master.lbl_loadingID
                        headerCommand.Parameters.AddWithValue("@id", Master.lbl_loadingID);
                        headerCommand.Parameters.AddWithValue("@BranchCode", Master.hd_branchCode);
                        headerCommand.Parameters.AddWithValue("@ZoneCode", Master.hd_zoneCode);
                        headerCommand.Parameters.AddWithValue("@ExpressCenterCode", Master.hd_expressCenterCode);

                        headerCommand.Parameters.AddWithValue("@transportationType", clvar.TransportType);
                        headerCommand.Parameters.AddWithValue("@routeId", clvar.Route);
                        headerCommand.Parameters.AddWithValue("@VehicleType", clvar.Type);
                        headerCommand.Parameters.AddWithValue("@vehicleId", clvar.VehicleId);
                        headerCommand.Parameters.AddWithValue("@description", clvar.Description);
                        headerCommand.Parameters.AddWithValue("@courierName", clvar.CourierName);
                        headerCommand.Parameters.AddWithValue("@origin", clvar.Orign);
                        headerCommand.Parameters.AddWithValue("@destination", clvar.Destination);
                        headerCommand.Parameters.AddWithValue("@createdBy", Master.hd_U_ID);
                        headerCommand.Parameters.AddWithValue("@sealNo", clvar.Seal);
                        if (clvar.TransportType == "197")
                        {
                            headerCommand.Parameters.AddWithValue("@FlightNo", clvar.FlightNo);
                            headerCommand.Parameters.AddWithValue("@DepartureFlightDate", clvar.FlightDepartureDate);
                        }
                        else
                        {
                            headerCommand.Parameters.AddWithValue("@FlightNo", DBNull.Value);
                            headerCommand.Parameters.AddWithValue("@DepartureFlightDate", DBNull.Value);
                        }
                        headerCommand.Parameters.AddWithValue("@VehicleRegNo", clvar.RegNo);

                        if (headInsert != "")
                        {
                            SqlTransaction trans_;
                            trans_ = con.BeginTransaction();
                            try
                            {

                                #region Inserting Header
                                headerCommand.Connection = con;
                                headerCommand.Transaction = trans_;
                                object obj = headerCommand.ExecuteScalar();

                                if (obj == null)
                                {
                                    resp = new Tuple<bool, string>(false, "Could Not Save Loading");
                                    return resp;
                                }
                                else
                                {

                                    //   clvar.LoadingId = LoadingID.ToString();
                                    //headInserted = true;
                                    Master.hd_IDChk = "1";

                                }
                                #endregion
                                trans_.Commit();
                            }
                            catch (Exception Er)
                            {
                                trans_.Rollback();

                            }
                        }
                        else
                        {
                            clvar.LoadingId = Master.lbl_loadingID;
                        }
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "ID0");
                        return resp;
                    }
                }

                if (Master.hd_IDChk.ToString() == "1")
                {
                    if (true)
                    {
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB", "0", "INSERT INTO MnP_LoadingBag Initialized", Bag.BagNo));
                        bagInsert = "INSERT INTO MnP_LoadingBag\n" +
                                            "  (loadingId,\n" +
                                            "   bagNumber,\n" +
                                            "   BagDestination,\n" +
                                            "   createdBy,\n" +
                                            "   createdOn,\n" +
                                            "   Remarks,\n" +
                                            "   BagWeight,\n" +
                                            "   BagOrigin,\n" +
                                            "   BagSeal,\n" +
                                            "   sortOrder)\n" +
                                            "  VALUES( @LoadingID,\n" +
                                            "         @bagNumber,\n" +
                                            "         @BagDestination,\n" +
                                            "         @createdBy,\n" +
                                            "         GETDATE(),\n" +
                                            "         @Remarks,\n" +
                                            "         @BagWeight,\n" +
                                            "         @BagOrigin,\n" +
                                            "         @BagSeal,\n" +
                                            "         @sortOrder)\n";

                        //if (clvar.LoadingId != Master.lbl_loadingID)
                        //{
                        //    resp = new Tuple<bool, string>(false, "Merging Error: On Insertion MNP Bag LoadingID is Different " + clvar.LoadingId + "!=" + Master.lbl_loadingID + ". Kindly Refresh Page and then use the same loading ID again through UPDATE tab or contact IT for assistance. ThankYou.");
                        //    return resp;
                        //}

                        bagCommand.CommandText = bagInsert;
                        bagCommand.Parameters.AddWithValue("@LoadingID", Master.lbl_loadingID);
                        bagCommand.Parameters.AddWithValue("@createdby", Master.hd_U_ID);
                        bagCommand.Parameters.AddWithValue("@bagNumber", Bag.BagNo);
                        bagCommand.Parameters.AddWithValue("@BagDestination", Bag.Destination);
                        bagCommand.Parameters.AddWithValue("@Remarks", Bag.Remarks);
                        bagCommand.Parameters.AddWithValue("@BagWeight", Bag.Weight);
                        bagCommand.Parameters.AddWithValue("@BagSeal", Bag.SealNo);
                        bagCommand.Parameters.AddWithValue("@sortOrder", Bag.SortOrder);
                        bagCommand.Parameters.AddWithValue("@BagOrigin", Bag.Origin);

                        SqlTransaction trans_1;
                        trans_1 = con.BeginTransaction();

                        try
                        {

                            bagCommand.Connection = con;
                            bagCommand.Transaction = trans_1;
                            int count = bagCommand.ExecuteNonQuery();
                            if (count <= 0)
                            {
                                resp = new Tuple<bool, string>(false, "Could Not save Bag");
                                return resp;
                            }
                            else
                            {
                                resp = new Tuple<bool, string>(true, clvar.LoadingId);
                            }
                            trans_1.Commit();
                        }
                        catch (Exception Err)
                        {
                            resp = new Tuple<bool, string>(false, Err.Message);
                            trans_1.Rollback();
                        }
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "Could Not save Bag");
                        return resp;
                    }
                }

            }
            catch (Exception ex)
            {
                transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB", "0", ex.Message.ToString().Trim('\''), Bag.BagNo));
                resp = new Tuple<bool, string>(false, ex.Message);
                // trans_.Rollback();
            }
            finally { con.Close(); }
            return resp;
        }
        public static DataTable GetBagDetails(string BagNumber)
        {

            string sqlString = "SELECT b.bagNumber,\n" +
            "       b.destination,\n" +
            "       b.totalWeight,\n" +
            "       b.origin,\n" +
            "       b.sealNo,\n" +
            "       b2.sname + '-' + b2.name OriginName\n" +
            "  FROM Bag b\n" +
            "  LEFT OUTER JOIN Branches b2\n" +
            "    ON b2.branchCode = b.origin\n" +
            " WHERE b.bagNumber = '" + BagNumber + "'";

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


        [WebMethod]
        public static string[][] Get_bagInformation(string BagNo, MasterModel Master)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "Get_bagInformation", "0", "Get_bagInformation Method Initialized", BagNo));

            List<string[]> resp = new List<string[]>();

            string ManifestNo_ = BagNo;
            DataTable ds = GetBagDetails(BagNo);

            if (ds.Rows.Count != 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    string[] consignment = { "", "", "", "", "", "" };
                    consignment[0] = dr[0].ToString();
                    consignment[1] = dr[3].ToString();
                    consignment[2] = dr[1].ToString();
                    consignment[3] = dr[2].ToString();
                    consignment[4] = dr[4].ToString();
                    consignment[5] = dr[5].ToString();
                    resp.Add(consignment);
                }
            }
            else
            {
                string[] consignment = { "" };
                consignment[0] = "N/A";
                resp.Add(consignment);
            }
            Transactionol(transactionQueires);
            transactionQueires.Clear();
            return resp.ToArray();
        }

        [WebMethod]
        public static string[][] Get_ConsignmentInformation(string ConsignmentNo)
        {
            List<string[]> resp = new List<string[]>();

            string ManifestNo_ = ConsignmentNo;
            string reason = "";
            bool runsheetchk = true;
            DataSet ds = GetConsignmentCheck_runsheet(ConsignmentNo);

            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        reason = ds.Tables[0].Rows[0]["Reason"].ToString();
                        if (reason == "59")
                        {
                            string[] consignment = { "", "", "" };
                            consignment[0] = "RSC";
                            consignment[1] = "RS-Return to Shipper";
                            consignment[2] = ConsignmentNo;
                            resp.Add(consignment);
                            runsheetchk = false;
                        }
                        else if (reason == "123")
                        {
                            string[] consignment = { "", "", "" };
                            consignment[0] = "RSC";
                            consignment[1] = "D-DELIVERED";
                            consignment[2] = ConsignmentNo;
                            resp.Add(consignment);
                            runsheetchk = false;

                        }
                        if (runsheetchk)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string[] consignment = { "", "", "", "", "", "", "" };
                                consignment[0] = ConsignmentNo;
                                consignment[1] = ds.Tables[0].Rows[0]["ServiceTypeName"].ToString();
                                consignment[2] = ds.Tables[0].Rows[0]["Destination"].ToString();
                                consignment[3] = ds.Tables[0].Rows[0]["Weight"].ToString();
                                consignment[4] = DateTime.Parse(ds.Tables[0].Rows[0]["createdon"].ToString()).ToString("yyyy-MM-dd");
                                consignment[5] = ds.Tables[0].Rows[0]["ConsignmentTypeID"].ToString();
                                consignment[6] = ds.Tables[0].Rows[0]["Pieces"].ToString();
                                resp.Add(consignment);
                                break;
                            }
                        }
                    }
                    else
                    {

                        string[] consignment = { "" };
                        consignment[0] = "N/A";
                        resp.Add(consignment);
                    }
                }
                else
                {
                    string[] consignment = { "" };
                    consignment[0] = "N/A";
                    resp.Add(consignment);
                }

            }

            return resp.ToArray();
        }

        private static DataTable chkRunsheet(string Consignment)
        {
            string query = "select * from RunsheetConsignment where Reason in ('59','123') and consignmentnumber = '" + Consignment + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt); ;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public static DataSet GetConsignmentCheck_runsheet(string ManifestNo)
        {
            Cl_Variables clvar = new Cl_Variables();
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "Select  p.*  from Consignment p where p.consignmentnumber ='" + ManifestNo + "'";

                cmd.CommandText = "SELECT p.ServiceTypeName, \n"
               + "       p.destination, \n"
               + "       p.[weight], \n"
               + "       p.createdOn, \n"
               + "       p.consignmentTypeId, \n"
               + "       p.pieces, \n"
               + "       r.Reason \n"
               + "FROM   Consignment p \n"
               + "       LEFT JOIN RunsheetConsignment r \n"
               + "            ON  r.consignmentNumber = p.consignmentNumber \n"
               + "WHERE  p.consignmentnumber = '" + ManifestNo + "' \n"
               + "ORDER BY \n"
               + "       r.createdOn DESC";

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }

        public static DataSet GetConsignmentCheck_Arival(string ManifestNo)
        {
            Cl_Variables clvar = new Cl_Variables();
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select ServiceType ServiceTypeName,BranchCode Destination,cnWeight Weight,a.CreatedOn,'1' ConsignmentTypeID,cnPieces Pieces from ArrivalScan_Detail ad inner join arrivalscan a on a.id = ArrivalID  where  ad.consignmentnumber ='" + ManifestNo + "'";

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }

        [WebMethod]
        public static string[] InsertConsignment(MasterModel Master, ConsignmentModel Consignment, BagModel_linked[] Bag_linked, ConsignmentModel_linked[] Consignment_linked)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertConsignment", "0", "InsertConsignment Method Initialized", Consignment.ConsignmentNumber));

            Tuple<bool, string> resp = InsertCNInDB(Master, Consignment);
            string[] result = { "", "" };
            if (resp.Item1)
            {

                result[0] = "1";
                result[1] = resp.Item2;

                string[] resp_Linked = InsertLoading_Linked(Master, Bag_linked, Consignment_linked, Consignment.ConsignmentNumber);
                if (resp_Linked[0] == "1")
                {
                    result[0] = "1";
                    result[1] = resp_Linked[1];
                }
                else
                {
                    result[0] = "2";
                    result[1] = resp_Linked[1];
                }

            }
            else
            {
                if (resp.Item2.ToString() == "ID0")
                {
                    result[0] = "ID0";
                    result[1] = resp.Item2;
                }
                else
                {
                    result[0] = "0";
                    result[1] = resp.Item2;
                }

            }
            Transactionol(transactionQueires);
            transactionQueires.Clear();
            return result;
        }

        private static string[] InsertLoading_Linked(MasterModel Master, BagModel_linked[] Bag_linked, ConsignmentModel_linked[] Consignment_linked, string CN_Bag)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertLoading_Linked", "0", "InsertLoading_Linked Method Initialized", CN_Bag));

            string[] resp_ = { "0", "" };
            string RandomNumber = "";

            DataTable tblBags = new DataTable();
            tblBags.Columns.AddRange(new DataColumn[]{
            new DataColumn("loadingId", typeof(Int64)) ,
            new DataColumn("bagNumber", typeof(string)) ,
            new DataColumn("BagDestination", typeof(string)) ,
            new DataColumn("UloadingStateID", typeof(string)) ,
            new DataColumn("Remarks", typeof(string)) ,
            new DataColumn("BagWeight", typeof(string)) ,
            new DataColumn("BagOrigin", typeof(string)) ,
            new DataColumn("BagSeal", typeof(string)),
            new DataColumn("UniqueNumber", typeof(string)),

            new DataColumn("SortOrder", typeof(int))
        });

            DataTable tblCn = new DataTable();
            tblCn.Columns.AddRange(new DataColumn[] {
            new DataColumn("loadingId", typeof(Int64)) ,
            new DataColumn("consignmentNumber", typeof(string)) ,
            new DataColumn("CNDestination", typeof(string)) ,
            new DataColumn("UnloadingStateID", typeof(string)) ,
            new DataColumn("Remarks", typeof(string)) ,
            new DataColumn("cnPieces", typeof(int)) ,
            new DataColumn("CNWeight", typeof(string)) ,
            new DataColumn("ServiceType", typeof(string)) ,
            new DataColumn("ConsignmentType", typeof(int)) ,
            new DataColumn("UniqueNumber", typeof(string)),

            new DataColumn("SortOrder", typeof(int))

        });
            int sortOrder = 1;

            foreach (BagModel_linked bag in Bag_linked)
            {
                DataRow dr = tblBags.NewRow();

                //dr["LoadingID"] = Master.LoadingID;
                dr["LoadingID"] = Master.lbl_loadingID;
                dr["bagNumber"] = bag.BagNo;
                dr["BagDestination"] = bag.Destination;
                dr["UloadingStateID"] = "";
                dr["Remarks"] = bag.Remarks;
                dr["BagWeight"] = bag.Weight;
                dr["BagOrigin"] = HttpContext.Current.Session["BranchCode"].ToString();
                dr["BagSeal"] = bag.SealNo;
                dr["UniqueNumber"] = RandomNumber;

                dr["SortOrder"] = sortOrder;
                sortOrder++;
                tblBags.Rows.Add(dr);
            }
            sortOrder = 1;

            foreach (ConsignmentModel_linked cn in Consignment_linked)
            {
                DataRow dr = tblCn.NewRow();
                //dr["loadingId"] = Master.LoadingID;
                dr["loadingId"] = Master.lbl_loadingID;
                dr["consignmentNumber"] = cn.ConsignmentNumber;
                dr["CNDestination"] = cn.Destination;
                dr["UnloadingStateID"] = "";
                dr["Remarks"] = cn.Remarks;
                int cnPieces = 0;
                int.TryParse(cn.Pieces, out cnPieces);
                cn.Pieces = cnPieces.ToString();
                dr["cnPieces"] = int.Parse(cn.Pieces.ToString());
                dr["CNWeight"] = cn.Weight;
                dr["ServiceType"] = cn.ServiceType;
                dr["ConsignmentType"] = 12;
                dr["UniqueNumber"] = RandomNumber;

                dr["SortOrder"] = sortOrder;
                sortOrder++;
                tblCn.Rows.Add(dr);
            }


            #region Variables
            if (Master.VehicleMode.ToUpper() == "R")//Rented.Checked)
            {
                clvar._VehicleId = "103";
                clvar.VehicleNo = Master.RegNo;

            }

            if (Master.VehicleMode.ToUpper() == "V")// Vehicle.Checked)
            {
                clvar._VehicleId = Master.VehicleNo;// dd_vehicle.SelectedValue;
            }

            if (Master.TransportType == "197")//dd_transporttype.SelectedValue == "197")
            {
                clvar.FlightNo = Master.FlightNo;// txt_flight.Text;
                                                 // clvar.FlightDepartureDate = dept_date.Text;
                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
                //if (dept_date.Text.Trim(' ') == "__:__")
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Provide Flight Departure time')", true);
                //    error_msg.Text = "Provide Flight Departure time";
                //    return;
                //}

                // "2013-09-16 12:22:38.833";
            }

            if (Master.TransportType != "197")//dd_transporttype.SelectedValue != "197")
            {
                clvar.FlightDepartureDate = "NULL";
            }
            string mode = "";
            clvar._Route = Master.Route;// dd_route.SelectedValue;
            clvar._TouchPoint = Master.TouchPoint;// dd_touchpoint.SelectedValue;
            clvar._TransportType = Master.TransportType;// dd_transporttype.SelectedValue;
            clvar._Vehicle = Master.VehicleNo;// dd_vehicle.SelectedValue;
            clvar._Orign = HttpContext.Current.Session["BranchCode"].ToString();// dd_orign.SelectedValue;
            clvar._StartDate = Master.Date;// dd_start_date.Text;
            clvar._Destination = Master.Destination;// dd_destination.SelectedValue;
            clvar._RegNo = Master.RegNo;// txt_reg.Text;
            clvar._CourierName = Master.CourierName;// txt_couriername.Text;
            clvar._Description = Master.Description;// txt_description.Text;
            clvar.Seal = Master.SealNo;// txt_seal.Text;
            clvar._StateId = "4"; // LOADED STATE ID
            clvar.Type = Master.VehicleType;// dd_vehicleType.SelectedValue;
                                            //clvar.LoadingId = Master.LoadingID;
            clvar.LoadingId = Master.lbl_loadingID;
            clvar.Remarks = RandomNumber;
            clvar.Weight = Master.TotalWeight;
            mode = Master.VehicleMode;

            #endregion
            if (Master.Mode.ToUpper() == "NEW")
            {
                if (Master.lbl_loadingID != "")
                {
                    Tuple<bool, string> resp = UpdateLoading(Master, tblBags, tblCn, mode, RandomNumber, CN_Bag);
                    if (resp.Item1)
                    {
                        resp_[0] = "1";
                        resp_[1] = resp.Item2.ToString();
                        Insert_ConsignmentTrackingHistoryFromLoading_consignment_Linked(Master, Consignment_linked, CN_Bag);
                        //Insert_ConsignmentTrackingHistoryFromLoading_consignment(Master, Consignments);
                        Insert_ConsignmentTrackingHistoryFromLoading_bag_Linked(Master, Bag_linked, CN_Bag);
                        //Insert_ConsignmentTrackingHistoryFromLoading_bag(Master, Bags);
                    }
                    else
                    {
                        resp_[0] = "0";
                        resp_[1] = resp.Item2.ToString();
                    }
                }

            }
            else if (Master.Mode.ToUpper() == "UPDATE")
            {
                Tuple<bool, string> resp = UpdateLoading(Master, tblBags, tblCn, mode, RandomNumber, CN_Bag);
                if (resp.Item1)
                {
                    resp_[0] = "1";
                    resp_[1] = resp.Item2.ToString();
                    Insert_ConsignmentTrackingHistoryFromLoading_consignment_Linked(Master, Consignment_linked, CN_Bag);
                    //Insert_ConsignmentTrackingHistoryFromLoading_consignment(Master, Consignments);
                    Insert_ConsignmentTrackingHistoryFromLoading_bag_Linked(Master, Bag_linked, CN_Bag);


                }
                else
                {
                    resp_[0] = "0";
                    resp_[1] = resp.Item2.ToString();
                }
            }


            return resp_;
        }

        private static void Insert_ConsignmentTrackingHistoryFromLoading_bag_Linked(MasterModel Master, BagModel_linked[] Bag_linked, string CN_Bag)
        {

            transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_bag_Linked", "0", "Insert_ConsignmentTrackingHistoryFromLoading_bag_Linked Method Initialized", CN_Bag));
            List<BagModel_linked> Bags = new List<BagModel_linked>();
            List<ConsignmentModel_linked> Consignments = new List<ConsignmentModel_linked>();

            foreach (BagModel_linked Bag in Bag_linked)
            {

                //string sqlString = "insert into ConsignmentsTrackingHistory\n" +
                // "  (loadingNumber, CurrentLocation, consignmentNumber, stateID, transactionTime)\n" +
                // "  select '" + Master.LoadingID + "' LOADING, '" + HttpContext.Current.Session["LocationName"].ToString() + "', cm.consignmentNumber CN, '4', GETDATE()\n" +
                //"    from Mnp_ConsignmentManifest cm\n" +
                //"   inner join BagManifest bm\n" +
                //"      on bm.manifestNumber = cm.manifestNumber\n" +
                //"   where bm.bagNumber in\n" +
                //"         (select lb.bagNumber\n" +
                //"            from MnP_LoadingBag lb\n" +
                //"           where lb.loadingId = '" + Master.LoadingID + "') and bm.bagnumber = '" + cn1.BagNo + "'";

                string sql = "/************************************************************ \n"
            + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
            + " * Time: 11/13/2018 4:28:12 PM \n"
            + " ************************************************************/ \n"
            + " \n"
            + "MERGE ConsignmentsTrackingHistory AS TARGET  \n"
            + "                               USING ( \n"
            + "                                         SELECT a.CNNUMBER consignmentNumber, \n"
            + "                                                '4' stateID, \n"
            + "         '" + HttpContext.Current.Session["LocationName"].ToString() + "' CurrentLocation,\n"
            + "                                                GETDATE() TransactionTime, \n"
            + "                                                '" + Master.lbl_loadingID + "' loadingNumber, a.bagNumber \n"
            + "                                         FROM   ( \n"
            + "                                                    SELECT ba.bagNumber, \n"
            + "                                                           '' ManifestNo, \n"
            + "                                                           ba.outpieceNumber  \n"
            + "                                                           CNNUMBER \n"
            + "                                                    FROM    \n"
            + "                                                           BagOutpieceAssociation  \n"
            + "                                                           ba \n"
            + "                                                    WHERE  ba.bagNumber IN ('" + Bag.BagNo.Trim() + "')\n"
            + "                                                    UNION \n"
            + "                                                    SELECT bm.bagNumber, \n"
            + "                                                           bm.manifestNumber  \n"
            + "                                                           ManifestNo, \n"
            + "                                                           cm.consignmentNumber  \n"
            + "                                                           CNNUMBER \n"
            + "                                                    FROM   BagManifest bm \n"
            + "                                                           INNER JOIN  \n"
            + "                                                                Mnp_ConsignmentManifest  \n"
            + "                                                                cm \n"
            + "                                                                ON  bm.manifestNumber =  \n"
            + "                                                                    cm.manifestNumber \n"
            + "                                                    WHERE  bm.bagNumber IN ('" + Bag.BagNo.Trim() + "') \n"
            + "                                                ) a \n"
            + "                                                INNER JOIN Branches b \n"
            + "                                                     ON  b.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n"
            + "                                     ) AS SOURCE  \n"
            + "                                ON ( \n"
            + "                                       TARGET.consignmentNumber = SOURCE.consignmentNumber \n"
            + "                                       AND source.loadingNumber = TARGET.loadingnumber \n"
            + "                                   ) \n"
            + "                                WHEN NOT MATCHED BY TARGET THEN   \n"
            + "INSERT  \n"
            + "  ( \n"
            + "    loadingNumber, \n"
            + "    CurrentLocation, \n"
            + "    consignmentNumber,bagNumber ,\n"
            + "    stateID, \n"
            + "    transactionTime \n"
            + "  ) \n"
            + "VALUES \n"
            + "  ( \n"
            + "    source.loadingNumber, \n"
            + "    '" + HttpContext.Current.Session["LocationName"].ToString() + "', \n"
            + "    SOURCE.consignmentNumber,SOURCE.bagNumber,\n"
            + "    '4', \n"
            + "    GETDATE() \n"
            + "  ); \n"
            + "";

                //string sqlString = "MERGE ConsignmentsTrackingHistory AS TARGET \n" +
                //                   "USING (  select  '" + Master.lbl_loadingID + "' loadingId , consignmentNumber " +
                //                   "    from Mnp_ConsignmentManifest cm\n" +
                //                   "   inner join BagManifest bm\n" +
                //                   "      on bm.manifestNumber = cm.manifestNumber\n" +
                //                   "   where bm.bagNumber in\n" +
                //                   "         (select lb.bagNumber\n" +
                //                   "            from MnP_LoadingBag lb\n" +
                //                   "           where lb.loadingId = '" + Master.lbl_loadingID + "') and bm.bagnumber = '" + Bag.BagNo.Trim() + "' \n" +
                //                   " ) AS SOURCE \n" +
                //                   " ON (TARGET.consignmentNumber = SOURCE.consignmentNumber and source.loadingID = Target.loadingnumber)\n" +
                //                   " WHEN NOT MATCHED BY TARGET THEN  \n" +
                //                   " INSERT(loadingNumber, CurrentLocation, consignmentNumber, stateID, transactionTime)\n" +
                //                   " values (source.loadingID, '" + HttpContext.Current.Session["LocationName"].ToString() + "', SOURCE.consignmentNumber , '4', GETDATE() );";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_bag_Linked", "0", "Error:" + ex.Message.Trim('\''), CN_Bag));
                    CommonFunction ComFunction = new CommonFunction();
                    ComFunction.InsertErrorLog("", "", "", "", Master.lbl_loadingID.ToString(), "", "LOADING", ex.Message);
                }
                con.Close();
            }
        }

        private static void Insert_ConsignmentTrackingHistoryFromLoading_consignment_Linked(MasterModel Master, ConsignmentModel_linked[] Consignment_linked, string CN_Bag)
        {

            transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_consignment_Linked", "0", "Insert_ConsignmentTrackingHistoryFromLoading_consignment_Linked Method Initialized", CN_Bag));

            List<BagModel_linked> Bags = new List<BagModel_linked>();
            List<ConsignmentModel_linked> Consignments = new List<ConsignmentModel_linked>();
            foreach (ConsignmentModel_linked cns in Consignment_linked)
            {

                //string sqlString = "insert into ConsignmentsTrackingHistory\n" +
                // "  (loadingNumber, CurrentLocation, consignmentNumber, stateID, transactionTime)\n" +
                // "  select '" + Master.LoadingID + "', '" + HttpContext.Current.Session["LocationName"].ToString() + "', lc.consignmentNumber CN, '4', GETDATE()\n" +
                // "    from MnP_LoadingConsignment lc\n" +
                // "   where lc.loadingId = '" + Master.LoadingID + "'\n" +
                // "    and lc.consignmentNumber ='" + cn1.ConsignmentNumber + "'\n";

                string sqlString = "MERGE ConsignmentsTrackingHistory AS TARGET \n" +
                                    "USING (  select loadingId,consignmentNumber " +
                                    "                 from MnP_LoadingConsignment lc \n" +
                                    "                where lc.loadingId = '" + Master.lbl_loadingID + "'\n" +
                                    "                 and lc.consignmentNumber ='" + cns.ConsignmentNumber.Trim() + "'\n" +
                                    " ) AS SOURCE \n" +
                                    " ON (TARGET.consignmentNumber = SOURCE.consignmentNumber and source.loadingID = Target.loadingnumber)\n" +
                                    " WHEN NOT MATCHED BY TARGET THEN  \n" +
                                    " INSERT(loadingNumber, CurrentLocation, consignmentNumber, stateID, transactionTime)\n" +
                                    " values (source.loadingID, '" + HttpContext.Current.Session["LocationName"].ToString() + "', SOURCE.consignmentNumber , '4', GETDATE() );";

                SqlConnection con = new SqlConnection(clvar.Strcon());
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sqlString, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "Insert_ConsignmentTrackingHistoryFromLoading_consignment_Linked", "0", "Error: " + ex.Message.Trim('\''), CN_Bag));
                    CommonFunction ComFunction = new CommonFunction();
                    ComFunction.InsertErrorLog("", "", "", "", Master.lbl_loadingID.ToString(), "", "LOADING", ex.Message);
                }
                con.Close();
            }
        }

        public static Tuple<bool, string> InsertCNInDB(MasterModel Master, ConsignmentModel cn)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertCNInDB", "0", "InsertCNInDB Method Initialized", cn.ConsignmentNumber));

            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");
            #region Variables
            string FlightDepartureDate = "";
            if (Master.VehicleMode.ToUpper() == "R")
            {
                clvar._VehicleId = "103";
                clvar.VehicleNo = Master.RegNo;

            }

            if (Master.VehicleMode.ToUpper() == "V")
            {
                clvar._VehicleId = Master.VehicleNo;
            }

            if (Master.TransportType == "197")
            {
                clvar.FlightNo = Master.FlightNo;
                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
                FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
            }

            if (Master.TransportType != "197")
            {
                clvar.FlightDepartureDate = "NULL";
            }

            clvar._Route = Master.Route;
            clvar._TouchPoint = Master.TouchPoint;
            clvar._TransportType = Master.TransportType;
            clvar._Vehicle = Master.VehicleNo;
            clvar._Orign = HttpContext.Current.Session["BranchCode"].ToString();
            clvar._StartDate = Master.Date;
            clvar._Destination = Master.Destination;
            clvar._RegNo = Master.RegNo;
            clvar._CourierName = Master.CourierName;
            clvar._Description = Master.Description;
            clvar.Seal = Master.SealNo;
            clvar._StateId = "4";
            clvar.Type = Master.VehicleType;
            //clvar.LoadingId = Master.LoadingID;
            clvar.LoadingId = Master.lbl_loadingID;
            #endregion
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand headerCommand = new SqlCommand();
            SqlCommand cnCommand = new SqlCommand();



            string headInsert = "";
            string cnInsert = "";


            // bool headInserted = false;
            try
            {
                transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertCNInDB", "0", "INSERT INTO MnP_Loading Method Initialized", cn.ConsignmentNumber));

                clvar.LoadingId = Master.lbl_loadingID;

                if (Master.hd_IDChk.ToString() == "0")
                {
                    bool chk = chkLoadingIDDuplication(clvar.LoadingId);
                    if (chk)
                    {
                        headInsert = "INSERT INTO MnP_Loading\n" +
                    "  (id,\n" +
                    "   date,\n" +
                    "   description,\n" +
                    "   transportationType,\n" +
                    "   vehicleId,\n" +
                    "   courierName,\n" +
                    "   origin,\n" +
                    "   destination,\n" +
                    "   expressCenterCode,\n" +
                    "   branchCode,\n" +
                    "   zoneCode,\n" +
                    "   createdBy,\n" +
                    "   createdOn,\n" +
                    "   routeId,\n" +
                    "   VehicleRegNo,\n" +
                    "   sealNo,\n" +
                    "   FlightNo,\n" +
                    "   DepartureFlightDate,\n" +
                    "   VehicleType,\n" +
                    "   IsMaster) output inserted.id\n" +
                    "VALUES\n" +
                    "  (@id,\n" +
                    "   GETDATE(),\n" +
                    "   @description,\n" +
                    "   @transportationType,\n" +
                    "   @vehicleId,\n" +
                    "   @courierName,\n" +
                    "   @origin,\n" +
                    "   @destination,\n" +
                    "   @expressCenterCode,\n" +
                    "   @branchCode,\n" +
                    "   @zoneCode,\n" +
                    "   @createdBy,\n" +
                    "   GETDATE(),\n" +
                    "   @routeId,\n" +
                    "   @VehicleRegNo,\n" +
                    "   @sealNo,\n" +
                    "   @flightNo,\n" +
                    "   @DepartureFlightDate,\n" +
                    "   @VehicleType,\n" +
                    "   '1')";

                        //if (Master.LoadingID != Master.lbl_loadingID)
                        //{
                        //    resp = new Tuple<bool, string>(false, "Merging Error: On InsertionMNP LOADING LoadingID is Different " + clvar.LoadingId + "!=" + Master.lbl_loadingID + ". Kindly Refresh Page and then use the same loading ID again through UPDATE tab or contact IT for assistance. ThankYou.");
                        //    return resp;
                        //}

                        headerCommand.CommandText = headInsert;
                        headerCommand.Parameters.AddWithValue("@id", Master.lbl_loadingID);
                        headerCommand.Parameters.AddWithValue("@BranchCode", Master.hd_branchCode);
                        headerCommand.Parameters.AddWithValue("@ZoneCode", Master.hd_zoneCode);
                        headerCommand.Parameters.AddWithValue("@ExpressCenterCode", Master.hd_expressCenterCode);

                        headerCommand.Parameters.AddWithValue("@transportationType", Master.TransportType);
                        headerCommand.Parameters.AddWithValue("@routeId", Master.Route);
                        headerCommand.Parameters.AddWithValue("@VehicleType", Master.VehicleType);
                        headerCommand.Parameters.AddWithValue("@vehicleId", Master.VehicleNo);
                        headerCommand.Parameters.AddWithValue("@description", Master.Description);
                        headerCommand.Parameters.AddWithValue("@courierName", Master.CourierName);
                        headerCommand.Parameters.AddWithValue("@origin", Master.hd_branchCode);
                        headerCommand.Parameters.AddWithValue("@destination", Master.Destination);
                        headerCommand.Parameters.AddWithValue("@createdBy", Master.hd_U_ID);
                        headerCommand.Parameters.AddWithValue("@sealNo", Master.SealNo);
                        if (Master.TransportType == "197")
                        {
                            headerCommand.Parameters.AddWithValue("@FlightNo", Master.FlightNo);
                            headerCommand.Parameters.AddWithValue("@DepartureFlightDate", FlightDepartureDate);
                        }
                        else
                        {
                            headerCommand.Parameters.AddWithValue("@FlightNo", DBNull.Value);
                            headerCommand.Parameters.AddWithValue("@DepartureFlightDate", DBNull.Value);
                        }
                        headerCommand.Parameters.AddWithValue("@VehicleRegNo", Master.RegNo);
                        #region Inserting Header

                        SqlTransaction trans_;
                        con.Open();
                        trans_ = con.BeginTransaction();
                        try
                        {

                            headerCommand.Connection = con;
                            headerCommand.Transaction = trans_;
                            object obj = headerCommand.ExecuteScalar();

                            if (obj == null)
                            {
                                resp = new Tuple<bool, string>(false, "Could Not Save Loading");
                                return resp;
                            }
                            else
                            {
                                Int64 LoadingID = 0;
                                Int64.TryParse(obj.ToString(), out LoadingID);
                                if (LoadingID <= 0)
                                {
                                    resp = new Tuple<bool, string>(false, "Could Not Save Loading");
                                    return resp;
                                }
                                else
                                {
                                    //  clvar.LoadingId = LoadingID.ToString();
                                    //  headInserted = true;
                                    Master.hd_IDChk = "1";
                                }
                            }
                            trans_.Commit();
                            con.Close();

                        }
                        catch (Exception Err)
                        {
                            trans_.Rollback();
                        }

                        #endregion
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "ID0");
                        return resp;
                    }
                }

                if (Master.hd_IDChk.ToString() == "1")
                {
                    if (true)
                    {
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "INSERT INTO MnP_LoadingConsignment", "0", "INSERT INTO MnP_LoadingConsignment Method Initialized", cn.ConsignmentNumber));
                        cnInsert = "INSERT INTO MnP_LoadingConsignment\n" +
                        "  (loadingId,\n" +
                        "   consignmentNumber,\n" +
                        "   CNDestination,\n" +
                        "   createdBy,\n" +
                        "   createdOn,\n" +
                        "   Remarks,\n" +
                        "   cnPieces,\n" +
                        "   CNWeight,\n" +
                        "   ServiceType,\n" +
                        "   ConsignmentType,\n" +
                        "   SortOrder)\n" +
                        "  SELECT @LoadingID,\n" +
                        "         @consignmentNumber,\n" +
                        "         @CNDestination,\n" +
                        "         @createdBy,\n" +
                        "         GETDATE(),\n" +
                        "         @Remarks,\n" +
                        "         @cnPieces,\n" +
                        "         @CNWeight,\n" +
                        "         @ServiceType,\n" +
                        "         @ConsignmentType,\n" +
                        "         @SortOrder";

                        //if (clvar.LoadingId != Master.lbl_loadingID)
                        //{
                        //    resp = new Tuple<bool, string>(false, "Merging Error: On InsertionMNP MNP Consignment LoadingID is Different " + clvar.LoadingId + "!=" + Master.lbl_loadingID + ". Kindly Refresh Page and then use the same loading ID again through UPDATE tab or contact IT for assistance. ThankYou.");
                        //    return resp;
                        //}

                        cnCommand.CommandText = cnInsert;
                        cnCommand.Parameters.AddWithValue("@LoadingID", clvar.LoadingId);
                        cnCommand.Parameters.AddWithValue("@createdby", Master.hd_U_ID);
                        cnCommand.Parameters.AddWithValue("@consignmentNumber", cn.ConsignmentNumber);
                        cnCommand.Parameters.AddWithValue("@CNDestination", cn.Destination);
                        cnCommand.Parameters.AddWithValue("@Remarks", cn.Remarks);
                        cnCommand.Parameters.AddWithValue("@cnPieces", cn.Pieces);
                        cnCommand.Parameters.AddWithValue("@CNWeight", cn.Weight);
                        cnCommand.Parameters.AddWithValue("@ServiceType", cn.ServiceType);
                        cnCommand.Parameters.AddWithValue("@ConsignmentType", cn.ConsignmentType);
                        cnCommand.Parameters.AddWithValue("@SortOrder", cn.SortOrder);

                        SqlTransaction trans_1;
                        con.Open();
                        trans_1 = con.BeginTransaction();

                        try
                        {

                            cnCommand.Connection = con;
                            cnCommand.Transaction = trans_1;

                            int count = cnCommand.ExecuteNonQuery();
                            if (count <= 0)
                            {
                                resp = new Tuple<bool, string>(false, "Could Not save Consignment");
                                return resp;
                            }
                            else
                            {
                                trans_1.Commit();
                                resp = new Tuple<bool, string>(true, clvar.LoadingId);
                            }
                            con.Close();

                        }
                        catch (Exception Err)
                        {
                            transactionQueires.Add(Get_ErrorLog_Query(Master, "INSERT INTO MnP_LoadingConsignment", "0", "ERROR:" + Err.Message.Trim('\''), cn.ConsignmentNumber));
                            resp = new Tuple<bool, string>(false, Err.Message);
                            trans_1.Rollback();
                        }
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "Could Not save Consignment");
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertCNInDB MnP_Loading", "0", "ERROR:" + ex.Message.Trim('\''), cn.ConsignmentNumber));
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }

            return resp;
        }

        [WebMethod]
        public static string RemoveRow(string LoadingID, string BagNumber, string ConsignmentNumber, string Remove)
        {

            clvar.LoadingId = LoadingID;
            if (Remove.ToUpper() == "BAG")
            {
                clvar.BagNumber = BagNumber;
                Tuple<bool, string> resp = RemoveBagFromDB(clvar);
                if (resp.Item1)
                {
                    return "OK";
                }
                else
                {
                    return resp.Item2;
                }
            }
            else if (Remove.ToUpper() == "CN")
            {
                clvar.ConsignmentNo = ConsignmentNumber;
                Tuple<bool, string> resp = RemoveCNFromDB(clvar);
                if (resp.Item1)
                {
                    return "OK";
                }
                else
                {
                    return resp.Item2;
                }
            }


            return "";

        }


        public static Tuple<bool, string> RemoveCNFromDB(Variable clvar)
        {

            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            string query1 = "insert into mnp_loadingConsignment_Archieve Select * from mnp_loadingConsignment where loadingID = '" + clvar.LoadingId + "' and consignmentNumber = '" + clvar.ConsignmentNo + "'";

            string query = "DELETE FROM mnp_loadingConsignment where loadingID = '" + clvar.LoadingId + "' and consignmentNumber = '" + clvar.ConsignmentNo + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(query1, con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(query, con);

                int count = cmd.ExecuteNonQuery();

                if (count <= 0)
                {
                    resp = new Tuple<bool, string>(false, "Could Not Remove");
                }
                else
                {
                    resp = new Tuple<bool, string>(true, "");

                    query = "DELETE FROM consignmentstrackinghistory where loadingnumber = '" + clvar.LoadingId + "' and consignmentNumber = '" + clvar.ConsignmentNo + "'";
                    cmd = new SqlCommand(query, con);
                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            { resp = new Tuple<bool, string>(false, ex.Message); }
            finally { con.Close(); }

            return resp;
        }

        public static Tuple<bool, string> RemoveBagFromDB(Variable clvar)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            string query1 = "insert into MnP_LoadingBag_archieve Select * from MnP_LoadingBag where loadingID = '" + clvar.LoadingId + "' and Bagnumber = '" + clvar.BagNumber + "'";

            string query = "delete from mnp_loadingBag where loadingID = '" + clvar.LoadingId + "' and bagNumber = '" + clvar.BagNumber + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query1, con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(query, con);

                int count = cmd.ExecuteNonQuery();

                if (count <= 0)
                {
                    resp = new Tuple<bool, string>(false, "Could Not Remove");
                }
                else
                {
                    resp = new Tuple<bool, string>(true, "");
                    query = "DELETE FROM consignmentstrackinghistory where loadingnumber = '" + clvar.LoadingId + "' and bagNumber = '" + clvar.BagNumber + "'";
                    cmd = new SqlCommand(query, con);
                    count = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            { resp = new Tuple<bool, string>(false, ex.Message); }
            finally { con.Close(); }

            return resp;
        }


        [WebMethod]
        public static string RefreshTime(string a)
        {
            return DateTime.Now.ToString();
        }

        public class ConTypes
        {

            public string ID { get; set; }
            public string Name { get; set; }
        }
        [WebMethod]
        public static ConTypes[] GetConTypesForDropDown()
        {
            List<ConTypes> contypes = new List<ConTypes>();


            DataTable dt = GetConTypes();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ConTypes contype = new ConTypes();
                        contype.ID = dr["id"].ToString();
                        contype.Name = dr["Name"].ToString();
                        contypes.Add(contype);
                    }
                }
            }





            return contypes.ToArray();
        }

        public static DataTable GetConTypes()
        {

            string sqlstring = "SELECT * FROM ConsignmentType ct WHERE ct.[status] = '1'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlstring, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public static DataTable Get_Loading(string Loadingid)
        {

            string sqlstring = "SELECT * FROM MnP_Loading ct WHERE ct.id ='" + Loadingid + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlstring, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public static DataTable GetLoadingNo()
        {

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("MnP_GenerateLoading_Number", con);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                //sda.SelectCommand.Parameters.AddWithValue("@year", DateTime.Now.ToString("yyyy").Substring(2, 2));
                //sda.SelectCommand.Parameters.AddWithValue("@branch", HttpContext.Current.Session["BRANCHCODE"].ToString());
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }


            return dt;

        }


        private static string Get_ErrorLog_Query(MasterModel Master, string EventName, string Status, string description, string CN_Bag)
        {
            string sql = "INSERT INTO [dbo].[MnP_Loading_ERROR_LOG] \n"
               + "           ([loadingID] \n"
               + "           ,[UserID] \n"
               + "           ,[date] \n"
               + "           ,[EventName] \n"
               + "           ,[description] \n"
               + "           ,[isAirpot] \n"
               + "           ,[branchCode] \n"
               + "           ,[ZONECODE] \n"
               + "           ,[ExpressCenter] \n"
               + "           ,[LocationName] \n"
               + "           ,[LocationID] \n"
               + "           ,[loadingID_Issue] \n"
               + "           ,[STATUS],CN_Bag) \n"
               + "     VALUES \n"
               + "           ('" + Master.lbl_loadingID + "','" + Master.hd_U_ID + "',\n"
               + " getdate(),\n"
               + " '" + EventName + "',\n"
               + " '" + description.Replace("'", "") + "',\n"
               + " '0',\n"
               + " '" + Master.hd_branchCode + "',\n"
               + " '" + Master.hd_zoneCode + "',\n"
               + " '" + Master.hd_expressCenterCode + "',\n"
               + " '" + Master.hd_LocationName + "',\n"
               + " '" + Master.hd_LocationID + "',\n"
               + " '" + Master.LoadingID + "','" + Status + "','" + CN_Bag + "')";

            return sql;
        }

        private static string Transactionol(List<string> queries)
        {
            string error = "";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            try
            {
                sqlcmd.Transaction = trans;
                sqlcmd.CommandType = CommandType.Text;
                for (int o = 0; o < queries.Count(); o++)
                {
                    sqlcmd.CommandText = queries[o].ToString();
                    int count = sqlcmd.ExecuteNonQuery();
                    if (count <= 0)
                    {
                        trans.Rollback();
                        error = "Could Not Update";
                        sqlcon.Close();
                        return error;
                    }
                }
                trans.Commit();
                sqlcon.Close();
            }
            catch (Exception ex)
            { trans.Rollback(); sqlcon.Close(); error = ex.Message; }
            finally { sqlcon.Close(); }


            return error;
        }

    }
}