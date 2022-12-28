using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MRaabta.Models;
using System.Configuration;

namespace MRaabta.Repo
{
    public class PickupRepo
    {
        SqlConnection con;
        public PickupRepo()
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
        public async Task<List<DropDownModel>> GetCouriers()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select [user_Id] as [Value], cast([user_Id] as varchar) + '-' + [userName] as [Text] from App_Users;");
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
        public async Task<List<DropDownModel>> GetLocations(int id)
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select [locationId] as [Value], [locationName] as [Text] from COD_CustomerLocations where CreditclientId = @id;", new { id });
                if (rs.Count() > 0 )
                {
                    return rs.ToList();
                }
                else
                    rs = await con.QueryAsync<DropDownModel>(@"select [Id] as [Value], CAST([id] AS varchar)+'-' + [Name] as [Text] from CreditClients where Id = @id;", new { id });
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
        public async Task<List<DropDownModel>> GetCustomers(string brancahCode)
        
        
        {
            try
            {
                //var rs = await con.QueryAsync<DropDownModel>(@"select [Id] as [Value], CAST([id] AS varchar)+'-' + [Name] as [Text] from CreditClients where id in (select distinct CreditClientID from [dbo].[COD_CustomerLocations] where brancahCode = @brancahCode);", new { @brancahCode = brancahCode });
                var rs = await con.QueryAsync<DropDownModel>(@"select [Id] as [Value], CAST([id] AS varchar)+'-' + [Name] as [Text] from CreditClients where status=1 and branchCode = @brancahCode", new { @brancahCode = brancahCode });
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
        public async Task<List<DropDownModel>> GetPriorities()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"select [Id] as [Value], [priority] as [Text] from App_PickupPriorities;");
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

        public async Task<bool> Delete(PickupRequestModel model)
        {
            try
            {
                var sqlStatement = @"UPDATE App_PickUpRequest
            SET    [STATUS]       = 0,
                   modifiedBy     = @modifiedBy ,
                   modifiedOn     = GETDATE() WHERE id = @id";
                await con.ExecuteAsync(sqlStatement, model);
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(PickupRequestModel model)
        {
            try
            {
                var sqlStatement = @"UPDATE App_PickUpRequest_CourierUpdate
                                SET    
                                       STATUS = 0
                                WHERE  scheduleId             = @id;
                                INSERT INTO App_PickUpRequest_CourierUpdate
                                            (
	                                            -- id -- this column value is auto-generated
	                                            scheduleId,
	                                            scheduleDate,
	                                            [User_ID],
	                                            [STATUS],
	                                            createdBy,
	                                            createdOn
                                            )
                                            VALUES
                                            (
	                                          @id,
	                                          cast(getdate() as date) ,
	                                          @CourierId,
	                                          1,
	                                          @modifiedBy,
	                                          getdate()
                                            );
                                    UPDATE App_PickUpRequest_CourierScheduleUpdate
                                                    SET    STATUS = '0'
                                                    WHERE  USER_ID IN (@DefaultCourierId,@CourierId);   
                                    INSERT INTO App_PickUpRequest_CourierScheduleUpdate
                                           (USER_ID
                                           ,STATUS
                                           ,createdBy
                                           ,createdOn)
                                    VALUES
                                      (@DefaultCourierId,'1',  @modifiedBy,getdate()),
                                    (@CourierId,'1',  @modifiedBy,getdate());";
                await con.ExecuteAsync(sqlStatement, model);
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<PickupRequestModel>> GetRequests(int att)
        {
            try
            {
                string sql = "/************************************************************ \n"
           + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
           + " * Time: 2019-06-10 3:06:02 PM \n"
           + " ************************************************************/ \n"
           + " \n"
           + "SELECT Max(A.id) as id,A.locationID, A.pickupDate, A.pickuptime, A.USERID, A.priority, A.isRoutine, A.STATUS, A.createdBy, A.createdOn, A.modifiedBy, A.modifiedOn, A.DateOrDays,A.Customer,A.Location,A.PriorityName,A.pickuptime, \n"
           + "       cour.userName          AS [Courier] \n"
           + "FROM   ( \n"
           + "           SELECT PR.id, \n"
           + "                  PR.locationID, \n"
           + "                  PR.pickupDate, \n"
           + "                  PR.pickuptime, \n"
           + "                  CASE  \n"
           + "                       WHEN APC.[User_ID] IS NOT NULL THEN APC.[User_ID] \n"
           + "                       ELSE PR.[User_ID] \n"
           + "                  END               AS USERID, \n"
           + "                  PR.priority, \n"
           + "                  PR.isRoutine, \n"
           + "                  PR.comments, \n"
           + "                  PR.STATUS, \n"
           + "                  PR.createdBy, \n"
           + "                  PR.createdOn, \n"
           + "                  PR.modifiedBy, \n"
           + "                  PR.modifiedOn, \n"
           + "                  STUFF( \n"
           + "                      ( \n"
           + "                          SELECT CAST(',' AS VARCHAR(MAX)) + rd.Day \n"
           + "                          FROM   App_PickUpReqDays rd \n"
           + "                          WHERE  rd.PickupRequestId = pr.Id \n"
           + "                          ORDER BY \n"
           + "                                 rd.id \n"
           + "                                 FOR XML PATH('') \n"
           + "                      ), \n"
           + "                      1, \n"
           + "                      1, \n"
           + "                      '' \n"
           + "                  )                 AS [DateOrDays], \n"
           + "                  cus.Name          AS [Customer], \n"
           + "                  cus.Name   AS [Location], \n"
           + "                  prio.Priority     AS [PriorityName], \n"
           + "                  CASE  \n"
           + "                       WHEN pr.IsRoutine = 1 THEN 'Schedule' \n"
           + "                       ELSE 'One Time' \n"
           + "                  END               AS [PickupType] \n"
           + "           FROM   [App_PickUpRequest] pr \n"
           + "                  left JOIN [dbo].[COD_CustomerLocations] loc \n"
           + "                       ON  loc.LocationId = pr.LocationId \n"
           + "                  INNER JOIN [dbo].[CreditClients] cus \n"
           + "                       ON  cus.Id = pr.LocationId \n"
           + "                  INNER JOIN App_PickupPriorities prio \n"
           + "                       ON  prio.Id = pr.Priority \n"
           + "                  LEFT JOIN App_PickUpRequest_CourierUpdate APC \n"
           + "                       ON  APC.[scheduleId] = pr.id \n"
           + "                       AND APC.scheduleDate = CAST(GETDATE() AS DATE) \n"
           + "                       AND APC.[STATUS] = '1' \n"
           + "           WHERE  pr.status = '1' \n"
           + "       ) A \n"
           + "       INNER JOIN [dbo].[App_Users] cour \n"
           + "            ON  cour.USER_ID = A.[USERID] \n"
           + "       WHERE pickupDate = cast(getdate() as date)";
           //+ "   Group by A.id, A.locationID, A.pickupDate, A.pickuptime, A.USERID, A.priority, A.isRoutine, A.STATUS, A.createdBy, A.createdOn, A.modifiedBy, A.modifiedOn, A.DateOrDays,A.Customer,A.Location,A.PriorityName,A.pickuptime, cour.userName";
           //+ "       INNER JOIN App_User_Attendance aua \n"
           //+ "            ON  aua.userid = cour.[USER_ID] ";



                if (att == 0)
                {
                    sql += " and cour.USER_ID in (select userid from App_User_Attendance where attDate = cast(getdate()  as date) and absentTime is null) ";
                }
                else if (att == 1)
                {
                    sql += " and cour.USER_ID not in (select userid from App_User_Attendance where attDate = cast(getdate() as date) and absentTime is null) ";
                    //sql += " and select userid from App_User_Attendance where attDate = cast(getdate() as date) and absentTime is null and userid  IN('12')";
                }
                sql += " Group by A.id, A.locationID, A.pickupDate, A.pickuptime, A.USERID, A.priority, A.isRoutine, A.STATUS, A.createdBy, A.createdOn, A.modifiedBy, A.modifiedOn, A.DateOrDays,A.Customer,A.Location,A.PriorityName,A.pickuptime, cour.userName";
                            
                var rs = await con.QueryAsync<PickupRequestModel>(sql);

                if (rs != null)
                {

                    return rs.ToList();
                }
                else
                {

                    return null;
                }
             

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
        public async Task<bool> Add(PickupRequestModel model)
        {
            var trans = con.BeginTransaction();
            try
            {
                var id = await con.QueryFirstOrDefaultAsync<int>(@"INSERT INTO [App_PickUpRequest]
                                                                ([LocationId]
                                                                ,[PickupDate]
                                                                ,[PickupTime]
                                                                ,[User_Id]
                                                                ,[Priority]
                                                                ,[IsRoutine]
                                                                ,[Comments]
                                                                ,[Status]
                                                                ,[CreatedBy]
                                                                ,[CreatedOn]
                                                                ,[modifiedBy]
                                                                ,[modifiedOn])
                                                                VALUES
                                                                (
                                                                @LocationId,
                                                                @PickupDate,
                                                                @PickupTime,
                                                                @CourierId,
                                                                @Priority,
                                                                @IsRoutine,
                                                                @Comments,
                                                                @Status,
                                                                @CreatedBy,
                                                                @CreatedOn,
                                                                2593,
                                                                2019
                                                                );SELECT SCOPE_IDENTITY();", model, transaction: trans);

                if (model.PickUpRequestDays != null && model.PickUpRequestDays.Any())
                {
                    foreach (var item in model.PickUpRequestDays)
                    {
                        //item.PickupRequestId = id;
                        await con.ExecuteAsync(@"INSERT INTO [App_PickUpReqDays]
                                ([PickupRequestId]
                                ,[Day]
                                ,[dayTime])
                                VALUES
                                (	  
                                @PickupRequestId,
                                @Day,
                                @Time
                                );",
                                new
                                {
                                    @PickupRequestId = id,
                                    @Day = item.Day,
                                    @Time = item.Time
                                }, transaction: trans);
                    }

                }


                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return false;
            }
            //catch (Exception ex)
            //{
            //    trans.Rollback();
            //    return false;
            //}
        }
    }
}