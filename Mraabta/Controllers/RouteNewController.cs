using MRaabta.Repo;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class RouteNewController : Controller
    {
        RouteNewRepo repo;
        public RouteNewController()
        {
            repo = new RouteNewRepo();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await repo.OpenAsync();
            var rs = await repo.GetRiders();
            ViewBag.Riders = rs.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRouteByRider(DateTime dt, int rid)
        {
            await repo.OpenAsync();
            var rs = await repo.GetRouteByRider(dt, rid.ToString());
            repo.Close();
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}