using System;
using System.Data;
using System.Data.SqlClient;

namespace MRaabta.Repo
{
    public class LocationDB
    {
        SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        public DataTable getLocations(string BranchCode)
        {
            BranchCode = "4";
            DataTable ds = new DataTable();
            string sql = "SELECT ccl.locationID,ccl.locationName where ccl.branchCode = '" + BranchCode + "'";

            try
            {
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);

                orcl.Close();

            }
            catch (Exception Err)
            {
                Err.Message.ToString();
                //return output;
            }
            return ds;
        }
    }
}