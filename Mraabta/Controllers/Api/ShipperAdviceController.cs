using MRaabta.Models.Api;
using MRaabta.Repo.Api;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MRaabta.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShipperAdviceController : ApiController
    {
        ShipperAdviceRepo repo;
        public ShipperAdviceController()
        {
            repo = new ShipperAdviceRepo();
        }

        [HttpPost, ActionName("GetInitData")]
        public async Task<HttpResponseMessage> GetInitData([FromBody] RequestModel model)
        {
            try
            {
                if (Request.Headers.Contains("token") && Request.Headers.GetValues("token").FirstOrDefault() == ConfigurationManager.AppSettings["apitoken"])
                {
                    var rs1 = await repo.GetInitData();
                    var rs2 = await repo.GetAdvices(model);
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            sts = 0,
                            calltracks = rs1.calltracks,
                            reattempts = rs1.reattempts,
                            advices = rs2,
                        });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new { sts = 1, msg = "Not Allowed" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { sts = 2, msg = ex.Message });
            }
        }

        [HttpPost, ActionName("GetAdvices")]
        public async Task<HttpResponseMessage> GetAdvices([FromBody] RequestModel model)
        {
            try
            {
                if (Request.Headers.Contains("token") && Request.Headers.GetValues("token").FirstOrDefault() == ConfigurationManager.AppSettings["apitoken"])
                {
                    var rs = await repo.GetAdvices(model);
                    return Request.CreateResponse(HttpStatusCode.OK, new { sts = 0, data = rs });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new { sts = 1, msg = "Not Allowed" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { sts = 2, msg = ex.Message });
            }
        }

        [HttpPost, ActionName("SaveAdvice")]
        public async Task<HttpResponseMessage> SaveAdvice([FromBody] ShipperAdviceDataModel model)
        {
            try
            {
                if (Request.Headers.Contains("token") && Request.Headers.GetValues("token").FirstOrDefault() == ConfigurationManager.AppSettings["apitoken"])
                {
                    if (model.Advice != 3)
                        model.ReattemptReason = 0;

                    var rs = await repo.SaveAdvice(model);
                    return Request.CreateResponse(HttpStatusCode.OK, new { sts = 0, msg = rs ? "Advice update successfully" : "Something went wrong" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new { sts = 1, msg = "Not Allowed" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { sts = 2, msg = ex.Message });
            }
        }

        [HttpGet, ActionName("TicketDetails")]
        public async Task<HttpResponseMessage> TicketDetails(string cn)
        {
            try
            {
                if (Request.Headers.Contains("token") && Request.Headers.GetValues("token").FirstOrDefault() == ConfigurationManager.AppSettings["apitoken"])
                {
                    var rs = await repo.TicketDetails(cn);
                    var data = rs.Select(x =>
                    new
                    {
                        Status = x.Status,
                        TicketNo = x.TicketNo,
                        Reason = x.Reason,
                        CallStatus = x.CallStatus,
                        CallTime = x.CallTime,
                        CallTrack = x.CallTrack,
                        Comments = x.Comments,
                        Consignee = x.Consignee,
                        ConsigneeCell = x.ConsigneeCell,
                        ConsigneeAddress = x.ConsigneeAddress
                    }).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { sts = 0, data = data });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new { sts = 1, msg = "Not Allowed" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { sts = 2, msg = ex.Message });
            }
        }
    }
}
