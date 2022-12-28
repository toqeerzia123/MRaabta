using System;
using System.Web;
using System.Net;
using System.Data;
using System.Configuration;
using System.Xml.Linq;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.IO;

namespace MRaabta.Files
{
    public partial class ManageConsignmentTracking : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        Function b_fun = new Function();

        string bagnumber, totalweight, orign, destination, sealno, createon, manifestcount, cncount;
        DataSet dsXML;
        protected void Page_Load(object sender, EventArgs e)
        {
            detail_div.Visible = false;
        }

        protected void Btn_Load_Click(object sender, EventArgs e)
        {
            dsXML = new DataSet();
            clvar._CNNumber = txt_consignmentno.Text.Trim();
            DataSet ds = Is_ConsignmentInternational(clvar);
            DataSet ds_detail = Get_ConsignmentTrackingHistory__QA(clvar);
            DataSet ds_Master = Get_ConsignmentTrackingHistory_Detail(clvar);
            //DataSet Loadinginfo = b_fun.Get_loadinginfo(clvar);


            if (dsXML.Tables.Count > 0)
            {
                ds_Master.Tables[0].Rows[0]["consignee"] = dsXML.Tables["HAWBDetails"].Rows[0]["ConsigneeName"];
                ds_Master.Tables[0].Rows[0]["weight"] = dsXML.Tables["HAWBDetails"].Rows[0]["Weight"];
                ds_Master.Tables[0].Rows[0]["ReceivedBy"] = dsXML.Tables["HAWBDetails"].Rows[0]["DeliveredTo"];
                ds_Master.Tables[0].Rows[0]["CurrentStatus"] = dsXML.Tables["HAWBUpdate"].Rows[0]["CustomerDescription"];
                ds_Master.Tables[0].Rows[0]["DeliveryTime"] = dsXML.Tables["HAWBDetails"].Rows[0]["DeliveredDate"];
                ds_Master.Tables[0].Rows[0]["Destination"] = dsXML.Tables["HAWBDetails"].Rows[0]["DestinationCity"] + ", " + dsXML.Tables["HAWBDetails"].Rows[0]["DestinationCountry"];
                ds_Master.Tables[0].Rows[0]["delievryRider"] = dsXML.Tables["HAWBUpdate"].Rows[0]["Comments2"];
                ds_Master.Tables[0].Rows[0]["Address"] = dsXML.Tables["HAWBDetails"].Rows[0]["ConsigneeAddress"];
                ds_Master.Tables[0].Rows[0]["ConsigneeCellNo"] = dsXML.Tables["HAWBDetails"].Rows[0]["ConsigneeNumber"];

                foreach (DataRow dr in dsXML.Tables["HAWBUpdate"].Rows)
                {
                    string[] arr = new string[6];
                    arr[0] = dr["TrackingConditionDescription"].ToString();
                    arr[1] = dr["UpdateLocationFormatted"].ToString();
                    arr[2] = "";
                    arr[3] = ds_detail.Tables[0].Rows[0]["consignmentNumber"].ToString();
                    arr[4] = dr["EntryDate"].ToString();
                    arr[5] = dr["CustomerDescription"].ToString();

                    ds_detail.Tables[0].Rows.Add(arr);
                }

            }

            gridview.Visible = true;

            if (ds_detail.Tables[0].Rows.Count != 0)
            {
                DataView dv = ds_detail.Tables[0].DefaultView;
                dv.Sort = "transactionTime desc";
                DataTable sortedDT = dv.ToTable();

                gridview.Visible = true;
                gridview.DataSource = sortedDT;
                gridview.DataBind();
            }
            else
            {
                // error_msg.Text = "No Record Found...";
                gridview.Visible = false;
            }

            if (ds_Master.Tables[0].Rows.Count != 0)
            {
                detail_div.Visible = true;
                // TrackingHistory_Detail.Visible = true;

                lbl_datetime.Text = "PROCESSING DATE TIME: " + DateTime.Now.ToString("dd-MM-yyyy h:mm:ss tt");
                lbl_con_num.Text = ds_Master.Tables[0].Rows[0]["consignmentNumber"].ToString();
                bdate.Text = ds_Master.Tables[0].Rows[0]["bookingDate"].ToString();
                lbl_orign.Text = ds_Master.Tables[0].Rows[0]["orign"].ToString();
                lbl_weight.Text = ds_Master.Tables[0].Rows[0]["weight"].ToString() + " KG";
                lbl_delivery_time.Text = ds_Master.Tables[0].Rows[0]["DeliveryTime"].ToString();
                lbl_delivery_rider.Text = ds_Master.Tables[0].Rows[0]["delievryRider"].ToString();
                lbl_serive_type.Text = ds_Master.Tables[0].Rows[0]["serviceTypeName"].ToString();
                lbl_destination.Text = ds_Master.Tables[0].Rows[0]["Destination"].ToString();
                lbl_shipper.Text = ds_Master.Tables[0].Rows[0]["consigner"].ToString();
                lbl_consignee.Text = ds_Master.Tables[0].Rows[0]["consignee"].ToString();
                lbl_received.Text = ds_Master.Tables[0].Rows[0]["ReceivedBy"].ToString();
                lbl_comment.Text = "";
                lbl_status.Text = ds_Master.Tables[0].Rows[0]["CurrentStatus"].ToString();
                lbl_account.Text = ds_Master.Tables[0].Rows[0]["AccoutNo"].ToString();
                lbl_address.Text = ds_Master.Tables[0].Rows[0]["Address"].ToString();
                lbl_rrno.Text = ds_Master.Tables[0].Rows[0]["RRNumber"].ToString();
                lbl_special.Text = ds_Master.Tables[0].Rows[0]["remarks"].ToString();
                lbl_consigneephone.Text = ds_Master.Tables[0].Rows[0]["consigneePhoneNo"].ToString();
                lbl_ReceivingDate.Text = ds_Master.Tables[0].Rows[0]["accountReceivingDate"].ToString();
                lbl_codamount.Text = ds_Master.Tables[0].Rows[0]["codamount"].ToString();
                lbl_rrdate.Text = ds_Master.Tables[0].Rows[0]["RRDate"].ToString();
                lbl_rruser.Text = ds_Master.Tables[0].Rows[0]["RRUser"].ToString();

                lbl_piece.Text = ds_Master.Tables[0].Rows[0]["pieces"].ToString();
                link.Text = "<a href='DeBriefing_Tracking.aspx?cn=" + ds_Master.Tables[0].Rows[0]["consignmentNumber"].ToString() + "&type=debriefing' target='_blank'>Link</a>";

                lbl_nic.Text = ds_Master.Tables[0].Rows[0]["Receiver_CNIC"].ToString();
                lbl_relation.Text = "";// ds_Master.Tables[0].Rows[0]["Relation"].ToString();
                lbl_IntCountryCode.Text = ds_Master.Tables[0].Rows[0]["destinationCountryCode"].ToString();
                lbl_void.Text = ds_Master.Tables[0].Rows[0]["void"].ToString();
                lbl_dimension.Text = "<a href='DeBriefing_Tracking.aspx?cn=" + ds_Master.Tables[0].Rows[0]["consignmentNumber"].ToString() + "&type=dimension' target='_blank'>Link</a>";
            }
        }

