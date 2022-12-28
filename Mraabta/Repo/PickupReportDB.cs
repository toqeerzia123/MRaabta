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
    public class PickupReportDB
    {
        SqlConnection orcl;

        public PickupReportDB()
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

        //All Records and Specific Record query
        public List<PickupReportModel> GetPickups(string Status, string SDate, string EDate)
        {
            try
            {
                orcl.Open();
                var rs = orcl.Query<PickupReportModel>(@"select c.ticketNumber,ISNULL(c.consignmentNumber, '-') as consignmentNumber, c.serviceTypeName as ServiceType,
concat(c.RiderCode,' - ',r.firstName) as rider, c.totalAmount as Amount,
case when ((c.callstatus is null or c.callstatus = 1 ) and c.consignmentNumber is null) then 'Pending'
when ((c.callStatus = 2 or c.callStatus = 3) and c.consignmentNumber is null ) then 'InProcess'
when ( c.consignmentNumber is not null) then 'Performed'
when (c.callStatus = 5 and c.consignmentNumber is null) then 'Cancelled' end as Status,b.name as origin ,b2.name as destination
from MNP_PreBookingConsignment c
left join Riders r on r.riderCode = c.RiderCode
left join Branches b on b.branchCode = c.orgin
left join Branches b2 on b2.branchCode = c.destination
left join MNP_NCI_CallStatus cs on cs.Id = c.callStatus
left join MNP_NCI_Reasons rs on c.Reason = rs.Id 
left join ExpressCenters ex on ex.expressCenterCode = c.expressCenterCode
where isMobile = '1' and cast(c.createdOn as date) between '" + SDate + "' and '" + EDate + "'");
                orcl.Close();
                return rs.ToList();
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

        public List<PickupTicketModel> GetRecord(string ticketnumber)
        {
            try
            {
                orcl.Open();
                var rs = orcl.Query<PickupTicketModel>(@" select c.ticketNumber,ISNULL(c.consignmentNumber, '-') as consignmentNumber, c.serviceTypeName as ServiceType,ISNULL(cs.NAME, '-') as CallStatus,ISNULL(rs.Name , '-')as Reason,
concat(c.RiderCode,' - ',r.firstName) as rider, c.totalAmount as Amount,FORMAT(c.pickupScheduled, 'yyyy-MM-dd') as PickupDate ,
CONVERT(VARCHAR(5),c.pickupTime,108) as PickupTime , c.pieces , c.WEIGHT as Weight,
c.consignee,c.consigneeAddress,c.consigneeCellNo,
c.consigner,c.consigneraddress,c.consignerCellNo,
case when ((c.callstatus is null or c.callstatus = 1 ) and c.consignmentNumber is null) then 'Pending'
when ((c.callStatus = 2 or c.callStatus = 3) and c.consignmentNumber is null ) then 'InProcess'
when (c.callStatus = 4 and c.consignmentNumber is not null) then 'Performed'
when (c.callStatus = 5 and c.consignmentNumber is null) then 'Cancelled' end as Status,b.name as origin ,b2.name as destination,
ca.longitude , ca.latitude
from MNP_PreBookingConsignment c
left join Riders r on r.riderCode = c.RiderCode
left join Branches b on b.branchCode = c.orgin
left join Branches b2 on b2.branchCode = c.destination
left join MNP_PreBookingCallStatus cs on cs.Id = c.callStatus
left join MNP_PreBookingReason rs on c.Reason = rs.Id 
left join ExpressCenters ex on ex.expressCenterCode = c.expressCenterCode
left join CustomerApp_PickupRequest ca on ca.ticketNumber = c.ticketNumber
where c.isMobile = '1' and c.ticketNumber ='" + ticketnumber + "'");
                orcl.Close();
                return rs.ToList();
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
        public List<PickupReportModel> getcities()
        {
            try
            {
                orcl.Open();
                //string sql = ;
                var rs = orcl.Query<PickupReportModel>(@" select cityName as value from Cities where isActive = 1 order by value asc");
                orcl.Close();
                return rs.ToList();
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
        public List<PickupStats> GetStats(string Status, string SDate, string EDate)
        {
            try
            {
                orcl.Open();
                //string sql = ;
                var rs = orcl.Query<PickupStats>(@" select (select count(c.ticketnumber) from MNP_PreBookingConsignment c where   c.isMobile = '1' 
and cast(c.createdOn as date) between '" + SDate + @"' and '" + EDate + @"' ) as Request ,  
--Pending Today
(select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' 
and (c.callStatus is Null or c.callStatus = 1  ) and cast(c.createdOn as date)between '" + SDate + @"' and '" + EDate + @"' ) as Pending, 
--In-process Today
(select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' 
and ( c.callStatus = 2 or c.callStatus = 3 ) and cast(c.createdOn as date) between '" + SDate + @"' and '" + EDate + @"') as Process, 
--cancel request
(select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' and c.callStatus = '5' 
and cast(c.createdOn as date) between '" + SDate + @"' and '" + EDate + @"') as Cancel, 
--performed request
(select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is not Null and c.isMobile = '1' 
and c.callStatus = 4   and cast(c.createdOn as date) between '" + SDate + @"' and '" + EDate + @"') as Performed");
                orcl.Close();
                return rs.ToList();
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

        // View Revenue


        public List<PickupReportModel> GetAmount(string SDate, string EDate)
        {
            try
            {
                orcl.Open();
                var rs = orcl.Query<PickupReportModel>(@"select c.consignmentNumber, c.chargedAmount as Amount,c.serviceTypeName as ServiceType,concat(c.RiderCode,' - ',r.firstName) as rider
from MNP_PreBookingConsignment c
left join Riders r on c.RiderCode = r.riderCode
where isBooked = '1' and consignmentNumber is not null and isMobile = '1'
 and cast(c.createdOn as date) between '" + SDate + "' and '" + EDate + "'");
                orcl.Close();
                return rs.ToList();
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
        public List<PickupTicketModel> GetAmountStats(string SDate, string EDate)
        {
            try
            {
                orcl.Open();
                var rs = orcl.Query<PickupTicketModel>(@"select sum(c.chargedAmount) as Amount, sum(c.gst) as gst, sum( c.totalAmount ) as totalAmount
from MNP_PreBookingConsignment c
left join Riders r on c.RiderCode = r.riderCode
where isBooked = '1' and consignmentNumber is not null and isMobile = '1'
and cast(c.createdOn as date) between '" + SDate + "' and '" + EDate + "'");
                orcl.Close();
                return rs.ToList();
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
        public List<PickupTicketModel> GetAmountRecord(string consignmentNumber)
        {
            try
            {
                orcl.Open();
                var rs = orcl.Query<PickupTicketModel>(@" select c.chargedAmount as Amount, c.gst,case when c.discountApplied = ' ' then '0' else   c.discountApplied end as discountApplied, 
 ISNULL(c.discountGST, '0') ,b.name as origin ,b2.name as destination ,
c.pieces , c.weight , c.totalAmount 
from MNP_PreBookingConsignment c
left join Branches b on b.branchCode = c.orgin
left join Branches b2 on b2.branchCode = c.destination 
where consignmentNumber ='" + consignmentNumber + "'");
                orcl.Close();
                return rs.ToList();
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

        public async Task<List<PickupRecords>> TotalConsignments()
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                var rs = await orcl.QueryAsync<PickupRecords>(@"
select top 10 xb.ticketNumber, xb.consigner, concat(xb.PickupDate,' ',xb.PickupTime) as scheduledtime, xb.chargedAmount, xb.Status,xb.callStatus  from (
select  c.ticketNumber,c.chargedAmount,c.consigner,cast(c.pickupScheduled as date) as PickupDate, c.createdOn,
CONVERT(VARCHAR(5),c.pickupTime,108) as PickupTime ,
case when ((c.callstatus is null or c.callstatus = 1 ) and c.consignmentNumber is null) then 'Pending'
when ((c.callStatus = 2 or c.callStatus = 3) and c.consignmentNumber is null ) then 'InProcess'
when ( c.callStatus = 4 and c.consignmentNumber is not null) then 'Performed'
when (c.callStatus = 5 and c.consignmentNumber is null) then 'Cancelled' end as Status, c.callStatus
from MNP_PreBookingConsignment c 
where c.isMobile = '1'   and cast(createdOn as date) = cast(GETDATE() as date)
) as xb
order by xb.createdOn desc"
                    );
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<PickupDashboard>> TotalReasonPoints(string branchcode)
        {
            try
            {
                SqlCommand command = orcl.CreateCommand();
                command.CommandTimeout = 300;
                var rs = await orcl.QueryAsync<PickupDashboard>(@"
                select 
                         (Select count(Id) from CustomerApp_Users where status = 1 ) as RegisteredUsers,
                        -- Active users 
                        (select distinct count(Id) from (
                        Select Id from CustomerApp_UserEntryLog where cast(CreatedOn as date) = GETDATE()
                        union
                        Select Customerapp_id as Id from MNP_PreBookingConsignment where cast(CreatedOn as date) = '2020-06-26' and isMobile = '1' ) as Test) as ActiveUsers,
                        --Total Pending Request
                        --(select count(Customerapp_id) as I from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' 
                        --and (c.callStatus is Null or c.callStatus = 1 or c.callStatus = 2 )  ) as TotalPending,

                        --Total Request
                        (select count(c.ticketnumber) from MNP_PreBookingConsignment c where c.isMobile = '1' ) as TotalRequest ,  

                        --Todays's Report
                        --Request Today
                        (select count(c.ticketnumber) from MNP_PreBookingConsignment c where  c.isMobile = '1' 
                        and cast(createdOn as date) = cast(GETDATE() as date) ) as TodayRequest ,  
                        --Pending Today
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' 
                        and (c.callStatus is Null or c.callStatus = 1 ) and cast(createdOn as date) = cast(GETDATE() as date) ) as TodayPending, --GETDATE()
                        --In-process Today
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' 
                        and (c.callStatus = 2 or c.callStatus = 3 ) and cast(createdOn as date) = cast(GETDATE() as date) ) as TodayProcess, --GETDATE()
                        --cancel request
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' and c.callStatus = '5' 
                        and cast(createdOn as date) = cast(GETDATE() as date) ) as TodayCancel, --GETDATE()
                        --performed request
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is not Null and c.isMobile = '1' 
                        and c.callStatus = 4   and cast(createdOn as date) =cast(GETDATE() as date) ) as TodayPerformed, --GETDATE()
                        (select count(c.ticketnumber) from MNP_PreBookingConsignment c where c.isMobile = '1' and 
                         format(createdOn,'MMMM') = FORMAT(GETDATE(), 'MMMM')
                         )as Request , 
                        --Pending Today
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' 
                        and (c.callStatus is Null or c.callStatus = 1  ) and 
                         format(createdOn,'MMMM') = FORMAT(GETDATE(), 'MMMM')
                         )as Pending ,
                        --In-process Today
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' 
                        and ( c.callStatus = 2 or c.callStatus = 3 ) and 
                         format(createdOn,'MMMM') = FORMAT(GETDATE(), 'MMMM')
                         )as Process ,
                        --cancel request
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' and c.callStatus = '5' 
                        and  format(createdOn,'MMMM') = FORMAT(GETDATE(), 'MMMM')
                         )as Cancel ,
                        --performed request
                        (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is not Null and c.isMobile = '1' 
                        and c.callStatus = 4   and 
                         format(createdOn,'MMMM') = FORMAT(GETDATE(), 'MMMM')
                         )as Booked,
                         (select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is not Null and c.isMobile = '1') as TotalBookings,
						(select count(c.ticketNumber) from MNP_PreBookingConsignment c where c.consignmentNumber is Null and c.isMobile = '1' and c.callStatus = '5')
						as TotalCancelled

                  ");
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}