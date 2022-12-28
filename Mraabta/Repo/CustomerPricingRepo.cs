using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using System.Data;

namespace MRaabta.Repo
{
    public class CustomerPricingRepo
    {
        SqlConnection con;
        public CustomerPricingRepo()
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

        public async Task<string> GetLastDate()
        {
            try
            {
                var query = $@"select convert(varchar, max(DateTime)+1, 23) as Date from Mnp_Account_DayEnd where zone = '1' and Doc_Type in ('O', 'A');";
                var lastdate = await con.QueryFirstOrDefaultAsync<string>(query);

                return lastdate;
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IEnumerable<dynamic>> GetAccounts(string ActNo)
        {
            try
            {
                var isacc = $@"select distinct name, accountno from CreditClients where accountNo='{ActNo}' and status=1 and IndustryId != 67 and accountNo not in ('0','4D1','4T154','4T67','1S257') and accountNo not like '3000%' and accountNo not like '%CC%';";
                var accrs = await con.QueryFirstOrDefaultAsync<string>(isacc);

                if (accrs != null)
                {
                    var query = $@"select count(*) as AccCount, cc.name as Name FROM   consignment AS c INNER JOIN CreditClients  AS cc ON  cc.id = c.creditClientId WHERE 
                    c.bookingDate > (select max(DateTime) from Mnp_Account_DayEnd where zone = '1' and Doc_Type in ('O','A')) and c.consigneraccountno 
                    not in ('0','4D','4T154','4T67','1S257') and c.consignerAccountNo = '{ActNo}' and isnull(c.status,'0') != '9' and c.consigneraccountno  not like  '3000%' --and c.consigneraccountno  not like  '281%'
                    and isnull(c.isinvoiced,'0') != '1' and isnull(c.isapproved,'0') = '1' and isnull(c.ispricecomputed,'0') = '0' and cc.CODType !='3' Group by cc.name;";

                    var rs = await con.QueryAsync<dynamic>(query);
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

        public async Task<int> RunProcedures(string acc, string From, string To, int Compute)
        {
            try
            {

                var q1 = await con.QueryFirstOrDefaultAsync<long>("SP_UpdateCNPrice_ACC_WISE", new { AccNo = acc, dtfrom = From, dtTo = To, Compute = Compute }, commandType: CommandType.StoredProcedure);
                var q2 = await con.QueryFirstOrDefaultAsync<string>("SP_UpdateCNPrice_Int_Accwise", new { AccNo = acc, dtfrom = From, dtTo = To, Compute = Compute }, commandType: CommandType.StoredProcedure);
                var q3 = await con.QueryFirstOrDefaultAsync<long>("SP_UpdateCNPrice_RNR_ZONING_Accwise", new { AccNo = acc, dtfrom = From, dtTo = To, Compute = Compute }, commandType: CommandType.StoredProcedure);
                var t = q1 == 0 & q2.Equals("TRUE") & q3 == 0 ? 1 : 0;
                return t;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
    }
}