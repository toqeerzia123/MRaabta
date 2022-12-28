using MRaabta.App_Start;
using MRaabta.Models;
using MRaabta.Repo;
using MRaabta.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MRaabta.Controllers
{
    public class RunsheetController : Controller
    {
        RunsheetRepo repo;
        public RunsheetController()
        {
            repo = new RunsheetRepo();
        }

        #region Runsheet
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;

                var chiefcourierprofiles = ConfigurationManager.AppSettings["chiefcourierprofiles"]?.Split(',')?.Select(x => int.Parse(x))?.ToList();
                if (chiefcourierprofiles != null && chiefcourierprofiles.Contains(u.Profile))
                {
                    ViewBag.IsChief = true;
                }
                else
                {
                    ViewBag.IsChief = false;
                }

                await repo.OpenAsync();
                var cnlengths = await repo.GetConsignmentsLength();
                var routes = await repo.RouteInfo(u.BranchCode);
                ViewBag.RunsheeTypes = await repo.RunsheeTypes();
                ViewBag.VehicleTypes = await repo.GetVehicleType();
                ViewBag.Vehicles = await repo.GetVehicles();
                ViewBag.CNLengths = cnlengths.Select(x => new { x.Product, x.Prefix, x.PrefixLength, x.Length }).ToList();
                ViewBag.Routes = routes.Select(x => new { x.RouteCode, x.Route, x.LandMark, x.RiderCode, x.Rider, x.Password, x.PhoneNo, x.IsChief }).ToList();
                repo.Close();
            }
            catch (Exception ex)
            {
                repo.Close();
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetCNInfo(string cn)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetCN(cn, int.Parse(u.BranchCode), "Runsheet");
                if (rs != null)
                {
                    if (rs.IsValid == 1)
                    {
                        return Json(new
                        {
                            type = 0,
                            CN = rs.CN,
                            Origin = rs.Origin,
                            DestinationId = rs.DestinationId,
                            Destination = rs.Destination,
                            ConsignmentType = rs.ConsignmentType,
                            IsCod = rs.IsCod
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { type = 1, msg = rs.Msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { type = 1, msg = "No data found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                repo.Close();
                return Json(new { type = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(RunsheetModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;

                model.BranchCode = u.BranchCode;
                model.ZoneCode = u.ZoneCode;
                model.ECCode = u.ExpressCenter;
                model.CreatedBy = u.Uid;
                model.Location = u.LocationName;

                var rs = await repo.Save(model);

                return Json(new { sts = rs.isOk ? 0 : 1, EncryptedId = rs.isOk ? CryptoEngine.Base64Encode(rs.response) : "0", msg = rs.isOk ? $"Runsheet {rs.response} saved" : rs.response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GeneratePin(string ridercode, string phoneno)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var pin = new Random().Next(0, 1000000).ToString("D6");
                bool msgSended = false;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var msg = $"your code for runsheet is {pin}";
                    var response = await client.GetAsync($"https://gateway.its.com.pk/api?action=sendmessage&username=MP_Express&password=mPty5a!701&recipient={phoneno}&originator=82660&messagedata={HttpUtility.UrlEncode(msg)}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var xml = XDocument.Parse(await response.Content.ReadAsStringAsync());


                        var data = xml.Root.Element("data");
                        var acceptreport = data?.Element("acceptreport");
                        var messageid = acceptreport?.Element("messageid")?.Value ?? "0";
                        var errorno = acceptreport?.Element("statuscode")?.Value ?? data?.Element("errorcode")?.Value;
                        var statusmessage = acceptreport?.Element("statusmessage")?.Value ?? data?.Element("errormessage")?.Value;
                        await repo.GeneratePinAndLog(u.BranchCode, ridercode, pin, phoneno, msg, u.Uid, int.Parse(errorno), statusmessage, int.Parse(messageid));
                        if (int.Parse(errorno) == 0)
                            msgSended = true;
                    }
                    else
                    {
                        await repo.GeneratePinAndLog(u.BranchCode, ridercode, pin, phoneno, msg, u.Uid, 500, "Internal Server Error", 0);
                    }
                }

                return Json(new { sts = msgSended ? 0 : 1, pin = msgSended ? pin : "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateRiderPass(string ridercode, string pass)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.UpdateRiderPass(u.BranchCode, ridercode, pass);
                return Json(new { sts = rs ? 0 : 1, msg = rs ? "Password updated successfully" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Search Runsheet
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchRunsheets(DateTime date, string routeCode)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.SearchRunsheets(u.BranchCode, date.ToString("yyyy-MM-dd"), routeCode);
                var data = rs.Select(x => new
                {
                    EncryptedId = CryptoEngine.Base64Encode(x.RunsheetNumber),
                    RunsheetNumber = x.RunsheetNumber,
                    RunsheetDate = x.RunsheetDate.ToString("dd-MM-yyyy"),
                    RouteCode = x.RouteCode,
                    RunsheetCount = x.RunsheetCount,
                }).ToList(); ;
                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Print Runsheet
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Print(string id)
        {
            var list = new List<List<RunsheetPrintViewModel>>();
            var x = long.Parse(CryptoEngine.Base64Decode(id));
            var data = await repo.GetData(x);
            int skip = 0;
            int take = 20;
            while (skip < data.Count())
            {
                list.Add(data.Skip(skip).Take(take).ToList());
                skip += take;
            }
            return View(list);
        }
        #endregion
    }
}