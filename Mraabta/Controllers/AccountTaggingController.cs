using MRaabta.App_Start;
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
    public class AccountTaggingController : Controller
    {
        AccountTaggingRepo repo;
        public AccountTaggingController()
        {
            repo = new AccountTaggingRepo();
        }
        // GET: AccountTagging
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Index()
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();
            ViewBag.Accounts = await repo.GetAccounts(Session["BRANCHCODE"].ToString());
            repo.Close();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CheckEmpStatus(string empNo)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var CnStatus = await repo.GetEmpDetail(empNo);

                if (CnStatus != null)
                {
                    var rs = await repo.GetAccountsHistory(empNo);
                    if (rs.Count() > 0)
                    {
                        return Json(new
                        {
                            sts = 4,
                            data = rs.ToList().Select(x => new
                            {
                                UserName = x.UserName,
                                Accounts = x.Account,
                                UserType = x.StaffType
                            }).ToList()
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { sts = 3, msg = "No Account History Found !!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { sts = 1, msg = "Employee Detail Not Found !!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public async Task<ActionResult> Save(string empNo, string Account)
        //{
        //    try
        //    {
        //        var u = Session["UserInfo"] as UserModel;
        //        if (u != null)
        //        {

        //            var Cn = await repo.OnSave(empNo, u.Uid, Account);

        //            if (Cn > 0)
        //            {
        //                var rs = await repo.GetCNHistory(cn);
        //                return Json(new
        //                {
        //                    sts = 1,
        //                    msg = "CN Destination has been Updated !!",
        //                    data = rs.ToList().Select(x => new
        //                    {
        //                        CN = x.CN,
        //                        Destination = x.Destination,
        //                        CREATEDBY = x.CREATEDBY,
        //                        CREATEDON = x.CREATEDON.ToString()
        //                    }).OrderByDescending(c => Convert.ToDateTime(c.CREATEDON)).ToList()
        //                }, JsonRequestBehavior.AllowGet);
        //            }

        //            return Json(new { sts = 2, msg = "Same Destination Already Exist !! !!" }, JsonRequestBehavior.AllowGet);


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }

        //}
    }
}