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
    public class CityReportController : Controller
    {
        CityReportDB rrd;
        public CityReportController()
        {
            rrd = new CityReportDB();
        }
        // GET: CityReport
        public ActionResult Index()
        {
            return View();
        }

        //Get Delivered Stats
        [HttpPost]
        public async Task<ActionResult> GetCitiesCount(int Category)
        {
            try
            {
                await rrd.OpenAsync();
                List<CityPoints> dataPoints = new List<CityPoints>();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await rrd.TotalCityStats(Category);

                rrd.Close();

                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                return null;
            }

        }

        //Total Stats

        [HttpPost]
        public async Task<ActionResult> GetCityStats(int Category)
        {
            try
            {
                await rrd.OpenAsync();
                List<CityDataPoints> dataPoints = new List<CityDataPoints>();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await rrd.TotalCitiesStats(Category);

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