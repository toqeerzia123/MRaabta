using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Text;
using System.IO;
using System.Globalization;
using Dapper;

namespace MRaabta.Files
{
    public partial class NCIReport : System.Web.UI.Page
    {
        CL_Customer clvar = new CL_Customer();
        Cl_Variables cl_var = new Cl_Variables();
        CommonFunction func = new CommonFunction();
        SqlConnection orcl;

        string startDate_ = ""; string endDate_ = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Get_CallTrackerStatus();

                report_type.Items.Insert(0, new ListItem("Initiate Request", "1"));
                report_type.Items.Insert(1, new ListItem("Receive Request", "2"));
                report_type.Items.Insert(2, new ListItem("Initiate by me", "3"));

                //if (rb_origin.Checked)
                //{
                //    btn_save_Click(sender, e);
                //}

                //if (rb_dst.Checked)
                //{
                //    btn_save_Click(sender, e);
                //} btn_save_Click(sender, e);

            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                GV_Histroy.DataSource = null;
                GV_Histroy.DataBind();
                Errorid.Text = "";

                string cn = txt_cn.Text.Trim();
                if (startDate.Text == "" || endDate.Text == "")
                {
                    if (startDate.Text == "")
                    {
                        Errorid.Text = "Please provide start date! ";
                        return;
                    }
                    if (endDate.Text == "")
                    {
                        Errorid.Text = "Please provide end date! ";
                        return;
                    }
                }
                startDate_ = DateTime.ParseExact(startDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                endDate_ = DateTime.ParseExact(endDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                String callTrack = dd_calltrack.SelectedValue.ToString();
                string accountNo = txt_Acc.Text;
                DataSet ds = Get_RequestHistory(cn, startDate_, endDate_, accountNo, callTrack);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    GV_Histroy.DataSource = ds.Tables[0].DefaultView;
                    GV_Histroy.DataBind();
                    
                    Gv_CSV.DataSource = ds.Tables[0].DefaultView;
                    Gv_CSV.DataBind();
                    foreach (GridViewRow row in GV_Histroy.Rows)
                    {
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
                    }
                }
                else
                {
                    Errorid.Text = "NO RECORD FOUND...";
                }
            }
            catch (Exception er)
            {
                Errorid.Text = "Error finding records! ";
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink lbl_CN = (HyperLink)e.Row.FindControl("lbl_CN");

                if (lbl_CN != null)
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    string cn = drv["CONSIGNMENTNUMBER"].ToString();

                    //lbl_CN.NavigateUrl = "GenerateAdvicePending.aspx?cn=" + cn;
                    lbl_CN.NavigateUrl = "~/GenerateAdvicePending?cn=" + cn;

                }
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Histroy.PageIndex = e.NewPageIndex;
            btn_save_Click(sender, e);
        }
        public void Get_CallTrackerStatus()
        {
            DataSet ds_callTrackerstatus = Get_AllCallTrackerStatus();
            if (ds_callTrackerstatus.Tables[0].Rows.Count != 0)
            {
                dd_calltrack.DataTextField = "Name";
                dd_calltrack.DataValueField = "id";
                dd_calltrack.DataSource = ds_callTrackerstatus.Tables[0].DefaultView;
                dd_calltrack.DataBind();
            }
            dd_calltrack.Items.Insert(0, new ListItem("Select Call Track", "0"));
            dd_calltrack.SelectedIndex = 0;  //first item

        }
        public DataSet Get_AllCallTrackerStatus()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "select * from MNP_NCI_CallTrack m WHERE m.status ='1' ORDER BY m.name";

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

        //public void ExportToCSVOriginal(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //GV_Histroy.DataSource = null;
        //        //GV_Histroy.DataBind();
        //        Errorid.Text = "";

        //        string cn = txt_cn.Text.Trim();
        //        if (startDate.Text == "" || endDate.Text == "")
        //        {
        //            if (startDate.Text == "")
        //            {
        //                Errorid.Text = "Please provide start date! ";
        //                return;
        //            }
        //            if (endDate.Text == "")
        //            {
        //                Errorid.Text = "Please provide end date! ";
        //                return;
        //            }
        //        }
        //        startDate_ = DateTime.Parse(startDate.Text).ToString("yyyy-MM-dd");
        //        endDate_ = DateTime.Parse(endDate.Text).ToString("yyyy-MM-dd");

        //        String callTrack = dd_calltrack.SelectedValue.ToString();
        //        string accountNo = txt_Acc.Text;

        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", "attachment;filename=NCI_Report.xls");
        //        Response.Charset = "";
        //        //Response.ContentType = "application/ms-excel";
        //        Response.ContentType = "application/vnd.ms-excel";


