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
    public partial class StockPrefix_IMS : System.Web.UI.Page
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

        #region method for product ddl
        [WebMethod]
        public static bool GetTypeSerialised(String OracleCode)
        {
            try
            {
                con.Open();
                bool rs = Convert.ToBoolean(con.ExecuteScalar(@"SELECT isnull(mpct.isSerialised,0) isSerialised FROM MnP_CNIssue_Product mpct WHERE mpct.OracleCode=@OracleCode", new { @OracleCode = OracleCode }));
                return rs;
            }
            catch (Exception ex)
            {
                return false;
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
        [WebMethod]
        public static List<CNIssuanceDropDownModel> GetZones()
        {
            try
            {
                con.Open();
                var rs = con.Query<CNIssuanceDropDownModel>(@"select [zoneCode] as Value,[Name] as Text from Zones where region is not null order by name asc;");
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


        #region search requests
        [WebMethod]
        public static List<StockPrefixModel> GetFilteredPrefix(string zone, string Product, string Prefix)
        {
            try
            {
                con.Open();
                if (Prefix != "")
                {
                    Prefix = string.Concat(Prefix, "%");
                    if (zone == "0")
                    {
                        var rs = con.Query<StockPrefixModel>(@" SELECT mpcp.Id     PrefixId,
                                                       mpcp.prefixLength,
                                                       z.name      ZoneName,
                                                       mpcp2.ProductName,
                                                       mpct.TypeName,mpcp.zoneId ZoneCode
                                                FROM   MnP_CNIssue_Prefix mpcp
                                                       INNER JOIN Zones z
                                                            ON  z.zoneCode = mpcp.zoneId
                                                       INNER JOIN MnP_CNIssue_Product mpcp2
                                                            ON  mpcp2.Id = mpcp.productId
                                                       INNER JOIN MnP_CNIssue_Type mpct
                                                            ON  mpct.ID = mpcp2.TypeId
                                                WHERE  mpcp.isActive = 1
                                                       And mpcp.productId=@Product
                                                       AND mpcp.Id LIKE @Prefix ", new { Prefix = Prefix, @Product = Product });

                        con.Close();
                        return rs.ToList();
                    }
                    else
                    {
                        var rs = con.Query<StockPrefixModel>(@" SELECT mpcp.Id     PrefixId,
                                                       mpcp.prefixLength,
                                                       z.name      ZoneName,
                                                       mpcp2.ProductName,
                                                       mpct.TypeName,mpcp.zoneId ZoneCode
                                                FROM   MnP_CNIssue_Prefix mpcp
                                                       INNER JOIN Zones z
                                                            ON  z.zoneCode = mpcp.zoneId
                                                       INNER JOIN MnP_CNIssue_Product mpcp2
                                                            ON  mpcp2.Id = mpcp.productId
                                                       INNER JOIN MnP_CNIssue_Type mpct
                                                            ON  mpct.ID = mpcp2.TypeId
                                                WHERE  mpcp.isActive = 1
                                                       And mpcp.productId=@Product
                                                       AND mpcp.zoneId=@zone
                                                       AND mpcp.Id LIKE @Prefix ", new { Prefix = Prefix, @Product = Product, @zone = zone, });

                        con.Close();
                        return rs.ToList();
                    }
                }

                if (zone == "0")
                {
                    var rs = con.Query<StockPrefixModel>(@" SELECT mpcp.Id     PrefixId,
                                                       mpcp.prefixLength,
                                                       z.name      ZoneName,
                                                       mpcp2.ProductName,
                                                       mpct.TypeName,mpcp.zoneId ZoneCode
                                                FROM   MnP_CNIssue_Prefix mpcp
                                                       INNER JOIN Zones z
                                                            ON  z.zoneCode = mpcp.zoneId
                                                       INNER JOIN MnP_CNIssue_Product mpcp2
                                                            ON  mpcp2.Id = mpcp.productId
                                                       INNER JOIN MnP_CNIssue_Type mpct
                                                            ON  mpct.ID = mpcp2.TypeId
                                                WHERE  mpcp.isActive = 1
                                                       AND mpcp.productId=@Product ", new { @Product = Product });

                    con.Close();
                    return rs.ToList();
                }
                else
                {
                    var rs = con.Query<StockPrefixModel>(@" SELECT mpcp.Id     PrefixId,
                                                       mpcp.prefixLength,
                                                       z.name      ZoneName,
                                                       mpcp2.ProductName,
                                                       mpct.TypeName,mpcp.zoneId ZoneCode
                                                FROM   MnP_CNIssue_Prefix mpcp
                                                       INNER JOIN Zones z
                                                            ON  z.zoneCode = mpcp.zoneId
                                                       INNER JOIN MnP_CNIssue_Product mpcp2
                                                            ON  mpcp2.Id = mpcp.productId
                                                       INNER JOIN MnP_CNIssue_Type mpct
                                                            ON  mpct.ID = mpcp2.TypeId
                                                WHERE  mpcp.isActive = 1
                                                       AND mpcp.zoneId=@zone And mpcp.productId=@Product ", new { @zone = zone, @Product = Product });

                    con.Close();
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

        #region Save Disable Prefix
        [WebMethod]
        public static string DisablePrefix(string Id, string ZoneCode)
        {
            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    string sqlQuery2 = " update MnP_CNIssue_Prefix set isActive=0,ModifiedBy=@ModifiedBy,ModifiedOn=GETDATE() where Id=@Id AND ZoneId=@ZoneCode";
                    con.Execute(sqlQuery2,
                        new
                        {
                            @Id = Id,
                            @ModifiedBy = HttpContext.Current.Session["U_ID"].ToString(),
                            @ZoneCode = ZoneCode
                        }, transaction: tran);
                    tran.Commit();
                    return "Prefix Disabled Successfully";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to disable prefix, Please contact IT for support.";
                }
                finally
                {
                    con.Close();
                }
            }
        }


        [WebMethod]
        public static string SavePrefixEntries(StockPrefixList model)
        {
            con.Open();
            object reqDetails;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    foreach (var singleDetail in model.PrefixDetailList)
                    {
                        singleDetail.CreatedBy = U_ID;

                        reqDetails = con.Execute(@"insert into [MnP_CNIssue_Prefix] (Id,zoneId, ProductId,prefixLength,CreatedBy,CreatedOn,isActive) 
                                                                                values(@PrefixId,@zoneId,@ProductID, @PrefixLength,@CreatedBy,GETDATE(),1);",
                        new
                        {
                            PrefixId = singleDetail.PrefixId,
                            PrefixLength = singleDetail.PrefixLength,
                            zoneId = singleDetail.ZoneCode,
                            ProductID = singleDetail.ProductId,
                            CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                        }, transaction: tran);
                    }

                    tran.Commit();
                    return "Prefix saved successfully ";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to save prefix, Please contact IT for support.";
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