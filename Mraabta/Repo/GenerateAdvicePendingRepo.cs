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
    public class GenerateAdvicePendingRepo
    {
        SqlConnection con;
        public GenerateAdvicePendingRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
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
        public async Task<List<DropDownModel>> Reasons()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select Id as Value, Name as Text from MNP_NCI_Reasons where [Status] = 1 order by Name;", commandTimeout: int.MaxValue);
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
        public async Task<List<DropDownModel>> ReasonNotes(int id)
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>($@"select Note_Id as Value, Name as Text from MNP_NCI_Note where Reason_Id = {id} and [STATUS] = 1 order by NAME;", commandTimeout: int.MaxValue);
                Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                Close();
                throw ex;
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public async Task<List<DropDownModel>> PhoneStatus()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select Id as Value, NAME as Text from MNP_NCI_CallStatus where [Status] = 1 order by NAME;", commandTimeout: int.MaxValue);
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
        public async Task<List<DropDownModel>> Status()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select Id as Value, NAME as Text from MNP_NCI_CallTrack where [Status] = 1 order by NAME;", commandTimeout: int.MaxValue);
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
        public async Task<List<DropDownModel>> Reattempts()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select ReAttempt_Id as Value, NAME as Text from MNP_NCI_ReAttempt where [STATUS] = 1;", commandTimeout: int.MaxValue);
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
        public async Task<(dynamic cndata, List<dynamic> logdata)> GetCNInfo(string cn)
        {
            try
            {
                var rs = await con.QueryFirstOrDefaultAsync($@"select c.consignmentNumber as CN,
                                                                c.orgin as OriginId,
                                                                org.name as Origin,
                                                                c.destination as DestinationId,
                                                                dest.name as Destination,
                                                                c.consignerAccountNo as AccountNo,
                                                                c.consigner as Consigner,
                                                                FORMAT(c.bookingDate,'dd-MMM-yyyy') as BookingDate,
                                                                c.consignee as Consignee,
                                                                c.consigneePhoneNo as ConsigneePhoneNo,
                                                                c.serviceTypeName as ServiceType,
                                                                c.address as ConsigneeAddress,
                                                                cd.codAmount as CodAmount,
                                                                rc.Reason as CNStatus,
                                                                rc.branchcode as RSBranch,
                                                                nci.CallTrack as NCIStatus,
                                                                ct.name as NCIStatusName,
                                                                case when isnull(c.[status],0) = 9 then cast(1 as bit) else cast(0 as bit) end as IsVoid,
                                                                nci.TicketNo,
                                                                (select count(distinct TicketNo) from MNP_NCI_Request where ConsignmentNumber = '{cn}') as TotalTickets
                                                            from consignment c
                                                                inner join Branches org on org.branchCode = c.orgin
                                                                inner join Branches dest on dest.branchCode = c.destination
                                                                inner join (
                                                            select top 1
                                                                    *
                                                                from RunsheetConsignment
                                                                where consignmentNumber = '{cn}'
                                                                order by createdOn desc
                                                            ) as rc on rc.consignmentNumber = c.consignmentNumber                                                                
                                                                left join MNP_NCI_Request nci on nci.consignmentnumber = c.consignmentnumber and isnull(nci.isLast, 0) = 1
                                                                left join MNP_NCI_CallTrack ct on ct.Id = nci.CallTrack and ct.[STATUS] = 1
                                                                left join CODConsignmentDetail_New cd on cd.consignmentNumber = c.consignmentNumber
                                                            where c.consignmentNumber = '{cn}'", commandTimeout: int.MaxValue);

                var rs2 = await con.QueryAsync($@"select 
                                                case when nci.CallTrack = 1 then 'INITIATE BY DESTINATION' else 'Closed' end as LogStatus,
                                                nci.TicketNo,
                                                r.Name as Reason,
                                                sn.NAME as Notes,
                                                cs.NAME as CallStatus,
                                                nci.CallTime,
                                                ct.NAME as NCIStatus,
                                                ra.NAME as Reattempt,
                                                nci.Comment,
                                                nci.Consignee,
                                                nci.ConsigneeCell,
                                                nci.ConsigneeAddress,
                                                u.U_NAME as CreatedBy,
                                                FORMAT(nci.CreatedOn,'dd-MMM-yyyy HH:mm') as CreatedOn
                                                from MNP_NCI_Request nci
                                                left join MNP_NCI_Reasons r on r.Id = nci.Reason
                                                left join MNP_NCI_Note sn on sn.Reason_Id = r.Id and sn.Note_Id = nci.StandardNotes
                                                left join MNP_NCI_CallStatus cs on cs.Id = nci.CallStatus
                                                left join MNP_NCI_CallTrack ct on ct.Id = nci.CallTrack
                                                left join MNP_NCI_ReAttempt ra on ra.ReAttempt_Id = nci.ReAttempt
                                                left join ZNI_USER1 u on u.U_ID = nci.CreatedBy
                                                where nci.ConsignmentNumber = '{cn}'
                                                order by nci.CreatedOn desc;", commandTimeout: int.MaxValue);

                Close();

                return (rs, rs2.ToList());
            }
            catch (SqlException ex)
            {
                Close();
                throw ex;
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public async Task<(bool isOk, string response)> InsertTicket(TicketModel model, UserModel u)
        {
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();
                string query = null;
                if (model.UserType == UserType.Coordinator)
                {
                    query = $"select count(*) from MNP_NCI_Request where ConsignmentNumber = '{model.CN}' and CallTrack = 3;";
                    var reattemptcount = await con.QueryFirstOrDefaultAsync<int>(query, transaction: trans);
                    if (reattemptcount >= 2)
                    {
                        query = $@"declare @maxid bigint = (select max(cast(isnull(TicketNo,0) as bigint)) from MNP_NCI_Request with (updlock)) + 1;
                                    update MNP_NCI_Request with (Readpast) set isLast = 0 where ConsignmentNumber = '{model.CN}';
                                    insert into MNP_NCI_Request (TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,CreatedBy,CallTime,ReAttempt,isLast) 
                                    values (@maxid,'{model.CN}','{model.Shippper}','{model.AccountNo}','{model.Consignee}','{model.ConsigneeCell}','{model.ConsigneeAddress}',{model.Origin},{model.Destination},{model.Reason},{model.Note},{model.PhoneStatus},{model.Status},'{model.Remarks}',{(model.CN[0] == '5' && model.CN.Length == 15 ? 1 : 0)},getdate(),{u.Uid},'{model.CallingTime.ToString()}',{model.Reattempt},0),
                                           (@maxid,'{model.CN}','{model.Shippper}','{model.AccountNo}','{model.Consignee}','{model.ConsigneeCell}','{model.ConsigneeAddress}',{model.Origin},{model.Destination},{model.Reason},{model.Note},{model.PhoneStatus},2,'{model.Remarks}',{(model.CN[0] == '5' && model.CN.Length == 15 ? 1 : 0)},getdate(),{u.Uid},'{model.CallingTime.ToString()}',{model.Reattempt},1);
                                    insert into ConsignmentsTrackingHistory (consignmentNumber,stateID,transactionTime,currentLocation,reason,internationalRemarks)  
                                    values('{model.CN}',22,getdate(),'{u.LocationName}','NCI',(select top 1 CONCAT('Reason:',' ',Name) from MNP_NCI_Reasons where id = {model.Reason} and [Status] = 1)),
                                          ('{model.CN}',22,getdate(),'{u.LocationName}','NCI',(select top 1 CONCAT('Advise:',' ',Name) from MNP_NCI_CallTrack where id = 2 and [Status] = 1));
                                    select @maxid as Id;";
                    }
                    else
                    {
                        query = $@"declare @maxid bigint = (select max(cast(isnull(TicketNo,0) as bigint)) from MNP_NCI_Request with (updlock)) + 1;
                                    update MNP_NCI_Request with (Readpast) set isLast = 0 where ConsignmentNumber = '{model.CN}';
                                    insert into MNP_NCI_Request (TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,CreatedBy,CallTime,ReAttempt,isLast) 
                                    values (@maxid,'{model.CN}','{model.Shippper}','{model.AccountNo}','{model.Consignee}','{model.ConsigneeCell}','{model.ConsigneeAddress}',{model.Origin},{model.Destination},{model.Reason},{model.Note},{model.PhoneStatus},{model.Status},'{model.Remarks}',{(model.CN[0] == '5' && model.CN.Length == 15 ? 1 : 0)},getdate(),{u.Uid},'{model.CallingTime.ToString()}',{model.Reattempt},1);
                                    insert into ConsignmentsTrackingHistory (consignmentNumber,stateID,transactionTime,currentLocation,reason,internationalRemarks)  
                                    values('{model.CN}',22,getdate(),'{u.LocationName}','NCI',(select top 1 CONCAT('Reason:',' ',Name) from MNP_NCI_Reasons where id = {model.Reason} and [Status] = 1));
                                    select @maxid as Id;";
                    }
                    var rs = await con.QueryFirstOrDefaultAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);
                    model.TicketNo = rs.ToString();
                }
                else
                {
                    query = $@"update MNP_NCI_Request set isLast = 0 where ConsignmentNumber = '{model.CN}';
                                insert into MNP_NCI_Request (TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,CreatedBy,CallTime,ReAttempt,isLast) 
                                values ('{model.TicketNo}','{model.CN}','{model.Shippper}','{model.AccountNo}','{model.Consignee}','{model.ConsigneeCell}','{model.ConsigneeAddress}',{model.Origin},{model.Destination},{model.Reason},{model.Note},{model.PhoneStatus},{model.Status},'{model.Remarks}',{(model.CN[0] == '5' && model.CN.Length == 15 ? 1 : 0)},getdate(),{u.Uid},'{model.CallingTime.ToString()}',{model.Reattempt},1);
                                insert into ConsignmentsTrackingHistory (consignmentNumber,stateID,transactionTime,currentLocation,reason,internationalRemarks)  
                                values('{model.CN}',22,getdate(),'{u.LocationName}','NCI',(select top 1 CONCAT('Advise:',' ',Name) from MNP_NCI_CallTrack where id = {model.Status} and [Status] = 1));";
                    var rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                }

                trans.Commit();
                Close();
                return (true, model.TicketNo);
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                Close();
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Close();
                return (false, ex.Message);
            }
        }
        public async Task<string> GetReasonById(int id)
        {
            try
            {
                var query = $@"select top 1
                                Name
                                from  MNP_NCI_Reasons 
                                where Id = {id} and [Status] = 1";
                await con.OpenAsync();
                var rs = await con.QueryFirstOrDefaultAsync<string>(query);
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
    }
}