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
    public class DemanifestController : Controller
    {
        DemanifestRepo repo;
        public DemanifestController()
        {
            repo = new DemanifestRepo();
        }
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
        public async Task<ActionResult> ManifestData(string manifestNo)
        {
            try
            {
                var x = await repo.ManifestData(manifestNo);
                return Json(new { sts = 0, data = x }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCN(string cn)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var x = await repo.GetCN(cn, int.Parse(u.BranchCode), "Deman");
                if (x != null)
                {
                    if (x.IsValid == 1)
                    {
                        var data = new
                        {
                            CN = x.CN,
                            Status = 7,
                            Origin = x.Origin,
                            Destination = x.Destination,
                            ServiceType = x.ServiceType,
                            CNType = x.ConsignmentType,
                            Pcs = x.Pcs,
                            Weight = x.Weight,
                            Remarks = "",
                        };
                        return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { sts = 1, msg = x.Msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { sts = 1, msg = "No data found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(ManifestDataModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.Save(model, u);

                return Json(new { sts = rs.isok ? 0 : 1, msg = rs.msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetDemanifests(DateTime date)
        {
            try
            {
                var rs = await repo.GetDemanifests(date.ToString("yyyy-MM-dd"));
                var data = rs.Select(x => new
                {
                    ManifestNo = x.ManifestNo,
                    Date = x.Date,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    TotalCNs = x.TotalCNs
                }).ToList();
                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [SkipFilter]
        public async Task<ActionResult> Print(string id)
        {
            var x = await repo.ManifestData(id);
            return View(x);
        }
    }
}