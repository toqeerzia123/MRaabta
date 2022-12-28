using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Cl_Manifest
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Cl_Manifest
    {
        public Cl_Manifest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string NewManifest(Cl_Variables clvar, DataTable cns, DataTable track, DataTable man)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "Bulk_Manifest";
                cmd1.Parameters.AddWithValue("@tblCustomers", man);
                cmd1.Parameters.AddWithValue("@tblcustomers2", cns);
                cmd1.Parameters.AddWithValue("@tblcustomers1", track);

                cmd1.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd1.Parameters.AddWithValue("@ManifestType", clvar.CheckCondition);
                cmd1.Parameters.AddWithValue("@ManifestDate", clvar.Day);
                cmd1.Parameters.AddWithValue("@origin", clvar.origin);
                cmd1.Parameters.AddWithValue("@destination", clvar.destination);
                cmd1.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd1.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["zoneCode"].ToString());
                cmd1.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd1.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd1.ExecuteNonQuery();
                string retunvalue = cmd1.Parameters["@result"].Value.ToString();
                //string error = cmd1.Parameters["@result"].ToString();

                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return ex.Message;
            }


            return "OK";
        }

        public string UpdateManifest(Cl_Variables clvar, DataTable cns, DataTable track, DataTable man)
        {




            return "OK";
        }

        public string DemanifestWithoutManifest(Cl_Variables clvar, DataTable cns, DataTable track, DataTable man)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd = new SqlCommand();
            SqlCommand cmd1 = new SqlCommand();
            SqlCommand cmd2 = new SqlCommand();
            cmd.Connection = con;
            cmd1.Connection = con;
            cmd2.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd.Transaction = transaction;
                cmd1.Transaction = transaction;
                cmd2.Transaction = transaction;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd2.CommandType = CommandType.StoredProcedure;
                //cns.Columns.Add("riderCode", typeof(string));
                cmd.CommandText = "Bulk_consignments";
                cmd.Parameters.AddWithValue("@tblcustomers", cns);
                cmd.ExecuteNonQuery();

                cmd1.CommandText = "Bulk_Manifest";
                cmd1.Parameters.AddWithValue("@tblCustomers", man);
                cmd1.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd1.Parameters.AddWithValue("@ManifestType", clvar.CheckCondition);
                cmd1.Parameters.AddWithValue("@ManifestDate", clvar.Day);
                cmd1.Parameters.AddWithValue("@origin", clvar.origin);
                cmd1.Parameters.AddWithValue("@destination", clvar.destination);
                cmd1.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd1.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["zoneCode"].ToString());
                cmd1.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd1.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                //SqlParameter retval = cmd1.Parameters.Add("@result", SqlDbType.Int);
                //retval.Direction = ParameterDirection.ReturnValue;
                //cmd1.Parameters.Add(retval);
                cmd1.ExecuteNonQuery();
                string retunvalue = cmd1.Parameters["@result"].Value.ToString();
                //string error = cmd1.Parameters["@result"].ToString();

                cmd2.CommandText = "Bulk_ConsignmentsTrackingHistory";
                cmd2.Parameters.AddWithValue("@tblcustomers", track);
                cmd2.ExecuteNonQuery();
                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return ex.Message;
            }

            return "OK";
        }
    }
}