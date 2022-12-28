using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class RiderReportController : Controller
    {
        RiderReportDB rrd;
        public RiderReportController()
        {
            rrd = new RiderReportDB();
        }
        RiderReportDB rrdd = new RiderReportDB();
        LocationDB ldb = new LocationDB();
        RiderDB rdb = new RiderDB();

        // GET: RiderReport
        public ActionResult Index()
        {
            if (Session["BRANCHCODE"] != null)
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                return View();
            }
            else
            {
                return Redirect("~/Login");
            }
        }

        public async Task<ActionResult> ViewRiders(string SDate, string EDate)
        {
            try
            {
                string branchcode = Session["BRANCHCODE"].ToString();
                await rrd.OpenAsync();
                var riders = await rrd.GetRiders(branchcode);
                rrd.Close();
                //var rs = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                return Json(riders, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                ViewBag.err = "error";
                //rs = null;
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> getRecordsByRider(List<string> riderCode, string SDate, string EDate)
        {
            try
            {
                riderCode = riderCode.Where(x => x != "0").ToList();
                var branchcode = Session["BRANCHCODE"].ToString();
                List<RiderReportModel> lp = new List<RiderReportModel>();
                if (SDate != "" && EDate != "")
                {
                    var rs = await rrd.GetRiderData(branchcode, riderCode, SDate, EDate);

                    foreach (var item in rs.GroupBy(x => x.RiderCode))
                    {
                        var totaltimetaken = TimeSpan.Parse("00:00:00");

                        foreach (var z in item.Where(x => x.PerformedOn.HasValue).GroupBy(x => x.PerformedOn.Value.Date))
                        {
                            var t1 = z.Min(x => x.PerformedOn).Value.TimeOfDay;
                            var t2 = z.Max(x => x.PerformedOn).Value.TimeOfDay;
                            totaltimetaken += t2 - t1;
                        }

                        lp.Add(new RiderReportModel
                        {
                            DownloadedRunsheet = item.Where(x => !string.IsNullOrEmpty(x.DR)).Select(x => x.DR).Distinct().Count(),
                            TotalRunsheet = item.Select(x => x.RS).Distinct().Count(),
                            riderCode = item.Key,
                            RiderName = item.FirstOrDefault().RiderName,
                            Location = item.FirstOrDefault().Branch,
                            RiderRoute = item.FirstOrDefault().Route,
                            //TotalCN = item.Select(x => x.CN).Distinct().Count(),
                            //TCNDownloaded = item.Where(x => !string.IsNullOrEmpty(x.DR)).Select(x => x.CN).Distinct().Count(),
                            TotalCN = item.GroupBy(x => x.RS).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum(),
                            TCNDownloaded = item.Where(x => !string.IsNullOrEmpty(x.DR)).GroupBy(x => x.DR).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum(),
                            delivered = item.Where(x => x.StatusId == 1).Select(x => x.CN).Distinct().Count(),
                            undelivered = item.Where(x => x.StatusId == 2).Select(x => x.CN).Distinct().Count(),
                            deliveredRts = item.Where(x => x.StatusId == 3).Select(x => x.CN).Distinct().Count(),
                            Touchpoints = item.Where(x => x.Lat.HasValue && x.Long.HasValue).GroupBy(x => new { x.RS, x.Lat, x.Long, RR = new List<int> { 1, 3 }.Contains(x.StatusId) ? x.Receiver : x.Reason }).Count(),
                            TotalTimeTaken = totaltimetaken.ToString(@"hh\:mm\:ss"),
                        });
                    }
                }
                else
                {
                    ViewBag.Error = "Kindly pass the parameters required";
                }
                ViewBag.StartDate = SDate;
                ViewBag.EndDate = EDate;
                return PartialView("getRecordsByRider", lp);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return PartialView("getRecordsByRider", null);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetReasonPoints(List<string> riderCode, string SDate, string EDate)
        {
            try
            {
                riderCode = riderCode.Where(x => x != "0").ToList();
                String branchcode = Session["BRANCHCODE"].ToString();

                var data = await rrd.GetRiderData(branchcode, riderCode, SDate, EDate);

                var rs = new List<Status> {
                    new Status
                    {
                        TotalRunsheet =  data.Select(x => x.RS).Distinct().Count(),
                        DownloadedRunsheet = data.Where(x => !string.IsNullOrEmpty(x.DR)).Select(x => x.DR).Distinct().Count(),
                        TotalCN = data.GroupBy(x => x.RS).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum(),
                        TCNDownloaded = data.Where(x => !string.IsNullOrEmpty(x.DR)).GroupBy(x => x.DR).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum(),
                        Touchpoints = data.Where(x => x.Lat.HasValue && x.Long.HasValue).GroupBy(x => new { x.RS, x.Lat, x.Long, RR = new List<int> { 1, 3 }.Contains(x.StatusId) ? x.Receiver : x.Reason }).Count(),
                        delivered = data.Where(x => x.StatusId == 1).GroupBy(x => x.DR).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum(),
                        undelivered = data.Where(x => x.StatusId == 2).GroupBy(x => x.DR).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum(),
                        deliveredRts = data.Where(x => x.StatusId == 3).GroupBy(x => x.DR).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum()
                    }
                };

                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                ViewBag.err = "error";
                return View();
            }
        }
    }
}