using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MRaabta.Models;

namespace MRaabta.Controllers
{
    public class ECBookingStaffIncentiveController : Controller
    {
        ECBookingStaffIncentiveRepo repo = new ECBookingStaffIncentiveRepo();
        public ActionResult Index()
        {
            if (HttpContext.Session == null)
            {
                return Redirect("/");
            }
            return View();
        }
        public async Task<ActionResult> GetReport(string Month, string Year)
        {
            ViewBag.Month = Month;
            ViewBag.Year = Year;
            var model = await repo.GetReport(Month, Year);
            return PartialView("_GetReport",model);
        }
        public async Task<ActionResult> GetDetailReport(ECBookingStaffIncentiveModel model)
        {
            var data = await repo.GetDetailReport(model);
            return View(data);
        }
    }
}