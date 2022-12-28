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
    public class RetailCODCRFController : Controller
    {
        RetailCODCRFRepo repo = new RetailCODCRFRepo();
        // GET: RetailCODCRF
        public async Task<ActionResult> Index()
        {
            ViewBag.Zone = new SelectList(await repo.GetZones(), "zoneCode", "ZoneName");
            ViewBag.Branch = new SelectList(await repo.GetBranches(0), "branchCode", "BranchName");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetReport(long zoneCode, string branchCode, string accountNo, string customerName)
        {
            var data = await repo.GetData(zoneCode, branchCode, accountNo, customerName);
            return PartialView("_GetReport", data);
        }

        [HttpPost]
        public async Task<JsonResult> GetBranches(long zoneCode)
        {
            ViewBag.Branch = new SelectList(await repo.GetBranches(zoneCode), "branchCode", "BranchName");
            return Json(ViewBag.Branch, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> ApproveCODCRF(string accountno, bool approve, long zoneCode, string branchCode, string account, string customerName)
        {
            int app = 1;
            if (approve)
            {
                app = 0;
            }
            var update = await repo.ApproveCRF(accountno, app);
            var data = await repo.GetData(zoneCode, branchCode, account, customerName);
            return PartialView("_GetReport", data);
        }
        public async Task<ActionResult> EditDetails(string accountno)
        {
            var data = await repo.GetData(0, null, " ", null);
            if (accountno != null)
            {
                data = await repo.GetData(0, null, accountno, null);
            }
            if (data.Count != 0 && data.Count > 0)
            {
                ViewBag.Bank = new SelectList(await repo.GetBanks(), "BankId", "BankName", data.FirstOrDefault().BankId);
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateDetail(RetailCODCRFNewModel model)
        {
            int updatesuccess = await repo.UpdateDetail(model);
            if (updatesuccess == 1)
            {
                TempData["Message"] = "Saved Successfully!";
            }
            else
            {
                TempData["Message"] = "Unsuccessful!";
            }
            return RedirectToAction("EditDetails", new { accountno = model.AccountNumber });
        }
    }
}