using MRaabta.App_Start;
using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class ManifestController : Controller
    {
        ManifestRepo repo;
        public ManifestController()
        {
            repo = new ManifestRepo();
        }

        #region Manifest Page
        public async Task<ActionResult> Index(string id)
        {
            await repo.OpenAsync();
            var cnlengths = await repo.GetConsignmentsLength();
            ViewBag.Branches = await repo.Branches(true);
            ViewBag.CNLengths = cnlengths.Select(x => new { x.Product, x.Prefix, x.PrefixLength, x.Length }).ToList();
            ViewBag.Products = await repo.NewProducts();

            if (!string.IsNullOrEmpty(id))
            {
                var rs = await repo.ManifestData(id);
                var data = new
                {
                    ManifestNo = rs.ManifestNo,
                    Destination = rs.DestinationId,
                    Service = rs.ServiceTypeId,
                    Product = rs.ProductId,
                    SHS = rs.SHS,
                    TotalWeight = double.Parse(rs.TotalWeight),
                    TotalPcs = rs.TotalPcs,
                    IsEdit = true,
                    Details = rs.ManifestDetail.Select(x => new
                    {
                        x.CN,
                        x.Origin,
                        x.Destination,
                        x.ServiceType,
                        x.Weight,
                        x.Pcs,
                        x.Remarks,
                        IsOld = true
                    }).ToList()
                };

                var services = await repo.NewServices(rs.ProductId);
                ViewBag.Data = new { id = id, data = data, services };
            }
            else
            {
                ViewBag.Data = new { id = "0" };
            }

            repo.Close();

            return View();
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
        public async Task<ActionResult> GetCNInfo(string cn)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetCN(cn, int.Parse(u.BranchCode), "Manifest");
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
                            ServiceType = rs.ServiceType,
                            IsCod = rs.IsCod,
                            Weight = rs.Weight,
                            Pcs = rs.Pcs,
                            Remarks = "",
                            IsOld = false
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
        public async Task<ActionResult> Save(ManifestModel model)
        {
            try
            {
                (bool isok, string msg) rs;
                var u = Session["UserInfo"] as UserModel;

                if (!model.IsEdit)
                {
                    rs = await repo.Save(model, u);
                }
                else
                {
                    rs = await repo.Update(model, u);
                }

                return Json(new { sts = rs.isok ? 0 : 1, response = rs.msg });
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, response = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult> IsManifestExists(string manifestno)
        {
            try
            {
                var rs = await repo.IsManifestExists(manifestno);
                return Json(new { sts = !rs ? 0 : 1, msg = !rs ? "" : "Manifest Already Exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SearchPage

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetManifests(DateTime date)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetManifests(date, u.BranchCode);
                var data = rs.Select(x => new
                {
                    ManifestNo = x.ManifestNo,
                    ServiceType = x.ServiceType,
                    Product = x.Product,
                    SHS = x.SHS,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    IsDemanifested = x.IsDemanifested,
                    CreatedBy = x.CreatedBy
                }).ToList();
                return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Print

        [SkipFilter]
        public async Task<ActionResult> Print(string id)
        {
            var rs = await repo.ManifestData(id);
            return View(rs);
        }
        #endregion
    }
}