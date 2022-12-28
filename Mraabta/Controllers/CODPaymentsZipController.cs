using Ionic.Zip;
using MRaabta.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class CODPaymentsZipController : Controller
    {
        [HttpGet, SkipFilter]
        public void Download()
        {
            try
            {
                if (Session["FolderName"] != null)
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(Session["FolderName"].ToString());
                        MemoryStream output = new MemoryStream();
                        Response.Clear();
                        Response.Charset = "";
                        Response.ContentType = "application/zip";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=aa.zip");
                        zip.Save(Response.OutputStream);
                        Response.End();                        
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(Server.MapPath("~/FileErrorLog.txt"), $"DateTime: {DateTime.Now.ToString()}, Exception: {ex.Message}\n");
            }
        }
    }
}