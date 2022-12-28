using Dapper;
using MRaabta.Models; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class StockIssuanceHODept_IMS : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlDataReader dr;
        DataTable dt = new DataTable();
        static String branchCode = "";
        static String ZoneCode = "", U_ID = "";
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

        #region Get Branch
        public void GetBranch()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                var sqlq = "select branchCode,name as BranchName from Branches";
                using (SqlCommand cmd = new SqlCommand(sqlq, con))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    //ViewState["dt"] = dt;
                    Session["dt"] = dt;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        Session["BranchId"] = dr[0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetBranches()
        {
            //return new StockBranchIssuanceRepo().GetBranches(ZoneCode);
            try
            {
                con.Open();

                var rs = con.Query<CNIssuanceDropDownModel>(@"select branchCode as [Value], name as [Text] from branches where zoneCode=28 AND [status]=1", new { @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString() });
                return rs.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get Branch Requests
        [WebMethod]
        public static List<StockRequestModel> GetRequests(int? id, string startdate, string enddate)
        {
            //return new StockBranchIssuanceRepo().GetRequestsZone(id, ZoneCode);
            try
            {
                con.Open();
                if (id > 0 && (startdate != "" || startdate != null) && (enddate != "" || enddate != null))
                {
                    var rs = con.Query<StockRequestModel>(@"select pr.OracleCode,sr.ID as [Id], sr.BranchCode,b.name as BranchName, sr.ECRiderCode DepartmentID, isnull(ml.department,'-') AS DepartmentName,b.branchCode, 
				                                        z.zoneCode , sr.Year,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn , sr.TotalReqQty,FORMAT(sr.TotalReqQty,N'N0') TotalReqQty_, 
				                                        mu.name Unit,case when si.StockReqID is null then 0 else 1 end as [IsUpdated]  
                                                        from [MnP_CNIssue_StockRequest] sr
                                                        inner join Zones z on sr.ZoneCode= z.zoneCode
                                                        inner join Branches b on sr.BranchCode= b.branchCode
														inner join MnP_CNIssue_StockReqDetails srd on srd.StockReqID=sr.id
														inner join MnP_CNIssue_Product pr on pr.ID=srd.ProductID
														inner join Mnp_CnIssue_MeasurementUnit mu on mu.ID=pr.Unit		        
                                                        left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                                                        LEFT JOIN department ml ON ml.departmentid= sr.ECRiderCode AND ml.isactive=1
                                                        where sr.isActive = 1  AND si.ID is null AND sr.TotalReqQty > 0 and sr.BranchCode=@BranchCode and sr.UserTypeID='1008'   and cast(sr.createdon as date) between isnull(@startdate,sr.createdon) and isnull(@enddate,sr.createdon) ", new { @BranchCode = id , @startdate = startdate, @enddate = enddate });
                    return rs.ToList();
                }
                else
                {
                    var rs = con.Query<StockRequestModel>(@"select mu.name Unit, pr.OracleCode, sr.ID as [Id],z.name as BranchName,sr.ECRiderCode DepartmentID, isnull(ml.department,'-') AS DepartmentName,sr.branchCode,sr.zoneCode, sr.TotalReqQty,FORMAT(sr.TotalReqQty,N'N0') TotalReqQty_,CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn, case when si.StockReqID is null then 0 else 1 end as [IsUpdated]  
                    from [MnP_CNIssue_StockRequest] sr
                    inner join Branches z on sr.BranchCode= z.branchCode
                    inner join MnP_CNIssue_StockReqDetails srd on srd.StockReqID=sr.id
					inner join MnP_CNIssue_Product pr on pr.ID=srd.ProductID
					inner join Mnp_CnIssue_MeasurementUnit mu on mu.ID=pr.Unit		                                                                
                    left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                    LEFT JOIN department ml ON ml.departmentid= sr.ECRiderCode AND ml.isactive=1                                                       
                    where sr.isActive = 1  AND si.ID is null AND sr.TotalReqQty > 0 and sr.ZoneCode=28 and sr.UserTypeID=1008  and cast(sr.createdon as date) between isnull(@startdate,sr.createdon) and isnull(@enddate,sr.createdon) ", new { @Zone = HttpContext.Current.Session["ZONECODE"].ToString(), @startdate = startdate, @enddate = enddate });
                    return rs.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        #endregion

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


        #region get Stock 
        [WebMethod]
        public static List<StockModel> GetMyStock()
        {
            // return new StockBranchIssuanceRepo().GetMyStock(branchCode, ZoneCode);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"
            select pr.OracleCode, productID,pr.ProductName,t.TypeName,z.name as Zone, sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
            select ProductID, ZoneCode, 
            case when StatusID = 2 then qty * (-1) 
              when StatusID = 1 then qty else qty * 0 end as qty 
            from Mnp_CNIssue_Stock 
            where UserTypeID = 1003
            ) as xb 
            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
            inner join Zones z on xb.ZoneCode = z.zoneCode
            group by  pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,name
            having xb.ZoneCode =28", new { @Zone = HttpContext.Current.Session["ZONECODE"].ToString() });
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
        public static List<StockModel> GetHODeptStock(string zone, string branch,string dept)
        {
             try
            {
                con.Open();
                
                var rs = con.Query<StockModel>(@"select pr.OracleCode,productID,pr.ProductName,t.TypeName,BranchID,d.department BranchName,z.name as Zone, sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
select ProductID, ZoneCode,BranchID, ecridercode,
case when StatusID = 2 then qty * (-1)
when StatusID = 1 then qty else qty * (0) end as qty 
from Mnp_CNIssue_Stock
where UserTypeID = 1008 and ECRiderCode=@Dept
) as xb
inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
inner join Zones z on xb.ZoneCode = z.zoneCode
inner join Branches b on xb.BranchID = b.branchCode
inner join department d on d.departmentid=xb.ECRiderCode
group by d.name, pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,z.name,BranchID
having xb.ZoneCode =28  and xb.BranchID = @BranchCode ", new { @BranchCode = branch, @Zone = zone, @Dept= dept });
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
            //return new StockBranchIssuanceRepo().GetZoneStock(zone);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,z.name as Zone, sum(qty) as Qty ,FORMAT(sum(qty),N'N0') as Qty_  from (
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
                                                having xb.ZoneCode =28;	", new { @Zone = zone });
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

        [WebMethod]
        public static List<PreviousIssuanceModel> GetPreviousIssuance()
        {
            //return new StockBranchIssuanceRepo().GetPreviousIssuanceZone();
            try
            {
                con.Open();
                var rs = con.Query<PreviousIssuanceModel>(@"select si.ID as [Id], z.name as BranchName ,si.StockReqID, sr.TotalReqQty, si.TotalIssueQty from [MnP_CNIssue_StockIssuance] si
                                                        left join [MnP_CNIssue_StockRequest] sr on sr.ID = si.StockReqID 
                                                        inner join Branches z on sr.BranchCode= z.branchCode
                                                        where sr.isActive = 1 and MONTH(si.CreatedOn)=Month(GETDATE());");
                //var rs = con.Query<PreviousIssuanceModel>(@"select si.ID as [Id], z.name as ZoneCode ,si.StockReqID, sr.TotalReqQty, si.TotalIssueQty from [MnP_CNIssue_StockIssuance] si
                //                                            left join [MnP_CNIssue_StockRequest] sr on sr.ID = si.StockReqID 
                //                                            inner join Zones z on z.zoneCode = sr.ZoneCode
                //                                            where sr.isActive = 1 and MONTH(si.CreatedOn)=Month(GETDATE());");
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

        #region Request Details
        [WebMethod]
        public static List<StockRequestDetailModel> GetRequestsDetails(int id)
        {
             try
            {
                con.Open();
                
                var rs = con.Query<StockRequestDetailModel>(@"select xb.OracleCode, xb.ProductID, xb.TypeName,xb.ID as Id,xb.ProductName,xb.Qty,yb.BarcodeIssueFrom,yb.QtyIssueFrom,yb.Qty [TotalSequence] from (
                    select pr.OracleCode, srd.ID,srd.ProductID,pr.ProductName,t.TypeName,srd.Qty from MnP_CNIssue_StockReqDetails srd 
                    inner join MnP_CNIssue_StockRequest sr on sr.ID = srd.StockReqID
                    inner join MnP_CNIssue_Product pr on pr.ID = srd.ProductID
                    inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId 
                    where StockReqID = @Id) as xb
                    LEFT join 
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
                    --ZoneCode = (select ZoneCode from MnP_CNIssue_StockRequest where ID = @Id) and 
                    UserTypeID = 1002 and BarcodeFrm between s.barcodeFrm and s.BarcodeTo) as xb 
                    ) as QtyIssueFrom
                    from Mnp_CNIssue_Stock s 
                    where s.UserTypeID =  1002 --and s.ProductID = 1
                   -- and ZoneCode = (select ZoneCode from MnP_CNIssue_StockRequest where ID = @Id)
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
            con.Open();
            object rs;
            object stockTransit;
            object stockIssue;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    var id = con.QueryFirstOrDefault<int>(@"insert into MnP_CNIssue_StockIssuance (StockReqID,[Year],ZoneCode,BranchCode,TotalIssueQty,isActive,CreatedBy,CreatedOn,UserType,FOCNumber,Remarks,ECRiderCode) values(@StockReqID,@Year,28,@BranchCode,@TotalIssueQty,1,@CreatedBy,GETDATE(),1008, @FOCNumber,@Remarks,@ECRiderCode);
                                                        SELECT SCOPE_IDENTITY();", new { @StockReqID = model.StockRequestId, @Year = model.Year, @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(), @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @TotalIssueQty = model.StockIssuanceDetails.Sum(x => x.IssueQty), @CreatedBy = HttpContext.Current.Session["U_ID"].ToString(), @FOCNumber = model.FOCNumber != "" ? model.FOCNumber : "", @Remarks = model.Remarks != "" ? model.Remarks : "", @ECRiderCode= model.DepartmentID }, transaction: tran);

                 
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
                                  CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                                  ProductID = single.ProductId,
                                  BranchID = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                                  ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                                  newbarcodefrom = single.BarcodeTo + 1,
                                  newQty = single.QtyIssueFrom - single.IssueQty
                              }
                              , transaction: tran);

                        stockTransit = con.Execute(@" insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,BranchID,ZoneCode,BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID,ECRiderCode) values(5,@ProductID,@BranchID,28,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1008,@ECRiderCode);"
                              , new
                              {
                                  StockIssuanceID = id,
                                  StockReqDetailID = single.StockRequestDetailId,
                                  ECRiderCode = model.DepartmentID,
                                  IssueQty = single.IssueQty,
                                  BarcodeFrm = single.BarcodeFrom,
                                  BarcodeTo = single.BarcodeTo,
                                  CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                                  ProductID = single.ProductId,
                                  BranchID = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                                  ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                                  newbarcodefrom = single.BarcodeTo + 1,
                                  newQty = single.QtyIssueFrom - single.IssueQty
                              }, transaction: tran);

                        stockIssue = con.Execute(@"insert into [Mnp_CNIssue_Stock] (StatusID,ProductID,BranchID,ZoneCode,BarcodeFrm,BarcodeTo,Qty,EntryDate,UserTypeID) values(2,@ProductID,@BranchID,28,@BarcodeFrm,@BarcodeTo,@IssueQty,GETDATE(),1002);"
                              , new
                              {
                                  StockIssuanceID = id,
                                  StockReqDetailID = single.StockRequestDetailId,
                                  IssueQty = single.IssueQty,
                                  BarcodeFrm = single.BarcodeFrom,
                                  BarcodeTo = single.BarcodeTo,
                                  CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                                  ProductID = single.ProductId,
                                  BranchID = HttpContext.Current.Session["BRANCHCODE"].ToString(),
                                  ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(),
                                  newbarcodefrom = single.BarcodeTo + 1,
                                  newQty = single.QtyIssueFrom - single.IssueQty
                              }, transaction: tran);
                    }


                    tran.Commit();
                    return "Successfully issued stock to this request";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to issue stock to this request, Please contact IT for support.";
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