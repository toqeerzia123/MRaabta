using Microsoft.Ajax.Utilities;
using MRaabta.Models;
using MRaabta.Repo;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace MRaabta.Controllers
{
    public class DeliveryController : Controller
    {
        DeliveryDB pd;
        SqlConnection orcl;
        DebreifingAllRepo dbrepo;
        public DeliveryController()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            pd = new DeliveryDB();
            dbrepo = new DebreifingAllRepo();
        }

        //GET: Delivery
        DeliveryDB pdb = new DeliveryDB();
        LocationDB ldb = new LocationDB();
        RiderDB rdb = new RiderDB();

        public ActionResult ViewDelivery()
        {
            return View();
        }
        //Get Riders
        public async Task<ActionResult> ViewRiders(string SDate, string EDate)
        {
            try
            {
                string branchcode = Session["BRANCHCODE"].ToString();
                await pd.OpenAsync();
                var riders = await pd.GetRiders(SDate, EDate, branchcode);
                pd.Close();
                return Json(riders, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        // GET: Delivery
        public async Task<ActionResult> getDeliveryByRider(string riderCode, string StartDate, string EndDate)
        {
            try
            {
                await pdb.OpenAsync();
                double suminKMs = 0;
                string allRunsheets = "";
                List<double> sumdist = new List<double>();
                String branchcode = Session["BRANCHCODE"].ToString();
                GetDeliveryDataWithDistance response = new GetDeliveryDataWithDistance();
                var GetDeliveryData = new GetDeliveryData();
                List<GetDeliveryData> lp = new List<GetDeliveryData>();
                lp = await pdb.getDelivery(riderCode, StartDate, EndDate, branchcode);
                if (lp.Count > 0)
                {
                    string lat = "";
                    string lon = "";
                    string url = "";
                    for (int i = 0; i < lp.Count; i++)
                    {
                        allRunsheets += "'" + lp[i].RunsheetNumber.ToString() + "',";
                    }
                    allRunsheets = allRunsheets.Remove(allRunsheets.Length - 1, 1);

                    var coords = await pdb.GetRiderLatLongs(riderCode, StartDate, EndDate, branchcode);

                    foreach (var item in coords.Where(x => x.Lat > 0 && x.Long > 0).OrderBy(x => x.CreatedOn).GroupBy(x => x.CreatedOn.Date))
                    {
                        var list = item.ToList();
                        var i = 0;

                        while (i < list.Count)
                        {
                            GeoCoordinate pin1 = new GeoCoordinate(list[i].Lat, list[i].Long);
                            i += 1;
                            if (i < list.Count)
                            {
                                GeoCoordinate pin2 = new GeoCoordinate(list[i].Lat, list[i].Long);
                                sumdist.Add(pin1.GetDistanceTo(pin2));
                            }
                        }
                    }

                    suminKMs = (sumdist.Sum()) / 1000;
                    ViewBag.IsPartiallyOff = coords.Any(x => x.Lat == 0 && x.Long == 0);
                }

                response.list = lp;
                string formattedkm = String.Format("{0:0.00}", suminKMs);
                response.distanceKm = formattedkm;
                pdb.Close();
                return PartialView("getDeliveryByRider", response);
            }
            catch (Exception er)
            {
                ViewBag.IsPartiallyOff = false;
                pdb.Close();
                return PartialView("getDeliveryByRider", new GetDeliveryDataWithDistance
                {
                    distanceKm = "",
                    list = new List<GetDeliveryData>()
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetDeliveryByRunsheet(string RunsheetID)
        {
            var superUser = ConfigurationManager.AppSettings["superusers"].Split(',').ToList().Any(x => x == Session["U_ID"].ToString());
            var branchcode = Session["BRANCHCODE"].ToString();
            List<double> sumdist = new List<double>();

            var rs = await dbrepo.GetRunsheetData(branchcode, RunsheetID);

            rs.ForEach(x =>
            {
                x.DBOnStr = x.DBOn.HasValue ? x.DBOn.Value.ToString("dd-MMM-yyyy HH:mm") : "";
            });

            var runsheetDate = rs.FirstOrDefault()?.RunsheetDate;

            if (superUser)
            {
                ViewBag.Edit = true;
            }
            else
            {
                if (runsheetDate.HasValue)
                {
                    if (runsheetDate.Value.Date < DateTime.Now.Date)
                    {
                        ViewBag.Edit = false;
                    }
                    else
                    {
                        ViewBag.Edit = true;
                    }
                }
            }

            var k = 0;

            while (k < rs.Count)
            {
                var pin1 = new GeoCoordinate(rs[k].Lat, rs[k].Long);
                k += 1;
                if (k < rs.Count)
                {
                    var pin2 = new GeoCoordinate(rs[k].Lat, rs[k].Long);
                    sumdist.Add(pin1.GetDistanceTo(pin2));
                }
            }

            var sum = (sumdist.Sum()) / 1000;
            var formattedkm = String.Format("{0:0.00}", sum);
            ViewBag.Reasons = await dbrepo.GetReasons();
            ViewBag.Relations = await dbrepo.GetRelations();
            ViewBag.RunsheetID = RunsheetID;
            ViewBag.DistanceKm = formattedkm;
            return PartialView("_GetRunsheetConsignments", rs);
        }

        [HttpPost]
        public async Task<ActionResult> AddMethod(int Consign_Id, bool verify, string comments)
        {
            Delivery_Child_details model = new Delivery_Child_details();
            model.Consign_id = Consign_Id;
            model.verify = verify;
            model.comments = comments;

            await pd.OpenAsync();
            var rs = await pd.Update(model);
            pd.Close();
            return Json(new { sts = rs ? 1 : 0, msg = rs ? "Updated Successfully" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetReasonPoints(string riderCode, string StartDate, string EndDate)
        {
            try
            {
                await pd.OpenAsync();
                var branchcode = Session["BRANCHCODE"].ToString();
                var rs = await pd.TotalReasonPoints(riderCode, StartDate, EndDate, branchcode);
                pd.Close();
                return Json(new { dataPoints = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { dataPoints = (object)null });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SubmitVerification(int Id, bool IsVerify, string Comment)
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await dbrepo.UpdateVerification(Id, IsVerify, Comment, u.Uid);
            return Json(new { sts = rs ? 1 : 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCN(int Id, string Reason)
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await dbrepo.UpdateReason(Id, Reason, u.Uid);
            return Json(new { sts = rs ? 1 : 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateRelationAndReceiver(int Id, string Relation, string Receiver)
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await dbrepo.UpdateRelationAndReceiver(Id, Relation, Receiver, u.Uid);
            return Json(new { sts = rs ? 1 : 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> VerifyAll(List<VerifyAllModel> model)
        {
            var u = Session["UserInfo"] as UserModel;
            var rs = await dbrepo.VerifyAll(model, u.Uid);
            return Json(new { sts = rs ? 1 : 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}