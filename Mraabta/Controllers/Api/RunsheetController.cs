using MRaabta.Models.Api;
using MRaabta.Repo.Api;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MRaabta.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RunsheetController : ApiController
    {
        RunsheetDB rundb = new RunsheetDB();

        [HttpGet]
        public RunsheetListOnlyNumberResponse GetRunsheetFromRoute(String routeCode, String branchcode)
        {
            RunsheetListOnlyNumberResponse response = new RunsheetListOnlyNumberResponse();
            try
            {
                List<RunsheetListOnlyNumber> runsheetList = new List<RunsheetListOnlyNumber>();

                runsheetList = rundb.GetRunsheetFromRouteCode(routeCode, branchcode);
                if (runsheetList.Count > 0)
                {
                    response.isSuccess = true;
                    response.Message = "Success";
                    response.RunsheetList = runsheetList;
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "No Data";
                    response.RunsheetList = runsheetList;
                    return response;
                }

            }
            catch (Exception er)
            {

                response.isSuccess = false;
                response.Message = "No Data";
                response.RunsheetList = null;
                return response;
            }
        }

        [HttpGet]
        public runsheetDeliveryDataResponse GetDeliveryDataFromRunsheetNumber(string Runsheet)
        {
            runsheetDeliveryDataResponse response = new runsheetDeliveryDataResponse();
            try
            {
                List<runsheetDeliveryData> runsheetList = new List<runsheetDeliveryData>();
                runsheetList = rundb.GetDeliveryDataFromRunsheetNumber(Runsheet);

                if (runsheetList.Count > 0)
                {
                    response.isSuccess = true;
                    response.Message = "Success";
                    response.deliverydataList = runsheetList;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "No Data";
                    response.deliverydataList = runsheetList;
                }
                return response;
            }
            catch (Exception er)
            {
                response.isSuccess = false;
                response.Message = "No Data";
                response.deliverydataList = null;
                return response;
            }
        }
    }
}
