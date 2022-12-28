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
using Dapper;

namespace MRaabta.Files
{
    public partial class MasterAirportLoading : System.Web.UI.Page
    {
        ////////*****Shaheer Sohail M&P IT*****////////
        public static Variable clvar = new Variable();
        public static bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();
        Cl_Variables cvar = new Cl_Variables();

        public static List<string> transactionQueires = new List<string>();

        LoadingPrintReport cl_lp = new LoadingPrintReport();

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
                //flight.Visible = false;
                Get_Orign();
                //    Get_Destination();
                Get_MasterRoute();
                Get_MasterTransportType();
                Get_MasterVehicle();

                GetCNLengths();
                GetVehicleType();
                Session["Loading_check"] = "1";



                DateTime dateNow = DateTime.Now;
                //int newID = (dateNow.Year + dateNow.Month + dateNow.Day + dateNow.Hour + dateNow.Minute + dateNow.Second) * dateNow.Millisecond;
                //txt_vid.Text = DateTime.Now.Year.ToString().Substring(2, 2) + HttpContext.Current.Session["BranchCode"].ToString() + HttpContext.Current.Session["U_ID"].ToString() + newID;

                string LoadingIDLogic = DateTime.Now.Year.ToString().Substring(2, 2) + dateNow.Month.ToString("d2") + dateNow.Day.ToString("d2") + dateNow.Hour.ToString("D2") + dateNow.Minute.ToString("D2") + dateNow.Second.ToString("D2") + dateNow.Millisecond.ToString("D3");

                bool chk = chkLoadingIDDuplication(LoadingIDLogic);
                if (chk)
                {
                    txt_vid.Text = LoadingIDLogic;
                }
                else
                {
                    while (!chk)
                    {
                        dateNow = DateTime.Now;
                        LoadingIDLogic = DateTime.Now.Year.ToString().Substring(2, 2) + dateNow.Month.ToString("d2") + dateNow.Day.ToString("d2") + dateNow.Hour.ToString("D2") + dateNow.Minute.ToString("D2") + dateNow.Second.ToString("D2") + dateNow.Millisecond.ToString("D3");
                        chk = chkLoadingIDDuplication(LoadingIDLogic);
                    }
                    txt_vid.Text = LoadingIDLogic;
                }

                if (rbtn_mode.SelectedValue.ToUpper() == "NEW")
                {
                    //hd_loadingID.Value = txt_vid.Text; 
                    hd_loadingID.Text = txt_vid.Text;
                }
                else
                {
                    hd_loadingID.Text = "";
                }

            }
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



