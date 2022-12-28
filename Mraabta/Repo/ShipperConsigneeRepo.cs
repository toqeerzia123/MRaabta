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
    public class ShipperConsigneeRepo
    {
        SqlConnection con;
        public ShipperConsigneeRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<ConsigneeShipperModel> GetData(int pickUpId, string by)
        {
            try
            {
                await con.OpenAsync();
                //await con.ExecuteAsync(@"update App_PickUpMaster set [IsLock] = 1, lockAt = getdate(), lockBy = '"+by + "' where PickUp_ID = @pickUpId;", new { pickUpId });

                var rs = await con.QueryFirstOrDefaultAsync<ConsigneeShipperModel>(@"Select top(1) pc.consignmentNumber as CNNumber, l.name as ShipperName, 
l.phoneNo as ShipperContact
, l.address  as ShipperAddress, pickupDateTime as CreatedOn, isEntered
from App_PickUpChild pc
left join [dbo].[App_PickUpMaster] pm on pc.pickUp_ID = pm.PickUp_ID
 left join CreditClients l on l.id = pm.locationID
where isnull(IsEntered,0) = 0 and pc.pickUp_ID = @pickUpId order by pickupDateTime", new { pickUpId });
                con.Close();               
                    return rs;



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

        public async Task<bool> AddData(ConsigneeShipperModel model)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(@"update App_PickUpChild set [IsEntered] = 1 where [consignmentNumber] = @CNNumber;
                                                INSERT INTO App_PickupDataEntry([CNNumber],[ConsigneeName],[ConsigneeContact],[ConsigneeAddress],[EnteredBy],[EnteredOn],[isfaulty], [town], [city]) 
                                                VALUES(@CNNumber,@ConsigneeName,@ConsigneeContact,@ConsigneeAddress,@EnteredBy,GETDATE(),@IsFaulty,@Town, @City);", model);
                con.Close();
                return rs > 0;
            }
            catch (SqlException ex)
            {
                con.Close();
                return false;
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }

        public async Task<List<SearchModel>> GetCountries(string prefix)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<SearchModel>(@"select [cityName] as cityName from cities WHERE [cityName] LIKE ''+@prefix+'%'", new { prefix });
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