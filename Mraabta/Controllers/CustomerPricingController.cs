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
    public class CustomerPricingController : Controller
    {
        CustomerPricingRepo repo;
        public CustomerPricingController()
        {
            repo = new CustomerPricingRepo();
        }
        // GET: ModifyDestination
        public async Task<ActionResult> Index()
        {
            ViewBag.Date = await repo.GetLastDate();
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> CheckAccStatus(string acc)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;

                var rs = await repo.GetAccounts(acc);
                if (rs != null)
                {
                    return Json(new
                    {
                        sts = 1,
                        data = rs.Select(c => new
                        {
                            AccCount = c.AccCount == null ? 0 : c.AccCount,
                            AccTitle = c.Name == null ? " " : c.Name,
                        }).FirstOrDefault()
                    }, JsonRequestBehavior.AllowGet); ;
                }
                else
                {
                    return Json(new { sts = 0, msg = "Account Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 0, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Save(string acc, string From, string To, int Compute)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;

                var rs = await repo.RunProcedures(acc, From, To, Compute);

                if (rs.Equals(1))
                {
                    return Json(new { sts = 1, msg = "Pricing has been Inserted !!" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { sts = 2, msg = "Pricing Not Inserted !! !!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}