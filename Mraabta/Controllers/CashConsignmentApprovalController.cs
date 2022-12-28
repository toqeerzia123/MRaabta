using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class CashConsignmentApprovalController : Controller
    {
        CashConsignmentApprovalRepo repo = new CashConsignmentApprovalRepo();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetDetail(string CN)
        {
            var data = await repo.GetDetail(CN);
            if (data!=null)
            {
                return Json(new
                {
                    RiderCode = data.Select(x => x.RIDERCODE).First(),
                    Branch = data.Select(x => x.BRANCH).First(),
                    Zone = data.Select(x => x.ZONE).First(),
                    ExpressCenterCode = data.Select(x => x.expressCenterCode).First(),
                    SaleDate = data.Select(x => x.SALEDATE).First()
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateDetail(string CN, string SaleDate)
        {
            DateTime allowedDate = repo.MonthEnd();
            string Message;
            if (allowedDate == null || allowedDate == DateTime.MinValue)
            {
                Message = "Could Not Fetch Month End Date. Please Contact I.T. Support";
            }
            else
            {
                bool data = await repo.UpdateDetail(CN, SaleDate);
                if (data)
                {
                    Message = "Update Successful";
                }
                else
                {
                    Message = "Unsuccessful";
                }
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
    }
}