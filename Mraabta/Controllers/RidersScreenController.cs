using MRaabta.Models;
using Dapper;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class RidersScreenController : Controller
    {
        SqlConnection orcl;
        public RidersScreenController()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        }
        // GET: RidersScreen
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RiderActivity()
        {
            try
            {

                String branchcode = Session["BRANCHCODE"].ToString();
                var rs = await orcl.QueryAsync<RidersActivity>($@"select 
                                                                USER_ID AS UserId, 
                                                                userName AS UserName,
                                                                riderCode AS RiderCode, 
                                                                isNULL(isActive,0) IsActive,
                                                                [Password],
                                                                [STATUS] as [Status],
                                                                (SELECT TOP 1 adcd.Battery FROM dbo.App_Delivery_ConsignmentData adcd WHERE adcd.created_by = USER_ID ORDER BY adcd.performed_on desc) AS Battery
                                                                from App_Users where branchCode = '{branchcode}' and STATUS = 1;", commandTimeout: int.MaxValue);
                return Json(rs.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Logout(int uid)
        {
            try
            {
                await orcl.OpenAsync();
                var rs = await orcl.ExecuteAsync($@"UPDATE App_Users SET IsActive = 0 where USER_ID = {uid} and IsActive = 1;");
                orcl.Close();
                return Json(new { type = rs > 0 ? 0 : 1, msg = rs > 0 ? "Logout Successfull" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = 1, msg = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus(int uid, bool sts)
        {
            try
            {
                await orcl.OpenAsync();
                var rs = await orcl.ExecuteAsync($@"update App_Users set STATUS = {(sts ? "1" : "0")} where USER_ID = {uid}");
                orcl.Close();
                return Json(new { type = rs > 0 ? 0 : 1, msg = rs > 0 ? $"User {(sts ? "Activated" : "Deactivated")}" : "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = 1, msg = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangePass(int uid, string pass)
        {
            try
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                await orcl.OpenAsync();
                var rs = await orcl.ExecuteAsync(@"update App_Users set [Password] = @pass where branchCode = @branchcode and [USER_ID] = @uid;", new { uid, pass, branchcode });
                orcl.Close();
                return Json(new { sts = rs > 0 ? 1 : 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                return Json(new { sts = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}