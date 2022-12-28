using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class StockIssuanceAccount_IMS : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        static String BranchCode = "";
        static String ZoneCode = "", U_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ZoneCode = Session["ZONECODE"].ToString();
                BranchCode = Session["BRANCHCODE"].ToString();
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
        public static List<CNIssuanceDropDownModel> GetAccount()
        {
            try
            {
                con.Open();

                var rs = con.Query<CNIssuanceDropDownModel>(@"SELECT accountNo as [Value],cc.accountNo+' - ' +cc.name as [Text] 
from CreditClients cc
WHERE cc.branchCode = @branchCode and status = '1' ;", new { @branchCode = HttpContext.Current.Session["BRANCHCODE"].ToString() });
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

        #region Requests and stock details
        [WebMethod]
        public static List<StockRequestModel> GetRequests(string id, string startdate, string enddate)
        {
            //return new StockIssuanceRiderRepo().GetRequestsZone(id,BranchCode);
            try
            {
                con.Open();

                if ((id != "-1" && id!="") && (startdate != "" || startdate != null) && (enddate != "" || enddate != null))
                {
                    var rs = con.Query<StockRequestModel>(@"select sr.ID as [Id],sr.ECRiderCode as ZoneCode, sr.ECRiderCode+' - '+cc.name  as ZoneName,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn , sr.TotalReqQty,FORMAT(sr.TotalReqQty,N'N0') as TotalReqQty_,  case when si.StockReqID is null then 0 else 1 end as [IsUpdated]    
                                                        from [MnP_CNIssue_StockRequest] sr
                                                        inner join Zones z on sr.ZoneCode= z.ZoneCode
                                                        INNER JOIN CreditClients cc ON cc.accountNo=sr.ECRiderCode AND sr.ZoneCode=cc.zoneCode AND sr.BranchCode=cc.branchCode
                                                        left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                                                        where sr.isActive = 1  
                                                              AND si.ID is null 
                                                                    AND sr.TotalReqQty > 0  AND sr.ECRiderCode=@AccountNo and sr.BranchCode = @BranchCode and sr.UserTypeID = '1007'  and cast(sr.createdon as date) between isnull(@startdate,sr.createdon) and isnull(@enddate,sr.createdon)  ORDER BY sr.CreatedOn desc", new
                    {
                        @AccountNo = id,
                        @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                        @startdate = startdate,
                        @enddate = enddate
                    });
                    con.Close();
                    return rs.ToList();
                }
                else if((startdate != "" || startdate != null) && (enddate != "" || enddate != null))
                {
                    var rs = con.Query<StockRequestModel>(@"select sr.ID as [Id],sr.ECRiderCode as ZoneCode, sr.ECRiderCode+' - '+cc.name  as ZoneName,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn , sr.TotalReqQty,FORMAT(sr.TotalReqQty,N'N0') as TotalReqQty_,  case when si.StockReqID is null then 0 else 1 end as [IsUpdated]    
                                                        from [MnP_CNIssue_StockRequest] sr
                                                        inner join Zones z on sr.ZoneCode= z.ZoneCode
                                                        INNER JOIN CreditClients cc ON cc.accountNo=sr.ECRiderCode AND sr.ZoneCode=cc.zoneCode AND sr.BranchCode=cc.branchCode
                                                        left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                                                        where sr.isActive = 1  
                                                              AND si.ID is null 
                                                                    AND sr.TotalReqQty > 0  and sr.BranchCode = @BranchCode and sr.UserTypeID = '1007'  and cast(sr.createdon as date) between isnull(@startdate,sr.createdon) and isnull(@enddate,sr.createdon)  ORDER BY sr.CreatedOn desc", new
                    {
                        @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                        @startdate = startdate,
                        @enddate = enddate
                    });
                    con.Close();
                    return rs.ToList();
                }
                else
                {
                    var rs = con.Query<StockRequestModel>(@"select sr.ID as [Id],sr.ECRiderCode as ZoneCode,  sr.ECRiderCode+' - '+cc.name as ZoneName,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn , sr.TotalReqQty, FORMAT(sr.TotalReqQty,N'N0') as TotalReqQty_, case when si.StockReqID is null then 0 else 1 end as [IsUpdated]    
                                                        from [MnP_CNIssue_StockRequest] sr
                                                        inner join Zones z on sr.ZoneCode= z.ZoneCode
                                                        INNER JOIN CreditClients cc ON cc.accountNo=sr.ECRiderCode AND sr.ZoneCode=cc.zoneCode AND sr.BranchCode=cc.branchCode
                                                        left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                                                        where sr.isActive = 1  
                                                            AND si.ID is null 
                                                                AND sr.TotalReqQty > 0  and sr.BranchCode = @BranchCode and sr.UserTypeID = '1005'  ORDER BY sr.CreatedOn desc", new
                    {
                        @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(),
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
            //return new StockIssuanceRiderRepo().GetMyStock(ZoneCode,BranchCode);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select pr.OracleCode, productID,pr.ProductName,t.TypeName,BranchID,z.name as Zone, 
                                            sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                                            select ProductID, ZoneCode,BranchID,  CASE WHEN StatusID = 2 THEN qty * (-1) WHEN StatusID = 1 THEN qty ELSE qty * 0  END AS qty from Mnp_CNIssue_Stock 
                                            where UserTypeID = 1004
                                            ) as xb 
                                            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                            inner join Zones z on xb.ZoneCode = z.zoneCode
                                            inner join Branches b on xb.BranchID = b.branchCode
                                            group by pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,z.name,BranchID
                                            having xb.ZoneCode = @ZoneCode and xb.BranchID = @BranchCode", new
                {
                    @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                    @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString()
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
        public static List<StockModel> GetZoneStock(string zone)
        {
            // return new StockIssuanceRiderRepo().GetZoneStock(ZoneCode,BranchCode,zone);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select pr.OracleCode,productID,pr.ProductName,t.TypeName,xb.BranchID,xb.ECRiderCode,z.name as Zone,
                                            sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                                            select ProductID, ZoneCode,BranchID,ECRiderCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock 
                                            where UserTypeID = 1007
                                            ) as xb 
                                            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                            inner join Zones z on xb.ZoneCode = z.zoneCode
                                            inner join Branches b on xb.BranchID = b.branchCode
                                            INNER JOIN CreditClients cc ON cc.accountNo=xb.ECRiderCode AND xb.ZoneCode=cc.zoneCode AND xb.BranchID=cc.branchCode
                                            group by pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,z.name,xb.BranchID,xb.ECRiderCode
                                            having xb.ZoneCode = @ZoneCode and xb.BranchID = @BranchCode and xb.ECRiderCode = @Rider ", new { @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(), @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @Rider = zone });
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

                var rs = con.Query<StockRequestDetailModel>(@"select xb.OracleCode, xb.ProductID, xb.TypeName,xb.ID as Id,xb.ProductName,FORMAT(xb.Qty,N'N0') as Qty_,xb.Qty,yb.BarcodeIssueFrom,yb.QtyIssueFrom, CASE WHEN xb.isSerialized=0 THEN 0 else yb.Qty end [TotalSequence] 
  from (
                    select pr.OracleCode, srd.ID,srd.ProductID,pr.ProductName,t.TypeName,srd.Qty, t.isSerialized
                      from MnP_CNIssue_StockReqDetails srd 
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
        #endregion

        #region 

        [WebMethod]
        public static string SaveIssuanceDetails(StockIssuanceModel model)
        {
            model.CreatedBy = HttpContext.Current.Session["U_ID"].ToString();
            model.Year = DateTime.Now.Year;
            model.BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString();
            model.ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString();
            //return new StockIssuanceRiderRepo().SaveIssuanceDetails(model);
            foreach (var item in model.StockIssuanceDetails)
            {
                if (item.BarcodeTo == "" || item.BarcodeTo == null)
                {
                    return "Please provide correct quantity!";
                }
            }
            object rs;
            object insertStocktransit;
            object insertStockIssue;
            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    var id = con.QueryFirstOrDefault<int>(@"insert into MnP_CNIssue_StockIssuance (StockReqID,[Year],ZoneCode,BranchCode,ECRiderCode,TotalIssueQty,isActive,CreatedBy,CreatedOn,UserType,Remarks) values(@StockReqID,@Year,@ZoneCode,@BranchCode,@ECRiderCode,@TotalIssueQty,1,@CreatedBy,GETDATE(),1007,@Remarks);
                                                        SELECT SCOPE_IDENTITY();", new { @StockReqID = model.StockRequestId, @Year = DateTime.Now.Year, @ZoneCode = model.ZoneCode, @BranchCode = model.BranchCode, @ECRiderCode = model.ECRiderCode, @TotalIssueQty = model.StockIssuanceDetails.Sum(x => x.IssueQty), @CreatedBy = model.CreatedBy, @Remarks= model.Remarks==null?"":model.Remarks }, transaction: tran);

                    foreach (var single in model.StockIssuanceDetails)
                    {

                        rs = con.Execute(@"insert into MnP_CNIssue_StockIssuanceDetail (StockIssuanceID,StockReqDetailID,IssueQty,BarcodeFrm,BarcodeTo,CreatedBy,CreatedOn,isActive) values(@StockIssuanceID,@StockReqDetailID,@IssueQty,@BarcodeFrm,@BarcodeTo,@CreatedBy,GETDATE(),1);
                                  ", new
                        {
                            StockIssuanceID = id,
                            StockReqDetailID = single.StockRequestDetailId,
                            IssueQty = single.IssueQty,
                            BarcodeFrm = single.BarcodeFrom,
                            BarcodeTo = single.BarcodeTo,
                            CreatedBy = model.CreatedBy,
                            ProductID = single.ProductId,
                            ZoneCode = model.ZoneCode,
                            newbarcodefrom = single.BarcodeTo + 1,
                            newQty = single.QtyIssueFrom - single.IssueQty,
                            BranchCode = model.BranchCode,
                            ECRiderCode = model.ECRiderCode
                        }, transaction: tran);


                        insertStocktransit = con.Execute(@" insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,ZoneCode,BranchID,ECRiderCode,BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID) values(1,@ProductID,@ZoneCode,@BranchCode,@ECRiderCode,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1007);
                                  ", new
                        {
                            StockIssuanceID = id,
                            StockReqDetailID = single.StockRequestDetailId,
                            IssueQty = single.IssueQty,
                            BarcodeFrm = single.BarcodeFrom,
                            BarcodeTo = single.BarcodeTo,
                            CreatedBy = model.CreatedBy,
                            ProductID = single.ProductId,
                            ZoneCode = model.ZoneCode,
                            newbarcodefrom = single.BarcodeTo + 1,
                            newQty = single.QtyIssueFrom - single.IssueQty,
                            BranchCode = model.BranchCode,
                            ECRiderCode = model.ECRiderCode
                        }, transaction: tran);


                        insertStockIssue = con.Execute(@"insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,ZoneCode,BranchID,ECRiderCode,BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID) values(2,@ProductID,@ZoneCode,@BranchCode,@ECRiderCode,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1004);
                                 ", new
                        {
                            StockIssuanceID = id,
                            StockReqDetailID = single.StockRequestDetailId,
                            IssueQty = single.IssueQty,
                            BarcodeFrm = single.BarcodeFrom,
                            BarcodeTo = single.BarcodeTo,
                            CreatedBy = model.CreatedBy,
                            ProductID = single.ProductId,
                            ZoneCode = model.ZoneCode,
                            newbarcodefrom = single.BarcodeTo + 1,
                            newQty = single.QtyIssueFrom - single.IssueQty,
                            BranchCode = model.BranchCode,
                            ECRiderCode = model.ECRiderCode
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