using MRaabta.Models.Api;
using MRaabta.Repo.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MRaabta.Controllers.Api
{
    public class PRASyncController : ApiController
    {
        PRARepo repo;
        public PRASyncController()
        {
            repo = new PRARepo();
        }

        [HttpGet, ActionName("Sync")]
        public async Task<HttpResponseMessage> Sync()
        {
            try
            {
                await repo.OpenAsync();
                var list = await repo.GetData();
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        //await Task.Delay(1000);
                        try
                        {
                            var praModel = @"{
                            ""POSID"": 131515,
                            ""USIN"": ""@USIN"",
                            ""DateTime"": ""@DateTime"",
                            ""BuyerPNTN"": null,
                            ""BuyerCNIC"": ""@BuyerCNIC"",
                            ""BuyerName"": ""@BuyerName"",
                            ""BuyerPhoneNumber"": ""@BuyerPhoneNumber"",
                            ""TotalBillAmount"": @TotalBillAmount,
                            ""TotalQuantity"": @TotalQuantity,
                            ""TotalSaleValue"": @TotalSaleValue,
                            ""TotalTaxCharged"": @TotalTaxCharged,
                            ""Discount"": 0.0,
                            ""FurtherTax"": 0.0,
                            ""PaymentMode"": 1,
                            ""RefUSIN"": null,
                            ""InvoiceType"": 1,
                            ""Items"": [
                                {
                                    ""ItemCode"": ""@ItemCode"",
                                    ""ItemName"": ""@ItemName"",
                                    ""Quantity"": @Quantity,
                                    ""PCTCode"": ""00000000"",
                                    ""TaxRate"": @TaxRate,
                                    ""SaleValue"": @SaleValue,
                                    ""TotalAmount"": @TotalAmount,
                                    ""TaxCharged"": @TaxCharged,
                                    ""Discount"": 0.0,
                                    ""FurtherTax"": 0.0,
                                    ""InvoiceType"": 1,
                                    ""RefUSIN"": null
                                }
                            ]
                        }";

                            praModel = praModel.Replace("@USIN", item.CN);
                            praModel = praModel.Replace("@DateTime", item.AccountReceivingDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            praModel = praModel.Replace("@BuyerCNIC", item.BuyerCNIC);
                            praModel = praModel.Replace("@BuyerName", item.BuyerName);
                            praModel = praModel.Replace("@BuyerPhoneNumber", item.BuyerPhoneNumber);
                            praModel = praModel.Replace("@TotalBillAmount", item.TotalBillAmount.ToString());
                            praModel = praModel.Replace("@TotalQuantity", item.TotalQuantity.ToString());
                            praModel = praModel.Replace("@TotalSaleValue", item.TotalSaleValue.ToString());
                            praModel = praModel.Replace("@TotalTaxCharged", item.TotalTaxCharged.ToString());
                            praModel = praModel.Replace("@ItemCode", item.Service);
                            praModel = praModel.Replace("@ItemName", item.Service);
                            praModel = praModel.Replace("@Quantity", item.TotalQuantity.ToString());
                            praModel = praModel.Replace("@TaxRate", item.TaxRate.ToString());
                            praModel = praModel.Replace("@SaleValue", item.TotalSaleValue.ToString());
                            praModel = praModel.Replace("@TotalAmount", item.TotalBillAmount.ToString());
                            praModel = praModel.Replace("@TaxCharged", item.TotalTaxCharged.ToString());

                            //For Sandbox
                            //var response = await repo.PRACall("https://ims.pral.com.pk/ims/sandbox/api/Live/PostData", praModel);
                            //For Production
                            var response = await repo.PRACall("https://ims.pral.com.pk/ims/production/api/Live/PostData", praModel);
                            if (response != null)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var json = await response.Content.ReadAsStringAsync();
                                    var rs = JsonConvert.DeserializeObject<PRAResponseModel>(json);
                                    await repo.UpdateCN(item.CN, rs.InvoiceNumber, rs.Code, rs.Response);
                                }
                                else
                                {
                                    var json = await response.Content.ReadAsStringAsync();
                                    await repo.UpdateCN(item.CN, null, null, json);
                                }
                            }
                            else
                            {
                                await repo.UpdateCN(item.CN, null, null, "Response is Null");
                            }
                        }
                        catch (Exception ex)
                        {
                            await repo.UpdateCN(item.CN, null, null, ex.Message);
                        }
                    }

                    repo.Close();
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    repo.Close();
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "No data found");
                }
            }
            catch (Exception ex)
            {
                repo.Close();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}