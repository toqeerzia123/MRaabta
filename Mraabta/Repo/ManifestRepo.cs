using MRaabta.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace MRaabta.Repo
{
    public class ManifestRepo : GeneralRepo
    {
        SqlConnection con;
        public ManifestRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SetCon(con);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
        public async Task<List<DropDownModel>> NewProducts()
        {
            try
            {
                var query = $@"select Id as Value, Name as Text from Products where IsActive = 1;";
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
        public async Task<List<DropDownModel>> NewServices(int id)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select Id as Value, Name as Text from Services where ProductId = {id}";
                var rs = await con.QueryAsync<DropDownModel>(query);
                con.Close();
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
        public async Task<(bool isok, string msg)> Save(ManifestModel model, UserModel u)
        {
            SqlTransaction trans = null;
            var errorcn = "";
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $@"insert into Mnp_Manifest (manifestNumber,manifestType,[date],origin,destination,createdBy,createdOn,zoneCode,branchCode,syncId,LocationID,TotalWeight,Pieces,Service,SHS)
                                values('{model.ManifestNo}','{model.Service}',getdate(),{u.BranchCode},{model.Destination},{u.Uid},getdate(),'{u.ZoneCode}',{u.BranchCode},NEWID(),'{u.LocationId}',{model.TotalWeight},{model.TotalPcs},{model.Service},{model.SHS});";

                var rs = await con.ExecuteAsync(query, transaction: trans, commandTimeout: int.MaxValue);

                foreach (var item in model.Details)
                {
                    errorcn = item.CN;
                    query = $@"insert into Mnp_ConsignmentManifest (consignmentNumber,manifestNumber,statusCode,Remarks,createdOn,WEIGHT,Pieces)
                                values('{item.CN}','{model.ManifestNo}',6,'{item.Remarks}',getdate(),{item.Weight},{item.Pcs});
                                insert into ConsignmentsTrackingHistory(consignmentNumber,stateID,currentLocation,manifestNumber,transactionTime,statusTime)
                                values('{item.CN}',2,'{u.LocationName}','{model.ManifestNo}',getdate(),getdate());";
                    await con.ExecuteAsync(query, transaction: trans, commandTimeout: int.MaxValue);
                }

                trans.Commit();
                con.Close();

                return (true, model.ManifestNo);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"Error occured in CN# {errorcn} error " + ex.Message);
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"Error occured in CN# {errorcn} error " + ex.Message);
            }
        }
        public async Task<bool> IsManifestExists(string manifestno)
        {
            try
            {
                var rs = await con.QueryFirstOrDefaultAsync<int>($"select count(*) as Total from Mnp_Manifest where manifestNumber = '{manifestno}';");
                return rs > 0;
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
        public async Task<List<dynamic>> GetManifests(DateTime date, string branch)
        {
            try
            {
                var query = $@"select 
                            manifestNumber as ManifestNo,
                            p.Name as Product,
                            s.Name as ServiceType,
                            m.SHS,
                            CONCAT(org.sname,' - ',org.name) as Origin,
                            CONCAT(dest.sname,' - ',dest.name) as Destination,
                            case
                            when ISNULL(m.isDemanifested,0) = 1 then cast(1 as bit)
                            when (select top 1 manifestNumber from BagManifest where manifestNumber = m.manifestNumber order by createdOn desc) is not null then cast(1 as bit) 
                            else CAST(0 as bit) end as IsDemanifested,
                            u.Name as CreatedBy
                            from Mnp_Manifest m
                            inner join Services s on s.Id = m.Service
                            inner join Products p on p.Id = s.ProductId
                            inner join Branches org on m.origin = org.branchCode
                            inner join Branches dest on m.destination = dest.branchCode
                            INNER join ZNI_USER1 u on m.createdBy = u.U_ID
                            where cast(m.createdOn as date) = '{date.ToString("yyyy-MM-dd")}' and m.branchCode = {branch}
                            order by m.createdOn desc;";
                var rs = await con.QueryAsync(query, commandTimeout: int.MaxValue);
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
        public async Task<ManifestDataModel> ManifestData(string id)
        {
            try
            {
                var query = $@"select top 1
                                manifestNumber as ManifestNo,
                                p.Id as ProductId,
                                p.Name as Product,
                                s.Id as ServiceTypeId,
                                s.Name as ServiceType,
                                m.SHS,
                                CONCAT(org.sname,' - ',org.name) as Origin,
                                m.destination as DestinationId,
                                CONCAT(dest.sname,' - ',dest.name) as Destination,
                                m.TotalWeight as TotalWeight,
                                m.Pieces as TotalPcs,
                                u.Name as CreatedBy,
                                m.createdOn as CreatedOn
                                from Mnp_Manifest m
                                inner join Services s on s.Id = m.Service
                                inner join Products p on p.Id = s.ProductId
                                inner join Branches org on m.origin = org.branchCode
                                inner join Branches dest on m.destination = dest.branchCode
                                INNER join ZNI_USER1 u on m.createdBy = u.U_ID
                                where m.manifestNumber = '{id}';
                                select 
                                mc.consignmentNumber as CN,
                                c.consigner as Shipper,
                                c.consignee as Consignee,
                                mc.pieces as Pcs,
                                mc.Remarks,
                                c.serviceTypeName as ServiceType,
                                org.name as Origin,
                                dest.name as Destination
                                from Mnp_ConsignmentManifest mc
                                left join consignment c on c.consignmentNumber = mc.consignmentNumber
                                left join Branches org on c.orgin = org.branchCode
                                left join Branches dest on c.destination = dest.branchCode
                                where mc.manifestNumber = '{id}';";
                using (var rs = await con.QueryMultipleAsync(query, commandTimeout: int.MaxValue))
                {
                    var manifest = await rs.ReadFirstOrDefaultAsync<ManifestDataModel>();
                    var details = await rs.ReadAsync<ManifestDataDetailPrintModel>();
                    manifest.ManifestDetail = details.ToList();
                    con.Close();
                    return manifest;
                }
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
        public async Task<(bool isok, string msg)> Update(ManifestModel model, UserModel u)
        {
            SqlTransaction trans = null;
            var errorcn = "";
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $"update Mnp_Manifest set modifiedBy = {u.Uid}, modifiedOn = getdate(), Service = {model.Service}, TotalWeight = {model.TotalWeight} where manifestNumber = '{model.ManifestNo}';";
                await con.ExecuteAsync(query, transaction: trans, commandTimeout: int.MaxValue);

                foreach (var item in model.Details.Where(x => !x.IsOld))
                {
                    errorcn = item.CN;
                    query = $@"insert into Mnp_ConsignmentManifest (consignmentNumber,manifestNumber,statusCode,Remarks,createdOn,WEIGHT,Pieces)
                                values('{item.CN}','{model.ManifestNo}',6,'{item.Remarks}',getdate(),{item.Weight},{item.Pcs});
                                insert into ConsignmentsTrackingHistory(consignmentNumber,stateID,currentLocation,manifestNumber,transactionTime,statusTime)
                                values('{item.CN}',2,'{u.LocationName}','{model.ManifestNo}',getdate(),getdate());";
                    await con.ExecuteAsync(query, transaction: trans, commandTimeout: int.MaxValue);
                }

                trans.Commit();
                con.Close();

                return (true, model.ManifestNo);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"Error occured in CN# {errorcn} error " + ex.Message);
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"Error occured in CN# {errorcn} error " + ex.Message);
            }
        }
    }
}