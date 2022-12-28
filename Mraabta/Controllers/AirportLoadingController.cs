using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MRaabta.Repo;
using System.Web.Mvc;
using MRaabta.Util;
using MRaabta.App_Start;

namespace MRaabta.Controllers
{
    public class AirportLoadingController : Controller
    {
        AirportLoadingRepo repo;
        public AirportLoadingController()
        {
            repo = new AirportLoadingRepo();
        }

        // GET: AirportLoading
        #region Airport Loading
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Index(string id = null)
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();
            var routes = await repo.GetRoutes(u.BranchCode);
            var transportType = await repo.GetTransportType();
            var vehicleType = await repo.GetVehicleType();
            var vehicles = await repo.GetVehicles();
            var cnLength = await repo.GetConsignmentsLength();
            var org = await repo.GetBranchDetails(u.BranchCode);
            var branches = await repo.GetBranches();

            if (!string.IsNullOrEmpty(id))
            {
                var loadingId = long.Parse(CryptoEngine.Base64Decode(id));
                var loadingData = await repo.GetLoadingDataForEdit(loadingId);
                var tp = await repo.GetRoutesTouchpoints(u.BranchCode, loadingData.loading.RouteId.ToString());
                var dest = await repo.GetRouteDestinations(loadingData.loading.RouteId.ToString());
                ViewBag.LoadingData = new { id = loadingId, loading = loadingData.loading, details = loadingData.details.OrderByDescending(x => x.SortOrder).ToList(), touchpoints = tp, destinations = dest };
            }
            else
            {
                ViewBag.LoadingData = new { id = 0, loading = new LoadingModel(), details = new List<LoadingDetailModel>(), touchpoints = new List<DropDownModel>(), destinations = new List<DropDownModel>() };
            }

            repo.Close();

            ViewBag.Origin = org;
            ViewBag.Routes = routes;
            ViewBag.TransportType = transportType;
            ViewBag.VehicleType = vehicleType;
            ViewBag.Vehicles = vehicles;
            ViewBag.CNLengths = cnLength.Select(x => new { Product = x.Product, Prefix = x.Prefix, Length = x.Length, PrefixLength = x.PrefixLength }).ToList();
            ViewBag.LoadingId = 0;
            ViewBag.Branches = branches;

            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetTouchpointsAndDestinations(string routeId)
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();
            var tp = await repo.GetRoutesTouchpoints(u.BranchCode, routeId);
            var dest = await repo.GetRouteDestinations(routeId);
            repo.Close();
            return Json(new { tp, dest }, JsonRequestBehavior.AllowGet);
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
                        Weight = rs.data.Weight,
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
                    return Json(new { type = 0, rs.CN, rs.OriginId, rs.Origin, rs.DestinationId, rs.Destination, rs.Weight, rs.Pieces, rs.ServiceType, rs.ConsignmentTypeId }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> LoadingDetails(long lid)
        {
            try
            {
                var rs = await repo.UnLoadingDetails(lid);
                return Json(new { sts = 1, data = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> PostLoading(AirLoadingModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                model.ECCode = u.ExpressCenter;
                model.BranchCode = int.Parse(u.BranchCode);
                model.ZoneCode = int.Parse(u.ZoneCode);
                model.LocationId = u.LocationId;
                model.CreatedBy = u.Uid;

                model.VehicleRegNo = "";
                model.Location = u.LocationName;

                if (model.Id == 0)
                {
                    var rs = await repo.InsertLoading(model);
                    return Json(new { sts = 0, id = rs, msg = $"Loading# {rs} saved" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (await repo.IsUnloaded(model.Id) == 1)
                    {
                        return Json(new { sts = 1, msg = "Loading already unloaded" }, JsonRequestBehavior.AllowGet);
                    }

                    var rs = await repo.UpdateLoading(model);
                    return Json(new { sts = 0, id = rs.ToString(), msg = $"Loading# {rs} updated" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetRunnerRoutes(int routeId)
        {
            try
            {
                var rs = await repo.GetRunnerRoutes(routeId);
                return Json(new { sts = 0, data = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Search Airport Loading
        public async Task<ActionResult> Search()
        {
            await repo.OpenAsync();
            var branches = await repo.GetBranches();
            var transportType = await repo.GetTransportType();
            repo.Close();
            ViewBag.Branches = branches;
            ViewBag.TransportType = transportType;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> GetLoadings(DateTime date, string loadingNo, string sealNo, string destinationId, string transportType)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetLoadings(u.BranchCode, date, loadingNo, sealNo, destinationId, transportType);
                var data = rs.Select(x => new
                {
                    Id = x.Id,
                    EncryptedId = CryptoEngine.Base64Encode(x.Id.ToString()),
                    TransportType = x.TransportType,
                    Date = x.Date,
                    VehicleName = x.VehicleName,
                    CourierName = x.CourierName,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    Description = x.Description,
                    FlightNo = x.FlightNo,
                    SealNo = x.SealNo,
                    DepartureFlightDate = x.DepartureFlightDate,
                    TotalWeight = String.Format("{0:0.0}", x.TotalWeight),
                    AtAirport = x.AtAirport,
                    CreatedBy = x.CreatedBy,
                    IsUnloaded = x.IsUnloaded
                });
                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult Print()
        {
            return View();
        }
    }
}