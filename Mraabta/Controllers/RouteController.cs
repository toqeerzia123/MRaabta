using MRaabta.Models;
using MRaabta.Repo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class RouteController : Controller
    {
        RouteRepo repo;
        public RouteController()
        {
            repo = new RouteRepo();

        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {

            //return View();
            try
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                await repo.OpenAsync();
                var rs = await repo.GetRiders(branchcode);
                ViewBag.Riders = rs.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> GetRouteByRider(DateTime dt, int rid, string rop)
        {
            try
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                await repo.OpenAsync();
                var rs = await repo.GetRouteByRider(dt, rid.ToString(), rop, branchcode);
                repo.Close();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }


        }
    }
}