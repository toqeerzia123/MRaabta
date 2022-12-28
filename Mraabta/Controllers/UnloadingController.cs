using MRaabta.App_Start;
using MRaabta.Models;
using MRaabta.Repo;
using MRaabta.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class UnloadingController : Controller
    {
        UnloadingRepo repo;
        public UnloadingController()
        {
            repo = new UnloadingRepo();
        }

        #region UnloadingPage
        [HttpGet]
        public async Task<ActionResult> Index(string id)
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();
            if (!string.IsNullOrEmpty(id))
            {
                var unloadingId = long.Parse(CryptoEngine.Base64Decode(id));
                var rs = await repo.GetUnloadingDataForEdit(unloadingId);
                ViewBag.UnloadingData = new { loadings = rs.loadings, details = rs.details, id = unloadingId };
            }
            else
            {
                ViewBag.UnloadingData = new { loadings = new List<LoadingInfo>(), details = new List<LoadingInfoDetails>(), id = 0 };
            }
            var cnLength = await repo.GetConsignmentsLength();
            var userbranch = await repo.GetBranch(u.BranchCode);
            repo.Close();
            ViewBag.Userbranch = userbranch;
            ViewBag.CNLengths = cnLength.Select(x => new { Product = x.Product, Prefix = x.Prefix, Length = x.Length, PrefixLength = x.PrefixLength }).ToList();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoadingDetails(long lid)
        {
            try
            {
                var rs = await repo.LoadingDetails(lid);
                return Json(new { sts = 1, data = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCNInfo(string cn)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                await repo.OpenAsync();
                var dvrtscheck = await repo.DVRTSPrimaryCheck(cn);
                if (string.IsNullOrEmpty(dvrtscheck))
                {
                    var rs = await repo.GetCN(cn);
                    repo.Close();
                    if (cn[0] == '5' && cn.Length == 15)
                    {
                        if (string.IsNullOrEmpty(rs.CN))
                        {
                            return Json(new { type = 1, msg = "Booking required for COD Consignment" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (rs.Status == 9)
                            {
                                return Json(new { type = 1, msg = "Consignment must not be void" }, JsonRequestBehavior.AllowGet);
                            }

                            if (rs.ReachedDestination)
                            {
                                return Json(new { type = 1, msg = "Once reached destination need RO/MR to move" }, JsonRequestBehavior.AllowGet);
                            }

                            if (!rs.IsApproved && u.BranchCode != rs.OriginId.ToString())
                            {
                                return Json(new { type = 1, msg = "First operation must be at origin" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    return Json(new { type = 0, rs.CN, rs.OriginId, rs.Origin, rs.DestinationId, rs.Destination, Weight = rs.Weight.ToString(), Pieces = rs.Pieces.ToString(), rs.ServiceType, rs.ConsignmentTypeId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    repo.Close();
                    return Json(new { type = 1, msg = dvrtscheck }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                repo.Close();
                return Json(new { type = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetBagInfo(string bagno)
        {
            var rs = await repo.GetBagInfo(bagno);
            if (!rs.error && rs.found)
            {
                return Json(new
                {
                    type = 0,
                    data = new
                    {
                        Bag = rs.data.Bag,
                        Weight = rs.data.Weight.ToString(),
                        OriginId = rs.data.OriginId,
                        DestinationId = rs.data.DestinationId,
                        SealNo = rs.data.SealNo,
                        OriginName = rs.data.OriginName,
                        DestinationName = rs.data.DestinationName
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else if (!rs.error && !rs.found)
            {
                return Json(new
                {
                    type = 1,
                    msg = $"Bag# {bagno} Not Found"
                }, JsonRequestBehavior.AllowGet);
            }
            else if (rs.error)
            {
                return Json(new
                {
                    type = 2,
                    msg = rs.msg
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveUnloading(long id, Unloading model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                model.DestinationId = int.Parse(u.BranchCode);
                model.BranchCode = u.BranchCode;
                model.CreatedBy = u.Uid;
                model.LocationId = u.LocationId;
                model.Location = u.LocationName;
                model.ZoneCode = u.ZoneCode;
                if (id <= 0)
                {
                    var rs = await repo.SaveUnloading(model);
                    return Json(new { sts = 0, EncryptedId = rs.id.ToString(), msg = rs.msg }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.Id = id;
                    var rs = await repo.UpdateUnloading(model);
                    return Json(new { sts = 0, EncryptedId = rs.id.ToString(), msg = rs.msg }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Search Unloading

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetUnloadings(DateTime date)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetUnloadings(u.BranchCode, date.ToString("yyyy-MM-dd"));
                var data = rs.Select(x => new
                {
                    Id = x.Id,
                    EncryptedId = CryptoEngine.Base64Encode(x.Id.ToString()),
                    Origin = x.Origin,
                    Destination = x.Destination,
                    Date = x.Date.ToString("dd-MMM-yyyy")
                }).ToList();
                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region UnloadingPrint

        [HttpGet, SkipFilter]
        public async Task<ActionResult> Print(long id)
        {
            var u = Session["UserInfo"] as UserModel;
            ViewBag.Name = u.UserName;
            var rs = await repo.UnloadingDetails(id);
            return View(rs);
        }
        #endregion
    }
}