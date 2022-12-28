using Dapper;
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
    public class ModifyDestinationController : Controller
    {
        RedefiningDestinationRepo repo;
        public ModifyDestinationController()
        {
            repo = new RedefiningDestinationRepo();
        }
        // GET: ModifyDestination
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Index()
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();
            ViewBag.Branches = await repo.GetBranches();
            repo.Close();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CheckCNStatus(string cn)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var CnStatus = await repo.GetCNDetail(cn);

                if (CnStatus == 4)
                {
                    var rs = await repo.GetCNHistory(cn);
                    if (rs.Count() > 0)
                    {
                        return Json(new
                        {
                            sts = 4,
                            data = rs.ToList().Select(x => new
                            {
                                CN = x.CN,
                                Destination = x.Destination,
                                CREATEDBY = x.CREATEDBY,
                                CREATEDON = x.CREATEDON.ToString()
                            }).OrderByDescending(c => Convert.ToDateTime(c.CREATEDON)).ToList()
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { sts = 3, msg = "No History Found !!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (CnStatus == 1)
                {
                    return Json(new { sts = 1, msg = "Already Delivered / Returned" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = "No Data Found !!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Save(string cn, string City)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var CNDestination = await repo.CheckCNDestination(cn, City);

                if (CNDestination == 1)
                {
                    var Cn = await repo.GetCNRedefiningDetail(cn, u.Uid, City);

                    if (Cn > 0)
                    {
                        var rs = await repo.GetCNHistory(cn);
                        return Json(new
                        {
                            sts = 1,
                            msg = "CN Destination has been Updated !!",
                            data = rs.ToList().Select(x => new
                            {
                                CN = x.CN,
                                Destination = x.Destination,
                                CREATEDBY = x.CREATEDBY,
                                CREATEDON = x.CREATEDON.ToString()
                            }).OrderByDescending(c => Convert.ToDateTime(c.CREATEDON)).ToList()
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { sts = 2, msg = "Same Destination Already Exist !! !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}