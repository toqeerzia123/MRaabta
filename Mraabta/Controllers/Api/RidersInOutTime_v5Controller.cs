using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MRaabta.Repo.Api;

namespace MRaabta.Controllers.Api
{
    public class RidersInOutTime_v5Controller : ApiController
    {
        RidersInOutTimeRepo_v5 repo;
        public RidersInOutTime_v5Controller()
        {
            repo = new RidersInOutTimeRepo_v5();
        }

        // GET: RidersOutTime
        [HttpPost, ActionName("InsertCourierOutTime")]
        public async Task<HttpResponseMessage> InsertCourierOutTime(String CourierTime,
            char Type, String UserId)
        {
            string message = "";
            bool isSuccess = false;
            try
            {

                if (CourierTime.ToString() != "" && ( Type == 'i' || Type == 'o' ) && UserId.ToString()!= "" )
                {
                    var rs = await repo.InsertData(Type, DateTime.Parse(CourierTime), Convert.ToInt64(UserId));
                    if (rs)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            isSuccess = true,
                            message = "Success"
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new
                        {
                            isSuccess = false,
                            message = "Unsuccessful"
                        });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        isSuccess = false,
                        message = "Incorrect values of parameters"
                    });
                }
               
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}