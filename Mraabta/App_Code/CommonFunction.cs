using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CommonFunction
/// </summary>
/// 
namespace MRaabta.App_Code
{
    public class CommonFunction
    {
        public CommonFunction()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        Cl_Variables clvar = new Cl_Variables();

        public DataTable Companies()
        {
            string query = "SELECT ID, COMPANYNAME from COMPANY where id in (1,4)";
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
        public DataTable GetRemarks()
        {
            string query = "SELECT CAST(COMPANY as VARCHAR) + '-' + ID RemarksID, SDESC, LDESC from MNP_VOIDCN_REMARKS where status = '1'";
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

        public DataSet Branch()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' and branchcode='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "' \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name,sname order by b.name ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Branch_()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname BName \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1'  \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name,sname order by b.name ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet Branch(Cl_Variables var)
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName, sname \n"
               + "FROM   Branches                          b \n"
               + "where b.[status] ='1' \n"
               + " and  b.branchCode = '" + var.Branch + "'"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name, sname";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataTable GetBranchesByCompany(Cl_Variables clvar)
        {

            string sqlString = "select b.name, b.branchCode, bg.branchCode bc_\n" +
            "  from BranchGST bg\n" +
            " right outer join Branches b\n" +
            "    on b.branchCode = bg.branchCode\n" +
            " where bg.companyId = '" + clvar.Company + "'\n" +
            " group by b.name, b.branchCode, bg.branchCode\n" +
            " order by name";
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

        public DataSet ConsignmentType()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = " SELECT ct.id, ct.name ConsignmentType FROM ConsignmentType ct  \n"
                + "       WHERE ct.[status]='1' \n"
                + "       GROUP BY ct.name,ct.id";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ServiceTypeName()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = @"SELECT st.name ServiceTypeName 
                                FROM ServiceTypes st 
                                WHERE st.IsIntl = '0' AND st.[status] = '1' And st.name not in ('Expressions','Road n Rail') 
                                and serviceTypeName <> case when (select ExpressCenterCode from MnP_Retail_Staff where status = 1 and EmployeeUsername = '"+ HttpContext.Current.Session["U_NAME"].ToString() + "') = '0117' then '' else 'DC Box' end ORDER BY st.name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ServiceTypeName_()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT st.name          ServiceTypeName, isintl \n"
                + "FROM   ServiceTypes     st \n"
                + "WHERE  \n"
                + "       st.[status] = '1' \n"
                + "       And st.name not in ('Expressions') \n"
                + "GROUP BY \n"
                + "       st.name,isintl  \n"
                + "ORDER BY \n"
                + "       st.name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet PriceModifiers()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT id, \n"
               + "       pm.name PriceModifier, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], calculationbase \n"
               + "FROM   PriceModifiers pm \n"
               + "WHERE  pm.[status] = '1' \n"
               + "AND pm.chkBillingModifier ='0' \n"
               + "GROUP BY \n"
               + "       id, \n"
               + "       pm.name, \n"
               + "       pm.calculationValue, \n"
               + "       pm.[description], pm.calculationbase \n"
               + "ORDER BY pm.name";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataTable Cities()
        {
            string query = "SELECT * FROM CITIES ORDER BY CITYNAME";

            string sql = "SELECT A.bid, \n"
               + "       A.CategoryId, \n"
               + "       A.ClientId, \n"
               + "       A.costCenterCode, \n"
               + "       A.description     NAME, \n"
               + "       A.expressCenterCode, \n"
               + "       A.sname, \n"
               + "       A.ServiceName,ServiceID, \n"
               + "       A.ZoneCode, \n"
               + "       A.CityId, \n"
               + "       A.description     EName \n"
               + "FROM   ( \n"
               + "           SELECT e.expresscentercode, \n"
               + "                  e.bid, \n"
               + "                  e.CategoryId, \n"
               + "                  e.ClientId, \n"
               + "                  e.costCenterCode, \n"
               + "                  e.description, \n"
               + "                  e.sname, \n"
               + "                  a.CityId, \n"
               + "                  a.name     ServiceName, a.id ServiceID, \n"
               + "                  ( \n"
               + "                      SELECT zonecode \n"
               + "                      FROM   Branches \n"
               + "                      WHERE  branchcode = e.bid \n"
               + "                  )          ZoneCode \n"
               + "           FROM   ServiceArea a \n"
               + "                  INNER JOIN ExpressCenters e \n"
               + "                       ON  e.expressCenterCode = a.Ec_code \n"
               + "                  INNER JOIN Cities c \n"
               + "                       ON  c.id = a.CityId \n"
               + "       )                 A \n"
               + "GROUP BY \n"
               + "       A.bid, \n"
               + "       A.CategoryId, \n"
               + "       A.ClientId, \n"
               + "       A.costCenterCode, \n"
               + "       A.description, \n"
               + "       A.expressCenterCode, \n"
               + "       A.sname, \n"
               + "       A.CityId, \n"
               + "       ServiceName, \n"
               + "       A.ZoneCode, \n"
               + "       A.description, \n"
               + "       A.ServiceID \n"
               + "ORDER BY \n"
               + "       ServiceName";

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
        public DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";
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
        public DataSet ExpressCenterLocal(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {

                string query = "SELECT e.expresscentercode, e.*, (Select zonecode from Branches where branchcode = e.bid) ZoneCode \n" +
                "  FROM ServiceArea a\n" +
                " inner join ExpressCenters e\n" +
                "    on e.expressCenterCode = a.Ec_code\n" +
                " inner join Cities c\n" +
                "    on c.id = a.CityId\n" +
                " where a.CityId = '" + clvar.CityCode + "'\n" +
                " group by e.expresscentercode, e.*, (Select zonecode from Branches where branchcode = e.bid)--e.expressCenterCode, e.description ,bid";


                query = "select A.bid,\n" +
                "       A.CategoryId,\n" +
                "       A.ClientId,\n" +
                "       A.costCenterCode,\n" +
                "       A.description NAME,\n" +
                "       A.expressCenterCode,\n" +
                "       A.sname,\n" +
                "       A.ZoneCode,A.CityId,A.description EName\n" +
                "  FROM (SELECT e.expresscentercode,\n" +
                "               e.bid,\n" +
                "               e.CategoryId,\n" +
                "               e.ClientId,\n" +
                "               e.costCenterCode,\n" +
                "               e.description,\n" +
                "               e.sname,a.CityId,\n" +
                "               (Select zonecode from Branches where branchcode = e.bid) ZoneCode\n" +
                "          FROM ServiceArea a\n" +
                "         inner join ExpressCenters e\n" +
                "            on e.expressCenterCode = a.Ec_code\n" +
                "         inner join Cities c\n" +
                "            on c.id = a.CityId\n) A\n" +
                " group by A.bid,\n" +
                "          A.CategoryId,\n" +
                "          A.ClientId,\n" +
                "          A.costCenterCode,\n" +
                "          A.description,\n" +
                "          A.expressCenterCode,\n" +
                "          A.sname,A.CityId,\n" +
                "          A.ZoneCode,A.description ORDER BY NAME";

                string sql = "SELECT A.bid, \n"
               + "       A.CategoryId, \n"
               + "       A.ClientId, \n"
               + "       A.costCenterCode, \n"
               + "       A.description     NAME, \n"
               + "       A.expressCenterCode, \n"
               + "       A.sname, \n"
               + "       A.ServiceName,ServiceID, \n"
               + "       A.ZoneCode, \n"
               + "       A.CityId, \n"
               + "       A.description     EName \n"
               + "FROM   ( \n"
               + "           SELECT e.expresscentercode, \n"
               + "                  e.bid, \n"
               + "                  e.CategoryId, \n"
               + "                  e.ClientId, \n"
               + "                  e.costCenterCode, \n"
               + "                  e.description, \n"
               + "                  e.sname, \n"
               + "                  a.CityId, \n"
               + "                  a.name     ServiceName, a.id ServiceID, \n"
               + "                  ( \n"
               + "                      SELECT zonecode \n"
               + "                      FROM   Branches \n"
               + "                      WHERE  branchcode = e.bid \n"
               + "                  )          ZoneCode \n"
               + "           FROM   ServiceArea a \n"
               + "                  INNER JOIN ExpressCenters e \n"
               + "                       ON  e.expressCenterCode = a.Ec_code \n"
               + "                  INNER JOIN Cities c \n"
               + "                       ON  c.id = a.CityId \n"
               + "       )                 A \n"
               + "GROUP BY \n"
               + "       A.bid, \n"
               + "       A.CategoryId, \n"
               + "       A.ClientId, \n"
               + "       A.costCenterCode, \n"
               + "       A.description, \n"
               + "       A.expressCenterCode, \n"
               + "       A.sname, \n"
               + "       A.CityId, \n"
               + "       ServiceName, \n"
               + "       A.ZoneCode, \n"
               + "       A.description, \n"
               + "       A.ServiceID \n"
               + "ORDER BY \n"
               + "       ServiceName";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ExpressCenterOrigin(Cl_Variables clvar)
        {
            DataSet Ds_1 = new DataSet();

            try
            {

                string query = "SELECT * FROM ExpressCenters ec where bid='" + clvar.origin + "' and status='1' order by NAME";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ExpressionItems()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "select CODE, code + ' -- ' + name as NAME, i.*  FROM ExpressionProduct i where status = '1'";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ExpressCenterIntl()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM ExpressCenters ec WHERE ec.bid  IN ('17') ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet ExpressCenterType()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT * FROM Mnp_ExpressCenterType ec ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataSet CheckServiceAvailability(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();

            string query = "select * FROM IntlZoneCountry where CountryCode = '" + clvar.destinationCountryCode + "' AND ServiceTypeID = '" + clvar.ServiceTypeName + "' and zonecode not in ('ROW','')";
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception ex)
            { }

            return ds;
        }

        public DataSet GetClientTarrifForInternationalConsignment(Cl_Variables clvar)
        {
            string query = "select * FROM tempClientTariff where ISINTLTARIFf = '1' AND ToZoneCode = '" + clvar.ToZoneCode + "' and BranchCode = '" + clvar.Branch + "' and ServiceID = '" + clvar.ServiceTypeName + "' and chkdeleted = 'False' --and client_id = '" + clvar.CustomerClientID + "'";
            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception ex)
            { }

            return ds;
        }

        public DataSet GetCurrencyConversionRates(Cl_Variables clvar)
        {
            string query = "select * FROM CurrencyConversions where baseCurrencyId = '" + clvar.BaseCurrency + "' and toCurrencyId = '2' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "' between fromDate and toDate";
            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception ex)
            { }

            return ds;
        }

        public DataTable ValidateRiderCode(Cl_Variables clvar)
        {
            string query = "SELECT * FROM RIDERS WHERE BRANCHID = '4' and expressCenterId = '0111' AND STATUS = '1'";
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

        public DataTable ClientTarifAddtionalFactor(Cl_Variables clvar)
        {
            string sql = "SELECT tm.*, convert(nvarchar(10), tm.toWeight) ToWeight_  \n"
                + "  FROM [APL_BTS].[dbo].[tempClientTariff] tm,  Zones z , Zones zz \n"
                + "  \n"
                + "WHERE   \n"
                + " tm.FromZoneCode=z.zoneCode \n"
                + " and tm.ToZoneCode=zz.zoneCode \n"
                + " AND  tm.FromZoneCode = '" + clvar.Zone + "' \n"
                + " AND tm.ServiceID IN ('" + clvar.ServiceType + "')  \n"
                + " AND Client_Id ='" + clvar.AccountNo + "'  \n"
                + " and chkdeleted ='False'";

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

        public DataTable ClientTarifAddtionalFactor_(Cl_Variables clvar)
        {
            string sql = "SELECT tm.*, convert(nvarchar(10), tm.toWeight) ToWeight_  \n"
                + "  FROM [APL_BTS].[dbo].[tempClientTariff] tm,  Zones z , Zones zz \n"
                + "  \n"
                + "WHERE   \n"
                + " tm.FromZoneCode=z.zoneCode \n"
                + " and tm.ToZoneCode=zz.zoneCode \n"
                + " AND  tm.FromZoneCode = '" + clvar.Zone + "' \n"
                //+ " AND tm.ServiceID IN ('" + clvar.ServiceType + "')  \n"
                + " AND Client_Id ='" + clvar.AccountNo + "'  \n"
                + " and chkdeleted ='False'";

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

        public DataTable RNRTarrif(Cl_Variables clvar)
        {
            string sql = "SELECT rt.*  \n"
               + "FROM   RnR_Tarrif        rt \n"
               + "WHERE  rt.Client_ID  = '" + clvar.CustomerClientID + "'";

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

        public DataTable GetCompanies()
        {
            string query = "SELECT ID, COMPANYNAME FROM COMPANY WHERE ISACTIVE = '1'";
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

        public DataTable GetServiceTypeCategories()
        {
            string sql = "select distinct c.serviceTypeCategory from ServiceTypes c";
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

        public DataTable GetAllServiceTypesDetails()
        {
            string sqlString = "SELECT T.ID, T.SERVICETYPENAME,\n" +
            "       C.COMPANYNAME, C.ID COMPANYID,\n" +
            "       CASE\n" +
            "         WHEN T.STATUS = '0' THEN\n" +
            "          'INACTIVE'\n" +
            "         WHEN T.STATUS = '1' THEN\n" +
            "          'ACTIVE'\n" +
            "       END STATUS,\n" +
            "       T.DESCRIPTION,\n" +
            "       CASE\n" +
            "         WHEN T.ISINTL = '0' THEN\n" +
            "          'DOMESTIC'\n" +
            "         WHEN T.STATUS = '1' THEN\n" +
            "          'INTERNATIONAL'\n" +
            "       END TYPE,\n" +
            "       T.SERVICETYPECATEGORY\n" +
            "  FROM SERVICETYPES_NEW T\n" +
            " INNER JOIN COMPANY C\n" +
            "    ON C.ID = T.COMPANYID\n" +
            " WHERE T.STATUS = '1'";
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

        public DataTable GetAllCurrencies()
        {
            string query = "select c.Id, c.name, c.code, c.countryId, CASE WHEN c.STATUS = '1' THEN 'Active' WHEN c.status = '0' THEN 'Inactive' END STATUS, c.symbol  from Currencies c";
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

        public DataTable GetCountriesForCurrencies()
        {
            string query = "SELECT * FROM COUNTRIES";
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

        public DataTable GetTarrifForEdit(Cl_Variables clvar)
        {
            string sqlString = " select z.name ZoneName, t.* From tempClientTariff t \n" +
                "inner join Zones z\n" +
             "on z.zoneCode = t.ToZoneCode\n" +
             "where t.Client_Id = '" + clvar.CustomerClientID + "'\n" +
             " and t.ServiceID = '" + clvar.ServiceTypeName + "' \n" +
             "and isintltariff = '0' \n" +
             "and chkdeleted = '0' \n" +
             //"and t.branchCode = '" + clvar.Branch + "'\n" +
             "and t.ToZoneCode = '" + clvar.ToZoneCode + "'\n" +
             "and t.branchCode = '" + clvar.Branch + "'\n" +
             "order by ZoneName, ToZoneCode, FromWeight";
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
        public DataTable GetInternationalTarrifForEdit(Cl_Variables clvar)
        {
            string sqlString = " select z.name ZoneName, t.* From tempClientTariff t \n" +
                "inner join Zones z\n" +
             "on z.zoneCode = t.ToZoneCode\n" +
             "where t.Client_Id = '" + clvar.CustomerClientID + "' and t.ServiceID = '" + clvar.ServiceTypeName + "' and ToZoneCode = '" + clvar.ToZoneCode + "' and isintltariff = '1' and chkdeleted = '0'\n" +
             "order by ZoneName, ToZoneCode, FromWeight";
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


        public DataTable GetInternationalTarrifForEdit_1(Cl_Variables clvar)
        {
            string sqlString = " select z.name ZoneName, t.* From tempClientTariff t \n" +
                "inner join Zones z\n" +
             "on z.zoneCode = t.ToZoneCode\n" +
             "where t.Client_Id = '" + clvar.CustomerClientID + "' and t.ServiceID = '" + clvar.ServiceTypeName + "' and isintltariff = '1' and chkdeleted = '0'\n" +
             "order by ZoneName, ToZoneCode, FromWeight";
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



        public DataTable GetInternationalTarrifForEdit_2(Cl_Variables clvar)
        {
            string sqlString = " select z.name ZoneName, t.* From tempClientTariff t \n" +
                "inner join Zones z  on z.zoneCode = t.ToZoneCode inner join Creditclients c on  c.id = t.client_id\n" +
             "\n" +
             "where c.accountno = '" + clvar.AccountNo + "' and t.ServiceID = '" + clvar.ServiceTypeName + "' and isintltariff = '1' and chkdeleted = '0'\n and c.branchCode='" + clvar.Branch + "'\n" +
             "order by ZoneName, ToZoneCode, FromWeight";

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

        public DataTable GetAllZones()
        {
            string query = "select zoneCode, name from Zones where status = '1' order by name";
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

        public DataTable GetRnRCategories()
        {
            string query = "select * From RnR_Categories";
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

        public DataTable GetMaxBranchCode()
        {
            string query = "SELECT MAX(CAST(branchCode as int)) + 1 from BRANCHES";
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

        public DataTable GetCitiesByZone(Cl_Variables clvar)
        {

            string query = "select za.zoneCode, * FROM Cities c\n" +
            "inner join ZoneCityAssociation za\n" +
            "on za.cityid = c.id\n" +
            "where za.zoneCode = '" + clvar.Zone + "' and c.isActive='1'\n" +
            "order by 3";

            query = "select * FROM MNP_CITIES c where c.zonecode = '" + clvar.Zone + "' and c.isActive='1' order by CityName";
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

        public DataTable GetZonesForBranches()
        {

            string sqlString = "select za.zoneCode, c.name FROM Zones c\n" +
            "inner join ZoneCityAssociation za\n" +
            "on za.zoneCode = c.zoneCode\n" +
            "\n" +
            "where c.status = '1'\n" +
            "group by za.zoneCode, c.name";

            sqlString = "select DISTINCT c.ZONEcode, z.name\n" +
            "  from MNP_CITIES c\n" +
            " inner join Zones z\n" +
            "    on CAST(c.zonecode as Varchar) = z.zoneCode order by name";

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

        public DataTable GetBranchDetails(Cl_Variables clvar)
        {
            string query = "";
            if (clvar.Zone == "" || clvar.Zone == "0")
            {
                query = "SELECT * FROM BRANCHES";
            }
            else if (clvar.CityCode == "" || clvar.CityCode == "0")
            {
                query = "SELECT * FROM BRANCHES WHERE ZONECODE = '" + clvar.Zone + "'";
            }
            else
            {
                query = "SELECT * FROM BRANCHES WHERE ZONECODE = '" + clvar.Zone + "' and cityID = '" + clvar.CityCode + "'";
            }

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

        public DataTable GetBranchGst(Cl_Variables clvar)
        {

            string sqlString = "select CompanyID, branchCode, effectiveFrom, effectiveTo, gst\n" +
            "  FROM BranchGST b\n" +
            " where b.companyId = '" + clvar.Company + "'\n" +
            "   and b.branchCode = '" + clvar.Branch + "'\n" +
            "   and b.status = '1' order by gst";
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

        public DataTable ServiceTypeNameRvdbo()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                //string query = "SELECT st.name          ServiceTypeName, st.ServiceTypeName serviceTypeID  \n"
                //+ "FROM   ServiceTypes     st \n"
                //+ "WHERE  st.isinternational = '0' \n"
                //+ "       AND st.isactive = '1' \n"
                //+ "GROUP BY \n"
                //+ "       st.name , st.serviceTypeID\n"
                //+ "ORDER BY \n"
                //+ "       st.name";

                string sql = " \n"
               + "SELECT st.name                ServiceTypeName, \n"
               + "       st.ServiceTypeName     serviceTypeID \n"
               + "FROM   ServiceTypes           st \n"
               + "WHERE  st.IsIntl = '0' \n"
               + "       AND st.[status] = '1' \n"
               + "GROUP BY \n"
               + "       st.name, \n"
               + "     \n"
               + "       st.name, \n"
               + "       st.ServiceTypeName \n"
               + "ORDER BY \n"
               + "    1";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return Ds_1.Tables[0];
        }

        public DataTable GetArrivalPrintHead(Variable clvar)
        {

            string sqlString = "select a.RiderCode,\n" +
            "       a.OriginExpressCenterCode,\n" +
            "       r.firstName + ' ' + r.lastName RiderName,\n" +
            "       COUNT(ad.consignmentNumber) ConsignmentCount\n" +
            "  from ArrivalScan a\n" +
            "\n" +
            " inner join ArrivalScan_Detail ad\n" +
            "    on ad.ArrivalID = a.Id\n" +
            " left outer join Riders r\n" +
            "    on a.OriginExpressCenterCode = r.expressCenterId\n" +
            "   and a.RiderCode = r.riderCode\n" +
            " where a.Id = '" + clvar.ArrivalID + "'\n" +
            "\n" +
            " group by a.RiderCode, a.OriginExpressCenterCode, r.firstName, r.lastName";
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

        public DataTable GetArrivalPrintDetail(Variable clvar)
        {

            string sqlString = "select ad.consignmentNumber, c.serviceTypeName, c.weight\n" +
            "  from ArrivalScan a\n" +
            " inner join ArrivalScan_Detail ad\n" +
            "    on a.Id = ad.ArrivalID\n" +
            " inner join Consignment_ops c\n" +
            "    on c.consignmentNumber = ad.consignmentNumber\n" +
            "\n" +
            " where ad.ArrivalID = '" + clvar.ArrivalID + "'";
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

        public DataTable GetBagPrintReportHeader(Cl_Variables clvar)
        {

            string sqlString = "select b.bagNumber,\n" +
            "       b.totalWeight,\n" +
            "       b1.name       Origin,\n" +
            "       b2.name       Destination,\n" +
            "       b3.name       Branch,\n" +
            "       b.totalWeight,\n" +
            "       b.date,\n" +
            "       b.sealNo , b.createdby\n" +
            "\n" +
            "  from Bag b\n" +
            " inner join Branches b1\n" +
            "    on b.origin = b1.branchCode\n" +
            "\n" +
            " inner join Branches b2\n" +
            "    on b.destination = b2.branchCode\n" +
            "\n" +
            " inner join Branches b3\n" +
            "    on b.branchCode = b3.branchCode\n" +
            "\n" +
            " where b.bagNumber = '" + clvar.BagNumber + "'";

            sqlString = "select b.bagNumber,\n" +
           "       b.totalWeight,\n" +
           "       b1.name       Origin,\n" +
           "       b2.name       Destination,\n" +
           "       b3.name       Branch,\n" +
           "       b.totalWeight,\n" +
           "       b.date,\n" +
           "       b.sealNo ,  z.Name createdby\n" +
           "\n" +
           "  from Bag b\n" +
           " inner join Branches b1\n" +
           "    on b.origin = b1.branchCode\n" +
           "\n" +
           " inner join Branches b2\n" +
           "    on b.destination = b2.branchCode\n" +
           "\n" +
           " inner join Branches b3\n" +
           "    on b.branchCode = b3.branchCode\n" +
           "\n" +
           " left outer join ZNI_USER1 z\n" +
           " on b.createdBy = CAST(z.U_ID as varchar)\n" +
           "\n" +
           " where b.bagNumber = '" + clvar.BagNumber + "'";


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

            string sqlString = "select b.manifestNumber,\n" +
            "       m.manifestType,\n" +
            "       b1.name          Origin,\n" +
            "       b2.name          Destination\n" +
            "\n" +
            "  from BagManifest b\n" +
            " inner join mnp_Manifest m\n" +
            "    on b.manifestNumber = m.manifestNumber\n" +
            "\n" +
            " inner join Branches b1\n" +
            "    on m.origin = b1.branchCode\n" +
            "\n" +
            " inner join Branches b2\n" +
            "    on m.destination = b2.branchCode\n" +
            "\n" +
            " where b.bagNumber = '" + clvar.BagNumber + "'";
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

            string sqlString = "select c.consignmentNumber, c.pieces, c.consigner, c.consignee, c.weight\n" +
            "  from BagOutpieceAssociation bp\n" +
            " inner join Consignment c\n" +
            "    on c.consignmentNumber = bp.outpieceNumber\n" +
            " where bp.bagNumber = '" + clvar.BagNumber + "'";
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

        public DataTable RunsheetTypes()
        {
            string query = "select l.Id, l.Code from Lookup l where l.DropDownName = 'RUNSHEET_TYPE' ";

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

        public DataTable Routes()
        {
            string query = "select r.routeCode,  r.name +  ' ( '+ r.routeCode + ' )' Route from Routes r where r.cityId = ( \n" +
                           "select cityid from Branches b where b.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "') order by 1";
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

        public DataTable RidersByRoutes(Cl_Variables clvar)
        {
            string query = "select riderCode, FirstName + ' ' + LastName RiderName from Riders r where r.routeCode = '" + clvar.routeCode + "' and r.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and r.status = '1'";
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

        public DataTable GetMaxRunsheetNumber()
        {
            string query = "select ISNULL(MAX(CAST(r.runsheetNumber as bigint)),(select year from SystemCodes where status = '1' and codeType = 'RUNSHEET_NO')+ '00000000') from Runsheet r where r.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
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

        public DataTable GetPODStatus()
        {
            string query = "select * FROM rvdbo.Lookup l where l.AttributeGroup = 'POD_STATUS' and Active ='1'";
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

        public DataTable GetPODReasons()
        {
            string query = "select * from rvdbo.Lookup l where l.AttributeGroup = 'POD_REASON'";
            query = "select L2.Id ID, L2.AttributeValue, L1.Id ID1 from rvdbo.Lookup L1\n" +
                "inner join rvdbo.Lookup L2\n" +
                "on L1.AttributeValue = L2.AttributeDesc\n" +
                "where L1.AttributeGroup = 'POD_STATUS'\n" +
                "and L2.AttributeGroup = 'POD_REASON' and L1.active = '1' and L2.active = '1'\n";
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

        public DataTable GetConsignmentForApproval(Cl_Variables clvar)
        {

            string sqlString = "select c.bookingDate,\n" +
            "       c.consignmentNumber,\n" +
            "       c.customerType,\n" +
            "       c.creditClientId,\n" +
            "       c.orgin,\n" +
            "       c.serviceTypeName,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       c.destination,\n" +
            "       c.weight,\n" +
            "       c.riderCode,\n" +
            "       c.originExpressCenter,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.chargedAmount,\n" +
            "       c.isApproved,\n" +
            "       ic.invoiceNumber,\n" +
            "       i.startDate ReportingDate,\n" +
            "       i.deliveryStatus,\n" +
            "       cm.priceModifierId,\n" +
            "       p.name,\n" +
            "       cm.calculatedValue,\n" +
            "       cm.calculationBase,\n" +
            "       cm.isTaxable,\n" +
            "       cm.SortOrder,\n" +
            "       p.description, c.destinationExpressCenterCode\n" +
            "\n" +
            "  from consignment c\n" +
            " inner join InvoiceConsignment ic\n" +
            "    on c.consignmentNumber = ic.consignmentNumber\n" +
            " inner join Invoice i\n" +
            "    on i.invoiceNumber = ic.invoiceNumber\n" +
            "  left outer join ConsignmentModifier cm\n" +
            "    on c.consignmentNumber = cm.consignmentNumber\n" +
            "  left outer join PriceModifiers p\n" +
            "    on cm.priceModifierId = p.id\n" +
            " where c.orgin = '4'\n" +
            "   and c.consignmentTypeId <> '10'\n" +
            "   and cm.priceModifierId is not null\n" +
            "   and IsNull( i.IsInvoiceCanceled , 0 ) ='0'\n" +
            "   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            " order by consignmentNumber, SortOrder";

            // sqlString = "select c.bookingDate,\n" +
            //"       c.consignmentNumber,\n" +
            //"       c.customerType,\n" +
            //"       c.creditClientId,\n" +
            //"       c.orgin,\n" +
            //"       c.serviceTypeName,\n" +
            //"       c.consigner,\n" +
            //"       c.consignee,\n" +
            //"       c.destination,\n" +
            //"       c.weight,\n" +
            //"       c.riderCode,\n" +
            //"       c.originExpressCenter,\n" +
            //"       c.consignmentTypeId,\n" +
            //"       c.chargedAmount,\n" +
            //"       CAST(c.isApproved as varchar) isApproved,\n" +
            //"       c.consignerAccountNo accountNo,\n" +
            //"       ic.invoiceNumber,\n" +
            //"       i.startDate ReportingDate,\n" +
            //"       i.deliveryStatus,\n" +
            //"       cm.priceModifierId,\n" +
            //"       p.name priceModifierName,\n" +
            //"       cm.calculatedValue,\n" +
            //"       cm.calculationBase,\n" +
            //"       cm.isTaxable,\n" +
            //"       cm.SortOrder,\n" +
            //"       p.description, c.destinationExpressCenterCode, c.accountReceivingDate, c.bookingDate, c.COD\n" +
            //"\n" +
            //"  from consignment c\n" +
            //" left outer join InvoiceConsignment ic\n" +
            //"    on c.consignmentNumber = ic.consignmentNumber\n" +
            //" left outer join Invoice i\n" +
            //"    on i.invoiceNumber = ic.invoiceNumber\n" +
            //"  left outer join ConsignmentModifier cm\n" +
            //"    on c.consignmentNumber = cm.consignmentNumber\n" +
            //"  left outer join PriceModifiers p\n" +
            //"    on cm.priceModifierId = p.id\n" +
            //" inner join CreditClients cc\n" +
            //"    on c.creditClientId = cc.id\n" +
            //" where /*c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            //"   and c.consignmentTypeId <> '10'\n" +
            //"   and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' and ISNULL(i.IsInvoiceCanceled,0) = '0' \n" +
            //" order by consignmentNumber, SortOrder";

            string sql = "SELECT *,  CASE  \n"
               + "WHEN i.IsInvoiceCanceled = '1' THEN ''\n"
               + " ELSE i.invoiceNumber      \n"
               + "END invoiceNumber_ FROM ( \n"
               + "SELECT --c.bookingDate, \n"
               + "       c.consignmentNumber, \n"
               + "       c.customerType, \n"
               + "       c.creditClientId, \n"
               + "       c.orgin, \n"
               + "       c.serviceTypeName, \n"
               + "       c.consigner, \n"
               + "       c.consignee, \n"
               + "       c.destination, \n"
               + "       c.weight, \n"
               + "       c.riderCode, \n"
               + "       c.originExpressCenter, \n"
               + "       c.consignmentTypeId, \n"
               + "       c.chargedAmount, \n"
               + "       CAST(c.isApproved AS VARCHAR)     isApproved, \n"
               + "       c.consignerAccountNo              accountNo, \n"
               + "       --ic.invoiceNumber, \n"
               + "   --    i.startDate                       ReportingDate, \n"
               + "   --    i.deliveryStatus, \n"
               + "       cm.priceModifierId, \n"
               + "       p.name                            priceModifierName, \n"
               + "       cm.calculatedValue, \n"
               + "       cm.calculationBase, \n"
               + "       cm.isTaxable, \n"
               + "       cm.SortOrder, \n"
               + "       p.description, \n"
               + "       c.destinationExpressCenterCode, \n"
               + "       c.accountReceivingDate, \n"
               + "       c.bookingDate, \n"
               + "       c.COD, \n"
               + "       ic.invoiceNumber \n"
               + "    --   i.IsInvoiceCanceled \n"
               + "FROM   consignment c \n"
               + "       LEFT OUTER JOIN InvoiceConsignment ic \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       --LEFT OUTER JOIN Invoice i \n"
               + "       --     ON  i.invoiceNumber = ic.invoiceNumber \n"
               + "       LEFT OUTER JOIN ConsignmentModifier cm \n"
               + "            ON  c.consignmentNumber = cm.consignmentNumber \n"
               + "       LEFT OUTER JOIN PriceModifiers p \n"
               + "            ON  cm.priceModifierId = p.id \n"
               + "       INNER JOIN CreditClients cc \n"
               + "            ON  c.creditClientId = cc.id \n"
               + "WHERE  /*c.orgin = '4' \n"
               + "       and c.consignmentTypeId <> '10' \n"
               + "       and */c.consignmentNumber = '" + clvar.consignmentNo.Trim() + "' \n"
               + "  --     AND ISNULL(i.IsInvoiceCanceled, 0) = '0' \n"
               + ") A \n"
               + "LEFT OUTER JOIN  \n"
               + "Invoice i  \n"
               + "ON A.invoiceNumber = i.invoiceNumber \n"
               + "  ORDER BY i.createdOn desc";

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

        public DataTable GetCODConsignmentForApproval(Cl_Variables clvar)
        {

            string sqlString = "select c.bookingDate,\n" +
            "       c.consignmentNumber,\n" +
            "       c.customerType,\n" +
            "       c.creditClientId,\n" +
            "       c.orgin,\n" +
            "       c.serviceTypeName,\n" +
            "       c.consigner,\n" +
            "       c.consignee,\n" +
            "       c.destination,\n" +
            "       c.weight,\n" +
            "       c.riderCode,\n" +
            "       c.originExpressCenter,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.chargedAmount,\n" +
            "       c.isApproved,\n" +
            "       ic.invoiceNumber,\n" +
            "       i.startDate ReportingDate,\n" +
            "       i.deliveryStatus,\n" +
            "       cm.priceModifierId,\n" +
            "       p.name,\n" +
            "       cm.calculatedValue,\n" +
            "       cm.calculationBase,\n" +
            "       cm.isTaxable,\n" +
            "       cm.SortOrder,\n" +
            "       p.description, c.destinationExpressCenterCode\n" +
            "\n" +
            "  from consignment c\n" +
            " inner join InvoiceConsignment ic\n" +
            "    on c.consignmentNumber = ic.consignmentNumber\n" +
            " inner join Invoice i\n" +
            "    on i.invoiceNumber = ic.invoiceNumber\n" +
            "  left outer join ConsignmentModifier cm\n" +
            "    on c.consignmentNumber = cm.consignmentNumber\n" +
            "  left outer join PriceModifiers p\n" +
            "    on cm.priceModifierId = p.id\n" +
            " where c.orgin = '4'\n" +
            "   and c.consignmentTypeId <> '10'\n" +
            "   and cm.priceModifierId is not null\n" +
            "   and c.consignmentNumber = '" + clvar.consignmentNo + "'\n" +
            " order by consignmentNumber, SortOrder";

            sqlString = "SELECT * FROM CODConsignmentDetail cd WHERE cd.consignmentNumber='" + clvar.consignmentNo + "'";

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

        public DataTable GetClientTariffForConsignmentApproval(Cl_Variables clvar)
        {

            string query = "select * \n" +
                           "  from tempClientTariff t \n" +
                           " where t.Client_Id = '" + clvar.CustomerClientID + "' \n" +
                           "   and t.ServiceID = '" + clvar.ServiceTypeName + "' and branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n";
            string alternateQuery = "select * \n" +
                           "  from tempClientTariff t \n" +
                           " where t.Client_Id = '0' \n" +
                           "   and t.ServiceID = '" + clvar.ServiceTypeName + "' and branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' \n";

            //query += "   and t.FromZoneCode = (select b.zonecode from Branches b where b.branchCode = '" + clvar.FromZoneCode + "') \n";
            //if (clvar.ToZoneCode != "17")
            //{
            //    query +=
            //               "   and t.ToZoneCode = (select b1.zonecode from Branches b1 where b1.branchCode = '" + clvar.ToZoneCode + "') \n" +
            //               "   and t.ServiceID = '" + clvar.ServiceTypeName + "' \n";
            //}
            //else
            //{
            //    query +=
            //               "   and t.ToZoneCode = '17' \n" +
            //               "   and t.ServiceID = '" + clvar.ServiceTypeName + "' \n";
            //}


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.StrconLive());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    sda = new SqlDataAdapter(alternateQuery, con);
                    sda.Fill(dt);
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable GetZoneByBranch(Cl_Variables clvar)
        {
            string query = "select b.BranchCode, b.zoneCode, z.colorId from Branches b inner join Zones z on z.zoneCode = b.zoneCode where b.branchCode in ('" + clvar.Branch + "','" + clvar.BranchName + "')";
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

        public DataTable RNRTarrifDefault(Cl_Variables clvar)
        {
            string sql = "SELECT rt.*  \n"
               + "FROM   RnR_Tarrif        rt \n"
               + "WHERE  rt.IsDefault = '1'";

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

        public DataTable GetAccountDetailByAccountNumber(Cl_Variables clvar)
        {

            string query = "SELECT  z.name ZoneName, c.* FROM CREDITCLIENTS c inner join Zones z on z.zoneCode = c.zoneCode where c.ACCOUNTNO = '" + clvar.AccountNo + "' and c.branchcode = '" + clvar.Branch + "'";
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

        public DataTable GetAccountDetailByAccountNumber_(Cl_Variables clvar)
        {

            string query = "SELECT   * FROM  CreditClients cc \n"
               + "   \n"
               + " Left OUTER JOIN ClientPriceModifierAssociation Cpa \n"
               + " ON cpa.creditClientId = cc.id \n"
               + " where cc.ACCOUNTNO = '" + clvar.AccountNo + "' and cc.branchcode = '" + clvar.Branch + "'";

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

        public DataTable GetRnRTariffForEdit(Cl_Variables clvar)
        {

            string query = "SELECT T.ID,\n" +
            "       T.CLIENT_ID,\n" +
            "       T.FROMCATID,\n" +
            "       C.LABEL FROMCAT,\n" +
            "       T.TOCATID,\n" +
            "       D.LABEL TOCAT,\n" +
            "       T.ISDEFAULT,\n" +
            "       T.VALUE\n" +
            "  FROM RNR_TARRIF T\n" +
            " INNER JOIN RNR_CATEGORIES C\n" +
            "    ON T.FROMCATID = C.ID\n" +
            " INNER JOIN RNR_CATEGORIES D\n" +
            "    ON T.TOCATID = D.ID\n" +
            " WHERE T.CLIENT_ID = '" + clvar.CustomerClientID + "' ORDER BY FROMCAT, TOCAT, VALUE";

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

        public DataTable GetZonesForDomesticTariff()
        {
            //string query = "select zoneCode, name from Zones where status = '1' order by name";

            string query = "select z.zoneCode, z.name\n" +
            "  from Zones z\n" +
            " inner join Branches b\n" +
            "    on b.zoneCode = z.zoneCode\n" +
            " where z.status = '1'\n" +
            "   and b.branchCode <> '17'\n" +
            " group by z.colorId,\n" +
            "          z.createdBy,\n" +
            "          z.createdOn,\n" +
            "          z.description,\n" +
            "          z.email,\n" +
            "          z.faxNo,\n" +
            "          z.hasStore,\n" +
            "          z.modifiedBy,\n" +
            "          z.modifiedOn,\n" +
            "          z.name,\n" +
            "          z.phoneNo,\n" +
            "          z.status,\n" +
            "          z.type,\n" +
            "          z.zoneCode\n" +
            "union all \n" +
            " select z.zoneCode, z.name from Zones z where z.zoneCode in ('14','16','17','DIFF','LOCAL','SAME')\n" +
            "order by zoneCode";


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

        public DataTable GetConsignmentsInInvoice(Cl_Variables clvar)
        {

            string sqlString = "selecT c.bookingDate,\n" +
            "       ic.consignmentNumber,\n" +
            "       c.pieces,\n" +
            "       c.serviceTypeName,\n" +
            "       b.name destination,\n" +
            "       c.weight,\n" +
            "       ic.consignmentAmount totalAmount\n" +
            "  from InvoiceConsignment ic\n" +
            " inner join Consignment c\n" +
            "    on c.consignmentNumber = ic.consignmentNumber\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = c.destination\n" +
            " where ic.invoiceNumber = '" + clvar.RefNumber + "'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.SelectCommand.CommandTimeout = 120;
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;

        }

        public DataTable GetConsignmentsInInvoice_(Cl_Variables clvar)
        {

            //string sqlString = "selecT  ROW_NUMBER() Over (Order by ic.consignmentNumber) As [Sno], cast(c.bookingDate as date) BookingDate,\n" +
            //"       ic.consignmentNumber,\n" +
            //"       c.pieces,\n" +
            //"       c.serviceTypeName,c.orgin,c.destination destination_,c.consignmentTypeId,\n" +
            //"       b.name destination,\n" +
            //"       c.weight,\n" +
            //"       c.totalAmount totalAmount, c.gst,stn.IsIntl\n" +
            //"  from InvoiceConsignment ic\n" +
            //" inner join Consignment c\n" +
            //"    on c.consignmentNumber = ic.consignmentNumber\n" +
            //" inner join Branches b\n" +
            //"    on b.branchCode = c.destination\n" +
            //" INNER JOIN ServiceTypes_New stn      \n" +
            //" ON c.serviceTypeName = stn.serviceTypeName    \n" +
            //" where ic.invoiceNumber = '" + clvar.RefNumber + "'";

            string sql = ""
               + "SELECT  \n"
               + "       CAST(c.bookingDate AS date)     BookingDate, \n"
               + "       ic.invoiceNumber, \n"
               + "       ic.consignmentNumber, \n"
               + "       c.consignerAccountNo Accountno, \n"
               + "       c.orgin, \n"
               + "       c.destination                   destination, \n"
               + "       c.serviceTypeName, \n"
               + "       c.consignmentTypeId, \n"
               + "       c.weight, \n"
               + "       c.totalAmount                   totalAmount, \n"
               + "       c.gst, \n"
               + "       stn.IsIntl, calculatedValue CM_value,calculatedGST CM_gst, priceModifierId \n"
               + "FROM   InvoiceConsignment ic \n"
               + "       INNER JOIN Consignment c \n"
               + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
               + "       INNER JOIN Branches b \n"
               + "            ON  b.branchCode = c.destination \n"
               + "       INNER JOIN ServiceTypes_New stn \n"
               + "            ON  c.serviceTypeName = stn.serviceTypeName "
               + "       inner join Invoice I "
               + "            ON  i.InvoiceNumber = ic.InvoiceNumber"
               + "       LEFT OUTER JOIN ConsignmentModifier CM ON "
               + "            ON CM.consignmentNumber = C.consignmentNumber "
               + " where i.InvoiceNumber ='" + clvar.InvoiceNo + "' and ic.consignmentNumber ='" + clvar.consignmentNo + "'  and isnull(i.IsInvoiceCanceled,'0') ='0'";

            sql = ""
              + "SELECT  \n"
              + "       CAST(c.bookingDate AS date)     BookingDate, \n"
              + "       ic.invoiceNumber, \n"
              + "       ic.consignmentNumber, \n"
              + "       c.consignerAccountNo Accountno, \n"
              + "       c.orgin, \n"
              + "       c.destination                   destination, \n"
              + "       c.serviceTypeName, \n"
              + "       c.consignmentTypeId, \n"
              + "       c.weight, \n"
              + "       c.totalAmount                   totalAmount, \n"
              + "       c.gst, \n"
              + "       stn.IsIntl, isnull(round(calculatedValue,2),'0') CM_value,isnull(round(calculatedGST,2),0) CM_gst, priceModifierId \n"
              + " "
              + "FROM   InvoiceConsignment ic \n"
              + "       INNER JOIN Consignment c \n"
              + "            ON  c.consignmentNumber = ic.consignmentNumber \n"
              + "       INNER JOIN Branches b \n"
              + "            ON  b.branchCode = c.destination \n"
              + "       INNER JOIN ServiceTypes_New stn \n"
              + "            ON  c.serviceTypeName = stn.serviceTypeName "
              + "       LEFT OUTER JOIN ConsignmentModifier CM "
              + "            ON CM.consignmentNumber = C.consignmentNumber "
              + " where  c.consignmentNumber ='" + clvar.consignmentNo + "' and c.orgin='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "'";



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

        public DataTable GetInvoiceHeader(Cl_Variables clvar)
        {

            string sqlString = "selecT i.companyId,cc.branchCode, cc.accountNo, cc.name, i.startDate, i.endDate, i.invoiceDate,isnull(cpa.calculationBase,0) calculationBase, isnull(cpa.modifiedCalculationValue,0) modifiedCalculationValue, isnull(cc.DIscountOnDomestic,0) DIscountOnDomestic,isnull(cc.discountOnSample,0) discountOnSample, isnull(cc.discountOnDocument,0) discountOnDocument  \n"
               + "  from Invoice i \n"
               + " inner join CreditClients cc \n"
               + "    on cc.id = i.clientId \n"
               + " Left OUTER JOIN ClientPriceModifierAssociation Cpa \n"
               + " ON cpa.creditClientId = cc.id"
               + " where i.invoiceNumber = '" + clvar.RefNumber + "'";

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

        public DataTable Get_CustomerHeader(Cl_Variables clvar)
        {

            string sqlString = "selecT cc.branchCode, cc.accountNo, cc.name, isnull(cpa.calculationBase,0) calculationBase, isnull(cpa.modifiedCalculationValue,0) modifiedCalculationValue, isnull(cc.DIscountOnDomestic,0) DIscountOnDomestic,isnull(cc.discountOnSample,0) discountOnSample, isnull(cc.discountOnDocument,0) discountOnDocument  \n"
               + "  From CreditClients cc \n"
               + " Left OUTER JOIN ClientPriceModifierAssociation Cpa \n"
               + " ON cpa.creditClientId = cc.id"
               + " where cc.accountNo = '" + clvar.AccountNo + "' and cc.branchCode ='" + HttpContext.Current.Session["BRANCHCODE"].ToString() + "'";

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

        public DataTable GetRidersByExpressCenter(Cl_Variables clvar)
        {
            string sqlString = "select  r.firstName+' '+r.lastName NAME, r.riderCode from Riders r where r.expressCenterId = '" + clvar.expresscenter + "' and r.branchId = '" + clvar.Branch + "' and r.status = '1'  order by 1";
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

        public DataTable GetRelations()
        {
            string query = "select * from ReceiverRelationShip r where r.status = '1'";
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

        public void InsertErrorLog(string ConsignmentNumber, string ArrivalScan, string ManifestNumber, string BagNumber, string LoadingNumber, string RunsheetNumber, string ProcessName, string ErrorMessage)
        {
            string columns = " ZoneCode, BranchCode, ExpressCenterCode, \n" +
                             " ConsignmentNumber, ArrivalScan, ManifestNumber, \n" +
                             " BagNumber, LoadingNumber, RunsheetNumber, \n" +
                             " ProcessName, ErrorMessage, \n" +
                             " CreatedOn, CreatedBy\n";
            string values = "'" + HttpContext.Current.Session["ZoneCode"].ToString() + "', '" + HttpContext.Current.Session["BranchCode"].ToString() + "', '" + HttpContext.Current.Session["ExpressCenter"].ToString() + "',\n" +
                            "'" + ConsignmentNumber + "', '" + ArrivalScan + "', '" + ManifestNumber + "',\n" +
                            "'" + BagNumber + "', '" + LoadingNumber + "', '" + RunsheetNumber + "', \n" +
                            "'" + ProcessName + "', '" + ErrorMessage.Replace("'", "''") + "',\n" +
                            "GETDATE(),'" + HttpContext.Current.Session["U_ID"].ToString() + "'";


            string query = "Insert into MnP_ErrorLog (" + columns + ") VALUES (" + values + ")";

            SqlConnection con = new SqlConnection(clvar.Strcon());


            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            { }
            con.Close();
        }

        public DataTable InternationalServiceTypes()
        {
            string query = "select name, serviceTypeName FROM ServiceTypes where status = '1' and companyId = '1' and IsIntl = '1'";
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

        public DataTable GetReceiptVoucherHeader(Cl_Variables clvar)
        {
            string query = "select p.Id,\n" +
            "       case\n" +
            "         when p.CashPaymentSource is null then\n" +
            "          'CREDIT'\n" +
            "         else\n" +
            "          'CASH'\n" +
            "       End PaymentMode,\n" +
            "       p.ExpressCenterCode,\n" +
            "       ec.name,\n" +
            "       p.RiderCode,\n" +
            "       r.firstName + ' ' + r.lastName RiderName,\n" +
            "       p.PaymentSourceId,\n" +
            "       Case\n" +
            "         When p.ChequeNo is null then\n" +
            "          'CASH'\n" +
            "         else\n" +
            "          ps.Name\n" +
            "       End PaySource,\n" +
            "       Case\n" +
            "         When p.IsCentralized = '1' then\n" +
            "          cg.name\n" +
            "         else\n" +
            "          cc.name\n" +
            "       End Client_Name,\n" +
            "       p.BankId,\n" +
            "       b.Name BankName,\n" +
            "       p.ChequeNo,\n" +
            "       p.ChequeDate,\n" +
            "       p.Amount,\n" +
            "       p.AmountUsed\n" +
            "  from PaymentVouchers p\n" +
            "  left outer join PaymentSource ps\n" +
            "    on ps.Id = p.PaymentSourceId\n" +
            "  left outer join CreditClients cc\n" +
            "    on p.CreditClientId = cc.id\n" +
            "  left outer join ClientGroups cg\n" +
            "    on p.ClientGroupId = cg.id\n" +
            "  left outer join Banks b\n" +
            "    on p.BankId = b.Id\n" +
            "  left outer join ExpressCenters ec\n" +
            "    on p.ExpressCenterCode = ec.expressCenterCode\n" +
            "  left outer join Riders r\n" +
            "    on r.riderCode = p.RiderCode\n" +
            "   and p.BranchCode = r.branchId\n" +
            "   and p.ExpressCenterCode = r.expressCenterId\n" +
            " where p.BranchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "   and p.Id = '" + clvar.VoucherNo + "'";

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

        public DataTable GetInternationalZones()
        {
            string query = "select * from Zones z where z.description = 'intl zone' and z.status = '1'";
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

        public DataTable GetCurrencies()
        {
            string query = "select * from Currencies";
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

        public string UnapproveConsignment(Cl_Variables clvar)
        {
            string query = "update consignment Set isApproved = '0' where consignmentnumber = '" + clvar.consignmentNo + "'";
            string query2 = "insert into MNP_ConsignmentUnapproval (ConsignmentNumber, USERID, TransactionTime, STATUS) VALUES (\n" +
                "'" + clvar.consignmentNo + "', '" + HttpContext.Current.Session["U_ID"].ToString() + "',GETDATE(), '0')";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query2, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //Consignemnts con = new Consignemnts();
                con.Close();
                InsertErrorLog(clvar.consignmentNo, "", "", "", "", "", "UNAPPROVE CONSIGNMENT", ex.Message);
                return ex.Message;
            }
            finally { con.Close(); }

            return "OK";
        }

        public DataTable CheckForCodSequence(string cn)
        {
            DataTable dt = new DataTable();
            string query = "select * from CODUserCNSequence where CAST('" + cn + "' as bigint) between SequenceStart AND SequenceEnd";
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

        public DataTable GetReceivingStatus()
        {
            string query = "select * from rvdbo.Lookup l where l.AttributeGroup = 'Receiving_status'";
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

        public DataTable GetConsignmentBookingDetailReport(Cl_Variables clvar)
        {

            string sqlString = "SELECT ROW_NUMBER()OVER(order by A." + clvar.CheckCondition + ") SNO , A.*,\n" +
            "       CASE\n" +
            "         WHEN i.IsInvoiceCanceled = '1' THEN\n" +
            "          ''\n" +
            "         ELSE\n" +
            "          i.invoiceNumber\n" +
            "       END invoiceNumber_\n" +
            "  FROM (SELECT c.consignmentNumber,\n" +
            "               c.creditClientId,\n" +
            "               b.name Origin,\n" +
            "               c.serviceTypeName,\n" +
            "               c.consigner,\n" +
            "               cc.name consignee,\n" +
            "               b2.sname Destination,\n" +
            "               round(c.weight,2) weight,\n" +
            "               c.riderCode,\n" +
            "               ec.name OriginExpressCenter,\n" +
            "               ct.name ConsignmentType,\n" +
            "               c.chargedAmount,\n" +
            "               CASE WHEN CAST(c.isApproved AS VARCHAR) = '1' then 'YES' ELSE 'NO' END Approved,\n" +
            "               c.consignerAccountNo accountNo,\n" +
            "               cm.priceModifierId,\n" +
            "               p.name priceModifierName,\n" +
            "               cm.calculatedValue,\n" +
            "               cm.calculationBase,\n" +
            "               cm.isTaxable,\n" +
            "               cm.SortOrder,\n" +
            "               p.description,\n" +
            "               ec2.name DestinationExpressCenter,\n" +
            "               c.accountReceivingDate,\n" +
            "               cast(c.bookingDate as date) bookingDate,\n" +
            "               c.COD,\n" +
            "               ic.invoiceNumber,\n" +
            "               z.name CreatedBy, c.CreatedOn, c.pieces\n" +
            "\n" +
            "          FROM consignment c\n" +
            "          LEFT OUTER JOIN InvoiceConsignment ic\n" +
            "            ON c.consignmentNumber = ic.consignmentNumber\n" +
            "          LEFT OUTER JOIN ConsignmentModifier cm\n" +
            "            ON c.consignmentNumber = cm.consignmentNumber\n" +
            "          LEFT OUTER JOIN PriceModifiers p\n" +
            "            ON cm.priceModifierId = p.id\n" +
            "         INNER JOIN CreditClients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "         inner join Branches b\n" +
            "            on b.branchCode = c.orgin\n" +
            "         inner join Branches b2\n" +
            "            on b2.branchCode = c.destination\n" +
            "         inner join ExpressCenters ec\n" +
            "            on ec.expressCenterCode = c.originExpressCenter\n" +
            "         inner join ExpressCenters ec2\n" +
            "            on ec2.expressCenterCode = c.destinationExpressCenterCode\n" +
            "         inner join ConsignmentType ct\n" +
            "            on CAST(c.consignmentTypeId as varchar) = CAST(ct.id as varchar)\n" +
            "          left outer join ZNI_USER1 z\n" +
            "            on CAST(z.U_ID as varchar) = c.createdBy\n" +
            "         WHERE CONVERT(date, c.bookingDate, 105) >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND CONVERT(date, c.bookingDate, 105) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "           and c.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' " + clvar.AccountNo + "\n" +
            "\n" +
            "        ) A\n" +
            "  LEFT OUTER JOIN Invoice i\n" +
            "    ON A.invoiceNumber = i.invoiceNumber\n" +
            " ORDER BY A." + clvar.CheckCondition + "";



            sqlString = "SELECT ROW_NUMBER()OVER(order by " + clvar.CheckCondition + ") SNO , A.*\n" +
            "  FROM (SELECT c.consignmentNumber,\n" +
            "               c.creditClientId,\n" +
            "               b.name Origin,\n" +
            "               c.serviceTypeName,\n" +
            "               c.consigner,\n" +
            "               cc.name consignee,\n" +
            "               b2.sname Destination,\n" +
            "               c.weight,\n" +
            "              CAST(c.riderCode as int) riderCode,\n" +
            "               ec.name OriginExpressCenter,\n" +
            "               ct.name ConsignmentType,\n" +
            "               c.chargedAmount,\n" +
            "               CASE WHEN CAST(c.isApproved AS VARCHAR) = '1' then 'YES' ELSE 'NO' END Approved,\n" +
            "               c.consignerAccountNo,\n" +

            "               ec2.name DestinationExpressCenter,\n" +
            "               c.accountReceivingDate,\n" +
            "               Convert(varchar, c.bookingDate, 106) bookingdate,\n" +
            "               c.COD,\n" +
           "               z.name CreatedBy, c.CreatedOn, r.firstName + ' ' + r.lastName RiderName, c.totalAmount, c.gst, c.pieces\n" +
            "\n" +
            "          FROM consignment c\n" +

            "         INNER JOIN CreditClients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "         inner join Branches b\n" +
            "            on b.branchCode = c.orgin\n" +
            "         inner join Branches b2\n" +
            "            on b2.branchCode = c.destination\n" +
            "         inner join ExpressCenters ec\n" +
            "            on ec.expressCenterCode = c.originExpressCenter\n" +
            "         inner join ExpressCenters ec2\n" +
            "            on ec2.expressCenterCode = c.destinationExpressCenterCode\n" +
            "         inner join ConsignmentType ct\n" +
            "            on CAST(c.consignmentTypeId as varchar) = CAST(ct.id as varchar)\n" +
            "          left outer join ZNI_USER1 z\n" +
            "            on CAST(z.U_ID as varchar) = c.createdBy\n" +
            "          left outer join Riders r\n" +
            "          on r.riderCode = c.riderCode\n" +
            "          and r.branchId = c.branchCode\n" +
            "         WHERE CONVERT(date, c.accountReceivingDate, 105) >= '" + clvar.FromDate.ToString("yyyy-MM-dd") + "' AND CONVERT(date, c.accountReceivingDate, 105) <= '" + clvar.ToDate.ToString("yyyy-MM-dd") + "'\n" +
            "           and c.orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' " + clvar.AccountNo + "\n" +
            "\n" +
            "\n" +
            "        ) A\n" +
            " ORDER BY " + clvar.CheckCondition + "";

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
        public void InsertConsignmentBookingDetailReportLog(Cl_Variables clvar)
        {
            string query = "Insert into mnp_consignmentBookingDetailReportLog (Criteria, AccountNo, RiderCode, DateFrom, DateTo, Sort, USERID) Values(\n" +
                           " " + clvar.CheckCondition + " )";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }

        public Cl_Variables Add_ExpressCenter(Cl_Variables obj)
        {
            clvar.Error = "";
            int IsUnique = 0;
            SqlConnection sqlcon = new SqlConnection(obj.Strcon());
            //    obj.Call_Log_ID = int.Parse(Get_maxValue()) + 1;
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("spSaveExpCenterDetail", sqlcon);
                // SqlCommand sqlcmd = new SqlCommand("MNP_InsertConsignment_Temp", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                // SqlParameter SParam = new SqlParameter("ReturnValue", SqlDbType.Int);
                // SParam.Direction = ParameterDirection.ReturnValue;
                //
                sqlcmd.Parameters.AddWithValue("@ExpCenCode", obj.expresscenter);
                sqlcmd.Parameters.AddWithValue("@Name", obj.expresscentername);
                sqlcmd.Parameters.AddWithValue("@Description", obj.productDescription);
                sqlcmd.Parameters.AddWithValue("@BranchId", obj.Branch);
                sqlcmd.Parameters.AddWithValue("@ShortName", obj.shortName);
                sqlcmd.Parameters.AddWithValue("@Email", obj.consignerEmail);
                sqlcmd.Parameters.AddWithValue("@Phone", obj.ConsignerPhone);
                sqlcmd.Parameters.AddWithValue("@Fax", obj.Fax);
                sqlcmd.Parameters.AddWithValue("@HasStore", "0");
                sqlcmd.Parameters.AddWithValue("@IsDistributionCenter", obj.isdistributionCenter);
                sqlcmd.Parameters.AddWithValue("@ClientId", obj.CreditClientID);
                sqlcmd.Parameters.AddWithValue("@CostCenterId", "0");
                sqlcmd.Parameters.AddWithValue("@IsFranchised", "0");
                sqlcmd.Parameters.AddWithValue("@FranchiseeAccountNo", obj.AccountNo);
                sqlcmd.Parameters.AddWithValue("@IsActive", "1");
                sqlcmd.Parameters.AddWithValue("@UserName", obj.isExpUser);
                sqlcmd.Parameters.AddWithValue("@ExpressCenterType", obj.expresscentertype);
                sqlcmd.Parameters.AddWithValue("@Dayoff", obj.Dayoff);
                sqlcmd.Parameters.AddWithValue("@MainEC", obj.ismainEc);

                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch (Exception Err)
            {
                clvar.Error = Err.Message.ToString();
                /// IsUnique = 1;
            }
            finally
            { }
            //return IsUnique;
            return clvar;
        }

        public DataTable ClientInfo(Cl_Variables clvar)
        {

            string sqlString = "SELECT * FROM CreditClients cc WHERE cc.accountNo ='" + clvar.AccountNo + "' AND cc.branchCode='" + clvar.Branch + "'";

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

        public DataTable GetTarrifForEdit_1(Cl_Variables clvar)
        {
            string sqlString = " select z.name ZoneName, t.* From tempClientTariff t \n" +
                "inner join Zones z\n" +
             "on z.zoneCode = t.ToZoneCode\n" +
             "where t.Client_Id = '" + clvar.CustomerClientID + "'\n" +
             " and t.ServiceID = '" + clvar.ServiceTypeName + "' \n" +
             "--and isintltariff = '0' \n" +
             "and chkdeleted = '0' \n" +
             "and t.branchCode = '" + clvar.Branch + "'\n" +
             "order by ZoneName, ToZoneCode, FromWeight";
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

        public DataTable Get_invoiceAdjStatus()
        {
            string query = "select * from Mnp_InvoiceAdjustment_Status l where l.Active = '1'";
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

        public DataTable Get_PriceModifier()
        {
            string query = "SELECT* FROM PriceModifiers pm WHERE pm.[status] ='1'";
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

        public DataTable GetDutyType()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "select l.AttributeValue from rvdbo.Lookup l where l.AttributeGroup = 'RIDER_DUTY_TYPE' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        public DataTable Get_Cities()
        {
            string query = "SELECT * FROM CITIES ORDER BY CITYNAME";
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

        public DataTable GetCID()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "select l.AttributeValue from rvdbo.Lookup l where l.AttributeGroup = 'RIDER_TYPE' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }


        #region Talha Coding Control on Consignment check 11 Nov 2020

        //This function will only take ConsignmentNo, current location, page



        #endregion



    }
}