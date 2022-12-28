using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MRaabta.Models;

namespace MRaabta.Controllers
{
    public class CSVUploadController : Controller
    {
        public SqlConnection con;

        public void connection()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        // GET: Test
        public ActionResult Index()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(file.InputStream);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn(
                )));
            dt.Columns[0].ColumnName = "id";
            dt.Columns[1].ColumnName = "runsheet_number";
            dt.Columns[2].ColumnName = "cn_number";
            dt.Columns[3].ColumnName = "name";
            dt.Columns[4].ColumnName = "phone_no";
            dt.Columns[5].ColumnName = "is_nic";
            dt.Columns[6].ColumnName = "is_self";
            dt.Columns[7].ColumnName = "is_cod";
            dt.Columns[8].ColumnName = "cod_amount";
            dt.Columns[9].ColumnName = "created_by";
            dt.Columns[10].ColumnName = "created_on";
            dt.Columns[11].ColumnName = "status";
            dt.Columns[12].ColumnName = "reason";
            dt.Columns[13].ColumnName = "relation";
            dt.Columns[14].ColumnName = "picker_name";
            dt.Columns[15].ColumnName = "latitude";
            dt.Columns[16].ColumnName = "longitude";
            dt.Columns[17].ColumnName = "user_id";
            dt.Columns[18].ColumnName = "is_transfer_data";
            dt.Columns[19].ColumnName = "is_transfer_image";
            dt.Columns[20].ColumnName = "delivery_id";
            dt.Columns[21].ColumnName = "nic_cumber";
            dt.Columns[22].ColumnName = "big_photo";
            dt.Columns[23].ColumnName = "is_transfer_image_big";
            dt.Columns[24].ColumnName = "picker_contact_number";
            dt.Columns[25].ColumnName = "rider_comments";
            dt.Columns[26].ColumnName = "is_performed";
            dt.Columns[27].ColumnName = "is_entered_cod_amount";
            dt.Columns[28].ColumnName = "rider_iemi";
            //dt.Columns[29].ColumnName = "rider_amount_entered";
            dt.AcceptChanges();
            //Read each line in the CVS file until it’s empty

            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();
                //line = line.Remove('\"');
                //add our current value to our data row
                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);


            }
            Session["abc"] = dt;
            ViewBag.Model = dt.AsEnumerable();


            return View(dt);
        }


        [HttpPost]
        public JsonResult Upload()
        {
            var partiallyExists = false;
            try
            {
                DataTable asset = Session["abc"] as DataTable;
                string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    if (asset.Rows.Count > 0)
                    {
                        con.Open();

                        for (int i = 0; i < asset.Rows.Count; i++)
                        {
                            try
                            {
                                String datee = asset.Rows[i]["created_on"].ToString().TrimStart('"').TrimEnd('"');
                                //DateTime date = Convert.ToDateTime(datee, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                DateTime date = DateTime.ParseExact(datee, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);


                                string phoneNo = asset.Rows[i]["phone_no"].ToString().TrimStart('"').TrimEnd('"');
                                phoneNo = Regex.Replace(phoneNo, @"[^\d]", "");
                                string NICNumbr = asset.Rows[i]["nic_cumber"].ToString().TrimStart('"').TrimEnd('"');
                                string PickerPhoneNumber = asset.Rows[i]["picker_contact_number"].ToString().TrimStart('"').TrimEnd('"');
                                if (phoneNo == null || phoneNo == "")
                                {
                                    phoneNo = "0";
                                }
                                if (NICNumbr == null || NICNumbr == "")
                                {
                                    NICNumbr = "0";
                                }
                                if (PickerPhoneNumber == null || PickerPhoneNumber == "")
                                {
                                    PickerPhoneNumber = "0";
                                }

                                string query = "Insert into App_Delivery_ConsignmentData ([ConsignmentNumber],[RunSheetNumber],[riderCode],[name],[phone_no],[is_nic],[is_self],[is_cod],[cod_amount], [created_by],[performed_on],[created_on] ,[reason] ,[relation] ,[picker_name] ,[latitude],[longitude] ,[delivery_id] ,[nic_number],[isMobilePerformed],[rider_comments],[pickerPhone_No],[rider_iemi],[rider_amount_entered])" + "\n"
                                + "Values ('" + asset.Rows[i]["cn_number"].ToString().Replace("'", "").TrimStart('"').TrimEnd('"') + "' , '" + asset.Rows[i]["runsheet_number"].ToString().Replace("'", "").TrimStart('"').TrimEnd('"') + "', '" + asset.Rows[i]["user_id"].ToString().TrimStart('"').TrimEnd('"') + "','" + asset.Rows[i]["name"].ToString().TrimStart('"').TrimEnd('"') + "'," + Convert.ToInt64(phoneNo) + "," + asset.Rows[i]["is_nic"].ToString().TrimStart('"').TrimEnd('"') + "," + asset.Rows[i]["is_self"].ToString().TrimStart('"').TrimEnd('"') + "," + asset.Rows[i]["is_cod"].ToString().TrimStart('"').TrimEnd('"') + "," + asset.Rows[i]["cod_amount"].ToString().TrimStart('"').TrimEnd('"') + ",'" + asset.Rows[i]["user_id"].ToString().TrimStart('"').TrimEnd('"') + "','" + date.ToString("yyyy-MM-dd HH:mm:ss") + "',getdate(),'" + asset.Rows[i]["reason"].ToString().TrimStart('"').TrimEnd('"') + "','" + asset.Rows[i]["relation"].ToString().TrimStart('"').TrimEnd('"') + "','" + asset.Rows[i]["picker_name"].ToString().TrimStart('"').TrimEnd('"') + "'," + asset.Rows[i]["latitude"].ToString().TrimStart('"').TrimEnd('"') + "," + asset.Rows[i]["longitude"].ToString().TrimStart('"').TrimEnd('"') + "," + asset.Rows[i]["delivery_id"].ToString().TrimStart('"').TrimEnd('"') + "," + Convert.ToInt64(NICNumbr) + "," + asset.Rows[i]["is_performed"].ToString().TrimStart('"').TrimEnd('"') + ",'" + asset.Rows[i]["rider_comments"].ToString().TrimStart('"').TrimEnd('"') + "'," + Convert.ToInt64(PickerPhoneNumber) + ",'" + asset.Rows[i]["rider_iemi"].ToString().TrimStart('"').TrimEnd('"') + "','" + asset.Rows[i]["is_entered_cod_amount"].ToString().TrimStart('"').TrimEnd('"') + "')";
                                SqlCommand orcd = new SqlCommand(query, con);
                                orcd.CommandType = CommandType.Text;
                                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                                orcd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                if (ex.Number == 2627)
                                {
                                    partiallyExists = true;
                                }
                            }
                        }

                        con.Close();

                        return Json(!partiallyExists ? "Data Sucessfully Uploaded" : "Data Partially Uploaded", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("No Records", JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

    }
}