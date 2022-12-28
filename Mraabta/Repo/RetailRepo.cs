using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MRaabta.Repo
{
    public class RetailRepo
    {
        SqlConnection con;
        public RetailRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public List<DropDownModel> GetZones()
        {
            try
            {
                con.Open();

                var rs = con.Query<DropDownModel>(@" select zoneCode as [Value], name as [Text] from zones"); 
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
        public List<DropDownModel> GetBranches(String zone)

        {
            try
            {
                con.Open();

                var rs = con.Query<DropDownModel>(@" select branchCode as [Value], name as [Text] from branches where zoneCode='@zone'", new { @zone=zone}); 
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
        public List<DropDownModel> GetExpressCenter(String branch)
        {
            try
            {
                con.Open();

                var rs = con.Query<DropDownModel>(@" select zoneCode as [Value], name as [Text] from branches where zoneCode='@branch'", new { @branch = branch }); 
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
        public String GetSpecialDiscountId()
        {
            try
            {
                con.Open();

                return "23423";
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
    }
}