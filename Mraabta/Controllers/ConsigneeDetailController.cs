using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MRaabta.App_Start;
using MRaabta.Util;

namespace MRaabta.Controllers
{
    public class ConsigneeDetailController : Controller
    {
        ShipperConsigneeRepo repo;
        BTS_DB pd;
        public ConsigneeDetailController()
        {
            try
            {
                repo = new ShipperConsigneeRepo();
                pd = new BTS_DB();
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<ActionResult> Index(int pickupId)
        {
            try
            {
                ViewBag.PickUpId = pickupId;
                var data = await repo.GetData(pickupId, Session["U_ID"].ToString());
                if (data == null)
                {
                    return RedirectToAction("Index", "PendingData");
                }
                return View(data ?? new ConsigneeShipperModel());
            }
            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }

        [HttpPost, SkipFilter]
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
                    if (rs == true || rs == false)
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index", new { pickupId = model.PickUpId });
                    }
                }
                return View(new ConsigneeShipperModel());
            }
            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }

        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetCountries(SearchModel model, string prefix)
        {
            ViewBag.Prefix = prefix;
            var rs = await repo.GetCountries(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetTowns(string prefix)
        {
            var rs = await repo.GetTowns(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetConsignmentNum(string prefix)
        {
            ViewBag.Prefix = prefix;
            var rs = await repo.GetConsignmentNum(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetConsignmentDetails(string prefix)
        {
            var rs = await repo.GetConsignmentDetails(prefix);
            return Json(new { list = rs.ToList() });
        }
        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetConsignmentAddDetails(string prefix)
        {
            var rs = await repo.GetConsignmentAddDetails(prefix);
            return Json(new { list = rs.ToList() });
        }

       // [HttpGet, SkipFilter]
        //protected void lnk_logout_Click(object sender, EventArgs e)
        //{
        //    Session.Clear();
        //    Session.Abandon();
        //    Session.RemoveAll();
        //    Response.Redirect("~/Login");
        //}
        //public ActionResult LogOut()
        //{
        //    try
        //    {
        //        //FormsAuthentication.SignOut();
        //        Session.Remove("NAME");
        //        Session.Clear();
        //        Session.Abandon();
        //        Session.RemoveAll();
        //        return Json(new { sts = 1 }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { sts = 0 }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //Dashboard


        //Daily Stats
        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetPoints()
        {
            try
            {
                await pd.OpenAsync();

                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await pd.TotalReasonPoints(branchcode);

                pd.Close();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                return null;
            }

        }
        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetMonthlyStats()

        {
            try
            {
                await pd.OpenAsync();
                List<DataPoint> dataPoints = new List<DataPoint>();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await pd.TotalMonthlyStats(branchcode);

                pd.Close();

                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                return null;
            }

        }
        [HttpPost, SkipFilter]
        public async Task<ActionResult> GetTodayConsignments()
        {
            try
            {
                await pd.OpenAsync();
                //List<DataPoint> dataPoints = new List<DataPoint>();
                if (Session["BRANCHCODE"] != null)
                {
                    var branchcode = Session["BRANCHCODE"].ToString();
                    var rs = await pd.TotalConsignments(branchcode);
                    pd.Close();
                    rs.ForEach(x => x.Time = x.PerformedOn.ToString("HH:mm"));
                    return Json(rs, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                return null;
            }

        }

    }
}