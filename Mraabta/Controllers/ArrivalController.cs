using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class ArrivalController : Controller
    {
        ArrivalRepo repo;
        public ArrivalController()
        {
            repo = new ArrivalRepo();
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                await repo.OpenAsync();
                var u = Session["UserInfo"] as UserModel;
                var riders = await repo.GetRiders(u.BranchCode);
                var cntypes = await repo.GetConsignmentTypes();
                var servicetypes = await repo.GetServiceTypes();
                var cnLengths = await repo.GetConsignmentsLength();
                repo.Close();

                ViewBag.Riders = riders.Select(x => new SelectListItem { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
                ViewBag.CNTypes = cntypes.Select(x => new SelectListItem { Text = x.Text, Value = x.Value, Selected = x.Value == "12" }).ToList();
                ViewBag.ServiceTypes = servicetypes.Select(x => new SelectListItem { Text = x.Text, Value = x.Text, Selected = x.Text.Equals("overnight") }).ToList();
                ViewBag.CNLengths = cnLengths.Select(x => new { Product = x.Product, Prefix = x.Prefix, Length = x.Length, PrefixLength = x.PrefixLength }).ToList();

            }
            catch (Exception ex)
            {
                repo.Close();
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAndCheckCN(string cn)
        {
            try
            {
                await repo.OpenAsync();
                var dvrtscheck = await repo.DVRTSPrimaryCheck(cn);
                if (string.IsNullOrEmpty(dvrtscheck))
                {
                    var rs = await repo.GetCN(cn);
                    repo.Close();
                    if (cn[0] == '5' && cn.Length == 15)
                    {
                        if (string.IsNullOrEmpty(rs.CN))
                        {
                            return Json(new { type = 1, msg = "Booking required for COD Consignment" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var u = Session["UserInfo"] as UserModel;
                            if (u.BranchCode != rs.Origin)
                            {
                                return Json(new { type = 1, msg = "COD Consignment can only arrive at origin" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    return Json(new { type = 0, rs.CN, rs.Pcs, rs.Weight, rs.CodAmount }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    repo.Close();
                    return Json(new { type = 1, msg = dvrtscheck }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                repo.Close();
                return Json(new { type = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertArrival(ArrivalModel model)
        {
            var u = Session["UserInfo"] as UserModel;
            model.BranchCode = u.BranchCode;
            model.CreatedBy = u.Uid;
            model.CreatedOn = DateTime.Now;
            model.ExpressCenterCode = u.ExpressCenter;
            model.OriginExpressCenterCode = u.ExpressCenter;
            model.Weight = model.ArrivalDetails.Sum(x => double.Parse(x.Weight)).ToString();
            model.LocationName = u.LocationName;
            var rs = await repo.InsertArrival(model);
            return Json(new { id = rs.isOk ? rs.msg : "", sts = rs.isOk ? 1 : 0, msg = rs.isOk ? $"Arrival Saved Id is {rs.msg}" : rs.msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ArrivalSlip(int id = 0)
        {
            var rs = await repo.GetArrivalById(id);
            return View(rs);
        }

        [HttpGet]
        public ActionResult ViewArrival()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> _GetArrivalByRider(string rc, string dt)
        {
            var data = new List<ViewArrivalModel>();
            var rs = await repo.GetArrivalByRider(rc, dt);

            foreach (var x in rs.GroupBy(x => x.Id))
            {
                data.Add(new ViewArrivalModel
                {
                    Id = x.Key,
                    Branch = x.FirstOrDefault()?.Branch,
                    ExpressCenter = x.FirstOrDefault()?.ExpressCenter,
                    Date = x.FirstOrDefault().Date,
                    RiderCode = x.FirstOrDefault().RiderCode,
                    Count = x.Count(),
                    Weight = x.Sum(y => y.Weight)
                });
            }

            return PartialView("_GetArrivalByRider", data);
        }
    }
}