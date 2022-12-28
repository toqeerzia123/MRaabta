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
    public class Function
    {

        public Function()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        Cl_Variables clvar = new Cl_Variables();

        #region BTSCODE

        public DataSet Get_ConsignmentTracking_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {


                string sql_ = "SELECT mcts.TrackingStatus, \n"
               + "       b.currentLocation, \n"
               + "       b.Booked, \n"
               + "       b.consignmentNumber,b.transactionTime, \n"
               + "       b.Detail \n"
               + "FROM   ( \n"
               + "           SELECT --mcts.TrackingStatus, \n"
               + "                  cth.transactionTime, \n"
               + "                  -- mcts.StatusID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  StateID, \n"
               + "                  --CASE \n"
               + "                  --     WHEN StateID = '1' THEN 'New' \n"
               + "                  --     WHEN StateID = '2' THEN 'Manifested' \n"
               + "                  --     WHEN StateID = '3' THEN 'Bagged' \n"
               + "                  --     WHEN StateID = '4' THEN 'Loaded' \n"
               + "                  --     WHEN StateID = '5' THEN 'Unload' \n"
               + "                  --     WHEN StateID = '6' THEN 'Debag' \n"
               + "                  --     WHEN StateID = '7' THEN 'Demanifested' \n"
               + "                  --     WHEN StateID = '8' THEN 'Runsheet' \n"
               + "                  --     WHEN StateID = '10' THEN 'POD' \n"
               + "                  --     WHEN StateID = '18' THEN 'Arrival' \n"
               + "                  --     WHEN StateID = '20' THEN 'Airport Material Arrival' \n"
               + "                  --     ELSE '' \n"
               + "                  --END \n"
               + "                  '' Booked, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '1' THEN ISNULL( \n"
               + "                                ( \n"
               + "                                    SELECT +'Consignment No: ' + c.consignmentNumber  \n"
               + "                                           + \n"
               + "                                           ' was booked on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                           + \n"
               + "                                           ' by User :' + zu.Name + \n"
               + "                                           ' on Location :'  \n"
               + "                                           + ec.name \n"
               + "                                    FROM   Consignment c, \n"
               + "                                           ZNI_USER1 zu, \n"
               + "                                           Branches b, \n"
               + "                                           ExpressCenters ec \n"
               + "                                    WHERE  CONVERT(NVARCHAR, c.createdby) =  \n"
               + "                                           CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                           AND zu.branchcode = b.branchCode \n"
               + "                                           AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                           AND RTRIM(LTRIM(c.consignmentNumber)) =  \n"
               + "                                               RTRIM(LTRIM(cth.consignmentNumber)) \n"
               + "                                ), \n"
               + "                                'New' \n"
               + "                            ) \n"
               + "                       WHEN cth.StateID = '2' THEN ( \n"
               + "                                SELECT +'Manifest No :' + c.manifestNumber + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   mnp_Manifest c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber \n"
               + "                            ) \n"
               + "                       WHEN cth.StateID = '3' THEN ( \n"
               + "                                SELECT +'Bag No: ' + c.bagNumber + \n"
               + "                                       ' was Generated on :' + \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   Bag c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.bagNumber = cth.bagNumber \n"
               + "                            ) \n"
               + "                       WHEN cth.StateID = '4' THEN ( \n"
               + "                                SELECT +'Loading No :' + CONVERT(VARCHAR, c.id)  \n"
               + "                                       + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   MnP_Loading c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.loadingNumber) \n"
               + "                            ) \n"
               + "                       WHEN cth.StateID = '18' THEN ( \n"
               + "                                SELECT + 'Arrival No :' + CONVERT(NVARCHAR, c.Id)  \n"
               + "                                       + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   ArrivalScan c, \n"
               + "                                       ArrivalScan_Detail asd, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  c.Id = asd.ArrivalID \n"
               + "                                       AND CONVERT(NVARCHAR, c.createdBy) =  \n"
               + "                                           CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND asd.consignmentNumber = cth.consignmentNumber \n"
               + "                                       AND c.Id = cth.ArrivalID \n"
               + "                            ) \n"
               + "                       WHEN cth.StateID = '6' THEN ( \n"
               + "                                SELECT +'DeBagging  was Generated on :' + \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   Bag c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.bagNumber = cth.bagNumber \n"
               + "                            ) \n"
               + "                       WHEN cth.StateID = '7' THEN ( \n"
               + "                                SELECT +'DeManifest was Generated on :' + \n"
               + "                                       CONVERT(VARCHAR(11), c.DemanifestDate, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   mnp_Manifest c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.DemanifestBy) =  \n"
               + "                                       CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber \n"
               + "                            ) \n"
               + "                       WHEN cth.StateID = '8' THEN ( \n"
               + "                                SELECT +'Runsheet No :' + c.runsheetNumber + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name  \n"
               + "                                       + ' against Rider :' + c.routeCode + ' -'  \n"
               + "                                       + cth.riderName \n"
               + "                                FROM   Runsheet c, \n"
               + "                                       RunsheetConsignment rc, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.runsheetNumber = rc.runsheetNumber \n"
               + "                                       AND c.routeCode = rc.RouteCode \n"
               + "                                       AND c.branchCode = rc.branchcode \n"
               + "                                       AND c.runsheetNumber = cth.runsheetNumber \n"
               + "                                       AND cth.consignmentNumber = rc.consignmentNumber \n"
               + "                            ) \n"
               + "                       WHEN cth.stateID = '10' \n"
               + "           AND LEN(cth.riderName) <> 0 THEN ( \n"
               + "                   SELECT 'Consignment has been \"' + cth.reason  \n"
               + "                          --   \n"
               + "                          + ' '  \n"
               + "                          + '\" Received By \"' + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN rc.receivedBy IS NULL THEN 'Not Feeded' \n"
               + "                                   ELSE rc.receivedBy \n"
               + "                              END \n"
               + "                          ) + '\" Dated: ' + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN cth.stateID = '10' THEN LEFT(rc.deliveryDate, 10) \n"
               + "                              END \n"
               + "                          ) + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN rc.time IS NULL THEN '' \n"
               + "                                   ELSE RIGHT(rc.time, 8) \n"
               + "                              END \n"
               + "                          ) + (CASE WHEN rc.Reason IS NULL THEN '' ELSE rc.Reason END)  \n"
               + "                          + '\" ' \n"
               + "                   FROM   runsheetconsignment rc, \n"
               + "                          runsheet r1 \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.branchcode = rc.branchcode \n"
               + "                          AND r1.routecode = rc.routecode \n"
               + "                          AND r1.createdBy = rc.createdBy \n"
               + "               ) \n"
               + "               WHEN cth.stateID = '10' \n"
               + "           AND LEN(cth.riderName) = 0 \n"
               + "           AND cth.reason in  \n"
               + "                ('RETURNED', 'UNDELIVERED') THEN ( \n"
               + "                   SELECT +' Consignment is ' + cth.reason + \n"
               + "                          ' .For RunsheetNumber :' + cth.runsheetNumber + \n"
               + "                          ' due to Following Reason :' + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN ISNULL(rc.Reason, '0') = '0' THEN '' \n"
               + "                                   ELSE ( \n"
               + "                                            SELECT v.AttributeValue \n"
               + "                                            FROM   rvdbo.Lookup v \n"
               + "                                            WHERE  v.Id = rc.Reason \n"
               + "                                        ) \n"
               + "                              END \n"
               + "                          )  \n"
               + "                          + '\" ' \n"
               + "                   FROM   runsheetconsignment rc, \n"
               + "                          runsheet r1 \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.branchcode = rc.branchcode \n"
               + "                          AND r1.routecode = rc.routecode \n"
               + "                          AND r1.createdBy = rc.createdBy \n"
               + "               ) \n"
               + "               WHEN cth.stateID = '10' \n"
               + "           AND cth.reason = 'DELIVERED' \n"
               + "           AND LEN(cth.riderName)  \n"
               + "               = 0 THEN (cth.reason)  \n"
               + "                \n"
               + "               ---- \n"
               + "               WHEN cth.StateID = '20' THEN ( \n"
               + "                   SELECT +'Material Arrival No :' + CONVERT(NVARCHAR, c.ArrivalID)  \n"
               + "                          + \n"
               + "                          ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                          + \n"
               + "                          ' by User :' + zu.Name + ' on Location :'  \n"
               + "                          + ec.name \n"
               + "                   FROM   MNP_MaterialArrival c, \n"
               + "                          MNP_MaterialArrivalDetail asd, \n"
               + "                          ZNI_USER1          zu, \n"
               + "                          Branches           b, \n"
               + "                          ExpressCenters     ec \n"
               + "                   WHERE  c.ArrivalID = asd.ArrivalID \n"
               + "                          AND CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                          AND zu.branchcode = b.branchCode \n"
               + "                          AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                          AND asd.ConsignmentNumber = cth.consignmentNumber \n"
               + "               ) \n"
               + "               ELSE '' \n"
               + "               END Detail \n"
               + "               FROM ( \n"
               + "                   SELECT * \n"
               + "                   FROM   ConsignmentsTrackingHistory \n"
               + "                   WHERE  consignmentNumber = '" + clvar.CNNumber + "'  \n"
               + "               ) cth \n"
               + "                \n"
               + "               WHERE -- mcts.[Active] = '1' \n"
               + "               cth.consignmentNumber = '" + clvar.CNNumber + "'  \n"
               + "               -- AND cth.stateID = '1' \n"
               + "               GROUP BY \n"
               + "               cth.consignmentNumber, \n"
               + "           -- mcts.StatusID, \n"
               + "           cth.stateID, \n"
               + "           cth.transactionTime, \n"
               + "           cth.currentLocation, \n"
               + "           cth.manifestNumber, \n"
               + "           cth.bagNumber, \n"
               + "           cth.SealNo, \n"
               + "           cth.loadingNumber, \n"
               + "           cth.ArrivalID, \n"
               + "           cth.runsheetNumber, \n"
               + "           cth.riderName, \n"
               + "           cth.reason \n"
               + "       ) b \n"
               + "       RIGHT OUTER JOIN MNP_ConsginmentTrackingStatus mcts \n"
               + "            ON  mcts.StatusID = b.stateID \n"
               + "WHERE  mcts.[Active] = '1' \n"
               + "GROUP BY \n"
               + "       mcts.TrackingStatus, \n"
               + "       b.currentLocation, \n"
               + "       b.Booked, \n"
               + "       b.consignmentNumber, \n"
               + "       b.Detail, \n"
               + "       mcts.sortorder,b.transactionTime \n"
               + "ORDER BY \n"
               + "CAST(  mcts.sortorder AS INT)  ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql_, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 30;
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

        public DataSet Get_ConsignmentTrackingHistory_Head(Variable clvar)
        {
            DataSet ds = new DataSet();

            try
            {
                string sqlString = "SELECT c.consignmentNumber,\n" +
          "       c.pieces,\n" +
          "       c.consigner,\n" +
          "       c.consignee              consignee,\n" +
          "       CONVERT(VARCHAR(10), c.bookingDate, 105) bookingDate,\n" +
          "       c.weight,\n" +
          "       c.serviceTypeName,\n" +
          "       r.receivedBy             ReceivedBy,\n" +
          "       CASE\n" +
          "            WHEN r.CurrentStatus IS NULL THEN s.TrackingStatus\n" +
          "            ELSE    (SELECT l.AttributeValue   \n" +
          "         FROM   rvdbo.Lookup l          \n" +
          "         WHERE  CONVERT(NVARCHAR,l.Id) =CONVERT(NVARCHAR, r.CurrentStatus))   \n" +
          "       END                      CurrentStatus,\n" +
          "       DeliveryTime_ DeliveryTime,\n" +
          "       bb.name                  Destination,\n" +
          "       c.consignerAccountNo     AccoutNo,\n" +
          "       b.name                   orign,\n" +
          "       r.delievryRider          delievryRider, c.address Address, c.consigneePhoneNo ConsigneeCellNo\n" +
          "FROM   Consignment c\n" +
          "       INNER JOIN Branches bb\n" +
          "            ON  c.destination = bb.branchCode\n" +
          "       INNER JOIN Branches b\n" +
          "            ON  c.orgin = b.branchCode\n" +
          "       LEFT JOIN (\n" +
          "              SELECT rc.consignmentNumber,\n" +
          "          rc.receivedBy       ReceivedBy,\n" +
          "           rc.Status           CurrentStatus,\n" +
          "           rc.deliveryDate   DeliveryTime,  rc.[time] DeliveryTime_,\n" +
          "           r.firstName + ' (' + r.riderCode + ' )' delievryRider, \n" +
          "        --   rs.runsheetNumber,\n" +
          "           rs.createdOn\n" +
          "    FROM   RunsheetConsignment rc,\n" +
          "		   Runsheet            rs,\n" +
          "          Riders              r--,\n" +
          "           --RiderRunsheet       rrs\n" +
          "    WHERE \n" +
          "            R.branchId = rs.branchCode\n" +
          "           AND R.routeCode = rs.routeCode\n" +
          "           and rc.branchcode = rs.branchCode\n" +
          "           and rc.createdBy = rs.createdBy\n" +
          "           and rs.runsheetNumber = rc.runsheetNumber\n" +
          "           and rc.consignmentNumber = '" + clvar._CNNumber + "' \n" +
          "           and rc.createdOn in (\n" +
          "              sELECT \n" +
          "              max(rc.createdOn)     \n" +
          "    FROM   RunsheetConsignment rc,\n" +
          "		   Runsheet            rs,\n" +
          "          Riders              r\n" +
          "     WHERE \n" +
          "            R.branchId = rs.branchCode\n" +
          "           AND R.routeCode = rs.routeCode\n" +
          "           and rc.branchcode = rs.branchCode\n" +
          "           and rc.createdBy = rs.createdBy\n" +
          "           and rs.runsheetNumber = rc.runsheetNumber\n" +
          "           and rc.consignmentNumber = '" + clvar._CNNumber + "'\n" +
          "            )) r\n" +
          "            ON  c.consignmentnumber = r.consignmentNumber\n" +
          "       INNER JOIN (\n" +
          "                SELECT MAX(t.id) ID,\n" +
          "                       t.consignmentNumber\n" +
          "                FROM   (Select * from Consignment_Tracking_View) \n" +
          "              t\n  GROUP BY\n" +
          "                       t.consignmentNumber\n" +
          "            ) \n" +
          "            ct\n" +
          "            ON  ct.consignmentNumber = c.consignmentNumber\n" +
          "       INNER JOIN (Select * from Consignment_Tracking_View) AS cth\n" +
          "            ON  cth.id = ct.ID\n" +
          "       INNER JOIN MNP_ConsginmentTrackingStatus AS s\n" +
          "            ON  s.StatusID = cth.stateID\n" +
          "WHERE  c.consignmentNumber = '" + clvar._CNNumber + "' ";



                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_loadinginfo(Variable clvar)
        {
            string sql = "SELECT v.name VehicaleType,mpl.VehicleRegNo, \n"
          + "       mpl.courierName, \n"
          + "       l.AttributeDesc Transport \n"
          + "FROM   MnP_Loading mpl \n"
          + "       INNER JOIN Vehicle v \n"
          + "            ON  mpl.vehicleId = v.id \n"
          + "       INNER JOIN rvdbo.Lookup l \n"
          + "            ON  l.Id = mpl.transportationType \n"
          + "WHERE  mpl.id = ( \n"
          + "           SELECT cth.loadingNumber \n"
          + "           FROM   ConsignmentsTrackingHistory cth \n"
          + "           WHERE  cth.id = ( \n"
          + "                      SELECT MAX(cth.id) \n"
          + "                      FROM   ConsignmentsTrackingHistory cth \n"
          + "                      WHERE  cth.stateID = '4' \n"
          + "                             AND cth.consignmentNumber = '" + clvar.CNNumber + "' \n"
          + "                  ) \n"
          + "       )";
            DataSet ds = new DataSet();

            try
            {
                //  string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' AND c.destination = '" + clvar._Designation + "' ";

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

        public DataSet Get_Destination()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM country order by name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Get_ConsignmentNumber(Variable clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = " select c.consignmentNumber \n" +
                                " from Consignment c\n" +
                                " where \n" +
                                " c.consignmentNumber = '" + clvar.ConsignmentNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Get_InternationConsignment(Variable clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = " select \n" +
                                " cc.accountNo, c.consignee, c.consigner, c.customerType, c.creditClientId, c.riderCode, c.orgin, c.destination, c.weight, c.serviceTypeName, \n" +
                                " CONVERT(VARCHAR(10), c.bookingDate, 105) bookingDate, c.pieces, c.chargedAmount, c.consigneePhoneNo, c.consignerCellNo ShipperPhone,  \n" +
                                " c.shipperAddress, c.Address ConsigneeAddress, c.destinationCountryCode, c.PakageContents, CONVERT(VARCHAR(10), \n" +
                                " c.accountReceivingDate, 105) accountReceivingDate, c.isApproved \n" +
                                " from Consignment c, InternationalConsignmentDetail cd, CreditClients cc \n" +
                                " where c.consignmentNumber = cd.consignmentNo\n" +
                                " and c.creditClientId = cc.id \n" +
                                " AND c.consignmentNumber = '" + clvar.ConsignmentNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Get_InternationConsignmentInvoiceStatus(Variable clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = " select ic.invoiceNumber,  ic.consignmentState from InvoiceConsignment ic where ic.consignmentNumber = '" + clvar.ConsignmentNo + "'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Get_ServiceTypes()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "select s.serviceTypeName from ServiceTypes s where s.IsIntl = '1' and s.status = '1' order by 1 ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public void Insert_InternationalConsignment(Variable clvar)
        {
            try
            {
                string query = "UPDATE Consignment SET \n" +
                                " isApproved = '" + clvar.Status + "', \n" +
                                " accountReceivingDate = '" + (DateTime.Parse(clvar.StartDate.ToString())).ToString("yyyy-MM-dd") + "', \n" +
                                clvar.Amount +
                                " modifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                " modifiedOn = GETDATE()  \n" +
                                " where consignmentNumber = '" + clvar.ConsignmentNo + "' ";

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



        #endregion

    }
}