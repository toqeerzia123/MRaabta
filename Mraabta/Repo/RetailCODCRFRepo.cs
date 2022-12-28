using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MRaabta.Models;
using MRaabta.Repo;

namespace MRaabta.Repo
{
    public class RetailCODCRFRepo
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
       // SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString());
            
        public async Task<List<ZoneModel>> GetZones()
        {
            try
            {
                String query = ""; string sessionzone = "";
                if (HttpContext.Current.Session["ZONECODE"].ToString() != "ALL")
                {
                    List<string> StrLineElements = HttpContext.Current.Session["ZONECODE"].ToString().Split(',').ToList();
                    for (int i = 0; i < StrLineElements.Count(); i++)
                    {
                        sessionzone += "'" + StrLineElements[i] + "',";
                    }
                    sessionzone = sessionzone.ToString().Trim(',');
                }

                query = @"SELECT distinct z.NAME AS ZoneName, z.zoneCode FROM Zones z 
INNER JOIN Branches b ON b.zoneCode = z.zoneCode AND z.[status]=1 AND z.region is not null and z.zone_type = '1' ";
                if (HttpContext.Current.Session["ZONECODE"].ToString() != "ALL")
                {
                    query += "  where z.zonecode IN (" + sessionzone + ")";
                }
                query+=" ORDER BY z.name";

                await con.OpenAsync();
                var rs = await con.QueryAsync<ZoneModel>(query);
                con.Close();
                List<ZoneModel> s = new List<ZoneModel>();
                s = (List<ZoneModel>)rs;
                return s.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }

        public async Task<List<BranchModel>> GetBranches(long zoneCode)
        {
            try
            {
                string query = @"SELECT b.name AS BranchName, b.branchCode FROM Branches b 
INNER JOIN zones z ON z.zoneCode= b.zoneCode AND b.[status]=1  AND z.zone_type='1' WHERE b.zoneCode = '" + zoneCode + "' ";
                
                await con.OpenAsync();
                var rs = await con.QueryAsync<BranchModel>(query);
                con.Close();
                List<BranchModel> s = new List<BranchModel>();
                s = (List<BranchModel>)rs;
                return s.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }

        public async Task<List<RetailCODCRFNewModel>> GetData(long zone, string branch, string accountNo, string customerName)
        {
            try
            {
                if (branch != "" && branch != null)
                {
                    branch = branch.TrimEnd(',');
                    //branch = branch.Replace("''", "");
                }

                string query = @" SELECT distinct z.name AS Zone, b.name AS Branch, cu.Email, cu.BankName AS BankId, ec.name as ECName,mprs.EmployeeUsername RSEName, CONVERT(VARCHAR, cu.createdOn, 106) DateOfOpening, 
 CU.[Status] ,  cu.isApproved, cu.approvedDate AS DateOfApproval, cu.CustomerName,
 cu.address Address, cu.CNIC,cu.AccountNumber,b2.Name BankName,cu.IBFT,cu.AccountTitle, cu.AccountNumber ,
 cu.MobileNumber BankContactNo,cu.BankBranchName,cu.BankBranchCode
 FROM MNP_RETAIL_COD_CUSTOMERS cu
 INNER JOIN MnP_Retail_Staff mprs ON mprs.UserId = cu.CreatedBy
 INNER JOIN CreditClients AS cc ON cu.AccountNumber = cc.accountNo
 INNER JOIN ExpressCenters ec ON ec.expressCenterCode = cc.expressCenterCode
 INNER JOIN Branches b ON b.branchCode=cu.Branch
 INNER JOIN Zones z ON z.zoneCode=b.zoneCode
 LEFT JOIN Banks b2 ON b2.Id=cu.BankName
 WHERE (cu.isApproved!=1 OR cu.isApproved IS NULL) ";
                
                if( zone != 0)
                {
                    query+= " and z.zoneCode = '" + zone + "' "; 
                }
                else
                {
                    string sessionzone = "";
                    List<string> StrLineElements = HttpContext.Current.Session["ZONECODE"].ToString().Split(',').ToList();
                    for (int i = 0; i < StrLineElements.Count(); i++)
                    {
                        sessionzone += "'" + StrLineElements[i] + "',";
                    }
                    sessionzone = sessionzone.ToString().Trim(',');
                    query += " and z.zoneCode IN (" + sessionzone + ")";
                }
                
                if(branch!="" && branch!=null)
                {
                    if(!branch.Contains("Select All"))
                    {
                        query += " and b.branchCode IN (" + branch + ")";
                    }
                }
                
                if (accountNo != null && accountNo!="")
                {
                    query += " and cu.AccountNumber= '" + accountNo + "' ";
                }
                if (customerName != null && customerName!="")
                {
                    query += " and cu.CustomerName LIKE '%" + customerName + "%' ";
                }
                query += " ORDER BY cu.AccountNumber";

                await con.OpenAsync();
                var rs = await con.QueryAsync<RetailCODCRFNewModel>(query);
                con.Close();
                List<RetailCODCRFNewModel> s = new List<RetailCODCRFNewModel>();
                s = (List<RetailCODCRFNewModel>)rs;
                return s.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<int> ApproveCRF(string accountno, int approve)
        {
            try
            {
                    string updquery = @"UPDATE MNP_Retail_COD_Customers SET isApproved = " + approve + ", approvedDate = Getdate()," +
                    " ApprovedBy = " + HttpContext.Current.Session["U_ID"] + " WHERE AccountNumber= '" + accountno + "'";
                    await con.OpenAsync();
                    int upd = await con.ExecuteAsync(updquery);
                    con.Close();
                return upd;
                
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }

        public async Task<List<BankModel>> GetBanks()
        {
            try
            {
                string query = @"SELECT Id AS BankId, NAME AS BankName FROM banks order by name ";


                await con.OpenAsync();
                var rs = await con.QueryAsync<BankModel>(query);
                con.Close();
                List<BankModel> s = new List<BankModel>();
                s = (List<BankModel>)rs;
                return s.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }

        public async Task<int> UpdateDetail(RetailCODCRFNewModel item)
        {
            try
            {
                string updquery = @"UPDATE MNP_Retail_COD_Customers SET CustomerName = '" + item.CustomerName + "'," +
                   " AccountTitle = '" + item.AccountTitle + "', Address = '" + item.Address + "',"+
                   " BankName = " + item.BankId + ", BankBranchName = '" + item.BankBranchName + "',"+
                   " BankBranchCode = '" + item.BankBranchCode + "', IBFT = '" + item.IBFT + "',"+
                   " ModifiedBy = " + HttpContext.Current.Session["U_ID"] + 
                    ", ModifiedOn = Getdate() WHERE AccountNumber= '" + item.AccountNumber + "'";
                await con.OpenAsync();
                   var upd = await con.ExecuteAsync(updquery);

                if (upd == 1)
                {
                    string updatecreditclient = @" UPDATE CreditClients
                                               SET
	                                           BeneficiaryName = '" + item.AccountTitle +
                                               "', BeneficiaryBankAccNo = '" + item.IBFT +
                                               "', BeneficiaryBankCode =  '" + item.BankId +
                                               "' WHERE accountNo ='" + item.AccountNumber + "'";
                   await con.ExecuteAsync(updatecreditclient);
                }


                con.Close();
                return upd;
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
    }
}
