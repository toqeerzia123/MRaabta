using MRaabta.Models.Api;
using MRaabta.Repo.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MRaabta.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConsignmentController : ApiController
    {
        ConsignmentDB consignDB = new ConsignmentDB();

        [HttpPost]
        public PickupInsertionUrlResponse InsertCnImages(ConsignmentNumberImage value)
        {
            PickupInsertionUrlResponse pir = new PickupInsertionUrlResponse();
            try
            {
                List<Images> listimages = new List<Images>();
                // for (int i = 0; i < value.Pickup_Child_List.Count; i++)

                String[] ConsignNumbers = value.image.consignmentNumber.Split(',');

                String type = value.image.TypeOfImage.ToLower();
                if (type.Equals("cn") || type.Equals("sign"))
                {
                    for (int i = 0; i < ConsignNumbers.Length; i++)
                    {
                        Tuple<bool, string> response;
                        string imageBase = value.image.imageCode;
                        string imageF5 = imageBase.Substring(0, 5);
                        string imageL5 = imageBase.Substring(imageBase.Length - 5);
                        if (imageF5.ToUpper() == "(MNP)" && imageL5.ToUpper() == "(MNP)")
                        {
                            imageBase = imageBase.Remove(0, 5);
                            imageBase = imageBase.Remove(imageBase.Length - 5, 5);

                            if (type.Equals("cn"))
                            {

                                response = consignDB.SaveImageToCN(imageBase, ConsignNumbers[i].ToString());
                                listimages.Add(new Images { isSuccess = response.Item1, ConsignmentNumber = ConsignNumbers[i].ToString(), Message = response.Item2 });

                            }
                            else if (type.Equals("sign"))
                            {

                                response = consignDB.SaveImageToSN(imageBase, ConsignNumbers[i].ToString());
                                listimages.Add(new Images { isSuccess = response.Item1, ConsignmentNumber = ConsignNumbers[i].ToString(), Message = response.Item2 });

                            }
                            else
                            {
                                response = new Tuple<bool, string>(false, "Error Saving Image!, please define type");
                                listimages.Add(new Images { isSuccess = false, ConsignmentNumber = ConsignNumbers[i].ToString(), Message = "Error Saving Image!, please define type" });

                            }

                        }
                        else
                        {
                            listimages.Add(new Images { isSuccess = false, ConsignmentNumber = ConsignNumbers[i].ToString() });
                            pir.isSuccess = false;
                            pir.message = "Image String not in proper format, Kindly concatenate (MNP) on both intial and end point";
                            pir.ListImages = listimages;
                            return pir;
                        }
                    }
                    pir.isSuccess = true;
                    pir.message = "Success";
                    pir.ListImages = listimages;
                    return pir;
                }
                else
                {
                    pir.isSuccess = false;
                    pir.message = "Please define type of image";
                    pir.ListImages = null;
                    return pir;
                }
            }
            catch (Exception er)
            {
                pir.isSuccess = false;
                pir.message = "Error saving images";
                pir.ListImages = null;
                return pir;
            }
        }

        [HttpPost]
        public async Task<ConsginmentStatusResponse> SendDeliveryData([FromBody] ConsginmentList ConsignmentObjects)
        {
            List<ConsignmentStatus> listConsignment = new List<ConsignmentStatus>();
            //get UserId from RiderCode if Needed
            AccountDB account_db = new AccountDB();

            string message = "";
            bool isSuccess = true;
            var consignStatus_ = new List<ConsignmentStatus>();
            ConsginmentStatusResponse response = new ConsginmentStatusResponse();
            try
            {
                Accounts acc_ = new Accounts();
                if (ConsignmentObjects.listOfconsignments.Count > 0)
                {
                    var uid = ConsignmentObjects.listOfconsignments.FirstOrDefault().createdBy;
                    await consignDB.UpdateIsActive(uid);
                    foreach (var item in ConsignmentObjects.listOfconsignments)
                    {
                        Tuple<bool, string> CheckingBusinessLogic = await consignDB.CheckValidConsignmentValues(item);
                        if (CheckingBusinessLogic.Item1)
                        {
                            listConsignment.Add(await consignDB.insertDB(item));
                            MailMessage message1 = new MailMessage();

                            string BCC01 = "software.trainee1@mulphilog.com";
                            message1.Bcc.Add(new MailAddress(BCC01));

                            message1.From = new MailAddress("no-reply@mulphilog.com");

                            message1.Subject = "Comfirmation Email";

                            message1.IsBodyHtml = true;

                            string text = "Dear, " + item.pickerName + "! <br />Your parcel of CN " + item.ConsignmentNumber + " against Runsheet No: " + item.runsheet + " is successfully delivered on the<br /> Location " + item.longitude + " and " + item.latitude + " by " + item.name;
                            message1.Body = text + "<br/>Thankyou for using our services. We look forward to provide you services in Future <br/><br/>" + "Your Service Provider," + "<br />" + "M&P Express Logistics";


                            SmtpClient mail = new SmtpClient();
                            mail.Port = int.Parse("587"); //25
                            mail.Host = "mail.mulphilog.com";
                            mail.EnableSsl = true;
                            mail.UseDefaultCredentials = true;
                            mail.Credentials = new System.Net.NetworkCredential("no-reply@mulphilog.com", "mpl@119");

                            //Add this line to bypass the certificate validation
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                            {
                                return true;
                            };
                            try
                            {
                                mail.Send(message1);
                                consignDB.MessageResponse(item);
                            }
                            catch (Exception Err)
                            {
                            }

                        }
                        else
                        {
                            listConsignment.Add(new ConsignmentStatus { consignmentNumber = item.ConsignmentNumber, isSuccess = false, Message = CheckingBusinessLogic.Item2 });
                            int insertedID = 0;
                            string param = Base64EncodingMethod(insertedID.ToString());

                        }
                    }
                    response.isSuccess = true;
                    response.Message = "Success";
                    response.listConsignment = listConsignment;
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.Message = "Error Inserting Data";
                    response.listConsignment = listConsignment;
                    return response;
                }
            }
            catch (Exception ee)
            {

                response.isSuccess = false;
                response.Message = "Error Adding Data!";
                return response;
            }
        }
        public static string Base64EncodingMethod(string Data)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(Data);
            string sReturnValues = System.Convert.ToBase64String(toEncodeAsBytes);
            return sReturnValues;
        }

        [HttpGet, ActionName("GetStatus")]
        public async Task<HttpResponseMessage> GetStatus()
        {
            try
            {
                var rs = await consignDB.GetStatus();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = rs.Select(x => new { Id = x.value, Name = x.name }).ToList() });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}