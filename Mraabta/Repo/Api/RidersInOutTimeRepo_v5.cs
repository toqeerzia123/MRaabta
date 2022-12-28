using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo.Api
{
    public class RidersInOutTimeRepo_v5
    {
        SqlConnection con;
        public RidersInOutTimeRepo_v5()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<bool> InsertData(char Type, DateTime? CourierTime, long UserId)
        {
            try
            {
           
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "INSERT INTO App_Delivery_Riderinout(Type, CourierTime, UserId) " +
                    "VALUES( @Type, @CourierTime, @UserId)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@CourierTime", CourierTime);
               // cmd.Parameters.AddWithValue("@CourierInTime", CourierInTime);
               // cmd.Parameters.AddWithValue("@RiderCode", RiderCode);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                var rs= cmd.ExecuteNonQuery();
                con.Close();
                return rs == 0 ? false : true;
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