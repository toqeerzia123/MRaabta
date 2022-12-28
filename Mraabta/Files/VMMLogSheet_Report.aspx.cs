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
    public partial class VMMLogSheet_Report : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        //  Cl_VmmVehicleLog clveh = new Cl_VmmVehicleLog();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // GetVehicle();

                GetZone();

                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
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
                dt = Get_VehicleLogSheet();
                DataSet dvt = new DataSet();
                dvt.Tables.Add(dt);

                if (dt.Rows.Count > 0)
                {
                    Errorid.Text = "";
                    ViewState["LOG_SHEET"] = dt;
                    ViewState["hf_count"] = hf_gv_count.Value = dt.Columns.Count.ToString();
                    gvwData.DataSource = dt;
                    gvwData.DataBind();
                }
                else
                {
                    Errorid.Text = "NO DATA";
                    //gvwData.DataSource = null;
                    // gvwData.DataBind();
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

        public DataTable Get_VehicleLogSheet()
        {
            string vehicle = "", zone = "";
            if (dd_van.SelectedValue != "0")
            {
                vehicle = "AND vmm.vehicle='" + dd_van.SelectedValue + "'";
            }
            else
            { vehicle = ""; }

            if (ddl_zone.SelectedValue != "0")
            {
                zone = "AND Z.ZONECODE = '" + ddl_zone.SelectedValue + "' \n";
            }

            //string sqlString = "SELECT convert(varchar,convert(datetime,vvlm.log_date,105),103) LOG_DATE,\n" +
            //"vvlm.log_no LOG_NO,r.firstName DSR,rr.firstName DRIVER, v.name VEHICLE, vvlm.opn_reading [OPEN], vvlm.cur_reading [CURRENT],\n" +
            //"convert(varchar,convert(datetime,vvlm.ddate,105),103) DEPARTURE,convert(varchar,convert(datetime,vvlm.adate,105),103) ARRIVAL, vvlm.enter_user [USER]\n" +
            //"FROM vmm_vehicle_log_master vvlm\n" +
            //"JOIN Riders r\n" +
            //"ON r.riderCode = vvlm.dsr\n" +
            //"JOIN Riders rr\n" +
            //"ON rr.riderCode = vvlm.driver\n" +
            //"JOIN Vehicle v\n" +
            //"ON vvlm.vehicle = v.id\n" +
            //"WHERE vvlm.log_date >= '"+DateFormat(txt_fromdate.Text)+"' AND\n" +
            //"        vvlm.log_date <= '" + DateFormat(txt_todate.Text) + "'\n" +
            //"ORDER BY vvlm.log_date";


            string sqlString = "select vmm.log_no Log_No, convert(varchar,convert(datetime,vmm.log_date,105),103) Log_Date,\n" +
            //"d.firstName+' '+d.lastName AS Driver, \n" +
            "CASE WHEN vmm.driver1 = '0' OR VMM.driver1 IS NULL THEN VMM.remarks ELSE d.firstName+' '+d.lastName END Driver, \n" +
            "vh.name Vehicle,vt.TypeDesc VehicleType,\n" +
            " vmm.opn_reading Opn, vmm.cur_reading Curr,convert(varchar,convert(datetime,vmm.ddate,105),103) DD,vmm.dtime DT,convert(varchar,convert(datetime,vmm.adate,105),103) AD,vmm.atime AT,vmm.fuel_type FUEL_TYPE,vmm.fuel_Amount FUEL_AMT\n" +
            "from vmm_vehicle_log_master vmm\n" +
            " left join riders d\n" +
            " on vmm.driver1 = d.ridercode\n" +
             "AND VMM.BRANCH = D.BRANCHID\n" +
            " inner join vehicle vh\n" +
            " on vmm.Vehicle = vh.id\n" +
            " INNER JOIN Vehicle_Type vt\n" +
            " on vmm.vehicle_type =  vt.typeid\n" +
            "LEFT JOIN Zones z ON vh.zoneCode = z.zoneCode \n" +
            " WHERE\n" +
    "vmm.log_date >= convert(datetime,'" + txt_fromdate.Text + "',105)\n" +
    "and vmm.log_date <= convert(datetime,'" + txt_todate.Text + "',105)\n" +
     "" + zone + "\n" + vehicle + "\n" +
    "ORDER BY vmm.log_Date,vmm.log_no";



            //string sqlString = "SELECT vvl.logId Log_No, convert(varchar,convert(datetime,vvl.logDate,105),103) Log_Date, d.firstName+' '+d.lastName AS Driver, v2.name Vehicle,\n" +
            //"vt.TypeDesc VehicleType, vvl.opn_reading Opn, vvl.cur_reading Curr,convert(varchar,convert(datetime,vvl.ddate,105),103) DD,vvl.dtime DT,convert(varchar,convert(datetime,vvl.adate,105),103) AD,vvl.atime AT\n" +
            //"  FROM VMM_VEHICLE_LOG vvl\n" +
            //"INNER JOIN Drivers d\n" +
            //"ON vvl.driverId_1 = d.did\n" +
            //"AND vvl.routeCode = d.routeCode\n" +
            //"AND vvl.zoneId = d.zoneId\n" +
            //"AND vvl.branchId = d.branchId\n" +
            //"INNER JOIN Vehicle_Type vt\n" +
            //"ON vvl.vehicle_type = vt.TypeID\n" +
            //"INNER JOIN Vehicle v2\n" +
            //"ON vvl.vehicle = v2.id\n" +
            //"AND vvl.vehicle_type = v2.VehicleType\n" +
            //"WHERE\n" +
            //"vvl.logDate >= convert(datetime,'" + txt_fromdate.Text + "',105)\n" +
            //"and vvl.logDate <= convert(datetime,'" + txt_todate.Text + "',105)\n" +
            //    //  "vvl.logDate >= '" + DateFormat(txt_fromdate.Text) + "'\n" +
            //    //  "and vvl.logDate <= '" + DateFormat(txt_todate.Text) + "'\n" +
            //"" + vehicle + "\n" +
            //"and vvl.[status]='1'\n" +
            //"ORDER BY vvl.logDate";



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
        private string DateFormat(string methDate)
        {
            string mdate = DateTime.ParseExact(methDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            string[] mmarr = mdate.Split('/', ' ');
            return string.Concat(mmarr);
        }

        public DataSet Get_VehicleListByID(string zone)
        {
            // string query = "SELECT v.id, v.NAME FROM Vehicle v where v.status = '1'";
            string query = "select * from vehicle where zonecode='" + zone + "' and status='1' ORDER BY NAME ASC";
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

        public DataSet Get_VehicleListByID()
        {

            string query = "SELECT v.id, v.NAME FROM Vehicle v order by v.id";

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
        protected void V_type_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                    string distributor = "";
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

                        //HeaderR1.Text = "Muller & Phipps Pakistan (Pvt.) Ltd.";
                        HeaderR1.Text = "M&P Express Logistics (Private) Limited.";
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
                        HeaderR2.Text = "Vehicle Log Sheet Report";
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
                        int colCountR4 = grid.Rows[0].Cells.Count - 12;
                        HeaderR4.ColumnSpan = colCountR4;
                        HeaderR4.Text = "Date From:<br>" + txt_fromdate.Text + " ";


                        TableCell HeaderR3 = new TableHeaderCell();
                        HeaderR3.Attributes.Add("Style", "FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        int colCountR3 = grid.Rows[0].Cells.Count - 2;
                        HeaderR3.ColumnSpan = colCountR3;
                        HeaderR3.Text = " " + distributor + " ";

                        TableCell HeaderR5 = new TableHeaderCell();
                        HeaderR5.Attributes.Add("Style", "text-align:left, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        int colCountR5 = grid.Rows[0].Cells.Count - 12;
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
                        HeaderR7.Attributes.Add("Style", "FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR7.ColumnSpan = 5;
                        HeaderR7.Text = "";

                        TableCell HeaderR6 = new TableHeaderCell();
                        HeaderR6.Attributes.Add("Style", "text-align:center, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR6.ColumnSpan = 2;
                        HeaderR6.Text = "METER READING";

                        TableCell HeaderR06 = new TableHeaderCell();
                        HeaderR06.Attributes.Add("Style", "text-align:center, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR06.ColumnSpan = 2;
                        HeaderR06.Text = "FUEL";


                        TableCell HeaderR8 = new TableHeaderCell();
                        HeaderR8.Attributes.Add("Style", "text-align:center, FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR8.ColumnSpan = 2;
                        HeaderR8.Text = "DEPARTURE";

                        TableCell HeaderR9 = new TableHeaderCell();
                        HeaderR9.Attributes.Add("Style", "FONT: 0.79em! important 'Helvetica Neue','Helvetica,sans-serif'");
                        HeaderR9.ColumnSpan = 4;
                        HeaderR9.Text = "ARRIVAL";


                        Row4.Cells.Add(HeaderR7);
                        Row4.Cells.Add(HeaderR6);
                        Row4.Cells.Add(HeaderR06);
                        Row4.Cells.Add(HeaderR8);
                        Row4.Cells.Add(HeaderR9);
                        Table R6Table = grid.Controls[0] as Table;
                        if (R6Table != null)
                        {
                            R6Table.Rows.AddAt(3, Row4);
                        }

                    }

                    //DataTable dt = (DataTable)ViewState["LOG_SHEET"];
                    //GridViewRow row_total = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
                    //row_total.BackColor = ColorTranslator.FromHtml("#00427c"); //#F4FC9E
                    //row_total.Attributes.Add("Style", " font-size: x-small;font-family: Calibri;");
                    //row_total.ForeColor = Color.White;
                    //row_total.Font.Bold = true;
                    //string grandtotal = dt.Compute("Sum(CM_INV_AMT)", "").ToString();
                    //if (dt != null)
                    //{
                    //    row_total.Cells.AddRange(new TableCell[] { 
                    //                                new TableCell { Text = "GRAND TOTAL", HorizontalAlign = HorizontalAlign.Left,ColumnSpan=13},
                    //                                new TableCell { Text = grandtotal, HorizontalAlign = HorizontalAlign.Center},
                    //                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                    //                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                    //                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                    //                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                    //                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                    //                                new TableCell { Text = "", HorizontalAlign = HorizontalAlign.Left},
                    //                             });
                    //    gvwData.Controls[0].Controls.Add(row_total);

                    //}

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