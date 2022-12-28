using MRaabta.App_Start;
using MRaabta.Models;
using MRaabta.Repo;
using MRaabta.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class BaggingController : Controller
    {
        BaggingRepo repo;
        public BaggingController()
        {
            repo = new BaggingRepo();
        }

        #region Bagging

        [HttpGet]
        public async Task<ActionResult> Index(string no = null)
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();
            var cnlengths = await repo.GetConsignmentsLength();
            ViewBag.CNLengths = cnlengths.Select(x => new { x.Product, x.Prefix, x.PrefixLength, x.Length }).ToList();
            ViewBag.Origin = u.BranchName;
            ViewBag.Branches = await repo.Branches();
            ViewBag.Products = await repo.NewProducts();

            if (no != null)
            {
                no = CryptoEngine.Base64Decode(no);
                var rs = await repo.GetDataForEdit(no);
                if (rs.data != null && rs.details != null)
                {
                    var data = new
                    {
                        BagNo = rs.data.BNo,
                        Branch = rs.data.Branch,
                        Destination = rs.data.Destination,
                        Product = rs.data.Product,
                        Service = rs.data.Service,
                        BagType = rs.data.Type,
                        SHS = rs.data.SHS,
                        TotalWeight = "",
                        SealNo = ""
                    };

                    var details = rs.details.Select(x => new
                    {
                        ManCN = x.ManCN,
                        ServiceType = x.ServiceType,
                        Origin = x.Origin,
                        DestinationId = x.DestinationId,
                        Destination = x.Destination,
                        Weight = x.Weight,
                        Pcs = x.Pcs,
                        IsMan = x.IsMan,
                        Remarks = x.Remarks,
                        AccountNo = x.AccountNo
                    }).ToList();

                    ViewBag.BaggingData = new { bagno = no, data = data, details = details };
                }
                else
                {
                    ViewBag.BaggingData = new { bagno = (string)null };
                }

            }
            else
            {
                ViewBag.BaggingData = new { bagno = no };
            }

            repo.Close();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CheckBag(string bagno)
        {
            try
            {
                var rs = await repo.IsBagExists(bagno);
                return Json(new { sts = rs ? 1 : 0, msg = rs ? "Bag already exists" : "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Services(int product)
        {
            try
            {
                var rs = await repo.NewServices(product);
                return Json(new { sts = 0, data = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetManisfest(string manifestNo)
        {
            try
            {
                var x = await repo.GetManisfest(manifestNo);
                if (x != null)
                {
                    var data = new
                    {
                        Id = 1,
                        ManCN = x.ManCN,
                        ServiceType = x.ServiceType,
                        Origin = x.Origin,
                        DestinationId = x.DestinationId,
                        Destination = x.Destination,
                        Weight = x.Weight,
                        Pcs = x.Pcs,
                        IsMan = true,
                        Remarks = "",
                        AccountNo = x.AccountNo
                    };
                    return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = "Manifest not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCN(string cn, int dest)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var x = await repo.GetCN(cn, int.Parse(u.BranchCode), "Bagging", dest);
                if (x != null)
                {
                    if (x.IsValid == 1)
                    {
                        var data = new
                        {
                            ManCN = x.CN,
                            ServiceType = x.ServiceType,
                            Origin = x.Origin,
                            DestinationId = x.AccountNo == "4D1" ? dest : x.DestinationId,
                            Destination = x.Destination,
                            Weight = x.Weight,
                            Pcs = x.Pcs,
                            IsMan = false,
                            Remarks = "",
                            AccountNo = x.AccountNo
                        };
                        return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { sts = 1, msg = x.Msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { sts = 1, msg = "No data found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                repo.Close();
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetBranchDestinations(int branch, string type)
        {
            try
            {
                var x = await repo.GetBranchDestinations(branch, type);
                return Json(new { sts = 0, data = x }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                repo.Close();
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Insert(BagModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                (bool success, string response) rs = (false, "");
                if (!model.Edit)
                    rs = await repo.Insert(model, u);
                else
                    rs = await repo.Update(model, u);

                return Json(new { sts = rs.success ? 0 : 1, id = rs.response, msg = rs.success ? $"Bag No {rs.response} saved" : rs.response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                repo.Close();
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Search

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchBags(DateTime date, bool isBag, string no)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.SearchBags(date, isBag, no, u.BranchCode);
                var data = rs.Select(x => new
                {
                    BagNo = x.BagNo,
                    EncBagNo = CryptoEngine.Base64Encode(x.BagNo),
                    TotalWeight = x.TotalWeight,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    SealNo = x.SealNo,
                    Edit = x.Edit,
                    CreatedOn = x.CreatedOn
                });
                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Print

        [HttpGet, SkipFilter]
        public async Task<ActionResult> Print(string bagno)
        {
            var rs = await repo.PrintData(bagno);
            return View(rs);
        }
        #endregion

        #region Routes
        public async Task<ActionResult> Routes()
        {
            ViewBag.Branches = await repo.Branches(true);
            var routes = await repo.GetRoutesData();
            ViewBag.RoutesData = routes.Select(x => new
            {
                x.Id,
                x.DestinationId,
                x.Destination,
                x.BranchId,
                x.Branch,
                x.Type,
                x.CreatedBy,
                x.CreatedOn
            }).ToList();
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> FilterBags(string Dest, string Branch, string Type)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var routes = await repo.GetFilteredRoutesData(Dest, Branch, Type);
                var data = routes.Select(x => new
                {
                    x.Id,
                    x.DestinationId,
                    x.Destination,
                    x.BranchId,
                    x.Branch,
                    x.Type,
                    x.CreatedBy,
                    x.CreatedOn
                }).ToList();

                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveRoute(int DestinationId, int BranchId, string Type)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.SaveRoute(DestinationId, BranchId, Type, u.Uid);
                if (rs)
                {
                    var routes = await repo.GetRoutesData();
                    var data = routes.Select(x => new
                    {
                        x.Id,
                        x.DestinationId,
                        x.Destination,
                        x.BranchId,
                        x.Branch,
                        x.Type,
                        x.CreatedBy,
                        x.CreatedOn
                    }).ToList();
                    return Json(new { sts = 0, msg = "Route saved successfully", data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = "Something went wrong" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> DeleteRoute(int id)
        {
            try
            {
                var rs = await repo.DeleteRoute(id);
                if (rs)
                {
                    var routes = await repo.GetRoutesData();
                    var data = routes.Select(x => new
                    {
                        x.Id,
                        x.DestinationId,
                        x.Destination,
                        x.BranchId,
                        x.Branch,
                        x.Type,
                        x.CreatedBy,
                        x.CreatedOn
                    }).ToList();
                    return Json(new { sts = 0, msg = "Route Deleted successfully", data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = "Something went wrong" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TagPrint
        [HttpGet, SkipFilter]
        public async Task<ActionResult> TagPrint(string bagno)
        {
            var rs = await repo.GetTagPrint(bagno);
            return View(rs);
        }
        #endregion
    }
}