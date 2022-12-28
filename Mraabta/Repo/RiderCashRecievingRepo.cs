using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo
{
    public class RiderCashRecievingRepo : GeneralRepo
    {
        SqlConnection con;
        public RiderCashRecievingRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SetCon(con);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            con.Close();
        }
        public async Task<List<DropDownModel>> GetRiders()
        {
            try
            {
                var rs = await con.QueryAsync<DropDownModel>($@"select r.riderCode as [Value], CONCAT(r.riderCode,' - ',r.firstName,' ',r.lastName) as [Text] from Riders r 
                            where r.status=1 and r.userTypeId in ('90','217','72') and r.riderCode!='';");
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> CheckRider(string riderId)
        {
            try
            {
                var rs = await con.QueryFirstOrDefaultAsync<string>($@"select r.firstName+' '+r.lastName as Name from Riders r where  r.riderCode = '{riderId}' and r.status = 1;");
                return rs;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetExpressCenter(string UserId)
        {
            try
            {
                var rs = await con.QueryFirstOrDefaultAsync<string>($@"select ExpressCenterCode from MnP_Retail_Staff where UserId= '{UserId}' and status = 1;");
                return rs;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<dynamic> InsertData(RiderCashRecievingModel model, UserModel u)
        {
            SqlTransaction trans = null;
            var CNCounts = 0;
            var CurrentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var insquery = $@" Declare @ExpressCenter varchar(20)
                                set @ExpressCenter= (select ExpressCenterCode from MnP_Retail_Staff where status=1 and UserId='{u.Uid}')
                                insert into tbl_RiderCashPayment
                                (RiderCode,RiderName,EcCode, ExpectedAmount, CollectedAmount, DiffAmount,DSSP, CreatedBy, CreatedOn)
                                values (@RiderCode,@RiderName, @ExpressCenter,@ExpAmount, @ColAmount, @DiffAmount, @DSSP,@userId, @CreatedOn);
                                SELECT SCOPE_IDENTITY() as Id; ";
                var inscheck = await con.QueryFirstOrDefaultAsync<long>(insquery,
                    new
                    {
                        RiderCode = model.RiderCode,
                        RiderName = model.RiderName,
                        ExpAmount = model.CurrExpAmount,
                        ColAmount = model.ColAmount,
                        DiffAmount = model.DiffAmount,
                        DSSP = 0,
                        userId = u.Uid,
                        CreatedOn = CurrentDate
                    }, transaction: trans);
                if (inscheck > 0)
                {
                    CNCounts = model.CNLineItem == null ? 0 : model.CNLineItem.Where(c => c.IsPaid.Equals(false)).Count();

                    if (CNCounts > 0)
                    {
                        int lineitemcheck = 0;
                        foreach (var item in model.CNLineItem.Where(c => c.IsRecieved.Equals(false)))
                        {
                            var lineitemquery = $@"insert into tbl_RiderCashRecvLineItem
                                (TransactionId,RunSheet_No,Consignment_No, Consignment_Amount, Amount_Paid, RR)
                                values (@TransactionId,@RunSheet_No, @Consignment_No,@Consignment_Amount, @Amount_Paid, @RR);";
                            lineitemcheck = await con.ExecuteAsync(lineitemquery,
                                new
                                {
                                    TransactionId = inscheck,
                                    RunSheet_No = item.RunSheetNumber,
                                    Consignment_No = item.ConsignmentNumber,
                                    Consignment_Amount = item.RiderAmount,
                                    Amount_Paid = item.AmountRcv,
                                    RR = 0
                                }, transaction: trans, commandTimeout: int.MaxValue);
                        }

                        if (model.paymentLineItem != null)
                        {
                            var TransactionHistory = model.paymentLineItem.Where(c => Math.Abs(Convert.ToInt32(c.ShortAmount)) > 0).ToList();
                            if (TransactionHistory.Count() > 0 && Convert.ToInt32(model.DiffAmount) != 0)
                            {
                                var Check = await CheckPreviousHistory(model, TransactionHistory, trans, inscheck.ToString());
                            }
                        }

                    }
                }

                trans.Commit();
                con.Close();

                #region RR Code                 
                //var TransactionSum = model.paymentLineItem != null ? model.paymentLineItem.Sum(c => Int32.Parse(c.SubmittedAmount)) + Int32.Parse(model.ColAmount) : 0;

                //if ((model.TotalExpAmount.Equals(model.ColAmount) && model.paymentLineItem == null) || ((TransactionSum).Equals(Int32.Parse(model.TotalExpAmount)) && Int32.Parse(model.TotalExpAmount) > 0))
                //{
                //    DataTable dt = new DataTable();

                //    dt.Columns.AddRange(new DataColumn[] {
                //            new DataColumn("RunsheetNumber", typeof(string)),
                //            new DataColumn("RiderCode", typeof(string)),
                //            new DataColumn("ExpressCenterCode", typeof(string)),
                //            new DataColumn("ConsignmentNumber", typeof(string)),
                //            new DataColumn("CODAmount", typeof(float)),
                //            new DataColumn("CreditClientID", typeof(Int64)),
                //            new DataColumn("RefNumber", typeof(string)),
                //            new DataColumn("PaymentSource", typeof(string)),
                //               new DataColumn("Remarks", typeof(string))
                //            });
                //    DataRow dr;

                //    foreach (var rs in model.CNLineItem.Select(c => c.RunSheetNumber).Distinct())
                //    {
                //        var diffCheck = Int32.Parse(model.ColAmount);
                //        foreach (var RSData in model.CNLineItem.Where(c => c.RunSheetNumber.Equals(rs)))
                //        {
                //            dr = dt.NewRow();
                //            // Get Consignment CredirClientID
                //            var CreditClient = GetCreditClientId(RSData.ConsignmentNumber, trans);
                //            diffCheck = diffCheck - Int32.Parse(RSData.AmountRcv);

                //            dr["RunsheetNumber"] = RSData.RunSheetNumber;
                //            dr["RiderCode"] = model.RiderCode;
                //            dr["ExpressCenterCode"] = u.ExpressCenter;
                //            dr["ConsignmentNumber"] = RSData.ConsignmentNumber;
                //            dr["CODAmount"] = RSData.AmountRcv;
                //            dr["CreditClientID"] = Int64.Parse(CreditClient);
                //            dr["RefNumber"] = "";
                //            dr["PaymentSource"] = "1";
                //            dr["Remarks"] = "Retail COD Cash Entry";

                //            dt.Rows.Add(dr);
                //        }
                //        dt.AcceptChanges();
                //        model.CreatedDate = Convert.ToDateTime(CurrentDate).ToString("yyyy-MM-dd");
                //        var RRresponse1 = CreateCODBulkVouchers(model, u, rs, dt);
                //        if (RRresponse1.Equals("OK"))
                //        {
                //            var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                //                set RR=1
                //                where RunSheet_No='{rs}';";
                //            var updatelineitemcheck = await con.ExecuteAsync(updatelineitemquery, transaction: trans, commandTimeout: int.MaxValue);
                //        }
                //        Response += "RR-" + RRresponse1.ToString();
                //    }
                //}
                #endregion
                return inscheck;
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
        }
        public async Task<string> CheckPreviousHistory(RiderCashRecievingModel model, List<RiderCashDetailModel> previousTransaction, SqlTransaction trans, string currTID)
        {
            var remainingamount = Int32.Parse(model.DiffAmount);
            // For Short Amount
            if (remainingamount > 0)
            {
                var uplineitemcheck = 0;
                foreach (var prevtransaction in previousTransaction.Where(c => Int32.Parse(c.ShortAmount) < 0))
                {
                    var previousDiffAmount = Int32.Parse(prevtransaction.ShortAmount);
                    var ActualpreviousAmount = Int32.Parse(prevtransaction.ShortAmount);
                    var CurrTransactionDetail = await GetPreviousTransactionsData(Int32.Parse(currTID), trans);
                    var MasterAmountDeduct = 0;
                    foreach (var item in CurrTransactionDetail.OrderByDescending(c => Int32.Parse(c.AmountRcv)))
                    {
                        var CNamountdeduct = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : Int32.Parse(item.RiderAmount) - Int32.Parse(item.AmountRcv);
                        if (CNamountdeduct <= Math.Abs(previousDiffAmount))
                        {

                            MasterAmountDeduct = MasterAmountDeduct + CNamountdeduct;
                            var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                                                                    set Amount_Paid=@AmountRcv
                                                                    where TransactionId=@TID and Consignment_No=@Consignment_No";
                            uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                                new
                                {
                                    TID = Int32.Parse(currTID),
                                    Consignment_No = item.ConsignmentNumber,
                                    AmountRcv = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : CNamountdeduct + Int32.Parse(item.AmountRcv)
                                }, transaction: trans, commandTimeout: int.MaxValue);
                            previousDiffAmount = Math.Abs(previousDiffAmount) - CNamountdeduct;
                        }
                        else if (CNamountdeduct > Math.Abs(previousDiffAmount) & Math.Abs(previousDiffAmount) > 0)
                        {
                            //  var CNamountdeduct = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : Int32.Parse(item.RiderAmount) - Int32.Parse(item.AmountRcv);                            

                            MasterAmountDeduct = MasterAmountDeduct + Math.Abs(previousDiffAmount);
                            var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                                                         set Amount_Paid=@AmountRcv
                                                         where TransactionId=@TID and Consignment_No=@Consignment_No";
                            uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                                new
                                {
                                    TID = Int32.Parse(currTID),
                                    Consignment_No = item.ConsignmentNumber,
                                    AmountRcv = Int32.Parse(item.AmountRcv) == 0 ? Math.Abs(previousDiffAmount) : Int32.Parse(item.AmountRcv) + Math.Abs(previousDiffAmount)
                                }, transaction: trans, commandTimeout: int.MaxValue);
                            previousDiffAmount = Math.Abs(previousDiffAmount) - Math.Abs(previousDiffAmount);
                        }
                    }
                    if (MasterAmountDeduct > 0)
                    {
                        var query = $@"Update tbl_RiderCashPayment  set DiffAmount=@PrevDiffAmount  where Id=@PrevTID   
                                       Update tbl_RiderCashPayment  set DiffAmount=@DiffAmount      where Id=@TID";
                        var querycheck = await con.ExecuteAsync(query,
                            new
                            {
                                PrevTID = prevtransaction.Id,
                                PrevDiffAmount = ActualpreviousAmount < 0 ? ActualpreviousAmount + MasterAmountDeduct : ActualpreviousAmount - MasterAmountDeduct,
                                TID = currTID,
                                DiffAmount = remainingamount - MasterAmountDeduct,
                            }, transaction: trans, commandTimeout: int.MaxValue);
                        remainingamount = remainingamount - MasterAmountDeduct;
                    }
                }
            }
            // For Excess Amount
            else if (remainingamount < 0)
            {
                var uplineitemcheck = 0;
                foreach (var prevtransaction in previousTransaction.Where(c => Int32.Parse(c.ShortAmount) > 0))
                {
                    var PrevTransactionDetail = await GetPreviousTransactionsData(prevtransaction.Id, trans);
                    var MasterAmountDeduct = 0;
                    foreach (var item in PrevTransactionDetail.OrderByDescending(c => Int32.Parse(c.AmountRcv)))
                    {
                        var CNamountdeduct = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : Int32.Parse(item.RiderAmount) - Int32.Parse(item.AmountRcv);
                        if (CNamountdeduct <= Math.Abs(remainingamount) && CNamountdeduct != 0)
                        {

                            MasterAmountDeduct = MasterAmountDeduct + CNamountdeduct;
                            var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                                                         set Amount_Paid=@AmountRcv
                                                         where TransactionId=@TID and Consignment_No=@Consignment_No";
                            uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                                new
                                {
                                    TID = prevtransaction.Id,
                                    Consignment_No = item.ConsignmentNumber,
                                    AmountRcv = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : CNamountdeduct + Int32.Parse(item.AmountRcv)
                                }, transaction: trans, commandTimeout: int.MaxValue);
                            remainingamount = Math.Abs(remainingamount) - CNamountdeduct;
                        }
                        else if (CNamountdeduct > Math.Abs(remainingamount) & Math.Abs(remainingamount) > 0)
                        {
                            //var AmountLeft = Math.Abs(remainingamount) + CNamountdeduct > Int32.Parse(item.RiderAmount) ? Math.Abs(remainingamount) :  CNamountdeduct - Math.Abs(remainingamount);
                            MasterAmountDeduct = MasterAmountDeduct + Math.Abs(remainingamount);


                            var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                                                         set Amount_Paid=@AmountRcv
                                                         where TransactionId=@TID and Consignment_No=@Consignment_No";
                            uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                                new
                                {
                                    TID = prevtransaction.Id,
                                    Consignment_No = item.ConsignmentNumber,
                                    AmountRcv = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : Math.Abs(remainingamount) + Int32.Parse(item.AmountRcv),
                                }, transaction: trans, commandTimeout: int.MaxValue);
                            remainingamount = Math.Abs(remainingamount) - Math.Abs(remainingamount);
                        }
                    }

                    if (MasterAmountDeduct > 0)
                    {
                        var query = $@"Update tbl_RiderCashPayment  set DiffAmount=@PrevDiffAmount  where Id=@PrevTID   
                                       Update tbl_RiderCashPayment  set DiffAmount=@DiffAmount      where Id=@TID";
                        var querycheck = await con.ExecuteAsync(query,
                            new
                            {
                                PrevTID = prevtransaction.Id,
                                PrevDiffAmount = Int32.Parse(prevtransaction.ShortAmount) - MasterAmountDeduct,
                                TID = currTID,
                                DiffAmount = MasterAmountDeduct - Math.Abs(Int32.Parse(model.DiffAmount)),
                            }, transaction: trans, commandTimeout: int.MaxValue);
                    }

                }
            }

            return "Ok";
        }
        public async Task<string> CheckPreviousHistoryArc(RiderCashRecievingModel model, List<RiderCashDetailModel> previousTransaction, SqlTransaction trans, string currTID)
        {
            var remainingamount = Int32.Parse(model.DiffAmount);
            // For Excess Amount
            if (remainingamount < 0)
            {
                int uplineitemcheck = 0;
                foreach (var prevtransaction in previousTransaction)
                {
                    //Particular Line items that has Short Amount                    
                    if (Int32.Parse(prevtransaction.ShortAmount) > Math.Abs(remainingamount) && Int32.Parse(prevtransaction.ShortAmount) > 0)
                    {
                        var PreviousTransactionDetail = await GetPreviousTransactionsData(prevtransaction.Id, trans);
                        #region Test
                        // Both Transaction have equal amount

                        //if (Int32.Parse(prevtransaction.ShortAmount) == remainingamount)
                        //{
                        //    //update master table
                        //    var query = $@"Update tbl_RiderCashPayment
                        //                   set DiffAmount=@PrevDiffAmount
                        //                   where Id=@PrevTID   
                        //                    Update tbl_RiderCashPayment
                        //                   set DiffAmount=@DiffAmount
                        //                   where Id=@TID";
                        //    var querycheck = await con.ExecuteAsync(query,
                        //        new
                        //        {
                        //            PrevTID = prevtransaction.Id,
                        //            PrevDiffAmount = 0,
                        //            TID = currTID,
                        //            DiffAmount = 0,
                        //        }, transaction: trans, commandTimeout: int.MaxValue);

                        //    foreach (var item in PreviousTransactionDetail)
                        //    {
                        //        var CNamountdeduct = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : Int32.Parse(item.RiderAmount) - Int32.Parse(item.AmountRcv);
                        //        if (CNamountdeduct <= remainingamount)
                        //        {
                        //            remainingamount = remainingamount - CNamountdeduct;

                        //            //  Update Line Item Table
                        //            var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                        //                                            set Amount_Paid=@AmountRcv
                        //                                            where TransactionId=@TID and Consignment_No=@Consignment_No";
                        //            uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                        //                new
                        //                {
                        //                    TID = prevtransaction.Id,
                        //                    Consignment_No = item.ConsignmentNumber,
                        //                    AmountRcv = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : CNamountdeduct + Int32.Parse(item.AmountRcv),
                        //                }, transaction: trans, commandTimeout: int.MaxValue);

                        //        }
                        //        //else
                        //        //{
                        //        //    //var CNamountdeduct = remainingamount > Int32.Parse(item.AmountRcv) ? Int32.Parse(item.RiderAmount) : Int32.Parse(item.RiderAmount) - Int32.Parse(item.PrevAmountRcv);
                        //        //    remainingamount = remainingamount - CNamountdeduct;

                        //        //    //  Update Line Item Table
                        //        //    var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                        //        //                                    set AmountPaid=@AmountRcv
                        //        //                                    where TransactionId=@TID and Consignment_No=@Consignment_No";
                        //        //    uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                        //        //        new
                        //        //        {
                        //        //            TID = prevtransaction.Id,
                        //        //            Consignment_No = item.ConsignmentNumber,
                        //        //            Amount_Paid = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : CNamountdeduct + Int32.Parse(item.AmountRcv),
                        //        //        }, transaction: trans, commandTimeout: int.MaxValue);
                        //        //}
                        //    }
                        //
                        #endregion

                        //if (Int32.Parse(prevtransaction.ShortAmount) > remainingamount)
                        //{

                        foreach (var item in PreviousTransactionDetail)
                        {
                            if (Int32.Parse(item.RiderAmount) < remainingamount)
                            {
                                var CNamountdeduct = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : Int32.Parse(item.RiderAmount) - Int32.Parse(item.AmountRcv);
                                remainingamount = remainingamount - CNamountdeduct;

                                var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                                                                    set Amount_Paid=@AmountRcv
                                                                    where TransactionId=@TID and Consignment_No=@Consignment_No";
                                uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                                    new
                                    {
                                        TID = prevtransaction.Id,
                                        Consignment_No = item.ConsignmentNumber,
                                        AmountRcv = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : CNamountdeduct + Int32.Parse(item.AmountRcv),
                                    }, transaction: trans, commandTimeout: int.MaxValue);
                            }
                        }

                        //update master table
                        var query = $@"Update tbl_RiderCashPayment
                                           set DiffAmount=@PrevDiffAmount
                                           where Id=@PrevTID   
                                            Update tbl_RiderCashPayment
                                           set DiffAmount=@DiffAmount
                                           where Id=@TID";
                        var querycheck = await con.ExecuteAsync(query,
                            new
                            {
                                PrevTID = prevtransaction.Id,
                                PrevDiffAmount = Int32.Parse(prevtransaction.ShortAmount) - Math.Abs(Int32.Parse(model.DiffAmount)),
                                TID = currTID,
                                DiffAmount = remainingamount,
                            }, transaction: trans, commandTimeout: int.MaxValue);

                        //}


                    }
                }
            }
            // For Short Amount
            else if (remainingamount > 0)
            {
                int uplineitemcheck = 0;
                foreach (var prevtransaction in previousTransaction)
                {
                    //Particular Line items that has Excess Amount                    
                    if (Int32.Parse(prevtransaction.ShortAmount) < remainingamount)
                    {
                        var PreviousTransactionDetail = await GetPreviousTransactionsData(prevtransaction.Id, trans);
                        var x = PreviousTransactionDetail.Where(c => c.AmountRcv != c.RiderAmount).Select(c => c);
                        if (x.Count() > 0)
                        {
                            foreach (var item in PreviousTransactionDetail)
                            {
                                var CNamountdeduct = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : Int32.Parse(item.RiderAmount) - Int32.Parse(item.AmountRcv);
                                if (CNamountdeduct < remainingamount)
                                {
                                    remainingamount = remainingamount - CNamountdeduct;

                                    var updatelineitemquery = $@"Update tbl_RiderCashRecvLineItem
                                                                    set Amount_Paid=@AmountRcv
                                                                    where TransactionId=@TID and Consignment_No=@Consignment_No";
                                    uplineitemcheck = await con.ExecuteAsync(updatelineitemquery,
                                        new
                                        {
                                            TID = currTID,
                                            Consignment_No = item.ConsignmentNumber,
                                            AmountRcv = Int32.Parse(item.AmountRcv) == 0 ? Int32.Parse(item.RiderAmount) : CNamountdeduct + Int32.Parse(item.AmountRcv),
                                        }, transaction: trans, commandTimeout: int.MaxValue);
                                }
                            }

                            //update master table
                            var query = $@"Update tbl_RiderCashPayment
                                           set DiffAmount=@PrevDiffAmount
                                           where Id=@PrevTID   
                                            Update tbl_RiderCashPayment
                                           set DiffAmount=@DiffAmount
                                           where Id=@TID";
                            var querycheck = await con.ExecuteAsync(query,
                                new
                                {
                                    PrevTID = prevtransaction.Id,
                                    PrevDiffAmount = remainingamount - Math.Abs(Int32.Parse(prevtransaction.ShortAmount)),
                                    TID = currTID,
                                    DiffAmount = remainingamount - Math.Abs(Int32.Parse(prevtransaction.ShortAmount)),
                                }, transaction: trans, commandTimeout: int.MaxValue);
                        }
                    }
                }
            }

            return "Ok";
        }
        public async Task<IEnumerable<RiderCODConsignmentModel>> GetPreviousTransactionsData(int Id, SqlTransaction trans)
        {
            try
            {
                var query = $@"select app.ConsignmentNumber,app.RunSheetNumber,app.cod_amount as RiderAmount,
                            ISNULL(rc.Amount_Paid,0) as AmountRcv  from App_Delivery_ConsignmentData app
                            left outer join tbl_RiderCashRecvLineItem rc on rc.Consignment_No=app.ConsignmentNumber
                            where rc.TransactionId={Id} and convert(date, app.created_on)=cast((GETDATE()) as date) and rc.Amount_Paid!=rc.Consignment_Amount 
                            and app.StatusId=1 and app.IsReceipt=1 order by app.RunSheetNumber,cast(rc.Amount_Paid as Int) asc;";
                var rs = await con.QueryAsync<RiderCODConsignmentModel>(query, transaction: trans);
                return rs;
            }
            catch (SqlException ex)
            {
                trans.Commit();
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Commit();
                con.Close();
                throw ex;
            }
        }
        public async Task<string> CreateCODBulkVouchersNotWork(RiderCashRecievingModel model, UserModel u, string rs, DataTable dt)
        {
            string msg = "";
            try
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@Runsheet", value: rs, direction: ParameterDirection.Input);
                param.Add("@Runsheetwithdash", value: rs + "-1", direction: ParameterDirection.Input);
                param.Add("@VoucherDate", value: model.CreatedDate, direction: ParameterDirection.Input);
                param.Add("@ZoneCode", value: u.ZoneCode, direction: ParameterDirection.Input);
                param.Add("@BranchCode", value: u.BranchCode, direction: ParameterDirection.Input);
                param.Add("@CreatedBy", value: u.Uid.ToString(), direction: ParameterDirection.Input);
                param.Add("@PaymentTypeID", value: "5", direction: ParameterDirection.Input);
                param.Add("@Consignments", value: dt, direction: ParameterDirection.Input);

                param.Add("@ErrorMessage", value: SqlDbType.VarChar, direction: ParameterDirection.Output);

                msg = await con.QueryFirstOrDefaultAsync<string>("MnP_CreateBulkCODPaymentVouchers_new", param
                   , commandType: CommandType.StoredProcedure, commandTimeout: int.MaxValue);
                con.Close();
            }
            catch (Exception ex)
            { msg = ex.Message; }
            finally { con.Close(); }

            return msg;
        }
        public string CreateCODBulkVouchers(RiderCashRecievingModel model, UserModel u, string rs, DataTable dt)
        {
            string error = "";
            string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    using (SqlCommand cmd = new SqlCommand("MnP_CreateBulkCODPaymentVouchers_new", con))
                    {
                        con.Open();
                        cmd.CommandTimeout = 300000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Runsheet", rs);
                        cmd.Parameters.AddWithValue("@Runsheetwithdash", rs + "-1");
                        cmd.Parameters.AddWithValue("@VoucherDate", model.CreatedDate);
                        cmd.Parameters.AddWithValue("@ZoneCode", u.ZoneCode);
                        cmd.Parameters.AddWithValue("@BranchCode", u.BranchCode);
                        cmd.Parameters.AddWithValue("@CreatedBy", u.Uid);
                        cmd.Parameters.AddWithValue("@PaymentTypeID", "5");
                        cmd.Parameters.AddWithValue("@Consignments", dt);
                        cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        error = cmd.Parameters["@ErrorMessage"].SqlValue.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                con.Close();
                error = ex.Message;
            }
            finally { con.Close(); }

            return error;
        }
        public async Task<IEnumerable<dynamic>> GetRiderDetail(string riderId)
        {
            try
            {
                var query = $@"select app.ConsignmentNumber,app.RunSheetNumber,app.rider_amount_entered as Amount
                               from App_Delivery_ConsignmentData app
                               where app.riderCode='{riderId}' and convert(date, app.created_on)=cast((GETDATE()) as date) and app.StatusId=1";
                var rs = await con.QueryAsync<dynamic>(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<RiderCODConsignmentModel>> getTodayCNs(string RiderId)
        {
            try
            {
                var rs = await con.QueryAsync<RiderCODConsignmentModel>($@"select app.ConsignmentNumber,app.RunSheetNumber,app.cod_amount as RiderAmount,
                                                                        ISNULL(rc.Amount_Paid,0) as AmountRcv,ISNULL(rc.Amount_Paid,0) as PrevAmountRcv,
                                                                        case when app.cod_amount=rc.Amount_Paid then 1 else 0 end as IsPaid,
                                                                        case when app.ConsignmentNumber=rc.Consignment_No then 1 else 0 end as IsRecieved
                                                                        from App_Delivery_ConsignmentData app
                                                                        left outer join tbl_RiderCashRecvLineItem rc on rc.Consignment_No=app.ConsignmentNumber
                                                                        where app.riderCode='{RiderId}' and convert(date, app.created_on)=cast((GETDATE()) as date) 
                                                                        and IsNull(rider_amount_entered,0)!=0 and app.StatusId=1 and app.IsReceipt=1 order by  IsPaid, IsRecieved, Runsheet_No;");
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<RiderCashDetailModel>> getTodayTransactionsByRider(string RiderId)
        {
            try
            {
                var query = $@"select Distinct(rc.RiderCode), rc.Id,rc.CreatedOn SubmitTime, ec.name ExpressCenter, rc.ExpectedAmount ExpectAmount, rc.CollectedAmount SubmittedAmount, rc.DiffAmount ShortAmount from tbl_RiderCashPayment rc                                                                    
                                                                    inner join ExpressCenters ec on ec.expressCenterCode=rc.EcCode
                                                                    where rc.riderCode='{RiderId}' and convert(date, rc.CreatedOn)=cast((GETDATE()) as date) 
																	order by rc.CreatedOn desc";

                //var xquery = $@"select Distinct(rc.RiderCode), rc.Id,rc.CreatedOn SubmitTime, ec.name ExpressCenter, rc.ExpectedAmount ExpectAmount, rc.CollectedAmount SubmittedAmount, rc.DiffAmount ShortAmount from tbl_RiderCashPayment rc
                //                                                    inner join App_Delivery_ConsignmentData app on app.riderCode=rc.RiderCode
                //                                                    inner join ExpressCenters ec on ec.expressCenterCode=rc.EcCode
                //                                                    where rc.riderCode='{RiderId}' and convert(date, rc.CreatedOn)=cast((GETDATE()) as date) 
                //                                                    and IsNull(app.rider_amount_entered,0)!=0 and app.StatusId=1  order by rc.CreatedOn desc";
                var rs = await con.QueryAsync<RiderCashDetailModel>(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<long> getTodaySumAmount(string RiderId)
        {
            try
            {
                var query = $@"select Isnull(CEILING(cast(sum(cd.codAmount) as float)),0) from CODConsignmentDetail_New cd 
                                join RunsheetConsignment rc on rc.consignmentNumber=cd.consignmentNumber
                                join Runsheet rs on rs.runsheetNumber=rc.runsheetNumber
                                join Consignment c on c.consignmentNumber=rc.consignmentNumber
                                where rs.ridercode='{RiderId}' and c.cod=1 and cast(rc.createdOn as date)=cast((GETDATE()) as date)";

                var rs = await con.QueryFirstOrDefaultAsync<long>(query);
                return rs;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RiderCashRecievingModel> PrintVoucher(string Id, string riderCode)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select r.Id, r.RiderCode, r.RiderName, IsNull(r.ExpectedAmount,0) as CurrExpAmount, r.CollectedAmount as ColAmount,r.DiffAmount, z.U_CODE as StaffCode,
                            z.Name as StaffName,r.CreatedOn as CreatedDate, r.EcCode ECCode,ec.name ECName,r.DSSP from tbl_RiderCashPayment r 
                            inner join ZNI_USER1 z on z.U_ID=r.CreatedBy
                            inner join ExpressCenters ec on ec.expressCenterCode=r.EcCode
                            where r.Id = '{Id}' and z.bts_user=1 and z.Status=1;
                            select Distinct(rc.RiderCode), rc.Id,rc.CreatedOn SubmitTime, ec.name ExpressCenter, rc.ExpectedAmount ExpectAmount, 
                            rc.CollectedAmount SubmittedAmount, (rc.CollectedAmount - rc.ExpectedAmount) ShortAmount from tbl_RiderCashPayment rc
                            inner join ExpressCenters ec on ec.expressCenterCode=rc.EcCode
                            where rc.riderCode='{riderCode}' and convert(date, rc.CreatedOn)=cast((GETDATE()) as date) order by rc.CreatedOn desc;
                            select rc.Runsheet_No as RunSheetNumber,rc.Consignment_No as ConsignmentNumber,rc.Consignment_Amount RiderAmount,rc.Amount_Paid AmountRcv from tbl_RiderCashRecvLineItem rc 
                            where rc.TransactionId='{Id}' ";

                using (var item = await con.QueryMultipleAsync(query, commandTimeout: int.MaxValue))
                {
                    var rs = await item.ReadFirstOrDefaultAsync<RiderCashRecievingModel>();
                    rs.paymentLineItem = await item.ReadAsync<RiderCashDetailModel>();
                    rs.CNLineItem = await item.ReadAsync<RiderCODConsignmentModel>();
                    return rs;
                }
            }
            catch (SqlException ex)
            {
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            finally { con.Close(); }
        }
        public async Task<string> SendSMS(string Id, string RiderId)
        {
            SqlTransaction trans;
            trans = con.BeginTransaction();
            try
            {
                string query = $@"select * FROM ";

                var smsquery = $@"insert into MnP_SmsStatus
                                (ConsignmentNumber,Recepient,MessageContent, Status, CreatedOn, CreatedBy,ModifiedOn, ModifiedBy, RunSheetNumber, ErrorCode, Error, SMSFormType, responseId)
                                values (@ConsignmentNumber,@Recepient, @MessageContent,@Status, @CreatedOn, @CreatedBy, @RunSheetNumber,@ErrorCode, @Error, @SMSFormType, @responseId);
                                SELECT SCOPE_IDENTITY() as Id; ";
                var inscheck = await con.QueryFirstOrDefaultAsync<string>(smsquery,
                    new
                    {
                        ConsignmentNumber = RiderId,
                        //Recepient = model.RiderName,
                        //EcCode = u.ExpressCenter,
                        //ExpAmount = model.CurrExpAmount,
                        //ColAmount = model.ColAmount,
                        //DiffAmount = model.DiffAmount,
                        //DSSP = 0,
                        //userId = u.Uid,
                        //CreatedOn = CurrentDate
                    }, transaction: trans);

                trans.Commit();
                con.Close();

                return inscheck;
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;

            }
            catch (Exception ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
        }
    }
}