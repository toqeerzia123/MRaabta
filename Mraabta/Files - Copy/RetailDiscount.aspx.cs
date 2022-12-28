using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using System.Web.Script.Services;
using System.Data;
using System.Linq;
using System.Globalization;
using Dapper;

namespace MRaabta.Files
{
    public partial class RetailDiscount : System.Web.UI.Page
    {

        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        static String Profile_User;
        static String U_ID = "";
        
        [WebMethod]
        public static List<DiscountExpressCenterModel> GetZones()
        {
            try
            {
                List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                con.Open();
                String sql = "select [zoneCode] as Value,[Name] as Text from Zones WHERE status=1 AND Region IS NOT NULL ORDER BY name";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1) });
                    }
                }
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
        public static List<DiscountExpressCenterModel> GetServiceTypes()
        {
            try
            {
                List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                con.Open();
                //            String sql = "SELECT [serviceTypeName] AS VALUE,[serviceTypeName] AS TEXT FROM ServiceTypes_New WHERE Products='Domestic' AND [status]=1 ORDER BY serviceTypeName";

                String sql = "SELECT [serviceTypeName] AS VALUE,[serviceTypeName] AS TEXT \n"
                           + "FROM   ServiceTypes_New s \n"
                           + "WHERE  s.Products in ('Domestic', 'Road n Rail') \n"
                           + "       AND s.[status] = '1' \n"
                           + "       AND s.serviceTypeName NOT IN ('Laptop', 'Return SERVICE', 'ZD','Bank to Bank') \n"
                           + "ORDER BY \n"
                           + "       s.serviceTypeName ASC";

                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1) });
                    }
                }
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
        public static List<DiscountExpressCenterModel> GetBranches(String zone)
        {
            try
            {
                List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                con.Open();

                String sql;
                zone = zone.ToLower();
                if (zone.ToString() != "all")
                {
                    sql = "select[branchCode] as Value,[Name] as Text from Branches WHERE zoneCode='" + zone + "' AND[status]=1 ORDER BY name";
                }
                else
                {
                    sql = "select[branchCode] as Value,[Name] as Text from Branches WHERE [status]=1 ORDER BY name";
                }
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1) });
                    }
                }
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
        public static List<DiscountExpressCenterModel> GetExpressCenters(String zone, String branch)
        {

            try
            {
                //List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                //con.Open();

                //String sql;
                //branch = branch.ToLower();
                //zone = zone.ToLower();

                //if (branch.ToString() != "all")
                //{
                //    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT,ec.bid branch, z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode = ec.bid   INNER JOIN Zones z ON z.zoneCode = b.zoneCode    WHERE ec.bid = " + branch + " AND ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name ";
                //}
                //else if (zone.ToString() != "all")
                //{
                //    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT  ,ec.bid branch,b.zoneCode zone FROM ExpressCenters ec	left JOIN Branches b ON b.zoneCode=" + zone + " WHERE ec.bid = b.branchCode AND ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name";
                //}
                //else
                //{
                //    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT ,ec.bid branch,z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode=ec.bid INNER JOIN Zones z ON z.zoneCode=b.zoneCode  WHERE  ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name";
                //}
                //using (var cmd2 = new SqlCommand(sql, con))
                //{
                //    SqlDataReader dr;
                //    dr = cmd2.ExecuteReader();

                //    while (dr.Read())
                //    {
                //        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1), branch = dr.GetString(2), zone = dr.GetString(3) });
                //    }
                //}
                //con.Close();
                return null;
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
        public static List<DiscountExpressCenterModel> GetExpressCentersShopnShop(String zone, String branch)
        {
            try
            {
                List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                con.Open();

                String sql;
                branch = branch.ToLower();
                zone = zone.ToLower();

                if (branch.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT,ec.bid branch, z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode = ec.bid   INNER JOIN Zones z ON z.zoneCode = b.zoneCode    WHERE ec.bid = " + branch + " AND ec.[status]=1 AND ec.Center_type=3 ORDER BY ec.name ";
                }
                else if (zone.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT  ,ec.bid branch,b.zoneCode zone FROM ExpressCenters ec	left JOIN Branches b ON b.zoneCode=" + zone + " WHERE ec.bid = b.branchCode AND ec.[status]=1 AND ec.Center_type=3 ORDER BY ec.name";
                }
                else
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT ,ec.bid branch,z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode=ec.bid INNER JOIN Zones z ON z.zoneCode=b.zoneCode  WHERE  ec.[status]=1 AND ec.Center_type=3 ORDER BY ec.name";
                }
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1), branch = dr.GetString(2), zone = dr.GetString(3) });
                    }
                }
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
        public static List<DiscountExpressCenterModel> GetExpressCentersFranchise(String zone, String branch)
        {
            try
            {
                List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                con.Open();

                String sql;
                branch = branch.ToLower();
                zone = zone.ToLower();

                if (branch.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT,ec.bid branch, z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode = ec.bid   INNER JOIN Zones z ON z.zoneCode = b.zoneCode    WHERE ec.bid = " + branch + " AND ec.[status]=1 AND ec.Center_type=2 ORDER BY ec.name ";
                }
                else if (zone.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT  ,ec.bid branch,b.zoneCode zone FROM ExpressCenters ec	left JOIN Branches b ON b.zoneCode=" + zone + " WHERE ec.bid = b.branchCode AND ec.[status]=1 AND ec.Center_type=2 ORDER BY ec.name";
                }
                else
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT ,ec.bid branch,z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode=ec.bid INNER JOIN Zones z ON z.zoneCode=b.zoneCode  WHERE  ec.[status]=1 AND ec.Center_type=2 ORDER BY ec.name";
                }
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1), branch = dr.GetString(2), zone = dr.GetString(3) });
                    }
                }
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
        public static List<DiscountExpressCenterModel> GetExpressCentersFranchiseBranch(String zone, String branch)
        {
            try
            {
                List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                con.Open();

                String sql;
                branch = branch.ToLower();
                zone = zone.ToLower();

                if (branch.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +'('+ec.expressCenterCode+')' TEXT,ec.bid branch, z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode = ec.bid   INNER JOIN Zones z ON z.zoneCode = b.zoneCode    WHERE ec.bid = " + branch + " AND ec.[status]=1 AND ec.Center_type=4 ORDER BY ec.name ";
                }
                else if (zone.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +'('+ec.expressCenterCode+')' TEXT  ,ec.bid branch,b.zoneCode zone FROM ExpressCenters ec	left JOIN Branches b ON b.zoneCode=" + zone + " WHERE ec.bid = b.branchCode AND ec.[status]=1 AND ec.Center_type=4 ORDER BY ec.name";
                }
                else
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +'('+ec.expressCenterCode+')' TEXT ,ec.bid branch,z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode=ec.bid INNER JOIN Zones z ON z.zoneCode=b.zoneCode  WHERE  ec.[status]=1 AND ec.Center_type=4 ORDER BY ec.name";
                }
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1), branch = dr.GetString(2), zone = dr.GetString(3) });
                    }
                }
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
        public static List<DiscountExpressCenterModel> GetExpressCentersCompanyMaintained(String zone, String branch)
        {
            try
            {
                List<DiscountExpressCenterModel> rs = new List<DiscountExpressCenterModel>();
                con.Open();

                String sql;
                branch = branch.ToLower();
                zone = zone.ToLower();

                if (branch.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT,ec.bid branch, z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode = ec.bid   INNER JOIN Zones z ON z.zoneCode = b.zoneCode    WHERE ec.bid = " + branch + " AND ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name ";
                }
                else if (zone.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT  ,ec.bid branch,b.zoneCode zone FROM ExpressCenters ec	left JOIN Branches b ON b.zoneCode=" + zone + " WHERE ec.bid = b.branchCode AND ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name";
                }
                else
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT ,ec.bid branch,z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode=ec.bid INNER JOIN Zones z ON z.zoneCode=b.zoneCode  WHERE  ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name";
                }
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountExpressCenterModel { Value = dr.GetString(0), Text = dr.GetString(1), branch = dr.GetString(2), zone = dr.GetString(3) });
                    }
                }
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
        public static String GetSpecialId()
        {
            try
            {
                long PrevSpecialId = 0;
                con.Open();
                String sql = "SELECT MAX(cast(SpecialId AS int)) FROM MnP_MasterDiscount ";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr.IsDBNull(0))
                            PrevSpecialId = 7;
                        else
                            PrevSpecialId = dr.GetInt32(0);
                    }
                }
                con.Close();
                long randomNum = getRandomNumber();
                return (PrevSpecialId + randomNum).ToString("D8");

            }
            catch (SqlException ex)
            {
                con.Close();
                return "";
            }
            catch (Exception ex)
            {
                con.Close();
                return "";
            }
        }


        private static long getRandomNumber()
        {
            Random rnd = new Random();
            long randnum = rnd.Next(3, 26);
            return randnum;
        }

        [WebMethod]
        public static String isSpecialIdUsed(String specialId)
        {
            try
            {
                int counter = 0;
                con.Open();
                String sql = "SELECT DiscountId FROM MnP_MasterDiscount mpmd WHERE mpmd.SpecialId=" + specialId;
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        counter++;
                    }
                }
                con.Close();
                if (counter > 0)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }

            }

            catch (Exception ex)
            {
                con.Close();
                return "false";
            }
        }

        [WebMethod]
        public static String isDicountEntrySame_AndSpecialDiscountCheck(DiscountModel data)
        {
            string response = "";
            try
            {
                String message = ""; String status = "";
                Int64 ParentGroupId; int counter = 0;
                String[] ECs = data.ExpressCenter.Split(',');
                var result = string.Join("','", ECs);
                String[] newfromDate = data.fromDate.Split('-');
                String[] newToDate = data.toDate.Split('-');
                
                DiscountModel rs = new DiscountModel();

                DateTime fromDatedt = Convert.ToDateTime(data.fromDate);

                if (fromDatedt < System.DateTime.Now.Date)
                {
                    return "date";
                }

 
                con.Open();
                int SpecialIsUsed= con.Query<int>(@"  Select [DiscountID] from MnP_MasterDiscount md  
                                                 where   
                                                 CAST('"+ data.fromDate + @"' AS DATE) between CAST(md.ValidFrom AS DATE) and CAST(md.ValidTo AS DATE) 
                                                 AND md.ServiceType ='" + data.serviceType + @"'    
                                                 AND md.DiscountValueType =" + data.discountType+@"   
                                                 AND md.DiscountValue="+data.discountValue+@"   
                                                 AND md.SpecialId='"+data.specialDiscountId+@"'
                                                 AND isnull(status,0)= '1'  
                                                 AND ISNULL(is_Approved, 0) = '1' 
                                                AND md.ExpressCenterCode in ('"+result+@"')  
                                                     UNION all   
                                                Select [DiscountID] from MnP_MasterDiscount md  
                                                 where  
                                                  CAST('"+ data.toDate + @"' AS DATE) between CAST(md.ValidFrom AS DATE) and CAST(md.ValidTo AS DATE)   
                                                 and md.ServiceType = '"+data.serviceType+@"'  
                                                  AND md.DiscountValueType ="+data.discountValue+ @" 
                                                  AND md.DiscountValue="+ data.discountType + @"                                                 
                                                     AND md.SpecialId='"+data.specialDiscountId + @"'
                                                   and isnull(status,0)= '1'  and isnull(is_Approved,0)='1' and md.ExpressCenterCode in ('"+result+"') ").Count();
                con.Close();
                if (SpecialIsUsed > 0)
                {
                    response= "SpecialUsed";
                }

                string sql = "Select [DiscountID] from MnP_MasterDiscount md  \n"
               + " where   \n"
               + " CAST('" + data.fromDate + "' AS DATE) between CAST(md.ValidFrom AS DATE) and CAST(md.ValidTo AS DATE) \n"
               + "AND md.ServiceType = '" + data.serviceType + "'   \n"
               + " AND md.MinCNCount>=" + data.minShipment + "    \n"
               + "  AND md.MaxCNCount<= " + data.maxShipment + "    \n "
               + "   AND md.MinWeight>=" + data.minShipmentWeight + "    \n "
               + "    AND md.MaxWeight<= " + data.maxShipmentWeight + " \n"
               + "   AND md.DiscountValueType =" + data.discountType + "   \n"
               + "  AND md.DiscountValue=" + data.discountValue + "   \n"
               + " AND isnull(status,0)= '1'  \n"
               + " AND ISNULL(is_Approved, 0) = '1'  AND md.ExpressCenterCode in ('" + result + "') "
                + "  UNION all  " +
               " Select [DiscountID] from MnP_MasterDiscount md  \n"
               + " where  \n"
               + "  CAST('" + data.toDate + "' AS DATE) between CAST(md.ValidFrom AS DATE) and CAST(md.ValidTo AS DATE)   \n"
               + "AND md.ServiceType = '" + data.serviceType + "'   \n"
               + " AND md.MinCNCount>=" + data.minShipment + "    \n"
               + "  AND md.MaxCNCount<=" + data.maxShipment + "    \n "
               + "   AND md.MinWeight>=" + data.minShipmentWeight + "    \n "
               + "    AND md.MaxWeight<=" + data.maxShipmentWeight + " \n"
               + "   AND md.DiscountValueType =" + data.discountType + "   \n"
               + "  AND md.DiscountValue=" + data.discountValue + "   \n"
               + " AND isnull(status,0)= '1' "
               + "AND isnull(is_Approved,0)='1'  AND md.ExpressCenterCode in ('" + result + "')";
                 
                //String sqlOld = "Select [DiscountID] from MnP_MasterDiscount md   where  '" + dateFrom + "' between md.ValidFrom and md.ValidTo  AND '" + dateTo + "' between md.ValidFrom and md.ValidTo   and md.ServiceType = '" + data.serviceType + "'   and isnull(is_Approved,0)= '1' and  md.ExpressCenterCode in ('" + result + "')";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        counter++;
                    }
                }
                con.Close();
                if (counter > 0)
                {
                    response= "false";
                }
                else
                {
                    response="true";
                }
            }
            catch (Exception ex)
            {
                response= "false";
            }
            finally
            {
                con.Close();
            }
            return response;

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static DiscountModelWithProfile GetDataFromGroupId(String groupId)
        {
            if (groupId != "" && groupId != null)
            {
                return GetDataFromGroupIdFunction(groupId);
            }
            else
            {
                return null;
            }
        }



        public static DiscountModelWithProfile GetDataFromGroupIdFunction(String groupId)
        {
            try
            {
                DiscountModelWithProfile rs = new DiscountModelWithProfile();
                String sql = "SELECT  [SDESC],[LDESC],[MinCNCount],[MaxCNCount],[MinWeight],[MaxWeight],[ServiceType],[DiscountValue],[DiscountValueType],[ValidFrom],[ValidTo],[IsSpecial],[SpecialId],[is_Approved] FROM[MnP_MasterDiscount] WHERE status=1 and parentDiscountId =" + groupId;
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    if (dr.Read())
                    {
                        rs.shortDescription = dr.GetString(0);
                        rs.longDescription = dr.GetString(1);
                        rs.minShipment = dr.GetInt32(2).ToString();
                        rs.maxShipment = dr.GetInt32(3).ToString();
                        rs.minShipmentWeight = Convert.ToString(dr.GetDecimal(4));
                        rs.maxShipmentWeight = Convert.ToString(dr.GetDecimal(5));
                        rs.serviceType = dr.GetString(6);
                        rs.discountValue = dr.GetDecimal(7).ToString();
                        rs.discountType = dr.GetInt32(8).ToString();
                        rs.fromDate = dr.GetDateTime(9).ToShortDateString();
                        rs.toDate = dr.GetDateTime(10).ToShortDateString();
                        rs.specialDiscountId = dr.IsDBNull(12) ? null : dr.GetString(12);
                        rs.is_Approved = dr.IsDBNull(13) ? null : dr.GetBoolean(13).ToString();
                    }
                }
                rs.Profile_User = Profile_User;

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
        public static BranchsZonesEcs GetZoneOneOrAll(String groupId)
        {
            if (groupId != "" && groupId != null)
            {
                return new DiscountRepo().GetZoneOneOrAll(groupId);
            }
            else
            {
                return null;
            }
        }


        [WebMethod]
        public static String ActivateDiscount(String groupId, String decision)
        {
            if (U_ID == "" || U_ID == null)
            {
                return "Please login again";
            }
            else
            {
                return new DiscountRepo().ActivateDiscount(groupId, decision, U_ID);
            }
        }
        [WebMethod]
        public static String SaveDiscountEntry(DiscountModel data)
        {
            if (U_ID == "" || U_ID == null)
            {
                return "Please login again";
            }
            else
            {
                data.CreatedBy = U_ID;
                return new DiscountRepo().SaveDiscount(data);
            }
        }
        [WebMethod]
        public static String EditDiscountEntry(DiscountModel data)
        {
            if (U_ID == "" || U_ID == null)
            {
                return "Please login again";
            }
            else
            {
                data.CreatedBy = U_ID;
                return new DiscountRepo().EditDiscountEntry(data);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String retailDesktop = "";
                //if (Session["RetailDesktop"]!= null)
                //{
                //    retailDesktop = Session["RetailDesktop"].ToString();
                //    if (retailDesktop!="123456")
                //    {
                //        Response.Redirect("~/login");
                //    }
                //}
                //else
                //{
                //    Response.Redirect("~/login");
                //}
                U_ID = Session["U_ID"].ToString();
                Profile_User = Session["PROFILE"].ToString();
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }
        }
    }
    public class DiscountExpressCenterModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string branch { get; set; }
        public string zone { get; set; }

    }
    public class DiscountModelWithProfile
    {
        public string Discount_ID { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string zone { get; set; }
        public string Branch { get; set; }
        public string ExpressCenter { get; set; }
        public string serviceType { get; set; }
        public string discountType { get; set; }
        public string discountValue { get; set; }
        public string shortDescription { get; set; }
        public string longDescription { get; set; }
        public string minShipment { get; set; }
        public string maxShipment { get; set; }
        public string minShipmentWeight { get; set; }
        public string maxShipmentWeight { get; set; }
        public string specialDiscountId { get; set; }
        public string CreatedBy { get; set; }
        public string parentGroupId { get; set; }

        public string is_Approved { get; set; }

        public String Profile_User { get; set; }

    }

    public class DiscountModel
    {
        public string Discount_ID { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string zone { get; set; }
        public string Branch { get; set; }
        public string ExpressCenter { get; set; }
        public string serviceType { get; set; }
        public string discountType { get; set; }
        public string discountValue { get; set; }
        public string shortDescription { get; set; }
        public string longDescription { get; set; }
        public string minShipment { get; set; }
        public string maxShipment { get; set; }
        public string minShipmentWeight { get; set; }
        public string maxShipmentWeight { get; set; }
        public string specialDiscountId { get; set; }
        public string CreatedBy { get; set; }
        public string parentGroupId { get; set; }

        public string is_Approved { get; set; }

    }
    public class MyExpressCenters
    {
        public String ExpressCenter { get; set; }
        public string branch { get; set; }
        public string zone { get; set; }

    }
    public class BranchsZonesEcs
    {
        public List<String> ECs { get; set; }

        public List<String> Branches { get; set; }

        public List<String> Zones { get; set; }

    }
    public class RetailDiscountDropdownModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string branch { get; set; }
        public string zone { get; set; }

    }

    public class DiscountRepo
    {
        SqlConnection con;

        public DiscountRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        }


        private long getRandomNumber()
        {
            Random rnd = new Random();
            long randnum = rnd.Next(3, 26);
            return randnum;
        }

        public List<RetailDiscountDropdownModel> GetServices()
        {
            try
            {
                List<RetailDiscountDropdownModel> rs = new List<RetailDiscountDropdownModel>();
                con.Open();
                String sql = "SELECT [serviceTypeName] AS VALUE,[serviceTypeName] AS TEXT FROM ServiceTypes_New WHERE Products='Domestic' AND [status]=1 ORDER BY serviceTypeName";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new RetailDiscountDropdownModel { Value = dr.GetString(0), Text = dr.GetString(1) });
                    }
                }
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



        public List<RetailDiscountDropdownModel> GetZones()
        {
            try
            {
                List<RetailDiscountDropdownModel> rs = new List<RetailDiscountDropdownModel>();
                con.Open();
                String sql = "select [zoneCode] as Value,[Name] as Text from Zones WHERE status=1 AND Region IS NOT NULL ORDER BY name";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new RetailDiscountDropdownModel { Value = dr.GetString(0), Text = dr.GetString(1) });
                    }
                }
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

        public List<RetailDiscountDropdownModel> GetBranches(String zone)
        {
            try
            {
                List<RetailDiscountDropdownModel> rs = new List<RetailDiscountDropdownModel>();
                con.Open();

                String sql;
                zone = zone.ToLower();
                if (zone.ToString() != "all")
                {
                    sql = "select[branchCode] as Value,[Name] as Text from Branches WHERE zoneCode='" + zone + "' AND[status]=1 ORDER BY name";
                }
                else
                {
                    sql = "select[branchCode] as Value,[Name] as Text from Branches WHERE [status]=1 ORDER BY name";
                }
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new RetailDiscountDropdownModel { Value = dr.GetString(0), Text = dr.GetString(1) });
                    }
                }
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

        public List<RetailDiscountDropdownModel> GetExpressCenters(String zone, String branch)
        {
            try
            {
                List<RetailDiscountDropdownModel> rs = new List<RetailDiscountDropdownModel>();
                con.Open();

                String sql;
                branch = branch.ToLower();
                zone = zone.ToLower();

                if (branch.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT,ec.bid branch, z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode = ec.bid   INNER JOIN Zones z ON z.zoneCode = b.zoneCode    WHERE ec.bid = " + branch + " AND ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name ";
                }
                else if (zone.ToString() != "all")
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT  ,ec.bid branch,b.zoneCode zone FROM ExpressCenters ec	left JOIN Branches b ON b.zoneCode=" + zone + " WHERE ec.bid = b.branchCode AND ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name";
                }
                else
                {
                    sql = "SELECT ec.expressCenterCode as Value,ec.name +' ('+ec.expressCenterCode+')' TEXT ,ec.bid branch,z.zoneCode zone   FROM ExpressCenters ec INNER JOIN Branches b ON b.branchCode=ec.bid INNER JOIN Zones z ON z.zoneCode=b.zoneCode  WHERE  ec.[status]=1 AND ec.Center_type=1 ORDER BY ec.name";
                }
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new RetailDiscountDropdownModel { Value = dr.GetString(0), Text = dr.GetString(1), branch = dr.GetString(2), zone = dr.GetString(3) });
                    }
                }
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
        public DiscountModel GetDataFromGroupId(String groupId)
        {
            try
            {
                DiscountModel rs = new DiscountModel();
                String sql = "SELECT  [SDESC],[LDESC],[MinCNCount],[MaxCNCount],[MinWeight],[MaxWeight],[ServiceType],[DiscountValue],[DiscountValueType],[ValidFrom],[ValidTo],[IsSpecial],[SpecialId],[is_Approved] FROM[MnP_MasterDiscount] WHERE status=1 and parentDiscountId =" + groupId;
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    if (dr.Read())
                    {
                        rs.shortDescription = dr.GetString(0);
                        rs.longDescription = dr.GetString(1);
                        rs.minShipment = dr.GetInt32(2).ToString();
                        rs.maxShipment = dr.GetInt32(3).ToString();
                        rs.minShipmentWeight = Convert.ToString(dr.GetDecimal(4));
                        rs.maxShipmentWeight = Convert.ToString(dr.GetDecimal(5));
                        rs.serviceType = dr.GetString(6);
                        rs.discountValue = dr.GetDecimal(7).ToString();
                        rs.discountType = dr.GetInt32(8).ToString();
                        rs.fromDate = dr.GetDateTime(9).ToShortDateString();
                        rs.toDate = dr.GetDateTime(10).ToShortDateString();
                        rs.specialDiscountId = dr.IsDBNull(12) ? null : dr.GetString(12);
                        rs.is_Approved = dr.IsDBNull(13) ? null : dr.GetBoolean(13).ToString();
                    }
                }
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
        public BranchsZonesEcs GetZoneOneOrAll(String groupId)
        {
            try
            {
                BranchsZonesEcs AllList = new BranchsZonesEcs();
                con.Open();

                List<String> listzone = new List<string>();
                String rs = "";
                String sqlZone = "SELECT distinct mpmd.ZoneCode FROM MnP_MasterDiscount mpmd WHERE status=1 and mpmd.parentDiscountId =" + groupId;
                using (var cmd = new SqlCommand(sqlZone, con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listzone.Add(dr.GetString(0));
                        }
                    }
                    if (listzone.Count > 1)
                    {
                        listzone = null;
                        listzone = new List<string>();
                        listzone.Add("All");
                    }
                }
                List<String> listbranch = new List<string>();
                String sqlBranch = "SELECT distinct mpmd.BranchCode FROM MnP_MasterDiscount mpmd WHERE status=1 and mpmd.parentDiscountId =" + groupId;

                using (var cmd2 = new SqlCommand(sqlBranch, con))
                {
                    using (SqlDataReader dr2 = cmd2.ExecuteReader())
                    {
                        while (dr2.Read())
                        {
                            listbranch.Add(dr2.GetString(0));
                        }
                    }
                    if (listbranch.Count > 1)
                    {
                        listbranch = null;
                        listbranch = new List<string>();
                        listbranch.Add("All");
                    }
                }
                String sqlEC = "SELECT distinct mpmd.ExpressCenterCode FROM MnP_MasterDiscount mpmd WHERE status=1 and mpmd.parentDiscountId =" + groupId;
                List<String> listEC = new List<string>();

                using (var cmd = new SqlCommand(sqlEC, con))
                {
                    SqlDataReader dr3;
                    dr3 = cmd.ExecuteReader();

                    while (dr3.Read())
                    {
                        listEC.Add(dr3.GetString(0));
                    }
                }
                con.Close();
                AllList.Zones = listzone;
                AllList.Branches = listbranch;
                AllList.ECs = listEC;
                return AllList;
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
         

        public String ActivateDiscount(String groupId, String decision, String createdBy)
        {
            try
            {
                con.Open();
                String sql = "UPDATE MnP_MasterDiscount  SET   ApprovedOn = GETDATE(),ApprovedBy = @createdBy,is_Approved = @is_approved  WHERE parentDiscountId = @groupId and status=1";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    SqlDataReader dr;

                    cmd2.Parameters.AddWithValue("@createdBy", createdBy);
                    cmd2.Parameters.AddWithValue("@is_approved", decision);
                    cmd2.Parameters.AddWithValue("@groupId", groupId);



                    int cc = cmd2.ExecuteNonQuery();
                    con.Close();

                    if (cc > 0)
                    {
                        if (decision == "1")
                        {
                            return "Discount offer approved successfully";

                        }
                        else
                        {
                            return "Discount offer rejected successfully";

                        }
                    }
                    else
                    {
                        return "Group id not found!";
                    }
                }
            }
            catch (SqlException ex)
            {
                con.Close();
                return "Error cannot activate!";
            }
            catch (Exception ex)
            {
                con.Close();
                return "Error cannot activate!";
            }
        }


        public String SaveDiscount(DiscountModel data)
        { 
            String message = "1"; String status = "";
            Int64 ParentGroupId;
            String[] ECs = data.ExpressCenter.Split(',');
            String[] branches = data.Branch.Split(',');
            String[] zones = data.zone.Split(',');
            DateTime fromDateType = DateTime.Now;
            DateTime toDateType = DateTime.Now;
            using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString)))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                transaction = conn.BeginTransaction("Insert discount entries");
                command.Connection = conn;
                command.Transaction = transaction;
                try
                {
             

                    fromDateType = DateTime.Parse(data.fromDate);
                    toDateType = DateTime.Parse(data.toDate);

                    String sqlOnceToGetGroupId = "insert into [MnP_MasterDiscount] ([SDESC],[LDESC],[MinCNCount],[MaxCNCount],[MinWeight],[MaxWeight],[ServiceType],[DiscountValue],[DiscountValueType],[ValidFrom], [ValidTo],[STATUS],[CreatedOn],[CreatedBy],[ZoneCode],[BranchCode],[ExpressCenterCode],[IsSpecial],[SpecialId]) OUTPUT INSERTED.DiscountID values(@SDESC,@LDESC,@MinCount,@MaxCount,@MinWeight,@MaxWeight,@serviceType,@DiscountValue,@DiscountType,@ValidFrom,@ValidTo,@STATUS,GETDATE(),@CreatedBy,@ZoneCode,@BranchCode,@ExpressCenterCode,@IsSpecial,@SpecialId);";

                    command.CommandText = sqlOnceToGetGroupId;
                    command.Parameters.AddWithValue("SDESC", data.shortDescription);
                    command.Parameters.AddWithValue("LDESC", data.longDescription);
                    command.Parameters.AddWithValue("MinCount", data.minShipment);
                    command.Parameters.AddWithValue("MaxCount", data.maxShipment);
                    command.Parameters.AddWithValue("MinWeight", data.minShipmentWeight);
                    command.Parameters.AddWithValue("MaxWeight", data.maxShipmentWeight);
                    command.Parameters.AddWithValue("serviceType", data.serviceType);
                    command.Parameters.AddWithValue("DiscountValue", data.discountValue);
                    command.Parameters.AddWithValue("DiscountType", data.discountType);
                    command.Parameters.AddWithValue("ValidFrom", fromDateType);
                    command.Parameters.AddWithValue("ValidTo", toDateType);
                    command.Parameters.AddWithValue("STATUS", 1);
                    command.Parameters.AddWithValue("CreatedBy", data.CreatedBy);
                    command.Parameters.AddWithValue("ZoneCode", zones[0]);
                    command.Parameters.AddWithValue("BranchCode", branches[0]);
                    command.Parameters.AddWithValue("ExpressCenterCode", ECs[0]);

                    if (data.specialDiscountId.Length > 0)
                    {
                        command.Parameters.AddWithValue("IsSpecial", 1);
                        command.Parameters.AddWithValue("SpecialId", data.specialDiscountId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("IsSpecial", 0);
                        command.Parameters.AddWithValue("SpecialId", DBNull.Value);
                    }
                    ParentGroupId = (Int64)command.ExecuteScalar();
                    ////updating the newly added record with its own PK identity
                    String sqlUpdateRecordForParentId = "update [MnP_MasterDiscount] set parentDiscountId=" + ParentGroupId + " where DiscountID=" + ParentGroupId;
                    command.CommandText = sqlUpdateRecordForParentId;
                    int check = command.ExecuteNonQuery();
                    if (check <= 0)
                    {
                        throw new Exception();
                    }

                    DataTable dt = new DataTable();
                    dt.Clear();
                    dt.Columns.Add("SDESC", typeof(string));
                    dt.Columns.Add("LDESC", typeof(string));
                    dt.Columns.Add("MinCNCount", typeof(int));
                    dt.Columns.Add("MaxCNCount", typeof(int));
                    dt.Columns.Add("MinWeight", typeof(float));
                    dt.Columns.Add("MaxWeight", typeof(float));
                    dt.Columns.Add("ServiceType", typeof(string));
                    dt.Columns.Add("DiscountValue", typeof(float));
                    dt.Columns.Add("DiscountValueType", typeof(int));
                    dt.Columns.Add("ValidFrom", typeof(DateTime));
                    dt.Columns.Add("ValidTo", typeof(DateTime));
                    dt.Columns.Add("STATUS", typeof(Boolean));
                    dt.Columns.Add("CreatedBy", typeof(string));
                    dt.Columns.Add("ZoneCode", typeof(string));
                    dt.Columns.Add("BranchCode", typeof(string));
                    dt.Columns.Add("ExpressCenterCode", typeof(string));
                    dt.Columns.Add("IsSpecial", typeof(int));
                    dt.Columns.Add("SpecialId", typeof(string));
                    dt.Columns.Add("parentDiscountId", typeof(Int64));

                    if (data.specialDiscountId.Length > 0)
                    {
                        for (int i = 1; i < ECs.Length; i++)
                        {
                            object[] o = { data.shortDescription, data.longDescription, data.minShipment, data.maxShipment, data.minShipmentWeight, data.maxShipmentWeight, data.serviceType, data.discountValue, data.discountType, fromDateType, toDateType, 1, data.CreatedBy, zones[i], branches[i], ECs[i], 1, data.specialDiscountId, ParentGroupId };
                            dt.Rows.Add(o);
                        }
                    }
                    else
                    {
                        for (int i = 1; i < ECs.Length; i++)
                        {
                            object[] o = { data.shortDescription, data.longDescription, data.minShipment, data.maxShipment, data.minShipmentWeight, data.maxShipmentWeight, data.serviceType, data.discountValue, data.discountType, fromDateType, toDateType, 1, data.CreatedBy, zones[i], branches[i], ECs[i], 0, DBNull.Value, ParentGroupId };
                            dt.Rows.Add(o);
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        command.CommandText = "MnP_MasterDiscount_sp";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@tbl", dt);
                        command.Parameters.Add("@returnMsg", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        message = command.Parameters["@returnMsg"].SqlValue.ToString().ToUpper();
                    }
                    transaction.Commit();
                    if (message != "1")
                    {
                        status = "Error Inserting discount record!: " + message;
                        throw new Exception();
                    }
                    else
                    {
                        status = "Group Id: " + ParentGroupId + " .Discount Record Successfully Saved, pending activation";
                        conn.Close();
                    }
                    return status;
                }

                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        return "Error inserting discount record: "+ex.Message.ToString();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.;
                        return "Error inserting discount record, rollback transaction failed: "+ex2;

                    }
                }
            }
        }

        public String EditDiscountEntry(DiscountModel data)
        {
            String message = ""; String status = "";
            Int64 ParentGroupId;
            String[] ECs = data.ExpressCenter.Split(',');
            String[] branches = data.Branch.Split(',');
            String[] zones = data.zone.Split(',');
            DateTime fromDateType = DateTime.Now;
            DateTime toDateType = DateTime.Now;
            try
            {
                fromDateType = DateTime.Parse(data.fromDate);
                toDateType = DateTime.Parse(data.toDate);

                int counter = 0;
                String sqlZone = "Select DiscountId from MnP_MasterDiscount where parentDiscountId =" + data.parentGroupId;
                con.Open();
                using (var cmd = new SqlCommand(sqlZone, con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            counter++;
                        }
                    }

                    if (counter <= 0)
                    {
                        return "Error no such entries exists with group id: " + data.parentGroupId;
                    }
                }
            }
            catch (Exception er)
            {
                return "Error :"+er.Message.ToString();
            }

            using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString)))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;

                transaction = conn.BeginTransaction("Updating discount entries");
                command.Connection = conn;
                command.Transaction = transaction;
                try
                {
                    String SqlUpdateStatus = "UPDATE MnP_MasterDiscount SET [STATUS]=0,[ModifyOn]=GETDATE(),[ModifyBy]=@CreatedBy WHERE parentDiscountId=@parent";
                    command.CommandText = SqlUpdateStatus;
                    command.Parameters.AddWithValue("CreatedBy", data.CreatedBy);
                    command.Parameters.AddWithValue("parent", data.parentGroupId);

                    command.ExecuteNonQuery();

                    DataTable dt = new DataTable();
                    dt.Clear();
                    dt.Columns.Add("SDESC", typeof(string));
                    dt.Columns.Add("LDESC", typeof(string));
                    dt.Columns.Add("MinCNCount", typeof(int));
                    dt.Columns.Add("MaxCNCount", typeof(int));
                    dt.Columns.Add("MinWeight", typeof(float));
                    dt.Columns.Add("MaxWeight", typeof(float));
                    dt.Columns.Add("ServiceType", typeof(string));
                    dt.Columns.Add("DiscountValue", typeof(float));
                    dt.Columns.Add("DiscountValueType", typeof(int));
                    dt.Columns.Add("ValidFrom", typeof(DateTime));
                    dt.Columns.Add("ValidTo", typeof(DateTime));
                    dt.Columns.Add("STATUS", typeof(Boolean));
                    dt.Columns.Add("CreatedBy", typeof(string));
                    dt.Columns.Add("ZoneCode", typeof(string));
                    dt.Columns.Add("BranchCode", typeof(string));
                    dt.Columns.Add("ExpressCenterCode", typeof(string));
                    dt.Columns.Add("IsSpecial", typeof(int));
                    dt.Columns.Add("SpecialId", typeof(string));
                    dt.Columns.Add("parentDiscountId", typeof(Int64));

                    if (data.specialDiscountId.Length > 0)
                    {
                        for (int i = 0; i < ECs.Length; i++)
                        {
                            object[] o = { data.shortDescription, data.longDescription, data.minShipment, data.maxShipment, data.minShipmentWeight, data.maxShipmentWeight, data.serviceType, data.discountValue, data.discountType, fromDateType, toDateType, 1, data.CreatedBy, zones[i], branches[i], ECs[i], 1, data.specialDiscountId, data.parentGroupId };
                            dt.Rows.Add(o);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ECs.Length; i++)
                        {
                            object[] o = { data.shortDescription, data.longDescription, data.minShipment, data.maxShipment, data.minShipmentWeight, data.maxShipmentWeight, data.serviceType, data.discountValue, data.discountType, fromDateType, toDateType, 1, data.CreatedBy, zones[i], branches[i], ECs[i], 0, DBNull.Value, data.parentGroupId };
                            dt.Rows.Add(o);
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        command.CommandText = "MnP_MasterDiscount_sp";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@tbl", dt);
                        command.Parameters.Add("@returnMsg", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        message = command.Parameters["@returnMsg"].SqlValue.ToString().ToUpper();
                    }
                    transaction.Commit();
                    if (message != "1")
                    {
                        status = "Error Inserting discount record!: " + message;
                        throw new Exception();
                    }
                    else
                    {
                        status = "Previous record successfully updated in database, group id: " + data.parentGroupId + " pending activation";
                        conn.Close();
                    }
                    return status;
                }

                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        return "Error inserting discount record, "+ex.Message.ToString();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.;
                        return "Error inserting discount record, rollback transaction failed: "+ex2.Message.ToString();

                    }
                }
            }


        }

    }
}