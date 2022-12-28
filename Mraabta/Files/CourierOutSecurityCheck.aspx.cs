using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Services;

namespace MRaabta.Files
{
    public partial class CourierOutSecurityCheck : System.Web.UI.Page
    {
        Variable clvar = new Variable();
        bayer_Function b_fun = new bayer_Function();
        DataSet ds_grid = new DataSet();
        cl_Encryption enc = new cl_Encryption();
        CommonFunction CF = new CommonFunction();
        DataTable dataSet = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {


            if (body2.Visible == true)
            {
                binddata();
            }
            else
            {

                Bindchild();
                bindcod();
                if (Session["last"] != null)
                {
                    pnl_lbl.Controls.Clear();
                    Label lbl = Session["last"] as Label;
                    pnl_lbl.Controls.Add(lbl);
                }
            }

            try
            {
                string name = Session["U_NAME"].ToString();
            }
            catch (Exception er)
            {
                Response.Redirect("~/login");
            }

            if (!Page.IsPostBack)
            {
                child_head.Visible = false;
                child_panel.Visible = false;
                child_panel2.Visible = false;
                body2.Visible = false;
                body1.Visible = true;
                //ask sir bilal
                lbl_msg.Visible = false;
                dd_start_date.Text = DateTime.Parse(DateTime.Now.Date.ToString()).ToString("yyyy-MM-dd"); //"2020-01-21"; 
                                                                                                          //   dd_start_date.Text = DateTime.Now.Date.ToString(); //"2020-01-21"; 
                DataTable dt_cons = new DataTable();
                dt_cons.Columns.Add("RunsheetNumber", typeof(string));
                dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
                dt_cons.Columns.Add("ISCOD", typeof(int));
                dt_cons.Columns.Add("ColorLabel", typeof(string));
                //dt_cons.Columns.Add("Remarks", typeof(string));

                Session["dt_cons"] = dt_cons;

                dt_cons = new DataTable();
                dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
                dt_cons.Columns.Add("ColorLabel", typeof(string));

                Session["cod_con"] = dt_cons;

                dt_cons = new DataTable();
                dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
                dt_cons.Columns.Add("RunsheetNumber", typeof(string));
                dt_cons.Columns.Add("ISCOD", typeof(int));
                dt_cons.Columns.Add("ColorLabel", typeof(string));
                dt_cons.Columns.Add("Remarks", typeof(string));

                Session["consignment"] = dt_cons;


                DataTable dt_Runsheet = new DataTable();
                dt_Runsheet.Columns.Add("RUNSHEETNUMBER", typeof(int));
                dt_Runsheet.Columns.Add("RUNSHEETDATE", typeof(DateTime));
                dt_Runsheet.Columns.Add("ROUTECODE", typeof(string));
                dt_Runsheet.Columns.Add("CNCOUNT", typeof(int));
                dt_Runsheet.Columns.Add("CODCNCOUNT", typeof(int));
                dt_Runsheet.Columns.Add("ColorLabel", typeof(string));
                dt_Runsheet.Columns.Add("Remarks", typeof(string));
                Session["runsheet"] = dt_Runsheet;

            }
        }
        protected void submit_Click(object sender, EventArgs e)
        {
            pnl_lbl.Controls.Clear();
            Session["last"] = null;
            child_panel.Controls.Clear();
            child_panel2.Controls.Clear();
            body2.Visible = false;
            lbl_sdo.Visible = false;
            lbl_sdoname.Visible = false;
            lbl_branch.Visible = false;
            lbl_branchname.Visible = false;
            body1.Visible = true;
            innerbody.Visible = true;
            Session["consignment"] = null;
            DataTable dt_cons = new DataTable();
            dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
            dt_cons.Columns.Add("RunsheetNumber", typeof(string));
            dt_cons.Columns.Add("ISCOD", typeof(int));
            dt_cons.Columns.Add("ColorLabel", typeof(string));
            dt_cons.Columns.Add("Remarks", typeof(string));


            Session["consignment"] = dt_cons;

            Session["cod_con"] = null;
            dt_cons = new DataTable();
            dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
            dt_cons.Columns.Add("ColorLabel", typeof(string));

            Session["cod_con"] = dt_cons;

            Session["dt_cons"] = null;
            dt_cons = null;
            dt_cons = new DataTable();
            dt_cons.Columns.Add("RunsheetNumber", typeof(string));
            dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
            dt_cons.Columns.Add("ISCOD", typeof(int));
            dt_cons.Columns.Add("ColorLabel", typeof(string));
            //dt_cons.Columns.Add("Remarks", typeof(string));

            Session["dt_cons"] = dt_cons;

            Session["runsheet"] = null;
            DataTable dt_Runsheet = new DataTable();
            dt_Runsheet.Columns.Add("RUNSHEETNUMBER", typeof(int));
            dt_Runsheet.Columns.Add("RUNSHEETDATE", typeof(DateTime));
            dt_Runsheet.Columns.Add("ROUTECODE", typeof(string));
            dt_Runsheet.Columns.Add("CNCOUNT", typeof(int));
            dt_Runsheet.Columns.Add("CODCNCOUNT", typeof(int));
            dt_Runsheet.Columns.Add("ColorLabel", typeof(string));
            dt_Runsheet.Columns.Add("Remarks", typeof(string));
            Session["runsheet"] = dt_Runsheet;

            lbl_msg.Visible = false;
            GridView1.DataSource = null;
            GridView1.DataBind();
            dd_CN_No.Text = "";
            dd_R_NO.Text = " ";
            Session["SDO"] = dd_RunSheetNo.Text;
            Session["Date"] = dd_start_date.Text;
            if (dd_RunSheetNo.Text != "" && dd_start_date.Text != " ")
            {

                try
                {
                    DataSet ds;
                    ds = getRunsheetDetail(dd_start_date.Text, dd_RunSheetNo.Text);

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        lbl_sdoname.Text = dd_RunSheetNo.Text + " – " + ds.Tables[0].Rows[0]["RiderName"].ToString();
                        lbl_branchname.Text = ds.Tables[0].Rows[0]["BranchName"].ToString();
                        lbl_sdo.Visible = true;
                        lbl_sdoname.Visible = true;
                        lbl_branch.Visible = true;
                        lbl_branchname.Visible = true;
                        System.Data.DataColumn newColumn = new System.Data.DataColumn("ColorLabel", typeof(string));
                        newColumn.DefaultValue = "0";
                        ds.Tables[0].Columns.Add(newColumn);
                        newColumn = new System.Data.DataColumn("Remarks", typeof(string));
                        newColumn.DefaultValue = " ";
                        ds.Tables[0].Columns.Add(newColumn);
                        DataTable dt = new DataTable();
                        dt.Columns.Add("RunsheetNumber", typeof(string));
                        dt.Columns.Add("ConsignmentNumber", typeof(string));
                        dt.Columns.Add("ISCOD", typeof(int));
                        dt.Columns.Add("ColorLabel", typeof(int));


                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataSet ds_con = getConsignmentDetail(ds.Tables[0].Rows[i]["RUNSHEETNUMBER"].ToString());
                            //System.Data.DataColumn newColumncons = new System.Data.DataColumn("ColorLabel", typeof(string));
                            //newColumncons.DefaultValue = "0";
                            //ds_con.Tables[0].Columns.Add(newColumncons);
                            //newColumncons = new System.Data.DataColumn("Remarks", typeof(string));
                            //newColumncons.DefaultValue = " ";
                            //ds_con.Tables[0].Columns.Add(newColumncons);
                            for (int j = 0; j < ds_con.Tables[0].Rows.Count; j++)
                            {
                                dt.Rows.Add(ds_con.Tables[0].Rows[j]["RUNSHEETNUMBER"], ds_con.Tables[0].Rows[j]["ConsignmentNumber"], ds_con.Tables[0].Rows[j]["ISCOD"], "0");//, ds_con.Tables[0].Rows[j]["ISCOD"], ds_con.Tables[0].Rows[j]["ColorLabel"], ds_con.Tables[0].Rows[j]["Remarks"]);

                            }

                        }
                        Session["dt_cons"] = dt;
                        GridView1.DataSource = ds.Tables[0];
                        GridView1.DataBind();
                        Session["runsheet"] = ds.Tables[0];
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            "alert('Runsheet not found');", true);
                        //Page.Response.Redirect(Page.Request.Url.ToString(), true);
                    }
                }
                catch (Exception Err)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                        "alert('Runsheet not found');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                        "alert('Please Fill Full Form');", true);
                //txt_route.Text = "";
                //dd_start_date.Text = "";
            }

        }
        public DataSet getRunsheetDetail(string date, string sdocdoe)
        {
            DateTime d = DateTime.Parse(date).AddDays(1);
            String dateto = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);//d.Date.ToString();
            DataSet ds = new DataSet();
            try
            {

                //String query = "SELECT RUNSHEETNUMBER,RUNSHEETDATE, ROUTECODE, SDOCODE,ISNULL(SUM(CNCOUNT),'0') CNCOUNT,ISNULL(SUM(CODCNCOUNT),'0') CODCNCOUNT " + "\n"
                //     + " FROM(SELECT R.RUNSHEETNUMBER, CONVERT(VARCHAR(10), R.RUNSHEETDATE, 105) RUNSHEETDATE, R.ROUTECODE, R.RIDERCODE SDOCODE, CASE WHEN ISNULL(RC.COD, '0') = '0' THEN COUNT(RC.CONSIGNMENTNUMBER) END CNCOUNT, " + "\n"
                //      + " CASE WHEN ISNULL(RC.COD, '0') = '1' THEN COUNT(RC.CONSIGNMENTNUMBER) END CODCNCOUNT FROM " + "\n"
                //     + " RUNSHEET R INNER JOIN RUNSHEETCONSIGNMENT RC ON R.RUNSHEETNUMBER = RC.RUNSHEETNUMBER AND R.BRANCHCODE = RC.BRANCHCODE AND R.ROUTECODE = RC.ROUTECODE " + "\n"
                //     + " WHERE R.RUNSHEETDATE = '" + date + "' AND R.RIDERCODE = '" + sdocdoe + "' AND RC.BRANCHCODE = '"+ Session["BRANCHCODE"] +"'  GROUP BY R.RUNSHEETNUMBER, R.RUNSHEETDATE, R.ROUTECODE, R.RIDERCODE, RC.COD) X GROUP BY RUNSHEETNUMBER, RUNSHEETDATE, ROUTECODE, SDOCODE ORDER BY 1,5 ";

                String query = "SELECT RUNSHEETNUMBER,RUNSHEETDATE,RunsheetType,ROUTECODE,RouteName,ISNULL(RouteType ,'') RouteType,SDOCODE,ISNULL(SUM(CNCOUNT) ,'0') CNCOUNT,ISNULL(SUM(CODCNCOUNT) ,'0')CODCNCOUNT, RiderName, BranchName" + "\n"
                                + "  FROM(SELECT R.RUNSHEETNUMBER, CONVERT(VARCHAR(10), R.RUNSHEETDATE, 105) RUNSHEETDATE, l.code RunsheetType, R.ROUTECODE, rt.name RouteName, rt2.RouteType, R.RIDERCODE SDOCODE, rider.firstName+rider.lastName as RiderName, b.sname as BranchName" + "\n"
                                + "  , CASE  WHEN ISNULL(RC.COD, '0') = '0' THEN COUNT(RC.CONSIGNMENTNUMBER) END  CNCOUNT, CASE WHEN ISNULL(RC.COD, '0') = '1' THEN COUNT(RC.CONSIGNMENTNUMBER) END   CODCNCOUNT" + "\n"
                                 + " FROM RUNSHEET R INNER JOIN RUNSHEETCONSIGNMENT RC ON  R.RUNSHEETNUMBER = RC.RUNSHEETNUMBER AND R.BRANCHCODE = RC.BRANCHCODE AND R.ROUTECODE = RC.ROUTECODE" + "\n"
                                + "  INNER JOIN routes rt ON  rc.RouteCode = rt.routeCode AND rc.branchcode = rt.BID LEFT JOIN RouteType rt2 ON rt.RouteType = rt2.Id INNER JOIN Lookup l ON  r.runsheetType = l.id" + "\n"
                                + "  INNER JOIN Riders rider on rider.ridercode = R.RIDERCODE AND rider.branchId = r.branchCode" + "\n"
                                 + " INNER JOIN Branches b on b.branchCode = rider.branchId" + "\n"
                                + "  WHERE  R.RUNSHEETDATE  >= '" + date + "' and R.RIDERCODE = '" + sdocdoe + "' AND RC.BRANCHCODE = '" + Session["BRANCHCODE"] + "'" + "\n"
                                + "  GROUP BY R.RUNSHEETNUMBER, R.RUNSHEETDATE, l.code, R.ROUTECODE, rt.name, rt2.RouteType, R.RIDERCODE, RC.COD, rider.firstName, b.sname,rider.lastName) X" + "\n"
                                + "           GROUP BY RUNSHEETNUMBER, RUNSHEETDATE, ROUTECODE, RouteName, ISNULL(RouteType, ''), RunsheetType, SDOCODE, RiderName, BranchName ORDER BY 1,5";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return ds;
        }
        public DataTable getRunsheetDetail(string runsheet)
        {
            DataTable ds = new DataTable();
            try
            {

                String query = "SELECT RUNSHEETNUMBER,RUNSHEETDATE, ROUTECODE, SDOCODE,ISNULL(SUM(CNCOUNT),'0') CNCOUNT,ISNULL(SUM(CODCNCOUNT),'0') CODCNCOUNT " + "\n"
                     + " FROM(SELECT R.RUNSHEETNUMBER, CONVERT(VARCHAR(10), R.RUNSHEETDATE, 105) RUNSHEETDATE, R.ROUTECODE, R.RIDERCODE SDOCODE, CASE WHEN ISNULL(RC.COD, '0') = '0' THEN COUNT(RC.CONSIGNMENTNUMBER) END CNCOUNT, " + "\n"
                      + " CASE WHEN ISNULL(RC.COD, '0') = '1' THEN COUNT(RC.CONSIGNMENTNUMBER) END CODCNCOUNT FROM " + "\n"
                     + " RUNSHEET R INNER JOIN RUNSHEETCONSIGNMENT RC ON R.RUNSHEETNUMBER = RC.RUNSHEETNUMBER AND R.BRANCHCODE = RC.BRANCHCODE AND R.ROUTECODE = RC.ROUTECODE " + "\n"
                     + " WHERE R.runsheetNumber = '" + runsheet + "'  AND RC.BRANCHCODE = '4'  GROUP BY R.RUNSHEETNUMBER, R.RUNSHEETDATE, R.ROUTECODE, R.RIDERCODE, RC.COD) X GROUP BY RUNSHEETNUMBER, RUNSHEETDATE, ROUTECODE, SDOCODE ORDER BY 1,5 ";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return ds;
        }
        public DataSet getConsignmentDetail(string consignment, string runsheet)
        {
            DataSet ds = new DataSet();
            try
            {

                String query = "select rc.consignmentNumber,rc.cod,rc.RouteCode from RunsheetConsignment rc where rc.consignmentNumber = '" + consignment + "' and rc.runsheetNumber = '" + runsheet + "'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return ds;
        }
        public DataSet getConsignmentDetail(string runsheet)
        {
            DataSet ds = new DataSet();
            try
            {

                String query = "select rc.consignmentNumber,rc.runsheetNumber,rc.cod as ISCOD from RunsheetConsignment rc where  rc.runsheetNumber = '" + runsheet + "'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return ds;
        }
        protected void dd_CN_No_TextChanged(object sender, EventArgs e)
        {
            #region old code
            //if (dd_CN_No.Text.Length == 12)
            //{
            //    clvar = new Variable();
            //    DataTable dt_head = Session["dt_cons"] as DataTable;
            //    clvar.RunsheetNumber = "201913379550";
            //    clvar.ConsignmentNo = dd_CN_No.Text;
            //    DataSet ds = getConsignmentDetail(clvar.ConsignmentNo, clvar.RunsheetNumber);// b_fun.Get_MNPSecurityScanD(clvar);
            //    string securityScanID, ConsignmentNumber, ISCOD, ColorLabel, Remarks;
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        securityScanID = "1";///Session["SecurityID"].ToString();
            //        ConsignmentNumber = dd_CN_No.Text;
            //        ISCOD = ds.Tables[0].Rows[0]["COD"].ToString();
            //        ColorLabel = "1";
            //        Remarks = "CN Matched Okay";

            //    }
            //    else
            //    {
            //        securityScanID = "1";//Session["SecurityID"].ToString();
            //        ConsignmentNumber = dd_CN_No.Text;
            //        ISCOD = "0";
            //        ColorLabel = "2";
            //        Remarks = "No CN Matched";

            //    }


            //    string find = "ConsignmentNumber = '"+ dd_CN_No.Text  +"'";
            //    if (dt_head.Rows.Count > 0)
            //    {                
            //        DataRow[] foundRows = dt_head.Select(find);               

            //        if (foundRows.Length == 0)
            //        {
            //            dt_head.Rows.Add(securityScanID, ConsignmentNumber, ISCOD, ColorLabel, Remarks);
            //            dt_head.AcceptChanges();
            //            Session["dt_cons"] = dt_head;
            //            //GridView.DataSource = dt_head;
            //            //GridView.DataBind();
            //            //You have it ...
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
            //              "alert('Consignment already Exists');", true);
            //        }
            //    }
            //    else
            //    {
            //        dt_head.Rows.Add(securityScanID, ConsignmentNumber, ISCOD, ColorLabel, Remarks);
            //        dt_head.AcceptChanges();
            //        Session["dt_cons"] = dt_head;
            //        //GridView.DataSource = dt_head;
            //        //GridView.DataBind();
            //    }
            //}
            #endregion

            pnl_lbl.Controls.Clear();
            dd_CN_No.Text = dd_CN_No.Text.Trim();
            DataTable dt = Session["consignment"] as DataTable;
            DataTable dt_codcon = Session["cod_con"] as DataTable;
            DataTable dt_runsheet = Session["runsheet"] as DataTable;
            if (!(dt_runsheet.Rows.Count > 0))
            {
                dd_CN_No.Text = "";
                //dd_RunSheetNo.Text = "";
                dd_R_NO.Text = "";
                dd_CN_No.Focus();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                    "alert('No Runsheet Searched');", true);
                return;
            }
            for (int a = 0; a < dt.Rows.Count; a++)
            {
                if (dd_CN_No.Text == dt.Rows[a]["ConsignmentNumber"].ToString())
                {
                    Label lbl = new Label();
                    lbl.Text = dt.Rows[a]["ConsignmentNumber"].ToString();
                    lbl.CssClass = "GreenLabel consignment-success";
                    pnl_lbl.Controls.Add(lbl);
                    Session["last"] = lbl;
                    Bindchild();
                    bindcod();
                    dd_CN_No.Text = "";
                    //dd_RunSheetNo.Text = "";
                    dd_R_NO.Text = "";
                    dd_CN_No.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                    "alert('Consignment already inserted');", true);
                    return;
                }
            }

            bool found = false;
            if (dd_CN_No.Text.Length >= 8 && dd_CN_No.Text.Length <= 18 && Regex.IsMatch(dd_CN_No.Text, @"^[0-9]+$"))
            {
                DataTable dt_head = Session["dt_cons"] as DataTable;

                if (dt_head.Rows.Count > 0)
                {

                    int i = 0;
                    for (i = 0; i < dt_head.Rows.Count; i++)
                    {
                        if (dt_head.Rows[i]["ConsignmentNumber"].ToString() == dd_CN_No.Text)
                        {


                            for (int j = 0; j < dt_runsheet.Rows.Count; j++)
                            {
                                if (dt_runsheet.Rows[j]["RUNSHEETNUMBER"].ToString() == dt_head.Rows[i]["RunsheetNumber"].ToString())
                                {
                                    if (dt_runsheet.Rows[j]["ColorLabel"].ToString() == "1")
                                    {
                                        dt.Rows.Add(dt_head.Rows[i]["ConsignmentNumber"], dt_head.Rows[i]["RunsheetNumber"], dt_head.Rows[i]["ISCOD"], "1", "");
                                        dt_head.Rows[i]["ColorLabel"] = "1";
                                        Label lbl = new Label();
                                        lbl.Text = dt_head.Rows[i]["ConsignmentNumber"].ToString();
                                        lbl.CssClass = "GreenLabel consignment-success";
                                        pnl_lbl.Controls.Add(lbl);
                                        Session["last"] = lbl;
                                        //lbl_msg01.Text = dt_head.Rows[i]["ConsignmentNumber"].ToString() + "added";
                                        if (dt_head.Rows[i]["ISCOD"].ToString() == "1")
                                        {
                                            for (int k = 0; k < dt_codcon.Rows.Count; k++)
                                            {
                                                if (dt_codcon.Rows[k]["ConsignmentNumber"] == dt_head.Rows[i]["ConsignmentNumber"])
                                                {
                                                    dt_head.Rows[i]["ColorLabel"] = "1";
                                                    dt_codcon.Rows[k]["ColorLabel"] = "1";
                                                    //lbl = new Label();
                                                    //lbl.Text = dt_codcon.Rows[k]["ConsignmentNumber"].ToString();
                                                    //lbl.CssClass = "GreenLabel consignment-success";
                                                    //Session["last"] = lbl;
                                                    //pnl_lbl.Controls.Add(lbl);
                                                    //lbl_msg01.Text = dt_codcon.Rows[k]["ConsignmentNumber"].ToString() + "added";
                                                    break;
                                                }
                                            }
                                        }
                                        found = true;
                                        break;
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                            "alert('Runsheet not scanned');", true);
                                        found = true;
                                    }
                                }
                            }
                            if (found == true)
                                break;
                        }

                    }
                }
                if (found == false)
                {
                    Label lbl = new Label();
                    lbl.Text = dd_CN_No.Text.ToString();
                    lbl.CssClass = "GreenLabel consignment-danger";
                    Session["last"] = lbl;
                    pnl_lbl.Controls.Add(lbl);
                    //lbl_msg01.Text = dd_CN_No.Text.ToString() + "added";
                    dt.Rows.Add(dd_CN_No.Text, " ", 0, "2", "");
                }
                Session["consignment"] = dt;
                dt_head.AcceptChanges();

            }
            else
            {
                Label lbl = new Label();
                lbl.Text = dd_CN_No.Text.ToString();
                lbl.CssClass = "GreenLabel consignment-danger";
                Session["last"] = lbl;
                pnl_lbl.Controls.Add(lbl);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                            "alert('Invalid Consignment Number');", true);
            }
            Bindchild();
            dd_CN_No.Text = "";
            //dd_RunSheetNo.Text = "";
            dd_R_NO.Text = "";
            dd_CN_No.Focus();
            Session["cod_con"] = dt_codcon;
            bindcod();
            //dd_start_date.Text = DateTime.Parse(DateTime.Now.Date.ToString()).ToString("yyyy-MM-dd");
        }
        protected void Bindchild()
        {
            child_panel.Controls.Clear();
            child_head.Visible = false;
            child_panel.Visible = false;
            child_panel2.Visible = false;
            DataTable dt = Session["consignment"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    child_head.Visible = true;
                    child_panel.Visible = true;
                    child_panel2.Visible = true;
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["ColorLabel"].ToString() == "1")
                        {
                            if (dt.Rows[k]["ISCOD"].ToString() != "1")
                            {
                                Label lbl = new Label();
                                lbl.Text = dt.Rows[k]["ConsignmentNumber"].ToString();
                                lbl.CssClass = "GreenLabel  consignment-success ";
                                child_panel.Controls.Add(lbl);
                                //lbl_msg01 = lbl;

                                //pnl_lbl.Controls.Add(lbl);

                            }
                        }
                        else if (dt.Rows[k]["ColorLabel"].ToString() == "2")
                        {
                            Label lbl = new Label();
                            lbl.Text = dt.Rows[k]["ConsignmentNumber"].ToString();
                            lbl.CssClass = "GreenLabel  consignment-danger ";
                            child_panel.Controls.Add(lbl);
                            //lbl_msg01 = lbl;
                            //pnl_lbl.Controls.Add(lbl);
                        }
                    }
                }

            }

        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkid");

                if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "ColorLabel")) == 1)
                {
                    //e.Row.BackColor = System.Drawing.Color.ForestGreen;
                    chk.Checked = true;
                    e.Row.Attributes["style"] = "background-color: #00A651";

                }
                else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "ColorLabel")) == 2)
                {
                    //e.Row.BackColor = System.Drawing.Color.Red;
                    chk.Checked = false;
                    e.Row.Attributes["style"] = "background-color: #CC2424";
                }


            }

        }
        public int id;
        protected void btnProceed_Click(object sender, EventArgs e)
        {
            //lbl_sdo.Visible = false;
            //lbl_sdoname.Visible = false;
            //lbl_branch.Visible = false;
            //lbl_branchname.Visible = false;
            bool flag_runsheet = false;
            bool flag_consignment = false;

            DataTable dt_runsheet = Session["runsheet"] as DataTable;
            DataTable dt_consignmnet = Session["consignment"] as DataTable;
            if (dt_runsheet.Rows.Count > 0)
            {

                try
                {
                    DataTable dt = new DataTable();
                    //String query = "select case when max(xb.Id) is null then 1 else max(xb.Id)+1 end as ID from (select row_number() over (order by SecurityScanId) as Id from MNP_RunsheetSecurityScan group by SecurityScanId)as xb";
                    String query = "select max(securityscanId)+1 as Id from MNP_RunsheetSecurityScan";

                    SqlConnection orcl = new SqlConnection(clvar.Strcon());
                    orcl.Open();
                    SqlCommand orcd = new SqlCommand(query, orcl);
                    orcd.CommandType = CommandType.Text;
                    SqlDataAdapter oda = new SqlDataAdapter(orcd);
                    oda.Fill(dt);
                    orcl.Close();
                    id = Int32.Parse(dt.Rows[0]["ID"].ToString());

                    for (int i = 0; i < dt_runsheet.Rows.Count; i++)
                    {
                        if (dt_runsheet.Rows[i]["ColorLabel"].ToString() == "1")
                        {

                            query = "Insert into MNP_RunsheetSecurityScan (SecurityScanId,RunSheetNumber,riderCode,RouteCode,RunsheetDate,ZoneCode,branchId,CNCount,CODCNCount,CreatedBy,RiderTimeOut)  " + "\n"
                           + "Values ('" + id + "', '" + dt_runsheet.Rows[i]["RUNSHEETNUMBER"].ToString() + "', '" + dt_runsheet.Rows[i]["SDOCode"].ToString() + "','" + dt_runsheet.Rows[i]["ROUTECODE"].ToString() + "','" + DateTime.ParseExact(dt_runsheet.Rows[i]["RUNSHEETDATE"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "'," + "\n"
                            + " '" + Session["ZONECODE"].ToString() + "' , '" + Session["BRANCHCODE"].ToString() + "','" + dt_runsheet.Rows[i]["CNCOUNT"].ToString() + "' , '" + dt_runsheet.Rows[i]["CODCNCOUNT"].ToString() + "','" + Session["U_ID"].ToString() + "',getdate())";
                            orcl = new SqlConnection(clvar.Strcon());
                            orcl.Open();
                            orcd = new SqlCommand(query, orcl);
                            orcd.CommandType = CommandType.Text;
                            oda = new SqlDataAdapter(orcd);
                            orcd.ExecuteNonQuery();
                            orcl.Close();
                            flag_runsheet = true;
                        }


                    }
                    if (dt_consignmnet.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_consignmnet.Rows.Count; i++)
                        {
                            query = "Insert into MNP_RunsheetSecurityScan_Detail (SecurityScanId,RunsheetNumber,ConsignmentNumber,CNColorLabel,CreatedBy,CreatedOn,ISCOD)  " + "\n"
                               + "Values ('" + id + "','" + dt_consignmnet.Rows[i]["RunsheetNumber"].ToString() + "', '" + dt_consignmnet.Rows[i]["ConsignmentNumber"].ToString() + "','" + dt_consignmnet.Rows[i]["ColorLabel"].ToString() + "','" + Session["U_ID"].ToString() + "'," + "\n"
                               + " getdate(),'" + dt_consignmnet.Rows[i]["ISCOD"].ToString() + "')";
                            orcl = new SqlConnection(clvar.Strcon());
                            orcl.Open();
                            orcd = new SqlCommand(query, orcl);
                            orcd.CommandType = CommandType.Text;
                            oda = new SqlDataAdapter(orcd);
                            orcd.ExecuteNonQuery();
                            orcl.Close();
                            flag_consignment = true;

                        }

                        //data = Session[]


                    }
                    binddata();
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                    //                       "alert('Runsheet and consignments added successfully');", true);
                    if (flag_runsheet == true || flag_consignment == true)
                    {
                        lbl_msg.Visible = true;
                        lbl_msg.Text = "Runsheet and consignments added successfully.TimeOut:";
                        innerbody.Visible = false;
                        body2.Visible = true;
                        dd_RunSheetNo.Enabled = false;
                        dd_R_NO.Enabled = false;
                        dd_CN_No.Enabled = false;
                        submit.Enabled = false;
                        //Page.Response.Redirect(Page.Request.Url.ToString(), true);
                        //DataTable data01 = getData(id.ToString());
                        //gvw_data.DataSource = data01;
                        //gvw_data.DataBind();
                        //DataTable dt_runsheet = Session["runsheet"] as DataTable;
                        string runsheet = null;
                        for (int i = 0; i < dt_runsheet.Rows.Count; i++)
                        {
                            if (dt_runsheet.Rows[i]["ColorLabel"].ToString() == "1")
                            {
                                if (runsheet != null)
                                    runsheet += ",'" + dt_runsheet.Rows[i]["RUNSHEETNUMBER"].ToString().Trim() + "'";
                                else
                                    runsheet += "'" + dt_runsheet.Rows[i]["RUNSHEETNUMBER"].ToString().Trim() + "'";
                                //if (i != dt_runsheet.Rows.Count - 1)
                                //{
                                //    runsheet += ",";
                                //}
                            }
                        }

                        DataTable data01 = getRunsheetData(runsheet, Session["Date"].ToString(), Session["SDO"].ToString());
                        gvw_runsheet.DataSource = data01;
                        gvw_runsheet.DataBind();
                        data01 = getSecurityData(id.ToString());
                        lbl_time.Text = (Convert.ToDateTime(data01.Rows[0]["RiderTimeOut"].ToString())).ToString("yyyy-M-dd hh:mm");
                        gvw_security.DataSource = data01;
                        gvw_security.DataBind();
                        gvw_runsheet.Visible = true;
                        //gvw_data.Visible = true;
                        gvw_security.Visible = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                              "alert('No Runsheet and Consignment Scanned');", true);
                    }


                }
                catch (Exception Err)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                                    "alert('Error Inserting Data. Contact IT.');", true);


                }
                finally
                {


                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                              "alert('No Runsheet Found');", true);
            }

            Bindchild();
            if (lbl_msg.Visible == true)
            {
                //refresh();
            }
            //if (flag_runsheet == false && flag_consignment == false)
            //{
            //    Errorid.Text = "No Runsheet and Consignment Scanned";
            //    Errorid.Visible = true;
            //    refresh();

            //}
            //if (flag_runsheet == false)
            //{
            //    Errorid.Text = "No Runsheet Scanned";
            //    Errorid.Visible = true;
            //    refresh();

            //}
            //if (flag_consignment == false)
            //{
            //    Errorid.Text = "No Consignment Scanned";
            //    Errorid.Visible = true;
            //    refresh();
            //}

        }
        public void binddata()
        {
            //DataTable data = new DataTable();
            DataTable data_run = Session["runsheet"] as DataTable;
            DataTable data = Session["consignment"] as DataTable;
            if (data != null)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (data.Rows[i]["ColorLabel"].ToString() == "2")
                    {
                        Label lbl = new Label();
                        lbl.Text = data.Rows[i]["ConsignmentNumber"].ToString();
                        lbl.CssClass = "GreenLabel  consignment-danger ";
                        pnl_miss.Controls.Add(lbl);
                    }

                }
                data = Session["dt_cons"] as DataTable;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (data.Rows[i]["ColorLabel"].ToString() == "0")
                    {
                        for (int k = 0; k < data_run.Rows.Count; k++)
                        {
                            if (data.Rows[i]["RunsheetNumber"].ToString() == data_run.Rows[k]["RunsheetNumber"].ToString())
                            {
                                if (data_run.Rows[k]["ColorLabel"].ToString() == "1")
                                {
                                    Label lbl = new Label();
                                    lbl.Text = data.Rows[i]["ConsignmentNumber"].ToString();
                                    lbl.CssClass = "GreenLabel  consignment-remain ";
                                    pnl_remain.Controls.Add(lbl);
                                    //break;
                                }

                            }

                        }

                    }

                }
            }

        }
        public void refresh()
        {
            lbl_sdo.Visible = false;
            lbl_sdoname.Visible = false;
            lbl_branch.Visible = false;
            lbl_branchname.Visible = false;
            body1.Visible = true;
            body2.Visible = false;
            dd_RunSheetNo.Enabled = true;
            dd_R_NO.Enabled = true;
            dd_CN_No.Enabled = true;
            submit.Enabled = true;
            innerbody.Visible = true;
            Session["runsheet"] = null;
            Session["consignment"] = null;
            Session["dt_cons"] = null;
            Session["cod_con"] = null;
            Session["last"] = null;

            DataTable dt_cons = new DataTable();
            dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
            dt_cons.Columns.Add("RunsheetNumber", typeof(string));
            dt_cons.Columns.Add("ColorLabel", typeof(string));
            dt_cons.Columns.Add("Remarks", typeof(string));

            Session["consignment"] = dt_cons;


            DataTable dt_Runsheet = new DataTable();
            dt_Runsheet.Columns.Add("RUNSHEETNUMBER", typeof(int));
            dt_Runsheet.Columns.Add("RUNSHEETDATE", typeof(DateTime));
            dt_Runsheet.Columns.Add("ROUTECODE", typeof(string));
            dt_Runsheet.Columns.Add("CNCOUNT", typeof(int));
            dt_Runsheet.Columns.Add("CODCNCOUNT", typeof(int));
            dt_Runsheet.Columns.Add("ColorLabel", typeof(string));
            dt_Runsheet.Columns.Add("Remarks", typeof(string));
            Session["runsheet"] = dt_Runsheet;

            dt_cons = new DataTable();
            dt_cons.Columns.Add("RunsheetNumber", typeof(string));
            dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
            dt_cons.Columns.Add("ISCOD", typeof(int));
            dt_cons.Columns.Add("ColorLabel", typeof(string));
            //dt_cons.Columns.Add("Remarks", typeof(string));

            Session["dt_cons"] = dt_cons;

            dt_cons = new DataTable();
            dt_cons.Columns.Add("ConsignmentNumber", typeof(string));
            dt_cons.Columns.Add("ColorLabel", typeof(string));

            Session["cod_con"] = dt_cons;

            GridView1.DataSource = Session["runsheet"];
            GridView1.DataBind();


            Bindchild();
            dd_CN_No.Text = "";
            dd_RunSheetNo.Text = "";
            dd_R_NO.Text = "";
            //date-change
            dd_start_date.Text = DateTime.Parse(DateTime.Now.Date.ToString()).ToString("yyyy-MM-dd");

        }
        protected void dd_R_NO_TextChanged(object sender, EventArgs e)
        {
            child_panel2.Visible = true;
            Errorid.Visible = false;
            dd_R_NO.Text = dd_R_NO.Text.Trim();
            DataTable dt_head = Session["runsheet"] as DataTable;
            DataTable dt_cons = Session["dt_cons"] as DataTable;
            DataTable dt_codcon = Session["cod_con"] as DataTable;
            if (dd_R_NO.Text.Length >= 8 && dd_R_NO.Text.Length <= 18 && Regex.IsMatch(dd_R_NO.Text, @"^[0-9]+$"))
            {
                if (dt_head.Rows.Count > 0)
                {
                    int i = 0;
                    for (i = 0; i < dt_head.Rows.Count; i++)
                    {
                        if (dt_head.Rows[i]["RUNSHEETNUMBER"].ToString() == dd_R_NO.Text && dt_head.Rows[i]["ColorLabel"].ToString() == "0")
                        {
                            dt_head.Rows[i]["ColorLabel"] = "1";
                            dt_head.Rows[i]["Remarks"] = "";
                            for (int j = 0; j < dt_cons.Rows.Count; j++)
                            {
                                if (dt_cons.Rows[j]["RunsheetNumber"].ToString() == dt_head.Rows[i]["RUNSHEETNUMBER"].ToString())
                                {
                                    if (dt_cons.Rows[j]["ISCOD"].ToString() == "1")
                                    {
                                        dt_codcon.Rows.Add(dt_cons.Rows[j]["ConsignmentNumber"], "0");
                                    }
                                }

                            }
                            break;

                        }
                    }
                    Session["runsheet"] = dt_head;
                    GridView1.DataSource = dt_head;
                    GridView1.DataBind();

                    if (i == dt_head.Rows.Count)
                    {
                        DataTable dt = Session["dt_cons"] as DataTable;
                        DataTable ds = getRunsheetDetail(dd_R_NO.Text);
                        if (ds != null && ds.Rows.Count > 0)
                        {
                            //Errorid.Text = "Runsheet valid but not ";
                            //Errorid.Visible = true;
                            //   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                            //"alert('Runsheet not found');", true);
                            //dt_head.Rows.Add(ds.Rows[0]["RUNSHEETNUMBER"], ds.Rows[0]["RUNSHEETDATE"], ds.Rows[0]["ROUTECODE"], ds.Rows[0]["SDOCODE"], Int32.Parse(ds.Rows[0]["CNCOUNT"].ToString()), Int32.Parse(ds.Rows[0]["CODCNCOUNT"].ToString()), "2"," ");
                            //DataSet ds_con = getConsignmentDetail(ds.Rows[0]["RUNSHEETNUMBER"].ToString());
                            //System.Data.DataColumn newColumncons = new System.Data.DataColumn("ColorLabel", typeof(string));
                            //newColumncons.DefaultValue = "0";
                            //ds_con.Tables[0].Columns.Add(newColumncons);
                            //newColumncons = new System.Data.DataColumn("Remarks", typeof(string));
                            //newColumncons.DefaultValue = " ";
                            //ds_con.Tables[0].Columns.Add(newColumncons);

                            //Session["dt_cons"] = dt;

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                          "alert('Runsheet not found');", true);
                            //Errorid.Text = "Runsheet not found";
                            //Errorid.Visible = true;
                        }
                    }


                }
                else
                {
                    //dt_runsheet.Rows.Add(1, dd_R_NO.Text, "2", "No Runsheet Matched");
                    // SCannned but not searched
                    DataTable ds = getRunsheetDetail(dd_R_NO.Text);
                    //dt_head.Rows.Add(ds.Rows[0]["RUNSHEETNUMBER"], ds.Rows[0]["RUNSHEETDATE"], ds.Rows[0]["ROUTECODE"], ds.Rows[0]["SDOCODE"], ds.Rows[0]["CNCOUNT"], ds.Rows[0]["CODCNCOUNT"],"2");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                         "alert('No Runsheet searched');", true);
                    //Errorid.Text = "No Runsheet searched";
                    //Errorid.Visible = true;
                }
                dt_head.AcceptChanges();
                Session["runsheet"] = dt_head;
                //GridView1.DataSource = dt_head;


            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError",
                         "alert('Invalid Runsheet Number');", true);
                //Errorid.Text = "Invalid Runsheet Number";
                //Errorid.Visible = true;
            }
            DataTable dt_child = Session["consignment"] as DataTable;
            if (dt_child.Rows.Count > 0)
            {
                Bindchild();
            }
            dd_CN_No.Text = "";
            //dd_RunSheetNo.Text = "";
            dd_R_NO.Text = "";
            //dd_start_date.Text = DateTime.Parse(DateTime.Now.Date.ToString()).ToString("yyyy-MM-dd");
            dd_R_NO.Focus();
            Session["cod_con"] = dt_codcon;
            bindcod();
        }
        protected void bindcod()
        {
            child_panel2.Controls.Clear();

            child_head.Visible = true;
            child_panel2.Visible = true;
            DataTable dt_cons = Session["cod_con"] as DataTable;
            if (dt_cons != null)
            {
                for (int j = 0; j < dt_cons.Rows.Count; j++)
                {
                    Label lbl = new Label();
                    lbl.Text = dt_cons.Rows[j]["ConsignmentNumber"].ToString();
                    if (dt_cons.Rows[j]["ColorLabel"].ToString() == "1")
                    {

                        lbl.CssClass = "GreenLabel consignment-success";

                    }
                    else
                    {
                        lbl.CssClass = "GreenLabel consignment-cod ";

                    }

                    child_panel2.Controls.Add(lbl);
                    // lbl_msg01 = lbl;

                    //pnl_lbl.Controls.Add(lbl);
                }
            }


        }
        protected void txt_comments_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
            DataTable dt_head = Session["runsheet"] as DataTable;
            dt_head.Rows[rowindex]["Remarks"] = tb.Text;
            Session["runsheet"] = dt_head;
            GridView1.DataSource = Session["runsheet"];
            GridView1.DataBind();

        }
        protected void GridViewChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (dd_CN_No.Text.Length > 12)
            {
                if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "ColorLabel")) == 1)
                {
                    e.Row.BackColor = System.Drawing.Color.Green;
                }
                else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "ColorLabel")) == 2)
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void txtchild_comments_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;
        }

        [WebMethod]
        public static void childcomments(string consignment, string value)
        {
            DataTable dt_head = HttpContext.Current.Session["dt_cons"] as DataTable;

            if (dt_head.Rows.Count > 0)
            {
                int i = 0;
                for (i = 0; i < dt_head.Rows.Count; i++)
                {
                    if (dt_head.Rows[i]["ConsignmentNumber"].ToString() == consignment)
                    {
                        dt_head.Rows[i]["Remarks"] = value;
                        break;
                    }
                }
            }
            HttpContext.Current.Session["dt_cons"] = dt_head;

        }
        public DataTable getData(string scanID)
        {
            DataTable dt = new DataTable();
            try
            {

                String query = "select xb.SecurityScanId,xb.TotalScannedRunsheet,xb.CODCount,xb.DomesticCount,xb.MissingRunsheet,xb.riderCode,rider.firstName as RiderName,xb.RouteCode,xb.date ,xb.Time from (" + "\n"
                                 + "select s.SecurityScanId,count(s.RunSheetNumber) as TotalScannedRunsheet, s.RouteCode, s.riderCode, s.branchId,CONVERT(VARCHAR(5), s.RiderTimeOut, 108) AS[Time],cast(s.RiderTimeOut as date) as date," + "\n"
                                 + "(select count(sd.ConsignmentNumber) from MNP_RunsheetSecurityScan_Detail sd where sd.SecurityScanId = '" + scanID + "' and sd.ISCOD = '1') as CODCount," + "\n"
                                 + "(select count(sd.ConsignmentNumber) from MNP_RunsheetSecurityScan_Detail sd where sd.SecurityScanId = '" + scanID + "' and sd.ISCOD = '0' and sd.RunsheetNumber != '0') as DomesticCount," + "\n"
                                 + "(select count(sd.ConsignmentNumber) from MNP_RunsheetSecurityScan_Detail sd where sd.SecurityScanId = '" + scanID + "' and sd.RunsheetNumber = '0') as MissingRunsheet" + "\n"
                                 + "from MNP_RunsheetSecurityScan s" + "\n"
                                 + "where s.SecurityScanId = '" + scanID + "'" + "\n"
                                 + "group by s.SecurityScanId, s.RouteCode, s.riderCode,s.branchId,CONVERT(VARCHAR(5), s.RiderTimeOut, 108),cast(s.RiderTimeOut as date)) as xb" + "\n"
                                + " left JOIN Riders rider on rider.ridercode = xb.RIDERCODE AND rider.branchId = xb.branchId ";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return dt;
        }
        public DataTable getRunsheetData(string runsheet, string date, string sdocode)
        {
            DataTable dt = new DataTable();
            try
            {

                String query = "SELECT RUNSHEETNUMBER,RUNSHEETDATE,RunsheetType,ROUTECODE,RouteName,ISNULL(RouteType ,'') RouteType,ISNULL(SUM(DOMCOUNT) ,'0') CNCOUNT,ISNULL(SUM(CODCOUNT) ,'0')CODCNCOUNT," + "\n"
                     + "RiderName, BranchName, case when runsheetNumber in (" + runsheet + ") then 'Yes' else 'No' end SecurityChecked" + "\n"
                     + "FROM(SELECT R.RUNSHEETNUMBER, CONVERT(VARCHAR(10), R.RUNSHEETDATE, 105) RUNSHEETDATE, l.code RunsheetType, R.ROUTECODE, rt.name RouteName, rt2.RouteType, R.RIDERCODE SDOCODE" + "\n"
                     + ", rider.firstName as RiderName, b.sname as BranchName, CASE  WHEN ISNULL(RC.COD, '0') = '0' THEN COUNT(RC.CONSIGNMENTNUMBER) END  DOMCOUNT, CASE WHEN ISNULL(RC.COD, '0') = '1' THEN COUNT(RC.CONSIGNMENTNUMBER)" + "\n"
                     + "END   CODCOUNT FROM RUNSHEET R INNER JOIN RUNSHEETCONSIGNMENT RC ON  R.RUNSHEETNUMBER = RC.RUNSHEETNUMBER AND R.BRANCHCODE = RC.BRANCHCODE AND R.ROUTECODE = RC.ROUTECODE" + "\n"
                     + " INNER JOIN routes rt ON  rc.RouteCode = rt.routeCode AND rc.branchcode = rt.BID LEFT JOIN RouteType rt2 ON rt.RouteType = rt2.Id INNER JOIN Lookup l ON  r.runsheetType = l.id" + "\n"
                     + "INNER JOIN Riders rider on rider.ridercode = R.RIDERCODE AND rider.branchId = r.branchCode INNER JOIN Branches b on b.branchCode = rider.branchId" + "\n"
                     + "WHERE  R.RUNSHEETDATE >= '" + date + "' and R.RIDERCODE = '" + sdocode + "' AND RC.BRANCHCODE = '4' GROUP BY R.RUNSHEETNUMBER, R.RUNSHEETDATE, l.code, R.ROUTECODE, rt.name, rt2.RouteType, R.RIDERCODE, RC.COD, rider.firstName, b.sname) X" + "\n"
                     + "GROUP BY RUNSHEETNUMBER, RUNSHEETDATE, ROUTECODE, RouteName, ISNULL(RouteType, ''), RunsheetType, SDOCODE, RiderName, BranchName ORDER BY 1,5";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return dt;
        }
        public DataTable getSecurityData(string scanID)
        {
            DataTable dt = new DataTable();
            try
            {
                String query = "Select x.SecurityScanId,x.RunSheetNumber,x.RiderTimeOut,cast(x.RunsheetDate as varchar) as RunsheetDate,x.RouteCode,x.riderCode as sdocode,X.TotalDOMCount, X.TotalCodCount,Sum(x.dom) as ScannedDOM,Sum(x.cod) as ScannedCOD," + "\n"
                                + " (X.TotalDOMCount - Sum(x.dom)) as RemainingDOM, (X.TotalCodCount - Sum(x.cod)) as RemainingCOD" + "\n"
                                + " from(select sd.ConsignmentNumber, s.RunSheetNumber,s.RiderTimeOut,s.SecurityScanId, s.RunsheetDate, s.RouteCode, s.riderCode, s.CNCount as TotalDOMCount, s.CODCNCount as TotalCodCount, " + "\n"
                                + " case when sd.ISCOD = '0' then 1 else 0 end dom,case when sd.ISCOD = '1' then 1 else 0 end cod from MNP_RunsheetSecurityScan s" + "\n"
                                + "left join MNP_RunsheetSecurityScan_Detail sd on sd.SecurityScanId = s.SecurityScanId and sd.RunsheetNumber = s.RunSheetNumber where s.SecurityScanId = '" + scanID + "'" + "\n"
                                + " group by sd.ConsignmentNumber,sd.ISCOD,s.RunSheetNumber,s.SecurityScanId,s.RunsheetDate ,s.RouteCode,s.CNCount ,s.CODCNCount,s.riderCode,s.RiderTimeOut) as X" + "\n"
                                + " group by x.SecurityScanId,x.RunSheetNumber,x.RunsheetDate,x.RouteCode,x.riderCode , X.TotalCodCount,X.TotalDOMCount,x.RiderTimeOut order by RunSheetNumber";

                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return dt;
        }
        public DataTable getConsignmentData(string scanID)
        {
            DataTable dt = new DataTable();
            try
            {

                String query = "select sd.ConsignmentNumber from  MNP_RunsheetSecurityScan_Detail sd where sd.SecurityScanId = '" + scanID + "' and sd.RunsheetNumber = '0'";


                SqlConnection orcl = new SqlConnection(clvar.Strcon());

                orcl.Open();
                SqlCommand orcd = new SqlCommand(query, orcl);

                orcd.CommandType = CommandType.Text;

                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception Err)
            {
                return null;
            }
            finally
            { }
            return dt;
        }
        protected void chkid_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow Row = ((GridViewRow)((Control)sender).Parent.Parent);
            int index = Row.RowIndex;
            CheckBox chkcheck = (CheckBox)Row.FindControl("chkid");
            DataTable dt_head = Session["runsheet"] as DataTable;
            DataTable dt_cons = Session["dt_cons"] as DataTable;
            DataTable dt_codcon = Session["cod_con"] as DataTable;

            //if (chkcheck.Checked)
            //{


            //}
            //else
            //{
            //    chkcheck.Checked = false;
            //    dt_head.Rows[index]["ColorLabel"] = "0";
            //    dt_head.Rows[index]["Remarks"] = "";
            //}

            for (int j = 0; j < dt_cons.Rows.Count; j++)
            {
                if (dt_cons.Rows[j]["RunsheetNumber"].ToString() == dt_head.Rows[index]["RUNSHEETNUMBER"].ToString())
                {
                    if (dt_cons.Rows[j]["ISCOD"].ToString() == "1" && dt_head.Rows[index]["ColorLabel"].ToString() == "0")
                    {
                        dt_codcon.Rows.Add(dt_cons.Rows[j]["ConsignmentNumber"], "0");
                    }
                }

            }
            chkcheck.Checked = true;
            dt_head.Rows[index]["ColorLabel"] = "1";
            dt_head.Rows[index]["Remarks"] = "";
            Session["runsheet"] = dt_head;
            GridView1.DataSource = dt_head;
            GridView1.DataBind();
            Bindchild();


            Session["cod_con"] = dt_codcon;
            bindcod();
        }

        protected void btn_refresh_Click(object sender, EventArgs e)
        {
            refresh();
        }


    }
}