using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class AddressLabelRepo
    {
       SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
      // SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString());

        public async Task<List<AddressLabelModel>> GetData(string Start, string End, int Type, string Service)
        {
            try
            {
                string query = $@"select c.ConsignmentNumber, c.consignee, c.consigner, c.Pieces, c.Weight, cd.codAmount totalamount, zu.Name CreatedBy, c.CreatedOn, b.name Destination from Consignment c
 INNER JOIN branches b ON b.branchcode = c.destination
 INNER JOIN creditclients cc ON c.creditClientId = cc.id 
 INNER JOIN ZNI_USER1 zu ON zu.U_ID= c.createdBy
 LEFT JOIN CODConsignmentDetail_new cd ON cd.consignmentNumber = c.consignmentNumber 
 where CAST(c.bookingDate as DATE) between '{Start}' and '{End}' AND c.[status]!=9 AND c.serviceTypeName Like '%{Service}%' ";
                if (Type== 0)
                {
                    query += " and c.expressionMessage !='1' ";
                }
                if (Type == 1)
                {
                    query += " and c.expressionMessage = '1' ";
                }
               
              
                await con.OpenAsync();
                var rs = await con.QueryAsync<AddressLabelModel>(query);
                con.Close();
                List<AddressLabelModel> s = new List<AddressLabelModel>();
                s = (List<AddressLabelModel>)rs;
                return s.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            }

        public async Task<List<AddressLabelDetailModel>> GetDetail(string consignmentNo, int type)
        {
            try
            {
                //consignmentNo = consignmentNo.TrimEnd(',');
                //consignmentNo = consignmentNo.Replace("''", "");

                if (type == 0)
                {
                    string updquery = @"UPDATE Consignment SET expressionMessage = '1' WHERE consignmentNumber IN (" + consignmentNo + ") ";
                   await con.OpenAsync();
                    var upd = await con.ExecuteAsync(updquery);
                    con.Close();
                } 
               

                string query = @"SELECT c.consignmentNumber Consignment, c.bookingDate, c.serviceTypeName SERVICE, b.name Origin, b1.name Destination, c.consigner Shipper, c.consignee,
c.address consigneeAddress, c.pieces, c.weight, c.insuarancePercentage insuranceValue, cd.codAmount, cd.productDescription ProductDetail, c.remarks, 
cd.orderRefNo CustomerRef, c.consigneePhoneNo, CONVERT(VARCHAR, c.createdOn, 114) BookingTime, ccl.locationName, ccl.locationAddress
FROM Consignment c 
INNER JOIN Branches b ON c.orgin = b.branchCode 
INNER JOIN Branches b1 ON c.destination = b1.branchCode 
LEFT JOIN CODConsignmentDetail_new cd ON cd.consignmentNumber = c.consignmentNumber 
LEFT JOIN COD_CustomerLocations ccl ON ccl.locationID = c.locationID 
WHERE ISNULL(cd.status, '0') <> '08' and c.status!=9 and c.consignmentNumber in (" + consignmentNo +   ") ";


                await con.OpenAsync();
                var rs = await con.QueryAsync<AddressLabelDetailModel>(query);
                con.Close();
                List<AddressLabelDetailModel> s = new List<AddressLabelDetailModel>();
                s = (List<AddressLabelDetailModel>)rs;
                return s.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          