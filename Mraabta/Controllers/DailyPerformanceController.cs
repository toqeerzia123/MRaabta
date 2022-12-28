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
    public class DailyPerformanceController : Controller
    {
        DailyPerformanceDB rrd;
        public DailyPerformanceController()
        {
            rrd = new DailyPerformanceDB();
        }
        // GET: DailyPerformance
        public ActionResult Index()
        {
            return View();
        }
        //Get riders according to the Daily selected
        public async Task<ActionResult> ViewRiders(string Date)
        {
            try
            {
                string branchcode = Session["BRANCHCODE"].ToString();
                await rrd.OpenAsync();
                var riders = await rrd.GetRiders(Date, branchcode);
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

        //Monthly Progress Count
        [HttpPost]

        public async Task<ActionResult> GetMonthlyCount(string riderCode, string Date)
        {
            try
            {
                await rrd.OpenAsync();
                List<MonthlyStats> dataPoints = new List<MonthlyStats>();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await rrd.TotalMonthlyCounts(riderCode, Date, branchcode);

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