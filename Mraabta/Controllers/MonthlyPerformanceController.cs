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
    public class MonthlyPerformanceController : Controller
    {
        MonthlyPerformanceDB rrd;
        public MonthlyPerformanceController()
        {
            rrd = new MonthlyPerformanceDB();
        }
        // GET: MonthlyPerformance
        public ActionResult Index()
        {
            return View();
        }
        //Get riders according to the month selected
        public async Task<ActionResult> ViewRiders(string Year, string Month)
        {
            try
            {
                string branchcode = Session["BRANCHCODE"].ToString();
                await rrd.OpenAsync();
                var riders = await rrd.GetRiders(Year, Month, branchcode);
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

        //Get Monthly Stats
        [HttpPost]
        public async Task<ActionResult> GetMonthlyStats(String riderCode, string Year, string Month)
        {
            try
            {
                await rrd.OpenAsync();
                List<DataPoint> dataPoints = new List<DataPoint>();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await rrd.TotalMonthlyStats(riderCode, Year, Month, branchcode);

                rrd.Close();

                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                return null;
            }

        }

        //Monthly Progress Count
        [HttpPost]

        public async Task<ActionResult> GetMonthlyCount(string riderCode, string Year, string Month)
        {
            try
            {
                await rrd.OpenAsync();
                List<MonthlyStats> dataPoints = new List<MonthlyStats>();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await rrd.TotalMonthlyCounts(riderCode, Year, Month, branchcode);

                rrd.Close();

                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                return null;
            }
        }

    }
}