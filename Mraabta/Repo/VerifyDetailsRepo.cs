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
    public class VerifyDetailsRepo
    {
        SqlConnection con;
        public VerifyDetailsRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<ConsigneeShipperModel> GetData(int pickUpId, string by)
        {
            try
            {
                await con.OpenAsync();
                //await con.ExecuteAsync(@"update app_pickupmaster set [islock] = 1, lockat = getdate(), lockby = '" + by + "' where pickup_id = @pickupid;", new { pickUpId });

                //var rs = await con.QueryFirstOrDefaultAsync<ConsigneeShipperModel>(@"Select top(1) pc.consignmentNumber as CNNumber, (select top(1) name from CreditClients where id = l.CreditClientID) as ShipperName, 
                //                                                                    (select top(1) phoneNo from CreditClients where id = l.CreditClientID) as ShipperContact
                //                                                                    , l.locationAddress as ShipperAddress, pickupDateTime as CreatedOn, isEntered
                //                                                                    , de.ConsigneeContact, de.ConsigneeName, de.ConsigneeAddress, de.town, de.city, de.isfaulty, (select top(1) Name from ZNI_USER1 where u_id = de.EnteredBy) as FeedBy, de.EnteredOn as FeedOn
                //                                                                    from App_PickUpChild pc
                //                                                                    inner join [dbo].[App_PickUpMaster] pm on pc.pickUp_ID = pm.PickUp_ID
                //                                                                    inner join COD_CustomerLocations l on l.locationID = pm.locationID
                //                                                                    inner join App_PickupDataEntry de on de.CNNumber = pc.consignmentNumber
                //                                                                    where IsEntered = 1 and isnull(isVerified,0) = 0 and pc.pickUp_ID = @pickUpId order by pickupDateTime;", new { pickUpId });
                var rs = await con.QueryFirstOrDefaultAsync<ConsigneeShipperModel>(@"Select PC.pickUp_ID, pc.consignmentNumber as CNNumber, l.name as ShipperName,
                                                                                    l.phoneNo as ShipperContact
                                                                                    ,l.address  as ShipperAddress, pickupDateTime as CreatedOn, isEntered
                                                                                    , de.ConsigneeContact, de.ConsigneeName, de.ConsigneeAddress, de.town, de.city, de.isfaulty, (select top(1) Name from ZNI_USER1 where u_id = de.EnteredBy) as FeedBy, de.EnteredOn as FeedOn
                                                                                    from App_PickUpChild pc
                                                                                    left join [dbo].[App_PickUpMaster] pm on pc.pickUp_ID = pm.PickUp_ID
                                                                                    left join CreditClients l on l.id = pm.locationID
                                                                                    left join App_PickupDataEntry de on de.CNNumber = pc.consignmentNumber
                                                                                    where IsEntered = 1 and isnull(isVerified,0) = 0 and pc.pickUp_ID = @pickUpId order by pickupDateTime;", new { pickUpId });
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
                var rs = await con.ExecuteAsync(@"update App_PickUpChild set [IsVerified] = 1 where [consignmentNumber] = @CNNumber;
                                                Update App_PickupDataEntry set [ConsigneeName]=@ConsigneeName,[ConsigneeContact]=@ConsigneeContact
                                                ,[ConsigneeAddress]=@ConsigneeAddress,[verifiedby]=@EnteredBy,[VerifiedOn]= getdate(),[isfaulty]=@IsFaulty, 
                                                [town]=@Town, [city]=@City where [CNNumber] = @CNNumber;", model);
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
                var rs = await con.QueryAsync<SearchModel>(@"select [id] as Value, [cityName] as Text from cities WHERE [cityName] LIKE ''+@prefix+'%'", new { prefix });
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
        
        //public async Task<List<SearchModel>> GetCountries(string empName)
        //{
        //    List<string> empResult = new List<string>();

        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandText = "select Top 10 EmployeeName from Employee where EmployeeName LIKE ''+@SearchEmpName+'%'";
        //            cmd.Connection = con;
        //            con.Open();
        //            cmd.Parameters.AddWithValue("@SearchEmpName", empName);
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                empResult.Add(dr["EmployeeName"].ToString());
        //            }
        //            con.Close();
        //            return empResult;
        //        }
        //    }
        //con.OpenAsync();
        //var rs = await con.QueryAsync<DropDownModel>(@"SELECT r.riderCode as value, r.riderCode + '-' + r.firstName + ' '+ r.lastName text from riders  r where r.branchId = '" + BranchCode + "' order by text");
        //return rs.ToList();
        //var rs = await con.QueryAsync<SearchModel>(@"select [id] as Value , [cityName] as Text from cities");
        //var names = con.Products.Where(p => p.Name.Contains(term)).Select(p => p.Name).ToList();
        //con.Close();
        //return rs.ToList();


    }

}



