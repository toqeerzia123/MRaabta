using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Repo
{
    public class DemanifestRepo : GeneralRepo
    {
        SqlConnection con;
        public DemanifestRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SetCon(con);
        }
        public async Task OpenAsync() => await con.OpenAsync();
        public void Close()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
        public async Task<ManifestDataModel> ManifestData(string manifestNo)
        {
            try
            {
                var query = $@"select
                                m.manifestNumber as ManifestNo,
                                format(m.createdOn,'dd-MMM-yyyy') as Date,
                                concat(org.sname,' - ',org.name) as Origin,
                                concat(dest.sname,' - ',dest.name) as Destination,
                                isnull(m.isDemanifested,0) as IsDemanifested,
                                format(m.DemanifestDate,'dd-MMM-yyyy') as DemanifestDate
                                from Mnp_Manifest m
                                inner join Branches org on org.branchCode = m.origin
                                inner join Branches dest on dest.branchCode = m.destination
                                where m.manifestNumber = '{manifestNo}';
                                select 
                                mc.consignmentNumber as CN,
                                concat(org.sname,' - ',org.name) as Origin,
                                concat(dest.sname,' - ',dest.name) as Destination,
                                c.serviceTypeName as ServiceType,
                                ct.name as CNType,
                                mc.Pieces as Pcs,
                                mc.WEIGHT as Weight,
                                mc.statusCode as Status,
                                mc.Remarks
                                from Mnp_ConsignmentManifest mc 
                                left join consignment c on mc.consignmentNumber = c.consignmentNumber
                                left join Branches org on org.branchCode = c.orgin
                                left join Branches dest on dest.branchCode = c.destination
                                left join ConsignmentType ct on ct.id = c.consignmentTypeId
                                where mc.manifestNumber = '{manifestNo}';";
                using (var rs = await con.QueryMultipleAsync(query, commandTimeout: int.MaxValue))
                {
                    var manifest = await rs.ReadFirstOrDefaultAsync<ManifestDataModel>();
                    if (manifest != null)
                    {
                        var details = await rs.ReadAsync<ManifestDataDetailPrintModel>();
                        manifest.ManifestDetail = details.ToList();
                    }
                    con.Close();
                    return manifest;
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
        public async Task<(bool isok, string msg)> Save(ManifestDataModel model, UserModel u)
        {
            SqlTransaction trans = null;
            string cn = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $@"update Mnp_Manifest set isDemanifested = 1, DemanifestDate = getdate(), Demanifestby = {u.Uid} where manifestNumber = '{model.ManifestNo}';";
                await con.ExecuteAsync(query, transaction: trans);

                foreach (var item in model.ManifestDetail)
                {
                    cn = item.CN;

                    if (item.Status == 7)
                    {
                        query = $@"insert into Mnp_ConsignmentManifest (consignmentNumber, manifestNumber,statusCode,Remarks,createdOn,WEIGHT,Pieces)
                                    values('{item.CN}', '{model.ManifestNo}', 7, '{item.Remarks}', getdate(), '{item.Weight}', '{item.Pcs}'); ";
                    }
                    else
                    {
                        query = $@"update Mnp_ConsignmentManifest set statusCode = {item.Status}, Remarks = '{item.Remarks}' where consignmentNumber = '{item.CN}' and manifestNumber = '{model.ManifestNo}';";
                    }

                    if (item.Status != 6)
                        query += $@"insert into ConsignmentsTrackingHistory (consignmentNumber,	stateID, currentLocation, manifestNumber, transactionTime, statusTime)
                                values('{item.CN}', 7, '{u.LocationName}', '{model.ManifestNo}',getdate(), getdate());";

                    await con.ExecuteAsync(query, transaction: trans);
                }

                trans.Commit();
                con.Close();

                return (true, model.ManifestNo);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"Error occured in CN# {cn}, Error is {ex.Message}");
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                return (false, $"Error occured in CN# {cn}, Error is {ex.Message}");
            }
        }
        public async Task<List<dynamic>> GetDemanifests(string date)
        {
            try
            {
                var query = $@"select
                                m.manifestNumber as ManifestNo,
                                format(m.createdOn,'dd-MMM-yyyy') as Date,
                                concat(org.sname,' - ',org.name) as Origin,
                                concat(dest.sname,' - ',dest.name) as Destination,
                                (select count(*) from Mnp_ConsignmentManifest where manifestNumber = m.manifestNumber) as TotalCNs
                                from Mnp_Manifest m
                                inner join Branches org on org.branchCode = m.origin
                                inner join Branches dest on dest.branchCode = m.destination
                                where cast(m.DemanifestDate as date) = '{date}' and isnull(m.isDemanifested,0) = 1";
                await con.OpenAsync();
                var rs = await con.QueryAsync(query, commandTimeout: int.MaxValue);
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