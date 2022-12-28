using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;

namespace MRaabta.Files
{
    public partial class Runsheet : System.Web.UI.Page
    {
        CommonFunction func = new CommonFunction();
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {


            Errorid.Text = "";
            if (hd_modeChanged.Value == "1")
            {
                gv_consignments.DataSource = null;
                gv_consignments.DataBind();

                DataTable dt = new DataTable();
                dt.Columns.Add("ConNo", typeof(string));
                dt.Columns.Add("ORIGIN", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Columns.Add("ConType", typeof(string));
                dt.Columns.Add("IsNew", typeof(string));
                dt.Columns.Add("SortOrder", typeof(int));
                dt.Columns.Add("removeable", typeof(int));
                dt.AcceptChanges();
                ViewState["dt"] = dt;
                hd_modeChanged.Value = "0";
            }
            if (rbtn_formMode.SelectedValue == "0")
            {

                txt_runsheetNumber.Enabled = false;
                dd_route.Enabled = true;
                txt_routeCode.Enabled = true;
                div_date.Style.Add("display", "block");
                div_dateDisplay.Style.Add("display", "none");
                dd_docType.Enabled = true;
                dd_runsheetType.Enabled = true;
            }
            else
            {

                txt_runsheetNumber.Enabled = true;
                dd_route.Enabled = false;
                txt_routeCode.Enabled = false;
                div_date.Style.Add("display", "none");
                div_dateDisplay.Style.Add("display", "block");
                dd_docType.Enabled = false;
                dd_runsheetType.Enabled = false;
            }
            err_msg.Text = Errorid.Text = "";
            Errorid.Text = "";
            if (!IsPostBack)
            {
                hd_currentMode.Value = rbtn_formMode.SelectedValue;
                picker1.SelectedDate = DateTime.Now.Date;
                picker1.MaxDate = DateTime.Now.Date.AddDays(1);
                //picker1.MinDate = DateTime.Now.Date;
                //txt_runsheetNumber.Text = (Convert.ToInt64(func.GetMaxRunsheetNumber().Rows[0][0].ToString()) + 1).ToString();
                DataTable dt = new DataTable();
                dt.Columns.Add("ConNo", typeof(string));
                dt.Columns.Add("ORIGIN", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Columns.Add("ConType", typeof(string));
                dt.Columns.Add("IsNew", typeof(string));
                dt.Columns.Add("SortOrder", typeof(int));
                dt.Columns.Add("removeable", typeof(int));
                dt.AcceptChanges();
                ViewState["dt"] = dt;
                // GetRiderNames();
                GetRouteNames();
                GetRunsheetTypes();
                GetOrigin();
                GetCNLengths();
                Get_MasterVehicle();
                GetVehicleType();
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["mode"].ToString() == "1")
                    {
                        txt_runsheetNumber.Text = Request.QueryString["XCode"].ToString();
                        txt_runsheetNumber.Enabled = true;
                        dd_route.Enabled = false;
                        txt_routeCode.Enabled = false;
                        div_date.Style.Add("display", "none");
                        div_dateDisplay.Style.Add("display", "block");
                        dd_docType.Enabled = false;
                        dd_runsheetType.Enabled = false;
                        rbtn_formMode.SelectedValue = "1";
                        txt_runsheetNumber_TextChanged(this, e);
                    }
                }
            }


        }
        public void GetCNLengths()
        {
            string query = "SELECT * FROM MNP_ConsignmentLengths where status = '1'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                ViewState["cnLengths"] = dt;
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        protected void GetRouteNames()
        {
            DataTable dt = func.Routes();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_route.DataSource = dt;
                    dd_route.DataTextField = "Route";
                    dd_route.DataValueField = "routeCode";
                    dd_route.DataBind();
                }
            }
        }
        protected void GetRiderNames()
        {
            clvar.routeCode = txt_routeCode.Text;
            DataTable dt = func.RidersByRoutes(clvar);
            dd_riders.Items.Clear();


            //dd_riders.Items.Add(new ListItem("Select Rider", "0"));
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_riders.Items.Add(new ListItem { Text = "Select Rider", Value = "0" });
                    dd_riders.DataSource = dt;
                    dd_riders.DataTextField = "RiderName";
                    dd_riders.DataValueField = "riderCode";
                    dd_riders.DataBind();
                    txt_riderno.Text = dt.Rows[0]["RiderCode"].ToString();
                    dd_riders.SelectedValue = dt.Rows[0]["RiderCode"].ToString();
                }
                else
                {
                    txt_routeCode.Text = "";
                    dd_route.ClearSelection();
                    txt_riderno.Text = "";
                    dd_riders.ClearSelection();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Rider Found')", true);
                    return;
                }
            }

        }
        protected void GetRunsheetTypes()
        {
            DataTable dt = func.RunsheetTypes();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dd_runsheetType.DataSource = dt;
                    dd_runsheetType.DataTextField = "code";
                    dd_runsheetType.DataValueField = "Id";
                    dd_runsheetType.DataBind();
                }
                dd_runsheetType.SelectedValue = "12";
            }
        }
        protected void GetVehicleType()
        {
            string query = "select * from Vehicle_Type where status = '1'";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dd_vehicleType.DataSource = dt;
                    dd_vehicleType.DataTextField = "TypeDesc";
                    dd_vehicleType.DataValueField = "Typeid";
                    dd_vehicleType.DataBind();
                }
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
        }
        public void Get_MasterVehicle()
        {
            DataSet ds_vehicle = Get_MasterVehicle(clvar);

            if (ds_vehicle.Tables[0].Rows.Count != 0)
            {
                dd_vehicle.DataTextField = "MakeModel";
                dd_vehicle.DataValueField = "VehicleCode";
                dd_vehicle.DataSource = ds_vehicle.Tables[0].DefaultView;
                dd_vehicle.DataBind();
            }
            //dd_vehicle.Items.Insert(0, new ListItem("Select Vehicle ", ""));
        }
        protected void GetOrigin()
        {
            DataTable dt = Branch().Tables[0];
            ViewState["origins"] = dt;
        }
        public DataSet Branch()
        {
            DataSet Ds_1 = new DataSet();

            try
            {
                string query = "SELECT b.branchCode, \n"
               + "       b.name     BranchName \n"
               + "FROM   Branches                          b \n"
               + "--where b.[status] ='1' \n"
               + "GROUP BY \n"
               + "       b.branchCode, \n"
               + "       b.name order by b.name ASC";

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
        protected void txt_cnNumber_TextChanged(object sender, EventArgs e)
        {
            DataTable cnLength = ViewState["cnLengths"] as DataTable;
            //if (!IsNumeric(txt_cnNumber.Text))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics Allowed in Consingnment Numbers')", true);
            //    Errorid.Text = err_msg.Text = "Only Numerics Allowed in Consingnment Numbers";
            //    txt_cnNumber.Text = "";
            //    txt_cnNumber.Focus();
            //    return;
            //}

            //if (txt_cnNumber.Text.Length < 11 || txt_cnNumber.Text.Length > 20)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number must be between 11 and 20 digits.')", true);
            //    Errorid.Text = err_msg.Text = "Consignment Number must be between 11 and 15 digits.";
            //    txt_cnNumber.Text = "";
            //    txt_cnNumber.Focus();
            //    return;
            //}

            clvar.consignmentNo = txt_cnNumber.Text;
            //if (clvar.consignmentNo.Length < 11)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('CN Must be atleast 11 Characters')", true);
            //    txt_cnNumber.Text = "";
            //    txt_cnNumber.Focus();
            //    return;
            //}
            DataTable dt = ViewState["dt"] as DataTable;
            if (dt.Select("ConNo = '" + txt_cnNumber.Text + "'", "").Count() > 0 && gv_consignments.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate')", true);
                txt_cnNumber.Text = "";
                txt_cnNumber.Focus();
                return;
            }

            int order = 0;
            int.TryParse(dt.Compute("MAX(SortOrder)", "").ToString(), out order);
            DataTable dt_ = GetConsignmentDetail_(clvar);
            if (dt_.Rows.Count > 0)
            {
                if (dt.Select("ConNo = '" + dt_.Rows[0][0].ToString() + "'", "").Count() > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate')", true);
                    txt_cnNumber.Text = "";
                    txt_cnNumber.Focus();
                    return;
                }
            }


            if (dt_ != null)
            {
                if (dt_.Rows.Count > 0)
                {
                    if (dt_.Rows[0]["WrongCN"].ToString() == "1")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Wrong Consignment.')", true);
                        Errorid.Text = "Wrong Consignment.";
                        txt_cnNumber.Text = "";
                        txt_cnNumber.Focus();

                        return;
                    }
                    if (dt_.Rows[0]["RunsheetAllowed"].ToString().ToUpper() == "TRUE")
                    {

                        if ((dt_.Rows[0]["RTO"].ToString().ToUpper() == "TRUE" || dt_.Rows[0]["RTO"].ToString() == "1"))
                        {
                            if (dt_.Rows[0]["Destination"].ToString() == HttpContext.Current.Session["BranchCode"].ToString())
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment already Returned to Origin.')", true);
                                Errorid.Text = "Consignment already Returned to Origin.";
                                txt_cnNumber.Text = "";
                                return;
                            }
                        }
                        int numberOfRecords = dt_.Select().Length;
                        lbl_count.Text = "Count: " + numberOfRecords.ToString();

                        DataRow dr = dt.NewRow();
                        dr["ConNo"] = dt_.Rows[0]["ConNo"].ToString();
                        dr["Origin"] = dt_.Rows[0]["Orgin"].ToString();
                        dr["Name"] = dt_.Rows[0]["Destination"].ToString();
                        dr["ConType"] = dt_.Rows[0]["ConType"].ToString();
                        dr["IsNew"] = "0";
                        dr["SortOrder"] = order + 1;
                        dr["Removeable"] = "1";
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                        ViewState["dt"] = dt;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Runsheet Not Allowed for this consignment.')", true);
                        Errorid.Text = "Runsheet Not Allowed for this consignment.";
                        txt_cnNumber.Text = "";
                        return;
                    }

                }
                else
                {
                    //DataRow[] lengthRow = cnLength.Select("PREFIX = SUBSTRING('" + txt_cnNumber.Text + "', '0', CONVERT(LEN(CONVERT(PREFIX, System.String)), System.String))");
                    bool found = false;
                    bool correctLength = false;
                    foreach (DataRow row in cnLength.Rows)
                    {
                        string prefix = row["Prefix"].ToString();
                        int length = int.Parse(row["Length"].ToString());
                        if (prefix.Length > txt_cnNumber.Text.Length)
                        {
                            continue;
                        }
                        if (txt_cnNumber.Text.Substring(0, prefix.Length) == prefix)
                        {
                            found = true;
                            if (txt_cnNumber.Text.Length == length)
                            {
                                correctLength = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Prefix.')", true);
                        Errorid.Text = "Invalid Prefix.";
                        txt_cnNumber.Text = "";
                        txt_cnNumber.Focus();
                        return;
                    }
                    if (!correctLength)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Length.')", true);
                        Errorid.Text = "Invalid Prefix.";
                        txt_cnNumber.Text = "";
                        txt_cnNumber.Focus();
                        return;
                    }
                    if (dt.Select("ConNo = '" + txt_cnNumber.Text + "'", "").Count() > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate')", true);
                        txt_cnNumber.Text = "";
                        txt_cnNumber.Focus();
                        return;
                    }
                    DataRow dr = dt.NewRow();
                    dr["ConNo"] = txt_cnNumber.Text;
                    dr["Origin"] = HttpContext.Current.Session["BranchCode"].ToString();
                    dr["Name"] = HttpContext.Current.Session["BranchCode"].ToString();
                    dr["ConType"] = "NORMAL";
                    dr["IsNew"] = "1";
                    dr["SortOrder"] = order + 1;
                    dr["Removeable"] = "1";
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    ViewState["dt"] = dt;

                    int numberOfRecords = dt.Select().Length;
                    lbl_count.Text = "Count: " + numberOfRecords.ToString();
                }
            }

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.AsDataView();
                    dv.Sort = "SortOrder DESC";

                    gv_consignments.DataSource = dv;
                    gv_consignments.DataBind();
                    txt_cnNumber.Text = "";
                    txt_cnNumber.Focus();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Does Not Exists')", true);
                    txt_cnNumber.Text = "";
                    txt_cnNumber.Focus();
                }
            }
            else
            {

            }
        }
        protected void txt_routeCode_TextChanged(object sender, EventArgs e)
        {
            bool found = false;
            string str = "";
            foreach (ListItem item in dd_route.Items)
            {
                if (txt_routeCode.Text == item.Value)
                {
                    str = item.Value;
                    item.Selected = true;
                    found = true;
                    break;
                }
            }
            if (found)
            {
                dd_route.SelectedValue = str;
                clvar.routeCode = txt_routeCode.Text;
                GetRiderNames();
                txt_cnNumber.Focus();
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('')", true);
                AlertMessage("Invalid Route Code", "Red");
                dd_route.ClearSelection();
                txt_routeCode.Text = "";
                txt_riderno.Text = "";
                dd_riders.ClearSelection();
                txt_routeCode.Focus();
                return;
            }
        }
        protected void dd_route_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_routeCode.Text.Trim() == "")
            {
                AlertMessage("Invalid Route code", "Red");
                return;
            }
            txt_routeCode.Text = dd_route.SelectedValue;
            GetRiderNames();
        }
        protected void dd_riders_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_riderno.Text = dd_riders.SelectedValue;
        }
        protected void gv_consignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                DataTable existingConsignments = new DataTable();
                try
                {
                    existingConsignments = ViewState["existingCNS"] as DataTable;
                }
                catch (Exception ex)
                { }



                DataTable dt = ViewState["dt"] as DataTable;
                DataRow dr = dt.Select("ConNo = '" + e.CommandArgument.ToString() + "'")[0];

                dt.Rows.Remove(dr);
                gv_consignments.DataSource = null;
                gv_consignments.DataBind();

                DataView dv = dt.AsDataView();
                dv.Sort = "SortOrder DESC";

                gv_consignments.DataSource = dv;
                gv_consignments.DataBind();
                if (existingConsignments != null)
                {
                    if (existingConsignments.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in gv_consignments.Rows)
                        {
                            if (existingConsignments.Select("ConsignmentNumber = '" + row.Cells[1].Text + "'", "").Count() > 0)
                            {
                                //foreach (System.Web.UI.HtmlControls.HtmlTableCell cell in row.Cells)
                                //{
                                //    cell.BgColor = "#FFCCD7";

                                //}
                                row.BackColor = System.Drawing.Color.FromName("#FFCCD7");
                            }
                        }
                    }
                }

            }
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {

            try
            {
                if (picker1.SelectedDate != null)
                {
                    DateTime runsheetDate = picker1.SelectedDate.Value;
                    if (runsheetDate > DateTime.Now.Date.AddDays(1))
                    {
                        AlertMessage("Invalid Date", "Red");
                        return;
                    }
                }
                else
                {
                    AlertMessage("Invalid Date", "Red");
                    return;
                }

                #region Validations
                if (rbtn_formMode.SelectedValue == "1")
                {
                    if (txt_runsheetNumber.Text.Trim() == "")
                    {
                        AlertMessage("Enter Runsheet Number", "Red");
                        return;
                    }
                }
                if (dd_riders.SelectedValue == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Rider')", true);
                    return;
                }
                if (dd_route.SelectedValue == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Route')", true);
                    return;
                }
                if (dd_runsheetType.SelectedValue == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Runsheet Type')", true);
                    return;
                }

                string VehicleNumber = "";
                string VehicleType = "";
                Int64 MeterStart = -1;
                Int64 MeterEnd = -1;
                if (dd_vehicle.SelectedValue == "0")
                {
                    if (txt_vehicleNumber.Text.Trim() != "")
                    {
                        VehicleNumber = txt_vehicleNumber.Text.Trim();
                    }
                    else
                    {
                        //AlertMessage("Select or Enter Vehicle Number", "Red");
                        //return;
                    }
                }
                else
                {
                    VehicleNumber = dd_vehicle.SelectedValue;
                }

                //if (dd_vehicleType.SelectedValue == "0")
                //{
                //    AlertMessage("Select Vehicle Type", "Red");
                //    return;
                //}
                //else
                //{
                //    VehicleType = dd_vehicleType.SelectedValue;
                //}
                Int64.TryParse(txt_meterStart.Text, out MeterStart);
                Int64.TryParse(txt_meterEnd.Text, out MeterEnd);

                //if (MeterStart < 0 || MeterEnd < 0)
                //{
                //    AlertMessage("Enter Proper Meter Reading", "Red");
                //    return;
                //}

                //if (DateTime.Compare(picker1.SelectedDate.Value, DateTime.Today) < 0)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Create Runsheet on BackDate')", true);
                //    return;
                //}
                //DataTable dt = new DataTable();//ViewState["dt"] as DataTable;
                DataTable newDt = new DataTable();//dt.Clone();
                newDt.Columns.Add("ConNo", typeof(string));
                newDt.Columns.Add("ORIGIN", typeof(string));
                newDt.Columns.Add("NAME", typeof(string));
                newDt.Columns.Add("ConType", typeof(string));
                newDt.Columns.Add("IsNew", typeof(string));
                newDt.Columns.Add("CITYCODE", typeof(string));
                newDt.AcceptChanges();
                //newDt = dt;
                foreach (GridViewRow row in gv_consignments.Rows)
                {
                    DropDownList dd_origin = row.FindControl("dd_gOrigin") as DropDownList;
                    DropDownList dd_destination = row.FindControl("dd_gDestination") as DropDownList;
                    if (dd_destination.SelectedValue == "0" || dd_origin.SelectedValue == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Origin')", true);
                        dd_origin.Focus();
                        return;
                    }
                    if ((row.FindControl("hd_new") as HiddenField).Value == "1")
                    {
                        DataRow dr = newDt.NewRow();
                        dr["ConNo"] = row.Cells[1].Text;
                        dr["Origin"] = dd_origin.SelectedValue;
                        dr["Name"] = dd_destination.SelectedValue;
                        dr["ConType"] = row.Cells[4].Text;
                        dr["CITYCODE"] = dd_origin.SelectedItem.Text;
                        newDt.Rows.Add(dr);
                        newDt.AcceptChanges();
                    }




                }

                //dt = dt.Clone();


                #endregion

                //clvar.RunSheetNumber = txt_runsheetNumber.Text;
                clvar.CustomerClientID = "330140";
                clvar.AccountNo = "4D1";
                clvar.riderCode = "";


                string error = "";
                if (newDt.Rows.Count > 0)
                {
                    clvar.Weight = 0.5f;
                    clvar.pieces = 1;
                    error = con.InsertConsignmentsFromRunsheet(clvar, newDt);
                }

                if (!(error.Contains("NOT OK")))
                {

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not Insert New CNs Error: " + error + "')", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(Runsheet Number '" + error + "' has been generated.)", true);
                    return;
                }

                clvar.RunSheetDate = picker1.SelectedDate.ToString();
                clvar.RunSheetTypeID = dd_runsheetType.SelectedValue;
                clvar.routeCode = dd_route.SelectedValue;
                clvar.riderCode = dd_riders.SelectedValue;
                clvar.RiderCode = dd_riders.SelectedItem.Text;
                string[,] arr = new string[gv_consignments.Rows.Count, 2];

                error = "";
                int counter = 0;
                foreach (GridViewRow row in gv_consignments.Rows)
                {
                    HiddenField hd_removeable = row.FindControl("hd_removeable") as HiddenField;
                    clvar.ClvarListStr.Add(row.Cells[1].Text);
                    arr[counter, 0] = row.Cells[1].Text;
                    arr[counter, 1] = hd_removeable.Value;

                    counter++;
                }
                //DataTable existingCNsInRunsheet = con.ExistingConsignmentInRunsheetSameDay(clvar);
                //ViewState["existingCNS"] = existingCNsInRunsheet;
                //if (existingCNsInRunsheet.Rows.Count > 0)
                //{
                //    foreach (GridViewRow row in gv_consignments.Rows)
                //    {
                //        if (existingCNsInRunsheet.Select("ConsignmentNumber = '" + row.Cells[1].Text + "'", "").Count() > 0)
                //        {
                //            //foreach (System.Web.UI.HtmlControls.HtmlTableCell cell in row.Cells)
                //            //{
                //            //    cell.BgColor = "#FFCCD7";

                //            //}
                //            row.BackColor = System.Drawing.Color.FromName("#FFCCD7");
                //        }
                //    }
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Runsheet Already Created for Highlighted CN(s)')", true);
                //    return;
                //}
                List<string> ResponseList = new List<string>();
                if (rbtn_formMode.SelectedValue == "0")
                {

                    ResponseList = GenerateRunsheet_new(clvar, VehicleNumber, VehicleType, MeterStart, MeterEnd);
                }
                else if (rbtn_formMode.SelectedValue == "1")
                {

                    clvar.RunsheetNumber = txt_runsheetNumber.Text;
                    ResponseList = EditRunsheet(clvar, arr, VehicleNumber, VehicleType, MeterStart, MeterEnd);
                }


                if (ResponseList[0] == "0")
                {

                }
                else if (ResponseList[0] == "1")
                {
                    if (rbtn_formMode.SelectedValue == "1")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Runsheet Updated.\\nRunsheet Number: " + ResponseList[1] + "')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Runsheet Generated.\\nRunsheet Number: " + ResponseList[1] + "')", true);
                    }

                    string temp_ = error.ToString();

                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                    string script = String.Format(script_, "RunsheetInvoice_Plain.aspx?Xcode=" + ResponseList[1] + "&RCode=" + dd_route.SelectedValue, "_blank", "");

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                    btn_reset_Click(sender, e);
                    return;
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Generate Runsheet. Error: " + error + "')", true);
                    Errorid.Text = "Could Not Generate Runsheet. Error: " + ResponseList[1] + "";

                    loader.Style.Add("display", "none");
                }

                #region MyRegion
                //error = GenerateRunsheet(clvar);

                //if (!(error.Contains("Could Not Save Runsheet Error")))
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Runsheet Generated.\\nRunsheet Number: " + error + "')", true);
                //    string temp_ = error.ToString();

                //    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                //    //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                //    string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_ + "&RCode=" + dd_route.SelectedValue, "_blank", "");

                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                //    btn_reset_Click(sender, e);
                //    return;


                //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(Runsheet Number '" + error + "' has been generated.)", true);

                //    //btn_reset_Click(sender, e);

                //}
                //else
                //{
                //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Generate Runsheet. Error: " + error + "')", true);
                //    Errorid.Text = "Could Not Generate Runsheet. Error: " + error + "";

                //    loader.Style.Add("display", "none");
                //} 
                #endregion
            }
            catch (Exception ex)
            {

                Errorid.Text = ex.Message;
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            //txt_runsheetNumber.Text = (Convert.ToInt64(func.GetMaxRunsheetNumber().Rows[0][0].ToString()) + 1).ToString();
            txt_cnNumber.Text = "";
            txt_routeCode.Text = "";
            //txt_runsheetNumber.Text = "";
            txt_riderno.Text = "";
            dd_riders.ClearSelection();
            dd_docType.ClearSelection();
            dd_route.ClearSelection();
            dd_runsheetType.ClearSelection();

            txt_vehicleNumber.Text = "";
            dd_vehicle.ClearSelection();
            dd_vehicleType.ClearSelection();
            txt_meterStart.Text = "";
            txt_meterEnd.Text = "";


            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
            DataTable dt = new DataTable();
            dt.Columns.Add("ConNo", typeof(string));
            dt.Columns.Add("ORIGIN", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ConType", typeof(string));
            dt.Columns.Add("IsNew", typeof(string));
            dt.Columns.Add("SortOrder", typeof(int));
            dt.Columns.Add("removeable", typeof(int));
            dt.AcceptChanges();
            ViewState["dt"] = dt;
            //ViewState["dt"] = dt;
            //gv_consignments.DataSource = null;
            //gv_consignments.DataBind();

            //Response.Redirect("Runsheet.aspx");

        }
        protected void txt_riderno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txt_riderno.Text.Trim() != "")
                {
                    dd_riders.SelectedValue = txt_riderno.Text;
                }
            }
            catch (Exception ex)
            { }
        }
        protected void gv_consignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dt = ViewState["dt"] as DataTable;

                DataRow dr = dt.Select("CONNO = '" + e.Row.Cells[1].Text + "'")[0];

                DataTable origins = ViewState["origins"] as DataTable;

                Button btn_remove = e.Row.FindControl("btn_remove") as Button;
                HiddenField hd_removeable = e.Row.FindControl("hd_removeable") as HiddenField;

                if (hd_removeable.Value == "0")
                {
                    btn_remove.Visible = false;
                }
                else
                {
                    btn_remove.Visible = true;
                }

                DropDownList dd_origin = e.Row.FindControl("dd_gOrigin") as DropDownList;
                DropDownList dd_destinations = e.Row.FindControl("dd_gDestination") as DropDownList;
                dd_origin.DataSource = origins;
                dd_origin.DataTextField = "BranchName";
                dd_origin.DataValueField = "BranchCode";
                dd_origin.DataBind();

                dd_destinations.DataSource = origins;
                dd_destinations.DataTextField = "BranchName";
                dd_destinations.DataValueField = "BranchCode";
                dd_destinations.DataBind();

                dd_origin.SelectedValue = dr["ORIGIN"].ToString();
                //dd_destinations.Items.Clear();
                //string destinationValue = dr["Name"].ToString();
                //string destinationName = origins.Select("BranchCode = '" + dr["Name"].ToString() + "'").FirstOrDefault()["BranchName"].ToString();

                //dd_destinations.Items.Add(new ListItem { Text = destinationName, Value = destinationValue });

                dd_destinations.SelectedValue = dr["NAME"].ToString();

                dd_destinations.Enabled = false;

                int numberOfRecords = dt.Select().Length;
                lbl_count.Text = "Count: " + numberOfRecords.ToString();
            }
        }
        protected void dd_gOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList origin = (DropDownList)sender;
            DataTable dt = ViewState["dt"] as DataTable;

            GridViewRow row = origin.Parent.Parent as GridViewRow;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ConNo"].ToString() == row.Cells[1].Text)
                {
                    dt.Rows[i]["Origin"] = origin.SelectedValue;
                }
            }
        }


        public string GenerateRunsheet(Cl_Variables clvar)
        {
            CommonFunction func = new CommonFunction();
            Int64 runsheetNumber = (Convert.ToInt64(func.GetMaxRunsheetNumber().Rows[0][0].ToString())) + 1;
            string error = "";
            int count = 0;

            DataTable Rdetails = new DataTable();
            Rdetails.Columns.Add("RunsheetNumber");
            Rdetails.Columns.Add("ConsignmentNumber");
            Rdetails.Columns.Add("SortOrder");

            string query = "insert into Runsheet (runsheetNumber, routeCode, createdBy, createdOn, runsheetDate, branchCode, runsheetType, route, syncID)\n" +
            "\t\t\tValues   (\n" +
            "                   '" + runsheetNumber.ToString() + "',\n" +
            "                   '" + clvar.routeCode + "',\n" +
            "                   '" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
            "                   GetDate(),\n" +
            "                   '" + DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd") + "',\n" +
            "                   '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            "                   '" + clvar.RunSheetTypeID + "',\n" +
            "                   '" + clvar.RouteDesc + "', NEWID()" +
            "\t\t\t)";

            string query2 = "insert into rvdbo.Runsheet (RunsheetId, RunsheetNo, RunsheetDate, RouteId, BranchId, ZoneId, RunsheetTypeId, RiderId, createdon, CreatedById )\n" +
            "              Values (\n" +
            "                       '" + runsheetNumber.ToString() + "',\n" +
            "                       '" + runsheetNumber.ToString() + "',\n" +
            "                       '" + DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd") + "',\n" +
            "                       '" + clvar.routeCode + "',\n" +
            "                       '" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
            "                       '" + HttpContext.Current.Session["ZoneCode"].ToString() + "',\n" +
            "                       '" + clvar.RunSheetTypeID + "',\n" +
            "                       '" + clvar.riderCode + "',\n" +
            "                       GetDAte(),\n" +
            "                       '" + HttpContext.Current.Session["U_ID"].ToString() + "'" +
            "                )";

            string query3 = "insert into RunsheetConsignment (runsheetNumber, consignmentNumber, createdBy, createdOn, Status, SortOrder,branchcode,RouteCode) \n";
            string query4 = "insert into rvdbo.RunsheetConsignment (RunsheetId, ConsignmentId, CreatedOn, StatusId, PODUpdateTypeId, SortOrder, IsPODSpecified, BranchCode )";
            string query6 = "INSERT into ConsignmentsTrackingHistory (ConsignmentNumber, StateId, CurrentLocation, riderName, runsheetNumber, TransactionTime) \n";
            int j = clvar.ClvarListStr.Count - 1;
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string cnNumbers = "";
            for (int i = 0; i < clvar.ClvarListStr.Count - 1; i++)
            {
                DataRow dr = Rdetails.NewRow();
                dr[1] = clvar.ClvarListStr[i].ToString();
                dr[2] = (i + 1).ToString();
                cnNumbers += "'" + clvar.ClvarListStr[i] + "',";
                query3 += " SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[i] + "', '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), 'UNDELIVERED', '" + (i + 1).ToString() + "', '" + HttpContext.Current.Session["branchcode"].ToString() + "','" + clvar.routeCode + "'\n" +
                          " UNION ALL \n";
                query4 += "  SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[i] + "', GETDATE(), '56', '112','" + (i + 1).ToString() + "', '0', '" + clvar.Branch + "'\n" +
                            "UNION ALL \n";
                query6 += "  SELECT '" + clvar.ClvarListStr[i] + "', '8', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', '" + clvar.RiderCode + "', '" + runsheetNumber.ToString() + "', GETDATE()\n" +
                            "UNION ALL";
            }
            cnNumbers += "'" + clvar.ClvarListStr[j] + "'";
            query3 += " SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[j] + "', '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(), 'UNDELIVERED', '" + (j + 1) + "', '" + HttpContext.Current.Session["branchcode"].ToString() + "','" + clvar.routeCode + "'\n";
            query4 += "SELECT '" + runsheetNumber.ToString() + "', '" + clvar.ClvarListStr[j] + "', GETDATE(), '56', '112', '" + (j + 1) + "', '0', '" + clvar.Branch + "'\n";
            query6 += "  SELECT '" + clvar.ClvarListStr[j] + "', '8', '" + func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString() + "', '" + clvar.RiderCode + "', '" + runsheetNumber.ToString() + "', GETDATE()\n";
            string query5 = "insert into RiderRunsheet (ridercode, runsheetNumber, createdBy, createdOn, expIdTemp) Values (\n" +
                           "                       '" + clvar.riderCode + "',\n" +
                           "                       '" + runsheetNumber.ToString() + "',\n" +
                           "                       '" + HttpContext.Current.Session["U_ID"].ToString() + "', GETDATE(),\n" +
                           "                       (select r.expressCenterId from Riders r where r.riderCode = '" + clvar.riderCode + "' and r.status = '1' and branchID = '" + HttpContext.Current.Session["BranchCode"].ToString() + "')\n" +
                           "                )";

            string givenToQuery = "UPDATE RUNSHEETCONSIGNMENT SET REASON = '204', STATUS = '56', GivenToRider='" + clvar.riderCode + "' where consignmentNumber in (" + cnNumbers + ") and branchCode = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' and reason is null";

            //SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            //SqlTransaction trans;
            //sqlcon.Open();
            //SqlCommand sqlcmd = new SqlCommand();
            //sqlcmd.Connection = sqlcon;
            //trans = sqlcon.BeginTransaction();
            //sqlcmd.Transaction = trans;
            //sqlcmd.CommandType = CommandType.Text;
            SqlConnection con = new SqlConnection(clvar.Strcon());
            #region MyRegion
            //try
            //{
            //    sqlcmd.CommandText = givenToQuery;
            //    sqlcmd.ExecuteNonQuery();

            //    sqlcmd.CommandText = query;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }

            //    count = 0;
            //    //sqlcmd.CommandText = query2;
            //    //count = sqlcmd.ExecuteNonQuery();
            //    //if (count == 0)
            //    //{
            //    //    trans.Rollback();
            //    //    return "NOT OK";
            //    //}
            //    //count = 0;
            //    sqlcmd.CommandText = query3;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }
            //    count = 0;
            //    //sqlcmd.CommandText = query4;
            //    //count = sqlcmd.ExecuteNonQuery();
            //    //if (count == 0)
            //    //{
            //    //    trans.Rollback();
            //    //    return "NOT OK";
            //    //}
            //    //count = 0;
            //    sqlcmd.CommandText = query5;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }
            //    count = 0;
            //    sqlcmd.CommandText = query6;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "OK";
            //    }


            //    trans.Commit();
            //    sqlcon.Close();
            //}
            //catch (Exception ex)
            //{
            //    trans.Rollback();
            //    return "Could Not Save Runsheet Error: " + ex.Message;
            //} 
            #endregion

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_GenerateRunsheet";

                cmd.Parameters.AddWithValue("@RDetails", Rdetails);
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@RouteCode", clvar.routeCode);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@RunsheetDate", DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@RunsheetType", clvar.RunSheetTypeID);
                cmd.Parameters.AddWithValue("@RouteDesc", clvar.RouteDesc);
                cmd.Parameters.AddWithValue("@RiderName", clvar.RiderCode);
                cmd.Parameters.AddWithValue("@RiderCode", clvar.riderCode);
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();



            }
            catch (Exception ex)
            { }
            return runsheetNumber.ToString();
        }
        public List<string> GenerateRunsheet_new(Cl_Variables clvar, string VehicleNumber, string VehicleType, Int64 MeterStart, Int64 MeterEnd)
        {
            List<string> ResponseList = new List<string>();
            CommonFunction func = new CommonFunction();
            //Int64 runsheetNumber = (Convert.ToInt64(func.GetMaxRunsheetNumber().Rows[0][0].ToString())) + 1;
            string error = "";
            int count = 0;

            DataTable Rdetails = new DataTable();
            Rdetails.Columns.Add("RunsheetNumber");
            Rdetails.Columns.Add("ConsignmentNumber");
            Rdetails.Columns.Add("SortOrder");


            int j = clvar.ClvarListStr.Count - 1;
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string cnNumbers = "";
            for (int i = 0; i < clvar.ClvarListStr.Count; i++)
            {
                DataRow dr = Rdetails.NewRow();
                dr[1] = clvar.ClvarListStr[i].ToString();
                dr[2] = (i + 1).ToString();
                Rdetails.Rows.Add(dr);

            }
            cnNumbers += "'" + clvar.ClvarListStr[j] + "'";

            //SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            //SqlTransaction trans;
            //sqlcon.Open();
            //SqlCommand sqlcmd = new SqlCommand();
            //sqlcmd.Connection = sqlcon;
            //trans = sqlcon.BeginTransaction();
            //sqlcmd.Transaction = trans;
            //sqlcmd.CommandType = CommandType.Text;
            SqlConnection con = new SqlConnection(clvar.Strcon());
            #region MyRegion
            //try
            //{
            //    sqlcmd.CommandText = givenToQuery;
            //    sqlcmd.ExecuteNonQuery();

            //    sqlcmd.CommandText = query;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }

            //    count = 0;
            //    //sqlcmd.CommandText = query2;
            //    //count = sqlcmd.ExecuteNonQuery();
            //    //if (count == 0)
            //    //{
            //    //    trans.Rollback();
            //    //    return "NOT OK";
            //    //}
            //    //count = 0;
            //    sqlcmd.CommandText = query3;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }
            //    count = 0;
            //    //sqlcmd.CommandText = query4;
            //    //count = sqlcmd.ExecuteNonQuery();
            //    //if (count == 0)
            //    //{
            //    //    trans.Rollback();
            //    //    return "NOT OK";
            //    //}
            //    //count = 0;
            //    sqlcmd.CommandText = query5;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }
            //    count = 0;
            //    sqlcmd.CommandText = query6;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "OK";
            //    }


            //    trans.Commit();
            //    sqlcon.Close();
            //}
            //catch (Exception ex)
            //{
            //    trans.Rollback();
            //    return "Could Not Save Runsheet Error: " + ex.Message;
            //} 
            #endregion

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_GenerateRunsheet";

                cmd.Parameters.AddWithValue("@RDetails", Rdetails);
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@RouteCode", clvar.routeCode);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@RunsheetDate", DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@RunsheetType", clvar.RunSheetTypeID);
                cmd.Parameters.AddWithValue("@RouteDesc", clvar.RouteDesc);
                cmd.Parameters.AddWithValue("@RiderName", clvar.RiderCode);
                cmd.Parameters.AddWithValue("@RiderCode", clvar.riderCode);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
                cmd.Parameters.AddWithValue("@MeterStart", MeterStart);
                cmd.Parameters.AddWithValue("@MeterEnd", MeterEnd);
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                ResponseList.Add(cmd.Parameters["@ResultStatus"].Value.ToString());
                ResponseList.Add(cmd.Parameters["@result"].Value.ToString());



            }
            catch (Exception ex)
            { }
            return ResponseList;
        }
        public List<string> EditRunsheet(Cl_Variables clvar, string[,] arr, string VehicleNumber, string VehicleType, Int64 MeterStart, Int64 MeterEnd)
        {
            List<string> ResponseList = new List<string>();
            CommonFunction func = new CommonFunction();
            //Int64 runsheetNumber = (Convert.ToInt64(func.GetMaxRunsheetNumber().Rows[0][0].ToString())) + 1;
            string error = "";
            int count = 0;

            DataTable Rdetails = new DataTable();
            Rdetails.Columns.Add("RunsheetNumber");
            Rdetails.Columns.Add("ConsignmentNumber");
            Rdetails.Columns.Add("SortOrder");
            Rdetails.Columns.Add("Removeable");

            int j = clvar.ClvarListStr.Count - 1;
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            string cnNumbers = "";
            for (int i = 0; i < arr.Length / 2; i++)
            {
                DataRow dr = Rdetails.NewRow();
                //dr[1] = clvar.ClvarListStr[i].ToString();
                dr[1] = arr[i, 0];
                dr[2] = (i + 1).ToString();
                dr[3] = arr[i, 1];
                Rdetails.Rows.Add(dr);

            }
            cnNumbers += "'" + clvar.ClvarListStr[j] + "'";

            //SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            //SqlTransaction trans;
            //sqlcon.Open();
            //SqlCommand sqlcmd = new SqlCommand();
            //sqlcmd.Connection = sqlcon;
            //trans = sqlcon.BeginTransaction();
            //sqlcmd.Transaction = trans;
            //sqlcmd.CommandType = CommandType.Text;
            SqlConnection con = new SqlConnection(clvar.Strcon());
            #region MyRegion
            //try
            //{
            //    sqlcmd.CommandText = givenToQuery;
            //    sqlcmd.ExecuteNonQuery();

            //    sqlcmd.CommandText = query;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }

            //    count = 0;
            //    //sqlcmd.CommandText = query2;
            //    //count = sqlcmd.ExecuteNonQuery();
            //    //if (count == 0)
            //    //{
            //    //    trans.Rollback();
            //    //    return "NOT OK";
            //    //}
            //    //count = 0;
            //    sqlcmd.CommandText = query3;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }
            //    count = 0;
            //    //sqlcmd.CommandText = query4;
            //    //count = sqlcmd.ExecuteNonQuery();
            //    //if (count == 0)
            //    //{
            //    //    trans.Rollback();
            //    //    return "NOT OK";
            //    //}
            //    //count = 0;
            //    sqlcmd.CommandText = query5;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "NOT OK";
            //    }
            //    count = 0;
            //    sqlcmd.CommandText = query6;
            //    count = sqlcmd.ExecuteNonQuery();
            //    if (count == 0)
            //    {
            //        trans.Rollback();
            //        return "OK";
            //    }


            //    trans.Commit();
            //    sqlcon.Close();
            //}
            //catch (Exception ex)
            //{
            //    trans.Rollback();
            //    return "Could Not Save Runsheet Error: " + ex.Message;
            //} 
            #endregion

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_EditRunsheet";

                cmd.Parameters.AddWithValue("@RunsheetNumber", clvar.RunsheetNumber);
                cmd.Parameters.AddWithValue("@RDetails", Rdetails);
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@RouteCode", clvar.routeCode);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@RunsheetDate", DateTime.Parse(clvar.RunSheetDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@RunsheetType", clvar.RunSheetTypeID);
                cmd.Parameters.AddWithValue("@RouteDesc", clvar.RouteDesc);
                cmd.Parameters.AddWithValue("@RiderName", clvar.RiderCode);
                cmd.Parameters.AddWithValue("@RiderCode", clvar.riderCode);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
                cmd.Parameters.AddWithValue("@MeterStart", MeterStart);
                cmd.Parameters.AddWithValue("@MeterEnd", MeterEnd);
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                ResponseList.Add(cmd.Parameters["@ResultStatus"].Value.ToString());
                ResponseList.Add(cmd.Parameters["@result"].Value.ToString());



            }
            catch (Exception ex)
            { }
            return ResponseList;
        }
        public bool IsNumeric(string text)
        {
            char[] arr = text.ToCharArray();
            foreach (char ch in arr)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }

            return true;
        }
        public DataTable GetConsignmentDetail_(Cl_Variables clvar)
        {
            string sqlString = "select c.*, c.consignmentNumber ConNo,\n" +
            "     c.consignerAccountNo AccountNo,\n" +
            "     c.riderCode,\n" +
            "     ct.name ConType,\n" +
            "     cc.cityName City,\n" +
            "     b2.name Branch,\n" +
            "     c.weight,\n" +
            "     c.serviceTypeName,\n" +
            "     c.discount,\n" +
            "     c.pieces,\n" +
            "     c.consignee,\n" +
            "     c.consigneePhoneNo ConsigneeCell,\n" +
            "     c.consigneeCNICNo ConsigneeCNIC,\n" +
            "     c.consigner,\n" +
            "     c.consignerCellNo ConsignerCell,\n" +
            "     c.consignerCNICNo ConsignerCNIC,\n" +
            "     c.couponNumber Coupon,\n" +
            "     c.decalaredValue DeclaredValue,\n" +
            "     c.PakageContents PackageContents,\n" +
            "     c.address Address,\n" +
            "     c.shipperAddress,\n" +
            "     c.bookingDate,\n" +
            "     b.name ORIGIN,\n" +
            "     c.originExpressCenter,\n" +
            "     ec.name,\n" +
            "     c.insuarancePercentage,\n" +
            "     c.totalAmount,\n" +
            "     c.chargedAmount,\n" +
            "     c.isInsured,\n" +
            "     c.dayType,c.gst, ccc.CODTYPE\n" +
            "     from Consignment c\n" +
            "     inner join Zones z\n" +
            "     on z.zoneCode = c.zoneCode\n" +
            "     inner join Branches b\n" +
            "     on b.branchCode = c.branchCode\n" +
            "     inner join Branches b2\n" +
            "\t   on b2.branchCode = c.destination\n" +
            "\t   inner join ConsignmentType ct\n" +
            "\t   on ct.id = c.consignmentTypeId\n" +
            "\t   inner join Cities cc\n" +
            "\t   on cc.id = b2.cityId\n" +
            "\t   left outer join ExpressCenters ec\n" +
            "\t   on ec.expressCenterCode = c.originExpressCenter\n" +
            "\t   inner join CreditClients ccc\n" +
            "\t    on ccc.id = c.creditClientId\n" +
            "\t    where c.consignmentNumber = '" + clvar.consignmentNo + "'";



            sqlString = "select c.*, c.consignmentNumber ConNo,\n" +
           "     c.consignerAccountNo AccountNo,\n" +
           "     c.riderCode,\n" +
           "     ct.name ConType,\n" +
           "\n" +
           "     b2.name Branch,\n" +
           "     c.weight,\n" +
           "     c.serviceTypeName,\n" +
           "     c.discount,\n" +
           "     c.pieces,\n" +
           "     c.consignee,\n" +
           "     c.consigneePhoneNo ConsigneeCell,\n" +
           "     c.consigneeCNICNo ConsigneeCNIC,\n" +
           "     c.consigner,\n" +
           "     c.consignerCellNo ConsignerCell,\n" +
           "     c.consignerCNICNo ConsignerCNIC,\n" +
           "     c.couponNumber Coupon,\n" +
           "     c.decalaredValue DeclaredValue,\n" +
           "     c.PakageContents PackageContents,\n" +
           "     c.address Address,\n" +
           "     c.shipperAddress,\n" +
           "     c.bookingDate,\n" +
           "     b.name ORIGIN,\n" +
           "     c.originExpressCenter,\n" +
           "\n" +
           "     c.insuarancePercentage,\n" +
           "     c.totalAmount,\n" +
           "     c.chargedAmount,\n" +
           "     c.isInsured,\n" +
           "     c.dayType,c.gst\n" +
           "     from Consignment c\n" +
           "     --inner join Zones z\n" +
           "     --on z.zoneCode = c.zoneCode\n" +
           "     inner join Branches b\n" +
           "     on b.branchCode = c.orgin\n" +
           "     inner join Branches b2\n" +
           "     on b2.branchCode = c.destination\n" +
           "     left outer join ConsignmentType ct\n" +
           "     on ct.id = c.consignmentTypeId\n" +
           "\n" +
           "\n" +
           "      where c.consignmentNumber = '" + clvar.consignmentNo + "'";




            sqlString = "select ISNULL(mco.isRunsheetAllowed, 1) RunsheetAllowed,\n" +
           "       c.*,\n" +
           "       c.consignmentNumber ConNo,\n" +
           "       c.consignerAccountNo AccountNo,\n" +
           "       c.riderCode,\n" +
           "       ct.name ConType,\n" +
           "\n" +
           "       b2.name               Branch,\n" +
           "       c.weight,\n" +
           "       c.serviceTypeName,\n" +
           "       c.discount,\n" +
           "       c.pieces,\n" +
           "       c.consignee,\n" +
           "       c.consigneePhoneNo    ConsigneeCell,\n" +
           "       c.consigneeCNICNo     ConsigneeCNIC,\n" +
           "       c.consigner,\n" +
           "       c.consignerCellNo     ConsignerCell,\n" +
           "       c.consignerCNICNo     ConsignerCNIC,\n" +
           "       c.couponNumber        Coupon,\n" +
           "       c.decalaredValue      DeclaredValue,\n" +
           "       c.PakageContents      PackageContents,\n" +
           "       c.address             Address,\n" +
           "       c.shipperAddress,\n" +
           "       c.bookingDate,\n" +
           "       b.name                ORIGIN,\n" +
           "       c.originExpressCenter,\n" +
           "\n" +
           "       c.insuarancePercentage,\n" +
           "       c.totalAmount,\n" +
           "       c.chargedAmount,\n" +
           "       c.isInsured,\n" +
           "       c.dayType,\n" +
           "       c.gst, case when mp.Prefix is null and isnull(c.isapproved,0) = '0' then '1' else '0' end WRONGCN, ISNULL(mco.RTO, 0) RTO, '1' Removeable\n" +
           "  from Consignment c\n" +
           "--inner join Zones z\n" +
           "--on z.zoneCode = c.zoneCode\n" +
           " LEFT OUTER JOIN (SELECT DISTINCT mpcl.prefix, mpcl.length FROM MnP_ConsignmentLengths mpcl) mp\n" +
            "            ON  LEN(c.consignmentNumber) = mp.Length\n" +
            "            AND mp.Prefix = SUBSTRING(c.consignmentNumber, 0, LEN(mp.Prefix) + 1)" +
            "\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = c.orgin\n" +
           " inner join Branches b2\n" +
           "    on b2.branchCode = c.destination\n" +
           "  left outer join ConsignmentType ct\n" +
           "    on ct.id = c.consignmentTypeId\n" +
           "  left outer join mnp_consignmentOperations mco\n" +
           "    on mco.consignmentId = c.consignmentNumber\n" +
           "\n" +
           " where c.consignmentNumber = '" + clvar.consignmentNo + "'";


            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }
        protected void rbtn_formMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void txt_runsheetNumber_TextChanged(object sender, EventArgs e)
        {

            if (txt_runsheetNumber.Text.Trim() == "")
            {
                AlertMessage("Enter Runsheet Number", "Red");
                return;
            }
            clvar.RunsheetNumber = txt_runsheetNumber.Text.Trim();
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();

            DataTable runsheetHeader = GetRunsheetHeader(clvar);
            if (runsheetHeader != null)
            {
                if (runsheetHeader.Rows.Count > 0)
                {
                    if (runsheetHeader.Rows[0]["PODUpdated"].ToString() != "0")
                    {
                        AlertMessage("POD Already Updated for this Runsheet. Cannot Edit this Runsheet", "Red");
                        return;
                    }
                    txt_vehicleNumber.Text = runsheetHeader.Rows[0]["VehicleNumber"].ToString();
                    List<string> temp = new List<string>();
                    foreach (ListItem item in dd_vehicle.Items)
                    {
                        if (txt_vehicleNumber.Text.Trim().ToUpper() == item.Value.Trim().ToUpper() || item.Value.Trim().Replace("-", "").ToUpper() == txt_vehicleNumber.Text.Trim().Replace("-", "").ToUpper())
                        {
                            dd_vehicle.SelectedValue = item.Value;
                            temp.Add(item.Value);
                            break;
                        }
                    }

                    dd_vehicleType.SelectedValue = runsheetHeader.Rows[0]["VehicleType"].ToString();
                    txt_meterStart.Text = runsheetHeader.Rows[0]["MeterStart"].ToString();
                    txt_meterEnd.Text = runsheetHeader.Rows[0]["MeterEnd"].ToString();
                    dd_route.SelectedValue = runsheetHeader.Rows[0]["RouteCode"].ToString();
                    txt_routeCode.Text = runsheetHeader.Rows[0]["RouteCode"].ToString();
                    dd_route_SelectedIndexChanged(this, e);
                    txt_date.Text = runsheetHeader.Rows[0]["RunsheetDate"].ToString();
                    dd_runsheetType.SelectedValue = runsheetHeader.Rows[0]["RunsheetType"].ToString();

                    DataTable runsheetDetail = GetRunsheetDetail(clvar);
                    DataTable dt = ViewState["dt"] as DataTable;

                    if (runsheetDetail != null)
                    {
                        if (runsheetDetail.Rows.Count > 0)
                        {
                            dt = runsheetDetail;
                            ViewState["dt"] = dt;

                            DataView dv = runsheetDetail.AsDataView();
                            dv.Sort = "SortOrder";
                            gv_consignments.DataSource = dt;
                            gv_consignments.DataBind();

                        }
                    }
                }
                else
                {
                    AlertMessage("Invalid Runsheet Number", "Red");
                    return;
                }
            }
            else
            {
                AlertMessage("Runsheet Not Found Contact I.T. Support.", "Red");
                return;
            }

        }

        public DataTable GetRunsheetHeader(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sqlString =
            "SELECT r.runsheetNumber,\n" +
            "       FORMAT(r.runsheetDate, 'yyyy-MM-dd') runsheetDate, \n" +
            "       r.routeCode,\n" +
            "       rr.riderCode,\n" +
            "       r.runsheetType, r.VehicleNumber, ISNULL(r.VehicleType,0) VehicleType, r.MeterStart, r.MeterEnd,\n" +
            "       COUNT(rc.reason) PODUpdated\n" +
            "  FROM Runsheet r\n" +
            " INNER JOIN RiderRunsheet rr\n" +
            "    ON rr.runsheetNumber = r.runsheetNumber\n" +
            "  LEFT OUTER JOIN RunsheetConsignment rc\n" +
            "    ON r.runsheetNumber = rc.runsheetNumber\n" +
            "   AND rc.routeCode = r.routeCode\n" +
            "   AND rc.branchCode = r.branchCode\n" +
            "   AND rc.Reason IS NOT NULL\n" +
            " WHERE r.runsheetNumber = '" + clvar.RunsheetNumber + "'\n" +
            "   AND r.branchCode = '" + clvar.Branch + "'\n" +
            "\n" +
            " GROUP BY r.runsheetNumber,\n" +
            "          r.runsheetDate,\n" +
            "          r.routeCode,\n" +
            "          rr.riderCode,\n" +
            "          r.runsheetType,\n" +
            "          rc.Reason, r.VehicleNumber, r.VehicleType, r.MeterStart, r.MeterEnd\n" +
            " ORDER BY runsheetdate DESC";

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public DataTable GetRunsheetDetail(Cl_Variables clvar)
        {
            DataTable dt = new DataTable();

            string sqlString = "SELECT c.consignmentNumber ConNo,\n" +
            "       c.orgin ORIGIN,\n" +
            "       c.destination NAME,\n" +
            "       ct.name ConType,\n" +
            "       '0' isnew,\n" +
            "       ROW_NUMBER() OVER(ORDER BY rc.SortOrder DESC) SortOrder,\n" +
            "       '0' Removeable\n" +
            "  FROM Consignment c\n" +
            " INNER JOIN RunsheetConsignment rc\n" +
            "    ON rc.consignmentNumber = c.consignmentNumber\n" +
            "  LEFT OUTER JOIN ConsignmentType ct\n" +
            "    ON ct.id = c.consignmentTypeId\n" +
            " WHERE rc.runsheetNumber = '" + clvar.RunsheetNumber + "'\n" +
            "   AND rc.branchcode = '" + clvar.Branch + "'\n" +
            " ORDER BY rc.SortOrder";


            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(sqlString, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            Errorid.Text = message;
            Errorid.ForeColor = System.Drawing.Color.FromName(color);
            err_msg.Text = message;
            err_msg.ForeColor = System.Drawing.Color.FromName(color);

        }

        public DataSet Get_MasterVehicle(Cl_Variables clvar)
        {
            DataSet ds = new DataSet();
            string temp = "";
            try
            {
                string sql = "select v.*, isnull(v.vehicleType, 0) VehicleType_ from rvdbo.Vehicle v where v.IsActive = '1' order by 1";

                SqlConnection orcl = new SqlConnection(clvar.Strcon2());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sql, orcl);
                orcd.CommandType = CommandType.Text;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(ds);
                orcl.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dd_vehicle.Items.Clear();
                    dd_vehicle.Items.Add(new ListItem { Text = "Select Vehicle", Value = "0" });
                    dd_vehicle.DataSource = ds.Tables[0];
                    dd_vehicle.DataTextField = "MakeModel";
                    dd_vehicle.DataValueField = "VehicleCode";
                    dd_vehicle.DataBind();

                    vehicleTypes.DataSource = ds.Tables[0];
                    vehicleTypes.DataBind();
                }
                else
                {
                    dd_vehicle.Items.Clear();
                    dd_vehicle.Items.Add(new ListItem { Text = "Select Vehicle", Value = "0" });
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Vehicle Available')", true);


                }
            }
            catch (Exception Err)
            {

            }
            finally
            { }
            return ds;
        }
    }
}