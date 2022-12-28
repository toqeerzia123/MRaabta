using Dapper;
using MRaabta.App_Start;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MRaabta.Controllers
{
    public class RunsheetPrintViewController : Controller
    {
        [SkipFilter]
        public async Task<ActionResult> Index(string rs)
        {
            var list = new List<List<RunsheetPrintViewModel>>();
            var data = await GetData(rs);

            int skip = 0;
            int take = 20;

            while (skip < data.Count())
            {
                list.Add(data.Skip(skip).Take(take).ToList());
                skip += take;
            }

            return View(list);
        }

        [SkipFilter]
        public async Task<ActionResult> SearchByCN(string cn)
        {
            var list = new List<List<RunsheetPrintViewModel>>();
            var data = await GetDataByCN(cn);

            if (data != null && data.Any())
            {
                int skip = 0;
                int take = 20;

                while (skip < data.Count())
                {
                    list.Add(data.Skip(skip).Take(take).ToList());
                    skip += take;
                }

                return View("Index", list);

            }
            else
            {
                return Content("No Data Found");
            }
        }

        [NonAction]
        public async Task<List<RunsheetPrintViewModel>> GetData(string runsheet)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                try
                {
                    await con.OpenAsync();
                    var query = $@"select 
                                    r.runsheetNumber as RS,
                                    cn.consignmentNumber as CN,
                                    rc.SortOrder as Sort,
                                    r.createdOn as RunsheetDate,
                                    br.sname as Branch,
                                    ri.riderCode as RiderCode,
                                    concat(ri.firstName,'',ri.lastName) as RiderName,
                                    rt.routeCode as RouteCode,
                                    rt.name as [Route],
                                    mrt.Master_Route_Code as RouteTerritoryCode,
                                    mrt.Master_Route_Name as RouteTerritory,
                                    cn.consignee as Consignee,
                                    cn.address as ConsigneeAddress,
                                    cn.consigneeCNICNo as ConsigneeCnicNo,
                                    org.sname as Origin,
                                    dest.sname as Destination,
                                    cn.weight as [Weight],
                                    cn.pieces as Pieces,
                                    isnull(adcd.StatusId,0) AS StatusId,
                                    isnull(adcd.performed_on,concat(r.runsheetDate , ' ' , FORMAT(rc.[time],'HH:mm:ss'))) AS PerformedOn,
                                    adcd.reason AS Reason,
                                    adcd.picker_name AS Receiver,
                                    adcd.relation AS Relation,
                                    cd.codAmount as CodAmount
                                    from Runsheet r
                                    inner join RunsheetConsignment rc on r.runsheetNumber = rc.runsheetNumber
                                    inner join Consignment cn on cn.consignmentNumber = rc.consignmentNumber
                                    inner join Branches org on org.branchCode = cn.orgin
                                    inner join Branches dest on dest.branchCode = cn.destination
                                    inner join Branches br on r.branchCode = br.branchCode
                                    inner join Routes rt on rt.routeCode = r.routeCode and rt.BID = r.branchCode and rt.[status] = 1
                                    inner join Riders ri on ri.routeCode = rt.routeCode and ri.branchId = rt.BID and ri.[status] = 1
                                    left join CODConsignmentDetail_New cd on cd.consignmentNumber = rc.consignmentNumber
                                    left join App_Delivery_ConsignmentData adcd ON rc.runsheetNumber = adcd.RunSheetNumber AND rc.consignmentNumber = adcd.ConsignmentNumber
                                    left join Route_Profile_Master mrt on mrt.Master_Route_Code = rt.RouteTerritory and mrt.BranchCode = rt.BID and mrt.Status = 1
                                    where r.runsheetNumber = '{runsheet}' order by rc.SortOrder;";
                    var rs = await con.QueryAsync<RunsheetPrintViewModel>(query);
                    con.Close();
                    return rs.ToList();
                }
                catch (SqlException ex)
                {
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                    return null;
                }
            }
        }

        [NonAction]
        public async Task<List<RunsheetPrintViewModel>> GetDataByCN(string cn)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                try
                {
                    await con.OpenAsync();
                    var query = $@"select top 1
                                    r.runsheetNumber as RS,
                                    cn.consignmentNumber as CN,
                                    rc.SortOrder as Sort,
                                    r.createdOn as RunsheetDate,
                                    br.sname as Branch,
                                    ri.riderCode as RiderCode,
                                    concat(ri.firstName,'',ri.lastName) as RiderName,
                                    rt.routeCode as RouteCode,
                                    rt.name as [Route],
                                    mrt.Master_Route_Code as RouteTerritoryCode,
                                    mrt.Master_Route_Name as RouteTerritory,
                                    cn.consignee as Consignee,
                                    cn.address as ConsigneeAddress,
                                    cn.consigneeCNICNo as ConsigneeCnicNo,
                                    org.sname as Origin,
                                    dest.sname as Destination,
                                    cn.weight as [Weight],
                                    cn.pieces as Pieces,
                                    isnull(adcd.StatusId,0) AS StatusId,
                                    isnull(adcd.performed_on,concat(r.runsheetDate , ' ' , FORMAT(rc.[time],'HH:mm:ss'))) AS PerformedOn,
                                    adcd.reason AS Reason,
                                    adcd.picker_name AS Receiver,
                                    adcd.relation AS Relation,
                                    cd.codAmount as CodAmount,
                                    rc.Lat,
                                    rc.Long,
                                    rc.RiderComments
                                    from Runsheet r
                                    inner join RunsheetConsignment rc on r.runsheetNumber = rc.runsheetNumber
                                    inner join Consignment cn on cn.consignmentNumber = rc.consignmentNumber
                                    inner join Branches org on org.branchCode = cn.orgin
                                    inner join Branches dest on dest.branchCode = cn.destination
                                    inner join Branches br on r.branchCode = br.branchCode
                                    inner join Routes rt on rt.routeCode = r.routeCode and rt.BID = r.branchCode and rt.[status] = 1
                                    inner join Riders ri on ri.routeCode = rt.routeCode and ri.branchId = rt.BID and ri.[status] = 1
                                    left join CODConsignmentDetail_New cd on cd.consignmentNumber = rc.consignmentNumber
                                    left join App_Delivery_ConsignmentData adcd ON rc.runsheetNumber = adcd.RunSheetNumber AND rc.consignmentNumber = adcd.ConsignmentNumber
                                    left join Route_Profile_Master mrt on mrt.Master_Route_Code = rt.RouteTerritory and mrt.BranchCode = rt.BID and mrt.Status = 1
                                    where rc.consignmentNumber = '{cn}' order by r.createdOn desc;";
                    var rs = await con.QueryAsync<RunsheetPrintViewModel>(query);
                    con.Close();
                    return rs.ToList();
                }
                catch (SqlException ex)
                {
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                    return null;
                }
            }
        }
        
        [SkipFilter]
        public async Task<ActionResult> UndeliveredPODSheet(string ridercode, string startrunsheetdate, string endrunsheetdate)
        {
            var list = new List<List<RunsheetPrintViewModel>>();
            var u = Session["UserInfo"] as UserModel;
            var data = await GetUndeliveredPODData(ridercode, u.BranchCode, startrunsheetdate, endrunsheetdate);

            int skip = 0;
            int take = 8;

            while (skip < data.Count())
            {
                list.Add(data.Skip(skip).Take(take).ToList());
                skip += take;
            }
            ViewBag.StartDate = startrunsheetdate;
            ViewBag.EndDate = endrunsheetdate;
            return View(list);
        }

        [NonAction]
        public async Task<List<RunsheetPrintViewModel>> GetUndeliveredPODData(string ridercode, string branchcode, string startrunsheetdate, string endrunsheetdate)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                try
                {
                    await con.OpenAsync();
                    var query = $@"select rc.Comments,adcd.ReasonId,cn.consigneePhoneNo as Phone,
r.runsheetNumber as RS, cn.consignmentNumber as CN, rc.SortOrder as Sort, r.runsheetdate as RunsheetDate, adcd.performed_on AS PerformedOn, br.sname as Branch,
ri.riderCode as RiderCode, concat(ri.firstName,'',ri.lastName) as RiderName, rt.routeCode as RouteCode, rt.name as [Route], 
mrt.Master_Route_Code as RouteTerritoryCode, mrt.Master_Route_Name as RouteTerritory, cn.consignee as Consignee, cn.address as ConsigneeAddress,
cn.consigneeCNICNo as ConsigneeCnicNo, org.sname as Origin, dest.sname as Destination, cn.weight as [Weight], cn.pieces as Pieces,
isnull(adcd.StatusId,0) AS StatusId, adcd.performed_on AS PerformedOn, adcd.reason AS Reason, adcd.picker_name AS Receiver, adcd.relation AS Relation
from Runsheet r
inner join RunsheetConsignment rc on r.runsheetNumber = rc.runsheetNumber
inner join Consignment cn on cn.consignmentNumber = rc.consignmentNumber
inner join Branches org on org.branchCode = cn.orgin
inner join Branches dest on dest.branchCode = cn.destination
inner join Branches br on r.branchCode = br.branchCode
inner join Riders ri on ri.riderCode = r.ridercode and ri.branchId = r.branchCode
inner join Routes rt on rt.BID = r.branchCode and rt.routeCode = r.routeCode
inner join App_Delivery_ConsignmentData adcd ON rc.runsheetNumber = adcd.RunSheetNumber AND rc.consignmentNumber = adcd.ConsignmentNumber
left join Route_Profile_Master mrt on mrt.Master_Route_Code = rt.RouteTerritory and mrt.BranchCode = rt.BID and mrt.Status = 1
where adcd.ReasonId NOT IN ('123','59') and
r.branchCode = '{branchcode}' and r.ridercode = '{ridercode}' and cast(runsheetDate as date) between '{startrunsheetdate}' and '{endrunsheetdate}'
order by rc.SortOrder desc;";
                    var rs = await con.QueryAsync<RunsheetPrintViewModel>(query);
                    con.Close();
                    return rs.ToList();
                }
                catch (SqlException ex)
                {
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                    return null;
                }
            }
        }
    }


}