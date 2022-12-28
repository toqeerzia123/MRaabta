using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class ShipperAdvice : System.Web.UI.Page
    {
        CL_Customer clvar = new CL_Customer();
        Cl_Variables cl_var = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        SqlConnection orcl;

        string datepicker_;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_CallTrackerStatus();
                Get_ReAttemptReason();
                btn_search_Click(sender, e);
            }
        }

        public void Get_CallTrackerStatus()
        {
            DataSet ds_advice = Get_AllCallTrackerStatus();

            if (ds_advice.Tables[0].Rows.Count != 0)
            {
                dd_advice.DataTextField = "Name";
                dd_advice.DataValueField = "id";
                dd_advice.DataSource = ds_advice.Tables[0].DefaultView;
                dd_advice.DataBind();
            }
            dd_advice.SelectedIndex = 0;  //first item
        }

        public void Get_ReAttemptReason()
        {
            DataSet ds_reattempt = Get_ReAttempt();

            if (ds_reattempt.Tables[0].Rows.Count != 0)
            {
                dd_reattempt.DataTextField = "Name";
                dd_reattempt.DataValueField = "ReAttempt_Id";
                dd_reattempt.DataSource = ds_reattempt.Tables[0].DefaultView;
                dd_reattempt.DataBind();
            }
            dd_reattempt.SelectedIndex = 0;  //first item
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            GV_Histroy.DataSource = null;
            GV_Histroy.DataBind();
            Errorid.Text = "";

            string cn = txt_cn.Text;

            if (datepicker.Text != "")
            {
                datepicker_ = DateTime.Parse(datepicker.Text).ToString("yyyy-MM-dd");
            }
            DataSet ds = Get_RequestHistory(cn, datepicker_);

            if (ds.Tables[0].Rows.Count != 0)
            {
                GV_Histroy.DataSource = ds.Tables[0].DefaultView;
                GV_Histroy.DataBind();
            }
            else
            {
                Errorid.Text = "NO RECORD FOUND...";
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Histroy.PageIndex = e.NewPageIndex;
            btn_search_Click(sender, e);
        }

        public DataSet Get_RequestHistory(string cn, string date)
        {
            DataSet dt = new DataSet();
            try
            {
                string query = "SELECT DISTINCT \n"
               + "	C.CONSIGNMENTNUMBER, CONVERT(VARCHAR(11),C.BOOKINGDATE,106) BOOKINGDATE, TICKETNO, CONVERT(VARCHAR(11),MIN(NR.CREATEDON),106) TICKETDATE, \n "
               + "B.NAME DESTINATIONBRANCH, NRS.NAME PENDINGREASON,  \n"
               + "	MNN.NAME STANDARDNOTE, CS.NAME CALLSTATUS, '' KPI, '' ADDITIONALREMARKS \n"
               + "FROM  \n"
               + "	CONSIGNMENT C \n"
               + "	INNER JOIN MNP_NCI_REQUEST NR ON NR.CONSIGNMENTNUMBER = C.CONSIGNMENTNUMBER \n"
               + "	INNER JOIN BRANCHES B ON B.BRANCHCODE = NR.DESTINATION  \n"
               + "	INNER JOIN MNP_NCI_REASONS NRS ON NRS.ID = NR.REASON \n"
               + "	INNER JOIN MNP_NCI_NOTE MNN ON MNN.NOTE_ID = NR.STANDARDNOTES \n"
               + "	INNER JOIN MNP_NCI_CALLSTATUS CS ON CS.ID = NR.CALLSTATUS \n"
               + "WHERE  \n"
               + "	NR.ACCOUNTNO = '4j24' \n";
                if (cn != "")
                {
                    query += "AND NR.CONSIGNMENTNUMBER = '" + cn + "' \n";
                }
                if (date != "" && date != null)
                {
                    query += "AND cast(NR.CREATEDON as date) = '" + date + "' \n";
                }
                query += "GROUP BY \n"
               + "	C.CONSIGNMENTNUMBER, C.BOOKINGDATE,B.NAME, NRS.NAME,MNN.NAME, CS.NAME,TICKETNO";

                orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return dt;
        }

        public DataSet Get_AllCallTrackerStatus()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "select * from MNP_NCI_CallTrack m WHERE m.status ='1' ORDER BY m.id";

                orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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

        public DataSet Get_ReAttempt()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT * FROM MNP_NCI_ReAttempt m WHERE m.status ='1' and CallTrack_Id = '3' ORDER BY m.name asc ";

                orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
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

        protected void dd_advice_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        #region AJAX

        public class ConsignmentClass
        {
            public string CN { get; set; }
            public string RequestDate { get; set; }
            public string BookingDate { get; set; }
            public string RequestID { get; set; }
            public string Destination { get; set; }
            public string Service { get; set; }
            public string ConsigneeName { get; set; }
            public string ConsigneeNo { get; set; }
            public string ConsigneeAddress { get; set; }
            public string ReasonPending { get; set; }
            public string StandardNotes { get; set; }
            public string CallingStatus { get; set; }
            public string CNStatus { get; set; }
            public string Remark { get; set; }
            public string Advice { get; set; }
            public string ShipperName { get; set; }
            public string AccountNo { get; set; }
            public string Origin { get; set; }
            public string DestinationCode { get; set; }
            public string Reason { get; set; }
            public string StandardNotesCode { get; set; }
            public string CallStatus { get; set; }
            public string ReAttempt { get; set; }

        }

        public class userDetails
        {
            public string firstName;
            public string lastName;
            public string location;
        }

        [WebMethod]
        public static List<ConsignmentClass> GetConsignmentDetails(string consignment)
        {
            List<ConsignmentClass> resp = new List<ConsignmentClass>();

            Cl_Variables clvar = new Cl_Variables();

            DataTable dt = new DataTable();
            string sqlString = "SELECT TOP 1 \n"
               + "C.CREATEDON, \n"
               + "(SELECT MIN(V.CREATEDON) FROM MNP_NCI_REQUEST V WHERE V.CONSIGNMENTNUMBER = '" + consignment + "' ) REQUESTDATE, \n"
               + "C.CONSIGNMENTNUMBER, C.TICKETNO, B.NAME DESTINATION, C.CONSIGNEE, C.CONSIGNEECELL, C.CONSIGNEEADDRESS, R.NAME REASON, \n"
               + "N.NAME NOTE, CS.NAME CALLINGSTATUS, C.COMMENT, c.ShipperName, c.AccountNo, c.Origin, c.Destination DestinationCode, \n"
               + "c.Reason, c.StandardNotes StandardNotesCode,c.CallStatus \n"
               + "FROM MNP_NCI_REQUEST C  \n"
               + "INNER JOIN BRANCHES B ON C.DESTINATION = B.BRANCHCODE \n"
               + "INNER JOIN MNP_NCI_REASONS R ON R.ID = C.REASON \n"
               + "INNER JOIN MNP_NCI_NOTE N ON N.NOTE_ID = C.STANDARDNOTES \n"
               + "INNER JOIN MNP_NCI_CALLSTATUS CS ON CS.ID = C.CALLSTATUS \n"
               + "WHERE C.CONSIGNMENTNUMBER = '" + consignment + "'  \n"
               + "GROUP BY \n"
               + "C.CONSIGNMENTNUMBER, C.TICKETNO, B.NAME, C.CONSIGNEE, C.CONSIGNEECELL, C.CONSIGNEEADDRESS, R.NAME, \n"
               + "N.NAME, CS.NAME, C.COMMENT, C.CREATEDON, c.ShipperName, c.AccountNo, c.Origin, c.Destination, c.Reason, c.StandardNotes,c.CallStatus \n"
               + "ORDER BY C.CREATEDON DESC \n";

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataReader rdr = orcd.ExecuteReader();

                while (rdr.Read())
                {
                    ConsignmentClass cnrecord = new ConsignmentClass();
                    cnrecord.CN = rdr["consignmentNumber"].ToString();
                    cnrecord.RequestID = rdr["TICKETNO"].ToString();
                    cnrecord.Destination = rdr["DESTINATION"].ToString();
                    cnrecord.ConsigneeName = rdr["CONSIGNEE"].ToString();
                    cnrecord.ConsigneeNo = rdr["CONSIGNEECELL"].ToString();
                    cnrecord.ConsigneeAddress = rdr["CONSIGNEEADDRESS"].ToString();
                    cnrecord.ReasonPending = rdr["REASON"].ToString();
                    cnrecord.StandardNotes = rdr["NOTE"].ToString();
                    cnrecord.CallingStatus = rdr["CALLINGSTATUS"].ToString();
                    cnrecord.Remark = rdr["COMMENT"].ToString();
                    cnrecord.ShipperName = rdr["ShipperName"].ToString();
                    cnrecord.AccountNo = rdr["AccountNo"].ToString();
                    cnrecord.Origin = rdr["Origin"].ToString();
                    cnrecord.DestinationCode = rdr["DestinationCode"].ToString();
                    cnrecord.Reason = rdr["Reason"].ToString();
                    cnrecord.StandardNotesCode = rdr["StandardNotesCode"].ToString();
                    cnrecord.CallStatus = rdr["CallStatus"].ToString();

                    resp.Add(cnrecord);
                }
            }
            catch (Exception ex)
            {

            }

            return resp;
        }

        public class CheckingModel
        {
            public string RequestID { get; set; }
            public string Advice { get; set; }
            public string ReAttempt { get; set; }
            public string ConsigneeName { get; set; }
            public string ConsigneeNo { get; set; }
            public string ConsigneeAddress { get; set; }
            public string Remark { get; set; }
            public string CN { get; set; }
        }

        [WebMethod]
        public static void SaveToDataBase(CheckingModel data_)
        {
            List<ConsignmentClass> resp = new List<ConsignmentClass>();

            Cl_Variables clvar = new Cl_Variables();

            DataSet ds = new DataSet();

            string sqlString = "SELECT TOP 1 * FROM MNP_NCI_Request m WHERE m.TicketNo = '" + data_.RequestID + "' ORDER BY m.CreatedOn DESC";

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandTimeout = 3000;
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
            }
            catch (Exception ex)
            {

            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                string ShipperName = ds.Tables[0].Rows[0]["ShipperName"].ToString();
                string AccountNo = ds.Tables[0].Rows[0]["AccountNo"].ToString();
                string Orign = ds.Tables[0].Rows[0]["Origin"].ToString();
                string DestinationCode = ds.Tables[0].Rows[0]["Destination"].ToString();
                string Reason = ds.Tables[0].Rows[0]["Reason"].ToString();
                string StandardNotesCode = ds.Tables[0].Rows[0]["StandardNotes"].ToString();
                string CallStatus = ds.Tables[0].Rows[0]["CallStatus"].ToString();
                string ISCOD = ds.Tables[0].Rows[0]["ISCOD"].ToString();
                string UserId = HttpContext.Current.Session["U_ID"].ToString();


                string temp = "";

                string sql = "INSERT INTO MNP_NCI_Request \n" +
            "           (\n" +
            "           TicketNo,ConsignmentNumber \n" +
            "           ,ShipperName \n" +
            "           ,AccountNo \n" +
            "           ,Consignee \n" +
            "           ,ConsigneeCell \n" +
            "           ,ConsigneeAddress \n" +
            "           ,Origin \n" +
            "           ,Destination \n" +
            "           ,Reason \n" +
            "           ,StandardNotes \n" +
            "           ,CallStatus \n" +
            "           ,CallTrack \n" +
            "           ,Comment \n" +
            "           ,ISCOD \n" +
            "           ,CreatedOn \n" +
            "           ,PortalCreatedBy \n" +
            "           ,ReAttempt \n" +
            "           ) \n" +
            "           VALUES ( \n" +
            "           '" + data_.RequestID + "', \n" +
            "           '" + data_.CN + "', \n" +
            "           '" + ShipperName + "', \n" +
            "           '" + AccountNo + "', \n" +
            "           '" + data_.ConsigneeName + "', \n" +
            "           '" + data_.ConsigneeNo + "', \n" +
            "           '" + data_.ConsigneeAddress + "', \n" +
            "           '" + Orign + "', \n" +
            "           '" + DestinationCode + "', \n" +
            "           '" + Reason + "', \n" +
            "           '" + StandardNotesCode + "', \n" +
            "           '" + CallStatus + "', \n" +
            "           '" + data_.Advice + "', \n" +
            "           '" + data_.Remark + "', \n" +
            "           '" + ISCOD + "', \n" +
            "           GETDATE(), \n" +
            "           '" + UserId + "', \n" +
            "           '" + data_.ReAttempt + "' \n" +
            "           ) \n";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.ExecuteNonQuery();
                temp = "Succes";
            }

            //return resp;
        }


        #endregion


    }
}