using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MRaabta.Models;
using System.Configuration;
using System.Web.Mvc;

namespace MRaabta.Repo
{
    public class RouteRepo
    {
        SqlConnection con;
        public RouteRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            con.Close();
        }

        public async Task<ListOfTrackingNConsignmentDrop> GetRouteByRider(DateTime Date, string RiderId, string rop, string branchcode)
        {
            try
            {
                if (RiderId == "0")
                {
                    ListOfTrackingNConsignmentDrop listBoth = new ListOfTrackingNConsignmentDrop();

                    var rs = await con.QueryAsync<RiderRouteModel>(@"select distinct  rl.user_id  as Id,  rl.logTime, u.userName, latitude as Lat, longitude as Long, convert(bit, 1) as IsMarker
                            , u.userName + ' ('+ u.ridercode+') @ '+ convert(varchar(20), rl.logTime) as Details, rl.logTime as CreationDate
                            from [dbo].[App_RidersLocation] rl  
                            inner join (select user_id, max(logTime) as logTime, max(systime) as systime from [dbo].[App_RidersLocation] where cast(logTime as date) = cast(@Date as date) group by user_id) xb
                            on xb.user_id = rl.user_id and xb.logTime = rl.logTime and xb.systime = rl.systime
                            inner join [dbo].[App_Users] u on rl.user_id = u.user_id
                             where u.branchCode= @branchcode  and CAST(rl.logTime as date)= CAST(@Date as date);", new { @RiderId = RiderId, @Date = Date, @branchcode = branchcode });
                    listBoth.consignment = rs.ToList();
                    listBoth.tracking = null;
                    return listBoth;
                }

                else if (RiderId != "0" && rop == "Delivery")
                {
                    ListOfTrackingNConsignmentDrop listBoth = new ListOfTrackingNConsignmentDrop();
                    string query = "select row_number() over(order by  CAST(created_on AS date)) as Id,CAST(format(created_on,'HH:mm') as time) as [Created_on Time],' '+picker_name+ ' ' +reason + ' ' + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details, m.RunSheetNumber as Runsheet, relation, (CASE WHEN latitude = 0 and r.branchCode = '4' then  24.8354138 When latitude = 0 and r.branchCode = '43' then 33.6577612  When latitude = 0 and r.branchCode = '34' then 3	When latitude = 0 and r.branchCode = '1' then 1 ELSE latitude END) as Lat,((CASE WHEN longitude = 0 and r.branchCode = '4' then  67.0698366	When longitude = 0 and r.branchCode = '43' then 73.0493912 	When longitude = 0 and r.branchCode = '34' then 34	When longitude = 0 and r.branchCode = '1' then 1 ELSE longitude END)) as Long, picker_name as name,reason as Reason, 1 as IsMarker, (Select Top(1) ConsignmentNumber from App_Delivery_ConsignmentData ab where ab.RunSheetNumber = m.RunSheetNumber and cast(format(ab.created_on,'HH:mm') as time) = cast(format(ab.created_on,'HH:mm') as time) and ab.picker_name = m.picker_name and  ab.reason = m.reason and ab.longitude = m.longitude and ab.latitude = m.latitude) as consignmentNumber,cod_amount, pickupDetail = STUFF((          SELECT top(10) ' <br/>' + 'CONSIGNMENT NO: ' + md.ConsignmentNumber  FROM [App_Delivery_ConsignmentData] md WHERE md.longitude = m.longitude and md.latitude = m.latitude and md.RunSheetNumber = m.RunSheetNumber and cast(format(md.created_on,'HH:mm') as time) = cast(format(m.created_on,'HH:mm') as time) and md.picker_name = m.picker_name FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') +'<br/><br/><a href =\"/mraabtatest/deliverycoordinator/viewdelivery?" + Date.ToString("yyyy-MM-dd") + "&" + RiderId + "&" + "'+ m.RunSheetNumber +'" + "\" target=\"_blank\">View More</a>' from [App_Delivery_ConsignmentData] m inner join Runsheet r on r.runsheetNumber= m.RunSheetNumber where m.riderCode = @RiderId AND CAST(created_on as date) = CAST(@Date as date) group by cast(created_on as date),m.riderCode, m.RunSheetNumber, relation,latitude, longitude, picker_name, relation, reason, CAST(format(created_on,'HH:mm') as time), r.branchCode,cod_amount order by CAST(format(created_on,'HH:mm') as time)";
                    var rs = await con.QueryAsync<RiderRouteModel>(query, new { @RiderId = RiderId, @Date = Date });

                    // String query2 = "SELECT arl.latitude,arl.longitude  FROM [App_RidersLocation] arl  Inner JOIN App_Users au ON au.riderCode=@RiderTracking WHERE cast(arl.logTime AS date) =CAST(@DateTracking AS date) AND arl.[USER_ID]=au.[USER_ID] GROUP BY arl.latitude,arl.longitude";

                    //string query4 = "SELECT * FROM ( \n"
                    //       + "SELECT k.Latitude,k.Longitude,max(k.logTime) logTime \n"
                    //       + "  FROM   (           SELECT latitude AS Latitude, longitude  AS Longitude, m.created_on  AS logTime  \n"
                    //       + "           FROM   [App_Delivery_ConsignmentData] m  \n"
                    //       + "           WHERE  riderCode = @RiderTracking  \n"
                    //       + "                  AND CAST(created_on AS date) = CAST(@DateTracking AS date)  \n"
                    //       + "           UNION ALL	 \n"
                    //       + "           SELECT arl.latitude as Latitude,  \n"
                    //       + "                  arl.longitude as Longitude,  \n"
                    //       + "                  MAX(arl.logTime) logTime  \n"
                    //       + "           FROM   [App_RidersLocation] arl  \n"
                    //       + "                  INNER JOIN App_Users au  \n"
                    //       + "                       ON  au.riderCode = @RiderTracking \n"
                    //       + "           WHERE  CAST(arl.logTime AS date) = CAST(@DateTracking AS date)  \n"
                    //       + "                  AND arl.[USER_ID] = au.[USER_ID]  \n"
                    //       + "           GROUP BY  \n"
                    //       + "                  arl.latitude,  \n"
                    //       + "                  arl.longitude  \n"
                    //       + "       ) AS k \n"
                    //       + "        \n"
                    //       + "        \n"
                    //       + "GROUP BY k.Latitude,k.Longitude \n"
                    //       + ") AS s \n"
                    //       + "ORDER BY s.logTime";

                    String query4 = "  SELECT k.Latitude,k.Longitude,k.logTime logTime  \n"
                              + "                              FROM   (           SELECT ( CASE WHEN latitude = 0 and r.branchCode = '4' then  24.8354138 When latitude = 0 and r.branchCode = '43' then 33.6577612 When latitude = 0 and r.branchCode = '34' then 3  When latitude = 0 and r.branchCode = '1' then 1 ELSE latitude END) \n"
                              + " AS Latitude, \n"
                              + " (  CASE WHEN longitude = 0 and r.branchCode = '4' then  67.0698366 When longitude = 0 and r.branchCode = '43' then 73.0493912 When longitude = 0 and r.branchCode = '34' then 3 When longitude = 0 and r.branchCode = '1' then 1 ELSE longitude END)  AS Longitude, m.created_on  AS logTime   \n"
                              + "                                       FROM   [App_Delivery_ConsignmentData] m   \n"
                              + "                                       inner join Runsheet r on r.runsheetNumber = m.RunSheetNumber \n"
                              + "                                       WHERE  m.riderCode = @RiderTracking   \n"
                              + "                                              AND CAST(created_on AS date) = CAST(@DateTracking AS date)   \n"
                              + "                                       UNION ALL	  \n"
                              + "                                       SELECT (  CASE WHEN arl.latitude = 0 and au.branchCode = '4' then  4	When arl.latitude = 0 and au.branchCode = '43' then 33.6577612 When arl.latitude = 0 and au.branchCode = '34' then 3   When arl.latitude = 0 and au.branchCode = '1' then 1 ELSE arl.latitude END) as Latitude,   \n"
                              + "                                             ( 	 CASE WHEN arl.longitude = 0 and au.branchCode = '4' then  67.0698366 When arl.longitude = 0 and au.branchCode = '43' then 73.0493912 When arl.longitude = 0 and au.branchCode = '34' then 3 When arl.longitude = 0 and au.branchCode = '1' then 1  ELSE arl.longitude END) as Longitude,   \n"
                              + "                                              MAX(arl.logTime) logTime   \n"
                              + "                                       FROM   [App_RidersLocation] arl   \n"
                              + "                                              INNER JOIN App_Users au   \n"
                              + "                                                   ON  au.riderCode = @RiderTracking  \n"
                              + "                                       WHERE  CAST(arl.logTime AS date) = CAST(@DateTracking AS date)   \n"
                              + "                                              AND arl.[USER_ID] = au.[USER_ID]   \n"
                              + "                                       GROUP BY   \n"
                              + "                                              arl.latitude,   \n"
                              + "                                              arl.longitude,  \n"
                                + "                  au.branchCode \n"
                              + "                                   ) AS k  \n"
                              + "                                     \n"
                              + "                           ORDER BY k.logTime";

                    var rsTracking = await con.QueryAsync<LogTrackingModel>(query4, new { @RiderTracking = RiderId, @DateTracking = Date });

                    listBoth.consignment = rs.ToList();
                    listBoth.tracking = rsTracking.ToList();
                    return listBoth;
                }

                else
                {
                    ListOfTrackingNConsignmentDrop listBoth = new ListOfTrackingNConsignmentDrop();

                    string query = "select *, pickupDetail = STUFF((SELECT top(10) ' <br/>' + 'CONSIGNMENT NO ' + consignmentNumber + ' ' + cast(WEIGHT as varchar) + 'Kg ' + cast(pieces as varchar) + ' Pieces <a href=\"http://20.46.47.21/Mraabta/cnimage/' + consignmentNumber + '.jpg\" target=\"_blank\">View Image</a>' FROM App_PickUpChild md WHERE md.pickUp_ID = xb.id FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') +'<br/><br/> <a href=\"/MRaabtaTest/pickup/viewpickup?" + Date.ToString("yyyy-MM-dd") + "&" + RiderId + "\" target=\"_blank\">View More</a>' from(select top(10000) Id, user_id as RiderId, latitude as Lat, longitude as Long, convert(bit, 0) as IsMarker, '' as Details, '' as accountNo, '' as name, '' as locationName, logTime as CreationDate from [dbo].[App_RidersLocation] where USER_ID = (select top(1) user_id from App_Users where riderCode = @RiderId and status = 1) and CONVERT(date, logTime) = CONVERT(date, @Date) union select Pickup_ID as Id, p.createdBy as RiderId, latitude as Lat, longitude as Long, convert(bit, 1) as IsMarker,  isnull(locationName, '') + isnull(' (' + locationAddress + ')', '') + (select ' having ' + cast(count(pickupchild_id) as varchar(50)) + ' Items in ' + cast(sum(pieces) as varchar(10)) + ' No of Pieces of ' + cast(sum(weight) as varchar(10)) + 'Kg.'  from App_PickUpChild where pickUp_ID = p.PickUp_ID ) as Details,  c.accountNo, c.name, c.address as locationName, p.createdOn as CreationDate from[dbo].[App_PickUpMaster] p left join COD_CustomerLocations l on p.locationID = l.locationID left join CreditClients c on c.id = p.locationID where p.riderCode = @RiderId and CONVERT(date, p.createdOn) = CONVERT(date, @Date)) as xb where xb.Details like '%having%'; ";
                    var rs = await con.QueryAsync<RiderRouteModel>(query, new { @RiderId = RiderId, @Date = Date });

                    String query2 = "SELECT k.longitude as Longitude, \n"
                           + "       k.latitude as Latitude, \n"
                           + "       k.createdOn as logTime\n"
                           + "FROM   ( \n"
                           + "           SELECT apum.longitude, \n"
                           + "                  apum.latitude, \n"
                           + "                  apum.createdOn \n"
                           + "           FROM   App_PickUpMaster apum \n"
                           + "           WHERE  apum.riderCode = @RiderTracking \n"
                           + "                  AND CAST(apum.createdOn AS date) = CAST(@DateTracking  AS date)  \n"
                           + "            \n"
                           + "           UNION ALL \n"
                           + "           SELECT arl.latitude      AS Latitude, \n"
                           + "                  arl.longitude     AS Longitude, \n"
                           + "                  MAX(arl.logTime)     logTime \n"
                           + "           FROM   [App_RidersLocation] arl \n"
                           + "                  INNER JOIN App_Users au \n"
                           + "                       ON  au.riderCode = @RiderTracking  \n"
                           + "           WHERE  CAST(arl.logTime AS date) = CAST(@DateTracking  AS date) \n"
                           + "                  AND arl.[USER_ID] = au.[USER_ID] \n"
                           + "           GROUP BY \n"
                           + "                  arl.latitude, \n"
                           + "                  arl.longitude, \n"
                           + "                  au.branchCode \n" 
                           + "       ) k \n"
                           + "ORDER BY k.createdOn \n";

                    var rsTracking = await con.QueryAsync<LogTrackingModel>(query2, new { @RiderTracking = RiderId, @DateTracking = Date });
                    listBoth.consignment = rs.ToList();
                    listBoth.tracking = rsTracking.ToList();
                    return listBoth;
                }
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<DropDownModel>> GetRiders(string branchcode)
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"	SELECT u.riderCode as [Value],(r.riderCode + '-' + r.firstName + ' '+ r.lastName) as [Text], r.routeCode as [routecode] from riders  r 
                inner join App_Users u on u.riderCode = r.riderCode where r.branchId = @branchcode  and r.riderCode not like ' %' and r.status='1'
                union
                select '0' as [Value], 'All' as [Text] , '' as [routecode] from [dbo].[App_Users]
                order by Value", new { @branchcode = branchcode });
                return rs.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}