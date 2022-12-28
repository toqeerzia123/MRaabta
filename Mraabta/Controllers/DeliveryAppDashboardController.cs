using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class DeliveryAppDashboardController : Controller
    {
        DeliveryAppDashboardRepo repo;

        public DeliveryAppDashboardController()
        {
            repo = new DeliveryAppDashboardRepo();
        }
        public async Task<ActionResult> Index()
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await repo.GetData(u.BranchCode);

            if (!rs.MonthlyStatsModels.Any())
            {
                var monthlystats = new List<MonthlyStatsModel>();
                for (int i = 1; i <= DateTime.Now.Day; i++)
                {
                    monthlystats.Add(new MonthlyStatsModel { Day = i, DeliveredCNs = 0 });
                }
                rs.MonthlyStatsModels = monthlystats;
            }

            var dcn = rs.RunsheetStatsModels.Where(x => !string.IsNullOrEmpty(x.CN)).GroupBy(x => x.RS).Select(x => x.Select(y => y.CN).Distinct().Count()).Sum();
            var performed = rs.RunsheetStatsModels.Where(x => x.StatusId > 0).GroupBy(x => x.RS).Select(x => x.Select(y => y.CN).Distinct().Count()).Sum();
            var delivered = rs.RunsheetStatsModels.Where(x => x.StatusId == 1).GroupBy(x => x.RS).Select(x => x.Select(y => y.CN).Distinct().Count()).Sum();
            var undelivered = rs.RunsheetStatsModels.Where(x => x.StatusId == 2).GroupBy(x => x.RS).Select(x => x.Select(y => y.CN).Distinct().Count()).Sum();
            var deliveredRts = rs.RunsheetStatsModels.Where(x => x.StatusId == 3).GroupBy(x => x.RS).Select(x => x.Select(y => y.CN).Distinct().Count()).Sum();
            var dashboardData = new DashboardViewModel
            {
                DRS = rs.RunsheetStatsModels.Select(x => x.RS).Distinct().Count(),
                DCN = dcn,
                Performed = performed,
                UnPerformed = dcn - performed,
                TouchPoints = rs.RunsheetStatsModels.Where(x => x.StatusId > 0).GroupBy(x => new { RS = x.RS, Lat = x.Lat, Long = x.Long, ReceiverOrReason = new List<int> { 1, 3 }.Contains(x.StatusId) ? x.Receiver : x.Reason }).Count(),
                TotalRiders = rs.RiderCountModel.TotalRiders,
                ActiveRiders = rs.RiderCountModel.ActiveRiders,
                PerformingRiders = rs.RiderCountModel.PerformingRiders,
                MonthlyStats = rs.MonthlyStatsModels,
                Delivered = delivered,
                Undelivered = undelivered,
                DeliveredRTS = deliveredRts,
                DashboardRiderReport = rs.DashboardRiderReport
            };

            return View(dashboardData);
        }

        public async Task<ActionResult> GetRiderReport()
        {
            //var u = Session["UserInfo"] as UserModel;
            //var rs = await repo.GetRiderReport(u.BranchCode);
            var rs = "";
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}