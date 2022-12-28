using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for bayer_Function
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class LoadingPrintReport
    {
        CommonFunction ComFunction = new CommonFunction();
        public LoadingPrintReport()
        {

        }

        public DataTable GetLoadingHeader(Cl_Variables clvar)
        {
            string sqlString = "select lu.AttributeDesc TransportType,\n" +
            "       l.date,\n" +
            "       v.MakeModel + ' (' + v.Description + ')' VehicleName,\n" +
            "       l.courierName,\n" +
            "       b1.name OrgName,\n" +
            "       b2.name DestName,\n" +
            "       l.description,\n" +
            "       l.flightNo,\n" +
            "       l.sealno,\n" +
            "       l.departureflightdate \n" +
            "  from mnp_Loading l\n" +
            " left outer join rvdbo.Lookup lu\n" +
            "    on lu.id = l.transportationType\n" +
            "   and lu.AttributeGroup = 'TRANSPORT_TYPE'\n" +
            " inner join rvdbo.Vehicle v\n" +
            "    on v.VehicleCode = l.vehicleId\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = l.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = l.destination\n" +
            " where l.id = '" + clvar.LoadingID + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        public DataTable GetLoadingDetail(Cl_Variables clvar)
        {
            string sqlString = "select lb.loadingId,\n" +
            "\t   b.bagNumber, b.totalWeight,\n" +
            "\t   b1.name OrgName,\n" +
            "\t   b2.name DestName,\n" +
            "\t   b.sealNo\n" +
            "  from mnp_LoadingBag lb\n" +
            " inner join Bag b\n" +
            "\ton b.bagNumber = lb.bagNumber\n" +
            " inner join Branches b1\n" +
            "\ton b1.branchCode = b.origin\n" +
            " inner join Branches b2\n" +
            "\ton b2.branchCode = b.destination\n" +
            " where lb.loadingId = '" + clvar.LoadingID + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        public DataTable GetLoadingConsignments(Cl_Variables clvar)
        {
            string sqlString = "select l.consignmentNumber,\n" +
            "       c.pieces,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       b.name Destination,\n" +
            "       c.weight\n" +
            "  from mnp_LoadingConsignment l\n" +
            " inner join Consignment c\n" +
            "    on c.consignmentNumber = l.consignmentNumber\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = c.destination\n" +
            " where l.loadingId = '" + clvar.LoadingID + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Insert Loading Record into Loading Table
        //public void Insert_Loading(Variable clvar)
        //{
        //    try
        //    {
        //        string query = "insert into rvdbo.Loading \n" +
        //                        "  (LoadingDate, Description, VehicleId, CourierName, MovementRouteId, TransportTypeId, OriginBranchId, DestBranchId, ZoneId, IsUnLoaded, CreatedOn, ismaster, VehicleRegNo)\n" +
        //                        "values\n" +
        //                        "  ( \n" +
        //                        "   '" + clvar._StartDate + "',\n" +
        //                        "   '" + clvar._Description + "',\n" +
        //                        "   '" + clvar._VehicleId + "',\n" +
        //                        "   '" + clvar._CourierName + "',\n" +
        //                        "   '" + clvar._Route + "',\n" +
        //                        "   '" + clvar._TransportType + "',\n" +
        //                        "   '" + clvar._Orign + "',\n" +
        //                        "   '" + clvar._Destination + "',\n" +
        //                        "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
        //                        "   '1', \n" +
        //                        "   GETDATE(),\n" +
        //                        "   '1', \n" +
        //                        "   '" + clvar.VehicleNo + "'\n" +
        //                        " ) ";

        //        SqlConnection orcl = new SqlConnection(clvar.Strcon2());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(query, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        orcd.ExecuteNonQuery();
        //        orcl.Close();
        //    }
        //    catch (Exception Err)
        //    {

        //    }
        //    finally
        //    { }
        //}
        public string Insert_Loading(Variable clvar)
        {


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            Int64 LoadingID = 0;
            try
            {
                #region RVDBO INSERT QUERY
                string query = "insert into rvdbo.Loading \n" +
                               "  (LoadingDate, Description, VehicleId, CourierName, MovementRouteId, TransportTypeId, OriginBranchId, DestBranchId, ZoneId, IsUnLoaded, CreatedOn, ismaster, VehicleRegNo)\n" +
                               " OUTPUT INSERTED.LOADINGID\n" +
                               "values\n" +
                               "  ( \n" +
                               "   '" + clvar._StartDate + "',\n" +
                               "   '" + clvar._Description + "',\n" +
                               "   '" + clvar._VehicleId + "',\n" +
                               "   '" + clvar._CourierName + "',\n" +
                               "   '" + clvar._Route + "',\n" +
                               "   '" + clvar._TransportType + "',\n" +
                               "   '" + clvar._Orign + "',\n" +
                               "   '" + clvar._Destination + "',\n" +
                               "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
                               "   '1', \n" +
                               "   GETDATE(),\n" +
                               "   '1', \n" +
                               "   '" + clvar.VehicleNo + "'\n" +
                               " ) ";
                #endregion

                //sqlcmd.CommandText = query;
                //LoadingID = (Int64)sqlcmd.ExecuteScalar();

                //clvar.LoadingId = LoadingID.ToString();
                //if (LoadingID == 0)
                //{
                //    trans.Rollback();
                //    return "0";

                //}
                #region DBO INSERT QUERY
                string query2 = "insert into MNP_Loading \n" +
                                        "  (l.date, description, transportationType, vehicleId, courierName, origin, destination, branchCode, zoneCode, createdBy, createdOn, routeId, sealNo, FlightNo,\n";
                if (clvar.FlightDepartureDate.ToString() != "NULL")
                {
                    query2 += " DepartureFlightDate, ";
                }

                query2 += "IsMaster, VehicleRegNo)  OUTPUT INSERTED.id \n" +
                                 "values\n" +
                                 "  ( \n" +
                                 //"   '" + clvar.LoadingId + "',\n" +
                                 "   '" + clvar._StartDate + "',\n" +
                                 "   '" + clvar._Description + "',\n" +
                                 "   '" + clvar._TransportType + "',\n" +
                                 "   '" + clvar._VehicleId + "',\n" +
                                 "   '" + clvar._CourierName + "',\n" +
                                 "   '" + clvar._Orign + "',\n" +
                                 "   '" + clvar._Destination + "',\n" +
                                 "   '" + HttpContext.Current.Session["branchcode"].ToString() + "', \n" +
                                 "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
                                 "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                 "   GETDATE(),\n" +
                                 "   '" + clvar._Route + "', \n" +
                                 "   '" + clvar.Seal + "',\n";
                if (clvar.FlightDepartureDate.ToString() != "NULL")
                {
                    query2 += "   '" + clvar.FlightDepartureDate + "',\n";
                }
                query2 += "   '" + clvar.FlightNo + "',\n" +

                "   '1', \n" +
                "   '" + clvar.VehicleNo + "'\n" +
                " ) ";
                #endregion

                sqlcmd.CommandText = query2;
                LoadingID = (Int64)sqlcmd.ExecuteScalar();
                if (LoadingID == 0)
                {
                    trans.Rollback();
                    return "0";
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "Error:" + ex.Message;
            }

            sqlcon.Close();
            return LoadingID.ToString();
        }

        // Get Last Loading Id
        public DataSet Get_LastLoadingId(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select max(l.Loadingid) LoadingId from rvdbo.Loading l";

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

        // Insert Loading Record into Loading Table
        public void Insert_Loading1(Variable clvar)
        {
            try
            {
                string query = "insert into Loading \n" +
                                "  (id, l.date, description, transportationType, vehicleId, courierName, origin, destination, branchCode, zoneCode, createdBy, createdOn, routeId, sealNo, FlightNo,\n";
                if (clvar.FlightDepartureDate.ToString() != "NULL")
                {
                    query += " DepartureFlightDate, ";
                }

                query += "IsMaster, VehicleRegNo)\n" +
                                 "values\n" +
                                 "  ( \n" +
                                 "   '" + clvar._LoadingId + "',\n" +
                                 "   '" + clvar._StartDate + "',\n" +
                                 "   '" + clvar._Description + "',\n" +
                                 "   '" + clvar._TransportType + "',\n" +
                                 "   '" + clvar._VehicleId + "',\n" +
                                 "   '" + clvar._CourierName + "',\n" +
                                 "   '" + clvar._Orign + "',\n" +
                                 "   '" + clvar._Destination + "',\n" +
                                 "   '" + HttpContext.Current.Session["branchcode"].ToString() + "', \n" +
                                 "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
                                 "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                 "   GETDATE(),\n" +
                                 "   '" + clvar._Route + "', \n" +
                                 "   '" + clvar.Seal + "',\n";
                if (clvar.FlightDepartureDate.ToString() != "NULL")
                {
                    query += "   '" + clvar.FlightDepartureDate + "',\n";
                }
                query += "   '" + clvar.FlightNo + "',\n" +

                "   '1', \n" +
                "   '" + clvar.VehicleNo + "'\n" +
                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
        }

        // Insert LoadingBag Record into Loading Table

        //public void Insert_LoadingBag(Variable clvar)
        //{
        //    try
        //    {
        //        string query = "insert into rvdbo.LoadingBag \n" +
        //                        "  (LoadingId, BagId)\n" +
        //                        "values\n" +
        //                        "  ( \n" +
        //                        "   '" + clvar._LoadingId + "',\n" +
        //                        "   '" + clvar.BagNumber + "'\n" +
        //                        " ) ";

        //        SqlConnection orcl = new SqlConnection(clvar.Strcon2());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(query, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        orcd.ExecuteNonQuery();
        //        orcl.Close();
        //    }
        //    catch (Exception Err)
        //    {

        //    }
        //    finally
        //    { }
        //}
        public void Insert_LoadingBag(Variable clvar, DataTable dt)
        {
            string query = "";
            SqlConnection orcl = new SqlConnection(clvar.Strcon2());
            orcl.Open();
            foreach (DataRow dr in dt.Rows)
            {
                try
                {

                    query = "insert into MNP_LoadingBag (loadingId, bagNumber, BagDestination, createdBy, createdOn, BagWeight) \n" +
                    "Values ('" + dr["LoadingID"].ToString() + "', '" + dr["BagNumber"].ToString() + "', '" + dr["BagDestination"].ToString() + "', \n" +
                             "'" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), '" + dr["BagWeight"].ToString() + "')";



                    SqlCommand orcd = new SqlCommand(query, orcl);
                    orcd.CommandType = CommandType.Text;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    orcd.ExecuteNonQuery();


                }
                catch (Exception Err)
                {
                    ComFunction.InsertErrorLog("", "", "", dr["BagNumber"].ToString(), dr["LoadingID"].ToString(), "", "LOADING", Err.Message);
                }
                finally
                { }
            }
            orcl.Close();

        }

        public DataSet Get_Branches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    query = "select NAME, branchCode from Branches where status = '1' ORDER BY NAME";
                }
                else
                {
                    //  string query = "select NAME, branchCode from Branches where status = '1' ORDER BY NAME";
                    query = "select NAME, branchCode from Branches where status = '1' \n" +
                                    " AND branchCode IN (" + HttpContext.Current.Session["BRANCHCODE"].ToString() + ")  ORDER BY NAME";
                }


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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

        public void Insert_ConsignmentTrackingHistory(Variable clvar)
        {
            try
            {
                string query = "insert into ConsignmentsTrackingHistory \n" +
                                "  (consignmentNumber, stateID, currentLocation, manifestNumber, bagNumber, loadingNumber, mawbNumber, runsheetNumber, riderName, transactionTime)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ConsignmentNo + "',\n" +
                                "   '" + clvar._StateId + "',\n" +
                                "   '" + HttpContext.Current.Session["LocationName"].ToString() + "',\n" +
                                "   '" + clvar._Manifest + "',\n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + clvar._LoadingId + "',\n" +
                                "   '" + clvar._MawbNumber + "',\n" +
                                "   '" + clvar._RunsheetNumber + "',\n" +
                                "   '" + clvar.RiderCode + "',\n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
        }

        public DataSet Get_BagConsignment(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                //  string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' AND c.destination = '" + clvar._Designation + "' ";
                string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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

        public void Insert_ConsignmentFromLoading(Variable clvar)
        {
            try
            {
                string query = "insert into Consignment \n" +
                                "  (consignmentNumber, serviceTypeName, riderCode, consignmentTypeId, weight, orgin, destination, createdon, createdby, customerType, pieces, \n" +
                                "   creditClientId, weightUnit, discount, cod, totalAmount, zoneCode, branchCode, gst, consignerAccountNo, bookingDate, isApproved, \n" +
                                "   deliveryStatus, dayType, isPriceComputed, isNormalTariffApplied, receivedFromRider, originExpressCenter, syncId)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ConsignmentNo + "',\n" +
                                "   '" + clvar._Services + "',\n" +
                                "   '" + clvar._RiderCode + "',\n" +
                                "   '" + clvar._ConsignmentType + "',\n" +
                                "   '" + clvar._Weight + "',\n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                                "   '" + clvar._TownCode + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                "   '1',\n" +
                                "   '1',\n" +
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
                                "   '1', '" + clvar.Expresscentercode + "', \n" +
                                "  NewID() \n" +
                                " ) ";


                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
        }

        // Insert LoadingBag Record into Loading Table
        public void Insert_LoadingConsignment_NewByRabi(Variable clvar, DataTable dt)
        {
            SqlConnection orcl = new SqlConnection(clvar.Strcon2());
            orcl.Open();

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string query = "insert into MNP_LoadingConsignment \n" +
                                    "  (loadingId, consignmentNumber, CNDestination, createdBy, CNPieces, createdOn)\n" +
                                    "values\n" +
                                    "  ( \n" +
                                    "   '" + dr["LoadingID"].ToString() + "',\n" +
                                    "   '" + dr["CN"].ToString() + "',\n" +
                                    "   '" + dr["CNDestination"].ToString() + "',\n" +
                                    "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', '" + dr["pieces"].ToString() + "',\n" +
                                    "   GETDATE()\n" +
                                    " ) ";

                    SqlCommand orcd = new SqlCommand(query, orcl);
                    orcd.CommandType = CommandType.Text;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    orcd.ExecuteNonQuery();

                }

                catch (Exception Err)
                {
                    ComFunction.InsertErrorLog(dr["CN"].ToString(), "", "", "", dr["LoadingID"].ToString(), "", "LOADING", Err.Message);
                }
                finally
                { }
            }
            orcl.Close();

        }

        // Master Transport Type
        public DataSet Get_MasterTransportType(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select l.AttributeValue, l.id, l.AttributeDesc from rvdbo.Lookup l where l.AttributeGroup = 'TRANSPORT_TYPE' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_SearchLoading(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = " select l.id,lu.AttributeDesc TransportType, \n" +
                             " CONVERT(VARCHAR(10), l.date, 105) date,\n" +
                             " v.MakeModel + ' (' + v.Description + ')' VehicleName,\n" +
                             " l.courierName,\n" +
                             " b1.name OrgName,\n" +
                             " b2.name DestName,\n" +
                             " l.description, \n" +
                             " l.flightNo, \n" +
                             " l.sealno, \n" +
                             " l.departureflightdate  \n" +
                             " from mnp_Loading l \n" +
                             " left outer join rvdbo.Lookup lu \n" +
                             " on lu.id = l.transportationType \n" +
                             " and lu.AttributeGroup = 'TRANSPORT_TYPE' \n" +
                             " inner join rvdbo.Vehicle v \n" +
                             " on v.VehicleCode = l.vehicleId\n" +
                             " inner join Branches b1 \n" +
                             " on b1.branchCode = l.origin \n" +
                             " inner join Branches b2 \n" +
                             " \n" +
                             " on b2.branchCode = l.destination \n" +
                             " where l.id = '" + clvar.LoadingId + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public string Insert_ConsignmentTrackingHistoryFromLoading(Variable clvar)
        {


            string sqlString = "select '' LOADING, lc.consignmentNumber CN\n" +
            "  from MnP_LoadingConsignment lc\n" +
            " where lc.loadingId = '" + clvar._LoadingId + "'\n" +
            "union\n" +
            "select '' LOADING, ba.outpieceNumber CN\n" +
            "  from BagOutpieceAssociation ba\n" +
            " where ba.bagNumber in\n" +
            "       (select lb.bagNumber\n" +
            "          from MnP_LoadingBag lb\n" +
            "         where lb.loadingId = '" + clvar._LoadingId + "')\n" +
            "union\n" +
            "select '' LOADING, cm.consignmentNumber CN\n" +
            "  from Mnp_ConsignmentManifest cm\n" +
            " inner join BagManifest bm\n" +
            "    on bm.manifestNumber = cm.manifestNumber\n" +
            " where bm.bagNumber in\n" +
            "       (select lb.bagNumber\n" +
            "          from MnP_LoadingBag lb\n" +
            "         where lb.loadingId = '" + clvar._LoadingId + "')";

            sqlString = "insert into ConsignmentsTrackingHistory\n" +
           "  (loadingNumber, consignmentNumber, stateID)\n" +
           "  select '" + clvar._LoadingId + "' LOADING, lc.consignmentNumber CN, '4'\n" +
           "    from MnP_LoadingConsignment lc\n" +
           "   where lc.loadingId = '" + clvar._LoadingId + "'\n" +
           "  union\n" +
           "  select '" + clvar._LoadingId + "' LOADING, ba.outpieceNumber CN, '4'\n" +
           "    from BagOutpieceAssociation ba\n" +
           "   where ba.bagNumber in\n" +
           "         (select lb.bagNumber\n" +
           "            from MnP_LoadingBag lb\n" +
           "           where lb.loadingId = '" + clvar._LoadingId + "')\n" +
           "  union\n" +
           "  select '" + clvar._LoadingId + "' LOADING, cm.consignmentNumber CN, '4'\n" +
           "    from Mnp_ConsignmentManifest cm\n" +
           "   inner join BagManifest bm\n" +
           "      on bm.manifestNumber = cm.manifestNumber\n" +
           "   where bm.bagNumber in\n" +
           "         (select lb.bagNumber\n" +
           "            from MnP_LoadingBag lb\n" +
           "           where lb.loadingId = '" + clvar._LoadingId + "')";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlString, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { ComFunction.InsertErrorLog("", "", "", "", clvar._LoadingId.ToString(), "", "LOADING", ex.Message); }
            con.Close();

            return "";
        }

        // Print Receipt Report
        public DataSet Get_ReceiptReport(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT pv.*,  \n"
               + "       CASE   \n"
               + "            WHEN pv.IsByCreditClient = '1' THEN 'Credit'  \n"
               + "            ELSE 'Cash'  \n"
               + "       END                       PaymentMode,  \n"
               + "         \n"
               + "       pt.Name                   PaymentType,  \n"
               + "       (Amount - AmountUsed)     Balance,  \n"
               + "         \n"
               + "       (  \n"
               + "           SELECT TOP(1) r.firstName + ' ' + r.lastName  \n"
               + "           FROM   Riders r  \n"
               + "           WHERE  r.riderCode = pv.RiderCode  AND r.branchId = pv.BranchCode   \n"
               + "       )                         RiderName,  \n"
               + "       (  \n"
               + "           SELECT r.name  \n"
               + "           FROM   ExpressCenters r  \n"
               + "           WHERE  r.expressCenterCode = pv.ExpressCenterCode  \n"
               + "       )                         ExpressCenter,  \n"
               + "       (  \n"
               + "           SELECT r.accountNo  \n"
               + "           FROM   CreditClients r  \n"
               + "           WHERE   r.id = pv.CreditClientId  \n"
               + "       )                         accountNo,  \n"
               + "       (  \n"
               + "           SELECT r.name  \n"
               + "           FROM   CreditClients r  \n"
               + "           WHERE   r.id = pv.CreditClientId  \n"
               + "       )                         name,  \n"
               + "        (  \n"
               + "           SELECT r.name  \n"
               + "           FROM   PaymentSource r  \n"
               + "           WHERE  pv.PaymentSourceId = r.Id  \n"
               + "       )                         PaymentSource, \n"
               + "       b.name BranchName, b.sname sbr,  \n"
               + "       z.name ZoneName,pv.ExpressCenterCode,pv.RiderCode,pv.ChequeNo,pv.BankId    \n"
               + "FROM   PaymentVouchers           pv,  \n"
               + "         \n"
               + "       PaymentTypes              pt,  \n"
               + "        \n"
               + "       Branches b,  \n"
               + "       Zones z  \n"
               + "WHERE   \n"
               + "       pv.PaymentTypeId = pt.Id  \n"
               + "       AND b.branchCode = pv.BranchCode  \n"
               + "       AND b.zoneCode = pv.ZoneCode  \n"
               + "       AND z.zoneCode = pv.ZoneCode \n"
                + "       AND pv.Id = '" + clvar.VoucherNo + "'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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


    }

}