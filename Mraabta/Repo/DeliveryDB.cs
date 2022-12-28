using Dapper;
using Microsoft.Ajax.Utilities;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MRaabta.Repo
{
    public class DeliveryDB
    {
        SqlConnection orcl;
        public DeliveryDB()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await orcl.OpenAsync();
        }
        public void Close()
        {
            orcl.Close();
        }

        public async Task<List<CoordinatesDataM>> GetCoordinatesUnionLog(string riderCode, string StartDate, string EndDate, string branchcode, string allRunsheets)
        {
            try
            {
                string sql = " \n"
           + "SELECT k.createdOn     createdOn, \n"
           + "       k.latitude, \n"
           + "       k.longitude \n"
           + "FROM   ( \n"
           + "           SELECT MAX(arl.logTime) createdOn, \n"
           + "                  arl.latitude, \n"
           + "                  arl.longitude \n"
           + "           FROM   App_RidersLocation arl \n"
           + "                  INNER JOIN App_Users au \n"
           + "                       ON  au.[USER_ID] = arl.[USER_ID] \n"
           + "           WHERE  au.riderCode = '" + riderCode + "' \n"
           + "                  AND CAST(arl.logTime AS date) >= CAST('" + StartDate + "' AS date) \n"
           + "                  AND CAST(arl.logTime AS DATE) <= CAST('" + EndDate + "' AS date) \n"
           + "           GROUP BY \n"
           + "                  arl.longitude, \n"
           + "                  arl.latitude \n"
           + "           UNION ALL	 \n"
           + "           SELECT MAX(adcd.created_on) createdOn, \n"
           + "                  adcd.latitude, \n"
           + "                  adcd.longitude \n"
           + "           FROM   App_Delivery_ConsignmentData adcd \n"
           + "           WHERE  adcd.riderCode = '" + riderCode + "' \n"
           + "                  AND adcd.RunSheetNumber IN (" + allRunsheets + ") \n"
           + "           GROUP BY \n"
           + "                  adcd.longitude, \n"
           + "                  adcd.latitude \n"
           + "       )             k \n"
           + "      WHERE k.longitude!=0 AND k.latitude!=0"
           + "GROUP BY \n"
           + "       k.longitude, \n"
           + "       k.latitude, \n"
           + "       k.createdOn \n"
           + "ORDER BY \n"
           + "       k.createdOn \n"
           + " \n"
           + "";

                var rs = await orcl.QueryAsync<CoordinatesDataM>(sql);
                return rs.ToList();
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public async Task<List<GetDeliveryData>> getDelivery(string riderCode, string StartDate, string EndDate, String branchCode)
        {
            try
            {
                var data = new List<GetDeliveryData>();
                var query = @"select 
                            r.runsheetDate as RunsheedDate,
                            r.createdOn as CreationDate,
                            r.runsheetNumber as RS,
                            rf.runsheetNo as FetchedRS,
                            rc.consignmentNumber as CN,
                            isnull(d.StatusId,0) as StatusId,
                            d.performed_on as PerformedOn,
                            d.latitude as Lat,
                            d.longitude as Long,
                            d.picker_name as Receiver,
                            d.reason as Reason
                            from Runsheet r
                            inner join RunsheetConsignment rc on rc.runsheetNumber = r.runsheetNumber
                            left join App_Delivery_RunsheetFetched rf on rf.runsheetNo = r.runsheetNumber
                            left join App_Delivery_ConsignmentData d on d.RunSheetNumber = rc.runsheetNumber and d.ConsignmentNumber = rc.consignmentNumber
                            where cast(r.runsheetDate as date) between @from and @to and r.branchCode = @bc and r.ridercode = @rc;";

                var rs = await orcl.QueryAsync<(DateTime RunsheedDate, DateTime CreationDate, string RS, string FetchedRS, string CN, int StatusId, DateTime? PerformedOn, float? Lat, float? Long, string Receiver, string Reason)>(query, new { @rc = riderCode, @from = StartDate, @to = EndDate, @bc = branchCode }, commandTimeout: int.MaxValue);

                foreach (var item in rs.GroupBy(x => x.RS))
                {
                    var z = new GetDeliveryData();
                    z.RunsheetNumber = long.Parse(item.Key);
                    z.RunsheetDate = item.FirstOrDefault().RunsheedDate.ToString("dd/MM/yyyy");
                    z.RUNSHEETTIME = item.FirstOrDefault().CreationDate.ToString("hh:mm tt");
                    z.PODCN = item.Where(y => y.StatusId > 0).Select(x => x.CN).Distinct().Count();
                    z.TotalCN = item.Select(x => x.CN).Distinct().Count();
                    z.DLVCN = item.Where(y => y.StatusId == 1).Select(x => x.CN).Distinct().Count();
                    var touchpointcount = item.Where(y => y.Lat.HasValue && y.Long.HasValue).GroupBy(y => new { y.Lat, y.Long, ReceiverOrReason = new List<int> { 1, 3 }.Contains(y.StatusId) ? y.Receiver : y.Reason }).Count();
                    z.Touchpoints = touchpointcount.ToString();
                    if (touchpointcount == 1)
                    {
                        z.TotalTimeTaken = "Single Touchpoint";
                    }
                    else if (touchpointcount > 1)
                    {

                        var t1 = item.Min(y => y.PerformedOn)?.TimeOfDay;
                        var t2 = item.Max(y => y.PerformedOn)?.TimeOfDay;
                        var t3 = t2 - t1;
                        z.TotalTimeTaken = t3?.ToString(@"hh\:mm\:ss");
                    }
                    else
                    {
                        z.TotalTimeTaken = "-";
                    }

                    data.Add(z);
                }

                return data;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public async Task<List<CoordinatesDataM>> GetCoordinatesFromAllRunsheets(string allRunsheets, string branchForCoordinates)
        {
            try
            {
                string sql = " \n"
                   + "SELECT adcd.RunSheetNumber, \n"
                   + "       adcd.longitude, \n"
                   + "       adcd.latitude,CAST(adcd.created_on AS date)  createdOn \n"
                   + "FROM   App_Delivery_ConsignmentData adcd \n"
                   + "WHERE  adcd.RunSheetNumber IN (" + allRunsheets + ") \n"
                   + "       AND adcd.longitude != 0 \n"
                   + "       AND adcd.latitude != 0 \n"
                   + "GROUP BY \n"
                   + "       adcd.RunSheetNumber, \n"
                   + "       adcd.longitude, \n"
                   + "       adcd.latitude,CAST(adcd.created_on AS date) \n";

                string sql2 = "";
                if (branchForCoordinates == "43")
                {
                    sql2 = @"SELECT adcd.RunSheetNumber,adcd.longitude,
                           adcd.latitude
                    FROM   App_Delivery_ConsignmentData adcd
                    WHERE  adcd.RunSheetNumber IN (" + allRunsheets + @")
		                       AND adcd.longitude != 0
		                       AND adcd.latitude != 0
                    GROUP BY
                    adcd.RunSheetNumber,
                           adcd.longitude,
                           adcd.latitude

                    UNION ALL 

                    SELECT adcd.RunSheetNumber,73.0493912 longitude,
                           33.6577612 latitude
                    FROM   App_Delivery_ConsignmentData adcd
                    WHERE  adcd.RunSheetNumber IN (" + allRunsheets + @")
                    AND			adcd.longitude != 0
		                       AND adcd.latitude != 0
                    GROUP BY adcd.RunSheetNumber

                    ORDER BY adcd.RunSheetNumber
                    ";
                }
                else if (branchForCoordinates == "4")
                {
                    sql2 = @"SELECT adcd.RunSheetNumber,adcd.longitude,
                           adcd.latitude
                    FROM   App_Delivery_ConsignmentData adcd
                    WHERE  adcd.RunSheetNumber IN (" + allRunsheets + @")
		                       AND adcd.longitude != 0
		                       AND adcd.latitude != 0
                    GROUP BY
                    adcd.RunSheetNumber,
                           adcd.longitude,
                           adcd.latitude

                    UNION ALL 

                    SELECT adcd.RunSheetNumber,67.0698366 longitude,
                           24.8354138 latitude
                    FROM   App_Delivery_ConsignmentData adcd
                    WHERE  adcd.RunSheetNumber IN (" + allRunsheets + @")
                    AND			adcd.longitude != 0
		                       AND adcd.latitude != 0
                    GROUP BY adcd.RunSheetNumber

                    ORDER BY adcd.RunSheetNumber
                    ";
                }
                else
                {
                    sql2 = @"SELECT adcd.RunSheetNumber,adcd.longitude,
                           adcd.latitude
                    FROM   App_Delivery_ConsignmentData adcd
                    WHERE  adcd.RunSheetNumber IN (" + allRunsheets + @")
		                       AND adcd.longitude != 0
		                       AND adcd.latitude != 0
                    GROUP BY
                    adcd.RunSheetNumber,
                           adcd.longitude,
                           adcd.latitude
                 
                    ORDER BY adcd.RunSheetNumber
                    ";
                }
                var rs = await orcl.QueryAsync<CoordinatesDataM>(sql);
                return rs.ToList();
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public async Task<List<Delivery_Child_details>> getDeliveryDetails(Int64 Runsheet_ID, string branchcode)
        {
            try
            {

                string sql = @"
             SELECT
                isnull(appdt.Consign_id,0) as Consign_id, isnull(appdt.verify,0) verify, appdt.comments,
                rc.SortOrder, c.consignmentNumber, c.remarks, c.pieces, c.weight, b.sname OriginCity, b2.sname destination, c.consignee ConsigneeName, isnull(cd.codAmount,0) codAmount,
                isnull(appdt.rider_comments,'') rider_comments, isnull(appdt.pickerPhone_No,0) pickerPhone_No,isnull(appdt.rider_iemi,'') imei, nic_number,
                isnull(ri.riderCode + ' ' + ri.firstName + ' ' + ri.lastName, '-')  Rider, appdt.picker_name Receiver, appdt.relation , isnull(appdt.reason,'') as reason, case when appdt.ConsignmentNumber is null then 1 else 0 end as ispending,
                case when appdt.ConsignmentNumber is not null and (appdt.relation is null or appdt.relation = '') then 0 else 1 end isDelivered, isnull(appdt.nic_number,0) as nic_number,
                case when appdt.ConsignmentNumber is not null  then CONVERT(VARCHAR, appdt.created_on, 103) else 'null' end as DeliveredDate,
                case when appdt.ConsignmentNumber is not null then LTRIM(RIGHT(CONVERT(VARCHAR(20), appdt.created_on, 100), 7)) else 'null' end as DeliveredTime,
                appdt.longitude,appdt.latitude
                FROM Runsheet R
                INNER JOIN RunsheetConsignment rc ON  R.runsheetNumber = rc.runsheetNumber AND R.branchCode = rc.branchcode AND R.routeCode = rc.RouteCode
                INNER JOIN Consignment c ON C.consignmentNumber = rc.consignmentNumber
                INNER JOIN Branches B ON B.branchCode = C.orgin
                INNER JOIN Branches b2 ON c.destination = b2.branchCode
                LEFT OUTER JOIN CODConsignmentDetail_New cd ON  cd.consignmentNumber = c.consignmentNumber
                LEFT OUTER JOIN Riders ri ON  rc.branchcode = ri.branchId AND rc.GivenToRider = ri.riderCode
                left join App_Delivery_ConsignmentData appdt on rc.runsheetNumber = appdt.RunSheetNumber and c.consignmentNumber = appdt.ConsignmentNumber
                WHERE rc.runsheetNumber = '" + Runsheet_ID + "' AND (R.branchCode = '" + branchcode + "') order by rc.SortOrder desc";


                var rs = await orcl.QueryAsync<Delivery_Child_details>(sql);
                return rs.ToList();
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public async Task<List<DropDownModel>> GetRiders(string SDate, string EDate, string BranchCode)
        {
            try
            {
                string sql = @"select
                            DISTINCT Appdt.RIDERCODE as Value  ,
                            (select top 1 (r.firstName + ' '+ r.lastName) from Riders r
                            where r.riderCode = Appdt.riderCode AND r.branchId = '" + BranchCode + @"') as Text
                            FROM   App_Delivery_ConsignmentData Appdt
                            inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                            WHERE R.BRANCHCODE = '" + BranchCode + "' AND cast(R.RUNSHEETDATE as date) between '" + SDate + "' and '" + EDate + "'";
                var rs = await orcl.QueryAsync<DropDownModel>(sql, new { @bc = BranchCode });
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> Update(Delivery_Child_details model)
        {
            try
            {


                var sqlStatement = @"UPDATE App_Delivery_ConsignmentData 
                                     SET verify = @verify,
                                         comments = @comments
                                     WHERE Consign_id = @Consign_id;";

                await orcl.ExecuteAsync(sqlStatement, model);

                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Status>> TotalReasonPoints(string ridercode, string StartDate, string EndDate, string branchcode)
        {
            try
            {
                var query = $@"select 
                            r.runsheetNumber as TRS,
                            rf.runsheetNo as DRS,
                            rc.consignmentNumber as CN,
                            ISNULL(dd.StatusId,0) as StatusId,
                            dd.latitude as Lat,
                            dd.longitude as Long,
                            dd.rider_iemi as RiderImei,
                            dd.performed_on as PerformedOn,
                            b.name as City,
                            concat(ri.firstName,' ',ri.lastName) as Courier,
                            rou.name as [Route],
                            dd.picker_name as Receiver,
                            dd.reason as Reason
                            from Runsheet r
                            inner join RunsheetConsignment rc on r.runsheetNumber = rc.runsheetNumber
                            inner join Riders ri on ri.riderCode = r.ridercode and ri.branchId = r.branchCode
                            inner join Routes rou on rou.routeCode = r.routeCode and rou.BID = ri.branchId
                            inner join Branches b on b.branchCode = r.branchCode
                            left join App_Delivery_RunsheetFetched rf on r.runsheetNumber = rf.runsheetNo
                            left join App_Delivery_ConsignmentData dd on dd.RunSheetNumber = r.runsheetNumber and dd.ConsignmentNumber = rc.consignmentNumber
                            where cast(r.runsheetDate as date) between '{StartDate}' and '{EndDate}' and r.branchCode = {branchcode} and r.ridercode = '{ridercode}';";

                var rs = await orcl.QueryAsync<(string TRS, string DRS, string CN, int StatusId, float? Lat, float? Long, string RiderImei, DateTime? PerformedOn, string City, string Courier, string Route, string Receiver, string Reason)>(query, commandTimeout: int.MaxValue);

                var tp = rs.Where(x => x.Lat.HasValue && x.Long.HasValue).GroupBy(x => new { x.TRS, x.Lat, x.Long, ReceiverOrReason = new List<int> { 1, 3 }.Contains(x.StatusId) ? x.Receiver : x.Reason }).Count();

                TimeSpan? time = TimeSpan.Parse("00:00:00");

                foreach (var item in rs.Where(x => x.PerformedOn.HasValue).GroupBy(x => x.PerformedOn.Value.Date))
                {
                    var min = item.Min(x => x.PerformedOn)?.TimeOfDay;
                    var max = item.Max(x => x.PerformedOn)?.TimeOfDay;
                    time += (max - min);
                }

                var data = new List<Status>
                {
                    new Status
                    {
                        TotalRunsheet = rs.Select(x => x.TRS).Distinct().Count(),
                        DownloadedRunsheet = rs.Where(x => !string.IsNullOrEmpty(x.DRS)).Select(x => x.DRS).Distinct().Count(),
                        City = rs.FirstOrDefault().City,
                        Courier = rs.FirstOrDefault().Courier,
                        delivered = rs.GroupBy(x => x.TRS).Select(x => x.Where(y => y.StatusId == 1).Select(y => y.CN).Distinct().Count()).Sum(),
                        deliveredRts = rs.GroupBy(x => x.TRS).Select(x => x.Where(y => y.StatusId == 3).Select(y => y.CN).Distinct().Count()).Sum(),
                        undelivered = rs.GroupBy(x => x.TRS).Select(x => x.Where(y => y.StatusId == 2).Select(y => y.CN).Distinct().Count()).Sum(),
                        Pending = rs.GroupBy(x => x.TRS).Select(x => x.Where(y => y.StatusId == 0).Select(y => y.CN).Distinct().Count()).Sum(),
                        Rider_IEMI = rs.FirstOrDefault(x => !string.IsNullOrEmpty(x.RiderImei)).RiderImei,
                        Route = rs.FirstOrDefault().Route,
                        TCNDownloaded = rs.Where(x => !string.IsNullOrEmpty(x.DRS)).GroupBy(x => x.TRS).Select(x => x.Select(y => y.CN).Distinct().Count()).Sum(),
                        TotalCN = rs.GroupBy(x => x.TRS).Select(x => x.Select(y => y.CN).Distinct().Count()).Sum(),
                        Touchpoints = tp,
                        TotalTimeTaken = time?.ToString(@"hh\:mm\:ss")
                    }
                };


                return data;
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Points>> TotalPoints(string ridercode, string StartDate, string EndDate)
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 5000;
                var rs = await orcl.QueryAsync<Points>(@"
                     Select top 1 Repeated_times, reason as [Reason] from (
                    select Count(v.Maxx) [Repeated_times]
                    ,v.Maxx [reason] from(
                    select  MAX(appdt.reason) OVER(PARTITION BY  appdt.reason) AS 'Maxx'
                    from RUNSHEET r
                    INNER JOIN RunsheetConsignment rc ON R.runsheetNumber = rc.runsheetNumber AND R.branchCode = rc.branchcode AND R.routeCode = rc.RouteCode
                    INNER JOIN Consignment c ON C.consignmentNumber = rc.consignmentNumber
                    left join App_Delivery_ConsignmentData appdt on rc.runsheetNumber = appdt.RunSheetNumber and c.consignmentNumber = appdt.ConsignmentNumber
                    where r.ridercode =  @rc and cast(r.runsheetDate as date) between '" + StartDate + "' and '" + EndDate + "' and appdt.reason!='' and appdt.reason is not null)  as v group by v.Maxx ) as k order by Repeated_times desc",
               new { @rc = ridercode, @sd = StartDate, @ed = EndDate });

                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<LatLongModel>> GetRiderLatLongs(string rider, string from, string to, string branch)
        {
            try
            {
                var rs = await orcl.QueryAsync<LatLongModel>($@"with t as (select isnull(rl.latitude,0) as Lat, isnull(rl.longitude,0) as Long, rl.logTime as CreatedOn from App_RidersLocation rl
                                                            inner join App_Users u on rl.USER_ID = u.USER_ID
                                                            where u.branchCode = '{branch}'
                                                            and u.riderCode = '{rider}'
                                                            and cast(rl.logTime as date) between '{from}' and '{to}'
                                                            union
                                                            select ISNULL(latitude,0) as Lat, ISNULL(longitude,0) as Long, performed_on as CreatedOn from App_Delivery_ConsignmentData
                                                            where cast(performed_on as date) between '{from}' and '{to}' and riderCode = '{rider}')
                                                            select cast(t.CreatedOn as date) as CreatedOn, t.Lat, t.Long from t
                                                            group by cast(t.CreatedOn as date), t.Lat, t.Long;");
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<TimeTakenModel>> GetTotalTimeTaken(string rider, string from, string to, string branch)
        {
            try
            {
                //await orcl.OpenAsync();
                var rs = await orcl.QueryAsync<TimeTakenModel>(@"select 
                                                                CAST(appd.performed_on AS DATE) as [Date],
                                                                MIN(appd.performed_on) as MinDate,
                                                                max(appd.performed_on) as MaxDate 
                                                                from App_Delivery_ConsignmentData appd
                                                                inner join Runsheet r on r.runsheetNumber = appd.RunSheetNumber
                                                                where r.branchCode = @branch and appd.riderCode = @rider
                                                                and cast(appd.performed_on as date) between @from and @to
                                                                GROUP BY CAST(appd.performed_on AS DATE);", new { rider, from, to, branch });
                //orcl.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                //orcl.Close();
                return null;
            }
            catch (Exception ex)
            {
                //orcl.Close();
                return null;
            }
        }
    }
}