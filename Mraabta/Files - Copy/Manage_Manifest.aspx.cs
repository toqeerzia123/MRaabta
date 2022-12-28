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
    public partial class Manage_Manifest : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        CommonFunction fun = new CommonFunction();
        cl_Encryption enc = new cl_Encryption();
        Cl_Variables clvar_ = new Cl_Variables();


        protected void Page_Load(object sender, EventArgs e)
        {
            txt_date.Enabled = false;
            ErrorID.Text = "";
            if (!IsPostBack)
            {

                GetOrigin();
                GetDestination();
                GetServiceTypes();

                txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");


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
                ViewState["temp"] = null;
                ViewState["temp"] = dt;

                GetCNLengths();
                ViewState["types"] = fun.ConsignmentType().Tables[0];
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
            cnControls.DataSource = dt;
            cnControls.DataBind();
        }

        protected void GetOrigin()
        {
            if (Session["BranchCode"] != null)
            {
                DataTable dt = new DataTable();



                dt = fun.Branch().Tables[0];

                DataView dv = dt.AsDataView();
                dv.Sort = "BranchName";
                dt = dv.ToTable();
                dd_origin.DataSource = dv;
                dd_origin.DataTextField = "BranchName";
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
            DataTable dt = Cities_();
            //dt = fun.Branch().Tables[0];
            //DataView dv = dt.AsDataView();
            //dt = dv.ToTable();
            //dv.Sort = "BranchName";
            if (dt.Rows.Count > 0)
            {
                dd_destination.DataSource = dt;
                dd_destination.DataTextField = "BRANCHNAME";
                dd_destination.DataValueField = "branchCode";
                dd_destination.DataBind();
            }



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
            dd_serviceType.SelectedValue = "overnight";
            ViewState["serviceTypes"] = dt;

        }


        protected void btn_reset_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["temp"] as DataTable;
            dt.Clear();
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
                #region Validations
                if (txt_manifestNo.Text.Count() < 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Number Must be 12 Characters.')", true);
                    return;
                }
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

                if (rbtn_search.SelectedValue == "1" || rbtn_search.SelectedValue == "3")
                {
                    clvar.deliveryDate = DateTime.Parse(txt_date.Text);
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
                    new DataColumn("ISMODIFIED")

                });
                    dt.AcceptChanges();

                    foreach (GridViewRow row in gv_consignments.Rows)
                    {
                        DropDownList temp = (row.FindControl("dd_gorigin") as DropDownList);
                        if (temp.SelectedValue == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Origin')", true);
                            ErrorID.Text = "Select Origin";
                            ErrorID.ForeColor = Color.Red;
                            temp.Focus();
                            row.Cells[2].BackColor = System.Drawing.Color.DarkGray;
                            return;
                        }
                        DataRow dr = dt.NewRow();
                        //clvar.consignmentNo = row.Cells[1].Text;
                        //clvar.origin = (row.FindControl("dd_gorigin") as DropDownList).SelectedValue;
                        //clvar.destination = (row.FindControl("hd_destination") as HiddenField).Value;
                        //clvar.Con_Type = int.Parse((row.FindControl("dd_contype") as DropDownList).SelectedValue);
                        //clvar.ServiceTypeName = (row.FindControl("hd_serviceType") as Label).Text;
                        //clvar.Weight = float.Parse((row.FindControl("txt_gWeight") as TextBox).Text);
                        //clvar.pieces = int.Parse((row.FindControl("txt_gPieces") as TextBox).Text);

                        dr[0] = row.Cells[1].Text;
                        dr[1] = (row.FindControl("dd_gorigin") as DropDownList).SelectedValue;
                        dr[2] = (row.FindControl("dd_gorigin") as DropDownList).SelectedItem.Text;
                        dr[3] = (row.FindControl("hd_destination") as HiddenField).Value;
                        dr[4] = (row.FindControl("lbl_destination") as Label).Text;
                        dr[5] = (row.FindControl("dd_contype") as DropDownList).SelectedValue;
                        dr[6] = (row.FindControl("hd_serviceType") as Label).Text;
                        dr[7] = (row.FindControl("txt_gWeight") as TextBox).Text;
                        dr[8] = (row.FindControl("txt_gPieces") as TextBox).Text;
                        dr[9] = (row.FindControl("hd_isModified") as HiddenField).Value;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                    string searchCriteria = "";
                    if (rbtn_search.SelectedValue == "3")
                    {
                        searchCriteria = "INSERT";
                    }
                    else
                    {
                        searchCriteria = "NEW CONSIGNMENT";
                    }
                    if (dt.Select("ISMODIFIED = '" + searchCriteria + "'").Count() > 0)
                    {
                        clvar.Weight = 0.5f;
                        clvar.pieces = 1;
                        DataTable newConsignments = new DataTable();
                        newConsignments.Columns.AddRange(new DataColumn[]
                    {
                    new DataColumn("ConNo"),
                    new DataColumn("Origin"),
                    new DataColumn("Name"),
                    new DataColumn("CITYCODE"),
                    new DataColumn("Weight"),
                    new DataColumn("Pieces")
                    });

                        foreach (DataRow row in dt.Select("ISMODIFIED = '" + searchCriteria + "'").CopyToDataTable().Rows)
                        {
                            DataRow dr = newConsignments.NewRow();
                            dr[0] = row["ConsignmentNumber"].ToString();
                            dr[1] = row["Orgin"].ToString();
                            dr[2] = row["Destination"].ToString();
                            dr[3] = row["OriginName"].ToString();
                            dr[4] = row["Weight"].ToString();
                            dr[5] = row["Pieces"].ToString();
                            newConsignments.Rows.Add(dr);
                            newConsignments.AcceptChanges();
                        }
                        clvar.CustomerClientID = "330140";
                        clvar.AccountNo = "4D1";
                        clvar.riderCode = "";

                        string error = InsertConsignmentsFromRunsheet_mm(clvar, newConsignments);
                        if (error != "OK")
                        {
                            if (error == "NOT OK")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Insert New Consignments \\nPlease Contact I.T. Support ')", true);
                                ErrorID.Text = "Could Not Insert New Consignments. Please Contact I.T. Support ";
                                ErrorID.ForeColor = Color.Red;
                                return;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Inert New Consignments Error: '" + error + "'. \\nPlease Contact I.T. Support ')", true);
                                ErrorID.Text = "Could Not Inert New Consignments Error: '" + error + "'. Please Contact I.T. Support";
                                ErrorID.ForeColor = Color.Red;
                                return;
                            }

                        }
                    }
                    if (btn_save.CommandName == "UPDATE")
                    {
                        string error = UpdateManifest(clvar, dt, gv_consignments);
                        if (error == "OK")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Updated')", true);
                            ErrorID.Text = "Manifest Updated";
                            ErrorID.ForeColor = Color.Green;
                            txt_manifestNo_TextChanged(this, e);

                        }
                        else
                        {
                            ErrorID.Text = error.ToString();
                            ErrorID.ForeColor = Color.Red;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Update Manifest Error: '" + error + "'. \\nPlease Contact I.T. Support ')", true);
                            return;
                        }
                    }
                    else
                    {


                        if (GenerateManifest(clvar, dt) != "OK")
                        {

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Could Not Generate Manifest. \\nPlease Contact I.T. Support ')", true);
                            ErrorID.Text = "Could Not Generate Manifest. \\nPlease Contact I.T. Support ";
                            ErrorID.ForeColor = Color.Red;
                        }
                        else
                        {

                            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                            string script = String.Format(script_, "Manifest_Print.aspx?Xcode=" + txt_manifestNo.Text, "_blank", "");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);


                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Generated')", true);
                            //ErrorID.Text = "Manifest Generated ";
                            //ErrorID.ForeColor = Color.Green;
                            btn_reset_Click(sender, e);
                            return;

                        }
                    }
                }
                else
                {

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
            clvar.consignmentNo = txt_consignmentNo.Text;
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
            if (!IsNumeric(clvar.consignmentNo))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Number')", true);
                return;
            }

            //if (txt_consignmentNo.Text.Length > 15 || txt_consignmentNo.Text.Length < 11)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number must be between 11 and 15 digits.')", true);
            //    txt_consignmentNo.Text = "";
            //    return;
            //}

            if (dd_destination.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Destination')", true);
                txt_consignmentNo.Text = "";
                return;
            }

            // Consingmnet Validation
            clvar_.manifestNo = txt_consignmentNo.Text.ToString();
            clvar_.Branch = HttpContext.Current.Session["BRANCHCODE"].ToString();
            clvar_.Zone = HttpContext.Current.Session["ZONECODE"].ToString();
            clvar_.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();

            //if (inv_.SequenceCheck_Branch(clvar_).Rows.Count == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('This CN Number is Not valid.')", true);
            //    txt_consignmentNo.Text = "";
            //    txt_consignmentNo.Focus();
            //    return;
            //}

            //

            clvar.consignmentNo = txt_consignmentNo.Text;
            clvar.ServiceTypeName = dd_serviceType.SelectedItem.Text;

            clvar.origin = dd_origin.SelectedValue;
            clvar.destination = dd_destination.SelectedValue;
            DataTable types = ViewState["types"] as DataTable;
            DataTable destinations = ViewState["destinations"] as DataTable;
            DataTable dt_ = con.GetConsignmentDetailForNewManifest(clvar);
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


            foreach (GridViewRow row in gv_consignments.Rows)
            {
                DataRow dr_ = dt.NewRow();
                clvar.consignmentNo = row.Cells[1].Text;
                //clvar.origin = (row.FindControl("dd_gorigin") as DropDownList).SelectedValue;
                //clvar.destination = (row.FindControl("hd_destination") as HiddenField).Value;
                //clvar.Con_Type = int.Parse((row.FindControl("dd_contype") as DropDownList).SelectedValue);
                //clvar.ServiceTypeName = (row.FindControl("hd_serviceType") as Label).Text;
                //clvar.Weight = float.Parse((row.FindControl("txt_gWeight") as TextBox).Text);
                //clvar.pieces = int.Parse((row.FindControl("txt_gPieces") as TextBox).Text);

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
            DataRow dr = dt.NewRow();
            if (dt_.Rows.Count == 0)
            {
                if (txt_consignmentNo.Text.Trim() != "")
                {
                    if (dt.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate //Consignments.')", true);
                        txt_consignmentNo.Text = "";
                        return;
                    }
                    else
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
                        if (rbtn_search.SelectedValue == "3")
                        {
                            dr["ISMODIFIED"] = "INSERT";
                        }
                        else
                        {
                            dr["ISMODIFIED"] = "NEW CONSIGNMENT";
                        }

                        dr["Order"] = count.ToString();
                        count++;

                        dt.Rows.Add(dr);
                        dt.AcceptChanges();

                        ViewState["temp"] = null;
                        ViewState["temp"] = dt;


                        if (dt.Rows.Count > 0)
                        {
                            lbl_count.Text = dt.Rows.Count.ToString();
                            dt.AsDataView().Sort = "Order asc";
                            gv_consignments.DataSource = dt;
                            gv_consignments.DataBind();
                            txt_consignmentNo.Text = "";
                            txt_consignmentNo.Focus();
                        }
                    }
                }
                //txt_consignmentNo.Text = "";
                txt_consignmentNo.Focus();
                return;
            }

            if (dt_.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
            {
                if (dt.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate //Consignments.')", true);
                    txt_consignmentNo.Text = "";
                    return;
                }
                dr["ConsignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                dr["Orgin"] = dt_.Rows[0]["Orgin"].ToString();
                dr["OriginName"] = dt_.Rows[0]["OriginName"].ToString();
                dr["Destination"] = dt_.Rows[0]["Destination"].ToString();
                dr["DestinationName"] = dt_.Rows[0]["DestinationName"].ToString();
                dr["ConsignmentTypeID"] = dt_.Rows[0]["ConsignmentTypeID"].ToString();
                dr["ServiceTypeName"] = dt_.Rows[0]["ServiceTypeName"].ToString();
                dr["Weight"] = dt_.Rows[0]["Weight"].ToString();
                dr["Pieces"] = dt_.Rows[0]["Pieces"].ToString();
                if (rbtn_search.SelectedValue == "3")
                {
                    dr["ISMODIFIED"] = "INSERT";
                }
                else
                {
                    dr["ISMODIFIED"] = dt_.Rows[0]["ISMODIFIED"].ToString();
                }
                dr["Order"] = count;
                count++;
                dt.Rows.Add(dr);
                //dt.AcceptChanges();
                ViewState["temp"] = null;
                ViewState["temp"] = dt;


                if (dt.Rows.Count > 0)
                {
                    lbl_count.Text = dt.Rows.Count.ToString();
                    dt.AsDataView().Sort = "Order desc";
                    gv_consignments.DataSource = dt;
                    gv_consignments.DataBind();
                    txt_consignmentNo.Text = "";
                    txt_consignmentNo.Focus();
                }
            }
            else
            {
                if (dt.Select("ConsignmentNumber = '" + txt_consignmentNo.Text + "'").Count() != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Cannot Add Duplicate //Consignments.')", true);
                    txt_consignmentNo.Text = "";
                    return;
                }
                dr["ConsignmentNumber"] = dt_.Rows[0]["ConsignmentNumber"].ToString();
                dr["Orgin"] = dt_.Rows[0]["Orgin"].ToString();
                dr["OriginName"] = dt_.Rows[0]["OriginName"].ToString();
                dr["Destination"] = dt_.Rows[0]["Destination"].ToString();
                dr["DestinationName"] = dt_.Rows[0]["DestinationName"].ToString();
                dr["ConsignmentTypeID"] = dt_.Rows[0]["ConsignmentTypeID"].ToString();
                dr["ServiceTypeName"] = dt_.Rows[0]["ServiceTypeName"].ToString();
                dr["Weight"] = dt_.Rows[0]["Weight"].ToString();
                dr["Pieces"] = dt_.Rows[0]["Pieces"].ToString();
                if (rbtn_search.SelectedValue == "3")
                {
                    dr["ISMODIFIED"] = "INSERT";
                }
                else
                {
                    dr["ISMODIFIED"] = "NEW CONSIGNMENT";
                }
                dr["Order"] = count;
                count++;
                dt.Rows.Add(dr);
                //dt.AcceptChanges();
                ViewState["temp"] = null;
                ViewState["temp"] = dt;


                if (dt.Rows.Count > 0)
                {
                    lbl_count.Text = dt.Rows.Count.ToString();
                    dt.AsDataView().Sort = "Order desc";
                    gv_consignments.DataSource = dt;
                    gv_consignments.DataBind();
                    txt_consignmentNo.Text = "";
                    txt_consignmentNo.Focus();
                }
            }

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
                dd.DataTextField = "BranchName";
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
            }
        }
        protected void gv_consignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {
                string con = e.CommandArgument.ToString();
                DataTable dt = ViewState["temp"] as DataTable;
                DataRow dr = dt.Select("ConsignmentNumber = '" + con + "'")[0];
                if (rbtn_search.SelectedValue == "3")
                {
                    dr["ISMODIFIED"] = "DELETE";
                }
                else
                {
                    dt.Rows.Remove(dr);
                }
                dt.AcceptChanges();
                ViewState["temp"] = dt;
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
        }
        protected void btn_print_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            string script = String.Format(script_, "Manifest_Print.aspx?Xcode=" + txt_manifestNo.Text, "_blank", "");
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

            sqlString = "select b.branchCode , b.sname + ' - ' + b.name BRANCHNAME, b.branchCode\n" +
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


            sqlString = "SELECT cm.consignmentNumber, \n"
               + "        c.consigner, \n"
               + "        c.consignee, \n"
               + "        c.orgin, \n"
               + "        b.sname + '-' + b.name       OriginName, \n"
               + "        c.destination, \n"
               + "        b2.sname + '-' + b2.name     DestinationName, \n"
               + "        c.consignmentTypeId, \n"
               + "        c.serviceTypeName, \n"
               + "        cm.[Weight], \n"
               + "        cm.Pieces, \n"
               + "        'NO' ISMODIFIED, \n"
               + "        ( \n"
               + "            SELECT Date \n"
               + "            FROM   Mnp_Manifest mm \n"
               + "            WHERE  mm.manifestNumber = '" + clvar.manifestNo + "' \n"
               + "        )                            ManifestDate \n"
               + " FROM   Mnp_ConsignmentManifest cm \n"
               + "        INNER JOIN Consignment c \n"
               + "             ON  c.consignmentNumber = cm.consignmentNumber \n"
               + "        INNER JOIN Branches b \n"
               + "             ON  b.branchCode = c.orgin \n"
               + "        INNER JOIN Branches b2 \n"
               + "             ON  b2.branchCode = c.destination \n"
               + " WHERE  cm.manifestNumber = '" + clvar.manifestNo + "'";
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
        public string InsertConsignmentsFromRunsheet_mm(Cl_Variables clvar, DataTable dt)
        {
            string trackQuery = "";

            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;

            //" ) ";
            int count = 0;
            string check = "";
            string query = "";
            string query1 = "";

            //query = " INSERT INTO CONSIGNMENT (ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, weight , pieces, syncid,bookingDate,createdOn) ";
            //foreach (DataRow row in dt.Rows)
            //{
            //    query1 += "";

            //}
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    query = " INSERT INTO CONSIGNMENT (ConsignmentNumber, Orgin, Destination, CreditClientId, ConsignerAccountNo, weight , pieces, syncid,bookingDate,createdOn,zoneCode, branchCode, serviceTypeName, consignmentTypeId) ";
                    query += " Values( '" + dt.Rows[i]["ConNo"].ToString() + "', '" + dt.Rows[i]["Origin"].ToString() + "', '" + dt.Rows[i]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "', '" + dt.Rows[i]["Weight"].ToString() + "', '" + dt.Rows[i]["Pieces"].ToString() + "' , NewID(), GETDATE(), GETDATE(), '" + HttpContext.Current.Session["zonecode"].ToString() + "',\n" +
                                "   '" + HttpContext.Current.Session["BranchCode"].ToString() + "', '" + clvar.ServiceTypeName + "','12')";

                    sqlcmd.CommandText = query;
                    sqlcmd.ExecuteNonQuery();

                    trackQuery = "insert into ConsignmentsTrackingHistory \n" +
                                   "  (consignmentNumber, stateID, currentLocation, transactionTime)\n";
                    trackQuery += " VALUES(   '" + dt.Rows[i]["CONNO"].ToString() + "',\n" +
                                   "   '1',\n" +
                                   "   '" + dt.Rows[i]["CITYCODE"].ToString() + "',\n" +
                                   "   GETDATE()\n )";
                    sqlcmd.CommandText = trackQuery;
                    sqlcmd.ExecuteNonQuery();


                    trans.Commit();
                }
                catch (Exception ex)
                {

                    //throw;
                }
                //query += " SELECT '" + dt.Rows[i]["ConNo"].ToString() + "', '" + dt.Rows[i]["Origin"].ToString() + "', '" + dt.Rows[i]["Name"].ToString() + "', '" + clvar.CustomerClientID + "', '" + clvar.AccountNo + "', '" + clvar.Weight + "', '" + clvar.pieces + "' , NewID(), GETDATE(), GETDATE()\n" +
                //        " UNION ALL";

            }

            sqlcon.Close();



            return "OK";
        }
        public string GenerateManifest(Cl_Variables clvar, DataTable dt)
        {
            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            int count = 0;
            try
            {
                CommonFunction func = new CommonFunction();
                clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
                string branchName = func.Branch(clvar).Tables[0].Rows[0]["BranchName"].ToString();

                string query = "insert into MNP_Manifest (manifestNumber, origin, destination, branchCode, zoneCode, manifestType, date, createdBy, createdOn, isTemp)\n" +
                               " VALUES ( \n" +
                               "'" + clvar.manifestNo + "',\n" +
                               "'" + clvar.origin + "',\n" +
                               "'" + clvar.destination + "',\n" +
                               "'" + HttpContext.Current.Session["BranchCode"].ToString() + "',\n" +
                               "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                               "'" + clvar.ServiceTypeName + "',\n" +
                               "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
                               "'" + HttpContext.Current.Session["U_ID"].ToString() + "',\n" +
                               " GETDATE() ,\n" +
                               "'0'\n" +
                               ")";
                string query_ = "Insert into rvdbo.manifest (ManifestID, ManifestNo , ServiceTypeID, ManifestDate, ZoneID, OriginBranchID, DestBranchID,  isTemp,  isDemanifested) " +
                                "Values " +
                                "(" +
                                "'" + clvar.manifestNo.TrimStart('0') + "',\n" +
                                "'" + clvar.manifestNo + "',\n" +
                                "'" + clvar.serviceTypeId + "',\n" +
                                "'" + clvar.deliveryDate.ToString("yyyy-MM-dd") + "',\n" +
                                "'" + HttpContext.Current.Session["ZONECODE"].ToString() + "',\n" +
                                "'" + clvar.origin + "',\n" +
                                "'" + clvar.destination + "', '0', '0')";
                string query1_ = "Insert into rvdbo.Manifestconsignment (manifestID, ConsignmentID) ";
                string query1 = "INSERT INTO MNP_ConsignmentManifest (manifestNumber,consignmentNumber, weight, pieces ) \n";
                string trackQuery = "INSERT INTO ConsignmentsTrackingHistory (ConsignmentNumber, stateId, CurrentLocation, manifestNumber, transactionTime) \n";
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[i][0].ToString() + "', '" + dt.Rows[i]["Weight"].ToString() + "', '" + dt.Rows[i]["Pieces"].ToString() + "' \n UNION ALL \n";
                    query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "','" + dt.Rows[i][0].ToString() + "' \n UNION ALL \n";
                    trackQuery += "SELECT '" + dt.Rows[i][0].ToString() + "', '2', '" + branchName + "', '" + clvar.manifestNo + "',  GETDATE() \n" +
                                  " UNION ALL \n";

                }
                int j = dt.Rows.Count - 1;
                query1 += "SELECT '" + clvar.manifestNo + "', '" + dt.Rows[j][0].ToString() + "', '" + dt.Rows[j]["Weight"].ToString() + "', '" + dt.Rows[j]["Pieces"].ToString() + "'";
                query1_ += "SELECT '" + clvar.manifestNo.TrimStart('0') + "', '" + dt.Rows[j][0].ToString() + "'";
                trackQuery += "SELECT '" + dt.Rows[j][0].ToString() + "', '2', '" + branchName + "', '" + clvar.manifestNo + "',  GETDATE() \n";


                //SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
                //SqlTransaction trans;
                //sqlcon.Open();
                //SqlCommand sqlcmd = new SqlCommand();
                //sqlcmd.Connection = sqlcon;
                //trans = sqlcon.BeginTransaction();
                //sqlcmd.Transaction = trans;
                //sqlcmd.CommandType = CommandType.Text;

                sqlcmd.CommandText = query;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }
                //sqlcmd.CommandText = query_;
                //count = sqlcmd.ExecuteNonQuery();
                sqlcmd.CommandText = query1;
                count = sqlcmd.ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    count = 0;
                    return "NOT OK";
                }
                //sqlcmd.CommandText = query1_;
                //sqlcmd.ExecuteNonQuery();
                sqlcmd.CommandText = trackQuery;
                sqlcmd.ExecuteNonQuery();
                trans.Commit();
                // trans.Rollback();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                count = 0;
                return ex.Message;
            }
            finally
            {
                sqlcon.Close();
            }

            return "OK";
        }
        public string UpdateManifest(Cl_Variables clvar, DataTable dt, GridView gv)
        {
            int count = 0;
            string insertQuery = " INSERT INTO MNP_ConsignmentManifest (manifestNumber,consignmentNumber, weight, pieces ) \n";
            string updateQuery = "";
            string delQuery = "";

            string case1 = "case consignmentNumber \n";
            string case2 = "case consignmentNumber \n";
            //string case3 = "case consignmentNumber \n";
            //string case4 = "case consignmentNumber \n";
            //string case5 = "case ConsignmentNumber \n";

            string updateCns = "";
            string delCns = "";
            List<Tuple<string, string, string>> insertCnsList = new List<Tuple<string, string, string>>();
            List<List<string>> insertList = new List<List<string>>();
            bool updateFlag = false;
            bool delFlag = false;
            bool insertFlag = false;

            foreach (GridViewRow row in gv.Rows)
            {
                if ((row.FindControl("hd_isModified") as HiddenField).Value == "DELETE")
                {
                    delFlag = true;
                    delCns += "'" + row.Cells[1].Text.ToString() + "'";
                    continue;
                }
                if ((row.FindControl("hd_isModified") as HiddenField).Value == "INSERT")
                {
                    insertFlag = true;
                    Tuple<string, string, string> tup = new Tuple<string, string, string>(row.Cells[1].Text, (row.FindControl("txt_gWeight") as TextBox).Text, (row.FindControl("txt_gPieces") as TextBox).Text);
                    //insertCnsList.Add("'" + row.Cells[1].Text.ToString() + "'");
                    insertCnsList.Add(tup);
                    count++;
                    continue;
                }
                //case1 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("dd_gorigin") as DropDownList).SelectedValue + "'\n";
                //case2 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("dd_contype") as DropDownList).SelectedValue + "'\n";
                //case3 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("txt_gWeight") as TextBox).Text + "'\n";
                //case4 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("txt_gPieces") as TextBox).Text + "'\n";
                //case5 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("txt_gWeight") as TextBox).Text + "'\n";
                case1 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("txt_gPieces") as TextBox).Text + "'\n";
                case2 += "WHEN '" + row.Cells[1].Text.ToString() + "' then '" + (row.FindControl("txt_gWeight") as TextBox).Text + "'\n";
                updateCns += "'" + row.Cells[1].Text.ToString() + "'";

                updateFlag = true;
            }

            if (insertFlag)
            {
                for (int i = 0; i < insertCnsList.Count - 1; i++)
                {
                    insertQuery += "SELECT '" + clvar.manifestNo + "', '" + insertCnsList[i].Item1.ToString() + "', '" + insertCnsList[i].Item2.ToString() + "', '" + insertCnsList[i].Item3.ToString() + "'  \n UNION ALL \n";
                }
                insertQuery += "SELECT '" + clvar.manifestNo + "', '" + insertCnsList[insertCnsList.Count - 1].Item1.ToString() + "', '" + insertCnsList[insertCnsList.Count - 1].Item2.ToString() + "', '" + insertCnsList[insertCnsList.Count - 1].Item3.ToString() + "'  \n";

            }
            updateCns = updateCns.Replace("''", "','");
            case1 += " end\n";
            case2 += " end\n";
            //case3 += " end\n";
            //case4 += " end\n";
            //updateQuery = "UPDATE CONSIGNMENT SET Orgin = " + case1 + ", ConsignmentTypeID = " + case2 + ", weight = " + case3 + ", pieces = " + case4 + " where consignmentNumber in (" + updateCns + ")";
            updateQuery = "UPDATE MnP_ConsignmentManifest set pieces = " + case1 + ", weight = " + case2 + " where consignmentNumber in (" + updateCns + ")";
            delCns = delCns.Replace("''", "','");
            delQuery = "DELETE from MNP_ConsignmentManifest where manifestNumber = '" + clvar.manifestNo + "' and consignmentNumber in (" + delCns + ")";


            SqlConnection sqlcon = new SqlConnection(clvar.Strcon());
            SqlTransaction trans;
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            trans = sqlcon.BeginTransaction();
            sqlcmd.Transaction = trans;
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                if (insertFlag)
                {
                    sqlcmd.CommandText = insertQuery;
                    sqlcmd.ExecuteNonQuery();
                }
                if (updateFlag)
                {
                    sqlcmd.CommandText = updateQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                if (delFlag)
                {
                    sqlcmd.CommandText = delQuery;
                    sqlcmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                return ex.Message;
            }

            sqlcon.Close();

            return "OK";
        }
    }
}