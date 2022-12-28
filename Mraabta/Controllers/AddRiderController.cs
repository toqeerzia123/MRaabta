using System;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MRaabta.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace MRaabta.Controllers
{
    public class AddRiderController : Controller
    {
        SqlConnection con;
        public AddRiderController()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public async Task<ActionResult> Index(int id = 0)
        {
            var u = Session["UserInfo"] as UserModel;
            var a = await GetRiderHub(u.BranchCode);
            if (id > 0)
            {
                ViewBag.IsEdit = true;
                var model = await GetRiderData(id);
                if (a != null)
                {
                    if (a.Count() > 0)
                    {
                        ViewBag.HubId = new SelectList(await GetRiderHub(u.BranchCode), "Id", "HubName", model.HubId);
                    }
                }
                return View(model);
            }
            else
            {
                ViewBag.IsEdit = false;
                if (a != null)
                {
                    if (a.Count() > 0)
                    {
                        ViewBag.HubId = new SelectList(await GetRiderHub(u.BranchCode), "Id", "HubName");
                    }
                }
                return View(new RiderFormModel());
            }
        }

        [HttpPost]
        public async Task<ActionResult> Index(RiderFormModel model)
        {
            var u = Session["UserInfo"] as UserModel;
            if (model.UserId == null)
            {
                model.BranchCode = u.BranchCode;
                model.CreatedBy = u.Uid;
                var rs = await InsertRider(model);
                ViewBag.Msg = rs;
                ViewBag.IsEdit = false;
            }
            else
            {
                var rs = await UpdateRider(model);
                ViewBag.Msg = rs;
                ViewBag.IsEdit = true;
            }
            var a = await GetRiderHub(u.BranchCode);
            if (a != null)
            {
                if (a.Count() > 0)
                {
                    ViewBag.HubId = new SelectList(await GetRiderHub(u.BranchCode), "Id", "HubName", model.HubId);
                }
            }
            return View(new RiderFormModel());
        }

        [NonAction]
        public async Task<string> InsertRider(RiderFormModel model)
        {
            try
            {
                var query = "";
                if (model.HubId == 0)
                {
                    //var a = await GetRiderHub(model.BranchCode);
                    //if (a.Count() == 0)
                    //{
                    query = @"insert into App_Users (userName,PASSWORD,branchCode,roleID,createdBy,createdOn,STATUS,riderCode,isActive,IMEI1,IMEI2,SimSNO)
                                                    values(
                                                    @RiderName,
                                                    @Password,
                                                    @BranchCode,
                                                    1,
                                                    @CreatedBy,
                                                    getdate(),
                                                    1,
                                                    @RiderCode,
                                                    0,
                                                    @Imei1,
                                                    @Imei2,
                                                    @SimNO
                                                    );";
                    //}
                    //else
                    //{
                    //    model.HubId = a.First().Id;
                    //    query = @"insert into App_Users (userName,PASSWORD,branchCode,roleID,createdBy,createdOn,STATUS,riderCode,isActive,IMEI1,IMEI2,SimSNO, HubId)
                    //                                values(
                    //                                @RiderName,
                    //                                @Password,
                    //                                @BranchCode,
                    //                                1,
                    //                                @CreatedBy,
                    //                                getdate(),
                    //                                1,
                    //                                @RiderCode,
                    //                                0,
                    //                                @Imei1,
                    //                                @Imei2,
                    //                                @SimNO,
                    //                                @HubId
                    //                                );";
                    //}
                }
                else
                {
                    query = @"insert into App_Users (userName,PASSWORD,branchCode,roleID,createdBy,createdOn,STATUS,riderCode,isActive,IMEI1,IMEI2,SimSNO, HubId)
                                                    values(
                                                    @RiderName,
                                                    @Password,
                                                    @BranchCode,
                                                    1,
                                                    @CreatedBy,
                                                    getdate(),
                                                    1,
                                                    @RiderCode,
                                                    0,
                                                    @Imei1,
                                                    @Imei2,
                                                    @SimNO,
                                                    @HubId
                                                    );";
                }
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(query, model);
                con.Close();
                return rs > 0 ? "Rider Added" : "Something went wrong";
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return ex.Message;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return ex.Message;
            }
        }

        [NonAction]
        public async Task<string> UpdateRider(RiderFormModel model)
        {
            try
            {
                var query = "";
                if (model.HubId == 0)
                {
                    query = @"update App_Users set
                            userName = @RiderName,
                            PASSWORD = @Password,
                            IMEI1 = @Imei1,
                            IMEI2 = @Imei2,
                            SimSNO = @SimNO
                            where USER_ID = @UserId;";
                }
                else
                {
                    query = @"update App_Users set
                            userName = @RiderName,
                            PASSWORD = @Password,
                            IMEI1 = @Imei1,
                            IMEI2 = @Imei2,
                            SimSNO = @SimNO,
                            HubId = @HubId
                            where USER_ID = @UserId;";
                }
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(query, model);
                con.Close();
                return rs > 0 ? "Rider Updated" : "Something went wrong";
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return ex.Message;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return ex.Message;
            }
        }

        [NonAction]
        public async Task<RiderFormModel> GetRiderData(int id)
        {
            try
            {
                var query = $@"select top 1
                            USER_ID as UserId,
                            USERNAME as RiderName,
                            riderCode as RiderCode,
                            PASSWORD as [Password],
                            IMEI1 as Imei1,
                            IMEI2 as Imei2,
                            SimSNO as SimNO,
                            HubId as HubId
                            from App_Users where USER_ID = {id};";
                await con.OpenAsync();
                var rs = await con.QueryFirstOrDefaultAsync<RiderFormModel>(query);
                con.Close();
                return rs;
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
        }

        [NonAction]
        public async Task<IEnumerable<RiderHub>> GetRiderHub(string branchCode)
        {
            try
            {
                var query = $@"select Id, HubName
                            from App_Delivery_Location_Geofence where BranchCode = { branchCode }; ";
                await con.OpenAsync();
                IEnumerable<RiderHub> rs = await con.QueryAsync<RiderHub>(query);
                con.Close();
                return rs;
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
        }
    }
}