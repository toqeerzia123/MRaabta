using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace MRaabta.Controllers
{
    public class VerifyDetailController : Controller
    {
        VerifyDetailsRepo repo;
        public VerifyDetailController()
        {
            repo = new VerifyDetailsRepo();
        }
        public async Task<ActionResult> Index(int pickupId)
        {
            try
            {
                ViewBag.PickUpId = pickupId;
                var data = await repo.GetData(pickupId, Session["U_ID"].ToString());
                if (data == null)
                {
                    return RedirectToAction("Index", "VerifyData");
                }
                return View(data ?? new ConsigneeShipperModel());
            }
            catch(Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }


        [HttpPost]
        public async Task<ActionResult> Index(ConsigneeShipperModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        model.EnteredBy = Convert.ToInt32(Session["U_ID"]);
                    }
                    catch (Exception)
                    {
                        model.EnteredBy = 1;
                    }

                    var rs = await repo.AddData(model);
                    if (rs)
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index", new { pickupId = model.PickUpId });
                    }
                }
                return View(new ConsigneeShipperModel());
            }
            catch(Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCountries(string prefix)
        {
            //ViewBag.Prefix = prefix;
            var rs = await repo.GetCountries(prefix);
            return Json(new { list = rs.ToList() });
        }

    }
}