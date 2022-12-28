using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Threading.Tasks;
using Firebase.NET.Messages;
using Firebase.NET.Notifications;
using Firebase.NET;

namespace MRaabta.Controllers
{
    public class MyMnPNotificationHandlerController : Controller
    {

        SqlConnection orcl;
        public MyMnPNotificationHandlerController()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public ActionResult Index()
        {
            RiderPerformanceReportModel model = new RiderPerformanceReportModel();
            if (Session["U_ID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(model);
        }


        public async Task<JsonResult> GetCustomers()
        {
            try
            {
                string sql = @" select '0' as [Value], 'Select All' as [Text]
                                union 
                            SELECT Id AS Value,cau.Name TEXT FROM CustomerApp_Users cau  ";
                var Response = await orcl.QueryAsync<DropDownModel>(sql);
                //return ;
                return Json(new { Status = true, Data = Response.ToList() });
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<JsonResult> SendNotifications(List<string> Customers, string body, string title)
        {
            try
            {
                Customers = Customers.Where(x => x != "0").ToList();
                var ids = string.Join(",", Customers);
                string sql = @"  SELECT token FROM CustomerApp_Users cau WHERE cau.Id IN (" + ids + ")  ";
                var tokens = await orcl.QueryAsync<string>(sql);
    
                var requestMessage = new RequestMessage
                {
                    Body = {
                                    RegistrationIds = tokens.ToArray(),
                                    Notification = new CrossPlatformNotification
                                    {
                                        Title = title,
                                        Body = body
                                    } 
                           }
                };

                var pushService = new PushNotificationService("AAAAHmYXHaY:APA91bFM4ajb2lqLuzFEn0NXHB8K96nt56PtcYRO1PNxCaibnnMLFoM2dX5tkoBD3bdAvOzV2RYo4p6SsqZ9cvOj_bL-4rFwmGZZLWdKKC3fl7VlhTmljz_hodMtPG21mZW8VRhvoPpd");
                var responseMessage = await pushService.PushMessage(requestMessage);
                return Json(new { Status = true, Message = "Successfully sent notifications" });
            }
            catch (Exception ex)
            {
                string msg = "Error occured: " + ex.Message.ToString();
                msg = msg.Replace("\n", String.Empty);
                msg = msg.Replace("\r", String.Empty);
                ViewBag.ErrorMessage = msg;
                return Json(new { Status = false, Message = "Error " + ex.Message.ToString() });
            }
            finally
            {
                orcl.Close();
            }
        }

    }
}