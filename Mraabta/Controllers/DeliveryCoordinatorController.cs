using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace MRaabta.Controllers
{
    public class DeliveryCoordinatorController : Controller
    {
        DeliveryCordinatorDB pd;
        DebreifingAllRepo dbrepo;

        public DeliveryCoordinatorController()
        {
            pd = new DeliveryCordinatorDB();
            dbrepo = new DebreifingAllRepo();
        }

        // GET: DeliveryCordinator
        DeliveryCordinatorDB pdb = new DeliveryCordinatorDB();
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
                var rs = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                return Json(rs, JsonRequestBehavior.AllowGet);
            }

            catch (Exception er)
            {
                ViewBag.err = "error";
                //rs = null;
                return null;
            }
        }
        // GET: Delivery
        public async Task<ActionResult> getDeliveryByRider(string riderCode, string StartDate, string EndDate)
        {
            try
            {
                string allRunsheets = "";
                List<double> sumdist = new List<double>();
                double suminKMs = 0;

                await pdb.OpenAsync();

                var branchcode = Session["BRANCHCODE"].ToString();
                GetDeliveryDataWithDistance response = new GetDeliveryDataWithDistance();
                var GetDeliveryData = new GetDeliveryData();
                List<GetDeliveryData> lp = new List<GetDeliveryData>();

                lp = await pdb.getDelivery(riderCode, StartDate, EndDate, branchcode);
                if (lp.Count > 0)
                {
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
                else
                {
                    TempData["error"] = "No record found";
                    ViewBag.Error = TempData["error"];
                }
                response.list = lp;
                string formattedkm = String.Format("{0:0.00}", suminKMs);
                response.distanceKm = formattedkm;
                pdb.Close();
                return PartialView("getDeliveryByRider", response);
            }
            catch (Exception er)
            {
                pdb.Close();
                return PartialView("getDeliveryByRider", new GetDeliveryDataWithDistance
                {
                    distanceKm = "",
                    list = new List<GetDeliveryData>()
                });
            }
        }


        //public async Task<ActionResult> GetDeliveryByRunsheet(Int64 RunsheetID)
        //{
        //    GetDeliveryChildDataDistance response = new GetDeliveryChildDataDistance();

        //    try
        //    {
        //        double sum;
        //        List<double> sumdist = new List<double>();
        //        await pdb.OpenAsync();
        //        String branchcode = Session["BRANCHCODE"].ToString();
        //        //locationID = "3";
        //        //StartDate = "01-03-2019";
        //        //EndDate = "06-04-2019";
        //        Delivery_Child_details pcd = new Delivery_Child_details();
        //        DataTable dt = new DataTable();
        //        List<Delivery_Child_details> DetailsModelList = new List<Delivery_Child_details>();
        //        DetailsModelList = await pdb.getDeliveryDetails(RunsheetID, branchcode);

        //        for (int k = 0; k < DetailsModelList.Count; k++)
        //        {
        //            double lat = DetailsModelList[k].latitude;
        //            double longi = DetailsModelList[k].longitude;



        //            if (k == DetailsModelList.Count - 1) { }

        //            else
        //            {
        //                if (DetailsModelList[k].latitude != 0 && DetailsModelList[k + 1].latitude != 0)
        //                {
        //                    GeoCoordinate pin1 = new GeoCoordinate(DetailsModelList[k].latitude, DetailsModelList[k].longitude);
        //                    GeoCoordinate pin2 = new GeoCoordinate(DetailsModelList[k + 1].latitude, DetailsModelList[k + 1].longitude);
        //                    sumdist.Add(pin1.GetDistanceTo(pin2));
        //                }
        //            }
        //        }
        //        sum = (sumdist.Sum()) / 1000;

        //        string formattedkm = String.Format("{0:0.00}", sum);
        //        response.distanceKm = formattedkm;
        //        response.list = DetailsModelList;
        //        ViewBag.RunsheetID = RunsheetID;
        //        return View(response);
        //    }
        //    catch (Exception er)
        //    {
        //        return View();
        //    }
        //    finally
        //    {
        //        pdb.Close();
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult> GetDeliveryByRunsheet(string RunsheetID)
        {
            var branchcode = Session["BRANCHCODE"].ToString();
            List<double> sumdist = new List<double>();

            var rs = await dbrepo.GetRunsheetData(branchcode, RunsheetID);

            var runsheetDate = rs.FirstOrDefault()?.RunsheetDate;

            ViewBag.Edit = false;

            for (int k = 0; k < rs.Count; k++)
            {
                if (k < rs.Count - 1)
                {
                    double lat1 = rs[k].Lat;
                    double long1 = rs[k].Long;
                    var pin1 = new GeoCoordinate(rs[k].Lat, rs[k].Long);
                    var pin2 = new GeoCoordinate(rs[k + 1].Lat, rs[k + 1].Long);
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
        public async Task<ActionResult> GetReasonPoints(string riderCode, string StartDate, string EndDate)
        {
            try
            {
                await pdb.OpenAsync();
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await pd.TotalReasonPoints(riderCode, StartDate, EndDate, branchcode);
                pd.Close();
                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                ViewBag.err = "error";
                return View();
            }
            finally
            {
                pdb.Close();
            }

        }

        [HttpPost]
        public async Task<ActionResult> GetPoints(string riderCode, string StartDate, string EndDate)
        {
            try
            {
                await pdb.OpenAsync();
                var rs = await pd.TotalPoints(riderCode, StartDate, EndDate);
                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                ViewBag.err = "error";
                return View();
            }
            finally
            {
                pdb.Close();
            }

        }
    }
}