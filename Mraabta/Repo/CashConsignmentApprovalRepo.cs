using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo
{
    public class CashConsignmentApprovalRepo
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        public async Task<List<dynamic>> GetDetail(string CN)
        {
            try
            {
                int update = 0;
                string updquery = $@"SELECT CONSIGNMENTNUMBER, RIDERCODE, b.sname BRANCH, z.name ZONE, Consignment.expressCenterCode, CONSIGNER, CONSIGNEE, Cast(accountReceivingDate as Date) as SALEDATE 
                                     FROM Consignment
                                     INNER JOIN Branches b ON b.branchCode= Consignment.orgin and b.branchCode = '{HttpContext.Current.Session["BranchCode"]}'
                                     INNER JOIN Zones z ON z.zoneCode=b.zoneCode
                                     WHERE consignmentNumber = '{CN}' and consignerAccountNo = '0'";
                await con.OpenAsync();
                var data = await con.QueryAsync(updquery);
                con.Close();
                if (data.Count() != 0)
                {
                    updquery = $@"INSERT INTO Consignment_Archive				
                                  SELECT * FROM  Consignment 
                                  WHERE consignmentNumber = '{CN}' and consignerAccountNo = '0' and orgin = '{HttpContext.Current.Session["BranchCode"]}'";
                    await con.OpenAsync();
                    update = await con.ExecuteAsync(updquery);
                    con.Close();
                }
                return (data.Count() != 0 && update == 1) ? data.ToList() : null;
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

        public async Task<bool> UpdateDetail(string CN, string SaleDate)
        {
            try
            {
                string updquery = $@"UPDATE Consignment 
                                     SET isApproved = '1', isPriceComputed = '1', accountReceivingDate = '{SaleDate}' , 
                                     createdby= '{HttpContext.Current.Session["U_ID"]}', createdon = '{DateTime.Now}'
                                     WHERE consignmentNumber = '{CN}'  and consignerAccountNo = '0' and orgin = '{HttpContext.Current.Session["BranchCode"]}'";
                await con.OpenAsync();
                var upd = await con.ExecuteAsync(updquery);
                con.Close();
                return upd == 1 ? true : false;
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

        public DateTime MonthEnd()
        {
            try
            {
                string query = $@"SELECT CASE 
WHEN ZU.CNApproval ='1' AND 
 MAX(A.OPSDATEALLOWED) > MAX(A.ACCDATEALLOWED) THEN 
 MAX(A.OPSDATEALLOWED) 
WHEN ZU.CNApproval ='1' AND 
 MAX(A.OPSDATEALLOWED) <= MAX(A.ACCDATEALLOWED) THEN 
 MAX(A.ACCDATEALLOWED) 
WHEN ZU.CNApproval ='2' THEN 
 MAX(A.ACCDATEALLOWED) 
END DateAllowed 
 FROM (SELECT MAX(D.DATETIME) ACCDATEALLOWED, 0 OPSDATEALLOWED 
 FROM MNP_ACCOUNT_DAYEND D 
WHERE D.BRANCH = '{HttpContext.Current.Session["BranchCode"]}' 
AND D.DOC_TYPE = 'A' 
 UNION ALL 
 SELECT 0 ACCDATEALLOWED, MAX(D.DATETIME) OPSDATEALLOWED 
 FROM MNP_ACCOUNT_DAYEND D 
WHERE D.BRANCH = '{HttpContext.Current.Session["BranchCode"]}' 
AND D.DOC_TYPE = 'O') A 
INNER JOIN ZNI_USER1 ZU 
 ON ZU.U_ID = '{HttpContext.Current.Session["U_ID"]}' 
GROUP BY ZU.CNApproval";

                con.Open();
                var upd = con.Query<DateTime>(query);
                con.Close();
                if (upd.Count() != 0)
                {
                    return upd.First();
                }
                else return new DateTime();
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