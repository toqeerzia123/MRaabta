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
    public partial class StockType : System.Web.UI.Page
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


        #region method to get existing types
        [WebMethod]
        public static List<dynamic> GetStockTypes()
        {
            try
            {
                con.Open();
                var rs = con.Query<dynamic>(@"select ID, TypeName, Details Description from MnP_CNIssue_Type where isActive=1;");
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
        public static string SaveTypeEntries(List<StockTypeModel> model)
        {
            con.Open();
            object reqDetails;
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    foreach (var item in model)
                    {

                        reqDetails = con.Execute(@"insert into MnP_CNIssue_Type
                                                  (TypeName,Details,isActive,CreatedBy,CreatedOn)
                                                   values
                                                  (@TypeName,@Details,1,@CreatedBy,getdate());",
                        new
                        {
                            TypeName = item.Type,
                            Details = item.Detail,
                            CreatedBy = HttpContext.Current.Session["U_ID"].ToString()
                        }, transaction: tran);
                    }

                    tran.Commit();
                    return "Type saved successfully ";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return "Failed to save Type, Please contact IT for support.";
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