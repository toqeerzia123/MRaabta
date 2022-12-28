using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using MRaabta.Models;
using Dapper;
using System.Web;

namespace MRaabta.Files
{
    public partial class StockRequestZone_IMS : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlDataReader dr;
        DataTable dt = new DataTable();
        static string ZoneCode, ZoneName;
        static string UserType, U_ID;

        [WebMethod]
        public static List<StockModel> GetHOStock()
        { 
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select pr.OracleCode, pr.ProductName,t.TypeName,z.name as Zone,
                    sum(qty) as Qty,
                    FORMAT(sum(qty),N'N0') as Qty_ from (
                    select ProductID, ZoneCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock 
                    where UserTypeID = 1002
                    ) as xb 
                    inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                    inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                    inner join Zones z on xb.ZoneCode = z.zoneCode
                    group by pr.OracleCode,ProductID, xb.ZoneCode,ProductName,TypeName,name
                    having xb.ZoneCode = '28'");

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
        public static List<StockModel> GetZoneStock()
        { 
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"
            select pr.OracleCode, productID,pr.ProductName,t.TypeName,z.name as Zone, 
            sum(qty) as Qty,
            FORMAT(sum(qty),N'N0')  as Qty_ from (
            select ProductID, ZoneCode, 
            case when StatusID = 2 then qty * (-1) 
              when StatusID = 1 then qty else qty * 0 end as qty 
            from Mnp_CNIssue_Stock 
            where UserTypeID = 1003
            ) as xb 
            inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
            inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
            inner join Zones z on xb.ZoneCode = z.zoneCode
            group by pr.OracleCode, ProductID, xb.ZoneCode,ProductName,TypeName,name
            having xb.ZoneCode = @Zone", new { @Zone = HttpContext.Current.Session["ZONECODE"].ToString() });

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
        public static List<StockModel> GetIssuedRequest()
        { 
            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"select pr.OracleCode, s.Qty,ty.TypeName,pr.ProductName 
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

            try
            {
                con.Open();
                var rs = con.Query<StockModel>(@"
		select p.OracleCode, sreq.Qty,p.ProductName,t.TypeName from MnP_CNIssue_StockReqDetails sreq
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
        public static Response_StockRequestModel GetFilteredStockZone(String StartDate, String EndDate)
        {
            Response_StockRequestModel resp = new Response_StockRequestModel();
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
                var rs = con.Query<StockRequestModel>(@"select sr.ID, sr.TotalReqQty, FORMAT(sr.TotalReqQty,N'N0')  TotalReqQty_, case when si.StockReqID is null then 0 else 1 end as [IsUpdated]  
                                                    from [MnP_CNIssue_StockRequest] sr
													left join [MnP_CNIssue_StockIssuance] si on sr.ID = si.StockReqID
                                                    where sr.isActive = 1  AND si.ID is null AND sr.TotalReqQty > 0 and sr.ZoneCode=@Zone and 
                                                    sr.UserTypeID = '1003'and cast(sr.CreatedOn as date) between @startt and @endd;", new { @Zone = HttpContext.Current.Session["ZONECODE"].ToString(), @startt = StartDate, @endd = EndDate });

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
        public static StockRequestDetailModel GetFilteredStockZonePer(String Id)
        { 
            try
            {
                con.Open();
                var rs = con.QueryFirst<StockRequestDetailModel>(@"select sred.ID DetailId,sred.StockReqID ReqId,sred.Qty,ty.TypeName,ty.ID TypeId,pr.ID ProductId,pr.ProductName  Product
                from MnP_CNIssue_StockReqDetails sred 
                left join MnP_CNIssue_StockRequest sr on sred.StockReqID=sr.ID
                left join MnP_CNIssue_Product pr on pr.ID=sred.ProductID
                left join MnP_CNIssue_Type ty on pr.TypeId=ty.ID
                where sred.ID=@IDD", new { @IDD = Id });

                con.Close();
                return rs;
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
                        string sqlQuery = "update MnP_CNIssue_StockReqDetails  set ProductID=@ProductId ,Qty=@Qty,LastModifiedOn=GETDATE(),LastModifiedBy=@U_ID where ID=@DetailId and StockReqID=@Id";
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


                    string sqlQuery2 = "update MnP_CNIssue_StockRequest set TotalReqQty=@TotalQty,LastModifiedOn=GETDATE(),LastModifiedBy=@U_ID where ID=@Id";
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
                ZoneCode = Session["ZONECODE"].ToString();

                U_ID = Session["U_ID"].ToString();
                GetName();

                if (ZoneCode == "" || ZoneCode == null || U_ID == null || U_ID == "")
                {
                    Response.Redirect("~/Login");
                }
                string sql = "select z.name from ZNI_USER1 u left join Zones z on u.ZoneCode = z.zoneCode " +
                 "where U_ID = " + U_ID;
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                using (var command = new SqlCommand(@sql, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Don't assume we have any rows.
                        {
                            int ord = reader.GetOrdinal("name");
                            ZoneName = reader.GetString(ord); // Handles nulls and empty strings.
                        }
                    }
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
            con.Open();
            object rs;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    var id = con.QueryFirstOrDefault<int>(@"insert into [MnP_CNIssue_StockRequest] ([Year],ZoneCode,TotalReqQty,isActive,CreatedBy,CreatedOn,UserTypeID) values(@Year,@ZoneCode,@TotalReqQty,1,@CreatedBy,GETDATE(),1003);
                                                        SELECT SCOPE_IDENTITY();", new { @Year = DateTime.Now.Year, @ZoneCode = model.ZoneCode, @TotalReqQty = model.StockRequestDetails.Sum(x => x.Qty), @CreatedBy = model.CreatedBy }, transaction: tran);

                    foreach (var single in model.StockRequestDetails)
                    {
                        rs = con.Execute(@"insert into [MnP_CNIssue_StockReqDetails] (StockReqID,ProductID, Qty,isActive,CreatedBy,CreatedOn) values(@StockReqID,@ProductID, @Qty,1,@CreatedBy,GETDATE());"
                        , new
                        {
                            StockReqID = id,
                            ProductID = single.ProductId,
                            Qty = single.Qty,
                            CreatedBy = model.CreatedBy
                        }, transaction: tran);
                    }
                    tran.Commit();
                    return "Request sent successfully, Request id: " + id;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Error in request, please contact IT for support";
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
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                con.Close();
            }
        }

        [WebMethod]
        public static List<StockRequestDetailModel> GetRequestsDetails(int id)
        {
            try
            {
                con.Open();
                var rs = con.Query<StockRequestDetailModel>(@"select srd.ID,ProductID,Qty,pr.ProductName,t.TypeName,pr.TypeId from MnP_CNIssue_StockReqDetails srd
                                                        inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                                                        inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                                                        where StockReqID = @Id;", new { @Id = id });

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
    }
}