using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MRaabta.Models;
using OfficeOpenXml;

namespace MRaabta.Repo
{
    public class DebitNotesRepo
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        // SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString());

        public async Task<string> UpdateDetail(string ClientID, decimal Amount, string VoucherDate, long Company)
        {
            try
            {
                var sql = "Create_DebitNote";

                con.Open();
                string result = "";
                var message = await con.QueryAsync(sql,
                    new { clientid = ClientID, dt = VoucherDate, amount = Amount, @companyID = Company },
                    commandType: CommandType.StoredProcedure);
                con.Close();
                if (message != null)
                {
                    foreach (var row in message)
                    {
                        foreach (var col in row)
                        {
                            result = col.Value;
                        }
                    }
                }
                return result;
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
        public async Task<string> UpdateBulkDN(List <DebitNotesModel> data)
        {
            try
            {
                var sql = "Create_DebitNote";

                con.Open();
                string result = "";

                for (int i = 0; i < data.Count(); i++)
                {
                  var message = await con.QueryAsync(sql,
                   new { clientid = data[i].clientid, dt = data[i].voucherdate, amount = data[i].amount, @companyID = Convert.ToInt32( data[i].companyid.Replace("\n","")) },
                   commandType: CommandType.StoredProcedure);
                    con.Close();
                    if (message != null)
                    {
                        foreach (var row in message)
                        {
                            foreach (var col in row)
                            {
                                result += col.Value+ "\n";
                            }
                        }
                    }
                }
                return result;
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