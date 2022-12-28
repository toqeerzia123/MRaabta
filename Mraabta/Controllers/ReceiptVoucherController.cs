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
    public class ReceiptVoucherController : Controller
    {
        ReceiptVoucherRepo repo = new ReceiptVoucherRepo();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetVoucherDateAndAmount(string VoucherID)
        {
            var data = await repo.GetVoucherDateAndAmount(VoucherID);
            if (data.Count != 0)
            {
                return Json(new
                {
                    VoucherDate = data.Select(x => x.VoucherDate).First(),
                    Amount = data.Select(x => x.Amount).First()
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetReport(string VoucherID, string VoucherDate)
        {
            DateTime allowedDate = repo.MonthEnd();
            DateTime dates = repo.MinimumDate();
            DateTime minAllowedDate = ((DateTime)dates).AddDays(1);
            DateTime maxAllowedDate = DateTime.Now;

            string Message;
            if (allowedDate == null || allowedDate == DateTime.MinValue)
            {
                Message = "Could Not Fetch Month End Date. Please Contact I.T. Support";
            }
            else if (DateTime.Parse(VoucherDate) < minAllowedDate || DateTime.Parse(VoucherDate) > maxAllowedDate)
            {
                Message = "Day End has already been performed for selected Dates. Cannot Update Voucher";
            }
            else
            {
                bool data = await repo.UpdateVoucherDate(VoucherID, VoucherDate);
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