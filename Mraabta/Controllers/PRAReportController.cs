using MRaabta.Repo.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class PRAReportController : Controller
    {
        PRARepo repo;
        public PRAReportController()
        {
            repo = new PRARepo();
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.Zones = await repo.Zones();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetReport(int type, string zoneCode, DateTime dt)
        {
            try
            {
                if (type == 1)
                {
                    var rs = await repo.DifferenceInChargeAmountReport(zoneCode, dt);
                    var data = rs.Select(x => new
                    {
                        Region = x.Region,
                        Zone = x.Zone,
                        Branch = x.Branch,
                        AccoutReceivingDate = x.AccoutReceivingDate,
                        CN = x.CN,
                        Consigner = x.Consigner,
                        ConsignerNTN = x.ConsignerNTN,
                        DestZone = x.DestZone,
                        DestBranch = x.DestBranch,
                        Service = x.Service,
                        Weight = x.Weight,
                        Pcs = x.Pcs,
                        CNChargedAmount = x.CNChargedAmount,
                        PRAChargedAmount = x.PRAChargedAmount,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceType = x.InvoiceType
                    }).ToList();

                    var json = new JsonResult
                    {
                        MaxJsonLength = int.MaxValue,
                        Data = new { sts = 0, type = type, data = data },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return json;
                }
                else if (type == 2)
                {
                    var rs = await repo.TodayDataToBeShared();
                    var data = rs.Select(x => new
                    {
                        Region = x.Region,
                        Zone = x.Zone,
                        Branch = x.Branch,
                        AccoutReceivingDate = x.AccoutReceivingDate,
                        CN = x.CN,
                        Consigner = x.Consigner,
                        ConsignerNTN = x.ConsignerNTN,
                        DestZone = x.DestZone,
                        DestBranch = x.DestBranch,
                        Service = x.Service,
                        Weight = x.Weight,
                        Pcs = x.Pcs,
                        CNChargedAmount = x.CNChargedAmount,
                        PRAChargedAmount = x.PRAChargedAmount,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceType = x.InvoiceType
                    }).ToList();

                    var json = new JsonResult
                    {
                        MaxJsonLength = int.MaxValue,
                        Data = new { sts = 0, type = type, data = data },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return json;
                }
                else if (type == 3)
                {
                    var rs = await repo.RemainingData();
                    var data = rs.Select(x => new
                    {
                        Region = x.Region,
                        Zone = x.Zone,
                        Branch = x.Branch,
                        AccoutReceivingDate = x.AccoutReceivingDate,
                        CN = x.CN,
                        Consigner = x.Consigner,
                        ConsignerNTN = x.ConsignerNTN,
                        DestZone = x.DestZone,
                        DestBranch = x.DestBranch,
                        Service = x.Service,
                        Weight = x.Weight,
                        Pcs = x.Pcs,
                        CNChargedAmount = x.CNChargedAmount,
                        PRAChargedAmount = x.PRAChargedAmount,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceType = x.InvoiceType
                    }).ToList();

                    var json = new JsonResult
                    {
                        MaxJsonLength = int.MaxValue,
                        Data = new { sts = 0, type = type, data = data.Take(1000).ToList() },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return json;
                }
                else if (type == 4)
                {
                    var rs = await repo.FailedToSendData();
                    var data = rs.Select(x => new
                    {
                        Region = x.Region,
                        Zone = x.Zone,
                        Branch = x.Branch,
                        AccoutReceivingDate = x.AccoutReceivingDate,
                        CN = x.CN,
                        Consigner = x.Consigner,
                        ConsignerNTN = x.ConsignerNTN,
                        DestZone = x.DestZone,
                        DestBranch = x.DestBranch,
                        Service = x.Service,
                        Weight = x.Weight,
                        Pcs = x.Pcs,
                        CNChargedAmount = x.CNChargedAmount,
                        PRAChargedAmount = x.PRAChargedAmount,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceType = x.InvoiceType
                    }).ToList();

                    var json = new JsonResult
                    {
                        MaxJsonLength = int.MaxValue,
                        Data = new { sts = 0, type = type, data = data },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return json;
                }
                else if (type == 5)
                {
                    var rs = await repo.QualifiedButNotSend();
                    var data = rs.Select(x => new
                    {
                        Region = x.Region,
                        Zone = x.Zone,
                        Branch = x.Branch,
                        AccoutReceivingDate = x.AccoutReceivingDate,
                        CN = x.CN,
                        Consigner = x.Consigner,
                        ConsignerNTN = x.ConsignerNTN,
                        DestZone = x.DestZone,
                        DestBranch = x.DestBranch,
                        Service = x.Service,
                        Weight = x.Weight,
                        Pcs = x.Pcs,
                        CNChargedAmount = x.CNChargedAmount,
                        PRAChargedAmount = x.PRAChargedAmount,
                        InvoiceDate = x.InvoiceDate,
                        InvoiceNumber = x.InvoiceNumber,
                        InvoiceType = x.InvoiceType
                    }).ToList();

                    var json = new JsonResult
                    {
                        MaxJsonLength = int.MaxValue,
                        Data = new { sts = 0, type = type, data = data },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return json;
                }
                else if (type == 6)
                {
                    var rs = await repo.SummaryReport(dt.Year, dt.Month);
                    var data = rs.Select(x => new
                    {
                        Region = x.Region,
                        Zone = x.Zone,
                        Branch = x.Branch,
                        AccoutReceivingDate = x.AccoutReceivingDate,
                        InvoiceType = x.InvoiceType,
                        TotalBookedCNs = x.TotalBookedCNs,
                        InvoicedCns = x.InvoicedCns
                    }).ToList();

                    var json = new JsonResult
                    {
                        MaxJsonLength = int.MaxValue,
                        Data = new { sts = 0, type = type, data = data },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return json;
                }
                else
                {
                    return Json(new { sts = 1, type = type, msg = "Something went wrong" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, type = type, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}