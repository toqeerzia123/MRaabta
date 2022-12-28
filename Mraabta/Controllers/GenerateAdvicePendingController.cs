using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class GenerateAdvicePendingController : Controller
    {
        GenerateAdvicePendingRepo repo;
        SmsRepo smsrepo;
        public GenerateAdvicePendingController()
        {
            repo = new GenerateAdvicePendingRepo();
            smsrepo = new SmsRepo();
        }

        [HttpGet]
        public async Task<ActionResult> Index(string cn = "")
        {
            try
            {
                ViewBag.CN = cn;
                await repo.OpenAsync();
                ViewBag.Reasons = await repo.Reasons();
                ViewBag.PhoneStatus = await repo.PhoneStatus();
                ViewBag.Status = await repo.Status();
                ViewBag.Reattempts = await repo.Reattempts();
                repo.Close();
                return View();
            }
            catch (Exception ex)
            {
                repo.Close();
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetNotes(int rid)
        {
            try
            {
                var rs = await repo.ReasonNotes(rid);
                return Json(new { sts = 0, data = rs }, JsonRequestBehavior.AllowGet);
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
                bool showPendingInfo = true;
                string msg = null;

                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.GetCNInfo(cn);
                if (rs.cndata != null)
                {
                    UserType utype = UserType.None;

                    var cnInfo = new
                    {
                        CN = rs.cndata.CN,
                        OriginId = rs.cndata.OriginId,
                        Origin = rs.cndata.Origin,
                        DestinationId = rs.cndata.DestinationId,
                        Destination = rs.cndata.Destination,
                        AccountNo = rs.cndata.AccountNo,
                        Consigner = rs.cndata.Consigner,
                        BookingDate = rs.cndata.BookingDate,
                        Consignee = rs.cndata.Consignee,
                        ConsigneePhoneNo = rs.cndata.ConsigneePhoneNo,
                        ServiceType = rs.cndata.ServiceType,
                        ConsigneeAddress = rs.cndata.ConsigneeAddress,
                        CodAmount = rs.cndata.CodAmount,
                        CNStatus = rs.cndata.CNStatus,
                        RSBranch = rs.cndata.RSBranch,
                        NCIStatus = rs.cndata.NCIStatus,
                        NCIStatusName = rs.cndata.NCIStatusName,
                        IsVoid = rs.cndata.IsVoid,
                        TicketNo = rs.cndata.TicketNo,
                        TotalTickets = rs.cndata.TotalTickets,
                        ticketClosingDate = GetTicketClosingDate("17:00")
                    };

                    if (!cnInfo.IsVoid)
                    {
                        if (cnInfo.CNStatus == "59" || cnInfo.NCIStatus == 2)
                        {
                            showPendingInfo = false;
                            msg = "CN already RTS";
                        }
                        else if (cnInfo.CNStatus == "123")
                        {
                            showPendingInfo = false;
                            msg = "CN already Delivered";
                        }
                        else
                        {
                            var iscod = cn[0] == '5' && cn.Length == 15;

                            if (iscod)
                            {
                                if (u.Profile == 2418)
                                    utype = UserType.CXM;
                                else if (u.BranchCode == cnInfo.RSBranch.ToString())
                                {
                                    utype = UserType.Coordinator;
                                }
                                else if (u.Profile == 2619)
                                    utype = UserType.Project;
                            }
                            else
                            {
                                if (u.Profile == 2418)
                                    utype = UserType.CXM;
                                else if (u.BranchCode == cnInfo.RSBranch.ToString())
                                {
                                    utype = UserType.Coordinator;
                                }
                                else if (u.BranchCode == cnInfo.OriginId.ToString())
                                {
                                    utype = UserType.Project;
                                }
                            }

                            if (utype == UserType.None)
                            {
                                showPendingInfo = false;
                                msg = "You are not authorized to do this operation.";
                            }
                            else if ((utype == UserType.CXM || utype == UserType.Project) && cnInfo.NCIStatus != 1)
                            {
                                showPendingInfo = false;
                                msg = "No open ticket found.";
                            }
                            else if (utype == UserType.Coordinator && cnInfo.NCIStatus == 1)
                            {
                                showPendingInfo = false;
                                msg = "Ticket already opened.";
                            }
                            else if (utype == UserType.Coordinator && cnInfo.TotalTickets >= 3)
                            {
                                showPendingInfo = false;
                                msg = "Max ticket limit exceeds.";
                            }
                        }
                    }
                    else
                    {
                        showPendingInfo = false;
                        msg = "CN is void";
                    }

                    var data = new
                    {
                        cnInfo = cnInfo,
                        role = utype,
                        showPendingInfo = showPendingInfo,
                        msg = msg,
                        logData = (rs.logdata != null && rs.logdata.Any()) ? rs.logdata.Select(x => new
                        {
                            LogStatus = x.LogStatus,
                            TicketNo = x.TicketNo,
                            Reason = x.Reason,
                            Notes = x.Notes,
                            CallStatus = x.CallStatus,
                            CallTime = x.CallTime,
                            NCIStatus = x.NCIStatus,
                            Reattempt = x.Reattempt,
                            Comment = x.Comment,
                            Consignee = x.Consignee,
                            ConsigneeCell = x.ConsigneeCell,
                            ConsigneeAddress = x.ConsigneeAddress,
                            CreatedBy = x.CreatedBy,
                            CreatedOn = x.CreatedOn
                        }).ToList() : null
                    };

                    return Json(new { sts = 0, data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sts = 1, msg = "Invalid Consignment No" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertTicket(TicketModel model)
        {
            try
            {
                var u = Session["UserInfo"] as UserModel;
                var rs = await repo.InsertTicket(model, u);
                if (rs.isOk && model.UserType == UserType.Coordinator)
                {
                    var phoneNo = model.ConsigneeCell.Replace("-", "");
                    var reason = await repo.GetReasonById(model.Reason);
                    string msg = "";
                    if (new List<string> { "1C17", "1P47" }.Contains(model.AccountNo))
                    {
                        msg = $@"Dear {model.Consignee}, Thank you for choosing M&P. Your driving license with CN# {model.CN} is pending due to {reason}. Kindly collect it from our {u.BranchName} during 09:00 to 17:00 Hrs.";
                    }
                    else
                    {
                        msg = $@"Dear {model.Consignee}, Thank you for choosing M&P. Your shipment with CN# {model.CN} booked from {model.Shippper} is pending for delivery as {reason}. Kindly contact our Customer Service at 021-111-202-202.";
                    }
                    var apiresponse = await smsrepo.SendSms(phoneNo, msg);
                    var query = "";
                    if (apiresponse.exeption == null)
                    {
                        var xml = apiresponse.doc;
                        if (xml != null)
                        {
                            var errorno = int.Parse(xml.Root.Element("errorno").Value);
                            var action = xml.Root.Element("action")?.Value;
                            var description = xml.Root.Element("description")?.Value;
                            var data = xml.Root.Element("data")?.Element("acceptreport");
                            var statusmessage = data?.Element("statusmessage")?.Value;
                            var messageid = data?.Element("messageid")?.Value;
                            query = $@"insert into SmsLogs (CN,PhoneNo,Msg,MsgId,StatusCode,Status,CreatedBy)
                                        values('{model.CN ?? ""}','{phoneNo}','{msg}',{messageid ?? "0"},{errorno},'{statusmessage ?? description}',0);";
                        }
                        else
                        {
                            query = $@"insert into SmsLogs (CN,PhoneNo,Msg,MsgId,StatusCode,Status,CreatedBy)
                                        values('{model.CN ?? ""}','{phoneNo}','{msg}', 0, 0,'Response is null',0);";
                        }
                    }
                    else
                    {
                        query = $@"insert into SmsLogs (CN,PhoneNo,Msg,MsgId,StatusCode,Status,CreatedBy)
                                values('{model.CN ?? ""}','{phoneNo}','{msg}', 0, 0,'{apiresponse.exeption}',0);";
                    }
                    var path = Server.MapPath("~/Quartz.txt");
                    await smsrepo.Log(query, path);
                }

                return Json(new { sts = rs.isOk ? 0 : 1, msg = rs.response }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string GetTicketClosingDate(string time)
        {
            var dt = DateTime.Now.AddHours(24);
            if (dt.TimeOfDay > TimeSpan.Parse(time) || dt.DayOfWeek == DayOfWeek.Sunday)
            {
                dt = dt.AddDays(1);
            }
            return $"{dt.ToString("dd-MMM-yyyy")} {time}";
        }
    }
}