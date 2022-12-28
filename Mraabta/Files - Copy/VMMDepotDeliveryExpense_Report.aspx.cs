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
using System.Drawing;

namespace MRaabta.Files
{
    public partial class VMMDepotDeliveryExpense_Report : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["U_NAME"] == null)
            {
                Response.Redirect("~/login");
            }

            if (!IsPostBack)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Errorid.Text = "";

                GetZone();
                //GetVehicle();
            }

            gvwData.Visible = false;


        }

        public DataSet Get_VehicleListByID(string zone)
        {
            // string query = "SELECT v.id, v.NAME FROM Vehicle v where v.status = '1'";
            string query = "select * from vehicle where zonecode='" + zone + "' and status='1'";
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return ds;
        }

        protected void GetVehicle(string zone)
        {
            Errorid.Text = "";
            DataSet ds = new DataSet();
            ds = Get_VehicleListByID(zone);

            dd_van.Items.Clear();

            if (ds.Tables[0].Rows.Count != 0)
            {
                dd_van.DataTextField = "NAME";
                dd_van.DataValueField = "id";
                dd_van.DataSource = ds.Tables[0].DefaultView;
                dd_van.DataBind();
            }
            else
            {
                ds = null;
                dd_van.DataSource = ds;
                dd_van.DataBind();
                Errorid.Text = "No Vehicle Present.";
                dd_van.Items.Clear();
            }
            dd_van.Items.Insert(0, new ListItem("SELECT VEHICLE", "0"));
        }

        protected void GetZone()
        {
            string query = "SELECT DISTINCT z.zoneCode,\n" +
                      "z.name as zonename\n" +
               "FROM   Zones z\n" +
                      "INNER JOIN Branches b\n" +
                           "ON b.zoneCode = z.zoneCode\n" +
                           "AND b.[status] = '1'\n" +
               "WHERE z.[status] = '1' \n" +
               "ORDER BY z.name asc \n";

            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(query, con);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ddl_zone.Items.Insert(0, new ListItem(" Select Zone", "0"));
                    ddl_zone.DataTextField = "zonename";
                    ddl_zone.DataValueField = "zoneCode";
                    ddl_zone.DataSource = ds.Tables[0].DefaultView;
                    ddl_zone.DataBind();
                }

                ddl_zone.Items.Insert(0, new ListItem("SELECT ZONE", "0"));
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
        }
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string strFromDate = txt_fromdate.Text;
            string strToDate = txt_todate.Text;

            DateTime FromDate = DateTime.ParseExact(strFromDate, "dd/MM/yyyy", null);
            DateTime ToDate = DateTime.ParseExact(strToDate, "dd/MM/yyyy", null);


            int result = DateTime.Compare(FromDate, ToDate);

            if (result <= 0)
            {

                DataTable dt = new DataTable();
                dt = Get_ExpenseLogSheet();
                DataSet dvt = new DataSet();
                dvt.Tables.Add(dt);

                if (dt.Rows.Count > 0)
                {
                    gvwData.Visible = true;
                    Errorid.Text = "";
                    ViewState["LOG_SHEET"] = dt;
                    ViewState["hf_count"] = hf_gv_count.Value = dt.Columns.Count.ToString();
                    gvwData.DataSource = dt;
                    gvwData.DataBind();
                }
                else
                {
                    gvwData.Visible = false;
                    Errorid.Text = "NO DATA";
                    dvt.Tables[0].Rows.Add(dvt.Tables[0].NewRow());
                    gvwData.DataSource = dvt;
                    gvwData.DataBind();
                    int columncount = gvwData.Rows[0].Cells.Count;
                    gvwData.Rows[0].Cells.Clear();
                    gvwData.Rows[0].Cells.Add(new TableCell());
                    gvwData.Rows[0].Cells[0].ColumnSpan = columncount;
                    gvwData.Rows[0].Cells[0].Text = "No Records Found";

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('From and To Date Selection is Incorrect!');", true);
                return;
            }
        }
        public string Get_Expense_Amount()
        {
            string sqlString = "SELECT SUM(BILL_AMOUNT)\n" +
            "  FROM vmm_expense_sheet\n" +
            "WHERE edate >= '" + DateFormat(txt_fromdate.Text) + "' AND\n" +
            "       edate <= '" + DateFormat(txt_todate.Text) + "' \n";
            if (dd_van.SelectedValue != "0")
            {
                sqlString += "AND vehicle = '" + dd_van.SelectedValue + "' \n";
            }

            if (ddl_zone.SelectedValue != "0")
            {
                sqlString += "AND zone = '" + ddl_zone.SelectedValue + "' \n";
            }

            string TotalSize = "";
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(sqlString, con);
                TotalSize = orcd.ExecuteScalar().ToString();
                con.Close();

            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return TotalSize;
        }
        private string DateFormat(string methDate)
        {
            string mdate = DateTime.ParseExact(methDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            string[] mmarr = mdate.Split('/', ' ');
            return string.Concat(mmarr);
        }
        public DataTable Get_ExpenseLogSheet()
        {

            string sqlString = "SELECT vmm.eid,\n" +
            "convert(varchar,convert(datetime,vmm.edate,105),103) EXPENSE_DATE,\n" +
            //"R.firstName DSR,\n" +
            "V.name VEHICLE,\n" +
            "MMC.mmc_ldesc MMC,\n" +
            "MSC.msc_ldesc MSC,\n" +
            "vmm.workorder_amount workorder,\n" +
            "vmm.bill_no [NO.],\n" +
            "convert(varchar,convert(datetime,vmm.bill_date,105),103) [DATE],\n" +
            "vmm.bill_amount AMOUNT,\n" +
            "vmm.m_reading MTR_READING\n" +
            "from VMM_EXPENSE_SHEET vmm\n" +
            "INNER JOIN Vehicle v\n" +
            "ON VMM.vehicle = V.id\n" +
            "INNER JOIN VMM_MAINTENANCEMCATEGORY MMC\n" +
            "ON VMM.mmc_id = MMC.mmc_id\n" +
            "INNER JOIN VMM_MAINTENANCESCATEGORY MSC\n" +
            "ON VMM.msc_id = MSC.msc_id AND VMM.mmc_id = MSC.mmc_id\n" +
            "LEFT JOIN Zones z ON v.zoneCode = z.zoneCode \n" +
            //"INNER JOIN Riders R\n" +
            //"ON vmm.driver = R.riderCode\n" +
            //"AND '"+ HttpContext.Current.Session["BRANCHCODE"] +"' = R.branchId\n" +
            "WHERE vmm.edate >= '" + DateFormat(txt_fromdate.Text) + "' AND\n" +
            "vmm.edate <= '" + DateFormat(txt_todate.Text) + "'\n";
            if (dd_van.SelectedValue != "0")
            {
                sqlString += "AND v.id = '" + dd_van.SelectedValue + "' \n";
            }

            if (ddl_zone.SelectedValue != "0")
            {
                sqlString += "AND z.zoneCode = '" + ddl_zone.SelectedValue + "' \n";
            }
            sqlString += "ORDER BY VMM.edate,VMM.eid";


            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand orcd = new SqlCommand(sqlString, con);
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                con.Close();
            }
            catch (Exception Err)
            { }
            finally
            { con.Close(); }
            return dt;

        }
        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {

        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            try
            {
                if (gvwData.Rows.Count > 0)
                {
                    string distributor = "LAHORE";
                    //clvars.Date_From = DateTime.Parse(txt_fromdate.Text).ToString("dd/MM/yyyy");
                    //clvars.Date_to = DateTime.Parse(txt_todate.Text).ToString("dd/MM/yyyy");

                    GridView grid = sender as GridView;

                    if (grid != null)
                    {
                        GridViewRow Row1 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                        //Row1.BackColor = Color.White;
                        // Row1.ForeColor = ColorTranslator.FromHtml("#00427c");
                        TableCell HeaderR1 = new TableHeaderCell();
                        HeaderR1.Attributes.Add("Style", "color: font-size:11pt; font-style: sans-serif;");
                        int colCountR1 = grid.Rows[0].Cells.Count;
                        HeaderR1.ColumnSpan = colCountR1;
                        HeaderR1.Text = "Muller & Phipps Pakistan (Pvt.) Ltd.";
                        Row1.Cells.Add(HeaderR1);
                        Table R1Table = grid.Controls[0] as Table;
                        if (R1Table != null)
                        {
                            R1Table.Rows.AddAt(0, Row1);
                        }

                        GridViewRow Row2 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                        //  Row2.BackColor = Color.White;
                        //  Row2.ForeColor = ColorTranslator.FromHtml("#00427c");
                        Row2.BackColor = ColorTranslator.FromHtml("#454a51");
                        Row2.ForeColor = Color.White;
                        TableCell HeaderR2 = new TableHeaderCell();
                        HeaderR2.Attributes.Add("Style", "FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        int colCountR2 = grid.Rows[0].Cells.Count;
                        HeaderR2.ColumnSpan = colCountR2;
                        HeaderR2.Text = "Expense Sheet Report";
                        Row2.Cells.Add(HeaderR2);
                        Table R2Table = grid.Controls[0] as Table;
                        if (R2Table != null)
                        {
                            R2Table.Rows.AddAt(1, Row2);
                        }

                        GridViewRow Row3 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

                        Row3.BackColor = ColorTranslator.FromHtml("#454a51");
                        Row3.ForeColor = Color.White;
                        TableCell HeaderR4 = new TableHeaderCell();
                        HeaderR4.Attributes.Add("Style", "text-align:left, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        int colCountR4 = grid.Rows[0].Cells.Count - 10;
                        HeaderR4.ColumnSpan = colCountR4;
                        HeaderR4.Text = "Date From:<br>" + txt_fromdate.Text + " ";


                        TableCell HeaderR3 = new TableHeaderCell();
                        HeaderR3.Attributes.Add("Style", "FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        int colCountR3 = grid.Rows[0].Cells.Count - 2;
                        HeaderR3.ColumnSpan = colCountR3;
                        HeaderR3.Text = "";

                        TableCell HeaderR5 = new TableHeaderCell();
                        HeaderR5.Attributes.Add("Style", "text-align:left, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        int colCountR5 = grid.Rows[0].Cells.Count - 9;
                        HeaderR5.ColumnSpan = colCountR5;
                        HeaderR5.Text = "Date To:<br>" + txt_todate.Text + " ";

                        Row3.Cells.Add(HeaderR4);
                        Row3.Cells.Add(HeaderR3);
                        Row3.Cells.Add(HeaderR5);
                        Table R3Table = grid.Controls[0] as Table;
                        if (R3Table != null)
                        {
                            R3Table.Rows.AddAt(2, Row3);
                        }

                        GridViewRow Row4 = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                        Row4.BackColor = ColorTranslator.FromHtml("#ee701b");
                        Row4.ForeColor = Color.White;

                        TableCell HeaderR7 = new TableHeaderCell();
                        HeaderR7.Attributes.Add("Style", "text-align:center, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR7.ColumnSpan = 2;
                        HeaderR7.Text = "EXPENSE";

                        TableCell HeaderR10 = new TableHeaderCell();
                        HeaderR10.Attributes.Add("Style", "FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR10.ColumnSpan = 2;
                        HeaderR10.Text = "";

                        TableCell HeaderR6 = new TableHeaderCell();
                        HeaderR6.Attributes.Add("Style", "text-align:center, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR6.ColumnSpan = 2;
                        HeaderR6.Text = "MAINTENANCE";

                        TableCell HeaderR8 = new TableHeaderCell();
                        HeaderR8.Attributes.Add("Style", "text-align:center, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR8.ColumnSpan = 4;
                        HeaderR8.Text = "BILLs";

                        TableCell HeaderR9 = new TableHeaderCell();
                        HeaderR9.Attributes.Add("Style", "FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR9.ColumnSpan = 1;
                        HeaderR9.Text = "";


                        Row4.Cells.Add(HeaderR7);
                        Row4.Cells.Add(HeaderR10);
                        Row4.Cells.Add(HeaderR6);
                        Row4.Cells.Add(HeaderR8);
                        Row4.Cells.Add(HeaderR9);
                        Table R6Table = grid.Controls[0] as Table;
                        if (R6Table != null)
                        {
                            R6Table.Rows.AddAt(3, Row4);
                        }

                    }

                    DataTable dt = (DataTable)ViewState["LOG_SHEET"];
                    GridViewRow row_total = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
                    row_total.BackColor = ColorTranslator.FromHtml("#454a51");
                    row_total.Attributes.Add("Style", " font-size: x-small;font-family: Calibri;");
                    row_total.ForeColor = Color.White;
                    row_total.Font.Bold = true;
                    string total = Get_Expense_Amount();
                    if (string.IsNullOrEmpty(total) || string.IsNullOrWhiteSpace(total) || total == "NULL")
                    {
                        total = "0Rs";
                    }
                    if (dt != null)
                    {
                        row_total.Cells.AddRange(new TableCell[] {
                                                new TableCell { Text = "TOTAL BILL(s) AMOUNT", HorizontalAlign = HorizontalAlign.Left,ColumnSpan=7},
                                                new TableCell { Text = total , HorizontalAlign = HorizontalAlign.Center},
                                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                                             });
                        gvwData.Controls[0].Controls.Add(row_total);

                    }

                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void ddl_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetVehicle(ddl_zone.SelectedValue);
        }
    }
}