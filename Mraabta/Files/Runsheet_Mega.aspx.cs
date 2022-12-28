using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using MRaabta.App_Code;
using Dapper;

namespace MRaabta.Files
{
    public partial class Runsheet_Mega : System.Web.UI.Page
    {
        public class CNModel
        {
            public string status { get; set; }
            public string cause { get; set; }
            public string ConsignmentNumber { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string OriginName { get; set; }
            public string DestinationName { get; set; }
            public string ConsignmentType { get; set; }
            public string ConsignmentTypeName { get; set; }
            public string SortOrder { get; set; }
            public string isNew { get; set; }
            public string removeable { get; set; }
            public string isCOD { get; set; }
        }
        public class RunsheetModel
        {
            public string status { get; set; }
            public string cause { get; set; }
            public string RunsheetNumber { get; set; }
            public string RunsheetDate { get; set; }
            public string RunsheetType { get; set; }
            public string BranchCode { get; set; }
            public string ZoneCode { get; set; }
            public string ExpressCenterCode { get; set; }
            public string CreatedBy { get; set; }
            public string RouteCode { get; set; }
            public string RouteName { get; set; }
            public string RiderCode { get; set; }
            public string RiderName { get; set; }
            public string VehicleNumber { get; set; }
            public string VehicleType { get; set; }
            public string MeterStart { get; set; }
            public string MeterEnd { get; set; }
        }
        public class Branches
        {
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
        }
        public class OnLoadReturn
        {
            public Branches[] Branches { get; set; }

        }

