using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class DebreifingAllRepo
    {
        SqlConnection con;
        public DebreifingAllRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<List<DebreifingAllModel>> GetData(string branchId)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<DebreifingAllModel>(@"
                                                SELECT
                                                isnull(appdt.Consign_id,0) as ConsignId,
                                                appdt.comments as Comments,
                                                rc.SortOrder,
                                                c.consignmentNumber as CN, 
                                                c.remarks as Remarks,
                                                c.pieces as Pcs,
                                                c.weight as [Weight], 
                                                b.sname as Origin, b2.sname as Destination, 
                                                c.consignee as ConsigneeName,
                                                c.consigneePhoneNo as ConsigneePhone,
                                                c.address as ConsigneeAddress,
                                                isnull(cd.codAmount,0) as CodAmount,
                                                isnull(appdt.rider_comments,'') as RiderComments,
                                                isnull(appdt.pickerPhone_No,0) as PickerPhoneNo,
                                                isnull(appdt.rider_iemi,'') as Imei,
                                                ri.riderCode as RiderCode,
                                                ri.firstName as RiderFName,
                                                ri.lastName as RiderLName,
                                                appdt.picker_name as Receiver,
                                                appdt.relation as Relation, 
                                                isnull(appdt.reason,'') as Reason,
                                                case when appdt.ConsignmentNumber is null then 1 else 0 end as IsPending,
                                                case when appdt.ConsignmentNumber is not null and (appdt.relation is null or appdt.relation = '') then 0 else 1 end IsDelivered, 
                                                isnull(appdt.nic_number,0) as NicNo,
                                                case when appdt.ConsignmentNumber is not null  then CONVERT(VARCHAR, appdt.created_on, 103) else 'null' end as DeliveredDate,
                                                case when appdt.ConsignmentNumber is not null then LTRIM(RIGHT(CONVERT(VARCHAR(20), appdt.created_on, 100), 7)) else 'null' end as DeliveredTime,
                                                appdt.latitude as Lat,
                                                appdt.longitude as Long,
                                                r.runsheetNumber as RunsheetNo,
                                                appdt.performed_on as PerformedOn
                                                FROM Runsheet R
                                                INNER JOIN RunsheetConsignment rc ON  R.runsheetNumber = rc.runsheetNumber AND R.branchCode = rc.branchcode AND R.routeCode = rc.RouteCode
                                                INNER JOIN Consignment c ON C.consignmentNumber = rc.consignmentNumber
                                                INNER JOIN Branches B ON B.branchCode = C.orgin
                                                INNER JOIN Branches b2 ON c.destination = b2.branchCode
                                                LEFT OUTER JOIN CODConsignmentDetail_New cd ON  cd.consignmentNumber = c.consignmentNumber
                                                left join App_Delivery_ConsignmentData appdt on rc.runsheetNumber = appdt.RunSheetNumber and rc.consignmentNumber = appdt.ConsignmentNumber
                                                LEFT OUTER JOIN Riders ri ON r.branchcode = ri.branchId and r.riderCode = ri.riderCode
                                                WHERE r.branchCode = @branchId
                                                and cast(r.runsheetDate as date) = cast(getdate() as date)
                                                and isnull(appdt.verify,0) = 0
                                                and appdt.StatusId = 2
                                                order by rc.SortOrder desc;", new { branchId }, commandTimeout: int.MaxValue);
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
        public async Task<List<DebreifingAllModel>> GetRunsheetData(string branchId, string runsheetNo)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<DebreifingAllModel>(@"
                                                SELECT
                                                isnull(appdt.Consign_id,0) as ConsignId,
                                                appdt.comments as Comments,
                                                rc.SortOrder,
                                                c.consignmentNumber as CN, 
                                                c.remarks as Remarks,
                                                c.pieces as Pcs,
                                                c.weight as [Weight], 
                                                b.sname as Origin, b2.sname as Destination, 
                                                c.consignee as ConsigneeName,
                                                c.consigneePhoneNo as ConsigneePhone,
                                                c.address as ConsigneeAddress,
                                                isnull(cd.codAmount,0) as CodAmount,
                                                isnull(appdt.rider_comments,'') as RiderComments,
                                                cast(appdt.pickerPhone_No as varchar) as PickerPhoneNo,
                                                isnull(appdt.rider_iemi,'') as Imei,
                                                ri.riderCode as RiderCode,
                                                ri.firstName as RiderFName,
                                                ri.lastName as RiderLName,
                                                appdt.picker_name as Receiver,
                                                appdt.relation as Relation, 
                                                isnull(appdt.reason,'') as Reason,
                                                case when appdt.StatusId is null then 1 else 0 end as IsPending,
                                                --case when appdt.ConsignmentNumber is not null and (appdt.relation is null or appdt.relation = '') then 0 else 1 end IsDelivered, 
                                                isnull(appdt.nic_number,0) as NicNo,
                                                appdt.performed_on as PerformedOn,
                                                isnull(appdt.StatusId,0) as StatusId,
                                                isnull(sts.Name,'Pending') as [Status],
                                                appdt.latitude as Lat,
                                                appdt.longitude as Long,
                                                r.runsheetNumber as RunsheetNo,
                                                isnull(appdt.verify,0) Verify,
                                                r.runsheetDate as RunsheetDate,
                                                isnull(zu.Name,'') AS [DBUserName],
                                                appdt.DBOn AS DBOn
                                                FROM Runsheet R
                                                INNER JOIN RunsheetConsignment rc ON  R.runsheetNumber = rc.runsheetNumber AND R.branchCode = rc.branchcode AND R.routeCode = rc.RouteCode
                                                INNER JOIN Consignment c ON C.consignmentNumber = rc.consignmentNumber
                                                INNER JOIN Branches B ON B.branchCode = C.orgin
                                                INNER JOIN Branches b2 ON c.destination = b2.branchCode
                                                LEFT OUTER JOIN CODConsignmentDetail_New cd ON  cd.consignmentNumber = c.consignmentNumber
                                                left join App_Delivery_ConsignmentData appdt on rc.runsheetNumber = appdt.RunSheetNumber and rc.consignmentNumber = appdt.ConsignmentNumber
                                                LEFT OUTER JOIN Riders ri ON r.branchcode = ri.branchId and r.riderCode = ri.riderCode
                                                left join App_Delivery_Status sts on sts.Id = appdt.StatusId
                                                LEFT JOIN dbo.ZNI_USER1 zu ON zu.U_ID = appdt.DBBy
                                                WHERE r.branchCode = @branchId
                                                and r.runsheetNumber = @runsheetNo
                                                order by rc.SortOrder desc;", new { branchId, runsheetNo }, commandTimeout: int.MaxValue);
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
        public async Task<bool> UpdateVerification(int Id, bool IsVerify, string Comment, int uid)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(@"update App_Delivery_ConsignmentData set verify = @IsVerify, comments = @Comment, DBBy = @uid, DBOn = getdate() where Consign_id = @Id;", new { Id, IsVerify, Comment, uid });
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
        public async Task<bool> UpdateReason(int Id, string reason, int uid)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(@"update App_Delivery_ConsignmentData set reason = @reason, DBBy = @uid, DBOn = getdate() where Consign_id = @Id;", new { Id, reason, uid });
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
        public async Task<List<DropDownModel>> GetReasons()
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<DropDownModel>(@"select Id as [Value], AttributeValue as [Text] from rvdbo.Lookup where AttributeGroup like '%POD_REASON%' and ACTIVE = 1 and GroupId = 1;");
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
        public async Task<List<DropDownModel>> GetRelations()
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<DropDownModel>(@"select ID as [Value], [Name] as [Text] from ReceiverRelationship where Status = 1");
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
        public async Task<bool> UpdateRelationAndReceiver(int Id, string Relation, string Receiver, int uid)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(@"update App_Delivery_ConsignmentData set relation = @Relation, picker_name = @Receiver, DBBy = @uid, DBOn = getdate() where Consign_id = @Id;", new { Id, Relation, Receiver, uid });
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
        public async Task<bool> VerifyAll(List<VerifyAllModel> model, int uid)
        {
            try
            {
                var sb = new StringBuilder();
                model.ForEach(x => {
                    sb.AppendLine($"update App_Delivery_ConsignmentData set verify = 1, comments = {(string.IsNullOrEmpty(x.Comment) ? "null" : $"'{x.Comment}'")}, DBBy = {uid}, DBOn = getdate() where Consign_id = {x.Id};");
                });
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(sb.ToString());
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
    }
}