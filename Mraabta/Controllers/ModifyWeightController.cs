using MRaabta.App_Start;
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
    public class ModifyWeightController : Controller
    {
        ModifyWeightRepo repo;
        public ModifyWeightController()
        {
            repo = new ModifyWeightRepo();
        }

        // GET: ModifyWeight
        [HttpGet, SkipFilter]
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetWeightDetail(WeightModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;

                var rs = await repo.WeightDetail(model, u);
                if (rs != null)
                {
                    return Json(new { sts = 1, data = rs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 2, msg = "No Data Found !! !!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Details(string CN)
        {
            try
            {
                var weightdata = await repo.GetWeightDetail(CN);
                var isupdated = await repo.GetWeightUpdated(CN);

                ViewBag.WeightData = new { sts = 1, data = weightdata, CN = weightdata.consignmentNumber, Status = isupdated > 0 ? "Close" : "Open" };
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Update(string CN, string Remarks, int IsCorrect)
        {
            var u = Session["UserInfo"] as UserModel;
            var IsSuccess = await repo.AddRemarks(CN, u, Remarks, IsCorrect);

            if (IsSuccess > 0)
            {
                return Json(new { sts = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { sts = 2, msg = "Error !! Request Not Sent" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}