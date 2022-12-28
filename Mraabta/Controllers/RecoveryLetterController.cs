using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MRaabta.Controllers
{
    public class RecoveryLetterController : Controller
    {
        RecoveryLetterRepo repo = new RecoveryLetterRepo();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetReport(string AccountNo, string GroupId, string Days)
        {
            var data = await repo.GetData(AccountNo, GroupId, Days);
            ViewBag.Days = Days;
            return PartialView("_GetReport", data);
        }
        [HttpPost]
        public ActionResult SetTempInvoices(string invoices, string Days)
        {
            if (invoices != "")
            {
                TempData["invoices"] = invoices;
                TempData["days"] = Days;
             //   TempData["actualdays"] = actualDays;
            }
            return Json(new { sts = 0 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ViewRecoveryLetter(string Days = null, string InvoiceNumber = null)
        {
            string invoices, days;

            if (InvoiceNumber != null)
            {
                invoices = InvoiceNumber;
                var data = await repo.GetDetailData(invoices, Days);
                ViewBag.Days = Days;
                return View(data);
            }
            else
            {
                if (TempData["invoices"] != null && TempData["days"] != null)
                {
                    invoices = TempData["invoices"].ToString();
                    days = TempData["days"].ToString();
                    var data = await repo.GetDetailData(invoices, days);
                    ViewBag.Days = days;
                    return View(data);
                }
            }
            return View();
        }

        //public string RenderViewAsString(string viewName, object model)
        //{
        //    // create a string writer to receive the HTML code
        //    StringWriter stringWriter = new StringWriter();

        //    // get the view to render
        //    ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
        //    // create a context to render a view based on a model
        //    ViewContext viewContext = new ViewContext(
        //            ControllerContext,
        //            viewResult.View,
        //            new ViewDataDictionary(model),
        //          new TempDataDictionary(),
        //          stringWriter
        //          );

        //    // render the view to a HTML code
        //    viewResult.View.Render(viewContext, stringWriter);

        //    // return the HTML code
        //    return stringWriter.ToString();
        //}

        //public string HTMLString()
        //{
        //    StringBuilder htmltext = new StringBuilder();
        //    htmltext.Append("<div><h1>Welcome To aspdotnetpools</h1></div>");
        //    htmltext.Append("<table width='100%' border='1'>");
        //    htmltext.Append("<tr>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("</tr>");
        //    htmltext.Append("<tr>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("</tr>");
        //    htmltext.Append("<tr>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("</tr>");
        //    htmltext.Append("<tr>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("</tr>");
        //    htmltext.Append("<tr>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("</tr>");
        //    htmltext.Append("<tr>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("</tr>");
        //    htmltext.Append("<tr>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("<td>Data 1</td>");
        //    htmltext.Append("</tr>");
        //    htmltext.Append("</table>");
        //    htmltext.Append("<br/><br/>");
        //    htmltext.Append("Thanks you for downloading");
        //    htmltext.Append("aspdotnet-pools.com");

        //    return htmltext.ToString();
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult ConvertThisPageToPdf(string invoices, string Days)
        //{
        //    byte[] bytes; string GridHtml = "abcd";
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader sr = new StringReader(GridHtml);
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        HTMLWorker htmlWorker = new HTMLWorker(pdfDoc);
        //        htmlWorker.Parse(sr);
        //        pdfDoc.Close();
        //        bytes = stream.ToArray();
        //    }
        //    string filePath = "D:/";
        //    System.IO.File.WriteAllBytes(filePath, bytes);
        //    return Json(new { sts = 0 }, JsonRequestBehavior.AllowGet);
        //}
    }
}