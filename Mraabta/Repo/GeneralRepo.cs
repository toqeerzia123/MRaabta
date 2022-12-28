using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace MRaabta.Repo
{
    public class GeneralRepo
    {
        SqlConnection con;
        protected void SetCon(SqlConnection _con)
        {
            con = _con;
        }
        public async Task<List<(string Product, int Prefix, int Length, int PrefixLength)>> GetConsignmentsLength()
        {
            try
            {
                var rs = await con.QueryAsync<(string Product, int Prefix, int Length, int PrefixLength)>(@"select Product, Prefix, Length, len(Prefix) as PrefixLength from MnP_ConsignmentLengths where status = 1 and Prefix is not null;", commandTimeout: int.MaxValue);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<List<DropDownModel>> Branches(bool withShortNme = false)
        {
            try
            {
                var query = $@"select 
                            branchCode as Value,
                            {(withShortNme ? "CONCAT(sname,' - ',name) as Text" : "name as Text")}
                            from Branches
                            where [status] = 1
                            order by sname;";
                var rs = await con.QueryAsync<DropDownModel>(query);

                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<List<string>> Services()
        {
            try
            {
                var query = $@"SELECT distinct
                            ServiceTypeName Service
                            FROM ServiceTypes          
                            WHERE  IsIntl = '0'
                            AND [status] = '1'
                            order by serviceTypeName;";
                var rs = await con.QueryAsync<string>(query);

                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<dynamic> GetCN(string cn, int branch, string state, int? dest = null)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryFirstOrDefaultAsync("sp_ValidateCN", new { @ConsignmentNumber = cn, @BrCode = branch, @state = state, @dest = dest }, commandType: System.Data.CommandType.StoredProcedure);
                con.Close();
                return rs;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<List<string>> Products()
        {
            try
            {
                var query = $@"select distinct Products as Product from ServiceTypes_New where [status] = 1;";
                var rs = await con.QueryAsync<string>(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
    }
}