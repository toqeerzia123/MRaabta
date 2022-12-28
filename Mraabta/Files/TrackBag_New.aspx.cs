using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRaabta.App_Code;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MRaabta.Files
{
    public partial class TrackBag_New : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        CommonFunction fun = new CommonFunction();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string temp = HttpContext.Current.Session["BranchCode"].ToString();
            }
            catch (Exception)
            {
                Errorid.Text = "Your Session has Expired. Please Login Again";
                Errorid.ForeColor = System.Drawing.Color.Red;
            }

            if (!IsPostBack)
            {
                rbtn_status.Enabled = false;
                GetBranches();
            }
        }
        protected void ReceivingStatus()
        {

            DataTable dt = GetReceivingStatus();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["RS"] = dt;
                }
            }

        }
        protected void GetBranches()
        {
            DataTable dt = Cities_();//.Branch().Tables[0];
            if (dt.Rows.Count > 0)
            {
                dd_departureTo.DataSource = dt;
                dd_departureTo.DataTextField = "BranchName";
                dd_departureTo.DataValueField = "branchCode";
                dd_departureTo.DataBind();

                dd_arrivedAt.DataSource = dt;
                dd_arrivedAt.DataTextField = "BranchName";
                dd_arrivedAt.DataValueField = "BranchCode";
                dd_arrivedAt.DataBind();

                dd_origin.DataSource = dt;
                dd_origin.DataTextField = "BranchName";
                dd_origin.DataValueField = "BranchCode";
                dd_origin.DataBind();

                dd_destination.DataSource = dt;
                dd_destination.DataTextField = "BranchName";
                dd_destination.DataValueField = "BranchCode";
                dd_destination.DataBind();


                try
                {
                    dd_arrivedAt.SelectedValue = HttpContext.Current.Session["Branchcode"].ToString();
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/login");
                }

                dd_arrivedAt.Enabled = false;
            }
        }
        protected void rbtn_status_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtn_status.SelectedValue == "1")
            {
                dd_departureTo.Enabled = false;
                string branchName = "";
                try
                {
                    branchName = HttpContext.Current.Session["BranchCode"].ToString();

                }
                catch (Exception ex)
                {
                    Response.Redirect("~/login");
                }

                if (txt_destination.Text.ToUpper() == dd_arrivedAt.SelectedItem.Text.ToUpper() || hd_debagWithoutBag.Value == "1")
                {
                    //divDialogue.Style.Add("display", "block");
                    if (gv_bagManifests.Rows.Count == 0)
                    {

                        //clvar.BagNumber = txt_bagNo.Text;
                        //DataTable dt = con.GetBagManifests(clvar);
                        //if (dt != null)
                        //{
                        //    if (dt.Rows.Count > 0)
                        //    {

                        //        gv_bagManifests.DataSource = dt;
                        //        gv_bagManifests.DataBind();
                        //    }
                        //}
                    }
                    dd_arrivedAt.Enabled = false;
                    //dd_departureTo.Enabled = true;

                }
                else
                {
                    AlertMessage("Cannot Select Final. This Branch is not the Destination of this Bag");

                    return;
                }


                //gv_bagManifests.DataSource = null;
                //gv_bagManifests.DataBind();
            }
            else
            {
                dd_arrivedAt.Enabled = false;
                dd_departureTo.Enabled = true;
                //divDialogue.Style.Add("display", "none");
                if (txt_destination.Text.ToUpper() == dd_arrivedAt.SelectedItem.Text.ToUpper() && hd_debagWithoutBag.Value == "0")
                {
                    rbtn_status.ClearSelection();
                    AlertMessage("Cannot Reroute this Bag. This IS the Destination");

                    return;
                }
                //dd_departureTo.Enabled = true;

            }
        }
        protected void btn_closePopUp_Click(object sender, EventArgs e)
        {
            //divDialogue.Style.Add("display", "none");

        }
        protected void txt_bagNo_TextChanged(object sender, EventArgs e)
        {
            ViewState["WB"] = "0";
            hd_debagWithoutBag.Value = "0";
            if (txt_bagNo.Text.Trim(' ') != "")
            {
                clvar.BagNumber = txt_bagNo.Text;
                DataTable dt = GetBagDetail(clvar);
                gv_bagManifests.DataSource = null;
                gv_bagManifests.DataBind();

                if (dt.Rows.Count > 0)
                {
                    dd_origin.Enabled = false;
                    dd_destination.Enabled = false;
                    rbtn_status.Enabled = false;
                    txt_date.Text = dt.Rows[0]["Date"].ToString();
                    txt_description.Text = dt.Rows[0]["Description"].ToString();
                    txt_destination.Text = dt.Rows[0]["DestName"].ToString();
                    txt_origin.Text = dt.Rows[0]["OriginName"].ToString();
                    txt_sealNo.Text = dt.Rows[0]["SealNo"].ToString();
                    txt_totalWeight.Text = dt.Rows[0]["totalWeight"].ToString();
                    hd_destination.Value = dt.Rows[0]["Destination"].ToString();
                    dd_destination.SelectedValue = dt.Rows[0]["Destination"].ToString();
                    hd_origin.Value = dt.Rows[0]["origin"].ToString();
                    dd_origin.SelectedValue = dt.Rows[0]["origin"].ToString();
                    clvar.BagNumber = txt_bagNo.Text;



                    if (dt.Rows[0]["Destination"].ToString() == Session["BranchCode"].ToString())
                    {
                        rbtn_status.SelectedValue = "1";
                        dd_departureTo.Enabled = false;
                        DataTable dt_ = GetBagManifests(clvar);
                        if (dt_ != null)
                        {
                            if (dt_.Rows.Count > 0)
                            {

                                gv_bagManifests.DataSource = dt_;
                                gv_bagManifests.DataBind();



                                foreach (GridViewRow row in gv_bagManifests.Rows)
                                {
                                    string statusID = (row.FindControl("hd_Gstatus") as HiddenField).Value;
                                    if (statusID == "5" || statusID == "7")
                                    {
                                        CheckBox chk = row.FindControl("chk_received") as CheckBox;
                                        chk.Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                gv_bagManifests.DataSource = null;
                                gv_bagManifests.DataBind();
                            }
                        }
                        else
                        {
                            gv_bagManifests.DataSource = null;
                            gv_bagManifests.DataBind();
                        }
                        DataTable dt__ = GetBagOutpieces(clvar);
                        if (dt__ != null)
                        {
                            if (dt__.Rows.Count > 0)
                            {

                                gv_outpieces.DataSource = dt__;
                                gv_outpieces.DataBind();

                                foreach (GridViewRow row in gv_outpieces.Rows)
                                {
                                    string statusID = (row.FindControl("hd_Gstatus") as HiddenField).Value;
                                    if (statusID == "5" || statusID == "7")
                                    {
                                        CheckBox chk = row.FindControl("chk_received") as CheckBox;
                                        chk.Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                gv_outpieces.DataSource = null;
                                gv_outpieces.DataBind();
                            }
                        }
                        else
                        {
                            gv_outpieces.DataSource = null;
                            gv_outpieces.DataBind();
                        }
                    }
                    else
                    {
                        rbtn_status.SelectedValue = "0";
                        dd_departureTo.Enabled = true;
                    }


                }
                else
                {
                    txt_date.Text = "";
                    txt_description.Text = "";
                    txt_destination.Text = "";
                    txt_origin.Text = "";
                    txt_sealNo.Text = "";
                    dd_origin.ClearSelection();
                    dd_destination.ClearSelection();
                    dd_origin.Enabled = true;
                    dd_destination.Enabled = true;
                    txt_totalWeight.Text = "";
                    //AlertMessage("Bag Does Not Exists.");
                    rbtn_status.Enabled = true;
                    div1.Style.Add("display", "block");

                    return;
                }

            }

        }
        protected void btn_save_Click(object sender, EventArgs e)
        {

            if (rbtn_status.SelectedValue == "0" && dd_departureTo.SelectedValue == "0")
            {
                AlertMessage("Select Departed To");
                return;
            }
            string arrivedAt = dd_arrivedAt.SelectedValue;
            string departedTo = dd_departureTo.SelectedValue;
            string CurrentLocation = dd_arrivedAt.SelectedItem.Text;
            clvar.BagNumber = txt_bagNo.Text;
            clvar.origin = dd_origin.SelectedValue;// hd_origin.Value;
            clvar.destination = dd_destination.SelectedValue;// hd_destination.Value;
            clvar.Branch = HttpContext.Current.Session["BranchCode"].ToString();
            clvar.Zone = HttpContext.Current.Session["ZoneCode"].ToString();
            clvar.Weight = float.Parse(txt_totalWeight.Text);
            clvar.SealNumber = txt_sealNo.Text;
            //string arrivedAt = dd_arrivedAt.SelectedValue;
            //string departedTo = dd_departureTo.SelectedValue;
            //string CurrentLocation = dd_arrivedAt.SelectedItem.Text;
            clvar.productDescription = txt_description.Text;
            clvar.BookingDate = DateTime.Parse(txt_date.Text).ToString("yyyy-MM-dd");
            clvar.expresscenter = HttpContext.Current.Session["ExpressCenter"].ToString();
            bool isFinal;
            if (rbtn_status.SelectedValue == "0")
            {
                isFinal = false;
            }
            else
            {
                isFinal = true;
            }
            DataTable bags = new DataTable();
            DataTable cns = new DataTable();
            #region Debag Without Bag
            if (hd_debagWithoutBag.Value == "1")
            {
                if (dd_origin.SelectedValue == "" || dd_destination.SelectedValue == "")
                {
                    AlertMessage("Select Origin/Destination");
                    return;
                }

                if (dd_destination.SelectedValue == HttpContext.Current.Session["BranchCode"].ToString() && rbtn_status.SelectedValue == "0")
                {
                    AlertMessage("Cannot Reroute This Bag. Selected Destination IS this Branch");
                    return;
                }
                else if (dd_destination.SelectedValue != HttpContext.Current.Session["BranchCode"].ToString() && rbtn_status.SelectedValue == "1")
                {
                    AlertMessage("Cannot Final This Bag. Selected Destination is not this Branch");
                    return;
                }

                bags.Columns.Add("ManifestNumber", typeof(string));

                bags.Columns.Add("StatusCode", typeof(int));
                bags.Columns.Add("Reason", typeof(string));



                bags.AcceptChanges();
                foreach (GridViewRow row in gv_bagManifests.Rows)
                {
                    //HtmlTableCell cell = row.Cells[4] as HtmlTableCell;
                    TextBox l = row.FindControl("txt_statusDesc") as TextBox;
                    CheckBox chk = row.FindControl("chk_received") as CheckBox;

                    if (chk.Checked)
                    {
                        if (l.Text.ToUpper() == "RECEIVED")
                        {
                            (row.FindControl("hd_Gstatus") as HiddenField).Value = "5";
                        }
                        else
                        {
                            (row.FindControl("hd_Gstatus") as HiddenField).Value = "7";
                        }
                    }
                    else
                    {
                        (row.FindControl("hd_Gstatus") as HiddenField).Value = "6";
                    }
                    DataRow dr = bags.NewRow();


                    dr["ManifestNumber"] = row.Cells[1].Text;

                    dr["StatusCode"] = int.Parse((row.FindControl("hd_GStatus") as HiddenField).Value);
                    dr["Reason"] = (row.FindControl("txt_remarks") as TextBox).Text;


                    bags.Rows.Add(dr);
                }



                cns.Columns.Add("OutpieceNumber", typeof(string));

                cns.Columns.Add("StatusCode", typeof(int));
                cns.Columns.Add("Reason", typeof(string));
                cns.Columns.Add("cnWeight", typeof(string));
                cns.Columns.Add("cnPieces", typeof(string));

                foreach (GridViewRow row in gv_outpieces.Rows)
                {
                    CheckBox chk = row.FindControl("chk_received") as CheckBox;
                    TextBox l = row.FindControl("txt_statusDesc") as TextBox;
                    TextBox weight = row.FindControl("txt_cnWeight") as TextBox;
                    TextBox pieces = row.FindControl("txt_cnPieces") as TextBox;
                    if (chk.Checked)
                    {
                        if (l.Text.ToUpper() == "RECEIVED")
                        {
                            (row.FindControl("hd_Gstatus") as HiddenField).Value = "5";
                        }
                        else
                        {
                            (row.FindControl("hd_Gstatus") as HiddenField).Value = "7";
                        }


                    }
                    else
                    {
                        (row.FindControl("hd_Gstatus") as HiddenField).Value = "6";
                    }
                    DataRow dr = cns.NewRow();


                    dr["OutpieceNumber"] = row.Cells[1].Text;

                    dr["StatusCode"] = int.Parse((row.FindControl("hd_GStatus") as HiddenField).Value);
                    dr["Reason"] = (row.FindControl("txt_remarks") as TextBox).Text;
                    dr["cnPieces"] = pieces.Text;
                    dr["cnWeight"] = weight.Text;
                    cns.Rows.Add(dr);
                }

                if (rbtn_status.SelectedValue == "0")
                {
                    isFinal = false;
                    departedTo = dd_departureTo.SelectedValue;
                    List<string> errorMessage = RerouteABagWithoutBag(clvar, arrivedAt, departedTo, CurrentLocation, bags, cns);
                    if (errorMessage[0] == "0")
                    {
                        AlertMessage("Bag Could Not be Rerouted. Error:" + errorMessage[1]);
                    }
                    else
                    {
                        AlertMessage("Bag Rerouted Successfully");
                    }
                    //AlertMessage(message);

                }
                else
                {
                    List<string> error = DebagABag(clvar, bags, cns, arrivedAt, departedTo, isFinal);
                    if (error[0].ToString() == "0")
                    {
                        AlertMessage("Could Not Debag. Error: " + error[1]);

                    }
                    else
                    {
                        AlertMessage("Debag Complete. Debag ID = " + error[1]);
                        string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                        //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                        string script = String.Format(script_, "Debag_print.aspx?Xcode=" + error[1], "_blank", "");

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                        ResetAll();
                        //Response.Redirect("TrackBag_new.aspx");
                    }
                }
                return;
            }

            #endregion


            #region Debag With Bag

            if (rbtn_status.SelectedValue == "0")
            {

                clvar.BagNumber = txt_bagNo.Text;
                List<string> errorMessages = RerouteABag(clvar, arrivedAt, departedTo, CurrentLocation);
                if (errorMessages[0] == "0")
                {
                    AlertMessage(errorMessages[1]);
                }
                else
                {
                    AlertMessage("Bag Rerouted SuccessFully");
                }
                return;
            }






            bags.Columns.Add("ManifestNumber", typeof(string));

            bags.Columns.Add("StatusCode", typeof(int));
            bags.Columns.Add("Reason", typeof(string));



            bags.AcceptChanges();
            foreach (GridViewRow row in gv_bagManifests.Rows)
            {
                //HtmlTableCell cell = row.Cells[4] as HtmlTableCell;
                TextBox l = row.FindControl("txt_statusDesc") as TextBox;
                CheckBox chk = row.FindControl("chk_received") as CheckBox;

                if (chk.Checked)
                {
                    if (l.Text.ToUpper() == "RECEIVED")
                    {
                        (row.FindControl("hd_Gstatus") as HiddenField).Value = "5";
                    }
                    else
                    {
                        (row.FindControl("hd_Gstatus") as HiddenField).Value = "7";
                    }
                }
                else
                {
                    (row.FindControl("hd_Gstatus") as HiddenField).Value = "6";
                }
                DataRow dr = bags.NewRow();


                dr["ManifestNumber"] = row.Cells[1].Text;

                dr["StatusCode"] = int.Parse((row.FindControl("hd_GStatus") as HiddenField).Value);
                dr["Reason"] = (row.FindControl("txt_remarks") as TextBox).Text;

                bags.Rows.Add(dr);
            }



            cns.Columns.Add("OutpieceNumber", typeof(string));

            cns.Columns.Add("StatusCode", typeof(int));
            cns.Columns.Add("Reason", typeof(string));
            cns.Columns.Add("cnWeight", typeof(string));
            cns.Columns.Add("cnPieces", typeof(string));

            foreach (GridViewRow row in gv_outpieces.Rows)
            {
                CheckBox chk = row.FindControl("chk_received") as CheckBox;
                TextBox l = row.FindControl("txt_statusDesc") as TextBox;
                TextBox weight = row.FindControl("txt_cnWeight") as TextBox;
                TextBox pieces = row.FindControl("txt_cnPieces") as TextBox;
                if (chk.Checked)
                {
                    if (l.Text.ToUpper() == "RECEIVED")
                    {
                        (row.FindControl("hd_Gstatus") as HiddenField).Value = "5";
                    }
                    else
                    {
                        (row.FindControl("hd_Gstatus") as HiddenField).Value = "7";
                    }


                }
                else
                {
                    (row.FindControl("hd_Gstatus") as HiddenField).Value = "6";
                }
                DataRow dr = cns.NewRow();


                dr["OutpieceNumber"] = row.Cells[1].Text;

                dr["StatusCode"] = int.Parse((row.FindControl("hd_GStatus") as HiddenField).Value);
                dr["Reason"] = (row.FindControl("txt_remarks") as TextBox).Text;
                dr["cnPieces"] = pieces.Text;
                dr["cnWeight"] = weight.Text;
                cns.Rows.Add(dr);
            }

            if (rbtn_status.SelectedValue == "0")
            {
                isFinal = false;
                departedTo = dd_departureTo.SelectedValue;

            }
            else
            {
                //string message = con.DebagABag(clvar, bags, cns, arrivedAt, departedTo, isFinal);
                List<string> error = DebagABag(clvar, bags, cns, arrivedAt, departedTo, isFinal);
                if (error[0].ToString() == "0")
                {
                    AlertMessage("Could Not Debag. Error: " + error[1]);
                }
                else
                {
                    AlertMessage("Debag Complete. Debag ID = " + error[1]);
                    string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                    //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
                    string script = String.Format(script_, "printDebugDetails.aspx?id=" + error[1], "_blank", "");

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
                    ResetAll();
                }
            }

            #endregion

        }
        protected void chk_received_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;
            if ((gr.FindControl("chk_received") as CheckBox).Checked)
            {
                //gr.Cells[2].Text = "Received";
                //(gr.FindControl("lbl_gStatus") as Label).Text = "Received";
                (gr.FindControl("hd_Gstatus") as HiddenField).Value = "5";
            }
            else
            {
                //(gr.FindControl("lbl_gStatus") as Label).Text = "ShortReceived";
                (gr.FindControl("hd_Gstatus") as HiddenField).Value = "6";
            }

        }
        protected void btn_ok_Click(object sender, EventArgs e)
        {
            Boolean flag = false;
            foreach (GridViewRow row in gv_bagManifests.Rows)
            {
                if ((row.FindControl("lbl_gStatus") as Label).Text != "Received" && (row.FindControl("txt_remarks") as TextBox).Text.Trim(' ') == "")
                {
                    (row.FindControl("lbl_gStatus") as Label).Text = "Short Received";
                    flag = true;
                }
            }

            if (flag)
            {
                AlertMessage("Please Provide Remarks For Short Received");

            }
            else
            {
                //divDialogue.Style.Add("display", "none");

            }



        }


        protected void ResetAll()
        {
            txt_bagNo.Text = "";
            txt_date.Text = "";
            txt_description.Text = "";
            txt_destination.Text = "";
            //txt_manifestNumber.Text = "";
            txt_origin.Text = "";
            txt_sealNo.Text = "";
            txt_totalWeight.Text = "";
            rbtn_status.ClearSelection();
            gv_bagManifests.DataSource = null;
            gv_bagManifests.DataBind();
            gv_outpieces.DataSource = null;
            gv_outpieces.DataBind();



            txt_date.Text = "";
            txt_description.Text = "";
            txt_destination.Text = "";
            txt_origin.Text = "";
            txt_sealNo.Text = "";
            dd_origin.ClearSelection();
            dd_destination.ClearSelection();
            dd_origin.Enabled = false;
            dd_destination.Enabled = false;
            txt_totalWeight.Text = "";
            //AlertMessage("Bag Does Not Exists.");
            rbtn_status.Enabled = false;

        }

        protected void txt_consignment_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txt_manifest_TextChanged(object sender, EventArgs e)
        {
            clvar.manifestNo = txt_manifest.Text;
            DataTable dt_ = GetExcessManifest(clvar);
            DataTable dt = dt_.Copy();
            dt.Clear();
            if (dt_ != null)
            {

                foreach (GridViewRow row in gv_bagManifests.Rows)
                {
                    DataRow dr = dt.NewRow();
                    HiddenField hd_origin = row.FindControl("hd_Gorigin") as HiddenField;
                    HiddenField hd_destination = row.FindControl("hd_Gdestination") as HiddenField;
                    HiddenField hd_statusCode = row.FindControl("hd_Gstatus") as HiddenField;
                    TextBox txt_descStatus = row.FindControl("txt_statusDesc") as TextBox;
                    TextBox txt_reason = row.FindControl("txt_remarks") as TextBox;
                    CheckBox chk_received = row.FindControl("chk_received") as CheckBox;
                    DropDownList dd_status = row.FindControl("dd_status") as DropDownList;
                    string wbCheck = ViewState["WB"].ToString();
                    if (hd_debagWithoutBag.Value == "1")
                    {
                        txt_descStatus.Text = "RECEIVED";
                        hd_statusCode.Value = "5";
                    }
                    else
                    {
                        if (txt_descStatus.Text.ToUpper() == "SHORT RECEIVED")
                        {
                            hd_statusCode.Value = "6";
                        }
                        else if (txt_descStatus.Text.ToUpper() == "RECEIVED")
                        {
                            hd_statusCode.Value = "5";
                        }
                        else if (txt_descStatus.Text.ToUpper() == "EXCESS RECEIVED")
                        {
                            hd_statusCode.Value = "7";
                        }
                    }
                    dr["ManifestNo"] = row.Cells[1].Text;
                    dr["OriginID"] = hd_origin.Value;
                    dr["origin"] = row.Cells[2].Text;
                    dr["DestinationID"] = hd_destination.Value;
                    dr["dest"] = row.Cells[3].Text;
                    dr["statusCode"] = hd_statusCode.Value;
                    dr["statusDesc"] = txt_descStatus.Text;
                    dr["Reason"] = txt_reason.Text;

                    dt.Rows.Add(dr);
                    if (hd_debagWithoutBag.Value == "1")
                    {

                        chk_received.Enabled = false;
                        row.Cells[6].Visible = true;
                    }
                    else //Debag without bag wala scene BC
                    {




                    }

                }
                if (dt_.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["ManifestNo"] = dt_.Rows[0]["ManifestNo"].ToString();
                    dr["OriginID"] = dt_.Rows[0]["OriginID"];
                    dr["origin"] = dt_.Rows[0]["origin"];
                    dr["DestinationID"] = dt_.Rows[0]["DestinationID"];
                    dr["dest"] = dt_.Rows[0]["dest"];
                    if (hd_debagWithoutBag.Value == "1")
                    {
                        dr["statusCode"] = "5";
                        dr["StatusDesc"] = "RECEIVED";
                    }
                    else
                    {
                        dr["statusCode"] = dt_.Rows[0]["StatusCode"].ToString();
                        dr["StatusDesc"] = dt_.Rows[0]["StatusDesc"].ToString();
                    }
                    dr["Reason"] = dt_.Rows[0]["Reason"];

                    dt.Rows.Add(dr);
                }
                else
                {
                    AlertMessage("Consignment Does not Exist");

                }

                if (dt.Rows.Count > 0)
                {
                    gv_bagManifests.DataSource = dt;
                    gv_bagManifests.DataBind();
                }
                foreach (GridViewRow row in gv_bagManifests.Rows)
                {
                    if ((row.FindControl("hd_Gstatus") as HiddenField).Value == "7" || (row.FindControl("hd_Gstatus") as HiddenField).Value == "5")
                    {
                        if ((row.FindControl("hd_Gstatus") as HiddenField).Value == "7")
                        {
                            (row.FindControl("chk_received") as CheckBox).Enabled = false;
                        }

                        (row.FindControl("chk_received") as CheckBox).Checked = true;
                    }
                }
                txt_manifest.Text = "";
            }

        }
        protected void txt_outpiece_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumeric(txt_cnNo.Text))
            {
                AlertMessage("Only Numerics allowed in Consignment Number");
                txt_cnNo.Text = "";
                txt_cnNo.Focus();
                return;
            }
            if (txt_cnNo.Text.Length > 15 || txt_cnNo.Text.Length < 11)
            {
                AlertMessage("Consignment Number must be between 11 and 15 digits");
                txt_cnNo.Text = "";
                txt_cnNo.Focus();
                return;
            }
            #region Primary Check By Fahad 12-oct-2020
            var rs = PrimaryCheck(txt_cnNo.Text);
            if (!string.IsNullOrEmpty(rs))
            {
                AlertMessage(rs);
                txt_cnNo.Text = "";
                txt_cnNo.Focus();
                return;
            }
            #endregion

            #region RTS/DLV Check by Talha 23-oct-2020
            var rss = alreadyRTS_DLV(txt_cnNo.Text);
            if (!string.IsNullOrEmpty(rss))
            {
                AlertMessage(rss);
                txt_cnNo.Text = "";
                txt_cnNo.Focus();
                return;
            }
            #endregion

            if (txt_cnNo.Text.StartsWith("5") && txt_cnNo.Text.Length == 15)
            {
                DataTable BookingDT = CheckConsignmentBooking(txt_cnNo.Text);
                DataTable FirstProcessDT = CheckFirstProcessOrigin(txt_cnNo.Text);
                string status = "true";
                if (BookingDT.Rows.Count > 0)
                {
                    //if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                    if (BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                    {
                        AlertMessage("Once reached destination can only move with Return NCI");
                        txt_cnNo.Text = "";
                        txt_cnNo.Focus();
                        return;
                    }
                    else if (FirstProcessDT.Rows.Count > 0 || BookingDT.Rows[0]["isApproved"].ToString() == "True")
                    {
                        if (BookingDT.Rows[0]["status"].ToString() == "9")
                        {
                            AlertMessage("Consignment is Void perform Arrival");
                            txt_cnNo.Text = "";
                            txt_cnNo.Focus();
                            return;
                        }
                    }
                    else
                    {
                        AlertMessage("First Process Must be at orign");
                        txt_cnNo.Text = "";
                        txt_cnNo.Focus();
                        return;
                    }
                }
                else
                {
                    AlertMessage("No booking found for this COD CN");
                    txt_cnNo.Text = "";
                    txt_cnNo.Focus();
                    return;
                }
            }
            
            clvar.consignmentNo = txt_cnNo.Text;
            DataTable dt_ = GetExcessOutPieces(clvar);
            DataTable dt = dt_.Copy();
            dt.Clear();
            if (dt_ != null)
            {

                foreach (GridViewRow row in gv_outpieces.Rows)
                {
                    DataRow dr = dt.NewRow();
                    HiddenField hd_origin = row.FindControl("hd_Gorigin") as HiddenField;
                    HiddenField hd_destination = row.FindControl("hd_Gdestination") as HiddenField;
                    HiddenField hd_statusCode = row.FindControl("hd_Gstatus") as HiddenField;
                    TextBox txt_descStatus = row.FindControl("txt_statusDesc") as TextBox;
                    TextBox txt_reason = row.FindControl("txt_remarks") as TextBox;
                    CheckBox chk_received = row.FindControl("chk_received") as CheckBox;
                    TextBox weight = row.FindControl("txt_cnWeight") as TextBox;
                    TextBox pieces = row.FindControl("txt_cnpieces") as TextBox;
                    if (hd_debagWithoutBag.Value == "1")
                    {
                        txt_descStatus.Text = "RECEIVED";
                        hd_statusCode.Value = "5";
                    }
                    else
                    {
                        if (txt_descStatus.Text.ToUpper() == "SHORT RECEIVED")
                        {
                            hd_statusCode.Value = "6";
                        }
                        else if (txt_descStatus.Text.ToUpper() == "RECEIVED")
                        {
                            hd_statusCode.Value = "5";
                        }
                        else if (txt_descStatus.Text.ToUpper() == "EXCESS RECEIVED")
                        {
                            hd_statusCode.Value = "7";
                        }
                    }
                    dr["outpieceNumber"] = row.Cells[1].Text;
                    dr["origin"] = hd_origin.Value;
                    dr["originName"] = row.Cells[2].Text;
                    dr["destination"] = hd_destination.Value;
                    dr["destName"] = row.Cells[3].Text;
                    dr["statusCode"] = hd_statusCode.Value;
                    dr["statusDesc"] = txt_descStatus.Text;
                    dr["Reason"] = txt_reason.Text;
                    dr["weight"] = weight.Text;
                    dr["pieces"] = pieces.Text;
                    dt.Rows.Add(dr);

                }
                if (dt_.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["outpieceNumber"] = dt_.Rows[0]["outpieceNumber"].ToString();
                    dr["origin"] = dt_.Rows[0]["origin"];
                    dr["originName"] = dt_.Rows[0]["originName"];
                    dr["destination"] = dt_.Rows[0]["destination"];
                    dr["destName"] = dt_.Rows[0]["destName"];
                    if (hd_debagWithoutBag.Value == "1")
                    {
                        dr["statusCode"] = "5";
                        dr["StatusDesc"] = "RECEIVED";
                    }
                    else
                    {
                        dr["statusCode"] = dt_.Rows[0]["StatusCode"].ToString();
                        dr["StatusDesc"] = dt_.Rows[0]["StatusDesc"].ToString();
                    }
                    dr["Reason"] = dt_.Rows[0]["Reason"];
                    dr["weight"] = dt_.Rows[0]["Weight"].ToString();
                    dr["pieces"] = dt_.Rows[0]["Pieces"].ToString();

                    dt.Rows.Add(dr);
                }

                if (dt.Rows.Count > 0)
                {
                    gv_outpieces.DataSource = dt;
                    gv_outpieces.DataBind();
                }
                foreach (GridViewRow row in gv_outpieces.Rows)
                {
                    if ((row.FindControl("hd_Gstatus") as HiddenField).Value == "7" || (row.FindControl("hd_Gstatus") as HiddenField).Value == "5")
                    {
                        if ((row.FindControl("hd_Gstatus") as HiddenField).Value == "7")
                        {
                            (row.FindControl("chk_received") as CheckBox).Enabled = false;
                        }

                        (row.FindControl("chk_received") as CheckBox).Checked = true;
                    }
                }
                txt_cnNo.Text = "";
                txt_cnNo.Focus();
            }

        }



        protected void AlertMessage(string message)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }
        protected void btn_ok2_Click(object sender, EventArgs e)
        {
            ViewState["WB"] = "1";
            hd_debagWithoutBag.Value = "1";
            div1.Style.Add("display", "none");
            gv_bagManifests.DataSource = null;
            gv_bagManifests.DataBind();
            gv_outpieces.DataSource = null;
            gv_outpieces.DataBind();
        }



        public DataTable GetReceivingStatus()
        {
            string query = "select l.Id statusCode, l.AttributeDesc StatusDesc from rvdbo.Lookup l where l.AttributeGroup = 'RECEIVING_STATUS'";
            SqlConnection con = new SqlConnection(clvar.Strcon());
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);

            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public string DebagWithoutBag()
        {
            return "OK";
        }


        public List<string> DebagABag(Cl_Variables clvar, DataTable man, DataTable cn, string arrivedAt, string departedTo, bool isFinal)
        {
            List<string> errorMessages = new List<string>();
            string debagID = "";
            string errorMessage = "";
            try
            {


                using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
                {
                    using (SqlCommand cmd = new SqlCommand("Bulk_DEBAG"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@BagNumber", clvar.BagNumber);
                        cmd.Parameters.AddWithValue("@origin", clvar.origin);
                        cmd.Parameters.AddWithValue("@destination", clvar.destination);
                        cmd.Parameters.AddWithValue("@description", clvar.productDescription);
                        cmd.Parameters.AddWithValue("@date", clvar.Bookingdate);
                        cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                        cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                        cmd.Parameters.AddWithValue("@ExpressCenterCode", HttpContext.Current.Session["ExpressCenter"].ToString());
                        cmd.Parameters.AddWithValue("@SealNo", clvar.SealNumber);
                        cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                        cmd.Parameters.AddWithValue("@IsFinal", isFinal);
                        if (isFinal == true)
                        {
                            cmd.Parameters.AddWithValue("@DepartedTo", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@DepartedTo", departedTo);
                        }
                        cmd.Parameters.AddWithValue("@TotalWeight", clvar.Weight);
                        cmd.Parameters.AddWithValue("@ArrivedAt", arrivedAt);
                        cmd.Parameters.AddWithValue("@tbl_manifests", man);
                        cmd.Parameters.AddWithValue("@tbl_cn", cn);
                        cmd.Parameters.Add("@DEBAG_ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@error_message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        debagID = cmd.Parameters["@DEBAG_ID"].Value.ToString();
                        if (debagID == "0")
                        {
                            debagID = cmd.Parameters["@error_message"].Value.ToString();
                            errorMessages.Add("0");
                            errorMessages.Add(debagID);
                        }
                        else
                        {
                            errorMessages.Add("1");
                            errorMessages.Add(debagID);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                debagID = "ERROR: " + ex.Message;
                errorMessages.Add("0");
                errorMessages.Add(ex.Message);

            }


            return errorMessages;
        }
        public List<string> RerouteABag(Cl_Variables clvar, string arrivedAt, string departedTo, string currentlocation)
        {
            List<string> errorMessages = new List<string>();
            string debagID = "";
            string errorMessage = "";
            try
            {


                using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
                {
                    using (SqlCommand cmd = new SqlCommand("RerouteBag"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@BagNumber", clvar.BagNumber);
                        cmd.Parameters.AddWithValue("@date", clvar.Bookingdate);
                        cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                        cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                        cmd.Parameters.AddWithValue("@ExpressCenterCode", HttpContext.Current.Session["ExpressCenter"].ToString());
                        cmd.Parameters.AddWithValue("@DepartedTo", departedTo);
                        cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                        cmd.Parameters.AddWithValue("@ArrivedAt", arrivedAt);
                        cmd.Parameters.Add("@TrackBagID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@error_message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        debagID = cmd.Parameters["@TrackBagID"].Value.ToString();
                        if (debagID == "0")
                        {
                            debagID = cmd.Parameters["@error_message"].Value.ToString();

                            errorMessages.Add("0");
                            errorMessages.Add(debagID);
                        }
                        else
                        {
                            errorMessages.Add("1");
                            errorMessages.Add(debagID);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                debagID = "ERROR: " + ex.Message;
                errorMessages.Add("1");
                errorMessages.Add(ex.Message);

            }

            return errorMessages;
        }
        public List<string> RerouteABagWithoutBag(Cl_Variables clvar, string arrivedAt, string departedTo, string currentlocation, DataTable man, DataTable cns)
        {
            string debagID = "";
            List<string> errorMessages = new List<string>();
            try
            {


                using (SqlConnection con = new SqlConnection(clvar.Strcon2()))
                {
                    using (SqlCommand cmd = new SqlCommand("Reroutebagwithoutbag"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@BagNumber", clvar.BagNumber);
                        cmd.Parameters.AddWithValue("@date", clvar.Bookingdate.ToString("yyyy-MM-dd HH:mm:ss.000"));
                        cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                        cmd.Parameters.AddWithValue("@zoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                        cmd.Parameters.AddWithValue("@ExpressCenterCode", HttpContext.Current.Session["ExpressCenter"].ToString());
                        cmd.Parameters.AddWithValue("@DepartedTo", departedTo);
                        cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["U_ID"].ToString());
                        cmd.Parameters.AddWithValue("@ArrivedAt", arrivedAt);
                        cmd.Parameters.AddWithValue("@ManifestNumbers", man);
                        cmd.Parameters.AddWithValue("@CnNumbers", cns);
                        cmd.Parameters.Add("@TrackBagID", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@error_message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        debagID = cmd.Parameters["@TrackBagID"].Value.ToString();
                        if (debagID == "0")
                        {
                            debagID = cmd.Parameters["@error_message"].Value.ToString();
                            errorMessages.Add("0");
                            errorMessages.Add(debagID);
                        }
                        else
                        {
                            errorMessages.Add("1");
                            errorMessages.Add(debagID);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                debagID = "ERROR: " + ex.Message;
                errorMessages.Add("0");
                errorMessages.Add(ex.Message);
            }

            return errorMessages;
        }

        public DataTable GetBagManifests(Cl_Variables clvar)
        {

            string sqlString = "select b.BagManifestId,\n" +
            "   \t   m.ManifestNo,\n" +
            "   \t   b.ManifestId,\n" +
            "   \t   m.OriginBranchId,\n" +
            "   \t   b1.name Origin,\n" +
            "   \t   b2.name Dest,\n" +
            "   \t   m.DestBranchId, '' Description\n" +
            "  from rvdbo.BagManifest b\n" +
            " inner join rvdbo.Manifest m\n" +
            "    on m.ManifestId = b.ManifestId\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = m.OriginBranchId\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = m.DestBranchId\n" +
            " where b.BagId = (select BagId from rvdbo.Bag  where BagNo = '" + clvar.BagNumber + "')";


            sqlString = "select bm.manifestNumber ManifestNo,\n" +
           "       b.origin          OriginID,\n" +
           "       org.name          origin,\n" +
           "       b.destination     destinationID,\n" +
           "       dest.name         Dest,\n" +
           "       dm.statusCode,\n" +
           "       dm.reason,\n" +
           "       l.AttributeDesc   StatusDesc\n" +
           "  from BagManifest bm\n" +
           " inner join Bag b\n" +
           "    on b.bagNumber = bm.bagNumber\n" +
           " inner join Branches org\n" +
           "    on org.branchCode = b.origin\n" +
           " inner join Branches dest\n" +
           "    on dest.branchCode = b.destination\n" +
           "  left outer join MnP_DebagManifest dm\n" +
           "    on dm.bagNumber = bm.bagNumber\n" +
           "   and dm.manifestNumber = bm.manifestNumber\n" +
           "  left outer join rvdbo.Lookup l\n" +
           "    on l.AttributeGroup = 'RECEIVING_STATUS'\n" +
           "   and l.Id = dm.statusCode\n" +
           " where bm.bagNumber = '" + clvar.BagNumber + "'";


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
        public DataTable GetBagOutpieces(Cl_Variables clvar)
        {

            string sqlString = "select ba.outpieceNumber,\n" +
            "       c.orgin           origin,\n" +
            "       b1.name           originName,\n" +
            "       c.destination,\n" +
            "       b2.name           destName,\n" +
            "       do.statusCode,\n" +
            "       do.reason,\n" +
            "       l.AttributeDesc   StatusDesc, CAST(c.weight as varchar) weight, CAST(c.Pieces as varchar) pieces\n" +
            "  from BagOutpieceAssociation ba\n" +
            " inner join consignment c\n" +
            "    on c.consignmentNumber = ba.outpieceNumber\n" +
            " inner join Branches b1\n" +
            "    on b1.branchCode = c.orgin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = c.destination\n" +
            "  left outer join MnP_Debag d\n" +
            "    on d.BagNumber = ba.bagNumber\n" +
            "  left outer join MnP_DebagOutPieces do\n" +
            "    on do.bagNumber = ba.bagNumber\n" +
            "   and do.outpieceNumber = ba.outpieceNumber\n" +
            "  left outer join rvdbo.Lookup l\n" +
            "    on l.Id = do.statusCode\n" +
            "   and l.AttributeGroup = 'RECEIVING_STATUS'\n" +
            " where ba.bagNumber = '" + clvar.BagNumber + "'";
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
        public DataTable GetExcessOutPieces(Cl_Variables clvar)
        {

            string sqlString = "select m.consignmentNumber outpieceNumber,\n" +
            "       m.orgin origin,\n" +
            "       org.name originName,\n" +
            "       m.destination destination,\n" +
            "       de.name destName,\n" +
            "       '7' statusCode,\n" +
            "       'EXCESS RECEIVED' StatusDesc,\n" +
            "       '' Reason, CAST(m.weight as varchar) weight, CAST(m.pieces as varchar) pieces\n" +
            "  from consignment m\n" +
            " inner join Branches org\n" +
            "    on org.branchCode = m.orgin\n" +
            " inner join Branches de\n" +
            "    on de.branchCode = m.destination\n" +
            "\n" +
            " where m.consignmentNumber = '" + clvar.consignmentNo + "'";
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
        public DataTable GetExcessManifest(Cl_Variables clvar)
        {

            string sqlString = "select m.manifestNumber ManifestNo,\n" +
            "       m.origin OriginID,\n" +
            "       org.name origin,\n" +
            "       m.destination destinationID,\n" +
            "       de.name dest,\n" +
            "       '7' statusCode,\n" +
            "       'EXCESS RECEIVED' StatusDesc,\n" +
            "       '' Reason\n" +
            "  from Mnp_Manifest m\n" +
            " inner join Branches org\n" +
            "    on org.branchCode = m.origin\n" +
            " inner join Branches de\n" +
            "    on de.branchCode = m.destination\n" +
            "\n" +
            " where m.manifestNumber = '" + clvar.manifestNo + "'";
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
        public DataTable GetBagDetail(Cl_Variables clvar)
        {

            string sqlString = "select b.bagNumber,\n" +
            "       b.origin, \n" +
            "       b1.name       OriginName,\n" +
            "       b.destination,\n" +
            "       b2.name       DestName,\n" +
            "       b.totalWeight,\n" +
            "       b.sealNo,\n" +
            "       b.date,\n" +
            "       b.description\n" +
            "  from Bag b\n" +
            " inner join Branches b1\n" +
            "    on b.origin = b1.branchCode\n" +
            "\n" +
            " inner join Branches b2\n" +
            "    on b.destination = b2.branchCode\n" +
            " where b.bagNumber = '" + clvar.BagNumber + "' --and b.destination = '" + HttpContext.Current.Session["BranchCode"].ToString() + "'";
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

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
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
        protected void btn_search_Click(object sender, EventArgs e)
        {
            string script_ = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            //string script = String.Format(script_, "RunsheetInvoice.aspx?Xcode=" + temp_, "_blank", "");
            string script = String.Format(script_, "Search_debag.aspx", "_blank", "");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Redirect", script, true);
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

        private static DataTable CheckConsignmentBooking(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = @"SELECT *, (select count(*) from tbl_CODControlBypass where isactive = 1 and consignmentNumber = '" + Consignment + @"') bypass FROM Consignment c 
            inner join (select consignmentnumber, sum(AtDest) as AtDest, sum(allowRTN) as allowRTN from (
            select '" + Consignment + @"' consignmentnumber, 0 AtDest, 0 allowRTN union
            select consignmentnumber, case when Reason in ('70', '58') then 0 else 1 end AtDest, 0 as allowRTN from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' 
            and createdOn = (select max(createdon) from RunsheetConsignment where consignmentNumber = '" + Consignment + @"' ) union
            select consignmentnumber, 0 as AtDest, case when COUNT(*) > 0 then 1 else 0 end as allowRTN 
            from MNP_NCI_Request where calltrack = 2 and consignmentnumber ='" + Consignment + @"' group by consignmentnumber) xb
            group by consignmentnumber) xxb on xxb.consignmentNumber = c.consignmentNumber
            WHERE c.consignmentNumber = '" + Consignment + "'";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }



        private static DataTable CheckFirstProcessOrigin(string Consignment)
        {
            Cl_Variables clvar = new Cl_Variables();
            string query = " select * from consignment where consignmentNumber = '" + Consignment + "' and orgin = '" + HttpContext.Current.Session["BranchCode"].ToString() + "' ";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }
            catch (Exception ex)
            { }
            finally { con.Close(); }
            return dt;
        }

        public static string PrimaryCheck(string cn)
        {
            Cl_Variables clvar = new Cl_Variables();
            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                try
                {
                    var query = $"select 'Consignment already exist in archive database' AS Msg from primaryconsignments where isManual = 1 AND consignmentnumber = '{cn}'";
                    con.Open();
                    var rs = con.QueryFirstOrDefault<string>(query);
                    con.Close();
                    return rs;
                }
                catch (SqlException ex)
                {
                    con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
            }
        }

        public static string alreadyRTS_DLV(string cn)
        {
            Cl_Variables clvar = new Cl_Variables();
            using (SqlConnection con = new SqlConnection(clvar.Strcon()))
            {
                try
                {
                    var query = $"select top(1) 'Consignment already Marked Delivered or Returned' AS Msg from Mnp_ConsignmentOperations where ConsignmentId = '{cn}' and (IsDelivered = 1 or IsReturned = 1) ";
                    con.Open();
                    var rs = con.QueryFirstOrDefault<string>(query);
                    con.Close();
                    return rs;
                }
                catch (SqlException ex)
                {
                    con.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
            }
        }
    }
}