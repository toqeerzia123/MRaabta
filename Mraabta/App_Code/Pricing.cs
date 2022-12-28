using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Pricing
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Pricing
    {
        public Pricing()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet Get_ConsignmentInfo(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT *  \n"
                  + "FROM   Consignment c  \n"
                  + "WHERE  c.isApproved = '1'  \n"
                  + "      AND c.totalAmount is null  \n"
                  + "       AND MONTH(c.bookingDate) = '06'  \n"
                  + "       AND YEAR(c.bookingDate) = '2016'  \n"
                  + "      AND c.consignerAccountNo not in ('0','993','999','16043','4D1')  \n"
                  + "   ----    AND c.[weight] ='0.5'        \n"
                  + "      and c.serviceTypeName not in ('Sunday & Holiday','Second Day','International_Doc_Special_Hub','International 15 Percent Discount tariff Non Doc','International Special Rates from Up Country','Bank to Bank','International_Doc','Road n Rail','Aviation Sale','International 10 Percent Discount tariff','NTS') \n"
                  + "     -- and ConsignmentNumber not in('103110532325') "
                  + "   union all \n"
                  + "   SELECT *  \n"
                  + "FROM   Consignment c  \n"
                  + "WHERE  c.isApproved = '1'  \n"
                  + "      AND c.totalAmount ='0' \n"
                  + "       AND MONTH(c.bookingDate) = '06'  \n"
                  + "       AND YEAR(c.bookingDate) = '2016'  \n"
                  + "           AND c.consignerAccountNo not in ('0','993','999','16043','4D1')  \n"
                  + "      and c.serviceTypeName not in ('Sunday & Holiday','Second Day','International_Doc_Special_Hub','International 15 Percent Discount tariff Non Doc','International Special Rates from Up Country','Bank to Bank','International_Doc','Road n Rail','Aviation Sale','International 10 Percent Discount tariff','NTS') \n"
                  + "      --and ConsignmentNumber not in('103110532325') "
                  + "  ";
                //   Variable clvar = new Variable();
                // Cl_Variables clvar = new Cl_Variables();
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }


        public DataSet Get_ConsignmentInfo_Iscomputed(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "   SELECT  *  \n"
                  + "FROM   Consignment c  \n"
                  + "WHERE  c.isApproved = '1'  \n"
                  + "  \n"
                  + "    and c.isPriceComputed ='0'\n"
                  + "    AND MONTH(c.bookingDate) = '06'  \n"
                  + "    AND YEAR(c.bookingDate) = '2016'  \n"
                  + "   -- and c.zoneCode ='2' \n"
                  + "  ";
                //   Variable clvar = new Variable();
                // Cl_Variables clvar = new Cl_Variables();
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }

        public string InsertConsignmentsFromRunsheet_(Cl_Variables clvar)
        {
            int count = 0;
            string check = "";
            string query = "";
            string query1 = "";
            query = "UPDATE Consignment SET totalAmount = '" + clvar.TotalAmount + "', gst = '" + clvar.gst + "', isPriceComputed = '1' WHERE consignmentNumber ='" + clvar.consignmentNo + "'";


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return "NOT OK";
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                return "NOT OK" + ex.Message;
            }
            return "OK";
        }
    }
}