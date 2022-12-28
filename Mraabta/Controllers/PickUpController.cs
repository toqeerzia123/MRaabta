using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using MRaabta.Repo;
using MRaabta.Models;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Globalization;
using Org.BouncyCastle.Crypto.Tls;

namespace MRaabta.Controllers
{
    public class ExcelData
    {
        public string Account { get; set; }
        public string OriginId { get; set; }
        public string AlternateAddress { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string Weight { get; set; }
        public string Pieces { get; set; }
        public string RiderCode { get; set; }
        public string RiderMobileNo { get; set; }
    }
    public class PickUpController : Controller
    {
        PickUpDB pd;

        public PickUpController()
        {
            pd = new PickUpDB();
        }

        // GET: PickUp
        PickUpDB pdb = new PickUpDB();
        LocationDB ldb = new LocationDB();
        RiderDB rdb = new RiderDB();

        public async Task<ActionResult> ViewPickUp()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            string date = null;
            date = Request.Url.Query;
            if (date == null || date == "")
            {

            }
            else
            {
                string[] array = date.Split(new char[] { '&' }, 2);
                ViewBag.Date = array[0].Remove(0, 1);
                ViewBag.RiderID = array[1];
            }


            try
            {
                var branchcode = Session["BRANCHCODE"].ToString();
                await pd.OpenAsync();
                var riders = await pd.GetRiders(branchcode);
                pd.Close();
                ViewBag.Riders = riders.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }
        // GET: PickUp
        [HttpPost]
        public ActionResult getTotalConsignment(string riderCode, string StartDate, string EndDate)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            var riders = pd.TotalConsignment(riderCode, StartDate, EndDate);
            pd.Close();
            //JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            //var points = JsonConvert.SerializeObject(riders, _jsonSetting);

            return Json(new { dataPoints = riders });
        }
        public ActionResult getPickUpByRider(string riderCode, string StartDate, string EndDate)
        {
            //List<DataPoint> dataPoints = new List<DataPoint>();
            //var riders = pd.TotalConsignment(riderCode);
            //pd.Close();
            //ViewBag.DataPoints = JsonConvert.SerializeObject(riders);
            try
            {
                var GetPickupData = new GetPickupData();
                List<GetPickupData> lp = new List<GetPickupData>();
                if (riderCode != "" && StartDate != "" && EndDate != "")
                {
                    DataTable dt = new DataTable();


                    dt = pdb.getPickUp(riderCode, StartDate, EndDate);
                    if (dt.Rows.Count > 0)
                    {
                        string lat = "";
                        string lon = "";
                        string url = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string time = "";

                            string date = (DateTime.Parse(dt.Rows[i]["PickUpTime"].ToString())).ToString("yyyy-MM-dd hh:mm:ss.fff");
                            string[] d = date.Split(' ');
                            date = d[0];
                            time = d[1];

                            lat = dt.Rows[i]["latitude"].ToString();
                            lon = dt.Rows[i]["longitude"].ToString();
                            if (lat != "" & lon != "")
                            {
                                url = "http://www.google.com/maps/place/" + lat + "," + lon + "";
                            }
                            GetPickupData = new GetPickupData
                            {
                                PickUp_ID = int.Parse(dt.Rows[i]["PickUp_ID"].ToString()),
                                ClientName = dt.Rows[i]["ClientName"].ToString(),
                                locationName = dt.Rows[i]["locationName"].ToString(),
                                locationID = int.Parse(dt.Rows[i]["locationID"].ToString()),
                                riderCode = dt.Rows[i]["riderCode"].ToString(),
                                riderName = dt.Rows[i]["riderName"].ToString(),
                                PickUpTime = DateTime.Parse(dt.Rows[i]["PickUpTime"].ToString()),
                                latitude = double.Parse(dt.Rows[i]["latitude"].ToString()),
                                longitude = double.Parse(dt.Rows[i]["longitude"].ToString()),
                                PickUpUrl = url,
                                pick_Date = date,
                                pick_Time = time
                            };
                            lp.Add(GetPickupData);
                        }
                    }
                    else
                    {
                        TempData["error"] = "false";
                        ViewBag.Error = TempData["error"];
                    }
                }
                else
                {
                    TempData["error"] = "Kindly pass the parameters required";
                    ViewBag.Error = TempData["error"];
                    return View();
                }
                return View(lp);
            }


            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }


