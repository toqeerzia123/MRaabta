using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MRaabta.Models;
using MRaabta.Repo;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MRaabta.Controllers
{
    public class PickupReportController : Controller
    {
        PickupReportDB prd = new PickupReportDB();
        // GET: PickupReport

        public ActionResult ViewPickup()
        {

            return View();
        }
        [HttpPost]
        public ActionResult getRecordsByRider(string Reason, string SDate, string EDate)
        {
            List<PickupReportModel> result = new List<PickupReportModel>();
            result = prd.GetPickups(Reason, SDate, EDate);
            return Json(new { result = result });

        }
        [HttpPost]
        public ActionResult getRecordByTicket(string ticketNumber)
        {
            List<PickupTicketModel> result = new List<PickupTicketModel>();
            result = prd.GetRecord(ticketNumber);
            return Json(new { result = result });

        }
        [HttpPost]
        public ActionResult getStats(string Reason, string SDate, string EDate)
        {
            List<PickupStats> result = new List<PickupStats>();
            result = prd.GetStats(Reason, SDate, EDate);
            return Json(new { result = result });

        }
        [HttpPost]
        public ActionResult getCities()
        {
            List<PickupReportModel> result = new List<PickupReportModel>();
            result = prd.getcities();
            return Json(new { result = result });
        }

        [HttpPost]
        public async Task<ActionResult> GetTodayConsignments()
        {
            try
            {
                await prd.OpenAsync();
                List<DataPoint> dataPoints = new List<DataPoint>();
                //String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await prd.TotalConsignments();

                prd.Close();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                return null;
            }

        }

        [HttpPost]
        public async Task<ActionResult> GetPoints()
        {
            try
            {
                await prd.OpenAsync();

                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await prd.TotalReasonPoints(branchcode);

                prd.Close();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                return null;
            }

        }
    }
}