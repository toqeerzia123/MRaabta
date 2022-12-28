using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using MRaabta.App_Start;
using System.Web.Optimization;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using MRaabta.Models;
using MRaabta.App_Code;
using System.Collections.Generic;

namespace MRaabta
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //Code that runs on application startup
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            QuartzSchedular.Start();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region stopping cache generation
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            #endregion
        }
        //protected void Application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    #region Set session from cookies
        //    HttpContext context = HttpContext.Current;
        //    if (context.Session != null)
        //    if (context.Session["U_NAME"] == null)
        //    {
        //        if (context.Request.Cookies["userCookie"] != null)
        //        {
        //            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(context.Request.Cookies.Get("userCookie").Value);
        //            UserLogin(user.UserName, user.Password, context);
        //        }
        //    }
        //    #endregion
        //}
        private bool UserLogin(string userName, string password, HttpContext context)
        {
            bool temp = false;
            DataSet ds = new DataSet();
            // OracleConnection con = new OracleConnection(clvar.Strcon());
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            try
            {
                string sql_ = "SELECT\t   u.[U_CODE],\n" +
                            "\t         u.[U_ID],\n" +
                            "           u.[U_NAME],\n" +
                            "           u.[U_PASSWORD], u.[PROFILE],\n" +
                            "           UPPER(u.NAME) AS NAME,\n" +
                            "           u.[U_TYPE],\n" +
                            "           u.[STATUS],\n" +
                            "           u.[ACTIVE_DATE],\n" +
                            "           u.[INACTIVE_DATE],\n" +
                            "           u.[CHANGE_PASS_FLAG],\n" +
                            "           u.[USER_MAC_ADD],\n" +
                            "           u.[EXCEL_PERMISSION],\n" +
                            "           u.[DSG_CODE],\n" +
                            "           u.[ZONECODE],\n" +
                            "           u.[bts_user],\n" +
                            "           u.[BRANCHCODE],\n" +
                            "           u.[ExpressCenter],u.StaffLevel,\n" +
                            "           convert(nvarchar(10), u.INACTIVE_DATE, 105) INACTIVE_DATE ,\n" +
                            "           convert(nvarchar(11), (convert(datetime, e.workingdate)), 113) workingdate ,\n" +
                            "           e.name EXPRESSCENTERNAME,t.id LocationID, t.Name LocationName,  \n" +
                            "           mrs.BookingStaff,mrs.zone retail_zone, mrs.branch retail_branch, mrs.expresscentercode retail_eccode,re.name retail_ecname,mrs.shift retail_shift, \n" +
                            "           CASE WHEN mrs.UserId IS NOT NULL THEN '1' ELSE '0' END BookingUserStatus \n" +
                            "  FROM ZNI_USER1 u\n" +
                            " left outer JOIN EXPRESSCENTERs e ON e.expressCenterCode = u.ExpressCenter\n" +
                            " left outer join mnp_locations t on u.locationid = t.id \n" +
                            " LEFT JOIN MnP_Retail_Staff mrs ON mrs.UserId = u.U_ID and mrs.status ='1' \n" +
                            " LEFT JOIN EXPRESSCENTERs re ON  re.expressCenterCode = mrs.ExpressCenterCode \n" +
                            " WHERE UPPER(u.U_NAME) = UPPER('" + userName + "') AND u.U_PASSWORD = '" + password + "' AND bts_user = '1'  and isnull(u.status , 0) = '1' ";


                SqlCommand cmd = new SqlCommand(sql_, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter orda = new SqlDataAdapter(cmd);

                orda.Fill(ds);
                Session["Ds_Login"] = ds.Tables[0];

                if (ds.Tables[0].Rows.Count != 0)
                {
                    temp = true;
                    context.Session["U_NAME"] = ds.Tables[0].Rows[0]["U_NAME"].ToString();
                    context.Session["NAME"] = ds.Tables[0].Rows[0]["NAME"].ToString();
                    context.Session["ExpiryDate"] = ds.Tables[0].Rows[0]["inactive_date"].ToString();
                    context.Session["U_ID"] = ds.Tables[0].Rows[0]["U_ID"].ToString();
                    context.Session["ZONECODE"] = ds.Tables[0].Rows[0]["ZONECODE"].ToString();
                    context.Session["BRANCHCODE"] = ds.Tables[0].Rows[0]["BRANCHCODE"].ToString();
                    context.Session["bts_user"] = ds.Tables[0].Rows[0]["bts_user"].ToString();
                    context.Session["ExpressCenter"] = ds.Tables[0].Rows[0]["ExpressCenter"].ToString();
                    context.Session["WorkingDate"] = ds.Tables[0].Rows[0]["workingdate"].ToString();
                    context.Session["EXPRESSCENTERNAME"] = ds.Tables[0].Rows[0]["EXPRESSCENTERNAME"].ToString();
                    context.Session["INACTIVE_DATE"] = ds.Tables[0].Rows[0]["INACTIVE_DATE"].ToString();
                    context.Session["PROFILE"] = ds.Tables[0].Rows[0]["PROFILE"].ToString();
                    context.Session["LocationID"] = ds.Tables[0].Rows[0]["LocationID"].ToString();
                    context.Session["LocationName"] = ds.Tables[0].Rows[0]["LocationName"].ToString();
                    DataTable dt_riders = Get_RIDERS(Session["BRANCHCODE"].ToString(), con);
                    HttpContext.Current.Session["dt_riders"] = dt_riders;
                    HttpContext.Current.Session["StaffLevel"] = ds.Tables[0].Rows[0]["StaffLevel"].ToString();
                    HttpContext.Current.Session["BookingUserStatus"] = ds.Tables[0].Rows[0]["BookingUserStatus"].ToString();

                    if (ds.Tables[0].Rows[0]["BookingUserStatus"].ToString() == "1")
                    {
                        context.Session["ZONECODE"] = ds.Tables[0].Rows[0]["retail_zone"].ToString();
                        context.Session["BRANCHCODE"] = ds.Tables[0].Rows[0]["retail_branch"].ToString();
                        context.Session["ExpressCenter"] = ds.Tables[0].Rows[0]["retail_eccode"].ToString();
                        context.Session["EXPRESSCENTERNAME"] = ds.Tables[0].Rows[0]["retail_ecname"].ToString();
                        context.Session["BookingStaff"] = ds.Tables[0].Rows[0]["BookingStaff"].ToString();
                        context.Session["retail_shift"] = ds.Tables[0].Rows[0]["retail_shift"].ToString();
                        //Session["BookingUserStatus"] = ds.Tables[0].Rows[0]["BookingUserStatus"].ToString();                    
                    }
                    else
                    {
                        context.Session["ZONECODE"] = ds.Tables[0].Rows[0]["ZONECODE"].ToString();
                        context.Session["BRANCHCODE"] = ds.Tables[0].Rows[0]["BRANCHCODE"].ToString();
                        context.Session["ExpressCenter"] = ds.Tables[0].Rows[0]["ExpressCenter"].ToString();
                        context.Session["EXPRESSCENTERNAME"] = ds.Tables[0].Rows[0]["EXPRESSCENTERNAME"].ToString();

                    }

                    Consignemnts clvar = new Consignemnts();

                    //Getting Menus And Save to Session

                    List<MenuModel> menus = new List<MenuModel>();

                    var rs = con.Query<MenuModel>(@"select m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink
                                                    from Profile_Head ph
                                                    inner join Profile_Detail pd on ph.Profile_Id = pd.Profile_Id
                                                    inner join main_menu m on pd.MainMenu_Id = m.Menu_Id
                                                    inner join ZNI_USER1 u on u.Profile = ph.Profile_Id
                                                    where u.Profile = ph.Profile_Id and ph.Profile_Id = pd.Profile_Id and pd.MainMenu_Id = m.Menu_Id 
                                                    and m.Status = '1' and u.bts_User = '1' and u.U_ID = @uid
                                                    group by m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink
                                                    Order by m.Menu_Id, m.Menu_Name, u.U_NAME, u.Profile, m.HyperLink;", new { @uid = int.Parse(ds.Tables[0].Rows[0]["U_ID"].ToString()) });


                    var rs2 = con.Query<SubMenuModel>(@"SELECT m.Menu_id, c.child_menuid, c.sub_menu_name, c.Hyperlink sublink	
                                                        FROM main_menu m 
                                                        INNER JOIN child_menu c ON  c.main_Menu_id = m.Menu_id 
                                                        INNER JOIN Profile_Detail pd ON  pd.ChildMenu_Id = c.Child_MenuId
                                                        inner join Profile_Head ph on ph.Profile_Id = pd.Profile_Id
                                                        inner join ZNI_USER1 u on u.Profile = ph.Profile_Id
                                                        WHERE u.U_ID = @uid and m.Menu_Id in @mids;",
                                                        new
                                                        {
                                                            @uid = int.Parse(ds.Tables[0].Rows[0]["U_ID"].ToString()),
                                                            @mids = rs.Select(x => x.Menu_Id).ToList()
                                                        });

                    foreach (var item in rs)
                    {
                        item.SubMenus = rs2.Where(x => x.Menu_id == item.Menu_Id).ToList();
                        menus.Add(item);
                    }

                    context.Session["Menus2"] = menus;

                }
                else
                {
                    temp = false;
                }

                con.Close();
            }
            catch (Exception Err)
            {
                //AppMsg.Caption = Err.Message.ToString();
                temp = false;
            }
            finally
            {

            }
            return temp;
        }
        private DataTable Get_RIDERS(string BranchID, SqlConnection con)
        {
            string query = "SelecT r.firstName + ' ' + r.lastName RiderName, r.* from Riders r where  r.BranchID = '" + BranchID + "' and r.status = '1'";

            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            finally { }
            return dt;
        }
    }
}