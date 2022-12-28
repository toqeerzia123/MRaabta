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
    public class cl_material
    {

        public cl_material()
        {
        }

        Cl_Variables clvar = new Cl_Variables();

        #region Material CLASS


        public DataTable GetBagPrintReportHeader(Cl_Variables clvar)
        {
            /*
            string sqlString = "select b.arrivalid,\n" +
         //   "       b.totalWeight,\n" +
            "       b1.name       Origin,\n" +
           // "       b2.name       Destination,\n" +
            "       b3.name       Branch,\n" +
           // "       b.totalWeight,\n" +
            "       b.createdon\n" +
          //  "       b.sealNo , b.createdby\n" +
            "\n" +
            "  from MNP_MaterialArrival b\n" +
            " inner join Branches b1\n" +
            "    on b.origin = b1.branchCode\n" +
            //"\n" +
            //" inner join Branches b2\n" +
            //"    on b.destination = b2.branchCode\n" +
            "\n" +
            " inner join Branches b3\n" +
            "    on b.branchCode = b3.branchCode\n" +
            "\n" +
            " where b.arrivalid = '" + clvar.BagNumber + "'";

            sqlString = "select b.arrivalid,\n" +
          // "       b.totalWeight,\n" +
           "       b1.name       Origin,\n" +
         //  "       b2.name       Destination,\n" +
           "       b3.name       Branch,\n" +
        //   "       b.totalWeight,\n" +
           "       b.createdon\n" +
        //   "       b.sealNo ,  z.Name createdby\n" +
           "\n" +
           "  from MNP_MaterialArrival b\n" +
           " inner join Branches b1\n" +
           "    on b.origin = b1.branchCode\n" +
           "\n" +
           //" inner join Branches b2\n" +
           //"    on b.destination = b2.branchCode\n" +
           //"\n" +
           " inner join Branches b3\n" +
           "    on b.branchCode = b3.branchCode\n" +
           "\n" +
           " left outer join ZNI_USER1 z\n" +
           " on b.createdBy = CAST(z.U_ID as varchar)\n" +
           "\n" +
           " where b.arrivalid = '" + clvar.BagNumber + "'";

            */
            string sqlString = "select b.arrivalid,\n" +
            "       b3.name       Branch,\n" +
            "       b.createdon\n" +
            "  from MNP_MaterialArrival b\n" +
            " inner join Branches b3\n" +
            "    on b.origin = b3.branchCode\n" +
            " left outer join ZNI_USER1 z\n" +
            " on b.createdBy = CAST(z.U_ID as varchar)\n" +
            " where b.arrivalid = '" + clvar.BagNumber + "'";



            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable GetBagPrintReportDetail(Cl_Variables clvar)
        {

            string sqlString_old = "select c.bagnumber \n" +
           "  from MNP_MaterialArrival bp\n" +
           " inner join MNP_MaterialArrivalDetail c\n" +
           "    on c.arrivalid = bp.arrivalid\n" +
           " where bp.arrivalid = '" + clvar.BagNumber + "' \n" +
           "and c.bagnumber IS not NULL";


            string sqlString = "select c.bagnumber , b1.name orign, b2.name destination, c.remarks\n" +
            "  from MNP_MaterialArrival bp\n" +
            " inner join MNP_MaterialArrivalDetail c\n" +
            "    on c.arrivalid = bp.arrivalid\n" +
            " inner join Bag b\n" +
            "\ton c.bagnumber = b.bagNumber\n" +
            " inner join Branches b1\n" +
            "\ton b.origin = b1.branchCode\n" +
            " inner join Branches b2\n" +
            "  on b.destination = b2.branchCode\n" +
            " where bp.arrivalid = '" + clvar.BagNumber + "' \n" +
            "and c.bagnumber IS not NULL";


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable GetBagOutPieces(Cl_Variables clvar)
        {

            string sqlString_old = "select c.consignmentNumber \n" +
            "  from MNP_MaterialArrival bp\n" +
            " inner join MNP_MaterialArrivalDetail c\n" +
            "    on c.arrivalid = bp.arrivalid\n" +
            " where bp.arrivalid = '" + clvar.BagNumber + "' \n" +
            "and c.consignmentNumber IS not NULL";


            string sqlString = "select c.consignmentNumber , b1.name orign, b2.name destination, c.pieces \n" +
            "  from MNP_MaterialArrival bp\n" +
            " inner join MNP_MaterialArrivalDetail c\n" +
            "    on c.arrivalid = bp.arrivalid\n" +
            " inner join Consignment b\n" +
            "\ton c.consignmentNumber = b.consignmentNumber\n" +
            " inner join Branches b1\n" +
            "\ton b.orgin = b1.branchCode\n" +
            " inner join Branches b2\n" +
            "\ton b.destination = b2.branchCode\n" +
            " where bp.arrivalid = '" + clvar.BagNumber + "' \n" +
            "and c.consignmentNumber IS not NULL";




            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }


        #endregion

    }
}