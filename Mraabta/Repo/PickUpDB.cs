using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MRaabta.Models;
using System.Configuration;
using System.Data;

namespace MRaabta.Repo
{
    public class PickUpDB
    {
        SqlConnection orcl;
        public PickUpDB()
        {
            orcl = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await orcl.OpenAsync();
        }
        public void Close()
        {
            orcl.Close();
        }


        public DataTable getPickUp(string riderCode, string StartDate, string EndDate)
        {
            DataTable ds = new DataTable();
            string sqls = "SELECT apum.PickUp_ID, apum.locationID,  r.firstName + r.lastName riderName, apum.riderCode riderCode, apum.createdOn PickUpTime, apum.longitude, apum.latitude FROM   App_PickUpMaster AS apum INNER JOIN Riders  AS r ON apum.riderCode = r.riderCode WHERE apum.riderCode = '" + riderCode + "' AND apum.createdOn > '" + StartDate + "' AND cast(apum.createdOn as date) <= '" + EndDate + "' order by apum.createdOn";
            //string sql = "SELECT apum.PickUp_ID, cc.name                              ClientName, \n"
            //  + "       cc.name as locationName, \n"
            //  + "       apum.locationID, \n"
            //  + "       r.firstName + r.lastName             riderName, \n"
            //  + "       apum.riderCode                       riderCode, \n"
            //  + "       apum.createdOn                       PickUpTime, \n"
            //  + "       apum.longitude, \n"
            //  + "       apum.latitude  \n"
            //  + "FROM   App_PickUpMaster                  AS apum \n"
            //  + "       INNER JOIN CreditClients          AS cc \n"
            //  + "            ON apum.locationID = cc.id  \n"
            //  + "       INNER JOIN Riders                 AS r \n"
            //  + "            ON  apum.riderCode = r.riderCode \n"
            //  + "WHERE  apum.riderCode = '" + riderCode + "' \n"
            //  + "       AND apum.createdOn > '" + StartDate + "' \n"
            //  + "       AND cast(apum.createdOn as date) <=  '" + EndDate + "' order by apum.PickUp_ID";

            string sql = "SELECT max(apum.PickUp_ID) as PickUp_ID , cc.name                              ClientName, \n"
           + "       cc.name as locationName, \n"
           + "       apum.locationID, \n"
           + "       r.firstName + r.lastName             riderName, \n"
           + "       apum.riderCode                       riderCode, \n"
           + "       apum.createdOn                       PickUpTime, \n"
           + "       apum.longitude, \n"
           + "       apum.latitude  \n"
           + "FROM   App_PickUpMaster                  AS apum \n"
           + "       INNER JOIN CreditClients          AS cc \n"
           + "            ON apum.locationID = cc.id  \n"
           + "       INNER JOIN Riders                 AS r \n"
           + "            ON  apum.riderCode = r.riderCode \n"
            + "       INNER JOIN App_PickUpChild                 AS pc \n"
           + "           ON apum.PickUp_ID = pc.PickUp_ID \n"
           + "WHERE  apum.riderCode = '" + riderCode + "' \n"
           + "       AND apum.createdOn > '" + StartDate + "' \n"
           + "       AND cast(apum.createdOn as date) <=  '" + EndDate + "' \n"
           + "       group by  apum.PickUp_ID, cc.name,apum.locationID, r.firstName + r.lastName             , apum.riderCode                       ,apum.createdOn                       , apum.longitude, apum.latitude";

            try
            {
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);



            }
            catch (Exception Err)
            {
                Err.Message.ToString();
                //return output;
            }
            finally { orcl.Close(); }
            return ds;
        }

        public DataTable getPickUpDetails(int pickUp_ID)
        {
            DataTable ds = new DataTable();

            string sql = "/************************************************************ \n"
           + " * Code formatted by SoftTree SQL Assistant © v6.3.153 \n"
           + " * Time: 2019-05-14 4:26:55 PM \n"
           + " ************************************************************/ \n"
           + " \n"
           + "SELECT pc.consignmentNumber, \n"
           + "       pc.WEIGHT, \n"
           + "       pc.pieces, \n"
           + "       CASE  \n"
           + "            WHEN pc.CN_picURL IS NULL THEN '/CnImage/' + pc.consignmentNumber + \n"
           + "                 '.JPG' \n"
           + "            ELSE pc.CN_picURL \n"
           + "       END AS CN_picURL \n"
           + "FROM   App_PickUpChild pc \n"
           + "       INNER JOIN App_PickUpMaster pm \n"
           + "            ON  pm.PickUp_ID = pc.pickUp_ID \n"
           + "WHERE  pc.pickUp_ID = '" + pickUp_ID + "'";

           

            try
            {
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
               
                oda.Fill(ds);
               


                //if(ds.count())
            }
            catch (Exception Err)
            {
                Err.Message.ToString();
                //return output;
            }
            finally { orcl.Close(); }
       
            return ds;
        }


        public async Task<List<DropDownModel>> GetRiders(string BranchCode)
        {
            try
            {
                string sql = @"	SELECT r.riderCode as [Value], (r.riderCode + '-' + r.firstName + ' '+ r.lastName) as [Text] from riders  r     inner join App_Users u on u.riderCode = r.riderCode where r.branchId = @bc  and r.riderCode not like ' %' order by r.riderCode";
                var rs = await orcl.QueryAsync<DropDownModel>(sql, new { @bc = BranchCode });
                //var rs = await orcl.QueryAsync<DropDownModel>(@"SELECT riderCode as [Value], riderCode + '-' + firstName + ' '+ lastName as [Text] from riders  where branchId = @bc  order by [Text];", new { @bc = BranchCode });
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public  List<DataPoint> TotalConsignment(string ridercode, string StartDate, string EndDate)
        {
                try
                {
                    orcl.Open();
                    var rs = orcl.Query<DataPoint>(@"select cc.name as Label,count(apuc.consignmentNumber) as Y from App_PickUpChild apuc inner join App_PickUpMaster apum on apum.PickUp_ID = apuc.pickUp_ID inner join CreditClients cc on apum.locationID = cc.id where apum.riderCode = @rider AND apum.createdOn > '" + StartDate + "' AND cast(apum.createdOn as date) <= '" + EndDate + "'group by apum.riderCode,apum.locationID, cc.name", new { @rider = ridercode });
                    orcl.Close();
                    return rs.ToList();
                }
                catch (SqlException ex)
                {
                    orcl.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    orcl.Close();
                    return null;
                }
            }
    }
}