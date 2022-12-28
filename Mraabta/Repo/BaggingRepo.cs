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
    public class BaggingRepo : GeneralRepo
    {
        SqlConnection con;
        public BaggingRepo()
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
        public async Task<dynamic> GetManisfest(string manifestNo)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select 
                                m.manifestNumber as ManCN,
                                m.manifestType as ServiceType,
                                org.sname as Origin,
                                m.destination as DestinationId,
                                dest.sname as Destination,
                                cast(m.TotalWeight as nvarchar) as Weight,
                                m.Pieces as Pcs
                                from Mnp_Manifest m
                                inner join Branches org on org.branchCode = m.branchCode
                                inner join Branches dest on dest.branchCode = m.destination
                                where m.manifestNumber = '{manifestNo}';";
                var rs = await con.QueryFirstOrDefaultAsync(query);
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
        public async Task<List<int>> GetBranchDestinations(int branch, string type)
        {
            try
            {
                var query = $@"select distinct Destination from BaggingMisrouteProcedure where Branch = {branch} and [Type] = '{type}' and IsActive = 1;";
                await con.OpenAsync();
                var rs = await con.QueryAsync<int>(query);
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
        public async Task<(bool success, string response)> Insert(BagModel model, UserModel u)
        {
            string msg = "";
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $@"insert into Bag (bagNumber,createdBy,createdOn,totalWeight,origin,destination,date,branchCode,zoneCode,sealNo,LocationID,Service,[Type],SHS)
                                values('{model.BagNo}',{u.Uid},getdate(),{model.TotalWeight},{u.BranchCode},{model.Destination},'{DateTime.Now.ToString("yyyy-MM-dd")}',{u.BranchCode},'{u.ZoneCode}',{(string.IsNullOrEmpty(model.SealNo) ? "null" : $"'{model.SealNo}'")},{u.LocationId},{model.Service},'{model.BagType}',{model.SHS});";
                var rs = await con.ExecuteAsync(query, transaction: trans);

                foreach (var item in model.BagDetails)
                {
                    msg = $"Error occured in {(item.IsMan ? "Manifest" : "Consignment")} {item.ManCN}";

                    if (item.IsMan)
                    {
                        query = $@"insert into BagManifest (bagNumber,manifestNumber,createdBy,createdOn,pieces,Remarks,weight,statusCode)
                                    values('{model.BagNo}','{item.ManCN}',{u.Uid},getdate(),{item.Pcs},'{item.Remarks}',{item.Weight},6);
                                insert into BulkTrackingData (DocumentNumber,ActivityNumber,StateId,LocationID)
                                values('{item.ManCN}','{model.BagNo}',3,{u.LocationId});";
                        rs = await con.ExecuteAsync(query, transaction: trans);
                    }
                    else
                    {
                        query = $@"insert into BagOutpieceAssociation (bagNumber,outpieceNumber,pieces,Remarks,createdOn,WEIGHT,statusCode,destination)
                                    values('{model.BagNo}','{item.ManCN}',{item.Pcs},'{item.Remarks}',getdate(),{item.Weight},6,{item.DestinationId});
                                insert into ConsignmentsTrackingHistory (consignmentNumber,stateID,currentLocation,bagNumber,transactionTime,statusTime,SealNo)
                                    values ('{item.ManCN}',3,'{u.LocationName}','{model.BagNo}',getdate(),getdate(),'{model.SealNo}');";

                        if (item.AccountNo == "4D1")
                        {
                            query += $@"update consignment set destination = {item.DestinationId} where consignmentNumber = '{item.ManCN}' and  consignerAccountNo = '4D1';";
                        }

                        rs = await con.ExecuteAsync(query, transaction: trans);
                    }
                }

                trans.Commit();
                con.Close();
                return (true, model.BagNo);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"{msg} error: {ex.Message}");
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"{msg} error: {ex.Message}");
            }
        }
        public async Task<(bool success, string response)> Update(BagModel model, UserModel u)
        {
            string msg = "";
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $@"update Bag set totalWeight = {model.TotalWeight}, sealNo = {(string.IsNullOrEmpty(model.SealNo) ? "null" : $"'{model.SealNo}'")}, modifiedBy = {u.Uid}, modifiedOn = getdate() where bagNumber = '{model.BagNo}';";
                var rs = await con.ExecuteAsync(query, transaction: trans);

                if (model.BagDetails != null)
                {
                    foreach (var item in model.BagDetails)
                    {
                        msg = $"Error occured in {(item.IsMan ? "Manifest" : "Consignment")} {item.ManCN}";

                        if (item.IsMan)
                        {
                            query = $@"insert into BagManifest (bagNumber,manifestNumber,createdBy,createdOn,pieces,Remarks,weight,statusCode)
                                    values('{model.BagNo}','{item.ManCN}',{u.Uid},getdate(),{item.Pcs},'{item.Remarks}',{item.Weight},6);
                                insert into BulkTrackingData (DocumentNumber,ActivityNumber,StateId,LocationID)
                                values('{item.ManCN}','{model.BagNo}',3,{u.LocationId});";
                            rs = await con.ExecuteAsync(query, transaction: trans);
                        }
                        else
                        {
                            query = $@"insert into BagOutpieceAssociation (bagNumber,outpieceNumber,pieces,Remarks,createdOn,WEIGHT,statusCode,destination)
                                    values('{model.BagNo}','{item.ManCN}',{item.Pcs},'{item.Remarks}',getdate(),{item.Weight},6,{item.DestinationId});
                                insert into ConsignmentsTrackingHistory (consignmentNumber,stateID,currentLocation,bagNumber,transactionTime,statusTime,SealNo)
                                    values ('{item.ManCN}',3,'{u.LocationName}','{model.BagNo}',getdate(),getdate(),'{model.SealNo}');";

                            if (item.AccountNo == "4D1")
                            {
                                query += $@"update consignment set destination = {item.DestinationId} where consignmentNumber = '{item.ManCN}' and  consignerAccountNo = '4D1';";
                            }

                            rs = await con.ExecuteAsync(query, transaction: trans);
                        }
                    }
                }

                trans.Commit();
                con.Close();
                return (true, model.BagNo);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"{msg} error: {ex.Message}");
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"{msg} error: {ex.Message}");
            }
        }
        public async Task<List<dynamic>> SearchBags(DateTime date, bool isBag, string no, string branch)
        {
            try
            {
                var query = $@"select 
                            b.bagNumber as BagNo,
                            b.totalWeight as TotalWeight,
                            org.name as Origin,
                            dest.name as Destination,
                            b.sealNo as SealNo,
                            case when isnull(b.sealNo,'') = '' then cast(1 as bit) else cast(0 as bit) end as Edit,
                            format(b.createdOn,'dd-MMM-yyyy') as CreatedOn
                            from Bag b
                            inner join Branches org on org.branchCode = b.origin
                            inner join Branches dest on dest.branchCode = b.destination
                            where cast(b.createdOn as date) = '{date.ToString("yyyy-MM-dd")}' 
                            and b.branchCode = {branch}
                            {(!string.IsNullOrEmpty(no) ? isBag ? $"and b.bagNumber = '{no}'" : $"and b.sealNo = '{no}'" : "")};";
                await con.OpenAsync();
                var rs = await con.QueryAsync(query, commandTimeout: int.MaxValue);
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
        public async Task<BagPrintModel> PrintData(string bagno)
        {
            try
            {
                var query = $@"select 
                                b.bagNumber as BagNo,
                                b.totalWeight as TotalWeight,
                                org.name as Origin,
                                dest.name as Destination,
                                b.sealNo as SealNo,
                                p.Name as Product,
                                s.Name as Service,
                                b.[Type],
                                b.SHS,
                                u.Name as CreatedBy,
                                format(b.createdOn,'dd-MMM-yyyy') as CreatedOn
                                from Bag b
                                inner join Branches org on org.branchCode = b.origin
                                inner join Branches dest on dest.branchCode = b.destination
                                inner join ZNI_USER1 u on u.U_ID = b.createdBy
                                left join Services s on s.Id = b.Service
                                INNER join Products p on p.Id = s.ProductId
                                where b.bagNumber = '{bagno}';
                                select 
                                bm.manifestNumber as ManNo,
                                bm.pieces as Pcs,
                                bm.weight as Weight,
                                bm.Remarks,
                                dest.name as Destination
                                from BagManifest bm
                                inner join mnp_manifest m on bm.manifestNumber = m.manifestNumber
                                inner join branches dest on dest.branchCode = m.destination
                                where bm.bagNumber = '{bagno}';
                                select 
                                bo.outpieceNumber as CN,
                                bo.pieces as Pcs,
                                bo.WEIGHT as Weight,
                                bo.Remarks,
                                c.consigner as Consigner,
                                c.consignee as Consignee,
                                dest.name as Destination
                                from BagOutpieceAssociation bo
                                inner join branches dest on dest.branchCode = bo.destination
                                left join consignment c on c.consignmentNumber = bo.outpieceNumber
                                where bo.bagNumber = '{bagno}';";
                using (var item = await con.QueryMultipleAsync(query, commandTimeout: int.MaxValue))
                {
                    var rs = await item.ReadFirstOrDefaultAsync<BagPrintModel>();
                    rs.Manifests = await item.ReadAsync<BagPrintManifestModel>();
                    rs.CNs = await item.ReadAsync<BagPrintCNModel>();
                    return rs;
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
        public async Task<List<dynamic>> GetRoutesData()
        {
            try
            {
                var query = $@"select
                            brp.Id,
                            brp.Destination as DestinationId,
                            concat(dest.sname,' - ',dest.name) as Destination,
                            brp.Branch as BranchId,
                            concat(b.sname,' - ',b.name) as Branch,
                            brp.[Type],
                            u.Name as CreatedBy,
                            format(brp.CreatedOn,'dd-MMM-yyyy') as CreatedOn
                            from BaggingMisrouteProcedure brp
                            inner join Branches b on b.branchCode = brp.Branch
                            inner join Branches dest on dest.branchCode = brp.Destination
                            inner join ZNI_USER1 u on u.U_ID = brp.CreatedBy
                            where brp.IsActive = 1 order by brp.CreatedOn desc;";
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
        public async Task<List<dynamic>> GetFilteredRoutesData(string Dest, string Branch, string Type)
        {
            try
            {
                var query = $@"select
                            brp.Id,
                            brp.Destination as DestinationId,
                            concat(dest.sname,' - ',dest.name) as Destination,
                            brp.Branch as BranchId,
                            concat(b.sname,' - ',b.name) as Branch,
                            brp.[Type],
                            u.Name as CreatedBy,
                            format(brp.CreatedOn,'dd-MMM-yyyy') as CreatedOn
                            from BaggingMisrouteProcedure brp
                            inner join Branches b on b.branchCode = brp.Branch
                            inner join Branches dest on dest.branchCode = brp.Destination
                            inner join ZNI_USER1 u on u.U_ID = brp.CreatedBy
                            where brp.IsActive = 1 
                           {(!string.IsNullOrEmpty(Dest) ? $"and brp.Destination={Dest}" : "")}
                           {(string.IsNullOrEmpty(Branch) || Branch == "undefined" ? "" : $"and brp.Branch={Branch}")}
                           {(!string.IsNullOrEmpty(Type) ? $"and brp.Type='{Type}' " : "")}
                            order by brp.CreatedOn desc;";
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
        public async Task<bool> SaveRoute(int DestinationId, int BranchId, string Type, int uid)
        {
            try
            {
                var query = $@"insert into BaggingMisrouteProcedure (Destination,Branch,[Type],CreatedBy) values({DestinationId},{BranchId},'{Type}',{uid});";
                var rs = await con.ExecuteAsync(query);
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
        public async Task<bool> DeleteRoute(int id)
        {
            try
            {
                var query = $@"delete from BaggingMisrouteProcedure where Id = {id};";
                var rs = await con.ExecuteAsync(query);
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
        public async Task<BagPrintModel> GetTagPrint(string bagno)
        {
            try
            {
                var query = $@"select
                                b.Id,
                                b.bagNumber as BagNo,
                                org.sname as Origin,
                                dest.sname as Destination,
                                b.Type,
                                p.Name as Product,
                                s.Name as Service,
                                b.sealNo as SealNo,
                                b.totalWeight as TotalWeight
                                from Bag b
                                inner join branches org on org.branchCode = b.branchCode
                                inner join branches dest on dest.branchCode = b.destination
                                left join Services s on s.Id = b.Service
                                inner join Products p on p.Id = s.ProductId
                                where b.bagNumber = '{bagno}';";
                var rs = await con.QueryFirstOrDefaultAsync<BagPrintModel>(query);
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
        public async Task<bool> IsBagExists(string bagno)
        {
            try
            {
                var query = $@"select count(*) as Total from Bag where bagNumber = '{bagno}';";
                var rs = await con.QueryFirstOrDefaultAsync<int>(query);
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
        public async Task<(dynamic data, IEnumerable<dynamic> details)> GetDataForEdit(string bagno)
        {
            try
            {
                var query = $@"select top 1
                                bagNumber as BNo,
                                br.name as Branch,
                                dest.branchCode as Destination,
                                pr.Id as Product,
                                sv.Id as Service,
                                b.[Type],
                                b.SHS
                                from Bag b
                                inner join Branches br on b.branchCode = br.branchCode
                                inner join Branches dest on b.destination = dest.branchCode
                                left join Services sv on b.Service = sv.Id
                                left join Products pr on sv.ProductId = pr.Id
                                where b.bagNumber = '{bagno}' and ISNULL(b.sealNo,'') = '' and isnull(b.totalWeight,0) = 0;
                                select 
                                bm.manifestNumber as ManCN,
                                m.manifestType as ServiceType,
                                org.sname as Origin,
                                dest.branchCode as DestinationId,
                                dest.sname as Destination,
                                bm.weight as Weight,
                                bm.pieces as Pcs,
                                cast(1 as bit) as IsMan,                                
                                isnull(bm.Remarks,'') as Remarks,
                                null as AccountNo
                                from BagManifest bm
                                inner join Mnp_Manifest m on bm.manifestNumber = m.manifestNumber
                                inner join Branches org on org.branchCode = m.origin
                                inner join Branches dest on dest.branchCode = m.destination
                                where bagNumber = '{bagno}'
                                union
                                select
                                bo.outpieceNumber as ManCN,
                                c.serviceTypeName as ServiceType,
                                org.sname as Origin,
                                dest.branchCode as DestinationId,
                                dest.sname as Destination,
                                bo.weight as Weight,
                                bo.pieces as Pcs,
                                cast(0 as bit) as IsMan,
                                isnull(bo.Remarks,'') as Remarks,
                                c.consignerAccountNo as AccountNo
                                from BagOutpieceAssociation bo
                                inner join Consignment c on c.consignmentNumber = bo.outpieceNumber
                                inner join Branches org on org.branchCode = c.orgin
                                inner join Branches dest on dest.branchCode = c.destination
                                where bagNumber = '{bagno}';";

                using (var rs = await con.QueryMultipleAsync(query))
                {
                    var data = await rs.ReadFirstOrDefaultAsync();
                    if (data != null)
                    {
                        var list = await rs.ReadAsync();
                        return (data, list);
                    }
                    else
                    {
                        return (data, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}