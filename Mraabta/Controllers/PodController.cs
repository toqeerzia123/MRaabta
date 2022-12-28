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
    public class PodController : Controller
    {
        PodRepo repo;
        public PodController()
        {
            repo = new PodRepo();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var u = Session["UserInfo"] as UserModel;
            ViewBag.Branch = u.BranchCode;
            await repo.OpenAsync();
            ViewBag.Reasons = await repo.Reasons();
            ViewBag.Relations = await repo.Relations();
            repo.Close();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetRunsheet(string no)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetRunsheet(no, u.BranchCode, u.Profile);
                if (rs != null)
                {
                    return Json(new { sts = 0, data = rs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = "No Data Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(Pod model)
        {
            try
            {
                model.CNs = model.CNs.Where(x => x.Update && !x.ReadOnly && x.Reason != "0");
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.Save(model, u);
                return Json(new { sts = rs ? 0 : 1, msg = rs ? "POD Saved" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}