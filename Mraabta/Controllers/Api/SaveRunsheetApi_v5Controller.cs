using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Dapper;


namespace MRaabta.Controllers.Api
{
    public class SaveRunsheetApi_v5Controller : ApiController
    {
        SqlConnection con;
        public SaveRunsheetApi_v5Controller()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        [HttpPost, ActionName("Save"), EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
        public async Task<HttpResponseMessage> Save([FromBody] RunsheetModel model)
        {
            try
            {
                var rs = await SaveRunsheet(model);
                return Request.CreateResponse(HttpStatusCode.OK, new { sts = rs.sts, msg = rs.msg });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [NonAction]
        public async Task<(int sts, string msg)> SaveRunsheet(RunsheetModel model)
        {
            SqlTransaction trans = null;
            try
            {
                var error = "";
                var cnlist = model.Details.Select(x => x.ConsignmentNumber).ToList();
                var cns = string.Join(",", cnlist.Select(x => $"'{x}'"));

                var query = $@"SELECT rc.runsheetNumber, rc.branchcode, rc.consignmentNumber, DATEDIFF(Minute, rc.createdOn, GETDATE()) FROM RunsheetConsignment rc WHERE rc.consignmentNumber IN ({cns}) AND DATEDIFF(SECOND, rc.createdOn, GETDATE()) <= 6000;";

                await con.OpenAsync();
                trans = con.BeginTransaction();
                var existingCheck = await con.QueryAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                if (existingCheck.Any())
                {
                    error = "Runsheet Already made in 5 mins. CNs are " + string.Join(",", existingCheck.Select(x => x.consignmentNumber));
                }
                else
                {
                    query = "select Product, Prefix, Length, len(Prefix) as PrefixLength from MnP_ConsignmentLengths where status = 1 and Prefix is not null;";
                    var cnLengths = await con.QueryAsync<(string Product, int Prefix, int Length, int PrefixLength)>(query, commandTimeout: int.MaxValue, transaction: trans);

                    if (cnLengths.Any())
                    {
                        List<string> prefixErrorCNs = new List<string>();
                        foreach (var item in cnlist)
                        {
                            var z = cnLengths.FirstOrDefault(x => item.Substring(0, x.PrefixLength) == x.Prefix.ToString());
                            if (z.Prefix > 0)
                            {
                                if (z.Length != item.Length)
                                {
                                    prefixErrorCNs.Add(item);
                                }
                            }
                            else
                            {
                                prefixErrorCNs.Add(item);
                            }

                            //foreach (var x in cnLengths)
                            //{
                            //    var prefix = item.Substring(0, x.PrefixLength);
                            //    if (prefix != x.Prefix.ToString())//Error Yahan hat Raha hay
                            //    {
                            //        prefixErrorCNs.Add(item);
                            //    }
                            //    else
                            //    {
                            //        if (item.Length != x.Length)
                            //        {
                            //            prefixErrorCNs.Add(item);
                            //        }
                            //    }
                            //}
                        }

                        if (prefixErrorCNs.Any())
                        {
                            error = "Prefix Error CNs are " + string.Join(",", prefixErrorCNs.Select(x => x));
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            query = $@"Select * from  consignment where consignmentnumber in({cns})";
                            var cnsInDb = await con.QueryAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                            if (cnsInDb.Any() && cnsInDb.Count() != model.Details.Count())
                            {
                                var cnsToInsert = model.Details.Where(x => !cnsInDb.Select(y => y.consignmentNumber).Contains(x.ConsignmentNumber)).ToList();
                                foreach (var item in cnsToInsert)
                                {
                                    sb.AppendLine($"INSERT INTO CONSIGNMENT(ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, weight, pieces, syncid, bookingDate, createdOn, zoneCode, branchCode, serviceTypeName, consignmentTypeId) values('{item.ConsignmentNumber}', {model.BranchCode}, {item.Destination}, '330140', '4D1', '0.5', '1', NewID(), GETDATE(), GETDATE(), '{model.ZoneCode}', {model.BranchCode}, 'overnight', '12');");
                                }
                                await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);
                            }

                            query = $"select * from RUNSHEETCONSIGNMENT  WHERE consignmentNumber IN ({cns}) AND Reason IS NULL";
                            var runsheetCNs = await con.QueryAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                            if (runsheetCNs.Any())
                            {
                                sb.Clear();

                                foreach (var item in runsheetCNs)
                                {
                                    var modelCN = model.Details.FirstOrDefault(x => x.ConsignmentNumber == item.consignmentNumber);
                                    sb.AppendLine($@"INSERT INTO ConsignmentsTrackingHistory ConsignmentNumber,StateId, CurrentLocation, reason, runsheetNumber, TransactionTime,statusTime, internationalRemarks)
                                    SELECT {item.consignmentNumber}, '10',{modelCN.Destination},'UNDELIVERED','{item.runsheetNumber}',GETDATE(),GETDATE(), '');");
                                }
                                await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);
                            }

                            query = $"UPDATE RUNSHEETCONSIGNMENT SET REASON = '204', STATUS = '56', GivenToRider = '{model.RiderCode}' WHERE consignmentNumber IN ({cns}) AND Reason IS NULL";
                            await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                            query = $@"INSERT INTO Runsheet (runsheetNumber, routeCode, createdBy, createdOn, runsheetDate, branchCode, runsheetType, syncID, MeterStart, MeterEnd, VEHICLENUMBER, VEHICLETYPE,ridercode,expressCenterCode,zoneCode)
                                        VALUES ('{model.RunsheetNumber}', '{model.RouteCode}',{model.CreatedBy}, GETDATE(),'{model.RunsheetDate}',{model.BranchCode},{model.RunsheetType}, NEWID(),
                                        '{model.MeterStart}', '{model.MeterEnd}', '{model.VehicleNumber}', '{model.VehicleType}','{model.RiderCode}', '{model.ExpressCenterCode}', '{model.ZoneCode}');";
                            await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                            query = $@"INSERT INTO RiderRunsheet (ridercode, runsheetNumber, createdBy, createdOn, expIdTemp)
                                        Values ('{model.RiderCode}', '{model.RunsheetNumber}', {model.CreatedBy}, getdate(), '{model.ExpressCenterCode}');";
                            await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);


                            sb.Clear();

                            foreach (var item in model.Details)
                            {
                                sb.AppendLine($@"INSERT INTO RUNSHEETCONSIGNMENT (RUNSHEETNUMBER, CONSIGNMENTNUMBER, CREATEDBY, CREATEDON, STATUS, SORTORDER, BRANCHCODE,ROUTECODE,COD)
                                                Values ('{model.RunsheetNumber}', '{item.ConsignmentNumber}', {model.CreatedBy}, getdate(), '56', '{item.SortOrder}', {model.BranchCode}, '{model.RouteCode}', '{item.isCOD}');");

                                sb.AppendLine($@"INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber,StateId,CurrentLocation, RiderName,runsheetNumber,TransactionTime, internationalRemarks)
                                                Values('{item.ConsignmentNumber}', '8',{item.Destination}, '{model.RiderName}','{model.RunsheetNumber}', GETDATE(), '');");
                            }

                            await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);

                        }
                    }
                }

                trans.Commit();
                con.Close();

                return (sts: string.IsNullOrEmpty(error) ? 1 : 0, msg: string.IsNullOrEmpty(error) ? model.RunsheetNumber.ToString() : error);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
        }
    }
}
