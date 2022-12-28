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
    public class ScheduleController : Controller
    {
        ScheduleRepo repo;
        public ScheduleController()
        {
            repo = new ScheduleRepo();
        }
        // GET: Schdeule
        public async Task<ActionResult> Index()
        {
            var abc = Session["U_ID"].ToString();
            await repo.OpenAsync();
            var couriers = await repo.GetCouriers();
            repo.Close();
            ViewBag.Couriers = couriers.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> updateSchedule(CourierTransferModel model)
        {
            model.modifiedBy = int.Parse(Session["U_ID"].ToString());
            model.modifiedOn = DateTime.Now;

            var rs = false;
            await repo.OpenAsync();
            if (model.RB_TYPE == 1)
            {
                rs = await repo.UpdateSchdeuleRequest(model);
            }
            else if (model.RB_TYPE == 2)
            {
                CourierTransferDateModel ctdm = new CourierTransferDateModel();
                ctdm.oldCourierID = model.oldCourierID;
                ctdm.newCourierID = model.newCourierID;
                ctdm.scheduleDate = model.Date;
                ctdm.STATUS = 1;
                ctdm.createdBy = int.Parse(Session["U_ID"].ToString());
                List<int> scheduleIds = new List<int>();
                var ScheduleIDs = await repo.getScheduleIDs(model.oldCourierID);
                for (int i = 0; i < ScheduleIDs.Count; i++)
                {
                    scheduleIds.Add(int.Parse(ScheduleIDs[i].ToString()));
                }
                ctdm.scheduleId = scheduleIds;
                rs = await repo.UpdateCourierSchedule(ctdm);

            }

            repo.Close();
            return Json(new { sts = rs ? 1 : 0, msg = rs ? "Courier Transferred Successfully" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
        }
    }
}