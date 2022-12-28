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
    public class DebrieferReportController : Controller
    {
        DebrieferReportDB rrd;
        public DebrieferReportController()
        {
            rrd = new DebrieferReportDB();
        }
        // GET: DebrieferReport
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ViewRiders(string StartDate, string EndDate)
        {
            try
            {
                string branchcode = Session["BRANCHCODE"].ToString();
                await rrd.OpenAsync();
                var riders = await rrd.GetRiders(StartDate,EndDate ,branchcode);
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


        public async Task<ActionResult> GetDebrieferCount(string riderCode, string StartDate, string EndDate)
        {
            try
            {
                await rrd.OpenAsync();
                List<DebrieferCount> dataPoints = new List<DebrieferCount>();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await rrd.TotalDebreiferCount(riderCode, StartDate, EndDate, branchcode);

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