using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MRaabta.Repo
{
    public class HistoryDB
    {

        SqlConnection orcl;

        public HistoryDB()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        }
        public async Task OpenAsync()
        {
            await orcl.OpenAsync();
        }
        public void Close()
        {
            orcl.Close();
        }
        //All Records of inserted Consignment Number
        public DataTable getRecords(string consignmentNumber)
        {
            DataTable ds = new DataTable();
            try
            {
                    string sql = @"select ConsignmentNumber, au.UserName as RiderName, RunSheetNumber, name,cast(cast(performed_on as date) as varchar) as CreatedDate,LEFT(cast(cast(performed_on as time) as varchar),8) as CreatedTime, picker_name from App_Delivery_ConsignmentData adcd inner join App_users au on au.riderCode = adcd.riderCode where ConsignmentNumber='" + consignmentNumber + @"'";
                    orcl.Open();
                    SqlCommand orcd = new SqlCommand(sql, orcl);
                    orcd.CommandType = CommandType.Text;
                    orcd.CommandTimeout = 5000;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    oda.Fill(ds);
                    orcl.Close();
                if (ds.Rows.Count == 0)
                    {
                    string sqlcmd = @"select ConsignmentNumber, au.UserName as RiderName, RunSheetNumber, name, cast(cast(performed_on as date) as varchar) as CreatedDate,LEFT(cast(cast(performed_on as time) as varchar),8) as CreatedTime, picker_name from App_Delivery_ConsignmentData adcd inner join App_users au on au.riderCode = adcd.riderCode where RunSheetNumber='" + consignmentNumber + @"'";
                    orcl.Open();
                    SqlCommand orcdd = new SqlCommand(sqlcmd, orcl);
                    orcd.CommandType = CommandType.Text;
                    orcd.CommandTimeout = 5000;
                    SqlDataAdapter odaa = new SqlDataAdapter(orcdd);
                    odaa.Fill(ds);
                }
                    orcl.Close();
            }
            catch (Exception Err)
            {
                Err.Message.ToString();
            }
            finally { orcl.Close(); }
            return ds;
        }
    }
}