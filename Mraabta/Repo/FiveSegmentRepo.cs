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
    public class FiveSegmentRepo : GeneralRepo
    {

        SqlConnection con;
        public FiveSegmentRepo()
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
        public async Task<List<DropDownModel>> FilteredBranches(bool withShortNme = false)
        {
            try
            {
                var query = $@"select 
                            branchCode as Value,
                            {(withShortNme ? "CONCAT(sname,' - ',name) as Text" : "name as Text")}
                            from Branches
                            where [status] = 1 and branchCode in ('43','4','1','34')
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
        public async Task<dynamic> GetCNInfo(string cn)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select outpieceNumber from FSBagConsignments where outpieceNumber = '{cn}';";

                var rs = con.QueryFirstOrDefault(query);
                con.Close();
                return rs;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<DropDownModel>> GetRiders(string branchId, int? rider = null)
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>($@"select r.riderCode as [Value], CONCAT(r.riderCode,' ',r.firstName,' ',r.lastName) as [Text] from Riders r where  r.BranchID = '{branchId}' and r.status = 1
                        {(rider == null ? "" : $"and r.riderCode = '{rider}'")};");
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
        public async Task<List<int>> GetRouteDestination(int dest, string type)
        {
            try
            {
                var query = $@"select distinct Branch from BaggingMisrouteProcedure where Destination = {dest} and [Type] = '{type}' and IsActive = 1;";
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
        public async Task<(bool success, string response)> Insert(FiveSegmentModel model, UserModel u)
        {
            string msg = "";
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();
                long arrID = 0;
                var query = $@"insert into FSBag (bagNumber,createdBy,createdOn,modifiedBy,modifiedOn,totalWeight,transportationType,transportNumber,description,origin,destination,transportDescription,date,Status,expressCenterCode,branchCode,zoneCode,destinationHash,transportationNature,sealNo,bagScreenId,LocationID,Service,[Type],SHS)
                                values('{model.BagNo}',{u.Uid},getdate(),null,null,{model.TotalWeight},0,null,null,{u.BranchCode},{model.Destination},null,'{DateTime.Now.ToString("yyyy-MM-dd")}',null,null,{u.BranchCode},'{u.ZoneCode}',null,null,{(string.IsNullOrEmpty(model.SealNo) ? "null" : $"'{model.SealNo}'")},null,{u.LocationId},{model.Service},'{model.BagType}',{model.SHS});";

                var rs = await con.ExecuteAsync(query, transaction: trans);
                int i = 0;
                foreach (var item in model.FsBagDetails)
                {
                    ++i;
                    msg = $"Error occured in {"Consignment"} {item.ManCN}";
                    foreach (var item1 in model.FsBagDetails.Select(c => c.RiderCode).Distinct())
                    {
                        arrID = await con.QueryFirstOrDefaultAsync<long>($@"INSERT INTO ArrivalScan(BranchCode,OriginExpressCenterCode,RiderCode,[Weight],CreatedOn,CreatedBy,ZoneCode,ExpressCenterCode)
                                                                values('{u.BranchCode}',{u.ExpressCenter},{item.RiderCode},{item.Weight},getdate(),{u.Uid},{u.ZoneCode},{u.ExpressCenter});
                                                                SELECT SCOPE_IDENTITY() as Id;", transaction: trans, commandTimeout: int.MaxValue);
                    }
                    if (arrID != 0)
                    {
                        var arrivalExist = await con.QueryFirstOrDefaultAsync<int?>($@"SELECT TOP 1 ad.ArrivalID AS ArrivalId FROM ArrivalScan_Detail ad WHERE ad.consignmentNumber = '{item.ManCN}'", transaction: trans);

                        if (arrivalExist == null)
                        {
                            query = $@"INSERT INTO ArrivalScan_Detail (ArrivalID,consignmentNumber,CreatedOn,CreatedBy,cnWeight,cnPieces,ServiceType,ConsignmentType,SortOrder)
                                                    values(
                                                    {arrID},
                                                    '{item.ManCN}',
                                                    getdate(),
                                                    '{u.Uid}',
                                                    '{item.Weight}',
                                                    '{item.Pcs}',
                                                    '{model.Service}',
                                                    '{12}',
                                                    {i}
                                                    );
                                        INSERT INTO ConsignmentsTrackingHistory (consignmentNumber,stateID,currentLocation,transactionTime,statusTime,ArrivalID) 
                                                    values(
                                                    '{item.ManCN}',
                                                    18,
                                                    '{u.LocationName}',
                                                    getDate(),
                                                    getDate(),
                                                    {arrID}
                                                    );
                                        INSERT INTO Consignment_Archive
                                                    SELECT * FROM consignment c WHERE c.cod = '1'  AND c.consignmentNumber = '{item.ManCN}';
                                        UPDATE consignment SET isapproved = '1', otherCharges = {0}, docPouchNo = weight, WEIGHT = '{item.Weight}', pieces = '{item.Pcs}', accountReceivingDate = getDate(), riderCode = '{item.RiderCode}', status = '1'
                                                    WHERE cod = '1' AND consignmentNumber = '{item.ManCN}';
                                        UPDATE CODConsignmentDetail_New SET status = '04' WHERE consignmentNumber = '{item.ManCN}';
                                        INSERT INTO mnp_smsstatus
                                                    SELECT dbo.RemoveAllSpaces(c.consignmentnumber), c.consigneePhoneNo, 'Dear Customer, Your order from ' + REPLACE(cc1.name, '(COD)', '') + ' has been picked under CN:' + c.consignmentnumber + ' .  Now you can track it on www.mulphilog.com ', '0', GETDATE(), {u.Uid}, NULL, NULL, 'N/A', '0', '', '7', NULL
                                                    FROM   consignment c
                                                    INNER JOIN CODConsignmentDetail_New c2 ON  c2.consignmentNumber = c.consignmentNumber
                                                    INNER JOIN creditclients cc ON  c.creditclientid = cc.id
                                                    INNER JOIN CODUsers cc1 ON  cc1.creditclientid = cc.id
                                                    WHERE c.consignmentnumber = '{item.ManCN}' and c.cod = '1' AND allowsms = '1';";
                            var rs2 = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                            query = $@"insert into FSBagConsignments(bagNumber, outpieceNumber, statusCode, reason, pieces, Remarks, createdOn, WEIGHT, ismerged, RiderCode)
                                    values('{model.BagNo}', '{item.ManCN}', 6, null,{ item.Pcs},'{item.Remarks}',getdate(),{ item.Weight},null,{ item.RiderCode});";
                        }
                    }
                    if (item.AccountNo == "4D1")
                    {
                        query += $@"update consignment set destination = {item.DestinationId} where consignmentNumber = '{item.ManCN}' and  consignerAccountNo = '4D1';";
                    }

                    rs = await con.ExecuteAsync(query, transaction: trans);

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
        public async Task<(bool success, string response)> Update(FiveSegmentModel model, UserModel u)
        {
            string msg = "";
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $@"update FSBag set totalWeight = {model.TotalWeight}, sealNo = {(string.IsNullOrEmpty(model.SealNo) ? "null" : $"'{model.SealNo}'")}, modifiedBy = {u.Uid}, modifiedOn = getdate() where bagNumber = '{model.BagNo}';";
                var rs = await con.ExecuteAsync(query, transaction: trans);

                if (model.FsBagDetails != null)
                {
                    foreach (var item in model.FsBagDetails)
                    {
                        msg = $"Error occured in {"Consignment"} {item.ManCN}";

                        query = $@"insert into  FSBagConsignments (bagNumber,outpieceNumber,statusCode,reason,pieces,Remarks,createdOn,WEIGHT,ismerged,RiderCode)
                                    values('{model.BagNo}','{item.ManCN}',6,null,{item.Pcs},'{item.Remarks}',getdate(),{item.Weight},null,{item.RiderCode});
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
        public async Task<List<dynamic>> SearchBags(DateTime date, bool isBag, string no, string branch, string Rider)
        {
            try
            {
                var query = $@"select Top 1 
                            b.bagNumber as BagNo,
                            b.totalWeight as TotalWeight,
                            org.name as Origin,
                            dest.name as Destination,                            
                            b.sealNo as SealNo,
                            bc.RiderCode,
                            case when isnull(b.sealNo,'') = '' then cast(1 as bit) else cast(0 as bit) end as Edit,
                            format(b.createdOn,'dd-MMM-yyyy') as CreatedOn
                            from FSBag b
                            inner join FSBagConsignments bc on bc.Bagnumber = b.Bagnumber
                            inner join Branches org on org.branchCode = b.origin
                            inner join Branches dest on dest.branchCode = b.destination
                            where cast(b.createdOn as date) = '{date.ToString("yyyy-MM-dd")}' 
                            and b.branchCode = {branch} 
                            {(string.IsNullOrEmpty(Rider) ? "" : $"and bc.riderCode = '{Rider}'")}
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
        public async Task<FiveSegmentPrintModel> PrintData(string bagno, string riderId)
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
                                from FSBag b
                                inner join Branches org on org.branchCode = b.origin
                                inner join Branches dest on dest.branchCode = b.destination
                                inner join ZNI_USER1 u on u.U_ID = b.createdBy
                                left join Services s on s.Id = b.Service
                                INNER join Products p on p.Id = s.ProductId
                                where b.bagNumber = '{bagno}';                               
                                select 
                                bo.outpieceNumber as CN,
								ar.ArrivalID,
                                bo.pieces as Pcs,
                                bo.WEIGHT as Weight,
                                bo.RiderCode,
                                r.firstName as Rider,
                                bo.Remarks,
                                c.consigner as Consigner,
                                c.consignee as Consignee
                                from FSBagConsignments bo
                                left join consignment c on c.consignmentNumber = bo.outpieceNumber
								inner join ArrivalScan_Detail ar on ar.consignmentNumber = bo.outpieceNumber
                                inner join  Riders r on r.riderCode=Convert(varchar, bo.RiderCode)
                                where bo.bagNumber = '{bagno}' and r.riderCode='{riderId}';
                                select count(*) as RiderCount,fs.RiderCode as RiderId,r.firstName from FSBagConsignments fs
								inner join Riders r on r.riderCode=Convert(nvarchar, fs.RiderCode)
                                where bagNumber = '{bagno}' and r.riderCode='{riderId}' Group by fs.RiderCode,r.firstName order by RiderCount desc";
                using (var item = await con.QueryMultipleAsync(query, commandTimeout: int.MaxValue))
                {
                    var rs = await item.ReadFirstOrDefaultAsync<FiveSegmentPrintModel>();
                    rs.CNs = await item.ReadAsync<FiveSegmentPrintCNModel>();
                    rs.CNCount = await item.ReadAsync<FiveSegmentPrintCountModel>();
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
        public async Task<FiveSegmentPrintModel> GetTagPrint(string bagno)
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
                                from FsBag b
                                inner join branches org on org.branchCode = b.branchCode
                                inner join branches dest on dest.branchCode = b.destination
                                left join Services s on s.Id = b.Service
                                inner join Products p on p.Id = s.ProductId
                                where b.bagNumber = '{bagno}';";
                var rs = await con.QueryFirstOrDefaultAsync<FiveSegmentPrintModel>(query);
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
                var query = $@"select count(*) as Total from FsBag where bagNumber = '{bagno}';";
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
        public async Task<(dynamic data, IEnumerable<dynamic> details)> GetDataForEdit(string bagno, string riderId)
        {
            try
            {
                var query = $@" select top 1
                                bagNumber as BNo,
                                br.name as Branch,
                                dest.branchCode as Destination,
                                pr.Id as Product,
                                sv.Id as Service,
                                b.[Type],
                                b.SHS
                                from FsBag b
                                inner join Branches br on b.branchCode = br.branchCode
                                inner join Branches dest on b.destination = dest.branchCode
                                left join Services sv on b.Service = sv.Id
                                left join Products pr on sv.ProductId = pr.Id
                                where b.bagNumber = '{bagno}' and ISNULL(b.sealNo,'') = '' and isnull(b.totalWeight,0) = 0;                                
                                select
                                bo.outpieceNumber as ManCN,
                                c.serviceTypeName as ServiceType,
                                org.sname as Origin,
                                dest.branchCode as DestinationId,
                                dest.sname as Destination,
                                bo.weight as Weight,
                                bo.pieces as Pcs,
                                r.RiderCode +' '+ r.FirstName +' '+r.lastName as Rider,
                                cast(0 as bit) as IsMan,
                                isnull(bo.Remarks,'') as Remarks,
                                c.consignerAccountNo as AccountNo
                                from FSBagConsignments bo
                                inner join Riders r on r.riderCode = CONVERT(varchar, bo.RiderCode)
                                inner join Consignment c on c.consignmentNumber = bo.outpieceNumber
                                inner join Branches org on org.branchCode = c.orgin
                                inner join Branches dest on dest.branchCode = c.destination
                                where bagNumber = '{bagno}' 
                                {(string.IsNullOrEmpty(riderId) ? "" : $"and r.riderCode = '{riderId}'")};";

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