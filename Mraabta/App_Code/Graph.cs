using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for bayer_Function
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Graph
    {
        public Graph()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        Cl_Variables clvar = new Cl_Variables();

        public DataSet Get_Graph(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT z.name ZoneName,\n" +
                "       COUNT(a.consignmentNumber) CNCount\n" +
                "  FROM Consignment a\n" +
                " INNER JOIN Branches b\n" +
                "    ON a.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    ON b.zoneCode = z.zoneCode\n" +
                " where YEAR(bookingDate) = '" + clvar._Year + "'\n" +
                "   and month(bookingDate) = '" + clvar._Month + "'\n" +
                " GROUP BY z.name\n" +
                " ORDER BY z.name ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }
        /*
        public DataSet Get_TotalCN(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT \n" +
                "       COUNT(a.consignmentNumber) CNCount\n" +
                "  FROM Consignment a\n" +
                " INNER JOIN Branches b\n" +
                "    ON a.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    ON b.zoneCode = z.zoneCode\n" +
                " where YEAR(bookingDate) = '" + clvar._Year + "'\n" +
                "   and month(bookingDate) = '" + clvar._Month + "'\n" +
                " GROUP BY z.name\n" +
                " ORDER BY z.name ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }
        */
        public DataSet Get_DayEnd(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select\n" +
                                    "c.expressCenterCode, c.name, c.description, CONVERT(NVARCHAR, c.WorkingDate, 105) WorkingDate , z.name zonename, b.name branchname,\n" +
                                    " CASE WHEN c.status = '1' THEN 'ACTIVE' ELSE 'DEACTIVE' END \n" +
                                    "from\n" +
                                    "Zones z, Branches b, ExpressCenters c\n" +
                                    "where\n" +
                                    "c.bid = b.branchCode\n" +
                                    "and b.zoneCode = z.zoneCode\n" +
                                    clvar._Zone + "\n" + clvar._TownCode + "\n" + clvar._Year + "\n" +
                                    "and c.status ='1'\n" +
                                    "order by CONVERT(NVARCHAR, c.WorkingDate, 105) DESC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public DataSet Get_TotalCN(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT \n" +
                "       COUNT(a.consignmentNumber) CNCount\n" +
                "  FROM Consignment a\n" +
                " INNER JOIN Branches b\n" +
                "    ON a.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    ON b.zoneCode = z.zoneCode\n" +
                " where YEAR(bookingDate) = '" + clvar._Year + "'\n" +
                "   and month(bookingDate) = '" + clvar._Month + "'\n";
                //  " GROUP BY z.name\n" +
                //  " ORDER BY z.name ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public DataSet Get_DayWise(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {

                string sqlString = "SELECT *\n" +
                                    "FROM   (\n" +
                                    "           SELECT z.name                  Zone,\n" +
                                    "                  DAY(c.bookingDate)      DAY,\n" +
                                    "                  c.consignmentNumber     CN\n" +
                                    "           FROM   Consignment             c,\n" +
                                    "                  Branches                b,\n" +
                                    "                  Zones                   z\n" +
                                    "           WHERE  c.orgin = b.branchCode\n" +
                                    "                  AND b.zoneCode = z.zoneCode\n" +
                                    "                  AND z.zoneCode = '" + clvar.Zone + "' \n" +
                                    "                  AND YEAR(c.bookingDate) = '" + clvar._Year + "' \n" +
                                    "                  AND MONTH(c.bookingDate) = '" + clvar._Month + "' \n" +
                                    "       ) AS s\n" +
                                    "       PIVOT(\n" +
                                    "           COUNT(CN)\n" +
                                    "           FOR DAY IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11],\n" +
                                    "                      [12], [13], [14], [15], [16], [17], [18], [19], [20], [21],\n" +
                                    "                      [22], [23], [24], [25], [26], [27], [28], [29], [30], [31])\n" +
                                    "       ) AS pvt\n" +
                                    "ORDER BY\n" +
                                    "       1";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

    }
}