using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MRaabta.Repo
{
    public class DlvRouteRepo
    {
        SqlConnection con;
        public DlvRouteRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            con.Close();
        }


        public async Task<DLVRoute_ListOfTrackingNConsignmentDrop> GetRouteByRider(bool isCN, string rsh, string cn)
        {
            try
            {


                DLVRoute_ListOfTrackingNConsignmentDrop listBoth = new DLVRoute_ListOfTrackingNConsignmentDrop();

                if (isCN)
                {
                    //var rs = await con.QueryAsync<DlvRouteModel>(@"select 0 as id, riderCode as RiderID, latitude as Lat, longitude as Long, 1 as IsMarker,'CONSIGNMENT NO: ' + ConsignmentNumber + ' ' + case when isnull(relation,'') = '' then reason else relation end as pickupDetail,
                    // + RunSheetNumber + ' ' + name + ' ' + ' ' + reason + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details,
                    //'' as accountNo, picker_name as name,''  as locationname, created_on as CreationDate,reason as Reason from [dbo].[App_Delivery_ConsignmentData]
                    //where ConsignmentNumber = @ID AND latitude>0 AND longitude>0;", new { ID });
                    var rs = await con.QueryAsync<DlvRouteModel>(@"select row_number() over(order by CAST(created_on AS date)) as Id, riderCode as RiderID, latitude as Lat, longitude as Long, 1 as IsMarker,'CONSIGNMENT NO: ' + ConsignmentNumber + ' ' + case when isnull(relation,'') = '' then reason else relation end as pickupDetail,
                     + ConsignmentNumber + ' ' + name + ' ' + ' ' + reason + LEFT(CAST(CAST(format(created_on,'HH:mm') as time) as varchar(max)),5) as Details,
                    '' as accountNo, picker_name as name,''  as locationname, performed_on as CreationDate,reason as Reason, ConsignmentNumber from [dbo].[App_Delivery_ConsignmentData]
                    where RunSheetNumber = @rs and ConsignmentNumber = @ID AND latitude > 0 AND longitude > 0;", new { @rs = rsh, @ID = cn });
                    listBoth.consignment = rs.ToList();
                    listBoth.tracking = null;
                    return listBoth;
                }
                else
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    var rs = await con.QueryAsync<DlvRouteModel>(@"select row_number() over(order by  CAST(performed_on AS date)) as Id,
                                                                CAST(format(performed_on,'HH:mm') as time) as [performed_on Time],
                                                                m.riderCode as RiderID, 
                                                                concat(m.RunSheetNumber, ' ' , picker_name , ' ' ,reason , ' ' ,FORMAT(performed_on,'hh:mm tt')) as Details,
                                                                m.RunSheetNumber as Runsheet, relation, 
                                                                (
                                                                CASE WHEN latitude = 0 and r.branchCode = '4' then  24.8354138 
                                                                When latitude = 0 and r.branchCode = '43' then 33.6577612  
                                                                When latitude = 0 and r.branchCode = '34' then 3	
                                                                When latitude = 0 and r.branchCode = '1' then 1 ELSE latitude END
                                                                ) as Lat,
                                                                (
                                                                CASE WHEN longitude = 0 and r.branchCode = '4' then  67.0698366	
                                                                When longitude = 0 and r.branchCode = '43' then 73.0493912 	
                                                                When longitude = 0 and r.branchCode = '34' then 34	
                                                                When longitude = 0 and r.branchCode = '1' then 1 ELSE longitude END
                                                                )
                                                                as Long, picker_name as name, reason as Reason, 1 as IsMarker, 
                                                                '<br />' + STRING_AGG('CONSIGNMENT NO: '+m.ConsignmentNumber,'<br />') as pickupDetail
                                                                from [App_Delivery_ConsignmentData] m 
                                                                inner join Runsheet r on r.runsheetNumber= m.RunSheetNumber 
                                                                where m.RunSheetNumber = @ID
                                                                group by
                                                                m.RunSheetNumber,r.branchCode,m.ridercode,m.performed_on,latitude, longitude, picker_name, relation, reason order by performed_on;", new { @ID = rsh });

                    //string sql_new = "SELECT * FROM   ( " +
                    //        "           SELECT ( CASE WHEN latitude = 0 and r.branchCode = '4' then  24.8354138 When latitude = 0 and r.branchCode = '43' then 33.6577612 When latitude = 0 and r.branchCode = '34' then 3  When latitude = 0 and r.branchCode = '1' then 1 ELSE latitude END) as latitude, (  CASE WHEN longitude = 0 and r.branchCode = '4' then  67.0698366 When longitude = 0 and r.branchCode = '43' then 73.0493912 When longitude = 0 and r.branchCode = '34' then 3 When longitude = 0 and r.branchCode = '1' then 1 ELSE longitude END) as longitude, MAX(created_on) logTime " +
                    //        "             FROM   App_Delivery_ConsignmentData adcd inner join Runsheet r on r.runsheetNumber = adcd.RunSheetNumber WHERE adcd.RunSheetNumber = @runsheetID " +
                    //        "           GROUP BY  latitude,longitude,r.branchCode  " +
                    //        "           UNION ALL " +
                    //        "           SELECT  (  CASE WHEN arl.latitude = 0 and au.branchCode = '4' then  4	When arl.latitude = 0 and au.branchCode = '43' then 33.6577612 When arl.latitude = 0 and au.branchCode = '34' then 3   When arl.latitude = 0 and au.branchCode = '1' then 1 ELSE arl.latitude END) as latitude, ( 	 CASE WHEN arl.longitude = 0 and au.branchCode = '4' then  67.0698366 When arl.longitude = 0 and au.branchCode = '43' then 73.0493912 When arl.longitude = 0 and au.branchCode = '34' then 3 When arl.longitude = 0 and au.branchCode = '1' then 1  ELSE arl.longitude END) as longitude ,MAX(arl.logTime) logTime " +
                    //        "           FROM[App_RidersLocation] arl INNER JOIN App_Users au ON  au.riderCode = @ridercode " +
                    //        "           WHERE CAST(arl.logTime AS date) = CAST(@Datee AS date) " +
                    //        "                  AND arl.[USER_ID] = au.[USER_ID] GROUP BY arl.latitude, arl.longitude, au.branchCode ) k" +
                    //        "          ORDER BY    k.logTime";

                    //var rs_tracking = await con.QueryAsync<DLVRoute_LogTrackingModel>(sql_new, new { @runsheetID = rsh, @Datee = DateTime.Now.ToString("yyyy-MM-dd") });
                    ////listBoth.tracking = rs_tracking.ToList();
                    listBoth.consignment = rs.ToList();
                    return listBoth;

                }
                //var rs = await con.QueryAsync<RiderRouteModel>(@"SELECT * FROM RiderRoutes where RiderId = @RiderId and CONVERT(date, CreationDate) = CONVERT(date,@Date);", new { RiderId, Date });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<DropDownModel>> GetRiders()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>(@"	SELECT u.USER_ID as [Value], (r.riderCode + '-' + r.firstName + ' '+ r.lastName) as [Text] from riders  r 
	                    inner join App_Users u on u.riderCode = r.riderCode where r.branchId = '4'  and r.riderCode not like ' %'
	                    union
	                    select '0' as [Value], 'All' as [Text] from [dbo].[App_Users]
	
	                    order by Value");
                return rs.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}