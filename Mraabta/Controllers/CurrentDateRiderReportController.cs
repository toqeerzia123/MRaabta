using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class CurrentDateRiderReportController : Controller
    {
        CurrentDateRiderDB rrd;
        public CurrentDateRiderReportController()
        {
            rrd = new CurrentDateRiderDB();
        }
        CurrentDateRiderDB rrdd = new CurrentDateRiderDB();
        LocationDB ldb = new LocationDB();
        RiderDB rdb = new RiderDB();
        // GET: CurrentDateRiderReport
        public async Task<ActionResult> Index()
        {
            try
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                await rrd.OpenAsync();
                string SDate = DateTime.Now.Date.ToShortDateString();
                string EDate = DateTime.Now.Date.ToShortDateString();
                var rs = await rrd.GetRiders(SDate, EDate, branchcode);
                ViewBag.Riders = rs.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }

        }

        public async Task<ActionResult> ViewRiders(string SDate, string EDate)
        {
            try
            {
                string branchcode = Session["BRANCHCODE"].ToString();
                await rrd.OpenAsync();
                var riders = await rrd.GetRiders(SDate, EDate, branchcode);
                rrd.Close();
                //var rs = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                return Json(riders, JsonRequestBehavior.AllowGet);
            }

            catch (Exception er)
            {
                ViewBag.err = "error";
                //rs = null;
                return null;
            }
        }

        public ActionResult getRecordsByRider(string riderCode, string SDate, string EDate)
        {
            try
            {
                String branchcode = Session["BRANCHCODE"].ToString();

                var RiderReportModel = new RiderReportModel();
                List<RiderReportModel> lp = new List<RiderReportModel>();
                if (SDate != "" && EDate != "")
                {
                    DataTable dt = new DataTable();


                    dt = rrd.getRecords(riderCode, SDate, EDate, branchcode);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //string time = "";
                            RiderReportModel = new RiderReportModel
                            {

                                DownloadedRunsheet = Int64.Parse(dt.Rows[i]["DownloadedRunsheet"].ToString()),
                                TotalRunsheet = Convert.ToInt32(dt.Rows[i]["TotalRunsheet"]),
                                riderCode = Convert.ToInt32(dt.Rows[i]["riderCode"]),
                                RiderName = dt.Rows[i]["RiderName"].ToString(),
                                Location = dt.Rows[i]["Location"].ToString(),
                                RiderRoute = dt.Rows[i]["RiderRoute"].ToString(),
                                TotalCN = Convert.ToInt32(dt.Rows[i]["TotalCN"]),
                                TCNDownloaded = Convert.ToInt32(dt.Rows[i]["TCNDownloaded"]),
                                delivered = Convert.ToInt32(dt.Rows[i]["delivered"]),
                                undelivered = Convert.ToInt32(dt.Rows[i]["undelivered"]),
                                //Route = dt.Rows[i]["Route"].ToString(),
                                //Touchpoints = dt.Rows[i]["Touchpoints"].ToString(),
                                TotalTimeTaken = dt.Rows[i]["TotalTimeTaken"].ToString()
                            };
                            lp.Add(RiderReportModel);
                        }
                    }
                    else
                    {
                        TempData["error"] = "false";
                        ViewBag.Error = TempData["error"];
                    }
                }
                else
                {
                    TempData["error"] = "Kindly pass the parameters required";
                    ViewBag.Error = TempData["error"];
                    return View();
                }
                return View(lp);
            }
            catch (Exception er)
            {
                return Redirect("../Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetReasonPoints(string riderCode, string SDate, string EDate)
        {
            try
            {
                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await rrd.TotalReasonPoints(riderCode, SDate, EDate, branchcode);

                rrd.Close();

                return Json(new { dataPoints = rs });
            }
            catch (Exception er)
            {
                ViewBag.err = "error";
                return View();
            }

        }
    }
}