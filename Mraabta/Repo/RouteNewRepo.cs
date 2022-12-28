using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MRaabta.Models;
using System.Configuration;

namespace MRaabta.Repo
{
    public class RouteNewRepo
    {
        SqlConnection con;
        public RouteNewRepo()
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

        public async Task<List<RiderRouteModel>> GetRouteByRider(DateTime Date, string RiderId)
        {
            try
            {
                if (RiderId == "0")
                {
                    var rs = await con.QueryAsync<RiderRouteModel>(@"SELECT rtid as id, ur.id as riderid, latitude as Lat, longitude as Long, 'abc' as Details, logDateTime as CreatedDate 
                                                                        FROM [APL_BTS].[dbo].[App_RouteTracking] rt
                                                                        inner join [APL_BTS].[dbo].[tblapprouteusers] ur on ur.mobileid = rt.deviceName
                                                                        where CONVERT(date, logDateTime) = CONVERT(date,@Date)
                                                                        order by sysDateTime;", new { RiderId, Date });
                    return rs.ToList();
                }
                else
                {
                    var rs = await con.QueryAsync<RiderRouteModel>(@"select rtid as id, id as riderid, latitude as Lat, longitude as Long, convert(bit, isMarker) as IsMarker, detail as Details, logDateTime as CreatedDate from (
                                                                        select row_number() over (order by logDateTime) as rtid, * from (
                                                                        select srt.logDateTime, srt.devicename, srt.latitude, srt.longitude, 1 as isMarker, 'start: ' + convert(varchar, srt.logDateTime, 8) as detail from App_RouteTracking srt inner join(
                                                                        select deviceName, min(logDateTime) as logtime
                                                                        from App_RouteTracking 
                                                                        where CONVERT(date, logdatetime) = CONVERT(date,@Date) and isPickup = 0 and isstart = 1
                                                                        and left(deviceName,5) = (select left(mobileid,5) from [tblAppRouteUsers] where id = @RiderId)
                                                                        group by deviceName) ssrt on ssrt.logtime = srt.logDateTime and ssrt.deviceName = srt.deviceName
                                                                        union
                                                                        select min(logDateTime) as logDateTime, deviceName, latitude, longitude, 1 as isMarker, 'reached :' + convert(varchar, min(logDateTime), 8) + '-' + convert(varchar, max(logDateTime), 8) as detail
                                                                        from App_RouteTracking 
                                                                        where CONVERT(date, logdatetime) = CONVERT(date,@Date) and isPickup = 0 and isReach = 1
                                                                        and left(deviceName,5) = (select left(mobileid,5) from [tblAppRouteUsers] where id = @RiderId)
                                                                        group by deviceName, latitude, longitude
                                                                        union
                                                                        select top(1) logDateTime, deviceName, latitude, longitude, 1 as isMarker, 'End :' + convert(varchar, logDateTime, 8) as detail
                                                                        from App_RouteTracking 
                                                                        where CONVERT(date, logdatetime) = CONVERT(date,@Date) and isPickup = 0 and isEnd = 1
                                                                        and left(deviceName,5) = (select left(mobileid,5) from [tblAppRouteUsers] where id = @RiderId)
                                                                        --order by logDateTime desc
                                                                        union
                                                                        select logDateTime, deviceName, latitude, longitude, 0 as isMarker, detail from (
                                                                        select logDateTime, deviceName, latitude, longitude, 'on way' as detail , row_number() over (order by logDateTime) RowNumber
                                                                        from App_RouteTracking 
                                                                        where CONVERT(date, logdatetime) = CONVERT(date,@Date) and isPickup = 0 and isReach = 0 and isEnd = 0
                                                                        and left(deviceName,5) = (select left(mobileid,5) from [tblAppRouteUsers] where id = @RiderId)) as xb where RowNumber != 1
                                                                        ) as xxb
                                                                        inner join [dbo].[tblAppRouteUsers] aru on left(xxb.deviceName,5) = aru.mobileid and aru.isActive = 1) as xcb;", new { RiderId, Date });
                    return rs.ToList();                    
                }
                //var rs = await con.QueryAsync<RiderRouteModel>(@"SELECT * FROM RiderRoutes where RiderId = @RiderId and CONVERT(date, CreationDate) = CONVERT(date,@Date);", new { RiderId, Date });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<DropDownModel>> GetRiders()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select id as [value], userid as [Text] from tblAppRouteUsers
                                                                union
                                                                select 0 as [Value], 'All' as [Text]
                                                            ");
                return rs.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}