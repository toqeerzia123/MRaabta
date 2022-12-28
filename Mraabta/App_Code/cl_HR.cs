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
    public class cl_HR
    {

        public cl_HR()
        {
        }

        Cl_Variables clvar = new Cl_Variables();

        #region HR CLASS

        public DataSet Get_AllRiders(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                query = "select r.firstName + ' ' + r.lastName ridername, r.riderCode from Riders r order by ridername ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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


        public DataSet Get_RidersAttendanceRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                string sqlString_old = "select\n" +
                "a.ACNO,\n" +
                "a.shift,\n" +
                "a.firstName,\n" +
                "a.lastName,\n" +
                "a.ZoneCode,\n" +
                "a.Zone,\n" +
                "a.branchCode,\n" +
                "a.Branch,\n" +
                "a.Department,\n" +
                "a.Designation,\n" +
                "a.DATE,\n" +
                "a.Designation,\n" +
                "a.userTypeId ,\n" +
                "CONVERT(varchar(15), CAST(a.LASTIN AS TIME), 108) LASTIN,\n" +
                "CONVERT(VARCHAR(10), a.LASTIN, 103) DateIn,\n" +
                "CONVERT(varchar(15), CAST(a.FIRSTOUT AS TIME), 108) FIRSTOUT,\n" +
                "CONVERT(VARCHAR(10), a.FIRSTOUT , 103) DateOut\n" +
                "\n" +
                "\n" +
                " from\n" +
                "(\n" +
                "SELECT B.ACNO,\n" +
                "       R.firstName,\n" +
                "       R.lastName,\n" +
                "       Z.ZoneCode,\n" +
                "       Z.Name Zone,\n" +
                "       BB.branchCode,\n" +
                "       BB.name Branch,\n" +
                "       'Operations' Department,\n" +
                "       r.cid Designation,\n" +
                "       ec.ShiftName shift,\n" +
                "       r.userTypeId,\n" +
                "       B.DATE,\n" +
                "       MAX(CASE\n" +
                "             WHEN B.REMARKS = 'LASTIN' THEN\n" +
                "              B.TIME\n" +
                "             ELSE\n" +
                "              ' '\n" +
                "           END) LASTIN,\n" +
                "       MAX(CASE\n" +
                "             WHEN B.REMARKS = 'FIRSTOUT' THEN\n" +
                "              B.TIME\n" +
                "             ELSE\n" +
                "              ' '\n" +
                "           END) FIRSTOUT\n" +
                "  FROM (SELECT S.ACNO,\n" +
                "               CONVERT(VARCHAR(10), S.DATETIME, 103) DATE,\n" +
                "               MIN(S.DATETIME) TIME,\n" +
                "               'FIRSTOUT' REMARKS\n" +
                "          FROM TIMESTAMPING AS S\n" +
                "         --WHERE S.ACNO = '1039' AND\n" +
                "           where S.[NEW STATE] = 'C/OUT'\n" +
                "         GROUP BY S.ACNO, CONVERT(VARCHAR(10), S.DATETIME, 103)\n" +
                "\n" +
                "        UNION\n" +
                "\n" +
                "        SELECT S.ACNO,\n" +
                "               CONVERT(VARCHAR(10), S.DATETIME, 103) DATE,\n" +
                "               MAX(S.DATETIME) TIME,\n" +
                "               'LASTIN' REMARKS\n" +
                "          FROM TIMESTAMPING AS S\n" +
                "         WHERE -- S.ACNO = '1255' AND\n" +
                "         [NEW STATE] != 'C/OUT'\n" +
                "           --AND CONVERT(VARCHAR(10), S.DATETIME, 103) BETWEEN '01/07/2016' AND '11/07/2016'\n" +
                "         GROUP BY S.ACNO, CONVERT(VARCHAR(10), S.DATETIME, 103)) B\n" +
                "\n" +
                "\n" +
                " INNER JOIN Riders R\n" +
                " ON R.riderCode = B.ACNo\n" +
                " and R.riderCode not IN('N/A','na','new')\n" +
                "\n" +
                "-- INNER JOIN Riders R\n" +
                "  --  ON R.riderCode = B.ACNo\n" +
                " INNER JOIN Branches BB\n" +
                "    ON BB.branchCode = R.branchId\n" +
                "    INNER JOIN ExpressCenterShiftTime ec\n" +
                "    ON ec.Id = R.shift\n" +
                " INNER JOIN rvdbo.Zone Z\n" +
                "    ON Z.ZoneCode = R.zoneId\n" +
                "  --  where b.DATE\n" +
                " GROUP BY B.ACNO,\n" +
                "       R.firstName,\n" +
                "       R.lastName,\n" +
                "       Z.ZoneCode,\n" +
                "       Z.Name,\n" +
                "       BB.branchCode,\n" +
                "       BB.name,\n" +
                "       r.cid,\n" +
                "       r.userTypeId,\n" +
                "       ec.ShiftName,\n" +
                "       B.DATE\n" +
                "\n" +
                "       )\n" +
                "       a where a.ACNo='" + clvar.RiderCode + "' \n" +
                " and  a.DATE between CONVERT(VARCHAR(10), '" + clvar.StartDate + "', 103) and CONVERT(VARCHAR(10), '" + clvar.EndDate + "', 103) ";




                string sqlString = "select\n" +
                "a.ACNO,\n" +
                "a.shift,\n" +
                "a.firstName,\n" +
                "a.lastName,\n" +
                "a.ZoneCode,\n" +
                "a.Zone,\n" +
                "a.branchCode,\n" +
                "a.Branch,\n" +
                "a.Department,\n" +
                "a.Designation,\n" +
                "--a.DATE,\n" +
                "CONVERT(NVARCHAR, a.DATE, 105) DATE,\n" +
                "a.Designation,\n" +
                "a.userTypeId ,\n" +
                "CONVERT(varchar(15), CAST(a.LASTIN AS TIME), 108) LASTIN,\n" +
                "--CONVERT(VARCHAR(10), a.LASTIN, 103) DateIn,\n" +
                "CONVERT(NVARCHAR, a.LASTIN, 105) DateIn,\n" +
                "CONVERT(varchar(15), CAST(a.FIRSTOUT AS TIME), 108) FIRSTOUT,\n" +
                "--CONVERT(VARCHAR(10), a.FIRSTOUT , 103) DateOut,\n" +
                "--CONVERT(NVARCHAR, a.FIRSTOUT, 105) DateOut\n" +
                "CASE\n" +
                "         WHEN a.FIRSTOUT = '01-01-1900' THEN ''\n" +
                "         ELSE CONVERT(NVARCHAR, a.FIRSTOUT, 105)\n" +
                "       END as DateOut\n" +
                " from\n" +
                "(\n" +
                "SELECT B.ACNO,\n" +
                "       R.firstName,\n" +
                "       R.lastName,\n" +
                "       Z.ZoneCode,\n" +
                "       Z.Name Zone,\n" +
                "       BB.branchCode,\n" +
                "       BB.name Branch,\n" +
                "       'Operations' Department,\n" +
                "       r.cid Designation,\n" +
                "       ec.ShiftName shift,\n" +
                "       r.userTypeId,\n" +
                "     --  B.DATE,\n" +
                "       CONVERT(NVARCHAR, b.DATE, 105) DATE,\n" +
                "       MAX(CASE\n" +
                "             WHEN B.REMARKS = 'LASTIN' THEN\n" +
                "              B.TIME\n" +
                "             ELSE\n" +
                "              ' '\n" +
                "           END) LASTIN,\n" +
                "       MAX(CASE\n" +
                "             WHEN B.REMARKS = 'FIRSTOUT' THEN\n" +
                "              B.TIME\n" +
                "             ELSE\n" +
                "              ' '\n" +
                "           END) FIRSTOUT\n" +
                "  FROM (SELECT S.ACNO,\n" +
                "               --CONVERT(VARCHAR(10), S.DATETIME, 103) DATE,\n" +
                "               CONVERT(NVARCHAR, S.DATETIME, 105) DATE,\n" +
                "               MIN(S.DATETIME) TIME,\n" +
                "               'FIRSTOUT' REMARKS\n" +
                "          FROM TIMESTAMPING AS S\n" +
                "         --WHERE S.ACNO = '1039' AND\n" +
                "           where S.[NEW STATE] = 'C/OUT'\n" +
                "         GROUP BY S.ACNO, CONVERT(NVARCHAR, S.DATETIME, 105)\n" +
                "\n" +
                "        UNION\n" +
                "\n" +
                "        SELECT S.ACNO,\n" +
                "               CONVERT(NVARCHAR, S.DATETIME, 105) DATE,\n" +
                "               MAX(S.DATETIME) TIME,\n" +
                "               'LASTIN' REMARKS\n" +
                "          FROM TIMESTAMPING AS S\n" +
                "         WHERE -- S.ACNO = '1255' AND\n" +
                "         [NEW STATE] != 'C/OUT'\n" +
                "           --AND CONVERT(VARCHAR(10), S.DATETIME, 103) BETWEEN '01/07/2016' AND '11/07/2016'\n" +
                "         GROUP BY S.ACNO, CONVERT(NVARCHAR, S.DATETIME, 105)) B\n" +
                "\n" +
                "\n" +
                " INNER JOIN Riders R\n" +
                " ON R.riderCode = B.ACNo\n" +
                " and R.riderCode not IN('N/A','na','new')\n" +
                "\n" +
                "-- INNER JOIN Riders R\n" +
                "  --  ON R.riderCode = B.ACNo\n" +
                " INNER JOIN Branches BB\n" +
                "    ON BB.branchCode = R.branchId\n" +
                "    INNER JOIN ExpressCenterShiftTime ec\n" +
                "    ON ec.Id = R.shift\n" +
                " INNER JOIN rvdbo.Zone Z\n" +
                "    ON Z.ZoneCode = R.zoneId\n" +
                "  --  where b.DATE\n" +
                " GROUP BY B.ACNO,\n" +
                "       R.firstName,\n" +
                "       R.lastName,\n" +
                "       Z.ZoneCode,\n" +
                "       Z.Name,\n" +
                "       BB.branchCode,\n" +
                "       BB.name,\n" +
                "       r.cid,\n" +
                "       r.userTypeId,\n" +
                "       ec.ShiftName,\n" +
                "       B.DATE\n" +
                "       )\n" +
                 //   "       a where a.ACNo='" + clvar.RiderCode + "' \n" +
                 //  " and  a.DATE between CONVERT(VARCHAR(10), '" + clvar.StartDate + "', 103) and CONVERT(VARCHAR(10), '" + clvar.EndDate + "', 103) ";
                 "       a where  \n" +
                "           " + clvar.StartDate + " \n" +
                "           " + clvar._TownCode + " \n" +
                "           " + clvar._Zone + " \n" +
                "           " + clvar.RiderCode + " \n";



                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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


        public DataSet Get_RidersNameAttendanceRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {

                string sqlString = "select\n" +
                "a.firstName+' '+a.lastName ridername,\n" +
                "a.firstName,a.lastName\n" +
                " from\n" +
                "(\n" +
                "SELECT\n" +
                "       R.firstName,\n" +
                "       R.lastName,\n" +
                "       Z.ZoneCode,\n" +
                "       Z.Name Zone,\n" +
                "       BB.branchCode,\n" +
                "       BB.name Branch,\n" +
                "       'Operations' Department,\n" +
                "       r.cid Designation,\n" +
                "       ec.ShiftName shift,\n" +
                "       r.userTypeId,\n" +
                "     --  B.DATE,\n" +
                "       CONVERT(NVARCHAR, b.DATE, 105) DATE,\n" +
                "       MAX(CASE\n" +
                "             WHEN B.REMARKS = 'LASTIN' THEN\n" +
                "              B.TIME\n" +
                "             ELSE\n" +
                "              ' '\n" +
                "           END) LASTIN,\n" +
                "       MAX(CASE\n" +
                "             WHEN B.REMARKS = 'FIRSTOUT' THEN\n" +
                "              B.TIME\n" +
                "             ELSE\n" +
                "              ' '\n" +
                "           END) FIRSTOUT\n" +
                "  FROM (SELECT S.ACNO,\n" +
                "               --CONVERT(VARCHAR(10), S.DATETIME, 103) DATE,\n" +
                "               CONVERT(NVARCHAR, S.DATETIME, 105) DATE,\n" +
                "               MIN(S.DATETIME) TIME,\n" +
                "               'FIRSTOUT' REMARKS\n" +
                "          FROM TIMESTAMPING AS S\n" +
                "         --WHERE S.ACNO = '1039' AND\n" +
                "           where S.[NEW STATE] = 'C/OUT'\n" +
                "         GROUP BY S.ACNO, CONVERT(NVARCHAR, S.DATETIME, 105)\n" +
                "\n" +
                "        UNION\n" +
                "\n" +
                "        SELECT S.ACNO,\n" +
                "               CONVERT(NVARCHAR, S.DATETIME, 105) DATE,\n" +
                "               MAX(S.DATETIME) TIME,\n" +
                "               'LASTIN' REMARKS\n" +
                "          FROM TIMESTAMPING AS S\n" +
                "         WHERE -- S.ACNO = '1255' AND\n" +
                "         [NEW STATE] != 'C/OUT'\n" +
                "           --AND CONVERT(VARCHAR(10), S.DATETIME, 103) BETWEEN '01/07/2016' AND '11/07/2016'\n" +
                "         GROUP BY S.ACNO, CONVERT(NVARCHAR, S.DATETIME, 105)) B\n" +
                "\n" +
                "\n" +
                " INNER JOIN Riders R\n" +
                " ON R.riderCode = B.ACNo\n" +
                " and R.riderCode not IN('N/A','na','new')\n" +
                "\n" +
                "-- INNER JOIN Riders R\n" +
                "  --  ON R.riderCode = B.ACNo\n" +
                " INNER JOIN Branches BB\n" +
                "    ON BB.branchCode = R.branchId\n" +
                "    INNER JOIN ExpressCenterShiftTime ec\n" +
                "    ON ec.Id = R.shift\n" +
                " INNER JOIN rvdbo.Zone Z\n" +
                "    ON Z.ZoneCode = R.zoneId\n" +
                "  --  where b.DATE\n" +
                " GROUP BY B.ACNO,\n" +
                "       R.firstName,\n" +
                "       R.lastName,\n" +
                "       Z.ZoneCode,\n" +
                "       Z.Name,\n" +
                "       BB.branchCode,\n" +
                "       BB.name,\n" +
                "       r.cid,\n" +
                "       r.userTypeId,\n" +
                "       ec.ShiftName,\n" +
                "       B.DATE\n" +
                "       )\n" +
                "       a where\n" +
                "             a.DATE between CONVERT(VARCHAR(10), '01-04-2016', 103) and CONVERT(VARCHAR(10), '03-08-2016', 103)\n" +
                "             AND a.ZoneCode = '1'\n" +
                "             AND a.branchCode = '2'\n" +
                "\n" +
                "             group by a.firstName+' '+a.lastName, a.firstName,a.lastName";


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


        public DataSet Get_RidersAttendanceRecord_New(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {

                string sqlString_old = "SELECT *\n" +
                "FROM   (\n" +
                "           SELECT ts.RiderCode,\n" +
                "                  ts.Name,\n" +
                "                  DAY(ts.[DateTime])     DAY,\n" +
                "                  CONVERT(varchar(15),CAST(ts.[DateTime] AS TIME),100) FIRSTOUT\n" +
                "           FROM   TimeStamping        AS ts\n" +
                "           WHERE  ts.[New State] = 'C/Out'\n" +
                "           AND year(ts.DateTime) = '" + clvar.StartDate + "' \n" +
                "           AND month(ts.DateTime) = '" + clvar.Month + "' \n" +
                "           AND month(ts.DateTime) = '" + clvar.Month + "' \n" + clvar.RiderCode + "'\n" +
                "       ) AS s\n" +
                "       PIVOT(\n" +
                "           MIN(FIRSTOUT)\n" +
                "           FOR DAY IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11],\n" +
                "                      [12], [13], [14], [15], [16], [17], [18], [19], [20], [21],\n" +
                "                      [22], [23], [24], [25], [26], [27], [28], [29], [30], [31])\n" +
                "       ) AS pvt";



                string sqlString = " SELECT *\n" +
                "FROM   (\n" +
                "select --ts.RiderCode, r.firstName+' '+r.lastName ridername, z.name zonename, b.name branchname ,\n" +
                "ts.RiderCode,\n" +
                // "                  ts.Name,\n" +
                //  "                  r.firstName+' ' +r.lastName ridername,\n" +
                "                  ts.Name Excel_RiderName,\n" +
                "                  r.firstName+' ' +r.lastName DB_ridername,\n" +
                "                  z.name zonename,\n" +
                "                  b.name branchname, \n" +
                "                  DAY(ts.[DateTime])     DAY,\n" +
                "               --   ts.[DateTime]          TIME\n" +
                "                 -- CONVERT(varchar(15), CAST(ts.[DateTime] AS TIME), 108) FIRSTOUT\n" +
                "                  CONVERT(varchar(15),CAST(ts.[DateTime] AS TIME),100) FIRSTOUT\n" +
                "FROM TimeStamping ts\n" +
                "inner join Riders r\n" +
                " on ts.RiderCode = r.riderCode\n" +
                " and r.riderCode not in ('N/A',\n" +
                "'na',\n" +
                "'na',\n" +
                "'new'\n" +
                ")\n" +
                "inner join ExpressCenters ec\n" +
                " on r.expressCenterId = ec.expressCenterCode\n" +
                "inner join Branches b\n" +
                " on ec.bid = b.branchCode\n" +
                "inner join Zones z\n" +
                " on b.zoneCode = z.zoneCode\n" +
                "  WHERE  ts.[New State] = 'C/Out'\n" +

                "  AND year(ts.DateTime) = '" + clvar.StartDate + "' \n" +
                "  AND month(ts.DateTime) = '" + clvar.Month + "' \n" + clvar.RiderCode + "\n" + clvar._Zone + clvar._TownCode +

                "\n" +
                "       ) AS s\n" +
                "       PIVOT(\n" +
                "           MIN(FIRSTOUT)\n" +
                "           FOR DAY IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11],\n" +
                "                      [12], [13], [14], [15], [16], [17], [18], [19], [20], [21],\n" +
                "                      [22], [23], [24], [25], [26], [27], [28], [29], [30], [31])\n" +
                "       ) AS pvt order by 2,3";



                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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



        #endregion

    }
}