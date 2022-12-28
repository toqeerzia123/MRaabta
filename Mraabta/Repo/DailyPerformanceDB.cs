using Dapper;
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
    public class DailyPerformanceDB
    {
        SqlConnection orcl;
        public DailyPerformanceDB()
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
        public async Task<List<DropDownModel>> GetRiders(string Date, string branchcode)
        {
            try
            {
                string sql = @"SELECT
                    DISTINCT Appdt.RIDERCODE as Value  ,
                    (Select Top(1) userName from App_Users where ridercode=Appdt.riderCode AND branchCode = @bc) as Text
                    FROM   App_Delivery_ConsignmentData Appdt
                    inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                    WHERE R.BRANCHCODE = @bc AND cast(R.RUNSHEETDATE as date) = @Date";
                var rs = await orcl.QueryAsync<DropDownModel>(sql, new { @Date = Date, @bc = branchcode });
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

        public async Task<List<MonthlyStats>> TotalMonthlyCounts(string Date, string ridercode, string branchcode)
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                var rs = await orcl.QueryAsync<MonthlyStats>(@"
                  select
                (select count(r.runsheetNumber) from Runsheet r inner join (select distinct RunsheetNo from 
                App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber where r.routeCode in
                (select routeCode from Riders where riderCode in(SELECT DISTINCT Appdt.RIDERCODE as Value  
                FROM   App_Delivery_ConsignmentData Appdt inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                WHERE cast(R.RUNSHEETDATE as date) = @Date ))
                and cast(r.runsheetDate as date) = @Date and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)) as DownloadedRunsheet,
                (select COUNT(consignmentNumber) from RunsheetConsignment rc 
                INNER JOIN Runsheet r on rc.runsheetNumber = r.runsheetNumber and rc.branchcode = r.branchCode AND rc.RouteCode = r.routeCode
                inner join (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = R.RUNSHEETNUMBER
                WHERE cast(r.runsheetDate as date) = @Date and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)) as TCNDownloaded,
                (select count(runsheetNumber) from Runsheet r  
                inner join 
                (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber
                where cast(r.runsheetDate as date) = @Date and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)) 
                as DownloadedRunsheet,
                (select 
                LEFT(Cast(DATEADD(ms, SUM(DATEDIFF(ms, '00:00:00.000', bts.abc)), '00:00:00.000') as time),5) as time
                --Sum(bts.abc)
                from (
                select 
                LEFT(cast(max(cast(cast(xb.[Created_on Time] as time) as datetime)) -  min(cast(cast(xb.[Created_on Time] as time) as datetime)) as time),5) as abc
                --,xb.date
                from
                (
                select row_number() over(order by  CAST(created_on AS date)) as Id, CAST(created_on as date) as date,
                CAST(format(created_on,'HH:mm') as time) as [Created_on Time],' ' 
                +picker_name+ ' ' +reason + ' ' + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details,
                RunSheetNumber as Runsheet, relation, latitude as Lat,longitude as Long, picker_name as name,
                reason as Reason, 1 as IsMarker, (Select Top(1) ConsignmentNumber from App_Delivery_ConsignmentData ab 
                where ab.RunSheetNumber = m.RunSheetNumber and cast(format(ab.created_on,'HH:mm') as time) = cast(format(ab.created_on,'HH:mm') as time) 
                and ab.picker_name = m.picker_name and  ab.reason = m.reason and ab.longitude = m.longitude and ab.latitude = m.latitude) as consignmentNumber
                from [App_Delivery_ConsignmentData] m 
                where riderCode = @rc and CAST(created_on as date) = @Date
                group by cast(created_on as date),riderCode, RunSheetNumber, relation,latitude, longitude, picker_name, relation, reason, 
                CAST(format(created_on,'HH:mm') as time) 
                )
                as xb
                group by xb.date
                )
                as bts
                )  as TotalTimeTaken ,
                (
                SELECT COUNT(xb.Id)
                FROM   ( SELECT ROW_NUMBER() OVER(ORDER BY CAST(m.created_on AS date)) AS Id ,CAST(FORMAT(created_on ,'HH:mm') AS TIME) AS [Created_on Time]  
                ,' '+picker_name+ ' '+m.reason+' '+LEFT( CAST(CAST(FORMAT(created_on ,'HH:mm') AS TIME) AS VARCHAR(MAX)) ,5 )                 AS Details
                ,m.RunSheetNumber  AS Runsheet ,m.relation ,latitude          AS Lat ,longitude         AS Long ,picker_name       AS NAME ,m.reason            AS Reason  ,1                 AS IsMarker
                ,(
                SELECT TOP(1) ConsignmentNumber
                FROM   App_Delivery_ConsignmentData ab
                WHERE  ab.RunSheetNumber = m.RunSheetNumber
                AND CAST(FORMAT(ab.created_on ,'HH:mm') AS TIME) = CAST(FORMAT(ab.created_on ,'HH:mm') AS TIME)
                AND ab.picker_name = m.picker_name
                AND ab.reason = m.reason
                AND ab.longitude = m.longitude
                AND ab.latitude = m.latitude
                )                 AS consignmentNumber

                FROM   [App_Delivery_ConsignmentData] m
                INNER JOIN Runsheet R
                ON  r.runsheetNumber = m.RunSheetNumber
                INNER JOIN RunsheetConsignment rc ON
                r.runsheetNumber = rc.runsheetNumber
                WHERE m.ridercode = '" + ridercode + @"' and R.BRANCHCODE = '" + branchcode + @"'
                AND CAST(R.RUNSHEETDATE AS date) = @Date
                GROUP BY CAST(m.created_on AS date) ,m.riderCode ,m.RunSheetNumber ,m.relation,latitude ,longitude,picker_name,m.reason,CAST(FORMAT(m.created_on ,'HH:mm') AS TIME)) AS xb) as [Touchpoints],
                SUM(case when isnull(appdt.StatusId,0) = 1 then 1 else 0 end) delivered,
                SUM(case when isnull(appdt.StatusId,0) = 2 then 1 else 0 end) undelivered,
                SUM(case when isnull(appdt.StatusId,0) = 3 then 1 else 0 end) deliveredRts
                from App_Delivery_ConsignmentData appdt
                inner join runsheet r on r.runsheetNumber = appdt.RunSheetNumber
                where cast(r.runsheetDate as date) = @Date and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)
                    ", new { @bc = branchcode, @rc = ridercode, @Date = Date });
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

    }
}