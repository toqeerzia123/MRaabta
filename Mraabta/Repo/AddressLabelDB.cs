using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo
{
    public class AddressLabelDB
    {
       SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
      //  SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString());
        public class AddressLabelModel
        {
            public string ConsignmentNumber { get; set; }
            public string consignee { get; set; }
            public string consigner { get; set; }
            public long Pieces { get; set; }
            public long Weight { get; set; }
            public long totalamount { get; set; }
            public string orderrefno { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string Destination { get; set; }
        }
        public class AddressLabelDetailModel
        {
            public string Consignment { get; set; }
            public string Service { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string Shipper { get; set; }
            public string consignee { get; set; }
            public string consigneeAddress { get; set; }
            public long pieces { get; set; }
            public decimal weight { get; set; }
            public long insuranceValue { get; set; }
            public decimal codAmount { get; set; }
            public string ProductDetail { get; set; }
            public DateTime bookingDate { get; set; }
            public string remarks { get; set; }
            public string CustomerRef { get; set; }
            public string consigneePhoneNo { get; set; }
           public string BookingTime { get; set; }
            public string locationName { get; set; }
            public string locationAddress { get; set; }
        }

        public async Task<List<AddressLabelModel>> GetData(string Start, string End, int Type)
        {
            try
            {
                string query = @"select c.ConsignmentNumber, c.consignee, c.consigner, c.Pieces, c.Weight, cd.codAmount totalamount, zu.Name CreatedBy, c.CreatedOn, b.name Destination from Consignment c
 INNER JOIN branches b ON b.branchcode = c.destination
 INNER JOIN creditclients cc ON c.creditClientId = cc.id 
 INNER JOIN ZNI_USER1 zu ON zu.U_ID= c.createdBy
 LEFT JOIN CODConsignmentDetail_new cd ON cd.consignmentNumber = c.consignmentNumber 
 where CAST(c.bookingDate as DATE) between '" + Start + "' and '" + End + "' AND c.[status]!=9 AND c.serviceTypeName IN('Date Box 2Kg','Date Box 5Kg') ";
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
WHERE ISNULL(c.isApproved, '0') = '0' AND ISNULL(cd.status, '0') <> '08' and c.status!=9 and c.consignmentNumber in (" + consignmentNo +   ") ";


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