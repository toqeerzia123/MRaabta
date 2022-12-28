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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class StockRequestRider : System.Web.UI.Page
    {

        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlDataReader dr;
        DataTable dt = new DataTable();
        static string ZoneCode, ZoneName;
        static string BranchCode, BranchName;
        static string UserType, U_ID;

        [WebMethod]
        public static List<StockModel> GetHOStock()
        {
            //return new StockIssuanceRiderRepo().GetMyStock(ZoneCode, BranchCode);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,BranchID,z.name as Zone, 
                                        sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                                            select ProductID, ZoneCode,BranchID, CASE WHEN StatusID = 2 THEN qty * (-1) WHEN StatusID = 1 THEN qty ELSE qty * 0  END AS qty  from Mnp_CNIssue_Stock 
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
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetRider()
        {
            //return new StockRequestRiderRepo().GetRider(BranchCode);
            try
            {
                con.Open();

                var rs = con.Query<CNIssuanceDropDownModel>(@"select riderCode as [Value],(riderCode+' - ' + firstName +' - '+ isnull(hrs_Code,0) ) as [Text] from Riders where branchId = @branchCode and status = '1'", new { @branchCode = HttpContext.Current.Session["BRANCHCODE"].ToString() });
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
            //return new StockRequestRiderRepo().GetZoneStock(ZoneCode, BranchCode, zone);
            var RiderCode = zone;
            try
            {
                con.Open();
                if (RiderCode == "0")
                {
                    var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,xb.BranchID,xb.ECRiderCode,z.name as Zone, 
                                        sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                                            select ProductID, ZoneCode,BranchID,ECRiderCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock 
                                            where UserTypeID = 1005
                                            ) as xb 
                                            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                            inner join Zones z on xb.ZoneCode = z.zoneCode
                                            inner join Branches b on xb.BranchID = b.branchCode
                                            inner join Riders r on r.riderCode = xb.ECRiderCode and r.branchId = xb.BranchID
                                            group by ProductID, xb.ZoneCode,ProductName,TypeName,z.name,xb.BranchID,xb.ECRiderCode
                                            having xb.ZoneCode = @ZoneCode and xb.BranchID = @BranchCode ", new { @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(), @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString() });
                    con.Close();
                    return rs.ToList();
                }
                else
                {
                    var rs = con.Query<StockModel>(@"select productID,pr.ProductName,t.TypeName,xb.BranchID,xb.ECRiderCode,z.name as Zone, 
                                            sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                                            select ProductID, ZoneCode,BranchID,ECRiderCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock 
                                            where UserTypeID = 1005
                                            ) as xb 
                                            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                            inner join Zones z on xb.ZoneCode = z.zoneCode
                                            inner join Branches b on xb.BranchID = b.branchCode
                                            inner join Riders r on r.riderCode = xb.ECRiderCode and r.branchId = xb.BranchID
                                            group by ProductID, xb.ZoneCode,ProductName,TypeName,z.name,xb.BranchID,xb.ECRiderCode
                                            having xb.ZoneCode = @ZoneCode and xb.BranchID = @BranchCode and xb.ECRiderCode = @Rider ", new { @ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString(), @BranchCode = HttpContext.Current.Session["BRANCHCODE"].ToString(), @Rider = RiderCode });
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
        public static List<StockModel> GetIssuedRequest()
        {
            // return new ZoneStockRequestRespo().IssuedRequest(ZoneCode);

            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select s.Qty,ty.TypeName,pr.ProductName 
                from MnP_CNIssue_Stock s
                left join MnP_CNIssue_Product pr on pr.ID=s.ProductID
                left join MnP_CNIssue_Type ty on pr.TypeId=ty.ID
                where s.UserTypeID=1003 and s.ZoneCode=@Zone", new { @Zone = HttpContext.Current.Session["ZONECODE"].ToString() });

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
        public static List<StockModel> GetPendingRequestDetails(String Id)
        {
            //return new ZoneStockRequestRespo().GetPendingRequestDetails(Id);

            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"
		select sreq.Qty,p.ProductName,t.TypeName from MnP_CNIssue_StockReqDetails sreq
		left join MnP_CNIssue_Product p on p.ID=sreq.ProductID
		left join MnP_CNIssue_Type t on t.ID=p.TypeId
		where sreq.StockReqID=@Id", new { @Id = Id });

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
        public static List<StockModel> GetPendingRequest()
        {
            //return new ZoneStockRequestRespo().PendingRequest(ZoneCode);
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"Select sr.ID Id,sr.TotalReqQty Qty from MnP_CNIssue_StockRequest sr 
	where sr.ZoneCode=@Zone  and MONTH(sr.CreatedOn)=MONTH(GETDATE())", new { @Zone = HttpContext.Current.Session["ZONECODE"].ToString() });

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
                var rs = con.Query<StockRequestDetailModel>(@"Select sr.ID ReqId,sr.TotalReqQty Qty, FORMAT(sr.TotalReqQty,N'N0') Qty_,  CONVERT(VARCHAR(11),                   
                     Sr.CreatedOn, 106) CreatedOn ,  sr.ECRiderCode+'-'+r.firstName+'-'+isnull(r.hrs_Code,'')   as TypeName
		                    from MnP_CNIssue_StockRequest sr 
		                    LEFT JOIN MnP_CNIssue_StockIssuance sid on sid.StockReqID= sr.ID
                     inner join Riders r on r.riderCode = sr.ECRiderCode and r.zoneId = sr.ZoneCode and r.branchId = sr.BranchCode
                        where sr.ZoneCode=@Zonee and sr.BranchCode=@Branch and sr.UserTypeID=1005 and sid.ID is null and sr.CreatedOn between @startt +' 00:00:00.000' and @endd+' 23:59:59.999 ';
                ", new { @Zonee = HttpContext.Current.Session["ZONECODE"].ToString(), @startt = StartDate, @endd = EndDate, @Branch = HttpContext.Current.Session["BRANCHCODE"].ToString() });

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

        

                [WebMethod]
        public static List<StockRequestDetailModel> GetFilteredStockZonePer(String Id)
        {

            // return new StockRequestRiderRepo().GetFilteredStockZonePer(Id);
            try
            {
                con.Open();
                var rs = con.Query<StockRequestDetailModel>(@"
				select sred.ID DetailId,sred.StockReqID ReqId,sred.Qty,ty.TypeName,ty.ID TypeId,pr.ID ProductId,pr.ProductName  Product
                from MnP_CNIssue_StockRequest sr 
				inner join MnP_CNIssue_StockReqDetails sred on sred.StockReqID=sr.ID                
                left join MnP_CNIssue_Product pr on pr.ID=sred.ProductID
                left join MnP_CNIssue_Type ty on pr.TypeId=ty.ID
                where sr.ID=@IDD", new { @IDD = Id });

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
        public static List<CNIssuanceDropDownModel> GetProductsSingle(String Id)
        {
            List<CNIssuanceDropDownModel> dr = new List<CNIssuanceDropDownModel>();
            //var rs = new StockInventoryRepo().GetProductsSingle(Id);
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"select p.ID as [Value], p.ProductName as [Text] from [MnP_CNIssue_Product] p where p.TypeId=@id", new { @id = Id });
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
        public static string ModifyRequest(StockRequestModel model)
        {
            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    model.CreatedBy = HttpContext.Current.Session["U_ID"].ToString();
                    model.ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString();
                    model.UserTypeId = UserType;
                    //return rs;

                    List<int> qtys = new List<int>();

                    foreach (var v in model.StockRequestDetails)
                    {
                        string sqlQuery = "update MnP_CNIssue_StockReqDetails  set ProductID=@ProductId ,Qty=@Qty,LastModifiedOn=GETDATE(),LastModifiedBy=@U_ID where ID=@DetailId and StockReqID=@Id;";
                        con.Execute(sqlQuery,
                            new
                            {
                                v.ProductId,
                                v.Qty,
                                U_ID = model.CreatedBy,
                                v.DetailId,
                                model.Id
                            }, transaction: tran);
                    }
                    string sqlQuery2 = "update MnP_CNIssue_StockRequest set TotalReqQty=@TotalQty,LastModifiedOn=GETDATE(),LastModifiedBy=@U_ID where ID=@Id;";
                    con.Execute(sqlQuery2,
                        new
                        {
                            @TotalQty = model.StockRequestDetails.Sum(x => x.Qty),
                            U_ID = model.CreatedBy,
                            model.Id

                        }, transaction: tran);
                    tran.Commit();
                    return "Request Modified Succcessfully";

                }
                catch (Exception er)
                {
                    tran.Rollback();
                    return "Error in modification, please contact IT for support";
                }
                finally
                {
                    con.Close();
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // ZoneName = Session["ZoneName"].ToString();
                ZoneCode = Session["ZONECODE"].ToString();
                BranchCode = Session["BRANCHCODE"].ToString();
                U_ID = Session["U_ID"].ToString();
                GetName();

                if (ZoneCode == "" || ZoneCode == null || U_ID == null || U_ID == "" || BranchCode == "" || BranchCode == null)
                {
                    Response.Redirect("~/Login");
                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/Login");
            }

            if (!IsPostBack)
            {
                GetData();

            }
        }


        [WebMethod]
        public void GetName()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                var sqlq = "select z.Name, z.zoneCode, b.name, b.branchCode from Zones z inner join Branches b on z.zoneCode = b.zoneCode where z.zone_type = 1 AND z.zoneCode = '" + ZoneCode + "';";
                using (SqlCommand cmd = new SqlCommand(sqlq, con))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    //ViewState["dt"] = dt;
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
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetProducts()
        {
            //var rs = new ZoneStockRequestRespo().GetProducts();
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"select ID as [Value], ProductName as [Text] from [MnP_CNIssue_Product];");
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
        public static List<CNIssuanceDropDownModel> GetTypeProducts()
        {
            //var rs = new ZoneStockRequestRespo().GetTypeProducts();
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"select ID as [Value], TypeName as [Text] from MnP_CNIssue_Type;");
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
        public static string SaveStockDetails(StockRequestModel model)
        {
            model.CreatedBy = HttpContext.Current.Session["U_ID"].ToString();
            model.ZoneCode = HttpContext.Current.Session["ZONECODE"].ToString();
            model.UserTypeId = UserType;
            model.BranchCo = HttpContext.Current.Session["BRANCHCODE"].ToString();
            //var rs = new StockRequestRiderRepo().SaveStockDetails(model);

            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    var id = con.QueryFirstOrDefault<int>(@"insert into [MnP_CNIssue_StockRequest] ([Year],ZoneCode,BranchCode,ECRiderCode,TotalReqQty,isActive,CreatedBy,CreatedOn,UserTypeID) values(@Year,@ZoneCode,@BranchCode,@RiderCode,@TotalReqQty,1,@CreatedBy,GETDATE(),1005);
                                                        SELECT SCOPE_IDENTITY();", new { @Year = DateTime.Now.Year, @ZoneCode = model.ZoneCode, @BranchCode = model.BranchCo, @RiderCode = model.ECRiderCode, @TotalReqQty = model.StockRequestDetails.Sum(x => x.Qty), @CreatedBy = model.CreatedBy }, transaction: tran);

                    foreach (var single in model.StockRequestDetails)
                    {
                        var rs = con.Execute(@"insert into [MnP_CNIssue_StockReqDetails] (StockReqID,ProductID, Qty,isActive,CreatedBy,CreatedOn) values(@StockReqID,@ProductID, @Qty,1,@CreatedBy,GETDATE());"
                        , new
                        {
                            StockReqID = id,
                            ProductID = single.ProductId,
                            Qty = single.Qty,
                            CreatedBy = model.CreatedBy
                        }, transaction: tran);
                    }
                    tran.Commit();
                    return "Stock request sent successfully, Request id: " + id;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Error Failed to request stock, please contact IT for support";
                }
                finally
                {
                    con.Close();
                }
            }
        }


        protected void gv_list_Load(object sender, EventArgs e)
        {
            base.OnLoad(e);
            GetData();
        }





        [WebMethod]
        public void GetData()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                var sql = "select ss.ZoneCode, sd.QTY, p.ProductName from MnP_CNIssue_StockRequest ss INNER JOIN MnP_CNIssue_StockReqDetails sd ON ss.ID = sd.StockReqID INNER JOIN MnP_CNIssue_Product p ON p.ID = sd.ProductID";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    //ViewState["dt"] = dt;
                    //gv_list.DataSource = dt;
                    //gv_list.DataBind();
                }

            }
            catch (Exception ex)
            { }
            finally
            {
                con.Close();
            }
        }


    }
}