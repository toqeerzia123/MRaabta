using MRaabta.Models.Api;
using MRaabta.Repo.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MRaabta.Controllers.Api
{

    public class CODCNCashConfirmationController : ApiController
    {
        CODCNCashConfirmationRepo repo;
        public CODCNCashConfirmationController()
        {
            repo = new CODCNCashConfirmationRepo();
        }
        // GET: CODCNCashConfirmation
        [HttpPost, ActionName("CNsConfirmation")]
        public async Task<HttpResponseMessage> CNsConfirmation([FromBody] CODCNCashConfirmationModel model)
        {
            try
            {
                if (model.ID != "" && model.RiderCode != "" && model.Date != "")
                {
                    var rs = await repo.GetData(model);
                    if (rs != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            isSuccess = true,
                            message = "Success",
                            ID = model.ID,
                            RiderCode = model.RiderCode,
                            Date = model.Date,
                            Details = rs
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new
                        {
                            isSuccess = false,
                            message = "Unsuccessful",
                            ID = model.ID,
                            RiderCode = model.RiderCode,
                            Date = model.Date
                        });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        isSuccess = false,
                        message = "Parameters Mismatch"
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