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
    public class ProjectRunsheetController : Controller
    {
        #region Properties
        ProjectRunsheetRepo repo;
        #endregion

        #region Constructors
        public ProjectRunsheetController()
        {
            repo = new ProjectRunsheetRepo();
        }
        #endregion

        #region ProjectRunsheetPage
        public async Task<ActionResult> Index()
        {
            var u = Session["UserInfo"] as UserModel;
            var routes = await repo.GetRoutes(u.BranchCode);
            ViewBag.Routes = routes.Select(x => new { Value = x.Value, Text = x.Text }).ToList();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Get(string rc)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetRidersAnCNCounts(u.BranchCode, rc);
                return Json(new { sts = 1, riders = rs.riders.Select(x => new { Value = x.Value, Text = x.Text, RouteCode = x.RouteCode }).ToList(), Remaining = rs.counts.Remaining, Used = rs.counts.Used }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(ProjectRunsheetModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.Save(u, model);
                return Json(new { sts = rs.success ? 1 : 2, rsno = rs.rsno, msg = rs.msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 3, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region PrintPage
        [HttpGet]
        public async Task<ActionResult> Print(string rs)
        {
            var u = Session["UserInfo"] as UserModel;
            ViewBag.Data = await repo.GetRunsheetDetailsForPrint(rs,u.BranchCode);
            return View();
        }
        #endregion

        #region PODScreen
        [HttpGet]
        public ActionResult POD()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetRunsheetData(string rs)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var data = await repo.GetRunsheetDetails(rs,u.BranchCode);
                var i = 1;
                var d = data.Select(x => new
                {
                    Sno = i++,
                    Date = x.Date.ToString("dd-MM-yyyy"),
                    Branch = x.Branch,
                    RunsheetNumber = x.RunsheetNumber,
                    RouteCode = x.RouteCode,
                    Route = x.Route,
                    RiderCode = x.RiderCode,
                    Rider = x.Rider,
                    CN = x.CN,
                    Address = x.Address,
                    Receiver = "",
                    PhoneNo = "",
                    Delivered = false

                }).ToList();

                return Json(new { sts = 1, data = d }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 3, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePOD(List<ProjectRunsheetPODModel> model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.UpdatePOD(u, model);
                return Json(new { sts = rs ? 1 : 2, msg = rs ? "Updated successfully" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 3, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SearchRunsheetScreen
        [HttpGet]
        public async Task<ActionResult> Runsheets()
        {
            var u = Session["UserInfo"] as UserModel;
            var routes = await repo.GetRoutes(u.BranchCode);
            ViewBag.Routes = routes.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Runsheets(DateTime date, string route)
        {
            var u = Session["UserInfo"] as UserModel;
            var routes = await repo.GetRoutes(u.BranchCode);
            ViewBag.Routes = routes.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewBag.Date = date.ToString("yyyy-MM-dd");
            ViewBag.Route = route;
            var runsheets = await repo.GetRunsheets(u.BranchCode, date, route);
            return View(runsheets);
        }
        #endregion
    }
}