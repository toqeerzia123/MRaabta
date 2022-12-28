using MRaabta.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public void SetUserCookie(UserModel user)
        {
            var userString = JsonConvert.SerializeObject(user);
            var userCookie = new HttpCookie("userCookie", userString);
            userCookie.Expires = DateTime.Now.AddHours(8);
            Response.Cookies.Add(userCookie);
        }

        [NonAction]
        public UserModel GetUser()
        {
            if (Request.Cookies["userCookie"] != null)
            {
                return JsonConvert.DeserializeObject<UserModel>(Request.Cookies.Get("userCookie").Value);
            }
            else
            {
                return null;
            }
        }

        //[NonAction]
        //public void SetUrlCookie(string url)
        //{
        //    var urlCookie = new HttpCookie("urlCookie", url);
        //    urlCookie.Expires = DateTime.Now.AddHours(8);
        //    Response.Cookies.Add(urlCookie);
        //}

        [NonAction]
        public void DeleteAllCookie()
        {
            string[] myCookies = Request.Cookies.AllKeys;
            HttpCookie cookies = null;
            foreach (string cookie in myCookies)
            {
                cookies = Request.Cookies.Get(cookie);
                cookies.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookies);
            }
        }
    }
}