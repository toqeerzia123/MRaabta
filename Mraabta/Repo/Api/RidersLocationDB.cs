using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MRaabta.Repo.Api
{
    public class RidersLocationDB
    {
        SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());

        public Tuple<bool, string> InsertRouteTracking(DataTable dt)
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            try
            {
                orcl.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = orcl;
                cmd.CommandText = "APP_InsertRiderTracking";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dt_RiderTracking", dt);


                cmd.Parameters.Add("@finalResult", SqlDbType.NVarChar, 2000).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                string response = cmd.Parameters["@finalResult"].SqlValue.ToString();

                resp = new Tuple<bool, string>(true, response);

            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { orcl.Close(); }

            return resp;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Tuple<bool, string> InsertRiderLocation(DataTable dt)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Tuple<bool, string> resp = new Tuple<bool, string>(false, "");

            try
            {
                orcl.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = orcl;
                cmd.CommandText = "APP_InsertRiderLocation";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dt_RiderLocations", dt);


                cmd.Parameters.Add("@finalResult", SqlDbType.NVarChar, 2000).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                string response = cmd.Parameters["@finalResult"].SqlValue.ToString();

                resp = new Tuple<bool, string>(true, response);

            }
            catch (Exception ex)
            {
                resp = new Tuple<bool, string>(false, ex.Message);
            }
            finally { orcl.Close(); }

            return resp;
        }


        public DataTable GetRiderData(int supervisorID)
        {
            DataTable ds = new DataTable();

            string sqlString = "SELECT r.riderCode,au.User_ID riderID, \n"
           + "       r.phoneNo, \n"
           + "       r.firstName + ' ' + r.lastName     RiderName, \n"
           + "       CASE  \n"
           + "            WHEN aua.isForceLogout IS NULL OR aua.isForceLogout = '1' THEN  \n"
           + "                 'FALSE' \n"
           + "            ELSE 'TRUE' \n"
           + "       END                                RiderAttendance \n"
           + "FROM   App_Users au \n"
           + "       INNER JOIN App_UsersChild auc \n"
           + "            ON  auc.[User_ID] = au.[USER_ID] \n"
           + "       INNER JOIN Riders r \n"
           + "            ON  r.riderCode = au.riderCode \n"
           + "       LEFT JOIN App_User_Attendance aua \n"
           + "            ON  aua.userid = au.[USER_ID] \n"
           + "            AND aua.attDate = CAST(GETDATE() AS date) \n"
           + "WHERE  au.roleID = '1' \n"
           + "       AND auc.SupervisorID = '" + supervisorID + "'";

            try
            {
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);

                orcl.Close();
            }
            catch (Exception Err)
            {
                //return output;
            }
            return ds;
        }
    }
}