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
    public class DebrieferReportDB
    {
        SqlConnection orcl;
        public DebrieferReportDB()
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

        public async Task<List<DropDownModel>> GetRiders(string StartDate, string EndDate, string branchcode)
        {
            try
            {
                string sql = @"SELECT
                    DISTINCT Appdt.RIDERCODE as Value  ,
                    (Select Top(1) userName from App_Users where ridercode=Appdt.riderCode AND branchCode = @bc) as Text
                    FROM   App_Delivery_ConsignmentData Appdt
                    inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                    WHERE R.BRANCHCODE = @bc AND cast(R.RUNSHEETDATE as date) between @StartDate and @EndDate";
                var rs = await orcl.QueryAsync<DropDownModel>(sql, new { @StartDate = StartDate, @EndDate = EndDate, @bc = branchcode });
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

        public async Task<List<DebrieferCount>> TotalDebreiferCount(string ridercode, string StartDate, string EndDate, string branchcode)
        {
            try
            {
                var query = @"select 
                            rc.consignmentNumber as CN,
                            ISNULL(appd.StatusId,0) as StatusId,
                            ISNULL(appd.verify,0) as Verified,
                            ISNULL(appd.comments,'') as Comments,
                            ISNULL(appd.reason,'') as Reasons,
                            appd.performed_on as [Date]
                            from App_Delivery_RunsheetFetched rf
                            INNER JOIN Runsheet r on r.runsheetNumber = rf.runsheetNo
                            INNER JOIN RunsheetConsignment rc on rc.runsheetNumber = r.runsheetNumber
                            LEFT JOIN App_Delivery_ConsignmentData appd on appd.RunSheetNumber = rc.runsheetNumber and appd.ConsignmentNumber = rc.consignmentNumber
                            WHERE r.runsheetDate between @StartDate and @EndDate and r.branchCode = @branchcode and r.ridercode = @ridercode;";

                var rs = await orcl.QueryAsync<(string CN, int StatusId, bool Verified, string Comments, string Reasons, DateTime Date)>(query, new { ridercode, StartDate, EndDate, branchcode }, commandTimeout: int.MaxValue);

                return new List<DebrieferCount> {
                        new DebrieferCount {
                        Attempted = rs.Where(x => x.StatusId > 0).Select(x => x.CN).Distinct().Count(),
                        UnAttempted = rs.Where(x => x.StatusId == 0).Select(x => x.CN).Distinct().Count(),
                        TotalDelivered = rs.Where(x => x.StatusId == 1).Select(x => x.CN).Distinct().Count(),
                        TotalDeliveredRts = rs.Where(x => x.StatusId == 3).Select(x => x.CN).Distinct().Count(),
                        TotalUndelivered = rs.Where(x => x.StatusId == 2).Select(x => x.CN).Distinct().Count(),
                        VerifiedCount = rs.Where(x => x.StatusId == 2 && x.Verified).Select(x => x.CN).Distinct().Count(),
                        CommentsCount = rs.Where(x => x.StatusId == 2 && x.Verified && !string.IsNullOrEmpty(x.Comments)).Select(x => x.CN).Distinct().Count(),
                        MostOccuringReason = rs.OrderByDescending(x => x.Date).FirstOrDefault().Reasons,
                        TCNDownloaded = rs.Select(x => x.CN).Distinct().Count(),
                    }
                };
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