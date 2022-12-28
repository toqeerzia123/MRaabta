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
    public class MISReport
    {
        public MISReport()
        {

        }

        #region BTSCODE

        // Monthly Packet Movement Report
        public DataSet Get_SummaryMonthlyPacketMovement(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT COUNT(c.consignmentNumber) Packets,\n" +
                                    "       ROUND(sum(c.weight), 2) Total_Weight,\n" +
                                    "       ROUND(SUM(c.totalAmount), 2) TotalAmount\n" +
                                    "  FROM Consignment AS c \n" +
                                    "         WHERE \n" +
                                    clvar._Year + "\n" + clvar._Month + "\n" + clvar._TownCode + "\n";


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

        public DataSet Get_MonthlyPacketMovementReport(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {

                string sqlString = "SELECT z.name        Origin_Zone,\n" +
                "       b.name                           Origin_Branch,\n" +
                "       z2.name                          Destination_Zone,\n" +
                "       b2.name                          Destination_Branch,\n" +
                "       c.[Month],\n" +
                "       c.serviceTypeName                SERVICE,\n" +
                "       c.[Cash/Credit],\n" +
                "       c.[weight],\n" +
                "       COUNT(c.consignmentNumber)       Packets,\n" +
                "       ROUND(SUM(c.totalAmount), 2)     TotalAmount,\n" +
                "       SUM(c.[weight])                  Total_Weight\n" +
                "FROM   (\n" +
                "           SELECT  c.orgin,\n" +
                "                  c.destination,\n" +
                "                  DATENAME(MM, c.bookingDate) + '-' + DATENAME(YY, c.bookingDate)\n" +
                "                  MONTH,\n" +
                "                  c.serviceTypeName,\n" +
                "                  CASE\n" +
                "                       WHEN cc.accountNo = '0' THEN 'Cash'\n" +
                "                       ELSE       'Credit'\n" +
                "                  END 'Cash/Credit',\n" +
                "                  c.[weight],\n" +
                "                  c.consignmentNumber,\n" +
                "                  c.totalAmount\n" +
                "           FROM   Consignment  AS c\n" +
                "                  INNER JOIN CreditClients AS cc\n" +
                "                       ON  cc.id = c.creditClientId\n" +
                "           WHERE\n" +

                clvar._Year + "\n" + clvar._Month + "\n" + clvar._TownCode + "\n" +

                "       ) c\n" +
                "       INNER JOIN Branches           AS b\n" +
                "            ON  b.branchCode = c.orgin\n" +
                "       INNER JOIN Zones              AS z\n" +
                "            ON  z.zoneCode = b.zoneCode\n" +
                "       INNER JOIN Branches           AS b2\n" +
                "            ON  b2.branchCode = c.destination\n" +
                "       INNER JOIN Zones              AS z2\n" +
                "            ON  z2.zoneCode = b2.zoneCode\n" +
                "GROUP BY\n" +
                "       z.name,b.name,z2.name,b2.name,c.[MONTH],c.serviceTypeName,c.[Cash/Credit],c.[weight]\n" +
                "ORDER BY\n" +
                "       1,2,3,4,6,7,8";


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

        public DataSet cust_wise_sales_report(Variable clvar)
        {
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();

            string sqlString = "Select c.Region,\n" +
            "       c.FiscalYear,\n" +
            "       Zone,\n" +
            "       Branch,\n" +
            "       c.Period,\n" +
            //  "       c.regDate,\n" +
            "       CASE  \n" +
            "         WHEN c.regDate = '01-01-1979' THEN '' \n" +
            "         ELSE CONVERT(NVARCHAR, c.regDate, 105)  \n" +
            "       END as regDate ,\n" +
            "       Sales_Person,\n" +
            "       staff,\n" +
            "       c.Cus_No,\n" +
            "       c.Cus_Name,\n" +
            "       c.Customer_Group,\n" +
            "       c.Products,\n" +
            "       sum(c.WT) WT,\n" +
            "       SUM(c.SHIP) SHIP,\n" +
            "       SUM(c.Revenue) Revenue,\n" +
            "       sum(c. [ Fuel% ]) Fuel,\n" +
            "       SUM(c.LastyearRevenue) LastyearRevenue,\n" +
            "       SUM(c.LastyearFuel) LastyearFuel\n" +
            "\n" +
            "  from (SELECT z.Region,\n" +
            "               c.FiscalYear,\n" +
            "               z.name Zone,\n" +
            "               b.name Branch,\n" +
            "               c.Period,\n" +
            "               c.regDate,\n" +
            "               c.sfirstName + ' ' + c.slastName Sales_Person,\n" +
            "               c.mfirstName + ' ' + c.mlastName staff,\n" +
            "               c.Cus_No,\n" +
            "               c.Cus_Name,\n" +
            "               c.Customer_Group,\n" +
            "               c.Products,\n" +
            "               SUM(c.WT) WT,\n" +
            "               COUNT(c.SHIP) SHIP,\n" +
            "               SUM(c.Revenue) Revenue,\n" +
            "               sum(c.Fuel) [ Fuel% ],\n" +
            "               0 LastyearRevenue,\n" +
            "               0 LastyearFuel\n" +
            "          FROM (SELECT CASE\n" +
            "                         WHEN MONTH(c.bookingDate) < '7' THEN\n" +
            "                          CAST((DATENAME(YY, c.bookingDate) - 1) AS VARCHAR) + '-' +\n" +
            "                          RIGHT(YEAR(c.bookingDate), 2)\n" +
            "                         ELSE\n" +
            "                          CAST(DATENAME(YY, c.bookingDate) AS VARCHAR) + '-' +\n" +
            "                          RIGHT(YEAR(c.bookingDate) + 1, 2)\n" +
            "                       END FiscalYear,\n" +
            "                       c.orgin,\n" +
            "                       DATENAME(MM, c.bookingDate) + '-' +\n" +
            "                       DATENAME(YY, c.bookingDate) Period,\n" +
            "                       cc.regDate,\n" +
            "                       bs1.firstName sfirstName,\n" +
            "                       bs1.lastName slastName,\n" +
            "                       bs2.firstName mfirstName,\n" +
            "                       bs2.lastName mlastName,\n" +
            "                       cc.accountNo Cus_No,\n" +
            "                       cc.name Cus_Name,\n" +
            "                       cg.name Customer_Group,\n" +
            "                       stn.Products,\n" +
            "                       CASE\n" +
            "                         WHEN c.serviceTypeName = 'domestics' AND c.weight < 5 THEN\n" +
            "                          '5'\n" +
            "                         ELSE\n" +
            "                          c.weight\n" +
            "                       END WT,\n" +
            "                       c.consignmentNumber SHIP,\n" +
            "                       c.totalAmount Revenue,\n" +
            "                       ISNULL(cpma.modifiedCalculationValue, 0) AS Fuel\n" +
            "\n" +
            "                  FROM Consignment AS c\n" +
            "                 INNER JOIN ServiceTypes_New AS stn\n" +
            "                    ON stn.serviceTypeName = c.serviceTypeName\n" +
            "                 INNER JOIN CreditClients AS cc\n" +
            "                    ON cc.id = c.creditClientId\n" +
            "                  LEFT JOIN ClientStaff AS cs1\n" +
            "                    ON cs1.ClientId = cc.id\n" +
            "                   AND cs1.StaffTypeId = '214'\n" +
            "                  LEFT JOIN BTSUsers bs1\n" +
            "                    ON cs1.UserName = bs1.username\n" +
            "                  LEFT JOIN ClientStaff AS cs2\n" +
            "                    ON cs2.ClientId = cc.id\n" +
            "                   AND cs2.StaffTypeId = '217'\n" +
            "                  LEFT JOIN BTSUsers bs2\n" +
            "                    ON cs1.UserName = bs2.username\n" +
            "                  LEFT JOIN ClientPriceModifierAssociation AS cpma\n" +
            "                    ON cpma.creditClientId = cc.id\n" +
            "                  LEFT JOIN ClientGroups AS cg\n" +
            "                    ON cg.id = cc.clientGrpId\n" +
            "\n" +
            "                 WHERE " + clvar.Year + "\n";
            if (clvar.Month != "")
            {
                sqlString += clvar.Month;
            }

            if (clvar.Services != "")
            {
                sqlString += clvar.Services;

            }


            sqlString += " and c.isPriceComputed = '1' AND c.creditClientId NOT IN\n" +
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
            "   '344009', '342768', '342987')  AND c.isApproved = '1'\n" +
            "                -- AND cc.accountNo NOT IN ('0', '4D1')\n" +
            "                ) c\n" +
            "         INNER JOIN Branches AS b\n" +
            "            ON b.branchCode = c.orgin\n" +
            "         INNER JOIN Zones AS z\n" +
            "            ON z.zoneCode = b.zoneCode\n" +
            "         where \n" + clvar.Zone +
            //"               z.zoneCode IN ('11', '7', '1')\n" +


            "\n" +
            "         GROUP BY z.Region,\n" +
            "                  c.FiscalYear,\n" +
            "                  z.name,\n" +
            "                  b.name,\n" +
            "                  c.Period,\n" +
            "                  c.regDate,\n" +
            "                  c.sfirstName + ' ' + c.slastName,\n" +
            "                  c.mfirstName + ' ' + c.mlastName,\n" +
            "                  c.Cus_No,\n" +
            "                  c.Cus_Name,\n" +
            "                  c.Customer_Group,\n" +
            "                  c.Products,\n" +
            "                  CAST(c.Fuel AS VARCHAR)\n" +
            "        union all\n" +
            "\n" +
            "        SELECT z.Region,\n" +
            "               c.FiscalYear,\n" +
            "               z.name Zone,\n" +
            "               b.name [ Station / Branch ],\n" +
            "               c.Period,\n" +
            "               c.regDate,\n" +
            "               c.sfirstName + ' ' + c.slastName Sales_Person,\n" +
            "               c.mfirstName + ' ' + c.mlastName [ ABH / ZM / CSH ],\n" +
            "               c.Cus_No,\n" +
            "               c.Cus_Name,\n" +
            "               c.Customer_Group,\n" +
            "               c.Products,\n" +
            "               SUM(c.WT) WT,\n" +
            "               COUNT(c.SHIP) SHIP,\n" +
            "               0,\n" +
            "               0,\n" +
            "               SUM(c.Revenue) Revenue,\n" +
            "               sum(c.Fuel) [ Fuel% ]\n" +
            "          FROM (SELECT CASE\n" +
            "                         WHEN MONTH(c.bookingDate) < '7' THEN\n" +
            "                          CAST((DATENAME(YY, c.bookingDate) - 1) AS VARCHAR) + '-' +\n" +
            "                          RIGHT(YEAR(c.bookingDate), 2)\n" +
            "                         ELSE\n" +
            "                          CAST(DATENAME(YY, c.bookingDate) AS VARCHAR) + '-' +\n" +
            "                          RIGHT(YEAR(c.bookingDate) + 1, 2)\n" +
            "                       END FiscalYear,\n" +
            "                       c.orgin,\n" +
            "                       DATENAME(MM, c.bookingDate) + '-' +\n" +
            "                       DATENAME(YY, c.bookingDate) Period,\n" +
            "                       cc.regDate,\n" +
            "                       bs1.firstName sfirstName,\n" +
            "                       bs1.lastName slastName,\n" +
            "                       bs2.firstName mfirstName,\n" +
            "                       bs2.lastName mlastName,\n" +
            "                       cc.accountNo Cus_No,\n" +
            "                       cc.name Cus_Name,\n" +
            "                       cg.name Customer_Group,\n" +
            "                       stn.Products,\n" +
            "                       CASE\n" +
            "                         WHEN c.serviceTypeName = 'domestics' AND c.weight < 5 THEN\n" +
            "                          '5'\n" +
            "                         ELSE\n" +
            "                          c.weight\n" +
            "                       END WT,\n" +
            "                       c.consignmentNumber SHIP,\n" +
            "                       c.totalAmount Revenue,\n" +
            "                       ISNULL(cpma.modifiedCalculationValue, 0) AS Fuel\n" +
            "\n" +
            "                  FROM Consignment_2015 AS c\n" +
            "                 INNER JOIN ServiceTypes_New AS stn\n" +
            "                    ON stn.serviceTypeName = c.serviceTypeName\n" +
            "                 INNER JOIN CreditClients AS cc\n" +
            "                    ON cc.id = c.creditClientId\n" +
            "                  LEFT JOIN ClientStaff AS cs1\n" +
            "                    ON cs1.ClientId = cc.id\n" +
            "                   AND cs1.StaffTypeId = '214'\n" +
            "                  LEFT JOIN BTSUsers bs1\n" +
            "                    ON cs1.UserName = bs1.username\n" +
            "                  LEFT JOIN ClientStaff AS cs2\n" +
            "                    ON cs2.ClientId = cc.id\n" +
            "                   AND cs2.StaffTypeId = '217'\n" +
            "                  LEFT JOIN BTSUsers bs2\n" +
            "                    ON cs1.UserName = bs2.username\n" +
            "                  LEFT JOIN ClientPriceModifierAssociation AS cpma\n" +
            "                    ON cpma.creditClientId = cc.id\n" +
            "                  LEFT JOIN ClientGroups AS cg\n" +
            "                    ON cg.id = cc.clientGrpId\n" +
            "\n" +
            // "                 WHERE YEAR(c.bookingDate) = '" + clvar.DayMon + "'\n";
            "                 WHERE " + clvar.Year + "\n";
            if (clvar.Month != "")
            {
                sqlString += clvar.Month;
            }

            if (clvar.Services != "")
            {
                sqlString += clvar.Services;

            }


            sqlString += "    AND c.isApproved = '1'\n" +
            "                -- AND cc.accountNo NOT IN ('0', '4D1')\n" +
            "                ) c\n" +
            "         INNER JOIN Branches AS b\n" +
            "            ON b.branchCode = c.orgin\n" +
            "         INNER JOIN Zones AS z\n" +
            "            ON z.zoneCode = b.zoneCode\n" +
            "         where \n" +
            // "         z.zoneCode IN ('11', '7', '1')\n" + 
            clvar.Zone +
            "\n" +
            "         GROUP BY z.Region,\n" +
            "                  c.FiscalYear,\n" +
            "                  z.name,\n" +
            "                  b.name,\n" +
            "                  c.Period,\n" +
            "                  c.regDate,\n" +
            "                  c.sfirstName + ' ' + c.slastName,\n" +
            "                  c.mfirstName + ' ' + c.mlastName,\n" +
            "                  c.Cus_No,\n" +
            "                  c.Cus_Name,\n" +
            "                  c.Customer_Group,\n" +
            "                  c.Products,\n" +
            "                  CAST(c.Fuel AS VARCHAR) + '%') c\n" +
            "\n" +
            " group by c.Region,\n" +
            "          c.FiscalYear,\n" +
            "          Zone,\n" +
            "          Branch,\n" +
            "          c.Period,\n" +
            "          c.regDate,\n" +
            "          Sales_Person,\n" +
            "          staff,\n" +
            "          c.Cus_No,\n" +
            "          c.Cus_Name,\n" +
            "          c.Customer_Group,\n" +
            "          c.Products";


            SqlCommand cmd = new SqlCommand(sqlString, orcl);
            cmd.CommandTimeout = 500;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            orcl.Close();
            return ds;


        }

        public DataSet Get_serviceType(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {

                string sqlString = "select distinct name from ServiceTypes_New\n" +
                "order by name";


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

        /*
        public DataSet cash_vs_credit_report(Variable clvar)
        {
            //SqlConnection conn = new SqlConnection("Data Source=192.168.200.36;Integrated Security=true;Initial Catalog=APL_BTS;uid=sa; pwd=ap_ocs123");
            //conn.Open();
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();

            string sqlString = "Select t.Zone, t.segment,sum(t.TotalSale) total_sale, sum(international) international,\n" +
            "round((sum(international)/SUM(TotalSale)) * 100,2) international_per  ,\n" +
            "sum(Domestics) domestics,\n" +
            "round((sum(Domestics)/SUM(TotalSale)) * 100,2) domestics_per\n" +
            "  ,\n" +
            "sum(Expressions) expressions,\n" +
            "round((sum(Expressions)/SUM(TotalSale)) * 100,2) expressions_per\n" +
            " ,\n" +
            "sum(RnR) RnR,\n" +
            "round((sum(RnR)/SUM(TotalSale)) * 100,2) RnR_per\n" +
            " ,\n" +
            "sum(smart_cargo) smart_cargo,\n" +
            "round((sum(smart_cargo)/SUM(TotalSale)) * 100,2) smart_cargo_per\n" +
            " ,\n" +
            "sum(hand_carry) hand_carry,\n" +
            "round((sum(hand_carry)/SUM(TotalSale)) * 100,2) hand_carry_per\n" +
            " ,\n" +
            "sum(logex) logex,\n" +
            "round((sum(logex)/SUM(TotalSale)) * 100,2) logex_per\n" +
            ",\n" +
            "sum(others) others,\n" +
            "round((sum(others)/SUM(TotalSale)) * 100,2) others_per\n" +
            " from (\n" +
            "Select z.Zone, Z.segment,\n" +
            "case when Products='International' then SUM(Sale) else 0 end International ,\n" +
            "case when Products='Domestics' then SUM(Sale) else 0 end Domestics ,\n" +
            "case when Products='Expressions' then SUM(Sale) else 0 end Expressions ,\n" +
            "case when Products='Road N Rail' then SUM(Sale) else 0 end RnR ,\n" +
            "case when Products='Smart Cargo' then SUM(Sale) else 0 end smart_cargo ,\n" +
            "case when Products='Hand Carry' then SUM(Sale) else 0 end hand_carry ,\n" +
            "case when Products='Logex' then SUM(Sale) else 0 end logex ,\n" +
            " case when Products='Aviation Sale' then SUM(Sale) else 0 end others ,\n" +
            "SUM(Sale) TotalSale  from (\n" +
            "Select St.Products,\n" +
            "       case\n" +
            "         when c.consignerAccountNo = '0' then\n" +
            "          'Cash'\n" +
            "         else\n" +
            "          'Credit'\n" +
            "       end segment,\n" +
            "\n" +
            "       Z.name Zone,\n" +
            "\n" +
            "       SUM(c.totalAmount) Sale\n" +
            "  from Consignment c,\n" +
            "\n" +
            "       Branches         b,\n" +
            "       Zones            Z,\n" +
            "       ServiceTypes_New St\n" +
            "\n" +
            " where b.branchCode = c.orgin\n" +
            "   and z.zoneCode = b.zoneCode\n" +
            "   and st.serviceTypeName = c.serviceTypeName\n" +
            "\n" +
            "   and c.isApproved = '1'\n" +
            " and c.isPriceComputed = '1' \n" +
            " AND c.creditClientId NOT IN\n" +
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
            "   '344009', '342768', '342987') \n";
            if (clvar.Year != "")
            {
                sqlString += clvar.Year;
            }
            if (clvar.Month != "")
            {
                sqlString += clvar.Month;
            }
            sqlString += "   --AND ST.IsIntl != '1'\n" +
               "\n" +
               " group by c.consignerAccountNo, St.Products, Z.name\n" +
               " )\n" +
               " Z\n" +
               " group by\n" +
               " z.Zone, Z.segment,Z.Products)t\n" +
               "  group by\n" +
               "  t.Zone, t.segment\n" +
               "  order by t.Zone,t.segment\n" +
               "";

            SqlCommand cmd = new SqlCommand(sqlString, orcl);
            cmd.CommandTimeout = 500;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            orcl.Close();
            return ds;


        }
        */

        public DataSet cash_vs_credit_report(Variable clvar)
        {
            //SqlConnection conn = new SqlConnection("Data Source=192.168.200.36;Integrated Security=true;Initial Catalog=APL_BTS;uid=sa; pwd=ap_ocs123");
            //conn.Open();
            SqlConnection orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();
            /*
            string sqlString = "Select t.Zone, t.segment,sum(t.TotalSale) total_sale, sum(international) international,\n" +
            "round((sum(international)/SUM(TotalSale)) * 100,2) international_per  ,\n" +
            "sum(Domestics) domestics,\n" +
            "round((sum(Domestics)/SUM(TotalSale)) * 100,2) domestics_per\n" +
            "  ,\n" +
            "sum(Expressions) expressions,\n" +
            "round((sum(Expressions)/SUM(TotalSale)) * 100,2) expressions_per\n" +
            " ,\n" +
            "sum(RnR) RnR,\n" +
            "round((sum(RnR)/SUM(TotalSale)) * 100,2) RnR_per\n" +
            " ,\n" +
            "sum(smart_cargo) smart_cargo,\n" +
            "round((sum(smart_cargo)/SUM(TotalSale)) * 100,2) smart_cargo_per\n" +
            " ,\n" +
            "sum(hand_carry) hand_carry,\n" +
            "round((sum(hand_carry)/SUM(TotalSale)) * 100,2) hand_carry_per\n" +
            " ,\n" +
            "sum(logex) logex,\n" +
            "round((sum(logex)/SUM(TotalSale)) * 100,2) logex_per\n" +
            ",\n" +
            "sum(others) others,\n" +
            "round((sum(others)/SUM(TotalSale)) * 100,2) others_per\n" +
            " from (\n" +
            "Select z.Zone, Z.segment,\n" +
            "case when Products='International' then SUM(Sale) else 0 end International ,\n" +
            "case when Products='Domestics' then SUM(Sale) else 0 end Domestics ,\n" +
            "case when Products='Expressions' then SUM(Sale) else 0 end Expressions ,\n" +
            "case when Products='Road N Rail' then SUM(Sale) else 0 end RnR ,\n" +
            "case when Products='Smart Cargo' then SUM(Sale) else 0 end smart_cargo ,\n" +
            "case when Products='Hand Carry' then SUM(Sale) else 0 end hand_carry ,\n" +
            "case when Products='Logex' then SUM(Sale) else 0 end logex ,\n" +
            " case when Products='Aviation Sale' then SUM(Sale) else 0 end others ,\n" +
            "SUM(Sale) TotalSale  from (\n" +
            "Select St.Products,\n" +
            "       case\n" +
            "         when c.consignerAccountNo = '0' then\n" +
            "          'Cash'\n" +
            "         else\n" +
            "          'Credit'\n" +
            "       end segment,\n" +
            "\n" +
            "       Z.name Zone,\n" +
            "\n" +
            "       SUM(c.totalAmount) Sale\n" +
            "  from Consignment c,\n" +
            "\n" +
            "       Branches         b,\n" +
            "       Zones            Z,\n" +
            "       ServiceTypes_New St\n" +
            "\n" +
            " where b.branchCode = c.orgin\n" +
            "   and z.zoneCode = b.zoneCode\n" +
            "   and st.serviceTypeName = c.serviceTypeName\n" +
            "\n" +
            "   and c.isApproved = '1'\n" +
            "   and c.isPriceComputed = '1'\n" +
            " AND c.creditClientId NOT IN\n" +
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
            "   '344009', '342768', '342987') \n";
            if (clvar.Year != "")
            {
                sqlString += clvar.Year;
            }
            if (clvar.Month != "")
            {
                sqlString += clvar.Month;
            }
            sqlString += "   --AND ST.IsIntl != '1'\n" +
               "\n" +
               " group by c.consignerAccountNo, St.Products, Z.name\n" +
               " )\n" +
               " Z\n" +
               " group by\n" +
               " z.Zone, Z.segment,Z.Products)t\n" +
               "  group by\n" +
               "  t.Zone, t.segment\n" +
               "  order by t.Zone,t.segment\n" +
               "";
            */



            string sqlString = "SELECT b.Zone,b.Type,\n" +
            "sum(CASE WHEN b.Product='Domestic' THEN NetAmount ELSE 0 END) Domestic,\n" +
            "sum(CASE WHEN b.Product='International' THEN NetAmount ELSE 0 END) International,\n" +
            "sum(CASE WHEN b.Product='Road n Rail' THEN NetAmount ELSE 0 END) Road_n_Rail,\n" +
            "sum(CASE WHEN b.Product='Expressions' THEN NetAmount ELSE 0 END) Expressions,\n" +
            "sum(CASE WHEN b.Product='Aviation Sale' THEN NetAmount ELSE 0 END) Aviation_Sale,\n" +
            "sum(CASE WHEN b.Product='Smart Cargo' THEN NetAmount ELSE 0 END) Smart_Cargo,\n" +
            "sum(NetAmount) TotalSale\n" +
            "FROM (\n" +
            "(\n" +
            "SELECT z.name Zone,'Credit' Type,stn.Products Product,sum(c.totalAmount)  AS NetAmount\n" +
            "           FROM   Consignment c\n" +
            "INNER JOIN Branches b ON  c.orgin = b.branchCode\n" +
            "INNER JOIN Zones z ON  b.zoneCode = z.zoneCode\n" +
            "INNER JOIN CreditClients cc ON  c.creditClientId = cc.id\n" +
            "INNER JOIN ServiceTypes_New AS stn ON  stn.serviceTypeName = c.serviceTypeName\n" +
            "WHERE  consignerAccountNo NOT IN ('0', '999') AND isApproved = '1'\n";
            if (clvar.Year != "")
            {
                sqlString += clvar.Year;
            }
            if (clvar.Month != "")
            {
                sqlString += clvar.Month;
            }
            sqlString += "   \n" +
            "AND c.isPriceComputed='1'\n" +
            " AND c.creditClientId != '9'\n" +
            // " ('305289','350799','350800','350801','350802','350804','350806','350807','350808','350809','350810','305290','305291','305292','305552','305554','305555','305905','306180','306851','306852','306853','306875','307126','307127','307128','308228','309598','294188','294192','296013','311104','308518','308520','312271','312449','312769','312775','315238','314629','314630','317032','317040','317043','317045','319113','322994','322461','327590','328002','330058','335397','342428','343850','344309','344310','344311','344312','344315','344316','328802','303511','303514','323828','323835','323838','323841','324212','324214','324216','326070','326071','326088','326089','326092','326256','326421','326423','326620','326622','326623','324297','324298','324299','324300','324301','325436','325437','325438','325879','330249','326971','326973','326974','332171','332407','331420','342506','344138','344553','304648','304649','304650','304651','297298','297301','297303','297305','297307','305620','305780','306566','306567','306568','302799','302801','306726','306727','306728','308515','308516','307832','312452','312453','312767','314631','317039','318543','319114','319115','322991','322993','323839','323840','324210','324213','325439','325441','322462','322463','326069','326072','326091','326094','326420','326422','326970','326972','333578','336164','335996','335997','336092','336113','336817','336819','336912','342404','326619','326621','344313','344314','344712','343130','289745','290836','292001','292002','292626','293291','294175','294177','294178','294186','294187','294190','294191','294729','295352','296010','296012','296753','296653','296740','296742','297294','297295','297296','297297','297300','297302','297304','297306','296822','296837','302798','305889','306021','306422','306837','306967','306968','306969','307289','307290','307291','307365','307368','307369','307370','308517','308519','308521','308522','307805','307859','312928','312929','312269','312445','312446','314627','314628','317041','317044','318544','319116','322992','323806','323833','323834','323836','323837','308684','324211','324215','324312','325440','325442','325443','332717','326967','326968','326969','330141','322464','326068','326073','326074','326087','326090','326093','326418','326419','326424','326617','326618','326624','342464','344009','342768','342987')\n" +
            "GROUP BY z.name ,stn.Products\n" +
            ")\n" +
            "UNION\n" +
            "(SELECT z.name Zone,'Cash' TYPE, stn.Products  AS Product,\n" +
            "sum(CASE\n" +
            "WHEN c.serviceTypeName = 'Expressions' THEN c.totalAmount\n" +
            "ELSE (c.chargedAmount / ((tm.gst / 100) + 1))\n" +
            "END) NetAmount\n" +
            "FROM   Consignment c\n" +
            "INNER JOIN Branches b\n" +
            " ON  c.orgin = b.branchCode\n" +
            "INNER JOIN zones z\n" +
            " ON  b.zoneCode = z.zoneCode\n" +
            "INNER JOIN ServiceTypes_New AS stn\n" +
            " ON  stn.serviceTypeName = c.serviceTypeName\n" +
            "INNER JOIN ServiceTypes s\n" +
            " ON  c.serviceTypeName = s.serviceTypeName\n" +
            "left join  (select bb.companyId,b1.sname Branch,bb.gst,t.[Max ID] from BranchGST bb inner join Branches b1 on bb.branchCode=b1.branchCode inner join (select b.companyId,b.branchCode, max(b.effectiveFrom)'Max ID' from BranchGST b group by b.companyId,b.branchCode)t on  bb.branchCode=t.branchCode and bb.companyId=t.companyId  and bb.effectiveFrom=t.[Max ID])tm on b.sname=tm.Branch and s.companyId=tm.companyId\n" +
            "WHERE  c.consignerAccountNo = '0' AND isApproved = '1'\n";

            if (clvar.Year != "")
            {
                sqlString += clvar.Year;
            }
            if (clvar.Month != "")
            {
                sqlString += clvar.Month;
            }

            sqlString += "   \n" +
            "GROUP BY z.name ,stn.Products\n" +
            ")) b\n" +
            "GROUP BY\n" +
            "b.Zone,b.Type\n" +
            "ORDER BY 1,2";


            SqlCommand cmd = new SqlCommand(sqlString, orcl);
            cmd.CommandTimeout = 500;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            orcl.Close();
            return ds;


        }

        public DataTable Get_AccountwiseBookingReport(Variable clvar)
        {


            string sqlString = "select\n" +
            "       cc.accountNo 'ACCOUNT NO',\n" +
            "       cc.name 'PRODUCT',\n" +
            "       c.consignmentNumber 'CN NO',\n" +
            "       CONVERT(VARCHAR(10), c.bookingDate, 105) 'B.DATE',\n" +
            "       c.consignee 'CONSIGNEE',\n" +
            "       c.couponNumber 'REF NO',\n" +
            "       c.address 'ADDRESS',\n" +
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
            "       end 'DEST',\n" +
            "       ZZ.name ZONE,\n" +
            "       c.weight WEIGHT,\n" +
            "       case\n" +
            "         when c.consignmentTypeId = '13' and c.serviceTypeName = 'overnight' then\n" +
            "          'Hand Carry'\n" +
            "         else\n" +
            "          c.serviceTypeName\n" +
            "       end SERVICE\n" +
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
            " where\n" +
            "CAST(c.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "'\n" +
            "and c.consignerAccountNo = '" + clvar.ACNumber + "' \n" +
             "   and c.orgin = '" + clvar.Expresscentercode + "'\n";


            string sqlString_old = "select c.consignmentNumber 'Consignment Number',\n" +
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

            string sqlString = "SELECT w.consignmentNumber,\n" +
            "       w.OriginZone,\n" +
            "       w.OriginBranch,\n" +
            "       w.AccountNo,\n" +
            "       w.ClientAccountNo,\n" +
            "       w.BookingCode,\n" +
            "       w.ClientName,\n" +
            "       w.ConsigneeName,\n" +
            "       w.ReferenceNo,\n" +
            "       w.Address,\n" +
            "       CONVERT(NVARCHAR, w.BookingDate, 105) BookingDate,\n" +
            "       w.Service,\n" +
            "       w.CNType,\n" +
            "       w.Products,\n" +
            "       w.DestinationZone,\n" +
            "       w.DestinationBranch,\n" +
            "       w.Weight,\n" +
            "       w.pieces,\n" +
            "       w.Amount,\n" +
            "       max(w.riderCode) DeliveryRiderCode,\n" +
            //"       MAX(w.firstName) firstName,\n" +
            //"       MAX(w.lastName) lastName,\n" +
            "       MAX(w.firstName) + ' ' + MAX(w.lastName) FullName, \n" +
            "       CASE\n" +
            "         WHEN MAX(w.Status) = '' THEN\n" +
            "          'In Transit'\n" +
            "         ELSE\n" +
            "          MAX(w.Status)\n" +
            "       end STATUS,\n" +
            "       MAX(w.Reason) Reasion,\n" +
            "       MAX(w.RunSheetNumber) RunSheetNumber,\n" +
            "       MAX(w.ReceivedBy) ReceivedBy,\n" +
            "       MAX(w.Receiver_CNIC) CNIC,\n" +
            //  "       MAX(w.DeliveryDate) DeliveryDate,\n" +
            //  "       MAX(w.Time) TIME\n" +
            "       CASE WHEN CONVERT(NVARCHAR, MAX(w.DeliveryDate), 105) = '01-01-1900' THEN ' - ' ELSE CONVERT(NVARCHAR, MAX(w.DeliveryDate), 105) end DeliveryDate ,\n" +
            "       CONVERT(VARCHAR(8),MAX(w.Time),108) AS Time \n" +
            "  FROM (SELECT c.consignmentNumber,\n" +
            "               z.zoneCode,\n" +
            "               b.branchCode,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "               c.consignee ConsigneeName,\n" +
            "               c.couponNumber ReferenceNo,\n" +
            "               c.address ADDRESS,\n" +
            "               c.bookingDate BookingDate,\n" +
            "               c.serviceTypeName Service,\n" +
            "               ct.name CNType,\n" +
            "\n" +
            "               t.Products,\n" +
            "               zz.name DestinationZone,\n" +
            "               CASE\n" +
            "                 WHEN c.serviceTypeName IN ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') THEN\n" +
            "                  c.destinationCountryCode\n" +
            "                 ELSE\n" +
            "                  bb.sname\n" +
            "               END DestinationBranch,\n" +
            "               c.weight WEIGHT,\n" +
            "               c.pieces,\n" +
            "               c.TotalAmount Amount,\n" +
            "               '' riderCode,\n" +
            "               '' firstName,\n" +
            "               '' lastName,\n" +
            "               '' STATUS,\n" +
            "               '' Reason,\n" +
            "               '' RunSheetNumber,\n" +
            "               '' ReceivedBy,\n" +
            "               '' Receiver_CNIC,\n" +
            "               '' DeliveryDate,\n" +
            "               '' TIME\n" +
            "          FROM Consignment c\n" +
            "         INNER JOIN creditclients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "         INNER JOIN ConsignmentType AS ct\n" +
            "            ON ct.id = c.consignmentTypeId\n" +
            "         INNER JOIN Branches b\n" +
            "            ON c.orgin = b.branchCode\n" +
            "         INNER JOIN Zones z\n" +
            "            ON b.zoneCode = z.zoneCode\n" +
            "         INNER JOIN Branches bb\n" +
            "            ON c.destination = bb.branchCode\n" +
            "         INNER JOIN ServiceTypes_New t\n" +
            "            ON c.serviceTypeName = t.serviceTypeName\n" +
            "         INNER JOIN Zones zz\n" +
            "            ON bb.zoneCode = zz.zoneCode\n" +
            "        UNION\n" +
            "\n" +
            "        SELECT c.consignmentNumber,\n" +
            "               z.zoneCode,\n" +
            "               b.branchCode,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "               c.consignee ConsigneeName,\n" +
            "               c.couponNumber ReferenceNo,\n" +
            "               c.address ADDRESS,\n" +
            "               c.bookingDate BookingDate,\n" +
            "               c.serviceTypeName Service,\n" +
            "               ct.name CNType,\n" +
            "               t.Products,\n" +
            "               zz.name DestinationZone,\n" +
            "               CASE\n" +
            "                 WHEN c.serviceTypeName IN ('International Cargo',\n" +
            "                                            'International Expressions',\n" +
            "                                            'International_Box',\n" +
            "                                            'International_Doc',\n" +
            "                                            'International_Doc_Special_Hub',\n" +
            "                                            'International_Non-Doc',\n" +
            "                                            'International_Non-Doc_Special_Hub_2014',\n" +
            "                                            'Logex') THEN\n" +
            "                  c.destinationCountryCode\n" +
            "                 ELSE\n" +
            "                  bb.sname\n" +
            "               END DestinationBranch,\n" +
            "               c.weight WEIGHT,\n" +
            "               c.pieces,\n" +
            "               c.TotalAmount Amount,\n" +
            "               --  rc.Status,\n" +
            "               rr1.riderCode,\n" +
            "               r.firstName,\n" +
            "               r.lastName,\n" +
            "               rc.[Status],\n" +
            "               rc.Reason,\n" +
            "               rc.runsheetNumber RunSheetNumber,\n" +
            "               rc.receivedBy     ReceivedBy,\n" +
            "               rc.Receiver_CNIC,\n" +
            "               rc.deliveryDate   DeliveryDate,\n" +
            "               rc.time           TIME\n" +
            "          FROM Consignment c\n" +
            "         INNER JOIN creditclients cc\n" +
            "            ON c.creditClientId = cc.id\n" +
            "         INNER JOIN ConsignmentType AS ct\n" +
            "            ON ct.id = c.consignmentTypeId\n" +
            "         INNER JOIN Branches b\n" +
            "            ON c.orgin = b.branchCode\n" +
            "         INNER JOIN Zones z\n" +
            "            ON b.zoneCode = z.zoneCode\n" +
            "         INNER JOIN Branches bb\n" +
            "            ON c.destination = bb.branchCode\n" +
            "         INNER JOIN ServiceTypes_New t\n" +
            "            ON c.serviceTypeName = t.serviceTypeName\n" +
            "         INNER JOIN Zones zz\n" +
            "            ON bb.zoneCode = zz.zoneCode\n" +
            "         INNER JOIN RunsheetConsignment rc\n" +
            "            ON rc.consignmentNumber = c.consignmentNumber\n" +
            "           AND rc.createdOn =\n" +
            "               (SELECT MAX(createdon)\n" +
            "                  FROM RunsheetConsignment rc1\n" +
            "                 WHERE rc1.consignmentNumber = rc.consignmentNumber)\n" +
            "         INNER JOIN (SELECT rr.*, rec.bid\n" +
            "                      FROM RiderRunsheet AS rr\n" +
            "                     INNER JOIN ExpressCenters AS rec\n" +
            "                        ON rec.expressCenterCode = rr.expIdTemp) as rr1\n" +
            "            ON rr1.runsheetNumber = rc.runsheetNumber\n" +
            "           AND rr1.bid = rc.branchcode\n" +
            "         INNER JOIN Riders AS r\n" +
            "            ON r.riderCode = rr1.riderCode\n" +
            "           AND r.expressCenterId = rr1.expIdTemp\n" +
            "\n" +
            "        ) w\n" +
            " WHERE\n" +
            "\n" +
            //" CAST(w.bookingDate AS DATE) BETWEEN '2016-06-01' AND '2016-06-30'\n" +
            //" AND w.zoneCode IN ('2')\n" +
            //" AND w.branchCode IN ('4')\n" +
            //" AND w.AccountNo = '28001'\n" +

            " CAST(w.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "' \n" +
            " and w.AccountNo = '" + clvar.ACNumber + "' \n" +
            " and w.branchCode = '" + clvar.Expresscentercode + "' \n" +

            " GROUP BY w.consignmentNumber,\n" +
            "          w.zoneCode,\n" +
            "          w.branchCode,\n" +
            "          w.OriginZone,\n" +
            "          w.OriginBranch,\n" +
            "          w.AccountNo,\n" +
            "          w.ClientAccountNo,\n" +
            "          w.BookingCode,\n" +
            "          CONVERT(NVARCHAR, w.BookingDate, 105),\n" +
            "          w.ClientName,\n" +
            "          w.ConsigneeName,\n" +
            "          w.ReferenceNo,\n" +
            "          w.Address,\n" +
            "          w.Service,\n" +
            "          w.CNType,\n" +
            "          w.Products,\n" +
            "          w.DestinationZone,\n" +
            "          w.DestinationBranch,\n" +
            "          w.Weight,\n" +
            "          w.pieces,\n" +
            "          w.Amount\n" +
            " ORDER BY 2, 3, 11";



            #region OLD_QUERY_OK

            string sqlString_ok = "select\n" +
            "     w.consignmentNumber,\n" +
            "       w.OriginZone,\n" +
            "       w.OriginBranch,\n" +
            "       w.AccountNo,\n" +
            "       w.ClientAccountNo,\n" +
            "       w.BookingCode,\n" +
            "       w.ClientName,\n" +
            "       w.ConsigneeName,\n" +
            "       w.ReferenceNo,\n" +
            "       w.Address,\n" +
            "       CONVERT(NVARCHAR, w.BookingDate, 105) BookingDate,\n" +
            "       w.Service,\n" +
            "       w.Products,\n" +
            "       w.DestinationZone,\n" +
            "       w.DestinationBranch,\n" +
            "       w.Zoning,\n" +
            "       w.Weight,\n" +
            "       w.pieces,\n" +
            "       w.Amount,\n" +
            "       w.branchCode,\n" +
            "       CASE WHEN MAX(w.Status) = '' THEN 'In Transit' ELSE MAX(w.Status) end STATUS ,\n" +
            "       MAX(w.RunSheetNumber) RunSheetNumber,\n" +
            "       MAX(w.ReceivedBy) ReceivedBy,\n" +
            //   "       MAX(w.DeliveryDate) DeliveryDate,\n" +
            //   "       MAX(w.Time) Time\n" +
            "      CASE WHEN CONVERT(NVARCHAR, MAX(w.DeliveryDate), 105) = '01-01-1900' THEN ' - ' ELSE CONVERT(NVARCHAR, MAX(w.DeliveryDate), 105) end DeliveryDate ,\n" +
            "       CONVERT(VARCHAR(8),MAX(w.Time),108) AS Time \n" +
            "  from (select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               b.branchCode,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "               c.consignee ConsigneeName,\n" +
            "               c.couponNumber ReferenceNo,\n" +
            "               c.address Address,\n" +
            "               c.bookingDate BookingDate,\n" +
            "               case\n" +
            "                 when c.consignmentTypeId = '13' and\n" +
            "                      c.serviceTypeName = 'overnight' then\n" +
            "                  'Hand Carry'\n" +
            "                 else\n" +
            "                  c.serviceTypeName\n" +
            "               end Service,\n" +
            "               t.Products,\n" +
            "               zz.name DestinationZone,\n" +
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
            "union\n" +
            "\n" +
            "        select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               b.branchCode,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "               c.consignee ConsigneeName,\n" +
            "               c.couponNumber ReferenceNo,\n" +
            "               c.address Address,\n" +
            "               c.bookingDate BookingDate,\n" +
            "               case\n" +
            "                 when c.consignmentTypeId = '13' and\n" +
            "                      c.serviceTypeName = 'overnight' then\n" +
            "                  'Hand Carry'\n" +
            "                 else\n" +
            "                  c.serviceTypeName\n" +
            "               end Service,\n" +
            "               t.Products,\n" +
            "               zz.name DestinationZone,\n" +
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
            " ) w\n" +
            " where\n" +
            " CAST(w.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "' \n" +
            " and w.AccountNo = '" + clvar.ACNumber + "' \n" +
            " and w.branchCode = '" + clvar.Expresscentercode + "' \n" +
            " group by w.consignmentNumber,\n" +
            "          w.OriginZone,\n" +
            "          w.OriginBranch,\n" +
            "          w.AccountNo,\n" +
            "          w.ClientAccountNo,\n" +
            "          w.BookingCode,\n" +
            "          CONVERT(NVARCHAR, w.BookingDate, 105),\n" +
            "          w.ClientName,\n" +
            "          w.ConsigneeName,\n" +
            "          w.ReferenceNo,\n" +
            "          w.Address,\n" +
            "          w.Service,\n" +
            "          w.Products,\n" +
            "          w.DestinationZone,\n" +
            "          w.DestinationBranch,\n" +
            "          w.Zoning,\n" +
            "          w.Weight,\n" +
            "          w.pieces,\n" +
            "          w.Amount,\n" +
            "          w.branchCode\n" +
            " order by 1";

            #endregion

            #region OLD_QUERY

            /*
             * 


            string sqlString22 = "\n" +
            "select\n" +
            "     w.consignmentNumber,\n" +
            "       w.OriginZone,\n" +
            "       w.OriginBranch,\n" +
            "       w.AccountNo,\n" +
            "       w.ClientAccountNo,\n" +
            "       w.BookingCode,\n" +
            "       w.ClientName,\n" +
            "       w.ConsigneeName,\n" +
            "       w.ReferenceNo,\n" +
            "       w.Address,\n" +
            "       w.BookingCode,\n" +
            "       CONVERT(NVARCHAR, w.BookingDate, 105) BookingDate,\n" +
            "       w.Service,\n" +
            "       w.Products,\n" +
            "       w.DestinationZone,\n" +
            "       w.DestinationBranch,\n" +
            "       w.Zoning,\n" +
            "       w.Weight,\n" +
            "       w.pieces,\n" +
            "       w.Amount,\n" +
            "     CASE WHEN MAX(w.Status) = '' THEN 'In Transit' ELSE MAX(w.Status) end STATUS ,\n" +
            "       MAX(w.RunSheetNumber) RunSheetNumber,\n" +
            "       MAX(w.ReceivedBy) ReceivedBy,\n" +
            "      CASE WHEN CONVERT(NVARCHAR, MAX(w.DeliveryDate), 105) = '01-01-1900' THEN ' - ' ELSE CONVERT(NVARCHAR, MAX(w.DeliveryDate), 105) end DeliveryDate ,\n" +
            "       CONVERT(VARCHAR(8),MAX(w.Time),108) AS Time  from (select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.branchCode branchCode,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "               c.consignee ConsigneeName,\n" +
            "               c.couponNumber ReferenceNo,\n" +
            "               c.address Address,\n" +
            "               c.bookingDate BookingDate,\n" +
            "               case\n" +
            "                 when c.consignmentTypeId = '13' and\n" +
            "                      c.serviceTypeName = 'overnight' then\n" +
            "                  'Hand Carry'\n" +
            "                 else\n" +
            "                  c.serviceTypeName\n" +
            "               end Service,\n" +
            "               t.Products,\n" +
            "               zz.name DestinationZone,\n" +
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
            "union\n" +
            "\n" +
            "        select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               cc.accountNo AccountNo,\n" +
            "               b.branchCode branchCode,\n" +
            "               z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
            "               b.sname + '-' + c.riderCode BookingCode,\n" +
            "               cc.name ClientName,\n" +
            "               c.consignee ConsigneeName,\n" +
            "               c.couponNumber ReferenceNo,\n" +
            "               c.address Address,\n" +
            "               c.bookingDate BookingDate,\n" +
            "               case\n" +
            "                 when c.consignmentTypeId = '13' and\n" +
            "                      c.serviceTypeName = 'overnight' then\n" +
            "                  'Hand Carry'\n" +
            "                 else\n" +
            "                  c.serviceTypeName\n" +
            "               end Service,\n" +
            "               t.Products,\n" +
            "               zz.name DestinationZone,\n" +
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
            " ) w\n" +
            " where\n" +
            //" CAST(w.bookingDate as date) between '2016-07-01' AND '2016-07-02'\n" +
            //" and w.AccountNo = '7240'\n" +
            //" and w.branchCode = '4'\n" +

             " CAST(w.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "' \n" +
            " and w.AccountNo = '" + clvar.ACNumber + "' \n" +
            " and w.branchCode = '" + clvar.Expresscentercode + "' \n" +

            " group by w.consignmentNumber,\n" +
            "          w.OriginZone,\n" +
            "          w.OriginBranch,\n" +
            "          w.AccountNo,\n" +
            "          w.ClientAccountNo,\n" +
            "          w.BookingCode,\n" +
            "          CONVERT(NVARCHAR, w.BookingDate, 105),\n" +
            "          w.branchCode,\n" +
            "          w.ClientName,\n" +
            "          w.ConsigneeName,\n" +
            "          w.ReferenceNo,\n" +
            "          w.Address,\n" +
            "          w.Service,\n" +
            "          w.Products,\n" +
            "          w.DestinationZone,\n" +
            "          w.DestinationBranch,\n" +
            "          w.Zoning,\n" +
            "          w.Weight,\n" +
            "          w.pieces,\n" +
            "          w.Amount\n" +
            " order by 1\n" +
            "";
             * 
             * 
            string sqlString = "select w.consignmentNumber,\n" +
            "       w.OriginZone,\n" +
            "       w.OriginBranch,\n" +
            "       w.branchCode branchCode,\n" +
            "       w.AccountNo,\n" +
            "       w.ClientAccountNo,\n" +
            "       w.BookingCode,\n" +
            "       CONVERT(NVARCHAR, w.BookingDate, 105) BookingDate,\n" +
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
          //  "       MAX(w.DeliveryDate) DeliveryDate,\n" +
          //  "       MAX(w.Time) Time\n" +
            "       CONVERT(NVARCHAR, MAX(w.DeliveryDate), 105) DeliveryDate,\n" +
           "       CONVERT(VARCHAR(8),MAX(w.Time),108) AS Time\n" +
            "  from (select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               b.branchCode branchCode,\n" +
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
            "\n" +
            "        union\n" +
            "\n" +
            "        select c.consignmentNumber,\n" +
            "               z.name OriginZone,\n" +
            "               b.name OriginBranch,\n" +
            "               b.branchCode branchCode,\n" +
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
            "\t\t\t   ) w\n" +
            " where\n" +
            " CAST(w.bookingDate as date) between '" + clvar.StartDate + "' AND '" + clvar.EndDate + "' \n" +
            " and w.AccountNo = '" + clvar.ACNumber + "' \n" +
            " and w.branchCode = '" + clvar.Expresscentercode + "' \n" +
            " group by w.consignmentNumber,\n" +
            "          w.OriginZone,\n" +
            "          w.OriginBranch,\n" +
            "          w.branchCode,\n" +
            "          w.AccountNo,\n" +
            "          w.ClientAccountNo,\n" +
            "          w.BookingCode,\n" +
            "          CONVERT(NVARCHAR, w.BookingDate, 105),\n" +
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
            */
            /*

            string sqlString_old = "Select *\n" +
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

            sqlString_old = "Select w.*,\n" +
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


            sqlString_old = "select w.consignmentNumber,\n" +
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
            */

            #endregion

            DataTable dt = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 3000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception ex)
            { }
            return dt;
        }



        public DataSet Get_AccountWiseDeliveryHeader(Variable clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sqlString = "SELECT z.zoneCode,\n" +
                "       b.branchCode,\n" +
                "       z.name OriginZone,\n" +
                "       b.name OriginBranch,\n" +
                "       cc.accountNo AccountNo,\n" +
                "       z.name + '-' + b.sname + '-' + cc.accountNo ClientAccountNo,\n" +
                "       cc.name ClientName\n" +
                "  FROM creditclients cc\n" +
                " INNER JOIN Branches b\n" +
                "    ON cc.branchCode = b.branchCode\n" +
                " INNER JOIN Zones z\n" +
                "    ON b.zoneCode = z.zoneCode\n" +
                " where \n" +
                " cc.AccountNo = '" + clvar.ACNumber + "' \n" +
                " and cc.branchCode = '" + clvar.Expresscentercode + "' \n";

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

        #endregion
    }
}