using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace MRaabta.Repo
{
    public class CurrentDateAllRiderDB
    {
        SqlConnection orcl;
        public CurrentDateAllRiderDB()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await orcl.OpenAsync();
        }
        public void Close()
        {
            orcl.Close();
        }

        //Get riders according to the date selected
        public async Task<List<DropDownModel>> GetRiders(string SDate, string EDate, string branchcode)
        {
            try
            {
                string sql = @"SELECT
                            DISTINCT Appdt.RIDERCODE as Value  ,
                            (Select Top(1) userName from App_Users where ridercode=Appdt.riderCode AND branchCode = @bc) as Text
                            FROM   App_Delivery_ConsignmentData Appdt
                            inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                            WHERE R.BRANCHCODE = '" + branchcode + "'  AND cast(R.RUNSHEETDATE as date) between '" + SDate + "' and '" + EDate + "'";
                var rs = await orcl.QueryAsync<DropDownModel>(sql, new { @SDate = SDate, @EDate = EDate, @bc = branchcode });
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //All Records and Specific Record query
        public DataTable getRecords(string riderCode, string SDate, string EDate, string branchCode)
        {
            DataTable ds = new DataTable();
            try
            {

                if (riderCode.Split(',').Contains("0"))
                {
                    string sql = @"select rr.routeCode,appdt.riderCode as RiderCode,
                    (select count(runsheetNumber) from Runsheet r  
                    inner join 
                    (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber where 
                    r.routeCode=rr.routeCode and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' ) as DownloadedRunsheet,
                    (select count(runsheetNumber) from RUNSHEET r where 
                    cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' and branchCode = '" + branchCode + @"' and r.routeCode=rr.routeCode) as TotalRunsheet,
                    (select  TOP(1) au.userName from App_Users au inner join Riders r on au.riderCode = r.riderCode and r.routeCode = rr.routeCode) as RiderName,
                    (select name from Branches where branchCode = '" + branchCode + @"') as Location,
                    (select top 1 name as routeName from routes rou where rou.routeCode = rr.routeCode and rou.BID = '" + branchCode + @"') as RiderRoute,
                    (select count(consignmentNumber) from RunsheetConsignment rc inner join Runsheet r on r.runsheetNumber = rc.runsheetNumber 
                    where cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' and r.branchCode = '" + branchCode + @"' and r.routeCode=rr.routeCode) as TotalCN,
                    (select COUNT(consignmentnumber) from RunsheetConsignment  rc 
                    INNER JOIN Runsheet r on rc.runsheetNumber = r.runsheetNumber 
                    inner join (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.RUNSHEETNUMBER  and rc.branchcode = r.branchCode AND rc.RouteCode = r.routeCode
                    WHERE cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' and r.routeCode=rr.routeCode ) as TCNDownloaded,
                    SUM(case when appdt.ConsignmentNumber is not null and(appdt.relation is null or appdt.relation = '') then 0 else 1 end) delivered,
                    SUM(case when appdt.ConsignmentNumber is not null and(appdt.reason is null or appdt.reason = '') then 0 else 1 end) undelivered,
                    (
                    SELECT COUNT(xb.Id)
                    FROM   ( SELECT ROW_NUMBER() OVER(ORDER BY CAST(m.created_on AS date)) AS Id ,CAST(FORMAT(created_on ,'HH:mm') AS TIME) AS [Created_on Time]  
                    ,' '+picker_name+ ' '+m.reason+' '+LEFT( CAST(CAST(FORMAT(created_on ,'HH:mm') AS TIME) AS VARCHAR(MAX)) ,5 )                 AS Details
                    ,m.RunSheetNumber  AS Runsheet ,m.relation ,latitude          AS Lat ,longitude         AS Long ,picker_name       AS NAME ,m.reason            AS Reason  ,1                 AS IsMarker
                    ,(
                    SELECT TOP(1) ConsignmentNumber
                    FROM   App_Delivery_ConsignmentData ab
                    WHERE  ab.RunSheetNumber = m.RunSheetNumber
                    AND CAST(FORMAT(ab.created_on ,'HH:mm') AS TIME) = CAST(FORMAT(ab.created_on ,'HH:mm') AS TIME)
                    AND ab.picker_name = m.picker_name
                    AND ab.reason = m.reason
                    AND ab.longitude = m.longitude
                    AND ab.latitude = m.latitude
                    )                 AS consignmentNumber

                    FROM   [App_Delivery_ConsignmentData] m
                    INNER JOIN Runsheet R
                    ON  r.runsheetNumber = m.RunSheetNumber
                    INNER JOIN RunsheetConsignment rc ON
                    r.runsheetNumber = rc.runsheetNumber
                    WHERE r.routeCode = RR.routeCode and R.BRANCHCODE = '" + branchCode + @"'
                    AND CAST(R.RUNSHEETDATE AS date) BETWEEN '" + SDate + @"' and '" + EDate + @"'
                    GROUP BY CAST(m.created_on AS date) ,m.riderCode ,m.RunSheetNumber ,m.relation,latitude ,longitude,picker_name,m.reason,CAST(FORMAT(m.created_on ,'HH:mm') AS TIME)) AS xb) as [Touchpoints],
                    (
                    select 
                    LEFT(cast(max(cast(cast(xb.[Created_on Time] as time) as datetime)) -  min(cast(cast(xb.[Created_on Time] as time) as datetime)) as time),5)
                    from
                    (select row_number() over(order by  CAST(created_on AS date)) as Id, 
                    CAST(format(created_on,'HH:mm') as time) as [Created_on Time],' ' 
                    +picker_name+ ' ' +reason + ' ' + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details,
                    m.RunSheetNumber as Runsheet, relation, latitude as Lat,longitude as Long, picker_name as name,
                    reason as Reason, 1 as IsMarker, (Select Top(1) ConsignmentNumber from App_Delivery_ConsignmentData ab 
                    where ab.RunSheetNumber = m.RunSheetNumber and cast(format(ab.created_on,'HH:mm') as time) = cast(format(ab.created_on,'HH:mm') as time) 
                    and ab.picker_name = m.picker_name and  ab.reason = m.reason and ab.longitude = m.longitude and ab.latitude = m.latitude) as consignmentNumber
                    from [App_Delivery_ConsignmentData] m 
                    inner join Runsheet r on r.runsheetNumber = m.runsheetNumber
                    where r.routeCode=RR.routeCode AND CAST(created_on as date) between '" + SDate + @"' and '" + EDate + @"'
                    group by cast(created_on as date),m.riderCode, m.RunSheetNumber, m.relation,m.latitude, m.longitude, m.picker_name,m.reason, CAST(format(created_on,'HH:mm') as time) 
                    ) as xb) as TotalTimeTaken,
                    (
                    SELECT TOP 1 RC.NAME + ' -- ' + R.ROUTECODE + ' | ' + isnull(RP.Master_Route_Name, ' - ') + ' -- ' + isnull(RP.Master_Route_Code,' - ') ROUTE_MASTER 
                    FROM   RUNSHEET R
                    LEFT OUTER JOIN ROUTES RC ON  rc.BID = r.branchCode AND RC.ROUTECODE = R.ROUTECODE
                    LEFT JOIN Route_Profile_Master RP ON  rp.Master_Route_Code = rc.RouteTerritory AND rp.BranchCode = rc.BID
                    WHERE R.routeCode=RR.routeCode 
                    AND cast(R.RUNSHEETDATE as date) between '" + SDate + @"' and '" + EDate + @"')
                    as Route
                    from App_Delivery_ConsignmentData appdt
                    inner join runsheet RR on RR.runsheetNumber = appdt.RunSheetNumber
                    where rr.routeCode=rr.routeCode and cast(RR.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"'  and branchCode = '" + branchCode + @"'";
                    sql += "group by  rr.routeCode,appdt.riderCode";

                    orcl.Open();
                    SqlCommand orcd = new SqlCommand(sql, orcl);
                    orcd.CommandType = CommandType.Text;
                    orcd.CommandTimeout = 5000;
                    //
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    oda.Fill(ds);
                    orcl.Close();

                }
                else
                {
                    riderCode = riderCode.Replace(",", "','");
                    string sql = @"select appdt.riderCode as RiderCode,rr.routecode,
                    (select count(runsheetNumber) from Runsheet r  
                    inner join 
                    (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber where r.routeCode =  RR.routeCode 
                    and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"') as DownloadedRunsheet,

                    (select count(runsheetNumber) from RUNSHEET r where r.routeCode =  RR.routeCode and 
                    cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' and branchCode = '" + branchCode + @"') as TotalRunsheet,

                    (select  TOP(1) au.userName from App_Users au inner join Riders r on au.riderCode = r.ridercode where r.routeCode =  RR.routeCode) as RiderName,

                    (select name from Branches where branchCode = '" + branchCode + @"') as Location,

                    (select top 1 name as routeName from routes rou where rou.routeCode = RR.routeCode and rou.BID = '" + branchCode + @"') as RiderRoute,

                    (select count(consignmentNumber) from RunsheetConsignment rc inner join Runsheet r on r.runsheetNumber = rc.runsheetNumber 
                    where r.routeCode =  RR.routeCode and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' and r.branchCode = '" + branchCode + @"') as TotalCN,

                    (select COUNT(consignmentnumber) from RunsheetConsignment  rc 
                    INNER JOIN Runsheet r on rc.runsheetNumber = r.runsheetNumber 
                    inner join (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.RUNSHEETNUMBER  and rc.branchcode = r.branchCode AND rc.RouteCode = r.routeCode
                    WHERE R.routeCode =  RR.routeCode  and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"') as TCNDownloaded,

                    SUM(case when appdt.ConsignmentNumber is not null and(appdt.relation is null or appdt.relation = '') then 0 else 1 end) delivered,
                    SUM(case when appdt.ConsignmentNumber is not null and(appdt.reason is null or appdt.reason = '') then 0 else 1 end) undelivered,

                    (select count(xb.Id) from
                    (select row_number() over(order by  CAST(created_on AS date)) as Id, 
                    CAST(format(created_on,'HH:mm') as time) as [Created_on Time],' ' +picker_name+ 
                    ' ' +reason + ' ' + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details, 
                    m.RunSheetNumber as Runsheet, relation, latitude as Lat,longitude as Long, picker_name as name,
                    reason as Reason, 1 as IsMarker, (Select Top(1) ConsignmentNumber from App_Delivery_ConsignmentData ab 
                    where ab.RunSheetNumber = m.RunSheetNumber and cast(format(ab.created_on,'HH:mm') as time) = cast(format(ab.created_on,'HH:mm') as time) 
                    and ab.picker_name = m.picker_name and  ab.reason = m.reason and ab.longitude = m.longitude and ab.latitude = m.latitude) as consignmentNumber
                    from [App_Delivery_ConsignmentData] m inner join Runsheet r on r.runsheetNumber = m.RunSheetNumber
                    where r.routeCode = RR.routeCode AND CAST(created_on as date) between '" + SDate + @"' and '" + EDate + @"' group by cast(created_on as date),m.riderCode,
                    m.RunSheetNumber, m.relation,m.latitude, m.longitude, m.picker_name, m.relation, m.reason, CAST(format(m.created_on,'HH:mm') as time)  ) as xb) as [Touchpoints],
                    (
                    select 
                    LEFT(cast(max(cast(cast(xb.[Created_on Time] as time) as datetime)) -  min(cast(cast(xb.[Created_on Time] as time) as datetime)) as time),5)
                    from
                    (select row_number() over(order by  CAST(created_on AS date)) as Id, 
                    CAST(format(created_on,'HH:mm') as time) as [Created_on Time],' ' 
                    +picker_name+ ' ' +reason + ' ' + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details,
                    m.RunSheetNumber as Runsheet, relation, latitude as Lat,longitude as Long, picker_name as name,
                    reason as Reason, 1 as IsMarker, (Select Top(1) ConsignmentNumber from App_Delivery_ConsignmentData ab 
                    where ab.RunSheetNumber = m.RunSheetNumber and cast(format(ab.created_on,'HH:mm') as time) = cast(format(ab.created_on,'HH:mm') as time) 
                    and ab.picker_name = m.picker_name and  ab.reason = m.reason and ab.longitude = m.longitude and ab.latitude = m.latitude) as consignmentNumber
                    from [App_Delivery_ConsignmentData] m 
                    inner join Runsheet r on r.runsheetNumber = m.runsheetNumber
                    where r.routeCode=RR.routeCode AND CAST(created_on as date) between '" + SDate + @"' and '" + EDate + @"'
                    group by cast(created_on as date),m.riderCode, m.RunSheetNumber, m.relation,m.latitude, m.longitude, m.picker_name,m.reason, CAST(format(created_on,'HH:mm') as time) 
                    ) as xb) as TotalTimeTaken
                    from App_Delivery_ConsignmentData appdt
                    inner join runsheet RR on RR.runsheetNumber = appdt.RunSheetNumber
                    where RR.routeCode in(select routeCode from Riders r where r.riderCode in('" + riderCode + @"') and branchId = '" + branchCode + @"') and cast(RR.runsheetDate as date)
                    between '" + SDate + @"' and '" + EDate + @"'";
                    sql += "group by RR.routecode, appdt.riderCode";
                    orcl.Open();
                    SqlCommand orcd = new SqlCommand(sql, orcl);
                    orcd.CommandType = CommandType.Text;
                    orcd.CommandTimeout = 5000;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    oda.Fill(ds);

                }
            }
            catch (Exception Err)
            {
                Err.Message.ToString();
            }
            finally { orcl.Close(); }
            return ds;
        }

        public async Task<List<Status>> TotalReasonPoints(string ridercode, string SDate, string EDate, string branchcode)
        {
            try
            {
                orcl.Open();

                if (ridercode.Split(',').Contains("0"))
                {
                    var rs = await orcl.QueryAsync<Status>(@"select
                    (select count(r.runsheetNumber) from Runsheet r inner join (select distinct RunsheetNo from 
                    App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber where r.routeCode in
                    (select routeCode from Riders where riderCode in(SELECT DISTINCT Appdt.RIDERCODE as Value  
                    FROM   App_Delivery_ConsignmentData Appdt inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber
                    WHERE R.BRANCHCODE = '" + branchcode + @"'  AND cast(R.RUNSHEETDATE as date) between '" + SDate + @"' and '" + EDate + @"') and branchId='" + branchcode + @"')
                    and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"') as DownloadedRunsheet,
                    (select count(runsheetNumber) from RUNSHEET r where r.routeCode in(select routeCode from Riders
                    where riderCode in(SELECT DISTINCT Appdt.RIDERCODE as Value  FROM   App_Delivery_ConsignmentData Appdt inner join 
                    Runsheet R on R.runsheetNumber = appdt.RunSheetNumber WHERE R.BRANCHCODE = '" + branchcode + @"'  AND 
                    cast(R.RUNSHEETDATE as date) between '" + SDate + @"' and '" + EDate + @"') and branchId='" + branchcode + @"') and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' and branchCode = '" + branchcode + @"') as TotalRunsheet,
                    (select count(consignmentNumber) from RunsheetConsignment rc inner join Runsheet r on r.runsheetNumber = rc.runsheetNumber 
                    where r.routeCode in(select routeCode from Riders
                    where riderCode in(SELECT DISTINCT Appdt.RIDERCODE as Value  FROM   App_Delivery_ConsignmentData Appdt inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber WHERE R.BRANCHCODE = '" + branchcode + @"'  AND cast(R.RUNSHEETDATE as date) between '" + SDate + @"' and '" + EDate + @"') and branchId='" + branchcode + @"') and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"'
                    and r.branchCode = '" + branchcode + @"') as TotalCN,
                    (select COUNT(consignmentnumber) from RunsheetConsignment rc 
                    INNER JOIN Runsheet r on rc.runsheetNumber = r.runsheetNumber and rc.branchcode = r.branchCode AND rc.RouteCode = r.routeCode
                    inner join (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = R.RUNSHEETNUMBER
                    WHERE r.routeCode in(select routeCode from Riders
                    where riderCode in(SELECT DISTINCT Appdt.RIDERCODE as Value  FROM   App_Delivery_ConsignmentData Appdt inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber WHERE R.BRANCHCODE = '" + branchcode + @"'  AND cast(R.RUNSHEETDATE as date) between '" + SDate + @"' and '" + EDate + @"') and branchId='" + branchcode + @"') and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"') as TCNDownloaded,
                    SUM(case when appdt.ConsignmentNumber is not null and(appdt.relation is null or appdt.relation = '') then 0 else 1 end) delivered,
                    SUM(case when appdt.ConsignmentNumber is not null and(appdt.reason is null or appdt.reason = '') then 0 else 1 end) undelivered,
                    (    SELECT COUNT(xb.Id)
                    FROM   (
                    SELECT ROW_NUMBER() OVER(ORDER BY CAST(m.created_on AS date)) AS Id  ,CAST(FORMAT(created_on ,'HH:mm') AS TIME) AS  [Created_on Time] ,' '+picker_name+ ' '+m.reason+' '+LEFT(
                    CAST(CAST(FORMAT(created_on ,'HH:mm') AS TIME) AS VARCHAR(MAX)),5 ) AS Details
                    ,m.RunSheetNumber  AS Runsheet  ,m.relation ,latitude          AS Lat ,longitude         AS Long ,picker_name       AS NAME,m.reason            AS Reason,1                 AS IsMarker
                    ,(
                    SELECT TOP(1) ConsignmentNumber
                    FROM   App_Delivery_ConsignmentData ab
                    WHERE  ab.RunSheetNumber = m.RunSheetNumber
                    AND CAST(FORMAT(ab.created_on ,'HH:mm') AS TIME) =
                    CAST(FORMAT(ab.created_on ,'HH:mm') AS TIME)
                    AND ab.picker_name = m.picker_name
                    AND ab.reason = m.reason
                    AND ab.longitude = m.longitude
                    AND ab.latitude = m.latitude
                    )                 AS consignmentNumber

                    FROM   [App_Delivery_ConsignmentData] m
                    INNER JOIN Runsheet R
                    ON  r.runsheetNumber = m.RunSheetNumber
                    INNER JOIN RunsheetConsignment rc ON
                    r.runsheetNumber = rc.runsheetNumber
                    WHERE  R.BRANCHCODE = '" + branchcode + @"'
                    AND CAST(R.RUNSHEETDATE AS date) BETWEEN '" + SDate + @"' and '" + EDate + @"'
                    GROUP BY
                    CAST(m.created_on AS date)
                    ,m.riderCode
                    ,m.RunSheetNumber
                    ,m.relation
                    ,latitude
                    ,longitude
                    ,picker_name
                    ,m.reason
                    ,CAST(FORMAT(m.created_on ,'HH:mm') AS TIME)
                    ) AS xb
                    ) as [Touchpoints]
                    from App_Delivery_ConsignmentData appdt
                    inner join runsheet rr on rr.runsheetNumber = appdt.RunSheetNumber
                    where rr.routeCode in(select routeCode from Riders
                    where riderCode in(SELECT DISTINCT Appdt.RIDERCODE as Value  FROM   App_Delivery_ConsignmentData Appdt inner join Runsheet R on R.runsheetNumber = appdt.RunSheetNumber WHERE R.BRANCHCODE = '" + branchcode + @"'  AND cast(R.RUNSHEETDATE as date) between '" + SDate + @"' and '" + EDate + @"') and branchId='" + branchcode + @"') 
                    and cast(rr.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"'",
                    new { @rc = ridercode, @sd = SDate, @ed = EDate, @bcode = branchcode }, commandTimeout: 5000);
                    orcl.Close();
                    return rs.ToList();
                }
                else
                {
                    ridercode = ridercode.Replace(",", "','");
                    var rs = orcl.Query<Status>(@"select
                        (select count(runsheetNumber) from Runsheet r  
                        inner join 
                        (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = r.runsheetNumber
                        where R.routeCode in(select routeCode from Riders where riderCode in('" + ridercode + @"')  and branchId = '" + branchcode + @"') and cast(r.runsheetDate as date) 
                        between '" + SDate + @"' and '" + EDate + @"') as DownloadedRunsheet,
                        (select count(runsheetNumber) from RUNSHEET r where  R.routeCode in(select routeCode from Riders where riderCode in('" + ridercode + @"')  and branchId = '" + branchcode + @"') and 
                        cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' and branchCode = '" + branchcode + @"') as TotalRunsheet,
                        (select count(consignmentNumber) from RunsheetConsignment rc inner join Runsheet r on r.runsheetNumber = rc.runsheetNumber 
                        where R.routeCode in(select routeCode from Riders where riderCode in('" + ridercode + @"')  and branchId =  '" + branchcode + @"') and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"' 
                        and r.branchCode = '" + branchcode + @"') as TotalCN,
                        (select COUNT(consignmentnumber) from RunsheetConsignment rc 
                        INNER JOIN Runsheet r on rc.runsheetNumber = r.runsheetNumber and rc.branchcode = r.branchCode AND rc.RouteCode = r.routeCode
                        inner join (select distinct RunsheetNo from App_Delivery_RunsheetFetched) ar on ar.runsheetno = R.RUNSHEETNUMBER
                        WHERE  R.routeCode in(select routeCode from Riders where  riderCode in('" + ridercode + @"')  and branchId = '" + branchcode + @"') and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"') as TCNDownloaded,
                        SUM(case when appdt.ConsignmentNumber is not null and(appdt.relation is null or appdt.relation = '') then 0 else 1 end) delivered,
                        SUM(case when appdt.ConsignmentNumber is not null and(appdt.reason is null or appdt.reason = '') then 0 else 1 end) undelivered,
                        (select count(xb.Id) from
                        (select row_number() over(order by  CAST(created_on AS date)) as Id, 
                        CAST(format(created_on,'HH:mm') as time) as [Created_on Time],' ' +picker_name+ 
                        ' ' +reason + ' ' + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details, 
                        m.RunSheetNumber as Runsheet, relation, latitude as Lat,longitude as Long, picker_name as name,
                        reason as Reason, 1 as IsMarker, (Select Top(1) ConsignmentNumber from App_Delivery_ConsignmentData ab 
                        where ab.RunSheetNumber = m.RunSheetNumber and cast(format(ab.created_on,'HH:mm') as time) = cast(format(ab.created_on,'HH:mm') as time) 
                        and ab.picker_name = m.picker_name and  ab.reason = m.reason and ab.longitude = m.longitude and ab.latitude = m.latitude) as consignmentNumber
                        from [App_Delivery_ConsignmentData] m 
                        inner join Runsheet R on R.runsheetNumber = m.RunSheetNumber
                        where R.routeCode in(select routeCode from Riders
                        where riderCode in('" + ridercode + @"')  and branchId = '" + branchcode + @"') AND CAST(created_on as date) between '" + SDate + @"' and '" + EDate + @"' group by cast(created_on as date),
                        m.riderCode,
                        m.RunSheetNumber, relation,latitude, longitude, picker_name, relation, reason, CAST(format(created_on,'HH:mm') as time)  ) as xb) as [Touchpoints],
                        (
                        select 
                        LEFT(cast(max(cast(cast(xb.[Created_on Time] as time) as datetime)) -  min(cast(cast(xb.[Created_on Time] as time) as datetime)) as time),5)
                        from
                        (select row_number() over(order by  CAST(created_on AS date)) as Id, 
                        CAST(format(created_on,'HH:mm') as time) as [Created_on Time],' ' 
                        +picker_name+ ' ' +reason + ' ' + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details,
                        m.RunSheetNumber as Runsheet, relation, latitude as Lat,longitude as Long, picker_name as name,
                        reason as Reason, 1 as IsMarker, (Select Top(1) ConsignmentNumber from App_Delivery_ConsignmentData ab 
                        where ab.RunSheetNumber = m.RunSheetNumber and cast(format(ab.created_on,'HH:mm') as time) = cast(format(ab.created_on,'HH:mm') as time) 
                        and ab.picker_name = m.picker_name and  ab.reason = m.reason and ab.longitude = m.longitude and ab.latitude = m.latitude) as consignmentNumber
                        from [App_Delivery_ConsignmentData] m 
                        inner join Runsheet R on r.runsheetNumber = m.RunSheetNumber
                        where R.routeCode in(select routeCode from Riders
                        where riderCode in('" + ridercode + @"')  and branchId = '" + branchcode + @"') AND CAST(created_on as date) between '" + SDate + @"' and '" + EDate + @"'
                        group by cast(created_on as date),m.riderCode, m.RunSheetNumber, relation,latitude, longitude, picker_name, relation, reason, CAST(format(created_on,'HH:mm') as time) 
                        ) as xb)  as TotalTimeTaken
                        from App_Delivery_ConsignmentData appdt
                        inner join runsheet r on r.runsheetNumber = appdt.RunSheetNumber
                        where R.routeCode in(select routeCode from Riders
                        where riderCode in('" + ridercode + @"')  and branchId = '" + branchcode + @"') and cast(r.runsheetDate as date) between '" + SDate + @"' and '" + EDate + @"'",
                        new { @rc = ridercode, @sd = SDate, @ed = EDate, @bcode = branchcode }, commandTimeout: 5000);
                    orcl.Close();

                    return rs.ToList();
                }

            }
            catch (SqlException ex)
            {
                orcl.Close();
                return null;
            }
            catch (Exception ex)
            {
                orcl.Close();
                return null;
            }
        }
    }
}