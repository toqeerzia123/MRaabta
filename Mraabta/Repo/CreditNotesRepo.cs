using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MRaabta.Models;
using OfficeOpenXml;

namespace MRaabta.Repo
{
    public class CreditNotesRepo
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        //SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString());
        public async Task<List<CreditNotesModel>> GetDetail(string Invoice)
        {
            try
            {
                string updquery = $@"SELECT 'Invoice' AS type, '' creditnoteid, i.invoiceNumber,cc.accountNo, CC.IsCOD, b.sname AS branch, cc.name AS customername, i.totalAmount, i.invoiceDate, isnull(zu.U_NAME, i.createdBy)  CreatedBy, i.createdOn
                                        FROM Invoice i
                                        INNER JOIN CreditClients cc ON cc.id= i.clientId 
                                        INNER JOIN Branches b ON b.branchCode = cc.branchCode
                                        left JOIN ZNI_USER1 zu ON zu.U_ID= case when ISNUMERIC(i.createdBy) = 1 then i.createdBy end
                                        WHERE i.invoiceNumber='{Invoice}'
                                        UNION ALL
                                        SELECT 'Credit Note' AS type, gv.id creditnoteid, gv.InvoiceNo AS invoicenumber, cc.accountNo,CC.IsCOD, b.sname AS branch, cc.name AS customername, gv.Amount AS totalamount, gv.VoucherDate AS invoiceDate, isnull(zu.U_NAME, gv.createdBy)  CreatedBy, gv.CreatedOn
                                        FROM GeneralVoucher gv
                                        INNER JOIN CreditClients cc ON cc.id= gv.CreditClientId
                                        INNER JOIN Branches b ON b.branchCode = cc.branchCode
                                        left JOIN ZNI_USER1 zu ON zu.U_ID= case when ISNUMERIC(gv.createdBy) = 1 then gv.createdBy end
                                        WHERE gv.InvoiceNo='{Invoice}'
                                        UNION ALL
                                        SELECT 'Invoice Redeem' AS type, '' creditnoteid, InvoiceNo AS invoicenumber, cc.accountNo,CC.IsCOD, b.sname AS branch, cc.name AS customername, ir.Amount AS totalamount, pv.VoucherDate AS invoiceDate, isnull(zu.U_NAME, ir.createdBy)  CreatedBy, ir.CreatedOn
                                        FROM InvoiceRedeem ir
                                        INNER JOIN PaymentVouchers pv ON pv.Id= ir.PaymentVoucherId
                                        INNER JOIN CreditClients cc ON cc.id= pv.CreditClientId
                                        INNER JOIN Branches b ON b.branchCode = cc.branchCode
                                        left JOIN ZNI_USER1 zu ON zu.U_ID= case when ISNUMERIC(ir.createdBy) = 1 then ir.createdBy end
                                        WHERE ir.InvoiceNo='{Invoice}'";
                await con.OpenAsync();
                var data = await con.QueryAsync<CreditNotesModel>(updquery);
                con.Close();
                return data.Count() != 0 ? data.ToList() : null;
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

        [NonAction]
        public async Task<List<PaymentTypesModel>> GetPaymentTypes()
        {
            try
            {
                var query = $@"SELECT Id, Name FROM PaymentTypes pt WHERE id IN (2,3,7,8)";
                await con.OpenAsync();
                var rs = await con.QueryAsync<PaymentTypesModel>(query);
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
        }

        public async Task<bool> UpdateDetail(string Invoice, decimal Amount, string VoucherDate, long PaymentType)
        {
            try
            {
                string query = $@"SELECT i.clientid, cc.branchCode,cc.zoneCode FROM Invoice i 
                                  INNER JOIN CreditClients cc ON cc.id=i.clientId 
                                  where i.invoiceNumber='{Invoice}'";
                await con.OpenAsync();
                var data = await con.QueryAsync(query);
                con.Close();

                if (data != null)
                {
                    string updquery = $@"INSERT into GeneralVoucher
(
	VoucherDate,
	IsByCreditClient,
	IsByExpressCenter,
	ExpressCenterCode,
	RiderCode,
	CreditClientId,
	InvoiceNo,
	PaymentTypeId,
	Amount,
	BranchCode,
	ZoneCode,
	CreatedOn,
	CreatedBy
)
VALUES
(
	'{VoucherDate}',
	1,
	null,
	null,
	null,
	'{data.First().clientid}',
	'{Invoice}',
	{PaymentType},
	{Amount},
	'{data.First().branchCode}',
	'{data.First().zoneCode}',
	GetDate(),
	'{HttpContext.Current.Session["U_ID"]}'
)";
                    await con.OpenAsync();
                    var upd = await con.ExecuteAsync(updquery);
                    con.Close();
                    return upd == 1 ? true : false;
                }
                return false;
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

        public async Task<CreditNotesModel> GetCreditNoteDetail(string Id)
        {
            try
            {
                var query = $@"SELECT gv.Id creditnoteid, FORMAT(voucherdate, 'yyyy-MM-dd') invoiceDate, PaymentTypeId, Amount totalAmount, cc.IsCOD from GeneralVoucher gv
                               INNER JOIN CreditClients cc ON cc.id= gv.CreditClientId 
                               where gv.Id= " + Id;

                await con.OpenAsync();
                var rs = await con.QueryAsync<CreditNotesModel>(query);
                con.Close();
                return rs != null ? rs.First() : null;
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

        public async Task<bool> UpdateCreditNoteDetail(string Id, decimal Amount, long PaymentType, string VoucherDate)
        {
            try
            {
                var query = $@"UPDATE GeneralVoucher 
                               SET VoucherDate='{VoucherDate}', Amount={Amount}, PaymentTypeId='{PaymentType}'
                               where id='{Id}' ";

                await con.OpenAsync();
                int rs = await con.ExecuteAsync(query);
                con.Close();
                return rs == 1 ? true : false;
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
        public DataSet ToDataTable(ExcelPackage package)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataSet table = new DataSet();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Tables[0].Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.Tables[0].NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Tables[0].Rows.Add(newRow);
            }
            return table;
        }

        public async Task<string> CheckInvoiceExists(string data)
        {
            try
            {
                var query = $@"SELECT t.invoicenumber  from (
                           values {data}
                           ) t (invoicenumber)
                           left join Invoice i on t.invoicenumber = i.invoiceNumber
                           where i.invoiceNumber is null ";
                string invoiceexists = "";
                await con.OpenAsync();
                var rs = await con.QueryAsync(query);
                rs = rs.ToList();
                if (rs != null)
                {

                    foreach (var item in rs)
                    {
                        invoiceexists += "'" + item.invoicenumber + "', ";
                    }

                    invoiceexists = invoiceexists.Trim(',');
                }
                con.Close();
                return invoiceexists;
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

        public async Task<List<CreditNotesModel>> FindTotalAmounts(string data)
        {
            try
            {
                var query = $@"SELECT i.invoiceNumber,cc.IsCOD, i.totalAmount- ((SELECT isnull(sum(gv1.Amount),0) FROM generalvoucher gv1 where gv1.InvoiceNo = i.invoiceNumber )  + 
                              (SELECT isnull(sum(ir1.Amount),0) FROM InvoiceRedeem ir1 where ir1.InvoiceNo= i.invoiceNumber )) AS totalamount
                              FROM Invoice i
                              INNER JOIN CreditClients cc ON cc.id=i.clientId 
                              WHERE i.invoiceNumber IN ({data}) ";
                await con.OpenAsync();
                var rs = await con.QueryAsync<CreditNotesModel>(query);
                con.Close();
                return rs.ToList();
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

        public async Task<bool> InsertBulkRecords(List<CreditNotesModel> models)
        {
            try
            {
                string updquery = "";
                if (models != null)
                {
                    if (models.Count > 0)
                    {
                        updquery = $@"INSERT into GeneralVoucher
(
	VoucherDate,
	IsByCreditClient,
	IsByExpressCenter,
	ExpressCenterCode,
	RiderCode,
	CreditClientId,
	InvoiceNo,
	PaymentTypeId,
	Amount,
	BranchCode,
	ZoneCode,
	CreatedOn,
	CreatedBy
)
VALUES ";

                        for (int i = 0; i < models.Count; i++)
                        {
                            updquery += $@"(

    '{models[i].VoucherDate}',
	1,
	null,
	null,
	null,
	'{models[i].creditclientid}',
	'{models[i].invoiceNumber}',
	{models[i].paymenttypeid},
	{models[i].totalAmount},
	'{models[i].branch}',
	'{HttpContext.Current.Session["ZoneCode"]}',
	GetDate(),
	'{HttpContext.Current.Session["U_ID"]}'),";
                        }
                        updquery = updquery.Trim(',');
                    }
                }
                await con.OpenAsync();
                var upd = await con.ExecuteAsync(updquery);
                con.Close();
                return upd == 1 ? true : false;
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }

        [NonAction]
        public async Task<dynamic> GetDayEnd(string InvoiceNumber)
        {
            try
            {
                var query = $@"select Cast(MAX([DateTime]) as Date) as MaxDate from Mnp_Account_DayEnd 
                               where zone = (SELECT zonecode FROM CreditClients cc 
                               INNER JOIN Invoice i ON i.clientId=cc.id AND i.invoiceNumber='{InvoiceNumber}') and Doc_Type in ('C')";
                await con.OpenAsync();
                var rs = await con.QueryAsync(query);
                con.Close();
                return rs;
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
        }
        [NonAction]
        public async Task<List<dynamic>> GetDayEndforBulk(string InvoiceNumbers)
        {
            try
            {
                var query = $@"select i.invoiceNumber, Cast(MAX([DateTime]) as Date) as MaxDate from Mnp_Account_DayEnd, invoice i
                                    INNER JOIN CreditClients cc ON cc.id=i.clientId
                                    where zone = cc.zoneCode and Doc_Type in ('C') and i.clientId=cc.id AND i.invoiceNumber IN ({InvoiceNumbers})
                                    GROUP BY i.invoiceNumber";
                await con.OpenAsync();
                var rs = await con.QueryAsync(query);
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return null;
            }

        }
    }
}