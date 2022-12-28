using MRaabta.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MRaabta.Models;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using OfficeOpenXml;
using System.IO;

namespace MRaabta.Controllers
{
    public class CreditNotesController : Controller
    {
        CreditNotesRepo repo = new CreditNotesRepo();
        public async Task<ActionResult> Index(List<CreditNotesModel> model)
        {
            ViewBag.PaymentType = new SelectList(await repo.GetPaymentTypes(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetDetail(string Invoice)
        {
            var data = await repo.GetDetail(Invoice);
            if (data != null)
            {
                return Json(new
                {
                    data
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateDetail(string Invoice, decimal Amount, string VoucherDate, long PaymentType)
        {
            string Message = "";
            var dayend = await repo.GetDayEnd(Invoice);
            if (dayend != null)
            {
               if( Convert.ToDateTime(VoucherDate) < Convert.ToDateTime(dayend[0].MaxDate))
                {
                    Message = "Voucher Date must be greater than Day End Date "+ dayend[0].MaxDate.Day+"-"+ dayend[0].MaxDate.Month + "-"+ dayend[0].MaxDate.Year;
                }
                else if (Convert.ToDateTime(VoucherDate) > DateTime.Now)
                {
                    Message = "Voucher Date must be less than Todays date ";
                }
                else
                {
                    bool data = await repo.UpdateDetail(Invoice, Amount, VoucherDate, PaymentType);
                    if (data)
                    {
                        Message = "Update Successful";
                    }
                    else
                    {
                        Message = "Unsuccessful";
                    }
                }
            }
            else { Message = "Day-End Date can't be found."; }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> EditCreditNotes(string Invoice)
        {
            var data = await repo.GetDetail(Invoice);
            if (data != null)
            {
                data = data.Where(x => x.type == "Credit Note").ToList();
            }
            return View(data);
        }
        public async Task<ActionResult> EditDetail(string Id, string Invoice)
        {
            ViewBag.ModalTitle = "Edit Credit Note: " + Id;
            var data = await repo.GetCreditNoteDetail(Id);
            ViewBag.PaymentType = new SelectList(await repo.GetPaymentTypes(), "Id", "Name", data.paymenttypeid);
            var dayend = await repo.GetDayEnd(Invoice);
            if (dayend != null)
            {
                ViewBag.DayEnd = dayend[0].MaxDate.Year + "-" + dayend[0].MaxDate.Month + "-" + dayend[0].MaxDate.Day;
            }
            ViewBag.Invoice = Invoice; ViewBag.TotalAmount = 0;
            var checkamount = await repo.GetDetail(Invoice);
            if (checkamount != null)
            {
                ViewBag.TotalAmount = checkamount.Where(x => x.type == "Invoice").Sum(x => x.totalAmount) + (checkamount.Where(x => x.type == "Invoice").Sum(x => x.totalAmount) - checkamount.Where(x => x.creditnoteid != Id).Sum(x => x.totalAmount));
            }
            return PartialView("_EditDetail", data);
        }

        public async Task<ActionResult> UpdateCreditNoteDetail(string Invoice, string Id, decimal Amount, long PaymentType, string VoucherDate)
        {
            var rs = await repo.UpdateCreditNoteDetail(Id, Amount, PaymentType, VoucherDate);
            return rs == true ? RedirectToAction("EditCreditNotes", new { Invoice }) : null;
        }

        [HttpPost]
        public async Task<ActionResult> UploadExcel(HttpPostedFileBase postedFile)
        {
            ViewBag.PaymentType = new SelectList(await repo.GetPaymentTypes(), "Id", "Name");
            DataSet dt = new DataSet();
            List<CreditNotesModel> modellist = new List<CreditNotesModel>();
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    int FileLen;
                    System.IO.Stream MyStream;

                    FileLen = postedFile.ContentLength;
                    var input = new byte[FileLen];

                    // Initialize the stream.
                    MyStream = postedFile.InputStream;

                    // Read the file into the byte array.
                    MyStream.Read(input, 0, FileLen);

                    using (ExcelPackage package = new ExcelPackage(MyStream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int colCount = worksheet.Dimension.End.Column;
                        int rowCount = worksheet.Dimension.End.Row;

                        var dataTable = worksheet;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            CreditNotesModel model = new CreditNotesModel();
                            model.invoiceNumber = worksheet.Cells[row, 1].Value.ToString();
                            model.invoiceDate = Convert.ToDateTime(worksheet.Cells[row, 2].Value);
                            model.paymenttypeid = Convert.ToInt32(worksheet.Cells[row, 3].Value);
                            model.totalAmount = Convert.ToDecimal(worksheet.Cells[row, 4].Value);
                            model.accountNo = worksheet.Cells[row, 5].Value.ToString();
                            model.branch = worksheet.Cells[row, 6].Value.ToString();
                            modellist.Add(model);
                        }
                        List<ExcelErrorModel> errors = new List<ExcelErrorModel>();
                        ExcelErrorModel md1 = new ExcelErrorModel();
                        ExcelErrorModel md2 = new ExcelErrorModel();
                        ExcelErrorModel md3 = new ExcelErrorModel();
                        ExcelErrorModel md4 = new ExcelErrorModel();
                        var query = modellist.GroupBy(x => x.invoiceNumber).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                        if (query != null)
                        {
                            if (query.Count > 0)
                            {
                                for (int i = 0; i < query.Count; i++)
                                {
                                    md1.Invoice += query[i] + ", ";
                                }
                                md1.Error += "Duplicate Invoice In Excel";
                                errors.Add(md1);
                            }
                        }

                        var stringinvoices = ""; var excelinvoices="" ;
                        var check2 = modellist.OrderBy(x=>x.invoiceNumber).Select(x => x.invoiceNumber).Distinct().ToList();
                        if (check2 != null)
                        {
                            for (int i = 0; i < check2.Count; i++)
                            {
                                stringinvoices += "'" + check2[i] + "',";
                                excelinvoices += "('" + check2[i] + "'),";

                            }
                            stringinvoices = stringinvoices.Trim(',');
                            excelinvoices = excelinvoices.Trim(',');
                        }
                        var checknotexist = await repo.CheckInvoiceExists(excelinvoices);
                        if (checknotexist!=null && checknotexist!="")
                        {
                            md2.Invoice = checknotexist;
                            md2.Error = "These invoices dont exist in records";
                            errors.Add(md2);
                        }
                        var amountcheck = await repo.FindTotalAmounts(stringinvoices);
                        var invoicenumbers = ""; var amounts = "";
                        if (amountcheck != null)
                        {
                            for (int i = 0; i < amountcheck.Count; i++)
                            {
                                if (amountcheck[i].invoiceNumber == modellist[i].invoiceNumber)
                                {
                                    if (amountcheck[i].isCOD!=1 && amountcheck[i].totalAmount < modellist[i].totalAmount)
                                    {
                                        invoicenumbers += "'" + amountcheck[i].invoiceNumber + "',\n";
                                        amounts += "'" + amountcheck[i].totalAmount + "',\n";
                                        invoicenumbers = invoicenumbers.Trim(',');
                                        amounts = amounts.Trim(',');
                                        md3.Invoice = invoicenumbers;
                                        md3.Error = "Allowed amounts are: \n " + amounts;
                                        errors.Add(md3);
                                        break;
                                    }
                                }
                            }
                        }

                        var dayends = await repo.GetDayEndforBulk(stringinvoices);
                        if (dayends != null)
                        {
                            var dates = "";
                            for (int i = 0; i < dayends.Count; i++)
                            {
                                if (dayends[i].invoiceNumber == modellist[i].invoiceNumber)
                                {
                                    if (dayends[i].MaxDate > modellist[i].invoiceDate || modellist[i].invoiceDate > DateTime.Now)
                                    {
                                        invoicenumbers += "'" + dayends[i].invoiceNumber + "',\n";
                                        dates += "'" + dayends[i].MaxDate + "',\n";
                                        invoicenumbers = invoicenumbers.Trim(',');
                                        dates = dates.Trim(',');
                                        md4.Invoice = invoicenumbers;
                                        md4.Error = "Voucher Date must be greater than Day-End & less than todays date \n ";
                                        errors.Add(md4);
                                        break;
                                    }
                                }
                            }
                        }

                        TempData["Htmlcode"] = errors;
                        TempData["Message"] = "Displayed Successfully!";
                        return View("Index", modellist);
                    }
                }
                catch (Exception err)
                {

                }
            }
            return View("Index", "");
        }

        [HttpPost]
        public async Task<JsonResult> InsertBulkRecords(List<CreditNotesModel> items)
        {
            string Message = "";
            bool data = await repo.InsertBulkRecords(items);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}