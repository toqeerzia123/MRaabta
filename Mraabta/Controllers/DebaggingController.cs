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
    public class DebaggingController : Controller
    {
        DebaggingRepo repo;
        public DebaggingController()
        {
            repo = new DebaggingRepo();
        }

        #region Debagging
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await repo.OpenAsync();
            var cnlengths = await repo.GetConsignmentsLength();
            repo.Close();
            ViewBag.CNLengths = cnlengths.Select(x => new { x.Product, x.Prefix, x.PrefixLength, x.Length }).ToList();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetBagInfo(string bagno)
        {
            try
            {
                if (!await repo.IsDebagged(bagno))
                {
                    var rs = await repo.GetBagInfo(bagno);
                    return Json(new { sts = 0, data = rs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = "Bag already debagged" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetExcessManCN(string no, bool isMan)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetExcessManCN(no, isMan, int.Parse(u.BranchCode));
                if (rs.data != null)
                {
                    return Json(new { sts = 0, data = rs.data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = rs.msg }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(BagInfoModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.Save(model, u);
                return Json(new { sts = rs.sts ? 0 : 1, msg = rs.response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Search
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetDebags(DateTime date)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetDebags(date.ToString("yyyy-MM-dd"), u.BranchCode);
                var data = rs.Select(x => new
                {
                    Id = x.Id,
                    BagNo = x.BagNo,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    Date = x.Date,
                    TotalManifests = x.TotalManifests,
                    TotalCNs = x.TotalCNs
                }).ToList();

                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Search

        [SkipFilter]
        public async Task<ActionResult> Print(string id)
        {
            ViewBag.Name = (Session["UserInfo"] as UserModel).Name;
            var rs = await repo.PrintData(id);
            return View(rs);
        }
        #endregion
    }
}