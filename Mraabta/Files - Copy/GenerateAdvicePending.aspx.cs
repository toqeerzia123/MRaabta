using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRaabta.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace MRaabta.Files
{
    public partial class GenerateAdvicePending : System.Web.UI.Page
    {
        CL_Customer clvar = new CL_Customer();
        Cl_Variables cl_var = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        SqlConnection orcl;

        string consignment;
        string reattemptremarks;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_Origin_Dest();
                Get_Reasons();
                Get_CallStatus();
                Get_CallTrackerStatus();
                if (Request.QueryString["cn"] != null)
                {
                    txt_consignment_TextChanged(sender, e);

                }
            }

        }

        public void Get_Reasons()
        {
            DataSet ds_reason = Get_AllReasons();

            if (ds_reason.Tables[0].Rows.Count != 0)
            {
                dd_reason.DataTextField = "Name";
                dd_reason.DataValueField = "id";
                dd_reason.DataSource = ds_reason.Tables[0].DefaultView;
                dd_reason.DataBind();
            }
            dd_reason.Items.Insert(0, new ListItem("Select Reason", "0"));
        }

        public void Get_CallStatus()
        {
            DataSet ds_callstatus = Get_AllCallStatus();

            if (ds_callstatus.Tables[0].Rows.Count != 0)
            {
                dd_callstatus.DataTextField = "Name";
                dd_callstatus.DataValueField = "id";
                dd_callstatus.DataSource = ds_callstatus.Tables[0].DefaultView;
                dd_callstatus.DataBind();
            }
            dd_callstatus.Items.Insert(0, new ListItem("Select Call Status", "0"));
        }

        public void Get_CallTrackerStatus(string Id = null)
        {
            DataSet ds_callTrackerstatus = Get_AllCallTrackerStatus(Id);

            if (ds_callTrackerstatus.Tables[0].Rows.Count != 0)
            {
                dd_calltrack.DataTextField = "Name";
                dd_calltrack.DataValueField = "id";
                dd_calltrack.DataSource = ds_callTrackerstatus.Tables[0].DefaultView;
                dd_calltrack.SelectedIndex = -1;
                dd_calltrack.DataBind();
            }
            dd_calltrack.SelectedIndex = 0;  //first item
                                             // dd_calltrack.Items.Insert(0, new ListItem("Select Call Track", "0"));
        }

        protected void txt_consignment_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["cn"] == null)
                {
                    consignment = txt_cn.Text;
                }
                else
                {
                    consignment = Request.QueryString["cn"];
                    txt_cn.Enabled = false;
                }
                consignment = consignment.Trim();
                txt_cn.Text = consignment;
                if (consignment == "")
                {
                    Response.Redirect(Request.RawUrl);
                    return;
                }

                dd_origin.Enabled = true;
                dd_reason.Enabled = true;

                dd_origin.Enabled = true;
                dd_reason.Enabled = true;
                dd_standardnote.Enabled = true;
                ddl_Destination.Enabled = true;
                txt_consignee.Enabled = true;
                txt_consignee_cell.Enabled = true;
                txt_ConsigneeAddress.Enabled = true;
                dd_callstatus.Enabled = true;
                dd_calltrack.Enabled = true;
                txt_comment.Enabled = true;

                dd_standardnote.Enabled = true;
                ddl_Destination.Enabled = true;
                GV_Histroy.DataSource = null;
                GV_Histroy.DataBind();
                dd_calltrack.DataSource = null;
                dd_calltrack.DataBind();
                dd_calltrack.Items.Clear();
                txt_ticketalready.Value = "";
                txt_ticketno.Text = "";
                hd_origin.Value = "";
                hd_destination.Value = "";
                txt_shipper.Text = "";
                hd_shippername.Value = "";
                lbl_account.Text = "";
                txt_consignee.Text = "";
                txt_consignee_cell.Text = "";
                txt_ConsigneeAddress.Text = "";
                hd_currentstatus.Value = "";
                lbl_cn.Text = "";
                lbl_origin.Text = "";
                lbl_destination.Text = "";
                lbl_shippername.Text = "";
                lbl_account.Text = "";
                lbl_consignee.Text = "";
                lbl_consigneecell.Text = "";
                lbl_consigneeaddress.Text = "";
                lbl_bookingdate.Text = "";
                lbl_service.Text = ""; ;
                lbl_currentstatus.Text = "";

                lbl_cod.Text = "";
                btn_save.Visible = true;


                // Amir Saleem - 3 Aug 2020
                Get_CallTrackerStatus();
                //
                DataTable dt = Get_Consignment(consignment); // FOR NEW CN
                DataTable dt1 = Get_CNFromHistory(consignment); // FOR OLD CN

                
                if (dt1.Rows.Count > 0)
                {
                    div_box.Visible = true;
                    var hours = Enumerable.Range(00, 24).Select(i => i.ToString("D2"));
                    var minutes = Enumerable.Range(00, 60).Select(i => i.ToString("D2"));
                    dd_hour.DataSource = hours;
                    dd_hour.DataBind();
                    dd_min.DataSource = minutes;
                    dd_min.DataBind();
                    string[] calltime = dt1.Rows[0]["calltime"].ToString().Split(':');
                    if (calltime[0] != "")
                    {
                        dd_hour.SelectedValue = calltime[0];
                        dd_min.SelectedValue = calltime[1];
                    }
                    txt_ticketalready.Value = dt1.Rows[0]["TICKETNO"].ToString();
                    txt_ticketno.Text = dt1.Rows[0]["TICKETNO"].ToString();
                    txt_cn.Text = dt1.Rows[0]["CONSIGNMENTNUMBER"].ToString();
                    dd_origin.SelectedValue = dt1.Rows[0]["origin"].ToString();
                    ddl_Destination.SelectedValue = dt1.Rows[0]["destination"].ToString();
                    hd_origin.Value = dt.Rows[0]["orgin"].ToString();
                    hd_destination.Value = dt.Rows[0]["destination"].ToString();
                    txt_shipper.Text = dt1.Rows[0]["ShipperName"].ToString().ToUpper();
                    hd_shippername.Value = dt.Rows[0]["consigner"].ToString().ToUpper();
                    lbl_account.Text = dt1.Rows[0]["AccountNo"].ToString().ToUpper();
                    txt_consignee.Text = dt1.Rows[0]["consignee"].ToString().ToUpper();
                    txt_consignee_cell.Text = dt1.Rows[0]["consigneeCell"].ToString().ToUpper();
                    string ConsigneeAddress = dt1.Rows[0]["consigneeaddress"].ToString().ToUpper();
                    txt_ConsigneeAddress.Text = ConsigneeAddress.Replace("<BR/>", " ");
                    hd_currentstatus.Value = dt.Rows[0]["DeliveredStatus"].ToString();
                    dd_reason.SelectedValue = dt1.Rows[0]["reason"].ToString();
                    //dd_standardnote.SelectedValue = dt1.Rows[0]["StandardNotes"].ToString();
                    //dd_callstatus.SelectedValue = dt1.Rows[0]["CallStatus"].ToString();
                    if (dd_calltrack.SelectedValue == dt1.Rows[0]["CallTrack"].ToString())
                        dd_calltrack.SelectedValue = dt1.Rows[0]["CallTrack"].ToString();


                    // Amir Saleem - 3 Aug 2020
                    Get_CallTrackerStatus();
                    //
                    lbl_cn.Text = dt.Rows[0]["CONSIGNMENTNUMBER"].ToString();
                    lbl_origin.Text = dt.Rows[0]["origin"].ToString();
                    lbl_destination.Text = dt.Rows[0]["dst"].ToString();
                    lbl_shippername.Text = dt.Rows[0]["consigner"].ToString().ToUpper();
                    lbl_account.Text = dt.Rows[0]["consignerAccountNo"].ToString().ToUpper();
                    lbl_consignee.Text = dt.Rows[0]["consignee"].ToString().ToUpper();
                    lbl_consigneecell.Text = dt.Rows[0]["consigneePhoneNo"].ToString().ToUpper();
                    lbl_consigneeaddress.Text = dt.Rows[0]["address"].ToString().ToUpper();
                    lbl_bookingdate.Text = dt.Rows[0]["bookingdate"].ToString().ToUpper();
                    lbl_service.Text = dt.Rows[0]["servicetypename"].ToString().ToUpper();
                    lbl_currentstatus.Text = dt.Rows[0]["rc_status"].ToString().ToUpper();

                    if (dt1.Rows[0]["iscod"].ToString().ToUpper() == "1")
                    {
                        lbl_cod.Text = "COD CUSTOMER";
                        cod_row.Visible = false;
                    }
                    else
                    {
                        lbl_cod.Text = "";
                    }


                    dd_reason.Focus();
                    dd_origin.Enabled = false;
                    dd_reason.Enabled = false;
                    dd_standardnote.Enabled = false;
                    ddl_Destination.Enabled = false;
                    dd_hour.Enabled = true;
                    dd_min.Enabled = true;
                    if (dt1.Rows[0]["CallTrack"].ToString() == "2")
                    {
                        btn_new_request.Visible = false;
                        dd_origin.Enabled = false;
                        dd_reason.Enabled = false;
                        dd_standardnote.Enabled = false;
                        ddl_Destination.Enabled = false;
                        txt_consignee.Enabled = false;
                        txt_consignee_cell.Enabled = false;
                        txt_ConsigneeAddress.Enabled = false;
                        dd_callstatus.Enabled = false;
                        dd_calltrack.Enabled = false;
                        txt_comment.Enabled = false;
                    }
                    if (dt1.Rows[0]["CallTrack"].ToString() == "3")
                    {
                        btn_new_request.Visible = true;
                        dd_origin.Enabled = false;
                        dd_reason.Enabled = false;
                        dd_standardnote.Enabled = false;
                        ddl_Destination.Enabled = false;
                        txt_consignee.Enabled = false;
                        txt_consignee_cell.Enabled = false;
                        txt_ConsigneeAddress.Enabled = false;
                        dd_callstatus.Enabled = false;
                        dd_calltrack.Enabled = false;
                        txt_comment.Enabled = false;
                    }

                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        div_box.Visible = true;
                        var hours = Enumerable.Range(00, 24).Select(i => i.ToString("D2"));
                        var minutes = Enumerable.Range(00, 60).Select(i => i.ToString("D2"));
                        dd_hour.DataSource = hours;
                        dd_hour.DataBind();
                        dd_min.DataSource = minutes;
                        dd_min.DataBind();

                        lbl_cn.Text = dt.Rows[0]["CONSIGNMENTNUMBER"].ToString();
                        lbl_origin.Text = dt.Rows[0]["origin"].ToString();
                        hd_origin.Value = dt.Rows[0]["orgin"].ToString();
                        lbl_destination.Text = dt.Rows[0]["dst"].ToString();
                        hd_destination.Value = dt.Rows[0]["destination"].ToString();
                        lbl_shippername.Text = dt.Rows[0]["consigner"].ToString().ToUpper();
                        hd_shippername.Value = dt.Rows[0]["consigner"].ToString().ToUpper();
                        lbl_account.Text = dt.Rows[0]["consignerAccountNo"].ToString().ToUpper();
                        txt_consignee.Text = dt.Rows[0]["consignee"].ToString().ToUpper();
                        hd_currentstatus.Value = dt.Rows[0]["DeliveredStatus"].ToString();
                        // Amir Saleen - 3 Aug 2020
                        string consignee_cell = dt.Rows[0]["consigneePhoneNo"].ToString().ToUpper().Trim();
                        if (consignee_cell != "")
                        {
                            consignee_cell = consignee_cell.Replace(" ", "");
                            string checkfirsttwodigit = consignee_cell.Substring(0, 2);
                            if (checkfirsttwodigit == "92")
                            {
                                consignee_cell = "0" + consignee_cell.Substring(2);
                            }
                            else if (checkfirsttwodigit == "+9")
                            {
                                consignee_cell = "0" + consignee_cell.Substring(3);
                            }
                            consignee_cell = consignee_cell.Replace("-", "");
                            if (consignee_cell.Length == 10)
                            {
                                txt_consignee_cell.Text = "0" + consignee_cell;
                            }
                            else
                            {
                                txt_consignee_cell.Text = consignee_cell;
                            }
                        }
                        else
                        {
                            txt_consignee_cell.Text = consignee_cell;
                        }
                        lbl_consignee.Text = dt.Rows[0]["consignee"].ToString().ToUpper();
                        lbl_consigneecell.Text = dt.Rows[0]["consigneePhoneNo"].ToString().ToUpper();
                        lbl_bookingdate.Text = dt.Rows[0]["bookingdate"].ToString().ToUpper();
                        lbl_service.Text = dt.Rows[0]["servicetypename"].ToString().ToUpper();
                        lbl_currentstatus.Text = dt.Rows[0]["rc_status"].ToString().ToUpper();
                        string ConsigneeAddress = dt.Rows[0]["address"].ToString().ToUpper();
                        lbl_consigneeaddress.Text = ConsigneeAddress.Replace("<BR/>", " ");
                        ConsigneeAddress = dt.Rows[0]["Address"].ToString().ToUpper();
                        txt_ConsigneeAddress.Text = ConsigneeAddress.Replace("<BR/>", " ");
                        if (dt.Rows[0]["cod"].ToString().ToUpper() == "1")
                        {
                            lbl_cod.Text = "COD CUSTOMER";
                            cod_row.Visible = false;
                        }
                        else
                        {
                            lbl_cod.Text = "";
                        }

                        dd_origin.Enabled = false;
                        ddl_Destination.Enabled = false;
                        if (txt_ticketno.Text == "")
                        {
                            dd_reattemptremarks.Enabled = false;
                        }
                        dd_reason.Focus();
                    }
                }

                dd_hour.SelectedIndex = 0;
                dd_min.SelectedIndex = 0;
                DataSet ds_gv = Get_RequestHistory(consignment);

                if (ds_gv.Tables[0].Rows.Count != 0)
                {
                    div_grid.Visible = true;
                    GV_Histroy.DataSource = ds_gv.Tables[0].DefaultView;
                    GV_Histroy.DataBind();
                    foreach (GridViewRow row in GV_Histroy.Rows)
                    {
                        if (row.Cells[1].Text == "INITIATE BY DESTINATION")
                        {
                            GV_Histroy.Rows[row.RowIndex].Cells[1].BackColor = System.Drawing.Color.Gray;
                            GV_Histroy.Rows[row.RowIndex].Cells[1].ForeColor = System.Drawing.Color.White;
                        }

                        if (row.Cells[1].Text == "UPDATE BY DESTINATION")
                        {
                            GV_Histroy.Rows[row.RowIndex].Cells[1].BackColor = System.Drawing.Color.Gray;
                            GV_Histroy.Rows[row.RowIndex].Cells[1].ForeColor = System.Drawing.Color.White;
                        }

                        if (row.Cells[1].Text == "UPDATE BY ORIGIN")
                        {
                            GV_Histroy.Rows[row.RowIndex].Cells[1].BackColor = System.Drawing.Color.Green;
                            GV_Histroy.Rows[row.RowIndex].Cells[1].ForeColor = System.Drawing.Color.White;
                        }

                        if (row.Cells[1].Text == "UPDATE BY COD CUSTOMER")
                        {
                            GV_Histroy.Rows[row.RowIndex].Cells[1].BackColor = System.Drawing.Color.YellowGreen;
                            GV_Histroy.Rows[row.RowIndex].Cells[1].ForeColor = System.Drawing.Color.White;
                        }

                        if (row.Cells[1].Text == "CLOSE")
                        {
                            GV_Histroy.Rows[row.RowIndex].Cells[1].BackColor = System.Drawing.Color.Red;
                            GV_Histroy.Rows[row.RowIndex].Cells[1].ForeColor = System.Drawing.Color.White;
                        }
                    }
                    if (GV_Histroy.Rows[0].Cells[1].Text=="CLOSE")
                    {
                        btn_save.Visible = false;
                    }
                }
                btn_save.Enabled = true;
            }
            catch (Exception er)
            {

            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (dd_reason.SelectedValue.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Select Reason.');", true);
                return;
            }
            if (dd_standardnote.SelectedValue.ToString() == "0" || dd_standardnote.SelectedValue.ToString() == "Select Standard Notes")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Select Standard Notes .');", true);
                return;
            }
            if (dd_callstatus.SelectedValue.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Select Call Status.');", true);
                return;
            }
            if (hd_currentstatus.Value.Trim() == "123" || hd_currentstatus.Value.Trim() == "59")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('NCI Cannot be generated againt Delivered or Returned Shipment.');", true);
                return;
            }
            if (txt_comment.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Enter Comment.');", true);
                return;
            }
            string hd_ticket = txt_ticketalready.Value;
            string ticketno = txt_ticketno.Text;
            string cn = txt_cn.Text.Trim();
            string origin = hd_origin.Value;
            string dst = hd_destination.Value;
            string shippername = hd_shippername.Value;
            string account = lbl_account.Text;
            string consignee = txt_consignee.Text;
            string consigneecell = txt_consignee_cell.Text;
            string consigneeaddress = txt_ConsigneeAddress.Text;
            string reason = dd_reason.SelectedValue.ToString();
            string standardnotes = dd_standardnote.SelectedValue.ToString();
            string callstatus = dd_callstatus.SelectedValue.ToString();
            string calltrack = dd_calltrack.SelectedValue.ToString();
            string comment = txt_comment.Text;
            string iscod = "0";
            string calltime = dd_hour.SelectedValue.ToString() + ":" + dd_min.SelectedValue.ToString();
            orcl = new SqlConnection(clvar.Strcon());
            orcl.Open();
            var count = orcl.QueryFirstOrDefault<int>(@" select count(distinct r.TicketNo)  from MNP_NCI_Request r  where r.ConsignmentNumber = '" + cn + "'  ");
            if (count == 1 || count == 2)
            {
                var CreatedBy = orcl.QueryFirstOrDefault<string>(" select cast(r.CreatedBy AS VARCHAR) CreatedBy  from MNP_NCI_Request r   where r.ConsignmentNumber = '" + cn + @"' 
                                                                    and r.createdby is not null
                                                                    and r.TicketNo = (select max(TicketNo) from MNP_NCI_Request rr where rr.ConsignmentNumber = r.ConsignmentNumber) ");
                if (CreatedBy == Session["U_ID"].ToString() && calltrack == "2" && dd_calltrack.SelectedValue == "2" && dd_calltrack.Items.Count != 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('You cannot advise for return');", true);
                    return;
                }
            }
            orcl.Close();
            if (calltime == "00:00")
            {
                var src = DateTime.Now;
                string HR = "";
                string MI = "";
                if (src.Hour.ToString().Length == 1)
                {
                    HR = "0" + src.Hour.ToString();
                }
                else
                {
                    HR = src.Hour.ToString();
                }

                if (src.Minute.ToString().Length == 1)
                {
                    MI = "0" + src.Minute.ToString();
                }
                else
                {
                    MI = src.Minute.ToString();
                }

                calltime = HR + ":" + MI;
            }
            string ReasonName = dd_reason.SelectedItem.ToString();
            string CallTrackName = dd_calltrack.SelectedItem.ToString();

            if (dd_reattemptremarks.SelectedValue != "")
            {
                int n;
                bool dd_reattemptremarks_isNumeric = int.TryParse(dd_reattemptremarks.SelectedValue, out n);
                if (!dd_reattemptremarks_isNumeric)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('please select Re-Attempt Remarks.');", true);
                    return;
                }
            }
            if (dd_reattemptremarks.SelectedValue == "")
            {
                reattemptremarks = "0";
            }
            else
            {
                reattemptremarks = dd_reattemptremarks.SelectedValue.ToString();
            }

            if (lbl_cod.Text == "COD CUSTOMER")
            {
                iscod = "1";
            }
            else
            {
                iscod = "0";
            }

            string succes = insertRequest(hd_ticket, ticketno, cn, origin, dst, shippername, account, consignee, consigneecell, consigneeaddress, reason, standardnotes, callstatus, comment, calltrack, iscod, calltime, reattemptremarks, ReasonName, CallTrackName);

            if (succes == "Succes")
            {
                // Consingee
                string ConsingeeNumber = SendMobileNumName(consigneecell);

                string SMSContent = "Dear " + consignee + ", your shipment " + cn + " booked from " + shippername + " is pending for delivery due to" + ReasonName + ". Please contact our local office or UAN helpline 111-202-202 within 24 hours.";

                insertMNPSms(cn, ConsingeeNumber, SMSContent);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('New Request Generated.');", true);

                ResetAll(sender, e);
                div_box.Visible = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Not Generated.');", true);
            }
        }

        protected void btn_New_Request_Click(object sender, EventArgs e)
        {
            btn_save.Visible = true;
            btn_new_request.Visible = false;
            dd_origin.Enabled = false;
            ddl_Destination.Enabled = false;
            txt_consignee.Enabled = true;
            txt_consignee_cell.Enabled = true;
            txt_ConsigneeAddress.Enabled = true;
            dd_callstatus.Enabled = true;
            dd_calltrack.Enabled = true;
            txt_comment.Enabled = true;
            dd_reason.Enabled = true;
            dd_standardnote.Enabled = true;
            dd_hour.Enabled = true;
            dd_min.Enabled = true;

            txt_ticketno.Text = "";
            txt_ticketalready.Value = "";

            dd_callstatus.SelectedIndex = 0;
            dd_reason.SelectedIndex = 0;
            orcl = new SqlConnection(clvar.Strcon());
            try
            {
                DataTable dt_NumberOfTickets = new DataTable();
                string query = @" Select Count(Distinct(n.ticketno)) from MNP_NCI_Request n where n.ConsignmentNumber =@CN ";
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.Parameters.AddWithValue("@CN", lbl_cn.Text);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt_NumberOfTickets);

                if (dt_NumberOfTickets.Rows.Count > 0)
                {
                    if (int.Parse(dt_NumberOfTickets.Rows[0][0].ToString()) >= 2)
                    {
                        Get_CallTrackerStatus("2");
                        dd_calltrack_SelectedIndexChanged(sender, e);
                    }
                    else if (int.Parse(dt_NumberOfTickets.Rows[0][0].ToString()) == 1)
                    {
                        Get_CallTrackerStatus("1");
                        dd_calltrack_SelectedIndexChanged(sender, e);
                    }
                }
            }
            catch (Exception er)
            {

            }
            finally
            {
                orcl.Close();
            }

        }

        public void Get_Origin_Dest()
        {
            DataSet ds_branches = Get_Branches();

            dd_origin.Items.Clear();

            if (ds_branches.Tables[0].Rows.Count != 0)
            {
                dd_origin.DataTextField = "Name";
                dd_origin.DataValueField = "branchCode";
                dd_origin.DataSource = ds_branches.Tables[0].DefaultView;
                dd_origin.DataBind();
                dd_origin.Items.Insert(0, "Select");


                ddl_Destination.DataTextField = "Name";
                ddl_Destination.DataValueField = "branchCode";
                ddl_Destination.DataSource = ds_branches.Tables[0].DefaultView;
                ddl_Destination.DataBind();
                ddl_Destination.Items.Insert(0, "Select");
            }
        }

        public DataSet Get_AllReasons()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "select * from  MNP_NCI_Reasons m WHERE m.status ='1' ORDER BY m.name";

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

        public DataSet Get_AllCallStatus()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "select * from MNP_NCI_CallStatus m WHERE m.status ='1' ORDER BY m.name";

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

        public DataSet Get_AllCallTrackerStatus(string Id = null)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "";
                // Amir 3 Aug 2020 - Changes
                if (txt_ticketno.Text.Trim().ToString() != "")
                {
                    query = "select * from MNP_NCI_CallTrack m WHERE m.status ='1' ORDER BY m.name";
                }
                else if (Id != null)
                {
                    query = "select * from MNP_NCI_CallTrack m WHERE m.status ='1' and Id =" + Id + " ORDER BY m.name";
                }
                else
                {
                    query = "select * from MNP_NCI_CallTrack m WHERE m.status ='1' and Id=1 ORDER BY m.name";
                }
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

        public DataSet Get_Branches()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "select branchCode, name from Branches where status = '1' order by name";

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

        public DataTable Get_Consignment(string cn)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = @"Select b.name dst, b1.name origin,rc.reason DeliveredStatus, l.AttributeValue rc_status, c.consigneePhoneNo, c.shipperAddress,c.*  
                                from consignment c  
                                INNER JOIN Branches b1 ON b1.branchCode = c.orgin  
                                INNER JOIN Branches b ON b.branchCode = c.destination  
                                LEFT JOIN RunsheetConsignment rc ON c.consignmentNumber = rc.consignmentNumber  
                                AND rc.createdOn = (SELECT MAX(rc1.createdOn) FROM RunsheetConsignment rc1 WHERE rc1.consignmentNumber = rc.consignmentNumber)  
                                LEFT JOIN rvdbo.Lookup l ON rc.[Status] = l.Id  
                                WHERE c.consignmentnumber ='" + cn + "'";

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

        public DataTable Get_CNFromHistory(string cn)
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT * FROM MNP_NCI_Request m WHERE m.ConsignmentNumber ='" + cn + "' ORDER BY CreatedOn DESC";
                orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            { }
            finally
            { }
            return dt;
        }

        public DataTable Get_MaxTicketNo()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT MAX(TicketNumber)+1 TicketNumber FROM MNP_NCI_TicketNo m";
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

        public DataSet Get_RequestHistory(string cn)
        {
            DataSet dt = new DataSet();
            try
            {
                string query = @" SELECT c.TicketNo,B.NAME ORIGIN, B1.NAME DST,M.NAME REASON,S.NAME CALLSTATUS,ct.NAME calltrack,C.CONSIGNMENTNUMBER, C.SHIPPERNAME,  
                               C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON, c.CALLTIME PHONECALLTIME,  
                               case when ZU.U_NAME != '' then ZU.U_NAME ELSE 'CUSTOMER ENTRY' end CREATEDBY, 
                               C.MODIFIEDON,ZU1.U_NAME MODIFIEDBY, C.PORTALCREATEDON, C.PORTALCREATEDBY, 
                               CASE WHEN C.ISCOD = '1' THEN 'SEND TO COD PORTAL' ELSE '' END COD, 
                               CASE 
                               WHEN 
                               (SELECT MIN(m.CreatedOn) FROM MNP_NCI_Request m WHERE m.ConsignmentNumber ='" + cn + @"' AND m.CallTrack != '3') =  min(c.CreatedOn) 
                               AND c.Destination = zu.branchcode
                               THEN 'INITIATE BY DESTINATION'
                               WHEN 
                               (SELECT max(m.CreatedOn) FROM MNP_NCI_Request m WHERE m.ConsignmentNumber ='" + cn + @"' AND m.CallTrack IN ('2','3')) =  max(c.CreatedOn) THEN 'CLOSE'
                               WHEN zu.branchcode = c.Origin THEN 'UPDATE BY ORIGIN'
                               WHEN zu.branchcode = c.Destination THEN 'UPDATE BY DESTINATION'
                               WHEN c.PORTALCREATEDBY != ''  THEN 'UPDATE BY COD CUSTOMER' 
                               END LOGSTATUS, NR.NAME REATTEMPT 
                               FROM MNP_NCI_REQUEST C  
                               INNER JOIN BRANCHES B ON B.BRANCHCODE = C.ORIGIN  
                               INNER JOIN BRANCHES B1 ON B1.BRANCHCODE = C.DESTINATION  
                               INNER JOIN MNP_NCI_REASONS M ON C.REASON = M.ID  
                               INNER JOIN MNP_NCI_CALLSTATUS S ON C.CALLSTATUS = S.ID  
                               left JOIN ZNI_USER1 ZU ON C.CREATEDBY = ZU.U_ID 
                               LEFT JOIN MNP_NCI_CallTrack ct ON c.CallTrack = ct.Id  
                               LEFT JOIN ZNI_USER1 ZU1 ON C.MODIFIEDBY = ZU1.U_ID 
                               LEFT JOIN MNP_NCI_ReAttempt NR ON NR.ReAttempt_Id = C.ReAttempt 
                               WHERE C.CONSIGNMENTNUMBER = '" + cn + @"'  GROUP BY 
                               c.TicketNo,B.NAME, B1.NAME,M.NAME,S.NAME,C.CONSIGNMENTNUMBER, C.SHIPPERNAME,c.CALLTIME,  
                               C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON, 
                               ZU.U_NAME, C.MODIFIEDON,ZU1.U_NAME, C.PORTALCREATEDON, C.PORTALCREATEDBY,NR.NAME, 
                               C.ISCOD, c.CreatedOn, ct.NAME,c.Destination,zu.branchcode,zu.branchcode,c.Origin ,c.Destination 
                               ORDER BY C.CREATEDON DESC ";
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

        protected void ResetAll(object sender, EventArgs e)
        {
            txt_cn.Text = "";
            dd_origin.ClearSelection();
            ddl_Destination.ClearSelection();
            txt_shipper.Text = "";
            // txt_shippercell.Text = "";
            lbl_account.Text = "";
            txt_consignee.Text = "";
            txt_consignee_cell.Text = "";
            txt_ConsigneeAddress.Text = "";
            dd_reason.ClearSelection();
            dd_callstatus.ClearSelection();
            txt_comment.Text = "";
            lbl_cod.Text = "";
            GV_Histroy.DataSource = null;
            GV_Histroy.DataBind();
            txt_cn.Focus();
            div_grid.Visible = false;
            cod_row.Visible = false;
            lbl_cn.Text = "";
            lbl_origin.Text = "";
            hd_origin.Value = "";
            lbl_destination.Text = "";
            hd_destination.Value = "";
            lbl_account.Text = "";
            lbl_shippername.Text = "";
            hd_shippername.Value = "";
            lbl_bookingdate.Text = "";
            lbl_consignee.Text = "";
            lbl_consigneecell.Text = "";
            lbl_service.Text = "";
            lbl_consigneeaddress.Text = "";
            lbl_currentstatus.Text = "";
            div_box.Visible = false;
        }

        protected void dd_reason_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetNode();
            dd_standardnote.Focus();
        }

        protected void dd_calltrack_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_calltrack.SelectedValue == "3")
            {
                GetReAttempt();
                dd_reattemptremarks.Focus();
                dd_reattemptremarks.Enabled = true;
            }
            else
            {
                dd_reattemptremarks.Items.Clear();
                dd_reattemptremarks.Enabled = false;
            }
        }
        public void GetNode()
        {
            int rt_ID = int.Parse(dd_reason.SelectedValue.ToString());
            DataSet ds_node = Get_NodeByRT_ID(rt_ID);
            dd_standardnote.Items.Clear();
            if (ds_node.Tables[0].Rows.Count != 0)
            {
                dd_standardnote.DataTextField = "name";
                dd_standardnote.DataValueField = "note_ID";
                dd_standardnote.DataSource = ds_node.Tables[0].DefaultView;
                dd_standardnote.DataBind();
                dd_standardnote.Items.Insert(0, "Select Standard Notes");
                // ddl_notes.SelectedValue
                dd_standardnote.SelectedIndex = 0;

            }
        }

        public void GetReAttempt()
        {
            int rt_ID = int.Parse(dd_calltrack.SelectedValue.ToString());
            DataSet ds_reattempt = Get_ReAttemptRemarks(rt_ID);

            dd_reattemptremarks.Items.Clear();

            if (ds_reattempt.Tables[0].Rows.Count != 0)
            {
                dd_reattemptremarks.DataTextField = "name";
                dd_reattemptremarks.DataValueField = "ReAttempt_Id";
                dd_reattemptremarks.DataSource = ds_reattempt.Tables[0].DefaultView;
                dd_reattemptremarks.DataBind();

                dd_reattemptremarks.Items.Insert(0, "Select Re-Attempt Remarks");

                // ddl_notes.SelectedValue
                dd_reattemptremarks.SelectedIndex = 0;
            }
        }

        public DataSet Get_NodeByRT_ID(int rt_ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = $"SELECT * FROM MNP_NCI_Note c WHERE c.Reason_Id = '{rt_ID }' ORDER BY c.NAME ASC";

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

        public DataSet Get_ReAttemptRemarks(int rt_ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = $"SELECT * FROM MNP_NCI_ReAttempt n WHERE n.CallTrack_Id = '{rt_ID}' AND n.[STATUS] = '1' ORDER BY n.NAME ASC";

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

        public string SendMobileNumName(string consigneeNumber)
        {
            string Number = consigneeNumber.Replace("-", "");

            int numLength = Number.Length;
            string n2 = Number;

            if (numLength != 12)
            {
                if (numLength == 13)
                {
                    n2 = Number.Remove(0, 1);
                }
                else if (numLength == 11)
                {

                    string code = "92";
                    n2 = code + Number.Remove(0, 1);

                }
                else if (numLength == 10)
                {
                    string code = "92";
                    n2 = code + Number;
                }
                else
                {
                    string code = "92";
                    n2 = code + Number;
                }

            }
            return n2;
        }

        private void insertMNPSms(string consignment, string number, string SMSContent)
        {
            string temp = "";
            try
            {
                string sqlString = $@"INSERT INTO MnP_SmsStatus  (consignmentNumber,Recepient,MessageContent,[STATUS],CreatedOn,CreatedBy,SMSFormType) VALUES 
                                               ('{consignment}','{number}','{SMSContent}',0,GETDATE(),'{ Session["U_ID"].ToString()} ','5')";

                orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.ExecuteNonQuery();
                temp = "Succes";
            }
            catch (Exception Err)
            {
                temp = "Error";
            }
            finally
            { orcl.Close(); }


        }

        public string insertRequest(string hd_ticket, string ticketno, string cn, string origin, string dst, string shippername, string account, string consignee, string consigneecell, string consigneeaddress, string reason, string standardnotes, string callstatus, string comment, string calltrack, string iscod, string calltime, string reattemptremarks, string reasonName, string CallTrackName)
        {
            string temp;
            try
            {
                if (ticketno == null || ticketno == "")
                {
                    DataTable dt_ticket = Get_MaxTicketNo();

                    if (dt_ticket.Rows.Count > 0)
                    {
                        ticketno = dt_ticket.Rows[0]["TicketNumber"].ToString();
                    }
                }
                using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString)))
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    SqlTransaction transaction;

                    transaction = conn.BeginTransaction("Insert advise");
                    command.Connection = conn;
                    command.Transaction = transaction;
                    try
                    {
                        if (string.IsNullOrEmpty(ticketno))
                            throw new Exception();

                        int TicketCount = 0;

                        string ReasonDescription = "Reason: " + reasonName;

                        command.CommandText = " update MNP_NCI_Request set isLast = null where ConsignmentNumber = '" + cn + "' ";
                        int check2 = command.ExecuteNonQuery();

                        string sql_CountConsignment = " Select * from MNP_NCI_Request r where r.ConsignmentNumber = '" + cn + "' ";
                        command.CommandText = sql_CountConsignment;
                        DataTable dt_Id = new DataTable();
                        SqlDataAdapter oda = new SqlDataAdapter(command);
                        oda.Fill(dt_Id);
                        if (dt_Id.Rows.Count == 0)
                        {
                        }
                        else
                        {
                            ReasonDescription = "advise: Call Status - " + CallTrackName;
                            //dosra check consignment and ticket match with existing

                            string sql_ConsignmentTicketCheck = " Select * from MNP_NCI_Request r where r.ConsignmentNumber = '" + cn + "' and r.TicketNo='" + hd_ticket + "' ";
                            command.CommandText = sql_ConsignmentTicketCheck;
                            DataTable dt_TicketConsignment = new DataTable();
                            SqlDataAdapter oda_TicketConsignment = new SqlDataAdapter(command);
                            oda_TicketConsignment.Fill(dt_TicketConsignment);
                            if (dt_TicketConsignment.Rows.Count == 0)
                            {
                                ReasonDescription = "Reason: " + reasonName;
                            }
                            else
                            {
                                ReasonDescription = "advise: " + CallTrackName;
                            }
                        }

                        
                        string sql_TickDuplicate = " Select * from MNP_NCI_Request r Where r.TicketNo='" + ticketno + "' order by CreatedOn desc ;Select count(distinct(TicketNo)) as TicketCount from MNP_NCI_Request r where r.ConsignmentNumber = '" + cn + "' ";
                        command.CommandText = sql_TickDuplicate;
                        DataSet dt_DuplicateChk = new DataSet();
                        SqlDataAdapter oda_DupCNCheck = new SqlDataAdapter(command);
                        oda_DupCNCheck.Fill(dt_DuplicateChk);
                        if (dt_DuplicateChk.Tables[0].Rows.Count > 0)
                        {
                            if (dt_DuplicateChk.Tables[1].Rows.Count > 0)
                            {
                                TicketCount = int.Parse(dt_DuplicateChk.Tables[1].Rows[0]["TicketCount"].ToString());
                            }

                            if (TicketCount <= 2)
                            {
                                if (dt_DuplicateChk.Tables[0].Rows[0]["CallTrack"].ToString() == "2")
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Ticket already RTS.');", true);
                                    throw new Exception();
                                }
                                if (dt_DuplicateChk.Tables[0].Rows[0]["CallTrack"].ToString() == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Ticket already Reattempt.');", true);
                                    throw new Exception();
                                }
                            }

                            //if (dt_DuplicateChk.Tables[0].Rows[0]["consignmentNumber"].ToString() != cn)
                            //{
                            //    throw new Exception();
                            //}

                        }

                        // Changes 25 March 2021
                        if (dt_DuplicateChk.Tables[0].Rows.Count > 0)
                        {
                            if (dt_DuplicateChk.Tables[0].Rows[0]["consignmentNumber"].ToString() != cn)
                            {
                                string sqlString = $@" INSERT INTO MNP_NCI_Request(  TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee ,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,CreatedBy,CallTime,ReAttempt  ,islast ) 
                                   VALUES ( (SELECT MAX(TicketNumber)+1  FROM MNP_NCI_TicketNo),'{cn}','{shippername }', '{account }', '{consignee}',  '{consigneecell}',  '{consigneeaddress}',  '{origin}',  '{dst}',  '{reason}',  '{standardnotes }',  '{callstatus}',  '{calltrack}',  '{comment} ',  '{iscod}',  GETDATE(),'{ Session["U_ID"].ToString()}',  '{calltime}', '{reattemptremarks}'  ,'1') ";
                                command.CommandText = sqlString;
                                int check = command.ExecuteNonQuery();
                            }
                            else
                            {
                                string sqlString = $@" INSERT INTO MNP_NCI_Request(  TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee ,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,CreatedBy,CallTime,ReAttempt  ,islast ) 
                                   VALUES ( '{ticketno }','{cn}','{shippername }', '{account }', '{consignee}',  '{consigneecell}',  '{consigneeaddress}',  '{origin}',  '{dst}',  '{reason}',  '{standardnotes }',  '{callstatus}',  '{calltrack}',  '{comment} ',  '{iscod}',  GETDATE(),'{ Session["U_ID"].ToString()}',  '{calltime}', '{reattemptremarks}'  ,'1') ";
                                command.CommandText = sqlString;
                                int check = command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string sqlString = $@" INSERT INTO MNP_NCI_Request(  TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee ,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,CreatedBy,CallTime,ReAttempt  ,islast ) 
                                   VALUES ( '{ticketno }','{cn}','{shippername }', '{account }', '{consignee}',  '{consigneecell}',  '{consigneeaddress}',  '{origin}',  '{dst}',  '{reason}',  '{standardnotes }',  '{callstatus}',  '{calltrack}',  '{comment} ',  '{iscod}',  GETDATE(),'{ Session["U_ID"].ToString()}',  '{calltime}', '{reattemptremarks}'  ,'1') ";
                            command.CommandText = sqlString;
                            int check = command.ExecuteNonQuery();
                        }

                        //
                        //string sqlString = $@" INSERT INTO MNP_NCI_Request(  TicketNo,ConsignmentNumber,ShipperName,AccountNo,Consignee ,ConsigneeCell,ConsigneeAddress,Origin,Destination,Reason,StandardNotes,CallStatus,CallTrack,Comment,ISCOD,CreatedOn,CreatedBy,CallTime,ReAttempt  ,islast ) 
                        //           VALUES ( '{ticketno }','{cn}','{shippername }', '{account }', '{consignee}',  '{consigneecell}',  '{consigneeaddress}',  '{origin}',  '{dst}',  '{reason}',  '{standardnotes }',  '{callstatus}',  '{calltrack}',  '{comment} ',  '{iscod}',  GETDATE(),'{ Session["U_ID"].ToString()}',  '{calltime}', '{reattemptremarks}'  ,'1') ";
                        //command.CommandText = sqlString;
                        //int check = command.ExecuteNonQuery();

                        string sqlStringHistory = @"insert into ConsignmentsTrackingHistory (consignmentNumber,stateID,transactionTime,currentLocation,reason,internationalRemarks)  
                                   values('" + cn + "',22,getdate(),'" + Session["LocationName"].ToString() + "','NCI','" + ReasonDescription + "')";
                        command.CommandText = sqlStringHistory;
                        int Status2 = command.ExecuteNonQuery();

                        string query2;
                        if (hd_ticket == ticketno)
                        {
                            query2 = "";
                        }
                        else
                        {
                            query2 = "UPDATE MNP_NCI_TicketNo SET TicketNumber = '" + ticketno + "', CreatedOn = GETDATE(), CreatedBy = '" + Session["U_ID"] + "' WHERE ID = '1' ";
                        }

                        if (hd_ticket != ticketno)
                        {
                            command.CommandText = query2;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        temp = "Succes";
                    }

                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                            return "Error inserting discount record";
                        }
                        catch (Exception ex2)
                        {
                            return "Error inserting discount record, rollback transaction failed";
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                temp = "Error";
            }
            finally
            { }

            return temp;
        }

    }
}