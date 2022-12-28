using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRaabta.Repo
{
    public class RunsheetRepo : GeneralRepo
    {
        SqlConnection con;
        public RunsheetRepo()
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
        public async Task<List<DropDownModel>> RunsheeTypes()
        {
            try
            {
                var query = $@"select l.Id as Value, l.Code as Text from Lookup l where l.DropDownName = 'RUNSHEET_TYPE';";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<(string RouteCode, string Route, string LandMark, string RiderCode, string Rider, string Password, string PhoneNo, bool IsChief)>> RouteInfo(string bid)
        {
            try
            {
                var query = $@"select 
                            r.routeCode as RouteCode, 
                            r.name as Route, 
                            r.LandMark,
                            ri.RiderCode,
                            concat(ri.firstName,' ', ri.lastName) as Rider,
                            ri.POD_Password as Password,
                            REPLACE(ri.phoneNo,'-','') as PhoneNo,
                            isnull(ri.IsChief,0) as IsChief
                            from Routes r 
                            inner join Riders ri on r.routeCode = ri.routeCode and r.BID = ri.branchId
                            where r.bid = {bid} and r.status = 1 and ri.status = 1;";
                var rs = await con.QueryAsync<(string RouteCode, string Route, string LandMark, string RiderCode, string Rider, string Password, string PhoneNo, bool IsChief)>(query);
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
        public async Task<List<DropDownModel>> GetVehicleType()
        {
            try
            {
                var query = $@"select TypeID as [Value], TypeDesc as [Text] from Vehicle_Type where status = '1'";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<DropDownModel>> GetVehicles()
        {
            try
            {
                var query = $@"select v.VehicleCode as [Value], v.MakeModel as [Text] from rvdbo.Vehicle v where v.IsActive = 1 order by VehicleId;";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<(bool isOk, string response)> Save(RunsheetModel model)
        {
            bool isOk = false;
            string resp = "";
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var r = await con.QueryFirstOrDefaultAsync(@"MnP_GenerateRunsheet_Number_check", commandType: System.Data.CommandType.StoredProcedure, transaction: trans);

                var query = $@"INSERT INTO Runsheet (runsheetNumber, routeCode, createdBy, createdOn, runsheetDate, branchCode, runsheetType, syncID, MeterStart, MeterEnd, VEHICLENUMBER, VEHICLETYPE,ridercode,expressCenterCode,zoneCode,IsChief)
                                VALUES ('{r.RunsheetNumber}', '{model.RouteCode}',{model.CreatedBy}, GETDATE(),'{model.Date.ToString("yyyy-MM-dd")}',{model.BranchCode},{model.Type}, NEWID(),
                                '{model.MeterStart}', '{model.MeterEnd}', '{model.VehicleNo}', '{model.VehicleType}','{model.RiderCode}', '{model.ECCode}', '{model.ZoneCode}',{(model.IsChief ? "1" : "0")});";
                var rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                if (rs > 0)
                {
                    isOk = true;
                    resp = r.RunsheetNumber;

                    query = $@"INSERT INTO RiderRunsheet (ridercode, runsheetNumber, createdBy, createdOn, expIdTemp)
                                        Values ('{model.RiderCode}', '{r.RunsheetNumber}', {model.CreatedBy}, getdate(), '{model.ECCode}');";
                    await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                    var cns = string.Join(",", model.Details.Select(x => $"'{x.CN}'"));

                    query = $"select * from RUNSHEETCONSIGNMENT  WHERE consignmentNumber IN ({cns}) AND Reason IS NULL";
                    var runsheetCNs = await con.QueryAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                    StringBuilder sb = new StringBuilder();

                    if (runsheetCNs.Any())
                    {

                        foreach (var item in runsheetCNs)
                        {
                            var modelCN = model.Details.FirstOrDefault(x => x.CN == item.consignmentNumber);
                            sb.AppendLine($@"INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber,StateId, CurrentLocation, reason, runsheetNumber, TransactionTime,statusTime, internationalRemarks)
                                        values({item.consignmentNumber}, '10','{model.Location}','UNDELIVERED','{item.runsheetNumber}',GETDATE(),GETDATE(), '');");
                        }

                        sb.AppendLine($"UPDATE RUNSHEETCONSIGNMENT SET REASON = '204', STATUS = '56', GivenToRider = '{model.RiderCode}' WHERE consignmentNumber IN ({cns}) AND Reason IS NULL;");

                        await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);
                        sb.Clear();
                    }


                    foreach (var item in model.Details)
                    {
                        try
                        {
                            sb.AppendLine($@"INSERT INTO RUNSHEETCONSIGNMENT (RUNSHEETNUMBER, CONSIGNMENTNUMBER, CREATEDBY, CREATEDON, STATUS, SORTORDER, BRANCHCODE,ROUTECODE,COD)
                                                Values ('{r.RunsheetNumber}', '{item.CN}', {model.CreatedBy}, getdate(), '56', {item.Sort}, {model.BranchCode}, '{model.RouteCode}', {(item.IsCod ? 1 : 0)});");

                            sb.AppendLine($@"INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber,StateId,CurrentLocation, RiderName,runsheetNumber,TransactionTime, internationalRemarks)
                                                Values('{item.CN}', '8', '{model.Location}', '{model.Rider}','{r.RunsheetNumber}', GETDATE(), '');");

                            await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);
                            sb.Clear();
                        }
                        catch (SqlException ex)
                        {
                            isOk = false;
                            resp = $"Error occured in CN {item.CN}, error is {ex.Message}";
                            break;
                        }
                        catch (Exception ex)
                        {
                            isOk = false;
                            resp = $"Error occured in CN {item.CN}, error is {ex.Message}";
                            break;
                        }
                    }
                }
                else
                {
                    isOk = false;
                    resp = $"Error occured in generating runsheet";
                }

                if (isOk)
                    trans.Commit();
                else
                    trans.Rollback();

                con.Close();

                return (isOk, resp);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
        }
        public async Task<List<RunsheetPrintViewModel>> GetData(long runsheet)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select 
                            r.runsheetNumber as RS,
                            cn.consignmentNumber as CN,
                            rc.SortOrder as Sort,
                            r.runsheetDate as RunsheetDate,
                            br.sname as Branch,
                            ri.riderCode as RiderCode,
                            concat(ri.firstName,'',ri.lastName) as RiderName,
                            rt.routeCode as RouteCode,
                            rt.name as [Route],
                            mrt.Master_Route_Code as RouteTerritoryCode,
                            mrt.Master_Route_Name as RouteTerritory,
                            cn.consignee as Consignee,
                            cn.address as ConsigneeAddress,
                            cn.consigneeCNICNo as ConsigneeCnicNo,
                            org.sname as Origin,
                            dest.sname as Destination,
                            cn.weight as [Weight],
                            cn.pieces as Pieces,
                            vt.TypeDesc as VehicleType,
                            r.VehicleNumber,
                            r.MeterStart,
                            r.MeterEnd,
                            l.Code as RSType,
                            cd.codAmount as CodAmount
                            from Runsheet r
                            inner join RunsheetConsignment rc on r.runsheetNumber = rc.runsheetNumber
                            inner join Consignment cn on cn.consignmentNumber = rc.consignmentNumber
                            inner join Branches org on org.branchCode = cn.orgin
                            inner join Branches dest on dest.branchCode = cn.destination
                            inner join Branches br on r.branchCode = br.branchCode
                            inner join Routes rt on rt.routeCode = r.routeCode and rt.BID = r.branchCode and rt.[status] = 1
                            inner join Riders ri on ri.routeCode = rt.routeCode and ri.branchId = r.branchCode and ri.[status] = 1
                            left join CODConsignmentDetail_New cd on cd.consignmentNumber = cn.consignmentNumber
                            left join App_Delivery_ConsignmentData adcd ON rc.runsheetNumber = adcd.RunSheetNumber AND rc.consignmentNumber = adcd.ConsignmentNumber
                            left join Route_Profile_Master mrt on mrt.Master_Route_Code = rt.RouteTerritory and mrt.BranchCode = rt.BID and mrt.Status = 1
                            left join Vehicle_Type vt on vt.TypeID = r.VehicleType and vt.[STATUS] = 1
                            inner join Lookup l on l.id = r.runsheetType and l.DropDownName = 'RUNSHEET_TYPE'
                            where r.runsheetNumber = '{runsheet}' order by rc.SortOrder;";
                var rs = await con.QueryAsync<RunsheetPrintViewModel>(query);
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
        public async Task<List<dynamic>> SearchRunsheets(string bc, string date, string routeCode)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select 
                                r.runsheetNumber as RunsheetNumber,
                                r.runsheetDate as RunsheetDate,
                                rt.routeCode as RouteCode,
                                (select COUNT(*) from RunsheetConsignment where runsheetNumber = r.runsheetNumber) as RunsheetCount
                                 from runsheet r
                                inner join Routes rt on rt.routeCode = r.routeCode and rt.BID = r.branchCode and rt.[status] = 1
                                where r.branchCode = '{bc}' and r.runsheetDate = '{date}' and rt.routeCode = '{routeCode}' order by r.createdOn desc";
                var rs = await con.QueryAsync(query);
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
        public async Task<bool> GeneratePinAndLog(string branchCode, string riderCode, string pin, string phoneno, string msg, int createdby, int code, string response, int msgid)
        {
            try
            {
                var query = $@"update riders set TempPinCode = '{pin}' where branchId = {branchCode} and riderCode = '{riderCode}';
                                insert into MnP_SmsStatus (Recepient,MessageContent,[STATUS],CreatedOn,CreatedBy,ErrorCode,Error,SMSFormType,responseId)
                                values('{phoneno}','{msg}',1,getdate(),{createdby},{code},'{response}',11,{msgid});";
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(query);
                con.Close();
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

        public async Task<bool> UpdateRiderPass(string branchCode, string riderCode, string pass)
        {
            try
            {
                var query = $@"update Riders set POD_Password = '{pass}' where branchId = '{branchCode}' and riderCode = '{riderCode}';";
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(query);
                con.Close();
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
    }
}