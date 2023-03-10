using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MRaabta.Models;
using System.Configuration;

namespace MRaabta.Repo
{
    public class PendingDataRepo
    {
        SqlConnection con;
        public PendingDataRepo()
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
        public async Task<List<DropDownModel>> GetRiders(string branchCode)
        {
            try
            {
                //string BranchCode = "4";
                var rs = await con.QueryAsync<DropDownModel>(@"SELECT r.riderCode as value, r.riderCode + '-' + r.firstName + ' '+ r.lastName text from riders  r where r.branchId = '" + branchCode + "' order by text");
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
        public async Task<List<DropDownModel>> GetAccounts(string branchCode)
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select accountNo as [Value],[name] as [Text] from CreditClients where branchCode = @branchCode ;", new { @branchCode = branchCode});
                //var rs = await con.QueryAsync<DropDownModel>(@"select id as [Value],[name] as [Text] from CreditClients;");
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

        public async Task<List<dynamic>> GetGrid(DateTime dt, int? RiderCode, string AccountCode)
        {
            try
            {
                var rs = await con.QueryAsync($@"
                    select * from (
                    select pm.PickUp_ID as [PickUpId], cc.accountNo,cc.name ClientName, pm.riderCode, r.firstName + r.lastName riderName, cc.name as LocationName, u.userName, pm.createdOn, (select count(*) from App_PickUpChild pc where pc.pickUp_ID = pm.PickUp_ID and isEntered = 1) as dne, 
                    (select count(*) from App_PickUpChild pc where pc.pickUp_ID = pm.PickUp_ID) as cnt, pm.islock , (select top(1) name from ZNI_USER1 where u_id = pm.lockby) as lockby
                    from [dbo].[App_PickUpMaster] pm
                    inner join App_Users u on u.riderCode = pm.riderCode and u.STATUS = 1
                    left join COD_CustomerLocations cl on cl.locationID = pm.locationID
                    left JOIN CreditClients cc ON  cc.id = pm.locationID
                    INNER JOIN Riders AS r ON u.riderCode = r.riderCode
                    ) as xb
                    where cnt-dne > 0
                    and cast(createdOn as date) = @dt
                    {(!string.IsNullOrEmpty(AccountCode) ? "and accountNo = @accno" : "")}
                    {(RiderCode.HasValue ? "and riderCode = @rcode" : "")};", new { @dt = dt.ToString("yyyy-MM-dd"), @accno = AccountCode, @rcode = RiderCode }, commandTimeout: int.MaxValue);
                if (rs.Count() <= 0)
                {
                    return null;
                }
                else
                {

                    return rs.ToList();
                }
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

        public async Task<List<SearchModel>> GetCountries(string prefix)
        {
            try
            {
                await con.OpenAsync();
                //var rs = await con.QueryAsync<SearchModel>(@"select [cityName] as cityName from cities WHERE [cityName] LIKE ''+@prefix+'%'", new { prefix });
                var rs = await con.QueryAsync<SearchModel>(@"select [cityName] as Text from cities WHERE [cityName] LIKE ''+@prefix+'%'", new { prefix });
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }

        }

        public async Task<List<SearchModel>> GetConsignmentNum(string prefix)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<SearchModel>(@"select distinct ConsigneeContact as Text from App_PickupDataEntry WHERE ConsigneeContact LIKE ''+@prefix+'%'", new { prefix });
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }

        }

        public async Task<List<SearchModel>> GetTowns(string prefix)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<SearchModel>(@"select distinct town as Text from App_PickupDataEntry where town is not null and town LIKE ''+@prefix+'%'", new { prefix });
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }

        }

        public async Task<List<SearchModel>> GetConsignmentDetails(string prefix)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<SearchModel>(@"select distinct ConsigneeName as Text from App_PickupDataEntry  where ConsigneeContact=@contact", new { @contact = prefix });
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }

        }
        public async Task<List<SearchModel>> GetConsignmentAddDetails(string prefix)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<SearchModel>(@"select ConsigneeAddress as Text from App_PickupDataEntry  where ConsigneeContact=@contact", new { @contact = prefix });
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }

        }
    }
}