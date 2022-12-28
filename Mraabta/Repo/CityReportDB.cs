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
    public class CityReportDB
    {
        SqlConnection orcl;
        public CityReportDB()
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
        public async Task<List<CityPoints>> TotalCityStats(int month)
        {
            try
            {
                var rs = await orcl.QueryAsync<(int CityCode, string RS, string CN, int User, int ActiveUser, int StatusId)>(@"with t as(
                                                                                                                                select r.runsheetNumber, r.branchCode, r.ridercode from App_Delivery_RunsheetFetched rf
                                                                                                                                inner join Runsheet r on r.runsheetNumber = rf.runsheetNo
                                                                                                                                where r.branchCode in @bcs
                                                                                                                                and month(r.runsheetDate) = @month
                                                                                                                                and year(r.runsheetDate) = year(getdate()))
                                                                                                                                select 
                                                                                                                                u.branchCode as CityCode,
                                                                                                                                t.runsheetNumber as RS,
                                                                                                                                rc.consignmentNumber as CN,
                                                                                                                                u.USER_ID as [User],
                                                                                                                                isnull(appd.created_by,0) as ActiveUser,
                                                                                                                                isnull(appd.StatusId,0) as StatusId
                                                                                                                                from App_Users u
                                                                                                                                left join t on t.branchCode = u.branchCode --and t.ridercode = u.riderCode
                                                                                                                                left join RunsheetConsignment rc on t.runsheetNumber = rc.runsheetNumber
                                                                                                                                left join App_Delivery_ConsignmentData appd on appd.RunSheetNumber = rc.runsheetNumber and appd.ConsignmentNumber = rc.consignmentNumber
                                                                                                                                where u.branchCode in @bcs;", new { month, @bcs = new List<int> { 34, 43, 1, 4 } }, commandTimeout: int.MaxValue);
                var data = new List<CityPoints>
                {
                    new CityPoints
                    {
                        KarachiTotalDelivered = rs.Where(x => x.CityCode == 4 && x.StatusId == 1).Select(x => x.CN).Distinct().Count(),
                        KarachiTCNDownloaded = rs.Where(x => x.CityCode == 4).Select(x => x.CN).Distinct().Count(),
                        LahoreTotalDelivered = rs.Where(x => x.CityCode == 1 && x.StatusId == 1).Select(x => x.CN).Distinct().Count(),
                        LahoreTCNDownloaded = rs.Where(x => x.CityCode == 1).Select(x => x.CN).Distinct().Count(),
                        RawalpindiTotalDelivered = rs.Where(x => x.CityCode == 34 && x.StatusId == 1).Select(x => x.CN).Distinct().Count(),
                        RawalpindiTCNDownloaded = rs.Where(x => x.CityCode == 34).Select(x => x.CN).Distinct().Count(),
                        IslamabadTotalDelivered = rs.Where(x => x.CityCode == 43 && x.StatusId == 1).Select(x => x.CN).Distinct().Count(),
                        IslamabadTCNDownloaded = rs.Where(x => x.CityCode == 43).Select(x => x.CN).Distinct().Count(),
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

        public async Task<List<CityDataPoints>> TotalCitiesStats(int month)
        {
            try
            {
                var rs = await orcl.QueryAsync<(int CityCode, string RS, string CN, int User, int ActiveUser, int StatusId)>(@"with t as(
                                                                                                                                select r.runsheetNumber, r.branchCode, r.ridercode from App_Delivery_RunsheetFetched rf
                                                                                                                                inner join Runsheet r on r.runsheetNumber = rf.runsheetNo
                                                                                                                                where r.branchCode in @bcs
                                                                                                                                and month(r.runsheetDate) = @month
                                                                                                                                and year(r.runsheetDate) = year(getdate()))
                                                                                                                                select 
                                                                                                                                u.branchCode as CityCode,
                                                                                                                                t.runsheetNumber as RS,
                                                                                                                                rc.consignmentNumber as CN,
                                                                                                                                u.USER_ID as [User],
                                                                                                                                isnull(appd.created_by,0) as ActiveUser,
                                                                                                                                isnull(appd.StatusId,0) as StatusId
                                                                                                                                from App_Users u
                                                                                                                                left join t on t.branchCode = u.branchCode --and t.ridercode = u.riderCode
                                                                                                                                left join RunsheetConsignment rc on t.runsheetNumber = rc.runsheetNumber
                                                                                                                                left join App_Delivery_ConsignmentData appd on appd.RunSheetNumber = rc.runsheetNumber and appd.ConsignmentNumber = rc.consignmentNumber
                                                                                                                                where u.branchCode in @bcs;", new { month, @bcs = new List<int> { 34, 43, 1, 4 } }, commandTimeout: int.MaxValue);
                var data = new List<CityDataPoints>
                {
                    new CityDataPoints
                    {
                        //Karachi
                        KarachiTotalRidersCount = rs.Where(x => x.CityCode == 4).Select(x => x.User).Distinct().Count(),
                        KarachiActiveRiders = rs.Where(x => x.CityCode == 4 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count(),
                        KarachiDownloadedRunsheet = rs.Where(x => x.CityCode == 4).Select(x => x.RS).Distinct().Count(),
                        KarachiTCNDownloaded = rs.Where(x => x.CityCode == 4).Select(x => x.CN).Distinct().Count(),
                        KarachiTotalDeliveredRTS = rs.Where(x => x.CityCode == 4 && x.StatusId == 3).Select(x => x.CN).Distinct().Count(),
                        KarachiTotalUndelivered = rs.Where(x => x.CityCode == 4 && x.StatusId == 2).Select(x => x.CN).Distinct().Count(),
                        KarachiOfflineRiders = (rs.Where(x => x.CityCode == 4).Select(x => x.User).Distinct().Count()) - (rs.Where(x => x.CityCode == 4 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count()),


                        //Lahore
                        LahoreTotalRidersCount = rs.Where(x => x.CityCode == 1).Select(x => x.User).Distinct().Count(),
                        LahoreActiveRiders = rs.Where(x => x.CityCode == 1 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count(),
                        LahoreDownloadedRunsheet = rs.Where(x => x.CityCode == 1).Select(x => x.RS).Distinct().Count(),
                        LahoreTCNDownloaded = rs.Where(x => x.CityCode == 1).Select(x => x.CN).Distinct().Count(),
                        LahoreTotalDeliveredRTS = rs.Where(x => x.CityCode == 1 && x.StatusId == 3).Select(x => x.CN).Distinct().Count(),
                        LahoreTotalUndelivered = rs.Where(x => x.CityCode == 1 && x.StatusId == 2).Select(x => x.CN).Distinct().Count(),
                        LahoreOfflineRiders = (rs.Where(x => x.CityCode == 1).Select(x => x.User).Distinct().Count()) - (rs.Where(x => x.CityCode == 1 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count()),


                        //RAWALPINDI
                        RawalpindiTotalRidersCount = rs.Where(x => x.CityCode == 34).Select(x => x.User).Distinct().Count(),
                        RawalpindiActiveRiders = rs.Where(x => x.CityCode == 34 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count(),
                        RawalpindiDownloadedRunsheet = rs.Where(x => x.CityCode == 34).Select(x => x.RS).Distinct().Count(),
                        RawalpindiTCNDownloaded = rs.Where(x => x.CityCode == 34).Select(x => x.CN).Distinct().Count(),
                        RawalpindiTotalDeliveredRTS = rs.Where(x => x.CityCode == 34 && x.StatusId == 3).Select(x => x.CN).Distinct().Count(),
                        RawalpindiTotalUndelivered = rs.Where(x => x.CityCode == 34 && x.StatusId == 2).Select(x => x.CN).Distinct().Count(),
                        RawalpindiOfflineRiders = (rs.Where(x => x.CityCode == 34).Select(x => x.User).Distinct().Count()) - (rs.Where(x => x.CityCode == 34 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count()),


                        //Islamabad
                        IslamabadTotalRidersCount = rs.Where(x => x.CityCode == 43).Select(x => x.User).Distinct().Count(),
                        IslamabadActiveRiders = rs.Where(x => x.CityCode == 43 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count(),
                        IslamabadDownloadedRunsheet = rs.Where(x => x.CityCode == 43).Select(x => x.RS).Distinct().Count(),
                        IslamabadTCNDownloaded = rs.Where(x => x.CityCode == 43).Select(x => x.CN).Distinct().Count(),
                        IslamabadTotalDeliveredRTS = rs.Where(x => x.CityCode == 43 && x.StatusId == 3).Select(x => x.CN).Distinct().Count(),
                        IslamabadTotalUndelivered = rs.Where(x => x.CityCode == 43 && x.StatusId == 2).Select(x => x.CN).Distinct().Count(),
                        IslamabadOfflineRiders = (rs.Where(x => x.CityCode == 43).Select(x => x.User).Distinct().Count()) - (rs.Where(x => x.CityCode == 43 && x.ActiveUser > 0).Select(x => x.ActiveUser).Distinct().Count()),
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
    }
}