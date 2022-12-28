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
    public class cl_DayClose
    {

        public cl_DayClose()
        {
        }

        Cl_Variables clvar = new Cl_Variables();

        #region DayClose CLASS

        public DataSet Get_DayCloseDate(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string query;
            try
            {
                // query = "SELECT DATEADD(DAY, -19, '" + clvar.BookingDate + "') date";
                query = "SELECT CONVERT(DATE,DATEADD(DAY, -1, '" + clvar.BookingDate + "'),105)date";

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

        #region MyRegion
        //public DataTable GetDayCloseReport(Cl_Variables clvar)
        //{
        //    string sqlString = "select c.consignmentNumber [CN No],\n" +
        //    "\t   c.consigner Consigner,\n" +
        //    "\t   c.consignee,\n" +
        //    "\t   c.serviceTypeName,\n" +
        //    "\t   b.name Origin,\n" +
        //    "\t   c.destination Destination,\n" +
        //    "\t   c.consignerAccountNo AccNo,\n" +
        //    "\t   c.weight,\n" +
        //    "\t   c.gst TAX,\n" +
        //    "\t   c.chargedAmount,\n" +
        //    "     case\n" +
        //    "\t\t\twhen c.isInsured = '1'\n" +
        //    "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
        //    "\t\t\telse c.totalAmount\n" +
        //    "\t\tend TotalAmount," +
        //    "\t   case\n" +
        //    "\t\t\twhen c.orgin = c.destination\n" +
        //    "\t\t\tthen 'Local'\n" +
        //    "\t\t\telse 'Domestic'\n" +
        //    "\t   end ConType\n" +
        //    "\n" +
        //    "  FROM Consignment c\n" +
        //    "  inner join Branches b\n" +
        //    "  on b.branchCode = c.orgin\n" +
        //    " where c.bookingDate = '" + clvar.Day + "'\n" +
        //    "   and c.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
        //    " order by contype, serviceTypeName";



        //    sqlString = "select c.bookingDate ,c.consignmentNumber [CN No], c.customerType,\n" +
        //   "\t   c.consigner Consigner,\n" +
        //   "\t   c.consignee,\n" +
        //   "\t   c.serviceTypeName,\n" +
        //   "\t   b.name Origin,\n" +
        //   "\t   (               \n" +
        //   "\t    SELECT b2.name    \n" +
        //   "\t    FROM   Branches b2    \n" +
        //   "\t   WHERE  b2.branchCode = c.destination    \n" +
        //   "\t )   DestinationBranch,   \n" +
        //   "\t  (        \n" +
        //   "\t      SELECT ec.name   \n" +
        //   "\t      FROM   ExpressCenters ec   \n" +
        //   "\t     WHERE  ec.expressCenterCode = c.destinationExpressCenterCode    \n" +
        //   "\t  )    Destination,     \n" +
        //   "\t   c.consignerAccountNo AccNo,\n" +
        //   "\t   c.weight,\n" +
        //   "\t   c.gst TAX,\n" +
        //   "\t   c.chargedAmount,\n" +
        //   "     case\n" +
        //   "\t\t\twhen c.isInsured = '1'\n" +
        //   "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
        //   "\t\t\telse c.totalAmount\n" +
        //   "\t\tend TotalAmount," +
        //   "\t   case when (select COUNT(vc.ConsignmentNo) from MNP_VOID_Consignment vc where vc.ConsignmentNo = c.consignmentNumber) > 0 then 'VOID'\n" +
        //   "\t\t\twhen c.orgin = c.destination\n" +
        //   "\t\t\tthen 'Local'\n" +
        //   "\t\t\telse 'Domestic'\n" +
        //   "\t   end ConType\n" +
        //   "\n" +
        //   "  FROM Consignment c\n" +
        //   "  inner join Branches b\n" +
        //   "  on b.branchCode = c.orgin\n" +
        //   " left outer join InternationalConsignmentDetail cd on cd.consignmentNo = c.consignmentNumber \n " +
        //   " where c.bookingDate = '" + clvar.Day + "'\n" +
        //   "   and c.expressCenterCode = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
        //   " order by contype, serviceTypeName";
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlConnection orcl = new SqlConnection(clvar.Strcon());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(sqlString, orcl);
        //        orcd.CommandTimeout = 300;
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(dt);
        //        orcl.Close();
        //    }
        //    catch (Exception)
        //    { }

        //    return dt;
        //} 
        #endregion
        public DataTable GetDayCloseReport(Cl_Variables clvar)
        {
            string sqlString = "select c.consignmentNumber [CN No], convert(varchar(11), c.bookingDate, 106) bookingDate,\n" +
           "\t   convert(varchar(11), c.createdOn, 106) createdOn, \n" +
            "\t   c.consigner Consigner,\n" +
            "\t   c.consignee,\n" +
            "\t   c.serviceTypeName,\n" +
            "\t   b.name Origin,\n" +
            "\t   c.destination Destination,\n" +
            "\t   c.consignerAccountNo AccNo,\n" +
            "\t   c.weight,\n" +
            "\t   c.gst TAX,\n" +
            "\t   c.chargedAmount,\n" +
            "     case\n" +
            "\t\t\twhen c.isInsured = '1'\n" +
            "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
            "\t\t\telse c.totalAmount\n" +
            "\t\tend TotalAmount," +
            "\t   case\n" +
            "\t\t\twhen c.orgin = c.destination\n" +
            "\t\t\tthen 'Local'\n" +
            "\t\t\telse 'Domestic'\n" +
            "\t   end ConType\n" +
            "\n" +
            "  FROM Consignment c\n" +
            "  inner join Branches b\n" +
            "  on b.branchCode = c.orgin\n" +
           " where CONVERT(date, c.createdOn, 105) = '" + clvar.Day + "'\n" +
           "   and c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
            " order by contype, serviceTypeName";



            sqlString = "select c.consignmentNumber [CN No], c.customerType,convert(varchar(11), c.bookingDate, 106) bookingDate,\n" +
           "\t   convert(varchar(11), c.createdOn, 106) createdOn, \n" +
           "\t   c.consigner Consigner,\n" +
           "\t   c.consignee,\n" +
           "\t   c.serviceTypeName,\n" +
           "\t   b.name Origin,\n" +
           "\t   (               \n" +
           "\t    SELECT b2.name    \n" +
           "\t    FROM   Branches b2    \n" +
           "\t   WHERE  b2.branchCode = c.destination    \n" +
           "\t )   DestinationBranch,   \n" +
           "\t  (        \n" +
           "\t      SELECT ec.name   \n" +
           "\t      FROM   ExpressCenters ec   \n" +
           "\t     WHERE  ec.expressCenterCode = c.destinationExpressCenterCode    \n" +
           "\t  )    Destination,     \n" +
           "\t   c.consignerAccountNo AccNo,\n" +
           "\t   c.weight,\n" +
           "\t   c.gst TAX,\n" +
           "\t   c.chargedAmount,\n" +
           "     case\n" +
           "\t\t\twhen c.isInsured = '1'\n" +
           "\t\t\tthen (c.decalaredValue * (c.insuarancePercentage/100)) + c.totalAmount\n" +
           "\t\t\telse c.totalAmount\n" +
           "\t\tend TotalAmount," +
           "\t   case when (select COUNT(vc.ConsignmentNo) from MNP_VOID_Consignment vc where vc.ConsignmentNo = c.consignmentNumber) > 0 then 'VOID'\n" +
           "\t\t\twhen c.orgin = c.destination\n" +
           "\t\t\tthen 'Local'\n" +
           "\t\t\telse 'Domestic'\n" +
           "\t   end ConType\n" +
           "\n" +
           "  FROM Consignment c\n" +
           "  inner join Branches b\n" +
           "  on b.branchCode = c.orgin\n" +
           " left outer join InternationalConsignmentDetail cd on cd.consignmentNo = c.consignmentNumber \n " +
           " where CONVERT(date, c.createdOn, 105) = '" + clvar.Day + "'\n" +
           "   and c.originExpressCenter = '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "'\n" +
           " order by contype, serviceTypeName";

            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 300;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }


        #endregion

    }
}