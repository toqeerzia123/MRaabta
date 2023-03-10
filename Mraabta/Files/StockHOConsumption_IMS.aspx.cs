using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using MRaabta.Models;
using Dapper;

namespace MRaabta.Files
{
    public partial class StockHOConsumption_IMS : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlDataReader dr;
        DataTable dt = new DataTable();
        static string ZoneCode, ZoneName, BranchCode;
        static string UserType, U_ID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { 
                ZoneCode = Session["ZONECODE"].ToString();
                BranchCode = Session["BRANCHCODE"].ToString();
                U_ID = Session["U_ID"].ToString();
            }
            catch (Exception er)
            {
                Response.Redirect("~/Login");
            }
        }

        #region Getting Zone Stock
        [WebMethod]
        public static List<StockModel> GetZoneStock()
        { 
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"
            select pr.OracleCode, productID,pr.ProductName,t.TypeName,z.name as Zone, 
            sum(qty) as Qty, 
            FORMAT(sum(qty),N'N0') as Qty_  from (
            select ProductID, ZoneCode, 
            case when StatusID = 2 then qty * (-1) 
              when StatusID = 1 then qty else qty * 0 end as qty 
            from Mnp_CNIssue_Stock 
            where UserTypeID = 1003
            ) as xb 
            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
            inner join Zones z on xb.ZoneCode = z.zoneCode
            group by pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,name
            having xb.ZoneCode = @Zone", new { @Zone = 28 });

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

        #region Getting Branch Stock
        [WebMethod]
        public static List<StockModel> GetBranchStock()
        { 
            try
            {
                con.Open(); 

                var rs = con.Query<StockModel>(@"select pr.OracleCode, productID,pr.ProductName,t.TypeName,BranchID,z.name as Zone, 
                        sum(qty) as Qty,
                        FORMAT(sum(qty),N'N0') as Qty_ from (
                     select ProductID, ZoneCode,BranchID, 
                    case when StatusID = 2 then qty * (-1)
                     when StatusID = 1 then qty else qty * (0) end as qty 
                      from Mnp_CNIssue_Stock
                     where UserTypeID = 1004
                     ) as xb
                     inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                     inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                     inner join Zones z on xb.ZoneCode = z.zoneCode
                     inner join Branches b on xb.BranchID = b.branchCode
                     group by  pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,z.name,BranchID
                     having xb.ZoneCode = @Zone  and xb.BranchID = @BranchCode ", new { @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @Zone = 28 });

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

        #region method for product ddl
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetProductsSingle(String Id)
        {
            List<CNIssuanceDropDownModel> dr = new List<CNIssuanceDropDownModel>();
             
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"select p.ID as [Value], p.ProductName as [Text] from [MnP_CNIssue_Product] p where p.TypeId=@id", new { @id = Id });
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

        #region method for type ddl
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetTypeProducts()
        { 
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"select ID as [Value], TypeName as [Text] from MnP_CNIssue_Type;");
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

        #region method for department ddl
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetDepartments()
        {
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"SELECT DepartmentId as [Value], Department  as [Text] FROM department WHERE isActive=1");
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

        #region zoneName from session
        [WebMethod]
        public void GetName()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                var sqlq = "select z.Name, z.zoneCode, b.name, b.branchCode from Zones z inner join Branches b on z.zoneCode = b.zoneCode where z.zone_type = 1 AND z.zoneCode = '" + 28 + "';";
                using (SqlCommand cmd = new SqlCommand(sqlq, con))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt); 
                    Session["dt"] = dt;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        Session["ZoneName"] = dr[0].ToString();
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
        #endregion zoneCode

        #region usertype from session
        [WebMethod]
        public void GetUser()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                var sqlq = "select * from MnP_CNIssue_UserType where UserType = 'Zone';";
                using (SqlCommand cmd = new SqlCommand(sqlq, con))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    Session["dt"] = dt;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        Session["User_ID"] = dr[0].ToString();
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
        #endregion

        public static List<StockModel> GetIssuedRequest()
        { 
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select s.Qty,ty.TypeName,pr.ProductName 
            from MnP_CNIssue_Stock s
            left join MnP_CNIssue_Product pr on pr.ID=s.ProductID
            left join MnP_CNIssue_Type ty on pr.TypeId=ty.ID
            where s.UserTypeID=1003 and s.ZoneCode=@Zone", new { @Zone = 28 });
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

        [WebMethod]
        public static List<StockModel> GetPendingRequestDetails(String Id)
        { 
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"
		    select sreq.Qty,p.ProductName,t.TypeName from MnP_CNIssue_StockReqDetails sreq
		    left join MnP_CNIssue_Product p on p.ID=sreq.ProductID
		    left join MnP_CNIssue_Type t on t.ID=p.TypeId
		    where sreq.StockReqID=@Id", new { @Id = Id });
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

        [WebMethod]
        public static List<StockModel> GetPendingRequest()
        { 
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"Select sr.ID Id,sr.TotalReqQty Qty from MnP_CNIssue_StockRequest sr 
	        where sr.ZoneCode=@Zone  and MONTH(sr.CreatedOn)=MONTH(GETDATE())", new { @Zone = 28 });

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

        #region search requests
        [WebMethod]
        public static Response_StockRequestDetailModel GetFilteredStockZone(String StartDate, String EndDate)
        {
            Response_StockRequestDetailModel resp = new Response_StockRequestDetailModel();
            try
            {
                DateTime Start = DateTime.Parse(StartDate);
                DateTime End = DateTime.Parse(EndDate);
                if ((End - Start).TotalDays > 31)
                {
                    resp.Data = null;
                    resp.status = false;
                    resp.statusMessage = "Maximum 31 days limit";
                    return resp;

                }
                if ((End - Start).TotalDays < 0)
                {
                    resp.Data = null;
                    resp.status = false;
                    resp.statusMessage = "Please provide correct dates";
                    return resp;
                }
                con.Open();

                var rs = con.Query<StockRequestDetailModel>(@"Select  sr.ID ReqId,sr.ECRiderCode DepartmentID, isnull(ml.department,'-') AS DepartmentName,sr.TotalReqQty Qty,  FORMAT(sr.TotalReqQty,N'N0') as Qty_  ,
                CONVERT(VARCHAR(11),Sr.CreatedOn, 106) CreatedOn 
		        from MnP_CNIssue_StockRequest sr 
				LEFT JOIN MnP_CNIssue_StockIssuance sid on sid.StockReqID= sr.ID
                LEFT JOIN Department ml ON cast(ml.departmentid as varchar)= sr.ECRiderCode AND ml.isactive=1
                where sr.ZoneCode=@Zonee and sr.BranchCode=@Branch  and UserTypeID=1008 and sid.ID is null and cast(sr.CreatedOn as date)  between @startt and @endd;
                        ", new { @Branch = HttpContext.Current.Session["BRANCHCODE"].ToString(), @Zonee = 28, @startt = StartDate, @endd = EndDate });

                con.Close();
                resp.Data = rs.ToList();
                resp.status = true;
                resp.statusMessage = "Success";
            }
            catch (SqlException ex)
            {
                con.Close();
                resp.Data = null;
                resp.status = false;
                resp.statusMessage = ex.ToString();
            }
            catch (Exception ex)
            {
                con.Close();
                resp.Data = null;
                resp.status = false;
                resp.statusMessage = ex.ToString();
            }
            return resp;
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

        #region edit request click get details
        [WebMethod]
        public static List<StockRequestDetailModel> GetFilteredStockZonePer(String Id)
        {         
            try
            {
                con.Open();
                var rs = con.Query<StockRequestDetailModel>(@"select pr.OracleCode,sred.ID DetailId,sred.StockReqID ReqId,sred.Qty,ty.TypeName,ty.ID TypeId,pr.ID ProductId,pr.ProductName  Product
                from MnP_CNIssue_StockRequest sr 
				inner join MnP_CNIssue_StockReqDetails sred on sred.StockReqID=sr.ID                
                left join MnP_CNIssue_Product pr on pr.ID=sred.ProductID
                left join MnP_CNIssue_Type ty on pr.TypeId=ty.ID
                where sr.ID=@IDD", new { @IDD = Id });

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

        #region CRUD StockRequest
        [WebMethod]
        public static string ModifyRequest(StockRequestModel model)
        { 
            model.CreatedBy = U_ID;
            model.ZoneCode = ZoneCode;
            model.UserTypeId = UserType;
             
            List<int> qtys = new List<int>();
            con.Open();
            object updateDetails;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    foreach (var v in model.StockRequestDetails)
                    {
                        string sqlQuery = "update MnP_CNIssue_StockReqDetails  set ProductID=@ProductId ,Qty=@Qty,LastModifiedOn=GETDATE(),LastModifiedBy=@U_ID where ID=@DetailId and StockReqID=@Id;";
                        updateDetails = con.Execute(sqlQuery,
                            new
                            {
                                v.ProductId,
                                v.Qty,
                                U_ID = HttpContext.Current.Session["U_ID"].ToString(),
                                v.DetailId,
                                model.Id
                            }, transaction: tran);
                    }

                    string sqlQuery2 = "update MnP_CNIssue_StockRequest set TotalReqQty=@TotalQty,LastModifiedOn=GETDATE(),LastModifiedBy=@U_ID where ID=@Id;";
                    con.Execute(sqlQuery2,
                        new
                        {
                            @TotalQty = model.StockRequestDetails.Sum(x => x.Qty),
                            U_ID = HttpContext.Current.Session["U_ID"].ToString(),
                            model.Id
                        }, transaction: tran);

                    tran.Commit();
                    return "Request Modified Succcessfully";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to modify stock request, Please contact IT for support.";
                }
                finally
                {
                    con.Close();
                }
            }
        }
         
        [WebMethod]
        public static string SaveStockDetails(StockRequestModel model)
        {
            model.CreatedBy = U_ID;
            model.ZoneCode = "28";
            model.UserTypeId = UserType;
            model.BranchCode = BranchCode;

            con.Open();
            object reqDetails;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    var id = con.QueryFirstOrDefault<int>(@"insert into [MnP_Stock_Consumption] ([Year],ZoneCode, BranchCode, UserTypeID, ConsumedTotalQty,isActive,CreatedBy,CreatedOn,ECRiderCode) 
                                                            values(@Year,@zonecode,@BranchCode,1008,@TotalReqQty,1,@CreatedBy,GETDATE(),@ECRiderCode);
                                                        SELECT SCOPE_IDENTITY();", new { @Year = DateTime.Now.Year, @ZoneCode = model.ZoneCode, @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @TotalReqQty = model.StockRequestDetails.Sum(x => x.Qty), @CreatedBy = HttpContext.Current.Session["U_ID"].ToString(), ECRiderCode = model.StockRequestDetails.First().DepartmentID }, transaction: tran);

                    foreach (var singleDetail in model.StockRequestDetails)
                    {
                        reqDetails = con.Execute(@"insert into [MnP_Stock_ConsumptionDetails] (ConsumeID,ProductID, Qty,isActive,CreatedBy,CreatedOn) values(@ConsumeID,@ProductID, @Qty,1,@CreatedBy,GETDATE());",
                        new
                        {
                            ConsumeID = id,
                            ProductID = singleDetail.ProductId,
                            Qty = singleDetail.Qty,
                            CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                        }, transaction: tran);
                    }

                    tran.Commit();
                    return "Consumption saved successfully, Request id: " + id;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to submit stock request, Please contact IT for support.";
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