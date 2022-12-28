using Dapper;
using MRaabta.App_Code;
using MRaabta.Models;
using MRaabta.Repo;
using MRaabta.Util;
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
    public class LoginController : Controller
    {
        UserRepository repo;
        bayer_Function b_fun;
        Variable clvar;

        public LoginController()
        {
            repo = new UserRepository();
            clvar = new Variable();
            b_fun = new bayer_Function();
        }

        [HttpGet]
        public ActionResult Index()
        {
            //DeleteAllCookie();
            Session.Abandon();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string userName, string password)
        {
            var user = await repo.Login(userName.ToUpper(), password);

            if (user != null)
            {
                //SetUserCookie(user);
                Session["Menus"] = repo.GetMenues(user.Uid);
                Session["UserInfo"] = user;


                //Temp work for running webforms
                var rs = UserLogin(userName, password);
                if (rs)
                {
                    Session["User_Info"] = user.Name;
                    if (Session["Profile"].ToString() == "4")
                    {
                        return Redirect("~/ECSelection.aspx");
                    }
                }

                var deliveryProfiles = await repo.GetDeliveryProfiles(795);

                if (deliveryProfiles != null && deliveryProfiles.Contains(user.Profile))
                {
                    Session["DashboardHomeUrl"] = Url.Action("Index", "DeliveryAppDashboard");
                    return RedirectToAction("Index", "DeliveryAppDashboard");
                }
                else
                {
                    Session["DashboardHomeUrl"] = Url.Action("Index", "Home");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Msg = "Login Failed";
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoginApp(string i, string k)
        {
            try
            {
                i = CryptoEngine.Base64Decode(i);
                k = CryptoEngine.Base64Decode(k);
                var user = await repo.Login(i.ToUpper(), k);
                if (user != null)
                {
                    Session["Menus"] = repo.GetMenues(user.Uid);
                    Session["UserInfo"] = user;
                    //Temp work for running webforms
                    var rs = UserLogin(i, k);
                    if (rs)
                    {
                        Session["User_Info"] = user.Name;
                        if (Session["Profile"].ToString() == "4")
                        {
                            return Redirect("~/ECSelection.aspx");
                        }
                    }

                    var deliveryProfiles = await repo.GetDeliveryProfiles(795);

                    if (deliveryProfiles != null && deliveryProfiles.Contains(user.Profile))
                    {
                        Session["DashboardHomeUrl"] = Url.Action("Index", "DeliveryAppDashboard");
                        return RedirectToAction("Index", "DeliveryAppDashboard");
                    }
                    else
                    {
                        Session["DashboardHomeUrl"] = Url.Action("Index", "Home");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return Content("Invalid User Name/Password");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            //DeleteAllCookie();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        //Temp work for running webforms
        [NonAction]
        private bool UserLogin(string userName, string password)
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
                            "           u.[department],\n" +
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
                    Session["U_NAME"] = ds.Tables[0].Rows[0]["U_NAME"].ToString();
                    Session["NAME"] = ds.Tables[0].Rows[0]["NAME"].ToString();
                    Session["ExpiryDate"] = ds.Tables[0].Rows[0]["inactive_date"].ToString();
                    Session["U_ID"] = ds.Tables[0].Rows[0]["U_ID"].ToString();
                    Session["ZONECODE"] = ds.Tables[0].Rows[0]["ZONECODE"].ToString();
                    Session["BRANCHCODE"] = ds.Tables[0].Rows[0]["BRANCHCODE"].ToString();
                    Session["bts_user"] = ds.Tables[0].Rows[0]["bts_user"].ToString();
                    Session["ExpressCenter"] = ds.Tables[0].Rows[0]["ExpressCenter"].ToString();
                    Session["WorkingDate"] = ds.Tables[0].Rows[0]["workingdate"].ToString();
                    Session["EXPRESSCENTERNAME"] = ds.Tables[0].Rows[0]["EXPRESSCENTERNAME"].ToString();
                    Session["INACTIVE_DATE"] = ds.Tables[0].Rows[0]["INACTIVE_DATE"].ToString();
                    Session["PROFILE"] = ds.Tables[0].Rows[0]["PROFILE"].ToString();
                    Session["LocationID"] = ds.Tables[0].Rows[0]["LocationID"].ToString();
                    Session["LocationName"] = ds.Tables[0].Rows[0]["LocationName"].ToString();
                    DataTable dt_riders = Get_RIDERS(Session["BRANCHCODE"].ToString());
                    Session["dt_riders"] = dt_riders;
                    Session["StaffLevel"] = ds.Tables[0].Rows[0]["StaffLevel"].ToString();
                    Session["BookingUserStatus"] = ds.Tables[0].Rows[0]["BookingUserStatus"].ToString();
                    // Added for Inventory System
                    Session["DepartmentID"] = ds.Tables[0].Rows[0]["department"].ToString();

                    if (ds.Tables[0].Rows[0]["BookingUserStatus"].ToString() == "1")
                    {
                        Session["ZONECODE"] = ds.Tables[0].Rows[0]["retail_zone"].ToString();
                        Session["BRANCHCODE"] = ds.Tables[0].Rows[0]["retail_branch"].ToString();
                        Session["ExpressCenter"] = ds.Tables[0].Rows[0]["retail_eccode"].ToString();
                        Session["EXPRESSCENTERNAME"] = ds.Tables[0].Rows[0]["retail_ecname"].ToString();
                        Session["BookingStaff"] = ds.Tables[0].Rows[0]["BookingStaff"].ToString();
                        Session["retail_shift"] = ds.Tables[0].Rows[0]["retail_shift"].ToString();
                        //Session["BookingUserStatus"] = ds.Tables[0].Rows[0]["BookingUserStatus"].ToString();                    
                    }
                    else
                    {
                        Session["ZONECODE"] = ds.Tables[0].Rows[0]["ZONECODE"].ToString();
                        Session["BRANCHCODE"] = ds.Tables[0].Rows[0]["BRANCHCODE"].ToString();
                        Session["ExpressCenter"] = ds.Tables[0].Rows[0]["ExpressCenter"].ToString();
                        Session["EXPRESSCENTERNAME"] = ds.Tables[0].Rows[0]["EXPRESSCENTERNAME"].ToString();

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

                    Session["Menus2"] = menus;

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

        [NonAction]
        private DataTable Get_RIDERS(string BranchID)
        {
            string query = "SelecT r.firstName + ' ' + r.lastName RiderName, r.* from Riders r where  r.BranchID = '" + BranchID + "' and r.status = '1'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            finally { con.Close(); }
            return dt;
        }
    }
}