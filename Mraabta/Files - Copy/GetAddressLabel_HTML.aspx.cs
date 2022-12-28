using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRaabta.Files
{
    public partial class GetAddressLabel_HTML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateReport();
            }
        }

        public void GenerateReport()
        {
            DataSet ds1 = Get_PreBookingsToProcess();

            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    printAddressLabel(ds1.Tables[0].Rows[i]["Consignment"].ToString(), ds1.Tables[0].Rows[i]["shipper"].ToString(), ds1.Tables[0].Rows[i]["service"].ToString(), ds1.Tables[0].Rows[i]["remarks"].ToString(), ds1.Tables[0].Rows[i]["pieces"].ToString(), ds1.Tables[0].Rows[i]["origin"].ToString(), ds1.Tables[0].Rows[i]["destination"].ToString(), ds1.Tables[0].Rows[i]["consignee"].ToString(), ds1.Tables[0].Rows[i]["codamount"].ToString(), ds1.Tables[0].Rows[i]["weight"].ToString(), ds1.Tables[0].Rows[i]["productdetail"].ToString(), ds1.Tables[0].Rows[i]["CustomerRef"].ToString(), ds1.Tables[0].Rows[i]["Consigneeaddress"].ToString(), ds1.Tables[0].Rows[i]["ConsigneePhoneNo"].ToString(), ds1.Tables[0].Rows[i]["Bookingdate"].ToString(), ds1.Tables[0].Rows[i]["consignee"].ToString(), ds1.Tables[0].Rows[i]["shipperAddress"].ToString(), ds1.Tables[0].Rows[i]["consignerPhoneNo"].ToString());
                    //if (i == (ds1.Tables[0].Rows.Count - 1))
                    //{
                    //    //Response.Write("<p style=\"page-break-after:always;\"></p>");
                    //}
                    //else
                    //{
                    //    //add page break here
                    //    //lt_main.Text += "<p style=\"page-break-after:always;\"></p>";
                    //}
                }
            }
            else
            {
                //add page break here
                //lt_main.Text += "<p style=\"page-break-after:always;\"></p>";
                Response.Write("<p style=\"page-break-after:always;\"><b>No data against this CN.</b></p>");
            }
        }

        public DataSet Get_PreBookingsToProcess()
        {
            string sortColumn = "consignmentNumber";
            string sortDirection = "ASC";
            string orderBy = "";
            if (sortColumn != "" && sortDirection != "")
            {
                orderBy = "ORDER BY CAST(" + sortColumn + " as varchar) " + sortDirection;
                if (sortColumn.ToUpper() == "CONSIGNMENTNUMBER")
                {
                    orderBy = "ORDER BY c.ConsignmentNumber " + sortDirection;
                }
                if (sortColumn.ToUpper() == "BOOKINGDATE")
                {
                    orderBy = "ORDER BY FORMAT(" + "c." + sortColumn + ", 'yyyy-MM-dd') " + sortDirection;
                }
            }
            if (Request.QueryString["con"] == null)
            {
                Response.Write("<p style=\"page-break-after:always;\">Kindly Pass Consignment Number <br /> QueryString Format Example:GetAddressLabel_HTML.aspx?con=889977000302&UserID=Nasir.Hussain&password=*****</p>");
                Response.End();
            }
            //else if (Request.QueryString["UserID"] == null)
            //{
            //    Response.Write("<p style=\"page-break-after:always;\">Kindly Pass User ID <br /> QueryString Format Example:GetAddressLabel_HTML.aspx?con=889977000302&UserID=Nasir.Hussain&password=*****</p>");
            //    Response.End();
            //}
            //else if (Request.QueryString["password"] == null)
            //{
            //    Response.Write("<p style=\"page-break-after:always;\">Kindly Pass Password <br /> QueryString Format Example:GetAddressLabel_HTML.aspx?con=889977000302&UserID=Nasir.Hussain&password=*****</p>");
            //    Response.End();
            //}
            string consignment = Request.QueryString["con"].Trim();
            //string UserID = Request.QueryString["UserID"].Trim();
            ////string accountNumber = Request.QueryString["accountNumber"].Trim();
            //string password = Request.QueryString["password"].Trim();
            string consignmentNo = "";
            DataTable ds_locations = new DataTable();

            //DataTable dt_user = getUserDetails(UserID, password);
            //string locations = "";
            //string U_ID = "";
            //if (dt_user.Rows.Count > 0)
            //{

            //U_ID = dt_user.Rows[0]["U_ID"].ToString();
            //string roleTypeID = dt_user.Rows[0]["roleTypeID"].ToString();

            //ds_locations = Check_Login_Location(U_ID, roleTypeID);
            //if (ds_locations.Rows.Count == 0)
            //{
            //    Response.Write("<p style=\"page-break-after:always;\"><b>No Locations against this userID.</b></p>");
            //}
            //else
            //{
            //    for (int i = 0; i < ds_locations.Rows.Count; i++)
            //    {
            //        locations += "'" + ds_locations.Rows[i][0].ToString() + "',";
            //    }
            //    locations = locations.TrimEnd(',');
            //}
            // }


            if (consignment.Contains(";"))
            {
                string[] consignmentNumbers = consignment.Split(';');
                int count = consignmentNumbers.Length;
                for (int i = 0; i < count; i++)
                {
                    consignmentNo += "'" + consignmentNumbers[i].ToString() + "',";
                }
                consignment = consignmentNo.TrimEnd(',');
            }
            else
            {
                consignmentNo += "'" + consignment.Trim() + "',";
                consignment = consignmentNo.TrimEnd(',');
            }

            string sql = "  \n"
           + "SELECT c.consignmentNumber        Consignment, c.bookingDate, \n"
           + "       c.serviceTypeName          SERVICE, \n"
           + "       b.name                     Origin, \n"
           + "       b1.name                    Destination, \n"
           + "       c.consigner                Shipper, \n"
           + "       c.consignee,consignerPhoneNo, \n"
           + "       c.address                  consigneeAddress, \n"
           + "       c.pieces, \n"
           + "       c.weight, \n"
           + "       c.insuarancePercentage     insuranceValue, \n"
           + "       0 as codAmount, \n"
           + "       cd.productDescription      ProductDetail, \n"
           + "       c.remarks, \n"
           + "       cd.orderRefNo              CustomerRef, \n"
           + "       c.consigneePhoneNo, \n"
           + "       CONVERT(VARCHAR, c.createdOn, 114) BookingTime, c.consignee, c.shipperAddress \n"
           + "FROM   Consignment c \n"
           + "       INNER JOIN Branches b \n"
           + "            ON  c.orgin = b.branchCode \n"
           + "       INNER JOIN Branches b1 \n"
           + "            ON  b1.branchCode = c.destination \n"
           + "       INNER JOIN CODConsignmentDetail_new cd \n"
           + "            ON  c.consignmentNumber = cd.consignmentNumber \n"
           + "WHERE  c.orgin = b.branchCode \n"
           + "       AND c.destination = b1.branchCode \n"
           + "       AND cd.consignmentNumber = c.consignmentNumber \n"
           + "       AND ISNULL(c.isApproved, '0') = '0' \n"
           + "       AND ISNULL(cd.status, '0') <> '08' \n"
           + "       AND ISNULL(c.status, '0') <> '9' \n"
           + "       AND c.consignmentNumber IN (" + consignment + ") \n";
            //+ "       AND cuc.U_ID = '" + U_ID +"'";



            DataSet ds = new DataSet();
            try
            {
                SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }

        public void printAddressLabel(string consignment, string Shipper, string Service, string Remarks, string pieces, string Origin, string destination, string Consignee, string codamount, string Weight, string productDetail, string orderRef, string ConsigneeAddress, string consigneePhone, string bookingTime, string locationName, string locationAddress, string consignerPhoneNo)
        {


            string sqlString_ = "<p style=\"font-family:IDAutomationHC39M\">";


            string sqlString = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border: 1px solid black; height:228px\">\n" +
            "           <tr>\n" +
            "               <td rowspan=\"3\" style=\"width: 130px;\">\n" +
            "                   <img src=\"../images/mnpLogo.png\" alt=\"logo\" width=\"130\" height=\"50\" />\n" +
            "               </td>\n" +
            "               <td rowspan=\"3\" style=\"text-align: center; width: 220px;font-size:12px\">\n" +
            "                   <p style=\"font-family:IDAutomationHC39M\">*" + consignment + "*</p>\n" +
            "                   <h5>\n" +
            "                       Consignee Copy\n" +
            "                   </h5>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Booking Date</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                  <p>" + bookingTime + "</p>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Print Date & Time</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
           "                    <p>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Service</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                    <p>" + Service + "</p>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Location Name</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
           "                    <p>" + locationName + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Origin</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                    <p>" + Origin + "</p>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Destination</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                    <p>" + destination + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Shipper</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 1px;\">\n" +
            "                    <p>" + Shipper +
            "               </td>\n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Consignee</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 1px solid black;\">\n" +
            "                    <p>" + Consignee + "</p>\n" +
            "               </td>\n" +


            "           </tr>\n" +
            "           <tr> \n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Pickup Address</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 0px;\">\n" +
            "                    <p>" + locationAddress + " (" + consignerPhoneNo + ")</p>\n" + "</p>\n" +
            "               </td>\n" +
            "            <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Delivery Address</b>\n" +
            "               </td>\n" +
             "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px;height:30px;\">\n" +
            "                    <p>" + ConsigneeAddress.Substring(0, Math.Min(120, ConsigneeAddress.Length)) + "</p>\n" +
            "               </td>\n" +
            "           </tr>   \n" +

            "           <tr>\n" +

            "               <td colspan=\"3\" style=\"border-top: 0px;\">\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px ;height:30px;\">\n" +
            "                     \n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border: 1px black; padding-left: 0px;\">\n" +
            "                   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n" +
            "                       <tr>\n" +
            "                           <td style=\"width: 100px; padding-left: 5px;border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Pieces</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-right: 5px;width: 50px; border: 1px solid black; text-align: right;font-size:12px\">\n" +
            "                                <p>" + pieces + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-left: 5px;width: 120px; border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Weight</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-right: 5px;width: 55px; border: 1px solid black; text-align: right;font-size:12px\">\n" +
            "                               <p>" + Weight + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-left: 5px;border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Fragile</b>\n" +
            "                           </td>\n" +
            "                       </tr>\n" +
            "                   </table>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <p>Yes</p>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Consignee Contact</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                    <p>" + consigneePhone + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"2\">\n" +
            "                   <b>Declared Insurance Value </b>\n" +
            "               </td>\n" +
            "               <td style=\"padding-right: 5px;text-align: right; font-size:12px\">\n" +
            "                    <p>0</p>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>COD AMOUNT</b>\n" +
            "               </td>\n" +
            "               <td style=\"padding-left: 5px;text-align: left; font-size:12px\">\n" +
            "                    <p>" + codamount + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Product Detail</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"5\">\n" +
            "                    <p>" + productDetail + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Remarks</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"5\">\n" +
            "                    <b>" + Remarks + "</b>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Customer Ref. #</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"4\" style='padding-left: 5px;font-size:12px'>\n" +
            "                   <p>" + orderRef + "</p>\n" +
            "               </td>\n" +
            "               <td>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"6\" style=\"padding-left: 5px;text-align: center;font-size:12px\">\n" +
            "                   <b>Please don't accept if shipment is not intact. Before paying the COD, shipment can\n" +
            "                       not be opened.</b>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "       </table>\n" +
            "       <div style=\"width: 90%; padding-left: 70px;\">\n" +
            "           <hr />\n" +
            "       </div>\n" +
            "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border: 1px solid black; height:228px\">\n" +
            "           <tr>\n" +
            "               <td rowspan=\"3\" style=\"width: 130px;\">\n" +
            "                   <img src=\"../images/mnpLogo.png\" alt=\"logo\" width=\"130\" height=\"50\" />\n" +
            "               </td>\n" +
            "               <td rowspan=\"3\" style=\"text-align: center; width: 220px;font-size:12px\">\n" +
            "                   <p style=\"font-family:IDAutomationHC39M\">*" + consignment + "*</p>\n" +
            "                   <h5>\n" +
            "                       Accounts Copy\n" +
            "                   </h5>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Booking Date</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                  <p>" + bookingTime + "</p>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Print Date & Time</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                    <p>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Service</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                    <p>" + Service + "</p>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Location Name</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
           "                    <p>" + locationName + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Origin</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                    <p>" + Origin + "</p>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                   <b>Destination</b>\n" +
            "               </td>\n" +
            "               <td style='text-align: center;font-size:12px'>\n" +
            "                    <p>" + destination + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Shipper</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 1px;\">\n" +
            "                             <p>" + Shipper +
            "               </td>\n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Consignee</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 1px solid black;\">\n" +
            "                    <p>" + Consignee + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr> \n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Pickup Address</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 0px;\">\n" +
            "                    <p>" + locationAddress + " (" + consignerPhoneNo + ")</p>\n" + " </p>\n" +
            "               </td>\n" +
               "            <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Delivery Address</b>\n" +
            "               </td>\n" +
             "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px;height:30px;\">\n" +
            "                    <p>" + ConsigneeAddress.Substring(0, Math.Min(120, ConsigneeAddress.Length)) + "</p>\n" +
            "               </td>\n" +
            "           </tr>   \n" +

            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px;\">\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px;height:30px;\">\n" +

            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border: 1px black; padding-left: 0px;\">\n" +
            "                   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n" +
            "                       <tr>\n" +
            "                           <td style=\"width: 100px; padding-left: 5px;border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Pieces</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-right: 5px;width: 50px; border: 1px solid black; text-align: right;font-size:12px\">\n" +
            "                                <p>" + pieces + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-left: 5px;width: 120px; border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Weight</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-right: 5px;width: 55px; border: 1px solid black; text-align: right;font-size:12px\">\n" +
            "                               <p>" + Weight + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-left: 5px;border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Fragile</b>\n" +
            "                           </td>\n" +
            "                       </tr>\n" +
            "                   </table>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <p>Yes</p>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Consignee Contact</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                    <p>" + consigneePhone + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"2\">\n" +
            "                   <b>Declared Insurance Value </b>\n" +
            "               </td>\n" +
            "               <td style=\"padding-right: 5px;text-align: right; font-size:12px\">\n" +
            "                    <p>0</p>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>COD AMOUNT</b>\n" +
            "               </td>\n" +
            "               <td style=\"padding-left: 5px;text-align: left; font-size:12px\">\n" +
            "                    <p>" + codamount + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Product Detail</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"5\">\n" +
            "                    <p>" + productDetail + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Remarks</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"5\">\n" +
            "                    <b>" + Remarks + "</b>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Customer Ref. #</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"4\">\n" +
            "                   <p>" + orderRef + "</p>\n" +
            "               </td>\n" +
            "               <td>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"6\" style=\"padding-left: 5px;text-align: center;font-size:12px\">\n" +
            "                   <b>Please don't accept if shipment is not intact. Before paying the COD, shipment can\n" +
            "                       not be opened.</b>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "       </table>\n" +
            "       <div style=\"padding-left: 5px;width: 90%; padding-left: 70px;font-size:12px\">\n" +
            "           <hr />\n" +
            "       </div>\n" +
            "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border: 1px solid black; height:228px;font-size:12px\">\n" +
            "           <tr>\n" +
            "               <td rowspan=\"3\" style=\"width: 130px;\">\n" +
            "                   <img src=\"../images/mnpLogo.png\" alt=\"logo\" width=\"130\" height=\"50\" />\n" +
            "               </td>\n" +
            "               <td rowspan=\"3\" style=\"text-align: center; width: 220px;\">\n" +
            "                   <p style=\"font-family:IDAutomationHC39M\">*" + consignment + "*</p>\n" +
            "                   <h5>\n" +
            "                       Shippers Copy\n" +
            "                   </h5>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                     <b>Booking Date</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                  <p>" + bookingTime + "</p>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Print Date & Time</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                    <p>" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Service</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                    <p>" + Service + "</p>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Location Name</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
           "                    <p>" + locationName + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Origin</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                    <p>" + Origin + "</p>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Destination</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                    <p>" + destination + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Shipper</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 1px;\">\n" +
            "                     <p>" + Shipper +
            "               </td>\n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Consignee</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 1px solid black;\">\n" +
            "                    <p>" + Consignee + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr> \n" +
            "               <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Pickup Address</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" style=\"padding-left: 5px;border-bottom: 0px;\">\n" +
            "                    <p>" + locationAddress + " (" + consignerPhoneNo + ")</p>\n" + "</p>\n" +
            "               </td>\n" +
            "            <td style=\"padding-left: 5px;border-bottom: 2px solid black;\">\n" +
            "                   <b>Delivery Address</b>\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px;height:30px;\">\n" +
            "                    <p>" + ConsigneeAddress.Substring(0, Math.Min(120, ConsigneeAddress.Length)) + "</p>\n" +
            "               </td>\n" +
            "           </tr>   \n" +

            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px;\">\n" +
            "               </td>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border-top: 0px;height:30px;\">\n" +

            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"3\" style=\"padding-left: 5px;border: 1px black; padding-left: 0px;\">\n" +
            "                   <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n" +
            "                       <tr>\n" +
            "                           <td style=\"width: 100px; padding-left: 5px;border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Pieces</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-right: 5px;width: 50px; border: 1px solid black; text-align: right;font-size:12px\">\n" +
            "                                <p>" + pieces + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-left: 5px;width: 120px; border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Weight</b>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-right: 5px;width: 55px; border: 1px solid black; text-align: right;font-size:12px\">\n" +
            "                               <p>" + Weight + "</p>\n" +
            "                           </td>\n" +
            "                           <td style=\"padding-left: 5px;border: 1px solid black;font-size:12px\">\n" +
            "                               <b>Fragile</b>\n" +
            "                           </td>\n" +
            "                       </tr>\n" +
            "                   </table>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <p>Yes</p>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Consignee Contact</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                    <p>" + consigneePhone + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"2\">\n" +
            "                   <b>Declared Insurance Value </b>\n" +
            "               </td>\n" +
            "               <td style=\"padding-right: 5px;text-align: right;font-size:12px\">\n" +
            "                    <p>0</p>\n" +
            "               </td>\n" +
            "               <td colspan=\"2\" >\n" +
            "                   <b style='padding-left: 5px;font-size:12px'>COD AMOUNT</b>\n" +
            "               </td>\n" +
            "               <td style=\"padding-left: 5px;text-align: left;font-size:12px\">\n" +
            "                    <p>" + codamount + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Product Detail</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"5\">\n" +
            "                    <p>" + productDetail + "</p>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Remarks</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"5\">\n" +
            "                    <b>" + Remarks + "</b>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td style='padding-left: 5px;font-size:12px'>\n" +
            "                   <b>Customer Ref. #</b>\n" +
            "               </td>\n" +
            "               <td style='padding-left: 5px;font-size:12px' colspan=\"4\">\n" +
            "                   <p>" + orderRef + "</p>\n" +
            "               </td>\n" +
            "               <td>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "           <tr>\n" +
            "               <td colspan=\"6\" style=\"text-align: center; font-size:12px\">\n" +
            "                   <b>Please don't accept if shipment is not intact. Before paying the COD, shipment can\n" +
            "                       not be opened.</b>\n" +
            "               </td>\n" +
            "           </tr>\n" +
            "       </table>\n" +
            "       <div style=\"width: 90%; padding-left: 70px;\">\n" +
            "           <hr />\n" +
            "       </div>";

            //lt_main.Text += sqlString;
            Response.Write(sqlString);

        }

        private DataTable getUserDetails(string username, string password)
        {
            string sql = "        SELECT cun.roleTypeID, \n"
             + "                   cun.U_ID \n"
             + "            FROM   COD_Users_New cun \n"
             + "            WHERE  cun.[USER_ID] = '" + username + "' \n"
             + "                   AND cun.[PASSWORD] = '" + password + "' \n"
             + "                   AND cun.[status] = '1'";

            DataTable ds = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }

        private DataTable Check_Login_Location(string U_ID, string roleTypeID)
        {
            string super = "";
            string superBranch = "";
            if (roleTypeID != "1" && roleTypeID != "3")
            {
                super = "   AND ccl.branchCode = cuc.branchCode";
                if (roleTypeID != "2" && roleTypeID != "4")
                {
                    superBranch = "  AND ccl.locationID = cuc.locationID";
                }
                else
                {
                    superBranch = "";
                }
            }
            else
            {
                super = "";
            }

            string sql = " \n"
           + "SELECT ccl.locationID \n"
           + "FROM   COD_Users_New cun \n"
           + "       INNER JOIN COD_User_Child cuc \n"
           + "            ON  cuc.U_ID = cun.U_ID \n"
           + "       INNER JOIN COD_CustomerLocations ccl \n"
           + "            ON  cuc.accountNumber = ccl.accountNo \n"
           + "         " + super + "  \n"
           + "         " + superBranch + " \n"
           + "       INNER JOIN CreditClients cc \n"
           + "            ON  cc.id = ccl.CreditClientID \n"
            + "WHERE  cun.[U_ID] = '" + U_ID + "'";


            DataTable ds = new DataTable();
            try
            {
                SqlConnection orcl = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return ds;
        }
    }
}