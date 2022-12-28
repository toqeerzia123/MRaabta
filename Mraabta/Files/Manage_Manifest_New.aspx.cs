using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using System.Drawing;

namespace MRaabta.Files
{
    public partial class Manage_Manifest_New : System.Web.UI.Page
    {

        bayer_Function b_fun = new bayer_Function();
        Cl_Variables clvar = new Cl_Variables();
        Variable clvar_ = new Variable();
        Consignemnts con = new Consignemnts();
        CommonFunction fun = new CommonFunction();
        cl_Encryption enc = new cl_Encryption();
        Cl_Manifest mani = new Cl_Manifest();

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_date.Enabled = false;
            ErrorID.Text = "";
            if (!IsPostBack)
            {
                GetReceivingStatus();
                GetOrigin();
                GetDestination();
                GetServiceTypes();

                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");

                if (Request.QueryString.Count > 0)
                {
                    dd_origin.Enabled = true;
                    rbtn_search.Visible = false;
                    dd_destination.SelectedValue = HttpContext.Current.Session["BranchCode"].ToString();
                    dd_destination.Enabled = false;
                    txt_manifestNo.Text = Request.QueryString["mode"];
                    txt_manifestNo_TextChanged(this, e);
                }
                else
                {
                    dd_destination.Enabled = true;
                    dd_origin.Enabled = false;
                }
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("ConsignmentNumber"),
                    new DataColumn("Orgin"),
                    new DataColumn("OriginName"),
                    new DataColumn("Destination"),
                    new DataColumn("DestinationName"),
                    new DataColumn("ConType"),
                    new DataColumn("ServiceTypeName"),
                    new DataColumn("Weight"),
                    new DataColumn("Pieces"),
                    new DataColumn("ISMODIFIED"),
                    new DataColumn("Order")

                });
                dt.AcceptChanges();

                // Manifest
                DataTable dt_Manifest = new DataTable();
                dt_Manifest.Columns.Add("consignmentNumber", typeof(string));
                dt_Manifest.Columns.Add("manifestNumber", typeof(string));
                dt_Manifest.Columns.Add("statusCode", typeof(string));
                dt_Manifest.Columns.Add("reason", typeof(string));
                dt_Manifest.Columns.Add("DeManifestStateID", typeof(string));
                dt_Manifest.Columns.Add("Remarks", typeof(string));

