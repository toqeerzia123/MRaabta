using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace MRaabta.Files
{
    public partial class ExistingRetailDiscounts : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            GetActiveDiscountsGrid();
            GetPendingDiscountsGrid();
            GetInactiveDiscountsGrid();

        }
        protected void GetActiveDiscountsGrid()
        {
            DataTable dt = new DataTable();
            try
            {

                string sql = "SELECT md.parentDiscountId [Group Id],z.name zone,b.name branch,ec.name +' ('+ec.expressCenterCode+')' expressCenter,Convert(varchar,[ValidFrom],106) [Valid From],Convert(varchar,[ValidTo],106) [Valid To],[ServiceType] [Service Type],md.[SDESC] [Short Description],md.[LDESC] [Long Description],[MinCNCount] [Min. CN Count],[MaxCNCount] [Max. CN Count],[MinWeight] [Min. Weight],[MaxWeight] [Max. Weight], \n"
               + "        CASE WHEN md.DiscountValueType='1' THEN 'Percentage' ELSE 'Amount' END AS [Discount Value Type],[DiscountValue] [Discount Value],[SpecialId] [Special Id] \n"
               + "FROM   [MnP_MasterDiscount] md \n"
               + "INNER JOIN Zones z ON z.zoneCode=md.ZoneCode \n"
               + "INNER JOIN Branches b ON b.branchCode=md.BranchCode \n"
               + "INNER JOIN ExpressCenters ec ON ec.ExpressCenterCode = md.ExpressCenterCode \n"
               + "WHERE  md.STATUS = 1 \n"
               + "		ANd md.is_Approved='1' \n"
               + "		AND md.ValidTo>=CAST(GETDATE() AS date)  ORDER BY md.parentDiscountId,ec.name";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter dr;
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                ActiveGridTable(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }
        protected void GetInactiveDiscountsGrid()
        {
            DataTable dt = new DataTable();
            try
            {

                string sql = "SELECT md.parentDiscountId [Group Id],z.name zone,b.name branch,ec.name +' ('+ec.expressCenterCode+')' expressCenter,Convert(varchar,[ValidFrom],106) [Valid From],Convert(varchar,[ValidTo],106) [Valid To],[ServiceType] [Service Type],md.[SDESC] [Short Description],md.[LDESC] [Long Description],[MinCNCount] [Min. CN Count],[MaxCNCount] [Max. CN Count],[MinWeight] [Min. Weight],[MaxWeight] [Max. Weight], \n"
               + "        CASE WHEN md.DiscountValueType='1' THEN 'Percentage' ELSE 'Amount' END AS [Discount Value Type],[DiscountValue] [Discount Value],[SpecialId] [Special Id] \n"
               + "FROM   [MnP_MasterDiscount] md \n"
               + "INNER JOIN Zones z ON z.zoneCode=md.ZoneCode \n"
               + "INNER JOIN Branches b ON b.branchCode=md.BranchCode \n"
               + "INNER JOIN ExpressCenters ec ON ec.ExpressCenterCode = md.ExpressCenterCode \n"
               + "WHERE  md.STATUS = 1 \n"
               + "		ANd md.is_Approved='1' \n"
               + "		AND md.ValidTo<=CAST(GETDATE() AS date)  ORDER BY md.parentDiscountId,ec.name";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter dr;
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                InactiveGridTable(dt);

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }


        public void GetPendingDiscountsGrid()
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT md.parentDiscountId [Group Id],z.name zone,b.name branch,ec.name +' ('+ec.expressCenterCode+')' expressCenter,Convert(varchar,[ValidFrom],106) [Valid From],Convert(varchar,[ValidTo],106) [Valid To],[ServiceType] [Service Type],md.[SDESC] [Short Description],md.[LDESC] [Long Description],[MinCNCount] [Min. CN Count],[MaxCNCount] [Max. CN Count],[MinWeight] [Min. Weight],[MaxWeight] [Max. Weight], \n"
               + "        CASE WHEN md.DiscountValueType='1' THEN 'Percentage' ELSE 'Amount' END AS [Discount Value Type],[DiscountValue] [Discount Value],[SpecialId] [Special Id] \n"
               + "FROM   [MnP_MasterDiscount] md \n"
               + "INNER JOIN Zones z ON z.zoneCode=md.ZoneCode \n"
               + "INNER JOIN Branches b ON b.branchCode=md.BranchCode \n"
               + "INNER JOIN ExpressCenters ec ON ec.ExpressCenterCode = md.ExpressCenterCode \n"
               + "WHERE  md.STATUS = 1 \n"
               + "		ANd md.is_Approved is null \n"
               + "		AND md.ValidTo>=CAST(GETDATE() AS date)  ORDER BY md.parentDiscountId,ec.name";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataAdapter oda = new SqlDataAdapter(cmd2);
                    oda.Fill(dt);
                }
                PendingGridTable(dt);
                con.Close();
            }

            catch (Exception ex)
            {
                con.Close();
            }
        }
        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true)]
        //public static List<DiscountsDataModel> GetActiveDiscounts(){
        //    try
        //    {
        //        List<DiscountsDataModel> rs = new List<DiscountsDataModel>();

        //        string sql = "SELECT md.[SDESC],md.[LDESC],[MinCNCount],[MaxCNCount],[MinWeight],[MaxWeight],[ServiceType],[DiscountValue], \n"
        //       + "       [DiscountValueType],[ValidFrom],[ValidTo],[IsSpecial],[SpecialId], \n"
        //       + "       [is_Approved],z.name zone,b.name branch,ec.name +' ('+ec.expressCenterCode+')' expressCenter,md.parentDiscountId \n"
        //       + "FROM   [MnP_MasterDiscount] md \n"
        //       + "INNER JOIN Zones z ON z.zoneCode=md.ZoneCode \n"
        //       + "INNER JOIN Branches b ON b.branchCode=md.BranchCode \n"
        //       + "INNER JOIN ExpressCenters ec ON ec.ExpressCenterCode = md.ExpressCenterCode \n"
        //       + "WHERE  md.STATUS = 1 \n"
        //       + "		ANd md.is_Approved='1' \n"
        //       + "		AND md.ValidTo>=CAST(GETDATE() AS date)  ORDER BY md.parentDiscountId,ec.name";
        //        using (var cmd2 = new SqlCommand(sql, con))
        //        {
        //            con.Open();
        //            SqlDataReader dr;
        //            dr = cmd2.ExecuteReader();

        //            while (dr.Read())
        //            {
        //                rs.Add(new DiscountsDataModel { 
        //                    shortDescription = dr.GetString(0),longDescription= dr.GetString(1),minShipment= dr.GetInt32(2).ToString(),
        //                    maxShipment = dr.GetInt32(3).ToString(),minShipmentWeight = Convert.ToString(dr.GetDecimal(4)),
        //                    maxShipmentWeight = Convert.ToString(dr.GetDecimal(5)),serviceType= dr.GetString(6),
        //                    discountValue= dr.GetDecimal(7).ToString(),
        //                    discountType = dr.GetInt32(8).ToString(),
        //                    fromDate = dr.GetDateTime(9).ToShortDateString(),
        //                    toDate = dr.GetDateTime(10).ToShortDateString(),
        //                    specialDiscountId= dr.IsDBNull(12) ? null : dr.GetString(12),
        //                    is_Approved = dr.IsDBNull(13) ? null : dr.GetBoolean(13).ToString(),
        //                    zone= dr.GetString(14),
        //                    branch = dr.GetString(15),
        //                    expressCenter= dr.GetString(16),
        //                    parentGroupId=dr.GetInt64(17).ToString()
        //                });
        //            }
        //        }
        //        con.Close();
        //        return rs;
        //    }
        //    catch (Exception ex)
        //    {
        //        con.Close();
        //        return null;
        //    }
        //}



        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json,UseHttpGet =true)]
        public static String GetInactiveDiscounts()
        {
            try
            {
                List<DiscountsDataModel> rs = new List<DiscountsDataModel>();

                string sql = "SELECT md.[SDESC],md.[LDESC],[MinCNCount],[MaxCNCount],[MinWeight],[MaxWeight],[ServiceType],[DiscountValue], \n"
               + "       [DiscountValueType],[ValidFrom],[ValidTo],[IsSpecial],[SpecialId], \n"
               + "       [is_Approved],z.name zone,b.name branch,ec.name +' ('+ec.expressCenterCode+')' expressCenter,md.parentDiscountId \n"
               + "FROM   [MnP_MasterDiscount] md \n"
               + "INNER JOIN Zones z ON z.zoneCode=md.ZoneCode \n"
               + "INNER JOIN Branches b ON b.branchCode=md.BranchCode \n"
               + "INNER JOIN ExpressCenters ec ON ec.ExpressCenterCode = md.ExpressCenterCode \n"
               + "WHERE  md.STATUS = 1 \n"
               + "		ANd md.is_Approved='1' \n"
               + "		AND md.ValidTo<=CAST(GETDATE() AS date)  ORDER BY md.parentDiscountId,ec.name";
                using (var cmd2 = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr;
                    dr = cmd2.ExecuteReader();

                    while (dr.Read())
                    {
                        rs.Add(new DiscountsDataModel
                        {
                            shortDescription = dr.GetString(0),
                            longDescription = dr.GetString(1),
                            minShipment = dr.GetInt32(2).ToString(),
                            maxShipment = dr.GetInt32(3).ToString(),
                            minShipmentWeight = Convert.ToString(dr.GetDecimal(4)),
                            maxShipmentWeight = Convert.ToString(dr.GetDecimal(5)),
                            serviceType = dr.GetString(6),
                            discountValue = dr.GetDecimal(7).ToString(),
                            discountType = dr.GetInt32(8).ToString(),
                            fromDate = dr.GetDateTime(9).ToShortDateString(),
                            toDate = dr.GetDateTime(10).ToShortDateString(),
                            specialDiscountId = dr.IsDBNull(12) ? null : dr.GetString(12),
                            is_Approved = dr.IsDBNull(13) ? null : dr.GetBoolean(13).ToString(),
                            zone = dr.GetString(14),
                            branch = dr.GetString(15),
                            expressCenter = dr.GetString(16),
                            parentGroupId = dr.GetInt64(17).ToString()
                        });
                    }
                }
                con.Close();

                //var serializerr = new JavaScriptSerializer() { MaxJsonLength = 500000000 };

                //// Perform your serialization
                //serializerr.Serialize(rs);


                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = 500000000;

                var rse = JsonConvert.SerializeObject(rs);
                return rse;
            }

            catch (Exception ex)
            {
                con.Close();
                return null;
            }
        }
        private void ActiveGridTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;
                sb.Append("<tr>");


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("<td style='text-align:center;font-size:12px'>" + serial + "</td>");

                    }
                    DataColumn columnSingle = dt.Columns[i];
                    sb.Append("<td style='text-align:center;font-size:12px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");

                }

                sb.Append("</tr>");
            }


            ActiveLiteral.Text = sb.ToString();
        }
        private void InactiveGridTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;
                sb.Append("<tr>");


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("<td style='text-align:center;font-size:12px'>" + serial + "</td>");

                    }
                    DataColumn columnSingle = dt.Columns[i];
                    sb.Append("<td style='text-align:center;font-size:12px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                }

                sb.Append("</tr>");
            }


            InactiveLiteral.Text = sb.ToString();
        }
        private void PendingGridTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int serial = 0;
            int rowMax = dt.Rows.Count;
            for (int j = 0; j < rowMax; j++)
            {
                DataRow rowSingle = dt.Rows[j];
                serial++;
                sb.Append("<tr>");


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("<td style='text-align:center;font-size:12px'>" + serial + "</td>");

                    }
                    DataColumn columnSingle = dt.Columns[i];
                    sb.Append("<td style='text-align:center;font-size:12px'>" + rowSingle[columnSingle.ColumnName].ToString() + "</td>");
                }

                sb.Append("</tr>");
            }


            PendingLiteral.Text = sb.ToString();
        }
    }


    public class DiscountsDataModel
    {
        public string Discount_ID { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string zone { get; set; }
        public string branch { get; set; }
        public string expressCenter { get; set; }
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
}