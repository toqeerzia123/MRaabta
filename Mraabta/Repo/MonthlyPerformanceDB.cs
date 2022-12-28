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
    public class MonthlyPerformanceDB
    {
        SqlConnection orcl;
        public MonthlyPerformanceDB()
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
        public async Task<List<DropDownModel>> GetRiders(string Year, string Month, string branchcode)
        {
            try
            {
                string sql = @"SELECT
                    DISTINCT Appdt.RIDERCODE as Value  ,
                    (Select Top(1) userName from App_Users where ridercode=Appdt.riderCode AND branchCode = @bc) as Text
                    FROM   App_Delivery_ConsignmentData Appdt
                    inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                    WHERE R.BRANCHCODE = @bc AND cast(R.RUNSHEETDATE as date) LIKE '" + Year + "-" + Month + "%';";
                var rs = await orcl.QueryAsync<DropDownModel>(sql, new { @Year = Year, @Month = Month, @bc = branchcode });
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

        public async Task<List<DataPoint>> TotalMonthlyStats(string ridercode, string Year, string Month, string branchcode)
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                var rs = await orcl.QueryAsync<DataPoint>(@"
                   select  cast(xy.runsheetDate as varchar) as X, Y  from (select runsheetDate, branchCode,routeCode,
                    sum(B) as Y from (
                    select r.runsheetDate, r.branchCode, r.routeCode, case when isnull(relation, '') <> '' then 1 else 0 end as B
                    from App_Delivery_ConsignmentData rf
                    inner join Runsheet r on r.runsheetNumber = rf.RunSheetNumber) as xb
                    where branchCode=@bc and routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)
                    group by runsheetDate, branchCode,routeCode
                    ) as xy
                    where cast(xy.runsheetDate as date) LIKE '" + Year + "-" + Month + "%' order by xy.runsheetDate", new { @bc = branchcode, @rc = ridercode });
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

        public async Task<List<MonthlyStats>> TotalMonthlyCounts(string ridercode, string Year, string Month, string branchcode)
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
                    WHERE cast(R.RUNSHEETDATE as date) LIKE '" + Year + @"-" + Month + @"%' ))
                    and cast(r.runsheetDate as date) LIKE '" + Year + @"-" + Month + @"%' and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)) as DownloadedRunsheet,
                    (select COUNT(consignmentNumber) from RunsheetConsignment rc 
                    INNER JOIN Runsheet r on rc.runsheetNumber = r.runsheetNumber and rc.branchcode = r.branchCode AND rc.RouteCode = r.routeCode
                    inner join (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = R.RUNSHEETNUMBER
                    WHERE cast(r.runsheetDate as date) LIKE '" + Year + @"-" + Month + @"%' and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)) as TCNDownloaded,
                    (select count(runsheetNumber) from Runsheet r  
                    inner join 
                    (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber
                    where cast(r.runsheetDate as date) LIKE '" + Year + @"-" + Month + @"%' and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)) 
                    as DownloadedRunsheet,
                      SUM(case when isnull(appdt.StatusId,0) = 1 then 1 else 0 end) delivered,
                    SUM(case when isnull(appdt.StatusId,0) = 2 then 1 else 0 end) undelivered,
                    SUM(case when isnull(appdt.StatusId,0) = 3 then 1 else 0 end) deliveredRts
                    from App_Delivery_ConsignmentData appdt
                    inner join runsheet r on r.runsheetNumber = appdt.RunSheetNumber
                    where cast(r.runsheetDate as date) LIKE '" + Year + @"-" + Month + @"%' and r.branchCode=@bc and r.routeCode=(select routeCode from Riders rr where rr.riderCode=@rc and rr.branchId=@bc)
                    ", new { @bc = branchcode, @rc = ridercode });
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