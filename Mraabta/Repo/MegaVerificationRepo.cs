using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class MegaVerificationRepo
    {
        SqlConnection con;
        public MegaVerificationRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public async Task<CNModel> GetCN()
        {
            try
            {
                var query = $@"select top 1
                                c.consignmentNumber as CN,
                                c.consigner as Shipper,
                                c.consignee as Consignee,
                                rc.receivedBy as Receiver,
                                c.consigneePhoneNo as ReceiverNumber,
                                'Answered' as CallStatus,
                                1 as DeliveryVerified,
                                '' as CustomerRemarks,
                                '' as StaffRemarks
                                from Consignment c
                                inner join RunsheetConsignment rc on rc.consignmentNumber = c.consignmentNumber
                                left join MegaVerifications mv on mv.ConsignmentNumber = c.consignmentNumber
                                where 
                                cast(c.createdOn as date) >= cast(DATEADD(DAY, -1,GETDATE()) as date) 
                                and mv.ConsignmentNumber is null
                                and rc.Reason = '123'";
                await con.OpenAsync();
                var rs = await con.QueryFirstOrDefaultAsync<CNModel>(query, commandTimeout: int.MaxValue);
                con.Close();
                return rs;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }

        public async Task<bool> Save(CNModel model)
        {
            try
            {
                var query = $@"insert into MegaVerifications (ConsignmentNumber,Shipper,Consignee,Receiver,ReceiverNumber,CallStatus,DeliveryVerified,CustomerRemarks,StaffRemarks,CreatedBy)
                                values(@CN,@Shipper,@Consignee,@Receiver,@ReceiverNumber,@CallStatus,@DeliveryVerified,@CustomerRemarks,@StaffRemarks,@CreatedBy);";
                await con.OpenAsync();
                var rs = await con.ExecuteAsync(query, model);
                con.Close();
                return rs > 0;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
    }
}