using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class ArrivalRepo
    {
        SqlConnection con;
        public ArrivalRepo()
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
        public async Task<List<DropDownModel>> GetRiders(string branchId)
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>($@"select r.riderCode as [Value], CONCAT(r.riderCode,' ',r.firstName,' ',r.lastName) as [Text] from Riders r where  r.BranchID = '{branchId}' and r.status = 1");
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
        public async Task<List<DropDownModel>> GetServiceTypes()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select s.serviceTypeName as [Text] from ServiceTypes s where s.status = 1;", commandTimeout: int.MaxValue);
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
        public async Task<List<DropDownModel>> GetConsignmentTypes()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select ct.id as [Value],ct.name as [Text] from ConsignmentType ct where status = 1;", commandTimeout: int.MaxValue);
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
        public async Task<List<(string Product, int Prefix, int Length, int PrefixLength)>> GetConsignmentsLength()
        {
            try
            {
                var rs = await con.QueryAsync<(string Product, int Prefix, int Length, int PrefixLength)>(@"select Product, Prefix, Length, len(Prefix) as PrefixLength from MnP_ConsignmentLengths where status = 1 and Prefix is not null;", commandTimeout: int.MaxValue);
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
        public async Task<string> DVRTSPrimaryCheck(string cn)
        {
            try
            {
                var query = $@"select top 1 xb.Msg from(
                                SELECT top(1) 1 AS SNO, 'Consignment already exist in archive database' AS Msg from primaryconsignments
                                where isManual = 1 AND consignmentnumber = '{cn}'
                                union
                                select top(1) 2 AS SNO,
                                case 
                                when Reason = '59' then concat('This CN ',consignmentNumber,' is already Mark as RS-Return to Shipper')
                                when Reason = '123' then concat('This CN ',consignmentNumber,' is already Mark as Delivered')
                                end as Msg
                                from RunsheetConsignment where Reason in ('59','123') and consignmentnumber = '{cn}'
                                union
                                select top(1) 3 AS SNO,
                                concat('Arrived against Arrival Id = ',ArrivalID) as Msg
                                from ArrivalScan_Detail where consignmentnumber = '{cn}') as xb ORDER BY xb.SNO;";
                var rs = await con.QueryFirstOrDefaultAsync<string>(query);
                return rs;
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
        public async Task<(string CN, double Weight, int Pcs, int CodAmount, string Origin)> GetCN(string cn)
        {
            try
            {
                var query = $@"SELECT top(1) c.consignmentNumber as CN, c.weight as [Weight], c.pieces as Pcs, cdn.codAmount as CodAmount, c.orgin as Origin
                                                FROM Consignment c 
                                                LEFT JOIN CODConsignmentDetail_New cdn ON  cdn.consignmentNumber = c.consignmentNumber
                                                WHERE c.consignmentNumber = '{cn}'";
                var rs = await con.QueryFirstOrDefaultAsync<(string CN, double Weight, int Pcs, int CodAmount, string Origin)>(query);
                return rs;
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
        public async Task<(bool isOk, string msg)> InsertArrival(ArrivalModel model)
        {
            try
            {
                await con.OpenAsync();
                var trans = con.BeginTransaction();
                try
                {
                    var rs = await con.QueryFirstOrDefaultAsync<long>($@"INSERT INTO ArrivalScan(BranchCode,OriginExpressCenterCode,RiderCode,[Weight],CreatedOn,CreatedBy,ZoneCode,ExpressCenterCode)
                                                                values('{model.BranchCode}','{model.OriginExpressCenterCode}','{model.RiderCode}','{model.Weight}',getDate(),{model.CreatedBy},'{model.ZoneCode}','{model.ExpressCenterCode}');
                                                                SELECT SCOPE_IDENTITY() as Id;", transaction: trans, commandTimeout: int.MaxValue);

                    if (rs > 0)
                    {
                        foreach (var x in model.ArrivalDetails)
                        {
                            var isScale = x.ConsignmentNumber.Split(',')[1];
                            x.ConsignmentNumber = x.ConsignmentNumber.Split(',')[0];
                            try
                            {
                                x.ArrivalID = rs;
                                x.ConsignmentType = model.CNType;
                                x.ServiceType = model.ServiceType;
                                x.CreatedBy = model.CreatedBy.ToString();
                                x.CreatedOn = DateTime.Now;

                                var arrivalId = await con.QueryFirstOrDefaultAsync<int?>($@"SELECT TOP 1 ad.ArrivalID AS ArrivalId FROM ArrivalScan_Detail ad WHERE ad.consignmentNumber = '{x.ConsignmentNumber}'", transaction: trans);

                                if (arrivalId == null)
                                {
                                    var query = $@"INSERT INTO ArrivalScan_Detail (ArrivalID,consignmentNumber,CreatedOn,CreatedBy,cnWeight,cnPieces,ServiceType,ConsignmentType,SortOrder)
                                                    values(
                                                    {x.ArrivalID},
                                                    '{x.ConsignmentNumber}',
                                                    getdate(),
                                                    '{x.CreatedBy}',
                                                    '{x.Weight}',
                                                    '{x.Pieces}',
                                                    '{x.ServiceType}',
                                                    {x.ConsignmentType},
                                                    {x.SortOrder}
                                                    );
                                        INSERT INTO ConsignmentsTrackingHistory (consignmentNumber,stateID,currentLocation,transactionTime,statusTime,ArrivalID) 
                                                    values(
                                                    '{x.ConsignmentNumber}',
                                                    18,
                                                    '{model.LocationName}',
                                                    getDate(),
                                                    getDate(),
                                                    {x.ArrivalID}
                                                    );
                                        INSERT INTO Consignment_Archive
                                                    SELECT * FROM consignment c WHERE c.cod = '1'  AND c.consignmentNumber = '{x.ConsignmentNumber}';
                                        UPDATE consignment SET isapproved = '1', otherCharges = {isScale}, docPouchNo = weight, WEIGHT = '{x.Weight}', pieces = '{x.Pieces}', accountReceivingDate = getDate() ,riderCode = case when isApproved = 1 then riderCode else '{model.RiderCode}' end, status = '1' WHERE cod = '1' AND consignmentNumber = '{x.ConsignmentNumber}';
                                        UPDATE CODConsignmentDetail_New SET status = '04' WHERE consignmentNumber = '{x.ConsignmentNumber}';
                                        INSERT INTO mnp_smsstatus
                                                    SELECT dbo.RemoveAllSpaces(c.consignmentnumber), c.consigneePhoneNo, 'Dear Customer, Your order from ' + REPLACE(cc1.name, '(COD)', '') + ' has been picked under CN:' + c.consignmentnumber + ' .  Now you can track it on www.mulphilog.com ', '0', GETDATE(), {model.CreatedBy}, NULL, NULL, 'N/A', '0', '', '7', NULL
                                                    FROM   consignment c
                                                    INNER JOIN CODConsignmentDetail_New c2 ON  c2.consignmentNumber = c.consignmentNumber
                                                    INNER JOIN creditclients cc ON  c.creditclientid = cc.id
                                                    INNER JOIN CODUsers cc1 ON  cc1.creditclientid = cc.id
                                                    WHERE c.consignmentnumber = '{x.ConsignmentNumber}' and c.cod = '1' AND allowsms = '1';";
                                    var rs2 = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                                }
                                else
                                {
                                    trans.Rollback();
                                    con.Close();
                                    return (false, $"Consignment {x.ConsignmentNumber} already arrived against Arrival Id = {arrivalId.Value}");
                                }
                            }
                            catch (SqlException ex)
                            {
                                trans.Rollback();
                                con.Close();
                                return (false, $"Consignment #{x.ConsignmentNumber} error : {ex.Message.ToString()}");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                con.Close();
                                return (false, $"Consignment #{x.ConsignmentNumber} error : {ex.Message.ToString()}");
                            }
                        }
                    }

                    trans.Commit();
                    con.Close();
                    return (true, rs.ToString());
                }
                catch (SqlException ex)
                {
                    trans.Rollback();
                    con.Close();
                    return (false, ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    con.Close();
                    return (false, ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                return (false, ex.Message.ToString());
            }
        }
        public async Task<List<PrintArrivalModel>> GetArrivalById(int id)
        {
            try
            {

                await con.OpenAsync();
                var rs = await con.QueryAsync<PrintArrivalModel>($@"select 
                                                    a.Id as ArrivalId,
                                                    a.RiderCode,
                                                    CONCAT(r.firstName,' ',r.lastName) as [RiderName],
                                                    a.CreatedOn as [ArrivaDate],
                                                    ad.consignmentNumber as CN,
                                                    ad.ServiceType,
                                                    ad.cnWeight as [Weight],
                                                    ad.cnPieces as [Pieces]
                                                    from [ArrivalScan] a
                                                    inner join [ArrivalScan_Detail] ad on a.Id = ad.ArrivalID
                                                    inner join [Riders] r on r.riderCode = a.RiderCode and r.branchId = a.BranchCode and r.status = 1 
                                                    where a.id = {id};", commandTimeout: int.MaxValue);
                con.Close();
                return rs.OrderBy(x => x.CN).ToList();
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
        public async Task<List<ViewArrivalModel>> GetArrivalByRider(string rc, string dt)
        {
            try
            {
                var query = $@"select 
                                a.Id as Id,
                                a.RiderCode,
                                cast(a.CreatedOn as date) as [Date], 
                                concat(br.name,' (',br.branchCode,')') as Branch,
                                concat(ec.name,' (',ec.expressCenterCode,')') as ExpressCenter,
                                cast(ad.cnWeight as float) as [Weight]
                                from [ArrivalScan] a
                                inner join ArrivalScan_Detail ad on ad.ArrivalID = a.Id
                                inner join Branches br on br.branchCode = a.BranchCode
                                inner join [Riders] r on r.riderCode = a.RiderCode and r.branchId = a.BranchCode and r.status = 1 
                                inner join ExpressCenters ec on ec.expressCenterCode = r.expressCenterId
                                where r.riderCode = '{rc}' 
                                and cast(a.CreatedOn as date) = '{dt}'
                                order by a.Id desc;";
                await con.OpenAsync();
                var rs = await con.QueryAsync<ViewArrivalModel>(query, commandTimeout: int.MaxValue);
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
    }
}