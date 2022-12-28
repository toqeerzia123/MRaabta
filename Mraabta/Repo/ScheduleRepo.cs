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
    public class ScheduleRepo
    {
        SqlConnection con;
        public ScheduleRepo()
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
                var rs = await con.QueryAsync<DropDownModel>(@"select [user_Id] as [Value], [userName] as [Text] from App_Users where roleID = '1';");
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

        public async Task<bool> UpdateSchdeuleRequest(CourierTransferModel model)
        {
            try
            {
                var sqlStatement = @"UPDATE App_PickUpRequest
            SET    User_ID       = @newCourierID,
                   modifiedBy     = @modifiedBy ,
                   modifiedOn     = GETDATE() WHERE User_ID = @oldCourierID";

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

        public async Task<List<string>> getScheduleIDs(int userID)
        {
            try
            {
                var rs = await con.QueryAsync<string>(
                @"SELECT id
                FROM   App_PickUpRequest apur
                WHERE  apur.[User_ID] = '" + userID + "' and status = '1' and isRoutine = '1'");

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

        public async Task<bool> UpdateCourierSchedule(CourierTransferDateModel model)
        {
            var trans = con.BeginTransaction();
            try
            {
                await con.ExecuteAsync(@"UPDATE App_PickUpRequest_CourierScheduleUpdate
                                                    SET    STATUS = '0'
                                                    WHERE  USER_ID IN (@oldCourierID,@newCourierID);   
                                    INSERT INTO App_PickUpRequest_CourierScheduleUpdate
                                           (USER_ID
                                           ,STATUS
                                           ,createdBy
                                           ,createdOn)
                                    VALUES
                                      (@oldCourierID,'1',  @createdBy,getdate()),
                                    (@newCourierID,'1',  @createdBy,getdate());", model, transaction: trans);
                if (model.scheduleId != null && model.scheduleId.Any())
                {
                    foreach (var item in model.scheduleId)
                    {
                        await con.ExecuteAsync(@"UPDATE App_PickUpRequest_CourierUpdate
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
	                                          @scheduleDate ,
	                                          @newCourierID,
	                                          1,
	                                          @createdBy,
	                                          getdate()
                                            );",
                            new
                            {
                                @id = item,
                                @scheduleDate = model.scheduleDate,
                                @newCourierID = model.newCourierID,
                                @createdBy = model.createdBy
                            }, transaction: trans);
                    }
                }

                trans.Commit();
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

    }
}