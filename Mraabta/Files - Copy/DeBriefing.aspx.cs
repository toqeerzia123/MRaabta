using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class DeBriefing : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Variable clvar_v = new Variable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CN"] != null)
                {
                    txt_consignment.Text = Request.QueryString["CN"].ToString();
                    txt_consignment_TextChanged(sender, e);
                }
            }

            Errorid.Text = "";
        }




        protected void txt_consignment_TextChanged(object sender, EventArgs e)
        {
            string CN = txt_consignment.Text;
            if (CN == "")
            {
                Errorid.Text = "Kindly insert Consignment Number";
                return;
            }

            DataTable dt = getConsignmentDetail(CN);

            if (dt.Rows.Count > 0)
            {
                ///////////////Details////////////////////////////
                txt_shipperName.Text = dt.Rows[0]["consigner"].ToString();
                txt_shipperAddress.Text = dt.Rows[0]["shipperAddress"].ToString();
                txt_shipperContact.Text = dt.Rows[0]["consignerCellNo"].ToString();
                txt_ConsigneeName.Text = dt.Rows[0]["consignee"].ToString();
                string addressConsignee = dt.Rows[0]["address"].ToString().Replace("<br/>", ".");
                txt_ConsigneeAddress.Text = addressConsignee;
                txt_ConsigneeContact.Text = dt.Rows[0]["consigneePhoneNo"].ToString();


                ///////////////Runsheet////////////////////////////
                //DataTable dt_runSheet = getRunSheets(CN);
                //if (dt_runSheet.Rows.Count > 0)
                //{
                //    ddl_runsheetNumber.DataSource = dt_runSheet;
                //    ddl_runsheetNumber.DataTextField = "runsheetNumber";
                //    ddl_runsheetNumber.DataValueField = "runsheetNumber";

                //    ddl_runsheetNumber.DataBind();
                //    ddl_runsheetNumber.Items.Insert(0, "Select Runsheet");
                //}

                DataTable dt_runSheet = getRunSheets(CN);
                if (dt_runSheet.Rows.Count > 0)
                {
                    txt_runsheetNumber.Text = dt_runSheet.Rows[0]["runsheetNumber"].ToString();
                    txt_runsheetNumber.Enabled = false;
                }
                else
                {
                    txt_runsheetNumber.Enabled = true;
                }

                ///////////////Resons////////////////////////////
                DataTable dt_Resons = getResons();
                if (dt_Resons.Rows.Count > 0)
                {
                    ddl_reason.DataSource = dt_Resons;
                    ddl_reason.DataTextField = "AttributeDesc";
                    ddl_reason.DataValueField = "AttributeValue";

                    ddl_reason.DataBind();
                    ddl_reason.Items.Insert(0, "Select Reason");
                }

                ///////////////Debriefing Status////////////////////////////
                DataTable dt_status = getStatus();
                if (dt_status.Rows.Count > 0)
                {
                    ddl_status.DataSource = dt_status;
                    ddl_status.DataTextField = "VALUE";
                    ddl_status.DataValueField = "ID";

                    ddl_status.DataBind();
                    ddl_status.Items.Insert(0, "Select Status");
                }

                ///////////////De Briefing////////////////////////////
                DataTable dt_debreifing = getDeBriefing(CN);
                if (dt_debreifing.Rows.Count > 0)
                {
                    int count = dt_debreifing.Rows.Count;

                    gv_debriefing.DataSource = dt_debreifing.DefaultView;
                    gv_debriefing.DataBind();

                    txt_shipperName.Text = dt_debreifing.Rows[0]["shipperName"].ToString();
                    txt_shipperAddress.Text = dt_debreifing.Rows[0]["shipperAddress"].ToString();
                    txt_shipperContact.Text = dt_debreifing.Rows[0]["shipperContact"].ToString();
                    txt_ConsigneeName.Text = dt_debreifing.Rows[0]["consigneeName"].ToString();
                    txt_ConsigneeAddress.Text = dt_debreifing.Rows[0]["consigneeaddress"].ToString();
                    txt_ConsigneeContact.Text = dt_debreifing.Rows[0]["consigneeContact"].ToString();

                    lbl_shipperName.Text = dt_debreifing.Rows[0]["shipperName"].ToString();
                    lbl_shipperAddress.Text = dt_debreifing.Rows[0]["shipperAddress"].ToString();
                    lbl_shipperContact.Text = dt_debreifing.Rows[0]["shipperContact"].ToString();
                    lbl_consigneeName.Text = dt_debreifing.Rows[0]["consigneeName"].ToString();
                    lbl_consigneeAddress.Text = dt_debreifing.Rows[0]["consigneeaddress"].ToString();
                    lbl_consigneeContact.Text = dt_debreifing.Rows[0]["consigneeContact"].ToString();

                    lbl_bookingDate.Text = dt_debreifing.Rows[0]["bookingDate"].ToString();
                    lbl_weight.Text = dt_debreifing.Rows[0]["weight"].ToString();
                    lbl_pieces.Text = dt_debreifing.Rows[0]["pieces"].ToString();
                    lbl_origin.Text = dt_debreifing.Rows[0]["Origin"].ToString();
                    lbl_Destination.Text = dt_debreifing.Rows[0]["destination"].ToString();
                    if (dt_debreifing.Rows[0]["codamount"].ToString() != "")
                    {
                        string amount = dt_debreifing.Rows[0]["codamount"].ToString();
                        decimal moneyvalue = decimal.Parse(amount);

                        string html = String.Format("{0:N}", moneyvalue);
                        lbl_CODamount.Text = html;
                    }
                    else
                    {
                        lbl_CODamount.Text = "";
                    }

                }
                else
                {
                    gv_debriefing.DataSource = null;
                    gv_debriefing.DataBind();

                    lbl_shipperName.Text = "";
                    lbl_shipperAddress.Text = "";
                    lbl_shipperContact.Text = "";
                    lbl_consigneeName.Text = "";
                    lbl_consigneeAddress.Text = "";
                    lbl_consigneeContact.Text = "";

                    lbl_bookingDate.Text = "";
                    lbl_weight.Text = "";
                    lbl_pieces.Text = "";
                    lbl_origin.Text = "";
                    lbl_Destination.Text = "";
                    lbl_CODamount.Text = "";
                }

                /////////////////Tracking/////////////////////
                clvar_v.CNNumber = CN;

                DataSet ds1 = Get_ConsignmentTracking_Detail(clvar_v);
                if (ds1.Tables[0].Rows.Count != 0)
                {
                    gv_tracking.DataSource = ds1.Tables[0].DefaultView;
                    gv_tracking.DataBind();
                }
                else
                {
                    gv_tracking.DataSource = null;
                    gv_tracking.DataBind();
                }

                loader.Style.Add("display", "none");

            }
            else
            {
                //   Response.Write("<script>alert('Consignment Number Does not exits');</script>");
                loader.Style.Add("display", "none");
                Errorid.Text = "Consignment Number Does not exits";

                return;
            }
        }



        private DataTable getResons()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "SELECT * FROM rvdbo.Lookup l WHERE l.AttributeGroup = 'DEBREIFING' AND l.[ACTIVE] = '1'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        private DataTable getStatus()
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "SELECT * FROM Debriefing_Status s WHERE s.status = '1'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        private DataTable getDeBriefing(string CN)
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "  SELECT db.shipperName,db.shipperAddress,db.shipperContact,db.consigneeName, \n"
               + "  db.consigneeAddress,db.consigneeContact,db.runsheetNumber,db.comments,db.createdOn,z.name zoneCode,b.name branchCode,zu.U_NAME, \n"
               + "  c.bookingDate,c.weight,c.pieces,b1.name Origin,b2.name destination,cdn.codamount,ds.value,db.reason \n"
               + "    FROM Debriefing_Consignment db \n"
               + "   INNER JOIN Zones z ON z.zoneCode = db.zonecode \n"
               + "   INNER JOIN branches b ON b.branchCode = db.branchCode \n"
               + "   INNER JOIN ZNI_USER1 zu ON db.createdBy = zu.U_ID \n"
               + "   INNER JOIN consignment c ON c.consignmentNumber = db.consignmentNumber \n"
               + "   INNER JOIN branches b1 ON b1.branchCode = c.orgin \n"
               + "   INNER JOIN branches b2 ON b2.branchCode = c.destination \n"
               + "   Left JOIN Debriefing_Status ds ON ds.ID = db.status \n"
               + "   left JOIN CODConsignmentDetail_New cdn ON cdn.consignmentNumber = c.consignmentNumber \n"
               + "  WHERE db.consignmentNumber = '" + CN + "' ORDER BY db.createdOn desc";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        private DataTable getConsignmentDetail(string CN)
        {
            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "SELECT c.consigner,c.consignerCellNo,c.shipperAddress, c.consignee,c.consigneePhoneNo,c.[address] \n"
               + "  FROM Consignment c WHERE c.consignmentNumber = '" + CN + "'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }

        private DataTable getRunSheets(string CN)
        {

            DataTable Ds_1 = new DataTable();

            try
            {
                string query = "SELECT * FROM RunsheetConsignment rc  \n"
                  + "WHERE rc.createdOn IN (SELECT MAX(rc1.createdOn) FROM RunsheetConsignment rc1 where rc1.consignmentNumber = '" + CN + "')  \n"
                  + "and rc.consignmentNumber = '" + CN + "'";
                //string query = "SELECT * FROM RunsheetConsignment rc WHERE rc.consignmentNumber  = '" + CN + "' ORDER BY rc.createdOn desc";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(Ds_1);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }

            return Ds_1;
        }


        protected void btn_save_Click(object sender, EventArgs e)
        {
            string consignmentNumber = txt_consignment.Text;
            string shipper = txt_shipperName.Text;
            string shipperAddress = txt_shipperAddress.Text;
            string shipperContact = txt_shipperContact.Text;
            string ConsigneeName = txt_ConsigneeName.Text;
            string ConsigneeAddress = txt_ConsigneeAddress.Text;
            string ConsigneeContact = txt_ConsigneeContact.Text;
            //string runsheet = ddl_runsheetNumber.SelectedValue.ToString();
            string runsheet = txt_runsheetNumber.Text;
            if (runsheet == "Select Runsheet")
            {
                runsheet = "";
            }
            string reason = ddl_reason.SelectedValue.ToString();
            if (reason == "Select Reason")
            {
                reason = "";
            }
            string comment = txt_comments.Text.Replace("'", "\\");
            string status = ddl_status.SelectedValue.ToString();
            string u_ID = Session["U_ID"].ToString();
            string zone = Session["ZONECODE"].ToString();
            string branch = Session["BRANCHCODE"].ToString();

            int chk = insertDebiefing(consignmentNumber, shipper, shipperAddress, shipperContact, ConsigneeName, ConsigneeAddress, ConsigneeContact, runsheet, comment, u_ID, zone, branch, reason, status);

            if (chk == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                "alert('Debriefing Inserted successfully!'); window.location='DeBriefing.aspx?CN=" + consignmentNumber + "';", true);
                //  ScriptManager.RegisterStartupScript(this.Page, GetType(), "Debriefing Inserted successfully!", "window.location='DeBriefing.aspx';", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Debriefing Inserted successfully!');window.location='DeBriefing.aspx';</script>'");
            }
            else
            {
                Errorid.Text = "Error While inserting DeBriefing";

            }

        }

        private int insertDebiefing(string consignmentNumber, string shipper, string shipperAddress, string shipperContact, string ConsigneeName, string ConsigneeAddress, string ConsigneeContact, string runsheet, string comment, string u_ID, string zone, string branch, string reason, string status)
        {
            int i = 0;

            string query = "Insert into Debriefing_Consignment (consignmentNumber,shipperName,shipperAddress,shipperContact,consigneeName,consigneeAddress,consigneeContact,runsheetNumber,comments,createdBy,zoneCode,branchCode,createdOn,reason,status) values\n"
             + "    ('" + consignmentNumber + "','" + shipper + "','" + shipperAddress + "','" + shipperContact + "', '" + ConsigneeName + "','" + ConsigneeAddress + "',\n"
             + "  '" + ConsigneeContact + "','" + runsheet + "','" + comment + "','" + u_ID + "','" + zone + "','" + branch + "',GetDate(),'" + reason + "','" + status + "') ";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { i = 1; }
            finally { con.Close(); }

            return i;
        }


        public DataSet Get_ConsignmentTracking_Detail(Variable clvar)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = "SELECT   \n"
                   + "----- MIS Consignment Tracking REPORT  \n"
                   + " --Supportdesk@mulphico.pk \n"
                   + "       mcts.TrackingStatus,   \n"
                   + "       case when mcts.StatusID = '20' then (Select top(1) m.Name  from rvdbo.MovementRoute m where m.[MovementRouteid] = b.currentLocation and m.IsActive='1')  else b.currentLocation end currentLocation,   \n"
                   + "       b.Booked,   \n"
                   + "       b.consignmentNumber,   \n"
                   + "       b.transactionTime,   \n"
                   + "       b.Detail   \n"
                   + "FROM   (   \n"
                   + "           SELECT --mcts.TrackingStatus,    \n"
                   + "                  cth.transactionTime,   \n"
                   + "                  -- mcts.StatusID,    \n"
                   + "                  cth.consignmentNumber,   \n"
                   + "                  StateID,   \n"
                   + "                  '' Booked,   \n"
                   + "                  cth.currentLocation,   \n"
                   + "                  CASE    \n"
                   + "                       WHEN cth.StateID = '1' THEN /* ISNULL(   \n"
                   + "                                (   \n"
                   + "                                    SELECT +'Consignment No: ' + c.consignmentNumber    \n"
                   + "                                           +   \n"
                   + "                                           ' was booked on :' + CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                           +   \n"
                   + "                                           ' by User :' + zu.Name +   \n"
                   + "                                           ' on Location :'    \n"
                   + "                                           + ec.name   \n"
                   + "                                    FROM   Consignment c,   \n"
                   + "                                           ZNI_USER1 zu,   \n"
                   + "                                           Branches b,   \n"
                   + "                                           ExpressCenters ec   \n"
                   + "                                    WHERE  CONVERT(NVARCHAR, c.createdby) =    \n"
                   + "                                           CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                           AND zu.branchcode = b.branchCode   \n"
                   + "                                           AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                           AND RTRIM(LTRIM(c.consignmentNumber)) =    \n"
                   + "                                               RTRIM(LTRIM(cth.consignmentNumber))   \n"
                   + "                                ),   \n"
                   + "                                'New'   \n"
                   + "                            )  */ 'New'  \n"
                   + "                       WHEN cth.StateID = '2' THEN (   \n"
                   + "                                SELECT +'Manifest No :' + c.manifestNumber +   \n"
                   + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name   \n"
                   + "                                FROM   mnp_Manifest c,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND c.manifestNumber = cth.manifestNumber   \n"
                   + "                            )   \n"
                   + "                       WHEN cth.StateID = '3' THEN (   \n"
                   + "                                SELECT +'Bag No: ' + c.bagNumber +   \n"
                   + "                                       ' was Generated on :' +   \n"
                   + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name   \n"
                   + "                                FROM   Bag c,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND c.bagNumber = cth.bagNumber   \n"
                   + "                            )   \n"
                   + "                       WHEN cth.StateID = '4' THEN (   \n"
                   + "                                SELECT +'Loading No :' + CONVERT(VARCHAR, c.id)    \n"
                   + "                                       +   \n"
                   + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name   \n"
                   + "                                FROM   MnP_Loading c,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.loadingNumber)   \n"
                   + "                            )   \n"
                   + "                       WHEN cth.StateID = '5' THEN (   \n"
                   + "                                SELECT +'UnLoading No :' + CONVERT(VARCHAR, c.id)    \n"
                   + "                                       +   \n"
                   + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name   \n"
                   + "                                FROM   mnp_unloading c,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND CONVERT(NVARCHAR, c.id) = CONVERT(NVARCHAR, cth.unloadingnumber)   \n"
                   + "                            )   \n"
                   + "                 \n"
                   + "                                  WHEN cth.StateID = '18' THEN (   \n"
                   + "                                SELECT + 'Arrival No :' + CONVERT(NVARCHAR, c.Id)    \n"
                   + "                                       +   \n"
                   + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name   \n"
                   + "                                FROM   ArrivalScan c,   \n"
                   + "                                       ArrivalScan_Detail asd,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  c.Id = asd.ArrivalID   \n"
                   + "                                       AND CONVERT(NVARCHAR, c.createdBy) =    \n"
                   + "                                           CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND asd.consignmentNumber = cth.consignmentNumber   \n"
                   + "                                       AND c.Id = cth.ArrivalID   \n"
                   + "                            )   \n"
                   + "                       WHEN cth.StateID = '6' THEN (   \n"
                   + "                                SELECT +'DeBagging No: ' + CONVERT(NVARCHAR, c.id)   \n"
                   + "                                       + '  was Generated on :' +   \n"
                   + "                                       CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name   \n"
                   + "                                FROM   MnP_Debag c,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND c.bagNumber = cth.bagNumber   \n"
                   + "                            )   \n"
                   + "                       WHEN cth.StateID = '7' THEN (   \n"
                   + "                                SELECT +'DeManifest No: ' + c.manifestNumber +    \n"
                   + "                                       ' was Generated on :' +   \n"
                   + "                                       CONVERT(VARCHAR(11), c.DemanifestDate, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name   \n"
                   + "                                FROM   Mnp_Manifest c,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  CONVERT(NVARCHAR, c.DemanifestBy) =    \n"
                   + "                                       CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND c.manifestNumber = cth.manifestNumber   \n"
                   + "                            )   \n"
                   + "                       WHEN cth.StateID = '8' THEN (   \n"
                   + "                                SELECT +'Runsheet No :' + c.runsheetNumber +   \n"
                   + "                                       ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                                       +   \n"
                   + "                                       ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                                       + ec.name    \n"
                   + "                                       + ' against Rider :' + c.routeCode + ' -'    \n"
                   + "                                       + cth.riderName   \n"
                   + "                                FROM   Runsheet c,   \n"
                   + "                                       RunsheetConsignment rc,   \n"
                   + "                                       ZNI_USER1 zu,   \n"
                   + "                                       Branches b,   \n"
                   + "                                       ExpressCenters ec   \n"
                   + "                                WHERE  CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                                       AND zu.branchcode = b.branchCode   \n"
                   + "                                       AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                                       AND c.runsheetNumber = rc.runsheetNumber   \n"
                   + "                                       AND c.routeCode = rc.RouteCode   \n"
                   + "                                       AND c.branchCode = rc.branchcode   \n"
                   + "                                       AND c.runsheetNumber = cth.runsheetNumber   \n"
                   + "                                       AND cth.consignmentNumber = rc.consignmentNumber   \n"
                   + "                            )   \n"
                   + "                       WHEN cth.stateID = '10'   \n"
                   + "           AND LEN(cth.riderName) <> 0 THEN (   \n"
                   + "                   SELECT 'Consignment has been \"' + cth.reason    \n"
                   + "                          --      \n"
                   + "                          + ' '    \n"
                   + "                          + '\" Received By \"' + (   \n"
                   + "                              CASE    \n"
                   + "                                   WHEN rc.receivedBy IS NULL THEN 'Not Feeded'   \n"
                   + "                                   ELSE rc.receivedBy   \n"
                   + "                              END   \n"
                   + "                          ) + '\" Dated: ' + (   \n"
                   + "                              CASE    \n"
                   + "                                   WHEN cth.stateID = '10' THEN LEFT(rc.deliveryDate, 10)   \n"
                   + "                              END   \n"
                   + "                          ) + (   \n"
                   + "                              CASE    \n"
                   + "                                   WHEN rc.time IS NULL THEN ''   \n"
                   + "                                   ELSE RIGHT(rc.time, 8)   \n"
                   + "                              END   \n"
                   + "                          ) + (CASE WHEN rc.Reason IS NULL THEN '' ELSE rc.Reason END)    \n"
                   + "                          + '\" Comment: ' + rc.Comments    \n"
                   + "                   FROM   runsheetconsignment rc,   \n"
                   + "                          runsheet r1   \n"
                   + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber   \n"
                   + "                          AND cth.runsheetnumber = rc.runsheetnumber   \n"
                   + "                          AND r1.runsheetnumber = rc.runsheetnumber   \n"
                   + "                          AND r1.branchcode = rc.branchcode   \n"
                   + "                          AND r1.routecode = rc.routecode   \n"
                   + "               )    \n"
                   + "               WHEN cth.stateID = '10'   \n"
                   + "           AND LEN(cth.riderName) = 0   \n"
                   + "           AND cth.reason IN ('RETURNED', 'UNDELIVERED') THEN (   \n"
                   + "                   SELECT +' Consignment is ' + cth.reason +   \n"
                   + "                          ' .For RunsheetNumber :' + cth.runsheetNumber +   \n"
                   + "                          ' due to Following Reason :' + (   \n"
                   + "                              CASE    \n"
                   + "                                   WHEN ISNULL(rc.Reason, '0') = '0' THEN ''   \n"
                   + "                                   ELSE (   \n"
                   + "                                            SELECT v.AttributeValue   \n"
                   + "                                            FROM   rvdbo.Lookup v   \n"
                   + "                                            WHERE  v.Id = rc.Reason   \n"
                   + "                                        )   \n"
                   + "                              END   \n"
                   + "                          )    \n"
                   + "                          + '\" Comment: ' + rc.Comments    \n"
                   + "                   FROM   runsheetconsignment rc,   \n"
                   + "                          runsheet r1   \n"
                   + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber   \n"
                   + "                          AND cth.runsheetnumber = rc.runsheetnumber   \n"
                   + "                          AND r1.runsheetnumber = rc.runsheetnumber   \n"
                   + "                          AND r1.branchcode = rc.branchcode   \n"
                   + "                          AND r1.routecode = rc.routecode   \n"
                   + "               )    \n"
                   + "               WHEN cth.stateID = '10'   \n"
                   + "           AND cth.reason = 'DELIVERED'   \n"
                   + "           AND LEN(cth.riderName) = 0 THEN -- (cth.reason)    \n"
                   + "                           (    \n"
                   + "                   SELECT cth.reason + ' Comment: ' + rc.Comments     \n"
                   + "                   FROM   runsheetconsignment rc,    \n"
                   + "                          runsheet r1    \n"
                   + "                   WHERE  cth.consignmentnumber = rc.consignmentnumber    \n"
                   + "                          AND cth.runsheetnumber = rc.runsheetnumber    \n"
                   + "                          AND r1.runsheetnumber = rc.runsheetnumber    \n"
                   + "                          AND r1.branchcode = rc.branchcode    \n"
                   + "                          AND r1.routecode = rc.routecode    \n"
                   + "               )                    \n"
                   + "               ----    \n"
                   + "               WHEN cth.StateID = '20' THEN (   \n"
                   + "                   SELECT +'Loading No :' + CONVERT(NVARCHAR, c.id)    \n"
                   + "                          +   \n"
                   + "                          ' was Generated on :' + CONVERT(VARCHAR(11), c.createdOn, 106)    \n"
                   + "                          +   \n"
                   + "                          ' by User :' + zu.Name + ' on Location :'    \n"
                   + "                          + ec.name   \n"
                   + "                   FROM   mnp_loading c,   \n"
                   + "                          dbo.mnp_loadingconsignment asd,   \n"
                   + "                          ZNI_USER1          zu,   \n"
                   + "                          Branches           b,   \n"
                   + "                          ExpressCenters     ec   \n"
                   + "                   WHERE  c.id = asd.loadingId   \n"
                   + "                          AND CONVERT(NVARCHAR, c.createdBy) = CONVERT(NVARCHAR, zu.U_ID)   \n"
                   + "                          AND zu.branchcode = b.branchCode   \n"
                   + "                          AND zu.ExpressCenter = ec.expressCentercode   \n"
                   + "                          AND asd.ConsignmentNumber = cth.consignmentNumber  \n"
                   + "                          AND c.id = cth.loadingNumber  \n"
                   + "               )    \n"
                   + "               ELSE ''    \n"
                   + "               END Detail    \n"
                   + "               FROM (   \n"
                   + "                   SELECT *   \n"
                   + "                   FROM   Consignment_Tracking_View   \n"
                   + "                   WHERE  consignmentNumber = '" + clvar.CNNumber + "'   \n"
                   + "               ) cth    \n"
                   + "                  \n"
                   + "               WHERE -- mcts.[Active] = '1'    \n"
                   + "               cth.consignmentNumber = '" + clvar.CNNumber + "'    \n"
                   + "               -- AND cth.stateID = '1'    \n"
                   + "               GROUP BY    \n"
                   + "               cth.consignmentNumber,   \n"
                   + "           -- mcts.StatusID,    \n"
                   + "           cth.stateID,   \n"
                   + "           cth.transactionTime,   \n"
                   + "           cth.currentLocation,   \n"
                   + "           cth.manifestNumber,   \n"
                   + "           cth.bagNumber,   \n"
                   + "           cth.SealNo,   \n"
                   + "           cth.loadingNumber,   \n"
                   + "           cth.ArrivalID,   \n"
                   + "           cth.runsheetNumber,   \n"
                   + "           cth.riderName,   \n"
                   + "           cth.reason, cth.unloadingnumber \n"
                   + "       ) b   \n"
                   + "       RIGHT OUTER JOIN MNP_ConsginmentTrackingStatus mcts   \n"
                   + "            ON  mcts.StatusID = b.stateID   \n"
                   + "WHERE  mcts.[Active] = '1'   \n"
                   + "AND b.Detail IS NOT NULL  \n"
                   + "GROUP BY   \n"
                   + "       mcts.TrackingStatus,   \n"
                   + "       b.currentLocation,   \n"
                   + "       b.Booked,   \n"
                   + "       b.consignmentNumber,   \n"
                   + "       b.Detail,   \n"
                   + "       mcts.sortorder,   \n"
                   + "       b.transactionTime, mcts.StatusID  \n"
                   + "ORDER BY   \n"
                   + "     --  CAST(mcts.sortorder AS INT)  \n"
                   + "b.transactionTime ASC  \n"
                   + "";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 30;
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

        public DataTable getStatusCN { get; set; }
    }
}