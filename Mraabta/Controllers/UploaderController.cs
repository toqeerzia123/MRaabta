using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using MRaabta.Models;

namespace MRaabta.Controllers
{
    public class UploaderController : Controller
    {
        // GET: Uploader
        SqlConnection orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public ActionResult Index()
        {
            if (Session["U_ID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Status = "";
            return View();
        }
        [HttpPost]

        public ActionResult Index(HttpPostedFileBase CSVFile)
        {
            PaymentInstrumentModel model = new PaymentInstrumentModel();
            List<InstrumentUploaderModel> list = new List<InstrumentUploaderModel>();
            string filePath = string.Empty;
            try
            {
                if (CSVFile.ContentLength > 0)
                {
                    if (CSVFile != null)
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(CSVFile.FileName);
                        string extension = Path.GetExtension(CSVFile.FileName);
                        if (extension == ".csv")
                        {
                            CSVFile.SaveAs(filePath);
                            string[] lines = System.IO.File.ReadAllLines(filePath);
                            string[] columns = new string[3];
                            foreach (string line in lines)
                            {
                                columns = line.Split(',');
                                if (!columns[0].ToLower().Contains("payment"))
                                    list.Add(new InstrumentUploaderModel { PaymentId = columns[0].ToString().Trim(),   PaymentMode = columns[1].ToString().Trim(), PaymentModeNo = columns[2].ToString().Trim(), isUpdated = false });
                            }
                            model.InstrumentModelList = list;
                        }
                        else
                        {
                            ViewBag.Status = "Warning, Please upload csv file!";
                            return View(model);
                        }
                        model.FileName = CSVFile.FileName;
                        ViewBag.Status = "Total rows found: " + list.Count;
                    }
                }
                return View(model);
            }
            catch (Exception er)
            {
                ViewBag.Status = "Error: " + er.Message.ToString();
                return View(model);
            }
        }

        public ActionResult UploadFile(string FileName)
        {
            PaymentInstrumentModel model = new PaymentInstrumentModel();
            string filePath = string.Empty;
            var list = new List<InstrumentUploaderModel>();
            try
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    ViewBag.Status = "Warning, Please upload csv file!";
                    return View("Index");
                }
                string path = Server.MapPath("~/Uploads/");
                filePath = path + Path.GetFileName(FileName);
                string extension = Path.GetExtension(FileName);

                var dt = new DataTable();
                if (extension != ".csv")
                {
                    ViewBag.Status = "Warning, Please upload csv file!";
                    return View();
                }
                orcl.Open();
                int rowsEffected = 0;
                int TotalRows = 0;
                bool isUpdated = false;
                using (var transaction = orcl.BeginTransaction())
                {
                    string[] lines = System.IO.File.ReadAllLines(filePath);
                    string[] columns = new string[3];
                    foreach (string line in lines)
                    {
                        columns = line.Split(',');
                        if (!columns[0].ToLower().Contains("payment"))
                        {
                            rowsEffected = 0;
                            //rowsEffected = orcl.Execute($" Update Consignment set InstrumentNumber='" + columns[3].ToString().Trim() + "',InstrumentMode='" + columns[2].ToString().Trim() + "' where consignerAccountNo='" + columns[1].ToString().Trim() + "' and  transactionNumber= '" + columns[0].ToString().Trim() + "'  ", null, transaction);
                            rowsEffected = orcl.Execute($" Update Consignment set InstrumentNumber='" + columns[2].ToString().Trim() + "',InstrumentMode='" + columns[1].ToString().Trim() + "' where  transactionNumber= '" + columns[0].ToString().Trim() + "'  ", null, transaction);
                            TotalRows += rowsEffected;
                            if (rowsEffected == 0)
                            {
                                isUpdated = false;
                            }
                            else
                            {
                                isUpdated = true;
                            }
                            list.Add(new InstrumentUploaderModel { PaymentId = columns[0].ToString().Trim(),   PaymentMode = columns[1].ToString().Trim(), PaymentModeNo = columns[2].ToString().Trim(), isUpdated = isUpdated });
                        }
                    }

                    model.InstrumentModelList = list.OrderBy(a => a.isUpdated).ToList();
                    transaction.Commit();
                }
                ViewBag.Status = "CNs instrument updated successfully, Total Rows updated: " + list.Count(a => a.isUpdated == true) + " \r\n. Total CNs updated against excel: " + TotalRows;
                return View("Index", model);
            }
            catch (Exception er)
            {
                ViewBag.Status = "Error: " + er.Message.ToString();
                return View("Index");
            }
            finally
            {
                if (!string.IsNullOrEmpty(filePath))
                    System.IO.File.Delete(filePath);
                orcl.Close();
            }
        }

    }
}