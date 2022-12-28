using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Configuration;
using System.Web.Services;
using Dapper;
using MRaabta.Models;

namespace MRaabta.Files
{
    public partial class StockInventory : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        static SqlConnection constatic = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlDataReader dr;
        static DataTable dtStatic = new DataTable();
        DataTable dt = new DataTable();
        static string ZoneCode, ZoneName, U_ID;
        static string UserType;
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
                string query = "select p.zoneId,pr.TypeId,p.productId,p.id,p.prefixLength from MnP_CNIssue_Prefix p inner join MnP_CNIssue_Product pr on pr.id = p.productId where p.isActive = '1' order by pr.TypeId,p.productId";
                DataTable dt = new DataTable();
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                using (var command = new SqlCommand(@sql, connection))
                {
                    connection.Open();
                    SqlCommand orcd = new SqlCommand(query, connection);

                    orcd.CommandType = CommandType.Text;

                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    oda.Fill(dt);
                    connection.Close();
                    Session["Prefix"] = dt;
                }
            }
            catch (Exception er)
            {
                Response.Redirect("~/Login");
            }

        }

        [WebMethod]
        //btn search method
        public void GetGridData()
        {
            string sql = "SELECT m.ID, (SELECT RIGHT('000000000000' + CAST(BarcodeFrm as varchar), 11)) as Barcodefrm, (SELECT RIGHT('000000000000' + CAST(BarcodeTo as varchar), 12)) as BarcodeTo,mm.ProductName, m.ZoneCode, m.Qty from MnP_CNIssue_Inventory m inner join MnP_CNIssue_Product mm on m.ProductID = mm.ID WHERE m.IsActive = '1'";

            using (SqlCommand cmdd = new SqlCommand(sql, con))
            {

                SqlDataAdapter sda = new SqlDataAdapter(cmdd);
                con.Open();
                sda.Fill(dt);
                ViewState["dt"] = dt;
                // gv_list.DataSource = ViewState["dt"];
                //gv_list.DataBind();

            }

            con.Close();
        }

        [WebMethod]
        public static List<StockModel> GetHOInventory()
        {
            try
            {
                constatic.Open();
                var rs = constatic.Query<StockModel>(@"
                select pr.ProductName,t.TypeName,z.name as ZoneCode, sum(qty) as Qty, FORMAT(sum(qty),N'N0') as Qty_  from (
                select ProductID, ZoneCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock 
                where UserTypeID = 1002
                ) as xb 
                inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                inner join Zones z on xb.ZoneCode = z.zoneCode
                group by ProductID, xb.ZoneCode,ProductName,TypeName,name
                order by z.name

            ");
                constatic.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                constatic.Close();
                return null;
            }
            catch (Exception ex)
            {
                constatic.Close();
                return null;
            }
        }

        [WebMethod]
        public static List<StockModel> GetHOStock()
        {
            try
            {
                constatic.Open();
                var rs = constatic.Query<StockModel>(@"
                    select pr.ProductName,t.TypeName,z.name as ZoneCode, sum(qty) as Qty from (
                    select ProductID, ZoneCode, case when StatusID = 2 then qty * (-1) else qty end as qty from Mnp_CNIssue_Stock 
                    where UserTypeID = 1002
                    ) as xb 
                    inner join [MnP_CNIssue_Product] pr on ProductID = pr.ID
                    inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                    inner join Zones z on xb.ZoneCode = z.zoneCode
                    group by ProductID, xb.ZoneCode,ProductName,TypeName,name
                    order by z.name ");
                constatic.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                constatic.Close();
                return null;
            }
            catch (Exception ex)
            {
                constatic.Close();
                return null;
            }
        }


        [WebMethod]
        //btn search for barcode 
        public static bool CheckSequenceDuplication(string barcodeFrm, string barcodeto)
        {
            string sql = "SELECT m.ID, (SELECT RIGHT('000000000000' + CAST(BarcodeFrm as varchar), 11)) as Barcodefrm, (SELECT RIGHT('000000000000' + CAST(BarcodeTo as varchar), 12)) as BarcodeTo,mm.ProductName, m.ZoneCode, m.Qty from MnP_CNIssue_Inventory m inner join MnP_CNIssue_Product mm on m.ProductID = mm.ID WHERE m.IsActive = '1'";

            using (SqlCommand cmdd = new SqlCommand(sql, constatic))
            {

                SqlDataAdapter sda = new SqlDataAdapter(cmdd);
                constatic.Open();

                bool status = true;

                var rs = constatic.Query<string>(@"select * from MnP_CNIssue_Inventory where BarcodeFrm between @barcodeFrm and @barcodeto union select * from MnP_CNIssue_Inventory where BarcodeTo between @barcodeFrm and @barcodeto union select * from MnP_CNIssue_Inventory where @barcodeFrm between BarcodeFrm and BarcodeTo union select * from MnP_CNIssue_Inventory where @barcodeto between BarcodeFrm and BarcodeTo", new { @barcodeFrm = barcodeFrm, @barcodeto = barcodeto });
                constatic.Close();

                //Validations
                if (rs.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        [WebMethod]
        //btn search for barcode 
        public static bool ValidateWithDetailIdInModal(string barcodeFrm, string barcodeto, string Detailid)
        {
            string sql = "SELECT m.ID, (SELECT RIGHT('000000000000' + CAST(BarcodeFrm as varchar), 11)) as Barcodefrm, (SELECT RIGHT('000000000000' + CAST(BarcodeTo as varchar), 12)) as BarcodeTo,mm.ProductName, m.ZoneCode, m.Qty from MnP_CNIssue_Inventory m inner join MnP_CNIssue_Product mm on m.ProductID = mm.ID WHERE m.IsActive = '1'";

            using (SqlCommand cmdd = new SqlCommand(sql, constatic))
            {

                SqlDataAdapter sda = new SqlDataAdapter(cmdd);
                constatic.Open();

                bool status = true;

                var rs = constatic.Query<string>(@"select * from MnP_CNIssue_Inventory where BarcodeFrm between @barcodeFrm and @barcodeto and id not in (@DetailId) union select * from MnP_CNIssue_Inventory where BarcodeTo between @barcodeFrm and @barcodeto and id not in (@DetailId) union select * from MnP_CNIssue_Inventory where @barcodeFrm between BarcodeFrm and BarcodeTo and id not in (@DetailId) union select * from MnP_CNIssue_Inventory where @barcodeto between BarcodeFrm and BarcodeTo and id not in (@DetailId)", new { @barcodeFrm = barcodeFrm, @barcodeto = barcodeto, @DetailId = Detailid });
                constatic.Close();

                //Validations
                if (rs.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        //ddl products method
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetProducts()
        {
            //var rs = new StockInventoryRepo().GetProducts();

            try
            {
                constatic.Open();
                var rs = constatic.Query<CNIssuanceDropDownModel>(@"select ID as [Value], ProductName as [Text] from [MnP_CNIssue_Product];");
                constatic.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                constatic.Close();
                return null;
            }
            catch (Exception ex)
            {
                constatic.Close();
                return null;
            }
        }


        [WebMethod]
        public static List<StockModel> GetMyStock()
        {
            //return new StockInventoryRepo().GetMyStock();

            try
            {
                constatic.Open();
                var rs = constatic.Query<StockModel>(@"select pr.ProductName,st.BarcodeFrm,st.BarcodeTo,st.Qty,Year from Mnp_CNIssue_Stock  st inner join [MnP_CNIssue_Product] pr on st.ProductID = pr.ID
                                                                                        where UserTypeID = 1002");
                constatic.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                constatic.Close();
                return null;
            }
            catch (Exception ex)
            {
                constatic.Close();
                return null;
            }
        }

        //ddl Types method
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetTypes()
        {
            //var rs = new StockInventoryRepo().GetTypes();

            try
            {
                constatic.Open();
                var rs = constatic.Query<CNIssuanceDropDownModel>(@"select [ID] as Value, [TypeName] as Text  from MnP_CNIssue_Type where isActive = 1;");
                constatic.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                constatic.Close();
                return null;
            }
            catch (Exception ex)
            {
                constatic.Close();
                return null;
            }
        }

        //ddl Zones method

        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetZones()
        {
            //var rs = new StockInventoryRepo().GetZones();

            try
            {
                constatic.Open();
                var rs = constatic.Query<CNIssuanceDropDownModel>(@"select [zoneCode] as Value,[Name] as Text from Zones where region is not null order by name asc;");
                constatic.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                constatic.Close();
                return null;    
            }
            catch (Exception ex)
            {
                constatic.Close();
                return null;
            }
        }

        #region insert stock details
        [WebMethod]
        public static string SaveStockDetails(StockRequestModel model)
        {
            model.CreatedBy = U_ID;
            //var rs = new StockInventoryRepo().SaveStockDetails(model);
            constatic.Open();
            object inventoryInsert;
            object stockInsert;
            using (var tran = constatic.BeginTransaction())
            {
                try
                {
                    foreach (var item in model.StockRequestDetails)
                    {
                        inventoryInsert = constatic.Execute(@"insert into[MnP_CNIssue_Inventory] (IsActive,ProductID,ZoneCode,BarcodeFrm,BarcodeTo,Qty,CreatedOn,CreatedBy, Year) values(1,@ProductID,@ZoneCode,@BarcodeFrm,@BarcodeTo,@Qty,GETDATE(),@CreatedBy,@yearr);"
                          , new
                          {
                              ProductID = item.ProductId,
                              ZoneCode = item.ZoneCode,
                              BarcodeFrm = item.BarcodeFrm,
                              BarcodeTo = item.BarcodeTo,
                              Qty = item.Qty,
                              CreatedBy = HttpContext.Current.Session["U_ID"].ToString(),
                              yearr = DateTime.Now.Year
                          }, transaction: tran);

                        stockInsert = constatic.Execute(@"insert into[Mnp_CNIssue_Stock] (StatusID,ProductID,ZoneCode,BarcodeFrm,BarcodeTo,UserTypeID,Qty,EntryDate, Year) values(1,@ProductID,@ZoneCode,@BarcodeFrm,@BarcodeTo,1002,@Qty,GETDATE(),@year_);"
                            , new
                            {
                                ProductID = item.ProductId,
                                ZoneCode = item.ZoneCode,
                                BarcodeFrm = item.BarcodeFrm,
                                BarcodeTo = item.BarcodeTo,
                                Qty = item.Qty,
                                year_ = DateTime.Now.Year
                            }, transaction: tran);
                    }
                    tran.Commit();
                    return "Stock Saved Successfully";

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Error in saving stock inventory!";
                }
                finally
                {
                    constatic.Close();
                }
            }
        }
        #endregion

        #region product list
        //ddl products method
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetProductsSingle(String Id)
        {
            try
            {
                constatic.Open();
                var rs = constatic.Query<CNIssuanceDropDownModel>(@"select p.ID as [Value], p.ProductName as [Text] from [MnP_CNIssue_Product] p where p.TypeId=@id", new { @id = Id });
                return rs.ToList();
            } 
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                constatic.Close();
            }
        }
        #endregion

        #region Prefixes
        //Get Prefixes
        [WebMethod]
        public static CNIssuanceDropDownModel GetPrefixes(string ZoneId, string TypeId, string ProductId, string Barcode)
        {
            DataTable dt = HttpContext.Current.Session["Prefix"] as DataTable;
            CNIssuanceDropDownModel d = new CNIssuanceDropDownModel();
            if (TypeId == "1")
            {
                d.Text = "1";
                d.Value = "6";
                return d;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["zoneId"].ToString() == ZoneId)
                {
                    if (dt.Rows[i]["productId"].ToString() == ProductId)
                    {
                        if (Barcode.StartsWith(dt.Rows[i]["id"].ToString()))
                        {
                            d.Text = dt.Rows[i]["id"].ToString();
                            d.Value = dt.Rows[i]["prefixLength"].ToString();
                            return d;
                        }


                    }
                }

            }
            return d;
        }
        #endregion

        #region Filtered Stock zone
        [WebMethod]
        public static Response_StockRequestDetailModel GetFilteredStockZone(String StartDate, String EndDate, int zone)
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

                constatic.Open();
                if (zone > 0)
                {
                    var rs = constatic.Query<StockRequestDetailModel>(@"select i.ID as Inv_ID,xb.ID as stockID,xb.ProductID,pr.ProductName,xb.ZoneCode,z.name ,t.TypeName,xb.BarcodeFrom+(xb.Qty - xb.QtyIssueFrom ) as BarcodeFrom, xb.BarcodeTo,xb.QtyIssueFrom as Qty , FORMAT(xb.QtyIssueFrom,N'N0') as Qty_,
                pr.TypeId, case when xb.Qty - xb.QtyIssueFrom > 0 then 'true' else 'false' end IsUpdated
                from (select ID,ProductID, s.barcodeFrm as BarcodeFrom,s.BarcodeTo,s.Qty,s.zonecode,
                (select sum(Qty) as qty from (
                select ProductID,ZoneCode,UserTypeID,
                case when statusID = 1 then Qty * 1 else Qty * -1 end as Qty 
                from Mnp_CNIssue_Stock
                where
                 UserTypeID = 1002 and BarcodeFrm between s.barcodeFrm and s.BarcodeTo and EntryDate between @startt+' 00:00:00.000' and @endd+' 23:59:59.999' and s.ZoneCode = @zone) as xb 
                ) as QtyIssueFrom
                from Mnp_CNIssue_Stock s 
                where s.UserTypeID =  1002 
                and statusID = 1 and EntryDate between @startt+' 00:00:00.000' and @endd+' 23:59:59.999' and ZoneCode= @zone ) as xb 
                inner join MnP_CNIssue_Inventory i on xb.ProductID = i.ProductID and xb.ZoneCode = i.ZoneCode and xb.BarcodeFrom = i.BarcodeFrm 
                and xb.BarcodeTo = i.BarcodeTo 
                inner join [MnP_CNIssue_Product] pr on xb.ProductID = pr.ID
                inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                inner join Zones z on xb.zonecode = z.zoneCode
                order by i.ID,xb.ID", new { @startt = StartDate, @endd = EndDate, @zone = zone });
                    constatic.Close();
                     
                    resp.Data = rs.ToList();
                    resp.status = true;
                    resp.statusMessage = "Success";
                }
                else
                {
                    var rs = constatic.Query<StockRequestDetailModel>(@"select i.ID as Inv_ID,xb.ID as stockID,xb.ProductID,pr.ProductName,xb.ZoneCode,z.name ,t.TypeName,xb.BarcodeFrom+(xb.Qty - xb.QtyIssueFrom ) as BarcodeFrom, xb.BarcodeTo,xb.QtyIssueFrom as Qty, FORMAT(xb.QtyIssueFrom,N'N0') as Qty_,
                pr.TypeId, case when xb.Qty - xb.QtyIssueFrom > 0 then 'true' else 'false' end IsUpdated
                from (select ID,ProductID, s.barcodeFrm as BarcodeFrom,s.BarcodeTo,s.Qty,s.zonecode,
                (select sum(Qty) as qty from (
                select ProductID,ZoneCode,UserTypeID,
                case when statusID = 1 then Qty * 1 else Qty * -1 end as Qty 
                from Mnp_CNIssue_Stock
                where
                 UserTypeID = 1002 and BarcodeFrm between s.barcodeFrm and s.BarcodeTo and EntryDate between @startt+' 00:00:00.000' and @endd+' 23:59:59.999') as xb 
                ) as QtyIssueFrom
                from Mnp_CNIssue_Stock s 
                where s.UserTypeID =  1002 
                and statusID = 1 and EntryDate between @startt+' 00:00:00.000' and @endd+' 23:59:59.999' ) as xb 
                inner join MnP_CNIssue_Inventory i on xb.ProductID = i.ProductID and xb.ZoneCode = i.ZoneCode and xb.BarcodeFrom = i.BarcodeFrm 
                and xb.BarcodeTo = i.BarcodeTo 
                inner join [MnP_CNIssue_Product] pr on xb.ProductID = pr.ID
                inner join [MnP_CNIssue_Type] t on t.id = pr.TypeId
                inner join Zones z on xb.zonecode = z.zoneCode
                order by i.ID,xb.ID", new { @startt = StartDate, @endd = EndDate });
                    constatic.Close();

                    resp.Data = rs.ToList();
                    resp.status = true;
                    resp.statusMessage = "Success";
                }
            }
            catch (SqlException ex)
            {
                constatic.Close();
                resp.Data = null;
                resp.status = false;
                resp.statusMessage = ex.ToString();
            }
            catch (Exception ex)
            {
                constatic.Close();
                resp.Data = null;
                resp.status = false;
                resp.statusMessage = ex.ToString();
            }
            return resp;
        }

        [WebMethod]
        public static StockRequestDetailModel GetFilteredStockZonePer(string qty, string zone, string product, string BarcodeFrom, string barcodeTo)
        {
            //return new StockInventoryRepo().GetFilteredStockZonePer(qty, zone, product, BarcodeFrom, barcodeTo);
            try
            {
                constatic.Open();
                var rs = constatic.QueryFirstOrDefault<StockRequestDetailModel>(@"Select s.ID Id,p.id ProductId,p.ProductName ProductName,p.TypeId TypeId,t.TypeName TypeName,ZoneCode ZoneCode,s.Qty Qty,s.BarcodeFrm BarcodeFrm,s.BarcodeTo BarcodeTo
	            from MnP_CNIssue_Inventory s 
                left join MnP_CNIssue_Product p on p.ID=s.ProductID
                left join MnP_CNIssue_Type t on t.ID=p.TypeId 
				where BarcodeTo=@BarcodeTo and p.ProductName=@ProductName", new { @BarcodeTo = barcodeTo, @ProductName = product });

                constatic.Close();
                return rs;
            }
            catch (SqlException ex)
            {
                constatic.Close();
                return null;
            }
            catch (Exception ex)
            {
                constatic.Close();
                return null;
            }
        }

        #endregion

        #region modify request

        [WebMethod]
        public static string ModifyRequest(String Inv_id, String stock_id, String valueZone, String valueType, String valueProd, String qtyModal, String barcodeFrm, String barcodeToo)
        {
            constatic.Open();
            using (var tran = constatic.BeginTransaction())
            {
                try
                {

                    string sqlQuery = "update MnP_CNIssue_Inventory set ProductID = @valueProd, ZoneCode = @valueZone, BarcodeFrm = @barcodeFrm, BarcodeTo = @barcodeToo, Qty = @qtyModal, LastModifiedOn = GETDATE(), LastModifiedBy = @U_ID  where ID = @Inv_id";
                    constatic.Execute(sqlQuery,
                        new
                        {
                            valueProd,
                            valueZone,
                            barcodeFrm,
                            barcodeToo,
                            qtyModal,
                            U_ID = HttpContext.Current.Session["U_ID"].ToString(),
                            Inv_id

                        }, transaction: tran);

                    string sqlQuery2 = "update Mnp_CNIssue_Stock set ProductID = @valueProd, ZoneCode = @valueZone, BarcodeFrm = @barcodeFrm, BarcodeTo = @barcodeToo, Qty = @qtyModal where ID=@stock_id";
                    constatic.Execute(sqlQuery2,
                        new
                        {
                            valueProd,
                            valueZone,
                            barcodeFrm,
                            barcodeToo,
                            qtyModal,
                            stock_id

                        }, transaction: tran);

                    tran.Commit();
                    return "Stock Modified Successfully";

                }
                catch (Exception er)
                {
                    tran.Rollback();
                    return "Failed to modify stock, please contact IT for support";
                }
                finally
                {
                    constatic.Close();
                }
            }
        }

        #endregion
    }
}