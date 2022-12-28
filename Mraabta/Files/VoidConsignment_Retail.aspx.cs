using MRaabta.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class VoidConsignment_Retail : System.Web.UI.Page
    {
        static Cl_Variables clvar = new Cl_Variables();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static ConsignmentModel GetConsignmentData(string consignmentNum)
        {
            ConsignmentModel model = new ConsignmentModel();
            try
            {
                DataSet Ds_1 = new DataSet();
                string query = @"SELECT *,Convert(varchar,c.bookingDate,105) _bookingDate ,ob.name originName,od.name destinationName
                                FROM Consignment c 
                                INNER JOIN Branches ob ON ob.branchCode=c.orgin
                                INNER JOIN Branches od ON od.branchCode=c.destination
                                WHERE c.consignmentNumber=@conNum";
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.SelectCommand.Parameters.AddWithValue("@conNum", consignmentNum);
                oda.Fill(Ds_1);
                orcl.Close();

                if (Ds_1 != null)
                {
                    if (Ds_1.Tables[0].Rows.Count > 0)
                    {
                        model.AccountNo = Ds_1.Tables[0].Rows[0]["consignerAccountNo"].ToString();
                        model.ConsignmentNum = Ds_1.Tables[0].Rows[0]["consignmentNumber"].ToString();
                        model.Consigner = Ds_1.Tables[0].Rows[0]["consigner"].ToString();
                        model.Consignee = Ds_1.Tables[0].Rows[0]["consignee"].ToString();
                        model.Origin = Ds_1.Tables[0].Rows[0]["originName"].ToString();
                        model.Destination = Ds_1.Tables[0].Rows[0]["destinationName"].ToString();
                        model.BookingDate = Ds_1.Tables[0].Rows[0]["_bookingDate"].ToString();
                        model.Weight = Ds_1.Tables[0].Rows[0]["weight"].ToString();
                        model.Pieces = Ds_1.Tables[0].Rows[0]["pieces"].ToString();
                        model.DestinationCode = Ds_1.Tables[0].Rows[0]["destination"].ToString();
                        model.OriginCode = Ds_1.Tables[0].Rows[0]["orgin"].ToString();
                    }
                }
                return model;
            }
            catch (Exception er)
            {
                return model;
            }
        }



        [WebMethod]
        public static string VoidConsignment(string consignmentNum)
        {
            using (SqlConnection conn = new SqlConnection(clvar.Strcon()))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                transaction = conn.BeginTransaction("Void Consignment");
                command.Connection = conn;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = @" insert into Consignment_Archive
                                             select *
                                            from Consignment
                                           where consignmentNumber = @consignNumber";
                    command.Parameters.AddWithValue("@consignNumber", consignmentNum);
                    int numberOfRecords = command.ExecuteNonQuery();
                    if (numberOfRecords <= 0)
                    {
                        throw new Exception();
                    }

                    string sqlUpdateRecordForParentId = "update [Consignment] set consigner=@consigner,creditClientId=@creditClientId,consignerAccountNo=@consignerAccount,createdBy=@createdBy" +
                        ",createdOn=GetDate() where consignmentNumber=@consignmentNo";
                    command.CommandText = sqlUpdateRecordForParentId;
                    command.Parameters.AddWithValue("@consigner", "VOID CONSIGNMENT");
                    command.Parameters.AddWithValue("@creditClientId", "352666");
                    command.Parameters.AddWithValue("@consignerAccount", "300001");
                    command.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                    command.Parameters.AddWithValue("@consignmentNo", consignmentNum);
                    int check = command.ExecuteNonQuery();
                    if (check <= 0)
                    {
                        throw new Exception();
                    }

                    transaction.Commit();
                    return "Consignment Successfully made void!";
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        return "Error updating consignment!";

                    }
                    catch (Exception ex2)
                    {
                        return "Error updating consignment!";
                    }
                }

                finally
                {
                    conn.Close();
                }
            }
        }
    }
    public class ConsignmentModel
    {
        public string ConsignmentNum { get; set; }
        public string BookingDate { get; set; }
        public string AccountNo { get; set; }
        public string Consigner { get; set; }
        public string Consignee { get; set; }
        public string Origin { get; set; }
        public string OriginCode { get; set; }
        public string Destination { get; set; }
        public string DestinationCode { get; set; }
        public string Weight { get; set; }
        public string Pieces { get; set; }
    }
    public class BranchesModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}