using Dapper;
using MRaabta.Files;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MRaabta.Files.TrackingModel;


namespace MRaabta.Repo
{
    public class ComplainRepo : GeneralRepo
    {
        SqlConnection con;
        public ComplainRepo()
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

        #region Complain
        public int GetUserLevel(int uid)
        {
            try
            {
                var query = $@"select EscalationLevel from tbl_UserProfiling t1 where t1.UserId='{uid}' and t1.Status = 1;";
                var rs = con.QueryFirstOrDefault<int>(query);

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
        public async Task<List<DropDownModel>> FilteredBranches(int Uid)
        {
            try
            {
                var query = "";
                var levelquery = $@"select EscalationLevel from tbl_UserProfiling where UserId='{Uid}';";
                int Level = con.QueryFirstOrDefault<int>(levelquery);

                //if (Level == 6 || Level == 7)
                //{

                query = $@"select Distinct b.branchCode as Value, b.name as Text from (
                                select case when BranchId = 0 then b.branchCode else BranchId end branchid
                                from tbl_UserProfiling up
                                left join Branches b on b.zoneCode = up.ZoneId and b.status = 1 and 1 = (case when up.BranchId = 0 then 1 else 0 end)                                
                                {(Level == 6 || Level == 7 ? "" : $"WHERE UserId={Uid} ")}) as xb
                                inner join Branches b on b.branchCode = xb.branchid and b.status = 1 order by Value";

                //query = $@"select Distinct BranchId as Value, AllocatedBranch as Text from tbl_UserProfiling 
                //               where AllocatedBranch not like '%All of Allocated Zone%' 
                //                {(Level == 6 || Level == 7 ? " " : $"and UserId={Uid}")} order by Value;";


                //}
                // else
                // {
                //     query = $@"select t3.branchCode as Value, t3.name as Text from tbl_UserProfiling t1 
                //                join Zones t2 on cast(t2.zoneCode as varchar)=cast(t1.ZoneId as varchar)
                //                join Branches t3 on t3.zoneCode=t2.zoneCode where t2.type=1 and t2.status=1 and t3.status=1 and t1.UserId={Uid}  order by Text;";
                // }

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
        public async Task<List<DropDownModel>> Zones(int Uid)
        {
            try
            {
                var query = "";
                var levelquery = $@"select EscalationLevel from tbl_UserProfiling where UserId='{Uid}';";
                int Level = con.QueryFirstOrDefault<int>(levelquery);

                //if (Level == 6 || Level == 7)
                //{
                query = $@"select distinct ZoneId as Value, AllocatedZone as Text from tbl_UserProfiling 
                           {(Level == 6 || Level == 7 ? "" : $"where UserId={Uid}")} order by Value;";
                //}
                //else
                //{
                //    query = $@"select Distinct ZoneId as Value, AllocatedZone as Text from tbl_UserProfiling where UserId='{Uid}' order by Value;";
                //}

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
        public async Task<List<DropDownModel>> GetBranches(string zoneid, int uid)
        {
            try
            {
                var levelquery = $@"select EscalationLevel from tbl_UserProfiling where UserId='{uid}';";
                int Level = con.QueryFirstOrDefault<int>(levelquery);

                var query = "";
                if (zoneid == "0")
                {
                    query = $@"select Distinct BranchId as Value, AllocatedBranch as Text from tbl_UserProfiling where 
                               {(Level == 6 || Level == 7 ? "" : $" UserId={uid} and")} AllocatedBranch not like '%All of Allocated Zone%' order by Value;";
                }
                else
                {
                    query = $@"select Distinct b.branchCode as Value, b.name as Text from (
                                select case when BranchId = 0 then b.branchCode else BranchId end branchid
                                from tbl_UserProfiling up
                                left join Branches b on b.zoneCode = up.ZoneId and b.status = 1 and 1 = (case when up.BranchId = 0 then 1 else 0 end)
                                where up.ZoneId = {zoneid} 
                                {(Level == 6 || Level == 7 ? "" : $"and UserId={uid} ")}) as xb
                                inner join Branches b on b.branchCode = xb.branchid and b.status = 1 order by Value";

                    //            query = $@"select Distinct T3.branchCode as Value, t3.Name as Text from tbl_UserProfiling t1
                    //                        join Zones t2 on cast(t1.ZoneId as varchar) = cast(t2.zoneCode as varchar)
                    //join Branches T3 on T3.zoneCode = t2.zoneCode
                    //                        where t2.type = 1 and t2.status = 1 and t3.status=1 and
                    //                     {(Level == 6 || Level == 7 ? "" : $"t1.UserId={uid} and")} t1.ZoneId={zoneid} order by Value;";

                }
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
        public async Task<List<ComplainDisplayModel>> ComplainDetail(ComplainModel model, bool Filtered)
        {
            try
            {
                var zones = ""; var branches = "";
                if (model.Zones.Count() > 0)
                {
                    foreach (var item in model.Zones)
                    {
                        zones += "" + item.Value + ",";
                    }
                    zones = zones.Remove(zones.Length - 1);
                }
                if (model.Branches.Count() > 0)
                {
                    foreach (var item in model.Branches)
                    {
                        branches += "" + item.Value + ",";
                    }
                    branches = branches.Remove(branches.Length - 1);
                }
                var query = "";
                if (!Filtered)
                {
                    query = $@"select 
                    case when cs.User_Name is null then 'UnAssigned' else cs.User_Name end AssignedTo,
                    nr.nr_ID as TicketID, z.name AS Zone, br.name as Branch,
                    nr.ConsignmentNum, rn.rn_id, rn.rn_name as RequestNature , rt.rt_name as RequestType,
                    nr.nr_comments as [Description], nr.EsalationLevel as EscLevel,
                    CASE when nr.nr_status = '0' then 'Close' else 'Open' end as RequestStatus,
                    CASE when csd.User_ID != '3176' then 'CS'+' / '+ csd.Name
					else 'Customer' end as [LaunchBy],
                    nr.nr_CreatedDate as [LaunchDate],
					case when FinalResponse=1 then 'Green'  else 
					case when nr.ApplicationUsed = 1 and FinalResponse=0 then 'Yellow' else 'transparent' End End  ColorStatus
                    from CSD_NewRequest nr  inner  join CSD_RequestType rt on 
                    nr.RequestType = rt.rt_ID inner join CSD_RequestNature rn on rt.rn_ID = rn.rn_ID
                    inner join CSD_Users csd on nr.nr_createdBy = csd.User_ID
                    inner join Branches br on  br.branchcode=nr.nr_allocationBranch
					inner join Zones z on cast(z.zoneCode as varchar)=cast(br.zoneCode as varchar)
                    left outer join CSD_Users cs on nr.nr_AssignedTo = cs.User_ID
                    where cast(nr.nr_createddate as DATE) >= '{model.From}' and cast(nr.nr_createddate as DATE) <= '{model.To}'  and                    
                    rn.rn_ID IN(1,2) and z.status=1     and z.type=1 and br.status=1 and z.zoneCode NOT IN  ('diff','LOCAL','SAME')
                    and (rn.rn_name!='Trace' or  nr.InquirerType!='4')
                    {(zones == "" || model.Zones.FirstOrDefault().Value.Equals("0") ? " " : $"and z.zoneCode in ({zones})")}
                    {(branches == "" || model.Branches.FirstOrDefault().Value.Equals("0") ? " " : $"and br.branchCode in ({branches})")}
                    {(model.ConsignmentNo == null ? " " : $"and nr.ConsignmentNum = '{model.ConsignmentNo}'")}
                    {(model.Weight == 0 ? " " : model.Weight == 2 ? $"and nr.weight > 1" : $"and nr.weight <= 1")}
                    {(model.Status == 0 ? " " : model.Status == 2 ? $"and nr.nr_status = '0'" : $"and nr.nr_status != '0'")}
                    {(model.OPSStatus == 0 ? " " : model.OPSStatus == 1 ? $"and ISNULL(nr.ApplicationUsed, 0) = 0 and ISNULL(nr.FinalResponse, 0) = 0 " : model.OPSStatus == 2 ? $"and nr.ApplicationUsed = 1 and nr.FinalResponse=0" :  $"and nr.FinalResponse = '1'")}
                    {(model.Escalation == 8 ? " " : $"and Cast(nr.EsalationLevel as Int) = '{model.Escalation}'")}  
                    order by Convert(Int,nr.EsalationLevel) desc,nr.nr_CreatedDate desc;; ";
                }
                else
                {
                    query = $@"select 
                    case when cs.User_Name is null then 'UnAssigned' else cs.User_Name end AssignedTo,
                    nr.nr_ID as TicketID, z.name AS Zone, br.name as Branch,
                    nr.ConsignmentNum, rn.rn_id, rn.rn_name as RequestNature , rt.rt_name as RequestType,
                    nr.nr_comments as [Description], nr.EsalationLevel as EscLevel,
                    CASE when nr.nr_status = '0' then 'Close' else 'Open' end as RequestStatus,
                    CASE when csd.User_ID != '3176' then 'CS'+' / '+ csd.Name
					else 'Customer' end as [LaunchBy],
                    nr.nr_CreatedDate as [LaunchDate],
					case when FinalResponse=1 then 'Green'  else  
					case when nr.ApplicationUsed = 1 and FinalResponse=0 then 'Yellow' else 'transparent' End End  ColorStatus
                    from CSD_NewRequest nr  inner  join CSD_RequestType rt on 
                    nr.RequestType = rt.rt_ID inner join CSD_RequestNature rn on rt.rn_ID = rn.rn_ID
                    inner join CSD_Users csd on nr.nr_createdBy = csd.User_ID
                    inner join Branches br on  br.branchcode=nr.nr_allocationBranch
					inner join Zones z on cast(z.zoneCode as varchar)=cast(br.zoneCode as varchar)
                    left outer join CSD_Users cs on nr.nr_AssignedTo = cs.User_ID
                    where nr.nr_status != '0' and rn.rn_ID IN(1,2) and z.status=1  and z.type=1 and br.status=1 and z.zoneCode NOT IN  ('diff','LOCAL','SAME')
                    and (rn.rn_name!='Trace' or  nr.InquirerType!='4')
                    {(zones == "" ? " " : $"and nr.nr_allocationZone in ({zones})")}
                    {(branches == "" ? " " : $"and nr.nr_allocationBranch in ({branches})")}
                    { (model.Escalation == 8 ? " " : $"and Cast(nr.EsalationLevel as Int) >= '{model.Escalation}' ")} 
                    order by Convert(Int,nr.EsalationLevel) desc,nr.nr_CreatedDate desc;";
                }

                var rs = await con.QueryAsync<ComplainDisplayModel>(query);
                con.Close();
                return rs.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        #region Complain Detail
        public dynamic CurrentTrackingStatus(string CN)
        {
            try
            {
                RestApiConnection rac = new RestApiConnection();
                string Username = "test_user";
                string password = "12345";
                string url = "Tracking/Consignment_Tracking?Username=" + Username + "&password=" + password + "&consignment=" + CN + "";
                IEnumerable<result_batch_updated> resp = rac.getAPI_trackingAsync(url);
                result_batch_updated result = null;
                OrderStatus_New_updated tr_details = null;
                if (resp.Count() > 0)
                {
                    foreach (result_batch_updated a in resp)
                    {
                        result = a;
                        break;
                    }
                    if (Convert.ToBoolean(result.isSuccess))
                    {
                        tr_details = result.tracking_Details[0];
                    }
                }
                return tr_details;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<int> GetId(string CN)
        {
            try
            {
                var query = $@"select rn_ID from CSD_NewRequest where ConsignmentNum = '{CN}';";
                var rs = await con.QueryFirstOrDefaultAsync<int>(query);

                return rs;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<dynamic>> ComplainHistory(string CN, int ReqId)
        {
            try
            {
                var query = $@"select
					case when nr.ApplicationUsed = 1 then d.Department+' / '+ Convert(varchar, z.Name) else 
					case when nr.nr_modifiedBy != 3176 then  'CS'+' / '+ Convert(varchar, csd.Name)
					else 'CUSTOMER' End End as Name, format(nr.nr_modifiedDate,'dd-MMM-yyyy') as CreatedTime,
					case when nr.FileUpload is null or nr.FileUpload = '' then case when nr.nr_comments is not null then nr.nr_comments
					else ''	end	else nr.FileUpload end as Message,
					case when nr.FileUpload is not null and nr.FileUpload != '' then 'pdf' else 'Message' end as MessageType
                    from CSD_NewRequest_history nr left join ZNI_USER1 z on z.U_ID=nr.nr_modifiedBy					
					inner join Department d on d.DepartmentID=z.department					
					left join CSD_Users csd on csd.User_ID=nr.nr_modifiedBy
                    where 1=1  and nr.ConsignmentNum = '{CN}' and nr.nr_ID = '{ReqId}' order by  nr.nr_modifiedDate desc ;";
                var rs = await con.QueryAsync<dynamic>(query);

                return rs.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<dynamic> CNDetail(string CN)
        {
            try
            {
                var query = $@"SELECT   c.*,ob.name OriginName,ob.branchCode OriginCode,od.name destinationName,od.branchCode DestinationCode,oz.name OriginZoneName,oz.zoneCode OriginZoneCode, n.orderRefNo , cc.phoneno as ShipperContactNo ,cc.email as ShipperEmail
                           FROM   Cod_Consignment c
						   inner join CODConsignmentDetail_New n on c.consignmentNumber = n.consignmentNumber 
                           INNER JOIN Branches ob ON ob.branchCode=c.orgin
                           INNER JOIN Branches od ON od.branchCode=c.destination 
                           Inner Join CreditClients cc on cc.id = c.creditClientId
                           INNER JOIN zones oz ON oz.zoneCode=ob.zoneCode
                           where c.ConsignmentNumber = '{CN}';";
                var rs = await con.QueryFirstOrDefaultAsync<dynamic>(query);

                return rs.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<dynamic>> ComplainTicketDetail(string CN, int ReqId)
        {
            try
            {
                var query = $@"select case when nr_status = '0' then 'Close' else 'Open' end as ticketStatus,rn.rn_name as RequestNature , rt.rt_name 
                as Request_Type, c.orderRefNo, CONVERT(VARCHAR(10), nr.nr_CreatedDate, 103) + ' '  + convert(VARCHAR(8),nr.nr_CreatedDate, 14) as CreatedOn,
                CASE when csd.User_ID!='3176' then 'M&P Agent' else 'Customer' end as LaunchBy, nr.consigneeAddress Address,
                nr.nr_ID as Ticket_Id,nr.EsalationLevel, nr.ConsignmentNum, cast(ISNULL(nr.FinalResponse,0) as bit) as FinalResponse
                from CSD_NewRequest nr
                left outer join CODConsignmentDetail_New c on c.consignmentNumber=nr.ConsignmentNum
				inner  join CSD_RequestType rt on nr.RequestType = rt.rt_ID 
				inner join CSD_RequestNature rn on rt.rn_ID = rn.rn_ID
                inner join CSD_Users csd on nr.nr_createdBy = csd.User_ID 
                where 1=1  and nr.ConsignmentNum = '{CN}' and nr.nr_ID = '{ReqId}' order by nr.nr_createdBy;";
                var rs = await con.QueryAsync<dynamic>(query);

                return rs.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> AddRemarks(string CN, UserModel u, int ReqId, string Remarks, bool? FinalResponse = false, string _File = "")
        {
            SqlTransaction trans = null;
            long inscheck = 0;
            var updcheck = 0;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();
                if (_File == "")
                {
                    var updquery = $@"Update CSD_NewRequest
                                SET nr_comments = @val, nr_modifiedBy=@UserId, nr_modifiedDate = GETDATE(),FinalResponse=@FinalResponse,FileUpload=null,ApplicationUsed=1  WHERE ConsignmentNum='{CN}';";
                    updcheck = await con.ExecuteAsync(updquery, new { val = Remarks, UserId = u.Uid, FinalResponse = FinalResponse }, transaction: trans);
                    // ApplicationUsed = 1    Entry from MRaabta
                }
                else if (_File != "")
                {
                    var updquery = $@"Update CSD_NewRequest
                                SET nr_modifiedBy=@UserId, nr_modifiedDate = GETDATE(),FileUpload=@File, ApplicationUsed=1  WHERE ConsignmentNum='{CN}';";
                    updcheck = await con.ExecuteAsync(updquery, new { UserId = u.Uid, File = _File }, transaction: trans);
                }
                if (updcheck > 0)
                {
                    var insquery = $@"Insert into CSD_NewRequest_history
                                      Select * from CSD_NewRequest WHERE ConsignmentNum='{CN}';
                                      SELECT SCOPE_IDENTITY() as Id;";
                    inscheck = await con.QueryFirstOrDefaultAsync<long>(insquery, transaction: trans);
                    if (_File != "")
                    {
                        var updquery1 = $@"Update CSD_NewRequest_history set FileUpload=@File,ApplicationUsed=1  WHERE RequestHistory_Id='{inscheck}';";
                        var t = inscheck + "_" + _File;
                        var updcheck1 = await con.ExecuteAsync(updquery1, new { UserId = u.Uid, File = t }, transaction: trans);
                    }

                }
                trans.Commit();
                con.Close();
                return inscheck;
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
        }
        #endregion
    }
}