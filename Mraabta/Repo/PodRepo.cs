using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class PodRepo
    {
        SqlConnection con;
        public PodRepo()
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
        public async Task<Pod> GetRunsheet(string rs, string branch, int profile)
        {
            try
            {
                var query = $@"select
                                rs.runsheetNumber as RS,
                                r.name as Route,
                                ri.riderCode as RiderCode,
                                concat(ri.firstName,' ',ri.lastName) as Rider,
                                FORMAT(rs.runsheetDate,'yyyy-MM-dd HH:mm:ss') as RSDate,
                                rs.VehicleNumber as Vehicle,
                                isnull(vt.TypeDesc,'') as VehicleType,
                                rs.MeterStart,
                                rs.MeterEnd
                                from Runsheet rs
                                inner join Routes r on r.routeCode = rs.routeCode and r.[status] = 1
                                inner join riders ri on ri.routeCode = r.routeCode and ri.[status] = 1
                                left join Vehicle_Type vt on vt.TypeID = rs.VehicleType
                                where rs.runsheetNumber = '{rs}' and rs.branchCode = {branch} and isnull(rs.IsChief,0) = 0;
                                select                                 
                                rc.consignmentNumber as CN,
                                c.consignee as Consignee,
                                c.orgin as OriginId,
                                org.name as Origin,
                                c.destination as DestinationId,
                                dest.name as Destination,
                                rc.GivenToRider,
                                isnull(format(rc.[time],'HH:mm'),'') as Time,
                                isnull(rc.receivedBy,'') as ReceivedBy,
                                cast(isnull(rc.Reason,0) as nvarchar) as Reason,
                                isnull(rc.Receiver_CNIC,'') as ReceiverCNIC,
                                cast(isnull(rc.Relation,0) as nvarchar) as ReceiverRelation,
                                isnull(rc.Comments,'') as Comments,
                                --case when rc.modifiedOn is not null then cast(1 as bit) else cast(0 as bit) end as IsPOD,
                                case when bc.ConsignmentNumber is not null then cast(1 as bit) else cast(0 as bit) end as IsBypass,
                                0 as [Update],
                                case 
                                when isnull(rc.GivenToRider,'') <> '' then cast(1 as bit)
                                when {profile} = 78 and isnull(rc.Reason,'0') in('59','123','0') then cast(0 as bit)
                                when {profile} <> 78 and isnull(rc.Reason,'0') not in('59','123') then cast(0 as bit)
                                else cast(1 as bit)
                                end as ReadOnly
                                from RunsheetConsignment rc
                                inner join Consignment c on c.consignmentNumber = rc.consignmentNumber
                                inner join Branches org on org.branchCode = c.orgin
                                inner join Branches dest on dest.branchCode = c.destination
                                left join tbl_CODControlBypass bc on bc.ConsignmentNumber = rc.consignmentNumber
                                where rc.runsheetNumber = '{rs}'
                                order by rc.SortOrder;";
                await con.OpenAsync();
                using (var obj = await con.QueryMultipleAsync(query))
                {
                    var model = await obj.ReadFirstOrDefaultAsync<Pod>();
                    if (model != null)
                        model.CNs = await obj.ReadAsync<PodDetails>();
                    con.Close();
                    return model;
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
        public async Task<List<DropDownModel>> Reasons()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select r.Id as Value, r.AttributeValue as Text
                                                                from rvdbo.Lookup r
                                                                where r.AttributeGroup = 'POD_REASON' and r.ACTIVE = 1;");
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
        public async Task<List<DropDownModel>> Relations()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select ID as Value, Name as Text from ReceiverRelationShip r where r.status = 1;");
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
        public async Task<bool> Save(Pod model, UserModel u)
        {
            SqlTransaction trans;
            await con.OpenAsync();
            trans = con.BeginTransaction();
            var rsdate = DateTime.ParseExact(model.RSDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            try
            {
                var query = $@"update Runsheet set MeterStart = {model.MeterStart}, MeterEnd = {model.MeterEnd}, modifiedBy = {u.Uid}, modifiedOn = getdate() where runsheetNumber = '{model.RS}';";
                var rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                foreach (var item in model.CNs)
                {
                    query = $@"update RunsheetConsignment 
                                set modifiedOn = getdate(),
                                modifiedBy = {u.Uid},
                                [time] = {(!string.IsNullOrEmpty(item.Time) ? $"'{rsdate.ToString("yyyy-MM-dd")} {item.Time}'" : "null")},
                                receivedBy = '{item.ReceivedBy}',
                                Comments = '{item.Comments}',
                                Reason = {item.Reason},
                                Receiver_CNIC = '{item.ReceiverCNIC}',
                                Relation = {item.ReceiverRelation},
                                deliveryDate = {(item.Reason == "59" || item.Reason == "123" ? $"'{rsdate.ToString("yyyy-MM-dd")}'" : "null")},
                                DeliveryDateTime = {(item.Reason == "59" || item.Reason == "123" && !string.IsNullOrEmpty(item.Time) ? $"'{rsdate.ToString("yyyy-MM-dd")} {item.Time}'" : "null")},
                                Status = (select top 1 s.Id
                                from rvdbo.Lookup r
                                left join rvdbo.Lookup s on s.AttributeGroup = 'POD_STATUS' and r.AttributeDesc = s.AttributeValue and s.ACTIVE = 1
                                where r.AttributeGroup = 'POD_REASON' and r.ACTIVE = 1 and r.Id = {item.Reason})
                                WHERE runsheetNumber = '{model.RS}' and consignmentNumber = '{item.CN}';
                                insert into ConsignmentsTrackingHistory (consignmentNumber,stateID,currentLocation,runsheetNumber,transactionTime,reason,statusTime)
                                values ('{item.CN}',10,'{u.LocationName}','{model.RS}',getdate(),(select top 1 AttributeDesc
                                from rvdbo.Lookup r
                                where r.AttributeGroup = 'POD_REASON' and r.ACTIVE = 1 and r.Id = {item.Reason}),getdate());
                                update Mnp_ConsignmentOperations 
                                set IsRunsheetAllowed = {(item.Reason == "123" ? 0 : 1)}, 
                                IsDelivered = {(item.Reason == "123" ? 1 : 0)}, 
                                RTO = {(item.Reason == "58" ? 1 : 0)} 
                                where ConsignmentId = '{item.CN}';";
                    rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                }

                trans.Commit();
                Close();
                return rs > 0;
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Close();
                throw ex;
            }
        }
    }
}