using MRaabta.App_Start;
using MRaabta.Models;
using MRaabta.Repo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class TaxForwarderController : Controller
    {
        TaxForwarderRepo repo;
        public TaxForwarderController()
        {
            repo = new TaxForwarderRepo();
        }
        // GET: TaxForwarder
        [HttpGet, SkipFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Save(HttpPostedFileBase file)
        {
            var FormDate = HttpContext.Request.Form["Date"];
            var ext = System.IO.Path.GetExtension(file.FileName);
            if (ext == ".xlsx" || ext == ".xls")
            {
                try
                {
                    string parseError = null;
                    var u = Session["UserInfo"] as UserModel;

                    if (file != null)
                    {
                        List<TaxForwarderModel> list = new List<TaxForwarderModel>();
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelPackage package = new ExcelPackage(file.InputStream);
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        int rows = worksheet.Dimension.Rows;
                        int columns = worksheet.Dimension.Columns;

                        if (
                            worksheet.Cells[1, 1].Value?.ToString()?.ToLower() == "CPR No".ToLower() &&
                            worksheet.Cells[1, 2].Value?.ToString()?.ToLower() == "Paid Amount".ToLower() &&
                            worksheet.Cells[1, 3].Value?.ToString()?.ToLower() == "Customer".ToLower() &&
                            worksheet.Cells[1, 4].Value?.ToString()?.ToLower() == "Invoice No".ToLower() &&
                            worksheet.Cells[1, 5].Value?.ToString()?.ToLower() == "Type".ToLower() &&
                            worksheet.Cells[1, 6].Value?.ToString()?.ToLower() == "Source".ToLower()
                            )
                        {
                            for (int i = 2; i <= rows; i++)
                            {
                                TaxForwarderModel pb = new TaxForwarderModel();
                                if (worksheet.Cells[i, 1].Value != null)
                                {
                                    pb.CPRNo = worksheet.Cells[i, 1].Value.ToString();
                                    if (pb.CPRNo.Contains("T"))
                                    {
                                        pb.CPRNo = pb.CPRNo;
                                    }
                                    else
                                    {
                                        pb.CPRNo = "Sales Tax";
                                    }
                                }
                                else
                                {
                                    parseError = $"Error in CBR Number at line number {i}";
                                    break;
                                }

                                if (worksheet.Cells[i, 2].Value != null && IsFloat(worksheet.Cells[i, 2].Value.ToString()))
                                {
                                    pb.PaidAmount = float.Parse(worksheet.Cells[i, 2].Value.ToString());
                                }
                                else
                                {
                                    parseError = $"Error in Paid Amount at line number {i}";
                                    break;
                                }

                                if (worksheet.Cells[i, 3].Value != null)
                                {
                                    pb.Customer = worksheet.Cells[i, 3].Value.ToString();
                                }
                                else
                                {
                                    parseError = $"Error in Customer at line number {i}";
                                    break;
                                }

                                if (worksheet.Cells[i, 4].Value != null)
                                {
                                    if (!worksheet.Cells[i, 4].Value.ToString().Contains("-"))
                                    {
                                        pb.InvoiceNumber = worksheet.Cells[i, 4].Value.ToString();
                                        pb.INV = worksheet.Cells[i, 4].Value.ToString();
                                    }
                                    else
                                    {
                                        var customerwithdash = worksheet.Cells[i, 4].Value.ToString().Split('-');
                                        pb.InvoiceNumber = customerwithdash[1];
                                        pb.INV = customerwithdash[1];
                                    }
                                }
                                else
                                {
                                    parseError = $"Error in Invoice Number at line number {i}";
                                    break;
                                }

                                if (worksheet.Cells[i, 5].Value != null && IsNum(worksheet.Cells[i, 5].Value.ToString()))
                                {
                                    pb.Type = int.Parse(worksheet.Cells[i, 5].Value.ToString());
                                }
                                else
                                {
                                    parseError = $"Error in Type at line number {i}";
                                    break;
                                }

                                if (worksheet.Cells[i, 6].Value != null && IsNum(worksheet.Cells[i, 6].Value.ToString()))
                                {
                                    pb.Source = int.Parse(worksheet.Cells[i, 6].Value.ToString());
                                }
                                else
                                {
                                    parseError = $"Error in Source at line number {i}";
                                    break;
                                }

                                list.Add(pb);
                            }

                            var IsInserted = repo.InsertData(FormDate, list);
                        }
                        else
                        {
                            parseError = $"Invalid file format kindly download sample";
                        }

                        if (string.IsNullOrEmpty(parseError))
                        {
                            return Json(new { sts = 0, msg = "Data has been Saved Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { sts = 1, msg = parseError }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { sts = 1, msg = "File not found" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { sts = 2, msg = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { sts = 1, msg = "Only excel files allowed" }, JsonRequestBehavior.AllowGet);
            }
        }


        [NonAction]
        public bool IsAplhaOrNumWithSpace(string str)
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            return regexItem.IsMatch(str);
        }

        [NonAction]
        public bool IsNum(string str)
        {
            var regexItem = new Regex("^[0-9]*$");
            return regexItem.IsMatch(str);
        }

        [NonAction]
        public bool IsFloat(string str)
        {
            var regexItem = new Regex("^[0-9.]*$");
            return regexItem.IsMatch(str);
        }

    }

}