using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace MRaabta.Repo
{
    public class DebaggingRepo : GeneralRepo
    {
        SqlConnection con;
        public DebaggingRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SetCon(con);
        }
        public async Task OpenAsync() => await con.OpenAsync();
        public void Close()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
        public async Task<bool> IsDebagged(string bagno)
        {
            try
            {
                var query = $@"select COUNT(*) as Total from MnP_Debag where BagNumber = '{bagno}'";
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
        public async Task<BagInfoModel> GetBagInfo(string bagno)
        {
            try
            {
                var query = $@"select
                                b.bagNumber as BagNo,
                                b.origin as OriginId,
                                concat(org.sname,' - ',org.name) as Origin,
                                concat(dest.sname,' - ',dest.name) as Destination,
                                b.totalWeight as TotalWeight,
                                b.sealNo as SealNo,
                                format(b.createdOn,'dd-MMM-yyyy') as Date
                                from Bag b
                                inner join Branches org on org.branchCode = b.origin
                                inner join Branches dest on dest.branchCode = b.destination
                                where b.bagNumber = '{bagno}';
                                select 
                                bm.manifestNumber as ManCN,
                                isnull(bm.reason,'') as Reason,
                                concat(org.sname,' - ',org.name) as Origin,
                                concat(dest.sname,' - ',dest.name) as Destination,
                                m.TotalWeight,
                                m.Pieces,
                                1 as IsMan,
                                case when (bm.statusCode = 0 or bm.statusCode is null) then 6 else bm.statusCode end as Status                                
                                from BagManifest bm
                                inner join Mnp_Manifest m on bm.manifestNumber = m.manifestNumber
                                inner join Branches org on org.branchCode = m.origin
                                inner join Branches dest on dest.branchCode = m.destination
                                where bagNumber = '{bagno}'
                                union
                                select 
                                bcn.outpieceNumber as ManCN,
                                isnull(bcn.reason,'') as Reason,
                                concat(org.sname,' - ',org.name) as Origin,
                                concat(dest.sname,' - ',dest.name) as Destination,
                                bcn.WEIGHT as TotalWeight,
                                bcn.pieces as Pieces,
                                0 as IsMan,                                
                                case when (bcn.statusCode = 0 or bcn.statusCode is null) then 6 else bcn.statusCode end as Status
                                from BagOutpieceAssociation bcn
                                inner join consignment c on bcn.outpieceNumber = c.consignmentNumber
                                inner join Branches org on org.branchCode = c.orgin
                                inner join Branches dest on dest.branchCode = c.destination
                                where bagNumber = '{bagno}';";
                using (var rs = await con.QueryMultipleAsync(query))
                {
                    var baginfo = await rs.ReadFirstOrDefaultAsync<BagInfoModel>();
                    if (baginfo != null)
                        baginfo.Details = await rs.ReadAsync<BagManifestInfoModel>();
                    con.Close();
                    return baginfo;
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
        public async Task<(BagManifestInfoModel data, string msg)> GetExcessManCN(string no, bool isman, int branch)
        {
            try
            {
                BagManifestInfoModel model = null;
                string msg = null;
                if (isman)
                {
                    var query = $@"select 
                                    m.manifestNumber as ManCn,
                                    '' as Reason,
                                    concat(org.sname,' - ',org.name) as Origin,
                                    concat(dest.sname,' - ',dest.name) as Destination,
                                    m.TotalWeight,
                                    m.Pieces as Pieces,
                                    1 as IsMan,
                                    7 as Status
                                    from Mnp_Manifest m
                                    inner join Branches org on m.origin = org.branchCode
                                    inner join Branches dest on m.destination = dest.branchCode
                                    where m.manifestNumber = '{no}';";
                    model = await con.QueryFirstOrDefaultAsync<BagManifestInfoModel>(query);
                    if (model == null)
                        msg = "No Manifest Found";
                }
                else
                {
                    var rs = await GetCN(no, branch, "Debag");
                    if (rs.IsValid == 1)
                    {
                        model = new BagManifestInfoModel
                        {
                            ManCN = rs.CN,
                            Origin = rs.Origin,
                            Destination = rs.Destination,
                            Reason = "",
                            IsMan = false,
                            TotalWeight = rs.Weight,
                            Pieces = rs.Pcs,
                            Status = 7
                        };
                    }
                    else
                    {
                        msg = rs.Msg;
                    }
                }

                return (model, msg);
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
        public async Task<(bool sts, string response)> Save(BagInfoModel model, UserModel u)
        {
            string msg = null;
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $@"insert into MnP_Debag (BagNumber,CreatedBy,CreatedON,TotalWeight,origin,destination,[Date],expressCEnterCode,BranchCode,ZoneCode,SealNo) 
                                values('{model.BagNo}',{u.Uid},getdate(),{model.TotalWeight},{model.OriginId},{u.BranchCode},getdate(),'{u.ExpressCenter}',{u.BranchCode},'{u.ZoneCode}','{model.SealNo}');
                                select SCOPE_IDENTITY() as Id;";

                var id = await con.QueryFirstOrDefaultAsync<long>(query, commandTimeout: int.MaxValue, transaction: trans);

                if (id > 0)
                {
                    foreach (var item in model.Details)
                    {
                        msg = $"Error occured in {(item.IsMan ? "Manifest" : "CN")}# {item.ManCN}";
                        if (item.IsMan)
                        {
                            query = $@"insert into MnP_DebagManifest (DebagID,bagNumber,manifestNumber,createdBy,createdOn,statusCode,reason) values ({id},'{(item.Status == 7 ? "0" : model.BagNo)}','{item.ManCN}',{u.Uid},getdate(),{item.Status},'{item.Reason}');
                                        insert into BulkTrackingData (DocumentNumber,ActivityNumber,StateId,LocationID) values('{id}','{item.ManCN}',6,{u.LocationId});";
                        }
                        else
                        {
                            query = $@"insert into MnP_DebagOutPieces (DebagID,bagNumber,outpieceNumber,statusCode,reason,CreatedBy,CreatedOn,cnPieces,cnWeight) values({id},'{(item.Status == 7 ? "0" : model.BagNo)}','{item.ManCN}',{item.Status},'{item.Reason}',{u.Uid},getdate(),{item.Pieces},{item.TotalWeight});";
                            if (item.Status != 6)
                                query += $@"insert into  ConsignmentsTrackingHistory(consignmentNumber,stateID,currentLocation,bagNumber,transactionTime,reason,DebagID) values('{item.ManCN}',6,'{u.LocationName}','{model.BagNo}',getdate(),'DEBAGGED',{id});";
                        }
                        await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                    }
                }

                trans.Commit();
                con.Close();

                return (true, id.ToString());
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"{msg}. Error is {ex.Message}");
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"{msg}. Error is {ex.Message}");
            }
        }
        public async Task<List<dynamic>> GetDebags(string date, string bc)
        {
            try
            {
                var query = $@"select 
                                db.id as Id,
                                db.BagNumber as BagNo,
                                org.name as Origin,
                                dest.name as Destination,
                                FORMAT(db.CreatedON,'dd-MMM-yyyy HH:mm') as Date,
                                (select count(*) from MnP_DebagManifest where DebagID = db.id) as TotalManifests,
                                (select count(*) from MnP_DebagOutPieces where DebagID = db.id) as TotalCNs
                                from MnP_Debag db
                                inner join Branches org on org.branchCode = db.origin
                                inner join Branches dest on dest.branchCode = db.destination
                                where cast(db.CreatedON as date) = '{date}' and db.BranchCode = {bc} order by db.CreatedON desc;";
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

        public async Task<DebagPrintModel> PrintData(string id)
        {
            try
            {
                var query = $@"select 
                                db.id as Id,
                                db.BagNumber as BagNo,
                                org.name as Origin,
                                dest.name as Destination,
                                FORMAT(db.CreatedON,'dd-MMM-yyyy HH:mm') as CreatedOn,
                                db.TotalWeight
                                from MnP_Debag db
                                inner join Branches org on org.branchCode = db.origin
                                inner join Branches dest on dest.branchCode = db.destination
                                where db.id = {id};
                                select 
                                dbm.manifestNumber as Manifest,
                                org.name as Origin,
                                dest.name as Destination,
                                case when dbm.statusCode = 5 then 'Recieved' when dbm.statusCode = 7 then 'Excess Recieved' else 'Short Recieved' end as Status,
                                m.TotalWeight as Weight,
                                m.Pieces as Pcs,
                                dbm.reason as Remarks
                                from MnP_DebagManifest dbm
                                inner join Mnp_Manifest m on dbm.manifestNumber = m.manifestNumber
                                inner join Branches org on org.branchCode = m.origin
                                inner join Branches dest on dest.branchCode = m.destination
                                where dbm.DebagID = {id};
                                select 
                                dbo.outpieceNumber as CN,
                                org.name as Origin,
                                dest.name as Destination,
                                case when dbo.statusCode = 5 then 'Recieved' when dbo.statusCode = 7 then 'Excess Recieved' else 'Short Recieved' end as Status,
                                dbo.cnWeight as Weight,
                                dbo.cnPieces as Pcs,
                                dbo.reason as Remarks
                                from MnP_DebagOutPieces dbo
                                inner join consignment c on dbo.outpieceNumber = c.consignmentNumber
                                inner join Branches org on org.branchCode = c.orgin
                                inner join Branches dest on dest.branchCode = c.destination
                                where dbo.DebagID = {id};";
                using (var rs = await con.QueryMultipleAsync(query, commandTimeout: int.MaxValue))
                {
                    var debag = await rs.ReadFirstOrDefaultAsync<DebagPrintModel>();
                    debag.Manifests = await rs.ReadAsync<DebagManPrintModel>();
                    debag.CNs = await rs.ReadAsync<DebagCNPrintModel>();
                    con.Close();
                    return debag;
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
    }
}