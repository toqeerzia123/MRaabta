using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class ProjectRunsheetRepo
    {
        SqlConnection con;
        public ProjectRunsheetRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task<List<dynamic>> GetRoutes(string bc)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync(@"select distinct r.routeCode as Value, concat(r.routeCode,' - ',r.name) as Text
                                                            from projectdata pd
                                                            inner join Routes r on r.BID = pd.Destination and pd.RouteCode = r.routeCode and r.status = 1
                                                            where pd.Destination = @bc", new { bc });

                con.Close();
                return rs.ToList();
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
        public async Task<(List<dynamic> riders, dynamic counts)> GetRidersAnCNCounts(string bc, string rc)
        {
            try
            {
                await con.OpenAsync();
                var riders = await con.QueryAsync($@"select riderCode as Value, concat(riderCode,' - ',firstName,' ', lastName) as Text, r.routeCode as RouteCode from Riders r where r.branchId = {bc} and status = 1;", new { bc });
                var counts = await con.QueryFirstOrDefaultAsync(@"select
                                                                (select count(pd.Consignmentnumber) from projectdata pd  
                                                                where pd.Destination = @bc and pd.RouteCode = @rc and ISNULL(pd.RunsheetNumber,'') = '') as Remaining,
                                                                (select count(pd.Consignmentnumber) from projectdata pd  
                                                                where pd.Destination = @bc and pd.RouteCode = @rc and ISNULL(pd.RunsheetNumber,'') <> '') as Used;", new { bc, rc });
                con.Close();
                return (riders.ToList(), counts);
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
        public async Task<(string msg, string rsno, bool success)> Save(UserModel u, ProjectRunsheetModel model)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select top {model.Assign} Consignmentnumber from projectdata pd
                                where pd.Destination = {u.BranchCode} and pd.RouteCode = '{model.RouteCode}' and isnull(pd.RunsheetNumber,'') = '';";
                var cns = await con.QueryAsync<string>(query);

                if (model.Assign == cns.Count())
                {
                    StringBuilder sb = new StringBuilder();
                    cns.ToList().ForEach(x => sb.Append($"'{x}',"));
                    var c = sb.ToString().TrimEnd(',');
                    var rsn = DateTime.Now.Ticks.ToString() + new Random().Next(0, 9).ToString();
                    query = $@"update projectdata set 
                                RunsheetNumber = '{rsn}',
                                Runsheetdate = '{model.Date.ToString("yyyy-MM-dd")}',
                                RunsheetBranch = {u.BranchCode},
                                RunsheetRiderCode = '{model.RiderCode}',
                                RunsheetCreatedOn = getdate(),
                                RunsheetCreatedBy = {u.Uid}
                                where Consignmentnumber in ({c})";
                    var rs = await con.ExecuteAsync(query);
                    con.Close();
                    return ($"Runsheet# {rsn} saved", rsn, true);
                }
                else
                {
                    con.Close();
                    return ($"{cns.Count()} CN available", null, false);
                }
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
        public async Task<List<dynamic>> GetRunsheetDetails(string rsno,string bc)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync(@"select 
                                                p.Runsheetdate as Date,
                                                b.name as Branch,
                                                p.RunsheetNumber,
                                                p.RouteCode,
                                                r.name as Route,
                                                p.RunsheetRiderCode as RiderCode,
                                                concat(ri.firstName,' ',ri.lastName) as Rider,
                                                p.Consignmentnumber as CN,
                                                p.Address
                                                from ProjectData p 
                                                inner join Branches b on b.branchCode = p.BranchCode
                                                inner join Routes r on r.routeCode = p.RouteCode and r.BID = p.Destination and r.status = 1
                                                inner join Riders ri on ri.riderCode = p.RunsheetRiderCode and ri.branchId = p.Destination and ri.status = 1
                                                where p.RunsheetNumber = @rsno and p.RunsheetBranch = @bc and ((p.PODCreatedBy is null and p.PODCreatedOn is null) or (p.Status = 56)) order by p.Consignmentnumber;", new { rsno, bc });

                con.Close();
                return rs.ToList();
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
        public async Task<List<dynamic>> GetRunsheetDetailsForPrint(string rsno, string bc)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync(@"select 
                                                p.Runsheetdate as Date,
                                                b.name as Branch,
                                                p.RunsheetNumber,
                                                p.RouteCode,
                                                r.name as Route,
                                                p.RunsheetRiderCode as RiderCode,
                                                concat(ri.firstName,' ',ri.lastName) as Rider,
                                                p.Consignmentnumber as CN,
                                                p.Address
                                                from ProjectData p 
                                                inner join Branches b on b.branchCode = p.BranchCode
                                                inner join Routes r on r.routeCode = p.RouteCode and r.BID = p.Destination and r.status = 1
                                                inner join Riders ri on ri.riderCode = p.RunsheetRiderCode and ri.branchId = p.Destination and ri.status = 1
                                                where p.RunsheetNumber = @rsno and p.RunsheetBranch = @bc order by p.Consignmentnumber;", new { rsno, bc });

                con.Close();
                return rs.ToList();
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
        public async Task<bool> UpdatePOD(UserModel u, List<ProjectRunsheetPODModel> model)
        {
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();
                StringBuilder sb = new StringBuilder();
                foreach (var item in model)
                {
                    sb.AppendLine($"update ProjectData set ReceivedBy = '{item.Receiver}', ReceiverPhoneNo = '{item.PhoneNo}', PODCreatedOn = getdate(), PODCreatedBy = {u.Uid}, {(item.Delivered ? "Status = 55, DeliveryDate = getdate()" : "Status = 56")} where Consignmentnumber = '{item.CN}';");
                }

                var rs = await con.ExecuteAsync(sb.ToString(), transaction: trans);
                trans.Commit();
                con.Close();
                return rs > 0;
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

        public async Task<List<dynamic>> GetRunsheets(string bc, DateTime date, string route)
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync($@"with t as(select 
                                                p.RunsheetNumber,
                                                p.Runsheetdate as Date,
                                                b.name as Branch,
                                                p.RouteCode,
                                                r.name as Route,
                                                p.RunsheetRiderCode as RiderCode,
                                                concat(ri.firstName,' ',ri.lastName) as Rider
                                                from ProjectData p 
                                                inner join Branches b on b.branchCode = p.BranchCode
                                                inner join Routes r on r.routeCode = p.RouteCode and r.BID = p.Destination and r.status = 1
                                                inner join Riders ri on ri.riderCode = p.RunsheetRiderCode and ri.branchId = p.Destination and ri.status = 1
                                                where p.Destination = {bc} and  cast(p.Runsheetdate as date) = '{date.ToString("yyyy-MM-dd")}'
                                                {(string.IsNullOrEmpty(route) ? "" : $"and p.RouteCode = '{route}'")}
                                                )
                                                select * from t
                                                group by  t.Date, t.RunsheetNumber,t.Branch,t.RouteCode, t.Route, t.RiderCode, t.Rider
                                                order by t.RunsheetNumber;");

                con.Close();
                return rs.ToList();
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