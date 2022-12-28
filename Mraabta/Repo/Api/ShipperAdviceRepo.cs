using MRaabta.Models.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using MRaabta.Models;

namespace MRaabta.Repo.Api
{
    public class ShipperAdviceRepo
    {
        SqlConnection con;
        public ShipperAdviceRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<(List<DropDownModel> calltracks, List<DropDownModel> reattempts)> GetInitData()
        {
            try
            {
                var query = $@"select Id as Value, Name as Text from MNP_NCI_CallTrack where Id <> 1 and [STATUS] = 1;
                               select ReAttempt_Id as Value, NAME as Text from MNP_NCI_ReAttempt where [STATUS] = 1;";
                await con.OpenAsync();
                using (var rs = await con.QueryMultipleAsync(query))
                {
                    var rs1 = (await rs.ReadAsync<DropDownModel>()).ToList();
                    var rs2 = (await rs.ReadAsync<DropDownModel>()).ToList();
                    con.Close();
                    return (rs1, rs2);
                }
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
        }
        public async Task<List<ShipperAdviceDataModel>> GetAdvices(RequestModel model)
        {
            try
            {
                var query = "";
                var condition = "";

                if (model.Type == 1)
                {
                    if (model.From.HasValue && model.To.HasValue && !string.IsNullOrEmpty(model.CN))
                    {
                        condition = $@"AND cast(NR.CreatedOn AS date) BETWEEN '{model.From.Value.ToString("yyyy-MM-dd")}' AND '{model.To.Value.ToString("yyyy-MM-dd")}'
                                       AND NR.CONSIGNMENTNUMBER = '{model.CN}'";
                    }
                    else if (model.From.HasValue && model.To.HasValue)
                    {
                        condition = $@"AND cast(NR.CreatedOn AS date) BETWEEN '{model.From.Value.ToString("yyyy-MM-dd")}' AND '{model.To.Value.ToString("yyyy-MM-dd")}'";
                    }
                    else if (!string.IsNullOrEmpty(model.CN))
                    {
                        condition = $"AND NR.CONSIGNMENTNUMBER = '{model.CN}'";
                    }


                    query = $@"SELECT nr.CONSIGNMENTNUMBER as CN,
                                FORMAT(C.BOOKINGDATE,'dd-MMM-yyyy') as BookingDate,
                                nr.TICKETNO as TicketNo,
                                FORMAT(NR.CREATEDON,'dd-MMM-yyyy') as TicketDate,
                                NR.CREATEDON as CreatedOn,
                                B.NAME as DestinationBranch,
                                NRS.NAME as PendingReason,
                                MNN.NAME as StandardNote,
                                CS.NAME as CallStatus,
                                (cast((datediff(MINUTE, nr.CreatedOn, cast(cast(cast(DATEADD(dd,case when cast(nr.CreatedOn as time) > '17:00' then 2 else 1 end, nr.CreatedOn) as date) as varchar) +' 17:00:00' as datetime))-(DATEDIFF(MINUTE, nr.createdOn, getdate())))/ 60 as varchar) +':' +
                                cast((datediff(MINUTE, nr.CreatedOn, cast(cast(cast(DATEADD(dd,case when cast(nr.CreatedOn as time) > '17:00' then 2 else 1 end, nr.CreatedOn) as date) as varchar) +' 17:00:00' as datetime))-(DATEDIFF(MINUTE, nr.createdOn, getdate())))% 60 as varchar)
                                ) as KPI,
                                (select top 1 orderRefNo from CODConsignmentDetail_New CN where cn.consignmentNumber = nr.CONSIGNMENTNUMBER) as OrderRefNo ,
                                '' as Comments,
                                c.Consignee,
                                c.consigneePhoneNo as ConsigneeContact,
                                c.address as ConsigneeAddress,
                                cdn.codAmount as CodAmount,
                                nr.CallTrack as TempAdvice
                                FROM MNP_NCI_REQUEST nr
                                INNER JOIN Consignment C ON NR.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER and nr.AccountNo = c.consignerAccountNo
                                inner join CODConsignmentDetail_New cdn on C.consignmentNumber = cdn.consignmentNumber
                                INNER JOIN BRANCHES B ON B.BRANCHCODE = NR.DESTINATION
                                INNER JOIN MNP_NCI_REASONS NRS ON NRS.ID = NR.REASON
                                left JOIN MNP_NCI_NOTE MNN ON MNN.NOTE_ID = NR.STANDARDNOTES
                                INNER JOIN MNP_NCI_CALLSTATUS CS ON CS.ID = NR.CALLSTATUS                                
                                where AccountNo = '{model.AccountNo}' and isnull(nr.isclosed,0) = 0 and isnull(nr.isLast, 0) = 1 and nr.ISCOD = 1                                
                                {condition};";
                }
                else
                {
                    if (model.From.HasValue && model.To.HasValue && !string.IsNullOrEmpty(model.CN))
                    {
                        condition = $@"AND cast(NR.CreatedOn AS date) BETWEEN '{model.From.Value.ToString("yyyy-MM-dd")}' AND '{model.To.Value.ToString("yyyy-MM-dd")}'
                                       AND NR.CONSIGNMENTNUMBER = '{model.CN}'";
                    }
                    else if (model.From.HasValue && model.To.HasValue)
                    {
                        condition = $@"AND cast(NR.CreatedOn AS date) BETWEEN '{model.From.Value.ToString("yyyy-MM-dd")}' AND '{model.To.Value.ToString("yyyy-MM-dd")}'";
                    }
                    else if (!string.IsNullOrEmpty(model.CN))
                    {
                        condition = $"AND NR.CONSIGNMENTNUMBER = '{model.CN}'";
                    }

                    query = $@"SELECT nr.CONSIGNMENTNUMBER as CN,
                            FORMAT(C.BOOKINGDATE,'dd-MMM-yyyy') as BookingDate,
                            nr.TICKETNO as TicketNo,
                            FORMAT(NR.CREATEDON,'dd-MMM-yyyy') as TicketDate,
                            NR.CREATEDON as CreatedOn,
                            B.NAME as DestinationBranch, 
                            NRS.NAME as PendingReason, 
                            MNN.NAME as StandardNote,
                            CS.NAME as CallStatus,
                            C.remarks as AdditionalRemarks,
                            (select top 1 orderRefNo from CODConsignmentDetail_New CN where cn.consignmentNumber = nr.CONSIGNMENTNUMBER) as OrderRefNo , NR.COMMENT as Comments,
                            ct.NAME as CallTrackName,
                            c.Consignee,
                            c.consigneePhoneNo as ConsigneeContact,
                            c.address as ConsigneeAddress,
                            cdn.codAmount as CodAmount
                            FROM MNP_NCI_REQUEST nr                            
                            INNER JOIN Consignment C ON NR.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER and nr.AccountNo = c.consignerAccountNo
                            inner join CODConsignmentDetail_New cdn on C.consignmentNumber = cdn.consignmentNumber
                            INNER JOIN BRANCHES B ON B.BRANCHCODE = NR.DESTINATION
                            inner join MNP_NCI_CallTrack ct on ct.Id = nr.CallTrack and ct.[STATUS] = 1
                            INNER JOIN MNP_NCI_REASONS NRS ON NRS.ID = NR.REASON
                            left JOIN MNP_NCI_NOTE MNN ON MNN.NOTE_ID = NR.STANDARDNOTES
                            INNER JOIN MNP_NCI_CALLSTATUS CS ON CS.ID = NR.CALLSTATUS
                            where isnull(nr.isclosed,0) = 1 AND AccountNo = '{model.AccountNo}' and CallTrack in (2, 3) and isnull(nr.islast,0) = 1 and nr.ISCOD = 1
                            {condition}
                            ORDER BY nr.CONSIGNMENTNUMBER";
                }

                await con.OpenAsync();
                var rs = (await con.QueryAsync<ShipperAdviceDataModel>(query, commandTimeout: int.MaxValue)).ToList();
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
        }
        public async Task<bool> SaveAdvice(ShipperAdviceDataModel model)
        {
            try
            {
                var query = $@"update MNP_NCI_Request set isLast = 0, IsClosed = 0 where ConsignmentNumber = @CN;
                                INSERT INTO MNP_NCI_Request (TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,PortalCreatedBy,ReAttempt,islast,IsClosed)
                                SELECT TOP 1 
                                @TicketNo,
                                @CN,
                                ShipperName,
                                AccountNo,
                                Consignee,
                                {(model.ReattemptReason == 2 ? "@ConsigneeContact" : "ConsigneeCell")},                            
                                {(model.ReattemptReason == 2 ? "@ConsigneeAddress" : "ConsigneeAddress")},                            
                                Origin,
                                Destination,
                                Reason,
                                StandardNotes,
                                CallStatus,
                                @Advice,
                                @Comments,
                                1,
                                getdate(),
                                @CreatedBy,
                                @ReattemptReason,
                                1,
                                {(model.Advice > 1 && model.Advice < 4 ? 1 : 0)}
                                FROM MNP_NCI_Request m WHERE m.TicketNo = @TicketNo AND m.ConsignmentNumber = @CN 
                                and m.ticketno not in (select TicketNo from MNP_NCI_Request where TicketNo = @TicketNo and CallTrack in (2,3)) ORDER BY m.CreatedOn DESC;";

                await con.OpenAsync();
                var rs = await con.ExecuteAsync(query, model, commandTimeout: int.MaxValue);
                con.Close();

                return rs > 0;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
        }
        public async Task<List<dynamic>> TicketDetails(string cn)
        {
            try
            {
                var query = $@"SELECT 
                                CASE   
                                WHEN  
                                (SELECT MIN(m.CreatedOn) FROM MNP_NCI_Request m WHERE m.ConsignmentNumber ='{cn}' AND m.CallTrack != '3') =  min(c.CreatedOn)  
                                AND c.Destination = zu.branchcode 
                                THEN 'INITIATE BY DESTINATION' 
                                WHEN  
                                (SELECT max(m.CreatedOn) FROM MNP_NCI_Request m WHERE m.ConsignmentNumber ='{cn}' AND m.CallTrack = '3') =  max(c.CreatedOn) THEN 'CLOSE' 
                                WHEN zu.branchcode = c.Origin THEN 'UPDATE BY ORIGIN' 
                                WHEN zu.branchcode = c.Destination THEN 'UPDATE BY DESTINATION' 
                                WHEN c.PORTALCREATEDBY != ''  THEN 'UPDATE BY COD CUSTOMER'  
                                END as Status,
                                c.TicketNo,
                                M.NAME as Reason,
                                S.NAME as CallStatus,
                                c.CALLTIME as CallTime,
                                ct.NAME as CallTrack,
                                C.COMMENT as Comments,
                                C.CONSIGNEE as Consignee,
                                C.CONSIGNEECELL as ConsigneeCell,
                                C.ConsigneeAddress as ConsigneeAddress
                                FROM MNP_NCI_REQUEST C   
                                INNER JOIN BRANCHES B ON B.BRANCHCODE = C.ORIGIN   
                                INNER JOIN BRANCHES B1 ON B1.BRANCHCODE = C.DESTINATION   
                                INNER JOIN MNP_NCI_REASONS M ON C.REASON = M.ID   
                                INNER JOIN MNP_NCI_CALLSTATUS S ON C.CALLSTATUS = S.ID   
                                left JOIN ZNI_USER1 ZU ON C.CREATEDBY = ZU.U_ID  
                                LEFT JOIN MNP_NCI_CallTrack ct ON c.CallTrack = ct.Id   
                                LEFT JOIN ZNI_USER1 ZU1 ON C.MODIFIEDBY = ZU1.U_ID  
                                LEFT JOIN MNP_NCI_ReAttempt NR ON NR.ReAttempt_Id = C.ReAttempt  
                                WHERE C.CONSIGNMENTNUMBER = '{cn}'  
                                GROUP BY  
                                c.TicketNo,B.NAME, B1.NAME,M.NAME,S.NAME,C.CONSIGNMENTNUMBER, C.SHIPPERNAME,c.CALLTIME,   
                                C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON,  
                                ZU.U_NAME, C.MODIFIEDON,ZU1.U_NAME, C.PORTALCREATEDON, C.PORTALCREATEDBY,NR.NAME,  
                                C.ISCOD, c.CreatedOn, ct.NAME,c.Destination,zu.branchcode,zu.branchcode,c.Origin ,c.Destination  
                                ORDER BY C.CREATEDON DESC;";

                await con.OpenAsync();
                var rs = (await con.QueryAsync(query, commandTimeout: int.MaxValue)).ToList();
                con.Close();
                return rs;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                throw ex;
            }
        }
    }
}