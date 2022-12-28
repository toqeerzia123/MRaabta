using Dapper;
using MRaabta.Models.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MRaabta.Repo.Api
{
    public class CODCNCashConfirmationRepo
    {
        SqlConnection con;
        public CODCNCashConfirmationRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<List<CODCNCashConfirmationDetailReturnModel>> GetData(CODCNCashConfirmationModel model)
        {
            SqlTransaction trans = null;
            try
            {
                var rs = 0;
                await con.OpenAsync();
                trans = con.BeginTransaction();
                List<CODCNCashConfirmationDetailReturnModel> lst = new List<CODCNCashConfirmationDetailReturnModel>();

                foreach (var item in model.Detail)
                {
                    string query = $@"Update App_Delivery_ConsignmentData
                                    set isReceipt=1, ReceiptId='{model.ID}',ReceiptDateTime=getdate(), R_Latitude='{model.Latitude}', R_Longitude='{model.Longitude}'
                                    WHERE ReceiptId is null and riderCode = '{model.RiderCode}' AND cast(created_on AS date)= '{model.Date}' 
                                    and ConsignmentNumber = '{item.ConsignmentNumber}' and RunsheetNumber ='{item.RunsheetNumber}';";
                    rs = await con.ExecuteAsync(query, transaction: trans, commandTimeout: int.MaxValue);
                    if (rs == 1)
                    {
                        lst.Add(new CODCNCashConfirmationDetailReturnModel { Id = model.ID, ConsignmentNumber = item.ConsignmentNumber, IsUpdated = Convert.ToBoolean(rs) });
                    }
                    else
                    {
                        lst.Add(new CODCNCashConfirmationDetailReturnModel { Id = model.ID, ConsignmentNumber = item.ConsignmentNumber, IsUpdated = Convert.ToBoolean(rs) });
                    }
                }

                trans.Commit();
                con.Close();

                return lst;
            }
            catch (SqlException ex)
            {
                trans.Rollback();

                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
    }
}