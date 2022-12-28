using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Cl_Invocie
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class Cl_Invocie
    {
        public Cl_Invocie()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Insert_InvoiceNote(Cl_Variables clvar)
        {
            string sql = "INSERT INTO Invoice_Note \n"
               + "( \n"
               + "	invoiceNumber, \n"
               + "	companyId, \n"
               + "	clientId, \n"
               + "	chkIsAuto, \n"
               + "	startDate, \n"
               + "	endDate, \n"
               + "	invoiceDate, \n"
               + "	createdBy, \n"
               + "	createdOn, \n"
               + "	modifiedBy, \n"
               + "	modifiedOn, \n"
               + "	totalAmount, \n"
               + "	overdueDate, \n"
               + "	deliveryStatus, \n"
               + "	BillNo, \n"
               + "	DiscountOnDomestic, \n"
               + "	DiscountOnDocument, \n"
               + "	DiscountOnSample, \n"
               + "	MonthlyFixCharges, \n"
               + "	IsInvoiceCanceled,Approved,Gst,Discount,Note_type,Note_Number,Fuel,Fuelgst,Reason \n"
               + ") \n"
               + "SELECT  \n"
               + "	invoiceNumber, \n"
               + "	companyId, \n"
               + "	clientId, \n"
               + "	chkIsAuto, \n"
               + "	startDate, \n"
               + "	endDate, \n"
               + "	invoiceDate, \n"
               + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
               + "	getdate(), \n"
               + "	modifiedBy, \n"
               + "	modifiedOn, \n"
               + "	'" + clvar.TotalAmount.ToString() + "', \n"
               + "	overdueDate, \n"
               + "	deliveryStatus, \n"
               + "	BillNo, \n"
               + "	DiscountOnDomestic, \n"
               + "	DiscountOnDocument, \n"
               + "	DiscountOnSample, \n"
               + "	MonthlyFixCharges, \n"
               + "	IsInvoiceCanceled, 0,'" + clvar.gst + "','" + clvar.Insurance + "','" + clvar.docPouchNo + "','" + clvar.RefNo + "','" + clvar.Flyer + "','" + clvar.expressionMessage + "', '" + clvar.RemarksID + "' \n"
               + "	 FROM Invoice i WHERE i.invoiceNumber ='" + clvar.InvoiceNo + "' \n"
               + "";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = sql;

                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
            }
        }

        public DataTable Active_Zones(Cl_Variables clvar)
        {
            string sql = "SELECT * \n"
                 + "FROM   Zones        z, \n"
                 + "       Branches     b \n"
                 + "WHERE  b.zoneCode = z.zoneCode \n"
                 + "       AND b.MainBranch = '1' \n"
                 + "ORDER BY b.branchCode ";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable products(Cl_Variables clvar)
        {
            string sql = "SELECT stn.Products FROM ServiceTypes_New stn \n"
                    + "GROUP BY stn.Products";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable branch(Cl_Variables clvar)
        {
            string sql = "SELECT b.branchCode, \n"
                + "       b.name \n"
                + "FROM   Zones        z, \n"
                + "       Branches     b \n"
                + "WHERE  b.zoneCode = z.zoneCode \n"
                + "       AND b.zoneCode = '" + clvar.Zone + "' and b.status ='1' \n"
                + "ORDER BY \n"
                + "       CAST(b.branchCode AS INT) \n"
                + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable ExpressCenter(Cl_Variables clvar)
        {
            string sql = "SELECT ec.expressCenterCode, \n"
               + "       ec.expressCenterCode + ' - ' + ec.name NAME \n"
               + "FROM   Zones              z, \n"
               + "       Branches           b, \n"
               + "       ExpressCenters     ec \n"
               + "WHERE  b.zoneCode = z.zoneCode \n"
               + "       AND ec.bid = b.branchCode \n"
               + "       AND b.zoneCode = '" + clvar.Zone + "' \n"
               + "       AND b.branchCode = '" + clvar.Branch + "' and ec.status ='1' \n"
               + "ORDER BY \n"
               + "       ec.name \n"
               + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Rider(Cl_Variables clvar)
        {
            string sql = "SELECT r.expressCenterId, r.riderCode, \n"
                + "       r.riderCode + ' - ' + r.firstName NAME \n"
                + "FROM   Zones              z, \n"
                + "       Branches           b, \n"
                + "       ExpressCenters     ec,Riders r \n"
                + "WHERE  b.zoneCode = z.zoneCode \n"
                + "       AND ec.bid = b.branchCode \n"
                + "       AND r.branchId = b.branchCode \n"
                + "       AND r.expressCenterId = ec.expressCenterCode \n"
                + "       AND b.zoneCode = '" + clvar.Zone + "' \n"
                + "       AND ec.expressCenterCode ='" + clvar.expresscenter + "' \n"
                + "       AND b.branchCode = '" + clvar.Branch + "' \n"
                + "ORDER BY \n"
                + "       ec.name \n"
                + " \n"
                + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Zone_Sequence(Cl_Variables clvar)
        {
            string sql = " \n"
               + "SELECT z.name Zone, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.EndSequence, \n"
               + "       mzc.Product,\n"
               + "       mzc.EndSequence -  mzc.SequenceStart Qty, \n"
               + "       mzc.Created_On,\n"
               + "       zu.U_NAME, \n"
               + "       (             \n"
               + "               SELECT COUNT(c.consignmentNumber)   \n"
               + "                FROM   Consignment c             \n"
               + "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               + "                       AND CAST(mzc.EndSequence AS VARCHAR)  \n"
               + "                       AND c.zoneCode = mzc.ZoneCode   \n"
               + "            ) usage   \n"
               + "FROM   Mnp_ZoneCNSquence     mzc, \n"
               + "       Zones                 z, \n"
               + "       ZNI_USER1             zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND zu.U_ID = mzc.Created_By";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 30;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Brancg_Sequence(Cl_Variables clvar)
        {
            string sql = "SELECT z.name zone, \n"
               + "       b.sname Bname, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.SequenceEnd, \n"
               + "       mzc.Product, \n"
               + "       mzc.SequenceEnd -  mzc.SequenceStart Qty, \n"
               + "       zu.U_NAME                CreatedBy, \n"
               + "       mzc.Created_On,\n"
               + "       (             \n"
               + "               SELECT COUNT(c.consignmentNumber)   \n"
               + "                FROM   Consignment c             \n"
               + "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               + "                       AND CAST(mzc.SequenceEnd AS VARCHAR)  \n"
               + "                       AND c.zoneCode = mzc.ZoneCode  and c.orgin=  mzc.Branch \n"
               + "            ) usage   \n"
               + "FROM   Mnp_BranchCNSequence_     mzc, \n"
               + "       Zones                    z, \n"
               + "       Branches                 b, \n"
               + "       ZNI_USER1                zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND b.branchCode = mzc.Branch \n"
               + "       AND zu.U_ID = mzc.Created_By";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Brancg_Sequence_(Cl_Variables clvar)
        {
            string sql = "SELECT z.name zone, \n"
               + "       b.sname Bname, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.SequenceEnd, \n"
               + "       mzc.Product, \n"
               + "       mzc.SequenceEnd -  mzc.SequenceStart Qty, \n"
               + "       zu.U_NAME                CreatedBy, \n"
               + "       mzc.Created_On, \n"
               + "       (             \n"
               + "               SELECT COUNT(c.consignmentNumber)   \n"
               + "                FROM   Consignment c             \n"
               + "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               + "                       AND CAST(mzc.SequenceEnd AS VARCHAR)  \n"
               + "                       AND c.zoneCode = mzc.ZoneCode  and c.orgin=  mzc.Branch \n"
               + "            ) usage   \n"
               + "FROM   Mnp_BranchCNSequence_     mzc, \n"
               + "       Zones                    z, \n"
               + "       Branches                 b, \n"
               + "       ZNI_USER1                zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND b.branchCode = mzc.Branch \n"
               + "       AND zu.U_ID = mzc.Created_By"
               + "       AND b.branchCode ='" + clvar.Branch + "'\n";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Brancg_Sequence_1(Cl_Variables clvar)
        {
            string sql = "SELECT z.name zone, \n"
               + "       b.sname Bname, \n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.SequenceEnd, \n"
               + "       mzc.Product, \n"
               + "       mzc.SequenceEnd -  mzc.SequenceStart Qty, \n"
               + "       zu.U_NAME                CreatedBy, \n"
               + "       mzc.Created_On, \n"
               + "       (             \n"
               + "               SELECT COUNT(c.consignmentNumber)   \n"
               + "                FROM   Consignment c             \n"
               + "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               + "                       AND CAST(mzc.SequenceEnd AS VARCHAR)  \n"
               + "                       AND c.zoneCode = mzc.ZoneCode  and c.orgin=  mzc.Branch \n"
               + "            ) usage   \n"
               + "FROM   Mnp_BranchCNSequence_     mzc, \n"
               + "       Zones                    z, \n"
               + "       Branches                 b, \n"
               + "       ZNI_USER1                zu \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND b.branchCode = mzc.Branch \n"
               + "       AND zu.U_ID = mzc.Created_By"
               + "       AND b.branchCode ='" + clvar.Branch + "'\n"
               + "       AND mzc.Product ='" + clvar.productDescription + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public string Insert_ZoneSeq(Cl_Variables clvar)
        {
            string return_ = "0";
            string sql = "INSERT INTO Mnp_ZoneCNSquence \n"
                  + "( \n"
                  + "	-- ID -- this column value is auto-generated \n"
                  + "	ZoneCode, \n"
                  + "	SequenceStart, \n"
                  + "	EndSequence, \n"
                  + "	Product, \n"
                  + "	[Status], \n"
                  + "	Created_By, \n"
                  + "	Created_On \n"
                  + ") \n"
                  + "VALUES \n"
                  + "( \n"
                  + "	'" + clvar.Zone + "', \n"
                  + "	'" + clvar.startsequence + "', \n"
                  + "	'" + clvar.endsequence + "', \n"
                  + "	'" + clvar.productDescription + "', \n"
                  + "	'1', \n"
                  + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
                  + "	Getdate() \n"
                  + ")";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = sql;

                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
                return_ = "1";

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return_ = "0";
            }

            return return_;

        }

        public string Insert_ProductSeq(Cl_Variables clvar)
        {
            string return_ = "0";
            string sql = "INSERT INTO Mnp_PurchaseCNSquence \n"
                  + "( \n"
                  + "	-- ID -- this column value is auto-generated \n"
                  + "	PurchaseOrderID, \n"
                  + "	PurchaseOrderDate, \n"
                  + "	StartSequence, \n"
                  + "	EndSequence, \n"
                  + "	Product, \n"
                  + "	CNCount, \n"
                  + "	CNIssued, \n"
                  + "	Status, \n"
                  + "	Created_By, \n"
                  + "	Created_On \n"
                  + ") \n"
                  + "VALUES \n"
                  + "( \n"
                  + "	'" + clvar.pruchaseorder + "', \n"
                  + "	'" + clvar.pruchasedate + "', \n"
                  + "	'" + clvar.Zone + "', \n"
                  + "	'" + clvar.startsequence + "', \n"
                  + "	'" + clvar.endsequence + "', \n"
                  + "	'" + clvar.productDescription + "', \n"
                  + "    '" + (Int64.Parse(clvar.endsequence) - Int64.Parse(clvar.startsequence)).ToString() + "', \n"
                  + "	'0', \n"
                  + "	'1', \n"
                  + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
                  + "	Getdate() \n"
                  + ")";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = sql;

                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
                return_ = "1";

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return_ = "0";
            }

            return return_;

        }


        public string Insert_BranchSeq(Cl_Variables clvar)
        {
            string return_ = "0";
            string sql = "INSERT INTO Mnp_BranchCNSequence_ \n"
                  + "( \n"
                  + "	-- ID -- this column value is auto-generated \n"
                  + "	ZoneCode, \n"
                  + "   Branch, \n"
                  + "	SequenceStart, \n"
                  + "	SequenceEnd, \n"
                  + "	Product, \n"
                  + "	[Status], \n"
                  + "	Created_By, \n"
                  + "	Created_On \n"
                  + ") \n"
                  + "VALUES \n"
                  + "( \n"
                  + "	'" + clvar.Zone + "', \n"
                  + "	'" + clvar.Branch + "', \n"
                  + "	'" + clvar.startsequence + "', \n"
                  + "	'" + clvar.endsequence + "', \n"
                  + "	'" + clvar.productDescription + "', \n"
                  + "	'1', \n"
                  + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
                  + "	Getdate() \n"
                  + ")";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = sql;

                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
                return_ = "1";

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return_ = "0";
            }

            return return_;

        }

        public string Insert_expressSeq(Cl_Variables clvar)
        {
            string return_ = "0";
            string sql = "INSERT INTO Mnp_RiderCNSequence \n"
                  + "( \n"
                  + "	-- ID -- this column value is auto-generated \n"
                  + "	ZoneCode, \n"
                  + "   Branch, \n"
                  + "   ExpressCenter, \n"
                  + "   Rider, \n"
                  + "   Type, \n"
                  + "	SequenceStart, \n"
                  + "	SequenceEnd, \n"
                  + "	Product, \n"
                  + "	[Status], \n"
                  + "	Created_By, \n"
                  + "	Created_On \n"
                  + ") \n"
                  + "VALUES \n"
                  + "( \n"
                  + "	'" + clvar.Zone + "', \n"
                  + "	'" + clvar.Branch + "', \n"
                  + "	'" + clvar.expresscenter + "', \n"
                  + "	'" + clvar.RiderCode + "', \n"
                  + "	'" + clvar.Day + "', \n"
                  + "	'" + clvar.startsequence + "', \n"
                  + "	'" + clvar.endsequence + "', \n"
                  + "	'" + clvar.productDescription + "', \n"
                  + "	'1', \n"
                  + "	'" + HttpContext.Current.Session["U_ID"].ToString() + "', \n"
                  + "	Getdate() \n"
                  + ")";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = sql;

                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
                return_ = "1";

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return_ = "0";
            }

            return return_;

        }

        public DataTable Express_Sequence(Cl_Variables clvar)
        {
            string sql = "/************************************************************ \n"
               + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
               + " * Time: 01/12/2016 7:38:13 PM \n"
               + " ************************************************************/ \n"
               + " \n"
               + "SELECT z.name                  zone, \n"
               + "       b.sname                 Bname, \n"
               + "       ec.name              ECName, \n"
               + "       r.firstName +' '+r.lastName RiderName,\n"
               + "       mzc.SequenceStart, \n"
               + "       mzc.SequenceEnd, \n"
               + "       mzc.Product, \n"
               + "       mzc.SequenceEnd - mzc.SequenceStart Qty, \n"
               + "       zu.U_NAME               CreatedBy, \n"
               + "       mzc.Created_On, \n"
               + "       (             \n"
               + "               SELECT COUNT(c.consignmentNumber)   \n"
               + "                FROM   Consignment c             \n"
               + "                WHERE  CAST(c.consignmentNumber AS VARCHAR(16)) BETWEEN CAST(mzc.SequenceStart AS VARCHAR)   \n"
               + "                       AND CAST(mzc.SequenceEnd AS VARCHAR)  \n"
               + "                       AND c.zoneCode = mzc.ZoneCode  and c.orgin=  mzc.Branch and c.originexpresscenter= mzc.ExpressCenter\n"
               + "            ) usage   \n"

               + "FROM   Mnp_RiderCNSequence     mzc, \n"
               + "       Zones                   z, \n"
               + "       Branches                b,  \n"
               + "       ZNI_USER1               zu, \n"
               + "       ExpressCenters         ec,Riders r \n"
               + "WHERE  z.zoneCode = mzc.ZoneCode \n"
               + "       AND b.branchCode = mzc.Branch \n"
               + "       AND ec.bid = b.branchCode \n"
               + "       AND ec.expresscentercode = mzc.ExpressCenter \n"
               + "       AND zu.U_ID = mzc.Created_By \n"
               + "       AND mzc.Rider = r.riderCode \n"
               + "       AND r.branchId = b.branchCode";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable SequenceCheck(Cl_Variables clvar)
        {
            string sql = "SELECT * \n"
                + "FROM Mnp_RiderCNSequence mzc \n"
                + "WHERE   mzc.Branch='" + clvar.Branch + "' AND mzc.ZoneCode='" + clvar.Zone + "' AND mzc.ExpressCenter ='" + clvar.expresscenter + "' \n"
                + "       AND '" + clvar.manifestNo + "' BETWEEN mzc.SequenceStart AND mzc.SequenceEnd";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 120;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        public DataTable SequenceCheck_Branch(Cl_Variables clvar)
        {
            string sql = "SELECT * \n"
                + "FROM Mnp_RiderCNSequence mzc \n"
                + "WHERE   mzc.Branch='" + clvar.Branch + "' AND mzc.ZoneCode='" + clvar.Zone + "' AND mzc.ExpressCenter ='" + clvar.expresscenter + "' \n"
                + "        AND '" + clvar.manifestNo + "' BETWEEN mzc.SequenceStart AND mzc.SequenceEnd";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.CommandTimeout = 120;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        public DataTable Invoice_AdjustmentNumber(Cl_Variables clvar)
        {
            string sql = "  SELECT isnull(codeValue,'0') codeValue \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'INVOICE AD' \n"
               + "		                       AND identifier = '1' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable InvoiceAddApproval(Cl_Variables clvar)
        {
            string sql = " \n"
               + "SELECT in1.invoiceNumber, \n"
               + "       in1.companyId,(Select companyName from company where id=in1.companyId) companyName, \n"
               + "       cc.name       ClientName, \n"
               + "       cc.accountNo, \n"
               + "       z.name        Zonename, \n"
               + "       b.name        OriginName, \n"
               + "       in1.TotalAmount, \n"
               + "       in1.Gst, \n"
               + "       zu.U_NAME     CreatedBy, \n"
               + "       in1.Note_type, \n"
               + "       in1.Approved, in1.Note_Number,\n"
               + "       IN1.Discount,\n"
               + "       in1.Fuel,\n"
               + "       in1.Fuelgst\n"
               + "FROM   Invoice_Note in1 \n"
               + "       INNER JOIN InvoiceConsignment_Note icn \n"
               + "            ON  icn.invoiceNumber = in1.invoiceNumber \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  in1.clientId = cc.id \n"
               + "            AND icn.Account_No = cc.accountNo \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.branchCode = cc.branchCode \n"
               + "       INNER JOIN ZNI_USER1 zu \n"
               + "            ON  icn.createdBy = zu.U_ID \n"
               + "       INNER JOIN Zones z \n"
               + "            ON  b.zoneCode = z.zoneCode \n"
               + "WHERE  CAST(icn.createdOn AS date) >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' \n"
               + "       AND CAST(icn.createdOn AS date) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "' \n"
               + "       and in1.companyId ='" + clvar.Company + "'\n"
               + "       and in1.Approved = '0' "
               + "GROUP BY \n"
               + "       in1.invoiceNumber, \n"
               + "       in1.companyId, \n"
               + "       cc.name, \n"
               + "       cc.accountNo, \n"
               + "       z.name, \n"
               + "       b.name, \n"
               + "       in1.TotalAmount, \n"
               + "       in1.Gst, \n"
               + "       zu.U_NAME, \n"
               + "       in1.Note_type, \n"
               + "       in1.Approved, \n"
               + "       in1.Note_Number,\n"
               + "       IN1.Discount,\n"
               + "       in1.Fuel,\n"
               + "       in1.Fuelgst\n";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        public DataTable Company(Cl_Variables clvar)
        {
            string sql = " \n"
               + "Select * from Company";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        public void updateInvoice_AdjustmentNumber(Cl_Variables clvar)
        {
            string sql = "  update  SystemCodes set codeValue= '" + clvar.RefNo + "' \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'INVOICE AD' \n"
               + "		                       AND identifier = '1' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            //  return dt;
        }

        public DataTable Invoice_CCNumber(Cl_Variables clvar)
        {
            string sql = "  SELECT isnull(codeValue,'0') codeValue,identifier,year \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'CREDIT NOTE' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable Invoice_DDNumber(Cl_Variables clvar)
        {
            string sql = "  SELECT isnull(codeValue,'0') codeValue,identifier,year \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'DEBIT NOTE' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public void UpdateInvoice_CCNumber(Cl_Variables clvar)
        {
            string sql = "  update  SystemCodes set codeValue= '" + clvar.RefNo + "' \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'CREDIT NOTE' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            //  return dt;
        }

        public void UpdateInvoice_DDNumber(Cl_Variables clvar)
        {
            string sql = "  update  SystemCodes set codeValue= '" + clvar.RefNo + "' \n"
               + "		                FROM   SystemCodes \n"
               + "		                WHERE  codeType = 'DEBIT NOTE' \n"
               + "		                       AND [year] = CONVERT(VARCHAR, YEAR(getdate()))";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            //  return dt;
        }

        public void UpdateInvoice_Note(Cl_Variables clvar)
        {
            string sql = " UPDATE Invoice_Note \n"
               + " SET Approved = '1' \n"
               + " WHERE Note_Number ='" + clvar.NoteNumber + "'AND invoiceNumber='" + clvar.InvoiceNo + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            //  return dt;
        }

        public string CreditNOteInsert_GV(Cl_Variables clvar)
        {
            string flag = "0";
            // // Updateing Flag

            string sql = " UPDATE Invoice_Note \n"
              + " SET Approved = '1' \n"
              + " WHERE Note_Number ='" + clvar.NoteNumber + "'AND invoiceNumber='" + clvar.InvoiceNo + "'";


            //Updating Credit Note
            string sql_ = "INSERT INTO GeneralVoucher_Temp \n"
                + "  ( \n"
                + "    -- Id -- this column value is auto-generated \n"
                + "    VoucherDate, \n"
                + "    IsByCreditClient, \n"
                + "    IsByExpressCenter, \n"
                + "    ExpressCenterCode, \n"
                + "    RiderCode, \n"
                + "    CreditClientId, \n"
                + "    InvoiceNo, \n"
                + "    PaymentTypeId, \n"
                + "    Amount, \n"
                + "    BranchCode, \n"
                + "    ZoneCode, \n"
                + "    CreatedOn, \n"
                + "    CreatedBy \n"
                + "  ) \n"
                + "SELECT GETDATE(), \n"
                + "       '1', \n"
                + "       NULL, \n"
                + "       NULL, \n"
                + "       NULL, \n"
                + "       in1.clientId, \n"
                + "       in1.invoiceNumber, \n"
                + "       2, \n"
                + "       (in1.totalAmount + in1.Gst), \n"
                + "       ( \n"
                + "           SELECT cc.branchCode \n"
                + "           FROM   CreditClients cc \n"
                + "           WHERE  cc.id = in1.clientId \n"
                + "       ), \n"
                + "       ( \n"
                + "           SELECT ( \n"
                + "                      SELECT b.zoneCode \n"
                + "                      FROM   branches b \n"
                + "                      WHERE  b.branchCode = cc.branchCode \n"
                + "                  ) \n"
                + "           FROM   CreditClients cc \n"
                + "           WHERE  cc.id = in1.clientId \n"
                + "       ), \n"
                + "       GETDATE(), \n"
                + "       '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n"
                + "FROM   Invoice_Note in1 \n"
                + "WHERE  in1.invoiceNumber = '" + clvar.InvoiceNo + "' \n"
                + "       AND in1.Note_Number = '" + clvar.NoteNumber + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;

            SqlCommand cmd = new SqlCommand();
            SqlCommand cmd1 = new SqlCommand();
            cmd.Connection = con;
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd.Transaction = transaction;
                cmd1.Transaction = transaction;

                cmd.CommandType = CommandType.Text;
                cmd1.CommandType = CommandType.Text;

                cmd.CommandText = sql;
                cmd1.CommandText = sql_;
                cmd.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return ex.Message;
                flag = "1";
            }

            return flag;

        }

        public string DebitNoteInsert_Invoice(Cl_Variables clvar)
        {
            string flag = "0";

            string sql = " UPDATE Invoice_Note \n"
            + " SET Approved = '1' \n"
            + " WHERE Note_Number ='" + clvar.NoteNumber + "'AND invoiceNumber='" + clvar.InvoiceNo + "'";

            string sql_ = "INSERT INTO Invoice_temp \n"
                + "( \n"
                + "	invoiceNumber, \n"
                + "	companyId, \n"
                + "	clientId, \n"
                + "	chkIsAuto, \n"
                + "	startDate, \n"
                + "	endDate, \n"
                + "	invoiceDate, \n"
                + "	createdBy, \n"
                + "	createdOn, \n"
                + "	modifiedBy, \n"
                + "	modifiedOn, \n"
                + "	totalAmount, \n"
                + "	overdueDate, \n"
                + "	deliveryStatus, \n"
                + "	BillNo, \n"
                + "	DiscountOnDomestic, \n"
                + "	DiscountOnDocument, \n"
                + "	DiscountOnSample, \n"
                + "	MonthlyFixCharges \n"
                + ") \n"
                + "SELECT  \n"
                + "	invoiceNumber, \n"
                + "	companyId, \n"
                + "	clientId, \n"
                + "	chkIsAuto, \n"
                + "	startDate, \n"
                + "	endDate, \n"
                + "	invoiceDate, \n"
                + "	createdBy, \n"
                + "	createdOn, \n"
                + "	modifiedBy, \n"
                + "	modifiedOn, \n"
                + "	(totalAmount + gst), \n"
                + "	overdueDate, \n"
                + "	deliveryStatus, \n"
                + "	BillNo, \n"
                + "	DiscountOnDomestic, \n"
                + "	DiscountOnDocument, \n"
                + "	DiscountOnSample, \n"
                + "	MonthlyFixCharges \n"
                + "	FROM Invoice_note  WHERE Note_Number='" + clvar.NoteNumber + "'"
                + "  AND invoiceNumber = '" + clvar.InvoiceNo + "'";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;

            SqlCommand cmd = new SqlCommand();
            SqlCommand cmd1 = new SqlCommand();
            cmd.Connection = con;
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd.Transaction = transaction;
                cmd1.Transaction = transaction;

                cmd.CommandType = CommandType.Text;
                cmd1.CommandType = CommandType.Text;

                cmd.CommandText = sql;
                cmd1.CommandText = sql_;
                cmd.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();

                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return ex.Message;
                flag = "1";
            }

            return flag;
        }

        public DataTable Report_View(Cl_Variables clvar)
        {
            string sql = "/************************************************************ \n"
                + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
                + " * Time: 11/05/2017 11:42:04 AM \n"
                + " ************************************************************/ \n"
                + " \n"
                + "  \n"
                + "SELECT in1.invoiceNumber, \n"
                + "       in1.companyId, \n"
                + "       ( \n"
                + "           SELECT companyName \n"
                + "           FROM   company \n"
                + "           WHERE  id = in1.companyId \n"
                + "       )                                  companyName, \n"
                + "       cc.name                            ClientName, \n"
                + "       cc.accountNo, \n"
                + "       cc.[address]                       ClientAddress, \n"
                + "       cc.salesTaxNo, \n"
                + "       cc.ntnNo, \n"
                + "       z.name                             Zonename, \n"
                + "       b.sname                            OriginName, \n"
                + "       in1.TotalAmount, \n"
                + "       in1.Gst, \n"
                + "       zu.U_NAME                          CreatedBy, \n"
                + "       in1.Note_type, \n"
                + "       in1.Approved, \n"
                + "       in1.Note_Number, \n"
                + "       IN1.Discount, \n"
                + "       in1.Fuel, \n"
                + "       in1.Fuelgst, \n"
                + "       in1.invoiceDate, \n"
                + "       (Select mias.[Status] from Mnp_InvoiceAdjustment_Status mias WHERE mias.ID = in1.Reason) i_AdjReason, \n"
                + "       DATENAME(MONTH, in1.startDate)  AS 'MonthName' \n"
                + "FROM   Invoice_Note in1 \n"
                + "       INNER JOIN InvoiceConsignment_Note icn \n"
                + "            ON  icn.invoiceNumber = in1.invoiceNumber \n"
                + "       INNER JOIN CreditClients cc \n"
                + "            ON  in1.clientId = cc.id \n"
                + "            AND icn.Account_No = cc.accountNo \n"
                + "       INNER JOIN Branches b \n"
                + "            ON  b.branchCode = cc.branchCode \n"
                + "       INNER JOIN ZNI_USER1 zu \n"
                + "            ON  icn.createdBy = zu.U_ID \n"
                + "       INNER JOIN Zones z \n"
                + "            ON  b.zoneCode = z.zoneCode \n"
                + "       \n"
                + "WHERE  in1.Note_Number = '" + clvar.NoteNumber + "' \n"
                + "GROUP BY \n"
                + "       in1.invoiceNumber, \n"
                + "       in1.companyId, \n"
                + "       cc.name, \n"
                + "       cc.accountNo, \n"
                + "       z.name, \n"
                + "       b.sname, \n"
                + "       in1.TotalAmount, \n"
                + "       in1.Gst, \n"
                + "       zu.U_NAME, \n"
                + "       in1.Note_type, \n"
                + "       in1.Approved, \n"
                + "       in1.Note_Number, \n"
                + "       IN1.Discount, \n"
                + "       in1.Fuel, \n"
                + "       in1.Fuelgst, \n"
                + "       cc.[address], \n"
                + "       cc.salesTaxNo, \n"
                + "       cc.ntnNo, \n"
                + "       in1.invoiceDate, in1.Reason, \n"
                + "       DATENAME(MONTH, in1.startDate) \n"
                + "";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        public DataTable Report_View_Detail(Cl_Variables clvar)
        {
            string sql = "SELECT icn.consignmentNumber, \n"
                + "       CAST(icn.TotalAmount AS DECIMAL) + CAST(icn.Gst AS DECIMAL) CNAMount, \n"
                + "       CAST(icn.NewAmount AS DECIMAL) + CAST(icn.NewGst AS DECIMAL) NewAmount, \n"
                + "       CASE  \n"
                + "            WHEN ( \n"
                + "                     ( CAST(icn.TotalAmount AS DECIMAL) + CAST(icn.Gst AS DECIMAL)) -( CAST(icn.NewAmount AS DECIMAL) + CAST(icn.NewGst AS DECIMAL)) \n"
                + "                 ) > 0 THEN ( CAST(icn.TotalAmount AS DECIMAL) + CAST(icn.Gst AS DECIMAL)) -( CAST(icn.NewAmount AS DECIMAL) + CAST(icn.NewGst AS DECIMAL)) \n"
                + "            ELSE (CAST(icn.NewAmount AS DECIMAL)+ CAST(icn.NewGst AS DECIMAL)) -(CAST(icn.TotalAmount AS DECIMAL) + CAST(icn.Gst AS DECIMAL)) \n"
                + "       END                         Diff \n"
                + "FROM   InvoiceConsignment_Note     icn \n"
                + "WHERE  icn.Note_number = '" + clvar.NoteNumber + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

    }
}