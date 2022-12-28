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
    public partial class StockIssuanceBranchEC_IMS : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        static String BranchCode = "", ExpressCenter = "";
        static String ZoneCode = "", U_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ZoneCode = Session["ZONECODE"].ToString();
                U_ID = Session["U_ID"].ToString();
                ExpressCenter = Session["ExpressCenter"].ToString();
                BranchCode = Session["BRANCHCODE"].ToString();

                if (ZoneCode == "" || ZoneCode == null || U_ID == null || U_ID == "")
                {
                    Response.Redirect("~/Login");
                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/Login");
            }
        }

        #region Delete Stock Requests
        [WebMethod]
        public static bool DeleteStockRequest(String RequestId)
        {
            try
            {

                con.Open();
                var rs = con.Execute(@"Delete from [MnP_CNIssue_StockRequest] where ID=" + RequestId + "; Delete from MnP_CNIssue_StockReqDetails where StockReqID=" + RequestId);

                con.Close();
                return true;

            }
            catch (SqlException ex)
            {
                con.Close();
                return false;
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }
        #endregion


        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetExpressCenter()
        {
            //return new StockIssuanceBranchECRepo().GetZones(BranchCode);
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"  select expressCenterCode as [Value], (expressCenterCode+' - ' + name ) as [Text] from ExpressCenters where bid=@bid and status =1", new { @bid = HttpContext.Current.Session["BRANCHCODE"].ToString() });
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
        public static List<StockRequestModel> GetRequests(string EC, string startdate, string enddate)
        {
            try
            {
                con.Open();

                if (EC != "" && EC != "0")
                {
                    var rs = con.Query<StockRequestModel>($@"select sr.ID as [Id], sr.ZoneCode,b.name branchName,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn ,b.branchCode BranchCo,ec.expressCenterCode+' - '+ec.name  ECName,ec.expressCenterCode ECRiderCode, sr.Year, sr.TotalReqQty, FORMAT(sr.TotalReqQty,N'N0') as TotalReqQty_, 
                case when si.StockReqID is null then 0 else 1 end as [IsUpdated]  
                from [MnP_CNIssue_StockRequest] sr
                left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                left join Branches b on b.branchCode=sr.BranchCode
                left join ExpressCenters ec on Ec.expressCenterCode=sr.ECRiderCode
                where sr.isActive = 1  AND si.ID is null AND sr.TotalReqQty > 0 
                and sr.ZoneCode='{HttpContext.Current.Session["ZONECODE"].ToString()}' and sr.BranchCode='{HttpContext.Current.Session["BRANCHCODE"].ToString()}' and sr.ECRiderCode='{EC}' and sr.UserTypeID=1006  and cast(sr.createdon as date) between isnull(@startdate,sr.createdon) and isnull(@enddate,sr.createdon)",
                new { @startdate = startdate, @enddate = enddate });
                    con.Close();
                    return rs.ToList();
                }
                else
                {
                    var rs = con.Query<StockRequestModel>(@"select sr.ID as [Id], sr.ZoneCode,b.name branchName,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn ,b.branchCode BranchCo,ec.expressCenterCode+' - '+ec.name  ECName,ec.expressCenterCode ECRiderCode, sr.Year, sr.TotalReqQty, FORMAT(sr.TotalReqQty,N'N0') as TotalReqQty_, 
                case when si.StockReqID is null then 0 else 1 end as [IsUpdated]  
                from [MnP_CNIssue_StockRequest] sr
                left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                left join Branches b on b.branchCode=sr.BranchCode
                left join ExpressCenters ec on Ec.expressCenterCode=sr.ECRiderCode
                where sr.isActive = 1  AND si.ID is null AND sr.TotalReqQty > 0 
                and sr.ZoneCode=@ZoneCode and sr.BranchCode=@branch  and sr.UserTypeID=1006  and cast(sr.createdon as date) between isnull(@startdate,sr.createdon) and isnull(@enddate,sr.createdon)", new
                    {
                        @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                        @Branch = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                        @startdate = startdate,
                        @enddate = enddate
                    });
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
            //return new StockIssuanceBranchECRepo().GetMyStock(BranchCode);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select pr.OracleCode, productID,pr.ProductName,t.TypeName,BranchID,z.name as Zone, 
                    sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                    select ProductID, ZoneCode,BranchID, case when StatusID = 2 then qty * (-1) 
                      when StatusID = 1 then qty else qty * 0 end as qty from Mnp_CNIssue_Stock
                    where UserTypeID = 1004
                    ) as xb
                    inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                    inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                    inner join Zones z on xb.ZoneCode = z.zoneCode
                    inner join Branches b on xb.BranchID = b.branchCode
                    group by pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,z.name,BranchID
                having xb.BranchID = @Branch", new
                {
                    @Branch = HttpContext.Current.Session["BRANCHCODE"].ToString()
                });
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
        public static List<StockModel> GetZoneStock(string EC)
        {
            //return new StockIssuanceBranchECRepo().GetZoneStock(EC);

            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"Select pr.OracleCode, productID,pr.ProductName,t.TypeName, 
            sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
        select ProductID, ZoneCode, [BranchID],[ECRiderCode],
        case when StatusID = 2 then qty * (-1) 
            when StatusID = 1 then qty else qty * 0 end as qty 
        from Mnp_CNIssue_Stock 
        where UserTypeID = 1006
        ) as xb 
        inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
        inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
        inner join Zones z on xb.ZoneCode = z.zoneCode
		left join ExpressCenters ec on xb.ECRiderCode=ec.expressCenterCode			
        group by pr.OracleCode,ProductID,ProductName,TypeName,ec.name,xb.BranchID,xb.ECRiderCode
        having  xb.ECRiderCode=@EC", new { @EC = EC });
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
        public static List<StockRequestDetailModel> GetRequestsDetails(int id)
        {
            try
            {
                con.Open();
                var rs = con.Query<StockRequestDetailModel>(@"select xb.ProductID,xb.OracleCode , xb.TypeName,xb.ID as Id,xb.ProductName,FORMAT(xb.Qty,N'N0') as Qty_,xb.Qty,yb.BarcodeIssueFrom,yb.QtyIssueFrom,yb.Qty [TotalSequence]  from (
                    select pr.OracleCode, srd.ID,srd.ProductID,pr.ProductName,t.TypeName,srd.Qty from MnP_CNIssue_StockReqDetails srd 
                    inner join MnP_CNIssue_StockRequest sr on sr.ID = srd.StockReqID
                    inner join MnP_CNIssue_Product pr on pr.ID = srd.ProductID
                    inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId 
                    where StockReqID = @Id) as xb
                    left join 
                    (
                     SELECT *,
						CASE WHEN yb.ProductID=17 OR (yb.BarcodeFrom=0 AND yb.BarcodeTo=0) then
                        yb.BarcodeFrom
                        ELSE  (yb.BarcodeFrom + (yb.Qty - yb.QtyIssueFrom))
                        END AS BarcodeIssueFrom
                    from (
                    select ROW_NUMBER() OVER(PARTITION BY ProductID ORDER BY ProductID ASC) AS SNo,* from (select ProductID, s.barcodeFrm as BarcodeFrom,s.BarcodeTo,s.Qty,
                    (select sum(Qty) as qty from (
                    select ProductID,ZoneCode,UserTypeID,
                    case when statusID = 1 then Qty * 1 else Qty * -1 end as Qty 
                    from Mnp_CNIssue_Stock
                    where ProductID in (SELECT mpcsrd.ProductID FROM MnP_CNIssue_StockReqDetails mpcsrd WHERE mpcsrd.StockReqID=@Id) and 
                    ZoneCode = (select ZoneCode from MnP_CNIssue_StockRequest where ID = @Id) and BranchID = (select BranchCode from MnP_CNIssue_StockRequest where ID = @Id) and UserTypeID = 1004 and BarcodeFrm between s.barcodeFrm and s.BarcodeTo) as xb 
                    ) as QtyIssueFrom
                    from Mnp_CNIssue_Stock s 
                    where s.UserTypeID =  1004 and BranchID = (select BranchCode from MnP_CNIssue_StockRequest where ID = @Id)--and s.ProductID = 1
                    and ZoneCode = (select ZoneCode from MnP_CNIssue_StockRequest where ID = @Id)
                    and statusID = 1 ) as xb 
                    where xb.QtyIssueFrom > 0) yb where sno = 1) yb on yb.ProductID = xb.ProductID ;", new { @Id = id });
                con.Close();

                foreach (var item in rs)
                {
                    if (item.ProductId == 17)
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

        #region saving issuance detail
        [WebMethod]
        public static string SaveIssuanceDetails(StockIssuanceModel model)
        {
            model.CreatedBy = U_ID;
            model.Year = DateTime.Now.Year;
            model.ZoneCode = ZoneCode;
            foreach (var item in model.StockIssuanceDetails)
            {
                if (item.BarcodeTo == "" || item.BarcodeTo == null)
                {
                    return "Please provide correct quantity!";
                }
            }
            object rs;
            object stockTransit;
            object stockIssue;
            con.Open();

            using (var tran = con.BeginTransaction())
            {
                try
                {
                    var id = con.QueryFirstOrDefault<int>(@"insert into MnP_CNIssue_StockIssuance (StockReqID,[Year],ZoneCode,[BranchCode],[ECRiderCode],TotalIssueQty,isActive,CreatedBy,CreatedOn,UserType,Remarks) values(@StockReqID,@Year,@ZoneCode,@BranchCode,@ECRiderCode,@TotalIssueQty,1,@CreatedBy,GETDATE(),1006,@Remarks);
                                                        SELECT SCOPE_IDENTITY();", new { @StockReqID = model.StockRequestId, @Year = model.Year, @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(), @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @ECRiderCode = model.ZoneCode, @TotalIssueQty = model.StockIssuanceDetails.Sum(x => x.IssueQty), @CreatedBy = model.CreatedBy, @Remarks = model.Remarks == "" || model.Remarks == null ? "" : model.Remarks }, transaction: tran);

                    foreach (var single in model.StockIssuanceDetails)
                    {
                        rs = con.Execute(@"insert into MnP_CNIssue_StockIssuanceDetail (StockIssuanceID,StockReqDetailID,IssueQty,BarcodeFrm,BarcodeTo,CreatedBy,CreatedOn,isActive) values(@StockIssuanceID,@StockReqDetailID,@IssueQty,@BarcodeFrm,@BarcodeTo,@CreatedBy,GETDATE(),1);"
                       , new
                       {
                           StockIssuanceID = id,
                           StockReqDetailID = single.StockRequestDetailId,
                           IssueQty = single.IssueQty,
                           BarcodeFrm = single.BarcodeFrom,
                           BarcodeTo = single.BarcodeTo,
                           CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                       }, transaction: tran);

                        stockTransit = con.Execute(@"insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,ZoneCode,[BranchID],[ECRiderCode],BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID) values(5,@ProductID,@ZoneCode,@BranchID,@ECRiderCode,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1006);"
                       , new
                       {
                           ProductID = single.ProductId,
                           ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                           BranchID = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                           ECRiderCode = model.ZoneCode,
                           BarcodeFrm = single.BarcodeFrom,
                           BarcodeTo = single.BarcodeTo,
                           IssueQty = single.IssueQty,
                       }, transaction: tran);

                        stockIssue = con.Execute(@"Insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,ZoneCode,[BranchID],[ECRiderCode],BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID) values(2,@ProductID,@ZoneCode,@BranchID,@EC,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1004);"
                        , new
                        {
                            ProductID = single.ProductId,
                            ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                            BranchID = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                            EC = model.ZoneCode,
                            BarcodeFrm = single.BarcodeFrom,
                            BarcodeTo = single.BarcodeTo,
                            IssueQty = single.IssueQty,
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