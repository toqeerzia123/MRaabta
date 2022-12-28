using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo
{
    public class RedefiningDestinationRepo
    {
        SqlConnection con;
        public RedefiningDestinationRepo()
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
            con.Dispose();
        }

        public async Task<List<DDLModel>> GetBranches()
        {
            try
            {
                var query = @"select b.branchCode as Value , b.name  as Text
                              from branches b
                              left outer join ExpressCenters ec
                              on ec.bid = b.branchCode
                              and ec.Main_EC = '1'
                              where b.status = '1' and b.branchcode not in ('169','314','315','33','334','286','329','328','295','327')
                              order by 2;";
                var rs = await con.QueryAsync<DDLModel>(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<dynamic> GetCNDetail(string cn)
        {
            try
            {
                var query = $@"select d.CN,d.DELIVERED, d.RETURNED from MNP_REDEFINING d where d.CN =LTRIM('{cn}');";
                var cnstatus = await con.QueryFirstOrDefaultAsync<(string CN, int DELIVERED, int RETURNED)>(query);
                var status = cnstatus.CN != null ? cnstatus.DELIVERED == 1 || cnstatus.RETURNED == 1 ? 1 : 4 : 0;
                return status;
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

        public async Task<IEnumerable<dynamic>> GetCNHistory(string cn)
        {
            try
            {
                var query = $@"select c.CN,b.name as Destination,c.CREATEDON,c.CREATEDBY
                               from RedefiningDestinationUpdate c join branches b on b.branchCode=c.destination
                               where c.CN = '{cn}' union
                               select r.CN,b.name as Destination,r.CREATEDON,r.CREATEDBY from RedefiningDestinationUpdateArc r
							   join branches b on b.branchCode=r.Destination
                               where r.CN ='{cn}';";
                var rs = await con.QueryAsync(query);
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

        public async Task<dynamic> GetCNRedefiningDetail(string cn, int userId, string newDestination)
        {
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();
                var newinscheck = 0;
                var query = $@"select Id,c.CN,c.destination,c.CreatedOn as CreatedOn,c.CreatedBy as CreatedBy from RedefiningDestinationUpdate c
                               where c.CN ='{cn}';";
                var rs = await con.QueryFirstOrDefaultAsync<(int Id, string CN, string destination, string CreatedOn, string CreatedBy)>(query, transaction: trans);
                if (rs.CN != null)
                {
                    var insquery = $@"insert into RedefiningDestinationUpdateArc
                                (CN, Destination, CreatedOn, CreatedBy)
                                values(@CN,@destination,@CreatedOn,@userId);";
                    var inscheck = await con.ExecuteAsync(insquery, new { CN = rs.CN, destination = rs.destination, CreatedOn = rs.CreatedOn, userId = rs.CreatedBy }, transaction: trans);

                    if (inscheck > 0)
                    {
                        var delquery = $@"delete from RedefiningDestinationUpdate
                                        where CN=@Id;";
                        var delcheck = await con.ExecuteAsync(delquery, new { Id = rs.CN }, transaction: trans);

                        if (delcheck > 0)
                        {
                            var newinsquery = $@"insert into RedefiningDestinationUpdate
                                             (CN, Destination, CreatedOn, CreatedBy)
                                             values(@CN,@destination,@Date,@userId);";
                            newinscheck = await con.ExecuteAsync(newinsquery, new { CN = cn, destination = newDestination, Date = DateTime.Now, userId = userId }, transaction: trans);
                        }
                    }
                }

                trans.Commit();
                con.Close();
                return newinscheck;
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
        }

        public async Task<int> CheckCNDestination(string cn, string Destination)
        {
            try
            {
                var query = $@"select case when r.Destination='{Destination}' then '0'
                            else '1' END as CheckDest from MNP_REDEFINING d join RedefiningDestinationUpdate r on r.CN=d.CN
							where d.CN ='{cn}';";
                var status = await con.QueryFirstOrDefaultAsync<int>(query);
                return status;
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