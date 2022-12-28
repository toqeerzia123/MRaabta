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
    public class bayer_Function
    {
        public bayer_Function()
        {
            //
            // TODO: Add constructor logic here
            //
        }

    //    #region Common Functions

    //    //public DataSet Get_Depot(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = " SELECT d.link || '--' || d.link_name " +
    //    //                       " FROM depot d WHERE ldesc = 'Major' AND LEFT(d.depot, 3) LIKE '" + clvar.Depot + "'";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    { }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_AreaManager(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = "SELECT c5.comp5 AM_CODE,c5.ldesc AM, C4.comp4 SM_Code,c4.ldesc SM\n" +
    //    //                        "  FROm COMP_LEVEL5 c5, comp_level4 c4\n" +
    //    //                        " WHere c5.comp4 = c4.comp4 and c5.company = c4.company \n" +
    //    //                        "   AND c4.comp4 = '" + clvar.SalesManager + "' and c4.company ='02' order by AM ";

    //    //        /* string query = "SELECT comp5, ldesc\n" +
    //    //                        "  FROM Comp_Level5 c5\n" +
    //    //                        " WHERE c5.Additional_Field2 = 'Active'\n" +
    //    //                        " order by ldesc";
    //    //         */
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Er)
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_FMOList(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = "SELECT c6.comp6 FMO_CODE, c6.ldesc FMO, C5.comp5 AM_Code, c5.ldesc AM\n" +
    //    //                      "  FROm COMP_LEVEL6 c6, comp_level5 c5\n" +
    //    //                    " WHere c6.comp5 = c5.comp5\n" +
    //    //                    " and c6.company = c5.company \n" +
    //    //                    " AND c5.comp5 = '" + clvar.AreaManagerID + "' and c6.company='02' \n" +
    //    //                    " order by FMO";



    //    //        //"SELECT c5.comp5 AM_CODE,c5.ldesc AM, C4.comp4 SM_Code,c4.ldesc SM\n" +
    //    //        //            "  FROm COMP_LEVEL5 c5, comp_level4 c4\n" +
    //    //        //            " WHere c5.comp4 = c4.comp4\n" +
    //    //        //            "   AND c4.comp4 = '" + clvar.SalesManager + "'  order by AM ";

    //    //        /* string query = "SELECT comp5, ldesc\n" +
    //    //                        "  FROM Comp_Level5 c5\n" +
    //    //                        " WHERE c5.Additional_Field2 = 'Active'\n" +
    //    //                        " order by ldesc";
    //    //         */
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Er)
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_SalesManager(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = "SELECT comp4,ldesc FROM Comp_Level4 WHERE Additional_field2 = 'Active' " +
    //    //        "AND COMP3 = '" + clvar.BusinessHead + "' and company='02' order by ldesc";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Er)
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_Principal(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = " SELECT pr_code||'--'||pr_name  FROM zni.db_principal_snd WHERE pr_code ='" + clvar.Principal + "'";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    { }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_Bayer_Depot(Variable clvar)
    //    //{
    //    //    string query = @"SELECT distinct left(d.depot, 3) depot, d.depot_Name FROM depot d order by depot";

    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    { }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_Bayer_Depot_WithOutWarehouse(Variable clvar)
    //    //{
    //    //    string query = "SELECT distinct\n" +
    //    //                "  left(d.depot, 3) depot, d.depot_Name\n" +
    //    //                "  FROM depot d\n" +
    //    //                "WHERE DEPOT_Name not like 'CWH%'\n" +
    //    //                "\n" +
    //    //                " order by depot";

    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    { }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet get_principalbydivision(Variable clvar)
    //    //{
    //    //    string query = @" SELECT pr_code,pr_name,sdesc FROM db_principal_snd WHERE prod1='" + clvar.Division_ID + "' and pr_code IN('01','55')  order by pr_code";

    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_Zni());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    { }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    public DataTable get_Zone()
    //    {
    //        DataTable dta = new DataTable();
    //        dta.Columns.Add("ZoneID");
    //        dta.Columns.Add("ZoneName");
    //        DataRow dr = dta.NewRow();
    //        dta.Rows.Add("1", "Zone1");
    //        dta.Rows.Add("2", "Zone2");
    //        dta.Rows.Add("3", "Zone3");
    //        dta.Rows.Add("4", "Zone4");
    //        dta.AcceptChanges();
    //        return dta;
    //    }

    //    //public DataSet get_Division(Variable clvar)
    //    //{
    //    //    string query = @" SELECT
	   //    //             DISTINCT dp.prod1 div_id,
	   //    //             DECODE(
		  //    //              dp.prod1,
		  //    //              'PC_0101',
		  //    //              'Pharma',
		  //    //              'PC_0201',
		  //    //              'CPD',
		  //    //              'PC_0301',
		  //    //              'HCU',
		  //    //              'PC_0401',
		  //    //              'Stationary',
		  //    //              'PC_0501',
		  //    //              'Telecom'
	   //    //             ) division
    //    //            FROM 
	   //    //             db_principal_snd dp order by dp.prod1";

    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_Zni());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    { }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_DepotByZoneID(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = @"SELECT Distinct left(d.depot, 3),depot_sname FROM Depot d where d.P_Zone = '" + clvar.Zone + "'";
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    { }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_MonthBySale(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = "SELECT TNAME TBL_NAME,\n" +
    //    //                        "       DECODE(SUBSTR(TNAME, 7, 3),\n" +
    //    //                        "              'JAN',\n" +
    //    //                        "              '01',\n" +
    //    //                        "              'FEB',\n" +
    //    //                        "              '02',\n" +
    //    //                        "              'MAR',\n" +
    //    //                        "              '03',\n" +
    //    //                        "              'APR',\n" +
    //    //                        "              '04',\n" +
    //    //                        "              'MAY',\n" +
    //    //                        "              '05',\n" +
    //    //                        "              'JUN',\n" +
    //    //                        "              '06',\n" +
    //    //                        "              'JUL',\n" +
    //    //                        "              '07',\n" +
    //    //                        "              'AUG',\n" +
    //    //                        "              '08',\n" +
    //    //                        "              'SEP',\n" +
    //    //                        "              '09',\n" +
    //    //                        "              'OCT',\n" +
    //    //                        "              '10',\n" +
    //    //                        "              'NOV',\n" +
    //    //                        "              '11',\n" +
    //    //                        "              'DEC',\n" +
    //    //                        "              '12') MONTH_VAL,\n" +
    //    //                        "       SUBSTR(TNAME, 7) MONTH\n" +
    //    //                        "  FROM TAB\n" +
    //    //                        " WHERE TNAME LIKE 'SALES_%'\n" +
    //    //                        "   AND TABTYPE = 'TABLE'\n" +
    //    //                        "   AND SUBSTR(TNAME, 11) > = '11'\n" +
    //    //                        "   AND  TNAME NOT LIKE '%AFG'\n" +
    //    //                        "   AND TNAME NOT IN\n" +
    //    //                        "       ('SALES_GROUP', 'SALES_GROUP_SND', 'SALES_GROUP_SND_BK_270712')\n" +
    //    //                        "   AND INSTR(TNAME, 'YTD') = 0\n" +
    //    //                        "ORDER BY SUBSTR(TNAME, 11, 2) DESC,\n" +
    //    //                        "          TO_NUMBER(DECODE(SUBSTR(TNAME, 7, 3),\n" +
    //    //                        "                           'JAN',\n" +
    //    //                        "                           '01',\n" +
    //    //                        "                           'FEB',\n" +
    //    //                        "                           '02',\n" +
    //    //                        "                           'MAR',\n" +
    //    //                        "                           '03',\n" +
    //    //                        "                           'APR',\n" +
    //    //                        "                           '04',\n" +
    //    //                        "                           'MAY',\n" +
    //    //                        "                           '05',\n" +
    //    //                        "                           'JUN',\n" +
    //    //                        "                           '06',\n" +
    //    //                        "                           'JUL',\n" +
    //    //                        "                           '07',\n" +
    //    //                        "                           'AUG',\n" +
    //    //                        "                           '08',\n" +
    //    //                        "                           'SEP',\n" +
    //    //                        "                           '09',\n" +
    //    //                        "                           'OCT',\n" +
    //    //                        "                           '10',\n" +
    //    //                        "                           'NOV',\n" +
    //    //                        "                           '11',\n" +
    //    //                        "                           'DEC',\n" +
    //    //                        "                           '12')) DESC";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet get_Products_Principal(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        string query = "SELECT prd_code, prd_code||'=='||prd_name prd_name  FROM itemmaster_snd WHERE left(prd_code,2) in (" + clvar.Principal + ") order by prd_name";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet get_Products_Principal_SalesGrop(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        //string query = "SELECT prd_code, prd_code||'=='||prd_name prd_name  FROM itemmaster_snd WHERE left(prd_code,2) in (" + clvar.Principal + ") order by prd_name";

    //    //        string query = "SELECT prd_code, prd_code || '==' || prd_name prd_name\n" +
    //    //        "  FROM itemmaster_snd\n" +
    //    //        " WHERE left(prd_code, 2) in (" + clvar.Principal + ") \n" +
    //    //        clvar.SalesGroup +
    //    //        " order by prd_name";



    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_SalesGroup(Variable clvar)
    //    //{

    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT sgs.sales_group, sgs.grp_name FROM sales_group_snd sgs WHERE LEFT(sgs.sales_group, 2) IN (" + clvar.Principal + ")";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_SalesGroup_bayer(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT sgs.sales_group, sgs.grp_name  FROM Sales_Group_Snd sgs";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_SalesGroup_SNDQA(Variable clvar)
    //    //{

    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT DISTINCT SG.SALE_GROUP, SG.SDESC " +
    //    //                       " FROM sndqa.SALE_GROUP SG, sndqa.MASTER_SKU MS " +
    //    //                       " WHERE SG.COMPANY = MS.COMPANY AND SG.SALE_GROUP = MS.SALE_GROUP " +
    //    //                       " AND MS.PROD1 = '" + clvar.Division + "' AND MS.PROD2 = '" + clvar.Principal + "' ORDER BY SG.SDESC";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_YearBySNDQA(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT DISTINCT YEAR FROM CALENDAR where YEAR >= 2011 ORDER BY YEAR DESC";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_SNDQA());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_DepotBySndqa(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT d.distributor, d.NAME FROM sndqa.distributor d WHERE d.distributor NOT LIKE '2%'";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_SNDQA());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_DivisionbySNDQA(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT PROD1, UPPER(SDESC) SDESC FROM PROD_LEVEL1 ORDER BY PROD1";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_SNDQA());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_PrincipalBySNDQA(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT PROD2, UPPER(SDESC) SDESC FROM PROD_LEVEL2 WHERE PROD1 = '" + clvar.Division_ID + "' and additional_field1 = 1 ORDER BY PROD2";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_SNDQA());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_SalesGroup_Telecom(Variable clvar)
    //    //{

    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT DISTINCT SG.SALE_GROUP, SG.SDESC " +
    //    //                       " FROM sndqa.SALE_GROUP SG, sndqa.MASTER_SKU MS " +
    //    //                       " WHERE SG.COMPANY = MS.COMPANY AND SG.SALE_GROUP = MS.SALE_GROUP " +
    //    //                       " AND MS.PROD1 = '" + clvar.Division + "' AND MS.PROD2 = '" + clvar.Principal + "' ORDER BY SG.SDESC";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_Telecom());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_DepotByTelecom(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT d.distributor, d.NAME FROM sndqa.distributor d WHERE d.distributor NOT LIKE '2%'";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_Telecom());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_PrincipalByTelecom(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT PROD2, UPPER(SDESC) SDESC FROM PROD_LEVEL2 WHERE PROD1 = '" + clvar.Division + "' and additional_field1 = 1 ORDER BY PROD2";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_Telecom());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet ProductBySNDQA(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = " SELECT MASTER_SKU SKU, MASTER_SKU || '-' || SDESC SDESC " +
    //    //                       " FROM MASTER_SKU " +
    //    //                       " WHERE PROD1 ='" + clvar.Division + "'" +
    //    //                       " AND PROD2 = '" + clvar.Principal + "'" +
    //    //                       " AND SALE_GROUP = '" + clvar.SalesGroup + "'" +
    //    //                       " ORDER BY MASTER_SKU";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_SNDQA());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet ProductByTelecom(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = " SELECT MASTER_SKU SKU, MASTER_SKU || '-' || SDESC SDESC " +
    //    //                       " FROM MASTER_SKU " +
    //    //                       " WHERE PROD1 ='" + clvar.Division + "'" +
    //    //                       " AND PROD2 = '" + clvar.Principal + "'" +
    //    //                       " AND SALE_GROUP = '" + clvar.SalesGroup + "'" +
    //    //                       " ORDER BY MASTER_SKU";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_Telecom());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_ProductByProductID(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    try
    //    //    {
    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        string query = "SELECT prd_code,prd_name  FROM itemmaster_snd WHERE prd_code ='" + clvar.Prdcd + "'";
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    {

    //    //    }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_DivisionbyBayer(Variable clvar)
    //    //{
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl where cl.company ='02'";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon_SNDQA());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    //public DataSet Get_MonthByStockNSales(Variable clvar)
    //    //{
    //    //    string query = @"SELECT TNAME TBL_NAME,
    //    //               DECODE(SUBSTR(TNAME, 7, 3),
    //    //                      'JAN',
    //    //                      '01',
    //    //                      'FEB',
    //    //                      '02',
    //    //                      'MAR',
    //    //                      '03',
    //    //                      'APR',
    //    //                      '04',
    //    //                      'MAY',
    //    //                      '05',
    //    //                      'JUN',
    //    //                      '06',
    //    //                      'JUL',
    //    //                      '07',
    //    //                      'AUG',
    //    //                      '08',
    //    //                      'SEP',
    //    //                      '09',
    //    //                      'OCT',
    //    //                      '10',
    //    //                      'NOV',
    //    //                      '11',
    //    //                      'DEC',
    //    //                      '12') MONTH_VAL,
    //    //               SUBSTR(TNAME,15) MONTH
    //    //          FROM TAB
    //    //         WHERE TNAME LIKE 'STOCK_N_SALES_%'
    //    //           AND TNAME NOT LIKE 'STOCK_N_SALES_VW%'
    //    //           AND (SUBSTR(TNAME, 19, 2)) >= '11'
    //    //           AND tname NOT IN
    //    //               ('STOCK_N_SALES_12', 'STOCK_N_SALES_11', 'STOCK_N_SALES_10',
    //    //                'STOCK_N_SALES_09', 'STOCK_N_SALES_08', 'STOCK_N_SALES_07')

    //    //         ORDER BY TO_NUMBER(SUBSTR(TNAME, 19, 3)) DESC,
    //    //                  TO_NUMBER(DECODE(SUBSTR(TNAME, 15, 3),
    //    //                                   'JAN',
    //    //                                   '01',
    //    //                                   'FEB',
    //    //                                   '02',
    //    //                                   'MAR',
    //    //                                   '03',
    //    //                                   'APR',
    //    //                                   '04',
    //    //                                   'MAY',
    //    //                                   '05',
    //    //                                   'JUN',
    //    //                                   '06',
    //    //                                   'JUL',
    //    //                                   '07',
    //    //                                   'AUG',
    //    //                                   '08',
    //    //                                   'SEP',
    //    //                                   '09',
    //    //                                   'OCT',
    //    //                                   '10',
    //    //                                   'NOV',
    //    //                                   '11',
    //    //                                   'DEC',
    //    //                                   '12')) DESC";
    //    //    DataSet ds = new DataSet();
    //    //    string temp = "";
    //    //    try
    //    //    {
    //    //        // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //    //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //    //        orcl.Open();
    //    //        OracleCommand orcd = new OracleCommand(query, orcl);
    //    //        orcd.CommandType = CommandType.Text;
    //    //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //    //        oda.Fill(ds);
    //    //        orcl.Close();
    //    //    }
    //    //    catch (Exception Err)
    //    //    {

    //    //    }
    //    //    finally
    //    //    { }
    //    //    return ds;
    //    //}

    //    public DataTable get_BayerFmo_Val()
    //    {
    //        DataTable dta = new DataTable();
    //        dta.Columns.Add("FMOTEAMID");
    //        dta.Columns.Add("BAYERFMO");
    //        DataRow dr = dta.NewRow();
    //        dta.Rows.Add("FC", "FC");
    //        dta.Rows.Add("GT", "GT");
    //        dta.Rows.Add("NT-A", "NT-A");
    //        dta.Rows.Add("NT-B", "NT-B");
    //        dta.Rows.Add("NT", "NT");
    //        dta.Rows.Add("AI", "AI");
    //        dta.Rows.Add("CM", "CM");
    //        dta.Rows.Add("R&I", "R&I");
    //        dta.Rows.Add("SP", "SP");
    //        dta.AcceptChanges();
    //        return dta;
    //    }

    //    public DataTable get_BayerFmo_Val1()
    //    {
    //        DataTable dta = new DataTable();
    //        dta.Columns.Add("FMOTEAMID");
    //        dta.Columns.Add("BAYERFMO");
    //        DataRow dr = dta.NewRow();
    //        dta.Rows.Add("WHC", "WHC");
    //        dta.Rows.Add("NT", "NT");
    //        dta.Rows.Add("AI", "AI");
    //        dta.Rows.Add("CM", "CM");
    //        dta.Rows.Add("R&I", "R&I");
    //        dta.Rows.Add("SP", "SP");
    //        dta.AcceptChanges();
    //        return dta;
    //    }

    //    public DataSet Get_FMOSaleTable(Variable clvar)
    //    {
    //        string query = @"SELECT
	   //                 TNAME TBL_NAME,
	   //                 DECODE(
		  //                  SUBSTR(TNAME, 10, 3),
		  //                  'JAN',
		  //                  '01',
		  //                  'FEB',
		  //                  '02',
		  //                  'MAR',
		  //                  '03',
		  //                  'APR',
		  //                  '04',
		  //                  'MAY',
		  //                  '05',
		  //                  'JUN',
		  //                  '06',
		  //                  'JUL',
		  //                  '07',
		  //                  'AUG',
		  //                  '08',
		  //                  'SEP',
		  //                  '09',
		  //                  'OCT',
		  //                  '10',
		  //                  'NOV',
		  //                  '11',
		  //                  'DEC',
		  //                  '12'
	   //                 ) MONTH_VAL,
	   //                 SUBSTR(TNAME, 10) MONTH
    //                FROM
	   //                 TAB
    //                WHERE
	   //                 TNAME LIKE 'FMO%'
	   //                 AND TABTYPE = 'TABLE'
    //                    and TNAME not like '%_COPY%'
    //                    and TNAME not like '%_BKP%'
    //                    and TNAME not like '%AFG%'
    //                    AND tname NOT LIKE '%FMO_SALE_SEP13_18092013%'
    //                ORDER BY
	   //                 TO_NUMBER(SUBSTR(TNAME, 14, 2)) DESC,
	   //                 TO_NUMBER(
		  //                  DECODE(
			 //                   SUBSTR(TNAME, 10, 3),
			 //                   'JAN',
			 //                   '01',
			 //                   'FEB',
			 //                   '02',
			 //                   'MAR',
			 //                   '03',
			 //                   'APR',
			 //                   '04',
			 //                   'MAY',
			 //                   '05',
			 //                   'JUN',
			 //                   '06',
			 //                   'JUL',
			 //                   '07',
			 //                   'AUG',
			 //                   '08',
			 //                   'SEP',
			 //                   '09',
			 //                   'OCT',
			 //                   '10',
			 //                   'NOV',
			 //                   '11',
			 //                   'DEC',
			 //                   '12'
		  //                  )
	   //                 ) DESC";
    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_FMOSalesSingleYear(Variable clvar)
    //    {
    //        string query = "SELECT\n" +
    //"\t                    TNAME TBL_NAME,\n" +
    //"\t                    DECODE(\n" +
    //"\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t                    'JAN',\n" +
    //"\t\t                    '01',\n" +
    //"\t\t                    'FEB',\n" +
    //"\t\t                    '02',\n" +
    //"\t\t                    'MAR',\n" +
    //"\t\t                    '03',\n" +
    //"\t\t                    'APR',\n" +
    //"\t\t                    '04',\n" +
    //"\t\t                    'MAY',\n" +
    //"\t\t                    '05',\n" +
    //"\t\t                    'JUN',\n" +
    //"\t\t                    '06',\n" +
    //"\t\t                    'JUL',\n" +
    //"\t\t                    '07',\n" +
    //"\t\t                    'AUG',\n" +
    //"\t\t                    '08',\n" +
    //"\t\t                    'SEP',\n" +
    //"\t\t                    '09',\n" +
    //"\t\t                    'OCT',\n" +
    //"\t\t                    '10',\n" +
    //"\t\t                    'NOV',\n" +
    //"\t\t                    '11',\n" +
    //"\t\t                    'DEC',\n" +
    //"\t\t                    '12'\n" +
    //"\t                    ) MONTH_VAL,\n" +
    //"\t                    SUBSTR(TNAME, 10) MONTH\n" +
    //"                    FROM\n" +
    //"\t                    TAB\n" +
    //"                    WHERE\n" +
    //"\t                    TNAME LIKE 'FMO%'\n" +
    //"\t                    AND TABTYPE = 'TABLE'\n" +
    //"                        and TNAME not like '%_COPY%'\n" +
    //"                        and TNAME not like '%_BKP%'\n AND tname NOT LIKE '%FMO_SALE_SEP13_18092013%'" +
    //"                        and TNAME like '%" + clvar.Check_Condition2 + "'\n" +
    //"\n" +
    //"                    ORDER BY\n" +
    //"\t                    TO_NUMBER(SUBSTR(TNAME, 14, 2)) DESC,\n" +
    //"\t                    TO_NUMBER(\n" +
    //"\t\t                    DECODE(\n" +
    //"\t\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t\t                    'JAN',\n" +
    //"\t\t\t                    '01',\n" +
    //"\t\t\t                    'FEB',\n" +
    //"\t\t\t                    '02',\n" +
    //"\t\t\t                    'MAR',\n" +
    //"\t\t\t                    '03',\n" +
    //"\t\t\t                    'APR',\n" +
    //"\t\t\t                    '04',\n" +
    //"\t\t\t                    'MAY',\n" +
    //"\t\t\t                    '05',\n" +
    //"\t\t\t                    'JUN',\n" +
    //"\t\t\t                    '06',\n" +
    //"\t\t\t                    'JUL',\n" +
    //"\t\t\t                    '07',\n" +
    //"\t\t\t                    'AUG',\n" +
    //"\t\t\t                    '08',\n" +
    //"\t\t\t                    'SEP',\n" +
    //"\t\t\t                    '09',\n" +
    //"\t\t\t                    'OCT',\n" +
    //"\t\t\t                    '10',\n" +
    //"\t\t\t                    'NOV',\n" +
    //"\t\t\t                    '11',\n" +
    //"\t\t\t                    'DEC',\n" +
    //"\t\t\t                    '12'\n" +
    //"\t\t                    )\n" +
    //"\t                    ) DESC\n" +
    //"\n" +
    //"";

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_FMOSalesSingleYear_new(Variable clvar)
    //    {
    //        string query = "SELECT\n" +
    //"\t                    TNAME TBL_NAME,\n" +
    //"\t                    DECODE(\n" +
    //"\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t                    'JAN',\n" +
    //"\t\t                    '01',\n" +
    //"\t\t                    'FEB',\n" +
    //"\t\t                    '02',\n" +
    //"\t\t                    'MAR',\n" +
    //"\t\t                    '03',\n" +
    //"\t\t                    'APR',\n" +
    //"\t\t                    '04',\n" +
    //"\t\t                    'MAY',\n" +
    //"\t\t                    '05',\n" +
    //"\t\t                    'JUN',\n" +
    //"\t\t                    '06',\n" +
    //"\t\t                    'JUL',\n" +
    //"\t\t                    '07',\n" +
    //"\t\t                    'AUG',\n" +
    //"\t\t                    '08',\n" +
    //"\t\t                    'SEP',\n" +
    //"\t\t                    '09',\n" +
    //"\t\t                    'OCT',\n" +
    //"\t\t                    '10',\n" +
    //"\t\t                    'NOV',\n" +
    //"\t\t                    '11',\n" +
    //"\t\t                    'DEC',\n" +
    //"\t\t                    '12'\n" +
    //"\t                    ) MONTH_VAL,\n" +
    //"\t                    SUBSTR(TNAME, 10) MONTH\n" +
    //"                    FROM\n" +
    //"\t                    TAB\n" +
    //"                    WHERE\n" +
    //"\t                    TNAME LIKE 'FMO%'\n" +
    //"\t                    AND TABTYPE = 'TABLE'\n" +
    //"                        and TNAME not like '%_COPY%'\n" +
    //"                        and TNAME not like '%_BKP%'\n" +
    //"                        and TNAME like '%" + clvar.Check_Condition2 + "'\n" +
    //"\n" +
    //"                    ORDER BY\n" +
    //"\t                    TO_NUMBER(SUBSTR(TNAME, 14, 2)) DESC,\n" +
    //"\t                    TO_NUMBER(\n" +
    //"\t\t                    DECODE(\n" +
    //"\t\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t\t                    'JAN',\n" +
    //"\t\t\t                    '01',\n" +
    //"\t\t\t                    'FEB',\n" +
    //"\t\t\t                    '02',\n" +
    //"\t\t\t                    'MAR',\n" +
    //"\t\t\t                    '03',\n" +
    //"\t\t\t                    'APR',\n" +
    //"\t\t\t                    '04',\n" +
    //"\t\t\t                    'MAY',\n" +
    //"\t\t\t                    '05',\n" +
    //"\t\t\t                    'JUN',\n" +
    //"\t\t\t                    '06',\n" +
    //"\t\t\t                    'JUL',\n" +
    //"\t\t\t                    '07',\n" +
    //"\t\t\t                    'AUG',\n" +
    //"\t\t\t                    '08',\n" +
    //"\t\t\t                    'SEP',\n" +
    //"\t\t\t                    '09',\n" +
    //"\t\t\t                    'OCT',\n" +
    //"\t\t\t                    '10',\n" +
    //"\t\t\t                    'NOV',\n" +
    //"\t\t\t                    '11',\n" +
    //"\t\t\t                    'DEC',\n" +
    //"\t\t\t                    '12'\n" +
    //"\t\t                    )\n" +
    //"\t                    ) ASC\n" +
    //"\n" +
    //"";

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_FMOSalesSingleYear_2(Variable clvar)
    //    {
    //        string query = "SELECT\n" +
    //"\t                    TNAME TBL_NAME,\n" +
    //"\t                    DECODE(\n" +
    //"\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t                    'JAN',\n" +
    //"\t\t                    '01',\n" +
    //"\t\t                    'FEB',\n" +
    //"\t\t                    '02',\n" +
    //"\t\t                    'MAR',\n" +
    //"\t\t                    '03',\n" +
    //"\t\t                    'APR',\n" +
    //"\t\t                    '04',\n" +
    //"\t\t                    'MAY',\n" +
    //"\t\t                    '05',\n" +
    //"\t\t                    'JUN',\n" +
    //"\t\t                    '06',\n" +
    //"\t\t                    'JUL',\n" +
    //"\t\t                    '07',\n" +
    //"\t\t                    'AUG',\n" +
    //"\t\t                    '08',\n" +
    //"\t\t                    'SEP',\n" +
    //"\t\t                    '09',\n" +
    //"\t\t                    'OCT',\n" +
    //"\t\t                    '10',\n" +
    //"\t\t                    'NOV',\n" +
    //"\t\t                    '11',\n" +
    //"\t\t                    'DEC',\n" +
    //"\t\t                    '12'\n" +
    //"\t                    ) MONTH_VAL,\n" +
    //"\t                    SUBSTR(TNAME, 10) MONTH\n" +
    //"                    FROM\n" +
    //"\t                    TAB\n" +
    //"                    WHERE\n" +
    //"\t                    TNAME LIKE 'FMO%'\n" +
    //"\t                    AND TABTYPE = 'TABLE'\n" +
    //"                        and TNAME not like '%_COPY%'\n" +
    //"                        and TNAME not like '%_BKP%'\n" +
    //"                        and TNAME not like '%_AFG%'\n" +
    //"                    ORDER BY\n" +
    //"\t                    TO_NUMBER(SUBSTR(TNAME, 14, 2)) DESC,\n" +
    //"\t                    TO_NUMBER(\n" +
    //"\t\t                    DECODE(\n" +
    //"\t\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t\t                    'JAN',\n" +
    //"\t\t\t                    '01',\n" +
    //"\t\t\t                    'FEB',\n" +
    //"\t\t\t                    '02',\n" +
    //"\t\t\t                    'MAR',\n" +
    //"\t\t\t                    '03',\n" +
    //"\t\t\t                    'APR',\n" +
    //"\t\t\t                    '04',\n" +
    //"\t\t\t                    'MAY',\n" +
    //"\t\t\t                    '05',\n" +
    //"\t\t\t                    'JUN',\n" +
    //"\t\t\t                    '06',\n" +
    //"\t\t\t                    'JUL',\n" +
    //"\t\t\t                    '07',\n" +
    //"\t\t\t                    'AUG',\n" +
    //"\t\t\t                    '08',\n" +
    //"\t\t\t                    'SEP',\n" +
    //"\t\t\t                    '09',\n" +
    //"\t\t\t                    'OCT',\n" +
    //"\t\t\t                    '10',\n" +
    //"\t\t\t                    'NOV',\n" +
    //"\t\t\t                    '11',\n" +
    //"\t\t\t                    'DEC',\n" +
    //"\t\t\t                    '12'\n" +
    //"\t\t                    )\n" +
    //"\t                    ) DESC\n" +
    //"\n" +
    //"";

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_FMOSalesSingleYear_PreviousYear(Variable clvar)
    //    {
    //        string query = "SELECT\n" +
    //"\t                    TNAME TBL_NAME,\n" +
    //"\t                    DECODE(\n" +
    //"\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t                    'JAN',\n" +
    //"\t\t                    '01',\n" +
    //"\t\t                    'FEB',\n" +
    //"\t\t                    '02',\n" +
    //"\t\t                    'MAR',\n" +
    //"\t\t                    '03',\n" +
    //"\t\t                    'APR',\n" +
    //"\t\t                    '04',\n" +
    //"\t\t                    'MAY',\n" +
    //"\t\t                    '05',\n" +
    //"\t\t                    'JUN',\n" +
    //"\t\t                    '06',\n" +
    //"\t\t                    'JUL',\n" +
    //"\t\t                    '07',\n" +
    //"\t\t                    'AUG',\n" +
    //"\t\t                    '08',\n" +
    //"\t\t                    'SEP',\n" +
    //"\t\t                    '09',\n" +
    //"\t\t                    'OCT',\n" +
    //"\t\t                    '10',\n" +
    //"\t\t                    'NOV',\n" +
    //"\t\t                    '11',\n" +
    //"\t\t                    'DEC',\n" +
    //"\t\t                    '12'\n" +
    //"\t                    ) MONTH_VAL,\n" +
    //"\t                    SUBSTR(TNAME, 10) MONTH\n" +
    //"                    FROM\n" +
    //"\t                    TAB\n" +
    //"                    WHERE\n" +
    //"\t                    TNAME LIKE 'FMO%'\n" +
    //"\t                    AND TABTYPE = 'TABLE'\n" +
    //"                        and TNAME not like '%_COPY%'\n" +
    //"                        and TNAME not like '%_BKP%'\n" +
    //"                        and TNAME like '%" + clvar.Check_Condition + "'\n" +
    //"\n" +
    //"                    ORDER BY\n" +
    //"\t                    TO_NUMBER(SUBSTR(TNAME, 14, 2)) DESC,\n" +
    //"\t                    TO_NUMBER(\n" +
    //"\t\t                    DECODE(\n" +
    //"\t\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t\t                    'JAN',\n" +
    //"\t\t\t                    '01',\n" +
    //"\t\t\t                    'FEB',\n" +
    //"\t\t\t                    '02',\n" +
    //"\t\t\t                    'MAR',\n" +
    //"\t\t\t                    '03',\n" +
    //"\t\t\t                    'APR',\n" +
    //"\t\t\t                    '04',\n" +
    //"\t\t\t                    'MAY',\n" +
    //"\t\t\t                    '05',\n" +
    //"\t\t\t                    'JUN',\n" +
    //"\t\t\t                    '06',\n" +
    //"\t\t\t                    'JUL',\n" +
    //"\t\t\t                    '07',\n" +
    //"\t\t\t                    'AUG',\n" +
    //"\t\t\t                    '08',\n" +
    //"\t\t\t                    'SEP',\n" +
    //"\t\t\t                    '09',\n" +
    //"\t\t\t                    'OCT',\n" +
    //"\t\t\t                    '10',\n" +
    //"\t\t\t                    'NOV',\n" +
    //"\t\t\t                    '11',\n" +
    //"\t\t\t                    'DEC',\n" +
    //"\t\t\t                    '12'\n" +
    //"\t\t                    )\n" +
    //"\t                    ) DESC\n" +
    //"\n" +
    //"";

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_FMOSalesSingleYear_PreviousYear_new(Variable clvar)
    //    {
    //        string query = "SELECT\n" +
    //"\t                    TNAME TBL_NAME,\n" +
    //"\t                    DECODE(\n" +
    //"\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t                    'JAN',\n" +
    //"\t\t                    '01',\n" +
    //"\t\t                    'FEB',\n" +
    //"\t\t                    '02',\n" +
    //"\t\t                    'MAR',\n" +
    //"\t\t                    '03',\n" +
    //"\t\t                    'APR',\n" +
    //"\t\t                    '04',\n" +
    //"\t\t                    'MAY',\n" +
    //"\t\t                    '05',\n" +
    //"\t\t                    'JUN',\n" +
    //"\t\t                    '06',\n" +
    //"\t\t                    'JUL',\n" +
    //"\t\t                    '07',\n" +
    //"\t\t                    'AUG',\n" +
    //"\t\t                    '08',\n" +
    //"\t\t                    'SEP',\n" +
    //"\t\t                    '09',\n" +
    //"\t\t                    'OCT',\n" +
    //"\t\t                    '10',\n" +
    //"\t\t                    'NOV',\n" +
    //"\t\t                    '11',\n" +
    //"\t\t                    'DEC',\n" +
    //"\t\t                    '12'\n" +
    //"\t                    ) MONTH_VAL,\n" +
    //"\t                    SUBSTR(TNAME, 10) MONTH\n" +
    //"                    FROM\n" +
    //"\t                    TAB\n" +
    //"                    WHERE\n" +
    //"\t                    TNAME LIKE 'FMO%'\n" +
    //"\t                    AND TABTYPE = 'TABLE'\n" +
    //"                        and TNAME not like '%_COPY%'\n" +
    //"                        and TNAME not like '%_BKP%'\n" +
    //"                        and TNAME like '%" + clvar.Check_Condition + "'\n" +
    //"\n" +
    //"                    ORDER BY\n" +
    //"\t                    TO_NUMBER(SUBSTR(TNAME, 14, 2)) DESC,\n" +
    //"\t                    TO_NUMBER(\n" +
    //"\t\t                    DECODE(\n" +
    //"\t\t\t                    SUBSTR(TNAME, 10, 3),\n" +
    //"\t\t\t                    'JAN',\n" +
    //"\t\t\t                    '01',\n" +
    //"\t\t\t                    'FEB',\n" +
    //"\t\t\t                    '02',\n" +
    //"\t\t\t                    'MAR',\n" +
    //"\t\t\t                    '03',\n" +
    //"\t\t\t                    'APR',\n" +
    //"\t\t\t                    '04',\n" +
    //"\t\t\t                    'MAY',\n" +
    //"\t\t\t                    '05',\n" +
    //"\t\t\t                    'JUN',\n" +
    //"\t\t\t                    '06',\n" +
    //"\t\t\t                    'JUL',\n" +
    //"\t\t\t                    '07',\n" +
    //"\t\t\t                    'AUG',\n" +
    //"\t\t\t                    '08',\n" +
    //"\t\t\t                    'SEP',\n" +
    //"\t\t\t                    '09',\n" +
    //"\t\t\t                    'OCT',\n" +
    //"\t\t\t                    '10',\n" +
    //"\t\t\t                    'NOV',\n" +
    //"\t\t\t                    '11',\n" +
    //"\t\t\t                    'DEC',\n" +
    //"\t\t\t                    '12'\n" +
    //"\t\t                    )\n" +
    //"\t                    ) ASC\n" +
    //"\n" +
    //"";

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public void Update_Password(Variable clvar)
    //    {
    //        string query = "UPDATE zni_user1 SET u_password ='" + clvar.Password + "' WHERE u_code ='" + clvar.UserName + "'";
    //        try
    //        {
    //            // string query = "SELECT cl.comp1, cl.ldesc  FROM comp_level1 cl";

    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            orcd.ExecuteNonQuery();
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }

    //    }

    //    public DataSet Get_Processdate(Variable clvar)
    //    {
    //        string query = " SELECT to_char(MAX(PROCESS_DATE),'dd, Mon, yyyy') FROM stock_n_sales_13";
    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_Teams(Variable clvar)
    //    {
    //        string query = "SELECT prod1||''||prod2||''||prod3 ID, sdesc Team FROM prod_level3 p3 WHERE prod1||''||prod2='" + clvar.Team + "'" +
    //                        "and p3.sdesc not in('GM') ";

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_Teams1(Variable clvar)
    //    {
    //        string query = "SELECT prod1||''||prod2||''||prod3 ID, sdesc Team FROM prod_level3 p3 ";

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    public DataSet Get_Products_bySalesGroup(Variable clvar)
    //    {
    //        string query = "SELECT \n"
    //                       + "	item_snd.prd_code, \n"
    //                       + "	item_snd.prd_name \n"
    //                       + "FROM \n"
    //                       + "	itemmaster_snd item_Snd \n"
    //                       + "WHERE \n"
    //                       + "	item_Snd.Sales_Group = '" + clvar.SaleGroupID + "'"; ;

    //        DataSet ds = new DataSet();
    //        string temp = "";
    //        try
    //        {
    //            OracleConnection orcl = new OracleConnection(clvar.Strcon());
    //            orcl.Open();
    //            OracleCommand orcd = new OracleCommand(query, orcl);
    //            orcd.CommandType = CommandType.Text;
    //            OracleDataAdapter oda = new OracleDataAdapter(orcd);
    //            oda.Fill(ds);
    //            orcl.Close();
    //        }
    //        catch (Exception Err)
    //        {

    //        }
    //        finally
    //        { }
    //        return ds;
    //    }

    //    #endregion

        #region MISOCS

        // ======================== Added by Bilal ========================

        public DataSet Get_AllZones(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select za.zoneCode, z.Name Zone, c1.cityName\n" +
                                "from \n" +
                                "Zones z, ZoneCityAssociation za, Cities c1\n" +
                                "where z.zoneCode = za.zoneCode\n" +
                                "and za.cityId = c1.id\n" +
                                "group by za.zoneCode, z.Name , c1.cityName order by c1.cityName";

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

        /*
        public DataSet Get_AllZones1(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    query = "select zoneCode, Name\n" +
                                    "  from Zones z\n" +
                                    " where z.type = '1'\n" +
                                    "   AND z.zoneCode in ('1',\n" +
                                    "'10',\n" +
                                    "'11',\n" +
                                    "'12',\n" +
                                    "'2',\n" +
                                    "'27',\n" +
                                    "'3',\n" +
                                    "'4',\n" +
                                    "'5',\n" +
                                    "'7',\n" +
                                    "'9')\n" +
                        //     " AND zoneCode IN (" + HttpContext.Current.Session["ZONECODE"].ToString() + ") \n" +
                                    " order by z.name ASC";
                }
                else
                {
                    query = "select zoneCode, Name\n" +
                                    "  from Zones z\n" +
                                    " where z.type = '1'\n" +
                                    "   AND z.zoneCode in ('1',\n" +
                                    "'10',\n" +
                                    "'11',\n" +
                                    "'12',\n" +
                                    "'2',\n" +
                                    "'27',\n" +
                                    "'3',\n" +
                                    "'4',\n" +
                                    "'5',\n" +
                                    "'7',\n" +
                                    "'9')\n" +
                             " AND zoneCode IN (" + HttpContext.Current.Session["ZONECODE"].ToString() + ") \n" +
                                    " order by z.name ASC";
                }

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
        */

        public DataSet Get_AllZones1(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                if (HttpContext.Current.Session["ZONECODE"].ToString() == "ALL")
                {
                    query = "select zoneCode, Name\n" +
                                    "  from Zones z\n" +
                                    " where z.type = '1'\n" +
                                    "   AND z.zoneCode in ('1',\n" +
                                    "'10',\n" +
                                    "'11',\n" +
                                    "'12',\n" +
                                    "'2',\n" +
                                    "'27',\n" +
                                    "'3',\n" +
                                    "'4',\n" +
                                    "'5',\n" +
                                    "'7',\n" +
                                    "'9')\n" +
                                    //     " AND zoneCode IN (" + HttpContext.Current.Session["ZONECODE"].ToString() + ") \n" +
                                    " order by z.name ASC";
                }
                else
                {
                    query = "select zoneCode, Name\n" +
                                    "  from Zones z\n" +
                                    " where z.type = '1'\n" +
                                    "   AND z.zoneCode in ('1',\n" +
                                    "'10',\n" +
                                    "'11',\n" +
                                    "'12',\n" +
                                    "'2',\n" +
                                    "'27',\n" +
                                    "'3',\n" +
                                    "'4',\n" +
                                    "'5',\n" +
                                    "'7',\n" +
                                    "'9')\n" +
                             " AND zoneCode IN (" + HttpContext.Current.Session["ZONECODE"].ToString() + ") \n" +
                                    " order by z.name ASC";
                }

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
        public DataSet Get_MasterAllZones(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select z.zoneCode, z.Name, z.createdby, z.modifiedon, z.createdon, z.modifiedby, z.colorid from Zones z where \n" + clvar._status + "order by z.name ASC";

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

        public DataSet Get_MasterAllBranches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                //     string query = "select z.zoneCode, z.Name from Zones z where \n" + clvar._status + "order by z.name ASC";
                string query = " select b.branchCode,b.name branchname , b.zoneCode, b.name, b.createdon, b.createdby, b.modifiedon, b.modifiedby \n" +
                                "from branches b, Zones z\n" +
                                " where b.zoneCode = z.zoneCode \n" +
                                 clvar._status + clvar._Zone +
                                "order by b.name";

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

        public DataSet Get_MasterAllExpressCenter(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "  select ec.name,ec.expressCenterCode, b.branchCode,b.name branchname, ec.description, ec.createdon, ec.createdby, ec.modifiedon, ec.modifiedby \n" +
                                "from ExpressCenters ec, branches b \n" +
                                "where ec.bid = b.branchCode \n" +
                                //   "AND b.branchCode = z.zoneCode\n" +
                                //   "AND ec.status = '1'   \n" +
                                clvar._status + clvar._TownCode +
                                "order by ec.name ";

                //  clvar._status + clvar._Zone +


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









        public DataSet Get_MasterAllServiceTypes(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select s.serviceTypeName from ServiceTypes s where status ='1' " + clvar._status;

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

        public DataSet Get_MasterClientTariff_AccountInfo(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select z.name Zone,\n" +
                "       b.name Branch,\n" +
                "       c.accountNo 'Account No',\n" +
                "       z.name + '-' + b.sname + '-' + c.accountNo 'Client Account No',\n" +
                "       c.name 'Client Name',\n" +
                "       case\n" +
                "         when c.isActive = '1' then\n" +
                "          'Active'\n" +
                "         else\n" +
                "          'In Active'\n" +
                "       end 'Client Status',\n" +
                "       c.StatusCode,\n" +
                "       t.ServiceID 'Service', \n" +
                "       CONVERT(VARCHAR(10), c.createdon, 105) 'Client Created On'\n" +
                "  from tempClientTariff t, CreditClients c, Branches b, Zones z, Zones zz\n" +
                " where t.Client_Id = c.id\n" +
                "   and c.BranchCode = b.branchCode\n" +
                "   and t.FromZoneCode = z.zoneCode\n" +
                "   and t.ToZoneCode = zz.zoneCode\n" +
                "   and t.isIntlTariff = '0'\n" +
                //"   AND t.BranchCode = '4'\n" +
                //"   AND t.chkDeleted = '0'\n" +
                //"   AND c.accountNo = '0'\n" +
                //"   AND t.ServiceID = 'overnight'\n" +
                clvar._TownCode + clvar._status + clvar._ACNumber + clvar._Services + "\n" +
                " group by z.name,\n" +
                "          b.name,\n" +
                "          c.accountNo,\n" +
                "          z.name + '-' + b.sname + '-' + c.accountNo,\n" +
                "          c.name,\n" +
                "          c.StatusCode,\n" +
                "          t.ServiceID, CONVERT(VARCHAR(10), c.createdon, 105),\n" +
                "          case\n" +
                "            when c.isActive = '1' then\n" +
                "             'Active'\n" +
                "            else\n" +
                "             'In Active'\n" +
                "          end";

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

        public DataSet Get_MasterClientTariff_AccountInfo_RoadRail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {

                string sqlString = "\n" +
                "select z.name Zone,\n" +
                "       b.sname Branch,\n" +
                "       c.accountNo 'Account No',\n" +
                "       z.name + '-' + b.sname + '-' + c.accountNo 'Client Account No',\n" +
                "       c.name 'Client Name',\n" +
                "       case\n" +
                "         when c.isActive = '1' then\n" +
                "          'Active'\n" +
                "         else\n" +
                "          'In Active'\n" +
                "       end 'Client Status',\n" +
                "       c.StatusCode,\n" +
                "       convert(varchar(10),c.createdon,105) 'Client Created On'\n" +
                "  from RnR_Tarrif     t,\n" +
                "       CreditClients  c,\n" +
                "       Branches       b,\n" +
                "       Zones          z,\n" +
                "       RnR_Categories r,\n" +
                "       RnR_Categories rr\n" +
                " where t.Client_Id = c.id\n" +
                "   and c.branchCode = b.branchCode\n" +
                "   and c.zoneCode = z.zoneCode\n" +
                "   and t.FromCatId = r.Id\n" +
                "   and t.ToCatId = rr.Id\n" +
                //    "   and c.branchCode = '73'\n" +
                //    "   and c.accountNo = '0'\n" +
                clvar._TownCode + clvar._ACNumber + "\n" +
                " group by z.name,\n" +
                "          b.sname,\n" +
                "          c.accountNo,\n" +
                "          z.name + '-' + b.sname + '-' + c.accountNo,\n" +
                "          c.name,\n" +
                "          case\n" +
                "            when c.isActive = '1' then\n" +
                "             'Active'\n" +
                "            else\n" +
                "             'In Active'\n" +
                "          end,\n" +
                "          c.StatusCode,\n" +
                "          c.createdon";




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

        public DataSet Get_MasterAllClientTariff_Domestic(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "   select \n" +
                                    "       CONVERT(VARCHAR(10), t.createdOn, 105) 'Tariff Created On',\n" +
                                    "       CONVERT(VARCHAR(10), t.modifiedOn, 105) 'Tariff Modified On',\n" +
                                    "       zz.name 'Dest Zone',\n" +
                                    "       t.FromWeight 'From Weight',\n" +
                                    "       t.ToWeight 'To Weight',\n" +
                                    "       t.Price,\n" +
                                    "       t.additionalFactor 'Additional Factor'\n" +
                                    "  from tempClientTariff t, CreditClients c, Branches b, Zones z, Zones zz\n" +
                                    " where t.Client_Id = c.id\n" +
                                    "   and c.BranchCode = b.branchCode\n" +
                                    "   and t.FromZoneCode = z.zoneCode\n" +
                                    "   and t.ToZoneCode = zz.zoneCode\n" +
                                    "   and t.isIntlTariff = '0'\n" +
                                    "   and c.accountNo = '0'\n" +
                                    clvar._TownCode + clvar._status + clvar._Services + "\n";

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

        public DataSet Get_MasterAllClientTariff_International(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select \n" +
                "       CONVERT(VARCHAR(10), t.createdOn, 105) 'Tariff Created On',\n" +
                "       CONVERT(VARCHAR(10), t.modifiedOn, 105) 'Tariff Modified On',\n" +
                "       t.ServiceID 'Service',\n" +
                "       z.name 'From Zone',\n" +
                "       zz.name 'Dest Zone',\n" +
                "       cu.Name,\n" +
                "       t.FromWeight 'From Weight',\n" +
                "       t.ToWeight 'To Weight',\n" +
                "       t.Price,\n" +
                "       t.additionalFactor 'Additional Factor',\n" +
                "       cc.name            Currency_Name,\n" +
                "       cc.symbol          Currency_Symbol\n" +
                "  from tempClientTariff t,\n" +
                "       CreditClients    c,\n" +
                "       Branches         b,\n" +
                "       Zones            z,\n" +
                "       Zones            zz,\n" +
                "       Currencies       cc,\n" +
                "       IntlZoneCountry  iz,\n" +
                "       Country          cu\n" +
                " where t.Client_Id = c.id\n" +
                "   and c.BranchCode = b.branchCode\n" +
                "   and t.FromZoneCode = z.zoneCode\n" +
                "   and t.ToZoneCode = zz.zoneCode\n" +
                "   and t.currencyCodeId = cc.Id\n" +
                "   and t.ToZoneCode = iz.ZoneCode\n" +
                "   and t.ServiceID = iz.ServiceTypeId\n" +
                "   and iz.CountryCode = cu.Code\n" +
                "   and t.isIntlTariff = '0'\n" +
                //   "   and t.ServiceID = 'International_Doc'\n" +
                //   "   and t.BranchCode = '4'\n" +
                //   "   and c.accountNo = '0'\n" +
                //   "   and t.chkDeleted = '0'\n" +
                clvar._TownCode + clvar._status + clvar._Services + clvar._ACNumber + "\n" +
                " order by 1, 2, 3, 7";

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

        public DataSet Get_MasterAllClientTariff_RoadRail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select r.label 'From Zone',\n" +
                "       rr.label 'To Zone',\n" +
                "       (t.value * 5) 'Rate 0 to 5Kg',\n" +
                "       t.value 'AddKG Rate'\n" +
                "  from RnR_Tarrif     t,\n" +
                "       CreditClients  c,\n" +
                "       Branches       b,\n" +
                "       Zones          z,\n" +
                "       RnR_Categories r,\n" +
                "       RnR_Categories rr\n" +
                " where t.Client_Id = c.id\n" +
                "   and c.branchCode = b.branchCode\n" +
                "   and c.zoneCode = z.zoneCode\n" +
                "   and t.FromCatId = r.Id\n" +
                "   and t.ToCatId = rr.Id\n" +
                //   "   and c.branchCode = '4'\n" +
                //    "   and c.accountNo = '0'"+


                clvar._TownCode + clvar._ACNumber;// +clvar._Services + "\n";

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



        public DataSet Get_Branches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    query = "select NAME, branchCode from Branches where status = '1' ORDER BY NAME";
                }
                else
                {
                    //  string query = "select NAME, branchCode from Branches where status = '1' ORDER BY NAME";
                    query = "select NAME, branchCode from Branches where status = '1' \n" +
                                    " AND branchCode IN (" + HttpContext.Current.Session["BRANCHCODE"].ToString() + ")  ORDER BY NAME";
                }


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

        public DataSet Get_Zones(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "Select b.bookingDate, \n" +
                               "b.Zone,\n" +
                               "b.zoneCode,\n" +
                               "SUM(Approved) Approved,\n" +
                               "SUM(UnApproved) UnApproved\n" +
                               "from (Select b.bookingDate,\n" +
                               "b.zoneCode,\n" +
                               "b.Zone,\n" +
                               "b.Status_,\n" +
                               "Case\n" +
                               "when b.Status_ = 'Approved' then\n" +
                               "SUM(Con)\n" +
                               "else\n" +
                               "0\n" +
                               "end Approved,\n" +
                               "Case\n" +
                               "when b.Status_ = 'UnApproved' then\n" +
                               "SUM(Con)\n" +
                               "else\n" +
                               "0\n" +
                               "end UnApproved\n" +
                               "from (select convert(nvarchar(10), bookingDate, 105) bookingDate,\n" +
                               " c.zoneCode,\n" +
                               "Zone,\n" +
                               " COUNT(c.couponNumber) Con,\n" +
                               "'Approved' Status_\n" +
                               " FROM Consignment_Mar_2016 c,\n" +
                               "(select za.zoneCode, z.Name Zone\n" +
                               " from Zones z, ZoneCityAssociation za, Cities c1\n" +
                               " where z.zoneCode = za.zoneCode\n" +
                               " group by za.zoneCode, z.Name) b\n" +
                               "where b.zoneCode = c.zoneCode\n" +
                               "and c.isApproved = '1'\n" +
                               " group by convert(nvarchar(10), bookingDate, 105),\n" +
                               "c.zonecode,\n" +
                               " Zone\n" +
                               "union all\n" +
                               "select convert(nvarchar(10), bookingDate, 105) bookingDate,\n" +
                               "c.zoneCode,\n" +
                               "Zone,\n" +
                               "COUNT(c.couponNumber),\n" +
                               " 'UnApproved' Status\n" +
                               "FROM Consignment_Mar_2016 c,\n" +
                               " (select za.zoneCode, z.Name Zone\n" +
                               "from Zones z, ZoneCityAssociation za, Cities c1\n" +
                               " where z.zoneCode = za.zoneCode\n" +
                               "group by za.zoneCode, z.Name) b\n" +
                               " where b.zoneCode = c.zoneCode\n" +
                               "and c.isApproved = '0'\n" +
                               " group by convert(nvarchar(10), bookingDate, 105),\n" +
                               " c.zonecode,\n" +
                               "Zone) b\n" +
                               " group by b.bookingDate, b.Zone, b.Status_, b.zoneCode) b\n" +
                               " where " + clvar._Zone + "\n" +
                               "group by b.bookingDate, b.Zone, b.zoneCode \n" +
                               "order by b.bookingDate ASC";

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

        public DataSet Get_AllBranches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "Select b.bookingDate, \n" +
                                "b.branchCode,\n" +
                                "b1.name BranchName,\n" +
                                "SUM(Approved) Approved,\n" +
                                "SUM(UnApproved) UnApproved\n" +
                                "from (Select b.bookingDate,\n" +
                                "b.zoneCode,\n" +
                                "b.Zone,\n" +
                                "b.Status_,\n" +
                                "branchCode,\n" +
                                "Case\n" +
                                "when b.Status_ = 'Approved' then\n" +
                                "SUM(Con)\n" +
                                "else\n" +
                                "0\n" +
                                "end Approved,\n" +
                                "Case\n" +
                                "when b.Status_ = 'UnApproved' then\n" +
                                "SUM(Con)\n" +
                                "else\n" +
                                " 0\n" +
                                "end UnApproved\n" +
                                "from (select convert(nvarchar(10), bookingDate, 105) bookingDate,\n" +
                                "c.zoneCode,\n" +
                                "c.branchCode,\n" +
                                " Zone,\n" +
                                "COUNT(c.couponNumber) Con,\n" +
                                " 'Approved' Status_\n" +
                                "FROM Consignment c,\n" +
                                "(select za.zoneCode, z.Name Zone\n" +
                                " from Zones z, ZoneCityAssociation za, Cities c1\n" +
                                " where z.zoneCode = za.zoneCode\n" +
                                " group by za.zoneCode, z.Name) b\n" +
                                "where b.zoneCode = c.zoneCode\n" +
                                "and c.isApproved = '1'\n" +
                                " group by convert(nvarchar(10), bookingDate, 105),\n" +
                                "c.zonecode,\n" +
                                " c.branchCode,\n" +
                                "Zone\n" +
                                "union all\n" +
                                "select convert(nvarchar(10), bookingDate, 105) bookingDate,\n" +
                                "c.zoneCode,\n" +
                                "c.branchCode,\n" +
                                " Zone,\n" +
                                "COUNT(c.couponNumber),\n" +
                                " 'UnApproved' Status\n" +
                                " FROM Consignment c,\n" +
                                " (select za.zoneCode, z.Name Zone\n" +
                                "from Zones z, ZoneCityAssociation za, Cities c1\n" +
                                "where z.zoneCode = za.zoneCode\n" +
                                "group by za.zoneCode, z.Name) b\n" +
                                "where b.zoneCode = c.zoneCode\n" +
                                " and c.isApproved = '0'\n" +
                                " group by convert(nvarchar(10), bookingDate, 105),\n" +
                                "c.zonecode,\n" +
                                "c.branchCode,\n" +
                                "Zone) b\n" +
                                "group by b.bookingDate, b.Zone, b.Status_, b.zoneCode, branchCode) b,\n" +
                                "Branches b1\n" +
                                " where " + clvar._Zone + clvar._Year + "\n" +
                                "and b1.branchCode = b.branchCode\n" +
                                "group by b.bookingDate, b.branchCode,b1.name\n" +
                                " order by b.bookingDate ASC ";

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

        public DataSet Get_ZonebyBranches(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select branchCode, name from Branches where status = '1'  \n" +
                               " AND zoneCode = '" + clvar._Zone + "' \n" +
                               "order by name";

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

        public DataSet Get_ZonebyBranches1(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                //  string query = "select branchCode, name from Branches where status = '1' and  zoneCode = '" + clvar._Zone + "' order by name";

                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    query = "select branchCode, name from Branches where status = '1' and zoneCode = '" + clvar._Zone + "' order by name";
                }
                else
                {
                    query = "select branchCode, name from Branches where status = '1' and zoneCode = '" + clvar._Zone + "' \n " +
                            " AND BRANCHCODE IN (" + HttpContext.Current.Session["BRANCHCODE"].ToString() + ") \n" +
                            " order by name";
                }

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

        public DataSet Get_ZonebyBranches2(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                if (HttpContext.Current.Session["ZONECODE"].ToString() == "ALL")
                {
                    query = "select branchCode, name from Branches where status = '1' and  zoneCode IN (" + clvar._Zone + ") order by name";
                }
                // else
                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    // string query = "select branchCode, name from Branches where status = '1' and  zoneCode IN (" + clvar._Zone + ") order by name";
                    query = "select branchCode, name from Branches where status = '1' \n" +
                                    " AND zoneCode IN (" + clvar._Zone + ")  order by name";
                }
                else
                {
                    query = "select branchCode, name from Branches where status = '1' \n" +
                                                    " AND zoneCode IN (" + clvar._Zone + ")  AND BRANCHCODE IN (" + HttpContext.Current.Session["BRANCHCODE"].ToString() + ") order by name";
                }


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

        public DataSet Get_ZonebyBranches3(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                query = "select branchCode, name from Branches where status = '1' and  zoneCode IN (" + clvar._Zone + ") order by name";

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

        public DataSet Get_ZonebyBranches4(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            string query;
            try
            {
                if (HttpContext.Current.Session["BRANCHCODE"].ToString() == "ALL")
                {
                    // string query = "select branchCode, name from Branches where status = '1' and  zoneCode IN (" + clvar._Zone + ") order by name";
                    query = "select branchCode, name from Branches where status = '1' order by name";
                }
                else
                {
                    query = "select branchCode, name from Branches where status = '1' \n" +
                                                    " AND BRANCHCODE IN (" + HttpContext.Current.Session["BRANCHCODE"].ToString() + ") order by name";
                }


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

        public DataSet Get_Riders(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select expressCenterId, firstName+' '+lastName ridername from Riders where status = '1' AND expressCenterId IN ('" + clvar._Expresscentercode + "')";

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

        public DataSet Get_PaymentSource(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select * FROM PaymentSource order by name";

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

        public DataSet Get_ExpressCenter(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select expressCenterCode, name from ExpressCenters where status = '1' AND BID IN (" + clvar._brand + ")";
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

        public DataSet Get_Consignment(Variable clvar)
        {
            DataSet ds = new DataSet();
            string query = "SELECT \n" +
                            "c.consignmentNumber, b.name branchname, c.consigner customername, b2.name Destination, b3.name CustomerBranch, \n" +
                            "convert(nvarchar(10),bookingDate,105) bookingDate, '' pickuprider, \n" +
                            "c.weight, \n" +
                            "c.totalamount, c.gst, r.firstName + ' ' + r.lastName AS RiderName, deliveryStatus ,case when c.isApproved = '1' then 'Approved' else 'Not Approved' end Approval \n" +
                            "FROM Consignment c, Branches b, Branches b2, Branches b3, Riders r\n" +
                            "where c.orgin = b.branchCode \n" +
                            "AND c.destination = b2.branchCode \n" +
                            "AND c.branchCode = b3.branchCode \n" +
                            "AND r.ridercode = r.ridercode \n" +
                            clvar._Zone + clvar._Year + "\n";
            try
            {
                SqlConnection oc = new SqlConnection(clvar.Strcon());
                oc.Open();
                SqlCommand cmd = new SqlCommand(query, oc);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(query, oc);
                oda.Fill(ds);
                oc.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;


        }

        public DataSet Get_Consignment_Summary(Variable clvar)
        {
            DataSet ds = new DataSet();

            string query = "SELECT convert(nvarchar(10), bookingDate, 105) bookingDate,\n" +
                            "       SUM(c.chargedAmount) chargedAmount\n" +
                            "  FROM Consignment c\n" +
                            " INNER JOIN Riders r\n" +
                            "    ON c.riderCode = r.riderCode\n" +
                            "   AND c.orgin = r.branchId\n" +
                            " INNER JOIN ExpressCenters ec\n" +
                            "    ON r.expressCenterId = ec.expressCenterCode\n" +
                            " WHERE YEAR(c.bookingDate) = '2016'\n" +
                            "   AND MONTH(c.bookingDate) = '03'\n" +
                            "   AND c.consignerAccountNo = '0'\n" +
                            " group by convert(nvarchar(10), c.bookingDate, 105)\n" +
                            " order by bookingDate";
            try
            {
                SqlConnection oc = new SqlConnection(clvar.Strcon());
                oc.Open();
                SqlCommand cmd = new SqlCommand(query, oc);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(query, oc);
                oda.Fill(ds);
                oc.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public DataSet Get_CODDailyReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "SELECT \n" +
                                "c.consignmentNumber, 'COD' CODIndicator, \n" +
                                "'' OrignExpressCenter, b3.name OrignBranch, e.name destinationExpressCenterCode,\n" +
                                "b2.name DestinationBranch,\n" +
                                "c.consignee CustomerName, \n" +
                                "'' CustomerAccount,\n" +
                                "convert(nvarchar(10), c.bookingDate, 105) bookingDate,\n" +
                                "'' AccountInclTax,\n" +
                                "c.chargedAmount,\n" +
                                "'' Status\n" +
                                "FROM \n" +
                                "[APL_BTS].[dbo].[Consignment] c inner join \n" +
                                "[APL_BTS].[dbo].[CODConsignmentDetail] d\n" +
                                "on\n" +
                                "c.consignmentNumber = d.consignmentNumber,\n" +
                                "[APL_BTS].[dbo].[ExpressCenters] e,\n" +
                                "[APL_BTS].[dbo].Branches b,\n" +
                                "[APL_BTS].[dbo].Branches b2,\n" +
                                "[APL_BTS].[dbo].Branches b3\n" +
                                "where \n" +
                                "c.destinationExpressCenterCode = e.expressCenterCode\n" +
                                "and\n" +
                                "c.branchCode = b.branchCode\n" +
                                "and \n" +
                                "c.destination = b2.branchCode\n" +
                                "and \n" +
                                "c.orgin = b3.branchCode\n" +
                                "and \n" +
                                "convert(nvarchar(10), c.bookingDate, 105) = '" + clvar._Year + "'\n" +
                                "and \n" +
                                "c.zoneCode = '" + clvar._Zone + "'\n" +
                                "and \n" +
                                "c.branchCode = '" + clvar._TownCode + "'\n" +
                                "\n";

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

        public DataSet Get_ConsignmentInvoiceDiscrepancyReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "SELECT \n" +
                                "c.consignmentNumber, c.consigner,c.creditClientId,'' OrignExpressCenter, b3.name OrignBranch, \n" +
                                "b2.name DestinationBranch, convert(nvarchar(10), c.bookingDate, 105), c.serviceTypeName, c.weight, \n" +
                                "c.chargedAmount,\n" +
                                "case when c.isApproved = '1' then 'Approved' else 'UnApproved' end Approval\n" +
                                "FROM \n" +
                                "[APL_BTS].[dbo].Consignment c, ( \n" +
                                    "SELECT consignmentNumber, h.invoiceNumber\n" +
                                    "FROM [APL_BTS].[dbo].InvoiceConsignment h \n" +
                                    "where YEAR(createdOn) = '" + clvar._Year + "'\n" +
                                    "AND MONTH(createdOn) = '" + clvar._Month + "'\n" +
                                    ") i,\n" +
                                "[APL_BTS].[dbo].Branches b,\n" +
                                "[APL_BTS].[dbo].Branches b2,\n" +
                                "[APL_BTS].[dbo].Branches b3\n" +
                                "where i.consignmentNumber = c.consignmentNumber\n" +
                                "and\n" +
                                "c.branchCode = b.branchCode\n" +
                                "and \n" +
                                "c.destination = b2.branchCode\n" +
                                "and \n" +
                                "c.orgin = b3.branchCode\n" +
                                "and\n" +
                                "c.branchCode = '" + clvar._TownCode + "'\n" +
                                "group by \n" +
                                "c.consignmentNumber, c.consigner,c.creditClientId, b3.name , \n" +
                                "b2.name ,c.bookingDate,c.serviceTypeName,c.chargedAmount,c.weight,c.isApproved\n" +
                                "order by 1 ";

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

        public DataSet Get_InvoiceSummaryMonthly(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select\n" +
                "'IN-'+ic.invoiceNumber AS invoiceNumber,\n" +
                "cc.id,\n" +
                "CONVERT(NVARCHAR(20), cc.createdon, 105) createdon,\n" +
                "cc.createdby,\n" +
                "z.name Zone,\n" +
                "bb.name Branch, cc.accountNo,\n" +
                "z.name+'-'+bb.sname+'-'+cc.accountNo 'Client Account',\n" +
                "cc.name 'Client Name',\n" +
                "cg.name  'Client Parant/Group',\n" +
                "cg.description 'Parant Code',\n" +
                "cc.salesTaxNo 'Sales Tax No',\n" +
                "cc.memo 'Memo',\n" +
                "cc.ntnNo 'NTN No',\n" +
                "CONVERT(NVARCHAR(20), cc.regDate, 105) AS 'Reg Date',\n" +
                "CONVERT(NVARCHAR(20), cc.regEndDate, 105) AS 'RegEnd Date',\n" +
                "cc.domesticAmount'Domestic Committed Revenue',\n" +
                "cc.internationalAmount'International Committed Revenue',\n" +
                "datename(MM,i.startDate)+'-'+datename(YY,i.startDate) 'Invoice Month',\n" +
                "i.TotalAmount 'Invoice Amount',\n" +
                "round(sum (case When serviceTypeName in ('Flyer','NTS','HEC','Bank to Bank','Bulk Shipment','overnight','Return Service','Same Day','Second Day','Smart Box','Sunday & Holiday','Smart Box','Hand Carry','Smart Cargo','MB10','MB2','MB20','MB30','MB5','Aviation Sale') then c.totalAmount else 0 end ),2) as 'Domestic Sale',\n" +
                "round(sum (case when cc.discountOnDomestic is null then 0 when serviceTypeName in ('Flyer','NTS','HEC','Bank to Bank','Bulk Shipment','overnight','Return Service','Same Day','Second Day','Smart Box','Sunday & Holiday','Smart Box','Hand Carry','Smart Cargo','MB10','MB2','MB20','MB30','MB5','Aviation Sale') then c.totalAmount*(cc.discountOnDomestic/100) else 0 end ),2) as 'Domestic Discount',\n" +
                "sum (case When serviceTypeName in ('Flyer','NTS','HEC','Bank to Bank','Bulk Shipment','overnight','Return Service','Same Day','Second Day','Smart Box','Sunday & Holiday','Smart Box','Hand Carry','Smart Cargo','MB10','MB2','MB20','MB30','MB5','Aviation Sale') then cm.calculatedValue else 0 end)'Domestic Extra Charges',\n" +
                "'' 'Domestic Net Sale',\n" +
                "case when F1.[Fuel Surchege] is null then '' else F1.[Fuel Surchege] end 'Domestic Fuel Surchege',\n" +
                "round(sum (case When serviceTypeName = 'Road n Rail' then c.totalAmount else 0 end ),2) as 'Road n Rail Sale',\n" +
                "round(sum (case when cc.discountOnDomestic is null then 0 when serviceTypeName = 'Road n Rail' then c.totalAmount*(cc.discountOnDomestic/100) else 0 end ),2) as 'Road n Rail Discount',\n" +
                "sum (case When serviceTypeName = 'Road n Rail' then cm.calculatedValue else 0 end)'Road n Rail Extra Charges',\n" +
                "'' 'Road n Rail Net Sale',\n" +
                "case when F2.[Fuel Surchege] is null then '' else F2.[Fuel Surchege] end 'Road n Rail Fuel Surchege',\n" +
                "sum (case When c.serviceTypeName in ('Expressions','International Expressions','Mango','Mango Petty') then c.totalAmount  else 0 end ) as 'Expression Sale',\n" +
                "sum (case When c.consignmentScreen IN (2,8) and c.serviceTypeName in ('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014') then c.totalAmount  else 0 end ) as 'Sample Sale',\n" +
                "sum (case When c.consignmentScreen IN (2,8) and c.serviceTypeName in ('International_Doc_Special_Hub','International_Doc') then c.totalAmount else 0  end ) as 'Document Sale',\n" +
                "round(sum (case when cc.DiscountOnSample is null then 0  When c.consignmentScreen IN (2,8) and c.serviceTypeName in ('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014') then c.totalAmount*(cc.DiscountOnSample/100) else 0 end ),2) as 'Sample Discount',\n" +
                "round(sum (case when cc.DiscountOnDocument is null then 0  When c.consignmentScreen IN (2,8) and c.serviceTypeName in ('International_Doc_Special_Hub','International_Doc')then c.totalAmount*(cc.DiscountOnDocument/100) else 0 end ),2) as 'Document Discount',\n" +
                "sum (case When serviceTypeName in('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014','International_Doc_Special_Hub','International_Doc') then cm.calculatedValue else 0 end)'International Extra Charges',\n" +
                "'' 'International Net Sale',\n" +
                "case when F3.[Fuel Surchege] is null then '' else F3.[Fuel Surchege] end 'International Fuel Surchege',\n" +
                "round(sum (case when cc.DiscountOnDomestic is null and serviceTypeName in ('Flyer','NTS','HEC','Bank to Bank','Bulk Shipment','overnight','Return Service','Same Day','Second Day','Smart Box','Sunday & Holiday','Smart Box','Hand Carry','Smart Cargo','MB10','MB2','MB20','MB30','MB5','Aviation Sale') then c.gst When cc.DiscountOnDomestic is not null and serviceTypeName in ('Flyer','NTS','HEC','Bank to Bank','Bulk Shipment','overnight','Return Service','Same Day','Second Day','Smart Box','Sunday & Holiday','Smart Box','Hand Carry','Smart Cargo','MB10','MB2','MB20','MB30','MB5','Aviation Sale') then c.gst-c.gst*(cc.DiscountOnDomestic/100) else 0  end ),2) as 'Domestic GST',\n" +
                "case when F1.[Fuel Surchege GST] is null then '' else F1.[Fuel Surchege GST] end 'Domestic Fuel Surchege GST',\n" +
                "'' 'Domestic GST',\n" +
                "round(sum (case when cc.DiscountOnDomestic is null and serviceTypeName = 'Road n Rail' then c.gst When cc.DiscountOnDomestic is not null and serviceTypeName = 'Road n Rail' then c.gst-c.gst*(cc.DiscountOnDomestic/100) else 0  end ),2) as 'Road n Rail GST',\n" +
                "case when F2.[Fuel Surchege GST] is null then '' else F2.[Fuel Surchege GST] end 'Road n Rail Fuel Surchege GST',\n" +
                "'' 'Road n Rail GST',\n" +
                "sum (case When c.serviceTypeName in ('Expressions','International Expressions','Mango','Mango Petty') then c.gst  else 0 end )  as 'Expressions GST',\n" +
                "round(sum (case when cc.discountOnSample is null and c.consignmentScreen IN (2,8) and serviceTypeName in ('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014')then c.gst  When cc.discountOnSample is not null and c.consignmentScreen IN (2,8) and serviceTypeName in ('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014') then c.gst-c.gst*(cc.discountOnSample/100)   else 0 end ),2) as 'Sample GST',\n" +
                "round(sum (case when cc.discountOnDocument is null and c.consignmentScreen IN (2,8) and serviceTypeName in ('International_Doc_Special_Hub','International_Doc')then c.gst  When cc.discountOnDocument is not null and c.consignmentScreen IN (2,8) and serviceTypeName in ('International_Doc_Special_Hub','International_Doc') then c.gst-c.gst*(cc.discountOnDocument/100)   else 0 end ),2) as 'Document GST',\n" +
                "case when F3.[Fuel Surchege GST] is null then '' else F3.[Fuel Surchege GST] end 'International Fuel Surchege GST',\n" +
                "'' 'International GST'\n" +
                "from Consignment c\n" +
                "inner join InvoiceConsignment ic on c.consignmentNumber=ic.consignmentNumber\n" +
                "inner join Invoice i on ic.invoiceNumber=i.invoiceNumber\n" +
                "inner join CreditClients cc on i.clientid=cc.id\n" +
                "left join (\n" +
                "select  i1.invoiceNumber, sum(ipm.calculatedAmount)'Fuel Surchege',\n" +
                "sum(ipm.calculatedGST)'Fuel Surchege GST'\n" +
                "from Invoice i1\n" +
                "inner join InvoicePriceModifierAssociation ipm on i1.invoiceNumber=ipm.InvoiceNo\n" +
                "where modifierType='1' and companyId='1'\n" +
                "group by i1.invoiceNumber) F1 on i.invoiceNumber=F1.invoiceNumber\n" +
                "left join (\n" +
                "select  i1.invoiceNumber, sum(ipm.calculatedAmount)'Fuel Surchege',\n" +
                "sum(ipm.calculatedGST)'Fuel Surchege GST'\n" +
                "from Invoice i1\n" +
                "inner join InvoicePriceModifierAssociation ipm on i1.invoiceNumber=ipm.InvoiceNo\n" +
                "where modifierType='1' and companyId='2'\n" +
                "group by i1.invoiceNumber) F2 on i.invoiceNumber=F2.invoiceNumber\n" +
                "left join (\n" +
                "select  i1.invoiceNumber, sum(ipm.calculatedAmount)'Fuel Surchege',\n" +
                "sum(ipm.calculatedGST)'Fuel Surchege GST'\n" +
                "from Invoice i1\n" +
                "inner join InvoicePriceModifierAssociation ipm on i1.invoiceNumber=ipm.InvoiceNo\n" +
                "where modifierType='2'\n" +
                "group by i1.invoiceNumber) F3 on i.invoiceNumber=F3.invoiceNumber\n" +
                "inner join Branches bb on cc.branchCode = bb.branchCode\n" +
                "left join Zones z on bb.zoneCode=z.zoneCode\n" +
                "left join ConsignmentModifier cm on c.consignmentNumber=cm.consignmentNumber\n" +
                "left join ClientGroups cg on cc.clientGrpId=cg.id\n" +
                "where\n" +
                "cc.accountNo !='0'\n" +
                    clvar._Year + clvar._Month + clvar._TownCode + clvar._status +
                "group by\n" +
                "'IN-'+ic.invoiceNumber ,\n" +
                "cc.id,z.name ,\n" +
                "cc.createdon,\n" +
                "cc.createdby,\n" +
                "bb.name , cc.accountNo,\n" +
                "z.name+'-'+bb.sname+'-'+cc.accountNo,\n" +
                "cc.name ,\n" +
                "cg.name ,\n" +
                "cg.description ,\n" +
                "cc.salesTaxNo,\n" +
                "cc.memo,\n" +
                "cc.ntnNo,\n" +
                "cc.regDate,\n" +
                "cc.regEndDate,\n" +
                "cc.domesticAmount,\n" +
                "cc.internationalAmount,\n" +
                "datename(MM,i.startDate)+'-'+datename(YY,i.startDate) ,\n" +
                "i.TotalAmount,\n" +
                "case when F1.[Fuel Surchege] is null then '' else F1.[Fuel Surchege] end ,\n" +
                "case when F1.[Fuel Surchege GST] is null then '' else F1.[Fuel Surchege GST] end ,\n" +
                "case when F2.[Fuel Surchege] is null then '' else F2.[Fuel Surchege] end ,\n" +
                "case when F2.[Fuel Surchege GST] is null then '' else F2.[Fuel Surchege GST] end ,\n" +
                "case when F3.[Fuel Surchege] is null then '' else F3.[Fuel Surchege] end ,\n" +
                "case when F3.[Fuel Surchege GST] is null then '' else F3.[Fuel Surchege GST] end\n" +
                "order by z.name,bb.name,cc.accountNo";

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

        public DataSet Get_InvoiceSummaryMonthly_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {

                string query = "SELECT ic.invoiceNumber,\n" +
                "       c.consigner Customer,\n" +
                "       c.consignmentNumber,\n" +
                "       SUBSTRING(CONVERT(NVARCHAR(20), c.bookingDate, 106), 4, 8) Month,\n" +
                "       'CN-' + c.consignmentNumber     ConsignmentNumber,\n" +
                "       c.consignerAccountNo,\n" +
                "       (\n" +
                "           SELECT b.name\n" +
                "           FROM   Branches b\n" +
                "           WHERE  b.branchCode = c.orgin\n" +
                "       ) BranchName,\n" +
                "       (\n" +
                "           SELECT b.name\n" +
                "           FROM   Branches b\n" +
                "           WHERE  b.branchCode = c.destination\n" +
                "       ) DestinationBranchName,\n" +
                "       c.serviceTypeName               SERVICEName,\n" +
                "       c.consignmentTypeId,\n" +
                "       ct.name                         ConsignmentType,\n" +
                "       CONVERT(NVARCHAR(20), c.bookingDate, 105),\n" +
                "       c.[weight],\n" +
                "       c.totalAmount\n" +
                "FROM   InvoiceConsignment              ic,\n" +
                "       Consignment                     c,\n" +
                "       ConsignmentType                 ct\n" +
                "WHERE  c.consignmentNumber = ic.consignmentNumber\n" +
                "       AND c.consignmentTypeId = ct.id\n" +
                "       AND ic.invoiceNumber = '" + clvar._invoiceNo + "' \n" +
                "GROUP BY\n" +
                "       ic.invoiceNumber,\n" +
                "       c.consigner,\n" +
                "       c.consignmentNumber,\n" +
                "       CONVERT(NVARCHAR(20), c.bookingDate, 106),\n" +
                "       c.consignmentNumber,\n" +
                "       c.orgin,\n" +
                "       c.consignerAccountNo,\n" +
                "       c.destination,\n" +
                "       c.serviceTypeName,\n" +
                "       c.consignmentTypeId,\n" +
                "       ct.name,\n" +
                "       CONVERT(NVARCHAR(20), c.bookingDate, 105),\n" +
                "       c.[weight],\n" +
                "       c.totalAmount";



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

        // POD Daily Status Start
        public DataSet Get_POD_DailyStatus(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select z.name zoneName,\n" +
                "       b.branchCode,\n" +
                "       br.name branchSName,\n" +
                "       b.zoneCode,\n" +
                "       convert(varchar(11), runsheetDate, 105) runsheetDate,\n" +
                "       SUM(CN_Count) CN,\n" +
                "       SUM(EnterdCN) EnterCN\n" +
                "  from (SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               COUNT(runsheetNumber) CN_Count,\n" +
                "               0 EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                "                   and runsheetDate <= '" + clvar._Year + "'\n";
                if (clvar._TownCode != "0")
                {
                    sqlString += "                AND Rs.branchCode = '" + clvar._TownCode + "'\n";
                }
                sqlString += "                   AND Rs.zoneCode = '" + clvar._Zone + "') b\n" +
                "         WHERE PODEntered IN ('0')\n" +
                "         group by branchCode, zoneCode, runsheetDate\n" +
                "        union all\n" +
                "        SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               0 CN_Count,\n" +
                "               COUNT(runsheetNumber) EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                "                   and runsheetDate <= '" + clvar._Year + "'\n";
                if (clvar._TownCode != "0")
                {
                    sqlString += "                AND Rs.branchCode = '" + clvar._TownCode + "'\n";
                }
                sqlString += "                   AND Rs.zoneCode = '" + clvar._Zone + "'" +
                "                   and rc.modifiedOn = rs.runsheetDate " +
                ") b\n" +
                "                \n" +
                "         WHERE PODEntered not IN ('0', '1')\n" +
                "         group by branchCode, zoneCode, runsheetDate) b,\n" +
                "       Branches br,\n" +
                "       Zones Z\n" +
                " where b.branchCode = br.branchCode\n" +
                "   and br.zoneCode = z.zoneCode\n" +
                " group by b.branchCode, b.zoneCode, runsheetDate, br.name, z.name\n" +
                " order by runsheetDate";

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

        public DataSet Get_POD_DailyStatus_INformation(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select br.name branchSName, z.name zoneName \n" +
                "  from (SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               COUNT(runsheetNumber) CN_Count,\n" +
                "               0 EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                "                    and runsheetDate <= '" + clvar._Year + "'\n";
                if (clvar._TownCode != "0")
                {
                    sqlString += "                AND Rs.branchCode = '" + clvar._TownCode + "'\n";
                }
                sqlString += "                   AND Rs.zoneCode = '" + clvar._Zone + "') b\n" +
                "         WHERE PODEntered IN ('0')\n" +
                "         group by branchCode, zoneCode, runsheetDate\n" +
                "        union all\n" +
                "        SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               0 CN_Count,\n" +
                "               COUNT(runsheetNumber) EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
               "                   and runsheetDate <= '" + clvar._Year + "'\n";
                if (clvar._TownCode != "0")
                {
                    sqlString += "                AND Rs.branchCode = '" + clvar._TownCode + "'\n";
                }
                sqlString += "                   AND Rs.zoneCode = '" + clvar._Zone + "'" +
                "                   and rc.modifiedOn = rs.runsheetDate " +
                ") b\n" +
                "                \n" +
                "         WHERE PODEntered not IN ('0', '1')\n" +
                "         group by branchCode, zoneCode, runsheetDate) b,\n" +
                "       Branches br,\n" +
                "       Zones Z\n" +
                " where b.branchCode = br.branchCode\n" +
                "   and br.zoneCode = z.zoneCode\n" +
                " group by  br.name, z.name\n" +
                " order by br.name, z.name";

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

        public DataSet Get_POD_DailyStatus_Date(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select CONVERT(NVARCHAR, runsheetDate, 105) runsheetDate\n" +
               "  from (SELECT branchCode,\n" +
               "               zoneCode,\n" +
               "               runsheetDate,\n" +
               "               COUNT(runsheetNumber) CN_Count,\n" +
               "               0 EnterdCN\n" +
               "          FROM (SELECT rs.runsheetNumber,\n" +
               "                       rs.branchCode,\n" +
               "                       rs.zoneCode,\n" +
               "                       rs.runsheetDate,\n" +
               "                       rs.createdOn AS runsheetSystemDate,\n" +
               "                       rs.createdBy AS runsheetCreatedBy,\n" +
               "                       rc.consignmentNumber,\n" +
               "                       rc.deliveryDate,\n" +
               "                       rc.modifiedOn AS PODDate,\n" +
               "                       rc.modifiedBy AS PODCreatedBy,\n" +
               "                       rc.Status,\n" +
               "                       rc.time AS PODTime,\n" +
               "                       CASE\n" +
               "                         WHEN rc.Status IS NULL THEN\n" +
               "                          0\n" +
               "                         WHEN rc.Status = 'TRANSFERED' then\n" +
               "                          1\n" +
               "                         WHEN rc.Status = 'DELIVERED' then\n" +
               "                          2\n" +
               "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
               "                          3\n" +
               "                         ELSE\n" +
               "                          4\n" +
               "                       END AS PODEntered,\n" +
               "                       CASE\n" +
               "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
               "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
               "                              rc.Status IS NOT NULL THEN\n" +
               "                          1\n" +
               "                         ELSE\n" +
               "                          0\n" +
               "                       END AS PODBefore2pm\n" +
               "                  FROM dbo.Runsheet AS rs\n" +
               "                 INNER JOIN RunsheetConsignment AS rc\n" +
               "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
               "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
               "                   and runsheetDate <= '" + clvar._Year + "'\n";
                if (clvar._TownCode != "0")
                {
                    sqlString += "                AND Rs.branchCode = '" + clvar._TownCode + "'\n";
                }
                sqlString += "                   AND Rs.zoneCode = '" + clvar._Zone + "'\n" +
                "                   and rc.modifiedOn = rs.runsheetDate " +
                ") b\n" +
               "         WHERE PODEntered IN ('0')\n" +
               "         group by branchCode, zoneCode, runsheetDate\n" +
               "        union all\n" +
               "        SELECT branchCode,\n" +
               "               zoneCode,\n" +
               "               runsheetDate,\n" +
               "               0 CN_Count,\n" +
               "               COUNT(runsheetNumber) EnterdCN\n" +
               "          FROM (SELECT rs.runsheetNumber,\n" +
               "                       rs.branchCode,\n" +
               "                       rs.zoneCode,\n" +
               "                       rs.runsheetDate,\n" +
               "                       rs.createdOn AS runsheetSystemDate,\n" +
               "                       rs.createdBy AS runsheetCreatedBy,\n" +
               "                       rc.consignmentNumber,\n" +
               "                       rc.deliveryDate,\n" +
               "                       rc.modifiedOn AS PODDate,\n" +
               "                       rc.modifiedBy AS PODCreatedBy,\n" +
               "                       rc.Status,\n" +
               "                       rc.time AS PODTime,\n" +
               "                       CASE\n" +
               "                         WHEN rc.Status IS NULL THEN\n" +
               "                          0\n" +
               "                         WHEN rc.Status = 'TRANSFERED' then\n" +
               "                          1\n" +
               "                         WHEN rc.Status = 'DELIVERED' then\n" +
               "                          2\n" +
               "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
               "                          3\n" +
               "                         ELSE\n" +
               "                          4\n" +
               "                       END AS PODEntered,\n" +
               "                       CASE\n" +
               "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
               "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
               "                              rc.Status IS NOT NULL THEN\n" +
               "                          1\n" +
               "                         ELSE\n" +
               "                          0\n" +
               "                       END AS PODBefore2pm\n" +
               "                  FROM dbo.Runsheet AS rs\n" +
               "                 INNER JOIN RunsheetConsignment AS rc\n" +
               "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
               "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
               "                   and runsheetDate <= '" + clvar._Year + "'\n";
                if (clvar._TownCode != "0")
                {
                    sqlString += "                AND Rs.branchCode = '" + clvar._TownCode + "'\n";
                }
                sqlString += "                   AND Rs.zoneCode = '" + clvar._Zone + "') b\n" +
               "                \n" +
               "         WHERE PODEntered not IN ('0', '1')\n" +
               "         group by branchCode, zoneCode, runsheetDate) b,\n" +
               "       Branches br,\n" +
               "       Zones Z\n" +
               " where b.branchCode = br.branchCode\n" +
               "   and br.zoneCode = z.zoneCode\n" +
               " group by  runsheetDate\n" +
               " order by runsheetDate desc";

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
        // POD Daily Status End

        // POD Same Day Start
        public DataSet Get_POD_SameStatus_INformation(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select z.name zoneName,\n" +
                "       b.branchCode,\n" +
                "       br.name branchSName,\n" +
                "       b.zoneCode\n" +
                "  from (SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               COUNT(runsheetNumber) CN_Count,\n" +
                "               0 EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
               "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                "                   and runsheetDate <= '" + clvar._Year + "'\n" +
                "                   AND Rs.branchCode = '" + clvar._TownCode + "'\n" +
                "                   AND Rs.zoneCode = '" + clvar._Zone + "') b\n" +
                "         WHERE PODEntered IN ('0')\n" +
                "         group by branchCode, zoneCode, runsheetDate\n" +
                "        union all\n" +
                "        SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               0 CN_Count,\n" +
                "               COUNT(runsheetNumber) EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                "                   and runsheetDate <= '" + clvar._Year + "'\n" +
                "                   AND Rs.branchCode = '" + clvar._TownCode + "'\n" +
                "                   AND Rs.zoneCode = '" + clvar._Zone + "'" +
                "                   and rc.modifiedOn = rs.runsheetDate " +
                ") b\n" +
                "                \n" +
                "         WHERE PODEntered not IN ('0', '1')\n" +
                "         and PODBefore2pm ='1' \n" +
                "         group by branchCode, zoneCode, runsheetDate) b,\n" +
                "       Branches br,\n" +
                "       Zones Z\n" +
                " where b.branchCode = br.branchCode\n" +
                "   and br.zoneCode = z.zoneCode\n" +
                " group by b.branchCode, b.zoneCode, br.name, z.name\n" +
                " order by b.branchCode, b.zoneCode, br.name, z.name\n";

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

        public DataSet Get_POD_SameStatus_DailyStatus(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select z.name zoneName,\n" +
                "       b.branchCode,\n" +
                "       br.name branchSName,\n" +
                "       b.zoneCode,\n" +
                "       convert(varchar(11), runsheetDate, 105) runsheetDate, \n" +
                "       SUM(CN_Count) CN,\n" +
                "       SUM(EnterdCN) EnterCN\n" +
                "  from (SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               COUNT(runsheetNumber) CN_Count,\n" +
                "               0 EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
               "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                "                   and runsheetDate <= '" + clvar._Year + "'\n" +
                "                   AND Rs.branchCode = '" + clvar._TownCode + "'\n" +
                "                   AND Rs.zoneCode = '" + clvar._Zone + "' \n" +
                "                   ) b\n" +
                "         WHERE PODEntered IN ('0')\n" +
                "         group by branchCode, zoneCode, runsheetDate\n" +
                "        union all\n" +
                "        SELECT branchCode,\n" +
                "               zoneCode,\n" +
                "               runsheetDate,\n" +
                "               0 CN_Count,\n" +
                "               COUNT(runsheetNumber) EnterdCN\n" +
                "          FROM (SELECT rs.runsheetNumber,\n" +
                "                       rs.branchCode,\n" +
                "                       rs.zoneCode,\n" +
                "                       rs.runsheetDate,\n" +
                "                       rs.createdOn AS runsheetSystemDate,\n" +
                "                       rs.createdBy AS runsheetCreatedBy,\n" +
                "                       rc.consignmentNumber,\n" +
                "                       rc.deliveryDate,\n" +
                "                       rc.modifiedOn AS PODDate,\n" +
                "                       rc.modifiedBy AS PODCreatedBy,\n" +
                "                       rc.Status,\n" +
                "                       rc.time AS PODTime,\n" +
                "                       CASE\n" +
                "                         WHEN rc.Status IS NULL THEN\n" +
                "                          0\n" +
                "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                "                          1\n" +
                "                         WHEN rc.Status = 'DELIVERED' then\n" +
                "                          2\n" +
                "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                "                          3\n" +
                "                         ELSE\n" +
                "                          4\n" +
                "                       END AS PODEntered,\n" +
                "                       CASE\n" +
                "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                "                              rc.Status IS NOT NULL THEN\n" +
                "                          1\n" +
                "                         ELSE\n" +
                "                          0\n" +
                "                       END AS PODBefore2pm\n" +
                "                  FROM dbo.Runsheet AS rs\n" +
                "                 INNER JOIN RunsheetConsignment AS rc\n" +
                "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                "                   and runsheetDate <= '" + clvar._Year + "'\n" +
                "                   AND Rs.branchCode = '" + clvar._TownCode + "'\n" +
                "                   AND Rs.zoneCode = '" + clvar._Zone + "'\n" +
                "                   and rc.modifiedOn = rs.runsheetDate " +
                ") b\n" +
                "                \n" +
                "         WHERE PODEntered not IN ('0', '1')\n" +
                "         and PODBefore2pm ='1' \n" +
                "         group by branchCode, zoneCode, runsheetDate) b,\n" +
                "       Branches br,\n" +
                "       Zones Z\n" +
                " where b.branchCode = br.branchCode\n" +
                "   and br.zoneCode = z.zoneCode\n" +
                " group by b.branchCode, b.zoneCode, runsheetDate, br.name, z.name\n" +
                " order by runsheetDate";

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

        public DataSet Get_POD_SamestatusDailyStatus_Date(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select CONVERT(NVARCHAR, runsheetDate, 105) runsheetDate\n" +
                 "  from (SELECT branchCode,\n" +
                 "               zoneCode,\n" +
                 "               runsheetDate,\n" +
                 "               COUNT(runsheetNumber) CN_Count,\n" +
                 "               0 EnterdCN\n" +
                 "          FROM (SELECT rs.runsheetNumber,\n" +
                 "                       rs.branchCode,\n" +
                 "                       rs.zoneCode,\n" +
                 "                       rs.runsheetDate,\n" +
                 "                       rs.createdOn AS runsheetSystemDate,\n" +
                 "                       rs.createdBy AS runsheetCreatedBy,\n" +
                 "                       rc.consignmentNumber,\n" +
                 "                       rc.deliveryDate,\n" +
                 "                       rc.modifiedOn AS PODDate,\n" +
                 "                       rc.modifiedBy AS PODCreatedBy,\n" +
                 "                       rc.Status,\n" +
                 "                       rc.time AS PODTime,\n" +
                 "                       CASE\n" +
                 "                         WHEN rc.Status IS NULL THEN\n" +
                 "                          0\n" +
                 "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                 "                          1\n" +
                 "                         WHEN rc.Status = 'DELIVERED' then\n" +
                 "                          2\n" +
                 "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                 "                          3\n" +
                 "                         ELSE\n" +
                 "                          4\n" +
                 "                       END AS PODEntered,\n" +
                 "                       CASE\n" +
                 "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                 "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                 "                              rc.Status IS NOT NULL THEN\n" +
                 "                          1\n" +
                 "                         ELSE\n" +
                 "                          0\n" +
                 "                       END AS PODBefore2pm\n" +
                 "                  FROM dbo.Runsheet AS rs\n" +
                 "                 INNER JOIN RunsheetConsignment AS rc\n" +
                 "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                 "                   and runsheetDate <= '" + clvar._Year + "'\n" +
                 "                   AND Rs.branchCode = '" + clvar._TownCode + "'\n" +
                 "                   AND Rs.zoneCode = '" + clvar._Zone + "') b\n" +
                 "         WHERE PODEntered IN ('0')\n" +
                 "         group by branchCode, zoneCode, runsheetDate\n" +
                 "        union all\n" +
                 "        SELECT branchCode,\n" +
                 "               zoneCode,\n" +
                 "               runsheetDate,\n" +
                 "               0 CN_Count,\n" +
                 "               COUNT(runsheetNumber) EnterdCN\n" +
                 "          FROM (SELECT rs.runsheetNumber,\n" +
                 "                       rs.branchCode,\n" +
                 "                       rs.zoneCode,\n" +
                 "                       rs.runsheetDate,\n" +
                 "                       rs.createdOn AS runsheetSystemDate,\n" +
                 "                       rs.createdBy AS runsheetCreatedBy,\n" +
                 "                       rc.consignmentNumber,\n" +
                 "                       rc.deliveryDate,\n" +
                 "                       rc.modifiedOn AS PODDate,\n" +
                 "                       rc.modifiedBy AS PODCreatedBy,\n" +
                 "                       rc.Status,\n" +
                 "                       rc.time AS PODTime,\n" +
                 "                       CASE\n" +
                 "                         WHEN rc.Status IS NULL THEN\n" +
                 "                          0\n" +
                 "                         WHEN rc.Status = 'TRANSFERED' then\n" +
                 "                          1\n" +
                 "                         WHEN rc.Status = 'DELIVERED' then\n" +
                 "                          2\n" +
                 "                         WHEN rc.Status = 'UNDELIVERED' then\n" +
                 "                          3\n" +
                 "                         ELSE\n" +
                 "                          4\n" +
                 "                       END AS PODEntered,\n" +
                 "                       CASE\n" +
                 "                         WHEN ((DATEPART(hh, rc.modifiedOn) * 100) +\n" +
                 "                              DATEPART(mm, rc.modifiedOn)) <= 1400 AND\n" +
                 "                              rc.Status IS NOT NULL THEN\n" +
                 "                          1\n" +
                 "                         ELSE\n" +
                 "                          0\n" +
                 "                       END AS PODBefore2pm\n" +
                 "                  FROM dbo.Runsheet AS rs\n" +
                 "                 INNER JOIN RunsheetConsignment AS rc\n" +
                 "                    ON rs.runsheetNumber = rc.runsheetNumber\n" +
                 "                 WHERE (rs.runsheetDate >= DATEADD(dd, -6, '" + clvar._Year + "'))\n" +
                 "                   and runsheetDate <= '" + clvar._Year + "'\n" +
                 "                   AND Rs.branchCode = '" + clvar._TownCode + "'\n" +
                 "                   AND Rs.zoneCode = '" + clvar._Zone + "' " +
                 "                   and rc.modifiedOn = rs.runsheetDate " +
                 ") b\n" +
                 "                \n" +
                 "         WHERE PODEntered not IN ('0', '1')\n" +
                 "         and PODBefore2pm ='1' \n" +
                 "         group by branchCode, zoneCode, runsheetDate) b,\n" +
                 "       Branches br,\n" +
                 "       Zones Z\n" +
                 " where b.branchCode = br.branchCode\n" +
                 "   and br.zoneCode = z.zoneCode\n" +
                 " group by runsheetDate\n" +
                 " order by runsheetDate desc";

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
        // POD Same Day End

        public DataSet Get_Credit_Consignment_Summary(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT CONVERT(NVARCHAR, bookingDate, 105) BDate, \n"
                + "       b.name                      AS BranchName, \n"
                + "       ( \n"
                + "           SELECT e.name \n"
                + "           FROM   ExpressCenters e \n"
                + "           WHERE  e.expressCenterCode = a.originExpressCenter \n"
                + "       )                              ExpressCenterName, \n"
                + "       a.originExpressCenter,a.orgin branchCode, \n"
                + "       COUNT(CASE WHEN a.isApproved = '0' THEN 1 END) + \n"
                + "       COUNT(CASE WHEN a.isApproved IS NULL THEN 1 END) AS UnApproved, \n"
                + "       COUNT(CASE WHEN a.isApproved = '1' THEN 1 END) AS \"Approved\", \n"
                + "       COUNT(a.consignmentNumber)  AS TotalConsi, \n" +
                " Max(a.chargedAmount) chargeAmount,  Max(TotalAmount)  TotalAmount   \n"
                + "FROM   Consignment a \n"
                + "       INNER JOIN Branches b \n"
                + "            ON  a.orgin = b.branchCode \n"
                + " \n" + clvar._Year + clvar._Month + clvar._TownCode + "\n"
                + "            AND consignerAccountNo != '0' \n"
                + "GROUP BY \n"
                + "       b.name, \n"
                + "       CONVERT(NVARCHAR, bookingDate, 105), \n"
                + "       a.originExpressCenter,a.orgin \n "
                + "ORDER BY \n"
                + "       b.name, BDate ASC";




                string sqlString = "SELECT CONVERT(NVARCHAR, bookingDate, 105) BDate,\n" +
                "       b.name AS BranchName, a.originExpressCenter, ec.name ExpressCenterName,\n" +
                "       b.branchCode,\n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved = '0' THEN\n" +
                "                1\n" +
                "             END) + COUNT(CASE\n" +
                "                            WHEN a.isApproved IS NULL THEN\n" +
                "                             1\n" +
                "                          END) AS UnApproved,\n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved = '1' THEN\n" +
                "                1\n" +
                "             END) AS \"Approved\",\n" +
                "       COUNT(a.consignmentNumber) AS TotalConsi,\n" +
                "       Max(a.chargedAmount) chargeAmount,\n" +
                "       Max(TotalAmount) TotalAmount\n" +
                "  FROM Consignment a\n" +
                " INNER JOIN Branches b\n" +
                "    ON a.orgin = b.branchCode\n" +
                /*
                " inner join (select *\n" +
                "               from Riders r1\n" +
                "              where r1.expressCenterId + r1.riderCode not in\n" +
                "                    ('3711108',\n" +
                "                     '0430122',\n" +
                "                     '0411234',\n" +
                "                     '2311328',\n" +
                "                     '1812-1401',\n" +
                "                     '1812-1403',\n" +
                "                     '1812-1406',\n" +
                "                     '0230-1450',\n" +
                "                     '0222-1514',\n" +
                "                     '0222-1519',\n" +
                "                     '1813-1601',\n" +
                "                     '1813-1607',\n" +
                "                     '31121007',\n" +
                "                     '11111101',\n" +
                "                     '01113001',\n" +
                "                     'EXCENTER-FRFSD00029001',\n" +
                "                     '013111714')) r\n" +
                "    on a.riderCode = r.riderCode\n" +
                "   and a.orgin = r.branchId\n" +
                 */
                "   AND consignerAccountNo != '0' INNER JOIN EXPRESSCENTERS EC on EC.ExpresscenterCode = a.originExpressCenter \n" +
                " \n" + clvar._Year + clvar._Month + clvar._TownCode + "\n" +
                " GROUP BY b.name, CONVERT(NVARCHAR, bookingDate, 105), b.branchCode, a.originExpressCenter, ec.name\n" +
                " ORDER BY b.name, BDate ASC";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
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

        public DataSet Get_Credit_Consignment_Summary_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT a.weight, a.consignmentNumber,CONVERT(NVARCHAR, bookingDate, 105) BDate,\n" +
                "       B2.NAME DESTINATIONBRANCH,\n" +
                "       b.name AS BranchName,\n" +
                "       (SELECT e.name\n" +
                "          FROM ExpressCenters e\n" +
                "         WHERE e.expressCenterCode = a.originExpressCenter) ExpressCenterName,\n" +

                "       (Select ISNULL(Max(cm.calculatedValue),0) from ConsignmentModifier cm \n" +
                "           where cm.consignmentNumber = a.consignmentNumber) FuelCharge,\n" +
                "       (Select ISNULL(Max(cm.calculatedGST),0) from ConsignmentModifier cm \n" +
                "           where cm.consignmentNumber = a.consignmentNumber) FuelGST, \n" +

                "       a.originExpressCenter,\n" +
                "       a.orgin branchCode,\n" +
                "       a.consignerAccountNo,\n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved = '0' THEN\n" +
                "                1\n" +
                "             END) + COUNT(CASE\n" +
                "                            WHEN a.isApproved IS NULL THEN\n" +
                "                             1\n" +
                "                          END) AS UnApproved,\n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved = '1' THEN\n" +
                "                1\n" +
                "             END) AS \"Approved\",\n" +
                "       COUNT(a.consignmentNumber) AS TotalConsi,\n" +
                "       Consigner CustomerName,\n" +
                "       ServiceTypeName Service,\n" +
                "       CreditClientID ClientID,\n" +
                "       (Select Name from ConsignmentType where id = Consignmenttypeid) Consignmenttype,\n" +
                "       ISNULL(Max(a.chargedAmount), 0) chargeAmount,\n" +
                "       ISNULL(Max(TotalAmount),0) TotalAmount\n" +
                //  "       r.firstName + ' ' + r.lastName RiderName\n" +
                "  FROM Consignment a\n" +
                " INNER JOIN Branches b\n" +
                "    ON a.orgin = b.branchCode\n" +
                " INNER JOIN BRANCHES B2\n" +
                "    ON A.DESTINATION = B2.BRANCHCODE\n" +
                //   " INNER JOIN Riders r\n" +
                //   "    ON a.riderCode = r.riderCode\n" +
                "   AND CONVERT(NVARCHAR, bookingDate, 105) = '" + clvar._FromMonth + "' \n" +
                "   AND b.branchCode IN (" + clvar._Zone + ")\n" +
                "   AND consignerAccountNo != '0'\n" +
                //    "   and a.originExpressCenter = '" + clvar._salegroup + "' \n" +
                "   and a.isApproved = '" + clvar._status + "' \n" +

                " GROUP BY a.consignmentNumber,\n" +
                "          b.name,\n" +
                "          a.consignerAccountNo,\n" +
                "          CONVERT(NVARCHAR, bookingDate, 105),\n" +
                "          a.originExpressCenter,\n" +
                "          a.orgin,\n" +
                "          Consigner,\n" +
                "          ServiceTypeName,\n" +
                "          CreditClientID,\n" +
                "          Consignmenttypeid,\n" +
                "          a.weight,\n" +
                "          B2.NAME\n" +
                //    "          r.firstName + ' ' + r.lastName\n" +
                " ORDER BY b.name, BDate ASC";



                /*

                string sqlString = "SELECT a.consignmentNumber,CONVERT(NVARCHAR, bookingDate, 105) BDate, B2.NAME DESTINATIONBRANCH,\n"
                + "       b.name                      AS BranchName, \n"
                + "       ( \n"
                + "           SELECT e.name \n"
                + "           FROM   ExpressCenters e \n"
                + "           WHERE  e.expressCenterCode = a.originExpressCenter \n"
                + "       )                              ExpressCenterName, \n"
                + "       a.originExpressCenter,a.orgin branchCode, a.consignerAccountNo,\n"
                + "       COUNT(CASE WHEN a.isApproved = '0' THEN 1 END) + \n"
                + "       COUNT(CASE WHEN a.isApproved IS NULL THEN 1 END) AS UnApproved, \n"
                + "       COUNT(CASE WHEN a.isApproved = '1' THEN 1 END) AS \"Approved\", \n"
                + "       COUNT(a.consignmentNumber)  AS TotalConsi, Consigner CustomerName, ServiceTypeName Service, CreditClientID ClientID, \n"
                + "        (Select Name from ConsignmentType where id = Consignmenttypeid) Consignmenttype, \n" 
                +" ISNULL(Max(a.chargedAmount),0) chargeAmount,  Max(TotalAmount)  TotalAmount ,  \n"
                + "       a.weight, r.firstName +' '+r.lastName RiderName \n"
                + "FROM   Consignment a \n"
                + "       INNER JOIN Branches b \n"
                + "            ON  a.orgin = b.branchCode \n"
                + "       INNER JOIN Riders r\n"
                + "       ON a.riderCode = r.riderCode \n"
                + "       INNER JOIN BRANCHES B2 ON A.DESTINATION = B2.BRANCHCODE  "
                + "            and convert(nvarchar, a.bookingDate, 105) = '" + clvar._FromMonth + "'\n"
                + "             AND consignerAccountNo != '0' \n"
                + "            and b.branchCode = '" + clvar._Zone + "'\n"
                + "            and   a.isApproved = '" + clvar._status + "'\n";
                if (clvar._salegroup != string.Empty)
                {
                    sqlString += "   and a.originExpressCenter = '" + clvar._salegroup + "'\n";
                }
                else
                {
                    sqlString += "   and a.originExpressCenter is null \n";
                }
                sqlString += "GROUP BY \n"
                + "       a.consignmentNumber, b.name, a.consignerAccountNo,\n"
                + "       CONVERT(NVARCHAR, bookingDate, 105), \n"
                + "       a.originExpressCenter,a.orgin, Consigner , ServiceTypeName , CreditClientID ,  Consignmenttypeid, a.weight, r.firstName +' '+r.lastName, B2.NAME \n"
                + "ORDER BY \n"
                + "       b.name, BDate ASC";
                */

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

        public DataSet Get_Cash_Consignment_Summary(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql_OLD = "SELECT CONVERT(NVARCHAR, bookingDate, 105) BDate, \n"
                + "       b.name                      AS BranchName, \n"
                + "       ( \n"
                + "           SELECT e.name \n"
                + "           FROM   ExpressCenters e \n"
                + "           WHERE  e.expressCenterCode = a.originExpressCenter \n"
                + "       )                              ExpressCenterName, \n"
                + "       a.originExpressCenter,a.orgin branchCode, \n"
                + "       COUNT(CASE WHEN a.isApproved = '0' THEN 1 END) + \n"
                + "       COUNT(CASE WHEN a.isApproved IS NULL THEN 1 END) AS UnApproved, \n"
                + "       COUNT(CASE WHEN a.isApproved = '1' THEN 1 END) AS \"Approved\", \n"
                + "       COUNT(a.consignmentNumber)  AS TotalConsi, \n"
                //  + "       Consigner CustomerName, ServiceTypeName Service, CreditClientID ClientID, (Select Name from ConsignmentType where id = Consignmenttypeid) Consignmenttype, \n"
                + "       Max(a.chargedAmount) chargeAmount,  Max(TotalAmount)  TotalAmount   \n"
                //  + "       --MAX(bookingDate)            AS 'Last Booking Date' \n"
                + "FROM   Consignment a \n"
                + "       INNER JOIN Branches b \n"
                + "            ON  a.orgin = b.branchCode \n"
                + " \n" + clvar._Year + clvar._Month + clvar._TownCode + "\n"
                + "            AND consignerAccountNo = '0' \n"
                + "GROUP BY \n"
                + "       b.name, \n"
                + "       CONVERT(NVARCHAR, bookingDate, 105), \n"
                + "       a.originExpressCenter,a.orgin \n"
                //   + "       Consigner , ServiceTypeName , CreditClientID ,  Consignmenttypeid \n"
                + "ORDER BY \n"
                + "       b.name, BDate ASC";



                string sqlString = "SELECT CONVERT(NVARCHAR, bookingDate, 105) BDate,\n" +
                "       b.name AS BranchName,b.branchCode, a.originExpressCenter, ec.Name ExpressCenterName, \n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved = '0' THEN\n" +
                "                1\n" +
                "             END) + COUNT(CASE\n" +
                "                            WHEN a.isApproved IS NULL THEN\n" +
                "                             1\n" +
                "                          END) AS UnApproved,\n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved = '1' THEN\n" +
                "                1\n" +
                "             END) AS \"Approved\",\n" +
                "       COUNT(a.consignmentNumber) AS TotalConsi,\n" +
                "       Max(a.chargedAmount) chargeAmount,\n" +
                "       Max(TotalAmount) TotalAmount\n" +
                "  FROM Consignment a\n" +
                " INNER JOIN Branches b\n" +
                "    ON a.orgin = b.branchCode\n" +
                /*
                " inner join (select *\n" +
                "               from Riders r1\n" +
                "              where r1.expressCenterId + r1.riderCode not in\n" +
                "                    ('3711108',\n" +
                "                     '0430122',\n" +
                "                     '0411234',\n" +
                "                     '2311328',\n" +
                "                     '1812-1401',\n" +
                "                     '1812-1403',\n" +
                "                     '1812-1406',\n" +
                "                     '0230-1450',\n" +
                "                     '0222-1514',\n" +
                "                     '0222-1519',\n" +
                "                     '1813-1601',\n" +
                "                     '1813-1607',\n" +
                "                     '31121007',\n" +
                "                     '11111101',\n" +
                "                     '01113001',\n" +
                "                     'EXCENTER-FRFSD00029001',\n" +
                "                     '013111714')) r\n" +
                "    on a.riderCode = r.riderCode\n" +
                "   and a.orgin = r.branchId\n" +
                 * */
                "\n" +
                //   "   AND cast(bookingDate as date) BETWEEN '2016-04-01' AND '2016-04-30'\n" +
                //   "   AND b.branchCode IN ('4')\n" +
                "   AND consignerAccountNo = '0'  INNER JOIN EXPRESSCENTERS EC on EC.ExpresscenterCode = a.originExpressCenter \n" +
                " \n" + clvar._Year + clvar._Month + clvar._TownCode + "\n" +
                " GROUP BY b.name, CONVERT(NVARCHAR, bookingDate, 105), b.branchCode,a.originExpressCenter, ec.Name\n" +
                " ORDER BY b.name, BDate ASC";




                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
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

        public DataSet Get_Cash_Consignment_Summary_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {

                string sqlString = "SELECT a.consignmentNumber,CONVERT(NVARCHAR, bookingDate, 105) BDate, a.weight, \n"
                //   + " r.firstName+' '+r.lastName ridername, \n"
                + "B2.NAME DESTINATIONBRANCH,\n"
                + "       b.name                      AS BranchName, \n"
                + "       ( \n"
                + "           SELECT e.name \n"
                + "           FROM   ExpressCenters e \n"
                + "           WHERE  e.expressCenterCode = a.originExpressCenter \n"
                + "       )                              ExpressCenterName, \n" +

                "       (Select ISNULL(Max(cm.calculatedValue),0) from ConsignmentModifier cm \n" +
                "           where cm.consignmentNumber = a.consignmentNumber) FuelCharge,\n" +
                "       (Select ISNULL(Max(cm.calculatedGST),0) from ConsignmentModifier cm \n" +
                "           where cm.consignmentNumber = a.consignmentNumber) FuelGST, \n"

                + "       a.originExpressCenter,a.orgin branchCode, a.consignerAccountNo,\n"
                + "       COUNT(CASE WHEN a.isApproved = '0' THEN 1 END) + \n"
                + "       COUNT(CASE WHEN a.isApproved IS NULL THEN 1 END) AS UnApproved, \n"
                + "       COUNT(CASE WHEN a.isApproved = '1' THEN 1 END) AS \"Approved\", \n"
                + "       COUNT(a.consignmentNumber)  AS TotalConsi, Consigner CustomerName, ServiceTypeName Service, CreditClientID ClientID, \n"
                + "        (Select Name from ConsignmentType where id = Consignmenttypeid) Consignmenttype, \n "
                + "        ISNULL(Max(a.chargedAmount),0) chargeAmount,  ISNULL(Max(TotalAmount),0) TotalAmount    \n"

                + "FROM   Consignment a \n"
                + "       INNER JOIN Branches b \n"
                + "            ON  a.orgin = b.branchCode \n"
                //   + " inner join Riders r\n"
                //   + " on a.riderCode = r.riderCode"
                + " INNER JOIN BRANCHES B2"
                + " ON A.DESTINATION = B2.BRANCHCODE"
                + "            and convert(nvarchar, a.bookingDate, 105) = '" + clvar._FromMonth + "'\n"
                + "             AND consignerAccountNo = '0' \n"
                + "            and b.branchCode = '" + clvar._Zone + "'\n"
                + "            and   a.isApproved = '" + clvar._status + "'\n";
                //if (clvar._salegroup != string.Empty)
                //{
                //    sqlString += "   and a.originExpressCenter = '" + clvar._salegroup + "'\n";
                //}
                //else
                //{
                //    sqlString += "   and a.originExpressCenter is null \n";
                //}
                sqlString += "GROUP BY \n"
                + "       a.consignmentNumber,b.name, a.consignerAccountNo,\n"
                + "       CONVERT(NVARCHAR, bookingDate, 105), \n"
                + "       a.originExpressCenter,a.orgin, Consigner , ServiceTypeName , CreditClientID ,  Consignmenttypeid, a.weight, \n"
                //  + " r.firstName+' '+r.lastName , \n"
                + " B2.NAME \n"
                + "ORDER BY \n"
                + "       b.name, BDate ASC";

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

        public DataSet Get_Graph(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT z.name ZoneName,\n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved = '1' THEN\n" +
                "                1\n" +
                "             END) AS \"Approved\",\n" +
                "       COUNT(CASE\n" +
                "               WHEN a.isApproved IS NULL THEN\n" +
                "                1\n" +
                "             END) AS UnApproved\n" +
                "  FROM Consignment a\n" +
                " INNER JOIN Branches b\n" +
                "    ON a.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    ON a.zoneCode = z.zoneCode\n" +
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

        public DataSet Get_MainMenu(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT MENU_ID, MENU_NAME FROM Main_Menu WHERE STATUS = '1' ";
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

        public DataSet Get_ChildMenubyMainMenu(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "select c.child_menuid, c.sub_menu_name\n" +
                "  from main_menu m, child_menu c\n" +
                " where c.main_Menu_id = m.Menu_id\n" +
                "   and m.Menu_id = '" + clvar._menuname + "' ";

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

        public void Insert_Main_Menu(Variable clvar)
        {
            try
            {
                string query =
                "insert into Main_Menu \n" +
                "  (Menu_Name, HyperLink, Status, EntryDateTime)\n" +
                "values\n" +
                "  ( \n" +
                "   '" + clvar._menuname + "',\n" +
                "   '" + clvar._hyperlink + "',\n" +
                "   '1',\n" +
                "   GETDATE()\n" +
               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public void Insert_Child_Main(Variable clvar)
        {
            try
            {
                string query =
                "insert into Child_Menu \n" +
                "  (Main_Menu_Id, Sub_Menu_Name, HyperLink, Status, EntryDateTime)\n" +
                "values\n" +
                "  ( \n" +
                "   '" + clvar._menuname + "',\n" +
                "   '" + clvar._childmenu + "',\n" +
                "   '" + clvar._hyperlink + "',\n" +
                "   '1',\n" +
                "   GETDATE()\n" +
               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public void Insert_Report(Variable clvar)
        {
            try
            {
                string query = "insert into Report_Info \n" +
                                "  (ReportName, HyperLink, EntryDateTime, ZoneCode, BranchCode, ExpressCenterCode)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._reportname + "',\n" +
                                "   '" + clvar._hyperlink + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar._Zone + "',\n" +
                                "   '" + clvar._brand + "',\n" +
                                "   '" + clvar._Expresscentercode + "'\n" +
                               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_ReportInfo(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT reportid,\n" +
                                    "       reportname,\n" +
                                    "       hyperlink,\n" +
                                    "       convert(varchar(11), entrydatetime, 106) entrydatetime \n" +
                                    "  FROM Report_Info ";

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

        public DataSet Get_ReportLink(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT hyperlink FROM Report_Info WHERE reportid = '" + clvar._reportid + "' ";
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

        public DataSet Get_MainMenuGridView(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT Menu_Id,\n" +
                            "       Menu_Name,\n" +
                            "       CONVERT(NVARCHAR, EntryDateTime, 105) EntryDateTime,\n" +
                            " hyperlink\n" +
                            "  from main_menu ";

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

        public DataSet Get_ChildMenuGridView(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select c.child_menuid, menu_name, sub_menu_name, c.hyperlink, CONVERT(NVARCHAR, c.entrydatetime, 105) entrydatetime from main_menu m, child_menu c where c.main_Menu_id = m.Menu_id ";

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

        public void Insert_Profile_Head(Variable clvar)
        {
            try
            {
                string query =
                "insert into Profile_Head \n" +
                "  (Profile_Name ,ZoneCode ,BranchCode ,ExpressCenterCode ,EntryDateTime ,EntryUserName ,Status)\n" +
                "values\n" +
                "  ( \n" +
                "   '" + clvar._ProfileName + "',\n" +
                "   '" + clvar._Zone + "',\n" +
                "   '" + clvar._TownCode + "',\n" +
                "   '" + clvar._Expresscentercode + "',\n" +
                "   GETDATE(),\n" +
                "   '',\n" +
                "   '1'\n" +
               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public void Insert_Profile_Detail(Variable clvar)
        {
            try
            {
                string query =
                "insert into Profile_Detail \n" +
                "  (Profile_Id, MainMenu_Id, ChildMenu_Id, EntryDateTime)\n" +
                "values\n" +
                "  ( \n" +
                "   '" + clvar._Id + "',\n" +
                "   '" + clvar._menuname + "',\n" +
                "   '" + clvar._childmenu + "',\n" +
                "   GETDATE()\n" +
               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_Profile_Max_Id(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select MAX(Profile_Id) Profile_Id from Profile_Head ";

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

        public DataSet Get_ProfileListGridView(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT H.PROFILE_ID,\n" +
                                "       H.PROFILE_NAME,\n" +
                                "       H.ZONECODE,\n" +
                                "       H.BRANCHCODE,\n" +
                                "       H.EXPRESSCENTERCODE,\n" +
                                "       CONVERT(NVARCHAR, H.ENTRYDATETIME, 105) ENTRYDATETIME\n" +
                                "  FROM PROFILE_HEAD H";

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

        public DataSet Get_SingleRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT H.PROFILE_ID,\n" +
                                "       H.PROFILE_NAME,\n" +
                                "       H.ZONECODE,\n" +
                                "       H.BRANCHCODE,\n" +
                                "       H.EXPRESSCENTERCODE,\n" +
                                "       CONVERT(NVARCHAR, H.ENTRYDATETIME, 105) ENTRYDATETIME\n" +
                                "  FROM PROFILE_HEAD H \n" +
                                "   WHERE \n" +
                                "       H.PROFILE_ID = '" + clvar._Id + "' ";

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

        public DataSet Get_AllUsersGridView(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT DSG_CODE,\n" +
                                    "       U_ID,\n" +
                                    "       NAME,\n" +
                                    "       U_NAME,\n" +
                                    "       Designation, Responsible,\n" +
                                    "       r.typename Type,\n" +
                                    "       p.Profile_Name,\n" +
                                    "       CASE WHEN u.STATUS = '1' THEN 'Active' WHEN u.STATUS = '0' THEN 'DeActive' END STATUS,\n" +
                                    "       CONVERT(NVARCHAR, ACTIVE_DATE, 105) ACTIVE_DATE ,\n" +
                                    "       CONVERT(NVARCHAR, INACTIVE_DATE, 105) INACTIVE_DATE, \n" +
                                    "       excel_permission \n" +
                                    "  FROM ZNI_USER1 u, Profile_Head p, User_type r\n" +
                                    " where u.Profile = p.Profile_Id\n" +
                                    "   and u.U_TYPE = r.TYPEID\n" +
                                    " order by U_NAME ASC";


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

        public DataSet Get_UserType(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT TYPEID, TYPENAME from User_type order by TYPENAME ASC";
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

        public void Insert_Users(Variable clvar)
        {
            try
            {
                string query =
                "insert into ZNI_USER1 \n" +
                "  (DSG_CODE, U_CODE, Designation, U_NAME, NAME, U_PASSWORD, U_TYPE, Profile, STATUS, ACTIVE_DATE, excel_permission, user_mac_add, department, Responsible, ZoneCode, branchCode, bts_User, ExpressCenter )\n" +
                "values\n" +
                "  ( \n" +
                "   '" + clvar._DesignationCode + "',\n" +
                "   '" + clvar._DesignationCode + "',\n" +
                "   '" + clvar._Designation + "',\n" +
                "   '" + clvar._UserId + "',\n" +
                "   '" + clvar._UserName + "',\n" +
                "   '" + clvar._password + "',\n" +
                "   '" + clvar._Type + "',\n" +
                "   '" + clvar._ProfileName + "',\n" +
                "   '1',\n" +
                "   GETDATE(),\n" +
                "   '" + clvar._ExcelPermission + "',\n" +
                "   '" + clvar._MACAddress + "',\n" +
                "   '" + clvar._Department + "', \n" +
                "   '" + clvar._Responsible + "', \n" +
                "   '" + clvar._Zone + "', \n" +
                "   '" + clvar._TownCode + "',\n" +
                "   '" + clvar._status + "',\n" +
                "   '" + clvar._Expresscentercode + "'\n" +
                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public void Update_Users(Variable clvar)
        {
            try
            {
                string query =
                "UPDATE ZNI_USER1 SET \n" +
                " DSG_CODE = '" + clvar._DesignationCode + "', \n" +
                " U_CODE = '" + clvar._DesignationCode + "', \n" +
                " Designation = '" + clvar._Designation + "',  \n" +
                " U_NAME = '" + clvar._UserId + "', \n" +
                " NAME = '" + clvar._UserName + "',  \n" +
                " U_PASSWORD =  '" + clvar._password + "', \n" +
                " U_TYPE = '" + clvar._Type + "',  \n" +
                " Profile = '" + clvar._ProfileName + "',  \n" +
                " STATUS =  '1', \n" +
                " excel_permission = '" + clvar._ExcelPermission + "',  \n" +
                " user_mac_add =  '" + clvar._MACAddress + "', \n" +
                " department = '" + clvar._Department + "', \n" +
                " ZoneCode =  '" + clvar._Zone + "', \n" +
                " branchCode = '" + clvar._TownCode + "'\n" +
                " Responsible = '" + clvar._Responsible + "'\n" +
                " where u_id = '' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_SingleUserRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select * from ZNI_USER1 z where z.U_ID = '" + clvar._UserId + "' ";

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

        public void Insert_UsersType(Variable clvar)
        {
            try
            {
                string query =
                "insert into User_type \n" +
                "  (Typename)\n" +
                "values\n" +
                "  ( \n" +
                "   '" + clvar._Type + "'\n" +
                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_Department(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT Departmentid, Department from Department order by Department ASC";
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

        public DataSet Get_Day_Wise_Cash_Summary(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                // CONVERT(varchar, CONVERT(money, sum(CHARGEDAMOUNT)), 1) CHARGEDAMOUNT
                //      string sql = "SELECT CONVERT(varchar, CONVERT(money, sum(CHARGEDAMOUNT)), 1) CHARGEDAMOUNT, ZONECODE,\n" +

                //string sql = "SELECT SUM(CHARGEDAMOUNT) CHARGEDAMOUNT, ZONECODE,\n" +
                //"       CONVERT(NVARCHAR, BOOKINGDATE, 105) BOOKINGDATE\n" +
                //"  FROM CONSIGNMENT\n" +
                //" WHERE p.BranchCode = b.branchCode \n" +
                //clvar._Year + clvar._Month + clvar._TownCode +
                //" GROUP BY CONVERT(NVARCHAR, BOOKINGDATE, 105),ZONECODE\n" +
                //" ORDER BY BOOKINGDATE";
                /*
                string sql = "SELECT \n" +
                " SUM(p.AMOUNTUSED) RECOVERAMOUNT,\n" +
                "       CONVERT(NVARCHAR, p.VOUCHERDATE, 105) RECEIPTDATE,\n" +
                "       p.ZONECODE,\n" +
                "       z.name\n" +
                "  FROM PAYMENTVOUCHERS p, zones z\n" +
                " WHERE p.zoneCode = z.zoneCode\n" +
                clvar._Year + clvar._Month + clvar._TownCode +
                " GROUP BY CONVERT(NVARCHAR, p.VOUCHERDATE, 105), p.ZONECODE, z.name\n" +
                " ORDER BY p.ZONECODE ASC";
                */



                string sql = "select cast(c.accountReceivingDate as DATE) 'RECEIPTDATE',\n" +
                                    "       z.name Zone, z.zoneCode zoneCode,\n" +
                                    "       b.name Branch,\n" +
                                    "       convert(varchar, cast(SUM(c.chargedAmount) as money), -1) RECOVERAMOUNT,\n" +
                                    "       count(c.consignmentNumber) CNCount\n" +
                                    "  from Consignment c, Branches b, Zones z\n" +
                                    " where \n" +
                                    // " year(c.accountReceivingDate) = '2016'\n" +
                                    // "   and month(c.accountReceivingDate) = '03'\n" +
                                    "   c.orgin = b.branchCode\n" +
                                    "   and b.zoneCode = z.zoneCode\n" +
                                    "   and c.isApproved = '1'\n" +
                                  //   "   and z.zoneCode = '2'\n" +
                                  " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9')\n" +
                                    clvar._Year + clvar._Month + clvar._TownCode +
                                    " group by cast(c.accountReceivingDate as DATE), z.name, b.name, z.zoneCode\n" +
                                    " order by 1, 2, 3";

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

        public DataSet Get_Day_Wise_Cash_Summary_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT SUM(CHARGEDAMOUNT) CHARGEDAMOUNT,\n" +
                "       CONVERT(NVARCHAR, BOOKINGDATE, 105) BOOKINGDATE\n" +
                "  FROM CONSIGNMENT\n" +
                " WHERE \n" +
                " CONVERT(NVARCHAR, BOOKINGDATE, 105) = '" + clvar._Year + "' \n" +
                " AND ZONECODE = '" + clvar._Zone + "' \n" +
                " GROUP BY CONVERT(NVARCHAR, BOOKINGDATE, 105),CHARGEDAMOUNT\n" +
                " ORDER BY BOOKINGDATE";


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

        public DataSet Get_Credit_Consignment_Booking(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT CONVERT(NVARCHAR, BOOKINGDATE, 105) BDATE,\n" +
                "       B.NAME AS BRANCHNAME,\n" +
                "       (SELECT E.NAME\n" +
                "          FROM EXPRESSCENTERS E\n" +
                "         WHERE E.EXPRESSCENTERCODE = A.ORIGINEXPRESSCENTER) EXPRESSCENTERNAME,\n" +
                "       A.ORIGINEXPRESSCENTER,\n" +
                "       A.ORGIN BRANCHCODE,\n" +
                "       COUNT(CASE\n" +
                "               WHEN A.ISAPPROVED = '0' THEN\n" +
                "                1\n" +
                "             END) + COUNT(CASE\n" +
                "                            WHEN A.ISAPPROVED IS NULL THEN\n" +
                "                             1\n" +
                "                          END) AS UNAPPROVED,\n" +
                "       COUNT(CASE\n" +
                "               WHEN A.ISAPPROVED = '1' THEN\n" +
                "                1\n" +
                "             END) AS \"APPROVED\",\n" +
                "       COUNT(A.CONSIGNMENTNUMBER) AS TOTALCONSI,\n" +
                "       MAX(BOOKINGDATE) AS 'LAST BOOKING DATE'\n" +
                "  FROM CONSIGNMENT A\n" +
                " INNER JOIN BRANCHES B\n" +
                "    ON A.ORGIN = B.BRANCHCODE\n" +
                " INNER JOIN CREDITCLIENTS C\n" +
                "    ON A.CREDITCLIENTID = C.ID\n" +
                "\n" +
                clvar._Year + clvar._Month + clvar._TownCode +
                "   AND CONSIGNERACCOUNTNO != '0'\n" +
                "   AND A.COD = '1' \n" +
                " GROUP BY B.NAME,\n" +
                "          CONVERT(NVARCHAR, BOOKINGDATE, 105),\n" +
                "          A.ORIGINEXPRESSCENTER,\n" +
                "          A.ORGIN\n" +
                " ORDER BY B.NAME, BDATE ASC";




                string sqlString = "SELECT CONVERT(NVARCHAR, BOOKINGDATE, 105) BDATE,\n" +
                "       B.NAME AS BRANCHNAME, a.originExpressCenter, ec.name ExpressCenterName,\n" +
                "       b.branchCode,\n" +
                "       COUNT(CASE\n" +
                "               WHEN A.ISAPPROVED = '0' THEN\n" +
                "                1\n" +
                "             END) + COUNT(CASE\n" +
                "                            WHEN A.ISAPPROVED IS NULL THEN\n" +
                "                             1\n" +
                "                          END) AS UNAPPROVED,\n" +
                "       COUNT(CASE\n" +
                "               WHEN A.ISAPPROVED = '1' THEN\n" +
                "                1\n" +
                "             END) AS \"APPROVED\",\n" +
                "       COUNT(A.CONSIGNMENTNUMBER) AS TOTALCONSI,\n" +
                "       MAX(BOOKINGDATE) AS 'LAST BOOKING DATE'\n" +
                "  FROM CONSIGNMENT A\n" +
                " INNER JOIN BRANCHES B\n" +
                "    ON A.ORGIN = B.BRANCHCODE\n" +
                " INNER JOIN CREDITCLIENTS C\n" +
                "    ON A.CREDITCLIENTID = C.ID\n" +
                /*
                " inner join (select *\n" +
                "               from Riders r1\n" +
                "              where r1.expressCenterId + r1.riderCode not in\n" +
                "                    ('3711108',\n" +
                "                     '0430122',\n" +
                "                     '0411234',\n" +
                "                     '2311328',\n" +
                "                     '1812-1401',\n" +
                "                     '1812-1403',\n" +
                "                     '1812-1406',\n" +
                "                     '0230-1450',\n" +
                "                     '0222-1514',\n" +
                "                     '0222-1519',\n" +
                "                     '1813-1601',\n" +
                "                     '1813-1607',\n" +
                "                     '31121007',\n" +
                "                     '11111101',\n" +
                "                     '01113001',\n" +
                "                     'EXCENTER-FRFSD00029001',\n" +
                "                     '013111714')) r\n" +
                "    on a.riderCode = r.riderCode\n" +
                "   and a.orgin = r.branchId\n" +
                 * */
                //  "   AND cast(bookingDate as date) BETWEEN '2016-04-01' AND '2016-04-25'\n" +
                //  "   AND b.branchCode IN (4)\n" +
                "   AND CONSIGNERACCOUNTNO != '0'\n" +
                "   AND A.COD = '1' INNER JOIN ExpressCenters ec on ec.expressCenterCode = a.originExpressCenter \n" +
              clvar._Year + clvar._Month + clvar._TownCode +
                " GROUP BY B.NAME, CONVERT(NVARCHAR, BOOKINGDATE, 105), b.branchCode, a.originExpressCenter, ec.name\n" +
                " ORDER BY B.NAME, BDATE ASC";




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

        public DataSet Get_CashConsignmentLessChargeReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select c.consignmentNumber 'Consignment Number',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       e.name 'Retail Office',\n" +
                "       cc.accountNo 'Account No',\n" +
                "       c.consigner 'Shipper',\n" +
                "       c.consignerCellNo 'Shipper Cell No',\n" +
                "       c.consignerPhoneNo 'Shipper Phone No',\n" +
                "       c.consignee 'Consignee',\n" +
                "       c.consigneePhoneNo 'Consignee Phone No',\n" +
                "       b.sname + '-' + c.riderCode 'Booking Code',\n" +
                "       case\n" +
                "         when r.lastName is null then\n" +
                "          r.firstName\n" +
                "         when r.lastName = 'null' then\n" +
                "          r.firstName\n" +
                "         when r.firstName = r.lastName then\n" +
                "          r.firstName\n" +
                "         else\n" +
                "          r.firstName + ' ' + r.lastName\n" +
                "       end 'Booking staff Name',\n" +
                "       r.userTypeId 'Source (CR / Counter)',\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
                "       CONVERT(VARCHAR(10), c.accountReceivingDate, 105) 'Sale Date',\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end Service,\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('Flyer',\n" +
                "                                    'NTS',\n" +
                "                                    'HEC',\n" +
                "                                    'Bank to Bank',\n" +
                "                                    'Bulk Shipment',\n" +
                "                                    'overnight',\n" +
                "                                    'Return Service',\n" +
                "                                    'Same Day',\n" +
                "                                    'Second Day',\n" +
                "                                    'Smart Box',\n" +
                "                                    'Sunday & Holiday',\n" +
                "                                    'Smart Box',\n" +
                "                                    'Hand Carry',\n" +
                "                                    'Smart Cargo',\n" +
                "                                    'MB10',\n" +
                "                                    'MB2',\n" +
                "                                    'MB20',\n" +
                "                                    'MB30',\n" +
                "                                    'MB5') then\n" +
                "          'Domestic'\n" +
                "         when c.serviceTypeName in\n" +
                "              ('International_Box',\n" +
                "               'International Cargo',\n" +
                "               'International_Non-Doc',\n" +
                "               'International_Non-Doc_Special_Hub_2014',\n" +
                "               'Logex',\n" +
                "               'International_Doc_Special_Hub',\n" +
                "               'International_Doc') then\n" +
                "          'International'\n" +
                "         when c.serviceTypeName in ('Expressions',\n" +
                "                                    'International Expressions',\n" +
                "                                    'Mango',\n" +
                "                                    'Mango Petty') then\n" +
                "          'Expressions'\n" +
                "         when c.serviceTypeName = 'Aviation Sale' then\n" +
                "          'Aviation Sale'\n" +
                "         when c.serviceTypeName = 'Road n Rail' then\n" +
                "          'Road n Rail'\n" +
                "       end 'Product',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          c.destinationCountryCode\n" +
                "         else\n" +
                "          bb.name\n" +
                "       end 'Destination Branch',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          'International'\n" +
                "         when c.orgin = c.destination then\n" +
                "          'local'\n" +
                "         when c.orgin != c.destination and z.colorId = zz.colorId then\n" +
                "          'Same Zone'\n" +
                "         when c.orgin != c.destination and z.colorId != zz.colorId then\n" +
                "          'Diff Zone'\n" +
                "       end 'Zoning',\n" +
                "       c.weight Weight,\n" +
                "       c.pieces,\n" +
                "       c.TotalAmount + c.gst + case\n" +
                "         when cm1.[Extra Charges] is null then\n" +
                "          0\n" +
                "         else\n" +
                "          cm1.[Extra Charges]\n" +
                "       end 'Gross Tariff',\n" +
                "       round(case\n" +
                "               when c.serviceTypeName = 'Expressions' then\n" +
                "                c.totalAmount + c.gst\n" +
                "               else\n" +
                "                c.chargedAmount\n" +
                "             end,\n" +
                "             0) 'Gross Sale',\n" +
                "       ceiling(c.TotalAmount + c.gst + case\n" +
                "                 when cm1.[Extra Charges] is null then\n" +
                "                  0\n" +
                "                 else\n" +
                "                  cm1.[Extra Charges]\n" +
                "               end - round(case\n" +
                "                             when c.serviceTypeName = 'Expressions' then\n" +
                "                              c.totalAmount + c.gst\n" +
                "                             else\n" +
                "                              c.chargedAmount\n" +
                "                           end,\n" +
                "                           0)) Difference\n" +
                "  from Consignment c\n" +
                " inner join Riders r\n" +
                "    on c.riderCode = r.riderCode\n" +
                "   and c.orgin = r.branchId\n" +
                "   and r.status in ('0', '1')\n" +
                " inner join ExpressCenters e\n" +
                "    on r.expressCenterId = e.expressCenterCode\n" +
                " inner join creditclients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                "  left join Zones zz\n" +
                "    on bb.zoneCode = zz.zoneCode\n" +
                "  left join (select cm.consignmentNumber,\n" +
                "                    sum(cm.calculatedValue + cm.calculatedGST) 'Extra Charges'\n" +
                "               from ConsignmentModifier cm\n" +
                "              group by cm.consignmentNumber) cm1\n" +
                "    on c.consignmentNumber = cm1.consignmentNumber\n" +
                " where \n" +
                //   "   year(c.accountReceivingDate) = '2016'\n" +
                //   "   and month(c.accountReceivingDate) = '04'\n" +
                "    c.consignerAccountNo = '0'\n" +
                "   and isApproved = '1'\n" +

                "   AND z.zoneCode in ('1',\n" +
                                "'10',\n" +
                                "'11',\n" +
                                "'12',\n" +
                                "'2',\n" +
                                "'27',\n" +
                                "'3',\n" +
                                "'4',\n" +
                                "'5',\n" +
                                "'7',\n" +
                                "'9')\n" +

                clvar._Year + clvar._TownCode + clvar._Zone +

                "   and ceiling(c.TotalAmount + c.gst + case\n" +
                "                 when cm1.[Extra Charges] is null then\n" +
                "                  0\n" +
                "                 else\n" +
                "                  cm1.[Extra Charges]\n" +
                "               end - round(case\n" +
                "                             when c.serviceTypeName = 'Expressions' then\n" +
                "                              c.totalAmount + c.gst\n" +
                "                             else\n" +
                "                              c.chargedAmount\n" +
                "                           end,\n" +
                "                           0)) NOT BETWEEN - 1 AND 1";


                /*
                string sql = "SELECT C.CONSIGNMENTNUMBER,\n" +
                "       CONVERT(NVARCHAR, C.BOOKINGDATE, 105) BOOKINGDATE,\n" +
                "       B.NAME ORIGNBRANCH,\n" +
                "       C.RIDERCODE,\n" +
                "       B2.NAME DESTINATIONBRANCH,\n" +
                "       C.WEIGHT,\n" +
                "       R.FIRSTNAME + ' ' + R.LASTNAME RIDERNAME,\n" +
                "         CASE WHEN \n" +
                "       CONVERT(VARCHAR,\n" +
                "       (C.TOTALAMOUNT -\n" +
                "       (C.CHARGEDAMOUNT - ((C.CHARGEDAMOUNT / 100) * A.GST))) ) is null THEN '0' ELSE \n" +
                "       CONVERT(VARCHAR,\n" +
                "       (C.TOTALAMOUNT -\n" +
                "       (C.CHARGEDAMOUNT - ((C.CHARGEDAMOUNT / 100) * A.GST))) )\n" +
                "        END TAMOUNT\n" +
                "         FROM BRANCHGST A, BRANCHES B, BRANCHES B2, CONSIGNMENT C, RIDERS R\n" +
                " WHERE C.BRANCHCODE = A.BRANCHCODE\n" +
                "   AND C.DESTINATION = B2.BRANCHCODE\n" +
                "   AND C.ORGIN = B.BRANCHCODE\n" +
                "   AND C.RIDERCODE = R.RIDERCODE\n" +
                "   AND C.DESTINATION = R.BRANCHID\n" +
                "   AND C.ISAPPROVED = '1'\n" +
                clvar._TownCode + clvar._Zone +
                "   AND A.COMPANYID = '1'\n" +
                "   AND A.STATUS = '1'\n" +
                "   AND R.STATUS = '1'\n" +
                "   AND A.MODIFIEDON IS NULL\n" +
                clvar._Year +
                " ORDER BY C.BOOKINGDATE";
                */
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

        public DataSet Get_Booking_Consignment_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString_old = "SELECT A.CONSIGNMENTNUMBER,\n" +
                "       Z.NAME ORIGINZONE,\n" +
                "       B.NAME AS ORIGINBRANCH,\n" +
                "       (SELECT E.NAME\n" +
                "          FROM EXPRESSCENTERS E\n" +
                "         WHERE E.EXPRESSCENTERCODE = A.ORIGINEXPRESSCENTER) ORIGINEC,\n" +
                "       A.ORIGINEXPRESSCENTER,\n" +
                "       B2.NAME DESTINATIONBRANCH,\n" +
                "       CONSIGNER CUSTOMERNAME,\n" +
                "       A.CONSIGNERACCOUNTNO,\n" +
                "       CONVERT(NVARCHAR, BOOKINGDATE, 105) BOOKINGDATE,\n" +
                "       A.TOTALAMOUNT,\n" +
                "       A.CHARGEDAMOUNT,\n" +
                "       A.GST,\n" +
                "       A.CONSIGNEE,\n" +
                "       A.CONSIGNEEPHONENO,\n" +
                "       R.FIRSTNAME + ' ' + R.LASTNAME AS RIDERNAME,\n" +
                "       A.DELIVERYSTATUS\n" +
                "  FROM CONSIGNMENT A\n" +
                " INNER JOIN BRANCHES B\n" +
                "    ON A.ORGIN = B.BRANCHCODE\n" +
                " INNER JOIN ZONES Z\n" +
                "    ON A.ZONECODE = Z.ZONECODE\n" +
                " INNER JOIN BRANCHES B2\n" +
                "    ON A.DESTINATION = B2.BRANCHCODE\n" +
                " INNER JOIN RIDERS R\n" +
                "    ON A.RIDERCODE = R.RIDERCODE\n" +
                "   AND CONVERT(NVARCHAR, A.BOOKINGDATE, 105) = '" + clvar._FromMonth + "'\n" +
                //"   AND A.ZONECODE = '2'\n" +
                "   AND A.BRANCHCODE = '" + clvar._TownCode + "'\n" +
                //  "   AND originExpressCenter = '" + clvar._Expresscentercode + "' \n" +
                //  clvar._Expresscentercode +
                "   AND A.COD = '1'\n" +
                " GROUP BY B.NAME,\n" +
                "          A.CONSIGNERACCOUNTNO,\n" +
                "          CONVERT(NVARCHAR, BOOKINGDATE, 105),\n" +
                "          A.ORIGINEXPRESSCENTER,\n" +
                "          A.ORGIN,\n" +
                "          CONSIGNER,\n" +
                "          SERVICETYPENAME,\n" +
                "          CREDITCLIENTID,\n" +
                "          CONSIGNMENTTYPEID,\n" +
                "          A.CONSIGNMENTNUMBER,\n" +
                "          Z.NAME,\n" +
                "          B2.NAME,\n" +
                "          A.CHARGEDAMOUNT,\n" +
                "          A.TOTALAMOUNT,\n" +
                "          A.CONSIGNEE,\n" +
                "          CONSIGNEEPHONENO,\n" +
                "          R.FIRSTNAME + ' ' + R.LASTNAME,\n" +
                "          A.GST,\n" +
                "          A.DELIVERYSTATUS\n" +
                " ORDER BY B.NAME, BOOKINGDATE ASC";





                string sqlString = "SELECT CONVERT(NVARCHAR, BOOKINGDATE, 105) BDATE,\n" +
                "       B.NAME AS BRANCHNAME,\n" +
                "       b.branchCode,\n" +
                "       A.CONSIGNMENTNUMBER,\n" +
                "       Z.NAME ORIGINZONE,\n" +
                "       B.NAME AS ORIGINBRANCH,\n" +
                "       (SELECT E.NAME\n" +
                "          FROM EXPRESSCENTERS E\n" +
                "         WHERE E.EXPRESSCENTERCODE = A.ORIGINEXPRESSCENTER) ORIGINEC,\n" +
                "       A.ORIGINEXPRESSCENTER,\n" +
                "       B2.NAME DESTINATIONBRANCH,\n" +
                "       CONSIGNER CUSTOMERNAME,\n" +
                "       A.CONSIGNERACCOUNTNO,\n" +
                "       CONVERT(NVARCHAR, BOOKINGDATE, 105) BOOKINGDATE,\n" +
                "       A.TOTALAMOUNT,\n" +
                "       A.CHARGEDAMOUNT,\n" +
                "       A.GST,\n" +
                "       A.CONSIGNEE,\n" +
                "       A.CONSIGNEEPHONENO,\n" +
                "       R.FIRSTNAME + ' ' + R.LASTNAME AS RIDERNAME,\n" +
                "       A.DELIVERYSTATUS\n" +
                "  FROM CONSIGNMENT A\n" +
                " INNER JOIN BRANCHES B\n" +
                "    ON A.ORGIN = B.BRANCHCODE\n" +
                " INNER JOIN CREDITCLIENTS C\n" +
                "    ON A.CREDITCLIENTID = C.ID\n" +
                " INNER JOIN ZONES Z\n" +
                "    ON A.ZONECODE = Z.ZONECODE\n" +
                " INNER JOIN BRANCHES B2\n" +
                "    ON A.DESTINATION = B2.BRANCHCODE\n" +
                " inner join (select *\n" +
                "               from Riders r1\n" +
                "              where r1.expressCenterId + r1.riderCode not in\n" +
                "                    ('3711108',\n" +
                "                     '0430122',\n" +
                "                     '0411234',\n" +
                "                     '2311328',\n" +
                "                     '1812-1401',\n" +
                "                     '1812-1403',\n" +
                "                     '1812-1406',\n" +
                "                     '0230-1450',\n" +
                "                     '0222-1514',\n" +
                "                     '0222-1519',\n" +
                "                     '1813-1601',\n" +
                "                     '1813-1607',\n" +
                "                     '31121007',\n" +
                "                     '11111101',\n" +
                "                     '01113001',\n" +
                "                     'EXCENTER-FRFSD00029001',\n" +
                "                     '013111714')) r\n" +
                "    on a.riderCode = r.riderCode\n" +
                "   and a.orgin = r.branchId\n" +
                "   AND A.COD = '1'\n" +
                "   AND CONVERT(NVARCHAR, A.BOOKINGDATE, 105) = '" + clvar._FromMonth + "'\n" +
                "   AND A.BRANCHCODE = '" + clvar._TownCode + "'\n" +
                " ORDER BY B.NAME, BDATE ASC";




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

        public void Insert_UserTrackLog(Variable clvar)
        {
            try
            {
                string query =
                "INSERT INTO USERTRACKLOG \n" +
                "  (userid, DateTime)\n" +
                "values\n" +
                "  ( \n" +
                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                "   GETDATE()\n" +
               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public void Insert_ReportTrackLog(Variable clvar)
        {
            try
            {
                string query =
                "INSERT INTO REPORTTRACKLOG \n" +
                "  (reportid, userid, DateTime)\n" +
                "values\n" +
                "  ( \n" +
                " '" + clvar._ReportId + "',\n" +
                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                "   GETDATE()\n" +
               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        // EC Wise Report Start
        /*
        public DataSet Get_EC_Sale_Product(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString_old =  "SELECT B.SERVICETYPENAME\n" +
                                    "  FROM (SELECT Z.NAME,\n" +
                                    "               EC.EXPRESSCENTERCODE,\n" +
                                    "               C.DESTINATIONEXPRESSCENTERCODE,\n" +
                                    "               C.SERVICETYPENAME,\n" +
                                    "               C.ORGIN,\n" +
                                    "               C.CHARGEDAMOUNT,\n" +
                                    "               C.TOTALAMOUNT,\n" +
                                    "               C.GST,\n" +
                                    "               BG.GST BRANCHGST,\n" +
                                    "               EC.NAME EXPRESSCENTER,\n" +
                                    "               EC.SNAME SHORTNAME,\n" +
                                    "               B.NAME BRANCHNAME,\n" +
                                    "               CONVERT(NVARCHAR(10), YEAR(C.BOOKINGDATE)) + ' - ' +\n" +
                                    "               CONVERT(NVARCHAR(10), MONTH(C.BOOKINGDATE)) DATE\n" +
                                    "          FROM CONSIGNMENT    C,\n" +
                                    "               EXPRESSCENTERS EC,\n" +
                                    "               BRANCHES       B,\n" +
                                    "               BRANCHGST      BG,\n" +
                                    "               ZONES          Z\n" +
                                    "         WHERE C.DESTINATIONEXPRESSCENTERCODE = EC.EXPRESSCENTERCODE\n" +
                                    "           AND C.DESTINATION = B.BRANCHCODE\n" +
                                    "           AND B.BRANCHCODE = BG.ID\n" +
                                    "           AND C.ZONECODE = Z.ZONECODE\n" +
                                    "           AND BG.STATUS = '1'\n" +
                                    "           AND EC.STATUS = '1'\n" +
                                   // "           AND YEAR(C.BOOKINGDATE) = '2016'\n" +
                                  //  "           AND MONTH(C.BOOKINGDATE) = '04'\n" +
                                //    "           and ec.expressCenterCode = '0111' \n" +
                                  //  "           AND C.ZONECODE = '2'\n" +
                                  //  "           AND C.BRANCHCODE = '4'\n" +
                                    clvar._TownCode + clvar._Zone + clvar._Year + clvar._Month + clvar._Expresscentercode +" \n" +
              //  "AND ec.expressCenterCode = '0111' \n" +
                                    "         GROUP BY EC.EXPRESSCENTERCODE,\n" +
                                    "                  C.DESTINATIONEXPRESSCENTERCODE,\n" +
                                    "                  C.SERVICETYPENAME,\n" +
                                    "                  C.ORGIN,\n" +
                                    "                  C.CHARGEDAMOUNT,\n" +
                                    "                  C.TOTALAMOUNT,\n" +
                                    "                  EC.NAME,\n" +
                                    "                  EC.SNAME,\n" +
                                    "                  B.NAME,\n" +
                                    "                  C.BOOKINGDATE,\n" +
                                    "                  BG.GST,\n" +
                                    "                  C.GST,\n" +
                                    "                  Z.NAME) B\n" +
                                    " GROUP BY B.SERVICETYPENAME\n" +
                                    " ORDER BY B.SERVICETYPENAME";

                string sqlString = "SELECT S.PRODUCTS SERVICETYPENAME FROM SERVICETYPES_NEW S GROUP BY S.PRODUCTS ";

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

        public DataSet Get_EC_SaleStatus_Information(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT b.expressCenterCode,\n" +
                "       b.ExpressCenter,\n" +
                "       b.branchGST,\n" +
                "       b.ShortName,\n" +
                "       b.BranchName,\n" +
                "       b.Date\n" +
                "  FROM (SELECT ec.expressCenterCode,\n" +
                "               c.destinationExpressCenterCode,\n" +
                "               c.serviceTypeName,\n" +
                "               c.orgin,\n" +
                "               c.chargedAmount,\n" +
                "               c.totalAmount,\n" +
                "               c.gst,\n" +
                "               bg.gst branchGST,\n" +
                "               ec.name ExpressCenter,\n" +
                "               ec.sname ShortName,\n" +
                "               b.name BranchName,\n" +
                "               CONVERT(NVARCHAR(10), YEAR(c.bookingDate)) + ' - ' +\n" +
                "               CONVERT(NVARCHAR(10), MONTH(c.bookingDate)) Date\n" +
                "          FROM Consignment c, ExpressCenters ec, Branches b, BranchGST bg\n" +
                "         WHERE c.destinationExpressCenterCode = ec.expressCenterCode\n" +
                "           AND c.destination = b.branchCode\n" +
                "           AND b.branchCode = bg.id\n" +
                "           AND bg.status = '1'\n" +
                "           AND ec.status = '1'\n" +
             //   "           AND YEAR(c.bookingDate) = '2016'\n" +
             //   "           AND MONTH(c.bookingDate) = '04'\n" +
              //  "           and ec.expressCenterCode = '0111'\n" +
             //   "           AND C.ZONECODE = '2'\n" +
             //   "           AND C.BRANCHCODE = '4'\n" +
                clvar._TownCode + clvar._Zone + clvar._Year + clvar._Month + clvar._Expresscentercode +" \n"+
             //   "AND ec.expressCenterCode = '0111' \n" +
                "         GROUP BY ec.expressCenterCode,\n" +
                "                  c.destinationExpressCenterCode,\n" +
                "                  c.serviceTypeName,\n" +
                "                  c.orgin,\n" +
                "                  c.chargedAmount,\n" +
                "                  c.totalAmount,\n" +
                "                  ec.name,\n" +
                "                  ec.sname,\n" +
                "                  b.name,\n" +
                "                  c.bookingDate,\n" +
                "                  bg.gst,\n" +
                "                  c.gst) b\n" +
                " GROUP BY b.expressCenterCode,\n" +
                "          b.branchGST,\n" +
                "          b.ShortName,\n" +
                "          b.BranchName,\n" +
                "          b.Date,\n" +
                "          b.ExpressCenter\n" +
                " ORDER BY expressCenterCode";


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
        */
        public DataSet Get_EC_GrossSale(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                /*
                            string sqlString_old = "SELECT b.expressCenterCode,\n" +
                            "       b.ExpressCenter,\n" +
                            "       b.serviceTypeName,\n" +
                            "       b.branchGST,\n" +
                            "       b.ShortName,\n" +
                            "       b.BranchName,\n" +
                            "       b.Date,\n" +
                            "       SUM(chargedAmount) chargedAmount,\n" +
                            "       SUM(b.totalAmount) totalAmount,\n" +
                            "       SUM(b.gst) gst\n" +
                            "  FROM (SELECT ec.expressCenterCode,\n" +
                            "               c.destinationExpressCenterCode,\n" +
                            "               c.serviceTypeName,\n" +
                            "               c.orgin,\n" +
                            "               c.chargedAmount,\n" +
                            "               c.totalAmount,\n" +
                            "               c.gst,\n" +
                            "               bg.gst branchGST,\n" +
                            "               ec.name ExpressCenter,\n" +
                            "               ec.sname ShortName,\n" +
                            "               b.name BranchName,\n" +
                            "               CONVERT(NVARCHAR(10), YEAR(c.bookingDate)) + ' - ' +\n" +
                            "               CONVERT(NVARCHAR(10), MONTH(c.bookingDate)) Date\n" +
                            "          FROM Consignment c, ExpressCenters ec, Branches b, BranchGST bg\n" +
                            "         WHERE c.destinationExpressCenterCode = ec.expressCenterCode\n" +
                            "           AND c.destination = b.branchCode\n" +
                            "           AND b.branchCode = bg.id\n" +
                            "           AND bg.status = '1'\n" +
                            "           AND ec.status = '1'\n" +
                         //   "           AND YEAR(c.bookingDate) = '2016'\n" +
                         //   "           AND MONTH(c.bookingDate) = '04'\n" +
                          //  "           and ec.expressCenterCode = '0111'\n" +
                         //   "           AND C.ZONECODE = '2'\n" +
                         //   "           AND C.BRANCHCODE = '4'\n" +
                            clvar._TownCode + clvar._Zone + clvar._Year + clvar._Month + clvar._Expresscentercode +" \n"+
                          //  "AND ec.expressCenterCode = '0111' \n" +
                            "         GROUP BY ec.expressCenterCode,\n" +
                            "                  c.destinationExpressCenterCode,\n" +
                            "                  c.serviceTypeName,\n" +
                            "                  c.orgin,\n" +
                            "                  c.chargedAmount,\n" +
                            "                  c.totalAmount,\n" +
                            "                  ec.name,\n" +
                            "                  ec.sname,\n" +
                            "                  b.name,\n" +
                            "                  c.bookingDate,\n" +
                            "                  bg.gst,\n" +
                            "                  c.gst) b\n" +
                            " GROUP BY b.expressCenterCode,\n" +
                            "          b.serviceTypeName,\n" +
                            "          b.branchGST,\n" +
                            "          b.ShortName,\n" +
                            "          b.BranchName,\n" +
                            "          b.Date,\n" +
                            "          b.ExpressCenter\n" +
                            " ORDER BY b.serviceTypeName";

                            */




                string sqlString = "select case\n" +
                "         when z.name in ('KHI', 'HDD', 'SKZ', 'UET') then\n" +
                "          'South'\n" +
                "         when z.name in ('FSD', 'LHE', 'MUX') then\n" +
                "          'Central'\n" +
                "         when z.name in ('PEW', 'RWP', 'ISB', 'GUJ') then\n" +
                "          'North'\n" +
                "       end 'Region',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       e.name 'Retail Office',\n" +
                //   "       e.expressCenterCode 'DB Code',\n" +
                "       e.sname 'Initial',\n" +
    /*
    "       sum(case\n" +
    "             when c.serviceTypeName in ('Flyer',\n" +
    "                                        'NTS',\n" +
    "                                        'HEC',\n" +
    "                                        'Bank to Bank',\n" +
    "                                        'Bulk Shipment',\n" +
    "                                        'overnight',\n" +
    "                                        'Return Service',\n" +
    "                                        'Same Day',\n" +
    "                                        'Second Day',\n" +
    "                                        'Smart Box',\n" +
    "                                        'Sunday & Holiday',\n" +
    "                                        'Smart Box',\n" +
    "                                        'Hand Carry',\n" +
    "                                        'Smart Cargo',\n" +
    "                                        'MB10',\n" +
    "                                        'MB2',\n" +
    "                                        'MB20',\n" +
    "                                        'MB30',\n" +
    "                                        'MB5') then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) 'Domestic',\n" +
    "       sum(case\n" +
    "             when c.serviceTypeName in\n" +
    "                  ('International_Box',\n" +
    "                   'International Cargo',\n" +
    "                   'International_Non-Doc',\n" +
    "                   'International_Non-Doc_Special_Hub_2014',\n" +
    "                   'International_Doc_Special_Hub',\n" +
    "                   'International_Doc') then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) 'International',\n" +
    "       sum(case\n" +
    "             when c.serviceTypeName = 'Logex' then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) 'Logex',\n" +
    "       sum(case\n" +
    "             when c.serviceTypeName in ('Expressions',\n" +
    "                                        'International Expressions',\n" +
    "                                        'Mango',\n" +
    "                                        'Mango Petty') then\n" +
    "              c.totalAmount + c.gst\n" +
    "             else\n" +
    "              0\n" +
    "           end) 'Expressions',\n" +
    "       sum(case\n" +
    "             when c.serviceTypeName = 'Aviation Sale' then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) 'Aviation Sale',\n" +
    "       sum(case\n" +
    "             when c.serviceTypeName = 'Road n Rail' then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) 'Road n Rail',\n" +
    "       CEILING(sum(case\n" +
    "                     when c.serviceTypeName = 'Expressions' then\n" +
    "                      c.totalAmount + c.gst\n" +
    "                     else\n" +
    "                      c.chargedAmount\n" +
    "                   end)) 'Grand Total'\n" +
     */

    "convert(varchar,cast(sum(case\n" +
    "             when c.serviceTypeName in ('Flyer',\n" +
    "                                        'NTS',\n" +
    "                                        'HEC',\n" +
    "                                        'Bank to Bank',\n" +
    "                                        'Bulk Shipment',\n" +
    "                                        'overnight',\n" +
    "                                        'Return Service',\n" +
    "                                        'Same Day',\n" +
    "                                        'Second Day',\n" +
    "                                        'Smart Box',\n" +
    "                                        'Sunday & Holiday',\n" +
    "                                        'Smart Box',\n" +
    "                                        'Hand Carry',\n" +
    "                                        'Smart Cargo',\n" +
    "                                        'MB10',\n" +
    "                                        'MB2',\n" +
    "                                        'MB20',\n" +
    "                                        'MB30',\n" +
    "                                        'MB5') then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) as money),-1) 'Domestic',\n" +
    "\n" +
    "           convert(varchar,cast(sum(case\n" +
    "             when c.serviceTypeName in\n" +
    "                  ('International_Box',\n" +
    "                   'International Cargo',\n" +
    "                   'International_Non-Doc',\n" +
    "                   'International_Non-Doc_Special_Hub_2014',\n" +
    "                   'International_Doc_Special_Hub',\n" +
    "                   'International_Doc') then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end)  as money),-1)  'International',\n" +
    "\n" +
    "           convert(varchar,cast(sum(case\n" +
    "             when c.serviceTypeName = 'Logex' then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) as money),-1) 'Logex',\n" +
    "\n" +
    "           convert(varchar,cast(sum(case\n" +
    "             when c.serviceTypeName in ('Expressions',\n" +
    "                                        'International Expressions',\n" +
    "                                        'Mango',\n" +
    "                                        'Mango Petty') then\n" +
    "              c.totalAmount + c.gst\n" +
    "             else\n" +
    "              0\n" +
    "           end) as money),-1) 'Expressions',\n" +
    "\n" +
    "           convert(varchar,cast(sum(case\n" +
    "             when c.serviceTypeName = 'Aviation Sale' then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) as money),-1) 'Aviation Sale',\n" +
    "\n" +
    "           convert(varchar,cast(sum(case\n" +
    "             when c.serviceTypeName = 'Road n Rail' then\n" +
    "              c.chargedAmount\n" +
    "             else\n" +
    "              0\n" +
    "           end) as money),-1) 'Road n Rail',\n" +
    "\n" +
    "           convert(varchar,cast(CEILING(sum(case\n" +
    "             when c.serviceTypeName = 'Expressions' then\n" +
    "              c.totalAmount + c.gst\n" +
    "             else\n" +
    "              c.chargedAmount\n" +
    "           end)) as money),-1) 'Grand Total' \n" +

                "  from Consignment c\n" +
                " inner join (select *\n" +
                "               from Riders r1\n" +
                "              where r1.expressCenterId + r1.riderCode not in\n" +
                "                    ('3711108',\n" +
                "                     '0430122',\n" +
                "                     '0411234',\n" +
                "                     '2311328',\n" +
                "                     '1812-1401',\n" +
                "                     '1812-1403',\n" +
                "                     '1812-1406',\n" +
                "                     '0230-1450',\n" +
                "                     '0222-1514',\n" +
                "                     '0222-1519',\n" +
                "                     '1813-1601',\n" +
                "                     '1813-1607',\n" +
                "                     '31121007',\n" +
                "                     '11111101',\n" +
                "                     '01113001',\n" +
                "                     'EXCENTER-FRFSD00029001',\n" +
                "                     '013111714')) r\n" +
                "    on c.riderCode = r.riderCode\n" +
                "   and c.orgin = r.branchId\n" +
                "  left join ExpressCenters e\n" +
                "    on r.expressCenterId = e.expressCenterCode\n" +
                " inner join creditclients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " where \n" +
                //  " year(c.accountReceivingDate) = '2016'\n" +
                //  "   and month(c.accountReceivingDate) = '03'\n" +
                "   cc.accountNo = '0'\n" +
                "   and r.userTypeId in ('EC', 'ECI', 'COUNTER')\n" +
                "   and isApproved = '1'\n" +
                //  "   and c.orgin = '4'\n" +
                clvar._Year + clvar._Month + clvar._Zone +
                " group by case\n" +
                "            when z.name in ('KHI', 'HDD', 'SKZ', 'UET') then\n" +
                "             'South'\n" +
                "            when z.name in ('FSD', 'LHE', 'MUX') then\n" +
                "             'Central'\n" +
                "            when z.name in ('PEW', 'RWP', 'ISB', 'GUJ') then\n" +
                "             'North'\n" +
                "          end,\n" +
                "          z.name,\n" +
                "          b.name,\n" +
                "e.name , e.sname \n" +
                //"e.expressCenterCode \n" +
                " ORDER BY 1, 2, 3, 4, 5";



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


        public DataSet Get_EC_NetSale(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {

                string sqlString = "select\n" +
                "case when z.name in ('KHI','HDD','SKZ','UET')then 'South'\n" +
                "when z.name in ('FSD','LHE','MUX') then 'Central'\n" +
                "when z.name in ('PEW','RWP','ISB','GUJ') then 'North' end 'Region',\n" +
                "z.name 'Origin Zone',\n" +
                "b.name 'Origin Branch',\n" +
                "e.name 'Retail Office',\n" +
                // "e.expressCenterCode 'DB Code',\n" +
                "e.sname 'Initial',\n" +
                "CEILING(sum(case when c.serviceTypeName in ('Flyer','NTS','HEC','Bank to Bank','Bulk Shipment','overnight','Return Service','Same Day','Second Day','Smart Box','Sunday & Holiday','Smart Box','Hand Carry','Smart Cargo','MB10','MB2','MB20','MB30','MB5') then c.chargedAmount/((tm.gst/100)+1) else 0 end)) 'Domestic' ,\n" +
                "CEILING(sum(case when c.serviceTypeName in ('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014','International_Doc_Special_Hub','International_Doc')then c.chargedAmount/((tm.gst/100)+1) else 0 end))  'International' ,\n" +
                "CEILING(sum(case when c.serviceTypeName = 'Logex' then c.chargedAmount/((tm.gst/100)+1) else 0 end))  'Logex' ,\n" +
                "CEILING(sum(case when c.serviceTypeName in ('Expressions','International Expressions','Mango','Mango Petty') then c.totalAmount else 0 end )) 'Expressions',\n" +
                "CEILING(sum(case when c.serviceTypeName = 'Aviation Sale' then c.chargedAmount/((tm.gst/100)+1) else 0 end)) 'Aviation Sale',\n" +
                "CEILING(sum(case when c.serviceTypeName = 'Road n Rail' then c.chargedAmount/((tm.gst/100)+1) else 0 end))  'Road n Rail' ,\n" +
                "CEILING(sum( case when  c.serviceTypeName='Expressions' then c.totalAmount else c.chargedAmount/((tm.gst/100)+1) end ))'Net Sale',\n" +
                "CEILING(sum(case when c.serviceTypeName='Expressions' then c.totalAmount+c.gst else c.chargedAmount end ))'Gross Sale'\n" +
                "from Consignment c\n" +
                "inner join\n" +
                "(select * from Riders r1 where\n" +
                "r1.expressCenterId+r1.riderCode not in\n" +
                "('3711108','0430122','0411234','2311328','1812-1401','1812-1403',\n" +
                "'1812-1406','0230-1450','0222-1514','0222-1519','1813-1601',\n" +
                "'1813-1607','31121007','11111101','01113001','EXCENTER-FRFSD00029001',\n" +
                "'013111714'))\n" +
                "r on c.riderCode = r.riderCode and c.orgin= r.branchId\n" +
                "left join ExpressCenters e on r.expressCenterId=e.expressCenterCode\n" +
                "inner join creditclients cc on c.creditClientId=cc.id\n" +
                "inner join Branches b on c.orgin=b.branchCode\n" +
                "inner join Zones z  on b.zoneCode=z.zoneCode\n" +
                "inner join ServiceTypes s on c.serviceTypeName=s.serviceTypeName\n" +
                "left join  (\n" +
                "select b.sname 'Branch',s.companyId,s.serviceTypeName,gst.gst from\n" +
                "(select * from BranchGST where\n" +
                "effectiveFrom< '2016-03-01'\n" +
                "and effectiveTo>'2016-03-01') gst\n" +
                "inner join ServiceTypes s on gst.companyId=s.companyId\n" +
                "inner join Branches b on gst.branchCode=b.branchCode)tm on b.sname=tm.Branch and s.companyId=tm.companyId and c.serviceTypeName=tm.serviceTypeName\n" +
                "where\n" +
                "cc.accountNo = '0'\n" +
                "and r.userTypeId in ('EC','ECI','COUNTER')\n" +
                "and isApproved ='1'\n" +
                clvar._Year + clvar._Month + clvar._Zone +
                "group by\n" +
                "case when z.name in ('KHI','HDD','SKZ','UET')then 'South'\n" +
                "when z.name in ('FSD','LHE','MUX') then 'Central'\n" +
                "when z.name in ('PEW','RWP','ISB','GUJ') then 'North' end ,\n" +
                "z.name ,\n" +
                "b.name ,\n" +
                "e.name ,\n" +
                //  "e.expressCenterCode ,\n" +
                "e.sname\n" +
                "ORDER BY 1,2,3,4,5\n" +
                "";

                /*
              //  convert(varchar,cast(sum( ) as money),-1)
                string sqlString = "select case\n" +
                "         when z.name in ('KHI', 'HDD', 'SKZ', 'UET') then\n" +
                "          'South'\n" +
                "         when z.name in ('FSD', 'LHE', 'MUX') then\n" +
                "          'Central'\n" +
                "         when z.name in ('PEW', 'RWP', 'ISB', 'GUJ') then\n" +
                "          'North'\n" +
                "       end 'Region',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       e.name 'Retail Office',\n" +
                "       e.expressCenterCode 'DB Code',\n" +
                "       e.sname 'Initial',\n" +
    "convert(varchar,cast(sum(case\n" +
    "             when c.serviceTypeName in ('Flyer',\n" + 
    "                                        'NTS',\n" + 
    "                                        'HEC',\n" + 
    "                                        'Bank to Bank',\n" + 
    "                                        'Bulk Shipment',\n" + 
    "                                        'overnight',\n" + 
    "                                        'Return Service',\n" + 
    "                                        'Same Day',\n" + 
    "                                        'Second Day',\n" + 
    "                                        'Smart Box',\n" + 
    "                                        'Sunday & Holiday',\n" + 
    "                                        'Smart Box',\n" + 
    "                                        'Hand Carry',\n" + 
    "                                        'Smart Cargo',\n" + 
    "                                        'MB10',\n" + 
    "                                        'MB2',\n" + 
    "                                        'MB20',\n" + 
    "                                        'MB30',\n" + 
    "                                        'MB5') then\n" + 
    "              c.chargedAmount\n" + 
    "             else\n" + 
    "              0\n" + 
    "           end) as money),-1) 'Domestic',\n" + 
    "\n" + 
    "           convert(varchar,cast(sum(case\n" + 
    "             when c.serviceTypeName in\n" + 
    "                  ('International_Box',\n" + 
    "                   'International Cargo',\n" + 
    "                   'International_Non-Doc',\n" + 
    "                   'International_Non-Doc_Special_Hub_2014',\n" + 
    "                   'International_Doc_Special_Hub',\n" + 
    "                   'International_Doc') then\n" + 
    "              c.chargedAmount\n" + 
    "             else\n" + 
    "              0\n" + 
    "           end)  as money),-1)  'International',\n" + 
    "\n" + 
    "           convert(varchar,cast(sum(case\n" + 
    "             when c.serviceTypeName = 'Logex' then\n" + 
    "              c.chargedAmount\n" + 
    "             else\n" + 
    "              0\n" + 
    "           end) as money),-1) 'Logex',\n" + 
    "\n" + 
    "           convert(varchar,cast(sum(case\n" + 
    "             when c.serviceTypeName in ('Expressions',\n" + 
    "                                        'International Expressions',\n" + 
    "                                        'Mango',\n" + 
    "                                        'Mango Petty') then\n" + 
    "              c.totalAmount + c.gst\n" + 
    "             else\n" + 
    "              0\n" + 
    "           end) as money),-1) 'Expressions',\n" + 
    "\n" + 
    "           convert(varchar,cast(sum(case\n" + 
    "             when c.serviceTypeName = 'Aviation Sale' then\n" + 
    "              c.chargedAmount\n" + 
    "             else\n" + 
    "              0\n" + 
    "           end) as money),-1) 'Aviation Sale',\n" + 
    "\n" + 
    "           convert(varchar,cast(sum(case\n" + 
    "             when c.serviceTypeName = 'Road n Rail' then\n" + 
    "              c.chargedAmount\n" + 
    "             else\n" + 
    "              0\n" + 
    "           end) as money),-1) 'Road n Rail',\n" + 



    "convert(varchar,cast(CEILING(sum(case\n" +
    "                     when c.serviceTypeName = 'Expressions' then\n" + 
    "                      c.totalAmount\n" + 
    "                     else\n" + 
    "                      c.chargedAmount / ((tm.gst / 100) + 1)\n" + 
    "                   end))  as money),-1) 'Net Sale',\n" + 
    "\n" + 
    "          convert(varchar,cast(CEILING(sum(case\n" + 
    "                     when c.serviceTypeName = 'Expressions' then\n" + 
    "                      c.totalAmount + c.gst\n" + 
    "                     else\n" + 
    "                      c.chargedAmount\n" + 
    "                   end)) as money),-1) 'Gross Sale' \n"+

                "  from Consignment c\n" +
                " inner join\n" +
              //  "--(select * from Riders r1 where\n" +
              //  "--r1.expressCenterId+r1.riderCode not in\n" +
                //"--('3711108','0430122','0411234','2311328','1812-1401','1812-1403',\n" +
                //"--'1812-1406','0230-1450','0222-1514','0222-1519','1813-1601',\n" +
                //"--'1813-1607','31121007','11111101','01113001','EXCENTER-FRFSD00029001',\n" +
                //"--'013111714'))\n" +
                "Riders r\n" +
                "    on c.riderCode = r.riderCode\n" +
                "   and c.orgin = r.branchId\n" +
                "  left join ExpressCenters e\n" +
                "    on r.expressCenterId = e.expressCenterCode\n" +
                " inner join creditclients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join ServiceTypes s\n" +
                "    on c.serviceTypeName = s.serviceTypeName\n" +
                "  left join (select b.sname 'Branch',\n" +
                "                    s.companyId,\n" +
                "                    s.serviceTypeName,\n" +
                "                    gst.gst\n" +
                "               from (select *\n" +
                "                       from BranchGST\n" +
                "                      where "+clvar._StartDate+") gst\n" +
                "              inner join ServiceTypes s\n" +
                "                 on gst.companyId = s.companyId\n" +
                "              inner join Branches b\n" +
                "                 on gst.branchCode = b.branchCode) tm\n" +
                "    on b.sname = tm.Branch\n" +
                "   and s.companyId = tm.companyId\n" +
                "   and c.serviceTypeName = tm.serviceTypeName\n" +
                " where \n" +
                "   isApproved = '1'\n" +
              //  "   and year(c.accountReceivingDate) = '2016'\n" +
              //  "   and month(c.accountReceivingDate) = '03'\n" +

                clvar._Year + clvar._Month + clvar._Zone +

                "   and cc.accountNo = '0'\n" +
                "   and r.userTypeId in ('EC', 'ECI', 'COUNTER')\n" +
                " group by case\n" +
                "            when z.name in ('KHI', 'HDD', 'SKZ', 'UET') then\n" +
                "             'South'\n" +
                "            when z.name in ('FSD', 'LHE', 'MUX') then\n" +
                "             'Central'\n" +
                "            when z.name in ('PEW', 'RWP', 'ISB', 'GUJ') then\n" +
                "             'North'\n" +
                "          end,\n" +
                "          z.name,\n" +
                "          b.name,\n" +
                "          e.name,\n" +
                "          e.expressCenterCode,\n" +
                "          e.sname\n" +
                " ORDER BY 1, 2, 3, 4, 5";
    */
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

        // EC Wise Report End

        public DataSet Get_Day_Wise_Nation_Wide_Recovery(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                /*
                string sql = "SELECT SUM(AMOUNTUSED) RECOVERAMOUNT,\n" +
                "       CONVERT(NVARCHAR, VOUCHERDATE, 105) RECEIPTDATE, ZONECODE\n" +
                "  FROM PAYMENTVOUCHERS\n" +
                " WHERE \n" +
                clvar._Year +  clvar._Month + clvar._TownCode + clvar._PaymentSource +
                " GROUP BY CONVERT(NVARCHAR, VOUCHERDATE, 105), ZONECODE\n" +
                " ORDER BY RECEIPTDATE";
                */

                /*
                string sql = "SELECT \n" +
              //  "SUM(p.AMOUNT) RECOVERAMOUNT,\n" +
                "CONVERT(varchar, CONVERT(money, SUM(p.AMOUNT)), 1)  RECOVERAMOUNT, \n" +
                "       CONVERT(NVARCHAR, p.VOUCHERDATE, 105) RECEIPTDATE,\n" +
                "       p.ZONECODE,\n" +
                "       z.name\n" +
                "  FROM PAYMENTVOUCHERS p, zones z\n" +
                " WHERE p.zoneCode = z.zoneCode and p.isbycreditclient = '1'\n" +
                clvar._Year + clvar._Month + clvar._TownCode + clvar._PaymentSource +
                " GROUP BY CONVERT(NVARCHAR, p.VOUCHERDATE, 105), p.ZONECODE, z.name\n" +
                " ORDER BY p.ZONECODE ASC";
                */

                // 06-05-2016 Added


                string sqlString = "select b.ReceiptDate,\n" +
                "       b.ZoneCode,\n" +
                "       b.BranchCode,\n" +
                "       b.zone,\n" +
                "       b.PaymentSourceId,\n" +
                "       CONVERT(varchar, CONVERT(money, sum(b.Amount)), 1) Amount,\n" +
                "       CONVERT(varchar, CONVERT(money, sum(b.AmountUsed)), 1) AmountUsed\n" +
                "  from ((Select convert(varchar(20), pv.voucherdate) ReceiptDate,\n" +
                "                pv.ZoneCode,\n" +
                "                pv.BranchCode,\n" +
                "                z.name zone,\n" +
                "                pv.ReceiptNo,\n" +
                "                pv.Amount,\n" +
                "                pv.AmountUsed,\n" +
                "                pv.PaymentSourceId\n" +
                "           FROM PaymentVouchers pv,\n" +
                "                ChequeStatus    CS,\n" +
                "                ChequeState     cs2,\n" +
                "                Zones           z\n" +
                "          where cs.PaymentVoucherId = pv.Id\n" +
                "            AND cs.ChequeStateId = cs2.Id\n" +
                "            and pv.PaymentSourceId in (2, 3, 4)\n" +
                "            and cs.ChequeStateId != '4'\n" +
                "            and cs.IsCurrentState = '1'\n" +
                "            and pv.ZoneCode = z.zoneCode) union all\n" +
                "        (Select convert(varchar(20), pv.voucherdate) ReceiptDate,\n" +
                "                pv.ZoneCode,\n" +
                "                pv.BranchCode,\n" +
                "                z.name zone,\n" +
                "                pv.ReceiptNo,\n" +
                "                pv.Amount,\n" +
                "                pv.AmountUsed,\n" +
                "                pv.PaymentSourceId\n" +
                "           FROM PaymentVouchers pv, Zones z\n" +
                "          where pv.PaymentSourceId in (1)\n" +
                "            and pv.ZoneCode = z.zoneCode)) b\n" +
                " where \n" +
                clvar._Year + clvar._Month + clvar._TownCode + clvar._PaymentSource +
                " group by b.ReceiptDate, b.ZoneCode, b.BranchCode,b.PaymentSourceId,b.zone";

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

        public DataSet Get_Day_Wise_Nation_Wide_Recovery_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT B.NAME        BRANCHNAME,\n" +
                                "       Z.NAME        ZONENAME,\n" +
                                "       CONVERT(NVARCHAR, P.VOUCHERDATE, 105) RECEIPTDATE,\n" +
                                "       C.NAME        CUSTOMERNAME,\n" +
                                "       C.ACCOUNTNO   ACCOUNTNUM,\n" +
                                "       P.RECEIPTNO   RECEIPTNUM,\n" +
                                "       P.REFNO       REFNUM,\n" +
                                "       P.AMOUNTUSED  AMOUNTRECOVER\n" +
                                "  FROM PAYMENTVOUCHERS P, CREDITCLIENTS C, BRANCHES B, ZONES Z\n" +
                                " WHERE P.CREDITCLIENTID = C.ID\n" +
                                "   AND P.BRANCHCODE = B.BRANCHCODE\n" +
                                "   AND P.ZONECODE = Z.ZONECODE\n" +
                                "   AND CONVERT(NVARCHAR, P.VOUCHERDATE, 105) = '" + clvar._Year + "'\n" +
                                "   AND P.ZONECODE = '" + clvar._Zone + "'\n" +
                                "   AND P.PAYMENTSOURCEID = '1' \n" +
                                " ORDER BY ReceiptNum ";


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

        public DataSet Get_ExpressCenters(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                //string sql = "select ec.name, ec.expressCenterCode\n" +
                //                "  from ExpressCenters ec, Consignment c\n" +
                //                " where ec.status = '1'\n" +
                //                "   and c.destinationExpressCenterCode = ec.expressCenterCode\n" +
                //                " group by ec.name, ec.expressCenterCode\n" +
                //                " order by name";


                string sql = "SELECT EC.NAME, EC.EXPRESSCENTERCODE\n" +
                "  FROM EXPRESSCENTERS EC, BRANCHES B\n" +
                " WHERE EC.STATUS = '1'\n" +
                "   AND B.BRANCHCODE = EC.BID\n" +
                "   AND B.STATUS = '1'\n" +
                " GROUP BY EC.NAME, EC.EXPRESSCENTERCODE\n" +
                " ORDER BY EC.NAME";



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

        public DataSet Get_COD_Consignment_Booking_Summary(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select CONVERT(NVARCHAR, cast(c.createdOn as Date), 105) 'Data Upload Date',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.AccountNo 'Account No.',\n" +
                "       c.consigner 'Shipper',\n" +
                "       count(c.refrenceNo) 'Total Uploaded Records',\n" +
                "       sum(case\n" +
                "             when c.isCNGenerated = '1' then\n" +
                "              1\n" +
                "             else\n" +
                "              0\n" +
                "           end) 'Total Booked'\n" +
                "  from cardConsignment c\n" +
                " inner join branches b\n" +
                "    on c.branchCode = b.branchCode\n" +
                " inner join zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " where \n" +

                "   cc.CODType in ('1', '3')\n" +
                clvar._StartDate + clvar._Zone + clvar._TownCode + clvar._ACNumber +
                // "-- and c.accountNo = #acno#\n" +
                " group by cast(c.createdOn as Date), b.name, c.AccountNo, c.consigner\n" +
                " order by cast(c.createdOn as Date), b.name, c.AccountNo, c.consigner";

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

        public DataSet Get_COD_Booking_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                /*
                string sql = "SELECT 'SN' + '-' + c.consignmentNumber ConsignmentNo,\n" +
                "       CASE\n" +
                "         WHEN c.cod = '1' THEN\n" +
                "          'COD'\n" +
                "         ELSE\n" +
                "          'Normal'\n" +
                "       END COD,\n" +
                "       (SELECT z.name FROM Zones z WHERE z.zoneCode = b.zoneCode) Zone,\n" +
                "       b.sname OriginBranch,\n" +
                "       (SELECT ec.name\n" +
                "          FROM ExpressCenters ec\n" +
                "         WHERE ec.expressCenterCode = c.originExpressCenter) OriginExpressCenterName,\n" +
                "       b2.sname,\n" +
                "       (SELECT z.name FROM Zones z WHERE z.zoneCode = b2.zoneCode) DestinationZone,\n" +
                "       (SELECT ec.name\n" +
                "          FROM ExpressCenters ec\n" +
                "         WHERE ec.expressCenterCode = c.destinationExpressCenterCode) DestinationEC,\n" +
                "       c.consigner CustomerName,\n" +
                "       c.consignerAccountNo AccountNumber,\n" +
                "       CONVERT(NVARCHAR, c.bookingDate, 105) bookingDate,\n" +
                "       c.totalAmount,\n" +
                "       c.chargedAmount,\n" +
                "       c.gst,\n" +
                "       c.consignee ConsigneeName,\n" +
                "       c.consigneePhoneNo,\n" +
                "       c.consigneeCNICNo,\n" +
                "       r.firstName + ' ' + r.lastName RiderName,\n" +
                "       rc.runsheetNumber,\n" +
                "       rc.receivedBy,\n" +
                "       CONVERT(NVARCHAR, rc.modifiedOn, 105) PODDate,\n" +
                "       (c.totalAmount + c.gst) CODAmount,\n" +
                "       '' Expression_CN_Ref,\n" +
                "       pv.Amount VoucherAmount,\n" +
                "       pv.ReceiptNo VoucherReceiptNo,\n" +
                "       CONVERT(NVARCHAR, pv.CreatedOn, 105) RREntryDate\n" +
                "  FROM PaymentVouchers     pv,\n" +
                "       Consignment         c,\n" +
                "       Riders              r,\n" +
                "       RunsheetConsignment rc,\n" +
                "       Branches            b,\n" +
                "       Branches            b2\n" +
                " WHERE c.consignmentNumber = pv.ConsignmentNo\n" +
                "   AND r.riderCode = c.riderCode\n" +
                "   AND rc.consignmentNumber = c.consignmentNumber\n" +
                "   AND b.branchCode = c.orgin\n" +
                "   AND b2.branchCode = c.destination\n" +
                "   AND c.cod = '1'\n" +
                clvar._StartDate + clvar._Zone + clvar._TownCode +
                " GROUP BY c.consignmentNumber,\n" +
                "          c.cod,\n" +
                "          b.zoneCode,\n" +
                "          b.sname,\n" +
                "          c.originExpressCenter,\n" +
                "          b2.sname,\n" +
                "          b2.zoneCode,\n" +
                "          c.destinationExpressCenterCode,\n" +
                "          c.consigner,\n" +
                "          c.consignerAccountNo,\n" +
                "          c.bookingDate,\n" +
                "          c.totalAmount,\n" +
                "          c.chargedAmount,\n" +
                "          c.gst,\n" +
                "          c.consignee,\n" +
                "          c.consigneePhoneNo,\n" +
                "          c.consigneeCNICNo,\n" +
                "          r.firstName + ' ' + r.lastName,\n" +
                "          rc.runsheetNumber,\n" +
                "          rc.receivedBy,\n" +
                "          rc.modifiedOn,\n" +
                "          pv.Amount,\n" +
                "          pv.ReceiptNo,\n" +
                "          pv.CreatedOn";
                */



                string sql = "select CONVERT(NVARCHAR, cast(c.createdOn as Date), 105) 'Created Date',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.AccountNo 'Account No.',\n" +
                "       c.consigner 'Shipper',\n" +
                "       c.refrenceNo 'Reference No.',\n" +
                "       c.consignee 'Consignee',\n" +
                "       c.address 'Consignee Address',\n" +
                "       c.ConsigneeContactNo 'Consignee Contact',\n" +
                "       c.CODAmount 'COD Amount',\n" +
                "       case\n" +
                "         when c.isCNGenerated = '1' then\n" +
                "          'Booked'\n" +
                "         else\n" +
                "          'Pending'\n" +
                "       end 'Booking Status'\n" +
                "  from cardConsignment c\n" +
                " inner join branches b\n" +
                "    on c.branchCode = b.branchCode\n" +
                " inner join zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " where \n" +
                //    " cast(c.createdOn as DATE) between #sdate# and #edate#\n" +
                "    cc.CODType in ('1', '3')\n" +
                //  "   and c.accountNo = #acno#\n" +

                clvar._StartDate + clvar._ACNumber + clvar._Zone + clvar._TownCode +
                " order by cast(c.createdOn as Date),\n" +
                "          b.name,\n" +
                "          c.AccountNo,\n" +
                "          case\n" +
                "            when c.isCNGenerated = '1' then\n" +
                "             'Booked'\n" +
                "            else\n" +
                "             'Pending'\n" +
                "          end";

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

        public DataSet Get_Consignment_Invoice_Discrepancy_Monthly(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select z.name Zone,\n" +
                "       b.name Branch,\n" +
                "       c.consignerAccountNo,\n" +
                "       cc.name Shipper,\n" +
                "       Case\n" +
                "         when cc.billingMode = '1' then\n" +
                "          'Auto'\n" +
                "         Else\n" +
                "          'Manual'\n" +
                "       end 'Billing Mode',\n" +
                "       c.consignee Consignee,\n" +
                "       c.consignmentNumber,\n" +
                "       cast(c.bookingDate as date) 'Booking Date',\n" +
                "       b.sname Orgin,\n" +
                "       bb.sname Dest,\n" +
                "       c.weight,\n" +
                "       c.pieces,\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' AND c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end as Service,\n" +
                "       c.totalAmount,\n" +
                "       c.gst,\n" +
                "       case\n" +
                "         when c.isApproved = '1' then\n" +
                "          'Approved'\n" +
                "         else\n" +
                "          'Not Approved'\n" +
                "       end as 'Approval Status'\n" +
                "  from Consignment c\n" +
                "  left join InvoiceConsignment ic\n" +
                "    on c.consignmentNumber = ic.consignmentNumber\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " inner join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                " inner join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join CreditClients cc\n" +
                "    on c.consignerAccountNo = cc.accountNo\n" +
                "   and c.orgin = cc.branchCode\n" +
                " where \n" +
                //    "   and month(c.bookingDate) = '03'\n" +
                "   ic.invoiceNumber is null\n" +
                "   and c.consignerAccountNo != '0'\n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode +

                " order by z.name, b.name, c.consignerAccountNo, c.bookingDate";

                /*
                string sql = "   SELECT CW.ORGIN,\n" +
                                "       'SN -' + CW.CONSIGNMENTNUMBER CN,\n" +
                                "       CW.CONSIGNER,\n" +
                                "       CW.CONSIGNERACCOUNTNO,\n" +
                                "       (SELECT B.NAME FROM BRANCHES B WHERE B.BRANCHCODE = CW.ORGIN) ORIGINBRANCH,\n" +
                                "       (SELECT B.NAME FROM BRANCHES B WHERE B.BRANCHCODE = CW.DESTINATION) DestinationBranch,\n" +
                                "       CONVERT(NVARCHAR, CW.BOOKINGDATE, 105) BOOKINGDATE,\n" +
                                "       CW.SERVICETYPENAME SERVICENAME,\n" +
                                "       CASE\n" +
                                "         WHEN CW.ISAPPROVED = '1' THEN\n" +
                                "          'APPROVED'\n" +
                                "         ELSE\n" +
                                "          'NOT APPROVED'\n" +
                                "       END STATUS,\n" +
                                "       CW.WEIGHT,\n" +
                                "       CASE WHEN CW.TOTALAMOUNT is null then '0' else CW.TOTALAMOUNT end TOTALAMOUNT,\n" +
                                "       CW.GST,\n" +                              
                                " CASE WHEN (CW.TOTALAMOUNT + CW.GST) is null then '0' else (CW.TOTALAMOUNT + CW.GST) end TarifAmount,\n" +   
                                "CASE WHEN cw.chargedAmount is null then '0' else cw.chargedAmount end ChargeAmount,\n" +   
                                "CASE WHEN (CW.TOTALAMOUNT + CW.GST) - cw.chargedAmount is null then '0' else (CW.TOTALAMOUNT + CW.GST) - cw.chargedAmount end DIFFERENCE\n" +                               
                                "  FROM CONSIGNMENT CW\n" +
                                " WHERE \n " +
                                clvar._status + clvar._Year + clvar._Month +
                                "   AND CW.CONSIGNMENTNUMBER NOT IN\n" +
                                clvar._Query + clvar._TownCode;
                 */

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

        public DataSet Get_Receipt_Redeem_Discrepancy_Report(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT B.NAME COLLECTIONBRANCH,\n" +
                                "       CC.NAME CUSTOMERNAME,\n" +
                                "       CC.ACCOUNTNO CUSTOMERACCOUNTNO,\n" +
                                "       CASE\n" +
                                "         WHEN CC.CENTRALIZEDCLIENT = '1' THEN\n" +
                                "          'CENTRALIZED'\n" +
                                "         ELSE\n" +
                                "          'NON-CENTRALIZED'\n" +
                                "       END CENTRALIZEDFLAG,\n" +
                                "       'PV-' + CONVERT(NVARCHAR(10), PV.ID) RECEIPTID,\n" +
                                "       CONVERT(NVARCHAR(10), PV.VOUCHERDATE, 105) RECEIPTDATEP,\n" +
                                "       PV.RECEIPTNO,\n" +
                                "       PV.REFNO,\n" +
                                "       PV.CHEQUENO,\n" +
                                "       PV.CHEQUEDATE,\n" +
                                "       PV.AMOUNT RECEIPTAMOUNT,\n" +
                                "       PV.AMOUNTUSED REDEEMAMOUNT\n" +
                                "  FROM PAYMENTVOUCHERS PV, CREDITCLIENTS CC, BRANCHES B\n" +
                                " WHERE PV.CREDITCLIENTID = CC.ID\n" +
                                "   AND PV.BRANCHCODE = B.BRANCHCODE \n" +
                                "AND (PV.AMOUNT - PV.AMOUNTUSED) > 0" +
                                clvar._status + clvar._Year + clvar._Month + clvar._TownCode;

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

        public DataSet Get_User_Report_Count(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT ZU.U_ID,\n" +
                                    "       ZU.U_NAME,\n" +
                                    "       ZU.NAME USERNAME,\n" +
                                    "       (SELECT COUNT(RL.REPORTID)\n" +
                                    "          FROM REPORTTRACKLOG RL\n" +
                                    "         WHERE RL.USERID = ZU.U_ID) REPORTCOUNT,\n" +
                                    "       (SELECT COUNT(UL.USERID)\n" +
                                    "          FROM USERTRACKLOG UL\n" +
                                    "         WHERE UL.USERID = ZU.U_ID) USERCOUNT\n" +
                                    "  FROM ZNI_USER1 ZU, REPORTTRACKLOG RL, USERTRACKLOG UL\n" +
                                    " WHERE ZU.U_ID = RL.USERID\n" +
                                    " GROUP BY ZU.U_ID, ZU.U_NAME, ZU.NAME\n" +
                                    " ORDER BY ZU.NAME ASC";

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

        public DataSet Get_Report_Count(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT RI.REPORTNAME, COUNT(RL.REPORTID) REPORTCOUNT\n" +
                                    "  FROM REPORT_INFO RI, REPORTTRACKLOG RL\n" +
                                    " WHERE RL.REPORTID = RI.REPORTID\n" +
                                    clvar._UserId +
                                    " GROUP BY RI.REPORTNAME \n" +
                                    " ORDER BY RI.REPORTNAME ASC ";

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

        public DataSet Get_Report_Day_Count(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select count(DISTINCT(CONVERT(VARCHAR(10), RL.DATETIME, 105))) DAYCOUNT from REPORTTRACKLOG RL \n" +
                                    "where \n" +
                                    clvar._UserName + clvar._StartDate;
                //      "AND DATETIME BETWEEN '2016-04-30' AND '2016-05-02 23:59:59.999'  ";

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


        public DataSet Get_All_User(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT z.U_ID, z.NAME+' ( '+z.U_NAME+' ) ('+d.Department+' )' NAME FROM Zni_User1 Z, Department d \n" +
                                    " WHERE STATUS = '1' AND Z.department= d.DepartmentID order by U_NAME ";

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

        public DataSet Get_UserReportDetail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString_old = "SELECT RI.REPORTNAME, CONVERT(NVARCHAR, RL.DATETIME) DATETIME \n" +
                                    "  FROM REPORTTRACKLOG RL, REPORT_INFO RI\n" +
                                    " WHERE RL.REPORTID = RI.REPORTID\n" +
                                    clvar._UserId + clvar._StartDate + " \n" +
                                    "ORDER BY DATETIME DESC";


                string sqlString = "SELECT RI.REPORTNAME, COUNT(RL.REPORTID) REPORTCOUNT, MM.Menu_Name, CONVERT(NVARCHAR, RL.DATETIME) DATETIME \n" +
                                    "FROM REPORT_INFO RI, REPORTTRACKLOG RL, ZNI_USER1 U, Main_Menu MM\n" +
                                    "WHERE RL.REPORTID = RI.REPORTID\n" +
                                    "AND RL.UserId = U.U_ID\n" +
                                    "AND U.Profile = MM.Menu_Id\n" +
                                    clvar._UserId + clvar._StartDate + " \n" +
                                    "GROUP BY RI.REPORTNAME, MM.Menu_Name, CONVERT(NVARCHAR, RL.DATETIME) \n" +
                                    "ORDER BY RI.REPORTNAME ASC ";

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

        public DataSet Get_BranchCollectionReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT b.name              CollectionBranch,\n" +
                                    "       cc.name             CustomerName,\n" +
                                    "       cc.accountNo        CustomerAccountNo,\n" +
                                    "       CASE\n" +
                                    "            WHEN cc.centralizedClient = '1' THEN 'CENTRALIZED'\n" +
                                    "            ELSE 'NON-CENTRALIZED'\n" +
                                    "       END                 CentralizedFlag,\n" +
                                    "       'PV-' + CONVERT(NVARCHAR(10), pv.Id) ReceiptID,\n" +
                                    "       CONVERT(NVARCHAR(10), pv.VoucherDate, 105) ReceiptDatep,\n" +
                                    "       pv.ReceiptNo,\n" +
                                    "       pv.RefNo,\n" +
                                    "       pv.ChequeNo,\n" +
                                    "       CONVERT(NVARCHAR(10), pv.ChequeDate, 105) ChequeDate, \n" +
                                    "       ps.Name PaymentSource,\n" +
                                    "       pv.Amount           ReceiptAmount,\n" +
                                    "       (\n" +
                                    "           SELECT cs2.Name\n" +
                                    "           FROM   ChequeState cs2\n" +
                                    "           WHERE  cs2.Id = cs.ChequeStateId\n" +
                                    "       )                   ChequeStatus\n" +
                                    "FROM   PaymentVouchers     pv,\n" +
                                    "       CreditClients       cc,\n" +
                                    "       Branches            b,\n" +
                                    "       ChequeStatus        cs,\n" +
                                    "       PaymentSource ps\n" +
                                    "WHERE  pv.CreditClientId = cc.id\n" +
                                    "       AND pv.BranchCode = b.branchCode\n" +
                                    "       AND cs.PaymentVoucherId = pv.Id\n" +
                                    "       AND pv.PaymentSourceId = ps.Id\n" +
                                    clvar._Year + clvar._Month + clvar._TownCode + clvar._PaymentSource +
                                    "GROUP BY\n" +
                                    "       b.name,\n" +
                                    "       cc.name,\n" +
                                    "       cc.accountNo,\n" +
                                    "       cc.centralizedClient,\n" +
                                    "       pv.Id,\n" +
                                    "       CONVERT(NVARCHAR(10), pv.VoucherDate, 105),\n" +
                                    "       pv.ReceiptNo,\n" +
                                    "       pv.RefNo,\n" +
                                    "       pv.ChequeNo,\n" +
                                    "       pv.ChequeDate,\n" +
                                    "       pv.Amount,\n" +
                                    "       cs.ChequeStateId,\n" +
                                    "       ps.name\n" +
                                    "       UNION ALL\n" +
                                    "SELECT b.name              CollectionBranch,\n" +
                                    "       cc.name             CustomerName,\n" +
                                    "       cc.accountNo        CustomerAccountNo,\n" +
                                    "       CASE\n" +
                                    "            WHEN cc.centralizedClient = '1' THEN 'CENTRALIZED'\n" +
                                    "            ELSE 'NON-CENTRALIZED'\n" +
                                    "       END                 CentralizedFlag,\n" +
                                    "       'PV-' + CONVERT(NVARCHAR(10), pv.Id) ReceiptID,\n" +
                                    "       CONVERT(NVARCHAR(10), pv.VoucherDate, 105) ReceiptDatep,\n" +
                                    "       pv.ReceiptNo,\n" +
                                    "       pv.RefNo,\n" +
                                    "       pv.ChequeNo,\n" +
                                    "       CONVERT(NVARCHAR(10), pv.ChequeDate, 105) ChequeDate, \n " +
                                    "       ps.Name PaymentSource,\n" +
                                    "       pv.Amount           ReceiptAmount,\n" +
                                    "       '' ChequeStatus\n" +
                                    "FROM   PaymentVouchers     pv,\n" +
                                    "       CreditClients       cc,\n" +
                                    "       Branches            b,\n" +
                                    "       PaymentSource ps\n" +
                                    "WHERE  pv.CreditClientId = cc.id\n" +
                                    "       AND pv.BranchCode = b.branchCode\n" +
                                    "       AND pv.PaymentSourceId = ps.Id\n" +
                                    clvar._Year + clvar._Month + clvar._TownCode + clvar._PaymentSource +
                                    "GROUP BY\n" +
                                    "       b.name,\n" +
                                    "       cc.name,\n" +
                                    "       cc.accountNo,\n" +
                                    "       cc.centralizedClient,\n" +
                                    "       pv.Id,\n" +
                                    "       CONVERT(NVARCHAR(10), pv.VoucherDate, 105),\n" +
                                    "       pv.ReceiptNo,\n" +
                                    "       pv.RefNo,\n" +
                                    "       pv.ChequeNo,\n" +
                                    "       pv.ChequeDate,\n" +
                                    "       pv.Amount,\n" +
                                    "       ps.name";


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

        public DataSet Get_ConsignmentTrackingHistory(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                /*
                string sqlString = "select CONVERT(NVARCHAR, d.transactionTime, 105) transactionTime,d.CStatus,d.consignmentNumber,d.LOCATION,d.Detail\n" +
                "from\n" +
                "(\n" +
                "select\n" +
                " ct.transactionTime,\n" +
                " case\n" +
                "   when ct.stateID = '1' then\n" +
                "    'NEW'\n" +
                "   when ct.stateID = '2' then\n" +
                "    'MANIFESTED'\n" +
                "   when ct.stateID = '3' then\n" +
                "    'BAGGED'\n" +
                "   when ct.stateID = '4' then\n" +
                "    'LOADED'\n" +
                "   when ct.stateID = '5' then\n" +
                "    'UNLOAD'\n" +
                "   when ct.stateID = '6' then\n" +
                "    'DEBAG'\n" +
                "   when ct.stateID = '7' then\n" +
                "    'DEMANIFEST'\n" +
                "   when ct.stateID = '8' then\n" +
                "    'RUNSHEET'\n" +
                "   when ct.stateID = '9' then\n" +
                "    'MAWB'\n" +
                "   when ct.stateID = '10' then\n" +
                "    'POD'\n" +
                "   when ct.stateID = '11' then\n" +
                "    'SHUFFLE_RIDER'\n" +
                "   when ct.stateID = '12' then\n" +
                "    'SHUFFLE_RUNSHEET'\n" +
                "   when ct.stateID = '13' then\n" +
                "    'TEMP_CONSIGNMENT'\n" +
                "   when ct.stateID = '14' then\n" +
                "    'TEMP_BAG'\n" +
                "   when ct.stateID = '15' then\n" +
                "    'INTL_STATUS update'\n" +
                "   when ct.stateID = '16' then\n" +
                "    'MAPPED'\n" +
                "   when ct.stateID = '17' then\n" +
                "    'MISROUTED'\n" +
                " end as CStatus,\n" +
                " ct.consignmentNumber,\n" +
                " ct.currentLocation as LOCATION,\n" +
                " case\n" +
                "   when ct.stateID = '1' then\n" +
                "    ' '\n" +
                "   when ct.stateID = '2' then\n" +
                "    ct.manifestNumber\n" +
                "   when ct.stateID = '3' then\n" +
                "    ct.bagNumber\n" +
                "   when ct.stateID = '4' then\n" +
                "    loadingNumber\n" +
                "   when ct.stateID = '5' then\n" +
                "    loadingNumber\n" +
                "   when ct.stateID = '6' then\n" +
                "    ct.bagNumber\n" +
                "   when ct.stateID = '7' then\n" +
                "    ct.manifestNumber\n" +
                "   when ct.stateID = '8' then\n" +
                "    ct.runsheetNumber + ', RIDER: ' + (case\n" +
                "      when r.riderCode is null then\n" +
                "       ''\n" +
                "      else\n" +
                "       r.riderCode\n" +
                "    end) + ' / ' + r.firstName + ' ' +\n" +
                "    (case\n" +
                "      when r.lastName is NULL then\n" +
                "       ''\n" +
                "      else\n" +
                "       r.lastName\n" +
                "    end) + ' '\n" +
                "   when ct.stateID = '9' then\n" +
                "    ' '\n" +
                "   when ct.stateID = '10' then\n" +
                "    'Status \"' +\n" +
                "    (case\n" +
                "      when ct.reason is null then\n" +
                "       ''\n" +
                "      else\n" +
                "       ct.reason\n" +
                "    end) + ' '\n" +
                "    + '\" Received By \"' + (case\n" +
                "      when rc.receivedBy is NULL then\n" +
                "       ''\n" +
                "      else\n" +
                "       rc.receivedBy\n" +
                "    end) + '\" Dated: ' +\n" +
                "    (case\n" +
                "      when ct.stateID = '10' then\n" +
                "       left(rc.deliveryDate, 10)\n" +
                "    end) +\n" +
                "    (case\n" +
                "      when rc.time is NULL then\n" +
                "       ''\n" +
                "      else\n" +
                "       right(rc.time, 8)\n" +
                "    end) +\n" +
                "    (case\n" +
                "      when rc.Reason is NULL then\n" +
                "       ''\n" +
                "      else\n" +
                "       rc.Reason\n" +
                "    end) + '\" '\n" +
                "   when ct.stateID = '11' then\n" +
                "    'SHUFFLE_RIDER'\n" +
                "   when ct.stateID = '12' then\n" +
                "    'SHUFFLE_RUNSHEET'\n" +
                "    when ct.stateID = '13' then\n" +
                "    'TEMP_CONSIGNMENT'\n" +
                "    when ct.stateID = '14' then\n" +
                "    'TEMP_BAG'\n" +
                "   when ct.stateID = '15' then\n" +
                "    'INTL_STATUS update'\n" +
                "    when ct.stateID = '16' then\n" +
                "    'MAPPED'\n" +
                "    when ct.stateID = '17' then\n" +
                "    'MISROUTED'\n" +
                "  end as Detail\n" +
                "  from ConsignmentsTrackingHistory ct\n" +
                "  left join RiderRunsheet rr\n" +
                "    on ct.runsheetNumber = rr.runsheetNumber\n" +
                "  left join Riders r\n" +
                "    on rr.riderCode + rr.expIdTemp = r.riderCode + r.expressCenterId\n" +
                "  left join RunsheetConsignment rc\n" +
                "    on ct.consignmentNumber = rc.consignmentNumber\n" +
                ") d\n" +
                "where \n"+
                " d.consignmentNumber = '231113288705'\n" +
              //  " d.consignmentNumber = '" + clvar._CNNumber + "'\n" +

                "group by d.transactionTime,d.CStatus,d.consignmentNumber,d.LOCATION,d.Detail";

                */

                string sqlString_ = "select CONVERT(NVARCHAR, d.transactionTime, 105) transactionTime, d.CStatus,d.consignmentNumber,d.LOCATION,d.Detail\n" +
                "from\n" +
                "(\n" +
                "select\n" +
                " ct.transactionTime,\n" +
                " case\n" +
                "   when ct.stateID = '1' then\n" +
                "    'BOOKED'\n" +
                "   when ct.stateID = '2' then\n" +
                "    'MANIFESTED'\n" +
                "   when ct.stateID = '3' then\n" +
                "    'BAGGED'\n" +
                "   when ct.stateID = '4' then\n" +
                "    'LOADED'\n" +
                "   when ct.stateID = '5' then\n" +
                "    'UNLOAD'\n" +
                "   when ct.stateID = '6' then\n" +
                "    'DEBAG'\n" +
                "   when ct.stateID = '7' then\n" +
                "    'DEMANIFEST'\n" +
                "   when ct.stateID = '8' then\n" +
                "    'RUNSHEET'\n" +
                "   when ct.stateID = '9' then\n" +
                "    'MAWB'\n" +
                "   when ct.stateID = '10' then\n" +
                "    'POD'\n" +
                "   when ct.stateID = '11' then\n" +
                "    'SHUFFLE_RIDER'\n" +
                "   when ct.stateID = '12' then\n" +
                "    'SHUFFLE_RUNSHEET'\n" +
                "   when ct.stateID = '13' then\n" +
                "    'TEMP_CONSIGNMENT'\n" +
                "   when ct.stateID = '14' then\n" +
                "    'TEMP_BAG'\n" +
                "   when ct.stateID = '15' then\n" +
                "    ct.reason\n" +
                "   when ct.stateID = '16' then\n" +
                "    'MAPPED'\n" +
                "   when ct.stateID = '17' then\n" +
                "    'MISROUTED'\n" +
                " end as CStatus,\n" +
                " ct.consignmentNumber,\n" +
                " ct.currentLocation as LOCATION,\n" +
                " case\n" +
                "   when ct.stateID = '1' then\n" +
                "    ' Conginment has been Booked on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '2' then\n" +
                "    'Consignment has been manifested. Manifest Number : '+ct.manifestNumber+' on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '3' then\n" +
                "    'Consignment has been Bagged. Bag Number : '+ct.bagNumber+' on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '4' then\n" +
                "\n" +
                "    'Consignment has been loaded. VAN Number : '+loadingNumber+' on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '5' then\n" +
                "    'Consignment Has been Recieved VAN Number: '+loadingNumber+' on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '6' then\n" +
                "    ct.bagNumber\n" +
                "   when ct.stateID = '7' then\n" +
                "    'Consignment has been De-Manifest Manifest Number: '+ct.manifestNumber+' on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '8' then\n" +
                "   'Consignment is out for delivery. Runsheet Number : '+ct.runsheetNumber+' Rider Name : '+r.firstName+' (' +r.riderCode+') on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '9' then\n" +
                "    'Consignment has been MAWB - Manifested AWB Number: '+ct.mawbnumber+' on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "   when ct.stateID = '10' then\n" +
                "    'Consignment has been \"' +\n" +
                "    (case\n" +
                "      when ct.reason is null then\n" +
                "       ''\n" +
                "      else\n" +
                "       ct.reason\n" +
                "    end) + ' '\n" +
                "    + '\" Received By \"' + (case\n" +
                "      when rc.receivedBy is NULL then\n" +
                "       ''\n" +
                "      else\n" +
                "       rc.receivedBy\n" +
                "    end) + '\" Dated: ' +\n" +
                "    (case\n" +
                "      when ct.stateID = '10' then\n" +
                "       left(rc.deliveryDate, 10)\n" +
                "    end) +\n" +
                "    (case\n" +
                "      when rc.time is NULL then\n" +
                "       ''\n" +
                "      else\n" +
                "       right(rc.time, 8)\n" +
                "    end) +\n" +
                "    (case\n" +
                "      when rc.Reason is NULL then\n" +
                "       ''\n" +
                "      else\n" +
                "       rc.Reason\n" +
                "    end) + '\" '\n" +
                "   when ct.stateID = '11' then\n" +
                "    'SHUFFLE_RIDER'\n" +
                "   when ct.stateID = '12' then\n" +
                "    'SHUFFLE_RUNSHEET'\n" +
                "    when ct.stateID = '13' then\n" +
                "    'Temp Consignment has booked on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "    when ct.stateID = '14' then\n" +
                "    'TEMP_BAG'\n" +
                "   when ct.stateID = '15' then\n" +
                "    'Consignment has been '+ct.reason+' ' +CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "    when ct.stateID = '16' then\n" +
                "    'Consignment has been Mapped with Company '+ct.reason+' ' +CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "    when ct.stateID = '17' then\n" +
                "    'Consignment has been Marked as MisRouted on '+CONVERT(NVARCHAR, transactionTime, 105)+' in '+ct.currentLocation\n" +
                "  end as Detail\n" +
                "  from ConsignmentsTrackingHistory ct\n" +
                "  left join RiderRunsheet rr\n" +
                "    on ct.runsheetNumber = rr.runsheetNumber\n" +
                "  left join Riders r\n" +
                "    on rr.riderCode + rr.expIdTemp = r.riderCode + r.expressCenterId\n" +
                "  left join RunsheetConsignment rc\n" +
                "    on ct.consignmentNumber = rc.consignmentNumber\n" +
                ") d\n" +
                "where d.consignmentNumber = '" + clvar._CNNumber + "'\n" +
                "group by d.transactionTime,d.CStatus,d.consignmentNumber,d.LOCATION,d.Detail";

                string sql = " \n"
               + "SELECT mcts.TrackingStatus, B.* \n"
               + "FROM   ( \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '1' THEN 'New' \n"
               + "                       ELSE '' \n"
               + "                  END     Booked, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '1' THEN ( \n"
               + "                                SELECT +'Consignment No: ' + c.consignmentNumber  \n"
               + "                                       + \n"
               + "                                       ' was booked on :' + CONVERT(VARCHAR(11), c.bookingDate, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   Consignment c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.consignmentNumber = cth.consignmentNumber \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '1' \n"
               + "           GROUP BY \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '2' THEN 'Manifested' \n"
               + "                       ELSE '' \n"
               + "                  END     Manifested, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '2' THEN ( \n"
               + "                                SELECT +'Manifest No :' + c.manifestNumber + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   mnp_Manifest c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '2' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '3' THEN 'Bagging' \n"
               + "                       ELSE '' \n"
               + "                  END     Bagging, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '3' THEN ( \n"
               + "                                SELECT +'Bag No: ' + c.bagNumber +  \n"
               + "                                       ' was Generated on :' + \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   Bag c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.bagNumber = cth.bagNumber \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '3' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation, \n"
               + "                  cth.bagNumber \n"
               + "            \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '4' THEN 'Loading' \n"
               + "                       ELSE '' \n"
               + "                  END     Bagging, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '4' THEN ( \n"
               + "                                SELECT +'Loading No :' + CONVERT(VARCHAR, c.id)  \n"
               + "                                       + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   MnP_Loading c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.loadingNumber) \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '4' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation, \n"
               + "                  cth.bagNumber, \n"
               + "                  cth.loadingNumber \n"
               + "            \n"
               + "            \n"
               + "            \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '18' THEN 'Arrival' \n"
               + "                       ELSE '' \n"
               + "                  END     Arrival, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '18' THEN ( \n"
               + "                                SELECT +'Arrival No :' + CONVERT(NVARCHAR, c.Id)  \n"
               + "                                       + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   ArrivalScan c, \n"
               + "                                       ArrivalScan_Detail asd, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  c.Id = asd.ArrivalID \n"
               + "                                       AND CONVERT(NVARCHAR, c.createdBy) =  \n"
               + "                                           CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND asd.consignmentNumber = cth.consignmentNumber \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '18' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation, \n"
               + "                  cth.bagNumber, \n"
               + "                  cth.loadingNumber \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '6' THEN 'DeBagging' \n"
               + "                       ELSE '' \n"
               + "                  END     Bagging, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '6' THEN ( \n"
               + "                                SELECT +'DeBagging  was Generated on :' +  \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   Bag c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.bagNumber = cth.bagNumber \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '6' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation, \n"
               + "                  cth.bagNumber \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '7' THEN 'Manifested' \n"
               + "                       ELSE '' \n"
               + "                  END     Manifested, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '7' THEN ( \n"
               + "                                SELECT +'DeManifest was Generated on :' +  \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   mnp_Manifest c, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '7' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '8' THEN 'Runsheet' \n"
               + "                       ELSE '' \n"
               + "                  END     Runsheet, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '8' THEN ( \n"
               + "                                SELECT +'Runsheet No :' + c.runsheetNumber + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name  \n"
               + "                                       + ' against Rider :' + c.routeCode + ' -'  \n"
               + "                                       + cth.riderName \n"
               + "                         FROM   Runsheet c, RunsheetConsignment rc, \n"
               + "                                        \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND c.runsheetNumber = rc.runsheetNumber \n"
               + "                                       AND c.routeCode = rc.RouteCode \n"
               + "                                       AND c.branchCode = rc.branchcode \n"
               + "                                       AND c.runsheetNumber = cth.runsheetNumber \n"
               + "                                       AND cth.consignmentNumber = rc.consignmentNumber    ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '8' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation, \n"
               + "                  cth.runsheetNumber, \n"
               + "                  cth.riderName \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '10' THEN 'POD' \n"
               + "                       ELSE '' \n"
               + "                  END Runsheet, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.stateID = '10' \n"
               + "           AND LEN(cth.riderName) <> 0 THEN ( \n"
               + "                   SELECT 'Consignment has been \"' + cth.reason \n"
               + "                          -- \n"
               + "                          + ' ' \n"
               + "                          + '\" Received By \"' + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN rc.receivedBy IS NULL THEN 'Not Feeded' \n"
               + "                                   ELSE rc.receivedBy \n"
               + "                              END \n"
               + "                          ) + '\" Dated: ' + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN cth.stateID = '10' THEN LEFT(rc.deliveryDate, 10) \n"
               + "                              END \n"
               + "                          ) + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN rc.time IS NULL THEN '' \n"
               + "                                   ELSE RIGHT(rc.time, 8) \n"
               + "                              END \n"
               + "                          ) + (CASE WHEN rc.Reason IS NULL THEN '' ELSE rc.Reason END) \n"
               + "                          + '\" ' \n"
               + "                   FROM   runsheetconsignment rc, \n"
               + "                          runsheet r1 \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.branchcode = rc.branchcode \n"
               + "                          AND r1.routecode = rc.routecode \n"
               + "                          AND r1.createdBy = rc.createdBy \n"
               + "               ) \n"
               + "               WHEN cth.stateID = '10' \n"
               + "           AND LEN(cth.riderName) = 0 \n"
               + "           AND cth.reason = \n"
               + "               'UNDELIVERED' THEN ( \n"
               + "                   SELECT +' Consignment is ' + cth.reason + \n"
               + "                          ' .For RunsheetNumber :' + cth.runsheetNumber + \n"
               + "                          ' due to Following Reason :' + ( \n"
               + "                              CASE  \n"
               + "                                   WHEN ISNULL(rc.Reason, '0') = '0' THEN '' \n"
               + "                                   ELSE ( \n"
               + "                                            SELECT v.AttributeValue \n"
               + "                                            FROM   rvdbo.Lookup v \n"
               + "                                            WHERE  v.Id = rc.Reason \n"
               + "                                        ) \n"
               + "                              END \n"
               + "                          )  \n"
               + "                          + '\" ' \n"
               + "                   FROM   runsheetconsignment rc, \n"
               + "                          runsheet r1 \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber \n"
               + "                          AND r1.branchcode = rc.branchcode \n"
               + "                          AND r1.routecode = rc.routecode \n"
               + "                          AND r1.createdBy = rc.createdBy \n"
               + "               ) \n"
               + "               WHEN cth.stateID = '10' \n"
               + "           AND cth.reason = 'DELIVERED' \n"
               + "           AND LEN(cth.riderName)  \n"
               + "               = 0 THEN (cth.reason) \n"
               + "               ELSE '' \n"
               + "               END Detail \n"
               + "               FROM ConsignmentsTrackingHistory cth \n"
               + "               WHERE cth.consignmentNumber = '214115813890' \n"
               + "           AND cth.stateID = '10' \n"
               + "               GROUP BY \n"
               + "               cth.manifestNumber, \n"
               + "           cth.consignmentNumber, \n"
               + "           cth.StateID, \n"
               + "           cth.transactionTime, \n"
               + "           cth.currentLocation, \n"
               + "           cth.runsheetNumber, \n"
               + "           cth.riderName, \n"
               + "           cth.reason  \n"
               + "           UNION ALL \n"
               + "           SELECT cth.transactionTime, \n"
               + "                  cth.stateID, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  CASE  \n"
               + "                       WHEN StateID = '20' THEN 'Airport Material Arival' \n"
               + "                       ELSE '' \n"
               + "                  END     Arrival, \n"
               + "                  cth.currentLocation, \n"
               + "                  CASE  \n"
               + "                       WHEN cth.StateID = '20' THEN ( \n"
               + "                                SELECT +'Material Arrival No :' + CONVERT(NVARCHAR, c.ArrivalID)  \n"
               + "                                       + \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)  \n"
               + "                                       + \n"
               + "                                       ' by User :' + zu.Name + ' on Location :'  \n"
               + "                                       + ec.name \n"
               + "                                FROM   MNP_MaterialArrival c, \n"
               + "                                       MNP_MaterialArrivalDetail asd, \n"
               + "                                       ZNI_USER1 zu, \n"
               + "                                       Branches b, \n"
               + "                                       ExpressCenters ec \n"
               + "                                WHERE  c.ArrivalID = asd.ArrivalID \n"
               + "                                       AND CONVERT(NVARCHAR, c.createdBy) =  \n"
               + "                                           CONVERT(NVARCHAR, zu.U_ID) \n"
               + "                                       AND zu.branchcode = b.branchCode \n"
               + "                                       AND zu.ExpressCenter = ec.expressCentercode \n"
               + "                                       AND asd.ConsignmentNumber = cth.consignmentNumber \n"
               + "                            ) \n"
               + "                       ELSE '' \n"
               + "                  END     Detail \n"
               + "           FROM   ConsignmentsTrackingHistory cth \n"
               + "           WHERE  cth.consignmentNumber = '214115813890' \n"
               + "                  AND cth.stateID = '20' \n"
               + "           GROUP BY \n"
               + "                  cth.manifestNumber, \n"
               + "                  cth.consignmentNumber, \n"
               + "                  cth.StateID, \n"
               + "                  cth.transactionTime, \n"
               + "                  cth.currentLocation, \n"
               + "                  cth.bagNumber, \n"
               + "                  cth.loadingNumber \n"
               + "       )b \n"
               + "       RIGHT OUTER JOIN MNP_ConsginmentTrackingStatus mcts \n"
               + "on  mcts.StatusID = b.stateID \n"
               + "WHERE mcts.[Active] ='1' \n"
               + "ORDER BY convert(int,SORTORDER), b.transactionTime";



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

        public DataSet Get_ConsignmentTrackingHistory_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                /*
                string sqlString = "\n" +
                "select c.consignmentNumber,\n" +
                "       c.consigner,\n" +
                "       c.consignee consignee,\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) bookingDate,\n" +
                "       c.weight,\n" +
                "       c.serviceTypeName,\n" +
                "       rc.receivedBy ReceivedBy,\n" +
                "       rc.Status CurrentStatus,\n" +
                "       rc.deliveryDate DeliveryTime,\n" +
                "       ec.name Destination,\n" +
                "       c.consignerAccountNo AccoutNo,\n" +
                "       b.name orign,\n" +
                "       r.firstName + ' (' + rrs.riderCode + ' )' delievryRider\n" +
                "\n" +
                "  from Consignment         c,\n" +
                "       RunsheetConsignment rc,\n" +
                "       ExpressCenters      ec,\n" +
                "       Branches            b,\n" +
                "       Riders              r,\n" +
                "       RiderRunsheet       rrs\n" +
                " where c.consignmentNumber = rc.consignmentNumber\n" +
                "   and c.destinationExpressCenterCode = ec.expressCenterCode\n" +
                "   and c.orgin = b.branchCode\n" +
                "   and rc.runsheetNumber = rrs.runsheetNumber\n" +
                "   and rrs.riderCode = r.riderCode\n" +
                "   and rrs.expIdTemp = r.expressCenterId\n" +
                "   and c.consignmentNumber = '" + clvar._CNNumber + "' ";
                */


                string sqlString = "select c.consignmentNumber,\n" +
                "       c.consigner,\n" +
                "       c.consignee consignee,\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) bookingDate,\n" +
                "       c.weight,\n" +
                "       c.serviceTypeName,\n" +
                "       r.receivedBy ReceivedBy,\n" +
                "       r.CurrentStatus CurrentStatus,\n" +
                "       DeliveryTime,\n" +
                "       ec.name Destination,\n" +
                "       c.consignerAccountNo AccoutNo,\n" +
                "       b.name orign,\n" +
                "       r.delievryRider delievryRider\n" +
                "  from Consignment c\n" +
                " inner join ExpressCenters ec\n" +
                "    on c.destinationExpressCenterCode = ec.expressCenterCode\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                "  left join (select rc.consignmentNumber,\n" +
                "                    rc.receivedBy ReceivedBy,\n" +
                "                    rc.Status CurrentStatus,\n" +
                "                    rc.deliveryDate DeliveryTime,\n" +
                "                    r.firstName + ' (' + rrs.riderCode + ' )' delievryRider\n" +
                "               from RunsheetConsignment rc, Riders r, RiderRunsheet rrs\n" +
                "              where rc.runsheetNumber = rrs.runsheetNumber\n" +
                "                and rrs.riderCode = r.riderCode\n" +
                "                and rrs.expIdTemp = r.expressCenterId) r\n" +
                "    on c.consignmentnumber = r.consignmentNumber\n" +
                " where c.consignmentNumber = '" + clvar._CNNumber + "' ";






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

        public DataSet Get_CustomerLedger(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select b.InvoiceMonth,\n" +
                "       b.invoicenumber,\n" +
                "       Receiptid, b.status,\n" +
                "       case\n" +
                "         when convert(varchar(10), b.ReceiptDate) = '1900-01-01' then\n" +
                "          ''\n" +
                "         else\n" +
                "          b.ReceiptDate\n" +
                "\n" +
                "       end ReceiptDate,\n" +
                "       b.InvoiceRedeemID,\n" +
                "       b.ReceiptNumber,\n" +
                "       b.ReferenceNumber,\n" +
                "       CONVERT(varchar, CONVERT(money, b.DebitAmount), 1) DebitAmount,\n" +
                "       CONVERT(varchar, CONVERT(money, b.CreditAmount), 1) CreditAmount,\n" +
                "       b.AggAmount\n" +
                "  FROM (\n" +
                "\n" +
                "        Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "                convert(varchar(20), i.invoicenumber) invoicenumber,\n" +
                "                convert(varchar(20), pv.id) Receiptid,\n" +
                "'Collection' status, \n" +
                "                convert(varchar(20), pv.voucherdate) ReceiptDate,\n" +
                "                convert(varchar(20), ir.id) InvoiceRedeemID,\n" +
                "                convert(varchar(20), pv.ReceiptNo) ReceiptNumber,\n" +
                "                CASE\n" +
                "                  WHEN pv.refno = '' THEN\n" +
                "                   '0'\n" +
                "                  ELSE\n" +
                "                   pv.refno\n" +
                "                end ReferenceNumber,\n" +
                "                0 DebitAmount,\n" +
                "                convert(varchar(20), ir.amount) CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          FROM PaymentVouchers pv,\n" +
                "                InvoiceRedeem   ir,\n" +
                "                invoice         i,\n" +
                "                ChequeStatus    CS,\n" +
                "                ChequeState     cs2,\n" +
                "                CreditClients   cc\n" +
                "         where cc.id = i.clientId\n" +
                "           and pv.Id = ir.PaymentVoucherId\n" +
                "           AND cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and i.invoicenumber = ir.invoiceNo\n" +
                //   " AND cc.accountNo = '9001' AND cast(i.startDate as date) BETWEEN '2015-01-01' AND '2016-04-25'\n" +
                clvar._ACNumber + clvar._TownCode + clvar._Zone +

                "  UNION ALL\n" +
                "        Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "                convert(varchar(20), i.invoicenumber) invoicenumber,\n" +
                "                '0' Receiptid,\n" +
                " 'Invoiced' status, \n" +
                "                '' ReceiptDate,\n" +
                "                '0' InvoiceRedeemID,\n" +
                "                '0' ReceiptNumber,\n" +
                "                '0' ReferenceNumber,\n" +
                "                i.totalAmount DebitAmount,\n" +
                "                0 CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          from invoice i, CreditClients cc\n" +
                "         where i.clientid = cc.id\n" +
                //   " AND cc.accountNo = '9001' AND cast(i.startDate as date) BETWEEN '2015-01-01' AND '2016-04-25'\n" +
                clvar._ACNumber + clvar._TownCode + clvar._Zone +
                " union all\n" +
                "\n" +
                "        Select '' InvoiceMonth,\n" +
                "                '' invoicenumber,\n" +
                "                convert(varchar(20), pv.id) Receiptid,\n" +
                " 'Collection' Status, \n" +
                "                convert(varchar(20), pv.voucherdate) ReceiptDate,\n" +
                "                '0' InvoiceRedeemID,\n" +
                "                convert(varchar(20), pv.ReceiptNo) ReceiptNumber,\n" +
                "                convert(varchar(20), ISNULL(pv.refno, '0')) ReferenceNumber,\n" +
                "                0 DebitAmount,\n" +
                "                Amount - AmountUsed CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          from PaymentVouchers pv,\n" +
                "                ChequeStatus    CS,\n" +
                "                ChequeState     cs2,\n" +
                "                CreditClients   cc\n" +
                "         where cc.id = pv.creditclientid\n" +
                "           and cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and cs.chequestateid in ('1', '2')\n" +
                "           and pv.paymentsourceid in ('1')\n" +
                "           and (Amount - AmountUsed) > 1\n" +
                "           and pv.paymentsourceid in ('1', '2')\n" +
                //    " AND cc.accountNo = '9001' AND cast(pv.voucherdate as date) BETWEEN '2015-01-01' AND '2016-04-25'\n" +
                clvar._ACNumber + clvar._TownCode + clvar._Zone +
                "        union all\n" +
                "\n" +
                "        select convert(varchar(11), i.startDate, 106) InvoiceDate,\n" +
                "                convert(varchar(20), i.invoiceNumber) InvoiceNumber,\n" +
                "                convert(varchar(20), g.id) ReceiptID,\n" +
                " 'Adjustment' Status, \n" +
                "                convert(varchar(20), g.VoucherDate) ReceiptDate,\n" +
                "                '0' InvoiceRedeemID,\n" +
                "                '0' ReceiptNumber,\n" +
                "                '0' ReferenceNumber,\n" +
                "                '0' DebitAmount,\n" +
                "                convert(varchar(20), g.Amount) CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          from GeneralVoucher g,\n" +
                "                Branches       b,\n" +
                "                Zones          z,\n" +
                "                PaymentTypes   d,\n" +
                "                CreditClients  cc,\n" +
                "                Invoice        i\n" +
                "\n" +
                "         where g.InvoiceNo = i.invoiceNumber\n" +
                "           and g.CreditClientId = cc.id\n" +
                "           and g.PaymentTypeId = d.Id\n" +
                "           and cc.ZoneCode = z.zoneCode\n" +
                "           and cc.BranchCode = b.branchCode\n" +
                //    " AND c.accountNo = '9001'\n" +
                //    "  AND cast(i.startDate as date) BETWEEN '2015-01-01' AND '2016-04-25'\n" +
                clvar._ACNumber + clvar._TownCode + clvar._Zone +
                "        union all\n" +
                "        Select '' InvoiceMonth,\n" +
                "                '' invoicenumber,\n" +
                "                pv.id Receiptid,\n" +
                " 'Cheque Dishonor' Status, \n" +
                "                convert(varchar(20), pv.voucherdate) ReceiptDate,\n" +
                "                '0' InvoiceRedeemID,\n" +
                "                pv.ReceiptNo ReceiptNumber,\n" +
                "                pv.refno ReferenceNumber,\n" +
                "                Amount DebitAmount,\n" +
                "                0 CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          from PaymentVouchers pv,\n" +
                "                ChequeStatus    CS,\n" +
                "                ChequeState     cs2,\n" +
                "                CreditClients   cc\n" +
                "         where cc.id = pv.creditclientid\n" +
                "           and cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and cs.chequestateid in ('3', '4')\n" +
                "           and pv.paymentsourceid in ('2')\n" +
                //    " AND cc.accountNo = '9001' AND cast(pv.voucherdate as date) BETWEEN '2015-01-01' AND '2016-04-25'\n" +
                clvar._ACNumber + clvar._TownCode + clvar._Zone +
                "   and cs.IsCurrentState = '1'\n" +
                "\n" +
                "        ) b\n" + clvar._StartDate +
                " group by b.InvoiceMonth,\n" +
                "          b.invoicenumber,\n" +
                "          Receiptid,\n" +
                "\n" +
                "          b.ReceiptDate,\n" +
                "          b.InvoiceRedeemID,\n" +
                " b.status, \n" +
                "          b.ReceiptNumber,\n" +
                "          b.ReferenceNumber,\n" +
                "          b.DebitAmount,\n" +
                "          b.CreditAmount,\n" +
                "          b.AggAmount\n" +
                " order by cast(b.InvoiceMonth as date)  ";

                /*
                string sqlString = "Select \n" +
                "       CONVERT(NVARCHAR, InvoiceMonth, 105) InvoiceMonth,\n" +
                "       invoicenumber,\n" +
                "       Receiptid,\n" +
                "       CONVERT(NVARCHAR, ReceiptDate, 105) ReceiptDate,\n" +
                "       InvoiceRedeemID,\n" +
                "       ReceiptNumber,\n" +
                "       ReferenceNumber,\n" +
             //   "       Sum(DebitAmount) DebitAmount,\n" +
             //   "       Sum(CreditAmount) CreditAmount\n" +
                "case when Sum(DebitAmount) IS null then 0 else Sum(DebitAmount) end DebitAmount,\n" +
                "case when Sum(CreditAmount) IS null then 0 else Sum(CreditAmount) end CreditAmount\n" +


                "  from (\n" +
                "        Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "                i.invoicenumber,\n" +
                "                '' Receiptid,\n" +
                "                '' ReceiptDate,\n" +
                "                '' InvoiceRedeemID,\n" +
                "                '' ReceiptNumber,\n" +
                "                '' ReferenceNumber,\n" +
                "                i.totalAmount DebitAmount,\n" +
                "                0 CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          from invoice i, CreditClients cc\n" +
                "         where i.clientid = cc.id\n" +
                clvar._ACNumber +
                "        union all\n" +
                "        Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "                i.invoicenumber,\n" +
                "                pv.id Receiptid,\n" +
                "                pv.voucherdate ReceiptDate,\n" +
                "                ir.id InvoiceRedeemID,\n" +
                "                pv.ReceiptNo ReceiptNumber,\n" +
                "                pv.refno ReferenceNumber,\n" +
                "                0 DebitAmount,\n" +
                "                ir.amount CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          FROM PaymentVouchers pv,\n" +
                "                InvoiceRedeem ir,\n" +
                "                invoice i,\n" +
                "                ChequeStatus CS,\n" +
                "                ChequeState  cs2\n" +
                "         WHERE pv.Id = ir.PaymentVoucherId\n" +
                "           AND cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and i.invoicenumber = ir.invoiceNo\n" +
                "           and cs.chequestateid in ('1', '2')\n" +
                "           and pv.paymentsourceid not in ('1')\n" +
               clvar._CNNumber + clvar._TownCode + clvar._Zone +
                "        union all\n" +
                "        Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "                i.invoicenumber,\n" +
                "                pv.id Receiptid,\n" +
                "                pv.voucherdate ReceiptDate,\n" +
                "                ir.id InvoiceRedeemID,\n" +
                "                pv.ReceiptNo ReceiptNumber,\n" +
                "                pv.refno ReferenceNumber,\n" +
                "                0 DebitAmount,\n" +
                "                ir.amount CreditAmount,\n" +
                "                0 AggAmount\n" +
                "          FROM PaymentVouchers pv,\n" +
                "                InvoiceRedeem ir,\n" +
                "                invoice i,\n" +
                "                ChequeStatus CS,\n" +
                "                ChequeState cs2\n" +
                "         WHERE pv.Id = ir.PaymentVoucherId\n" +
                "           AND cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and i.invoicenumber = ir.invoiceNo\n" +
                "           and pv.paymentsourceid in ('1')\n" +
                "           and cs.iscurrentstate = '1'\n" +
                clvar._CNNumber + clvar._TownCode + clvar._Zone +
                "        union all\n" +
                "        Select\n" +
                "         '' InvoiceMonth,\n" +
                "          '' invoicenumber,\n" +
                "          pv.id Receiptid,\n" +
                "          pv.voucherdate ReceiptDate,\n" +
                "          0 InvoiceRedeemID,\n" +
                "          pv.ReceiptNo ReceiptNumber,\n" +
                "          pv.refno ReferenceNumber,\n" +
                "          0 DebitAmount,\n" +
                "          Amount - AmountUsed CreditAmount,\n" +
                "          0 AggAmount\n" +
                "          from PaymentVouchers pv, ChequeStatus CS, ChequeState cs2\n" +
                "         where \n" +
                "           cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and cs.chequestateid in ('1', '2')\n" +
                "           and pv.paymentsourceid in ('1')\n" +
                "           and (Amount - AmountUsed) > 1\n" +
                "           and pv.paymentsourceid in ('1', '2')\n" +
                clvar._CNNumber + clvar._TownCode + clvar._Zone +
                "        ) b\n" +
                 clvar._StartDate +
                " group by InvoiceMonth,\n" +
                "          invoicenumber,\n" +
                "          Receiptid,\n" +
                "          ReceiptDate,\n" +
                "          InvoiceRedeemID,\n" +
                "          ReceiptNumber,\n" +
                "          ReferenceNumber\n" +
                " order by b.invoiceMonth";
                */

                /*
                string sqlString = "Select CONVERT(NVARCHAR, InvoiceMonth, 105) InvoiceMonth,\n" +
                "     invoicenumber,\n" +
                "       Receiptid,\n" +
             //   "       CONVERT(NVARCHAR, ReceiptDate, 105) ReceiptDate,\n" +
                " case\n" +
                " when\n" +
                " CONVERT(NVARCHAR, ReceiptDate, 105) = '" + clvar._StartDate + "' then '' \n" +
                " else CONVERT(NVARCHAR, ReceiptDate, 105) end ReceiptDate,\n" +
                "        InvoiceRedeemID,\n" +
                "       ReceiptNumber,\n" +
                "       ReferenceNumber,\n" +
                "       case\n" +
                "         when Sum(DebitAmount) IS null then\n" +
                "          0\n" +
                "         else\n" +
                "          Sum(DebitAmount)\n" +
                "       end DebitAmount,\n" +
                "       case\n" +
                "         when Sum(CreditAmount) IS null then\n" +
                "          0\n" +
                "         else\n" +
                "          Sum(CreditAmount)\n" +
                "       end CreditAmount\n" +
                "  from (Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "               i.invoicenumber,\n" +
                "               '' Receiptid,\n" +
                "               '' ReceiptDate,\n" +
                "               '' InvoiceRedeemID,\n" +
                "               '' ReceiptNumber,\n" +
                "               '' ReferenceNumber,\n" +
                "               i.totalAmount DebitAmount,\n" +
                "               0 CreditAmount,\n" +
                "               0 AggAmount\n" +
                "          from invoice i, CreditClients cc\n" +
                "         where i.clientid = cc.id\n" +
               // "           and cc.accountNo = '2F5'\n" +
                 clvar._ACNumber + clvar._TownCode + clvar._Zone +
                "        union all\n" +
                "        Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "               i.invoicenumber,\n" +
                "               pv.id Receiptid,\n" +
                "               pv.voucherdate ReceiptDate,\n" +
                "               ir.id InvoiceRedeemID,\n" +
                "               pv.ReceiptNo ReceiptNumber,\n" +
                "               pv.refno ReferenceNumber,\n" +
                "               0 DebitAmount,\n" +
                "               ir.amount CreditAmount,\n" +
                "               0 AggAmount\n" +
                "          FROM PaymentVouchers pv,\n" +
                "               InvoiceRedeem   ir,\n" +
                "               invoice         i,\n" +
                "               ChequeStatus    CS,\n" +
                "               ChequeState     cs2,\n" +
                "               CreditClients   cc\n" +
                "         where cc.id = pv.creditclientid\n" +
                "           and pv.Id = ir.PaymentVoucherId\n" +
                "           AND cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and i.invoicenumber = ir.invoiceNo\n" +
                "           and cs.chequestateid in ('1', '2')\n" +
                "           and pv.paymentsourceid not in ('1')\n" +
            //    "           and cc.accountNo = '2F5'\n" +
                clvar._ACNumber + clvar._TownCode + clvar._Zone +
                "        union all\n" +
                "        Select convert(varchar(11), i.startDate, 106) InvoiceMonth,\n" +
                "               i.invoicenumber,\n" +
                "               pv.id Receiptid,\n" +
                "               pv.voucherdate ReceiptDate,\n" +
                "               ir.id InvoiceRedeemID,\n" +
                "               pv.ReceiptNo ReceiptNumber,\n" +
                "               pv.refno ReferenceNumber,\n" +
                "               0 DebitAmount,\n" +
                "               ir.amount CreditAmount,\n" +
                "               0 AggAmount\n" +
                "          FROM PaymentVouchers pv,\n" +
                "               InvoiceRedeem   ir,\n" +
                "               invoice         i,\n" +
                "               ChequeStatus    CS,\n" +
                "               ChequeState     cs2,\n" +
                "               CreditClients   cc\n" +
                "         where cc.id = pv.creditclientid\n" +
                "           and pv.Id = ir.PaymentVoucherId\n" +
                "           AND cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and i.invoicenumber = ir.invoiceNo\n" +
                "           and pv.paymentsourceid in ('1')\n" +
                "           and cs.iscurrentstate = '1'\n" +
            //    "           and cc.accountNo = '2F5'\n" +
                clvar._ACNumber + clvar._TownCode + clvar._Zone +
                "        union all\n" +
                "        Select '' InvoiceMonth,\n" +
                "               '' invoicenumber,\n" +
                "               pv.id Receiptid,\n" +
                "               pv.voucherdate ReceiptDate,\n" +
                "               0 InvoiceRedeemID,\n" +
                "               pv.ReceiptNo ReceiptNumber,\n" +
                "               pv.refno ReferenceNumber,\n" +
                "               0 DebitAmount,\n" +
                "               Amount - AmountUsed CreditAmount,\n" +
                "               0 AggAmount\n" +
                "          from PaymentVouchers pv,\n" +
                "               ChequeStatus    CS,\n" +
                "               ChequeState     cs2,\n" +
                "               CreditClients   cc\n" +
                "         where cc.id = pv.creditclientid\n" +
                "           and cs.PaymentVoucherId = pv.Id\n" +
                "           AND cs.ChequeStateId = cs2.Id\n" +
                "           and cs.chequestateid in ('1', '2')\n" +
                "           and pv.paymentsourceid in ('1')\n" +
                "           and (Amount - AmountUsed) > 1\n" +
                "           and pv.paymentsourceid in ('1', '2')\n" +
              //  "           and cc.accountNo = '2F5' \n" +
              clvar._ACNumber + clvar._TownCode + clvar._Zone +
                " ) b\n" +
                clvar._StartDate +
                " group by InvoiceMonth,\n" +
                "\n" +
                "          invoicenumber,\n" +
                "          Receiptid,\n" +
                "          ReceiptDate,\n" +
                "          InvoiceRedeemID,\n" +
                "          ReceiptNumber,\n" +
                "          ReferenceNumber\n" +
                " order by b.invoiceMonth";
                 * */

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

        public DataSet Get_CustomerLedger_Redeem(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select CONVERT(NVARCHAR, pv.VoucherDate, 105) VoucherDate,\n" +
                                    "       pv.id,\n" +
                                    "       pv.ChequeNo,\n" +
                                    "       ps.name PaymentSource,\n" +
                                    "       (pv.Amount - pv.AmountUsed) NotRedem\n" +
                                    "  from PaymentVouchers pv,\n" +
                                    "       CreditClients   cc,\n" +
                                    "       Zones           z,\n" +
                                    "       Branches        b,\n" +
                                    "       PaymentSource   ps\n" +
                                    " where pv.ZoneCode = z.zoneCode\n" +
                                    "   and pv.BranchCode = b.branchCode\n" +
                                    "   and pv.CreditClientId = cc.id\n" +
                                    "   and pv.PaymentSourceId = ps.Id\n" +
                                    clvar._enddate2 + clvar._Zone2 + clvar._CNNumber +
                                    "   and pv.Amount != pv.AmountUsed\n" +
                                    " order by pv.VoucherDate\n";

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

        public DataSet Get_CustomerLedger_CustomerInfo(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select cc.name CustomerName, cc.accountNo, cc.address, cc.phoneNo, b.name BranchName, \n" +
                                    "CASE WHEN IsCOD = '0' THEN 'N' ELSE 'Y' END CODIndicator \n" +
                                    "from CreditClients cc, Branches b, Zones z \n" +
                                    "where \n" +
                                    "cc.branchCode = b.branchCode\n" + clvar._ACNumber + clvar._Zone_Type + clvar._BranchManager;
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

        public DataSet Get_CustomerData(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT cc.accountNo,cc.id,\n" +
                "       cc.name,\n" +
                "       cc.contactPerson,\n" +
                "       cc.address,\n" +
                "       cc.email,\n" +
                "       cc.faxNo,\n" +
                "       CASE\n" +
                "         WHEN cc.centralizedClient = '0' THEN\n" +
                "          'NON - CENTRALIZED'\n" +
                "         ELSE\n" +
                "          'CENTRALIZED'\n" +
                "       END CustomerHandling,\n" +
                "       z.name Zone,\n" +
                "       b.name Branch,\n" +
                "       CASE\n" +
                "         WHEN cc.accountNo = '0' THEN\n" +
                "          'Cash'\n" +
                "         ELSE\n" +
                "          'Credit'\n" +
                "       END CustomerType,\n" +
                "       CASE\n" +
                "         WHEN cc.IsCOD = '1' THEN\n" +
                "          'COD Customer'\n" +
                "         ELSE\n" +
                "          'NON COD Customer'\n" +
                "       END CODCustomer\n" +
                "  FROM CreditClients cc, Branches b, Zones z\n" +
                " WHERE b.branchCode = cc.branchCode\n" +
                "   AND b.zoneCode = z.zoneCode\n" +
                //    "   AND cc.accountNo != '0'\n" +
                //      "   AND cc.IsCOD = '1'\n" +
                clvar._ACNumber + clvar._Zone + clvar._TownCode + clvar._Expresscentercode + "\n" +

                "\n" +
                " GROUP BY cc.accountNo,cc.id,\n" +
                "          cc.name,\n" +
                "          cc.contactPerson,\n" +
                "          cc.centralizedClient,\n" +
                "          cc.address,\n" +
                "          cc.email,\n" +
                "          cc.faxNo,\n" +
                "          z.name,\n" +
                "          b.name,\n" +
                "          cc.accountNo,\n" +
                "          cc.IsCOD\n" +
                " ORDER BY cc.name, CODCustomer";

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

        public DataSet Get_CODClientList(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select z.name Zone,\n" +
                                    "       b.name Branch,\n" +
                                    "       c.accountNo 'Account No.',\n" +
                                    "       c.name 'Client Name',\n" +
                                    "       case\n" +
                                    "         when c.isActive = '1' then\n" +
                                    "          'Active'\n" +
                                    "         else\n" +
                                    "          'InActive'\n" +
                                    "       end Status,\n" +
                                    "       Case\n" +
                                    "         when c.CODType = '1' then\n" +
                                    "          'Collect Product Amount'\n" +
                                    "         when c.CODType = '2' then\n" +
                                    "          'Collect Service Charges'\n" +
                                    "         when c.CODType = '3' then\n" +
                                    "          'Collect Product Amount Collect Service Charges'\n" +
                                    "       end 'COD Type'\n" +
                                    "  from CreditClients c\n" +
                                    " inner join Branches b\n" +
                                    "    on c.branchCode = b.branchCode\n" +
                                    " inner join Zones z\n" +
                                    "    on b.zoneCode = z.zoneCode\n" +
                                    " where IsCOD = '1'\n" +
                                    "   and c.CODType in ('1', '3')\n" +
                                    clvar._Zone + clvar._TownCode + clvar._ACNumber + "\n" +
                                    " order by 1, 2, 3, 5";

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

        public DataSet Get_CODPreBookingSummary(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select convert(varchar(11), c.createdOn, 105) 'Data Upload Date',\n" +
                "       b.name 'Origin Branch', z.name zone,\n" +
                "       c.AccountNo 'Account No.',\n" +
                "       c.consigner 'Shipper',\n" +
                "       count(c.refrenceNo) 'Total Uploaded Records',\n" +
                "       sum(case\n" +
                "             when c.isCNGenerated = '1' then\n" +
                "              1\n" +
                "             else\n" +
                "              0\n" +
                "           end) 'Total Booked'\n" +
                "  from cardConsignment c\n" +
                " inner join branches b\n" +
                "    on c.branchCode = b.branchCode\n" +
                " inner join zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " where \n" +
                "   cc.CODType in ('1', '3')\n" +
                clvar._TownCode + clvar._Zone + clvar._Year + clvar._ACNumber +
                " group by convert(varchar(11), c.createdOn, 105), b.name, z.name, c.AccountNo, c.consigner\n" +
                " order by convert(varchar(11), c.createdOn, 105), b.name, z.name, c.AccountNo, c.consigner";

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

        public DataSet Get_CODPreBookingDetail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = " select convert(varchar(11), c.createdOn, 105) 'Created Date', z.name zone,\n" +
                                    "       b.name 'Origin Branch',\n" +
                                    "       c.AccountNo 'Account No.',\n" +
                                    "       c.consigner 'Shipper',\n" +
                                    "       c.refrenceNo 'Reference No.',\n" +
                                    "       c.consignee 'Consignee',\n" +
                                    "       c.address 'Consignee Address',\n" +
                                    "       c.ConsigneeContactNo 'Consignee Contact',\n" +
                                    "       c.CODAmount 'COD Amount',\n" +
                                    "       case\n" +
                                    "         when c.isCNGenerated = '1' then\n" +
                                    "          'Booked'\n" +
                                    "         else\n" +
                                    "          'Pending'\n" +
                                    "       end 'Booking Status'\n" +
                                    "  from cardConsignment c\n" +
                                    " inner join branches b\n" +
                                    "    on c.branchCode = b.branchCode\n" +
                                    " inner join zones z\n" +
                                    "    on b.zoneCode = z.zoneCode\n" +
                                    " inner join CreditClients cc\n" +
                                    "    on c.creditClientId = cc.id\n" +
                                    " where \n" +
                                    "   cc.CODType in ('1', '3')\n" +
                                        clvar._TownCode + clvar._Zone + clvar._Year + clvar._ACNumber +
                                    " order by convert(varchar(11), c.createdOn, 105),\n" +
                                    "          b.name,\n" +
                                    "          c.AccountNo,\n" +
                                    "          case\n" +
                                    "            when c.isCNGenerated = '1' then\n" +
                                    "             'Booked'\n" +
                                    "            else\n" +
                                    "             'Pending'\n" +
                                    "          end";

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

        public DataSet Get_Report_Version(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT ri.ReportId,ri.ReportName, u.NAME, rv.remarks, rv.version, rv.requestby, rv.developby, CONVERT(NVARCHAR, rv.date , 105) date \n" +
                                    "FROM Report_Version rv, Report_Info ri, ZNI_USER1 u \n" +
                                    "where \n" +
                                    "rv.reportid = ri.ReportId\n" +
                                    "and rv.userid = u.U_ID";

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

        public DataSet Get_Report_Version_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT ri.ReportId,ri.ReportName, u.NAME, rv.remarks, rv.version, rv.requestby, rv.developby, CONVERT(NVARCHAR, rv.date , 105) date \n" +
                                    "FROM Report_Version rv, Report_Info ri, ZNI_USER1 u \n" +
                                    "where \n" +
                                    "rv.reportid = ri.ReportId\n" +
                                    "and rv.userid = u.U_ID \n" +
                                    "AND ri.ReportId = '" + clvar._ReportId + "' ";

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

        public DataSet Get_Report_VersionById(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString_old = "select ISNULL(MAX(Replace(Str(parsename(rv.Version,1)+1,len(parsename(rv.Version,1)),0),' ','0')),0) version \n" +
                                    "from Report_Version rv\n" +
                                    "where rv.date = (SELECT MAX(date) FROM Report_Version rv where rv.ReportId = '" + clvar._reportid + "') ";



                string sqlString = "select right('00000' + CAST(max(SUBSTRING(rv.Version, 7, 11)+1) AS VARCHAR(MAX)),5) version \n" +
                                    "from Report_Version rv\n" +
                                    "where rv.date = (SELECT MAX(date) FROM Report_Version rv where rv.ReportId = '" + clvar._reportid + "') ";

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

        //public DataSet Get_Report_VersionByReportId(Variable clvar)
        //{
        //    DataSet ds = new DataSet();
        //    string temp = "";
        //    try
        //    {
        //        string sqlString = "SELECT rv.version \n" +
        //                            "FROM Report_Version rv\n" +
        //                            "where \n" +
        //                            "rv.date = (SELECT MAX(date) FROM Report_Version where ReportId = '" + clvar._reportid + "') ";

        //        SqlConnection orcl = new SqlConnection(clvar.Strcon());
        //        orcl.Open();
        //        SqlCommand orcd = new SqlCommand(sqlString, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        SqlDataAdapter oda = new SqlDataAdapter(orcd);
        //        oda.Fill(ds);
        //        orcl.Close();
        //    }
        //    catch (Exception Err)
        //    {

        //    }
        //    finally
        //    { }
        //    return ds;
        //}
        public DataSet Get_Report_VersionByReportId(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT rv.version \n" +
                                    "FROM Report_Version rv\n" +
                                    "where \n" +
                                    "rv.date = (SELECT MAX(date) FROM Report_Version where ReportId = '" + clvar._reportid + "') AND ReportId = '" + clvar._reportid + "' ";

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
        public void Insert_Report_Version(Variable clvar)
        {
            try
            {
                string query =
                "INSERT INTO Report_Version \n" +
                "  (UserId, ReportId, Date, Remarks, Version, RequestBy, DevelopBy, EntryDateTime )\n" +
                "values\n" +
                "  ( \n" +
                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                " '" + clvar._reportname + "' ,\n" +
                " '" + clvar._Year + "' ,\n" +
                " '" + clvar._Remarks + "' ,\n" +
                " '" + clvar._Version + "' ,\n" +
                " '" + clvar._Request + "' ,\n" +
                " '" + clvar._UserName + "' ,\n" +
                "   GETDATE()\n" +
               " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_ShortSale(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                #region MyRegion
                /*
                string sqlString = "select b.VoucherDate,\n" +
                "       b.branch,\n" +
                "       b.zone,\n" +
                "       b.Retail_Office,\n" +
                "       sum(b.chargedAmount) chargedAmount,\n" +
                "       SUM(b.CollectAmount) CollectAmount\n" +
                "  from (select CONVERT(NVARCHAR, c.accountReceivingDate, 105) VoucherDate,\n" +
                "               z.name zone,\n" +
                "               b.name branch,\n" +
                "               ec.name Retail_Office,\n" +
                "               '' CollectAmount,\n" +
                "               sum(c.chargedAmount) chargedAmount\n" +
                "          from Consignment    c,\n" +
                "               Branches       b,\n" +
                "               Zones          z,\n" +
                "               Riders         r,\n" +
                "               ExpressCenters ec\n" +
                "         where c.isApproved = '1'\n" +
                "           and c.consignerAccountNo = '0'\n" +
                "           and c.orgin = b.branchCode\n" +
                "           and b.zoneCode = z.zoneCode\n" +

                "           and c.riderCode = r.riderCode\n" +
                "           and r.branchId = b.branchCode\n" +
                "           and r.expressCenterId = ec.expressCenterCode\n" +

                clvar._TownCode + clvar._Zone  + clvar._Year + clvar._Month + 
                "         group by CONVERT(NVARCHAR, c.accountReceivingDate, 105),\n" +
                "                  ec.name,\n" +
                "                  z.name,\n" +
                "                  b.name\n" +
                "        union all\n" +
                "        select CONVERT(NVARCHAR, pv.VoucherDate, 105) VoucherDate,\n" +
                "               z.name Zone,\n" +
                "               b.name Branch,\n" +
                "               e.name Retail_Office,\n" +
                "               SUM(PV.Amount) CollectAmount,\n" +
                "               '' chargeAmount\n" +
                "          from PaymentVouchers PV, Branches b, ExpressCenters e, Zones z\n" +
                "         WHERE pv.ZoneCode = z.zoneCode\n" +
                "           and pv.BranchCode = b.branchCode\n" +
                "           and pv.ExpressCenterCode = e.expressCenterCode\n" +
                "           and pv.IsByCreditClient = '0'\n" +
                clvar._TownCode + clvar._Zone  + clvar._StartDate + clvar._enddate2 +
                "         group by pv.VoucherDate, z.name, b.name, e.name) b\n" +
                " group by b.VoucherDate, b.branch, b.zone, b.Retail_Office\n" +
                " order by VoucherDate";

                */
                /*

                string sqlString = "select \n" +
                "       CONVERT(NVARCHAR, b.Date, 105) Date,b.Zone,b.Branch,b.[Booking staff Name],b.[Source (CR / Counter)],b.[Retail_Office/EC],\n" +
              //  "       sum(b.CollectAmount) CollectAmount,\n" +
              //  "       sum(b.chargedAmount) chargedAmount,\n" +
              //  "       sum(b.CollectAmount) - sum(b.chargedAmount) 'Diff'\n" +

                "convert(varchar,cast(sum(b.CollectAmount) as money),-1) CollectAmount,\n" +
                "convert(varchar,cast(sum(b.chargedAmount) as money),-1) chargedAmount,\n" +
                "convert(varchar,cast(sum(b.CollectAmount) - sum(b.chargedAmount) as money),-1) 'Diff',\n" +
                "sum(CNCount) CNCount \n" +
                "  from ((select cast(c.accountReceivingDate as DATE) 'Date',\n" +
                "                z.name Zone,\n" +
                "                b.name Branch,\n" +
                "                case\n" +
                "                  when r.userTypeId in ('EC', 'ECI', 'COUNTER') then\n" +
                "                   e.name\n" +
                "                  else\n" +
                "                   (case\n" +
                "                     when r.lastName is null then\n" +
                "                      r.firstName\n" +
                "                     when r.lastName = 'null' then\n" +
                "                      r.firstName\n" +
                "                     when r.firstName = r.lastName then\n" +
                "                      r.firstName\n" +
                "                     else\n" +
                "                      r.firstName + ' ' + r.lastName\n" +
                "                   end)\n" +
                "                end 'Booking staff Name',\n" +
                "                case\n" +
                "                  when r.userTypeId in ('EC', 'ECI', 'COUNTER') then\n" +
                "                   'Counter'\n" +
                "                  else\n" +
                "                   (r.userTypeId)\n" +
                "                end 'Source (CR / Counter)',\n" +
                "                e.name 'Retail_Office/EC',\n" +
                "                '' CollectAmount,\n" +
                "                SUM(c.chargedAmount) chargedAmount, count(c.consignmentNumber) CNCount\n" +
                "           from Consignment    c,\n" +
                "                Riders         r,\n" +
                "                Branches       b,\n" +
                "                Zones          z,\n" +
                "                ExpressCenters e\n" +
                "          where \n " +
          //      "            year(c.accountReceivingDate) = '2016'\n" +
           //     "            and month(c.accountReceivingDate) = '03'\n" +
                "            c.riderCode = r.riderCode\n" +
                "            and c.orgin = r.branchId\n" +
                "            and r.expressCenterId = e.expressCenterCode\n" +
                "            and r.branchId = b.branchCode\n" +
                "            and b.zoneCode = z.zoneCode\n" +
                "            and c.isApproved = '1'\n" +
                clvar._TownCode + clvar._Year + clvar._Month + clvar._Zone +
                "          group by cast(c.accountReceivingDate as DATE),\n" +
                "                   z.name,\n" +
                "                   b.name,\n" +
                "                   case\n" +
                "                     when r.userTypeId in ('EC', 'ECI', 'COUNTER') then\n" +
                "                      e.name\n" +
                "                     else\n" +
                "                      (case\n" +
                "                        when r.lastName is null then\n" +
                "                         r.firstName\n" +
                "                        when r.lastName = 'null' then\n" +
                "                         r.firstName\n" +
                "                        when r.firstName = r.lastName then\n" +
                "                         r.firstName\n" +
                "                        else\n" +
                "                         r.firstName + ' ' + r.lastName\n" +
                "                      end)\n" +
                "                   end,\n" +
                "                   case\n" +
                "                     when r.userTypeId in ('EC', 'ECI', 'COUNTER') then\n" +
                "                      'Counter'\n" +
                "                     else\n" +
                "                      (r.userTypeId)\n" +
                "                   end,\n" +
                "                   e.name) UNION ALL\n" +
                "        (select pv.VoucherDate Date,\n" +
                "                z.name Zone,\n" +
                "                b.name Branch,\n" +
                "                case\n" +
                "                  when r.lastName is null then\n" +
                "                   r.firstName\n" +
                "                  when r.lastName = 'null' then\n" +
                "                   r.firstName\n" +
                "                  when r.firstName = r.lastName then\n" +
                "                   r.firstName\n" +
                "                  else\n" +
                "                   r.firstName + ' ' + r.lastName\n" +
                "                end 'Booking staff Name',\n" +
                "                r.userTypeId 'Source (CR / Counter)',\n" +
                "                e.name 'Retail_Office/EC',\n" +
                "                SUM(PV.Amount) CollectAmount,\n" +
                "                '' chargedAmount, '' CNCount\n" +
                "           from PaymentVouchers PV,\n" +
                "                Branches        b,\n" +
                "                ExpressCenters  e,\n" +
                "                Zones           z,\n" +
                "                Riders          r\n" +
                "          WHERE pv.RiderCode = r.riderCode\n" +
                "            and pv.ExpressCenterCode = r.expressCenterId\n" +
                "            and pv.ZoneCode = z.zoneCode\n" +
                "            and pv.BranchCode = b.branchCode\n" +
                "            and pv.ExpressCenterCode = e.expressCenterCode\n" +
              //  "            and year(PV.VoucherDate) = '2016'\n" +
             //   "            and month(PV.VoucherDate) = '03'\n" +
                clvar._TownCode + clvar._StartDate + clvar._enddate2 + clvar._Zone +
                "            and pv.IsByCreditClient = '0'\n" +
                "            and pv.CashPaymentSource = '2'\n" +
                "          group by pv.VoucherDate,\n" +
                "                   z.name,\n" +
                "                   b.name,\n" +
                "                   case\n" +
                "                     when r.lastName is null then\n" +
                "                      r.firstName\n" +
                "                     when r.lastName = 'null' then\n" +
                "                      r.firstName\n" +
                "                     when r.firstName = r.lastName then\n" +
                "                      r.firstName\n" +
                "                     else\n" +
                "                      r.firstName + ' ' + r.lastName\n" +
                "                   end,\n" +
                "                   r.userTypeId,\n" +
                "                   e.name) union all\n" +
                "        (select pv.VoucherDate Date,\n" +
                "                z.name Zone,\n" +
                "                b.name Branch,\n" +
                "                '' 'Booking staff Name',\n" +
                "                '' 'Source (CR / Counter)',\n" +
                "                e.name 'Retail_Office/EC',\n" +
                "                SUM(PV.Amount) CollectAmount,\n" +
                "                '' chargedAmount, '' CNCount\n" +
                "           from PaymentVouchers PV, Branches b, ExpressCenters e, Zones z\n" +
                "          WHERE pv.ZoneCode = z.zoneCode\n" +
                "            and pv.BranchCode = b.branchCode\n" +
                "            and pv.ExpressCenterCode = e.expressCenterCode\n" +
              //  "            and year(PV.VoucherDate) = '2016'\n" +
              //  "            and month(PV.VoucherDate) = '03'\n" +
                "            and pv.IsByCreditClient = '0'\n" +
                "            and pv.CashPaymentSource = '1'\n" +
                clvar._TownCode + clvar._StartDate + clvar._enddate2 + clvar._Zone +
                "          group by pv.VoucherDate, z.name, b.name, e.name)) b\n" +
                " group by \n" +
                "          CNCount,b.Date,b.Zone,b.Branch,b.[Booking staff Name],b.[Source (CR / Counter)],b.[Retail_Office/EC]\n" +
                "HAVING sum(b.CollectAmount) - sum(b.chargedAmount) != 0 order by date asc";


                */
                #endregion

                string sql = "select CONVERT(NVARCHAR, b.Date, 105) Date,\n" +
                "b.Zone, b.zoneCode, b.Branch, b.branchCode, b.ridercode,\n" +
                "b.[Booking staff Name],b.[Source (CR / Counter)],b.[Retail_Office/EC],\n" +
                // "sum(b.CollectAmount) CollectAmount,\n" +
                //  "sum(b.chargedAmount) chargedAmount,\n" +
                "sum(b.CNCount) CNCount,\n" +
                //  "sum(b.CollectAmount) -\n" +
                // "sum(b.chargedAmount) 'Diff'\n" +

                "convert(varchar,cast(sum(b.CollectAmount) as money),-1) CollectAmount,\n" +
                "convert(varchar,cast(sum(b.chargedAmount) as money),-1) chargedAmount,\n" +
                "convert(varchar,cast(sum(b.CollectAmount) - sum(b.chargedAmount) as money),-1) 'Diff'\n" +

                "from\n" +
                "((select\n" +
                "cast(c.accountReceivingDate as DATE) 'Date',\n" +
                "z.name Zone, z.zoneCode,\n" +
                " b.name Branch, b.branchCode,\n" +
                "case when r.userTypeId in ('EC','ECI','COUNTER') then e.name else\n" +
                "(case when r.lastName is null then r.firstName when r.lastName = 'null' then r.firstName when r.firstName=r.lastName then r.firstName else r.firstName+' ' +r.lastName  end ) end 'Booking staff Name',\n" +
                "case when r.userTypeId in ('EC','ECI','COUNTER') then 'Counter' else\n" +
                "(r.userTypeId) end  'Source (CR / Counter)',\n" +
                "e.name 'Retail_Office/EC',\n" +
                "''  CollectAmount ,\n" +
                "SUM(c.chargedAmount) chargedAmount,\n" +
                "count(c.consignmentNumber) CNCount, r.ridercode\n" +
                "from Consignment c,Riders r,Branches b,Zones z,ExpressCenters e\n" +
                "where\n" +
                "c.riderCode=r.riderCode and c.orgin=r.branchId\n" +
                "and r.expressCenterId=e.expressCenterCode\n" +
                "and r.branchId=b.branchCode\n" +
                "and b.zoneCode=z.zoneCode\n" +
                "and c.isApproved='1'\n" +
                "and c.consignerAccountNo = '0' \n" +
                clvar._Year + clvar._Month +
                "group by\n" +
                "cast(c.accountReceivingDate as DATE) ,\n" +
                "z.name, z.zoneCode, r.riderCode,\n" +
                "b.name, b.branchCode,\n" +
                "case when r.userTypeId in ('EC','ECI','COUNTER') then e.name else\n" +
                "(case when r.lastName is null then r.firstName when r.lastName = 'null' then r.firstName when r.firstName=r.lastName then r.firstName else r.firstName+' ' +r.lastName  end ) end ,\n" +
                "case when r.userTypeId in ('EC','ECI','COUNTER') then 'Counter' else\n" +
                "(r.userTypeId) end  ,\n" +
                "e.name)\n" +
                "UNION ALL\n" +
                "(select\n" +
                "pv.VoucherDate Date,\n" +
                "z.name Zone, z.zoneCode,\n" +
                "b.name Branch, b.branchCode,\n" +
                "case when r.lastName is null then r.firstName when r.lastName = 'null' then r.firstName when r.firstName=r.lastName then r.firstName else r.firstName+' ' +r.lastName  end 'Booking staff Name',\n" +
                "r.userTypeId 'Source (CR / Counter)',\n" +
                "e.name 'Retail_Office/EC',\n" +
                "SUM(PV.Amount) CollectAmount,\n" +
                "'' chargedAmount ,\n" +
                "'' CNCount, r.ridercode\n" +
                "from PaymentVouchers PV,Branches b,ExpressCenters e,Zones z,Riders r\n" +
                "WHERE\n" +
                "pv.RiderCode=r.riderCode\n" +
                "and pv.ExpressCenterCode=r.expressCenterId\n" +
                "and pv.ZoneCode=z.zoneCode and\n" +
                "pv.BranchCode=b.branchCode and\n" +
                "pv.ExpressCenterCode=e.expressCenterCode\n" +
               clvar._StartDate + clvar._enddate2 +
                "and pv.IsByCreditClient='0'\n" +
                "and pv.CashPaymentSource='2'\n" +
                "group by pv.VoucherDate ,\n" +
                "z.name, z.zoneCode, b.name, b.branchCode, r.riderCode,\n" +
                "case when r.lastName is null then r.firstName when r.lastName = 'null' then r.firstName when r.firstName=r.lastName then r.firstName else r.firstName+' ' +r.lastName  end ,\n" +
                "r.userTypeId ,\n" +
                "e.name )\n" +
                "union all\n" +
                "(select\n" +
                "pv.VoucherDate Date,\n" +
                "z.name Zone, z.zoneCode,\n" +
                "b.name Branch, b.branchCode,\n" +
                "e.name  'Booking staff Name',\n" +
                "'Counter'  'Source (CR / Counter)',\n" +
                "e.name 'Retail_Office/EC',\n" +
                "SUM(PV.Amount) CollectAmount ,\n" +
                "'' chargedAmount,\n" +
                "'' CNCount, '' RiderCode\n" +
                "from PaymentVouchers PV,Branches b,ExpressCenters e,Zones z\n" +
                "WHERE\n" +
                "pv.ZoneCode=z.zoneCode and\n" +
                "pv.BranchCode=b.branchCode and\n" +
                "pv.ExpressCenterCode=e.expressCenterCode\n" +
                clvar._StartDate + clvar._enddate2 +
                "and pv.IsByCreditClient='0'\n" +
                "and pv.CashPaymentSource='1'\n" +
                "group by pv.VoucherDate ,\n" +
                "z.name, z.zoneCode,\n" +
                "b.name, b.branchCode,\n" +
                "e.name ))b\n" +
                "group by\n" +
                "b.Date,\n" +
                "b.Branch,b.branchCode,b.Zone,b.zoneCode, b.ridercode,\n" +
                "\n" +
                "b.[Booking staff Name],b.[Source (CR / Counter)],b.[Retail_Office/EC]\n" +
                "having sum(b.CollectAmount) - sum(b.chargedAmount)!='0'\n" +
                clvar._Zone + clvar._TownCode + "  ORDER BY 1, 2, 3";


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
        public DataTable Get_ShortSaleDrill(Variable clvar)
        {

            string sqlString = "select\n" +
            "c.consignmentNumber Consignment_Number,\n" +
            "z.name Origin_Zone,\n" +
            "b.name Origin_Branch,\n" +
            "e.name Origin_Express_Centre,\n" +
            "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then 'International' else zz.name end  'Dest_Zone' ,\n" +
            "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then ee.name else bb.name end  'Dest Branch' ,\n" +
            "cast(c.bookingDate as date) Booking_Date,\n" +
            "cast(c.accountReceivingDate as date) Sale_Date,\n" +
            "c.ridercode Booking_Rider_Code,\n" +
            "case when r.lastName is null then r.firstName when r.lastName = 'null' then r.firstName when r.firstName=r.lastName then r.firstName else r.firstName+' ' +r.lastName  end Booking_Rider_Name,\n" +
            "r.userTypeId 'Source (CR / Counter)',\n" +
            "case when c.consignmentTypeId='13' and c.serviceTypeName='overnight' then 'Hand Carry' else c.serviceTypeName end Service,\n" +
            "c.weight weight,\n" +
            "c.pieces  Pcs,\n" +
            "c.chargedAmount Charged_Amount\n" +
            "\n" +
            "from Consignment c\n" +
            "inner join Branches b on c.orgin=b.branchCode\n" +
            "inner join Zones z on b.zoneCode=z.zoneCode\n" +
            "inner join Branches bb on c.destination=bb.branchCode\n" +
            "inner join Zones zz on bb.zoneCode=zz.zoneCode\n" +
            "left join Country ee on c.destinationCountryCode=ee.Code\n" +
            "inner join\n" +
            "(select * from Riders r1 where\n" +
            "r1.expressCenterId+r1.riderCode not in\n" +
            "('3711108','0430122','0411234','2311328','1812-1401','1812-1403',\n" +
            "'1812-1406','0230-1450','0222-1514','0222-1519','1813-1601',\n" +
            "'1813-1607','31121007','11111101','01113001','EXCENTER-FRFSD00029001',\n" +
            "'013111714'))\n" +
            "r on c.riderCode = r.riderCode and c.orgin= r.branchId\n" +
            "inner join ExpressCenters e on r.expressCenterId=e.expressCenterCode\n" +
            "\n" +
            "where\n" +
            "CAST(c.accountReceivingDate as Date) = '" + DateTime.Parse(clvar.Check_Condition1).ToString("yyyy-MM-dd") + "'\n" +
            "and c.riderCode='" + clvar.Check_Condition + "'\n" +
            "and c.consignerAccountNo = '0'\n" +
            "and isApproved='1'\n" +
            "and c.orgin='" + clvar.TownCode + "' order by Consignment_Number";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;

        }

        public DataSet Get_Manifest(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select c.consignmentNumber 'Consingment Number', cm.manifestNumber 'Manifest Number', \n" +
                                "CONVERT(NVARCHAR(10), m.date, 105) 'Manifest Date', b.name 'Manifest Branch', bb.name 'Manifest Destination' from consignment c\n" +
                                "left join consignmentManifest cm on c.consignmentNumber = cm.consignmentNumber\n" +
                                "inner join Manifest m on cm.manifestNumber = m.manifestNumber\n" +
                                "inner join Branches b on m.branchCode = b.branchCode\n" +
                                "inner join Branches bb on m.destination = bb.branchCode \n" +
                                clvar._CNNumber + " \n" +
                                "Order by c.consignmentNumber ASC";

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

        /*
        public DataSet Get_RunsheetSearchReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select \n" +
                                "CONVERT(NVARCHAR(10), r.runsheetDate, 105) RunSheetDate,\n" +
                                "rc.consignmentNumber ConsignmentNumber, \n" +
                                "c.consignerAccountNo  AccNo,\n" +
                                "b.sname Origin,\n" +
                                "bb.sname Dest,\n" +
                                "rr.riderCode RiderCode,\n" +
                                "case when a.lastName is null then a.firstName when a.lastName = 'null' then a.firstName when a.firstName=a.lastName then a.firstName else a.firstName+' ' +a.lastName  end 'Delivery Rider',\n" +
                                "a.userTypeId 'Source (CR / Counter)',\n" +
                                "case when c.consignmentTypeId='13' and c.serviceTypeName='overnight' then 'Hand Carry' else c.serviceTypeName end Service,\n" +
                                "c.consigner Shipper,\n" +
                                "c.consignee, \n" +
                                "rc.Status\n" +
                                "from RunsheetConsignment rc, RiderRunsheet rr, Runsheet r, Riders a, Consignment c, Branches b,branches bb\n" +
                                "where\n" +
                                "rc.runsheetNumber = rr.runsheetNumber\n" +
                                "and rc.runsheetNumber = r.runsheetNumber\n" +
                                "and rr.riderCode+rr.expIdTemp = a.riderCode+a.expressCenterId\n" +
                                "and rc.consignmentNumber = c.consignmentNumber\n" +
                                "and c.orgin = b.branchCode\n" +
                                "and r.branchCode=bb.branchCode\n" +
                                clvar._CNNumber + " \n" +
                                "order by CONVERT(NVARCHAR(10), r.runsheetDate, 105) ASC";

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
        */

        public DataSet Get_RunsheetSearchReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select \n" +
                                "CONVERT(NVARCHAR(10), r.runsheetDate, 105) RunSheetDate,\n" +
                                "rc.consignmentNumber ConsignmentNumber, \n" +
                                "c.consignerAccountNo  AccNo,\n" +
                                "b.sname Origin,\n" +
                                "bb.sname Dest,\n" +
                                "rr.riderCode RiderCode,\n" +
                                "case when a.lastName is null then a.firstName when a.lastName = 'null' then a.firstName when a.firstName=a.lastName then a.firstName else a.firstName+' ' +a.lastName  end 'Delivery Rider',\n" +
                                "a.userTypeId 'Source (CR / Counter)',\n" +
                                "case when c.consignmentTypeId='13' and c.serviceTypeName='overnight' then 'Hand Carry' else c.serviceTypeName end Service,\n" +
                                "c.consigner Shipper,\n" +
                                "c.consignee, rc.receivedBy,rc.Receiver_CNIC, \n" +
                                "rc.Status\n" +
                                "from RunsheetConsignment rc, RiderRunsheet rr, Runsheet r, Riders a, Consignment c, Branches b,branches bb\n" +
                                "where\n" +
                                "rc.runsheetNumber = rr.runsheetNumber\n" +
                                "and rc.runsheetNumber = r.runsheetNumber\n" +
                                "and rr.riderCode+rr.expIdTemp = a.riderCode+a.expressCenterId\n" +
                                "and rc.consignmentNumber = c.consignmentNumber\n" +
                                "and c.orgin = b.branchCode\n" +
                                "and r.branchCode=bb.branchCode\n" +
                                clvar._CNNumber + " \n" +
                                "order by CONVERT(NVARCHAR(10), r.runsheetDate, 105) ASC";

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

        public DataTable Get_BagOrSealReport(Variable clvar)
        {

            string sqlString = "\tSELECT * FROM (\n" +
            "\tselect cast(b.date as date) 'Date',\n" +
            "\t\t   b.sealNo Seal_No,\n" +
            "\t\t   b.bagNumber Bag_Number,\n" +
            "\t\t   'M-' + bm.manifestNumber M_O\n" +
            "\t  from Bag b\n" +
            "\t inner join BagManifest bm\n" +
            "\t\ton b.bagNumber = bm.bagNumber\n" +
            "\tunion\n" +
            "\tselect cast(b.date as date) 'Date',\n" +
            "\t\t   b.sealNo Seal_No,\n" +
            "       b.bagNumber Bag_Number,\n" +
            "       'CN-' + o.outpieceNumber M_O\n" +
            "    from Bag b\n" +
            "   inner join BagOutpieceAssociation o\n" +
            "    on b.bagNumber = o.bagNumber\n" +
            "  )  a\n" +
            "Where " + clvar.Check_Condition + "";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;
        }

        public DataTable Get_ClientTariffInfo(Variable clvar)
        {

            string sqlString = "select tc.Client_Id,\n" +
            "\t\tCase when tc.Client_Id = '0' then 'CASH ACCOUNT'\n" +
            "\t\telse cc.name\n" +
            "\t\tend ClientName,\n" +
            "\t   tc.ServiceID,\n" +
            "\t   tc.BranchCode,\n" +
            "\t   b.name,\n" +
            "\t   --tc.FromZoneCode,\n" +
            "\t   z.name FromZone,\n" +
            "\t   --tc.ToZoneCode,\n" +
            "\t   z_.name ToZone,\n" +
            "\t   tc.FromWeight,\n" +
            "\t   tc.ToWeight,\n" +
            "\t   tc.Price\n" +
            "  from tempClientTariff tc\n" +
            "  inner join CreditClients cc\n" +
            "  on cc.id = tc.Client_Id\n" +
            "  inner join Branches b\n" +
            "  on b.branchCode = tc.BranchCode\n" +
            "  inner join Zones z\n" +
            "  on z.zoneCode = tc.FromZoneCode\n" +
            "  inner join Zones z_\n" +
            "  on z_.zoneCode = tc.ToZoneCode\n" +
            "  where " + clvar.Check_Condition + "";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;

        }

        public DataSet Get_DailySalesReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString_old = "select CONVERT(NVARCHAR(10), c.bookingDate, 105) 'Date',\n" +
                "       DATENAME(dw, c.bookingDate) 'Day',\n" +
                "       z.name Zone,\n" +
                "       case\n" +
                "         when bs2.firstName = bs2.lastName then\n" +
                "          bs2.firstName\n" +
                "         when bs2.lastName is null then\n" +
                "          bs2.firstName\n" +
                "         else\n" +
                "          bs2.firstName + ' ' + bs2.lastName\n" +
                "       end 'ABH/ZM/CSH',\n" +
                "       b.sname Branch,\n" +
                "       case\n" +
                "         when bs1.firstName = bs1.lastName then\n" +
                "          bs1.firstName\n" +
                "         when bs1.lastName is null then\n" +
                "          bs1.firstName\n" +
                "         else\n" +
                "          bs1.firstName + ' ' + bs1.lastName\n" +
                "       end 'Sales Executive',\n" +
                "       t.Products,\n" +
                "       case\n" +
                "         when c.weight <= .5 and c.serviceTypeName != 'Road n Rail' then\n" +
                "          'Light'\n" +
                "         when c.weight > .5 then\n" +
                "          'Heavy'\n" +
                "         when c.serviceTypeName = 'Road n Rail' then\n" +
                "          'Heavy'\n" +
                "       end 'WT GROUP',\n" +
                "       COUNT(c.consignmentNumber) SHIPMENTS,\n" +
                "       sum(c.weight) 'Total Weight',\n" +
                "       sum(case\n" +
                "             when c.serviceTypeName = 'Road n Rail' and c.weight < 5 then\n" +
                "              '5'\n" +
                "             else\n" +
                "              c.weight\n" +
                "           end) 'Charged Weight',\n" +
                "       round(sum(totalAmount), 2) as REVENUE,\n" +
                "       round(sum((totalAmount) * tem.FS) / 100, 2) FUEL,\n" +
                "       round(sum((totalAmount) + (((totalAmount) * tem.FS) / 100)), 2) GROSS_REVENUE\n" +
                "  from Consignment c\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " inner join ServiceTypes_New t\n" +
                "    on c.serviceTypeName = t.serviceTypeName\n" +
                "  left JOIN ClientStaff cs1\n" +
                "    on cc.id = cs1.ClientId\n" +
                "   and cs1.StaffTypeId = '214'\n" +
                "  left join BTSUsers bs1\n" +
                "    on cs1.UserName = bs1.username\n" +
                "  left join (SELECT cc.id,\n" +
                "                    case\n" +
                "                      when cs1.UserName is null then\n" +
                "                       cs2.UserName\n" +
                "                      when cs1.UserName is null and cs2.UserName is null then\n" +
                "                       cs3.UserName\n" +
                "                      else\n" +
                "                       cs1.UserName\n" +
                "                    end UserName\n" +
                "               FROM CreditClients cc\n" +
                "               left JOIN ClientStaff cs1\n" +
                "                 on cc.id = cs1.ClientId\n" +
                "                and cs1.StaffTypeId = '217'\n" +
                "               left JOIN ClientStaff cs2\n" +
                "                 on cc.id = cs2.ClientId\n" +
                "                and cs2.StaffTypeId = '220'\n" +
                "               left JOIN ClientStaff cs3\n" +
                "                 on cc.id = cs3.ClientId\n" +
                "                and cs3.StaffTypeId = '221') cs2\n" +
                "    on cc.id = cs2.id\n" +
                "  left join BTSUsers bs2\n" +
                "    on cs2.UserName = bs2.username\n" +
                "  left join (select c.id,\n" +
                "                    ((case\n" +
                "                      when cm1.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm1.modifiedCalculationValue\n" +
                "                    end) * (case\n" +
                "                      when cm2.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm2.modifiedCalculationValue\n" +
                "                    end) / 100) + ((case\n" +
                "                      when cm1.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm1.modifiedCalculationValue\n" +
                "                    end) + (case\n" +
                "                      when cm2.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm2.modifiedCalculationValue\n" +
                "                    end)) FS\n" +
                "               from creditclients c\n" +
                "               left join ClientPriceModifierAssociation cm1\n" +
                "                 on c.id = cm1.creditClientId\n" +
                "                and cm1.sortOrder = '0'\n" +
                "               left join ClientPriceModifierAssociation cm2\n" +
                "                 on c.id = cm2.creditClientId\n" +
                "                and cm2.sortOrder = '1') tem\n" +
                "    on cc.id = tem.id\n" +
                " where year(bookingDate) = year(getDate())\n" +
                "   and month(bookingDate) = month(getDate())\n" +
                "   and day(bookingDate) = day(getDate())\n" +
                "   and consignerAccountNo not in ('0', '999')\n" +
                "   and isApproved = '1'\n" +
                clvar._TownCode + clvar._Zone +
                " group by CONVERT(NVARCHAR(10), c.bookingDate, 105),\n" +
                "          DATENAME(dw, c.bookingDate),\n" +
                "          z.name,\n" +
                "          case\n" +
                "            when bs2.firstName = bs2.lastName then\n" +
                "             bs2.firstName\n" +
                "            when bs2.lastName is null then\n" +
                "             bs2.firstName\n" +
                "            else\n" +
                "             bs2.firstName + ' ' + bs2.lastName\n" +
                "          end,\n" +
                "          b.sname,\n" +
                "          case\n" +
                "            when bs1.firstName = bs1.lastName then\n" +
                "             bs1.firstName\n" +
                "            when bs1.lastName is null then\n" +
                "             bs1.firstName\n" +
                "            else\n" +
                "             bs1.firstName + ' ' + bs1.lastName\n" +
                "          end,\n" +
                "          t.Products,\n" +
                "          case\n" +
                "            when c.weight <= .5 and c.serviceTypeName != 'Road n Rail' then\n" +
                "             'Light'\n" +
                "            when c.weight > .5 then\n" +
                "             'Heavy'\n" +
                "            when c.serviceTypeName = 'Road n Rail' then\n" +
                "             'Heavy'\n" +
                "          end\n" +
                " ORDER BY 1, 2, 3, 4, 7, 5";


                string sqlString = "select\n" +
                "CONVERT(NVARCHAR(10), c.bookingDate, 105) 'Date',\n" +
                "DATENAME(dw,c.bookingDate) 'Day',\n" +
                "z.name Zone,\n" +
                "case when bs2.firstName=bs2.lastName then bs2.firstName\n" +
                "when bs2.lastName is null then bs2.firstName\n" +
                "else bs2.firstName+' '+bs2.lastName end 'ABH/ZM/CSH',\n" +
                "b.sname Branch,\n" +
                "case when bs1.firstName=bs1.lastName then bs1.firstName\n" +
                "when bs1.lastName is null then bs1.firstName\n" +
                "else bs1.firstName+' '+bs1.lastName end 'Sales Executive',\n" +
                "t.Products ,\n" +
                "case when c.weight<=.5 and c.serviceTypeName!='Road n Rail' then 'Light'\n" +
                "when c.weight>.5 then 'Heavy'\n" +
                "when c.serviceTypeName='Road n Rail' then 'Heavy'\n" +
                "end 'WT GROUP',\n" +
                "COUNT(c.consignmentNumber) SHIPMENTS,\n" +
                "sum(c.weight) 'Total Weight',\n" +
                "sum (case when c.serviceTypeName = 'Road n Rail' and c.weight<5 then '5' else c.weight end )'Charged Weight',\n" +
                "round(sum(totalAmount),2) as REVENUE,\n" +
                "round(sum((totalAmount)*tem.FS)/100,2) FUEL,\n" +
                "round(sum((totalAmount+gst)+(((totalAmount+gst)*tem.FS)/100)),2) GROSS_REVENUE\n" +
                "\n" +
                "\n" +
                "from Consignment c\n" +
                "inner join Branches b on c.orgin = b.branchCode\n" +
                "inner join Zones z on b.zoneCode = z.zoneCode\n" +
                "inner join CreditClients cc on c.creditClientId=cc.id\n" +
                "inner join ServiceTypes_New t on c.serviceTypeName=t.serviceTypeName\n" +
                "left JOIN ClientStaff cs1 on cc.id=cs1.ClientId and cs1.StaffTypeId='214'\n" +
                "left join BTSUsers bs1 on cs1.UserName=bs1.username\n" +
                "\n" +
                "left join (SELECT\n" +
                "cc.id,\n" +
                "case\n" +
                "when cs1.UserName is null then cs2.UserName\n" +
                "when cs1.UserName is null and cs2.UserName is null then cs3.UserName\n" +
                "else cs1.UserName end UserName\n" +
                "FROM CreditClients cc\n" +
                "left JOIN ClientStaff cs1 on cc.id=cs1.ClientId and cs1.StaffTypeId='217'\n" +
                "left JOIN ClientStaff cs2 on cc.id=cs2.ClientId and cs2.StaffTypeId='220'\n" +
                "left JOIN ClientStaff cs3 on cc.id=cs3.ClientId and cs3.StaffTypeId='221') cs2 on cc.id=cs2.id\n" +
                "left join BTSUsers bs2 on cs2.UserName=bs2.username\n" +
                "\n" +
                "left join\n" +
                "(select c.id,\n" +
                "((case when cm1.modifiedCalculationValue is null then 0 else cm1.modifiedCalculationValue end )*\n" +
                "(case when cm2.modifiedCalculationValue is null then 0 else cm2.modifiedCalculationValue end)/100)+\n" +
                "((case when cm1.modifiedCalculationValue is null then 0 else cm1.modifiedCalculationValue end )+\n" +
                "(case when cm2.modifiedCalculationValue is null then 0 else cm2.modifiedCalculationValue end)) FS\n" +
                "\n" +
                "from  creditclients c\n" +
                "left join ClientPriceModifierAssociation cm1  on c.id=cm1.creditClientId and  cm1.sortOrder='0'\n" +
                "left join ClientPriceModifierAssociation cm2  on c.id=cm2.creditClientId and  cm2.sortOrder='1')\n" +
                "tem on cc.id=tem.id\n" +
                "\n" +
                "where\n" +
                "year(bookingDate) = year (getDate()) and month(bookingDate) = month (getDate())\n" +
                "--and day(bookingDate) = day(getDate())\n" +
                "\n" +
                "and consignerAccountNo not in ('0','999')\n" +
                "and isApproved='1'\n" +
               // "and z.zoneCode = '1'\n" +
               // "and b.branchCode = '1'\n" +
               " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
               "and c.isPriceComputed = '1' \n" +
               "AND c.creditClientId NOT IN\n" +
            "   ('305289', '350799', '350800', '350801', '350802', '350804', '350806',\n" +
            "   '350807', '350808', '350809', '350810', '305290', '305291', '305292',\n" +
            "   '305552', '305554', '305555', '305905', '306180', '306851', '306852',\n" +
            "   '306853', '306875', '307126', '307127', '307128', '308228', '309598',\n" +
            "   '294188', '294192', '296013', '311104', '308518', '308520', '312271',\n" +
            "   '312449', '312769', '312775', '315238', '314629', '314630', '317032',\n" +
            "   '317040', '317043', '317045', '319113', '322994', '322461', '327590',\n" +
            "   '328002', '330058', '335397', '342428', '343850', '344309', '344310',\n" +
            "   '344311', '344312', '344315', '344316', '328802', '303511', '303514',\n" +
            "   '323828', '323835', '323838', '323841', '324212', '324214', '324216',\n" +
            "   '326070', '326071', '326088', '326089', '326092', '326256', '326421',\n" +
            "   '326423', '326620', '326622', '326623', '324297', '324298', '324299',\n" +
            "   '324300', '324301', '325436', '325437', '325438', '325879', '330249',\n" +
            "   '326971', '326973', '326974', '332171', '332407', '331420', '342506',\n" +
            "   '344138', '344553', '304648', '304649', '304650', '304651', '297298',\n" +
            "   '297301', '297303', '297305', '297307', '305620', '305780', '306566',\n" +
            "   '306567', '306568', '302799', '302801', '306726', '306727', '306728',\n" +
            "   '308515', '308516', '307832', '312452', '312453', '312767', '314631',\n" +
            "   '317039', '318543', '319114', '319115', '322991', '322993', '323839',\n" +
            "   '323840', '324210', '324213', '325439', '325441', '322462', '322463',\n" +
            "   '326069', '326072', '326091', '326094', '326420', '326422', '326970',\n" +
            "   '326972', '333578', '336164', '335996', '335997', '336092', '336113',\n" +
            "   '336817', '336819', '336912', '342404', '326619', '326621', '344313',\n" +
            "   '344314', '344712', '343130', '289745', '290836', '292001', '292002',\n" +
            "   '292626', '293291', '294175', '294177', '294178', '294186', '294187',\n" +
            "   '294190', '294191', '294729', '295352', '296010', '296012', '296753',\n" +
            "   '296653', '296740', '296742', '297294', '297295', '297296', '297297',\n" +
            "   '297300', '297302', '297304', '297306', '296822', '296837', '302798',\n" +
            "   '305889', '306021', '306422', '306837', '306967', '306968', '306969',\n" +
            "   '307289', '307290', '307291', '307365', '307368', '307369', '307370',\n" +
            "   '308517', '308519', '308521', '308522', '307805', '307859', '312928',\n" +
            "   '312929', '312269', '312445', '312446', '314627', '314628', '317041',\n" +
            "   '317044', '318544', '319116', '322992', '323806', '323833', '323834',\n" +
            "   '323836', '323837', '308684', '324211', '324215', '324312', '325440',\n" +
            "   '325442', '325443', '332717', '326967', '326968', '326969', '330141',\n" +
            "   '322464', '326068', '326073', '326074', '326087', '326090', '326093',\n" +
            "   '326418', '326419', '326424', '326617', '326618', '326624', '342464',\n" +
            "   '344009', '342768', '342987')  \n" +
                clvar._TownCode + clvar._Zone +
                "\n" +
                "group by\n" +
                "\n" +
                "CONVERT(NVARCHAR(10), c.bookingDate, 105) ,\n" +
                "DATENAME(dw,c.bookingDate) ,\n" +
                "z.name ,\n" +
                "case when bs2.firstName=bs2.lastName then bs2.firstName\n" +
                "when bs2.lastName is null then bs2.firstName\n" +
                "else bs2.firstName+' '+bs2.lastName end ,\n" +
                "b.sname ,\n" +
                "case when bs1.firstName=bs1.lastName then bs1.firstName\n" +
                "when bs1.lastName is null then bs1.firstName\n" +
                "else bs1.firstName+' '+bs1.lastName end ,\n" +
                "t.Products ,\n" +
                "case when c.weight<=.5 and c.serviceTypeName!='Road n Rail' then 'Light'\n" +
                "when c.weight>.5 then 'Heavy'\n" +
                "when c.serviceTypeName='Road n Rail' then 'Heavy'\n" +
                "end\n" +
                "\n" +
                "ORDER BY 1,2,3,4,7,5";

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
        public DataSet Get_NewAcquisitionReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select z.name Zone,\n" +
                "       b.sname Branch,\n" +
                "       case\n" +
                "         when bs1.firstName = bs1.lastName then\n" +
                "          bs1.firstName\n" +
                "         when bs1.lastName is null then\n" +
                "          bs1.firstName\n" +
                "         else\n" +
                "          bs1.firstName + ' ' + bs1.lastName\n" +
                "       end 'Sales Executive',\n" +
                "       case\n" +
                "         when bs2.firstName = bs2.lastName then\n" +
                "          bs2.firstName\n" +
                "         when bs2.lastName is null then\n" +
                "          bs2.firstName\n" +
                "         else\n" +
                "          bs2.firstName + ' ' + bs2.lastName\n" +
                "       end 'ABH/ZM/CSH',\n" +
                "       datename(MM, c.bookingDate) + '-' + datename(YY, c.bookingDate) 'PERIOD',\n" +
                //   "       cc.regDate 'REGSTRATION_DAT',\n" +
                "       CONVERT(NVARCHAR(10), cc.regDate, 105) 'REGSTRATION_DAT',\n" +
                "       datename(MM, cc.createdon) + '-' + datename(YY, cc.regDate) 'REGS_MONTH',\n" +
                "       t.Products,\n" +
                "       cc.accountno 'Client Account Number',\n" +
                "       cc.name 'Client Name',\n" +
                "       cg.name 'Parent Name',\n" +
                "       round(sum(totalAmount), 2) REVENUE,\n" +
                "       cast(tem.FS as varchar) + '%' 'FUEL%',\n" +
                "       count(case\n" +
                "               when c.weight <= .5 and c.serviceTypeName != 'Road n Rail' then\n" +
                "                c.consignmentNumber\n" +
                "             end) 'LIGHT SHIP',\n" +
                "       count(Case\n" +
                "               when c.weight > .5 then\n" +
                "                c.consignmentNumber\n" +
                "               when c.serviceTypeName = 'Road n Rail' then\n" +
                "                c.consignmentNumber\n" +
                "             end) 'HEAVY SHIP',\n" +
                "       sum(Case\n" +
                "             when c.weight > .5 then\n" +
                "              c.weight\n" +
                "             when c.serviceTypeName = 'Road n Rail' then\n" +
                "              c.weight\n" +
                "           end) 'WEIGHT',\n" +
                "       case\n" +
                "         when (DATEDIFF(MONTH, cc.createdon, c.bookingDate)) < '4' then\n" +
                "          'NEW'\n" +
                "         else\n" +
                "          'OLD'\n" +
                "       end 'TYPE'\n" +
                "  from Consignment c\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " inner join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " inner join ServiceTypes_New t\n" +
                "    on c.serviceTypeName = t.serviceTypeName\n" +
                "  left JOIN ClientStaff cs1\n" +
                "    on cc.id = cs1.ClientId\n" +
                "   and cs1.StaffTypeId = '214'\n" +
                "  left join BTSUsers bs1\n" +
                "    on cs1.UserName = bs1.username\n" +
                "  LEFT JOIN ClientGroups cg\n" +
                "    on cc.clientGrpId = cg.id\n" +
                "  left join (SELECT cc.id,\n" +
                "                    case\n" +
                "                      when cs1.UserName is null then\n" +
                "                       cs2.UserName\n" +
                "                      when cs1.UserName is null and cs2.UserName is null then\n" +
                "                       cs3.UserName\n" +
                "                      else\n" +
                "                       cs1.UserName\n" +
                "                    end UserName\n" +
                "               FROM CreditClients cc\n" +
                "               left JOIN ClientStaff cs1\n" +
                "                 on cc.id = cs1.ClientId\n" +
                "                and cs1.StaffTypeId = '217'\n" +
                "               left JOIN ClientStaff cs2\n" +
                "                 on cc.id = cs2.ClientId\n" +
                "                and cs2.StaffTypeId = '220'\n" +
                "               left JOIN ClientStaff cs3\n" +
                "                 on cc.id = cs3.ClientId\n" +
                "                and cs3.StaffTypeId = '221') cs2\n" +
                "    on cc.id = cs2.id\n" +
                "  left join BTSUsers bs2\n" +
                "    on cs2.UserName = bs2.username\n" +
                "  left join (select c.id,\n" +
                "                    ((case\n" +
                "                      when cm1.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm1.modifiedCalculationValue\n" +
                "                    end) * (case\n" +
                "                      when cm2.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm2.modifiedCalculationValue\n" +
                "                    end) / 100) + ((case\n" +
                "                      when cm1.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm1.modifiedCalculationValue\n" +
                "                    end) + (case\n" +
                "                      when cm2.modifiedCalculationValue is null then\n" +
                "                       0\n" +
                "                      else\n" +
                "                       cm2.modifiedCalculationValue\n" +
                "                    end)) FS\n" +
                "               from creditclients c\n" +
                "               left join ClientPriceModifierAssociation cm1\n" +
                "                 on c.id = cm1.creditClientId\n" +
                "                and cm1.sortOrder = '0'\n" +
                "               left join ClientPriceModifierAssociation cm2\n" +
                "                 on c.id = cm2.creditClientId\n" +
                "                and cm2.sortOrder = '1') tem\n" +
                "    on cc.id = tem.id\n" +
                " where consignerAccountNo not in ('0', '999')\n" +
                "   and isApproved = '1'\n" +
                // "   and z.zoneCode = '2'\n" +
                // "   and b.branchCode = '4'\n" +
                // "   and year(bookingDate) = '2015'\n" +
                // "   and month(bookingDate) = '12'\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                 clvar._Year + clvar._Month + clvar._TownCode + clvar._Zone + clvar._Type +

                " group by z.name,\n" +
                "          b.sname,\n" +
                "          case\n" +
                "            when bs1.firstName = bs1.lastName then\n" +
                "             bs1.firstName\n" +
                "            when bs1.lastName is null then\n" +
                "             bs1.firstName\n" +
                "            else\n" +
                "             bs1.firstName + ' ' + bs1.lastName\n" +
                "          end,\n" +
                "          case\n" +
                "            when bs2.firstName = bs2.lastName then\n" +
                "             bs2.firstName\n" +
                "            when bs2.lastName is null then\n" +
                "             bs2.firstName\n" +
                "            else\n" +
                "             bs2.firstName + ' ' + bs2.lastName\n" +
                "          end,\n" +
                "          datename(MM, c.bookingDate) + '-' + datename(YY, c.bookingDate),\n" +
                "          CONVERT(NVARCHAR(10), cc.regDate, 105),\n" +
                //  "          cc.regDate,\n" +
                "          datename(MM, cc.createdon) + '-' + datename(YY, cc.regDate),\n" +
                "          t.Products,\n" +
                "          cc.accountno,\n" +
                "          cc.name,\n" +
                "          cg.name,\n" +
                "          cast(tem.FS as varchar) + '%',\n" +
                "          case\n" +
                "            when (DATEDIFF(MONTH, cc.createdon, c.bookingDate)) < '4' then\n" +
                "             'NEW'\n" +
                "            else\n" +
                "             'OLD'\n" +
                "          end\n" +
                " ORDER BY 1, 2, 3, 4, 7, 5";


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

        public DataSet Get_RPS_RPK_Report(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "Select\n" +
                "b.FiscalYear,\n" +
                "b.Zone,\n" +
                "b.Branch,\n" +
                "b.[Sales Executive],\n" +
                "b.[ABH/ZM/CSH],\n" +
                "b.PERIOD,\n" +
                "b.Products,\n" +
                "b.[LIGHT SHIP],\n" +
                "b.[LIGHT REVENUE],\n" +
                "b.[HEAVY WEIGHT],\n" +
                "b.[HEAVY REVENUE],\n" +
                "round(case when  b.[LIGHT REVENUE]  <> 0  AND b.[LIGHT SHIP] <>0 then b.[LIGHT REVENUE]/b.[LIGHT SHIP] else 0 end,2) RPS,\n" +
                "round(case when  b.[HEAVY REVENUE]  <> 0  AND b.[HEAVY WEIGHT] <>0 then b.[HEAVY REVENUE]/b.[HEAVY WEIGHT] else 0 end,2) RPK from (\n" +
                "select\n" +
                "case\n" +
                "when MONTH(c.bookingDate)<'7' then cast((datename(YY,c.bookingDate)-1) as varchar)+'-'+RIGHT(YEAR(c.bookingDate), 2)\n" +
                "else cast(datename(YY,c.bookingDate)as varchar)+'-'+RIGHT(YEAR(c.bookingDate)+1, 2) end FiscalYear,\n" +
                "z.name Zone,\n" +
                "b.sname Branch,\n" +
                "case when bs1.firstName=bs1.lastName then bs1.firstName\n" +
                "when bs1.lastName is null then bs1.firstName\n" +
                "else bs1.firstName+' '+bs1.lastName end 'Sales Executive',\n" +
                "case when bs2.firstName=bs2.lastName then bs2.firstName\n" +
                "when bs2.lastName is null then bs2.firstName\n" +
                "else bs2.firstName+' '+bs2.lastName end 'ABH/ZM/CSH',\n" +
                "datename(MM,c.bookingDate)+'-'+datename(YY,c.bookingDate) 'PERIOD',\n" +
                "t.Products,\n" +
                "sum(case when c.weight<=.5 and c.serviceTypeName!='Road n Rail' then 1 end)'LIGHT SHIP',\n" +
                "sum(case when c.weight<=.5 and c.serviceTypeName!='Road n Rail' then c.totalAmount end)'LIGHT REVENUE',\n" +
                "sum(case when c.weight>.5 then c.weight when c.serviceTypeName='Road n Rail' then c.weight else 0 end) 'HEAVY WEIGHT',\n" +
                "sum(case when c.weight>.5 then c.totalAmount when c.serviceTypeName='Road n Rail' then c.totalAmount else 0 end) 'HEAVY REVENUE'\n" +
                "from Consignment c\n" +
                "inner join Branches b on c.orgin = b.branchCode\n" +
                "inner join Zones z on b.zoneCode = z.zoneCode\n" +
                "inner join CreditClients cc on c.creditClientId=cc.id\n" +
                "inner join ServiceTypes_New t on c.serviceTypeName=t.serviceTypeName\n" +
                "left JOIN ClientStaff cs1 on cc.id=cs1.ClientId and cs1.StaffTypeId='214'\n" +
                "left join BTSUsers bs1 on cs1.UserName=bs1.username\n" +
                "LEFT JOIN ClientGroups cg on cc.clientGrpId=cg.id\n" +
                "left join (SELECT\n" +
                "cc.id,\n" +
                "case\n" +
                "when cs1.UserName is null then cs2.UserName\n" +
                "when cs1.UserName is null and cs2.UserName is null then cs3.UserName\n" +
                "else cs1.UserName end UserName\n" +
                "FROM CreditClients cc\n" +
                "left JOIN ClientStaff cs1 on cc.id=cs1.ClientId and cs1.StaffTypeId='217'\n" +
                "left JOIN ClientStaff cs2 on cc.id=cs2.ClientId and cs2.StaffTypeId='220'\n" +
                "left JOIN ClientStaff cs3 on cc.id=cs3.ClientId and cs3.StaffTypeId='221') cs2 on cc.id=cs2.id\n" +
                "left join BTSUsers bs2 on cs2.UserName=bs2.username\n" +
                "where\n" +
                "isApproved='1'\n" +
                "and consignerAccountNo not in ('0','999')\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._TownCode + clvar._Zone +
                "\n" +
                "group by\n" +
                "case\n" +
                "when MONTH(c.bookingDate)<'7' then cast((datename(YY,c.bookingDate)-1) as varchar)+'-'+RIGHT(YEAR(c.bookingDate), 2)\n" +
                "else cast(datename(YY,c.bookingDate)as varchar)+'-'+RIGHT(YEAR(c.bookingDate)+1, 2) end ,\n" +
                "z.name ,\n" +
                "b.sname ,\n" +
                "case when bs1.firstName=bs1.lastName then bs1.firstName\n" +
                "when bs1.lastName is null then bs1.firstName\n" +
                "else bs1.firstName+' '+bs1.lastName end ,\n" +
                "case when bs2.firstName=bs2.lastName then bs2.firstName\n" +
                "when bs2.lastName is null then bs2.firstName\n" +
                "else bs2.firstName+' '+bs2.lastName end ,\n" +
                "datename(MM,c.bookingDate)+'-'+datename(YY,c.bookingDate) ,\n" +
                "t.Products\n" +
                ") b\n" +
                "ORDER BY 1,2,3,4,7,5";


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


        // Dormant Accounts Report Status Start
        public DataSet Get_DormantAccounts_Status(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT\n" +
                "b.id,\n" +
                "z.name Zone,\n" +
                "bb.sname Branch,\n" +
                "b.accountNo,\n" +
                "case when bs1.firstName=bs1.lastName then bs1.firstName\n" +
                "when bs1.lastName is null then bs1.firstName\n" +
                "else bs1.firstName+' '+bs1.lastName end 'Sales Executive',\n" +
                "case when bs2.firstName=bs2.lastName then bs2.firstName\n" +
                "when bs2.lastName is null then bs2.firstName\n" +
                "else bs2.firstName+' '+bs2.lastName end 'ABH/ZM/CSH',\n" +
                "cg.name 'PARENT NAME',\n" +
                "b.Status,b.StatusCode,\n" +
                "\n" +
                "sum(b.[1]) a,sum(b.[2]) b,sum(b.[3]) c,sum(b.[4]) d,sum(b.[5]) e,sum(b.[6]) f,sum(b.[7]) g,sum(b.[8]) h\n" +
                "from (select\n" +
                "c.id,c.accountNo,c.branchCode,c.isActive Status,c.StatusCode,c.clientGrpId,\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='1' then i2.Amount ELSE '0' END  '1',\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='2' then i2.Amount ELSE '0' END  '2',\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='3' then i2.Amount ELSE '0'  END '3',\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='4' then i2.Amount ELSE '0'  END '4',\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='5' then i2.Amount ELSE '0'  END '5',\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='6' then i2.Amount ELSE '0'  END '6',\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='7' then i2.Amount ELSE '0'  END '7',\n" +
                "case when DATEDIFF(MONTH,i2.startDate,getdate())='8' then i2.Amount ELSE '0'  END '8'\n" +
                "from CreditClients c\n" +
                "left join\n" +
                "(select i.clientId,i.startDate,sum(c.totalAmount)Amount from InvoiceConsignment ic,Invoice i , Consignment c\n" +
                "where ic.invoiceNumber=i.invoiceNumber\n" +
                "and ic.consignmentNumber=c.consignmentNumber\n" +
                "group by i.clientId,i.startDate) i2 on c.id=i2.clientId\n" +
                ") b\n" +
                "inner join Branches bb on b.branchCode = bb.branchCode\n" +
                "inner join Zones z on bb.zoneCode = z.zoneCode\n" +
                "left JOIN ClientStaff cs1 on b.id=cs1.ClientId and cs1.StaffTypeId='214'\n" +
                "left join BTSUsers bs1 on cs1.UserName=bs1.username\n" +
                "LEFT JOIN ClientGroups cg on b.clientGrpId=cg.id\n" +
                "left join (SELECT\n" +
                "cc.id,\n" +
                "case\n" +
                "when cs1.UserName is null then cs2.UserName\n" +
                "when cs1.UserName is null and cs2.UserName is null then cs3.UserName\n" +
                "else cs1.UserName end UserName\n" +
                "FROM CreditClients cc\n" +
                "left JOIN ClientStaff cs1 on cc.id=cs1.ClientId and cs1.StaffTypeId='217'\n" +
                "left JOIN ClientStaff cs2 on cc.id=cs2.ClientId and cs2.StaffTypeId='220'\n" +
                "left JOIN ClientStaff cs3 on cc.id=cs3.ClientId and cs3.StaffTypeId='221') cs2 on b.id=cs2.id\n" +
                "left join BTSUsers bs2 on cs2.UserName=bs2.username\n" +
                "where \n" +
                "b.Status='1'\n" +
                clvar._Zone + clvar._TownCode +
                "group by\n" +
                "b.id,\n" +
                "z.name ,\n" +
                "bb.sname ,\n" +
                "b.accountNo,\n" +
                "case when bs1.firstName=bs1.lastName then bs1.firstName\n" +
                "when bs1.lastName is null then bs1.firstName\n" +
                "else bs1.firstName+' '+bs1.lastName end ,\n" +
                "case when bs2.firstName=bs2.lastName then bs2.firstName\n" +
                "when bs2.lastName is null then bs2.firstName\n" +
                "else bs2.firstName+' '+bs2.lastName end ,\n" +
                "cg.name ,\n" +
                "b.Status,b.StatusCode\n" +
                 clvar._Type +
                "order by b.id ASC ";

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

        public DataSet Get_DormantAccounts_Date(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT\n" +
                "DATENAME(month, DATEADD(MONTH,-1,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-1,GETDATE())) AS month , 1 As snumber\n" +
                "union\n" +
                "select\n" +
                "DATENAME(month, DATEADD(MONTH,-2,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-2,GETDATE())) , 2 As snumber\n" +
                "union\n" +
                "select\n" +
                "DATENAME(month, DATEADD(MONTH,-3,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-3,GETDATE())) , 3 As snumber\n" +
                "union\n" +
                "select\n" +
                "DATENAME(month, DATEADD(MONTH,-4,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-4,GETDATE())) , 4 As snumber\n" +
                "union\n" +
                "select\n" +
                "DATENAME(month, DATEADD(MONTH,-5,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-5,GETDATE())) , 5 As snumber\n" +
                "union\n" +
                "select\n" +
                "DATENAME(month, DATEADD(MONTH,-6,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-6,GETDATE())) , 6 As snumber\n" +
                "union\n" +
                "select\n" +
                "DATENAME(month, DATEADD(MONTH,-7,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-7,GETDATE())) , 7 As snumber\n" +
                "union\n" +
                "select\n" +
                "DATENAME(month, DATEADD(MONTH,-8,GETDATE())) + ' ' +DATENAME(year, DATEADD(MONTH,-8,GETDATE())) , 8 As snumber\n" +
                "order by 2";

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
        // Dormant Accounts Report Status End

        public DataSet Get_GeneralTariffCN(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select c.consignmentNumber 'Consignment Number',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.consignerAccountNo 'Account No',\n" +
                "       cc.name 'Client Name',\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
                "       c.riderCode 'Rider Code',\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end Service,\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          'International'\n" +
                "         else\n" +
                "          zz.name\n" +
                "       end 'Destination Zone',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          ee.name\n" +
                "         else\n" +
                "          bb.name\n" +
                "       end 'Destination Branch',\n" +
                "       c.weight Weight,\n" +
                "       c.pieces PCS,\n" +
                "       c.totalAmount Amount,\n" +
                "       c.gst 'GST',\n" +
                "       case\n" +
                "         when c.isApproved = '1' then\n" +
                "          'Approved'\n" +
                "         else\n" +
                "          'Not Approved'\n" +
                "       end 'Approval State',\n" +
                "       'General Tariff CNs' 'Remarks'\n" +
                "  from Consignment c\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " INNER join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " INNER join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " INNER join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                "  left join Zones zz\n" +
                "    on bb.zoneCode = zz.zoneCode\n" +
                "  left join Country ee\n" +
                "    on c.destinationCountryCode = ee.Code\n" +
                " where \n" +
                "   isPriceComputed = '1'\n" +
                "   and c.consignerAccountNo NOT IN ('4D1', '0')\n" +
                "   and c.isNormalTariffApplied = '0' \n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + "\n" +
                " order by CONVERT(VARCHAR(10), c.bookingDate, 105) ASC";

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

        public DataSet Get_AbnormalAmount(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select c.consignmentNumber 'Consignment Number',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.consignerAccountNo 'Account No',\n" +
                "       cc.name 'Client Name',\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
                "       c.riderCode 'Rider Code',\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end Service,\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          'International'\n" +
                "         else\n" +
                "          zz.name\n" +
                "       end 'Destination Zone',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          ee.name\n" +
                "         else\n" +
                "          bb.name\n" +
                "       end 'Destination Branch',\n" +
                "       c.weight Weight,\n" +
                "       c.pieces PCS,\n" +
                "       c.totalAmount Amount,\n" +
                "       c.gst 'GST',\n" +
                "       case\n" +
                "         when c.isApproved = '1' then\n" +
                "          'Approved'\n" +
                "         else\n" +
                "          'Not Approved'\n" +
                "       end 'Approval State',\n" +
                "       'Abnormal Amount CNs' 'Remarks'\n" +
                "  from Consignment c\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " INNER join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " INNER join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " INNER join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                "  left join Zones zz\n" +
                "    on bb.zoneCode = zz.zoneCode\n" +
                "  left join Country ee\n" +
                "    on c.destinationCountryCode = ee.Code\n" +
                " where \n" +
                "   c.serviceTypeName != 'Aviation Sale'\n" +
                "   and c.ispricecomputed = '1'\n" +
                "   and c.consignerAccountNo != '0' \n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + clvar._Amount + "\n" +
                " order by CONVERT(VARCHAR(10), c.bookingDate, 105) ASC";


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

        public DataSet Get_WrongCashAmountReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select c.consignmentNumber 'Consignment Number',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.consignerAccountNo 'Account No',\n" +
                "       cc.name 'Client Name',\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
                "       c.riderCode 'Rider Code',\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end Service,\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          'International'\n" +
                "         else\n" +
                "          zz.name\n" +
                "       end 'Destination Zone',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          ee.name\n" +
                "         else\n" +
                "          bb.name\n" +
                "       end 'Destination Branch',\n" +
                "       c.weight Weight,\n" +
                "       c.pieces PCS,\n" +
                "       c.totalAmount Amount,\n" +
                "       c.gst 'GST',\n" +
                "       c.chargedAmount ChargedAmount,\n" +
                "       case\n" +
                "         when c.isApproved = '1' then\n" +
                "          'Approved'\n" +
                "         else\n" +
                "          'Not Approved'\n" +
                "       end 'Approval State',\n" +
                "       'Wrong Cash Amount' 'Remarks'\n" +
                "  from Consignment c\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " INNER join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " INNER join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " INNER join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                "  left join Zones zz\n" +
                "    on bb.zoneCode = zz.zoneCode\n" +
                "  left join Country ee\n" +
                "    on c.destinationCountryCode = ee.Code\n" +
                " where \n" +
                "   c.consignerAccountNo = '0'\n" +
                "   and c.isApproved = '1'\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + clvar._Amount + "\n" +
                " order by CONVERT(VARCHAR(10), c.bookingDate, 105) ASC";

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

        public DataSet Get_WrongServiceCNReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "(select\n" +
                "c.consignmentNumber 'Consignment Number',\n" +
                "z.name 'Origin Zone',\n" +
                "b.name 'Origin Branch',\n" +
                "c.consignerAccountNo 'Account No',\n" +
                "cc.name 'Client Name',\n" +
                "CONVERT(VARCHAR(10),c.bookingDate,105)'Booking Date',\n" +
                "c.riderCode 'Rider Code',\n" +
                "case when c.consignmentTypeId='13' and c.serviceTypeName='overnight' then 'Hand Carry' else c.serviceTypeName end Service,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then 'International' else zz.name end  'Destination Zone' ,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then ee.name else bb.name end  'Destination Branch' ,\n" +
                "c.weight Weight,\n" +
                "c.pieces PCS,\n" +
                "c.totalAmount Amount,\n" +
                "c.gst 'GST',\n" +
                "case when c.isApproved='1' then 'Approved' else 'Not Approved' end 'Approval State',\n" +
                "'Wrong Service' 'Remarks'\n" +
                "from Consignment c\n" +
                "inner join CreditClients cc on c.creditClientId=cc.id\n" +
                "INNER join Branches b on c.orgin=b.branchCode\n" +
                "INNER join Zones z  on b.zoneCode=z.zoneCode\n" +
                "INNER join Branches bb on c.destination=bb.branchCode\n" +
                "left join Zones zz on bb.zoneCode=zz.zoneCode\n" +
                "left join Country ee on c.destinationCountryCode=ee.Code\n" +
                "where\n" +
                "c.isApproved='1'\n" +
                "and left(c.consignmentNumber,1)in ('1','2')\n" +
                "and c.serviceTypeName='Road n Rail'\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + ")\n" +
                "\n" +
                "union\n" +
                "(select\n" +
                "c.consignmentNumber 'Consignment Number',\n" +
                "z.name 'Origin Zone',\n" +
                "b.name 'Origin Branch',\n" +
                "c.consignerAccountNo 'Account No',\n" +
                "cc.name 'Client Name',\n" +
                "CONVERT(VARCHAR(10),c.bookingDate,105)'Booking Date',\n" +
                "c.riderCode 'Rider Code',\n" +
                "case when c.consignmentTypeId='13' and c.serviceTypeName='overnight' then 'Hand Carry' else c.serviceTypeName end Service,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then 'International' else zz.name end  'Destination Zone' ,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then ee.name else bb.name end  'Destination Branch' ,\n" +
                "c.weight Weight,\n" +
                "c.pieces PCS,\n" +
                "c.totalAmount Amount,\n" +
                "c.gst 'GST',\n" +
                "case when c.isApproved='1' then 'Approved' else 'Not Approved' end 'Approval State',\n" +
                "'Wrong Service' 'Remarks'\n" +
                "from Consignment c\n" +
                "inner join CreditClients cc on c.creditClientId=cc.id\n" +
                "INNER join Branches b on c.orgin=b.branchCode\n" +
                "INNER join Zones z  on b.zoneCode=z.zoneCode\n" +
                "INNER join Branches bb on c.destination=bb.branchCode\n" +
                "left join Zones zz on bb.zoneCode=zz.zoneCode\n" +
                "left join Country ee on c.destinationCountryCode=ee.Code\n" +
                "where\n" +
                "c.isApproved='1'\n" +
                "and left(c.consignmentNumber,1)in ('6','7')\n" +
                "and left(c.consignmentNumber,1)!='799'\n" +
                "and c.serviceTypeName!='Road n Rail'\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + ")\n" +
                "\n" +
                "union\n" +
                "\n" +
                "(select\n" +
                "c.consignmentNumber 'Consignment Number',\n" +
                "z.name 'Origin Zone',\n" +
                "b.name 'Origin Branch',\n" +
                "c.consignerAccountNo 'Account No',\n" +
                "cc.name 'Client Name',\n" +
                "CONVERT(VARCHAR(10),c.bookingDate,105)'Booking Date',\n" +
                "c.riderCode 'Rider Code',\n" +
                "case when c.consignmentTypeId='13' and c.serviceTypeName='overnight' then 'Hand Carry' else c.serviceTypeName end Service,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then 'International' else zz.name end  'Destination Zone' ,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then ee.name else bb.name end  'Destination Branch' ,\n" +
                "c.weight Weight,\n" +
                "c.pieces PCS,\n" +
                "c.totalAmount Amount,\n" +
                "c.gst 'GST',\n" +
                "case when c.isApproved='1' then 'Approved' else 'Not Approved' end 'Approval State',\n" +
                "'Wrong Service' 'Remarks'\n" +
                "from Consignment c\n" +
                "inner join CreditClients cc on c.creditClientId=cc.id\n" +
                "INNER join Branches b on c.orgin=b.branchCode\n" +
                "INNER join Zones z  on b.zoneCode=z.zoneCode\n" +
                "INNER join Branches bb on c.destination=bb.branchCode\n" +
                "left join Zones zz on bb.zoneCode=zz.zoneCode\n" +
                "left join Country ee on c.destinationCountryCode=ee.Code\n" +
                "where\n" +
                " c.isApproved='1'\n" +
                "and left(c.consignmentNumber,1)!='9'\n" +
                "and c.serviceTypeName in ('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex','International_Doc_Special_Hub','International_Doc')\n" +
                "\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + ")\n" +
                "\n" +
                "union\n" +
                "(select\n" +
                "c.consignmentNumber 'Consignment Number',\n" +
                "z.name 'Origin Zone',\n" +
                "b.name 'Origin Branch',\n" +
                "c.consignerAccountNo 'Account No',\n" +
                "cc.name 'Client Name',\n" +
                "CONVERT(VARCHAR(10),c.bookingDate,105)'Booking Date',\n" +
                "c.riderCode 'Rider Code',\n" +
                "case when c.consignmentTypeId='13' and c.serviceTypeName='overnight' then 'Hand Carry' else c.serviceTypeName end Service,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then 'International' else zz.name end  'Destination Zone' ,\n" +
                "case when c.serviceTypeName in  ('International Cargo','International Expressions','International_Box','International_Doc','International_Doc_Special_Hub','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex') then ee.name else bb.name end  'Destination Branch' ,\n" +
                "c.weight Weight,\n" +
                "c.pieces PCS,\n" +
                "c.totalAmount Amount,\n" +
                "c.gst 'GST',\n" +
                "case when c.isApproved='1' then 'Approved' else 'Not Approved' end 'Approval State',\n" +
                "'Wrong Service' 'Remarks'\n" +
                "from Consignment c\n" +
                "inner join CreditClients cc on c.creditClientId=cc.id\n" +
                "INNER join Branches b on c.orgin=b.branchCode\n" +
                "INNER join Zones z  on b.zoneCode=z.zoneCode\n" +
                "INNER join Branches bb on c.destination=bb.branchCode\n" +
                "left join Zones zz on bb.zoneCode=zz.zoneCode\n" +
                "left join Country ee on c.destinationCountryCode=ee.Code\n" +
                "where\n" +
                "c.isApproved='1'\n" +
                "and left(c.consignmentNumber,1)='9'\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + "\n" +
                "and c.serviceTypeName not in ('International_Box','International Cargo','International_Non-Doc','International_Non-Doc_Special_Hub_2014','Logex','International_Doc_Special_Hub','International_Doc') ) \n" +
                " order by CONVERT(VARCHAR(10), c.bookingDate, 105) ASC";

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

        public DataSet Get_ZeroAmountReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select c.consignmentNumber 'Consignment Number',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.consignerAccountNo 'Account No',\n" +
                "       cc.name 'Client Name',\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
                "       c.riderCode 'Rider Code',\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end Service,\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          'International'\n" +
                "         else\n" +
                "          zz.name\n" +
                "       end 'Destination Zone',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          ee.name\n" +
                "         else\n" +
                "          bb.name\n" +
                "       end 'Destination Branch',\n" +
                "       c.weight Weight,\n" +
                "       c.pieces PCS,\n" +
                "       c.totalAmount Amount,\n" +
                "       c.gst 'GST',\n" +
                "       case\n" +
                "         when c.isApproved = '1' then\n" +
                "          'Approved'\n" +
                "         else\n" +
                "          'Not Approved'\n" +
                "       end 'Approval State',\n" +
                "       'Zero Amount' 'Remarks'\n" +
                "  from Consignment c\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " INNER join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " INNER join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " INNER join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                "  left join Zones zz\n" +
                "    on bb.zoneCode = zz.zoneCode\n" +
                "  left join Country ee\n" +
                "    on c.destinationCountryCode = ee.Code\n" +
                " where \n" +
                //  "   and month(c.bookingDate) = '05'\n" +
                "   c.totalAmount = '0'\n" +
                "   and c.consignerAccountNo not in ('0', '4D1') \n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + clvar._Amount + "\n" +
                " order by CONVERT(VARCHAR(10), c.bookingDate, 105) ASC";

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

        public DataSet Get_PricenotComputedReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select c.consignmentNumber 'Consignment Number',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.consignerAccountNo 'Account No',\n" +
                "       cc.name 'Client Name',\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
                "       c.riderCode 'Rider Code',\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end Service,\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          'International'\n" +
                "         else\n" +
                "          zz.name\n" +
                "       end 'Destination Zone',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          ee.name\n" +
                "         else\n" +
                "          bb.name\n" +
                "       end 'Destination Branch',\n" +
                "       c.weight Weight,\n" +
                "       c.pieces PCS,\n" +
                "       c.totalAmount Amount,\n" +
                "       c.gst 'GST',\n" +
                "       c.chargedAmount ChargedAmount,\n" +
                "       case\n" +
                "         when c.isApproved = '1' then\n" +
                "          'Approved'\n" +
                "         else\n" +
                "          'Not Approved'\n" +
                "       end 'Approval State',\n" +
                "       'Price not Computed CNs' 'Remarks'\n" +
                "  from Consignment c\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " INNER join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " INNER join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " INNER join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                "  left join Zones zz\n" +
                "    on bb.zoneCode = zz.zoneCode\n" +
                "  left join Country ee\n" +
                "    on c.destinationCountryCode = ee.Code\n" +
                " where \n" +
                "   c.isApproved = '1'\n" +
                "   and c.isPriceComputed = '0' \n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + "\n" +
                " order by CONVERT(VARCHAR(10), c.bookingDate, 105) ASC";

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

        public DataSet Get_ClientsBillingModeManual(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "/*\n" +
                "select c.consignmentNumber 'Consignment Number',\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       c.consignerAccountNo 'Account No',\n" +
                "       cc.name 'Client Name',\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
                "       c.riderCode 'Rider Code',\n" +
                "       case\n" +
                "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
                "          'Hand Carry'\n" +
                "         else\n" +
                "          c.serviceTypeName\n" +
                "       end Service,\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          'International'\n" +
                "         else\n" +
                "          zz.name\n" +
                "       end 'Destination Zone',\n" +
                "       case\n" +
                "         when c.serviceTypeName in ('International Cargo',\n" +
                "                                    'International Expressions',\n" +
                "                                    'International_Box',\n" +
                "                                    'International_Doc',\n" +
                "                                    'International_Doc_Special_Hub',\n" +
                "                                    'International_Non-Doc',\n" +
                "                                    'International_Non-Doc_Special_Hub_2014',\n" +
                "                                    'Logex') then\n" +
                "          ee.name\n" +
                "         else\n" +
                "          bb.name\n" +
                "       end 'Destination Branch',\n" +
                "       c.weight Weight,\n" +
                "       c.pieces PCS,\n" +
                "       c.totalAmount Amount,\n" +
                "       c.gst 'GST',\n" +
                "       c.chargedAmount ChargedAmount,\n" +
                "       case\n" +
                "         when c.isApproved = '1' then\n" +
                "          'Approved'\n" +
                "         else\n" +
                "          'Not Approved'\n" +
                "       end 'Approval State',\n" +
                "       'Wrong Cash Amount' 'Remarks'\n" +
                "  from Consignment c\n" +
                " inner join CreditClients cc\n" +
                "    on c.creditClientId = cc.id\n" +
                " INNER join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " INNER join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                " INNER join Branches bb\n" +
                "    on c.destination = bb.branchCode\n" +
                "  left join Zones zz\n" +
                "    on bb.zoneCode = zz.zoneCode\n" +
                "  left join Country ee\n" +
                "    on c.destinationCountryCode = ee.Code\n" +
                " where\n" +
                "   c.consignerAccountNo = '0'\n" +
                "   and c.isApproved = '1'\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9')\n" +
                " AND year(c.bookingDate) = '2016'  AND MONTH(c.bookingDate) = '05'   AND c.chargedAmount <'2'\n" +
                " order by CONVERT(VARCHAR(10), c.bookingDate, 105) ASC\n" +
                "\n" +
                " */\n" +
                "\n" +
                "select cc.id,\n" +
                "       CONVERT(VARCHAR(10), cc.createdOn, 105) 'Created ON',\n" +
                "       cc.createdby,\n" +
                "       z.name 'Origin Zone',\n" +
                "       b.name 'Origin Branch',\n" +
                "       cc.accountNo 'Account No',\n" +
                "       Case\n" +
                "         when cc.billingMode = '1' then\n" +
                "          'Auto'\n" +
                "         Else\n" +
                "          'Manual'\n" +
                "       end 'Billing Mode',\n" +
                "       z.name + '-' + b.sname + '-' + cc.accountNo 'Client Account',\n" +
                "       cc.name 'Client Name',\n" +
                "       cc.contactPerson 'Contact Person',\n" +
                "       CONVERT(VARCHAR(10), cc.regDate, 105) 'Registration Date',\n" +
                "       CONVERT(VARCHAR(10), cc.regEndDate, 105) 'Registration End Date',\n" +
                "       case\n" +
                "         when cc.centralizedClient = '1' then\n" +
                "          'Centralized'\n" +
                "         else\n" +
                "          ' '\n" +
                "       end 'Centralized Indicator ',\n" +
                "       case\n" +
                "         when cc.IsCOD = '1' then\n" +
                "          'COD'\n" +
                "         else\n" +
                "          ' '\n" +
                "       end 'COD Indicator ',\n" +
                "       cc.StatusCode,\n" +
                "       case\n" +
                "         when isActive = '1' then\n" +
                "          'Active'\n" +
                "         else\n" +
                "          'In Active'\n" +
                "       end 'Client Status',\n" +
                "       count(c.consignmentNumber) 'No of Consignment in Current Month'\n" +
                "  from CreditClients cc\n" +
                " INNER join Branches b\n" +
                "    on cc.branchCode = b.branchCode\n" +
                " INNER join Zones z\n" +
                "    on b.zoneCode = z.zoneCode\n" +
                "  left join Consignment c\n" +
                "    on c.creditClientId = cc.id\n" +
                "   and year(bookingDate) = year(getDate())\n" +
                "   and month(bookingDate) = month(getDate())\n" +
                " WHERE cc.billingMode != '1'\n" +
                "   and cc.isActive = '1'\n" +
                "   and z.zoneCode = '2'\n" +
                "   and b.branchCode = '4'\n" +
                " group by cc.id,\n" +
                "          cc.createdOn,\n" +
                "          cc.createdby,\n" +
                "          z.name,\n" +
                "          b.name,\n" +
                "          cc.accountNo,\n" +
                "          Case\n" +
                "            when cc.billingMode = '1' then\n" +
                "             'Auto'\n" +
                "            Else\n" +
                "             'Manual'\n" +
                "          end,\n" +
                "          z.name + '-' + b.sname + '-' + cc.accountNo,\n" +
                "          cc.name,\n" +
                "          cc.contactPerson,\n" +
                "          cc.regDate,\n" +
                "          cc.regEndDate,\n" +
                "          case\n" +
                "            when cc.centralizedClient = '1' then\n" +
                "             'Centralized'\n" +
                "            else\n" +
                "             ' '\n" +
                "          end,\n" +
                "          case\n" +
                "            when cc.IsCOD = '1' then\n" +
                "             'COD'\n" +
                "            else\n" +
                "             ' '\n" +
                "          end,\n" +
                "          cc.StatusCode,\n" +
                "          case\n" +
                "            when isActive = '1' then\n" +
                "             'Active'\n" +
                "            else\n" +
                "             'In Active'\n" +
                "          end\n" +
                " AND z.zoneCode in ('1','10','11','12','2','27','3','4','5','7','9') \n" +
                clvar._Year + clvar._Month + clvar._Zone + clvar._TownCode + "\n" +
                " order by cc.createdOn ASC";

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

        public DataTable Get_AccountwiseBookingReport(Variable clvar)
        {

            string sqlString = "select c.consignmentNumber 'Consignment Number',\n" +
            "       z.name 'Origin Zone',\n" +
            "       b.name 'Origin Branch',\n" +
            "       cc.accountNo 'Account No',\n" +
            "       z.name + '-' + b.sname + '-' + cc.accountNo 'Client Account No',\n" +
            "       b.sname + '-' + c.riderCode 'Booking Code',\n" +
            "       cc.name 'Client Name',\n" +
            "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'Booking Date',\n" +
            "       case\n" +
            "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
            "          'Hand Carry'\n" +
            "         else\n" +
            "          c.serviceTypeName\n" +
            "       end Service,\n" +
            "       t.Products,\n" +
            "       case\n" +
            "         when c.serviceTypeName in ('International Cargo',\n" +
            "                                    'International Expressions',\n" +
            "                                    'International_Box',\n" +
            "                                    'International_Doc',\n" +
            "                                    'International_Doc_Special_Hub',\n" +
            "                                    'International_Non-Doc',\n" +
            "                                    'International_Non-Doc_Special_Hub_2014',\n" +
            "                                    'Logex') then\n" +
            "          c.destinationCountryCode\n" +
            "         else\n" +
            "          bb.name\n" +
            "       end 'Destination Branch',\n" +
            "       case\n" +
            "         when c.serviceTypeName in ('International Cargo',\n" +
            "                                    'International Expressions',\n" +
            "                                    'International_Box',\n" +
            "                                    'International_Doc',\n" +
            "                                    'International_Doc_Special_Hub',\n" +
            "                                    'International_Non-Doc',\n" +
            "                                    'International_Non-Doc_Special_Hub_2014',\n" +
            "                                    'Logex') then\n" +
            "          'International'\n" +
            "         when c.orgin = c.destination then\n" +
            "          'local'\n" +
            "         when c.orgin != c.destination and z.colorId = zz.colorId then\n" +
            "          'Same Zone'\n" +
            "         when c.orgin != c.destination and z.colorId != zz.colorId then\n" +
            "          'Diff Zone'\n" +
            "       end 'Zoning',\n" +
            "       c.weight Weight,\n" +
            "       c.pieces,\n" +
            "       c.TotalAmount 'Amount'\n" +
            "  from Consignment c\n" +
            " inner join creditclients cc\n" +
            "    on c.creditClientId = cc.id\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Zones z\n" +
            "    on b.zoneCode = z.zoneCode\n" +
            " inner join Branches bb\n" +
            "    on c.destination = bb.branchCode\n" +
            " inner join ServiceTypes_New t\n" +
            "    on c.serviceTypeName = t.serviceTypeName\n" +
            " inner join Zones zz\n" +
            "    on bb.zoneCode = zz.zoneCode\n" +
            " where CAST(c.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "'\n" +
            "   and c.orgin = '" + clvar.Expresscentercode + "'\n" +
            "   and c.consignerAccountNo = '" + clvar.ACNumber + "'";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;

        }

        public DataTable Get_AccountWiseDeliveryReport(Variable clvar)
        {


            string sqlString = "Select *\n" +
            "  from (select c.consignmentNumber 'Consignment Number',\n" +
            "               z.name 'Origin Zone',\n" +
            "               b.name 'Origin Branch',\n" +
            "               cc.accountNo 'Account No',\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo 'Client Account No',\n" +
            "               b.sname + '-' + c.riderCode 'Booking Code',\n" +
            "               cc.name 'Client Name',\n" +
            "\n" +
            "               CONVERT(VARCHAR(10), c.bookingDate, 105) BookingDate,\n" +
            "               case\n" +
            "                 when c.consignmentTypeId = '13' and\n" +
            "                      c.serviceTypeName = 'overnight' then\n" +
            "                  'Hand Carry'\n" +
            "                 else\n" +
            "                  c.serviceTypeName\n" +
            "               end Service,\n" +
            "               t.Products,\n" +
            "               case\n" +
            "                 when c.serviceTypeName in ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') then\n" +
            "                  c.destinationCountryCode\n" +
            "                 else\n" +
            "                  bb.name\n" +
            "               end 'Destination Branch',\n" +
            "               case\n" +
            "                 when c.serviceTypeName in ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') then\n" +
            "                  'International'\n" +
            "                 when c.orgin = c.destination then\n" +
            "                  'local'\n" +
            "                 when c.orgin != c.destination and z.colorId = zz.colorId then\n" +
            "                  'Same Zone'\n" +
            "                 when c.orgin != c.destination and z.colorId != zz.colorId then\n" +
            "                  'Diff Zone'\n" +
            "               end 'Zoning',\n" +
            "               c.weight Weight,\n" +
            "               c.pieces,\n" +
            "               c.TotalAmount 'Amount',\n" +
            "               case\n" +
            "                 when rr.Status is null then\n" +
            "                  'In Transit'\n" +
            "                 else\n" +
            "                  rr.Status\n" +
            "               end Status,\n" +
            "               rr.runsheetNumber 'RunSheet Number',\n" +
            "               rr.receivedBy 'Received By',\n" +
            "               rr.deliveryDate 'Delivery Date',\n" +
            "               rr.time 'Time'\n" +
            "          from Consignment c\n" +
            "         inner join creditclients cc\n" +
            "            on c.creditClientId = cc.id\n" +
            "         inner join Branches b\n" +
            "            on c.orgin = b.branchCode\n" +
            "         inner join Zones z\n" +
            "            on b.zoneCode = z.zoneCode\n" +
            "         inner join Branches bb\n" +
            "            on c.destination = bb.branchCode\n" +
            "         inner join ServiceTypes_New t\n" +
            "            on c.serviceTypeName = t.serviceTypeName\n" +
            "         inner join Zones zz\n" +
            "            on bb.zoneCode = zz.zoneCode\n" +
            "          left join (select rc2.*\n" +
            "                      from (select consignmentNumber,\n" +
            "                                   max(createdOn) as MaxcreatedOn\n" +
            "                              from RunsheetConsignment\n" +
            "                             group by consignmentNumber) rc1\n" +
            "                     inner join RunsheetConsignment rc2\n" +
            "                        on rc1.consignmentNumber = rc2.consignmentNumber\n" +
            "                       and rc1.MaxcreatedOn = rc2.createdOn) rr\n" +
            "            on c.consignmentNumber = rr.consignmentNumber\n" +
            "         where c.orgin = '" + clvar.Expresscentercode + "'\n" +
            "               " + clvar.Check_Condition + "" +
            "           and c.consignerAccountNo = '" + clvar.ACNumber + "') w\n" +
            " where CAST(w.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "'";

            sqlString = "Select w.*,\n" +
           "       case\n" +
           "         when rc.Status is null then\n" +
           "          'In Transit'\n" +
           "         else\n" +
           "          rc.Status\n" +
           "       end Status,\n" +
           "       rc.runsheetNumber 'RunSheet Number',\n" +
           "       rc.receivedBy 'Received By',\n" +
           "       rc.deliveryDate 'Delivery Date',\n" +
           "       rc.time 'Time'\n" +
           "  from (select c.consignmentNumber,\n" +
           "               z.name 'Origin Zone',\n" +
           "               b.name 'Origin Branch',\n" +
           "               cc.accountNo 'Account No',\n" +
           "               z.name + '-' + b.sname + '-' + cc.accountNo 'Client Account No',\n" +
           "               b.sname + '-' + c.riderCode 'Booking Code',\n" +
           "               cc.name 'Client Name',\n" +
           "\n" +
           "               c.bookingDate BookingDate,\n" +
           "               case\n" +
           "                 when c.consignmentTypeId = '13' and\n" +
           "                      c.serviceTypeName = 'overnight' then\n" +
           "                  'Hand Carry'\n" +
           "                 else\n" +
           "                  c.serviceTypeName\n" +
           "               end Service,\n" +
           "               t.Products,\n" +
           "               case\n" +
           "                 when c.serviceTypeName in ('International Cargo',\n" +
           "                                            'International Expressions',\n" +
           "                                            'International_Box',\n" +
           "                                            'International_Doc',\n" +
           "                                            'International_Doc_Special_Hub',\n" +
           "                                            'International_Non-Doc',\n" +
           "                                            'International_Non-Doc_Special_Hub_2014',\n" +
           "                                            'Logex') then\n" +
           "                  c.destinationCountryCode\n" +
           "                 else\n" +
           "                  bb.name\n" +
           "               end 'Destination Branch',\n" +
           "               case\n" +
           "                 when c.serviceTypeName in ('International Cargo',\n" +
           "                                            'International Expressions',\n" +
           "                                            'International_Box',\n" +
           "                                            'International_Doc',\n" +
           "                                            'International_Doc_Special_Hub',\n" +
           "                                            'International_Non-Doc',\n" +
           "                                            'International_Non-Doc_Special_Hub_2014',\n" +
           "                                            'Logex') then\n" +
           "                  'International'\n" +
           "                 when c.orgin = c.destination then\n" +
           "                  'local'\n" +
           "                 when c.orgin != c.destination and z.colorId = zz.colorId then\n" +
           "                  'Same Zone'\n" +
           "                 when c.orgin != c.destination and z.colorId != zz.colorId then\n" +
           "                  'Diff Zone'\n" +
           "               end 'Zoning',\n" +
           "               c.weight Weight,\n" +
           "               c.pieces,\n" +
           "               c.TotalAmount 'Amount'\n" +
           "\n" +
           "          from Consignment c\n" +
           "         inner join creditclients cc\n" +
           "            on c.creditClientId = cc.id\n" +
           "         inner join Branches b\n" +
           "            on c.orgin = b.branchCode\n" +
           "         inner join Zones z\n" +
           "            on b.zoneCode = z.zoneCode\n" +
           "         inner join Branches bb\n" +
           "            on c.destination = bb.branchCode\n" +
           "         inner join ServiceTypes_New t\n" +
           "            on c.serviceTypeName = t.serviceTypeName\n" +
           "         inner join Zones zz\n" +
           "            on bb.zoneCode = zz.zoneCode\n" +
           "\n" +
           "         where c.orgin = '" + clvar.Expresscentercode + "'\n" +
           "               " + clvar.Check_Condition + "" +
           "           and c.consignerAccountNo = '" + clvar.ACNumber + "') w\n" +
           "  left outer join RunsheetConsignment rc\n" +
           "    on rc.consignmentNumber = w.consignmentNumber\n" +
           "   and rc.createdOn =\n" +
           "       (SELECT MAX(CREATEDON)\n" +
           "          FROM RunsheetConsignment rc1\n" +
           "         where rc1.consignmentNumber = rc.consignmentNumber)\n" +
           " where CAST(w.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "'";


            sqlString = "select w.consignmentNumber,\n" +
            "       w.OriginZone,\n" +
            "       w.OriginBranch,\n" +
            "       w.AccountNo,\n" +
            "       w.ClientAccountNo,\n" +
            "       w.BookingCode,\n" +
            "       w.BookingDate,\n" +
            "       w.ClientName,\n" +
            "       w.Service,\n" +
            "       w.Products,\n" +
            "       w.DestinationBranch,\n" +
            "       w.Zoning,\n" +
            "       w.Weight,\n" +
            "       w.pieces,\n" +
            "       w.Amount,\n" +
            "       CASE WHEN MAX(w.Status) = '' THEN 'In Transit' ELSE MAX(w.Status) end STATUS ,\n" +
            "       MAX(w.RunSheetNumber) RunSheetNumber,\n" +
            "       MAX(w.ReceivedBy) ReceivedBy,\n" +
            "       MAX(w.DeliveryDate) DeliveryDate,\n" +
            "       MAX(w.Time) Time\n" +
            "  from (select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "\n" +
            "               c.bookingDate BookingDate,\n" +
            "               case\n" +
            "                 when c.consignmentTypeId = '13' and\n" +
            "                      c.serviceTypeName = 'overnight' then\n" +
            "                  'Hand Carry'\n" +
            "                 else\n" +
            "                  c.serviceTypeName\n" +
            "               end Service,\n" +
            "               t.Products,\n" +
            "               case\n" +
            "                 when c.serviceTypeName in ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') then\n" +
            "                  c.destinationCountryCode\n" +
            "                 else\n" +
            "                  bb.name\n" +
            "               end DestinationBranch,\n" +
            "               case\n" +
            "                 when c.serviceTypeName in ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') then\n" +
            "                  'International'\n" +
            "                 when c.orgin = c.destination then\n" +
            "                  'local'\n" +
            "                 when c.orgin != c.destination and z.colorId = zz.colorId then\n" +
            "                  'Same Zone'\n" +
            "                 when c.orgin != c.destination and z.colorId != zz.colorId then\n" +
            "                  'Diff Zone'\n" +
            "               end Zoning,\n" +
            "               c.weight Weight,\n" +
            "               c.pieces,\n" +
            "               c.TotalAmount Amount,\n" +
            "               '' Status,\n" +
            "               '' RunSheetNumber,\n" +
            "               '' ReceivedBy,\n" +
            "               '' DeliveryDate,\n" +
            "               '' Time\n" +
            "          from Consignment c\n" +
            "         inner join creditclients cc\n" +
            "            on c.creditClientId = cc.id\n" +
            "         inner join Branches b\n" +
            "            on c.orgin = b.branchCode\n" +
            "         inner join Zones z\n" +
            "            on b.zoneCode = z.zoneCode\n" +
            "         inner join Branches bb\n" +
            "            on c.destination = bb.branchCode\n" +
            "         inner join ServiceTypes_New t\n" +
            "            on c.serviceTypeName = t.serviceTypeName\n" +
            "         inner join Zones zz\n" +
            "            on bb.zoneCode = zz.zoneCode\n" +
            "         where c.orgin = '" + clvar.Expresscentercode + "'\n" +
           "               " + clvar.Check_Condition + "" +
           "           and c.consignerAccountNo = '" + clvar.ACNumber + "'\n" +
            "\n" +
            "        union\n" +
            "\n" +
            "        select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "               c.bookingDate BookingDate,\n" +
            "               case\n" +
            "                 when c.consignmentTypeId = '13' and\n" +
            "                      c.serviceTypeName = 'overnight' then\n" +
            "                  'Hand Carry'\n" +
            "                 else\n" +
            "                  c.serviceTypeName\n" +
            "               end Service,\n" +
            "               t.Products,\n" +
            "               case\n" +
            "                 when c.serviceTypeName in ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') then\n" +
            "                  c.destinationCountryCode\n" +
            "                 else\n" +
            "                  bb.name\n" +
            "               end DestinationBranch,\n" +
            "               case\n" +
            "                 when c.serviceTypeName in ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') then\n" +
            "                  'International'\n" +
            "                 when c.orgin = c.destination then\n" +
            "                  'local'\n" +
            "                 when c.orgin != c.destination and z.colorId = zz.colorId then\n" +
            "                  'Same Zone'\n" +
            "                 when c.orgin != c.destination and z.colorId != zz.colorId then\n" +
            "                  'Diff Zone'\n" +
            "               end Zoning,\n" +
            "               c.weight Weight,\n" +
            "               c.pieces,\n" +
            "               c.TotalAmount Amount,\n" +
            "               rc.Status,\n" +
            "               --end Status,\n" +
            "               rc.runsheetNumber RunSheetNumber,\n" +
            "               rc.receivedBy     ReceivedBy,\n" +
            "               rc.deliveryDate   DeliveryDate,\n" +
            "               rc.time           Time\n" +
            "          from Consignment c\n" +
            "         inner join creditclients cc\n" +
            "            on c.creditClientId = cc.id\n" +
            "         inner join Branches b\n" +
            "            on c.orgin = b.branchCode\n" +
            "         inner join Zones z\n" +
            "            on b.zoneCode = z.zoneCode\n" +
            "         inner join Branches bb\n" +
            "            on c.destination = bb.branchCode\n" +
            "         inner join ServiceTypes_New t\n" +
            "            on c.serviceTypeName = t.serviceTypeName\n" +
            "         inner join Zones zz\n" +
            "            on bb.zoneCode = zz.zoneCode\n" +
            "         inner join RunsheetConsignment rc\n" +
            "            on rc.consignmentNumber = c.consignmentNumber\n" +
            "           and rc.createdOn =\n" +
            "               (select MAX(createdon)\n" +
            "                  from RunsheetConsignment rc1\n" +
            "                 where rc1.consignmentNumber = rc.consignmentNumber)\n" +
            "         where c.orgin = '" + clvar.Expresscentercode + "'\n" +
           "               " + clvar.Check_Condition + "" +
           "           and c.consignerAccountNo = '" + clvar.ACNumber + "') w \n" +
           " where CAST(w.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "'\n" +
            " group by w.consignmentNumber,\n" +
            "          w.OriginZone,\n" +
            "          w.OriginBranch,\n" +
            "          w.AccountNo,\n" +
            "          w.ClientAccountNo,\n" +
            "          w.BookingCode,\n" +
            "          w.BookingDate,\n" +
            "          w.ClientName,\n" +
            "          w.Service,\n" +
            "          w.Products,\n" +
            "          w.DestinationBranch,\n" +
            "          w.Zoning,\n" +
            "          w.Weight,\n" +
            "          w.pieces,\n" +
            "          w.Amount\n" +
            "\n" +
            " order by 1";

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                //var temp = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                //temp.ConnectTimeout = 300;
                //orcl = new SqlConnection(temp.ConnectionString);
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;
        }

        public DataTable Get_CustomerWiseSales(Variable clvar)
        {

            string sqlString = "select case\n" +
            "         when MONTH(c.bookingDate) < '7' then\n" +
            "          cast((datename(YY, c.bookingDate) - 1) as varchar) + '-' +\n" +
            "          RIGHT(YEAR(c.bookingDate), 2)\n" +
            "         else\n" +
            "          cast(datename(YY, c.bookingDate) as varchar) + '-' +\n" +
            "          RIGHT(YEAR(c.bookingDate) + 1, 2)\n" +
            "       end 'FiscalYear',\n" +
            "       z.name Zone,\n" +
            "       b.sname Branch,\n" +
            "       datename(MM, c.bookingDate) + '-' + datename(YY, c.bookingDate) 'PERIOD',\n" +
            "       cc.regDate,\n" +
            "\n" +
            "       case\n" +
            "         when (DATEDIFF(MONTH, cc.createdon, getdate())) < '4' then\n" +
            "          'NEW'\n" +
            "         else\n" +
            "          'OLD'\n" +
            "       end 'NATURE',\n" +
            "       case\n" +
            "         when bs1.firstName = bs1.lastName then\n" +
            "          bs1.firstName\n" +
            "         when bs1.lastName is null then\n" +
            "          bs1.firstName\n" +
            "         else\n" +
            "          bs1.firstName + ' ' + bs1.lastName\n" +
            "       end 'Sales Executive',\n" +
            "       case\n" +
            "         when bs2.firstName = bs2.lastName then\n" +
            "          bs2.firstName\n" +
            "         when bs2.lastName is null then\n" +
            "          bs2.firstName\n" +
            "         else\n" +
            "          bs2.firstName + ' ' + bs2.lastName\n" +
            "       end 'ABH/ZM/CSH',\n" +
            "       cc.accountno 'Client Account Number',\n" +
            "       cc.name 'Client Name',\n" +
            "       cg.name 'Parent Name',\n" +
            "       t.Products,\n" +
            "       sum(case\n" +
            "             when c.serviceTypeName = 'Road n Rail' and c.weight < 5 then\n" +
            "              '5'\n" +
            "             else\n" +
            "              c.weight\n" +
            "           end) 'WT',\n" +
            "       COUNT(c.consignmentNumber) SHIPMENTS,\n" +
            "       round(sum(totalAmount), 2) REVENUE,\n" +
            "       cast(tem.FS as varchar) + '%' 'FUEL%',\n" +
            "       round(sum((totalAmount) * tem.FS) / 100, 2) FUEL,\n" +
            "       round(sum((totalAmount + gst) +\n" +
            "                 (((totalAmount + gst) * tem.FS) / 100)),\n" +
            "             2) GROSS_REVENUE\n" +
            "\n" +
            "  from Consignment c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Zones z\n" +
            "    on b.zoneCode = z.zoneCode\n" +
            " inner join CreditClients cc\n" +
            "    on c.creditClientId = cc.id\n" +
            " inner join ServiceTypes_New t\n" +
            "    on c.serviceTypeName = t.serviceTypeName\n" +
            "  left JOIN ClientStaff cs1\n" +
            "    on cc.id = cs1.ClientId\n" +
            "   and cs1.StaffTypeId = '214'\n" +
            "  left join BTSUsers bs1\n" +
            "    on cs1.UserName = bs1.username\n" +
            "  LEFT JOIN ClientGroups cg\n" +
            "    on cc.clientGrpId = cg.id\n" +
            "  left join (SELECT cc.id,\n" +
            "                    case\n" +
            "                      when cs1.UserName is null then\n" +
            "                       cs2.UserName\n" +
            "                      when cs1.UserName is null and cs2.UserName is null then\n" +
            "                       cs3.UserName\n" +
            "                      else\n" +
            "                       cs1.UserName\n" +
            "                    end UserName\n" +
            "               FROM CreditClients cc\n" +
            "               left JOIN ClientStaff cs1\n" +
            "                 on cc.id = cs1.ClientId\n" +
            "                and cs1.StaffTypeId = '217'\n" +
            "               left JOIN ClientStaff cs2\n" +
            "                 on cc.id = cs2.ClientId\n" +
            "                and cs2.StaffTypeId = '220'\n" +
            "               left JOIN ClientStaff cs3\n" +
            "                 on cc.id = cs3.ClientId\n" +
            "                and cs3.StaffTypeId = '221') cs2\n" +
            "    on cc.id = cs2.id\n" +
            "  left join BTSUsers bs2\n" +
            "    on cs2.UserName = bs2.username\n" +
            "\n" +
            "  left join (select c.id,\n" +
            "                    ((case\n" +
            "                      when cm1.modifiedCalculationValue is null then\n" +
            "                       0\n" +
            "                      else\n" +
            "                       cm1.modifiedCalculationValue\n" +
            "                    end) * (case\n" +
            "                      when cm2.modifiedCalculationValue is null then\n" +
            "                       0\n" +
            "                      else\n" +
            "                       cm2.modifiedCalculationValue\n" +
            "                    end) / 100) + ((case\n" +
            "                      when cm1.modifiedCalculationValue is null then\n" +
            "                       0\n" +
            "                      else\n" +
            "                       cm1.modifiedCalculationValue\n" +
            "                    end) + (case\n" +
            "                      when cm2.modifiedCalculationValue is null then\n" +
            "                       0\n" +
            "                      else\n" +
            "                       cm2.modifiedCalculationValue\n" +
            "                    end)) FS\n" +
            "\n" +
            "               from creditclients c\n" +
            "               left join ClientPriceModifierAssociation cm1\n" +
            "                 on c.id = cm1.creditClientId\n" +
            "                and cm1.sortOrder = '0'\n" +
            "               left join ClientPriceModifierAssociation cm2\n" +
            "                 on c.id = cm2.creditClientId\n" +
            "                and cm2.sortOrder = '1') tem\n" +
            "    on cc.id = tem.id\n" +
            "\n" +
            " where year(bookingDate) = year(getDate())\n" +
            "   and month(bookingDate) = month(getDate())\n" +
            "   and consignerAccountNo not in ('0', '999')\n" +
            "   and isApproved = '1'\n" +
            "   " + clvar.Check_Condition + "\n" +
            "   " + clvar.Check_Condition1 + "\n" +
            " group by case\n" +
            "            when MONTH(c.bookingDate) < '7' then\n" +
            "             cast((datename(YY, c.bookingDate) - 1) as varchar) + '-' +\n" +
            "             RIGHT(YEAR(c.bookingDate), 2)\n" +
            "            else\n" +
            "             cast(datename(YY, c.bookingDate) as varchar) + '-' +\n" +
            "             RIGHT(YEAR(c.bookingDate) + 1, 2)\n" +
            "          end,\n" +
            "          z.name,\n" +
            "          b.sname,\n" +
            "          datename(MM, c.bookingDate) + '-' + datename(YY, c.bookingDate),\n" +
            "          cc.regDate,\n" +
            "          case\n" +
            "            when (DATEDIFF(MONTH, cc.createdon, getdate())) < '4' then\n" +
            "             'NEW'\n" +
            "            else\n" +
            "             'OLD'\n" +
            "          end,\n" +
            "          case\n" +
            "            when bs1.firstName = bs1.lastName then\n" +
            "             bs1.firstName\n" +
            "            when bs1.lastName is null then\n" +
            "             bs1.firstName\n" +
            "            else\n" +
            "             bs1.firstName + ' ' + bs1.lastName\n" +
            "          end,\n" +
            "\n" +
            "          case\n" +
            "            when bs2.firstName = bs2.lastName then\n" +
            "             bs2.firstName\n" +
            "            when bs2.lastName is null then\n" +
            "             bs2.firstName\n" +
            "            else\n" +
            "             bs2.firstName + ' ' + bs2.lastName\n" +
            "          end,\n" +
            "          cc.accountno,\n" +
            "          cc.name,\n" +
            "          cg.name,\n" +
            "          t.Products,\n" +
            "          cast(tem.FS as varchar) + '%'\n" +
            "\n" +
            "\n" +
            " ORDER BY 1, 2, 3, 4, 7, 5";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;
        }

        public DataSet Get_ExpressCenterShiftTime(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select * from ExpressCenterShiftTime";

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

        public DataSet Get_SingleUserEmailAddress(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select COUNT(*) usercount from ZNI_USER1 z where UPPER(U_NAME) = UPPER('" + clvar._UserName + "') ";

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

        #endregion

        #region BTSCODE

        // ------------------------------------------- BTS CODE Start -------------------------------------------

        // Price Modifier Start
        public DataSet Get_MasterPriceModifier(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select id,name, description, calculationValue, createdBy, CONVERT(VARCHAR(10), createdOn, 105) createdOn, \n" +
                                    "modifiedBy, CONVERT(VARCHAR(10), modifiedOn, 105) modifiedOn, \n" +
                                    "CASE WHEN calculationBase = '1' THEN 'FLAT' ELSE 'PERCENTAGE' END calculationBase, \n" +
                                    "CASE WHEN status = '1' THEN 'ACTIVE' ELSE 'INACTIVE' END status, \n" +
                                    "CASE WHEN chkBillingModifier = '1' THEN 'YES' ELSE 'NO' END chkBillingModifier \n" +
                                    "from PriceModifiers \n" + clvar._status;

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

        public void Insert_PriceModifier(Variable clvar)
        {
            try
            {
                string query = "insert into PriceModifiers \n" +
                                "  (name, description, calculationValue, calculationBase, status,chkBillingModifier, createdBy, createdOn)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ModifierName + "',\n" +
                                "   '" + clvar._Detail + "',\n" +
                                "   '" + clvar._CalculationValue + "',\n" +
                                "   '" + clvar._CalculationBase + "',\n" +
                                "   '" + clvar._status + "',\n" +
                                "   '" + clvar._BillingModifier + "', \n" +
                                "   '" + HttpContext.Current.Session["NAME"].ToString() + "', \n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_SinglePriceModifierRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select id,name, description, calculationValue, calculationBase, status, \n" +
                             "CASE WHEN chkBillingModifier = '1' THEN 'YES' ELSE 'NO' END chkBillingModifier, \n" +
                             "CASE WHEN status = '1' THEN 'ACTIVE' ELSE 'INACTIVE' END status \n" +
                             "from PriceModifiers WHERE ID = '" + clvar._Id + "' ";

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

        public void Update_PriceModifier(Variable clvar)
        {
            try
            {
                string query =
                "UPDATE PriceModifiers SET \n" +
                " name = '" + clvar._ModifierName + "', \n" +
                " description = '" + clvar._Detail + "', \n" +
                " calculationValue = '" + clvar._CalculationValue + "',  \n" +
                " calculationBase = '" + clvar._CalculationBase + "', \n" +
                " status = '" + clvar._status + "',  \n" +
                " chkBillingModifier =  '" + clvar._BillingModifier + "', \n" +
                " modifiedOn = GETDATE(),  \n" +
                " modifiedBy = '" + HttpContext.Current.Session["NAME"].ToString() + "' \n" +
                " where id = '" + clvar._Id + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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
        // Price Modifier End

        // Consignment Type Start

        public DataSet Get_MasterConsignmentType(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select ct.id,ct.name,ct.description, CONVERT(VARCHAR(10), ct.createdOn, 105) createdOn, ct.createdBy, ct.modifiedBy, \n" +
                                    "CONVERT(VARCHAR(10), ct.modifiedOn, 105) modifiedOn, \n" +
                                    "CASE WHEN status = '1' THEN 'ACTIVE' ELSE 'INACTIVE' END status \n" +
                                    "from ConsignmentType ct  \n";// +clvar._status;

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

        public DataSet Get_SingleConsignmentTypeRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select ct.id,ct.name,ct.description, CONVERT(VARCHAR(10), ct.createdOn, 105) createdOn, ct.createdBy, ct.modifiedBy, \n" +
                            "CONVERT(VARCHAR(10), ct.modifiedOn, 105) modifiedOn, \n" +
                            "CASE WHEN status = '1' THEN 'ACTIVE' ELSE 'INACTIVE' END status \n" +
                            "from ConsignmentType ct WHERE ID = '" + clvar._Id + "' ";

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

        public void Insert_ConsignmentType(Variable clvar)
        {
            try
            {
                string query = "insert into ConsignmentType  \n" +
                                "  (name,description, status, createdBy,createdOn )\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ModifierName + "',\n" +
                                "   '" + clvar._Detail + "',\n" +
                                "   '" + clvar._status + "',\n" +
                                "   '" + HttpContext.Current.Session["NAME"].ToString() + "', \n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public void Update_ConsignmentType(Variable clvar)
        {
            try
            {
                string query =
                "UPDATE ConsignmentType SET \n" +
                " name = '" + clvar._ModifierName + "', \n" +
                " description = '" + clvar._Detail + "', \n" +
                " status = '" + clvar._status + "',  \n" +
                " modifiedOn = GETDATE(),  \n" +
                " modifiedBy = '" + HttpContext.Current.Session["NAME"].ToString() + "' \n" +
                " where id = '" + clvar._Id + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        // Consignment Type End

        // Expression Categories Start
        public DataSet Get_MasterExpressionCategories(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select \n" +
                                    "ec.id, ec.code, ec.name, ec.description, ec.createdBy, CONVERT(VARCHAR(10), ec.createdOn, 105) createdOn, \n" +
                                    "ec.modifiedBy, CONVERT(VARCHAR(10), ec.modifiedOn, 105) modifiedOn\n" +
                                    "from ExpressionCategories ec";

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

        public void Insert_ExpressionCategories(Variable clvar)
        {
            try
            {
                string query = "insert into ExpressionCategories \n" +
                                "  (name, description, code, createdBy, createdOn)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ModifierName + "',\n" +
                                "   '" + clvar._Detail + "',\n" +
                                "   '" + clvar._Code + "', \n" +
                                "   '" + HttpContext.Current.Session["NAME"].ToString() + "', \n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_SingleExpressionCategoriesRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select id, code, name, description \n" +
                             "from ExpressionCategories WHERE ID = '" + clvar._Id + "' ";

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

        public void Update_ExpressionCategories(Variable clvar)
        {
            try
            {
                string query =
                "UPDATE ExpressionCategories SET \n" +
                " name = '" + clvar._ModifierName + "', \n" +
                " description = '" + clvar._Detail + "', \n" +
                " code =  '" + clvar._Code + "', \n" +
                " modifiedOn = GETDATE(),  \n" +
                " modifiedBy = '" + HttpContext.Current.Session["NAME"].ToString() + "' \n" +
                " where id = '" + clvar._Id + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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
        // Expression Categories End

        // Expression Products Start
        public DataSet Get_MasterExpressionProduct(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "select \n" +
                                    "ep.id, ep.code, ep.name productname, ec.name categoryname, ep.description, ep.weight, ep.unit, ep.rate, ep.GSTAmount, \n" +
                                    "ep.serviceCharges, ep.ItemAmount, ep.productCharges, ep.operationalCost, ep.specialIncentive, \n" +
                                    "ep.productCharges, ep.createdBy, ep.createdOn, ep.modifiedBy, ep.modifiedOn \n" +
                                    "from ExpressionProduct ep, ExpressionCategories ec \n" +
                                    "where ep.categoryId = ec.id";

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

        public void Insert_ExpressionProducts(Variable clvar)
        {
            try
            {
                string query = "insert into ExpressionProduct \n" +
                                "  (code, name, categoryid, description, weight, unit, rate, GSTAmount, serviceCharges, ItemAmount, productCost, operationalCost, specialIncentive,productCharges, createdBy, createdOn,status)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._Code + "',\n" +
                                "   '" + clvar._ModifierName + "',\n" +
                                "   '" + clvar._Category + "', \n" +
                                "   '" + clvar._Detail + "', \n" +
                                "   '', \n" +
                                "   '', \n" +
                                "   '" + clvar._Rate + "', \n" +
                                "   '" + clvar._GST + "', \n" +
                                "   '" + clvar._ServiceCharges + "', \n" +
                                "   '" + clvar._Cost + "', \n" +
                                "   '', \n" +
                                "   '" + clvar._ServiceCharges + "', \n" +
                                "   '', \n" +
                                "   '" + clvar._Cost + "', \n" +
                                "   '" + HttpContext.Current.Session["NAME"].ToString() + "', \n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar._status + "' \n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        public DataSet Get_SingleExpressionProductRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select code, name, categoryid, description, weight, unit, rate, GSTAmount, serviceCharges, ItemAmount, \n" +
                             "productCost, operationalCost, specialIncentive,productCharges, status \n" +
                             "from ExpressionProduct WHERE ID = '" + clvar._Id + "' ";

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

        public void Update_ExpressionProduct(Variable clvar)
        {
            try
            {
                string query =
                "UPDATE ExpressionProduct SET \n" +
                " code = '" + clvar._Code + "', \n" +
                " name = '" + clvar._ModifierName + "', \n" +
                " categoryid = '" + clvar._Category + "', \n" +
                " description = '" + clvar._Detail + "', \n" +
                " weight =  '', \n" +
                " unit =  '', \n" +
                " rate =  '" + clvar._Rate + "', \n" +
                " GSTAmount =  '" + clvar._GST + "', \n" +
                " serviceCharges =  '" + clvar._ServiceCharges + "', \n" +
                " ItemAmount =  '" + clvar._Cost + "', \n" +
                " productCost =  '', \n" +
                " operationalCost =  '" + clvar._ServiceCharges + "', \n" +
                " specialIncentive =  '', \n" +
                " productCharges =  '" + clvar._Cost + "', \n" +
                " status =  '" + clvar._status + "', \n" +
                " modifiedOn = GETDATE(),  \n" +
                " modifiedBy = '" + HttpContext.Current.Session["NAME"].ToString() + "' \n" +
                " where id = '" + clvar._Id + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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
        // Expression Products End

        public DataSet Get_ServiceType(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select s.serviceTypeName from ServiceTypes s where s.status = '1'";

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

        public DataSet Get_RidersName(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select firstName+''+lastName ridername, expressCenterId from Riders where status = '1' \n" +
                                "AND ridercode = '" + clvar._RiderCode + "' and branchId = '" + clvar._TownCode + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        public DataSet Get_ArrivalScan(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select consignmentNumber from ArrivalScan_Detail where consignmentNumber = '" + clvar._ConsignmentNo + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        public Int64 Insert_ArrivalScan(Variable clvar)
        {
            Int64 count = 0;
            try
            {
                string query = "insert into ArrivalScan \n" +
                                "  (branchcode, originExpressCentercode, ridercode, weight, CreatedOn, CreatedBy) output INSERTED.ID \n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                                "   '" + clvar.Expresscentercode + "',\n" +
                                "   '" + clvar._RiderCode + "',\n" +
                                "   '" + clvar._Weight + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                //   SqlDataAdapter oda = new SqlDataAdapter(orcd);
                count = (Int64)orcd.ExecuteScalar();

                //if (count > 0)
                //{
                //    orcd.CommandText = "SELECT MAX(ID) FROM ARRIVALSCAN";
                //    DataTable dt = new DataTable();
                //    SqlDataAdapter sda = new SqlDataAdapter(orcd.CommandText, orcl);
                //    sda.Fill(dt);
                //    count = Int64.Parse(dt.Rows[0][0].ToString());
                //}

                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return count;
        }
        public void Insert_ArrivalScanDetail(Variable clvar)
        {
            try
            {
                string query = "insert into ArrivalScan_Detail \n" +
                                "  (ArrivalID, ConsignmentNumber, CreatedOn, CreatedBy)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   " + clvar.ArrivalID + ",\n" +
                                "   '" + clvar.ConsignmentNo + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n" +
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

        public void BulkInsertToDataBase(DataTable dt)
        {
            Cl_Variables clvar = new Cl_Variables();
            DataTable dtProductSold = dt;// (DataTable)ViewState["ProductsSold"];

            using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
            {
                using (SqlCommand cmd = new SqlCommand("Bulk_ArrivalInsert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@tblCustomers", dt);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public void BulkInsertTrackingtoDatabase(DataTable dt)
        {
            Cl_Variables clvar = new Cl_Variables();
            DataTable dtProductSold = dt;// (DataTable)ViewState["ProductsSold"];

            using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
            {
                using (SqlCommand cmd = new SqlCommand("Bulk_ConsignmentsTrackingHistory"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@tblCustomers", dt);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public void BulkInsertConsignmenttoDatabase(DataTable dt)
        {
            Cl_Variables clvar = new Cl_Variables();
            DataTable dtProductSold = dt;// (DataTable)ViewState["ProductsSold"];

            using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
            {
                using (SqlCommand cmd = new SqlCommand("Bulk_Consignments"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@tblCustomers", dt);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public DataSet Get_ConsignmentNumber(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                // string query = "select c.serviceTypeName, c.riderCode, c.consignmentTypeId, c.weight from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";
                string query = "select c.serviceTypeName, c.riderCode, c.consignmentTypeId, c.weight, c.originExpressCenter, c.ConsignmentNumber  from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";

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
        public DataSet Get_ConsignmentNumberForLoad(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                // string query = "select c.serviceTypeName, c.riderCode, c.consignmentTypeId, c.weight from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";

                string sqlString = "select c.consignmentNumber,\n" +
                "       c.serviceTypeName,\n" +
                "       c.consignmentTypeId,\n" +
                "       c.orgin,\n" +
                "       c.destination,\n" +
                "       b.name,\n" +
                "       b2.name DestinationName, c.weight, c.pieces, ct.name CNTYPE\n" +
                "  from Consignment c\n" +
                " inner join Branches b\n" +
                "    on c.orgin = b.branchCode\n" +
                " inner join Branches b2\n" +
                "    on c.destination = b2.branchCode\n" +
                " inner join ConsignmentType ct\n" +
                "    on ct.id = c.consignmentTypeId\n" +
                " where c.consignmentNumber = '" + clvar._ConsignmentNo + "'";


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


        public void Insert_Consignment(Variable clvar)
        {
            try
            {
                string query = "insert into Consignment \n" +
                                "  (consignmentNumber, serviceTypeName, riderCode, consignmentTypeId, weight, orgin, destination, createdon, createdby, customerType, pieces, \n" +
                                "   creditClientId, weightUnit, discount, cod, totalAmount, zoneCode, branchCode, gst, consignerAccountNo, bookingDate, isApproved, \n" +
                                "   deliveryStatus, dayType, isPriceComputed, isNormalTariffApplied, receivedFromRider, originExpressCenter)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ConsignmentNo + "',\n" +
                                "   '" + clvar._Services + "',\n" +
                                "   '" + clvar._RiderCode + "',\n" +
                                "   '" + clvar._ConsignmentType + "',\n" +
                                "   '" + clvar._Weight + "',\n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                                "   '1',\n" +
                                "   '1',\n" +
                                "   '330140',\n" +
                                "   '1',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '" + HttpContext.Current.Session["zonecode"].ToString() + "',\n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                                "   '0',\n" +
                                "   '4D1',\n" +
                                "   GETDATE(),\n" +
                                "   '0',\n" +
                                "   '4',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '0',\n" +
                                "   '1', '" + clvar.Expresscentercode + "'\n" +
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
        public void Insert_TrackingFromBagManifest(Variable clvar)
        {
            try
            {


                string query = "insert into ConsignmentsTrackingHistory (consignmentNumber, stateID, currentLocation, manifestNumber, bagNumber, transactionTime, sealNo) \n" +
                                "select consignmentNumber, \n" +
                                " '" + clvar._StateId + "' StateID,\n" +
                                " '" + HttpContext.Current.Session["LocationName"].ToString() + "' CurrentLocation, \n" +
                                " manifestNumber, \n" +
                                " '" + clvar._BagNumber + "' BagNumber, \n" +
                                " GETDATE(), \n" +
                                " '" + clvar.Seal + "' SealNo \n" +
                                " from Mnp_ConsignmentManifest \n" +
                                " where manifestNumber = '" + clvar._Manifest + "'";

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
        public void Insert_ConsignmentTrackingHistory(Variable clvar)
        {
            try
            {


                string query = "insert into ConsignmentsTrackingHistory \n" +
                                "  (consignmentNumber, stateID, currentLocation, manifestNumber, bagNumber, loadingNumber, mawbNumber, runsheetNumber, riderName, transactionTime, sealNo, VanNo)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._ConsignmentNo + "',\n" +
                                "   '" + clvar._StateId + "',\n" +
                                "   '" + HttpContext.Current.Session["LocationName"].ToString() + "',\n" +
                                "   '" + clvar._Manifest + "',\n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + clvar._LoadingId + "',\n" +
                                "   '" + clvar._MawbNumber + "',\n" +
                                "   '" + clvar._RunsheetNumber + "',\n" +
                                "   '" + clvar.RiderCode + "',\n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar.Seal + "',\n" +
                                "   '" + clvar.VehicleNo + "'\n" +

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

        // Master Orign
        public DataSet Get_MasterOrign(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT b.branchCode, \n"
                               + "       b.name     BranchName \n"
                               + "FROM   Branches                          b \n"
                               + "where b.status ='1' \n"
                               + " and b.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n"
                               + "GROUP BY \n"
                               + "       b.branchCode, \n"
                               + "       b.name \n"
                               + "ORDER BY BranchName ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Master Destination
        public DataSet Get_MasterDestination(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT b.branchCode, \n"
                               + "       b.name     BranchName \n"
                               + "FROM   Branches                          b \n"
                               + "where b.[status] ='1' \n"
                               + "GROUP BY \n"
                               + "       b.branchCode, \n"
                               + "       b.name \n"
                               + "ORDER BY BranchName ASC";

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

        // Master Destination
        public DataSet Get_MasterDestinationbyRouteId(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "SELECT b.branchCode, \n"
                               + "       b.name     BranchName \n"
                               + "FROM   Branches                          b \n"
                               + "where b.[status] ='1' \n"
                               + "and b.branchCode = (select MR.DestBranchId from rvdbo.MovementRoute MR where MR.MovementRouteId = '" + clvar._Route + "')  \n"
                               + "GROUP BY \n"
                               + "       b.branchCode, \n"
                               + "       b.name \n"
                               + "ORDER BY BranchName ASC";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Get Single Manifest Record
        public DataSet Get_SingleConsignmentRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select c.consignmentNumber , c.orgin, c.destination, c.serviceTypeName, b1.name OrignName, b2.name DestinationName\n" +
                             "from Consignment c, Branches b1, Branches b2, ServiceTypes\n" +
                             "where \n" +
                             "c.orgin = b1.branchCode \n" +
                             "and c.destination = b2.branchCode\n" +
                             "and c.consignmentNumber = '" + clvar._ConsignmentNo + "' \n" +
                             "group by \n" +
                             " c.consignmentNumber , c.orgin, c.destination, c.serviceTypeName, b1.name, b2.name ";

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

        // Get Single Manifest Record
        public DataSet Get_SingleManifestRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                // string sql = "select * from Manifest m where m.manifestNumber = '" + clvar._Manifest + "' ";

                //string sql = "select m.manifestNumber, m.manifestType, b1.name OrignName, b2.name DestinationName, m.origin, m.destination \n" +
                //             "from Manifest m, Branches b1, Branches b2 \n" +
                //             "where m.origin = b1.branchCode \n" +
                //             "and m.destination = b2.branchCode \n" +
                //             "and m.manifestNumber = '" + clvar._Manifest + "' ";

                string sql = "select m.manifestNumber, m.manifestType, 0 IsDeManifested, b1.name OrignName, b2.name DestinationName, m.origin, m.destination \n" +
                           "from MNP_Manifest m, Branches b1, Branches b2  \n" +
                           "where m.origin = b1.branchCode \n" +
                           "and m.destination = b2.branchCode \n" +
                           "and m.manifestNumber = '" + clvar._Manifest + "' \n";
                // "and m.destination = '" + clvar._Designation + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Get Single Manifest Record By Manifest Number
        public DataSet Get_ManifestRecordByManifestId(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select * from BagManifest b where b.manifestNumber = '" + clvar._Manifest + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Insert Bag Record 
        public void Insert_Bag(Variable clvar)
        {
            try
            {
                string query = "insert into Bag \n" +
                                "  (b.bagNumber, b.createdBy, b.createdOn, b.totalWeight, b.origin, b.destination, b.branchCode, b.zoneCode, b.sealNo, date)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar.BagNumber + "',\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar._Weight + "',\n" +
                                "   '" + clvar._Orign + "',\n" +
                                "   '" + clvar._Destination + "',\n" +
                                "   '" + HttpContext.Current.Session["branchcode"].ToString() + "', \n" +
                                "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
                                "   '" + clvar._Seal + "', \n" +
                                "   '" + clvar._StartDate + "'\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        // Insert rvdbo Bag Record 
        public void Insert_RVDBOBag(Variable clvar)
        {
            try
            {
                string query = "insert into rvdbo.Bag \n" +
                                " (b.bagid, b.BagNo, b.IsLocal, b.BagDate, b.OriginBranchId, b.DestBranchId, b.ZoneId, b.TotalWeight, b.SealId, b.SealNo, b.IsUnBaged,b.TransectionType, b.CreatedOn, b.CreatedById)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '1',\n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar._Orign + "',\n" +
                                "   '" + clvar._Destination + "',\n" +
                                "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
                                "   '" + clvar._Weight + "',\n" +
                                "   '" + clvar._Seal + "', \n" +
                                "   '" + clvar._Seal + "', \n" +
                                "   '1', \n" +
                                "   '1', \n" +
                                "   '" + clvar._StartDate + "', \n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "' \n" +
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

        // Insert Bag Record into BagManifest Table
        public void Insert_BagManifest(Variable clvar)
        {
            try
            {
                string query = "insert into BagManifest \n" +
                                "  (b.bagNumber, b.manifestNumber, b.createdBy, b.createdOn)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar.BagNumber + "',\n" +
                                "   '" + clvar._Manifest + "',\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        // Get Single Bag Record
        public DataSet Get_SingleBagNumberRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select b.bagnumber, b.totalWeight, b.origin, b.destination, b.sealNo, b.date \n" +
                             "from Bag b \n" +
                             "where \n" +
                             "b.bagNumber = '" + clvar.BagNumber + "' ";

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

        // Get Single Bag Record
        public DataSet Get_SingleRVDBOBagNumberRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql_old = "select b.BagId, b.BagNo, b.TotalWeight, b.OriginBranchId, b.DestBranchId, b.SealNo from rvdbo.Bag b \n" +
                             "where b.bagNo = '" + clvar._BagNumber + "' ";

                string sql = "select b.BagId, b.BagNo, b.TotalWeight, b.OriginBranchId, b.DestBranchId, b.SealNo, b1.name OrignName, b2.name DestinationName\n" +
                             "from rvdbo.Bag b ,Branches b1, Branches b2  \n" +
                             "where \n" +
                             "b.OriginBranchId = b1.branchCode \n" +
                             "and b.DestBranchId = b2.branchCode \n" +
                             "and b.bagNo = '" + clvar._BagNumber + "'" + clvar._Designation;


                string sqlString = "select b.bagNumber   bagNo,\n" +
                "       b.TotalWeight,\n" +
                "       b.origin      OriginBranchId,\n" +
                "       b.destination DestBranchId,\n" +
                "       b.SealNo,\n" +
                "       b1.name       OrignName,\n" +
                "       b2.name       DestinationName\n" +
                "  from Bag b, Branches b1, Branches b2\n" +
                " where b.origin = b1.branchCode\n" +
                "   and b.destination = b2.branchCode\n" +
                "   and b.bagNumber = '" + clvar._BagNumber + "'";


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

        // Master Vehicle
        public DataSet Get_MasterVehicle(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select * from rvdbo.Vehicle v where v.IsActive = '1' order by 1";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Get Single Bag Record
        public DataSet Get_BagManifestRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select b.bagNumber, b.manifestNumber, m.manifestType, b1.name OrignName, b2.name DestinationName \n" +
                             "from BagManifest b , Manifest m , Branches b1, Branches b2\n" +
                             "where \n" +
                             "b.manifestNumber = m.manifestNumber\n" +
                             "and m.origin = b1.branchCode\n" +
                             "and m.destination = b2.branchCode\n" +
                             "and b.bagNumber = '" + clvar.BagNumber + "' ";

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

        public void Update_Bag(Variable clvar)
        {
            try
            {
                string query = "UPDATE Bag SET \n" +
                                " totalWeight = '" + clvar._Weight + "', \n" +
                                " sealNo = '" + clvar._Seal + "', \n" +
                                " modifiedBy = '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                " modifiedOn = GETDATE()  \n" +
                                " where bagNumber = '" + clvar.BagNumber + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        // Insert Bag Record into BagManifest Table
        public void Insert_BagStatus(Variable clvar)
        {
            try
            {
                string query = "insert into BagStatus \n" +
                                "  (bagNumber, sealNo, status, createdBy, createdOn)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar.BagNumber + "',\n" +
                                "   '" + clvar._Seal + "',\n" +
                                "   '" + clvar._status + "',\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                "   GETDATE()\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        // Master Destination
        public DataSet Get_MasterRoute(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select * from rvdbo.MovementRoute MR where \n" +
                             "MR.OriginBranchId = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
                             "and MR.ParentMovementRouteId is null and MR.IsActive = '1' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Master Route By Route ID
        public DataSet Get_RouteByRouteId(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql_old = "select * from rvdbo.MovementRoute MR where \n" +
                             "MR.OriginBranchId = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
                             "and MR.ParentMovementRouteId = '" + clvar._Route + "' and MR.IsActive = '1' ";

                string sql = "select * from rvdbo.MovementRoute mr where \n" +
                              "mr.OriginBranchId = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
                              "and mr.ParentMovementRouteId = '" + clvar._Route + "' \n" +
                              "OR mr.MovementRouteId = '" + clvar._Route + "' \n" +
                              "and MR.IsActive = '1' order by name ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Get Single Seal Number Record
        public DataSet Get_SingleSealNumberRecord(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select b.sealNo \n" +
                             "from Bag b \n" +
                             "where \n" +
                             "b.sealNo = '" + clvar._Seal + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Master Transport Type
        public DataSet Get_MasterTransportType(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select l.AttributeValue, l.AttributeDesc from rvdbo.Lookup l where l.AttributeGroup = 'TRANSPORT_TYPE' ";

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

        // Insert Loading Record into Loading Table
        public void Insert_Loading(Variable clvar)
        {
            try
            {
                string query = "insert into rvdbo.Loading \n" +
                                "  (LoadingDate, Description, VehicleId, CourierName, MovementRouteId, TransportTypeId, OriginBranchId, DestBranchId, ZoneId, IsUnLoaded, CreatedOn)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._StartDate + "',\n" +
                                "   '" + clvar._Description + "',\n" +
                                "   '" + clvar._VehicleId + "',\n" +
                                "   '" + clvar._CourierName + "',\n" +
                                "   '" + clvar._Route + "',\n" +
                                "   '" + clvar._TransportType + "',\n" +
                                "   '" + clvar._Orign + "',\n" +
                                "   '" + clvar._Destination + "',\n" +
                                "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
                                "   '1', \n" +
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

        // Insert Loading Record into Loading Table
        public void Insert_Loading1(Variable clvar)
        {
            try
            {
                string query = "insert into Loading \n" +
                                "  (l.id, l.date, l.description, l.transportationType, l.vehicleId, l.courierName, l.origin, l.destination, l.branchCode, l.zoneCode, l.createdBy, l.createdOn, l.routeId)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._LoadingId + "',\n" +
                                "   '" + clvar._StartDate + "',\n" +
                                "   '" + clvar._Description + "',\n" +
                                "   '" + clvar._TransportType + "',\n" +
                                "   '" + clvar._VehicleId + "',\n" +
                                "   '" + clvar._CourierName + "',\n" +
                                "   '" + clvar._Orign + "',\n" +
                                "   '" + clvar._Destination + "',\n" +
                                "   '" + HttpContext.Current.Session["branchcode"].ToString() + "', \n" +
                                "   '" + HttpContext.Current.Session["zonecode"].ToString() + "', \n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
                                "   GETDATE(),\n" +
                                "   '" + clvar._Route + "'\n" +
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

        // Insert LoadingBag Record into Loading Table
        public void Insert_LoadingConsignment(Variable clvar)
        {
            try
            {
                string query = "insert into LoadingConsignment \n" +
                                "  (loadingId, consignmentNumber, createdBy, createdOn)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._LoadingId + "',\n" +
                                "   '" + clvar._ConsignmentNo + "',\n" +
                                "   '" + HttpContext.Current.Session["U_ID"].ToString() + "', \n" +
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

        // Insert LoadingBag Record into Loading Table
        public void Insert_LoadingBag(Variable clvar)
        {
            try
            {
                string query = "insert into rvdbo.LoadingBag \n" +
                                "  (LoadingId, BagId)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._LoadingId + "',\n" +
                                "   '" + clvar.BagNumber + "'\n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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

        // Get Last Loading Id
        public DataSet Get_LastLoadingId(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select max(l.Loadingid) LoadingId from rvdbo.Loading l";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Master Loading
        public DataSet Get_MasterLoadingBag(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql_old = "select * from rvdbo.LoadingBag l where \n" +
                             "l.originbranchid = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
                             "and l.LoadingId = '" + clvar._LoadingId + "' ";


                string sql = "SELECT b1.name OrignName, b2.name DestinationName,* from rvdbo.LoadingBag lb , rvdbo.Bag b, Branches b1, Branches b2 \n" +
                             "where \n" +
                             "lb.BagId = b.BagId\n" +
                             "and b.OriginBranchId = b1.branchCode\n" +
                             "and b.DestBranchId = b2.branchCode\n" +
                             "and lb.LoadingId = '" + clvar._LoadingId + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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


        // Get User Id
        public DataSet Get_UserEmailAddress(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select convert(nvarchar(10), INACTIVE_DATE, 105) INACTIVE_DATE, convert(nvarchar(10), modify_date, 105) modify_date, * from ZNI_USER1 where UPPER(U_NAME) = UPPER('" + clvar._UserName + "') \n" + clvar._OldUserName;

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
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

        // Update User Password
        public void Update_PasswordChange(Variable clvar)
        {
            try
            {
                string query = "UPDATE ZNI_USER1 SET U_PASSWORD = '" + clvar._password + "', CHANGE_PASS_FLAG = '" + clvar._Version + "', \n" +
                                " Modify_Date = GETDATE(), \n " +
                                " ACTIVE_DATE = GETDATE(), \n " +
                                //" INACTIVE_DATE = '" + clvar._End_Date + "' \n " + 
                                " INACTIVE_DATE = '" + clvar._End_Date.ToString("yyyy-MM-dd") + "' \n " +
                                " WHERE U_ID = '" + clvar._UserId + "' ";




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


        // Get Search Arrival Scan
        public DataSet Get_SearchArrivalScan(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql_old = " select CONVERT(NVARCHAR, a.createdon, 105) createdon, a.* \n" +
                             " from arrivalscan a where a.ridercode = '" + clvar.RiderCode + "' AND a.createdon = '" + clvar._StartDate + "' \n" +
                             " order by Id desc ";

                string sql = " select \n" +
                             " a.Id, b.name +' ('+b.branchcode+')' branchcode, e.name+' ('+e.expressCenterCode+')' originexpresscentercode, a.RiderCode, a.Weight, CONVERT(NVARCHAR, a.createdon, 105) createdon, COUNT(ad.consignmentNumber) Count\n" +
                             " from arrivalscan a  \n" +
                             " inner join ArrivalScan_Detail ad\n" +
                             " on ad.ArrivalID = a.Id \n" +
                             " inner join Branches b\n" +
                             " on b.branchCode = a.BranchCode \n" +
                             " inner join ExpressCenters e\n" +
                             " on e.expressCenterCode = a.OriginExpressCenterCode\n" +
                             " where a.ridercode = '" + clvar.RiderCode + "' AND CAST(a.CreatedOn AS date) = '" + clvar._StartDate + "' \n" +
                             " group by \n" +
                             " a.Id, b.name +' ('+b.branchcode+')', e.name+' ('+e.expressCenterCode+')', a.RiderCode, a.Weight, a.CreatedOn\n" +
                             " order by Id desc ";

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
        public DataTable Get_RunSheetInvoice(Variable clvar)
        {


            string sqlString = "select\n" +
            "r.runsheetNumber, r.runsheetDate, r.routeCode, r.route, c.consignmentNumber, c.consignee, c.pieces, c.weight,\n" +
            "rc.deliveryDate, b.sname, b1.sname orign, rs.firstName + ' '+rs.lastName courier, rr.riderCode\n" +
            "from\n" +
            "Runsheet r , RunsheetConsignment rc, RiderRunsheet rr, Riders rs, Branches b, Branches b1, Consignment c\n" +
            "where\n" +
            "r.runsheetNumber = rc.runsheetNumber\n" +
            "and r.branchCode = b.branchCode\n" +
            "and rc.consignmentNumber = c.consignmentNumber\n" +
            "and rc.branchcode = r.branchCode\n" +
            "and c.orgin = b1.branchCode\n" +
            "and rr.runsheetNumber = r.runsheetNumber\n" +
            "and rr.riderCode = rs.riderCode\n" +
            "and rr.expIdTemp = rs.expressCenterId\n" +
            "and r.runsheetNumber = '" + clvar.ArrivalID + "' \n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
            "and rs.branchid = '" + HttpContext.Current.Session["branchcode"].ToString() + "'\n" +
            "--and rr.riderCode = (select riderCode from Riders r1 where r1.routeCode = r.routeCode and r1.riderCode = rs.riderCode)\n" +
            "and r.routeCode = '" + clvar.Route + "'\n" +
            "order by SortOrder asc";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.StrconLive());
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

        public DataTable Get_RunSheetInvoice_header(Variable clvar)
        {
            string sqlString = "select r.runsheetNumber, r.route, r.routeCode, b1.sname, rs.firstName + ' '+rs.lastName courier, rr.riderCode, convert(varchar(10),r.runsheetDate,105) runsheetDate \n" +
            "from RunsheetConsignment rc,Runsheet r,\n" +
            "(select r.*,e.bid from RiderRunsheet r,ExpressCenters e\n" +
            "where r.expIdTemp=e.expressCenterCode)rr\n" +
            ",Riders rs\n" +
            ", Branches b, Branches b1, Consignment c\n" +
            "where\n" +
            "rc.runsheetNumber=r.runsheetNumber\n" +
            "and rc.branchcode=r.branchCode\n" +
            "and rs.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "and rc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "and rr.runsheetNumber=rc.runsheetNumber\n" +
            "and rr.bid=rc.branchcode\n" +
            "and rr.riderCode=rs.riderCode\n" +
            "and rr.expIdTemp=rs.expressCenterId\n" +
            "and rc.runsheetNumber= '" + clvar.ArrivalID + "' \n" +
            "and r.branchCode = b.branchCode\n" +
            "and rc.consignmentNumber = c.consignmentNumber\n" +
            "and c.orgin = b1.branchCode\n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n";

            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(clvar.StrconLive());
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
        */

        public DataTable Get_RunSheetInvoice(Variable clvar)
        {
            /*
            string sqlString = "select\n" +
            "r.runsheetNumber, r.runsheetDate, r.routeCode, r.route, c.consignmentNumber, c.consignee, c.pieces, c.weight,\n" +
            "rc.deliveryDate, b.sname, b1.sname orign, rs.firstName + ' '+rs.lastName courier, rr.riderCode\n" +
            "from\n" +
            "Runsheet r , RunsheetConsignment rc, RiderRunsheet rr, Riders rs, Branches b, Branches b1, Consignment c\n" +
            "where\n" +
            "r.runsheetNumber = rc.runsheetNumber\n" +
            "and r.branchCode = b.branchCode\n" +
            "and rc.consignmentNumber = c.consignmentNumber\n" +
            "and rc.branchcode = r.branchCode\n" +
            "and c.orgin = b1.branchCode\n" +
            "and rr.runsheetNumber = r.runsheetNumber\n" +
            "and rr.riderCode = rs.riderCode\n" +
            "and rr.expIdTemp = rs.expressCenterId\n" +
            "and r.runsheetNumber = '" + clvar.ArrivalID + "' \n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
            "and rs.branchid = '" + HttpContext.Current.Session["branchcode"].ToString() + "'\n" +
            "--and rr.riderCode = (select riderCode from Riders r1 where r1.routeCode = r.routeCode and r1.riderCode = rs.riderCode)\n" +
            "and r.routeCode = '" + clvar.Route + "'\n" +
            "order by SortOrder asc";
            */

            string sqlString = "select c.consignmentNumber, c.pieces, c.weight, b.sname orign, c.consignee \n" +
            "from RunsheetConsignment rc, Consignment c, Branches b \n" +
            "where \n" +
            "rc.consignmentNumber = c.consignmentNumber\n" +
            "and rc.branchCode = b.branchCode\n" +
            "and rc.runsheetNumber = '" + clvar.ArrivalID + "' \n" +
            "and rc.createdBy='" + clvar.Depot + "'\n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
            "order by rc.SortOrder asc ";

            sqlString = "select c.consignmentNumber, c.pieces, c.weight, b.sname orign, c.consignee\n" +
           "  from Runsheet R\n" +
           "\n" +
           " INNER JOIN RunsheetConsignment rc\n" +
           "    ON R.runsheetNumber = rc.runsheetNumber\n" +
           "   AND R.branchCode = rc.branchcode\n" +
           "   AND R.createdBy = rc.createdBy\n" +
           "   AND R.routeCode = rc.RouteCode\n" +
           "\n" +
           " INNER JOIN Consignment c\n" +
           "    ON C.consignmentNumber = rc.consignmentNumber\n" +
           "\n" +
           " INNER JOIN Branches B\n" +
           "    ON B.branchCode = C.orgin\n" +
           "\n" +
           " where rc.runsheetNumber = '" + clvar.ArrivalID + "'\n" +
           "   AND R.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "'\n" +
           "   AND R.routeCode = '" + clvar.Route + "'\n" +
           " order by rc.SortOrder desc";

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

        public DataTable Get_RunSheetInvoice_header(Variable clvar)
        {
            string sqlString = "select r.runsheetNumber, r.route, r.routeCode, b.sname, rs.firstName + ' '+rs.lastName courier, rr.riderCode, convert(varchar(10),r.runsheetDate,105) runsheetDate \n" +
            "from RunsheetConsignment rc,Runsheet r,\n" +
            "(select r.*,e.bid from RiderRunsheet r,ExpressCenters e\n" +
            "where r.expIdTemp=e.expressCenterCode)rr\n" +
            ",Riders rs\n" +
            ", Branches b, Consignment c\n" +
            "where\n" +
            "rc.runsheetNumber=r.runsheetNumber\n" +
            "and rc.branchcode=r.branchCode\n" +
            "and rs.branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "and rc.branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'\n" +
            "and rr.runsheetNumber=rc.runsheetNumber\n" +
            "and rr.bid=rc.branchcode\n" +
            "and rr.riderCode=rs.riderCode\n" +
            "and rr.expIdTemp=rs.expressCenterId\n" +
            "and rc.runsheetNumber= '" + clvar.ArrivalID + "' \n" +
            "and r.branchCode = b.branchCode\n" +
            "and rc.consignmentNumber = c.consignmentNumber\n" +
            //  "and c.orgin = b1.branchCode\n" +
            "and r.branchCode = b.branchCode \n" +
            "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n";


            sqlString = "select r.runsheetNumber,\n" +
           "       r.route,\n" +
           "       r.routeCode,\n" +
           "       b.sname,\n" +
           "       rs.firstName + ' ' + rs.lastName courier,\n" +
           "       rr.riderCode,\n" +
           "       convert(varchar(10), r.runsheetDate, 105) runsheetDate\n" +
           "\n" +
           "  FROM RUNSHEET R\n" +
           "\n" +
           " INNER JOIN RiderRunsheet RR\n" +
           "    ON R.runsheetNumber = RR.runsheetNumber\n" +
           "   AND R.createdBy = RR.createdBy\n" +
           "\n" +
           " INNER JOIN Riders RS\n" +
           "    ON RS.branchId = R.branchCode\n" +
           "   AND RS.routeCode = R.routeCode\n" +
           "   AND RS.riderCode = RR.riderCode\n" +
           "   AND RS.expressCenterId = RR.expIdTemp\n" +
           "\n" +
           " INNER JOIN Branches B\n" +
           "    ON B.branchCode = R.branchCode\n" +
           "\n" +
           " WHERE R.runsheetNumber = '" + clvar.ArrivalID + "'\n" +
           "   AND R.routeCode = '" + clvar.Route + "'\n" +
           "   AND R.branchCode = '" + HttpContext.Current.Session["branchcode"].ToString() + "'";


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


        public DataSet Get_SearchRunSheet(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "";

                //   sql = "select convert(varchar(10),r.runsheetDate,105) runsheetDate, r.* from Runsheet r where r.runsheetDate = '" + clvar._StartDate + "' AND routecode = '"+clvar.Route+"'";
                /*
                sql = "select convert(varchar(10),r.runsheetDate,105) runsheetDate, count(rc.consignmentnumber) CNcount, r.routeCode, r.runsheetNumber,r.createdby from Runsheet r, RunsheetConsignment rc \n" +
                      "where \n" +
                      "r.runsheetNumber = rc.runsheetNumber\n" +
                      "and r.branchCode = rc.branchcode\n" +
                      "and r.runsheetDate = '" + clvar._StartDate + "' AND routecode = '" + clvar.Route + "' \n" +
                      "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
                      "group by \n" +
                      "convert(varchar(10),r.runsheetDate,105) , r.routeCode, r.runsheetNumber,r.createdby  ";
                */

                sql = "select convert(varchar(10),r.runsheetDate,105) runsheetDate, count(rc.consignmentnumber) CNcount, r.routeCode, r.runsheetNumber,r.createdby from Runsheet r, RunsheetConsignment rc \n" +
                     "where \n" +
                     "r.runsheetNumber = rc.runsheetNumber\n" +
                     "and r.branchCode = rc.branchcode\n" +
                     "and r.routecode = rc.routecode\n" +
                     "and r.runsheetDate = '" + clvar._StartDate + "' AND rc.routecode = '" + clvar.Route + "' \n" +
                     "and rc.branchcode = '" + HttpContext.Current.Session["branchcode"].ToString() + "' \n" +
                     "--and r.createdBy = '" + HttpContext.Current.Session["u_id"].ToString() + "' \n" +
                     "--and rc.createdBy = '" + HttpContext.Current.Session["u_id"].ToString() + "' \n" +
                     "group by \n" +
                     "convert(varchar(10),r.runsheetDate,105) , r.routeCode, r.runsheetNumber,r.createdby  ";

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

        public DataSet Get_BranchDetail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select * from Branches b where b.branchCode = '" + clvar._Id + "' ";

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

        public DataSet Get_BagConsignment(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                //  string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' AND c.destination = '" + clvar._Designation + "' ";
                string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";

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

        // Insert rvdbo Bag Record 
        public void BagConsignment(Variable clvar)
        {
            try
            {
                string query = "insert into BagOutpieceAssociation \n" +
                                " (bagNumber, outpieceNumber)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + clvar._ConsignmentNo + "' \n" +
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



        public DataSet Get_BagConsignment_(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                //  string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' AND c.destination = '" + clvar._Designation + "' ";
                string query = "select * from Consignment c where c.consignmentNumber = '" + clvar._ConsignmentNo + "' ";

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

        /*
            // Get User Id
            public DataSet Get_SearchRunSheet(Variable clvar)
            {
                DataSet ds = new DataSet();
                string temp = "";
                try
                {
                    string sql = "";

                    sql = "select convert(varchar(10),r.runsheetDate,105) runsheetDate, r.* from Runsheet r where r.runsheetDate = '" + clvar._StartDate + "' ";

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
        /*
        // Insert Bag Record into BagOutpieceAssociation Table
        public void Insert_BagOutpieceAssociation(Variable clvar)
        {
            try
            {
                string query = "insert into BagOutpieceAssociation \n" +
                                "  (bagNumber, outpieceNumber)\n" +
                                "values\n" +
                                "  ( \n" +
                                "   '" + clvar._BagNumber + "',\n" +
                                "   '" + clvar._ConsignmentNo + "' \n" +
                                " ) ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
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
        */
        public DataSet Get_ConsignmentTypeDetail(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string query = "select * from ConsignmentType c where c.status = '1' AND c.id = '" + clvar._Id + "' ";

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

        #region Last Login

        //public DataSet Get_UserLastLogin(Variable clvar)
        //{

        //    string query = "SELECT MAX(d_date) FROM (" +
        //                    "SELECT" +
        //                    "\t distinct MAX(to_date(to_char(r.hit_date,'dd/MM/yyyy'),'dd/MM/yyyy')) d_date\n" +
        //                    "FROM\n" +
        //                    "\tuser_rpt_hits_log r\n" +
        //                    "WHERE\n" +
        //                    "\tUPPER(r.u_id) = UPPER('" + clvar.UserName + "')\n" +
        //                    "\tAND r.hit_date < (\n" +
        //                    "\t    \tSELECT\n" +
        //                    "\t    \t\tMAX(r2.hit_date)\n" +
        //                    "\t    \tFROM\n" +
        //                    "\t    \t\tuser_rpt_hits_log r2\n" +
        //                    "\t    \tWHERE\n" +
        //                    "\t    \t\tUPPER(r2.u_id) = UPPER('" + clvar.UserName + "')\n" +
        //                    "\t    )\n" +
        //                    "GROUP BY\n" +
        //                    "\tr.hit_date\n" +
        //                    ")";


        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        OracleConnection orcl = new OracleConnection(clvar.Strcon());
        //        orcl.Open();
        //        OracleCommand orcd = new OracleCommand(query, orcl);
        //        orcd.CommandType = CommandType.Text;
        //        OracleDataAdapter oda = new OracleDataAdapter(orcd);
        //        oda.Fill(ds);
        //        orcl.Close();
        //    }
        //    catch (Exception Err)
        //    { }
        //    finally
        //    { }
        //    return ds;
        //}

        #endregion

        public DataSet Get_SearchBags(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = " select b.bagNumber, CONVERT(NVARCHAR, b.createdon, 105) createdon, b.totalWeight, b.sealNo, \n" +
                             " b1.name orign, b2.name destination --, COUNT(bm.manifestNumber) manifestCount, COUNT(bo.outpieceNumber) CNCount\n" +
                             " from Bag b\n" +
                             //" inner join BagManifest bm\n" +
                             //" on bm.bagNumber = b.bagNumber\n" +
                             // " inner join BagOutpieceAssociation bo\n" +
                             // " on bo.bagNumber = b.bagNumber\n" +
                             " inner join Branches b1\n" +
                             " on b1.branchCode = b.origin\n" +
                             " inner join Branches b2\n" +
                             " on b2.branchCode = b.destination\n" +
                             " where \n" +
                             " cast(b.createdOn as date) = '" + clvar._StartDate + "' \n" +
                             " and b.bagNumber = '" + clvar.BagNumber + "' \n" +
                             " group by\n" +
                             " b.bagNumber, b.createdOn, b.totalWeight, b.sealNo, b.origin, b.destination, b1.name, b2.name";

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
                                    //  "and CAST(getdate() as date)=c.WorkingDate \n" +
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


        public string BulkInsertTo_ArrivalDetail(DataTable dt, DataTable dt1, DataTable dt2)
        {
            Cl_Variables clvar = new Cl_Variables();
            DataTable dtProductSold = dt;// (DataTable)ViewState["ProductsSold"];
            string Abc = "";
            using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
            {
                using (SqlCommand cmd = new SqlCommand("bulk_Arrival_Insert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@tblCustomers", dt);
                    cmd.Parameters.AddWithValue("@tblCustomers1", dt1);
                    cmd.Parameters.AddWithValue("@tblCustomers2", dt2);
                    cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    Abc = cmd.Parameters["@result"].Value.ToString();

                    con.Close();
                }
            }
            return Abc;
        }




        // ------------------------------------------- BTS CODE End -------------------------------------------



        #endregion
    }
}