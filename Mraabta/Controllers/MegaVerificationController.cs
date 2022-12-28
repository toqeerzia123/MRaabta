using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class MegaVerificationController : Controller
    {
        MegaVerificationRepo repo;
        public MegaVerificationController()
        {
            repo = new MegaVerificationRepo();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var rs = await repo.GetCN();
            return View(rs);
        }

        [HttpPost]
        public async Task<ActionResult> Save(CNModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                model.CreatedBy = u.Uid;
                var rs = await repo.Save(model);
                return Json(new { sts = rs ? 0 : 1, msg = rs ? "Saved Successfully" : "Something went wrong", data = rs ? await repo.GetCN() : null }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}