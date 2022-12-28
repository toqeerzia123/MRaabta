using MRaabta.Models;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class FetchDataController : Controller
    {
        // GET: FetchData
        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty(GetConStr()))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Index(string query, int type)
        {
            var conStr = ConfigurationManager.ConnectionStrings[GetConStr()].ConnectionString;
            var dt = await GetData(conStr, query);
            if (type == 1)
            {
                if (dt != null)
                {
                    var memoryStream = new MemoryStream();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var excelPackage = new ExcelPackage(memoryStream))
                    {
                        var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.None);
                        worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                        worksheet.DefaultRowHeight = 18;
                        var data = excelPackage.GetAsByteArray();
                        return File(data, "application/octet-stream", "Data.xlsx");
                    }
                }
                else
                {
                    ViewBag.Query = query;
                    return View(dt);
                }
            }
            else
            {
                ViewBag.Query = query;
                return View(dt);
            }
        }

        [NonAction]
        public async Task<DataTable> GetData(string conStr, string query)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    DataTable dt = new DataTable();
                    await con.OpenAsync();
                    var command = new SqlCommand(query, con);
                    command.CommandTimeout = TimeSpan.FromMinutes(5).Seconds;
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                    con.Close();
                    return dt;
                }
                catch (SqlException ex)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    return null;
                }
            }
        }

        [NonAction]
        private string GetConStr()
        {
            var u = Session["UserInfo"] as UserModel;
            return ConfigurationManager.AppSettings[u.Uid.ToString()];
        }
    }
}