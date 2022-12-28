using Dapper;
using MRaabta.Models;
using MRaabta.Models.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MRaabta.Repo.Api
{
    public class AccountDB_v5
    {
        SqlConnection con;
        public AccountDB_v5()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public DataTable validateUser(string userName, string password, string imei1, string imei2, string simSno)
        {
            DateTime createOn = DateTime.Now;
            DataTable ds = new DataTable();
            try
            {
                var query = $@"select usr.USER_ID,usr.branchCode,usr.createdBy,usr.createdOn,usr.userName,usr.PASSWORD,usr.riderCode,usr.roleID,adlg.Longitude, adlg.Latitude
                            from App_Users as usr 
                            INNER JOIN App_Delivery_Location_Geofence adlg ON adlg.Id= usr.HubId
                            where 
                            usr.STATUS = 1
                            and usr.riderCode='{userName}' 
                            and usr.password='{password}' 
                            {(!string.IsNullOrEmpty(imei1) ? $" and usr.IMEI1 = '{imei1}'" : "")}
                            {(!string.IsNullOrEmpty(imei2) ? $" and usr.IMEI2 = '{imei2}'" : "")}
                            {(!string.IsNullOrEmpty(simSno) ? $" and usr.SimSNO = '{simSno}'" : "")};";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                ds.Clear();
                ds.Columns.Add("USER_ID", typeof(Int32));
                ds.Columns.Add("branchCode", typeof(string));
                ds.Columns.Add("createdBy", typeof(Int32));
                ds.Columns.Add("createdOn", typeof(DateTime));
                ds.Columns.Add("userName", typeof(string));
                ds.Columns.Add("PASSWORD", typeof(string));
                ds.Columns.Add("riderCode", typeof(string));
                ds.Columns.Add("roleID", typeof(Int32));
                //ds.Columns.Add("status", typeof(Int32));

                SqlDataAdapter oda = new SqlDataAdapter(cmd);
                oda.Fill(ds);

            }
            catch (Exception Err)
            {
                throw Err;
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
        public DataTable validateUserLogout(string userName)
        {
            int RiderCode = 0, branchCode = 0, RoleId = 0;
            string RiderName = "", createdBy = "";
            DateTime createOn = DateTime.Now;
            DataTable ds = new DataTable();
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "App_dlv_GetUser_Logout";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RiderCode", userName);
                //cmd.Parameters.AddWithValue("@branchCode", branchcode);

                ds.Clear();
                ds.Columns.Add("USER_ID", typeof(Int32));
                ds.Columns.Add("branchCode", typeof(string));
                ds.Columns.Add("createdBy", typeof(Int32));
                ds.Columns.Add("createdOn", typeof(DateTime));
                ds.Columns.Add("userName", typeof(string));
                ds.Columns.Add("riderCode", typeof(string));
                ds.Columns.Add("roleID", typeof(Int32));


                SqlDataAdapter oda = new SqlDataAdapter(cmd);
                oda.Fill(ds);

            }
            catch (Exception Err)
            {
                //return output;
                throw Err;
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
        public Boolean AttendanceIn(int riderCode)
        {
            bool resp = false;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "App_dlv_AppMarkRiderPresent_proc";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@riderCode", riderCode);

                cmd.Parameters.Add("@finalResult", SqlDbType.NVarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                string response = cmd.Parameters["@finalResult"].SqlValue.ToString();
                string[] responseRecords = response.Split(';');

                if (responseRecords[0].ToString() == "1")
                {
                    resp = true;
                }
                else
                {
                    resp = false;
                }

            }
            catch (Exception ex)
            {
                resp = false;
            }
            finally { con.Close(); }



            return resp;
        }
        public Boolean IsActiveIn(int riderCode)
        {
            bool resp = false;

            try
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select IsActive from App_Users where USER_ID = " + riderCode + "");
                cmd1.Connection = con;
                DataTable ds = new DataTable();
                SqlDataAdapter oda = new SqlDataAdapter(cmd1);
                oda.Fill(ds);
                cmd1.ExecuteNonQuery();
                con.Close();
                if (ds.Rows[0]["IsActive"].ToString() == "True")
                {
                    resp = false;
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE App_Users SET IsActive = '1' where USER_ID = " + riderCode + " and isNull(isActive,0) = '0'");
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    resp = true;

                }


            }
            catch (Exception ex)
            {
                resp = false;
            }
            finally { con.Close(); }



            return resp;
        }
        public int getUserIdFromRiderCode(int createdBy)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetUserIdFromRiderCode";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@createdby", createdBy);

                cmd.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int response = int.Parse(cmd.Parameters["@UserId"].SqlValue.ToString());

                return response;
            }
            catch (Exception er)
            {
                return 0;
            }
        }
        public Tuple<bool, string> AttendanceOut(int riderCode)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "App_dlv_AppMarkUserLogout_proc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@riderCode", riderCode);

                cmd.Parameters.Add("@finalResult", SqlDbType.NVarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                string response = cmd.Parameters["@finalResult"].SqlValue.ToString();
                string[] responseRecords = response.Split(';');

                if (responseRecords[0].ToString() == "1")
                {

                    resp = new Tuple<bool, string>(true, "Attandance Updated Logged Out");
                }
                else
                {
                    resp = new Tuple<bool, string>(false, responseRecords[1].ToString());
                }

            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }



            return resp;
        }
        public Tuple<bool, string> UpdateStatus(int riderCode)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            try
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select IsActive from App_Users where USER_ID = " + riderCode + "");
                cmd1.Connection = con;
                DataTable ds = new DataTable();
                SqlDataAdapter oda = new SqlDataAdapter(cmd1);
                oda.Fill(ds);
                cmd1.ExecuteNonQuery();
                con.Close();
                if (ds.Rows[0]["IsActive"].ToString() == "False")
                {
                    resp = new Tuple<bool, string>(false, "");
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE App_Users SET IsActive = '0' where USER_ID = " + riderCode + " and IsActive = '1'");
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    resp = new Tuple<bool, string>(false, "");
                }
            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }



            return resp;
        }
        public Tuple<bool, string> AbsentMarkAttendance(int riderCode)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "App_dlv_AppMarkUserAbsent_proc";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@riderCode", riderCode);

                cmd.Parameters.Add("@finalResult", SqlDbType.NVarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                string response = cmd.Parameters["@finalResult"].SqlValue.ToString();
                string[] responseRecords = response.Split(';');

                if (responseRecords[0].ToString() == "1")
                {

                    resp = new Tuple<bool, string>(true, "Attandance Updated Absent");
                }
                else
                {
                    resp = new Tuple<bool, string>(false, responseRecords[1].ToString());
                }

            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { con.Close(); }



            return resp;
        }
        //public Tuple<bool, string> insertNewRider(AccountDB_v5 acc)
        //{
        //    {
        //        Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

        //        try
        //        {
        //            con.Open();
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.Connection = con;
        //            cmd.CommandText = "Select";
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Parameters.AddWithValue("@RiderCode", acc.RiderCode);
        //            cmd.Parameters.AddWithValue("@password", acc.password);
        //            cmd.Parameters.AddWithValue("@branchCode", acc.branchCode);
        //            cmd.Parameters.AddWithValue("@RiderContact", acc.RiderContact);
        //            cmd.Parameters.AddWithValue("@RoleId", acc.RoleId);
        //            cmd.Parameters.AddWithValue("@status", acc.Status);


        //            cmd.Parameters.Add("@finalResult", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
        //            cmd.ExecuteNonQuery();

        //            string response = cmd.Parameters["@finalResult"].SqlValue.ToString();

        //            resp = new Tuple<bool, string>(true, response);

        //        }
        //        catch (Exception ex)
        //        {
        //            resp = new Tuple<bool, string>(false, ex.Message);
        //        }
        //        finally { con.Close(); }

        //        return resp;
        //    }

        //}
        public bool is_logged_InDB(string riderCode)
        {
            bool resp = false;
            try
            {
                DataTable ds = new DataTable();
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "App_dlv_IsLoggedIn";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@riderCode", riderCode);
                //cmd.Parameters.AddWithValue("@branchCode", branchcode);
                ds.Clear();
                ds.Columns.Add("id", typeof(Int32));
                ds.Columns.Add("riderCode", typeof(string));
                //ds.Columns.Add("branchcode", typeof(string));

                SqlDataAdapter oda = new SqlDataAdapter(cmd);
                oda.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    resp = true;
                }
                else
                {
                    resp = false;
                }
            }
            catch (Exception ex)
            {
                resp = false;
            }
            finally { con.Close(); }
            return resp;
        }
        public async Task<bool> ChangePass(string ridercode, string pass, string newpass)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(@"update [App_Users] set [PASSWORD] = @newpass where riderCode = @ridercode and [PASSWORD] = @pass;", new { newpass, ridercode, pass });
                con.Close();
                return rs > 0;
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
        public async Task<(string AdminPass1, string AdminPass2)> GetAdminPasswords(string branchCode)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryFirstOrDefaultAsync<(string AdminPass1, string AdminPass2)>(@"select top(1) AdminPass1, AdminPass2 from BranchAdminPasswords where BranchCode = @branchCode;", new { branchCode });
                con.Close();
                return rs;
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
        public async Task<List<DropDownModel>> GetReasons()
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<DropDownModel>(@"select Id as [Value], AttributeValue as [Text] from rvdbo.Lookup where AttributeGroup like '%POD_REASON%' and ACTIVE = 1 and GroupId = 1;");
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<List<DropDownModel>> GetRelations()
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<DropDownModel>(@"select ID as [Value], [Name] as [Text] from ReceiverRelationship where Status = 1");
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
    }
}