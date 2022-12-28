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
    public class TaxForwarderRepo
    {
        SqlConnection con;
        public TaxForwarderRepo()
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

        public async Task<int> InsertData_Old(string date, List<TaxForwarderModel> data)
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[] {
                            new DataColumn("CPRNo", typeof(string)),
                            new DataColumn("Customer", typeof(string)),
                            new DataColumn("INV", typeof(string)),
                            new DataColumn("InvoiceNumber", typeof(string)),
                            new DataColumn("PaidAmount", typeof(string)),
                            new DataColumn("Source", typeof(string)),
                            new DataColumn("Type", typeof(string))
                            });

                foreach (var item in data)
                {
                    var row = dt.NewRow();

                    row["CPRNo"] = item.CPRNo;
                    row["Customer"] = item.Customer;
                    row["INV"] = item.INV;
                    row["InvoiceNumber"] = item.InvoiceNumber;
                    row["PaidAmount"] = item.PaidAmount;
                    row["Source"] = item.Source;
                    row["Type"] = item.Type;

                    dt.Rows.Add(row);
                }

                var q1 = await con.QueryFirstOrDefaultAsync<string>("SP_InsertTaxForwarder", new { FormDate = date, TaxData = dt, Message=0 }, commandType: CommandType.StoredProcedure);

                var t = q1 == "0" ? 1 : 0;
                return t;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }


        public string InsertData(string date, List<TaxForwarderModel> data)
        {
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[] {
                            new DataColumn("CPRNo", typeof(string)),
                            new DataColumn("PaidAmount", typeof(string)),
                            new DataColumn("Customer", typeof(string)),
                            new DataColumn("InvoiceNumber", typeof(string)),
                            new DataColumn("INV", typeof(string)),
                            new DataColumn("Type", typeof(string)),
                            new DataColumn("Source", typeof(string))
                            });

            foreach (var item in data)
            {
                var row = dt.NewRow();

                row["CPRNo"] = item.CPRNo;
                row["PaidAmount"] = item.PaidAmount;
                row["Customer"] = item.Customer;
                row["INV"] = item.INV;
                row["InvoiceNumber"] = item.InvoiceNumber;
            
                row["Source"] = item.Source;
                row["Type"] = item.Type;

                dt.Rows.Add(row);
            }

            string error = "";
            string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_InsertTaxForwarder", con))
                    {
                        con.Open();
                        cmd.CommandTimeout = 300000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FormDate", date);
                        cmd.Parameters.AddWithValue("@TaxData", dt);
                        cmd.Parameters.Add("@Message", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        error = cmd.Parameters["@Message"].SqlValue.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                con.Close();
                error = ex.Message;
            }
            finally { con.Close(); }

            return error;
        }
    }
}