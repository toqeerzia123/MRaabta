using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using Dapper;
using System.Web;
using MRaabta.Models;

namespace MRaabta.Files
{
    public partial class StockBranchReceiving : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        static String branchCode = "";
        static String ZoneCode = "", ZoneName = "";
        static String U_ID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ZoneCode = Session["ZONECODE"].ToString();
                branchCode = Session["BRANCHCODE"].ToString();
                U_ID = Session["U_ID"].ToString();


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
        #region issuance details
        [WebMethod]
        public static List<StockReceivingModel> GetIssuances()
        {
            // return new StockBranchReceivingRepo().GetIssuancesZone(branchCode,ZoneCode);
            try
            {
                con.Open();
                var rs = con.Query<StockReceivingModel>(@"select SI.ID AS IssuanceID,sr.ID RequestID ,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn ,sr.TotalReqQty, FORMAT(sr.TotalReqQty,N'N0') as TotalReqQty_ ,SI.TotalIssueQty, FORMAT(si.TotalIssueQty,N'N0') as TotalIssueQty_, sr.BranchCode
                                from MnP_CNIssue_StockRequest sr
                                inner join MnP_CNIssue_StockIssuance si on si.StockReqID = sr.ID
                                left join MnP_CNIssue_StockRecieving src on src.StockIssuanceID = si.ID
                                where sr.UserTypeID='1004' and src.id is null and sr.BranchCode=@BranchCode and sr.ZoneCode = @Zone order by sr.CreatedOn", new { @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @Zone = HttpContext.Current.Session["ZONECODE"].ToString() });
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
        public static List<StockReceivingDetailModel> GetIssuancesDetails(int ID)
        {
            // return new StockBranchReceivingRepo().GetIssuanceDetails(ID);

            try
            {
                con.Open();
                var rs = con.Query<StockReceivingDetailModel>
                    (@"Select  id.StockIssuanceID IssuanceID,t.TypeName,p.ProductName,  rd.Qty   RequestQty,
                   FORMAT(rd.Qty,N'N0') as  RequestQty_,       
                   id.IssueQty         RecievingQty,
                   FORMAT(id.IssueQty,N'N0') as RecievingQty_,
                    id.BarcodeFrm BarcodeFrom,id.BarcodeTo,id.ID stockIssuanceDetailID  
                 from MnP_CNIssue_StockReqDetails rd
                left join MnP_CNIssue_StockIssuanceDetail id on id.StockReqDetailID = rd.ID
                inner join MnP_CNIssue_Product p on p.id = rd.ProductID
                inner join MnP_CNIssue_Type t on t.ID=p.TypeId
                where rd.StockReqID = (select StockReqID from MnP_CNIssue_StockIssuance where ID = @Id)", new { @Id = ID });
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

        #region save received stock
        [WebMethod]
        public static string SaveReceivedPerIssued(String ID)
        {
            //return new StockBranchReceivingRepo().SaveReceivedZone(ID, ZoneCode, branchCode, U_ID);
            con.Open();
            object rs;
            object update;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    int UserTypeId;
                    UserTypeId = con.QueryFirst<int>(@"select ut.ID from MnP_CNIssue_UserType ut where ut.UserType='Zone' and ut.isActive=1", transaction: tran);

                    var Issuancemodel = con.QueryFirst<StockIssuanceModel>(@"select * from MnP_CNIssue_StockIssuance si where si.ID=@ID", new { @ID = ID }, transaction: tran);
                    var detailModel = con.Query<StockIssuanceDetailModel>(@"select sid.ID stockIssuanceDetailID,sid.BarcodeFrm BarcodeFrom,sid.BarcodeTo,sid.IssueQty from MnP_CNIssue_StockIssuanceDetail 
	             sid where sid.StockIssuanceID=@IssuanceID", new { @IssuanceID = ID }, transaction: tran);

                    var id = con.QueryFirstOrDefault<int>(@"insert into MnP_CNIssue_StockRecieving(StockIssuanceID,[Year],ZoneCode,[BranchCode],UserTypeID,TotalRecieveQty,isActive,CreatedBy,CreatedOn) 
	                                                    values(@StockIssueID,@Year,@ZoneCode,@BranchCode,@UserTypeID,@TotalReceiveQty,1,@CreatedBy,GETDATE());;
                                         SELECT SCOPE_IDENTITY();", new
                    {
                        @StockIssueID = Issuancemodel.ID,
                        @Year = DateTime.Now.Year,
                        @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                        @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                        @UserTypeID = UserTypeId,
                        @TotalReceiveQty = Issuancemodel.TotalIssueQty,
                        @CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                    }, transaction: tran);

                    foreach (var single in detailModel)
                    {
                        rs = con.Execute(@"insert into MnP_CNIssue_StockRecievingDetail(StockRecievingID,StockIssuanceDetailID,RecievingQty,BarcodeFrm,BarcodeTo,CreatedBy,CreatedOn,isActive) values(@StockRecievingID,@StockIssuanceDetailID,@RecievingQty,@BarcodeFrm,@BarcodeTo,@CreatedBy,GETDATE(),1);"
                         , new
                         {
                             StockRecievingID = id,
                             StockIssuanceDetailID = single.stockIssuanceDetailID,
                             RecievingQty = single.IssueQty,
                             BarcodeFrm = single.BarcodeFrom,
                             BarcodeTo = single.BarcodeTo,
                             CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                         }, transaction: tran);

                        update = con.Execute(@"update MnP_CNIssue_Stock set StatusID = 1, BranchID = " + HttpContext.Current.Session["BRANCHCODE"].ToString() + @"
                                               WHERE barcodeFrm = @BarcodeFrm and BarcodeTo = @BarcodeTo and statusID = 5"
                          , new
                          {
                              BarcodeFrm = single.BarcodeFrom,
                              BarcodeTo = single.BarcodeTo,
                          }, transaction: tran);
                    }

                    tran.Commit();
                    return "Stock Received Successfully";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to recieve issued stock, please contact IT for support";
                }
                finally
                {
                    con.Close();
                }
            }
        }

        [WebMethod]
        public static string SaveReceivingDetails(StockReceivingModel model)
        {
            model.CreatedBy = U_ID;
            model.Year = DateTime.Now.Year;

            //return new StockBranchReceivingRepo().SaveReceivingDetails(model, ZoneCode, branchCode);
            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    int UserTypeId;

                    if (branchCode.Equals("ALL"))
                    {
                        UserTypeId = con.QueryFirst<int>(@"select ut.ID from MnP_CNIssue_UserType ut where ut.UserType='Zone' and ut.isActive=1");
                    }
                    else
                    {
                        UserTypeId = con.QueryFirst<int>(@"select ut.ID from MnP_CNIssue_UserType ut where ut.UserType='Branch' and ut.isActive=1");
                    }

                    var id = con.QueryFirstOrDefault<int>(@"insert into MnP_CNIssue_StockRecieving(StockIssuanceID,[Year],ZoneCode,BranchCode,ECRiderCode,UserTypeID,TotalRecieveQty,isActive,CreatedBy,CreatedOn) 
	                                                    values(@StockIssueID,@Year,@ZoneCode,@BranchCode,@ECRiderCode,@UserTypeID,@TotalReceiveQty,1,@CreatedBy,GETDATE());
                                         SELECT SCOPE_IDENTITY();", new
                    {
                        @StockIssueID = model.IssuanceID,
                        @Year = model.Year,
                        @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                        @BranchCode = branchCode,
                        @ECRiderCode = model.ECRiderCode,
                        @UserTypeID = UserTypeId,
                        @TotalReceiveQty = model.StockReceivingDetails.Sum(x => x.RecievingQty),
                        @CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                    }, transaction: tran);

                    var rs = con.Execute(@"insert into MnP_CNIssue_StockRecievingDetail(StockRecievingID,StockIssuanceDetailID,RecievingQty,BarcodeFrm,BarcodeTo,CreatedBy,CreatedOn,isActive) values(@StockRecievingID,@StockIssuanceDetailID,@RecievingQty,@BarcodeFrm,@BarcodeTo,@CreatedBy,GETDATE(),1);"
                    , model.StockReceivingDetails.Select(x => new
                    {
                        StockRecievingID = id,
                        StockIssuanceDetailID = x.IssuanceDetailID,
                        RecievingQty = x.RecievingQty,
                        BarcodeFrm = x.BarcodeFrom,
                        BarcodeTo = x.BarcodeTo,
                        CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                    }).ToList(), transaction: tran);

                    tran.Commit();
                    return "Request successfully recieved, Request id: " + id;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to recieve issued stock, Please contact IT for support.";
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion

        #region get stock data
        [WebMethod]
        public static List<StockModel> GetMyStock()
        {
            //return new StockBranchReceivingRepo().GetMyStockZone(ZoneCode);

            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,z.name as Zone, sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                                            select ProductID, ZoneCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock
                                            where UserTypeID = 1003
                                            ) as xb
                                            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                            inner join Zones z on xb.ZoneCode = z.zoneCode
                                            group by ProductID, xb.ZoneCode,ProductName,TypeName,name
                                            having xb.ZoneCode = @Zone", new
                {
                    @Zone = HttpContext.Current.Session["ZONECODE"].ToString()
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
        public static List<StockModel> GetHOStock()
        {
            //return new StockBranchReceivingRepo().GetHOStock(ZoneCode, branchCode);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,BranchID,z.name as Zone, sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                                            select ProductID, ZoneCode,BranchID, 
											case when StatusID = 2 then qty * (-1) 
											when StatusID = 1 then qty else qty * 0 end as qty from Mnp_CNIssue_Stock
                                            where UserTypeID = 1004
                                            ) as xb
                                            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                            inner join Zones z on xb.ZoneCode = z.zoneCode
                                            inner join Branches b on xb.BranchID = b.branchCode
                                            group by ProductID, xb.ZoneCode,ProductName,TypeName,z.name,BranchID
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

        #endregion

        #region recieved requests
        [WebMethod]
        public static List<StockReceivingModel> getReceivedRequest()
        {
            // return new StockBranchReceivingRepo().getReceivedRequest(ZoneCode);

            try
            {
                con.Open();
                var rs = con.Query<StockReceivingModel>(@"select sr.id,z.name as ZoneName,sr.TotalRecieveQty ,sreq.TotalReqQty from MnP_CNIssue_StockRecieving sr
                inner join zones z on z.zoneCode =  sr.ZoneCode
                left join MnP_CNIssue_StockIssuance si on sr.StockIssuanceID = si.id
                left join MnP_CNIssue_StockRequest sreq on si.StockReqID = sreq.id 
                where sr.ZoneCode = @ZoneCode and Month(sr.CreatedOn)=Month(GETDATE())", new
                {
                    @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString()
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
        public static List<StockReceivingDetailModel> getReceivedRequestDetails(int id)
        {
            //return new StockBranchReceivingRepo().getReceivedRequestDetails(id);
            try
            {
                con.Open();
                var rs = con.Query<StockReceivingDetailModel>(@"select pr.ProductName,prt.Name as TypeName,sreqd.Qty as RequestQty,srd.RecievingQty from MnP_CNIssue_StockRecievingDetail srd 
            left join MnP_CNIssue_StockIssuanceDetail sid on srd.StockIssuanceDetailID = sid.id
            left join MnP_CNIssue_StockReqDetails sreqd on sid.StockReqDetailID = sreqd.id
            inner join MnP_CNIssue_Product pr on sreqd.ProductID = pr.ID
            inner join MNP_CN_Doc_Type prt on prt.id = pr.TypeId
            where srd.StockRecievingID = @ID ", new { ID = id });
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
    }
}