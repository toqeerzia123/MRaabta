using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace MRaabta.Repo
{
    public class EmailRepo
    {
        SqlConnection con;
        public EmailRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }
        public async Task<List<dynamic>> GetData()
        {
            try
            {
                var rs = await con.QueryAsync($@"select * from EmailSetup where SendDate <= getdate();");
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetQueryData(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.CommandTimeout = int.MaxValue;
                sda.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateEmail(int id)
        {
            try
            {
                var rs = await con.ExecuteAsync($@"update EmailSetup set SendDate = DATEADD(hour, Interval, SendDate) where Id = {id};
                                                    insert into EmailSetupLog (EmailId) values({id});");
                return rs > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}