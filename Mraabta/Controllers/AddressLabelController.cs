using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MRaabta.Repo;

namespace MRaabta.Controllers
{
    public class AddressLabelController : Controller
    {
        AddressLabelRepo repo = new AddressLabelRepo();
        // GET: AddressLabel
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> GetReport(string Start, string End, int Type, string Service)
        {
            var data = await repo.GetData(Start, End, Type, Service);
            ViewBag.Type = Type;
            return PartialView("_GetReport", data);
        }

        [HttpPost]
        public ActionResult SetTempCNs(string cons, int type)
        {
            if (cons != "")
            {
                TempData["cons"] = cons;
                TempData["type"] = type;
            }
            return Json(new { sts = 0 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ViewLoadSheet(int? type, string cn = null)
        {
            string cons = "";
            int t = 0;

            if (cn != null && type.HasValue)
            {
                cons = cn;
                t = type.Value;
                var data = await repo.GetDetail(cons, t);
                return View(data);
            }
            else
            {
                if (TempData["cons"] != null && TempData["type"] != null)
                {
                    cons = TempData["cons"].ToString();
                    t = Convert.ToInt32(TempData["type"]);
                    var data = await repo.GetDetail(cons, t);
                    return View(data);
                }
            }
            return View();
            
        }

    }
}
