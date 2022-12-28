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
    public class ReceiptVoucherRepo
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        public async Task<List<dynamic>> GetVoucherDateAndAmount(string VoucherID)
        {
            try
            {
                string updquery = $@"Select pv.VoucherDate, pv.Amount from PaymentVouchers pv INNER JOIN CreditClients cc ON cc.id= pv.CreditClientId
WHERE cc.accountNo='0' and pv.id = '{VoucherID}' and pv.BranchCode= '{HttpContext.Current.Session["BranchCode"].ToString()}'";
                await con.OpenAsync();
                var upd = await con.QueryAsync(updquery);
                con.Close();
                return upd.ToList();
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

        public async Task<bool> UpdateVoucherDate(string VoucherID, string VoucherDate)
        {
            try
            {
                string updquery = $@"UPDATE PaymentVouchers SET VoucherDate = '{VoucherDate}' WHERE id = '{VoucherID}' ";
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
            try {
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

        public DateTime MinimumDate()
        {
            try { 
            string query = $@"select MAX(DateTime) DateAllowed from Mnp_Account_DayEnd where Branch = '{HttpContext.Current.Session["BranchCode"].ToString()}' and doc_type = 'R'";
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