                //Consignment
                DataTable dt_ConsignmentTracking = new DataTable();
                dt_ConsignmentTracking.Columns.Add("consignmentNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("stateID", typeof(int));
                dt_ConsignmentTracking.Columns.Add("currentLocation", typeof(string));
                dt_ConsignmentTracking.Columns.Add("manifestNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("bagNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("loadingNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("mawbNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("runsheetNumber", typeof(string));
                dt_ConsignmentTracking.Columns.Add("riderName", typeof(string));
                dt_ConsignmentTracking.Columns.Add("transactionTime", typeof(DateTime));
                dt_ConsignmentTracking.Columns.Add("reason", typeof(string));
                dt_ConsignmentTracking.Columns.Add("statusTime", typeof(DateTime));
                dt_ConsignmentTracking.Columns.Add("mappingNo", typeof(string));
                dt_ConsignmentTracking.Columns.Add("isDeleted", typeof(string));
                dt_ConsignmentTracking.Columns.Add("internationalRemarks", typeof(string));
                dt_ConsignmentTracking.Columns.Add("SealNo", typeof(string));
                dt_ConsignmentTracking.Columns.Add("VanNo", typeof(string));
                dt_ConsignmentTracking.Columns.Add("MaterialArrival", typeof(string));
                dt_ConsignmentTracking.Columns.Add("ArrivalID", typeof(string));

                //Consignment
                DataTable dt_Consignment = new DataTable();
                dt_Consignment.Columns.Add("consignmentNumber", typeof(string));
                dt_Consignment.Columns.Add("serviceTypeName", typeof(string));
                dt_Consignment.Columns.Add("consignmentTypeId", typeof(string));
                dt_Consignment.Columns.Add("WEIGHT", typeof(string));
                dt_Consignment.Columns.Add("orgin", typeof(string));
                dt_Consignment.Columns.Add("destination", typeof(string));
                dt_Consignment.Columns.Add("createdon", typeof(DateTime));
                dt_Consignment.Columns.Add("createdby", typeof(string));
                dt_Consignment.Columns.Add("pieces", typeof(string));
                dt_Consignment.Columns.Add("creditClientId", typeof(string));
                dt_Consignment.Columns.Add("zoneCode", typeof(string));
                dt_Consignment.Columns.Add("branchCode", typeof(string));
                dt_Consignment.Columns.Add("consignerAccountNo", typeof(string));
                dt_Consignment.Columns.Add("bookingDate", typeof(DateTime));
                dt_Consignment.Columns.Add("riderCode", typeof(string));


                ViewState["temp"] = dt;
                ViewState["dt_Manifest"] = dt_Manifest;
                ViewState["dt_consignment"] = dt_Consignment;
                ViewState["dt_consigmnettracking"] = dt_ConsignmentTracking;


                ViewState["types"] = fun.ConsignmentType().Tables[0];
                GetCNLengths();
            }
        }

        protected void GetOrigin()
        {
            if (Session["BranchCode"] != null)
            {
                DataTable dt = new DataTable();



                dt = Cities_();// fun.Branch().Tables[0];

                //DataView dv = dt.AsDataView();
                //dv.Sort = "BranchName";
                //dt = dv.ToTable();
                dd_origin.DataSource = dt;
                dd_origin.DataTextField = "SNAME";
                dd_origin.DataValueField = "branchCode";
                if (Session["BranchCode"].ToString() == "ALL")
                {
                    dd_origin.Enabled = true;
                }
                else
                {
                    dd_origin.Enabled = false;
                    try
                    {
                        dd_origin.SelectedValue = Session["BranchCode"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("~/login");
                    }
                }
                dd_origin.DataBind();


            }
            else
            {
                Response.Redirect("~/login");
            }
        }
        protected void GetDestination()
        {
            DataTable dt = new DataTable();
            dt = Cities_(); //fun.Branch().Tables[0];
            DataView dv = dt.AsDataView();
            dt = dv.ToTable();
            dv.Sort = "SNAME";
            dd_destination.DataSource = dt;
            dd_destination.DataTextField = "SNAME";
            dd_destination.DataValueField = "branchCode";
            dd_destination.DataBind();


            ViewState["destinations"] = dt;
        }
        protected void GetServiceTypes()
        {
            DataTable dt = new DataTable();
            dt = fun.ServiceTypeNameRvdbo();
            dd_serviceType.DataSource = dt;
            dd_serviceType.DataTextField = "ServiceTypeName";
            dd_serviceType.DataValueField = "ServiceTypeID";
            dd_serviceType.DataBind();
            //dd_serviceType.SelectedValue = "overnight";

            foreach (ListItem item in dd_serviceType.Items)
            {
                if (item.Text.ToUpper() == "OVERNIGHT")
                {
                    item.Selected = true;
                    break;
                }
            }

            ViewState["serviceTypes"] = dt;

        }
        protected void GetReceivingStatus()
        {
            DataTable dt = fun.GetReceivingStatus();

            ViewState["ReceivingStatus"] = dt;
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
            cnControls.DataSource = dt;
            cnControls.DataBind();
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            DataTable dt_ = ViewState["temp"] as DataTable;
            dt_.Clear();
            btn_save.Text = "Save";
            btn_save.CommandName = "";
            rbtn_search.SelectedValue = "1";
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
            txt_consignmentNo.Text = "";
            txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txt_manifestNo.Text = "";
            dd_destination.ClearSelection();
            dd_serviceType.ClearSelection();

            lbl_count.Text = "";
            GetReceivingStatus();
            GetOrigin();
            GetDestination();
            GetServiceTypes();

            txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (Request.QueryString.Count > 0)
            {
                dd_origin.Enabled = true;
                rbtn_search.Visible = false;
            }
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("ConsignmentNumber"),
                    new DataColumn("Orgin"),
                    new DataColumn("OriginName"),
                    new DataColumn("Destination"),
                    new DataColumn("DestinationName"),
                    new DataColumn("ConType"),
                    new DataColumn("ServiceTypeName"),
                    new DataColumn("Weight"),
                    new DataColumn("Pieces"),
                    new DataColumn("ISMODIFIED"),
                    new DataColumn("Order")

                });
            dt.AcceptChanges();

            // Manifest
            DataTable dt_Manifest = new DataTable();
            dt_Manifest.Columns.Add("consignmentNumber", typeof(string));
            dt_Manifest.Columns.Add("manifestNumber", typeof(string));
            dt_Manifest.Columns.Add("statusCode", typeof(string));
            dt_Manifest.Columns.Add("reason", typeof(string));
            dt_Manifest.Columns.Add("DeManifestStateID", typeof(string));
            dt_Manifest.Columns.Add("Remarks", typeof(string));

            //Consignment
            DataTable dt_ConsignmentTracking = new DataTable();
            dt_ConsignmentTracking.Columns.Add("consignmentNumber", typeof(string));
            dt_ConsignmentTracking.Columns.Add("stateID", typeof(int));
            dt_ConsignmentTracking.Columns.Add("currentLocation", typeof(string));
            dt_ConsignmentTracking.Columns.Add("manifestNumber", typeof(string));
            dt_ConsignmentTracking.Columns.Add("bagNumber", typeof(string));
            dt_ConsignmentTracking.Columns.Add("loadingNumber", typeof(string));
            dt_ConsignmentTracking.Columns.Add("mawbNumber", typeof(string));
            dt_ConsignmentTracking.Columns.Add("runsheetNumber", typeof(string));
            dt_ConsignmentTracking.Columns.Add("riderName", typeof(string));
            dt_ConsignmentTracking.Columns.Add("transactionTime", typeof(DateTime));
            dt_ConsignmentTracking.Columns.Add("reason", typeof(string));
            dt_ConsignmentTracking.Columns.Add("statusTime", typeof(DateTime));
            dt_ConsignmentTracking.Columns.Add("mappingNo", typeof(string));
            dt_ConsignmentTracking.Columns.Add("isDeleted", typeof(string));
            dt_ConsignmentTracking.Columns.Add("internationalRemarks", typeof(string));
            dt_ConsignmentTracking.Columns.Add("SealNo", typeof(string));
            dt_ConsignmentTracking.Columns.Add("VanNo", typeof(string));
            dt_ConsignmentTracking.Columns.Add("MaterialArrival", typeof(string));
            dt_ConsignmentTracking.Columns.Add("ArrivalID", typeof(string));

            //Consignment
            DataTable dt_Consignment = new DataTable();
            dt_Consignment.Columns.Add("consignmentNumber", typeof(string));
            dt_Consignment.Columns.Add("serviceTypeName", typeof(string));
            dt_Consignment.Columns.Add("consignmentTypeId", typeof(string));
            dt_Consignment.Columns.Add("WEIGHT", typeof(string));
            dt_Consignment.Columns.Add("orgin", typeof(string));
            dt_Consignment.Columns.Add("destination", typeof(string));
            dt_Consignment.Columns.Add("createdon", typeof(DateTime));
            dt_Consignment.Columns.Add("createdby", typeof(string));
            dt_Consignment.Columns.Add("pieces", typeof(string));
            dt_Consignment.Columns.Add("creditClientId", typeof(string));
            dt_Consignment.Columns.Add("zoneCode", typeof(string));
            dt_Consignment.Columns.Add("branchCode", typeof(string));
            dt_Consignment.Columns.Add("consignerAccountNo", typeof(string));
            dt_Consignment.Columns.Add("bookingDate", typeof(DateTime));
            dt_Consignment.Columns.Add("riderCode", typeof(string));


            ViewState["temp"] = dt;
            ViewState["dt_Manifest"] = dt_Manifest;
            ViewState["dt_consignment"] = dt_Consignment;
            ViewState["dt_consigmnettracking"] = dt_ConsignmentTracking;


            ViewState["types"] = fun.ConsignmentType().Tables[0];

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                clvar.manifestNo = txt_manifestNo.Text;
                clvar.destination = dd_destination.SelectedValue;
                clvar.origin = dd_origin.SelectedValue;
                clvar.ServiceTypeName = dd_serviceType.SelectedItem.Text;
                clvar.serviceTypeId = dd_serviceType.SelectedValue;
                clvar.LoadingDate = txt_date.Text;
                #region Validations
                //if (txt_manifestNo.Text.Count() < 10)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Number Must be 12 Characters.')", true);
                //    return;
                //}
                if (dd_origin.SelectedValue == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Origin.')", true);
                    return;
                }
                if (dd_destination.SelectedValue == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Destination.')", true);
                    return;
                }
                //if (dd_destination.SelectedValue == dd_origin.SelectedValue)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Destination and origin cannot be same.')", true);
                //    return;
                //}
                if (dd_serviceType.SelectedValue == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Service Type.')", true);
                    return;
                }
                if (txt_manifestNo.Text.Trim(' ') == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Manifest Number.')", true);
                    return;
                }
                if (txt_date.Text.Trim(' ') == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter Date.')", true);
                    return;
                }
                #endregion
                DataTable dt_Manifest = ViewState["dt_Manifest"] as DataTable;
                DataTable dt_Consignment = ViewState["dt_consignment"] as DataTable;
                DataTable dt_ConsignmentTracking = ViewState["dt_consigmnettracking"] as DataTable;

                //return;
                if (Request.QueryString.Count > 0)
                {

                    #region Demanifest Without Manifest.
                    foreach (GridViewRow row in gv_consignments.Rows)
                    {
                        string cnNumber = row.Cells[1].Text;
                        DropDownList dd_gStatus = row.FindControl("dd_gStatus") as DropDownList;
                        DataRow dr = dt_Manifest.Select("consignmentNumber = '" + cnNumber + "'")[0];
                        dr["DemanifestStateID"] = dd_gStatus.SelectedValue;
                        dr.AcceptChanges();
                    }
                    string error = DemanifestWithoutManifest(clvar, dt_Consignment, dt_ConsignmentTracking, dt_Manifest);
                    if (error.ToUpper() == "OK")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Demanifest Successful')", true);
                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                        string script = String.Format(script_, "MNPManifest_Print.aspx?Xcode=" + txt_manifestNo.Text, "_blank", "");
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                    }
                    #endregion
                }
                else
                {
                    #region Manifest
                    string error = "";
                    if (rbtn_search.SelectedValue == "1") //New Manifest
                    {
                        clvar.manifestNo = txt_manifestNo.Text;
                        clvar.Day = txt_date.Text;
                        clvar.CheckCondition = dd_serviceType.SelectedItem.Text;
                        clvar.origin = dd_origin.SelectedValue;
                        clvar.destination = dd_destination.SelectedValue;
                        error = NewManifest(clvar, dt_Consignment, dt_ConsignmentTracking, dt_Manifest);

                    }
                    else if (rbtn_search.SelectedValue == "3") //Update Manifest.
                    {
                        dt_Consignment.Clear();
                        dt_ConsignmentTracking.Clear();
                        dt_Manifest.Clear();

                        foreach (GridViewRow rows in gv_consignments.Rows)
                        {
                            string modified = (rows.FindControl("hd_isModified") as HiddenField).Value;
                            HiddenField origin = rows.FindControl("hd_origin") as HiddenField;
                            HiddenField dest = rows.FindControl("hd_destination") as HiddenField;
                            Label serviceType = rows.FindControl("hd_serviceType") as Label;
                            TextBox weight = rows.FindControl("txt_gWeight") as TextBox;
                            TextBox pieces = rows.FindControl("txt_gPieces") as TextBox;

                            if (modified.ToUpper() == "NEW")
                            {
                                #region Adding in Consignment Table Type
                                DataRow dr_ = dt_Consignment.NewRow();
                                dr_["consignmentNumber"] = rows.Cells[1].Text;
                                dr_["serviceTypeName"] = serviceType.Text;
                                dr_["consignmentTypeId"] = "12";
                                dr_["WEIGHT"] = "0.5";
                                dr_["orgin"] = Session["BranchCode"].ToString();
                                dr_["destination"] = dd_destination.SelectedValue;
                                dr_["createdon"] = DateTime.Now;
                                dr_["createdby"] = HttpContext.Current.Session["U_ID"].ToString();
                                dr_["pieces"] = "1";
                                dr_["creditClientId"] = "330140";
                                dr_["zoneCode"] = Session["zoneCode"].ToString();
                                dr_["branchCode"] = Session["BranchCode"].ToString();
                                dr_["consignerAccountNo"] = "4D1";
                                dr_["bookingDate"] = DateTime.Now;
                                dt_Consignment.Rows.Add(dr_);
                                #endregion

                                #region Adding in Tracking Table Type
                                DataRow dtr = dt_ConsignmentTracking.NewRow();
                                dtr["consignmentNumber"] = txt_consignmentNo.Text;
                                dtr["stateID"] = "2";
                                foreach (ListItem item in dd_origin.Items)
                                {
                                    if (item.Value == HttpContext.Current.Session["BranchCode"].ToString())
                                    {
                                        dtr["currentLocation"] = item.Text;
                                        break;
                                    }
                                }
                                //dtr["currentLocation"] = dd_origin.SelectedItem.Text;
                                dtr["manifestNumber"] = txt_manifestNo.Text;
                                dtr["bagNumber"] = "";
                                dtr["loadingNumber"] = "";
                                dtr["mawbNumber"] = "";
                                dtr["runsheetNumber"] = "";
                                dtr["riderName"] = "";
                                dtr["transactionTime"] = DateTime.Now;
                                dtr["reason"] = "";
                                dtr["statusTime"] = DBNull.Value;
                                dtr["mappingNo"] = "";
                                dtr["isDeleted"] = "";
                                dtr["internationalRemarks"] = "";
                                dtr["SealNo"] = "";
                                dtr["VanNo"] = "";
                                dtr["MaterialArrival"] = "";
                                dtr["ArrivalID"] = "";
                                dt_ConsignmentTracking.Rows.Add(dtr);
                                #endregion
                            }
                        }

                        error = mani.UpdateManifest(clvar, dt_Consignment, dt_ConsignmentTracking, dt_Manifest);
                    }
                    #endregion


                    if (error != "OK")
                    {

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Generate Manifest. \\nPlease Contact I.T. Support ')", true);
                        ErrorID.Text = "Could Not Generate Manifest. \\nPlease Contact I.T. Support ";
                        ErrorID.ForeColor = Color.Red;
                    }
                    else
                    {

                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                        string script = String.Format(script_, "MNPManifest_Print.aspx?Xcode=" + txt_manifestNo.Text, "_blank", "");
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Generated')", true);
                        btn_reset_Click(sender, e);
                        return;

                    }
                }
            }
            catch (Exception ex)
            {

                ErrorID.Text = ex.Message;

            }
        }
        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void txt_consignmentNo_TextChanged(object sender, EventArgs e)
        {
            //if (txt_consignmentNo.Text.Length < 16)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Cannot be Less than 12 Digits')", true);
            //    txt_consignmentNo.Text = "";
            //    return;
            //}

            clvar.consignmentNo = txt_consignmentNo.Text;
            if (!IsNumeric(clvar.consignmentNo))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                return;
            }

            DataTable cnLength = ViewState["cnLengths"] as DataTable;
            bool prefixFound = false;
            if (cnLength != null)
            {
                if (cnLength.Rows.Count > 0)
                {
                    foreach (DataRow d in cnLength.Rows)
                    {
                        if (d[1].ToString().Length > txt_consignmentNo.Text.Length)
                        {
                            continue;
                        }
                        if (d[1].ToString() == txt_consignmentNo.Text.Substring(0, d[1].ToString().Length))
                        {
                            if (d[3].ToString() == txt_consignmentNo.Text.Length.ToString())
                            {
                                prefixFound = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!prefixFound)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Consignment Lengnth or Prefix.')", true);
                ErrorID.Text = "Invalid Consignment Lengnth or Prefix.";
                return;
            }
            if (dd_serviceType.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Service Type')", true);
                txt_consignmentNo.Text = "";
                return;
            }
            if (dd_destination.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
                txt_consignmentNo.Text = "";
                return;
            }

            clvar.consignmentNo = txt_consignmentNo.Text;
            clvar.ServiceTypeName = dd_serviceType.SelectedItem.Text;

            clvar.origin = dd_origin.SelectedValue;
            clvar.destination = dd_destination.SelectedValue;
            DataTable types = ViewState["types"] as DataTable;
            DataTable destinations = ViewState["destinations"] as DataTable;
            DataTable dt_ = GetConsignmentDetailForNewManifest_(clvar);
            //DataTable dt = dt_.Clone();// ViewState["temp"] as DataTable;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("ConsignmentNumber"),
                    new DataColumn("Orgin"),
                    new DataColumn("OriginName"),
                    new DataColumn("Destination"),
                    new DataColumn("DestinationName"),
                    new DataColumn("ConsignmentTypeID"),
                    new DataColumn("ServiceTypeName"),
                    new DataColumn("Weight"),
                    new DataColumn("Pieces"),
                    new DataColumn("ISMODIFIED"),
                    new DataColumn("Order")

                });
            dt.AcceptChanges();
            int count = 0;

            DataTable dt_Manifest = ViewState["dt_Manifest"] as DataTable;
            DataTable dt_Consignment = ViewState["dt_consignment"] as DataTable;
            DataTable dt_ConsignmentTracking = ViewState["dt_consigmnettracking"] as DataTable;


            foreach (GridViewRow row in gv_consignments.Rows)
            {
                DataRow dr_ = dt.NewRow();
                clvar.consignmentNo = row.Cells[1].Text;

                dr_[0] = row.Cells[1].Text;
                dr_[1] = (row.FindControl("dd_gorigin") as DropDownList).SelectedValue;
                dr_[2] = (row.FindControl("dd_gorigin") as DropDownList).SelectedItem.Text;
                dr_[3] = (row.FindControl("hd_destination") as HiddenField).Value;
                dr_[4] = (row.FindControl("lbl_destination") as Label).Text;
                dr_[5] = (row.FindControl("dd_contype") as DropDownList).SelectedValue;
                dr_[6] = (row.FindControl("hd_serviceType") as Label).Text;
                dr_[7] = (row.FindControl("txt_gWeight") as TextBox).Text;
                dr_[8] = (row.FindControl("txt_gPieces") as TextBox).Text;
                dr_[9] = (row.FindControl("hd_isModified") as HiddenField).Value;
                dr_["ORDER"] = row.RowIndex;
                dt.Rows.Add(dr_);
                dt.AcceptChanges();
            }
            if (gv_consignments.Rows.Count > 0)
            {
                count = int.Parse((dt.Compute("MAX(ORDER)", "")).ToString());
            }

            if (dt.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate //Consignments.')", true);
                txt_consignmentNo.Text = "";
                return;
            }
            DataRow dr = dt.NewRow();

            if (Request.QueryString.Count > 0)
            {
                #region Demanifest Without Manifest
                if (dt_ != null)
                {
                    if (dt_.Rows.Count > 0) //Consignment is found
                    {
                        dr["ConsignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                        dr["Orgin"] = dt_.Rows[0]["Orgin"].ToString();
                        dr["OriginName"] = dt_.Rows[0]["OriginName"].ToString();
                        dr["Destination"] = dt_.Rows[0]["Destination"].ToString();
                        dr["DestinationName"] = dt_.Rows[0]["DestinationName"].ToString();
                        dr["ConsignmentTypeID"] = dt_.Rows[0]["ConsignmentTypeID"].ToString();
                        dr["ServiceTypeName"] = dt_.Rows[0]["ServiceTypeName"].ToString();
                        dr["Weight"] = dt_.Rows[0]["Weight"].ToString();
                        dr["Pieces"] = dt_.Rows[0]["Pieces"].ToString();
                        dr["Ismodified"] = "INSERT";
                        dr["Order"] = count;
                        dt.Rows.Add(dr);


                        #region Adding in Tracking Table Type
                        DataRow dtr = dt_ConsignmentTracking.NewRow();
                        dtr["consignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                        dtr["stateID"] = "7";
                        foreach (ListItem item in dd_origin.Items)
                        {
                            if (item.Value == HttpContext.Current.Session["BranchCode"].ToString())
                            {
                                dtr["currentLocation"] = item.Text;
                                break;
                            }
                        }
                        //dtr["currentLocation"] = dd_origin.SelectedItem.Text;
                        dtr["manifestNumber"] = txt_manifestNo.Text;
                        dtr["bagNumber"] = "";
                        dtr["loadingNumber"] = "";
                        dtr["mawbNumber"] = "";
                        dtr["runsheetNumber"] = "";
                        dtr["riderName"] = "";
                        dtr["transactionTime"] = DateTime.Now;
                        dtr["reason"] = "";
                        dtr["statusTime"] = DBNull.Value;
                        dtr["mappingNo"] = "";
                        dtr["isDeleted"] = "";
                        dtr["internationalRemarks"] = "";
                        dtr["SealNo"] = "";
                        dtr["VanNo"] = "";
                        dtr["MaterialArrival"] = "";
                        dtr["ArrivalID"] = "";
                        dt_ConsignmentTracking.Rows.Add(dtr);
                        #endregion

                        #region Adding in Manifest Table Type
                        DataRow dm = dt_Manifest.NewRow();
                        dm["consignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                        dm["manifestNumber"] = txt_manifestNo.Text;
                        dm["statusCode"] = "";
                        dm["reason"] = "";
                        dm["DeManifestStateID"] = "";
                        dm["Remarks"] = "";
                        dt_Manifest.Rows.Add(dm);
                        #endregion

                        ViewState["temp"] = dt;
                        ViewState["dt_consignment"] = dt_Consignment;
                        ViewState["dt_Manifest"] = dt_Manifest;
                        ViewState["dt_consigmnettracking"] = dt_ConsignmentTracking;

                        gv_consignments.DataSource = dt;
                        gv_consignments.DataBind();
                        lbl_count.Text = dt.Rows.Count.ToString();
                    }
                    else //Consignment Not Found
                    {
                        dr["ConsignmentNumber"] = txt_consignmentNo.Text;
                        dr["Orgin"] = Session["BranchCode"].ToString();
                        dr["OriginName"] = "";
                        dr["Destination"] = dd_destination.SelectedValue;
                        dr["DestinationName"] = dd_destination.SelectedItem.Text;
                        dr["ConsignmentTypeID"] = "12";
                        dr["ServiceTypeName"] = dd_serviceType.SelectedItem.Text;
                        dr["Weight"] = "0.5";
                        dr["Pieces"] = "1";
                        dr["Ismodified"] = "NEW";
                        dr["Order"] = count;
                        dt.Rows.Add(dr);

                        #region Adding in Consignment Table Type
                        DataRow dr_ = dt_Consignment.NewRow();
                        dr_["consignmentNumber"] = txt_consignmentNo.Text;
                        dr_["serviceTypeName"] = dd_serviceType.SelectedItem.Text;
                        dr_["consignmentTypeId"] = "12";
                        dr_["WEIGHT"] = "0.5";
                        dr_["orgin"] = Session["BranchCode"].ToString();
                        dr_["destination"] = dd_destination.SelectedValue;
                        dr_["createdon"] = DateTime.Now;
                        dr_["createdby"] = HttpContext.Current.Session["U_ID"].ToString();
                        dr_["pieces"] = "1";
                        dr_["creditClientId"] = "330140";
                        dr_["zoneCode"] = Session["zoneCode"].ToString();
                        dr_["branchCode"] = Session["BranchCode"].ToString();
                        dr_["consignerAccountNo"] = "4D1";
                        dr_["bookingDate"] = DateTime.Now;
                        dt_Consignment.Rows.Add(dr_);
                        #endregion

                        #region Adding in Tracking Table Type
                        DataRow dtr = dt_ConsignmentTracking.NewRow();
                        dtr["consignmentNumber"] = txt_consignmentNo.Text;
                        dtr["stateID"] = "3";
                        foreach (ListItem item in dd_origin.Items)
                        {
                            if (item.Value == HttpContext.Current.Session["BranchCode"].ToString())
                            {
                                dtr["currentLocation"] = item.Text;
                                break;
                            }
                        }
                        //dtr["currentLocation"] = dd_origin.SelectedItem.Text;
                        dtr["manifestNumber"] = txt_manifestNo.Text;
                        dtr["bagNumber"] = "";
                        dtr["loadingNumber"] = "";
                        dtr["mawbNumber"] = "";
                        dtr["runsheetNumber"] = "";
                        dtr["riderName"] = "";
                        dtr["transactionTime"] = DateTime.Now;
                        dtr["reason"] = "";
                        dtr["statusTime"] = DBNull.Value;
                        dtr["mappingNo"] = "";
                        dtr["isDeleted"] = "";
                        dtr["internationalRemarks"] = "";
                        dtr["SealNo"] = "";
                        dtr["VanNo"] = "";
                        dtr["MaterialArrival"] = "";
                        dtr["ArrivalID"] = "";
                        dt_ConsignmentTracking.Rows.Add(dtr);
                        #endregion

                        #region Adding in Manifest Table Type
                        DataRow dm = dt_Manifest.NewRow();
                        dm["consignmentNumber"] = txt_consignmentNo.Text;
                        dm["manifestNumber"] = txt_manifestNo.Text;
                        dm["statusCode"] = "";
                        dm["reason"] = "";
                        dm["DeManifestStateID"] = "";
                        dm["Remarks"] = "";
                        dt_Manifest.Rows.Add(dm);
                        #endregion

                        ViewState["temp"] = dt;
                        ViewState["dt_consignment"] = dt_Consignment;
                        ViewState["dt_Manifest"] = dt_Manifest;
                        ViewState["dt_consigmnettracking"] = dt_ConsignmentTracking;

                        gv_consignments.DataSource = dt;
                        gv_consignments.DataBind();
                        lbl_count.Text = dt.Rows.Count.ToString();
                    }

                    txt_consignmentNo.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not find Consignment.')", true);
                    return;
                }
                #endregion
            }
            else
            {
                #region Manifest

                if (dt_ != null)
                {
                    if (dt_.Rows.Count > 0) //Consignment is found
                    {
                        dr["ConsignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                        dr["Orgin"] = dt_.Rows[0]["Orgin"].ToString();
                        dr["OriginName"] = dt_.Rows[0]["OriginName"].ToString();
                        dr["Destination"] = dt_.Rows[0]["Destination"].ToString();
                        dr["DestinationName"] = dt_.Rows[0]["DestinationName"].ToString();
                        dr["ConsignmentTypeID"] = dt_.Rows[0]["ConsignmentTypeID"].ToString();
                        dr["ServiceTypeName"] = dt_.Rows[0]["ServiceTypeName"].ToString();
                        dr["Weight"] = dt_.Rows[0]["Weight"].ToString();
                        dr["Pieces"] = dt_.Rows[0]["Pieces"].ToString();
                        dr["Ismodified"] = "INSERT";
                        dr["Order"] = count;
                        dt.Rows.Add(dr);

                        #region Adding in Tracking Table Type
                        DataRow dtr = dt_ConsignmentTracking.NewRow();
                        dtr["consignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                        dtr["stateID"] = "2";
                        foreach (ListItem item in dd_origin.Items)
                        {
                            if (item.Value == HttpContext.Current.Session["BranchCode"].ToString())
                            {
                                dtr["currentLocation"] = item.Text;
                                break;
                            }
                        }
                        //dtr["currentLocation"] = dd_origin.SelectedItem.Text;
                        dtr["manifestNumber"] = txt_manifestNo.Text;
                        dtr["bagNumber"] = "";
                        dtr["loadingNumber"] = "";
                        dtr["mawbNumber"] = "";
                        dtr["runsheetNumber"] = "";
                        dtr["riderName"] = "";
                        dtr["transactionTime"] = DateTime.Now;
                        dtr["reason"] = "";
                        dtr["statusTime"] = DBNull.Value;
                        dtr["mappingNo"] = "";
                        dtr["isDeleted"] = "";
                        dtr["internationalRemarks"] = "";
                        dtr["SealNo"] = "";
                        dtr["VanNo"] = "";
                        dtr["MaterialArrival"] = "";
                        dtr["ArrivalID"] = "";
                        dt_ConsignmentTracking.Rows.Add(dtr);
                        #endregion

                        #region Adding in Manifest Table Type
                        DataRow dm = dt_Manifest.NewRow();
                        dm["consignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                        dm["manifestNumber"] = txt_manifestNo.Text;
                        dm["statusCode"] = "";
                        dm["reason"] = "";
                        dm["DeManifestStateID"] = "";
                        dm["Remarks"] = "";
                        dt_Manifest.Rows.Add(dm);
                        #endregion

                        ViewState["temp"] = dt;
                        ViewState["dt_consignment"] = dt_Consignment;
                        ViewState["dt_Manifest"] = dt_Manifest;
                        ViewState["dt_consigmnettracking"] = dt_ConsignmentTracking;

                        gv_consignments.DataSource = dt;
                        gv_consignments.DataBind();
                    }
                    else //Consignment Not Found
                    {
                        dr["ConsignmentNumber"] = txt_consignmentNo.Text;
                        dr["Orgin"] = Session["BranchCode"].ToString();
                        dr["OriginName"] = "";
                        dr["Destination"] = dd_destination.SelectedValue;
                        dr["DestinationName"] = dd_destination.SelectedItem.Text;
                        dr["ConsignmentTypeID"] = "12";
                        dr["ServiceTypeName"] = dd_serviceType.SelectedItem.Text;
                        dr["Weight"] = "0.5";
                        dr["Pieces"] = "1";
                        dr["Ismodified"] = "NEW";
                        dr["Order"] = count;
                        dt.Rows.Add(dr);

                        #region Adding in Consignment Table Type
                        DataRow dr_ = dt_Consignment.NewRow();
                        dr_["consignmentNumber"] = txt_consignmentNo.Text;
                        dr_["serviceTypeName"] = dd_serviceType.SelectedItem.Text;
                        dr_["consignmentTypeId"] = "12";
                        dr_["WEIGHT"] = "0.5";
                        dr_["orgin"] = Session["BranchCode"].ToString();
                        dr_["destination"] = dd_destination.SelectedValue;
                        dr_["createdon"] = DateTime.Now;
                        dr_["createdby"] = HttpContext.Current.Session["U_ID"].ToString();
                        dr_["pieces"] = "1";
                        dr_["creditClientId"] = "330140";
                        dr_["zoneCode"] = Session["zoneCode"].ToString();
                        dr_["branchCode"] = Session["BranchCode"].ToString();
                        dr_["consignerAccountNo"] = "4D1";
                        dr_["bookingDate"] = DateTime.Now;
                        dt_Consignment.Rows.Add(dr_);
                        #endregion

                        #region Adding in Tracking Table Type
                        DataRow dtr = dt_ConsignmentTracking.NewRow();
                        dtr["consignmentNumber"] = txt_consignmentNo.Text;
                        dtr["stateID"] = "2";
                        foreach (ListItem item in dd_origin.Items)
                        {
                            if (item.Value == HttpContext.Current.Session["BranchCode"].ToString())
                            {
                                dtr["currentLocation"] = item.Text;
                                break;
                            }
                        }
                        //dtr["currentLocation"] = dd_origin.SelectedItem.Text;
                        dtr["manifestNumber"] = txt_manifestNo.Text;
                        dtr["bagNumber"] = "";
                        dtr["loadingNumber"] = "";
                        dtr["mawbNumber"] = "";
                        dtr["runsheetNumber"] = "";
                        dtr["riderName"] = "";
                        dtr["transactionTime"] = DateTime.Now;
                        dtr["reason"] = "";
                        dtr["statusTime"] = DBNull.Value;
                        dtr["mappingNo"] = "";
                        dtr["isDeleted"] = "";
                        dtr["internationalRemarks"] = "";
                        dtr["SealNo"] = "";
                        dtr["VanNo"] = "";
                        dtr["MaterialArrival"] = "";
                        dtr["ArrivalID"] = "";
                        dt_ConsignmentTracking.Rows.Add(dtr);
                        #endregion

                        #region Adding in Manifest Table Type
                        DataRow dm = dt_Manifest.NewRow();
                        dm["consignmentNumber"] = txt_consignmentNo.Text;
                        dm["manifestNumber"] = txt_manifestNo.Text;
                        dm["statusCode"] = "";
                        dm["reason"] = "";
                        dm["DeManifestStateID"] = "";
                        dm["Remarks"] = "";
                        dt_Manifest.Rows.Add(dm);
                        #endregion

                        ViewState["temp"] = dt;
                        ViewState["dt_consignment"] = dt_Consignment;
                        ViewState["dt_Manifest"] = dt_Manifest;
                        ViewState["dt_consigmnettracking"] = dt_ConsignmentTracking;

                        gv_consignments.DataSource = dt;
                        gv_consignments.DataBind();
                    }

                    txt_consignmentNo.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could not find Consignment.')", true);
                    return;
                }
                #endregion
            }


            txt_consignmentNo.Focus();

            #region IF CONSIGNMENT NOT EXISTS IN DB
            //if (dt_.Rows.Count == 0)
            //{
            //    if (txt_consignmentNo.Text.Trim() != "")
            //    {
            //        if (dt.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
            //        {
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate //Consignments.')", true);
            //            txt_consignmentNo.Text = "";
            //            return;
            //        }
            //        else
            //        {
            //            dr["ConsignmentNumber"] = txt_consignmentNo.Text;
            //            dr["Orgin"] = Session["BranchCode"].ToString();
            //            dr["OriginName"] = "";
            //            dr["Destination"] = dd_destination.SelectedValue;
            //            dr["DestinationName"] = dd_destination.SelectedItem.Text;
            //            dr["ConsignmentTypeID"] = "12";
            //            dr["ServiceTypeName"] = dd_serviceType.SelectedItem.Text;
            //            dr["Weight"] = "0.5";
            //            dr["Pieces"] = "1";
            //            if (Request.QueryString.Count > 0)
            //            {
            //                dr["ISMODIFIED"] = "NEW CONSIGNMENT";
            //            }
            //            else
            //            {
            //                if (rbtn_search.SelectedValue == "3")
            //                {
            //                    dr["ISMODIFIED"] = "INSERT";
            //                }
            //                else
            //                {
            //                    dr["ISMODIFIED"] = "NEW CONSIGNMENT";
            //                }
            //            }


            //            dr["Order"] = count.ToString();
            //            count++;

            //            dt.Rows.Add(dr);
            //            dt.AcceptChanges();

            //            // ManifestCOnsignment
            //            //dt_Manifest.Rows.Add(txt_consignmentNo.Text, "", "", "", "", "");
            //            //dt_ConsignmentTracking.Rows.Add(txt_consignmentNo.Text, "18", b_fun.Get_Branches(clvar_).Tables[0].Rows[0]["name"].ToString(), "", "", "", "", "", "", DateTime.Now, "", DateTime.Now, "", "0", "", "", "", "", "");


            //            ViewState["temp"] = null;
            //            ViewState["temp"] = dt;


            //            if (dt.Rows.Count > 0)
            //            {
            //                lbl_count.Text = dt.Rows.Count.ToString();
            //                dt.AsDataView().Sort = "Order asc";
            //                gv_consignments.DataSource = dt;
            //                gv_consignments.DataBind();
            //                txt_consignmentNo.Text = "";
            //                txt_consignmentNo.Focus();
            //            }
            //        }
            //    }
            //    //txt_consignmentNo.Text = "";
            //    txt_consignmentNo.Focus();
            //    return;
            //} 
            #endregion

            #region IF CONSIGNMENT EXISTS IN DB
            //if (dt_.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
            //{
            //    if (dt.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate //Consignments.')", true);
            //        txt_consignmentNo.Text = "";
            //        return;
            //    }
            //    dr["ConsignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
            //    dr["Orgin"] = dt_.Rows[0]["Orgin"].ToString();
            //    dr["OriginName"] = dt_.Rows[0]["OriginName"].ToString();
            //    dr["Destination"] = dt_.Rows[0]["Destination"].ToString();
            //    dr["DestinationName"] = dt_.Rows[0]["DestinationName"].ToString();
            //    dr["ConsignmentTypeID"] = dt_.Rows[0]["ConsignmentTypeID"].ToString();
            //    dr["ServiceTypeName"] = dt_.Rows[0]["ServiceTypeName"].ToString();
            //    dr["Weight"] = dt_.Rows[0]["Weight"].ToString();
            //    dr["Pieces"] = dt_.Rows[0]["Pieces"].ToString();
            //    if (Request.QueryString.Count > 0)
            //    {
            //        dr["ISMODIFIED"] = "INSERT";
            //    }
            //    else
            //    {
            //        if (rbtn_search.SelectedValue == "3")
            //        {
            //            dr["ISMODIFIED"] = "INSERT";
            //        }
            //        else
            //        {
            //            dr["ISMODIFIED"] = "NEW CONSIGNMENT";
            //        }
            //    }
            //    dr["Order"] = count;
            //    count++;
            //    dt.Rows.Add(dr);


            //    //dt.AcceptChanges();
            //    ViewState["temp"] = null;
            //    ViewState["temp"] = dt;


            //    if (dt.Rows.Count > 0)
            //    {
            //        lbl_count.Text = dt.Rows.Count.ToString();
            //        dt.AsDataView().Sort = "Order desc";
            //        gv_consignments.DataSource = dt;
            //        gv_consignments.DataBind();
            //        txt_consignmentNo.Text = "";
            //        txt_consignmentNo.Focus();
            //    }
            //}
            ////else
            ////{
            ////    if (dt.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
            ////    {
            ////        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate //Consignments.')", true);
            ////        txt_consignmentNo.Text = "";
            ////        return;
            ////    }
            ////    dr["ConsignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
            ////    dr["Orgin"] = dt_.Rows[0]["Orgin"].ToString();
            ////    dr["OriginName"] = dt_.Rows[0]["OriginName"].ToString();
            ////    dr["Destination"] = dt_.Rows[0]["Destination"].ToString();
            ////    dr["DestinationName"] = dt_.Rows[0]["DestinationName"].ToString();
            ////    dr["ConsignmentTypeID"] = dt_.Rows[0]["ConsignmentTypeID"].ToString();
            ////    dr["ServiceTypeName"] = dt_.Rows[0]["ServiceTypeName"].ToString();
            ////    dr["Weight"] = dt_.Rows[0]["Weight"].ToString();
            ////    dr["Pieces"] = dt_.Rows[0]["Pieces"].ToString();
            ////    if (Request.QueryString.Count > 0)
            ////    {
            ////        dr["ISMODIFIED"] = "NEW CONSIGNMENT";
            ////        //dt_Consignment.Rows.Add(txt_consignmentNo.Text, "overnight", "12", "0.5", HttpContext.Current.Session["BranchCode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), DateTime.Now, HttpContext.Current.Session["U_ID"].ToString(), "1", "330140", HttpContext.Current.Session["zonecode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), "4D1", DateTime.Now);
            ////    }
            ////    else
            ////    {
            ////        if (rbtn_search.SelectedValue == "3")
            ////        {
            ////            dr["ISMODIFIED"] = "INSERT";
            ////        }
            ////        else
            ////        {
            ////            dr["ISMODIFIED"] = "NEW CONSIGNMENT";
            ////            //dt_Consignment.Rows.Add(txt_consignmentNo.Text, "overnight", "12", "0.5", HttpContext.Current.Session["BranchCode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), DateTime.Now, HttpContext.Current.Session["U_ID"].ToString(), "1", "330140", HttpContext.Current.Session["zonecode"].ToString(), HttpContext.Current.Session["BranchCode"].ToString(), "4D1", DateTime.Now);
            ////        }

            ////    }
            ////    dr["Order"] = count;
            ////    count++;
            ////    dt.Rows.Add(dr);
            ////    //dt.AcceptChanges();
            ////    //dt_Manifest.Rows.Add(txt_consignmentNo.Text, "", "", "", "", "");
            ////    //dt_ConsignmentTracking.Rows.Add(txt_consignmentNo.Text, "18", b_fun.Get_Branches(clvar_).Tables[0].Rows[0]["name"].ToString(), "", "", "", "", "", "", DateTime.Now, "", DateTime.Now, "", "0", "", "", "", "", "");



            ////    ViewState["temp"] = null;
            ////    ViewState["temp"] = dt;





            ////    if (dt.Rows.Count > 0)
            ////    {
            ////        lbl_count.Text = dt.Rows.Count.ToString();
            ////        dt.AsDataView().Sort = "Order desc";
            ////        gv_consignments.DataSource = dt;
            ////        gv_consignments.DataBind();
            ////        txt_consignmentNo.Text = "";
            ////        txt_consignmentNo.Focus();
            ////    }
            ////} 
            #endregion

        }
        protected void gv_consignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string con = e.Row.Cells[1].Text;


                DataTable dt = ViewState["temp"] as DataTable;
                DataRow dr = dt.Select("ConsignmentNumber = '" + con + "'", "")[0];
                DataTable conType = ViewState["types"] as DataTable;
                DataTable dest = ViewState["destinations"] as DataTable;
                DropDownList dd = e.Row.FindControl("dd_gorigin") as DropDownList;
                dd.DataSource = dest;
                dd.DataTextField = "SNAME";
                dd.DataValueField = "branchCode";
                dd.DataBind();
                dd.SelectedValue = dr["Orgin"].ToString();

                DropDownList dd_ = e.Row.FindControl("dd_contype") as DropDownList;
                dd_.DataSource = conType;
                dd_.DataTextField = "ConsignmentType";
                dd_.DataValueField = "id";
                dd_.DataBind();
                dd_.SelectedValue = dr["ConsignmentTypeID"].ToString();

                if ((e.Row.FindControl("hd_isModified") as HiddenField).Value == "DELETE")
                {
                    e.Row.Visible = false;
                }

                if (Request.QueryString.Count > 0)
                {
                    e.Row.Cells[9].Visible = true;
                    gv_consignments.Columns[9].Visible = true;
                    DropDownList dd_status = ((DropDownList)e.Row.FindControl("dd_gStatus"));
                    DataTable ReceivingStatus = ViewState["ReceivingStatus"] as DataTable;
                    dd_status.DataSource = ReceivingStatus;
                    dd_status.DataTextField = "AttributeDesc";
                    dd_status.DataValueField = "id";
                    dd_status.DataBind();
                }



            }
        }
        protected void gv_consignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {
                string con = e.CommandArgument.ToString();
                DataTable dt = ViewState["temp"] as DataTable;
                DataTable dt_Manifest = ViewState["dt_Manifest"] as DataTable;
                DataTable dt_ConsignmentTracking = ViewState["dt_consigmnettracking"] as DataTable;//= dt_ConsignmentTracking;
                DataTable dt_Consignment = ViewState["dt_consignment"] as DataTable;//= dt_Consignment;
                DataRow[] dr = dt.Select("ConsignmentNumber = '" + con + "'");
                DataRow[] dr1 = dt_Manifest.Select("ConsignmentNumber = '" + con + "'");
                DataRow[] dr2 = dt_ConsignmentTracking.Select("ConsignmentNumber = '" + con + "'");
                DataRow[] dr3 = dt_Consignment.Select("ConsignmentNumber = '" + con + "'");

                if (rbtn_search.SelectedValue == "3")
                {
                    dr[0]["ISMODIFIED"] = "DELETE";
                }
                else
                {
                    if (dr.Count() > 0)
                    {
                        dt.Rows.Remove(dr[0]);
                    }
                    if (dr1.Count() > 0)
                    {
                        dt_Manifest.Rows.Remove(dr1[0]);
                    }
                    if (dr2.Count() > 0)
                    {
                        dt_ConsignmentTracking.Rows.Remove(dr2[0]);
                    }
                    if (dr3.Count() > 0)
                    {
                        dt_Consignment.Rows.Remove(dr3[0]);
                    }

                }
                dt.AcceptChanges();
                ViewState["temp"] = dt;
                ViewState["dtManifest"] = dt_Manifest;
                ViewState["dtConsignmentTracking"] = dt_ConsignmentTracking;
                ViewState["dtConsignment"] = dt_Consignment;

                lbl_count.Text = dt.Rows.Count.ToString();
                gv_consignments.DataSource = dt;
                gv_consignments.DataBind();
            }
            if (e.CommandName == "Update")
            {
                string con = e.CommandArgument.ToString();
                DataTable dt = ViewState["temp"] as DataTable;
                DataRow dr = dt.Select("ConsignmentNumber = '" + con + "'")[0];
                double newWeight = 0;
                int newPieces = 0;
                foreach (GridViewRow row in gv_consignments.Rows)
                {
                    if (row.Cells[1].Text == con)
                    {
                        newWeight = double.Parse((row.FindControl("txt_gWeight") as TextBox).Text);
                        newPieces = int.Parse((row.FindControl("txt_gPieces") as TextBox).Text);
                    }
                }
                if (newWeight > 0 && newPieces > 0)
                {
                    dr["Weight"] = newWeight.ToString();
                    dr["Pieces"] = newPieces.ToString();
                    dr["ISMODIFIED"] = "UPDATE";
                }
                dt.AcceptChanges();
                ViewState["temp"] = null;
                ViewState["temp"] = dt;
                lbl_count.Text = dt.Rows.Count.ToString();
                gv_consignments.DataSource = dt;
                gv_consignments.DataBind();
            }


        }
        protected void rbtn_search_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();

            if (rbtn_search.SelectedValue == "2")
            {
                btn_save.Visible = false;
                btn_print.Visible = true;
            }
            else if (rbtn_search.SelectedValue == "3")
            {
                if (txt_manifestNo.Text.Trim() != "")
                {
                    txt_manifestNo_TextChanged(sender, e);
                }
                btn_save.Visible = true;
                btn_print.Visible = false;
            }
            else
            {
                btn_save.Visible = true;
                btn_print.Visible = false;
                if (txt_manifestNo.Text.Trim() != "")
                {
                    txt_manifestNo_TextChanged(sender, e);
                }
            }
        }
        protected void txt_manifestNo_TextChanged(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
            {
                clvar.manifestNo = txt_manifestNo.Text;
                DataTable dt = GetConsignmentDetailByManifestNumber(clvar);
                if (dt.Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Exists')", true);
                    return;
                }

            }
            else
            {
                #region Manifest Wala Scene
                if (rbtn_search.SelectedValue == "2")
                {
                    clvar.manifestNo = txt_manifestNo.Text;
                    DataTable dt = GetConsignmentDetailByManifestNumber(clvar);
                    ViewState["temp"] = dt;
                    dt.Columns.Add("ORDER");
                    gv_consignments.Columns[8].Visible = false;
                    gv_consignments.DataSource = null;
                    gv_consignments.DataBind();
                    gv_consignments.DataSource = dt;
                    gv_consignments.DataBind();

                }
                else if (rbtn_search.SelectedValue == "1" || rbtn_search.SelectedValue == "3")
                {
                    clvar.manifestNo = txt_manifestNo.Text;
                    DataTable dt = GetConsignmentDetailByManifestNumber(clvar);
                    ViewState["temp"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        //DataTable header = con.GetManifestHeader(clvar);
                        //txt_date.Text = header.Rows[0]["date"].ToString();
                        //dd_destination.SelectedValue = header.Rows[0]["DCODE"].ToString();
                        //dd_origin.SelectedValue = header.Rows[0]["OCODE"].ToString();
                        //foreach (ListItem item in dd_serviceType.Items)
                        //{
                        //    if (item.Text.ToUpper() == header.Rows[0]["manifestType"].ToString().ToUpper())
                        //    {
                        //        item.Selected = true;
                        //        break;
                        //    }
                        //}
                        // dd_serviceType.SelectedValue = header.Rows[0]["manifestType"].ToString().ToUpper();
                        int count = dt.Rows.Count;

                        dt.Columns.Add("ORDER");
                        dt.AcceptChanges();
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["Order"] = count--;
                        }

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Exists')", true);

                        dt.AsDataView().Sort = "ORDER desc";
                        gv_consignments.DataSource = null;
                        gv_consignments.DataBind();
                        gv_consignments.DataSource = dt;
                        gv_consignments.DataBind();
                        //gv_consignments.Columns[0].Visible = false;
                        //gv_consignments.Columns[8].Visible = false;
                        gv_consignments.Visible = true;
                        btn_save.Enabled = false;
                        DataTable header = con.GetManifestHeader(clvar);
                        txt_date.Text = header.Rows[0]["date"].ToString();
                        dd_destination.SelectedValue = header.Rows[0]["DCODE"].ToString();
                        dd_origin.SelectedValue = header.Rows[0]["OCODE"].ToString();

                        for (int i = 0; i < dd_serviceType.Items.Count; i++)
                        {
                            if (dd_serviceType.Items[i].Text.ToUpper() == header.Rows[0]["manifestType"].ToString().ToUpper())
                            {
                                dd_serviceType.SelectedValue = dd_serviceType.Items[i].Value;
                                break;
                            }
                        }
                        if (rbtn_search.SelectedValue == "3")
                        {
                            gv_consignments.Columns[0].Visible = true;
                            btn_save.Enabled = true;
                            btn_save.CommandName = "UPDATE";
                            btn_save.Text = "Update";
                        }
                        else
                        {
                            gv_consignments.Columns[0].Visible = false;
                            gv_consignments.Columns[8].Visible = false;
                            btn_save.Enabled = false;
                            btn_save.CommandName = "";
                            btn_save.Text = "Save";

                        }
                        txt_consignmentNo.Focus();
                    }
                    else
                    {
                        btn_save.Enabled = true;
                        txt_consignmentNo.Focus();
                    }

                }
                #endregion
            }
        }
        protected void btn_print_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "MNPManifest_Print.aspx?Xcode=" + txt_manifestNo.Text, "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
        }
        protected void chk_weight_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void chk_pieces_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void dd_gorigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList origin = (DropDownList)sender;
            GridViewRow row = (GridViewRow)origin.Parent.Parent;

            DataTable dt = ViewState["temp"] as DataTable;

            string cn = row.Cells[1].Text;
            DataRow dr = dt.Select("ConsignmentNumber = '" + cn + "'", "")[0];
            dr["Orgin"] = origin.SelectedValue;
            dr["OriginName"] = origin.SelectedItem.Text;
            dr.AcceptChanges();
            if (origin.SelectedValue == "0")
            {
                row.Cells[2].BackColor = System.Drawing.Color.DarkGray;
            }
            else
            {
                row.Cells[2].BackColor = System.Drawing.Color.White;
            }
        }










        public DataTable GetConsignmentDetailForNewManifest_(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber,\n" +
            "       c.orgin,\n" +
            "       b.name              OriginName,\n" +
            "       c.destination,\n" +
            "       b2.name             DestinationName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces, 'NO' ISMODIFIED\n" +
            "  from consignment c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on c.destination = b2.branchCode\n" +
            " where c.consignmentNumber like '" + clvar.consignmentNo + "'\n" +
            "   --and c.serviceTypeName like '" + clvar.ServiceTypeName + "'\n" +
            "   --and c.orgin = '" + clvar.origin + "'";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }

        //public string DemanifestWithoutManifest(Cl_Variables clvar, DataTable cns, DataTable track, DataTable man)
        //{
        //    SqlConnection con = new SqlConnection(clvar.Strcon());
        //    SqlTransaction transaction;
        //    SqlCommand cmd = new SqlCommand();
        //    SqlCommand cmd1 = new SqlCommand();
        //    SqlCommand cmd2 = new SqlCommand();
        //    cmd.Connection = con;
        //    cmd1.Connection = con;
        //    cmd2.Connection = con;

        //    con.Open();
        //    transaction = con.BeginTransaction();

        //    try
        //    {
        //        cmd.Transaction = transaction;
        //        cmd1.Transaction = transaction;
        //        cmd2.Transaction = transaction;

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd1.CommandType = CommandType.StoredProcedure;
        //        cmd2.CommandType = CommandType.StoredProcedure;
        //        //cns.Columns.Add("riderCode", typeof(string));
        //        cmd.CommandText = "Bulk_Consignments_forManifest";
        //        cmd.Parameters.AddWithValue("@tblcustomers", cns);
        //        cmd.ExecuteNonQuery();

        //        cmd1.CommandText = "Bulk_Manifest";
        //        cmd1.Parameters.AddWithValue("@tblCustomers", man);
        //        cmd1.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
        //        cmd1.Parameters.AddWithValue("@ManifestType", clvar.CheckCondition);
        //        cmd1.Parameters.AddWithValue("@ManifestDate", clvar.Day);
        //        cmd1.Parameters.AddWithValue("@origin", clvar.origin);
        //        cmd1.Parameters.AddWithValue("@destination", clvar.destination);
        //        cmd1.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
        //        cmd1.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["zoneCode"].ToString());
        //        cmd1.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
        //        cmd1.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
        //        //SqlParameter retval = cmd1.Parameters.Add("@result", SqlDbType.Int);
        //        //retval.Direction = ParameterDirection.ReturnValue;
        //        //cmd1.Parameters.Add(retval);
        //        cmd1.ExecuteNonQuery();
        //        string retunvalue = cmd1.Parameters["@result"].Value.ToString();
        //        //string error = cmd1.Parameters["@result"].ToString();

        //        cmd2.CommandText = "Bulk_ConsignmentsTrackingHistory";
        //        cmd2.Parameters.AddWithValue("@tblcustomers", track);
        //        cmd2.ExecuteNonQuery();
        //        transaction.Commit();
        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        con.Close();
        //        return ex.Message;
        //    }

        //    return "OK";
        //}


        public string DemanifestWithoutManifest(Cl_Variables clvar, DataTable cns, DataTable track, DataTable man)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            string errorMessage = "";

            try
            {


                cmd.CommandType = CommandType.StoredProcedure;

                //cns.Columns.Add("riderCode", typeof(string));
                cmd.CommandText = "BULK_DEMANIFEST_WITHOUT_MANIFEST";
                cmd.Parameters.AddWithValue("@tblcustomers", cns);
                cmd.Parameters.AddWithValue("@tblCustomers1", track);
                cmd.Parameters.AddWithValue("@tblCustomers2", man);
                cmd.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@ManifestType", clvar.ServiceTypeName);
                cmd.Parameters.AddWithValue("@ManifestDate", clvar.LoadingDate);
                cmd.Parameters.AddWithValue("@origin", clvar.origin);
                cmd.Parameters.AddWithValue("@destination", clvar.destination);
                cmd.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["zoneCode"].ToString());
                cmd.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@error_message", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@result"].Value.ToString() != "1")
                {
                    errorMessage = cmd.Parameters["@result"].Value.ToString();
                }
                else
                {
                    errorMessage = "OK";
                }

                //cmd1.CommandText = "Bulk_Manifest";
                //cmd1.Parameters.AddWithValue("@tblCustomers", man);
                //cmd1.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                //cmd1.Parameters.AddWithValue("@ManifestType", clvar.CheckCondition);
                //cmd1.Parameters.AddWithValue("@ManifestDate", clvar.Day);
                //cmd1.Parameters.AddWithValue("@origin", clvar.origin);
                //cmd1.Parameters.AddWithValue("@destination", clvar.destination);
                //cmd1.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                //cmd1.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["zoneCode"].ToString());
                //cmd1.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
                //cmd1.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                ////SqlParameter retval = cmd1.Parameters.Add("@result", SqlDbType.Int);
                ////retval.Direction = ParameterDirection.ReturnValue;
                ////cmd1.Parameters.Add(retval);
                //cmd1.ExecuteNonQuery();
                //string retunvalue = cmd1.Parameters["@result"].Value.ToString();
                ////string error = cmd1.Parameters["@result"].ToString();

                //cmd2.CommandText = "Bulk_ConsignmentsTrackingHistory";
                //cmd2.Parameters.AddWithValue("@tblcustomers", track);
                //cmd2.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {

                con.Close();
                return ex.Message;
            }

            return errorMessage;
        }

        public string NewManifest(Cl_Variables clvar, DataTable cns, DataTable track, DataTable man)
        {
            SqlConnection con = new SqlConnection(clvar.Strcon());
            SqlTransaction transaction;
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con;

            con.Open();
            transaction = con.BeginTransaction();

            try
            {
                cmd1.Transaction = transaction;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "Bulk_Manifest_ForTest";
                cmd1.Parameters.AddWithValue("@tblCustomers", man);
                cmd1.Parameters.AddWithValue("@tblcustomers2", cns);
                cmd1.Parameters.AddWithValue("@tblcustomers1", track);

                cmd1.Parameters.AddWithValue("@ManifestNumber", clvar.manifestNo);
                cmd1.Parameters.AddWithValue("@ManifestType", clvar.CheckCondition);
                cmd1.Parameters.AddWithValue("@ManifestDate", clvar.Day);
                cmd1.Parameters.AddWithValue("@origin", clvar.origin);
                cmd1.Parameters.AddWithValue("@destination", clvar.destination);
                cmd1.Parameters.AddWithValue("@createdBy", HttpContext.Current.Session["U_ID"].ToString());
                cmd1.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["zoneCode"].ToString());
                cmd1.Parameters.AddWithValue("@branchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd1.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd1.ExecuteNonQuery();
                string retunvalue = cmd1.Parameters["@result"].Value.ToString();
                //string error = cmd1.Parameters["@result"].ToString();

                transaction.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                return ex.Message;
            }


            return "OK";
        }

        public DataTable GetConsignmentDetailByManifestNumber(Cl_Variables clvar)
        {

            string sqlString = "select c.consignmentNumber, c.consigner, c.consignee, c.weight,\n" +
            "       c.orgin,\n" +
            "       b.name              OriginName,\n" +
            "       c.destination,\n" +
            "       b2.name             DestinationName,\n" +
            "       c.consignmentTypeId,\n" +
            "       c.serviceTypeName,\n" +
            "       c.weight,\n" +
            "       c.pieces, 'NO' ISMODIFIED,\n" +
            "       (Select DATE from MNP_Manifest where manifestNumber = '" + clvar.manifestNo + "') ManifestDate" +
            "  from consignment c\n" +
            " inner join Branches b\n" +
            "    on c.orgin = b.branchCode\n" +
            " inner join Branches b2\n" +
            "    on c.destination = b2.branchCode\n" +
            " where c.consignmentNumber in ( SELECT cm.ConsignmentNumber from MNP_ConsignmentManifest cm where cm.manifestNumber = '" + clvar.manifestNo + "'  )";
            DataTable dt = new DataTable();

            try
            {
                SqlConnection orcl = new SqlConnection(clvar.Strcon());
                orcl.Open();
                SqlCommand orcd = new SqlCommand(sqlString, orcl);
                orcd.CommandType = CommandType.Text;
                orcd.CommandTimeout = 300;
                SqlDataAdapter oda = new SqlDataAdapter(orcd);
                oda.Fill(dt);
                orcl.Close();
            }
            catch (Exception)
            { }

            return dt;
        }


        public DataTable Cities_()
        {

            string sqlString = "SELECT CAST(A.bid as Int) bid, A.description NAME, A.expressCenterCode, A.CategoryID\n" +
            "  FROM (SELECT e.expresscentercode, e.bid, e.description, e.CategoryId\n" +
            "          FROM ServiceArea a\n" +
            "         right outer JOIN ExpressCenters e\n" +
            "            ON e.expressCenterCode = a.Ec_code where e.bid <> '17' and e.status = '1') A\n" +
            " GROUP BY A.bid, A.description, A.expressCenterCode, A.CategoryID\n" +
            " ORDER BY 2";

            sqlString = "";

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name SNAME, b.branchCode\n" +
            "  from branches b\n" +
            " where b.status = '1'\n" +
            " order by 2";

            DataTable dt = new DataTable();
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
    }
}