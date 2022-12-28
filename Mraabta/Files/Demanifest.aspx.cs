using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRaabta.App_Code;
using Dapper;


namespace MRaabta.Files
{
    public partial class Demanifest : System.Web.UI.Page
    {
        Cl_Variables clvar = new Cl_Variables();
        Consignemnts con = new Consignemnts();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCNLengths();
            }
            DataTable cnDetail = ViewState["cnDetails"] as DataTable;

            if (cnDetail != null)
            {
                foreach (GridViewRow row in gv_consignments.Rows)
                {
                    DataRow dr = cnDetail.Select("ConsignmentNumber = '" + row.Cells[1].Text + "'").FirstOrDefault();
                    dr["Remarks"] = (row.FindControl("txt_reason") as TextBox).Text;
                }

                ViewState["cnDetails"] = cnDetail;
            }
        }

        protected void txt_cnNumber_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumeric(txt_cnNumber.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Numerics allowed in Consignment Numbers')", true);
                lbl_error.Text = lbl_error2.Text = "Only Numerics allowed in Consignment Numbers";
                txt_cnNumber.Text = "";
                return;
            }
            if (txt_cnNumber.Text.Length > 15 || txt_cnNumber.Text.Length < 11)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment Number must be between 11 and 15 digits')", true);
                lbl_error.Text = lbl_error2.Text = "Consignment Number must be between 11 and 15 digits";
                txt_cnNumber.Text = "";
                return;
            }


            #region Primary Check By Fahad 12-oct-2020
            var rs = PrimaryCheck(txt_cnNumber.Text);
            if (!string.IsNullOrEmpty(rs))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('"+ rs + "')", true);
                lbl_error.Text = lbl_error2.Text = "Consignment Number must be between 11 and 15 digits";
                txt_cnNumber.Text = "";
                txt_cnNumber.Focus();
                return;
            }
            #endregion

            #region RTS/DLV Check by Talha 23-oct-2020
            var rss = alreadyRTS_DLV(txt_cnNumber.Text);
            if (!string.IsNullOrEmpty(rss))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + rss + "')", true);
                txt_cnNumber.Text = "";
                txt_cnNumber.Focus();
                return;
            }
            #endregion

            if (txt_cnNumber.Text.StartsWith("5") && txt_cnNumber.Text.Length == 15)
            {
                DataTable BookingDT = CheckConsignmentBooking(txt_cnNumber.Text);
                DataTable FirstProcessDT = CheckFirstProcessOrigin(txt_cnNumber.Text);
                string status = "true";
                if (BookingDT.Rows.Count > 0)
                {
                    //if (HttpContext.Current.Session["BranchCode"].ToString() != BookingDT.Rows[0]["destination"].ToString() && BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                    if (BookingDT.Rows[0]["AtDest"].ToString() == "1" && BookingDT.Rows[0]["allowRTN"].ToString() == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Once reached destination can only move with Return NCI')", true);
                        txt_cnNumber.Text = "";
                        txt_cnNumber.Focus();
                        return;
                    }
                    else if (FirstProcessDT.Rows.Count > 0 || BookingDT.Rows[0]["isApproved"].ToString() == "True")
                    {
                        if (BookingDT.Rows[0]["status"].ToString() == "9")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Consignment is Void perform Arrival')", true);
                            txt_cnNumber.Text = "";
                            txt_cnNumber.Focus();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('First Process Must be at orign')", true);
                        txt_cnNumber.Text = "";
                        txt_cnNumber.Focus();
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No booking found for this COD CN')", true);
                    txt_cnNumber.Text = "";
                    txt_cnNumber.Focus();
                    return;
                }
            }




            bool flag = true;

            DataTable cnDetail = ViewState["cnDetails"] as DataTable;
            if (cnDetail != null)
            {
                DataRow dr_ = cnDetail.Select("ConsignmentNumber = '" + txt_cnNumber.Text.Trim() + "'").FirstOrDefault();
                if (dr_ != null)
                {
                    dr_["DemanifestStateID"] = "5";
                    cnDetail.AcceptChanges();
                    ViewState["cnDetails"] = cnDetail;

                    gv_consignments.DataSource = cnDetail;
                    gv_consignments.DataBind();

                    txt_cnNumber.Text = "";
                    txt_cnNumber.Focus();
                    return;
                }

                #region MyRegion
                //foreach (GridViewRow row in gv_consignments.Rows)
                //{
                //    //if (row.Cells[0].Enabled == false)
                //    //{
                //    //    return;
                //    //}
                //    if (row.Cells[1].Text == txt_cnNumber.Text)
                //    {
                //        ((CheckBox)row.FindControl("chk_received")).Checked = true;
                //        row.Cells[2].Text = "Received";
                //        txt_cnNumber.Text = "";
                //        txt_cnNumber.Focus();
                //        return;
                //    }
                //    else
                //    {
                //        flag = false;
                //    }
                //} 
                #endregion
                else//(!flag)
                {
                    clvar.consignmentNo = txt_cnNumber.Text;
                    DataTable dt = GetConsignmentDetail_(clvar);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = cnDetail.NewRow();

                            dr["ManifestID"] = cnDetail.Rows[0]["ManifestID"].ToString();
                            dr["ManifestNo"] = cnDetail.Rows[0]["ManifestNo"].ToString();
                            dr["ConsignmentNumber"] = txt_cnNumber.Text.Trim();
                            dr["DemanifestStateID"] = "7";
                            dr["Remarks"] = "";
                            dr["Orgin"] = dt.Rows[0]["Orgin"].ToString();
                            dr["Dest"] = dt.Rows[0]["DestBranch"].ToString();
                            dr["Origin"] = dt.Rows[0]["Origin"].ToString();
                            dr["Destination"] = dt.Rows[0]["Destination"].ToString();
                            dr["ConsignmentTypeid"] = dt.Rows[0]["ConsignmentTypeID"].ToString();
                            dr["ConType"] = dt.Rows[0]["ConType"].ToString();
                            dr["serviceTypeName"] = dt.Rows[0]["ServiceTypeName"].ToString();
                            dr["weight"] = dt.Rows[0]["Weight"].ToString();

                            cnDetail.Rows.Add(dr);

                            cnDetail.AcceptChanges();

                            gv_consignments.DataSource = cnDetail;
                            gv_consignments.DataBind();
                            ViewState["cnDetails"] = cnDetail;
                            txt_cnNumber.Text = "";
                            txt_cnNumber.Focus();

                        }
                        else
                        {
                            DataTable cnLength = ViewState["cnLengths"] as DataTable;
                            bool prefixFound = false;
                            if (cnLength != null)
                            {
                                if (cnLength.Rows.Count > 0)
                                {
                                    foreach (DataRow d in cnLength.Rows)
                                    {
                                        if (d[1].ToString().Length > txt_cnNumber.Text.Length)
                                        {
                                            continue;
                                        }
                                        if (d[1].ToString() == txt_cnNumber.Text.Substring(0, d[1].ToString().Length))
                                        {
                                            if (d[3].ToString() == txt_cnNumber.Text.Length.ToString())
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
                                err_msg.Text = "Invalid Consignment Lengnth or Prefix.";
                                txt_cnNumber.Text = "";
                                txt_cnNumber.Focus();
                                //lbl_error.Text = "Invalid Consignment Lengnth or Prefix.";
                                //lbl_error2.Text = "Invalid Consignment Lengnth or Prefix.";
                                return;
                            }
                            DataRow dr = cnDetail.NewRow();

                            dr["ManifestID"] = cnDetail.Rows[0]["ManifestID"].ToString();
                            dr["ManifestNo"] = cnDetail.Rows[0]["ManifestNo"].ToString();
                            dr["ConsignmentNumber"] = txt_cnNumber.Text.Trim();
                            dr["DemanifestStateID"] = "777";
                            dr["Remarks"] = "";
                            dr["Orgin"] = hd_origin.Value;// HttpContext.Current.Session["BranchCode"].ToString();// dt.Rows[0]["Orgin"].ToString();
                            dr["Dest"] = txt_destination.Text;// dt.Rows[0]["DestBranch"].ToString();
                            dr["Origin"] = txt_origin.Text;// dt.Rows[0]["Origin"].ToString();
                            dr["Destination"] = hd_destination.Value;// HttpContext.Current.Session["Branchcode"].ToString();// dt.Rows[0]["Destination"].ToString();
                            dr["ConsignmentTypeid"] = "12";// dt.Rows[0]["ConsignmentTypeID"].ToString();
                            dr["ConType"] = "NORMAL";// dt.Rows[0]["ConType"].ToString();
                            dr["serviceTypeName"] = "overnight"; //dt.Rows[0]["ServiceTypeName"].ToString();
                            dr["weight"] = "0.5"; //dt.Rows[0]["Weight"].ToString();

                            cnDetail.Rows.Add(dr);

                            cnDetail.AcceptChanges();

                            gv_consignments.DataSource = cnDetail;
                            gv_consignments.DataBind();
                            ViewState["cnDetails"] = cnDetail;
                            txt_cnNumber.Text = "";
                            txt_cnNumber.Focus();
                        }
                    }
                }
            }
        }
        protected void txt_manifestNumber_TextChanged(object sender, EventArgs e)
        {
            clvar.manifestNo = txt_manifestNumber.Text.Trim();
            DataTable dt = con.GetConsignmentsInManifests(clvar);
            DataTable header = GetManifestHeader(clvar);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["cnDetails"] = dt;
                    gv_consignments.DataSource = dt;
                    gv_consignments.DataBind();
                    bool demanifested = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["DemanifestStateID"].ToString().Trim() != "")
                        {
                            //demanifested = true;
                        }
                    }

                    if (demanifested)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Demanifested')", true);
                        btn_save.Enabled = false;
                    }
                    if (header.Rows.Count > 0)
                    {
                        txt_manifestNumber.Text = header.Rows[0]["manifestNumber"].ToString();
                        txt_destination.Text = header.Rows[0]["Destination"].ToString();
                        txt_date.Text = header.Rows[0]["date"].ToString();
                        txt_origin.Text = header.Rows[0]["Origin"].ToString();
                        hd_origin.Value = header.Rows[0]["OCODE"].ToString();
                        hd_destination.Value = header.Rows[0]["DCODE"].ToString();
                        if (header.Rows[0]["IsDemanifested"].ToString() == "1" || header.Rows[0]["IsDemanifested"].ToString().ToUpper() == "TRUE")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Manifest Already Demanifested')", true);
                            btn_save.Enabled = false;
                        }
                        else
                        {
                            btn_save.Enabled = true;
                        }
                    }
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invalid Manifest Number')", true);

                    div1.Style.Add("display", "block");
                    return;
                }
            }
            else
            {

            }


        }
        protected void chk_received_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;
            if (gr.Cells[0].Enabled != true)
            {
                return;
            }
            if (chk.Checked)
            {
                gr.Cells[2].Text = "Received";

            }
            else
            {
                gr.Cells[2].Text = "Short Received";
            }
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            txt_cnNumber.Text = "";
            txt_date.Text = "";
            txt_destination.Text = "";
            txt_manifestNumber.Text = "";
            txt_origin.Text = "";
            gv_consignments.DataSource = null;
            gv_consignments.DataBind();
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            bool flag = false;

            #region Manifest Wala Scene
            DataTable dt = ViewState["cnDetails"] as DataTable;
            foreach (GridViewRow row in gv_consignments.Rows)
            {
                if (!((row.FindControl("chk_received") as CheckBox).Checked))
                {
                    row.Cells[2].Text = "Short Received";
                    DataRow dr = dt.Select("ConsignmentNumber = '" + row.Cells[1].Text + "'", "").FirstOrDefault();
                    dr["DemanifestStateID"] = "6";
                    //(row.FindControl("txt_reason") as TextBox).Text = "Short Received";
                    flag = true;
                }
            }
            if (flag)
            {
                //divDialogue.Visible = true;
                divDialogue.Style.Add("display", "block");
            }
            else
            {
                btn_okDialogue_Click(sender, e);
            }

            #endregion

        }
        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void btn_print_Click(object sender, EventArgs e)
        {

        }
        protected void btn_cancelDialogue_Click(object sender, EventArgs e)
        {
            divDialogue.Style.Add("display", "none");
        }
        protected void btn_okDialogue_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
        new DataColumn("ConsignmentNumber", typeof(string)),
        new DataColumn("ManifestNumber", typeof(string)),
        new DataColumn("StatusCode", typeof(string)),
        new DataColumn("reason", typeof(string)),
        new DataColumn("DemanifestStateID", typeof(string)),
        new DataColumn("Remarks", typeof(string)),
        new DataColumn("Weight", typeof(float)),
        new DataColumn("Pieces", typeof(int))
        });
            DataTable newConsignments = new DataTable();
            newConsignments.Columns.Add(new DataColumn("ConsignmentNumber", typeof(string)));
            DataTable cnDetail = ViewState["cnDetails"] as DataTable;

            foreach (DataRow dr in cnDetail.Rows)
            {
                DataRow row = dt.NewRow();
                row["ConsignmentNumber"] = dr["ConsignmentNumber"].ToString();
                row["ManifestNumber"] = dr["ManifestNo"].ToString();
                row["StatusCode"] = dr["DemanifestStateID"].ToString();
                row["reason"] = dr["Remarks"].ToString();
                row["DemanifestStateID"] = dr["DemanifestStateID"].ToString();
                float weight = 0f;
                int pieces = 0;

                float.TryParse(dr["Weight"].ToString(), out weight);
                //int.TryParse(dr["Pieces"].ToString(), out pieces);
                if (weight <= 0)
                {
                    AlertMessage("Invalid Weight", "Red");
                    return;
                }
                //if (pieces <= 0)
                //{
                //    AlertMessage("Invalid Pieces", "Red");
                //    return;
                //}

                row["Weight"] = weight;
                row["Pieces"] = 1;

                if (dr["DemanifestStateID"].ToString() == "7" || dr["DemanifestStateID"].ToString() == "777")
                {
                    row["Remarks"] = "EXCESS RECEIVED";
                }
                else if (dr["DemanifestStateID"].ToString() == "5")
                {
                    row["Remarks"] = "RECEIVED";
                }
                else if (dr["DemanifestStateID"].ToString() == "6")
                {
                    row["Remarks"] = "SHORT RECEIVED";
                }
                //row["Remarks"] = dr["Remarks"].ToString();
                dt.Rows.Add(row);
                dt.AcceptChanges();
                if (dr["DemanifestStateID"].ToString() == "777")
                {
                    newConsignments.Rows.Add(dr["ConsignmentNumber"].ToString());
                }
            }

            //foreach (GridViewRow row in gv_consignments.Rows)
            //{
            //    DataRow dr = dt.NewRow();
            //    if (row.Cells[2].Text.ToUpper() == "SHORT RECEIVED" && ((TextBox)row.FindControl("txt_reason")).Text.Trim() == "")
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Provide Reason For Short Received Consignments')", true);
            //        return;
            //    }
            //    if (row.Cells[2].Text.ToUpper() == "SHORT RECEIVED")
            //    {
            //        clvar.ShortManifestRemarks.Add("WHEN '" + row.Cells[1].Text + "' THEN '" + (row.FindControl("txt_reason") as TextBox).Text + "'\n");
            //        clvar.ShortReceivedBags.Add(row.Cells[1].Text);
            //    }
            //    else
            //    {
            //        clvar.NormalBags.Add(row.Cells[1].Text);
            //    }
            //}





            clvar.manifestNo = txt_manifestNumber.Text;
            Tuple<int, string> resp = Demanifests(clvar, dt, newConsignments);

            //clvar.manifestId = int.Parse((gv_consignments.Rows[0].FindControl("hd_manifestID") as HiddenField).Value);

            if (resp.Item1 != 1)
            {
                AlertMessage("Demanifest Unsuccessful", "Red");
                return;
            }
            else
            {
                divDialogue.Style.Add("display", "none");
                AlertMessage("Demanifest Successful", "Green");
                btn_reset_Click(this, e);
                return;
            }


            //string errorMessage = con.Demanifest(clvar);

            //if (errorMessage.ToString() == "OK")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Demanifest Successfull')", true);

            //    return;
            //    btn_reset_Click(sender, e);
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Demanifest Un-Successfull')", true);
            //    return;
            //}

        }
        protected void gv_consignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text.Trim() != "")
                {
                    e.Row.Cells[0].Enabled = false;
                    if (e.Row.Cells[2].Text.Trim() == "5")
                    {
                        e.Row.Cells[2].Text = "Received";
                        (e.Row.FindControl("chk_received") as CheckBox).Checked = true;
                    }
                    else if (e.Row.Cells[2].Text.Trim() == "6")
                    {
                        e.Row.Cells[2].Text = "Short Received";
                    }
                    else if (e.Row.Cells[2].Text.Trim() == "7" || e.Row.Cells[2].Text.Trim() == "777")
                    {
                        e.Row.Cells[2].Text = "Excess Received";
                        (e.Row.FindControl("chk_received") as CheckBox).Checked = true;
                    }
                }
            }
        }
        protected void btn_cancel2Dialogue_Click(object sender, EventArgs e)
        {

        }
        protected void btn_ok2Dialogue_Click(object sender, EventArgs e)
        {

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
           "       b2.name               DestBranch,\n" +
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
           "       c.gst\n" +
           "  from Consignment c\n" +
           "--inner join Zones z\n" +
           "--on z.zoneCode = c.zoneCode\n" +
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

        public DataTable GetManifestHeader(Cl_Variables clvar)
        {

            string sqlString = "\n" +
            "select b3.name          Branch,\n" +
            "       m.date,\n" +
            "       m.manifestNumber,\n" +
            "       b.name           Origin,\n" +
            "       b2.name          Destination\n" +
            "  from MNP_Manifest m\n" +
            " inner join Branches b\n" +
            "    on b.branchCode = m.origin\n" +
            " inner join Branches b2\n" +
            "    on b2.branchCode = m.destination\n" +
            " inner join Branches b3\n" +
            "    on b3.branchCode = m.branchCode\n" +
            " where m.manifestNumber = '" + clvar.consignmentNo + "'";

            sqlString = "\n" +
           "select b3.name          Branch,\n" +
           "       m.date, m.origin OCODE, m.Destination DCODE,\n" +
           "       m.manifestNumber,\n" +
           "       b.name           Origin,\n" +
           "       b2.name          Destination, m.manifestType, m.date, m.isDemanifested\n" +
           "  from MNP_Manifest m\n" +
           " inner join Branches b\n" +
           "    on b.branchCode = m.origin\n" +
           " inner join Branches b2\n" +
           "    on b2.branchCode = m.destination\n" +
           " inner join Branches b3\n" +
           "    on b3.branchCode = m.branchCode\n" +
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
            //ViewState["cnLengths"] = dt;
            //cnControls.DataSource = dt;
            //cnControls.DataBind();
        }

        public void AlertMessage(string message, string color)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
            //lbl_error.Text = message;
            //lbl_error2.Text = message;
            err_msg.Text = message;
            err_msg.ForeColor = System.Drawing.Color.FromName(color);
            //lbl_error.ForeColor = System.Drawing.Color.FromName(color);
            //lbl_error2.ForeColor = System.Drawing.Color.FromName(color);

            return;
        }

        public Tuple<int, string> Demanifests(Cl_Variables clvar, DataTable dt, DataTable cn)
        {
            Tuple<int, string> resp = new Tuple<int, string>(0, "");

            SqlConnection con = new SqlConnection(clvar.Strcon());
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "MnP_Demanifest";
                cmd.Parameters.AddWithValue("@ManifestNo", clvar.manifestNo);
                cmd.Parameters.AddWithValue("@tblDetails", dt);
                cmd.Parameters.AddWithValue("@tblNewCN", cn);
                cmd.Parameters.AddWithValue("@ZoneCode", HttpContext.Current.Session["ZoneCode"].ToString());
                cmd.Parameters.AddWithValue("@BranchCode", HttpContext.Current.Session["BranchCode"].ToString());
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["U_ID"].ToString());
                cmd.Parameters.Add("@ResultStatus", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int ResultStatus = 0;

                object obj = cmd.Parameters["@ResultStatus"].SqlValue;
                int.TryParse(obj.ToString(), out ResultStatus);

                obj = new object();
                obj = cmd.Parameters["@Result"].SqlValue;

                resp = new Tuple<int, string>(ResultStatus, obj.ToString());
            }
            catch (Exception ex)
            { resp = new Tuple<int, string>(0, ex.Message); }
            finally { con.Close(); }




            return resp;
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