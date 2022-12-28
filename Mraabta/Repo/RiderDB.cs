using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MRaabta.Repo
{
    public class RiderDB
    {
        SqlConnection con;
        public RiderDB()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            con.Close();
        }
        public IEnumerable<RiderData> getRiders(string BranchCode)
        {
            List<RiderData> DDL_RIDER = new List<RiderData>();
            RiderData rm = new RiderData();
            BranchCode = "4";
            DataTable ds = new DataTable();
            //string sql = "SELECT r.riderCode, (riderCode + '-' + r.firstName + ' '+ r.lastName) riderName from riders  r where r.branchId = '" + BranchCode + "' order by riderName";
            string sql = "	SELECT r.riderCode , (r.riderCode + '-' + r.firstName + ' '+ r.lastName) as riderName from riders  r     inner join App_Users u on u.riderCode = r.riderCode where r.branchId = '4'  and r.riderCode not like ' %' order by r.riderCode";

            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(sql, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    rm = new RiderData
                    {
                        riderCode = ds.Rows[i]["riderCode"].ToString(),
                        riderName = ds.Rows[i]["riderName"].ToString()
                    };
                    DDL_RIDER.Add(rm);
                }

                con.Close();

            }
            catch (Exception Err)
            {
                Err.Message.ToString();
                rm = new RiderData
                {
                    riderCode = Err.Message.ToString(),
                    riderName = Err.Message.ToString()
                };
                DDL_RIDER.Add(rm);
                //return output;
            }
            finally
            { con.Close(); }

            return DDL_RIDER;
        }

        public async Task<List<DropDownModel>> GetRiders(string BranchCode)
        {
            try
            {
                string sql = @"	SELECT r.riderCode as [Value], (r.riderCode + '-' + r.firstName + ' '+ r.lastName) as [Text] from riders  r     inner join App_Users u on u.riderCode = r.riderCode where r.branchId = @bc  and r.riderCode not like ' %' order by riderCode";
                var rs = await con.QueryAsync<DropDownModel>(sql, new { @bc = BranchCode });
                //var rs = await con.QueryAsync<DropDownModel>(@"SELECT riderCode as [Value], firstName + ' '+ lastName as [Text] from riders  where branchId = @bc  order by [Text];", new { @bc = BranchCode });
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}