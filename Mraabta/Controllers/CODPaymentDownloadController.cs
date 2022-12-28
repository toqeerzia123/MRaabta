using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class GridModel
    {
        public string Directory { get; set; }
        public string File { get; set; }
    }
    public class CODPaymentDownloadController : Controller
    {
        public ActionResult Index()
        {
            List<GridModel> rs = new List<GridModel>();
            DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/CODPayment"));//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.pdf", SearchOption.AllDirectories); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                rs.Add(new GridModel { Directory = file.DirectoryName.Substring(file.DirectoryName.IndexOf("CODPayment")), File = file.Name });              
            }
            return View(rs);
        }
    }
}