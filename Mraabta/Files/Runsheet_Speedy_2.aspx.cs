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
    public partial class Runsheet_Speedy_2 : System.Web.UI.Page
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
                SqlDataAdapter sda = new SqlDataAdapter("MnP_GenerateRunsheet_Number_check", con);
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

            string query = "SELECT * FROM MNP_ConsignmentLengths where status = '1'";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds, "Detail");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    oda = new SqlDataAdapter(query, orcl);
                    oda.Fill(ds, "CnLengths");
                }
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
        #region Controls
        [WebMethod]
        public static string[][] CheckCODControls(string cn)
        {
            List<string[]> resp = new List<string[]>();
            string[] Response = { "", "" };

            string ConsignmentNo = cn;

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
            if (ConsignmentNo.StartsWith("5") && ConsignmentNo.Length == 15)
            {
                string Reason = "";
                DataTable BookingDT = CheckConsignmentBooking(ConsignmentNo);
                DataTable FirstProcessDT = CheckFirstProcessOrigin(ConsignmentNo);
                string status = "true";
                if (BookingDT.Rows.Count > 0)
                {
                    if (BookingDT.Rows[0]["bypass"].ToString() == "0")
                    {
                        //if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                        if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                        {
                            status = "false";
                            Reason = "Alert: Once reached destination can only move with Return NCI";
                        }
                        //else if (HttpContext.Current.Session["BranchCode"].ToString() == BookingDT.Rows[0]["orgin"].ToString() && (BookingDT.Rows[0]["lastReason"].ToString() == "70" || BookingDT.Rows[0]["lastReason"].ToString() == "58"))
                        //{
                        //    status = "false";
                        //    Reason = "Alert: Miss Route Shipment cannot be processed at Origin";
                        //}
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
            //string query = @"SELECT c.consignmentNumber, (select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + Consignment + @"') bypass, c.consigner, c.consignee, c.couponNumber, c.customerType, c.orgin, isnull(xxb.branchcode, c.destination) as destination, c.pieces, c.serviceTypeName, c.creditClientId, c.weight, c.cod, c.address, c.status, c.totalAmount, c.zoneCode, c.branchCode, c.shipperAddress, c.transactionNumber, c.otherCharges, c.routeCode, c.docPouchNo, c.consignerPhoneNo, c.consignerCellNo, c.consignerCNICNo, c.consignerAccountNo, c.consignerEmail, c.docIsHomeDelivery, c.cutOffTime, c.destinationCountryCode, c.decalaredValue, c.insuarancePercentage, c.consignmentScreen, c.isInsured, c.isReturned, c.consigneeCNICNo, c.cutOffTimeShift, c.bookingDate, c.cnClientType, c.syncState, c.syncId, c.destinationExpressCenterCode, c.isApproved, c.deliveryStatus, c.dayType, c.originExpressCenter, c.isPriceComputed, c.isNormalTariffApplied, c.receivedFromRider, c.chargedAmount, c.misRouted, c.accountReceivingDate, c.IsInvoiced, c.ispayable, c.CorrectCN, c.cnReason, c.Region, c.DestinationZone, c.ZoningCriteria, c.Zoning, c.DenseWeight, c.Zoning_Criteria_origin, c.Zoning_origin, c.statusSync, c.paidon, c.InstrumentMode, c.InstrumentNumber, c.ConsignerCostCenter, c.Address2, c.Phone2, c.PackageContent2, c.InsertType, c.DiscountID, c.DiscountApplied, c.DiscountGST, c.VolWeight, c.origin_country, c.import, c.locationID, xxb.* FROM Consignment c 
            //inner join (select consignmentnumber, max(branchcode) as branchcode, sum(AtDest) as AtDest, sum(allowRTN) as allowRTN from (
            //select '" + Consignment + @"' consignmentnumber, '' branchcode, 0 AtDest, 0 allowRTN union
            //select consignmentnumber, branchcode, case when Reason in ('70', '58') then 0 else 1 end AtDest, 0 as allowRTN from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' 
            //and createdOn = (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' ) union
            //select consignmentnumber, '' branchcode, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as allowRTN 
            //from MNP_NCI_Request where calltrack = 2 and consignmentnumber ='" + Consignment + @"' group by consignmentnumber) xb
            //group by consignmentnumber) xxb on xxb.consignmentNumber = c.consignmentNumber
            //WHERE c.consignmentNumber = '" + Consignment + "'";

            string query = @"SELECT c.consignmentNumber, (select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + Consignment + @"') bypass, c.consigner, c.consignee, 
            c.couponNumber, c.customerType, c.orgin, isnull(xxb.branchcode, c.destination) as destination, c.pieces, c.serviceTypeName, c.creditClientId, c.weight, c.cod, c.address, c.status, 
            c.totalAmount, c.zoneCode, c.branchCode, c.shipperAddress, c.transactionNumber, c.otherCharges, c.routeCode, c.docPouchNo, c.consignerPhoneNo, c.consignerCellNo, c.consignerCNICNo, 
            c.consignerAccountNo, c.consignerEmail, c.docIsHomeDelivery, c.cutOffTime, c.destinationCountryCode, c.decalaredValue, c.insuarancePercentage, c.consignmentScreen, c.isInsured, 
            c.isReturned, c.consigneeCNICNo, c.cutOffTimeShift, c.bookingDate, c.cnClientType, c.syncState, c.syncId, c.destinationExpressCenterCode, c.isApproved, c.deliveryStatus, c.dayType, 
            c.originExpressCenter, c.isPriceComputed, c.isNormalTariffApplied, c.receivedFromRider, c.chargedAmount, c.misRouted, c.accountReceivingDate, c.IsInvoiced, c.ispayable, c.CorrectCN, 
            c.cnReason, c.Region, c.DestinationZone, c.ZoningCriteria, c.Zoning, c.DenseWeight, c.Zoning_Criteria_origin, c.Zoning_origin, c.statusSync, c.paidon, c.InstrumentMode, 
            c.InstrumentNumber, c.ConsignerCostCenter, c.Address2, c.Phone2, c.PackageContent2, c.InsertType, c.DiscountID, c.DiscountApplied, c.DiscountGST, c.VolWeight, c.origin_country, 
            c.import, c.locationID, xxb.* FROM Consignment c 
            inner join (
	            select consignmentnumber, max(branchcode) as branchcode, sum(AtDest) as AtDest, sum(allowRTN) as allowRTN, sum(lastReason) as lastReason from (
		            select '" + Consignment + @"' consignmentnumber, '' branchcode, 0 AtDest, 0 allowRTN, 0 lastReason union
		            select consignmentnumber, branchcode, case when Reason in ('70', '58') then 0 else 1 end AtDest, 0 as allowRTN, Reason as lastReason
			            from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' and createdOn = 
			            (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' ) union
		            select consignmentnumber, '' branchcode, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as allowRTN, 0 as lastReason 
			            from MNP_NCI_Request where calltrack = 2 and consignmentnumber ='" + Consignment + @"' group by consignmentnumber
	            ) xb
	            group by consignmentnumber
            ) xxb on xxb.consignmentNumber = c.consignmentNumber
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
        public static string alreadyRTS_DLV(string cn)
        {
            Cl_Variables clvar = new Cl_Variables();
            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                try
                {
                    var query = $"select top (1)'Consignment already Marked Delivered or Returned' AS Msg from Mnp_ConsignmentOperations where ConsignmentId = '{cn}' and (IsDelivered = 1 or IsReturned = 1)";
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
        #endregion
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
                    if (ConsignmentNumber.StartsWith("5") && ConsignmentNumber.Length == 15)
                    {
                        DataTable BookingDT = CheckConsignmentBooking(ConsignmentNumber);
                        DataTable FirstProcessDT = CheckFirstProcessOrigin(ConsignmentNumber);
                        if (BookingDT.Rows.Count > 0)
                        {
                            if (BookingDT.Rows[0]["bypass"].ToString() == "0")
                            {
                                //if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                                if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                                {
                                    consignment.status = "0";
                                    consignment.cause = "Once reached destination can only move with Return NCI";
                                    return consignment;
                                }
                                else if (FirstProcessDT.Rows.Count > 0 || BookingDT.Rows[0]["isApproved"].ToString() == "True")
                                {
                                    if (BookingDT.Rows[0]["status"].ToString() == "9")
                                    {
                                        consignment.status = "0";
                                        consignment.cause = "Consignment is Void perform Arrival";
                                        return consignment;
                                    }
                                }
                                else
                                {
                                    consignment.status = "0";
                                    consignment.cause = "First Process Must be at orign";
                                    return consignment;
                                }
                            }
                        }
                        else
                        {
                            consignment.status = "0";
                            consignment.cause = "No booking found for this COD CN";
                            return consignment;
                        }
                    }


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
                else if (ds.Tables["CnLengths"].Rows.Count > 0)
                {
                    #region Prefix and Length Validation in Case of New Consignment
                    bool found = false;
                    bool correctLength = false;
                    foreach (DataRow row in ds.Tables["CnLengths"].Rows)
                    {
                        string prefix = row["Prefix"].ToString();
                        int length = int.Parse(row["Length"].ToString());
                        if (prefix.Length > ConsignmentNumber.Length)
                        {
                            continue;
                        }
                        if (ConsignmentNumber.Substring(0, prefix.Length) == prefix)
                        {
                            found = true;
                            if (ConsignmentNumber.Length == length)
                            {
                                correctLength = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Prefix.')", true);
                        //Errorid.Text = "Invalid Prefix.";
                        //txt_cnNumber.Text = "";
                        //txt_cnNumber.Focus();
                        //return;

                        consignment.status = "0";
                        consignment.cause = "Invalid Prefix.";
                        return consignment;
                    }
                    if (!correctLength)
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Length.')", true);
                        //Errorid.Text = "Invalid Prefix.";
                        //txt_cnNumber.Text = "";
                        //txt_cnNumber.Focus();
                        //return;

                        consignment.status = "0";
                        consignment.cause = "Invalid Length.";
                        return consignment;
                    }
                    #endregion


                    consignment.status = "1";
                    consignment.cause = "";

                    consignment.ConsignmentNumber = ConsignmentNumber;
                    consignment.Origin = BranchCode;
                    consignment.Destination = BranchCode;
                    consignment.OriginName = "";
                    consignment.DestinationName = "";
                    consignment.ConsignmentTypeName = "NORMAL";
                    consignment.ConsignmentType = "12";
                    consignment.isNew = "1";
                    consignment.isCOD = "0";
                    return consignment;
                }
                else
                {
                    consignment.status = "0";
                    consignment.cause = "Error in Fetching Details";
                    return consignment;
                }
            }
            else
            {
                consignment.status = "0";
                consignment.cause = "Could not Find Details";
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
            DataTable dt_ = GetRunsheet(RN.Rows[0]["Runsheetnumber"].ToString());
            while (dt_.Rows.Count > 1)
            {
                RN = GetRunsheetNo();
                dt_ = GetRunsheet(RN.Rows[0]["Runsheetnumber"].ToString());
            }

            if (RN.Rows.Count != 0)
            {
                if (RN.Rows[0]["codevalue"].ToString() != "0")
                {
                    RunsheetNumber = Int64.Parse(RN.Rows[0]["runsheetnumber"].ToString());
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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ould not generate Runsheet.')", true);
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
                "alert('" + ex.Message.ToString() + "');window.location ='Runsheet_Speedy_2.aspx';",
                true);
            }
            finally { con.Close(); }
        }

        #region Runsheet Saving

        [WebMethod(EnableSession = true)]
        public static string[] SaveRunsheet(RunsheetModel Header, CNModel[] Details)
        {

            int l = 0;
            string[] resp = { "", "" };
            //string tempMaster = @"INSERT INTO tempRunsheet (RunsheetNumber,status,cause,RunsheetDate,branchCode,RunsheetType,ZoneCode,ExpressCenterCode,CreatedBy,RouteCode,RouteName,RiderCode ,RiderName,VehicleNumber,VehicleType,MeterStart,MeterEnd,entryOn) 
            //    VALUES  ( '" + Header.RunsheetNumber + "', '" + Header.status + "', '" + Header.cause + "', '" + Header.RunsheetDate + "', '" + Header.BranchCode + "', '" + Header.RunsheetType + "', '" + Header.ZoneCode + "', '" + Header.ExpressCenterCode + "', '" + Header.CreatedBy + "', '" + Header.RouteCode + "', '" + Header.RouteName + "', '" + Header.RiderCode + "', '" + Header.RiderName + "', '" + Header.VehicleNumber + "', '" + Header.VehicleType + "', '" + Header.MeterStart + "', '" + Header.MeterEnd + "', getdate() ); SELECT SCOPE_IDENTITY() ";
            //int id = 0;
            //try
            //{
            //    SqlConnection ccon = new SqlConnection(clvar.Strcon());
            //    SqlTransaction trans;
            //    ccon.Open();
            //    SqlCommand sqlcmd = new SqlCommand();
            //    sqlcmd.Connection = ccon;
            //    trans = ccon.BeginTransaction();
            //    sqlcmd.Transaction = trans;
            //    sqlcmd.CommandType = CommandType.Text;
            //    try
            //    {
            //        sqlcmd.CommandText = tempMaster;
            //        id = Convert.ToInt32(sqlcmd.ExecuteScalar());
            //        string tempDetail = "insert into TempRunsheetCN (RunsheetID, RunsheetNumber, ConsignmentNumber, status, cause, Origin, Destination, OriginName, DestinationName, ConsignmentType, ConsignmentTypeName, SortOrder, isNew, removeable, isCod, entryOn, userID) values ";
            //        foreach (CNModel row in Details)
            //        {
            //            string sortOrder = "0";
            //            sortOrder = row.SortOrder == "" ? "0" : row.SortOrder;
            //            tempDetail += " (" + id.ToString() + ", '" + Header.RunsheetNumber + "', '" + row.ConsignmentNumber + "', '" + row.status + "', '" + row.cause + "', '" + row.Origin + "', '" + row.Destination + "', '" + row.OriginName + "', '" + row.DestinationName + "', '" + row.ConsignmentType + "', '" + row.ConsignmentTypeName + "', '" + sortOrder + "', '" + row.isNew + "', '" + row.removeable + "', '" + row.isCOD + "', getdate(), " + Header.CreatedBy + "), ";
            //        }
                        
            //        tempDetail = tempDetail.Substring(0, tempDetail.Length - 2);

            //        sqlcmd.CommandText = tempDetail;
            //        sqlcmd.ExecuteNonQuery();
            //        trans.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        throw ex;
            //    }
            //    finally { ccon.Close(); }
            //}
            //catch (Exception ex)
            //{
            //    resp[0] = "0";
            //    resp[1] = "Error: " + ex.Message + "";
            //    return resp;
            //}

            DataTable newDt = new DataTable();//dt.Clone();
            try
            {
                newDt.Columns.Add("ConNo", typeof(string));
                newDt.Columns.Add("ORIGIN", typeof(string));
                newDt.Columns.Add("Destination", typeof(string));
                newDt.Columns.Add("ConType", typeof(string));
                newDt.Columns.Add("IsNew", typeof(string));
                newDt.Columns.Add("OriginName", typeof(string));
                newDt.Columns.Add("ISCOD", typeof(string));
                newDt.Columns.Add("Uniqueno", typeof(string));
                newDt.AcceptChanges();
            }
            catch (Exception ex)
            {
                resp[0] = "0";
                resp[1] = "Could not Make New CNs Error: " + ex.Message + "";
                return resp;
            }
            
            try
            {
                foreach (CNModel row in Details)
                {
                    if (row.isNew == "1")
                    {
                        l++;
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
                    clvar.Weight = 0.5f;
                    clvar.pieces = 1;
                    error = InsertConsignmentsFromRunsheet(clvar, newDt, Header);
                }

                if (!(error.Contains("NOT OK")))
                {

                }
                else
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

                            if (Header.RunsheetType == "12")
                            {
                                route_SMS(ResponseList[1].ToString(), Header.CreatedBy, Header.BranchCode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        resp[0] = "0";
                        resp[1] = "Runsheet Generated. Error1: " + ex.Message + "-" + l.ToString() + "-" + newDt.Rows.Count.ToString() + ", Contact IT!";
                    }
                    finally { con.Close(); }
                }
                else
                {
                    resp[0] = "0";
                    resp[1] = "Could Not Generate Runsheet. Error: " + ResponseList[1] + "";
                }

            }
            catch (Exception ex)
            {
                resp[0] = "0";
                resp[1] = "Runsheet Generated. Error2: " + ex.Message + "-" + l.ToString() + "-" + newDt.Rows.Count.ToString() + ", Contact IT!";
            }
            return resp;
        }

        public static string InsertConsignmentsFromRunsheet(Cl_Variables clvar, DataTable dt, RunsheetModel Header)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            string cnqry = "", cnNumbers = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cnqry += "select '" + dt.Rows[i]["ConNo"].ToString() + "' as ConNo from MnP_ConsignmentLengths where status = 1 and length= len('" + dt.Rows[i]["ConNo"].ToString() + "') and prefix = left('" + dt.Rows[i]["ConNo"].ToString() + "', len(prefix)) ";
                if (i < dt.Rows.Count - 1)
                {
                    cnqry += " union ";
                }
            }
            DataSet dsPassCN = new DataSet();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = cnqry;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dsPassCN);
                cmd.ExecuteNonQuery();
            }
            catch (Exception E)
            { }
            finally
            { con.Close(); }


            for (int i = 0; i < dsPassCN.Tables[0].Rows.Count; i++)
            {
                cnNumbers += "'" + dsPassCN.Tables[0].Rows[i]["ConNo"].ToString() + "'";
            }
            cnNumbers = cnNumbers.Replace("''", "','");


            DataSet dsExistingCN = new DataSet();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select '''' + STRING_AGG(CONVERT(NVARCHAR(max), consignmentNumber), ''',''') +'''' as CNs from consignment where consignmentnumber in ( " + cnNumbers + " )";
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dsExistingCN);
                cmd.ExecuteNonQuery();
            }
            catch (Exception E)
            { }
            finally
            { con.Close(); }
            string CCN = "";
            //if (dsExistingCN.Tables[0].Rows.Count>0 && dsExistingCN.Tables[0].Rows[0][0].ToString().Length > 0)
            //{
            //    CCN = dsExistingCN.Tables[0].Rows[0][0].ToString();
            //    string qry = "";
            
            //    qry = " UPDATE Consignment Set Orgin ='" + Header.BranchCode + "', Destination ='" + Header.BranchCode + "', CreditClientId ='330140', ConsignerAccountNo ='4d1', weight='0.5' , pieces='1',consignmentTypeId ='12' where consignmentnumber in (" + CCN    + ") ";
            //    SqlCommand cmd = new SqlCommand();
            //    con.Open();
            //    cmd.Connection = con;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.CommandText = qry;
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            string trackQuery = "insert into ConsignmentsTrackingHistory (consignmentNumber, stateID, currentLocation, transactionTime, internationalRemarks)\n";
            int count = 0;
            string query = "";
            query = " INSERT INTO CONSIGNMENT (ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, weight , pieces, syncid,bookingDate,createdOn,zoneCode, branchCode, serviceTypeName, consignmentTypeId) ";
            int COUNT_ = 0;
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                //DataSet dr = check_CN(dt.Rows[i]["ConNo"].ToString(), Header);

                if (!(CCN.Contains(dt.Rows[i]["ConNo"].ToString())))
                {
                    COUNT_ = 1;
                    query += " SELECT '" + dt.Rows[i]["ConNo"].ToString() + "', '" + Header.BranchCode + "', '" + dt.Rows[i]["Destination"].ToString() + "', '330140', '4D1', '0.5', '1' , NewID(), GETDATE(), GETDATE(), '" + Header.ZoneCode + "',\n" +
                                "   '" + Header.BranchCode + "', 'overnight', '12'\n" +
                                " UNION ALL";

                    trackQuery += " SELECT   '" + dt.Rows[i]["CONNO"].ToString() + "',\n" +
                                   "   '1',\n" +
                                   "   '" + HttpContext.Current.Session["LocationName"].ToString() + "',\n" +
                                   "   GETDATE(), '-'\n UNION ALL";
                }
            }
            int j = dt.Rows.Count - 1;

            //DataSet dr_ = check_CN(dt.Rows[j]["ConNo"].ToString(), Header);
            //if (dr_.Tables[0].Rows.Count == 0)
            if (!(CCN.Contains(dt.Rows[j]["ConNo"].ToString())))
            {
                COUNT_ = 1;

                query += " SELECT '" + dt.Rows[j]["ConNo"].ToString() + "', '" + Header.BranchCode + "', '" + dt.Rows[j]["Destination"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "', '" + clvar.Weight + "', '" + clvar.pieces + "' , NewID(),GETDATE(), GETDATE(),'" + Header.ZoneCode + "',\n" +
                         "   '" + Header.BranchCode + "', 'overnight', '12'\n";

                trackQuery += " SELECT   '" + dt.Rows[j]["CONNO"].ToString() + "',\n" +
                                       "   '1',\n" +
                                       "   '" + HttpContext.Current.Session["LocationName"].ToString() + "',\n" +
                                       "   GETDATE(), '-'\n";
            }

            if (COUNT_ == 1)
            {
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
                    sqlcmd.CommandText = query;
                    count = sqlcmd.ExecuteNonQuery();
                    if (count == 0)
                    {
                        trans.Rollback();
                        return "NOT OK";
                    }
                    sqlcmd.CommandText = trackQuery;
                    count = sqlcmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "NOT OK" + ex.Message;
                }
                sqlcon.Close();
            }
            return "OK";

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

                //string Condition = "";
                //if (Header.RunsheetNumber.Trim() != "")
                //    Condition = " AND rc.RunsheetNumber != '" + RunsheetNumber + "'\n";
                //string sqlString = "SELECT rc.runsheetNumber, rc.branchcode, rc.consignmentNumber, DATEDIFF(Minute, rc.createdOn, GETDATE()) FROM RunsheetConsignment rc\n" +
                //" WHERE rc.consignmentNumber IN (" + cnNumbers + ") AND rc.branchcode != '" + clvar.Branch + "' AND DATEDIFF(SECOND, rc.createdOn, GETDATE()) <= 6000 " +
                // Condition;


                //DataTable prevRunsheets = new DataTable();
                //try
                //{
                //    con.Open();
                //    SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                //    sda.Fill(prevRunsheets);

                //}
                //catch (Exception ex)
                //{ }
                //finally { con.Close(); }
                //if (prevRunsheets != null)
                //{
                //    if (prevRunsheets.Rows.Count > 0)
                //    {
                //        resp.Add("2");
                //        string errorNumbers = "";
                //        DataTable distinctCNS = prevRunsheets.DefaultView.ToTable(true, "ConsignmentNumber");

                //        foreach (DataRow dr in distinctCNS.Rows)
                //        {
                //            errorNumbers += dr["ConsignmentNumber"].ToString() + ",";
                //        }
                //        resp.Add("Runsheets for the following Consignment Number(s) " + errorNumbers + " have been made within last 5 minutes");
                //        trans.Rollback();
                //        sqlcon.Close();
                //        return resp;
                //    }
                //}
                //else
                //{
                //    resp.Add("2");
                //    resp.Add("Error generating Runsheet. Please try Again.");
                //    trans.Rollback();
                //    sqlcon.Close();
                //    return resp;
                //}

                string GivenTo = "UPDATE RUNSHEETCONSIGNMENT SET REASON = '204', STATUS = '56', GivenToRider = '" + Header.RiderCode + "' " +
                    " WHERE consignmentNumber IN (" + cnNumbers + ") AND branchcode = '" + Header.BranchCode + "' AND (Reason = '' OR Reason IS NULL)";


                string insertTracking_GivenTo = "INSERT INTO ConsignmentsTrackingHistory\n" +
                                    "  (ConsignmentNumber,\n" +
                                    "   StateId,\n" +
                                    "   CurrentLocation,\n" +
                                    "   reason,\n" +
                                    "   runsheetNumber,\n" +
                                    "   TransactionTime,statusTime, internationalRemarks)\n";

                string sql = "SELECT * FROM   RunsheetConsignment rc WHERE  rc.consignmentNumber IN  (" + cnNumbers + ")  AND (rc.Reason = '' OR rc.Reason IS NULL) \n"
                 + "       AND rc.branchcode = '" + Header.BranchCode + "' order by createdon desc";

                DataTable dt_GivenTo_trackiong = new DataTable();
                try
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                    sda.Fill(dt_GivenTo_trackiong);

                }
                catch (Exception ex)
                { }
                finally { con.Close(); }



                for (int i = 0; i < dt_GivenTo_trackiong.Rows.Count - 1; i++)
                {
                    insertTracking_GivenTo += "  SELECT '" + dt_GivenTo_trackiong.Rows[i]["ConsignmentNumber"].ToString() + "',\n" +
                                      "             '10',\n" +
                                      "             '" + currentLocation + "',\n" +
                                      "             'UNDELIVERED',\n" +
                                      "              '" + dt_GivenTo_trackiong.Rows[i]["runsheetNumber"].ToString() + "',\n" +
                                      "              GETDATE(),GETDATE(), ' ' \n" +
                                      "      UNION ALL";
                }
                if (dt_GivenTo_trackiong.Rows.Count > 0)
                {
                    insertTracking_GivenTo += "  SELECT '" + Rdetails.Rows[dt_GivenTo_trackiong.Rows.Count - 1]["ConsignmentNumber"].ToString() + "',\n" +
                                          "             '10',\n" +
                                          "             '" + currentLocation + "',\n" +
                                          "             'UNDELIVERED',\n" +
                                          "              '" + dt_GivenTo_trackiong.Rows[dt_GivenTo_trackiong.Rows.Count - 1]["runsheetNumber"].ToString() + "',\n" +
                                          "              GETDATE(),GETDATE(), ' ' \n";
                }
                string insertDetails = "INSERT INTO RUNSHEETCONSIGNMENT (RUNSHEETNUMBER, CONSIGNMENTNUMBER, CREATEDBY, CREATEDON, STATUS, SORTORDER, BRANCHCODE,ROUTECODE,COD)\n";

                string insertTracking = "INSERT INTO ConsignmentsTrackingHistory\n" +
                                 "  (ConsignmentNumber,\n" +
                                 "   StateId,\n" +
                                 "   CurrentLocation,\n" +
                                 "   RiderName,\n" +
                                 "   runsheetNumber,\n" +
                                 "   TransactionTime, internationalRemarks)\n";
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

                    insertTracking += "  SELECT '" + Rdetails.Rows[i]["ConsignmentNumber"].ToString() + "',\n" +
                                  "             '8',\n" +
                                  "             '" + currentLocation + "',\n" +
                                  "             '" + Header.RiderName + "',\n" +
                                  "             '" + RunsheetNumber.ToString() + "',\n" +
                                  "             GETDATE(), '' \n" +
                                  "      UNION ALL";
                }
                insertDetails += "  SELECT '" + RunsheetNumber.ToString() + "',\n" +
                                "          '" + Rdetails.Rows[Rdetails.Rows.Count - 1]["ConsignmentNumber"].ToString().Trim() + "',\n" +
                                "          '" + Header.CreatedBy + "',\n" +
                                "          GETDATE(),\n" +
                                "          '56',\n" +
                                "          '" + Rdetails.Rows[Rdetails.Rows.Count - 1]["SortOrder"].ToString() + "',\n" +
                                "          '" + Header.BranchCode + "',\n" +
                                "          '" + Header.RouteCode.ToString().ToUpper() + "','" + Rdetails.Rows[Rdetails.Rows.Count - 1]["Cod"].ToString() + "'\n";

                insertTracking += "  SELECT '" + Rdetails.Rows[Rdetails.Rows.Count - 1]["ConsignmentNumber"].ToString() + "',\n" +
                                  "             '8',\n" +
                                  "             '" + currentLocation + "',\n" +
                                  "             '" + Header.RiderName + "',\n" +
                                  "             '" + RunsheetNumber.ToString() + "',\n" +
                                  "             GETDATE(), '' \n";

                //sqlcmd = new SqlCommand();
                //sqlcmd.Connection = sqlcon;


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

                string insertRiderRunsheet_backup = "INSERT INTO RiderRunsheet_backup\n" +
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

                //sqlcmd = new SqlCommand();
                //sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = insertTracking;
                insertCount = 0;
                insertCount = sqlcmd.ExecuteNonQuery();
                if (insertCount <= 0)
                {
                    resp.Add("2");
                    resp.Add("Could not Save Runsheet Tracking.");
                    trans.Rollback();
                    sqlcon.Close();
                    return resp;
                }
                // Given to tacking POD ENTRY
                if (dt_GivenTo_trackiong.Rows.Count > 0)
                {
                    sqlcmd.CommandText = insertTracking_GivenTo;
                    insertCount = 0;
                    insertCount = sqlcmd.ExecuteNonQuery();
                    if (insertCount <= 0)
                    {
                        resp.Add("2");
                        resp.Add("Could not Save Runsheet Tracking (POD).");
                        trans.Rollback();
                        sqlcon.Close();
                        return resp;
                    }

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


                sqlcmd.CommandText = insertRiderRunsheet_backup;
                insertCount = 0;
                insertCount = sqlcmd.ExecuteNonQuery();
                if (insertCount <= 0)
                {
                    resp.Add("2");
                    resp.Add("Could not Save Runsheet Rider Backup.");
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
        private static int insertRunsheetbackup(RunsheetModel Header)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            int insertCount = 0;

            string insertHeader_Master = "INSERT INTO Runsheet\n" +
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
          "   VEHICLETYPE,ridercode,expressCenterCode,zoneCode)\n" +
          "VALUES\n" +
          "  ('" + Header.RunsheetNumber.ToString() + "',\n" +
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
           "  ('" + Header.RunsheetNumber.ToString() + "',\n" +
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
           "   '" + Header.VehicleType + "','" + Header.RiderCode + "','" + Header.ExpressCenterCode + "','" + Header.ZoneCode + "','0')";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = insertHeader_Master;
                insertCount = cmd.ExecuteNonQuery();
                if (insertCount == 1)
                {
                    cmd.CommandText = insertHeader_backup;
                    insertCount = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return insertCount;
        }
        public static void route_SMS(string runsheetnumber, string createdBy, string branchCode)
        {

            string sql = "";
            if (branchCode == "4")
            {
                sql = "/************************************************************ \n"
                  + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
                  + " * Time: 11/27/2018 10:42:01 AM \n"
                  + " ************************************************************/ \n"
                  + " \n"
                  + "SELECT temp.consignmentNumber, \n"
                  + "       temp.pieces, \n"
                  + "       temp.weight, \n"
                  + "       temp.orign, \n"
                  + "       temp.consignee, \n"
                  + "       temp.cod, \n"
                  + "       temp.SortOrder, \n"
                  + "       SUM(temp.codAmount)     CODAMOUNT, \n"
                  + "       temp.Receiver_CNIC, \n"
                  + "       temp.RiderName, \n"
                  + "       consigneePhoneNo \n"
                  + "FROM   ( \n"
                  + "           SELECT c.consignmentNumber, \n"
                  + "                  c.pieces, \n"
                  + "                  c.weight, \n"
                  + "                  b.sname orign, \n"
                  + "                  c.consignee, \n"
                  + "                  c.cod, \n"
                  + "                  rc.SortOrder, \n"
                  + "                  cd.codAmount, \n"
                  + "                  rc.Receiver_CNIC, \n"
                  + "                  ri.riderCode + ' ' + ri.firstName + ' ' + ri.lastName  \n"
                  + "                  RiderName, \n"
                  + "                  c.consigneePhoneNo \n"
                  + "           FROM   Runsheet R \n"
                  + "                  INNER JOIN RunsheetConsignment rc \n"
                  + "                       ON  R.runsheetNumber = rc.runsheetNumber \n"
                  + "                       AND R.branchCode = rc.branchcode \n"
                  + "                           --AND R.createdBy = rc.createdBy \n"
                  + "                       AND R.routeCode = rc.RouteCode \n"
                  + "                  INNER JOIN Consignment c \n"
                  + "                       ON  C.consignmentNumber = rc.consignmentNumber \n"
                  + "                  INNER JOIN Branches B \n"
                  + "                       ON  B.branchCode = C.orgin \n"
                  + "                  LEFT OUTER JOIN CODConsignmentDetail cd \n"
                  + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
                  + "                  LEFT OUTER JOIN Riders ri \n"
                  + "                       ON  rc.branchcode = ri.branchId \n"
                  + "                       AND rc.GivenToRider = ri.riderCode \n"
                  + "           WHERE  rc.runsheetNumber = '" + runsheetnumber + "' \n"
                  + "                  AND rc.cod = '1' \n"
                  + "                  AND rc.branchCode = '4' \n"
                  + "                  AND c.destination = '4' \n"
                  + "            \n"
                  + "           UNION \n"
                  + "           SELECT c.consignmentNumber, \n"
                  + "                  c.pieces, \n"
                  + "                  c.weight, \n"
                  + "                  b.sname orign, \n"
                  + "                  c.consignee, \n"
                  + "                  c.cod, \n"
                  + "                  rc.SortOrder, \n"
                  + "                  cd.codAmount, \n"
                  + "                  rc.Receiver_CNIC, \n"
                  + "                  ri.riderCode + ' ' + ri.firstName + ' ' + ri.lastName  \n"
                  + "                  RiderName, \n"
                  + "                  c.consigneePhoneNo \n"
                  + "           FROM   Runsheet R \n"
                  + "                  INNER JOIN RunsheetConsignment rc \n"
                  + "                       ON  R.runsheetNumber = rc.runsheetNumber \n"
                  + "                       AND R.branchCode = rc.branchcode \n"
                  + "                           --AND R.createdBy = rc.createdBy \n"
                  + "                       AND R.routeCode = rc.RouteCode \n"
                  + "                  INNER JOIN Consignment c \n"
                  + "                       ON  C.consignmentNumber = rc.consignmentNumber \n"
                  + "                  INNER JOIN Branches B \n"
                  + "                       ON  B.branchCode = C.orgin \n"
                  + "                  LEFT OUTER JOIN CODConsignmentDetail_New cd \n"
                  + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
                  + "                  LEFT OUTER JOIN Riders ri \n"
                  + "                       ON  rc.branchcode = ri.branchId \n"
                  + "                       AND rc.GivenToRider = ri.riderCode \n"
                  + "           WHERE  rc.runsheetNumber = '" + runsheetnumber + "' \n"
                  + "                  AND rc.cod = '1' \n"
                  + "                  AND rc.branchCode = '4' \n"
                  + "                  AND c.destination = '4' \n"
                  + "       )                       temp \n"
                  + "GROUP BY \n"
                  + "       temp.consignmentNumber, \n"
                  + "       temp.pieces, \n"
                  + "       temp.weight, \n"
                  + "       temp.orign, \n"
                  + "       temp.consignee, \n"
                  + "       temp.cod, \n"
                  + "       temp.SortOrder, \n"
                  + "       temp.Receiver_CNIC, \n"
                  + "       temp.RiderName, \n"
                  + "       temp.consigneePhoneNo \n"
                  + "ORDER BY \n"
                  + "       temp.SortOrder DESC";


            }
            else
            {
                sql = "/************************************************************ \n"
                + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
                + " * Time: 11/27/2018 10:42:01 AM \n"
                + " ************************************************************/ \n"
                + " \n"
                + "SELECT temp.consignmentNumber, \n"
                + "       temp.pieces, \n"
                + "       temp.weight, \n"
                + "       temp.orign, \n"
                + "       temp.consignee, \n"
                + "       temp.cod, \n"
                + "       temp.SortOrder, \n"
                + "       SUM(temp.codAmount)     CODAMOUNT, \n"
                + "       temp.Receiver_CNIC, \n"
                + "       temp.RiderName, \n"
                + "       consigneePhoneNo,temp.SMS_Content \n"
                + "FROM   ( \n"
                + "           SELECT c.consignmentNumber, \n"
                + "                  c.pieces, \n"
                + "                  c.weight, \n"
                + "                  b.sname orign, \n"
                + "                  c.consignee, \n"
                + "                  c.cod, \n"
                + "                  rc.SortOrder, \n"
                + "                  cd.codAmount, \n"
                + "                  rc.Receiver_CNIC, \n"
                + "                  ri.riderCode + ' ' + ri.firstName + ' ' + ri.lastName  \n"
                + "                  RiderName, \n"
                + "                  c.consigneePhoneNo,rsa.SMS_Content \n"
                + "           FROM   Runsheet R \n"
                + "                  INNER JOIN RunsheetConsignment rc \n"
                + "                       ON  R.runsheetNumber = rc.runsheetNumber \n"
                + "                       AND R.branchCode = rc.branchcode \n"
                + "                           --AND R.createdBy = rc.createdBy \n"
                + "                       AND R.routeCode = rc.RouteCode \n"
                + "                  INNER JOIN Consignment c \n"
                + "                       ON  C.consignmentNumber = rc.consignmentNumber \n"
                + "                  INNER JOIN Branches B \n"
                + "                       ON  B.branchCode = C.orgin \n"
                + "                  INNER JOIN SMS_Allowed rsa  \n"
                + "                        ON  rsa.AccountNumber = c.consignerAccountNo \n"
                + "                  LEFT OUTER JOIN CODConsignmentDetail cd \n"
                + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
                + "                  LEFT OUTER JOIN Riders ri \n"
                + "                       ON  rc.branchcode = ri.branchId \n"
                + "                       AND rc.GivenToRider = ri.riderCode \n"
                + "           WHERE  rc.runsheetNumber = '" + runsheetnumber + "' \n"
                + "                  AND rc.cod = '1' \n"
                + "                  AND rc.branchCode <> '4' \n"
                + "                  AND c.destination <> '4' \n"
                + "            \n"
                + "           UNION \n"
                + "           SELECT c.consignmentNumber, \n"
                + "                  c.pieces, \n"
                + "                  c.weight, \n"
                + "                  b.sname orign, \n"
                + "                  c.consignee, \n"
                + "                  c.cod, \n"
                + "                  rc.SortOrder, \n"
                + "                  cd.codAmount, \n"
                + "                  rc.Receiver_CNIC, \n"
                + "                  ri.riderCode + ' ' + ri.firstName + ' ' + ri.lastName  \n"
                + "                  RiderName, \n"
                + "                  c.consigneePhoneNo,rsa.SMS_Content \n"
                + "           FROM   Runsheet R \n"
                + "                  INNER JOIN RunsheetConsignment rc \n"
                + "                       ON  R.runsheetNumber = rc.runsheetNumber \n"
                + "                       AND R.branchCode = rc.branchcode \n"
                + "                           --AND R.createdBy = rc.createdBy \n"
                + "                       AND R.routeCode = rc.RouteCode \n"
                + "                  INNER JOIN Consignment c \n"
                + "                       ON  C.consignmentNumber = rc.consignmentNumber \n"
                + "                  INNER JOIN Branches B \n"
                + "                       ON  B.branchCode = C.orgin \n"
                + "                  INNER JOIN SMS_Allowed rsa  \n"
                + "                        ON  rsa.AccountNumber = c.consignerAccountNo \n"
                + "                  LEFT OUTER JOIN CODConsignmentDetail_New cd \n"
                + "                       ON  cd.consignmentNumber = c.consignmentNumber \n"
                + "                  LEFT OUTER JOIN Riders ri \n"
                + "                       ON  rc.branchcode = ri.branchId \n"
                + "                       AND rc.GivenToRider = ri.riderCode \n"
                + "           WHERE  rc.runsheetNumber = '" + runsheetnumber + "' \n"
                + "                  AND rc.cod = '1' \n"
                + "                  AND rc.branchCode <> '4' \n"
                + "                  AND c.destination <> '4' \n"
                + "       )                       temp \n"
                + "GROUP BY \n"
                + "       temp.consignmentNumber, \n"
                + "       temp.pieces, \n"
                + "       temp.weight, \n"
                + "       temp.orign, \n"
                + "       temp.consignee, \n"
                + "       temp.cod, \n"
                + "       temp.SortOrder, \n"
                + "       temp.Receiver_CNIC, \n"
                + "       temp.RiderName, \n"
                + "       temp.consigneePhoneNo, \n"
                + "       temp.SMS_Content \n"
                + "ORDER BY \n"
                + "       temp.SortOrder DESC";
            }
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


            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string consignmentNo = "";
                    string RIDER = "";
                    //string smsContent = "Your COD Shipment " + dt.Rows[i]["ConsignmentNumber"].ToString() + " is on route today.You can also pay RS:" + dt.Rows[i]["CODAMOUNT"].ToString() + " via Easypaisa. Android: https://s9ub3.app.goo.gl/UeW3,iOS: https://goo.gl/aN6iWC";
                    //string smsContent = "Your COD " + dt.Rows[i]["ConsignmentNumber"].ToString() + " is on route. Pay Rs:" + dt.Rows[i]["CODAMOUNT"].ToString() + " via Easypaisa & get 10% cashback (*max Rs.300). Android: https://bit.ly/2Ub7HEK, iOS: https://bit.ly/2KRsAk5";
                    string smsContent = "";
                    if (branchCode == "4")
                    {
                        smsContent = "Your COD shpt." + dt.Rows[i]["ConsignmentNumber"].ToString() + " is on route today.You can also pay Rs:" + dt.Rows[i]["CODAMOUNT"].ToString() + " via Easypaisa. Android: https://s9ub3.app.goo.gl/UeW3, iOS: https://goo.gl/aN6iWC";
                    }
                    else
                    {
                        consignmentNo = dt.Rows[i]["ConsignmentNumber"].ToString();
                        RIDER = dt.Rows[i]["RiderName"].ToString();
                        smsContent = dt.Rows[i]["SMS_Content"].ToString();
                        smsContent = smsContent.Replace("+CN+", consignmentNo);
                        smsContent = smsContent.Replace("+RIDER+", RIDER);
                    }
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
                          "   Error,smsformtype)\n";
                    smsCommand += "\n" +
                           "           SELECT '" + dt.Rows[i]["ConsignmentNumber"].ToString() + "' CN,\n" +
                           "                  '" + dt.Rows[i]["consigneePhoneNo"].ToString() + "' CONSIGNEEPHONE,\n" +
                           "                  '" + smsContent + "'     SMSCONTENT,\n" +
                           "                  '0' STATUS,\n" +
                           "                  GETDATE()       CREATEDON,\n" +
                           "                  '" + createdBy + "' CREATEDBY,\n" +
                           "                  NULL            MODIFIEDON,\n" +
                           "                  NULL            MODIFIEDBY,\n" +
                           "                  '" + runsheetnumber + "' RUNSHEETNUMBER,\n" +
                           "                  NULL            ERRORCODE,\n" +
                           "                  NULL            ERROR,'6'\n" +
                           "      ";

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
                        sqlcmd.CommandText = smsCommand;
                        sqlcmd.ExecuteNonQuery();
                        trans.Commit();

                    }
                    catch (Exception ER)
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        sqlcon.Close();
                    }

                }
            }
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