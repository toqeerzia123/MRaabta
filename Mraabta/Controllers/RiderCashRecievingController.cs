using MRaabta.App_Start;
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

namespace MRaabta.Controllers
{
    public class RiderCashRecievingController : Controller
    {
        RiderCashRecievingRepo repo;
        public RiderCashRecievingController()
        {
            repo = new RiderCashRecievingRepo();
        }
        // GET: RiderPayment
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Index()
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();
            var riders = await repo.GetRiders();
            ViewBag.Riders = riders.Select(x => new SelectListItem { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();

            repo.Close();
            return View();
        }
        public async Task<ActionResult> GetRiderCNDetails(string RiderId)
        {
            try
            {
                var CNData = await repo.getTodayCNs(RiderId);

                if (CNData.Count() > 0)
                {
                    var TotalExpAmount = CNData.Sum(x => Convert.ToDouble(x.AmountRcv));
                    return Json(new { sts = 1, CNsDetail = CNData, TotalExpAmount = TotalExpAmount }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { sts = 0, msg = "Data Not Found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 0, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> GetRiderPaymentDetail(string RiderId)
        {
            try
            {
                var PayData = await repo.getTodayTransactionsByRider(RiderId);
                var CNData = await repo.getTodayCNs(RiderId);

                var TotalExpAmountToday = await repo.getTodaySumAmount(RiderId);
                var ExpAmount = CNData.Where(c => c.IsRecieved.Equals(false) && !c.AmountRcv.Equals(0)).Sum(x => Convert.ToDouble(x.RiderAmount));
                //var ExpRcvAmount = CNData.Where(c => c.IsPaid.Equals(false) && !c.AmountRcv.Equals(0)).Sum(x => Convert.ToDouble(x.AmountRcv));
                return Json(new { sts = 1, RiderId = RiderId, PayDetail = PayData.Count() < 0 ? null : PayData, CNDetail = CNData.Count() < 0 ? null : CNData.OrderBy(c => c.IsRecieved), TotalExpAmount = TotalExpAmountToday, ExpAmount = ExpAmount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 0, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Save(RiderCashRecievingModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var RiderExist = await repo.CheckRider(model.RiderCode);

                if (RiderExist != null)
                {
                 //   var expresscenter = await repo.GetExpressCenter(u.Uid.ToString());

                 //   model.ECCode = expresscenter;
                    model.RiderName = RiderExist;
                    var Status = await repo.InsertData(model, u);

                    if (Status != null)
                    {
                        return Json(new
                        {
                            sts = 1,
                            msg = "Rider Amount has been Recieved Successfully !!",
                            DataId = Status,
                            RiderId = model.RiderCode,
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { sts = 2, msg = "Data Not Inserted !! !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Print(string Id, string RiderId)
        {
            var rs = await repo.PrintVoucher(Id, RiderId);
            return View(rs);
        }
        [HttpGet, SkipFilter]
        public async Task<ActionResult> SendSMStoRider(string Id, string RiderId)
        {
            var rs = await repo.SendSMS(Id, RiderId);

            return View(rs);
        }
    }
}