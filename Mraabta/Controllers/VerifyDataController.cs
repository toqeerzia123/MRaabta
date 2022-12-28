using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class VerifyDataController : Controller
    {
        VerifyDataRepo repo;
        public VerifyDataController()
        {
            repo = new VerifyDataRepo();
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                await repo.OpenAsync();
                var riders = await repo.GetRiders(branchcode);
                var accounts = await repo.GetAccounts(branchcode);
                repo.Close();

                ViewBag.Riders = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                ViewBag.Accounts = accounts.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();

                return View();
            }
            catch(Exception ex)
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
                var branchcode = Session["BRANCHCODE"].ToString();
                await repo.OpenAsync();
                var riders = await repo.GetRiders(branchcode);
                var accounts = await repo.GetAccounts(branchcode);
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
    }
}