using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using MRaabta.Models;

namespace MRaabta.Files
{
    public partial class StockReceivingEC_IMS : System.Web.UI.Page
    {
        static String ZoneCode = "", ZoneName = "", ExpressCenter, BranchCode;
        static String U_ID = "";
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

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

        #region issuance details
        [WebMethod]
        public static List<StockReceivingModel> GetIssuances()
        {
            // return new StockReceivingECRepo().GetIssuancesZone(ZoneCode,BranchCode,ExpressCenter);

            try
            {
                con.Open();
                var rs = con.Query<StockReceivingModel>(@"select SI.ID AS IssuanceID,sr.ID RequestID ,   CONVERT(VARCHAR(11),                   
            Sr.CreatedOn, 106) CreatedOn ,sr.TotalReqQty ,FORMAT(sr.TotalReqQty,N'N0') as TotalReqQty_,           SI.TotalIssueQty,   FORMAT(si.TotalIssueQty,N'N0') as TotalIssueQty_
            from MnP_CNIssue_StockRequest sr
            inner join MnP_CNIssue_StockIssuance si on si.StockReqID = sr.ID
            left join MnP_CNIssue_StockRecieving src on src.StockIssuanceID = si.ID
            where src.id is null and sr.ZoneCode=@Zonee and sr.BranchCode=@Branch and sr.ECRiderCode=@EC and sr.UserTypeID=1006 order by sr.CreatedOn", new { @Zonee = HttpContext.Current.Session["ZONECODE"].ToString(), @Branch = HttpContext.Current.Session["BRANCHCODE"].ToString(), @EC = HttpContext.Current.Session["ExpressCenter"].ToString() });
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
            //return new StockReceivingECRepo().GetIssuanceDetails(ID);

            try
            {
                con.Open();
                var rs = con.Query<StockReceivingDetailModel>
                    (@"Select p.OracleCode, id.StockIssuanceID IssuanceID,t.TypeName,p.ProductName,rd.Qty RequestQty,id.IssueQty RecievingQty,id.BarcodeFrm BarcodeFrom,id.BarcodeTo,id.ID stockIssuanceDetailID  
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

        #region insert stock details
        [WebMethod]
        public static string SaveReceivedPerIssued(String ID)
        {
            //return new StockReceivingECRepo().SaveReceivedZone(ID, ZoneCode, BranchCode,ExpressCenter, U_ID);
            object InsertInStockReceiving;
            object UpdateStockStatus;
            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    var Issuancemodel = con.QueryFirst<StockIssuanceModel>(@"select * from MnP_CNIssue_StockIssuance si where si.ID=@ID", new { @ID = ID }, transaction: tran);

                    var IssuancedetailModel = con.Query<StockIssuanceDetailModel>(@"select sid.ID stockIssuanceDetailID,sid.BarcodeFrm BarcodeFrom,sid.BarcodeTo,sid.IssueQty from MnP_CNIssue_StockIssuanceDetail sid where sid.StockIssuanceID=@IssuanceID", new { @IssuanceID = ID }, transaction: tran);

                    var id = con.QueryFirstOrDefault<int>(@"insert into MnP_CNIssue_StockRecieving(StockIssuanceID,[Year],ZoneCode,BranchCode,ECRiderCode,UserTypeID,TotalRecieveQty,isActive,CreatedBy,CreatedOn) 
	                                                    values(@StockIssueID,@Year,@ZoneCode,@BranchCode,@ECRiderCode,1006,@TotalReceiveQty,1,@CreatedBy,GETDATE());;
                                         SELECT SCOPE_IDENTITY();", new { @StockIssueID = Issuancemodel.ID, @Year = Issuancemodel.Year, @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(), @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @ECRiderCode = HttpContext.Current.Session["ExpressCenter"].ToString(), @TotalReceiveQty = Issuancemodel.TotalIssueQty, @CreatedBy = HttpContext.Current.Session["U_ID"].ToString() }, transaction: tran);

                    foreach (var single in IssuancedetailModel)
                    {
                        InsertInStockReceiving = con.Execute(@"insert into MnP_CNIssue_StockRecievingDetail(StockRecievingID,StockIssuanceDetailID,RecievingQty,BarcodeFrm,BarcodeTo,CreatedBy,CreatedOn,isActive) values(@StockRecievingID,@StockIssuanceDetailID,@RecievingQty,@BarcodeFrm,@BarcodeTo,@CreatedBy,GETDATE(),1);"
                       , new
                       {
                           StockRecievingID = id,
                           StockIssuanceDetailID = single.stockIssuanceDetailID,
                           RecievingQty = single.IssueQty,
                           BarcodeFrm = single.BarcodeFrom,
                           BarcodeTo = single.BarcodeTo,
                           CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                       }, transaction: tran);

                        UpdateStockStatus = con.Execute(@"Update MnP_CNIssue_Stock set StatusID = 1 WHERE barcodeFrm = @BarcodeFrm and BarcodeTo = @BarcodeTo and statusID = 5"
                       , new
                       {
                           BarcodeFrm = single.BarcodeFrom,
                           BarcodeTo = single.BarcodeTo,
                       }, transaction: tran);
                    }

                    tran.Commit();
                    return "Stock received successfully";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to receive stock, please contact IT for support";
                }
                finally
                {
                    con.Close();
                }
            }
        }
        #endregion

        //[WebMethod]
        //public static string SaveReceivingDetails(StockReceivingModel model)
        //{
        //    model.CreatedBy = U_ID;
        //    model.Year = DateTime.Now.Year;
        //    return new StockReceivingECRepo().SaveReceivingDetails(model, ZoneCode, BranchCode);
        //}

        #region my stock zone

        [WebMethod]
        public static List<StockModel> GetMyStock()
        {
            //return new StockReceivingECRepo().GetMyStockZone(ZoneCode,BranchCode,ExpressCenter);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"Select pr.OracleCode, productID,pr.ProductName,t.TypeName,xb.BranchID,xb.ECRiderCode,z.name as Zone, 
                sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                select ProductID, ZoneCode,BranchID,ECRiderCode, case when StatusID = 2 then qty * (-1) 
                  when StatusID = 1 then qty else qty * 0 end as qty from Mnp_CNIssue_Stock
                where UserTypeID = 1006
                ) as xb
                inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                inner join Zones z on xb.ZoneCode = z.zoneCode
                inner join Branches b on xb.BranchID = b.branchCode
                --inner join Riders r on r.riderCode = xb.ECRiderCode and r.branchId = xb.BranchID
                group by pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,z.name,xb.BranchID,xb.ECRiderCode
                having xb.ZoneCode = @Zone and xb.BranchID = @Branch and xb.ECRiderCode = @EC", new
                {
                    @Zone = HttpContext.Current.Session["ZONECODE"].ToString(),
                    @Branch = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                    @EC = HttpContext.Current.Session["ExpressCenter"].ToString()
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

        #region HO stock details
        [WebMethod]
        public static List<StockModel> GetHOStock()
        {
            //return new StockReceivingECRepo().GetHOStock(ZoneCode,BranchCode);

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
                    having xb.BranchID = @Branch", new { @Branch = HttpContext.Current.Session["BRANCHCODE"].ToString() });
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

        #region received request details
        [WebMethod]
        public static List<StockReceivingModel> getReceivedRequest()
        {
            //return new StockReceivingECRepo().getReceivedRequest(ZoneCode);
            try
            {
                con.Open();
                var rs = con.Query<StockReceivingModel>(@"select sr.id,z.name as ZoneName,sr.TotalRecieveQty ,sreq.TotalReqQty from MnP_CNIssue_StockRecieving sr
                inner join zones z on z.zoneCode =  sr.ZoneCode
                left join MnP_CNIssue_StockIssuance si on sr.StockIssuanceID = si.id
                left join MnP_CNIssue_StockRequest sreq on si.StockReqID = sreq.id 
                where sr.ZoneCode = @ZoneCode and Month(sr.CreatedOn)=Month(GETDATE())", new { @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString() });
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
            // return new StockReceivingECRepo().getReceivedRequestDetails(id);
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