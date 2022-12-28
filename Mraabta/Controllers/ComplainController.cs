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
    public class ComplainController : Controller
    {
        ComplainRepo repo;
        public ComplainController()
        {
            repo = new ComplainRepo();
        }
        // GET: Complain
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Index()
        {
            var u = Session["UserInfo"] as UserModel;
            await repo.OpenAsync();

            var branches = await repo.FilteredBranches(u.Uid);
            var zones = await repo.Zones(u.Uid);
            var esclevel = repo.GetUserLevel(u.Uid);

            var cnlengths = await repo.GetConsignmentsLength();
            ViewBag.CNLengths = cnlengths.Select(x => new { x.Product, x.Prefix, x.PrefixLength, x.Length }).ToList();
            ViewBag.Branches = branches.Select(x => new SelectListItem { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
            ViewBag.Zones = zones.Select(x => new SelectListItem { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
            //ViewBag.EscLevel = esclevel.Select(x => new SelectListItem { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
            ComplainModel model = new ComplainModel();
            model.Zones = zones; model.Branches = branches;
            model.Zone = u.ZoneCode; model.Branch = u.BranchCode; model.Escalation = esclevel;
            var openComplains = await repo.ComplainDetail(model, true);
            ViewBag.ComplainData = new { sts = 1, data = openComplains };
            repo.Close();
            return View();
        }
        public async Task<ActionResult> GetComplainDetail(ComplainModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;

                var branches = await repo.FilteredBranches(u.Uid);
                var zones = await repo.Zones(u.Uid);

                if (model.Branch != null)
                {
                    model.Branches = branches.Where(c => c.Value.Equals(model.Branch)).Select(x => new DropDownModel { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
                }
                else
                {
                    model.Branches = branches.Select(x => new DropDownModel { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
                }

                if (model.Zone != "000")
                {
                    model.Zones = zones.Where(c => c.Value.Equals(model.Zone)).Select(x => new DropDownModel { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
                }
                else
                {
                    model.Zones = zones.Select(x => new DropDownModel { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
                }

                var rs = await repo.ComplainDetail(model, false);
                if (rs != null)
                {
                    return Json(new { sts = 1, data = rs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 2, msg = "No Data Found !! !!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetBranch(string Zone)
        {
            var u = Session["UserInfo"] as UserModel;
            var dest = await repo.GetBranches(Zone, u.Uid);
            var destdropdown = dest.Select(x => new SelectListItem { Text = x.Text.Trim(), Value = x.Value.Trim() }).ToList();
            return Json(new { destdropdown }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet, SkipFilter]
        public async Task<ActionResult> Details(string CN, int ReqId)
        {
            try
            {
                var cnstatus = repo.CurrentTrackingStatus(CN);
                var complainhistorys = await repo.ComplainHistory(CN, ReqId);
                var complainhistory = complainhistorys.ToList().Select(c => new
                {
                    AgentType = c.AgentType,
                    Name = c.Name,
                    Branch = c.Branch,
                    CreatedTime = c.CreatedTime,
                    Message = c.Message,
                    MessageType = c.MessageType
                });
                var cndetail = await repo.ComplainTicketDetail(CN, ReqId);
                var rs = cndetail.ToList().Select(x => new
                {
                    Launch_By = x.LaunchBy,
                    ConsignmentNum = x.ConsignmentNum,
                    Ticket_Id = x.Ticket_Id,
                    FinalResponse = x.FinalResponse,
                    EscalationLevel = x.EsalationLevel,
                    ticketStatus = x.ticketStatus,
                    RequestType = x.Request_Type,
                    RequestNature = x.RequestNature,
                    OrderRefNo = x.orderRefNo,
                    CreatedOn = x.CreatedOn,
                    Address = x.Address,
                    ConsignmentStatus = cnstatus == null ? " " : cnstatus.CNStatus
                }).FirstOrDefault();

                ViewBag.ComplainData = new { sts = 1, data = rs, details = complainhistory, Status = rs.ticketStatus };
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> UploadFile(string CN, int ReqId)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;

                HttpPostedFileBase file = Request.Files[0];
                var fileext = file.FileName.Split('.');

                var oldcomplainhistorys = await repo.ComplainHistory(CN, ReqId);

                var oldcomplainhistory = oldcomplainhistorys.ToList().Select(c => new
                {
                    AgentType = c.AgentType,
                    Name = c.Name,
                    Branch = c.Branch,
                    CreatedTime = c.CreatedTime,
                    Message = c.Message,
                    MessageType = c.MessageType
                });

                if (!fileext[1].Equals("exe") && !fileext[1].Equals("zip"))
                {
                    string _File = ReqId + "." + fileext[1];
                    //string _File1 = ReqId + ".pdf";
                    var IsSuccess = await repo.AddRemarks(CN, u, ReqId, null, null, _File);

                    var complainhistorys = await repo.ComplainHistory(CN, ReqId);

                    var complainhistory = complainhistorys.ToList().Select(c => new
                    {
                        AgentType = c.AgentType,
                        Name = c.Name,
                        Branch = c.Branch,
                        CreatedTime = c.CreatedTime,
                        Message = c.Message,
                        MessageType = c.MessageType
                    });
                    if (IsSuccess > 0)
                    {

                        file.SaveAs(Server.MapPath("~/Documents/Complain/") + IsSuccess + "_" + _File);

                        return Json(new { sts = 1, details = complainhistory }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { sts = 2, msg = "Error !! Request Not Sent", details = oldcomplainhistory }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    return Json(new { sts = 2, msg = "File Format Not Allowed !!", details = oldcomplainhistory }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> Update(string CN, int ReqId, string Remarks, bool FinalResponse)
        {
            var u = Session["UserInfo"] as UserModel;
            var IsSuccess = await repo.AddRemarks(CN, u, ReqId, Remarks, FinalResponse);

            if (IsSuccess > 0)
            {
                var complainhistorys = await repo.ComplainHistory(CN, ReqId);
                var complainhistory = complainhistorys.ToList().Select(c => new
                {
                    AgentType = c.AgentType,
                    Name = c.Name,
                    Branch = c.Branch,
                    CreatedTime = c.CreatedTime,
                    Message = c.Message,
                    MessageType = c.MessageType
                });
                return Json(new { sts = 1, details = complainhistory }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { sts = 2, msg = "Error !! Request Not Sent" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}