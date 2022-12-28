using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Web.Services;
using System.Configuration;

namespace MRaabta.Files
{
    public partial class SalesPersonVisitingCustomer : System.Web.UI.Page
    {
        public static string U_ID = "";
        public string Profile_User = "";
        static string SessionZone = "", SessionBranch = "";
        static Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                U_ID = Session["U_ID"].ToString();
                Profile_User = Session["PROFILE"].ToString();
                SessionBranch = Session["BRANCHCODE"].ToString();
                SessionZone = Session["ZONECODE"].ToString();
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }
        [WebMethod]
        public static string FilterCustomer(SearchFilter data)
        {
            SqlConnection conn = new SqlConnection(clvar.Strcon());
            string table = "", SecondHead = "", DatePivot = "", DatePivotSum = "", DatePivotGroup = "";
            List<int> SundayList = new List<int>();

            try
            {
                HttpContext.Current.Session["SalesPersonSelected"] = data.salesPerson_ddl;

                conn.Open();
                String sql;
                string sql2, sql_old;
                int SundayCounter = 13;
                sql = @" SELECT YEAR,[Month],CONVERT(VARCHAR(2), CONVERT(VARCHAR, Date, 106) , 111) Date, SUBSTRING(DAY, 1, 3 )DAY,Flag,Holiday,date date_,Convert(varchar,date,23) date_Head FROM MNP_TargetCalendar WHERE [Year]='" + data.Year_ddl + "' AND [Month]='" + data.Month_ddl + "'";
                using (var cmd2 = new SqlCommand(sql, conn))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();
                    table = "<thead><tr><th class='sticky-col first-col'></th><th></th><th></th><th class='sticky-col second-col'></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th>";
                    while (dr.Read())
                    {
                        //  if (dr.GetDateTime(6).Date<=(System.DateTime.Now).Date.AddDays(-1)) {
                        DatePivot += ", 0 [" + dr.GetString(7) + "] ";
                        DatePivotSum += ",SUM([" + dr.GetString(7) + "]) [" + dr.GetDateTime(6).ToString() + "]";
                        DatePivotGroup += ",[" + dr.GetString(7) + "]";
                        if (dr.GetInt32(4) == 0)
                        {
                            SundayList.Add(SundayCounter);
                            table += "<th style='width: 64px!important;background-color:lightgreen;color:black'>" + dr.GetString(3) + "</th>";
                            SecondHead += "<th class='header_1' style='background-color:lightgreen;color:black'>" + dr.GetString(2) + "</th>";
                        }
                        else
                        {
                            table += "<th style='width: 64px!important; '>" + dr.GetString(3) + "</th>";
                            SecondHead += "<th class='header_1'>" + dr.GetString(2) + "</th>";
                        }
                        SundayCounter++;
                    }
                    table += "</tr><tr class='header_1'>";
                    table += "<th class='sticky-col first-col'>S. No</th><th >Region</th><th>Account No</th><th class='sticky-col second-col'>Customer Name</th><th>Category</th><th>Domestic</th><th>COD</th><th>International</th><th>Express Cargo</th><th>Total Revenue</th><th>Expected Revenue</th><th>Total Visit</th>";

                    table += SecondHead;
                    table += "</tr></thead>";
                    DatePivotGroup = DatePivotGroup.Remove(0, 1);

                }
                sql2 = @"SELECT 
                        REGION, ACCOUNTNO,zoneCode ,branchCode,CUSTOMERNAME,CATEGORY, 
                        SUM(ROUND(ISNULL(DOMESTIC,'0'),0)) DOMESTIC, 
                            SUM(ROUND(ISNULL(COD,'0'),0)) COD, 
                            SUM(ROUND(ISNULL(EXPRESSCARGO,'0'),0)) EXPRESSCARGO, 
                            SUM(ROUND(ISNULL(INTERNATIONAL,'0'),0))INTERNATIONAL,	
                            SUM(ROUND(ISNULL(DOMESTIC,'0'),0)) + 
                            SUM(ROUND(ISNULL(COD,'0'),0)) + 
                            SUM(ROUND(ISNULL(EXPRESSCARGO,'0'),0)) + 
                            SUM(ROUND(ISNULL(INTERNATIONAL,'0'),0)) REVENUE,
                            ExpectedRevenue,0 [Total Visit]
                            " + DatePivotSum + @"

                        FROM (

                        SELECT 
                            REGION, ACCOUNTNO,zoneCode ,branchCode,CUSTOMERNAME,CATEGORY, ExpectedRevenue,
                            SUM(ROUND(ISNULL(DOMESTIC,'0'),0)) DOMESTIC, 
                            SUM(ROUND(ISNULL(COD,'0'),0)) COD, 
                            SUM(ROUND(ISNULL(EXPRESSCARGO,'0'),0)) EXPRESSCARGO, 
                            SUM(ROUND(ISNULL(INTERNATIONAL,'0'),0))INTERNATIONAL,	
                            SUM(ROUND(ISNULL(DOMESTIC,'0'),0)) + 
                            SUM(ROUND(ISNULL(COD,'0'),0)) + 
                            SUM(ROUND(ISNULL(EXPRESSCARGO,'0'),0)) + 
                            SUM(ROUND(ISNULL(INTERNATIONAL,'0'),0)) REVENUE
                            " + DatePivot + @"
                        FROM (
                        SELECT DISTINCT
                            Z.REGION, Z.NAME+'-'+B.SNAME+'-'+CC.ACCOUNTNO ACCOUNTNO, CC.NAME CUSTOMERNAME, CS.USERNAME,'EXISTING' CATEGORY	
                            ,CASE WHEN ISNULL(C.COD,'0') = '1' THEN SUM(C.NETSALE) END COD 
                            ,CASE WHEN ISNULL(C.COD,'0') = '0' AND C.SERVICETYPENAME NOT LIKE 'INTERNA%' AND C.SERVICETYPENAME NOT IN ('EXPRESS CARGO') THEN SUM(C.NETSALE) END DOMESTIC 
                            ,CASE WHEN C.SERVICETYPENAME LIKE 'INTERNA%' THEN SUM(C.NETSALE) END INTERNATIONAL 
                            ,CASE WHEN C.SERVICETYPENAME ='EXPRESS CARGO' THEN SUM(C.NETSALE) END EXPRESSCARGO,z.zoneCode ,b.branchCode,0 ExpectedRevenue
                        FROM 
                            CLIENTSTAFF CS    
                            INNER JOIN BTSUSERS BS  ON  CS.USERNAME = BS.USERNAME
                            INNER JOIN USERASSOCIATION U ON CS.USERNAME = U.USERNAME
                            INNER JOIN BRANCHES B ON B.BRANCHCODE = U.BRANCHCODE
                            INNER JOIN ZONES Z ON B.ZONECODE = Z.ZONECODE
                            INNER JOIN CREDITCLIENTS CC ON CC.ID = CS.CLIENTID
                            LEFT JOIN (     
                               SELECT SUM(C.TOTALAMOUNT) NETSALE, CC.ID, C.COD, C.SERVICETYPENAME
                               FROM   CONSIGNMENT C     
                                      INNER JOIN CREDITCLIENTS CC     
                                           ON  C.CREDITCLIENTID = CC.ID     
                                      INNER JOIN BRANCHES B     
                                           ON  CC.BRANCHCODE = B.BRANCHCODE     
                                      INNER JOIN ZONES Z     
                                           ON  B.ZONECODE = Z.ZONECODE     
                               WHERE  YEAR(C.BOOKINGDATE) = '" + data.Year_ddl + @"'
                                    AND MONTH(C.BOOKINGDATE) =  '" + data.Month_ddl + @"'
                                    AND CONSIGNERACCOUNTNO NOT IN ('0', '999')  
                                    AND ISNULL(ISAPPROVED, '0') = '1'  
                                    AND ISNULL(CC.STATUS,'0') != '9'  
                                    AND ISNULL(C.ISPRICECOMPUTED, '0') = '1'
                               GROUP BY          
                                    CC.ID, C.COD,C.SERVICETYPENAME   
                            ) C        
                            ON  CC.ID = C.ID 
                        WHERE 
	                        CS.STAFFTYPEID = '214'  
	                        AND U.id = '" + data.salesPerson_ddl + @"'
	                        AND z.zoneCode='" + SessionZone + @"'
	                        AND b.branchCode='" + SessionBranch + @"'
                        GROUP BY
                            Z.REGION, Z.NAME+'-'+B.SNAME+'-'+CC.ACCOUNTNO, CC.NAME, CS.USERNAME, C.COD, C.SERVICETYPENAME	,z.zoneCode ,b.branchCode
                        ) X
                        GROUP BY
                            REGION, ACCOUNTNO, CUSTOMERNAME, USERNAME,CATEGORY,zoneCode ,branchCode,ExpectedRevenue
     
                        UNION ALL
    
                        SELECT * FROM (
                        SELECT DISTINCT
	                        z.REGION, Z.NAME+'-'+B.SNAME+'-'+CC.ACCOUNTNO ACCOUNTNO,
	                        Z.zoneCode,B.branchCode,
                            CASE WHEN SV.Category ='1' THEN CC.NAME WHEN SV.Category = '2' THEN SV.CustomerName END CUSTOMERNAME, 
	                        CASE WHEN SV.Category IN ('1',NULL) THEN 'EXISTING' WHEN SV.Category = '2' THEN 'NEW' END CATEGORY, sv.ExpectedRevenue,
                            0 DOMESTIC, 
                            0 COD, 
                            0 EXPRESSCARGO, 
                            0 INTERNATIONAL,	
                            0 REVENUE 
                            ,mtc.Date, sv.ScheduleVisit
                        FROM
	                        MNP_TargetCalendar mtc
	                        INNER JOIN MNP_ScheduleVisit sv ON mtc.Date = sv.Date  
                            INNER JOIN USERASSOCIATION U ON sv.StaffId = u.id
                            INNER join CLIENTSTAFF CS ON CS.USERNAME = U.USERNAME   
                            INNER JOIN BTSUSERS BS  ON  CS.USERNAME = BS.USERNAME
                            INNER JOIN BRANCHES B ON B.BRANCHCODE = SV.BRANCH 
                            INNER JOIN ZONES Z ON SV.ZONE = Z.ZONECODE 
                            left JOIN CREDITCLIENTS CC ON CC.accountNo = SV.AccountNo AND CC.id = CS.ClientId
                        WHERE 
	                        mtc.[Year]= '" + data.Year_ddl + @"'
	                        and mtc.[month]= '" + data.Month_ddl + @"'
	                        AND sv.StaffId = '" + data.salesPerson_ddl + @"'
	                        and z.zoneCode='" + SessionZone + @"'
	                        AND b.branchCode='" + SessionBranch + @"'
                        ) s 
                        PIVOT(
	                        sum(ScheduleVisit) FOR date IN (" + DatePivotGroup + @")	
                        )	AS p
    
                        ) b    
                        WHERE b.CUSTOMERNAME IS NOT NULL    
                        GROUP BY
                        REGION, ACCOUNTNO,zoneCode ,branchCode,CUSTOMERNAME,CATEGORY,ExpectedRevenue
                        ORDER BY   11 DESC,6,12 DESC,5 asc ";


                DataSet dt = new DataSet();
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sql2, orcl);
                orcd.CommandTimeout = 3000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();