        //        using (StringWriter sw = new StringWriter())
        //        {
        //            HtmlTextWriter hw = new HtmlTextWriter(sw);


        //            btn_save_Click(sender, e);
                  
        //            GV_Histroy.RenderControl(hw);



        //            //style to format numbers to string
        //            string style = @"<style> .textmode { } </style>";
        //            HttpContext.Current.Response.Write(style);
        //            HttpContext.Current.Response.Output.Write(sw.ToString());
                  

        //            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //            HttpContext.Current.ApplicationInstance.CompleteRequest();

        //            //    Response.AddHeader("content-disposition", "attachment;filename=NCIReport.xls");
        //            //Response.Charset = "";
        //            //Response.ContentType = "application/vnd.ms-excel";
        //            //using (StringWriter sw = new StringWriter())
        //            //{
        //            //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //            //    GV_Histroy.DataSource = ds.Tables[0].DefaultView;
        //            //    GV_Histroy.DataBind();
        //            //    GV_Histroy.RenderControl(hw);


        //            //    //style to format numbers to string
        //            //    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //            //    Response.Write(style);
        //            //    Response.Output.Write(sw.ToString());
        //            //    Response.Flush();
        //            //    Response.End();
        //            //}
        //        }
        //        }
        //    catch (Exception er)
        //    {
        //        Errorid.Text = er.Message.ToString();
        //    }
        //}

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }   
        
        public DataSet Get_RequestHistory(string cn, string startDate, string endDate, string accountNo, String callTrack)
        {
            DataSet dt = new DataSet();
            try
            {
                #region query__OLD
                string query__ = "SELECT \n"
                            + "CASE \n"
                            + "WHEN zu.branchcode = c.Origin THEN 'UPDATE BY ORIGIN'\n"
                            + "WHEN zu.branchcode = c.Destination THEN 'UPDATE BY DESTINATION'\n"
                            + "--WHEN zu.branchcode = c.Destination THEN 'UPDATE BY COD CUSTOMER'\n"
                            + "END LOGSTATUS,\n"
                            + "B.NAME ORIGIN, B1.NAME DST,M.NAME REASON,S.NAME CALLSTATUS,C.CONSIGNMENTNUMBER, C.SHIPPERNAME,  \n"
                            + "C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON,  \n"
                            + "ZU.U_NAME CREATEDBY, C.MODIFIEDON,ZU1.U_NAME MODIFIEDBY, C.PORTALCREATEDON, C.PORTALCREATEDBY, \n"
                            + "CASE WHEN C.ISCOD = '1' THEN 'SEND TO COD PORTAL' ELSE '' END COD \n"
                            + "FROM MNP_NCI_REQUEST C  \n"
                            + "INNER JOIN BRANCHES B ON B.BRANCHCODE = C.ORIGIN  \n"
                            + "INNER JOIN BRANCHES B1 ON B1.BRANCHCODE = C.DESTINATION  \n"
                            + "INNER JOIN MNP_NCI_REASONS M ON C.REASON = M.ID  \n"
                            + "INNER JOIN MNP_NCI_CALLSTATUS S ON C.CALLSTATUS = S.ID  \n"
                            + "INNER JOIN ZNI_USER1 ZU ON C.CREATEDBY = ZU.U_ID \n"
                            + "LEFT JOIN ZNI_USER1 ZU1 ON C.MODIFIEDBY = ZU1.U_ID \n"
                            + "WHERE \n";
                //+ "c.CreatedOn = (SELECT min(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n";
                //if (rb_origin.Checked)
                //{
                //    query__ += "c.CreatedOn = (SELECT MIN(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n"
                //            + "AND zu.branchcode = '" + Session["branchcode"].ToString() + "' \n";
                //}
                //if (rb_dst.Checked)
                //{
                //    query__ += "c.CreatedOn = (SELECT MAX(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n"
                //            + "AND c.origin = '" + Session["branchcode"].ToString() + "' \n";
                //}
                if (cn != "")
                {
                    query__ += "AND C.CONSIGNMENTNUMBER = '" + cn + "' \n";
                }
                if (startDate != "" && startDate != null)
                {
                    query__ += "AND cast(C.CREATEDON as date) = '" + startDate + "' \n";
                }
                query__ += "ORDER BY C.CREATEDON DESC \n";

                #endregion

                if (cod_dropdown.SelectedValue == "2")
                {
                    clvar.CODType = " INNER JOIN CreditClients cc ON cc.accountNo=c.AccountNo AND cc.CODType='3'  ";
                }

                string query = "SELECT  \n"
              + "MAX(x.LOGSTATUS) LOGSTATUS, max(ConsignmentNumber) ConsignmentNumber,x.SHIPPERNAME,ACCOUNTNO,ORIGIN, DST, REASON,CALLSTATUS,max(CREATEDON) CREATEDON \n"
              + "FROM ( \n"
              + "SELECT   \n"
              + "	CASE  \n"
              + "		WHEN (SELECT max(a.CreatedOn) FROM MNP_NCI_REQUEST a  \n"
              + "			  WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) = max(c.CreatedOn)  \n"
              + "		and c.Origin = zu.branchcode THEN 'UPDATE BY ORIGIN' \n"
              + " \n"
              + "		WHEN (SELECT max(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) = max(c.CreatedOn)  \n"
              + "		and zu.branchcode = c.Destination THEN 'UPDATE BY DESTINATION' \n"
              + " \n"
              + "		WHEN (SELECT max(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) = max(c.CreatedOn)  \n"
              + "		and c.PORTALCREATEDBY !=''  THEN 'UPDATE BY COD CUSTOMER' \n"
              + "	END LOGSTATUS,  \n"
              + "C.CONSIGNMENTNUMBER,B.NAME ORIGIN, B1.NAME DST,M.NAME REASON,S.NAME CALLSTATUS, C.SHIPPERNAME,   \n"
              + "C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON,   \n"
              + "ZU.U_NAME CREATEDBY, C.PORTALCREATEDBY,  \n"
              + "CASE WHEN C.ISCOD = '1' THEN 'SEND TO COD PORTAL' ELSE '' END COD,c.Destination, c.Origin OriginBranch  \n"
              + "FROM MNP_NCI_REQUEST C   \n"
              + "INNER JOIN BRANCHES B ON B.BRANCHCODE = C.ORIGIN   \n"
              + "INNER JOIN BRANCHES B1 ON B1.BRANCHCODE = C.DESTINATION   \n"
              + "INNER JOIN MNP_NCI_REASONS M ON C.REASON = M.ID   \n"
              + "INNER JOIN MNP_NCI_CALLSTATUS S ON C.CALLSTATUS = S.ID   \n" +    clvar.CODType 
              + "LEFT JOIN ZNI_USER1 ZU ON C.CREATEDBY = ZU.U_ID  \n"
              // + "LEFT JOIN ZNI_USER1 ZU1 ON C.MODIFIEDBY = ZU1.U_ID  \n"
              + " WHERE  \n";
                // + "c.CreatedOn = (SELECT max(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n";

                if (report_type.SelectedValue == "1")
                {
                    query += "c.CreatedOn = (SELECT MIN(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n"
                            + "AND zu.branchcode = '" + Session["branchcode"].ToString() + "' \n";
                }
                if (report_type.SelectedValue == "2")
                {
                    query += "c.CreatedOn = (SELECT MAX(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n"
                            + "AND c.origin = '" + Session["branchcode"].ToString() + "' \n";
                }
                if (report_type.SelectedValue == "3")
                {
                    query += "  c.CreatedBy='" + Session["U_ID"].ToString() + "' ";
                }

                if (cod_dropdown.SelectedValue == "0")
                {
                    query += "  AND isnull(c.ISCOD,'0')='0'  ";
                }
                if (cod_dropdown.SelectedValue == "1")
                {
                    query += "  AND isnull(c.ISCOD,'0')='1'  ";
                }
                if (startDate != "" && startDate != null)
                {
                    query += " AND cast(C.CREATEDON as date) >= cast('" + startDate + "' as date) \n";
                    query += " AND cast(C.CREATEDON as date) <= cast('" + endDate + "' as date) \n";
                }
                if (cn != "")
                {
                    query += "AND C.CONSIGNMENTNUMBER = '" + cn + "' \n";
                }
                if (accountNo != "")
                {
                    query += "AND C.AccountNo  = '" + accountNo.Trim() + "' \n";
                }
                if (callTrack != "0")
                {
                    query += " AND  c.CallTrack=" + callTrack + " \n";
                }

                //  + "AND c.Origin != '" + Session["branchcode"].ToString() + "'  \n"
                query += "GROUP BY \n"
               + "c.Origin,zu.branchcode, Destination, \n"
               + "B.NAME, B1.NAME,M.NAME,S.NAME,C.CONSIGNMENTNUMBER, C.SHIPPERNAME,   \n"
               + "C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON,   \n"
               + "ZU.U_NAME, C.PORTALCREATEDBY,  \n"
               + "C.ISCOD, c.Destination \n"
               + " \n"
               + "UNION ALL \n"
               + " \n"
               + "SELECT  \n"
               + "''LOGSTATUS,C.CONSIGNMENTNUMBER,B.NAME ORIGIN, B1.NAME DST,M.NAME REASON,S.NAME CALLSTATUS, C.SHIPPERNAME,   \n"
               + "C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON,   \n"
               + "ZU.U_NAME CREATEDBY, C.PORTALCREATEDBY,  \n"
               + "CASE WHEN C.ISCOD = '1' THEN 'SEND TO COD PORTAL' ELSE '' END COD,c.Destination, c.Origin OriginBranch  \n"
               + "FROM MNP_NCI_REQUEST C   \n"
               + "INNER JOIN BRANCHES B ON B.BRANCHCODE = C.ORIGIN   \n"
               + "INNER JOIN BRANCHES B1 ON B1.BRANCHCODE = C.DESTINATION   \n"
               + "INNER JOIN MNP_NCI_REASONS M ON C.REASON = M.ID   \n"
               + "INNER JOIN MNP_NCI_CALLSTATUS S ON C.CALLSTATUS = S.ID   \n"
               + "INNER JOIN ZNI_USER1 ZU ON C.CREATEDBY = ZU.U_ID  \n" + clvar.CODType
               + "LEFT JOIN ZNI_USER1 ZU1 ON C.MODIFIEDBY = ZU1.U_ID  \n"
               + "WHERE  \n";
                //  + "c.CreatedOn = (SELECT min(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER)  \n";
                //  + "AND zu.branchcode = '" + Session["branchcode"].ToString() + "'  \n"

                if (report_type.SelectedValue == "1")
                {
                    query += "c.CreatedOn = (SELECT MIN(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n"
                            + "AND zu.branchcode = '" + Session["branchcode"].ToString() + "' \n";
                }
                if (report_type.SelectedValue == "2")
                {
                    query += "c.CreatedOn = (SELECT MAX(a.CreatedOn) FROM MNP_NCI_REQUEST a WHERE a.ConsignmentNumber = c.CONSIGNMENTNUMBER) \n"
                            + "AND c.origin = '" + Session["branchcode"].ToString() + "' \n";
                }
                if (report_type.SelectedValue == "3")
                {
                    query += "  c.CreatedBy='" + Session["U_ID"].ToString() + "' ";
                }
                if (cod_dropdown.SelectedValue == "0")
                {
                    query += "  AND isnull(c.ISCOD,'0')='0'  ";
                }
                if (cod_dropdown.SelectedValue == "1")
                {
                    query += "  AND isnull(c.ISCOD,'0')='1'  ";
                }
                if (cn != "")
                {
                    query += "AND C.CONSIGNMENTNUMBER = '" + cn + "' \n";
                }
                if (startDate != "" && startDate != null)
                {
                    query += " AND cast(C.CREATEDON as date) >= cast('" + startDate + "' as date) \n";
                    query += " AND cast(C.CREATEDON as date) <= cast('" + endDate + "' as date) \n";
                }
                if (accountNo != "")
                {
                    query += "AND C.AccountNo  = '" + accountNo.Trim() + "' \n";
                }
                if (callTrack != "0")
                {
                    query += " AND  c.CallTrack=" + callTrack + " \n";
                }
                query += "GROUP BY \n"
               + "B.NAME, B1.NAME,M.NAME,S.NAME,C.CONSIGNMENTNUMBER, C.SHIPPERNAME,   \n"
               + "C.SHIPPERCELL, C.ACCOUNTNO,C.CONSIGNEE,C.CONSIGNEECELL,C.CONSIGNEEADDRESS,C.COMMENT, C.CREATEDON,   \n"
               + "ZU.U_NAME, C.PORTALCREATEDBY,  \n"
               + "C.ISCOD, zu.branchcode,c.Origin,c.Destination \n"
               + ") x \n";
                if (report_type.SelectedValue == "1")
                {
                    query += "WHERE x.Destination = '" + Session["branchcode"].ToString() + "' \n";
                }
                if (report_type.SelectedValue == "2")
                {
                    query += "WHERE x.OriginBranch = '" + Session["branchcode"].ToString() + "' \n";
                }
                query += "GROUP BY \n"
               + " x.ConsignmentNumber,ORIGIN, DST, REASON,CALLSTATUS,SHIPPERNAME,   \n"
               + " SHIPPERCELL, ACCOUNTNO,CONSIGNEE,CONSIGNEECELL,CONSIGNEEADDRESS,COD \n"
               + "   ORDER BY CREATEDON desc ";




                orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
                int rows = dt.Tables[0].Rows.Count;
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return dt;
        }


    }
}