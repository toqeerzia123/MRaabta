using System.Collections.Generic;
using System.Web.Mvc;
using MRaabta.Models;
using MRaabta.Repo;

namespace MRaabta.Controllers
{
    public class TotalRevenueController : Controller
    {
        PickupReportDB prd = new PickupReportDB();
        // GET: TotalRevenue
        public ActionResult ViewRevenue()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getAmount(string SDate, string EDate)
        {
            List<PickupReportModel> result = new List<PickupReportModel>();
            result = prd.GetAmount(SDate, EDate);
            return Json(new { result = result });

        }
        [HttpPost]
        public ActionResult getstats(string SDate, string EDate)
        {
            List<PickupTicketModel> result = new List<PickupTicketModel>();
            result = prd.GetAmountStats(SDate, EDate);
            return Json(new { result = result });

        }
        [HttpPost]
        public ActionResult getconsignmentamount(string ticketNumber)
        {
            List<PickupTicketModel> result = new List<PickupTicketModel>();
            result = prd.GetAmountRecord(ticketNumber);
            return Json(new { result = result });

        }
    }
}