        public static Tuple<bool, string> UpdateLoading(MasterModel Master, Variable clvar, DataTable Bags, DataTable Cns)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading_Method", "1", "InsertLoading_Linked_Method Initialized"));
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            bool updateFlag = false;
            bool delFlag = false;
            bool insertFlag = false;
            if (HttpContext.Current.Session["Loading_check"] == "2")
            {
                SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                try
                {
                    if (clvar.FlightDepartureDate == "NULL")
                    {
                        clvar.FlightDepartureDate = DateTime.Now.ToString();
                    }

                    string updateQuery = "UPDATE MnP_Loading  \n" +
                      "Set [description] = '" + Master.Description + "', \n" +
                      "transportationType = '" + Master.TransportType + "', \n" +
                      "vehicleId  ='" + clvar.VehicleId + "' ,      \n" +
                      "courierName = '" + Master.CourierName + "',  \n" +
                      "modifiedBy = '" + Master.hd_U_ID + "',    \n" +
                      "modifiedOn = GETDATE(),     \n" +
                      "VehicleRegNo = '" + Master.VehicleNo + "', \n" +
                      "sealNo = '" + Master.SealNo + "',       \n" +
                      "FlightNo = '" + clvar.FlightNo + "',  \n" +
                      "VehicleType = '" + clvar.Type + "',  \n" +
                      "IsAirport = '1',   \n" +
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
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading_Method_masterInsertion", "0", "UpdateLoading_Method_masterInsertion Initialized  Loading_check = 2"));
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
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", clvar.Remarks);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
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
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", clvar.Remarks);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
                        Int64 loadingID = 0;
                        if (sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                        {
                            resp = new Tuple<bool, string>(true, Master.lbl_loadingID);
                        }
                        else
                        {
                            transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading_Method_ChildInsertion", "0", resp + " Loading_check = 2"));
                            resp = new Tuple<bool, string>(false, sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper());
                        }
                    }

                    sqlcon.Close();


                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading_Method", "0", "UpdateLoading_Method Initialized  Loading_check = 2"));
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
                    string Vehicleclause = "";

                    if (Master.FlightDeparture == "NULL")
                    {
                        clvar.FlightDepartureDate = DateTime.Now.ToString();
                    }

                    if (Master.VehicleMode.ToUpper() == "R")
                    {
                        Vehicleclause = " vehicleRegno ='" + clvar.VehicleNo + "',vehicleid ='103',";
                    }
                    else
                    {
                        Vehicleclause = " vehicleid ='" + clvar.VehicleId + "',vehicleRegno ='',";
                    }


                    string updateQuery = "UPDATE MnP_Loading  \n" +
                      " Set \n" +
                      "[description] = '" + Master.Description + "', \n" +
                       Vehicleclause +
                      "modifiedBy = '" + Master.hd_U_ID + "',    \n" +
                      "modifiedOn = GETDATE(),     \n" +
                      "sealNo = '" + Master.SealNo + "',       \n" +
                      "isAirport ='1',             \n" +
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
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading_Method_masterInsertion", "0", "UpdateLoading_Method_masterInsertion Initialized  Else Condition"));
                        trans.Rollback();
                    }

                    if (updateFlag)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcon;
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "MnP_EditLoading_Merge1_new";
                        sqlcmd.Parameters.AddWithValue("@loadingID", Master.lbl_loadingID);
                        sqlcmd.Parameters.AddWithValue("@tblBag", Bags);
                        sqlcmd.Parameters.AddWithValue("@origin", Master.hd_branchCode.ToString());
                        sqlcmd.Parameters.AddWithValue("@createdBy", Master.hd_U_ID.ToString());
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", clvar.Remarks);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
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
                        sqlcmd.Parameters.AddWithValue("@origin", Master.hd_branchCode.ToString());
                        sqlcmd.Parameters.AddWithValue("@createdBy", Master.hd_U_ID.ToString());
                        sqlcmd.Parameters.AddWithValue("@UniqueNumber", clvar.Remarks);
                        sqlcmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlcmd.ExecuteNonQuery();
                        Int64 loadingID = 0;
                        if (sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper() == "OK")
                        {
                            resp = new Tuple<bool, string>(true, Master.lbl_loadingID);
                        }
                        else
                        {
                            transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading_Method_ChildInsertion", "0", resp + " Else Condtion"));
                            resp = new Tuple<bool, string>(false, sqlcmd.Parameters["@result"].SqlValue.ToString().ToUpper());
                        }
                    }

