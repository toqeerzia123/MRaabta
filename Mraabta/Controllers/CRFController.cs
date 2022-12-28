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
    public class CRFController : Controller
    {
        CRFRepo repo;
        public CRFController()
        {
            repo = new CRFRepo();
        }
        public async Task<ActionResult> Index()
        {
            await repo.OpenAsync();
            ViewBag.Industries = await repo.Industries();
            ViewBag.Banks = await repo.Banks();
            ViewBag.Branches = await repo.Branches();
            ViewBag.Groups = await repo.Groups();
            ViewBag.CustomerProducts = (await repo.CustomerProducts()).Select(x => new
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Selected = false
            }).ToList();
            repo.Close();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Save(Customer model)
        {
            try
            {
                if (model.IsSingle == 1)
                    model.GroupId = 0;
                model.PhoneNo = model.PhoneNo.Replace("-", "");
                model.CNIC = model.PhoneNo.Replace("-", "");

                model.ContactPersons.ForEach(x =>
                {
                    x.MobileNo = x.MobileNo.Replace("-", "");
                });

                model.PickupLocations.ForEach(x =>
                {
                    x.MobileNo = x.MobileNo.Replace("-", "");
                });

                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.Save(model, u);
                return Json(new { sts = 0, msg = "Customer saved successfully", id = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Print(int id)
        {
            var rs = await repo.Print(id);
            return View(rs);
        }

        [HttpGet]
        public async Task<ActionResult> Customers()
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await repo.Customers(u);
            ViewBag.UserType = (int)rs.type;
            return View(new { data = rs.data, lvlrates = rs.levelRates });
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus(CustomerStatusModel model)
        {
            try
            {
                (bool success, string msg) rs = (false, "");
                List<CustomerListModel> list = new List<CustomerListModel>();

                var u = Session["UserInfo"] as UserModel;

                if (model.Type == CRFUserType.AreaManager)
                {
                    rs = await repo.AMApproveReject(model.Approve, model.CustomerId, model.Remarks, u, model.Level);
                }
                else if (model.Type == CRFUserType.GeneralManager)
                {
                    rs = await repo.GMApproveReject(model.Approve, model.CustomerId, model.Remarks, u, model.Level);
                }
                else if (model.Type == CRFUserType.Director)
                {
                    rs = await repo.DRApproveReject(model.Approve, model.CustomerId, model.Remarks, u);
                }
                else if (model.Type == CRFUserType.ZA)
                {
                    rs = await repo.ZAApproveReject(model.Approve, model.CustomerId, model.Remarks, u);
                }

                if (rs.success)
                {
                    list = (await repo.Customers(u)).data;
                }

                return Json(new { sts = rs.success ? 0 : 1, msg = rs.msg, data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public async Task<ActionResult> GenerateAccountNo(int CustomerId, int RateId, string Name, string Branch)
        //{
        //    try
        //    {
        //        List<CustomerListModel> list = new List<CustomerListModel>();
        //        var rs = await repo.GenerateAccountNo(CustomerId, RateId, Name, Branch);
        //        if (rs.success)
        //        {
        //            var u = Session["UserInfo"] as UserModel;
        //            list = (await repo.Customers(u)).data;
        //        }
        //        return Json(new { sts = rs.success ? 0 : 1, msg = rs.msg, data = list }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}