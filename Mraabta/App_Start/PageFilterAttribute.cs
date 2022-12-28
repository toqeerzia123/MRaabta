using MRaabta.Models;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MRaabta.App_Start
{

    public class SkipFilterAttribute : Attribute { }
    public class PageFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        UserRepository repo;

        public PageFilterAttribute()
        {
            repo = new UserRepository();
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                bool skip = filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipFilterAttribute), true) ||
                        filterContext.ActionDescriptor.IsDefined(typeof(SkipFilterAttribute), true);

                if (!skip)
                {
                    if (filterContext.HttpContext.Request.AcceptTypes.Any(x => x == "text/html"))
                    {
                        var descriptor = filterContext.ActionDescriptor;
                        var controllerName = descriptor.ControllerDescriptor.ControllerName.ToLower();

                        if (!new List<string> { "login", "home" }.Contains(controllerName))
                        {
                            var user = filterContext.HttpContext.Session["UserInfo"] as UserModel;
                            if (user != null)
                            {
                                if (repo.IsAllowed(user.Uid, user.Profile, controllerName))
                                {
                                    var ip = GetUserIP(filterContext);
                                    PageLog(user.Uid.ToString(), controllerName, "MRaabtaMVC", "Layout", ip);
                                }
                                else
                                {
                                    if (filterContext.HttpContext.Session["DashboardHomeUrl"] != null)
                                    {

                                        filterContext.Result = new RedirectResult(filterContext.HttpContext.Session["DashboardHomeUrl"].ToString());
                                    }
                                    else
                                    {
                                        filterContext.Result = new RedirectResult("~/Login");
                                    }
                                }
                            }
                            else
                            {
                                var values = new RouteValueDictionary(new
                                {
                                    action = "Index",
                                    controller = "Login"
                                });
                                filterContext.Result = new RedirectToRouteResult(values);
                            }
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
        private string GetUserIP(ActionExecutingContext filterContext)
        {
            string ipList = filterContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }
            return filterContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"];
        }
        private void PageLog(string U_ID, string pageGetURL, String systemType, String originName, string ip)
        {
            string query = "Insert into Page_Usage_log (U_ID,CreatedOn,URL,systemType,originName,IpAddress) values (@U_ID,getDate(),@URL,@systemType,@originName,@ip)";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@U_ID", U_ID);
                    cmd.Parameters.AddWithValue("@URL", pageGetURL);
                    cmd.Parameters.AddWithValue("@systemType", systemType);
                    cmd.Parameters.AddWithValue("@originName", originName);
                    cmd.Parameters.AddWithValue("@ip", ip);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                }

            }
        }
    }
}