using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for MaterialArrival
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class MaterialArrival
    {
        CommonFunction ComFunction = new CommonFunction();
        public MaterialArrival()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable VerifyBags(DataTable dt, Variable clvar)
        {
            DataTable VerifiedBags = new DataTable();
            string bags = "";
            foreach (DataRow dr in dt.Rows)
            {
                bags += "'" + dr[0].ToString() + "'";
            }

            bags = bags.Replace("''", "','");


            string sqlString = "select b.bagNumber,\n" +
            "       b.totalWeight weight,\n" +
            "       b.origin,\n" +
            "       br.name       OriginName,\n" +
            "       b.sealNo, '' Remarks\n" +
            "  from Bag b\n" +
            " inner join Branches br\n" +
            "    on b.branchCode = br.branchCode\n" +
            " where b.bagNumber in (" + bags + ")";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(VerifiedBags);
            }
            catch (Exception ex)
            {
                con.Close();
                return dt;
            }
            con.Close();
            return VerifiedBags;


        }
        public DataTable VerifyCNs(DataTable dt, Variable clvar)
        {
            DataTable VerifiedBags = new DataTable();
            string CNs = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString().Trim() != "")
                {
                    CNs += "'" + dr[0].ToString() + "'";
                }

            }

            CNs = CNs.Replace("''", "','");



            string sqlString = "selecT c.consignmentNumber ConsignmentNo,\n" +
            "       c.serviceTypeName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.orgin origin,\n" +
            "       b.name OriginName,\n" +
            "       c.weight, '' Pieces\n" +
            "  from Consignment c\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = c.orgin\n" +
            " where c.consignmentNumber in\n" +
            "       (" + CNs + ")";



            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(VerifiedBags);
            }
            catch (Exception ex)
            {
                con.Close();
                return dt;
            }
            con.Close();
            return VerifiedBags;
        }

        public string InsertMaterialArrival(List<string> bags, List<string> CNs, Variable clvar)
        {
            string bag = "";
            foreach (string str in bags)
            {
                if (str.Trim() != "")
                {
                    bag = "'" + str + "'";
                }

            }
            bag = bag.Replace("''", "','");
            string cn = "";
            foreach (string str in CNs)
            {
                if (str.Trim() != "")
                {
                    cn = "'" + str + "'";
                }

            }
            cn = cn.Replace("''", "','");
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



                string arrivalID = "";
                string query = "Insert Into MNP_MaterialArrival (BranchCode, CreatedON, CreatedBy, ExpressCenterCode, VehicleType, VehicleRegNo, CourierName, SealNo, Origin, Description) OUTPUT INSERTED.ArrivalID \n" +
                               " VALUES('" + HttpContext.Current.Session["BranchCode"].ToString() + "', GETDATE(), '" + HttpContext.Current.Session["U_ID"].ToString() + "', '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                               "'" + clvar.Vehicle + "', '" + clvar.VehicleNo + "', '" + clvar.CourierName + "', '" + clvar.Seal + "', '" + clvar.Orign + "', '" + clvar.Description + "') ";

                sqlcmd.CommandText = query;
                arrivalID = sqlcmd.ExecuteScalar().ToString();


                if (arrivalID == "0")
                {
                    trans.Rollback();
                    return "Error";
                }
                trans.Commit();
                sqlcon.Close();
                clvar.LoadingId = arrivalID.ToString();
                Insert_LoadingBag(clvar, bags);
                Insert_LoadingConsignment_NewByRabi(clvar, CNs);
                string sqlString = "Insert into MNP_MaterialArrivalDetail (ArrivalID, BranchCode, BagNumber, ManifestNumber, ConsignmentNumber, CreatedOn, CreatedBy)" +
                     "select '" + arrivalID + "', '" + HttpContext.Current.Session["BranchCode"].ToString() + "', a.*, GetDAte(), 'HELLO'\n" +
                "  from (select bm.bagNumber, bm.manifestNumber, cm.consignmentNumber CN\n" +
                "          from BagManifest bm\n" +
                "         inner join Mnp_ConsignmentManifest cm\n" +
                "            on cm.manifestNumber = bm.manifestNumber\n" +
                "         where bagNumber in (" + bag + ")\n" +
                "        union\n" +
                "        select ba.bagNumber, '' ManifestNumber, ba.outpieceNumber CN\n" +
                "          from BagOutpieceAssociation ba\n" +
                "         where ba.bagNumber in (" + bag + ")\n" +
                "\n" +
                "        union\n" +
                "\n" +
                "        select '' bagNumber, '' manifestNumber, c.consignmentNumber\n" +
                "          from Consignment c\n" +
                "         inner join BagOutpieceAssociation ba\n" +
                "            on ba.outpieceNumber = c.consignmentNumber\n" +
                "         where c.consignmentNumber in (" + cn + ")\n" +
                "           and ba.bagNumber not in (" + bag + ")) a\n" +
                " order by 1, 2, 3, 4";
                //sqlcmd.CommandText = sqlString;
                //sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "ERROR:" + ex.Message;
            }
            return clvar.LoadingId;
        }
        public void Insert_LoadingBag(Variable clvar, List<string> bags)
        {

            string query = "";
            SqlConnection orcl = new SqlConnection(clvar.Strcon2());
            orcl.Open();
            string altufaltu = "";
            foreach (string dr in bags)
            {
                try
                {
                    altufaltu = "MATERIAL ARRIVAL";
                    query = "insert into MNP_MaterialArrivalDetail (ArrivalID, BranchCode, bagNumber, createdBy, createdOn, Remarks) \n" +
                    "Values ('" + clvar.LoadingId + "', '" + HttpContext.Current.Session["BranchCode"].ToString() + "', '" + dr.Split(';')[0].ToString() + "', \n" +
                             "'" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), '" + dr.Split(';')[1].ToString() + "')";



                    SqlCommand orcd = new SqlCommand(query, orcl);
                    orcd.CommandType = CommandType.Text;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    orcd.ExecuteNonQuery();
                    altufaltu = "TRACKING";
                    string trackQuery = "insert into ConsignmentsTrackingHistory \n" +
                                "  (bagNumber, ConsignmentNumber, stateID, currentLocation, transactionTime, MaterialArrival)\n" +

                                "selecT ba.bagNumber,\n" +
                                "       ba.outpieceNumber CN, '20', \n" +
                                "       (select b.name from Branches b where b.branchCode = '4'), GETDATE(), '" + clvar.LoadingId + "'\n" +
                                "  from BagOutpieceAssociation ba\n" +
                                " where ba.bagNumber = '" + dr.ToString() + "'\n" +
                                "UNION\n" +
                                "selecT bm.bagNumber,\n" +
                                "       m.consignmentNumber CN,'20',\n" +
                                "       (select b.name from Branches b where b.branchCode = '4'), GETDATE(), '" + clvar.LoadingId + "'\n" +
                                "  from BagManifest bm\n" +
                                " inner join ConsignmentManifest m\n" +
                                "    on m.manifestNumber = bm.manifestNumber\n" +
                                " where bm.bagNumber = '" + dr.ToString() + "'";

                    orcd.CommandText = trackQuery;
                    orcd.ExecuteNonQuery();

                }
                catch (Exception Err)
                {
                    ComFunction.InsertErrorLog("", "", "", dr.ToString(), clvar.LoadingId, "", altufaltu, Err.Message);
                }
                finally
                { }
            }
            orcl.Close();

        }
        public void Insert_LoadingConsignment_NewByRabi(Variable clvar, List<string> CNs)
        {
            SqlConnection orcl = new SqlConnection(clvar.Strcon2());
            orcl.Open();
            string altufaltu = "";
            foreach (string dr in CNs)
            {
                try
                {
                    altufaltu = "MATERIAL ARRIVAL";
                    string query = "insert into MNP_MaterialArrivalDetail \n" +
                                    "  (ArrivalID, consignmentNumber, Pieces, BranchCode, createdBy, createdOn)\n" +
                                    "values\n" +
                                    "  ( \n" +
                                    "   '" + clvar.LoadingId + "',\n" +
                                    "   '" + dr.Split(';')[0].ToString() + "',\n" +
                                    "   '" + dr.Split(';')[1].ToString() + "',\n" +
                                    "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "', \n" +
                                    "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                    "   GETDATE()\n" +
                                    " ) ";

                    SqlCommand orcd = new SqlCommand(query, orcl);
                    orcd.CommandType = CommandType.Text;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    orcd.ExecuteNonQuery();
                    altufaltu = "TRACKING";
                    string trackQuery = "insert into ConsignmentsTrackingHistory(consignmentNumber, stateID, currentLocation, MaterialArrival, TransactionTime)\n" +
                        " VALUES ('" + dr.ToString() + "','20', (select name from Branches where BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'), '" + clvar.LoadingId + "', GETDATE())";
                    orcd.CommandText = trackQuery;
                    orcd.ExecuteNonQuery();

                }

                catch (Exception Err)
                {
                    ComFunction.InsertErrorLog(dr.ToString(), "", "", "", clvar.LoadingId, "", altufaltu, Err.Message);
                }
                finally
                { }
            }
            orcl.Close();

        }
        public void Insert_ConsignmentTrackingHistory(Variable clvar)
        {
            try
            {
                string query = "insert into ConsignmentsTrackingHistory \n" +
                                "  (consignmentNumber, stateID, currentLocation, manifestNumber, bagNumber, loadingNumber, mawbNumber, runsheetNumber, riderName, transactionTime)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar.ConsignmentNo + "',\n" +
                                "   '" + clvar.StateId + "',\n" +
                                "   '" + clvar.CityCode + "',\n" +
                                "   '" + clvar.Manifest + "',\n" +
                                "   '" + clvar.BagNumber + "',\n" +
                                "   '" + clvar.LoadingId + "',\n" +
                                "   '" + clvar.MawbNumber + "',\n" +
                                "   '" + clvar.RunsheetNumber + "',\n" +
                                "   '" + clvar.RiderCode + "',\n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                orcd.ExecuteNonQuery();
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
        }

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

        public DataTable GetAllBranches()
        {
            Cl_Variables clvar = new Cl_Variables();
            DataTable dt = new DataTable();
            string query = "select b.branchCode, b.name from Branches b where b.status = '1' and b.branchCode <> '17'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                con.Close();
                return dt;
            }
            con.Close();
            return dt;
        }
    }
}