                    sqlcon.Close();


                }
                catch (Exception ex)
                {
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "UpdateLoading_Method", "0", "UpdateLoading_Method Initialized  Else Condtion"));
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
        public static string[] InsertLoading(MasterModel Master, BagModel[] Bags, ConsignmentModel[] Consignments)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertLoading", "1", "InsertLoading Method Initialized"));
            string RandomNumber = Master.hd_U_ID.ToString() + DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace("/", "");

            string[] resp_ = { "0", "" };

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
            //new DataColumn("CreatedBy", typeof(string)),
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
            //new DataColumn("CreatedBy", typeof(string)),
            new DataColumn("SortOrder", typeof(int))
        });
            int sortOrder = 1;
            foreach (BagModel bag in Bags)
            {
                DataRow dr = tblBags.NewRow();

                dr["LoadingID"] = Master.lbl_loadingID;
                dr["bagNumber"] = bag.BagNo;
                dr["BagDestination"] = bag.Destination;
                dr["UloadingStateID"] = "";
                dr["Remarks"] = bag.Remarks;
                dr["BagWeight"] = bag.Weight;
                dr["BagOrigin"] = Master.hd_branchCode.ToString();
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
                dr["consignmentNumber"] = cn.ConsignmentNumber;
                dr["CNDestination"] = cn.Destination;
                dr["UnloadingStateID"] = "";
                dr["Remarks"] = cn.Remarks;
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
                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
            }

            if (Master.TransportType != "197")//dd_transporttype.SelectedValue != "197")
            {
                clvar.FlightDepartureDate = "NULL";
            }
            clvar.Remarks = RandomNumber;

            #endregion
            if (Master.Mode.ToUpper() == "NEW")
            {
                if (Master.lbl_loadingID != "")
                {
                    Tuple<bool, string> resp = UpdateLoading(Master, clvar, tblBags, tblCn);
                    if (resp.Item1)
                    {
                        resp_[0] = "1";
                        resp_[1] = resp.Item2.ToString();
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
                Tuple<bool, string> resp = UpdateLoading(Master, clvar, tblBags, tblCn);
                if (resp.Item1)
                {
                    resp_[0] = "1";
                    resp_[1] = resp.Item2.ToString();
                }
                else
                {
                    resp_[0] = "0";
                    resp_[1] = resp.Item2.ToString();
                }
            }
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

        [WebMethod]
        public static ReturnEditClass GetLoadingData(string laodingID, string hd_branchCode)
        {
            ReturnEditClass resp = new ReturnEditClass();

            DataSet ds = GetLoadingDetails(laodingID, hd_branchCode);

            if (ds != null)
            {
                if (ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        resp.status = "1";

                        DataRow dr = ds.Tables[0].Rows[0];
                        MasterModel mm = new MasterModel();
                        mm.lbl_loadingID = dr["id"].ToString();
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
                        transactionQueires.Add(Get_ErrorLog_Query(mm, "GetLoadingData_method", "1", "GetLoadingData Method Initialized Geting Data"));
                        Transactionol(transactionQueires);
                        transactionQueires.Clear();
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
        public static DataSet GetLoadingDetails(string LoadingID, string hd_branchCode)
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
            "  WHERE mpl.id = '" + LoadingID + "' and mpl.branchCode = '" + hd_branchCode + "' and mpl.IsAirport ='1'";

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
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBag", "1", "InsertBag Method Initialized"));

            ReturnBagClass resp_ = new ReturnBagClass();
            resp_.Bag = Bag;

            Tuple<bool, string> resp = InsertBagInDB(Master, Bag);
            string[] result = { "", "" };
            if (resp.Item1)
            {
                string[] resp_Linked = InsertLoading_Linked(Master, Bag_linked, Consignment_linked);
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

        public static string Insert_ConsignmentTrackingHistoryFromLoading_bag(MasterModel Master, BagModel bag)
        {

            string sql = "/************************************************************ \n"
         + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
         + " * Time: 11/13/2018 4:28:12 PM \n"
         + " ************************************************************/ \n"
         + " \n"
         + "MERGE ConsignmentsTrackingHistory AS TARGET  \n"
         + "                               USING ( \n"
         + "                                         SELECT a.CNNUMBER consignmentNumber, \n"
         + "                                                '20' stateID, \n"
         + "                                        '" + Master.hd_LocationName.ToString() + "' CurrentLocation,\n"
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
         + "                                                    WHERE  ba.bagNumber IN ('" + bag.BagNo.Trim() + "')\n"
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
         + "                                                    WHERE  bm.bagNumber IN ('" + bag.BagNo.Trim() + "') \n"
         + "                                                ) a \n"
         + "                                                INNER JOIN Branches b \n"
         + "                                                     ON  b.branchCode = '" + Master.hd_branchCode.ToString() + "' \n"
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
         + "    '" + Master.lbl_loadingID + "', \n"
         + "    '" + Master.hd_LocationName.ToString()
         + "', \n"
         + "    SOURCE.consignmentNumber,SOURCE.bagNumber,\n"
         + "    '20', \n"
         + "    GETDATE() \n"
         + "  ); \n"
         + "";



            return sql;


        }

        [WebMethod]
        public static string[] InsertConsignment(MasterModel Master, ConsignmentModel Consignment, BagModel_linked[] Bag_linked, ConsignmentModel_linked[] Consignment_linked)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertConsignment", "1", "InsertConsignment Method Initialized"));
            Tuple<bool, string> resp = InsertCNInDB(Master, Consignment);
            string[] result = { "", "" };
            if (resp.Item1)
            {
                string[] resp_Linked = InsertLoading_Linked(Master, Bag_linked, Consignment_linked);
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

        public static string Insert_ConsignmentTrackingHistoryFromLoading_consignment(MasterModel Master, ConsignmentModel cn)
        {

            string sqlString = "insert into ConsignmentsTrackingHistory\n" +
             "  (loadingNumber, CurrentLocation, consignmentNumber, stateID, transactionTime)\n" +
             "  select '" + Master.lbl_loadingID + "' LOADING, '" +  HttpContext.Current.Session["LocationName"].ToString() + "', lc.consignmentNumber CN, '20', GETDATE()\n" +
             "    from MnP_LoadingConsignment lc\n" +
             "   where lc.loadingId = '" + Master.lbl_loadingID + "'\n" +
             "    and lc.consignmentNumber ='" + cn.ConsignmentNumber.Trim() + "'\n";
            return sqlString;
        }

        public static Tuple<bool, string> InsertBagInDB(MasterModel Master, BagModel Bag)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB_method", "1", "InsertBagInDB Method Initialized"));


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
                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
            }

            if (Master.TransportType != "197")
            {
                clvar.FlightDepartureDate = "";
            }

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
            SqlCommand headerCommand_backup = new SqlCommand();
            SqlCommand bagCommand = new SqlCommand();
            string headInsert = "";
            string headInsert_backup = "";
            string bagInsert = "";
            //bool headInserted = false;
            //string loadingID = Master.LoadingID;
            string loadingID = Master.lbl_loadingID;
            SqlConnection con = new SqlConnection(clvar.Strcon());
            //DataTable dsLoading = Get_Loading(loadingID);

            //if (dsLoading.Rows.Count == 0)
            //{

            try
            {

                #region Inserting Master Data

                if (Master.hd_IDChk.ToString() == "0")
                {
                    bool chk = chkLoadingIDDuplication(Master.lbl_loadingID);
                    if (chk)
                    {
                        if (Master.TransportType != "197")
                        {
                            Master.FlightNo = null;
                            Master.FlightDeparture = null;
                        }

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
                        "   IsMaster,LocationiD,IsAirport)\n" +
                        "VALUES\n" +
                        "  ('" + Master.lbl_loadingID + "','" + Master.Date + "',\n" +
                        "   '" + Master.Description + "',\n" +
                        "   '" + Master.TransportType + "',\n" +
                        "   '" + clvar._VehicleId + "',\n" +
                        "   '" + Master.CourierName + "',\n" +
                        "   '" + Master.hd_branchCode + "',\n" +
                        "   '" + Master.Destination + "',\n" +
                        "   '" + Master.hd_expressCenterCode + "',\n" +
                        "   '" + Master.hd_branchCode + "',\n" +
                        "   '" + Master.hd_zoneCode + "',\n" +
                        "   '" + Master.hd_U_ID + "',\n" +
                        "        GETDATE() ,\n" +
                        "   '" + Master.Route + "',\n" +
                        "   '" + clvar.VehicleNo + "',\n" +
                        "   '" + Master.SealNo + "',\n" +
                        "   '" + Master.FlightNo + "',\n" +
                        "   '" + clvar.FlightDepartureDate + "',\n" +
                        "   '" + Master.VehicleType + "',\n" +
                        "   '1','" + Master.hd_LocationID + "','1')";



                        headInsert_backup = "INSERT INTO MnP_Loading_backUp\n" +
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
                       "   IsMaster,LocationiD,IsAirport,insertType)\n" +
                         "VALUES\n" +
                        "  ('" + Master.lbl_loadingID + "','" + Master.Date + "',\n" +
                        "   '" + Master.Description + "',\n" +
                        "   '" + Master.TransportType + "',\n" +
                        "   '" + clvar._VehicleId + "',\n" +
                        "   '" + Master.CourierName + "',\n" +
                        "   '" + Master.hd_branchCode + "',\n" +
                        "   '" + Master.Destination + "',\n" +
                        "   '" + Master.hd_expressCenterCode + "',\n" +
                        "   '" + Master.hd_branchCode + "',\n" +
                        "   '" + Master.hd_zoneCode + "',\n" +
                        "   '" + Master.hd_U_ID + "',\n" +
                        "        GETDATE() ,\n" +
                        "   '" + Master.Route + "',\n" +
                        "   '" + clvar.VehicleNo + "',\n" +
                        "   '" + Master.SealNo + "',\n" +
                        "   '" + Master.FlightNo + "',\n" +
                        "   '" + clvar.FlightDepartureDate + "',\n" +
                        "   '" + Master.VehicleType + "',\n" +
                        "   '1','" + Master.hd_LocationID + "','1','1')";





                    }

                    if (headInsert != "")
                    {
                        List<string> queries = new List<string>();
                        queries.Add(headInsert);
                        queries.Add(headInsert_backup);

                        string result = Transactionol(queries);

                        if (result != "")
                        {
                            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB_MasterTransaction", "0", result));
                            resp = new Tuple<bool, string>(false, "Could Not Save Loading");
                            return resp;
                        }
                        else
                        {
                            Int64 LoadingID = 0;
                            Int64.TryParse(Master.lbl_loadingID.ToString(), out LoadingID);
                            if (LoadingID <= 0)
                            {
                                resp = new Tuple<bool, string>(false, "Could Not Save Loading");
                                return resp;
                            }
                            else
                            {
                                Master.hd_IDChk = "1";
                            }
                        }
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "ID0");
                        return resp;
                    }
                }

                #endregion

                #region Inserting Bag And Tracking

                if (Master.hd_IDChk.ToString() == "1")
                {
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
                                    "  VALUES( '" + Master.lbl_loadingID + "',\n" +
                                    "         '" + Bag.BagNo + "',\n" +
                                    "         '" + Bag.Destination + "',\n" +
                                    "         '" + Master.hd_U_ID + "',\n" +
                                    "         GETDATE(),\n" +
                                    "         '" + Bag.Remarks + "',\n" +
                                    "         '" + Bag.Weight + "',\n" +
                                    "         '" + Bag.Origin + "',\n" +
                                    "         '" + Bag.SealNo + "',\n" +
                                    "         '" + Bag.SortOrder + "')\n";

                    if (Master.LoadingID != Master.lbl_loadingID)
                    {

                    }

                    string insertBagTracking = Insert_ConsignmentTrackingHistoryFromLoading_bag(Master, Bag);

                    List<string> queries = new List<string>();

                    queries.Add(bagInsert);
                    queries.Add(insertBagTracking);

                    string result = Transactionol_tracking(queries);

                    if (result != "")
                    {
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB_Bag_tracking", "0", result));
                        resp = new Tuple<bool, string>(false, "Could Not save Bag");
                        return resp;
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(true, Master.lbl_loadingID);
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertBagInDB_method", "0", ex.Message.ToString()));
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { }
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

            sqlString = "SELECT TOP 1 B.BAGNUMBER,\n" +
            "       B.BAGDESTINATION DESTINATION,\n" +
            "       B.BAGWEIGHT TOTALWEIGHT,\n" +
            "       B.BAGORIGIN ORIGIN,\n" +
            "       B.BAGSEAL SEALNO,\n" +
            "       B2.SNAME + '-' + B2.NAME ORIGINNAME\n" +
            "  FROM MNP_LOADINGBAG B\n" +
            "  LEFT OUTER JOIN BRANCHES B2\n" +
            "    ON B2.BRANCHCODE = B.BAGORIGIN\n" +
            " WHERE B.BAGNUMBER = '" + BagNumber + "'\n" +
            " ORDER BY B.CREATEDON DESC";

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
        public static DataTable GetBagDetails_(string BagNumber)
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
        public static string[][] Get_bagInformation(string ConsignmentNo)
        {
            List<string[]> resp = new List<string[]>();

            string ManifestNo_ = ConsignmentNo;
            DataTable ds = GetBagDetails(ConsignmentNo);

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
                ds = GetBagDetails_(ConsignmentNo);
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
            }

            return resp.ToArray();
        }

        [WebMethod]
        public static string[][] Get_ConsignmentInformation(string ConsignmentNo)
        {
            List<string[]> resp = new List<string[]>();

            string ManifestNo_ = ConsignmentNo;
            DataSet ds = GetConsignmentCheck(ConsignmentNo);

            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string[] consignment = { "", "", "", "", "" };
                            consignment[0] = ConsignmentNo;
                            consignment[1] = ds.Tables[0].Rows[0][0].ToString();
                            consignment[2] = ds.Tables[0].Rows[0][2].ToString();
                            consignment[3] = ds.Tables[0].Rows[0][8].ToString();
                            consignment[4] = ds.Tables[0].Rows[0][7].ToString();// DateTime.Parse(ds.Tables[0].Rows[0][4].ToString()).ToString("yyyy-MM-dd");
                            resp.Add(consignment);
                        }
                    }
                    else
                    {
                        ds = GetConsignmentCheck_CN(ConsignmentNo);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string[] consignment = { "", "", "", "", "" };
                                consignment[0] = ConsignmentNo;
                                consignment[1] = ds.Tables[0].Rows[0][1].ToString();
                                consignment[2] = ds.Tables[0].Rows[0][2].ToString();
                                consignment[3] = ds.Tables[0].Rows[0][3].ToString();
                                consignment[4] = ds.Tables[0].Rows[0][4].ToString();// DateTime.Parse(ds.Tables[0].Rows[0][4].ToString()).ToString("yyyy-MM-dd");
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

        public static DataSet GetConsignmentCheck(string ManifestNo)
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
                cmd.CommandText = "Select  * from mnp_Loadingconsignment p where p.consignmentnumber ='" + ManifestNo + "' order by createdon desc";

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }

        public static DataSet GetConsignmentCheck_CN(string ManifestNo)
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
                cmd.CommandText = "Select  consignmentnumber, orgin, destination, weight, pieces from consignment p where p.consignmentnumber ='" + ManifestNo + "' order by createdon desc";

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return ds;
        }

        public static Tuple<bool, string> InsertCNInDB(MasterModel Master, ConsignmentModel cn)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertCNInDB_method", "1", "InsertCNInDB_method Initialized"));

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
                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
            }

            if (Master.TransportType != "197")
            {
                clvar.FlightDepartureDate = "";
            }


            #endregion
            SqlCommand headerCommand = new SqlCommand();
            SqlCommand cnCommand = new SqlCommand();
            string headInsert = "";
            string headInsert_backup = "";
            string cnInsert = "";

            try
            {
                #region Inserting Master Data

                if (Master.hd_IDChk.ToString() == "0")
                {
                    bool chk = chkLoadingIDDuplication(Master.lbl_loadingID);
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
                      "   IsMaster,LocationiD,IsAirport)\n" +
                      "VALUES\n" +
                      "  ('" + Master.lbl_loadingID + "','" + Master.Date + "',\n" +
                      "   '" + Master.Description + "',\n" +
                      "   '" + Master.TransportType + "',\n" +
                      "   '" + clvar._VehicleId + "',\n" +
                      "   '" + Master.CourierName + "',\n" +
                      "   '" + Master.hd_branchCode + "',\n" +
                      "   '" + Master.Destination + "',\n" +
                      "   '" + Master.hd_expressCenterCode + "',\n" +
                      "   '" + Master.hd_branchCode + "',\n" +
                      "   '" + Master.hd_zoneCode + "',\n" +
                      "   '" + Master.hd_U_ID + "',\n" +
                      "        GETDATE() ,\n" +
                      "   '" + Master.Route + "',\n" +
                      "   '" + clvar.VehicleNo + "',\n" +
                      "   '" + Master.SealNo + "',\n" +
                      "   '" + Master.FlightNo + "',\n" +
                      "   '" + clvar.FlightDepartureDate + "',\n" +
                      "   '" + Master.VehicleType + "',\n" +
                      "   '1','" + Master.hd_LocationID + "','1')";

                        headInsert_backup = "INSERT INTO MnP_Loading_backUp\n" +
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
                        "   IsMaster,LocationiD,IsAirport,insertType)\n" +
                          "VALUES\n" +
                         "  ('" + Master.lbl_loadingID + "','" + Master.Date + "',\n" +
                         "   '" + Master.Description + "',\n" +
                         "   '" + Master.TransportType + "',\n" +
                         "   '" + clvar._VehicleId + "',\n" +
                         "   '" + Master.CourierName + "',\n" +
                         "   '" + Master.hd_branchCode + "',\n" +
                         "   '" + Master.Destination + "',\n" +
                         "   '" + Master.hd_expressCenterCode + "',\n" +
                         "   '" + Master.hd_branchCode + "',\n" +
                         "   '" + Master.hd_zoneCode + "',\n" +
                         "   '" + Master.hd_U_ID + "',\n" +
                         "        GETDATE() ,\n" +
                         "   '" + Master.Route + "',\n" +
                         "   '" + clvar.VehicleNo + "',\n" +
                         "   '" + Master.SealNo + "',\n" +
                         "   '" + Master.FlightNo + "',\n" +
                         "   '" + clvar.FlightDepartureDate + "',\n" +
                         "   '" + Master.VehicleType + "',\n" +
                         "   '1','" + Master.hd_LocationID + "','1','2')";


                        if (headInsert != "")
                        {
                            List<string> queries = new List<string>();
                            queries.Add(headInsert);
                            queries.Add(headInsert_backup);

                            string result = Transactionol(queries);

                            if (result != "")
                            {
                                transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertCNInDB_method_MasterTransaction", "0", result));
                                resp = new Tuple<bool, string>(false, "Could Not Save Loading");
                                return resp;
                            }
                            else
                            {
                                Int64 LoadingID = 0;
                                Int64.TryParse(Master.lbl_loadingID.ToString(), out LoadingID);
                                if (LoadingID <= 0)
                                {
                                    resp = new Tuple<bool, string>(false, "Could Not Save Loading");
                                    return resp;
                                }
                                else
                                {

                                    Master.hd_IDChk = "1";
                                }
                            }

                        }
                    }
                    else
                    {
                        resp = new Tuple<bool, string>(false, "ID0");
                        return resp;
                    }

                }
                #endregion

                #region CN Data insertion

                if (Master.hd_IDChk.ToString() == "1")
                {

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
                "   SortOrder,ismerged)\n" +
                 "  VALUES( '" + Master.lbl_loadingID + "',\n" +
                  "         '" + cn.ConsignmentNumber + "',\n" +
                  "         '" + cn.Destination + "',\n" +
                  "         '" + Master.hd_U_ID + "',\n" +
                  "         GETDATE(),\n" +
                  "         '" + cn.Remarks + "',\n" +
                  "         '" + cn.Pieces + "',\n" +
                  "         '" + cn.Weight + "',\n" +
                  "         '" + cn.ServiceType + "',\n" +
                  "         '" + cn.ConsignmentType + "',\n" +
                  "         '" + cn.SortOrder + "','1')\n";

                    string insertCNTracking = Insert_ConsignmentTrackingHistoryFromLoading_consignment(Master, cn);

                    List<string> queries = new List<string>();

                    queries.Add(cnInsert);
                    queries.Add(insertCNTracking);

                    string result = Transactionol_tracking(queries);

                    if (result != "")
                    {
                        transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertCNInDB_method_CN_Tracking", "0", result));
                        resp = new Tuple<bool, string>(false, "Could Not save Consignment");
                        return resp;
                    }
                    else
                    {

                        resp = new Tuple<bool, string>(true, Master.lbl_loadingID);
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {
                transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertCNInDB_method", "0", ex.Message.ToString()));
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { }

            return resp;
        }

        [WebMethod]
        public static string RemoveRow(string lbl_loadingID, string BagNumber, string ConsignmentNumber, string Remove, string LoadingID_Issue)
        {
            MasterModel master = new MasterModel();
            master.lbl_loadingID = lbl_loadingID;
            master.LoadingID = LoadingID_Issue;
            transactionQueires.Add(Get_ErrorLog_Query(master, "RemoveRow_method", "1", "RemoveRow_method Initialized"));

            clvar.LoadingId = lbl_loadingID;
            if (Remove.ToUpper() == "BAG")
            {
                clvar.BagNumber = BagNumber;
                Tuple<bool, string> resp = RemoveBagFromDB(clvar, master);
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
                Tuple<bool, string> resp = RemoveCNFromDB(clvar, master);
                if (resp.Item1)
                {
                    return "OK";
                }
                else
                {
                    return resp.Item2;
                }
            }
            Transactionol(transactionQueires);
            transactionQueires.Clear();

            return "";

        }

        public static Tuple<bool, string> RemoveCNFromDB(Variable clvar, MasterModel Master)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "RemoveCNFromDB_method", "1", "RemoveCNFromDB_method Initialized"));
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            string query1 = "insert into mnp_loadingConsignment_Archieve Select * from mnp_loadingConsignment where loadingID = '" + Master.lbl_loadingID + "' and consignmentNumber = '" + clvar.ConsignmentNo + "'";

            string query = "DELETE FROM mnp_loadingConsignment where loadingID = '" + Master.lbl_loadingID + "' and consignmentNumber = '" + clvar.ConsignmentNo + "'";

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
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "RemoveCNFromDB_method_insertion_delete", "0", "Could Not Remove"));
                }
                else
                {
                    resp = new Tuple<bool, string>(true, "");

                    query = "DELETE FROM consignmentstrackinghistory where loadingnumber = '" + Master.lbl_loadingID + "' and consignmentNumber = '" + clvar.ConsignmentNo + "'";
                    cmd = new SqlCommand(query, con);
                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                transactionQueires.Add(Get_ErrorLog_Query(Master, "RemoveCNFromDB_method", "0", ex.Message));
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }

            return resp;
        }
        public static Tuple<bool, string> RemoveBagFromDB(Variable clvar, MasterModel Master)
        {

            transactionQueires.Add(Get_ErrorLog_Query(Master, "RemoveBagFromDB_method", "1", "RemoveBagFromDB_method Initialized"));
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            string query1 = "insert into MnP_LoadingBag_archieve Select * from MnP_LoadingBag where loadingID = '" + Master.lbl_loadingID + "' and Bagnumber = '" + clvar.BagNumber + "'";

            string query = "delete from mnp_loadingBag where loadingID = '" + Master.lbl_loadingID + "' and bagNumber = '" + clvar.BagNumber + "'";

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
                    transactionQueires.Add(Get_ErrorLog_Query(Master, "RemoveBagFromDB_method_insertion_delete", "0", "Could Not Remove"));
                }
                else
                {
                    resp = new Tuple<bool, string>(true, "");
                    query = "DELETE FROM consignmentstrackinghistory where loadingnumber = '" + Master.lbl_loadingID + "' and bagNumber = '" + clvar.BagNumber + "'";
                    cmd = new SqlCommand(query, con);
                    count = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                transactionQueires.Add(Get_ErrorLog_Query(Master, "RemoveBagFromDB_method", "0", ex.Message));
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }

            return resp;
        }

        [WebMethod]
        public static string RefreshTime(string a)
        {
            return DateTime.Now.ToString();
        }


        private static string[] InsertLoading_Linked(MasterModel Master, BagModel_linked[] Bag_linked, ConsignmentModel_linked[] Consignment_linked)
        {
            transactionQueires.Add(Get_ErrorLog_Query(Master, "InsertLoading_Linked_Method", "1", "InsertLoading_Linked_Method Initialized"));

            string[] resp_ = { "0", "" };
            string RandomNumber = Master.hd_U_ID.ToString() + DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace("/", "");

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
                dr["BagOrigin"] = Master.hd_branchCode.ToString();
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

                clvar.FlightDepartureDate = DateTime.Today.ToString("yyyy-MM-dd") + " " + Master.FlightDeparture;
            }

            if (Master.TransportType != "197")//dd_transporttype.SelectedValue != "197")
            {
                clvar.FlightDepartureDate = "NULL";
            }
            string mode = "";
            clvar.Remarks = RandomNumber;


            #endregion
            if (Master.Mode.ToUpper() == "NEW")
            {
                if (Master.lbl_loadingID != "")
                {

                    Tuple<bool, string> resp = UpdateLoading(Master, clvar, tblBags, tblCn);
                    if (resp.Item1)
                    {
                        resp_[0] = "1";
                        resp_[1] = resp.Item2.ToString();
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
                Tuple<bool, string> resp = UpdateLoading(Master, clvar, tblBags, tblCn);
                if (resp.Item1)
                {
                    resp_[0] = "1";
                    resp_[1] = resp.Item2.ToString();
                }
                else
                {
                    resp_[0] = "0";
                    resp_[1] = resp.Item2.ToString();
                }
            }


            return resp_;
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

        private static string Transactionol_tracking(List<string> queries)
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
                    if (count <= 0 && o == 0)
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

        private static string Get_ErrorLog_Query(MasterModel Master, string EventName, string Status, string description)
        {
            string sql = "INSERT INTO [dbo].[MnP_Loading_ERROR_LOG] \n"
               + "           ([loadingID],[UserID],[date],[EventName] \n"
               + "           ,[description],[isAirpot],[branchCode] \n"
               + "           ,[ZONECODE],[ExpressCenter],[LocationName] \n"
               + "           ,[LocationID],[loadingID_Issue],[STATUS]) \n"
               + "         VALUES \n"
               + "           ('" + Master.lbl_loadingID + "','" + Master.hd_U_ID + "', getdate(),\n"
               + "          '" + EventName + "',\n"
               + "          '" + description.Replace("'", "") + "', '1',\n"
               + "          '" + Master.hd_branchCode + "',\n"
               + "          '" + Master.hd_zoneCode + "',\n"
               + "          '" + Master.hd_expressCenterCode + "',\n"
               + "          '" + Master.hd_LocationName + "',\n"
               + "          '" + Master.hd_LocationID + "',\n"
               + "          '" + Master.LoadingID + "','" + Status + "')";
            return sql;
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
                    Response[1] = "Error: no booking found for this COD CN";
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
            string query = @"SELECT *, (select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + Consignment + @"') bypass FROM Consignment c 
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