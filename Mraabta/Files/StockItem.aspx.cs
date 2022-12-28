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
    public partial class StockItem : System.Web.UI.Page
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

        [WebMethod]
        public static List<DropDownModel> GetStockTypes()
        {
            try
            {
                con.Open();
                var rs = con.Query<DropDownModel>(@"select ID as Value, TypeName as Text from MnP_CNIssue_Type where isActive=1;");
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
        public static List<DropDownModel> GetStockUnits()
        {
            try
            {
                con.Open();
                var rs = con.Query<DropDownModel>(@"select ID as Value, Name as Text from Mnp_CnIssue_MeasurementUnit where isActive=1");
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
        public static List<StockProductModel> GetStockItems(string OracleCode)
        {
            try
            {
                con.Open();
                var rs = con.Query<StockProductModel>(@"  select OracleCode, ProductName,t.TypeName,
                                                case when isnull(p.isSerialised,0)=0 then 'No' else 'Yes' end isSerialised, u.Name Unit from MnP_CNIssue_Product p
                                                inner join Mnp_CnIssue_MeasurementUnit u on u.ID=p.Unit
                                                inner join MnP_CNIssue_Type t on t.ID=p.TypeId
                                                where p.isActive=1 and OracleCode='" + OracleCode+"'");
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
        public static string VerifyOracleCode(string OracleCode)
        {
            try
            {
                con.Open();
                var rs = con.QueryFirstOrDefault<string>(@"SELECT OracleCode FROM MnP_CNIssue_Product p WHERE p.isActive=1 AND p.OracleCode='" + OracleCode + "'");
                return rs;
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
        public static string SaveitemEntries(List<StockProductModel> model)
        {
            con.Open();
            object reqDetails;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    foreach (var item in model)
                    {

                        reqDetails = con.Execute(@"insert into MnP_CNIssue_Product
                    (TypeId,ProductName,OracleCode,isSerialised,Unit,isActive,CreatedBy,CreatedOn)
                    values
                    (@TypeId,@ProductName,@OracleCode,@isSerialised,@Unit,1,@CreatedBy,getdate())",
                        new
                        {
                            OracleCode = item.OracleCode,
                            ProductName = item.ProductName,
                            TypeId = item.TypeName,
                            isSerialised = item.isSerialised,
                            Unit = item.Unit,
                            CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                        }, transaction: tran);
                    }

                    tran.Commit();
                    return "Item saved successfully ";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to save Item, Please contact IT for support.";
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}