using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for cl_InvoiceCancelation
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class cl_InvoiceCancelation
    {
        public cl_InvoiceCancelation()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable InvoiceCancelation_Reasons(Cl_Variables clvar)
        {
            string query = "SELECT * FROM Mnp_InvoiceCancelationReasons micr WHERE micr.[Active]='1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public int AddInvoiceCancelation(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;
            string query = "INSERT INTO MNP_InvoiceCancelation \n"
                   + " ( \n"
                   + " 	-- id -- this column value is auto-generated \n"
                   + " 	InvocieNo, \n"
                   + " 	Cancelation_Reason, \n"
                   + " 	Created_date, \n"
                   + " 	Created_by \n"
                   + " ) \n"
                   + " VALUES \n"
                   + " ( \n"
                   + " 	'" + clvar.InvoiceNo + "', \n"
                   + " 	'" + clvar.remarks + "', \n"
                   + " 	GetDate(), \n"
                   + "  '" + HttpContext.Current.Session["U_ID"].ToString() + "'\n"
                   + " )";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int CancelInvocie(Cl_Variables clvar)
        {
            string error = "";
            int count = 0;

            string sql = "UPDATE Invoice \n"
                 + "SET IsInvoiceCanceled = '1' \n"
                 + "WHERE invoiceNumber ='" + clvar.InvoiceNo + "'";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sql, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int updateConsignments(Cl_Variables clvar)
        {
            string error = "";
            int count = 0; string numbers = "";

            for (int i = 0; i < clvar.ClvarListStr.Count; i++)
            {
                numbers += "'" + clvar.ClvarListStr[i] + "',";
            }

            numbers = numbers.Remove(numbers.Length - 1);

            string sql = " \n"
               + "UPDATE Consignment \n"
               + "SET IsInvoiced = '0' \n"
               + "WHERE consignmentNumber in (" + numbers + ")";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sql, sqlcon);
                sqlcmd.CommandType = CommandType.Text;

                count = sqlcmd.ExecuteNonQuery();
                //IsUnique = Int32.Parse(SParam.Value.ToString());
                // obj.XCode = obj.consignmentNo;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

    }
}