using Dapper;
using Microsoft.Ajax.Utilities;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo
{
    public class DeliveryAppDashboardRepo
    {
        SqlConnection con;
        public DeliveryAppDashboardRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<DashboardModel> GetData(string branchId)
        {
            try
            {
                var now = DateTime.Now;
                await con.OpenAsync();
                var query = $@"select 
                                rf.runsheetNo as RS,
                                rc.consignmentNumber as CN,
                                ISNULL(dd.StatusId,0) as StatusId,
                                ISNULL(dd.latitude,0) as Lat,
                                ISNULL(dd.longitude,0) as Long,
                                dd.picker_name as Receiver,
                                dd.reason as Reason
                                from Runsheet r
                                inner join App_Delivery_RunsheetFetched rf on r.runsheetNumber = rf.runsheetNo
                                left join RunsheetConsignment rc on r.runsheetNumber = rc.runsheetNumber
                                left join App_Delivery_ConsignmentData dd on dd.RunSheetNumber = rc.runsheetNumber and dd.ConsignmentNumber = rc.consignmentNumber
                                where cast(r.runsheetDate as date) = '{now.ToString("yyyy-MM-dd")}' and r.branchCode = '{branchId}';";
                var runsheetStatsData = await con.QueryAsync<RunsheetStatsModel>(query, commandTimeout: int.MaxValue);
                query = $@"with t as(
                        select USER_ID, isActive from App_Users where branchCode = '{branchId}' and STATUS =  1
                        )
                        select
                        (select count(distinct t.USER_ID) as UserId from t) as TotalRiders,
                        (select count(distinct t.USER_ID) as UserId from t where t.isActive = 1) as ActiveRiders,
                        (select count(distinct r.ridercode) from Runsheet r
                        inner join App_Delivery_ConsignmentData d on r.runsheetNumber = d.RunSheetNumber and r.ridercode = d.riderCode
                        where r.branchCode = '{branchId}' and r.runsheetDate = '{now.ToString("yyyy-MM-dd")}' and d.StatusId > 0) as PerformingRiders;";
                var riderCountData = await con.QueryFirstOrDefaultAsync<RiderCountModel>(query, commandTimeout: int.MaxValue);
                query = $@"with t as (
                                select 
                                day(r.runsheetDate) as Day,
                                count(distinct dd.ConsignmentNumber) as DeliveredCNs
                                from Runsheet r
                                inner join App_Delivery_ConsignmentData dd on dd.RunSheetNumber = r.runsheetNumber
                                where month(r.runsheetDate) = {now.Month} and year(r.runsheetDate) = {now.Year} and r.branchCode = '{branchId}' and dd.StatusId = 1
                                group by cast(r.runsheetDate as date), r.runsheetNumber)
                                select t.Day, SUM(t.DeliveredCNs) as DeliveredCNs from t
                                group by t.Day;";
                var monthlyStatsData = await con.QueryAsync<MonthlyStatsModel>(query, commandTimeout: int.MaxValue);

                query = $@"select 
                            top 10
                            d.StatusId as StatusId,
                            d.ConsignmentNumber as CN,
                            u.userName as RiderName,
                            d.performed_on as PerformedOn
                            from App_Delivery_ConsignmentData d
                            inner join App_Users u on u.USER_ID = d.created_by
                            where cast(d.performed_on as date) = '{DateTime.Now.ToString("yyyy-MM-dd")}'
                            and u.branchCode = {branchId}
                            order by performed_on desc;";
                var riderReport = await con.QueryAsync<DashboardRiderReport>(query);

                con.Close();

                return new DashboardModel
                {
                    RunsheetStatsModels = runsheetStatsData.ToList(),
                    RiderCountModel = riderCountData,
                    MonthlyStatsModels = monthlyStatsData.OrderBy(x => x.Day).ToList(),
                    DashboardRiderReport = riderReport.ToList()
                };
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return null;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return null;
            }
        }

        public async Task<List<DashboardRiderReport>> GetRiderReport(string branchId)
        {
            try
            {
                var query = $@"select 
                            top 10
                            d.StatusId as StatusId,
                            d.ConsignmentNumber as CN,
                            u.userName as RiderName,
                            d.performed_on as PerformedOn
                            from App_Delivery_ConsignmentData d
                            inner join App_Users u on u.USER_ID = d.created_by
                            where cast(d.performed_on as date) = '{DateTime.Now.ToString("yyyy-MM-dd")}'
                            and u.branchCode = {branchId}
                            order by performed_on desc;";
                await con.OpenAsync();
                var rs = await con.QueryAsync<DashboardRiderReport>(query);
                rs.ForEach(x =>
                {
                    x.PerformedOnStr = x.PerformedOn.ToString("HH:mm");
                });
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return new List<DashboardRiderReport>();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return new List<DashboardRiderReport>();
            }
        }
    }
}