        public DataSet Get_ConsignmentTrackingHistory_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                #region sqlString___08102020
                string sqlString___08102020 = "SELECT c.consignmentNumber,c.consigneePhoneNo,accountReceivingDate,\n" +
                "       c.pieces,c.destinationCountryCode,\n" +
                "       CASE WHEN ISNULL(c.[status],'0') = '9' OR c.consignerAccountNo = '300001' THEN 'YES' ELSE 'NO' END VOID, \n" +
                "       c.breadth,c.width,c.height ,  c.DenseWeight, \n" +
                "       c.consigner,\n" +
                "       c.consignee              consignee,\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) bookingDate,\n" +
                "       c.weight,\n" +
                "       c.serviceTypeName,\n" +
                "       r.receivedBy             ReceivedBy,\n" +
                "(SELECT p.ReceiptNo FROM PaymentVouchers p where ConsignmentNo = '" + clvar._CNNumber + "') RRNumber, \n" +
                "(SELECT convert(varchar(11),p.VoucherDate,106) VoucherDate FROM PaymentVouchers p where ConsignmentNo = '" + clvar._CNNumber + "') RRDate, \n" +
                "(SELECT zu.U_NAME FROM PaymentVouchers p INNER JOIN ZNI_USER1 zu ON p.CreatedBy = zu.U_ID where p.ConsignmentNo = '" + clvar._CNNumber + "') RRUser, \n" +
                "       CASE\n" +
                "            WHEN r.CurrentStatus IS NULL THEN s.TrackingStatus\n" +
                "            ELSE    (SELECT l.AttributeValue   \n" +
                "         FROM   rvdbo.Lookup l          \n" +
                "         WHERE  CONVERT(NVARCHAR,l.Id) =CONVERT(NVARCHAR, r.CurrentStatus))   \n" +
                "       END                      CurrentStatus,\n" +
                //"       DeliveryTime_ DeliveryTime,\n" +
                "       CASE WHEN r.CurrentStatus = '56' THEN NULL ELSE DeliveryTime_ END DeliveryTime, \n" +
                "       bb.name                  Destination,\n" +
                "       c.consignerAccountNo     AccoutNo,\n" +
                "       b.name                   orign,\n" +
                "       r.delievryRider          delievryRider, c.address Address, c.consigneePhoneNo ConsigneeCellNo, c.remarks, Receiver_CNIC\n" +
                "FROM   Consignment c\n" +
                "       INNER JOIN Branches bb\n" +
                "            ON  c.destination = bb.branchCode\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  c.orgin = b.branchCode\n" +
                "       LEFT JOIN (\n" +
                "SELECT rc.consignmentNumber, \n"
               + "       rc.receivedBy           ReceivedBy, \n"
               + "       rc.Status               CurrentStatus, \n"
               + "       rc.deliveryDate         DeliveryTime, \n"
               + "       rc.[time]               DeliveryTime_, \n"
               + "       r.firstName + ' ' + r.lastName + ' (' + r.riderCode + ')' delievryRider,  \n"
               + "       rs.createdOn, \n"
               + "       Receiver_CNIC \n"
               + "FROM   RunsheetConsignment     rc, \n"
               + "       Runsheet                rs, \n"
               + "       Riders                  r \n"
               + "WHERE   \n"
               + "       rs.runsheetNumber = rc.runsheetNumber \n"
               + "       AND RS.ridercode = R.riderCode \n"
               + "       AND RS.branchCode = R.branchId \n"
               + "              --AND R.routeCode = rs.routeCode \n"
               + "       AND rc.consignmentNumber = '" + clvar._CNNumber + "' \n"
               + "       AND rc.createdOn IN (SELECT MAX(rc.createdOn) \n"
               + "                            FROM   RunsheetConsignment rc \n"
               + "                            WHERE  rc.consignmentNumber = '" + clvar._CNNumber + "' \n )" +
                "            ) r\n" +
                "            ON  c.consignmentnumber = r.consignmentNumber\n" +
                "       LEFT JOIN (\n" +
                "                SELECT MAX(t.id) ID,\n" +
                "                       t.consignmentNumber\n" +
                "                FROM   (Select * from Consignment_Tracking_View) \n" +
                "              t\n  GROUP BY\n" +
                "                       t.consignmentNumber\n" +
                "            ) \n" +
                "            ct\n" +
                "            ON  ct.consignmentNumber = c.consignmentNumber\n" +
                "       LEFT JOIN (Select * from Consignment_Tracking_View) AS cth\n" +
                "            ON  cth.id = ct.ID\n" +
                "       LEFT JOIN MNP_ConsginmentTrackingStatus AS s\n" +
                "            ON  s.StatusID = cth.stateID\n" +
                "WHERE  c.consignmentNumber = '" + clvar._CNNumber + "' ";

                #endregion



