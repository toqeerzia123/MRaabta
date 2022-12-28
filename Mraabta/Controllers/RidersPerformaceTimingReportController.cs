using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class RidersPerformaceTimingReportController : Controller
    {
        // GET: RidersPerformaceTimingReport
        SqlConnection orcl;
        public RidersPerformaceTimingReportController()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
       
        public ActionResult Index()
        {
            RiderPerformanceReportModel model = new RiderPerformanceReportModel();
            if (Session["U_ID"] == null)
            {
                return RedirectToAction("Index", "Login");
            } 
            return View(model);
        }

        [HttpPost]

        public async Task<JsonResult> ViewRiders(DateTime SDate, DateTime EDate)
        {
            try
            {
                string BranchCode = Session["BRANCHCODE"].ToString();
                int TotalDays = (EDate - SDate).Days;
                if (TotalDays > 31 || TotalDays < 0)
                {
                    return Json(new { Status = false, Message = "Maximum allowed date is 31 days" });
                }
                await orcl.OpenAsync();
                var Riders = await GetRiders(SDate.ToString("yyyy-MM-dd"), EDate.ToString("yyyy-MM-dd"), BranchCode);
                return Json(new { Status = true, Message = "Success", Data = Riders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                orcl.Close();
            }
        }

        public async Task<List<DropDownModel>> GetRiders(string SDate, string EDate, string BranchCode)
        {
            try
            {
                string BranchCondition = "",BranchCondition2="";
                if (BranchCode.ToUpper()!="ALL") {
                    BranchCondition = " AND r.branchId = '" + BranchCode + "' ";
                    BranchCondition2 = " R.BRANCHCODE ='" + BranchCode + "' AND ";
                }
                string sql = @" select '0' as [Value], 'Select All' as [Text]
                                union
                      Select      DISTINCT Appdt.RIDERCODE as Value  ,
                            (select top 1 (r.firstName + ' '+ r.lastName) from Riders r
                            where r.riderCode = Appdt.riderCode " + BranchCondition + @" ) as Text
                            FROM   App_Delivery_ConsignmentData Appdt
                            inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                            WHERE " + BranchCondition2 + " cast(R.RUNSHEETDATE as date) between '" + SDate + "' and '" + EDate + "' ";
                var Response = await orcl.QueryAsync<DropDownModel>(sql);
                return Response.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<JsonResult> PerformanceReport(List<string> Riders, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                RiderPerformanceReportModel Data = new RiderPerformanceReportModel();
                Data.DataList = new List<RiderPerformanceDetailsModel>();
                int TotalDays = (EndDate - StartDate).Days;
                if (TotalDays > 31 || TotalDays < 0)
                {
                    return Json(new { Status = false, Message = "Maximum date is 31 days" });
                }
                var Branchcode = Session["BRANCHCODE"].ToString();
                Riders = Riders.Where(x => x != "0").ToList();
                await orcl.OpenAsync();
                var lp = await GetSummaryReport(Riders,StartDate, EndDate, Branchcode);

                foreach (var item in lp.Where(x => x.PerformedOn.HasValue).GroupBy(x => new { x.RiderCode, x.PerformedOn.Value.Date}))
                {
                    var totaltimetaken = (item.Select(x => x.First_RS_Download.Value).Min()- item.Select(x => x.PerformedOn.Value).Max());
                    Data.DataList.Add(new RiderPerformanceDetailsModel
                    {
                        RiderCode = item.Key.RiderCode,
                        RiderName = item.FirstOrDefault().RiderName,
                        TotalCNs = item.GroupBy(x => x.RS).Select(y => y.Select(z => z.CN).Distinct().Count()).Sum(),
                        CNDelivered = item.Where(x => x.StatusId == 1).Select(x => x.CN).Distinct().Count(),
                        CNUnDelivered = item.Where(x => x.StatusId == 2).Select(x => x.CN).Distinct().Count(),
                        CN_RTS = item.Where(x => x.StatusId == 3).Select(x => x.CN).Distinct().Count(),
                        TotalTimeOnRoute = totaltimetaken.ToString(@"hh\:mm\:ss"),
                        First_RS_DownloadString = String.Format("{0:G}", item.Select(x => x.First_RS_Download).Min()),
                        First_CN_Performed= String.Format("{0:G}", item.Select(x=>x.PerformedOn.Value).Min()), 
                        Last_CN_Performed= String.Format("{0:G}", item.Select(x=>x.PerformedOn.Value).Max()), 
                    });
                }
                Data.DataList = Data.DataList.OrderBy(a => a.RiderCode).ToList();
                return Json(new { Status = true, Message = "Success", Data = Data });
            }
            catch (Exception ex)
            {
                string msg = "Error occured: " + ex.Message.ToString();
                msg = msg.Replace("\n", String.Empty);
                msg = msg.Replace("\r", String.Empty);
                ViewBag.ErrorMessage = msg;
                return Json(new { Status = false, Message = "Error " + ex.Message.ToString() });
            }
            finally
            {
                orcl.Close();
            }   
        }


        private async Task<List<(string RS, string CN, string RiderCode, string RiderName, int StatusId, DateTime? PerformedOn, DateTime? First_RS_Download)>> GetSummaryReport(List<string> Rider, DateTime StartDate, DateTime EndDate,string Branchcode)
        {
            string BranchCondition = "";
            if (Branchcode.ToUpper()!="ALL")
            {
                BranchCondition = " R.branchCode =" + Branchcode+" AND";
            }
            StringBuilder rcsb = new StringBuilder();
            Rider.ForEach(x =>
            {
                rcsb.Append($"'{x}',");
            });
            var rcstr = rcsb.ToString().TrimEnd(',');

            string newSql = @" Select   ---Rider Performance Report 
                                        ---" + Session["U_NAME"].ToString() + @"
                             r.runsheetNumber as RS, rc.consignmentNumber as CN, ri.riderCode as RiderCode,
                            CONCAT(ri.firstName,' ',ri.lastName) as RiderName, isnull(appd.StatusId,0) as StatusId,appd.performed_on as PerformedOn,
							rf.fetchedAt First_RS_Download
                            from Runsheet r
                            inner join Riders ri on ri.riderCode = r.ridercode and ri.branchId = r.branchCode
                            inner join Branches br on br.branchCode = ri.branchId
                            inner join RunsheetConsignment rc on r.runsheetNumber = rc.runsheetNumber and r.branchCode = rc.branchcode
                            left join App_Delivery_RunsheetFetched rf on rf.runsheetNo = r.runsheetNumber
                            left join App_Delivery_ConsignmentData appd on appd.RunSheetNumber = rc.runsheetNumber and appd.ConsignmentNumber = rc.consignmentNumber
                            where " + BranchCondition + @"   cast(r.runsheetDate as date) between '" + StartDate.ToString("yyyy-MM-dd") + "' and '"+ EndDate.ToString("yyyy-MM-dd") + "' and  r.riderCode IN (" + rcstr + @")  
                              ORDER BY ri.riderCode,rf.fetchedAt  ";
            var rs = await orcl.QueryAsync<(string RS, string CN,string RiderCode, string RiderName, int StatusId,DateTime? PerformedOn, DateTime? First_RS_Download )>(newSql,commandTimeout:170);
            return rs.ToList();

        }
    }
}