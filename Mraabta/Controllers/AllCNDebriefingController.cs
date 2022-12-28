using MRaabta.Models;
using MRaabta.Repo;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class AllCNDebriefingController : Controller
    {
        DebreifingAllRepo repo;
        public AllCNDebriefingController()
        {
            repo = new DebreifingAllRepo();
        }
        public async Task<ActionResult> Index()
        {
            string branchcode = Session["BRANCHCODE"].ToString();
            ViewBag.Reasons = await repo.GetReasons();
            var rs = await repo.GetData(branchcode);
            return View(rs);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitVerification(int Id, bool IsVerify, string Comment)
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await repo.UpdateVerification(Id, IsVerify, Comment, u.Uid);
            return Json(new { sts = rs ? 1 : 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCN(int Id, string Reason)
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await repo.UpdateReason(Id, Reason, u.Uid);
            return Json(new { sts = rs ? 1 : 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}