                if (dt.Tables[0].Rows.Count > 0)
                {
                    table += "<tbody >";

                    //table += "<tbody id='tbodyMainTable'>";
                    for (int rows = 0; rows < dt.Tables[0].Rows.Count; rows++)
                    {
                        table += "<tr>";
                        table += "<td class='sticky-col first-col'>";
                        table += rows + 1;
                        table += "</td>";
                        for (int cols = 0; cols < dt.Tables[0].Columns.Count; cols++)
                        {
                            if (cols <= 12)
                            {
                                if (cols == 1)
                                {
                                    table += "<td width='10%' class=' '><input type='text' placeholder='AccountNo' value='" + dt.Tables[0].Rows[rows][cols].ToString() + "' class='AccountNo'   style='display:none'/>";
                                    table += dt.Tables[0].Rows[rows][cols].ToString();
                                    table += "</td>";
                                }
                                else if (cols == 2)
                                {

                                    table += "<td width='10%'  class=' ' style='display:none'><input type='text' placeholder='Zone' value='" + dt.Tables[0].Rows[rows][cols].ToString() + "' class='Zone'   style='display:none'/>";
                                    table += dt.Tables[0].Rows[rows][cols].ToString();
                                    table += "</td>";
                                }
                                else if (cols == 3)
                                {

                                    table += "<td width='10%'  class=' ' style='display:none'> <input type='text' placeholder='Branch' value='" + dt.Tables[0].Rows[rows][cols].ToString() + "' class='Branch'   style='display:none'/>";
                                    table += dt.Tables[0].Rows[rows][cols].ToString();
                                    table += "</td>";
                                }
                                else if (cols == 4)
                                {
                                    table += "<td width='10%'  class='sticky-col second-col'><input type='text' placeholder='Enter Customer' value='" + dt.Tables[0].Rows[rows][cols].ToString() + "' class='Customer'   style='display:none'/>";
                                    table += dt.Tables[0].Rows[rows][cols].ToString();
                                    table += "</td>";
                                }
                                else if (cols == 5)
                                {
                                    table += "<td width='10%'>";
                                    table += dt.Tables[0].Rows[rows][cols].ToString();
                                    table += "</td>";
                                    if (dt.Tables[0].Rows[rows][cols].ToString() == "EXISTING")
                                    {
                                        table += "<td width='10%' class='Category' style='display:none'>1</td>";
                                    }
                                    else
                                    {
                                        table += "<td width='10%' class='Category' style='display:none'>2</td>";
                                    }


                                }
                                else if (cols == 11)
                                {
                                    table += "<td width='10%' class=''  >" + float.Parse(dt.Tables[0].Rows[rows][cols].ToString()).ToString("N0") + "<input type='number' placeholder='Enter Revenue' value='" + dt.Tables[0].Rows[rows][cols].ToString() + "' class='ExpectedRevenue'   style='display:none'/></td>";

                                }
                                else if (cols == 12)
                                {
                                    table += "<td width='10%'  class='VisitCount'>0</td>";

                                }
                                else
                                {
                                    int num = 0;
                                    if (int.TryParse(dt.Tables[0].Rows[rows][cols].ToString(), out num))
                                    {
                                        table += "<td width='10%'>";
                                        table += num.ToString("N0");
                                        table += "</td>";
                                    }
                                    else
                                    {
                                        table += "<td width='10%'>";
                                        table += dt.Tables[0].Rows[rows][cols].ToString();
                                        table += "</td>";
                                    }
                                }
                            }
                            else
                            {
                                if (SundayList.Contains(cols))
                                {
                                    if (dt.Tables[0].Rows[rows][cols].ToString() == "1")
                                    {
                                        if (DateTime.Parse(dt.Tables[0].Columns[cols].ColumnName.ToString()) <= (System.DateTime.Now).Date)
                                        {
                                            table += "<td width='10%' class='scroll' style='background-color:lightgreen'>";
                                            table += "<input type='checkbox' onclick='CountSelectedDates(this)' value='' checked='checked' disabled='disabled'  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                        else
                                        {
                                            table += "<td width='10%' class='scroll' style='background-color:lightgreen'>";
                                            table += "<input type='checkbox' onclick='CountSelectedDates(this)' value='' checked='checked'  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                    }
                                    else
                                    {
                                        if (DateTime.Parse(dt.Tables[0].Columns[cols].ColumnName.ToString()) <= (System.DateTime.Now).Date)
                                        {
                                            table += "<td width='10%' class='scroll' style='background-color:lightgreen'>";
                                            table += "<input type='checkbox'  onclick='CountSelectedDates(this)'  value=''  disabled='disabled'  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                        else
                                        {
                                            table += "<td width='10%' class='scroll' style='background-color:lightgreen'>";
                                            table += "<input type='checkbox' onclick='CountSelectedDates(this)' value=''  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                    }
                                }
                                else
                                {
                                    if (dt.Tables[0].Rows[rows][cols].ToString() == "1")
                                    {
                                        if (DateTime.Parse(dt.Tables[0].Columns[cols].ColumnName.ToString()) <= (System.DateTime.Now).Date)
                                        {
                                            table += "<td width='10%' class='scroll' >";
                                            table += "<input type='checkbox' onclick='CountSelectedDates(this)' value='' checked='checked' disabled='disabled'  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                        else
                                        {
                                            table += "<td width='10%' class='scroll' >";
                                            table += "<input type='checkbox' onclick='CountSelectedDates(this)' value='' checked='checked'  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                    }
                                    else
                                    {

                                        if (DateTime.Parse(dt.Tables[0].Columns[cols].ColumnName.ToString()) <= (System.DateTime.Now).Date)
                                        {
                                            table += "<td width='10%'  class='scroll'>";
                                            table += "<input type='checkbox'  onclick='CountSelectedDates(this)' value=''  disabled='disabled'  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                        else
                                        {
                                            table += "<td width='10%'  class='scroll'>";
                                            table += "<input type='checkbox'  onclick='CountSelectedDates(this)' value=''  name='SaleDate' class='" + rows + " " + dt.Tables[0].Columns[cols].ColumnName.ToString() + "' />";
                                            table += "</td>";
                                        }
                                    }
                                }
                            }
                        }
                        table += "</tr>";
                    }
                    table += "</tbody>";

                }
                else
                {
                    return "";
                }
                return table;
            }
            catch (Exception er)
            {
                return "";
            }
            finally
            {
                conn.Close();
            }
        }
        [WebMethod]
        public static List<DropdownSalesPerson> GetSalesPerson()
        {
            List<DropdownSalesPerson> list = new List<DropdownSalesPerson>();
            SqlConnection conn = new SqlConnection(clvar.Strcon());
            try
            {
                conn.Open();
                String sql;

                sql = @"SELECT distinct
                    u.username, Cast(u.Id as varchar)Id
                    FROM CLIENTSTAFF c 
                    INNER JOIN UserAssociation u ON c.UserName = u.username
                    INNER JOIN UserStaffType us ON us.username = u.username AND us.staffTypeId = '215'
                    INNER JOIN Branches b ON b.branchCode = u.branchCode
                    INNER JOIN Zones z ON B.zoneCode = Z.zoneCode
                    WHERE z.zoneCode='" + SessionZone + "' AND b.branchCode='" + SessionBranch + @"'
                    ORDER BY u.username";
                using (var cmd2 = new SqlCommand(sql, conn))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new DropdownSalesPerson { Value = dr.GetString(1), Text = dr.GetString(0) });
                    }
                }
                conn.Close();
                return list;
            }
            catch (SqlException ex)
            {
                conn.Close();
                return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        [WebMethod]
        public static string AddCustomer(SearchFilter data)
        {
            SqlConnection conn = new SqlConnection(clvar.Strcon());
            string tableRow = "", SecondHead = "";
            string branchesDropdown = "";
            string zonesDropdown = "";
            int rowcount = int.Parse(data.tableRows);
            rowcount = rowcount - 1;
            string SessionRegion = "";
            List<int> SundayList = new List<int>();
            int SundayCounter = 10;
            int rowCounter = 0;
            try
            {
                conn.Open();
                String sql_branch;
                String sql;
                sql_branch = @"  SELECT b.branchCode,b.sname branch,z.zoneCode,z.name zone,z.Region FROM Branches b INNER JOIN Zones z ON z.zoneCode = b.zoneCode WHERE b.branchCode='" + SessionBranch + "' AND b.[status]=1   ";
                using (var cmd_branch = new SqlCommand(sql_branch, conn))
                {
                    SqlDataReader dr_branch;
                    dr_branch = cmd_branch.ExecuteReader();
                    branchesDropdown = "<select class='Branch'  >";
                    zonesDropdown = "<select class='Zone'>";

                    while (dr_branch.Read())
                    {
                        branchesDropdown += "<option value='" + dr_branch.GetString(0) + "'>" + dr_branch.GetString(1) + "</option >";
                        zonesDropdown += "<option  value='" + dr_branch.GetString(2) + "'>" + dr_branch.GetString(3) + "</option >";
                        SessionRegion = dr_branch.GetString(4);
                    }
                    branchesDropdown += "</select >";
                    zonesDropdown += "</select >";
                    dr_branch.Close();

                }

                sql = @" SELECT YEAR,[Month],Date,DAY,Flag,Holiday FROM MNP_TargetCalendar WHERE [Year]='" + data.Year_ddl + "' AND [Month]='" + data.Month_ddl + "'";
                using (var cmd2 = new SqlCommand(sql, conn))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();
                    tableRow = "<td class='sticky-col first-col'> " + rowcount.ToString() + "</ td>" +
                        "<td >" + SessionRegion + " </td>" +
                        "<td><input type='text' placeholder='AccountNo' value='' class='AccountNo'   style='display:none'/>Zone:" + zonesDropdown + " branch:" + branchesDropdown + "</td>" +
                        "<td class='sticky-col second-col'><input type='text' placeholder='Enter Customer' class='Customer' id='CustomerTxtbox'/></td>" +
                        "<td class=''>NEW</td>" +
                        "<td width='10%' class='Category' style='display:none'>2</td>" +
                        "<td>0</td>" +
                        "<td>0</td>" +
                        "<td>0</td>" +
                        "<td>0</td>" +
                        "<td>0</td>" +
                        "<td><input type='number' placeholder='Expected Revenue' class='ExpectedRevenue' onkeypress='return isNumberKey(event);' /></td>" +
                        "<td class='VisitCount'>0</td>";

                    rowcount = --rowcount;
                    while (dr.Read())
                    {
                        if (dr.GetInt32(4) == 0)
                        {
                            if (dr.GetDateTime(2) <= System.DateTime.Now.Date)
                            {
                                SecondHead += "<td class='scroll' style='background-color:lightgreen' width='10%'><input type='checkbox' onclick='CountSelectedDates(this)'  disabled='disabled'  id='SaleDate' name='SaleDate'   class='" + rowcount + "     " + dr.GetDateTime(2) + "'/></ td>";
                            }
                            else
                            {
                                SecondHead += "<td class='scroll'  style='background-color:lightgreen' width='10%'><input type='checkbox' onclick='CountSelectedDates(this)'  id='SaleDate' name='SaleDate'   class='" + rowcount + " " + dr.GetDateTime(2) + "'/></ td>";
                            }
                        }
                        else
                        {
                            if (dr.GetDateTime(2) <= System.DateTime.Now.Date)
                            {
                                SecondHead += "<td class='scroll' width='10%'><input type='checkbox' onclick='CountSelectedDates(this)'  disabled='disabled'  id='SaleDate' name='SaleDate'   class='" + rowcount + " " + dr.GetDateTime(2) + "'/></ td>";
                            }
                            else
                            {
                                SecondHead += "<td class='scroll'  width='10%'><input type='checkbox' onclick='CountSelectedDates(this)'    name='SaleDate'   class='" + rowcount + " " + dr.GetDateTime(2) + "'/></ td>";
                            }
                        }
                        rowCounter++;
                    }
                    tableRow += SecondHead + "";
                }
                return tableRow;
            }
            catch (Exception er)
            {
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        [WebMethod]
        public static string SaveCustomerSheet(SelectedCustomers data)
        {
            data.salesPerson = HttpContext.Current.Session["SalesPersonSelected"].ToString();

            using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString)))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                transaction = conn.BeginTransaction("Insert discount entries");
                command.Connection = conn;
                command.Transaction = transaction;
                try
                {
                    String sql = "Delete FROM MNP_ScheduleVisit WHERE StaffId='" + data.salesPerson + "' AND MONTH(date)>=MONTH(GETDATE()) AND YEAR(date)=YEAR(GETDATE())  AND CAST(date AS date) >= GETDATE()";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                    if (data.SaleDate.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Id", typeof(Int64));
                        dt.Columns.Add("staffId", typeof(Int64));
                        dt.Columns.Add("Date", typeof(DateTime));
                        dt.Columns.Add("Zone", typeof(string));
                        dt.Columns.Add("Branch", typeof(string));
                        dt.Columns.Add("AccountNo", typeof(string));
                        dt.Columns.Add("CustomerName", typeof(string));
                        dt.Columns.Add("ScheduleVisit", typeof(int));
                        dt.Columns.Add("Category", typeof(int));
                        dt.Columns.Add("Status", typeof(int));
                        dt.Columns.Add("CreatedBy", typeof(string));
                        dt.Columns.Add("Creat   edOn", typeof(DateTime));
                        dt.Columns.Add("ExpectedRevenue", typeof(float));

                        DataRow dr;
                        for (int j = 0; j < data.Customer.Count; j++)
                        {
                            if (DateTime.Parse(data.SaleDate[j]) > System.DateTime.Now.Date)
                            {
                                dr = dt.NewRow();
                                dr["staffId"] = data.salesPerson;
                                dr["AccountNo"] = data.AccountNo[j];
                                dr["Date"] = data.SaleDate[j];
                                dr["Zone"] = data.Zones[j];
                                dr["Branch"] = data.Branches[j];
                                dr["CustomerName"] = data.Customer[j];
                                dr["ScheduleVisit"] = 1;
                                dr["Category"] = data.Category[j];
                                dr["Status"] = 1;
                                dr["CreatedBy"] = HttpContext.Current.Session["U_ID"].ToString();
                                dr["CreatedOn"] = System.DateTime.Now;
                                dr["ExpectedRevenue"] = data.ExpectedRevenue[j];
                                dt.Rows.Add(dr);
                            }
                        }

                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                        {
                            sqlBulkCopy.DestinationTableName = "MNP_ScheduleVisit";
                            sqlBulkCopy.WriteToServer(dt);
                        }
                    }
                    transaction.Commit();
                    return "Schedule saved successfully";
                }

                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        return "Failed to save schedule, Please contact IT for support.";

                    }
                    catch (Exception ex2)
                    {
                        return "Failed to save schedule rollback, Please contact IT for support.";
                    }
                }
                finally
                {
                }
            }
        }

        [WebMethod]
        public static string CopyToNextMonth()
        {
            string salesPerson = HttpContext.Current.Session["SalesPersonSelected"].ToString();

            try
            {

                return "Schedule saved successfully";
            }
            catch (Exception ex)
            {
                return "Failed to save schedule, Please contact IT for support.";
            }
            finally
            {
            }
        }
    }


    public class SelectedCustomers
    {
        public List<String> Zones { get; set; }
        public List<String> Branches { get; set; }
        public List<String> AccountNo { get; set; }
        public List<String> Customer { get; set; }
        public List<String> Category { get; set; }
        public List<String> SaleDate { get; set; }
        public List<float> ExpectedRevenue { get; set; }
        public string salesPerson { get; set; }
    }

    public class DropdownSalesPerson
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
    public class SearchFilter
    {
        public string Year_ddl { get; set; }
        public string Month_ddl { get; set; }
        public string salesPerson_ddl { get; set; }
        public string tableRows { get; set; }

    }
}