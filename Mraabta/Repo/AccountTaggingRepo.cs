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
    public class AccountTaggingRepo
    {
        SqlConnection con;
        public AccountTaggingRepo()
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
            con.Dispose();
        }

        public async Task<List<DropDownModel>> GetAccounts(string branch)
        {
            try
            {
                var query = $@"SELECT cc.id Value, cc.accountNo+' - '+ cc.name Text FROM CreditClients AS cc 
                            INNER JOIN Branches AS b ON b.branchCode = cc.branchCode INNER JOIN zones z ON z.zoneCode = b.zoneCode
                            INNER JOIN Region AS r ON r.Region = z.Region WHERE cc.[status]=1 AND cc.isActive=1 AND 
                            r.Region=(SELECT TOP 1 r.Region FROM Branches AS b INNER JOIN Zones AS z ON z.zoneCode=b.zoneCode
				            INNER JOIN Region AS r ON r.Region=z.Region WHERE b.branchCode='{branch}') AND cc.accountNo!='0'";
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
        public async Task<EmployeeModel> GetEmpDetail(string EmpNo)
        {
            try
            {
                var query = $@"SELECT e.DecibelId, zu.U_ID , e.EMAIL FROM tmp_EMployeemaster e
                                INNER JOIN ZNI_USER1 AS zu ON zu.DSG_CODE=e.EmployeeId
                                WHERE e.employeeid='{EmpNo}' AND
                                zu.[STATUS]=1 AND zu.bts_User= 0-- Only Get MIS Accounts
                                AND e.SERVICE_STATUS NOT IN (2,3,4,6,7,8,9,15) ;";
                var detail = await con.QueryFirstOrDefaultAsync<EmployeeModel>(query);
                return detail;
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
        public async Task<List<dynamic>> GetAccountsHistory(string EmpNo)
        {
            try
            {
                var query = $@"SELECT cs.UserName, cc.accountNo +' - '+ cc.name Account,cst.Code StaffType, cs.StaffTypeId
                              FROM ClientStaff AS cs INNER JOIN CreditClients AS cc ON cc.id = cs.ClientId AND cc.isActive=1
                            INNER JOIN BTSUsers AS b ON b.username=cs.UserName
                            INNER JOIN Client_StaffType AS cst ON cst.Id = cs.StaffTypeId WHERE b.userCode='{EmpNo}' ";
                var history = await con.QueryAsync<dynamic>(query);

                return history.ToList();
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
        public async Task<dynamic> OnSave(string EmpNo, int userId, string Accounts)
        {
            try
            {
                var query = $@"Insert into btnusers values ({EmpNo},{userId}, {Accounts})";
                await con.OpenAsync();
                var result = await con.ExecuteAsync(query, commandTimeout: int.MaxValue);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { con.Close(); }
        }
    }
}