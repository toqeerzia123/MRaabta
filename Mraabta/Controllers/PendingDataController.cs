using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MRaabta.Models;

namespace MRaabta.Controllers
{
    public class PendingDataController : Controller
    {
        PendingDataRepo repo;
        public PendingDataController()
        {
            repo = new PendingDataRepo();
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                var branchCode = Session["BRANCHCODE"].ToString();
                await repo.OpenAsync();
                var riders = await repo.GetRiders(branchCode);
                var accounts = await repo.GetAccounts(branchCode);
                repo.Close();

                ViewBag.Riders = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                ViewBag.Accounts = accounts.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();

                return View();
            }
            catch (Exception)
            {
                ViewBag.err = "error";
                return View();
            }
        
        }

        [HttpPost]
        public async Task<ActionResult> Index(DateTime date, int? riderCode, string accountCode)
        {
            try
            {
                var branchCode = Session["BRANCHCODE"].ToString();
                await repo.OpenAsync();
                var riders = await repo.GetRiders(branchCode);
                var accounts = await repo.GetAccounts(branchCode);
                var rs = await repo.GetGrid(date.Date, riderCode, accountCode);
                repo.Close();           
                ViewBag.Riders = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                ViewBag.Accounts = accounts.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                if (rs == null)
                {
                    Response.Write(@"<script language='javascript'>alert('" + "No Records Found" + "');</script>");
                    return View(rs);

                }
                return View(rs);
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
           // ViewBag.Prefix = prefix;
            var rs = await repo.GetCountries(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost]
        public async Task<ActionResult> GetTowns(string prefix)
        {
            var rs = await repo.GetTowns(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost]
        public async Task<ActionResult> GetConsignmentNum(string prefix)
        {
            //ViewBag.Prefix = prefix;
            var rs = await repo.GetConsignmentNum(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost]
        public async Task<ActionResult> GetConsignmentDetails(string prefix)
        {
            var rs = await repo.GetConsignmentDetails(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost]
        public async Task<ActionResult> GetConsignmentAddDetails(string prefix)
        {
            var rs = await repo.GetConsignmentAddDetails(prefix);
            return Json(new { list = rs.ToList() });
        }
    }
}