        CommonFunction func = new CommonFunction();
        public static Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            err_msg.Text = Errorid.Text = "";
            Errorid.Text = "";
            if (!IsPostBack)
            {
                txt_consignment.Enabled = false;
                txt_branchCode.Text = Session["BRANCHCODE"].ToString();
                txt_zoneCode.Text = Session["ZONECODE"].ToString();
                txt_user.Text = Session["U_ID"].ToString();
                txt_expressCenter.Text = Session["ExpressCenter"].ToString();

                picker1.SelectedDate = DateTime.Now.Date;
                picker1.MaxDate = DateTime.Now.Date.AddDays(1);
                picker1.MinDate = DateTime.Now.Date.AddDays(-1);

                GetRunsheetTypes();
                GetRouteNames();
                GetRiderNames();
                Get_MasterVehicle_();
                GetVehicleType();
                GetPrefix();
            }
        }
        protected void GetRunsheetTypes()
        {
            DataTable dt = func.RunsheetTypes();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ddl_runsheetType.DataSource = dt;
                    ddl_runsheetType.DataTextField = "code";
                    ddl_runsheetType.DataValueField = "Id";
                    ddl_runsheetType.DataBind();
                }
                ddl_runsheetType.SelectedValue = "12";
            }
        }
        protected void GetRouteNames()
        {
            string query = "select r.routeCode,  r.name +  ' ( '+ r.routeCode + ' )' Route from Routes r where r.bid='" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.status ='1' order by 1";
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
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ddl_route.DataSource = dt;
                    ddl_route.DataTextField = "Route";
                    ddl_route.DataValueField = "routeCode";
                    ddl_route.DataBind();
                }
            }
        }
        protected void GetPrefix()
        {
            string query = "select * from MnP_ConsignmentLengths where status = 1 and Product = 'Mega'";
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
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    cnControls.DataSource = dt;
                    cnControls.DataBind();
                }
            }
        }
        protected void GetRiderNames()
        {
            //string query = "select riderCode, FirstName + ' ' + LastName RiderName from Riders r where r.branchID = '" + txt_branchCode.Text + "' and r.status = '1'";
            string query = "select riderCode, FirstName + ' ' + LastName RiderName from Riders r where r.branchID = '" + Session["BRANCHCODE"].ToString() + "' and r.status = '1'";
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
            ddl_riders.Items.Clear();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ddl_riders.Items.Add(new ListItem { Text = "Select Rider", Value = "0" });
                    ddl_riders.DataSource = dt.DefaultView;
                    ddl_riders.DataTextField = "RiderName";
                    ddl_riders.DataValueField = "riderCode";
                    ddl_riders.DataBind();
                }
                else
                {
                    txt_riderno.Text = "";
                    ddl_riders.ClearSelection();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Rider Found')", true);
                    return;
                }
            }

        }
        public void Get_MasterVehicle_()
        {
            DataSet ds_vehicle = new DataSet();
            SqlConnection orcl = new SqlConnection(clvar.Strcon2());
            try
            {
                string sql = "select v.*, isnull(v.vehicleType, 0) VehicleType_ from rvdbo.Vehicle v where v.IsActive = '1' order by 1";
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds_vehicle);

            }
            catch (Exception)
            {

            }
            finally
            {
                orcl.Close();
            }
            if (ds_vehicle.Tables[0].Rows.Count != 0)
            {
                ddl_vehicle.DataTextField = "MakeModel";
                ddl_vehicle.DataValueField = "VehicleCode";
                ddl_vehicle.DataSource = ds_vehicle.Tables[0].DefaultView;
                ddl_vehicle.DataBind();
            }
            else
            {
                ddl_vehicle.Items.Clear();
                ddl_vehicle.Items.Add(new ListItem { Text = "Select Vehicle", Value = "0" });
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Vehicle Available')", true);


            }
            //dd_vehicle.Items.Insert(0, new ListItem("Select Vehicle ", ""));
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
                    ddl_vehicleType.DataSource = dt;
                    ddl_vehicleType.DataTextField = "TypeDesc";
                    ddl_vehicleType.DataValueField = "Typeid";
                    ddl_vehicleType.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        
        


        public static DataTable RidersByRoutes_(Cl_Variables clvar)
        {
            string sql = "SELECT r.riderCode, \n"
               + "       ro.RouteType, \n"
               + "       ro.LandMark \n"
               + "FROM   Riders r \n"
               + "       INNER JOIN Routes ro \n"
               + "            ON  ro.routeCode = r.routeCode \n"
               + "WHERE  r.routeCode = '" + clvar.routeCode + "' \n"
               + "       AND r.branchId = '" + clvar.Branch + "' \n"
               + "       AND r.[status] = '1' \n"
               + "       AND ro.[status] = '1'";

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
        public static DataTable GetRunsheetNo()
        {

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("MnP_Generate_Mega_Runsheet_Number", con);
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
        public static DataTable GetRunsheet(string runsheetnumber)
        {

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("Select * from Runsheet_backup where masterButton = '1' and runsheetnumber ='" + runsheetnumber + "'", con);
                sda.SelectCommand.CommandType = CommandType.Text;
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }


            return dt;

        }
        private static string getBranches(string BranchCode)
        {
            string name = "";
            DataTable ds = new DataTable();
            string query = " SELECT b.name FROM Branches b WHERE b.branchCode = '" + BranchCode + "' AND b.[status] = '1'";



            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    name = ds.Rows[0]["name"].ToString();
                }
                else
                {
                    name = "";
                }
                orcl.Close();
            }
            catch (Exception)
            { }

            return name;


        }
        public static DataSet GetConsignmentDetail_ds(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            string sqlString = "select ISNULL(mco.isRunsheetAllowed, 1) RunsheetAllowed,\n" +
            "       c.*,\n" +
            "       c.consignmentNumber ConNo,\n" +
            "       c.consignerAccountNo AccountNo,\n" +
            "       c.riderCode,\n" +
            "       ct.name ConType,\n" +
            "       (case when cc.rtntype = 1 then cc.rtnbranch else c.orgin end) rtnbranch,\n" +
            "       (Select b.name FROM branches b where b.branchCode= (case when cc.rtntype = 1 then cc.rtnbranch else c.orgin end) and b.status='1') rtnbranchname,\n" +
            "       c.weight,\n" +
            "       c.serviceTypeName,\n" +
            "       c.discount,\n" +
            "       c.pieces,\n" +
            "       c.consignee,\n" +
            "       c.consigneePhoneNo    ConsigneeCell,\n" +
            "       c.consigneeCNICNo     ConsigneeCNIC,\n" +
            "       c.consigner,\n" +
            "       c.consignerCellNo     ConsignerCell,\n" +
            "       c.consignerCNICNo     ConsignerCNIC,\n" +
            "       c.couponNumber        Coupon,\n" +
            "       c.decalaredValue      DeclaredValue,\n" +
            "       c.PakageContents      PackageContents,\n" +
            "       c.address             Address,\n" +
            "       c.shipperAddress,\n" +
            "       c.bookingDate,\n" +
            "       (sELECT b.name FROM branches b where b.branchCode=c.orgin and b.status='1') ORIGIN,\n" +
            "       c.originExpressCenter,\n" +
            "\n" +
            "       c.insuarancePercentage,\n" +
            "       c.totalAmount,\n" +
            "       c.chargedAmount,\n" +
            "       c.isInsured,\n" +
            "       c.dayType,\n" +
            "       c.gst, case when mp.Prefix is null and isnull(c.isapproved,0) = '0' then '1' else '0' end WRONGCN, ISNULL(mco.RTO, 0) RTO, ISNULL(mco.IsDelivered,0) isDelivered, ISNULL(mco.IsReturned,0) IsReturned1, '1' Removeable,c.cod\n" +
            "  from Consignment c\n" +
            "  inner join creditclients cc on cc.id = c.creditclientid\n" +
            "--inner join Zones z\n" +
            "--on z.zoneCode = c.zoneCode\n" +
            " LEFT OUTER JOIN (SELECT DISTINCT mpcl.prefix, mpcl.length FROM MnP_ConsignmentLengths mpcl) mp\n" +
             "            ON  LEN(c.consignmentNumber) = mp.Length\n" +
             "            AND mp.Prefix = SUBSTRING(c.consignmentNumber, 0, LEN(mp.Prefix) + 1)" +
             "\n" +
            "  left outer join ConsignmentType ct\n" +
            "    on ct.id = c.consignmentTypeId\n" +
            "  left outer join mnp_consignmentOperations mco\n" +
            "    on mco.consignmentId = c.consignmentNumber\n" +
            "\n" +
            " where c.consignmentNumber = '" + clvar.consignmentNo + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds, "Detail");
                orcl.Close();
            }
            catch (Exception)
            { }

            return ds;
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



        [WebMethod(EnableSession = true)]
        public static string[] GetRider(string routeCode, string branchCode)
        {
            string[] resp = { "", "", "", "" };

            clvar.routeCode = routeCode;
            clvar.Branch = branchCode;

            DataTable dt = RidersByRoutes_(clvar);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    resp[0] = "1";
                    resp[1] = dt.Rows[0]["routeType"].ToString();
                    resp[2] = dt.Rows[0]["LandMark"].ToString();
                    resp[3] = dt.Rows[0]["riderCode"].ToString();
                }
                else
                {
                    resp[0] = "0";
                    resp[1] = "0";
                    resp[2] = "0";
                    resp[3] = "0";
                }
            }
            else
            {
                resp[0] = "0";
                resp[1] = "0";
                resp[2] = "Rider Not Found";
                resp[3] = "0";
            }

            return resp;

        }
        public static string PrimaryCheck(string cn)
        {
            var clvar = new Cl_Variables();
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
        [WebMethod(EnableSession = true)]
        public static OnLoadReturn GetBranchesForDropDown()
        {
            OnLoadReturn resp = new OnLoadReturn();
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



            resp.Branches = branches.ToArray();
            return resp;
            //return branches.ToArray();
        }
        [WebMethod(EnableSession = true)]
        public static CNModel GetConsignmentDetail(string ConsignmentNumber, string BranchCode, string expressCenter, string User)
        {
            string bracnhName = "";
            CNModel consignment = new CNModel();
            clvar.consignmentNo = ConsignmentNumber;

            #region Primary Check By Fahad 12-oct-2020
            var rs = PrimaryCheck(ConsignmentNumber);
            if (!string.IsNullOrEmpty(rs))
            {
                consignment.status = "0";
                consignment.cause = rs;
                return consignment;
            }
            #endregion
            DataSet ds = GetConsignmentDetail_ds(clvar);
            if (ds != null)
            {
                if (ds.Tables["Detail"].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables["Detail"];
                    if (dt.Rows[0]["WrongCN"].ToString() == "1" || dt.Rows[0]["WrongCN"].ToString() == "1")
                    {
                        consignment.status = "0";
                        consignment.cause = "Wrong Consignment";
                        return consignment;
                    }
                    else if (dt.Rows[0]["RunsheetAllowed"].ToString().ToUpper() == "FALSE" || dt.Rows[0]["RunsheetAllowed"].ToString().ToUpper() == "0")
                    {
                        if (dt.Rows[0]["IsDelivered"].ToString() == "1" || dt.Rows[0]["IsDelivered"].ToString().ToUpper() == "TRUE")
                        {
                            consignment.status = "0";
                            consignment.cause = "Consignment already Delivered.";
                            return consignment;
                        }
                        else if (dt.Rows[0]["IsReturned1"].ToString() == "1" || dt.Rows[0]["IsReturned1"].ToString().ToUpper() == "TRUE")
                        {
                            consignment.status = "0";
                            consignment.cause = "Consignment already Returned.";
                            return consignment;
                        }
                        consignment.status = "0";
                        consignment.cause = "Runsheet Not Allowed for This Consignment";
                        return consignment;
                    }
                    else if ((ConsignmentNumber.StartsWith("5") && ConsignmentNumber.Length == 15) && (dt.Rows[0]["RTO"].ToString().ToUpper() == "TRUE" || dt.Rows[0]["RTO"].ToString() == "1"))
                    {
                        if (dt.Rows[0]["rtnbranch"].ToString() != HttpContext.Current.Session["BranchCode"].ToString())
                        {
                            consignment.status = "0";
                            consignment.cause = "Consignment already marked RTO (Make Runsheet at " + dt.Rows[0]["rtnbranch"].ToString() + ").";
                            return consignment;
                        }
                        else
                        {
                            consignment.status = "1";
                        }
                    }
                    else if (dt.Rows[0]["IsDelivered"].ToString() == "1" || dt.Rows[0]["IsDelivered"].ToString().ToUpper() == "TRUE")
                    {
                        consignment.status = "0";
                        consignment.cause = "Consignment already Delivered.";
                        return consignment;
                    }
                    else if (dt.Rows[0]["IsReturned1"].ToString() == "1" || dt.Rows[0]["IsReturned1"].ToString().ToUpper() == "TRUE")
                    {
                        consignment.status = "0";
                        consignment.cause = "Consignment already Returned.";
                        return consignment;
                    }
                    else
                    {
                        consignment.status = "1";
                        consignment.cause = "";
                    }


                    consignment.removeable = dt.Rows[0]["removeable"].ToString();
                    if (dt.Rows[0]["Orgin"].ToString() != "")
                    {
                        consignment.Origin = dt.Rows[0]["Orgin"].ToString();
                        consignment.OriginName = dt.Rows[0]["ORIGIN"].ToString();
                    }
                    else
                    {
                        consignment.Origin = BranchCode;
                        consignment.OriginName = getBranches(BranchCode);
                        bracnhName = consignment.OriginName;
                    }


                    if (dt.Rows[0]["Destination"].ToString() != "")
                    {
                        consignment.Destination = dt.Rows[0]["Destination"].ToString();
                        consignment.DestinationName = getBranches(consignment.Destination);
                    }
                    else
                    {
                        consignment.Destination = BranchCode;
                        consignment.DestinationName = bracnhName;
                    }


                    if (dt.Rows[0]["consignmentTypeId"].ToString() != "")
                    {
                        consignment.ConsignmentType = dt.Rows[0]["consignmentTypeId"].ToString();
                    }
                    else
                    {
                        consignment.ConsignmentType = "12";

                    }
                    if (dt.Rows[0]["ConType"].ToString() != "")
                    {
                        consignment.ConsignmentTypeName = dt.Rows[0]["ConType"].ToString();
                    }
                    else
                    {
                        consignment.ConsignmentTypeName = "Normal";

                    }

                    consignment.isNew = "0";
                    consignment.isCOD = dt.Rows[0]["Cod"].ToString();
                    return consignment;

                }
                else
                {
                    consignment.status = "0";
                    consignment.cause = "Could not Find Mega Bookig Details";
                    return consignment;
                }
            }
            else
            {
                consignment.status = "0";
                consignment.cause = "Could not Find Mega Bookig Details";
                return consignment;
            }

            return consignment;
        }
        protected void btn_saveMaster_Click(object sender, EventArgs e)
        {
            string createdBy = txt_user.Text;
            string VehicleNumber = "";
            string VehicleType = "";
            if (picker1.SelectedDate == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Date is invalid')", true);
                return;
            }


            if (ddl_riders.SelectedValue.ToString() == "0" || txt_riderno.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly Select Rider')", true);
                return;
            }

            if (ddl_route.SelectedValue.ToString() == "0" || txt_routeCode.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly Select Route'')", true);
                return;
            }



            if (ddl_vehicle.SelectedValue.ToString() != "0")
            {
                if (ddl_vehicleType.SelectedValue.ToString() == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly select vehicle type')", true);
                    return;
                }
                else
                {
                    VehicleType = ddl_vehicleType.SelectedValue.ToString();
                }
                if (txt_vehicleNumber.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kindly Insert Vehicle Number')", true);
                    return;
                }
                else
                {
                    VehicleNumber = txt_vehicleNumber.Text;
                }

            }

            clvar.RunSheetDate = picker1.SelectedDate.Value.ToString();
            clvar.Branch = txt_branchCode.Text;
            clvar.Zone = txt_zoneCode.Text;
            clvar.expresscenter = txt_expressCenter.Text;
            clvar.RunSheetTypeID = ddl_runsheetType.SelectedValue.ToString();
            //clvar.RouteDesc = ddl_route.SelectedValue.ToString();
            string MeterStart = txt_meterStart.Text;
            string MeterEnd = txt_meterEnd.Text;
            clvar.routeCode = txt_routeCode.Text;
            clvar.riderCode = txt_riderno.Text;
            clvar.Zone = txt_zoneCode.Text;
            if (txt_expressCenter.Text != "")
            {
                clvar.expresscenter = txt_expressCenter.Text;
            }
            else
            {
                clvar.expresscenter = "0";
            }


            Int64 RunsheetNumber = 0;
            DataTable RN = GetRunsheetNo();
            DataTable dt_ = GetRunsheet(RN.Rows[0][0].ToString());
            while (dt_.Rows.Count > 1)
            {
                RN = GetRunsheetNo();
                dt_ = GetRunsheet(RN.Rows[0]["Runsheetnumber"].ToString());
            }

            if (RN.Rows.Count != 0)
            {
                if (RN.Rows[0][0].ToString() != "0")
                {
                    RunsheetNumber = Int64.Parse(RN.Rows[0][0].ToString());
                }
            }
            else
            {
                txt_runsheetNumber.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not generate Runsheet Number, Kindly Contact IT')", true);
                return;
            }

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                //string insertHeader = "INSERT INTO Runsheet\n" +
                // "  (runsheetNumber,\n" +
                // "   routeCode,\n" +
                // "   createdBy,\n" +
                // "   createdOn,\n" +
                // "   runsheetDate,\n" +
                // "   branchCode,\n" +
                // "   runsheetType,\n" +
                // "   syncID,\n" +
                // "   MeterStart,\n" +
                // "   MeterEnd,\n" +
                // "   VEHICLENUMBER,\n" +
                // "   VEHICLETYPE,ridercode,expressCenterCode,zoneCode)\n" +
                // "VALUES\n" +
                // "  ('" + RunsheetNumber.ToString() + "',\n" +
                // "   '" + clvar.routeCode + "',\n" +
                // "   '" + createdBy + "',\n" +
                // "   GETDATE(),\n" +
                // "   '" + DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd") + "',\n" +
                // "   '" + clvar.Branch + "',\n" +
                // "   '" + clvar.RunSheetTypeID + "',\n" +
                // "   NEWID(),\n" +
                // "   '" + MeterStart.ToString() + "',\n" +
                // "   '" + MeterEnd.ToString() + "',\n" +
                // "   '" + VehicleNumber + "',\n" +
                // "   '" + VehicleType + "','" + clvar.riderCode + "','" + clvar.expresscenter + "','" + clvar.Zone + "')";

                string insertHeader_backup = "INSERT INTO Runsheet_backup\n" +
               "  (runsheetNumber,\n" +
               "   routeCode,\n" +
               "   createdBy,\n" +
               "   createdOn,\n" +
               "   runsheetDate,\n" +
               "   branchCode,\n" +
               "   runsheetType,\n" +
               "   syncID,\n" +
               "   MeterStart,\n" +
               "   MeterEnd,\n" +
               "   VEHICLENUMBER,\n" +
               "   VEHICLETYPE,ridercode,expressCenterCode,zoneCode,masterButton)\n" +
               "VALUES\n" +
               "  ('" + RunsheetNumber.ToString() + "',\n" +
               "   '" + clvar.routeCode.ToString().ToUpper() + "',\n" +
               "   '" + createdBy + "',\n" +
               "   GETDATE(),\n" +
               "   '" + DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd") + "',\n" +
               "   '" + clvar.Branch + "',\n" +
               "   '" + clvar.RunSheetTypeID + "',\n" +
               "   NEWID(),\n" +
               "   '" + MeterStart.ToString() + "',\n" +
               "   '" + MeterEnd.ToString() + "',\n" +
               "   '" + VehicleNumber + "',\n" +
               "   '" + VehicleType + "','" + clvar.riderCode + "','" + clvar.expresscenter + "','" + clvar.Zone + "','1')";


                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = insertHeader_backup;
                int insertCount = 0;
                insertCount = cmd.ExecuteNonQuery();


                if (insertCount <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('could not generate Runsheet.')", true);
                }
                else
                {
                    //cmd.CommandText = insertHeader_backup;
                    //insertCount = 0;
                    //insertCount = cmd.ExecuteNonQuery();
                    //if (insertCount > 0)
                    //{
                    txt_runsheetNumber.Text = RunsheetNumber.ToString();
                    //txt_runsheetNumber.Enabled = false;
                    txt_date.Enabled = false;
                    ddl_runsheetType.Enabled = false;
                    txt_routeCode.Enabled = false;
                    ddl_route.Enabled = false;
                    // dd_docType.Enabled = false;
                    txt_riderno.ReadOnly = true;
                    ddl_riders.Enabled = false;
                    txt_vehicleNumber.ReadOnly = true;
                    ddl_vehicle.Enabled = false;
                    ddl_vehicleType.Enabled = false;
                    txt_meterEnd.Enabled = false;
                    txt_meterStart.Enabled = false;
                    txt_laskMark.Enabled = false;
                    btn_saveMaster.Enabled = false;
                    picker1.Enabled = false;
                    if (txt_runsheetNumber.Text != "")
                    {
                        txt_consignment.Enabled = true;
                    }
                    else
                    {
                        txt_consignment.Enabled = false;
                    }
                    txt_consignment.Focus();

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "PopulateBranches()", true);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "PopulateBranches", "PopulateBranches()", true);
                    // }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('" + ex.Message.ToString() + "');window.location ='Runsheet_Mega.aspx';",
                true);
            }
            finally { con.Close(); }
        }

        #region Runsheet Saving

        [WebMethod(EnableSession = true)]
        public static string[] SaveRunsheet(RunsheetModel Header, CNModel[] Details)
        {
            string[] resp = { "", "" };

            DataTable newDt = new DataTable();//dt.Clone();
            newDt.Columns.Add("ConNo", typeof(string));
            newDt.Columns.Add("ORIGIN", typeof(string));
            newDt.Columns.Add("Destination", typeof(string));
            newDt.Columns.Add("ConType", typeof(string));
            newDt.Columns.Add("IsNew", typeof(string));
            newDt.Columns.Add("OriginName", typeof(string));
            newDt.Columns.Add("ISCOD", typeof(string));

            newDt.Columns.Add("Uniqueno", typeof(string));

            newDt.AcceptChanges();
            //newDt = dt;
            foreach (CNModel row in Details)
            {
                if (row.isNew == "1")
                {
                    DataRow dr = newDt.NewRow();
                    dr["ConNo"] = row.ConsignmentNumber;
                    dr["Origin"] = row.Origin;
                    dr["Destination"] = row.Destination;
                    dr["ConType"] = row.ConsignmentType;
                    dr["IsNew"] = row.isNew;
                    dr["OriginName"] = row.OriginName;
                    dr["ISCOD"] = row.isCOD;
                    newDt.Rows.Add(dr);
                    newDt.AcceptChanges();
                }
            }

            clvar.CustomerClientID = "330140";
            clvar.AccountNo = "4D1";
            clvar.riderCode = "";


            string error = "";
            if (newDt.Rows.Count > 0)
            {
                resp[0] = "0";
                resp[1] = "Could not Insert New CNs Error: " + error + "";
                return resp;
            }
            clvar.RunSheetDate = Header.RunsheetDate;
            clvar.RunSheetTypeID = Header.RunsheetType;
            clvar.routeCode = Header.RouteCode;
            clvar.riderCode = Header.RiderCode;
            clvar.RunsheetNumber = Header.RunsheetNumber;
            clvar.createdBy = Header.CreatedBy;
            clvar.Zone = Header.ZoneCode;
            clvar.expresscenter = Header.ExpressCenterCode;
            clvar.Branch = Header.BranchCode;

            string[,] arr = new string[Details.Count(), 2];

            error = "";

            List<string> ResponseList = new List<string>();
            Int64 MeterStart = -1;
            Int64 MeterEnd = -1;
            Int64.TryParse(Header.MeterStart, out MeterStart);
            Int64.TryParse(Header.MeterEnd, out MeterEnd);

            ResponseList = GenerateRunsheet_new_2(clvar, Header.VehicleNumber, Header.VehicleType, MeterStart, MeterEnd, Details, Header);


            if (ResponseList[0] == "1")
            {

                SqlConnection con = new SqlConnection(clvar.Strcon());
                int insertCount = 0;

                string insertHeader_Master = "INSERT INTO Runsheet (runsheetNumber, routeCode, createdBy, createdOn, runsheetDate, branchCode, runsheetType, syncID, MeterStart, MeterEnd,\n" +
              "   VEHICLENUMBER, VEHICLETYPE,ridercode,expressCenterCode,zoneCode)\n" +
              "VALUES ('" + Header.RunsheetNumber.ToString() + "',\n" +
              "   '" + Header.RouteCode.ToString().ToUpper() + "',\n" +
              "   '" + Header.CreatedBy + "',\n" +
              "   GETDATE(),\n" +
              "   '" + DateTime.Parse(Header.RunsheetDate).ToString("yyyy-MM-dd") + "',\n" +
              "   '" + Header.BranchCode + "',\n" +
              "   '" + Header.RunsheetType + "',\n" +
              "   NEWID(),\n" +
              "   '" + Header.MeterStart.ToString() + "',\n" +
              "   '" + Header.MeterEnd.ToString() + "',\n" +
              "   '" + Header.VehicleNumber + "',\n" +
              "   '" + Header.VehicleType + "','" + Header.RiderCode + "','" + Header.ExpressCenterCode + "','" + Header.ZoneCode + "')";

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = insertHeader_Master;
                    insertCount = cmd.ExecuteNonQuery();
                    if (insertCount != 1)
                    {
                        resp[0] = "0";
                        resp[1] = "Runsheet Generated. Error: On Insertion, Contact IT!";
                    }
                    else
                    {
                        resp[0] = "1";
                        resp[1] = ResponseList[1].ToString();
                    }
                }
                catch (Exception ex)
                {
                    resp[0] = "0";
                    resp[1] = "Runsheet Generated. Error: " + ex.Message + ", Contact IT!";
                }
                finally { con.Close(); }
            }
            else
            {
                resp[0] = "0";
                resp[1] = "Could Not Generate Runsheet. Error: " + ResponseList[1] + "";
            }

            return resp;
        }

        public static List<string> GenerateRunsheet_new_2(Cl_Variables clvar, string VehicleNumber, string VehicleType, Int64 MeterStart, Int64 MeterEnd, CNModel[] details, RunsheetModel Header)
        {
            List<string> resp = new List<string>();
            Int64 RunsheetNumber = Int64.Parse(Header.RunsheetNumber);
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;

            sqlcmd.CommandType = CommandType.Text;
            try
            {
                CommonFunction func = new CommonFunction();
                clvar.Branch = Header.BranchCode;
                string currentLocation = HttpContext.Current.Session["LocationName"].ToString();

                DataTable Rdetails = new DataTable();
                Rdetails.Columns.Add("RunsheetNumber");
                Rdetails.Columns.Add("ConsignmentNumber");
                Rdetails.Columns.Add("Cod");
                Rdetails.Columns.Add("SortOrder");


                int j = clvar.ClvarListStr.Count - 1;
                string cnNumbers = "";
                for (int i = 0; i < details.Count(); i++)
                    cnNumbers += "'" + details[i].ConsignmentNumber + "'";
                cnNumbers = cnNumbers.Replace("''", "','");

                String CNQuery = "select '''' + STRING_AGG(CONVERT(NVARCHAR(max), consignmentNumber), ''',''') +'''' as CNs from Consignment where consignmentNumber in (" + cnNumbers + ")";
                DataTable DtCNs = new DataTable();
                SqlConnection con = new SqlConnection(clvar.Strcon());
                try
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(CNQuery, con);
                    sda.Fill(DtCNs);

                }
                catch (Exception ex)
                { }
                finally { con.Close(); }
                if (DtCNs.Rows.Count>0)
                {
                    cnNumbers = DtCNs.Rows[0][0].ToString();
                }
                else
                {
                    resp.Add("2");
                    resp.Add("Error generating Runsheet. Please try Again.");
                    trans.Rollback();
                    sqlcon.Close();
                    return resp;
                }

                for (int i = 0; i < details.Count(); i++)
                {
                    if (cnNumbers.Contains(details[i].ConsignmentNumber.ToString()))
                    {
                        DataRow dr = Rdetails.NewRow();
                        dr[0] = Header.RunsheetNumber;
                        dr[1] = details[i].ConsignmentNumber;
                        dr[2] = details[i].isCOD;
                        dr[3] = (i + 1).ToString();
                        Rdetails.Rows.Add(dr);
                    }
                }

                string Condition = "";
                if (Header.RunsheetNumber.Trim() != "")
                    Condition = " AND rc.RunsheetNumber != '" + RunsheetNumber + "'\n";
                string sqlString = "SELECT rc.runsheetNumber, rc.branchcode, rc.consignmentNumber, DATEDIFF(Minute, rc.createdOn, GETDATE()) FROM RunsheetConsignment rc\n" +
                " WHERE rc.consignmentNumber IN (" + cnNumbers + ") AND rc.branchcode != '" + clvar.Branch + "' AND DATEDIFF(SECOND, rc.createdOn, GETDATE()) <= 6000 " +
                 Condition;


                DataTable prevRunsheets = new DataTable();
                try
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                    sda.Fill(prevRunsheets);

                }
                catch (Exception ex)
                { }
                finally { con.Close(); }
                if (prevRunsheets != null)
                {
                    if (prevRunsheets.Rows.Count > 0)
                    {
                        resp.Add("2");
                        string errorNumbers = "";
                        DataTable distinctCNS = prevRunsheets.DefaultView.ToTable(true, "ConsignmentNumber");

                        foreach (DataRow dr in distinctCNS.Rows)
                        {
                            errorNumbers += dr["ConsignmentNumber"].ToString() + ",";
                        }
                        resp.Add("Runsheets for the following Consignment Number(s) " + errorNumbers + " have been made within last 5 minutes");
                        trans.Rollback();
                        sqlcon.Close();
                        return resp;
                    }
                }
                else
                {
                    resp.Add("2");
                    resp.Add("Error generating Runsheet. Please try Again.");
                    trans.Rollback();
                    sqlcon.Close();
                    return resp;
                }

                string GivenTo = "UPDATE RUNSHEETCONSIGNMENT SET REASON = '204', STATUS = '56', GivenToRider = '" + Header.RiderCode + "' " +
                    " WHERE consignmentNumber IN (" + cnNumbers + ") AND branchcode = '" + Header.BranchCode + "' AND Reason IS NULL";

                string insertDetails = "INSERT INTO RUNSHEETCONSIGNMENT (RUNSHEETNUMBER, CONSIGNMENTNUMBER, CREATEDBY, CREATEDON, STATUS, SORTORDER, BRANCHCODE,ROUTECODE,COD)\n";

                for (int i = 0; i < Rdetails.Rows.Count - 1; i++)
                {
                    insertDetails += "  SELECT '" + RunsheetNumber.ToString() + "',\n" +
                                    "          '" + Rdetails.Rows[i]["ConsignmentNumber"].ToString().Trim() + "',\n" +
                                    "          '" + Header.CreatedBy + "',\n" +
                                    "          GETDATE(),\n" +
                                    "          '56',\n" +
                                    "          '" + Rdetails.Rows[i]["SortOrder"].ToString() + "',\n" +
                                    "          '" + Header.BranchCode + "',\n" +
                                    "          '" + Header.RouteCode.ToString().ToUpper() + "','" + Rdetails.Rows[i]["Cod"].ToString() + "'\n" +
                                    "   UNION ALL";

                }
                insertDetails += "  SELECT '" + RunsheetNumber.ToString() + "',\n" +
                                "          '" + Rdetails.Rows[Rdetails.Rows.Count - 1]["ConsignmentNumber"].ToString().Trim() + "',\n" +
                                "          '" + Header.CreatedBy + "',\n" +
                                "          GETDATE(),\n" +
                                "          '56',\n" +
                                "          '" + Rdetails.Rows[Rdetails.Rows.Count - 1]["SortOrder"].ToString() + "',\n" +
                                "          '" + Header.BranchCode + "',\n" +
                                "          '" + Header.RouteCode.ToString().ToUpper() + "','" + Rdetails.Rows[Rdetails.Rows.Count - 1]["Cod"].ToString() + "'\n";

                string insertRiderRunsheet = "INSERT INTO RiderRunsheet\n" +
                "  (ridercode, runsheetNumber, createdBy, createdOn, expIdTemp)\n" +
                "  SELECT TOP(1) '" + Header.RiderCode + "',\n" +
                "         '" + RunsheetNumber.ToString() + "',\n" +
                "         '" + Header.CreatedBy + "',\n" +
                "         GETDATE(),\n" +
                "         r.expressCenterId\n" +
                "    FROM Riders r\n" +
                "   WHERE r.ridercode = '" + Header.RiderCode + "'\n" +
                "     AND r.branchId = '" + Header.BranchCode + "'\n" +
                "     AND r.status = '1'";

                sqlcmd.CommandText = GivenTo;
                sqlcmd.ExecuteNonQuery();


                sqlcmd.CommandText = insertDetails;
                int insertCount = 0;
                insertCount = sqlcmd.ExecuteNonQuery();
                if (insertCount <= 0)
                {
                    resp.Add("2");
                    resp.Add("Could not Save Runsheet Details.");
                    trans.Rollback();
                    sqlcon.Close();
                    return resp;
                }
                sqlcmd.CommandText = insertRiderRunsheet;
                insertCount = 0;
                insertCount = sqlcmd.ExecuteNonQuery();
                if (insertCount <= 0)
                {
                    resp.Add("2");
                    resp.Add("Could not Save Runsheet Rider.");
                    trans.Rollback();
                    sqlcon.Close();
                    return resp;
                }
                trans.Commit();
                resp.Add("1");
                resp.Add(RunsheetNumber.ToString());
                sqlcon.Close();

                return resp;
            }
            catch (Exception ex)
            {
                resp.Add("1");
                resp.Add(ex.Message);
                trans.Rollback();
                sqlcon.Close();
                return resp;
            }
            return resp;
        }
       
        public static DataSet check_CN(string COnsignmentno, RunsheetModel RM)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());

            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select * from consignment where consignmentnumber ='" + COnsignmentno + "'";
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(ds);
                cmd.ExecuteNonQuery();
            }
            catch (Exception E)
            { }
            finally
            { con.Close(); }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["orgin"].ToString() == "")
                {
                    string query = "";

                    query = " UPDATE Consignment Set Orgin ='" + RM.BranchCode + "', Destination ='" + RM.BranchCode + "', CreditClientId ='330140', ConsignerAccountNo ='4d1', weight='0.5' , pieces='1',consignmentTypeId ='12' where consignmentnumber ='" + COnsignmentno + "' ";
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return ds;
        }


        public static DataTable GetPrevRunsheets(string branchCode, string RunsheetNumber, string ConsignmentNumbers)
        {
            string Condition = "";
            if (RunsheetNumber.Trim() != "")
            {
                Condition = " AND rc.RunsheetNumber != '" + RunsheetNumber + "'\n";
            }
            else
            {
                Condition = "";
            }
            string sqlString = "SELECT rc.runsheetNumber,\n" +
            "       rc.branchcode,\n" +
            "       rc.consignmentNumber,\n" +
            "       DATEDIFF(Minute, rc.createdOn, GETDATE())\n" +
            "  FROM RunsheetConsignment rc\n" +
            " WHERE rc.consignmentNumber IN (" + ConsignmentNumbers + ")\n" +
            "   AND rc.branchcode != '" + branchCode + "'\n" +
             Condition +
            "   AND DATEDIFF(SECOND, rc.createdOn, GETDATE()) <= 6000";


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

        private static DataTable get_GivenTo(string cnNumbers, string branchcode)
        {
            string sql = "SELECT * FROM   RunsheetConsignment rc WHERE  rc.consignmentNumber IN  (" + cnNumbers + ")  AND (rc.Reason = '' OR rc.Reason IS NULL) \n"
                 + "       AND rc.branchcode = '" + branchcode + "' order by createdon desc";

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
        #endregion
    }
}