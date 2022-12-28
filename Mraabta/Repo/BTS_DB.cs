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
    public class BTS_DB
    {
        SqlConnection orcl;
        public BTS_DB()
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

        public async Task<List<PointsDashboard>> TotalReasonPoints(string branchcode)
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                var rs = await orcl.QueryAsync<PointsDashboard>(@"
                   select
                    (select count(USER_ID) from App_Users where status = 1 and branchCode=@bc) as RidersCount,
                   (select count(x.ridercode) from (select distinct au.riderCode from App_Users au left join App_Delivery_ConsignmentData adpd on adpd.riderCode = au.riderCode where au.branchCode=@bc and cast(created_on as date) = CAST(GETDATE() as date)) as x) as ActiveRiders,
                    (select count(au.riderCode) from App_Users au where au.branchCode=@bc and au.status = '1' and au.riderCode not in(select adpd.riderCode from App_Delivery_ConsignmentData adpd where cast(adpd.created_on as date) = CAST(GETDATE() as date)))as OfflineRiders, 
                    (select COUNT(consignmentNumber) from RunsheetConsignment rc 
                    INNER JOIN Runsheet r on rc.runsheetNumber = r.runsheetNumber and rc.branchcode = r.branchCode AND rc.RouteCode = r.routeCode
                    inner join (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = R.RUNSHEETNUMBER
                    WHERE cast(r.runsheetDate as date) = cast(GETDATE() AS DATE) AND RC.branchcode=@bc) as TCNDownloaded,
                    SUM(case when isnull(appdt.StatusId,0) > 0 then 1 else 0 end) [Touchpoints],
                    (select count(runsheetNumber) from Runsheet r  
                    inner join 
                    (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber
                    where cast(r.runsheetDate as date) = cast(GETDATE() AS DATE) and branchCode=@bc) 
                    as DownloadedRunsheet,
                    SUM(case when isnull(appdt.StatusId,0) = 1 then 1 else 0 end) delivered,
                    SUM(case when isnull(appdt.StatusId,0) = 3 then 1 else 0 end) deliveredRts,
                    SUM(case isnull(appdt.StatusId,0) when 2 then 1 else 0 end) undelivered
                    --ISNULL(SUM(case when appdt.ConsignmentNumber is not null and(appdt.relation is null or appdt.relation = '') then 0 else 1 end),0) delivered,
                    --ISNULL(SUM(case when appdt.ConsignmentNumber is not null and(appdt.reason is null or appdt.reason = '') then 0 else 1 end),0) undelivered
                    from App_Delivery_ConsignmentData appdt
                    inner join runsheet r on r.runsheetNumber = appdt.RunSheetNumber
                    where cast(r.runsheetDate as date) = cast(GETDATE() AS DATE) and branchCode=@bc", new { @bc = branchcode });

                var tp = await orcl.QueryFirstOrDefaultAsync<int>(@"select count(*) as TP from(
                                                                    select 
                                                                    appd.latitude as Lat,
                                                                    appd.longitude as Long
                                                                    from App_Delivery_ConsignmentData appd
                                                                    inner join Runsheet r on r.runsheetNumber = appd.RunSheetNumber and r.ridercode = appd.riderCode
                                                                    where cast(r.runsheetDate as date) = cast(GETDATE() as date)
                                                                    and r.branchCode = @bc 
                                                                    and appd.latitude != 0 and appd.longitude != 0
                                                                    group by appd.latitude,appd.longitude) as xb;", new { @bc = branchcode });

                rs.ForEach(x => x.Touchpoints = tp);

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

        public async Task<List<DataPoint>> TotalMonthlyStats(string branchcode)
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                //var rs = await orcl.QueryAsync<MonthlyStats>(@"
                //  select * from (select runsheetDate, branchCode, 
                //    sum(A) as TCNDownloaded, sum(B) as delivered, sum(C) as undelivered from (
                //    select r.runsheetDate, r.branchCode, 1 as A, case when isnull(relation, '') <> '' then 1 else 0 end as B,
                //    case when isnull(reason, '') <> '' then 1 else 0 end as C
                //    from App_Delivery_RunsheetFetched rf
                //    left join App_Delivery_ConsignmentData c on c.RunSheetNumber = rf.runsheetNo
                //    inner join Runsheet r on r.runsheetNumber = rf.runsheetNo) as xb
                //    where branchCode=@bc
                //    group by runsheetDate, branchCode
                //    ) as x
                //    where format(x.runsheetDate,'MMMM') = FORMAT(GETDATE(), 'MMMM')", new { @bc = branchcode });
                var rs = await orcl.QueryAsync<DataPoint>(@"
                   select  cast(xy.runsheetDate as varchar) as X, Y  from (select runsheetDate, branchCode,
                    sum(B) as Y from (
                    select r.runsheetDate, r.branchCode, case when isnull(rf.StatusId, 0) in(1,3) then 1 else 0 end as B
                    from App_Delivery_ConsignmentData rf
                    inner join Runsheet r on r.runsheetNumber = rf.RunSheetNumber) as xb
                    where branchCode=@bc
                    group by runsheetDate, branchCode
                    ) as xy
                    where format(xy.runsheetDate,'MMMM') = FORMAT(GETDATE(), 'MMMM')
                    order by xy.runsheetDate", new { @bc = branchcode });
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

        public async Task<List<LiveRecords>> TotalConsignments(string branchcode)
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                var rs = await orcl.QueryAsync<LiveRecords>(@"select top 10
                                                            s.Id as [StatusId],
                                                            s.Name as [Status],
                                                            ConsignmentNumber, 
                                                            apu.userName,
                                                            performed_on as PerformedOn
                                                            from App_Delivery_ConsignmentData appdt
                                                            inner join runsheet r on r.runsheetNumber = appdt.RunSheetNumber
                                                            inner join App_Users apu on appdt.riderCode = apu.riderCode
                                                            inner join App_Delivery_Status s on s.Id = appdt.StatusId
                                                            where r.branchCode= @bc
                                                            and cast(r.runsheetDate as date) = CAST(GETDATE() AS DATE)
                                                            order by performed_on desc;", new { @bc = branchcode });
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
        public async Task<List<LiveRecords>> TotalConsignments()
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                var rs = await orcl.QueryAsync<LiveRecords>(@"
select top 10 xb.ticketNumber, xb.consigner, concat(xb.PickupDate,' ',xb.PickupTime) as scheduledtime, xb.chargedAmount, xb.Status,xb.callStatus  from (
select  c.ticketNumber,c.chargedAmount,c.consigner,cast(c.pickupScheduled as date) as PickupDate, c.createdOn,
CONVERT(VARCHAR(5),c.pickupTime,108) as PickupTime ,
case when ((c.callstatus is null or c.callstatus = 1 ) and c.consignmentNumber is null) then 'Pending'
when ((c.callStatus = 2 or c.callStatus = 3) and c.consignmentNumber is null ) then 'InProcess'
when ( c.callStatus = 4 and c.consignmentNumber is not null) then 'Performed'
when (c.callStatus = 5 and c.consignmentNumber is null) then 'Cancelled' end as Status, c.callStatus
from MNP_PreBookingConsignment c 
where c.isMobile = '1'   and cast(createdOn as date) = cast(GETDATE() as date)
) as xb
order by xb.createdOn desc"
                    );
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