                string sqlString = "SELECT c.consignmentNumber,consigneePhoneNo, accountReceivingDate,\n" +
                "       c.pieces,ct1.name destinationCountryCode,\n" +
                "       CASE WHEN ISNULL(c.[status],'0') = '9' OR c.consignerAccountNo = '300001' THEN 'YES' ELSE 'NO' END VOID, \n" +
                "       c.breadth,c.width,c.height ,  c.DenseWeight, \n" +
                "       c.consigner,\n" +
                "       c.consignee              consignee,\n" +
                "       CONVERT(VARCHAR(10), c.bookingDate, 105) bookingDate,\n" +
                "       c.weight,\n" +
                "       c.serviceTypeName,\n" +
                "       r.receivedBy             ReceivedBy,\n" +
                "(SELECT p.ReceiptNo FROM PaymentVouchers p where ConsignmentNo = '" + clvar._CNNumber + "') RRNumber, \n" +
                "(SELECT convert(varchar(11),p.VoucherDate,106) VoucherDate FROM PaymentVouchers p where ConsignmentNo = '" + clvar._CNNumber + "') RRDate, \n" +
                "(SELECT zu.U_NAME FROM PaymentVouchers p INNER JOIN ZNI_USER1 zu ON p.CreatedBy = zu.U_ID where p.ConsignmentNo = '" + clvar._CNNumber + "') RRUser, \n" +
                "       CASE\n" +
                "            WHEN r.CurrentStatus IS NULL THEN s.TrackingStatus\n" +
                "            ELSE    (SELECT l.AttributeValue   \n" +
                "         FROM   rvdbo.Lookup l          \n" +
                "         WHERE  CONVERT(NVARCHAR,l.Id) =CONVERT(NVARCHAR, r.CurrentStatus))   \n" +
                "       END                      CurrentStatus,\n" +
                //"       DeliveryTime_ DeliveryTime,\n" +
                "       CASE WHEN r.CurrentStatus = '56' THEN NULL ELSE DeliveryTime_ END DeliveryTime, \n" +
                "       bb.name                  Destination,\n" +
                "       c.consignerAccountNo     AccoutNo,\n" +
                "       b.name                   orign,\n" +
                "       r.delievryRider          delievryRider, c.address Address, c.consigneePhoneNo ConsigneeCellNo, c.remarks, Receiver_CNIC, \n" +
                "       (SELECT MAX(it.ReferenceNumber) FROM Internationaltrackinghistory it WHERE it.ConsignmentNumber = '" + clvar._CNNumber + "') ReferenceNumber, \n" +
                "       (SELECT ic.name FROM Internationaltrackinghistory it LEFT JOIN MNP_InternationalCourier ic ON it.Courier = ic.Id WHERE it.ConsignmentNumber = '" + clvar._CNNumber + "' ) CourierName, \n" +
                "       (SELECT ic.HyperLink FROM Internationaltrackinghistory it LEFT JOIN MNP_InternationalCourier ic ON it.Courier = ic.Id WHERE it.ConsignmentNumber = '" + clvar._CNNumber + "' )  Hyperlink, \n" +
                "       (SELECT ROUND(CDN.CODAMOUNT,0) CODAMOUNT FROM CODCONSIGNMENTDETAIL_NEW CDN WHERE CDN.CONSIGNMENTNUMBER = '" + clvar._CNNumber + "' ) CODAMOUNT \n" +
                "FROM   Consignment c\n" +
                "       left JOIN Country ct1 ON c.destinationCountryCode = ct1.Code \n" +
                "       INNER JOIN Branches bb\n" +
                "            ON  c.destination = bb.branchCode\n" +
                "       INNER JOIN Branches b\n" +
                "            ON  c.orgin = b.branchCode\n" +
                "       LEFT JOIN (\n" +
                "              SELECT rc.consignmentNumber,\n" +
                "          rc.receivedBy       ReceivedBy,\n" +
                "           rc.Status           CurrentStatus,\n" +
                "           rc.deliveryDate   DeliveryTime,  rc.[time] DeliveryTime_,\n" +
                "           r.firstName + ' ' + r.lastName + ' (' + r.riderCode + ')' delievryRider,  \n" +
                "        --   rs.runsheetNumber,\n" +
                "           rs.createdOn, Receiver_CNIC\n" +
                "    FROM   RunsheetConsignment rc /*,\n" +
                "		   Runsheet            rs,\n" +
                "          Riders              r--,\n" +
                "           --RiderRunsheet       rrs */\n" +
                "           inner JOIN Runsheet rs ON rs.runsheetNumber = rc.runsheetNumber  \n" +
                "           LEFT JOIN Riders r ON R.branchId = rs.branchCode \n" +
                "           AND rs.ridercode = r.riderCode \n" +
                "    WHERE \n" +
                "           /* R.branchId = rs.branchCode\n" +
                "           and rs.runsheetNumber = rc.runsheetNumber \n" +
                "           and*/ rc.consignmentNumber = '" + clvar._CNNumber + "' \n" +
                "           and rc.createdOn in (\n" +
                "              sELECT \n" +
                "              max(rc.createdOn)     \n" +
                "    FROM   RunsheetConsignment rc --,\n" +
                //"		   Runsheet            rs,\n" +
                //"          Riders              r\n" +
                "     WHERE \n" +
                //"            R.branchId = rs.branchCode\n" +
                //"           AND R.routeCode = rs.routeCode\n" +
                //"           and rc.branchcode = rs.branchCode\n" +
                //"           and rc.createdBy = rs.createdBy\n" +
                //"           and rs.runsheetNumber = rc.runsheetNumber\n" +
                "           rc.consignmentNumber = '" + clvar._CNNumber + "'\n" +
                "            )) r\n" +
                "            ON  c.consignmentnumber = r.consignmentNumber\n" +
                "       LEFT JOIN (\n" +
                "                SELECT MAX(t.id) ID,\n" +
                "                       t.consignmentNumber\n" +
                "                FROM   (Select * from Consignment_Tracking_View) \n" +
                "              t\n  GROUP BY\n" +
                "                       t.consignmentNumber\n" +
                "            ) \n" +
                "            ct\n" +
                "            ON  ct.consignmentNumber = c.consignmentNumber\n" +
                "       LEFT JOIN (Select * from Consignment_Tracking_View) AS cth\n" +
                "            ON  cth.id = ct.ID\n" +
                "       LEFT JOIN MNP_ConsginmentTrackingStatus AS s\n" +
                "            ON  s.StatusID = cth.stateID\n" +
                "WHERE  c.consignmentNumber = '" + clvar._CNNumber + "' ";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public DataSet Get_ConsignmentTrackingHistory__QA(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                #region sqlString___28022019
                string sqlString___28022019 = "SELECT    \n"
               + "----- MIS Consignment Tracking REPORT \n"
               + " --" + HttpContext.Current.Session["U_NAME"].ToString() + " \n"
               + "       mcts.TrackingStatus,    \n"
               + "       b.currentLocation,    \n"
               + "       b.Booked,    \n"
               + "       b.consignmentNumber,    \n"
               + "       b.transactionTime,    \n"
               + "       b.Detail    \n"
               + "FROM   (    \n"
               + "           SELECT --mcts.TrackingStatus,     \n"
               + "                  cth.transactionTime,    \n"
               + "                  -- mcts.StatusID,     \n"
               + "                  cth.consignmentNumber,    \n"
               + "                  StateID,    \n"
               + "                  '' Booked,    \n"
               + "                  cth.currentLocation,    \n"
               + "                  CASE     \n"
               + "                       WHEN cth.StateID = '1' THEN /* ISNULL(    \n"
               + "                                (    \n"
               + "                                    SELECT +'Consignment No: ' + c.consignmentNumber     \n"
               + "                                           +    \n"
               + "                                           ' was booked on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               //+ "                                           +    \n"
               //+ "                                           ' by User :' + zu.Name +    \n"
               //+ "                                           ' on Location :'     \n"
               //+ "                                           + ec.name    \n"
               + "                                    FROM   Consignment c,    \n"
               + "                                           ZNI_USER1 zu,    \n"
               + "                                           Branches b    \n"
               //+ "                                           ExpressCenters ec    \n"
               + "                                    WHERE  CONVERT(NVARCHAR, c.createdby) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                           AND zu.branchcode = b.branchCode    \n"
               //  + "                                           AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                           AND RTRIM(LTRIM(c.consignmentNumber)) = RTRIM(LTRIM(cth.consignmentNumber))    \n"
               + "                                ),    \n"
               + "                                'New'    \n"
               + "                            )  */ 'Booking'   \n"
               + "                       WHEN cth.StateID = '2' THEN (    \n"
               + "                                SELECT +'Manifest No :' + c.manifestNumber +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name \n"
               //+ "                                       + ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   mnp_Manifest c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '3' THEN (    \n"
               + "                                SELECT +'Bag No: ' + c.bagNumber +    \n"
               + "                                       ' was Generated on :' +    \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   Bag c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b   \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.bagNumber = cth.bagNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '4' THEN (    \n"
               + "                                SELECT +'Loading No :' + CONVERT(VARCHAR, c.id)     \n"
               + "                                       +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   MnP_Loading c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.loadingNumber)    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '5' THEN (    \n"
               + "                                SELECT +'UnLoading No :' + CONVERT(VARCHAR, c.id)     \n"
               + "                                       +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   mnp_unloading c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.unloadingnumber)    \n"
               + "                            )    \n"
               + "                  \n"
               + "                                  WHEN cth.StateID = '18' THEN (    \n"
               + "                                SELECT Top(1) + 'Arrival No :' + CONVERT(NVARCHAR, c.Id)     \n"
               + "                                       +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name -- + ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   ArrivalScan c,    \n"
               + "                                       ArrivalScan_Detail asd,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  c.Id = asd.ArrivalID    \n"
               + "                                       AND CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND asd.consignmentNumber = cth.consignmentNumber    \n"
               + "                                       AND c.Id = cth.ArrivalID    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '6' THEN (    \n"
               + "                                SELECT +'DeBagging No: ' + CONVERT(NVARCHAR, c.id)    \n"
               + "                                       + '  was Generated on :' +    \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   MnP_Debag c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.bagNumber = cth.bagNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '7' THEN (    \n"
               + "                                SELECT +'DeManifest No: ' + c.manifestNumber +     \n"
               + "                                       ' was Generated on :' +    \n"
               + "                                       CONVERT(VARCHAR(11), c.DemanifestDate, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   Mnp_Manifest c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.DemanifestBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '8' THEN (    \n"
               + "                                SELECT +'Runsheet No :' + c.runsheetNumber +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name     \n"
               + "                                       + case when c.ridercode IS NULL THEN '' ELSE ' against Rider :' + c.ridercode end     \n"
               + "                                       + cth.riderName    \n"
               + "                                FROM   Runsheet c,    \n"
               + "                                       RunsheetConsignment rc,   \n"
               + "                                       ZNI_USER1 zu    \n"
               //+ "                                       Branches b,    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  \n"
               + "                                       CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               //+ "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       and c.runsheetNumber = rc.runsheetNumber    \n"
               + "                                       AND c.routeCode = rc.RouteCode    \n"
               + "                                       AND c.branchCode = rc.branchcode    \n"
               + "                                       AND c.runsheetNumber = cth.runsheetNumber    \n"
               + "                                       AND cth.consignmentNumber = rc.consignmentNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.stateID = '10'    \n"
               + "           AND LEN(cth.riderName) <> 0 THEN (    \n"
               + "                   SELECT 'Consignment has been \"' + cth.reason     \n"
               + "                          --       \n"
               + "                          + ' '     \n"
               + "                          + '\" Received By \"' + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN rc.receivedBy IS NULL THEN 'Not Feeded'    \n"
               + "                                   ELSE rc.receivedBy    \n"
               + "                              END    \n"
               + "                          ) + '\" Dated: ' + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN cth.stateID = '10' THEN LEFT(rc.deliveryDate, 10)    \n"
               + "                              END    \n"
               + "                          ) + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN rc.time IS NULL THEN ''    \n"
               + "                                   ELSE RIGHT(rc.time, 8)    \n"
               + "                              END    \n"
               + "                          ) + (CASE WHEN rc.Reason IS NULL THEN '' ELSE rc.Reason END)     \n"
               + "                          + '\" Comment: ' + rc.Comments     \n"
               + "                   FROM   runsheetconsignment rc,    \n"
               + "                          runsheet r1    \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber    \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.branchcode = rc.branchcode    \n"
               + "                          AND r1.routecode = rc.routecode    \n"
               + "               )     \n"
               + "               WHEN cth.stateID = '10'    \n"
               + "           AND LEN(cth.riderName) = 0    \n"
               + "           AND cth.reason IN ('RETURNED', 'UNDELIVERED') THEN (    \n"
               + "                   SELECT +' Consignment is ' + cth.reason +    \n"
               + "                          ' .For RunsheetNumber :' + cth.runsheetNumber +    \n"
               + "                          ' due to Following Reason :' + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN ISNULL(rc.Reason, '0') = '0' THEN ''    \n"
               + "                                   ELSE (    \n"
               + "                                            SELECT v.AttributeValue    \n"
               + "                                            FROM   rvdbo.Lookup v    \n"
               + "                                            WHERE  v.Id = rc.Reason    \n"
               + "                                        )    \n"
               + "                              END    \n"
               + "                          )     \n"
               + "                          + '\" Comment: ' + rc.Comments     \n"
               + "                   FROM   runsheetconsignment rc,    \n"
               + "                          runsheet r1    \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber    \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.branchcode = rc.branchcode    \n"
               + "                          AND r1.routecode = rc.routecode    \n"
               + "               )     \n"
               + "               WHEN cth.stateID = '10'    \n"
               + "           AND cth.reason = 'DELIVERED'    \n"
               + "           AND LEN(cth.riderName) = 0 THEN -- (cth.reason)     \n"
               + "                           (     \n"
               + "                   SELECT cth.reason + ' Comment: ' + rc.Comments  +    \n"
               + "                  ' Received By ' + \n"
               + "                  CASE \n"
               + "                  WHEN rc.receivedBy IS NULL THEN 'Not Feeded'\n"
               + "                  ELSE rc.receivedBy  \n"
               + "                  END \n"
               + "                   FROM   runsheetconsignment rc,     \n"
               + "                          runsheet r1     \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber     \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber     \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber     \n"
               + "                          AND r1.branchcode = rc.branchcode     \n"
               + "                          AND r1.routecode = rc.routecode     \n"
               + "               )                     \n"
               + "               ----     \n"
               + "               WHEN cth.StateID = '20' THEN (    \n"
               + "                   SELECT +'Loading No :' + CONVERT(NVARCHAR, c.id)     \n"
               + "                          +    \n"
               + "                          ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                          +    \n"
               + "                          ' by User :' + zu.Name     \n"
               + "                   FROM   mnp_loading c,    \n"
               + "                         -- dbo.mnp_loadingconsignment asd,    \n"
               + "                          ZNI_USER1          zu,    \n"
               + "                          MNP_Locations           b    \n"
               //+ "                          ExpressCenters     ec    \n"
               + "                   WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                          AND zu.Locationid = b.id   \n"
               //+ "                          AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                          AND c.id = cth.loadingNumber   \n"
               + "                     group by  zu.Name, c.id,c.createdOn  \n"
               + "               )     \n"
               + "               ELSE ''     \n"
               + "               END Detail     \n"
               + "               FROM (    \n"
               + "                   SELECT *    \n"
               + "                   FROM   Consignment_Tracking_View    \n"
               + "                   WHERE  consignmentNumber = '" + clvar._CNNumber + "'  \n"
               + "               ) cth     \n"
               + "                   \n"
               + "               WHERE -- mcts.[Active] = '1'     \n"
               + "               cth.consignmentNumber = '" + clvar._CNNumber + "'   \n"
               + "               -- AND cth.stateID = '1'     \n"
               + "               GROUP BY     \n"
               + "               cth.consignmentNumber,    \n"
               + "           -- mcts.StatusID,     \n"
               + "           cth.stateID,    \n"
               + "           cth.transactionTime,    \n"
               + "           cth.currentLocation,    \n"
               + "           cth.manifestNumber,    \n"
               + "           cth.bagNumber,    \n"
               + "           cth.SealNo,    \n"
               + "           cth.loadingNumber,    \n"
               + "           cth.ArrivalID,    \n"
               + "           cth.runsheetNumber,    \n"
               + "           cth.riderName,    \n"
               + "           cth.reason, cth.unloadingnumber  \n"
               + "       ) b    \n"
               + "       RIGHT OUTER JOIN MNP_ConsginmentTrackingStatus mcts    \n"
               + "            ON  mcts.StatusID = b.stateID    \n"
               + "WHERE  mcts.[Active] = '1'    \n"
               + "AND b.Detail IS NOT NULL   \n"
               + "GROUP BY    \n"
               + "       mcts.TrackingStatus,    \n"
               + "       b.currentLocation,    \n"
               + "       b.Booked,    \n"
               + "       b.consignmentNumber,    \n"
               + "       b.Detail,    \n"
               + "       mcts.sortorder,    \n"
               + "       b.transactionTime, mcts.StatusID   \n"
               + "UNION ALL \n"
               + "SELECT \n"
               + "carrier TrackingStatus, \n"
               + "IT.CurrentLocation,    \n"
               + "''Booked,   \n"
               + "IT.ConsignmentNumber,    \n"
               + "IT.transactionDate transactionTime, \n"
               + "IT.Details Detail \n"
               + "FROM \n"
               + "Internationaltrackinghistory it\n"
               + "WHERE it.ConsignmentNumber = '" + clvar._CNNumber + "'   \n"
               + "ORDER BY    \n"
               + "     --  CAST(mcts.sortorder AS INT)   \n"
               + "b.transactionTime ASC \n"
               + "";
                // + "ORDER BY    \n"
                // + "     --  CAST(mcts.sortorder AS INT)   \n"
                // + "b.transactionTime ASC \n"
                // + "";

                #endregion


                #region sqlString___08102020
                string sqlString___08102020 = "SELECT    \n"
               + "----- MIS Consignment Tracking REPORT \n"
               + " --" + HttpContext.Current.Session["U_NAME"].ToString() + " \n"
               + "       mcts.TrackingStatus,    \n"
               + "       b.currentLocation,    \n"
               + "       b.Booked,    \n"
               + "       b.consignmentNumber,    \n"
               + "       b.transactionTime,    \n"
               + "       b.Detail    \n"
               + "FROM   (    \n"
               + "           SELECT --mcts.TrackingStatus,     \n"
               + "                  cth.transactionTime,    \n"
               + "                  -- mcts.StatusID,     \n"
               + "                  cth.consignmentNumber,    \n"
               + "                  StateID,    \n"
               + "                  '' Booked,    \n"
               + "                  cth.currentLocation,    \n"
               + "                  CASE     \n"
               + "                       WHEN cth.StateID = '1' THEN /* ISNULL(    \n"
               + "                                (    \n"
               + "                                    SELECT +'Consignment No: ' + c.consignmentNumber     \n"
               + "                                           +    \n"
               + "                                           ' was booked on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               //+ "                                           +    \n"
               //+ "                                           ' by User :' + zu.Name +    \n"
               //+ "                                           ' on Location :'     \n"
               //+ "                                           + ec.name    \n"
               + "                                    FROM   Consignment c,    \n"
               + "                                           ZNI_USER1 zu,    \n"
               + "                                           Branches b    \n"
               //+ "                                           ExpressCenters ec    \n"
               + "                                    WHERE  CONVERT(NVARCHAR, c.createdby) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                           AND zu.branchcode = b.branchCode    \n"
               //  + "                                           AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                           AND RTRIM(LTRIM(c.consignmentNumber)) = RTRIM(LTRIM(cth.consignmentNumber))    \n"
               + "                                ),    \n"
               + "                                'New'    \n"
               + "                            )  */ 'Booking'   \n"
               + "                       WHEN cth.StateID = '2' THEN (    \n"
               + "                                SELECT +'Manifest No :' + c.manifestNumber +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name \n"
               //+ "                                       + ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   mnp_Manifest c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '3' THEN (    \n"
               + "                                SELECT +'Bag No: ' + c.bagNumber +    \n"
               + "                                       ' was Generated on :' +    \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   Bag c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b   \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.bagNumber = cth.bagNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '4' THEN (    \n"
               + "                                SELECT +'Loading No :' + CONVERT(VARCHAR, c.id)     \n"
               + "                                       +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   MnP_Loading c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.loadingNumber)    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '5' THEN (    \n"
               + "                                SELECT +'UnLoading No :' + CONVERT(VARCHAR, c.id)     \n"
               + "                                       +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   mnp_unloading c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.unloadingnumber)    \n"
               + "                            )    \n"
               + "                  \n"
               + "                                  WHEN cth.StateID = '18' THEN (    \n"
               + "                                SELECT Top(1) + 'Arrival No :' + CONVERT(NVARCHAR, c.Id)     \n"
               + "                                       +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name -- + ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   ArrivalScan c,    \n"
               + "                                       ArrivalScan_Detail asd,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  c.Id = asd.ArrivalID    \n"
               + "                                       AND CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND asd.consignmentNumber = cth.consignmentNumber    \n"
               + "                                       AND c.Id = cth.ArrivalID    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '6' THEN (    \n"
               + "                                SELECT +'DeBagging No: ' + CONVERT(NVARCHAR, c.id)    \n"
               + "                                       + '  was Generated on :' +    \n"
               + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   MnP_Debag c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.bagNumber = cth.bagNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '7' THEN (    \n"
               + "                                SELECT +'DeManifest No: ' + c.manifestNumber +     \n"
               + "                                       ' was Generated on :' +    \n"
               + "                                       CONVERT(VARCHAR(11), c.DemanifestDate, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name    \n"
               + "                                FROM   Mnp_Manifest c,    \n"
               + "                                       ZNI_USER1 zu,    \n"
               + "                                       Branches b    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  CONVERT(NVARCHAR, c.DemanifestBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       AND c.manifestNumber = cth.manifestNumber    \n"
               + "                            )    \n"
               + "                       WHEN cth.StateID = '8' THEN (    \n"
               + "                                SELECT +'Runsheet No :' + c.runsheetNumber +    \n"
               + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                                       +    \n"
               + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
               //+ "                                       + ec.name     \n"
               + "                                       + case when c.ridercode IS NULL THEN '' ELSE ' against Rider :' + c.ridercode end     \n"
               + "                                       + cth.riderName+ ' Runsheet Type: '+l.Code    \n"
               + "                                FROM   Runsheet c,    \n"
               + "                                       RunsheetConsignment rc,   \n"
               + "                                       ZNI_USER1 zu, dbo.Lookup l    \n"
               //+ "                                       Branches b,    \n"
               //+ "                                       ExpressCenters ec    \n"
               + "                                WHERE  \n"
               + "                                       CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               //+ "                                       AND zu.branchcode = b.branchCode    \n"
               //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                                       and c.runsheetNumber = rc.runsheetNumber    \n"
               + "                                       AND c.routeCode = rc.RouteCode    \n"
               + "                                       AND c.branchCode = rc.branchcode    \n"
               + "                                       AND c.runsheetNumber = cth.runsheetNumber    \n"
               + "                                       AND cth.consignmentNumber = rc.consignmentNumber    \n"
               + "                                       AND c.runsheetType = l.Id \n"
               + "                            )    \n"
               + "                       WHEN cth.stateID = '10'    \n"
               + "           AND LEN(cth.riderName) <> 0 THEN (    \n"
               + "                   SELECT 'Consignment has been \"' + cth.reason     \n"
               + "                          --       \n"
               + "                          + ' '     \n"
               + "                          + '\" Received By \"' + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN rc.receivedBy IS NULL THEN 'Not Feeded'    \n"
               + "                                   ELSE rc.receivedBy    \n"
               + "                              END    \n"
               + "                          ) + '\" Dated: ' + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN cth.stateID = '10' THEN LEFT(rc.deliveryDate, 10)    \n"
               + "                              END    \n"
               + "                          ) + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN rc.time IS NULL THEN ''    \n"
               + "                                   ELSE RIGHT(rc.time, 8)    \n"
               + "                              END    \n"
               + "                          ) + (CASE WHEN rc.Reason IS NULL THEN '' ELSE rc.Reason END)     \n"
               + "                          + '\" Comment: ' + rc.Comments+ ' Runsheet Type: '+l.Code    \n"
               + "                   FROM   runsheetconsignment rc,    \n"
               + "                          runsheet r1, dbo.Lookup l    \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber    \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.branchcode = rc.branchcode    \n"
               + "                          AND r1.routecode = rc.routecode    \n"
               + "                          AND r1.runsheetType = l.Id \n"
               + "               )     \n"
               + "               WHEN cth.stateID = '10'    \n"
               + "           AND LEN(ISNULL(cth.riderName,'')) = 0    \n"
               + "           AND cth.reason IN ('RETURNED', 'UNDELIVERED') THEN (    \n"
               + "                   SELECT +' Consignment is ' + cth.reason +    \n"
               + "                          ' .For RunsheetNumber :' + cth.runsheetNumber +    \n"
               + "                          ' due to Following Reason :' + (    \n"
               + "                              CASE     \n"
               + "                                   WHEN ISNULL(rc.Reason, '0') = '0' THEN ''    \n"
               + "                                   ELSE (    \n"
               + "                                            SELECT v.AttributeValue    \n"
               + "                                            FROM   rvdbo.Lookup v    \n"
               + "                                            WHERE  v.Id = rc.Reason    \n"
               + "                                        )    \n"
               + "                              END    \n"
               + "                          )     \n"
               + "                          + '\" Comment: ' + ISNULL(rc.Comments,'') + ' Runsheet Type: '+l.Code    \n"
               + "                   FROM   runsheetconsignment rc,    \n"
               + "                          runsheet r1, dbo.Lookup l    \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber    \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber    \n"
               + "                          AND r1.branchcode = rc.branchcode    \n"
               + "                          AND r1.routecode = rc.routecode    \n"
               + "                          AND r1.runsheetType = l.Id \n"
               + "               )     \n"
               + "               WHEN cth.stateID = '10'    \n"
               + "           AND cth.reason = 'DELIVERED'    \n"
               + "           AND LEN(cth.riderName) = 0 THEN -- (cth.reason)     \n"
               + "                           (     \n"
               //  + "                   SELECT cth.reason + ' Comment: ' + rc.Comments  +    \n"
               + "                   SELECT 'Consignment is '+cth.reason + ' For Runsheet No: ' + r1.runsheetNumber +' Comment: ' + rc.Comments  +    \n"
               + "                  ' Received By ' + \n"
               + "                  CASE \n"
               + "                  WHEN rc.receivedBy IS NULL THEN 'Not Feeded'\n"
               + "                  ELSE rc.receivedBy  \n"
               + "                  END + ' Runsheet Type: '+l.Code    \n"
               + "                   FROM   runsheetconsignment rc,     \n"
               + "                          runsheet r1, dbo.Lookup l     \n"
               + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber     \n"
               + "                          AND cth.runsheetnumber = rc.runsheetnumber     \n"
               + "                          AND r1.runsheetnumber = rc.runsheetnumber     \n"
               + "                          AND r1.branchcode = rc.branchcode     \n"
               + "                          AND r1.routecode = rc.routecode     \n"
               + "                          AND r1.runsheetType = l.Id \n"
               + "               )                     \n"
               + "               ----     \n"
               + "               WHEN cth.StateID = '20' THEN (    \n"
               + "                   SELECT +'Loading No :' + CONVERT(NVARCHAR, c.id)     \n"
               + "                          +    \n"
               + "                          ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
               + "                          +    \n"
               + "                          ' by User :' + zu.Name     \n"
               + "                   FROM   mnp_loading c,    \n"
               + "                         -- dbo.mnp_loadingconsignment asd,    \n"
               + "                          ZNI_USER1          zu,    \n"
               + "                          MNP_Locations           b    \n"
               //+ "                          ExpressCenters     ec    \n"
               + "                   WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
               + "                          AND zu.Locationid = b.id   \n"
               //+ "                          AND zu.ExpressCenter = ec.expressCentercode    \n"
               + "                          AND c.id = cth.loadingNumber   \n"
               + "                     group by  zu.Name, c.id,c.createdOn  \n"
               + "               )     \n"
               + "               ELSE ''     \n"
               + "               END Detail     \n"
               + "               FROM (    \n"
               + "                   SELECT *    \n"
               + "                   FROM   Consignment_Tracking_View    \n"
               + "                   WHERE  consignmentNumber = '" + clvar._CNNumber + "'  \n"
               + "               ) cth     \n"
               + "                   \n"
               + "               WHERE -- mcts.[Active] = '1'     \n"
               + "               cth.consignmentNumber = '" + clvar._CNNumber + "'   \n"
               + "               -- AND cth.stateID = '1'     \n"
               + "               GROUP BY     \n"
               + "               cth.consignmentNumber,    \n"
               + "           -- mcts.StatusID,     \n"
               + "           cth.stateID,    \n"
               + "           cth.transactionTime,    \n"
               + "           cth.currentLocation,    \n"
               + "           cth.manifestNumber,    \n"
               + "           cth.bagNumber,    \n"
               + "           cth.SealNo,    \n"
               + "           cth.loadingNumber,    \n"
               + "           cth.ArrivalID,    \n"
               + "           cth.runsheetNumber,    \n"
               + "           cth.riderName,    \n"
               + "           cth.reason, cth.unloadingnumber  \n"
               + "       ) b    \n"
               + "       RIGHT OUTER JOIN MNP_ConsginmentTrackingStatus mcts    \n"
               + "            ON  mcts.StatusID = b.stateID    \n"
               + "WHERE  mcts.[Active] = '1'    \n"
               + "AND b.Detail IS NOT NULL   \n"
               + "GROUP BY    \n"
               + "       mcts.TrackingStatus,    \n"
               + "       b.currentLocation,    \n"
               + "       b.Booked,    \n"
               + "       b.consignmentNumber,    \n"
               + "       b.Detail,    \n"
               + "       mcts.sortorder,    \n"
               + "       b.transactionTime, mcts.StatusID   \n"
               + "UNION ALL \n"
               + "SELECT \n"
               + "carrier TrackingStatus, \n"
               + "IT.CurrentLocation,    \n"
               + "''Booked,   \n"
               + "IT.ConsignmentNumber,    \n"
               + "IT.transactionDate transactionTime, \n"
               + "IT.Details Detail \n"
               + "FROM \n"
               + "Internationaltrackinghistory it\n"
               + "WHERE it.ConsignmentNumber = '" + clvar._CNNumber + "'   \n"
               + "ORDER BY    \n"
               + "     --  CAST(mcts.sortorder AS INT)   \n"
               + "b.transactionTime ASC \n"
               + "";
                // + "ORDER BY    \n"
                // + "     --  CAST(mcts.sortorder AS INT)   \n"
                // + "b.transactionTime ASC \n"
                // + "";

                #endregion








                string sqlString = "SELECT    \n"
              + "----- MIS Consignment Tracking REPORT \n"
              + " --" + HttpContext.Current.Session["U_NAME"].ToString() + " \n"
              + "       mcts.TrackingStatus,    \n"
              + "       b.currentLocation,    \n"
              + "       b.Booked,    \n"
              + "       b.consignmentNumber,    \n"
              + "       b.transactionTime,    \n"
              + "       b.Detail    \n"
              + "FROM   (    \n"
              + "           SELECT --mcts.TrackingStatus,     \n"
              + "                  cth.transactionTime,    \n"
              + "                  -- mcts.StatusID,     \n"
              + "                  cth.consignmentNumber,    \n"
              + "                  StateID,    \n"
              + "                  '' Booked,    \n"
              + "                  cth.currentLocation,    \n"
              + "                  CASE     \n"
              + "                       WHEN cth.StateID = '1' THEN /* ISNULL(    \n"
              + "                                (    \n"
              + "                                    SELECT +'Consignment No: ' + c.consignmentNumber     \n"
              + "                                           +    \n"
              + "                                           ' was booked on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              //+ "                                           +    \n"
              //+ "                                           ' by User :' + zu.Name +    \n"
              //+ "                                           ' on Location :'     \n"
              //+ "                                           + ec.name    \n"
              + "                                    FROM   Consignment c,    \n"
              + "                                           ZNI_USER1 zu,    \n"
              + "                                           Branches b    \n"
              //+ "                                           ExpressCenters ec    \n"
              + "                                    WHERE  CONVERT(NVARCHAR, c.createdby) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                           AND zu.branchcode = b.branchCode    \n"
              //  + "                                           AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                           AND RTRIM(LTRIM(c.consignmentNumber)) = RTRIM(LTRIM(cth.consignmentNumber))    \n"
              + "                                ),    \n"
              + "                                'New'    \n"
              + "                            )  */ 'Booking'   \n"
              + "                       WHEN cth.StateID = '2' THEN (    \n"
              + "                                SELECT +'Manifest No :' + c.manifestNumber +    \n"
              + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name \n"
              //+ "                                       + ' on Location :'     \n"
              //+ "                                       + ec.name    \n"
              + "                                FROM   mnp_Manifest c,    \n"
              + "                                       ZNI_USER1 zu,    \n"
              + "                                       Branches b    \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       AND c.manifestNumber = cth.manifestNumber    \n"
              + "                            )    \n"
              + "                       WHEN cth.StateID = '3' THEN (    \n"
              + "                                SELECT +'Bag No: ' + c.bagNumber +    \n"
              + "                                       ' was Generated on :' +    \n"
              + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
              //+ "                                       + ec.name    \n"
              + "                                FROM   Bag c,    \n"
              + "                                       ZNI_USER1 zu,    \n"
              + "                                       Branches b   \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       AND c.bagNumber = cth.bagNumber    \n"
              + "                            )    \n"
              + "                       WHEN cth.StateID = '4' THEN (    \n"
              + "                                SELECT +'Loading No :' + CONVERT(VARCHAR, c.id)     \n"
              + "                                       +    \n"
              + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
              //+ "                                       + ec.name    \n"
              + "                                FROM   MnP_Loading c,    \n"
              + "                                       ZNI_USER1 zu,    \n"
              + "                                       Branches b    \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.loadingNumber)    \n"
              + "                            )    \n"
              + "                       WHEN cth.StateID = '5' THEN (    \n"
              + "                                SELECT +'UnLoading No :' + CONVERT(VARCHAR, c.id)     \n"
              + "                                       +    \n"
              + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
              //+ "                                       + ec.name    \n"
              + "                                FROM   mnp_unloading c,    \n"
              + "                                       ZNI_USER1 zu,    \n"
              + "                                       Branches b    \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.unloadingnumber)    \n"
              + "                            )    \n"
              + "                  \n"
              + "                                  WHEN cth.StateID = '18' THEN (    \n"
              + "                                SELECT Top(1) + 'Arrival No :' + CONVERT(NVARCHAR, c.Id)     \n"
              + "                                       +    \n"
              + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name -- + ' on Location :'     \n"
              //+ "                                       + ec.name    \n"
              + "                                FROM   ArrivalScan c,    \n"
              + "                                       ArrivalScan_Detail asd,    \n"
              + "                                       ZNI_USER1 zu,    \n"
              + "                                       Branches b    \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  c.Id = asd.ArrivalID    \n"
              + "                                       AND CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       AND asd.consignmentNumber = cth.consignmentNumber    \n"
              + "                                       AND c.Id = cth.ArrivalID    \n"
              + "                            )    \n"
              + "                       WHEN cth.StateID = '6' THEN (    \n"
              + "                                SELECT +'DeBagging No: ' + CONVERT(NVARCHAR, c.id)    \n"
              + "                                       + '  was Generated on :' +    \n"
              + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
              //+ "                                       + ec.name    \n"
              + "                                FROM   MnP_Debag c,    \n"
              + "                                       ZNI_USER1 zu,    \n"
              + "                                       Branches b    \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       AND c.bagNumber = cth.bagNumber    \n"
              + "                            )    \n"
              + "                       WHEN cth.StateID = '7' THEN (    \n"
              + "                                SELECT +'DeManifest No: ' + c.manifestNumber +     \n"
              + "                                       ' was Generated on :' +    \n"
              + "                                       CONVERT(VARCHAR(11), c.DemanifestDate, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
              //+ "                                       + ec.name    \n"
              + "                                FROM   Mnp_Manifest c,    \n"
              + "                                       ZNI_USER1 zu,    \n"
              + "                                       Branches b    \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  CONVERT(NVARCHAR, c.DemanifestBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       AND c.manifestNumber = cth.manifestNumber    \n"
              + "                            )    \n"
              + "                       WHEN cth.StateID = '8' THEN (    \n"
              + "                                SELECT +'Runsheet No :' + c.runsheetNumber +    \n"
              + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                                       +    \n"
              + "                                       ' by User :' + zu.Name --+ ' on Location :'     \n"
              //+ "                                       + ec.name     \n"
              + "                                       + case when c.ridercode IS NULL THEN '' ELSE ' against Rider :' + c.ridercode end     \n"
              + "                                       + cth.riderName+ ' Runsheet Type: '+l.Code    \n"
              + "                                FROM   Runsheet c,    \n"
              + "                                       RunsheetConsignment rc,   \n"
              + "                                       ZNI_USER1 zu, dbo.Lookup l    \n"
              //+ "                                       Branches b,    \n"
              //+ "                                       ExpressCenters ec    \n"
              + "                                WHERE  \n"
              + "                                       CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              //+ "                                       AND zu.branchcode = b.branchCode    \n"
              //+ "                                       AND zu.ExpressCenter = ec.expressCentercode    \n"
              + "                                       and c.runsheetNumber = rc.runsheetNumber    \n"
              + "                                       AND c.routeCode = rc.RouteCode    \n"
              + "                                       AND c.branchCode = rc.branchcode    \n"
              + "                                       AND c.runsheetNumber = cth.runsheetNumber    \n"
              + "                                       AND cth.consignmentNumber = rc.consignmentNumber    \n"
              + "                                       AND c.runsheetType = l.Id \n"
              + "                            )    \n"
              + "                      WHEN cth.stateID = '10' \n"
              + "				             AND LEN(cth.riderName) <> 0 THEN ( \n"
              + "                          SELECT 'Consignment has been \"' + cth.reason                     \n"
              + "                          + ' '  \n"
              + "                          + '\" Received By \"' + ( \n"
              + "                              CASE  \n"
              + "                                   WHEN rc.receivedBy IS NULL THEN 'Not Feeded' \n"
              + "                                   ELSE rc.receivedBy \n"
              + "                              END \n"
              + "                          ) + '\" Dated: ' + ( \n"
              + "                              CASE  \n"
              + "                                   WHEN cth.stateID = '10' THEN LEFT(rc.deliveryDate, 10) \n"
              + "                              END \n"
              + "                          ) + ( \n"
              + "                              CASE  \n"
              + "                                   WHEN rc.time IS NULL THEN '' \n"
              + "                                   ELSE RIGHT(rc.time, 8) \n"
              + "                              END \n"
              + "                          )  \n"
              + "                           + ( \n"
              + "                          	CASE WHEN rc.modifiedBy IS NOT NULL THEN  \n"
              + "                          (SELECT  + ' Modified on ' +CONVERT(VARCHAR(11), rc.modifiedOn, 106)  + ' by User: ' + zu.NAME   \n"
              + "                           FROM ZNI_USER1 zu WHERE zu.U_ID=rc.modifiedBy	AND rc.modifiedBy IS NOT NULL) \n"
              + "                          ELSE '' end \n"
              + "                          )  \n"
              + "                          + (CASE WHEN rc.Reason IS NULL THEN '' ELSE rc.Reason END)  \n"
              + "                          + '\" Comment: ' + rc.Comments + ' Runsheet Type: ' + l.Code \n"
              + "                   FROM   runsheetconsignment rc, \n"
              + "                          runsheet       r1, \n"
              + "                          dbo.Lookup     l \n"
              + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber \n"
              + "                          AND cth.runsheetnumber = rc.runsheetnumber \n"
              + "                          AND r1.runsheetnumber = rc.runsheetnumber \n"
              + "                          AND r1.branchcode = rc.branchcode \n"
              + "                          AND r1.routecode = rc.routecode \n"
              + "                          AND r1.runsheetType = l.Id \n"
              + "                         -- AND rc.modifiedBy = zu.U_ID \n"
              + "               )  \n"
              + "               WHEN cth.stateID = '10' \n"
              + "           AND LEN(ISNULL(cth.riderName, '')) = 0 \n"
              + "           AND cth.reason IN ('RETURNED', 'UNDELIVERED') THEN ( \n"
              + "                   SELECT +'Consignment is ' + cth.reason + \n"
              + "                          ' .For RunsheetNumber :' + cth.runsheetNumber + --' ' +zu.NAME  \n"
              + "                           + ( \n"
              + "                          	CASE WHEN rc.modifiedBy IS NOT NULL THEN  \n"
              + "                          (SELECT  + ' Modified on ' +CONVERT(VARCHAR(11), rc.modifiedOn, 106)  + ' by User: ' + zu.NAME   \n"
              + "                           FROM ZNI_USER1 zu WHERE zu.U_ID=rc.modifiedBy	AND rc.modifiedBy IS NOT NULL) \n"
              + "                          ELSE '' end \n"
              + "                          )  \n"
              + "                          +' due to Following Reason :' + ( \n"
              + "                              CASE  \n"
              + "                                   WHEN ISNULL(rc.Reason, '0') = '0' THEN '' \n"
              + "                                   ELSE ( \n"
              + "                                            SELECT v.AttributeValue \n"
              + "                                            FROM   rvdbo.Lookup v \n"
              + "                                            WHERE  v.Id = rc.Reason \n"
              + "                                        ) \n"
              + "                              END \n"
              + "                          )  \n"
              + "                          + '\" Comment: ' + ISNULL(rc.Comments, '') +  \n"
              + "                          ' Runsheet Type: ' + l.Code \n"
              + "                   FROM   runsheetconsignment rc, \n"
              + "                          runsheet       r1, \n"
              + "                          dbo.Lookup     l \n"
              + "                          --,ZNI_USER1 zu                                                  \n"
              + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber \n"
              + "                          AND cth.runsheetnumber = rc.runsheetnumber \n"
              + "                          AND r1.runsheetnumber = rc.runsheetnumber \n"
              + "                          AND r1.branchcode = rc.branchcode \n"
              + "                          AND r1.routecode = rc.routecode \n"
              + "                          AND r1.runsheetType = l.Id   \n"
              + "                         -- AND rc.modifiedBy = zu.U_ID                         \n"
              + "           )  \n"
              + "               WHEN cth.stateID = '10' \n"
              + "           AND cth.reason = 'DELIVERED' \n"
              + "           AND LEN(cth.riderName) = 0 THEN -- (cth.reason)      \n"
              + "               ( \n"
              + "                   SELECT 'Consignment is ' + cth.reason + ' For Runsheet No: '  \n"
              + "                          + r1.runsheetNumber \n"
              + "                           \n"
              + "                        \n"
              + "                          + ( \n"
              + "                          	CASE WHEN rc.modifiedBy IS NOT NULL THEN  \n"
              + "                          (SELECT  + ' Modified on ' +CONVERT(VARCHAR(11), rc.modifiedOn, 106)  + ' by User: ' + zu.NAME   \n"
              + "                           FROM ZNI_USER1 zu WHERE zu.U_ID=rc.modifiedBy	AND rc.modifiedBy IS NOT NULL) \n"
              + "                          ELSE '' end \n"
              + "                          )  \n"
              + "                          +   \n"
              + "                          ' Comment: ' + rc.Comments + \n"
              + "                          ' Received By ' + \n"
              + "                          CASE  \n"
              + "                               WHEN rc.receivedBy IS NULL THEN 'Not Feeded' \n"
              + "                               ELSE rc.receivedBy \n"
              + "                          END + ' Runsheet Type: ' + l.Code \n"
              + "                   FROM   runsheetconsignment rc, \n"
              + "                          runsheet       r1, \n"
              + "                          dbo.Lookup     l \n"
              + "                         -- ZNI_USER1 zu \n"
              + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber \n"
              + "                          AND cth.runsheetnumber = rc.runsheetnumber \n"
              + "                          AND r1.runsheetnumber = rc.runsheetnumber \n"
              + "                          AND r1.branchcode = rc.branchcode \n"
              + "                          AND r1.routecode = rc.routecode \n"
              + "                          AND r1.runsheetType = l.Id \n"
              + "                         -- AND rc.modifiedBy = zu.U_ID \n"
              + "               ) "
               + "               WHEN cth.StateID = '20' THEN (    \n"
              + "                   SELECT +'Loading No :' + CONVERT(NVARCHAR, c.id)     \n"
              + "                          +    \n"
              + "                          ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)     \n"
              + "                          +    \n"
              + "                          ' by User :' + zu.Name     \n"
              + "                   FROM   mnp_loading c,    \n"
              + "                         -- dbo.mnp_loadingconsignment asd,    \n"
              + "                          ZNI_USER1          zu,    \n"
              + "                          MNP_Locations           b    \n"
              + "                   WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)    \n"
              + "                          AND zu.Locationid = b.id   \n"

              + "                          AND c.id = cth.loadingNumber   \n"
              + "                     group by  zu.Name, c.id,c.createdOn  \n"
              + "               )     \n"
              + "               ELSE ''     \n"
              + "               END Detail     \n"
              + "               FROM (    \n"
              + "                   SELECT *    \n"
              + "                   FROM   Consignment_Tracking_View    \n"
              + "                   WHERE  consignmentNumber = '" + clvar._CNNumber + "'  \n"
              + "               ) cth     \n"
              + "                   \n"
              + "               WHERE -- mcts.[Active] = '1'     \n"
              + "               cth.consignmentNumber = '" + clvar._CNNumber + "'   \n"
              + "               -- AND cth.stateID = '1'     \n"
              + "               GROUP BY     \n"
              + "               cth.consignmentNumber,    \n"
              + "           -- mcts.StatusID,     \n"
              + "           cth.stateID,    \n"
              + "           cth.transactionTime,    \n"
              + "           cth.currentLocation,    \n"
              + "           cth.manifestNumber,    \n"
              + "           cth.bagNumber,    \n"
              + "           cth.SealNo,    \n"
              + "           cth.loadingNumber,    \n"
              + "           cth.ArrivalID,    \n"
              + "           cth.runsheetNumber,    \n"
              + "           cth.riderName,    \n"
              + "           cth.reason, cth.unloadingnumber  \n"
              + "       ) b    \n"
              + "       RIGHT OUTER JOIN MNP_ConsginmentTrackingStatus mcts    \n"
              + "            ON  mcts.StatusID = b.stateID    \n"
              + "WHERE  mcts.[Active] = '1'    \n"
              + "AND b.Detail IS NOT NULL   \n"
              + "GROUP BY    \n"
              + "       mcts.TrackingStatus,    \n"
              + "       b.currentLocation,    \n"
              + "       b.Booked,    \n"
              + "       b.consignmentNumber,    \n"
              + "       b.Detail,    \n"
              + "       mcts.sortorder,    \n"
              + "       b.transactionTime, mcts.StatusID   \n"
              + "UNION ALL \n"
              + "SELECT \n"
              + "carrier TrackingStatus, \n"
              + "IT.CurrentLocation,    \n"
              + "''Booked,   \n"
              + "IT.ConsignmentNumber,    \n"
              + "IT.transactionDate transactionTime, \n"
              + "IT.Details Detail \n"
              + "FROM \n"
              + "Internationaltrackinghistory it\n"
              + "WHERE it.ConsignmentNumber = '" + clvar._CNNumber + "'   \n"
              + "ORDER BY    \n"
              + "     --  CAST(mcts.sortorder AS INT)   \n"
              + "b.transactionTime ASC \n";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }

        public DataSet Is_ConsignmentInternational(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sqlString = "SELECT top(1) * FROM Internationaltrackinghistory WHERE ConsignmentNumber = '" + clvar._CNNumber + "' ORDER BY Createdon";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();


            }
            catch (Exception Err)
            {

            }
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["carrier"].ToString() == "ARAMEX")
            {
                string url = String.Format("https://ws.aramex.net/custtracking/api/shipments?ShipmentNumber=" + ds.Tables[0].Rows[0][2].ToString());
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    Request.ContentType = "text/xml; encoding='utf-8'; version='1.0'";

                    WebResponse response = request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    dsXML.ReadXml(stream); // THE EXCEPTION OCCURS HERE
                }
                catch (Exception ex)
                {
                }

            }

            return ds;
        }


    }
}