        public ActionResult GetPickUpChildByLocations(int PickUpID)
        {
            try
            {
                //locationID = "3";
                //StartDate = "01-03-2019";
                //EndDate = "06-04-2019";
                Pickup_Child_details pcd = new Pickup_Child_details();
                DataTable dt = new DataTable();
                List<Pickup_Child_details> DetailsModelList = new List<Pickup_Child_details>();
                dt = pdb.getPickUpDetails(PickUpID);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    pcd = new Pickup_Child_details
                    {
                        consignmentNumber = dt.Rows[i]["consignmentNumber"].ToString(),
                        weight = double.Parse(dt.Rows[i]["weight"].ToString()),
                        pieces = int.Parse(dt.Rows[i]["pieces"].ToString()),
                        imageURL = "../" + dt.Rows[i]["CN_picURL"].ToString()
                    };
                    DetailsModelList.Add(pcd);
                }
                return View(DetailsModelList);
            }
            catch (Exception ex)
            {
                ViewBag.err = "error";
                return View();
            }
        }

        //public ActionResult GetPickUpDataByRider()
        //{
        //    ViewModelPickUp vm = new ViewModelPickUp();
        //    vm.RiderData =  tRidersByBranchCode("4");
        //    ViewBag.ddl_rider = vm.RiderData;
        //    return View(vm);
        //}

        [HttpPost]
        public async Task<ActionResult> UploadBulkPickup()
        {
            var uid = Session["U_ID"].ToString();
            var file = Request.Files[0];
            var ext = Path.GetExtension(file.FileName);
            if (new List<string> { ".xlsx", ".xls" }.Contains(ext))
            {
                var rs = await UploadBulkData(file.InputStream, uid);
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Upload Excel File Only", JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string SendMobileNumName(string consigneeNumber)
        {
            string Number = consigneeNumber.Replace("-", "");

            int numLength = Number.Length;
            string n2 = Number;

            if (numLength != 12)
            {
                if (numLength == 13)
                {
                    n2 = Number.Remove(0, 1);
                }
                else if (numLength == 11)
                {

                    string code = "92";
                    n2 = code + Number.Remove(0, 1);

                }
                else if (numLength == 10)
                {
                    string code = "92";
                    n2 = code + Number;
                }
                else
                {
                    string code = "92";
                    n2 = code + Number;
                }

            }
            return n2;
        }

        [NonAction]
        public async Task<string> UploadBulkData(Stream filedata, string uid)
        {
            try
            {
                var data = new List<ExcelData>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(filedata))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int colCount = worksheet.Dimension.End.Column;
                    int rowCount = worksheet.Dimension.End.Row;


                    for (int row = 2; row <= rowCount; row++)
                    {
                        var excelData = new ExcelData();
                        excelData.Account = worksheet.Cells[row, 1].Value?.ToString();
                        excelData.OriginId = worksheet.Cells[row, 2].Value?.ToString();
                        excelData.AlternateAddress = worksheet.Cells[row, 3].Value?.ToString();
                        //excelData.PickupDateTime = DateTime.ParseExact(worksheet.Cells[row, 4].Value?.ToString(), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                        excelData.PickupDateTime = (DateTime)worksheet.Cells[row, 4].Value;
                        excelData.Weight = worksheet.Cells[row, 5].Value?.ToString();
                        excelData.Pieces = worksheet.Cells[row, 6].Value?.ToString();
                        excelData.RiderCode = worksheet.Cells[row, 7].Value?.ToString();
                        excelData.RiderMobileNo = worksheet.Cells[row, 8].Value?.ToString();
                        data.Add(excelData);
                    }
                }

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    await con.OpenAsync();
                    var trans = con.BeginTransaction();
                    try
                    {
                        var rs = await con.ExecuteAsync(@"INSERT INTO Pickup_Details(AccountNo,origin,riderCode,[weight],pickup_Date, riderPhone, pieces, createdOn, pickup_status, alternate_address)
                                                            VALUES(@accountNo,@origin,@riderCode,@weight,@pickupDate, @riderPhone, @pieces, getdate(), 0 , @alternateaddress);", data.Select(x => new
                        {
                            @accountNo = x.Account,
                            @origin = x.OriginId,
                            @riderCode = x.RiderCode,
                            @weight = x.Weight,
                            @pickupDate = x.PickupDateTime,
                            @riderPhone = x.RiderMobileNo,
                            @pieces = x.Pieces,
                            @alternateaddress = x.AlternateAddress
                        }).ToList(), transaction: trans);


                        rs = await con.ExecuteAsync(@"INSERT INTO MnP_SmsStatus(Recepient,MessageContent,[STATUS],CreatedOn,CreatedBy,smsformtype)
                                                        VALUES(@Recepient,@MessageContent,0,getdate(),@uid,'9');", data.Select(x => new
                        {
                            @Recepient = SendMobileNumName(x.RiderMobileNo),
                            @MessageContent = "Dear Rider, \n Pickup Account : " + x.Account + "\n Pickup Address: " + x.AlternateAddress + " \n Pickup Date: " + x.PickupDateTime.ToString("dd-MMM-yyyy") + "\n Pickup Time: " + x.PickupDateTime.ToString("HH:mm") + "\n Weight: " + x.Weight + "\n Pieces: " + x.Pieces,
                            @uid = uid
                        }).ToList(), transaction: trans);

                        trans.Commit();
                        con.Close();
                        return "Data Upload Successfully";
                    }
                    catch (SqlException ex)
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            trans.Rollback();
                            con.Close();
                        }
                        return "Something went wrong";
                    }
                    catch (Exception ex)
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            trans.Rollback();
                            con.Close();
                        }
                        return "Something went wrong";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong";
            }
        }
    }
}
