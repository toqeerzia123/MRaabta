using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class StockIssuance : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        static String branchCode = "";
        static String ZoneCode = "", U_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ZoneCode = Session["ZONECODE"].ToString();

                U_ID = Session["U_ID"].ToString();


                if (ZoneCode == "" || ZoneCode == null || U_ID == null || U_ID == "")
                {
                    Response.Redirect("~/Login");
                }
                //if (!IsPostBack)
                //{
                //    bindZones();
                //}

                //branchCode = Session["BRANCHCODE"].ToString();
                //ZoneCode = Session["ZONECODE"].ToString();
                //branchCode = branchCode.ToUpper();


                //if (ZoneCode == "" || ZoneCode == null)
                //{
                //    Response.Redirect("~/Login.aspx");
                //}

            }
            catch (Exception er)
            {
                Response.Redirect("~/Login");
            }

        }
        //public void bindZones()
        //{
        //    try
        //    {
        //        string constring = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        //        using (SqlConnection con = new SqlConnection(constring))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("select zoneCode,name from zones", con))
        //            {
        //                SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //                DataTable dt = new DataTable();
        //                sda.Fill(dt);
        //                ddlZones.DataSource = dt;
        //                ddlZones.DataTextField = "name";
        //                ddlZones.DataValueField = "zoneCode";
        //                ddlZones.DataBind();
        //                ddlZones.Items.Insert(0, new ListItem("SELECT", "0"));
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //}
        #region  stock details
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetZones()
        {
            //return new StockIssuanceRepo().GetZones();
            try
            {
                con.Open();

                var rs = con.Query<CNIssuanceDropDownModel>(@" select zoneCode as [Value], name as [Text] from zones where region is not null order by name asc"); /*and sr.ZoneCode=@ZoneCode", new { @ZoneCode = zoneCode });*/
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
        [WebMethod]
        public static List<StockRequestModel> GetRequests(int? id)
        {
            //return new StockIssuanceRepo().GetRequestsZone(id);
            try
            {
                con.Open();

                if (id > 0)
                {
                    var rs = con.Query<StockRequestModel>(@"select sr.ID as [Id], sr.ZoneCode,z.name as ZoneName , sr.Year, FORMAT(sr.TotalReqQty,N'N0') TotalReqQty_, case when si.StockReqID is null then 0 else 1 end as [IsUpdated]  
                                                        from [MnP_CNIssue_StockRequest] sr
                                                        inner join Zones z on sr.ZoneCode= z.ZoneCode
                                                        left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                                                        where sr.isActive = 1  AND si.ID is null AND sr.TotalReqQty > 0 and sr.ZoneCode=@ZoneCode and sr.UserTypeID = '1003'", new { @ZoneCode = id });
                    con.Close();
                    return rs.ToList();
                }
                else
                {
                    var rs = con.Query<StockRequestModel>(@"select sr.ID as [Id],z.name as ZoneName,sr.ZoneCode, FORMAT(sr.TotalReqQty,N'N0') TotalReqQty_, case when si.StockReqID is null then 0 else 1 end as [IsUpdated]  
														from [MnP_CNIssue_StockRequest] sr
														inner join Zones z on sr.ZoneCode= z.ZoneCode
                                                        left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                                                        where sr.isActive = 1  AND si.ID is null AND sr.TotalReqQty > 0 and sr.UserTypeID = '1003' "); /*and sr.ZoneCode=@ZoneCode", new { @ZoneCode = zoneCode });*/
                    con.Close();
                    return rs.ToList();
                }
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

        [WebMethod]
        public static List<StockModel> GetMyStock()
        {
            // return new StockIssuanceRepo().GetMyStock();
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,z.name as Zone, sum(qty) as Qty,
                                            FORMAT(sum(qty),N'N0')  as Qty_
                                            from (
                                            select ProductID, ZoneCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock 
                                            where UserTypeID = 1002
                                            ) as xb 
                                            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                            inner join Zones z on xb.ZoneCode = z.zoneCode
                                            group by ProductID, xb.ZoneCode,ProductName,TypeName,name
                                            order by z.name");
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

        [WebMethod]
        public static List<StockModel> GetZoneStock(string zone)
        {
            //return new StockIssuanceRepo().GetZoneStock(zone);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,z.name as Zone, sum(qty) as Qty from (
                                                select ProductID, ZoneCode, 
                                                case when StatusID = 2 then qty * (-1) 
                                                  when StatusID = 1 then qty else qty * 0 end as qty 
                                                from Mnp_CNIssue_Stock 
                                                where UserTypeID = 1003
                                                ) as xb 
                                                inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                                inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                                inner join Zones z on xb.ZoneCode = z.zoneCode
                                                group by ProductID, xb.ZoneCode,ProductName,TypeName,name
                                                having xb.ZoneCode = @Zone;	", new { @Zone = zone });
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

         

        #endregion

        #region request details
        [WebMethod]
        public static List<StockRequestDetailModel> GetRequestsDetails(int id)
        {
            //return new StockIssuanceRepo().GetRequestsDetails(id);
            try
            {
                con.Open();
                var rs = con.Query<StockRequestDetailModel>(@"
                select xb.ProductID, xb.TypeName,xb.ID as Id,xb.ProductName,xb.Qty,yb.BarcodeIssueFrom,yb.QtyIssueFrom, yb.Qty [TotalSequence] from (
                    select srd.ID,srd.ProductID,pr.ProductName,t.TypeName,srd.Qty from MnP_CNIssue_StockReqDetails srd 
                    inner join MnP_CNIssue_StockRequest sr on sr.ID = srd.StockReqID
                    inner join MnP_CNIssue_Product pr on pr.ID = srd.ProductID
                    inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId 
                    where StockReqID = @Id) as xb
                    left join 
                    (
                        SELECT *,
						  CASE WHEN yb.ProductID=17 then
                           yb.BarcodeFrom
                           ELSE  (yb.BarcodeFrom + (yb.Qty - yb.QtyIssueFrom))
                           END AS BarcodeIssueFrom
                    from (
                    select ROW_NUMBER() OVER(PARTITION BY ProductID ORDER BY ProductID ASC) AS SNo,* from (select ProductID, s.barcodeFrm as BarcodeFrom,s.BarcodeTo,s.Qty,
                    (select sum(Qty) as qty from (
                    select ProductID,ZoneCode,UserTypeID,
                    case when statusID = 1 then Qty * 1 else Qty * -1 end as Qty 
                    from Mnp_CNIssue_Stock
                    where --ProductID = 1 and 
                    ZoneCode = (select ZoneCode from MnP_CNIssue_StockRequest where ID = @Id) and UserTypeID = 1002 and BarcodeFrm between s.barcodeFrm and s.BarcodeTo) as xb 
                    ) as QtyIssueFrom
                    from Mnp_CNIssue_Stock s 
                    where s.UserTypeID =  1002 --and s.ProductID = 1
                    and ZoneCode = (select ZoneCode from MnP_CNIssue_StockRequest where ID = @Id)
                    and statusID = 1 ) as xb 
                    where xb.QtyIssueFrom > 0) yb where sno = 1) yb on yb.ProductID = xb.ProductID", new { @Id = id }).ToList();
                con.Close();

                foreach (var item in rs)
                {
                    if (item.ProductId==17)
                    {
                        try
                        {
                            var amountRemain = item.TotalSequence - item.QtyIssueFrom;

                            var barcodeto = Decimal.Parse(item.BarcodeIssueFrom.Substring(0, item.BarcodeIssueFrom.Length - 1));
                            barcodeto = barcodeto + amountRemain;
                            var OCSANA = Math.Floor(barcodeto / 7);
                            OCSANA = barcodeto - OCSANA * 7;
                            string OCSANA_ = (barcodeto.ToString() + (OCSANA.ToString())).ToString();
                            item.BarcodeIssueFrom = OCSANA_;
                            //  barcodeto = barcodeto + issueqty - 1;
                        }
                        catch (Exception er)
                        {

                        }
                         
                    }
                }
                return rs;
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
        #endregion

        #region saving issuance details
        [WebMethod]
        public static string SaveIssuanceDetails(StockIssuanceModel model)
        {
            model.CreatedBy = U_ID;
            model.Year = DateTime.Now.Year;
            // return new StockIssuanceRepo().SaveIssuanceDetails(model);
            foreach (var item in model.StockIssuanceDetails)
            {
                if (item.BarcodeTo == "" || item.BarcodeTo == null)
                {
                    return "Please provide correct quantity!";
                }
            }

            con.Open();
            object issuanceDetail_Insert;
            object StockTransit;
            object StockReceiving;
            using (var tran = con.BeginTransaction())
            {
                try
                {

                    var id = con.QueryFirstOrDefault<int>(@"insert into MnP_CNIssue_StockIssuance (StockReqID,[Year],ZoneCode,TotalIssueQty,isActive,CreatedBy,CreatedOn,UserType) values(@StockReqID,@Year,@ZoneCode,@TotalIssueQty,1,@CreatedBy,GETDATE(),1003);;
                                                        SELECT SCOPE_IDENTITY();", new { @StockReqID = model.StockRequestId, DateTime.Now.Year, @ZoneCode = model.ZoneCode, @TotalIssueQty = model.StockIssuanceDetails.Sum(x => x.IssueQty), @CreatedBy = HttpContext.Current.Session["U_ID"].ToString() }, transaction: tran);

                    //var data = model.StockIssuanceDetails.Select(x => new
                    //{
                    //    StockIssuanceID = id,
                    //    StockReqDetailID = x.StockRequestDetailId,
                    //    IssueQty = x.IssueQty,
                    //    BarcodeFrm = x.BarcodeFrom,
                    //    BarcodeTo = x.BarcodeTo,
                    //    CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                    //    ProductID = x.ProductId,
                    //    ZoneCode = model.ZoneCode,
                    //    newbarcodefrom = x.BarcodeTo + 1,
                    //    newQty = x.QtyIssueFrom - x.IssueQty
                    //}).ToList();

                    foreach (var single in model.StockIssuanceDetails)
                    {
                        issuanceDetail_Insert = con.Execute(@"insert into MnP_CNIssue_StockIssuanceDetail (StockIssuanceID,StockReqDetailID,IssueQty,BarcodeFrm,BarcodeTo,CreatedBy,CreatedOn,isActive) values(@StockIssuanceID,@StockReqDetailID,@IssueQty,@BarcodeFrm,@BarcodeTo,@CreatedBy,GETDATE(),1); "
                       , new
                       {
                           StockIssuanceID = id,
                           StockReqDetailID = single.StockRequestDetailId,
                           IssueQty = single.IssueQty,
                           BarcodeFrm = single.BarcodeFrom,
                           BarcodeTo = single.BarcodeTo,
                           CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                           ProductID = single.ProductId,
                           ZoneCode = model.ZoneCode,
                           newbarcodefrom = single.BarcodeTo + 1,
                           newQty = single.QtyIssueFrom - single.IssueQty
                       }, transaction: tran);

                        StockTransit = con.Execute(@"insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,ZoneCode,BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID) values(5,@ProductID,@ZoneCode,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1003);"
                       , new
                       {
                           StockIssuanceID = id,
                           StockReqDetailID = single.StockRequestDetailId,
                           IssueQty = single.IssueQty,
                           BarcodeFrm = single.BarcodeFrom,
                           BarcodeTo = single.BarcodeTo,
                           CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                           ProductID = single.ProductId,
                           ZoneCode = model.ZoneCode,
                           newbarcodefrom = single.BarcodeTo + 1,
                           newQty = single.QtyIssueFrom - single.IssueQty
                       }, transaction: tran);

                        StockReceiving = con.Execute(@"insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,ZoneCode,BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID) values(2,@ProductID,@ZoneCode,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1002);"
                       , new
                       {
                           StockIssuanceID = id,
                           StockReqDetailID = single.StockRequestDetailId,
                           IssueQty = single.IssueQty,
                           BarcodeFrm = single.BarcodeFrom,
                           BarcodeTo = single.BarcodeTo,
                           CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                           ProductID = single.ProductId,
                           ZoneCode = model.ZoneCode,
                           newbarcodefrom = single.BarcodeTo + 1,
                           newQty = single.QtyIssueFrom - single.IssueQty
                       }, transaction: tran);
                    }
                    tran.Commit();
                    return "Stock issued successfully";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to issue stock, please contact IT for support";
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion
    }
}