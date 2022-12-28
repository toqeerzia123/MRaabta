using MRaabta.Models.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using Dapper;
using System.Threading.Tasks;
using System.Configuration;

namespace MRaabta.Repo.Api
{
    public class ConsignmentDB
    {

        SqlConnection con;

        public ConsignmentDB()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }


        public async Task<bool> UpdateIsActive(int uid)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.ExecuteAsync($@"update App_Users set isActive = 1 where USER_ID = {uid};");
                con.Close();
                return rs > 0;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
        }

        public async Task<ConsignmentStatus> insertDB(ConsginmentModel consignobjs)
        {
            ConsignmentStatus ConsignStatusList = new ConsignmentStatus();

            try
            {
                string trim_zero = consignobjs.phone_Number.ToString();
                string onlyNumbers_phoneNumber = Regex.Replace(consignobjs.phone_Number.ToString(), "[^0-9.]", "");
                string phone_no = onlyNumbers_phoneNumber;
                string nic_Number = consignobjs.nic_number.ToString();
                string PickerPhoneNumber = consignobjs.pickerPhone_No.ToString();
                con.Open();

                var query = @"insert into App_Delivery_ConsignmentData (ConsignmentNumber,RunSheetNumber,riderCode,name,phone_no,is_nic,
                            is_self,is_cod,cod_amount,created_by,created_on,reason,relation,picker_name,
                            latitude,longitude,delivery_id,nic_number,isMobilePerformed,rider_comments,pickerPhone_No,rider_iemi,rider_amount_entered,StatusId,Battery, performed_on,ReasonId,RelationId)
                            values (@ConsignmentNumber,@RunsheetNum,@riderCode,@name,@phone_no,@is_nic,@is_self,@is_cod,@cod_amount,@createdby, CAST(getdate() AS datetime),@reason,@relation,@picker_name,@latitude,@longitude,@deliveryId,@nic_num,@isMobilePerformed,@rider_comments,@pickerPhone_No,@rider_iemi,@rider_amount_entered,@statusId,@battery, @performedOn,@ReasonId,@RelationId)";

                var rs = await con.ExecuteAsync(query, new
                {
                    @ConsignmentNumber = consignobjs.ConsignmentNumber,
                    @RunsheetNum = consignobjs.runsheet,
                    @riderCode = consignobjs.riderCode,
                    @name = consignobjs.name,
                    @phone_no = string.IsNullOrEmpty(onlyNumbers_phoneNumber) ? 0 : Convert.ToInt64(onlyNumbers_phoneNumber),
                    @is_nic = consignobjs.is_nic,
                    @is_self = consignobjs.is_self,
                    @is_cod = consignobjs.is_cod,
                    @cod_amount = consignobjs.cod_amount,
                    @createdby = consignobjs.createdBy,
                    @reason = consignobjs.reason,
                    @relation = consignobjs.relation,
                    @picker_name = consignobjs.pickerName,
                    @latitude = consignobjs.latitude,
                    @longitude = consignobjs.longitude,
                    @deliveryId = consignobjs.deilvery_id,
                    @nic_num = string.IsNullOrEmpty(nic_Number) ? 0 : Convert.ToInt64(nic_Number),
                    @isMobilePerformed = consignobjs.isMobilePerformed,
                    @rider_comments = consignobjs.rider_comments,
                    @pickerPhone_No = string.IsNullOrEmpty(PickerPhoneNumber) ? 0 : Convert.ToInt64(PickerPhoneNumber),
                    @rider_iemi = consignobjs.rider_iemi,
                    @rider_amount_entered = consignobjs.rider_amount_entered,
                    @statusId = consignobjs.statusId,
                    @battery = consignobjs.battery,
                    @performedOn = consignobjs.performedOn,
                    @ReasonId = consignobjs.reasonId == 0 ? null : consignobjs.reasonId,
                    @RelationId = consignobjs.relationId == 0 ? null : consignobjs.relationId
                });

                ConsignStatusList.consignmentNumber = consignobjs.ConsignmentNumber;
                ConsignStatusList.isSuccess = rs > 0;
                ConsignStatusList.Message = "Successfully Added";

            }
            catch (SqlException ex)
            {
                ConsignStatusList.consignmentNumber = consignobjs.ConsignmentNumber;
                ConsignStatusList.isSuccess = false;
                ConsignStatusList.Message = "Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                ConsignStatusList.consignmentNumber = consignobjs.ConsignmentNumber;
                ConsignStatusList.isSuccess = false;
                ConsignStatusList.Message = "Error: " + ex.Message;
            }
            finally
            {
                con.Close();
            }
            return ConsignStatusList;
        }

        public void MessageResponse(ConsginmentModel consignobjs)
        {
            ConsignmentStatus ConsignStatusList = new ConsignmentStatus();
            try
            {
                string str = consignobjs.phone_Number.ToString();
                string trim_phoneNumber = str.Replace(" ", "");
                string onlyNumbers_phoneNumber = Regex.Replace(trim_phoneNumber, "[^0-9.]", "");
                int ab = trim_phoneNumber.IndexOf('0');
                String newnum = "";
                int lengthNum = onlyNumbers_phoneNumber.Length - 2;

                if (onlyNumbers_phoneNumber.StartsWith("92"))
                {
                    if (onlyNumbers_phoneNumber.ElementAt(2) == '0')
                    {
                        onlyNumbers_phoneNumber = onlyNumbers_phoneNumber.Remove(2, 1);
                        ConsignStatusList.isSuccess = true;
                    }
                }

                if (onlyNumbers_phoneNumber.StartsWith("00"))
                {
                    onlyNumbers_phoneNumber = onlyNumbers_phoneNumber.Remove(0, 2);
                    onlyNumbers_phoneNumber = "92" + onlyNumbers_phoneNumber;
                    ConsignStatusList.isSuccess = true;
                }
                if (onlyNumbers_phoneNumber.StartsWith("0"))
                {
                    onlyNumbers_phoneNumber = onlyNumbers_phoneNumber.Remove(0, 1);
                    onlyNumbers_phoneNumber = "92" + onlyNumbers_phoneNumber;
                    ConsignStatusList.isSuccess = true;
                }
                if (onlyNumbers_phoneNumber.StartsWith("3"))
                {
                    onlyNumbers_phoneNumber = "92" + onlyNumbers_phoneNumber;
                    ConsignStatusList.isSuccess = true;
                }
                if (onlyNumbers_phoneNumber.Length != 12)
                {
                    ConsignStatusList.isSuccess = false;
                }
                if (onlyNumbers_phoneNumber.StartsWith("9221"))
                {
                    ConsignStatusList.isSuccess = false;
                }
                if (onlyNumbers_phoneNumber.StartsWith("021"))
                {
                    ConsignStatusList.isSuccess = false;
                }
                if (ConsignStatusList.isSuccess == true)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into MnP_SmsStatus (ConsignmentNumber, Recepient, MessageContent, STATUS, CreatedOn, CreatedBy, RunsheetNumber) values(" + consignobjs.ConsignmentNumber + ", " + Convert.ToInt64(onlyNumbers_phoneNumber) + ", 'Testing phase 1', 0, GETDATE(), " + consignobjs.createdBy + ", " + consignobjs.runsheet + ")");
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    ConsignStatusList.isSuccess = true;
                }
                else
                {

                }

            }
            catch (Exception er)
            {
                ConsignStatusList.isSuccess = false;
                ConsignStatusList.Message = "Error: " + er.Message;
            }
            finally
            {
                con.Close();
            }

        }

        public async Task<Tuple<bool, string>> CheckValidConsignmentValues(ConsginmentModel consignmentObjects)
        {
            try
            {
                Tuple<bool, string> response = new Tuple<bool, string>(false, "");
                if (consignmentObjects.ConsignmentNumber == null || consignmentObjects.ConsignmentNumber == "")
                {
                    response = new Tuple<bool, string>(false, "consignment Number not provided");
                }
                string str = consignmentObjects.phone_Number.ToString();
                string trim_phoneNumber = str.Replace(" ", "");
                string onlyNumbers_phoneNumber = Regex.Replace(trim_phoneNumber, "[^0-9.]", "");
                int phoneNumber = onlyNumbers_phoneNumber.Length;

                if (consignmentObjects.is_cod == true && consignmentObjects.cod_amount <= 0)
                {
                    await con.OpenAsync();
                    var rs = await con.QueryFirstOrDefaultAsync<int>(@"select top 1 case when codAmount = 0 then 1 end as IsCodZero from CODConsignmentDetail_New where consignmentNumber = @cn", new { cn = consignmentObjects.ConsignmentNumber });
                    con.Close();

                    if (rs == 1)
                    {
                        response = new Tuple<bool, string>(true, "");
                    }
                    else
                    {
                        response = new Tuple<bool, string>(false, "COD Amount not provided, please provide amount");
                    }

                }
                else if (consignmentObjects.riderCode == null || consignmentObjects.riderCode == "")
                {
                    response = new Tuple<bool, string>(false, "invalid riderCode");
                }
                else if (consignmentObjects.runsheet == null || consignmentObjects.runsheet == "")
                {
                    response = new Tuple<bool, string>(false, "Invalid runsheet number");
                }
                else
                {
                    response = new Tuple<bool, string>(true, "Data valid");
                }

                return response;
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return new Tuple<bool, string>(false, ex.ToString());
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return new Tuple<bool, string>(false, ex.ToString());
            }
        }

        private int CheckUniqueConsignmentNumber(string consignmentNumber)
        {
            int rowcount = 10;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "App_SP_CheckConsignmentNumberUnique";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ConsignmentNumber", consignmentNumber);
                cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                rowcount = int.Parse(cmd.Parameters["@result"].SqlValue.ToString());
            }
            catch (Exception er)
            {
                rowcount = 10;
            }
            finally
            {
                con.Close();
            }
            return rowcount;
        }

        public DataTable ConvertConsignListToDataTable(List<ConsginmentModel> consignobjs)
        {
            DataTable dataTable = new DataTable(typeof(ConsginmentModel).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(ConsginmentModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (ConsginmentModel item in consignobjs)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public Tuple<bool, string> SaveImageToSN(string ImgStr, string ImgName)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            String path2 = Path.Combine(HttpRuntime.AppDomainAppPath, "Content");
            String path = Path.Combine(HttpRuntime.AppDomainAppPath, "SignImage");

            try
            {
                //Check if directory exist
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                }
                if (File.Exists(path + "\\" + ImgName + ".jpg"))
                {
                    resp = new Tuple<bool, string>(false, "Consignment image already exists");
                    return resp;
                }

                string imageName = ImgName + ".jpg";

                //set the image path
                string imgPath = Path.Combine(path, imageName);

                byte[] imageBytes = Convert.FromBase64String(ImgStr);


                // byte[] imageBytes = Convert.FromBase64String(ImgStr);
                if (!File.Exists(imgPath))
                {
                    File.WriteAllBytes(imgPath, imageBytes);
                }
                resp = new Tuple<bool, string>(true, "Image successfully added");

            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, "Cannot add image!");

            }
            return resp;
        }

        public Tuple<bool, string> SaveImageToCN(string ImgStr, string ImgName)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");
            string workingDirectory = Environment.CurrentDirectory;
            String path = Path.Combine(HttpRuntime.AppDomainAppPath, "CNImage");

            try
            {
                //Check if directory exist
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                }
                if (File.Exists(path + "\\" + ImgName + ".jpg"))
                {
                    resp = new Tuple<bool, string>(false, "Consignment image already exists");
                    return resp;
                }

                string imageName = ImgName + ".jpg";

                //set the image path
                string imgPath = Path.Combine(path, imageName);

                byte[] imageBytes = Convert.FromBase64String(ImgStr);

                if (!File.Exists(imgPath))
                {
                    File.WriteAllBytes(imgPath, imageBytes);
                }
                resp = new Tuple<bool, string>(true, "Image successfully added");

            }
            catch (Exception ex)
            {

                resp = new Tuple<bool, string>(false, "Cannot add image!");

            }
            return resp;
        }

        public string InsertCnURL(DataTable dt_childURL)
        {
            string success = "";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "App_UpdateCnImage";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tbl", dt_childURL);

                cmd.Parameters.Add("@message", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                success = cmd.Parameters["@message"].SqlValue.ToString().ToUpper();

            }
            catch (Exception ex)
            {
                success = "Failed |" + ex.Message;
            }
            finally { con.Close(); }
            return success;
        }

        public async Task<List<(int value, string name)>> GetStatus()
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<(int value, string name)>(@"select Id as [Value], [Name] as [Text] from App_Delivery_Status;");
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
        }
    }
}